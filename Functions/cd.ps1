# Generate shorthand upward-directory functions (e.g. .., ..., ....)
# Creates functions from two dots up to a configurable max depth.

# go home
function ~ { Set-Location $HOME }

Export-ModuleMember -Function '~'

# Generate shorthand functions for going up directories, and navigating history.
$maxDepth = 20
for ($i = 1; $i -le $maxDepth; $i++) {
    # upward directory: .., ..., etc. (goes up in directory)
    $dots = "." * ($i + 1)
    $upPath = (@('..') * $i) -join '/'
    $funcDef = "function global:$dots { Microsoft.PowerShell.Management\Set-Location -Path '$upPath' }"    
    Invoke-Expression $funcDef
    Export-ModuleMember -Function $dots

    # previous history entry: /, //, ///, etc. (goes back in history)
    $slashes = "/" * $i
    $cmdBack = "try { 1..$i | ForEach-Object { Microsoft.PowerShell.Management\Set-Location -Path - -ErrorAction Stop } } catch {}"
    $funcDefBack = "function global:$slashes { $cmdBack }"
    Invoke-Expression $funcDefBack
    Export-ModuleMember -Function $slashes

    # next history entry: \, \\, \\\, etc. (goes forward in history)
    $backslashes = "\" * $i
    $cmdForward = "try { 1..$i | ForEach-Object { Microsoft.PowerShell.Management\Set-Location -Path + -ErrorAction Stop } } catch {}"
    $funcDefForward = "function global:$backslashes { $cmdForward }"
    Invoke-Expression $funcDefForward
    Export-ModuleMember -Function $backslashes
}

function Set-CurrentDirectory {
    [CmdletBinding(DefaultParameterSetName = 'Path')]
    param(
        [Parameter(ParameterSetName = 'Path', Position = 0, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true)]
        [string]$Path,

        [Parameter(ParameterSetName = 'LiteralPath', Mandatory = $true, ValueFromPipelineByPropertyName = $true)]
        [string]$LiteralPath,

        [switch]$PassThru
    )

    process {
        # Decide which path parameter to use
        $targetParam = if ($PSCmdlet.ParameterSetName -eq 'LiteralPath') { 'LiteralPath' } else { 'Path' }
        $originalPath = if ($targetParam -eq 'LiteralPath') { $LiteralPath } else { $Path }

        # If the path contains ... then convert it
        if ($originalPath -and $originalPath -match '\.{3,}') {
            $newPath = [Regex]::Replace($originalPath, '\.{3,}', {
                    param($m)
                    $len = $m.Value.Length
                    # n dots are converted to n-1 ..
                    return (@('..') * ($len - 1) -join '/')
                })
            $PSBoundParameters[$targetParam] = $newPath
        }

        # Handle the case where cd is entered without parameters to go to ~
        if ($null -eq $Path -and $null -eq $LiteralPath) {
            $PSBoundParameters['Path'] = '~'
        }

        # Forward all parameters (including common parameters like -Verbose) to Set-Location
        Set-Location @PSBoundParameters
    }
}

Set-Alias -Name cd -Value Set-CurrentDirectory -Option AllScope -Force -Scope Global
Export-ModuleMember -Function Set-CurrentDirectory -Alias cd