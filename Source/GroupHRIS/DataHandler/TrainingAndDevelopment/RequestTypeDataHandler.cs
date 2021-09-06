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
    public class RequestTypeDataHandler : TemplateDataHandler
    {
        public Boolean Insert(String requestTypeName,
                              String description,
                              String statusCode,
                              String addedBy)
        {
            Boolean blInserted = false;

            string sMySqlString = "";
            string sRequestTypeID = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@RequestTypeName", requestTypeName.Trim() == "" ? (object)DBNull.Value : requestTypeName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                SerialHandler serialHandler = new SerialHandler();
                sRequestTypeID = serialHandler.getserilalReference(ref mySqlCon, "RTI");

                mySqlCmd.Parameters.Add(new MySqlParameter("@RequestTypeID", sRequestTypeID.Trim() == "" ? (object)DBNull.Value : sRequestTypeID.Trim()));

                sMySqlString = " INSERT INTO REQUEST_TYPE(REQUEST_TYPE_ID,TYPE_NAME,DESCRIPTION,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) " +
                                " VALUES(@RequestTypeID,@RequestTypeName,@Description,@statusCode,@addedBy,now(),@addedBy,now())";


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

        public Boolean Update(String requestTypeId,
                              String requestTypeName,
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
                mySqlCmd.Parameters.Add(new MySqlParameter("@RequestTypeID", requestTypeId.Trim() == "" ? (object)DBNull.Value : requestTypeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@CategoryName", requestTypeName.Trim() == "" ? (object)DBNull.Value : requestTypeName.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@Description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                sMySqlString = " UPDATE REQUEST_TYPE set TYPE_NAME =@CategoryName,DESCRIPTION=@Description,STATUS_CODE=@statusCode,MODIFIED_BY=@addedBy,MODIFIED_DATE=now() where REQUEST_TYPE_ID =@requestTypeId";

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
                dataTable.Clear();
                string sMySqlString = " SELECT REQUEST_TYPE_ID,TYPE_NAME,DESCRIPTION, " +
	                                  "   case when STATUS_CODE='1' then 'Active' " +
                                      "	       when  STATUS_CODE='0' then 'Inactive' " +
                                      "   end as STATUS  " +
                                      " FROM REQUEST_TYPE  " +
                                      " ORDER BY TYPE_NAME;";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow populate(string requestTypeId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString =   " SELECT REQUEST_TYPE_ID,TYPE_NAME,DESCRIPTION,STATUS_CODE " +
                                        " FROM REQUEST_TYPE where REQUEST_TYPE_ID ='" + requestTypeId.Trim() + "'";
                    
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


        public DataTable getRequestTypeNameAndId()
        {
            try
            {
                dataTable.Clear();
                string sMySqlString = " SELECT REQUEST_TYPE_ID,TYPE_NAME " +                                      
                                      " FROM REQUEST_TYPE  " +
                                      " WHERE STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "' " +
                                      " ORDER BY TYPE_NAME;";
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
