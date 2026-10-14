#!/usr/bin/env bash
# Dùng: ./doi-mssv.sh BIT240123
set -e
OLD="BIT240009"
NEW="$1"
[ -z "$NEW" ] && { echo "Usage: $0 <MSSV>"; exit 1; }

grep -rl "$OLD" . --include=*.cs --include=*.cshtml --include=*.json --include=*.csproj \
  --exclude-dir=bin --exclude-dir=obj | xargs -r sed -i "s/$OLD/$NEW/g"

find . -type f -name "*$OLD*" -not -path "*/bin/*" -not -path "*/obj/*" | while read -r f; do
  mv "$f" "$(dirname "$f")/$(basename "$f" | sed "s/$OLD/$NEW/g")"
done
echo "Da doi MSSV thanh $NEW. Database moi: ${NEW}_MID"
