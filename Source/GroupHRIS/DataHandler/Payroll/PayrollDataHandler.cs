using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Payroll
{
    public class PayrollDataHandler : TemplateDataHandler
    {
        public DataTable getCompany()
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = " SELECT COMP_NAME,COMPANY_ID FROM COMPANY WHERE STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' ORDER BY COMP_NAME  ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getCompany(string CompanyId)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = " SELECT COMP_NAME,COMPANY_ID FROM COMPANY WHERE COMPANY_ID ='" + CompanyId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPayrollDetails(string CompanyId, string TransMonth)
        {
            try
            {
                dataTable = new DataTable();

                mySqlCmd = new MySqlCommand();
                mySqlCmd.Connection = mySqlCon;

                string sMySqlString = @" 
                                            SELECT 
                                                T.CATEGORY, T.EPF_NO, T.TYPE_ID, T.FINALIZED_AMOUNT
                                            FROM
                                                TRANSACTIONS T,
                                                EMPLOYEE E
                                            WHERE
                                                T.TRANS_MONTH = @TRANS_MONTH 
                                                    AND T.EMPLOYEE_ID = E.EMPLOYEE_ID
                                                    AND T.IS_UPLOADED = @IS_UPLOADED 
                                                    AND E.COMPANY_ID = @COMPANY_ID 
                                            ORDER BY (T.EPF_NO * 1) ASC , TYPE_ID ASC;
                                        ";

                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.Parameters.Add(new MySqlParameter("@TRANS_MONTH", TransMonth));
                mySqlCmd.Parameters.Add(new MySqlParameter("@COMPANY_ID", CompanyId));
                mySqlCmd.Parameters.Add(new MySqlParameter("@IS_UPLOADED", Constants.CON_PENDING_STATUS));


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
        }
    }
}
