// push constant 10
@10
D=A
@SP
AM=M+1
A=A-1
M=D
// pop local 0
@LCL
D=M
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push constant 21
@21
D=A
@SP
AM=M+1
A=A-1
M=D
// push constant 22
@22
D=A
@SP
AM=M+1
A=A-1
M=D
// pop argument 2
@ARG
D=M
@2
D=D+A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// pop argument 1
@ARG
D=M
@1
D=D+A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push constant 36
@36
D=A
@SP
AM=M+1
A=A-1
M=D
// pop this 6
@THIS
D=M
@6
D=D+A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push constant 42
@42
D=A
@SP
AM=M+1
A=A-1
M=D
// push constant 45
@45
D=A
@SP
AM=M+1
A=A-1
M=D
// pop that 5
@THAT
D=M
@5
D=D+A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// pop that 2
@THAT
D=M
@2
D=D+A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push constant 510
@510
D=A
@SP
AM=M+1
A=A-1
M=D
// pop temp 6
@11
D=A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push local 0
@LCL
A=M
D=M
@SP
AM=M+1
A=A-1
M=D
// push that 5
@THAT
D=M
@5
A=D+A
D=M
@SP
AM=M+1
A=A-1
M=D
// add
@SP
AM=M-1
D=M
@SP
A=M-1
M=D+M
// push argument 1
@ARG
D=M
@1
A=D+A
D=M
@SP
AM=M+1
A=A-1
M=D
// sub
@SP
AM=M-1
D=M
@SP
A=M-1
M=M-D
// push this 6
@THIS
D=M
@6
A=D+A
D=M
@SP
AM=M+1
A=A-1
M=D
// push this 6
@THIS
D=M
@6
A=D+A
D=M
@SP
AM=M+1
A=A-1
M=D
// add
@SP
AM=M-1
D=M
@SP
A=M-1
M=D+M
// sub
@SP
AM=M-1
D=M
@SP
A=M-1
M=M-D
// push temp 6
@11
D=M
@SP
AM=M+1
A=A-1
M=D
// add
@SP
AM=M-1
D=M
@SP
A=M-1
M=D+M