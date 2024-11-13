// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/4/Fill.asm

// Runs an infinite loop that listens to the keyboard input. 
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel. When no key is pressed, 
// the screen should be cleared.

//// Replace this comment with your code.
  @SCREEN
  D=A
  @i
  M=D
(LOOP)
  @KBD
  D=A
  @i
  M=M+1
  D=D-M
  @CONT
  D;JGT
  @SCREEN
  D=A
  @i
  M=D
(CONT)
  @KBD
  D=M
  @PAINT
  D;JEQ
  D=-1
(PAINT)
  @i
  A=M
  M=D
  @LOOP
  0;JMP
  