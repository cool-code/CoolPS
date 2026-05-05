# Cool.psm1
# This is the main module file for the Cool PowerShell module.
Set-StrictMode -Version Latest

# set UTF-8 encoding for input and output to ensure proper handling of Unicode characters,
# which is essential for the color and icon features of the module.
$OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [Text.Encoding]::UTF8
[Console]::InputEncoding = [Text.Encoding]::UTF8
$PSDefaultParameterValues['Get-Content:Encoding'] = 'UTF8'

# Initialize a variable to track whether the module has been fully loaded. 
# This can be used to prevent certain actions from being performed before the module is ready.
Set-Variable -Name 'Cool_IsLoaded' -Value $false -Visibility Private -Scope Script

if ($PSVersionTable.PSVersion.Major -lt 6) {
    Set-Variable -Name 'IsWindows' -Value ($env:OS -eq 'Windows_NT') -Scope Global
}

# Import the Initial.ps1 script to perform environment detection and set up necessary variables and functions.
. (Join-Path $PSScriptRoot 'Initial.ps1')
# Import the PSReadLine.ps1 script to set up custom key handlers and options for the Cool module.
. (Join-Path $PSScriptRoot 'PSReadLine.ps1')

# To ensure that the module's command not found handler is properly chained with any existing handlers,
# we store the original CommandNotFoundAction and set up a cleanup action to restore it when the module is removed.
if (-not (Get-Variable -Name 'Cool_OriginalCommandNotFoundAction' -Scope Global -ErrorAction SilentlyContinue)) {
    Set-Variable -Name 'Cool_OriginalCommandNotFoundAction' -Value $ExecutionContext.InvokeCommand.CommandNotFoundAction -Visibility Private -Scope Global
    $originalCommandNotFoundAction = Get-Variable -Name 'Cool_OriginalCommandNotFoundAction' -Scope Global -ErrorAction SilentlyContinue
    $resetPSReadLineOptionsAndKeyHandlers = (Get-ResetPSReadLineOptionsAndKeyHandlers)
    $resetSystemAliasesAndFunctions = { (Get-Item -Path 'Function:Set-SystemAliasesAndFunctions').ScriptBlock.GetNewClosure().Invoke(@{ ls = 'Get-ChildItem'; cd = 'Set-Location' }) }
    $ExecutionContext.SessionState.Module.OnRemove = {
        $ExecutionContext.InvokeCommand.CommandNotFoundAction = $originalCommandNotFoundAction.Value
        $oldEvent = Get-EventSubscriber -SourceIdentifier 'CoolFileWatcher' -ErrorAction SilentlyContinue
        if ($oldEvent) { Unregister-Event -SourceIdentifier 'CoolFileWatcher' }
        & $resetSystemAliasesAndFunctions
        & $resetPSReadLineOptionsAndKeyHandlers
        $originalCommandNotFoundAction.Visibility = 'Public'
        Remove-Variable -Name 'Cool_OriginalCommandNotFoundAction' -Scope Global -Force
    }.GetNewClosure()
}

# Set up system aliases and functions that the Cool module relies on.
# This is done in a way that allows for easy cleanup when the module is removed.
Set-SystemAliasesAndFunctions @{ ls = 'l'; cd = 'Set-CurrentDirectory' }

# Set up PSReadLine options and key handlers for enhanced command line editing and history management.
Set-PSReadLineOptionsAndKeyHandlers

# Set up a command not found handler that will load the main components of the Cool module in a lazy manner,
# ensuring that they are only loaded when needed. 
# This allows for faster initial loading of the module and reduces memory usage until the features are actually used.
$ExecutionContext.InvokeCommand.CommandNotFoundAction = {
    param($commandName, $commandEventArgs)
    # Load the main components of the Cool module in a lazy manner,
    # ensuring that they are only loaded when needed.
    if (-not $script:Cool_IsLoaded) {
        . (Join-Path $PSScriptRoot 'LazyLoad.ps1')
    }
    Invoke-CommandNotFoundAction $commandName $commandEventArgs
}
