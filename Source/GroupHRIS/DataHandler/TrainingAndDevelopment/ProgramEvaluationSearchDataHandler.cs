using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.TrainingAndDevelopment
{
    public class ProgramEvaluationSearchDataHandler : TemplateDataHandler
    {
        public DataTable getAllPrograms()
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    pe.EVALUATION_ID,
                                    pe.RS_ID,r.RS_NAME,
                                    pe.TRAINING_PROGRAM_ID,
                                    p.PROGRAM_NAME,
                                    pe.EVALUATION_NAME,
                                    CASE WHEN pe.MCQ_INCLUDED = '1' THEN 'Yes' WHEN pe.MCQ_INCLUDED = '0' THEN 'No' END AS MCQ_INCLUDED,
                                    CASE WHEN pe.EQ_INCLUDED = '1' THEN 'Yes'WHEN pe.EQ_INCLUDED = '0' THEN 'No' END AS EQ_INCLUDED ,
                                    CASE WHEN pe.RQ_INCLUDED = '1' THEN 'Yes'WHEN pe.RQ_INCLUDED = '0' THEN 'No' END AS RQ_INCLUDED 
                                FROM
                                    PROGRAM_EVALUATION pe,
                                    TRAINING_PROGRAM p,RATING_SCHEME r
                                WHERE
                                    pe.STATUS_CODE = '1'
                                        AND p.PROGRAM_ID = pe.TRAINING_PROGRAM_ID AND pe.RS_ID = r.RS_ID;";

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
