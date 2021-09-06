using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.PerformanceManagement
{
    public class ViewTaskDetailsDataHandler : TemplateDataHandler
    {
        public DataTable Populate(string empId,string year)
        {
            DataTable dtUsers = new DataTable();            
            MySqlDataAdapter MSDA;
            try
            {
                mySqlCmd.CommandText = @"SELECT 
                                            t.GOAL_ID,
                                            t.TASK_ID,
                                            t.TASK_NAME,
                                            t.DESCRIPTION,
                                            CONVERT(t.PLAN_START_DATE,CHAR) AS PLAN_START_DATE,
                                            CONVERT(t.ACTUAL_START_DATE,CHAR) AS ACTUAL_START_DATE,
                                            CONVERT(t.TARGET_DATE,CHAR) AS TARGET_DATE,
                                            CONVERT(t.EXTENDED_TARGET_DATE,CHAR) AS EXTENDED_TARGET_DATE,
                                            t.TOTAL_COMPLETION
                                        FROM
                                            TASK t,
                                            EMPLOYEE_GOALS eg
                                        WHERE
                                            eg.GOAL_ID = t.GOAL_ID
                                                AND eg.EMPLOYEE_ID = @empId
                                                AND YEAR_OF_GOAL = @year;";

                mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId));
                mySqlCmd.Parameters.Add(new MySqlParameter("@year", year));

                MSDA = new MySqlDataAdapter(mySqlCmd);
                MSDA.Fill(dtUsers);

                return dtUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                MSDA = null;
                mySqlCmd.Parameters.Clear();
            }
        }

        public DataTable PopulateTaskExtentions(string tskId)
        {
            DataTable dtUsers = new DataTable();
            MySqlDataAdapter MSDA;

            try
            {
                mySqlCmd.CommandText = @"SELECT 
                                            TASK_ID,
                                            TASK_EXTENTION_ID,
                                            CONVERT(EXTENDED_DATE,CHAR) AS EXTENDED_DATE,
                                            REASON,
                                            CASE WHEN STATUS_CODE = '1' THEN 'Active'
                                                WHEN STATUS_CODE = '0' THEN 'Inactive'
                                            END AS STATUS_CODE
                                        FROM
                                            TASK_EXTENTIONS
                                        WHERE
                                            TASK_ID = @tskId;";

                mySqlCmd.Parameters.Add(new MySqlParameter("@tskId", tskId));

                MSDA = new MySqlDataAdapter(mySqlCmd);
                MSDA.Fill(dtUsers);

                return dtUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                MSDA = null;
                mySqlCmd.Parameters.Clear();
            }
        }

        public DataTable PopulateTaskProgress(string tskId)
        {
            DataTable dtUsers = new DataTable();
            MySqlDataAdapter MSDA;

            try
            {
                mySqlCmd.CommandText = @"SELECT 
                                            LINE_NO,
                                            TASK_ID,
                                            CONVERT(OBSERVED_DATE,CHAR) AS OBSERVED_DATE,
                                            PROGRESS,
                                            REMARKS,
                                            CASE WHEN STATUS_CODE = '1' THEN 'Active'
                                                WHEN STATUS_CODE = '0' THEN 'Inactive'
                                            END AS STATUS_CODE
                                        FROM
                                            TASK_PROGRESS
                                        WHERE
                                            TASK_ID = @tskId;";

                mySqlCmd.Parameters.Add(new MySqlParameter("@tskId", tskId));

                MSDA = new MySqlDataAdapter(mySqlCmd);
                MSDA.Fill(dtUsers);

                return dtUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                MSDA = null;
                mySqlCmd.Parameters.Clear();
            }
        }

        public String getGoal(string tskId)
        {
            string taskName = "";
            mySqlCmd.Parameters.Add(new MySqlParameter("@tskId", tskId.Trim() == "" ? (object)DBNull.Value : tskId.Trim()));

            mySqlCmd.CommandText = @"
                                        SELECT 
                                            eg.GOAL_AREA
                                        FROM 
                                            EMPLOYEE_GOALS eg,
                                            TASK t,
                                            TASK_EXTENTIONS te
                                        WHERE 
                                            te.TASK_ID = t.TASK_ID AND 
                                            te.TASK_ID = @tskId AND 
                                            t.GOAL_ID = eg.GOAL_ID AND 
                                            eg.STATUS_CODE = '1';
                                    ";

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
    }
}
