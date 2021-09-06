using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;
namespace DataHandler.MetaData
{
    public class BranchManagerDataHandler : TemplateDataHandler
    {
        
        public DataTable populateByBranchID(string sBranchID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT BM.LINE_NO, BM.BRANCH_ID ,BM.EMPLOYEE_ID ,DATE_FORMAT(BM.DATE_START,'%Y/%m/%d') as DATE_START ,DATE_FORMAT(BM.DATE_END,'%Y/%m/%d') as DATE_END , B.BRANCH_NAME," +
                                        " CASE " +
                                        " when BM.STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                        " when BM.STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "'" +
                                        " End  as STATUS_CODE " +
                                        "  FROM COMPANY_BRANCH_MANAGER BM,COMPANY_BRANCH B WHERE BM.BRANCH_ID = '" + sBranchID + "'" + " and  BM.BRANCH_ID = B.BRANCH_ID   ORDER BY LINE_NO";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public DataTable populateActiveData(string sBranchID)
        //{
        //    try
        //    {
        //        dataTable.Rows.Clear();
        //        string sMySqlString = "SELECT BM.LINE_NO, BM.BRANCH_ID ,BM.EMPLOYEE_ID ,DATE_FORMAT(BM.DATE_START,'%Y/%m/%d') as DATE_START ,DATE_FORMAT(BM.DATE_END,'%Y/%m/%d') as DATE_END , B.BRANCH_NAME," +
        //                                " CASE " +
        //                                " when BM.STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
        //                                " when BM.STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "'" +
        //                                " End  as STATUS_CODE " +
        //                                "  FROM COMPANY_BRANCH_MANAGER BM,COMPANY_BRANCH B WHERE BM.BRANCH_ID = '" + sBranchID + "'" + " and  BM.BRANCH_ID = B.BRANCH_ID  AND BM.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "'  ORDER BY LINE_NO";

        //        MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
        //        mySqlDa.Fill(dataTable);
        //        return dataTable;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        public DataTable populateActiveData(string sBranchID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT LINE_NO    FROM COMPANY_BRANCH_MANAGER  where  STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' and BRANCH_ID  ='" + sBranchID  + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow populate(string sLineNo)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = "SELECT  BRANCH_ID, EMPLOYEE_ID, DATE_FORMAT(DATE_START,'%Y/%m/%d') as DATE_START, DATE_FORMAT(DATE_END,'%Y/%m/%d') as DATE_END, STATUS_CODE, REMARKS " +
                                    " FROM COMPANY_BRANCH_MANAGER " +
                                    " WHERE LINE_NO ='" + sLineNo + "'";

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

        

        public Boolean insert(string sBranchID, string sEmpID, string sDateStart, string sDateEnd, string sRemarks, string sStatus, string sUser )
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sBranchID", sBranchID.Trim() == "" ? (object)DBNull.Value : sBranchID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEmpID", sEmpID.Trim() == "" ? (object)DBNull.Value : sEmpID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDateStart", sDateStart.Trim() == "" ? (object)DBNull.Value : sDateStart.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDateEnd", sDateEnd.Trim() == "" ? (object)DBNull.Value : sDateEnd.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemarks", sRemarks.Trim() == "" ? (object)DBNull.Value : sRemarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sUser", sUser.Trim() == "" ? (object)DBNull.Value : sUser.Trim()));

                sMySqlString = "INSERT INTO COMPANY_BRANCH_MANAGER ( BRANCH_ID, EMPLOYEE_ID, DATE_START, DATE_END,REMARKS, STATUS_CODE, ADDED_BY,ADDED_DATE,MODIFIED_BY ) " +
                                            " VALUES ( @sBranchID,@sEmpID, @sDateStart , @sDateEnd, @sRemarks,@sStatus,  @sUser ,now() , @sUser )";

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

        public Boolean update(string sLineNo, string sBranchID, string sEmpID, string sDateStart, string sDateEnd, string sRemarks, string sStatus, string sModifyUser)
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sLineNo", sLineNo.Trim() == "" ? (object)DBNull.Value : sLineNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sBranchID", sBranchID.Trim() == "" ? (object)DBNull.Value : sBranchID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEmpID", sEmpID.Trim() == "" ? (object)DBNull.Value : sEmpID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDateStart", sDateStart.Trim() == "" ? (object)DBNull.Value : sDateStart.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDateEnd", sDateEnd.Trim() == "" ? (object)DBNull.Value : sDateEnd.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemarks", sRemarks.Trim() == "" ? (object)DBNull.Value : sRemarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sModifyUser", sModifyUser.Trim() == "" ? (object)DBNull.Value : sModifyUser.Trim()));

                sMySqlString = "UPDATE COMPANY_BRANCH_MANAGER " +
                                " SET BRANCH_ID = @sBranchID, " +
                                " EMPLOYEE_ID = @sEmpID, " +
                                " DATE_START = @sDateStart, " +
                                " DATE_END = @sDateEnd, " +
                                " REMARKS = @sRemarks, " +
                                " STATUS_CODE = @sStatus, " +
                                " MODIFIED_BY = @sModifyUser, " +
                                " MODIFIED_DATE = now() " +
                                " WHERE LINE_NO = @sLineNo";

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

    
