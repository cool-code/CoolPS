# Core\VisualWidth.ps1

# Test Visual Width by using ANSI cursor position report (ESC[6n)
function Test-VisualWidth {
    param(
        [string]$Text,
        [int]$TimeoutMs = 300
    )
    $ESC = [char]27
    # Use ANSI cursor position report to determine the visual width of the text
    Write-Host -NoNewline "$ESC[8m$Text$ESC[0m$ESC[6n"

    $pos = ""
    $sw = [Diagnostics.Stopwatch]::StartNew()
    try {
        while ($sw.ElapsedMilliseconds -lt $TimeoutMs) {
            # Read cursor report response non-blockingly
            if ([System.Console]::KeyAvailable) {
                $key = [System.Console]::ReadKey($true)
                $char = $key.KeyChar
                $pos += $char
                if ($char -eq 'R') { break }
            }
            else {
                # If no input available, wait a bit before retrying to avoid busy loop
                Start-Sleep -Milliseconds 1
            }
        }
    }
    catch {}
    finally {
        # Ensure cursor is reset and traces are cleared on success, timeout, or exception
        Write-Host -NoNewline "$ESC[G$ESC[K"
        $sw.Stop()
    }

    # Parse the cursor report (should be in format ESC[row;colR)
    if ($pos -match ';(\d+)R') {
        # The column number in the cursor report indicates the visual width + 1 (because cursor is after the text)
        return [int]$matches[1] - 1
    }
    
    # If no coordinate matched, detection failed (timeout or exception)
    # In this case, we return $null to indicate failure,
    # and the caller can decide how to fallback (e.g. config or default value)
    return $null
}

function Test-AmbiguousAsWide {
    # Simple logic:
    # print an ambiguous width character (e.g. 'α')
    # and check how many cells it occupies
    $char = [char]0x03B1 # Greek letter Alpha
    $width = Test-VisualWidth -Text $char
    if ($null -ne $width) {
        return ($width -eq 2)
    }
    return $null
}

function Get-WTAmbiguousAsWide {
    # 1. Define possible config file paths (stable and preview)
    $localAppData = $Env:LOCALAPPDATA
    $paths = @(
        "$localAppData\Packages\Microsoft.WindowsTerminal_8wekyb3d8bbwe\LocalState\settings.json",
        "$localAppData\Packages\Microsoft.WindowsTerminalPreview_8wekyb3d8bbwe\LocalState\settings.json"
    )

    foreach ($path in $paths) {
        if (Test-Path $path) {
            try {
                # 2. Read and parse JSON
                $settings = Get-Content $path -Raw -ErrorAction Stop | ConvertFrom-Json --ErrorAction Stop
                if ($null -eq $settings) { continue }
                # 3. Priority: Specific Profile -> Global Defaults -> Default False
                # Note: In WT, the property is "ambiguousUnicodeWidth", value is "wide" or "narrow"
                $widthSetting = $null
                if ($null -ne $settings.profiles.defaults.ambiguousUnicodeWidth) {
                    $widthSetting = $settings.profiles.defaults.ambiguousUnicodeWidth
                }                
                # If found, return boolean value
                if ($widthSetting -eq "wide") { return $true }
                if ($widthSetting -eq "narrow") { return $false }
            }
            catch {
                continue # On parse failure, try next path
            }
        }
    }
    return $false # Default to False (Narrow)
}

function Get-InitialAmbiguousAsWide {
    # In VSCode integrated terminal, always return False,
    # as it does not support real-time cursor probing.
    if ($Env:TERM_PROGRAM -eq "vscode" -or $Host.Name -match "Visual Studio Code Host") {
        return $false
    }
    # Real-time cursor probing (most accurate, now robust)
    $detected = Test-AmbiguousAsWide  # This is the improved function
    if ($null -ne $detected) {
        return $detected
    }

    # Environment variable (user explicit declaration has high priority)
    if ($null -ne $Env:AMBIGUOUS_AS_WIDE) {
        return $Env:AMBIGUOUS_AS_WIDE -in @('1', 'true', '$true')
    }

    # Read software config (Windows Terminal settings.json)
    # This step is the last fallback
    if ($null -ne $Env:WT_SESSION) {
        $wtSetting = Get-WTAmbiguousAsWide
        if ($null -ne $wtSetting) { return $wtSetting }
    }

    # Final default value
    return $false 
}

function Test-ZWJSupport {
    # 👨‍👩‍👧‍👦 Family sequence (4 Emojis + 3 ZWJ)
    $familyEmoji = [char]::ConvertFromUtf32(0x1F468) + 
    [char]0x200D + 
    [char]::ConvertFromUtf32(0x1F469) + 
    [char]0x200D + 
    [char]::ConvertFromUtf32(0x1F467) + 
    [char]0x200D + 
    [char]::ConvertFromUtf32(0x1F466)

    $width = Test-VisualWidth -Text $familyEmoji
    if ($null -ne $width) {
        return @{
            Support = ($width -eq 2) # Proper ZWJ support if treated as single emoji (width 2)
            Width   = switch ($width) {
                2 { 0 } # Proper ZWJ support, treat as 0-width
                8 { 0 } # No ZWJ support, treat entire sequence as 2 separate emojis (4 chars * 2 width each)
                11 { 1 } # No ZWJ support, but counts each char as width 1 (e.g. some terminals), treat as 1-width
                Default { 0 } # Unexpected width, fallback to 0-width to avoid breaking layouts (best effort)
            }
        }
    }
    # If detection fails, assume no ZWJ support and treat as 0-width (safe fallback)
    return @{ Support = $false; Width = 0 }
}

