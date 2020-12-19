using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Model
{
    public class DatabaseConnection : IDatabaseConnection
    {
        
        private static DatabaseConnection _databaseConnection = new DatabaseConnection();

        private string _connectionString;

        private DatabaseConnection()
        {
        }

        public static DatabaseConnection getInstance()
        {
            return _databaseConnection;
        }

        public void initializeWith(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("The database connection string value is invalid");
            }
            this._connectionString = connectionString;
        }
        
        public SqlConnection GetConnection()
        {
            if (isInitialized())
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();
                return connection;
            }
            else
            {
                throw new InvalidOperationException("The class was not initialized! " + this.GetType().Name);
            }
            
        }

        private bool isInitialized()
        {
            return _connectionString != null;
        }
    }
}
