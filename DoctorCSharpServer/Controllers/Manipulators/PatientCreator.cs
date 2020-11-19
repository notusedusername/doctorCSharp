using DoctorCSharp.Model.Items;
using DoctorCSharpServer.Controllers.Exceptions;
using DoctorCSharpServer.Model.Items;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Controllers.Manipulators
{
    public class PatientCreator : AbstractManipulator
    {
        private SerializedPatient patient { get; }
        
        private SqlParameter returnValue { get; set; }

        public PatientCreator(SerializedPatient patient)
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
            return "insert_patient";
        }

        protected override Response getSuccessMessage()
        {
            if((int) returnValue.Value == -1)
            {
                Console.WriteLine("There is already a patient with this TAJ number!");
                throw new InvalidInputException("There is already a patient with this TAJ number!");

            }
            return new Response("New patient successfully saved!");
        }
    }
}
