using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace HotelAppLibrary.Databases
{
    public class SqlDataAccess : ISqlDataAccess
    {
        // _config saved for life of instanciated SqlDataAccess
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            // Pass in config and save it to class instance for the life of the class
            _config = config;
        }


        public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionStringName, bool isStoredProcedure = false)
        {
            // Assume CommandType is text unless we pass in an isStoredProcedure parameter as true
            CommandType commandType = CommandType.Text;

            // Check if isStoredProcedure parameter is true, and if it is set commandType to stored procedure
            if (isStoredProcedure == true)
            {
                // If it is set to stored procedure change the command type for the sql statement below
                commandType = CommandType.StoredProcedure;
            }

            // Get the connection string from the config
            string connectionString = _config.GetConnectionString(connectionStringName);

            // Make a connection to the Sql Server and pass in the connection string from the config
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                // Make a query to the sql server using the sqlStatement, paramaters and commandType
                // Save the returned rows to a Generic List <T> of whatever type we requested.
                // Return rows
                List<T> rows = connection.Query<T>(sqlStatement, parameters, commandType: commandType).ToList();
                return rows;
            }
        }

        public void SaveData<T>(string sqlStatement, T parameters, string connectionStringName, bool isStoredProcedure = false)
        {
            // Assume CommandType is text unless we pass in an isStoredProcedure parameter as true
            CommandType commandType = CommandType.Text;

            // Check if isStoredProcedure parameter is true, and if it is set commandType to stored procedure
            if (isStoredProcedure == true)
            {
                // If it is set to stored procedure change the command type for the sql statement below
                commandType = CommandType.StoredProcedure;
            }

            // Get the connection string from the config
            string connectionString = _config.GetConnectionString(connectionStringName);

            // Make a connection to the Sql Server and pass in the connection string from the config
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                // Exectue the provided sqlStatement with parameters and specified command type
                connection.Execute(sqlStatement, parameters, commandType: commandType);
            }
        }
    }
}
