
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
    public class DiagnosisCreator : AbstractManipulator
    {
        private string diagnosis { get; }

        private int id { get; }

        private SqlParameter returnValue { get; set; }

        public DiagnosisCreator(int id, string diagnosis)
        {
            this.diagnosis = diagnosis;
            this.id = id;
        }
        protected override void addParameters(SqlCommand command)
        {
            validateParameters();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@patient_id", id);
            command.Parameters.AddWithValue("@diagnosis", diagnosis);
            this.returnValue = command.Parameters.Add("@returnValue", System.Data.SqlDbType.Int);
            returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
        }

        private void validateParameters()
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                throw new InvalidInputException("The " + nameof(diagnosis) + " can not be empty!");
            }
        }

        protected override string getSqlCommand()
        {
            return "set_patient_diagnosis";
        }

        protected override Response getSuccessMessage()
        {
            if ((int)returnValue.Value == -1)
            {
                Console.WriteLine("There is no patient with the id " + id + "!");
                throw new InvalidInputException("There is no patient with the id " + id + "!");

            }
            else if ((int)returnValue.Value == -2)
            {
                Console.WriteLine("The patient is not waiting for diagnosis.");
                throw new InvalidInputException("The patient is not waiting for diagnosis! You can not diagnose the patient if she/he is not present.");

            }

            return new Response("New treatment successfully saved!");
        }
    }
}
