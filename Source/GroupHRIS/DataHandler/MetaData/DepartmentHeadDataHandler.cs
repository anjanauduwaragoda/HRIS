using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.MetaData
{
    public class DepartmentHeadDataHandler : TemplateDataHandler
    {
        //public DataTable populate()
        //{
        //    try
        //    {
        //        dataTable.Rows.Clear();
        //        string sMySqlString = "  SELECT DH.LINE_NO, DH.DEPT_ID, DH.EMPLOYEE_ID ,DATE_FORMAT(DH.DATE_START,'%Y/%m/%d') as DATE_START  ,  DATE_FORMAT(DH.DATE_END,'%Y/%m/%d')  as DATE_END   , EP.FIRST_NAME" +
        //                            " FROM DEPARTMENT_HEAD DH, EMPLOYEE EP WHERE DH.EMPLOYEE_ID = EP.EMPLOYEE_ID order by DH.LINE_NO";

        //        MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
        //        mySqlDa.Fill(dataTable);
        //        return dataTable;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable populate(string sDepID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "  SELECT DH.LINE_NO, DH.DEPT_ID, DH.EMPLOYEE_ID ,DATE_FORMAT(DH.DATE_START,'%Y/%m/%d') as DATE_START  ,  DATE_FORMAT(DH.DATE_END,'%Y/%m/%d')  as DATE_END   , EP.INITIALS_NAME AS 'FIRST_NAME' ," +
                                         " CASE " +
                                        " when DH.STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                        " when DH.STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "'" +
                                        " End  as STATUS_CODE " +
                                        " FROM DEPARTMENT_HEAD DH, EMPLOYEE EP WHERE  DH.DEPT_ID = '" + sDepID + "'" + " and DH.EMPLOYEE_ID = EP.EMPLOYEE_ID  order by DH.LINE_NO";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateActiveData(string sDepID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString  = " SELECT  DEPT_ID FROM DEPARTMENT_HEAD " +
                                      "  WHERE  DEPT_ID = '" + sDepID + "'" + " and  STATUS_CODE = '" +  Constants.STATUS_ACTIVE_VALUE + "'   order by LINE_NO";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        

        public DataRow getDepHeadDetails(string lineNo)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = "SELECT LINE_NO , DEPT_ID,EMPLOYEE_ID ,DATE_FORMAT(DATE_START,'%Y/%m/%d') as DATE_START  ,  DATE_FORMAT(DATE_END,'%Y/%m/%d') as DATE_END ,REMARKS,STATUS_CODE " +
                                    " FROM DEPARTMENT_HEAD " +
                                    " where LINE_NO ='" + lineNo + "'";

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

        public Boolean insert(string sDepId, string sEmpID, string sDateStart, string sDateEnd, string sRemarks, string sStatus,  string slogUser)
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDepID", sDepId.Trim() == "" ? (object)DBNull.Value : sDepId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEmpID", sEmpID.Trim() == "" ? (object)DBNull.Value : sEmpID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDateStart", sDateStart.Trim() == "" ? (object)DBNull.Value : sDateStart.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDateEnd", sDateEnd.Trim() == "" ? (object)DBNull.Value : sDateEnd.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemarks", sRemarks.Trim() == "" ? (object)DBNull.Value : sRemarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@slogUser", slogUser.Trim() == "" ? (object)DBNull.Value : slogUser.Trim()));


                sMySqlString = "INSERT INTO DEPARTMENT_HEAD ( DEPT_ID,EMPLOYEE_ID ,DATE_START, DATE_END, REMARKS,STATUS_CODE,ADDED_DATE,ADDED_BY,MODIFIED_BY ) " +
                                            " VALUES ( @sDepID,@sEmpID,  @sDateStart,@sDateEnd ,@sRemarks, @sStatus, now(),  @slogUser , @slogUser)";

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

        public Boolean update(string sLineNo,string sDepId, string sEmpID, string sDateStart, string sDateEnd, string sRemarks, string sStatus,  string sModUser)
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler nserialcode = new SerialHandler();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sLineNo", sLineNo.Trim() == "" ? (object)DBNull.Value : sLineNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDepID", sDepId.Trim() == "" ? (object)DBNull.Value : sDepId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEmpID", sEmpID.Trim() == "" ? (object)DBNull.Value : sEmpID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDateStart", sDateStart.Trim() == "" ? (object)DBNull.Value : sDateStart.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDateEnd", sDateEnd.Trim() == "" ? (object)DBNull.Value : sDateEnd.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemarks", sRemarks.Trim() == "" ? (object)DBNull.Value : sRemarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sModUser", sModUser.Trim() == "" ? (object)DBNull.Value : sModUser.Trim()));

                sMySqlString = "UPDATE  DEPARTMENT_HEAD " +
                                 "SET DEPT_ID =@sDepID, " +
                                 " EMPLOYEE_ID =@sEmpID, " +
                                 " DATE_START =@sDateStart, " +
                                 " DATE_END =@sDateEnd, " +
                                 " REMARKS = @sRemarks, " +
                                 " STATUS_CODE = @sStatus, " +
                                 " MODIFIED_BY = @sModUser, " +
                                 " MODIFIED_DATE= now() " +
                                 " WHERE LINE_NO = @sLineNo ";

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

