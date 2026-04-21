function script:ConvertFrom-SourceData {
    param(
        [string]$SourceFile
    )

    if (-not (Test-Path $SourceFile)) { return $null }

    $translate = @{
        "BLK"                   = "bd"
        "CAPABILITY"            = "ca"
        "CHR"                   = "cd"
        "DIR"                   = "di"
        "DOOR"                  = "do"
        "EXEC"                  = "ex"
        "FIFO"                  = "pi"
        "FILE"                  = "fi"
        "HIDDEN"                = "hi"
        "HIDDEN_DIR"            = "hd"
        "LINK"                  = "ln"
        "MISSING"               = "mi"
        "MULTIHARDLINK"         = "mh"
        "NORMAL"                = "no"
        "ORPHAN"                = "or"
        "OTHER_WRITABLE"        = "ow"
        "RESET"                 = "rs"
        "SETGID"                = "sg"
        "SETUID"                = "su"
        "SOCK"                  = "so"
        "STICKY"                = "st"
        "STICKY_OTHER_WRITABLE" = "tw"
    }
    $filters = [System.Collections.Generic.HashSet[string]]::new()
    $null = $filters.Add("TERM")
    $null = $filters.Add("COLOR")
    $lines = [System.IO.File]::ReadAllLines($SourceFile)
    if ($lines.Count -eq 0) { return $null }
    return @(
        foreach ($line in $lines) {
            $line = $line -replace '#.*$', ''
            if ($line -match '^\s*(\S+)\s+(\S+)') {
                $key = $Matches[1]
                $val = $Matches[2]
                # Skip already processed items and special filter items to avoid polluting environment variables
                if ($filters.Contains($key) -or $val -eq "*" -or $val -eq "") {
                    continue
                }
                $finalKey = if ($translate.ContainsKey($key)) {
                    $translate[$key] # Convert to short key first
                }
                elseif ($key.StartsWith('*.')) {
                    $key.ToLower() # Keep suffix match as is, but lowercase
                }
                elseif ($key.StartsWith('.')) {
                    "*" + $key.ToLower()  # File extension match with * prefix, and lowercase
                }
                else {
                    $key  # Otherwise use the original key
                }
                if ($filters.Contains($finalKey)) {
                    continue
                }
                $null = $filters.Add($key)
                $null = $filters.Add($finalKey)
                @($finalKey, $val) -join '='
            } 
        } 
    ) -join ':'
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
    $hash = [System.Collections.Generic.Dictionary[string, string]]::new() # Store exact match and suffix match (.py, di)
    $patterns = [System.Collections.Generic.List[PSCustomObject]]::new()   # Store wildcard and regex match (*.r[0-9], *README)

    if ($EnvVar) {
        foreach ($item in ($EnvVar -split ':')) {
            $kv = $item -split '='
            if ($kv.Count -eq 2) {
                $key = $kv[0]
                $val = $kv[1]
                if ($key.StartsWith('*')) {
                    $key = $key.Substring(1)
                    if (-not $key.StartsWith('.') -or $key -match '\*|\[|\{') {
                        # Escape wildcard and convert to Regex format
                        # Example: .r[0-9]{0,2} -> \.r[0-9]{0,2}$
                        $regexStr = [Regex]::Escape($key).Replace('\[', '[').Replace('\{', '{').Replace('\*', '.*') + "$"
                        $patterns.Add([PSCustomObject]@{ Regex = [Regex]::new($regexStr, [System.Text.RegularExpressions.RegexOptions]::Compiled); Value = $val })
                    }
                }
                $hash[$key] = $val
            }
        }
    }

    return $hash, $patterns
}

function script:Lookup {
    param($DefaultHash, $Hash, $Patterns, $Name, $Ext, $Attr)
    if ($null -ne $Name -and $Hash.ContainsKey($Name)) {
        return $Hash[$Name]
    }
    if ($null -ne $Ext -and $Hash.ContainsKey($Ext)) {
        return $Hash[$Ext] 
    }
    if ($null -ne $Name) {
        foreach ($p in $Patterns) {
            if ($p.Regex.IsMatch($Name)) {
                return $p.Value
            }
        }
    }
    if ($null -eq $Attr) {
        return $null
    }
    if ($Hash.ContainsKey($Attr)) {
        return $Hash[$Attr]
    }
    return $DefaultHash[$Attr]
}