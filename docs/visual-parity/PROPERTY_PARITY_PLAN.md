# Visual-Impact Property Parity Plan

Properties that drive visual appearance but are invisible to `maui-devflow property` today — Shadow details, Border.StrokeShape, brushes/gradients — are not covered by the current comparison workflow. This plan closes that gap.

---

## 1  Property Inventory Per Control

### Legend

| Column | Meaning |
|--------|---------|
| **DevFlow?** | Can `maui-devflow MAUI property <id> <Prop>` read it today? |
| **Handler-only** | Set via platform layer (iOS Layer / Android GradientDrawable / WinUI) with no cross-platform BindableProperty |

### 1.1 Button

| Property | Source | DevFlow? | Handler-only | Notes |
|----------|--------|----------|--------------|-------|
| BackgroundColor | Theme variant | ✅ | — | Readable |
| TextColor | Theme OnColor | ✅ | — | |
| CornerRadius | Theme CornerRadius{Sm,Lg,Pill} | ⚠️ Partial | iOS/Android only via layer | WinUI exposes it; iOS/Mac set layer.cornerRadius |
| BorderColor | Theme variant / outline | ❌ | ✅ | Set on GradientDrawable (Android), layer (iOS), BorderBrush (Win) |
| BorderWidth | Theme BorderWidth | ❌ | ✅ | Same as above |
| FontSize | Theme FontSize{Base,Sm,Lg} | ✅ | — | |
| Padding | Theme PaddingX/Y | ✅ | — | |
| MinimumHeightRequest | Theme MinHeight | ✅ | — | |
| Background (Gradient) | Slate theme: LinearGradientBrush | ❌ | ✅ | Brush type not exposed; handler sets native gradient drawable |
| Shadow | Not applied to Button | — | — | Vapor text-shadow accepted as not replicable |

### 1.2 Border

| Property | Source | DevFlow? | Handler-only | Notes |
|----------|--------|----------|--------------|-------|
| Stroke | Theme Outline / variant | ✅ | — | BindableProperty on Border |
| StrokeThickness | Theme BorderWidth | ✅ | — | |
| **StrokeShape** | RoundRectangle(CornerRadius) | **❌ Unknown** | — | BindableProperty exists but devflow may return type name only, not CornerRadius values |
| **Background** | SolidColorBrush | ✅ | — | Readable as color |
| **Shadow.Brush** | SolidColorBrush(Black.WithAlpha) | **❌ Nested** | — | Shadow is BindableProperty but `.Brush` is nested; devflow returns object reference |
| **Shadow.Offset** | Point(0, Y) | **❌ Nested** | — | Y = 2 / 8 / 16 per shadow level |
| **Shadow.Radius** | 4 / 16 / 48 per level | **❌ Nested** | — | |
| **Shadow.Opacity** | 0.075 / 0.15 / 0.175 | **❌ Nested** | — | Encoded in Brush alpha |

### 1.3 Entry

| Property | Source | DevFlow? | Handler-only | Notes |
|----------|--------|----------|--------------|-------|
| BackgroundColor | InputBackground | ✅ | — | |
| TextColor | InputText | ✅ | — | |
| PlaceholderColor | PlaceholderColor | ✅ | — | |
| FontSize | FontSize{Base,Sm,Lg} | ✅ | — | |
| **CornerRadius** | CornerRadius{Sm,Lg} | ❌ | ✅ | Entry has no CornerRadius property; applied per-platform |
| **BorderColor** | Variant: Danger/Success/Warning/Default | ❌ | ✅ | No BindableProperty on Entry |
| **BorderWidth** | 1px | ❌ | ✅ | |
| Padding | PaddingX/Y | ✅ | — | |
| MinimumHeightRequest | InputMinHeight | ✅ | — | |

### 1.4 Picker

