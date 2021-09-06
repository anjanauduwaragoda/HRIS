using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class GoalAgreementDataHandler : TemplateDataHandler
    {
        public DataTable getGoalList(string empId,string year)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    GOAL_ID,GOAL_AREA,WEIGHT,
                                    CASE
                                    WHEN EMPLOYEE_AGREE = '1' then 'Agreed'
                                    WHEN EMPLOYEE_AGREE = '0' then 'Disagreed'
                                End as EMPLOYEE_AGREE,
                                CASE WHEN SUPERVISOR_AGREE = '1' then 'Confirmed'
                                     WHEN SUPERVISOR_AGREE = '0' then 'Rejected' 
                                END as SUPERVISOR_AGREE
                                FROM
                                    EMPLOYEE_GOALS
                                WHERE
                                    EMPLOYEE_ID = '" + empId + "' AND YEAR_OF_GOAL = '" + year + "' AND STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "';";
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

        public String isReporter(string sSubordinate, string sSupervisor)
        {
            string id = "";

            mySqlCmd.CommandText = @"SELECT 
                                        REPORT_TO_1
                                    FROM
                                        EMPLOYEE
                                    WHERE 
                                        '" + sSupervisor + @"' IN (REPORT_TO_1)
                                            AND EMPLOYEE_ID = '" + sSubordinate + "';";

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

        public DataTable getGoalDetails(string empId, string year,string goalId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    GOAL_ID,
                                    GOAL_AREA,
                                    DESCRIPTION,
                                    MEASUREMENTS,
                                    WEIGHT,
                                    EMPLOYEE_AGREE,
                                    EMPLOYEE_REASON,
                                    SUPERVISOR_AGREE,
                                    SUPERVISOR_REASON
                                FROM
                                    EMPLOYEE_GOALS
                                WHERE
                                    EMPLOYEE_ID = '"+ empId +"' AND YEAR_OF_GOAL = '"+ year +@"'
                                        AND GOAL_ID = '" + goalId + "' AND STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "';";
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

        public bool Update(string goalId, string status, string reason,string empId,string year,string user)
        {
            bool isInsert = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@goalId", goalId.Trim() == "" ? (object)DBNull.Value : goalId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@reason", reason.Trim() == "" ? (object)DBNull.Value : reason.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId.Trim() == "" ? (object)DBNull.Value : empId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@year", year.Trim() == "" ? (object)DBNull.Value : year.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));

                sMySqlString = @"UPDATE EMPLOYEE_GOALS 
                                    SET 
                                        EMPLOYEE_AGREE = @status,
                                        EMPLOYEE_REASON = @reason,
                                        APPROVED_BY = @user
                                    WHERE
                                        GOAL_ID = @goalId
                                            AND EMPLOYEE_ID = @empId
                                            AND YEAR_OF_GOAL = @year; ";


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

        public bool UpdateSupervisor(string goalId, string status, string reason, string empId, string year,string user)
        {
            bool isInsert = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@goalId", goalId.Trim() == "" ? (object)DBNull.Value : goalId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@reason", reason.Trim() == "" ? (object)DBNull.Value : reason.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId.Trim() == "" ? (object)DBNull.Value : empId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@year", year.Trim() == "" ? (object)DBNull.Value : year.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));

                sMySqlString = @"UPDATE EMPLOYEE_GOALS 
                                    SET 
                                        SUPERVISOR_AGREE = @status,
                                        SUPERVISOR_REASON = @reason,
                                        RECOMEND_BY = @user
                                    WHERE
                                        GOAL_ID = @goalId
                                            AND EMPLOYEE_ID = @empId
                                            AND YEAR_OF_GOAL = @year; ";


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

        public bool UpdateIsFinalize(List<string> goalId,string empId, string year)
        {
            bool isFinalize = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                //mySqlCmd.Parameters.Add(new MySqlParameter("@goalId", goalId.Trim() == "" ? (object)DBNull.Value : goalId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId.Trim() == "" ? (object)DBNull.Value : empId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@year", year.Trim() == "" ? (object)DBNull.Value : year.Trim()));

                for (int i = 0; i < goalId.Count; i++)
                {
                    sMySqlString = @"UPDATE EMPLOYEE_GOALS 
                                    SET 
                                        IS_LOCKED = '1'
                                    WHERE
                                        GOAL_ID = '"+ goalId[i]+@"'
                                            AND EMPLOYEE_ID = @empId
                                            AND YEAR_OF_GOAL = @year; ";


                    mySqlCmd.Transaction = mySqlTrans;
                    mySqlCmd.CommandText = sMySqlString;

                    mySqlCmd.ExecuteNonQuery();
                }
                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isFinalize = true;
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
            return isFinalize;
        }

        public bool isLock(List<string> goalId, string empId, string year)
        {
            bool isLock = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;
            int count = 0;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId.Trim() == "" ? (object)DBNull.Value : empId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@year", year.Trim() == "" ? (object)DBNull.Value : year.Trim()));

                for (int i = 0; i < goalId.Count; i++)
                {
                    sMySqlString = @"SELECT IS_LOCKED 
                                    FROM EMPLOYEE_GOALS
                                    WHERE
                                        GOAL_ID = '" + goalId[i] + @"'
                                            AND EMPLOYEE_ID = @empId
                                            AND YEAR_OF_GOAL = @year; ";


                    mySqlCmd.Transaction = mySqlTrans;
                    mySqlCmd.CommandText = sMySqlString;

                    //mySqlCmd.ExecuteNonQuery();
                    if (mySqlCmd.ExecuteScalar() != null)
                    {
                        mySqlCmd.ExecuteScalar().ToString();
                        count = count + 1;
                    }
                }

                if (count == goalId.Count)
                {
                    isLock = true;
                }
                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

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
            return isLock;
        }

    }
}
