## .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256
```assembly
; Cool.Benchmarks.BitmapToStringBenchmarks.ToString_Wide()
       sub       rsp,28
       cmp       [rcx],ecx
       add       rcx,8
       call      Cool.Bitmap.ToString()
       mov       eax,[rax+8]
       add       rsp,28
       ret
; Total bytes of code 23
```
```assembly
; Cool.Bitmap.ToString()
       push      rbp
       push      r15
       push      r14
       push      r13
       push      r12
       push      rdi
       push      rsi
       push      rbx
       sub       rsp,0B8
       lea       rbp,[rsp+0F0]
       mov       rsi,rcx
       lea       rdi,[rbp-0C0]
       mov       ecx,22
       xor       eax,eax
       rep stosd
       mov       rcx,rsi
       mov       [rbp-0D0],rsp
       mov       rax,2BCCB0410B6
       mov       [rbp-40],rax
       mov       rsi,rcx
       mov       ecx,[rsi+8]
       shr       ecx,5
       lea       edi,[rcx+1]
       mov       ecx,[rsi+8]
       and       ecx,1F
       inc       ecx
       cmp       ecx,20
       je        short M01_L00
       mov       edx,1
       shl       edx,cl
       lea       ecx,[rdx-1]
       jmp       short M01_L01
M01_L00:
       mov       ecx,0FFFFFFFF
M01_L01:
       mov       ebx,ecx
       mov       rcx,[13F8C030]
       mov       edx,100
       cmp       [rcx],ecx
       call      Cool.StringBuilderPool.Rent(Int32)
       mov       [rbp-0B8],rax
       mov       r14d,1
       xor       r15d,r15d
       xor       r12d,r12d
       xor       r13d,r13d
       xor       eax,eax
       test      edi,edi
       jle       near ptr M01_L21
M01_L02:
       mov       rcx,[rsi]
       movsxd    rdx,eax
       mov       r8d,[rcx+rdx*4]
       lea       ecx,[rdi-1]
       cmp       eax,ecx
       jne       near ptr M01_L19
       mov       [rbp-0A4],ebx
       and       r8d,ebx
       test      r8d,r8d
       mov       ebx,[rbp-0A4]
       je        near ptr M01_L20
M01_L03:
       mov       rcx,[13F8BF20]
       mov       edx,r8d
       neg       edx
       mov       [rbp-0AC],r8d
       mov       r9d,r8d
       and       edx,r9d
       imul      edx,77CB531
       shr       edx,1B
       cmp       edx,[rcx+8]
       jae       short M01_L04
       movsxd    rdx,edx
       mov       ecx,[rcx+rdx*4+10]
       mov       [rbp-0A8],eax
       mov       edx,eax
       shl       edx,5
       lea       r9d,[rdx+rcx]
       test      r15d,r15d
       jne       short M01_L05
       mov       r15d,1
       mov       r13d,r9d
       mov       r12d,r13d
       jmp       near ptr M01_L18
M01_L04:
       call      CORINFO_HELP_RNGCHKFAIL
M01_L05:
       lea       ecx,[r13+1]
       cmp       r9d,ecx
       jne       short M01_L06
       mov       r13d,r9d
       jmp       near ptr M01_L18
M01_L06:
       test      r14d,r14d
       jne       short M01_L07
       mov       [rbp-0B0],r9d
       mov       rcx,[rbp-0B8]
       mov       edx,2C
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       jmp       short M01_L08
M01_L07:
       xor       r14d,r14d
       mov       [rbp-0B0],r9d
M01_L08:
       cmp       r12d,r13d
       jne       short M01_L11
       test      r12d,r12d
       jne       short M01_L09
       mov       rcx,[rbp-0B8]
       mov       edx,30
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       jmp       near ptr M01_L17
M01_L09:
       lea       rdx,[rbp-50]
       mov       ecx,8
M01_L10:
       mov       r10d,r12d
       and       r10d,0F
       dec       ecx
       movsxd    r11,ecx
       mov       r13,[13F8BF18]
       cmp       r10d,[r13+8]
       jae       near ptr M01_L04
       movsxd    r10,r10d
       movzx     r10d,word ptr [r13+r10*2+0C]
       mov       [rdx+r11*2],r10w
       shr       r12d,4
       test      r12d,r12d
       jne       short M01_L10
       movsxd    r10,ecx
       lea       rdx,[rdx+r10*2]
       neg       ecx
       lea       r8d,[rcx+8]
       mov       rcx,[rbp-0B8]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char*, Int32)
       jmp       near ptr M01_L17
M01_L11:
       test      r12d,r12d
       jne       short M01_L12
       mov       rcx,[rbp-0B8]
       mov       edx,30
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       jmp       short M01_L14
M01_L12:
       lea       rdx,[rbp-60]
       mov       ecx,8
       mov       [rbp-0A4],ebx
M01_L13:
       mov       r10d,r12d
       and       r10d,0F
       dec       ecx
       movsxd    r11,ecx
       mov       rbx,[13F8BF18]
       cmp       r10d,[rbx+8]
       jae       near ptr M01_L04
       movsxd    r10,r10d
       movzx     r10d,word ptr [rbx+r10*2+0C]
       mov       [rdx+r11*2],r10w
       shr       r12d,4
       test      r12d,r12d
       jne       short M01_L13
       mov       ebx,[rbp-0A4]
       movsxd    r10,ecx
       lea       rdx,[rdx+r10*2]
       neg       ecx
       lea       r8d,[rcx+8]
       mov       rcx,[rbp-0B8]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char*, Int32)
M01_L14:
       mov       rcx,[rbp-0B8]
       mov       edx,2D
       call      System.Text.StringBuilder.Append(Char)
       test      r13d,r13d
       jne       short M01_L15
       mov       rcx,[rbp-0B8]
       mov       edx,30
       call      System.Text.StringBuilder.Append(Char)
       jmp       short M01_L17
M01_L15:
       lea       rdx,[rbp-70]
       mov       r8d,8
M01_L16:
       mov       ecx,r13d
       and       ecx,0F
       dec       r8d
       movsxd    rax,r8d
       mov       r9,[13F8BF18]
       cmp       ecx,[r9+8]
       jae       near ptr M01_L04
       movsxd    rcx,ecx
       movzx     ecx,word ptr [r9+rcx*2+0C]
       mov       [rdx+rax*2],cx
       shr       r13d,4
       test      r13d,r13d
       jne       short M01_L16
       movsxd    rcx,r8d
       lea       rdx,[rdx+rcx*2]
       neg       r8d
       add       r8d,8
       mov       rcx,[rbp-0B8]
       call      System.Text.StringBuilder.Append(Char*, Int32)
M01_L17:
       mov       r13d,[rbp-0B0]
       mov       r12d,r13d
M01_L18:
       mov       r8d,[rbp-0AC]
       lea       ecx,[r8-1]
       and       r8d,ecx
       mov       eax,[rbp-0A8]
M01_L19:
       test      r8d,r8d
       jne       near ptr M01_L03
M01_L20:
       inc       eax
       cmp       eax,edi
       jl        near ptr M01_L02
M01_L21:
       test      r15d,r15d
       je        near ptr M01_L31
       test      r14d,r14d
       jne       short M01_L22
       mov       rcx,[rbp-0B8]
       mov       edx,2C
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
M01_L22:
       cmp       r12d,r13d
       jne       near ptr M01_L25
       test      r12d,r12d
       jne       short M01_L23
       mov       rcx,[rbp-0B8]
       mov       edx,30
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       jmp       near ptr M01_L31
M01_L23:
       lea       rdx,[rbp-80]
       mov       r8d,8
M01_L24:
       mov       ecx,r12d
       and       ecx,0F
       dec       r8d
       movsxd    rax,r8d
       mov       r9,[13F8BF18]
       cmp       ecx,[r9+8]
       jae       near ptr M01_L04
       movsxd    rcx,ecx
       movzx     ecx,word ptr [r9+rcx*2+0C]
       mov       [rdx+rax*2],cx
       shr       r12d,4
       test      r12d,r12d
       jne       short M01_L24
       movsxd    rcx,r8d
       lea       rdx,[rdx+rcx*2]
       neg       r8d
       add       r8d,8
       mov       rcx,[rbp-0B8]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char*, Int32)
       jmp       near ptr M01_L31
M01_L25:
       mov       esi,r12d
       test      esi,esi
       jne       short M01_L26
       mov       rcx,[rbp-0B8]
       mov       edx,30
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       jmp       short M01_L28
M01_L26:
       lea       rdx,[rbp-90]
       mov       r8d,8
M01_L27:
       mov       ecx,esi
       and       ecx,0F
       dec       r8d
       movsxd    rax,r8d
       mov       r9,[13F8BF18]
       cmp       ecx,[r9+8]
       jae       near ptr M01_L04
       movsxd    rcx,ecx
       movzx     ecx,word ptr [r9+rcx*2+0C]
       mov       [rdx+rax*2],cx
       shr       esi,4
       test      esi,esi
       jne       short M01_L27
       movsxd    rcx,r8d
       lea       rdx,[rdx+rcx*2]
       neg       r8d
       add       r8d,8
       mov       rcx,[rbp-0B8]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char*, Int32)
M01_L28:
       mov       rcx,[rbp-0B8]
       mov       edx,2D
       call      System.Text.StringBuilder.Append(Char)
       mov       esi,r13d
       test      esi,esi
       jne       short M01_L29
       mov       rcx,[rbp-0B8]
       mov       edx,30
       call      System.Text.StringBuilder.Append(Char)
       jmp       short M01_L31
M01_L29:
       lea       rdx,[rbp-0A0]
       mov       r8d,8
M01_L30:
       mov       ecx,esi
       and       ecx,0F
       dec       r8d
       movsxd    rax,r8d
       mov       r9,[13F8BF18]
       cmp       ecx,[r9+8]
       jae       near ptr M01_L04
       movsxd    rcx,ecx
       movzx     ecx,word ptr [r9+rcx*2+0C]
       mov       [rdx+rax*2],cx
       shr       esi,4
       test      esi,esi
       jne       short M01_L30
       movsxd    rcx,r8d
       lea       rdx,[rdx+rcx*2]
       neg       r8d
       add       r8d,8
       mov       rcx,[rbp-0B8]
       call      System.Text.StringBuilder.Append(Char*, Int32)
M01_L31:
       mov       rcx,[rbp-0B8]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.ToString()
       mov       [rbp-0C0],rax
       mov       rcx,rsp
       call      M01_L33
       nop
       mov       rax,[rbp-0C0]
       mov       rcx,2BCCB0410B6
       cmp       [rbp-40],rcx
       je        short M01_L32
       call      CORINFO_HELP_FAIL_FAST
M01_L32:
       nop
       lea       rsp,[rbp-38]
       pop       rbx
       pop       rsi
       pop       rdi
       pop       r12
       pop       r13
       pop       r14
       pop       r15
       pop       rbp
       ret
M01_L33:
       push      rbp
       push      r15
       push      r14
       push      r13
       push      r12
       push      rdi
       push      rsi
       push      rbx
       sub       rsp,28
       mov       rbp,[rcx+20]
       mov       [rsp+20],rbp
       lea       rbp,[rbp+0F0]
       mov       rcx,[13F8C030]
       mov       rdx,[rbp-0B8]
       cmp       [rcx],ecx
       call      Cool.StringBuilderPool.Return(System.Text.StringBuilder)
       nop
       add       rsp,28
       pop       rbx
       pop       rsi
       pop       rdi
       pop       r12
       pop       r13
       pop       r14
       pop       r15
       pop       rbp
       ret
; Total bytes of code 1356
```

