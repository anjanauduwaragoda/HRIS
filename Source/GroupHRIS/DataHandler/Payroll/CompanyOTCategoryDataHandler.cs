using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Payroll
{
    public class CompanyOTCategoryDataHandler : TemplateDataHandler
    {
        public DataTable Populate(string company)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Close();
                }

                string Qry = "sp_PopulateCompanySubcategoryData";
                mySqlCmd.Parameters.Add(new MySqlParameter("company", company));
                mySqlCmd.CommandText = Qry;
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable Populate()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Close();
                }

                string Qry = "sp_GetAllCompanySubcategoryType";
                mySqlCmd.CommandText = Qry;
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable getCompanyIdCompName()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT COMP_NAME,COMPANY_ID FROM COMPANY order by COMP_NAME";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getCompanyIdCompName(string companyId)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT COMP_NAME,COMPANY_ID FROM COMPANY where COMPANY_ID ='" + companyId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean InsertCompanyOTCategory(string company, string oTCategory, string description,string user,string active)
        {
            Boolean status = false;
            SerialHandler serialHandler = new SerialHandler();

            string otCategoryId = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Clear();

            mySqlCmd.Parameters.Add(new MySqlParameter("@company", company.Trim() == "" ? (object)DBNull.Value : company.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@oTCategory", oTCategory.Trim() == "" ? (object)DBNull.Value : oTCategory.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@active", active.Trim() == "" ? (object)DBNull.Value : active.Trim()));
            
            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                otCategoryId = serialHandler.getserila(mySqlCon, Constants.COMPANY_OT_CATEGORY);

                mySqlCmd.Parameters.Add(new MySqlParameter("@otCategoryId", otCategoryId.Trim() == "" ? (object)DBNull.Value : otCategoryId.Trim()));

                string Qry = "INSERT INTO COMPANY_OT_CATEGORIES(ROLE_CATEGORY_ID,COMPANY_OT_CATEGORY_NAME,COMPANY_ID,DESCRIPTION,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE)" +
                                " VALUES(@otCategoryId,@oTCategory,@company,@description,@active,@user,NOW(),@user,NOW())";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = Qry;

                mySqlCmd.ExecuteNonQuery();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                //mySqlTrans.Commit();
                status = true;
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
                serialHandler = null;
                mySqlCon.Close();
            }
            return status;
        }

        public Boolean UpdateCompanyOTCategory(string company, string oTCategory, string description, string user, string active, string id)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string Qry = "sp_UpdateCompanyOTCategory";
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.Add(new MySqlParameter("company", company));
                mySqlCmd.Parameters.Add(new MySqlParameter("otCategory", oTCategory));
                mySqlCmd.Parameters.Add(new MySqlParameter("description", description));
                mySqlCmd.Parameters.Add(new MySqlParameter("user", user));
                mySqlCmd.Parameters.Add(new MySqlParameter("actice", active));

                mySqlCmd.Parameters.Add(new MySqlParameter("id", id));

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

        public DataTable LoadDataGrid(string company)
        {
            try
            {
                dataTable.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
               
                string Qry = "sp_getAllOTCategory";

                mySqlCmd.Parameters.Add(new MySqlParameter("company", company));
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

        public DataTable LoadDataGrid()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string Qry = @"SELECT COMPANY_OT_CATEGORY_NAME,DESCRIPTION,COMPANY_ID,ROLE_CATEGORY_ID,
                                                                CASE WHEN STATUS_CODE='1' THEN 'Active'
		                                                             WHEN STATUS_CODE='0' THEN 'Inactive' 
	                                                            END AS STATUS_CODE FROM COMPANY_OT_CATEGORIES";
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
