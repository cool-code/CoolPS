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

# backup the existing CommandNotFoundAction to allow for chaining with other modules or default behavior if needed.
$script:OldCommandNotFoundAction = $ExecutionContext.InvokeCommand.CommandNotFoundAction
# Initialize a variable to track whether the module has been fully loaded. 
# This can be used to prevent certain actions from being performed before the module is ready.
$script:Cool_IsLoaded = $false
$script:Cool_LoadLock = [object]::new()
$script:ExportedMap = @{}

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
    if ($script:ExportedMap.ContainsKey($commandName)) {
        $commandEventArgs.CommandScriptBlock = [scriptblock]::Create("& '$commandName' @args")
        $commandEventArgs.StopSearch = $true
    }
    # Check if the command name corresponds to an existing directory. If it does, change to that directory.
    elseif (Test-Path -LiteralPath $commandName -PathType Container) {
        $commandEventArgs.CommandScriptBlock = [scriptblock]::Create("Set-CurrentDirectory -Path '$commandName'")
        $commandEventArgs.StopSearch = $true
    }
    # If the command is not a directory, but there was a previous CommandNotFoundAction defined,
    # invoke it to allow other modules or the default PowerShell behavior to handle the command.
    elseif ($null -ne $script:OldCommandNotFoundAction) {
        &$script:OldCommandNotFoundAction $commandName $commandEventArgs
    }
}

$MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
    $ExecutionContext.InvokeCommand.CommandNotFoundAction = $script:OldCommandNotFoundAction
}
