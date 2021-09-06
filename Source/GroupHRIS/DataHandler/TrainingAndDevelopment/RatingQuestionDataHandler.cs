using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.TrainingAndDevelopment
{
    public class RatingQuestionDataHandler : TemplateDataHandler
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
                                    rq.RQ_ID,rq.EVALUATION_ID,
                                    pe.EVALUATION_NAME,
                                    rq.QUESTION,
                                    CASE
                                        WHEN rq.STATUS_CODE = '1' THEN 'Active'
                                        WHEN rq.STATUS_CODE = '0' THEN 'Inactive'
                                    END AS STATUS_CODE
                                FROM
                                    RATING_QUESTIONS rq,
                                    PROGRAM_EVALUATION pe
                                WHERE
                                    pe.EVALUATION_ID = rq.EVALUATION_ID AND rq.EVALUATION_ID = '" + evalId + "';";

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

        public Boolean Insert(String evalId,String question,String status, String addedBy)
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
                //mySqlCmd.Parameters.Add(new MySqlParameter("@noofAns", noofAns.Trim() == "" ? (object)DBNull.Value : noofAns.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler serialHandler = new SerialHandler();
                sRQ_ID = serialHandler.getserilalReference(ref mySqlCon, "RQ");

                mySqlCmd.Parameters.Add(new MySqlParameter("@sRQ_ID", sRQ_ID.Trim() == "" ? (object)DBNull.Value : sRQ_ID.Trim()));

                sMySqlString = @"INSERT INTO RATING_QUESTIONS(RQ_ID,EVALUATION_ID,QUESTION,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE)
                                 VALUES(@sRQ_ID,@evalId,@question,@status,@addedBy,now(),@addedBy,now());";


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

        public bool Update(string rqId, string evalId, string question, string ststus,string logUser)
        {
            bool isUpdate = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Parameters.Add(new MySqlParameter("@rqId", rqId.Trim() == "" ? (object)DBNull.Value : rqId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@evalId", evalId.Trim() == "" ? (object)DBNull.Value : evalId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@question", question.Trim() == "" ? (object)DBNull.Value : question.Trim()));
               // mySqlCmd.Parameters.Add(new MySqlParameter("@noOfAns", noOfAns.Trim() == "" ? (object)DBNull.Value : noOfAns.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ststus", ststus.Trim() == "" ? (object)DBNull.Value : ststus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@logUser", logUser.Trim() == "" ? (object)DBNull.Value : logUser.Trim()));
                
                sMySqlString = @"UPDATE RATING_QUESTIONS 
                                SET 
                                    EVALUATION_ID = @evalId,
                                    QUESTION = @question,
                                    STATUS_CODE = @ststus,
                                    MODIFIED_BY = @logUser,
                                    MODIFIED_DATE = now()
                                WHERE
                                    RQ_ID = @rqId;";

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
                                    rq.RQ_ID,rq.EVALUATION_ID,
                                    pe.EVALUATION_NAME,
                                    rq.QUESTION,
                                    rq.NO_OF_ANSWERS,
                                    CASE
                                        WHEN rq.STATUS_CODE = '1' THEN 'Active'
                                        WHEN rq.STATUS_CODE = '0' THEN 'Inactive'
                                    END AS STATUS_CODE
                                FROM
                                    RATING_QUESTIONS rq,
                                    PROGRAM_EVALUATION pe
                                WHERE
                                    pe.EVALUATION_ID = rq.EVALUATION_ID AND rq.EVALUATION_ID = '" + evalId + "';";

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
