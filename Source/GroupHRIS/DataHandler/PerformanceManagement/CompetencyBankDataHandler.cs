using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class CompetencyBankDataHandler:TemplateDataHandler
    {

        public DataTable getAllActiveCompetencyGroups()
        {
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                string queryString = " SELECT COMPETENCY_GROUP_ID, COMPETENCY_GROUP_NAME FROM COMPETENCY_GROUP WHERE STATUS_CODE = '"+Constants.STATUS_ACTIVE_VALUE+"'";

                DataTable resultDataTable = new DataTable();
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(queryString, mySqlCon);
                mySqlDataAdapter.Fill(resultDataTable);

                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                return resultDataTable;
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

        public Boolean checkCompetencyNameExistance(string competencyName)
        {
            try
            {
                Boolean isExsists = false;
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                dataTable.Rows.Clear();


                string queryStr = "SELECT * FROM COMPETENCY_BANK WHERE COMPETENCY_NAME ='" + competencyName + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExsists = true;
                }
                return isExsists;
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

        public Boolean checkCompetencyNameExistance(string competencyName, string id)
        {

            dataTable.Rows.Clear();
            Boolean isExsists = false;

            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                string queryStr = "SELECT * FROM COMPETENCY_BANK WHERE COMPETENCY_NAME ='" + competencyName + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["COMPETENCY_ID"].ToString() == id)
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

        public Boolean insert(string name, string description, string competencyGroup, string status, string addedUserId)
        {
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
                string competencyId = serialCode.getserila(mySqlCon, "CP");

                mySqlCmd.Parameters.Add(new MySqlParameter("@competencyId", competencyId.Trim() == "" ? (object)DBNull.Value : competencyId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@name", name.Trim() == "" ? (object)DBNull.Value : name.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@competencyGroup", competencyGroup.Trim() == "" ? (object)DBNull.Value : competencyGroup.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                string queryString = " INSERT INTO COMPETENCY_BANK (COMPETENCY_ID, COMPETENCY_GROUP_ID, COMPETENCY_NAME, DESCRIPTION, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE) " +
                                     " VALUES(@competencyId,@competencyGroup,@name,@description,@status,@addedUserId,now(),@addedUserId,now())";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = queryString;
                mySqlCmd.ExecuteNonQuery();
                mySqlTrans.Commit();
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                inserted = true;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
            

            return inserted;
        }

        public DataTable getAllCompetencies()
        {
            DataTable restltDataTable = new DataTable();
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                mySqlCon.Open();
                //}

                string queryString = "select CB.COMPETENCY_ID, CB.COMPETENCY_NAME, CB.COMPETENCY_GROUP_ID, CG.COMPETENCY_GROUP_NAME,CB.DESCRIPTION," +
                                        " case when CB.STATUS_CODE = '1' then 'Active'" +
                                        " when CB.STATUS_CODE = '0' then 'Inactive'" +
                                        " end as STATUS_CODE " +
                                        " from COMPETENCY_BANK CB, COMPETENCY_GROUP CG " +
                                        " where CB.COMPETENCY_GROUP_ID = CG.COMPETENCY_GROUP_ID ORDER BY CB.COMPETENCY_NAME";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(queryString, mySqlCon);
                
                mySqlDataAdapter.Fill(restltDataTable);
                return restltDataTable;
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
                restltDataTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }

        }

        public bool update(string competencyId, string name, string description, string groupId, string status, string addedUserId)
        {
            bool isUpdated = false;
            MySqlTransaction mySqlTrans = null;
            string competencyUpdateString = null;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@competencyId", competencyId.Trim() == "" ? (object)DBNull.Value : competencyId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@name", name.Trim() == "" ? (object)DBNull.Value : name.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@groupId", groupId.Trim() == "" ? (object)DBNull.Value : groupId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                competencyUpdateString = "UPDATE COMPETENCY_BANK SET " +
                                           " COMPETENCY_GROUP_ID = @groupId, " +
                                           " COMPETENCY_NAME = @name, " +
                                           " DESCRIPTION = @description, " +
                                           " STATUS_CODE = @status, " +
                                           " MODIFIED_BY = @addedUserId, " +
                                           " MODIFIED_DATE = now() " +
                                           " WHERE COMPETENCY_ID = @competencyId ";

                mySqlCmd.CommandText = competencyUpdateString;
                mySqlCmd.ExecuteNonQuery();
                mySqlTrans.Commit();
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isUpdated = true;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }

            return isUpdated;
        }

        public bool update(string competencyId, string status, string addedUserId)
        {
            bool isUpdated = false;
            MySqlTransaction mySqlTrans = null;
            string competencyUpdateString = null;

            try
            {
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Open();
                }

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@competencyId", competencyId.Trim() == "" ? (object)DBNull.Value : competencyId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                competencyUpdateString = "UPDATE COMPETENCY_BANK SET " +                                           
                                           " STATUS_CODE = @status, " +
                                           " MODIFIED_BY = @addedUserId, " +
                                           " MODIFIED_DATE = now() " +
                                           " WHERE COMPETENCY_ID = @competencyId ";

                mySqlCmd.CommandText = competencyUpdateString;
                mySqlCmd.ExecuteNonQuery();
                mySqlTrans.Commit();
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isUpdated = true;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }

            return isUpdated;
        }

        public Boolean isCompetenciesExistForCompetencyGroup(string sCompetencyGroupId)
        {
            Boolean isExist = false;

            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT COMPETENCY_ID FROM COMPETENCY_BANK where COMPETENCY_GROUP_ID='" + sCompetencyGroupId.Trim() + "'";

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

        public DataTable getUsedCompetencyAssessments(string competencyId)
        {
            DataTable resultTable = new DataTable();

            try
            {
                mySqlCon.Open();

                string sqlQuery = " select ECA.ASSESSMENT_TOKEN, ECA.STATUS_CODE " +
                                  " from EMPLOYEE_COMPITANCY_ASSESSMENT ECA " +
                                  " where ECA.ASSESSMENT_TOKEN in ( " +
                                                " SELECT CAD.ASSESSMENT_TOKEN " +
                                                " FROM COMPETENCY_ASSESSMENT_DETAILS CAD " +
                                                " WHERE CAD.COMPETENCY_ID = '"+competencyId+"')";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
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
                resultTable.Dispose();
            }
        }

    }
}
