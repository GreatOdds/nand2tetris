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
// push constant 4000
@4000
D=A
@SP
AM=M+1
A=A-1
M=D
// pop pointer 0
@THIS
D=A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push constant 5000
@5000
D=A
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
// call Sys.main 0
@Sys.main
D=A
@R15
M=D
@5
D=A
@R14
M=D
@Sys.init$ret.0
D=A
@__START_CALL
0;JMP
(Sys.init$ret.0)
// pop temp 1
@6
D=A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// label LOOP
(Sys.init$LOOP)
// goto LOOP
@Sys.init$LOOP
0;JMP
// function Sys.main 5
(Sys.main)
@SP
A=M
M=0
A=A+1
M=0
A=A+1
M=0
A=A+1
M=0
A=A+1
M=0
A=A+1
D=A
@SP
M=D
// push constant 4001
@4001
D=A
@SP
AM=M+1
A=A-1
M=D
// pop pointer 0
@THIS
D=A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push constant 5001
@5001
D=A
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
// push constant 200
@200
D=A
@SP
AM=M+1
A=A-1
M=D
// pop local 1
@LCL
D=M
@1
D=D+A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push constant 40
@40
D=A
@SP
AM=M+1
A=A-1
M=D
// pop local 2
@LCL
D=M
@2
D=D+A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push constant 6
@6
D=A
@SP
AM=M+1
A=A-1
M=D
// pop local 3
@LCL
D=M
@3
D=D+A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push constant 123
@123
D=A
@SP
AM=M+1
A=A-1
M=D
// call Sys.add12 1
@Sys.add12
D=A
@R15
M=D
@6
D=A
@R14
M=D
@Sys.main$ret.0
D=A
@__START_CALL
0;JMP
(Sys.main$ret.0)
// pop temp 0
@5
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
// push local 2
@LCL
D=M
@2
A=D+A
D=M
@SP
AM=M+1
A=A-1
M=D
// push local 3
@LCL
D=M
@3
A=D+A
D=M
@SP
AM=M+1
A=A-1
M=D
// push local 4
@LCL
D=M
@4
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
// add
@SP
AM=M-1
D=M
A=A-1
M=D+M
// add
@SP
AM=M-1
D=M
A=A-1
M=D+M
// add
@SP
AM=M-1
D=M
A=A-1
M=D+M
// return
@__START_RETURN
0;JMP
// function Sys.add12 0
(Sys.add12)
// push constant 4002
@4002
D=A
@SP
AM=M+1
A=A-1
M=D
// pop pointer 0
@THIS
D=A
@SP
AM=M-1
D=D+M
A=D-M
M=D-A
// push constant 5002
@5002
D=A
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
// push argument 0
@ARG
A=M
D=M
@SP
AM=M+1
A=A-1
M=D
// push constant 12
@12
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
// return
@__START_RETURN
0;JMP
