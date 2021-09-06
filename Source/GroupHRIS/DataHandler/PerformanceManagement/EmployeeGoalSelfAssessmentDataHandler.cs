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
    public class EmployeeGoalSelfAssessmentDataHandler : TemplateDataHandler
    {
        public DataTable Populate(string EmployeeID, string YearOfGoal)
        {
            DataTable dtEmploeeGoalAssessment = new DataTable();
            dataTable = new DataTable();
            mySqlCmd = new MySqlCommand();
            try
            {
                dtEmploeeGoalAssessment.Columns.Add("ASSESSMENT_ID");
                dtEmploeeGoalAssessment.Columns.Add("ASSESSMENT_TOKEN");
                dtEmploeeGoalAssessment.Columns.Add("YEAR_OF_ASSESSMENT");
                dtEmploeeGoalAssessment.Columns.Add("EMPLOYEE_ID");
                dtEmploeeGoalAssessment.Columns.Add("GOAL_ID");
                dtEmploeeGoalAssessment.Columns.Add("GOAL_AREA");
                dtEmploeeGoalAssessment.Columns.Add("MEASUREMENTS");
                dtEmploeeGoalAssessment.Columns.Add("WEIGHT");
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"                                            
                                            SELECT 
                                                EMPLOYEE_ID, 
                                                GOAL_ID, 
                                                GOAL_AREA, 
                                                MEASUREMENTS, 
                                                WEIGHT 
                                            FROM 
                                                EMPLOYEE_GOALS 
                                            WHERE 
                                                EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                YEAR_OF_GOAL = @YEAR_OF_GOAL AND 
                                                STATUS_CODE = @STATUS_CODE AND 
                                                EMPLOYEE_AGREE = @EMPLOYEE_AGREE AND 
                                                SUPERVISOR_AGREE = @SUPERVISOR_AGREE AND 
                                                IS_LOCKED = @IS_LOCKED
                                            ORDER BY
                                                WEIGHT DESC;
                                        ";

                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_GOAL", YearOfGoal));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_AGREE", Constants.CON_ACTIVE_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@SUPERVISOR_AGREE", Constants.CON_ACTIVE_STATUS));
                mySqlCmd.Parameters.Add(new MySqlParameter("@IS_LOCKED", Constants.CON_ACTIVE_STATUS));
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow dr = dtEmploeeGoalAssessment.NewRow();
                    dr["EMPLOYEE_ID"] = dataTable.Rows[i]["EMPLOYEE_ID"].ToString();
                    dr["GOAL_ID"] = dataTable.Rows[i]["GOAL_ID"].ToString();
                    dr["GOAL_AREA"] = dataTable.Rows[i]["GOAL_AREA"].ToString();
                    dr["MEASUREMENTS"] = dataTable.Rows[i]["MEASUREMENTS"].ToString();
                    dr["WEIGHT"] = dataTable.Rows[i]["WEIGHT"].ToString();
                    dtEmploeeGoalAssessment.Rows.Add(dr);
                }
                dtEmploeeGoalAssessment.Columns.Add("SUPERVISOR_SCORE");//New Suggestion 2016/11/25
                return dtEmploeeGoalAssessment;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                dtEmploeeGoalAssessment.Dispose();
                dataTable.Dispose();
                mySqlCmd.Dispose();
            }
        }

        public DataTable Populate(string AssessmentID, string EmployeeID, string YearOfAssessment)
        {
            dataTable = new DataTable();
            mySqlCmd = new MySqlCommand();
            try
            {
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"                                            
                                            SELECT 
                                                EGA.ASSESSMENT_ID, 
                                                GAD.ASSESSMENT_TOKEN, 
                                                EGA.YEAR_OF_ASSESSMENT, 
                                                EGA.EMPLOYEE_ID, 
                                                EG.GOAL_ID, 
                                                EG.GOAL_AREA, 
                                                EG.MEASUREMENTS, 
                                                EG.WEIGHT, 
                                                GAD.EMPLOYEE_SELF_SCORE, 
                                                EGA.STATUS_CODE,
                                                GAD.SUPERVISOR_SCORE 
                                            FROM 
                                                EMPLOYEE_GOAL_ASSESSMENT EGA, 
                                                GOAL_ASSESSMENT_DETAILS GAD, 
                                                EMPLOYEE_GOALS EG 
                                            WHERE
                                                EGA.ASSESSMENT_TOKEN = GAD.ASSESSMENT_TOKEN AND
                                                EG.GOAL_ID = GAD.GOAL_ID AND
                                                EG.YEAR_OF_GOAL = EGA.YEAR_OF_ASSESSMENT AND
                                                EGA.ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                                EGA.EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                                EGA.YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT AND
                                                EG.EMPLOYEE_AGREE = @EMPLOYEE_AGREE AND
                                                EG.SUPERVISOR_AGREE = @SUPERVISOR_AGREE AND
                                                EG.IS_LOCKED = @IS_LOCKED
                                            ORDER BY
                                                EG.WEIGHT DESC
                                        ";

                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", AssessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", YearOfAssessment));
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
                dataTable.Dispose();
                mySqlCmd.Dispose();
            }
        }

        public Boolean Insert(string assessmentID, string yearOfAssessment, string employeeID, string assessmentToken, string AddedBy, double totalSelfScore, List<ListItem> liList)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                SerialHandler serialHandler = new SerialHandler();

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    mySqlCmd.Parameters.Clear();
                    if (assessmentToken == "")
                    {
                        assessmentToken = serialHandler.getserila(mySqlCon, Constants.GOAL_ASSESSMENT_TOKEN);
                    }


                    string Qry = @"
                                        INSERT INTO 
                                            EMPLOYEE_GOAL_ASSESSMENT
                                                (
                                                    ASSESSMENT_TOKEN, 
                                                    ASSESSMENT_ID, 
                                                    EMPLOYEE_ID, 
                                                    YEAR_OF_ASSESSMENT, 
                                                    TOTAL_SELF_SCORE, 
                                                    STATUS_CODE, 
                                                    ADDED_BY, 
                                                    ADDED_DATE, 
                                                    MODIFIED_BY, 
                                                    MODIFIED_DATE
                                                ) 
                                            VALUES
                                                (
                                                    @ASSESSMENT_TOKEN, 
                                                    @ASSESSMENT_ID, 
                                                    @EMPLOYEE_ID, 
                                                    @YEAR_OF_ASSESSMENT, 
                                                    @TOTAL_SELF_SCORE, 
                                                    @STATUS_CODE, 
                                                    @ADDED_BY, 
                                                    NOW(), 
                                                    @ADDED_BY, 
                                                    NOW()
                                                );
                                    ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", assessmentToken.Trim() == "" ? (object)DBNull.Value : assessmentToken.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID.Trim() == "" ? (object)DBNull.Value : assessmentID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment.Trim() == "" ? (object)DBNull.Value : yearOfAssessment.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TOTAL_SELF_SCORE", totalSelfScore.ToString().Trim() == "" ? (object)DBNull.Value : totalSelfScore.ToString().Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_ACTIVE_STATUS.Trim() == "" ? (object)DBNull.Value : Constants.ASSESSNEMT_ACTIVE_STATUS.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();

                    mySqlCmd.Parameters.Clear();

                    for (int i = 0; i < liList.Count; i++)
                    {
                        string goalID = liList[i].Text;
                        string achievedProgress = liList[i].Value;
                        mySqlCmd.Parameters.Clear();

                        mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", assessmentToken.Trim() == "" ? (object)DBNull.Value : assessmentToken.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@GOAL_ID", goalID.Trim() == "" ? (object)DBNull.Value : goalID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_SELF_SCORE", achievedProgress.Trim() == "" ? (object)DBNull.Value : achievedProgress.Trim()));
                        Qry = @"
                                    INSERT INTO 
                                        GOAL_ASSESSMENT_DETAILS
                                            (
                                                ASSESSMENT_TOKEN, 
                                                GOAL_ID, 
                                                EMPLOYEE_SELF_SCORE
                                            ) 
                                        VALUES
                                            (
                                                @ASSESSMENT_TOKEN, 
                                                @GOAL_ID, 
                                                @EMPLOYEE_SELF_SCORE
                                            )
                                ";
                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();
                    }

                    oMySqlTransaction.Commit(); ;
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

        public Boolean InsertAndFinalize(string assessmentID, string yearOfAssessment, string employeeID, string assessmentToken, string AddedBy, double totalSelfScore, List<ListItem> liList)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                SerialHandler serialHandler = new SerialHandler();

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    mySqlCmd.Parameters.Clear();
                    if (assessmentToken == "")
                    {
                        assessmentToken = serialHandler.getserila(mySqlCon, Constants.GOAL_ASSESSMENT_TOKEN);
                    }


                    string Qry = @"
                                        INSERT INTO 
                                            EMPLOYEE_GOAL_ASSESSMENT
                                                (
                                                    ASSESSMENT_TOKEN, 
                                                    ASSESSMENT_ID, 
                                                    EMPLOYEE_ID, 
                                                    YEAR_OF_ASSESSMENT, 
                                                    TOTAL_SELF_SCORE, 
                                                    EMPLOYEE_COMPLETED_DATE, 
                                                    STATUS_CODE, 
                                                    ADDED_BY, 
                                                    ADDED_DATE, 
                                                    MODIFIED_BY, 
                                                    MODIFIED_DATE
                                                ) 
                                            VALUES
                                                (
                                                    @ASSESSMENT_TOKEN, 
                                                    @ASSESSMENT_ID, 
                                                    @EMPLOYEE_ID, 
                                                    @YEAR_OF_ASSESSMENT, 
                                                    @TOTAL_SELF_SCORE,
                                                    NOW(), 
                                                    @STATUS_CODE, 
                                                    @ADDED_BY, 
                                                    NOW(), 
                                                    @ADDED_BY, 
                                                    NOW()
                                                );
                                    ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", assessmentToken.Trim() == "" ? (object)DBNull.Value : assessmentToken.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID.Trim() == "" ? (object)DBNull.Value : assessmentID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment.Trim() == "" ? (object)DBNull.Value : yearOfAssessment.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TOTAL_SELF_SCORE", totalSelfScore.ToString().Trim() == "" ? (object)DBNull.Value : totalSelfScore.ToString().Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS.Trim() == "" ? (object)DBNull.Value : Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();

                    mySqlCmd.Parameters.Clear();

                    for (int i = 0; i < liList.Count; i++)
                    {
                        string goalID = liList[i].Text;
                        string achievedProgress = liList[i].Value;
                        mySqlCmd.Parameters.Clear();

                        mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", assessmentToken.Trim() == "" ? (object)DBNull.Value : assessmentToken.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@GOAL_ID", goalID.Trim() == "" ? (object)DBNull.Value : goalID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_SELF_SCORE", achievedProgress.Trim() == "" ? (object)DBNull.Value : achievedProgress.Trim()));
                        Qry = @"
                                    INSERT INTO 
                                        GOAL_ASSESSMENT_DETAILS
                                            (
                                                ASSESSMENT_TOKEN, 
                                                GOAL_ID, 
                                                EMPLOYEE_SELF_SCORE
                                            ) 
                                        VALUES
                                            (
                                                @ASSESSMENT_TOKEN, 
                                                @GOAL_ID, 
                                                @EMPLOYEE_SELF_SCORE
                                            )
                                ";
                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();
                    }

                    oMySqlTransaction.Commit(); ;
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

        public Boolean Update(string assessmentToken, string assessmentID, string employeeID, string totalSelfScore, string modifiedBy, List<ListItem> goalWeights)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                SerialHandler serialHandler = new SerialHandler();

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    mySqlCmd.Parameters.Clear();


                    string Qry = @"
                                        UPDATE 
                                            EMPLOYEE_GOAL_ASSESSMENT 
                                        SET 
                                            TOTAL_SELF_SCORE = @TOTAL_SELF_SCORE, 
                                            MODIFIED_BY = @MODIFIED_BY, 
                                            MODIFIED_DATE = NOW() 
                                        WHERE 
                                            ASSESSMENT_TOKEN = @ASSESSMENT_TOKEN AND 
                                            ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                            EMPLOYEE_ID = @EMPLOYEE_ID;
                                    ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@TOTAL_SELF_SCORE", totalSelfScore.Trim() == "" ? (object)DBNull.Value : totalSelfScore.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", modifiedBy.Trim() == "" ? (object)DBNull.Value : modifiedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", assessmentToken.ToString().Trim() == "" ? (object)DBNull.Value : assessmentToken.ToString().Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID.Trim() == "" ? (object)DBNull.Value : assessmentID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();

                    mySqlCmd.Parameters.Clear();

                    for (int i = 0; i < goalWeights.Count; i++)
                    {
                        string goalID = goalWeights[i].Text;
                        string achievedProgress = goalWeights[i].Value;
                        mySqlCmd.Parameters.Clear();

                        mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_SELF_SCORE", achievedProgress.Trim() == "" ? (object)DBNull.Value : achievedProgress.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", assessmentToken.Trim() == "" ? (object)DBNull.Value : assessmentToken.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@GOAL_ID", goalID.Trim() == "" ? (object)DBNull.Value : goalID.Trim()));
                        Qry = @"
                                    UPDATE 
                                        GOAL_ASSESSMENT_DETAILS 
                                    SET 
                                        EMPLOYEE_SELF_SCORE = @EMPLOYEE_SELF_SCORE  
                                    WHERE 
                                        ASSESSMENT_TOKEN = @ASSESSMENT_TOKEN AND 
                                        GOAL_ID = @GOAL_ID;
                                ";
                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();
                    }

                    oMySqlTransaction.Commit(); ;
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

        public Boolean UpdateAndFinalize(string assessmentToken, string assessmentID, string employeeID, string totalSelfScore, string modifiedBy, List<ListItem> goalWeights)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                SerialHandler serialHandler = new SerialHandler();

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    mySqlCmd.Parameters.Clear();


                    string Qry = @"
                                        UPDATE 
                                            EMPLOYEE_GOAL_ASSESSMENT 
                                        SET 
                                            TOTAL_SELF_SCORE = @TOTAL_SELF_SCORE, 
                                            MODIFIED_BY = @MODIFIED_BY, 
                                            MODIFIED_DATE = NOW(), 
                                            STATUS_CODE = @STATUS_CODE, 
                                            EMPLOYEE_COMPLETED_DATE = NOW()  
                                        WHERE 
                                            ASSESSMENT_TOKEN = @ASSESSMENT_TOKEN AND 
                                            ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                            EMPLOYEE_ID = @EMPLOYEE_ID;
                                    ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@TOTAL_SELF_SCORE", totalSelfScore.Trim() == "" ? (object)DBNull.Value : totalSelfScore.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", modifiedBy.Trim() == "" ? (object)DBNull.Value : modifiedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", assessmentToken.ToString().Trim() == "" ? (object)DBNull.Value : assessmentToken.ToString().Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID.Trim() == "" ? (object)DBNull.Value : assessmentID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS.Trim() == "" ? (object)DBNull.Value : Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS.Trim()));

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();

                    mySqlCmd.Parameters.Clear();

                    for (int i = 0; i < goalWeights.Count; i++)
                    {
                        string goalID = goalWeights[i].Text;
                        string achievedProgress = goalWeights[i].Value;
                        mySqlCmd.Parameters.Clear();

                        mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_SELF_SCORE", achievedProgress.Trim() == "" ? (object)DBNull.Value : achievedProgress.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_TOKEN", assessmentToken.Trim() == "" ? (object)DBNull.Value : assessmentToken.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@GOAL_ID", goalID.Trim() == "" ? (object)DBNull.Value : goalID.Trim()));
                        Qry = @"
                                    UPDATE 
                                        GOAL_ASSESSMENT_DETAILS 
                                    SET 
                                        EMPLOYEE_SELF_SCORE = @EMPLOYEE_SELF_SCORE  
                                    WHERE 
                                        ASSESSMENT_TOKEN = @ASSESSMENT_TOKEN AND 
                                        GOAL_ID = @GOAL_ID;
                                ";
                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();
                    }

                    oMySqlTransaction.Commit(); ;
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

        public Boolean FinalizeGoalAssessment(string assessmentID, string yearOfAssessment, string employeeID, string modifiedBy)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                SerialHandler serialHandler = new SerialHandler();

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    mySqlCmd.Parameters.Clear();

                    string Qry = @"
                                        UPDATE 
                                            EMPLOYEE_GOAL_ASSESSMENT 
                                        SET 
                                            STATUS_CODE = @STATUS_CODE, 
                                            EMPLOYEE_COMPLETED_DATE = NOW() ,
                                            MODIFIED_BY = @MODIFIED_BY, 
                                            MODIFIED_DATE = NOW()  
                                        WHERE 
                                            ASSESSMENT_ID = @ASSESSMENT_ID AND 
                                            EMPLOYEE_ID = @EMPLOYEE_ID AND 
                                            YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT
                                    ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS.Trim() == "" ? (object)DBNull.Value : Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", modifiedBy.Trim() == "" ? (object)DBNull.Value : modifiedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", assessmentID.ToString().Trim() == "" ? (object)DBNull.Value : assessmentID.ToString().Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", yearOfAssessment.Trim() == "" ? (object)DBNull.Value : yearOfAssessment.Trim()));

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();
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

        public Boolean isNewRecord(string EmployeeID, string AssessmentID, string YearOfAssessment)
        {
            dataTable = new DataTable();
            mySqlCmd = new MySqlCommand();
            try
            {
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"                                            
                                            SELECT * FROM EMPLOYEE_GOAL_ASSESSMENT WHERE EMPLOYEE_ID = @EMPLOYEE_ID AND ASSESSMENT_ID = @ASSESSMENT_ID AND YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT;
                                        ";

                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", AssessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", YearOfAssessment));
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
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

        public Boolean isFinalized(string EmployeeID, string AssessmentID, string YearOfAssessment)
        {
            dataTable = new DataTable();
            mySqlCmd = new MySqlCommand();
            try
            {
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"                                            
                                            SELECT STATUS_CODE FROM EMPLOYEE_GOAL_ASSESSMENT WHERE ASSESSMENT_ID = @ASSESSMENT_ID AND EMPLOYEE_ID = @EMPLOYEE_ID AND YEAR_OF_ASSESSMENT = @YEAR_OF_ASSESSMENT
                                        ";

                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ASSESSMENT_ID", AssessmentID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@YEAR_OF_ASSESSMENT", YearOfAssessment));
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    string statusCode = dataTable.Rows[0]["STATUS_CODE"].ToString();
                    if (statusCode == Constants.ASSESSNEMT_ACTIVE_STATUS)
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
                    return false;
                }
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
    }
}