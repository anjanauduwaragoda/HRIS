using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Attendance
{
    public class AttendanceInAndOutDataHandler : TemplateDataHandler
    {
        public Boolean Insert(DataTable dtAttendanceDate,string logId,string name)
        {
            Boolean status = false;
            MySqlTransaction mySqlTrans = null;
            string sMySqlString = "";

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                foreach (DataRow dr in dtAttendanceDate.Rows)
                {
                    if (dr["IS_EXCLUDE"].ToString().Trim() == Constants.CON_ROSTER_EXCLUDE_NO.ToString().Trim())
                    {
                        mySqlCmd.Parameters.Clear();

                        string employeeId = dr["EMPLOYEE_ID"].ToString();
                        string attDate = dr["ATT_DATE"].ToString();

                        string[] dateArr = attDate.Split('/');
                        attDate = dateArr[2] + "/" + dateArr[1] + "/" + dateArr[0];

                        string attTime = dr["ATT_TIME"].ToString();
                        string direction = dr["DIRECTION"].ToString();
                        if (direction == "IN")
                        {
                            direction = "1";
                        }
                        else if (direction == "OUT")
                        {
                            direction = "0";
                        }
                        string companyId = dr["COMPANY_ID"].ToString();
                        string branchId = dr["BRANCH_ID"].ToString();
                        string reasonCode = dr["REASON_CODE"].ToString();
                        string officerCode = dr["OFFICE_CODE"].ToString();
                        string statusCode = dr["STATUS_CODE"].ToString();
                        string reason = dr["REASON"].ToString();
                        string remarks = dr["REMARK"].ToString();

                        //check for duplicate entry
                        //Boolean existLog = IsExist(employeeId, attDate, direction);
                        //if (existLog == true)
                        //{
                            mySqlCmd.Parameters.Add(new MySqlParameter("@empid", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@sDate", attDate.Trim() == "" ? (object)DBNull.Value : attDate.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@sTime", attTime.Trim() == "" ? (object)DBNull.Value : attTime.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@sDirection", direction.Trim() == "" ? (object)DBNull.Value : direction.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@sCompID", companyId.Trim() == "" ? (object)DBNull.Value : companyId.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@sLocation", branchId.Trim() == "" ? (object)DBNull.Value : branchId.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@sReasonCode", reasonCode.Trim() == "" ? (object)DBNull.Value : reasonCode.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@sRecommendby", officerCode.Trim() == "" ? (object)DBNull.Value : officerCode.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@sStatusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@sReason", reason.Trim() == "" ? (object)DBNull.Value : reason.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@sRemark", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@logId", logId.Trim() == "" ? (object)DBNull.Value : logId.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@name", name.Trim() == "" ? (object)DBNull.Value : name.Trim()));

                        
                            sMySqlString = "INSERT INTO ATTENDANCE_LOG(EMPLOYEE_ID,ATT_DATE,ATT_TIME,DIRECTION,COMPANY_ID,BRANCH_ID,REASON,REASON_CODE,OFFICE_CODE,STATUS_CODE,REMARK,ATT_LOG_ID,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) " +
                                            "VALUES(@empid,@sDate,@sTime,@sDirection,@sCompID,@sLocation,@sReason,@sReasonCode,@sRecommendby,@sStatusCode,@sRemark,@logId,@name,NOW(),@name,NOW())";

                            mySqlCmd.CommandText = sMySqlString;
                            mySqlCmd.ExecuteNonQuery();

                        //}
                        //else
                        //{
                        //    return false;
                        //}
                        
                    }
                }
                mySqlTrans.Commit();

                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                

                status = true;
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                    mySqlCmd.Transaction.Rollback();
                }

            }
            finally
            {
                mySqlCon.Close();
            }
            return status;
        }

        public Boolean IsExist(string empid, string inDate, string direction)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                //string Qry = @"SELECT EXISTS(SELECT 1 FROM ATTENDANCE_LOG WHERE EMPLOYEE_ID = '" + empid + "' AND ATT_DATE = '" + inDate + "' AND DIRECTION = '" + direction + "');";

                string Qry = "SELECT count(*) FROM ATTENDANCE_LOG WHERE EMPLOYEE_ID = '" + empid + "' AND DIRECTION = '" + direction + "' AND ATT_DATE = ' " + inDate + "' AND STATUS_CODE != '8';";

                mySqlCmd.Parameters.Add(new MySqlParameter("empid", empid));
                mySqlCmd.Parameters.Add(new MySqlParameter("inDate", inDate));
                //mySqlCmd.Parameters.Add(new MySqlParameter("inTime", inTime));
                mySqlCmd.Parameters.Add(new MySqlParameter("direction", direction));

                mySqlCmd.CommandText = Qry;
                //mySqlCmd.CommandType = CommandType.StoredProcedure;
                string value = mySqlCmd.ExecuteScalar().ToString();

                if (value == "0")
                {
                    Status = true;
                }
                else if (value != "0")
                {
                    Status = false;
                }

            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                //if (mySqlCon.State == ConnectionState.Open)
                //{
                //    mySqlCon.Close();
                //}
            }
            return Status;
        }

        public DataTable getRecomendByName(string recomenderId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string Qry = @"SELECT EMAIL,KNOWN_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = '" + recomenderId + "';";
                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;
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

        public DataTable getRecordsByEmployee(string empName)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string qry = @"SELECT 
                                    EMPLOYEE_ID,ATT_DATE,DIRECTION
                                FROM
                                    ATTENDANCE_LOG
                                WHERE
                                    EMPLOYEE_ID = '" + empName + "' AND (STATUS_CODE = '0' OR STATUS_CODE = '8' OR STATUS_CODE = '9')";

                mySqlCmd.CommandText = qry;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;
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

        public String getName(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT KNOWN_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = '" + empId + "';";

                MySqlCommand cmd = new MySqlCommand(Qry,mySqlCon);
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

        public String getEmail(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT EMAIL FROM EMPLOYEE WHERE EMPLOYEE_ID = '" + empId + "';";

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

        public String getSerial()
        {
            SerialHandler serialHandler = new SerialHandler();
            string attLogId = "";

            try
            {
                mySqlCon.Open();
                attLogId = serialHandler.getserila(mySqlCon, Constants.ATT_LOG);
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }
            finally
            {
                serialHandler = null;
                mySqlCon.Close();
            }
            return attLogId;
        }

        public DataTable getByAttLogId(string logId)
        {
            try
            {
                 if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                 string Qry = @"SELECT 
                                    EMPLOYEE_ID,
                                    CONVERT( ATT_DATE , CHAR) AS ATT_DATE,ATT_TIME,
                                    CASE
                                        WHEN DIRECTION = '1' THEN 'IN'
                                        WHEN DIRECTION = '0' THEN 'OUT'
                                    END AS DIRECTION,
                                    STATUS_CODE,REMARK
                                FROM
                                    ATTENDANCE_LOG
                                WHERE
                                    ATT_LOG_ID = '" + logId + @"'
                                    AND STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + "'";
                 mySqlCmd.CommandText = Qry;
                 MySqlDataAdapter dtAdapter = new MySqlDataAdapter(mySqlCmd);
                 dtAdapter.Fill(dataTable);
                 return dataTable;
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

        public DataTable getObsoletedRecords(string logId)
        {
            try
            {
                DataTable dtTable = new DataTable();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    EMPLOYEE_ID,
                                    CONVERT( ATT_DATE , CHAR) AS ATT_DATE,ATT_TIME,
                                    CASE
                                        WHEN DIRECTION = '1' THEN 'IN'
                                        WHEN DIRECTION = '0' THEN 'OUT'
                                    END AS DIRECTION,
                                    STATUS_CODE,REMARK
                                FROM
                                    ATTENDANCE_LOG
                                WHERE
                                    ATT_LOG_ID = '" + logId + @"'
                                    AND STATUS_CODE = '" + Constants.STATUS_CANCEL_VALUE + "'";
                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dtAdapter = new MySqlDataAdapter(mySqlCmd);
                dtAdapter.Fill(dtTable);
                return dtTable;
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

        public Boolean updateApproved(DataTable attLog,string logId,string code)
        {
            Boolean status = false;
            MySqlTransaction mySqlTrans = null;
            int count = 0;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;
               
                foreach (DataRow dr in attLog.Rows)
                {
                    string employeeId = dr["EMPLOYEE_ID"].ToString();
                    // string attDate = dr["ATT_DATE"].ToString();
                    string attTime = dr["ATT_TIME"].ToString();
                    string direction = dr["DIRECTION"].ToString();
                    string attDate = (Convert.ToDateTime(dr["ATT_DATE"])).ToString("yyyy-MM-dd"); //.ToString("yyyy-MM-dd");

                    if (direction == Constants.CON_DIRECTION_IN_TEXT)
                    {
                        direction = Constants.CON_DIRECTION_IN_VALUE;
                    }
                    else
                    {
                        direction = Constants.CON_DIRECTION_OUT_VALUE;
                    }

                    Boolean isAvailable = checkForUpdate(logId, employeeId, attDate, direction);
                    
                    if (isAvailable)
                    {
                        string Qry = "UPDATE ATTENDANCE_LOG SET STATUS_CODE = '" + code + "' WHERE ATT_LOG_ID = '" + logId + "' AND STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' AND EMPLOYEE_ID = '" + employeeId + "' AND ATT_DATE = '" + attDate + "' AND ATT_TIME = '" + attTime + "' AND DIRECTION = '" + direction + "'";
                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();

                        if (code == Constants.STATUS_ACTIVE_VALUE)
                        {
                            string insertQry = @"INSERT INTO ATTENDANCE(EMPLOYEE_ID,ATT_DATE,ATT_TIME,DIRECTION,COMPANY_ID,BRANCH_ID,REASON_CODE,STATUS_CODE) 
                                                    SELECT EMPLOYEE_ID,ATT_DATE,ATT_TIME,DIRECTION,COMPANY_ID,BRANCH_ID,
                                                    CASE WHEN DIRECTION = '1' THEN  '0' 
                                                    WHEN DIRECTION = '0' THEN REASON_CODE 
                                                    END AS REASON_CODE,STATUS_CODE 
                                                    FROM ATTENDANCE_LOG 
                                                    WHERE ATT_LOG_ID = '" + logId +@"' 
                                                    AND EMPLOYEE_ID = '" + employeeId + @"' 
                                                    AND ATT_DATE = '" + attDate + @"' 
                                                    AND DIRECTION = '" + direction + "'";
                            mySqlCmd.CommandText = insertQry;
                            mySqlCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        //return false;
                        count = count + 1;
                    }

                }
                mySqlTrans.Commit();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                if (attLog.Rows.Count == count)
                {
                    status = false;
                }
                else
                {
                    status = true;
                }
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return status;
        }

        public Boolean checkForUpdate(string logId,string employeeId, string attDate,string direction)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string Qry = "SELECT count(*) FROM ATTENDANCE_LOG WHERE  ATT_LOG_ID = '" + logId + "' AND STATUS_CODE != '" + Constants.STATUS_INACTIVE_VALUE + "' AND EMPLOYEE_ID = '" + employeeId + "' AND ATT_DATE = '" + attDate + "' AND DIRECTION = '" + direction +"' ;";

                mySqlCmd.Parameters.Add(new MySqlParameter("logId", logId));

                mySqlCmd.CommandText = Qry;
                string value = mySqlCmd.ExecuteScalar().ToString();

                if (value == "0")
                {
                    Status = true;
                }
                else if (value != "0")
                {
                    Status = false;
                }

            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
            return Status;
        }

        public String getCompanyName(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT c.COMP_NAME FROM EMPLOYEE e,COMPANY c WHERE c.COMPANY_ID = e.COMPANY_ID AND EMPLOYEE_ID = '" + empId + "';";

                MySqlCommand cmd = new MySqlCommand(Qry, mySqlCon);
                String rdr = Convert.ToString(cmd.ExecuteScalar());

                return rdr;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public string getSupervisor(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT REPORT_TO_1 FROM EMPLOYEE WHERE EMPLOYEE_ID = '" + empId + "';";

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

        public String getCompanyId(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT COMPANY_ID FROM EMPLOYEE WHERE  EMPLOYEE_ID = '" + empId + "';";

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

        public String getEmpName(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT KNOWN_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = '" + empId + "';";

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

        public DataTable populateAttendance(string mEmpID, DateTime mExpdate)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT  ATT_LOG_ID,EMPLOYEE_ID,DATE_FORMAT(ATT_DATE,'%Y/%m/%d') as ATT_DATE ,ATT_TIME," + 
                            " CASE " + 
                            " when DIRECTION = '1' then 'IN' " +
                            " when DIRECTION = '0' then 'OUT'" +
                            " End as DIRECTION " +
                            ",REASON, " +
                            " CASE " +
                            " when STATUS_CODE='0' then 'Pending' " +
                            " when STATUS_CODE='1' then 'Approved' " +
                            " when STATUS_CODE='9' then 'Rejected' " +
                            " End as STATUS " +
                            " FROM ATTENDANCE_LOG " +
                            " WHERE EMPLOYEE_ID = '" + mEmpID.ToString() + "' " +
                            " AND ATT_DATE >= '" + mExpdate.ToString("yyyy/MM/dd") + "' and STATUS_CODE<>'" + Constants.STATUS_CANCEL_VALUE + "' order by ATT_DATE,ATT_TIME";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

                if (sStatusCode == Constants.STATUS_ACTIVE_VALUE)
                {
                    sMySqlString = "INSERT INTO ATTENDANCE(EMPLOYEE_ID,ATT_DATE,ATT_TIME,DIRECTION,COMPANY_ID,BRANCH_ID,REASON_CODE,STATUS_CODE) " +
                                "VALUES(@empid,@sDate,@sTime,@sDirection,@sCompID,@sLocation,@sReasonCode,@sStatusCode)";
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

        public Boolean updateCancel(DataTable dtCancel,string name)
        { 
            bool status = false;
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;


                foreach (DataRow dr in dtCancel.Rows)
                {
                    string empId = dr["EMPLOYEE_ID"].ToString();
                    string dutyDate = dr["ATT_DATE"].ToString();
                    string intime = dr["ATT_TIME"].ToString();

                    string direction = dr["DIRECTION"].ToString();
                    string logid = dr["ATT_LOG_ID"].ToString();

                    if (direction == "IN")
                    {
                        direction = "1";
                    }
                    else if (direction == "OUT")
                    {
                        direction = "0";
                    }
                    string statusCode = Constants.STATUS_CANCEL_VALUE;

                    //Check availability
                    string approved_status = activeStatus(empId, dutyDate, direction, intime);

                    mySqlCmd.Parameters.Clear();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId.Trim() == "" ? (object)DBNull.Value : empId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@dutyDate", dutyDate.Trim() == "" ? (object)DBNull.Value : dutyDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@direction", direction.Trim() == "" ? (object)DBNull.Value : direction.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@stime", intime.Trim() == "" ? (object)DBNull.Value : intime.Trim()));
                    
                    mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ename", name.Trim() == "" ? (object)DBNull.Value : name.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@logId", logid.Trim() == "" ? (object)DBNull.Value : logid.Trim()));

                    if(approved_status != "0")
                    {
                        string Qry = @"UPDATE ATTENDANCE_LOG 
                                    SET 
                                        STATUS_CODE = '" + Constants.STATUS_CANCEL_VALUE + @"',
                                        MODIFIED_BY = @ename,
                                        MODIFIED_DATE = NOW() 
                                    WHERE
                                        EMPLOYEE_ID = @empId
                                        AND ATT_DATE = @dutyDate
                                        AND DIRECTION = @direction
                                        AND ATT_TIME = @stime
                                        AND ATT_LOG_ID = @logId 
                                        AND STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "'";

                        mySqlCmd.Transaction = mySqlTrans;
                        mySqlCmd.CommandText = Qry;

                        mySqlCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        return false;
                    }
                }
                mySqlTrans.Commit();
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                status = true;
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

            }
            return status;
        }

        public string activeStatus(string empId, string inOut, string direstion, string intime)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT count(*) 
                                FROM ATTENDANCE_LOG 
                                WHERE EMPLOYEE_ID = '" + empId + "' AND DIRECTION = '" + direstion + "' AND ATT_DATE = ' " + inOut + "' AND ATT_TIME = '" + intime + "' AND STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "'";

                MySqlCommand cmd = new MySqlCommand(Qry, mySqlCon);
                String rdr = Convert.ToString(cmd.ExecuteScalar());

                return rdr;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public string activeEmployee(string empId)
        {
            //bool status = false;
            String rdr = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT s.DESCRIPTION FROM EMPLOYEE e,EMPLOYEE_STATUS s WHERE e.EMPLOYEE_STATUS = s.STATUS_CODE AND EMPLOYEE_ID ='" + empId + "';";

                MySqlCommand cmd = new MySqlCommand(Qry, mySqlCon);
                 rdr = Convert.ToString(cmd.ExecuteScalar());
                //if (rdr == "S001")
                //{
                //    status = true;
                //}
                //else
                //{
                //    status = false;
                //}
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

            }
            return rdr;
        }

        public String isRoster(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT IS_ROSTER FROM EMPLOYEE WHERE EMPLOYEE_ID = '" + empId + "';";

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

        public DataTable roster(string empId , string rDate)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    r.ROSTR_ID, r.FROM_TIME, r.TO_TIME,t.DESCRIPTION
                                FROM
                                    ROSTER r,EMPLOYEE_ROSTER_DATE e,ROSTER_TYPES t
                                WHERE 
	                                r.ROSTR_ID = e.ROSTR_ID
	                                AND e.EMPLOYEE_ID = '" + empId + @"'
	                                AND DUTY_DATE = '" + rDate + @"' 
	                                AND r.ROSTER_TYPE = t.ROSTER_TYPE
                                    AND e.STATUS_CODE = '1';";
                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dtAdapter = new MySqlDataAdapter(mySqlCmd);
                dtAdapter.Fill(dataTable);
                return dataTable;
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

        public DataTable rosterCompanyWorkingTime(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    c.WORK_HOURS_START, c.WORK_HOURS_END,c.SATWORK_HOURS_START,c.SATWORK_HOURS_END
                                FROM
                                    COMPANY c,
                                    EMPLOYEE e
                                WHERE
                                    e.COMPANY_ID = c.COMPANY_ID
                                        AND EMPLOYEE_ID = '" + empId + "';";
                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dtAdapter = new MySqlDataAdapter(mySqlCmd);
                dtAdapter.Fill(dataTable);
                return dataTable;
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

        public DataTable reguler(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    c.COMPANY_ID,c.WORK_HOURS_START, c.WORK_HOURS_END,c.SATWORK_HOURS_START,c.SATWORK_HOURS_END
                                FROM
                                    COMPANY c,
                                    EMPLOYEE e
                                WHERE
                                    e.COMPANY_ID = c.COMPANY_ID
	                                AND e.EMPLOYEE_ID = '" + empId + "';";
                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dtAdapter = new MySqlDataAdapter(mySqlCmd);
                dtAdapter.Fill(dataTable);
                return dataTable;
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

        public DataTable attendance(string empId,string inDate)
        {
            try
            {
                dataTable.Rows.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                   CONVERT(ATT_DATE,CHAR) AS ATT_DATE, ATT_TIME
                                FROM
                                   ATTENDANCE
                                WHERE
                                  EMPLOYEE_ID = '" + empId + @"'
                                  AND ATT_DATE = '" + inDate + "'";
                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dtAdapter = new MySqlDataAdapter(mySqlCmd);
                dtAdapter.Fill(dataTable);
                return dataTable;
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

        public DataTable rosterInOut(string rosterId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    r.FROM_TIME, r.TO_TIME, t.ROSTER_TYPE
                                FROM
                                    ROSTER r,ROSTER_TYPES t
                                WHERE
                                    r.ROSTR_ID = '" + rosterId + @"' 
	                                AND r.ROSTER_TYPE = t.ROSTER_TYPE;";
                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dtAdapter = new MySqlDataAdapter(mySqlCmd);
                dtAdapter.Fill(dataTable);
                return dataTable;
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

        public Boolean isRecordExist(string employeeId, string sAttDate, string sAttTime)
        {
            Boolean recordExist = false;

            string commandSql = "";
            //int iCount = 0;

            string[] dateArr = sAttDate.Split('/');
            sAttDate = dateArr[2] + "-" + dateArr[1] + "-" + dateArr[0];

            try
            {
                //commandSql = " select count(*) from ATTENDANCE_LOG " +
                //             " where EMPLOYEE_ID = '" + employeeId.Trim() + "' AND DATE(ATT_DATE) = DATE('" + sAttDate.Trim() + "') " +
                //             " AND Time(ATT_TIME) = Time('" + sAttTime.Trim() + "')";

                commandSql = @"SELECT 
                                    count(*)
                                FROM
                                    ATTENDANCE_LOG
                                WHERE
                                    EMPLOYEE_ID = '"+  employeeId.Trim() +@"'
                                        AND DATE(ATT_DATE) = DATE('" + sAttDate.Trim() +@"')
                                        AND TIME(ATT_TIME) = TIME('"+ sAttTime.Trim() +@"')
		                                AND STATUS_CODE IN(0,1)";

                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
               
                 mySqlCon.Open();
                

               // mySqlCmd.CommandText = commandSql;

                DataTable datatabe = new DataTable();

                MySqlDataAdapter dt = new MySqlDataAdapter(commandSql, mySqlCon);
                dt.Fill(datatabe);

                //if (mySqlCmd.ExecuteScalar() != null)
                //{
                //    iCount = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());
                //}

                //mySqlCon.Close();
                //iCount = datatabe.Rows.Count;

                if (Int32.Parse(datatabe.Rows[0][0].ToString())> 0) { recordExist = true; }

                return recordExist;

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
