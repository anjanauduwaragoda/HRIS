using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Common;
using System.Data;

namespace DataHandler.Attendance
{
    public class AttendanceReconcileDataHandler:TemplateDataHandler
    {
        public DataTable reconcileAttendance(string empId, string frmDate, string toDate)
        {
            //frmDate = CommonUtils.dateFormatChange(frmDate);
            //toDate = CommonUtils.dateFormatChange(toDate);

            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = @"SELECT 
                                            CONVERT(ATT_DATE,CHAR) AS ATT_DATE,
                                            ATT_TIME,
                                            CASE
                                                WHEN DIRECTION = '1' THEN 'IN'
                                                WHEN DIRECTION = '0' THEN 'OUT'
                                            END AS DIRECTION,
                                            CASE
		                                        WHEN REASON_CODE = '0' THEN '' 
		                                        WHEN REASON_CODE = '1' THEN 'Day off'
                                                WHEN REASON_CODE = '2' THEN 'Official Reason'
		                                        WHEN REASON_CODE = '3' THEN 'Personal Reason'
	                                        END AS REASON_CODE
                                        FROM
                                            ATTENDANCE
                                        WHERE
                                            EMPLOYEE_ID = '" + empId.Trim() + "' AND ATT_DATE >= ' " + frmDate.Trim() + @"'
                                                AND ATT_DATE <= '" + toDate.Trim() + "';";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool removeAttendance(string empId, string attDate, string attTime, string direction,string remarks, string user)
        {
            Boolean status = false;
            MySqlTransaction mySqlTrans = null;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;
                //string[] dateArr = attDate.Split('-');
                //attDate = dateArr[2] + "-" + dateArr[1] + "-" + dateArr[0];

                if (direction == "IN")
                {
                    direction = "1";
                }
                else if (direction == "OUT")
                {
                    direction = "0";
                }

                //Delete record from Attendance table
                string Qry = @"DELETE FROM ATTENDANCE 
                                WHERE
                                    EMPLOYEE_ID = '" + empId + @"' AND ATT_DATE = '" + attDate + @"'
                                    AND ATT_TIME = '" + attTime + @"'
                                    AND DIRECTION = '" + direction + "';";
                mySqlCmd.CommandText = Qry;
                mySqlCmd.ExecuteNonQuery();

                mySqlCmd.Parameters.Add(new MySqlParameter("@empid", empId.Trim() == "" ? (object)DBNull.Value : empId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDate", attDate.Trim() == "" ? (object)DBNull.Value : attDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sTime", attTime.Trim() == "" ? (object)DBNull.Value : attTime.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDirection", direction.Trim() == "" ? (object)DBNull.Value : direction.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sRemarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sUser", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));
                            
                //Insert DELETED record to REMOVED_ATTENDANCE Table
                string logQry = @"INSERT INTO REMOVED_ATTENDANCE(EMPLOYEE_ID,ATT_DATE,ATT_TIME,DIRECTION,REMOVED_REASON,REMOVED_BY,REMOVED_DATE) 
                                    VALUES(@empid,@sDate,@sTime,@sDirection,@sRemarks,@sUser,NOW())";
                mySqlCmd.CommandText = logQry;
                mySqlCmd.ExecuteNonQuery();

                //Update ATTENDANCE_LOG table , deteted record to OBSOLETE status
                string updateAtt = @"UPDATE ATTENDANCE_LOG 
                                    SET 
                                        STATUS_CODE = '" + Constants.STATUS_CANCEL_VALUE + @"',
                                        MODIFIED_BY = @sUser,
                                        MODIFIED_DATE = NOW() 
                                    WHERE
                                        EMPLOYEE_ID = @empId
                                        AND ATT_DATE = @sDate
                                        AND DIRECTION = @sDirection
                                        AND ATT_TIME = @sTime
                                        AND STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "'";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = updateAtt;

                mySqlCmd.ExecuteNonQuery();

                status = true;
            }
            catch(Exception ex)
            {
               // return ex;
            }
            //finally
            //{
            //    mySqlCmd.Parameters.Clear();
            //    if (mySqlCon.State == ConnectionState.Open)
            //    {
            //        mySqlCon.Close();
            //    }
            //}

            mySqlTrans.Commit();
            mySqlTrans.Dispose();
            mySqlCmd.Dispose();
            mySqlCon.Close();

            return status;
        }

        public string returnEmpName(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT FULL_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = '" + empId + "';";

                MySqlCommand cmd = new MySqlCommand(Qry, mySqlCon);
                String rdr = Convert.ToString(cmd.ExecuteScalar());

                return rdr;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

    }
}
