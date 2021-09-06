using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;
namespace DataHandler.PerformanceManagement
{
    public class QuestionBankDataHandler :TemplateDataHandler
    {
        public DataTable Populate()
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"
                                            SELECT 
                                                QUESTION_ID, QUESTION, REMARKS, STATUS_CODE
                                            FROM
                                                QUESTIONNAIRE_BANK 
                                            ORDER BY SUBSTRING(QUESTION_ID, 3,9)+0 ASC;                                            
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                dataTable.Columns.Add("STATUS_CODE_VALUE");

                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        string Status = dataTable.Rows[i]["STATUS_CODE"].ToString();
                        if (Constants.STATUS_ACTIVE_VALUE == Status)
                        {
                            dataTable.Rows[i]["STATUS_CODE_VALUE"] = Constants.STATUS_ACTIVE_TAG;
                        }
                        else if (Constants.STATUS_INACTIVE_VALUE == Status)
                        {
                            dataTable.Rows[i]["STATUS_CODE_VALUE"] = Constants.STATUS_INACTIVE_TAG;
                        }
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable PopulateActiveSelfAssessmentProfiles(string QuestionID)
        {
            try
            {
                dataTable = new DataTable();

                mySqlCmd.Parameters.Clear();
                mySqlCmd.Connection = mySqlCon;
                string sMySqlString = @"
                                            SELECT 
                                                SAP.SELF_ASSESSMENT_PROFILE_ID, 
                                                SAP.PROFILE_NAME, 
                                                SAP.STATUS_CODE 
                                            FROM 
                                                SELF_ASSESSMENT_PROFILE_QUESTIONS_BANK SAPQB,
                                                SELF_ASSESSMENT_PROFILE SAP
                                            WHERE
                                                SAPQB.SELF_ASSESSMENT_PROFILE_ID = SAP.SELF_ASSESSMENT_PROFILE_ID AND 
                                                SAPQB.QUESTION_ID = @QUESTION_ID AND 
                                                SAP.STATUS_CODE = @STATUS_CODE;                                          
                                        ";
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@QUESTION_ID", QuestionID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Insert(string Question, string Remarks, string StatusCode, string AddedBy)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                SerialHandler serialHandler = new SerialHandler();
                string QuestionBankID = String.Empty;

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    mySqlCmd.Parameters.Clear();

                    QuestionBankID = serialHandler.getserila(mySqlCon, Constants.QUESTION_BANK_ID);


                    string Qry = @"
                                        INSERT INTO QUESTIONNAIRE_BANK
                                            (
                                                QUESTION_ID,
                                                QUESTION,
                                                REMARKS,
                                                STATUS_CODE,
                                                ADDED_BY,
                                                ADDED_DATE,
                                                MODIFIED_BY,
                                                MODIFIED_DATE
                                            )
                                        VALUES
                                            (
                                                @QUESTION_ID,
                                                @QUESTION,
                                                @REMARKS,
                                                @STATUS_CODE,
                                                @ADDED_BY,
                                                NOW(),
                                                @ADDED_BY,
                                                NOW()
                                            );
                                    ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@QUESTION_ID", QuestionBankID.Trim() == "" ? (object)DBNull.Value : QuestionBankID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@QUESTION", Question.Trim() == "" ? (object)DBNull.Value : Question.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@REMARKS", Remarks.Trim() == "" ? (object)DBNull.Value : Remarks.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));

                    mySqlCmd.CommandText = Qry;
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

        public Boolean Update(string Question, string Remarks, string StatusCode, string ModifiedBy, string QuestionBankID)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    mySqlCmd.Parameters.Clear();



                    string Qry = @"
                                        UPDATE 
                                            QUESTIONNAIRE_BANK
                                        SET
                                                QUESTION = @QUESTION,
                                                REMARKS = @REMARKS,
                                                STATUS_CODE = @STATUS_CODE,
                                                MODIFIED_BY = @MODIFIED_BY,
                                                MODIFIED_DATE = NOW()
                                        WHERE
                                                QUESTION_ID = @QUESTION_ID;
                                    ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@QUESTION_ID", QuestionBankID.Trim() == "" ? (object)DBNull.Value : QuestionBankID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@QUESTION", Question.Trim() == "" ? (object)DBNull.Value : Question.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@REMARKS", Remarks.Trim() == "" ? (object)DBNull.Value : Remarks.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));

                    mySqlCmd.CommandText = Qry;
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

        public Boolean CheckQuestionExsistance(string question)
        {
            Boolean isExsists = false;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();


                string queryStr = "SELECT * FROM QUESTIONNAIRE_BANK WHERE QUESTION ='" + question + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExsists = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return isExsists;
        }

        public Boolean CheckQuestionExsistance(string question, string id)
        {

            dataTable = new DataTable();
            Boolean isExsists = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string queryStr = "SELECT * FROM QUESTIONNAIRE_BANK WHERE QUESTION ='" + question + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["QUESTION_ID"].ToString() == id)
                        {
                            isExsists = false;
                        }
                        else
                        {
                            isExsists = true;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }

            return isExsists;
        }        

    }
}
