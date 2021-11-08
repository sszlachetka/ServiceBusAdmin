using System;

namespace ServiceBusAdmin.Tool
{
    public class SebaCommandParsingException : Exception
    {
        public SebaCommandParsingException(string message) : base(message)
        {
        }
    }
}