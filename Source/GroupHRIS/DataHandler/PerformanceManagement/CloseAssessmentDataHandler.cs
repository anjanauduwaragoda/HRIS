using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class CloseAssessmentDataHandler:TemplateDataHandler
    {

        public DataTable getAllActiveCompanies()
        {
            DataTable resultDataTable = new DataTable();
            try
            {
                mySqlCon.Open();

                string sqlQueryString = " SELECT COMPANY_ID, COMP_NAME FROM COMPANY WHERE STATUS_CODE = '1' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQueryString, mySqlCon);
                mySqlDataAdapter.Fill(resultDataTable);
                return resultDataTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                resultDataTable.Dispose();
            }
        }

        public DataTable getCompanyById(string companyId)
        {
            DataTable resultDataTable = new DataTable();
            try
            {
                mySqlCon.Open();

                string sqlQueryString = " SELECT COMPANY_ID, COMP_NAME FROM COMPANY WHERE STATUS_CODE = '1' && COMPANY_ID = '" + companyId + "'";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQueryString, mySqlCon);
                mySqlDataAdapter.Fill(resultDataTable);

                return resultDataTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                resultDataTable.Dispose();
            }
        }

        public DataTable getDistinctYears()
        {
            DataTable resultDataTable = new DataTable();
            try
            {
                mySqlCon.Open();

                string sqlQueryString = " SELECT DISTINCT YEAR_OF_ASSESSMENT FROM ASSESSMENT WHERE YEAR_OF_ASSESSMENT != 'null'";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQueryString, mySqlCon);
                mySqlDataAdapter.Fill(resultDataTable);
                return resultDataTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                resultDataTable.Dispose();
            }
        }

        /// <summary>
        /// get unclosed assessments for given company and year
        /// </summary>
        /// <returns></returns>
        public DataTable getClosedAndCompletedAssessmentsSummery(string companyId ,string year)
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();

                string sqlQuery = "SELECT ASSESSMENT_ID, STATUS_CODE, count(ASSESSMENT_ID)" +
                                    " FROM ASSESSMENT " +
                                    " WHERE COMPANY_ID = '" + companyId + "' && YEAR_OF_ASSESSMENT ='" + year + "' && STATUS_CODE in ('" + Constants.ASSESSNEMT_CLOSED_STATUS + "','" + Constants.ASSESSNEMT_CEO_FINALIZED_STATUS + "' )" +
                                    " Group by STATUS_CODE ";
                ;
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                return resultTable;
                
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable getUnclosedAndInCompleteAssessmentsSummery(string companyId, string year)
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();

                string sqlQuery = "SELECT ASSESSMENT_ID, STATUS_CODE, count(ASSESSMENT_ID)" +
                                    " FROM ASSESSMENT " +
                                    " WHERE COMPANY_ID = '" + companyId + "' && YEAR_OF_ASSESSMENT ='" + year + "' && STATUS_CODE  NOT IN ('" + Constants.ASSESSNEMT_CLOSED_STATUS + "','" + Constants.ASSESSNEMT_CEO_FINALIZED_STATUS + "' , '" + Constants.ASSESSNEMT_OBSOLETE_STATUS + "' )";
                                    //" Group by STATUS_CODE ";
                ;
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                return resultTable;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable getAssessmentSummery(string companyId, string year)
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                string sqlQuery = "SELECT STATUS_CODE, count(ASSESSMENT_ID)" +
                                        " FROM ASSESSMENT " +
                                        " WHERE COMPANY_ID = '" + companyId + "' && YEAR_OF_ASSESSMENT ='" + year + "' " +
                                        " && STATUS_CODE != '"+Constants.ASSESSNEMT_PENDING_STATUS+"' "+
                                        " Group by STATUS_CODE ";
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);

                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable getAssessmentsByStatus(string status, string companyId, string year)
    {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                //string sqlQuery = "SELECT A.ASSESSMENT_ID, A.ASSESSMENT_NAME,AT.ASSESSMENT_TYPE_NAME, count(AE.ASSESSMENT_ID) as EMP_COUNT " +
                //                  " FROM ASSESSMENT A, ASSESSED_EMPLOYEES AE, ASSESSMENT_TYPE AT" +
                //                  " where A.STATUS_CODE = '"+status+"' && A.ASSESSMENT_ID = AE.ASSESSMENT_ID && A.ASSESSMENT_TYPE_ID = AT.ASSESSMENT_TYPE_ID "+
                //                  " && A.COMPANY_ID ='" +companyId+"' && YEAR_OF_ASSESSMENT ='"+year+"'"+
                //                  " GROUP BY AE.ASSESSMENT_ID ";

                string sql = "SELECT A.ASSESSMENT_ID, A.ASSESSMENT_NAME, AT.ASSESSMENT_TYPE_NAME, count(AE.ASSESSMENT_ID) as COUNT, " +
                            " CASE " +
                                        " when A.STATUS_CODE='" + Constants.ASSESSNEMT_ACTIVE_STATUS + "' then '" + Constants.ASSESSNEMT_ACTIVE_TAG + "' " +
                                        " when A.STATUS_CODE='" + Constants.ASSESSNEMT_PENDING_STATUS + "' then '" + Constants.ASSESSNEMT_PENDING_TAG + "' " +
                                        " when A.STATUS_CODE='" + Constants.ASSESSNEMT_OBSOLETE_STATUS + "' then '" + Constants.ASSESSNEMT_OBSOLETE_TAG + "' " +
                                        " when A.STATUS_CODE='" + Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS + "' then '" + Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG + "' " +
                                        " when A.STATUS_CODE='" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS + "' then '" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG + "' " +
                                        " when A.STATUS_CODE='" + Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS + "' then '" + Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG + "' " +
                                        " when A.STATUS_CODE='" + Constants.ASSESSNEMT_CEO_FINALIZED_STATUS + "' then '" + Constants.ASSESSNEMT_CEO_FINALIZED_TAG + "' " +
                                        " END AS STATUS_CODE " +
                               " FROM ASSESSMENT A " +
                               " LEFT JOIN ASSESSMENT_TYPE AT on A.ASSESSMENT_TYPE_ID = AT.ASSESSMENT_TYPE_ID "+
                               " LEFT JOIN ASSESSED_EMPLOYEES AE on A.ASSESSMENT_ID = AE.ASSESSMENT_ID "; 
                               if(status ==  Constants.ASSESSNEMT_PENDING_STATUS)
                               {
                                   sql += " WHERE A.STATUS_CODE in ('" + Constants.ASSESSNEMT_PENDING_STATUS + "','" + Constants.ASSESSNEMT_ACTIVE_STATUS + "','" + Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS + "','" + Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS + "','" + Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS + "','" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS + "') ";
                               }
                               else if (status == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                               {
                                   sql += " WHERE A.STATUS_CODE = '" + Constants.ASSESSNEMT_CEO_FINALIZED_STATUS + "' ";
                               }
                               sql += "&& A.COMPANY_ID ='" + companyId + "' && A.YEAR_OF_ASSESSMENT ='" + year + "' "; 
                               sql += " GROUP BY A.ASSESSMENT_ID ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sql, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable assessmentStatusSummery(string assessmentId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                string sqlQuery = "SELECT STATUS_CODE, count(EMPLOYEE_ID) " +
                                  " FROM ASSESSED_EMPLOYEES" +
                                  " WHERE ASSESSMENT_ID = '" + assessmentId + "'" +
                                  " GROUP BY STATUS_CODE";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                resultTable.Dispose();
            }
        }

        public bool closeAssessment(string assessmentId, string addedUserId, string reason)
        {
            bool isClosed = false;
            MySqlTransaction mySqlTrans = null;
            try
            {
                mySqlCon.Open();
                string status = Constants.ASSESSNEMT_CLOSED_STATUS;

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentId", assessmentId.Trim() == "" ? (object)DBNull.Value : assessmentId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@reason", reason.Trim() == "" ? (object)DBNull.Value : reason.Trim()));


                string updateAssessmentSql = "UPDATE ASSESSMENT SET " +
                                           " STATUS_CODE = @status, " +
                                           " MODIFIED_BY = @addedUserId, " +
                                           " MODIFIED_DATE = now(), " +
                                           " CLOSING_REASON = @reason " +
                                           " WHERE ASSESSMENT_ID = @assessmentId ";

                mySqlCmd.CommandText = updateAssessmentSql;
                mySqlCmd.ExecuteNonQuery();

                string updateAssessedEmployeesSql = "UPDATE ASSESSED_EMPLOYEES SET " +
                                           " STATUS_CODE = @status, " +
                                           " MODIFIED_BY = @addedUserId, " +
                                           " MODIFIED_DATE = now() " +
                                           " WHERE ASSESSMENT_ID = @assessmentId " +
                                           " AND STATUS_CODE ='" + Constants.ASSESSNEMT_CEO_FINALIZED_STATUS + "' ";

                mySqlCmd.CommandText = updateAssessedEmployeesSql;
                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isClosed = true;


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return isClosed;
        }

        public DataTable getAssessmentCountForDistinctStatusInDistinctActiveAssessments(string companyId, string year)
        {
            DataTable resultTable = new DataTable();

            try
            {
                mySqlCon.Open();
                string sql = @"select A.ASSESSMENT_ID, AE.STATUS_CODE AS STATUS, count(AE.STATUS_CODE) as COUNT FROM ASSESSMENT A
                                LEFT JOIN ASSESSED_EMPLOYEES AE
	                                on A.ASSESSMENT_ID = AE.ASSESSMENT_ID
                                LEFT JOIN EMPLOYEE EM
	                                on AE.EMPLOYEE_ID = EM.EMPLOYEE_ID                                
                                where A.COMPANY_ID = '" + companyId + "' and A.YEAR_OF_ASSESSMENT = '" + year + "' and A.STATUS_CODE ='" + Constants.ASSESSNEMT_ACTIVE_STATUS + "' " +
                                "group by A.ASSESSMENT_ID, AE.STATUS_CODE";
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sql, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }
        /// <summary>
        /// returns "Active" assessments which are not in ceo finalized status
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="year"></param>
        /// <param name="completedAssessmentList"></param>
        /// <returns></returns>
        public DataTable getPendingOrCompletedAssessments(string companyId, string year, List<string> completedAssessmentList, string status)
        {
            DataTable resultTable = new DataTable();
            try
            {                              
                mySqlCon.Open();
                string sql = @"SELECT A.ASSESSMENT_ID, A.ASSESSMENT_NAME, AT.ASSESSMENT_TYPE_NAME, count(AE.ASSESSMENT_ID) as COUNT
                                FROM ASSESSMENT A
                                LEFT JOIN ASSESSMENT_TYPE AT on A.ASSESSMENT_TYPE_ID = AT.ASSESSMENT_TYPE_ID
                                LEFT JOIN ASSESSED_EMPLOYEES AE on A.ASSESSMENT_ID = AE.ASSESSMENT_ID
                                where A.STATUS_CODE ='1'
                                ";

                if (completedAssessmentList.Count > 0)
                {
                    int index = 0;
                    if (status == Constants.ASSESSNEMT_PENDING_STATUS)
                    {
                        sql += " and A.ASSESSMENT_ID not in (";
                    }
                    else if (status == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        sql += " and A.ASSESSMENT_ID in (";
                    }
                    foreach (var item in completedAssessmentList)
                    {
                        sql += "'" + item.ToString() + "' ";
                        if (index < completedAssessmentList.Count - 1)
                        {
                            sql += ",";
                            index++;
                        }
                        
                    }
                    sql += ") ";
                }

                sql += " and A.COMPANY_ID ='"+companyId+"' and A.YEAR_OF_ASSESSMENT ='"+year+"' "+
                         " GROUP BY A.ASSESSMENT_ID";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sql, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        
    }
}
