#!/bin/bash
# compare-round.sh — Generates valid comparison pairs from a verified round.
# Refuses to compare unverified screenshots.
# Usage: ./scripts/compare-round.sh docs/screenshots/round-XXXXXXXX
set -euo pipefail

ROUND_DIR="${1:?Usage: $0 <round-directory>}"
MANIFEST="${ROUND_DIR}/manifest.json"
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

# Gate: validate the round first
echo "🔒 Running pre-comparison validation..."
"${SCRIPT_DIR}/validate-round.sh" "$ROUND_DIR" || {
  echo "❌ Cannot compare: round validation failed"
  exit 1
}

echo ""
echo "📊 Generating valid comparison pairs..."
echo "   (same theme + same page, different app, all gates passed)"
echo ""

# Generate valid comparison pairs
PAIRS=$(jq -r '
  [.captures[] | select(.bundle_id_verified and .theme_verified and .page_verified)]
  | group_by("\(.theme)-\(.page)")
  | map(select(length > 1))
  | .[]
  | [combinations]
  | map(select(length == 2))
  | .[]
  | select(.[0].app != .[1].app)
  | "  \(.[0].filename)  ↔  \(.[1].filename)  [\(.[0].theme)/\(.[0].page)]"
' "$MANIFEST" 2>/dev/null || echo "")

if [ -z "$PAIRS" ]; then
  echo "⚠️  No valid comparison pairs found."
  echo "   Need at least 2 different apps with the same theme+page combination."
  exit 0
fi

PAIR_COUNT=$(echo "$PAIRS" | wc -l | tr -d ' ')
echo "Found ${PAIR_COUNT} valid comparison pair(s):"
echo ""
echo "$PAIRS"
echo ""
echo "✅ All pairs are manifest-verified and gate-checked."
