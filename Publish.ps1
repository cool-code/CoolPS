# Publish script for Cool module
param(
    [string]$ApiKey
)
# This script is intended to be run from the root directory of the CoolPS repository
# and will publish the module to the PowerShell Gallery.
# It reads the file list from Cool.psd1, copies only those files to a temporary directory,
# and then runs Publish-Module from that clean directory to ensure no extraneous files
# are included in the published package.

# 1. Define source and build directories
$SourceDir = $PSScriptRoot
$BuildDir = Join-Path $env:TEMP "CoolPublish_$(Get-Date -Format 'yyyyMMddHHmmss')"

# 2. Read the file list from Cool.psd1 to determine which files to include in the package
$manifest = Import-PowerShellDataFile (Join-Path $SourceDir "Cool.psd1")
$fileList = $manifest.FileList

# 3. Create a clean build directory
New-Item -Path $BuildDir -Type Directory -Force | Out-Null

# 4. Copy only the files listed in FileList
foreach ($file in $fileList) {
    $srcFile = Join-Path $SourceDir $file
    $destFile = Join-Path $BuildDir $file
    
    # Ensure the source file exists before copying
    $parentDir = Split-Path $destFile
    if (!(Test-Path $parentDir)) { New-Item $parentDir -Type Directory -Force | Out-Null }
    
    Copy-Item $srcFile $destFile -Force
}

# 5. From this clean directory, execute the publish command
Write-Host "[Cool] Publishing clean module package..." -ForegroundColor Cyan
Publish-PSResource -Path $BuildDir -ApiKey $ApiKey -Repository PSGallery -Verbose

# 6. Clean up the temporary build directory
Remove-Item $BuildDir -Recurse -Force