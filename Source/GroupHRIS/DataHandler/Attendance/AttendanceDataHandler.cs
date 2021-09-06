using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using Common;

namespace DataHandler.Attendance
{
    public class Attendance
    {
        public string Employee_id { get; set; }
        public string Att_Date { get; set; }
        public string Att_Time { get; set; }
        public string Direction { get; set; }
        public string Com_Id { get; set; }
        public string Branch_Id { get; set; }
        public string Reason_Code { get; set; }
        public DateTime AttDateTime { get; set; }

    }

    public class AttendanceDataHandler : TemplateDataHandler
    {
        public DataTable populateAttendance(string mEmpID, DateTime mExpdate)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT  EMPLOYEE_ID,DATE_FORMAT(ATT_DATE,'%Y/%m/%d') as ATT_DATE ,ATT_TIME,DIRECTION,COMPANY_ID,BRANCH_ID,REASON, " +
                                        " CASE " +
                                        " when STATUS_CODE='0' then 'Pending' " +
                                        " when STATUS_CODE='1' then 'Approved' " +
                                        " when STATUS_CODE='9' then 'Rejected' " +
                                        " End as STATUS " +
                                        " FROM ATTENDANCE_LOG " +
                                        " WHERE EMPLOYEE_ID = '" + mEmpID.ToString() + "' " +
                                        " AND ATT_DATE >= '" + mExpdate.ToString("yyyy-MM-dd") + "' and STATUS_CODE<>'" + Constants.STATUS_CANCEL_VALUE + "' order by ATT_DATE,ATT_TIME";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateAttendanceOfficer(string mEmpID, DateTime mExpdate)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT  a.EMPLOYEE_ID,e.KNOWN_NAME as EMPNAME,DATE_FORMAT(a.ATT_DATE,'%Y-%m-%d') as ATT_DATE,a.ATT_TIME,a.COMPANY_ID,a.BRANCH_ID,CONCAT(a.REASON , ' - ', a.REMARK) as REASON,a.REASON_CODE,a.DIRECTION as DIRECTIONINOUT,c.COMP_NAME as COMPANY," +
                            " CASE " +
                            " when a.STATUS_CODE='0' then 'Pending' " +
                            " when a.STATUS_CODE='1' then 'Approved' " +
                            " when a.STATUS_CODE='9' then 'Rejected' " +
                            " End as STATUS, " +
                            " CASE " +
                            " when a.DIRECTION='0' then 'OUT' " +
                            " when a.DIRECTION='1' then 'IN' " +
                            " End as DIRECTION " +
                            " FROM ATTENDANCE_LOG a,EMPLOYEE e,COMPANY c " +
                            " WHERE a.EMPLOYEE_ID  = e.EMPLOYEE_ID  AND a.COMPANY_ID = c.COMPANY_ID " +
                            " AND a.OFFICE_CODE = '" + mEmpID.ToString() + "' " +
                            " AND a.ATT_DATE >= '" + mExpdate.ToString("yyyy-MM-dd") + "' AND  a.STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' Group By a.EMPLOYEE_ID,ATT_DATE,ATT_TIME "; //order by ATT_DATE,ATT_TIME";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean InsertAttendanceLog(string empid, string officerid, string sCompID, string sDirection, string sDate, string sLocation, string sTime, string sReasonCode, string sReason, string sRecommendby, string sStatusCode, string sRemark)
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            //string AttDateTime = DateTime.Today.ToString();
            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Transaction = mySqlTrans;

                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@empid", empid.Trim() == "" ? (object)DBNull.Value : empid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sCompID", sCompID.Trim() == "" ? (object)DBNull.Value : sCompID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDirection", sDirection.Trim() == "" ? (object)DBNull.Value : sDirection.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDate", sDate.Trim() == "" ? (object)DBNull.Value : sDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLocation", empid.Trim() == "" ? (object)DBNull.Value : sLocation.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sTime", sTime.Trim() == "" ? (object)DBNull.Value : sTime.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sReasonCode", sReasonCode.Trim() == "" ? (object)DBNull.Value : sReasonCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sReason", sReason.Trim() == "" ? (object)DBNull.Value : sReason.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRecommendby", sRecommendby.Trim() == "" ? (object)DBNull.Value : sRecommendby.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatusCode", sStatusCode.Trim() == "" ? (object)DBNull.Value : sStatusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemark", sRemark.Trim() == "" ? (object)DBNull.Value : sRemark.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@sAttDateTime", AttDateTime.Trim() == null ? (object)DBNull.Value : AttDateTime.ToString()));

                sMySqlString = "INSERT INTO ATTENDANCE_LOG(EMPLOYEE_ID,ATT_DATE,ATT_TIME,DIRECTION,COMPANY_ID,BRANCH_ID,REASON,REASON_CODE,OFFICE_CODE,STATUS_CODE,REMARK) " +
                                "VALUES(@empid,@sDate,@sTime,@sDirection,@sCompID,@sLocation,@sReason,@sReasonCode,@sRecommendby,@sStatusCode,@sRemark)";

                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();
                

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Parameters.Clear();
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

        public Boolean UpdateAttendanceLog(string empid, string sCompID, string sDirection, string sDate, string sLocation, string sTime, string sReasonCode, string sStatusCode)
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Transaction = mySqlTrans;

                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@empid", empid.Trim() == "" ? (object)DBNull.Value : empid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sCompID", sCompID.Trim() == "" ? (object)DBNull.Value : sCompID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDirection", sDirection.Trim() == "" ? (object)DBNull.Value : sDirection.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDate", sDate.Trim() == "" ? (object)DBNull.Value : sDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLocation", empid.Trim() == "" ? (object)DBNull.Value : sLocation.Trim()));
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
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sLocation", empid.Trim() == "" ? (object)DBNull.Value : sLocation.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sTime", sTime.Trim() == "" ? (object)DBNull.Value : sTime.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sReasonCode", sReasonCode.Trim() == "" ? (object)DBNull.Value : sReasonCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sStatusCode", sStatusCode.Trim() == "" ? (object)DBNull.Value : sStatusCode.Trim()));

                    sMySqlString = @"INSERT INTO ATTENDANCE(EMPLOYEE_ID,ATT_DATE,ATT_TIME,DIRECTION,COMPANY_ID,BRANCH_ID,REASON_CODE,STATUS_CODE)
                                VALUES(@empid,@sDate,@sTime,@sDirection,@sCompID,@sLocation,@sReasonCode,@sStatusCode)";
                    mySqlCmd.CommandText = sMySqlString;
                    mySqlCmd.ExecuteNonQuery();
                }

                mySqlTrans.Commit();
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Parameters.Clear();
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

        public String getCount(string empId,string attDate,string attTime,string direction,string stcode)
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

        public Boolean Insert(List<Attendance> attendance)
        {
            Boolean blInserted = false;
            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Transaction = mySqlTrans;


                foreach (Attendance oAttendance in attendance)
                {
                    if (oAttendance != null)
                    {
                        mySqlCmd.Parameters.Clear();

                        mySqlCmd.Parameters.Add(new MySqlParameter("@EmpId", oAttendance.Employee_id.Trim() == "" ? (object)DBNull.Value : oAttendance.Employee_id.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@AttDate", oAttendance.Att_Date.Trim() == "" ? (object)DBNull.Value : oAttendance.Att_Date.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@AttTime", oAttendance.Att_Time.Trim() == "" ? (object)DBNull.Value : oAttendance.Att_Time.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@Direction", oAttendance.Direction.Trim() == "" ? (object)DBNull.Value : oAttendance.Reason_Code.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ComId", oAttendance.Com_Id.Trim() == "" ? (object)DBNull.Value : oAttendance.Com_Id.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@BranchId", oAttendance.Branch_Id.Trim() == "" ? (object)DBNull.Value : oAttendance.Branch_Id.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ReasonCode", oAttendance.Reason_Code.Trim() == "" ? (object)DBNull.Value : oAttendance.Reason_Code.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("?AttDateTime", oAttendance.AttDateTime == null ? (object)DBNull.Value : oAttendance.AttDateTime.ToString("yyyy-MM-dd HH:mm:ss")));
                        //mySqlCmd.Parameters.Add(new MySqlParameter("@AttDateTime", oAttendance.AttDateTime == null ? (object)DBNull.Value : oAttendance.AttDateTime.ToString("yyyy-MM-dd HH:mm:ss")/*//Unreachable expression code detected waring*/));
                        //mySqlCmd.Parameters.Add(new MySqlParameter("?AttDateTime", oAttendance.AttDateTime == null ? (object)DBNull.Value : oAttendance.AttDateTime.ToString("yyyy-MM-dd HH:mm:ss")));
                        //string s = oAttendance.AttDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                        dataTable.Clear();

                        //STR_TO_DATE(oAttendance.Att_Date.Trim(), '%Y/%m/%d')
                        string querry = "SELECT EMPLOYEE_ID FROM ATTENDANCE where ATT_DATE = STR_TO_DATE('" + oAttendance.Att_Date.ToString() + "', '%Y/%m/%d') and ATT_TIME =TIME_FORMAT('" + oAttendance.Att_Time.ToString() + "','%H:%i:%s') and EMPLOYEE_ID ='" + oAttendance.Employee_id.Trim() + "'";

                        MySqlDataAdapter mySqlDa = new MySqlDataAdapter(querry, mySqlCon);
                        mySqlDa.Fill(dataTable);

                        if (dataTable.Rows.Count == 0)
                        {
                            //sMySqlString = "INSERT INTO ATTENDANCE(EMPLOYEE_ID,ATT_DATE,ATT_TIME,DIRECTION,COMPANY_ID,BRANCH_ID,REASON_CODE,ATT_DATE_TIME) " +
                            //               "VALUES(@EmpId,@AttDate,@AttTime,@Direction,@ComId,@BranchId,@ReasonCode,STR_TO_DATE(@AttDateTime, '%Y%m%d %H%i%s'))";
                            //STR_TO_DATE(@AttDateTime, '%Y%m%d %H%i%s')

                            sMySqlString = "INSERT INTO ATTENDANCE(EMPLOYEE_ID,ATT_DATE,ATT_TIME,DIRECTION,COMPANY_ID,BRANCH_ID,REASON_CODE,ATT_DATE_TIME) " +
                                           "VALUES(@EmpId,@AttDate,@AttTime,@Direction,@ComId,@BranchId,@ReasonCode,?AttDateTime)";

                            mySqlCmd.CommandText = sMySqlString;

                            mySqlCmd.ExecuteNonQuery();
                        }

                    }
                }


                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Dispose();
                attendance = null;
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

    }
}
