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
    public class DivisionHeadDatahandler : TemplateDataHandler
    {

        //public DataTable populate()
        //{
        //    try
        //    {
        //        dataTable.Rows.Clear();
        //        string sMySqlString = "  SELECT DV.LINE_NO, DV.DIVISION_ID,DV.EMPLOYEE_ID ,DATE_FORMAT(DV.DATE_START,'%Y/%m/%d') as DATE_START  ,  DATE_FORMAT(DV.DATE_END,'%Y/%m/%d')  as DATE_END , EP.FIRST_NAME " +
        //            " FROM DIVISION_HEAD DV, EMPLOYEE EP WHERE  DV.EMPLOYEE_ID = EP.EMPLOYEE_ID order by DV.LINE_NO";

        //        MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
        //        mySqlDa.Fill(dataTable);
        //        return dataTable;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        public DataTable populate(string sDivID)
        {
            try
            {
                dataTable.Rows.Clear();
                //string sMySqlString = " select * from DIVISION_HEAD where DIVISION_ID = ' " + sDivID + "'";
                string sMySqlString = "  SELECT DV.LINE_NO, DV.DIVISION_ID,DV.EMPLOYEE_ID ,DATE_FORMAT(DV.DATE_START,'%Y/%m/%d') as DATE_START  ,  DATE_FORMAT(DV.DATE_END,'%Y/%m/%d')  as DATE_END , EP.INITIALS_NAME AS 'FIRST_NAME' , " +
                                        " CASE " +
                                        " when DV.STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                        " when DV.STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "'" +
                                        " End  as STATUS_CODE " +
                                      " FROM DIVISION_HEAD DV, EMPLOYEE EP WHERE DV.DIVISION_ID = '" + sDivID + "'" + " and   DV.EMPLOYEE_ID = EP.EMPLOYEE_ID order by DV.LINE_NO";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable populateActiveData(string sDivID)
        {
            try
            {
                dataTable.Rows.Clear();
                
                string sMySqlString = "  SELECT DIVISION_ID  " +
                                      " FROM DIVISION_HEAD  WHERE DIVISION_ID = '" + sDivID + "'" + " and  STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        
        public DataRow getDivHeadDetails(string lineNo)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = "SELECT LINE_NO , DIVISION_ID,EMPLOYEE_ID ,DATE_FORMAT(DATE_START,'%Y/%m/%d') as DATE_START  ,  DATE_FORMAT(DATE_END,'%Y/%m/%d') as DATE_END ,REMARKS,  STATUS_CODE" +
                                      " FROM DIVISION_HEAD " +
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


        public Boolean insert(string sDivId, string sEmpID, string sDateStart, string sDateEnd, string sRemarks, string sStatus,  string slogUser)
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                //SerialHandler nserialcode = new SerialHandler();
                //string sLineNo = nserialcode.getserila(mySqlCon, "LIN");

                //mySqlCmd.Parameters.Add(new MySqlParameter("@sLineNo", sLineNo.Trim() == "" ? (object)DBNull.Value : sLineNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDivID", sDivId.Trim() == "" ? (object)DBNull.Value : sDivId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEmpID", sEmpID.Trim() == "" ? (object)DBNull.Value : sEmpID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDateStart", sDateStart.Trim() == "" ? (object)DBNull.Value : sDateStart.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDateEnd", sDateEnd.Trim() == "" ? (object)DBNull.Value : sDateEnd.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemarks", sRemarks.Trim() == "" ? (object)DBNull.Value : sRemarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@sadddate", sadddate.Trim() == "" ? (object)DBNull.Value : sadddate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@slogUser", slogUser.Trim() == "" ? (object)DBNull.Value : slogUser.Trim()));

                sMySqlString = "INSERT INTO DIVISION_HEAD ( DIVISION_ID,EMPLOYEE_ID ,DATE_START, DATE_END, REMARKS,STATUS_CODE,ADDED_DATE,ADDED_BY ,MODIFIED_BY ) " +
                                            " VALUES ( @sDivID, @sEmpID,  @sDateStart, @sDateEnd ,@sRemarks, @sStatus, now(), @slogUser, @slogUser)";

                
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

        public Boolean update(string sLineNo, string sDivId, string sEmpID, string sDateStart, string sDateEnd, string sRemarks, string sStatus, string sModUser)
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
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDivID", sDivId.Trim() == "" ? (object)DBNull.Value : sDivId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEmpID", sEmpID.Trim() == "" ? (object)DBNull.Value : sEmpID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDateStart", sDateStart.Trim() == "" ? (object)DBNull.Value : sDateStart.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDateEnd", sDateEnd.Trim() == "" ? (object)DBNull.Value : sDateEnd.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemarks", sRemarks.Trim() == "" ? (object)DBNull.Value : sRemarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@sModDate", sModDate.Trim() == "" ? (object)DBNull.Value : sModDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sModUser", sModUser.Trim() == "" ? (object)DBNull.Value : sModUser.Trim()));

                sMySqlString = "UPDATE  DIVISION_HEAD " +
                                 "SET DIVISION_ID =@sDivID, " +
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
