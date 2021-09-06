using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainingRequestDataHandler:TemplateDataHandler
    {
        public Boolean Insert(String sCategoryId,
                              String sSubcategoryId,
                              String sCompanyId,
                              string sDeptId,
                              string sDivId,
                              string sBranchId,
                              string sRequestTypeId,
                              string sRequestedBy,
                              string sDesignation,
                              string sEmail,
                              string sRequestReason,
                              string sDescription,
                              string sSkillsExpected,
                              Int32  iNoOfParticipants,
                              string sRequestedDate,
                              string sRemarks,
                              string sToRecommend,
                              string sToApprove,
                              string sFinancialYear,
                              string sStatus,
                              String addedBy)
        {
            Boolean blInserted = false;

            string sMySqlString = "";
            string sRequestId = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@CategoryId", sCategoryId.Trim() == "" ? (object)DBNull.Value : sCategoryId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@SubcategoryId", sSubcategoryId.Trim() == "" ? (object)DBNull.Value : sSubcategoryId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@CompanyId", sCompanyId.Trim() == "" ? (object)DBNull.Value : sCompanyId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@DeptId", sDeptId.Trim() == "" ? (object)DBNull.Value : sDeptId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@DivId", sDivId.Trim() == "" ? (object)DBNull.Value : sDivId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@BranchId", sBranchId.Trim() == "" ? (object)DBNull.Value : sBranchId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RequestTypeId", sRequestTypeId.Trim() == "" ? (object)DBNull.Value : sRequestTypeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RequestedBy", sRequestedBy.Trim() == "" ? (object)DBNull.Value : sRequestedBy.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Designation", sDesignation.Trim() == "" ? (object)DBNull.Value : sDesignation.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Email", sEmail.Trim() == "" ? (object)DBNull.Value : sEmail.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RequestReason", sRequestReason.Trim() == "" ? (object)DBNull.Value : sRequestReason.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Description", sDescription.Trim() == "" ? (object)DBNull.Value : sDescription.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@SkillsExpected", sSkillsExpected.Trim() == "" ? (object)DBNull.Value : sSkillsExpected.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@NoOfParticipants", iNoOfParticipants));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RequestedDate", sRequestedDate.Trim() == "" ? (object)DBNull.Value : sRequestedDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Remarks", sRemarks.Trim() == "" ? (object)DBNull.Value : sRemarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ToRecommend", sToRecommend.Trim() == "" ? (object)DBNull.Value : sToRecommend.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ToApprove", sToApprove.Trim() == "" ? (object)DBNull.Value : sToApprove.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Status", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@FinancialYear", sFinancialYear.Trim() == "" ? (object)DBNull.Value : sFinancialYear.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler serialHandler = new SerialHandler();
                sRequestId = serialHandler.getserilalReference(ref mySqlCon, "TR");

                mySqlCmd.Parameters.Add(new MySqlParameter("@RequestId", sRequestId.Trim() == "" ? (object)DBNull.Value : sRequestId.Trim()));

                sMySqlString = " INSERT INTO TRAINING_REQUEST(REQUEST_ID,TRAINING_CATEGORY,TRAINING_SUB_CATEGORY_ID,COMPANY_ID,DEPARTMENT_ID,DIVISION_ID,BRANCH_ID,REQUEST_TYPE,REQUESTED_BY, " +
                               " DESIGNATION,EMAIL,REASON,DESCRIPTION_OF_TRAINING,SKILLS_EXPECTED,NUMBER_OF_PARTICIPANTS,REQUESTED_DATE,REMARKS,TO_RECOMMEND, " + 
                               " TO_APPROVE,FINANCIAL_YEAR,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) " +
                               " VALUES(@RequestId,@CategoryId,@SubcategoryId,@CompanyId,@DeptId,@DivId,@BranchId,@RequestTypeId," +
                               "@RequestedBy,@Designation,@Email,@RequestReason,@Description,@SkillsExpected,@NoOfParticipants,@RequestedDate," +
                               "@Remarks,@ToRecommend,@ToApprove,@FinancialYear,@Status,@addedBy,now(),@addedBy,now())";


                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                serialHandler = null;

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


        public Boolean Update(string sRequestId,
                              string sCategoryId,
                              string sSubcategoryId,
                              string sCompanyId,
                              string sDeptId,
                              string sDivId,
                              string sBranchId,
                              string sRequestTypeId,     
                              string sRequestReason,
                              string sDescription,
                              string sSkillsExpected,
                              Int32 iNoOfParticipants,
                              string sRequestedDate,
                              string sRemarks,
                              string sToRecommend,
                              string sToApprove,
                              string sFinancialYear,
                              string sStatus,
                              string addedBy,
                              string isApproved,
                              string sApprovedReason,
                              string sApprovedDate,
                              string sApprovedBy,
                              string isRecommended,
                              string sRecommendedReason,
                              string sRecommendedDate,
                              string sRecommendedBy,
                              Boolean bRecommend,
                              Boolean bApprove)
        {
            Boolean blInserted = false;

            string sMySqlString = "";
            string sMySqlStringRecommend = "";
            string sMySqlStringApprove = "";            

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@CategoryId", sCategoryId.Trim() == "" ? (object)DBNull.Value : sCategoryId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@SubcategoryId", sSubcategoryId.Trim() == "" ? (object)DBNull.Value : sSubcategoryId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@CompanyId", sCompanyId.Trim() == "" ? (object)DBNull.Value : sCompanyId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@DeptId", sDeptId.Trim() == "" ? (object)DBNull.Value : sDeptId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@DivId", sDivId.Trim() == "" ? (object)DBNull.Value : sDivId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@BranchId", sBranchId.Trim() == "" ? (object)DBNull.Value : sBranchId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RequestTypeId", sRequestTypeId.Trim() == "" ? (object)DBNull.Value : sRequestTypeId.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@RequestedBy", sRequestedBy.Trim() == "" ? (object)DBNull.Value : sRequestedBy.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@Designation", sDesignation.Trim() == "" ? (object)DBNull.Value : sDesignation.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@Email", sEmail.Trim() == "" ? (object)DBNull.Value : sEmail.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RequestReason", sRequestReason.Trim() == "" ? (object)DBNull.Value : sRequestReason.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Description", sDescription.Trim() == "" ? (object)DBNull.Value : sDescription.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@SkillsExpected", sSkillsExpected.Trim() == "" ? (object)DBNull.Value : sSkillsExpected.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@NoOfParticipants", iNoOfParticipants));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RequestedDate", sRequestedDate.Trim() == "" ? (object)DBNull.Value : sRequestedDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Remarks", sRemarks.Trim() == "" ? (object)DBNull.Value : sRemarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ToRecommend", sToRecommend.Trim() == "" ? (object)DBNull.Value : sToRecommend.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ToApprove", sToApprove.Trim() == "" ? (object)DBNull.Value : sToApprove.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Status", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@FinancialYear", sFinancialYear.Trim() == "" ? (object)DBNull.Value : sFinancialYear.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RequestId", sRequestId.Trim() == "" ? (object)DBNull.Value : sRequestId.Trim()));

                // recommendation
                mySqlCmd.Parameters.Add(new MySqlParameter("@IsRecommended", isRecommended.Trim() == "" ? (object)DBNull.Value : isRecommended.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RecommendedReason", sRecommendedReason.Trim() == "" ? (object)DBNull.Value : sRecommendedReason.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RecommendedDate", sRecommendedDate.Trim() == "" ? (object)DBNull.Value : sRecommendedDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@RecommendedBy", sRecommendedBy.Trim() == "" ? (object)DBNull.Value : sRecommendedBy.Trim()));

                // Approval 
                mySqlCmd.Parameters.Add(new MySqlParameter("@IsApproved", isApproved.Trim() == "" ? (object)DBNull.Value : isApproved.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ApprovedReason", sApprovedReason.Trim() == "" ? (object)DBNull.Value : sApprovedReason.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ApprovedDate", sApprovedDate.Trim() == "" ? (object)DBNull.Value : sApprovedDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@ApprovedBy", sApprovedBy.Trim() == "" ? (object)DBNull.Value : sApprovedBy.Trim()));

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                sMySqlString =  " UPDATE TRAINING_REQUEST set TRAINING_CATEGORY = @CategoryId ,TRAINING_SUB_CATEGORY_ID = @SubcategoryId,COMPANY_ID= @CompanyId,DEPARTMENT_ID=@DeptId," +
                                " DIVISION_ID=@DivId,BRANCH_ID=@BranchId,REQUEST_TYPE=@RequestTypeId, " +
                                " REASON=@RequestReason,DESCRIPTION_OF_TRAINING=@Description," +
                                " SKILLS_EXPECTED=@SkillsExpected,NUMBER_OF_PARTICIPANTS=@NoOfParticipants,REQUESTED_DATE=@RequestedDate," +
                                " REMARKS=@Remarks,TO_RECOMMEND=@ToRecommend," +
                                " TO_APPROVE=@ToApprove,FINANCIAL_YEAR=@FinancialYear,STATUS_CODE=@Status," +
                                " MODIFIED_BY=@addedBy,MODIFIED_DATE=now()" +
                                " WHERE REQUEST_ID = @RequestId";


                mySqlCmd.CommandText = sMySqlString;
                
                mySqlCmd.ExecuteNonQuery();

                if (bRecommend == true)
                {

                    sMySqlStringRecommend = "UPDATE TRAINING_REQUEST set IS_RECOMENDED =@IsRecommended,RECOMENDED_REASON =@RecommendedReason,RECOMENDED_DATE = @RecommendedDate," +
                                            "RECOMMENDED_BY=@RecommendedBy where REQUEST_ID = @RequestId";

                    mySqlCmd.CommandText = sMySqlStringRecommend;

                    mySqlCmd.ExecuteNonQuery();

                }

                if (bApprove == true)
                {
                    sMySqlStringApprove = "UPDATE TRAINING_REQUEST set IS_APPROVED =@IsApproved,APPROVED_REASON =@ApprovedReason,APPROVED_DATE =@ApprovedDate," +
                                          "APPROVED_BY=@ApprovedBy where REQUEST_ID = @RequestId";

                    mySqlCmd.CommandText = sMySqlStringApprove;

                    mySqlCmd.ExecuteNonQuery();

                }


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


        public DataRow getRecommendedApprovedPerson(string requestId)
        {
            try
            {
                dataTable.Clear();
                string sMySqlString = " SELECT TO_RECOMMEND,TO_APPROVE,IFNULL(IS_RECOMENDED,'') IS_RECOMENDED,IFNULL(IS_APPROVED,'') IS_APPROVED FROM TRAINING_REQUEST where REQUEST_ID ='" + requestId.Trim() + "'";
                
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);

                mySqlDa.Fill(dataTable);
                DataRow dr = null;
                if (dataTable.Rows.Count > 0)
                {
                    dr = dataTable.Rows[0];
                }
                                
                mySqlDa.Dispose();
                mySqlCon.Close();

                return dr;
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }
        }

        public DataTable getPersonalRequests(string requestedBy,string sYear)
        {
            string sMySqlString = "";
 
            try
            {
                dataTable.Clear();

                sMySqlString =  "SELECT REQUEST_ID,DESCRIPTION_OF_TRAINING,NUMBER_OF_PARTICIPANTS,REQUESTED_DATE," +
                                " case when IS_RECOMENDED is null then 'Pending'" +
                                "	when IS_RECOMENDED = '0' then 'Rejected'" +
                                "	when IS_RECOMENDED = '1' then 'Recommended'" +
                                " end as IS_RECOMENDED," +
                                " case when IS_APPROVED is null then 'Pending'" +
                                " 	when IS_APPROVED = '0' then 'Rejected'" +
                                " 	when IS_APPROVED = '1' then 'Approved'" +
                                " end as IS_APPROVED" +
                                " FROM TRAINING_REQUEST" +
                                " WHERE REQUESTED_BY='" + requestedBy.Trim() + "' and FINANCIAL_YEAR = '" + sYear.Trim() + "' and STATUS_CODE ='1'" +
                                " order by  DATE(REQUESTED_DATE) DESC";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);

                mySqlDa.Fill(dataTable);
                mySqlCon.Close();
                return dataTable;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }
        }

        public DataTable getPersonalRequests(string requestedBy, string sStatus,string sYear)
        {
            string sMySqlString = "";
            string paraStatus = "";
            try
            {
                dataTable.Clear();

                if (sStatus.Trim() == String.Empty)
                {
                    paraStatus = " ";
                }
                else if(sStatus.Trim() == Constants.CON_PENDING_STRING)
                {
                    paraStatus = " and (IS_RECOMENDED  is null or IS_APPROVED  is null) ";
                }
                else if (sStatus.Trim() == Constants.CON_REJECTED_STRING)
                {
                    paraStatus = " and (IS_RECOMENDED = '" + Constants.CON_REJECTED + "' or IS_APPROVED = '" + Constants.CON_REJECTED + "') ";
                }
                else if (sStatus.Trim() == Constants.CON_RECOMENDED_STRING)
                {
                    paraStatus = " and (IS_RECOMENDED = '" + Constants.CON_APPROVED + "') ";
                }
                else if (sStatus.Trim() == Constants.CON_APPROVED_STRING)
                {
                    paraStatus = " and (IS_APPROVED = '" + Constants.CON_APPROVED + "') ";
                }

                sMySqlString = "SELECT REQUEST_ID,DESCRIPTION_OF_TRAINING,NUMBER_OF_PARTICIPANTS,DATE_FORMAT(REQUESTED_DATE, '%Y/%m/%d') as REQUESTED_DATE," +
                                " case when IS_RECOMENDED is null then 'Pending'" +
                                "	when IS_RECOMENDED = '0' then 'Rejected'" +
                                "	when IS_RECOMENDED = '1' then 'Recommended'" +
                                " end as IS_RECOMENDED," +
                                " case when IS_APPROVED is null then 'Pending'" +
                                " 	when IS_APPROVED = '0' then 'Rejected'" +
                                " 	when IS_APPROVED = '1' then 'Approved'" +
                                " end as IS_APPROVED" +
                                " FROM TRAINING_REQUEST" +
                                " WHERE REQUESTED_BY='" + requestedBy.Trim() + "' and STATUS_CODE ='1'" + paraStatus +
                                " and  FINANCIAL_YEAR = '" + sYear.Trim() + "'" +
                                " order by  DATE(REQUESTED_DATE) DESC";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);

                mySqlDa.Fill(dataTable);
                mySqlCon.Close();
                return dataTable;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }
        }


        public DataRow getRequest(string requestId)
        {
            try
            {
                dataTable.Clear();
                
                string sMySqlString =  " SELECT REQUEST_ID,TRAINING_CATEGORY,TRAINING_SUB_CATEGORY_ID,COMPANY_ID,DEPARTMENT_ID,DIVISION_ID,BRANCH_ID,REQUEST_TYPE, " +
                                       " REQUESTED_BY,DESIGNATION,EMAIL,REASON,DESCRIPTION_OF_TRAINING,SKILLS_EXPECTED,CONVERT(NUMBER_OF_PARTICIPANTS, char(10)) as NUMBER_OF_PARTICIPANTS,DATE_FORMAT(REQUESTED_DATE, '%Y/%m/%d') as REQUESTED_DATE, " +
                                       " REMARKS,TO_RECOMMEND,TO_APPROVE,DATE_FORMAT(RECOMENDED_DATE, '%Y/%m/%d') as RECOMENDED_DATE,RECOMENDED_REASON,IS_RECOMENDED,APPROVED_BY,DATE_FORMAT(APPROVED_DATE, '%Y/%m/%d') as APPROVED_DATE,APPROVED_REASON, " +
                                       " IS_APPROVED,FINANCIAL_YEAR,STATUS_CODE  " +
                                       " FROM TRAINING_REQUEST " +
                                       " where REQUEST_ID='" + requestId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);

                mySqlDa.Fill(dataTable);
                DataRow dr = null;

                if (dataTable.Rows.Count > 0)
                {
                    dr = dataTable.Rows[0];
                }

                mySqlDa.Dispose();
                mySqlCon.Close();

                return dr;
            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }



        }

        public DataTable getRequestsCategories()
        {
            string sMySqlString = "";

            try
            {
                dataTable.Clear();

                sMySqlString = "SELECT TRAINING_CATEGORY_ID, CATEGORY_NAME FROM TRAINING_CATEGORY WHERE STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "';";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);

                mySqlDa.Fill(dataTable);
                mySqlCon.Close();
                return dataTable;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }
        }

        public DataTable getRequestsTypes()
        {
            string sMySqlString = "";

            try
            {
                dataTable.Clear();

                sMySqlString = "SELECT REQUEST_TYPE_ID, TYPE_NAME FROM REQUEST_TYPE WHERE STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "';";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);

                mySqlDa.Fill(dataTable);
                mySqlCon.Close();
                return dataTable;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }
        }

        public DataTable getRequests(string CompanyID, string DepartmentID, string Division, string Branch, string Category, string RequestType)
        {
            string sMySqlString = "";

            try
            {
                dataTable.Clear();

                sMySqlString = @"
                                    SELECT 
                                        REQUEST_ID,
                                        DESCRIPTION_OF_TRAINING,
                                        SKILLS_EXPECTED,
                                        REMARKS,
                                        NUMBER_OF_PARTICIPANTS
                                    FROM
                                        TRAINING_REQUEST
                                    WHERE
                                        STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + @"'
                                ";

                if (CompanyID != String.Empty)
                {
                    sMySqlString += @" AND COMPANY_ID = '" + CompanyID + @"' ";
                }

                if (DepartmentID != String.Empty)
                {
                    sMySqlString += @" AND DEPARTMENT_ID = '" + DepartmentID + @"' ";
                }

                if (Division != String.Empty)
                {
                    sMySqlString += @" AND DIVISION_ID = '" + Division + @"' ";
                }

                if (Branch != String.Empty)
                {
                    sMySqlString += @" AND BRANCH_ID = '" + Branch + @"' ";
                }

                if (Category != String.Empty)
                {
                    sMySqlString += @" AND TRAINING_CATEGORY = '" + Category + @"' ";
                }

                if (RequestType != String.Empty)
                {
                    sMySqlString += @" AND REQUEST_TYPE = '" + RequestType + @"' ";
                }

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);

                mySqlDa.Fill(dataTable);
                mySqlCon.Close();
                return dataTable;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw ex;
            }
        }

        /// addedd by chathura_a ///
        /// 
        public DataTable getAllActiveCompanies()
        {
            DataTable resultDataTable = new DataTable();
            try
            {

                string sqlQueryString = " SELECT COMPANY_ID, COMP_NAME FROM COMPANY WHERE STATUS_CODE = '"+Constants.STATUS_ACTIVE_VALUE+"' ";

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

        public DataTable getActiveBranchesForCompany(string companyCode)
        {
            try
            {
                dataTable = new DataTable();

                string sMySqlString = @"

                                            SELECT 
                                                BRANCH_ID, BRANCH_NAME
                                            FROM
                                                COMPANY_BRANCH
                                            WHERE
                                                COMPANY_ID = '" + companyCode + @"' AND STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' ";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool checkCategoryStatus(string categoryId)
        {
            bool status = false;
            DataTable ResultTable = new DataTable();
            try
            {
                string sqlQuery = " SELECT STATUS_CODE FROM TRAINING_CATEGORY WHERE TRAINING_CATEGORY_ID ='" + categoryId + "' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery,mySqlCon);
                mySqlDataAdapter.Fill(ResultTable);

                if (ResultTable.Rows.Count > 0)
                {
                    if (ResultTable.Rows.Count == 1)
                    {
                        string status_code = ResultTable.Rows[0][0].ToString();
                        if (status_code == Constants.CON_ACTIVE_STATUS)
                        {
                            status = true;
                        }
                        else if (status_code == Constants.CON_INACTIVE_STATUS)
                        {
                            status = false;
                        }
                    }
                }
                return status;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                ResultTable.Dispose();
            }

        }

        public bool checkSubCategoryStatus(string subcategoryId)
        {
            bool status = false;
            DataTable ResultTable = new DataTable();
            try
            {
                string sqlQuery = " SELECT STATUS_CODE FROM TRAINING_SUB_CATEGORY WHERE TYPE_ID ='" + subcategoryId + "' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(ResultTable);

                if (ResultTable.Rows.Count > 0)
                {
                    if (ResultTable.Rows.Count == 1)
                    {
                        string status_code = ResultTable.Rows[0][0].ToString();
                        if (status_code == Constants.CON_ACTIVE_STATUS)
                        {
                            status = true;
                        }
                        else if (status_code == Constants.CON_INACTIVE_STATUS)
                        {
                            status = false;
                        }
                    }
                }
                return status;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                ResultTable.Dispose();
            }
        }
    }
}