## .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256
```assembly
; Cool.Benchmarks.BitmapToStringBenchmarks.ToString_Sparse()
       sub       rsp,28
       cmp       [rcx],ecx
       add       rcx,18
       call      Cool.Bitmap.ToString()
       mov       eax,[rax+8]
       add       rsp,28
       ret
; Total bytes of code 23
```
```assembly
; Cool.Bitmap.ToString()
       push      rbp
       push      r15
       push      r14
       push      r13
       push      r12
       push      rdi
       push      rsi
       push      rbx
       sub       rsp,0B8
       lea       rbp,[rsp+0F0]
       mov       rsi,rcx
       lea       rdi,[rbp-0C0]
       mov       ecx,22
       xor       eax,eax
       rep stosd
       mov       rcx,rsi
       mov       [rbp-0D0],rsp
       mov       rax,6A315131EB07
       mov       [rbp-40],rax
       mov       rsi,rcx
       mov       ecx,[rsi+8]
       shr       ecx,5
       lea       edi,[rcx+1]
       mov       ecx,[rsi+8]
       and       ecx,1F
       inc       ecx
       cmp       ecx,20
       je        short M01_L00
       mov       edx,1
       shl       edx,cl
       lea       ecx,[rdx-1]
       jmp       short M01_L01
M01_L00:
       mov       ecx,0FFFFFFFF
M01_L01:
       mov       ebx,ecx
       mov       rcx,[13E3C030]
       mov       edx,100
       cmp       [rcx],ecx
       call      Cool.StringBuilderPool.Rent(Int32)
       mov       [rbp-0B8],rax
       mov       r14d,1
       xor       r15d,r15d
       xor       r12d,r12d
       xor       r13d,r13d
       xor       eax,eax
       test      edi,edi
       jle       near ptr M01_L21
M01_L02:
       mov       rcx,[rsi]
       movsxd    rdx,eax
       mov       r8d,[rcx+rdx*4]
       lea       ecx,[rdi-1]
       cmp       eax,ecx
       jne       near ptr M01_L19
       mov       [rbp-0A4],ebx
       and       r8d,ebx
       test      r8d,r8d
       mov       ebx,[rbp-0A4]
       je        near ptr M01_L20
M01_L03:
       mov       rcx,[13E3BF20]
       mov       edx,r8d
       neg       edx
       mov       [rbp-0AC],r8d
       mov       r9d,r8d
       and       edx,r9d
       imul      edx,77CB531
       shr       edx,1B
       cmp       edx,[rcx+8]
       jae       short M01_L04
       movsxd    rdx,edx
       mov       ecx,[rcx+rdx*4+10]
       mov       [rbp-0A8],eax
       mov       edx,eax
       shl       edx,5
       lea       r9d,[rdx+rcx]
       test      r15d,r15d
       jne       short M01_L05
       mov       r15d,1
       mov       r13d,r9d
       mov       r12d,r13d
       jmp       near ptr M01_L18
M01_L04:
       call      CORINFO_HELP_RNGCHKFAIL
M01_L05:
       lea       ecx,[r13+1]
       cmp       r9d,ecx
       jne       short M01_L06
       mov       r13d,r9d
       jmp       near ptr M01_L18
M01_L06:
       test      r14d,r14d
       jne       short M01_L07
       mov       [rbp-0B0],r9d
       mov       rcx,[rbp-0B8]
       mov       edx,2C
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       jmp       short M01_L08
M01_L07:
       xor       r14d,r14d
       mov       [rbp-0B0],r9d
M01_L08:
       cmp       r12d,r13d
       jne       short M01_L11
       test      r12d,r12d
       jne       short M01_L09
       mov       rcx,[rbp-0B8]
       mov       edx,30
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       jmp       near ptr M01_L17
M01_L09:
       lea       rdx,[rbp-50]
       mov       ecx,8
M01_L10:
       mov       r10d,r12d
       and       r10d,0F
       dec       ecx
       movsxd    r11,ecx
       mov       r13,[13E3BF18]
       cmp       r10d,[r13+8]
       jae       near ptr M01_L04
       movsxd    r10,r10d
       movzx     r10d,word ptr [r13+r10*2+0C]
       mov       [rdx+r11*2],r10w
       shr       r12d,4
       test      r12d,r12d
       jne       short M01_L10
       movsxd    r10,ecx
       lea       rdx,[rdx+r10*2]
       neg       ecx
       lea       r8d,[rcx+8]
       mov       rcx,[rbp-0B8]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char*, Int32)
       jmp       near ptr M01_L17
M01_L11:
       test      r12d,r12d
       jne       short M01_L12
       mov       rcx,[rbp-0B8]
       mov       edx,30
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       jmp       short M01_L14
M01_L12:
       lea       rdx,[rbp-60]
       mov       ecx,8
       mov       [rbp-0A4],ebx
M01_L13:
       mov       r10d,r12d
       and       r10d,0F
       dec       ecx
       movsxd    r11,ecx
       mov       rbx,[13E3BF18]
       cmp       r10d,[rbx+8]
       jae       near ptr M01_L04
       movsxd    r10,r10d
       movzx     r10d,word ptr [rbx+r10*2+0C]
       mov       [rdx+r11*2],r10w
       shr       r12d,4
       test      r12d,r12d
       jne       short M01_L13
       mov       ebx,[rbp-0A4]
       movsxd    r10,ecx
       lea       rdx,[rdx+r10*2]
       neg       ecx
       lea       r8d,[rcx+8]
       mov       rcx,[rbp-0B8]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char*, Int32)
M01_L14:
       mov       rcx,[rbp-0B8]
       mov       edx,2D
       call      System.Text.StringBuilder.Append(Char)
       test      r13d,r13d
       jne       short M01_L15
       mov       rcx,[rbp-0B8]
       mov       edx,30
       call      System.Text.StringBuilder.Append(Char)
       jmp       short M01_L17
M01_L15:
       lea       rdx,[rbp-70]
       mov       r8d,8
M01_L16:
       mov       ecx,r13d
       and       ecx,0F
       dec       r8d
       movsxd    rax,r8d
       mov       r9,[13E3BF18]
       cmp       ecx,[r9+8]
       jae       near ptr M01_L04
       movsxd    rcx,ecx
       movzx     ecx,word ptr [r9+rcx*2+0C]
       mov       [rdx+rax*2],cx
       shr       r13d,4
       test      r13d,r13d
       jne       short M01_L16
       movsxd    rcx,r8d
       lea       rdx,[rdx+rcx*2]
       neg       r8d
       add       r8d,8
       mov       rcx,[rbp-0B8]
       call      System.Text.StringBuilder.Append(Char*, Int32)
M01_L17:
       mov       r13d,[rbp-0B0]
       mov       r12d,r13d
M01_L18:
       mov       r8d,[rbp-0AC]
       lea       ecx,[r8-1]
       and       r8d,ecx
       mov       eax,[rbp-0A8]
M01_L19:
       test      r8d,r8d
       jne       near ptr M01_L03
M01_L20:
       inc       eax
       cmp       eax,edi
       jl        near ptr M01_L02
M01_L21:
       test      r15d,r15d
       je        near ptr M01_L31
       test      r14d,r14d
       jne       short M01_L22
       mov       rcx,[rbp-0B8]
       mov       edx,2C
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
M01_L22:
       cmp       r12d,r13d
       jne       near ptr M01_L25
       test      r12d,r12d
       jne       short M01_L23
       mov       rcx,[rbp-0B8]
       mov       edx,30
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       jmp       near ptr M01_L31
M01_L23:
       lea       rdx,[rbp-80]
       mov       r8d,8
M01_L24:
       mov       ecx,r12d
       and       ecx,0F
       dec       r8d
       movsxd    rax,r8d
       mov       r9,[13E3BF18]
       cmp       ecx,[r9+8]
       jae       near ptr M01_L04
       movsxd    rcx,ecx
       movzx     ecx,word ptr [r9+rcx*2+0C]
       mov       [rdx+rax*2],cx
       shr       r12d,4
       test      r12d,r12d
       jne       short M01_L24
       movsxd    rcx,r8d
       lea       rdx,[rdx+rcx*2]
       neg       r8d
       add       r8d,8
       mov       rcx,[rbp-0B8]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char*, Int32)
       jmp       near ptr M01_L31
M01_L25:
       mov       esi,r12d
       test      esi,esi
       jne       short M01_L26
       mov       rcx,[rbp-0B8]
       mov       edx,30
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       jmp       short M01_L28
M01_L26:
       lea       rdx,[rbp-90]
       mov       r8d,8
M01_L27:
       mov       ecx,esi
       and       ecx,0F
       dec       r8d
       movsxd    rax,r8d
       mov       r9,[13E3BF18]
       cmp       ecx,[r9+8]
       jae       near ptr M01_L04
       movsxd    rcx,ecx
       movzx     ecx,word ptr [r9+rcx*2+0C]
       mov       [rdx+rax*2],cx
       shr       esi,4
       test      esi,esi
       jne       short M01_L27
       movsxd    rcx,r8d
       lea       rdx,[rdx+rcx*2]
       neg       r8d
       add       r8d,8
       mov       rcx,[rbp-0B8]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char*, Int32)
M01_L28:
       mov       rcx,[rbp-0B8]
       mov       edx,2D
       call      System.Text.StringBuilder.Append(Char)
       mov       esi,r13d
       test      esi,esi
       jne       short M01_L29
       mov       rcx,[rbp-0B8]
       mov       edx,30
       call      System.Text.StringBuilder.Append(Char)
       jmp       short M01_L31
M01_L29:
       lea       rdx,[rbp-0A0]
       mov       r8d,8
M01_L30:
       mov       ecx,esi
       and       ecx,0F
       dec       r8d
       movsxd    rax,r8d
       mov       r9,[13E3BF18]
       cmp       ecx,[r9+8]
       jae       near ptr M01_L04
       movsxd    rcx,ecx
       movzx     ecx,word ptr [r9+rcx*2+0C]
       mov       [rdx+rax*2],cx
       shr       esi,4
       test      esi,esi
       jne       short M01_L30
       movsxd    rcx,r8d
       lea       rdx,[rdx+rcx*2]
       neg       r8d
       add       r8d,8
       mov       rcx,[rbp-0B8]
       call      System.Text.StringBuilder.Append(Char*, Int32)
M01_L31:
       mov       rcx,[rbp-0B8]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.ToString()
       mov       [rbp-0C0],rax
       mov       rcx,rsp
       call      M01_L33
       nop
       mov       rax,[rbp-0C0]
       mov       rcx,6A315131EB07
       cmp       [rbp-40],rcx
       je        short M01_L32
       call      CORINFO_HELP_FAIL_FAST
M01_L32:
       nop
       lea       rsp,[rbp-38]
       pop       rbx
       pop       rsi
       pop       rdi
       pop       r12
       pop       r13
       pop       r14
       pop       r15
       pop       rbp
       ret
M01_L33:
       push      rbp
       push      r15
       push      r14
       push      r13
       push      r12
       push      rdi
       push      rsi
       push      rbx
       sub       rsp,28
       mov       rbp,[rcx+20]
       mov       [rsp+20],rbp
       lea       rbp,[rbp+0F0]
       mov       rcx,[13E3C030]
       mov       rdx,[rbp-0B8]
       cmp       [rcx],ecx
       call      Cool.StringBuilderPool.Return(System.Text.StringBuilder)
       nop
       add       rsp,28
       pop       rbx
       pop       rsi
       pop       rdi
       pop       r12
       pop       r13
       pop       r14
       pop       r15
       pop       rbp
       ret
; Total bytes of code 1356
```

