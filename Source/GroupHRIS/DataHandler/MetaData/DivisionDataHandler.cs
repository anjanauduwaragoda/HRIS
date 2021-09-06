using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.MetaData
{
    public class DivisionDataHandler: TemplateDataHandler 
    {

        public DataTable populate()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DV.DIVISION_ID, DV.DEPT_ID, DV.DIV_NAME, DV.DESCRIPTION,  DV.COST_CENTER_CODE, DV.PROFIT_CENTER_CODE, DV.LAND_PHONE ,DT.DEPT_NAME, " +
                                        " CASE " +
                                        " when DV.STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                        " when DV.STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "'" +
                                        " End  as STATUS_CODE " +
                                        " FROM DIVISION DV , DEPARTMENT DT WHERE DV.DEPT_ID = DT.DEPT_ID ORDER BY DIVISION_ID";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Boolean populate(string mDepID, string sDivisionID)
        {
            Boolean isExist = false;
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DIVISION_ID " +
                                        "FROM DIVISION WHERE DEPT_ID='" + mDepID + "' AND DIVISION_ID = '" + sDivisionID + "'";

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

        public DataTable populateByDepID (string mDepID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DIVISION_ID ,DEPT_ID,DIV_NAME,DESCRIPTION,COST_CENTER_CODE,PROFIT_CENTER_CODE, "   +
                                         " CASE " +
                                         " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                         " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "'" +
                                        " End  as STATUS_CODE " +
                                        " from DIVISION where DEPT_ID = '" + mDepID + "' AND STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' order by DIV_NAME ";
                
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populategrid(string mDepID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DIVISION_ID ,DEPT_ID,DIV_NAME,DESCRIPTION,COST_CENTER_CODE,PROFIT_CENTER_CODE, " +
                                         " CASE " +
                                         " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                         " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "'" +
                                        " End  as STATUS_CODE " +
                                        " from DIVISION where DEPT_ID = '" + mDepID + "' order by DIV_NAME ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable populateByName(string sDivName,string sDepID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DIVISION_ID  " +
                                        " from DIVISION where DEPT_ID = '" + sDepID + "' and DIV_NAME = '" + sDivName  + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable populateByDepIDActive(string sDepID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DIVISION_ID  " +
                                        " from DIVISION where DEPT_ID = '" + sDepID + "' and STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateByNameID(string sDivName,string sDivID ,  string sDepID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DIVISION_ID  " +
                                      " from DIVISION where DEPT_ID = '" + sDepID + "' and DIV_NAME = '" + sDivName + "' and DIVISION_ID <> '" + sDivID + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataRow populate(string mDepID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DIVISION_ID, DEPT_ID,  DIV_NAME, DESCRIPTION,  COST_CENTER_CODE, PROFIT_CENTER_CODE,LAND_PHONE,STATUS_CODE " + 
                                        " FROM DIVISION WHERE DIVISION_ID = '" + mDepID + "' ";

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

        public Boolean insert(string sDepID, string sDivName, string sDesc, string sLandNo, string sStatus, string sCostCen, string sProCen)
        {
            Boolean blInserted = false;
            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler nserialcode = new SerialHandler();
                string sDivID = nserialcode.getserila(mySqlCon, "DIV");
              
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDivID", sDivID.Trim() == "" ? (object)DBNull.Value : sDivID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDepID", sDepID.Trim() == "" ? (object)DBNull.Value : sDepID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDivName", sDivName.Trim() == "" ? (object)DBNull.Value : sDivName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDesc", sDesc.Trim() == "" ? (object)DBNull.Value : sDesc.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLandNo", sLandNo.Trim() == "" ? (object)DBNull.Value : sLandNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sCostCen", sCostCen.Trim() == "" ? (object)DBNull.Value : sCostCen.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sProCen", sProCen.Trim() == "" ? (object)DBNull.Value : sProCen.Trim()));

                sMySqlString = "INSERT INTO DIVISION (DIVISION_ID,DEPT_ID,DIV_NAME,DESCRIPTION,LAND_PHONE,STATUS_CODE,COST_CENTER_CODE,PROFIT_CENTER_CODE ) " +
                                            " VALUES (@sDivID, @sDepID,@sDivName,  @sDesc,@sLandNo ,@sStatus, @sCostCen, @sProCen)";

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

        public Boolean update(string sDivID , string sDepID, string sDivName, string sDesc, string sLandNo, string sStatus, string sCostCen, string sProCen)
        {
                Boolean blInserted = false;
                string sMySqlString = "";
                MySqlTransaction mySqlTrans = null;

                try
                {
                    mySqlCon.Open();
                    mySqlTrans = mySqlCon.BeginTransaction();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@sDivID", sDivID.Trim() == "" ? (object)DBNull.Value : sDivID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sDepID", sDepID.Trim() == "" ? (object)DBNull.Value : sDepID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sDivName", sDivName.Trim() == "" ? (object)DBNull.Value : sDivName.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sDesc", sDesc.Trim() == "" ? (object)DBNull.Value : sDesc.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sLandNo", sLandNo.Trim() == "" ? (object)DBNull.Value : sLandNo.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sCostCen", sCostCen.Trim() == "" ? (object)DBNull.Value : sCostCen.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sProCen", sProCen.Trim() == "" ? (object)DBNull.Value : sProCen.Trim()));
                    
                    sMySqlString = "UPDATE DIVISION " +
                                "SET DEPT_ID=@sDepID," +
                                "DIV_NAME=@sDivName," +
                                "DESCRIPTION=@sDesc," +
                                "LAND_PHONE=@sLandNo," +
                                "STATUS_CODE=@sStatus," +
                                "COST_CENTER_CODE=@sCostCen," +
                                "PROFIT_CENTER_CODE=@sProCen " +
                                "WHERE DIVISION_ID = @sDivID";

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


        // Anjana uduwaragoda on 06-11-2014
        // this function is written to used at webFrmEmploee.aspx

        public DataTable getDivisionIdDivName(string departmentId)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT DIVISION_ID,DIV_NAME FROM DIVISION where DEPT_ID = '" + departmentId.Trim() + "' AND STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow getDivisionName(string sDivisionID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT DIV_NAME FROM DIVISION where DIVISION_ID = '" + sDivisionID.Trim() + "'";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                DataRow dataRow = null;

                if (dataTable.Rows.Count > 0)
                {
                    dataRow = dataTable.Rows[0];
                }
                return dataRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateByComId(string sComID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DT.DEPT_ID,DT.COMPANY_ID,DT.DEPT_NAME,DT.LAND_PHONE,DT.DESCRIPTION, CO.COMP_NAME," +
                                        " CASE " +
                                        " when DT.STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "'" +
                                        " when DT.STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "'" +

                                        " End  as STATUS_CODE " +
                                        "  FROM DEPARTMENT DT , COMPANY CO  WHERE ( DT.COMPANY_ID = '" + sComID + "'  AND  DT.COMPANY_ID = CO.COMPANY_ID AND DT.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "')  ORDER BY DEPT_NAME ";
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
