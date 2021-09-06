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
    public class BankBranchDataHandler:TemplateDataHandler
    {
        // Anjana Uduwaragoda
        // Used in webFrmEmployeeBankAccount
        public DataTable populate(string bankId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT BRANCH_ID,BRANCH_NAME FROM BANK_BRANCH where BANK_ID='" + bankId.Trim() + "' order by BRANCH_NAME";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
        
        public DataTable GetBankBranches()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT BB.BRANCH_ID,BB.BRANCH_NAME,B.BANK_NAME,BB.ADDRESS,BB.LAND_PHONE1,BB.LAND_PHONE2 
                                FROM BANK_BRANCH BB,BANK B
                                WHERE B.BANK_ID=BB.BANK_ID;";
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

        public DataTable GetBankBranches(string BankID)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT   BB.BRANCH_ID,BB.BRANCH_NAME,B.BANK_NAME,BB.ADDRESS,BB.LAND_PHONE1,BB.LAND_PHONE2 
                                FROM    BANK_BRANCH BB,BANK B
                                WHERE   B.BANK_ID=@BankID AND B.BANK_ID=BB.BANK_ID
                                ORDER   BY BRANCH_NAME ASC;";
                mySqlCmd.Parameters.Add(new MySqlParameter("@BankID", BankID));
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

        public Boolean InsertBankBranch(string BranchID, string BankID, string BranchName, string Address, string LandPhone1, string LandPhone2)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"INSERT INTO BANK_BRANCH(BRANCH_ID,BANK_ID,BRANCH_NAME,ADDRESS,LAND_PHONE1,LAND_PHONE2)
                                VALUES(@BranchID,@BankID,@BranchName,@Address,@LandPhone1,@LandPhone2);";
                mySqlCmd.Parameters.Add(new MySqlParameter("@BranchID", BranchID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@BankID", BankID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@BranchName", BranchName));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Address", Address));
                mySqlCmd.Parameters.Add(new MySqlParameter("@LandPhone1", LandPhone1));
                mySqlCmd.Parameters.Add(new MySqlParameter("@LandPhone2", LandPhone2));
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

        public Boolean UpdateBankBranch(string BankID, string BranchName, string Address, string LandPhone1, string LandPhone2, string BranchID)
        {
            Boolean Status = false;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"UPDATE BANK_BRANCH
                                SET BRANCH_NAME=@BranchName,ADDRESS=@Address,LAND_PHONE1=@LandPhone1,LAND_PHONE2=@LandPhone2
                                WHERE BRANCH_ID=@BranchID;";

                mySqlCmd.Parameters.Add(new MySqlParameter("@BankID", BankID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@BranchName", BranchName));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Address", Address));
                mySqlCmd.Parameters.Add(new MySqlParameter("@LandPhone1", LandPhone1));
                mySqlCmd.Parameters.Add(new MySqlParameter("@LandPhone2", LandPhone2));
                mySqlCmd.Parameters.Add(new MySqlParameter("@BranchID", BranchID));

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

        public DataTable SearchBankBranch()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT BB.BRANCH_ID,BB.BRANCH_NAME,B.BANK_NAME,BB.ADDRESS,BB.LAND_PHONE1,BB.LAND_PHONE2 
                                FROM BANK_BRANCH BB,BANK B
                                WHERE B.BANK_ID=BB.BANK_ID;";

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

        public DataTable PopulateBanks()
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

        public Boolean CheckRecords(string BankName, string BranchID)
        {
            try
            {
                Boolean State = false;
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT * 
                                FROM  BANK_BRANCH BB,BANK B
                                WHERE B.BANK_NAME = @BankName AND BB.BRANCH_ID = @BranchID AND BB.BANK_ID = B.BANK_ID;";

                mySqlCmd.Parameters.Add(new MySqlParameter("@BankName", BankName));
                mySqlCmd.Parameters.Add(new MySqlParameter("@BranchID", BranchID));
                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    State = true;
                }

                return State;
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

        public Boolean isBanBranchkExist(string bankName,string branchName)
        {
            Boolean isExist = false;
            dataTable.Rows.Clear();
            try
            {

                string qry = "SELECT BRANCH_NAME FROM BANK_BRANCH WHERE BANK_ID = '" + bankName + "' AND BRANCH_NAME = '" + branchName + "';";
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
