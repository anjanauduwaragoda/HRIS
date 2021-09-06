using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class EmployeeSelfTrainigEvaluationDashboardDataHandler : TemplateDataHandler
    {
        public DataTable getAllEvaluations(string employeeId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                //PTE.IS_POST_EVALUATION,
                //                    PTE.EVALUATION_START_DATE,
                //                    PTE.EVALUATION_END_DATE,
                //                    PTE.STATUS_CODE
                string sqlQuery = @"SELECT 
                                    PTE.TRAINING_ID,
                                    T.TRAINING_NAME,
                                    TP.PROGRAM_NAME,
                                    PTE.EVALUATION_ID,
                                    PE.EVALUATION_NAME,
                                    PE.MCQ_INCLUDED,
                                    PE.EQ_INCLUDED,
                                    PE.RQ_INCLUDED,
                                    CAST(PTE.IS_POST_EVALUATION AS CHAR) AS POST_EVALUATION,
                                    CAST(PTE.EVALUATION_START_DATE AS CHAR) START_DATE,
                                    CAST(PTE.EVALUATION_END_DATE AS CHAR) AS END_DATE,
                                    PTE.STATUS_CODE AS STATUS 
                                    
                                FROM
                                    PARTICIPANT_TRAINING_EVALUATION PTE
                                        LEFT JOIN
                                    TRAINING T ON PTE.TRAINING_ID = T.TRAINING_ID
                                        LEFT JOIN
                                    TRAINING_PROGRAM TP ON T.TRAINING_PROGRAM_ID = TP.PROGRAM_ID
                                        LEFT JOIN
                                    PROGRAM_EVALUATION PE ON PTE.EVALUATION_ID = PE.EVALUATION_ID
                                WHERE
                                    PTE.EMPLOYEE_ID = '" + employeeId + "' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);
                return resultTable;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                resultTable.Dispose();
            }
        }

        public int getFinalizedAnswerCount(string answerTable, string evaluationId, string trainingId, string employeeId, string isPostEvaluation)
        {
            int answerCount = 0;
            DataTable dtResult = new DataTable();
            try
            {
                string sqlQuery = String.Empty;

                if (answerTable == "TE_MCQ_ANSWERS")
                {
                    sqlQuery += " SELECT COUNT(MCQ_ID) AS ANSWER_COUNT FROM TE_MCQ_ANSWERS ";
                }
                else if (answerTable == "TE_RATING_QUESTION_ANSWERS")
                {
                    sqlQuery += " SELECT COUNT(RQ_ID) AS ANSWER_COUNT FROM TE_RATING_QUESTION_ANSWERS ";
                }
                else if (answerTable == "TE_EQ_ANSWERS")
                {
                    sqlQuery += " SELECT COUNT(ESSAY_QUESTION_ID) AS ANSWER_COUNT FROM TE_EQ_ANSWERS ";
                }
                sqlQuery += " WHERE EVALUATION_ID = '" + evaluationId + "' AND TRAINING_ID ='" + trainingId + "' AND EMPLOYEE_ID ='" + employeeId + "' AND IS_FINALIZED ='" + Constants.STATUS_ACTIVE_VALUE + "' AND IS_POST_EVALUATION='"+isPostEvaluation+"' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(dtResult);

                if (dtResult.Rows.Count > 0)
                {
                    answerCount = Convert.ToInt32(dtResult.Rows[0][0].ToString());
                }
                return answerCount;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
