using System;
using System.Collections.Generic;
using System.Text;

namespace AssistentClient.Exceptions
{
    public class InvalidInputException : Exception
    {
        public string message { get; }

        public InvalidInputException(string message)
        {
            this.message = message;
        }
    }
}
