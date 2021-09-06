using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;
using DataHandler.Userlogin;

namespace DataHandler.Attendance
{
    public class AttendanceApproveRejectDataHandler : TemplateDataHandler
    {

        public bool UpdateAttendanceLog(DataTable attData)
        {
            Boolean status = false;
            MySqlTransaction mySqlTrans = null;
            string sMySqlString = "";

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                foreach (DataRow dr in attData.Rows)
                {
                    mySqlCmd.Parameters.Clear();

                    string empid = dr["EMPLOYEE_ID"].ToString();
                    string empName = dr["EMPNAME"].ToString();
                    string sDate = dr["ATT_DATE"].ToString();
                    string sTime = dr["ATT_TIME"].ToString();
                    string sCompID = dr["COMPANY_ID"].ToString();
                    string sDirection = dr["DIRECTION"].ToString();
                    string sReasonCode = dr["REASON_CODE"].ToString();
                    string sLocation = dr["BRANCH_ID"].ToString();
                    string sStatusCode = Constants.STATUS_ACTIVE_VALUE;

                    string email = dr["EMAIL"].ToString();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@empid", empid.Trim() == "" ? (object)DBNull.Value : empid.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sCompID", sCompID.Trim() == "" ? (object)DBNull.Value : sCompID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sDirection", sDirection.Trim() == "" ? (object)DBNull.Value : sDirection.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sDate", sDate.Trim() == "" ? (object)DBNull.Value : sDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sLocation", sLocation.Trim() == "" ? (object)DBNull.Value : sLocation.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sTime", sTime.Trim() == "" ? (object)DBNull.Value : sTime.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sReasonCode", sReasonCode.Trim() == "" ? (object)DBNull.Value : sReasonCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sStatusCode", sStatusCode.Trim() == "" ? (object)DBNull.Value : sStatusCode.Trim()));

                    sMySqlString = "UPDATE ATTENDANCE_LOG SET STATUS_CODE =@sStatusCode WHERE  EMPLOYEE_ID=@empid AND ATT_DATE=@sDate AND ATT_TIME=@sTime AND STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "'";
                    mySqlCmd.CommandText = sMySqlString;
                    mySqlCmd.ExecuteNonQuery();
                    string code = Constants.STATUS_ACTIVE_VALUE;

                    string count = getCount(empid, sDate, sTime, sDirection, code);

                    if (sStatusCode == Constants.STATUS_ACTIVE_VALUE && count == "0")
                    {
                        mySqlCmd.Parameters.Clear();

                        mySqlCmd.Parameters.Add(new MySqlParameter("@empid", empid.Trim() == "" ? (object)DBNull.Value : empid.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@sCompID", sCompID.Trim() == "" ? (object)DBNull.Value : sCompID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@sDirection", sDirection.Trim() == "" ? (object)DBNull.Value : sDirection.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@sDate", sDate.Trim() == "" ? (object)DBNull.Value : sDate.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@sLocation", sLocation.Trim() == "" ? (object)DBNull.Value : sLocation.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@sTime", sTime.Trim() == "" ? (object)DBNull.Value : sTime.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@sReasonCode", sReasonCode.Trim() == "" ? (object)DBNull.Value : sReasonCode.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@sStatusCode", sStatusCode.Trim() == "" ? (object)DBNull.Value : sStatusCode.Trim()));

                        sMySqlString = @"INSERT INTO ATTENDANCE(EMPLOYEE_ID,ATT_DATE,ATT_TIME,DIRECTION,COMPANY_ID,BRANCH_ID,REASON_CODE,STATUS_CODE)
                                VALUES(@empid,@sDate,@sTime,@sDirection,@sCompID,@sLocation,@sReasonCode,@sStatusCode)";
                        mySqlCmd.CommandText = sMySqlString;
                        mySqlCmd.ExecuteNonQuery();
                    }

                    //if (sDirection == "1")
                    //{
                    //    sDirection = "IN";
                    //}
                    //else if (sDirection == "0")
                    //{
                    //    sDirection = "OUT";
                    //}

                    //if (email.Trim() != "")
                    //{
                    //    EmailHandler.SendDefaultEmail(Constants.CON_SYSTEM_NAME, email, "", "IN/OUT approval", getApprovedMessage(empName, sDirection, sDate, sTime));
                    //}

                }

                mySqlTrans.Commit();
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Dispose();
                status = true;
            }
            catch (Exception)
            {

                throw;
            }

