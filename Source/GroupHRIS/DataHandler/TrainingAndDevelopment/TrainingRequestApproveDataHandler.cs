using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainingRequestApproveDataHandler : TemplateDataHandler
    {
        public DataTable getTrainingAllRequest(string year,string emp)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT tr.REQUEST_ID,tr.REQUEST_TYPE,rt.TYPE_NAME,tr.DESCRIPTION_OF_TRAINING,tr.SKILLS_EXPECTED,
                                   tr.NUMBER_OF_PARTICIPANTS,CONVERT( tr.REQUESTED_DATE , CHAR) AS REQUESTED_DATE,
                                   CASE
                                       WHEN tr.IS_RECOMENDED is null THEN 'Recommend' ELSE 'N/A'
                                   END AS STATUS_CODE
                            FROM TRAINING_REQUEST tr,REQUEST_TYPE rt
                            WHERE FINANCIAL_YEAR = '"+ year + @"' AND tr.STATUS_CODE = '1' AND 
	                              (tr.IS_RECOMENDED is null AND tr.IS_APPROVED is null)
                                  AND TO_RECOMMEND = '"+ emp +@"' AND tr.REQUEST_TYPE = rt.REQUEST_TYPE_ID                              
                            UNION ALL 
                            SELECT tr.REQUEST_ID,tr.REQUEST_TYPE,rt.TYPE_NAME,tr.DESCRIPTION_OF_TRAINING,tr.SKILLS_EXPECTED,
                                   tr.NUMBER_OF_PARTICIPANTS,CONVERT( tr.REQUESTED_DATE , CHAR) AS REQUESTED_DATE,
                                   CASE
                                       WHEN (tr.IS_APPROVED is null) THEN 'Approve' ELSE 'N/A'
                                       END AS STATUS_CODE
                            FROM TRAINING_REQUEST tr,REQUEST_TYPE rt
                            WHERE FINANCIAL_YEAR = '" + year +@"' AND tr.STATUS_CODE = '1' AND 
                                  (tr.IS_RECOMENDED = 1 AND tr.IS_APPROVED is null)
                                  AND tr.TO_APPROVE = '"+ emp +@"' 
                                  AND tr.REQUEST_TYPE = rt.REQUEST_TYPE_ID;";
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

        public String getRecommendCount(string empId, string year)
        {
            string count = "";

            mySqlCmd.CommandText = @"SELECT 
                                        COUNT(*)
                                    FROM
                                        TRAINING_REQUEST
                                    WHERE
                                        FINANCIAL_YEAR = '" + year + @"'
                                            AND TO_RECOMMEND = '" + empId + @"' AND (IS_RECOMENDED is null)
                                            AND STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "';";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    count = mySqlCmd.ExecuteScalar().ToString();
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

            return count;
        }

        public String getApprovedCount(string empId, string year)
        {
            string count = "";

            mySqlCmd.CommandText = @"SELECT 
                                            COUNT(tr.REQUEST_ID)
                                        FROM
                                            TRAINING_REQUEST tr,
                                            REQUEST_TYPE rt
                                        WHERE
                                            FINANCIAL_YEAR = '"+ year +@"'
                                                AND tr.STATUS_CODE = '1'
                                                AND (tr.IS_RECOMENDED = 1 AND tr.IS_APPROVED is null)
                                                AND tr.TO_APPROVE = '"+ empId +@"'
                                                AND tr.REQUEST_TYPE = rt.REQUEST_TYPE_ID;";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    count = mySqlCmd.ExecuteScalar().ToString();
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

            return count;
        }

        public DataTable getRecommendRequest(string year, string emp)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    tr.REQUEST_ID,
                                    tr.REQUEST_TYPE,
                                    rt.TYPE_NAME,
                                    tr.DESCRIPTION_OF_TRAINING,
                                    tr.SKILLS_EXPECTED,
                                    tr.NUMBER_OF_PARTICIPANTS,
                                    CONVERT( tr.REQUESTED_DATE , CHAR) AS REQUESTED_DATE,
                                    CASE
                                        WHEN tr.TO_RECOMMEND = '" + emp + @"' THEN 'Recommend'
		                                ELSE 'N/A'
                                    END AS STATUS_CODE
                                FROM
                                    TRAINING_REQUEST tr,
                                    REQUEST_TYPE rt
                                WHERE
                                    FINANCIAL_YEAR = '" + year + @"'
                                        AND tr.STATUS_CODE != '0' AND (tr.IS_RECOMENDED is null)
                                        AND TO_RECOMMEND = '" + emp + @"'
                                        AND tr.REQUEST_TYPE = rt.REQUEST_TYPE_ID;";
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

        public DataTable getApproveRequest(string year, string emp)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    tr.REQUEST_ID,
                                    tr.REQUEST_TYPE,
                                    rt.TYPE_NAME,
                                    tr.DESCRIPTION_OF_TRAINING,
                                    tr.SKILLS_EXPECTED,
                                    tr.NUMBER_OF_PARTICIPANTS,
                                    CONVERT( tr.REQUESTED_DATE , CHAR) AS REQUESTED_DATE,
                                    CASE
                                        WHEN (tr.IS_APPROVED is null) THEN 'Approve' ELSE 'N/A'
                                    END AS STATUS_CODE
                                FROM
                                    TRAINING_REQUEST tr,
                                    REQUEST_TYPE rt
                                WHERE
                                    FINANCIAL_YEAR = '"+ year +@"'
                                        AND tr.STATUS_CODE = '1'
                                        AND (tr.IS_RECOMENDED = 1 AND tr.IS_APPROVED is null)
                                        AND tr.TO_APPROVE = '"+ emp +@"'
                                        AND tr.REQUEST_TYPE = rt.REQUEST_TYPE_ID;";
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
