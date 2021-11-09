using System;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Tool.Options;

namespace ServiceBusAdmin.Tool
{
    public class SebaConsole
    {
        private readonly IsVerboseOutput _isVerboseOutput;

        internal IConsole InternalConsole { get; }

        public SebaConsole(IConsole console, IsVerboseOutput isVerboseOutput)
        {
            InternalConsole = console;
            _isVerboseOutput = isVerboseOutput;
        }

        public void Info(string message)
        {
            InternalConsole.ResetColor();
            InternalConsole.Out.WriteLine(message);
        }

        public void Info(string format, params object[]? args)
        {
            InternalConsole.ResetColor();
            InternalConsole.Out.WriteLine(string.Format(format, args));
        }

        public void Error(string message)
        {
            InternalConsole.ForegroundColor = ConsoleColor.Red;
            InternalConsole.Error.WriteLine(message);
        }

        public void Verbose(string message)
        {
            if (!_isVerboseOutput()) return;

            InternalConsole.ResetColor();
            InternalConsole.Out.WriteLine(message);
        }
    }
}