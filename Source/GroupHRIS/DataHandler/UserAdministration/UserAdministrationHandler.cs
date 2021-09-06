using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Common ;

namespace DataHandler.UserAdministration
{
    public class UserAdministrationHandler:TemplateDataHandler
    {

        public DataTable populateuserRoles()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " select ROLE_ID,ROLE_NAME," +
                                        " CASE " +
                                        " when STATUS_CODE='0' then 'Inactive' " +
                                        " when STATUS_CODE='1' then 'Active' " +
                                        " End as STATUS, " +
                                        " CASE " +
                                        " when IS_DEFAULT='0' then 'User Role' " +
                                        " when IS_DEFAULT='1' then 'Common User Role' " +
                                        " End as DEFAULTROLE " +
                                       " from HRIS_ROLE where STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' order by ROLE_ID";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataTable populateuserRoles(string sROLE_ID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " select STATUS_CODE " +
                                       " from HRIS_USER_ROLE where ROLE_ID = '" + sROLE_ID + "' and STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable populateuserRolesName(string sROLE_NAME)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " select ROLE_NAME " +
                                       " from HRIS_ROLE where ROLE_NAME = '" + sROLE_NAME + "' and STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable populateuserRolesDefault()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "select STATUS_CODE,ROLE_ID from HRIS_ROLE where IS_DEFAULT = '" + Constants.STATUS_ACTIVE_VALUE + "' and STATUS_CODE= '" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Boolean InsertUserRole(string sROLE_NAME, string sADDED_BY, string ADDED_DATE, string STATUS_CODE, string isRoleDefault)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler nserialcode = new SerialHandler();
                string sROLE_ID = nserialcode.getserila(mySqlCon,"URO");

                mySqlCmd.Parameters.Add(new MySqlParameter("@sROLE_ID", sROLE_ID.Trim() == "" ? (object)DBNull.Value : sROLE_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sROLE_NAME", sROLE_NAME.Trim() == "" ? (object)DBNull.Value : sROLE_NAME.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_BY", sADDED_BY.Trim() == "" ? (object)DBNull.Value : sADDED_BY.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_DATE", ADDED_DATE.Trim() == "" ? (object)DBNull.Value : ADDED_DATE.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSTATUS_CODE", STATUS_CODE.Trim() == "" ? (object)DBNull.Value : STATUS_CODE.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sisRoleDefault", isRoleDefault.Trim() == "" ? (object)DBNull.Value : isRoleDefault.Trim()));

                sMySqlString = "INSERT INTO HRIS_ROLE(ROLE_ID,ROLE_NAME,ADDED_BY,ADDED_DATE,STATUS_CODE,IS_DEFAULT) " +
                               "VALUES(@sROLE_ID,@sROLE_NAME,@sADDED_BY,@sADDED_DATE,@sSTATUS_CODE,@sisRoleDefault)";
                

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


        public Boolean UpdateUserRole(string sROLE_CODE, string sROLE_NAME, string sADDED_BY, string ADDED_DATE, string STATUS_CODE, string isRoleDefault)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sROLE_CODE", sROLE_CODE.Trim() == "" ? (object)DBNull.Value : sROLE_CODE.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sROLE_NAME", sROLE_NAME.Trim() == "" ? (object)DBNull.Value : sROLE_NAME.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_BY", sADDED_BY.Trim() == "" ? (object)DBNull.Value : sADDED_BY.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_DATE", ADDED_DATE.Trim() == "" ? (object)DBNull.Value : ADDED_DATE.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSTATUS_CODE", STATUS_CODE.Trim() == "" ? (object)DBNull.Value : STATUS_CODE.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sisRoleDefault", isRoleDefault.Trim() == "" ? (object)DBNull.Value : isRoleDefault.Trim()));

                sMySqlString = "UPDATE HRIS_ROLE SET IS_DEFAULT=@sisRoleDefault,ROLE_NAME=@sROLE_NAME,MODIFIED_BY=@sADDED_BY,MODIFIED_DATE=@sADDED_DATE,STATUS_CODE=@sSTATUS_CODE WHERE ROLE_ID=@sROLE_CODE ";


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

        public DataTable populateuserMainNodes()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " select SCHEME_ID,SCHEME_NAME,ADDED_BY,DATE_FORMAT(ADDED_DATE,'%Y-%m-%d')" +
                                       " from PRIVILAGE_SCHEME order by SCHEME_ID";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable populateuserMainNodes(string sSCHEME_NAME)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " select SCHEME_ID from PRIVILAGE_SCHEME where SCHEME_NAME = '" + sSCHEME_NAME + "'";

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
        ///Check whether the role is default or not 
        ///</summary>
        //----------------------------------------------------------------------------------------
        public bool isDefaultRole(string roleID)
        {
            bool isDefault = false;
            int iRecs = 0;

            String statusCode = Constants.STATUS_ACTIVE_VALUE;

            try
            {
                string sMySqlString = "SELECT count(*) FROM HRIS_ROLE " +
                                        " where ROLE_ID = @roleID " +
                                        " and IS_DEFAULT = @statusCode ";


                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.Parameters.Add(new MySqlParameter("@roleID", roleID.Trim() == "" ? (object)DBNull.Value : roleID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));

                mySqlCon.Open();

                iRecs = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());

                if (iRecs > 0)
                    isDefault = true;
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


            return isDefault;
        }


        public Boolean Insertmainnode(string sSCHEME_NAME, string sADDED_BY, string ADDED_DATE, string STATUS_CODE)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler nserialcode = new SerialHandler();
                string sSCHEME_ID = nserialcode.getserila(mySqlCon, "MNO");

                mySqlCmd.Parameters.Add(new MySqlParameter("@sSCHEME_ID", sSCHEME_ID.Trim() == "" ? (object)DBNull.Value : sSCHEME_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSCHEME_NAME", sSCHEME_NAME.Trim() == "" ? (object)DBNull.Value : sSCHEME_NAME.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_BY", sADDED_BY.Trim() == "" ? (object)DBNull.Value : sADDED_BY.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_DATE", ADDED_DATE.Trim() == "" ? (object)DBNull.Value : ADDED_DATE.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSTATUS_CODE", STATUS_CODE.Trim() == "" ? (object)DBNull.Value : STATUS_CODE.Trim()));

                sMySqlString = "INSERT INTO PRIVILAGE_SCHEME(SCHEME_ID,SCHEME_NAME,ADDED_BY,ADDED_DATE,STATUS_CODE) " +
                               "VALUES(@sSCHEME_ID,@sSCHEME_NAME,@sADDED_BY,@sADDED_DATE,@sSTATUS_CODE)";


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
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
            return blInserted;
        }


