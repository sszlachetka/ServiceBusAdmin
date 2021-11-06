using System;

namespace ServiceBusAdmin
{
    public class SebaCommandParsingException : Exception
    {
        public SebaCommandParsingException(string message) : base(message)
        {
        }
    }
}