
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.PerformanceManagement
{
    public class TaskDashboardDataHandler : TemplateDataHandler
    {
        public DataTable getEmployeeTask(string empId,string tyear)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"
                                SELECT 
                                    g.GOAL_ID,
                                    t.TASK_ID,
                                    t.TASK_NAME,
                                    t.DESCRIPTION,
                                    CONVERT( t.PLAN_START_DATE , CHAR) AS PLAN_START_DATE,
                                    CONVERT( t.TARGET_DATE , CHAR) AS TARGET_DATE,
                                CASE
                                    WHEN t.IS_AGREE = '1' then 'Agreed'
                                    WHEN t.IS_AGREE = '0' then 'Disagreed'
                                End as IS_AGREE,t.REASON,
                                CASE
                                    WHEN t.SUPERVISOR_AGREE = '1' then 'Confirmed'
                                    WHEN t.SUPERVISOR_AGREE = '0' then 'Disagreed'
                                End as SUPERVISOR_AGREE,t.SUPERVISOR_REASON
                                FROM
                                    TASK t,
                                    EMPLOYEE_GOALS g
                                WHERE
                                    g.GOAL_ID = t.GOAL_ID
                                        AND g.EMPLOYEE_ID = '" + empId +@"'
                                        AND t.TASK_YEAR = '"+ tyear +@"'
                                ORDER BY t.TARGET_DATE;
                            ";
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

        public String getGoal(string goalId)
        {
            string goalName = "";

            mySqlCmd.CommandText = @"
                                        SELECT 
                                            GOAL_AREA 
                                        FROM 
                                            EMPLOYEE_GOALS 
                                        WHERE 
                                            GOAL_ID = '" + goalId + @"';
                                    ";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    goalName = mySqlCmd.ExecuteScalar().ToString();
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

            return goalName;
        }

        public bool Update(string tskId,string status,string reason,string user)
        {
            bool isInsert = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@tskId", tskId.Trim() == "" ? (object)DBNull.Value : tskId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@reason", reason.Trim() == "" ? (object)DBNull.Value : reason.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));

                sMySqlString = @"
                                    UPDATE 
                                        TASK SET 
                                        IS_AGREE = @status,
                                        REASON = @reason,
                                        APPROVED_BY = @user
                                    WHERE 
                                        TASK_ID = @tskId; 
                                ";


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

        public String isReporter(string sSubordinate,string sSupervisor)
        {
            string id = "";

            mySqlCmd.CommandText = @"
                                    SELECT 
                                        REPORT_TO_1
                                    FROM
                                        EMPLOYEE
                                    WHERE 
                                        '" + sSupervisor + @"' IN (REPORT_TO_1)
                                            AND EMPLOYEE_ID = '" + sSubordinate + @"';";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    id = mySqlCmd.ExecuteScalar().ToString();
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

            return id;
        }

        public bool UpdateIsAgree(string tskId, string status, string reason, string user)
        {
            bool isInsert = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@tskId", tskId.Trim() == "" ? (object)DBNull.Value : tskId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@reason", reason.Trim() == "" ? (object)DBNull.Value : reason.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));

                sMySqlString = @"
                                UPDATE TASK SET 
                                    SUPERVISOR_AGREE = @status,
                                    SUPERVISOR_REASON = @reason,
                                    RECOMEND_BY = @user
                                WHERE 
                                    TASK_ID = @tskId; 
                                ";


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

        public DataTable getEmployeeTaskStatus(string empId, string tyear,string empstatus)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"
                                SELECT 
                                    g.GOAL_ID,
                                    t.TASK_ID,
                                    t.TASK_NAME,
                                    t.DESCRIPTION,
                                    CONVERT( t.PLAN_START_DATE , CHAR) AS PLAN_START_DATE,
                                    CONVERT( t.TARGET_DATE , CHAR) AS TARGET_DATE,
                                CASE
                                    WHEN t.IS_AGREE = '1' then 'Agreed'
                                    WHEN t.IS_AGREE = '0' then 'Disagreed'
                                End as IS_AGREE,t.REASON,
                                CASE
                                    WHEN t.SUPERVISOR_AGREE = '1' then 'Confirmed'
                                    WHEN t.SUPERVISOR_AGREE = '0' then 'Disagreed'
                                End as SUPERVISOR_AGREE,t.SUPERVISOR_REASON
                                FROM
                                    TASK t,
                                    EMPLOYEE_GOALS g
                                WHERE
                                    g.GOAL_ID = t.GOAL_ID
                                        AND g.EMPLOYEE_ID = '" + empId + @"'
                                        AND t.TASK_YEAR = '" + tyear + @"' AND t.IS_AGREE = '"+ empstatus +@"'
                                ORDER BY t.TARGET_DATE;";
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

        public DataTable getSupervisorTaskStatus(string empId, string tyear, string empstatus)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"
                                SELECT 
                                    g.GOAL_ID,
                                    t.TASK_ID,
                                    t.TASK_NAME,
                                    t.DESCRIPTION,
                                    CONVERT( t.PLAN_START_DATE , CHAR) AS PLAN_START_DATE,
                                    CONVERT( t.TARGET_DATE , CHAR) AS TARGET_DATE,
                                CASE
                                    WHEN t.IS_AGREE = '1' then 'Agreed'
                                    WHEN t.IS_AGREE = '0' then 'Disagreed'
                                End as IS_AGREE,t.REASON,
                                CASE
                                    WHEN t.SUPERVISOR_AGREE = '1' then 'Confirmed'
                                    WHEN t.SUPERVISOR_AGREE = '0' then 'Disagreed'
                                End as SUPERVISOR_AGREE,t.SUPERVISOR_REASON
                                FROM
                                    TASK t,
                                    EMPLOYEE_GOALS g
                                WHERE
                                    g.GOAL_ID = t.GOAL_ID
                                        AND g.EMPLOYEE_ID = '" + empId + @"'
                                        AND t.TASK_YEAR = '" + tyear + @"' AND t.SUPERVISOR_AGREE = '" + empstatus + @"'
                                ORDER BY t.TARGET_DATE;";
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

