using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.Payroll
{
    public class CategoryDataHandler : TemplateDataHandler
    {
        public DataTable GetCategories()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "sp_GetAllTransactionCategory";
                mySqlCmd.CommandText = Qry;
                mySqlCmd.CommandType = CommandType.StoredProcedure;
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

        public Boolean InsertCategory(string CategoryName, string Remarks, string status, string user, string chk)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                //string Qry = @"INSERT INTO TRANSACTION_CATEGORY(CATEGORY,REMARKS,STATUS_CODE,ADDED_BY,ADDED_DATE) VALUES(@CategoryName,@Remarks,@status,@user,@Addeddate)";
                string Qry = "sp_InsertCategory";
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.Add(new MySqlParameter("CategoryName", CategoryName));
                mySqlCmd.Parameters.Add(new MySqlParameter("Remarks", Remarks));
                mySqlCmd.Parameters.Add(new MySqlParameter("statusCode", status));
                mySqlCmd.Parameters.Add(new MySqlParameter("user", user));
                mySqlCmd.Parameters.Add(new MySqlParameter("chk", chk));

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

        public Boolean UpdateCategory(string categoryUpdate,string CategoryName, string Remarks,string status,string user,string chk)
        {
            Boolean Status = false;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string CategoryStatus = getStatus(categoryUpdate);
                //string Qry = @"UPDATE TRANSACTION_CATEGORY SET REMARKS=@Remarks,STATUS_CODE=@status,MODIFIED_BY=@user,MODIFIED_DATE=@Addeddate WHERE CATEGORY=@CategoryName;";
                if (CategoryStatus == "1" || status != CategoryStatus)
                {
                    string Qry = "sp_UpdateCategory";
                    mySqlCmd.CommandType = CommandType.StoredProcedure;
                    mySqlCmd.Parameters.Add(new MySqlParameter("categoryUpdate", categoryUpdate));
                    mySqlCmd.Parameters.Add(new MySqlParameter("CategoryName", CategoryName));
                    mySqlCmd.Parameters.Add(new MySqlParameter("Remarks", Remarks));
                    mySqlCmd.Parameters.Add(new MySqlParameter("statusCode", status));
                    mySqlCmd.Parameters.Add(new MySqlParameter("user", user));
                    mySqlCmd.Parameters.Add(new MySqlParameter("chk", chk));
                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();
                    Status = true;
                }
                else
                {
                    Status = false;
                }
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

        public String getStatus(string category)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT STATUS_CODE FROM TRANSACTION_CATEGORY WHERE CATEGORY = '" + category + "';";

                MySqlCommand cmd = new MySqlCommand(Qry, mySqlCon);
                String rdr = Convert.ToString(cmd.ExecuteScalar());

                return rdr;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }
    }
}
