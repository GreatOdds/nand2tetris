namespace Hack
{
    public class CodeWriter
    {
        private StreamWriter sw;

        private string className = "";
        private int eqCount = 0;
        private int gtCount = 0;
        private int ltCount = 0;

        public CodeWriter(
            string outputPath,
            bool hasEQ = false,
            bool hasGT = false,
            bool hasLT = false
        )
        {
            sw = new(outputPath);
            className = Path.GetFileNameWithoutExtension(outputPath);
            if (hasEQ || hasGT || hasLT)
            {
                sw.WriteLine("@__END_LOGIC\n0;JMP");
                if (hasEQ)
                {
                    sw.WriteLine(
                        """
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
                        """
                    );
                }
                if (hasGT)
                {
                    sw.WriteLine(
                        """
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
                        """
                    );
                }
                if (hasLT)
                {
                    sw.WriteLine(
                        """
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
                        """
                    );
                }
                sw.WriteLine("(__END_LOGIC)");
            }
        }

        // add, sub, and, or
        // ---
        // @SP
        // AM=M-1
        // D=M
        // @SP
        // A=M-1
        // M=(D+M) or (M-D) or (D&M) or (D|M) // Change this

        // neg, not
        // ---
        // @SP
        // A=M-1
        // M=(-M) or !M

        // eq, gt, lt // true: -1, false: 0
        // ---
        // // Logic functions
        // // Store return address
        // @__END_LOGIC
        // 0;JMP
        // (__START_COMP)
        // @R15
        // M=D
        // @SP
        // AM=M-1
        // D=M
        // A=A-1
        // D=M-D // 0 if M=D, >0 if M>D, <0 if M<D
        // M=0
        // @__END_COMP
        // D;(JNE) or (JLE) or (JGE)
        // @SP
        // A=M-1
        // M=-1
        // (__END_COMP)
        // // Return to calling address
        // @R15
        // A=M
        // 0;JMP
        // (__END_LOGIC)
        //
        // Usage
        // ---
        // @__RET_COMP_N
        // D=A
        // @__START_COMP
        // 0;JMP
        // (__RET_COMP_N)
        public void WriteArithmetic(string command)
        {
            switch (command)
            {
                case "add":
                    sw.WriteLine("// add");
                    sw.WriteLine(
                        """
                        @SP
                        AM=M-1
                        D=M
                        @SP
                        A=M-1
                        M=D+M
                        """
                    );
                    break;
                case "sub":
                    sw.WriteLine("// sub");
                    sw.WriteLine(
                        """
                        @SP
                        AM=M-1
                        D=M
                        @SP
                        A=M-1
                        M=M-D
                        """
                    );
                    break;
                case "and":
                    sw.WriteLine("// and");
                    sw.WriteLine(
                        """
                        @SP
                        AM=M-1
                        D=M
                        @SP
                        A=M-1
                        M=D&M
                        """
                    );
                    break;
                case "or":
                    sw.WriteLine("// or");
                    sw.WriteLine(
                        """
                        @SP
                        AM=M-1
                        D=M
                        @SP
                        A=M-1
                        M=D|M
                        """
                    );
                    break;
                case "neg":
                    sw.WriteLine("// neg");
                    sw.WriteLine(
                        """
                        @SP
                        A=M-1
                        M=-M
                        """
                    );
                    break;
                case "not":
                    sw.WriteLine("// not");
                    sw.WriteLine(
                        """
                        @SP
                        A=M-1
                        M=!M
                        """
                    );
                    break;
                case "eq":
                    sw.WriteLine("// eq");
                    sw.WriteLine(
                        $"""
                        @__RET_EQ_{eqCount}
                        D=A
                        @__START_EQ
                        0;JMP
                        (__RET_EQ_{eqCount})
                        """
                    );
                    eqCount++;
                    break;
                case "gt":
                    sw.WriteLine("// gt");
                    sw.WriteLine(
                        $"""
                        @__RET_GT_{gtCount}
                        D=A
                        @__START_GT
                        0;JMP
                        (__RET_GT_{gtCount})
                        """
                    );
                    gtCount++;
                    break;
                case "lt":
                    sw.WriteLine("// lt");
                    sw.WriteLine(
                        $"""
                        @__RET_LT_{ltCount}
                        D=A
                        @__START_LT
                        0;JMP
                        (__RET_LT_{ltCount})
                        """
                    );
                    ltCount++;
                    break;
            }
        }

        // push constant i
        // ---
        // @i
        // D=A
        //
        // @SP
        // AM=M+1
        // A=A-1
        // M=D

        // push segment i
        // ---
        // // D = RAM[segmentPtr + i]
        // @SEGMENT
        // D=M
        // @i
        // A=D+A
        // D=M
        // // RAM[SP] = D
        // @SP
        // AM=M+1
        // A=A-1
        // M=D

        // pop segment i
        // ---
        // // D = segmentPtr + i
        // @SEGMENT
        // D=M
        // @i // Can remove if i = 0
        // D=D+A // Can be one-lined if i = 1 or -1
        // // D = addr + val
        // @SP
        // AM=M-1
        // D=D+M
        // // A = (addr + val) - val
        // A=D-M
        // // M = (addr + val) - addr
        // M=D-A

        // Segments
        // LCL: local
        // ARG: argument
        // THIS: this
        // THAT: that
        public void WritePushPop(bool isPop, string segment, int index)
        {
            if (index < 0)
                return;
            sw.WriteLine(
                $"""
                // {(isPop ? "pop" : "push")} {segment} {index}
                """
            );

            switch (segment)
            {
                case "constant":
                    sw.WriteLine(
                        $"""
                        @{index}
                        D=A
                        """
                    );
                    WritePush();
                    return;
                case "static":
                    string staticName = $"{className}.{index}";
                    WritePushPopDirect(isPop, staticName);
                    return;
                case "temp":
                    if (index < 0 || index > 7)
                        return;
                    int tmpIdx = 5 + index;
                    WritePushPopDirect(isPop, $"{tmpIdx}");
                    return;
                case "pointer":
                    if (index < 0 || index > 1)
                        return;
                    string tmp = index == 0 ? "THIS" : "THAT";
                    WritePushPopDirect(isPop, tmp);
                    return;
            }

            string SEG = "SP";
            switch (segment)
            {
                case "local":
                    SEG = "LCL";
                    break;
                case "argument":
                    SEG = "ARG";
                    break;
                case "this":
                    SEG = "THIS";
                    break;
                case "that":
                    SEG = "THAT";
                    break;
                default:
                    return;
            }
            if (isPop)
            {
                WriteGetAddress(SEG, index);
                WritePop();
            }
            else
            {
                WriteGetValue(SEG, index);
                WritePush();
            }
        }

        void WritePushPopDirect(bool isPop, string segment)
        {
            if (isPop)
            {
                sw.WriteLine(
                    $"""
                    @{segment}
                    D=A
                    """
                );
                WritePop();
            }
            else
            {
                sw.WriteLine(
                    $"""
                    @{segment}
                    D=M
                    """
                );
                WritePush();
            }
        }

        void WriteGetValue(string segment, int index)
        {
            // D = RAM[RAM[segment] + index]
            if (index == 0)
            {
                sw.WriteLine(
                    $"""
                    @{segment}
                    A=M
                    D=M
                    """
                );
            }
            else
            {
                sw.WriteLine(
                    $"""
                    @{segment}
                    D=M
                    @{index}
                    A=D+A
                    D=M
                    """
                );
            }
        }

        void WriteGetAddress(string segment, int index)
        {
            // D = RAM[segment] + index
            sw.WriteLine(
                $"""
                @{segment}
                D=M
                """
            );
            if (index > 0)
            {
                sw.WriteLine(
                    $"""
                    @{index}
                    D=D+A
                    """
                );
            }
        }

        void WritePush()
        {
            // D contains value to be pushed
            sw.WriteLine(
                """
                @SP
                AM=M+1
                A=A-1
                M=D
                """
            );
        }

        void WritePop()
        {
            // D contains destination address
            sw.WriteLine(
                """
                @SP
                AM=M-1
                D=D+M
                A=D-M
                M=D-A
                """
            );
        }

        public void Close()
        {
            try
            {
                sw.Close();
                sw.Dispose();
            }
            catch (Exception) { }
        }
    }
}
