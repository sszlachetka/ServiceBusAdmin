using System;
using System.IO;
using System.Text;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Tests
{
    internal class TestConsole : IConsole
    {
        private readonly StringBuilder _output = new();
        private readonly StringBuilder _error = new();

        public string OutputText => _output.ToString();
        public string ErrorText => _error.ToString();

        public TestConsole()
        {
            Out = new StringWriter(_output);
            Error = new StringWriter(_error);
        }

        public void ResetColor()
        {
        }

        public TextWriter Out { get; }
        public TextWriter Error { get; }
        public TextReader In => throw new NotImplementedException();
        public bool IsInputRedirected => throw new NotImplementedException();
        public bool IsOutputRedirected => true;
        public bool IsErrorRedirected => true;
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public event ConsoleCancelEventHandler? CancelKeyPress;
    }
}