using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Model.Items
{
    public class ActiveComplaint
    {
        public int id { get; set; }
        public int patient_id { get; set; }

        public string patient_name { get; set; }

        public string arrival { get; set; }
        public string complaint { get; set; }
       

        public ActiveComplaint(int id, int patient_id, string patient_name, string arrival, string complaint)
        {
            this.id = id;
            this.patient_id = patient_id;
            this.patient_name = patient_name;
            this.arrival = arrival;
            this.complaint = complaint;
        }
    }
}
