using System;
using System.Text.RegularExpressions;

namespace Hack
{
    public class Parser
    {
        public enum CommandType
        {
            C_ARITHMETIC,
            C_PUSH,
            C_POP,
            C_LABEL,
            C_GOTO,
            C_IF,
            C_FUNCTION,
            C_RETURN,
            C_CALL,
            INVALID
        }

        /***
        ^\s*
        (?<command>
            [a-z]+
        )
        (?:
            \s*
            (?<segment>
                [a-z]+
            )
            \s*
            (?<index>
                \d+
            )
        )?
        \s*(?:\/\/.*)?$
         ***/
        private static readonly Regex command =
            new(@"^\s*(?<command>[a-z]+)(?:\s*(?<arg1>[a-z]+)\s*(?<arg2>\d+))?\s*(?:\/\/.*)?$");

        private List<string> lines = [];
        private int currentLine = -1;
        private CommandType commandType = CommandType.INVALID;
        private string arg1 = "";
        private int arg2 = -1;

        public bool hasEQ = false;
        public bool hasGT = false;
        public bool hasLT = false;

        public Parser(string inputPath)
        {
            using (var sr = File.OpenText(inputPath))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    var match = command.Match(line);
                    if (!match.Success)
                        continue;
                    var groups = match.Groups;
                    var cmd = groups["command"].Value;
                    if (!hasEQ && cmd == "eq")
                        hasEQ = true;
                    if (!hasGT && cmd == "gt")
                        hasGT = true;
                    if (!hasLT && cmd == "lt")
                        hasLT = true;
                    line = (
                        cmd
                        + (groups["arg1"].Value != string.Empty ? (" " + groups["arg1"].Value) : "")
                        + (groups["arg2"].Value != string.Empty ? (" " + groups["arg2"].Value) : "")
                    );
                    lines.Add(line);
                }
            }
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
            commandType = CommandType.INVALID;
            arg1 = "";
            arg2 = -1;
            var groups = command.Match(lines[currentLine]).Groups;

            var cmd = groups["command"].Value;
            switch (cmd)
            {
                case "push":
                    commandType = CommandType.C_PUSH;
                    arg1 = groups["arg1"].Value;
                    if (!int.TryParse(groups["arg2"].Value, out arg2))
                    {
                        arg2 = -1;
                    }
                    break;
                case "pop":
                    commandType = CommandType.C_POP;
                    arg1 = groups["arg1"].Value;
                    if (!int.TryParse(groups["arg2"].Value, out arg2))
                    {
                        arg2 = -1;
                    }
                    break;
                case "add":
                case "sub":
                case "neg":
                case "not":
                case "and":
                case "or":
                case "eq":
                case "gt":
                case "lt":
                    commandType = CommandType.C_ARITHMETIC;
                    arg1 = groups["command"].Value;
                    break;
            }
        }

        public CommandType GetCommandType()
        {
            return commandType;
        }

        public string Arg1()
        {
            return arg1;
        }

        public int Arg2()
        {
            return arg2;
        }
    }
}
