function script:Get-Translate {
    $translate = [System.Collections.Generic.Dictionary[string, string]]::new([System.StringComparer]::OrdinalIgnoreCase)
    $translate.Add("BLK", "bd")
    $translate.Add("CAPABILITY", "ca")
    $translate.Add("CHR", "cd")
    $translate.Add("DIR", "di")
    $translate.Add("DOOR", "do")
    $translate.Add("EXEC", "ex")
    $translate.Add("FIFO", "pi")
    $translate.Add("FILE", "fi")
    $translate.Add("HIDDEN", "hi")
    $translate.Add("LINK", "ln")
    $translate.Add("MISSING", "mi")
    $translate.Add("MULTIHARDLINK", "mh")
    $translate.Add("NORMAL", "no")
    $translate.Add("ORPHAN", "or")
    $translate.Add("OTHER_WRITABLE", "ow")
    $translate.Add("RESET", "rs")
    $translate.Add("SETGID", "sg")
    $translate.Add("SETUID", "su")
    $translate.Add("SOCK", "so")
    $translate.Add("STICKY", "st")
    $translate.Add("STICKY_OTHER_WRITABLE", "tw")
    return $translate    
}

$script:TranslateCache = Get-Translate

function script:ConvertFrom-SourceData {
    param(
        [string]$SourceFile
    )

    if (-not [System.IO.File]::Exists($SourceFile)) { return $null }

    $filters = [System.Collections.Generic.HashSet[string]]::new(2048)
    $null = $filters.Add("TERM")
    $null = $filters.Add("COLOR")
    $null = $filters.Add("*")

    $lines = [System.IO.File]::ReadAllLines($SourceFile)
    if ($lines.Count -eq 0) { return $null }

    $result = [System.Text.StringBuilder]::new(16384)

    foreach ($line in $lines) {
        # remove comments and trim whitespace
        $cleanLine = $line.Split('#')[0].Trim()
        if (-not $cleanLine) { continue }

        # use regex to split by the last occurrence of whitespace, to allow keys with spaces (like "Saved Games")
        $parts = [regex]::Split($cleanLine, '\s+(?=\S+$)')

        if ($parts.Count -lt 2) { continue }

        $key = $parts[0]
        $val = $parts[1]

        if ($val -eq "*" -or $val -eq "" -or $filters.Contains($key)) { continue }

        $finalKey = $null
        if ($script:TranslateCache.TryGetValue($key, [ref]$finalKey)) {
            # Found a translation, use it
        }
        elseif ($key.StartsWith('*.')) {
            $finalKey = $key.ToLower()
        }
        elseif ($key.StartsWith('.')) {
            $finalKey = "*" + $key.ToLower()
        }
        else {
            $finalKey = $key
        }

        if ($filters.Contains($finalKey)) { continue }
        
        $null = $filters.Add($key)
        $null = $filters.Add($finalKey)
        if ($result.Length -gt 0) { $null = $result.Append(':') }
        $null = $result.Append($finalKey).Append('=').Append($val)
    }

    return $result.ToString()
}

function script:Get-CacheData {
    param(
        [string]$SourceFile,
        [string]$CacheFile
    )
    if ([System.IO.File]::Exists($CacheFile)) {
        $cached = [System.IO.File]::ReadAllText($CacheFile, [System.Text.Encoding]::UTF8)
        if ($cached) { return $cached }
    }
    $result = (ConvertFrom-SourceData $SourceFile)
    if ($result) {
        [System.IO.File]::WriteAllText($CacheFile, $result, [System.Text.Encoding]::UTF8)
    }
    return $result
}

$script:DefaultColors = @{
    "fi" = "0"                   # Default file no color
    "di" = "38;5;30"             # Directory default blue-green
    "ln" = "38;5;81;1"           # Link default cyan bold
    "or" = "48;5;196;38;5;232;1" # Orphan default red background with black bold
    "ex" = "38;5;208;1"          # Executable file default orange bold
    "hi" = "38;5;90"             # Hidden file default purple-gray
    "pi" = "38;5;126"            # FIFO default yellow-green
    "so" = "38;5;197"            # Socket default pink
}

