using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace DataHandler.PerformanceManagement
{
    public class SupervisorGoalAssessmentDataHandler : TemplateDataHandler
    {
        public DataTable Populate(string AssessmentID, string EmployeeID, string YearOfGoal)
        {
            dataTable = new DataTable();
            mySqlCmd = new MySqlCommand();
            try
            {
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"                                            
                                            SELECT 
                                                GAD.ASSESSMENT_TOKEN,
                                                GAD.GOAL_ID,
                                                EG.GOAL_AREA,
                                                EG.MEASUREMENTS,
                                                EG.WEIGHT AS 'EXPECTED_WEIGHT',
                                                GAD.EMPLOYEE_SELF_SCORE,
                                                GAD.SUPERVISOR_SCORE,
                                                EGA.COMMENTS,
	                                            EGA.STATUS_CODE
                                            FROM  
                                                GOAL_ASSESSMENT_DETAILS GAD,
                                                EMPLOYEE_GOAL_ASSESSMENT EGA,
                                                EMPLOYEE_GOALS EG
                                            WHERE
                                                GAD.ASSESSMENT_TOKEN = EGA.ASSESSMENT_TOKEN AND
                                                EG.EMPLOYEE_ID = EGA.EMPLOYEE_ID AND
                                                EG.GOAL_ID = GAD.GOAL_ID AND
                                                EGA.ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EGA.EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                EGA.YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT AND
                                                EG.EMPLOYEE_AGREE = @EMPLOYEE_AGREE AND
                                                EG.SUPERVISOR_AGREE = @SUPERVISOR_AGREE AND
                                                EG.IS_LOCKED = @IS_LOCKED
                                            ORDER BY
                                                EG.WEIGHT DESC;
                                        ";

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", AssessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", YearOfGoal));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_AGREE", Constants.CON_ACTIVE_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@SUPERVISOR_AGREE", Constants.CON_ACTIVE_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@IS_LOCKED", Constants.CON_ACTIVE_STATUS));
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

        public DataTable PopulatePreviousData(string EmployeeID)
        {
            dataTable = new DataTable();
            mySqlCmd = new MySqlCommand();
            try
            {
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"                                            
                                            SELECT 
                                                CONVERT( SUPERVISOR_COMPLETED_DATE , CHAR) AS 'SUPERVISOR_COMPLETED_DATE',
                                                CONVERT( TOTAL_SUPERVISORS_SCORE , CHAR) AS 'TOTAL_SUPERVISOR_SCORE'
                                            FROM
                                                EMPLOYEE_GOAL_ASSESSMENT
                                            WHERE
                                                EMPLOYEE_ID = @EMPLOYEE_ID
                                                    AND TOTAL_SUPERVISORS_SCORE IS NOT NULL
                                                    AND SUPERVISOR_COMPLETED_DATE IS NOT NULL
                                                    AND SUPERVISOR_COMPLETED_DATE >= DATE_SUB(NOW(), INTERVAL 5 YEAR)
                                            ORDER BY SUPERVISOR_COMPLETED_DATE ASC;
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

        public Boolean IsSubordinateFinalized(string AssessmentID, string EmployeeID, string YearOfAssessment)
        {
            Boolean Status = false;
            mySqlCmd = new MySqlCommand();

            try
            {
                mySqlCmd.Connection = mySqlCon;

                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", AssessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", YearOfAssessment));

                string Qry = @"
                                    SELECT 
                                        STATUS_CODE 
                                    FROM 
                                        EMPLOYEE_GOAL_ASSESSMENT 
                                    WHERE 
                                        ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                        EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                        YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT
                              ";

                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter da = new MySqlDataAdapter(mySqlCmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    string GoalAssessmentStatus = dt.Rows[0]["STATUS_CODE"].ToString();

                    if ((GoalAssessmentStatus == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS) || (GoalAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS))
                    {
                        Status = false;
                    }
                    else
                    {
                        Status = true;
                    }
                }
                else
                {
                    Status = true;
                }

                return Status;
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

        public void Complete(string AssessmentToken, string Comments, string ModifiedBy, string SubordinatesEmployeeID, string AssessmentID, string YearOfAssessment, DataTable GoalAssessmentDetails)
        {
            MySqlTransaction oMySqlTransaction;
            string CommandText = String.Empty;
            try
            {
                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;
                oMySqlTransaction = mySqlCon.BeginTransaction();

                double TotalSupervisorScore = 0.0;

                for (int i = 0; i < GoalAssessmentDetails.Rows.Count; i++)
                {
                    mySqlCmd.Parameters.Clear();

                    double SupervisorScore = 0.0;
                    string GoalID = String.Empty;

                    if (Double.TryParse(GoalAssessmentDetails.Rows[i]["SUPERVISOR_SCORE"].ToString(), out SupervisorScore))
                    {
                        TotalSupervisorScore += SupervisorScore;
                    }
                    GoalID = GoalAssessmentDetails.Rows[i]["GOAL_ID"].ToString();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@SUPERVISOR_SCORE", SupervisorScore.ToString().Trim() == "" ? (object)DBNull.Value : SupervisorScore.ToString().Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", AssessmentToken.ToString().Trim() == "" ? (object)DBNull.Value : AssessmentToken.ToString().Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@GOAL_ID", GoalID.ToString().Trim() == "" ? (object)DBNull.Value : GoalID.ToString().Trim()));

                    CommandText = @"
                                        UPDATE 
                                            GOAL_ASSESSMENT_DETAILS 
                                        SET 
                                            SUPERVISOR_SCORE = @SUPERVISOR_SCORE 
                                        WHERE 
                                            ASSESSMENT_TOKEN = @ASSESSMENT_TOKEN AND 
                                            GOAL_ID = @GOAL_ID
                                  ";

                    mySqlCmd.CommandText = CommandText;
                    mySqlCmd.ExecuteNonQuery();

                }


                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@TOTAL_SUPERVISORS_SCORE", TotalSupervisorScore.ToString().Trim() == "" ? (object)DBNull.Value : TotalSupervisorScore.ToString().Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMMENTS", Comments.ToString().ToString().Trim() == "" ? (object)DBNull.Value : Comments.ToString().Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS.ToString().Trim() == "" ? (object)DBNull.Value : Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS.ToString().Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", ModifiedBy.ToString().Trim() == "" ? (object)DBNull.Value : ModifiedBy.ToString().Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", SubordinatesEmployeeID.ToString().Trim() == "" ? (object)DBNull.Value : SubordinatesEmployeeID.ToString().Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", AssessmentID.ToString().Trim() == "" ? (object)DBNull.Value : AssessmentID.ToString().Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", YearOfAssessment.ToString().Trim() == "" ? (object)DBNull.Value : YearOfAssessment.ToString().Trim()));

                CommandText = @"
                                    UPDATE 
                                        EMPLOYEE_GOAL_ASSESSMENT 
                                    SET 
                                        TOTAL_SUPERVISORS_SCORE = @TOTAL_SUPERVISORS_SCORE,
                                        SUPERVISOR_COMPLETED_DATE = NOW(),  
                                        COMMENTS = @COMMENTS, 
                                        STATUS_CODE = @STATUS_CODE, 
                                        MODIFIED_BY = @MODIFIED_BY, 
                                        MODIFIED_DATE = NOW() 
                                    WHERE 
                                        EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                        ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                        YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT;
                               ";
                mySqlCmd.CommandText = CommandText;
                mySqlCmd.ExecuteNonQuery();


                oMySqlTransaction.Commit();
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                oMySqlTransaction = null;
            }
        }

        public void Insert(string AssessmentToken, string Comments, string ModifiedBy, string SubordinatesEmployeeID, string AssessmentID, string YearOfAssessment, DataTable GoalAssessmentDetails)
        {
            MySqlTransaction oMySqlTransaction;
            string CommandText = String.Empty;
            try
            {
                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;
                oMySqlTransaction = mySqlCon.BeginTransaction();

                double TotalSupervisorScore = 0.0;

                for (int i = 0; i < GoalAssessmentDetails.Rows.Count; i++)
                {
                    mySqlCmd.Parameters.Clear();

                    double SupervisorScore = 0.0; ;
                    string GoalID = String.Empty;
                    if (GoalAssessmentDetails.Rows[i]["SUPERVISOR_SCORE"].ToString() != "")
                    {
                        if (Double.TryParse(GoalAssessmentDetails.Rows[i]["SUPERVISOR_SCORE"].ToString(), out SupervisorScore))
                        {
                            TotalSupervisorScore += SupervisorScore;
                        }
                        mySqlCmd.Parameters.Add(new MySqlParameter("@SUPERVISOR_SCORE", SupervisorScore.ToString().Trim() == "" ? (object)DBNull.Value : SupervisorScore.ToString().Trim()));
                    }
                    else
                    {
                        mySqlCmd.Parameters.Add(new MySqlParameter("@SUPERVISOR_SCORE", String.Empty == "" ? (object)DBNull.Value : String.Empty));
                    }
                    GoalID = GoalAssessmentDetails.Rows[i]["GOAL_ID"].ToString();

                    
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", AssessmentToken.ToString().Trim() == "" ? (object)DBNull.Value : AssessmentToken.ToString().Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@GOAL_ID", GoalID.ToString().Trim() == "" ? (object)DBNull.Value : GoalID.ToString().Trim()));

                    CommandText = @"
                                        UPDATE 
                                            GOAL_ASSESSMENT_DETAILS 
                                        SET 
                                            SUPERVISOR_SCORE = @SUPERVISOR_SCORE 
                                        WHERE 
                                            ASSESSMENT_TOKEN = @ASSESSMENT_TOKEN AND 
                                            GOAL_ID = @GOAL_ID
                                  ";

                    mySqlCmd.CommandText = CommandText;
                    mySqlCmd.ExecuteNonQuery();

                }


                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@TOTAL_SUPERVISORS_SCORE", TotalSupervisorScore.ToString().Trim() == "" ? (object)DBNull.Value : TotalSupervisorScore.ToString().Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMMENTS", Comments.ToString().Trim() == "" ? (object)DBNull.Value : Comments.ToString().Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS.ToString().Trim() == "" ? (object)DBNull.Value : Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS.ToString().Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", ModifiedBy.ToString().Trim() == "" ? (object)DBNull.Value : ModifiedBy.ToString().Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", SubordinatesEmployeeID.ToString().Trim() == "" ? (object)DBNull.Value : SubordinatesEmployeeID.ToString().Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", AssessmentID.ToString().Trim() == "" ? (object)DBNull.Value : AssessmentID.ToString().Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", YearOfAssessment.ToString().Trim() == "" ? (object)DBNull.Value : YearOfAssessment.ToString().Trim()));

                CommandText = @"
                                    UPDATE 
                                        EMPLOYEE_GOAL_ASSESSMENT 
                                    SET 
                                        TOTAL_SUPERVISORS_SCORE = @TOTAL_SUPERVISORS_SCORE, 
                                        COMMENTS = @COMMENTS, 
                                        STATUS_CODE = @STATUS_CODE, 
                                        MODIFIED_BY = @MODIFIED_BY, 
                                        MODIFIED_DATE = NOW() 
                                    WHERE 
                                        EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                        ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                        YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT;
                               ";
                mySqlCmd.CommandText = CommandText;
                mySqlCmd.ExecuteNonQuery();


                oMySqlTransaction.Commit();
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                oMySqlTransaction = null;
            }
        }

        public string GetGoalAssessmentStatus(string assessmentID, string yearOfAssessment, string employeeID)
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
                                                EMPLOYEE_GOAL_ASSESSMENT 
                                            WHERE 
                                                ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT AND 
                                                EMPLOYEE_ID = @EMPLOYEE_ID;                                           
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
    }
}
