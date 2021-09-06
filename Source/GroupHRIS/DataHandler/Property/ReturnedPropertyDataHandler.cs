using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;


namespace DataHandler.Property
{
    public class ReturnedPropertyDataHandler:TemplateDataHandler
    {
        public DataTable getEmployeeProperty(string employeeId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                dataTable.Rows.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
                string Qry = "sp_GetPropertyForReturn";
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

        public DataTable getEmployeeName(string empName)
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

        public Boolean update(string assignId,string propertyId, string returnDate, string status, string remarks, string logUser)
        {
            Boolean Status = false;
            MySqlTransaction mysqlTrans = null;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                    mysqlTrans = mySqlCon.BeginTransaction();
                    mySqlCmd.Transaction = mysqlTrans;
                }

                string Qry = "sp_UpdatePropertyReturned";
                string QryUpdatePropertyStatus = "sp_UpdatePropertyStatus";

                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.Add(new MySqlParameter("assignId", assignId));
                mySqlCmd.Parameters.Add(new MySqlParameter("propertyId", propertyId));
                mySqlCmd.Parameters.Add(new MySqlParameter("returnDate", returnDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("propertyStatus", status));
                mySqlCmd.Parameters.Add(new MySqlParameter("remarks", remarks));
                mySqlCmd.Parameters.Add(new MySqlParameter("logUser", logUser));
                mySqlCmd.CommandText = Qry;
                mySqlCmd.ExecuteNonQuery();

                mySqlCmd.CommandText = QryUpdatePropertyStatus;
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

        public DataTable returnProperty(string lineId,string propertyId,string empId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = @"SELECT 
                                            pt.DESCRIPTION,p.REFERENCE_NO,p.MODEL,p.SERIAL_NO,CONVERT(ep.ASSIGNED_DATE,CHAR) AS ASSIGNED_DATE,CONVERT(ep.RETURNED_DATE,CHAR) AS RETURNED_DATE
                                        FROM
                                            EMPLOYEE_PROPERTY_DETAILS ep,PROPERTY p,PROPERTY_TYPE pt
                                        WHERE
                                            ep.LINE_ID = '" + lineId + "' AND ep.PROPERTY_ID = '"+ propertyId +@"'
                                                AND ep.EMPLOYEE_ID = '" + empId + @"'
                                                AND p.PROPERTY_ID = ep.PROPERTY_ID
                                                AND pt.TYPE_ID = p.PROPERTY_TYPE_ID";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
