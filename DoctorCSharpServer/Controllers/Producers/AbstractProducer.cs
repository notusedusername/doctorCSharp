using DoctorCSharpServer.Model;
using DoctorCSharpServer.Model.Items;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DoctorCSharpServer.Controllers.Producers
{
    public abstract class AbstractProducer
    {

        public AbstractProducer()
        {

        }

        public IEnumerable<Object> select()
        {
            SqlDataReader reader;
            List<Object> list = new List<Object>();
                using (SqlConnection connection = DatabaseConnection.getInstance().GetConnection())
                {

                    using (SqlCommand command = new SqlCommand(getSqlCommand(), connection))
                    {
                        command.Prepare();
                        addParameters(command);
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            list.Add(getItemFromReader(reader));
                        }
                    }
                }
                return list;
       
        }

        protected abstract void addParameters(SqlCommand command);

        protected abstract Object getItemFromReader(SqlDataReader reader);

        protected abstract string getSqlCommand();
    }
}
