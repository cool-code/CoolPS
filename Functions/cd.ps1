
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
    try {
        $null = $PSBoundParameters.Remove('ErrorAction')
        # --- 0. Handle stack switching (StackName) with highest priority ---
        # If the user provided StackName, it indicates a system-level stack operation, so we forward it directly and exit
        if ($PSBoundParameters.ContainsKey('StackName')) {
            return Microsoft.PowerShell.Management\Set-Location @PSBoundParameters -ErrorAction Stop
        }
    
        $targetParam = if ($PSCmdlet.ParameterSetName -eq 'LiteralPath') { 'LiteralPath' } else { 'Path' }
        $originalPath = if ($targetParam -eq 'LiteralPath') { $LiteralPath } else { $Path }

        # Handle 'cd' with no arguments to go to the home directory
        if ($null -eq $originalPath -or [string]::IsNullOrWhiteSpace($originalPath)) {
            $PSBoundParameters['Path'] = '~'
        }
        # Handle 'cd -' for going back in history
        elseif ($originalPath -eq '-') {
            if ($script:Cool_NavIndex -gt 0) {
                $script:Cool_NavIndex--
                $targetPath = $script:Cool_NavHistory[$script:Cool_NavIndex]
                # We use Set-Location instead of Push-Location/Pop-Location to maintain our own history stack and support forward navigation with cd +
                # Using -LiteralPath to prevent issues with special characters in paths
                $null = $PSBoundParameters.Remove($targetParam)
                return Microsoft.PowerShell.Management\Set-Location -LiteralPath $targetPath @PSBoundParameters -ErrorAction Stop
            }
            throw "There is no location history left to navigate backwards."
        }
        # Handle 'cd +' for going forward in history
        elseif ($originalPath -eq '+') {
            if ($script:Cool_NavIndex -lt ($script:Cool_NavHistory.Count - 1)) {
                $script:Cool_NavIndex++
                $targetPath = $script:Cool_NavHistory[$script:Cool_NavIndex]
                $null = $PSBoundParameters.Remove($targetParam)
                return Microsoft.PowerShell.Management\Set-Location -LiteralPath $targetPath @PSBoundParameters -ErrorAction Stop
            }
            throw "There is no location history left to navigate forwards."
        }
        # Handle 'cd ...' for going up multiple directories
        else {
            $PSBoundParameters[$targetParam] = Convert-MultiDots $originalPath
        }

        # Execute the jump and update history
        try {
            $null = $PSBoundParameters.Remove('PassThru')

            # First, execute the jump.
            $result = Microsoft.PowerShell.Management\Set-Location @PSBoundParameters -PassThru -ErrorAction Stop
            if ($null -ne $result) {
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
            }
            if ($PassThru) {
                return $result
            }
        }
        catch {
            throw $_
        }
    }
    catch {
        $PSCmdlet.WriteError($_)
    }
}

Export-ModuleMember -Function Set-CurrentDirectory

# The function is exported as an alias 'cd' for easy use,
# and it also defines a shorthand function '~' for going to the home directory,
# as well as functions for going up multiple directories
# and navigating history with repeated slashes and backslashes.

# go home
function global:~ { Set-CurrentDirectory $HOME }

Export-ModuleMember -Function '~'

# Generate shorthand functions for going up directories, and navigating history.
$maxDepth = 20

$commands = [System.Text.StringBuilder]::new(10240)
foreach ($i in 1..$maxDepth) {
    # upward directory: .., ..., etc. (goes up in directory)
    $null = $commands.Append('function global:').Append('.' * ($i + 1))
    $null = $commands.Append(' { param([switch]$PassThru) Set-CurrentDirectory -Path ').Append((@('..') * $i) -join '/').Append(' @PSBoundParameters }').AppendLine()

    # previous history entry: /, //, ///, etc. (goes back in history)
    $null = $commands.Append('function global:').Append('/' * $i)
    $null = $commands.Append(' { param([switch]$PassThru) try { 1..').Append($i).Append(' | ForEach-Object { Set-CurrentDirectory -LiteralPath - @PSBoundParameters } } catch { } }').AppendLine()

    # next history entry: \, \\, \\\, etc. (goes forward in history)
    $null = $commands.Append('function global:').Append('\' * $i)
    $null = $commands.Append(' { param([switch]$PassThru) try { 1..').Append($i).Append(' | ForEach-Object { Set-CurrentDirectory -LiteralPath + @PSBoundParameters } } catch { } }').AppendLine()
}

Invoke-Expression $commands.ToString()

$functions = foreach ($i in 1..$maxDepth) {
    '.' * ($i + 1)
    '/' * $i
    '\' * $i
}
Export-ModuleMember -Function $functions
