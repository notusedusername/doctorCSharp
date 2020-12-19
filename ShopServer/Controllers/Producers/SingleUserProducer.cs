using Commons.Items;
using DoctorCSharp.Model.Items;
using DoctorCSharpServer.Controllers.Exceptions;
using DoctorCSharpServer.Model.Items;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Controllers.Producers
{
    public class SingleUserProducer : AbstractProducer
    {

        private readonly int id;

        public SingleUserProducer(int id)
        {
            this.id = id;
        }

        protected override void addParameters(SqlCommand command)
        {
            command.Parameters.AddWithValue("@p0", id);
        }

        protected override object getItemFromReader(SqlDataReader reader)
        {
            return new User(reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3));
        }

        protected override string getSqlCommand()
        {
            return "SELECT id, name, email, password FROM Users WHERE id=@p0";
        }
    }
}
