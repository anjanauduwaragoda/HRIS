using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySql.Data.Common;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class 
        AssessmentTypeDataHandler : TemplateDataHandler
    {
        public Boolean Insert(string assessmentTypeName, string description, string status, string addedUserId)
        {
            Boolean isSuccess = false;
            MySqlTransaction mySqlTrans = null;
            string sqlQuery = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler serialCode = new SerialHandler();
                string assessmentTypeId = serialCode.getserila(mySqlCon, "AT");

                mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentTypeId", assessmentTypeId.Trim() == "" ? (object)DBNull.Value : assessmentTypeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentTypeName", assessmentTypeName.Trim() == "" ? (object)DBNull.Value : assessmentTypeName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                sqlQuery = "INSERT INTO ASSESSMENT_TYPE (ASSESSMENT_TYPE_ID, ASSESSMENT_TYPE_NAME, DESCRIPTION, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE)" +
                           " values (@assessmentTypeId,@assessmentTypeName,@description,@status,@addedUserId,now(),@addedUserId,now())";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sqlQuery;
                mySqlCmd.ExecuteNonQuery();
                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                mySqlTrans.Rollback();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }


                isSuccess = false;
                throw ex;
            }
            finally
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
            return isSuccess;
        }

        public Boolean CheckAssessmentTypeNameExsistance(string assessmentTypeName) /// this methode is used when saving an entry
        {
            Boolean nameIsExsists = false;
            dataTable.Rows.Clear();

            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                string sqlQuery = "SELECT * FROM ASSESSMENT_TYPE WHERE ASSESSMENT_TYPE_NAME ='" + assessmentTypeName + "'";
                MySqlDataAdapter da = new MySqlDataAdapter(sqlQuery, mySqlCon);
                da.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    nameIsExsists = true;
                }
                else
                {
                    nameIsExsists = false;
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

        public Boolean CheckAssessmentTypeNameExsistance(string assessmentTypeName, string id) /// this methode is used when updating an entry
        {
            Boolean nameIsExsists = false;
            dataTable.Rows.Clear();

            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                string sqlQuery = "SELECT * FROM ASSESSMENT_TYPE WHERE ASSESSMENT_TYPE_NAME ='" + assessmentTypeName + "'";
                MySqlDataAdapter da = new MySqlDataAdapter(sqlQuery, mySqlCon);
                da.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["ASSESSMENT_TYPE_ID"].ToString() == id)
                        {
                            nameIsExsists = false;
                        }
                        else
                        {
                            nameIsExsists = true;
                        }
                    }
                }
                else
                {
                    nameIsExsists = false;
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

        public DataTable Populate()
        {
            dataTable.Rows.Clear();
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                string sqlQuery = "SELECT ASSESSMENT_TYPE_ID, ASSESSMENT_TYPE_NAME, DESCRIPTION, " +
                                      "CASE" +
                                      " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then 'Active' " +
                                      " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then 'Inactive' " +
                                      " END AS STATUS_CODE " +
                                      " FROM ASSESSMENT_TYPE ORDER BY ASSESSMENT_TYPE_NAME";

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
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

        public DataRow GetElementById(string assessmentTypeId)
        {
            dataTable.Rows.Clear();
            DataRow row = null;
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                string sqlQuery = "SELECT ASSESSMENT_TYPE_NAME, DESCRIPTION, STATUS_CODE FROM ASSESSMENT_TYPE " +
                                   "WHERE ASSESSMENT_TYPE_ID ='" + assessmentTypeId + "'";

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
                dataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    row = dataTable.Rows[0];
                }
                dataAdapter.Dispose();
                return row;
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

        public Boolean Update(string assessmentTypeId, string assesmentTypeName, string description, string status, string addedUser)
        {
            Boolean isSuccess = false;
            MySqlTransaction mySqlTrans = null;
            string sqlQuery = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentTypeId", assessmentTypeId.Trim() == "" ? (object)DBNull.Value : assessmentTypeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentTypeName", assesmentTypeName.Trim() == "" ? (object)DBNull.Value : assesmentTypeName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));

                sqlQuery = "UPDATE ASSESSMENT_TYPE SET " +
                           " ASSESSMENT_TYPE_NAME = @assessmentTypeName, " +
                           " DESCRIPTION = @description, "+
                           " STATUS_CODE = @status, " +
                           " MODIFIED_BY = @addedUserId, " +
                           " MODIFIED_DATE = now() " +
                           " WHERE ASSESSMENT_TYPE_ID = @assessmentTypeId ";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sqlQuery;
                mySqlCmd.ExecuteNonQuery();
                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                mySqlTrans.Rollback();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Close();
                }

                

                throw ex;
            }

            return isSuccess;
        }

        public Boolean Update(string assessmentTypeId, string status, string addedUser)
        {
            Boolean isSuccess = false;
            MySqlTransaction mySqlTrans = null;
            string sqlQuery = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentTypeId", assessmentTypeId.Trim() == "" ? (object)DBNull.Value : assessmentTypeId.Trim()));

                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));

                sqlQuery = "UPDATE ASSESSMENT_TYPE SET " +
                           " STATUS_CODE = @status, " +
                           " MODIFIED_BY = @addedUserId, " +
                           " MODIFIED_DATE = now() " +
                           " WHERE ASSESSMENT_TYPE_ID = @assessmentTypeId ";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sqlQuery;
                mySqlCmd.ExecuteNonQuery();
                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                mySqlTrans.Rollback();
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Close();
                }



                throw ex;
            }

            return isSuccess;
        }

        public DataTable getUsedAssessments(string assessmentTypeId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                string sqlQuery = " SELECT ASSESSMENT_ID, STATUS_CODE FROM ASSESSMENT WHERE ASSESSMENT_TYPE_ID = '" + assessmentTypeId + "' ";
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
                resultTable.Dispose();
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
            }
        }
    }
}
