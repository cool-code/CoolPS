$Script:DefaultColors = @{
    "fi" = "0"     # Default file no color
    "di" = "34;1" # Directory default blue bold
    "ow" = "36;1" # Writable directory default cyan bold
    "ln" = "35;1" # Link default purple bold
    "or" = "31;1" # Orphan default red bold
    "ex" = "32"   # Executable file default green
    "hi" = "90"   # Hidden file default gray
    "hd" = "90;1" # Hidden folder default gray bold
}

$Script:COLORS_SOURCE = Join-Path $PSScriptRoot "Data\LS_COLORS"
$Script:COLORS_CACHE = Join-Path $PSScriptRoot "Data\.LS_COLORS_CACHE"

function Get-Colors {
    return Get-CacheData $Script:COLORS_SOURCE $Script:COLORS_CACHE
}

if (-not $Env:LS_COLORS) {
    $Env:LS_COLORS = Get-Colors
}

$Script:ColorsMemCache = [PSCustomObject]@{
    Hash     = [Dictionary[string, string]]::new()
    Patterns = [List[PSCustomObject]]::new()
    IsInit   = $false
}

function Initialize-ColorsMemCache {
    if (-not $Script:ColorsMemCache.IsInit) {
        $Script:ColorsMemCache.Hash, $Script:ColorsMemCache.Patterns = ConvertTo-MemCache $Env:LS_COLORS
        $Script:ColorsMemCache.IsInit = $true
    }
}

function Update-ColorsCache {
    Remove-Item $Script:COLORS_CACHE -ErrorAction SilentlyContinue
    $Env:LS_COLORS = Get-Colors
    $Script:ColorsMemCache.IsInit = $false # Force reinitialize color cache
    Initialize-ColorsMemCache
    $msg = Get-LocalizedString 'LSColorsCacheUpdated'
    Write-Host $msg -ForegroundColor Green
}

function Get-Color {
    param($Name, $Ext, $Attr)
    return Lookup $Script:DefaultColors $Script:ColorsMemCache.Hash $Script:ColorsMemCache.Patterns $Name $Ext $Attr
}

function Get-DefaultColor {
    param($Attr)
    return $Script:DefaultColors[$Attr]
}

function EscapeColor {
    param($Color)
    return "$([char]27)[${Color}m"
}

# Red, Orange, Yellow, Green, Cyan, Blue, Purple, Gray, Silver, White 10 color cycle, index 0-9
$Script:Colors = @(196, 208, 220, 40, 81, 75, 141, 242, 250, 253) | ForEach-Object { EscapeColor "38;5;$_" }

function Color {
    param($Index)
    return $Script:Colors[$Index]
}

function ColorReset {
    return EscapeColor 0
}

function ColorRed {
    return $Script:Colors[0]
}

function ColorOrange {
    return $Script:Colors[1]
}

function ColorYellow {
    return $Script:Colors[2]
}

function ColorGreen {
    return $Script:Colors[3]
}

function ColorCyan {
    return $Script:Colors[4]
}

function ColorBlue {
    return $Script:Colors[5]
}

function ColorPurple {
    return $Script:Colors[6]
}

function ColorGray {
    return $Script:Colors[7]
}

function ColorSilver {
    return $Script:Colors[8]
}

function ColorWhite {
    return $Script:Colors[9]
}