using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Reminders
{
    public class ReminderDataHandler : TemplateDataHandler
    {
        public string getCultryDateTime()
        {
            string sCultryDateTime = "";

            mySqlCmd.CommandText = "SELECT  NOW() from dual";
            mySqlCon.Open();

            if (mySqlCmd.ExecuteScalar() != null)
            {
                sCultryDateTime = mySqlCmd.ExecuteScalar().ToString();
            }

            mySqlCon.Close();

            return sCultryDateTime;
        }

        public DataTable populateReminders(string mEmpID,DateTime mExpdate)
        {
            try
            {
                dataTable = new DataTable();
                mExpdate = mExpdate.AddDays(-2);
                string sMySqlString = "SELECT  DESCRIPTION FROM REMINDERS where STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' " +
                     " AND ( REMINDTO = 'ALL' OR  REMINDTO = '" + mEmpID.ToString() + "') " +
                     " AND EXPDATE >= '" + mExpdate.ToString("yyyy-MM-dd") + "' order by EXPDATE,DESCRIPTION";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateRemindersUser(string mEmpID, DateTime mExpdate)
        {
            try
            {
                dataTable.Rows.Clear();
                mExpdate = mExpdate.AddDays(-2);
                string sMySqlString = "SELECT  IDNO,DATE_FORMAT(EXPDATE,'%Y/%m/%d') as EXPDATE,DESCRIPTION FROM REMINDERS where STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' " +
                     " AND  REMINDTO = '" + mEmpID.ToString() + "' " +
                     " AND EXPDATE >= '" + mExpdate.ToString("yyyy-MM-dd") + "' order by EXPDATE,DESCRIPTION";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean InsertReminder(string sRemDate, string sRemiderDescrip, string sRemStatus, string sADDED_BY)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemDate", sRemDate.Trim() == "" ? (object)DBNull.Value : sRemDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemiderDescrip", sRemiderDescrip.Trim() == "" ? (object)DBNull.Value : sRemiderDescrip.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_BY", sADDED_BY.Trim() == "" ? (object)DBNull.Value : sADDED_BY.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemStatus", sRemStatus.Trim() == "" ? (object)DBNull.Value : sRemStatus.Trim()));

                sMySqlString = "INSERT INTO REMINDERS(DESCRIPTION,EXPDATE,REMINDTO,ADDED_BY,STATUS_CODE) " +
                               "VALUES(@sRemiderDescrip,@sRemDate,@sADDED_BY,@sADDED_BY,@sRemStatus)";


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


        public Boolean UpdateReminder(string sId,string sRemDate, string sRemiderDescrip, string sRemStatus, string sADDED_BY)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sId", sId.Trim() == "" ? (object)DBNull.Value : sId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemDate", sRemDate.Trim() == "" ? (object)DBNull.Value : sRemDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemiderDescrip", sRemiderDescrip.Trim() == "" ? (object)DBNull.Value : sRemiderDescrip.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_BY", sADDED_BY.Trim() == "" ? (object)DBNull.Value : sADDED_BY.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemStatus", sRemStatus.Trim() == "" ? (object)DBNull.Value : sRemStatus.Trim()));

                sMySqlString = "UPDATE REMINDERS set DESCRIPTION=@sRemiderDescrip,EXPDATE=@sRemDate,REMINDTO=@sADDED_BY,ADDED_BY=@sADDED_BY,STATUS_CODE=@sRemStatus where idno =@sID ";


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
