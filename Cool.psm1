# Cool.psm1
. (Join-Path $PSScriptRoot "Core.ps1")

Export-ModuleMember -Function Get-CoolName, Format-CoolSize
Export-ModuleMember -Function Get-VisualWidth, Format-VisualWidthString

Set-Alias -Name vpad -Value VisualWidthPad
Set-Alias -Name vtrunc -Value VisualWidthTruncate

Export-ModuleMember -Alias vpad, vtrunc

Get-ChildItem (Join-Path $PSScriptRoot "Functions\*.ps1") | ForEach-Object {
    . $_.FullName
    Export-ModuleMember -Function $_.BaseName
}
