using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataAccess
{
    public class UserActivities : IUserActivities
    {
        public User Regitration(User user)
        {
            try
            {
                SqlParameter[] sqlParameters = SetupRegistrationParameters(user);
                SqlParameterCollection sqlParameterCollection = SqlUtilityClass.ExecuteNonQuery("RegisterUser", sqlParameters);
                int userid = Convert.ToInt32(sqlParameterCollection["@UserId"].Value);
                if (userid > 0)
                {
                    user.UserId = userid;
                }
            }
            catch (Exception)
            {

                throw;
            }


            return user;
        }

        public bool Logout(User user)
        {
            return true;
        }

        public User Login(User user)
        {
            // return true;LoginUser   @UserId=UserId,@UserName=UseName,@Role

            try
            {
                SqlParameter[] sqlParameters = SetupLoginParameters(user);
                SqlParameterCollection sqlParameterCollection = SqlUtilityClass.ExecuteNonQuery("LoginUser", sqlParameters);
                int userid = Convert.ToInt32(sqlParameterCollection["@UserId"].Value);

                if (userid > 0)
                {
                    user.UserId = userid;
                    user.Name = sqlParameterCollection["@UserName"].Value.ToString();
                    user.Role = Convert.ToInt32(sqlParameterCollection["@Role"].Value);

                }
            }
            catch (Exception)
            {

                throw;
            }

            return user;
        }

        //public int IssueBook(BookIssue bookIssue)
        //{
        //    SqlParameter[] sqlParameters = SetBookIssueParameters(bookIssue);
        //    int result = SqlUtilityClass.ExecuteNonQuery(sqlParameters, "RemoveBook");

        //    return result;
        //}

        private SqlParameter[] SetupLoginParameters(User user)
        {
            SqlParameter[] sqlParameters = new SqlParameter[4];
            try
            {
                SqlParameter sqlParameterEmail = new SqlParameter("@UserEmail", user.Email);
                sqlParameterEmail.SqlDbType = SqlDbType.VarChar;
                sqlParameterEmail.Size = 50;
                sqlParameterEmail.Direction = System.Data.ParameterDirection.Input;

                sqlParameters[0] = sqlParameterEmail;
                //return sqlParameters

                SqlParameter sqlParameterUserId = new SqlParameter("@UserId", SqlDbType.Int, 20);
                sqlParameterUserId.Direction = System.Data.ParameterDirection.Output;
                sqlParameters[1] = sqlParameterUserId;

                SqlParameter sqlParameterUserName = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
                sqlParameterUserName.Direction = System.Data.ParameterDirection.Output;
                sqlParameters[2] = sqlParameterUserName;

                SqlParameter sqlParameterUseridOutput = new SqlParameter("@Role", SqlDbType.Int, 2);
                sqlParameterUseridOutput.Direction = System.Data.ParameterDirection.Output;
                sqlParameters[3] = sqlParameterUseridOutput;
            }
            catch (Exception)
            {

                throw;
            }

            return sqlParameters;
        }
        private SqlParameter[] SetupRegistrationParameters(User user)
        {
            SqlParameter[] sqlParameters = new SqlParameter[4];
            try
            {
                SqlParameter sqlParameterName = new SqlParameter("@UserName", user.Name);
                sqlParameterName.Direction = System.Data.ParameterDirection.Input;
                sqlParameters[0] = sqlParameterName;

                SqlParameter sqlParameterEmail = new SqlParameter("@UserEmail", user.Email);
                sqlParameterEmail.Direction = System.Data.ParameterDirection.Input;
                sqlParameters[1] = sqlParameterEmail;

                SqlParameter sqlParameterRole = new SqlParameter("@Role", user.Role);
                sqlParameterRole.Direction = System.Data.ParameterDirection.Input;
                sqlParameters[2] = sqlParameterRole;

                SqlParameter sqlParameterUseridOutput = new SqlParameter("@UserId", System.Data.SqlDbType.Int);
                sqlParameterUseridOutput.Direction = System.Data.ParameterDirection.Output;
                sqlParameters[3] = sqlParameterUseridOutput;
            }
            catch (Exception)
            {

                throw;
            }



            return sqlParameters;
        }

        private SqlParameter[] SetBookIssueParameters(BookIssue bookIssue)
        {

            DataTable bookItems = CreateTable();

            foreach (BookItem bi in bookIssue.BookItems)
            {
                bookItems.Rows.Add(bi.BookitemId, bi.BorrowedDate, bi.DueDate);
            }

            SqlParameter[] sqlParameters = new SqlParameter[3];

            SqlParameter sqlParameterUserId = new SqlParameter("@Userid", bookIssue.UserId);
            sqlParameterUserId.Direction = System.Data.ParameterDirection.Input;
            sqlParameters[0] = sqlParameterUserId;

            // Configure the command and parameter.  
            SqlParameter tvpParam = new SqlParameter("@BookItems", bookItems);
            tvpParam.SqlDbType = SqlDbType.Structured;
            tvpParam.TypeName = "dbo.BookItemsTableType";
            tvpParam.Direction = ParameterDirection.Input;
            sqlParameters[1] = tvpParam;

            SqlParameter sqlParameterReferenceNoOutput = new SqlParameter("@ReferenceNo", System.Data.SqlDbType.VarChar, 50);
            sqlParameterReferenceNoOutput.Direction = System.Data.ParameterDirection.Output;
            sqlParameters[2] = sqlParameterReferenceNoOutput;

            return sqlParameters;
        }

        private DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BookItemid", typeof(Int32));
            dt.Columns.Add("BorrowDate", typeof(DateTime));
            dt.Columns.Add("DueDate", typeof(DateTime));
            return dt;
        }

        public string IssueBook(BookIssue bookIssue)
        {
            SqlParameter[] sqlParameters = SetBookIssueParameters(bookIssue);
            SqlParameterCollection sqlParameterCollection = SqlUtilityClass.ExecuteNonQuery("IssueBooks", sqlParameters);

            string ReferenceNo = sqlParameterCollection["@ReferenceNo"].Value.ToString();

            return ReferenceNo;
        }
    }
}
