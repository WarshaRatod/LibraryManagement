using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DataAccess
{
    public static class SqlUtilityClass
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        public static int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageSize"));

        public static SqlConnection con;
        
        public static void OpenConection()
        {
           
            con = new SqlConnection(ConnectionString);
            con.Open();
        }

        public static void CloseConnection()
        {
            con.Close();
        }

        public static SqlParameterCollection ExecuteNonQuery(string spName,SqlParameter[] sqlParameters)
        {
            SqlParameterCollection sqlParameterCollection = null;
            OpenConection();
            try
            {
                using (SqlCommand cmd = new SqlCommand(spName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(sqlParameters);
                    cmd.ExecuteNonQuery();

                    sqlParameterCollection = cmd.Parameters;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }

            return sqlParameterCollection;


        }


        public static int ExecuteNonQuery(SqlParameter[] sqlParameters,string spName)
        {
            int result = 0;
            OpenConection();
            try
            {
                using (SqlCommand cmd = new SqlCommand(spName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(sqlParameters);
                   result= cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }

            return result;
        }

        public static SqlDataReader ExecuteReader(string Query_)
        {
            SqlDataReader dr = null;
            OpenConection();
            try
            {
                SqlCommand cmd = new SqlCommand(Query_, con);
                dr = cmd.ExecuteReader();
               
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                //CloseConnection();
            }
            return dr;
        }


        public static SqlDataReader ExecuteReader(string Query_,SqlParameter [] sqlParameters)
        {
            SqlDataReader dr = null;
            OpenConection();
            try
            {
                SqlCommand cmd = new SqlCommand(Query_, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(sqlParameters);
                dr = cmd.ExecuteReader();

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                //CloseConnection();
            }
            return dr;
        }

        public static DataSet getDataSet(string Query)
        {
            DataSet ds = null;
            try
            {
                OpenConection();
                SqlDataAdapter dr = new SqlDataAdapter(Query, ConnectionString);
                ds = new DataSet();
                dr.Fill(ds);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                CloseConnection();
            }
            return ds;
        }
    }
}