function Get-InitialZWJSupport {
    # In VSCode integrated terminal, xterm.js has no ZWJ support,
    # but it treats ZWJ as zero-width (no visual effect, just concatenation).
    if ($Env:TERM_PROGRAM -eq "vscode" -or $Host.Name -match "Visual Studio Code Host") {
        return @{ Support = $false; Width = 0; }
    }

    # Windows Terminal has good emoji support including ZWJ.
    if ($null -ne $Env:WT_SESSION) {
        return @{ Support = $true; Width = 0; }
    }

    # Real-time probing on other terminals (most accurate)
    return Test-ZWJSupport
}

# Define SGR (color) regex (ends with m)
$Script:sgrRegex = [Regex]::new("\x1B\[[0-9;]*m", [RegexOptions]::Compiled)
# Define ANSI regex to match all ANSI escape codes, including SGR and others (like cursor movement), for proper splitting
$Script:ansiRegex = [Regex]::new("([\u001B\u009B][[\]()#;?]*(?:(?:(?:[a-zA-Z\d]*(?:;[a-zA-Z\d]*)*)?\u0007)|(?:(?:\d{1,4}(?:;\d{0,4})*)?[\dA-PR-TZcf-ntqry=><~])))", [RegexOptions]::Compiled)
# Define regex to detect full-width characters (CJK, Emoji, etc.)
$Script:fullWidthRegex = [Regex]::new('[\u1100-\u11ff\u2e80-\ua4cf\uac00-\ud7af\uf900-\ufaff\ufe30-\ufe4f\uff00-\uffee]', [RegexOptions]::Compiled)
# Define regex to detect Unicode 17 Emojis (for better emoji width handling)
$Script:emojiRegex = [Regex]::new('[#*0-9]\uFE0F?\u20E3|[\xA9\xAE\u203C\u2049\u2122\u2139\u2194-\u2199\u21A9\u21AA\u231A\u231B\u2328\u23CF\u23ED-\u23EF\u23F1\u23F2\u23F8-\u23FA\u24C2\u25AA\u25AB\u25B6\u25C0\u25FB\u25FC\u25FE\u2600-\u2604\u260E\u2611\u2614\u2615\u2618\u2620\u2622\u2623\u2626\u262A\u262E\u262F\u2638-\u263A\u2640\u2642\u2648-\u2653\u265F\u2660\u2663\u2665\u2666\u2668\u267B\u267E\u267F\u2692\u2694-\u2697\u2699\u269B\u269C\u26A0\u26A7\u26AA\u26B0\u26B1\u26BD\u26BE\u26C4\u26C8\u26CF\u26D1\u26E9\u26F0-\u26F5\u26F7\u26F8\u26FA\u2702\u2708\u2709\u270F\u2712\u2714\u2716\u271D\u2721\u2733\u2734\u2744\u2747\u2757\u2763\u27A1\u2934\u2935\u2B05-\u2B07\u2B1B\u2B1C\u2B55\u3030\u303D\u3297\u3299]\uFE0F?|[\u261D\u270C\u270D](?:\uD83C[\uDFFB-\uDFFF]|\uFE0F)?|[\u270A\u270B](?:\uD83C[\uDFFB-\uDFFF])?|[\u23E9-\u23EC\u23F0\u23F3\u25FD\u2693\u26A1\u26AB\u26C5\u26CE\u26D4\u26EA\u26FD\u2705\u2728\u274C\u274E\u2753-\u2755\u2795-\u2797\u27B0\u27BF\u2B50]|\u26D3\uFE0F?(?:\u200D\uD83D\uDCA5)?|\u26F9(?:\uD83C[\uDFFB-\uDFFF]|\uFE0F)?(?:\u200D[\u2640\u2642]\uFE0F?)?|\u2764\uFE0F?(?:\u200D(?:\uD83D\uDD25|\uD83E\uDE79))?|\uD83C(?:[\uDC04\uDD70\uDD71\uDD7E\uDD7F\uDE02\uDE37\uDF21\uDF24-\uDF2C\uDF36\uDF7D\uDF96\uDF97\uDF99-\uDF9B\uDF9E\uDF9F\uDFCD\uDFCE\uDFD4-\uDFDF\uDFF5\uDFF7]\uFE0F?|[\uDF85\uDFC2\uDFC7](?:\uD83C[\uDFFB-\uDFFF])?|[\uDFC4\uDFCA](?:\uD83C[\uDFFB-\uDFFF])?(?:\u200D[\u2640\u2642]\uFE0F?)?|[\uDFCB\uDFCC](?:\uD83C[\uDFFB-\uDFFF]|\uFE0F)?(?:\u200D[\u2640\u2642]\uFE0F?)?|[\uDCCF\uDD8E\uDD91-\uDD9A\uDE01\uDE1A\uDE2F\uDE32-\uDE36\uDE38-\uDE3A\uDE50\uDE51\uDF00-\uDF20\uDF2D-\uDF35\uDF37-\uDF43\uDF45-\uDF4A\uDF4C-\uDF7C\uDF7E-\uDF84\uDF86-\uDF93\uDFA0-\uDFC1\uDFC5\uDFC6\uDFC8\uDFC9\uDFCF-\uDFD3\uDFE0-\uDFF0\uDFF8-\uDFFF]|\uDDE6\uD83C[\uDDE8-\uDDEC\uDDEE\uDDF1\uDDF2\uDDF4\uDDF6-\uDDFA\uDDFC\uDDFD\uDDFF]|\uDDE7\uD83C[\uDDE6\uDDE7\uDDE9-\uDDEF\uDDF1-\uDDF4\uDDF6-\uDDF9\uDDFB\uDDFC\uDDFE\uDDFF]|\uDDE8\uD83C[\uDDE6\uDDE8\uDDE9\uDDEB-\uDDEE\uDDF0-\uDDF7\uDDFA-\uDDFF]|\uDDE9\uD83C[\uDDEA\uDDEC\uDDEF\uDDF0\uDDF2\uDDF4\uDDFF]|\uDDEA\uD83C[\uDDE6\uDDE8\uDDEA\uDDEC\uDDED\uDDF7-\uDDFA]|\uDDEB\uD83C[\uDDEE-\uDDF0\uDDF2\uDDF4\uDDF7]|\uDDEC\uD83C[\uDDE6\uDDE7\uDDE9-\uDDEE\uDDF1-\uDDF3\uDDF5-\uDDFA\uDDFC\uDDFE]|\uDDED\uD83C[\uDDF0\uDDF2\uDDF3\uDDF7\uDDF9\uDDFA]|\uDDEE\uD83C[\uDDE8-\uDDEA\uDDF1-\uDDF4\uDDF6-\uDDF9]|\uDDEF\uD83C[\uDDEA\uDDF2\uDDF4\uDDF5]|\uDDF0\uD83C[\uDDEA\uDDEC-\uDDEE\uDDF2\uDDF3\uDDF5\uDDF7\uDDFC\uDDFE\uDDFF]|\uDDF1\uD83C[\uDDE6-\uDDE8\uDDEE\uDDF0\uDDF7-\uDDFB\uDDFE]|\uDDF2\uD83C[\uDDE6\uDDE8-\uDDED\uDDF0-\uDDFF]|\uDDF3\uD83C[\uDDE6\uDDE8\uDDEA-\uDDEC\uDDEE\uDDF1\uDDF4\uDDF5\uDDF7\uDDFA\uDDFF]|\uDDF4\uD83C\uDDF2|\uDDF5\uD83C[\uDDE6\uDDEA-\uDDED\uDDF0-\uDDF3\uDDF7-\uDDF9\uDDFC\uDDFE]|\uDDF6\uD83C\uDDE6|\uDDF7\uD83C[\uDDEA\uDDF4\uDDF8\uDDFA\uDDFC]|\uDDF8\uD83C[\uDDE6-\uDDEA\uDDEC-\uDDF4\uDDF7-\uDDF9\uDDFB\uDDFD-\uDDFF]|\uDDF9\uD83C[\uDDE6\uDDE8\uDDE9\uDDEB-\uDDED\uDDEF-\uDDF4\uDDF7\uDDF9\uDDFB\uDDFC\uDDFF]|\uDDFA\uD83C[\uDDE6\uDDEC\uDDF2\uDDF3\uDDF8\uDDFE\uDDFF]|\uDDFB\uD83C[\uDDE6\uDDE8\uDDEA\uDDEC\uDDEE\uDDF3\uDDFA]|\uDDFC\uD83C[\uDDEB\uDDF8]|\uDDFD\uD83C\uDDF0|\uDDFE\uD83C[\uDDEA\uDDF9]|\uDDFF\uD83C[\uDDE6\uDDF2\uDDFC]|\uDF44(?:\u200D\uD83D\uDFEB)?|\uDF4B(?:\u200D\uD83D\uDFE9)?|\uDFC3(?:\uD83C[\uDFFB-\uDFFF])?(?:\u200D(?:[\u2640\u2642]\uFE0F?(?:\u200D\u27A1\uFE0F?)?|\u27A1\uFE0F?))?|\uDFF3\uFE0F?(?:\u200D(?:\u26A7\uFE0F?|\uD83C\uDF08))?|\uDFF4(?:\u200D\u2620\uFE0F?|\uDB40\uDC67\uDB40\uDC62\uDB40(?:\uDC65\uDB40\uDC6E\uDB40\uDC67|\uDC73\uDB40\uDC63\uDB40\uDC74|\uDC77\uDB40\uDC6C\uDB40\uDC73)\uDB40\uDC7F)?)|\uD83D(?:[\uDC3F\uDCFD\uDD49\uDD4A\uDD6F\uDD70\uDD73\uDD76-\uDD79\uDD87\uDD8A-\uDD8D\uDDA5\uDDA8\uDDB1\uDDB2\uDDBC\uDDC2-\uDDC4\uDDD1-\uDDD3\uDDDC-\uDDDE\uDDE1\uDDE3\uDDE8\uDDEF\uDDF3\uDDFA\uDECB\uDECD-\uDECF\uDEE0-\uDEE5\uDEE9\uDEF0\uDEF3]\uFE0F?|[\uDC42\uDC43\uDC46-\uDC50\uDC66\uDC67\uDC6B-\uDC6D\uDC72\uDC74-\uDC76\uDC78\uDC7C\uDC83\uDC85\uDC8F\uDC91\uDCAA\uDD7A\uDD95\uDD96\uDE4C\uDE4F\uDEC0\uDECC](?:\uD83C[\uDFFB-\uDFFF])?|[\uDC6E-\uDC71\uDC73\uDC77\uDC81\uDC82\uDC86\uDC87\uDE45-\uDE47\uDE4B\uDE4D\uDE4E\uDEA3\uDEB4\uDEB5](?:\uD83C[\uDFFB-\uDFFF])?(?:\u200D[\u2640\u2642]\uFE0F?)?|[\uDD74\uDD90](?:\uD83C[\uDFFB-\uDFFF]|\uFE0F)?|[\uDC00-\uDC07\uDC09-\uDC14\uDC16-\uDC25\uDC27-\uDC3A\uDC3C-\uDC3E\uDC40\uDC44\uDC45\uDC51-\uDC65\uDC6A\uDC79-\uDC7B\uDC7D-\uDC80\uDC84\uDC88-\uDC8E\uDC90\uDC92-\uDCA9\uDCAB-\uDCFC\uDCFF-\uDD3D\uDD4B-\uDD4E\uDD50-\uDD67\uDDA4\uDDFB-\uDE2D\uDE2F-\uDE34\uDE37-\uDE41\uDE43\uDE44\uDE48-\uDE4A\uDE80-\uDEA2\uDEA4-\uDEB3\uDEB7-\uDEBF\uDEC1-\uDEC5\uDED0-\uDED2\uDED5-\uDED8\uDEDC-\uDEDF\uDEEB\uDEEC\uDEF4-\uDEFC\uDFE0-\uDFEB\uDFF0]|\uDC08(?:\u200D\u2B1B)?|\uDC15(?:\u200D\uD83E\uDDBA)?|\uDC26(?:\u200D(?:\u2B1B|\uD83D\uDD25))?|\uDC3B(?:\u200D\u2744\uFE0F?)?|\uDC41\uFE0F?(?:\u200D\uD83D\uDDE8\uFE0F?)?|\uDC68(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:\uDC8B\u200D\uD83D)?\uDC68|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDC68\uDC69]\u200D\uD83D(?:\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?)|[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?)|\uD83E(?:[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3]))|\uD83C(?:\uDFFB(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:\uDC8B\u200D\uD83D)?\uDC68\uD83C[\uDFFB-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC30\u200D\uD83D\uDC68\uD83C[\uDFFC-\uDFFF])|\uD83E(?:[\uDD1D\uDEEF]\u200D\uD83D\uDC68\uD83C[\uDFFC-\uDFFF]|[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3])))?|\uDFFC(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:\uDC8B\u200D\uD83D)?\uDC68\uD83C[\uDFFB-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC30\u200D\uD83D\uDC68\uD83C[\uDFFB\uDFFD-\uDFFF])|\uD83E(?:[\uDD1D\uDEEF]\u200D\uD83D\uDC68\uD83C[\uDFFB\uDFFD-\uDFFF]|[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3])))?|\uDFFD(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:\uDC8B\u200D\uD83D)?\uDC68\uD83C[\uDFFB-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC30\u200D\uD83D\uDC68\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF])|\uD83E(?:[\uDD1D\uDEEF]\u200D\uD83D\uDC68\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF]|[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3])))?|\uDFFE(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:\uDC8B\u200D\uD83D)?\uDC68\uD83C[\uDFFB-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC30\u200D\uD83D\uDC68\uD83C[\uDFFB-\uDFFD\uDFFF])|\uD83E(?:[\uDD1D\uDEEF]\u200D\uD83D\uDC68\uD83C[\uDFFB-\uDFFD\uDFFF]|[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3])))?|\uDFFF(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:\uDC8B\u200D\uD83D)?\uDC68\uD83C[\uDFFB-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC30\u200D\uD83D\uDC68\uD83C[\uDFFB-\uDFFE])|\uD83E(?:[\uDD1D\uDEEF]\u200D\uD83D\uDC68\uD83C[\uDFFB-\uDFFE]|[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3])))?))?|\uDC69(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:\uDC8B\u200D\uD83D)?[\uDC68\uDC69]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?|\uDC69\u200D\uD83D(?:\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?))|\uD83E(?:[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3]))|\uD83C(?:\uDFFB(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:[\uDC68\uDC69]|\uDC8B\u200D\uD83D[\uDC68\uDC69])\uD83C[\uDFFB-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC30\u200D\uD83D\uDC69\uD83C[\uDFFC-\uDFFF])|\uD83E(?:[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3]|\uDD1D\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFC-\uDFFF]|\uDEEF\u200D\uD83D\uDC69\uD83C[\uDFFC-\uDFFF])))?|\uDFFC(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:[\uDC68\uDC69]|\uDC8B\u200D\uD83D[\uDC68\uDC69])\uD83C[\uDFFB-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC30\u200D\uD83D\uDC69\uD83C[\uDFFB\uDFFD-\uDFFF])|\uD83E(?:[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3]|\uDD1D\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFB\uDFFD-\uDFFF]|\uDEEF\u200D\uD83D\uDC69\uD83C[\uDFFB\uDFFD-\uDFFF])))?|\uDFFD(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:[\uDC68\uDC69]|\uDC8B\u200D\uD83D[\uDC68\uDC69])\uD83C[\uDFFB-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC30\u200D\uD83D\uDC69\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF])|\uD83E(?:[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3]|\uDD1D\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF]|\uDEEF\u200D\uD83D\uDC69\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF])))?|\uDFFE(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:[\uDC68\uDC69]|\uDC8B\u200D\uD83D[\uDC68\uDC69])\uD83C[\uDFFB-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC30\u200D\uD83D\uDC69\uD83C[\uDFFB-\uDFFD\uDFFF])|\uD83E(?:[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3]|\uDD1D\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFB-\uDFFD\uDFFF]|\uDEEF\u200D\uD83D\uDC69\uD83C[\uDFFB-\uDFFD\uDFFF])))?|\uDFFF(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D\uD83D(?:[\uDC68\uDC69]|\uDC8B\u200D\uD83D[\uDC68\uDC69])\uD83C[\uDFFB-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC30\u200D\uD83D\uDC69\uD83C[\uDFFB-\uDFFE])|\uD83E(?:[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3]|\uDD1D\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFB-\uDFFE]|\uDEEF\u200D\uD83D\uDC69\uD83C[\uDFFB-\uDFFE])))?))?|\uDD75(?:\uD83C[\uDFFB-\uDFFF]|\uFE0F)?(?:\u200D[\u2640\u2642]\uFE0F?)?|\uDE2E(?:\u200D\uD83D\uDCA8)?|\uDE35(?:\u200D\uD83D\uDCAB)?|\uDE36(?:\u200D\uD83C\uDF2B\uFE0F?)?|\uDE42(?:\u200D[\u2194\u2195]\uFE0F?)?|\uDEB6(?:\uD83C[\uDFFB-\uDFFF])?(?:\u200D(?:[\u2640\u2642]\uFE0F?(?:\u200D\u27A1\uFE0F?)?|\u27A1\uFE0F?))?)|\uD83E(?:[\uDD0C\uDD0F\uDD18-\uDD1F\uDD30-\uDD34\uDD36\uDD77\uDDB5\uDDB6\uDDBB\uDDD2\uDDD3\uDDD5\uDEC3-\uDEC5\uDEF0\uDEF2-\uDEF8](?:\uD83C[\uDFFB-\uDFFF])?|[\uDD26\uDD35\uDD37-\uDD39\uDD3C-\uDD3E\uDDB8\uDDB9\uDDCD\uDDCF\uDDD4\uDDD6-\uDDDD](?:\uD83C[\uDFFB-\uDFFF])?(?:\u200D[\u2640\u2642]\uFE0F?)?|[\uDDDE\uDDDF](?:\u200D[\u2640\u2642]\uFE0F?)?|[\uDD0D\uDD0E\uDD10-\uDD17\uDD20-\uDD25\uDD27-\uDD2F\uDD3A\uDD3F-\uDD45\uDD47-\uDD76\uDD78-\uDDB4\uDDB7\uDDBA\uDDBC-\uDDCC\uDDD0\uDDE0-\uDDFF\uDE70-\uDE7C\uDE80-\uDE8A\uDE8E-\uDEC2\uDEC6\uDEC8\uDECD-\uDEDC\uDEDF-\uDEEA\uDEEF]|\uDDCE(?:\uD83C[\uDFFB-\uDFFF])?(?:\u200D(?:[\u2640\u2642]\uFE0F?(?:\u200D\u27A1\uFE0F?)?|\u27A1\uFE0F?))?|\uDDD1(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\uD83C[\uDF3E\uDF73\uDF7C\uDF84\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3\uDE70]|\uDD1D\u200D\uD83E\uDDD1|\uDDD1\u200D\uD83E\uDDD2(?:\u200D\uD83E\uDDD2)?|\uDDD2(?:\u200D\uD83E\uDDD2)?))|\uD83C(?:\uDFFB(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D(?:\uD83D\uDC8B\u200D)?\uD83E\uDDD1\uD83C[\uDFFC-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF84\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC30\u200D\uD83E\uDDD1\uD83C[\uDFFC-\uDFFF])|\uD83E(?:[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3\uDE70]|\uDD1D\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFF]|\uDEEF\u200D\uD83E\uDDD1\uD83C[\uDFFC-\uDFFF])))?|\uDFFC(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D(?:\uD83D\uDC8B\u200D)?\uD83E\uDDD1\uD83C[\uDFFB\uDFFD-\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF84\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC30\u200D\uD83E\uDDD1\uD83C[\uDFFB\uDFFD-\uDFFF])|\uD83E(?:[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3\uDE70]|\uDD1D\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFF]|\uDEEF\u200D\uD83E\uDDD1\uD83C[\uDFFB\uDFFD-\uDFFF])))?|\uDFFD(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D(?:\uD83D\uDC8B\u200D)?\uD83E\uDDD1\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF84\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC30\u200D\uD83E\uDDD1\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF])|\uD83E(?:[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3\uDE70]|\uDD1D\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFF]|\uDEEF\u200D\uD83E\uDDD1\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF])))?|\uDFFE(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D(?:\uD83D\uDC8B\u200D)?\uD83E\uDDD1\uD83C[\uDFFB-\uDFFD\uDFFF]|\uD83C[\uDF3E\uDF73\uDF7C\uDF84\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC30\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFD\uDFFF])|\uD83E(?:[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3\uDE70]|\uDD1D\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFF]|\uDEEF\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFD\uDFFF])))?|\uDFFF(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F?|\u2764\uFE0F?\u200D(?:\uD83D\uDC8B\u200D)?\uD83E\uDDD1\uD83C[\uDFFB-\uDFFE]|\uD83C[\uDF3E\uDF73\uDF7C\uDF84\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uDC30\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFE])|\uD83E(?:[\uDDAF\uDDBC\uDDBD](?:\u200D\u27A1\uFE0F?)?|[\uDDB0-\uDDB3\uDE70]|\uDD1D\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFF]|\uDEEF\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFE])))?))?|\uDEF1(?:\uD83C(?:\uDFFB(?:\u200D\uD83E\uDEF2\uD83C[\uDFFC-\uDFFF])?|\uDFFC(?:\u200D\uD83E\uDEF2\uD83C[\uDFFB\uDFFD-\uDFFF])?|\uDFFD(?:\u200D\uD83E\uDEF2\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF])?|\uDFFE(?:\u200D\uD83E\uDEF2\uD83C[\uDFFB-\uDFFD\uDFFF])?|\uDFFF(?:\u200D\uD83E\uDEF2\uD83C[\uDFFB-\uDFFE])?))?)', [RegexOptions]::Compiled)
# Define AmbiguousWidth regex (includes Greek, Cyrillic, some symbols, etc.)
$Script:ambigWidthRegex = [Regex]::new('[\u00a1\u00a4\u00a7\u00a8\u00aa\u00ad\u00ae\u00b0-\u00b4\u00b6\u00b7\u00b8\u00ba\u00bc-\u00be\u00bf\u00c6\u00d0\u00d7\u00d8\u00de-\u00e1\u00e6\u00e8-\u00ea\u00ec-\u00ed\u00f0\u00f2-\u00f3\u00f7-\u00fa\u00fc\u00fe\u0101\u0111\u0113\u011b\u0126\u012b\u0131-\u0133\u0138\u013f\u0141\u0142\u0144\u0148\u0149\u014a\u014d\u0152\u0153\u0166\u0167\u016b\u01ce\u01d0\u01d2\u01d4\u01d6\u01d8\u01da\u01dc\u0251\u0261\u02c4\u02c7\u02c9-\u02cb\u02cd\u02d0\u02d8\u02d9\u02da\u02db\u02dd\u02df\u0300-\u036f\u0391-\u03a9\u03b1-\u03c9\u0401\u0410-\u044f\u0451\u2010\u2013-\u2016\u2018\u2019\u201c\u201d\u2020-\u2022\u2024-\u2027\u2030\u2032\u2033\u2035\u203b\u203e\u2074\u207f\u2081-\u2084\u20ac\u2103\u2105\u2109\u2113\u2116\u2121\u2122\u2126\u212b\u2153-\u2154\u215b-\u215e\u2160-\u216b\u2170-\u217b\u2189\u2190-\u2199\u21b8\u21b9\u21d2\u21d4\u21e7\u2200\u2202\u2203\u2207\u2208\u220b\u220f\u2211\u2215\u221a\u221d-\u221f\u2220\u2223\u2225\u2227-\u222c\u222e\u2234-\u2237\u223d\u2248\u224c\u2252\u2260\u2261\u2264\u2265\u2266\u2267\u226a\u226b\u226e\u226f\u2282\u2283\u2286\u2287\u2295\u2299\u22a5\u22bf\u2312\u2460-\u24e9\u24eb-\u254b\u2550-\u2573\u2580-\u258f\u2592-\u2595\u25a0\u25a1\u25a3-\u25a9\u25b2\u25b3\u25b6\u25b7\u25bc\u25bd\u25c0\u25c1\u25c6-\u25c8\u25cb\u25ce-\u25d1\u25e2-\u25e5\u25ef\u2605\u2606\u2609\u260e\u260f\u2614\u2615\u261c\u261e\u2640\u2642\u2660\u2661\u2663-\u2665\u2667-\u266a\u266c-\u266d\u266f\u273d\u2776-\u277f\ue000-\uf8ff\ufe00-\ufe0f\ufffd]', [RegexOptions]::Compiled)
 
