using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Items
{
    public class jsonError
    {
        public string message { get; set; }
        public string content { get; set; }

        public jsonError(string message, string content)
        {
            this.message = message;
            this.content = content;
        }
        public jsonError()
        {

        }
    }
}
