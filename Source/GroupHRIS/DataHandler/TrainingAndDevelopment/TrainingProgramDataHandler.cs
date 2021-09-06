using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainingProgramDataHandler : TemplateDataHandler
    {
        #region Training Program
        public DataTable getTrainingTypes()
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
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
                mySqlCon.Close();
            }
        }

        public DataTable getTrainingCategories()
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
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
                mySqlCon.Close();
            }
        }

        public DataTable getSubcategoriesForCategory(string categoryId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                string sqlQuery = "SELECT TYPE_ID, TYPE_NAME FROM TRAINING_SUB_CATEGORY WHERE CATEGORY_ID ='" + categoryId + "' && STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";
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

        public Boolean Insert(string programeCode, string programeName, string trainigType, string trainigCategory, string trainingSubcategory, decimal duration, string programType, int minBatch, int maxBatch, string description, string objectives, string trainigStatus,string addedUserId, DataTable dtOutcome)
        {

            Boolean inserted = false;
            MySqlTransaction mySqlTrans = null;
            string mySqlQuery = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler serialHandler = new SerialHandler();
                string programeId = serialHandler.getserilalReference(ref mySqlCon, "TP");

                mySqlCmd.Parameters.Add(new MySqlParameter("@programeId", programeId.Trim() == "" ? (object)DBNull.Value : programeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@programeCode", programeCode.Trim() == "" ? (object)DBNull.Value : programeCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@programeName", programeName.Trim() == "" ? (object)DBNull.Value : programeName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@trainigType", trainigType.Trim() == "" ? (object)DBNull.Value : trainigType.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@trainigCategory", trainigCategory.Trim() == "" ? (object)DBNull.Value : trainigCategory.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@trainigSubcategory", trainingSubcategory.Trim() == "" ? (object)DBNull.Value : trainingSubcategory.Trim()));

                mySqlCmd.Parameters.Add(new MySqlParameter("@programType", programType.Trim() == "" ? (object)DBNull.Value : programType.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@minBatch", minBatch  == "" ? (object)DBNull.Value : minBatch.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@maxBatch", maxBatch.Trim() == "" ? (object)DBNull.Value : maxBatch.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@objectives", objectives.Trim() == "" ? (object)DBNull.Value : objectives.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@trainigStatus", trainigStatus.Trim() == "" ? (object)DBNull.Value : trainigStatus.Trim()));

                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));


                mySqlQuery = @"insert into TRAINING_PROGRAM (PROGRAM_ID, 
                                TRAINING_TYPE,
                                TRAINING_CATEGORY,
                                TRAINING_SUBCATEGORY, 
                                PROGRAM_CODE, 
                                PROGRAM_NAME,
                                DESCRIPTION,
                                OBJECTIVES, 
                                PROGRAM_DURATION,
                                MINIMUM_BATCH_SIZE,
                                MAXIMUM_BATCH_SIZE,
                                PROGRAM_TYPE,
                                STATUS_CODE,
                                ADDED_BY,
                                ADDED_DATE,
                                MODIFIED_BY,
                                MODIFIED_DATE)" +
                             " values (@programeId,@trainigType,@trainigCategory,@trainigSubcategory,@programeCode,@programeName,@description, @objectives,'" + duration + "','" + minBatch + "','" + maxBatch + "',@programType,@trainigStatus,@addedUserId,now(),@addedUserId,now())";

                mySqlCmd.CommandText = mySqlQuery;
                mySqlCmd.ExecuteNonQuery();

                /// begin adding Outcomes to programe ////
                #region add Outcomes to programe

                string outcomeId = "";
                string outcomeText = "";
                string outcomeStatus = "";


                //outcomeId = serialHandler.getserilalReference(ref mySqlCon, "TO");

                foreach (DataRow outcome in dtOutcome.Rows)
                {
                    mySqlCmd.Parameters.Clear();

                    outcomeId = serialHandler.getserilalReference(ref mySqlCon, "TO");
                    outcomeText = outcome["outcome"].ToString();
                    outcomeStatus = outcome["outcomeStatus"].ToString();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@programeId", programeId.Trim() == "" ? (object)DBNull.Value : programeId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@outcomeId", outcomeId.Trim() == "" ? (object)DBNull.Value : outcomeId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@outcomeText", outcomeText.Trim() == "" ? (object)DBNull.Value : outcomeText.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@outcomeStatus", outcomeStatus.Trim() == "" ? (object)DBNull.Value : outcomeStatus.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                    string addOutComeQuery = @" insert into TRAINING_OUTCOMES (
                                                OUTCOME_ID, PROGRAM_ID, OUTCOME, STATUS_CODE, ADDED_BY, ADDED_DATE, MODIFIED_BY, MODIFIED_DATE )
                                                values (@outcomeId, @programeId, @outcomeText, @outcomeStatus, @addedUserId, now(), @addedUserId, now()) ";


                    mySqlCmd.CommandText = addOutComeQuery;
                    mySqlCmd.ExecuteNonQuery();
                    //outcomeId = "TO"+(Convert.ToInt32(outcomeId.Replace("TO", "")) + 1).ToString("00000000");
                    
                }

                # endregion
                /// end adding Outcomes to programe ////

                mySqlCmd.Transaction = mySqlTrans;
                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                serialHandler = null;
                inserted = true;
            }
            catch (Exception e)
            {
                mySqlTrans.Rollback(); 
                inserted = false;
                throw e;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }

            return inserted;
        }

        public DataTable getAllTrainingProgrames()
        {
            DataTable resultTable = new DataTable();
            try
            {
                //mySqlCon.Open();
                string sqlQuery = @"SELECT PROGRAM_ID,PROGRAM_CODE,PROGRAM_NAME,PROGRAM_DURATION,TT.TYPE_NAME,TC.CATEGORY_NAME,
                                    CASE 
                                        when TP.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "' " +
                                        "when TP.STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' ";
                       sqlQuery += @"end as STATUS_CODE
                                    FROM TRAINING_PROGRAM TP
                                    left join TRAINING_TYPE TT on TP.TRAINING_TYPE = TT.TRAINING_TYPE_ID
                                    left join TRAINING_CATEGORY TC on TP.TRAINING_CATEGORY = TC.TRAINING_CATEGORY_ID
                                    ORDER BY PROGRAM_ID DESC";
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

        public DataTable getProgrameById(string selectedPrograme)
        {
            DataTable resultTable = new DataTable();
            try
            {
                //mySqlCon.Open();
                string sqlQuery = @"SELECT PROGRAM_ID,PROGRAM_CODE,PROGRAM_NAME,PROGRAM_DURATION,TT.TRAINING_TYPE_ID,TT.TYPE_NAME,TC.TRAINING_CATEGORY_ID, TC.CATEGORY_NAME, TP.TRAINING_SUBCATEGORY, TP.PROGRAM_TYPE, TP.DESCRIPTION, TP.OBJECTIVES, TP.MINIMUM_BATCH_SIZE, TP.MAXIMUM_BATCH_SIZE,TP.STATUS_CODE
                                    FROM TRAINING_PROGRAM TP
                                    left join TRAINING_TYPE TT on TP.TRAINING_TYPE = TT.TRAINING_TYPE_ID
                                    left join TRAINING_CATEGORY TC on TP.TRAINING_CATEGORY = TC.TRAINING_CATEGORY_ID
                                    WHERE PROGRAM_ID = '" + selectedPrograme+"' ";
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

        public DataTable getOutcomesByProgrameId(string selectedProgrameId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                //mySqlCon.Open();
                mySqlCmd.Parameters.Add(new MySqlParameter("@programeId", selectedProgrameId.Trim() == "" ? (object)DBNull.Value : selectedProgrameId.Trim()));

                string sqlQuery = @"SELECT OUTCOME_ID as outcomeId, OUTCOME as outcome, STATUS_CODE as outcomeStatus
                                    FROM TRAINING_OUTCOMES 
                                    WHERE PROGRAM_ID = '"+selectedProgrameId+"' ";
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

        public bool Update(string programeId, string programeCode, string programeName, string trainigType, string trainigCategory, string trainingSubcategory, decimal duration, string programType, int minBatch, int maxBatch, string description, string objectives, string trainigStatus, string addedUserId, DataTable dtOutcomes)
        {
            Boolean updated = false;
            MySqlTransaction mySqlTrans = null;
            string mySqlQuery = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@programeId", programeId.Trim() == "" ? (object)DBNull.Value : programeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@programeCode", programeCode.Trim() == "" ? (object)DBNull.Value : programeCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@programeName", programeName.Trim() == "" ? (object)DBNull.Value : programeName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@trainigType", trainigType.Trim() == "" ? (object)DBNull.Value : trainigType.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@trainigCategory", trainigCategory.Trim() == "" ? (object)DBNull.Value : trainigCategory.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@trainigSubcategory", trainingSubcategory.Trim() == "" ? (object)DBNull.Value : trainingSubcategory.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@programType", programType.Trim() == "" ? (object)DBNull.Value : programType.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@minBatch", minBatch  == "" ? (object)DBNull.Value : minBatch.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@maxBatch", maxBatch.Trim() == "" ? (object)DBNull.Value : maxBatch.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@objectives", objectives.Trim() == "" ? (object)DBNull.Value : objectives.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@trainigStatus", trainigStatus.Trim() == "" ? (object)DBNull.Value : trainigStatus.Trim()));

                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));


                mySqlQuery = @"UPDATE TRAINING_PROGRAM SET 
                                TRAINING_TYPE = @trainigType,
                                TRAINING_CATEGORY = @trainigCategory, 
                                TRAINING_SUBCATEGORY = @trainigSubcategory,
                                PROGRAM_CODE = @programeCode, 
                                PROGRAM_NAME = @programeName,
                                DESCRIPTION = @description, 
                                OBJECTIVES = @objectives,
                                PROGRAM_DURATION =" + duration+", "+
                                "MINIMUM_BATCH_SIZE ="+minBatch+", "+
                                "MAXIMUM_BATCH_SIZE ="+maxBatch+", ";
                mySqlQuery +=  @"PROGRAM_TYPE = @programType, 
                                STATUS_CODE = @trainigStatus,
                                MODIFIED_BY = @addedUserId,
                                MODIFIED_DATE = now()
                                WHERE PROGRAM_ID = @programeId";
                                             
                mySqlCmd.CommandText = mySqlQuery;
                mySqlCmd.ExecuteNonQuery();

                ///// delete existing outcomes ////////
                string deleteExistingOutcomes = " DELETE FROM TRAINING_OUTCOMES WHERE PROGRAM_ID ='" + programeId + "' ";
                mySqlCmd.CommandText = deleteExistingOutcomes;
                mySqlCmd.ExecuteNonQuery();
                ///// end delete existing outcomes ////////

                /// begin adding Outcomes to programe ////
                #region add Outcomes to programe

                string outcomeId = "";
                string outcomeText = "";
                string outcomeStatus = "";

                SerialHandler serialHandler = new SerialHandler();
                

                foreach (DataRow outcome in dtOutcomes.Rows)
                {
                    mySqlCmd.Parameters.Clear();

                    outcomeId = serialHandler.getserilalReference(ref mySqlCon, "TO");
                    outcomeText = outcome["outcome"].ToString();
                    outcomeStatus = outcome["outcomeStatus"].ToString();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@programeId", programeId.Trim() == "" ? (object)DBNull.Value : programeId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@outcomeId", outcomeId.Trim() == "" ? (object)DBNull.Value : outcomeId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@outcomeText", outcomeText.Trim() == "" ? (object)DBNull.Value : outcomeText.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@outcomeStatus", outcomeStatus.Trim() == "" ? (object)DBNull.Value : outcomeStatus.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                    string addOutComeQuery = @" insert into TRAINING_OUTCOMES (
                                                OUTCOME_ID, PROGRAM_ID, OUTCOME, STATUS_CODE, ADDED_BY, ADDED_DATE, MODIFIED_BY, MODIFIED_DATE )
                                                values (@outcomeId, @programeId, @outcomeText, @outcomeStatus, @addedUserId, now(), @addedUserId, now()) ";


                    mySqlCmd.CommandText = addOutComeQuery;
                    mySqlCmd.ExecuteNonQuery();
                    //outcomeId = "TO" + (Convert.ToInt32(outcomeId.Replace("TO", "")) + 1).ToString("00000000");

                }

                # endregion
                /// end adding Outcomes to programe ////
                             
                mySqlCmd.Transaction = mySqlTrans;
                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                serialHandler = null;
                updated = true;
            }
            catch(Exception)
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
            return updated;
        }

        public DataTable filterProgrames(string selectedType, string selectedCategory, string selectedSubcategory)
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                //string sqlQuery = "SELECT * FROM TRAINING_PROGRAM WHERE STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";
                string sqlQuery = @"SELECT PROGRAM_ID,PROGRAM_CODE,PROGRAM_NAME,PROGRAM_DURATION,TT.TYPE_NAME,TC.CATEGORY_NAME,
                                    CASE 
                                        when TP.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "' " +
                                        "when TP.STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' END AS STATUS_CODE ";

                sqlQuery += @" FROM TRAINING_PROGRAM TP 
                            left join TRAINING_TYPE TT on TP.TRAINING_TYPE = TT.TRAINING_TYPE_ID
                            left join TRAINING_CATEGORY TC on TP.TRAINING_CATEGORY = TC.TRAINING_CATEGORY_ID ";

                if (!String.IsNullOrEmpty(selectedType) && !String.IsNullOrEmpty(selectedCategory))
                {
                    sqlQuery += " WHERE TP.TRAINING_TYPE ='" + selectedType + "' ";
                    sqlQuery += " and TP.TRAINING_CATEGORY ='" + selectedCategory + "' ";

                    if (!String.IsNullOrEmpty(selectedSubcategory))
                    {
                        sqlQuery += " and TP.TRAINING_SUBCATEGORY ='" + selectedSubcategory + "' ";
                    }

                }

                if (!String.IsNullOrEmpty(selectedCategory) && String.IsNullOrEmpty(selectedType))
                {
                    sqlQuery += " WHERE TP.TRAINING_CATEGORY ='" + selectedCategory + "' ";
                    if (!String.IsNullOrEmpty(selectedSubcategory))
                    {
                        sqlQuery += " and TP.TRAINING_SUBCATEGORY ='" + selectedSubcategory + "' ";
                    }
                   
                }
                if (!String.IsNullOrEmpty(selectedType) && String.IsNullOrEmpty(selectedCategory))
                {
                    sqlQuery += " WHERE TP.TRAINING_TYPE ='" + selectedType + "' ";
                    //sqlQuery += " and TC.TRAINING_CATEGORY ='" + selectedCategory + "' ";
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
                mySqlCon.Close();
            }
        }

        //        public bool InsertOutcome(string programeId, string outcome, string outcomeStatus, string addedUserId)
