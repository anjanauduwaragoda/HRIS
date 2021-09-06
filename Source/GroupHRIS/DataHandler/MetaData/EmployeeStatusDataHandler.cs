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
    public class EmployeeStatusDataHandler : TemplateDataHandler
    {
        // Anjana Uduwaragoda
        // Used in webFrmEmployee
        public DataTable populate()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT STATUS_CODE,DESCRIPTION FROM EMPLOYEE_STATUS";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEmployeeStatus()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"select STATUS_CODE,DESCRIPTION from EMPLOYEE_STATUS;";
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

        public Boolean InsertEmployeeStatus(string Description)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                SerialHandler oSerialHandler = new SerialHandler();
                string StatusCode = oSerialHandler.getserila(mySqlCon, Constants.EMPLOYEE_STATUS_ID_STAMP);

                string Qry = @"insert into EMPLOYEE_STATUS(STATUS_CODE,DESCRIPTION)
                            values(@AssessmentStatusCode,@Remarks);";
                mySqlCmd.Parameters.Add(new MySqlParameter("@AssessmentStatusCode", StatusCode));
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

        public Boolean UpdateEmployeeStatus(string Description, string StatusCode)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"update EMPLOYEE_STATUS
                                set DESCRIPTION=@Remarks
                                where STATUS_CODE=@AssessmentStatusCode;";
                mySqlCmd.Parameters.Add(new MySqlParameter("@Remarks", Description));
                mySqlCmd.Parameters.Add(new MySqlParameter("@AssessmentStatusCode", StatusCode));
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
    }
}
