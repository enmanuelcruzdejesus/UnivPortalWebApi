using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace PortalUnivWebApi.Utils.Database
{
    public class GDSDataHelper
    {
        public static string ConnectionString { get; set; }

        public static async Task<IEnumerable<object>> GetAnonymousResults(DbContext unitOfWork, string spName, SqlParameter[] outParameters, params  SqlParameter[] parameters)
        {

            //meh, you only need the context here. I happened to use UnitOfWork pattern and hence this.
            var context = unitOfWork as DbContext;

            DbCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = spName;
            command.Connection = context.Database.GetDbConnection();

            command.Parameters.AddRange(parameters);

            //Forget this if you don't have any out parameters

            if (outParameters != null)
              command.Parameters.AddRange(outParameters);

            try
            {
                command.Connection.Open();
                var reader = await command.ExecuteReaderAsync();
                return reader.ToObjectList();//A custom method implemented below
            }
            finally
            {
                command.Connection.Close();
            }
        }


        public static Dictionary<string, object> GetProcedureParams(string connectionString, string procedureName, object[] paramValues)
        {
            List<string> paramNames = new List<string>();
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("spc_GetProcedureParams", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@procedureName", procedureName);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            paramNames.Add(reader["PARAMETER_NAME"].ToString());

                    }
                }

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                for (int i = 0; i < paramNames.Count; i++)
                {
                    parameters.Add(paramNames[i], paramValues[i]);
                }
                return parameters;
            }



        }


        public static Dictionary<string, object> GetProcedureParams( string procedureName, object[] paramValues)
        {
            List<string> paramNames = new List<string>();
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("spc_GetProcedureParams", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@procedureName", procedureName);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            paramNames.Add(reader["PARAMETER_NAME"].ToString());

                    }
                }

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                for (int i = 0; i < paramNames.Count; i++)
                {
                    parameters.Add(paramNames[i], paramValues[i]);
                }
                return parameters;
            }



        }
    }
}
