function script:Get-WindowsPermString {
    param($Item)
    $fa = [System.IO.FileAttributes]
    $attrs = $Item.Attributes
    $isLink = $attrs.HasFlag($fa::ReparsePoint)
    $isDir = $attrs.HasFlag($fa::Directory)
    $isRO = $attrs.HasFlag($fa::ReadOnly)
    $isSys = $attrs.HasFlag($fa::System)
    $isHide = $attrs.HasFlag($fa::Hidden)

    $type = if ($isLink) { 'l' } elseif ($isDir) { 'd' } else { '-' }

    $isExec = $isDir -or ($Item.Extension -in @('.exe', '.com', '.bat', '.cmd', '.ps1', '.sh'))
    $isRestricted = $isSys -or $isHide

    # owner bits
    $or = 'r'
    $ow = if ($isRO) { '-' } else { 'w' }
    $ox = if ($isExec) { 'x' } else { '-' }
    # group bits
    $gr = if ($isRestricted) { '-' } else { 'r' }
    $gw = if ($isRO -or $isRestricted) { '-' } else { 'w' }
    $gx = $ox
    # everyone bits
    $er = $gr; $ew = $gw; $ex = $ox

    return "$type$or$ow$ox$gr$gw$gx$er$ew$ex"
}

# Format date in Linux ls -l style (12 chars wide)
function script:Format-LLDate {
    param([DateTime]$DateTime, [DateTime]$Now)
    $sixMonthsAgo = $now.AddMonths(-6)
    $mon = $DateTime.ToString("MMM", [System.Globalization.CultureInfo]::InvariantCulture)
    $day = $DateTime.Day.ToString().PadLeft(2)
    if ($DateTime -gt $sixMonthsAgo -and $DateTime -le $Now) {
        return "$mon $day $($DateTime.ToString('HH:mm'))"
    }
    else {
        return "$mon $day  $($DateTime.Year)"
    }
}

function script:Get-LLItemInfo {
    param(
        [System.IO.FileSystemInfo]$Item
    )
    $isDir = $Item -is [System.IO.DirectoryInfo]
    $size = if ($isDir) { -1 } else { $Item.Length }
    $owner = $env:USERNAME
    $group = 'None'
    $linkCount = 1
    $permString = ''
    $blocks = 0

    if ($script:IsWindows) {
        try {
            $acl = [System.IO.FileSystemAclExtensions]::GetAccessControl($item)
            $owner = $acl.GetOwner([System.Security.Principal.NTAccount]).Value.Split('\')[-1]
            $group = $acl.GetGroup([System.Security.Principal.NTAccount]).Value.Split('\')[-1]
            $linkCount = if ($isDir) { $item.GetFileSystemInfos().Count + 2 } else { 1 }
        }
        catch {}
        $permString = Get-WindowsPermString -Item $Item
        $blocks = if ($isDir) { 0 } else { [Math]::Ceiling($Item.Length / 512) }
    }
    else {
        $owner = $item.PSObject.Properties['User'].Value
        $group = $item.PSObject.Properties['Group'].Value
        $linkCount = $item.PSObject.Properties['UnixStat'].Value.HardlinkCount
        $permString = $item.PSObject.Properties['UnixMode'].Value
        $blocks = $item.PSObject.Properties['UnixStat'].Value.NumberOfBlocks
    }

    [PSCustomObject]@{
        Item         = $Item
        IsDirectory  = $isDir
        Permissions  = $permString
        LinkCount    = $linkCount
        Owner        = $owner
        Group        = $group
        SizeBytes    = $size
        LastModified = $Item.LastWriteTime
        Blocks       = $blocks
    }
}

function global:ll {
    [CmdletBinding(DefaultParameterSetName = 'Path')]
    param(
        [string]$Path = ".",
        [switch]$All
    )

    try {
        $items = Get-ChildItem -LiteralPath $Path -Force:$All -ErrorAction Stop
    }
    catch {
        $PSCmdlet.WriteError($_)
        return
    }
    if (-not $items) {
        Write-Host "total 0" -ForegroundColor Cyan
        return
    }

    $reset = ColorReset
    $now = Get-Date

    # Collect row data
    $rows = @(foreach ($item in $items) { Get-LLItemInfo -Item $item })

    $totalBlocks = ($rows | Measure-Object -Property Blocks -Sum).Sum
    Write-Host "total $totalBlocks" -ForegroundColor Cyan

    # Column widths
    $maxLinksW = [Math]::Max(5, ($rows | ForEach-Object { "$($_.LinkCount)".Length } | Measure-Object -Maximum).Maximum)
    $maxOwnerW = [Math]::Max(5, ($rows | ForEach-Object { $_.Owner.Length }     | Measure-Object -Maximum).Maximum)
    $maxGroupW = [Math]::Max(5, ($rows | ForEach-Object { $_.Group.Length }     | Measure-Object -Maximum).Maximum)
    $permW = 11
    $sizeW = 10  # Format-CoolSize always outputs 10 visual chars
    $modW = 12  # "MMM DD HH:mm" or "MMM DD  YYYY"

    $termWidth = $Host.UI.RawUI.WindowSize.Width
    $usedW = 2 + $permW + 1 + $maxLinksW + 1 + $maxOwnerW + 1 + $maxGroupW + 1 + $sizeW + 1 + $modW + 1
    $nameW = [Math]::Max(8, $termWidth - $usedW)

    # Data rows
    foreach ($row in $rows) {
        $cPerm = vPadRight  (Format-CoolPermissions $row.Permissions) $permW
        $cLinks = vPadLeft  "$(Color 1)$($row.LinkCount)$reset"  $maxLinksW
        $cOwner = vPadRight "$(Color 2)$($row.Owner)$reset"  $maxOwnerW
        $cGroup = vPadRight "$(Color 3)$($row.Group)$reset"  $maxGroupW
        $cSize = if ($row.SizeBytes -lt 0) {
            vPadLeft "$(Color 4)-$reset" $sizeW
        }
        else {
            Format-CoolSize -Bytes $row.SizeBytes -ValueColor (Color 4)
        }
        $cMod = vPadLeft "$(Color 5)$(Format-LLDate -DateTime $row.LastModified -Now $now)$reset" $modW

        $mode = if ($row.IsDirectory) { 1 } else { 0 }
        $nameText = VisualWidthTruncate -Text (Format-CoolName -Item $row.Item) -MaxWidth $nameW -Mode $mode

        "$cPerm  $cLinks  $cOwner  $cGroup  $cSize  $cMod  $nameText"
    }
}