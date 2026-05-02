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
. (Join-Path $PSScriptRoot 'PSReadLine.ps1')

$restoreScript = @'
        Set-Alias -Name ls -Value l -Option AllScope -Force -Scope Global
        Set-Alias -Name cd -Value Set-CurrentDirectory -Option AllScope -Force -Scope Global
        Set-Item -Path 'Function:global:cd~' -Value ([scriptblock]::Create('Set-CurrentDirectory ~')) -Force
        Set-Item -Path 'Function:global:cd..' -Value ([scriptblock]::Create('Set-CurrentDirectory ..')) -Force
        Set-Item -Path 'Function:global:cd\' -Value ([scriptblock]::Create('Set-CurrentDirectory \')) -Force
        Set-Item -Path 'Function:global:cd/' -Value ([scriptblock]::Create('Set-CurrentDirectory /')) -Force
        foreach ($d in 65..90) { 
            $drive = "$([char]$d):"
            Set-Item -Path "Function:global:$drive" -Value ([scriptblock]::Create('Set-CurrentDirectory $MyInvocation.MyCommand.Name')) -Force 
        }
'@
[scriptblock]::Create($restoreScript).Invoke()

# Initialize a variable to track whether the module has been fully loaded. 
# This can be used to prevent certain actions from being performed before the module is ready.
$script:Cool_IsLoaded = $false
$script:Cool_LoadLock = [object]::new()
$script:ExportedFunctions = [System.Collections.Generic.HashSet[string]]::new(2048, [System.StringComparer]::OrdinalIgnoreCase)
$script:LastCommandOffset = 0
$script:LastHistoryId = -1
$script:LastFullInput = ''

# To ensure that the module's command not found handler is properly chained with any existing handlers,
# we store the original CommandNotFoundAction and set up a cleanup action to restore it when the module is removed.
if (-not (Get-Variable -Name 'Cool_OriginalCommandNotFoundAction' -Scope Global -ErrorAction SilentlyContinue)) {
    $global:Cool_OriginalCommandNotFoundAction = $ExecutionContext.InvokeCommand.CommandNotFoundAction
    $MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
        $ExecutionContext.InvokeCommand.CommandNotFoundAction = $global:Cool_OriginalCommandNotFoundAction
        $restoreScript = @'
        Set-Alias -Name ls -Value Get-ChildItem -Option AllScope -Force -Scope Global
        Set-Alias -Name cd -Value Set-Location -Option AllScope -Force -Scope Global
        Set-Item -Path 'Function:global:cd~' -Value ([scriptblock]::Create('Set-Location ~')) -Force
        Set-Item -Path 'Function:global:cd..' -Value ([scriptblock]::Create('Set-Location ..')) -Force
        Set-Item -Path 'Function:global:cd\' -Value ([scriptblock]::Create('Set-Location \')) -Force
        Set-Item -Path 'Function:global:cd/' -Value ([scriptblock]::Create('Set-Location /')) -Force
        foreach ($d in 65..90) { 
            $drive = "$([char]$d):"
            Set-Item -Path "Function:global:$drive" -Value ([scriptblock]::Create('Set-Location $MyInvocation.MyCommand.Name')) -Force 
        }
'@
        [scriptblock]::Create($restoreScript).Invoke()
        $oldEvent = Get-EventSubscriber -SourceIdentifier 'CoolFileWatcher' -ErrorAction SilentlyContinue
        if ($oldEvent) { Unregister-Event -SourceIdentifier 'CoolFileWatcher' }
        Remove-Variable -Name 'Cool_OriginalCommandNotFoundAction' -Scope Global -Force
    }
}

$ExecutionContext.InvokeCommand.CommandNotFoundAction = {
    param($commandName, $commandEventArgs)
    # Load the main components of the Cool module in a lazy manner,
    # ensuring that they are only loaded when needed.
    if (-not $script:Cool_IsLoaded) {
        . (Join-Path $PSScriptRoot 'LazyLoad.ps1')
    }
    Invoke-CommandNotFoundAction $commandName $commandEventArgs
}