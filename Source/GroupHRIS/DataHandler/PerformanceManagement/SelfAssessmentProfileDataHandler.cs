using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class SelfAssessmentProfileDataHandler : TemplateDataHandler
    {
        public DataTable getAssessmentGroup()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT GROUP_ID,GROUP_NAME FROM ASSESSMENT_GROUP WHERE STATUS_CODE = '1';";
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

        public bool Insert(string grpname, string proName, string des, string status, string user,DataTable questionList)
        {
            bool isInsert = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;
            string questionId = "";
            string noOfAns = "";
            string exclude = "";

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler nserialcode = new SerialHandler();
                string pro_id = nserialcode.getserila(mySqlCon, "PRO");

                mySqlCmd.Parameters.Add(new MySqlParameter("@pro_id", pro_id.Trim() == "" ? (object)DBNull.Value : pro_id.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@grpname", grpname.Trim() == "" ? (object)DBNull.Value : grpname.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@progress", proName.Trim() == "" ? (object)DBNull.Value : proName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@des", des.Trim() == "" ? (object)DBNull.Value : des.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));

                sMySqlString = @"INSERT INTO SELF_ASSESSMENT_PROFILE(SELF_ASSESSMENT_PROFILE_ID,GROUP_ID,PROFILE_NAME,DESCRIPTION,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                    VALUES(@pro_id,@grpname,@progress,@des,@status,@user,now(),@user,now())";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                foreach (DataRow dr in questionList.Rows)
                {
                    questionId = dr["QUESTION_ID"].ToString();
                    noOfAns = dr["NO_OF_ANSWERS"].ToString();
                    exclude = dr["EXCLUDE"].ToString();

                    if (exclude == "False")
                    {
                        sMySqlString = @"INSERT INTO SELF_ASSESSMENT_PROFILE_QUESTIONS_BANK(SELF_ASSESSMENT_PROFILE_ID,QUESTION_ID,NO_OF_ANSWERS,REMARKS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                    VALUES(@pro_id,'" + questionId + "','" + noOfAns + "',@des,@status,@user,now(),@user,now())";
                        mySqlCmd.Transaction = mySqlTrans;
                        mySqlCmd.CommandText = sMySqlString;

                        mySqlCmd.ExecuteNonQuery();
                    }
                }

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isInsert = true;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return isInsert;
        }

        public bool Update(string pro_id,string grpname, string proName, string des, string status, string user, DataTable questionList)
        {
            bool isUpdate = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;
            string questionId = "";
            string noOfAns = "";
            string exclude = "";

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@pro_id", pro_id.Trim() == "" ? (object)DBNull.Value : pro_id.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@grpname", grpname.Trim() == "" ? (object)DBNull.Value : grpname.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@proName", proName.Trim() == "" ? (object)DBNull.Value : proName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@des", des.Trim() == "" ? (object)DBNull.Value : des.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));
                
                sMySqlString = @"UPDATE SELF_ASSESSMENT_PROFILE 
                                        SET 
                                            GROUP_ID = @grpname,
	                                        PROFILE_NAME = @proName,
	                                        DESCRIPTION = @des,
                                            STATUS_CODE = @status,
                                            MODIFIED_BY = @user,
                                            MODIFIED_DATE = now()
                                        WHERE
                                            SELF_ASSESSMENT_PROFILE_ID = @pro_id;";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                foreach (DataRow dr in questionList.Rows)
                {
                    questionId = dr["QUESTION_ID"].ToString();
                    noOfAns = dr["NO_OF_ANSWERS"].ToString();
                    exclude = dr["EXCLUDE"].ToString();

                    string Qry = @"SELECT * FROM SELF_ASSESSMENT_PROFILE_QUESTIONS_BANK WHERE QUESTION_ID = '" + questionId + "' AND SELF_ASSESSMENT_PROFILE_ID = @pro_id;";
                    
                    mySqlCmd.Connection = mySqlCon;
                    mySqlCmd.CommandText = Qry;

                    MySqlDataAdapter da = new MySqlDataAdapter(mySqlCmd);
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        if (exclude == "True")
                        {
                            sMySqlString = @"DELETE FROM SELF_ASSESSMENT_PROFILE_QUESTIONS_BANK 
                                            WHERE
                                                SELF_ASSESSMENT_PROFILE_ID = @pro_id AND QUESTION_ID = '" + questionId + "'";
                        }
                        else
                        {
                            sMySqlString = @"UPDATE SELF_ASSESSMENT_PROFILE_QUESTIONS_BANK 
                                            SET 
                                                NO_OF_ANSWERS = '" + noOfAns + @"',
                                                REMARKS = @des,
                                                MODIFIED_BY = @user,
                                                MODIFIED_DATE = now()
                                            WHERE
                                                SELF_ASSESSMENT_PROFILE_ID = @pro_id
                                                    AND QUESTION_ID = '" + questionId + "'";

                        }
                        mySqlCmd.Transaction = mySqlTrans;
                        mySqlCmd.CommandText = sMySqlString;
                        mySqlCmd.ExecuteNonQuery();
                    }
                    else //Update question bank
                    {
                        sMySqlString = @"INSERT INTO SELF_ASSESSMENT_PROFILE_QUESTIONS_BANK(SELF_ASSESSMENT_PROFILE_ID,QUESTION_ID,NO_OF_ANSWERS,REMARKS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                       VALUES(@pro_id,'" + questionId + "','" + noOfAns + "',@des,@status,@user,now(),@user,now())";
                        mySqlCmd.Transaction = mySqlTrans;
                        mySqlCmd.CommandText = sMySqlString;

                        mySqlCmd.ExecuteNonQuery();
                    }
                    
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
        
        public DataTable getSelfAssessmentProfile()
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                ap.SELF_ASSESSMENT_PROFILE_ID,
                                ag.GROUP_NAME,
                                ap.DESCRIPTION,
                                ap.PROFILE_NAME,
                                CASE
                                    WHEN ap.STATUS_CODE = '1' then 'Active'
                                    WHEN ap.STATUS_CODE = '0' then 'Inactive'
                                End as STATUS_CODE
                            FROM
                                SELF_ASSESSMENT_PROFILE ap,
                                ASSESSMENT_GROUP ag
                            WHERE
                                ap.GROUP_ID = ag.GROUP_ID AND ag.STATUS_CODE = '1';";
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

        public DataTable getAssessmentProfileQuestion(string assProId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    b.QUESTION_ID,q.QUESTION, b.NO_OF_ANSWERS,
                                CASE
                                    WHEN b.STATUS_CODE = '1' then 'False'
                                    WHEN b.STATUS_CODE = '0' then 'True'
                                End as EXCLUDE
                                FROM
                                    SELF_ASSESSMENT_PROFILE_QUESTIONS_BANK b,QUESTIONNAIRE_BANK q
                                WHERE
                                b.QUESTION_ID = q.QUESTION_ID AND
                                    b.SELF_ASSESSMENT_PROFILE_ID = '" + assProId + "';";

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

        //public Boolean IsExist(string qId)
        //{
        //    Boolean Status = false;

        //    try
        //    {
        //        if (mySqlCon.State == ConnectionState.Closed)
        //        {
        //            mySqlCon.Open();
        //        }

        //        string Qry = @"SELECT * FROM SELF_ASSESSMENT_PROFILE_QUESTIONS_BANK WHERE QUESTION_ID = @qId ;";
        //        mySqlCmd.Parameters.Add(new MySqlParameter("@qId", qId));
        //        mySqlCmd.Connection = mySqlCon;
        //        mySqlCmd.CommandText = Qry;

        //        MySqlDataAdapter da = new MySqlDataAdapter(mySqlCmd);
        //        DataTable dt = new DataTable();

        //        da.Fill(dt);

        //        if (dt.Rows.Count > 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }

        //    }
        //    finally
        //    {
        //        mySqlCmd.Parameters.Clear();
        //        if (mySqlCon.State == ConnectionState.Open)
        //        {
        //            mySqlCon.Close();
        //        }
        //    }
        //    return Status;
        //}

        public String getAssGroupId(string groupId)
        {
            string id = "";

            mySqlCmd.Parameters.Add(new MySqlParameter("@groupId", groupId.Trim() == "" ? (object)DBNull.Value : groupId.Trim()));

            mySqlCmd.CommandText = @"SELECT 
                                        SELF_ASSESSMENT_PROFILE_ID
                                    FROM
                                        SELF_ASSESSMENT_PROFILE
                                    WHERE
                                        GROUP_ID = @groupId
                                            AND STATUS_CODE = '1'";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    id = mySqlCmd.ExecuteScalar().ToString();
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

            return id;
        }

        public DataTable getAssessmentList(string assProId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    sap.SELF_ASSESSMENT_PROFILE_ID,
                                    sap.GROUP_ID,
                                    ag.GROUP_NAME,
                                    sap.PROFILE_NAME,
                                    agr.ROLE_ID,
                                    ae.ASSESSMENT_ID,
                                    ae.EMPLOYEE_ID,
                                    a.STATUS_CODE
                                FROM
                                    SELF_ASSESSMENT_PROFILE sap,
                                    ASSESSMENT_GROUP ag,
                                    ASSESSMENT_GROUP_ROLES agr,
                                    ASSESSED_EMPLOYEES ae,
                                    ASSESSMENT a
                                WHERE
                                    ag.GROUP_ID = sap.GROUP_ID
                                        and agr.GROUP_ID = ag.GROUP_ID
                                        AND sap.SELF_ASSESSMENT_PROFILE_ID = '"+ assProId +@"'
                                        AND agr.ROLE_ID = ae.ROLE_ID
                                        AND a.ASSESSMENT_ID = ae.ASSESSMENT_ID
                                GROUP BY ae.ASSESSMENT_ID , agr.GROUP_ID;";
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
