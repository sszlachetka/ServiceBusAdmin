using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Tests
{
    public class TestConsole : IConsole
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

        public void RaiseCancelKeyPress()
        {
            // See https://github.com/dotnet/corefx/blob/f2292af3a1794378339d6f5c8adcc0f2019a2cf9/src/System.Console/src/System/ConsoleCancelEventArgs.cs#L14
            var eventArgs = typeof(ConsoleCancelEventArgs)
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .First()
                .Invoke(new object[] { ConsoleSpecialKey.ControlC });
            CancelKeyPress?.Invoke(this, (ConsoleCancelEventArgs)eventArgs);
        }
    }
}