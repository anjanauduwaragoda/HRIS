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
    public class EmployeeTypeDataHandler :TemplateDataHandler
    {
        // Anjana Uduwaragoda
        // Used in webFrmEmployee
        public DataTable populate()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT EMP_TYPE_ID,TYPE_NAME FROM EMPLOYEE_TYPE";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean isEmpTypeExist(string sEMP_TYPE_ID)
        {
            Boolean isExist = false;
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT EMP_TYPE_ID " +
                                    " FROM EMPLOYEE_TYPE WHERE EMP_TYPE_ID = '" + sEMP_TYPE_ID + "'";

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

        public DataTable GetEmployeeTypes()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"SELECT EMP_TYPE_ID,TYPE_NAME,DESCRIPTION FROM EMPLOYEE_TYPE;";
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

        public Boolean InsertEmployeeType(string TypeName, string Description)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                SerialHandler serialHandler = new SerialHandler();
                string EmployeeTypeID = serialHandler.getserila(mySqlCon, Constants.EMPLOYEE_TYPE_ID_STAMP);

                string Qry = @"INSERT INTO EMPLOYEE_TYPE(EMP_TYPE_ID,TYPE_NAME,DESCRIPTION)
                            VALUES(@EmployeeTypeID,@TypeName,@Remarks);";
                mySqlCmd.Parameters.Add(new MySqlParameter("@EmployeeTypeID", EmployeeTypeID));
                mySqlCmd.Parameters.Add(new MySqlParameter("@TypeName", TypeName));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Remarks", Description));
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

        public Boolean UpdateEmployeeType(string TypeName, string Description, string EmployeeTypeID)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"UPDATE EMPLOYEE_TYPE
                                SET TYPE_NAME=@TypeName, DESCRIPTION=@Remarks
                                WHERE EMP_TYPE_ID=@EmployeeTypeID;";
                mySqlCmd.Parameters.Add(new MySqlParameter("@TypeName", TypeName));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Remarks", Description));
                mySqlCmd.Parameters.Add(new MySqlParameter("@EmployeeTypeID", EmployeeTypeID));
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

        public Boolean CheckPrevRecord(string EmployeeTypeName)
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

                MSC.Parameters.Add(new MySqlParameter("@EmployeeTypeName", EmployeeTypeName.Trim() == "" ? (object)DBNull.Value : EmployeeTypeName.Trim()));
                string sMySqlString = @"select * from EMPLOYEE_TYPE where TYPE_NAME=@EmployeeTypeName;";
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
