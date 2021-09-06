using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class PerformanceEvaluationDataHandler : TemplateDataHandler
    {
        public DataTable PopulateAssessmentDetails(string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT 
                                                A.ASSESSMENT_NAME, 
                                                AST.ASSESSMENT_TYPE_NAME 
                                            FROM 
                                                ASSESSMENT A,
                                                ASSESSMENT_TYPE AST,
                                                ASSESSED_EMPLOYEES AE
                                            WHERE 
                                                A.ASSESSMENT_ID = AE.ASSESSMENT_ID AND
                                                AST.ASSESSMENT_TYPE_ID = A.ASSESSMENT_TYPE_ID AND
                                                AE.ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                AE.EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                AE.YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                mySqlDa.Dispose();

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataTable.Dispose();
                mySqlCmd.Parameters.Clear();
            }
        }

        public DataTable PopulateAssessmentPurposes(string assessmentID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));

                string sMySqlString = @"                                            
                                            SELECT 
                                                A.ASSESSMENT_NAME, 
                                                APS.PURPOSE_ID, 
                                                AP.NAME 
                                            FROM 
                                                ASSESSMENT A,
                                                ASSESSMENT_PURPOSE AP,
                                                ASSESSMENT_PURPOSES APS
                                            WHERE 
                                                A.ASSESSMENT_ID = APS.ASSESSMENT_ID AND 
                                                AP.PURPOSE_ID = APS.PURPOSE_ID AND
                                                APS.ASSESSMENT_ID = @ASSESSMENT_ID; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                mySqlDa.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public DataTable PopulateEmployeeDetails(string employeeID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));

                string sMySqlString = @"                                            
                                            SELECT 
                                                E.EMPLOYEE_ID, E.TITLE, E.INITIALS_NAME, ED.DESIGNATION_NAME
                                            FROM
                                                EMPLOYEE E,
                                                EMPLOYEE_DESIGNATION ED
                                            WHERE
                                                E.EMPLOYEE_ID = E.EMPLOYEE_ID
                                                    AND E.DESIGNATION_ID = ED.DESIGNATION_ID
                                                    AND E.EMPLOYEE_ID = @EMPLOYEE_ID; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public DataTable PopulateSupervisorDetails(string employeeID, string assessmentID, string yearofAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearofAssessment));

                string sMySqlString = @"                                            
                                            SELECT 
                                                E.EMPLOYEE_ID, E.TITLE, E.INITIALS_NAME, ED.DESIGNATION_NAME, E.EMAIL 
                                            FROM
                                                ASSESSED_EMPLOYEES AE,
                                                EMPLOYEE E,
                                                EMPLOYEE_DESIGNATION ED
                                            WHERE
                                                AE.APPRASER = E.EMPLOYEE_ID
                                                    AND E.DESIGNATION_ID = ED.DESIGNATION_ID
                                                    AND AE.ASSESSMENT_ID = @ASSESSMENT_ID
                                                    AND AE.EMPLOYEE_ID = @EMPLOYEE_ID
                                                    AND AE.YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public DataTable PopulateCEODetails(string employeeID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));

                string sMySqlString = @"                                            
                                            SELECT 
                                                CEO.TITLE, CEO.INITIALS_NAME, CEO.EMAIL
                                            FROM
                                                EMPLOYEE SE,
                                                EMPLOYEE CEO
                                            WHERE
                                                CEO.EMPLOYEE_ID = SE.REPORT_TO_3
                                                    AND SE.EMPLOYEE_ID = @EMPLOYEE_ID; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public DataTable PopulatePreviousData(string EmployeeID)
        {
            dataTable = new DataTable();
            mySqlCmd = new MySqlCommand();
            try
            {
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"                                            
                                            SELECT 
                                                CONVERT( DATE_FORMAT(MODIFIED_DATE,'%Y-%m-%d') , CHAR) AS 'SUPERVISOR_COMPLETED_DATE',
                                                CONVERT( TOTAL , CHAR) AS 'TOTAL_SUPERVISOR_SCORE'
                                            FROM
                                                ASSESSED_EMPLOYEES
                                            WHERE
                                                EMPLOYEE_ID = @EMPLOYEE_ID 
                                                    AND TOTAL IS NOT NULL
                                                    AND MODIFIED_DATE IS NOT NULL
                                                    AND MODIFIED_DATE >= DATE_SUB(NOW(), INTERVAL 5 YEAR)
                                            ORDER BY MODIFIED_DATE ASC;
                                        ";

                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                dataTable.Dispose();
                mySqlCmd.Dispose();
            }
        }

        public DataTable PopulateGoalAssessmentMarks(string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT 
                                                TOTAL_SELF_SCORE AS 'TOTAL_SELF_SCORE', 
                                                TOTAL_SUPERVISORS_SCORE AS 'TOTAL_SUPERVISOR_SCORE' 
                                            FROM 
                                                EMPLOYEE_GOAL_ASSESSMENT 
                                            WHERE 
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public DataTable PopulateEmployeeCompetencyAssessmentMarks(string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT 
                                                CAD.EMPLOYEE_RATING, 
                                                CAD.EMPLOYEE_WEIGHT, 
                                                COUNT(CAD.EMPLOYEE_WEIGHT) AS 'WEIGHT_COUNT' 
                                            FROM 
                                                EMPLOYEE_COMPITANCY_ASSESSMENT ECA,
                                                COMPETENCY_ASSESSMENT_DETAILS CAD
                                            WHERE
                                                ECA.ASSESSMENT_TOKEN = CAD.ASSESSMENT_TOKEN AND
                                                ECA.EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                ECA.ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                ECA.YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT
                                            GROUP BY 
                                                CAD.EMPLOYEE_RATING; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public DataTable PopulateProficiencyLevels(string employeeID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));

                string sMySqlString = @"                                            
                                            SELECT 
                                                PL.RATING, 
                                                PL.REMARKS 
                                            FROM 
                                                PROFICIENCY_LEVELS PL, 
                                                COMPETENCY_PROFILE CP,
                                                ASSESSMENT_GROUP_ROLES AGR,
                                                EMPLOYEE E
                                            WHERE 
                                                PL.STATUS_CODE = @STATUS_CODE AND 
                                                CP.PROFICIENCY_SCHEME_ID = PL.SCHEME_ID AND 
                                                AGR.GROUP_ID = CP.GROUP_ID AND 
                                                E.ROLE_ID = AGR.ROLE_ID AND 
                                                E.EMPLOYEE_ID = @EMPLOYEE_ID; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public DataTable PopulateSupervisorCompetencyAssessmentMarks(string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT 
                                                CAD.SUPERVISOR_RATING, 
                                                CAD.SUPERVISOR_WEIGHT, 
                                                COUNT(CAD.SUPERVISOR_WEIGHT) AS 'WEIGHT_COUNT'
                                            FROM 
                                                EMPLOYEE_COMPITANCY_ASSESSMENT ECA,
                                                COMPETENCY_ASSESSMENT_DETAILS CAD
                                            WHERE
                                                ECA.ASSESSMENT_TOKEN = CAD.ASSESSMENT_TOKEN AND
                                                ECA.EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                ECA.ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                ECA.YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT
                                            GROUP BY 
                                                CAD.SUPERVISOR_RATING;
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public DataTable PopulateSupervisorComments(string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT 
                                                APPRASER_COMMENTS, 
                                                RECOMMENDATION, 
                                                TRAINING_NEEDS, 
                                                IS_FEEDBACK_SUBMITTED 
                                            FROM 
                                                ASSESSED_EMPLOYEES 
                                            WHERE
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public void UpdateSupervisorComments(string appraserComments, string recommendation, string trainingNeeds, Boolean isfeedbackSubmitted, string assessmentID, string employeeID, string yearOfAssessment, string grivanceComments)
        {
            mySqlCmd = new MySqlCommand();
            try
            {
                dataTable = new DataTable();

                mySqlCmd.Parameters.Add(new MySqlParameter("@APPRASER_COMMENTS", appraserComments));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RECOMMENDATION", recommendation));
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_NEEDS", trainingNeeds));
                mySqlCmd.Parameters.Add(new MySqlParameter("@GRIVANCE_COMMENTS", grivanceComments));

                if (isfeedbackSubmitted == true)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_FEEDBACK_SUBMITTED", Constants.CON_ACTIVE_STATUS));
                }
                else
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_FEEDBACK_SUBMITTED", Constants.CON_INACTIVE_STATUS));
                }

                
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            UPDATE 
                                                ASSESSED_EMPLOYEES 
                                            SET 
                                                APPRASER_COMMENTS = @APPRASER_COMMENTS, 
                                                RECOMMENDATION = @RECOMMENDATION, 
                                                TRAINING_NEEDS = @TRAINING_NEEDS, 
                                                IS_FEEDBACK_SUBMITTED = @IS_FEEDBACK_SUBMITTED, 
                                                GRIVANCE_COMMENTS = @GRIVANCE_COMMENTS 
                                            WHERE
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;

                mySqlCon.Open();

                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.ExecuteNonQuery();

                sMySqlString = String.Empty;
                mySqlCon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                //mySqlCmd = null;
            }
        }

        public DataTable PopulateEmployeeComments(string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT 
                                                IS_EMPLOYEE_AGREED,
                                                DISAGREEMENTS,
                                                GRIVANCE_COMMENTS 
                                            FROM 
                                                ASSESSED_EMPLOYEES 
                                            WHERE
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                mySqlDa.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                dataTable.Dispose();
            }
        }

        public void UpdateEmployeeComments(string disagreements, Boolean isEmployeeAgreed, string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();

                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@DISAGREEMENTS", disagreements));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@GRIVANCE_COMMENTS", grivanceComments));

                if (isEmployeeAgreed == true)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_EMPLOYEE_AGREED", Constants.CON_ACTIVE_STATUS));
                    //mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS));
                }
                else
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_EMPLOYEE_AGREED", Constants.CON_INACTIVE_STATUS));
                    //mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS));
                }

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            UPDATE 
                                                ASSESSED_EMPLOYEES 
                                            SET 
                                                IS_EMPLOYEE_AGREED = @IS_EMPLOYEE_AGREED,
                                                DISAGREEMENTS = @DISAGREEMENTS
                                            WHERE
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;

                mySqlCon.Open();

                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.ExecuteNonQuery();

                mySqlCon.Close();
                
                sMySqlString = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public DataTable PopulateCEOComments(string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT 
                                                CEO_COMMENTS,
                                                IS_INCREMENT_GRANTED,
                                                INCREMENT_PERCENTAGE,
                                                TO_BE_REVIEWED,
                                                REVIEW_MONTHS,
                                                IS_CONFRIMED,
                                                IS_TRAINING_END,
                                                IS_PROBATION_EXTENDED,
                                                EXTENDED_MONTHS 
                                            FROM 
                                                ASSESSED_EMPLOYEES 
                                            WHERE
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public void UpdateCEOComments(string cEOComments, Boolean isIncrementGranted, string incrementPrecentage, Boolean isToBeReviewed, string reviewMonths, Boolean isConfrimed, Boolean isTrainingEnd, Boolean isProbationExtended, string extendedMonths, string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                mySqlCmd.Parameters.Add(new MySqlParameter("@CEO_COMMENTS", cEOComments));
                
                if (isIncrementGranted)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_INCREMENT_GRANTED", Constants.CON_ACTIVE_STATUS));
                }
                else
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_INCREMENT_GRANTED", Constants.CON_INACTIVE_STATUS));
                }

                mySqlCmd.Parameters.Add(new MySqlParameter("@INCREMENT_PERCENTAGE", incrementPrecentage));

                if (isToBeReviewed)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TO_BE_REVIEWED", Constants.CON_ACTIVE_STATUS));
                }
                else
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TO_BE_REVIEWED", Constants.CON_INACTIVE_STATUS));
                }
                mySqlCmd.Parameters.Add(new MySqlParameter("@REVIEW_MONTHS", reviewMonths));


                if (isConfrimed)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_CONFRIMED", Constants.CON_ACTIVE_STATUS));
                }
                else
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_CONFRIMED", Constants.CON_INACTIVE_STATUS));
                }

                if (isTrainingEnd)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_TRAINING_END", Constants.CON_ACTIVE_STATUS));
                }
                else
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_TRAINING_END", Constants.CON_INACTIVE_STATUS));
                }

                if (isProbationExtended)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_PROBATION_EXTENDED", Constants.CON_ACTIVE_STATUS));
                }
                else
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_PROBATION_EXTENDED", Constants.CON_INACTIVE_STATUS));
                }

                mySqlCmd.Parameters.Add(new MySqlParameter("@EXTENDED_MONTHS", extendedMonths));

                string sMySqlString = @"                                            
                                            UPDATE 
                                                ASSESSED_EMPLOYEES 
                                            SET  
                                                CEO_COMMENTS = @CEO_COMMENTS,
                                                IS_INCREMENT_GRANTED = @IS_INCREMENT_GRANTED,
                                                INCREMENT_PERCENTAGE = @INCREMENT_PERCENTAGE,
                                                TO_BE_REVIEWED = @TO_BE_REVIEWED,
                                                REVIEW_MONTHS = @REVIEW_MONTHS,
                                                IS_CONFRIMED = @IS_CONFRIMED,
                                                IS_TRAINING_END = @IS_TRAINING_END,
                                                IS_PROBATION_EXTENDED = @IS_PROBATION_EXTENDED,
                                                EXTENDED_MONTHS = @EXTENDED_MONTHS
                                            WHERE
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;

                mySqlCon.Open();

                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.ExecuteNonQuery();

                mySqlCon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public void FinalizeCEOComments(string cEOComments, Boolean isIncrementGranted, string incrementPrecentage, Boolean isToBeReviewed, string reviewMonths, Boolean isConfrimed, Boolean isTrainingEnd, Boolean isProbationExtended, string extendedMonths, string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();



                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                mySqlCmd.Parameters.Add(new MySqlParameter("@CEO_COMMENTS", cEOComments));

                if (isIncrementGranted)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_INCREMENT_GRANTED", Constants.CON_ACTIVE_STATUS));
                }
                else
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_INCREMENT_GRANTED", Constants.CON_INACTIVE_STATUS));
                }

                mySqlCmd.Parameters.Add(new MySqlParameter("@INCREMENT_PERCENTAGE", incrementPrecentage));

                if (isToBeReviewed)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TO_BE_REVIEWED", Constants.CON_ACTIVE_STATUS));
                }
                else
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TO_BE_REVIEWED", Constants.CON_INACTIVE_STATUS));
                }
                mySqlCmd.Parameters.Add(new MySqlParameter("@REVIEW_MONTHS", reviewMonths));


                if (isConfrimed)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_CONFRIMED", Constants.CON_ACTIVE_STATUS));
                }
                else
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_CONFRIMED", Constants.CON_INACTIVE_STATUS));
                }

                if (isTrainingEnd)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_TRAINING_END", Constants.CON_ACTIVE_STATUS));
                }
                else
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_TRAINING_END", Constants.CON_INACTIVE_STATUS));
                }

                if (isProbationExtended)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_PROBATION_EXTENDED", Constants.CON_ACTIVE_STATUS));
                }
                else
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_PROBATION_EXTENDED", Constants.CON_INACTIVE_STATUS));
                }

                mySqlCmd.Parameters.Add(new MySqlParameter("@EXTENDED_MONTHS", extendedMonths));

                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_CEO_FINALIZED_STATUS));

                string sMySqlString = @"                                            
                                            UPDATE 
                                                ASSESSED_EMPLOYEES 
                                            SET  
                                                CEO_COMMENTS = @CEO_COMMENTS,
                                                IS_INCREMENT_GRANTED = @IS_INCREMENT_GRANTED,
                                                INCREMENT_PERCENTAGE = @INCREMENT_PERCENTAGE,
                                                TO_BE_REVIEWED = @TO_BE_REVIEWED,
                                                REVIEW_MONTHS = @REVIEW_MONTHS,
                                                IS_CONFRIMED = @IS_CONFRIMED,
                                                IS_TRAINING_END = @IS_TRAINING_END,
                                                IS_PROBATION_EXTENDED = @IS_PROBATION_EXTENDED,
                                                EXTENDED_MONTHS = @EXTENDED_MONTHS,
                                                STATUS_CODE = @STATUS_CODE
                                            WHERE
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;

                mySqlCon.Open();

                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.ExecuteNonQuery();

                mySqlCon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public string GetAppraser(string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT 
                                                APPRASER 
                                            FROM 
                                                ASSESSED_EMPLOYEES 
                                            WHERE
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT AND 
                                                ASSESSMENT_ID = @ASSESSMENT_ID; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["APPRASER"].ToString().Trim();
                }
                else
                {
                    return String.Empty;
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public string GetCEO(string employeeID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));

                string sMySqlString = @"                                            
                                            SELECT 
                                                REPORT_TO_3 
                                            FROM 
                                                EMPLOYEE 
                                            WHERE
                                                EMPLOYEE_ID = @EMPLOYEE_ID; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["REPORT_TO_3"].ToString().Trim();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public Boolean IsEmployeeAgreed(string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT
                                                IS_EMPLOYEE_AGREED 
                                            FROM  
                                                ASSESSED_EMPLOYEES
                                            WHERE
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows[0]["IS_EMPLOYEE_AGREED"].ToString().Trim() == Constants.CON_ACTIVE_STATUS)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public string EmployeeAgreedStatus(string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT
                                                IS_EMPLOYEE_AGREED 
                                            FROM  
                                                ASSESSED_EMPLOYEES
                                            WHERE
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["IS_EMPLOYEE_AGREED"].ToString().Trim();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public Boolean IsSupervisorCompleted(string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT
                                                STATUS_CODE 
                                            FROM  
                                                ASSESSED_EMPLOYEES
                                            WHERE
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows[0]["STATUS_CODE"].ToString().Trim() == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public Boolean IsSupervisorFinalized(string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT
                                                STATUS_CODE 
                                            FROM  
                                                ASSESSED_EMPLOYEES
                                            WHERE
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows[0]["STATUS_CODE"].ToString().Trim() == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public Boolean IsCEOFinalized(string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT
                                                STATUS_CODE 
                                            FROM  
                                                ASSESSED_EMPLOYEES
                                            WHERE
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows[0]["STATUS_CODE"].ToString().Trim() == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public Boolean IsClosedOrObsolete(string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT
                                                STATUS_CODE 
                                            FROM  
                                                ASSESSED_EMPLOYEES
                                            WHERE
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    if ((dataTable.Rows[0]["STATUS_CODE"].ToString().Trim() == Constants.ASSESSNEMT_CLOSED_STATUS) || (dataTable.Rows[0]["STATUS_CODE"].ToString().Trim() == Constants.ASSESSNEMT_OBSOLETE_STATUS))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public void SupervisorCompleteEvaluation(string assessmentID, string employeeID, string yearOfAssessment)
        {
            mySqlCon.Open();
            MySqlTransaction oMySqlTransaction;
            oMySqlTransaction = mySqlCon.BeginTransaction();

            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();


                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS));

                string sMySqlString = @"                                            
                                            UPDATE 
                                                ASSESSED_EMPLOYEES 
                                            SET 
                                                STATUS_CODE = @STATUS_CODE, 
                                                IS_EMPLOYEE_AGREED = NULL 
                                            WHERE 
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                sMySqlString = @"                                            
                                            UPDATE 
                                                EMPLOYEE_GOAL_ASSESSMENT 
                                            SET 
                                                STATUS_CODE = @STATUS_CODE, 
                                                SUPERVISOR_COMPLETED_DATE = NOW() 
                                            WHERE
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                sMySqlString = @"                                            
                                            UPDATE 
                                                EMPLOYEE_COMPITANCY_ASSESSMENT 
                                            SET 
                                                STATUS_CODE = @STATUS_CODE, 
                                                SUPERVISOR_COMPLETED_DATE = NOW()  
                                            WHERE
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();


                sMySqlString = @"                                            
                                            UPDATE 
                                                EMPLOYEE_SELF_ASSESSMENT 
                                            SET 
                                                STATUS_CODE = @STATUS_CODE 
                                            WHERE
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                oMySqlTransaction.Commit();

                mySqlCon.Close();
            }
            catch (Exception ex)
            {
                oMySqlTransaction.Rollback();
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                mySqlCmd.Parameters.Clear();
            }
        }

        public void SupervisorFinalizeEvaluation(string assessmentID, string employeeID, string yearOfAssessment,string CompetencyScore, string KPIScore, string Total)
        {
            mySqlCon.Open();
            MySqlTransaction oMySqlTransaction;
            oMySqlTransaction = mySqlCon.BeginTransaction();

            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMPITANCY_SCORE", CompetencyScore));
                mySqlCmd.Parameters.Add(new MySqlParameter("@KPI_SCORE", KPIScore));
                mySqlCmd.Parameters.Add(new MySqlParameter("@TOTAL", Total));

                string sMySqlString = @"                                            
                                            UPDATE 
                                                ASSESSED_EMPLOYEES 
                                            SET 
                                                STATUS_CODE = @STATUS_CODE, 
                                                COMPITANCY_SCORE = @COMPITANCY_SCORE, 
                                                KPI_SCORE = @KPI_SCORE, 
                                                TOTAL = @TOTAL 
                                            WHERE 
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                sMySqlString = @"                                            
                                            UPDATE 
                                                EMPLOYEE_GOAL_ASSESSMENT 
                                            SET 
                                                STATUS_CODE = @STATUS_CODE  
                                            WHERE
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                sMySqlString = @"                                            
                                            UPDATE 
                                                EMPLOYEE_COMPITANCY_ASSESSMENT 
                                            SET 
                                                STATUS_CODE = @STATUS_CODE 
                                            WHERE
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();


                sMySqlString = @"                                            
                                            UPDATE 
                                                EMPLOYEE_SELF_ASSESSMENT 
                                            SET 
                                                STATUS_CODE = @STATUS_CODE 
                                            WHERE
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                oMySqlTransaction.Commit();

                mySqlCon.Close();
            }
            catch (Exception ex)
            {
                oMySqlTransaction.Rollback();
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                mySqlCmd.Parameters.Clear();
            }
        }

        public void CEOFinalizeEvaluation(string assessmentID, string employeeID, string yearOfAssessment)
        {
            mySqlCon.Open();
            MySqlTransaction oMySqlTransaction;
            oMySqlTransaction = mySqlCon.BeginTransaction();

            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();


                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_CEO_FINALIZED_STATUS));

                string sMySqlString = @"                                            
                                            UPDATE 
                                                ASSESSED_EMPLOYEES 
                                            SET 
                                                STATUS_CODE = @STATUS_CODE 
                                            WHERE 
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                sMySqlString = @"                                            
                                            UPDATE 
                                                EMPLOYEE_GOAL_ASSESSMENT 
                                            SET 
                                                STATUS_CODE = @STATUS_CODE  
                                            WHERE
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                sMySqlString = @"                                            
                                            UPDATE 
                                                EMPLOYEE_COMPITANCY_ASSESSMENT 
                                            SET 
                                                STATUS_CODE = @STATUS_CODE 
                                            WHERE
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();


                sMySqlString = @"                                            
                                            UPDATE 
                                                EMPLOYEE_SELF_ASSESSMENT 
                                            SET 
                                                STATUS_CODE = @STATUS_CODE 
                                            WHERE
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                oMySqlTransaction.Commit();

                mySqlCon.Close();
            }
            catch (Exception ex)
            {
                oMySqlTransaction.Rollback();
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                mySqlCmd.Parameters.Clear();
            }
        }

        public string GetEmailAddress(string EmployeeId)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeId));

                string sMySqlString = @"                                            
                                            SELECT 
                                                EMAIL 
                                            FROM 
                                                EMPLOYEE 
                                            WHERE 
                                                EMPLOYEE_ID = @EMPLOYEE_ID; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["EMAIL"].ToString().Trim();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public string GetEmployeeName(string EmployeeId)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeId));

                string sMySqlString = @"                                            
                                            SELECT 
                                                TITLE, 
                                                INITIALS_NAME 
                                            FROM 
                                                EMPLOYEE 
                                            WHERE 
                                                EMPLOYEE_ID = @EMPLOYEE_ID; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["TITLE"].ToString().Trim() + " " + dataTable.Rows[0]["INITIALS_NAME"].ToString().Trim();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public string GetCutOffDate(string AssessmentID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", AssessmentID));

                string sMySqlString = @"                                            
                                           SELECT 
                                                CONVERT( EXPECTED_COMPLETION_DATE , CHAR) AS 'EXPECTED_COMPLETION_DATE'
                                            FROM
                                                ASSESSMENT
                                            WHERE
                                                ASSESSMENT_ID = @ASSESSMENT_ID; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["EXPECTED_COMPLETION_DATE"].ToString().Trim();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }
    }
}
