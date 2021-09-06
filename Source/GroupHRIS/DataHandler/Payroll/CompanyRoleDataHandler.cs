using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.Payroll
{
    public class CompanyRoleDataHandler : TemplateDataHandler
    {

        public DataTable getCompanyIdCompName()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT COMP_NAME,COMPANY_ID FROM COMPANY WHERE COMPANY_ID = 'CP03' OR COMPANY_ID = 'CP64' order by COMP_NAME";

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

        public DataTable getCompanyOTCategory(string companyId)
        {
            try
            {
                dataTable.Rows.Clear();
                string Qry = "SELECT OT_CATEGORY_NAME,COMPANY_OT_CATEGORY_NAME FROM COMPANY_OT_CATEGORIES WHERE COMPANY_ID='" + companyId + "'";
                MySqlDataAdapter data = new MySqlDataAdapter(Qry,mySqlCon);
                data.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public DataTable getRole()
        {
            try
            {
                dataTable.Rows.Clear();
                string Qry = "SELECT ROLE_ID,ROLE_NAME FROM EMPLOYEE_ROLE";
                MySqlDataAdapter data = new MySqlDataAdapter(Qry,mySqlCon);
                data.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Boolean InsertRole(string company , string category,string role,string active,string user)
        {
            Boolean Status = false;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string Qry = "sp_InsertCompanyRole";
                
                mySqlCmd.Parameters.Add(new MySqlParameter("company", company));
                mySqlCmd.Parameters.Add(new MySqlParameter("category", category));
                mySqlCmd.Parameters.Add(new MySqlParameter("role", role));
                mySqlCmd.Parameters.Add(new MySqlParameter("active", active));
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

        public Boolean UpdateRole(string company, string category, string role, string active, string newCategory, string newRole, string user)
        {
            Boolean Status = false;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string activeStatus = getStatus(company, category, role);

                if (activeStatus == "1" || activeStatus != active)
                {
                    string Qry = "sp_UpdateCompanyRole";
                    mySqlCmd.CommandType = CommandType.StoredProcedure;
                    mySqlCmd.Parameters.Add(new MySqlParameter("company", company));
                    mySqlCmd.Parameters.Add(new MySqlParameter("OTcategory", category));
                    mySqlCmd.Parameters.Add(new MySqlParameter("role", role));
                    mySqlCmd.Parameters.Add(new MySqlParameter("active", active));
                    mySqlCmd.Parameters.Add(new MySqlParameter("newCategory", newCategory));
                    mySqlCmd.Parameters.Add(new MySqlParameter("newRole", newRole)); //user
                    mySqlCmd.Parameters.Add(new MySqlParameter("user", user));
                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();
                    Status = true;
                }
                else
                {
                    Status = true;
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

        public String getStatus(string company, string category, string role)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT STATUS_CODE FROM EMPLOYEE_ROLE_CATEGORY WHERE COMPANY_ID='" + company + "' AND OT_CATEGORY_NAME = '" + category + "' AND ROLE_ID = '" + role + "'";

                MySqlCommand cmd = new MySqlCommand(Qry, mySqlCon);
                String rdr = Convert.ToString(cmd.ExecuteScalar());

                return rdr;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }


        public DataTable GetAll(string company)
        {
            try
            {
                dataTable.Rows.Clear();
                
                string Qry = "sp_GetAllRolesByCompany";

                mySqlCmd.Parameters.Add(new MySqlParameter("company", company));
                mySqlCmd.CommandText = Qry;
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Boolean IsExist(string company , string otCategory , string role)
        {
            Boolean Status = false;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                
                string Qry = "sp_IsCompanyRoleExist";
                mySqlCmd.Parameters.Add(new MySqlParameter("company", company));
                mySqlCmd.Parameters.Add(new MySqlParameter("otCategory", otCategory));
                mySqlCmd.Parameters.Add(new MySqlParameter("role", role));
                
                mySqlCmd.CommandText = Qry;
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                string value = mySqlCmd.ExecuteScalar().ToString();

                if (value == "0")
                {
                    Status = true;
                }
                else if (value == "1")
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

    }
}
