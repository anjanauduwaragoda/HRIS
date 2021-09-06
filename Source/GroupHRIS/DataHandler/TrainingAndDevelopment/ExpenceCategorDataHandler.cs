using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.TrainingAndDevelopment
{
    public class ExpenceCategorDataHandler : TemplateDataHandler
    {
        public bool Insert(string name, string description, string status, string user)
        {
            bool isInsert = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler nserialcode = new SerialHandler();
                string sEXCATEGORY_ID = nserialcode.getserila(mySqlCon, "EC");

                mySqlCmd.Parameters.Add(new MySqlParameter("@sEXCATEGORY_ID", sEXCATEGORY_ID.Trim() == "" ? (object)DBNull.Value : sEXCATEGORY_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@name", name.Trim() == "" ? (object)DBNull.Value : name.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));

                sMySqlString = @"INSERT INTO EXPENSE_CATEGORY (EXPENSE_CATEGORY_ID,CATEGORY_NAME,DESCRIPTION,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
						               VALUES(@sEXCATEGORY_ID,@name,@description,@status,@user,now(),@user,now());";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isInsert = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return isInsert;
        }

        public bool Update(string expenceCategoryId, string name, string description, string status, string user)
        {
            bool isUpdate = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Parameters.Add(new MySqlParameter("@expenceCategoryId", expenceCategoryId.Trim() == "" ? (object)DBNull.Value : expenceCategoryId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@name", name.Trim() == "" ? (object)DBNull.Value : name.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));

                sMySqlString = @"UPDATE EXPENSE_CATEGORY SET CATEGORY_NAME= @name,
                                    DESCRIPTION = @description,
                                    STATUS_CODE = @status,
                                    MODIFIED_BY = @user,
                                    MODIFIED_DATE = now()
                                    WHERE EXPENSE_CATEGORY_ID = @expenceCategoryId;";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isUpdate = true;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }

            return isUpdate;
        }

        public DataTable getAllCategory()
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    EXPENSE_CATEGORY_ID,
                                    CATEGORY_NAME,
                                    DESCRIPTION,
                                    CASE
                                        WHEN STATUS_CODE = '1' THEN 'Active'
                                        WHEN STATUS_CODE = '0' THEN 'Inactive'
                                    End AS STATUS_CODE
                                FROM
                                    EXPENSE_CATEGORY ORDER BY CATEGORY_NAME;";

                mySqlCmd.CommandText = Qry;
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
