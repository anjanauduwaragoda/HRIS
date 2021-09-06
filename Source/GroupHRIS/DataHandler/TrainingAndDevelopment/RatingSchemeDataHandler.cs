using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class RatingSchemeDataHandler : TemplateDataHandler
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
                string schemeId = serialCode.getserila(mySqlCon, "RS");

                mySqlCmd.Parameters.Add(new MySqlParameter("@schemeId", schemeId.Trim() == "" ? (object)DBNull.Value : schemeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@schemeName", schemeName.Trim() == "" ? (object)DBNull.Value : schemeName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));


                mySqlQuery = "insert into RATING_SCHEME (RS_ID, RS_NAME, DESCRIPTION, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE)" + " values (@schemeId,@schemeName,@remarks,@status,@addedUserId,now(),@addedUserId,now())";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = mySqlQuery;
                mySqlCmd.ExecuteNonQuery();


                foreach (DataRow dataRow in proficiencyLevelDataTable.Rows)
                {
                    mySqlCmd.Parameters.Clear();
                    string rating = dataRow[1].ToString();
                    int weight = Convert.ToInt32((dataRow[2]).ToString());
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

                    mySqlQueryInsertLevels = "insert into RS_RATINGS (RS_ID, RATING, WEIGHT, REMARKS, DESCRIPTION, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE)" + " values (@schemeId,@rating,@weight,@levelRemarks,@levelDescription,@levelStatus,@addedUserId,now(),@addedUserId,now())";

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
                string queryString = "SELECT RS_ID, RS_NAME, DESCRIPTION, " +
                                     " CASE " +
                                     " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '"+Constants.STATUS_INACTIVE_TAG+"' " +
                                     " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "' " +
                                     " End  as STATUS_CODE " +
                                     " FROM RATING_SCHEME ORDER BY RS_NAME ASC";

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
                string queryString = "select * from RATING_SCHEME  where RS_ID = '" + schemeId + "' ";

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
                                  " FROM RS_RATINGS " +
                                  " WHERE RS_ID ='" + schemeId + "'";

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

                mySqlQuery = "UPDATE RATING_SCHEME SET " +
                    "RS_NAME = @schemeName, " +
                    "DESCRIPTION = @remarks, " +
                    "STATUS_CODE = @status, " +
                    "MODIFIED_BY = @UserId, " +
                    "MODIFIED_DATE = now() " +
                    "WHERE RS_ID = @schemeId";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = mySqlQuery;
                mySqlCmd.ExecuteNonQuery();

                //DataTable existing PopulateProficiencyLevels(string schemeId)

               // INSERT IGNORE INTO users_partners (uid,pid) VALUES (1,1),(1,2),(1,3),(1,4)

                foreach (DataRow dataRow in proficiencyLevelDataTable.Rows)
                {
                    mySqlCmd.Parameters.Clear();

                    string levelDescription = dataRow[2].ToString();
                    string rating = dataRow[8].ToString();
                    int weight = Convert.ToInt32(dataRow[9]);
                    string levelRemarks = dataRow[10].ToString();

                    string levelStatus = "";
                    if (dataRow[3].ToString() == Constants.STATUS_ACTIVE_TAG)
                    {
                        levelStatus = Constants.STATUS_ACTIVE_VALUE;
                    }
                    else if (dataRow[3].ToString() == Constants.STATUS_INACTIVE_TAG)
                    {
                        levelStatus = Constants.STATUS_INACTIVE_VALUE;
                    }
                    mySqlCmd.Parameters.Add(new MySqlParameter("@schemeId", schemeId.Trim() == "" ? (object)DBNull.Value : schemeId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@rating", rating.Trim() == "" ? (object)DBNull.Value : rating.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@levelRemarks", levelRemarks.Trim() == "" ? (object)DBNull.Value : levelRemarks.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@weight", weight));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@levelDescription", levelDescription.Trim() == "" ? (object)DBNull.Value : levelDescription.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@levelStatus", levelStatus.Trim() == "" ? (object)DBNull.Value : levelStatus.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", UserId.Trim() == "" ? (object)DBNull.Value : UserId.Trim()));

                    Boolean RateExists = CheckProficiencyLevelExistance(schemeId, rating);
                    if (RateExists)
                    {
                        mySqlQueryUpdateLevels =    "UPDATE RS_RATINGS SET " +
                                                    "WEIGHT = @weight, " +
                                                    "REMARKS = @levelRemarks, " +
                                                    "DESCRIPTION = @levelDescription, "+
                                                    "STATUS_CODE = @levelStatus, " +
                                                    "MODIFIED_BY = @addedUserId, " +
                                                    "MODIFIED_DATE = now() " +
                                                    "WHERE RS_ID = @schemeId && RATING = @rating";
                    }
                    else
                    {
                        mySqlQueryUpdateLevels = "insert into RS_RATINGS (RS_ID, RATING, WEIGHT, REMARKS, DESCRIPTION, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE)" + " values (@schemeId,@rating,@weight,@levelRemarks,@levelDescription,@levelStatus,@addedUserId,now(),@addedUserId,now())";
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
                string queryStr = "SELECT * FROM RATING_SCHEME WHERE RS_NAME ='" + schemeName + "'";

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
                string queryStr = "SELECT * FROM RATING_SCHEME WHERE RS_NAME ='" + schemeName + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["RS_ID"].ToString() == id)
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
                string queryString = "SELECT * FROM RS_RATINGS WHERE RS_ID ='" + schemeId + "' && RATING ='" + rating + "'";


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
        public Boolean CheckUsageOfRatingLevel(string schemeId) 
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
