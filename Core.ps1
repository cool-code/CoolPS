. (Join-Path $PSScriptRoot "Cache.ps1")
. (Join-Path $PSScriptRoot "Colors.ps1")
. (Join-Path $PSScriptRoot "Icons.ps1")
. (Join-Path $PSScriptRoot "VisualWidth.ps1")

function Initialize-MemCache {
    Initialize-ColorsMemCache
    Initialize-IconsMemCache
}

function Get-ColorAndIcon {
    param(
        [FileSystemInfo]$Item,
        [int]$Depth = 0 # Prevent infinite loop caused by circular links
    )
    Initialize-MemCache

    # Get basic attributes
    $name = $Item.Name
    $ext = $Item.Extension.ToLower()
    $attr = "fi" 
    $isLink = $Item.Attributes.HasFlag([FileAttributes]::ReparsePoint)

    if ($isLink) {
        if (-not (Test-Path $Item.LinkTarget)) { $attr = "or" } else { $attr = "ln" }
    }
    elseif ($Item -is [DirectoryInfo]) {
        $attr = if ($Item.Attributes.HasFlag([FileAttributes]::Hidden)) { "hd" } else { "di" }
    }
    elseif ($ext -match '\.(com|exe|bat|cmd|ps1)$') {
        $attr = "ex"
    }
    elseif ($Item.Attributes.HasFlag([FileAttributes]::Hidden)) {
        $attr = "hi"
    }

    # Get initial color and icon
    $color = Get-Color $name $ext $attr
    $icon = Get-Icon $name $ext $attr

    # Handle target logic (only for links and not exceeding recursion depth)
    if ($isLink -and ($color -eq "target" -or $icon -eq "target") -and $Depth -lt 3) {
        try {
            # Get the actual path the link points to
            $targetPath = $Item.LinkTarget
            if ($null -ne $targetPath) {
                $targetItem = Get-Item -LiteralPath $targetPath -ErrorAction Stop
                # Recursive call, depth+1
                $targetColor, $targetIcon = Get-ColorAndIcon -Item $targetItem -Depth ($Depth + 1)
                # If config is target, override with target's color/icon
                if ($color -eq "target") { $color = $targetColor }
                if ($icon -eq "target") { $icon = $targetIcon }
            }
        }
        catch {
            # If the target does not exist (dead link), fallback to or (Orphan) or default value
            $color = Get-Color -Attr "or"
            $icon = Get-Icon -Attr "or"
        }
    }

    # Final rendering
    if ($null -eq $color -or $color -eq "target") {
        $color = Get-DefaultColor $attr
    }
    if ($null -eq $icon -or $icon -eq "target") {
        $icon = Get-DefaultIcon $attr
    }

    return $color, $icon
}

function Get-CoolName {
    param(
        [FileSystemInfo]$Item
    )
    $color, $icon = Get-ColorAndIcon -Item $Item
    return "$(EscapeColor $color)" + (vPadRight $icon 3) + "$($Item.Name)$(ColorReset)"
}

function Format-CoolSize {
    <#
    .SYNOPSIS
        Formats bytes into a colorful, human-readable string (B, KB, MB, GB, TB, PB, EB).
        The digital part is 7 chars wide, total output is 10 chars wide.
    #>
    param(
        [double]$Bytes,
        [string]$ValueColor = "" # Optional color for the numeric part
    )

    $units = ' B', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB'
    
    # Handle zero or negative values
    if ($Bytes -le 0) {
        return "$(ColorGray)      0  B$(ColorReset)"
    }

    $index = 0
    $value = $Bytes

    # Calculate unit level
    while ($value -ge 1024 -and $index -lt ($units.Count - 1)) {
        $value /= 1024
        $index++
    }

    # Format numeric part (PadLeft 7)
    $formattedValue = ("{0:N2}" -f $value).PadLeft(7)
    $unit = $units[$index]
    
    # Get colors
    $unitColor = Color $index

    # Combine: Value(7) + Space(1) + Unit(2) = 10 chars
    return "${ValueColor}${formattedValue} ${unitColor}$unit$(ColorReset)"
}
