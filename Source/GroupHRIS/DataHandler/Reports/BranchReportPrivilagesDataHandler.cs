using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.Reports
{
    public class BranchReportPrivilagesDataHandler : TemplateDataHandler
    {
        public DataTable populateCompanies()
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();
                sMySqlString = @"
                                    SELECT 
                                        ROLE_ID, ROLE_NAME
                                    FROM
                                        HRIS_ROLE
                                    WHERE
                                        STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"';
                                ";


                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);


                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Clone();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populate(string RoleID)
        {
            string sMySqlString = "";
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable = new DataTable();

//                sMySqlString = @"
//                                    SELECT 
//                                        RP.PRIVILAGE_ID,RP.REPORT_ID,R.description AS 'REPORT_NAME', RP.STATUS 
//                                    FROM 
//                                        REPORT_PRIVILAGES RP,REPORTS R
//                                    WHERE 
//                                        RP.REPORT_ID = R.repcode AND RP.ROLE_ID = '" + RoleID + @"' AND R.status = '" + Constants.STATUS_ACTIVE_VALUE + @"';
//                                ";

                sMySqlString = @"
                                    SELECT 
                                        (SELECT RPT.PRIVILAGE_ID FROM REPORT_PRIVILAGES RPT WHERE RPT.REPORT_ID = R.repcode AND RPT.ROLE_ID = '" + RoleID + @"') AS 'PRIVILAGE_ID',
                                        R.repcode AS 'REPORT_ID',
                                        R.description AS 'REPORT_NAME', 
                                        (SELECT RPT.STATUS FROM REPORT_PRIVILAGES RPT WHERE RPT.REPORT_ID = R.repcode AND RPT.ROLE_ID = '" + RoleID + @"') AS 'STATUS' 
                                    FROM 
                                        REPORTS R
                                    WHERE 
                                        R.status = '" + Constants.STATUS_ACTIVE_VALUE + @"';
                                ";

                mySqlCmd.Connection = mySqlCon;
                mySqlCmd.CommandText = sMySqlString;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(mySqlCmd);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count == 0)
                {
                    sMySqlString = @"
                                    SELECT 
                                        (SELECT PRIVILAGE_ID FROM REPORT_PRIVILAGES WHERE REPORT_ID = R.repcode AND ROLE_ID = '" + RoleID + @"') AS 'PRIVILAGE_ID',
                                        R.repcode AS 'REPORT_ID',
                                        R.description AS 'REPORT_NAME',
                                        (SELECT STATUS FROM REPORT_PRIVILAGES WHERE REPORT_ID = R.repcode AND ROLE_ID = '" + RoleID + @"') AS 'STATUS'
                                    FROM 
                                        REPORTS R
                                    WHERE 
                                         R.status = '" + Constants.STATUS_ACTIVE_VALUE + @"';
                                ";

                    mySqlCmd.Connection = mySqlCon;
                    mySqlCmd.CommandText = sMySqlString;

                    MySqlDataAdapter mySqlDa1 = new MySqlDataAdapter(mySqlCmd);
                    mySqlDa1.Fill(dataTable);
                }

                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Clone();
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void insert(string ROLE_ID, string ADDED_BY, DataTable PrivilageData)
        {
            SerialHandler serialHandler = new SerialHandler();
            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;
            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                for (int i = 0; i < PrivilageData.Rows.Count; i++)
                {
                    mySqlCmd.Parameters.Clear();

                    string PRIVILAGE_ID = PrivilageData.Rows[i]["PRIVILAGE_ID"].ToString();
                    string REPORT_ID = PrivilageData.Rows[i]["REPORT_ID"].ToString();
                    string REPORT_NAME = PrivilageData.Rows[i]["REPORT_NAME"].ToString();
                    string STATUS = PrivilageData.Rows[i]["STATUS"].ToString();

                    if (PRIVILAGE_ID == "")
                    {
                        PRIVILAGE_ID = serialHandler.getserila(mySqlCon, Constants.BRANCH_REPORT_PRIVILAGE_ID_STAMP);


                        mySqlCmd.Parameters.Add(new MySqlParameter("@PRIVILAGE_ID", PRIVILAGE_ID.Trim() == "" ? (object)DBNull.Value : PRIVILAGE_ID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ROLE_ID", ROLE_ID.Trim() == "" ? (object)DBNull.Value : ROLE_ID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@REPORT_ID", REPORT_ID.Trim() == "" ? (object)DBNull.Value : REPORT_ID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS", STATUS.Trim() == "" ? (object)DBNull.Value : STATUS.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", ADDED_BY.Trim() == "" ? (object)DBNull.Value : ADDED_BY.Trim()));

                        sMySqlString = @"
                                            INSERT INTO REPORT_PRIVILAGES(PRIVILAGE_ID,ROLE_ID,REPORT_ID,STATUS,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) 
                                            VALUES(@PRIVILAGE_ID,@ROLE_ID,@REPORT_ID,@STATUS,@ADDED_BY,NOW(),@MODIFIED_BY,NOW())
                                        ";

                        mySqlCmd.CommandText = sMySqlString;
                        mySqlCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        mySqlCmd.Parameters.Add(new MySqlParameter("@PRIVILAGE_ID", PRIVILAGE_ID.Trim() == "" ? (object)DBNull.Value : PRIVILAGE_ID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@STATUS", STATUS.Trim() == "" ? (object)DBNull.Value : STATUS.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@ADDED_BY", ADDED_BY.Trim() == "" ? (object)DBNull.Value : ADDED_BY.Trim()));

                        sMySqlString = @"
                                            UPDATE REPORT_PRIVILAGES 
                                            SET  STATUS = @STATUS, MODIFIED_BY = @ADDED_BY,MODIFIED_DATE = NOW()
                                            WHERE PRIVILAGE_ID = @PRIVILAGE_ID
                                        ";

                        mySqlCmd.CommandText = sMySqlString;
                        mySqlCmd.ExecuteNonQuery();
                    }
                }

                mySqlTrans.Commit();
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

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
        }
    }
}