## .NET Framework 4.8.1 (4.8.9325.0), X64 RyuJIT VectorSize=256
```assembly
; Cool.Benchmarks.BitmapToStringBenchmarks.ToString_Dense()
       sub       rsp,28
       cmp       [rcx],ecx
       add       rcx,28
       call      Cool.Bitmap.ToString()
       mov       eax,[rax+8]
       add       rsp,28
       ret
; Total bytes of code 23
```
```assembly
; Cool.Bitmap.ToString()
       push      rbp
       push      r15
       push      r14
       push      r13
       push      r12
       push      rdi
       push      rsi
       push      rbx
       sub       rsp,0B8
       lea       rbp,[rsp+0F0]
       mov       rsi,rcx
       lea       rdi,[rbp-0C0]
       mov       ecx,22
       xor       eax,eax
       rep stosd
       mov       rcx,rsi
       mov       [rbp-0D0],rsp
       mov       rax,12598953CE60
       mov       [rbp-40],rax
       mov       rsi,rcx
       mov       ecx,[rsi+8]
       shr       ecx,5
       lea       edi,[rcx+1]
       mov       ecx,[rsi+8]
       and       ecx,1F
       inc       ecx
       cmp       ecx,20
       je        short M01_L00
       mov       edx,1
       shl       edx,cl
       lea       ecx,[rdx-1]
       jmp       short M01_L01
M01_L00:
       mov       ecx,0FFFFFFFF
M01_L01:
       mov       ebx,ecx
       mov       rcx,[1438C030]
       mov       edx,100
       cmp       [rcx],ecx
       call      Cool.StringBuilderPool.Rent(Int32)
       mov       [rbp-0B8],rax
       mov       r14d,1
       xor       r15d,r15d
       xor       r12d,r12d
       xor       r13d,r13d
       xor       eax,eax
       test      edi,edi
       jle       near ptr M01_L21
M01_L02:
       mov       rcx,[rsi]
       movsxd    rdx,eax
       mov       r8d,[rcx+rdx*4]
       lea       ecx,[rdi-1]
       cmp       eax,ecx
       jne       near ptr M01_L19
       mov       [rbp-0A4],ebx
       and       r8d,ebx
       test      r8d,r8d
       mov       ebx,[rbp-0A4]
       je        near ptr M01_L20
M01_L03:
       mov       rcx,[1438BF20]
       mov       edx,r8d
       neg       edx
       mov       [rbp-0AC],r8d
       mov       r9d,r8d
       and       edx,r9d
       imul      edx,77CB531
       shr       edx,1B
       cmp       edx,[rcx+8]
       jae       short M01_L04
       movsxd    rdx,edx
       mov       ecx,[rcx+rdx*4+10]
       mov       [rbp-0A8],eax
       mov       edx,eax
       shl       edx,5
       lea       r9d,[rdx+rcx]
       test      r15d,r15d
       jne       short M01_L05
       mov       r15d,1
       mov       r13d,r9d
       mov       r12d,r13d
       jmp       near ptr M01_L18
M01_L04:
       call      CORINFO_HELP_RNGCHKFAIL
M01_L05:
       lea       ecx,[r13+1]
       cmp       r9d,ecx
       jne       short M01_L06
       mov       r13d,r9d
       jmp       near ptr M01_L18
M01_L06:
       test      r14d,r14d
       jne       short M01_L07
       mov       [rbp-0B0],r9d
       mov       rcx,[rbp-0B8]
       mov       edx,2C
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       jmp       short M01_L08
M01_L07:
       xor       r14d,r14d
       mov       [rbp-0B0],r9d
M01_L08:
       cmp       r12d,r13d
       jne       short M01_L11
       test      r12d,r12d
       jne       short M01_L09
       mov       rcx,[rbp-0B8]
       mov       edx,30
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       jmp       near ptr M01_L17
M01_L09:
       lea       rdx,[rbp-50]
       mov       ecx,8
M01_L10:
       mov       r10d,r12d
       and       r10d,0F
       dec       ecx
       movsxd    r11,ecx
       mov       r13,[1438BF18]
       cmp       r10d,[r13+8]
       jae       near ptr M01_L04
       movsxd    r10,r10d
       movzx     r10d,word ptr [r13+r10*2+0C]
       mov       [rdx+r11*2],r10w
       shr       r12d,4
       test      r12d,r12d
       jne       short M01_L10
       movsxd    r10,ecx
       lea       rdx,[rdx+r10*2]
       neg       ecx
       lea       r8d,[rcx+8]
       mov       rcx,[rbp-0B8]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char*, Int32)
       jmp       near ptr M01_L17
M01_L11:
       test      r12d,r12d
       jne       short M01_L12
       mov       rcx,[rbp-0B8]
       mov       edx,30
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       jmp       short M01_L14
M01_L12:
       lea       rdx,[rbp-60]
       mov       ecx,8
       mov       [rbp-0A4],ebx
M01_L13:
       mov       r10d,r12d
       and       r10d,0F
       dec       ecx
       movsxd    r11,ecx
       mov       rbx,[1438BF18]
       cmp       r10d,[rbx+8]
       jae       near ptr M01_L04
       movsxd    r10,r10d
       movzx     r10d,word ptr [rbx+r10*2+0C]
       mov       [rdx+r11*2],r10w
       shr       r12d,4
       test      r12d,r12d
       jne       short M01_L13
       mov       ebx,[rbp-0A4]
       movsxd    r10,ecx
       lea       rdx,[rdx+r10*2]
       neg       ecx
       lea       r8d,[rcx+8]
       mov       rcx,[rbp-0B8]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char*, Int32)
M01_L14:
       mov       rcx,[rbp-0B8]
       mov       edx,2D
       call      System.Text.StringBuilder.Append(Char)
       test      r13d,r13d
       jne       short M01_L15
       mov       rcx,[rbp-0B8]
       mov       edx,30
       call      System.Text.StringBuilder.Append(Char)
       jmp       short M01_L17
M01_L15:
       lea       rdx,[rbp-70]
       mov       r8d,8
M01_L16:
       mov       ecx,r13d
       and       ecx,0F
       dec       r8d
       movsxd    rax,r8d
       mov       r9,[1438BF18]
       cmp       ecx,[r9+8]
       jae       near ptr M01_L04
       movsxd    rcx,ecx
       movzx     ecx,word ptr [r9+rcx*2+0C]
       mov       [rdx+rax*2],cx
       shr       r13d,4
       test      r13d,r13d
       jne       short M01_L16
       movsxd    rcx,r8d
       lea       rdx,[rdx+rcx*2]
       neg       r8d
       add       r8d,8
       mov       rcx,[rbp-0B8]
       call      System.Text.StringBuilder.Append(Char*, Int32)
M01_L17:
       mov       r13d,[rbp-0B0]
       mov       r12d,r13d
M01_L18:
       mov       r8d,[rbp-0AC]
       lea       ecx,[r8-1]
       and       r8d,ecx
       mov       eax,[rbp-0A8]
M01_L19:
       test      r8d,r8d
       jne       near ptr M01_L03
M01_L20:
       inc       eax
       cmp       eax,edi
       jl        near ptr M01_L02
M01_L21:
       test      r15d,r15d
       je        near ptr M01_L31
       test      r14d,r14d
       jne       short M01_L22
       mov       rcx,[rbp-0B8]
       mov       edx,2C
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
M01_L22:
       cmp       r12d,r13d
       jne       near ptr M01_L25
       test      r12d,r12d
       jne       short M01_L23
       mov       rcx,[rbp-0B8]
       mov       edx,30
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       jmp       near ptr M01_L31
M01_L23:
       lea       rdx,[rbp-80]
       mov       r8d,8
M01_L24:
       mov       ecx,r12d
       and       ecx,0F
       dec       r8d
       movsxd    rax,r8d
       mov       r9,[1438BF18]
       cmp       ecx,[r9+8]
       jae       near ptr M01_L04
       movsxd    rcx,ecx
       movzx     ecx,word ptr [r9+rcx*2+0C]
       mov       [rdx+rax*2],cx
       shr       r12d,4
       test      r12d,r12d
       jne       short M01_L24
       movsxd    rcx,r8d
       lea       rdx,[rdx+rcx*2]
       neg       r8d
       add       r8d,8
       mov       rcx,[rbp-0B8]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char*, Int32)
       jmp       near ptr M01_L31
M01_L25:
       mov       esi,r12d
       test      esi,esi
       jne       short M01_L26
       mov       rcx,[rbp-0B8]
       mov       edx,30
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char)
       jmp       short M01_L28
M01_L26:
       lea       rdx,[rbp-90]
       mov       r8d,8
M01_L27:
       mov       ecx,esi
       and       ecx,0F
       dec       r8d
       movsxd    rax,r8d
       mov       r9,[1438BF18]
       cmp       ecx,[r9+8]
       jae       near ptr M01_L04
       movsxd    rcx,ecx
       movzx     ecx,word ptr [r9+rcx*2+0C]
       mov       [rdx+rax*2],cx
       shr       esi,4
       test      esi,esi
       jne       short M01_L27
       movsxd    rcx,r8d
       lea       rdx,[rdx+rcx*2]
       neg       r8d
       add       r8d,8
       mov       rcx,[rbp-0B8]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.Append(Char*, Int32)
M01_L28:
       mov       rcx,[rbp-0B8]
       mov       edx,2D
       call      System.Text.StringBuilder.Append(Char)
       mov       esi,r13d
       test      esi,esi
       jne       short M01_L29
       mov       rcx,[rbp-0B8]
       mov       edx,30
       call      System.Text.StringBuilder.Append(Char)
       jmp       short M01_L31
M01_L29:
       lea       rdx,[rbp-0A0]
       mov       r8d,8
M01_L30:
       mov       ecx,esi
       and       ecx,0F
       dec       r8d
       movsxd    rax,r8d
       mov       r9,[1438BF18]
       cmp       ecx,[r9+8]
       jae       near ptr M01_L04
       movsxd    rcx,ecx
       movzx     ecx,word ptr [r9+rcx*2+0C]
       mov       [rdx+rax*2],cx
       shr       esi,4
       test      esi,esi
       jne       short M01_L30
       movsxd    rcx,r8d
       lea       rdx,[rdx+rcx*2]
       neg       r8d
       add       r8d,8
       mov       rcx,[rbp-0B8]
       call      System.Text.StringBuilder.Append(Char*, Int32)
M01_L31:
       mov       rcx,[rbp-0B8]
       cmp       [rcx],ecx
       call      System.Text.StringBuilder.ToString()
       mov       [rbp-0C0],rax
       mov       rcx,rsp
       call      M01_L33
       nop
       mov       rax,[rbp-0C0]
       mov       rcx,12598953CE60
       cmp       [rbp-40],rcx
       je        short M01_L32
       call      CORINFO_HELP_FAIL_FAST
M01_L32:
       nop
       lea       rsp,[rbp-38]
       pop       rbx
       pop       rsi
       pop       rdi
       pop       r12
       pop       r13
       pop       r14
       pop       r15
       pop       rbp
       ret
M01_L33:
       push      rbp
       push      r15
       push      r14
       push      r13
       push      r12
       push      rdi
       push      rsi
       push      rbx
       sub       rsp,28
       mov       rbp,[rcx+20]
       mov       [rsp+20],rbp
       lea       rbp,[rbp+0F0]
       mov       rcx,[1438C030]
       mov       rdx,[rbp-0B8]
       cmp       [rcx],ecx
       call      Cool.StringBuilderPool.Return(System.Text.StringBuilder)
       nop
       add       rsp,28
       pop       rbx
       pop       rsi
       pop       rdi
       pop       r12
       pop       r13
       pop       r14
       pop       r15
       pop       rbp
       ret
; Total bytes of code 1356
```

