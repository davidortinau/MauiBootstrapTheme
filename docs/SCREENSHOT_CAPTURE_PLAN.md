# Screenshot Capture & Comparison Execution Plan

## Problem Statement

Previous screenshot comparison rounds suffered from **catastrophic invalid comparisons** caused by app/theme mix-ups during capture. Without identity or theme verification, screenshots were taken from the wrong app, with the wrong theme active, or on the wrong page — then compared as if they were valid. This invalidated entire comparison rounds and wasted significant effort.

### Root Causes Identified

1. **No app identity check** — Nothing verified *which* sample app (XAML, Blazor, Reactor) was actually running
2. **No theme fingerprint** — Nothing confirmed which Bootstrap theme was active before capture
3. **No route/page verification** — Screenshots could be of any page, not the intended one
4. **No metadata manifest** — Rounds had only image files with naming conventions, no machine-readable record of what was captured or under what conditions
5. **No hard stops** — Capture continued even when pre-conditions were obviously wrong

---

## Architecture Overview

```
┌──────────────────────────────────────────────────────────┐
│                   CAPTURE PIPELINE                       │
│                                                          │
│  ┌─────────┐   ┌──────────┐   ┌──────────┐   ┌───────┐ │
│  │ Pre-    │──▶│ Identity │──▶│ Capture  │──▶│ Post- │ │
│  │ flight  │   │ Gate     │   │ + Tag    │   │ Audit │ │
│  └─────────┘   └──────────┘   └──────────┘   └───────┘ │
│       │              │              │              │     │
│   Build &        App ID +       Screenshot +    Manifest│
│   Launch         Theme +        Metadata       Validate │
│   Verify         Route                                  │
└──────────────────────────────────────────────────────────┘
```

---

## The Three Sample Apps

| App | Project | Bundle ID | Prefix |
|-----|---------|-----------|--------|
| XAML (native MAUI) | `MauiBootstrapTheme.Sample` | `com.simplyprofound.bootstrapmaui` | `maui-` |
| Blazor Hybrid | `MauiBootstrapTheme.Sample.Blazor` | `com.simplyprofound.bootstrapblazor` | `blazor-` |
| MauiReactor | `MauiBootstrapTheme.Sample.Reactor` | `com.simplyprofound.bootstrapreactor` | `reactor-` |

## The Seven Themes

`default`, `darkly`, `slate`, `flatly`, `sketchy`, `vapor`, `brite`

## The Target Pages

`controls`, `inputs`, `typography`, `cards`, `themes`, `forms`, `variants`, `spacing`

---

## Phase 1: Pre-Flight Checks

Before any capture round begins, validate the environment.

### Step 1.1 — Clean Build

```bash
# Build all three sample apps from scratch
dotnet build samples/MauiBootstrapTheme.Sample -c Release -f net10.0-maccatalyst
dotnet build samples/MauiBootstrapTheme.Sample.Blazor -c Release -f net10.0-maccatalyst
dotnet build samples/MauiBootstrapTheme.Sample.Reactor -c Release -f net10.0-maccatalyst
```

**HARD STOP**: If any build fails, do not proceed. Fix build errors first.

### Step 1.2 — Create Round Directory

```bash
ROUND_NUM=$(date +%Y%m%d-%H%M%S)
ROUND_DIR="docs/screenshots/round-${ROUND_NUM}"
mkdir -p "${ROUND_DIR}"
```

### Step 1.3 — Initialize Manifest

Create `manifest.json` in the round directory before any screenshots are taken:

```json
{
  "round_id": "round-20250101-120000",
  "created_at": "2025-01-01T12:00:00Z",
  "operator": "copilot-agent",
  "platform": "maccatalyst-arm64",
  "dotnet_version": "10.0.100-preview.X",
  "captures": []
}
```

---

## Phase 2: Identity Gate (Per-App, Per-Screenshot)

Every single screenshot MUST pass these three gates before being accepted.

### Gate 1 — App Identity Check

**Purpose**: Confirm the correct app is running.

#### Method A: Process Verification (Preferred)

