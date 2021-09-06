using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainingLocationDataHandler : TemplateDataHandler
    {
        public DataTable getBank()
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT BANK_ID,BANK_NAME FROM BANK;";
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

        public DataTable getBankBranch(string bankId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT BRANCH_ID,BRANCH_NAME FROM BANK_BRANCH WHERE BANK_ID='" + bankId + "';";
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

        public bool Insert(string locationName,string address,string contact1,string contact2,string email,string capacity,string description,string bank,string branch,string account,string paymentIns,string status,string user,string pro,string dis)
        {
            bool isInsert = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler nserialcode = new SerialHandler();
                string sLOCATION_ID = nserialcode.getserila(mySqlCon, "L");

                mySqlCmd.Parameters.Add(new MySqlParameter("@sLOCATION_ID", sLOCATION_ID.Trim() == "" ? (object)DBNull.Value : sLOCATION_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@locationName", locationName.Trim() == "" ? (object)DBNull.Value : locationName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@address", address.Trim() == "" ? (object)DBNull.Value : address.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@contact1", contact1.Trim() == "" ? (object)DBNull.Value : contact1.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@contact2", contact2.Trim() == "" ? (object)DBNull.Value : contact2.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@email", email.Trim() == "" ? (object)DBNull.Value : email.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@capacity", capacity.Trim() == "" ? (object)DBNull.Value : capacity.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@bank", bank.Trim() == "" ? (object)DBNull.Value : bank.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@branch", branch.Trim() == "" ? (object)DBNull.Value : branch.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@account", account.Trim() == "" ? (object)DBNull.Value : account.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@paymentIns", paymentIns.Trim() == "" ? (object)DBNull.Value : paymentIns.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@pro", pro.Trim() == "" ? (object)DBNull.Value : pro.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@dis", dis.Trim() == "" ? (object)DBNull.Value : dis.Trim()));

                sMySqlString = @"INSERT INTO TRAINING_LOCATIONS (LOCATION_ID,LOCATION_NAME,ADDRESS,PROVINCE_ID,DISTRICT_ID,CONTACT_NO_1,CONTACT_NO_2,EMAIL,CAPACITY,DESCRIPTION,BANK_ID,BANK_BRANCH_ID,ACCOUNT_NUMBER,PAYMENT_INSTRUCTIONS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
						               VALUES(@sLOCATION_ID,@locationName,@address,@pro,@dis,@contact1,@contact2,@email,@capacity,@description,@bank,@branch,@account,@paymentIns,@status,@user,now(),@user,now());";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isInsert = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return isInsert;
        }

        public bool Update(string locationId,string locationName, string address, string contact1, string contact2, string email, string capacity, string description, string bank, string branch, string account, string paymentIns, string status, string user,string pro,string dis)
        {
            bool isUpdate = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Parameters.Add(new MySqlParameter("@locationId", locationId.Trim() == "" ? (object)DBNull.Value : locationId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@locationName", locationName.Trim() == "" ? (object)DBNull.Value : locationName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@address", address.Trim() == "" ? (object)DBNull.Value : address.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@contact1", contact1.Trim() == "" ? (object)DBNull.Value : contact1.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@contact2", contact2.Trim() == "" ? (object)DBNull.Value : contact2.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@email", email.Trim() == "" ? (object)DBNull.Value : email.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@capacity", capacity.Trim() == "" ? (object)DBNull.Value : capacity.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@bank", bank.Trim() == "" ? (object)DBNull.Value : bank.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@branch", branch.Trim() == "" ? (object)DBNull.Value : branch.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@account", account.Trim() == "" ? (object)DBNull.Value : account.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@paymentIns", paymentIns.Trim() == "" ? (object)DBNull.Value : paymentIns.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@pro", pro.Trim() == "" ? (object)DBNull.Value : pro.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@dis", dis.Trim() == "" ? (object)DBNull.Value : dis.Trim()));

                sMySqlString = @"UPDATE TRAINING_LOCATIONS SET LOCATION_NAME= @locationName,
                                    ADDRESS = @address,
                                    PROVINCE_ID = @pro,
                                    DISTRICT_ID = @dis,
                                    CONTACT_NO_1 = @contact1,
                                    CONTACT_NO_2 = @contact2,
                                    EMAIL = @email,
                                    CAPACITY = @capacity,
                                    DESCRIPTION = @description,
                                    BANK_ID = @bank,
                                    BANK_BRANCH_ID = @branch,
                                    ACCOUNT_NUMBER = @account,
                                    PAYMENT_INSTRUCTIONS = @paymentIns,
                                    STATUS_CODE = @status,
                                    MODIFIED_BY = @user,
                                    MODIFIED_DATE = now()
                                    WHERE LOCATION_ID = @locationId ;";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isUpdate = true;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }

            return isUpdate;
        }

        public DataTable getAllLocation()
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    l.LOCATION_ID,
                                    l.LOCATION_NAME,
                                    l.ADDRESS,
                                    l.CONTACT_NO_1,
                                    l.EMAIL,
                                    l.CAPACITY,
                                    b.BANK_NAME,
                                    bb.BRANCH_NAME,
                                    p.PROVINCE_NAME,
                                    d.DISTRICT_NAME,
                                    CASE WHEN l.STATUS_CODE = '1' THEN 'Active'
                                        WHEN l.STATUS_CODE = '0' THEN 'Inactive'
                                    END AS STATUS_CODE
                                FROM
                                    TRAINING_LOCATIONS l,
                                    BANK b,
                                    BANK_BRANCH bb,
                                    PROVINCE p,
                                    DISTRICT d
                                WHERE
                                        l.BANK_ID = b.BANK_ID
                                        AND bb.BRANCH_ID = l.BANK_BRANCH_ID
                                        AND bb.BANK_ID = l.BANK_ID
                                        AND p.PROVINCE_ID = d.PROVINCE_ID
                                        AND p.PROVINCE_ID = l.PROVINCE_ID
                                        AND d.DISTRICT_ID = l.DISTRICT_ID
                                        AND p.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"'
                                        AND d.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "' ORDER BY l.LOCATION_NAME;";

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

        public DataTable getSelectedLocation(string locationId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    LOCATION_NAME,
                                    ADDRESS,
                                    CONTACT_NO_1,
                                    CONTACT_NO_2,
                                    EMAIL,
                                    CAPACITY,
                                    DESCRIPTION,
                                    BANK_ID,
                                    BANK_BRANCH_ID,
                                    ACCOUNT_NUMBER,
                                    PAYMENT_INSTRUCTIONS,
                                    STATUS_CODE,PROVINCE_ID,DISTRICT_ID
                                FROM
                                    TRAINING_LOCATIONS
                                WHERE
                                    LOCATION_ID = '" + locationId + "';";

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

        public DataTable getProvince()
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT PROVINCE_ID,PROVINCE_NAME FROM PROVINCE WHERE STATUS_CODE='" + Constants.CON_ACTIVE_STATUS + "';";
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

        public DataTable getDistrict(string proId)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT DISTRICT_ID,DISTRICT_NAME FROM DISTRICT WHERE STATUS_CODE='" + Constants.CON_ACTIVE_STATUS + "' AND PROVINCE_ID = '" + proId + "'";
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

        public DataTable filterTrainingLocation(string province, string district)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    l.LOCATION_ID,
                                    l.LOCATION_NAME,
                                    l.ADDRESS,
                                    l.CONTACT_NO_1,
                                    l.EMAIL,
                                    l.CAPACITY,
                                    b.BANK_NAME,
                                    bb.BRANCH_NAME,
                                    p.PROVINCE_NAME,
                                    d.DISTRICT_NAME,
                                    CASE WHEN l.STATUS_CODE = '1' THEN 'Active'
                                        WHEN l.STATUS_CODE = '0' THEN 'Inactive'
                                    END AS STATUS_CODE
                                FROM
                                    TRAINING_LOCATIONS l,
                                    BANK b,
                                    BANK_BRANCH bb,
                                    PROVINCE p,
                                    DISTRICT d
                                WHERE
                                    l.BANK_ID = b.BANK_ID
                                        AND bb.BRANCH_ID = l.BANK_BRANCH_ID
                                        AND bb.BANK_ID = l.BANK_ID
                                        AND p.PROVINCE_ID = d.PROVINCE_ID
                                        AND p.PROVINCE_ID = l.PROVINCE_ID
                                        AND d.DISTRICT_ID = l.DISTRICT_ID
                                        AND p.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"'
                                        AND d.STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "' ";

                if (province != String.Empty)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@province", province));
                    Qry += @" AND l.PROVINCE_ID = @province ";
                }

                if (district != String.Empty)
                {
                    mySqlCmd.Parameters.Add(new MySqlParameter("@district", district));
                    Qry += @" AND l.DISTRICT_ID = @district ";
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
