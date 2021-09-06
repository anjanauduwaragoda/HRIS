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
    public class DepartmentDataHandler : TemplateDataHandler
    {
        public DataTable populate()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DEPT_ID,COMPANY_ID,DEPT_NAME,LAND_PHONE,DESCRIPTION , " +
                                    " CASE " +
                                    " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then 'Inactive' " +
                                    " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then 'Active' " +
                                    " End  as STATUS_CODE " +
                                    "FROM DEPARTMENT   ORDER BY DEPT_ID";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public DataRow populate(string sDepID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DEPT_ID,COMPANY_ID,DEPT_NAME,LAND_PHONE,DESCRIPTION ,STATUS_CODE " +
                                        "FROM DEPARTMENT WHERE  DEPT_ID = '"  + sDepID + "'";

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

        public Boolean populate(string sComID, string sDepID)
        {
            Boolean isExist = false;
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DEPT_ID " +
                                        "FROM DEPARTMENT WHERE COMPANY_ID='" + sComID  + "' AND DEPT_ID = '" + sDepID + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExist = true;
                }

                return isExist;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //public DataTable populateDepData(string sDepID)
        //{
        //    try
        //    {
        //        dataTable.Rows.Clear();
        //        string sMySqlString = "SELECT * FROM DEPARTMENT WHERE  DEPT_ID = '" + sDepID + "'";
        //        MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
        //        mySqlDa.Fill(dataTable);
        //        return dataTable;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable populateByComId(string sComID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DT.DEPT_ID,DT.COMPANY_ID,DT.DEPT_NAME,DT.LAND_PHONE,DT.DESCRIPTION, CO.COMP_NAME," +
                                        " CASE " +
                                        " when DT.STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "'" +
                                        " when DT.STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE   + "' then '" + Constants.STATUS_ACTIVE_TAG + "'" +
                                        //" when DT.STATUS_CODE='0' then 'Inactive' " +
                                        //" when DT.STATUS_CODE='1' then 'Active' " +
                                        " End  as STATUS_CODE " +
                                        "  FROM DEPARTMENT DT , COMPANY CO  WHERE ( DT.COMPANY_ID = '" + sComID + "'  AND  DT.COMPANY_ID = CO.COMPANY_ID)  ORDER BY DEPT_NAME ";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable populateByName(string sDepName, string sCompID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_NAME = '" + sDepName + "' and COMPANY_ID ='" + sCompID  + "'";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateByNameID(string sDepName, string sDepID, string sCompID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DEPT_NAME FROM DEPARTMENT WHERE DEPT_NAME = '" + sDepName + "' and DEPT_ID <> '" + sDepID + "'  and COMPANY_ID ='" + sCompID  + "'";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateByComIDActive(string sComID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DEPT_NAME FROM DEPARTMENT WHERE COMPANY_ID = '" + sComID + "' and status_code = '" + Constants.STATUS_ACTIVE_VALUE + "'" ;
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Insert(string mComID, string mDepName, string mLandNo, string mDesc, string mStatus)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler nserialcode = new SerialHandler();
                string sDEP_ID = nserialcode.getserila(mySqlCon, "DEP");

                mySqlCmd.Parameters.Add(new MySqlParameter("@sDEP_ID", sDEP_ID.Trim() == "" ? (object)DBNull.Value : sDEP_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@SComID", mComID.Trim() == "" ? (object)DBNull.Value : mComID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@mDepName", mDepName.Trim() == "" ? (object)DBNull.Value : mDepName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@mLandNo", mLandNo.Trim() == "" ? (object)DBNull.Value : mLandNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@mDesc", mDesc.Trim() == "" ? (object)DBNull.Value : mDesc.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@mStatus", mStatus.Trim() == "" ? (object)DBNull.Value : mStatus.Trim()));

                sMySqlString = "INSERT INTO DEPARTMENT (DEPT_ID, COMPANY_ID,DEPT_NAME,LAND_PHONE,STATUS_CODE,DESCRIPTION) " +
                                            " VALUES   (@sDEP_ID, @SComID,@mDepName, @mLandNo,@mStatus,@mDesc )";


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


        public Boolean Update(string sDepID, string sComID, string sDepName, string sLandNo, string sDesc, string sStatus)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;
            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sDepID", sDepID.Trim() == "" ? (object)DBNull.Value : sDepID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sComID", sComID.Trim() == "" ? (object)DBNull.Value : sComID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDepName", sDepName.Trim() == "" ? (object)DBNull.Value : sDepName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLandNo", sLandNo.Trim() == "" ? (object)DBNull.Value : sLandNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDesc", sDesc.Trim() == "" ? (object)DBNull.Value : sDesc.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));

                sMySqlString =  "UPDATE DEPARTMENT " +
                                "SET COMPANY_ID=@sComID," +
                                "DEPT_NAME=@sDepName," +
                                "LAND_PHONE=@sLandNo," +
                                "STATUS_CODE=@sStatus," +
                                "DESCRIPTION=@sDesc " +
                                "WHERE DEPT_ID = @sDepID";

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

        // Anjana uduwaragoda on 06-11-2014
        // this function is written to used at webFrmEmploee.aspx

        public DataTable getDepartmentIdDeptName(string companyId)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = "SELECT DEPT_ID,DEPT_NAME FROM DEPARTMENT where COMPANY_ID='" + companyId.Trim() + "' AND STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Anjana uduwaragoda on 01-10-2015
        // this function is written to used at Leave Detail Report "rptEmployeeLeaveDetails.rdlc"

        public string getDepartmentName(string DepId)
        {

            string deptName = "";

            mySqlCmd.CommandText = "SELECT DEPT_NAME FROM DEPARTMENT where DEPT_ID ='" + DepId.Trim() + "'";

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    deptName = mySqlCmd.ExecuteScalar().ToString();
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

            return deptName;
        }









        
    }

    
}