# Global flags for ambiguous width and ZWJ support, initialized at startup
$Script:AmbiguousAsWide = Get-InitialAmbiguousAsWide
$Script:ZWJ = [PSCustomObject](Get-InitialZWJSupport)

Add-Type -AssemblyName System.Runtime.Caching
if ($null -eq $Script:WidthCache) {
    $Script:WidthCache = [System.Runtime.Caching.MemoryCache]::Default
}

function Get-VisualElementsAndWidths {
    param([string]$Text)

    $elements = [List[string]]::new()
    $widths = [List[int]]::new()

    if ([string]::IsNullOrEmpty($Text)) { return $elements, $widths }

    $cacheKey = "E_$($Script:ZWJ.Support)_$($Script:ZWJ.Width)_$($Script:AmbiguousAsWide)_$Text"
    $cached = $Script:WidthCache.Get($cacheKey)

    if ($null -ne $cached) {
        return $cached.elements, $cached.widths
    }

    # Use regex Split to iterate, only keep ANSI codes with color features
    # Split text by all ANSI codes
    $parts = $Script:ansiRegex.Split($Text)

    foreach ($part in $parts) {
        if ([string]::IsNullOrEmpty($part)) { continue }

        # If it's an ANSI code
        if ($Script:ansiRegex.IsMatch($part)) {
            # Only keep SGR (color/bold) codes as 0-width elements, discard others
            if ($Script:sgrRegex.IsMatch($part)) {
                [void]$elements.Add($part)
                [void]$widths.Add(0)
            }
            continue
        }

        # --- Normal text processing ---
        $it = [StringInfo]::GetTextElementEnumerator($part)
        while ($it.MoveNext()) {
            $char = $it.GetTextElement()
            # ZWJ logic
            if ($Script:ZWJ.Support -and $elements.Count -gt 0 -and (([char[]]$elements[-1])[-1] -eq 0x200D -or ([char[]]$char)[0] -eq 0x200D)) {
                $elements[$elements.Count - 1] += $char
            }
            elseif (-not $Script:ZWJ.Support -and $char.Contains([char]0x200D)) {
                $emojiParts = $char -split [char]0x200D
                for ($i = 0; $i -lt $emojiParts.Length; $i++) {
                    if ($emojiParts[$i][0] -eq 0x200D) {
                        $elements[$elements.Count - 1] += $emojiParts[$i] # Append ZWJ to previous element
                        # If ZWJ is not supported, treat the ZWJ character itself according to the configured width (0 or 1)
                        $widths[$elements.Count - 1] += $Script:ZWJ.Width
                    }
                    else {
                        [void]$elements.Add($emojiParts[$i]) # Add the emoji part as a new element
                        [void]$widths.Add(2) # Emoji parts are treated as width 2 regardless of ZWJ support, to avoid breaking layouts (best effort)
                    }
                }
            }
            else {
                [void]$elements.Add($char)
                if ($Script:fullWidthRegex.IsMatch($char)) {
                    # Full-width characters (CJK, Emoji, etc.) treated as width 2
                    [void]$widths.Add(2)
                }
                elseif ($char.Contains([char]0xFE0F)) {
                    # Emoji variation selector (ZWJ sequence) treated as width 2
                    [void]$widths.Add(2)
                }
                elseif ($char.Contains([char]0xFE0E)) {
                    # Text variation selector treated as width 1
                    [void]$widths.Add(1)
                }
                elseif ($Script:AmbiguousAsWide -and $Script:ambigWidthRegex.IsMatch($char)) {
                    # Ambiguous width characters (e.g., Greek, Cyrillic) treated as width 2 if global flag is set
                    [void]$widths.Add(2)
                }
                elseif ($Script:emojiRegex.IsMatch($char)) {
                    # Emoji characters treated as width 2
                    [void]$widths.Add(2)
                }
                else {
                    # Normal ASCII and combining marks
                    # Combining marks may have .Length > 1, but visually follow the previous character, width is 1
                    [void]$widths.Add(1)
                }
            }
        }
    }

    # Cache the result with a sliding expiration of 5 minutes
    $policy = New-Object System.Runtime.Caching.CacheItemPolicy
    $policy.SlidingExpiration = [TimeSpan]::FromMinutes(5)

    $cacheEntry = @{
        elements = $elements.ToArray()
        widths   = $widths.ToArray()
    }

    $Script:WidthCache.Set($cacheKey, $cacheEntry, $policy)

    return $cacheEntry.elements, $cacheEntry.widths
}

