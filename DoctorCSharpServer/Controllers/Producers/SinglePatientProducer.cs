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
    public class SinglePatientProducer : AbstractProducer
    {

        private readonly int id;

        public SinglePatientProducer(int id)
        {
            this.id = id;
        }

        protected override void addParameters(SqlCommand command)
        {
            command.Parameters.AddWithValue("@p0", id);
        }

        protected override object getItemFromReader(SqlDataReader reader)
        {
            return new Patient(reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4));
        }

        protected override string getSqlCommand()
        {
            return "SELECT id, name, TAJ_nr, address, phone FROM Patient WHERE id=@p0";
        }
    }
}
