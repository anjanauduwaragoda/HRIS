using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainingSearchDataHandler : TemplateDataHandler
    {
        public DataTable getAllTraining()
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    t.TRAINING_ID,
                                    tp.PROGRAM_NAME,
                                    t.TRAINING_CODE,
                                    t.TRAINING_NAME,
                                    tt.TYPE_NAME,
                                    CONVERT( t.PLANNED_START_DATE , CHAR) As PLANNED_START_DATE,
                                    CONVERT( t.PLANNED_END_DATE , CHAR) AS PLANNED_END_DATE,
                                    t.PLANNED_PARTICIPANTS,
                                    t.PLANNED_TOTAL_HOURS
                                FROM
                                    TRAINING t,
                                    TRAINING_TYPE tt,
                                    TRAINING_PROGRAM tp
                                WHERE
                                    t.STATUS_CODE = '"+ Constants.CON_ACTIVE_STATUS +@"'
                                        AND tt.TRAINING_TYPE_ID = t.TRAINING_TYPE
                                        AND tp.PROGRAM_ID = t.TRAINING_PROGRAM_ID;";

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

        public DataTable getTrainingByType(string typeId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    TRAINING_ID,
                                    TRAINING_NAME,
                                    CASE
                                        WHEN TRAINING_TYPE = '0' THEN 'Sechedul Training'
                                        WHEN TRAINING_TYPE = '1' THEN 'Add hoc Training'
                                    END AS TRAINING_TYPE,
                                    CONVERT(PLANNED_START_DATE,CHAR) As PLANNED_START_DATE,
                                    CONVERT(PLANNED_END_DATE,CHAR) AS PLANNED_END_DATE,
                                    PLANNED_PARTICIPANTS,
                                    PLANNED_TOTAL_HOURS
                                FROM
                                    TRAINING
                                WHERE
                                    STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "' AND TRAINING_TYPE = '" + typeId + "'";

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

        public DataTable getTrainingProgrms()
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    PROGRAM_ID, PROGRAM_NAME
                                FROM
                                    TRAINING_PROGRAM
                                WHERE
                                    STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "';";

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

        public DataTable getTrainingType()
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    TRAINING_TYPE_ID, TYPE_NAME
                                FROM
                                    TRAINING_TYPE
                                WHERE
                                    STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "';";

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

        public DataTable filterTraining(string stDate,string endDate,string name,string code)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                t.TRAINING_ID,
                                tp.PROGRAM_NAME,
                                t.TRAINING_CODE,
                                t.TRAINING_NAME,
                                tt.TYPE_NAME,
                                CONVERT( t.PLANNED_START_DATE , CHAR) As PLANNED_START_DATE,
                                CONVERT( t.PLANNED_END_DATE , CHAR) AS PLANNED_END_DATE,
                                t.PLANNED_PARTICIPANTS,
                                t.PLANNED_TOTAL_HOURS
                            FROM
                                TRAINING t,
                                TRAINING_TYPE tt,
                                TRAINING_PROGRAM tp
                            WHERE
                                t.STATUS_CODE = '1'
                                    AND tt.TRAINING_TYPE_ID = t.TRAINING_TYPE
                                    AND tp.PROGRAM_ID = t.TRAINING_PROGRAM_ID ";

                //if (type != String.Empty)
                //{
                //    mySqlCmd.Parameters.Add(new MySqlParameter("@type", type));
                //    Qry += @" AND t.TRAINING_TYPE = @type ";
                //}

                //if (year != String.Empty)
                //{
                //    mySqlCmd.Parameters.Add(new MySqlParameter("@year",year));
                //    Qry += @" AND t.TRAINING_PROGRAM_ID = @year ";
                //}

                if (stDate != String.Empty)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@stDate", stDate));
                    Qry += @" AND t.PLANNED_START_DATE = @stDate ";
                }

                if (endDate != String.Empty)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@endDate", endDate));
                    Qry += @" AND t.PLANNED_END_DATE = @endDate ";
                }

                if (name != String.Empty)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@name", "%" + name + "%"));
                    Qry += @" AND t.TRAINING_NAME LIKE CONCAT('%','"+name+"','%') ";
                }

                if (code != String.Empty)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@code", "%" + code + "%"));
                    Qry += @" AND t.TRAINING_CODE LIKE CONCAT('%','"+code+"','%') ";
                }

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
