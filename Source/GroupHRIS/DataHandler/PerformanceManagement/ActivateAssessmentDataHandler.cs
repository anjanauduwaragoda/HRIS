using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Common;
using MySql.Data;
using MySql.Data.MySqlClient;
using NLog;
namespace DataHandler.PerformanceManagement
{
    public class ActivateAssessmentDataHandler : TemplateDataHandler
    {
        public DataTable getAllActiveAssessmentTypes()
        {
            DataTable assessmentTypeDataTable = new DataTable();

            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
               // {
                    mySqlCon.Open();
              //  }

                string sqlQueryString = " SELECT ASSESSMENT_TYPE_ID, ASSESSMENT_TYPE_NAME " +
                                        " FROM ASSESSMENT_TYPE " +
                                        " WHERE STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQueryString, mySqlCon);
                mySqlDataAdapter.Fill(assessmentTypeDataTable);

                return assessmentTypeDataTable;

            }
            catch (Exception e)
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

        public DataTable getAllActiveAssessmentPurpose()
        {
            DataTable assessmentPurposeDataTable = new DataTable();
            try
            {
               /// if (mySqlCon.State == ConnectionState.Closed)
               // {
                    mySqlCon.Open();
               // }

                string sqlQueryString = " SELECT * FROM ASSESSMENT_PURPOSE WHERE STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "'";
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQueryString, mySqlCon);
                mySqlDataAdapter.Fill(assessmentPurposeDataTable);
                return assessmentPurposeDataTable;
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

        public DataRow getPurposeDetailsById(string assessmentPurposeId) // returns active purposes only
        {
            try
            {
                DataRow purposeDetails = null;
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}

                string sqlQueryString = "SELECT NAME, DESCRIPTION FROM ASSESSMENT_PURPOSE " +
                                        " WHERE PURPOSE_ID ='" + assessmentPurposeId + "' AND STATUS_CODE = '1'";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQueryString, mySqlCon);

                DataTable assessmentPurposeDataTable = new DataTable();
                mySqlDataAdapter.Fill(assessmentPurposeDataTable);

                if (assessmentPurposeDataTable.Rows.Count > 0)
                {
                    purposeDetails = assessmentPurposeDataTable.Rows[0];
                }

                mySqlDataAdapter.Dispose();
                return purposeDetails;
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

        /////////////////////////////////   Assessed Employees ////////////////////////

        public DataTable getAllActiveCompanies()
        {
            DataTable resultDataTable = new DataTable();
            try
            {
               // if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
               // }

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

            }
        }

        public DataTable getDistinctYears()
        {
            DataTable resultDataTable = new DataTable();
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
               // {
                    mySqlCon.Open();
               // }

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

            }
        }

        public DataTable getCompanyById(string companyId)
        {
            DataTable resultDataTable = new DataTable();
            try
            {
               // if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
               // }

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

            }
        }

        public DataTable getActiveDepartmentsForCompany(string selectedCompanyId)
        {
            DataTable resultDataTable = new DataTable();
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string sqlQueryString = " SELECT DEPT_ID, DEPT_NAME FROM DEPARTMENT WHERE COMPANY_ID ='" + selectedCompanyId + "' AND STATUS_CODE = '1' ";

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

            }
        }

