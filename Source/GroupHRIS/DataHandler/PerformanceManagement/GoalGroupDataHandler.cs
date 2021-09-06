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
    public class GoalGroupDataHandler : TemplateDataHandler
    {
        public Boolean Insert(string groupName, string description, string status, string addedUserId)
        {

            Boolean inserted = false;
            MySqlTransaction mySqlTrans = null;
            string mySqlQuery = null;

            try {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler serialCode = new SerialHandler();
                string groupId = serialCode.getserila(mySqlCon, "GG");

                mySqlCmd.Parameters.Add(new MySqlParameter("@groupId", groupId.Trim() == "" ? (object)DBNull.Value : groupId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@TrainingName", groupName.Trim() == "" ? (object)DBNull.Value : groupName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));


                mySqlQuery = "insert into GOAL_GROUP (GOAL_GROUP_ID, GROUP_NAME, DESCRIPTION, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE)" + 
                             " values (@groupId,@TrainingName,@description,@status,@addedUserId,now(),@addedUserId,now())";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = mySqlQuery;
                mySqlCmd.ExecuteNonQuery();
                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                inserted = true;
            }
            catch (Exception e) {
                mySqlTrans.Rollback();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                
                inserted = false;
                throw e;
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
                string queryString = "SELECT GOAL_GROUP_ID, GROUP_NAME, DESCRIPTION, STATUS_CODE, " +
                                     " CASE " +
                                     " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then 'Inactive' " +
                                     " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then 'Active' " +
                                     " End  as STATUS_CODE1 " +
                                     " FROM GOAL_GROUP ORDER BY GROUP_NAME";

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

        public DataRow populate(string goalGroupId)
        {
            try
            {
                mySqlCon.Open();

                dataTable.Rows.Clear();
                string queryString = "select * from GOAL_GROUP  where GOAL_GROUP_ID = '" + goalGroupId + "' ";

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

        public Boolean Update(string groupId, string groupName, string description, string status, string UserId)
        {
            Boolean isUpdated = false;
            MySqlTransaction mySqlTrans = null;
            string mySqlQuery = null;

            try
            {
                mySqlCon.Open();
                
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@groupId", groupId.Trim() == "" ? (object)DBNull.Value : groupId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@TrainingName", groupName.Trim() == "" ? (object)DBNull.Value : groupName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@UserId", UserId.Trim() == "" ? (object)DBNull.Value : UserId.Trim()));

                mySqlQuery = "UPDATE GOAL_GROUP SET " +
                    "GROUP_NAME = @TrainingName, " +
                    "DESCRIPTION = @description, " +
                    "STATUS_CODE = @status, " +
                    "MODIFIED_BY = @UserId, " +
                    "MODIFIED_DATE = now() " +
                    "WHERE GOAL_GROUP_ID = @groupId";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = mySqlQuery;
                mySqlCmd.ExecuteNonQuery();
                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isUpdated = true;

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
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }

            return isUpdated;
        }

        public Boolean CheckGoalNameExsistance(string groupName) 
        {
            Boolean isExsists = false;
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                dataTable.Rows.Clear();


                string queryStr = "SELECT * FROM GOAL_GROUP WHERE upper(REPLACE(GROUP_NAME, ' ', '')) ='" + groupName + "'";

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
            finally {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return isExsists;
        }

        public Boolean CheckGoalNameExsistance(string groupName, string id)
        {

            dataTable.Rows.Clear();
            Boolean isExsists = false;

            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                string queryStr = "SELECT * FROM GOAL_GROUP WHERE upper(REPLACE(GROUP_NAME, ' ', '')) = '" + groupName + "' and GOAL_GROUP_ID <> '" + id.Trim() + "'";

                MySqlDataAdapter mySqlDA = new MySqlDataAdapter(queryStr, mySqlCon);
                mySqlDA.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExsists = true;

                    //foreach (DataRow row in dataTable.Rows)
                    //{
                    //    if (row["GOAL_GROUP_ID"].ToString() == id)
                    //    {
                    //        isExsists = false;
                    //    }
                    //    else
                    //    {
                    //        isExsists = true;
                    //    }
                    //}
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

        public bool IsUsed(string goalGroupId)
        {
            bool isUsed = false;
            try
            {

                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}

                string sqlQuery = "SELECT GOAL_ID FROM EMPLOYEE_GOALS WHERE GOAL_GROUP_ID ='" + goalGroupId + "' && STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "'";
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                DataTable resultTable = new DataTable();
                mySqlDataAdapter.Fill(resultTable);

                if (resultTable.Rows.Count > 0)
                {
                    isUsed = true;
                }
                return isUsed;

            }
            catch (Exception)
            {

                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return isUsed;
        }
    }
}
