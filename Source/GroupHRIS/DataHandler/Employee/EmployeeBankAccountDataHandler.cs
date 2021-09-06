using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Common;

namespace DataHandler.Employee
{
    public class EmployeeBankAccountDataHandler:TemplateDataHandler
    {
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Get all employees bank accounts for a given employee id
        ///</summary>
        ///<param name="employeeId">Pass a employeeid string to query to retrive bank account for that employee id</param>
        //----------------------------------------------------------------------------------------
        public DataTable populate(string employeeId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT eba.EMPLOYEE_ID,eba.BANK_ID,b.BANK_NAME,eba.BRANCH_ID, " +
	                                  " bb.BRANCH_NAME,eba.BANK_ACCOUNT_NUMBER, " +
	                                  " CASE " +
		                              "  when eba.STATUS_CODE='0' then 'Inactive' " +
		                              "  when eba.STATUS_CODE='1' then 'Active'  " +
                                      " End as ACCOUNT_STATUS,eba.REMARKS  " +
                                      " FROM EMPLOYEE_BANK_ACCOUNT eba,BANK b,BANK_BRANCH bb " +
                                      " where eba.BANK_ID=b.BANK_ID and eba.BANK_ID=bb.BANK_ID  " +
                                      " 	  and eba.BRANCH_ID=bb.BRANCH_ID and eba.EMPLOYEE_ID ='" + employeeId.Trim() + "'";

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
        ///An Employees bank account is inserted to the database
        ///</summary>
        ///<param name="employeeId">Pass a employeeid string to query </param>
        ///<param name="bankId">Pass a bank id string to query </param>
        ///<param name="branchId">Pass a bank branch id string to query </param>
        ///<param name="bankAccountNumber">Pass a bank account number string to query </param>
        ///<param name="statusCode">Pass the status of the bank account string to query </param>
        ///<param name="remarks">Pass a remarks string to query </param>        
        //----------------------------------------------------------------------------------------
        public Boolean Insert(String employeeId,
                              String bankId,
                              String branchId,
                              String bankAccountNumber,
                              String statusCode,
                              String remarks,
                              string addedBy)
        {
            Boolean blInserted = false;
                        
            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@bankId", bankId.Trim() == "" ? (object)DBNull.Value : bankId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@branchId", branchId.Trim() == "" ? (object)DBNull.Value : branchId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@bankAccountNumber", bankAccountNumber.Trim() == "" ? (object)DBNull.Value : bankAccountNumber.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));



            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                sMySqlString = "INSERT INTO EMPLOYEE_BANK_ACCOUNT(EMPLOYEE_ID,BANK_ID,BRANCH_ID,BANK_ACCOUNT_NUMBER,STATUS_CODE,REMARKS,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) " +
                                " VALUES(@employeeId,@bankId,@branchId,@bankAccountNumber,@statusCode,@remarks,@addedBy,now(),@addedBy,now())";


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
        ///An Employees bank account details are modified
        ///</summary>
        ///<param name="employeeId">Pass a employeeid string to query </param>
        ///<param name="bankId">Pass a bank id string to query </param>
        ///<param name="branchId">Pass a bank branch id string to query </param>
        ///<param name="bankAccountNumber">Pass a bank account number string to query </param>
        ///<param name="statusCode">Pass the status of the bank account string to query </param>
        ///<param name="remarks">Pass a remarks string to query </param>        
        //----------------------------------------------------------------------------------------
        public Boolean Update(String employeeId,
                              String bankId,
                              String branchId,
                              String bankAccountNumber,
                              String statusCode,
                              String remarks,
                              string addedBy)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@bankId", bankId.Trim() == "" ? (object)DBNull.Value : bankId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@branchId", branchId.Trim() == "" ? (object)DBNull.Value : branchId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@bankAccountNumber", bankAccountNumber.Trim() == "" ? (object)DBNull.Value : bankAccountNumber.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                sMySqlString = " UPDATE EMPLOYEE_BANK_ACCOUNT SET BANK_ACCOUNT_NUMBER=@bankAccountNumber,STATUS_CODE=@statusCode,REMARKS=@remarks,MODIFIED_BY=@addedBy,MODIFIED_DATE=now()" +
                               " WHERE EMPLOYEE_ID=@employeeId and  BANK_ID=@bankId and BRANCH_ID=@branchId";
                
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

        public Boolean isAccountExist(string bankId,string branchId,string accountNo)
        {
            Boolean isExist = false;

            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT BANK_ACCOUNT_NUMBER FROM EMPLOYEE_BANK_ACCOUNT " +
                                      " where BANK_ID='" + bankId.Trim() + "' AND BRANCH_ID ='" + branchId.Trim() + "' AND BANK_ACCOUNT_NUMBER='" + accountNo.Trim() + "'";
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


    }
}
