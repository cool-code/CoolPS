$script:DefaultColors = @{
    "fi" = "0"                   # Default file no color
    "di" = "38;5;30"             # Directory default blue
    "ow" = "38;5;220;1"          # Writable directory default yellow bold
    "ln" = "38;5;35;1"           # Link default cyan bold
    "or" = "48;5;196;38;5;232;1" # Orphan default white on red bold
    "ex" = "38;5;208;1"          # Executable file default orange bold
    "hi" = "38;5;90"             # Hidden file default gray
    "hd" = "38;5;90;1"           # Hidden directory default gray bold
    "pi" = "38;5;126"            # FIFO default purple
    "so" = "38;5;197"            # Socket default pink
}

$script:DefaultIcons = @{
    "fi" = "" # File default file icon
    "di" = "" # Directory default folder icon
    "ow" = "" # Writable directory default open folder icon
    "ln" = "" # Link default link icon
    "or" = "" # Orphan default broken link icon
    "ex" = "" # Executable file default program icon
    "hi" = "" # Hidden file default hidden icon
    "hd" = "" # Hidden folder default hidden icon
    "pi" = "" # FIFO default pipe icon
    "so" = "" # Socket default socket icon
}

$script:COLORS_SOURCE = Join-Path $PSScriptRoot "../Data/LS_COLORS"
$script:COLORS_CACHE = Join-Path $PSScriptRoot "../Data/.LS_COLORS_CACHE"
$script:ICONS_SOURCE = Join-Path $PSScriptRoot "../Data/LS_ICONS"
$script:ICONS_CACHE = Join-Path $PSScriptRoot "../Data/.LS_ICONS_CACHE"

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
    $attr = "fi"
    $isLink = $attrs.HasFlag($fa::ReparsePoint)

    if ($isLink) {
        if (-not (Test-Path $Item.LinkTarget)) { $attr = "or" } else { $attr = "ln" }
    }
    elseif ($Item -is [System.IO.DirectoryInfo]) {
        $attr = if ($attrs.HasFlag($fa::Hidden)) { "hd" } else { "di" }
    }
    elseif ($attrs.HasFlag($fa::Hidden)) {
        $attr = "hi"
    }
    elseif ($attrs.HasFlag($fa::SparseFile)) {
        $attr = "pi"
    }
    elseif ($ext -eq ".sock" -or $ext -eq ".socket") {
        $attr = "so"
    }
    elseif ($ext -match '\.(com|exe|bat|cmd|ps1|sh)$') {
        $attr = "ex"
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
