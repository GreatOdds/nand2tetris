using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Hack
{
    public partial class Parser
    {
        public enum InstructionType
        {
            A, // @xxx, where xxx is either a decimal number or a symbol
            C, // dest = comp ; jump
            L, // (label)
            INVALID
        }

        const string LInstruction = "L";
        const string AInstruction = "A";
        const string CInstruction = "C";
        const string SYMBOL = "symbol";
        const string CONSTANT = "constant";
        const string DEST = "dest";
        const string COMP = "comp";
        const string JUMP = "jump";

        /***
        ^\s* # Start of line and any whitespace
        (?<instruction>
            (?<L> # L instruction
                \(
                    (?<symbol>[a-zA-Z_.$:][0 - 9a - zA - Z_.$:]*)
                \)
            )|
            (?<A> # A instruction
                @
                (?:
                    (?<constant>\d+)|
                    (?<symbol>[a-zA-Z_.$:][0 - 9a - zA - Z_.$:]*)
                )
            )|
            (?<C> # C instruction
                (?:
                    (?<dest>A? M?D?)=
                )?
                (?<comp>
                    [AMD][-+&|][AMD]|
                    [AMD][-+]1|
                    [-!]?[AMD]|
                    -1|
                    1|
                    0
                )
                (?:
                    ;
                    (?<jump>J(?:G[TE]|L[TE]|EQ|NE|MP))
                )?
            )
        )
        \s*(?:\/\/.*)?$ # Whitespace, Comments and End of line
        ***/
        private static readonly Regex instruction = new(@"^\s*(?<instruction>(?<L>\((?<symbol>[a-zA-Z_.$:][0-9a-zA-Z_.$:]*)\))|(?<A>@(?:(?<constant>\d+)|(?<symbol>[a-zA-Z_.$:][0-9a-zA-Z_.$:]*)))|(?<C>(?:(?<dest>A?M?D?)=)?(?<comp>[AMD][-+&|][AMD]|[AMD][-+]1|[-!]?[AMD]|-1|1|0)(?:;(?<jump>J(?:G[TE]|L[TE]|EQ|NE|MP)))?))\s*(?:\/\/.*)?$");
        private static readonly Regex getWhiteSpace = new(@"\s+");
        private static readonly Regex getComments = new(@"\/\/.*");

        private List<string> lines = [];
        private int currentLine = -1;
        private InstructionType instructionType = InstructionType.INVALID;
        private bool hasConstant;
        private bool hasSymbol;
        private string? constant;
        private string? symbol;
        private string? dest;
        private string? comp;
        private string? jump;

        public Parser(string filePath)
        {
            using (var sr = File.OpenText(filePath))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    line = getComments.Replace(getWhiteSpace.Replace(line, ""), "");
                    var match = instruction.Match(line);
                    if (string.IsNullOrEmpty(line) || !match.Success) continue;
                    lines.Add(line);
                }
            }

            Console.WriteLine($"Total lines: {lines.Count}");
        }

        public bool HasMoreLines()
        {
            return (currentLine + 1) < lines.Count;
        }

        public void Reset()
        {
            currentLine = -1;
        }

        public void Advance()
        {
            currentLine++;
            hasConstant = false;
            hasSymbol = false;
            constant = null;
            symbol = null;
            dest = null;
            comp = null;
            jump = null;
            var groups = instruction.Match(lines[currentLine]).Groups;

            // foreach (Group group in groups)
            // {
            //     Console.WriteLine("Group {0}: {1}", group.Name, group.Value);
            //     int i = 0;
            //     foreach (Capture capture in group.Captures)
            //     {
            //         Console.WriteLine("\tCapture {0}: {1}", i, capture.Value);
            //         i++;
            //     }
            // }

            if (groups[LInstruction].Value != string.Empty)
            {
                instructionType = InstructionType.L;
                if (groups[SYMBOL].Value != string.Empty)
                {
                    symbol = groups[SYMBOL].Value;
                    hasSymbol = true;
                }
                // Console.WriteLine("L");
            }
            else if (groups[AInstruction].Value != String.Empty)
            {
                instructionType = InstructionType.A;
                if (groups[CONSTANT].Value != String.Empty)
                {
                    constant = groups[CONSTANT].Value;
                    hasConstant = true;
                }
                else if (groups[SYMBOL].Value != String.Empty)
                {
                    symbol = groups[SYMBOL].Value;
                    hasSymbol = true;
                }
                // Console.WriteLine("A");
            }
            else if (groups[CInstruction].Value != String.Empty)
            {
                instructionType = InstructionType.C;
                if (groups[DEST].Value != String.Empty)
                {
                    dest = groups[DEST].Value;
                }
                comp = groups[COMP].Value;
                if (groups[JUMP].Value != String.Empty)
                {
                    jump = groups[JUMP].Value;
                }
                // Console.WriteLine("C");
            }
        }

        public InstructionType GetInstructionType()
        {
            return instructionType;
        }

        public bool HasConstant()
        {
            return hasConstant;
        }

        public bool HasSymbol()
        {
            return hasSymbol;
        }

        public string? Constant()
        {
            return constant;
        }

        public string? Symbol()
        {
            return symbol;
        }

        public string? Dest()
        {
            return dest;
        }

        public string? Comp()
        {
            return comp;
        }

        public string? Jump()
        {
            return jump;
        }
    }
}