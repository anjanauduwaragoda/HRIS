using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.MetaData
{
    public class BankDataHandler:TemplateDataHandler
    {
        // Anjana Uduwaragoda
        // Used in webFrmEmployeeBankAccount
        public DataTable populate()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT BANK_ID,BANK_NAME FROM BANK order by BANK_NAME;";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public DataTable GetBanks()
        {
            try
            {
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

        public Boolean InsertBank(string BankID, string BankName)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"INSERT INTO BANK(BANK_ID,BANK_NAME) VALUES(@BankID,@BankName);";
                mySqlCmd.Parameters.Add(new MySqlParameter("@BankID", BankID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@BankName", BankName));
                mySqlCmd.CommandText = Qry;
                mySqlCmd.ExecuteNonQuery();
                Status = true;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return Status;
        }

        public Boolean UpdateBank(string BankName, string BankID)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"UPDATE BANK SET BANK_NAME=@BankName WHERE BANK_ID=@BankID;";
                mySqlCmd.Parameters.Add(new MySqlParameter("@BankName", BankName));
                mySqlCmd.Parameters.Add(new MySqlParameter("@BankID", BankID));
                mySqlCmd.CommandText = Qry;
                mySqlCmd.ExecuteNonQuery();
                Status = true;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return Status;
        }

        public DataTable SearchBankByBankIDOrBankName(string Keyword)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT BANK_ID,BANK_NAME
                                FROM BANK
                                WHERE BANK_NAME LIKE ('%" + Keyword + "%') OR BANK_ID LIKE ('%" + Keyword + "%');";

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

        public Boolean isBankExist(string bankName)
        {
            Boolean isExist = false;
            dataTable.Rows.Clear();
            try
            {

                string qry = "SELECT UPPER(REPLACE(BANK_NAME,' ','')) FROM BANK WHERE UPPER(REPLACE(BANK_NAME,' ','')) = '" + bankName + "';";
                MySqlDataAdapter mySqlData = new MySqlDataAdapter(qry, mySqlCon);
                mySqlData.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExist = true;
                }
                return isExist;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
