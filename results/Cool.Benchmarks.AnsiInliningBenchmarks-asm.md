## .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256
```assembly
; Cool.Benchmarks.AnsiInliningBenchmarks.Foreground_Xterm256_ReturnLen()
       sub       rsp,28
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+8]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L01
M00_L00:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       short M00_L02
       movsxd    r9,edx
       mov       r8d,[r8+r9*4+10]
       mov       r9,[13F4C260]
       cmp       r8d,[r9+8]
       jae       short M00_L02
       movsxd    r8,r8d
       mov       r8,[r9+r8*8+10]
       add       eax,[r8+8]
       inc       edx
       cmp       [rcx+8],edx
       jg        short M00_L00
M00_L01:
       add       rsp,28
       ret
M00_L02:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 79
```

## .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256
```assembly
; Cool.Benchmarks.AnsiInliningBenchmarks.Foreground_Xterm16_ReturnLen()
       sub       rsp,28
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+10]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L01
M00_L00:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       short M00_L02
       movsxd    r9,edx
       mov       r8d,[r8+r9*4+10]
       mov       r9,[138CC270]
       cmp       r8d,[r9+8]
       jae       short M00_L02
       movsxd    r8,r8d
       mov       r8,[r9+r8*8+10]
       add       eax,[r8+8]
       inc       edx
       cmp       [rcx+8],edx
       jg        short M00_L00
M00_L01:
       add       rsp,28
       ret
M00_L02:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 79
```

## .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256
```assembly
; Cool.Benchmarks.AnsiInliningBenchmarks.Foreground_RGB_ReturnLen()
       push      rdi
       push      rsi
       push      rbx
       sub       rsp,30
       vzeroupper
       xor       eax,eax
       mov       [rsp+20],rax
       mov       [rsp+28],rax
       mov       rsi,rcx
       xor       edi,edi
       xor       ebx,ebx
       mov       rcx,[rsi+18]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L01
M00_L00:
       mov       rcx,[rsi+18]
       cmp       ebx,[rcx+8]
       jae       short M00_L02
       movsxd    rdx,ebx
       shl       rdx,4
       lea       rcx,[rcx+rdx+10]
       vmovdqu   xmm0,xmmword ptr [rcx]
       vmovdqu   xmmword ptr [rsp+20],xmm0
       mov       ecx,[rsp+20]
       mov       edx,[rsp+24]
       mov       r8d,[rsp+28]
       call      Cool.Ansi.Foreground(Int32, Int32, Int32)
       add       edi,[rax+8]
       inc       ebx
       mov       rax,[rsi+18]
       cmp       [rax+8],ebx
       jg        short M00_L00
M00_L01:
       mov       eax,edi
       add       rsp,30
       pop       rbx
       pop       rsi
       pop       rdi
       ret
M00_L02:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 120
```
```assembly
; Cool.Ansi.Foreground(Int32, Int32, Int32)
       push      rdi
       push      rsi
       push      rbp
       push      rbx
       sub       rsp,28
       mov       esi,ecx
       mov       edi,edx
       mov       ebx,r8d
       mov       rcx,[144AC280]
       mov       edx,14
       cmp       [rcx],ecx
       call      Cool.StringBuilderPool.Rent(Int32)
       mov       rbp,rax
       mov       rdx,[144B2010]
       mov       rcx,rbp
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(System.String)
       mov       rcx,rax
       mov       rdx,[144B2018]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(System.String)
       mov       rcx,rax
       mov       edx,esi
       call      Cool.Ansi.AppendFastInt(System.Text.StringBuilder, Int32)
       mov       rcx,rax
       mov       edx,3B
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       mov       rcx,rax
       mov       edx,edi
       call      Cool.Ansi.AppendFastInt(System.Text.StringBuilder, Int32)
       mov       rcx,rax
       mov       edx,3B
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       mov       rcx,rax
       mov       edx,ebx
       call      Cool.Ansi.AppendFastInt(System.Text.StringBuilder, Int32)
       mov       rcx,rax
       mov       edx,6D
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       mov       rcx,rbp
       call      System.Text.StringBuilder.ToString()
       mov       rsi,rax
       mov       rcx,[144AC280]
       mov       rdx,rbp
       cmp       [rcx],ecx
       call      Cool.StringBuilderPool.Return(System.Text.StringBuilder)
       mov       rax,rsi
       add       rsp,28
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       ret
; Total bytes of code 190
```

## .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256
```assembly
; Cool.Benchmarks.AnsiInliningBenchmarks.EscapeSGR_ReturnLen()
       push      rdi
       push      rsi
       push      rbx
       sub       rsp,20
       mov       rsi,rcx
       xor       edi,edi
       xor       ebx,ebx
       mov       rcx,[rsi+10]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L01
M00_L00:
       mov       rcx,[13572000]
       call      Cool.Ansi.EscapeSGR(System.String)
       add       edi,[rax+8]
       inc       ebx
       mov       rax,[rsi+10]
       cmp       [rax+8],ebx
       jg        short M00_L00
M00_L01:
       mov       eax,edi
       add       rsp,20
       pop       rbx
       pop       rsi
       pop       rdi
       ret
; Total bytes of code 61
```
```assembly
; Cool.Ansi.EscapeSGR(System.String)
       mov       rdx,rcx
       mov       rcx,[13572008]
       mov       r8,[13571F98]
       mov       rax,offset System.String.Concat(System.String, System.String, System.String)
       jmp       rax
; Total bytes of code 32
```

## .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256
```assembly
; Cool.Benchmarks.AnsiInliningBenchmarks.Bold_ReturnLen()
       push      rdi
       push      rsi
       sub       rsp,28
       xor       esi,esi
       xor       edi,edi
M00_L00:
       mov       rcx,[13FC2000]
       call      Cool.Ansi.Bold(System.String)
       add       esi,[rax+8]
       inc       edi
       cmp       edi,186A0
       jl        short M00_L00
       mov       eax,esi
       add       rsp,28
       pop       rsi
       pop       rdi
       ret
; Total bytes of code 45
```
```assembly
; Cool.Ansi.Bold(System.String)
       mov       rdx,rcx
       mov       rcx,[13FC2008]
       mov       r8,[13FC2010]
       mov       rax,offset System.String.Concat(System.String, System.String, System.String)
       jmp       rax
; Total bytes of code 32
```

