using DoctorCSharpServer.Model.Items;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Controllers.Manipulators
{
    public class PatientRemover : AbstractManipulator
    {
        private SerializedPatient patient { get; }

        private SqlParameter returnValue { get; set; }

        public PatientRemover(SerializedPatient patient)
        {
            this.patient = patient;
        }

        protected override void addParameters(SqlCommand command)
        {
            validateParameters();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@TAJ_nr", patient.TAJ_nr);
            this.returnValue = command.Parameters.Add("@returnValue", System.Data.SqlDbType.Int);
            returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
        }
        private void validateParameters()
        {
            patient.validate_taj();
        }

        protected override string getSqlCommand()
        {
            return "delete_patient";
        }

        protected override Response getSuccessMessage()
        {
            if ((int)returnValue.Value == -1)
            {
                Console.WriteLine("There is not exists a patient with this TAJ number!");
                return new Response("There is not exists a patient with this TAJ number!");

            }
            return new Response("Patient successfully deleted!");
        }
    }
}
