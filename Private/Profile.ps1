# Cool Profile Initialization Script
# This script checks if the current user's PowerShell profile is configured to import the Cool module.

function script:Initialize-CoolProfile {
    # Get the path to the current user's PowerShell profile
    $path = $PROFILE.CurrentUserAllHosts
    $name = if ($path -match "WindowsPowerShell") { "WindowsPowerShell" } else { "PowerShell" }
    # 1. Ensure the parent directory of the profile file exists, if not, create it
    $parentDir = Split-Path $path
    if (!(Test-Path $parentDir)) {
        New-Item -Path $parentDir -Type Directory -Force | Out-Null
    }
    # 2. Check if the profile file exists, if not, create an empty one
    if (!(Test-Path $path)) {
        New-Item -Path $path -Type File -Force | Out-Null
        Write-Host (Get-LocalizedString 'CoolProfileCreated' $name) -ForegroundColor Cyan
    }

    # 3. Check if the profile already contains the import statement for Cool, if not, append it
    $content = [string](Get-Content $path -Raw)
    if ($content -notlike "*Import-Module Cool*" -or $content -match "#.*Import-Module Cool") {
        Add-Content -Path $path -Value "`r`nImport-Module Cool" -Encoding UTF8
        $msg = Get-LocalizedString 'CoolProfileUpdated' $name
        Write-Host $msg -ForegroundColor Green
    }
    else {
        $msg = Get-LocalizedString 'CoolProfileAlreadyConfigured' $name
        Write-Host $msg -ForegroundColor Gray
    }
}
