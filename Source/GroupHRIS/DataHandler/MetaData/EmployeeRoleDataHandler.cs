using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.MetaData
{
    public class EmployeeRoleDataHandler : TemplateDataHandler
    {
        // Anjana Uduwaragoda
        // Used in webFrmEmployee
        public DataTable populate()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT ROLE_ID,ROLE_NAME FROM EMPLOYEE_ROLE WHERE STATUS = '" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean isRoleIDExist(string sROLE_ID)
        {
            Boolean isExist = false;
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT ROLE_ID " +
                                    " FROM EMPLOYEE_ROLE WHERE ROLE_ID = '" + sROLE_ID + "'";

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

        public DataTable GetEmployeeRoles()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT ROLE_ID,ROLE_NAME,DESCRIPTION,STATUS FROM EMPLOYEE_ROLE;";
                mySqlCmd.CommandText = Qry;
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlCmd);
                dataAdapter.Fill(dataTable);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string Status = dataTable.Rows[i]["STATUS"].ToString();
                    if (Status == Constants.STATUS_ACTIVE_VALUE)
                    {
                        Status = Constants.STATUS_ACTIVE_TAG;
                    }
                    else if (Status == Constants.STATUS_INACTIVE_VALUE)
                    {
                        Status = Constants.STATUS_INACTIVE_TAG;
                    }
                    dataTable.Rows[i]["STATUS"] = Status;
                }
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

        public Boolean InsertEmployeeRole(string RoleName, string Description, string RoleStatus)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                SerialHandler serialHandler = new SerialHandler();
                string RoleID = serialHandler.getserila(mySqlCon, Constants.EMPLOYEE_ROLE_ID_STAMP);

                string Qry = @"INSERT INTO EMPLOYEE_ROLE(ROLE_ID,ROLE_NAME,DESCRIPTION,STATUS)
                            VALUES(@RoleID,@RoleName,@Remarks,@RoleStatus);";
                mySqlCmd.Parameters.Add(new MySqlParameter("@RoleID", RoleID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RoleName", RoleName));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Remarks", Description));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RoleStatus", RoleStatus));
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

        public Boolean UpdateEmployeeRole(string RoleName, string Description, string RoleID, string RoleStatus)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"UPDATE EMPLOYEE_ROLE
                                SET ROLE_NAME=@RoleName, DESCRIPTION=@Remarks, STATUS=@RoleStatus
                                WHERE ROLE_ID=@RoleID;";
                mySqlCmd.Parameters.Add(new MySqlParameter("@RoleName", RoleName));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Remarks", Description));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RoleID", RoleID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RoleStatus", RoleStatus));
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

        public Boolean CheckPrevRecord(string RoleName)
        {
            Boolean State = false;

            try
            {
                MySqlCommand MSC = new MySqlCommand();
                MSC.Connection = mySqlCon;
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                MSC.Parameters.Add(new MySqlParameter("@RoleName", RoleName.Trim() == "" ? (object)DBNull.Value : RoleName.Trim()));
                string sMySqlString = @"select * from EMPLOYEE_ROLE where ROLE_NAME=@RoleName;";
                MSC.CommandText = sMySqlString;

                MySqlDataReader MDR = MSC.ExecuteReader();
                if (MDR.Read())
                {
                    State = true;
                }

                return State;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }


    }
}
