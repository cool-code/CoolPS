# HotKeys.ps1 - Define custom hotkeys for PSReadLine

# Create a shortcut variable for easier access to PSReadLine methods in hotkey script blocks.
$script:PSRL = [Microsoft.PowerShell.PSConsoleReadLine]

function Get-InputFromPSReadLine {
    $line = $null
    $cursor = $null
    # Get the current command line and cursor position
    $script:PSRL::GetBufferState([ref]$line, [ref]$cursor)
    if ($null -ne $line -and -not [string]::IsNullOrWhiteSpace($line)) {
        $target = $line.Trim()
        return $target
    }
    return $null
}

# This script sets up custom hotkeys in PSReadLine for enhanced command line editing and history management.
# Set up command prediction to use history and display predictions in a list view style.
if ($host.Name -eq 'ConsoleHost' -and $Host.UI.SupportsVirtualTerminal) {
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

        # Configure the Tab key to trigger menu completion, which allows cycling through possible completions in a dropdown menu.
        Set-PSReadLineKeyHandler -Key "Tab" -Function MenuComplete -ErrorAction Stop

        # Configure Up and Down arrow keys to move the cursor to the end of the line when searching history, which is more intuitive for most users.
        Set-PSReadLineOption -HistorySearchCursorMovesToEnd -ErrorAction Stop

        # Set Up and Down arrow keys to search through command history based on the current input, similar to typical shell behavior.
        Set-PSReadLineKeyHandler -Key UpArrow -Function HistorySearchBackward -ErrorAction Stop
        Set-PSReadLineKeyHandler -Key DownArrow -Function HistorySearchForward -ErrorAction Stop

        # This script sets up a custom hotkey (Alt+Delete) in PSReadLine to delete the current command line from history.
        # It works by directly manipulating the history file and then refreshing the PSReadLine cache.
        Set-PSReadLineKeyHandler -Chord 'Alt+Delete' `
            -BriefDescription "DeleteFromHistory" `
            -LongDescription "Delete the current command line from history" `
            -ScriptBlock {
            # Yes, this is a hack, but PSReadLine does not provide a direct API to check dropdown visibility
            $options = $script:PSRL::GetOptions()
            $isPredictorOn = ($options.PredictionSource -ne 'None')
    
            $target = Get-InputFromPSReadLine
            # Only proceed if there is a non-empty command line
            if ($null -ne $target) {
                $historyPath = $options.HistorySavePath
                if (Test-Path $historyPath) {
                    # remove the target command from history content
                    $newContent = [System.IO.File]::ReadAllLines($historyPath) | Where-Object { $_ -ne $target }
            
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
                        $script:PSRL::BackwardDeleteChar()
                    }
                }
            }
        } -ErrorAction Stop

        Set-PSReadLineKeyHandler -Chord 'Alt+s' `
            -BriefDescription "SaveInHistory" `
            -LongDescription "Save the current command line in history but do not execute it" `
            -ScriptBlock {
            $target = Get-InputFromPSReadLine
            # Only proceed if there is a non-empty command line
            if ($null -ne $target) {
                $script:PSRL::AddToHistory($target)
            }
            $script:PSRL::RevertLine()
        } -ErrorAction Stop
    }
    catch {
        # If PSReadLine is not available or any error occurs,
        # we can safely ignore it as these hotkeys are optional enhancements.
    }
}
