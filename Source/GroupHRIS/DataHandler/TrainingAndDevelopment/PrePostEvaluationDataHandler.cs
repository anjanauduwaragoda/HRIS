using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataHandler.TrainingAndDevelopment
{
    public class PrePostEvaluationDataHandler : TemplateDataHandler
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
                                e.TRAINING_PROGRAM_ID = '" + programId + @"'
                                    AND (e.RS_ID = r.RS_ID || e.RS_ID IS NULL) GROUP BY e.EVALUATION_ID AND e.STATUS_CODE = '1';";
                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                mySqlCon.Close();
            }
        }

        public DataTable getParticipantDetails(string trainingId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                String Qry = @"SELECT 
                                    tp.COMPANY_ID,
                                    c.COMP_NAME,
                                    tp.EMPLOYEE_ID,
                                    e.KNOWN_NAME,
                                    tp.EPF,
                                    tp.DESIGNATION,
                                    ed.DESIGNATION_NAME,
                                    tp.DEPARTMENT,
                                    d.DEPT_NAME,
                                    tp.DIVISION,
                                    dv.DIV_NAME,
                                    tp.BRANCH,
                                    (SELECT 
                                            KNOWN_NAME
                                        FROM
                                            EMPLOYEE
                                        WHERE
                                            EMPLOYEE_ID = tp.REPORTING_HEAD) AS REPORTING_HEAD,
                                    tp.REPORTING_HEAD
                                FROM
                                    TRAINING_PARTICIPANTS tp,
                                    COMPANY c,
                                    DEPARTMENT d,
                                    EMPLOYEE_DESIGNATION ed,
                                    DIVISION dv,
                                    EMPLOYEE e
                                WHERE
                                    tp.TRAINING_ID = '"+trainingId +@"'
                                        AND d.DEPT_ID = tp.DEPARTMENT
                                        AND ed.DESIGNATION_ID = tp.DESIGNATION
                                        AND dv.DIVISION_ID = tp.DIVISION
                                        AND tp.COMPANY_ID = c.COMPANY_ID
                                        AND tp.EMPLOYEE_ID = e.EMPLOYEE_ID;";

                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                mySqlCon.Close();
            }
        }

        public String getTrainingProgrm(string trId)
        {
            string progrmId = "";

            mySqlCmd.CommandText = @"SELECT 
                                        TRAINING_PROGRAM_ID
                                    FROM
                                        TRAINING
                                    WHERE
                                        TRAINING_ID = '" + trId + "';";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    progrmId = mySqlCmd.ExecuteScalar().ToString();
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

            return progrmId;
        }

        public Boolean Insert(String trainingId,string evaluationId, string isPreEval, DataTable empDetails, String stDate, String endDate, String status, String comments, String user)
        {
            Boolean blInserted = false;

            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@trainingId", trainingId.Trim() == "" ? (object)DBNull.Value : trainingId.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@comments", comments.Trim() == "" ? (object)DBNull.Value : comments.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@stDate", stDate.Trim() == "" ? (object)DBNull.Value : stDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@endDate", endDate.Trim() == "" ? (object)DBNull.Value : endDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@isPreEvaluation", isPreEval.Trim() == "" ? (object)DBNull.Value : isPreEval.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@evaluationId", evaluationId.Trim() == "" ? (object)DBNull.Value : evaluationId.Trim()));
               
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Transaction = mySqlTrans;

                    foreach (DataRow dr in empDetails.Rows)
                    {
                        string empId = dr["EMPLOYEE_ID"].ToString();
                        string compId = dr["COMPANY_ID"].ToString();

                        string s_Evaluater = dr["SUPERVISOR_EVALUATION"].ToString();
                        string e_Evaluater = dr["EMPLOYEE_EVALUATION"].ToString();

                       if (s_Evaluater == "1")
                        {
                            string qry = @"SELECT 
                                                REPORT_TO_1
                                            FROM
                                                EMPLOYEE
                                            WHERE
                                                EMPLOYEE_ID = '" + empId + "';";

                            mySqlCmd.CommandText = qry;
                            if (mySqlCmd.ExecuteScalar() != null)
                            {
                                s_Evaluater = mySqlCmd.ExecuteScalar().ToString();
                            }

                        }
                        else
                        {
                            s_Evaluater = empId;
                        }

                        sMySqlString = @"INSERT INTO PARTICIPANT_TRAINING_EVALUATION(TRAINING_ID,EVALUATION_ID,EMPLOYEE_ID,COMPANY_ID,EVALUATOR,COMMENTS,IS_POST_EVALUATION,EVALUATION_START_DATE,EVALUATION_END_DATE,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE)
                                    VALUES(@trainingId, @evaluationId ,'" + empId + "','" + compId + "','" + s_Evaluater + "',@comments,@isPreEvaluation,@stDate,@endDate,@status,@user,NOW(),@user,NOW());";


                        mySqlCmd.CommandText = sMySqlString;

                        mySqlCmd.ExecuteNonQuery();
                    }

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

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
                mySqlCon.Close();
            }

            return blInserted;
        }

        public Boolean Update(string trainingId, string evaluationId, string isPreEval, DataTable empDetails, string stDate, string endDate, string status, string comments, string user)
        {
            Boolean blInserted = false;

            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            string qDeletTraining = "";

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@trainingId", trainingId.Trim() == "" ? (object)DBNull.Value : trainingId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@comments", comments.Trim() == "" ? (object)DBNull.Value : comments.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@stDate", stDate.Trim() == "" ? (object)DBNull.Value : stDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@endDate", endDate.Trim() == "" ? (object)DBNull.Value : endDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@isPreEvaluation", isPreEval.Trim() == "" ? (object)DBNull.Value : isPreEval.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@evaluationId", evaluationId.Trim() == "" ? (object)DBNull.Value : evaluationId.Trim()));
               
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Transaction = mySqlTrans;

                if (isPreEval == "1")
                {
                    qDeletTraining = @"DELETE FROM PARTICIPANT_TRAINING_EVALUATION 
                                    WHERE
                                        TRAINING_ID = @trainingId AND IS_POST_EVALUATION = '1';";
                    mySqlCmd.CommandText = qDeletTraining;
                    mySqlCmd.ExecuteNonQuery();


                    foreach (DataRow dr in empDetails.Rows)
                    {
                        string empId = dr["EMPLOYEE_ID"].ToString();
                        string compId = dr["COMPANY_ID"].ToString();

                        string s_Evaluater = dr["SUPERVISOR_EVALUATION"].ToString();
                        string e_Evaluater = dr["EMPLOYEE_EVALUATION"].ToString();

                        if (s_Evaluater == "1")
                        {
                            string qry = @"SELECT 
                                                REPORT_TO_1
                                            FROM
                                                EMPLOYEE
                                            WHERE
                                                EMPLOYEE_ID = '" + empId + "';";

                            mySqlCmd.CommandText = qry;
                            if (mySqlCmd.ExecuteScalar() != null)
                            {
                                s_Evaluater = mySqlCmd.ExecuteScalar().ToString();
                            }

                        }
                        else
                        {
                            s_Evaluater = empId;
                        }

                        sMySqlString = @"INSERT INTO PARTICIPANT_TRAINING_EVALUATION(TRAINING_ID,EVALUATION_ID,EMPLOYEE_ID,COMPANY_ID,EVALUATOR,COMMENTS,IS_POST_EVALUATION,EVALUATION_START_DATE,EVALUATION_END_DATE,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE)
                                    VALUES(@trainingId, @evaluationId ,'" + empId + "','" + compId + "','" + s_Evaluater + "',@comments,@isPreEvaluation,@stDate,@endDate,@status,@user,NOW(),@user,NOW());";


                        mySqlCmd.CommandText = sMySqlString;

                        mySqlCmd.ExecuteNonQuery();
                    }

                }
                else
                {
                    qDeletTraining = @"DELETE FROM PARTICIPANT_TRAINING_EVALUATION 
                                    WHERE
                                        TRAINING_ID = @trainingId AND IS_POST_EVALUATION = '0';";
                    mySqlCmd.CommandText = qDeletTraining;
                    mySqlCmd.ExecuteNonQuery();


                    foreach (DataRow dr in empDetails.Rows)
                    {
                        string empId = dr["EMPLOYEE_ID"].ToString();
                        string compId = dr["COMPANY_ID"].ToString();

                        string s_Evaluater = dr["SUPERVISOR_EVALUATION"].ToString();
                        string e_Evaluater = dr["EMPLOYEE_EVALUATION"].ToString();

                        if (s_Evaluater == "1")
                        {
                            string qry = @"SELECT 
                                                REPORT_TO_1
                                            FROM
                                                EMPLOYEE
                                            WHERE
                                                EMPLOYEE_ID = '" + empId + "';";

                            mySqlCmd.CommandText = qry;
                            if (mySqlCmd.ExecuteScalar() != null)
                            {
                                s_Evaluater = mySqlCmd.ExecuteScalar().ToString();
                            }

                        }
                        else
                        {
                            s_Evaluater = empId;
                        }

                        sMySqlString = @"INSERT INTO PARTICIPANT_TRAINING_EVALUATION(TRAINING_ID,EVALUATION_ID,EMPLOYEE_ID,COMPANY_ID,EVALUATOR,COMMENTS,IS_POST_EVALUATION,EVALUATION_START_DATE,EVALUATION_END_DATE,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE)
                                    VALUES(@trainingId, @evaluationId ,'" + empId + "','" + compId + "','" + s_Evaluater + "',@comments,@isPreEvaluation,@stDate,@endDate,@status,@user,NOW(),@user,NOW());";


                        mySqlCmd.CommandText = sMySqlString;

                        mySqlCmd.ExecuteNonQuery();
                    }
                }
                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

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

        public DataTable getExistData(string trainingId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                String Qry = @"SELECT 
                                    EVALUATION_ID,
                                    EMPLOYEE_ID,
                                    COMPANY_ID,
                                    EVALUATOR,
                                    COMMENTS,
                                    IS_POST_EVALUATION,
                                    CONVERT(EVALUATION_START_DATE,CHAR) AS EVALUATION_START_DATE,
                                    CONVERT(EVALUATION_END_DATE,CHAR) AS EVALUATION_END_DATE,
                                    CASE
                                        WHEN STATUS_CODE = '1' THEN 'Active'
                                        WHEN STATUS_CODE = '0' THEN 'Inactive'
                                    END AS STATUS_CODE
                                FROM
                                    PARTICIPANT_TRAINING_EVALUATION
                                WHERE
                                    TRAINING_ID = '" + trainingId + "'"; //

                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                mySqlCon.Close();
            }
        }


    }
}
