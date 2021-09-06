using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class ProficiencySchemeDataHandler : TemplateDataHandler
    {
        public Boolean Insert(string schemeName, string remarks, string status, string addedUserId, DataTable proficiencyLevels)
        {

            Boolean inserted = false;
            MySqlTransaction mySqlTrans = null;
            string mySqlQuery = null;
            string mySqlQueryInsertLevels = null;

            DataTable proficiencyLevelDataTable = proficiencyLevels;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler serialCode = new SerialHandler();
                string schemeId = serialCode.getserila(mySqlCon, "PS");

                mySqlCmd.Parameters.Add(new MySqlParameter("@schemeId", schemeId.Trim() == "" ? (object)DBNull.Value : schemeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@schemeName", schemeName.Trim() == "" ? (object)DBNull.Value : schemeName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));


                mySqlQuery = "insert into PROFICIENCY_SCHEME (SCHEME_ID, SCHEME_NAME, REMARKS, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE)" + " values (@schemeId,@schemeName,@remarks,@status,@addedUserId,now(),@addedUserId,now())";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = mySqlQuery;
                mySqlCmd.ExecuteNonQuery();


                foreach (DataRow dataRow in proficiencyLevelDataTable.Rows)
                {
                    mySqlCmd.Parameters.Clear();
                    string rating = dataRow[1].ToString();
                    decimal weight = Convert.ToDecimal(dataRow[2]);
                    string levelRemarks = dataRow[4].ToString().Trim();
                    string levelDescription = dataRow[5].ToString().Trim();

                    string levelStatus = "";
                    if (dataRow[3].ToString() == Constants.STATUS_ACTIVE_TAG)
                    {
                        levelStatus = Constants.STATUS_ACTIVE_VALUE;
                    }
                    else if (dataRow[3].ToString() == Constants.STATUS_INACTIVE_TAG)
                    {
                        levelStatus = Constants.STATUS_ACTIVE_VALUE;
                    }
                    mySqlCmd.Parameters.Add(new MySqlParameter("@schemeId", schemeId.Trim() == "" ? (object)DBNull.Value : schemeId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@rating", rating.Trim() == "" ? (object)DBNull.Value : rating.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@weight", weight));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@levelRemarks", levelRemarks.Trim() == "" ? (object)DBNull.Value : levelRemarks.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@levelDescription", levelDescription.Trim() == "" ? (object)DBNull.Value : levelDescription.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@levelStatus", levelStatus.Trim() == "" ? (object)DBNull.Value : levelStatus.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                    mySqlQueryInsertLevels = "insert into PROFICIENCY_LEVELS (SCHEME_ID, RATING, WEIGHT, REMARKS, DESCRIPTION, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE)" + " values (@schemeId,@rating,@weight,@levelRemarks,@levelDescription,@levelStatus,@addedUserId,now(),@addedUserId,now())";

                    mySqlCmd.CommandText = mySqlQueryInsertLevels;
                    mySqlCmd.ExecuteNonQuery();

                }

                mySqlTrans.Commit();
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                inserted = true;
            }
            catch (Exception e)
            {                
                mySqlTrans.Rollback();
                inserted = false;
                throw e;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }

            return inserted;
        }

        public DataTable Populate()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable.Rows.Clear();
                string queryString = "SELECT SCHEME_ID, SCHEME_NAME, REMARKS, " +
                                     " CASE " +
                                     " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '"+Constants.STATUS_INACTIVE_TAG+"' " +
                                     " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "' " +
                                     " End  as STATUS_CODE " +
                                     " FROM PROFICIENCY_SCHEME ORDER BY SCHEME_NAME ASC";

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(queryString, mySqlCon);
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataRow PopulateSchemes(string schemeId)
        {
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                dataTable.Rows.Clear();
                string queryString = "select * from PROFICIENCY_SCHEME  where SCHEME_ID = '" + schemeId + "' ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(queryString, mySqlCon);
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
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable PopulateProficiencyLevels(string schemeId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable.Rows.Clear();
                string sqlQuery = "SELECT RATING, WEIGHT, REMARKS, DESCRIPTION, " +
                                  " CASE " +
                                  " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then 'Inactive' " +
                                  " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then 'Active' " +
                                  " End  as STATUS_CODE " +
                                  " FROM PROFICIENCY_LEVELS " +
                                  " WHERE SCHEME_ID ='" + schemeId + "'";

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                dataAdapter.Fill(dataTable);
                return dataTable;
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

        public Boolean Update(string schemeId, string schemeName, string remarks, string status, string UserId, DataTable proficiencyLevelDataTable)
        {
            Boolean isUpdated = false;
            MySqlTransaction mySqlTrans = null;
            string mySqlQuery = null;
            string mySqlQueryUpdateLevels = null;
            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@schemeId", schemeId.Trim() == "" ? (object)DBNull.Value : schemeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@schemeName", schemeName.Trim() == "" ? (object)DBNull.Value : schemeName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@UserId", UserId.Trim() == "" ? (object)DBNull.Value : UserId.Trim()));

                mySqlQuery = "UPDATE PROFICIENCY_SCHEME SET " +
                    "SCHEME_NAME = @schemeName, " +
                    "REMARKS = @remarks, " +
                    "STATUS_CODE = @status, " +
                    "MODIFIED_BY = @UserId, " +
                    "MODIFIED_DATE = now() " +
                    "WHERE SCHEME_ID = @schemeId";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = mySqlQuery;
                mySqlCmd.ExecuteNonQuery();

                //DataTable existing PopulateProficiencyLevels(string schemeId)

               // INSERT IGNORE INTO users_partners (uid,pid) VALUES (1,1),(1,2),(1,3),(1,4)

                foreach (DataRow dataRow in proficiencyLevelDataTable.Rows)
                {
                    mySqlCmd.Parameters.Clear();
                    string rating = dataRow[8].ToString();
                    decimal weight = Convert.ToDecimal(dataRow[9]);
                    string levelRemarks = dataRow[2].ToString();
                    string levelDescription = dataRow[10].ToString();

                    string levelStatus = "";
                    if (dataRow[3].ToString() == "Active")
                    {
                         levelStatus = "1";
                    }
                    else if (dataRow[3].ToString() == "Inactive")
                    {
                         levelStatus = "0";
                    }
                    mySqlCmd.Parameters.Add(new MySqlParameter("@schemeId", schemeId.Trim() == "" ? (object)DBNull.Value : schemeId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@rating", rating.Trim() == "" ? (object)DBNull.Value : rating.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@weight", weight));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@levelRemarks", levelRemarks.Trim() == "" ? (object)DBNull.Value : levelRemarks.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@levelDescription", levelDescription.Trim() == "" ? (object)DBNull.Value : levelDescription.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@levelStatus", levelStatus.Trim() == "" ? (object)DBNull.Value : levelStatus.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", UserId.Trim() == "" ? (object)DBNull.Value : UserId.Trim()));

                    Boolean RateExists = CheckProficiencyLevelExistance(schemeId, rating);
                    if (RateExists)
                    {
                        mySqlQueryUpdateLevels = "UPDATE PROFICIENCY_LEVELS SET " +
                    "WEIGHT = @weight, " +
                    "DESCRIPTION = @levelDescription, "+
                    "REMARKS = @levelRemarks, " +
                    "STATUS_CODE = @levelStatus, " +
                    "MODIFIED_BY = @addedUserId, " +
                    "MODIFIED_DATE = now() " +
                    "WHERE SCHEME_ID = @schemeId && RATING = @rating";
                    }
                    else
                    {
                        mySqlQueryUpdateLevels = "insert into PROFICIENCY_LEVELS (SCHEME_ID, RATING, WEIGHT, REMARKS, DESCRIPTION, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE)" + " values (@schemeId,@rating,@weight,@levelRemarks,@levelDescription,@levelStatus,@addedUserId,now(),@addedUserId,now())";
                    }
                    mySqlCmd.CommandText = mySqlQueryUpdateLevels;
                    mySqlCmd.ExecuteNonQuery();
                    
                }
                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isUpdated = true;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {

                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }

            return isUpdated;
        }

        public Boolean CheckSchemeNameExsistance(string schemeName) ///// used when saving a record
        {
            Boolean isExsists = false;
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                dataTable.Rows.Clear();                
                string queryStr = "SELECT * FROM PROFICIENCY_SCHEME WHERE SCHEME_NAME ='" + schemeName + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExsists = true;
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

        public Boolean CheckSchemeNameExsistance(string schemeName, string id) ///// used when updating a record
        {

            
            Boolean isExsists = false;

            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                dataTable.Rows.Clear();
                string queryStr = "SELECT * FROM PROFICIENCY_SCHEME WHERE SCHEME_NAME ='" + schemeName + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["SCHEME_ID"].ToString() == id)
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

        public Boolean CheckProficiencyLevelExistance(string schemeId, string rating)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                dataTable.Rows.Clear();
                string queryString = "SELECT * FROM PROFICIENCY_LEVELS WHERE SCHEME_ID ='" + schemeId + "' && RATING ='" + rating + "'";


                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(queryString, mySqlCon);
                dataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                //if (mySqlCon.State == ConnectionState.Open)
                //{
                //    mySqlCon.Close();
                //}
            }
        
        }

        /// this methode check whether a particular proficiency level has been used in a 
        /// competency profile (COMPETENCY_PROFILE) and the corresponding competency profile has been used to
        /// evaluate an employee (EMPLOYEE_COMPITANCY_ASSESSMENT)
        /// 
        ///// returns true if scheme used to evaluate an employee
        public Boolean CheckUsageOfProficiencyLevel(string schemeId) 
        {
            dataTable.Rows.Clear();
            Boolean isUsed = false;

            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}

               // string evaluatedEmployees = " SELECT * FROM PROFICIENCY_SCHEME PS " +
                                        // " JOIN COMPETENCY_PROFILE CP ON PS.SCHEME_ID = CP.PROFICIENCY_SCHEME_ID" +
                                       //  " JOIN EMPLOYEE_COMPITANCY_ASSESSMENT ECA ON ECA.COMPETENCY_PROFILE_ID = CP.COMPETENCY_PROFILE_ID" +
                                        // " WHERE PS.SCHEME_ID = '" + schemeId + "'";

                string evaluatedEmployees = " SELECT COMPETENCY_PROFILE_ID FROM COMPETENCY_PROFILE CP " +
                                            " WHERE CP.PROFICIENCY_SCHEME_ID ='" + schemeId + "'" +
                                            " AND CP.COMPETENCY_PROFILE_ID IN " +
                                            " (SELECT COMPETENCY_PROFILE_ID FROM  EMPLOYEE_COMPITANCY_ASSESSMENT) ";
                                            
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(evaluatedEmployees, mySqlCon);
                mySqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isUsed = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally 
            {
                mySqlCmd.Parameters.Clear();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

            }

            return isUsed;
        }

        //public bool checkUseageOfProficiencyScheme(string schemeId)
        //{ 
        
        //}

       
    }
}
