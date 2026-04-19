$Script:DefaultIcons = @{
    "fi" = "" # File default file icon
    "di" = "" # Directory default folder icon
    "ow" = "" # Writable directory default open folder icon
    "ln" = "" # Link default link icon
    "or" = "" # Orphan default broken link icon
    "ex" = "󰞷" # Executable file default program icon
    "hi" = "󰘓" # Hidden file default hidden icon
    "hd" = "󱞞" # Hidden folder default hidden icon
}

$Script:ICONS_SOURCE = Join-Path $PSScriptRoot "Data\LS_ICONS"
$Script:ICONS_CACHE = Join-Path $PSScriptRoot "Data\.LS_ICONS_CACHE"

function Get-Icons {
    return Get-CacheData $Script:ICONS_SOURCE $Script:ICONS_CACHE
}

if (-not $Env:LS_ICONS) {
    $Env:LS_ICONS = Get-Icons
}

$Script:IconsMemCache = [PSCustomObject]@{
    Hash     = [Dictionary[string, string]]::new()
    Patterns = [List[PSCustomObject]]::new()
    IsInit   = $false
}

function Initialize-IconsMemCache {
    if (-not $Script:IconsMemCache.IsInit) {
        $Script:IconsMemCache.Hash, $Script:IconsMemCache.Patterns = ConvertTo-MemCache $Env:LS_ICONS
        $Script:IconsMemCache.IsInit = $true
    }
}

function Update-IconsCache {
    Remove-Item $Script:ICONS_CACHE -ErrorAction SilentlyContinue
    $Env:LS_ICONS = Get-Icons
    $Script:IconsMemCache.IsInit = $false # Force reinitialize icon cache
    Initialize-IconsMemCache
    $msg = Get-LocalizedString 'LSIconsCacheUpdated'
    Write-Host $msg -ForegroundColor Green
}

function Get-Icon {
    param($Name, $Ext, $Attr)
    return Lookup $Script:DefaultIcons $Script:IconsMemCache.Hash $Script:IconsMemCache.Patterns $Name $Ext $Attr
}

function Get-DefaultIcon {
    param($Attr)
    return $Script:DefaultIcons[$Attr]
}
