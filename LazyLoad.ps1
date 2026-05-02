function script:HiddenUnexportedFunctions {
    Get-Command -Module Cool -CommandType Function | ForEach-Object {
        if ($_.Name -notin $script:ExportedFunctions) {
            $_.Visibility = 'Private'
        }
    }
}

# Ensure the module is fully loaded before handling any commands,
# to avoid issues with commands being invoked before their definitions are available.
[System.Threading.Monitor]::Enter($script:Cool_LoadLock)
try {
    if (-not $script:Cool_IsLoaded) {
        # Load all necessary components of the Cool module.
        . (Join-Path $PSScriptRoot 'Private/Localization.ps1') | Out-Null
        . (Join-Path $PSScriptRoot 'Private/ColorAndIcon.ps1') | Out-Null
        . (Join-Path $PSScriptRoot 'Private/VisualWidth.ps1') | Out-Null

        # Create functions for multi-level directory navigation using dots and slashes, and export them to the global scope.
        foreach ($i in 1..20) {
            New-Item -Path Function:\ -Name "global:$('.' * ($i + 1))" -Value 'Set-CurrentDirectory $MyInvocation.MyCommand.Name' -Force
            New-Item -Path Function:\ -Name "global:$('/' * ($i + 1))" -Value "try { 1..$i | ForEach-Object { Set-CurrentDirectory - -ErrorAction Stop } } catch { }" -Force
            New-Item -Path Function:\ -Name "global:$('\' * ($i + 1))" -Value "try { 1..$i | ForEach-Object { Set-CurrentDirectory + -ErrorAction Stop } } catch { }" -Force
        }
        Set-Item -Path 'Function:global:~' -Value 'Set-CurrentDirectory $MyInvocation.MyCommand.Name' -Force

        # Mark the module as fully loaded to prevent reinitialization.
        $manifestPath = Join-Path $PSScriptRoot 'Cool.psd1'
        $manifest = Import-PowerShellDataFile -Path $manifestPath
        foreach ($name in $manifest.FunctionsToExport) {
            if ($name -and $name -ne '*') {
                $null = $script:ExportedFunctions.Add($name)
                Export-ModuleMember -Function $name
            }
        }
        HiddenUnexportedFunctions
        $script:Cool_IsLoaded = $true
    }
}
finally {
    [System.Threading.Monitor]::Exit($script:Cool_LoadLock)
}

function script:Get-TargetNode {
    param($ast, $offset)
    $ast.FindAll({ $args[0] -is [System.Management.Automation.Language.CommandAst] }, $true) | 
    Where-Object { 
        $_.Extent.StartOffset -ge $offset -and 
        ($_.GetCommandName() -eq $commandName -or $_.Extent.Text.StartsWith($commandName))
    } | Select-Object -First 1
}
function script:Invoke-CommandNotFoundAction {
    param($commandName, $commandEventArgs)

    # If we've already handled this event, we should not attempt to handle it again.
    # This can happen if the original CommandNotFoundAction calls back into this handler.
    if ($commandEventArgs.PSObject.Properties['Cool_Handled']) {
        return
    }

    # If the command is part of this module, we attempt to load it from the Functions directory if it's not already loaded.
    # This allows for lazy loading of command implementations, improving performance by only loading what is necessary when it is needed.
    # This mechanism allows us to keep the initial load time of the module fast, while still providing access to all commands when they are needed.
    if ($script:ExportedFunctions.Contains($commandName)) {
        $path = (Join-Path $PSScriptRoot "Functions/$commandName.ps1")
        if ([System.IO.File]::Exists($path)) {
            . $path | Out-Null
            HiddenUnexportedFunctions
        }
        $commandEventArgs.CommandScriptBlock = [scriptblock]::Create("& '$commandName' @args")
        $commandEventArgs.StopSearch = $true
        return
    }

    $fullInput, $cursor = Get-InputFromPSReadLine
    $ast = [System.Management.Automation.Language.Parser]::ParseInput($fullInput, [ref]$null, [ref]$null)

    $currentHistory = (Get-History -Count 1)
    if ($currentHistory) {
        $currentId = $currentHistory.Id
    }
    else {
        $currentId = 0
    }

    # initialize offset tracking (reset if it's a new line)
    if ($currentId -ne $script:LastHistoryId -or $fullInput -ne $script:LastFullInput) {
        $script:LastCommandOffset = 0
        $script:LastHistoryId = $currentId
        $script:LastFullInput = $fullInput
    }

    # Attempt to find the command AST node that corresponds to the command that was not found,
    # starting from the last known offset. This allows us to handle cases where the command not
    # found is part of a larger command line with parameters, and we want to extract just the
    # command portion for path resolution.
    $targetAst = Get-TargetNode $ast $script:LastCommandOffset
    if ($null -eq $targetAst -and $script:LastCommandOffset -gt 0) {
        $script:LastCommandOffset = 0
        $targetAst = Get-TargetNode $ast 0
    }

    # If we still can't find it after resetting the offset,
    # we will just use the command name as a fallback.
    if ($null -ne $targetAst) {
        $commandText = $targetAst.Extent.Text.Trim()
        $script:LastCommandOffset = $targetAst.Extent.EndOffset
    }
    else {
        $commandText = $commandName
    }

    $passThru = $false
    # adjust the regex to ensure it only matches -PassThru as a standalone parameter,
    # not as part of another parameter or argument
    if ($commandText -match '^(.*?)\s+(-PassThru)') {
        $commandText = $matches[1]
        $passThru = $true
    }

    # Remove any trailing parameters to isolate the command text for path resolution.
    if ($commandText -match '^(.*?)\s+(-\w+)') {
        $commandText = $matches[1]
    }

    $absolutePath = $null
    try {
        # 1. If the command text starts with &, we treat the rest as a potential path,
        # allowing for commands like & .\script.ps1
        if ($commandText.Trim() -like "&*") {
            $rawPath = Convert-MultiDots $commandName
        }
        else {
            $rawPath = Convert-MultiDots $commandText
        }

        # 2. Only resolve if it looks like a path (contains a slash or dot, or is an existing directory)
        if ($rawPath -match '[\\/\.]' -or [System.IO.Directory]::Exists($rawPath)) {
            $absolutePath = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($rawPath)
        }
    }
    catch {
        # Silently fail to prevent red errors like "Drive not found"
        $absolutePath = $null
    }

    # Check if the command name corresponds to an existing directory. If it does, change to that directory.
    if ($null -ne $absolutePath -and [System.IO.Directory]::Exists($absolutePath)) {
        $command = "Set-CurrentDirectory -LiteralPath '$absolutePath'"
        if ($passThru) {
            $command += ' -PassThru'
        }
        $commandEventArgs.CommandScriptBlock = [scriptblock]::Create($command)
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
        $commandEventArgs | Add-Member -NotePropertyName 'Cool_Handled' -NotePropertyValue $true
        $global:Cool_OriginalCommandNotFoundAction.Invoke($commandName, $commandEventArgs)
    }
}