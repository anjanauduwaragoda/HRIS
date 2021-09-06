using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.TrainingAndDevelopment
{
    public class TrainingCategoryDataHandler : TemplateDataHandler
    {
        public Boolean Insert(String categoryName,
                              String description,
                              String statusCode,
                              String addedBy)
        {
            Boolean blInserted = false;

            string sMySqlString = "";
            string sCategoryID = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@CategoryName", categoryName.Trim() == "" ? (object)DBNull.Value : categoryName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler serialHandler = new SerialHandler();
                sCategoryID = serialHandler.getserilalReference(ref mySqlCon, "TC");

                mySqlCmd.Parameters.Add(new MySqlParameter("@CategoryId", sCategoryID.Trim() == "" ? (object)DBNull.Value : sCategoryID.Trim()));

                sMySqlString = " INSERT INTO TRAINING_CATEGORY(TRAINING_CATEGORY_ID,CATEGORY_NAME,CATEGORY_DESCRIPTION,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) " +
                                " VALUES(@CategoryId,@CategoryName,@Description,@statusCode,@addedBy,now(),@addedBy,now())";


                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                serialHandler = null;

                blInserted = true;
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

            return blInserted;
        }

        public Boolean Update(String categoryId,
                              String categoryName,
                              String description,
                              String statusCode,
                              String addedBy)
        {
            Boolean blUpdated = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@TCategoryId", categoryId.Trim() == "" ? (object)DBNull.Value : categoryId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@CategoryName", categoryName.Trim() == "" ? (object)DBNull.Value : categoryName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();


                sMySqlString = " UPDATE TRAINING_CATEGORY set CATEGORY_NAME =@CategoryName,CATEGORY_DESCRIPTION=@Description,STATUS_CODE=@statusCode,MODIFIED_BY=@addedBy,MODIFIED_DATE=now() where TRAINING_CATEGORY_ID =@TCategoryId";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                blUpdated = true;
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

            return blUpdated;
        }


        public DataTable populate()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT TRAINING_CATEGORY_ID,CATEGORY_NAME,CATEGORY_DESCRIPTION," +
                                       " case when STATUS_CODE = '1' then 'Active'" +
                                       " 	when STATUS_CODE = '0' then 'Inactive'" +
                                       "    end as STATUS" +
                                       " FROM TRAINING_CATEGORY ORDER BY CATEGORY_NAME";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataRow populate(string categoryId)
        {

            try
            {
                
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT TRAINING_CATEGORY_ID,CATEGORY_NAME,CATEGORY_DESCRIPTION,STATUS_CODE " +
                                        " FROM TRAINING_CATEGORY " +
                                        " where TRAINING_CATEGORY_ID = '" + categoryId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);

                mySqlDa.Fill(dataTable);
                DataRow dr = null;
                if (dataTable.Rows.Count > 0)
                {
                    dr = dataTable.Rows[0];
                }

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getCategories()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT TRAINING_CATEGORY_ID,CATEGORY_NAME,STATUS_CODE" +
                                      " FROM TRAINING_CATEGORY WHERE STATUS_CODE='1' order by CATEGORY_NAME";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable getCategories(String sStatus,string sCategoryId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT TRAINING_CATEGORY_ID,CATEGORY_NAME,STATUS_CODE" +
                                      " FROM TRAINING_CATEGORY WHERE STATUS_CODE='" + sStatus.Trim() + "' or TRAINING_CATEGORY_ID ='" + sCategoryId.Trim() + "' order by CATEGORY_NAME";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getCategoryNameAndId()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString =  " SELECT TRAINING_CATEGORY_ID,CATEGORY_NAME " +                                       
                                       " FROM TRAINING_CATEGORY where STATUS_CODE='1'" +
                                       " ORDER BY CATEGORY_NAME";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

    }
}