        public Boolean Updatemainnode(string smCode, string sSCHEME_NAME, string sADDED_BY, string ADDED_DATE)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler nserialcode = new SerialHandler();
                string sSCHEME_ID = nserialcode.getserila(mySqlCon, "MNO");

                mySqlCmd.Parameters.Add(new MySqlParameter("@smCode", smCode.Trim() == "" ? (object)DBNull.Value : smCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSCHEME_ID", sSCHEME_ID.Trim() == "" ? (object)DBNull.Value : sSCHEME_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSCHEME_NAME", sSCHEME_NAME.Trim() == "" ? (object)DBNull.Value : sSCHEME_NAME.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_BY", sADDED_BY.Trim() == "" ? (object)DBNull.Value : sADDED_BY.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_DATE", ADDED_DATE.Trim() == "" ? (object)DBNull.Value : ADDED_DATE.Trim()));

                sMySqlString = "UPDATE PRIVILAGE_SCHEME set SCHEME_NAME=@sSCHEME_NAME,MODIFIED_BY=@sADDED_BY,MODIFIED_DATE=@sADDED_DATE where SCHEME_ID = @smCode ";

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
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
            return blInserted;
        }

        public DataTable populateuserSubNodes(string SCHEME_ID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " select SCHEME_LINE_ID,DESCRIPTION,REDIRECT_URL" +
                                       " from PRIVILAGE_SCHEME_LINE where SCHEME_ID = '" + SCHEME_ID + "' order by SCHEME_LINE_ID";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable populateuserSubNodesName(string sDESCRIPTION)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " select DESCRIPTION from PRIVILAGE_SCHEME_LINE where DESCRIPTION = '" + sDESCRIPTION + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Boolean Insertsubnode(string sSubNodeCode ,string sSCHEME_ID, string sDESCRIPTION, string sREDIRECT_URL, string sADDED_BY, string ADDED_DATE, string STATUS_CODE)
        {
            Boolean blInserted = false;

            string sMySqlString = "";
            
            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sSubNodeCode", sSubNodeCode.Trim() == "" ? (object)DBNull.Value : sSubNodeCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSCHEME_ID", sSCHEME_ID.Trim() == "" ? (object)DBNull.Value : sSCHEME_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sREDIRECT_URL", sREDIRECT_URL.Trim() == "" ? (object)DBNull.Value : sREDIRECT_URL.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDESCRIPTION", sDESCRIPTION.Trim() == "" ? (object)DBNull.Value : sDESCRIPTION.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_BY", sADDED_BY.Trim() == "" ? (object)DBNull.Value : sADDED_BY.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_DATE", ADDED_DATE.Trim() == "" ? (object)DBNull.Value : ADDED_DATE.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSTATUS_CODE", STATUS_CODE.Trim() == "" ? (object)DBNull.Value : STATUS_CODE.Trim()));

                if (!sSubNodeCode.Equals(""))
                {

                    sMySqlString = "UPDATE PRIVILAGE_SCHEME_LINE SET REDIRECT_URL=@sREDIRECT_URL,DESCRIPTION=@sDESCRIPTION,MODIFIED_BY=@sADDED_BY,MODIFIED_DATE=@sADDED_DATE  WHERE SCHEME_ID=@sSCHEME_ID AND SCHEME_LINE_ID=@sSubNodeCode ";
                    mySqlCmd.Transaction = mySqlTrans;
                    mySqlCmd.CommandText = sMySqlString;

                    mySqlCmd.ExecuteNonQuery();
                }
                else
                {
                    SerialHandler nserialcode = new SerialHandler();
                    string sSCHEME_LINE_ID = nserialcode.getserila(mySqlCon, "SNO");//CHANGED "SN0" TO "SNO" //CHATHURA  //2015/05/08 

                    mySqlCmd.Parameters.Add(new MySqlParameter("@sSCHEME_LINE_ID", sSCHEME_LINE_ID.Trim() == "" ? (object)DBNull.Value : sSCHEME_LINE_ID.Trim()));

                    sMySqlString = "INSERT INTO PRIVILAGE_SCHEME_LINE(SCHEME_LINE_ID,SCHEME_ID,REDIRECT_URL,DESCRIPTION,ADDED_BY,ADDED_DATE) " +
                                   "VALUES(@sSCHEME_LINE_ID,@sSCHEME_ID,@sREDIRECT_URL,@sDESCRIPTION,@sADDED_BY,@sADDED_DATE)";

                    mySqlCmd.Transaction = mySqlTrans;
                    mySqlCmd.CommandText = sMySqlString;
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

                mySqlTrans.Rollback();
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
            return blInserted;
        }

        public DataTable populateaccessrights(string sROLE_ID , string sMainnode , string sSubnode)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " select ROLE_ID,SCHEME_ID,SCHEME_LINE_ID" +
                                       " from ROLE_PRIVILAGE_SCHEME_LOG where ROLE_ID = '" + sROLE_ID.ToString() + "' and SCHEME_ID ='" + sMainnode.ToString() + "' and SCHEME_LINE_ID = '" + sSubnode.ToString() + "' and IS_ADDED = '" + Constants.STATUS_ACTIVE_VALUE + "' order by ROLE_ID";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Boolean UpdateAccess(string sROLE_ID, string sMainnode)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sROLE_ID", sROLE_ID.Trim() == "" ? (object)DBNull.Value : sROLE_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMainnode", sMainnode.Trim() == "" ? (object)DBNull.Value : sMainnode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sIS_ADDED", "C"));


                sMySqlString = "Update ROLE_PRIVILAGE_SCHEME_LOG " +
                               "set IS_ADDED= @sIS_ADDED " +
                               "where ROLE_ID= @sROLE_ID and SCHEME_ID = @sMainnode";

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
            finally
            {
                mySqlCmd.Parameters.Clear();
            }

            return blInserted;
        }

        public Boolean InsertUserAccess(DataTable mDtUseraccess, string sIS_ADDED, string sDESCRIPTION, string sADDED_BY, string ADDED_DATE)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                for (int x = 0; x < mDtUseraccess.Rows.Count; x++ )
                {

                    string sROLE_ID = mDtUseraccess.Rows[x][0].ToString();
                    string sMainnode = mDtUseraccess.Rows[x][1].ToString();
                    string sSubnode = mDtUseraccess.Rows[x][2].ToString();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@sROLE_ID", sROLE_ID.Trim() == "" ? (object)DBNull.Value : sROLE_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sMainnode", sMainnode.Trim() == "" ? (object)DBNull.Value : sMainnode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sSubnode", sSubnode.Trim() == "" ? (object)DBNull.Value : sSubnode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sIS_ADDED", sIS_ADDED.Trim() == "" ? (object)DBNull.Value : sIS_ADDED.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sDESCRIPTION", sDESCRIPTION.Trim() == "" ? (object)DBNull.Value : sDESCRIPTION.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_BY", sADDED_BY.Trim() == "" ? (object)DBNull.Value : sADDED_BY.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_DATE", ADDED_DATE.Trim() == "" ? (object)DBNull.Value : ADDED_DATE.Trim()));

                    sMySqlString = "INSERT INTO ROLE_PRIVILAGE_SCHEME_LOG(ROLE_ID,SCHEME_ID,SCHEME_LINE_ID,IS_ADDED,DESCRIPTION,ADDED_BY,ADDED_DATE) " +
                                   "VALUES(@sROLE_ID,@sMainnode,@sSubnode,@sIS_ADDED,@sDESCRIPTION,@sADDED_BY,@sADDED_DATE)";
                    mySqlCmd.Transaction = mySqlTrans;
                    mySqlCmd.CommandText = sMySqlString;
                    mySqlCmd.ExecuteNonQuery();
                    mySqlCmd.Parameters.Clear();

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

                mySqlTrans.Rollback();
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
            return blInserted;
        }

        public DataTable populateuserprivilage(string sROLE_ID)
        {
            try
            {
                dataTable.Rows.Clear();
                //string sMySqlString = " select  PRIVILAGE_SCHEME.SCHEME_ID as tmaincode,PRIVILAGE_SCHEME.SCHEME_NAME,PRIVILAGE_SCHEME_LINE.DESCRIPTION,PRIVILAGE_SCHEME_LINE.REDIRECT_URL,ROLE_PRIVILAGE_SCHEME_LOG.ROLE_ID " +
                //                       " from PRIVILAGE_SCHEME,PRIVILAGE_SCHEME_LINE,ROLE_PRIVILAGE_SCHEME_LOG " +
                //                       " where PRIVILAGE_SCHEME.SCHEME_ID = PRIVILAGE_SCHEME_LINE.SCHEME_ID " +
                //                       " and PRIVILAGE_SCHEME_LINE.SCHEME_LINE_ID = ROLE_PRIVILAGE_SCHEME_LOG.SCHEME_LINE_ID " +
                //                       " and PRIVILAGE_SCHEME.SCHEME_ID = ROLE_PRIVILAGE_SCHEME_LOG.SCHEME_ID " +
                //                       " and ROLE_PRIVILAGE_SCHEME_LOG.ROLE_ID = '" + sROLE_ID  + "'" +
                //                       " and ROLE_PRIVILAGE_SCHEME_LOG.IS_ADDED = '" + Constants.STATUS_ACTIVE_VALUE + "' order by PRIVILAGE_SCHEME.SCHEME_ID,PRIVILAGE_SCHEME_LINE.SEQNO ";
                
                string sMySqlString = @"
                                            select 
                                                PRIVILAGE_SCHEME.SCHEME_ID as tmaincode,
                                                PRIVILAGE_SCHEME.SCHEME_NAME,
                                                PRIVILAGE_SCHEME_LINE.DESCRIPTION,
                                                PRIVILAGE_SCHEME_LINE.REDIRECT_URL,
                                                ROLE_PRIVILAGE_SCHEME_LOG.ROLE_ID,
	                                            PRIVILAGE_SCHEME_LINE.SEQNO
                                            from
                                                PRIVILAGE_SCHEME,
                                                PRIVILAGE_SCHEME_LINE,
                                                ROLE_PRIVILAGE_SCHEME_LOG
                                            where
                                                PRIVILAGE_SCHEME.SCHEME_ID = PRIVILAGE_SCHEME_LINE.SCHEME_ID
                                                    and PRIVILAGE_SCHEME_LINE.SCHEME_LINE_ID = ROLE_PRIVILAGE_SCHEME_LOG.SCHEME_LINE_ID
                                                    and PRIVILAGE_SCHEME.SCHEME_ID = ROLE_PRIVILAGE_SCHEME_LOG.SCHEME_ID
                                                    and ROLE_PRIVILAGE_SCHEME_LOG.ROLE_ID = '" + sROLE_ID  + @"'
                                                    and ROLE_PRIVILAGE_SCHEME_LOG.IS_ADDED = '" + Constants.STATUS_ACTIVE_VALUE + @"' and PRIVILAGE_SCHEME.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + @"'
                                            order by PRIVILAGE_SCHEME_LINE.SCHEME_ID , PRIVILAGE_SCHEME_LINE.SEQNO
                                        ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable populateuserSubNodesForAccess(string SCHEME_ID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " select SCHEME_ID,DESCRIPTION,REDIRECT_URL,SCHEME_LINE_ID" +
                                       " from PRIVILAGE_SCHEME_LINE where SCHEME_ID = '" + SCHEME_ID + "' order by SCHEME_LINE_ID";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable populateHrisUser(string sEmpid, string sStatus_Code)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " select EMPLOYEE_ID,USER_ID,FIRST_NAME,LAST_NAME,STATUS_CODE " +
                                       " from HRIS_USER where EMPLOYEE_ID = '" + sEmpid + "' and STATUS_CODE ='" + sStatus_Code + "' order by EMPLOYEE_ID";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable populateHrisUser(string sUSER_ID)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " select EMPLOYEE_ID,USER_ID,FIRST_NAME,LAST_NAME,STATUS_CODE " +
                                       " from HRIS_USER where USER_ID = '" + sUSER_ID + "' and STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateHrisUser(string sEPF_NO, string sCompID, string sDeptID, string sEMP_NIC, string sLAST_NAME)
        {
            try
            {
                string sMySqlString = "";
                dataTable.Rows.Clear();
                if (sDeptID.Equals(""))
                {
                    sMySqlString = " select EMPLOYEE.EMPLOYEE_ID as EPF_NO,EPF_NO,IFNULL(USER_ID,'-'),EMPLOYEE.INITIALS_NAME,EMPLOYEE.KNOWN_NAME,IFNULL(HRIS_USER.COMPANY_ID,'-') as COMPANY_ID,IFNULL(EMPLOYEE.EMAIL,'-') as EMAIL " +
                                   " from  EMPLOYEE left outer join HRIS_USER on EMPLOYEE.EMPLOYEE_ID = HRIS_USER.EMPLOYEE_ID " +
                                   " where EMPLOYEE.EPF_NO like '" + sEPF_NO + "%' and EMPLOYEE.COMPANY_ID like  '" + sCompID + "%' and EMPLOYEE.EMP_NIC  like  '" + sEMP_NIC + "%' and EMPLOYEE.KNOWN_NAME  like  '" + sLAST_NAME + "%' and EMPLOYEE.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STS + "'";
                }
                else
                {
                    sMySqlString = " select EMPLOYEE.EMPLOYEE_ID as EPF_NO,EPF_NO,IFNULL(USER_ID,'-'),EMPLOYEE.INITIALS_NAME,EMPLOYEE.KNOWN_NAME,IFNULL(HRIS_USER.COMPANY_ID,'-') as COMPANY_ID,IFNULL(EMPLOYEE.EMAIL,'-') as EMAIL" +
                                   " from  EMPLOYEE left outer join HRIS_USER on EMPLOYEE.EMPLOYEE_ID = HRIS_USER.EMPLOYEE_ID " +
                                   " where EMPLOYEE.EPF_NO like '" + sEPF_NO + "%' and EMPLOYEE.COMPANY_ID like  '" + sCompID + "%' and DEPT_ID like  '" + sDeptID + "%' and EMPLOYEE.EMP_NIC  like  '" + sEMP_NIC + "%' and EMPLOYEE.KNOWN_NAME  like  '" + sLAST_NAME + "%' and EMPLOYEE.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STS + "'";
                }
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Boolean InsertHrisUser(string sCompany_ID,string sEmpPass,string sEMPLOYEE_ID,string sUSER_ID, string sFIRST_NAME, string sLAST_NAME, string sSTATUS_CODE, string sADDED_BY, string ADDED_DATE)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sCompany_ID", sCompany_ID.Trim() == "" ? (object)DBNull.Value : sCompany_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEMPLOYEE_ID", sEMPLOYEE_ID.Trim() == "" ? (object)DBNull.Value : sEMPLOYEE_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sUSER_ID", sUSER_ID.Trim() == "" ? (object)DBNull.Value : sUSER_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEmpPass", sEmpPass.Trim() == "" ? (object)DBNull.Value : sEmpPass.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sFIRST_NAME", sFIRST_NAME.Trim() == "" ? (object)DBNull.Value : sFIRST_NAME.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLAST_NAME", sLAST_NAME.Trim() == "" ? (object)DBNull.Value : sLAST_NAME.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSTATUS_CODE", sSTATUS_CODE.Trim() == "" ? (object)DBNull.Value : sSTATUS_CODE.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_BY", sADDED_BY.Trim() == "" ? (object)DBNull.Value : sADDED_BY.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_DATE", ADDED_DATE.Trim() == "" ? (object)DBNull.Value : ADDED_DATE.Trim()));

                sMySqlString = "INSERT INTO HRIS_USER(COMPANY_ID,EMPLOYEE_ID,USER_ID,USER_PASSWORD,FIRST_NAME,LAST_NAME,STATUS_CODE,ADDED_BY,ADDED_DATE) " +
                               "VALUES(@sCompany_ID,@sEMPLOYEE_ID,@sUSER_ID,@sEmpPass,@sFIRST_NAME,@sLAST_NAME,@sSTATUS_CODE,@sADDED_BY,@sADDED_DATE)";

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
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
            return blInserted;
        }


        // update hris user profile update
        public Boolean UpdateHrisUserProfile(string sEMPLOYEE_ID, string sFIRST_NAME, string sLAST_NAME,string sBirthDay,string sGender,string sMaritalsts,string sReligion, string sSTATUS_CODE, string sMODIFIY_BY, string sMODIFIY_DATE)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sReligion", sReligion.Trim() == "" ? (object)DBNull.Value : sReligion.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMaritalsts", sMaritalsts.Trim() == "" ? (object)DBNull.Value : sMaritalsts.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sGender", sGender.Trim() == "" ? (object)DBNull.Value : sGender.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sBirthDay", sBirthDay.Trim() == "" ? (object)DBNull.Value : sBirthDay.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sLAST_NAME", sLAST_NAME.Trim() == "" ? (object)DBNull.Value : sLAST_NAME.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sFIRST_NAME", sFIRST_NAME.Trim() == "" ? (object)DBNull.Value : sFIRST_NAME.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEMPLOYEE_ID", sEMPLOYEE_ID.Trim() == "" ? (object)DBNull.Value : sEMPLOYEE_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSTATUS_CODE", sSTATUS_CODE.Trim() == "" ? (object)DBNull.Value : sSTATUS_CODE.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMODIFIY_BY", sMODIFIY_BY.Trim() == "" ? (object)DBNull.Value : sMODIFIY_BY.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMODIFIY_DATE", sMODIFIY_DATE.Trim() == "" ? (object)DBNull.Value : sMODIFIY_DATE.Trim()));

                sMySqlString = "UPDATE HRIS_USER set MODIFIED_BY=@sMODIFIY_BY, MODIFIED_DATE=@sMODIFIY_DATE,FIRST_NAME=@sFIRST_NAME,LAST_NAME=@sLAST_NAME  where EMPLOYEE_ID=@sEMPLOYEE_ID  and STATUS_CODE = @sSTATUS_CODE";
                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                sMySqlString = "UPDATE EMPLOYEE set GENDER=@sGender,MARITAL_STATUS=@sMaritalsts,RELIGION=@sReligion,DOB=@sBirthDay  where EMPLOYEE_ID=@sEMPLOYEE_ID  and STATUS_CODE = @sSTATUS_CODE";
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
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
            return blInserted;
        }

        // Password Reset set password .

        public Boolean UpdateHrisUser(string sEmpPass, string sEMPLOYEE_ID, string sSTATUS_CODE, string sMODIFIY__BY, string sMODIFIY_DATE)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sEmpPass", sEmpPass.Trim() == "" ? (object)DBNull.Value : sEmpPass.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEMPLOYEE_ID", sEMPLOYEE_ID.Trim() == "" ? (object)DBNull.Value : sEMPLOYEE_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSTATUS_CODE", sSTATUS_CODE.Trim() == "" ? (object)DBNull.Value : sSTATUS_CODE.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMODIFIY__BY", sMODIFIY__BY.Trim() == "" ? (object)DBNull.Value : sMODIFIY__BY.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMODIFIY_DATE", sMODIFIY_DATE.Trim() == "" ? (object)DBNull.Value : sMODIFIY_DATE.Trim()));

                sMySqlString = "UPDATE HRIS_USER set MODIFIED_BY = '" + @sMODIFIY__BY + "', MODIFIED_DATE='" + @sMODIFIY_DATE + "', USER_PASSWORD=@sEmpPass  where EMPLOYEE_ID= '" + @sEMPLOYEE_ID + "' and STATUS_CODE = '" + @sSTATUS_CODE + "'";
                //sMySqlString = "UPDATE HRIS_USER set MODIFIED_BY =@sMODIFIY__BY, MODIFIED_DATE=@sMODIFIY_DATE ,USER_PASSWORD=@sEmpPass  where EMPLOYEE_ID=@sEMPLOYEE_ID and STATUS_CODE = @sSTATUS_CODE"; 

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
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
            return blInserted;
        }


        // Update Hris user company .

        public Boolean UpdateHrisUserCompany(string sCompanyId, string sEMPLOYEE_ID, string sSTATUS_CODE, string sMODIFIY__BY, string sMODIFIY_DATE)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sCompanyId", sCompanyId.Trim() == "" ? (object)DBNull.Value : sCompanyId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEMPLOYEE_ID", sEMPLOYEE_ID.Trim() == "" ? (object)DBNull.Value : sEMPLOYEE_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSTATUS_CODE", sSTATUS_CODE.Trim() == "" ? (object)DBNull.Value : sSTATUS_CODE.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMODIFIY__BY", sMODIFIY__BY.Trim() == "" ? (object)DBNull.Value : sMODIFIY__BY.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sMODIFIY_DATE", sMODIFIY_DATE.Trim() == "" ? (object)DBNull.Value : sMODIFIY_DATE.Trim()));

                sMySqlString = "UPDATE HRIS_USER set MODIFIED_BY = '" + @sMODIFIY__BY + "', MODIFIED_DATE='" + @sMODIFIY_DATE + "', COMPANY_ID=@sCompanyId  where EMPLOYEE_ID= '" + @sEMPLOYEE_ID + "' and STATUS_CODE = '" + @sSTATUS_CODE + "'";

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
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
            return blInserted;
        }

