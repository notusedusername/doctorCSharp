using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoctorCSharpServer.Model.Items;
using System.Data.SqlClient;

namespace DoctorCSharpServer.Controllers.Manipulators
{
    public class PatientUpdater : AbstractManipulator
    {
        private SerializedPatient patient { get; }

        private SqlParameter returnValue { get; set; }

        public PatientUpdater(SerializedPatient patient)
        {
            this.patient = patient;
        }

        protected override void addParameters(SqlCommand command)
        {
            validateParameters();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@name", patient.name);
            command.Parameters.AddWithValue("@TAJ_nr", patient.TAJ_nr);
            command.Parameters.AddWithValue("@address", patient.address);
            command.Parameters.AddWithValue("@phone", patient.phone);
            this.returnValue = command.Parameters.Add("@returnValue", System.Data.SqlDbType.Int);
            returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
        }
        private void validateParameters()
        {
            patient.validate();
        }

        protected override string getSqlCommand()
        {
            return "update_patient";
            //return "UPDATE Patient SET [name] = @name,[address] = @address,phone = @phone WHERE TAJ_nr = @TAJ_nr";
        }

        protected override Response getSuccessMessage()
        {
            if ((int)returnValue.Value == -1)
            {
                Console.WriteLine("There is not exists a patient with this TAJ number!");
                return new Response("There is not exists a patient with this TAJ number!");

            }
            return new Response("Patient successfully updated!");
        }
    }
}
