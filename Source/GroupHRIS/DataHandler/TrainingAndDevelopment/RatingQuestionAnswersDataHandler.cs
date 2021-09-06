using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class RatingQuestionAnswersDataHandler : TemplateDataHandler
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
                                                AND PE.RQ_INCLUDED = @STATUS_CODE
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

        public DataTable PopulateRatingQuestions(string EvaluationID)
        {
            DataTable dtResult = new DataTable();
            try
            {
                mySqlCmd.Parameters.Clear();

                string sqlString = @"   
                                        SELECT 
                                            RQ_ID, QUESTION, NO_OF_ANSWERS
                                        FROM
                                            RATING_QUESTIONS 
                                        WHERE
                                            EVALUATION_ID = @EVALUATION_ID
                                                AND STATUS_CODE  = @STATUS_CODE
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

        public DataTable PopulateRQAnswers(string EvaluationID)
        {
            DataTable dtResult = new DataTable();
            try
            {
                mySqlCmd.Parameters.Clear();

                string sqlString = @"   
                                        SELECT 
                                            RR.RS_ID, RR.RATING, RR.WEIGHT, RR.DESCRIPTION, RR.REMARKS
                                        FROM
                                            PROGRAM_EVALUATION PE,
                                            RATING_SCHEME RS,
                                            RS_RATINGS RR
                                        WHERE
                                            PE.EVALUATION_ID = @EVALUATION_ID
                                                AND PE.STATUS_CODE = @STATUS_CODE
                                                AND RS.RS_ID = PE.RS_ID
                                                AND RS.STATUS_CODE = @STATUS_CODE
                                                AND RR.RS_ID = RS.RS_ID
                                                AND RR.STATUS_CODE = @STATUS_CODE
                                        ORDER BY RR.RATING ASC
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

        public void Insert(DataTable RQ_ANS)
        {
            try
            {

                string sMySqlString = "";

                MySqlTransaction mySqlTrans = null;
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                foreach (DataRow dr in RQ_ANS.Rows)
                {

                    mySqlCmd.Parameters.Clear();

                    string EVALUATION_ID = dr["EVALUATION_ID"].ToString().Trim();
                    string RQ_ID = dr["RQ_ID"].ToString().Trim();
                    string TRAINING_ID = dr["TRAINING_ID"].ToString().Trim();
                    string EMPLOYEE_ID = dr["EMPLOYEE_ID"].ToString().Trim();
                    string RATING = dr["ANSWER_ID"].ToString().Trim();


                    mySqlCmd.Parameters.Add(new MySqlParameter("@EVALUATION_ID", EVALUATION_ID.Trim() == "" ? (object)DBNull.Value : EVALUATION_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@RQ_ID", RQ_ID.Trim() == "" ? (object)DBNull.Value : RQ_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TRAINING_ID.Trim() == "" ? (object)DBNull.Value : TRAINING_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EMPLOYEE_ID.Trim() == "" ? (object)DBNull.Value : EMPLOYEE_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@RATING", RATING.Trim() == "" ? (object)DBNull.Value : RATING.Trim()));

                    sMySqlString = @" 
                                        DELETE FROM 
                                            TE_RATING_QUESTION_ANSWERS 
                                        WHERE 
                                            EVALUATION_ID = @EVALUATION_ID 
                                            AND RQ_ID = @RQ_ID 
                                            AND TRAINING_ID = @TRAINING_ID 
                                            AND EMPLOYEE_ID = @EMPLOYEE_ID 
                                            AND RATING = @RATING 
                                    ";


                    mySqlCmd.Transaction = mySqlTrans;
                    mySqlCmd.CommandText = sMySqlString;


                    sMySqlString = @" 
                                        INSERT INTO 
                                            TE_RATING_QUESTION_ANSWERS
                                            (
                                                EVALUATION_ID,
                                                RQ_ID,
                                                TRAINING_ID,
                                                EMPLOYEE_ID,
                                                RATING
                                            )
                                            VALUES
                                            (
                                                @EVALUATION_ID,
                                                @RQ_ID,
                                                @TRAINING_ID,
                                                @EMPLOYEE_ID,
                                                @RATING
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
