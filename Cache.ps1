function ConvertFrom-SourceData {
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

    $entries = [System.Collections.Generic.List[string]]::new()
    $filters = [System.Collections.Generic.HashSet[string]]::new()
    [void]$filters.Add("TERM")
    [void]$filters.Add("COLOR")

    foreach ($line in Get-Content $SourceFile) {
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
            [void]$entries.Add("$finalKey=$val")
            [void]$filters.Add($key)
            [void]$filters.Add($finalKey)
        }
    }
    return [string]::Join(":", $entries)
}

function Get-CacheData {
    param(
        [string]$SourceFile,
        [string]$CacheFile
    )
    if (Test-Path $CacheFile) {
        $cached = Get-Content $CacheFile -Raw -Encoding utf8 -ErrorAction SilentlyContinue
        if ($cached) { return $cached }
    }
    $result = (ConvertFrom-SourceData $SourceFile)
    if ($result) {
        $result | Out-File -FilePath $CacheFile -Encoding utf8 -NoNewline
    }
    return $result
}

function ConvertTo-MemCache {
    param($EnvVar)
    $hash = [System.Collections.Generic.Dictionary[string, string]]::new() # Store exact match and suffix match (.py, di)
    $patterns = [System.Collections.Generic.List[PSCustomObject]]::new()   # Store wildcard and regex match (*.r[0-9], *README)

    if ($EnvVar) {
        $EnvVar -split ':' | ForEach-Object {
            $kv = $_ -split '='
            if ($kv.Count -eq 2) {
                $key = $kv[0]
                $val = $kv[1]
                if ($key.StartsWith('*')) {
                    $key = $key.Substring(1)
                    if ($key -match '\*|\[|\{' -or -not $key.StartsWith('.')) {
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

function Lookup {
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