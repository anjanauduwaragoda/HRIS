using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class TaskProgressDataHandler : TemplateDataHandler
    {

        public DataTable getTaskProgress(string empId,string yer)
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
                                    t.TASK_NAME,
                                    t.TOTAL_COMPLETION AS PROGRESS,
                                    CONVERT( t.TARGET_DATE , CHAR) AS TARGET_DATE,
                                    CONVERT( t.PLAN_START_DATE , CHAR) AS PLAN_START_DATE,
                                    CONVERT( t.ACTUAL_START_DATE , CHAR) AS ACTUAL_START_DATE,
                                    CASE
                                        WHEN t.STATUS_CODE = '1' then 'Active'
                                        WHEN t.STATUS_CODE = '0' then 'Inactive'
                                    End as STATUS_CODE
                                FROM
                                    TASK t,
                                    EMPLOYEE_GOALS eg
                                WHERE
                                    eg.EMPLOYEE_ID = '"+ empId +@"'
                                        AND t.TASK_YEAR = '"+ yer + @"'
                                        AND t.GOAL_ID = eg.GOAL_ID
                                        AND t.STATUS_CODE = '1' AND t.IS_AGREE = '1' AND t.SUPERVISOR_AGREE = '1' AND eg.SUPERVISOR_AGREE = '1'
                                group by t.TASK_ID;";
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

        public bool Insert(string tskId, string obDate, string progress, string obremarks,string obstatus, string obuser)
        {
            bool isInsert = false;
            string sMySqlString = "";
            obDate = CommonUtils.dateFormatChange(obDate);
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Clear();

                //SerialHandler nserialcode = new SerialHandler();
                //string line_id = nserialcode.getserila(mySqlCon, "PRO");

                //mySqlCmd.Parameters.Add(new MySqlParameter("@line_id", line_id.Trim() == "" ? (object)DBNull.Value : line_id.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@tskId", tskId.Trim() == "" ? (object)DBNull.Value : tskId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@obDate", obDate.Trim() == "" ? (object)DBNull.Value : obDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@progress", progress.Trim() == "" ? (object)DBNull.Value : progress.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@obremarks", obremarks.Trim() == "" ? (object)DBNull.Value : obremarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@obstatus", obstatus.Trim() == "" ? (object)DBNull.Value : obstatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@obuser", obuser.Trim() == "" ? (object)DBNull.Value : obuser.Trim()));

                sMySqlString = @"INSERT INTO TASK_PROGRESS (TASK_ID,OBSERVED_DATE,PROGRESS,REMARKS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                    VALUES(@tskId,@obDate,@progress,@obremarks,@obstatus,@obuser,now(),@obuser,now())";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                sMySqlString = "UPDATE TASK SET TOTAL_COMPLETION = @progress WHERE TASK_ID = @tskId; ";
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

        public bool Update(string line_id,string tskId, string obDate, string progress, string obremarks, string obstatus, string obuser)
        {
            bool isUpdate = false;
            string sMySqlString = "";
            obDate = CommonUtils.dateFormatChange(obDate);
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@line_id", line_id.Trim() == "" ? (object)DBNull.Value : line_id.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@tskId", tskId.Trim() == "" ? (object)DBNull.Value : tskId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@obDate", obDate.Trim() == "" ? (object)DBNull.Value : obDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@progress", progress.Trim() == "" ? (object)DBNull.Value : progress.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@obremarks", obremarks.Trim() == "" ? (object)DBNull.Value : obremarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@obstatus", obstatus.Trim() == "" ? (object)DBNull.Value : obstatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@obuser", obuser.Trim() == "" ? (object)DBNull.Value : obuser.Trim()));

                sMySqlString = @"UPDATE TASK_PROGRESS 
                                        SET 
                                            OBSERVED_DATE = @obDate,
	                                        PROGRESS = @progress,
	                                        REMARKS = @obremarks,
                                            STATUS_CODE = @obstatus,
                                            MODIFIED_BY = @logUser,
                                            MODIFIED_DATE = now()
                                        WHERE
                                            LINE_NO = @line_id;";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                sMySqlString = "UPDATE TASK SET TOTAL_COMPLETION = @progress WHERE TASK_ID = @tskId; ";
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
        


        public String getGoal(string tskId)
        {
            string taskName = "";

            mySqlCmd.Parameters.Add(new MySqlParameter("@tskId", tskId.Trim() == "" ? (object)DBNull.Value : tskId.Trim()));

            mySqlCmd.CommandText = @"SELECT 
                                        eg.GOAL_AREA
                                    FROM
                                        EMPLOYEE_GOALS eg,
                                        TASK t,
                                        TASK_PROGRESS tp
                                    WHERE
                                        tp.TASK_ID = t.TASK_ID
                                            AND tp.TASK_ID = @tskId
                                            AND t.GOAL_ID = eg.GOAL_ID;";

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

        public DataTable getTaskProgressbyYear(string empId, string yer)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"";
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

        public DataTable getTaskProgressList(string tskId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    t.TASK_ID, t.TASK_NAME, CONVERT(tp.OBSERVED_DATE,CHAR) AS OBSERVED_DATE, tp.PROGRESS,tp.LINE_NO,tp.REMARKS,tp.STATUS_CODE
                                FROM
                                    TASK t,
                                    TASK_PROGRESS tp
                                WHERE
                                    t.TASK_ID = tp.TASK_ID
                                        AND t.TASK_ID = '" + tskId + "' AND tp.STATUS_CODE = '1';";
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

        public String getMaxProgress(string tskId)
        {
            string taskName = "";

            mySqlCmd.Parameters.Add(new MySqlParameter("@tskId", tskId.Trim() == "" ? (object)DBNull.Value : tskId.Trim()));

            mySqlCmd.CommandText = @"SELECT MAX(PROGRESS) FROM TASK_PROGRESS WHERE TASK_ID = @tskId AND STATUS_CODE = '1';";

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

        public String getMaxBeforProgress(string tskId)
        {
            string taskName = "";

            mySqlCmd.Parameters.Add(new MySqlParameter("@tskId", tskId.Trim() == "" ? (object)DBNull.Value : tskId.Trim()));

            mySqlCmd.CommandText = @"SELECT 
                                        MAX(PROGRESS)
                                    FROM
                                        TASK_PROGRESS
                                    WHERE
                                        PROGRESS NOT IN (SELECT 
                                                MAX(PROGRESS)
                                            FROM
                                                TASK_PROGRESS
                                            WHERE
                                                TASK_ID = @tskId AND STATUS_CODE = '1' )
                                            AND TASK_ID = @tskId AND STATUS_CODE = '1';";

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
                                         MAX(CONVERT( te.OBSERVED_DATE , CHAR)) AS OBSERVED_DATE
                                    FROM
                                        TASK_PROGRESS te,
                                        EMPLOYEE_GOALS eg,
                                        TASK t
                                    WHERE 
                                        eg.GOAL_ID = t.GOAL_ID AND te.STATUS_CODE = '1'
                                            AND eg.EMPLOYEE_ID = '" + empId + @"' AND t.TASK_ID = te.TASK_ID 
                                            AND t.TASK_ID = '" + tskId + "';";

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
