using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.Payroll
{
    public class CompanySubCategoryTypeDataHandler : TemplateDataHandler
    {
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

        public DataTable GetSubcategories(string category)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                //dataTable.Clear();
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

        public DataTable Populate(string company)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                //string Qry = @"SELECT c.COMP_NAME, " +
                //                       " t.CATEGORY, " +
                //                       " t.SUB_CATEGORY, " +
                //                       " t.SUB_CAT_TYPE_ID, " +
                //                       " case " +
                //                            " when  t.STATUS_CODE ='1' then 'Active' " +
                //                            " when  t.STATUS_CODE = '0' then 'Inactive' " +
                //                            " End as STATUS_CODE " +
                //                " FROM COMPANY_SUBCATEGORY_TYPES t " +
                //                " INNER JOIN COMPANY c ON t.COMPANY_ID = c.COMPANY_ID ";

                //string Qry = "sp_GetAllCompanySubcategoryType";

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
                //string Qry = @"SELECT c.COMP_NAME, " +
                //                       " t.CATEGORY, " +
                //                       " t.SUB_CATEGORY, " +
                //                       " t.SUB_CAT_TYPE_ID, " +
                //                       " case " +
                //                            " when  t.STATUS_CODE ='1' then 'Active' " +
                //                            " when  t.STATUS_CODE = '0' then 'Inactive' " +
                //                            " End as STATUS_CODE " +
                //                " FROM COMPANY_SUBCATEGORY_TYPES t " +
                //                " INNER JOIN COMPANY c ON t.COMPANY_ID = c.COMPANY_ID ";

                //string Qry = "sp_GetAllCompanySubcategoryType";

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


        public Boolean InsertSubCategory(string id, string category, string subcategory, string typeid, string status, string addedBy)
        {
            Boolean Status = false;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                //string Qry = @"INSERT INTO COMPANY_SUBCATEGORY_TYPES(COMPANY_ID,CATEGORY,SUB_CATEGORY,SUB_CAT_TYPE_ID,STATUS_CODE,ADDED_BY,ADDED_DATE)" +
                //                " VALUES(@id,@category,@subcategory,@typeid,@status,@addedBy,@addedDate) ";

                string Qry = "sp_InsertCompanySubcategoryTypes";
                mySqlCmd.Parameters.Add(new MySqlParameter("id", id));
                mySqlCmd.Parameters.Add(new MySqlParameter("category", category));
                mySqlCmd.Parameters.Add(new MySqlParameter("subcategory", subcategory));
                mySqlCmd.Parameters.Add(new MySqlParameter("typeid", typeid));
                mySqlCmd.Parameters.Add(new MySqlParameter("statusCode", status));
                mySqlCmd.Parameters.Add(new MySqlParameter("addedBy", addedBy));

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

        public Boolean UpdateSubcatogoryTypes(string id, string category, string subcategory, string typeid, string status, string addedBy)
        {
            Boolean Status = false;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string subcategoryStatus = getStatus(id, category, subcategory);

                if (subcategoryStatus == "1" || subcategoryStatus != status)
                {
                    string Qry = "sp_UpdateCompanySubcategoryTypes";
                    mySqlCmd.Parameters.Add(new MySqlParameter("id", id));
                    mySqlCmd.Parameters.Add(new MySqlParameter("category", category));
                    mySqlCmd.Parameters.Add(new MySqlParameter("subcategory", subcategory));
                    mySqlCmd.Parameters.Add(new MySqlParameter("typeid", typeid));
                    mySqlCmd.Parameters.Add(new MySqlParameter("statusCode", status));
                    mySqlCmd.Parameters.Add(new MySqlParameter("addedBy", addedBy));
                    //mySqlCmd.Parameters.Add(new MySqlParameter("addedDate", addedDate));
                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.CommandType = CommandType.StoredProcedure;
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

        public String getStatus(string company,string category,string subcategory)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT STATUS_CODE FROM COMPANY_SUBCATEGORY_TYPES WHERE COMPANY_ID = '" + company + "' AND CATEGORY = '" + category + "' AND SUB_CATEGORY = '" + subcategory + "'";

                MySqlCommand cmd = new MySqlCommand(Qry, mySqlCon);
                String rdr = Convert.ToString(cmd.ExecuteScalar());

                return rdr;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }

        public Boolean IsExist(string id, string category, string subcategory)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                //string Qry = @"SELECT EXISTS(SELECT 1 FROM COMPANY_SUBCATEGORY_TYPES WHERE COMPANY_ID =@id  AND CATEGORY=@category AND SUB_CATEGORY=@subcategory )";

                string Qry = "sp_IsExist";
                mySqlCmd.Parameters.Add(new MySqlParameter("id", id));
                mySqlCmd.Parameters.Add(new MySqlParameter("category", category));
                mySqlCmd.Parameters.Add(new MySqlParameter("subcategory", subcategory));
                
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

        public DataTable ExistingTypeId(string id, string category, string typeid)
        {
            try
            {
                dataTable = new DataTable();
                string Qry = "sp_ExistingTypeId";
                mySqlCmd.Parameters.Add(new MySqlParameter("companyId", id));
                mySqlCmd.Parameters.Add(new MySqlParameter("categoryName", category));
                mySqlCmd.Parameters.Add(new MySqlParameter("typeid", typeid));

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
