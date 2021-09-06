using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.TrainingAndDevelopment
{
    public class ProgramEvaluationDataHandler : TemplateDataHandler
    {

        public DataTable getEvaluationDetails(string programId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                String Qry = @"SELECT 
                                e.EVALUATION_ID,
                                CASE WHEN e.RS_ID IS NULL THEN '' WHEN e.RS_ID IS NOT NULL THEN r.RS_NAME END AS RS_NAME,
                                e.EVALUATION_NAME,
                                CASE
                                    WHEN e.MCQ_INCLUDED = '1' THEN 'Yes'
                                    WHEN e.MCQ_INCLUDED = '0' THEN 'No'
                                END AS MCQ_INCLUDED,
                                CASE
                                    WHEN e.EQ_INCLUDED = '1' THEN 'Yes'
                                    WHEN e.EQ_INCLUDED = '0' THEN 'No'
                                END AS EQ_INCLUDED,
                                CASE
                                    WHEN e.RQ_INCLUDED = '1' THEN 'Yes'
                                    WHEN e.RQ_INCLUDED = '0' THEN 'No'
                                END AS RQ_INCLUDED,
                                CASE
                                    WHEN e.STATUS_CODE = '1' THEN 'Active'
                                    WHEN e.STATUS_CODE = '0' THEN 'Inactive'
                                END AS STATUS_CODE
                            FROM
                                PROGRAM_EVALUATION e,
                                RATING_SCHEME r
                            WHERE
                                e.TRAINING_PROGRAM_ID = '" + programId +@"'
                                    AND (e.RS_ID = r.RS_ID || e.RS_ID IS NULL) GROUP BY e.EVALUATION_ID;";
                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public DataTable getRatingScheme()
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                String Qry = @"SELECT 
                                    RS_ID, RS_NAME
                                FROM
                                    RATING_SCHEME
                                WHERE
                                    STATUS_CODE = '1';";

                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Boolean Insert(String ratingSchemeId,String trId,String evalName,String isMcq,String isEssay,String isRq,String status,String user)
        {
            Boolean blInserted = false;

            string sMySqlString = "";
            string sEvaluation_ID = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@ratingSchemeId", ratingSchemeId.Trim() == "" ? (object)DBNull.Value : ratingSchemeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@trId", trId.Trim() == "" ? (object)DBNull.Value : trId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@evalName", evalName.Trim() == "" ? (object)DBNull.Value : evalName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@isMcq", isMcq.Trim() == "" ? (object)DBNull.Value : isMcq.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@isEssay", isEssay.Trim() == "" ? (object)DBNull.Value : isEssay.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@isRq", isRq.Trim() == "" ? (object)DBNull.Value : isRq.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler serialHandler = new SerialHandler();
                sEvaluation_ID = serialHandler.getserilalReference(ref mySqlCon, "EVA");

                mySqlCmd.Parameters.Add(new MySqlParameter("@sEvaluation_ID", sEvaluation_ID.Trim() == "" ? (object)DBNull.Value : sEvaluation_ID.Trim()));

                sMySqlString = @"INSERT INTO PROGRAM_EVALUATION(EVALUATION_ID,RS_ID,TRAINING_PROGRAM_ID,EVALUATION_NAME,MCQ_INCLUDED,EQ_INCLUDED,RQ_INCLUDED,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                 VALUES(@sEvaluation_ID,@ratingSchemeId,@trId,@evalName,@isMcq,@isEssay,@isRq,@status,@user,now(),@user,now());";


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
        
        public Boolean Update(String evalId,String rsId,String programId,String evalName,String isMcq,String isEq,String isRq,String status,String User)
        {
            bool isUpdate = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Parameters.Add(new MySqlParameter("@evalId", evalId.Trim() == "" ? (object)DBNull.Value : evalId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@rsId", rsId.Trim() == "" ? (object)DBNull.Value : rsId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@programId", programId.Trim() == "" ? (object)DBNull.Value : programId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@evalName", evalName.Trim() == "" ? (object)DBNull.Value : evalName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@isMcq", isMcq.Trim() == "" ? (object)DBNull.Value : isMcq.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@isEq", isEq.Trim() == "" ? (object)DBNull.Value : isEq.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@isRq", isRq.Trim() == "" ? (object)DBNull.Value : isRq.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@User", User.Trim() == "" ? (object)DBNull.Value : User.Trim()));

                sMySqlString = @"UPDATE PROGRAM_EVALUATION 
                                    SET 
                                        RS_ID = @rsId,
                                        EVALUATION_NAME = @evalName,
                                        MCQ_INCLUDED = @isMcq,
                                        EQ_INCLUDED = @isEq,
                                        RQ_INCLUDED = @isRq,
                                        STATUS_CODE = @status,
                                        MODIFIED_BY = @User,
                                        MODIFIED_DATE = NOW()
                                    WHERE
                                        EVALUATION_ID = @evalId AND 
                                        TRAINING_PROGRAM_ID = @programId;";

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

        public Int32 getActiveEvaluation(string evaluationId)
        {
            Int32 evaluation = 0;

            mySqlCmd.Parameters.Add(new MySqlParameter("@evaluationId", evaluationId.Trim() == "" ? (object)DBNull.Value : evaluationId.Trim()));

            mySqlCmd.CommandText = @"SELECT 
                                        COUNT(*)
                                    FROM
                                        PROGRAM_EVALUATION
                                    WHERE
                                        TRAINING_PROGRAM_ID = '"+ evaluationId +@"'
                                            AND STATUS_CODE = '1';";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    evaluation = Convert.ToInt32(mySqlCmd.ExecuteScalar());
                }
                mySqlCon.Close();
            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }

            return evaluation;
        }

        public DataTable getRatingSchemeDetails(string schemeId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                String Qry = @"SELECT 
                                    RATING, WEIGHT, DESCRIPTION
                                FROM
                                    RS_RATINGS
                                WHERE
                                    RS_ID = '" + schemeId + "';";

                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;

            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
