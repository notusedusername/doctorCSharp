using DoctorCSharpServer.Model.Items;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Controllers.Manipulators
{
    public class TreatmentCreator : AbstractManipulator
    {
        private SerializedTreatment treatment { get; }

        private SqlParameter returnValue { get; set; }

        public TreatmentCreator(SerializedTreatment treatment)
        {
            this.treatment = treatment;
        }
        protected override void addParameters(SqlCommand command)
        {
            validateParameters();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@patient_id", treatment.patient_id);
            command.Parameters.AddWithValue("@arrival", treatment.arrival);
            command.Parameters.AddWithValue("@complaint", treatment.complaint);
            command.Parameters.AddWithValue("@diagnosis", treatment.diagnosis);
            command.Parameters.AddWithValue("@isClosed", treatment.isClosed);
            this.returnValue = command.Parameters.Add("@returnValue", System.Data.SqlDbType.Int);
            returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
        }

        private void validateParameters()
        {
            treatment.validate();
        }

        protected override string getSqlCommand()
        {
            return "insert_treatment";
        }

        protected override Response getSuccessMessage()
        {
            if ((int)returnValue.Value == -1)
            {
                Console.WriteLine("There is already a treatment with this patient_id!");
                return new Response("There is already a patient with this patient_id");

            }
            return new Response("New treatment successfully saved!");
        }
    }
}