        public DataTable populateHrisUserRole(string sEPF_NO, string sCompID, string sDeptID, string sEMP_NIC, string sLAST_NAME)
        {
            try
            {
                string sMySqlString = "";
                dataTable.Rows.Clear();
                if (sDeptID.Equals(""))
                {
                    sMySqlString = " select b.EMPLOYEE_ID,a.EPF_NO,b.USER_ID, " +
                                   " a.INITIALS_NAME as name,IFNULL(d.ROLE_NAME,'-') as ROLE_NAME,IFNULL(c.DESCRIPTION,'-') FROM EMPLOYEE a INNER JOIN HRIS_USER b " +
                                   " on a.EMPLOYEE_ID = b.EMPLOYEE_ID " +
                                   " left outer join HRIS_USER_ROLE c " +
                                   " ON b.USER_ID = c.USER_ID " +
                                   " left outer join HRIS_ROLE d " +
                                   " ON c.ROLE_ID = d.ROLE_ID  " +
                                   " where (a.EPF_NO like '" + sEPF_NO + "%' and a.COMPANY_ID like  '" + sCompID + "%' and a.EMP_NIC  like  '" + sEMP_NIC + "%' and a.KNOWN_NAME  like  '" + sLAST_NAME + "%' and  a.EMPLOYEE_STATUS =  '" + Constants.CON_EMPLOYEE_STS + "' and b.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' and c.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "') " +
                                   " or ( a.EPF_NO like '" + sEPF_NO + "%' and a.COMPANY_ID like  '" + sCompID + "%' and a.EMP_NIC  like  '" + sEMP_NIC + "%' and a.KNOWN_NAME  like  '" + sLAST_NAME + "%' and  a.EMPLOYEE_STATUS =  '" + Constants.CON_EMPLOYEE_STS + "' and b.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' and  c.STATUS_CODE is null) order by b.EMPLOYEE_ID";
                }
                else
                {
                    sMySqlString = " select b.EMPLOYEE_ID,a.EPF_NO,b.USER_ID, " +
                                   " a.INITIALS_NAME as name,IFNULL(d.ROLE_NAME,'-') as ROLE_NAME,IFNULL(c.DESCRIPTION,'-') FROM EMPLOYEE a INNER JOIN HRIS_USER b " +
                                   " on a.EMPLOYEE_ID = b.EMPLOYEE_ID " +
                                   " left outer join HRIS_USER_ROLE c " +
                                   " ON b.USER_ID = c.USER_ID " +
                                   " left outer join HRIS_ROLE d " +
                                   " ON c.ROLE_ID = d.ROLE_ID " +
                                   " where (a.EPF_NO like '" + sEPF_NO + "%' and a.COMPANY_ID like  '" + sCompID + "%' and a.DEPT_ID like  '" + sDeptID + "%'  and a.EMP_NIC  like  '" + sEMP_NIC + "%' and a.KNOWN_NAME  like  '" + sLAST_NAME + "%' and  a.EMPLOYEE_STATUS =  '" + Constants.CON_EMPLOYEE_STS + "' and b.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' and c.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "') " +
                                   " or (a.EPF_NO like '" + sEPF_NO + "%' and a.COMPANY_ID like  '" + sCompID + "%' and a.DEPT_ID like  '" + sDeptID + "%'  and a.EMP_NIC  like  '" + sEMP_NIC + "%' and a.KNOWN_NAME  like  '" + sLAST_NAME + "%' and  a.EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STS + "' and b.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "'  and  c.STATUS_CODE is null) order by b.EMPLOYEE_ID";
                }
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Boolean InsertHrisUserRole(string sUSER_ID, string sROLE_ID, string sDESCRIPTION, string sSTATUS_CODE, string sADDED_BY, string ADDED_DATE)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sROLE_ID", sROLE_ID.Trim() == "" ? (object)DBNull.Value : sROLE_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sUSER_ID", sUSER_ID.Trim() == "" ? (object)DBNull.Value : sUSER_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sDESCRIPTION", sDESCRIPTION.Trim() == "" ? (object)DBNull.Value : sDESCRIPTION.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSTATUS_CODE", sSTATUS_CODE.Trim() == "" ? (object)DBNull.Value : sSTATUS_CODE.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_BY", sADDED_BY.Trim() == "" ? (object)DBNull.Value : sADDED_BY.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sADDED_DATE", ADDED_DATE.Trim() == "" ? (object)DBNull.Value : ADDED_DATE.Trim()));

                sMySqlString = "UPDATE  HRIS_USER_ROLE SET STATUS_CODE='0',MODIFIED_BY=@sADDED_BY,MODIFIED_DATE=@sADDED_DATE WHERE USER_ID=@sUSER_ID AND STATUS_CODE=@sSTATUS_CODE";
                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                sMySqlString = "INSERT INTO HRIS_USER_ROLE(USER_ID,ROLE_ID,DESCRIPTION,STATUS_CODE,ADDED_BY,ADDED_DATE) " +
                               "VALUES(@sUSER_ID,@sROLE_ID,@sDESCRIPTION,@sSTATUS_CODE,@sADDED_BY,@sADDED_DATE)";


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
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
            return blInserted;
        }


        public DataRow populateuserProfile(string sEmployeeID)
        {
            DataRow dataRow = null;

            try
            {
                string sMySqlString = "";
                dataTable.Rows.Clear();
                sMySqlString = " select e.EPF_NO AS EPF_NO,e.email as EMAIL,e.PERMANENT_ADDRESS as PERMANENT_ADDRESS,e.CURRENT_ADDRESS as CURRENT_ADDRESS, " +
                                " hr.FIRST_NAME as FIRST_NAME ,hr.LAST_NAME as LAST_NAME,hr.USER_ID as USER_ID, " +
                                " e.EMP_NIC AS EMP_NIC,e.GENDER as GENDER,e.MARITAL_STATUS as MARITAL_STATUS,e.RELIGION as RELIGION,e.DOB as DOB, " +
                                " dp.DEPT_NAME AS DEPT_NAME,dv.DIV_NAME " +
                                " AS DIV_NAME from ((((EMPLOYEE e join COMPANY c) join DEPARTMENT dp) join DIVISION dv) join HRIS_USER hr)" +
                                " where ((e.COMPANY_ID = c.COMPANY_ID) and (e.DEPT_ID = dp.DEPT_ID) and (e.EMPLOYEE_ID = hr.EMPLOYEE_ID) " +
                                " and (e.DIVISION_ID = dv.DIVISION_ID)) " +
                                " and e.EMPLOYEE_ID = '" + sEmployeeID + "' and e.STATUS_CODE <> '0' and hr.STATUS_CODE <> '0'";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
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
    }
}
