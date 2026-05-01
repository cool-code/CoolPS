# Cool localization helper

function Initialize-Localization {
    $locale = if ($env:LANG) { $env:LANG } else { (Get-Culture).Name }
    $localizedRoot = Join-Path $PSScriptRoot "../Localized"

    $script:LocalizedMessages = try {
        Import-LocalizedData -FileName "Messages" -UICulture $locale -BaseDirectory $localizedRoot -ErrorAction Stop
    }
    catch {
        $base = $locale.Split('-')[0]
        $fallbackLocale = switch ($base) {
            'zh' { 'zh-CN' }
            'ja' { 'ja-JP' }
            'ko' { 'ko-KR' }
            default { 'en-US' }
        }
        Import-LocalizedData -FileName "Messages" -UICulture $fallbackLocale -BaseDirectory $localizedRoot -ErrorAction SilentlyContinue
    }
}

Initialize-Localization

function script:Get-LocalizedString {
    param(
        [Parameter(Mandatory = $true)][string]$Key,
        [Parameter(ValueFromRemainingArguments = $true)][object[]]$Args
    )
    $msg = $script:LocalizedMessages[$Key]
    if ($null -eq $msg) { $msg = $Key }
    if ($Args) { return ($msg -f $Args) }
    return $msg
}
