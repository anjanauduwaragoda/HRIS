using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class SupervisorSelfAssessmentReviewDatahandler : TemplateDataHandler
    {
        public DataTable PopulateSelfAssessmentDetails(string assessmentID,string yearOfAssessment,string employeeID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));

                string sMySqlString = @"                                            
                                            SELECT 
                                                ESA.ASSESSMENT_TOKEN, 
                                                ESA.SELF_ASSESSMENT_PROFILE_ID, 
                                                ESA.STATUS_CODE 
                                            FROM 
                                                EMPLOYEE_SELF_ASSESSMENT ESA 
                                            WHERE 
                                                ESA.YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT AND 
                                                ESA.ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                ESA.EMPLOYEE_ID = @EMPLOYEE_ID;                                           
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

        public DataTable PopulateAnswersWithRequiredAnswerCount(string selfAssessmentProfileID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@SELF_ASSESSMENT_PROFILE_ID", selfAssessmentProfileID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));

                string sMySqlString = @"                                            
                                            SELECT 
                                                SAQB.QUESTION_ID, 
                                                QB.QUESTION, 
                                                SAQB.NO_OF_ANSWERS 
                                            FROM 
                                                SELF_ASSESSMENT_PROFILE_QUESTIONS_BANK SAQB,
                                                QUESTIONNAIRE_BANK QB
                                            WHERE 
                                                SAQB.SELF_ASSESSMENT_PROFILE_ID = @SELF_ASSESSMENT_PROFILE_ID AND 
                                                SAQB.STATUS_CODE = QB.STATUS_CODE AND 
                                                SAQB.STATUS_CODE = @STATUS_CODE AND
                                                QB.QUESTION_ID = SAQB.QUESTION_ID; 
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

        public DataTable PopulateEmployeeAnswers(string assessmentToken)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", assessmentToken));

                string sMySqlString = @"                                            
                                            SELECT 
                                                SAA.SELF_ASSESSMENT_ANSWER_ID,
                                                SAA.QUESTION_ID,
                                                SAA.ANSWER 
                                            FROM 
                                                SELF_ASSESSMENT_ANSWERS SAA 
                                            WHERE 
                                                SAA.ASSESSMENT_TOKEN = @ASSESSMENT_TOKEN; 
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

        public DataTable PopulatePreviousComments(string assessmentID, string EmployeeID, string YearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", YearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT 
                                                COMMENTS 
                                            FROM 
                                                EMPLOYEE_SELF_ASSESSMENT 
                                            WHERE 
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND EMPLOYEE_ID = @EMPLOYEE_ID AND YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
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

        public Boolean ActiveStatus(string assessmentID)
        {
            Boolean Status = true;
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));

                string sMySqlString = @"                                            
                                            SELECT 
                                                STATUS_CODE 
                                            FROM 
                                                EMPLOYEE_SELF_ASSESSMENT 
                                            WHERE 
                                                ASSESSMENT_ID = @ASSESSMENT_ID; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows[0]["STATUS_CODE"].ToString() == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        Status = true;
                    }
                    else
                    {
                        Status = false;
                    }
                }
                else
                {
                    Status = false;
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
            return Status;
        }

        public DataTable Insert(string assessmentID, string EmployeeID, string yearOfAssessment, string comment, string supervisorEmployeeID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMMENTS", comment));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", supervisorEmployeeID));

                string sMySqlString = @"                                            
                                            UPDATE 
                                                EMPLOYEE_SELF_ASSESSMENT 
                                            SET 
                                                COMMENTS = @COMMENTS, 
                                                STATUS_CODE = @STATUS_CODE,
                                                MODIFIED_BY = @MODIFIED_BY, 
                                                MODIFIED_DATE = NOW()  
                                            WHERE 
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID; 
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

        public DataTable Complete(string assessmentID,string empId, string comment, string supervisorEmployeeID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMMENTS", comment));
                mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", supervisorEmployeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId));

                string sMySqlString = @"                                            
                                            UPDATE 
                                                EMPLOYEE_SELF_ASSESSMENT 
                                            SET 
                                                COMMENTS = @COMMENTS,
                                                STATUS_CODE = @STATUS_CODE,  
                                                MODIFIED_BY = @MODIFIED_BY, 
                                                MODIFIED_DATE = NOW()  
                                            WHERE 
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND EMPLOYEE_ID = @empId; 
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

        public DataTable InsertAndFinalize(string assessmentID, string comment, string supervisorEmployeeID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMMENTS", comment));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", supervisorEmployeeID));

                string sMySqlString = @"                                            
                                            UPDATE 
                                                EMPLOYEE_SELF_ASSESSMENT 
                                            SET 
                                                COMMENTS = @COMMENTS, 
                                                STATUS_CODE = @STATUS_CODE, 
                                                MODIFIED_BY = @MODIFIED_BY, 
                                                MODIFIED_DATE = NOW()  
                                            WHERE 
                                                ASSESSMENT_ID = @ASSESSMENT_ID; 
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

        public DataTable Finalize(string assessmentID, string comment, string supervisorEmployeeID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", supervisorEmployeeID));

                string sMySqlString = @"                                            
                                            UPDATE 
                                                EMPLOYEE_SELF_ASSESSMENT 
                                            SET 
                                                STATUS_CODE = @STATUS_CODE, 
                                                MODIFIED_BY = @MODIFIED_BY, 
                                                MODIFIED_DATE = NOW()  
                                            WHERE 
                                                ASSESSMENT_ID = @ASSESSMENT_ID;
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

        public Boolean IsFinalized(string assessmentID, string employeeID, string yearOfAssessment)
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
                                                EMPLOYEE_SELF_ASSESSMENT
                                            WHERE
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT; 
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows[0]["STATUS_CODE"].ToString().Trim() == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        return false;
                    }
                    else if (dataTable.Rows[0]["STATUS_CODE"].ToString().Trim() == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
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

        public string GetSelfAssessmentStatus(string assessmentID, string yearOfAssessment, string employeeID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));

                string sMySqlString = @"                                            
                                            SELECT 
                                                STATUS_CODE 
                                            FROM 
                                                EMPLOYEE_SELF_ASSESSMENT 
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
                    return dataTable.Rows[0]["STATUS_CODE"].ToString();
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

        public string GetSupervisorName(string assessmentID, string yearOfAssessment, string employeeID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));

                string sMySqlString = @"                                            
                                            SELECT 
                                                E.TITLE, 
                                                E.INITIALS_NAME 
                                            FROM 
                                                ASSESSED_EMPLOYEES AE,
                                                EMPLOYEE E
                                            WHERE 
                                                E.EMPLOYEE_ID = AE.APPRASER AND 
                                                AE.EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                AE.ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                AE.YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT;                                           
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Connection = mySqlCon;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["TITLE"].ToString() + " " + dataTable.Rows[0]["INITIALS_NAME"].ToString();
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
