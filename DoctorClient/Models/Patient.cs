using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AssistentClient.Models
{
    public class Patient
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string taj { get; set; }
        public string phone { get; set; }

        public Patient(string name)
        {
            this.name = name;
        }
        public Patient(int id, string name, string address, string taj, string phone)
        {
            this.id = id;
            this.name = name;
            this.address = address;
            this.taj = taj;
            this.phone = phone;
        }
        public Patient()
        {
        }

        public override string ToString()
        {
            return name.ToString();
        }

    }
}
