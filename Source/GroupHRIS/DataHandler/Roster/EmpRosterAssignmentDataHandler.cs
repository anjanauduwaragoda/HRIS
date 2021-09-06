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
    public class EmpRosterAssignmentDataHandler : TemplateDataHandler
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public DataTable populate()
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT * " +
                                        " FROM EMPLOYEE_ROSTER_DATE ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populate(string empID, string rosterDate)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT erd.ROSTR_ID as ROSTR_ID, erd.EMPLOYEE_ID as EMPLOYEE_ID, DATE(erd.DUTY_DATE) as DUTY_DATE, erd.IS_OFF_DAY as IS_OFF_DAY, erd.IS_SUMMARIZED as IS_SUMMARIZED, erd.INTERCHANGE_NUMBER as INTERCHANGE_NUMBER, erd.DUTY_COVERED_BY as DUTY_COVERED_BY, erd.REASON as REASON, erd.STATUS_CODE as STATUS_CODE, r.FROM_TIME as FROM_TIME, r.TO_TIME as TO_TIME, r.ROSTER_TYPE as ROSTER_TYPE   " +
                                      " FROM EMPLOYEE_ROSTER_DATE erd, ROSTER r " +
                                      " WHERE r.ROSTR_ID = erd.ROSTR_ID " +
                                      " AND erd.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' "+
                                      " AND DUTY_DATE = '" + rosterDate + "' " +
                                      " AND EMPLOYEE_ID = '" + empID + "' ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populate(string empID, string fromDate, string toDate)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT erd.ROSTR_ID as ROSTR_ID, erd.EMPLOYEE_ID as EMPLOYEE_ID, DATE(erd.DUTY_DATE) as DUTY_DATE, erd.IS_OFF_DAY as IS_OFF_DAY, erd.IS_SUMMARIZED as IS_SUMMARIZED, erd.INTERCHANGE_NUMBER as INTERCHANGE_NUMBER, erd.DUTY_COVERED_BY as DUTY_COVERED_BY, erd.REASON as REASON, erd.STATUS_CODE as STATUS_CODE, r.FROM_TIME as FROM_TIME, r.TO_TIME as TO_TIME, r.ROSTER_TYPE as ROSTER_TYPE, rt.description as description, emp.FIRST_NAME " +
                                      " FROM ROSTER r, ROSTER_TYPES rt, EMPLOYEE_ROSTER_DATE erd  left outer join EMPLOYEE emp on emp.EMPLOYEE_ID = erd.DUTY_COVERED_BY " +
                                      " WHERE r.ROSTR_ID = erd.ROSTR_ID " +
                                      " AND r.ROSTER_TYPE = rt.ROSTER_TYPE " +
                                      " AND (DUTY_DATE BETWEEN '" + fromDate + "' AND '" + toDate + "') " +
                                      " AND erd.EMPLOYEE_ID = '" + empID + "' " +
                                      " AND erd.STATUS_CODE <> '" + Constants.STATUS_OBSOLETE_VALUE + "' order by DATE(erd.DUTY_DATE)";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateEmpProfile(string empID, string fromDate, string toDate)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT DATE_FORMAT(erd.DUTY_DATE,'%Y-%m-%d') as DUTY_DATE, erd.IS_SUMMARIZED as IS_SUMMARIZED, " +
                                      " erd.INTERCHANGE_NUMBER as INTERCHANGE_NUMBER,e.FIRST_NAME as FIRST_NAME," +
                                      " r.FROM_TIME as FROM_TIME,r.TO_TIME as TO_TIME,rt.description as DESCRIP " +
                                      " FROM EMPLOYEE_ROSTER_DATE as erd left outer join EMPLOYEE as e on erd.DUTY_COVERED_BY = e.EMPLOYEE_ID " +
                                      " inner join ROSTER r on erd.ROSTR_ID = r.ROSTR_ID inner join ROSTER_TYPES rt on r.ROSTER_TYPE = rt.ROSTER_TYPE " +
                                      " AND erd.EMPLOYEE_ID = '" + empID + "' " +
                                      " AND erd.DUTY_DATE >= '" + fromDate + "' AND erd.DUTY_DATE <= '" + toDate + "'" +
                                      " AND erd.STATUS_CODE <> '" + Constants.STATUS_OBSOLETE_VALUE + "' ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow populateRosterDate(string empID, string fromDate, string STATUS_CODE)
        {
            try
            {
                dataTable.Rows.Clear();
                DataRow dr = null;
                string sMySqlString = " SELECT erd.ROSTR_ID as ROSTR_ID, erd.EMPLOYEE_ID as EMPLOYEE_ID, DATE(erd.DUTY_DATE) as DUTY_DATE, r.FROM_TIME as FROM_TIME, r.TO_TIME as TO_TIME   " +
                                      " FROM EMPLOYEE_ROSTER_DATE erd, ROSTER r, ROSTER_TYPES rt " +
                                      " WHERE r.ROSTR_ID = erd.ROSTR_ID " +
                                      " AND r.ROSTER_TYPE = rt.ROSTER_TYPE " +
                                      " AND DUTY_DATE = '" + fromDate + "'  " +
                                      " AND EMPLOYEE_ID = '" + empID + "' " +
                                      " AND erd.STATUS_CODE = '" + STATUS_CODE + "' ";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                     dr = dataTable.Rows[0];
                }
                else
                {
                     dr = null;
                }
                
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateRostersModifiedToday(string empID)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT erd.ROSTR_ID as ROSTR_ID, erd.EMPLOYEE_ID as EMPLOYEE_ID, DATE(erd.DUTY_DATE) as DUTY_DATE, erd.IS_OFF_DAY as IS_OFF_DAY, erd.IS_SUMMARIZED as IS_SUMMARIZED, erd.INTERCHANGE_NUMBER as INTERCHANGE_NUMBER, erd.DUTY_COVERED_BY as DUTY_COVERED_BY, erd.REASON as REASON, erd.STATUS_CODE as STATUS_CODE, r.FROM_TIME as FROM_TIME, r.TO_TIME as TO_TIME, r.ROSTER_TYPE as ROSTER_TYPE, rt.description as description, emp.FIRST_NAME   " +
                                      " FROM ROSTER r, ROSTER_TYPES rt, EMPLOYEE_ROSTER_DATE erd left outer join EMPLOYEE emp on emp.EMPLOYEE_ID = erd.DUTY_COVERED_BY " +
                                      " WHERE r.ROSTR_ID = erd.ROSTR_ID " +
                                      " AND r.ROSTER_TYPE = rt.ROSTER_TYPE " +
                                      " AND date(erd.MODIFIED_DATE) = curdate() " +
                                      " AND erd.STATUS_CODE <> '"+ Constants.STATUS_OBSOLETE_VALUE +"' " +
                                      " AND erd.EMPLOYEE_ID = '" + empID + "' " +
                                      " ORDER BY DATE(erd.DUTY_DATE)";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool checkForOverlappingRostersRegular(string empID, string dutyDate, string fromTime, string toTime)
        {
            bool isOverlapping  = false;
            int iRecs           = 0;
            int iRecs2          = 0;
            int iRecs3 = 0;

            try
            {

                //string sMySqlString = "SELECT count(*) " +
                //                        " FROM EMPLOYEE_ROSTER_DATE erd, ROSTER r " +
                //                        " where erd.ROSTR_ID = r.ROSTR_ID " +
                //                        " and date(DUTY_DATE) = @dutyDate " +
                //                        " and EMPLOYEE_ID = @empID " +
                //                        " and ( (@fromTime >= r.FROM_TIME and @fromTime < r.TO_TIME)   OR   (@toTime > r.FROM_TIME and @toTime =< r.TO_TIME) ) ";

                // 2015-10-29
                string sMySqlString = "SELECT count(*)" +
                                        "FROM EMPLOYEE_ROSTER_DATE erd, ROSTER r " +
                                        "where erd.ROSTR_ID = r.ROSTR_ID  " +
                                        " and DATE(DUTY_DATE) = DATE('" + dutyDate.Trim() + "') " +
                                        " and EMPLOYEE_ID = '" + empID.Trim() + "' and erd.STATUS_CODE='1' " +
                                        " and ((TIME('" + fromTime.Trim() + "') <= TIME(r.FROM_TIME) and TIME('" + toTime.Trim() + "') <= TIME(r.TO_TIME) and TIME('" + toTime.Trim() + "') > TIME(r.FROM_TIME)) " +
                                        " OR    (TIME('" + fromTime.Trim() + "') >= TIME(r.FROM_TIME) and TIME('" + toTime.Trim() + "') <= TIME(r.TO_TIME) and TIME('" + fromTime.Trim() + "') <  TIME(r.TO_TIME)) " +
                                        " OR 	  (TIME('" + fromTime.Trim() + "') >= TIME(r.FROM_TIME) and TIME('" + fromTime.Trim() + "') < TIME(r.TO_TIME) and TIME('" + toTime.Trim() + "') >= TIME(r.TO_TIME))" +
                                        " OR (TIME('" + fromTime.Trim() + "') >= TIME(r.FROM_TIME) and TIME('" + fromTime.Trim() + "') < TIME(r.TO_TIME) and TIME('" + toTime.Trim() + "') > TIME(r.TO_TIME)))";

               
                
                mySqlCmd.CommandText = sMySqlString;

                
                mySqlCon.Open();

                iRecs = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());

                if (iRecs > 0)
                    isOverlapping = true;

                // check whether it overlaps with previous day overnight roster
                string sMySqlString2 = " SELECT count(*) " +
                                        " FROM EMPLOYEE_ROSTER_DATE erd,  ROSTER r  " +
                                        " where erd.ROSTR_ID = r.ROSTR_ID  and  EMPLOYEE_ID='" + empID.Trim() + "' and  " +
                                        " r.ROSTER_TYPE = '2' and  DATE(DUTY_DATE) =  DATE(DATE_SUB('" + dutyDate.Trim() + "' , INTERVAL 1 DAY)) and erd.STATUS_CODE='1' and  " +
                                        "       (TIME('" + fromTime.Trim() + "') < TIME(r.TO_TIME))";



                mySqlCmd.CommandText = sMySqlString2;



                //mySqlCon.Open();

                iRecs2 = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());

                if (iRecs2 > 0)
                    isOverlapping = true;



                // check whether it overlaps with current day overnight roster
                string sMySqlString3 = " SELECT count(*) " +
                                        " FROM EMPLOYEE_ROSTER_DATE erd,  ROSTER r  " +
                                        " where erd.ROSTR_ID = r.ROSTR_ID  and  EMPLOYEE_ID='" + empID.Trim() + "' and  " +
                                        " r.ROSTER_TYPE = '2' and  DATE(DUTY_DATE) =  DATE('" + dutyDate.Trim() + ") and erd.STATUS_CODE='1' and  " +
                                        "       (TIME('" + toTime.Trim() + "') > TIME(r.FROM_TIME))";



                mySqlCmd.CommandText = sMySqlString2;



                //mySqlCon.Open();

                iRecs3 = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());

                if (iRecs3 > 0)
                    isOverlapping = true;


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


            return isOverlapping;
        }



        //public bool checkForOverlappingRostersOverNightWithOverNight(string empID, string dutyDate, string fromTime, string toTime)
        //{
        //    bool isOverlapping = false;
        //    int iRecs = 0;
        //    string status = Constants.CON_ROSTER_OVER_NIGHT_CODE;

        //    try
        //    {
        //        string sMySqlString = "SELECT count(*) " +
        //                                "FROM EMPLOYEE_ROSTER_DATE erd, ROSTER r  " +
        //                                "where erd.ROSTR_ID = r.ROSTR_ID  " +
        //                                "and r.ROSTER_TYPE = '2' " +
        //                                "and DATE_ADD(DUTY_DATE, INTERVAL 1 DAY) = @dutyDate " +
        //                                "and EMPLOYEE_ID = @empID  and erd.STATUS_CODE='1' " +
        //                                "and ( (@fromTime < r.TO_TIME) ) ";


        //        mySqlCmd.CommandText = sMySqlString;

        //        mySqlCmd.Parameters.Clear();

        //        mySqlCmd.Parameters.Add(new MySqlParameter("@empID", empID.Trim() == "" ? (object)DBNull.Value : empID.Trim()));
        //        mySqlCmd.Parameters.Add(new MySqlParameter("@dutyDate", dutyDate.Trim() == "" ? (object)DBNull.Value : dutyDate.Trim()));
        //        mySqlCmd.Parameters.Add(new MySqlParameter("@fromTime", fromTime.Trim() == "" ? (object)DBNull.Value : fromTime.Trim()));
        //        mySqlCmd.Parameters.Add(new MySqlParameter("@status", status.Trim()));


        //        mySqlCon.Open();

        //        iRecs = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());

        //        if (iRecs > 0)
        //            isOverlapping = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        mySqlCon.Close();
        //        mySqlCmd.Dispose();
        //    }


        //    return isOverlapping;
        //}


        public bool checkForOverlappingRostersOverNight(string empID, string dutyDate, string fromTime, string toTime)
        {
            bool isOverlapping = false;
            int iRecs3 = 0;
            int iRecs1 = 0;
            int iRecs2 = 0;
            int iRecs4 = 0;
            int iRecs5 = 0;
            

            string status = Constants.CON_ROSTER_OVER_NIGHT_CODE;

            try
            {
                //// checked whether over night roster overlaps with next day over night roster
                //string sMySqlString = "SELECT count(*) " +
                //                        "FROM EMPLOYEE_ROSTER_DATE erd, ROSTER r  " +
                //                        "where erd.ROSTR_ID = r.ROSTR_ID  " +
                //                        "and r.ROSTER_TYPE = '2' " +
                //                        "and DATE_ADD(DUTY_DATE, INTERVAL 1 DAY) = @dutyDate " +
                //                        "and EMPLOYEE_ID = @empID  and erd.STATUS_CODE='1' " +
                //                        "and ( ('" + toTime.Trim() + "' < r.FROM_TIME) ) ";

                              


                // checked whether over night roster overlaps with next day roster
                string sMySqlString = "SELECT count(*) " +
                                        "FROM EMPLOYEE_ROSTER_DATE erd, ROSTER r  " +
                                        "where erd.ROSTR_ID = r.ROSTR_ID  " +
                                        "and  DATE(DUTY_DATE) = DATE_ADD(DATE('" + dutyDate.Trim() + "'), INTERVAL 1 DAY) " +
                                        "and EMPLOYEE_ID ='" + empID.Trim() + "' and erd.STATUS_CODE='1' " +
                                        "and ( ('" + toTime.Trim() + "' > r.FROM_TIME) ) ";

                mySqlCmd.CommandText = sMySqlString;               

                mySqlCon.Open();

                iRecs3 = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());

                if (iRecs3 > 0)
                    isOverlapping = true;


                // checked whether over night roster overlaps with previous day over night roster

                string sMySqlString4 = " SELECT count(*) " +
                                        " FROM EMPLOYEE_ROSTER_DATE erd,  ROSTER r  " +
                                        " where erd.ROSTR_ID = r.ROSTR_ID  and  EMPLOYEE_ID='" + empID.Trim() + "' and  " +
                                        " r.ROSTER_TYPE = '2' and  DATE(DUTY_DATE) =  DATE(DATE_SUB('" + dutyDate.Trim() + "' , INTERVAL 1 DAY)) and erd.STATUS_CODE='1' and  " +
                                        "       (TIME('" + fromTime.Trim() + "') < TIME(r.TO_TIME))";                      

                mySqlCmd.CommandText = sMySqlString4;

                iRecs4 = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());

                if (iRecs4 > 0)
                    isOverlapping = true;

                
                // check whether it overlaps with current day regular roster  
                string sMySqlString2 = " SELECT count(*) " +
                                        " FROM EMPLOYEE_ROSTER_DATE erd,  ROSTER r  " +
                                        " where erd.ROSTR_ID = r.ROSTR_ID  and  EMPLOYEE_ID='" + empID.Trim() + "' and  " +
                                        " 	  DATE(DUTY_DATE) = DATE('" + dutyDate.Trim() + "') and erd.STATUS_CODE='1' and  " +
                                        "       (TIME('" + fromTime.Trim() + "') < TIME(r.TO_TIME))";



                mySqlCmd.CommandText = sMySqlString2;



                //mySqlCon.Open();

                iRecs2 = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());

                if (iRecs2 > 0)
                    isOverlapping = true;

                // 
                string sMySqlString1 = " SELECT count(*) " +
                                      " FROM EMPLOYEE_ROSTER_DATE erd,  ROSTER r  " +
                                      " where erd.ROSTR_ID = r.ROSTR_ID and r.ROSTER_TYPE='2'  and  EMPLOYEE_ID='" + empID.Trim() + "' and  " +
                                      " 	  DATE(DUTY_DATE) = DATE('" + dutyDate.Trim() + "') and erd.STATUS_CODE='1' and  " +
                                      "       ((TIME('" + fromTime.Trim() + "') >= TIME(r.FROM_TIME)) OR (TIME('" + fromTime.Trim() + "') <= TIME(r.FROM_TIME)))";



                mySqlCmd.CommandText = sMySqlString1;



                //mySqlCon.Open();

                iRecs1 = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());

                if (iRecs1 > 0)
                    isOverlapping = true;
                
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


            return isOverlapping;
        }


        //public bool checkForOverlappingRostersOverNightWithRegular(string empID, string dutyDate, string fromTime, string toTime)
        //{
        //    bool isOverlapping = false;
        //    int iRecs1 = 0;
        //    int iRecs2 = 0; 
        //    try
        //    {
        //        string sMySqlString =   " SELECT count(*) " +
        //                                " FROM HRIS.EMPLOYEE_ROSTER_DATE erd,  ROSTER r  " +
        //                                " where erd.ROSTR_ID = r.ROSTR_ID  and  EMPLOYEE_ID='" + empID.Trim() + "' and  " +
        //                                " 	  DATE(DUTY_DATE) = DATE('" + dutyDate.Trim() + "') and erd.STATUS_CODE='1' and  " +
        //                                "       (TIME('" + fromTime.Trim() + "') <= TIME(r.TO_TIME))";                    
                    
                    

        //        mySqlCmd.CommandText = sMySqlString;

                

        //        mySqlCon.Open();

        //        iRecs1 = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());

        //        if (iRecs1 > 0)
        //            isOverlapping = true;

        //        string sMySqlString2 = " SELECT count(*) " +
        //                                " FROM HRIS.EMPLOYEE_ROSTER_DATE erd,  ROSTER r  " +
        //                                " where erd.ROSTR_ID = r.ROSTR_ID  and  EMPLOYEE_ID='" + empID.Trim() + "' and  " +
        //                                " 	  DATE(DUTY_DATE) =  DATE_ADD(DATE('" + dutyDate.Trim() + "'), INTERVAL 1 DAY) and erd.STATUS_CODE='1' and  " +
        //                                "       (TIME('" + toTime.Trim() + "') >= TIME(r.FROM_TIME))";

        //        mySqlCmd.CommandText = sMySqlString2;

        //        iRecs2 = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());

        //        if (iRecs2 > 0)
        //            isOverlapping = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        mySqlCon.Close();
        //        mySqlCmd.Dispose();
        //    }


        //    return isOverlapping;
        //}



        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Insert Rosters 
        ///</summary>
        //----------------------------------------------------------------------------------------
        public Boolean Insert(String rosterID,
                              String employeeID,
                              String dutyDate,
                              String isOffDay,
                              String isSummarized,
                              String interchangeNumber,
                              String dutyCoverredBy,
                              String reason,
                              String userID)
        {
            Boolean bInserted = false;
            String statusCode = "";
             
            statusCode = Constants.STATUS_ACTIVE_VALUE;

            //SerialHandler serialHandler = new SerialHandler();

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                //rosterId = serialHandler.getserila(mySqlCon, Constants.ROSTER_ID_STAMP);

                mySqlCmd.Parameters.Add(new MySqlParameter("@rosterID", rosterID.Trim() == "" ? (object)DBNull.Value : rosterID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@employeeID", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@dutyDate", dutyDate.Trim() == "" ? (object)DBNull.Value : dutyDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@isOffDay", isOffDay.Trim() == "" ? (object)DBNull.Value : isOffDay.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@isSummarized", isSummarized.Trim() == "" ? (object)DBNull.Value : isSummarized.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@interchangeNumber", interchangeNumber.Trim() == "" ? (object)DBNull.Value : interchangeNumber.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@dutyCoverredBy", dutyCoverredBy.Trim() == "" ? (object)DBNull.Value : dutyCoverredBy.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@reason", reason.Trim() == "" ? (object)DBNull.Value : reason.Trim()));

                mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));


                sMySqlString = " INSERT INTO EMPLOYEE_ROSTER_DATE  " +
                                " (ROSTR_ID,  " +
                                " EMPLOYEE_ID,  " +
                                " DUTY_DATE,  " +
                                " STATUS_CODE,  " +
                                " IS_OFF_DAY,  " +
                                " IS_SUMMARIZED,  " +
                                " INTERCHANGE_NUMBER,  " +
                                " DUTY_COVERED_BY,  " +
                                " REASON,  " +
                                " ADDED_BY,  " +
                                " ADDED_DATE,  " +
                                " MODIFIED_BY,  " +
                                " MODIFIED_DATE)  " +
                                " VALUES  " +
                                " (@rosterID,  " +
                                " @employeeID,  " +
                                " @dutyDate,  " +
                                " @statusCode,  " +
                                " @isOffDay,  " +
                                " @isSummarized,  " +
                                " @interchangeNumber,  " +
                                " @dutyCoverredBy,  " +
                                " @reason,  " +
                                " @userID,  " +
                                " now() ,  " +
                                " @userID,  " +
                                " now() )  ";



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

                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                //serialHandler = null;
            }

            return bInserted;
        }

        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Insert Rosters 
        ///anjana 11/03/2015
        ///</summary>
        //----------------------------------------------------------------------------------------
        public DataTable InsertMultipleRoster(String employeeID,
                                            DataTable dtShedule,
                                            String isOffDay,
                                            String isSummarized,
                                            String interchangeNumber,
                                            String dutyCoverredBy,
                                            String reason,
                                            String userID)
        {
            Boolean bInserted = false;
            String statusCode = "";

            DataTable rosterTemp = new DataTable();
            rosterTemp.Columns.Add("ROSTR_ID", typeof(string));
            rosterTemp.Columns.Add("ROSTR_TIME", typeof(string));
            rosterTemp.Columns.Add("DUTY_DATE", typeof(string));
            rosterTemp.PrimaryKey = new[] { rosterTemp.Columns["ROSTR_ID"], rosterTemp.Columns["ROSTR_TIME"], rosterTemp.Columns["DUTY_DATE"] };

            statusCode = Constants.STATUS_ACTIVE_VALUE;

            //SerialHandler serialHandler = new SerialHandler();

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                //rosterId = serialHandler.getserila(mySqlCon, Constants.ROSTER_ID_STAMP);
                foreach (DataRow dr in dtShedule.Rows)
                {
                    mySqlCmd.Parameters.Clear();

                    if (dr["IS_EXCLUDE"].ToString().Trim() == Constants.CON_ROSTER_EXCLUDE_NO.ToString().Trim())
                    {

                        mySqlCmd.Parameters.Add(new MySqlParameter("@rosterID", dr["ROSTR_ID"].ToString().Trim() == "" ? (object)DBNull.Value : dr["ROSTR_ID"].ToString().Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@employeeID", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@dutyDate", dr["DUTY_DATE"].ToString().Trim() == "" ? (object)DBNull.Value : dr["DUTY_DATE"].ToString().Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@isOffDay", isOffDay.Trim() == "" ? (object)DBNull.Value : isOffDay.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@isSummarized", isSummarized.Trim() == "" ? (object)DBNull.Value : isSummarized.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@interchangeNumber", interchangeNumber.Trim() == "" ? (object)DBNull.Value : interchangeNumber.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@dutyCoverredBy", dutyCoverredBy.Trim() == "" ? (object)DBNull.Value : dutyCoverredBy.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@reason", reason.Trim() == "" ? (object)DBNull.Value : reason.Trim()));

                        mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));


                        sMySqlString = " INSERT INTO EMPLOYEE_ROSTER_DATE  " +
                                        " (ROSTR_ID,  " +
                                        " EMPLOYEE_ID,  " +
                                        " DUTY_DATE,  " +
                                        " STATUS_CODE,  " +
                                        " IS_OFF_DAY,  " +
                                        " IS_SUMMARIZED,  " +
                                        " INTERCHANGE_NUMBER,  " +
                                        " DUTY_COVERED_BY,  " +
                                        " REASON,  " +
                                        " ADDED_BY,  " +
                                        " ADDED_DATE,  " +
                                        " MODIFIED_BY,  " +
                                        " MODIFIED_DATE)  " +
                                        " VALUES  " +
                                        " (@rosterID,  " +
                                        " @employeeID,  " +
                                        " @dutyDate,  " +
                                        " @statusCode,  " +
                                        " @isOffDay,  " +
                                        " @isSummarized,  " +
                                        " @interchangeNumber,  " +
                                        " @dutyCoverredBy,  " +
                                        " @reason,  " +
                                        " @userID,  " +
                                        " now() ,  " +
                                        " @userID,  " +
                                        " now() )  ";



                        mySqlCmd.Transaction = mySqlTrans;
                        mySqlCmd.CommandText = sMySqlString;

                        mySqlCmd.ExecuteNonQuery();

                        DataRow datarow = rosterTemp.NewRow();

                        datarow["ROSTR_ID"] = dr["ROSTR_ID"].ToString().Trim();
                        datarow["ROSTR_TIME"] = dr["ROSTR_TIME"].ToString().Trim(); 
                        datarow["DUTY_DATE"] = dr["DUTY_DATE"].ToString().Trim();

                        rosterTemp.Rows.Add(datarow);
                    }
                }

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

                //serialHandler = null;
            }

            return rosterTemp;
        }




        public Boolean Update(String rosterID,
                              String employeeID,
                              String dutyDate,
                              String isOffDay,
                              String isSummarized,
                              String interchangeNumber,
                              String dutyCoverredBy,
                              String reason,
                              String userID,
                              String statusCode)
        {
            Boolean bUpdated = false;


            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@rosterId", rosterID.Trim() == "" ? (object)DBNull.Value : rosterID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeID", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@dutyDate", dutyDate.Trim() == "" ? (object)DBNull.Value : dutyDate.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@isOffDay", isOffDay.Trim() == "" ? (object)DBNull.Value : isOffDay.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@isSummarized", isSummarized.Trim() == "" ? (object)DBNull.Value : isSummarized.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@interchangeNumber", interchangeNumber.Trim() == "" ? (object)DBNull.Value : interchangeNumber.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@dutyCoverredBy", dutyCoverredBy.Trim() == "" ? (object)DBNull.Value : dutyCoverredBy.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@reason", reason.Trim() == "" ? (object)DBNull.Value : reason.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                sMySqlString = " UPDATE EMPLOYEE_ROSTER_DATE " +
                                " SET " +
                                " DUTY_DATE = @dutyDate, " +
                                " STATUS_CODE = @statusCode, " +
                                " IS_OFF_DAY = @isOffDay, " +
                                " IS_SUMMARIZED = @isSummarized, " +
                                " INTERCHANGE_NUMBER = @interchangeNumber, " +
                                " DUTY_COVERED_BY = @dutyCoverredBy, " +
                                " REASON = @reason, " +
                                " ADDED_BY = @userID, " +
                                " ADDED_DATE = now() , " +
                                " MODIFIED_BY = @userID, " +
                                " MODIFIED_DATE = now()  " +
                                " WHERE ROSTR_ID = @rosterId AND EMPLOYEE_ID = @employeeID AND DUTY_DATE = @dutyDate ";

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




        public Boolean obsolete(String rosterID,
                                  String employeeID,
                                  String dutyDate,
                                  String isSummarized,
                                  String userID,
                                  String existingStatus)
        {
            Boolean bUpdated = false;
            string statusCode = Constants.STATUS_OBSOLETE_VALUE;


            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Add(new MySqlParameter("@rosterId", rosterID.Trim() == "" ? (object)DBNull.Value : rosterID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeID", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@dutyDate", dutyDate.Trim() == "" ? (object)DBNull.Value : dutyDate.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@isSummarized", isSummarized.Trim() == "" ? (object)DBNull.Value : isSummarized.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@existingStatus", existingStatus.Trim() == "" ? (object)DBNull.Value : existingStatus.Trim()));


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                sMySqlString = " UPDATE EMPLOYEE_ROSTER_DATE " +
                                " SET " +
                                " STATUS_CODE = @statusCode, " +
                                " IS_SUMMARIZED = @isSummarized, " +
                                " MODIFIED_BY = @userID, " +
                                " MODIFIED_DATE = now()  " +
                                " WHERE ROSTR_ID = @rosterId AND EMPLOYEE_ID = @employeeID AND DUTY_DATE = @dutyDate and STATUS_CODE = @existingStatus ";

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

        public Boolean isObsolete(DataTable obsolete, String empId, String isSummarized, string userId)
        {
            bool status = false;
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                
                foreach (DataRow dr in obsolete.Rows)
                {
                    string rosterId = dr["ROSTR_ID"].ToString();
                    string dutyDate = dr["DUTY_DATE"].ToString();
                    string interchangeNo = dr["INTERCHANGE_NUMBER"].ToString();
                    string existingStatus = dr["STATUS_CODE"].ToString();
                    string statusCode = Constants.STATUS_OBSOLETE_VALUE;

                    mySqlCmd.Parameters.Clear();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@rosterId", rosterId.Trim() == "" ? (object)DBNull.Value : rosterId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@dutyDate", dutyDate.Trim() == "" ? (object)DBNull.Value : dutyDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@interchangeNo", interchangeNo.Trim() == "" ? (object)DBNull.Value : interchangeNo.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@isSummarized", isSummarized.Trim() == "" ? (object)DBNull.Value : isSummarized.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userId.Trim() == "" ? (object)DBNull.Value : userId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@employeeID", empId.Trim() == "" ? (object)DBNull.Value : empId.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@existingStatus", existingStatus.Trim() == "" ? (object)DBNull.Value : existingStatus.Trim()));

                    string Qry =" UPDATE EMPLOYEE_ROSTER_DATE " +
                                " SET " +
                                " STATUS_CODE = @statusCode, " +
                                " IS_SUMMARIZED = @isSummarized, " +
                                " MODIFIED_BY = @userID, " +
                                " MODIFIED_DATE = now()  " +
                                " WHERE ROSTR_ID = @rosterId AND EMPLOYEE_ID = @employeeID AND DUTY_DATE = @dutyDate and STATUS_CODE = @existingStatus ";

                    mySqlCmd.Transaction = mySqlTrans;
                    mySqlCmd.CommandText = Qry;

                    mySqlCmd.ExecuteNonQuery();



                }

                mySqlTrans.Commit();
                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                status = true;
            }
           catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }
            return status;
        }




        public Boolean obsoleteInterchange(String interchangeNumber,
                                              String employeeID,
                                              String isSummarized,
                                              String userID)
        {
            Boolean bUpdated = false;

            string statusCode       = Constants.STATUS_OBSOLETE_VALUE;
            string activeStatusCode = Constants.STATUS_ACTIVE_VALUE;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            mySqlCmd.Parameters.Clear();

            mySqlCmd.Parameters.Add(new MySqlParameter("@interchangeNumber", interchangeNumber.Trim() == "" ? (object)DBNull.Value : interchangeNumber.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeID", employeeID.Trim() == "" ? (object)DBNull.Value : employeeID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@isSummarized", isSummarized.Trim() == "" ? (object)DBNull.Value : isSummarized.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@userID", userID.Trim() == "" ? (object)DBNull.Value : userID.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
            mySqlCmd.Parameters.Add(new MySqlParameter("@activeStatusCode", activeStatusCode.Trim() == "" ? (object)DBNull.Value : activeStatusCode.Trim()));


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                sMySqlString = " UPDATE EMPLOYEE_ROSTER_DATE " +
                                " SET " +
                                " STATUS_CODE = @statusCode, " +
                                " IS_SUMMARIZED = @isSummarized, " +
                                " MODIFIED_BY = @userID, " +
                                " MODIFIED_DATE = now()  " +
                                " WHERE INTERCHANGE_NUMBER = @interchangeNumber AND EMPLOYEE_ID = @employeeID AND STATUS_CODE = @activeStatusCode ";

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



    }
}
