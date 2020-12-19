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
    public class UserCreator : AbstractManipulator
    {
        private SerializedUser user { get; }
        
        private SqlParameter returnValue { get; set; }

        public UserCreator(SerializedUser user)
        {
            this.user = user;
        }
        protected override void addParameters(SqlCommand command)
        {
            validateParameters();
            command.CommandType = System.Data.CommandType.StoredProcedure;
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
            return "insert_user";
        }

        protected override Response getSuccessMessage()
        {
            if((int) returnValue.Value == -1)
            {
                Console.WriteLine("There is already a user with this email!");
                throw new InvalidInputException("There is already a user with this email!");

            }
            return new Response("New user successfully saved!");
        }
    }
}
