using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.MetaData
{
    public class CompanyDataHandler : TemplateDataHandler
    {
        // Anjana uduwaragoda on 06-11-2014
        // this function is written to used at webFrmEmploeeDesignation.aspx

        // used in webFrmEmploee.aspx

        public DataTable getCompanyIdCompName()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT COMP_NAME,COMPANY_ID FROM COMPANY order by COMP_NAME  ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Anjana uduwaragoda on 06-11-2014
        // this function is written to used at webFrmEmploeeDesignation.aspx

        // used in webFrmEmploee.aspx

        public DataTable getCompanyIdCompName(string companyId)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT COMP_NAME,COMPANY_ID FROM COMPANY where COMPANY_ID ='" + companyId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getDepartments(string companyId)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT DEPT_ID, DEPT_NAME FROM DEPARTMENT WHERE COMPANY_ID ='" + companyId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getDivisions(string DepartmentID)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT DIVISION_ID, DIV_NAME FROM DIVISION WHERE DEPT_ID ='" + DepartmentID.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getCompanyBranches(string CompanyID)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT BRANCH_ID, BRANCH_NAME FROM COMPANY_BRANCH WHERE COMPANY_ID ='" + CompanyID.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateByName(string sCompName  )
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT COMP_NAME  FROM COMPANY where COMP_NAME ='" + sCompName.Trim() + "'"; 

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateBySapID(string sSAPId)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT COMP_NAME  FROM COMPANY where COMP_SAP_ID ='" + sSAPId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateByEPFNo(string sEPFNo)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT COMP_NAME  FROM COMPANY where EMPLOYER_EPF  ='" + sEPFNo.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateByNameID(string sCompName , string sCompID)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT COMP_NAME  FROM COMPANY where COMP_NAME ='" + sCompName.Trim() + "' and COMPANY_ID <> '" + sCompID  + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateByEPFCompID (string sEPFNo,string sCompID)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT COMP_NAME  FROM COMPANY where EMPLOYER_EPF ='" + sEPFNo.Trim() + "'  and COMPANY_ID <> '" + sCompID  + "'" ;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }

        public DataTable populateBySapCompID(string sSAPId, string sCompID)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT COMP_NAME  FROM COMPANY where COMP_SAP_ID ='" + sSAPId.Trim() + "'  and COMPANY_ID <> '" + sCompID  + "'" ;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populate()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString =   "select COMPANY_ID,COMP_NAME,LAND_PHONE1,EMPLOYER_EPF,COMP_SAP_ID, " +
                                        " CASE " +
                                        " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then 'Inactive' " +
                                        " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then 'Active' " +
                                        " End  as STATUS_CODE " +
                                        " from COMPANY  order by COMPANY_ID";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow populate(string mCompID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "select * from COMPANY  where COMPANY_ID = '" + mCompID + "' ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                DataRow dataRow = null;

                if (dataTable.Rows.Count > 0)
                {
                    dataRow = dataTable.Rows[0];
                }
                return dataRow;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public Boolean Update(string sCompID, string sComName, string sAdd1, string sAdd2, string sAdd3, string sAdd4, string sLandNo1, string sLandNo2, string sEmail, string sStsCode, string sFaxNo, string sWrkHrSt, string sWrkHrEn, string sSAPId, string sEPFNo, string sVission, string sMission, string sMotto, string sModifyUser, string sBusinessType, string sHremail,string satStrtTime, string satEndTime)
        {

            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;
            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sCompID", sCompID.Trim() == "" ? (object)DBNull.Value : sCompID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sComName", sComName.Trim() == "" ? (object)DBNull.Value : sComName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd1", sAdd1.Trim() == "" ? (object)DBNull.Value : sAdd1.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd2", sAdd2.Trim() == "" ? (object)DBNull.Value : sAdd2.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd3", sAdd3.Trim() == "" ? (object)DBNull.Value : sAdd3.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd4", sAdd4.Trim() == "" ? (object)DBNull.Value : sAdd4.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLandNo1", sLandNo1.Trim() == "" ? (object)DBNull.Value : sLandNo1.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLandNo2", sLandNo2.Trim() == "" ? (object)DBNull.Value : sLandNo2.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEmail", sEmail.Trim() == "" ? (object)DBNull.Value : sEmail.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStsCode", sStsCode.Trim() == "" ? (object)DBNull.Value : sStsCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sFaxNo", sFaxNo.Trim() == "" ? (object)DBNull.Value : sFaxNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sWrkHrSt", sWrkHrSt.Trim() == "" ? (object)DBNull.Value : sWrkHrSt.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sWrkHrEn", sWrkHrEn.Trim() == "" ? (object)DBNull.Value : sWrkHrEn.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSAPId", sSAPId.Trim() == "" ? (object)DBNull.Value : sSAPId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEPFNo", sEPFNo.Trim() == "" ? (object)DBNull.Value : sEPFNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sVission", sVission.Trim() == "" ? (object)DBNull.Value : sVission.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMission", sMission.Trim() == "" ? (object)DBNull.Value : sMission.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMotto", sMotto.Trim() == "" ? (object)DBNull.Value : sMotto.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sModifyUser", sModifyUser.Trim() == "" ? (object)DBNull.Value : sModifyUser.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sBusinessType", sBusinessType.Trim() == "" ? (object)DBNull.Value : sBusinessType.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sHremail", sHremail.Trim() == "" ? (object)DBNull.Value : sHremail.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@satStrtTime", satStrtTime.Trim() == "" ? (object)DBNull.Value : satStrtTime.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@satEndTime", satEndTime.Trim() == "" ? (object)DBNull.Value : satEndTime.Trim()));



                    sMySqlString = "UPDATE COMPANY " +
                                    "SET COMP_NAME=@sComName," +
                                    "COMP_ADDRESS_LINE1=@sAdd1," +
                                    "COMP_ADDRESS_LINE2=@sAdd2," +
                                    "COMP_ADDRESS_LINE3=@sAdd3," +
                                    "COMP_ADDRESS_LINE4=@sAdd4," +
                                    "LAND_PHONE1=@sLandNo1," +
                                    "LAND_PHONE2=@sLandNo2," +
                                    "EMAIL_ADDRESS=@sEmail," +
                                    "VISION=@sVission," +
                                    "MISSION=@sMission," +
                                    "STATUS_CODE=@sStsCode," +
                                    "FAX_NUMBER=@sFaxNo," +
                                    "WORK_HOURS_START=@sWrkHrSt," +
                                    "WORK_HOURS_END=@sWrkHrEn," +
                                    "COMP_SAP_ID=@sSAPId," +
                                    "EMPLOYER_EPF=@sEPFNo," +
                                    "COMPANY_MOTTO=@sMotto, " +
                                    "MODIFIED_DATE=now() , " +
                                    "MODIFIED_BY=@sModifyUser, " +
                                    "BUSINESS_TYPE=@sBusinessType, " +
                                    "HR_EMAILS=@sHremail, " +
                                    "SATWORK_HOURS_START=@satStrtTime, " +
                                    "SATWORK_HOURS_END=@satEndTime " +
                                    "WHERE COMPANY_ID = @sCompID";
               

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

            return blInserted;
        }


        public Boolean Insert(string sComName, string sAdd1, string sAdd2, string sAdd3, string sAdd4, string sLandNo1, string sLandNo2, string sEmail, string sstsCode, string sFaxNo, string sWrkHrSt, string sWrkHrEn, string sSAPId, string sEPFNo, string sVission, string sMission, string sMotto, string slogUser, string sBusinessType, string sHremail, string satStrtTime, string satEndTime)

        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler nserialcode = new SerialHandler();
                string sCOMP_ID = nserialcode.getserila(mySqlCon, "CMP");

                mySqlCmd.Parameters.Add(new MySqlParameter("@sCOMP_ID", sCOMP_ID.Trim() == "" ? (object)DBNull.Value : sCOMP_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sComName", sComName.Trim() == "" ? (object)DBNull.Value : sComName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd1", sAdd1.Trim() == "" ? (object)DBNull.Value : sAdd1.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd2", sAdd2.Trim() == "" ? (object)DBNull.Value : sAdd2.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd3", sAdd3.Trim() == "" ? (object)DBNull.Value : sAdd3.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd4", sAdd4.Trim() == "" ? (object)DBNull.Value : sAdd4.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLandNo1", sLandNo1.Trim() == "" ? (object)DBNull.Value : sLandNo1.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLandNo2", sLandNo2.Trim() == "" ? (object)DBNull.Value : sLandNo2.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEmail", sEmail.Trim() == "" ? (object)DBNull.Value : sEmail.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sstsCode", sstsCode.Trim() == "" ? (object)DBNull.Value : sstsCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sFaxNo", sFaxNo.Trim() == "" ? (object)DBNull.Value : sFaxNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sWrkHrSt", sWrkHrSt.Trim() == "" ? (object)DBNull.Value : sWrkHrSt.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sWrkHrEn", sWrkHrEn.Trim() == "" ? (object)DBNull.Value : sWrkHrEn.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSAPId", sSAPId.Trim() == "" ? (object)DBNull.Value : sSAPId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sVission", sVission.Trim() == "" ? (object)DBNull.Value : sVission.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEPFNo", sEPFNo.Trim() == "" ? (object)DBNull.Value : sEPFNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMission", sMission.Trim() == "" ? (object)DBNull.Value : sMission.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMotto", sMotto.Trim() == "" ? (object)DBNull.Value : sMotto.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@slogUser", slogUser.Trim() == "" ? (object)DBNull.Value : slogUser.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sBusinessType", sBusinessType.Trim() == "" ? (object)DBNull.Value : sBusinessType.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sHremail", sHremail.Trim() == "" ? (object)DBNull.Value : sHremail.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@satStrtTime", satStrtTime.Trim() == "" ? (object)DBNull.Value : satStrtTime.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@satEndTime", satEndTime.Trim() == "" ? (object)DBNull.Value : satEndTime.Trim()));

                sMySqlString = "INSERT INTO COMPANY (COMPANY_ID,COMP_NAME,COMP_ADDRESS_LINE1,COMP_ADDRESS_LINE2,COMP_ADDRESS_LINE3,COMP_ADDRESS_LINE4,LAND_PHONE1,LAND_PHONE2,EMAIL_ADDRESS,VISION,MISSION,STATUS_CODE,FAX_NUMBER,WORK_HOURS_START,WORK_HOURS_END,COMP_SAP_ID, EMPLOYER_EPF,ADDED_BY,ADDED_DATE,COMPANY_MOTTO , BUSINESS_TYPE ,MODIFIED_BY ,HR_EMAILS, SATWORK_HOURS_START, SATWORK_HOURS_END) " +
                                   " VALUES (@sCOMP_ID, @sComName,@sAdd1,  @sAdd2,@sAdd3 ,@sAdd4, @sLandNo1, @sLandNo2, @sEmail,  @sVission, @sMission, @sstsCode, @sFaxNo, @sWrkHrSt,@sWrkHrEn, @sSAPId, @sEPFNo,  @slogUser , now(),  @sMotto, @sBusinessType , @slogUser ,@sHremail, @satStrtTime, @satEndTime)";                

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

            return blInserted;
        }

        public DataTable getCompanyMotto(string sCOMP_ID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT COMP_NAME,VISION,MISSION,COMPANY_MOTTO FROM COMPANY where COMPANY_ID = '" + sCOMP_ID + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow getCompanyOtheDetails(string sCOMP_ID)
        {
            DataRow dr = null;
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT COMPANY_ID,COMP_NAME,COMP_ADDRESS_LINE1,COMP_ADDRESS_LINE2,COMP_ADDRESS_LINE3,COMP_ADDRESS_LINE4,LAND_PHONE1,LAND_PHONE2,FAX_NUMBER,WORK_HOURS_START,WORK_HOURS_END FROM COMPANY where COMPANY_ID = '" + sCOMP_ID + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    dr = dataTable.Rows[0];
                }
                else
                {
                    dr = null;
                }
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getCompanyNameByCompanyId(string companyId)
        {

            string companyName= "";

            mySqlCmd.Parameters.Add(new MySqlParameter("@companyId", companyId.Trim() == "" ? (object)DBNull.Value : companyId.Trim()));

            mySqlCmd.CommandText = "SELECT COMP_NAME FROM COMPANY where COMPANY_ID=@companyId";

            try
            {
                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    companyName = mySqlCmd.ExecuteScalar().ToString();
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

            return companyName;
        }

        public DataRow getCompanyINOUT(string sCOMP_ID)
        {
            DataRow dr = null;
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT COMPANY_ID,WORK_HOURS_START,WORK_HOURS_END FROM COMPANY where COMPANY_ID = '" + sCOMP_ID + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    dr = dataTable.Rows[0];
                }
                else
                {
                    dr = null;
                }
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getHREmailAddressesForCompany(string compId)
        {
            string emailAddresses = "";

            try
            {
                string sMySqlString = " SELECT HR_EMAILS FROM COMPANY where COMPANY_ID = @compId ";


                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.Parameters.Add(new MySqlParameter("@compId", compId.Trim() == "" ? (object)DBNull.Value : compId.Trim()));

                mySqlCon.Open();

                emailAddresses = mySqlCmd.ExecuteScalar().ToString();

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


            return emailAddresses;
        }
    }
}