```bash
# Get the bundle ID of the frontmost app
APP_BUNDLE=$(mdls -name kMDItemCFBundleIdentifier -raw \
  "$(osascript -e 'tell application "System Events" to get name of first process whose frontmost is true')" \
  2>/dev/null)

# Or use lsappinfo
APP_BUNDLE=$(lsappinfo info -only bundleid $(lsappinfo front) | grep -o '"[^"]*"' | tr -d '"')
```

#### Method B: MauiDevFlow Identity Query

```bash
# Use maui-devflow to query the running app's identity
maui-devflow screenshot --app-info
```

#### Verification Logic

```
EXPECTED_BUNDLE = lookup(app_prefix)
  "maui-"    → "com.simplyprofound.bootstrapmaui"
  "blazor-"  → "com.simplyprofound.bootstrapblazor"
  "reactor-" → "com.simplyprofound.bootstrapreactor"

if ACTUAL_BUNDLE != EXPECTED_BUNDLE:
  ❌ HARD STOP — "Expected app ${EXPECTED_BUNDLE} but found ${ACTUAL_BUNDLE}"
  abort capture, do NOT save screenshot
```

### Gate 2 — Theme Fingerprint Check

**Purpose**: Confirm the intended Bootstrap theme is active.

#### Method A: UI Element Color Sampling

Each theme has a unique primary button color. Sample the primary button's color and compare:

| Theme | Primary Button Color (approx.) | Background |
|-------|-------------------------------|------------|
| default | `#0d6efd` (blue) | `#ffffff` (white) |
| darkly | `#375a7f` (dark blue) | `#222222` (dark) |
| slate | `#3a3f44` (charcoal) | `#272b30` (dark) |
| flatly | `#2c3e50` (navy) | `#ffffff` (white) |
| sketchy | `#333333` (near black) | `#ffffff` (white) |
| vapor | `#6f42c1` (purple) | `#1a1a2e` (very dark) |
| brite | `#ff6f00` (orange) | `#ffffff` (white) |

#### Method B: MauiDevFlow Element Inspection

```bash
# Inspect a known button element to read its background color
maui-devflow inspect --element "Primary Button" --property BackgroundColor
```

#### Method C: Window Title / Theme Label Check

If the Themes page is visible, read the currently selected theme label from the UI tree.

#### Verification Logic

```
EXPECTED_THEME = parse_theme_from_filename(target_filename)
ACTUAL_COLOR = sample_primary_button_color()

if !color_matches(ACTUAL_COLOR, THEME_FINGERPRINTS[EXPECTED_THEME], tolerance=15):
  ❌ HARD STOP — "Theme mismatch: expected ${EXPECTED_THEME} primary color
     ${THEME_FINGERPRINTS[EXPECTED_THEME]} but sampled ${ACTUAL_COLOR}"
  abort capture, do NOT save screenshot
```

### Gate 3 — Route / Page Verification

**Purpose**: Confirm we are on the correct page before capturing.

#### Method A: UI Tree Inspection

```bash
# Read the page title or navigation state
maui-devflow inspect --element "PageTitle" --property Text
# Or check the Shell's current route
maui-devflow inspect --element "Shell" --property CurrentRoute
```

#### Method B: Accessibility Snapshot Keyword Match

Take an accessibility snapshot and verify that expected control types are present:

| Page | Required Elements |
|------|------------------|
| controls | "Primary" button, ProgressBar, ActivityIndicator |
| inputs | Entry, Editor, DatePicker, Switch |
| typography | H1-H6 labels, Badge |
| cards | Multiple Border/Frame with shadow |
| themes | Theme picker buttons (Default, Darkly, etc.) |

#### Verification Logic

```
EXPECTED_PAGE = parse_page_from_target()
PAGE_ELEMENTS = get_accessibility_tree()

if !contains_required_elements(PAGE_ELEMENTS, PAGE_REQUIREMENTS[EXPECTED_PAGE]):
  ❌ HARD STOP — "Route mismatch: expected '${EXPECTED_PAGE}' page but
     required elements not found in UI tree"
  abort capture, do NOT save screenshot
```

---

## Phase 3: Capture + Tag

Only after all three gates pass does a screenshot get captured and recorded.

### Step 3.1 — Capture Screenshot

