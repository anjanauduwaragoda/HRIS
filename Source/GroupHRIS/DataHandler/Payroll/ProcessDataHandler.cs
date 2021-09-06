using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace DataHandler.Payroll
{
    public class ProcessDataHandler : TemplateDataHandler
    {
        public DataTable GetCompany()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable.Clear();

                string Qry = "sp_GetAllCompany";
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

        public DataTable GetTransactionNopay(string company)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "sp_GetAllNopayTransactions";
                mySqlCmd.CommandText = Qry;
                mySqlCmd.Parameters.Add(new MySqlParameter("company", company));
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

        public DataTable GetTransactionOT(string company)
        {
            try
            {
                dataTable.Rows.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "sp_GetOvertimeData";
                mySqlCmd.CommandText = Qry;
                mySqlCmd.Parameters.Add(new MySqlParameter("company", company));
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

        public DataTable GetProcessedOT(string TransMonth, string CompanyID)
        {
            try
            {
                dataTable.Rows.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"
                                SELECT 
	                                T.EMPLOYEE_ID,
                                    T.EPF_NO,
                                    T.CATEGORY,
                                    T.SUB_CATEGORY,
                                    T.TYPE_ID,
                                    T.FINALIZED_AMOUNT AS 'AMOUNT'
                                FROM
                                    TRANSACTIONS T,
                                    EMPLOYEE E
                                WHERE
                                    T.TRANS_MONTH = @TRANS_MONTH
                                        AND E.EMPLOYEE_ID = T.EMPLOYEE_ID
                                        AND E.COMPANY_ID = @COMPANY_ID
                                ORDER BY CAST(E.EPF_NO AS UNSIGNED) ASC , T.TYPE_ID ASC;
                            ";
                mySqlCmd.CommandText = Qry;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRANS_MONTH", TransMonth));
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", CompanyID));
                // mySqlCmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable GetModifiedOT(string TransMonth, string CompanyID)
        {
            try
            {
                dataTable.Rows.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = @"
                                SELECT 
	                                T.EMPLOYEE_ID,
                                    T.EPF_NO,
                                    T.CATEGORY,
                                    T.SUB_CATEGORY,
                                    T.TYPE_ID,
                                    T.FINALIZED_AMOUNT AS 'AMOUNT'
                                FROM
                                    TRANSACTIONS T,
                                    EMPLOYEE E
                                WHERE
                                    T.TRANS_MONTH = @TRANS_MONTH
                                        AND E.EMPLOYEE_ID = T.EMPLOYEE_ID
                                        AND E.COMPANY_ID = @COMPANY_ID
                                        AND T.REMARKS IS NOT NULL
                                ORDER BY CAST(E.EPF_NO AS UNSIGNED) ASC , T.TYPE_ID ASC;
                            ";
                mySqlCmd.CommandText = Qry;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRANS_MONTH", TransMonth));
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", CompanyID));
                // mySqlCmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable ProcessETIOvertime(string ProcessingUser)
        {
            try
            {
                dataTable.Rows.Clear();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "SP_ETI_OT";
                mySqlCmd.CommandText = Qry;
                mySqlCmd.Parameters.Add(new MySqlParameter("V_USER", ProcessingUser));
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

        public Boolean InsertNoPay(string company, string month, string user)
        {
            Boolean Status = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string Qry = "sp_ProcessNoPay";
                mySqlCmd.CommandText = Qry;
                mySqlCmd.Parameters.Add(new MySqlParameter("company", company));
                mySqlCmd.Parameters.Add(new MySqlParameter("currentMonth", month));
                mySqlCmd.Parameters.Add(new MySqlParameter("user", user));

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

        public Boolean InsertOT(string company, string month, string user)
        {
            Boolean Status = false;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string readSp = @"SELECT 
                                        d.PROCEDURE_NAME
                                    FROM
                                        MEARGE_COMPANY_OVERTIME m,
                                        STORED_PROCEDURE_DETAILS d
                                    WHERE
                                        d.PROCEDURE_ID = m.PROCEDURE_ID
                                            AND m.COMPANY_ID = '" + company + "';";
                MySqlCommand cmd = new MySqlCommand(readSp, mySqlCon);
                String rdr = Convert.ToString(cmd.ExecuteScalar());

                string Qry = rdr;
                mySqlCmd.CommandText = Qry;
                mySqlCmd.Parameters.Add(new MySqlParameter("company", company));
                mySqlCmd.Parameters.Add(new MySqlParameter("currentMonth", month));
                mySqlCmd.Parameters.Add(new MySqlParameter("user", user));                
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

    }
}
