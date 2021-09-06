using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;
namespace DataHandler.MetaData
{
    public class LeaveSchemeDataHandler : TemplateDataHandler
    {
        
        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 07-07-2014
        // this function is written to used at webFrmEmploeeLeaveScheme.aspx
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return set of LEAVE_SCHEME_ID and LEAVE_SCHEM_NAME for all leave schems
        ///</summary>        
        //----------------------------------------------------------------------------------------

        public DataTable getLeaveSchemes()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT  LEAVE_SCHEME_ID , LEAVE_SCHEM_NAME,STATUS_CODE " +
                                      " from LEAVE_SCHEME order by LEAVE_SCHEME_ID ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populate()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT  LEAVE_SCHEME_ID , LEAVE_SCHEM_NAME," +
                                        " CASE " +
                                        " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                        " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "'" +
                                        //" when STATUS_CODE='0' then 'Inactive' " +
                                        //" when STATUS_CODE='1' then 'Active' " +
                                        " End  as STATUS_CODE " +
                                        " from LEAVE_SCHEME   order by LEAVE_SCHEME_ID";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataRow populate(string sLeaveSchemeID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT    LEAVE_SCHEM_NAME , STATUS_CODE " +
                                      " FROM   LEAVE_SCHEME " +
                                      " WHERE  LEAVE_SCHEME_ID = '" + sLeaveSchemeID + "'";

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
        public Boolean insert(string sLeaveScheme, string sStatus, DataTable LeaveShemeItemDataTable)
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            LeaveTypeDataHandler oLeaveTypeDataHandler = new LeaveTypeDataHandler();
            for (int i = 0; i < LeaveShemeItemDataTable.Rows.Count; i++)
            {
                LeaveShemeItemDataTable.Rows[i]["Leave Scheme Type"] = oLeaveTypeDataHandler.populateLeaveTypeIDByLeaveTypeName(LeaveShemeItemDataTable.Rows[i]["Leave Scheme Type"].ToString());
            }


                try
                {
                    mySqlCon.Open();

                    mySqlTrans = mySqlCon.BeginTransaction();

                    SerialHandler nserialcode = new SerialHandler();
                    string sLeaveSchemeID = nserialcode.getserila(mySqlCon, "LVS");

                    mySqlCmd.Parameters.Add(new MySqlParameter("@sLeaveSchemeID", sLeaveSchemeID.Trim() == "" ? (object)DBNull.Value : sLeaveSchemeID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sLeaveScheme", sLeaveScheme.Trim() == "" ? (object)DBNull.Value : sLeaveScheme.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));

                    sMySqlString = "INSERT INTO LEAVE_SCHEME  (  LEAVE_SCHEME_ID , LEAVE_SCHEM_NAME , STATUS_CODE ) " +
                                                " VALUES (@sLeaveSchemeID, @sLeaveScheme,@sStatus)";


                    mySqlCmd.Transaction = mySqlTrans;
                    mySqlCmd.CommandText = sMySqlString;

                    mySqlCmd.ExecuteNonQuery();

                    for (int i = 0; i < LeaveShemeItemDataTable.Rows.Count; i++)
                    {
                        string LeaveSchemeType = LeaveShemeItemDataTable.Rows[i]["Leave Scheme Type"].ToString();
                        string NumberofDays = LeaveShemeItemDataTable.Rows[i]["Number of Days"].ToString();
                        string Remarks = LeaveShemeItemDataTable.Rows[i]["Remarks"].ToString();

                        mySqlCmd.Parameters.Add(new MySqlParameter("@LEAVE_TYPE_ID", LeaveSchemeType.Trim() == "" ? (object)DBNull.Value : LeaveSchemeType.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@NUMBER_OF_DAYS", NumberofDays.Trim() == "" ? (object)DBNull.Value : NumberofDays.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@REMARKS", Remarks.Trim() == "" ? (object)DBNull.Value : Remarks.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@LEAVE_SCHEME_ID", sLeaveSchemeID.Trim() == "" ? (object)DBNull.Value : sLeaveSchemeID.Trim()));

                        string LeavSchemeItemQuery = @"INSERT INTO LEAVE_SCHEME_ITEM(LEAVE_SCHEME_ID,LEAVE_TYPE_ID,NUMBER_OF_DAYS,REMARKS) VALUES(@LEAVE_SCHEME_ID,@LEAVE_TYPE_ID,@NUMBER_OF_DAYS,@REMARKS);";

                        mySqlCmd.CommandText = LeavSchemeItemQuery;
                        mySqlCmd.ExecuteNonQuery();
                        mySqlCmd.Parameters.Clear();

                    }



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
        public Boolean update(string sLeaveSchemeID, string sLeaveScheme, string sStatus, DataTable LeaveShemeItemDataTable)
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sLeaveSchemeID", sLeaveSchemeID.Trim() == "" ? (object)DBNull.Value : sLeaveSchemeID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLeaveScheme", sLeaveScheme.Trim() == "" ? (object)DBNull.Value : sLeaveScheme.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));

                sMySqlString = "UPDATE  LEAVE_SCHEME " +
                                " SET  LEAVE_SCHEM_NAME = @sLeaveScheme, " +
                                " STATUS_CODE = @sStatus " +
                                " WHERE LEAVE_SCHEME_ID  = @sLeaveSchemeID";

                mySqlCmd.Transaction = mySqlTrans;

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();


                sMySqlString = "DELETE FROM LEAVE_SCHEME_ITEM " +
                                " WHERE LEAVE_SCHEME_ID  = @sLeaveSchemeID";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                for (int i = 0; i < LeaveShemeItemDataTable.Rows.Count; i++)
                {
                    string LeaveSchemeType = LeaveShemeItemDataTable.Rows[i][0].ToString();
                    string NumberofDays = LeaveShemeItemDataTable.Rows[i][1].ToString();
                    string Remarks = LeaveShemeItemDataTable.Rows[i][2].ToString();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@LEAVE_SCHEME_ID", sLeaveSchemeID.Trim() == "" ? (object)DBNull.Value : sLeaveSchemeID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@LEAVE_TYPE_NAME", LeaveSchemeType.Trim() == "" ? (object)DBNull.Value : LeaveSchemeType.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@NUMBER_OF_DAYS", NumberofDays.Trim() == "" ? (object)DBNull.Value : NumberofDays.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@REMARKS", Remarks.Trim() == "" ? (object)DBNull.Value : Remarks.Trim()));

                    string LeavSchemeItemQuery = @"INSERT INTO LEAVE_SCHEME_ITEM(LEAVE_SCHEME_ID,LEAVE_TYPE_ID,NUMBER_OF_DAYS,REMARKS) VALUES(@LEAVE_SCHEME_ID,(SELECT LEAVE_TYPE_ID FROM  LEAVE_TYPE WHERE LEAVE_TYPE_NAME=@LEAVE_TYPE_NAME),@NUMBER_OF_DAYS,@REMARKS);";

                    mySqlCmd.CommandText = LeavSchemeItemQuery;
                    mySqlCmd.ExecuteNonQuery();
                    mySqlCmd.Parameters.Clear();

                }




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
        public DataTable populateSchemeItems(string sLeaveSchemeID)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = " SELECT LT.LEAVE_TYPE_NAME as 'Leave Type',LSI.NUMBER_OF_DAYS as 'Number of Days',LSI.REMARKS as 'Remarks' " +
                                      " FROM  LEAVE_SCHEME_ITEM LSI,LEAVE_TYPE LT " +
                                      " WHERE LSI.LEAVE_SCHEME_ID = '" + sLeaveSchemeID + "' and LT.LEAVE_TYPE_ID=LSI.LEAVE_TYPE_ID ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public Boolean CheckPrevRecord(string LeaveType)
        {
            Boolean State = false;

            try
            {
                MySqlCommand MSC = new MySqlCommand();
                MSC.Connection = mySqlCon;
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                MSC.Parameters.Add(new MySqlParameter("@LeaveType", LeaveType.Trim() == "" ? (object)DBNull.Value : LeaveType.Trim()));
                string sMySqlString = @"select * from LEAVE_SCHEME where LEAVE_SCHEM_NAME=@LeaveType;";
                MSC.CommandText = sMySqlString;

                MySqlDataReader MDR = MSC.ExecuteReader();
                if (MDR.Read())
                {
                    State = true;
                }

                return State;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }


        public DataTable CheckAssignEmployees(string LeaveSchemeID)
        {
            try
            {
                MySqlCommand MSC = new MySqlCommand();
                MSC.Connection = mySqlCon;
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                MSC.Parameters.Add(new MySqlParameter("@LeaveSchemeID", LeaveSchemeID.Trim() == "" ? (object)DBNull.Value : LeaveSchemeID.Trim()));
                string sMySqlString = @"select EMPLOYEE_ID from EMPLOYEE_LEAVE_SCHEME where LEAVE_SCHEME_ID=@LeaveSchemeID;";
                MSC.CommandText = sMySqlString;

                MySqlDataAdapter MSDA = new MySqlDataAdapter(MSC);
                DataTable DT = new DataTable();
                MSDA.Fill(DT);
                return DT;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

    }
}
