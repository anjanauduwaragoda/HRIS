using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.MetaData
{
    public class SalaryComponentDataHandler : TemplateDataHandler
    {
        public DataTable populate()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "  SELECT   COMPONENT_ID, COMPONENT_NAME , REMARKS,PAYROLL_CODE," +
                                        " CASE " +
                                        " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                        " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "'" +

                                        //" when STATUS_CODE='0' then 'Inactive' " +
                                        //" when STATUS_CODE='1' then 'Active' " +
                                        " End  as STATUS_CODE " +
                                        " from SALARY_COMPONENTS order by COMPONENT_NAME ASC;";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable populateActive()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "  SELECT   COMPONENT_ID, COMPONENT_NAME , REMARKS,PAYROLL_CODE," +
                                        " CASE " +
                                        " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                        " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "'" +

                                        //" when STATUS_CODE='0' then 'Inactive' " +
                    //" when STATUS_CODE='1' then 'Active' " +
                                        " End  as STATUS_CODE " +
                                        " from SALARY_COMPONENTS  where STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "'  order by COMPONENT_NAME ASC;";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataRow populate(string sSalaryComponentID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT COMPONENT_NAME , REMARKS,STATUS_CODE,PAYROLL_CODE " +
                                      " FROM SALARY_COMPONENTS " +
                                      " WHERE  COMPONENT_ID = '" + sSalaryComponentID + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                DataRow dataRow = null;

                if (dataTable.Rows.Count > 0)
                {
                    dataRow = dataTable.Rows[0];
                }
                return dataRow;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public DataTable populateByName(string sSalaryComName)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "  SELECT COMPONENT_NAME" +
                                        " from SALARY_COMPONENTS where COMPONENT_NAME = '" + sSalaryComName + "'  order by COMPONENT_ID";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateByPayRollCode(string sPayRollCode)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = " SELECT COMPONENT_ID, PAYROLL_CODE " +
                                      " FROM SALARY_COMPONENTS " +
                                      " WHERE  PAYROLL_CODE = '" + sPayRollCode + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public DataTable populateByNameID(string sSalaryComName,string sSalaryID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "  SELECT COMPONENT_NAME" +
                                        " from SALARY_COMPONENTS where COMPONENT_NAME = '" + sSalaryComName + "' and COMPONENT_ID <> '" + sSalaryID  + "'  order by COMPONENT_ID";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean insert(string sSalaryComName, string sPayrollcode, string sRemarks , string sStatus ,  string slogUser )
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler nserialcode = new SerialHandler();
                string sSalaryComponentID = nserialcode.getserila(mySqlCon, "SAC");

                mySqlCmd.Parameters.Add(new MySqlParameter("@sSalaryComponentID", sSalaryComponentID.Trim() == "" ? (object)DBNull.Value : sSalaryComponentID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSalaryComName", sSalaryComName.Trim() == "" ? (object)DBNull.Value : sSalaryComName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sPayrollcode", sPayrollcode.Trim() == "" ? (object)DBNull.Value : sPayrollcode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemarks", sRemarks.Trim() == "" ? (object)DBNull.Value : sRemarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@sAdddate", sAdddate.Trim() == "" ? (object)DBNull.Value : sAdddate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@slogUser", slogUser.Trim() == "" ? (object)DBNull.Value : slogUser.Trim()));

                sMySqlString = "INSERT INTO SALARY_COMPONENTS (COMPONENT_ID, COMPONENT_NAME, PAYROLL_CODE,REMARKS , STATUS_CODE, ADDED_DATE ,ADDED_BY ) " +
                                            " VALUES (@sSalaryComponentID, @sSalaryComName,@sPayrollcode, @sRemarks , @sStatus , now() , @slogUser)";


                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                blInserted = true;


            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                mySqlTrans.Rollback();
                throw ex;
            }

            return blInserted;

        }

        public Boolean update(string sSalaryComID,string sSalaryComName, string sPayrollcode,string sRemarks, string sStatus,  string slogUser)
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sSalaryComID", sSalaryComID.Trim() == "" ? (object)DBNull.Value : sSalaryComID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSalaryComName", sSalaryComName.Trim() == "" ? (object)DBNull.Value : sSalaryComName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sPayrollcode", sPayrollcode.Trim() == "" ? (object)DBNull.Value : sPayrollcode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemarks", sRemarks.Trim() == "" ? (object)DBNull.Value : sRemarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@sAdddate", sAdddate.Trim() == "" ? (object)DBNull.Value : sAdddate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@slogUser", slogUser.Trim() == "" ? (object)DBNull.Value : slogUser.Trim()));

                sMySqlString = "UPDATE  SALARY_COMPONENTS  " +
                                " SET COMPONENT_NAME = @sSalaryComName, " +
                                " PAYROLL_CODE = @sPayrollcode ," +
                                " REMARKS = @sRemarks ," +
                                " STATUS_CODE = @sStatus, " +
                                " MODIFIED_BY = @slogUser, " +
                                " MODIFIED_DATE = now() " +
                                " WHERE COMPONENT_ID  = @sSalaryComID";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                blInserted = true;


            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                mySqlTrans.Rollback();
                throw ex;
            }

            return blInserted;

        }

    }
}
