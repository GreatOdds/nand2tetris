namespace Hack
{
    public class Logger
    {
        bool silent;

        public Logger(bool silent)
        {
            this.silent = silent;
        }

        public void LogLine(string message)
        {
            if (!silent) Console.WriteLine(message);
        }

        public void Log(string message)
        {
            if (!silent) Console.Write(message);
        }
    }
}