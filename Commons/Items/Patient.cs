
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharp.Model.Items
{
    public class Patient
    {
        public int id { get; set; }
        public string name { get; set; }

        public string taj { get; set; }

        public string address { get; set; }
        public string phone { get; set; }

        public Patient(int id, string name, string taj, string address, string phone)
        {
            this.id = id;
            this.name = name;
            this.taj = taj;
            this.address = address;
            this.phone = phone;
        }

        public Patient()
        {

        }
    }
}
