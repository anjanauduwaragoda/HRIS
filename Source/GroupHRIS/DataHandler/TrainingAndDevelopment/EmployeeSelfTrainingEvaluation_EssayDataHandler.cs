using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.TrainingAndDevelopment
{
    public class EmployeeSelfTrainingEvaluation_EssayDataHandler : TemplateDataHandler
    {
        public DataTable getEssayQuestions(string evaluationId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = @"SELECT EQ_ID, QUESTION, NO_OF_ANSWERS 
                                    FROM ESSAY_QUESTIONS 
                                    WHERE EVALUATION_ID ='" + evaluationId + "' AND STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                return resultTable;
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }
        }

        public bool insertAnswers(string employeeId, string evaluationId, string trainingId, Dictionary<string, Array> answerDictionary, string isFinalizedStatus)
        {
            bool inserted = false;
            MySqlTransaction mySqlTrans = null;
            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                string sqlDeleteExisting = @"DELETE FROM TE_EQ_ANSWERS 
                                                    WHERE  EMPLOYEE_ID ='" + employeeId + "' AND EVALUATION_ID ='" + evaluationId + "' AND TRAINING_ID ='" + trainingId + "' ";

                mySqlCmd.CommandText = sqlDeleteExisting;
                mySqlCmd.ExecuteNonQuery();

                foreach(KeyValuePair<string, Array> item in answerDictionary )
                {
                    string questionId = item.Key.ToString();
                    int answerOrder = 1;
                    foreach (var answer in item.Value)
                    {
                        string answerNo = answerOrder.ToString();
                        
                        mySqlCmd.Parameters.Clear();

                        mySqlCmd.Parameters.Add(new MySqlParameter("@EVALUATION_ID", evaluationId.Trim() == "" ? (object)DBNull.Value : evaluationId.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ESSAY_QUESTION_ID", questionId.Trim() == "" ? (object)DBNull.Value : questionId.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", trainingId.Trim() == "" ? (object)DBNull.Value : trainingId.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ANSWER_ORDER_NO", answerNo.Trim() == "" ? (object)DBNull.Value : answerNo.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ANSWER", answer.ToString().Trim() == "" ? (object)DBNull.Value : answer.ToString().Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@IS_FINALIZED", isFinalizedStatus.Trim() == "" ? (object)DBNull.Value : isFinalizedStatus.Trim()));


                        string sqlString = @"   INSERT INTO TE_EQ_ANSWERS
                                                (
                                                    EVALUATION_ID,
                                                    ESSAY_QUESTION_ID,
                                                    TRAINING_ID,
                                                    EMPLOYEE_ID,
                                                    ANSWER_ORDER_NO,
                                                    ANSWER,
                                                    IS_FINALIZED
                                                )
                                                VALUES 
                                                (
                                                    @EVALUATION_ID,
                                                    @ESSAY_QUESTION_ID,
                                                    @TRAINING_ID,
                                                    @EMPLOYEE_ID,
                                                    @ANSWER_ORDER_NO,
                                                    @ANSWER,
                                                    @IS_FINALIZED
                                                )";

                        mySqlCmd.CommandText = sqlString;
                        mySqlCmd.ExecuteNonQuery();

                        answerOrder++;
                    }
                }

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                return inserted;
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }

        }

        public DataTable getEssayAnswers(string employeeId, string evaluationId, string trainingId, string isPostEvaluation)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlString =@"SELECT 
                                        EVALUATION_ID,
                                        ESSAY_QUESTION_ID,
                                        TRAINING_ID,
                                        EMPLOYEE_ID,
                                        ANSWER_ORDER_NO,
                                        ANSWER 
                                    FROM
                                        TE_EQ_ANSWERS
                                    WHERE
                                        EMPLOYEE_ID ='"+employeeId+"' AND EVALUATION_ID ='"+evaluationId+"' AND TRAINING_ID ='"+trainingId+"' AND IS_POST_EVALUATION ='"+isPostEvaluation+"' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlString, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
            }
        }

    }
}
