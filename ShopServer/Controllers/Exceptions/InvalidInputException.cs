using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Controllers.Exceptions
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
