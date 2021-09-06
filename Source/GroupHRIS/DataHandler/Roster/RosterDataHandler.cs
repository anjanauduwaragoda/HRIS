using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Common;
using NLog;

namespace DataHandler.Roster
{
    public class RosterDataHandler : TemplateDataHandler
    {
        private static Logger log = LogManager.GetCurrentClassLogger();



        public DataTable getRosterTypes()
        {

            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT * FROM ROSTER_TYPES " +
                                       " order by ROSTER_TYPE";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable populate()
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT r.*, rt.DESCRIPTION " +
                                        " FROM ROSTER r, ROSTER_TYPES rt " +
                                        " where r.ROSTER_TYPE = rt.ROSTER_TYPE ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable populate(String companyCode)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT r.*, rt.DESCRIPTION " +
                                        " FROM ROSTER r, ROSTER_TYPES rt " +
                                        " where r.ROSTER_TYPE = rt.ROSTER_TYPE " +
                                        "and r.COMPANY_ID = '" + companyCode + "' ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable populateForDropDown()
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT ROSTR_ID,  concat(FROM_TIME , ' - ', TO_TIME ) as ROSTER_DESC " +
                                        " FROM ROSTER " +
                                        " WHERE STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE +  "' " +
                                        " ORDER BY FROM_TIME " ;

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable populateForDropDown(string compCode)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT ROSTR_ID,  concat(FROM_TIME , ' - ', TO_TIME ) as ROSTER_DESC " +
                                        " FROM ROSTER " +
                                        " WHERE STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' " +
                                        " AND COMPANY_ID = '" + compCode.Trim() + "' " +
                                        " ORDER BY FROM_TIME ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /*

         */


        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Insert Rosters 
        ///</summary>
        //----------------------------------------------------------------------------------------
        public Boolean Insert(String companyCode,
                              String fromTime,
                              String toTime,
                              String flexibleTime,
                              String rosterType,
                              String userID,
                              String status,
                              String numDays)
        {
            Boolean bInserted = false;
            String statusCode = "";

            if (status.Equals(Constants.STATUS_ACTIVE_TAG))
                statusCode = Constants.STATUS_ACTIVE_VALUE;
            else
                statusCode = Constants.STATUS_INACTIVE_VALUE;

            SerialHandler serialHandler = new SerialHandler();

            string rosterId = "";
            
            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                rosterId = serialHandler.getserila(mySqlCon, Constants.ROSTER_ID_STAMP);

                mySqlCmd.Parameters.Add(new MySqlParameter("@rosterId", rosterId.Trim() == "" ? (object)DBNull.Value : rosterId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@companyCode", companyCode.Trim() == "" ? (object)DBNull.Value : companyCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@fromTime", fromTime.Trim() == "" ? (object)DBNull.Value : fromTime.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@toTime", toTime.Trim() == "" ? (object)DBNull.Value : toTime.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@flexibleTime", flexibleTime.Trim() == "" ? (object)DBNull.Value : flexibleTime.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@rosterType", rosterType.Trim() == "" ? (object)DBNull.Value : rosterType.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@numDays", numDays.Trim() == "" ? (object)DBNull.Value : numDays.Trim()));


                sMySqlString = " INSERT INTO ROSTER " +
                                "(ROSTR_ID," +
                                "FROM_TIME," +
                                "TO_TIME," +
                                "STATUS_CODE," +
                                "FLEXIBLE_TIME," +
                                "ROSTER_TYPE," +
                                "NUM_DAYS," +
                                "COMPANY_ID,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE) " +
                                "VALUES" +
                                "(@rosterId," +
                                "@fromTime," +
                                "@toTime," +
                                "@statusCode," +
                                "@flexibleTime," +
                                "@rosterType," +
                                "@numDays," +
                                "@companyCode," +
                                "@userID,now(),@userID,now())";



                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();


                bInserted = true;
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
                if (!bInserted) 
                    mySqlTrans.Rollback();
                else
                    mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                serialHandler = null;
            }

            return bInserted;
        }




