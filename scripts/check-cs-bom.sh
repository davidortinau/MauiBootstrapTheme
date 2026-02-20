#!/usr/bin/env bash
set -euo pipefail

ROOT="${1:-.}"

missing=0
while IFS= read -r -d '' file; do
  bom="$(xxd -p -l 3 "$file" 2>/dev/null || true)"
  if [[ "$bom" != "efbbbf" ]]; then
    echo "Missing UTF-8 BOM: $file"
    missing=1
  fi
done < <(find "$ROOT" \
  -type d \( -name obj -o -name bin -o -name .git \) -prune -o \
  -type f -name '*.cs' -print0)

if [[ "$missing" -ne 0 ]]; then
  echo "BOM check failed."
  exit 1
fi

echo "BOM check passed."
