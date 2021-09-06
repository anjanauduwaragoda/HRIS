using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class CEOCompetencyAssessmentDataHandler : TemplateDataHandler
    {

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

        public DataTable PopulateSubordinatesInfo(string employeeID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));

                string sMySqlString = @"                                            
                                            SELECT 
                                                E.TITLE, 
                                                E.INITIALS_NAME, 
                                                ED.DESIGNATION_NAME, 
                                                ER.ROLE_NAME 
                                            FROM 
                                                EMPLOYEE E, 
                                                EMPLOYEE_DESIGNATION ED,
                                                EMPLOYEE_ROLE ER 
                                            WHERE
                                                E.EMPLOYEE_ID = @EMPLOYEE_ID AND
                                                E.DESIGNATION_ID = ED.DESIGNATION_ID AND 
                                                E.ROLE_ID = ER.ROLE_ID; 
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

        public DataTable Populate(string assessmentID, string employeeID, string yearOfAssessment)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT 
                                                CAD.ASSESSMENT_TOKEN,
                                                CAD.COMPETENCY_ID,
                                                CB.COMPETENCY_NAME,
                                                CB.DESCRIPTION, 
                                                PC.EXPECTED_PROFICIENCY_RATING,
                                                PC.EXPECTED_PROFICIENCY_WEIGHT,
                                                CAD.EMPLOYEE_RATING,
                                                CAD.EMPLOYEE_WEIGHT,
                                                CAD.SUPERVISOR_RATING,
                                                CAD.SUPERVISOR_WEIGHT,
                                                ECA.COMPETENCY_PROFILE_ID,
                                                ECA.COMMENTS  
                                            FROM 
                                                EMPLOYEE_COMPITANCY_ASSESSMENT ECA,
                                                COMPETENCY_ASSESSMENT_DETAILS CAD,
                                                COMPETENCY_BANK CB,
                                                PROFILE_COMPETENCIES PC
                                            WHERE
                                                CAD.COMPETENCY_ID = PC.COMPETENCY_ID AND 
                                                PC.COMPETENCY_PROFILE_ID = ECA.COMPETENCY_PROFILE_ID AND 
                                                ECA.ASSESSMENT_TOKEN = CAD.ASSESSMENT_TOKEN AND 
                                                CB.COMPETENCY_ID = CAD.COMPETENCY_ID AND
                                                PC.STATUS_CODE = @STATUS_CODE AND 
                                                CB.STATUS_CODE = @STATUS_CODE AND 
                                                ECA.ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                ECA.EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                ECA.YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT
                                            ORDER BY
                                                COMPETENCY_NAME ASC; 
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

        public DataTable PopulateRatings(string assessmentID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));

                string sMySqlString = @"                                            
                                            SELECT 
                                                PL.RATING,
                                                PL.WEIGHT 
                                            FROM  
                                                PROFICIENCY_SCHEME PS,
                                                COMPETENCY_PROFILE CP,
                                                PROFICIENCY_LEVELS PL,
                                                EMPLOYEE_COMPITANCY_ASSESSMENT ECA
                                            WHERE
                                                PS.SCHEME_ID = PL.SCHEME_ID AND 
                                                PS.SCHEME_ID = CP.PROFICIENCY_SCHEME_ID AND
                                                CP.COMPETENCY_PROFILE_ID = ECA.COMPETENCY_PROFILE_ID AND
                                                ECA.ASSESSMENT_ID = @ASSESSMENT_ID AND
                                                PL.STATUS_CODE = @STATUS_CODE AND 
                                                CP.STATUS_CODE = @STATUS_CODE AND 
                                                PS.STATUS_CODE = @STATUS_CODE; 
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

        public Boolean Insert(string totalSupervisorScore, string comments, string competencyProfileID, string assessmentToken, string assessmentID, string subordinatesEmployeeID, string yearOfAssessment, string modifiedBy, DataTable dtEmployeeCompetencies)
        {
            Boolean Status = false;

            try
            {

                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                SerialHandler serialHandler = new SerialHandler();
                string EmployeeGoalID = String.Empty;

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    for (int i = 0; i < dtEmployeeCompetencies.Rows.Count; i++)
                    {

                        string SupervisorRating = dtEmployeeCompetencies.Rows[i]["SUPERVISOR_RATING"].ToString();
                        string SupervisorWeight = dtEmployeeCompetencies.Rows[i]["SUPERVISOR_WEIGHT"].ToString();
                        string CompetencyID = dtEmployeeCompetencies.Rows[i]["COMPETENCY_ID"].ToString();

                        string DetailQuery = @"
                                                UPDATE 
                                                    COMPETENCY_ASSESSMENT_DETAILS 
                                                SET 
                                                    SUPERVISOR_RATING = @SUPERVISOR_RATING, 
                                                    SUPERVISOR_WEIGHT = @SUPERVISOR_WEIGHT  
                                                WHERE 
                                                    ASSESSMENT_TOKEN = @ASSESSMENT_TOKEN AND 
                                                    COMPETENCY_ID = @COMPETENCY_ID;
                                            ";

                        mySqlCmd.Parameters.Add(new MySqlParameter("@SUPERVISOR_RATING", SupervisorRating.Trim() == "" ? (object)DBNull.Value : SupervisorRating.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@SUPERVISOR_WEIGHT", SupervisorWeight.Trim() == "" ? (object)DBNull.Value : SupervisorWeight.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@COMPETENCY_ID", CompetencyID.Trim() == "" ? (object)DBNull.Value : CompetencyID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", assessmentToken.Trim() == "" ? (object)DBNull.Value : assessmentToken.Trim()));

                        mySqlCmd.CommandText = DetailQuery;
                        mySqlCmd.ExecuteNonQuery();

                        mySqlCmd.Parameters.Clear();
                    }

                    string HeaderQuery = @"
                                            UPDATE 
                                                EMPLOYEE_COMPITANCY_ASSESSMENT 
                                            SET 
                                                TOTAL_SUPERVISOR_SCORE = @TOTAL_SUPERVISOR_SCORE, 
                                                COMMENTS = @COMMENTS, 
                                                MODIFIED_BY = @MODIFIED_BY, 
                                                MODIFIED_DATE = NOW()
                                            WHERE 
                                                COMPETENCY_PROFILE_ID = @COMPETENCY_PROFILE_ID AND 
                                                ASSESSMENT_TOKEN = @ASSESSMENT_TOKEN AND 
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT;
                                        ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@TOTAL_SUPERVISOR_SCORE", totalSupervisorScore.Trim() == "" ? (object)DBNull.Value : totalSupervisorScore.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@COMMENTS", comments.Trim() == "" ? (object)DBNull.Value : comments.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", modifiedBy.Trim() == "" ? (object)DBNull.Value : modifiedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@COMPETENCY_PROFILE_ID", competencyProfileID.Trim() == "" ? (object)DBNull.Value : competencyProfileID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", assessmentToken.Trim() == "" ? (object)DBNull.Value : assessmentToken.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID.Trim() == "" ? (object)DBNull.Value : assessmentID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", subordinatesEmployeeID.Trim() == "" ? (object)DBNull.Value : subordinatesEmployeeID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment.Trim() == "" ? (object)DBNull.Value : yearOfAssessment.Trim()));

                    mySqlCmd.CommandText = HeaderQuery;
                    mySqlCmd.ExecuteNonQuery();

                    mySqlCmd.Parameters.Clear();

                    oMySqlTransaction.Commit();
                }
                catch (Exception exp)
                {
                    oMySqlTransaction.Rollback();
                    throw exp;
                }
                Status = true;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return Status;
        }

        public Boolean Finalize(string totalSupervisorScore, string competencyProfileID, string assessmentToken, string assessmentID, string subordinatesEmployeeID, string yearOfAssessment, string modifiedBy, DataTable dtEmployeeCompetencies)
        {
            Boolean Status = false;

            try
            {

                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                SerialHandler serialHandler = new SerialHandler();
                string EmployeeGoalID = String.Empty;

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    for (int i = 0; i < dtEmployeeCompetencies.Rows.Count; i++)
                    {

                        string SupervisorRating = dtEmployeeCompetencies.Rows[i]["SUPERVISOR_RATING"].ToString();
                        string SupervisorWeight = dtEmployeeCompetencies.Rows[i]["SUPERVISOR_WEIGHT"].ToString();
                        string CompetencyID = dtEmployeeCompetencies.Rows[i]["COMPETENCY_ID"].ToString();

                        string DetailQuery = @"
                                                UPDATE 
                                                    COMPETENCY_ASSESSMENT_DETAILS 
                                                SET 
                                                    SUPERVISOR_RATING = @SUPERVISOR_RATING, 
                                                    SUPERVISOR_WEIGHT = @SUPERVISOR_WEIGHT  
                                                WHERE 
                                                    ASSESSMENT_TOKEN = @ASSESSMENT_TOKEN AND 
                                                    COMPETENCY_ID = @COMPETENCY_ID;
                                            ";

                        mySqlCmd.Parameters.Add(new MySqlParameter("@SUPERVISOR_RATING", SupervisorRating.Trim() == "" ? (object)DBNull.Value : SupervisorRating.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@SUPERVISOR_WEIGHT", SupervisorWeight.Trim() == "" ? (object)DBNull.Value : SupervisorWeight.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@COMPETENCY_ID", CompetencyID.Trim() == "" ? (object)DBNull.Value : CompetencyID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", assessmentToken.Trim() == "" ? (object)DBNull.Value : assessmentToken.Trim()));

                        mySqlCmd.CommandText = DetailQuery;
                        mySqlCmd.ExecuteNonQuery();

                        mySqlCmd.Parameters.Clear();
                    }

                    string HeaderQuery = @"
                                            UPDATE 
                                                EMPLOYEE_COMPITANCY_ASSESSMENT 
                                            SET 
                                                TOTAL_SUPERVISOR_SCORE = @TOTAL_SUPERVISOR_SCORE,
                                                SUPERVISOR_COMPLETED_DATE = NOW(),
                                                STATUS_CODE = @STATUS_CODE,
                                                MODIFIED_BY = @MODIFIED_BY, 
                                                MODIFIED_DATE = NOW()
                                            WHERE 
                                                COMPETENCY_PROFILE_ID = @COMPETENCY_PROFILE_ID AND 
                                                ASSESSMENT_TOKEN = @ASSESSMENT_TOKEN AND 
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT;
                                        ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_CEO_FINALIZED_STATUS.Trim() == "" ? (object)DBNull.Value : Constants.ASSESSNEMT_CEO_FINALIZED_STATUS.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TOTAL_SUPERVISOR_SCORE", totalSupervisorScore.Trim() == "" ? (object)DBNull.Value : totalSupervisorScore.Trim()));
                    //mySqlCmd.Parameters.Add(new MySqlParameter("@COMMENTS", comments.Trim() == "" ? (object)DBNull.Value : comments.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", modifiedBy.Trim() == "" ? (object)DBNull.Value : modifiedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@COMPETENCY_PROFILE_ID", competencyProfileID.Trim() == "" ? (object)DBNull.Value : competencyProfileID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", assessmentToken.Trim() == "" ? (object)DBNull.Value : assessmentToken.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID.Trim() == "" ? (object)DBNull.Value : assessmentID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", subordinatesEmployeeID.Trim() == "" ? (object)DBNull.Value : subordinatesEmployeeID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment.Trim() == "" ? (object)DBNull.Value : yearOfAssessment.Trim()));

                    mySqlCmd.CommandText = HeaderQuery;
                    mySqlCmd.ExecuteNonQuery();

                    mySqlCmd.Parameters.Clear();

                    oMySqlTransaction.Commit();
                }
                catch (Exception exp)
                {
                    oMySqlTransaction.Rollback();
                    throw exp;
                }
                Status = true;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return Status;
        }

        public Boolean IsSupervisorFinalized(string competencyProfileID, string assessmentToken, string assessmentID, string subordinatesEmployeeID, string yearOfAssessment)
        {
            Boolean status = false;
            try
            {
                dataTable = new DataTable();
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@COMPETENCY_PROFILE_ID", competencyProfileID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", assessmentToken));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", subordinatesEmployeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment));

                string sMySqlString = @"                                            
                                            SELECT 
                                                STATUS_CODE 
                                            FROM 
                                                EMPLOYEE_COMPITANCY_ASSESSMENT 
                                            WHERE
                                                COMPETENCY_PROFILE_ID = @COMPETENCY_PROFILE_ID AND
                                                ASSESSMENT_TOKEN = @ASSESSMENT_TOKEN AND
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
                    string statusCode = dataTable.Rows[0]["STATUS_CODE"].ToString();

                    if (statusCode == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
                }
                else
                {
                    status = false;
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
            return status;
        }
    
    }
}
