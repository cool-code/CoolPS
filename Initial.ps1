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
    return ($width -eq 2)
}

function Get-InitialAmbiguousAsWide {
    # In VSCode integrated terminal, always return False,
    # as it does not support real-time cursor probing.
    if ($env:TERM_PROGRAM -eq "vscode" -or $Host.Name -match "Visual Studio Code Host") {
        return $false
    }

    # Environment variable (user explicit declaration has high priority)
    if ($null -ne $env:AMBIGUOUS_AS_WIDE) {
        return $env:AMBIGUOUS_AS_WIDE -in @('1', 'true', '$true')
    }

    # Real-time cursor probing (most accurate, now robust)
    return Test-AmbiguousAsWide  # This is the improved function
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

Set-Variable -Name 'AmbiguousAsWide' -Value (Get-InitialAmbiguousAsWide) -Visibility Private -Option Constant -Scope Script
Set-Variable -Name 'ZWJ' -Value ([PSCustomObject](Get-InitialZWJSupport)) -Visibility Private -Option Constant -Scope Script