| Property | Source | DevFlow? | Handler-only | Notes |
|----------|--------|----------|--------------|-------|
| BackgroundColor | InputBackground | ✅ | — | |
| TextColor | InputText | ✅ | — | |
| FontSize | FontSize{Base,Sm,Lg} | ✅ | — | |
| **CornerRadius** | CornerRadius{Sm,Lg} | ❌ | ✅ | No BindableProperty on Picker |
| **BorderColor** | Variant color | ❌ | ✅ | |
| **BorderWidth** | 1px | ❌ | ✅ | |
| Padding | PaddingX/Y | ✅ | — | |
| MinimumHeightRequest | InputMinHeight | ✅ | — | |

### 1.5 Label

| Property | Source | DevFlow? | Handler-only | Notes |
|----------|--------|----------|--------------|-------|
| TextColor | Variant / OnBackground | ✅ | — | |
| FontSize | FontSizeH1-H6, Lead, Small | ✅ | — | |
| FontAttributes | Bold (headings) | ✅ | — | |
| LineHeight | 1.2 / 1.5 | ⚠️ Partial | — | BindableProperty exists; devflow may not return it |
| Background (Badge) | SolidColorBrush(variant) | ✅ | — | |
| Padding (Badge) | 8,4 | ✅ | — | |
| Margin | Heading-dependent | ✅ | — | |
| FontFamily | Theme font (Sketchy: MarkerFelt-Wide) | ✅ | — | |

### Summary: Properties Invisible to Current Workflow

| Control | Invisible Properties | Risk |
|---------|---------------------|------|
| **Button** | BorderColor, BorderWidth, CornerRadius (iOS/Android), gradient Background | HIGH — outline variants and Slate gradients unverifiable |
| **Border** | Shadow.{Brush,Offset,Radius}, StrokeShape corner values | HIGH — shadow regression breaks card look, not caught |
| **Entry** | CornerRadius, BorderColor, BorderWidth | MEDIUM — validation-state borders (danger/success) invisible |
| **Picker** | CornerRadius, BorderColor, BorderWidth | MEDIUM — same as Entry |
| **Label** | LineHeight | LOW — subtle spacing only |

---

## 2  Extraction Approach When DevFlow Lacks Nested Properties

### 2.1 Strategy: Multi-Layer Verification

```
┌─────────────────────────────────────────────────────┐
│            PROPERTY VERIFICATION STACK               │
│                                                      │
│  Layer 1: maui-devflow property (flat props)         │
│  Layer 2: maui-devflow cdp snapshot (DOM/a11y tree)  │
│  Layer 3: Platform-native inspection (fallback)      │
│  Layer 4: Pixel-sampling validation (last resort)    │
└─────────────────────────────────────────────────────┘
```

### 2.2 Layer 1 — DevFlow Direct Property Read

```bash
# Readable flat properties — use for baseline
maui-devflow MAUI property <id> BackgroundColor
maui-devflow MAUI property <id> TextColor
maui-devflow MAUI property <id> FontSize
maui-devflow MAUI property <id> Stroke          # Border only
maui-devflow MAUI property <id> StrokeThickness  # Border only
```

### 2.3 Layer 2 — DevFlow Snapshot + JSON Parsing

Use `maui-devflow cdp snapshot` or `maui-devflow MAUI element <id>` and parse the JSON output for nested properties:

```bash
# Get full element JSON and extract Shadow sub-properties
maui-devflow MAUI element <id> | jq '.Shadow'
# Expected: { "Brush": "...", "Offset": "0,8", "Radius": 16 }

# Test whether StrokeShape reports CornerRadius
maui-devflow MAUI property <id> StrokeShape
# If it returns "Microsoft.Maui.Controls.Shapes.RoundRectangle" (type only),
# try reading the StrokeShape object's CornerRadius:
maui-devflow MAUI element <id> | jq '.StrokeShape.CornerRadius'
```

**If Layer 2 returns type-name-only for compound properties, that is the bug to report (§4).**

### 2.4 Layer 3 — Platform-Native Inspection (iOS/Mac Catalyst)

When devflow cannot introspect handler-only properties, fall back to Accessibility Inspector or `lldb` attach:

