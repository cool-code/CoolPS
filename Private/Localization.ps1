# Cool localization helper

$script:Translations = @{
    'en-US' = @{
        'LSColorsCacheUpdated'         = 'LS_COLORS cache updated!'
        'LSIconsCacheUpdated'          = 'LS_ICONS cache updated!'
        'CoolUsage'                    = "Usage:`r`n  cool init`r`n  cool update [colors|icons]"
        'UnknownCoolUpdateSubcommand'  = 'Unknown subcommand: {0}. Available: [colors|icons]'
        'UnknownCoolCommand'           = 'Unknown command: {0}.'
        'CoolProfileCreated'           = 'Created new {0} profile file.'
        'CoolProfileUpdated'           = 'Updated {0} profile file with Cool import.'
        'CoolProfileAlreadyConfigured' = 'Your {0} profile is already configured for Cool.'
        "NextPage"                     = " Next Page "
        "PrevPage"                     = " Prev Page "
        "NextPageToolTip"              = "Next page has {0} more results"
        "PrevPageToolTip"              = "Prev page has {0} more results"
    }
    'zh-CN' = @{
        'LSColorsCacheUpdated'         = 'LS_COLORS 缓存已更新！'
        'LSIconsCacheUpdated'          = 'LS_ICONS 缓存已更新！'
        'CoolUsage'                    = "用法:`r`n  cool init`r`n  cool update [colors|icons]"
        'UnknownCoolUpdateSubcommand'  = '未知子命令: {0}. 可用子命令: [colors|icons]'
        'UnknownCoolCommand'           = '未知命令: {0}.'
        'CoolProfileCreated'           = '已创建新的 {0} Profile 文件。'
        'CoolProfileUpdated'           = '已将 Cool 导入语句添加到 {0} Profile 文件。'
        'CoolProfileAlreadyConfigured' = '您的 {0} Profile 已经配置过了，无需重复操作。'
        "NextPage"                     = " 下一页 "
        "PrevPage"                     = " 上一页 "
        "NextPageToolTip"              = "下一页还有 {0} 个补全结果"
        "PrevPageToolTip"              = "上一页还有 {0} 个补全结果"
    }
    'zh-TW' = @{
        'LSColorsCacheUpdated'         = 'LS_COLORS 快取已更新！'
        'LSIconsCacheUpdated'          = 'LS_ICONS 快取已更新！'
        'CoolUsage'                    = "用法:`r`n  cool init`r`n  cool update [colors|icons]"
        'UnknownCoolUpdateSubcommand'  = '未知子命令: {0}. 可用子命令: [colors|icons]'
        'UnknownCoolCommand'           = '未知命令: {0}.'
        'CoolProfileCreated'           = '已創建新的 {0} Profile 文件。'
        'CoolProfileUpdated'           = '已將 Cool 導入語句添加到 {0} Profile 文件。'
        'CoolProfileAlreadyConfigured' = '您的 {0} Profile 已經配置過了，無需重複操作。'
        "NextPage"                     = " 下一頁 "
        "PrevPage"                     = " 上一頁 "
        "NextPageToolTip"              = "下一頁還有 {0} 個補全結果"
        "PrevPageToolTip"              = "上一頁還有 {0} 個補全結果"
    }
    'ja-JP' = @{
        'LSColorsCacheUpdated'         = 'LS_COLORS キャッシュを更新しました！'
        'LSIconsCacheUpdated'          = 'LS_ICONS キャッシュを更新しました！'
        'CoolUsage'                    = "使い方:`r`n  cool init`r`n  cool update [colors|icons]"
        'UnknownCoolUpdateSubcommand'  = '不明なサブコマンド: {0}. 使用可能: [colors|icons]'
        'UnknownCoolCommand'           = '不明なコマンド: {0}.'
        'CoolProfileCreated'           = '新しい {0} プロファイルファイルを作成しました。'
        'CoolProfileUpdated'           = '{0} プロファイルファイルに Cool のインポート文を追加しました。'
        'CoolProfileAlreadyConfigured' = '{0} プロファイルはすでに Cool 用に設定されています。'
        "NextPage"                     = " 次のページ "
        "PrevPage"                     = " 前のページ "
        "NextPageToolTip"              = "次のページにはさらに {0} 件の候補があります"
        "PrevPageToolTip"              = "前のページにはさらに {0} 件の候補があります"
    }
    'ko-KR' = @{
        'LSColorsCacheUpdated'         = 'LS_COLORS 캐시가 업데이트되었습니다!'
        'LSIconsCacheUpdated'          = 'LS_ICONS 캐시가 업데이트되었습니다!'
        'CoolUsage'                    = "사용법:`r`n  cool init`r`n  cool update [colors|icons]"
        'UnknownCoolUpdateSubcommand'  = '알 수 없는 하위 명령: {0}. 사용 가능: [colors|icons]'
        'UnknownCoolCommand'           = '알 수 없는 명령: {0}.'
        'CoolProfileCreated'           = '새 {0} 프로파일 파일을 생성했습니다.'
        'CoolProfileUpdated'           = '{0} 프로파일 파일에 Cool의 가져오기 문을 추가했습니다.'
        'CoolProfileAlreadyConfigured' = '{0} 프로파일은 이미 Cool용으로 설정되어 있습니다.'
        "NextPage"                     = " 다음 페이지 "
        "PrevPage"                     = " 이전 페이지 "
        "NextPageToolTip"              = "다음 페이지에 {0}개의 더 많은 결과가 있습니다"
        "PrevPageToolTip"              = "이전 페이지에 {0}개의 더 많은 결과가 있습니다"
    }
}

function script:Get-CoolLocale {
    $locale = if ($env:LANG) { $env:LANG } else { (Get-Culture).Name }
    switch -Regex ($locale) {
        '^zh[-_]?(TW|HK)' { return 'zh-TW' }
        '^zh' { return 'zh-CN' }
        '^ja' { return 'ja-JP' }
        '^ko' { return 'ko-KR' }
        '^en' { return 'en-US' }
        default { return 'en-US' }
    }
}

function script:Get-LocalizedString {
    param(
        [Parameter(Mandatory = $true)][string]$Key,
        [Parameter(ValueFromRemainingArguments = $true)][object[]]$Args
    )
    $locale = Get-CoolLocale
    if (-not $script:Translations.ContainsKey($locale)) {
        $base = $locale.Split('-')[0]
        $locale = switch ($base) {
            'zh' { 'zh-CN' }
            'ja' { 'ja-JP' }
            'ko' { 'ko-KR' }
            _ { 'en-US' }
        }
    }
    $map = $script:Translations[$locale]
    if (-not $map) { return $Key }
    $value = $map[$Key]
    if (-not $value) { return $Key }
    if ($Args -and $Args.Count -gt 0) { return ($value -f $Args) }
    return $value
}
