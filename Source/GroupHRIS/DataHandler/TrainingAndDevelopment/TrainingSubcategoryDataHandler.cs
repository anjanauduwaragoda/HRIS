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
    public class TrainingSubcategoryDataHandler :TemplateDataHandler
    {
        public Boolean Insert(String categoryId,
                              String subCategoryName,
                              String description,
                              String statusCode,
                              String addedBy)
        {
            Boolean blInserted = false;

            string sMySqlString = "";
            string sSubCategoryID = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@CategoryId", categoryId.Trim() == "" ? (object)DBNull.Value : categoryId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@SubCategoryName", subCategoryName.Trim() == "" ? (object)DBNull.Value : subCategoryName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler serialHandler = new SerialHandler();
                sSubCategoryID = serialHandler.getserilalReference(ref mySqlCon, "TSC");

                mySqlCmd.Parameters.Add(new MySqlParameter("@SubCategoryId", sSubCategoryID.Trim() == "" ? (object)DBNull.Value : sSubCategoryID.Trim()));

                sMySqlString = " INSERT INTO TRAINING_SUB_CATEGORY(TYPE_ID,CATEGORY_ID,TYPE_NAME,DESCRIPTION,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) " +
                                " VALUES(@SubCategoryId,@CategoryId,@SubCategoryName,@Description,@statusCode,@addedBy,now(),@addedBy,now())";


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

        public Boolean Update(String sSubCategoryID,
                              String categoryId,
                              String subCategoryName,
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
                mySqlCmd.Parameters.Add(new MySqlParameter("@SubCategoryId", sSubCategoryID.Trim() == "" ? (object)DBNull.Value : sSubCategoryID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@CategoryId", categoryId.Trim() == "" ? (object)DBNull.Value : categoryId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@SubCategoryName", subCategoryName.Trim() == "" ? (object)DBNull.Value : subCategoryName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                sMySqlString = " UPDATE TRAINING_SUB_CATEGORY set CATEGORY_ID = @CategoryId,TYPE_NAME = @SubCategoryName,DESCRIPTION = @Description,STATUS_CODE = @statusCode, " +
                               " MODIFIED_BY = @addedBy,MODIFIED_DATE = now() where TYPE_ID = @SubCategoryId";
                

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
                string sMySqlString = " SELECT tsc.CATEGORY_ID,tsc.TYPE_ID,tc.CATEGORY_NAME,tsc.TYPE_NAME,tsc.DESCRIPTION, " +
	                                  " case when tsc.STATUS_CODE ='1' then 'Active' " +
		                              "      when tsc.STATUS_CODE ='0' then 'Inactive'  " +
	                                  "      end as STATUS  " +
                                      " FROM TRAINING_SUB_CATEGORY tsc, TRAINING_CATEGORY tc  " +
                                      " where tsc.CATEGORY_ID = tc.TRAINING_CATEGORY_ID  " +
                                      " Order by CATEGORY_NAME,TYPE_NAME ";
                                    
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populate(String sCategoryId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT tsc.CATEGORY_ID,tsc.TYPE_ID,tc.CATEGORY_NAME,tsc.TYPE_NAME,tsc.DESCRIPTION, " +
                                      " case when tsc.STATUS_CODE ='1' then 'Active' " +
                                      "      when tsc.STATUS_CODE ='0' then 'Inactive'  " +
                                      "      end as STATUS  " +
                                      " FROM TRAINING_SUB_CATEGORY tsc, TRAINING_CATEGORY tc  " +
                                      " where tsc.CATEGORY_ID = tc.TRAINING_CATEGORY_ID and tsc.CATEGORY_ID = '" + sCategoryId.Trim() + "' " +
                                      " Order by TYPE_NAME ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataRow getSubCategory(String sTypeId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT tsc.CATEGORY_ID,tsc.TYPE_ID,tc.CATEGORY_NAME,tsc.TYPE_NAME,tsc.DESCRIPTION,tsc.STATUS_CODE " +
                                      " FROM TRAINING_SUB_CATEGORY tsc, TRAINING_CATEGORY tc  " +
                                      " where tsc.CATEGORY_ID = tc.TRAINING_CATEGORY_ID and TYPE_ID ='" + sTypeId.Trim() + "'";
                                     

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

        public Boolean isActiveSubcategoryExist(String categoryId)
        {
            Boolean isSubCatExist = false;

            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT TYPE_ID FROM TRAINING_SUB_CATEGORY where CATEGORY_ID='" + categoryId.Trim() + "' and STATUS_CODE='1'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);


                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0) { isSubCatExist = true; }

                return isSubCatExist;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getSubcategoryNameAndId(string sCategory)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString =   " SELECT TYPE_NAME,TYPE_ID " +
                                        " FROM TRAINING_SUB_CATEGORY " +
                                        " where CATEGORY_ID='" + sCategory.Trim() + "' and STATUS_CODE='1'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getSubcategoryNameAndIdWithInactiveSubcategory(string sCategory,string sSubcategory)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT TYPE_NAME,TYPE_ID " +
                                        " FROM TRAINING_SUB_CATEGORY " +
                                        " where CATEGORY_ID='" + sCategory.Trim() + "' and (STATUS_CODE='1' or TYPE_ID='" + sSubcategory.Trim() + "')";

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
