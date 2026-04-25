# Cool.psm1
# This is the main module file for the Cool PowerShell module.
Set-StrictMode -Version Latest

# set UTF-8 encoding for input and output to ensure proper handling of Unicode characters,
# which is essential for the color and icon features of the module.
$OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [Text.Encoding]::UTF8
[Console]::InputEncoding = [Text.Encoding]::UTF8
$PSDefaultParameterValues['Get-Content:Encoding'] = 'UTF8'

# Import the PSReadLine script to set up custom key handlers and options for the Cool module.
. (Join-Path $PSScriptRoot "PSReadLine.ps1")

# Set up aliases for 'ls' and 'cd' to point to the module's implementations,
# and export them for use in the global scope.
Set-Alias -Name 'ls' -Value 'l' -Option AllScope -Force -Scope Global
Set-Alias -Name 'cd' -Value 'Set-CurrentDirectory' -Option AllScope -Force -Scope Global

# Initialize a variable to track whether the module has been fully loaded. 
# This can be used to prevent certain actions from being performed before the module is ready.
$script:Cool_IsLoaded = $false
$script:Cool_LoadLock = [object]::new()
$script:ExportedSet = [System.Collections.Generic.HashSet[string]]::new(2048, [System.StringComparer]::OrdinalIgnoreCase)
$Script:ExportedMap = [System.Collections.Generic.Dictionary[string, string]]::new([System.StringComparer]::OrdinalIgnoreCase)
$script:LastCommandOffset = 0
$script:LastHistoryId = -1
$script:LastFullInput = ''

# To ensure that the module's command not found handler is properly chained with any existing handlers,
# we store the original CommandNotFoundAction and set up a cleanup action to restore it when the module is removed.
if (-not (Get-Variable -Name "Cool_Module_IsImported" -Scope Global -ErrorAction SilentlyContinue)) {
    $global:Cool_OriginalCommandNotFoundAction = $ExecutionContext.InvokeCommand.CommandNotFoundAction
    $global:Cool_Module_IsImported = $true
    $MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
        $ExecutionContext.InvokeCommand.CommandNotFoundAction = $global:Cool_OriginalCommandNotFoundAction
        $global:Cool_Module_IsImported = $false
    }
}

$ExecutionContext.InvokeCommand.CommandNotFoundAction = {
    param($commandName, $commandEventArgs)
    # Load the main components of the Cool module in a lazy manner,
    # ensuring that they are only loaded when needed.
    if (-not $script:Cool_IsLoaded) {
        . (Join-Path $PSScriptRoot "LazyLoad.ps1")
    }
    Invoke-CommandNotFoundAction $commandName $commandEventArgs
}