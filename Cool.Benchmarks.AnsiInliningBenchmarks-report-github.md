```

BenchmarkDotNet v0.13.8, Windows 11 (10.0.26100.32690) (Hyper-V)
AMD EPYC 9V74, 1 CPU, 4 logical and 2 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256


```
| Method                        | Mean        | Error    | StdDev   | Code Size | Gen0      | Gen1      | Gen2    | Allocated  |
|------------------------------ |------------:|---------:|---------:|----------:|----------:|----------:|--------:|-----------:|
| Foreground_Xterm256_ReturnLen |    198.4 μs |  3.94 μs |  6.48 μs |      79 B |         - |         - |       - |          - |
| Foreground_Xterm16_ReturnLen  |    195.3 μs |  0.58 μs |  0.55 μs |      79 B |         - |         - |       - |          - |
| Foreground_RGB_ReturnLen      | 24,860.6 μs | 33.34 μs | 26.03 μs |     310 B | 2437.5000 | 1156.2500 | 31.2500 | 15314420 B |
| EscapeSGR_ReturnLen           |  4,099.3 μs | 74.36 μs | 69.56 μs |      93 B | 1273.4375 |         - |       - |  8023570 B |
| Bold_ReturnLen                |  1,995.4 μs | 36.17 μs | 33.84 μs |      77 B |  890.6250 |         - |       - |  5616519 B |
