using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using DataHandler.Employee;
using DataHandler.MetaData;
using Common;
using NLog;

namespace DataHandler.Employee
{

    public class HigherEducationDataHandler : TemplateDataHandler
    {
        private static Logger log = LogManager.GetCurrentClassLogger();



        public DataTable populate(string empId)
        {

            try
            {
                dataTable.Rows.Clear();

                /*
                string sMySqlString = " SELECT * FROM HIGHER_EDUCATION " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " and STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' " +
                                      " order by LINE_NO ";
                */
                string sMySqlString = " SELECT LINE_NO,EMPLOYEE_ID,INSTITUTE,PROGRAM,PROGRAME_NAME,DURATION,FROM_YEAR,TO_YEAR,GRADE,REMARKS,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE,SECTOR,STATUS_CODE,CASE WHEN STATUS_CODE = '0' THEN 'Pending' WHEN STATUS_CODE = '1' THEN 'Verified' WHEN STATUS_CODE = '9' THEN 'Rejected'END AS STATUS_DESC,VERIFIED_BY FROM HIGHER_EDUCATION " +
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

        public DataTable populateValid(string empId , string sStausCode)
        {

            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT LINE_NO,EMPLOYEE_ID,INSTITUTE,PROGRAM,PROGRAME_NAME,DURATION,FROM_YEAR,TO_YEAR,GRADE,REMARKS,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE,SECTOR,STATUS_CODE,CASE WHEN STATUS_CODE = '0' THEN 'Pending' WHEN STATUS_CODE = '1' THEN 'Verified' WHEN STATUS_CODE = '9' THEN 'Rejected'END AS STATUS_DESC,VERIFIED_BY FROM HIGHER_EDUCATION " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " and STATUS_CODE = '" + sStausCode + "' " +
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

                string sMySqlString = " SELECT INSTITUTE,PROGRAM,PROGRAME_NAME,DURATION,FROM_YEAR,TO_YEAR,GRADE,REMARKS,SECTOR,STATUS_CODE FROM HIGHER_EDUCATION " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " order by STATUS_CODE,LINE_NO ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateUnverified(string empId)
        {

            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT * FROM HIGHER_EDUCATION " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " and STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' " +
                                      " and (TRIM(VERIFIED_BY) = '' OR VERIFIED_BY is null) " +
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


        public DataTable populateEmailInfromation(string empId)
        {

            try
            {
                dataTable.Rows.Clear();

                /*
                string sMySqlString = " SELECT * FROM HIGHER_EDUCATION " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " and STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' " +
                                      " order by LINE_NO ";
                */
                string sMySqlString = " SELECT E.TITLE, E.INITIALS_NAME, E.EMP_NIC, E.EPF_NO, (SELECT COMP_NAME FROM COMPANY WHERE COMPANY_ID = E.COMPANY_ID) AS 'COMPANY_NAME' FROM EMPLOYEE E " +
                                      " WHERE E.EMPLOYEE_ID = '" + empId.Trim() + "' ";

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
                              String institute,
                              String program,
                              String programName,
                              String sector,
                              String duration,
                              String fromYear,
                              String toYear,
                              String grade,
                              String remarks,
                              String userID)
        {
            Boolean bInserted = false;

            String statusCode = Constants.STATUS_INACTIVE_VALUE;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@institute", institute.Trim() == "" ? (object)DBNull.Value : institute.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@program", program.Trim() == "" ? (object)DBNull.Value : program.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@programName", programName.Trim() == "" ? (object)DBNull.Value : programName.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@sector", sector.Trim() == "" ? (object)DBNull.Value : sector.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@duration", duration.Trim() == "" ? (object)DBNull.Value : duration.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@fromYear", fromYear.Trim() == "" ? (object)DBNull.Value : fromYear.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toYear", toYear.Trim() == "" ? (object)DBNull.Value : toYear.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@grade", grade.Trim() == "" ? (object)DBNull.Value : grade.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();



                sMySqlString = " INSERT INTO HIGHER_EDUCATION " +
	                            " (EMPLOYEE_ID, " +
	                            " INSTITUTE, " +
	                            " PROGRAM, " +
	                            " PROGRAME_NAME, " +
	                            " DURATION, " +
	                            " FROM_YEAR, " +
	                            " TO_YEAR, " +
	                            " GRADE, " +
	                            " REMARKS, " +
	                            " ADDED_BY, " +
	                            " ADDED_DATE, " +
	                            " MODIFIED_BY, " +
	                            " MODIFIED_DATE, " +
	                            " SECTOR, " +
	                            " STATUS_CODE, " +
	                            " VERIFIED_BY) " +
                            " VALUES " +
	                            " (@employeeId, " +
                                " @institute, " +
                                " @program, " +
                                " @programName, " +
                                " @duration, " +
                                " @fromYear, " +
                                " @toYear, " +
                                " @grade, " +
                                " @remarks, " +
	                            " @userID, " +
	                            " now() , " +
	                            " @userID, " +
	                            " now() , " +
                                " @sector, " +
                                " @statusCode, " +
	                            " '') ";


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
                    mySqlTrans.Rollback();
                else
                    mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

            }

            return bInserted;
        }





        public Boolean Update(String employeeId,
                              String lineNo,
                              String institute,
                              String program,
                              String programName,
                              String sector,
                              String duration,
                              String fromYear,
                              String toYear,
                              String grade,
                              String remarks,
                              String userID)
        {
            Boolean bUpdated = false;

            String statusCode = Constants.STATUS_INACTIVE_VALUE;


            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@lineNo", lineNo.Trim() == "" ? (object)DBNull.Value : lineNo.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@institute", institute.Trim() == "" ? (object)DBNull.Value : institute.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@program", program.Trim() == "" ? (object)DBNull.Value : program.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@programName", programName.Trim() == "" ? (object)DBNull.Value : programName.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@sector", sector.Trim() == "" ? (object)DBNull.Value : sector.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@duration", duration.Trim() == "" ? (object)DBNull.Value : duration.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@fromYear", fromYear.Trim() == "" ? (object)DBNull.Value : fromYear.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toYear", toYear.Trim() == "" ? (object)DBNull.Value : toYear.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@grade", grade.Trim() == "" ? (object)DBNull.Value : grade.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));




            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                sMySqlString = " UPDATE HIGHER_EDUCATION " +
                                " SET " +
                                " INSTITUTE = @institute, " +
                                " PROGRAM = @program, " +
                                " PROGRAME_NAME = @programName, " +
                                " DURATION = @duration, " +
                                " FROM_YEAR = @fromYear, " +
                                " TO_YEAR = @toYear, " +
                                " GRADE = @grade, " +
                                " REMARKS = @remarks, " +
                                " STATUS_CODE = @statusCode, " +
                                " MODIFIED_BY = @userID, " +
                                " MODIFIED_DATE = now() , " +
                                " SECTOR = @sector " +
                                " WHERE EMPLOYEE_ID = @employeeId AND LINE_NO = @lineNo ";

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



        public Boolean verify(string employeeID, string lineNo, string userID)
        {
            Boolean bUpdated = false;
            String statusCode = Constants.STATUS_ACTIVE_VALUE;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Clear();

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@lineNo", lineNo.Trim() == "" ? (object)DBNull.Value : lineNo.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@verifiedBy", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();


                sMySqlString = "UPDATE HIGHER_EDUCATION " +
                                " SET " +
                                " STATUS_CODE = @statusCode , " +
                                " VERIFIED_BY = @verifiedBy , " +
                                " VERIFIED_DATE = now() " +
                                " WHERE EMPLOYEE_ID = @employeeId " +
                                " AND LINE_NO = @lineNo ";


                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();


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
                if ((!bUpdated))
                    mySqlTrans.Rollback();
                else
                    mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
            }

            return bUpdated;
        }

        public Boolean reject(string employeeID, string lineNo, string userID, string reason)
        {
            Boolean bUpdated = false;
            String statusCode = Constants.STATUS_OBSOLETE_VALUE;


            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Clear();

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@lineNo", lineNo.Trim() == "" ? (object)DBNull.Value : lineNo.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@verifiedBy", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@reason", reason.Trim() == "" ? (object)DBNull.Value : reason.Trim()));

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();


                sMySqlString = "UPDATE HIGHER_EDUCATION " +
                                " SET " +
                                " STATUS_CODE = @statusCode , " +
                                " VERIFIED_BY = @verifiedBy , " +
                                " REJECT_REASON = @reason , " +
                                " VERIFIED_DATE = now() " +
                                " WHERE EMPLOYEE_ID = @employeeId " +
                                " AND LINE_NO = @lineNo ";


                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();


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
                if ((!bUpdated))
                    mySqlTrans.Rollback();
                else
                    mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
            }

            return bUpdated;
        }










    }
}
