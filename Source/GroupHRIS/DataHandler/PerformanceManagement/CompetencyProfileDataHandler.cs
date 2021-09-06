using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Common;
using MySql.Data.MySqlClient;

namespace DataHandler.PerformanceManagement
{
    public class CompetencyProfileDataHandler:TemplateDataHandler
    {
        #region parentForm
        public DataTable getAllActiveProficiencySchemes()
        {

            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                mySqlCon.Open();
                //}

                string status = Constants.STATUS_ACTIVE_VALUE;

                //mySqlCmd.Parameters.AddWithValue("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim());

                string queryString = "SELECT SCHEME_ID, SCHEME_NAME from PROFICIENCY_SCHEME WHERE STATUS_CODE = '" + status + "'";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(queryString, mySqlCon);
                DataTable resultTable = new DataTable();
                mySqlDataAdapter.Fill(resultTable);


                return resultTable;
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

        public DataTable getProficiencyLevels(string schemeId)
        {
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                mySqlCon.Open();
                //}
                DataTable resultTable = new DataTable();
                string sqlQuery = "SELECT RATING, WEIGHT " +
                                  " FROM PROFICIENCY_LEVELS " +
                                  " WHERE SCHEME_ID ='" + schemeId + "' && STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                dataAdapter.Fill(resultTable);
                return resultTable;
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

        public DataTable getActiveAssessmentGroups()
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string status = Constants.STATUS_ACTIVE_VALUE;

                //mySqlCmd.Parameters.AddWithValue("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim());

                string queryString = "SELECT GROUP_ID, GROUP_NAME from ASSESSMENT_GROUP WHERE STATUS_CODE = '" + status + "'";

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
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }  
            }
        }

