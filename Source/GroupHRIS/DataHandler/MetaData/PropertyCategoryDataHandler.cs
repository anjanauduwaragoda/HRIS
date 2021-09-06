using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataHandler.MetaData
{
    public class PropertyCategoryDataHandler : TemplateDataHandler
    {
        public Boolean InsertProperty(string propertyName,string Status,string user)
        {
            Boolean status = false;
            SerialHandler serialHandler = new SerialHandler();
            string propertyId = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Clear();
            mySqlCmd.Parameters.Add(new MySqlParameter("@propertyName", propertyName.Trim() == "" ? (object)DBNull.Value : propertyName.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@Status", Status.Trim() == "" ? (object)DBNull.Value : Status.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));
            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@propertyId", propertyId.Trim() == "" ? (object)DBNull.Value : propertyId.Trim()));

                string Qry = "INSERT INTO PROPERTY_TYPE(TYPE_ID,DESCRIPTION,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE)" +
                                " VALUES(@propertyId,@propertyName,,@Status,@user,NOW(),@user,NOW())";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = Qry;

                mySqlCmd.ExecuteNonQuery();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                //mySqlTrans.Commit();
                status = true;
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }
            finally
            {
                serialHandler = null;
                mySqlCon.Close();
            }
            return status;
        }

        public Boolean UpdateProperty(string propertyName, string Status,string user,string id)
        {
            Boolean status = false;
            if (mySqlCon.State == ConnectionState.Closed)
            {
                mySqlCon.Open();
            }
            string Qry = @"UPDATE PROPERTY_TYPE SET DESCRIPTION = @propertyName,STATUS_CODE=@Status,MODIFIED_BY=@user WHERE TYPE_ID=@id";
            mySqlCmd.CommandText = Qry;
            mySqlCmd.CommandType = CommandType.StoredProcedure;
            mySqlCmd.Parameters.Add(new MySqlParameter("@propertyName", propertyName.Trim() == "" ? (object)DBNull.Value : propertyName.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@Status", Status.Trim() == "" ? (object)DBNull.Value : Status.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@id",id.Trim() == ""? (object)DBNull.Value : id.Trim()));
                
            mySqlCmd.ExecuteNonQuery();
            return status;
        }

        public DataTable GetAllProperties()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "SELECT TYPE_ID,DESCRIPTION,STATUS_CODE FROM PROPERTY_TYPE";
                mySqlCmd.CommandText = Qry;
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }


    }
}