            return status;
        }

        public bool UpdateAttendanceLogRejection(DataTable attData)
        {
            Boolean status = false;
            MySqlTransaction mySqlTrans = null;
            string sMySqlString = "";

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                foreach (DataRow dr in attData.Rows)
                {
                    mySqlCmd.Parameters.Clear();

                    string empid = dr["EMPLOYEE_ID"].ToString();
                    string empName = dr["EMPNAME"].ToString();
                    string sDate = dr["ATT_DATE"].ToString();
                    string sTime = dr["ATT_TIME"].ToString();
                    string sCompID = dr["COMPANY_ID"].ToString();
                    string sDirection = dr["DIRECTION"].ToString();
                    string sReasonCode = dr["REASON_CODE"].ToString();
                    string sLocation = dr["BRANCH_ID"].ToString();
                    string sStatusCode = Constants.STATUS_OBSOLETE_VALUE;

                    string email = dr["EMAIL"].ToString();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@empid", empid.Trim() == "" ? (object)DBNull.Value : empid.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sCompID", sCompID.Trim() == "" ? (object)DBNull.Value : sCompID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sDirection", sDirection.Trim() == "" ? (object)DBNull.Value : sDirection.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sDate", sDate.Trim() == "" ? (object)DBNull.Value : sDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sLocation", sLocation.Trim() == "" ? (object)DBNull.Value : sLocation.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sTime", sTime.Trim() == "" ? (object)DBNull.Value : sTime.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sReasonCode", sReasonCode.Trim() == "" ? (object)DBNull.Value : sReasonCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sStatusCode", sStatusCode.Trim() == "" ? (object)DBNull.Value : sStatusCode.Trim()));

                    sMySqlString = "UPDATE ATTENDANCE_LOG SET STATUS_CODE =@sStatusCode WHERE  EMPLOYEE_ID=@empid AND ATT_DATE=@sDate AND ATT_TIME=@sTime AND STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "'";
                    mySqlCmd.CommandText = sMySqlString;
                    mySqlCmd.ExecuteNonQuery();
                    string code = Constants.STATUS_ACTIVE_VALUE;

                    //if (sDirection == "1")
                    //{
                    //    sDirection = "IN";
                    //}
                    //else if (sDirection == "0")
                    //{
                    //    sDirection = "OUT";
                    //}

                    //if (email.Trim() != "")
                    //{
                    //    EmailHandler.SendDefaultEmail(Constants.CON_SYSTEM_NAME, email, "", "IN/OUT Rejection", getRejectedMessage(empName, sDirection, sDate, sTime));
                    //}

                }

                mySqlTrans.Commit();
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Dispose();
                status = true;
            }
            catch (Exception)
            {

                throw;
            }

            return status;
        }

        private StringBuilder getApprovedMessage(string sEmpName, string sINOUT, string sAttDate, string sAttTime)
        {

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear " + sEmpName + "," + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Your Clock " + sINOUT + " time " + sAttTime + " on " + sAttDate + " has been approved " + Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }

        private StringBuilder getRejectedMessage(string sEmpName, string sINOUT, string sAttDate, string sAttTime)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear " + sEmpName + "," + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Your Clock " + sINOUT + " time " + sAttTime + " on " + sAttDate + " has been rejected " + Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }

        public String getCount(string empId, string attDate, string attTime, string direction, string stcode)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT Count(*) FROM ATTENDANCE WHERE EMPLOYEE_ID='" + empId + "' AND ATT_DATE='" + attDate + "' AND ATT_TIME='" + attTime + "' AND DIRECTION='" + direction + "' AND STATUS_CODE = '" + stcode + "';";

                MySqlCommand cmd = new MySqlCommand(Qry, mySqlCon);
                String rdr = Convert.ToString(cmd.ExecuteScalar());

                return rdr;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();

            }
        }


    }
}
