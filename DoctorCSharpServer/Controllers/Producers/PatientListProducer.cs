using DoctorCSharp.Model.Items;
using DoctorCSharpServer.Model.Items;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Controllers.Producers
{
    public class PatientListProducer : AbstractProducer
    {

        private readonly string filter;

        public PatientListProducer(string filter)
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
            return new Patient(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4)
                );
        }

        protected override string getSqlCommand()
        {
            return "SELECT id, name, TAJ_nr, address, phone FROM Patient WHERE name LIKE @p0 OR TAJ_nr LIKE @p0";
        }
    }
}
