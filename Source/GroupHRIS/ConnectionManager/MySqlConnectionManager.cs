using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace ConnectionManager
{
    public class MySqlConnectionManager
    {
        private static MySqlConnection MySqlCon;

        static MySqlConnectionManager()
        {
            MySqlConnectionManager.MySqlCon = null;
        }

        public MySqlConnectionManager()
        {

        }

        //public static MySqlConnection getConnection()
        //{
        //    if (MySqlCon == null)
        //    {
        //        MySqlCon = new MySqlConnection();

        //        string strConnString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

        //        MySqlCon.ConnectionString = strConnString;
        //    }

        //    return MySqlCon;
        //}

        public static MySqlConnection getConnection()
        {
            if (MySqlCon == null || MySqlCon.State != ConnectionState.Closed)
            {
                MySqlCon = null;
                MySqlCon = new MySqlConnection();

                string strConnString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;

                MySqlCon.ConnectionString = strConnString;
            }

            return MySqlCon;
        }
    }
}