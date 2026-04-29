# Functions\l.ps1

function global:l {
    param(
        [string]$Path = ".",
        [switch]$All
    )
    $items = Get-ChildItem -LiteralPath $Path -Force:$All
    if (-not $items) { return }

    $data = @(
        foreach ($item in $items) {
            $text = Format-CoolName -Item $item
            [PSCustomObject]@{
                Text  = $text
                Width = Get-VisualWidth -Text $text
            }
        }
    )

    $termWidth = $Host.UI.RawUI.WindowSize.Width - 2
    $maxW = ($data | Measure-Object -Property Width -Maximum).Maximum + 3
    $cols = [Math]::Max(1, [Math]::Floor($termWidth / $maxW))
    $rows = [Math]::Ceiling($data.Count / $cols)

    for ($r = 0; $r -lt $rows; $r++) {
        $line = "  "
        for ($c = 0; $c -lt $cols; $c++) {
            $idx = $r + ($c * $rows)
            if ($idx -lt $data.Count) {
                $line += vPadRight -Text $data[$idx].Text -Width $maxW
            }
        }
        Write-Host $line
    }
}