using System;

namespace Hack
{
    public class VMTranslator
    {
        public static void Main(string[] args)
        {
            if (args == null || args.Length < 1)
                return;

            if (!File.Exists(args[0]))
                return;

            string inputPath = args[0];
            string outputPath = Path.ChangeExtension(inputPath, ".asm");

            if (!IsValidPath(outputPath))
                return;

            var p = new Parser(inputPath);
            var c = new CodeWriter(outputPath, p.hasEQ, p.hasGT, p.hasLT);

            while (p.HasMoreLines())
            {
                p.Advance();
                var cmdType = p.GetCommandType();
                switch (cmdType)
                {
                    case Parser.CommandType.C_ARITHMETIC:
                        Console.WriteLine(p.Arg1());
                        c.WriteArithmetic(p.Arg1());
                        break;
                    case Parser.CommandType.C_PUSH:
                        Console.WriteLine("push " + p.Arg1() + " " + p.Arg2().ToString());
                        c.WritePushPop(false, p.Arg1(), p.Arg2());
                        break;
                    case Parser.CommandType.C_POP:
                        Console.WriteLine("pop " + p.Arg1() + " " + p.Arg2().ToString());
                        c.WritePushPop(true, p.Arg1(), p.Arg2());
                        break;
                }
            }

            c.Close();
        }

        static bool IsValidPath(string path)
        {
            try
            {
                Path.GetFullPath(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
