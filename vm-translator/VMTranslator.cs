using System.CommandLine;

namespace Hack
{
    public class VMTranslator
    {
        static async Task<int> Main(string[] args)
        {
            var inputArgument = new Argument<FileSystemInfo?>(
                name: "file|directory",
                description: "File or directory to translate.");

            var outputOption = new Option<FileSystemInfo?>(
                aliases: ["--output", "-o"],
                description: "Path to output.");

            var verboseOption = new Option<bool>(
                aliases: ["--silent", "-s"],
                description: "Turn off translation status messages.");

            var rootCommand = new RootCommand("Translates VM language programs to Hack machine language.");
            rootCommand.Add(inputArgument);
            rootCommand.Add(outputOption);
            rootCommand.Add(verboseOption);
            rootCommand.SetHandler(Translate, inputArgument, outputOption, verboseOption);

            return await rootCommand.InvokeAsync(args);
        }

        static void Translate(FileSystemInfo? inputPath, FileSystemInfo? outputPath, bool silent = false)
        {
            var logger = new Logger(silent);

            if (inputPath == null || !(
                File.Exists(inputPath.FullName) ||
                Directory.Exists(inputPath.FullName)))
            {
                logger.LogLine("File or directory does not exist.");
                return;
            }

            var isFile = true;
            if (inputPath.Attributes.HasFlag(FileAttributes.Directory))
            {
                isFile = false;
            }
            logger.LogLine($"Input from {inputPath.FullName}");

            var outputFileName = (isFile ? Path.GetFileNameWithoutExtension(inputPath.Name) : inputPath.Name) + ".asm";
            var outputDir = isFile ? Path.GetDirectoryName(inputPath.FullName)! : inputPath.FullName;
            if (outputPath != null)
            {
                if (outputPath.Attributes.HasFlag(FileAttributes.Directory))
                {
                    logger.LogLine("Store in directory.");
                    outputDir = outputPath.FullName;
                }
                else
                {
                    logger.LogLine("Store in file.");
                    outputFileName = outputPath.Name;
                    outputDir = Path.GetDirectoryName(outputPath.FullName)!;
                }
            }

            var codeWriter = new CodeWriter(
                new FileInfo(Path.Combine(outputDir, outputFileName)),
                !isFile,
                logger);

            if (isFile)
            {
                TranslateFile(new FileInfo(inputPath.FullName), codeWriter, logger);
            }
            else
            {
                var dirInfo = new DirectoryInfo(inputPath.FullName);
                foreach (var fileInfo in dirInfo.GetFiles("*.vm"))
                {
                    TranslateFile(fileInfo, codeWriter, logger);
                }
            }

            codeWriter.Close();
            logger.LogLine($"Output to {Path.Combine(outputDir, outputFileName)}");
        }

        static void TranslateFile(FileInfo fileInfo, CodeWriter codeWriter, Logger? logger)
        {
            codeWriter.SetFileName(fileInfo);
            logger?.LogLine($"Translating {fileInfo.FullName}\n");
            var parser = new Parser(fileInfo, logger);
            while (parser.HasMoreLines())
            {
                parser.Advance();
                var commandType = parser.GetCommandType();
                switch (commandType)
                {
                    case Parser.CommandType.C_ARITHMETIC:
                        codeWriter.WriteArithmetic(parser.GetArg1());
                        break;
                    case Parser.CommandType.C_PUSH:
                    case Parser.CommandType.C_POP:
                        codeWriter.WritePushPop(commandType, parser.GetArg1(), parser.GetArg2());
                        break;
                    case Parser.CommandType.C_LABEL:
                        codeWriter.WriteLabel(parser.GetArg1());
                        break;
                    case Parser.CommandType.C_GOTO:
                        codeWriter.WriteGoto(parser.GetArg1());
                        break;
                    case Parser.CommandType.C_IF:
                        codeWriter.WriteIf(parser.GetArg1());
                        break;
                    case Parser.CommandType.C_FUNCTION:
                        codeWriter.WriteFunction(parser.GetArg1(), parser.GetArg2());
                        break;
                    case Parser.CommandType.C_RETURN:
                        codeWriter.WriteReturn();
                        break;
                    case Parser.CommandType.C_CALL:
                        codeWriter.WriteCall(parser.GetArg1(), parser.GetArg2());
                        break;
                }
            }
        }
    }
}
