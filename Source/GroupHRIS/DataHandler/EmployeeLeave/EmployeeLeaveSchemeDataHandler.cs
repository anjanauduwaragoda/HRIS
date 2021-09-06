using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Common;

namespace DataHandler.EmployeeLeave
{
    public class EmployeeLeaveSchemeDataHandler:TemplateDataHandler
    {

        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 08-07-2014
        // this function is written to used at webFrmEmploeeLeaveScheme.aspx
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return employee leave schemes for a given employee
        ///<param name="employeeId">Pass a employeeid string to query </param>
        ///</summary>        
        //----------------------------------------------------------------------------------------

        public DataTable getEmployeeLeveSchemes(string employeeId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString =   " SELECT els.LEAVE_SCHEME_ID,ls.LEAVE_SCHEM_NAME, " +
                                        " CASE  " +
                                        " when els.STATUS_CODE='0' then 'Inactive' " +
                                        " when els.STATUS_CODE='1' then 'Active'  " +
                                        " End as STATUS,coalesce(els.REMARKS,'') REMARKS,els.LINE_NO " +
                                        " FROM EMPLOYEE_LEAVE_SCHEME as els,LEAVE_SCHEME as ls " +
                                        " WHERE els.LEAVE_SCHEME_ID = ls.LEAVE_SCHEME_ID and  els.EMPLOYEE_ID='" + employeeId.Trim() + "'";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 09-07-2014
        // this function is written to used at webFrmEmploeeLeaveScheme.aspx
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return true if given leave scheme has assigned to employee and it is active for a given employee
        ///<param name="employeeId">Pass a employeeid string to query </param>
        ///<param name="schemeId">Pass a leave scheme id string to query </param>
        ///</summary>        
        //----------------------------------------------------------------------------------------

        public Boolean isSchemeExist(string employeeId,string schemeId)
        {
            Boolean isExist = false;
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT LEAVE_SCHEME_ID " +
                                      " FROM EMPLOYEE_LEAVE_SCHEME " +
                                      " WHERE LEAVE_SCHEME_ID ='" + schemeId.Trim() + "' and EMPLOYEE_ID ='" + employeeId.Trim() + "' and STATUS_CODE='1'";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    isExist = true;
                }
                return isExist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //----------------------------------------------------------------------------------------
        ///<summary>
        ///An Employees leave scheme is inserted to the database
        ///</summary>
        ///<param name="employeeId">Pass a employeeid string to query </param>
        ///<param name="leaveSchemeId">Pass a leave scheme id string to query </param>
        ///<param name="statuscode">Pass a status code string to query </param> 
        ///<param name="remarks">Pass a remarks string to query </param>
        //----------------------------------------------------------------------------------------
        public Boolean Insert(String employeeId,
                              String leaveSchemeId,
                              String statuscode,
                              String remarks)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@leaveSchemeId", leaveSchemeId.Trim() == "" ? (object)DBNull.Value : leaveSchemeId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statuscode", statuscode.Trim() == "" ? (object)DBNull.Value : statuscode.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                sMySqlString = " INSERT INTO EMPLOYEE_LEAVE_SCHEME(EMPLOYEE_ID,LEAVE_SCHEME_ID,STATUS_CODE,REMARKS) " +
                               " VALUES(@employeeId,@leaveSchemeId,@statuscode,@remarks)";                     

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

            }
            return blInserted;
        }

        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Status and remarks of an Employees leave scheme is changed 
        ///</summary>
        ///<param name="employeeId">Pass a employeeid string to query </param>
        ///<param name="leaveSchemeId">Pass a leave scheme id string to query </param>
        ///<param name="statuscode">Pass a status code string to query </param> 
        ///<param name="remarks">Pass a remarks string to query </param>
        //----------------------------------------------------------------------------------------
        public Boolean Update(String employeeId,
                              String leaveSchemeId,
                              String statuscode,
                              String remarks,
                              String lineNo)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@leaveSchemeId", leaveSchemeId.Trim() == "" ? (object)DBNull.Value : leaveSchemeId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statuscode", statuscode.Trim() == "" ? (object)DBNull.Value : statuscode.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@lineNo", lineNo.Trim() == "" ? (object)DBNull.Value : lineNo.Trim()));

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                sMySqlString = " UPDATE EMPLOYEE_LEAVE_SCHEME set STATUS_CODE =@statuscode,REMARKS=@remarks WHERE LINE_NO=@lineNo and EMPLOYEE_ID=@employeeId and LEAVE_SCHEME_ID=@leaveSchemeId";
                
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

            }
            return blInserted;
        }

        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 10-07-2014
        // this function is written to used at webFrmEmploeeLeaveScheme.aspx
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return true if active leave scheme is exist for a given employee 
        ///<param name="employeeId">Pass a employeeid string to query </param>
        ///</summary>        
        //----------------------------------------------------------------------------------------

        public Boolean isActiveSchemeExist(string employeeId)
        {
            Boolean isExist = false;
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT LEAVE_SCHEME_ID " +
                                      " FROM EMPLOYEE_LEAVE_SCHEME " +
                                      " WHERE EMPLOYEE_ID ='" + employeeId.Trim() + "' and STATUS_CODE='1'";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    isExist = true;
                }
                return isExist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 18-07-2014
        // this function is written to used at webFrmEmploeeLeaveSchedule.aspx
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return leave types of the leave scheme which is active for a given employee
        ///<param name="employeeId">Pass a employeeid string to query </param>        
        ///</summary>        
        //----------------------------------------------------------------------------------------

        public DataTable getLeaveTypesOfActiveLeaveScheme(string employeeId)
        {            
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString =   " SELECT lt.LEAVE_TYPE_NAME,lt.LEAVE_TYPE_ID " +
                                        " FROM EMPLOYEE_LEAVE_SCHEME as els inner join LEAVE_SCHEME_ITEM as lsi " + 
                                        " 	 on els.LEAVE_SCHEME_ID = lsi.LEAVE_SCHEME_ID " + 
                                        " 	 inner join LEAVE_TYPE as lt on lsi.LEAVE_TYPE_ID = lt.LEAVE_TYPE_ID " +
                                        " where els.STATUS_CODE='1' and els.EMPLOYEE_ID='" + employeeId.Trim() + "' order by lt.LEAVE_TYPE_NAME";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 15-10-2015
        // this function is written to used at online leaves 
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return leave types of the leave scheme which is active for a given employee
        ///<param name="employeeId">Pass a employeeid string to query </param>        
        ///</summary>        
        //----------------------------------------------------------------------------------------

        public DataTable getLeaveTypesOfActiveLeaveSchemeForOnline(string employeeId)
        {
            try
            {
                DataTable dtLTypes = new DataTable();
                dtLTypes.Rows.Clear();
                string sMySqlString = " SELECT lt.LEAVE_TYPE_NAME,lt.LEAVE_TYPE_ID " +
                                        " FROM EMPLOYEE_LEAVE_SCHEME as els inner join LEAVE_SCHEME_ITEM as lsi " +
                                        " 	 on els.LEAVE_SCHEME_ID = lsi.LEAVE_SCHEME_ID " +
                                        " 	 inner join LEAVE_TYPE as lt on lsi.LEAVE_TYPE_ID = lt.LEAVE_TYPE_ID " +
                                        " where els.STATUS_CODE='1' and lt.IS_DISPLAY_ONLINE='1' and els.EMPLOYEE_ID='" + employeeId.Trim() + "' order by lt.LEAVE_TYPE_NAME";
                //
                MySqlDataAdapter dt = new MySqlDataAdapter(sMySqlString, mySqlCon);
                dt.Fill(dtLTypes);

                return dtLTypes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 22-07-2014      
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return active leave scheme line 
        ///<param name="employeeId">Pass a employeeid string to query </param>
        ///</summary>        
        //----------------------------------------------------------------------------------------


        public int getActiveLeaveSchemeLine(string employeeId)
        {

            int lineNo = 0;

            mySqlCmd.CommandText = "SELECT LINE_NO FROM EMPLOYEE_LEAVE_SCHEME WHERE EMPLOYEE_ID='" + employeeId.Trim() + "' and STATUS_CODE='1'";

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    lineNo = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());
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

            return lineNo;
        }

        
    }
}
