#!/bin/bash
# validate-round.sh — Validates a screenshot capture round's manifest and files.
# Usage: ./scripts/validate-round.sh docs/screenshots/round-XXXXXXXX
set -euo pipefail

ROUND_DIR="${1:?Usage: $0 <round-directory>}"
MANIFEST="${ROUND_DIR}/manifest.json"
ERRORS=0

fail() { echo "❌ $1"; ERRORS=$((ERRORS + 1)); }
warn() { echo "⚠️  $1"; }

# 1. Manifest exists
if [ ! -f "$MANIFEST" ]; then
  echo "❌ No manifest.json in ${ROUND_DIR}"
  exit 1
fi

CAPTURE_COUNT=$(jq '.captures | length' "$MANIFEST")
echo "📋 Validating round: $(jq -r '.round_id' "$MANIFEST") (${CAPTURE_COUNT} captures)"

# 2. All referenced files exist
jq -r '.captures[].filename' "$MANIFEST" | while read -r f; do
  [ -f "${ROUND_DIR}/${f}" ] || fail "Missing file: ${f}"
done

# 3. SHA-256 checksums match
jq -r '.captures[] | "\(.sha256)  \(.filename)"' "$MANIFEST" | while read -r hash file; do
  ACTUAL=$(shasum -a 256 "${ROUND_DIR}/${file}" | awk '{print $1}')
  if [ "$hash" != "$ACTUAL" ]; then
    fail "SHA-256 mismatch: ${file} (expected ${hash:0:12}... got ${ACTUAL:0:12}...)"
  fi
done

# 4. All three gates passed on every capture
UNGATED=$(jq '[.captures[] | select((.gates_passed | length) < 3)] | length' "$MANIFEST")
if [ "$UNGATED" -ne 0 ]; then
  fail "${UNGATED} capture(s) missing full gate verification (need app_identity + theme_fingerprint + route_verification)"
  jq -r '.captures[] | select((.gates_passed | length) < 3) | "  - \(.filename): gates=\(.gates_passed)"' "$MANIFEST"
fi

# 5. All bundle_id, theme, and page verified flags are true
UNVERIFIED=$(jq '[.captures[] | select(.bundle_id_verified != true or .theme_verified != true or .page_verified != true)] | length' "$MANIFEST")
if [ "$UNVERIFIED" -ne 0 ]; then
  fail "${UNVERIFIED} capture(s) have unverified identity/theme/page"
fi

# 6. Duplicate SHA-256 detection (same image content with different names = likely mix-up)
DUPES=$(jq -r '.captures[].sha256' "$MANIFEST" | sort | uniq -d)
if [ -n "$DUPES" ]; then
  fail "Duplicate SHA-256 values found — possible app/theme mix-up:"
  echo "$DUPES" | while read -r dup_hash; do
    jq -r --arg h "$dup_hash" '.captures[] | select(.sha256 == $h) | "  - \(.filename) (\(.app), \(.theme), \(.page))"' "$MANIFEST"
  done
fi

# 7. Filename convention check: {app}-{theme}-{page}.png
jq -r '.captures[].filename' "$MANIFEST" | while read -r f; do
  if ! echo "$f" | grep -qE '^(maui|blazor|reactor)-(default|darkly|slate|flatly|sketchy|vapor|brite)-(controls|inputs|typography|cards|themes|forms|variants|spacing)\.png$'; then
    warn "Non-standard filename: ${f}"
  fi
done

# 8. Dimension consistency (all screenshots should be same size)
UNIQUE_DIMS=$(jq -r '.captures[].dimensions' "$MANIFEST" | sort -u | wc -l | tr -d ' ')
if [ "$UNIQUE_DIMS" -gt 1 ]; then
  warn "Multiple screenshot dimensions detected (${UNIQUE_DIMS} unique sizes) — inconsistent window sizes?"
  jq -r '.captures[].dimensions' "$MANIFEST" | sort | uniq -c | sort -rn | head -5
fi

# 9. Cross-reference: no files in directory without manifest entry
for png in "${ROUND_DIR}"/*.png; do
  [ -f "$png" ] || continue
  BASENAME=$(basename "$png")
  if ! jq -e --arg f "$BASENAME" '.captures[] | select(.filename == $f)' "$MANIFEST" > /dev/null 2>&1; then
    warn "Untracked file: ${BASENAME} (in directory but not in manifest)"
  fi
done

if [ "$ERRORS" -gt 0 ]; then
  echo ""
  echo "❌ VALIDATION FAILED: ${ERRORS} error(s) found"
  exit 1
else
  echo "✅ Round validation passed: ${CAPTURE_COUNT} captures verified"
  exit 0
fi
