using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataHandler.TrainingAndDevelopment
{
    public class EssayQuestionDataHandler:TemplateDataHandler
    {
        public DataTable getAllQuestions(string evalId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                eq.EQ_ID,
                                eq.EVALUATION_ID,
                                pe.EVALUATION_NAME,
                                eq.QUESTION,
                                NO_OF_ANSWERS,
                                CASE WHEN eq.STATUS_CODE='1' THEN 'Active'
                                    WHEN eq.STATUS_CODE = '0' THEN 'Inactive'
                                END AS STATUS_CODE
                            FROM
                                ESSAY_QUESTIONS eq,
                                PROGRAM_EVALUATION pe
                            WHERE
                                pe.EVALUATION_ID = eq.EVALUATION_ID AND eq.EVALUATION_ID = '" + evalId + "';";

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

        public Boolean Insert(String evalId, String question, String noofAns, String status, String addedBy)
        {
            Boolean blInserted = false;

            string sMySqlString = "";
            string sRQ_ID = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@evalId", evalId.Trim() == "" ? (object)DBNull.Value : evalId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@question", question.Trim() == "" ? (object)DBNull.Value : question.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@noofAns", noofAns.Trim() == "" ? (object)DBNull.Value : noofAns.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler serialHandler = new SerialHandler();
                sRQ_ID = serialHandler.getserilalReference(ref mySqlCon, "EQ");

                mySqlCmd.Parameters.Add(new MySqlParameter("@sEQ_ID", sRQ_ID.Trim() == "" ? (object)DBNull.Value : sRQ_ID.Trim()));

                sMySqlString = @"INSERT INTO ESSAY_QUESTIONS(EQ_ID,EVALUATION_ID,QUESTION,NO_OF_ANSWERS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE)
                                 VALUES(@sEQ_ID,@evalId,@question,@noofAns,@status,@addedBy,now(),@addedBy,now());";


                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

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

        public bool Update(string rqId, string evalId, string question, string noOfAns, string ststus, string logUser)
        {
            bool isUpdate = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Parameters.Add(new MySqlParameter("@eqId", rqId.Trim() == "" ? (object)DBNull.Value : rqId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@evalId", evalId.Trim() == "" ? (object)DBNull.Value : evalId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@question", question.Trim() == "" ? (object)DBNull.Value : question.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@noOfAns", noOfAns.Trim() == "" ? (object)DBNull.Value : noOfAns.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ststus", ststus.Trim() == "" ? (object)DBNull.Value : ststus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@logUser", logUser.Trim() == "" ? (object)DBNull.Value : logUser.Trim()));

                sMySqlString = @"UPDATE ESSAY_QUESTIONS 
                                SET 
                                    EVALUATION_ID = @evalId,
                                    QUESTION = @question,
                                    NO_OF_ANSWERS = @noOfAns,
                                    STATUS_CODE = @ststus,
                                    MODIFIED_BY = @logUser,
                                    MODIFIED_DATE = now()
                                WHERE
                                    EQ_ID = @eqId;";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

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


        public DataTable getAllQuestionsWithEvaluation(string evalId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                eq.EQ_ID,
                                eq.EVALUATION_ID,
                                pe.EVALUATION_NAME,
                                eq.QUESTION,
                                NO_OF_ANSWERS,
                                CASE WHEN eq.STATUS_CODE='1' THEN 'Active'
                                    WHEN eq.STATUS_CODE = '0' THEN 'Inactive'
                                END AS STATUS_CODE
                            FROM
                                ESSAY_QUESTIONS eq,
                                PROGRAM_EVALUATION pe
                            WHERE
                                pe.EVALUATION_ID = eq.EVALUATION_ID AND eq.EVALUATION_ID = '" + evalId + "';";

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
