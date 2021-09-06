using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using Common;

namespace DataHandler.Property
{
    public class PropertyCategoryDataHandler:TemplateDataHandler
    {
        public Boolean Insert(string propertyName, string Status, string user)
        {
            Boolean status = false;
            SerialHandler serialHandler = new SerialHandler();
            string propertyId = "";

            mySqlCmd.Parameters.Clear();
           
            try
            {
                mySqlCon.Open();

                propertyId = serialHandler.getserila(mySqlCon, Constants.PROPERTY_CATEGORY);

                mySqlCmd.Parameters.Add(new MySqlParameter("propertyId", propertyId));
                mySqlCmd.Parameters.Add(new MySqlParameter("propertyName", propertyName));
                mySqlCmd.Parameters.Add(new MySqlParameter("SCode", Status));
                mySqlCmd.Parameters.Add(new MySqlParameter("user", user));

                string Qry = "sp_InsertProperty";

                mySqlCmd.CommandType = CommandType.StoredProcedure;

                mySqlCmd.CommandText = Qry;
                mySqlCmd.ExecuteNonQuery();
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

        public Boolean Update(string propertyName, string Status, string user, string id)
        {
            Boolean status = false;
            if (mySqlCon.State == ConnectionState.Closed)
            {
                mySqlCon.Open();
            }
            string Qry = @"UPDATE PROPERTY_TYPE SET DESCRIPTION = @propertyName,STATUS_CODE=@Status,MODIFIED_BY=@user,MODIFIED_DATE = NOW() WHERE TYPE_ID=@id";
            mySqlCmd.CommandText = Qry;
            //mySqlCmd.CommandType = CommandType.StoredProcedure;
            mySqlCmd.Parameters.Add(new MySqlParameter("@propertyName", propertyName.Trim() == "" ? (object)DBNull.Value : propertyName.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@Status", Status.Trim() == "" ? (object)DBNull.Value : Status.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@id", id.Trim() == "" ? (object)DBNull.Value : id.Trim()));

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
                string Qry = "sp_GetAllProperties";
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

        public Boolean IsExist(string name)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string Qry = @"SELECT * FROM PROPERTY_TYPE WHERE DESCRIPTION = @name;";
                mySqlCmd.Parameters.Add(new MySqlParameter("@name", name));
                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = Qry;

                MySqlDataAdapter da = new MySqlDataAdapter(mySqlCmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    Status = true;
                }
                else
                {
                    Status = false;
                }

            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return Status;
        }

        public string GetTypeIDByName(string Name)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "SELECT TYPE_ID FROM PROPERTY_TYPE WHERE DESCRIPTION = @Name;";
                mySqlCmd.Parameters.Add(new MySqlParameter("@Name", Name));
                mySqlCmd.CommandText = Qry;
                mySqlCmd.Connection = mySqlCon;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["TYPE_ID"].ToString();
                }
                else
                {
                    return String.Empty;
                }
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
