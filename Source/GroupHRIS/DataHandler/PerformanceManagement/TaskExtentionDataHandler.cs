using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class TaskExtentionDataHandler : TemplateDataHandler
    {

        public bool Insert(string tskId,string exDate,string exReason,string exStatus , string user)
        {
            bool isInsert = false;
            string sMySqlString = "";
            exDate = CommonUtils.dateFormatChange(exDate);
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@tskId", tskId.Trim() == "" ? (object)DBNull.Value : tskId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@exDate", exDate.Trim() == "" ? (object)DBNull.Value : exDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@exReason", exReason.Trim() == "" ? (object)DBNull.Value : exReason.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@exStatus", exStatus.Trim() == "" ? (object)DBNull.Value : exStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@exuser", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));

                sMySqlString = @"INSERT INTO TASK_EXTENTIONS (TASK_ID,EXTENDED_DATE,REASON,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                    VALUES(@tskId,@exDate,@exReason,@exStatus,@exuser,now(),@exuser,now())";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                sMySqlString = "UPDATE TASK SET EXTENDED_TARGET_DATE = @exDate WHERE TASK_ID = @tskId; ";
                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isInsert = true;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return isInsert;
        }

        public bool Update(string taskId,string taskExId,string exDate,string exReason, string status, string logUser)
        {
            bool isUpdate = false;
            string sMySqlString = "";
            exDate = CommonUtils.dateFormatChange(exDate);
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Parameters.Add(new MySqlParameter("@taskExId", taskExId.Trim() == "" ? (object)DBNull.Value : taskExId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@exDate", exDate.Trim() == "" ? (object)DBNull.Value : exDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@exReason", exReason.Trim() == "" ? (object)DBNull.Value : exReason.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@logUser", logUser.Trim() == "" ? (object)DBNull.Value : logUser.Trim()));

                sMySqlString = @"UPDATE TASK_EXTENTIONS SET 
                                    EXTENDED_DATE = @exDate,
                                    REASON = @exReason,
                                    STATUS_CODE = @status ,
                                    MODIFIED_BY = @logUser,
                                    MODIFIED_DATE = now()
                                    WHERE TASK_EXTENTION_ID = @taskExId; ";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                sMySqlString = "UPDATE TASK SET EXTENDED_TARGET_DATE = @exDate WHERE TASK_ID = '" + taskId + "'; ";
                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isUpdate = true;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }

            return isUpdate;
        }

        public DataTable GetTasksExtentions(string empId,string yer)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    t.TASK_ID,
                                    CONVERT( t.TARGET_DATE , CHAR) AS TARGET_DATE,
                                    t.TASK_NAME,
                                    CONVERT( t.EXTENDED_TARGET_DATE , CHAR) AS EXTENDED_DATE,t.TOTAL_COMPLETION,
                                    CONVERT(t.PLAN_START_DATE,CHAR) AS PLAN_START_DATE,
                                    CONVERT(t.ACTUAL_START_DATE,CHAR) AS ACTUAL_START_DATE,
                                    CASE
                                        WHEN t.STATUS_CODE = '1' then 'Active'
                                        WHEN t.STATUS_CODE = '0' then 'Inactive'
                                    End as STATUS_CODE
                                FROM
                                    TASK t,
                                    EMPLOYEE_GOALS eg
                                WHERE
                                     t.STATUS_CODE = '1' AND t.IS_AGREE = '1' AND t.SUPERVISOR_AGREE = '1' AND eg.SUPERVISOR_AGREE = '1'
                                        AND eg.EMPLOYEE_ID = '" + empId + "' AND t.TASK_YEAR = '" + yer + "' AND t.GOAL_ID = eg.GOAL_ID group by t.TASK_ID;";

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

        public DataTable getTaskListForSelectedEmployee(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "";
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

        public DataTable getAllTaskList(string empid)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    t.TASK_ID, t.TASK_NAME
                                FROM
                                    TASK t,
                                    EMPLOYEE_GOALS eg
                                WHERE
                                    t.STATUS_CODE = '1' 
                                        AND t.GOAL_ID = eg.GOAL_ID
                                        AND eg.EMPLOYEE_ID = '" + empid + "';";
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

        public String getGoal(string tskId)
        {
            string taskName = "";

            mySqlCmd.Parameters.Add(new MySqlParameter("@tskId", tskId.Trim() == "" ? (object)DBNull.Value : tskId.Trim()));

            mySqlCmd.CommandText = @"SELECT eg.GOAL_AREA 
                                        FROM EMPLOYEE_GOALS eg,TASK t,TASK_EXTENTIONS te
                                        WHERE te.TASK_ID = t.TASK_ID AND te.TASK_ID = @tskId AND t.GOAL_ID = eg.GOAL_ID AND eg.STATUS_CODE = '1';";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    taskName = mySqlCmd.ExecuteScalar().ToString();
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

            return taskName;
        }

        public String getTargetDate(string empId, string tskId)
        {
            string taskName = "";

            mySqlCmd.CommandText = @"SELECT 
                                        CONVERT( t.TARGET_DATE , CHAR) AS TARGET_DATE
                                    FROM
                                        EMPLOYEE_GOALS eg,
                                        TASK t
                                    WHERE
                                         eg.GOAL_ID = t.GOAL_ID 
                                            AND eg.EMPLOYEE_ID = '" + empId + "' AND t.TASK_ID = '" + tskId + "';";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    taskName = mySqlCmd.ExecuteScalar().ToString();
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

            return taskName;
        }

        public String getMaxTargetDate(string empId, string tskId)
        {
            string taskName = "";

            mySqlCmd.CommandText = @"SELECT 
                                         MAX(CONVERT( te.EXTENDED_DATE , CHAR)) AS EXTENDED_DATE
                                    FROM
                                        TASK_EXTENTIONS te,
                                        EMPLOYEE_GOALS eg,
                                        TASK t
                                    WHERE 
                                        eg.GOAL_ID = t.GOAL_ID AND te.STATUS_CODE = '1'
                                            AND eg.EMPLOYEE_ID = '" + empId +@"' AND t.TASK_ID = te.TASK_ID 
                                            AND t.TASK_ID = '"+ tskId +"';";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    taskName = mySqlCmd.ExecuteScalar().ToString();
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

            return taskName;
        }

        public String getSecondMaxTargetDate(string empId, string tskId)
        {
            string taskName = "";

            mySqlCmd.CommandText = @"SELECT 
                                        MAX(CONVERT( te.EXTENDED_DATE , CHAR)) AS EXTENDED_DATE
                                    FROM
                                        TASK_EXTENTIONS te,
                                        EMPLOYEE_GOALS eg,
                                        TASK t
                                    WHERE
                                        eg.GOAL_ID = t.GOAL_ID
                                            AND eg.EMPLOYEE_ID = '"+ empId + @"'
                                            AND t.TASK_ID = te.TASK_ID AND te.STATUS_CODE = '1'
                                            AND t.TASK_ID = '" + tskId +@"'
                                            AND te.EXTENDED_DATE NOT IN (SELECT 
                                                MAX(CONVERT( te.EXTENDED_DATE , CHAR)) AS EXTENDED_DATE
                                            FROM
                                                TASK_EXTENTIONS te,
                                                EMPLOYEE_GOALS eg,
                                                TASK t
                                            WHERE
                                                eg.GOAL_ID = t.GOAL_ID
                                                    AND eg.EMPLOYEE_ID = '"+ empId + @"'
                                                    AND t.TASK_ID = te.TASK_ID AND te.STATUS_CODE = '1'
                                                    AND t.TASK_ID = '" + tskId + "');";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    taskName = mySqlCmd.ExecuteScalar().ToString();
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

            return taskName;
        }

        public DataTable getTaskExtentionList(string tskId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    te.TASK_EXTENTION_ID, te.TASK_ID,t.TASK_NAME,CONVERT(t.TARGET_DATE,CHAR) AS TARGET_DATE,CONVERT(te.EXTENDED_DATE,CHAR) AS EXTENDED_DATE,te.REASON,te.STATUS_CODE
                                FROM
                                    TASK_EXTENTIONS te,TASK t
                                WHERE 
                                    te.TASK_ID = '" + tskId + "' AND t.TASK_ID = te.TASK_ID AND te.STATUS_CODE = '1' AND t.IS_AGREE = '1';";
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

    }
}
