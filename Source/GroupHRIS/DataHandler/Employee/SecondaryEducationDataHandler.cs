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
    public class SecondaryEducationDataHandler : TemplateDataHandler
    {
        private static Logger log = LogManager.GetCurrentClassLogger();



        public bool Save(List<SecondaryEducation> results, string userID)  
        {
            SecondaryEducation oSecEdu;

            bool bRetVal            = false;
            bool bObsoleted         = false;
            string sPrevCategory    = "";


            for (int i = 0; i < results.Count; i++)
            {
                oSecEdu = results.ElementAt(i);

                //---------------------------------------------------------------------------
                //Save only the records with not null subjects & grades pairs
                //---------------------------------------------------------------------------
                if ((oSecEdu.subject.Trim().Length > 0) && (oSecEdu.grade.Trim().Length > 0))
                {
                    //------------------------------------------------------------------------
                    //Make obsolete all the prev recs for the employee , AL/OL , Attempt
                    //------------------------------------------------------------------------
                    if((!bObsoleted) || (sPrevCategory != oSecEdu.isAL.ToString()))
                        bObsoleted = obsoleteSecondaryEducationRecs(oSecEdu, userID);

                    if (bObsoleted)
                    {
                        if (Insert(oSecEdu, userID))
                            bRetVal = true;
                        else
                        {
                            bRetVal = false;
                            break;
                        }
                    }
                    else
                    {
                        bRetVal = false;
                        break;
                    }

                    sPrevCategory = oSecEdu.isAL.ToString();
                }
            }

            oSecEdu = null;

            return bRetVal;
        }




        public Boolean Insert(SecondaryEducation oSecEdu, String userID)
        {
            Boolean bInserted = false;

            String statusCode = Constants.STATUS_INACTIVE_VALUE;
            String isAL = "N";

            if (oSecEdu.isAL)
                isAL = "Y";


            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Clear();

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", oSecEdu.empID == "" ? (object)DBNull.Value : oSecEdu.empID));
            mySqlCmd.Parameters.Add(new MySqlParameter("@isAL", isAL == "" ? (object)DBNull.Value : isAL));
            mySqlCmd.Parameters.Add(new MySqlParameter("@year", oSecEdu.year == "" ? (object)DBNull.Value : oSecEdu.year));
            mySqlCmd.Parameters.Add(new MySqlParameter("@attempt", oSecEdu.attempt == "" ? (object)DBNull.Value : oSecEdu.attempt));
            mySqlCmd.Parameters.Add(new MySqlParameter("@school", oSecEdu.school == "" ? (object)DBNull.Value : oSecEdu.school));
            mySqlCmd.Parameters.Add(new MySqlParameter("@subject", oSecEdu.subject == "" ? (object)DBNull.Value : oSecEdu.subject));
            mySqlCmd.Parameters.Add(new MySqlParameter("@grade", oSecEdu.grade == "" ? (object)DBNull.Value : oSecEdu.grade));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));



            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();



                sMySqlString = " INSERT INTO SECONDARY_EDUCATION " +
                                " (EMPLOYEE_ID, " +
                                " SUBJECT_NAME, " +
                                " GRADE, " +
                                " IS_AL, " +
                                " ADDED_BY, " +
                                " ADDED_DATE, " +
                                " MODIFIED_BY, " +
                                " MODIFIED_DATE, " +
                                " ATTEMPT, " +
                                " ATTEMPTED_YEAR, " +
                                " SCHOOL, " +
                                " STATUS_CODE, " +
                                " VERIFIED_BY) " +
                                " VALUES " +
                                " (@employeeId, " +
                                " @subject, " +
                                " @grade, " +
                                " @isAL, " +
                                " @userID, " +
                                " now() , " +
                                " @userID, " +
                                " now() , " +
                                " @attempt, " +
                                " @year, " +
                                " @school, " +
                                " @statusCode, " +
                                " '' ) ";

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




        public Boolean obsoleteSecondaryEducationRecs(SecondaryEducation oSecEdu, String userID)
        {
            Boolean bInserted = false;

            //String statusCode = Constants.STATUS_INACTIVE_VALUE;
            String statusCode = Constants.STATUS_OBSOLETE_VALUE;

            String isAL = "N";

            if (oSecEdu.isAL)
                isAL = "Y";

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Clear();

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", oSecEdu.empID == "" ? (object)DBNull.Value : oSecEdu.empID));
            mySqlCmd.Parameters.Add(new MySqlParameter("@isAL", isAL == "" ? (object)DBNull.Value : isAL));
            mySqlCmd.Parameters.Add(new MySqlParameter("@attempt", oSecEdu.attempt == "" ? (object)DBNull.Value : oSecEdu.attempt));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();


                sMySqlString = "UPDATE SECONDARY_EDUCATION " +
                                " SET " +
                                " STATUS_CODE = @statusCode , " +
                                " MODIFIED_BY = @userID , " +
                                " MODIFIED_DATE = now() " +
                                " WHERE EMPLOYEE_ID = @employeeId " +
                                " AND ATTEMPT = @attempt " +
                                " AND IS_AL = @isAL" ;


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
                if ((!bInserted) )
                    mySqlTrans.Rollback();
                else
                    mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
            }

            return bInserted;
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


                sMySqlString = "UPDATE SECONDARY_EDUCATION " +
                                " SET " +
                                " STATUS_CODE = @statusCode , " +
                                " VERIFIED_BY = @verifiedBy , " +
                                " MODIFIED_BY = @userID , " +
                                " MODIFIED_DATE = now() " +
                                " WHERE EMPLOYEE_ID = @employeeId " +
                                " AND LINE_NO = @lineNo " ;


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

        public Boolean verifyEntireSet(string employeeID, string isAL, string attempt, string statusCode, string userID)
        {
            Boolean bUpdated = false;

            //String existingStatusCode   = Constants.STATUS_INACTIVE_VALUE;
            string pendingStatusCode           = "0";

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Clear();

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@verifiedBy", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@isAL", isAL.Trim() == "" ? (object)DBNull.Value : isAL.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@attempt", attempt.Trim() == "" ? (object)DBNull.Value : attempt.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@pendingStatusCode", pendingStatusCode.Trim() == "" ? (object)DBNull.Value : pendingStatusCode.Trim()));


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();


                sMySqlString = "UPDATE SECONDARY_EDUCATION " +
                                " SET " +
                                " STATUS_CODE = @statusCode , " +
                                " VERIFIED_BY = @verifiedBy , " +
                                " MODIFIED_BY = @userID , " +
                                " MODIFIED_DATE = now() " +
                                " WHERE EMPLOYEE_ID = @employeeId " +
                                " AND IS_AL = @isAL " +
                                " AND STATUS_CODE = @pendingStatusCode " +
                                " AND ATTEMPT = @attempt ";


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


        public Boolean reject(string employeeID, string lineNo, string userID)
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


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();


                sMySqlString = "UPDATE SECONDARY_EDUCATION " +
                                " SET " +
                                " STATUS_CODE = @statusCode , " +
                                " VERIFIED_BY = @verifiedBy , " +
                                " MODIFIED_BY = @userID , " +
                                " MODIFIED_DATE = now() " +
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


        public Boolean rejectEntireSet(string employeeID, string isAL, string attempt, string existingStatusCode, string userID)
        {
            Boolean bUpdated = false;

            //String existingStatusCode   = Constants.STATUS_INACTIVE_VALUE;
            String statusCode           = Constants.STATUS_OBSOLETE_VALUE;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Clear();

            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@verifiedBy", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));

            mySqlCmd.Parameters.Add(new MySqlParameter("@isAL", isAL.Trim() == "" ? (object)DBNull.Value : isAL.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@attempt", attempt.Trim() == "" ? (object)DBNull.Value : attempt.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@existingStatusCode", existingStatusCode.Trim() == "" ? (object)DBNull.Value : existingStatusCode.Trim()));


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();


                sMySqlString = "UPDATE SECONDARY_EDUCATION " +
                                " SET " +
                                " STATUS_CODE = @statusCode , " +
                                " VERIFIED_BY = @verifiedBy , " +
                                " MODIFIED_BY = @userID , " +
                                " MODIFIED_DATE = now() " +
                                " WHERE EMPLOYEE_ID = @employeeId " +
                                " AND IS_AL = @isAL " +
                                " AND ATTEMPT = @attempt " +
                                " AND STATUS_CODE = @existingStatusCode ";


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


        public DataTable populate(string empId)
        {

            try
            {
                dataTable.Rows.Clear();


                string sMySqlString = " SELECT LINE_NO,EMPLOYEE_ID,SUBJECT_NAME,GRADE,IS_AL,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE,ATTEMPT,ATTEMPTED_YEAR,SCHOOL,STATUS_CODE,CASE WHEN STATUS_CODE = '0' THEN 'Pending' WHEN STATUS_CODE = '1' THEN 'Verified' WHEN STATUS_CODE = '9' THEN 'Rejected'END AS STATUS_DESC,VERIFIED_BY FROM SECONDARY_EDUCATION " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " order by ATTEMPT, IS_AL, LINE_NO ";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateOLAL(string empId , string sAttemp , string iSAl)
        {

            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT LINE_NO,EMPLOYEE_ID,SUBJECT_NAME,GRADE,IS_AL,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE,ATTEMPT,ATTEMPTED_YEAR,SCHOOL,STATUS_CODE,CASE WHEN STATUS_CODE = '0' THEN 'Pending' WHEN STATUS_CODE = '1' THEN 'Verified' WHEN STATUS_CODE = '9' THEN 'Rejected'END AS STATUS_DESC,VERIFIED_BY FROM SECONDARY_EDUCATION " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " and STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + "'  " +
                                      " and IS_AL='" + iSAl.Trim() + "' and ATTEMPT = '" + sAttemp.Trim() + "' order by LINE_NO ";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateValid(string empId)
        {

            try
            {
                dataTable.Rows.Clear();

                /*
                string sMySqlString = " SELECT * FROM SECONDARY_EDUCATION " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " and STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' " +
                                      " order by ATTEMPT, IS_AL, LINE_NO ";
                */

                string sMySqlString = " SELECT LINE_NO,EMPLOYEE_ID,SUBJECT_NAME,GRADE,IS_AL,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE,ATTEMPT,ATTEMPTED_YEAR,SCHOOL,STATUS_CODE,CASE WHEN STATUS_CODE = '0' THEN 'Pending' WHEN STATUS_CODE = '1' THEN 'Verified' WHEN STATUS_CODE = '9' THEN 'Rejected'END AS STATUS_DESC,VERIFIED_BY FROM SECONDARY_EDUCATION " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " and (STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' OR STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + "') "   +
                                      " order by ATTEMPT, IS_AL, LINE_NO ";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateValid(string empId,Boolean isAl)
        {
            string olAlTag = "";

            if (isAl == true)
            {
                olAlTag = "Y";
            }
            else
            {
                olAlTag = "N";
            }

            try
            {
                dataTable.Rows.Clear();

                /*
                string sMySqlString = " SELECT * FROM SECONDARY_EDUCATION " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " and STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' " +
                                      " order by ATTEMPT, IS_AL, LINE_NO ";
                */

                string sMySqlString = " SELECT LINE_NO,EMPLOYEE_ID,SUBJECT_NAME,GRADE,IS_AL,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE,ATTEMPT,ATTEMPTED_YEAR,SCHOOL,STATUS_CODE,CASE WHEN STATUS_CODE = '0' THEN 'Pending' WHEN STATUS_CODE = '1' THEN 'Verified' WHEN STATUS_CODE = '9' THEN 'Rejected'END AS STATUS_DESC,VERIFIED_BY FROM SECONDARY_EDUCATION " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' and IS_AL='" + olAlTag.Trim() + "'" +
                                      " and (STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' OR STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + "') " +
                                      " order by ATTEMPT, IS_AL, LINE_NO ";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable populateObsolete(string empId)
        {

            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT LINE_NO,EMPLOYEE_ID,SUBJECT_NAME,GRADE,IS_AL,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE,ATTEMPT,ATTEMPTED_YEAR,SCHOOL,STATUS_CODE,CASE WHEN STATUS_CODE = '0' THEN 'Pending' WHEN STATUS_CODE = '1' THEN 'Verified' WHEN STATUS_CODE = '9' THEN 'Rejected'END AS STATUS_DESC,VERIFIED_BY FROM SECONDARY_EDUCATION " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " and STATUS_CODE = '" + Constants.STATUS_OBSOLETE_VALUE + "' " +
                                      " order by ATTEMPT, IS_AL, LINE_NO ";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable populateEmpProfile(string empId, string isAl, string sAttempt)
        {

            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT SUBJECT_NAME as SUBJECT_NAME,GRADE as GRADE  FROM SECONDARY_EDUCATION " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " and STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' " +
                                      " and IS_AL =  '" + isAl.Trim() + "' " +
                                      " and ATTEMPT =  '" + sAttempt.Trim() + "' " +
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

        public DataTable populateEmpIsAlCount(string empId)
        {

            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT DISTINCT(IS_AL) as IS_AL,ATTEMPT as ATTEMPT,ATTEMPTED_YEAR as ATTEMPTED_YEAR,SCHOOL as SCHOOL FROM SECONDARY_EDUCATION " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " and STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' " +
                                      " order by IS_AL,ATTEMPT,LINE_NO ";

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

                string sMySqlString = " SELECT * FROM SECONDARY_EDUCATION " +
                                      " where EMPLOYEE_ID = '" + empId.Trim() + "' " +
                                      " and STATUS_CODE = '" + Constants.STATUS_INACTIVE_VALUE + "' " +
                                      " and VERIFIED_BY = '' " +
                                      " order by ATTEMPT, IS_AL, LINE_NO ";

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



    public class SecondaryEducation
    {
        public string empID {get; set;}
        public string school { get; set; }
        public string year { get; set; }
        public string attempt { get; set; } 
        public bool isAL { get; set; }
        public string subject { get; set; }
        public string grade { get; set; }


        public SecondaryEducation() {}

        public SecondaryEducation(string empID_, string school_, string year_, string attempt_, bool isAL_, string subject_, string grade_)
        {
            empID   = empID_;
            school  = school_;
            year    = year_;
            attempt = attempt_;
            isAL    = isAL_;
            subject = subject_;
            grade   = grade_;
        }

    }


}
