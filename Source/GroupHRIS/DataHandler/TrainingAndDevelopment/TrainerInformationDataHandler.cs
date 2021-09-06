using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainerInformationDataHandler : TemplateDataHandler
    {
        public DataTable getAllBanks()
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = "SELECT BANK_ID, BANK_NAME FROM BANK ";

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

        public DataTable getBranchesForSelectedBank(string bankId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = "SELECT BRANCH_ID, BRANCH_NAME FROM BANK_BRANCH WHERE BANK_ID ='"+bankId+"' ";

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

        public DataTable getTrainingNature()
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = "SELECT TRAINING_NATURE_ID, NAME FROM TRAINING_NATURE WHERE STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";

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

        public bool insertTrainer(string nameInitial, string nameFull, Byte[] bytes, string nic, string internalExternal, string landline, string mobile, string address, string bank, string branch, string accountNo, string payment, string description, string qualifications, string nature, string status, string addedUserId, DataTable dtCompetencies)
        {

            Boolean inserted = false;
            MySqlTransaction mySqlTrans = null;
            string mySqlQuery = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler serialCode = new SerialHandler();
                string trainerId = serialCode.getserila(mySqlCon, "TRI");

                mySqlCmd.Parameters.Add(new MySqlParameter("@trainerId", trainerId.Trim() == "" ? (object)DBNull.Value : trainerId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@nameInitial", nameInitial.Trim() == "" ? (object)DBNull.Value : nameInitial.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@nameFull", nameFull.Trim() == "" ? (object)DBNull.Value : nameFull.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@imageUrl", imageUrl.Trim() == "" ? (object)DBNull.Value : imageUrl.Trim()));
                mySqlCmd.Parameters.Add("@imageUrl", MySqlDbType.Blob).Value = bytes;
                mySqlCmd.Parameters.Add(new MySqlParameter("@nic", nic.Trim() == "" ? (object)DBNull.Value : nic.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@internalExternal", internalExternal.Trim() == "" ? (object)DBNull.Value : internalExternal.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@landline", landline.Trim() == "" ? (object)DBNull.Value : landline.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@mobile", mobile.Trim() == "" ? (object)DBNull.Value : mobile.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@address", address.Trim() == "" ? (object)DBNull.Value : address.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@bank", bank.Trim() == "" ? (object)DBNull.Value : bank.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@branch", branch.Trim() == "" ? (object)DBNull.Value : branch.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@accountNo", accountNo.Trim() == "" ? (object)DBNull.Value : accountNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@paymentInstructions", payment.Trim() == "" ? (object)DBNull.Value : payment.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@qualifications", qualifications.Trim() == "" ? (object)DBNull.Value : qualifications.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@nature", nature.Trim() == "" ? (object)DBNull.Value : nature.Trim()));

                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));


                mySqlQuery = @" insert into 
                                        TRAINER_INFROMATION 
                                            (
                                                TRAINER_ID, 
                                                NAME_WITH_INITIALS, 
                                                FULL_NAME, 
                                                NIC, 
                                                IS_EXTERNAL, 
                                                CONTACT_LAND, 
                                                CONTACT_MOBILE, 
                                                ADDRESS, 
                                                BANK_ID, 
                                                BANK_BRANCH_ID, 
                                                ACCOUNT_NUMBER, 
                                                DESCRIPTION, 
                                                QUALIFICATIONS, 
                                                PHOTO, 
                                                TRAINING_NATURE_ID, 
                                                PAYMENT_INSTRUCTIONS, 
                                                STATUS_CODE, 
                                                ADDED_BY, 
                                                ADDED_DATE, 
                                                MODIFIED_BY, 
                                                MODIFIED_DATE
                                            ) 
                                        values 
                                            (
                                                @trainerId,
                                                @nameInitial,
                                                @nameFull,
                                                @nic,
                                                @internalExternal,
                                                @landline,
                                                @mobile,
                                                @address,
                                                @bank,
                                                @branch,
                                                @accountNo,
                                                @description,
                                                @qualifications,
                                                @imageUrl,
                                                @nature,
                                                @paymentInstructions,
                                                @status,
                                                @addedUserId,
                                                now(),
                                                @addedUserId,
                                                now()
                                            )
                                ";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = mySqlQuery;
                mySqlCmd.ExecuteNonQuery();


                /// Adding Competencies to the trainer///
                /// 

                foreach (DataRow competency in dtCompetencies.Rows)
                {
                    mySqlCmd.Parameters.Clear();

                    string competencyId = competency["COMPETENCY_ID"].ToString();
                    string competencyDesc = competency["DESCRIPTION"].ToString();
                    string compStatus = competency["STATUS_CODE"].ToString();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@trainerId", trainerId.Trim() == "" ? (object)DBNull.Value : trainerId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@competencyId", competencyId.Trim() == "" ? (object)DBNull.Value : competencyId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@competencyDesc", competencyDesc.Trim() == "" ? (object)DBNull.Value : competencyDesc.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@compStatus", compStatus.Trim() == "" ? (object)DBNull.Value : compStatus.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                    string competencyInsertString = @" INSERT INTO TRAINER_COMPETENCY (COMPETENCY_ID, TRAINER_ID, DESCRIPTION, STATUS_CODE, ADDED_BY, ADDED_DATE, MODIFIED_BY, MODIFIED_DATE)
                                                           VALUES (@competencyId, @trainerId, @competencyDesc, @compStatus, @addedUserId, now(), @addedUserId, now())";

                    mySqlCmd.CommandText = competencyInsertString;
                    mySqlCmd.ExecuteNonQuery();
                }

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                inserted = true;
            }
            catch (Exception e)
            {
                mySqlTrans.Rollback();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }


                inserted = false;
                throw e;
            }

            return inserted;


        }

        public bool updateTrainer(string trainerId, string nameInitial, string nameFull, Byte [] bytes, string nic, string internalExternal, string landline, string mobile, string address, string bank, string branch, string accountNo, string payment, string description, string qualifications, string nature, string status, string addedUserId, DataTable dtCompetencies)
        {
            Boolean updated = false;
            MySqlTransaction mySqlTrans = null;
            string mySqlQuery = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@trainerId", trainerId.Trim() == "" ? (object)DBNull.Value : trainerId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@nameInitial", nameInitial.Trim() == "" ? (object)DBNull.Value : nameInitial.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@nameFull", nameFull.Trim() == "" ? (object)DBNull.Value : nameFull.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@imageUrl", imageUrl.Trim() == "" ? (object)DBNull.Value : imageUrl.Trim()));
                mySqlCmd.Parameters.Add("@imageUrl", MySqlDbType.Blob).Value = bytes;
                mySqlCmd.Parameters.Add(new MySqlParameter("@nic", nic.Trim() == "" ? (object)DBNull.Value : nic.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@internalExternal", internalExternal.Trim() == "" ? (object)DBNull.Value : internalExternal.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@landline", landline.Trim() == "" ? (object)DBNull.Value : landline.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@mobile", mobile.Trim() == "" ? (object)DBNull.Value : mobile.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@address", address.Trim() == "" ? (object)DBNull.Value : address.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@bank", bank.Trim() == "" ? (object)DBNull.Value : bank.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@branch", branch.Trim() == "" ? (object)DBNull.Value : branch.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@accountNo", accountNo.Trim() == "" ? (object)DBNull.Value : accountNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@payment", payment.Trim() == "" ? (object)DBNull.Value : payment.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@qualifications", qualifications.Trim() == "" ? (object)DBNull.Value : qualifications.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@nature", nature.Trim() == "" ? (object)DBNull.Value : nature.Trim()));

                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));


                mySqlQuery = @" UPDATE TRAINER_INFROMATION SET 
                                    NAME_WITH_INITIALS = @nameInitial, 
                                    FULL_NAME = @nameFull, 
                                    NIC = @nic, 
                                    IS_EXTERNAL = @internalExternal, 
                                    CONTACT_LAND = @landline, 
                                    CONTACT_MOBILE = @mobile, 
                                    ADDRESS = @address, 
                                    BANK_ID = @bank, 
                                    BANK_BRANCH_ID = @branch, 
                                    ACCOUNT_NUMBER = @accountNo, 
                                    DESCRIPTION = @description, 
                                    QUALIFICATIONS = @qualifications, 
                                    PHOTO = @imageUrl, 
                                    TRAINING_NATURE_ID = @nature, 
                                    PAYMENT_INSTRUCTIONS = @payment, 
                                    STATUS_CODE = @status, 
                                    MODIFIED_BY = @addedUserId, 
                                    MODIFIED_DATE = now()
                                WHERE TRAINER_ID = @trainerId";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = mySqlQuery;
                mySqlCmd.ExecuteNonQuery();


                /// Adding / Updating Competencies to the trainer///

                foreach (DataRow competency in dtCompetencies.Rows)
                {
                    mySqlCmd.Parameters.Clear();

                    string competencyId = competency["COMPETENCY_ID"].ToString();

                    bool competencyExist = checkCompetencyExistanceForTrainer(trainerId, competencyId);

                    string competencyDesc = competency["DESCRIPTION"].ToString();
                    string compStatus = competency["STATUS_CODE"].ToString();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@trainerId", trainerId.Trim() == "" ? (object)DBNull.Value : trainerId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@competencyId", competencyId.Trim() == "" ? (object)DBNull.Value : competencyId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@competencyDesc", competencyDesc.Trim() == "" ? (object)DBNull.Value : competencyDesc.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@compStatus", compStatus.Trim() == "" ? (object)DBNull.Value : compStatus.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                    if (!competencyExist)
                    {
                        string competencyInsertString = @" INSERT INTO TRAINER_COMPETENCY (COMPETENCY_ID, TRAINER_ID, DESCRIPTION, STATUS_CODE, ADDED_BY, ADDED_DATE, MODIFIED_BY, MODIFIED_DATE)
                                                           VALUES (@competencyId, @trainerId, @competencyDesc, @compStatus, @addedUserId, now(), @addedUserId, now())";
                        mySqlCmd.CommandText = competencyInsertString;
                    }
                    else
                    {
                        string competencyUpdateString = @" UPDATE TRAINER_COMPETENCY SET
                                                            DESCRIPTION = @competencyDesc, 
                                                            STATUS_CODE = @compStatus, 
                                                            MODIFIED_BY = @addedUserId, 
                                                            MODIFIED_DATE = now()
                                                        WHERE 
                                                            COMPETENCY_ID = @competencyId 
                                                            && TRAINER_ID = @trainerId";

                        mySqlCmd.CommandText = competencyUpdateString;
                    }

                    
                    mySqlCmd.ExecuteNonQuery();
                }

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                updated = true;
            }
            catch (Exception e)
            {
                mySqlTrans.Rollback();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }


                updated = false;
                throw e;
            }

            return updated;
        }

        private bool checkCompetencyExistanceForTrainer(string trainerId, string competencyId)
        {
            DataTable resultTable = new DataTable();
            
            try
            {
                bool isExist = false;
                string sqlQuery = "SELECT COMPETENCY_ID FROM TRAINER_COMPETENCY WHERE COMPETENCY_ID ='"+competencyId+"' and TRAINER_ID ='"+trainerId+"' ";

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

                throw;
            }
            //// do not close the connection here. This methode is called from trainerUpdate()   
        }

        public DataTable getCompetencies()
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = @"SELECT 
                                        COMPETENCY_ID, NAME 
                                    FROM
                                        TRAINER_COMPETENCY_AREA
                                    WHERE 
                                        STATUS_CODE ='"+ Constants.STATUS_ACTIVE_VALUE+"' ";

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

        public DataTable getAllTrainers()
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = " SELECT TRAINER_ID, NAME_WITH_INITIALS AS NAME, CONTACT_MOBILE, TN.NAME AS TRAINING_NATURE, TI.TRAINING_NATURE_ID, " +
                                  " CASE " +
                                        "when TI.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "' " +
                                        "when TI.STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                  "end as STATUS " +
                                  " FROM TRAINER_INFROMATION TI " +
                                  " LEFT JOIN TRAINING_NATURE TN ON TI.TRAINING_NATURE_ID = TN.TRAINING_NATURE_ID ORDER BY TRAINER_ID DESC";

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
                string sqlQuery = @" SELECT TRAINER_ID, NAME_WITH_INITIALS, CONTACT_MOBILE, QUALIFICATIONS, 
                                            IS_EXTERNAL,ADDRESS, TN.TRAINING_NATURE_ID, TN.NAME as NATURE_NAME, TI.DESCRIPTION, 
                                            FULL_NAME, NIC, CONTACT_LAND, BANK_ID, BANK_BRANCH_ID, ACCOUNT_NUMBER, PHOTO, PAYMENT_INSTRUCTIONS, TI.STATUS_CODE" +
                                  " FROM TRAINER_INFROMATION TI " +
                                  " left join TRAINING_NATURE TN " +
                                  " on TI.TRAINING_NATURE_ID = TN.TRAINING_NATURE_ID " +
                                  " WHERE TI.TRAINER_ID ='" + trainerId + "' ";

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

        public DataTable getCompetenciesByTrainerId(string trainerId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = @" SELECT 
                                        TC.COMPETENCY_ID, 
                                        TCA.NAME,
                                        TC.DESCRIPTION,
                                        TC.STATUS_CODE,
                                            CASE 
                                                when TC.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "' " +
                                                "when TC.STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                            "end as STATUS_TEXT "+
                                    " FROM TRAINER_COMPETENCY TC "+
                                    " LEFT JOIN TRAINER_COMPETENCY_AREA TCA "+
                                        " ON TC.COMPETENCY_ID = TCA.COMPETENCY_ID "+
                                    " WHERE TRAINER_ID ='" + trainerId + "' ";

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

        //public bool insertTrainer(string nameInitial, string nameFull, byte[] bytes, string nic, string internalExternal, string landline, string mobile, string address, string bank, string branch, string accountNo, string payment, string description, string qualifications, string nature, string status, string addedUserId, DataTable dtCompetencies)
        //{
        //    throw new NotImplementedException();
        //}

        public DataTable getAllTrainingNature()
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = "SELECT TRAINING_NATURE_ID, NAME FROM TRAINING_NATURE";

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

        //Trainser Search

        public DataTable PopulateTrainers(string FullName, string NIC, string MobileNumber, Boolean isExternal)
        {
            DataTable resultTable = new DataTable();
            try
            {
                string sqlQuery = @"
                                        SELECT 
                                            TRAINER_ID,
                                            NAME_WITH_INITIALS,
                                            FULL_NAME,
                                            NIC,
                                            CONTACT_MOBILE,
                                            CONTACT_LAND,
                                            ADDRESS
                                        FROM
                                            TRAINER_INFROMATION
                                        WHERE
                                            STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + @"' 
                                ";

                if (FullName != String.Empty)
                {
                    sqlQuery += " AND FULL_NAME LIKE ('%" + FullName + "%') ";
                }

                if (NIC != String.Empty)
                {
                    sqlQuery += " AND NIC LIKE ('%" + NIC + "%') ";
                }

                if (MobileNumber != String.Empty)
                {
                    sqlQuery += " AND CONTACT_MOBILE LIKE ('%" + MobileNumber + "%') ";
                }

                if (isExternal == true)
                {
                    sqlQuery += " AND IS_EXTERNAL = '" + Constants.TRAINER_EXTERNAL_VALUE + "' ";
                }
                else
                {
                    sqlQuery += " AND IS_EXTERNAL = '" + Constants.TRAINER_INTERNAL_VALUE + "' ";
                }

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
