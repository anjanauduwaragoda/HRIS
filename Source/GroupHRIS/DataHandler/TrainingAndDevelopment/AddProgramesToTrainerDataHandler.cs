using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class AddProgramesToTrainerDataHandler : TemplateDataHandler
    {
        public DataTable getAllPrograms()
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                string sqlQuery = "SELECT * FROM TRAINING_PROGRAM WHERE STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";
                MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlQuery, mySqlCon);
                sqlDa.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                mySqlCon.Close();
            }
        }

        public DataTable getAddedProgrames(string trainerId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                string sqlQuery = " SELECT TP.PROGRAM_ID, TP.PROGRAM_CODE, TP.PROGRAM_NAME,  " +
                                    " CASE " +
                                        " when TTP.STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then 'Inactive' " +
                                        " when TTP.STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then 'Active' " +
                                    " End  as STATUS_CODE " +
                                    " FROM TRAINER_TRAINING_PROGRAMS TTP " +
                                    " join TRAINING_PROGRAM TP " +
                                    " on TTP.TRAINING_PROGRAM_ID = TP.PROGRAM_ID " +
                                    " WHERE TTP.TRAINER_ID = '" + trainerId + "' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);
                return resultTable;
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
                resultTable.Dispose();
            }
        }

        public DataTable getTrainingTypes()
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = "SELECT TRAINING_TYPE_ID, TYPE_NAME FROM TRAINING_TYPE WHERE STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";
                MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlQuery, mySqlCon);
                sqlDa.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable getTrainingCategories()
        {
            DataTable resultTable = new DataTable();
            try
            {
                
                string sqlQuery = "SELECT TRAINING_CATEGORY_ID, CATEGORY_NAME FROM TRAINING_CATEGORY WHERE STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";
                MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlQuery, mySqlCon);
                sqlDa.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable filterProgrames(string selectedType, string selectedCategory)
        {
            DataTable resultTable = new DataTable();
            try
            {
                
                string sqlQuery = "SELECT * FROM TRAINING_PROGRAM WHERE STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";

                if (!String.IsNullOrEmpty(selectedType))
                {
                    sqlQuery += " and TRAINING_TYPE ='" + selectedType + "' ";
                }

                if (!String.IsNullOrEmpty(selectedCategory))
                {
                    sqlQuery += " and TRAINING_CATEGORY ='" + selectedCategory + "' ";
                }
                MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlQuery, mySqlCon);
                sqlDa.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable getProgramById(string programId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = @"SELECT PROGRAM_NAME,
                                        DESCRIPTION,
                                        OBJECTIVES,
                                        PROGRAM_DURATION,
                                        PROGRAM_TYPE,
                                        MINIMUM_BATCH_SIZE,
                                        MAXIMUM_BATCH_SIZE,
                                        PROGRAM_CODE 
                                    FROM 
                                        TRAINING_PROGRAM 
                                    WHERE 
                                        PROGRAM_ID ='" + programId + "' ";
                MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlQuery, mySqlCon);
                sqlDa.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public bool checkProgrameExistance(string trainerId, string programId)
        {
            DataTable resultTable = new DataTable();
            bool isUsed = false;
            try
            {

                string sqlQuery = " SELECT TRAINER_ID FROM TRAINER_TRAINING_PROGRAMS WHERE TRAINER_ID ='" + trainerId + "' && TRAINING_PROGRAM_ID ='" + programId + "' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                if (resultTable.Rows.Count > 0)
                {
                    isUsed = true;
                }
                return isUsed;
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
                resultTable.Dispose();
            }
        }

        public bool addProgrameToTrainer(string trainerId, string programeId, string costPerSession, string description ,string status, string addedUserId)
        {
            bool inserted = false;

            try
            {
                mySqlCon.Open();

                mySqlCmd.Parameters.Add(new MySqlParameter("@trainerId", trainerId.Trim() == "" ? (object)DBNull.Value : trainerId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@programeId", programeId.Trim() == "" ? (object)DBNull.Value : programeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@costPerSession", costPerSession.Trim() == "" ? (object)DBNull.Value : costPerSession.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                string sqlQuery = " INSERT INTO TRAINER_TRAINING_PROGRAMS " +
                                    " (TRAINER_ID, TRAINING_PROGRAM_ID, COST_PER_SESSION, DESCRIPTION, STATUS_CODE, ADDED_BY, ADDED_DATE, MODIFIED_BY, MODIFIED_DATE) " +
                                    " values (@trainerId,@programeId,@costPerSession,@description,@status,@addedUserId,now(),@addedUserId,now())";

                mySqlCmd.CommandText = sqlQuery;
                mySqlCmd.ExecuteNonQuery();
                inserted = true;
                return inserted;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                mySqlCon.Close();
            }

        }

        public bool addProgrameStatusInTrainer(string trainerId, string programId, string costPerSession, string description ,string status, string UserId)
        {
            bool updated = false;
            try
            {
                mySqlCon.Open();

                mySqlCmd.Parameters.Add(new MySqlParameter("@trainerId", trainerId.Trim() == "" ? (object)DBNull.Value : trainerId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@programId", programId.Trim() == "" ? (object)DBNull.Value : programId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@costPerSession", costPerSession.Trim() == "" ? (object)DBNull.Value : costPerSession.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@UserId", UserId.Trim() == "" ? (object)DBNull.Value : UserId.Trim()));

                string mySqlQuery = "UPDATE TRAINER_TRAINING_PROGRAMS SET " +
                                    "COST_PER_SESSION = @costPerSession, " +
                                    "DESCRIPTION = @description, " +
                                    "STATUS_CODE = @status, " +
                                    "MODIFIED_BY = @UserId, " +
                                    "MODIFIED_DATE = now() " +
                                    "WHERE TRAINER_ID = @trainerId " +
                                    " && TRAINING_PROGRAM_ID = @programId ";

                mySqlCmd.CommandText = mySqlQuery;
                mySqlCmd.ExecuteNonQuery();
                updated = true;
                return updated;
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
        }

        public DataTable getTrainerProgrameDetails(string trainerId, string programId)
        {
            DataTable resultTable = new DataTable();
            
            try
            {

                string sqlQuery = @"SELECT 
                                        TRAINING_PROGRAM_ID, 
                                        COST_PER_SESSION,
                                        DESCRIPTION,
                                        STATUS_CODE
                                    FROM 
                                        TRAINER_TRAINING_PROGRAMS 
                                    WHERE 
                                        TRAINER_ID ='" + trainerId + "' && TRAINING_PROGRAM_ID ='" + programId + "' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                return resultTable;
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
                resultTable.Dispose();
            }
        }
    }
}
