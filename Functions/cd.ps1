
# initializing navigation history (if not already initialized)
$script:Cool_NavHistory = [System.Collections.Generic.List[string]]::new()
$script:Cool_NavHistory.Add((Get-Location).Path)
$script:Cool_NavIndex = 0

# This function replaces the built-in Set-Location (cd) to add enhanced navigation features:
# 1. cd - : Go back in history
# 2. cd + : Go forward in history
# 3. cd ... : Go up multiple directories (e.g., cd ... goes up 2 levels, cd .... goes up 3 levels, etc.)
# It also maintains a custom navigation history stack to support these features, while still allowing users to use the original cd functionality for normal directory changes.
# The function is designed to be robust and handle edge cases, such as trying to navigate back or forward when there is no history, or using the ... syntax with varying numbers of dots.
# The function uses Set-Location with -LiteralPath to avoid issues with special characters in paths, and it updates the history stack only after a successful directory change.
# Additionally, it limits the history length to 20 entries to prevent unbounded growth, consistent with typical shell behavior.
function global:Set-CurrentDirectory {
    [CmdletBinding(DefaultParameterSetName = 'Path', SupportsTransactions = $true)]
    param(
        [Parameter(ParameterSetName = 'Path', Position = 0, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true)]
        [string]$Path,

        [Parameter(ParameterSetName = 'LiteralPath', Mandatory = $true, ValueFromPipelineByPropertyName = $true)]
        [string]$LiteralPath,

        [string]$StackName,

        [switch]$PassThru
    )
    process {
        # --- 0. Handle stack switching (StackName) with highest priority ---
        # If the user provided StackName, it indicates a system-level stack operation, so we forward it directly and exit
        if ($PSBoundParameters.ContainsKey('StackName')) {
            return Microsoft.PowerShell.Management\Set-Location @PSBoundParameters
        }
    
        $targetParam = if ($PSCmdlet.ParameterSetName -eq 'LiteralPath') { 'LiteralPath' } else { 'Path' }
        $originalPath = if ($targetParam -eq 'LiteralPath') { $LiteralPath } else { $Path }

        # --- 1. Handle cd - (go back) ---
        if ($originalPath -eq '-') {
            if ($script:Cool_NavIndex -gt 0) {
                $script:Cool_NavIndex--
                $targetPath = $script:Cool_NavHistory[$script:Cool_NavIndex]
                # We use Set-Location instead of Push-Location/Pop-Location to maintain our own history stack and support forward navigation with cd +
                # Using -LiteralPath to prevent issues with special characters in paths
                return Microsoft.PowerShell.Management\Set-Location -LiteralPath $targetPath -ErrorAction Stop
            }
            throw "There is no location history left to navigate backwards."
        }

        # --- 2. Handle cd + (go forward) ---
        if ($originalPath -eq '+') {
            if ($script:Cool_NavIndex -lt ($script:Cool_NavHistory.Count - 1)) {
                $script:Cool_NavIndex++
                $targetPath = $script:Cool_NavHistory[$script:Cool_NavIndex]
                return Microsoft.PowerShell.Management\Set-Location -LiteralPath $targetPath -ErrorAction Stop
            }
            throw "There is no location history left to navigate forwards."
        }

        # --- 3. Handle ... (enhanced logic) ---
        if ($originalPath -and $originalPath -match '\.{3,}') {
            $newPath = [Regex]::Replace($originalPath, '\.{3,}', {
                    param($m)
                    $parts = @('..') * ($m.Value.Length - 1)
                    return $parts -join '/'
                })
            $PSBoundParameters[$targetParam] = $newPath
        }

        # --- 4. Handle the case where cd is entered without parameters to go to ~ ---
        if ($null -eq $originalPath -or [string]::IsNullOrWhiteSpace($originalPath)) {
            $PSBoundParameters['Path'] = '~'
        }

        # --- 5. Execute the jump and update history ---
        try {
            $null = $PSBoundParameters.Remove('PassThru')

            # First, execute the jump.
            $result = Microsoft.PowerShell.Management\Set-Location @PSBoundParameters -PassThru
            $newActualPath = $result.Path

            # If the jump is successful, update the history.
            # If we jump to a new directory in the middle of the history, we should cut off the "forward" history.
            while ($script:Cool_NavIndex -lt ($script:Cool_NavHistory.Count - 1)) {
                $script:Cool_NavHistory.RemoveAt($script:Cool_NavHistory.Count - 1)
            }

            # Only record the new path if it is different from the current path in history.
            if ($newActualPath -ne $script:Cool_NavHistory[$script:Cool_NavIndex]) {
                $script:Cool_NavHistory.Add($newActualPath)
                $script:Cool_NavIndex++
            }
            
            # Limit history length to 20, consistent with system behavior.
            if ($script:Cool_NavHistory.Count -gt 20) {
                $script:Cool_NavHistory.RemoveAt(0)
                $script:Cool_NavIndex--
            }

            if ($PassThru) {
                return $result
            }            
        }
        catch {
            throw $_
        }
    }
}

Export-ModuleMember -Function Set-CurrentDirectory

# The function is exported as an alias 'cd' for easy use,
# and it also defines a shorthand function '~' for going to the home directory,
# as well as functions for going up multiple directories
# and navigating history with repeated slashes and backslashes.

# go home
function ~ { Set-CurrentDirectory $HOME }

Export-ModuleMember -Function '~'

# Generate shorthand functions for going up directories, and navigating history.
$maxDepth = 20
$commands = @(
    foreach ($i in 1..$maxDepth) {
        # upward directory: .., ..., etc. (goes up in directory)
        $dots = '.' * ($i + 1)
        $upPath = (@('..') * $i) -join '/'
        "function global:$dots { Set-CurrentDirectory -Path '$upPath' }"

        # previous history entry: /, //, ///, etc. (goes back in history)
        $slashes = '/' * $i
        $cmdBack = 'try { 1..$i | ForEach-Object { Set-CurrentDirectory -Path - -ErrorAction Stop } } catch {}'
        "function global:$slashes { $cmdBack }"

        # next history entry: \, \\, \\\, etc. (goes forward in history)
        $backslashes = '\' * $i
        $cmdForward = 'try { 1..$i | ForEach-Object { Set-CurrentDirectory -Path + -ErrorAction Stop } } catch {}'
        "function global:$backslashes { $cmdForward }"
    }
) -join "`n"

Invoke-Expression $commands

$functions = foreach ($i in 1..$maxDepth) {
    '.' * ($i + 1)
    '/' * $i
    '\' * $i
}
Export-ModuleMember -Function $functions
