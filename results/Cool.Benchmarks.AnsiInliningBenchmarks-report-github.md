```

BenchmarkDotNet v0.13.8, Windows 11 (10.0.26100.32690) (Hyper-V)
AMD EPYC 7763, 1 CPU, 4 logical and 2 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256


```
| Method                        | Mean        | Error     | StdDev    | Code Size | Gen0      | Gen1      | Gen2    | Allocated  |
|------------------------------ |------------:|----------:|----------:|----------:|----------:|----------:|--------:|-----------:|
| Foreground_Xterm256_ReturnLen |    202.3 μs |   0.37 μs |   0.33 μs |      79 B |         - |         - |       - |          - |
| Foreground_Xterm16_ReturnLen  |    202.2 μs |   0.94 μs |   0.83 μs |      79 B |         - |         - |       - |          - |
| Foreground_RGB_ReturnLen      | 26,475.4 μs | 187.89 μs | 175.75 μs |     310 B | 2437.5000 | 1062.5000 | 31.2500 | 15314192 B |
| EscapeSGR_ReturnLen           |  3,972.0 μs |  55.11 μs |  51.55 μs |      93 B | 1273.4375 |         - |       - |  8023570 B |
| Bold_ReturnLen                |  1,972.7 μs |  12.11 μs |  10.11 μs |      77 B |  890.6250 |         - |       - |  5616519 B |