```bash
# Mac Catalyst: read native layer properties via lldb
xcrun lldb --attach-name "MauiBootstrapTheme.Sample" --one-line \
  'expr -l objc -- [(CALayer *)[[(UIView *)0xADDRESS layer] borderColor] description]'
```

This is expensive and fragile. Use only for one-time audits, not CI.

### 2.5 Layer 4 — Pixel-Sampling Validation

For properties that are truly opaque (gradients, shadow blur), sample pixels from a screenshot at known coordinates:

```bash
# Take screenshot, sample pixel at button border location
maui-devflow MAUI screenshot --output /tmp/capture.png

# Sample border-edge pixel (requires ImageMagick)
convert /tmp/capture.png -crop 1x1+120+45 txt:- | tail -1
# Expected output contains the border color hex value

# Sample shadow region below a card
convert /tmp/capture.png -crop 1x1+200+310 txt:- | tail -1
# Shadow pixel should be darker than background
```

**For gradient buttons (Slate theme):**
```bash
# Sample top and bottom of button — gradient should differ
TOP=$(convert /tmp/capture.png -crop 1x1+150+40 txt:- | tail -1)
BOT=$(convert /tmp/capture.png -crop 1x1+150+65 txt:- | tail -1)
# If TOP == BOT, gradient is not rendering
```

### 2.6 Practical Extraction Matrix

| Property | Primary Method | Fallback | Confidence |
|----------|---------------|----------|------------|
| Border.Shadow.Radius | `element <id>` JSON → `.Shadow.Radius` | Pixel blur halo width | Medium |
| Border.Shadow.Offset | `element <id>` JSON → `.Shadow.Offset` | Pixel: shadow centroid vs element centroid | Medium |
| Border.StrokeShape corners | `property <id> StrokeShape` → parse | Visual: screenshot rounded corner measurement | Low |
| Entry.BorderColor | Not available via devflow | Pixel sample at border edge | Low |
| Entry.CornerRadius | Not available via devflow | Pixel: corner arc measurement | Low |
| Button gradient | Not available via devflow | Pixel: sample top vs bottom of button | Medium |
| Button.BorderColor | Not available via devflow | Pixel sample at border edge | Low |

---

## 3  Validation Gates to Prevent Theme/Page Mismatch

The existing three-gate system (App Identity → Theme Fingerprint → Route/Page) is sound. Extend it with a **property-level gate** for high-value comparisons.

### 3.1 Gate 4 — Property Fingerprint (NEW)

After gates 1-3 pass and before recording comparison results, sample a set of "canary" properties to confirm the theme's visual state is actually applied:

```bash
# Canary properties per theme (fast, <2s total)
PRIMARY_BG=$(maui-devflow MAUI property <primary-btn-id> BackgroundColor)
CARD_STROKE=$(maui-devflow MAUI property <card-border-id> Stroke)
CARD_STROKE_W=$(maui-devflow MAUI property <card-border-id> StrokeThickness)
INPUT_BG=$(maui-devflow MAUI property <entry-id> BackgroundColor)

# Validate against expected theme values
assert_color "$PRIMARY_BG" "$EXPECTED_PRIMARY" 10   # tolerance ±10 per channel
assert_color "$CARD_STROKE" "$EXPECTED_OUTLINE" 10
assert_equals "$CARD_STROKE_W" "1"
```

### 3.2 Gate Integration in Manifest

Extend `manifest.json` capture entries:

```json
{
  "filename": "maui-default-controls.png",
  "gates_passed": ["app_identity", "theme_fingerprint", "route_verification", "property_fingerprint"],
  "property_fingerprint": {
    "primary_button_bg": "#0d6efd",
    "card_border_stroke": "#dee2e6",
    "card_border_thickness": "1",
    "entry_bg": "#ffffff"
  }
}
```

### 3.3 Cross-App Comparison Prerequisite

Before comparing MAUI vs Blazor screenshots, verify both screenshots were captured with matching property fingerprints for the same theme. Add to `compare-round.sh`:

