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
// function SimpleFunction.test 2
(SimpleFunction.test)
@SP
A=M
M=0
A=A+1
M=0
A=A+1
D=A
@SP
M=D
// push local 0
@LCL
A=M
D=M
@SP
AM=M+1
A=A-1
M=D
// push local 1
@LCL
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
// not
@SP
A=M-1
M=!M
// push argument 0
@ARG
A=M
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
A=A-1
M=M-D
// return
@__START_RETURN
0;JMP
