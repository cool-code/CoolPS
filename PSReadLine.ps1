# PSReadLine.ps1 - Define custom key handlers for PSReadLine to enhance command line editing and history management.
# This script is imported by Cool.psm1 to set up the necessary key handlers and options in PSReadLine for the Cool module.

function Convert-MultiDots {
    param([string]$InputString)
    if ([string]::IsNullOrWhiteSpace($InputString)) { return $InputString }
    # Regex logic:
    # (?<=^|[\\/])  : Preceded by start of string or a slash
    # \.{3,}        : Match three or more dots
    # (?=[\\/]|$)   : Followed by a slash or end of string
    return [Regex]::Replace($InputString, '(?<=^|[\\/])\.{3,}(?=[\\/]|$)', {
            param($m)
            $dotsCount = $m.Value.Length
            # Default slash style: use \ if the path contains backslashes, otherwise use /
            $sep = if ($InputString -match '\\') { '\' } else { '/' }
            $ups = @('..') * ($dotsCount - 1)
            return $ups -join $sep
        })
}

$script:PSRL = [Microsoft.PowerShell.PSConsoleReadLine]

function Get-InputFromPSReadLine {
    $line = $null
    $cursor = $null
    # Get the current command line and cursor position
    $script:PSRL::GetBufferState([ref]$line, [ref]$cursor)
    return $line, $cursor
}

function DeleteFromHistory {
    # Yes, this is a hack, but PSReadLine does not provide a direct API to check dropdown visibility
    $options = $script:PSRL::GetOptions()
    $isPredictorOn = ($options.PredictionSource -ne 'None')
    
    $line, $cursor = Get-InputFromPSReadLine
    # Only proceed if there is a non-empty command line
    if (-not [string]::IsNullOrWhiteSpace($line)) {
        $target = $line.Trim()
        $historyPath = $options.HistorySavePath
        if ([System.IO.File]::Exists($historyPath)) {
            # remove the target command from history content
            $newContent = [System.IO.File]::ReadAllLines($historyPath) | Where-Object { $_.Trim() -ne $target }

            # move original history file to a backup location to prevent conflicts during clearing
            Move-Item $historyPath "$historyPath.bak" -Force -ErrorAction SilentlyContinue
            
            # Clear history using PSReadLine API
            $script:PSRL::RevertLine()
            try {
                $script:PSRL::ClearHistory()
                
                # Rebuild history file and memory cache using AddToHistory
                if ($null -ne $newContent) {
                    foreach ($h in $newContent) { 
                        $script:PSRL::AddToHistory($h) 
                    }
                }
                # Cleanup backup file
                Remove-Item "$historyPath.bak" -ErrorAction SilentlyContinue
            }
            catch {
                # In case of any error, restore the original history file to prevent data loss
                Move-Item "$historyPath.bak" $historyPath -Force -ErrorAction SilentlyContinue
            }

            # If predictor is on, we need to trigger it to refresh its cache after history change
            if ($isPredictorOn) {
                # Trigger a dummy input to refresh predictor cache
                $script:PSRL::Insert(' ')
                $script:PSRL::Undo()
            }
        }
    }
}

function SaveInHistory {
    $line, $cursor = Get-InputFromPSReadLine
    # Only proceed if there is a non-empty command line
    if (-not [string]::IsNullOrWhiteSpace($line)) {
        $target = $line.Trim()
        $script:PSRL::AddToHistory($target)
    }
    $script:PSRL::RevertLine()
}

function SmartDirectoryNavigation {
    $line, $cursor = Get-InputFromPSReadLine
    if (-not [string]::IsNullOrWhiteSpace($line)) {
        $script:PSRL::AcceptLine()
        return
    }

    # Parse the input line to analyze its structure.
    # We will check if it's a simple string that can be treated as a directory path for quick navigation.
    $errors = $null
    $tokens = $null
    $ast = [System.Management.Automation.Language.Parser]::ParseInput($line, [ref]$tokens, [ref]$errors)

    if (-not $errors) {
        $pipeline = $ast.EndBlock.Statements
        if ($pipeline.Count -eq 1 -and $pipeline[0] -is [System.Management.Automation.Language.PipelineAst]) {
            $command = $pipeline[0].PipelineElements
            if ($command.Count -eq 1 -and $command[0] -is [System.Management.Automation.Language.CommandExpressionAst]) {
                $expression = $command[0].Expression
                $potentialPath = $null
                if ($expression -is [System.Management.Automation.Language.StringConstantExpressionAst]) {
                    $potentialPath = $expression.Value
                }
                elseif ($expression -is [System.Management.Automation.Language.ExpandableStringExpressionAst]) {
                    try {
                        $potentialPath = $ExecutionContext.InvokeCommand.ExpandString($expression.Value)
                    }
                    catch {
                        $potentialPath = $null
                    }
                }
                if ($null -ne $potentialPath) {
                    try {
                        $potentialPath = Convert-MultiDots $potentialPath
                        $absPath = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($potentialPath)
                        if ([System.IO.Directory]::Exists($absPath)) {
                            $script:PSRL::Replace(0, $line.Length, $absPath)
                        }
                    }
                    catch {}
                }
            }
        }
    }
    $script:PSRL::AcceptLine()
}

function EnhancedMenuComplete {
    $line, $cursor = Get-InputFromPSReadLine

    # 1. Find the start position of the "word" at the cursor
    # We consider space, semicolon, and quotes as word delimiters for simplicity,
    # which covers most cases for file paths and commands.
    $firstHalf = $line.SubString(0, $cursor)
    $lastSpaceIndex = $firstHalf.LastIndexOfAny(@(' ', ';', '"', "'"))
    $startIndex = $lastSpaceIndex + 1
    $currentWord = $firstHalf.SubString($startIndex)

    # 2. Only perform multi-dot conversion on the current "word"
    # under the cursor to avoid unintended changes to the entire line.
    $converted = Convert-MultiDots $currentWord

    # 3. If there is a change, only replace the current word in the buffer,
    # preserving the rest of the line and cursor position.
    if ($converted -ne $currentWord) {
        $script:PSRL::Replace($startIndex, $currentWord.Length, $converted)
    }

    # 4. Execute system completion after potential conversion,
    # which will now work with the updated word under the cursor.
    try {
        while ($true) {
            $script:PSRL::MenuComplete()
            $line, $cursor = Get-InputFromPSReadLine
            if ($line.SubString(0, $cursor) -match "\0\d+\s*$") {
                continue
            }
            break
        }
    }
    finally {
        $line, $cursor = Get-InputFromPSReadLine
        if ($line -match "\0") {
            $cleanLine = $line -replace "\0\d+\s*", ""
            $script:PSRL::Replace(0, $line.Length, $cleanLine)
            $newCursor = [Math]::Min($cursor, $cleanLine.Length)
            $script:PSRL::SetCursorPosition($newCursor)
            $script:PSRL::InvokePrompt()
        }
    }
}

function TabExpansion2 {
    [CmdletBinding()]
    param([string]$inputScript, [int]$cursorColumn, $hashtable)
    # 1. Get the original completion results from the system
    $offset = 0
    if ($inputScript -match "(.*)\0(\d+)\s*$") {
        $replacementLength = $inputScript.Length
        $inputScript = $matches[1]
        $cursorColumn = $inputScript.Length
        $offset = [int]$matches[2]
        $script:PSRL::Replace(0, $replacementLength, $inputScript)
    }

    $ret = [System.Management.Automation.CommandCompletion]::CompleteInput($inputScript, $cursorColumn, $hashtable)

    # If there are no matches, return the original result without modification
    if ($null -eq $ret -or $ret.CompletionMatches.Count -eq 0) { return $ret }

    $inputScript = $inputScript.SubString($ret.ReplacementIndex, $ret.ReplacementLength)

    $count = $ret.CompletionMatches.Count
    if ([Console]::WindowHeight -le 7) {
        $offset = 0
        $maxSafeCount = $count
    }
    else {
        $maxVisibleHeight = [Console]::WindowHeight - 5
        $windowWidth = [Console]::WindowWidth
        $maxItemWidth = ($ret.CompletionMatches | Measure-Object -Property { Get-VisualWidth $_.ListItemText } -Maximum).Maximum + 5
        $columns = [Math]::Max(1, [Math]::Floor($windowWidth / $maxItemWidth))
        $maxSafeCount = $maxVisibleHeight * $columns
    }

    $newMatches = New-Object System.Collections.Generic.List[System.Management.Automation.CompletionResult]

    if ($offset -gt 0) {
        $i = if ($offset -gt $maxSafeCount) { $offset - $maxSafeCount + 1 } else { 0 }
        $newMatches.Add([System.Management.Automation.CompletionResult]::new(
                $inputScript + [char]0 + $i + " ",
                (ColorOrange) + (EscapeColor "27") + "" + (EscapeColor "7") + " $offset " + "" + (ColorYellow) + (EscapeColor "27") + "" + (EscapeColor "7") + "" + (ColorReset),
                "Text",
                "$offset more items above... "
            ))
    }
    foreach ($i in $offset..($count - 1)) {
        $result = $ret.CompletionMatches[$i]
        if ($newMatches.Count -ge $maxSafeCount) {
            $moreCount = $count - $i
            $newMatches.Add([System.Management.Automation.CompletionResult]::new(
                    $inputScript + [char]0 + $i + " ",
                    (ColorCyan) + (EscapeColor "7") + "" + (EscapeColor "27") + "" + (ColorBlue) + (EscapeColor "7") + "" + " $moreCount " + (EscapeColor "27") + "" + (ColorReset),
                    "Text",
                    "$moreCount more items... "
                ))
            break
        }
        $added = $false
        # 2. Only apply "beautification" for filesystem paths
        if ($result.ResultType -in @('ProviderContainer', 'ProviderItem', 'Command')) {
            $item = Get-Item -LiteralPath $result.ToolTip -Force -ErrorAction SilentlyContinue
            # In some cases, certain files may not be accessible via Get-Item (e.g., due to permission issues).
            # In such cases, we attempt to retrieve the same-named file from its parent directory using Get-ChildItem
            # to bypass permission issues, allowing us to at least beautify its name.
            if ($null -eq $item) {
                $parentPath = [System.IO.Path]::GetDirectoryName($result.ToolTip)
                $fileName = [System.IO.Path]::GetFileName($result.ToolTip)
                if ($parentPath -and (Test-Path -LiteralPath $parentPath)) {
                    $item = Get-ChildItem -LiteralPath $parentPath -Filter $fileName -Force -ErrorAction SilentlyContinue
                }
            }
            if ($null -ne $item) {
                $listItemText = Format-CoolName -Item $item
                $newMatches.Add([System.Management.Automation.CompletionResult]::new(
                        $result.CompletionText,
                        $listItemText, # Menu display colored text
                        $result.ResultType,
                        $result.ToolTip
                    ))
                $added = $true
            }
        }
        # 3. Fallback logic: if the item wasn't beautified, add the original result back
        if (-not $added) {
            $newMatches.Add($result)
        }
    }

    # 4. Key step: reconstruct and return the CommandCompletion object
    return [System.Management.Automation.CommandCompletion]::new(
        $newMatches,
        $ret.CurrentMatchIndex,
        $ret.ReplacementIndex,
        $ret.ReplacementLength
    )
}

# This script sets up custom hotkeys in PSReadLine for enhanced command line editing and history management.
# Set up command prediction to use history and display predictions in a list view style.
try {
    if ($PSVersionTable.PSVersion -ge [Version]"7.2") {
        # Enable command prediction based on history
        Set-PSReadLineOption -PredictionSource HistoryAndPlugin -ErrorAction Stop
    }
    else {
        # For older versions, just use history for prediction
        Set-PSReadLineOption -PredictionSource History -ErrorAction Stop
    }

    # Set the prediction view style to ListView for better visibility of suggestions.
    Set-PSReadLineOption -PredictionViewStyle ListView -ErrorAction Stop

    # Configure Up and Down arrow keys to move the cursor to the end of the line when searching history, which is more intuitive for most users.
    Set-PSReadLineOption -HistorySearchCursorMovesToEnd -ErrorAction Stop

    # Set Up and Down arrow keys to search through command history based on the current input, similar to typical shell behavior.
    Set-PSReadLineKeyHandler -Key UpArrow -Function HistorySearchBackward -ErrorAction Stop
    Set-PSReadLineKeyHandler -Key DownArrow -Function HistorySearchForward -ErrorAction Stop

    # This script sets up a custom hotkey (Alt+Delete) in PSReadLine to delete the current command line from history.
    # It works by directly manipulating the history file and then refreshing the PSReadLine cache.
    Set-PSReadLineKeyHandler -Chord 'Alt+Delete', "Ctrl+Alt+d" `
        -BriefDescription "DeleteFromHistory" `
        -LongDescription "Delete the current command line from history" `
        -ScriptBlock { try { DeleteFromHistory } catch { } } `
        -ErrorAction Stop

    # This script sets up a custom hotkey (Alt+s) in PSReadLine to save the current command line to history without executing it.
    # It retrieves the current command line, adds it to history, and then reverts the line to allow the user to continue editing or executing it as they wish.
    Set-PSReadLineKeyHandler -Chord 'Alt+s' `
        -BriefDescription "SaveInHistory" `
        -LongDescription "Save the current command line in history but do not execute it" `
        -ScriptBlock { try { SaveInHistory } catch { } } `
        -ErrorAction Stop

    # This script sets up a custom hotkey (Enter) in PSReadLine to implement smart directory navigation.
    # It intercepts the Enter key, checks if the input is a simple path-like string, and if so,
    # it attempts to resolve it as a directory and change to that directory instead of executing it as a command.
    Set-PSReadLineKeyHandler -Chord 'Enter' `
        -BriefDescription "SmartDirectoryNavigation" `
        -LongDescription "Navigate to the directory if the input is a valid path, otherwise execute the command" `
        -ScriptBlock { try { SmartDirectoryNavigation } catch { } } `
        -ErrorAction Stop

    # This script sets up a custom hotkey (Tab) in PSReadLine to trigger menu completion with enhanced formatting for filesystem paths.
    # It intercepts the Tab key, checks if the current word under the cursor is a potential filesystem path, applies multi-dot conversion if needed,
    # and then triggers the menu completion to show suggestions with colors and icons.
    Set-PSReadLineKeyHandler -Chord 'Tab' `
        -BriefDescription "EnhancedMenuComplete" `
        -LongDescription "Trigger menu completion with enhanced formatting for filesystem paths" `
        -ScriptBlock { try { EnhancedMenuComplete } catch { } } `
        -ErrorAction Stop

    Set-PSReadLineOption -AddToHistoryHandler {
        param($command)
        if ($command -match "\0\d+\s*") {
            return $false
        }
        return $true
    }
}
catch {
    # If PSReadLine is not available or any error occurs,
    # we can safely ignore it as these hotkeys are optional enhancements.
}