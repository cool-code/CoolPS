function cool {
    param (
        [string]$Command
    )
    if (-not $Command) {
        $msg = Get-LocalizedString 'CoolUsage'
        Write-Host $msg -ForegroundColor Cyan
        return
    }
    switch ($Command) {
        "update" {
            if ($args.Count -eq 0) {
                Update-ColorsCache
                Update-IconsCache
            }
            else {
                foreach ($cmd in $args) {
                    switch ($cmd) {
                        "colors" { Update-ColorsCache }
                        "icons" { Update-IconsCache }
                        default {
                            $msg = Get-LocalizedString 'UnknownCoolSubcommand' $cmd
                            Write-Host $msg -ForegroundColor Yellow
                        }
                    }
                }
            }
        }
        default {
            $msg = Get-LocalizedString 'UnknownCoolCommand' $Command
            Write-Host $msg -ForegroundColor Yellow
        }
    }
}
