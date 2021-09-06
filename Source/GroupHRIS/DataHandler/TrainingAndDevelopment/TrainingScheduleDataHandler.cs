using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainingScheduleDataHandler : TemplateDataHandler
    {
        public DataTable Populate(string TrainingID)
        {
            try
            {
                dataTable = new DataTable();
                
                string sMySqlString = @"
                                            SELECT 
                                                TS.RECORD_ID, 
                                                TS.TRAINING_ID, 
                                                T.TRAINING_NAME, 
                                                CONVERT(DATE_FORMAT(TS.PLANNED_SCHEDULE_DATE, '%d/%m/%Y') ,CHAR) AS 'PLANNED_SCHEDULE_DATE', 
                                                CONVERT(DATE_FORMAT(TS.ACTUAL_DATE, '%d/%m/%Y') ,CHAR) AS 'ACTUAL_DATE', 
                                                TS.PLANNED_FROM_TIME, 
                                                TS.PLANNED_TO_TIME, 
                                                TS.LOCATION_ID, 
                                                TL.LOCATION_NAME, 
                                                TS.ACTUAL_FROM_TIME, 
                                                TS.ACTUAL_TO_TIME, 
                                                TS.STATUS_CODE, 
                                                TS.TRAINER_ID, 
                                                TI.NAME_WITH_INITIALS
                                            FROM 
                                                TRAINING_SCHEDULE TS,
                                                TRAINING T,
                                                TRAINING_LOCATIONS TL, 
                                                TRAINER_INFROMATION TI
                                            WHERE
                                                T.TRAINING_ID = TS.TRAINING_ID AND 
                                                TS.LOCATION_ID = TL.LOCATION_ID AND 
                                                TI.TRAINER_ID = TS.TRAINER_ID AND 
                                                T.TRAINING_ID = @TRAINING_ID;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                dataTable.Columns.Add("STATUS_CODE_TEXT");

                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if (dataTable.Rows[i]["STATUS_CODE"].ToString() == Constants.CON_ACTIVE_STATUS)
                        {
                            dataTable.Rows[i]["STATUS_CODE_TEXT"] = Constants.STATUS_ACTIVE_TAG;
                        }
                        else
                        {
                            dataTable.Rows[i]["STATUS_CODE_TEXT"] = Constants.STATUS_INACTIVE_TAG;
                        }
                    }
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
                mySqlCmd.Dispose();
                dataTable.Dispose();
            }
        }

        public DataTable PopulateTrainingLocations()
        {
            try
            {
                dataTable = new DataTable();
                mySqlCon.Open();

                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                LOCATION_ID, 
                                                LOCATION_NAME 
                                            FROM 
                                                TRAINING_LOCATIONS
                                            WHERE 
                                                STATUS_CODE = @STATUS_CODE
                                            ORDER BY
                                                LOCATION_NAME ASC;                                              
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

        public DataTable PopulateTrainers(string TrainingID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCon.Open();

                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                TT.TRAINER_ID, TI.NAME_WITH_INITIALS
                                            FROM
                                                TRAINING_TRAINERS TT,
                                                TRAINER_INFROMATION TI
                                            WHERE
                                                TT.TRAINER_ID = TI.TRAINER_ID
                                                    AND TT.TRAINING_ID = @TRAINING_ID                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Clear();

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

        public DataTable PopulateDuplicateRecords(string TrainingID, string PlannedScheduledDate, string PlannedFromTime, string PlannedToTime, string LocationID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCon.Open();

                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                *
                                            FROM
                                                TRAINING_SCHEDULE
                                            WHERE";
                                                    /*TRAINING_ID = @TRAINING_ID 
                                                    AND*/
                                sMySqlString += @" PLANNED_SCHEDULE_DATE = @PLANNED_SCHEDULE_DATE 
                                                    AND (((PLANNED_FROM_TIME > @plannedFromTime)
                                                    AND (PLANNED_TO_TIME <= @PlannedToTime))
                                                    OR ((PLANNED_FROM_TIME < @plannedFromTime)
                                                    AND (PLANNED_TO_TIME >= @PlannedToTime))
                                                    OR ((PLANNED_FROM_TIME > @plannedFromTime)
                                                    AND (PLANNED_FROM_TIME <= @PlannedToTime))
                                                    OR ((PLANNED_TO_TIME > @plannedFromTime)
                                                    AND (PLANNED_TO_TIME <= @PlannedToTime)))
                                                    AND LOCATION_ID = @LOCATION_ID 
                                                    AND STATUS_CODE = @STATUS_CODE;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                //mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_SCHEDULE_DATE", PlannedScheduledDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("@plannedFromTime", PlannedFromTime));
                mySqlCmd.Parameters.Add(new MySqlParameter("@PlannedToTime", PlannedToTime));
                mySqlCmd.Parameters.Add(new MySqlParameter("@LOCATION_ID", LocationID));
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
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
                dataTable.Dispose();
                mySqlCmd.Dispose();
            }
        }

        public DataTable PopulateDuplicateRecords(string TrainingID, string PlannedScheduledDate, string PlannedFromTime, string PlannedToTime, string LocationID, string RecordID)
        {
            try
            {
                dataTable = new DataTable();
                mySqlCon.Open();

                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                *
                                            FROM
                                                TRAINING_SCHEDULE
                                            WHERE";
                                                    /*TRAINING_ID = @TRAINING_ID 
                                                        AND*/
                                sMySqlString += @" PLANNED_SCHEDULE_DATE = @PLANNED_SCHEDULE_DATE 
                                                    AND (((PLANNED_FROM_TIME > @plannedFromTime)
                                                    AND (PLANNED_TO_TIME <= @PlannedToTime))
                                                    OR ((PLANNED_FROM_TIME < @plannedFromTime)
                                                    AND (PLANNED_TO_TIME >= @PlannedToTime))
                                                    OR ((PLANNED_FROM_TIME > @plannedFromTime)
                                                    AND (PLANNED_FROM_TIME <= @PlannedToTime))
                                                    OR ((PLANNED_TO_TIME > @plannedFromTime)
                                                    AND (PLANNED_TO_TIME <= @PlannedToTime)))
                                                    AND LOCATION_ID = @LOCATION_ID 
                                                    AND RECORD_ID <> @RECORD_ID 
                                                    AND STATUS_CODE = @STATUS_CODE;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                //mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_SCHEDULE_DATE", PlannedScheduledDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("@plannedFromTime", PlannedFromTime));
                mySqlCmd.Parameters.Add(new MySqlParameter("@PlannedToTime", PlannedToTime));
                mySqlCmd.Parameters.Add(new MySqlParameter("@LOCATION_ID", LocationID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RECORD_ID", RecordID));
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
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
                dataTable.Dispose();
                mySqlCmd.Dispose();
            }
        }

        public Boolean Insert(string TrainingID, string PlannedScheduledDate, string ActualDate, string PlannedFromTime, string PlannedToTime, string LocationID, string ActualFromTime, string ActualToTime, string StatusCode, string AddedBy, string Trainer)
        {
            Boolean Status = false;
            try
            {
                mySqlCon.Open();

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    mySqlCmd.Parameters.Clear();

                    string Qry = @"
                                        INSERT INTO 
                                            TRAINING_SCHEDULE
                                                (
                                                    TRAINING_ID, 
                                                    PLANNED_SCHEDULE_DATE, 
                                                    ACTUAL_DATE, 
                                                    PLANNED_FROM_TIME, 
                                                    PLANNED_TO_TIME, 
                                                    LOCATION_ID, 
                                                    ACTUAL_FROM_TIME, 
                                                    ACTUAL_TO_TIME, 
                                                    TRAINER_ID,
                                                    STATUS_CODE, 
                                                    ADDED_BY, 
                                                    ADDED_DATE, 
                                                    MODIFIED_BY, 
                                                    MODIFIED_DATE
                                                ) 
                                                VALUES
                                                (
                                                    @TRAINING_ID, 
                                                    @PLANNED_SCHEDULE_DATE, 
                                                    @ACTUAL_DATE, 
                                                    @PLANNED_FROM_TIME, 
                                                    @PLANNED_TO_TIME, 
                                                    @LOCATION_ID, 
                                                    @ACTUAL_FROM_TIME, 
                                                    @ACTUAL_TO_TIME,  
                                                    @TRAINER_ID,
                                                    @STATUS_CODE, 
                                                    @ADDED_BY, 
                                                    NOW(), 
                                                    @ADDED_BY, 
                                                    NOW()
                                                );
                                    ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID.Trim() == "" ? (object)DBNull.Value : TrainingID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_SCHEDULE_DATE", PlannedScheduledDate.Trim() == "" ? (object)DBNull.Value : PlannedScheduledDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_DATE", ActualDate.Trim() == "" ? (object)DBNull.Value : ActualDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_FROM_TIME", PlannedFromTime.Trim() == "" ? (object)DBNull.Value : PlannedFromTime.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_TO_TIME", PlannedToTime.Trim() == "" ? (object)DBNull.Value : PlannedToTime.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@LOCATION_ID", LocationID.Trim() == "" ? (object)DBNull.Value : LocationID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_FROM_TIME", ActualFromTime.Trim() == "" ? (object)DBNull.Value : ActualFromTime.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_TO_TIME", ActualToTime.Trim() == "" ? (object)DBNull.Value : ActualToTime.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINER_ID", Trainer.Trim() == "" ? (object)DBNull.Value : Trainer.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));
                    

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();
                    oMySqlTransaction.Commit();

                    mySqlCmd.Parameters.Clear();
                }
                catch (Exception exp)
                {
                    oMySqlTransaction.Rollback();
                    throw exp;
                }
                Status = true;
            }
            finally
            {
                TrainingID = String.Empty;
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
            return Status;
        }

        public Boolean Update(string TrainingID, string PlannedScheduledDate, string ActualDate, string PlannedFromTime, string PlannedToTime, string LocationID, string ActualFromTime, string ActualToTime, string StatusCode, string AddedBy, String RecordID, string Trainer)
        {
            Boolean Status = false;
            try
            {
                mySqlCon.Open();

                MySqlTransaction oMySqlTransaction;
                oMySqlTransaction = mySqlCon.BeginTransaction();
                try
                {
                    mySqlCmd.Parameters.Clear();

                    string Qry = @"
                                        UPDATE 
                                            TRAINING_SCHEDULE 
                                        SET  
                                            TRAINING_ID = @TRAINING_ID, 
                                            PLANNED_SCHEDULE_DATE = @PLANNED_SCHEDULE_DATE, 
                                            ACTUAL_DATE = @ACTUAL_DATE, 
                                            PLANNED_FROM_TIME = @PLANNED_FROM_TIME, 
                                            PLANNED_TO_TIME = @PLANNED_TO_TIME, 
                                            LOCATION_ID = @LOCATION_ID, 
                                            ACTUAL_FROM_TIME = @ACTUAL_FROM_TIME, 
                                            ACTUAL_TO_TIME = @ACTUAL_TO_TIME, 
                                            TRAINER_ID = @TRAINER_ID, 
                                            STATUS_CODE = @STATUS_CODE, 
                                            MODIFIED_BY = @MODIFIED_BY, 
                                            MODIFIED_DATE = NOW()
                                        WHERE 
                                            RECORD_ID = @RECORD_ID;
                                    ";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID.Trim() == "" ? (object)DBNull.Value : TrainingID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_SCHEDULE_DATE", PlannedScheduledDate.Trim() == "" ? (object)DBNull.Value : PlannedScheduledDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_DATE", ActualDate.Trim() == "" ? (object)DBNull.Value : ActualDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_FROM_TIME", PlannedFromTime.Trim() == "" ? (object)DBNull.Value : PlannedFromTime.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_TO_TIME", PlannedToTime.Trim() == "" ? (object)DBNull.Value : PlannedToTime.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@LOCATION_ID", LocationID.Trim() == "" ? (object)DBNull.Value : LocationID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_FROM_TIME", ActualFromTime.Trim() == "" ? (object)DBNull.Value : ActualFromTime.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_TO_TIME", ActualToTime.Trim() == "" ? (object)DBNull.Value : ActualToTime.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINER_ID", Trainer.Trim() == "" ? (object)DBNull.Value : Trainer.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@RECORD_ID", RecordID.Trim() == "" ? (object)DBNull.Value : RecordID.Trim()));


                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();
                    oMySqlTransaction.Commit();

                    mySqlCmd.Parameters.Clear();
                }
                catch (Exception exp)
                {
                    oMySqlTransaction.Rollback();
                    throw exp;
                }
                Status = true;
            }
            finally
            {
                TrainingID = String.Empty;
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
            return Status;
        }
    }
}