```bash
# Reject pairs where property fingerprints diverge on theme-level values
MAUI_PRIMARY=$(jq -r '.property_fingerprint.primary_button_bg' <<< "$MAUI_ENTRY")
BLAZOR_PRIMARY=$(jq -r '.property_fingerprint.primary_button_bg' <<< "$BLAZOR_ENTRY")
if ! color_within_tolerance "$MAUI_PRIMARY" "$BLAZOR_PRIMARY" 15; then
  echo "❌ Theme mismatch: primary colors diverge ($MAUI_PRIMARY vs $BLAZOR_PRIMARY)"
  exit 1
fi
```

### 3.4 Dark-Mode Guard

Dark mode can silently activate between captures. Add an OS-theme check:

```bash
# Verify OS appearance hasn't changed mid-round
CURRENT_APPEARANCE=$(defaults read -g AppleInterfaceStyle 2>/dev/null || echo "Light")
if [ "$CURRENT_APPEARANCE" != "$ROUND_APPEARANCE" ]; then
  echo "❌ HARD STOP: OS appearance changed from $ROUND_APPEARANCE to $CURRENT_APPEARANCE"
  exit 1
fi
```

Record `ROUND_APPEARANCE` in the manifest at round start.

---

## 4  Bug-Report Criteria & Minimum Repro for maui-devflow

### 4.1 What Constitutes a Bug

File a `maui-devflow` issue when **all** of the following are true:

1. A BindableProperty exists on the cross-platform MAUI control (e.g., `Border.Shadow`, `Border.StrokeShape`)
2. The property is set programmatically (handler or XAML) and the value is non-default
3. `maui-devflow MAUI property <id> <PropertyName>` returns one of:
   - Empty / null
   - Type name only (e.g., `"Microsoft.Maui.Controls.Shapes.RoundRectangle"`) without nested values
   - Incorrect value
   - Error / exception
4. The property **is not** handler-only / platform-layer-only (those are "won't fix" by design)

### 4.2 Categorization

| Category | Example | Severity |
|----------|---------|----------|
| **Missing nested property introspection** | `Shadow` returns object ref, not `{Brush, Offset, Radius}` | P2 |
| **Missing BindableProperty read** | `StrokeShape` returns type name, not corner radius values | P2 |
| **Wrong value returned** | `BackgroundColor` returns white when handler set it to #0d6efd | P1 |
| **Crash on property read** | `property <id> Shadow` throws | P1 |
| **Handler-only property not readable** | `Entry.BorderColor` not exposed (by design) | Enhancement request, not bug |

### 4.3 Minimum Repro Template

```markdown
## maui-devflow: [Shadow/StrokeShape/Gradient] property not introspectable

### Environment
- maui-devflow version: `maui-devflow --version`
- .NET SDK: `dotnet --version`
- MAUI workload: `dotnet workload list | grep maui`
- Platform: macOS 15.x / Mac Catalyst

### Repro Steps

1. Create a .NET MAUI app with this XAML:

```xml
<Border x:Name="TestBorder"
        AutomationId="TestBorder"
        StrokeThickness="1"
        Stroke="#dee2e6"
        StrokeShape="{RoundRectangle CornerRadius=6}">
    <Border.Shadow>
        <Shadow Brush="Black" Offset="0,8" Radius="16" />
    </Border.Shadow>
    <Label Text="Card content" />
</Border>
```

2. Run the app on Mac Catalyst
3. Connect maui-devflow:

```bash
maui-devflow MAUI status   # confirm connected
```

4. Query the Border element:

```bash
maui-devflow MAUI query --automationId "TestBorder"
# Note the element ID, e.g., 42
```

5. Read Shadow property:

```bash
maui-devflow MAUI property 42 Shadow
```

### Expected Result

```json
{
  "Brush": { "Color": "#000000" },
  "Offset": { "X": 0, "Y": 8 },
  "Radius": 16
}
```

### Actual Result

```
Microsoft.Maui.Controls.Shadow
```
(Type name only, no property values)

### Impact

Cannot programmatically verify shadow rendering in automated visual-parity comparisons.
Theme comparison requires pixel sampling as a workaround, which is fragile and slow.
```

