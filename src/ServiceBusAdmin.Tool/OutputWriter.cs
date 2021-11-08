using System;

namespace ServiceBusAdmin.Tool
{
    public interface IOutputWriter
    {
        void WriteLine(string message);
        void WriteLine(string format, params object[] args);
        void WriteErrorLine(string message);
    }

    public class OutputWriter : IOutputWriter
    {
        public void WriteLine(string message)
        {
            Console.ResetColor();
            Console.WriteLine(message);
        }

        public void WriteLine(string format, params object[] args)
        {
            Console.ResetColor();
            Console.WriteLine(format, args);
        }

        public void WriteErrorLine(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(message);
        }
    }
}