//        {
//            Boolean inserted = false;



//            try
//            {
//                mySqlCon.Open();

//                SerialHandler serialCode = new SerialHandler();
//                string outcomeId = serialCode.getserilalReference(ref mySqlCon, "TO");

//                string outcomeText = outcome;
//                string Status = outcomeStatus;

//                mySqlCmd.Parameters.Add(new MySqlParameter("@programeId", programeId.Trim() == "" ? (object)DBNull.Value : programeId.Trim()));
//                mySqlCmd.Parameters.Add(new MySqlParameter("@outcomeId", outcomeId.Trim() == "" ? (object)DBNull.Value : outcomeId.Trim()));
//                mySqlCmd.Parameters.Add(new MySqlParameter("@outcomeText", outcomeText.Trim() == "" ? (object)DBNull.Value : outcomeText.Trim()));
//                mySqlCmd.Parameters.Add(new MySqlParameter("@status", outcomeStatus.Trim() == "" ? (object)DBNull.Value : outcomeStatus.Trim()));
//                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

//                string addOutcomeQuery = @" insert into TRAINING_OUTCOMES (
//                                                OUTCOME_ID, PROGRAM_ID, OUTCOME, STATUS_CODE, ADDED_BY, ADDED_DATE, MODIFIED_BY, MODIFIED_DATE )
//                                                values (@outcomeId, @programeId, @outcomeText, @outcomeStatus, @addedUserId, now(), @addedUserId, now()) ";


