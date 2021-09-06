using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.PerformanceManagement
{
    public class ViewSelfAssessmentProfileDataHandler : TemplateDataHandler
    {
        public DataTable getSelfAssessmentProfileQuestions(string empId)
        {
            try
            {
                dataTable = new DataTable();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    e.EMPLOYEE_ID,
                                    e.ROLE_ID,agr.GROUP_ID,ag.GROUP_NAME,sap.SELF_ASSESSMENT_PROFILE_ID,sap.PROFILE_NAME,q.QUESTION_ID,
                                    q.NO_OF_ANSWERS,qb.QUESTION
                                FROM
                                    EMPLOYEE e,ASSESSMENT_GROUP_ROLES agr,
                                    ASSESSMENT_GROUP ag,SELF_ASSESSMENT_PROFILE sap,
                                    SELF_ASSESSMENT_PROFILE_QUESTIONS_BANK q,QUESTIONNAIRE_BANK qb
                                WHERE
                                    e.EMPLOYEE_ID = '" + empId + @"'
                                        AND agr.ROLE_ID = e.ROLE_ID
                                        AND agr.STATUS_CODE = 1
                                        AND ag.GROUP_ID = agr.GROUP_ID
                                        AND sap.GROUP_ID = agr.GROUP_ID
                                        AND sap.GROUP_ID = ag.GROUP_ID
                                        AND q.SELF_ASSESSMENT_PROFILE_ID = sap.SELF_ASSESSMENT_PROFILE_ID
                                        AND qb.QUESTION_ID = q.QUESTION_ID;";
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

        public Boolean Insert(string assessmentId, DataTable assmtAnswers, string user, string yer)
        {
            Boolean status = false;
            MySqlTransaction mySqlTrans = null;
            string questionId_pre = "";
            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                if (assmtAnswers.Rows.Count > 0)
                {
                    SerialHandler nserialcode = new SerialHandler();
                    string tokenId = nserialcode.getserila(mySqlCon, "TK");

                    int count = 1;

                    mySqlCmd.Parameters.Clear();

                    string assProId = assmtAnswers.Rows[0]["SELF_ASSESSMENT_PROFILE_ID"].ToString();
                    mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentId", assessmentId.Trim() == "" ? (object)DBNull.Value : assessmentId.Trim()));

                    mySqlCmd.Parameters.Add(new MySqlParameter("@tokenId", tokenId.Trim() == "" ? (object)DBNull.Value : tokenId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@assProId", assProId.Trim() == "" ? (object)DBNull.Value : assProId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@yer", yer.Trim() == "" ? (object)DBNull.Value : yer.Trim()));

                    string qry_esa = @"INSERT INTO EMPLOYEE_SELF_ASSESSMENT(ASSESSMENT_TOKEN,ASSESSMENT_ID,EMPLOYEE_ID,SELF_ASSESSMENT_PROFILE_ID,YEAR_OF_ASSESSMENT,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                    VALUES(@tokenId,@assessmentId,@user,@assProId,@yer,'" + 1 + "',@user,now(),@user,now())";

                    mySqlCmd.CommandText = qry_esa;
                    mySqlCmd.ExecuteNonQuery();


                    foreach (DataRow dr in assmtAnswers.Rows)
                    {
                        //   string assProId = dr["SELF_ASSESSMENT_PROFILE_ID"].ToString();
                        string questionId = dr["QUESTION_ID"].ToString();
                        string ansId = dr["NO_OF_ANSWERS"].ToString();
                        string answer = dr["ANSWER"].ToString();

                        if (questionId_pre != questionId)
                        {
                            count = 1;
                        }

                        //mySqlCmd.Parameters.Clear();

                        //mySqlCmd.Parameters.Add(new MySqlParameter("@sANS_ID", ansId.Trim() == "" ? (object)DBNull.Value : ansId.Trim()));
                        //mySqlCmd.Parameters.Add(new MySqlParameter("@questionId", questionId.Trim() == "" ? (object)DBNull.Value : questionId.Trim()));
                        //mySqlCmd.Parameters.Add(new MySqlParameter("@answer", answer.Trim() == "" ? (object)DBNull.Value : answer.Trim()));


                        string Qry = @"INSERT INTO SELF_ASSESSMENT_ANSWERS(SELF_ASSESSMENT_ANSWER_ID,ASSESSMENT_TOKEN,QUESTION_ID,ANSWER,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                    VALUES('" + count + "','" + tokenId + "','" + questionId + "','" + answer + "','" + 1 + "',@user,now(),@user,now())";

                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();

                        questionId_pre = questionId;
                        count++;
                    }


                    mySqlTrans.Commit();

                    mySqlTrans.Dispose();
                    mySqlCmd.Dispose();

                    status = true;
                }
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                    mySqlCmd.Transaction.Rollback();
                }

                throw ex;
            }
            finally
            {
                mySqlCon.Close();
            }
            return status;
        }

        public Boolean InsertifExist(string assessmentId, DataTable assmtAnswers, string user, string yer, string tokenId)
        {
            Boolean status = false;
            MySqlTransaction mySqlTrans = null;
            string questionId_pre = "";
            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                if (assmtAnswers.Rows.Count > 0)
                {
                    int count = 1;

                    mySqlCmd.Parameters.Clear();

                    string assProId = assmtAnswers.Rows[0]["SELF_ASSESSMENT_PROFILE_ID"].ToString();
                    mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentId", assessmentId.Trim() == "" ? (object)DBNull.Value : assessmentId.Trim()));

                    mySqlCmd.Parameters.Add(new MySqlParameter("@tokenId", tokenId.Trim() == "" ? (object)DBNull.Value : tokenId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@assProId", assProId.Trim() == "" ? (object)DBNull.Value : assProId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@yer", yer.Trim() == "" ? (object)DBNull.Value : yer.Trim()));

                    string qry_esa = @"DELETE FROM SELF_ASSESSMENT_ANSWERS WHERE ASSESSMENT_TOKEN = @tokenId";

                    mySqlCmd.CommandText = qry_esa;
                    mySqlCmd.ExecuteNonQuery();


                    foreach (DataRow dr in assmtAnswers.Rows)
                    {
                        string questionId = dr["QUESTION_ID"].ToString();
                        string ansId = dr["NO_OF_ANSWERS"].ToString();
                        string answer = dr["ANSWER"].ToString();

                        if (questionId_pre != questionId)
                        {
                            count = 1;
                        }

                        string Qry = @"INSERT INTO SELF_ASSESSMENT_ANSWERS(SELF_ASSESSMENT_ANSWER_ID,ASSESSMENT_TOKEN,QUESTION_ID,ANSWER,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                    VALUES('" + count + "','" + tokenId + "','" + questionId + "','" + answer + "','" + 1 + "',@user,now(),@user,now())";

                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();

                        questionId_pre = questionId;
                        count++;
                    }


                    mySqlTrans.Commit();

                    mySqlTrans.Dispose();
                    mySqlCmd.Dispose();

                    status = true;
                }
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                    mySqlCmd.Transaction.Rollback();
                }

                throw ex;
            }
            finally
            {
                mySqlCon.Close();
            }
            return status;
        }

        public Boolean FinalizedifExist(string tokenId, string user)
        {
            Boolean status = false;

            try
            {
                mySqlCon.Open();

                string Qry = @"UPDATE EMPLOYEE_SELF_ASSESSMENT 
                                    SET 
                                        STATUS_CODE = '2',MODIFIED_BY = '" + user + @"',MODIFIED_DATE = now()
                                    WHERE
                                        ASSESSMENT_TOKEN = '" + tokenId + "'";

                mySqlCmd.CommandText = Qry;
                mySqlCmd.ExecuteNonQuery();


                mySqlCmd.Dispose();

                status = true;
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                    mySqlCmd.Transaction.Rollback();
                }

                throw ex;
            }
            finally
            {
                mySqlCon.Close();
            }
            return status;
        }

        //public String getGoal(string tskId)
        //{
        //    string taskName = "";

        //    mySqlCmd.Parameters.Add(new MySqlParameter("@tskId", tskId.Trim() == "" ? (object)DBNull.Value : tskId.Trim()));

        //    mySqlCmd.CommandText = @"";

        //    try
        //    {
        //        mySqlCon.Open();

        //        if (mySqlCmd.ExecuteScalar() != null)
        //        {
        //            taskName = mySqlCmd.ExecuteScalar().ToString();
        //        }
        //        mySqlCon.Close();
        //    }

        //    catch (Exception ex)
        //    {
        //        if (mySqlCon.State == ConnectionState.Open)
        //        {
        //            mySqlCon.Close();
        //        }
        //        throw ex;
        //    }

        //    return taskName;
        //}

        public DataTable getexistAssessment(string empId, string assmentId, string year)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    ASSESSMENT_TOKEN
                                FROM
                                    EMPLOYEE_SELF_ASSESSMENT
                                WHERE
                                    ASSESSMENT_ID = '" + assmentId + "' AND EMPLOYEE_ID = '" + empId + @"'
                                        AND YEAR_OF_ASSESSMENT = '" + year + "';";
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

        public DataTable getexistAssessmentQuestions(string tokenId)
        {
            try
            {
                dataTable = new DataTable();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    SELF_ASSESSMENT_ANSWER_ID,QUESTION_ID, ANSWER
                                FROM
                                    SELF_ASSESSMENT_ANSWERS
                                WHERE
                                    ASSESSMENT_TOKEN = '" + tokenId + "' ORDER BY QUESTION_ID,SELF_ASSESSMENT_ANSWER_ID;";
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

        public String getStatus(string tskId)
        {
            string taskName = "";

            mySqlCmd.Parameters.Add(new MySqlParameter("@tskId", tskId.Trim() == "" ? (object)DBNull.Value : tskId.Trim()));

            mySqlCmd.CommandText = @"SELECT STATUS_CODE FROM EMPLOYEE_SELF_ASSESSMENT WHERE ASSESSMENT_TOKEN = @tskId";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    taskName = mySqlCmd.ExecuteScalar().ToString();
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

            return taskName;
        }
    }
}
