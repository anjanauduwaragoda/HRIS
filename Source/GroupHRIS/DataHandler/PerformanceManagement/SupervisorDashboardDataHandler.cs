using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class SupervisorDashboardDataHandler : TemplateDataHandler
    {
        public DataTable Populate(string EmployeeID, string YearOfAssessment, string AssessmentStatusCode, string AssessmentID, string EvaluationStatusCode)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                dataTable.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@APPRASER", EmployeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", YearOfAssessment));

                string queryString = @"
                                            SELECT 
                                                AE.ASSESSMENT_ID,
                                                E.EMPLOYEE_ID, 
                                                E.TITLE, 
                                                E.INITIALS_NAME, 
                                                AE.YEAR_OF_ASSESSMENT, 
                                                AE.INCLUDE_GOAL_ASSESSMENT,
                                                (SELECT EGA.STATUS_CODE FROM EMPLOYEE_GOAL_ASSESSMENT EGA WHERE EGA.ASSESSMENT_ID = AE.ASSESSMENT_ID AND EGA.EMPLOYEE_ID = AE.EMPLOYEE_ID AND EGA.YEAR_OF_ASSESSMENT = AE.YEAR_OF_ASSESSMENT) AS 'GOAL_ASSESSMENT_STATUS', 
                                                AE.INCLUDE_COMPITANCY_ASSESSMENT, 
                                                (SELECT ECA.STATUS_CODE FROM EMPLOYEE_COMPITANCY_ASSESSMENT ECA WHERE ECA.ASSESSMENT_ID = AE.ASSESSMENT_ID AND ECA.EMPLOYEE_ID = AE.EMPLOYEE_ID AND ECA.YEAR_OF_ASSESSMENT = AE.YEAR_OF_ASSESSMENT) AS 'COMPETENCY_ASSESSMENT_STATUS', 
                                                AE.INCLUDE_SELF_ASSESSMENT, 
                                                (SELECT ESA.STATUS_CODE FROM EMPLOYEE_SELF_ASSESSMENT ESA WHERE ESA.ASSESSMENT_ID = AE.ASSESSMENT_ID AND ESA.EMPLOYEE_ID = AE.EMPLOYEE_ID AND ESA.YEAR_OF_ASSESSMENT = AE.YEAR_OF_ASSESSMENT) AS 'SELF_ASSESSMENT_STATUS', 
                                                AE.STATUS_CODE,
                                                AE.IS_EMPLOYEE_AGREED 
                                            FROM  
                                                ASSESSED_EMPLOYEES AE, 
                                                EMPLOYEE E,
                                                ASSESSMENT A
                                            WHERE 
                                                AE.EMPLOYEE_ID = E.EMPLOYEE_ID AND 
                                                AE.ASSESSMENT_ID = A.ASSESSMENT_ID AND                                                 
                                                AE.APPRASER = @APPRASER AND 
                                                AE.YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT 
                                        ";
                if (AssessmentStatusCode != "")
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", AssessmentStatusCode));
                    queryString += @" AND A.STATUS_CODE = @STATUS_CODE ";
                }
                if (EvaluationStatusCode != "")
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EVALUATION_STATUS_CODE", EvaluationStatusCode));
                    queryString += @" AND AE.STATUS_CODE = @EVALUATION_STATUS_CODE ";
                }
                if (AssessmentID != "")
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", AssessmentID));
                    queryString += @" AND AE.ASSESSMENT_ID = @ASSESSMENT_ID ";
                }

                mySqlCmd.CommandText = queryString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
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
        
        public DataTable PopulateYears()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();
                string queryString = @"
                                            SELECT 
                                                DISTINCT(YEAR_OF_ASSESSMENT) 
                                            FROM 
                                                ASSESSED_EMPLOYEES 
                                            ORDER BY 
                                                YEAR_OF_ASSESSMENT DESC;
                                      ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(queryString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable PopulateAssessments(string YearOfAssessment, string StatusCode, string Appraser)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();

                string queryString = @"
                                        SELECT 
                                            DISTINCT(A.ASSESSMENT_ID) AS 'ASSESSMENT_ID', 
                                            A.ASSESSMENT_NAME 
                                        FROM 
                                            ASSESSMENT A,
                                            ASSESSED_EMPLOYEES AE
                                        WHERE 
                                            A.ASSESSMENT_ID = AE.ASSESSMENT_ID AND
                                            A.YEAR_OF_ASSESSMENT = AE.YEAR_OF_ASSESSMENT AND
                                            A.STATUS_CODE = '" + StatusCode + @"' AND 
                                            A.YEAR_OF_ASSESSMENT = '" + YearOfAssessment + @"' AND
                                            AE.APPRASER = '" + Appraser + @"';
                                    ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(queryString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable PopulateAssessments(string YearOfAssessment, string Appraser)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();

                string queryString = @"
                                        SELECT DISTINCT
                                            (A.ASSESSMENT_ID) AS 'ASSESSMENT_ID', A.ASSESSMENT_NAME
                                        FROM
                                            ASSESSMENT A,
                                            ASSESSED_EMPLOYEES AE
                                        WHERE
                                            A.ASSESSMENT_ID = AE.ASSESSMENT_ID
                                                AND A.YEAR_OF_ASSESSMENT = AE.YEAR_OF_ASSESSMENT
                                                AND A.YEAR_OF_ASSESSMENT = '" + YearOfAssessment + @"'
                                                AND AE.APPRASER = '" + Appraser + @"';
                                    ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(queryString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable PopulateEmployeeCompetencyAssessmentDetails(string AppraserID, string YearOfAssessment, string AssessmentID, string AssessmentStatus)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();
                string queryString = @"

                                        SELECT 
                                            ECA.STATUS_CODE, 
                                            COUNT(ECA.EMPLOYEE_ID) AS 'EMPLOYEE_COUNT'
                                        FROM 
                                            EMPLOYEE_COMPITANCY_ASSESSMENT ECA,
                                            ASSESSED_EMPLOYEES AE, 
                                            ASSESSMENT A  
                                        WHERE
                                            A.ASSESSMENT_ID = AE.ASSESSMENT_ID 
                                            AND ECA.ASSESSMENT_ID = AE.ASSESSMENT_ID 
                                            AND ECA.EMPLOYEE_ID = AE.EMPLOYEE_ID 
                                            AND ECA.YEAR_OF_ASSESSMENT = AE.YEAR_OF_ASSESSMENT 
                                    ";

                if (AppraserID != "")
                {
                    queryString += " AND AE.APPRASER = '" + AppraserID + "' ";
                }
                if (YearOfAssessment != "")
                {
                    queryString += " AND AE.YEAR_OF_ASSESSMENT = '" + YearOfAssessment + "' ";
                }
                if (AssessmentID != "")
                {
                    queryString += " AND AE.ASSESSMENT_ID = '" + AssessmentID + "' ";
                }
                if (AssessmentStatus != "")
                {
                    queryString += " AND A.STATUS_CODE = '" + AssessmentStatus + "' ";
                }

                queryString += " GROUP BY ECA.STATUS_CODE; ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(queryString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        string Status = dataTable.Rows[i]["STATUS_CODE"].ToString().Trim();

                        if (Status == Constants.ASSESSNEMT_PENDING_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_PENDING_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_ACTIVE_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_ACTIVE_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_CEO_FINALIZED_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_CLOSED_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_CLOSED_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_OBSOLETE_TAG;
                        }
                    }
                }

                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable PopulateEmployeeGoalAssessmentDetails(string AppraserID, string YearOfAssessment, string AssessmentID, string AssessmentStatus)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();
                string queryString = @"

                                        SELECT 
                                            EGA.STATUS_CODE, 
                                            COUNT(EGA.EMPLOYEE_ID) AS 'EMPLOYEE_COUNT'
                                        FROM 
                                            EMPLOYEE_GOAL_ASSESSMENT EGA,
                                            ASSESSED_EMPLOYEES AE,
                                            ASSESSMENT A 
                                        WHERE
                                            A.ASSESSMENT_ID = AE.ASSESSMENT_ID AND
                                            EGA.ASSESSMENT_ID = AE.ASSESSMENT_ID AND
                                            EGA.EMPLOYEE_ID = AE.EMPLOYEE_ID AND
                                            EGA.YEAR_OF_ASSESSMENT = AE.YEAR_OF_ASSESSMENT  
                                    ";

                if (AppraserID != "")
                {
                    queryString += " AND AE.APPRASER = '" + AppraserID + "' ";
                }
                if (YearOfAssessment != "")
                {
                    queryString += " AND AE.YEAR_OF_ASSESSMENT = '" + YearOfAssessment + "' ";
                }
                if (AssessmentID != "")
                {
                    queryString += " AND AE.ASSESSMENT_ID = '" + AssessmentID + "' ";
                }
                if (AssessmentStatus != "")
                {
                    queryString += " AND A.STATUS_CODE = '" + AssessmentStatus + "' ";
                }

                queryString += " GROUP BY EGA.STATUS_CODE; ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(queryString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        string Status = dataTable.Rows[i]["STATUS_CODE"].ToString().Trim();

                        if (Status == Constants.ASSESSNEMT_PENDING_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_PENDING_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_ACTIVE_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_ACTIVE_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_CEO_FINALIZED_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_CLOSED_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_CLOSED_TAG;
                        }
                        else if (Status == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE"] = Constants.ASSESSNEMT_OBSOLETE_TAG;
                        }
                    }
                }


                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
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
