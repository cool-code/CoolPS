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
    $translate.Add("HIDDEN_DIR", "hd")
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
        $commentIdx = $line.IndexOf('#')
        if ($commentIdx -ge 0) { $line = $line.Substring(0, $commentIdx) }
        
        if ([string]::IsNullOrWhiteSpace($line)) { continue }

        $parts = $line.Split(" `t", [System.StringSplitOptions]::RemoveEmptyEntries)
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