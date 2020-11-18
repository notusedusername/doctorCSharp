using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoctorCSharpServer.Model.Items;
using System.Data.SqlClient;
using DoctorCSharpServer.Controllers.Exceptions;

namespace DoctorCSharpServer.Controllers.Manipulators
{
    public class PatientUpdater : AbstractManipulator
    {
        private SerializedPatient patient { get; }

        private int id { get; }

        private SqlParameter returnValue { get; set; }

        public PatientUpdater(int id,  SerializedPatient patient)
        {
            this.id = id;
            this.patient = patient;
        }

        protected override void addParameters(SqlCommand command)
        {
            validateParameters();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
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
        }

        protected override Response getSuccessMessage()
        {
            if ((int)returnValue.Value == -1)
            {
                Console.WriteLine("There is not exists a patient with the id " + id + "!");
                throw new InvalidInputException("There is not exists a patient with the id " + id + "!");

            }
            else if((int)returnValue.Value == -2)
            {
                Console.WriteLine("The TAJ belongs to another patient!");
                throw new InvalidInputException("The TAJ belongs to another patient!");
            }
            return new Response("Patient successfully updated!");
        }
    }
}
