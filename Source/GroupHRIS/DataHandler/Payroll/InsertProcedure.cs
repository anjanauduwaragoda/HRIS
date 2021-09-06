using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Common;
using System.Data;

namespace DataHandler.Payroll
{
    public class InsertProcedure : TemplateDataHandler
    {
        public Boolean Insert(string query, string remarks, string addedBy)
        {
            Boolean isInsert = false;

            SerialHandler serialHandler = new SerialHandler();

            mySqlCmd.Parameters.Add(new MySqlParameter("@spquery", query.Trim() == "" ? (object)DBNull.Value : query.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

            try
            {
                mySqlCon.Open();
                string spId = serialHandler.getserila(mySqlCon, Constants.STORED_PROCEDURE_ID);

                mySqlCmd.Parameters.Add(new MySqlParameter("@spId", spId.Trim() == "" ? (object)DBNull.Value : spId.Trim()));

                string sMySqlString = @"INSERT INTO STORED_PROCEDURE_DETAILS(PROCEDURE_ID,PROCEDURE_NAME,REMARKS,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                        VALUES(@spId,@spquery,@remarks,@addedBy,Now(),@addedBy,Now());";

                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                isInsert = true;
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }

            return isInsert;
        }

        public DataTable populate()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT PROCEDURE_ID,PROCEDURE_NAME,REMARKS FROM STORED_PROCEDURE_DETAILS;";
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

        public Boolean Update(string meargeId, string query, string remarks, string user)
        {
            Boolean isUpdate = false;

            mySqlCmd.Parameters.Add(new MySqlParameter("@spquery", query.Trim() == "" ? (object)DBNull.Value : query.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@spId", meargeId.Trim() == "" ? (object)DBNull.Value : meargeId.Trim()));

            try
            {

                mySqlCon.Open();
                string sMysqlQryString = @"UPDATE STORED_PROCEDURE_DETAILS 
                                            SET 
                                                PROCEDURE_NAME = @spquery,
                                                REMARKS = @remarks,
                                                MODIFIED_BY = @user,
                                                MODIFIED_DATE = Now()
                                            WHERE
                                                PROCEDURE_ID = @spId;";

                mySqlCmd.CommandText = sMysqlQryString;

                mySqlCmd.ExecuteNonQuery();

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

    }
}
