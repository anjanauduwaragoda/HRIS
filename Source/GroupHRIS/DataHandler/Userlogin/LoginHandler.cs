using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Userlogin
{

    public class LoginHandler : TemplateDataHandler 
    {
        public DataTable populateHrisUser(string sUSER_ID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " select USER_PASSWORD " +
                                       " from HRIS_USER where USER_ID = '" + sUSER_ID + "' and STATUS_CODE =  '" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable populateUserSessions(string sUserID)
        {
            try
            {
                string sMySqlString = "";
                dataTable = new DataTable();

                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.Parameters.Add(new MySqlParameter("@sUserID", sUserID));

                sMySqlString = " select b.EMPLOYEE_ID,b.USER_ID,b.USER_PASSWORD,b.FIRST_NAME,d.ROLE_ID,b.COMPANY_ID,a.GENDER " +
                                   " FROM EMPLOYEE a INNER JOIN HRIS_USER b " +
                                   " on a.EMPLOYEE_ID = b.EMPLOYEE_ID " +
                                   " left outer join HRIS_USER_ROLE c " +
                                   " ON b.USER_ID = c.USER_ID " +
                                   " left outer join HRIS_ROLE d " +
                                   " ON c.ROLE_ID = d.ROLE_ID  " +
                                   " where b.USER_ID = @sUserID and a.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' and a.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "' and c.STATUS_CODE =  '" + Constants.STATUS_ACTIVE_VALUE + "'";

                mySqlCmd.CommandText = sMySqlString;

                //MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                mySqlCmd.Parameters.Clear();

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public Boolean Updatenewpassword(string sUserid, string sUSER_PASSWORD, string sSTATUS_CODE, string sMODIFIY_BY, string sMODIFIY_DATE)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sUserid", sUserid.Trim() == "" ? (object)DBNull.Value : sUserid.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sUSER_PASSWORD", sUSER_PASSWORD.Trim() == "" ? (object)DBNull.Value : sUSER_PASSWORD.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSTATUS_CODE", sSTATUS_CODE.Trim() == "" ? (object)DBNull.Value : sSTATUS_CODE.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMODIFIY_BY", sMODIFIY_BY.Trim() == "" ? (object)DBNull.Value : sMODIFIY_BY.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMODIFIY_DATE", sMODIFIY_DATE.Trim() == "" ? (object)DBNull.Value : sMODIFIY_DATE.Trim()));

                sMySqlString = "UPDATE HRIS_USER set MODIFIED_BY =@sMODIFIY_BY, MODIFIED_DATE=@sMODIFIY_DATE,USER_PASSWORD=@sUSER_PASSWORD  where USER_ID=@sUserid and STATUS_CODE =@sSTATUS_CODE";

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

                mySqlTrans.Rollback();
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
            return blInserted;
        }

        public DataTable populateEmployeeGender(string sEMPLOYEE_ID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " select GENDER,STATUS_CODE " +
                                       " from EMPLOYEE where EMPLOYEE_ID = '" + sEMPLOYEE_ID + "' and STATUS_CODE =  '" + Constants.STATUS_ACTIVE_VALUE + "'";

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
