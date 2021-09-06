using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Common;
using MySql.Data.MySqlClient;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainingInstituteProgrameDataHandler : TemplateDataHandler
    {
        public DataTable getAllPrograms()
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                string sqlQuery = " SELECT PROGRAM_ID, PROGRAM_CODE, PROGRAM_NAME, MINIMUM_BATCH_SIZE, MAXIMUM_BATCH_SIZE, "+
                                    " CASE " +
                                        " when PROGRAM_TYPE='" + Constants.PROGRAME_TYPE_LONG_TERM_VALUE + "' then '"+Constants.PROGRAME_TYPE_LONG_TERM_TAG+"' " +
                                        " when PROGRAM_TYPE ='" + Constants.PROGRAME_TYPE_SHORT_TERM_VALUE + "' then '"+Constants.PROGRAME_TYPE_SHORT_TERM_TAG +"' " +
                                    " End  as PROGRAM_TYPE " +
                                    " FROM TRAINING_PROGRAM WHERE STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ORDER BY PROGRAM_NAME";
                MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlQuery, mySqlCon);
                sqlDa.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                mySqlCon.Close();
            }
        }

        public DataTable getProgramById(string programId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                string sqlQuery = "SELECT PROGRAM_NAME,DESCRIPTION,OBJECTIVES,PROGRAM_DURATION,MINIMUM_BATCH_SIZE,MAXIMUM_BATCH_SIZE,PROGRAM_CODE, "+
                                    " CASE " +
                                        " when PROGRAM_TYPE='" + Constants.PROGRAME_TYPE_LONG_TERM_VALUE + "' then '"+Constants.PROGRAME_TYPE_LONG_TERM_TAG+"' " +
                                        " when PROGRAM_TYPE ='" + Constants.PROGRAME_TYPE_SHORT_TERM_VALUE + "' then '"+Constants.PROGRAME_TYPE_SHORT_TERM_TAG +"' " +
                                    " End  as PROGRAM_TYPE " +
                                    " FROM TRAINING_PROGRAM WHERE PROGRAM_ID ='" + programId + "' ";
                MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlQuery, mySqlCon);
                sqlDa.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                mySqlCon.Close();
            }
        }

        public bool addProgrameToInstitute(string instituteId, string programeId, string status, string addedUserId)
        {
            bool inserted = false;
         
            try
            {
                mySqlCon.Open();

                mySqlCmd.Parameters.Add(new MySqlParameter("@instituteId", instituteId.Trim() == "" ? (object)DBNull.Value : instituteId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@programeId", programeId.Trim() == "" ? (object)DBNull.Value : programeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedUserId", addedUserId.Trim() == "" ? (object)DBNull.Value : addedUserId.Trim()));

                string sqlQuery =   " INSERT INTO TRAINING_INSTITUTE_PROGRAMS " +
                                    " (INSTITUTE_ID, TRAINING_PROGRAM_ID, STATUS_CODE, ADDED_BY, ADDED_DATE, MODIFIED_BY, MODIFIED_DATE) " +
                                    " values (@instituteId,@programeId,@status,@addedUserId,now(),@addedUserId,now())";

                mySqlCmd.CommandText = sqlQuery;
                mySqlCmd.ExecuteNonQuery();
                inserted = true;
                return inserted;

            }
            catch (Exception)
            {

                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
        }

        public bool checkProgrameExistance(string instituteId, string programId)
        {
            DataTable resultTable = new DataTable();
            bool isUsed = false;
            try
            {
                mySqlCon.Open();
                string sqlQuery = " SELECT INSTITUTE_ID FROM TRAINING_INSTITUTE_PROGRAMS WHERE INSTITUTE_ID ='" + instituteId + "' && TRAINING_PROGRAM_ID ='" + programId + "' ";

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
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
                return isUsed;
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

        public DataTable getAddedProgrames(string instituteId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                string sqlQuery =   " SELECT TP.PROGRAM_ID, TP.PROGRAM_CODE, TP.PROGRAM_NAME,  "+
                                    " CASE " +
                                        " when TIP.STATUS_CODE='" + Constants.STATUS_INACTIVE_VALUE + "' then 'Inactive' " +
                                        " when TIP.STATUS_CODE='" + Constants.STATUS_ACTIVE_VALUE + "' then 'Active' " +
                                    " End  as STATUS_CODE " +
                                    " FROM TRAINING_INSTITUTE_PROGRAMS TIP "+
                                    " join TRAINING_PROGRAM TP "+
	                                " on TIP.TRAINING_PROGRAM_ID = TP.PROGRAM_ID "+
                                    " WHERE TIP.INSTITUTE_ID = '"+instituteId+"' " ;

                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlQuery, mySqlCon);
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
                resultTable.Dispose();
            }
        }

        public bool addProgrameStatusInInstitute(string instituteId, string programId, string status, string UserId)
        {
            bool updated = false;
            try
            {
                mySqlCon.Open();

                mySqlCmd.Parameters.Add(new MySqlParameter("@instituteId", instituteId.Trim() == "" ? (object)DBNull.Value : instituteId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@programId", programId.Trim() == "" ? (object)DBNull.Value : programId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim() == "" ? (object)DBNull.Value : status.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@UserId", UserId.Trim() == "" ? (object)DBNull.Value : UserId.Trim()));

                string mySqlQuery = "UPDATE TRAINING_INSTITUTE_PROGRAMS SET " +
                                    "STATUS_CODE = @status, " +
                                    "MODIFIED_BY = @UserId, " +
                                    "MODIFIED_DATE = now() " +
                                    "WHERE INSTITUTE_ID = @instituteId "+
                                    " && TRAINING_PROGRAM_ID = @programId ";

                mySqlCmd.CommandText = mySqlQuery;
                mySqlCmd.ExecuteNonQuery();
                updated = true;
                return updated;
            }
            catch (Exception)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }
                throw;
            }
        }

        public DataTable getTrainingTypes()
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                string sqlQuery = "SELECT TRAINING_TYPE_ID, TYPE_NAME FROM TRAINING_TYPE WHERE STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";
                MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlQuery, mySqlCon);
                sqlDa.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                mySqlCon.Close();
            }
        }

        public DataTable getTrainingCategories()
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                string sqlQuery = "SELECT TRAINING_CATEGORY_ID, CATEGORY_NAME FROM TRAINING_CATEGORY WHERE STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";
                MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlQuery, mySqlCon);
                sqlDa.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                mySqlCon.Close();
            }
        }

        public DataTable getSubcategoriesForCategory(string categoryId)
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                string sqlQuery = "SELECT TYPE_ID, TYPE_NAME FROM TRAINING_SUB_CATEGORY WHERE CATEGORY_ID ='" + categoryId + "' && STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";
                MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlQuery, mySqlCon);
                sqlDa.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                mySqlCon.Close();
            }
        }

        public DataTable filterProgrames(string selectedType, string selectedCategory, string selectedSubcategory)
        {
            DataTable resultTable = new DataTable();
            try
            {
                mySqlCon.Open();
                string sqlQuery = "SELECT * FROM TRAINING_PROGRAM WHERE STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ";

                if (!String.IsNullOrEmpty(selectedType))
                {
                    sqlQuery += " and TRAINING_TYPE ='" + selectedType + "' ";
                }

                if (!String.IsNullOrEmpty(selectedCategory))
                {
                    sqlQuery += " and TRAINING_CATEGORY ='" + selectedCategory + "' ";
                }

                if (!String.IsNullOrEmpty(selectedSubcategory))
                {
                    sqlQuery += " and TRAINING_SUBCATEGORY ='" + selectedSubcategory + "' ";
                }
                MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlQuery, mySqlCon);
                sqlDa.Fill(resultTable);
                return resultTable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                resultTable.Dispose();
                mySqlCon.Close();
            }
        }
    }
}