$script:DefaultIcons = @{
    "fi" = "" # File default file icon
    "di" = "" # Directory default folder icon
    "ln" = "" # Link default link icon
    "or" = "" # Orphan default broken link icon
    "ex" = "" # Executable file default program icon
    "hi" = "" # Hidden file default hidden icon
    "pi" = "" # FIFO default pipe icon
    "so" = "" # Socket default socket icon
}

$script:COLORS_SOURCE = Join-Path $PSScriptRoot "../Data/LS_COLORS"
$script:COLORS_CACHE = Join-Path $HOME ".LS_COLORS_CACHE"
$script:ICONS_SOURCE = Join-Path $PSScriptRoot "../Data/LS_ICONS"
$script:ICONS_CACHE = Join-Path $HOME ".LS_ICONS_CACHE"

function script:Get-Colors {
    return Get-CacheData $script:COLORS_SOURCE $script:COLORS_CACHE
}

function script:Get-Icons {
    return Get-CacheData $script:ICONS_SOURCE $script:ICONS_CACHE
}

$script:ColorsMemCache = [PSCustomObject]@{
    Hash   = [System.Collections.Generic.Dictionary[string, string]]::new()
    IsInit = $false
}

$script:IconsMemCache = [PSCustomObject]@{
    Hash   = [System.Collections.Generic.Dictionary[string, string]]::new()
    IsInit = $false
}

function script:ConvertTo-MemCache {
    param($EnvVar)
    $hash = [System.Collections.Generic.Dictionary[string, string]]::new([System.StringComparer]::OrdinalIgnoreCase) # Store exact match and suffix match (.py, di)

    foreach ($item in ($EnvVar -split ':')) {
        $kv = $item -split '='
        if ($kv.Count -ne 2) { continue }
        $key = $kv[0]
        $val = $kv[1]
        if ($key -match '^(.*)\[0-9\]\{0,(\d+)\}$') {
            $prefix = $Matches[1].TrimStart('*')
            $maxLen = [int]$Matches[2]
            # To prevent generating too many entries,
            # we can limit the maxLen to a reasonable number, say 3.
            # This means we will generate entries for up to 999 suffixes.
            if ($maxLen -gt 3) { $maxLen = 3 }
            $hash[$prefix] = $val
            $maxN = [math]::Pow(10, $maxLen) - 1
            foreach ($i in 0..$maxN) {
                $n = "$i"
                foreach ($j in $n.Length..$maxLen) {
                    $fmt = $n.PadLeft($j, '0')
                    $hash[$prefix + $fmt] = $val
                }
            }
        }
        elseif ($key.StartsWith('*')) {
            $hash[$key.TrimStart('*')] = $val
        }
        else {
            $hash[$key] = $val
        }
    }

    return $hash
}

function script:Initialize-ColorsMemCache {
    if (-not $script:ColorsMemCache.IsInit) {
        if (-not $env:LS_COLORS) {
            $env:LS_COLORS = Get-Colors
        }
        $script:ColorsMemCache.Hash = ConvertTo-MemCache $env:LS_COLORS
        $script:ColorsMemCache.IsInit = $true
    }
}

function script:Initialize-IconsMemCache {
    if (-not $script:IconsMemCache.IsInit) {
        if (-not $env:LS_ICONS) {
            $env:LS_ICONS = Get-Icons
        }
        $script:IconsMemCache.Hash = ConvertTo-MemCache $env:LS_ICONS
        $script:IconsMemCache.IsInit = $true
    }
}

Initialize-ColorsMemCache
Initialize-IconsMemCache

function script:Lookup {
    param($DefaultHash, $Hash, $Name, $Ext, $Attr)
    if ($null -ne $Name -and $Hash.ContainsKey($Name)) {
        return $Hash[$Name]
    }
    if ($null -ne $Ext -and $Hash.ContainsKey($Ext)) {
        return $Hash[$Ext] 
    }
    if ($null -eq $Attr) {
        return $null
    }
    if ($Hash.ContainsKey($Attr)) {
        return $Hash[$Attr]
    }
    return $DefaultHash[$Attr]
}

function script:Get-Color {
    param($Name, $Ext, $Attr)
    return Lookup $script:DefaultColors $script:ColorsMemCache.Hash $Name $Ext $Attr
}

function script:Get-Icon {
    param($Name, $Ext, $Attr)
    return Lookup $script:DefaultIcons $script:IconsMemCache.Hash $Name $Ext $Attr
}