```bash
FILENAME="${APP_PREFIX}${THEME}-${PAGE}.png"
maui-devflow screenshot --output "${ROUND_DIR}/${FILENAME}"
```

### Step 3.2 — Compute File Fingerprint

```bash
SHA256=$(shasum -a 256 "${ROUND_DIR}/${FILENAME}" | awk '{print $1}')
FILE_SIZE=$(stat -f%z "${ROUND_DIR}/${FILENAME}")
DIMENSIONS=$(sips -g pixelWidth -g pixelHeight "${ROUND_DIR}/${FILENAME}" 2>/dev/null \
  | awk '/pixelWidth/{w=$2} /pixelHeight/{h=$2} END{print w"x"h}')
```

### Step 3.3 — Append to Manifest

```json
{
  "filename": "maui-default-controls.png",
  "app": "MauiBootstrapTheme.Sample",
  "bundle_id": "com.simplyprofound.bootstrapmaui",
  "bundle_id_verified": true,
  "theme": "default",
  "theme_verified": true,
  "theme_verification_method": "primary_button_color",
  "sampled_primary_color": "#0d6efd",
  "page": "controls",
  "page_verified": true,
  "page_verification_method": "accessibility_tree_keywords",
  "captured_at": "2025-01-01T12:05:32Z",
  "sha256": "abc123...",
  "dimensions": "1440x900",
  "file_size_bytes": 234567,
  "gates_passed": ["app_identity", "theme_fingerprint", "route_verification"]
}
```

---

## Phase 4: Post-Capture Audit

After all screenshots in a round are captured, run validation on the complete set.

### Check 4.1 — Completeness Matrix

Verify all expected combinations were captured:

```
Expected = {apps} × {themes} × {pages}
Missing  = Expected - Captured

if |Missing| > 0:
  ⚠️ WARNING — "${|Missing|} screenshots missing from this round"
  list missing combinations
```

### Check 4.2 — Duplicate Detection

```bash
# Find any duplicate images (same content, different names)
shasum -a 256 ${ROUND_DIR}/*.png | sort | uniq -d -w 64
```

**HARD STOP**: If duplicates are found, it strongly suggests a mix-up. Investigate before proceeding to comparison.

### Check 4.3 — Dimension Consistency

```bash
# All screenshots from the same platform should have identical dimensions
sips -g pixelWidth -g pixelHeight ${ROUND_DIR}/*.png | \
  awk '/pixelWidth/{w=$2} /pixelHeight/{h=$2; print w"x"h}' | sort -u
```

**WARNING**: If more than one unique dimension exists, screenshots may have been taken at different window sizes.

### Check 4.4 — Manifest Integrity

```
for each entry in manifest.captures:
  assert file_exists(entry.filename)
  assert sha256(entry.filename) == entry.sha256
  assert entry.bundle_id_verified == true
  assert entry.theme_verified == true
  assert entry.page_verified == true
  assert len(entry.gates_passed) == 3
```

**HARD STOP**: If any manifest entry fails integrity, the round is invalid.

---

## Phase 5: Comparison Rules

Only compare screenshots that have passed all gates and audits.

### Rule 5.1 — Same-Page, Same-Theme, Cross-App

```
VALID COMPARISON:
  maui-default-controls.png  vs  blazor-default-controls.png
  ✅ Same theme (default), same page (controls), different app

INVALID COMPARISON:
  maui-default-controls.png  vs  blazor-darkly-controls.png
  ❌ Different themes — NEVER compare cross-theme
```

### Rule 5.2 — Manifest-Gated Comparison

```python
def can_compare(a: ManifestEntry, b: ManifestEntry) -> bool:
    return (
        a.theme == b.theme
        and a.page == b.page
        and a.app != b.app
        and a.theme_verified and b.theme_verified
        and a.page_verified and b.page_verified
        and a.bundle_id_verified and b.bundle_id_verified
    )
```

### Rule 5.3 — Comparison Output

Each comparison produces a record:

```json
{
  "left": "maui-default-controls.png",
  "right": "blazor-default-controls.png",
  "left_verified": true,
  "right_verified": true,
  "comparison_valid": true,
  "differences_found": ["progress_bar_track", "button_height"],
  "pixel_diff_percentage": 12.3
}
```

