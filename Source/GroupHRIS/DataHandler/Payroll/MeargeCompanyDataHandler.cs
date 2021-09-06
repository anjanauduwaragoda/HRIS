using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using Common;

namespace DataHandler.Payroll
{
    public class MeargeCompanyDataHandler : TemplateDataHandler
    {
        public Boolean Insert(string company,string query,string remarks,string addedBy)
        {
            Boolean isInsert = false;

            SerialHandler serialHandler = new SerialHandler();

            mySqlCmd.Parameters.Add(new MySqlParameter("@companyId", company.Trim() == "" ? (object)DBNull.Value : company.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@spquery", query.Trim() == "" ? (object)DBNull.Value : query.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));
            
            try
            {
                mySqlCon.Open();
                string meargeId = serialHandler.getserila(mySqlCon, Constants.MEARGE_ID);

                mySqlCmd.Parameters.Add(new MySqlParameter("@meargeId", meargeId.Trim() == "" ? (object)DBNull.Value : meargeId.Trim()));

                string sMySqlString = @"INSERT INTO MEARGE_COMPANY_OVERTIME(MEARGE_ID,COMPANY_ID,PROCEDURE_ID,REMARKS,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                        VALUES(@meargeId,@companyId,@spquery,@remarks,@addedBy,Now(),@addedBy,Now());";

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
                string Qry = @"SELECT 
                                    m.MEARGE_ID, c.COMP_NAME, d.PROCEDURE_NAME, m.REMARKS
                                FROM
                                    MEARGE_COMPANY_OVERTIME m,
                                    COMPANY c,
                                    STORED_PROCEDURE_DETAILS d
                                WHERE
                                    c.COMPANY_ID = m.COMPANY_ID
                                        AND d.PROCEDURE_ID = m.PROCEDURE_ID";
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

        public Boolean Update(string meargeId,string company,string query,string remarks,string user)
        {
            Boolean isUpdate = false;

            mySqlCmd.Parameters.Add(new MySqlParameter("@companyId", company.Trim() == "" ? (object)DBNull.Value : company.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@spquery", query.Trim() == "" ? (object)DBNull.Value : query.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@meargeId", meargeId.Trim() == "" ? (object)DBNull.Value : meargeId.Trim()));

            try
            {
                
                mySqlCon.Open();
                string sMysqlQryString = @"UPDATE MEARGE_COMPANY_OVERTIME 
                                            SET 
                                                COMPANY_ID = @companyId,
                                                PROCEDURE_ID = @spquery,
                                                REMARKS = @remarks,
                                                MODIFIED_BY = @user,
                                                MODIFIED_DATE = Now()
                                            WHERE
                                                MEARGE_ID = @meargeId;";

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

        public DataTable getStoredProcedure()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT PROCEDURE_ID,PROCEDURE_NAME FROM STORED_PROCEDURE_DETAILS;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
