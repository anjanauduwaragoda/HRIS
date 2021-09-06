using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class MCQDataHandler : TemplateDataHandler
    {
        public DataTable PopulateEvaluations(string EmployeeID)
        {
            DataTable dtResult = new DataTable();
            try
            {
                mySqlCmd.Parameters.Clear();

                string sqlString = @"   
                                        SELECT 
                                            PE.EVALUATION_ID,
                                            TP.TRAINING_ID,
                                            TP.EMPLOYEE_ID,
                                            PE.EVALUATION_NAME,
                                            T.TRAINING_NAME,
                                            T.TRAINING_CODE
                                        FROM
                                            TRAINING T,
                                            TRAINING_PARTICIPANTS TP,
                                            PROGRAM_EVALUATION PE
                                        WHERE
                                            TP.TRAINING_ID = T.TRAINING_ID
                                                AND T.TRAINING_PROGRAM_ID = PE.TRAINING_PROGRAM_ID
                                                AND PE.MCQ_INCLUDED = @STATUS_CODE
                                                AND TP.EMPLOYEE_ID = @EMPLOYEE_ID
                                                AND T.STATUS_CODE = @STATUS_CODE
                                    ";

                mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EmployeeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));

                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sqlString;

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCmd);
                mySqlDataAdapter.Fill(dtResult);

                return dtResult;
            }
            catch (Exception)
            {
                mySqlCon.Close();
                throw;
            }
            finally
            {
                dtResult.Dispose();
                mySqlCon.Close();
            }
        }

        public DataTable PopulateMCQ(string EvaluationID)
        {
            DataTable dtResult = new DataTable();
            try
            {
                mySqlCmd.Parameters.Clear();

                string sqlString = @"   
                                        SELECT 
                                            MCQ.EVALUATION_ID, MCQ.MCQ_ID, MCQ.QUESTION, MCQ.CHOICES
                                        FROM
                                            MULTIPLE_CHOICE_QUESTIONS MCQ
                                        WHERE
                                            MCQ.EVALUATION_ID = @EVALUATION_ID
                                                AND MCQ.STATUS_CODE = @STATUS_CODE
                                    ";

                mySqlCmd.Parameters.Add(new MySqlParameter("@EVALUATION_ID", EvaluationID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));

                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sqlString;

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCmd);
                mySqlDataAdapter.Fill(dtResult);

                return dtResult;
            }
            catch (Exception)
            {
                mySqlCon.Close();
                throw;
            }
            finally
            {
                dtResult.Dispose();
                mySqlCon.Close();
            }
        }

        public DataTable PopulateMCQAnswers(string EvaluationID)
        {
            DataTable dtResult = new DataTable();
            try
            {
                mySqlCmd.Parameters.Clear();

                string sqlString = @"   
                                        SELECT 
                                            MCQA.EVALUATION_ID,
                                            MCQA.MCQ_ID,
                                            MCQA.ANSWER_ID,
                                            MCQA.ANSWER,
                                            MCQA.IS_ANSWER
                                        FROM
                                            MCQ_ANSWERS MCQA
                                        WHERE
                                            MCQA.EVALUATION_ID = @EVALUATION_ID
                                                AND MCQA.STATUS_CODE = @STATUS_CODE
                                        ORDER BY MCQA.EVALUATION_ID ASC, MCQA.MCQ_ID ASC, MCQA.ANSWER_ID ASC
                                    ";

                mySqlCmd.Parameters.Add(new MySqlParameter("@EVALUATION_ID", EvaluationID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));

                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sqlString;

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCmd);
                mySqlDataAdapter.Fill(dtResult);

                return dtResult;
            }
            catch (Exception)
            {
                mySqlCon.Close();
                throw;
            }
            finally
            {
                dtResult.Dispose();
                mySqlCon.Close();
            }
        }

        public void Insert(DataTable MCQ_ANS)
        {
            try
            {

                string sMySqlString = "";

                MySqlTransaction mySqlTrans = null;
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                foreach (DataRow dr in MCQ_ANS.Rows)
                {

                    mySqlCmd.Parameters.Clear();

                    string EVALUATION_ID = dr["EVALUATION_ID"].ToString().Trim();
                    string MCQ_ID = dr["MCQ_ID"].ToString().Trim();
                    string TRAINING_ID = dr["TRAINING_ID"].ToString().Trim();
                    string EMPLOYEE_ID = dr["EMPLOYEE_ID"].ToString().Trim();
                    string ANSWER_ID = dr["ANSWER_ID"].ToString().Trim();


                    mySqlCmd.Parameters.Add(new MySqlParameter("@EVALUATION_ID", EVALUATION_ID.Trim() == "" ? (object)DBNull.Value : EVALUATION_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MCQ_ID", MCQ_ID.Trim() == "" ? (object)DBNull.Value : MCQ_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TRAINING_ID.Trim() == "" ? (object)DBNull.Value : TRAINING_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EMPLOYEE_ID.Trim() == "" ? (object)DBNull.Value : EMPLOYEE_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ANSWER_ID", ANSWER_ID.Trim() == "" ? (object)DBNull.Value : ANSWER_ID.Trim()));

                    sMySqlString = @" 
                                        DELETE FROM 
                                            TE_MCQ_ANSWERS 
                                        WHERE 
                                            EVALUATION_ID = @EVALUATION_ID 
                                            AND MCQ_ID = @MCQ_ID 
                                            AND TRAINING_ID = @TRAINING_ID 
                                            AND EMPLOYEE_ID = @EMPLOYEE_ID 
                                            AND ANSWER_ID = @ANSWER_ID 
                                    ";


                    mySqlCmd.Transaction = mySqlTrans;
                    mySqlCmd.CommandText = sMySqlString;


                    sMySqlString = @" 
                                        INSERT INTO TE_MCQ_ANSWERS
                                        (
                                            EVALUATION_ID,
                                            MCQ_ID,
                                            TRAINING_ID,
                                            EMPLOYEE_ID,
                                            ANSWER_ID)
                                            VALUES
                                        (
                                            @EVALUATION_ID,
                                            @MCQ_ID,
                                            @TRAINING_ID,
                                            @EMPLOYEE_ID,
                                            @ANSWER_ID
                                        );
                                    ";


                    mySqlCmd.Transaction = mySqlTrans;
                    mySqlCmd.CommandText = sMySqlString;

                    mySqlCmd.ExecuteNonQuery();
                }
                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }
            finally
            {

            }
        }
    }
}
