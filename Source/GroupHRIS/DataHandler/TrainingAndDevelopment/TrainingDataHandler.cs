using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainingDataHandler : TemplateDataHandler
    {
        public DataTable Populate()
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"
                                            SELECT 
                                                T.TRAINING_ID,
                                                T.TRAINING_NAME, 
                                                T.TRAINING_CODE, 
                                                T.TRAINING_PROGRAM_ID, 
                                                TP.PROGRAM_NAME, 
                                                T.TRAINING_TYPE, 
                                                TT.TYPE_NAME, 
                                                CONVERT(DATE_FORMAT(T.PLANNED_START_DATE, '%d/%m/%Y') ,CHAR) AS 'PLANNED_START_DATE',  
                                                CONVERT(DATE_FORMAT(T.PLANNED_END_DATE, '%d/%m/%Y') ,CHAR) AS 'PLANNED_END_DATE', 
                                                T.PLANNED_TOTAL_HOURS, 
                                                T.PLANNED_PARTICIPANTS, 
                                                CONVERT(DATE_FORMAT(T.ACTUAL_START_DATE, '%d/%m/%Y') ,CHAR) AS 'ACTUAL_START_DATE', 
                                                CONVERT(DATE_FORMAT(T.ACTUAL_END_DATE, '%d/%m/%Y') ,CHAR) AS 'ACTUAL_END_DATE', 
                                                T.ACTUAL_TOTAL_HOURS, 
                                                T.ACTUAL_PARTICIPANTS, 
                                                T.IS_OUT_OF_BUDGET, 
                                                T.IS_POSTPONED, 
                                                T.POSTPONED_REASON, 
                                                T.STATUS_CODE 
                                            FROM  
                                                TRAINING T,
                                                TRAINING_PROGRAM TP,
                                                TRAINING_TYPE TT
                                            WHERE
                                                T.TRAINING_PROGRAM_ID = TP.PROGRAM_ID AND
                                                T.TRAINING_TYPE = TT.TRAINING_TYPE_ID
                                            ORDER BY
                                                T.ADDED_DATE DESC
                                            LIMIT 10;                                              
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
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

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Populate(string TrainingID)
        {
            try
            {
                dataTable = new DataTable();

                mySqlCon.Open();
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @"
                                            SELECT 
                                                T.TRAINING_ID,
                                                T.TRAINING_NAME, 
                                                T.TRAINING_CODE, 
                                                T.TRAINING_PROGRAM_ID, 
                                                TP.PROGRAM_NAME, 
                                                T.TRAINING_TYPE, 
                                                TT.TYPE_NAME, 
                                                CONVERT(DATE_FORMAT(T.PLANNED_START_DATE, '%d/%m/%Y') ,CHAR) AS 'PLANNED_START_DATE',  
                                                CONVERT(DATE_FORMAT(T.PLANNED_END_DATE, '%d/%m/%Y') ,CHAR) AS 'PLANNED_END_DATE', 
                                                T.PLANNED_TOTAL_HOURS, 
                                                T.PLANNED_PARTICIPANTS, 
                                                CONVERT(DATE_FORMAT(T.ACTUAL_START_DATE, '%d/%m/%Y') ,CHAR) AS 'ACTUAL_START_DATE', 
                                                CONVERT(DATE_FORMAT(T.ACTUAL_END_DATE, '%d/%m/%Y') ,CHAR) AS 'ACTUAL_END_DATE', 
                                                T.ACTUAL_TOTAL_HOURS, 
                                                T.ACTUAL_PARTICIPANTS, 
                                                T.IS_OUT_OF_BUDGET, 
                                                T.IS_POSTPONED, 
                                                T.POSTPONED_REASON, 
                                                T.STATUS_CODE 
                                            FROM  
                                                TRAINING T,
                                                TRAINING_PROGRAM TP,
                                                TRAINING_TYPE TT
                                            WHERE
                                                T.TRAINING_PROGRAM_ID = TP.PROGRAM_ID AND
                                                T.TRAINING_TYPE = TT.TRAINING_TYPE_ID AND 
                                                T.TRAINING_ID = @TRAINING_ID;                                              
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Clear();
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
            }
        }

        public DataTable PopulateTrainingProgramTypes()
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"
                                            SELECT 
                                                PROGRAM_ID, PROGRAM_NAME
                                            FROM
                                                TRAINING_PROGRAM
                                            WHERE
                                                STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"';                                              
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable PopulateTrainingTypes()
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"
                                            SELECT 
                                                TRAINING_TYPE_ID, TYPE_NAME
                                            FROM
                                                TRAINING_TYPE
                                            WHERE
                                                STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"';                                              
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable PopulateCompanies()
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"
                                            SELECT 
                                                COMPANY_ID, COMP_NAME
                                            FROM
                                                COMPANY
                                            WHERE
                                                STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"';                                              
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Insert(string TrainingName, string TrainingCode, string TrainingProgramID, string TrainingType, string PlannedStartDate, string PlannedEndDate, string PlannedTotalHours, string PlannedParticipants, string ActualStartDate, string ActualEndDate, string ActualTotalHours, string ActualParticipants, string IsOutOfBudget, string IsPostponed, string PostponedReason, string StatusCode, string AddedBy, DataTable TrainingCompany, DataTable TrainingTrainers)
        {
            Boolean Status = false;
            string TrainingID = String.Empty;
            SerialHandler serialHandler = new SerialHandler();
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
                                            TRAINING
                                                (
                                                    TRAINING_ID, 
                                                    TRAINING_NAME, 
                                                    TRAINING_CODE, 
                                                    TRAINING_PROGRAM_ID, 
                                                    TRAINING_TYPE, 
                                                    PLANNED_START_DATE, 
                                                    PLANNED_END_DATE, 
                                                    PLANNED_TOTAL_HOURS, 
                                                    PLANNED_PARTICIPANTS, 
                                                    ACTUAL_START_DATE, 
                                                    ACTUAL_END_DATE, 
                                                    ACTUAL_TOTAL_HOURS, 
                                                    ACTUAL_PARTICIPANTS, 
                                                    IS_OUT_OF_BUDGET, 
                                                    IS_POSTPONED, 
                                                    POSTPONED_REASON, 
                                                    STATUS_CODE, 
                                                    ADDED_BY, 
                                                    ADDED_DATE, 
                                                    MODIFIED_BY, 
                                                    MODIFIED_DATE
                                                ) 
                                            VALUES
                                                (
                                                    @TRAINING_ID, 
                                                    @TRAINING_NAME, 
                                                    @TRAINING_CODE, 
                                                    @TRAINING_PROGRAM_ID, 
                                                    @TRAINING_TYPE, 
                                                    @PLANNED_START_DATE, 
                                                    @PLANNED_END_DATE, 
                                                    @PLANNED_TOTAL_HOURS, 
                                                    @PLANNED_PARTICIPANTS, 
                                                    @ACTUAL_START_DATE, 
                                                    @ACTUAL_END_DATE, 
                                                    @ACTUAL_TOTAL_HOURS, 
                                                    @ACTUAL_PARTICIPANTS, 
                                                    @IS_OUT_OF_BUDGET, 
                                                    @IS_POSTPONED, 
                                                    @POSTPONED_REASON, 
                                                    @STATUS_CODE, 
                                                    @ADDED_BY, 
                                                    NOW(), 
                                                    @ADDED_BY, 
                                                    NOW()
                                                );
                                    ";

                    TrainingID = serialHandler.getserila(mySqlCon, Constants.TRAINING_ID_STAMP);

                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID.Trim() == "" ? (object)DBNull.Value : TrainingID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_NAME", TrainingName.Trim() == "" ? (object)DBNull.Value : TrainingName.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_CODE", TrainingCode.Trim() == "" ? (object)DBNull.Value : TrainingCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_PROGRAM_ID", TrainingProgramID.Trim() == "" ? (object)DBNull.Value : TrainingProgramID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_TYPE", TrainingType.Trim() == "" ? (object)DBNull.Value : TrainingType.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_START_DATE", PlannedStartDate.Trim() == "" ? (object)DBNull.Value : PlannedStartDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_END_DATE", PlannedEndDate.Trim() == "" ? (object)DBNull.Value : PlannedEndDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_TOTAL_HOURS", PlannedTotalHours.Trim() == "" ? (object)DBNull.Value : PlannedTotalHours.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_PARTICIPANTS", PlannedParticipants.Trim() == "" ? (object)DBNull.Value : PlannedParticipants.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_START_DATE", ActualStartDate.Trim() == "" ? (object)DBNull.Value : ActualStartDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_END_DATE", ActualEndDate.Trim() == "" ? (object)DBNull.Value : ActualEndDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_TOTAL_HOURS", ActualTotalHours.Trim() == "" ? (object)DBNull.Value : ActualTotalHours.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_PARTICIPANTS", ActualParticipants.Trim() == "" ? (object)DBNull.Value : ActualParticipants.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_OUT_OF_BUDGET", IsOutOfBudget.Trim() == "" ? (object)DBNull.Value : IsOutOfBudget.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_POSTPONED", IsPostponed.Trim() == "" ? (object)DBNull.Value : IsPostponed.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@POSTPONED_REASON", PostponedReason.Trim() == "" ? (object)DBNull.Value : PostponedReason.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();
                    mySqlCmd.Parameters.Clear();

                    for (int i = 0; i < TrainingCompany.Rows.Count; i++)
                    {

                        string CompnayID = TrainingCompany.Rows[i]["COMPANY_ID"].ToString();
                        PlannedParticipants = TrainingCompany.Rows[i]["PLANNED_PARTICIPANTS"].ToString();
                        ActualParticipants = TrainingCompany.Rows[i]["ACTUAL_PARTICIPANTS"].ToString();
                        string Description = TrainingCompany.Rows[i]["DESCRIPTION"].ToString();
                        StatusCode = TrainingCompany.Rows[i]["STATUS_CODE"].ToString();

                        Qry = @"
                                INSERT INTO 
	                                TRAINING_COMPANY
	                                (
		                                TRAINING_ID, 
		                                COMPANY_ID, 
		                                PLANNED_PARTICIPANTS, 
		                                ACTUAL_PARTICIPANTS, 
		                                DESCRIPTION, 
		                                STATUS_CODE, 
		                                ADDED_BY, 
		                                ADDED_DATE, 
		                                MODIFIED_BY, 
		                                MODIFIED_DATE
	                                )
                                VALUES 
                                (
	                                @TRAINING_ID, 
	                                @COMPANY_ID, 
	                                @PLANNED_PARTICIPANTS, 
	                                @ACTUAL_PARTICIPANTS, 
	                                @DESCRIPTION, 
	                                @STATUS_CODE, 
	                                @ADDED_BY, 
	                                NOW(), 
	                                @ADDED_BY, 
	                                NOW()
                                )
                          ";

                        mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID.Trim() == "" ? (object)DBNull.Value : TrainingID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", CompnayID.Trim() == "" ? (object)DBNull.Value : CompnayID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_PARTICIPANTS", PlannedParticipants.Trim() == "" ? (object)DBNull.Value : PlannedParticipants.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_PARTICIPANTS", ActualParticipants.Trim() == "" ? (object)DBNull.Value : ActualParticipants.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", Description.Trim() == "" ? (object)DBNull.Value : Description.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));

                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();
                        mySqlCmd.Parameters.Clear();
                    }

                    for (int i = 0; i < TrainingTrainers.Rows.Count; i++)
                    {

                        string TrainerID = TrainingTrainers.Rows[i]["TRAINER_ID"].ToString();
                        string TrainserName = TrainingTrainers.Rows[i]["NAME_WITH_INITIALS"].ToString();
                        string SelectedReason = TrainingTrainers.Rows[i]["SELECTED_REASON"].ToString();
                        StatusCode = TrainingTrainers.Rows[i]["STATUS_CODE"].ToString();

                        Qry = @"
                                INSERT INTO 
                                    TRAINING_TRAINERS
                                    (
                                        TRAINING_ID, 
                                        TRAINER_ID, 
                                        SELECTED_REASON, 
                                        STATUS_CODE, 
                                        ADDED_BY, 
                                        ADDED_DATE, 
                                        MODIFIED_BY, 
                                        MODIFIED_DATE
                                    )
                                VALUES
                                (
                                    @TRAINING_ID, 
                                    @TRAINER_ID, 
                                    @SELECTED_REASON, 
                                    @STATUS_CODE, 
                                    @ADDED_BY, 
                                    NOW(), 
                                    @ADDED_BY, 
                                    NOW()
                                )
                          ";

                        mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID.Trim() == "" ? (object)DBNull.Value : TrainingID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINER_ID", TrainerID.Trim() == "" ? (object)DBNull.Value : TrainerID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@SELECTED_REASON", SelectedReason.Trim() == "" ? (object)DBNull.Value : SelectedReason.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", AddedBy.Trim() == "" ? (object)DBNull.Value : AddedBy.Trim()));

                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();
                        mySqlCmd.Parameters.Clear();
                    }

                    oMySqlTransaction.Commit();
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
                serialHandler = null;
                TrainingID = String.Empty;
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
            return Status;
        }
        
        public Boolean Update(string TrainingName, string TrainingCode, string TrainingProgramID, string TrainingType, string PlannedStartDate, string PlannedEndDate, string PlannedTotalHours, string PlannedParticipants, string ActualStartDate, string ActualEndDate, string ActualTotalHours, string ActualParticipants, string IsOutOfBudget, string IsPostponed, string PostponedReason, string StatusCode, string ModifiedBy, string TrainingID, DataTable TrainingCompany, DataTable TrainingTrainers)
        {
            Boolean Status = false;
            //string TrainingID = String.Empty;
            //SerialHandler serialHandler = new SerialHandler();
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
                                            TRAINING 
                                        SET  
                                            TRAINING_NAME = @TRAINING_NAME, 
                                            TRAINING_CODE = @TRAINING_CODE, 
                                            TRAINING_PROGRAM_ID = @TRAINING_PROGRAM_ID, 
                                            TRAINING_TYPE = @TRAINING_TYPE, 
                                            PLANNED_START_DATE = @PLANNED_START_DATE, 
                                            PLANNED_END_DATE = @PLANNED_END_DATE, 
                                            PLANNED_TOTAL_HOURS = @PLANNED_TOTAL_HOURS, 
                                            PLANNED_PARTICIPANTS = @PLANNED_PARTICIPANTS, 
                                            ACTUAL_START_DATE = @ACTUAL_START_DATE, 
                                            ACTUAL_END_DATE = @ACTUAL_END_DATE, 
                                            ACTUAL_TOTAL_HOURS = @ACTUAL_TOTAL_HOURS, 
                                            ACTUAL_PARTICIPANTS = @ACTUAL_PARTICIPANTS, 
                                            IS_OUT_OF_BUDGET = @IS_OUT_OF_BUDGET, 
                                            IS_POSTPONED = @IS_POSTPONED, 
                                            POSTPONED_REASON = @POSTPONED_REASON, 
                                            STATUS_CODE = @STATUS_CODE, 
                                            MODIFIED_BY = @MODIFIED_BY, 
                                            MODIFIED_DATE = NOW()
                                        WHERE 
                                            TRAINING_ID = @TRAINING_ID;
                                    ";

                    //TrainingID = serialHandler.getserila(mySqlCon, Constants.TRAINING_ID_STAMP);

                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID.Trim() == "" ? (object)DBNull.Value : TrainingID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_NAME", TrainingName.Trim() == "" ? (object)DBNull.Value : TrainingName.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_CODE", TrainingCode.Trim() == "" ? (object)DBNull.Value : TrainingCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_PROGRAM_ID", TrainingProgramID.Trim() == "" ? (object)DBNull.Value : TrainingProgramID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_TYPE", TrainingType.Trim() == "" ? (object)DBNull.Value : TrainingType.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_START_DATE", PlannedStartDate.Trim() == "" ? (object)DBNull.Value : PlannedStartDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_END_DATE", PlannedEndDate.Trim() == "" ? (object)DBNull.Value : PlannedEndDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_TOTAL_HOURS", PlannedTotalHours.Trim() == "" ? (object)DBNull.Value : PlannedTotalHours.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_PARTICIPANTS", PlannedParticipants.Trim() == "" ? (object)DBNull.Value : PlannedParticipants.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_START_DATE", ActualStartDate.Trim() == "" ? (object)DBNull.Value : ActualStartDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_END_DATE", ActualEndDate.Trim() == "" ? (object)DBNull.Value : ActualEndDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_TOTAL_HOURS", ActualTotalHours.Trim() == "" ? (object)DBNull.Value : ActualTotalHours.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_PARTICIPANTS", ActualParticipants.Trim() == "" ? (object)DBNull.Value : ActualParticipants.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_OUT_OF_BUDGET", IsOutOfBudget.Trim() == "" ? (object)DBNull.Value : IsOutOfBudget.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@IS_POSTPONED", IsPostponed.Trim() == "" ? (object)DBNull.Value : IsPostponed.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@POSTPONED_REASON", PostponedReason.Trim() == "" ? (object)DBNull.Value : PostponedReason.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@MODIFIED_BY", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();
                    mySqlCmd.Parameters.Clear();


                    Qry = @" DELETE FROM TRAINING_COMPANY WHERE TRAINING_ID = @TRAINING_ID ";
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID.Trim() == "" ? (object)DBNull.Value : TrainingID.Trim()));
                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();
                    mySqlCmd.Parameters.Clear();

                    for (int i = 0; i < TrainingCompany.Rows.Count; i++)
                    {

                        string CompnayID = TrainingCompany.Rows[i]["COMPANY_ID"].ToString();
                        PlannedParticipants = TrainingCompany.Rows[i]["PLANNED_PARTICIPANTS"].ToString();
                        ActualParticipants = TrainingCompany.Rows[i]["ACTUAL_PARTICIPANTS"].ToString();
                        string Description = TrainingCompany.Rows[i]["DESCRIPTION"].ToString();
                        StatusCode = TrainingCompany.Rows[i]["STATUS_CODE"].ToString();

                        Qry = @"
                                INSERT INTO 
	                                TRAINING_COMPANY
	                                (
		                                TRAINING_ID, 
		                                COMPANY_ID, 
		                                PLANNED_PARTICIPANTS, 
		                                ACTUAL_PARTICIPANTS, 
		                                DESCRIPTION, 
		                                STATUS_CODE, 
		                                ADDED_BY, 
		                                ADDED_DATE, 
		                                MODIFIED_BY, 
		                                MODIFIED_DATE
	                                )
                                VALUES 
                                (
	                                @TRAINING_ID, 
	                                @COMPANY_ID, 
	                                @PLANNED_PARTICIPANTS, 
	                                @ACTUAL_PARTICIPANTS, 
	                                @DESCRIPTION, 
	                                @STATUS_CODE, 
	                                @ADDED_BY, 
	                                NOW(), 
	                                @ADDED_BY, 
	                                NOW()
                                )
                          ";

                        mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID.Trim() == "" ? (object)DBNull.Value : TrainingID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", CompnayID.Trim() == "" ? (object)DBNull.Value : CompnayID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@PLANNED_PARTICIPANTS", PlannedParticipants.Trim() == "" ? (object)DBNull.Value : PlannedParticipants.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ACTUAL_PARTICIPANTS", ActualParticipants.Trim() == "" ? (object)DBNull.Value : ActualParticipants.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@DESCRIPTION", Description.Trim() == "" ? (object)DBNull.Value : Description.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));

                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();
                        mySqlCmd.Parameters.Clear();
                    }


                    Qry = @" DELETE FROM TRAINING_TRAINERS WHERE TRAINING_ID = @TRAINING_ID ";
                    mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID.Trim() == "" ? (object)DBNull.Value : TrainingID.Trim()));
                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();
                    mySqlCmd.Parameters.Clear();

                    for (int i = 0; i < TrainingTrainers.Rows.Count; i++)
                    {

                        string TrainerID = TrainingTrainers.Rows[i]["TRAINER_ID"].ToString();
                        string TrainserName = TrainingTrainers.Rows[i]["NAME_WITH_INITIALS"].ToString();
                        string SelectedReason = TrainingTrainers.Rows[i]["SELECTED_REASON"].ToString();
                        StatusCode = TrainingTrainers.Rows[i]["STATUS_CODE"].ToString();

                        Qry = @"
                                INSERT INTO 
                                    TRAINING_TRAINERS
                                    (
                                        TRAINING_ID, 
                                        TRAINER_ID, 
                                        SELECTED_REASON, 
                                        STATUS_CODE, 
                                        ADDED_BY, 
                                        ADDED_DATE, 
                                        MODIFIED_BY, 
                                        MODIFIED_DATE
                                    )
                                VALUES
                                (
                                    @TRAINING_ID, 
                                    @TRAINER_ID, 
                                    @SELECTED_REASON, 
                                    @STATUS_CODE, 
                                    @ADDED_BY, 
                                    NOW(), 
                                    @ADDED_BY, 
                                    NOW()
                                )
                          ";

                        mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINING_ID", TrainingID.Trim() == "" ? (object)DBNull.Value : TrainingID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@TRAINER_ID", TrainerID.Trim() == "" ? (object)DBNull.Value : TrainerID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@SELECTED_REASON", SelectedReason.Trim() == "" ? (object)DBNull.Value : SelectedReason.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS_CODE", StatusCode.Trim() == "" ? (object)DBNull.Value : StatusCode.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", ModifiedBy.Trim() == "" ? (object)DBNull.Value : ModifiedBy.Trim()));

                        mySqlCmd.CommandText = Qry;
                        mySqlCmd.ExecuteNonQuery();
                        mySqlCmd.Parameters.Clear();
                    }

                    oMySqlTransaction.Commit();

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
                //serialHandler = null;
                TrainingID = String.Empty;
                mySqlCmd.Parameters.Clear();
                mySqlCon.Close();
            }
            return Status;
        }

        public Boolean CheckTrainingNameExsistance(string TrainingName)
        {
            Boolean isExsists = false;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();


                string queryStr = "SELECT * FROM TRAINING WHERE TRAINING_NAME ='" + TrainingName + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExsists = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return isExsists;
        }

        public Boolean CheckTrainingNameExsistance(string TrainingName, string id)
        {

            dataTable = new DataTable();
            Boolean isExsists = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string queryStr = "SELECT * FROM TRAINING WHERE TRAINING_NAME ='" + TrainingName + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["TRAINING_ID"].ToString() == id)
                        {
                            isExsists = false;
                        }
                        else
                        {
                            isExsists = true;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }

            return isExsists;
        }

        /// <summary>
        /// Used to Populate Training Companies
        /// </summary>
        /// <param name="TrainingID">Training ID for get Training Companies</param>
        /// <returns>This Method Returns a Datatable with Training Compaies According to the Training ID </returns>
        public DataTable PopulateTrainingCompanies(string TrainingID)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"
                                            SELECT 
                                                TC.TRAINING_ID,
                                                TC.COMPANY_ID,
                                                C.COMP_NAME,
                                                CONVERT(TC.PLANNED_PARTICIPANTS, CHAR) AS 'PLANNED_PARTICIPANTS',
                                                CONVERT(TC.ACTUAL_PARTICIPANTS, CHAR) AS 'ACTUAL_PARTICIPANTS', 
                                                TC.DESCRIPTION,
                                                TC.STATUS_CODE
                                            FROM
                                                TRAINING_COMPANY TC,
                                                COMPANY C
                                            WHERE
                                                TC.COMPANY_ID = C.COMPANY_ID
                                                    AND TC.TRAINING_ID = '" + TrainingID + @"';                                              
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable PopulateTrainingTrainers(string TrainingID)
        {
            try
            {
                dataTable = new DataTable();
                string sMySqlString = @"
                                            SELECT 
                                                TT.TRAINING_ID,
                                                TT.TRAINER_ID,
                                                TI.NAME_WITH_INITIALS,
                                                TT.SELECTED_REASON,
                                                TT.STATUS_CODE
                                            FROM
                                                TRAINING_TRAINERS TT,
                                                TRAINER_INFROMATION TI
                                            WHERE
                                                TT.TRAINER_ID = TI.TRAINER_ID
                                                    AND TT.TRAINING_ID = '" + TrainingID + @"';                                              
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
