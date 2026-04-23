function Invoke-CommandNotFoundAction {
    param($commandName, $commandEventArgs)

    # If we've already handled this event, we should not attempt to handle it again.
    # This can happen if the original CommandNotFoundAction calls back into this handler.
    if ($commandEventArgs.PSObject.Properties['Cool_Handled']) {
        return
    }

    # Ensure the module is fully loaded before handling any commands,
    # to avoid issues with commands being invoked before their definitions are available.
    if (-not $script:Cool_IsLoaded) {
        [System.Threading.Monitor]::Enter($script:Cool_LoadLock)
        try {
            if (-not $script:Cool_IsLoaded) {
                # Load all necessary components of the Cool module.
                .  (Join-Path $PSScriptRoot 'Private/Localization.ps1')
                .  (Join-Path $PSScriptRoot 'Private/Cache.ps1')
                .  (Join-Path $PSScriptRoot 'Private/ColorAndIcon.ps1')
                .  (Join-Path $PSScriptRoot 'Private/VisualWidth.ps1')
                .  (Join-Path $PSScriptRoot 'Private/Profile.ps1')
                .  (Join-Path $PSScriptRoot 'Private/Core.ps1')
                # Mark the module as fully loaded to prevent reinitialization.
                $manifestPath = Join-Path $PSScriptRoot 'Cool.psd1'
                $manifest = Import-PowerShellDataFile -Path $manifestPath
                foreach ($name in ($manifest.FunctionsToExport + $manifest.AliasesToExport)) {
                    if ($name -and $name -ne '*') {
                        $null = $script:ExportedSet.Add($name)
                    }
                }
                $script:ExportedMap.Clear()
                $script:ExportedMap.Add('cool', (Join-Path $PSScriptRoot 'Functions/cool.ps1'))
                $script:ExportedMap.Add('l', (Join-Path $PSScriptRoot 'Functions/ls.ps1'))
                $cdPath = (Join-Path $PSScriptRoot 'Functions/cd.ps1')
                $script:ExportedMap.Add('Set-CurrentDirectory', $cdPath)
                $script:ExportedMap.Add('~', $cdPath)
                $maxDepth = 20
                foreach ($i in 1..$maxDepth) {
                    $script:ExportedMap.Add('.' * ($i + 1), $cdPath)
                    $script:ExportedMap.Add('\' * $i, $cdPath)
                    $script:ExportedMap.Add('/' * $i, $cdPath)
                }
                $script:Cool_IsLoaded = $true
            }
        }
        finally {
            [System.Threading.Monitor]::Exit($script:Cool_LoadLock)
        }
    }

    $fullInput = Get-InputFromPSReadLine

    # If the command name matches an entry in the ExportedMap,
    # we source the corresponding file to ensure the command is defined.
    if ($script:ExportedMap.ContainsKey($commandName)) {
        . $script:ExportedMap[$commandName]
    }

    # Check if the command matches an exported function or alias from this module.
    # If it does, we create a script block to invoke that command and set it as the action for this event,
    # effectively handling the command not found scenario for commands that are actually part of this module.
    if ($script:ExportedSet.Contains($commandName)) {
        $commandEventArgs.CommandScriptBlock = [scriptblock]::Create($fullInput)
        $commandEventArgs.StopSearch = $true
        return
    }

    if ($fullInput -and $fullInput -match '\.{3,}') {
        $fullInput = [Regex]::Replace($fullInput, '\.{3,}', {
                param($m)
                $parts = @('..') * ($m.Value.Length - 1)
                return $parts -join '/'
            })
    }
    $absolutePath = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($fullInput)

    # Check if the command name corresponds to an existing directory. If it does, change to that directory.
    if ([System.IO.Directory]::Exists($absolutePath)) {
        $commandEventArgs.CommandScriptBlock = [scriptblock]::Create("Set-CurrentDirectory -LiteralPath '$absolutePath'")
        $commandEventArgs.StopSearch = $true
        return
    }
    
    if ($null -ne $global:Cool_OriginalCommandNotFoundAction) {
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