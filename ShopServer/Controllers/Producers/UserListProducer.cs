using Commons.Items;
using DoctorCSharp.Model.Items;
using DoctorCSharpServer.Model.Items;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Controllers.Producers
{
    public class UserListProducer : AbstractProducer
    {

        private readonly string filter;

        public UserListProducer(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                filter = "%";
            }
            else
            {
                filter = "%" + filter + "%";
            }
            this.filter = filter;
        }

        protected override void addParameters(SqlCommand command)
        {
            command.Parameters.AddWithValue("@p0", this.filter);
        }

        protected override Object getItemFromReader(SqlDataReader reader)
        {
            return new User(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3)
                );
        }

        protected override string getSqlCommand()
        {
            return "SELECT id, name, email, password FROM Users WHERE name LIKE @p0 OR email LIKE @p0";
        }
    }
}
