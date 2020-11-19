
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharp.Model.Items
{
    public class Patient
    {
        public int id { get; }
        public string name { get; }

        public string TAJ_nr { get; }

        public string address { get; }
        public string phone { get; }

        public Patient(int id, string name, string TAJ_nr, string address, string phone)
        {
            this.id = id;
            this.name = name;
            this.TAJ_nr = TAJ_nr;
            this.address = address;
            this.phone = phone;
        }

    }
}
