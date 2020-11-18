using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Model.Items
{
    public class Treatment
    {
        public int id { get; set; }
        public int patient_id { get; set; }
        public DateTime arrival { get; set; }
        public string complaint { get; set; }
        public string diagnosis { get; set; }
        public string isClosed { get; set; }

        public Treatment(int id, int patient_id, DateTime arrival, string complaint, string diagnosis, string isClosed)
        {
            this.id = id;
            this.patient_id = patient_id;
            this.arrival = arrival;
            this.complaint = complaint;
            this.diagnosis = diagnosis;
            this.isClosed = isClosed;
        }
    }
}