//                mySqlCmd.CommandText = addOutcomeQuery;
//                mySqlCmd.ExecuteNonQuery();
//                mySqlCon.Close();
//                mySqlCmd.Dispose();

//                inserted = true;
//            }
//            catch (Exception)
//            {

//                inserted = false;
//            }
//            finally
//            {
//                if (mySqlCon.State == ConnectionState.Open)
//                {
//                    mySqlCon.Close();
//                }
//            }


//            return inserted;
        //        }

        #endregion

        #region Trainer Training Programe

        public DataTable getAllTrainers()
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = " SELECT TRAINER_ID, NAME_WITH_INITIALS, CONTACT_MOBILE, COST_PER_SESSION," +
                                  " CASE "+
                                        "when STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "' " +
                                        "when STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' "+
                                  "end as STATUS_CODE "+
                                  " FROM TRAINER_INFROMATION WHERE STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ORDER BY NAME_WITH_INITIALS";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                
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

        public DataTable getTrainerById(string trainerId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = " SELECT TRAINER_ID, NAME_WITH_INITIALS, CONTACT_MOBILE, COST_PER_SESSION, QUALIFICATIONS, IS_EXTERNAL,ADDRESS, TN.NAME as NATURE, TI.DESCRIPTION, " +
                                  " CASE " +
                                        "when TI.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "' " +
                                        "when TI.STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                  "end as STATUS_CODE ";

                      sqlQuery += @"FROM TRAINER_INFROMATION TI
                                        left join TRAINING_NATURE TN 
                                                on TI.TRAINING_NATURE_ID = TN.TRAINING_NATURE_ID
                                  WHERE TI.TRAINER_ID ='" + trainerId + "' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

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

        public bool checkTrainerExistanceForPrograme(string trainerId, string programId)
        {
            bool isExist = false;
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = " SELECT TRAINING_PROGRAM_ID FROM TRAINER_TRAINING_PROGRAMS WHERE TRAINING_PROGRAM_ID ='" + programId + "' && TRAINER_ID ='" + trainerId + "' ";
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                if (resultTable.Rows.Count > 0)
                {
                    isExist = true;
                }
                return isExist;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw;
            }
            finally
            {
                resultTable.Dispose();
            }
        }

        public bool addTrainer(string trainerId, string programeId, string costPerSession, string description, string addedUserId, string status)
        {
            bool inserted = false;
            try
            {
                mySqlCon.Open();
                mySqlCmd.Parameters.Add(new MySqlParameter("@programeId", programeId.Trim() == "" ? (object)DBNull.Value : programeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@trainerId", trainerId.Trim() == "" ? (object)DBNull.Value : trainerId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@costPerSession", costPerSession.Trim() == "" ? (object)DBNull.Value : costPerSession.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                string sqlString = " INSERT INTO TRAINER_TRAINING_PROGRAMS (TRAINING_PROGRAM_ID, TRAINER_ID, COST_PER_SESSION, DESCRIPTION, STATUS_CODE, ADDED_BY, ADDED_DATE, MODIFIED_BY, MODIFIED_DATE) " +
                                    " VALUES (@programeId,@trainerId,@costPerSession,@description,@status,@addedUserId,now(),@addedUserId,now()) ";

                mySqlCmd.CommandText = sqlString;
                mySqlCmd.ExecuteNonQuery();
                mySqlCon.Close();
                mySqlCmd.Dispose();

                inserted = true;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                
                throw;
            }
            return inserted;
        }

        public bool updateTrainer(string trainerId, string programeId, string costPerSession, string description, string addedUserId, string status)
        {
            bool updated = false;
            try
            {
                mySqlCon.Open();
                mySqlCmd.Parameters.Add(new MySqlParameter("@programeId", programeId.Trim() == "" ? (object)DBNull.Value : programeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@trainerId", trainerId.Trim() == "" ? (object)DBNull.Value : trainerId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@costPerSession", costPerSession.Trim() == "" ? (object)DBNull.Value : costPerSession.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                string sqlString = @"   UPDATE TRAINER_TRAINING_PROGRAMS 
                                        SET
                                            COST_PER_SESSION = @costPerSession,
                                            DESCRIPTION = @description,
                                            STATUS_CODE = @status, 
                                            MODIFIED_BY = @addedUserId, 
                                            MODIFIED_DATE = now()
                                            WHERE TRAINER_ID ='"+trainerId+"' and TRAINING_PROGRAM_ID ='"+programeId+"' ";

                mySqlCmd.CommandText = sqlString;
                mySqlCmd.ExecuteNonQuery();
                mySqlCon.Close();
                mySqlCmd.Dispose();

                updated = true;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw;
            }
            return updated;
        }

        public DataTable getTrainersToPrograme(string selectedProgrameId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = @"select TI.TRAINER_ID, TI.NAME_WITH_INITIALS , TTP.COST_PER_SESSION,  "+
                                   " CASE "+
                                        "when TTP.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "' " +
                                        "when TTP.STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' "+
                                   "end as STATUS_CODE ";
                       sqlQuery += @" from TRAINER_TRAINING_PROGRAMS TTP
                                    left join TRAINER_INFROMATION TI
	                                    on TTP.TRAINER_ID = TI.TRAINER_ID
                                    where TTP.TRAINING_PROGRAM_ID = '"+selectedProgrameId+"'";

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
                resultTable.Dispose();
                mySqlCon.Close();
            }
        }

        public DataTable getTrainerProgrameDetails(string trainerId, string programId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = @"select TI.TRAINER_ID, TI.NAME_WITH_INITIALS , TTP.COST_PER_SESSION, TTP.DESCRIPTION ,TI.QUALIFICATIONS, TI.IS_EXTERNAL ,ADDRESS, TN.NAME as NATURE, TI.CONTACT_MOBILE, TTP.STATUS_CODE";
                                   //" CASE " +
                                   //     "when TTP.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "' " +
                                   //     "when TTP.STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                   //"end as STATUS_CODE ";
                sqlQuery += @" from TRAINER_TRAINING_PROGRAMS TTP
                                    left join TRAINER_INFROMATION TI
	                                    on TTP.TRAINER_ID = TI.TRAINER_ID
                                    left join TRAINING_NATURE TN 
	                                on TI.TRAINING_NATURE_ID = TN.TRAINING_NATURE_ID
                                    where TTP.TRAINER_ID = '" + trainerId + "' AND "+
                                    " TTP.TRAINING_PROGRAM_ID ='"+programId+"' ";

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
                resultTable.Dispose();
                mySqlCon.Close();
            }
        }

        public DataTable getFilteredTrainers(string selectedCompetency)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = " SELECT TI.TRAINER_ID, TI.NAME_WITH_INITIALS, TI.CONTACT_MOBILE, TI.COST_PER_SESSION," +
                                  " CASE " +
                                        "when TI.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "' " +
                                        "when TI.STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                  "end as STATUS_CODE " +
                                  " FROM TRAINER_INFROMATION TI "+
                                  " LEFT JOIN TRAINER_COMPETENCY TC ON TI.TRAINER_ID = TC.TRAINER_ID "+
                                  " WHERE TI.STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' AND TC.STATUS_CODE = '"+ Constants.STATUS_ACTIVE_VALUE +"' AND TC.COMPETENCY_ID = '"+selectedCompetency+"' ORDER BY NAME_WITH_INITIALS";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

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

        public DataTable getTrainerCompetenciesById(string trainerId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = @"SELECT TC.COMPETENCY_ID, TCA.NAME
                                    FROM TRAINER_COMPETENCY TC
	                                    LEFT JOIN TRAINER_COMPETENCY_AREA TCA
			                                      ON TC.COMPETENCY_ID = TCA.COMPETENCY_ID
                                    WHERE TC.TRAINER_ID = '" + trainerId + "' ";
                sqlQuery += @" && TC.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {
                throw;
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            finally
            {
                resultTable.Dispose();
                mySqlCon.Close();
            }
        }
        #endregion

        
    }
}
