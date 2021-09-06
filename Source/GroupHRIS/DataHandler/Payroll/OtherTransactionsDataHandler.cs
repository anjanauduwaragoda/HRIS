using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.Payroll
{
    public class OtherTransactionsDataHandler : TemplateDataHandler
    {
        public DataTable GetCategories(string company)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable.Clear();

                string Qry = @"SELECT s.CATEGORY FROM TRANSACTION_CATEGORY AS c , COMPANY_SUBCATEGORY_TYPES AS s WHERE (c.STATUS_CODE='1' AND s.STATUS_CODE='1' AND c.IS_DISPLAY_IN_OTHER_PAYMENTS='1' AND s. COMPANY_ID = '" + company + "') GROUP BY s.CATEGORY;";
                
               // mySqlCmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable GetSubcategories(string category)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string Qry = @"SELECT SUB_CATEGORY FROM TRANSACTION_SUBCATEGORY WHERE CATEGORY = '" + category + "' AND STATUS_CODE='1'";
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

        public DataTable GetTypeId(string company,string category,string subcategory)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                mySqlCmd.Parameters.Add(new MySqlParameter("@company", company));
                mySqlCmd.Parameters.Add(new MySqlParameter("@category", category));
                mySqlCmd.Parameters.Add(new MySqlParameter("@subcategory", subcategory));

                //string Qry = "SELECT SUB_CAT_TYPE_ID FROM COMPANY_SUBCATEGORY_TYPES WHERE (COMPANY_ID =@company AND CATEGORY=@category AND SUB_CATEGORY=@subcategory )";
                string Qry = "SELECT cs.SUB_CAT_TYPE_ID FROM COMPANY_SUBCATEGORY_TYPES AS cs,COMPANY AS c WHERE ( cs.CATEGORY=@category AND cs.SUB_CATEGORY=@subcategory AND cs.COMPANY_ID=c.COMPANY_ID AND c.COMP_NAME=@company)";
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

        public Boolean Insert(string empId,string category,string subcategory,string typeId,string amount,string user)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "sp_InsertOtherTransactions";
                
                mySqlCmd.Parameters.Add(new MySqlParameter("empId",empId));
                mySqlCmd.Parameters.Add(new MySqlParameter("category", category));
                mySqlCmd.Parameters.Add(new MySqlParameter("subcategory", subcategory));
                mySqlCmd.Parameters.Add(new MySqlParameter("typeId", typeId));
                mySqlCmd.Parameters.Add(new MySqlParameter("amount", amount));
                mySqlCmd.Parameters.Add(new MySqlParameter("user", user));
                
                mySqlCmd.CommandText = Qry;
                mySqlCmd.CommandType = CommandType.StoredProcedure;
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

        public Boolean UpdateOtherTransactions(string updateCategory,string updateSubcategory, string empId, string category, string subcategory, string typeId, string amount, string user)
        {
            Boolean Status = false;
            dataTable.Rows.Clear();
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "sp_UpdateOtherTransactions";

                mySqlCmd.Parameters.Add(new MySqlParameter("empId", empId));
                mySqlCmd.Parameters.Add(new MySqlParameter("Tcategory", category));
                mySqlCmd.Parameters.Add(new MySqlParameter("subcategory", subcategory));
                mySqlCmd.Parameters.Add(new MySqlParameter("typeId", typeId));
                mySqlCmd.Parameters.Add(new MySqlParameter("amount", amount));
                mySqlCmd.Parameters.Add(new MySqlParameter("user", user));
                mySqlCmd.Parameters.Add(new MySqlParameter("UpdateCategory", updateCategory));
                mySqlCmd.Parameters.Add(new MySqlParameter("updateSubcategory", updateSubcategory));

                mySqlCmd.CommandText = Qry;
                mySqlCmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable GetEmployeeData(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string Qry = "sp_GetSelectedEmployee";

                mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId));

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

        public DataTable GetEmployeeCompany(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "sp_GetCompanyBasedOnEmployee";

                mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId));

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

        public DataTable GetEmployeeName(string empName)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "sp_GetEmployeeName";

                mySqlCmd.Parameters.Add(new MySqlParameter("EmpName", empName));

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

        public DataTable GetEmployeeEPF(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "sp_GetEmployeeEPF";

                mySqlCmd.Parameters.Add(new MySqlParameter("@empId", empId));

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

    }
}
