using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Property
{
    public class EmployeePropertyDetailsDataHandler : TemplateDataHandler
    {
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

        public DataTable getDefaultPropertyList(string employeeId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                dataTable.Rows.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("employeeId", employeeId));
                string Qry = "sp_GetSelectedProperties";
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

        public DataTable availabalePropertyLits(string employeeId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable.Rows.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("employeeId", employeeId));
                string Qry = "sp_AvalableProperties";
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

        public Boolean insert(string employeeId, DataTable propertyBucket, string assignDate, string returnedDate, string remarks, string clearanceMail, string user)
        {
            Boolean status = false;
            string empAssignId = "";
            SerialHandler serialHandler = new SerialHandler();
            MySqlTransaction mySqlTrans = null;


            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;
                assignDate = CommonUtils.dateFormatChange(assignDate);

                foreach (DataRow dr in propertyBucket.Rows)
                {
                    mySqlCmd.Parameters.Clear();

                    mySqlCmd.Parameters.Add(new MySqlParameter("empId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("assignDate", assignDate.Trim() == "" ? (object)DBNull.Value : assignDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("returnedDate", returnedDate.Trim() == "" ? (object)DBNull.Value : returnedDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("clearanceMail", clearanceMail.Trim() == "" ? (object)DBNull.Value : clearanceMail.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("user", user.Trim() == "" ? (object)DBNull.Value : user.Trim()));

                    string propertyId = dr["PROPERTY_ID"].ToString();
                    empAssignId = serialHandler.getserila(mySqlCon, Constants.ASSIGN_ID);

                    mySqlCmd.Parameters.Add(new MySqlParameter("empAssignId", empAssignId.Trim() == "" ? (object)DBNull.Value : empAssignId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("propertyId", propertyId.Trim() == "" ? (object)DBNull.Value : propertyId.Trim()));

                    string Qry = "sp_InsertEmployeePropertyDetails";
                    string QryUpdate = "sp_UpdatePropertyTableAvailability";
                    mySqlCmd.CommandType = CommandType.StoredProcedure;

                    mySqlCmd.CommandText = Qry;
                    mySqlCmd.ExecuteNonQuery();

                    mySqlCmd.Parameters.Clear();
                    mySqlCmd.Parameters.Add(new MySqlParameter("propertyId", propertyId.Trim() == "" ? (object)DBNull.Value : propertyId.Trim()));
                    //string commandString = " UPDATE PROPERTY 	SET STATUS_CODE = '0' WHERE PROPERTY_ID ='" + propertyId.Trim() + "'";
                    mySqlCmd.CommandText = QryUpdate;
                    mySqlCmd.CommandType = CommandType.StoredProcedure;
                    mySqlCmd.ExecuteNonQuery();

                }

                mySqlTrans.Commit();

                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                //if (status == true)
                //{
                //    //string QryUpdate = "UPDATE PROPERTY SET STATUS_CODE = '0' WHERE PROPERTY_ID ='" + propertyId + "'";
                //    string QryUpdate = "sp_UpdatePropertyTableAvailability";
                //    mySqlCmd.CommandType = CommandType.StoredProcedure;
                //    mySqlCmd.CommandText = QryUpdate;
                //    mySqlCmd.ExecuteNonQuery();
                //}
                status = true;
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                    mySqlCmd.Transaction.Rollback();
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

        public Boolean update(string assDate,string remarks,string email,string line)
        {
            Boolean status = false;
            string sMySqlString = "";

            string[] dateArr = assDate.Split('/', '-');
            assDate = dateArr[2] + "/" + dateArr[1] + "/" + dateArr[0];

            try
            {
                mySqlCon.Open();
                sMySqlString = @"UPDATE EMPLOYEE_PROPERTY_DETAILS 
                                    SET 
                                        ASSIGNED_DATE = '"+ assDate +@"',
                                        REMARKS = '" + remarks +@"',
                                        CLEARANCE_MAIL = '" + email +@"'
                                    WHERE
                                        LINE_ID = '" + line + "';";
                mySqlCmd.CommandText = sMySqlString;
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

        public DataTable getAssignProperties(string employeeId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                dataTable.Rows.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("employeeId", employeeId));
                string Qry = "sp_GetAssignedProperties";
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

        public String getEmployeeNameForProperty(string empCode)
        {
            string employeeName = "";

            mySqlCmd.Parameters.Add(new MySqlParameter("@empCode", empCode.Trim() == "" ? (object)DBNull.Value : empCode.Trim()));

            mySqlCmd.CommandText = "SELECT KNOWN_NAME FROM EMPLOYEE WHERE EMPLOYEE_ID = @empCode;";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    employeeName = mySqlCmd.ExecuteScalar().ToString();
                }
                mySqlCon.Close();
            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }

            return employeeName;
        }

        public String getEmployeeCompany(string empId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT COMPANY_ID FROM EMPLOYEE WHERE EMPLOYEE_ID = '" + empId + "';";

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
