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

function script:Get-Editor {
    $editorEnv = $env:EDITOR
    $rawEditor = ''
    $initialArgs = @()

    if ($editorEnv) {
        $parts = $editorEnv.Split(' ', [System.StringSplitOptions]::RemoveEmptyEntries)
        $rawEditor = $parts[0]
        if ($parts.Count -gt 1) { $initialArgs = $parts[1..($parts.Count - 1)] }
    }
    
    if (-not $rawEditor) {
        $searchList = @('code', 'notepad', 'vim', 'vi', 'nano')
        foreach ($cmd in $searchList) {
            if (Get-Command $cmd -ErrorAction SilentlyContinue) {
                $rawEditor = $cmd
                break
            }
        }
    }

    if (-not $rawEditor) { throw (Get-LocalizedString 'NoEditorFound') }

    return [PSCustomObject]@{
        Path = $rawEditor
        Args = $initialArgs
    }
}

function script:CoolEdit {
    param (
        [string]$FilePath
    )
    $editorObj = Get-Editor
    $targetFile = "`"$FilePath`""
    
    $finalArgs = $editorObj.Args + $targetFile
    
    $spawnArgs = @{
        FilePath     = $editorObj.Path
        ArgumentList = $finalArgs
    }

    $isWin = if ($null -ne $PSVersionTable.PSVersion.Major -and $PSVersionTable.PSVersion.Major -ge 6) { 
        $IsWindows 
    }
    else { 
        $env:OS -match 'Windows_NT' 
    }

    if ($isWin) {
        if ($editorObj.Path -match 'code|subl|atom') {
            $spawnArgs.WindowStyle = 'Hidden'
        }
    }

    Start-Process @spawnArgs
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
        "edit" {
            if ($args.Count -eq 1) {
                $cmd = $args[0]
                switch ($cmd) {
                    "colors" {
                        CoolEdit -FilePath $script:COLORS_SOURCE
                    }
                    "icons" {
                        CoolEdit -FilePath $script:ICONS_SOURCE
                    }
                    default {
                        $msg = Get-LocalizedString 'UnknownCoolEditSubcommand' $cmd
                        Write-Host $msg -ForegroundColor Yellow
                    }
                }
            }
            else {
                $msg = Get-LocalizedString 'CoolUsage'
                Write-Host $msg -ForegroundColor Cyan
            }
        }
        default {
            $msg = Get-LocalizedString 'UnknownCoolCommand' $Command
            Write-Host $msg -ForegroundColor Yellow
        }
    }
}