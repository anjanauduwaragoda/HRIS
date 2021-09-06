using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Employee
{
    public class DesignationDataHandler : TemplateDataHandler
    {
        public DataTable getDesignationIdDesigName(string companyId)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = "SELECT DESIGNATION_NAME,DESIGNATION_ID FROM EMPLOYEE_DESIGNATION where COMPANY_ID='" + companyId.Trim() + "' AND STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' order by DESIGNATION_NAME";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Boolean isDesigIDExist(string companyId, string sDESIGNATION_ID)
        {
            Boolean isExist = false;
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DESIGNATION_ID " +
                                    " FROM EMPLOYEE_DESIGNATION WHERE COMPANY_ID = '" + companyId + "' AND DESIGNATION_ID = '" + sDESIGNATION_ID + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExist = true;
                }

                return isExist;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public DataTable populate()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString =   " SELECT emp.DESIGNATION_ID,emp.DESIGNATION_NAME, " +
                                        " CASE " +
                                        " 	when emp.STATUS_CODE='0' then 'Inactive' " +
                                        " 	when emp.STATUS_CODE='1' then 'Active' " +
                                        " End as STATUS,com.COMP_NAME as COMPANY" +
                                        " FROM EMPLOYEE_DESIGNATION emp,COMPANY com " +
                                        " where emp.COMPANY_ID = com.COMPANY_ID " +
                                        " order by emp.COMPANY_ID";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populate(String companyId)
        {
            try
            {
                dataTable.Rows.Clear();


                string sMySqlString = " SELECT emp.DESIGNATION_ID,emp.DESIGNATION_NAME, " +
                                        " CASE " +
                                        " 	when emp.STATUS_CODE='0' then 'Inactive' " +
                                        " 	when emp.STATUS_CODE='1' then 'Active' " +
                                        " End as STATUS,com.COMP_NAME as COMPANY " +
                                        " FROM EMPLOYEE_DESIGNATION emp,COMPANY com " +
                                        " where emp.COMPANY_ID = com.COMPANY_ID " +
                                        " and emp.COMPANY_ID ='" + companyId.Trim() + "'" +
                                        " order by emp.DESIGNATION_NAME";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow getDesignationDetails(string designationId)
        {
            try
            {
                dataTable.Rows.Clear();

                

                string sMySqlString = " SELECT DESIGNATION_ID,DESIGNATION_NAME,REMARKS,STATUS_CODE,COMPANY_ID " +
                                      " FROM EMPLOYEE_DESIGNATION " +
                                      " where DESIGNATION_ID ='" + designationId + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                DataRow dataRow = null; 

                if (dataTable.Rows.Count > 0)
                {
                    dataRow = dataTable.Rows[0];
                }

                return dataRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Boolean Insert(String designationName,
                              String remarks,
                              String statusCode,
                              String companyId,
                              String addedBy)
        {
            Boolean blInserted = false;

            SerialHandler serialHandler = new SerialHandler();

            string designationId = "";

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;
                        
            mySqlCmd.Parameters.Add(new MySqlParameter("@designationName", designationName.Trim() == "" ? (object)DBNull.Value : designationName.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@companyId", companyId.Trim() == "" ? (object)DBNull.Value : companyId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                designationId = serialHandler.getserila(mySqlCon, Constants.DESIGNATION_ID_STAMP);

                mySqlCmd.Parameters.Add(new MySqlParameter("@designationId", designationId.Trim() == "" ? (object)DBNull.Value : designationId.Trim()));

                sMySqlString = "INSERT INTO EMPLOYEE_DESIGNATION(DESIGNATION_ID,DESIGNATION_NAME,REMARKS,STATUS_CODE,COMPANY_ID,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) " +
                                "VALUES(@designationId,@designationName,@remarks,@statusCode,@companyId,@addedBy,now(),@addedBy,now())";


                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                blInserted = true;
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
            }
            return blInserted;
        }

        public Boolean Update(String designationId,
                              String designationName,
                              String remarks,
                              String statusCode,
                              String companyId,
                              String addedBy)
        {
            Boolean blUpdated = false;
            
            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@designationId", designationId.Trim() == "" ? (object)DBNull.Value : designationId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@designationName", designationName.Trim() == "" ? (object)DBNull.Value : designationName.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@companyId", companyId.Trim() == "" ? (object)DBNull.Value : companyId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();              


                sMySqlString = " UPDATE EMPLOYEE_DESIGNATION set DESIGNATION_NAME =@designationName,REMARKS=@remarks,STATUS_CODE=@statusCode,COMPANY_ID=@companyId,MODIFIED_BY=@addedBy,MODIFIED_DATE=now() " +
                               " WHERE DESIGNATION_ID=@designationId ";
               
                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                blUpdated = true;
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }
           
            return blUpdated;
        }
    }
}
