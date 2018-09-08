using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApiImageUpload.DataAccessLayer
{
    public class DatabaseAccess
    {
        public static string GetConnetionString()
        {
            string conn = string.Empty;
            conn = "Data Source = .; Initial Catalog = GeoStudent; Integrated Security = True";
            return conn;
        }

        public static SqlConnection GetConnection()
        {
            string cons = GetConnetionString();
            SqlConnection cn = new SqlConnection(cons);
            cn.Open();
            return cn;
        }

        public static string DALExecuteCommand(string storedProcName, Dictionary<string, SqlParameter> procParameters)
        {
            string comm = string.Empty;
            int rc;
            using (SqlConnection cn = GetConnection())
            {
                // create a SQL command to execute the stored procedure
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcName;
                    // assigning the parameters passed in the command
                    foreach (var procParameter in procParameters)
                    {
                        cmd.Parameters.Add(procParameter.Value);
                    }

                    rc = cmd.ExecuteNonQuery();
                    comm = rc.ToString();
                }
            }
            return comm;
        }

        public static DataSet DALExecuteQuery(string storedProcName, Dictionary<string, SqlParameter> procParameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection cn = GetConnection())
            {
                // create a SQL command to execute the stored procedure
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcName;
                    // assign parameters passed in to the command
                    foreach (var procParameter in procParameters)
                    {
                        cmd.Parameters.Add(procParameter.Value);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }

                return ds;
            }

        }
    }
}