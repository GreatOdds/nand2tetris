@__END_TOOL
0;JMP
(__START_LT)
@R15
M=D
@SP
AM=M-1
D=M
A=A-1
D=M-D
M=0
@__END_LT
D;JGE
@SP
A=M-1
M=-1
(__END_LT)
@R15
A=M
0;JMP
(__START_EQ)
@R15
M=D
@SP
AM=M-1
D=M
A=A-1
D=M-D
M=0
@__END_EQ
D;JNE
@SP
A=M-1
M=-1
(__END_EQ)
@R15
A=M
0;JMP
(__START_GT)
@R15
M=D
@SP
AM=M-1
D=M
A=A-1
D=M-D
M=0
@__END_GT
D;JLE
@SP
A=M-1
M=-1
(__END_GT)
@R15
A=M
0;JMP
(__START_CALL)
@SP
AM=M+1
A=A-1
M=D
@LCL
D=M
@SP
AM=M+1
A=A-1
M=D
@ARG
D=M
@SP
AM=M+1
A=A-1
M=D
@THIS
D=M
@SP
AM=M+1
A=A-1
M=D
@THAT
D=M
@SP
AM=M+1
A=A-1
M=D
@SP
D=M
@LCL
M=D
@R14
D=D-M
@ARG
M=D
@R15
A=M
0;JMP
(__START_RETURN)
// store endFrame in R15
@LCL
D=M
@R15
M=D
// store retAddr in R14
@5
D=A
@R15
A=M-D
D=M
@R14
M=D
// get return value
@SP
AM=M-1
D=M
@ARG
A=M
M=D
// reposition SP
D=A+1
@SP
M=D
// restore THAT, THIS, ARG, LCL
@R15
A=M-1
D=M
@THAT
M=D
@2
D=A
@R15
A=M-D
D=M
@THIS
M=D
@3
D=A
@R15
A=M-D
D=M
@ARG
M=D
@4
D=A
@R15
A=M-D
D=M
@LCL
M=D
// jump to retAddr
@R14
A=M
0;JMP
(__END_TOOL)
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
// pop pointer 1
@THAT
D=A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push constant 0
@0
D=A
@SP
AM=M+1
A=A-1
M=D
// pop that 0
@THAT
D=M
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push constant 1
@1
D=A
@SP
AM=M+1
A=A-1
M=D
// pop that 1
@THAT
D=M
@1
D=D+A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push argument 0
@ARG
A=M
D=M
@SP
AM=M+1
A=A-1
M=D
// push constant 2
@2
D=A
@SP
AM=M+1
A=A-1
M=D
// sub
@SP
AM=M-1
D=M
A=A-1
M=M-D
// pop argument 0
@ARG
D=M
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// label LOOP
(LOOP)
// push argument 0
@ARG
A=M
D=M
@SP
AM=M+1
A=A-1
M=D
// if-goto COMPUTE_ELEMENT
@SP
AM=M-1
D=M
@COMPUTE_ELEMENT
D;JNE
// goto END
@END
0;JMP
// label COMPUTE_ELEMENT
(COMPUTE_ELEMENT)
// push that 0
@THAT
A=M
D=M
@SP
AM=M+1
A=A-1
M=D
// push that 1
@THAT
D=M
@1
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
A=A-1
M=D+M
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
// push pointer 1
@THAT
D=M
@SP
AM=M+1
A=A-1
M=D
// push constant 1
@1
D=A
@SP
AM=M+1
A=A-1
M=D
// add
@SP
AM=M-1
D=M
A=A-1
M=D+M
// pop pointer 1
@THAT
D=A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push argument 0
@ARG
A=M
D=M
@SP
AM=M+1
A=A-1
M=D
// push constant 1
@1
D=A
@SP
AM=M+1
A=A-1
M=D
// sub
@SP
AM=M-1
D=M
A=A-1
M=M-D
// pop argument 0
@ARG
D=M
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// goto LOOP
@LOOP
0;JMP
// label END
(END)