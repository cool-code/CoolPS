function script:Get-Translate {
    $translate = [System.Collections.Generic.Dictionary[string, string]]::new([System.StringComparer]::OrdinalIgnoreCase)
    $null = $translate.Add("BLK", "bd")
    $null = $translate.Add("CAPABILITY", "ca")
    $null = $translate.Add("CHR", "cd")
    $null = $translate.Add("DIR", "di")
    $null = $translate.Add("DOOR", "do")
    $null = $translate.Add("EXEC", "ex")
    $null = $translate.Add("FIFO", "pi")
    $null = $translate.Add("FILE", "fi")
    $null = $translate.Add("HIDDEN", "hi")
    $null = $translate.Add("HIDDEN_DIR", "hd")
    $null = $translate.Add("LINK", "ln")
    $null = $translate.Add("MISSING", "mi")
    $null = $translate.Add("MULTIHARDLINK", "mh")
    $null = $translate.Add("NORMAL", "no")
    $null = $translate.Add("ORPHAN", "or")
    $null = $translate.Add("OTHER_WRITABLE", "ow")
    $null = $translate.Add("RESET", "rs")
    $null = $translate.Add("SETGID", "sg")
    $null = $translate.Add("SETUID", "su")
    $null = $translate.Add("SOCK", "so")
    $null = $translate.Add("STICKY", "st")
    $null = $translate.Add("STICKY_OTHER_WRITABLE", "tw")
    return $translate    
}

$script:TranslateCache = Get-Translate

function script:ConvertFrom-SourceData {
    param(
        [string]$SourceFile
    )

    if (-not [System.IO.File]::Exists($SourceFile)) { return $null }

    $filters = [System.Collections.Generic.HashSet[string]]::new(16384)
    $null = $filters.Add("TERM")
    $null = $filters.Add("COLOR")
    $null = $filters.Add("*")

    $lines = [System.IO.File]::ReadAllLines($SourceFile)
    if ($lines.Count -eq 0) { return $null }

    $result = [System.Text.StringBuilder]::new()

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
    if (Test-Path $CacheFile) {
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