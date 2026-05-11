```

BenchmarkDotNet v0.13.8, Windows 11 (10.0.26100.32690) (Hyper-V)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256


```
| Method          | Mean      | Error    | StdDev   | Gen0   | Code Size | Gen1   | Allocated |
|---------------- |----------:|---------:|---------:|-------:|----------:|-------:|----------:|
| ToString_Wide   |  91.58 μs | 0.470 μs | 0.416 μs | 0.3662 |   1,379 B |      - |    2373 B |
| ToString_Sparse |  23.65 μs | 0.185 μs | 0.173 μs | 2.2583 |   1,379 B | 0.0305 |   14403 B |
| ToString_Dense  | 212.96 μs | 0.347 μs | 0.271 μs |      - |   1,379 B |      - |      54 B |
