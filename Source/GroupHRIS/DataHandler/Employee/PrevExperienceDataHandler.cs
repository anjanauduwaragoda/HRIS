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
    public class PrevExperienceDataHandler : TemplateDataHandler
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public DataTable populate(string empId)
        {

            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT LINE_NO, EMPLOYEE_ID, ORGANIZATION, ADDRESS, DESIGNATION, FROM_DATE, TO_DATE, REMARKS, REJECT_REASON, ADDED_BY, ADDED_DATE, MODIFIED_BY, MODIFIED_DATE, PHONE_NUMBER, VERIFIED_BY, VERIFIED_DATE, VERIFIED_BY_SERVICE_LETTER, CONVERT(RECORD_STATUS,CHAR) AS 'RECORD_STATUS' FROM PREVIOUS_EMPLOYEMENT " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " order by LINE_NO ";

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

                string sMySqlString = " SELECT ORGANIZATION,ADDRESS,DESIGNATION, DATE_FORMAT(FROM_DATE,'%Y-%m-%d') as FROM_DATE, DATE_FORMAT(TO_DATE,'%Y-%m-%d') as TO_DATE ,REMARKS FROM PREVIOUS_EMPLOYEMENT " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' AND RECORD_STATUS = '" + Constants.CON_PREVIOUS_EMPLOYMENT_APPROVED_CODE + "' " +
                                      " order by LINE_NO ";

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
                              String organization,
                              String address,
                              String designation,
                              String phoneNumber,
                              String fromDate,
                              String toDate,
                              String remarks,
                              String userID)
        {
            Boolean bInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@organization", organization.Trim() == "" ? (object)DBNull.Value : organization.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@address", address.Trim() == "" ? (object)DBNull.Value : address.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@designation", designation.Trim() == "" ? (object)DBNull.Value : designation.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@phoneNumber", phoneNumber.Trim() == "" ? (object)DBNull.Value : phoneNumber.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@fromDate", fromDate.Trim() == "" ? (object)DBNull.Value : fromDate.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toDate", toDate.Trim() == "" ? (object)DBNull.Value : toDate.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                sMySqlString = "  INSERT INTO PREVIOUS_EMPLOYEMENT "  +
                                "  (EMPLOYEE_ID,  "  +
                                "  ORGANIZATION,  "  +
                                "  ADDRESS,  "  +
                                "  DESIGNATION,  "  +
                                "  PHONE_NUMBER, " +
                                "  FROM_DATE,  "  +
                                "  TO_DATE,  "  +
                                "  REMARKS,  "  +
                                "  ADDED_BY,  "  +
                                "  ADDED_DATE,  "  +
                                "  MODIFIED_BY,  "  +
                                "  MODIFIED_DATE)  "  +
                                " VALUES  " +
                                "  (@employeeId,  " +
                                "  @organization,  " +
                                "  @address,  " +
                                "  @designation,  " +
                                "  @phoneNumber, " +
                                "  @fromDate,  " +
                                "  @toDate,  " +
                                "  @remarks,  " +
                                "  @userID,  " +
                                "  now() ,  " +
                                "  @userID,  " +
                                "  now() ) ";
                

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
                if ((!bInserted))
                {
                    mySqlTrans.Rollback();
                }
                else
                {
                    mySqlTrans.Commit();
                }
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

            }

            return bInserted;
        }

        public Boolean Update(String employeeId,
                              String lineNo,
                              String organization,
                              String address,
                              String designation,
                              String phoneNumber,
                              String fromDate,
                              String toDate,
                              String remarks,
                              String userID)
        {
            Boolean bUpdated = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@lineNo", lineNo.Trim() == "" ? (object)DBNull.Value : lineNo.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@organization", organization.Trim() == "" ? (object)DBNull.Value : organization.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@address", address.Trim() == "" ? (object)DBNull.Value : address.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@designation", designation.Trim() == "" ? (object)DBNull.Value : designation.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@phoneNumber", phoneNumber.Trim() == "" ? (object)DBNull.Value : phoneNumber.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@fromDate", fromDate.Trim() == "" ? (object)DBNull.Value : fromDate.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toDate", toDate.Trim() == "" ? (object)DBNull.Value : toDate.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                sMySqlString = "  UPDATE PREVIOUS_EMPLOYEMENT  " +
                                "  SET  " +
                                "  ORGANIZATION = @organization ,  " +
                                "  ADDRESS = @address ,  " +
                                "  DESIGNATION = @designation ,  " +
                                "  PHONE_NUMBER = @phoneNumber , " +
                                "  FROM_DATE = @fromDate ,  " +
                                "  TO_DATE = @toDate ,  " +
                                "  REMARKS = @remarks ,  " +
                                "  MODIFIED_BY = @userID ,  " +
                                "  MODIFIED_DATE = now() " +
                                "  WHERE LINE_NO = @lineNo AND EMPLOYEE_ID = @employeeId  ";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                bUpdated = true;
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
            return bUpdated;
        }


        public string getEmployeeName(string empId)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT KNOWN_NAME AS EMP_NAME FROM EMPLOYEE " +
                                      " WHERE EMPLOYEE_ID = '" + empId.Trim() + "' ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0]["EMP_NAME"].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getHrEmail(string empId)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = @"
                                            SELECT 
                                                C.HR_EMAILS, 
                                                E.TITLE, 
                                                E.KNOWN_NAME, 
                                                E.EMP_NIC, 
                                                E.EPF_NO, 
                                                C.COMP_NAME 
                                            FROM 
                                                EMPLOYEE E, 
                                                COMPANY C
                                            WHERE 
                                                E.COMPANY_ID = C.COMPANY_ID AND 
                                                E.EMPLOYEE_ID = '" + empId + @"';
                                        ";

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
