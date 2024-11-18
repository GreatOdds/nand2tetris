using System;
using System.IO;

namespace Hack
{
    public class Assembler
    {
        static void Main(string[] args)
        {
            if (args == null) return;
            if (args.Length < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: ");
                Console.ResetColor();
                Console.WriteLine("no input file");
                PrintHelp();
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("could not find input file");
                return;
            }

            string inputPath = args[0];
            string outputPath = Path.GetFileNameWithoutExtension(inputPath) + ".hack";
            if (args.Length >= 2)
            {
                outputPath = args[1];
            }

            if (!IsValidPath(outputPath))
            {
                Console.WriteLine("invalid output file path");
                return;
            }

            var st = new SymbolTable();
            for (int i = 0; i <= 15; i++)
            {
                st.AddEntry($"R{i}", i);
            }
            st.AddEntry("SP", 0);
            st.AddEntry("LCL", 1);
            st.AddEntry("ARG", 2);
            st.AddEntry("THIS", 3);
            st.AddEntry("THAT", 4);

            st.AddEntry("SCREEN", 16384);
            st.AddEntry("KBD", 24576);

            // First pass
            var p = new Parser(inputPath);
            int address = 0;
            while (p.HasMoreLines())
            {
                p.Advance();
                if (p.GetInstructionType() == Parser.InstructionType.L)
                {
                    var symbol = p.Symbol();
                    if (symbol != null && !st.Contains(symbol))
                    {
                        st.AddEntry(symbol, address);
                    }
                }
                else
                {
                    address++;
                }
            }
            p.Reset();

            // Second pass
            address = 16;
            using (var sw = new StreamWriter(outputPath, false))
            {
                while (p.HasMoreLines())
                {
                    p.Advance();
                    var type = p.GetInstructionType();
                    if (type == Parser.InstructionType.A)
                    {
                        if (p.HasConstant())
                        {
                            int c = 0;
                            if (int.TryParse(p.Constant(), out c))
                            {
                                WriteToFile(sw, ConvertToBinary(c));
                            }
                        }
                        else if (p.HasSymbol())
                        {
                            var symbol = p.Symbol();
                            if (symbol != null)
                            {
                                if (st.Contains(symbol))
                                {
                                    WriteToFile(sw, ConvertToBinary(st.GetAddress(symbol)));
                                }
                                else
                                {
                                    st.AddEntry(symbol, address);
                                    WriteToFile(sw, ConvertToBinary(address));
                                    address++;
                                }
                            }
                        }
                    }
                    else if (type == Parser.InstructionType.C)
                    {
                        WriteToFile(sw,
                            "111" +
                            Generator.Comp(p.Comp()) +
                            Generator.Dest(p.Dest()) +
                            Generator.Jump(p.Jump())
                        );
                    }
                }
            }
        }

        static void WriteToFile(StreamWriter sw, string? value)
        {
            Console.WriteLine(value);
            sw.WriteLine(value);
        }

        static void PrintHelp()
        {
            Console.WriteLine($"Usage: {System.AppDomain.CurrentDomain.FriendlyName} input_file [output_file]");
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

        static string ConvertToBinary(int value)
        {
            value &= 0b0111_1111_1111_1111;
            return Convert.ToString(value, 2).PadLeft(16, '0');
        }
    }
}