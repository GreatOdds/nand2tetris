using System;
using System.Text.RegularExpressions;

namespace Hack
{
    public class Parser
    {
        public enum CommandType
        {
            C_ARITHMETIC, // add, sub, neg, eq, gt, lt, and, or, not
            C_PUSH, C_POP, // push SEGMENT INDEX, pop SEGMENT INDEX
            C_LABEL, C_GOTO, C_IF, // label LABEL, goto LABEL, if-goto LABEL
            C_FUNCTION, C_RETURN, C_CALL, // function NAME nVars, return, call NAME nARGS
            C_INVALID
        }

        struct Command
        {
            public CommandType type = CommandType.C_INVALID;
            public string arg1 = string.Empty;
            public int arg2 = -1;

            public Command() { }
        }

        private static readonly Regex commandMatch = new(
            @"^\s*(?<command>[a-z-]+)(?:[^\S\n]*(?<arg1>[a-zA-Z_.$:][a-zA-Z_.$:0-9]*)(?:[^\S\n]*(?<arg2>\d+))?)?[^\S\n]*(?:\/\/.*)?$");
        private List<Command> commands = new List<Command>();
        private int currentLine = -1;

        public Parser(FileInfo inputFile, Logger? logger)
        {
            using (var sr = inputFile.OpenText())
            {
                var line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    var match = commandMatch.Match(line);
                    if (!match.Success) continue;

                    var groups = match.Groups;
                    Group? group;
                    if (groups.TryGetValue("command", out group) && group.Value != string.Empty)
                    {
                        var command = new Command();
                        var commandStr = group.Value;
                        command.type = StringToCommandType(commandStr);

                        switch (command.type)
                        {
                            case CommandType.C_ARITHMETIC:
                            case CommandType.C_RETURN:
                                command.arg1 = group.Value; // Store command in arg1
                                break;
                            case CommandType.C_LABEL:
                            case CommandType.C_GOTO:
                            case CommandType.C_IF:
                                if (groups.TryGetValue("arg1", out group) && group.Value != string.Empty)
                                    command.arg1 = group.Value;
                                else
                                    command.type = CommandType.C_INVALID;
                                break;
                            case CommandType.C_PUSH:
                            case CommandType.C_POP:
                            case CommandType.C_FUNCTION:
                            case CommandType.C_CALL:
                                if (groups.TryGetValue("arg1", out group) && group.Value != string.Empty)
                                    command.arg1 = group.Value;
                                else
                                    command.type = CommandType.C_INVALID;
                                if (command.type != CommandType.C_INVALID && groups.TryGetValue("arg2", out group) && group.Value != string.Empty)
                                    if (!int.TryParse(group.Value, out command.arg2))
                                        command.type = CommandType.C_INVALID;
                                break;
                        }
                        if (command.type == CommandType.C_INVALID) continue;

                        commands.Add(command);
                    }
                }
            }

            foreach (var command in commands)
            {
                LogCommand(command, logger);
            }
        }

        public bool HasMoreLines()
        {
            return currentLine < (commands.Count - 1);
        }

        public void Advance()
        {
            currentLine += 1;
        }

        public CommandType GetCommandType()
        {
            return commands[currentLine].type;
        }

        public string GetArg1()
        {
            return commands[currentLine].arg1;
        }

        public int GetArg2()
        {
            return commands[currentLine].arg2;
        }

        CommandType StringToCommandType(string command)
        {
            switch (command)
            {
                case "add":
                case "sub":
                case "neg":
                case "eq":
                case "gt":
                case "lt":
                case "and":
                case "or":
                case "not":
                    return CommandType.C_ARITHMETIC;
                case "push":
                    return CommandType.C_PUSH;
                case "pop":
                    return CommandType.C_POP;
                case "label":
                    return CommandType.C_LABEL;
                case "goto":
                    return CommandType.C_GOTO;
                case "if-goto":
                    return CommandType.C_IF;
                case "function":
                    return CommandType.C_FUNCTION;
                case "return":
                    return CommandType.C_RETURN;
                case "call":
                    return CommandType.C_CALL;
                default:
                    return CommandType.C_INVALID;
            }
        }

        void LogCommand(Command command, Logger? logger)
        {
            switch (command.type)
            {
                case CommandType.C_ARITHMETIC:
                case CommandType.C_RETURN:
                    logger?.LogLine(command.arg1);
                    break;
                case CommandType.C_LABEL:
                    logger?.LogLine($"label {command.arg1}");
                    break;
                case CommandType.C_GOTO:
                    logger?.LogLine($"goto {command.arg1}");
                    break;
                case CommandType.C_IF:
                    logger?.LogLine($"if-goto {command.arg1}");
                    break;
                case CommandType.C_PUSH:
                    logger?.LogLine($"push {command.arg1} {command.arg2}");
                    break;
                case CommandType.C_POP:
                    logger?.LogLine($"pop {command.arg1} {command.arg2}");
                    break;
                case CommandType.C_FUNCTION:
                    logger?.LogLine($"function {command.arg1} {command.arg2}");
                    break;
                case CommandType.C_CALL:
                    logger?.LogLine($"call {command.arg1} {command.arg2}");
                    break;
                case CommandType.C_INVALID:
                default:
                    break;
            }
        }
    }
}
