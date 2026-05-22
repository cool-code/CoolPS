$paths = @(
    'Benchmarks\\bin\\Release\\net48-x86\\Cool.dll',
    'Benchmarks\\bin\\Release\\net48-x64\\Cool.dll',
    'Benchmarks\\bin\\Release\\net48-x86\\Cool.Benchmarks.exe',
    'Benchmarks\\bin\\Release\\net48-x64\\Cool.Benchmarks.exe'
)

foreach ($p in $paths) {
    if (Test-Path $p) {
        try {
            $rp = Resolve-Path $p
            $an = [System.Reflection.AssemblyName]::GetAssemblyName($rp.Path)
            Write-Host $p
            Write-Host "  FullName: $($an.FullName)"
            Write-Host "  ProcessorArchitecture: $($an.ProcessorArchitecture)"
        }
        catch {
            Write-Host "$p - cannot read assembly name: $($_.Exception.Message)"
        }
    }
    else {
        Write-Host "$p - not found"
    }
}
