using System;
using ConnectionManager;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler
{
    public abstract class TemplateDataHandler
    {
        protected MySqlConnection mySqlCon;
        protected MySqlCommand mySqlCmd;
        protected DataTable dataTable;


        //protected TemplateDataHandler()
        //{
        //    try
        //    {
        //        mySqlCon = MySqlConnectionManager.getConnection();
        //        mySqlCmd = mySqlCon.CreateCommand();
        //        dataTable = new DataTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (mySqlCon.State == ConnectionState.Open)
        //        {
        //            mySqlCon.Close();
        //        }
        //    }

        //}


        protected TemplateDataHandler()
        {
            try
            {
                mySqlCon = MySqlConnectionManager.getConnection();
                mySqlCmd = mySqlCon.CreateCommand();
                dataTable = new DataTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (mySqlCon.State != ConnectionState.Closed)
                {
                    mySqlCon.Close();
                }
            }

        }

    }
}