        public DataRow getAssessmentGroupById(string groupId)
        {
            DataRow resultRow = null;
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                mySqlCon.Open();
                //}

                //mySqlCmd.Parameters.AddWithValue("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim());

                string queryString = "SELECT STATUS_CODE from ASSESSMENT_GROUP WHERE GROUP_ID = '" + groupId + "'";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(queryString, mySqlCon);
                DataTable resultTable = new DataTable();
                mySqlDataAdapter.Fill(resultTable);

                if (resultTable.Rows.Count > 0)
                {
                    resultRow = resultTable.Rows[0];
                }


                return resultRow;
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

        public Boolean CheckCompetencyProfileNameExsistance(string competencyProfileName) /// this methode is used when saving an entry
        {
            Boolean nameIsExsists = false;
            dataTable.Rows.Clear();

            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                dataTable = new DataTable();
                string competencyProfileNameUpper = competencyProfileName.Replace(" ", "").ToUpper().Trim();

                string queryStr = " SELECT PROFILE_NAME FROM COMPETENCY_PROFILE where upper(REPLACE(PROFILE_NAME, ' ', '')) = '" + competencyProfileNameUpper.Trim() + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    nameIsExsists = true;
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

            return nameIsExsists;
        }

        public Boolean CheckCompetencyProfileNameExsistance(string competencyProfileName, string id) /// this methode is used when updating an entry
        {
            Boolean nameIsExsists = false;
            dataTable.Rows.Clear();

            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                dataTable = new DataTable();

                string competencyProfileNameUpper = competencyProfileName.Replace(" ", "").ToUpper().Trim();

                string queryStr = " SELECT PROFILE_NAME FROM COMPETENCY_PROFILE where upper(REPLACE(PROFILE_NAME, ' ', '')) = '" + competencyProfileNameUpper.Trim() + "' and COMPETENCY_PROFILE_ID <> '" + id.Trim() + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    nameIsExsists = true;
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


            return nameIsExsists;
        }

        /// <summary>
        /// used to check if the assessment group is already assigned to an active competency profile
        /// </summary>
        /// <param name="assessmentGroupId"></param>
        /// <returns>
        /// true if assigned to an actve competency profile
        /// if not false 
        /// </returns>
        public Boolean CheckUsageOfAssessmentGroup(string assessmentGroupId) 
        {
            bool isExists = false;
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}

                string queryString = "SELECT COMPETENCY_PROFILE_ID FROM COMPETENCY_PROFILE WHERE GROUP_ID ='" + assessmentGroupId + "' && STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "'";
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(queryString, mySqlCon);
                DataTable resultTable = new DataTable();
                mySqlDataAdapter.Fill(resultTable);

                if (resultTable.Rows.Count > 0)
                {
                    isExists = true;
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
            return isExists;
        }

        public Boolean CheckUsageOfAssessmentGroup(string assessmentGroupId, string competencyProfileId)
        {
            bool isExists = false;
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}

                string queryString = "SELECT COMPETENCY_PROFILE_ID FROM COMPETENCY_PROFILE WHERE GROUP_ID ='" + assessmentGroupId + "' && STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' && COMPETENCY_PROFILE_ID <> '" + competencyProfileId + "'";
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(queryString, mySqlCon);
                DataTable resultTable = new DataTable();
                mySqlDataAdapter.Fill(resultTable);

                if (resultTable.Rows.Count > 0)
                {
                    isExists = true;
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
            return isExists;
        }

        public DataTable getAllCompetencyProfiles()
        {
            DataTable resultTable = new DataTable();
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                mySqlCon.Open();
                //}

                string status = Constants.STATUS_ACTIVE_VALUE;

                //mySqlCmd.Parameters.AddWithValue("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim());

                string queryString = "SELECT CP.COMPETENCY_PROFILE_ID, CP.PROFILE_NAME, CP.GROUP_ID, AG.GROUP_NAME, CP.PROFICIENCY_SCHEME_ID, PS.SCHEME_NAME, CP.DESCRIPTION, " +
                                     " CASE " +
                                     " when CP.STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then '" + Constants.STATUS_ACTIVE_TAG + "' " +
                                     " when CP.STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                     " END STATUS_CODE " +
                                     " FROM COMPETENCY_PROFILE CP, PROFICIENCY_SCHEME PS, ASSESSMENT_GROUP AG " +
                                     " WHERE CP.GROUP_ID = AG.GROUP_ID && CP.PROFICIENCY_SCHEME_ID = PS.SCHEME_ID";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(queryString, mySqlCon);

                mySqlDataAdapter.Fill(resultTable);


                return resultTable;
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable getCompetenciesByProfileId(string competencyProfileId)
        {
            DataTable resultTable = new DataTable();
            try
            {

                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                
                string sqlQuery = " SELECT PC.COMPETENCY_ID, CB.COMPETENCY_GROUP_ID, CB.COMPETENCY_NAME, CB.DESCRIPTION, CAST(PC.EXPECTED_PROFICIENCY_RATING as CHAR(7)) as EXPECTED_PROFICIENCY_RATING, PC.EXPECTED_PROFICIENCY_WEIGHT, '1' AS INCLUDE" +
                                  " FROM PROFILE_COMPETENCIES PC, COMPETENCY_BANK CB "+
                                  " WHERE PC.COMPETENCY_PROFILE_ID = '"+competencyProfileId+"' "+
                                  " && PC.COMPETENCY_ID = CB.COMPETENCY_ID ";

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                dataAdapter.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public bool insert(string name,string description,string proficiencySchmId,string assessmentGroupId,string status,DataTable competencies,string addedUserId) {
           // bool isInserted = false;

            Boolean inserted = false;
            MySqlTransaction mySqlTrans = null;
            

            try
            {

                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler serialCode = new SerialHandler();
                string profileId = serialCode.getserila(mySqlCon, "COP");

                mySqlCmd.Parameters.Add(new MySqlParameter("@profileId", profileId.Trim() == "" ? (object)DBNull.Value : profileId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@name", name.Trim() == "" ? (object)DBNull.Value : name.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@proficiencySchmId", proficiencySchmId.Trim() == "" ? (object)DBNull.Value : proficiencySchmId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentGroupId", assessmentGroupId.Trim() == "" ? (object)DBNull.Value : assessmentGroupId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));


                string profileInsertString = "INSERT INTO COMPETENCY_PROFILE (COMPETENCY_PROFILE_ID, GROUP_ID, PROFICIENCY_SCHEME_ID, PROFILE_NAME, DESCRIPTION, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE) " +
                                         "VALUES(@profileId,@assessmentGroupId, @proficiencySchmId, @name,@description,@status,@addedUserId,now(),@addedUserId,now())";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = profileInsertString;
                mySqlCmd.ExecuteNonQuery();

                /////////////////////////////////////  adding competencies to profile competencies table /////////////////////////////////////////

                foreach (DataRow competency in competencies.Rows)
                {
                    mySqlCmd.Parameters.Clear();


                    string competencyId = competency["COMPETENCY_ID"].ToString();
                    string rate = competency["EXPECTED_PROFICIENCY_RATING"].ToString();
                    decimal weight = Convert.ToDecimal(competency["EXPECTED_PROFICIENCY_WEIGHT"].ToString());
                    string remarks = competency["DESCRIPTION"].ToString();
                    string CompetencyStatus = "1";


                    mySqlCmd.Parameters.Add(new MySqlParameter("@profileId", profileId.Trim() == "" ? (object)DBNull.Value : profileId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@competencyId", competencyId.Trim() == "" ? (object)DBNull.Value : competencyId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@rate", rate.Trim() == "" ? (object)DBNull.Value : rate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@weight", weight));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@CompetencyStatus", CompetencyStatus.Trim() == "" ? (object)DBNull.Value : CompetencyStatus.Trim()));
                    
                    mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                    string competencyInsertString = "INSERT INTO PROFILE_COMPETENCIES (COMPETENCY_PROFILE_ID, COMPETENCY_ID, EXPECTED_PROFICIENCY_RATING, EXPECTED_PROFICIENCY_WEIGHT, REMARKS, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE)" +
                                           " VALUES( @profileId, @competencyId, @rate, @weight, @remarks, @CompetencyStatus, @addedUserId,now(),@addedUserId,now())";

                    mySqlCmd.CommandText = competencyInsertString;
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
                throw e;
            }

            return inserted;


        }

        public bool update(string profileId, string name, string description, string proficiencySchmId, string assessmentGroupId, string status, DataTable competencies, string addedUserId)
        {
            Boolean updated = false;
            MySqlTransaction mySqlTrans = null;


            try
            {

                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                mySqlTrans = mySqlCon.BeginTransaction();               

                mySqlCmd.Parameters.Add(new MySqlParameter("@profileId", profileId.Trim() == "" ? (object)DBNull.Value : profileId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@name", name.Trim() == "" ? (object)DBNull.Value : name.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@proficiencySchmId", proficiencySchmId.Trim() == "" ? (object)DBNull.Value : proficiencySchmId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentGroupId", assessmentGroupId.Trim() == "" ? (object)DBNull.Value : assessmentGroupId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));


                //string profileInsertString = "INSERT INTO COMPETENCY_PROFILE (COMPETENCY_PROFILE_ID, GROUP_ID, PROFICIENCY_SCHEME_ID, PROFILE_NAME, DESCRIPTION, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE) " +
                //                         "VALUES(@profileId,@assessmentGroupId, @proficiencySchmId, @name,@description,@status,@addedUserId,now(),@addedUserId,now())";

                string profileUpdateString = "UPDATE COMPETENCY_PROFILE SET " +
                    " GROUP_ID = @assessmentGroupId, " +
                    " PROFICIENCY_SCHEME_ID = @proficiencySchmId, " +
                    " PROFILE_NAME = @name, " +
                    " DESCRIPTION = @description, " +
                    " STATUS_CODE = @status, " +
                    " MODIFIED_BY = @addedUserId, " +
                    " MODIFIED_DATE = now() " +
                    " WHERE COMPETENCY_PROFILE_ID = @profileId ";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = profileUpdateString;
                mySqlCmd.ExecuteNonQuery();

                /////////////////////////////////////  adding competencies to profile competencies table /////////////////////////////////////////
                deleteExistingCompetenciesForProfile(profileId);
                foreach (DataRow competency in competencies.Rows)
                {
                    mySqlCmd.Parameters.Clear();


                    string competencyId = competency["COMPETENCY_ID"].ToString();
                    string rate = competency["EXPECTED_PROFICIENCY_RATING"].ToString();
                    decimal weight = Convert.ToDecimal(competency["EXPECTED_PROFICIENCY_WEIGHT"].ToString());
                    string remarks = competency["DESCRIPTION"].ToString();
                    string CompetencyStatus = "1";


                    mySqlCmd.Parameters.Add(new MySqlParameter("@profileId", profileId.Trim() == "" ? (object)DBNull.Value : profileId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@competencyId", competencyId.Trim() == "" ? (object)DBNull.Value : competencyId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@rate", rate.Trim() == "" ? (object)DBNull.Value : rate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@weight", weight));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@CompetencyStatus", CompetencyStatus.Trim() == "" ? (object)DBNull.Value : CompetencyStatus.Trim()));

                    mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                    string competencyInsertString = "INSERT INTO PROFILE_COMPETENCIES (COMPETENCY_PROFILE_ID, COMPETENCY_ID, EXPECTED_PROFICIENCY_RATING, EXPECTED_PROFICIENCY_WEIGHT, REMARKS, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE)" +
                                           " VALUES( @profileId, @competencyId, @rate, @weight, @remarks, @CompetencyStatus, @addedUserId,now(),@addedUserId,now())";

                    mySqlCmd.CommandText = competencyInsertString;
                    mySqlCmd.ExecuteNonQuery();

                }

                mySqlTrans.Commit();
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                updated = true;
            }

            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            

            return updated;


        }
        
        /// <summary>
        /// Update only the name and status of competency profile
        /// used to update when all the competency assessments used that paticular
        /// comp profile are closed
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="status"></param>
        /// <param name="addedUserId"></param>
        /// <returns></returns>
        public bool update(string profileId,string name, string status, string addedUserId) 
        {
            bool isUpdated = false;
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@profileId", profileId.Trim() == "" ? (object)DBNull.Value : profileId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@name", name.Trim() == "" ? (object)DBNull.Value : name.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                string profileUpdateString = "UPDATE COMPETENCY_PROFILE SET " +
                                                " PROFILE_NAME = @name, " +
                                                " STATUS_CODE = @status, " +
                                                " MODIFIED_BY = @addedUserId, " +
                                                " MODIFIED_DATE = now() " +
                                                " WHERE COMPETENCY_PROFILE_ID = @profileId ";
                mySqlCmd.CommandText = profileUpdateString;
                mySqlCmd.ExecuteNonQuery();
                isUpdated = true;
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
            return isUpdated; 
        }

        public bool update(string profileId, string name, string addedUserId)
        {
            bool isUpdated = false;
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@profileId", profileId.Trim() == "" ? (object)DBNull.Value : profileId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@name", name.Trim() == "" ? (object)DBNull.Value : name.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                string profileUpdateString = "UPDATE COMPETENCY_PROFILE SET " +
                                                " PROFILE_NAME = @name, " +
                                                " MODIFIED_BY = @addedUserId, " +
                                                " MODIFIED_DATE = now() " +
                                                " WHERE COMPETENCY_PROFILE_ID = @profileId ";
                mySqlCmd.CommandText = profileUpdateString;
                mySqlCmd.ExecuteNonQuery();
                isUpdated = true;
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
            return isUpdated;
        }

        protected void deleteExistingCompetenciesForProfile(string profileId)
        {
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                string sqlString = " DELETE FROM PROFILE_COMPETENCIES WHERE COMPETENCY_PROFILE_ID ='" + profileId + "'";

                mySqlCmd.CommandText = sqlString;
                mySqlCmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="competencyProfileId"></param>
        /// <returns> return competency assessments where paticular competency profile used </returns>
        public DataTable getUsedCompetencyAssessments(string competencyProfileId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }
                string sqlQuery = " SELECT ASSESSMENT_TOKEN,ASSESSMENT_ID,EMPLOYEE_ID,STATUS_CODE FROM EMPLOYEE_COMPITANCY_ASSESSMENT " +
                                  " where COMPETENCY_PROFILE_ID = '" + competencyProfileId + "'";

                
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                mySqlDataAdapter.Fill(resultTable);
                
                mySqlDataAdapter = null;
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable = null;
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                
            }

                               
        }
        #endregion

        #region childForm
        public DataTable getAllActiveCompetencyGroups()
        {
            DataTable resultTable = new DataTable();
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}

                string queryString = " SELECT COMPETENCY_GROUP_ID, COMPETENCY_GROUP_NAME FROM COMPETENCY_GROUP WHERE STATUS_CODE = '"+Constants.STATUS_ACTIVE_VALUE+"'";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(queryString, mySqlCon);
                
                mySqlDataAdapter.Fill(resultTable);


                return resultTable;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable getCompetenciesForCompetencyGroup(string groupId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}



                string queryString = " SELECT COMPETENCY_ID, COMPETENCY_GROUP_ID, COMPETENCY_NAME, DESCRIPTION FROM COMPETENCY_BANK WHERE COMPETENCY_GROUP_ID ='" + groupId + "' && STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(queryString, mySqlCon);
                
                mySqlDataAdapter.Fill(resultTable);


                return resultTable;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }

        public DataTable getAllCompetencies()
        {
            DataTable resultTable = new DataTable();
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}



                string queryString = " SELECT COMPETENCY_ID, COMPETENCY_GROUP_ID, COMPETENCY_NAME, DESCRIPTION FROM COMPETENCY_BANK WHERE STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "'";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(queryString, mySqlCon);

                mySqlDataAdapter.Fill(resultTable);


                return resultTable;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }
       
        #endregion



        
    }
}
