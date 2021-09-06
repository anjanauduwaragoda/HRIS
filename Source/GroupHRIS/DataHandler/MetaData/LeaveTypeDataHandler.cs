using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.MetaData
{
    public class LeaveTypeDataHandler : TemplateDataHandler
    {
        public DataTable populate()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT  LEAVE_TYPE_ID , LEAVE_TYPE_NAME, " +
                                        " CASE " +
                                        " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                        " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "'" +
                                        " End  as STATUS_CODE, " +
                                        " CASE " +
                                        "WHEN CATEGORY= 'C' THEN 'Casual' " +
                                        "WHEN CATEGORY= 'M' THEN 'Medical' " +
                                        "WHEN CATEGORY= 'A' THEN 'Annual' " +
                                        "WHEN CATEGORY= 'O' THEN 'Other' " +
                                        " End  as CATEGORY " +
                                        " from LEAVE_TYPE WHERE STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "'  order by LEAVE_TYPE_ID";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateByName(string sLeaveTypeName )
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT   LEAVE_TYPE_NAME " +
                                      " from LEAVE_TYPE   where LEAVE_TYPE_NAME = '" + sLeaveTypeName  + "'";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable populateByID(string sLeaveTypeID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT   LEAVE_TYPE_NAME " +
                                      " from LEAVE_TYPE   where LEAVE_TYPE_ID = '" + sLeaveTypeID + "'";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateByCategory(string sCategoryCode)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT   LEAVE_TYPE_NAME " +
                                      " from LEAVE_TYPE   where category = '" + sCategoryCode + "'";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable populateByCategoryID(string sCategoryCode , string sLeaveID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT  LEAVE_TYPE_NAME " +
                                      " from LEAVE_TYPE   where category = '" + sCategoryCode + "' and LEAVE_TYPE_ID <> '" + sLeaveID  + "'";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string populateLeaveTypeIDByLeaveTypeName(string sLeaveTypeName)
        {
            try
            {
            string sMySqlString = @"SELECT  LEAVE_TYPE_ID 
                                        FROM    LEAVE_TYPE 
                                        WHERE   LEAVE_TYPE_NAME='" + sLeaveTypeName + "';";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                DataTable dt = new DataTable();
                mySqlDa.Fill(dt);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }

        public DataTable populateByNameID(string sLeaveTypeName, string sLeaveTypeID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT   LEAVE_TYPE_NAME " +
                                      " from LEAVE_TYPE   where LEAVE_TYPE_NAME = '" + sLeaveTypeName + "' and LEAVE_TYPE_ID <> '" + sLeaveTypeID + "'";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow populate(string sLeaveTypeID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT  LEAVE_TYPE_ID , LEAVE_TYPE_NAME , STATUS_CODE, CATEGORY " +
                                      " FROM LEAVE_TYPE " +
                                      " WHERE  LEAVE_TYPE_ID = '" + sLeaveTypeID + "'";

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



        public Boolean insert(string sLeaveTypeID, string sLeaveType, string sStatus, string sLeaveCategory)
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sLeaveTypeID", sLeaveTypeID.Trim() == "" ? (object)DBNull.Value : sLeaveTypeID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLeaveType", sLeaveType.Trim() == "" ? (object)DBNull.Value : sLeaveType.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLeaveCategory", sLeaveCategory.Trim() == "" ? (object)DBNull.Value : sLeaveCategory.Trim()));

                sMySqlString = "INSERT INTO LEAVE_TYPE  ( LEAVE_TYPE_ID , LEAVE_TYPE_NAME , STATUS_CODE ,CATEGORY ) " +
                                            " VALUES (@sLeaveTypeID, @sLeaveType,@sStatus,@sLeaveCategory)";


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



        public Boolean update(string sLeaveTypeID, string sLeaveType, string sStatus, string sLeaveCategory)
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();
                
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLeaveTypeID", sLeaveTypeID.Trim() == "" ? (object)DBNull.Value : sLeaveTypeID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLeaveType", sLeaveType.Trim() == "" ? (object)DBNull.Value : sLeaveType.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLeaveCategory", sLeaveCategory.Trim() == "" ? (object)DBNull.Value : sLeaveCategory.Trim()));

                sMySqlString = "UPDATE  LEAVE_TYPE " +
                                " SET LEAVE_TYPE_NAME = @sLeaveType, " +
                                " STATUS_CODE = @sStatus," +
                                " CATEGORY = @sLeaveCategory" +
                                " WHERE LEAVE_TYPE_ID  = @sLeaveTypeID";

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

        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 22-07-2014
        // this function is written to used at webFrmEmployeeLeaveSchedule.aspx
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return category for a given leave type
        ///<param name="leaveTypeId">Pass a leaveTypeId string to query </param>
        ///</summary>        
        //----------------------------------------------------------------------------------------

        public string getLeaveCategory(String leaveTypeId)
        {
            string category = "";

            mySqlCmd.CommandText = "SELECT CATEGORY FROM LEAVE_TYPE WHERE LEAVE_TYPE_ID='" + leaveTypeId.Trim() + "'";

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    category = mySqlCmd.ExecuteScalar().ToString();
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return category;



        }



    }
}
