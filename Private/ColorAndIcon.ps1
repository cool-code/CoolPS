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

# Red, Orange, Yellow, Green, Cyan, Blue, Purple, Gray, Silver, White 10 color cycle, index 0-9
$script:Colors = @(196, 208, 220, 40, 81, 75, 141, 242, 250, 253) | ForEach-Object { EscapeColor "38;5;$_" }

function script:Color {
    param($Index)
    return $script:Colors[$Index]
}

function script:ColorReset {
    return EscapeColor 0
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
