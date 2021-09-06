using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.PerformanceManagement
{
    public class AssessmentPurposeDataHandler : TemplateDataHandler
    {
        public Boolean Insert(string purposeName, string description, string status, string addedUserId)
        {
            
            Boolean isSuccess = false;
            MySqlTransaction mySqlTrans = null;
            string sqlQuery = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler serialCode = new SerialHandler();
                string assessmentPurposeId = serialCode.getserila(mySqlCon, "AP");

                mySqlCmd.Parameters.Add(new MySqlParameter("@assessmentPurposeId", assessmentPurposeId.Trim() == "" ? (object)DBNull.Value : assessmentPurposeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@purposeName", purposeName.Trim() == "" ? (object)DBNull.Value : purposeName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                sqlQuery = "INSERT INTO ASSESSMENT_PURPOSE (PURPOSE_ID, NAME, DESCRIPTION, STATUS_CODE, ADDED_BY, ADDED_DATE,MODIFIED_BY, MODIFIED_DATE)" +
                           " values (@assessmentPurposeId,@purposeName,@description,@status,@addedUserId,now(),@addedUserId,now())";

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
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                mySqlTrans.Rollback();
                isSuccess = false;
                throw ex;
            }
            return isSuccess;
        }

        public Boolean CheckNameExsistance(string assessmentPurpose) /// used in saving a record
        {
            Boolean nameIsExsists = false;
            dataTable.Rows.Clear();

            try
            {
                string sqlQuery = "SELECT * FROM ASSESSMENT_PURPOSE WHERE NAME ='" + assessmentPurpose + "'";
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

        public Boolean CheckNameExsistance(string assessmentPurpose, string id) /// used in updating a record
        {
            Boolean nameIsExsists = false;
            dataTable.Rows.Clear();

            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                string sqlQuery = "SELECT * FROM ASSESSMENT_PURPOSE WHERE NAME ='" + assessmentPurpose + "'";
                MySqlDataAdapter da = new MySqlDataAdapter(sqlQuery, mySqlCon);
                da.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["PURPOSE_ID"].ToString() == id)
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
                string sqlQuery = "SELECT PURPOSE_ID, NAME, DESCRIPTION, " +
                                      "CASE" +
                                      " when STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then 'Active' " +
                                      " when STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then 'Inactive' " +
                                      " END AS STATUS_CODE " +
                                      " FROM ASSESSMENT_PURPOSE ORDER BY PURPOSE_ID";

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

        public DataRow GetElementById(string purposeId)
        {
            dataTable.Rows.Clear();
            DataRow row = null;
            try
            {
                //if (mySqlCon.State == ConnectionState.Closed)
                //{
                    mySqlCon.Open();
                //}
                string sqlQuery = "SELECT NAME, DESCRIPTION, STATUS_CODE FROM ASSESSMENT_PURPOSE " +
                                   "WHERE PURPOSE_ID ='" + purposeId + "'";

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

        public Boolean Update(string purposeId, string name, string description, string status, string addedUser)
        {
            Boolean isSuccess = false;
            MySqlTransaction mySqlTrans = null;
            string sqlQuery = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@purposeId", purposeId.Trim() == "" ? (object)DBNull.Value : purposeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@name", name.Trim() == "" ? (object)DBNull.Value : name.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUser.Trim() == "" ? (object)DBNull.Value : addedUser.Trim()));

                sqlQuery = "UPDATE ASSESSMENT_PURPOSE SET " +
                           " NAME = @name, " +
                           " DESCRIPTION = @description, " +
                           " STATUS_CODE = @status, " +
                           " MODIFIED_BY = @addedUserId, " +
                           " MODIFIED_DATE = now() " +
                           " WHERE PURPOSE_ID = @purposeId ";

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
                if (mySqlCon.State == ConnectionState.Closed)
                {
                    mySqlCon.Close();
                }

                mySqlTrans.Rollback();

                throw ex;
            }

            return isSuccess;
        }
    }
}