### 4.4 Properties to Test & File

Run this diagnostic sequence against a running sample app and file bugs for each failure:

```bash
#!/bin/bash
# diagnostic-devflow-props.sh — Test maui-devflow nested property support
set -euo pipefail

echo "=== Testing Border Shadow introspection ==="
BORDER_ID=$(maui-devflow MAUI query --automationId "CardBorder" | jq -r '.[0].id')
echo "Border ID: $BORDER_ID"

echo "--- Shadow (compound) ---"
maui-devflow MAUI property "$BORDER_ID" Shadow
echo "--- StrokeShape (compound) ---"
maui-devflow MAUI property "$BORDER_ID" StrokeShape
echo "--- Shadow.Radius (nested dot-path) ---"
maui-devflow MAUI property "$BORDER_ID" Shadow.Radius 2>/dev/null || echo "FAIL: dot-path not supported"
echo "--- Shadow.Offset (nested dot-path) ---"
maui-devflow MAUI property "$BORDER_ID" Shadow.Offset 2>/dev/null || echo "FAIL: dot-path not supported"

echo ""
echo "=== Testing Entry handler-only properties ==="
ENTRY_ID=$(maui-devflow MAUI query --type Entry | jq -r '.[0].id')
echo "Entry ID: $ENTRY_ID"

echo "--- BackgroundColor (should work) ---"
maui-devflow MAUI property "$ENTRY_ID" BackgroundColor
echo "--- (No BindableProperty expected for BorderColor, CornerRadius) ---"

echo ""
echo "=== Testing Button gradient background ==="
BTN_ID=$(maui-devflow MAUI query --automationId "PrimaryButton" | jq -r '.[0].id')
echo "Button ID: $BTN_ID"

echo "--- Background (brush type) ---"
maui-devflow MAUI property "$BTN_ID" Background
echo "--- BackgroundColor (solid fallback) ---"
maui-devflow MAUI property "$BTN_ID" BackgroundColor
```

---

## 5  Risk Mitigations

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|------------|
| maui-devflow never adds nested property support | Medium | HIGH — pixel sampling remains only option | File enhancement request early; build pixel-sampling into CI as permanent fallback |
| Pixel coordinates shift between OS/resolution | High | MEDIUM — pixel sampling breaks | Use relative coordinates: sample at `(element.X + element.Width/2, element.Y + element.Height - 2)` via element bounds from `maui-devflow MAUI element <id>` |
| Shadow values change between .NET versions | Low | LOW | Pin expected values in theme test data; update when SDK updates |
| Gradient rendering differs per platform | Medium | MEDIUM — Slate theme buttons | Test on all 3 platforms separately; document known platform differences |
| Gate 4 property fingerprint adds capture latency | Low | LOW — <2s overhead | Only sample 4 canary properties; parallelize reads |
| Entry/Picker border styling is entirely handler-only | Certain | MEDIUM | Accept as architectural limitation; verify via pixel sampling only for validation-state variants (danger/success) |

---

## 6  Implementation Priority

| Phase | Work | Prerequisite |
|-------|------|--------------|
| **Phase 1** (Now) | Run `diagnostic-devflow-props.sh` to establish current devflow capability baseline | Running sample app |
| **Phase 2** (Now) | File maui-devflow bugs for any BindableProperty that returns type-name-only | Phase 1 results |
| **Phase 3** (1 day) | Add Gate 4 property fingerprint to capture workflow; extend `manifest.json` schema | None |
| **Phase 4** (1 day) | Build pixel-sampling helpers for handler-only properties (Entry border, Button gradient) | ImageMagick available |
| **Phase 5** (Ongoing) | Add `ThemeFidelityTests.cs` assertions for shadow/gradient values from theme data | Theme data accessible in test project |
| **Phase 6** (When devflow ships fix) | Replace pixel-sampling with devflow nested reads; remove Layer 3-4 fallbacks | maui-devflow update |
