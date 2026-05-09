$eastAsianWidthUrl = "https://www.unicode.org/Public/UCD/latest/ucd/EastAsianWidth.txt"
$unicodeDataUrl = "https://www.unicode.org/Public/UCD/latest/ucd/UnicodeData.txt"
$emojiDataUrl = "https://www.unicode.org/Public/UCD/latest/ucd/emoji/emoji-data.txt"

$eastAsianWidthContent = (Invoke-WebRequest $eastAsianWidthUrl).Content -split "`n"
$unicodeContent = (Invoke-WebRequest $unicodeDataUrl).Content -split "`n"
$emojiContent = (Invoke-WebRequest $emojiDataUrl).Content -split "`n"

function ParseEastAsianWidth($typeRegex) {
    $bits = New-Object System.Collections.BitArray 0x20000
    foreach ($line in $eastAsianWidthContent) {
        if ($line -match '^([0-9A-F]+)(?:\.\.([0-9A-F]+))?\s*;\s*(?:' + $typeRegex + ')') {
            $s = [Convert]::ToInt32($Matches[1], 16)
            $e = if ($Matches[2]) { [Convert]::ToInt32($Matches[2], 16) } else { $s }
            for ($i = $s; $i -le $e -and $i -lt 0x20000; $i++) { $bits.Set($i, $true) }
        }
    }
    return $bits
}

function ParseZeroWidth() {
    $bits = New-Object System.Collections.BitArray 0x20000
    $unicodeContent | ForEach-Object {
        $line = $_.Trim()
        if ($line -eq '' -or $line.StartsWith('#')) { return }
        $parts = $line -split ';'
        $cp = [Convert]::ToInt32($parts[0], 16)
        if ($cp -ge 0x20000) { return }
        $category = $parts[2]

        if (($category -match 'Mn|Me|Mc|Cf') -or ($cp -ge 0x1F3FB -and $cp -le 0x1F3FF)) {
            $bits.Set($cp, $true)
        }
    }
    return $bits
}

function ParseEmojiData() {
    $bits = New-Object System.Collections.BitArray 0x20000
    $emojiRegex = '^([0-9A-F]+)(?:\.\.([0-9A-F]+))?\s*;\s*(Extended_Pictographic|Emoji_Presentation)'

    $emojiContent | ForEach-Object {
        if ($_ -match $emojiRegex) {
            $s = [Convert]::ToInt32($Matches[1], 16)
            $e = if ($Matches[2]) { [Convert]::ToInt32($Matches[2], 16) } else { $s }
            for ($i = $s; $i -le $e -and $i -lt 0x20000; $i++) { $bits.Set($i, $true) }
        }
    }
    return $bits
}

function Get-OptimizedString($bits) {
    $result = New-Object System.Collections.Generic.List[string]
    $inRange = $false
    $start = 0
    for ($i = 0; $i -lt 0x20000; $i++) {
        if ($bits.Get($i)) {
            if (-not $inRange) { $start = $i; $inRange = $true }
        }
        elseif ($inRange) {
            $end = $i - 1
            if ($start -eq $end) { $result.Add("{0:X}" -f $start) }
            else { $result.Add(("{0:X}-{1:X}" -f $start, $end)) }
            $inRange = $false
        }
    }
    if ($inRange) {
        $end = 0x20000 - 1
        if ($start -eq $end) { $result.Add("{0:X}" -f $start) }
        else { $result.Add(("{0:X}-{1:X}" -f $start, $end)) }
    }
    return $result -join ","
}

$wideBits = ParseEastAsianWidth "W|F"

$ambigBits = ParseEastAsianWidth "A"

$zeroBits = ParseZeroWidth

$emojiBits = ParseEmojiData

$wideRangeStr = Get-OptimizedString $wideBits
$ambigRangeStr = Get-OptimizedString $ambigBits
$emojiRangeStr = Get-OptimizedString $emojiBits
$zeroRangeStr = Get-OptimizedString $zeroBits

"private const string _wideRange = " + '"' + $wideRangeStr + '";'
"private const string _ambigRange = " + '"' + $ambigRangeStr + '";'
"private const string _zeroRange = " + '"' + $zeroRangeStr + '";'
"private const string _emojiRange = " + '"' + $emojiRangeStr + '";'
