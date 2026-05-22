<#
.SYNOPSIS
  Build and run Benchmarks/Cool.Benchmarks for x86 and x64.

.DESCRIPTION
  This script builds the specified Benchmarks project for both x86 and x64
  (into separate output folders) and runs each produced executable. It does
  not modify source code.

.PARAMETER Project
  Path to the csproj to build. Default: Benchmarks\Cool.Benchmarks.csproj

.PARAMETER Framework
  Target framework to build (e.g. net48). Default: net48

.PARAMETER Configuration
  Build configuration. Default: Release

.PARAMETER OutBase
  Base output folder (relative to repo root). Default: Benchmarks\bin\Release

.PARAMETER Filter
  BenchmarkDotNet filter passed as `--filter` to the benchmark exe.

.PARAMETER NoBuild
  If set, skip the build step and only run existing exes.

.PARAMETER Parallel
  If set, start both runs in parallel (background processes).

.EXAMPLE
  .\tools\run-bench-arch.ps1 -Framework net48 -Filter "*AnsiBenchmarks.*RGB*"
#>

param(
    [string]$Project = "Benchmarks\Cool.Benchmarks.csproj",
    [string]$Framework = "net48",
    [string]$Configuration = "Release",
    [string]$OutBase = "Benchmarks\bin\Release",
    [string]$Filter = "*",
    [string]$ResultsBase = "Benchmarks\AggregatedResults",
    [switch]$NoBuild,
    [switch]$Parallel,
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$RemainingArgs
)

function Resolve-OutDir([string]$outBase, [string]$framework, [string]$arch) {
    return Join-Path -Path (Get-Location).Path -ChildPath (Join-Path -Path $outBase -ChildPath "$framework-$arch")
}

# validate CLI
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Error "dotnet CLI not found in PATH."
    exit 2
}

$exeName = [System.IO.Path]::GetFileNameWithoutExtension($Project) + ".exe"
$archs = @("x86", "x64")
$exitCodes = @()

# prepare aggregated results base dir
$resultsBaseDir = Join-Path -Path (Get-Location).Path -ChildPath $ResultsBase
if (-not (Test-Path $resultsBaseDir)) { New-Item -ItemType Directory -Path $resultsBaseDir | Out-Null }

$running = @()          # for parallel runs: list of @{Arch,Proc,ArtifactsDir,OutDir}
$copiedMapping = @()    # list of @{Dir,Arch}

foreach ($arch in $archs) {
    $outDir = Resolve-OutDir -outBase $OutBase -framework $Framework -arch $arch

    if (-not $NoBuild) {
        Write-Host "`n===== Build $arch ($Framework) -> $outDir ====="
        # Build Benchmarks and its project references into the same OutDir so referenced DLLs match the requested arch
        dotnet build $Project -c $Configuration -f $Framework -p:Platform=$arch -p:PlatformTarget=$arch -p:OutDir=$outDir -o $outDir
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Build failed for $arch (exit $LASTEXITCODE)"
            exit $LASTEXITCODE
        }
    }

    $exePath = Join-Path -Path $outDir -ChildPath $exeName
    if (-not (Test-Path $exePath)) {
        Write-Error "Executable not found: $exePath"
        exit 3
    }

    # ensure a per-arch artifacts directory to avoid concurrent write/reuse issues
    $artifactsDir = Join-Path -Path $outDir -ChildPath "BenchmarkDotNet.Artifacts"
    if (-not (Test-Path $artifactsDir)) { New-Item -ItemType Directory -Path $artifactsDir | Out-Null }

    $runArgs = @()
    $runArgs += "--artifacts"; $runArgs += $artifactsDir
    if ($Filter -and $Filter -ne "*") { $runArgs += "--filter"; $runArgs += $Filter }
    if ($RemainingArgs) { $runArgs += $RemainingArgs }

    Write-Host "`n===== Run $arch ====="
    if ($Parallel) {
        $proc = Start-Process -FilePath $exePath -ArgumentList $runArgs -PassThru -NoNewWindow -WorkingDirectory $outDir
        Write-Host "Started PID: $($proc.Id)"
        $running += [pscustomobject]@{Arch = $arch; Proc = $proc; ArtifactsDir = $artifactsDir; OutDir = $outDir }
    }
    else {
        & $exePath @runArgs
        $exitCodes += $LASTEXITCODE
        if ($LASTEXITCODE -ne 0) { Write-Warning "Run failed for $arch (exit $LASTEXITCODE)" }

        # copy artifacts for this serial run to an aggregated, timestamped folder
        $timestamp = Get-Date -Format 'yyyyMMdd-HHmmss'
        $archDestDir = Join-Path -Path $resultsBaseDir -ChildPath "$Framework-$arch-$timestamp"
        if (-not (Test-Path $archDestDir)) { New-Item -ItemType Directory -Path $archDestDir | Out-Null }
        $sourceResults = Join-Path -Path $artifactsDir -ChildPath 'results'
        if (Test-Path $sourceResults) {
            Copy-Item -Path (Join-Path $sourceResults '*') -Destination $archDestDir -Recurse -Force
            $copiedMapping += [pscustomobject]@{Dir = $archDestDir; Arch = $arch }
            Write-Host "Copied artifacts to $archDestDir"
        }
    }
}

