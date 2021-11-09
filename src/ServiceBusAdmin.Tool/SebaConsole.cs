using System;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Tool.Options;

namespace ServiceBusAdmin.Tool
{
    public class SebaConsole
    {
        private readonly IConsole _console;
        private readonly IsVerboseOutput _isVerboseOutput;

        internal IConsole InternalConsole => _console;

        public SebaConsole(IConsole console, IsVerboseOutput isVerboseOutput)
        {
            _console = console;
            _isVerboseOutput = isVerboseOutput;
        }

        public void Info(string message)
        {
            _console.ResetColor();
            _console.Out.WriteLine(message);
        }

        public void Info(string format, params object[] args)
        {
            _console.ResetColor();
            _console.Out.WriteLine(string.Format(format, args));
        }

        public void Error(string message)
        {
            _console.ForegroundColor = ConsoleColor.Red;
            _console.Error.WriteLine(message);
        }

        public void Verbose(string message)
        {
            if (!_isVerboseOutput()) return;

            _console.ResetColor();
            _console.Out.WriteLine(message);
        }
    }
}