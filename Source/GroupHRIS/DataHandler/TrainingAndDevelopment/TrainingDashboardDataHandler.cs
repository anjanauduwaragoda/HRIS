using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainingDashboardDataHandler : TemplateDataHandler
    {

        public DataTable getAvailableTrainingEvaluations(string empId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    e.TRAINING_ID,
                                    t.TRAINING_NAME,
                                    e.EVALUATION_ID,
                                    p.EVALUATION_NAME,
                                    e.EMPLOYEE_ID,
                                    e.EVALUATOR,
                                    CASE
                                        WHEN IS_POST_EVALUATION = '1' THEN 'Pre Evaluation'
                                        WHEN IS_POST_EVALUATION = '0' THEN 'Post Evaluation'
                                    END AS IS_POST_EVALUATION,
                                    CONVERT(e.EVALUATION_START_DATE,CHAR) AS EVALUATION_START_DATE,
                                    CONVERT(e.EVALUATION_END_DATE,CHAR) As EVALUATION_END_DATE,
                                    CASE
                                        WHEN e.STATUS_CODE = '1' THEN 'Active'
                                        WHEN e.STATUS_CODE = '0' THEN 'Inactive'
                                    END AS STATUS_CODE
                                FROM
                                    PARTICIPANT_TRAINING_EVALUATION e,
                                    TRAINING t,
                                    PROGRAM_EVALUATION p
                                WHERE
                                    e.TRAINING_ID = t.TRAINING_ID
                                        AND e.EVALUATION_ID = p.EVALUATION_ID
                                        AND e.EVALUATOR = '" + empId + "';";

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

        public DataTable searchAvailableTrainingEvaluations(string assignee, string empId, string training, string evaluation, string frmDate, string toDate)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string Qry = @"SELECT 
                                    e.TRAINING_ID,
                                    t.TRAINING_NAME,
                                    e.EVALUATION_ID,
                                    p.EVALUATION_NAME,
                                    e.EMPLOYEE_ID,
                                    e.EVALUATOR,
                                    CASE
                                        WHEN IS_POST_EVALUATION = '1' THEN 'Pre Evaluation'
                                        WHEN IS_POST_EVALUATION = '0' THEN 'Post Evaluation'
                                    END AS IS_POST_EVALUATION,
                                    CONVERT(e.EVALUATION_START_DATE,CHAR) AS EVALUATION_START_DATE,
                                    CONVERT(e.EVALUATION_END_DATE,CHAR) As EVALUATION_END_DATE,
                                    CASE
                                        WHEN e.STATUS_CODE = '1' THEN 'Active'
                                        WHEN e.STATUS_CODE = '0' THEN 'Inactive'
                                    END AS STATUS_CODE
                                FROM
                                    PARTICIPANT_TRAINING_EVALUATION e,
                                    TRAINING t,
                                    PROGRAM_EVALUATION p
                                WHERE
                                    e.TRAINING_ID = t.TRAINING_ID
                                        AND e.EVALUATION_ID = p.EVALUATION_ID
                                        AND e.EVALUATOR = '" + empId + "'";

                if (assignee != "")
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", empId));

                    if (assignee == "0")
                    {
                        Qry += @" AND e.EMPLOYEE_ID = @EMPLOYEE_ID ";
                    }
                    else
                    {
                        Qry += @" AND e.EMPLOYEE_ID != @EMPLOYEE_ID ";
                    }
                }

                if (evaluation != "")
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_POST_EVALUATION", evaluation));
                    Qry += @" AND e.IS_POST_EVALUATION != @IS_POST_EVALUATION ";
                }

                if (frmDate != String.Empty)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@frmDate", frmDate));
                    Qry += @" AND e.EVALUATION_START_DATE = @frmDate ";
                }

                if (toDate != String.Empty)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@toDate", toDate));
                    Qry += @" AND e.EVALUATION_END_DATE = @toDate ";
                }

                if (training != String.Empty)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@training", "%" + training + "%"));
                    Qry += @" AND t.TRAINING_NAME LIKE CONCAT('%','" + training + "','%') ";
                }
                Qry += @" ORDER BY e.TRAINING_ID; ";


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

        public DataTable getEvaluationDetails(string evaluationId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    EVALUATION_ID, RS_ID, MCQ_INCLUDED, EQ_INCLUDED, RQ_INCLUDED
                                FROM
                                    PROGRAM_EVALUATION
                                WHERE
                                    EVALUATION_ID = '" + evaluationId + "';";

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
