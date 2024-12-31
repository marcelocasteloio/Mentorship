#!/bin/bash

deletePaths=("bin" "obj" ".specflow" "output")

foldersToDelete=()
while IFS= read -r -d '' folder; do
    folderName=$(basename "$folder")
    for path in "${deletePaths[@]}"; do
        if [[ "$folderName" == "$path" ]]; then
            foldersToDelete+=("$folder")
        fi
    done
done < <(find . -type d -print0)

echo "[MCIO] Pastas a serem deletadas:"
for folder in "${foldersToDelete[@]}"; do
    echo "$folder"
done

for folder in "${foldersToDelete[@]}"; do
    echo "[MCIO] Deletando pasta: $folder"
    rm -rf "$folder"
done