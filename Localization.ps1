# Cool localization helper

$Script:Translations = @{
    'en-US' = @{
        'LSColorsCacheUpdated'  = 'LS_COLORS cache updated!'
        'LSIconsCacheUpdated'   = 'LS_ICONS cache updated!'
        'CoolUsage'             = 'Usage: cool update [colors|icons]'
        'UnknownCoolSubcommand' = 'Unknown subcommand: {0}. Available: [colors|icons]'
        'UnknownCoolCommand'    = 'Unknown command: {0}. Available commands: update [colors|icons]'
    }
    'zh-CN' = @{
        'LSColorsCacheUpdated'  = 'LS_COLORS 缓存已更新！'
        'LSIconsCacheUpdated'   = 'LS_ICONS 缓存已更新！'
        'CoolUsage'             = '用法: cool update [colors|icons]'
        'UnknownCoolSubcommand' = '未知子命令: {0}. 可用子命令: [colors|icons]'
        'UnknownCoolCommand'    = '未知命令: {0}. 可用命令: update [colors|icons]'
    }
    'zh-TW' = @{
        'LSColorsCacheUpdated'  = 'LS_COLORS 快取已更新！'
        'LSIconsCacheUpdated'   = 'LS_ICONS 快取已更新！'
        'CoolUsage'             = '用法: cool update [colors|icons]'
        'UnknownCoolSubcommand' = '未知子命令: {0}. 可用子命令: [colors|icons]'
        'UnknownCoolCommand'    = '未知命令: {0}. 可用命令: update [colors|icons]'
    }
    'ja-JP' = @{
        'LSColorsCacheUpdated'  = 'LS_COLORS キャッシュを更新しました！'
        'LSIconsCacheUpdated'   = 'LS_ICONS キャッシュを更新しました！'
        'CoolUsage'             = '使い方: cool update [colors|icons]'
        'UnknownCoolSubcommand' = '不明なサブコマンド: {0}. 使用可能: [colors|icons]'
        'UnknownCoolCommand'    = '不明なコマンド: {0}. 使用可能なコマンド: update [colors|icons]'
    }
    'ko-KR' = @{
        'LSColorsCacheUpdated'  = 'LS_COLORS 캐시가 업데이트되었습니다!'
        'LSIconsCacheUpdated'   = 'LS_ICONS 캐시가 업데이트되었습니다!'
        'CoolUsage'             = '사용법: cool update [colors|icons]'
        'UnknownCoolSubcommand' = '알 수 없는 하위 명령: {0}. 사용 가능: [colors|icons]'
        'UnknownCoolCommand'    = '알 수 없는 명령: {0}. 사용 가능한 명령: update [colors|icons]'
    }
}

function Get-CoolLocale {
    $locale = if ($Env:LANG) { $Env:LANG } else { (Get-Culture).Name }
    switch -Regex ($locale) {
        '^zh[-_]?(TW|HK)' { return 'zh-TW' }
        '^zh' { return 'zh-CN' }
        '^ja' { return 'ja-JP' }
        '^ko' { return 'ko-KR' }
        '^en' { return 'en-US' }
        default { return 'en-US' }
    }
}

function Get-LocalizedString {
    param(
        [Parameter(Mandatory = $true)][string]$Key,
        [Parameter(ValueFromRemainingArguments = $true)][object[]]$Args
    )
    $locale = Get-CoolLocale
    if (-not $Script:Translations.ContainsKey($locale)) {
        $base = $locale.Split('-')[0]
        $locale = switch ($base) {
            'zh' { 'zh-CN' }
            'ja' { 'ja-JP' }
            'ko' { 'ko-KR' }
            _ { 'en-US' }
        }
    }
    $map = $Script:Translations[$locale]
    if (-not $map) { return $Key }
    $value = $map[$Key]
    if (-not $value) { return $Key }
    if ($Args -and $Args.Count -gt 0) { return ($value -f $Args) }
    return $value
}
