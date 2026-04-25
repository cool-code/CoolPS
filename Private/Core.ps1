function global:Format-CoolName {
    <#
    .SYNOPSIS
        Formats the input filesystem object as a string with color and icon.
    .PARAMETER Item
        A FileSystemInfo object representing a file or directory.
    .OUTPUT
        A string with ANSI color codes and an icon, visually representing the file or directory.
    #>
    param(
        [System.IO.FileSystemInfo]$Item
    )
    $color, $icon = Get-ColorAndIcon -Item $Item
    return "$(EscapeColor $color)$(vPadRight $icon 3)$($Item.Name)$(ColorReset)"
}

function global:Format-CoolSize {
    <#
    .SYNOPSIS
        Formats bytes into a colorful, human-readable string (B, KB, MB, GB, TB, PB, EB).
        The digital part is 7 chars wide, total output is 10 chars wide.
    .PARAMETER Bytes
        The size in bytes to format.
    .PARAMETER ValueColor
        Optional color for the numeric part.
    .OUTPUT
        A string with ANSI color codes, where the numeric part is right-aligned to 7 characters,
        followed by a space and a 2-character unit, for a total width of 10 characters.
    .EXAMPLE
        Format-CoolSize -Bytes 123456789 -ValueColor (ColorRed)
        Output: " 117.74 MB" with "117.74" in red and "MB" in the color corresponding to its unit.
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

function global:Get-VisualWidth {
    <#
    .SYNOPSIS
        Calculates the visual width of a string.
    .PARAMETER Text
        The string for which to calculate the visual width.
    .OUTPUT
        An integer representing the visual width of the string.
    #>
    param([string]$Text)
    $elements, $widths = Get-VisualElementsAndWidths -Text $Text
    $totalWidth = 0
    foreach ($w in $widths) { $totalWidth += $w }
    return $totalWidth
}

function global:Format-VisualWidthString {
    <#
    .SYNOPSIS
        Unified entry point for visual width operations.
    .PARAMETER Text
        The string to format.
    .PARAMETER VisualWidth
        The target visual width for the output string.
    .PARAMETER Mode
        Available: PadLeft, PadCenter, PadRight, TruncateFile, TruncateDir
    .OUTPUT
        A string that has been padded or truncated to fit the specified visual width, according to the selected mode.
    .EXAMPLE
        Format-VisualWidthString -Text "👨‍👩‍👧‍👦中国家庭.txt" -VisualWidth 20 -Mode PadRight
        Output: "👨‍👩‍👧‍👦中国家庭.txt      " on support ZWJ term (treating 👨‍👩‍👧‍👦 as width 2)
        Output: "👨‍👩‍👧‍👦中国家庭.txt" on non-support ZWJ term (treating 👨‍👩‍👧‍👦 as width 8 or 11)
    #>
    param(
        [Parameter(Mandatory = $true)]
        [string]$Text,
        
        [Parameter(Mandatory = $true)]
        [int]$VisualWidth,
        
        [Parameter(Mandatory = $true)]
        [ValidateSet("PadLeft", "PadCenter", "PadRight", "TruncateFile", "TruncateDir")]
        [string]$Mode
    )
    switch ($Mode) {
        "PadLeft" { return VisualWidthPad -Text $Text -Width $VisualWidth -Alignment -1 }
        "PadCenter" { return VisualWidthPad -Text $Text -Width $VisualWidth -Alignment 0 }
        "PadRight" { return VisualWidthPad -Text $Text -Width $VisualWidth -Alignment 1 }
        "TruncateFile" { return VisualWidthTruncate -Text $Text -MaxWidth $VisualWidth -Mode 0 }
        "TruncateDir" { return VisualWidthTruncate -Text $Text -MaxWidth $VisualWidth -Mode 1 }
    }
}

Export-ModuleMember -Function Format-CoolName, Format-CoolSize, Get-VisualWidth, Format-VisualWidthString