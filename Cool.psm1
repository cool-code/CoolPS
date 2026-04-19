# Cool.psm1
# This is the main module file for the Cool PowerShell module.

# set UTF-8 encoding for input and output to ensure proper handling of Unicode characters,
# which is essential for the color and icon features of the module.
$OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [Text.Encoding]::UTF8
[Console]::InputEncoding = [Text.Encoding]::UTF8
$PSDefaultParameterValues['Get-Content:Encoding'] = 'UTF8'

# Import all the necessary scripts that contain the core functionality of the module.
. (Join-Path $PSScriptRoot "Localization.ps1")
. (Join-Path $PSScriptRoot "Cache.ps1")
. (Join-Path $PSScriptRoot "Colors.ps1")
. (Join-Path $PSScriptRoot "Icons.ps1")
. (Join-Path $PSScriptRoot "VisualWidth.ps1")
. (Join-Path $PSScriptRoot "HotKeys.ps1")
. (Join-Path $PSScriptRoot "Profile.ps1")
. (Join-Path $PSScriptRoot "Core.ps1")

# Export the public functions of the module.
Export-ModuleMember -Function Get-CoolName, Format-CoolSize
Export-ModuleMember -Function Get-VisualWidth, Format-VisualWidthString

# Import all function scripts from the Functions directory.
Join-Path $PSScriptRoot "Functions\cool.ps1"
Join-Path $PSScriptRoot "Functions\ls.ps1"
Join-Path $PSScriptRoot "Functions\cd.ps1"