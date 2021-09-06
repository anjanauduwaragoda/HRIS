using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.PerformanceManagement
{
    public class ViewQuestionDataHandler : TemplateDataHandler
    {
        public DataTable getAllQuestins()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"
                                SELECT 
                                    QUESTION_ID, 
                                    QUESTION, 
                                    REMARKS 
                                FROM 
                                    QUESTIONNAIRE_BANK 
                                WHERE 
                                    STATUS_CODE = '1' 
                                ORDER BY 
                                    SUBSTRING(QUESTION_ID, 3,9)+0 ASC
                            ";
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

        //public DataTable viewQuestion(DataTable questionBucket)
        //{
        //    try
        //    {
        //        if (mySqlCon.State == ConnectionState.Closed)
        //        {
        //            mySqlCon.Open();
        //        }

        //        DataTable dt = new DataTable();

        //        dt.Columns.Add("QUESTION_ID");
        //        dt.Columns.Add("QUESTION");
        //        dt.Columns.Add("REMARKS");
        //        dt.Columns.Add("NO_OF_ANSWERS");

        //        foreach (DataRow dr in questionBucket.Rows)
        //        {
        //            string questionId = dr["QUESTION_ID"].ToString();
        //            string noOfAns = dr["NO_OF_ANSWERS"].ToString();

        //            string Qry = @"SELECT QUESTION_ID,QUESTION,REMARKS FROM QUESTIONNAIRE_BANK WHERE STATUS_CODE = '1' AND QUESTION_ID = '" + questionId + "';";

        //            mySqlCmd.CommandText = Qry;
        //            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
        //            dataAdapter.Fill(dataTable);

        //        }

        //        for (int i = 0; i < dataTable.Rows.Count; i++)
        //        {
        //            DataRow drv = dt.NewRow();
        //            drv["QUESTION_ID"] = dataTable.Rows[i]["QUESTION_ID"].ToString();
        //            drv["QUESTION"] = dataTable.Rows[i]["QUESTION"].ToString();
        //            drv["REMARKS"] = dataTable.Rows[i]["REMARKS"].ToString();
        //            drv["NO_OF_ANSWERS"] = dataTable.Rows[i]["NO_OF_ANSWERS"].ToString();
        //            dt.Rows.Add(drv);
        //        }

        //        return dt;
        //    }
        //    finally
        //    {
        //        mySqlCmd.Parameters.Clear();
        //        if (mySqlCon.State == ConnectionState.Open)
        //        {
        //            mySqlCon.Close();
        //        }
        //    }

        //}

        public String getQuestion(string qId)
        {
            string qName = "";


            mySqlCmd.CommandText = @"
                                        SELECT 
                                            1 
                                        FROM 
                                            QUESTIONNAIRE_BANK 
                                        WHERE 
                                            QUESTION_ID = 'Q1';
                                    ";

            try
            {
                mySqlCon.Open();
                if (mySqlCmd.ExecuteScalar() != null)
                {
                    qName = mySqlCmd.ExecuteScalar().ToString();
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

            return qName;
        }
    }
}
