using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorClient.Models
{
    public class ActiveComplaint
    {
        public int id { get; set; }
        public int patient_id { get; set; }
        public DateTime arrival { get; set; }
        public string complaint { get; set; }
        public string diagnosis { get; set; }

        public ActiveComplaint(int id)
        {
            this.id = id;
        }
        public ActiveComplaint(string diagnosis)
        {
            this.diagnosis = diagnosis;
        }


        public ActiveComplaint(int id, int patient_id, DateTime arrival, string complaint)
        {
            this.id = id;
            this.patient_id = patient_id;
            this.arrival = arrival;
            this.complaint = complaint;
        }
        public ActiveComplaint()
        {

        }
    }
}
