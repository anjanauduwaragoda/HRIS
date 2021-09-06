using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace ConnectionManager
{
    public class MSSqlConnectionManager
    {
        private static SqlConnection MSSqlCon;

        static MSSqlConnectionManager()
        {
            MSSqlConnectionManager.MSSqlCon = null;
        }

        public MSSqlConnectionManager()
        {

        }

        public static SqlConnection getConnection(string companyId)
        {
            if (MSSqlCon == null)
            {
                MSSqlCon = new SqlConnection();

                //string strConnString = ConfigurationManager.ConnectionStrings["\" + companyId.Trim() + \""].ConnectionString;
                string strConnString = ConfigurationManager.ConnectionStrings[companyId.Trim()].ConnectionString;
                MSSqlCon.ConnectionString = strConnString;
            }

            return MSSqlCon;
        }


        //private string getConnectionString(string companyId)
        //{
        //    string conString = String.Empty;

        //    //connection string name : MySqlConnectionStringCP02
        //    string connection = "MySqlConnectionString";

        //    string ConnectionStringName = string.Concat(connection + companyId);
        //    conString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;

        //    switch (companyId)
        //    {
        //        case "CP01":
        //            conString = ConfigurationManager.ConnectionStrings[" +   + "].ConnectionString;
        //            break;

        //        default:
        //            //Console.WriteLine("Default case");
        //            break;
        //    }

        //    return conString;
        //}




       
    }
}