function script:Get-DefaultColor {
    param($Attr)
    return $script:DefaultColors[$Attr]
}

function script:Get-DefaultIcon {
    param($Attr)
    return $script:DefaultIcons[$Attr]
}

function script:Get-ColorAndIcon {
    param(
        [System.IO.FileSystemInfo]$Item,
        [int]$Depth = 0 # Prevent infinite loop caused by circular links
    )
    # Get basic attrs
    $name = $Item.Name
    $ext = $Item.Extension.ToLower()
    $attrs = $Item.Attributes
    $fa = [System.IO.FileAttributes]
    $isLink = $attrs.HasFlag($fa::ReparsePoint)

    $attr = if ($isLink) {
        if ($item.PSObject.Methods['ResolveLinkTarget']) {
            try {
                $target = $item.ResolveLinkTarget($true);
                $name = $target.Name
                $ext = $target.Extension.ToLower()
                "ln"
            }
            catch { "or" }
        }
        else { "ln" }
    }
    elseif ($attrs.HasFlag($fa::Hidden)) { "hi" }
    elseif ($Item -is [System.IO.DirectoryInfo]) { "di" }
    elseif ($attrs.HasFlag($fa::SparseFile)) { "pi" }
    elseif ($ext -eq ".sock" -or $ext -eq ".socket") { "so" }
    elseif ($ext -match '\.(com|exe|bat|cmd|ps1|sh)$') { "ex" }
    else { "fi" }

    # Get initial color and icon
    $color = Get-Color $name $ext $attr
    $icon = Get-Icon $name $ext $attr

    # Final rendering
    if ($null -eq $color -or $color -eq "target") {
        $color = Get-DefaultColor $attr
    }
    if ($null -eq $icon -or $icon -eq "target") {
        $icon = Get-DefaultIcon $attr
    }

    if ($attrs.HasFlag($fa::System)) {
        $color += ";2" # Dim system files
    }
    if ($attrs.HasFlag($fa::ReadOnly)) {
        $color += ";4" # Underline read-only files
    }
    return $color, $icon
}

function script:EscapeColor {
    param($Color)
    return "$([char]27)[${Color}m"
}

function script:FontBold {
    param($Text)
    return "$(EscapeColor '1')${Text}$(EscapeColor '22')"
}

function script:FontDim {
    param($Text)
    return "$(EscapeColor '2')${Text}$(EscapeColor '22')"
}

function script:FontItalic {
    param($Text)
    return "$(EscapeColor '3')${Text}$(EscapeColor '23')"
}

function script:FontUnderline {
    param($Text)
    return "$(EscapeColor '4')${Text}$(EscapeColor '24')"
}

function script:FontBlinking {
    param($Text)
    return "$(EscapeColor '5')${Text}$(EscapeColor '25')"
}

function script:FontReverse {
    param($Text)
    return "$(EscapeColor '7')${Text}$(EscapeColor '27')"
}

function script:FontHidden {
    param($Text)
    return "$(EscapeColor '8')${Text}$(EscapeColor '28')"
}

function script:FontStrikeThrough {
    param($Text)
    return "$(EscapeColor '9')${Text}$(EscapeColor '29')"
}

function script:ColorReset {
    return EscapeColor "0"
}

# Red, Orange, Yellow, Green, Cyan, Blue, Purple, Gray, Silver, White 10 color cycle, index 0-9
$script:Colors = @(196, 208, 220, 40, 81, 33, 135, 242, 250, 253) | ForEach-Object { EscapeColor "38;5;$_" }

function script:Color {
    param($Index)
    return $script:Colors[$Index]
}

function script:ColorRed {
    return $script:Colors[0]
}

function script:ColorOrange {
    return $script:Colors[1]
}

function script:ColorYellow {
    return $script:Colors[2]
}

function script:ColorGreen {
    return $script:Colors[3]
}

function script:ColorCyan {
    return $script:Colors[4]
}

function script:ColorBlue {
    return $script:Colors[5]
}

function script:ColorPurple {
    return $script:Colors[6]
}

function script:ColorGray {
    return $script:Colors[7]
}

function script:ColorSilver {
    return $script:Colors[8]
}

function script:ColorWhite {
    return $script:Colors[9]
}

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
    if ($color -eq "0") {
        return "$(vPadRight $icon 3)$($Item.Name)"
    }
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

