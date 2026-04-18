function cool {
    param (
        [string]$Command
    )
    switch ($Command) {
        "update" {
            if ($args.Count -eq 0) {
                Update-ColorsCache
                Update-IconsCache
            }
            else {
                foreach ($cmd in $args) {
                    switch ($cmd.ToLower()) {
                        "colors" { Update-ColorsCache }
                        "icons" { Update-IconsCache }
                        default { Write-Host "未知子命令: $cmd. 可用子命令: colors, icons" -ForegroundColor Yellow }
                    }
                }
            }
        }
        default { Write-Host "未知命令: $Command. 可用命令: update colors, update icons" -ForegroundColor Yellow }    
    }
}
