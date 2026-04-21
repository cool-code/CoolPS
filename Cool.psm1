# Cool.psm1
# This is the main module file for the Cool PowerShell module.

# set UTF-8 encoding for input and output to ensure proper handling of Unicode characters,
# which is essential for the color and icon features of the module.
$OutputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [Text.Encoding]::UTF8
[Console]::InputEncoding = [Text.Encoding]::UTF8
$PSDefaultParameterValues['Get-Content:Encoding'] = 'UTF8'

# Import the HotKeys script first to ensure that any hotkey-related functionality is available
# before loading the rest of the module's features.
. (Join-Path $PSScriptRoot "HotKeys.ps1")

Set-Alias -Name 'ls' -Value 'l' -Option AllScope -Force -Scope Global
Set-Alias -Name 'cd' -Value 'Set-CurrentDirectory' -Option AllScope -Force -Scope Global
Export-ModuleMember -Alias 'ls', 'cd'

# Initialize a variable to track whether the module has been fully loaded. 
# This can be used to prevent certain actions from being performed before the module is ready.
$script:Cool_IsLoaded = $false
$script:Cool_LoadLock = [object]::new()
$script:ExportedMap = @{}

# To ensure that the module's command not found handler is properly chained with any existing handlers,
# we store the original CommandNotFoundAction and set up a cleanup action to restore it when the module is removed.
if (-not $global:Cool_Module_IsImported) {
    $global:Cool_OriginalCommandNotFoundAction = $ExecutionContext.InvokeCommand.CommandNotFoundAction
    $global:Cool_Module_IsImported = $true
    $MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
        $ExecutionContext.InvokeCommand.CommandNotFoundAction = $global:Cool_OriginalCommandNotFoundAction
        $global:Cool_Module_IsImported = $false
    }
}

$ExecutionContext.InvokeCommand.CommandNotFoundAction = {
    param($commandName, $commandEventArgs)

    # Ensure the module is fully loaded before handling any commands,
    # to avoid issues with commands being invoked before their definitions are available.
    if (-not $script:Cool_IsLoaded) {
        [System.Threading.Monitor]::Enter($script:Cool_LoadLock)
        try {
            if (-not $script:Cool_IsLoaded) {
                # Load all necessary components of the Cool module.
                .  (Join-Path $PSScriptRoot "Private/Localization.ps1")
                .  (Join-Path $PSScriptRoot "Private/Cache.ps1")
                .  (Join-Path $PSScriptRoot "Private/ColorAndIcon.ps1")
                .  (Join-Path $PSScriptRoot "Private/VisualWidth.ps1")
                .  (Join-Path $PSScriptRoot "Private/Profile.ps1")
                .  (Join-Path $PSScriptRoot "Private/Core.ps1")
                .  (Join-Path $PSScriptRoot "Functions/ls.ps1")
                .  (Join-Path $PSScriptRoot "Functions/cool.ps1")
                .  (Join-Path $PSScriptRoot "Functions/cd.ps1")
                # Mark the module as fully loaded to prevent reinitialization.
                $manifestPath = Join-Path $PSScriptRoot "Cool.psd1"
                $manifest = Import-PowerShellDataFile -Path $manifestPath
                foreach ($name in ($manifest.FunctionsToExport + $manifest.AliasesToExport)) {
                    if ($name -and $name -ne '*') {
                        $script:ExportedMap[[string]$name] = $true
                    }
                }
                $script:Cool_IsLoaded = $true
            }
        }
        finally {
            [System.Threading.Monitor]::Exit($script:Cool_LoadLock)
        }
    }

    $fullInput = Get-InputFromPSReadLine
    # Check if the command matches an exported function or alias from this module.
    # If it does, we create a script block to invoke that command and set it as the action for this event,
    # effectively handling the command not found scenario for commands that are actually part of this module.
    if ($script:ExportedMap.ContainsKey($commandName)) {
        $commandEventArgs.CommandScriptBlock = [scriptblock]::Create($fullInput)
        $commandEventArgs.StopSearch = $true
    }
    # Check if the command name corresponds to an existing directory. If it does, change to that directory.
    elseif (Test-Path -LiteralPath $fullInput -PathType Container) {
        $commandEventArgs.CommandScriptBlock = [scriptblock]::Create("Set-CurrentDirectory -LiteralPath '$fullInput'")
        $commandEventArgs.StopSearch = $true
    }
    elseif ($null -ne $global:Cool_OriginalCommandNotFoundAction -and -not $commandEventArgs.PSObject.Properties['Cool_Handled']) {
        # If the command was not handled by Cool Module, and there is an original CommandNotFoundAction
        # from another module or default, we call it to allow for chaining of command not found handlers.
        # We also add a check to prevent infinite loops in case the original handler tries to invoke a 
        # command that also triggers Cool Module's handler.
        # By marking the event args with a custom property, we can detect if we've already handled this
        # event and avoid calling the original handler again.
        $commandEventArgs | Add-Member -NotePropertyName "Cool_Handled" -NotePropertyValue $true
        $global:Cool_OriginalCommandNotFoundAction.Invoke($commandName, $commandEventArgs)
    }
}