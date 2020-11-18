using DoctorCSharpServer.Model.Items;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Controllers.Producers
{
    public class TreatmentListProducer : AbstractProducer
    {
        private readonly string filter;

        public TreatmentListProducer(string filter)
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
            return new Treatment(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetDateTime(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5)
                );
        }

        protected override string getSqlCommand()
        {
            return "SELECT id, patient_id, arrival, complaint, diagnosis,isClosed FROM Treatment WHERE id LIKE @p0 OR patient_id LIKE @p0";
        }
    }
}