        public Boolean Update(String rosterID,
                              String companyCode,
                              String fromTime,
                              String toTime,
                              String flexibleTime,
                              String rosterType,
                              String userID,
                              String status,
                              String numDays)
        {
            Boolean bUpdated = false;
            String statusCode = "";

            if (status.Equals(Constants.STATUS_ACTIVE_TAG))
                statusCode = Constants.STATUS_ACTIVE_VALUE;
            else
                statusCode = Constants.STATUS_INACTIVE_VALUE;


            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@rosterId", rosterID.Trim() == "" ? (object)DBNull.Value : rosterID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@companyCode", companyCode.Trim() == "" ? (object)DBNull.Value : companyCode.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@fromTime", fromTime.Trim() == "" ? (object)DBNull.Value : fromTime.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@toTime", toTime.Trim() == "" ? (object)DBNull.Value : toTime.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@flexibleTime", flexibleTime.Trim() == "" ? (object)DBNull.Value : flexibleTime.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@rosterType", rosterType.Trim() == "" ? (object)DBNull.Value : rosterType.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@numDays", numDays.Trim() == "" ? (object)DBNull.Value : numDays.Trim()));


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                sMySqlString = " UPDATE ROSTER " +
                                " SET " +
                                " FROM_TIME = @fromTime, " +
                                " TO_TIME = @toTime, " +
                                " STATUS_CODE = @statusCode, " +
                                " FLEXIBLE_TIME = @flexibleTime, " +
                                " ROSTER_TYPE = @rosterType, " +
                                " NUM_DAYS = @numDays, " +
                                " COMPANY_ID = @companyCode, " +
                                " MODIFIED_BY = @userID, " +
                                " MODIFIED_DATE = now() " +
                                " WHERE ROSTR_ID = @rosterId ";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                bUpdated = true;
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
            return bUpdated;
        }


        public List<string> getTimeRangeForRoster(string empID)
        {
            List<string> result;

            try
            {
                string sMySqlString = "SELECT concat(FROM_TIME,'" + Constants.CON_FIELD_SEPARATOR + "',TO_TIME,'" + Constants.CON_FIELD_SEPARATOR + "',ROSTER_TYPE)  " +
                                        " FROM ROSTER  " +
                                        " where ROSTR_ID = @empID ";


                mySqlCmd.CommandText = sMySqlString;
                
                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@empID", empID.Trim() == "" ? (object)DBNull.Value : empID.Trim()));

                mySqlCon.Open();

                result = mySqlCmd.ExecuteScalar().ToString().Split(Constants.CON_FIELD_SEPARATOR).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                mySqlCmd.Dispose();
            }

            return result;
        }


        public bool isAlreadyExists(string compCode, string fromTime, string toTime)
        {
            bool isDuplicateExists = false;
            int iRecs = 0;

            try
            {
                string sMySqlString = " SELECT count(*) FROM ROSTER " +
                                      " where COMPANY_ID = '"+ compCode + "' " +
                                      " and FROM_TIME = '" + fromTime +  "' " +
                                      " and TO_TIME = '" + toTime + "' ";


                mySqlCmd.CommandText = sMySqlString;

                //mySqlCmd.Parameters.Add(new MySqlParameter("@compCode", compCode.Trim() == "" ? (object)DBNull.Value : compCode.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@fromTime", fromTime.Trim() == "" ? (object)DBNull.Value : fromTime.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@toTime", toTime.Trim() == "" ? (object)DBNull.Value : toTime.Trim()));

                mySqlCon.Open();

                iRecs = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());

                if (iRecs != 0)
                    isDuplicateExists = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mySqlCon.Close();
                mySqlCmd.Dispose();
            }


            return isDuplicateExists;
        }





    }


}
