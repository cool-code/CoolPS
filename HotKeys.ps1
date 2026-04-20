# HotKeys.ps1 - Define custom hotkeys for PSReadLine

# This script sets up custom hotkeys in PSReadLine for enhanced command line editing and history management.
# Set up command prediction to use history and display predictions in a list view style.
if ($host.Name -eq 'ConsoleHost') {
    if ($PSVersionTable.PSVersion -ge [Version]"7.2") {
        # Enable command prediction based on history
        Set-PSReadLineOption -PredictionSource HistoryAndPlugin
    }
    else {
        # For older versions, just use history for prediction
        Set-PSReadLineOption -PredictionSource History
    }

    # Set the prediction view style to ListView for better visibility of suggestions.
    Set-PSReadLineOption -PredictionViewStyle ListView

    # Configure the Tab key to trigger menu completion, which allows cycling through possible completions in a dropdown menu.
    Set-PSReadLineKeyHandler -Key "Tab" -Function MenuComplete

    # Configure Up and Down arrow keys to move the cursor to the end of the line when searching history, which is more intuitive for most users.
    Set-PSReadLineOption -HistorySearchCursorMovesToEnd

    # Set Up and Down arrow keys to search through command history based on the current input, similar to typical shell behavior.
    Set-PSReadLineKeyHandler -Key UpArrow -Function HistorySearchBackward 
    Set-PSReadLineKeyHandler -Key DownArrow -Function HistorySearchForward

    # Create a shortcut variable for easier access to PSReadLine methods in hotkey script blocks.
    $PSRL = [Microsoft.PowerShell.PSConsoleReadLine]

    # This script sets up a custom hotkey (Alt+Delete) in PSReadLine to delete the current command line from history.
    # It works by directly manipulating the history file and then refreshing the PSReadLine cache.
    Set-PSReadLineKeyHandler -Chord 'Alt+Delete' `
        -BriefDescription "DeleteFromHistory" `
        -LongDescription "Delete the current command line from history" `
        -ScriptBlock {
        $line = $null
        $cursor = $null
        # Get the current command line and cursor position
        $PSRL::GetBufferState([ref]$line, [ref]$cursor)
    
        # Yes, this is a hack, but PSReadLine does not provide a direct API to check dropdown visibility
        $options = $PSRL::GetOptions()
        $isPredictorOn = ($options.PredictionSource -ne 'None')
    
        # Only proceed if there is a non-empty command line
        if ($null -ne $line -and -not [string]::IsNullOrWhiteSpace($line)) {
            $target = $line.Trim()
            $historyPath = $options.HistorySavePath
        
            if (Test-Path $historyPath) {
                # remove the target command from history content
                $newContent = [System.IO.File]::ReadAllLines($historyPath) | Where-Object { $_ -ne $target }
            
                # move original history file to a backup location to prevent conflicts during clearing
                Move-Item $historyPath "$historyPath.bak" -Force -ErrorAction SilentlyContinue
            
                # Clear history using PSReadLine API
                $PSRL::RevertLine()
                try {
                    $PSRL::ClearHistory()
                
                    # Rebuild history file and memory cache using AddToHistory
                    if ($null -ne $newContent) {
                        foreach ($h in $newContent) { 
                            $PSRL::AddToHistory($h) 
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
                    $PSRL::Insert(' ')
                    $PSRL::BackwardDeleteChar()
                }
            }
        }
    }

    Set-PSReadLineKeyHandler -Chord 'Alt+s' `
        -BriefDescription "SaveInHistory" `
        -LongDescription "Save the current command line in history but do not execute it" `
        -ScriptBlock {
        $line = $null
        $cursor = $null
        # Get the current command line and cursor position
        $PSRL::GetBufferState([ref]$line, [ref]$cursor)
        if ($null -ne $line -and -not [string]::IsNullOrWhiteSpace($line)) {
            $PSRL::AddToHistory($line.Trim())
        }
        $PSRL::RevertLine()
    }
}
