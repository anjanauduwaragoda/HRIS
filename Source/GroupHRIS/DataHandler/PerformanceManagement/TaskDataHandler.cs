using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class TaskDataHandler :TemplateDataHandler
    {
        public DataTable getGoaAlllList()
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "SELECT GOAL_ID,GOAL_AREA FROM EMPLOYEE_GOALS WHERE STATUS_CODE = '1';";
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

        public DataTable getGoalList(string empId, string gyear)
        {
            try
            {
                dataTable.Clear();    
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "SELECT GOAL_ID,GOAL_AREA FROM EMPLOYEE_GOALS WHERE STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "' AND EMPLOYEE_ID = '" + empId + "' AND YEAR_OF_GOAL = '" + gyear + "' AND SUPERVISOR_AGREE = '" + Constants.CON_ACTIVE_STATUS + "' AND EMPLOYEE_AGREE = '" + Constants.CON_ACTIVE_STATUS + "';";
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

        public bool Insert(string taskName,string goalId,string tYear,string tDescription,string tDate,string tStatus,string user,string plnDate,string actDate,string remarks)
        {
            bool isInsert = false;
            string sMySqlString = "";
            
            tDate = CommonUtils.dateFormatChange(tDate);
            plnDate = CommonUtils.dateFormatChange(plnDate);
            if (actDate != "")
            {
                actDate = CommonUtils.dateFormatChange(actDate);
            }
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler nserialcode = new SerialHandler();
                string sTASK_ID = nserialcode.getserila(mySqlCon, "T");

                mySqlCmd.Parameters.Add(new MySqlParameter("@sTASK_ID", sTASK_ID.Trim() == "" ? (object)DBNull.Value : sTASK_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@taskName", taskName.Trim() == "" ? (object)DBNull.Value : taskName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@goalId", goalId.Trim() == "" ? (object)DBNull.Value : goalId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@tYear", tYear.Trim() == "" ? (object)DBNull.Value : tYear.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@tDescription", tDescription.Trim() == "" ? (object)DBNull.Value : tDescription.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@tDate", tDate.Trim() == "" ? (object)DBNull.Value : tDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@tStatus", tStatus.Trim() == "" ? (object)DBNull.Value : tStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@plnDate", plnDate.Trim() == "" ? (object)DBNull.Value : plnDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@actDate", actDate.Trim() == "" ? (object)DBNull.Value : actDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));

                sMySqlString = @"INSERT INTO TASK (TASK_ID,GOAL_ID,TASK_YEAR,TASK_NAME,DESCRIPTION,PLAN_START_DATE,ACTUAL_START_DATE,TARGET_DATE,TOTAL_COMPLETION,STATUS_CODE,REMARKS,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                    VALUES(@sTASK_ID,@goalId,@tYear,@taskName,@tDescription,@plnDate,@actDate,@tDate,'"+ 0 +"',@tStatus,@remarks,@user,now(),@user,now())";


                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

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

        public bool Update(string taskId,string taskName, string goalName,string year,string description,string tDate,string status,string logUser,string actualStDate,string remarks)
        {
            bool isUpdate = false;
            string sMySqlString = "";
            tDate = CommonUtils.dateFormatChange(tDate);
            actualStDate = CommonUtils.dateFormatChange(actualStDate);
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Parameters.Add(new MySqlParameter("@taskId", taskId.Trim() == "" ? (object)DBNull.Value : taskId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@taskName", taskName.Trim() == "" ? (object)DBNull.Value : taskName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@goalName", goalName.Trim() == "" ? (object)DBNull.Value : goalName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@year", year.Trim() == "" ? (object)DBNull.Value : year.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@tDate", tDate.Trim() == "" ? (object)DBNull.Value : tDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@logUser", logUser.Trim() == "" ? (object)DBNull.Value : logUser.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@actualStDate", actualStDate.Trim() == "" ? (object)DBNull.Value : actualStDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));

                sMySqlString = @"UPDATE TASK SET GOAL_ID =  @goalName ,
                                TASK_YEAR = @year,
                                TASK_NAME = @taskName,
                                DESCRIPTION = @description, 
                                TARGET_DATE =  @tDate, 
                                STATUS_CODE =  @status, 
                                MODIFIED_BY =  @logUser, 
                                MODIFIED_DATE = now(),
                                ACTUAL_START_DATE =  @actualStDate,
                                REMARKS = @remarks
                                WHERE TASK_ID = @taskId; ";

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

        public DataTable GetTasks(string empId, string tyear)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT t.TASK_NAME,g.GOAL_ID,g.GOAL_AREA,t.TASK_YEAR,CONVERT(t.TARGET_DATE,CHAR) AS TARGET_DATE,CONVERT(t.EXTENDED_TARGET_DATE,CHAR) AS EXTENDED_TARGET_DATE,t.TOTAL_COMPLETION,t.DESCRIPTION,CONVERT(t.PLAN_START_DATE,CHAR) AS PLAN_START_DATE,CONVERT(t.ACTUAL_START_DATE,CHAR) AS ACTUAL_START_DATE,t.REMARKS, 
                                CASE
                                    WHEN t.STATUS_CODE = '1' then 'Active'
                                    WHEN t.STATUS_CODE = '0' then 'Inactive'
                                End as STATUS_CODE,
                                 CASE
                                    WHEN t.IS_AGREE = '1' then 'Agree'
                                    WHEN t.IS_AGREE = '0' then 'Disagree'
                                    ELSE 'Pending'
                                End as IS_AGREE,t.TASK_ID,
                                CASE
                                    WHEN t.SUPERVISOR_AGREE = '1' then 'Approve'
                                    WHEN t.SUPERVISOR_AGREE = '0' then 'Discard'
                                    ELSE 'Pending'
                                End as SUPERVISOR_AGREE
                                FROM TASK t,EMPLOYEE_GOALS g
                                WHERE g.GOAL_ID = t.GOAL_ID AND g.EMPLOYEE_ID = '" + empId + "' AND t.TASK_YEAR = '" + tyear + "' ORDER BY t.TARGET_DATE; ";
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

        public DataTable GetTasksForSelectedGoal(string goalId, string empId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT g.GOAL_ID,t.TASK_NAME,g.GOAL_AREA,t.TASK_YEAR,CONVERT(t.TARGET_DATE,CHAR) AS TARGET_DATE,CONVERT(t.EXTENDED_TARGET_DATE,CHAR) AS EXTENDED_TARGET_DATE,t.TOTAL_COMPLETION,t.DESCRIPTION,CONVERT(t.PLAN_START_DATE,CHAR) AS PLAN_START_DATE,CONVERT(t.ACTUAL_START_DATE,CHAR) AS ACTUAL_START_DATE,t.REMARKS,
                                CASE
                                    WHEN t.STATUS_CODE = '1' then 'Active'
                                    WHEN t.STATUS_CODE = '0' then 'Inactive'
                                End as STATUS_CODE,
                                CASE
                                    WHEN t.IS_AGREE = '1' then 'Agree'
                                    WHEN t.IS_AGREE = '0' then 'Disagree'
                                End as IS_AGREE,t.TASK_ID,
                                CASE
                                    WHEN t.SUPERVISOR_AGREE = '1' then 'Approve'
                                    WHEN t.SUPERVISOR_AGREE = '0' then 'Discard'
                                End as SUPERVISOR_AGREE
                                FROM TASK t,EMPLOYEE_GOALS g
                                WHERE g.GOAL_ID = t.GOAL_ID AND t.GOAL_ID = '" + goalId + "' AND g.EMPLOYEE_ID = '" + empId + "'; ";
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
