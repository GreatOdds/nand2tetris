namespace Hack
{
    public class CodeWriter
    {
        private StreamWriter sw;
        private Logger? logger;

        string fileName = string.Empty;
        bool inFunction = false;
        string callerName = "";
        int callCount = 0;

        int ltCount = 0;
        int eqCount = 0;
        int gtCount = 0;

        public CodeWriter(FileInfo inputFile, bool createBootstrap, Logger? logger)
        {
            sw = new StreamWriter(inputFile.FullName, false);
            this.logger = logger;

            // sw.Write("@256\nD=A\n@SP\nM=D\n");

            sw.Write("@__END_TOOL\n0;JMP\n");
            ComparisonBootstrap("LT", "JGE");
            ComparisonBootstrap("EQ", "JNE");
            ComparisonBootstrap("GT", "JLE");
            sw.WriteLine("(__START_CALL)");
            // Push return address to stack
            WritePush();
            // Save caller frame
            WritePushPopDirect(Parser.CommandType.C_PUSH, "LCL");
            WritePushPopDirect(Parser.CommandType.C_PUSH, "ARG");
            WritePushPopDirect(Parser.CommandType.C_PUSH, "THIS");
            WritePushPopDirect(Parser.CommandType.C_PUSH, "THAT");
            // Reposition LCL
            // Reposition ARG (SP - (5 + nArgs)) in R14
            // Goto functionName (R15)
            sw.WriteLine($"""
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
                """);
            // R15 = endFrame, R14 = retAddr
            sw.WriteLine("""
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
                """);
            sw.Write("(__END_TOOL)\n");

            if (createBootstrap)
            {
                sw.WriteLine("""
                    @256
                    D=A
                    @SP
                    M=D
                    """);
                WriteCall("Sys.init", 0);
            }
        }

        public void SetFileName(FileInfo fileInfo)
        {
            fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            callerName = fileName;
            inFunction = false;
        }

        public void WriteArithmetic(string command)
        {
            sw.WriteLine($"// {command}");
            switch (command)
            {
                case "add":
                    WriteBinaryComp("D+M");
                    break;
                case "sub":
                    WriteBinaryComp("M-D");
                    break;
                case "and":
                    WriteBinaryComp("D&M");
                    break;
                case "or":
                    WriteBinaryComp("D|M");
                    break;
                case "neg":
                    WriteUnaryComp("-M");
                    break;
                case "not":
                    WriteUnaryComp("!M");
                    break;
                case "lt":
                    WriteComparison("LT", ltCount);
                    ltCount++;
                    break;
                case "eq":
                    WriteComparison("EQ", eqCount);
                    eqCount++;
                    break;
                case "gt":
                    WriteComparison("GT", gtCount);
                    gtCount++;
                    break;
            }
        }

        public void WritePushPop(Parser.CommandType command, string segment, int index)
        {
            if (index < 0 || !(
                command == Parser.CommandType.C_PUSH ||
                command == Parser.CommandType.C_POP)) return;

            sw.WriteLine($"// {(command == Parser.CommandType.C_PUSH ? "push" : "pop")} {segment} {index}");

            switch (segment)
            {
                case "constant":
                    if (command != Parser.CommandType.C_PUSH) return;
                    sw.WriteLine($"""
                        @{index}
                        D=A
                        """);
                    WritePush();
                    break;
                case "static":
                    WritePushPopDirect(command, $"{fileName}.{index}");
                    break;
                case "temp":
                    if (index > 7) return;
                    WritePushPopDirect(command, $"{5 + index}");
                    break;
                case "pointer":
                    if (index > 1) return;
                    WritePushPopDirect(command, index == 0 ? "THIS" : "THAT");
                    break;
                case "local":
                    WritePushPopAddress(command, "LCL", index);
                    break;
                case "argument":
                    WritePushPopAddress(command, "ARG", index);
                    break;
                case "this":
                    WritePushPopAddress(command, "THIS", index);
                    break;
                case "that":
                    WritePushPopAddress(command, "THAT", index);
                    break;
            }
        }

        public void WriteLabel(string label)
        {
            sw.WriteLine($"// label {label}");

            if (inFunction)
                sw.Write($"({callerName}${label})\n");
            else
                sw.Write($"({label})\n");
        }

        public void WriteGoto(string label)
        {
            sw.WriteLine($"// goto {label}");
            if (inFunction)
                sw.WriteLine($"""
                    @{callerName}${label}
                    0;JMP
                    """);
            else
                sw.WriteLine($"""
                    @{label}
                    0;JMP
                    """);
        }

        public void WriteIf(string label)
        {
            sw.WriteLine($"// if-goto {label}");


            if (inFunction)
                sw.WriteLine($"""
                    @SP
                    AM=M-1
                    D=M
                    @{callerName}${label}
                    D;JNE
                    """);
            else
                sw.WriteLine($"""
                    @SP
                    AM=M-1
                    D=M
                    @{label}
                    D;JNE
                    """);
        }

        public void WriteFunction(string functionName, int nVars)
        {
            sw.WriteLine($"// function {functionName} {nVars}");

            inFunction = true;
            callerName = functionName;
            callCount = 0;
            sw.WriteLine($"({callerName})");
            if (nVars > 0)
            {
                sw.WriteLine("""
                    @SP
                    A=M
                    """);
                for (int i = 0; i < nVars; i++)
                {
                    sw.WriteLine("""
                        M=0
                        A=A+1
                        """);
                }
                sw.WriteLine("""
                    D=A
                    @SP
                    M=D
                    """);
            }
        }

        public void WriteCall(string functionName, int nArgs)
        {
            sw.WriteLine($"// call {functionName} {nArgs}");

            var returnLabel = $"{callerName}$ret.{callCount}";

            // Push return address to stack
            // Save caller frame
            // Reposition LCL
            // Reposition ARG
            // Goto functionName
            sw.WriteLine($"""
                @{functionName}
                D=A
                @R15
                M=D
                @{5 + nArgs}
                D=A
                @R14
                M=D
                @{returnLabel}
                D=A
                @__START_CALL
                0;JMP
                ({returnLabel})
                """);

            callCount++;
        }

        public void WriteReturn()
        {
            sw.WriteLine("// return");

            sw.WriteLine("""
                @__START_RETURN
                0;JMP
                """);
        }

        public void Close()
        {
            sw.Close();
        }

        // How comparisons work:
        // - Store return address in D
        // - Copy return address to R15
        // - Pop right value to D
        // - Subtract right value from left
        // - Store false (0) on stack
        // - D = left - right
        //   - lt: D >= 0 : Jump End
        //   - eq: D != 0 : Jump End
        //   - gt: D <= 0 : Jump End
        // - store true (-1) on stack
        // - END
        // - Jump to return address from R15
        void ComparisonBootstrap(string compLabel, string compJump)
        {
            sw.WriteLine($"""
                (__START_{compLabel})
                @R15
                M=D
                @SP
                AM=M-1
                D=M
                A=A-1
                D=M-D
                M=0
                @__END_{compLabel}
                D;{compJump}
                @SP
                A=M-1
                M=-1
                (__END_{compLabel})
                @R15
                A=M
                0;JMP
                """);
        }

        void WriteComparison(string compLabel, int compCount)
        {
            sw.WriteLine($"""
                @__RET_{compLabel}{compCount}
                D=A
                @__START_{compLabel}
                0;JMP
                (__RET_{compLabel}{compCount})
                """);
        }

        // Pops right value to D
        // Uses M as left value
        // Stores result on top of stack
        void WriteBinaryComp(string comp)
        {
            sw.WriteLine($"""
                @SP
                AM=M-1
                D=M
                A=A-1
                M={comp}
                """);
        }

        // Uses M as value
        // Stores result on top of stack
        void WriteUnaryComp(string comp)
        {
            sw.WriteLine($"""
                @SP
                A=M-1
                M={comp}
                """);
        }

        // Pushes value in D to stack
        void WritePush()
        {
            sw.WriteLine("""
                @SP
                AM=M+1
                A=A-1
                M=D
                """);
        }

        // Pops top of stack to RAM[D]
        void WritePop()
        {
            sw.WriteLine("""
                @SP
                AM=M-1
                D=D+M
                A=D-M
                M=D-A
                """);
        }

        void WritePushPopDirect(Parser.CommandType command, string address)
        {
            if (command == Parser.CommandType.C_PUSH)
            {
                sw.WriteLine($"""
                    @{address}
                    D=M
                    """);
                WritePush();
            }
            else
            {
                sw.WriteLine($"""
                    @{address}
                    D=A
                    """);
                WritePop();
            }
        }

        void WritePushPopAddress(Parser.CommandType command, string segment, int index)
        {
            if (command == Parser.CommandType.C_PUSH)
            {
                if (index > 0)
                    sw.WriteLine($"""
                        @{segment}
                        D=M
                        @{index}
                        A=D+A
                        D=M
                        """);
                else
                    sw.WriteLine($"""
                        @{segment}
                        A=M
                        D=M
                        """);
                WritePush();
            }
            else
            {
                sw.WriteLine($"""
                    @{segment}
                    D=M
                    """);
                if (index > 0)
                    sw.WriteLine($"""
                        @{index}
                        D=D+A
                        """);
                WritePop();
            }
        }
    }
}
