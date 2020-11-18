using DoctorCSharpServer.Controllers.Exceptions;
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
        private string complaint { get; }

        private int id { get;  }

        private SqlParameter returnValue { get; set; }

        public TreatmentCreator(int id, string complaint)
        {
            this.complaint = complaint;
            this.id = id;
        }
        protected override void addParameters(SqlCommand command)
        {
            validateParameters();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@patient_id", id);
            command.Parameters.AddWithValue("@complaint", complaint);
            this.returnValue = command.Parameters.Add("@returnValue", System.Data.SqlDbType.Int);
            returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
        }

        private void validateParameters()
        {
            if (string.IsNullOrWhiteSpace(complaint))
            {
                throw new InvalidInputException("The complaint can not be empty!");
            }
        }

        protected override string getSqlCommand()
        {
            return "pick_up_patient_complaint";
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
                Console.WriteLine("The patient is waiting for treatment.");
                throw new InvalidInputException("The patient is waiting for treatment! You can not pick up new treatment while the older one is not closed.");

            }

            return new Response("New treatment successfully saved!");
        }
    }
}