function Get-VisualWidth {
    param([string]$Text)
    $elements, $widths = Get-VisualElementsAndWidths -Text $Text
    $totalWidth = 0
    foreach ($w in $widths) { $totalWidth += $w }
    return $totalWidth
}

function VisualWidthPad {
    <#
    .SYNOPSIS
        Pads a string to a specific visual width.
    .PARAMETER Alignment
        -1: Left (Pad right)
         0: Center
         1: Right (Pad left)
    #>
    param(
        [string]$Text,
        [int]$Width,
        [int]$Alignment
    )

    $currentWidth = Get-VisualWidth -Text $Text
    
    $padTotal = $Width - $currentWidth
    if ($padTotal -le 0) { return $Text }

    switch ($Alignment) {
        -1 { 
            # Left: Text + Spaces
            return $Text + (" " * $padTotal) 
        }
        0 { 
            # Center: HalfSpaces + Text + HalfSpaces
            $leftPad = [Math]::Floor($padTotal / 2)
            $rightPad = $padTotal - $leftPad
            return (" " * $leftPad) + $Text + (" " * $rightPad)
        }
        1 { 
            # Right: Spaces + Text
            return (" " * $padTotal) + $Text 
        }
    }
}

function vPadLeft {
    param([string]$Text, [int]$Width)
    return VisualWidthPad -Text $Text -Width $Width -Alignment 1
}

