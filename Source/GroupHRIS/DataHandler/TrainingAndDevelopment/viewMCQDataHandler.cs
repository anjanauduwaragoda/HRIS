using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.TrainingAndDevelopment
{
    public class viewMCQDataHandler:TemplateDataHandler
    {
        public DataTable getMCQ(string evalId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    m.MCQ_ID, m.CHOICES, m.QUESTION, a.ANSWER_ID, a.ANSWER
                                FROM
                                    MULTIPLE_CHOICE_QUESTIONS m,
                                    MCQ_ANSWERS a
                                WHERE
                                    m.EVALUATION_ID = '"+ evalId +@"'
                                        AND a.MCQ_ID = m.MCQ_ID;";

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
