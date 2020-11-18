using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DoctorCSharpServer.Controllers.Exceptions;

namespace DoctorCSharpServer.Model.Items
{
    public class SerializedTreatment
    {
        public int patient_id { get; set; }
        public DateTime arrival { get; set; }
        public string complaint { get; set; }
        public string diagnosis { get; set; }
        public string isClosed { get; set; }

        public SerializedTreatment()
        {

        }
        public void validate()
        {
            validateComplaint();
            validateDiagnosis();

        }
        public void validateComplaint()
        {
            if (string.IsNullOrWhiteSpace(complaint))
            {
                throw new InvalidInputException("The value of " + nameof(complaint) + " can not be empty!");
            }
        }
        public void validateDiagnosis()
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                throw new InvalidInputException("The value of " + nameof(diagnosis) + " can not be empty!");
            }
        }
    }
}
