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
    public class CompetencyGroupDataHandler : TemplateDataHandler
    {
            public Boolean Insert(String groupName,
                                  String description,
                                  String statusCode,                              
                                  String addedBy)
            {
                Boolean blInserted = false;

                string sMySqlString = "";
                string sGroupID = "";

                MySqlTransaction mySqlTrans = null;
            
                try
                {
                    mySqlCmd.Parameters.Clear();
                    mySqlCmd.Parameters.Add(new MySqlParameter("@GroupName", groupName.Trim() == "" ? (object)DBNull.Value : groupName.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@Description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));               
                    mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

                    mySqlCon.Open();

                    mySqlTrans = mySqlCon.BeginTransaction();

                    SerialHandler serialHandler = new SerialHandler();
                    sGroupID = serialHandler.getserilalReference(ref mySqlCon, "CG");

                    mySqlCmd.Parameters.Add(new MySqlParameter("@GroupId", sGroupID.Trim() == "" ? (object)DBNull.Value : sGroupID.Trim()));

                    sMySqlString = " INSERT INTO COMPETENCY_GROUP(COMPETENCY_GROUP_ID,COMPETENCY_GROUP_NAME,DESCRIPTION,STATUS_CODE,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) " +
                                    " VALUES(@GroupId,@GroupName,@Description,@statusCode,@addedBy,now(),@addedBy,now())";


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

        
            public Boolean Update(String GroupId,
                                  String groupName,
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
                    mySqlCmd.Parameters.Add(new MySqlParameter("@GroupId", GroupId.Trim() == "" ? (object)DBNull.Value : GroupId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@GroupName", groupName.Trim() == "" ? (object)DBNull.Value : groupName.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@Description", description.Trim() == "" ? (object)DBNull.Value : description.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@addedBy", addedBy.Trim() == "" ? (object)DBNull.Value : addedBy.Trim()));

                    mySqlCon.Open();

                    mySqlTrans = mySqlCon.BeginTransaction();


                    sMySqlString = " UPDATE COMPETENCY_GROUP set COMPETENCY_GROUP_NAME =@GroupName,DESCRIPTION=@Description,STATUS_CODE=@statusCode,MODIFIED_BY=@addedBy,MODIFIED_DATE=now() where COMPETENCY_GROUP_ID =@GroupId";

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
                    string sMySqlString =  " SELECT COMPETENCY_GROUP_ID,COMPETENCY_GROUP_NAME,DESCRIPTION," +
	                                       " case when STATUS_CODE = '1' then 'Active'" +
	                                       " 	when STATUS_CODE = '0' then 'Inactive'" +
	                                       "    end as Status" +
                                           " FROM hris_dev.COMPETENCY_GROUP";
                    MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                    mySqlDa.Fill(dataTable);

                    return dataTable;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
        }


            public DataRow populate(string groupId)
            {

                try
                {
                    //mySqlCmd.Parameters.Clear();

                    ////mySqlCmd.Parameters.Add(new MySqlParameter("@GroupId", groupId.Trim() == "" ? (object)DBNull.Value : groupId.Trim()));

                    //mySqlCmd.Parameters.Add(new MySqlParameter("@GroupId", groupId.Trim()));

                    dataTable.Rows.Clear();
                    string sMySqlString =   " SELECT COMPETENCY_GROUP_ID,COMPETENCY_GROUP_NAME,DESCRIPTION,STATUS_CODE " +
                                            " FROM COMPETENCY_GROUP " +
                                            " where COMPETENCY_GROUP_ID = '" + groupId.Trim() + "'";

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

            public Boolean isCompetancyGroupExist(string groupName)
            {
                Boolean isExsists = false;
                try
                {
                    if (mySqlCon.State == ConnectionState.Closed)
                    {
                        mySqlCon.Open();
                    }
                    dataTable = new DataTable();


                    string queryStr = " SELECT COMPETENCY_GROUP_NAME FROM COMPETENCY_GROUP where upper(REPLACE(COMPETENCY_GROUP_NAME, ' ', '')) = '" + groupName.Trim() + "'";

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

            public Boolean isCompetancyGroupExist(string groupName, string groupId)
            {
                Boolean isExsists = false;
                try
                {
                    if (mySqlCon.State == ConnectionState.Closed)
                    {
                        mySqlCon.Open();
                    }
                    dataTable = new DataTable();


                    string queryStr = " SELECT COMPETENCY_GROUP_NAME FROM COMPETENCY_GROUP where upper(REPLACE(COMPETENCY_GROUP_NAME, ' ', '')) = '" + groupName.Trim() + "' and COMPETENCY_GROUP_ID <> '" + groupId.Trim() + "'";

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


    }
}
