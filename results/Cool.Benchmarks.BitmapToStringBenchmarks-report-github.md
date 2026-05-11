```

BenchmarkDotNet v0.13.8, Windows 11 (10.0.26100.32690) (Hyper-V)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256


```
| Method          | Mean      | Error    | StdDev   | Median    | Gen0   | Code Size | Gen1   | Allocated |
|---------------- |----------:|---------:|---------:|----------:|-------:|----------:|-------:|----------:|
| ToString_Wide   | 112.51 μs | 2.239 μs | 5.408 μs | 114.92 μs | 0.3662 |   1,379 B |      - |    2373 B |
| ToString_Sparse |  22.49 μs | 0.029 μs | 0.024 μs |  22.49 μs | 2.2583 |   1,379 B | 0.0305 |   14403 B |
| ToString_Dense  | 237.37 μs | 1.578 μs | 1.318 μs | 237.04 μs |      - |   1,379 B |      - |      54 B |
