﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Model
{
    interface IDatabaseConnection
    {
        public SqlConnection GetConnection();
    }
}
