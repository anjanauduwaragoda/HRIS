using System;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.MetaData
{
    public class BranchCenterDataHandler: TemplateDataHandler
    {
        //public DataTable populate()
        //{
        //    try
        //    {
        //        dataTable.Rows.Clear();
        //        string sMySqlString = "SELECT CB.BRANCH_ID ,CB.COMPANY_ID, CB.BRANCH_NAME, CB.LAND_PHONE1,CB.LAND_PHONE2 ,CB.FAX_NUMBER ,CB.BRANCH_ADDRESS_LINE1,C.COMP_NAME ," +
        //                                " CASE " +
        //                                " when CB.STATUS_CODE='0' then 'Inactive' " +
        //                                " when CB.STATUS_CODE='1' then 'Active' " +
        //                                " End  as STATUS_CODE " +
        //                                "  FROM COMPANY_BRANCH CB ,COMPANY C WHERE C.COMPANY_ID = CB.COMPANY_ID  ORDER BY CB.BRANCH_ID";

        //        MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
        //        mySqlDa.Fill(dataTable);
        //        return dataTable;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        public Boolean isBranchCodeExist(string branchCode, string mCompId)
        {
            Boolean isExist = false;
            dataTable.Rows.Clear();
            try
            {

                string qry = "SELECT BRANCH_ID FROM COMPANY_BRANCH WHERE BRANCH_CODE = '" + branchCode + "' AND COMPANY_ID = '" + mCompId + "';";
                MySqlDataAdapter mySqlData = new MySqlDataAdapter(qry,mySqlCon);
                mySqlData.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExist = true;
                }
                return isExist;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public Boolean isCompanyBranchExist(string companyId, string branchid)
        {
            Boolean isExist = false;
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT BRANCH_ID FROM COMPANY_BRANCH WHERE COMPANY_ID='" + companyId.Trim() + "' and BRANCH_ID='" + branchid.Trim() + "'";

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

        public DataTable populate(string sComID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT CB.BRANCH_ID ,CB.COMPANY_ID, CB.BRANCH_NAME, CB.LAND_PHONE1,CB.LAND_PHONE2 ,CB.FAX_NUMBER ,CB.BRANCH_ADDRESS_LINE1,CB.BRANCH_CODE,C.COMP_NAME ," +
                                        " CASE " +
                                        " when CB.STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                        " when CB.STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "'" +
                                        //" when CB.STATUS_CODE='0' then 'Inactive' " +
                                        //" when CB.STATUS_CODE='1' then 'Active' " +
                                        " End  as STATUS_CODE " +
                                        "  FROM COMPANY_BRANCH CB ,COMPANY C WHERE CB.COMPANY_ID = '" + sComID + "'" + " and C.COMPANY_ID = CB.COMPANY_ID ORDER BY CB.BRANCH_NAME";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateBranch(string sComID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT CB.BRANCH_ID ,CB.COMPANY_ID, CB.BRANCH_NAME, CB.LAND_PHONE1,CB.LAND_PHONE2 ,CB.FAX_NUMBER ,CB.BRANCH_ADDRESS_LINE1,CB.BRANCH_CODE,C.COMP_NAME ," +
                                        " CASE " +
                                        " when CB.STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                        " when CB.STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "'" +
                                        " End  as STATUS_CODE " +
                                        "  FROM COMPANY_BRANCH CB ,COMPANY C WHERE CB.COMPANY_ID = '" + sComID + "'" + " and C.COMPANY_ID = CB.COMPANY_ID AND CB.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' ORDER BY CB.BRANCH_NAME";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable getBranchesForCompany(string sCompID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT BRANCH_ID, BRANCH_NAME" +
                                      " FROM COMPANY_BRANCH where COMPANY_ID  = '" + sCompID.Trim() + "'" +
                                      " AND STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable populateByName(string sBranchName,string sCompID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT BRANCH_NAME" +
                                      "  FROM COMPANY_BRANCH where BRANCH_NAME = '" + sBranchName + "' and COMPANY_ID  = '" + sCompID + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable populateByComIDActive(string sCompID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT BRANCH_NAME" +
                                      "  FROM COMPANY_BRANCH where COMPANY_ID  = '" + sCompID + "' and status_code = '" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable populateByNameID(string sBranchName, string sCompID, string sBranchID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT BRANCH_NAME " +
                                        "  FROM COMPANY_BRANCH where BRANCH_NAME = '" + sBranchName + "' and BRANCH_ID <> '" + sBranchID + "' and COMPANY_ID  = '" + sCompID  + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Boolean insert(string sCompId, string sBrName, string sAdd1, string sAdd2, string sAdd3, string sAdd4, string sLandNo1, string sLandNo2, string sFaxNo, string sStatus, string sContactPerson , string sBranchCode)
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler nserialcode = new SerialHandler();
                string sBranchID = nserialcode.getserila(mySqlCon, "BRC");

                mySqlCmd.Parameters.Add(new MySqlParameter("@sBranchID", sBranchID.Trim() == "" ? (object)DBNull.Value : sBranchID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sCompId", sCompId.Trim() == "" ? (object)DBNull.Value : sCompId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sBrName", sBrName.Trim() == "" ? (object)DBNull.Value : sBrName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd1", sAdd1.Trim() == "" ? (object)DBNull.Value : sAdd1.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd2", sAdd2.Trim() == "" ? (object)DBNull.Value : sAdd2.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd3", sAdd3.Trim() == "" ? (object)DBNull.Value : sAdd3.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd4", sAdd4.Trim() == "" ? (object)DBNull.Value : sAdd4.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLandNo1", sLandNo1.Trim() == "" ? (object)DBNull.Value : sLandNo1.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLandNo2", sLandNo2.Trim() == "" ? (object)DBNull.Value : sLandNo2.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sFaxNo", sFaxNo.Trim() == "" ? (object)DBNull.Value : sFaxNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sContactPerson", sContactPerson.Trim() == "" ? (object)DBNull.Value : sContactPerson.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sBranchCode", sBranchCode.Trim() == "" ? (object)DBNull.Value : sBranchCode.Trim()));

                sMySqlString = "INSERT INTO COMPANY_BRANCH (BRANCH_ID,COMPANY_ID,BRANCH_NAME,BRANCH_ADDRESS_LINE1,BRANCH_ADDRESS_LINE2,BRANCH_ADDRESS_LINE3,BRANCH_ADDRESS_LINE4,LAND_PHONE1,LAND_PHONE2,FAX_NUMBER, STATUS_CODE, CONTACT_PERSON, BRANCH_CODE ) " +
                                            " VALUES (@sBranchID, @sCompId,@sBrName, @sAdd1 , @sAdd2, @sAdd3,@sAdd4, @sLandNo1, @sLandNo2  ,@sFaxNo,  @sStatus ,@sContactPerson, @sBranchCode)";

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


        public DataRow getBranchCenterDetails(string sBranchID)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = "SELECT COMPANY_ID,BRANCH_NAME,BRANCH_ADDRESS_LINE1,BRANCH_ADDRESS_LINE2,BRANCH_ADDRESS_LINE3,BRANCH_ADDRESS_LINE4, LAND_PHONE1,LAND_PHONE2,FAX_NUMBER,CONTACT_PERSON,STATUS_CODE,BRANCH_CODE " +
                                    "FROM COMPANY_BRANCH " +
                                    "WHERE BRANCH_ID ='" + sBranchID + "'";


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



        public Boolean update(string sBranchID, string sCompId, string sBrName, string sAdd1, string sAdd2, string sAdd3, string sAdd4, string sLandNo1, string sLandNo2, string sFaxNo, string sStatus, string sContactPerson, string sBranchCode)
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sBranchID", sBranchID.Trim() == "" ? (object)DBNull.Value : sBranchID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sCompId", sCompId.Trim() == "" ? (object)DBNull.Value : sCompId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sBrName", sBrName.Trim() == "" ? (object)DBNull.Value : sBrName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd1", sAdd1.Trim() == "" ? (object)DBNull.Value : sAdd1.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd2", sAdd2.Trim() == "" ? (object)DBNull.Value : sAdd2.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd3", sAdd3.Trim() == "" ? (object)DBNull.Value : sAdd3.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sAdd4", sAdd4.Trim() == "" ? (object)DBNull.Value : sAdd4.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLandNo1", sLandNo1.Trim() == "" ? (object)DBNull.Value : sLandNo1.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLandNo2", sLandNo2.Trim() == "" ? (object)DBNull.Value : sLandNo2.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sFaxNo", sFaxNo.Trim() == "" ? (object)DBNull.Value : sFaxNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sContactPerson", sContactPerson.Trim() == "" ? (object)DBNull.Value : sContactPerson.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sBranchCode", sBranchCode.Trim() == "" ? (object)DBNull.Value : sBranchCode.Trim()));

                sMySqlString = "UPDATE  COMPANY_BRANCH " +
                               "SET COMPANY_ID = @sCompId, " +
                               " BRANCH_NAME = @sBrName, " +
                               " BRANCH_ADDRESS_LINE1 = @sAdd1, " +
                               " BRANCH_ADDRESS_LINE2 = @sAdd2, " +
                               " BRANCH_ADDRESS_LINE3 = @sAdd3, " +
                               " BRANCH_ADDRESS_LINE4 = @sAdd4, " +
                               " LAND_PHONE1 = @sLandNo1, " +
                               " LAND_PHONE2 = @sLandNo2, " +
                               " STATUS_CODE = @sStatus, " +
                               " FAX_NUMBER = @sFaxNo, " +
                               " CONTACT_PERSON = @sContactPerson, " +
                               " BRANCH_CODE = @sBranchCode " +
                               " WHERE BRANCH_ID = @sBranchID ";

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


        public DataTable getBranchIdBranchName(string companyId)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = "SELECT BRANCH_NAME,BRANCH_ID FROM COMPANY_BRANCH where COMPANY_ID='" + companyId.Trim() + "' order by BRANCH_NAME";

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
