using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Property
{
    public class PropertyDetailsSearchDataHandler:TemplateDataHandler
    {

        #region DataHandler methods

        public DataTable GetPropertyDetails()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "sp_GetAllPropertyDetails";
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

        public DataTable GetPropertiesByTypeId(string CompanyId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                dataTable.Rows.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("CompanyId", CompanyId.Trim() == "" ? (object)DBNull.Value : CompanyId.Trim()));
                string Qry = "sp_GetPropertyByIdIfAvalable";
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

        public DataTable GetEmployeeProperty(string employeeId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                dataTable.Rows.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
                string Qry = "sp_GetEmployeePropertyDetails";
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

        public DataTable GetEmployeeUtilizedProperty(string employeeId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = @"SELECT t.DESCRIPTION,CONVERT(e.ASSIGNED_DATE,CHAR) as ASSIGNED_DATE,CONVERT(e.RETURNED_DATE ,CHAR) as RETURNED_DATE,e.REMARKS,e.CLEARANCE_MAIL,
		                                    CASE WHEN e.PROPERTY_STATUS = '0' THEN 'Returned'
				                                    WHEN e.PROPERTY_STATUS = '1' THEN 'Utilized' 
				                                    When e.PROPERTY_STATUS = '2' THEN 'Discard'
		                                    END AS PROPERTY_STATUS,p.PROPERTY_ID,p.PROPERTY_TYPE_ID,e.LINE_ID
	                                    FROM EMPLOYEE_PROPERTY_DETAILS e ,PROPERTY_TYPE t,PROPERTY p
	                                    WHERE p.PROPERTY_ID = e.PROPERTY_ID AND 
			                                    p.PROPERTY_TYPE_ID = t.TYPE_ID AND
			                                    e.EMPLOYEE_ID = '" + employeeId + "' AND e.PROPERTY_STATUS = '1';";

                MySqlDataAdapter mySqlDataTable = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDataTable.Fill(dataTable);
                return dataTable;
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
        }

        public Boolean InsertPropertyDetails(string type, string reference, string model, string serial, string company, string SCode, string user)
        {
            Boolean status = false;
            SerialHandler serialHandler = new SerialHandler();
            string propertyId = "";

            mySqlCmd.Parameters.Clear();

            try
            {
                mySqlCon.Open();
                propertyId = serialHandler.getserila(mySqlCon, Constants.PROPERTY_ID);

                mySqlCmd.Parameters.Add(new MySqlParameter("propertyId", propertyId));
                mySqlCmd.Parameters.Add(new MySqlParameter("Ptype", type));
                mySqlCmd.Parameters.Add(new MySqlParameter("reference", reference));
                mySqlCmd.Parameters.Add(new MySqlParameter("model", model));
                mySqlCmd.Parameters.Add(new MySqlParameter("serialNo", serial));
                mySqlCmd.Parameters.Add(new MySqlParameter("company", company));
                mySqlCmd.Parameters.Add(new MySqlParameter("SCode", SCode));
                mySqlCmd.Parameters.Add(new MySqlParameter("user", user));

                string Qry = "sp_InsertPropertyDetails";

                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.CommandText = Qry;
                mySqlCmd.ExecuteNonQuery();
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

        public Boolean UpdateDetails(string id, string type, string reference, string model, string serial, string company, string SCode, string user)
        {
            Boolean status = false;
            try
            {
                mySqlCon.Open();

                mySqlCmd.Parameters.Add(new MySqlParameter("propertyId", id));
                mySqlCmd.Parameters.Add(new MySqlParameter("Ptype", type));
                mySqlCmd.Parameters.Add(new MySqlParameter("reference", reference));
                mySqlCmd.Parameters.Add(new MySqlParameter("model", model));
                mySqlCmd.Parameters.Add(new MySqlParameter("serialNo", serial));
                mySqlCmd.Parameters.Add(new MySqlParameter("company", company));
                mySqlCmd.Parameters.Add(new MySqlParameter("SCode", SCode));
                mySqlCmd.Parameters.Add(new MySqlParameter("user", user));

                string Qry = "sp_UpdatePropertyDetails";

                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.CommandText = Qry;
                mySqlCmd.ExecuteNonQuery();
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
                mySqlCon.Close();
            }
            return status;
        }

        public DataTable GetSelectedProperty(string TypeId,string companyId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = @"SELECT 
                                        p.PROPERTY_TYPE_ID,p.PROPERTY_ID,t.DESCRIPTION,p.REFERENCE_NO,p.MODEL,p.SERIAL_NO,c.COMP_NAME,
                                        case
                                            when p.STATUS_CODE = '1' then 'Avilable'
                                            when p.STATUS_CODE = '2' then 'Assigned'
                                            when p.STATUS_CODE = '3' then 'Disposed'
                                        End as STATUS_CODE
                                    FROM
                                        PROPERTY p,PROPERTY_TYPE t,COMPANY c
                                    WHERE
                                        p.PROPERTY_TYPE_ID = t.TYPE_ID
                                            AND c.COMPANY_ID = p.COMPANY_ID
                                            AND t.STATUS_CODE = '1'
                                            AND p.STATUS_CODE = '1'
                                            AND p.PROPERTY_TYPE_ID = '" + TypeId + "' AND c.COMPANY_ID = '" + companyId + "';";

                MySqlDataAdapter mySqlDataTable = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDataTable.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetSelectedPropertyByCompany(string TypeId, string company)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = @"SELECT 
                                        p.PROPERTY_TYPE_ID,p.PROPERTY_ID,t.DESCRIPTION,p.REFERENCE_NO,p.MODEL,p.SERIAL_NO,c.COMP_NAME,
                                        case
                                            when p.STATUS_CODE = '1' then 'Avilable'
                                            when p.STATUS_CODE = '2' then 'Assigned'
                                            when p.STATUS_CODE = '3' then 'Disposed'
                                        End as STATUS_CODE
                                    FROM
                                        PROPERTY p,PROPERTY_TYPE t,COMPANY c
                                    WHERE
                                        p.PROPERTY_TYPE_ID = t.TYPE_ID
                                            AND c.COMPANY_ID = p.COMPANY_ID
                                            AND t.STATUS_CODE = '1'
                                            AND p.STATUS_CODE = '1'
                                            AND p.PROPERTY_TYPE_ID = '" + TypeId + "' AND p.COMPANY_ID = '" + company + "';";

                MySqlDataAdapter mySqlDataTable = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDataTable.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable PropertyByCompany(string company)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = @"SELECT 
                                        p.PROPERTY_TYPE_ID,p.PROPERTY_ID,t.DESCRIPTION,p.REFERENCE_NO,p.MODEL,p.SERIAL_NO,c.COMP_NAME,
                                        case
                                            when p.STATUS_CODE = '1' then 'Avilable'
                                            when p.STATUS_CODE = '2' then 'Assigned'
                                            when p.STATUS_CODE = '3' then 'Disposed'
                                        End as STATUS_CODE
                                    FROM
                                        PROPERTY p,PROPERTY_TYPE t,COMPANY c
                                    WHERE
                                        p.PROPERTY_TYPE_ID = t.TYPE_ID
                                            AND c.COMPANY_ID = p.COMPANY_ID
                                            AND t.STATUS_CODE = '1'
                                            AND p.STATUS_CODE = '1'
                                            AND p.COMPANY_ID = '" + company + "';";

                MySqlDataAdapter mySqlDataTable = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDataTable.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
