using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.PerformanceManagement
{
    public class EmployeeAssessmentDashboardDataHandler : TemplateDataHandler
    {
        public DataTable getAvailableAssessmentList(string empId, string assYear, string assStatus,string empStatus)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT
                                    a.ASSESSMENT_ID,
                                    a.ASSESSMENT_NAME,
                                    at.ASSESSMENT_TYPE_NAME,
                                    ae.INCLUDE_SELF_ASSESSMENT,
                                    ae.INCLUDE_COMPITANCY_ASSESSMENT,
                                    ae.INCLUDE_GOAL_ASSESSMENT,
                                    CASE
                                            WHEN ae.STATUS_CODE = '1' THEN 'Active'
                                            WHEN ae.STATUS_CODE = '9' THEN 'Obsolete'
                                            WHEN ae.STATUS_CODE = '0' THEN 'Pending'
                                            WHEN ae.STATUS_CODE = '2' THEN 'Subordinate Finalized'
                                            WHEN (ae.STATUS_CODE = '3' AND ae.IS_EMPLOYEE_AGREED IS null) THEN 'Supervisor Completed'
											WHEN (ae.STATUS_CODE = '3' AND ae.IS_EMPLOYEE_AGREED = '0') THEN 'Subordinate Disagreed'
											WHEN (ae.STATUS_CODE = '3' AND ae.IS_EMPLOYEE_AGREED = '1') THEN 'Subordinate Agreed'
                                            WHEN ae.STATUS_CODE = '4' THEN 'Supervisor Finalized'
                                            WHEN ae.STATUS_CODE = '5' THEN 'CEO Finalized'
                                            WHEN (a.STATUS_CODE = '6' OR ae.STATUS_CODE = '6') THEN 'Closed'
                                        END AS STATUS_CODE
                                FROM
                                    ASSESSED_EMPLOYEES ae,
                                    ASSESSMENT a,
                                    ASSESSMENT_TYPE at
                                WHERE
                                    ae.EMPLOYEE_ID = '" + empId + @"'
                                        AND a.ASSESSMENT_ID = ae.ASSESSMENT_ID
                                        AND at.ASSESSMENT_TYPE_ID = a.ASSESSMENT_TYPE_ID AND (a.STATUS_CODE = '1' OR a.STATUS_CODE = '6')
                                        AND a.YEAR_OF_ASSESSMENT = '" + assYear + "' AND ae.STATUS_CODE = '" + assStatus + "'";

                if (empStatus != String.Empty)
                {
                    if (empStatus != "null")
                    {
                        Qry += @" AND ae.IS_EMPLOYEE_AGREED = '" + empStatus + "' ";
                    }
                    else
                    {
                        Qry += @" AND ae.IS_EMPLOYEE_AGREED IS " + empStatus + " ";
                    }
                }

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

        

        public DataTable getAllAvailableAssessmentList(string empId, string assYear)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT
                                    a.ASSESSMENT_ID,
                                    a.ASSESSMENT_NAME,
                                    at.ASSESSMENT_TYPE_NAME,
                                    ae.INCLUDE_SELF_ASSESSMENT,
                                    ae.INCLUDE_COMPITANCY_ASSESSMENT,
                                    ae.INCLUDE_GOAL_ASSESSMENT,
                                    CASE
                                            WHEN ae.STATUS_CODE = '1' THEN 'Active'
                                            WHEN ae.STATUS_CODE = '9' THEN 'Obsolete'
                                            WHEN ae.STATUS_CODE = '0' THEN 'Pending'
                                            WHEN ae.STATUS_CODE = '2' THEN 'Subordinate Finalized'
                                            WHEN (ae.STATUS_CODE = '3' AND ae.IS_EMPLOYEE_AGREED IS null) THEN 'Supervisor Completed'
											WHEN (ae.STATUS_CODE = '3' AND ae.IS_EMPLOYEE_AGREED = '0') THEN 'Subordinate Disagreed'
											WHEN (ae.STATUS_CODE = '3' AND ae.IS_EMPLOYEE_AGREED = '1') THEN 'Subordinate Agreed'
                                            WHEN ae.STATUS_CODE = '4' THEN 'Supervisor Finalized'
                                            WHEN ae.STATUS_CODE = '5' THEN 'CEO Finalized'
                                            WHEN (a.STATUS_CODE = '6' OR ae.STATUS_CODE = '6') THEN 'Closed'
                                        END AS STATUS_CODE
                                FROM
                                    ASSESSED_EMPLOYEES ae,
                                    ASSESSMENT a,
                                    ASSESSMENT_TYPE at
                                WHERE
                                    ae.EMPLOYEE_ID = '" + empId + @"' AND ae.STATUS_CODE NOT IN (0,9)
                                        AND a.ASSESSMENT_ID = ae.ASSESSMENT_ID AND a.STATUS_CODE NOT IN(0,9)
                                        AND at.ASSESSMENT_TYPE_ID = a.ASSESSMENT_TYPE_ID AND (a.STATUS_CODE = '1' OR a.STATUS_CODE = '6')
                                        AND a.YEAR_OF_ASSESSMENT = '" + assYear + "';";
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
        //getAvailableAssessmentListByYear

        public DataTable getAllAvailableAssessmentList(string empId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    ae.ASSESSMENT_ID,
                                    a.ASSESSMENT_NAME,
                                    CASE
                                        WHEN ae.INCLUDE_SELF_ASSESSMENT = '1' THEN 'Active'
                                        WHEN ae.INCLUDE_SELF_ASSESSMENT = '0' THEN 'N/A'
                                        ELSE 'Pending'
                                    END AS INCLUDE_SELF_ASSESSMENT,
                                    CASE
                                        WHEN ae.INCLUDE_COMPITANCY_ASSESSMENT = '1' THEN 'Active'
                                        WHEN ae.INCLUDE_COMPITANCY_ASSESSMENT = '0' THEN 'N/A'
                                        ELSE 'Pending'
                                    END AS INCLUDE_COMPITANCY_ASSESSMENT,
                                    CASE
                                        WHEN ae.INCLUDE_GOAL_ASSESSMENT = '1' THEN 'Active'
                                        WHEN ae.INCLUDE_GOAL_ASSESSMENT = '0' THEN 'N/A'
                                        ELSE 'Pending'
                                    END AS INCLUDE_GOAL_ASSESSMENT
                                FROM
                                    ASSESSED_EMPLOYEES ae,
                                    ASSESSMENT a
                                WHERE
                                    ae.ASSESSMENT_ID = a.ASSESSMENT_ID
                                        AND ae.EMPLOYEE_ID = '" + empId + "' AND ae.STATUS_CODE = '1';";
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

        public String getGoalStatus(string assessmentId, string empId, string yearId)
        {
            string gStatus = "";

            //mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentId", assessmentId.Trim() == "" ? (object)DBNull.Value : assessmentId.Trim()));
            //mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId.Trim() == "" ? (object)DBNull.Value : empId.Trim()));
            //mySqlCmd.Parameters.Add(new MySqlParameter("@yearId", yearId.Trim() == "" ? (object)DBNull.Value : yearId.Trim()));

            mySqlCmd.CommandText = @"SELECT 
                                        CASE
                                            WHEN STATUS_CODE = '1' THEN 'Active'
                                            WHEN STATUS_CODE = '9' THEN 'Obsolete'
                                            WHEN STATUS_CODE = '0' THEN 'Pending'
                                            WHEN STATUS_CODE = '2' THEN 'Subordinate Finalized'
                                            WHEN STATUS_CODE = '3' THEN 'Supervisor Completed'
                                            WHEN STATUS_CODE = '4' THEN 'Supervisor Finalized'
                                            WHEN STATUS_CODE = '5' THEN 'CEO Finalized'
                                            WHEN STATUS_CODE = '6' THEN 'Closed'
                                        END AS STATUS_CODE
                                    FROM
                                        EMPLOYEE_GOAL_ASSESSMENT
                                    WHERE
                                        ASSESSMENT_ID = '" + assessmentId + "' AND EMPLOYEE_ID = '" + empId + "' AND YEAR_OF_ASSESSMENT = '" + yearId + "' ;";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    gStatus = mySqlCmd.ExecuteScalar().ToString();
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

            return gStatus;
        }

        public String getCompetencyStatus(string assessmentId, string empId, string yearId)
        {
            string cStatus = "";

            //mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentId", assessmentId.Trim() == "" ? (object)DBNull.Value : assessmentId.Trim()));
            //mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId.Trim() == "" ? (object)DBNull.Value : empId.Trim()));
            //mySqlCmd.Parameters.Add(new MySqlParameter("@yearId", yearId.Trim() == "" ? (object)DBNull.Value : yearId.Trim()));

            mySqlCmd.CommandText = @"SELECT 
                                        CASE
                                            WHEN STATUS_CODE = '1' THEN 'Active'
                                            WHEN STATUS_CODE = '9' THEN 'Obsolete'
                                            WHEN STATUS_CODE = '0' THEN 'Pending'
                                            WHEN STATUS_CODE = '2' THEN 'Subordinate Finalized'
                                            WHEN STATUS_CODE = '3' THEN 'Supervisor Completed'
                                            WHEN STATUS_CODE = '4' THEN 'Supervisor Finalized'
                                            WHEN STATUS_CODE = '5' THEN 'CEO Finalized'
                                            WHEN STATUS_CODE = '6' THEN 'Closed'
                                        END AS STATUS_CODE
                                    FROM
                                        EMPLOYEE_COMPITANCY_ASSESSMENT
                                    WHERE
                                        ASSESSMENT_ID = '" + assessmentId + "' AND EMPLOYEE_ID = '" + empId + "' AND YEAR_OF_ASSESSMENT = '" + yearId + "' ;";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    cStatus = mySqlCmd.ExecuteScalar().ToString();
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

            return cStatus;
        }

        public String getSelfAssessmentStatus(string assessmentId, string empId, string yearId)
        {
            string sStatus = "";

            //mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentId", assessmentId.Trim() == "" ? (object)DBNull.Value : assessmentId.Trim()));
            //mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId.Trim() == "" ? (object)DBNull.Value : empId.Trim()));
            //mySqlCmd.Parameters.Add(new MySqlParameter("@yearId", yearId.Trim() == "" ? (object)DBNull.Value : yearId.Trim()));

            mySqlCmd.CommandText = @"SELECT 
                                        CASE
                                            WHEN STATUS_CODE = '1' THEN 'Active'
                                            WHEN STATUS_CODE = '9' THEN 'Obsolete'
                                            WHEN STATUS_CODE = '0' THEN 'Pending'
                                            WHEN STATUS_CODE = '2' THEN 'Subordinate Finalized'
                                            WHEN STATUS_CODE = '3' THEN 'Supervisor Completed'
                                            WHEN STATUS_CODE = '4' THEN 'Supervisor Finalized'
                                            WHEN STATUS_CODE = '5' THEN 'CEO Finalized'
                                            WHEN STATUS_CODE = '6' THEN 'Closed'
                                        END AS STATUS_CODE
                                    FROM
                                        EMPLOYEE_SELF_ASSESSMENT
                                    WHERE
                                        ASSESSMENT_ID = '" + assessmentId + "' AND EMPLOYEE_ID = '" + empId + "' AND YEAR_OF_ASSESSMENT = '" + yearId + "' ;";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    sStatus = mySqlCmd.ExecuteScalar().ToString();
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

            return sStatus;
        }

        public DataTable getLatestAssessment(string empId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    ae.ASSESSMENT_ID,
                                    a.ASSESSMENT_NAME,
                                    a.ASSESSMENT_TYPE_ID,
                                    ap.PURPOSE_ID,
                                    p.NAME,
                                    t.ASSESSMENT_TYPE_NAME
                                FROM
                                    ASSESSED_EMPLOYEES ae,
                                    ASSESSMENT a,
                                    ASSESSMENT_PURPOSES ap,
                                    ASSESSMENT_PURPOSE p,
                                    ASSESSMENT_TYPE t
                                WHERE
                                    ae.ADDED_DATE = (SELECT 
                                            MAX(ADDED_DATE)
                                        FROM
                                            ASSESSED_EMPLOYEES
                                        WHERE
                                            EMPLOYEE_ID = '" + empId + @"')
                                        AND ae.ASSESSMENT_ID = a.ASSESSMENT_ID
                                        AND a.ASSESSMENT_ID = ap.ASSESSMENT_ID AND ap.STATUS_CODE = '1'
                                        AND ap.PURPOSE_ID = p.PURPOSE_ID AND a.STATUS_CODE = '1'
                                        AND t.ASSESSMENT_TYPE_ID = a.ASSESSMENT_TYPE_ID GROUP BY ap.PURPOSE_ID
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

        public DataTable getAssessmentPurposes(string assId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    ap.ASSESSMENT_ID, ap.PURPOSE_ID, p.NAME
                                FROM
                                    ASSESSMENT_PURPOSES ap,
                                    ASSESSMENT_PURPOSE p
                                WHERE
                                    ap.ASSESSMENT_ID = '" + assId + @"'
                                        AND ap.STATUS_CODE = '1'
                                        AND ap.PURPOSE_ID = p.PURPOSE_ID
                                        AND p.STATUS_CODE;";
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

        public DataTable latestAssessmentAvalability(string assId, string empId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    ae.ASSESSMENT_ID,
                                    ae.EMPLOYEE_ID,
                                    ae.YEAR_OF_ASSESSMENT,
                                    ae.INCLUDE_SELF_ASSESSMENT,
                                    ae.INCLUDE_COMPITANCY_ASSESSMENT,
                                    ae.INCLUDE_GOAL_ASSESSMENT,
                                    CASE
                                        WHEN ae.STATUS_CODE = '1' THEN 'Active'
                                            WHEN ae.STATUS_CODE = '9' THEN 'Obsolete'
                                            WHEN ae.STATUS_CODE = '0' THEN 'Pending'
                                            WHEN ae.STATUS_CODE = '2' THEN 'Subordinate Finalized'
                                            WHEN (ae.STATUS_CODE = '3' AND ae.IS_EMPLOYEE_AGREED IS null) THEN 'Supervisor Completed'
											WHEN (ae.STATUS_CODE = '3' AND ae.IS_EMPLOYEE_AGREED = '0') THEN 'Subordinate Disagreed'
											WHEN (ae.STATUS_CODE = '3' AND ae.IS_EMPLOYEE_AGREED = '1') THEN 'Subordinate Agreed'
                                            WHEN ae.STATUS_CODE = '4' THEN 'Supervisor Finalized'
                                            WHEN ae.STATUS_CODE = '5' THEN 'CEO Finalized'
                                            WHEN ae.STATUS_CODE = '6' THEN 'Closed'
                                    END AS STATUS_CODE
                                FROM
                                    ASSESSED_EMPLOYEES ae,
                                    ASSESSMENT a
                                WHERE
                                    ae.ASSESSMENT_ID = '" + assId + @"'
                                        AND ae.EMPLOYEE_ID = '" + empId + @"'
                                        AND a.ASSESSMENT_ID = ae.ASSESSMENT_ID;";
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

        public String isFeedbackGiven(string assessmentId, string empId, string yearId)
        {
            string cStatus = "";

            mySqlCmd.CommandText = @"SELECT 
                                        ASSESSMENT_ID
                                    FROM
                                        ASSESSED_EMPLOYEES
                                    WHERE
                                        ASSESSMENT_ID = '" + assessmentId + "' AND EMPLOYEE_ID = '"+ empId +@"'
                                            AND YEAR_OF_ASSESSMENT = '"+ yearId +@"'
                                            AND IS_FEEDBACK_SUBMITTED = '1';";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    cStatus = mySqlCmd.ExecuteScalar().ToString();
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

            return cStatus;
        }

    }

}