---

## Step-by-Step Runbook

### Full Capture Round

```
1. PRE-FLIGHT
   1.1  git pull && dotnet restore
   1.2  Build all 3 sample apps (HARD STOP on failure)
   1.3  Create timestamped round directory
   1.4  Initialize manifest.json

2. CAPTURE BLAZOR (gold standard first)
   For each theme in [default, darkly, slate, flatly, sketchy, vapor, brite]:
     For each page in [controls, inputs, typography, cards, themes]:
       2.1  Launch/focus Blazor app
       2.2  ✅ Gate 1: Verify bundle ID = com.simplyprofound.bootstrapblazor
       2.3  Switch to target theme (via Themes page)
       2.4  ✅ Gate 2: Verify theme fingerprint (primary button color)
       2.5  Navigate to target page
       2.6  ✅ Gate 3: Verify page route (accessibility tree keywords)
       2.7  Wait 2s for rendering to settle
       2.8  Capture screenshot
       2.9  Compute SHA-256, dimensions
       2.10 Append to manifest
       2.11 ❌ HARD STOP if any gate failed

3. CAPTURE MAUI XAML
   (Same loop as step 2, with bundle ID = com.simplyprofound.bootstrapmaui)

4. CAPTURE REACTOR
   (Same loop as step 2, with bundle ID = com.simplyprofound.bootstrapreactor)

5. POST-CAPTURE AUDIT
   5.1  Run completeness matrix check
   5.2  Run duplicate detection (HARD STOP if found)
   5.3  Run dimension consistency check
   5.4  Validate manifest integrity (HARD STOP if invalid)

6. COMPARISON
   6.1  For each (theme, page) pair:
          Compare maui vs blazor (if both verified)
          Compare reactor vs blazor (if both verified)
   6.2  Generate comparison report with diff percentages
   6.3  Append comparison results to manifest

7. COMMIT
   7.1  git add round directory + manifest
   7.2  Commit with round ID in message
```

---

## Hard Stop Conditions Summary

| Condition | When | Severity |
|-----------|------|----------|
| Build failure | Phase 1 | 🔴 Abort entire round |
| App bundle ID mismatch | Phase 2, Gate 1 | 🔴 Abort current capture |
| Theme fingerprint mismatch | Phase 2, Gate 2 | 🔴 Abort current capture |
| Route/page verification fail | Phase 2, Gate 3 | 🔴 Abort current capture |
| Duplicate image SHA-256 | Phase 4, Check 2 | 🔴 Abort — investigate mix-up |
| Manifest integrity failure | Phase 4, Check 4 | 🔴 Round invalid |
| Cross-theme comparison attempt | Phase 5 | 🔴 Reject comparison |
| Unverified screenshot in comparison | Phase 5 | 🔴 Reject comparison |

---

## Suggested Automation Checks for This Repo

### 1. Manifest Validation Script

Add `scripts/validate-round.sh` to verify a completed round:

```bash
#!/bin/bash
# Usage: ./scripts/validate-round.sh docs/screenshots/round-XXXXXXXX
ROUND_DIR="$1"
MANIFEST="${ROUND_DIR}/manifest.json"

# Check manifest exists
[ -f "$MANIFEST" ] || { echo "❌ No manifest.json in ${ROUND_DIR}"; exit 1; }

# Check all referenced files exist
jq -r '.captures[].filename' "$MANIFEST" | while read f; do
  [ -f "${ROUND_DIR}/${f}" ] || { echo "❌ Missing file: ${f}"; exit 1; }
done

# Verify SHA-256 checksums
jq -r '.captures[] | "\(.sha256)  \(.filename)"' "$MANIFEST" | while read hash file; do
  ACTUAL=$(shasum -a 256 "${ROUND_DIR}/${file}" | awk '{print $1}')
  [ "$hash" = "$ACTUAL" ] || { echo "❌ SHA mismatch: ${file}"; exit 1; }
done

# Check all gates passed
UNGATED=$(jq '[.captures[] | select(.gates_passed | length < 3)] | length' "$MANIFEST")
[ "$UNGATED" -eq 0 ] || { echo "❌ ${UNGATED} captures missing gate verification"; exit 1; }

# Check for duplicates
DUPES=$(jq -r '.captures[].sha256' "$MANIFEST" | sort | uniq -d | wc -l | tr -d ' ')
[ "$DUPES" -eq 0 ] || { echo "⚠️ ${DUPES} duplicate SHA-256 values found"; exit 1; }

echo "✅ Round validation passed: $(jq '.captures | length' "$MANIFEST") captures verified"
```

