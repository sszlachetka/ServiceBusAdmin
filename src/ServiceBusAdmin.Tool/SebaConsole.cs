using System;
using System.IO;
using System.Text.Json;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Tool.Options;

namespace ServiceBusAdmin.Tool
{
    public class SebaConsole
    {
        private readonly IsVerboseOutput _isVerboseOutput;
        private readonly IConsole _internalConsole;

        public SebaConsole(IConsole console, IsVerboseOutput isVerboseOutput)
        {
            _internalConsole = console;
            _isVerboseOutput = isVerboseOutput;
        }

        public void Info(object value)
        {
            // TODO: Move serialization/deserialization to single component with the same (configurable?) options: naming policy, enum-as-string 
            WithOutput(output => output.WriteLine(JsonSerializer.Serialize(value)));
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
            _internalConsole.ResetColor();
            action(_internalConsole.Out);
        }

        public void Error(string message)
        {
            _internalConsole.ForegroundColor = ConsoleColor.Red;
            _internalConsole.Error.WriteLine(message);
        }

        public void Verbose(string message)
        {
            if (!_isVerboseOutput()) return;

            _internalConsole.ResetColor();
            _internalConsole.Out.WriteLine(message);
        }
    }
}