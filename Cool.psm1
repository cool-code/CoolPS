# Cool.psm1
. (Join-Path $PSScriptRoot "Core.ps1")

Export-ModuleMember -Function Get-CoolName, Format-CoolSize
Export-ModuleMember -Function Get-VisualWidth, Format-VisualWidthString

Get-ChildItem (Join-Path $PSScriptRoot "Functions\*.ps1") | ForEach-Object { . $_.FullName }

