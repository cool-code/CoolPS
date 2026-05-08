$url = "https://www.unicode.org/Public/UCD/latest/ucd/EastAsianWidth.txt"
$content = (Invoke-WebRequest $url).Content -split "`n"

function Get-OptimizedRanges($typeRegex) {
    $bits = New-Object System.Collections.BitArray 262144
    foreach ($line in $content) {
        if ($line -match '^([0-9A-F]+)(?:\.\.([0-9A-F]+))?\s*;\s*(?:' + $typeRegex + ')') {
            $s = [Convert]::ToInt32($Matches[1], 16)
            $e = if ($Matches[2]) { [Convert]::ToInt32($Matches[2], 16) } else { $s }
            for ($i = $s; $i -le $e -and $i -lt 262144; $i++) { $bits.Set($i, $true) }
        }
    }

    $result = New-Object System.Collections.Generic.List[string]
    $inRange = $false
    $start = 0
    for ($i = 0; $i -lt 262144; $i++) {
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
        $end = 262143
        if ($start -eq $end) { $result.Add("{0:X}" -f $start) }
        else { $result.Add(("{0:X}-{1:X}" -f $start, $end)) }
    }
    return $result -join ","
}


$wideStr = Get-OptimizedRanges "W|F"

$ambigStr = Get-OptimizedRanges "A"

$wideStr | Set-Content "wide.txt"
$ambigStr | Set-Content "ambig.txt"
