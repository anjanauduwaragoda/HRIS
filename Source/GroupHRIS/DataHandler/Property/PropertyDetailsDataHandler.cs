using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Property
{
    public class PropertyDetailsDataHandler : TemplateDataHandler
    {
        public DataTable GetPropertytypes()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "SELECT TYPE_ID,DESCRIPTION FROM PROPERTY_TYPE WHERE STATUS_CODE = '1'";
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

        public Boolean UpdateDetails(string propertyId,string type, string reference, string model, string serial, string company, string SCode, string user)
        {
            Boolean status = false;
            try
            {
                mySqlCon.Open();

                mySqlCmd.Parameters.Add(new MySqlParameter("propertyId", propertyId));
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

        public DataTable GetPropertyDetailsForSelectedProperty(string propertyId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
               // mySqlCmd.Parameters.Add(new MySqlParameter("propertyId", propertyId));

                string Qry = @"SELECT p.PROPERTY_ID,t.DESCRIPTION,p.REFERENCE_NO,p.MODEL,p.SERIAL_NO,c.COMP_NAME,
			                            case
				                            when p.STATUS_CODE = '1' then 'Avilable'
				                            when p.STATUS_CODE = '2' then 'Assigned'
				                            when p.STATUS_CODE = '3' then 'Disposed'
			                            End as STATUS_CODE 
	                            FROM PROPERTY p , PROPERTY_TYPE t,COMPANY c
	                            WHERE p.PROPERTY_TYPE_ID=t.TYPE_ID AND 
                                        c.COMPANY_ID = p.COMPANY_ID AND t.TYPE_ID = '" + propertyId + "';";
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

        public Boolean IsExistRefSerial(string refNo, string serial)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                
                string Qry = "SELECT EXISTS(SELECT 1 FROM PROPERTY WHERE REFERENCE_NO = '" + refNo + "' OR SERIAL_NO='" + serial + "');";
                
                mySqlCmd.CommandText = Qry;
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

        public String getStatus(string propertyId, string propertyTypeId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT STATUS_CODE FROM PROPERTY WHERE PROPERTY_ID = '" + propertyId + "' AND PROPERTY_TYPE_ID = '" + propertyTypeId + "';";

                MySqlCommand cmd = new MySqlCommand(Qry, mySqlCon);
                String rdr = Convert.ToString(cmd.ExecuteScalar());

                return rdr;
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

        public DataTable GetPropertyDetailsForCompany(string company, string propertyType)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    p.PROPERTY_ID,t.DESCRIPTION,p.REFERENCE_NO,p.MODEL,p.SERIAL_NO,c.COMP_NAME,
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
                                        AND c.COMPANY_ID = '" + company + "' AND p.PROPERTY_TYPE_ID = '" + propertyType + "';";

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

        public DataTable GetAllPropertyDetailsForCompany(string company)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT 
                                    p.PROPERTY_ID,t.DESCRIPTION,p.REFERENCE_NO,p.MODEL,p.SERIAL_NO,c.COMP_NAME,
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
                                        AND c.COMPANY_ID = '" + company + "';";

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
