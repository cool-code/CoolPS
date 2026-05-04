# Test Visual Width by using ANSI cursor position report
function Test-VisualWidth {
    param([string]$Text)
    $oldPos = $Host.UI.RawUI.CursorPosition
    Write-Host -NoNewline "$([char]27)[8m$Text$([char]27)[0m"
    $newPos = $Host.UI.RawUI.CursorPosition
    
    Write-Host -NoNewline "$([char]27)[G$([char]27)[K" 
    return $newPos.X - $oldPos.X
}

function Test-AmbiguousAsWide {
    if ([Console]::IsOutputRedirected) {
        # Detection not possible when output is redirected, return null to indicate unknown
        return $null
    }
    # Simple logic:
    # print an ambiguous width character (e.g. 'α')
    # and check how many cells it occupies
    $char = [char]0x03B1 # Greek letter Alpha
    $width = Test-VisualWidth -Text $char
    if ($null -ne $width) {
        return ($width -eq 2)
    }
    return $null
}

function Get-WTAmbiguousAsWide {
    # 1. Define possible config file paths (stable and preview)    
    $paths = @(
        $localAppData = $env:LOCALAPPDATA
        "$localAppData\Packages\Microsoft.WindowsTerminal_8wekyb3d8bbwe\LocalState\settings.json"
        "$localAppData\Packages\Microsoft.WindowsTerminalPreview_8wekyb3d8bbwe\LocalState\settings.json"
        "$env:USERPROFILE\AppData\Local\Microsoft\Windows Terminal\settings.json"
    )
 
    foreach ($path in $paths) {
        if ([System.IO.File]::Exists($path)) {
            try {
                # 2. Read and parse JSON
                $settings = [System.IO.File]::ReadAllText($path, [System.Text.Encoding]::UTF8) | ConvertFrom-Json --ErrorAction Stop
                if ($null -eq $settings) { continue }
                # 3. Priority: Specific Profile -> Global Defaults -> Default False
                # Note: In WT, the property is "ambiguousUnicodeWidth", value is "wide" or "narrow"
                $widthSetting = $null
                if ($null -ne $settings.profiles.defaults.ambiguousUnicodeWidth) {
                    $widthSetting = $settings.profiles.defaults.ambiguousUnicodeWidth
                }
                # If found, return boolean value
                if ($widthSetting -eq "wide") { return $true }
                if ($widthSetting -eq "narrow") { return $false }
            }
            catch {
                continue # On parse failure, try next path
            }
        }
    }
    return $false # Default to False (Narrow)
}

function Get-InitialAmbiguousAsWide {
    # In VSCode integrated terminal, always return False,
    # as it does not support real-time cursor probing.
    if ($env:TERM_PROGRAM -eq "vscode" -or $Host.Name -match "Visual Studio Code Host") {
        return $false
    }

    # Read software config (Windows Terminal settings.json)
    # This step is the last fallback
    if ($null -ne $env:WT_SESSION) {
        $wtSetting = Get-WTAmbiguousAsWide
        if ($null -ne $wtSetting) { return $wtSetting }
    }

    # Real-time cursor probing (most accurate, now robust)
    $detected = Test-AmbiguousAsWide  # This is the improved function
    if ($null -ne $detected) {
        return $detected
    }

    # Environment variable (user explicit declaration has high priority)
    if ($null -ne $env:AMBIGUOUS_AS_WIDE) {
        return $env:AMBIGUOUS_AS_WIDE -in @('1', 'true', '$true')
    }

    # Final default value
    return $false 
}

function Test-ZWJSupport {
    if ([Console]::IsOutputRedirected) {
        # Detection not possible when output is redirected, return false to indicate no support (safe fallback)
        return @{ Support = $false; Width = 1 }
    }
    # 👨‍👩‍👧‍👦 Family sequence (4 Emojis + 3 ZWJ)
    $familyEmoji = [char]::ConvertFromUtf32(0x1F468) + 
    [char]0x200D + 
    [char]::ConvertFromUtf32(0x1F469) + 
    [char]0x200D + 
    [char]::ConvertFromUtf32(0x1F467) + 
    [char]0x200D + 
    [char]::ConvertFromUtf32(0x1F466)

    $width = Test-VisualWidth -Text $familyEmoji
    if ($null -ne $width) {
        return @{
            Support = ($width -eq 2) # Proper ZWJ support if treated as single emoji (width 2)
            Width   = switch ($width) {
                2 { 0 } # Proper ZWJ support, treat as 0-width
                8 { 0 } # No ZWJ support, treat entire sequence as 2 separate emojis (4 chars * 2 width each)
                11 { 1 } # No ZWJ support, but counts each char as width 1 (e.g. some terminals), treat as 1-width
                Default { 0 } # Unexpected width, fallback to 0-width to avoid breaking layouts (best effort)
            }
        }
    }
    # If detection fails, assume no ZWJ support and treat as 0-width (safe fallback)
    return @{ Support = $false; Width = 0 }
}

function Get-InitialZWJSupport {
    # In VSCode integrated terminal, xterm.js has no ZWJ support,
    # but it treats ZWJ as zero-width (no visual effect, just concatenation).
    if ($env:TERM_PROGRAM -eq "vscode" -or $Host.Name -match "Visual Studio Code Host") {
        return @{ Support = $false; Width = 0; }
    }

    # Windows Terminal has good emoji support including ZWJ.
    if ($null -ne $env:WT_SESSION) {
        return @{ Support = $true; Width = 0; }
    }

    # Real-time probing on other terminals (most accurate)
    return Test-ZWJSupport
}

$script:AmbiguousAsWide = Get-InitialAmbiguousAsWide
$script:ZWJ = [PSCustomObject](Get-InitialZWJSupport)