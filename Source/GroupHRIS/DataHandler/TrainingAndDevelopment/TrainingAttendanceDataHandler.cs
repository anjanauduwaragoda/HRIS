using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Common;
using System.Data;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainingAttendanceDataHandler : TemplateDataHandler
    {
        /// <summary>
        /// Get Training Types from Database
        /// </summary>
        /// <returns>DataTale with Training Type Details </returns>
        public DataTable PopulateTrainingType()
        {
            try
            {
                dataTable = new DataTable();
                mySqlCon.Open();

                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                TRAINING_TYPE_ID, TYPE_NAME
                                            FROM
                                                TRAINING_TYPE
                                            WHERE
                                                STATUS_CODE = @STATUS_CODE
                                            ORDER BY TYPE_NAME ASC;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);



                mySqlDa.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                dataTable.Dispose();
                mySqlCmd.Dispose();
            }
        }

        /// <summary>
        /// Get Training Programs from Database
        /// </summary>
        /// <returns>Datatable with Training program Details</returns>
        public DataTable PopulateTrainingPrograms()
        {
            try
            {
                dataTable = new DataTable();
                mySqlCon.Open();

                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                PROGRAM_ID, PROGRAM_NAME
                                            FROM
                                                TRAINING_PROGRAM
                                            WHERE
                                                STATUS_CODE = @STATUS_CODE
                                            ORDER BY PROGRAM_NAME ASC;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Constants.CON_ACTIVE_STATUS));


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);



                mySqlDa.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                dataTable.Dispose();
                mySqlCmd.Dispose();
            }
        }

        /// <summary>
        /// This Method is Used to Get Trainings Accroding to the Filters
        /// </summary>
        /// <param name="TrainingType"> Training Type </param>
        /// <param name="TrainingCode"> Training Code </param>
        /// <param name="TrainingProgram">Training Program </param>
        /// <param name="Status"> Training Staus </param>
        /// <returns> DataTable with Trainings</returns>
        public DataTable PopulateTrainings(string TrainingType, string TrainingCode, string TrainingProgram, string Status)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCon.Open();

                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                TRAINING_ID,
                                                TRAINING_NAME,
                                                TRAINING_CODE,
                                                CONVERT(PLANNED_START_DATE , CHAR) AS 'PLANNED_START_DATE',
                                                CONVERT(PLANNED_END_DATE , CHAR) AS 'PLANNED_END_DATE'
                                            FROM
                                                TRAINING                                                                                  
                                        ";

                mySqlCmd.CommandText = sMySqlString;

                string WhereString = String.Empty;

                if (TrainingType != String.Empty)
                {
                    if (WhereString != String.Empty)
                    {
                        WhereString += " AND ";
                    }
                    else
                    {
                        WhereString += " WHERE ";
                    }
                    WhereString += " TRAINING_TYPE = @TRAINING_TYPE ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_TYPE", TrainingType));
                }
                if (TrainingCode != String.Empty)
                {
                    if (WhereString != String.Empty)
                    {
                        WhereString += " AND ";
                    }
                    else
                    {
                        WhereString += " WHERE ";
                    }
                    WhereString += " TRAINING_CODE LIKE('%" + TrainingCode + "%') ";

                }
                if (TrainingProgram != String.Empty)
                {
                    if (WhereString != String.Empty)
                    {
                        WhereString += " AND ";
                    }
                    else
                    {
                        WhereString += " WHERE ";
                    }
                    WhereString += " TRAINING_PROGRAM_ID = @TRAINING_PROGRAM_ID ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_PROGRAM_ID", TrainingProgram));
                }
                if (Status != String.Empty)
                {
                    if (WhereString != String.Empty)
                    {
                        WhereString += " AND ";
                    }
                    else
                    {
                        WhereString += " WHERE ";
                    }
                    WhereString += " STATUS_CODE = @STATUS_CODE ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", Status));
                }

                mySqlCmd.CommandText += WhereString;
                mySqlCmd.CommandText += " ORDER BY TRAINING_NAME ASC ";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);



                mySqlDa.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                dataTable.Dispose();
                mySqlCmd.Dispose();
            }
        }

        /// <summary>
        /// Get Training Schedules from Database
        /// </summary>
        /// <param name="TrainingID">Training ID</param>
        /// <returns>DataTabe with Training Schedule Details</returns>
        public DataTable PopulateTrainingSchedule(string TrainingID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCon.Open();

                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                RECORD_ID,
                                                CONCAT(CONCAT(CONCAT(CONCAT(CONVERT( PLANNED_SCHEDULE_DATE , CHAR),
                                                                                ' | '),
                                                                        CONVERT( PLANNED_FROM_TIME , CHAR)),
                                                                ' - '),
                                                        CONVERT( PLANNED_TO_TIME , CHAR)) AS 'DATE_TIME'
                                            FROM
                                                TRAINING_SCHEDULE
                                            WHERE
                                                TRAINING_ID = @TRAINING_ID
                                            ORDER BY PLANNED_SCHEDULE_DATE ASC, PLANNED_FROM_TIME ASC;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);



                mySqlDa.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                dataTable.Dispose();
                mySqlCmd.Dispose();
            }
        }

        /// <summary>
        /// Get Training Attendace from Database
        /// </summary>
        /// <param name="TrainingID">Training ID</param>
        /// <returns>DataTable with Training Attendance Details</returns>
        public DataTable PopulateTrainingAttendance(string TrainingID,string TrainingScheduleID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCon.Open();

                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT DISTINCT
                                                TP.EMPLOYEE_ID,
                                                CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) AS 'EMP_NAME',
                                                CASE
                                                    WHEN
                                                        TA.ACTUAL_DATE IS NULL
                                                    THEN
                                                        (SELECT 
                                                                CONVERT(TS.PLANNED_SCHEDULE_DATE, CHAR) AS 'PLANNED_SCHEDULE_DATE'
                                                            FROM
                                                                TRAINING_SCHEDULE TS
                                                            WHERE
                                                                TS.TRAINING_ID = TP.TRAINING_ID)
                                                    ELSE CONVERT(TA.ACTUAL_DATE, CHAR) 
                                                END AS 'ACTUAL_DATE',
                                                CASE
                                                    WHEN
                                                        TA.ARRIVED_TIME IS NULL
                                                    THEN
                                                        (SELECT 
                                                                TS.PLANNED_FROM_TIME
                                                            FROM
                                                                TRAINING_SCHEDULE TS
                                                            WHERE
                                                                TS.TRAINING_ID = TP.TRAINING_ID)
                                                    ELSE TA.ARRIVED_TIME
                                                END AS 'ARRIVED_TIME',
                                                CASE
                                                    WHEN
                                                        TA.DEPARTURE_TIME IS NULL
                                                    THEN
                                                        (SELECT 
                                                                TS.PLANNED_TO_TIME
                                                            FROM
                                                                TRAINING_SCHEDULE TS
                                                            WHERE
                                                                TS.TRAINING_ID = TP.TRAINING_ID)
                                                    ELSE TA.DEPARTURE_TIME
                                                END AS 'DEPARTURE_TIME',
                                                TA.REMARKS,
                                                TA.STATUS_CODE,
                                                CASE
                                                    WHEN TA.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"' THEN '" + Constants.STATUS_ACTIVE_TAG + @"'
                                                    ELSE '" + Constants.STATUS_INACTIVE_TAG + @"'
                                                END AS 'STATUS_CODE1',
                                                TA.IS_ATTEND
                                            FROM
                                                TRAINING_PARTICIPANTS TP
                                                    LEFT OUTER JOIN
                                                TRAINING_ATTENDANCE TA ON TP.EMPLOYEE_ID = TA.EMPLOYEE_ID
                                                    INNER JOIN
                                                EMPLOYEE E ON TP.EMPLOYEE_ID = E.EMPLOYEE_ID
                                            WHERE
                                                TP.TRAINING_ID = @TRAINING_ID AND (TA.TRAINING_SCHEDULE_ID = @TRAINING_SCHEDULE_ID OR TA.TRAINING_SCHEDULE_ID IS NULL);                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_SCHEDULE_ID", TrainingScheduleID));


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count == 0)
                {
                    sMySqlString = @"
                                            SELECT 
                                                TP.EMPLOYEE_ID,
                                                CONCAT(CONCAT(E.TITLE, ' '), E.INITIALS_NAME) AS 'EMP_NAME',
                                                CONVERT( TS.ACTUAL_DATE , CHAR) AS 'ACTUAL_DATE',
                                                CONVERT( TS.PLANNED_FROM_TIME , CHAR) AS 'ARRIVED_TIME',
                                                CONVERT( TS.PLANNED_TO_TIME , CHAR) AS 'DEPARTURE_TIME',
                                                '' AS 'REMARKS',
                                                TP.STATUS_CODE,
                                                CASE
                                                    WHEN TP.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"' THEN '" + Constants.STATUS_ACTIVE_TAG + @"'
                                                    ELSE '" + Constants.STATUS_INACTIVE_TAG + @"'
                                                END AS 'STATUS_CODE1',
                                                '' AS 'IS_ATTEND'
                                            FROM
                                                TRAINING_PARTICIPANTS TP
                                                    LEFT OUTER JOIN
                                                TRAINING_SCHEDULE TS ON TP.TRAINING_ID = TS.TRAINING_ID
                                                    INNER JOIN
                                                EMPLOYEE E ON TP.EMPLOYEE_ID = E.EMPLOYEE_ID
                                            WHERE
                                                TS.RECORD_ID = @TRAINING_SCHEDULE_ID
                                                    AND TP.TRAINING_ID = @TRAINING_ID
                                    ";

                    mySqlCmd.CommandText = sMySqlString;
                    mySqlCmd.Parameters.Clear();
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_SCHEDULE_ID", TrainingScheduleID));


                    mySqlDa = new MySqlDataAdapter(mySqlCmd);
                    mySqlDa.Fill(dataTable);
                }


                mySqlDa.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                dataTable.Dispose();
                mySqlCmd.Dispose();
            }
        }

        /// <summary>
        /// This method is used to insert data to database
        /// </summary>
        /// <param name="TrainingAttendance"></param>
        /// <param name="ModifiedBy"></param>
        /// <returns></returns>
        public Boolean Insert(DataTable TrainingAttendance, String ModifiedBy)
        {
            Boolean blInserted = false;

            string sMySqlString = "";
            string sCategoryID = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                int i = 0;
                while (i < TrainingAttendance.Rows.Count)
                {
                    string TRAINING_ID = TrainingAttendance.Rows[i]["TRAINING_ID"].ToString();
                    string TRAINING_SCHEDULE_ID = TrainingAttendance.Rows[i]["TRAINING_SCHEDULE_ID"].ToString();
                    string EMPLOYEE_ID = TrainingAttendance.Rows[i]["EMPLOYEE_ID"].ToString();
                    string ACTUAL_DATE = TrainingAttendance.Rows[i]["ACTUAL_DATE"].ToString();
                    string ARRIVED_TIME = TrainingAttendance.Rows[i]["ARRIVED_TIME"].ToString();
                    string DEPARTURE_TIME = TrainingAttendance.Rows[i]["DEPARTURE_TIME"].ToString();
                    string REMARKS = TrainingAttendance.Rows[i]["REMARKS"].ToString();
                    string STATUS_CODE = TrainingAttendance.Rows[i]["STATUS_CODE"].ToString();
                    string IS_ATTEND = TrainingAttendance.Rows[i]["IS_ATTEND"].ToString();


                    mySqlCmd.Parameters.Clear();
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TRAINING_ID.Trim() == "" ? (object)DBNull.Value : TRAINING_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_SCHEDULE_ID", TRAINING_SCHEDULE_ID.Trim() == "" ? (object)DBNull.Value : TRAINING_SCHEDULE_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@EMPLOYEE_ID", EMPLOYEE_ID.Trim() == "" ? (object)DBNull.Value : EMPLOYEE_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_DATE", ACTUAL_DATE.Trim() == "" ? (object)DBNull.Value : ACTUAL_DATE.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ARRIVED_TIME", ARRIVED_TIME.Trim() == "" ? (object)DBNull.Value : ARRIVED_TIME.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@DEPARTURE_TIME", DEPARTURE_TIME.Trim() == "" ? (object)DBNull.Value : DEPARTURE_TIME.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@REMARKS", REMARKS.Trim() == "" ? (object)DBNull.Value : REMARKS.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", STATUS_CODE.Trim() == "" ? (object)DBNull.Value : STATUS_CODE.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_ATTEND", IS_ATTEND.Trim() == "" ? (object)DBNull.Value : IS_ATTEND.Trim()));

                    

                    //SerialHandler serialHandler = new SerialHandler();
                    //sCategoryID = serialHandler.getserilalReference(ref mySqlCon, "TC");

                    sMySqlString = @" 
                                    REPLACE INTO 
                                        TRAINING_ATTENDANCE
                                        (
                                            TRAINING_ID, 
                                            TRAINING_SCHEDULE_ID, 
                                            EMPLOYEE_ID, 
                                            ACTUAL_DATE, 
                                            ARRIVED_TIME, 
                                            DEPARTURE_TIME, 
                                            REMARKS,
                                            IS_ATTEND, 
                                            STATUS_CODE, 
                                            ADDED_BY, 
                                            ADDED_DATE, 
                                            MODIFIED_BY, 
                                            MODIFIED_DATE
                                        ) 
                                    VALUES
                                    (
                                        @TRAINING_ID, 
                                        @TRAINING_SCHEDULE_ID, 
                                        @EMPLOYEE_ID, 
                                        @ACTUAL_DATE, 
                                        @ARRIVED_TIME, 
                                        @DEPARTURE_TIME, 
                                        @REMARKS,
                                        @IS_ATTEND, 
                                        @STATUS_CODE, 
                                        @MODIFIED_BY, 
                                        NOW(), 
                                        @MODIFIED_BY, 
                                        NOW()
                                    )
                                ";


                    mySqlCmd.Transaction = mySqlTrans;
                    mySqlCmd.CommandText = sMySqlString;

                    mySqlCmd.ExecuteNonQuery();
                    i++;
                }

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                //serialHandler = null;

                

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
    }
}
