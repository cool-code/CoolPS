# Cool Profile Initialization Script
function script:Initialize-CoolProfile {
    # Get the path to the current user's PowerShell profile
    $path = $PROFILE.CurrentUserAllHosts
    $name = if ($path -match "WindowsPowerShell") { "WindowsPowerShell" } else { "PowerShell" }
    # 1. Ensure the parent directory of the profile file exists, if not, create it
    $parentDir = Split-Path $path
    if (-not [System.IO.Directory]::Exists($parentDir)) {
        New-Item -Path $parentDir -Type Directory -Force | Out-Null
    }
    # 2. Check if the profile file exists, if not, create an empty one
    if (-not [System.IO.File]::Exists($path)) {
        New-Item -Path $path -Type File -Force | Out-Null
        Write-Host (Get-LocalizedString 'CoolProfileCreated' $name) -ForegroundColor Cyan
    }

    # 3. Check if the profile already contains the import statement for Cool, if not, append it
    $content = [string][System.IO.File]::ReadAllText($path, [System.Text.Encoding]::UTF8)
    if ($content -notlike "*Import-Module Cool*" -or $content -match "#.*Import-Module Cool") {
        [System.IO.File]::AppendAllText($path, "`r`nImport-Module Cool`r`n", [System.Text.Encoding]::UTF8)
        $msg = Get-LocalizedString 'CoolProfileUpdated' $name
        Write-Host $msg -ForegroundColor Green
    }
    else {
        $msg = Get-LocalizedString 'CoolProfileAlreadyConfigured' $name
        Write-Host $msg -ForegroundColor Gray
    }
}

function script:Update-ColorsCache {
    Remove-Item $script:COLORS_CACHE -ErrorAction SilentlyContinue
    $env:LS_COLORS = Get-Colors
    $script:ColorsMemCache.IsInit = $false # Force reinitialize color cache
    Initialize-ColorsMemCache
    $msg = Get-LocalizedString 'LSColorsCacheUpdated'
    Write-Host $msg -ForegroundColor Green
}

function script:Update-IconsCache {
    Remove-Item $script:ICONS_CACHE -ErrorAction SilentlyContinue
    $env:LS_ICONS = Get-Icons
    $script:IconsMemCache.IsInit = $false # Force reinitialize icon cache
    Initialize-IconsMemCache
    $msg = Get-LocalizedString 'LSIconsCacheUpdated'
    Write-Host $msg -ForegroundColor Green
}

function global:cool {
    param (
        [string]$Command
    )
    if (-not $Command) {
        $msg = Get-LocalizedString 'CoolUsage'
        Write-Host $msg -ForegroundColor Cyan
        return
    }
    switch ($Command) {
        "init" {
            Initialize-CoolProfile
        }
        "update" {
            if ($args.Count -eq 0) {
                Update-ColorsCache
                Update-IconsCache
            }
            else {
                foreach ($cmd in $args) {
                    switch ($cmd) {
                        "colors" { Update-ColorsCache }
                        "icons" { Update-IconsCache }
                        default {
                            $msg = Get-LocalizedString 'UnknownCoolUpdateSubcommand' $cmd
                            Write-Host $msg -ForegroundColor Yellow
                        }
                    }
                }
            }
        }
        default {
            $msg = Get-LocalizedString 'UnknownCoolCommand' $Command
            Write-Host $msg -ForegroundColor Yellow
            cool
        }
    }
}