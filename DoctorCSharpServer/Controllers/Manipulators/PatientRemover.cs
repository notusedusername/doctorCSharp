using DoctorCSharpServer.Controllers.Exceptions;
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
        private int id { get; }

        private SqlParameter returnValue { get; set; }

        public PatientRemover(int id)
        {
            this.id = id; ;
        }

        protected override void addParameters(SqlCommand command)
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id); ;
            this.returnValue = command.Parameters.Add("@returnValue", System.Data.SqlDbType.Int);
            returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
        }

        protected override string getSqlCommand()
        {
            return "delete_patient";
        }

        protected override Response getSuccessMessage()
        {
            if ((int)returnValue.Value == -1)
            {
                Console.WriteLine("There is not exists a patient with id " + id + "!");
                throw new InvalidInputException("There is not exists a patient with id " + id + "!");

            }
            return new Response("Patient successfully deleted!");
        }
    }
}
