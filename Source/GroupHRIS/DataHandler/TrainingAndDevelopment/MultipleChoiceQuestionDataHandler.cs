using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.TrainingAndDevelopment
{
    public class MultipleChoiceQuestionDataHandler : TemplateDataHandler
    {

        public DataTable getMCQ(string evalId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                MCQ_ID, QUESTION, CHOICES,
                                    CASE WHEN STATUS_CODE = '1' THEN 'Active' 
                                        WHEN STATUS_CODE = '0' THEN 'Inactive' 
                                    END AS STATUS_CODE
                            FROM
                                MULTIPLE_CHOICE_QUESTIONS WHERE EVALUATION_ID = '" + evalId + "';";

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

        public Boolean Insert(string evalId, string question, string choices, string status, DataTable dt, string addedBy)
        {
            Boolean blInserted = false;

            string sMySqlString = "";
            string sMCQ_ID = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                if (status == "Active")
                {
                    status = "1";
                }
                else
                {
                    status = "0";
                }

                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@evalId", evalId.Trim() == "" ? (object)DBNull.Value : evalId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@question", question.Trim() == "" ? (object)DBNull.Value : question.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@choices", choices.Trim() == "" ? (object)DBNull.Value : choices.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Transaction = mySqlTrans;

                SerialHandler serialHandler = new SerialHandler();
                sMCQ_ID = serialHandler.getserilalReference(ref mySqlCon, "MCQ");

                mySqlCmd.Parameters.Add(new MySqlParameter("@sMCQ_ID", sMCQ_ID.Trim() == "" ? (object)DBNull.Value : sMCQ_ID.Trim()));

                sMySqlString = @"INSERT INTO MULTIPLE_CHOICE_QUESTIONS(MCQ_ID,EVALUATION_ID,QUESTION,CHOICES,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE)
                                VALUES(@sMCQ_ID,@evalId,@question,@choices,@status,@addedBy,now(),@addedBy,now());";
                
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();


                // insert in to mcq answer table
                foreach (DataRow dr in dt.Rows)
                {
                    string answerId = dr["ANSWER_ID"].ToString();
                    string answer = dr["ANSWER"].ToString();
                    string isAnswer = dr["IS_ANSWER"].ToString();
                    string ansStatus = dr["STATUS_CODE"].ToString();

                    if (ansStatus == "Active")
                    {
                        ansStatus = "1";
                    }
                    else
                    {
                        ansStatus = "0";
                    }

                    if (isAnswer == "Yes")
                    {
                        isAnswer = "1";
                    }
                    else
                    {
                        isAnswer = "0";
                    }


                    string qry = @"INSERT INTO MCQ_ANSWERS(EVALUATION_ID,MCQ_ID,ANSWER_ID,ANSWER,IS_ANSWER,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                    VALUES(@evalId,@sMCQ_ID,'" + answerId + "','" + answer + "','" + isAnswer + "','" + ansStatus + "',@addedBy,now(),@addedBy,now());";

                    mySqlCmd.CommandText = qry;
                    mySqlCmd.ExecuteNonQuery();
                }


                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                serialHandler = null;

                blInserted = true;
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

            return blInserted;
        }

        public bool Update(string qId, string evalId, string question, string choices, string status, DataTable dt, string addedBy)
        {
            bool isUpdate = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                if (status == "Active")
                {
                    status = "1";
                }
                else
                {
                    status = "0";
                }

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Parameters.Add(new MySqlParameter("@qId", qId.Trim() == "" ? (object)DBNull.Value : qId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@evalId", evalId.Trim() == "" ? (object)DBNull.Value : evalId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@question", question.Trim() == "" ? (object)DBNull.Value : question.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@choices", choices.Trim() == "" ? (object)DBNull.Value : choices.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

                sMySqlString = @"UPDATE MULTIPLE_CHOICE_QUESTIONS 
                                    SET 
                                        QUESTION = @question,
                                        CHOICES = @choices,
                                        STATUS_CODE = @status,
                                        MODIFIED_BY = @addedBy,
                                        MODIFIED_DATE = NOW()
                                    WHERE
                                        MCQ_ID = @qId AND EVALUATION_ID = @evalId;";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                string delQuery = "DELETE FROM MCQ_ANSWERS WHERE EVALUATION_ID = @evalId AND MCQ_ID = @qId;";
                mySqlCmd.CommandText = delQuery;
                mySqlCmd.ExecuteNonQuery();


                // insert in to mcq answer table
                foreach (DataRow dr in dt.Rows)
                {
                    string answerId = dr["ANSWER_ID"].ToString();
                    string answer = dr["ANSWER"].ToString();
                    string isAnswer = dr["IS_ANSWER"].ToString();
                    string ansStatus = dr["STATUS_CODE"].ToString();

                    if (ansStatus == "Active")
                    {
                        ansStatus = "1";
                    }
                    else
                    {
                        ansStatus = "0";
                    }

                    if (isAnswer == "Yes")
                    {
                        isAnswer = "1";
                    }
                    else
                    {
                        isAnswer = "0";
                    }


                    string qry = @"INSERT INTO MCQ_ANSWERS(EVALUATION_ID,MCQ_ID,ANSWER_ID,ANSWER,IS_ANSWER,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                    VALUES(@evalId,@qId,'" + answerId + "','" + answer + "','" + isAnswer + "','" + ansStatus + "',@addedBy,now(),@addedBy,now());";

                    mySqlCmd.CommandText = qry;
                    mySqlCmd.ExecuteNonQuery();
                }

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isUpdate = true;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }

            return isUpdate;
        }

        public DataTable getMCQAnswers(string evalId,string questionId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    ANSWER_ID,
                                    ANSWER,
                                    CASE
                                        WHEN IS_ANSWER = '1' THEN 'Yes'
                                        WHEN IS_ANSWER = '0' THEN 'No'
                                    END AS IS_ANSWER,
                                    CASE
                                        WHEN STATUS_CODE = '1' THEN 'Active'
                                        WHEN STATUS_CODE = '0' THEN 'Inactive'
                                    END AS STATUS_CODE
                                FROM
                                    MCQ_ANSWERS
                                WHERE
                                    EVALUATION_ID = '" + evalId + "' AND MCQ_ID = '" + questionId + "';";

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

    }
}