### 2. CI Workflow Addition (`.github/workflows/ci.yml`)

Add a validation step that runs when screenshot rounds are committed:

```yaml
  validate-screenshots:
    runs-on: macos-latest
    if: contains(github.event.head_commit.message, 'screenshot') || 
        contains(join(github.event.commits.*.modified, ','), 'docs/screenshots/')
    steps:
      - uses: actions/checkout@v4
      - name: Validate screenshot manifests
        run: |
          for manifest in docs/screenshots/round-*/manifest.json; do
            echo "Validating $(dirname $manifest)..."
            ./scripts/validate-round.sh "$(dirname $manifest)"
          done
```

### 3. Pre-Comparison Gate Check

Add `scripts/compare-round.sh` that refuses to compare unverified screenshots:

```bash
#!/bin/bash
# Usage: ./scripts/compare-round.sh docs/screenshots/round-XXXXXXXX
ROUND_DIR="$1"
MANIFEST="${ROUND_DIR}/manifest.json"

# Refuse to compare if manifest validation fails
./scripts/validate-round.sh "$ROUND_DIR" || { echo "❌ Cannot compare invalid round"; exit 1; }

# Generate valid comparison pairs (same theme + page, different app)
jq -r '
  [.captures[] | select(.bundle_id_verified and .theme_verified and .page_verified)]
  | group_by(.theme + "-" + .page)
  | map(select(length > 1))
  | .[]
  | combinations(2)
  | select(.[0].app != .[1].app)
  | "\(.[0].filename) ↔ \(.[1].filename)"
' "$MANIFEST"
```

### 4. Agent Capture Checklist (for Copilot / AI agent use)

When an AI agent is performing screenshot capture, it MUST follow this checklist for every single screenshot. Embed this in agent instructions:

```
BEFORE EVERY SCREENSHOT:
□ Confirmed app bundle ID matches expected app
□ Confirmed theme via primary button color or theme label
□ Confirmed page via navigation state or element presence
□ Waited ≥2 seconds after navigation for rendering to settle

AFTER EVERY SCREENSHOT:
□ Computed SHA-256 and recorded in manifest
□ Recorded dimensions and file size
□ Verified filename matches {app}-{theme}-{page}.png convention

AFTER ALL SCREENSHOTS:
□ Ran duplicate detection — no duplicate SHA-256 values
□ Ran completeness check — all expected combinations present
□ Manifest integrity verified
```

---

## Naming Convention (Revised)

### Filenames

```
{app}-{theme}-{page}.png
```

| Segment | Values |
|---------|--------|
| `{app}` | `maui`, `blazor`, `reactor` |
| `{theme}` | `default`, `darkly`, `slate`, `flatly`, `sketchy`, `vapor`, `brite` |
| `{page}` | `controls`, `inputs`, `typography`, `cards`, `themes`, `forms`, `variants`, `spacing` |

**Examples**: `maui-default-controls.png`, `blazor-darkly-typography.png`, `reactor-vapor-cards.png`

### Round Directories

```
docs/screenshots/round-{YYYYMMDD-HHMMSS}/
├── manifest.json
├── maui-default-controls.png
├── maui-default-inputs.png
├── blazor-default-controls.png
├── blazor-default-inputs.png
├── ...
└── comparison-report.json  (generated after comparison)
```

---

## Migration from Legacy Rounds

Existing `round2/` through `round20/` directories lack manifests and verification. They should be treated as **unverified historical data**. Do not use them as comparison baselines without re-verification.

For future rounds, only rounds with valid `manifest.json` files where all captures have `gates_passed.length == 3` should be used for comparison.