function vPadCenter {
    param([string]$Text, [int]$Width)
    return VisualWidthPad -Text $Text -Width $Width -Alignment 0
}

function vPadRight {
    param([string]$Text, [int]$Width)
    return VisualWidthPad -Text $Text -Width $Width -Alignment -1
}

$Script:MergeSlashRegex = [Regex]::new('\/+((' + $Script:sgrRegex + ')*\/+)+', [RegexOptions]::Compiled)
$Script:TrailingSlashRegex = [Regex]::new('\/(' + $Script:sgrRegex + ')*$', [RegexOptions]::Compiled)
$Script:TrailingColorRegex = [Regex]::new('(.*)(' + $Script:sgrRegex + ')$', [RegexOptions]::Compiled -bor [RegexOptions]::Singleline)

function Format-DirName {
    param ([string]$Text)
    if ([string]::IsNullOrEmpty($Text)) { return "/" }

    # 1. Path normalization: replace backslashes with slashes, merge consecutive slashes
    $Text = $Text.Replace('\', '/')
    # 2. Merge consecutive slashes, also merge if color codes are between slashes (e.g., "///", "/\e[31m/\e[0m/")
    $Text = $Script:MergeSlashRegex.Replace($Text, '/')
    # 3. Ensure there is a trailing slash, or a slash before trailing color code
    if ($Script:TrailingSlashRegex.IsMatch($Text)) {
        return $Text
    }

    # 4. If there is a trailing color code but no slash, add a slash before the color code
    $match = $Script:TrailingColorRegex.Match($Text)
    if ($match.Success) {
        return $match.Groups[1].Value + "/" + $match.Groups[2].Value
    }
    # 5. Otherwise, just add a slash
    return $Text + "/"
}

function VisualWidthTruncate {
    <#
    .SYNOPSIS
        Intelligent semantic truncation for file names and paths.
        Supports CJK characters, Emojis (ZWJ), and extension preservation.
    .PARAMETER Text
        The original string to truncate.
    .PARAMETER MaxWidth
        The maximum visual width (ASCII = 1, CJK/Emoji = 2).
    .PARAMETER Mode
        0: File mode (Preserve extension).
        1: Directory mode (Add trailing slash).
        2: Raw mode (Internal use for recursive base name truncation).
        3: Force truncate (Ignore extension).
    #>
    param(
        [string]$Text,
        [int]$MaxWidth,
        [int]$Mode = 0
    )
    
    if ($Mode -eq 1) { $Text = Format-DirName -Text $Text }

    if ($MaxWidth -lt 0) { return $Text }

    $elements, $widths = Get-VisualElementsAndWidths -Text $Text

    $totalWidth = 0
    foreach ($width in $widths) { $totalWidth += $width }

    # If within limit, return original (with optional slash)
    if ($totalWidth -le $MaxWidth) { return $Text }
 
    # --- Mode Handling ---
    
    # File Mode: Preserve extension
    if ($Mode -eq 0) {
        $ext = [Path]::GetExtension($Text)
        $base = [Path]::GetFileNameWithoutExtension($Text)
        
        # If extension itself is too long, fallback to force truncate
        $limitForBase = $MaxWidth - (Get-VisualWidth -Text $ext)
        if ($limitForBase -lt 3) {
            return VisualWidthTruncate -Text $Text -MaxWidth $MaxWidth -Mode 3
        }
        
        # Recursively truncate the base name and append extension
        return (VisualWidthTruncate -Text $base -MaxWidth $limitForBase -Mode 2) + $ext
    }

    # --- Truncation Logic ---
    # Reserve space for dots: Mode 1 (dir) needs at least 2 dots + '/', Mode 2 (internal) needs 1 dot, others 2 dots
    $reserve = switch ($Mode) {
        1 { 3 } # "../"
        2 { 1 } # "."
        default { 2 } # ".."
    }
    
    $limit = $MaxWidth - $reserve
    $result = ""
    $currentWidth = 0
    $i = 0

    # Add beginning SGR (color/bold) codes
    for (; $i -lt $elements.Count; $i++) {
        if ($widths[$i] -gt 0) { break }
        $result += $elements[$i]
    }

    for (; $i -lt $elements.Count; $i++) {
        if ($currentWidth + $widths[$i] -gt $limit) { break }
        $result += $elements[$i]
        $currentWidth += $widths[$i]
    }
    
    # --- Precision Padding with Dots ---
    $dotCount = $MaxWidth - $currentWidth
    if ($Mode -eq 1) {
        $result += ("." * ($dotCount - 1)) + "/"
    }
    else {
        $result += ("." * $dotCount)
    }

    # Add remaining SGR (color/bold) codes
    for (; $i -lt $elements.Count; $i++) {
        if ($widths[$i] -gt 0) { continue }
        $result += $elements[$i]
    }
    return $result
}

# --- Unified formatting entry function ---
function Format-VisualWidthString {
    <#
    .SYNOPSIS
        Unified entry point for visual width operations.
    .PARAMETER Mode
        Available: PadLeft, PadCenter, PadRight, TruncateFile, TruncateDir
    #>
    param(
        [Parameter(Mandatory = $true)]
        [string]$Text,
        
        [Parameter(Mandatory = $true)]
        [int]$VisualWidth,
        
        [Parameter(Mandatory = $true)]
        [ValidateSet("PadLeft", "PadCenter", "PadRight", "TruncateFile", "TruncateDir")]
        [string]$Mode
    )

    switch ($Mode) {
        "PadLeft" { return VisualWidthPad -Text $Text -Width $VisualWidth -Alignment -1 }
        "PadCenter" { return VisualWidthPad -Text $Text -Width $VisualWidth -Alignment 0 }
        "PadRight" { return VisualWidthPad -Text $Text -Width $VisualWidth -Alignment 1 }
        "TruncateFile" { return VisualWidthTruncate -Text $Text -MaxWidth $VisualWidth -Mode 0 }
        "TruncateDir" { return VisualWidthTruncate -Text $Text -MaxWidth $VisualWidth -Mode 1 }
    }
}
