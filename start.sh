#!/bin/bash

PROXY_TXT="config/user/global/proxy.txt"
CONFIG_INI="MinecraftClient.ini"

proxy_block=$(sed -n '/^\[Proxy\]/,/^\[/{p}' "$PROXY_TXT" | tail -n +2 | head -7)

if [ $(echo "$proxy_block" | wc -l) -lt 7 ]; then
  echo "Error"
  exit 1
fi

cp "$CONFIG_INI" "${CONFIG_INI}.bak"

awk -v repl="$proxy_block" '
  BEGIN { in_proxy=0; replaced=0 }
  /^\[Proxy\]/ {
    print $0
    in_proxy=1
    skip_lines=7
    next
  }
  in_proxy && skip_lines > 0 {
    if (replaced == 0) {
      print repl
      replaced=1
    }
    skip_lines--
    next
  }
  {
    print
  }
' "$CONFIG_INI" > "${CONFIG_INI}.tmp"

mv "${CONFIG_INI}.tmp" "$CONFIG_INI"

echo "Config has been updated"

while true; do
  sleep 5
  ./MinecraftClient
done
