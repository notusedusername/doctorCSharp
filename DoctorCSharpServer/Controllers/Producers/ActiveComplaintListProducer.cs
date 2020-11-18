using DoctorCSharpServer.Model.Items;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Controllers.Producers
{
    public class ActiveComplaintListProducer : AbstractProducer
    {

        protected override void addParameters(SqlCommand command)
        {
            
        }

        protected override Object getItemFromReader(SqlDataReader reader)
        {
            return new ActiveComplaint(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4)
                );
        }

        protected override string getSqlCommand()
        {
            return "SELECT t.id, patient_id, p.name name, CONVERT(VARCHAR, arrival, 120), complaint FROM Treatment t inner join patient p on t.patient_id = p.id WHERE t.isClosed = 'F'";
        }
    }
}
