using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainingInstituteDataHandler:TemplateDataHandler
    {
        public DataTable getAllBanks()
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                string sqlQuery = "SELECT BANK_ID, BANK_NAME FROM BANK";

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
                mySqlCon.Open();
                string sqlQuery = "SELECT BRANCH_ID, BRANCH_NAME FROM BANK_BRANCH WHERE BANK_ID ='" + bankId + "' ";

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

        public Boolean Insert(string name, string address, string contact_1, string contact_2, string email, string bank, string branch, string account, string paymentInstructions, string status, string addedUserId)
        {

            Boolean inserted = false;
            MySqlTransaction mySqlTrans = null;
            string mySqlQuery = null;

            try {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler serialCode = new SerialHandler();
                string instituteId = serialCode.getserila(mySqlCon, "TI");

                mySqlCmd.Parameters.Add(new MySqlParameter("@instituteId", instituteId.Trim() == "" ? (object)DBNull.Value : instituteId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@name", name.Trim() == "" ? (object)DBNull.Value : name.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@address", address.Trim() == "" ? (object)DBNull.Value : address.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@contact_1", contact_1.Trim() == "" ? (object)DBNull.Value : contact_1.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@contact_2", contact_2.Trim() == "" ? (object)DBNull.Value : contact_2.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@email", email.Trim() == "" ? (object)DBNull.Value : email.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@bank", bank.Trim() == "" ? (object)DBNull.Value : bank.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@branch", branch.Trim() == "" ? (object)DBNull.Value : branch.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@account", account.Trim() == "" ? (object)DBNull.Value : account.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@paymentInstructions", paymentInstructions.Trim() == "" ? (object)DBNull.Value : paymentInstructions.Trim()));

                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));


                mySqlQuery = "insert into TRAINING_INSTITUTES (INSTITUTE_ID, INSTITUTE_NAME, INSTITUTE_ADDRESS, CONTACT_NO_1, CONTACT_NO_2, EMAIL_ADDRESS, BANK_ID, BANK_BRANCH_ID, ACCOUNT_NUMBER, PAYMENT_INSTRUCTIONS, STATUS_CODE, ADDED_BY, ADDED_DATE, MODIFIED_BY, MODIFIED_DATE)" +
                                                    " values (@instituteId,@name,@address,@contact_1,@contact_2,@email,@bank,@branch,@account,@paymentInstructions,@status,@addedUserId,now(),@addedUserId,now())";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = mySqlQuery;
                mySqlCmd.ExecuteNonQuery();
                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                inserted = true;
            }
            catch (Exception e) {
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

        public Boolean Update(string instituteId, string name, string address, string contact_1, string contact_2, string email, string bank, string branch, string account, string paymentInstructions, string status, string addedUserId)
        {
            Boolean isUpdated = false;
            MySqlTransaction mySqlTrans = null;
            string mySqlQuery = null;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@instituteId", instituteId.Trim() == "" ? (object)DBNull.Value : instituteId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@name", name.Trim() == "" ? (object)DBNull.Value : name.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@address", address.Trim() == "" ? (object)DBNull.Value : address.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@contact_1", contact_1.Trim() == "" ? (object)DBNull.Value : contact_1.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@contact_2", contact_2.Trim() == "" ? (object)DBNull.Value : contact_2.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@email", email.Trim() == "" ? (object)DBNull.Value : email.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@bank", bank.Trim() == "" ? (object)DBNull.Value : bank.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@branch", branch.Trim() == "" ? (object)DBNull.Value : branch.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@account", account.Trim() == "" ? (object)DBNull.Value : account.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@paymentInstructions", paymentInstructions.Trim() == "" ? (object)DBNull.Value : paymentInstructions.Trim()));

                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

               // INSTITUTE_ID, INSTITUTE_NAME, INSTITUTE_ADDRESS, CONTACT_NO_1, CONTACT_NO_2, EMAIL_ADDRESS, BANK_ID, BANK_BRANCH_ID, ACCOUNT_NUMBER, PAYMENT_INSTRUCTIONS, STATUS_CODE, ADDED_BY, ADDED_DATE, MODIFIED_BY, MODIFIED_DATE
                mySqlQuery = "UPDATE TRAINING_INSTITUTES SET " +

                    "INSTITUTE_NAME = @name, " +
                    "INSTITUTE_ADDRESS = @address, " +
                    "CONTACT_NO_1 = @contact_1, " +
                    "CONTACT_NO_2 = @contact_2, " +
                    "EMAIL_ADDRESS = @email, " +
                    "BANK_ID = @bank, " +
                    "BANK_BRANCH_ID = @branch, " +
                    "ACCOUNT_NUMBER = @account, " +
                    "PAYMENT_INSTRUCTIONS = @paymentInstructions, " +
                    "STATUS_CODE = @status, " +
                    "MODIFIED_BY = @addedUserId, " +
                    "MODIFIED_DATE = now() " +
                    "WHERE INSTITUTE_ID = @instituteId";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = mySqlQuery;
                mySqlCmd.ExecuteNonQuery();
                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isUpdated = true;

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
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }

            return isUpdated;
        }
        public DataTable getAllInstitutes()
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                string sqlQuery = "SELECT INSTITUTE_ID, INSTITUTE_NAME, CONTACT_NO_1, EMAIL_ADDRESS, " +
                                     " CASE " +
                                     " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then 'Inactive' " +
                                     " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then 'Active' " +
                                     " End  as STATUS_CODE " +
                                     " FROM TRAINING_INSTITUTES "+
                                     " ORDER BY INSTITUTE_NAME ASC";

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

        public DataRow getInstituteById(string instituteId)
        {
            try
            {

                mySqlCon.Open();
                
                dataTable.Rows.Clear();
                string queryString = "select INSTITUTE_ID, INSTITUTE_NAME, INSTITUTE_ADDRESS, CONTACT_NO_1, CONTACT_NO_2, EMAIL_ADDRESS, BANK_ID, BANK_BRANCH_ID, ACCOUNT_NUMBER, PAYMENT_INSTRUCTIONS, STATUS_CODE from TRAINING_INSTITUTES  where INSTITUTE_ID = '" + instituteId + "' ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(queryString, mySqlCon);
                mySqlDa.Fill(dataTable);

                DataRow dataRow = null;

                if (dataTable.Rows.Count > 0)
                {
                    dataRow = dataTable.Rows[0];
                }
                return dataRow;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }
    }
}