if ($Parallel -and $running.Count -gt 0) {
    Write-Host "`nWaiting for parallel runs to finish..."
    $codes = @()
    foreach ($r in $running) {
        $p = $r.Proc
        try { $p.WaitForExit() } catch { }
        $codes += $p.ExitCode

        # copy artifacts for this parallel run into aggregated folder
        $timestamp = Get-Date -Format 'yyyyMMdd-HHmmss'
        $archDestDir = Join-Path -Path $resultsBaseDir -ChildPath "$Framework-$($r.Arch)-$timestamp"
        if (-not (Test-Path $archDestDir)) { New-Item -ItemType Directory -Path $archDestDir | Out-Null }
        $sourceResults = Join-Path -Path $r.ArtifactsDir -ChildPath 'results'
        if (Test-Path $sourceResults) {
            Copy-Item -Path (Join-Path $sourceResults '*') -Destination $archDestDir -Recurse -Force
            $copiedMapping += [pscustomobject]@{Dir = $archDestDir; Arch = $r.Arch }
            Write-Host "Copied artifacts to $archDestDir"
        }
    }
    $exitCodes = $codes
}

Write-Host "`nAll runs finished. Exit codes: $($exitCodes -join ', ')"

# return non-zero if any run failed
if ($exitCodes -contains 0) { 
    # generate comparison markdown if we have at least two aggregated result folders
    if ($copiedMapping.Count -gt 1) {
        $timestamp = Get-Date -Format 'yyyyMMdd-HHmmss'
        $comparisonFile = Join-Path -Path $resultsBaseDir -ChildPath "comparison-$Framework-$timestamp.md"
        $reportGroups = @{}
        foreach ($cm in $copiedMapping) {
            $csvs = Get-ChildItem -Path (Join-Path $cm.Dir '*-report.csv') -File -ErrorAction SilentlyContinue
            foreach ($csv in $csvs) {
                $base = $csv.BaseName
                if (-not $reportGroups.ContainsKey($base)) { $reportGroups[$base] = @{} }
                $reportGroups[$base][$cm.Arch] = $csv.FullName
            }
        }

        $mdAll = @()
        foreach ($base in $reportGroups.Keys) {
            $mdAll += "## Report: $base`n"
            $archList = $reportGroups[$base].Keys
            $mdHeader = "| Method | " + ($archList | ForEach-Object { "$_ Mean | $_ Allocated" } ) -join ' | ' + " |"
            $mdSep = "|---|" + ($archList | ForEach-Object { "---: | ---" } ) -join '|' + "|"
            $mdAll += $mdHeader
            $mdAll += $mdSep

            # import CSV per arch
            $imported = @{}
            $methods = @{}
            foreach ($arch in $archList) {
                $imported[$arch] = @()
                try { $imported[$arch] = Import-Csv -Path $reportGroups[$base][$arch] -ErrorAction Stop } catch { $imported[$arch] = @() }
                foreach ($r in $imported[$arch]) { $methods[$r.Method] = $true }
            }

            foreach ($m in $methods.Keys | Sort-Object) {
                $cells = @()
                foreach ($arch in $archList) {
                    $row = $imported[$arch] | Where-Object { $_.Method -eq $m } | Select-Object -First 1
                    if ($null -ne $row) {
                        $cells += $row.Mean
                        $cells += $row.Allocated
                    }
                    else {
                        $cells += 'N/A'; $cells += 'N/A'
                    }
                }
                $mdAll += "| $m | " + ($cells -join ' | ') + " |"
            }
            $mdAll += "`n"
        }

        $mdAll -join "`n" | Out-File -FilePath $comparisonFile -Encoding utf8
        Write-Host "Comparison written to: $comparisonFile"
    }
    exit 0 
}
else { exit ($exitCodes[0]) }
