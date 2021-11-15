using System;
using System.IO;
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
            WithOutput(output => output.WriteLine(message));
        }

        public void Info(string format, params object[] args)
        {
            WithOutput(output => output.WriteLine(string.Format(format, args)));
        }

        public void Info(long value)
        {
            WithOutput(output => output.WriteLine(value));
        }

        private void WithOutput(Action<TextWriter> action)
        {
            InternalConsole.ResetColor();
            action(InternalConsole.Out);
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