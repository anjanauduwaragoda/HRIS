using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.MetaData
{
    public class MachineLocationDataHandler :TemplateDataHandler
    {
        public DataTable populate(string sComCode)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT COMPANY_ID, MACHINE_ID, LOCATION, IP_ADDRESS , " +
                                    " CASE " +
                                    " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then 'Inactive' " +
                                    " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then 'Active' " +
                                    " End  as STATUS_CODE " +
                                    "FROM MACHINE_LOCATION  where  COMPANY_ID = '" + sComCode + "' ORDER BY MACHINE_ID";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public DataTable populateByComMacID(string sComCode, string sMachineID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT COMPANY_ID, MACHINE_ID " +
                                       "FROM MACHINE_LOCATION  where  COMPANY_ID = '" + sComCode + "' and MACHINE_ID ='" + sMachineID + "'ORDER BY MACHINE_ID";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public DataRow populateByComID(string sComCode , string sMachineID )
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT * " +
                                     "FROM MACHINE_LOCATION  where  COMPANY_ID = '" + sComCode + "' and MACHINE_ID ='" + sMachineID  + "'ORDER BY MACHINE_ID";

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

        public Boolean insert(string sCompID, string sMachineID, string sLocation, string sBrandName, string sVendor, string sContactNo, string sIPAddress, string sStatus ,string sAddedUser)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            try
            {
                mySqlCon.Open();
                mySqlCmd.Parameters.Add(new MySqlParameter("@sCompID", sCompID.Trim() == "" ? (object)DBNull.Value : sCompID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMachineID", sMachineID.Trim() == "" ? (object)DBNull.Value : sMachineID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLocation", sLocation.Trim() == "" ? (object)DBNull.Value : sLocation.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sBrandName", sBrandName.Trim() == "" ? (object)DBNull.Value : sBrandName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sVendor", sVendor.Trim() == "" ? (object)DBNull.Value : sVendor.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sContactNo", sContactNo.Trim() == "" ? (object)DBNull.Value : sContactNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sIPAddress", sIPAddress.Trim() == "" ? (object)DBNull.Value : sIPAddress.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAddedUser", sAddedUser.Trim() == "" ? (object)DBNull.Value : sAddedUser.Trim()));

                sMySqlString = "INSERT INTO MACHINE_LOCATION  (COMPANY_ID , MACHINE_ID,LOCATION,BRAND_NAME,VENDOR_NAME,CONTACT_NO,IP_ADDRESS,STATUS_CODE,ADDED_BY,ADDED_DATE) " +
                                            " VALUES   (@sCompID, @sMachineID,@sLocation, @sBrandName,@sVendor,@sContactNo,@sIPAddress ,@sStatus,@sAddedUser,now() )";

                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlCon.Close();
                mySqlCmd.Dispose();

                blInserted = true;
            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }
            return blInserted;
        }

        public Boolean update(string sCompID, string sMachineID, string sLocation, string sBrandName, string sVendor, string sContactNo, string sIPAddress, string sStatus,string sModifiedUser)
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sCompID", sCompID.Trim() == "" ? (object)DBNull.Value : sCompID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMachineID", sMachineID.Trim() == "" ? (object)DBNull.Value : sMachineID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLocation", sLocation.Trim() == "" ? (object)DBNull.Value : sLocation.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sBrandName", sBrandName.Trim() == "" ? (object)DBNull.Value : sBrandName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sVendor", sVendor.Trim() == "" ? (object)DBNull.Value : sVendor.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sContactNo", sContactNo.Trim() == "" ? (object)DBNull.Value : sContactNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sIPAddress", sIPAddress.Trim() == "" ? (object)DBNull.Value : sIPAddress.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sModifiedUser", sModifiedUser.Trim() == "" ? (object)DBNull.Value : sModifiedUser.Trim()));
                
                sMySqlString = "UPDATE  MACHINE_LOCATION  " +
                                " SET COMPANY_ID = @sCompID, " +
                                " MACHINE_ID = @sMachineID ," +
                                " LOCATION = @sLocation, " +
                                " BRAND_NAME = @sBrandName, " +
                                " VENDOR_NAME = @sVendor, " +
                                " CONTACT_NO = @sContactNo, " +
                                " IP_ADDRESS = @sIPAddress, " +
                                " STATUS_CODE = @sStatus, " +
                                " MODIFIED_BY = @sModifiedUser , " + 
                                " MODIFIED_DATE = now() " +
                                " WHERE COMPANY_ID  = @sCompID and MACHINE_ID =  @sMachineID ";

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

        public Boolean isIPExist(string IPAddress)
        {
            Boolean isExist = false;
            try
            {
                dataTable.Rows.Clear();
                //mySqlCmd.Parameters.Add(new MySqlParameter("@IPAddress", IPAddress));

                string sMySqlString = " SELECT * FROM MACHINE_LOCATION WHERE IP_ADDRESS= '" + IPAddress.Trim() + "'";               

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExist = true;
                }
                mySqlCmd.Parameters.Clear();
                return isExist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
