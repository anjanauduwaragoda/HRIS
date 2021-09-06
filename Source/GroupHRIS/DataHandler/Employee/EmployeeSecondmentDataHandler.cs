using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Common;
using NLog;

namespace DataHandler.Employee
{
    public class EmployeeSecondmentDataHandler : TemplateDataHandler
    {
        private static Logger log = LogManager.GetCurrentClassLogger();


        public DataTable populate(string empId)
        {

            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " select es.EMPLOYEE_ID, es.COMPANY_ID, co.COMP_NAME, es.DEPT_ID, dp.DEPT_NAME, es.DIVISION_ID, dv.DIV_NAME, CONVERT(es.FROM_DATE,CHAR) as FROM_DATE, CONVERT(es.END_DATE,CHAR) as END_DATE, es.STATUS_CODE, es.BRANCH_ID, es.COST_CENTER , es.PROFIT_CENTER " +
                                      " from EMPLOYEE_SECONDMENTS es, COMPANY co, DEPARTMENT dp, DIVISION dv " +
                                      " where es.COMPANY_ID = co.COMPANY_ID " +
                                      " and es.DEPT_ID = dp.DEPT_ID " +
                                      " and es.DIVISION_ID = dv.DIVISION_ID " +
                                      " and es.EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " order by es.line_no ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable populateEmpProfile(string empId)
        {

            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " select  co.COMP_NAME as COMP_NAME, dp.DEPT_NAME as DEPT_NAME, dv.DIV_NAME as DIV_NAME, DATE_FORMAT(es.FROM_DATE,'%Y-%m-%d') as FROM_DATE, DATE_FORMAT(es.END_DATE,'%Y-%m-%d') as END_DATE, es.REMARKS as REMARKS " +
                                      " from EMPLOYEE_SECONDMENTS es, COMPANY co, DEPARTMENT dp, DIVISION dv " +
                                      " where es.COMPANY_ID = co.COMPANY_ID " +
                                      " and es.DEPT_ID = dp.DEPT_ID " +
                                      " and es.DIVISION_ID = dv.DIVISION_ID " +
                                      " and es.EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " and es.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' " +
                                      " order by es.FROM_DATE,es.line_no ";

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
        ///<summary>
        ///Insert Secondments 
        ///</summary>
        //----------------------------------------------------------------------------------------
        public Boolean Insert(String employeeId,
                      String companyCode,
                      String departmentID,
                      String divisionID,
                      String branchID,
                      String costCenter,
                      String profitCenter,
                      String fromDate,
                      String endDate,
                      String remarks,
                      String userID)
        {
            Boolean bInserted   = false;
            Boolean bUpdated    = false;

            String statusCode = Constants.STATUS_ACTIVE_VALUE;

            EmployeeDataHandler dhEmployee = new EmployeeDataHandler();

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@companyCode", companyCode.Trim() == "" ? (object)DBNull.Value : companyCode.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@departmentID", departmentID.Trim() == "" ? (object)DBNull.Value : departmentID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@divisionID", divisionID.Trim() == "" ? (object)DBNull.Value : divisionID.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@branchID", branchID.Trim() == "" ? (object)DBNull.Value : branchID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@costCenter", costCenter.Trim() == "" ? (object)DBNull.Value : costCenter.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@profitCenter", profitCenter.Trim() == "" ? (object)DBNull.Value : profitCenter.Trim()));
            
            mySqlCmd.Parameters.Add(new MySqlParameter("@fromDate", fromDate.Trim() == "" ? (object)DBNull.Value : fromDate.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@endDate", endDate.Trim() == "" ? (object)DBNull.Value : endDate.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));


            //
            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();


                //--------------------------------------------------------------------------------------------------------------
                //Update Prev Status
                //--------------------------------------------------------------------------------------------------------------
                bUpdated = UpdatePreviousStatusAsInactive(mySqlCon, employeeId, userID);
                

                sMySqlString = " INSERT INTO EMPLOYEE_SECONDMENTS " +
                               " (EMPLOYEE_ID,COMPANY_ID,DEPT_ID,DIVISION_ID,BRANCH_ID,COST_CENTER,PROFIT_CENTER,  FROM_DATE,END_DATE,REMARKS,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) " +
                               " VALUES (@employeeId,@companyCode,@departmentID,@divisionID,@branchID,@costCenter,@profitCenter,   @fromDate,@endDate,@remarks,@statusCode,@userID,now(),@userID,now()) ";



                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();


                bInserted = true;
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
                if ((!bInserted) || (!bUpdated))
                    mySqlTrans.Rollback();
                else
                    mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                dhEmployee = null;
            }

            return bInserted;
        }



        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Check whether active secondments exist for the employee with open end dates or
        ///dates beyond today
        ///</summary>
        //----------------------------------------------------------------------------------------
        public DataRow getActiveSecondment(string empID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT * FROM EMPLOYEE_SECONDMENTS " +
                                      " where EMPLOYEE_ID = '" + empID.Trim() + "' " +
                                      " and STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' " +
                                      " and (END_DATE is null or END_DATE >= current_date) " +
                                      " order by LINE_NO desc";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                DataRow dr = null;
                mySqlDa.Fill(dataTable);

                if(dataTable.Rows.Count > 0)
                    dr = dataTable.Rows[0];

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Update all the previous secondment status of the given employee as inactive
        ///</summary>
        //----------------------------------------------------------------------------------------
        public Boolean UpdatePreviousStatusAsInactive(MySqlConnection mySqlCon_,
                                                      String employeeID_,
                                                      String userID_)
        {
            Boolean bUpdates = false;

            MySqlCommand cmd = mySqlCon_.CreateCommand();
            string sMySqlString = "";

            //MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeID_", employeeID_.Trim() == "" ? (object)DBNull.Value : employeeID_.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID_", userID_.Trim() == "" ? (object)DBNull.Value : userID_.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode_", Constants.STATUS_INACTIVE_VALUE));

            try
            {
                //mySqlCon.Open();

                //mySqlTrans = mySqlCon.BeginTransaction();

                sMySqlString = " update EMPLOYEE_SECONDMENTS " +
                                " set STATUS_CODE = @statusCode_ , " +
                                " MODIFIED_BY = @userID_, " +
                                " MODIFIED_DATE = now() " +
                                " WHERE EMPLOYEE_ID=@employeeID_ ";


                //mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                //mySqlTrans.Commit();

                //mySqlCon.Close();
                //mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                bUpdates = true;
            }
            catch (Exception ex)
            {
                if (mySqlCon_.State == ConnectionState.Open)
                {
                    mySqlCon_.Close();
                }

                throw ex;
            }
            finally
            {

            }
            return bUpdates;
        }




        public bool isNonOverlappingDateRange(string empID, string newFromDate, string newEndDate)
        {
            bool isValid = false;
            int iRecs = 0;

            try
            {
                string sMySqlString = " SELECT count(*) FROM EMPLOYEE_SECONDMENTS " +
                                        " where (EMPLOYEE_ID = @empID and FROM_DATE <= @newFromDate and END_DATE >= @newFromDate) " +
                                        " or    (EMPLOYEE_ID = @empID and FROM_DATE <= @newEndDate and END_DATE >= @newEndDate) ";


                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.Parameters.Add(new MySqlParameter("@empID", empID.Trim() == "" ? (object)DBNull.Value : empID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@newFromDate", newFromDate.Trim() == "" ? (object)DBNull.Value : newFromDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@newEndDate", newEndDate.Trim() == "" ? (object)DBNull.Value : newEndDate.Trim()));

                mySqlCon.Open();

                iRecs = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());

                if (iRecs == 0)
                    isValid = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                mySqlCmd.Dispose();
            }


            return isValid;
        }



    }
}
