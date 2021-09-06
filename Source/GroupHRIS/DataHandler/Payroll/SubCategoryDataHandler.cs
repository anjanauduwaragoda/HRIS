using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.Payroll
{
    public class SubCategoryDataHandler : TemplateDataHandler
    {
        
        public DataTable GetSubCategories()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                //string Qry = @"SELECT SUB_CATEGORY,REMARKS,STATUS_CODE,CATEGORY FROM TRANSACTION_SUBCATEGORY;";
                //string Qry = "SELECT SUB_CATEGORY,REMARKS, " +
                //                " case " +
                //                " when  STATUS_CODE ='1' then 'Active' " +
                //                " when  STATUS_CODE = '0' then 'Inactive' " +
                //                " End as STATUS_CODE,CATEGORY " +
                //                " FROM TRANSACTION_SUBCATEGORY";

                string Qry = "sp_GetAllSubcategories";
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

        public DataTable GetCategories()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable.Clear();

                //string Qry = @"SELECT CATEGORY FROM TRANSACTION_CATEGORY WHERE STATUS_CODE='1';";
                string Qry = "sp_GetSubcategoryDDL";
                mySqlCmd.CommandType = CommandType.StoredProcedure;
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

        public Boolean InsertSubCategory(string SubCategoryName, string Remarks, string status, string user,  string category)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string Qry = "sp_InsertSubcategories";
                mySqlCmd.CommandType = CommandType.StoredProcedure;

                mySqlCmd.Parameters.Add(new MySqlParameter("SubCategoryName", SubCategoryName));
                mySqlCmd.Parameters.Add(new MySqlParameter("Remarks", Remarks));
                mySqlCmd.Parameters.Add(new MySqlParameter("statusCode", status));
                mySqlCmd.Parameters.Add(new MySqlParameter("user", user));
                mySqlCmd.Parameters.Add(new MySqlParameter("category", category));

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

        public Boolean UpdateSubCategory(string subcategoryUpdate,string SubCategoryName, string Remarks, string status, string user, string Category)
        {
            Boolean Status = false;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                //string Qry = @"UPDATE TRANSACTION_SUBCATEGORY  SET REMARKS=@Remarks,STATUS_CODE=@Remarks,MODIFIED_BY=@user,MODIFIED_DATE=@date WHERE (CATEGORY=@Category AND SUB_CATEGORY=@SubCategoryName);";

                string subcategoryStatus = getStatus(subcategoryUpdate);

                if (subcategoryStatus == "1" || subcategoryStatus != status)
                {
                    string Qry = "sp_UpdateSubcategory";
                    mySqlCmd.CommandType = CommandType.StoredProcedure;
                    mySqlCmd.Parameters.Add(new MySqlParameter("subcategoryUpdate", subcategoryUpdate));
                    mySqlCmd.Parameters.Add(new MySqlParameter("SubCategoryName", SubCategoryName));
                    mySqlCmd.Parameters.Add(new MySqlParameter("Remarks", Remarks));
                    mySqlCmd.Parameters.Add(new MySqlParameter("statusCode", status));
                    mySqlCmd.Parameters.Add(new MySqlParameter("user", user));
                    mySqlCmd.Parameters.Add(new MySqlParameter("Category", Category));
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

        public String getStatus(string subcategory)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT STATUS_CODE FROM TRANSACTION_SUBCATEGORY WHERE SUB_CATEGORY = '" + subcategory + "';";

                MySqlCommand cmd = new MySqlCommand(Qry, mySqlCon);
                String rdr = Convert.ToString(cmd.ExecuteScalar());

                return rdr;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public DataTable GetSubcategoryGrid(string category)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable.Clear();

                string Qry = @"SELECT SUB_CATEGORY,REMARKS,CASE WHEN STATUS_CODE = '1' THEN 'Active'
                                                                WHEN STATUS_CODE = '0' THEN 'Inactive'
                                                            END AS STATUS_CODE,CATEGORY 
                                FROM TRANSACTION_SUBCATEGORY 
                                WHERE CATEGORY = '" + category + "';";
                
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
