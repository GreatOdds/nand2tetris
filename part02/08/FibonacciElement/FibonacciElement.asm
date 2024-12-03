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
@256
D=A
@SP
M=D
// call Sys.init 0
@Sys.init
D=A
@R15
M=D
@5
D=A
@R14
M=D
@$ret.0
D=A
@__START_CALL
0;JMP
($ret.0)
// function Sys.init 0
(Sys.init)
// push constant 4
@4
D=A
@SP
AM=M+1
A=A-1
M=D
// call Main.fibonacci 1
@Main.fibonacci
D=A
@R15
M=D
@6
D=A
@R14
M=D
@Sys.init$ret.0
D=A
@__START_CALL
0;JMP
(Sys.init$ret.0)
// label END
(Sys.init$END)
// goto END
@Sys.init$END
0;JMP
// function Main.fibonacci 0
(Main.fibonacci)
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
// lt
@__RET_LT0
D=A
@__START_LT
0;JMP
(__RET_LT0)
// if-goto N_LT_2
@SP
AM=M-1
D=M
@Main.fibonacci$N_LT_2
D;JNE
// goto N_GE_2
@Main.fibonacci$N_GE_2
0;JMP
// label N_LT_2
(Main.fibonacci$N_LT_2)
// push argument 0
@ARG
A=M
D=M
@SP
AM=M+1
A=A-1
M=D
// return
@__START_RETURN
0;JMP
// label N_GE_2
(Main.fibonacci$N_GE_2)
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
// call Main.fibonacci 1
@Main.fibonacci
D=A
@R15
M=D
@6
D=A
@R14
M=D
@Main.fibonacci$ret.0
D=A
@__START_CALL
0;JMP
(Main.fibonacci$ret.0)
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
// call Main.fibonacci 1
@Main.fibonacci
D=A
@R15
M=D
@6
D=A
@R14
M=D
@Main.fibonacci$ret.1
D=A
@__START_CALL
0;JMP
(Main.fibonacci$ret.1)
// add
@SP
AM=M-1
D=M
A=A-1
M=D+M
// return
@__START_RETURN
0;JMP