        public DataTable getActiveDevisionsForDepartment(string selectedDepartmentId)
        {
            DataTable resultDataTable = new DataTable();
            try
            {
               // if (mySqlCon.State == ConnectionState.Closed)
              //  {
                    mySqlCon.Open();
              //  }
            
                string sqlQueryString = " SELECT DIVISION_ID, DIV_NAME FROM DIVISION WHERE DEPT_ID ='" + selectedDepartmentId + "' AND STATUS_CODE = '1' ";

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

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="purposeDataTable"></param>
        /// <param name="assessedEmployees"></param>
        /// <param name="assessmentName"></param>
        /// <param name="assessmentType"></param>
        /// <param name="remarks"></param>
        /// <param name="year"></param>
        /// <param name="status"></param>
        /// <param name="addedUserId"></param>
        /// <returns></returns>
        //public Boolean insert(DataTable purposeDataTable, DataTable assessedEmployees, string assessmentName, string assessmentType, string remarks, string year, string status, string addedUserId, string companyId)
        //{
        //    Boolean inserted = false;
        //    MySqlTransaction mySqlTrans = null;
        //    string assessmentInsertString = null;
        //    string purposesInsertString = null;
        //    string employeeInsertString = null;

        //    try
        //    {

        //        if (mySqlCon.State == ConnectionState.Closed)
        //        {
        //            mySqlCon.Open();
        //        }
        //        mySqlTrans = mySqlCon.BeginTransaction();

        //        SerialHandler serialCode = new SerialHandler();
        //        string assessmentId = serialCode.getserila(mySqlCon, "AS");

        //        mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentId", assessmentId.Trim() == "" ? (object)DBNull.Value : assessmentId.Trim()));
        //        mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentName", assessmentName.Trim() == "" ? (object)DBNull.Value : assessmentName.Trim()));
        //        mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentType", assessmentType.Trim() == "" ? (object)DBNull.Value : assessmentType.Trim()));
        //        mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
        //        mySqlCmd.Parameters.Add(new MySqlParameter("@year", year.Trim() == "" ? (object)DBNull.Value : year.Trim()));
        //        mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
        //        mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));
        //        mySqlCmd.Parameters.Add(new MySqlParameter("@companyId", companyId.Trim() == "" ? (object)DBNull.Value : companyId.Trim()));

        //        assessmentInsertString = "INSERT INTO ASSESSMENT (ASSESSMENT_ID, ASSESSMENT_TYPE_ID, COMPANY_ID, ASSESSMENT_NAME, REMARKS, YEAR_OF_ASSESSMENT, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE) " +
        //                                 "VALUES(@assessmentId,@assessmentType, @companyId, @assessmentName,@remarks,@year,@status,@addedUserId,now(),@addedUserId,now())";

        //        mySqlCmd.Transaction = mySqlTrans;
        //        mySqlCmd.CommandText = assessmentInsertString;
        //        mySqlCmd.ExecuteNonQuery();

        //        ///////////////////////////// adding data to assessment purposes /////////////////////////

        //        foreach (DataRow purpose in purposeDataTable.Rows)
        //        {
        //            mySqlCmd.Parameters.Clear();

        //            string purposeId = purpose[1].ToString();
        //            string purposeStatus = null;
        //            if (purpose[3].ToString() == "Active")
        //            {
        //                purposeStatus = "1";
        //            }
        //            else if (purpose[3].ToString() == "Inactive")
        //            {
        //                purposeStatus = "0";
        //            }

        //            mySqlCmd.Parameters.Add(new MySqlParameter("@purposeId", purposeId.Trim() == "" ? (object)DBNull.Value : purposeId.Trim()));
        //            mySqlCmd.Parameters.Add(new MySqlParameter("@purposeStatus", purposeStatus.Trim() == "" ? (object)DBNull.Value : purposeStatus.Trim()));
        //            mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentId", assessmentId.Trim() == "" ? (object)DBNull.Value : assessmentId.Trim()));
        //            mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

        //            purposesInsertString = "INSERT INTO ASSESSMENT_PURPOSES (PURPOSE_ID, ASSESSMENT_ID, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE) " +
        //                                    " VALUES (@purposeId, @assessmentId, @purposeStatus, @addedUserId,now(),@addedUserId,now())";

        //            mySqlCmd.CommandText = purposesInsertString;
        //            mySqlCmd.ExecuteNonQuery();
        //        }

        //        foreach (DataRow employee in assessedEmployees.Rows)
        //        {
        //            mySqlCmd.Parameters.Clear();
                    

        //            string employeeId = employee["emp_id"].ToString();
        //            string company = employee["company_id"].ToString();
        //            string dept = employee["dept_id"].ToString();
        //            string division = employee["division_id"].ToString();
        //            string epf = employee["epf_no"].ToString();
        //            string goal = employee["goal"].ToString();
        //            string self = employee["self"].ToString();
        //            string competency = employee["competency"].ToString();
        //            string assessedEmployeeStatus = "1";

        //            mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentId", assessmentId.Trim() == "" ? (object)DBNull.Value : assessmentId.Trim()));
        //            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
        //            mySqlCmd.Parameters.Add(new MySqlParameter("@company", company.Trim() == "" ? (object)DBNull.Value : company.Trim()));
        //            mySqlCmd.Parameters.Add(new MySqlParameter("@dept", dept.Trim() == "" ? (object)DBNull.Value : dept.Trim()));
        //            mySqlCmd.Parameters.Add(new MySqlParameter("@division", division.Trim() == "" ? (object)DBNull.Value : division.Trim()));
        //            mySqlCmd.Parameters.Add(new MySqlParameter("@goal", goal.Trim() == "" ? (object)DBNull.Value : goal.Trim()));
        //            mySqlCmd.Parameters.Add(new MySqlParameter("@self", self.Trim() == "" ? (object)DBNull.Value : self.Trim()));
        //            mySqlCmd.Parameters.Add(new MySqlParameter("@competency", competency.Trim() == "" ? (object)DBNull.Value : competency.Trim()));
        //            mySqlCmd.Parameters.Add(new MySqlParameter("@epf", epf.Trim() == "" ? (object)DBNull.Value : epf.Trim()));

        //            mySqlCmd.Parameters.Add(new MySqlParameter("@year", year.Trim() == "" ? (object)DBNull.Value : year.Trim()));
        //            mySqlCmd.Parameters.Add(new MySqlParameter("@assessedEmployeeStatus", assessedEmployeeStatus.Trim() == "" ? (object)DBNull.Value : assessedEmployeeStatus.Trim()));
        //            mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

        //            employeeInsertString = "INSERT INTO ASSESSED_EMPLOYEES (ASSESSMENT_ID, EMPLOYEE_ID, COMPANY, DEPARTMENT, DIVISION, YEAR_OF_ASSESSMENT, INCLUDE_SELF_ASSESSMENT, INCLUDE_COMPITANCY_ASSESSMENT, INCLUDE_GOAL_ASSESSMENT, EPF_NO, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE)" +
        //                                   " VALUES( @assessmentId, @employeeId, @company, @dept, @division, @year, @self, @competency, @goal, @epf, @assessedEmployeeStatus, @addedUserId,now(),@addedUserId,now())";

        //            mySqlCmd.CommandText = employeeInsertString;
        //            mySqlCmd.ExecuteNonQuery();

        //        }

        //        mySqlTrans.Commit();
        //        mySqlCon.Close();
        //        mySqlTrans.Dispose();
        //        mySqlCmd.Dispose();

        //        inserted = true;
        //    }
        //    catch (Exception e)
        //    {
        //        mySqlTrans.Rollback();
        //        inserted = false;
        //        throw e;
        //    }





        //    return inserted;
        //}

        public Boolean update(string assessmentId, DataTable purposeDataTable, DataTable assessedEmployees, string assessmentName, string assessmentType, string remarks, string year, string status, string addedUserId, string companyId)
        {
            Boolean updated = false;
            MySqlTransaction mySqlTrans = null;
            string assessmentUpdateString = null;
            string purposesUpdateString = null;
            //string employeeUpdateString = null;

            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                mySqlCon.Open();
                // }

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentId", assessmentId.Trim() == "" ? (object)DBNull.Value : assessmentId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentName", assessmentName.Trim() == "" ? (object)DBNull.Value : assessmentName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentType", assessmentType.Trim() == "" ? (object)DBNull.Value : assessmentType.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@year", year.Trim() == "" ? (object)DBNull.Value : year.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@companyId", companyId.Trim() == "" ? (object)DBNull.Value : companyId.Trim()));
                assessmentUpdateString = "UPDATE ASSESSMENT SET " +
                                           " ASSESSMENT_TYPE_ID = @assessmentType, " +
                                           " COMPANY_ID = @companyId, " +
                                           " ASSESSMENT_NAME = @assessmentName, " +
                                           " REMARKS = @remarks, " +
                                           " YEAR_OF_ASSESSMENT = @year, " +
                                           " STATUS_CODE = @status, " +
                                           " MODIFIED_BY = @addedUserId, " +
                                           " MODIFIED_DATE = now() " +
                                           " WHERE ASSESSMENT_ID = @assessmentId ";

                mySqlCmd.CommandText = assessmentUpdateString;
                mySqlCmd.ExecuteNonQuery();


                foreach (DataRow purpose in purposeDataTable.Rows)
                {
                    mySqlCmd.Parameters.Clear();

                    string purposeId = purpose[0].ToString();
                    string purposeStatus = null;
                    if (purpose[2].ToString() == "Active")
                    {
                        purposeStatus = "1";
                    }
                    else if (purpose[2].ToString() == "Inactive")
                    {
                        purposeStatus = "0";
                    }

                    mySqlCmd.Parameters.Add(new MySqlParameter("@purposeId", purposeId.Trim() == "" ? (object)DBNull.Value : purposeId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@purposeStatus", purposeStatus.Trim() == "" ? (object)DBNull.Value : purposeStatus.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentId", assessmentId.Trim() == "" ? (object)DBNull.Value : assessmentId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                    purposesUpdateString = "INSERT INTO ASSESSMENT_PURPOSES (PURPOSE_ID, ASSESSMENT_ID, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE) " +
                                            " VALUES (@purposeId, @assessmentId, @purposeStatus, @addedUserId,now(),@addedUserId,now())";

                    Boolean isExists = CheckPurposeExistanceForAssessment(assessmentId, purposeId);
                    if (isExists)
                    {
                        purposesUpdateString = "UPDATE ASSESSMENT_PURPOSES SET " +
                                               " ASSESSMENT_ID = @assessmentId, " +
                                               " STATUS_CODE = @purposeStatus, " +
                                               " MODIFIED_BY = @assessmentId, " +
                                               " MODIFIED_DATE = now() " +
                                               " WHERE ASSESSMENT_ID = @assessmentId AND PURPOSE_ID = @purposeId";
                    }
                    else if (isExists == false)
                    {
                        purposesUpdateString = "INSERT INTO ASSESSMENT_PURPOSES (PURPOSE_ID, ASSESSMENT_ID, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE) " +
                                                " VALUES (@purposeId, @assessmentId, @purposeStatus, @addedUserId,now(),@addedUserId,now())";
                    }
                    mySqlCmd.CommandText = purposesUpdateString;
                    mySqlCmd.ExecuteNonQuery();
                }

                deleteAllAssessedEmployees(assessmentId);

                foreach (DataRow employee in assessedEmployees.Rows)
                {
                    mySqlCmd.Parameters.Clear();

                    string employeeId = employee["emp_id"].ToString();
                    string company = employee["company_id"].ToString();
                    string dept = employee["dept_id"].ToString();
                    string division = employee["division_id"].ToString();
                    string epf = employee["epf_no"].ToString();
                    string goal = employee["goal"].ToString();
                    string self = employee["self"].ToString();
                    string competency = employee["competency"].ToString();
                    string assessedEmployeeStatus = "1";

                    mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentId", assessmentId.Trim() == "" ? (object)DBNull.Value : assessmentId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@company", company.Trim() == "" ? (object)DBNull.Value : company.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@dept", dept.Trim() == "" ? (object)DBNull.Value : dept.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@division", division.Trim() == "" ? (object)DBNull.Value : division.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@goal", goal.Trim() == "" ? (object)DBNull.Value : goal.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@self", self.Trim() == "" ? (object)DBNull.Value : self.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@competency", competency.Trim() == "" ? (object)DBNull.Value : competency.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@epf", epf.Trim() == "" ? (object)DBNull.Value : epf.Trim()));

                    mySqlCmd.Parameters.Add(new MySqlParameter("@year", year.Trim() == "" ? (object)DBNull.Value : year.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@assessedEmployeeStatus", assessedEmployeeStatus.Trim() == "" ? (object)DBNull.Value : assessedEmployeeStatus.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));



                    string employeeInsertString = "INSERT INTO ASSESSED_EMPLOYEES (ASSESSMENT_ID, EMPLOYEE_ID, COMPANY, DEPARTMENT, DIVISION, YEAR_OF_ASSESSMENT, INCLUDE_SELF_ASSESSMENT, INCLUDE_COMPITANCY_ASSESSMENT, INCLUDE_GOAL_ASSESSMENT, EPF_NO, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE)" +
                                           " VALUES( @assessmentId, @employeeId, @company, @dept, @division, @year, @self, @competency, @goal, @epf, @assessedEmployeeStatus, @addedUserId,now(),@addedUserId,now())";

                    mySqlCmd.CommandText = employeeInsertString;
                    mySqlCmd.ExecuteNonQuery();


                }
                mySqlCmd.Transaction = mySqlTrans;
                mySqlTrans.Commit();
                mySqlCon.Close();

                return true;
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

        public Boolean CheckPurposeExistanceForAssessment(string assessmentId, string purposeId)
        {
            Boolean isExists = false;
            string sqlQuery = " SELECT * FROM ASSESSMENT_PURPOSES WHERE ASSESSMENT_ID ='" + assessmentId + "' AND PURPOSE_ID ='" + purposeId + "'";
            //if(mySqlCon.State == ConnectionState.Closed)
            //{
                mySqlCon.Open();
            //}
            DataTable resultTable = new DataTable();
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
            mySqlDataAdapter.Fill(resultTable);

            if (resultTable.Rows.Count > 0)
            {
                isExists = true;
            }

            return isExists;
        }

        public DataTable getAllAssessments()
        {
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                mySqlCon.Open();
                //}
                string sqlQueryString = " SELECT A.ASSESSMENT_ID, A.ASSESSMENT_NAME, A.YEAR_OF_ASSESSMENT, AT.ASSESSMENT_TYPE_NAME, AT.ASSESSMENT_TYPE_ID, A.COMPANY_ID, " +
                                        " CASE " +
                                        " when A.STATUS_CODE='" + Constants.ASSESSNEMT_ACTIVE_STATUS + "' then '" + Constants.ASSESSNEMT_ACTIVE_TAG + "' " +
                                        " when A.STATUS_CODE='" + Constants.ASSESSNEMT_PENDING_STATUS + "' then '" + Constants.ASSESSNEMT_PENDING_TAG + "' " +
                                        " when A.STATUS_CODE='" + Constants.ASSESSNEMT_OBSOLETE_STATUS + "' then '" + Constants.ASSESSNEMT_OBSOLETE_TAG + "' " +
                                        " when A.STATUS_CODE='" + Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS + "' then '" + Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG + "' " +
                                        " when A.STATUS_CODE='" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS + "' then '" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG + "' " +
                                        " when A.STATUS_CODE='" + Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS + "' then '" + Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG + "' " +
                                        " when A.STATUS_CODE='" + Constants.ASSESSNEMT_CEO_FINALIZED_STATUS + "' then '" + Constants.ASSESSNEMT_CEO_FINALIZED_TAG + "' " +
                                        " END AS STATUS_CODE " +
                                        " FROM ASSESSMENT A, ASSESSMENT_TYPE AT WHERE A.ASSESSMENT_TYPE_ID = AT.ASSESSMENT_TYPE_ID && " + "A.STATUS_CODE='" + Constants.ASSESSNEMT_PENDING_STATUS + "' " +
                                        " ORDER BY A.ASSESSMENT_ID DESC";

                DataTable resultDataTable = new DataTable();
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQueryString, mySqlCon);
                mySqlDataAdapter.Fill(resultDataTable);

                return resultDataTable;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
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

        public DataTable getAssessmentById(string assessmentId)
        {
            try
            {
                // if (mySqlCon.State == ConnectionState.Closed)
                // {
                mySqlCon.Open();
                // }
                string sqlQueryString = " SELECT A.ASSESSMENT_ID, A.ASSESSMENT_NAME, A.COMPANY_ID, A.YEAR_OF_ASSESSMENT, A.REMARKS, AT.ASSESSMENT_TYPE_NAME, AT.ASSESSMENT_TYPE_ID, A.STATUS_CODE " +

                                        " FROM ASSESSMENT A, ASSESSMENT_TYPE AT WHERE ASSESSMENT_ID='" + assessmentId + "' AND A.ASSESSMENT_TYPE_ID = AT.ASSESSMENT_TYPE_ID ";

                DataTable resultDataTable = new DataTable();
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQueryString, mySqlCon);
                mySqlDataAdapter.Fill(resultDataTable);

                return resultDataTable;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
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

        public DataTable getPurposesForAssessment(string assessmentId)
        {
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                // {
                mySqlCon.Open();
                //}

                string sqlQueryString = "SELECT AP.PURPOSE_ID, AP.NAME, " +
                                        " CASE " +
                                        " when APS.STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then 'Active' " +
                                        " when APS.STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then 'Inactive' " +
                                        " END AS STATUS_CODE" +
                                        " FROM ASSESSMENT_PURPOSE AP, ASSESSMENT_PURPOSES APS " +
                                        " WHERE APS.PURPOSE_ID = AP.PURPOSE_ID && APS.ASSESSMENT_ID = '" + assessmentId + "'";

                DataTable resultDataTable = new DataTable();
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
            }
        }

        public DataTable getEmployeesForAssessment(string assessmentId)
        {
            DataTable resultDataTable = new DataTable();
            try
            {
               // if (mySqlCon.State == ConnectionState.Closed)
               // {
                    mySqlCon.Open();
                //}

                //string sqlQueryString = "SELECT AE.EMPLOYEE_ID as emp_id, AE.COMPANY as company_id, AE.DEPARTMENT as dept_id, AE.DIVISION as division_id, E.EPF_NO as epf_no, E.INITIALS_NAME as emp_name, AE.STATUS_CODE as exclude" +
                //                        " FROM ASSESSED_EMPLOYEES AE, EMPLOYEE E WHERE AE.EMPLOYEE_ID = E.EMPLOYEE_ID AND " +
                //                        " AE.ASSESSMENT_ID = '" + assessmentId + "' AND " +
                //                        " (AE.STATUS_CODE = '1' OR AE.STATUS_CODE = '0')";

                string sqlQueryString = "SELECT AE.EMPLOYEE_ID , AE.COMPANY as COMPANY_ID, AE.DEPARTMENT as DEPT_ID, AE.DIVISION as DIVISION_ID,E.EMAIL, E.EPF_NO, E.KNOWN_NAME as KNOWN_NAME, 1 as INCLUDE, AE.INCLUDE_GOAL_ASSESSMENT as GOAL, AE.INCLUDE_SELF_ASSESSMENT as SELF, AE.INCLUDE_COMPITANCY_ASSESSMENT as COMPETENCY " +
                                        " FROM ASSESSED_EMPLOYEES AE, EMPLOYEE E WHERE AE.EMPLOYEE_ID = E.EMPLOYEE_ID AND " +
                                        " AE.ASSESSMENT_ID = '" + assessmentId + "' AND " +
                                        " (AE.STATUS_CODE = '1' OR AE.STATUS_CODE = '0')"+
                                        " order by CAST(E.EPF_NO AS unsigned) asc ";

                
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

        public void deleteAllAssessedEmployees(string assessmentId)
        {

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string sqlString = " DELETE FROM ASSESSED_EMPLOYEES WHERE ASSESSMENT_ID ='" + assessmentId + "'";

                mySqlCmd.CommandText = sqlString;
                mySqlCmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                
                throw;
            }

        }

        public Boolean checkAssessmentNameExistance(string assessmentName)
        {
            try
            {
                Boolean isExsists = false;
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                dataTable.Rows.Clear();


                string queryStr = "SELECT * FROM ASSESSMENT WHERE ASSESSMENT_NAME ='" + assessmentName + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExsists = true;
                }
                return isExsists;
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

        public Boolean checkAssessmentNameExistance(string assessmentName, string id)
        {

            dataTable.Rows.Clear();
            Boolean isExsists = false;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string queryStr = "SELECT * FROM ASSESSMENT WHERE ASSESSMENT_NAME ='" + assessmentName + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["ASSESSMENT_ID"].ToString() == id)
                        {
                            isExsists = false;
                        }
                        else
                        {
                            isExsists = true;
                        }
                    }
                }
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

            return isExsists;
        }

        public bool ActivateAssessment(string assessmentId, DataTable assessedEmployeesDataTable, string addedUser)
        {
            bool isActivated = false;
            MySqlTransaction mySqlTrans = null;
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string assessmentUpdateQuery = " UPDATE ASSESSMENT SET " +
                                //" EXPECTED_COMPLETION_DATE = '" + expectedCompletionDate + "', " +
                                " STATUS_CODE = '" + Constants.ASSESSNEMT_ACTIVE_STATUS + "', " +
                                " MODIFIED_BY = '"+addedUser+"', " +
                                " MODIFIED_DATE = now() " +
                                " WHERE ASSESSMENT_ID = '" + assessmentId + "' ";

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.CommandText = assessmentUpdateQuery;
                mySqlCmd.ExecuteNonQuery();

                string updateEmployeeStatusQuery =  " UPDATE ASSESSED_EMPLOYEES SET "+
                                                        " STATUS_CODE = '" + Constants.ASSESSNEMT_ACTIVE_STATUS + "', " +
                                                        " MODIFIED_BY = '" + addedUser + "', " +
                                                        " MODIFIED_DATE = now() " +
                                                        " WHERE ASSESSMENT_ID = '" + assessmentId + "' ";

                mySqlCmd.CommandText = updateEmployeeStatusQuery;
                mySqlCmd.ExecuteNonQuery();

                mySqlCmd.Transaction = mySqlTrans;
                mySqlTrans.Commit();
                mySqlCon.Close();

                isActivated = true;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return isActivated;
        }

        public DataTable getPendingAssessmentsSummery(string year, string company)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                
                string queryString = "select A.COMPANY_ID, C.COMP_NAME, A.YEAR_OF_ASSESSMENT, count(A.ASSESSMENT_ID) as PENDING " +
                                     "from ASSESSMENT A, COMPANY C " +
                                     //"where A.STATUS_CODE = 0 and A.COMPANY_ID = C.COMPANY_ID and A.COMPANY_ID ='" + company + "' and A.YEAR_OF_ASSESSMENT ='"+year+"' " +
                                     "where A.STATUS_CODE = 0 and A.COMPANY_ID = C.COMPANY_ID ";
                if (!String.IsNullOrEmpty(year))
                {
                    queryString += " and A.YEAR_OF_ASSESSMENT ='" + year + "' "; 
                }
                if (!String.IsNullOrEmpty(company))
                {
                    queryString += "and A.COMPANY_ID ='" + company + "' ";
                }

                queryString += "group by A.COMPANY_ID, A.YEAR_OF_ASSESSMENT " +
                                     "order by A.YEAR_OF_ASSESSMENT desc ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(queryString, mySqlCon);
                DataTable resultTable = new DataTable();
                mySqlDataAdapter.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
            }
        }
    }
}
