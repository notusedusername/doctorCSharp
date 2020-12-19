using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoctorCSharpServer.Model.Items;
using System.Data.SqlClient;
using DoctorCSharpServer.Controllers.Exceptions;
using DoctorCSharp.Model.Items;

namespace DoctorCSharpServer.Controllers.Manipulators
{
    public class PatientUpdater : AbstractManipulator
    {
        private SerializedUser user { get; }

        private int id { get; }

        private SqlParameter returnValue { get; set; }

        public PatientUpdater(int id,  SerializedUser user)
        {
            this.id = id;
            this.user = user;
        }

        protected override void addParameters(SqlCommand command)
        {
            validateParameters();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@name", user.name);
            command.Parameters.AddWithValue("@email", user.email);
            command.Parameters.AddWithValue("@password", user.password);
            this.returnValue = command.Parameters.Add("@returnValue", System.Data.SqlDbType.Int);
            returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
        }
        private void validateParameters()
        {
            user.validate();
        }

        protected override string getSqlCommand()
        {
            return "update_user";
        }

        protected override Response getSuccessMessage()
        {
            if ((int)returnValue.Value == -1)
            {
                Console.WriteLine("There is not exists a user with the id " + id + "!");
                throw new InvalidInputException("There is not exists a user with the id " + id + "!");

            }
            else if((int)returnValue.Value == -2)
            {
                Console.WriteLine("The email belongs to another user!");
                throw new InvalidInputException("The email belongs to another user!");
            }
            return new Response("User successfully updated!");
        }
    }
}
