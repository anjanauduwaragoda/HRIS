using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.Property
{
    public class DefaultPropertyDataHandler : TemplateDataHandler
    {
        public DataTable getPropertyList()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "SELECT TYPE_ID,DESCRIPTION FROM PROPERTY_TYPE WHERE STATUS_CODE = '1';";
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

        public DataTable getRoleList()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "SELECT ROLE_ID,ROLE_NAME FROM EMPLOYEE_ROLE";
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

        public DataTable getSelectedData(string roleId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                dataTable.Rows.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("roleId",roleId));
                string Qry = @"SELECT p.TYPE_ID,p.DESCRIPTION ,d.STATUS_CODE
	                            FROM DEFAULT_PROPERTY d , PROPERTY_TYPE p 
	                            WHERE p.TYPE_ID = d.PROPERTY_TYPE_ID AND d.EMPLOYEE_ROLE_ID ='" + roleId + "'";
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

        public Boolean getActiverecord(string id)
        {
            Boolean status = false;

            mySqlCmd.Parameters.Clear();

            try
            {
                mySqlCon.Open();

                mySqlCmd.Parameters.Add(new MySqlParameter("id", id));

                string Qry = "sp_selectCheckBox";
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

        public Boolean insert(string userRole, string property, string logUser, string isActive)
        {
            Boolean status = false;

            mySqlCmd.Parameters.Clear();

            try
            {
                mySqlCon.Open();

                mySqlCmd.Parameters.Add(new MySqlParameter("role", userRole));
                mySqlCmd.Parameters.Add(new MySqlParameter("property", property));
                mySqlCmd.Parameters.Add(new MySqlParameter("loguser", logUser));
                mySqlCmd.Parameters.Add(new MySqlParameter("isActive", isActive));

                string Qry = "sp_InsertDefaultProperty";

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

        public Boolean delete(string userRole, string property)
        {
            Boolean status = false;

            mySqlCmd.Parameters.Clear();

            try
            {
                mySqlCon.Open();

                mySqlCmd.Parameters.Add(new MySqlParameter("@role", userRole));
                mySqlCmd.Parameters.Add(new MySqlParameter("@property", property));

                string Qry = "sp_DeleteDefaultProperty";
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

    }
}
