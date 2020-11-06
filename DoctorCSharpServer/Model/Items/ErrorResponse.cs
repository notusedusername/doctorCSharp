using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Model.Items
{

    public class ErrorResponse
    {
        public readonly static string INTERNAL_ERROR = "Something went wrong :/";
        public readonly static string BAD_REQUEST = "You messed up something...";

        private ErrorResponse()
        {

        }
    }
}
