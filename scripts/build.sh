#!/bin/bash

rootLocation=$(pwd)

echo_green() {
    echo -e "\033[0;32m$1\033[0m"
}

integrationTestProjects=$(find ./tests/IntegrationTests -name "*.csproj")
smokeTestingScripts=$(find ./tests/LoadTests/SmokeTesting -name "*.js")

echo_green "[MCIO] Restore"
dotnet restore

echo_green "[MCIO] Build Release"
dotnet build -c Release --no-restore

run_integration_tests() {
    local projects=("$@")

    # Instala a ferramenta SpecFlow LivingDoc, se ainda n�o estiver instalada
    echo_green "[MCIO] Install Specflow Living Doc"
    specflowLivingDocFolderPath="./.specflow/livingdoc"

    if [ ! -d "$specflowLivingDocFolderPath" ]; then
        mkdir -p "$specflowLivingDocFolderPath"
        dotnet tool update SpecFlow.Plus.LivingDoc.CLI --tool-path "$specflowLivingDocFolderPath"
    fi

    for project in "${projects[@]}"; do
    
        projectDir=$(dirname "$project")

        echo_green "[MCIO] Run Integration Tests for $project"
        dotnet test -c Release --no-build --verbosity normal --filter "Category=integration-test" "$project"
        
        echo_green "[MCIO] Generating Integration Tests Report for $project"
        ./.specflow/livingdoc/livingdoc test-assembly "$projectDir/bin/Release/net9.0/IntegrationTests.dll" -t "$projectDir/bin/Release/net9.0/TestExecution.json" --output "$projectDir/bin/Release/net9.0/LivingDoc.html"
    
    done
}

run_smoke_testing() {
    local files=("$@")

    for file in "${files[@]}"; do
        
        fileDir=$(dirname "$file")
        filename=$(basename -- "$file")
        filenameWithoutExtension="${filename%.*}"
        reportFilename="$fileDir/output/$filenameWithoutExtension.html"
        outputDir="$fileDir/output"

        echo_green "[MCIO] Smoke testing for file $file"

        echo_green "[MCIO] Deleting folder $outputDir"
        rm -rf "$outputDir"

        echo_green "[MCIO] Creating folder $outputDir"
        mkdir "$outputDir"

        cd "$outputDir" || exit
        pwd
        
        echo_green "[MCIO] Run smoke test from file $filename"
        k6 run "../$filename"

        cd "$rootLocation" || exit

    done
}

open_integration_test_reports() {
    local projects=("$@")

    for project in "${projects[@]}"; do

        projectDir=$(dirname "$project")

        echo_green "[MCIO] Open Integration Test Report for $project"
        if [ -f "$projectDir/bin/Release/net9.0/LivingDoc.html" ]; then
            if [[ "$OSTYPE" == "msys" ]]; then
                start "$projectDir/bin/Release/net9.0/LivingDoc.html"
            else
                xdg-open "$projectDir/bin/Release/net9.0/LivingDoc.html"
            fi
        else
            echo_green "[MCIO] Integration test not found for $project"
        fi
    done
}

open_k6_reports() {
    local files=("$@")

    for file in "${files[@]}"; do

        fileDir=$(dirname "$file")
        filename=$(basename -- "$file")
        filenameWithoutExtension="${filename%.*}"
        reportFilename="$fileDir/output/$filenameWithoutExtension.html"

        echo_green "[MCIO] Open K6 report $reportFilename"
        if [ -f "$reportFilename" ]; then
            if [[ "$OSTYPE" == "msys" ]]; then
                start "$reportFilename"
            else
                xdg-open "$reportFilename"
            fi
        else
            echo_green "[MCIO] K6 report not found for $reportFilename"
        fi
    done
}

run_integration_tests "${integrationTestProjects[@]}"
run_smoke_testing "${smokeTestingScripts[@]}"

open_integration_test_reports "${integrationTestProjects[@]}"
open_k6_reports "${smokeTestingScripts[@]}"