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
// push constant 6
@6
D=A
@SP
AM=M+1
A=A-1
M=D
// push constant 8
@8
D=A
@SP
AM=M+1
A=A-1
M=D
// call Class1.set 2
@Class1.set
D=A
@R15
M=D
@7
D=A
@R14
M=D
@Sys.init$ret.0
D=A
@__START_CALL
0;JMP
(Sys.init$ret.0)
// pop temp 0
@5
D=A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push constant 23
@23
D=A
@SP
AM=M+1
A=A-1
M=D
// push constant 15
@15
D=A
@SP
AM=M+1
A=A-1
M=D
// call Class2.set 2
@Class2.set
D=A
@R15
M=D
@7
D=A
@R14
M=D
@Sys.init$ret.1
D=A
@__START_CALL
0;JMP
(Sys.init$ret.1)
// pop temp 0
@5
D=A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// call Class1.get 0
@Class1.get
D=A
@R15
M=D
@5
D=A
@R14
M=D
@Sys.init$ret.2
D=A
@__START_CALL
0;JMP
(Sys.init$ret.2)
// call Class2.get 0
@Class2.get
D=A
@R15
M=D
@5
D=A
@R14
M=D
@Sys.init$ret.3
D=A
@__START_CALL
0;JMP
(Sys.init$ret.3)
// label END
(Sys.init$END)
// goto END
@Sys.init$END
0;JMP
// function Class1.set 0
(Class1.set)
// push argument 0
@ARG
A=M
D=M
@SP
AM=M+1
A=A-1
M=D
// pop static 0
@Class1.0
D=A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
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
// pop static 1
@Class1.1
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
// return
@__START_RETURN
0;JMP
// function Class1.get 0
(Class1.get)
// push static 0
@Class1.0
D=M
@SP
AM=M+1
A=A-1
M=D
// push static 1
@Class1.1
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
// function Class2.set 0
(Class2.set)
// push argument 0
@ARG
A=M
D=M
@SP
AM=M+1
A=A-1
M=D
// pop static 0
@Class2.0
D=A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
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
// pop static 1
@Class2.1
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
// return
@__START_RETURN
0;JMP
// function Class2.get 0
(Class2.get)
// push static 0
@Class2.0
D=M
@SP
AM=M+1
A=A-1
M=D
// push static 1
@Class2.1
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