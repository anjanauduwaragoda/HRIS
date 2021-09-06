using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Common;


namespace DataHandler.EmployeeLeave
{
    public class LeaveScheduleDataHandler : TemplateDataHandler
    {

        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 14-07-2014
        // this function is written to used at LeaveConstraints.cs
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return number of months, a given employee has serviced
        ///<param name="employeeId">Pass a employeeid string to query </param>
        ///</summary>        
        //----------------------------------------------------------------------------------------


        public decimal getServiceMonthsofEmployee(string employeeId)
        {

            decimal dServiceMonths = 0;

            mySqlCmd.CommandText = "SELECT TIMESTAMPDIFF(MONTH,DOJ,now()) as Service_Months FROM EMPLOYEE where EMPLOYEE_ID='" + employeeId.Trim() + "'";

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    dServiceMonths = Decimal.Parse(mySqlCmd.ExecuteScalar().ToString());
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return dServiceMonths;
        }

        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 22-07-2014
        // this function is written to used at LeaveConstraints.cs
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return number of days, a given employee has serviced
        ///<param name="employeeId">Pass a employeeid string to query </param>
        ///</summary>        
        //----------------------------------------------------------------------------------------

        public decimal getServiceDaysOfEmployee(string employeeId)
        {
            decimal dServiceDays = 0;

            mySqlCmd.CommandText = "SELECT TIMESTAMPDIFF(DAY,DOJ,now()) as Service_Days FROM EMPLOYEE where EMPLOYEE_ID = '" + employeeId.Trim() + "'";

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    dServiceDays = Decimal.Parse(mySqlCmd.ExecuteScalar().ToString());
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return dServiceDays;


        }


        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 14-07-2014
        // this function is written to used at LeaveConstraints.cs
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return joind month of a given employee 
        ///<param name="employeeId">Pass a employeeid string to query </param>
        ///</summary>        
        //----------------------------------------------------------------------------------------


        public int getJoindMonthofEmployee(string employeeId)
        {

            int dJoinedMonths = 0;

            mySqlCmd.CommandText = "SELECT MONTH(DOJ) as joindMonth FROM EMPLOYEE where EMPLOYEE_ID='" + employeeId.Trim() + "'";

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    dJoinedMonths = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return dJoinedMonths;
        }

        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 14-07-2014
        // this function is written to used at LeaveConstraints.cs
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return joind year of a given employee 
        ///<param name="employeeId">Pass a employeeid string to query </param>
        ///</summary>        
        //----------------------------------------------------------------------------------------


        public int getJoindYearofEmployee(string employeeId)
        {

            int JoinedYear = 0;

            mySqlCmd.CommandText = "SELECT YEAR(DOJ) as joindYear FROM EMPLOYEE where EMPLOYEE_ID ='" + employeeId.Trim() + "'";

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    JoinedYear = Int32.Parse(mySqlCmd.ExecuteScalar().ToString());
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return JoinedYear;
        }

        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 17-07-2014
        // this function is written to used at webFrmEmploeeLeaveSchedule.aspx
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return leave summary for a given employee and year
        ///<param name="employeeId">Pass a employeeid string to query </param>
        ///<param name="sYear">Pass a Year string to query </param>
        ///</summary>        
        //----------------------------------------------------------------------------------------

        public DataTable getEmployeeLeveSummary(string employeeId, string sYear)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString =   " SELECT lt.LEAVE_TYPE_ID,lt.LEAVE_TYPE_NAME,lsi.NUMBER_OF_DAYS,loj.leaves_taken,(lsi.NUMBER_OF_DAYS - loj.leaves_taken) as Leves_Remain" +
                                        " FROM EMPLOYEE_LEAVE_SCHEME as els inner join LEAVE_SCHEME_ITEM as lsi on els.LEAVE_SCHEME_ID = lsi.LEAVE_SCHEME_ID " +
                                        " 	 inner join LEAVE_TYPE as lt on lsi.LEAVE_TYPE_ID = lt.LEAVE_TYPE_ID " +
                                        " 	 left outer join ( " +
                                        " 	 select lty.LEAVE_TYPE_NAME,sum(elsh.NO_OF_DAYS) as leaves_taken " +
                                        " 	 from EMPLOYEE_LEAVE_SCHEDULE as elsh inner join LEAVE_TYPE as lty on elsh.LEAVE_TYPE_ID=lty.LEAVE_TYPE_ID " +
                                        " 	 where elsh.LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_REJECTED + "' AND elsh.LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_DISCARDED + "'" +
                                        "    AND CAST(YEAR(elsh.LEAVE_DATE) as CHAR) ='" + sYear.Trim() + "'" +
                                        "    and elsh.EMPLOYEE_ID ='" + employeeId.Trim() + "' " +
                                        " 	 group by lty.LEAVE_TYPE_NAME ) as loj on loj.LEAVE_TYPE_NAME = lt.LEAVE_TYPE_NAME " +
                                        " WHERE els.STATUS_CODE ='1' and els.EMPLOYEE_ID ='" + employeeId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        public DataTable getRequestedLeaves(string sLeaveSheetId)
        {

            try
            {
                dataTable.Clear();

                string sMySqlString = " SELECT LEAVE_TYPE_ID,convert(sum(NO_OF_DAYS),char) NO_OF_DAYS from EMPLOYEE_LEAVE_SCHEDULE " +
                                      " where LEAVE_SHEET_ID='" + sLeaveSheetId.Trim() + "'" +
                                      " group by LEAVE_TYPE_ID order by LEAVE_TYPE_ID "; 
                
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public string getRequestedShortLeaves(string sLeaveSheetId)
        {

            string sNumberOfLeave = "";

            mySqlCmd.CommandText =  " SELECT convert(count(NO_OF_DAYS),char) NO_OF_DAYS from EMPLOYEE_LEAVE_SCHEDULE" +
                                    " where LEAVE_SHEET_ID='" + sLeaveSheetId.Trim() + "' and LEAVE_TYPE_ID ='" + Constants.CON_SHORT_LEAVE_LEAVE_ID + "'" +
                                    " group by LEAVE_TYPE_ID order by LEAVE_TYPE_ID";
            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    sNumberOfLeave = mySqlCmd.ExecuteScalar().ToString();
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return sNumberOfLeave;
        }

        public string getConsumedShortLeaves(string sYear, string sMonth,string sEmployee)
        {
            string sNumberOfLeave = "";

            mySqlCmd.CommandText =  " SELECT convert(count(NO_OF_DAYS),char) NO_OF_DAYS from EMPLOYEE_LEAVE_SCHEDULE " +
                                    " where EMPLOYEE_ID ='" + sEmployee.Trim() + "' and LEAVE_TYPE_ID='" + Constants.CON_SHORT_LEAVE_LEAVE_ID + "' and " +
                                    " CAST(YEAR(LEAVE_DATE) as CHAR) = '" + sYear.Trim() + "' and CAST(MONTH(LEAVE_DATE) as CHAR) ='" + sMonth.Trim() +
                                    "' and LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_REJECTED + "' and LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_DISCARDED + "'";
            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    sNumberOfLeave = mySqlCmd.ExecuteScalar().ToString();
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }
            return sNumberOfLeave;
        }

        public DataTable getEmployeeLeveSummaryChart(string employeeId, string sYear)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString =   " SELECT lt.LEAVE_TYPE_NAME, IFNULL(lsi.NUMBER_OF_DAYS,0) AS NUMBER_OF_DAYS, IFNULL(loj.leaves_taken,0) AS LEAVE_TAKEN ,IFNULL((lsi.NUMBER_OF_DAYS - loj.leaves_taken),0) as Leves_Remain" +
                                        " FROM EMPLOYEE_LEAVE_SCHEME as els inner join LEAVE_SCHEME_ITEM as lsi on els.LEAVE_SCHEME_ID = lsi.LEAVE_SCHEME_ID " +
                                        " 	 inner join LEAVE_TYPE as lt on lsi.LEAVE_TYPE_ID = lt.LEAVE_TYPE_ID " +
                                        " 	 left outer join ( " +
                                        " 	 select lty.LEAVE_TYPE_NAME,IFNULL(sum(elsh.NO_OF_DAYS),0) as leaves_taken " +
                                        " 	 from EMPLOYEE_LEAVE_SCHEDULE as elsh inner join LEAVE_TYPE as lty on elsh.LEAVE_TYPE_ID=lty.LEAVE_TYPE_ID " +
                                        " 	 where CAST(YEAR(elsh.LEAVE_DATE) as CHAR) ='" + sYear.Trim() + "' and elsh.EMPLOYEE_ID ='" + employeeId.Trim() + "' " +
                                        " 	 group by lty.LEAVE_TYPE_NAME ) as loj on loj.LEAVE_TYPE_NAME = lt.LEAVE_TYPE_NAME " +
                                        " WHERE els.STATUS_CODE ='1' and els.EMPLOYEE_ID ='" + employeeId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Pass employeeId sYear to get leave balance 
        //----------------------------------------------------------------------------------------

        public DataTable getEmployeeLeveBalance(string employeeId, string sYear)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString =   " SELECT lt.LEAVE_TYPE_NAME as leavetype,(IFNULL(lsi.NUMBER_OF_DAYS,0) - IFNULL(loj.leaves_taken,0)) as leavebalance" +
                                        " FROM EMPLOYEE_LEAVE_SCHEME as els inner join LEAVE_SCHEME_ITEM as lsi on els.LEAVE_SCHEME_ID = lsi.LEAVE_SCHEME_ID " +
                                        " 	 inner join LEAVE_TYPE as lt on lsi.LEAVE_TYPE_ID = lt.LEAVE_TYPE_ID " +
                                        " 	 left outer join ( " +
                                        " 	 select lty.LEAVE_TYPE_NAME,IFNULL(sum(elsh.NO_OF_DAYS),0) as leaves_taken " +
                                        " 	 from EMPLOYEE_LEAVE_SCHEDULE as elsh inner join LEAVE_TYPE as lty on elsh.LEAVE_TYPE_ID=lty.LEAVE_TYPE_ID " +
                                        " 	 where CAST(YEAR(elsh.LEAVE_DATE) as CHAR) ='" + sYear.Trim() + "' and elsh.EMPLOYEE_ID ='" + employeeId.Trim() + "' " +
                                        " 	 group by lty.LEAVE_TYPE_NAME ) as loj on loj.LEAVE_TYPE_NAME = lt.LEAVE_TYPE_NAME " +
                                        " WHERE els.STATUS_CODE ='1' and els.EMPLOYEE_ID ='" + employeeId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Decimal getLeavesTaken(string employeeId, string leaveYear, string leaveTypeId)
        {
            decimal leaveTaken = 0;

            mySqlCmd.CommandText = " SELECT loj.leaves_taken " +
                                    " FROM EMPLOYEE_LEAVE_SCHEME as els inner join LEAVE_SCHEME_ITEM as lsi on els.LEAVE_SCHEME_ID = lsi.LEAVE_SCHEME_ID " +
                                    " 	 inner join LEAVE_TYPE as lt on lsi.LEAVE_TYPE_ID = lt.LEAVE_TYPE_ID " +
                                    " 	 right outer join ( " +
                                    " 	 select lty.LEAVE_TYPE_NAME,sum(elsh.NO_OF_DAYS) as leaves_taken " +
                                    " 	 from EMPLOYEE_LEAVE_SCHEDULE as elsh inner join LEAVE_TYPE as lty on elsh.LEAVE_TYPE_ID=lty.LEAVE_TYPE_ID " +
                                    " 	 where CAST(YEAR(elsh.LEAVE_DATE) as CHAR) ='" + leaveYear.Trim() + "' and elsh.EMPLOYEE_ID ='" + employeeId.Trim() + "' " +
                                    " 	 group by lty.LEAVE_TYPE_NAME ) as loj on loj.LEAVE_TYPE_NAME = lt.LEAVE_TYPE_NAME " +
                                    " WHERE els.STATUS_CODE ='1' and els.EMPLOYEE_ID ='" + employeeId.Trim() + "' and lsi.LEAVE_TYPE_ID='" + leaveTypeId.Trim() + "'";

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    leaveTaken = Decimal.Parse(mySqlCmd.ExecuteScalar().ToString());
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return leaveTaken;

        }

        //----------------------------------------------------------------------------------------
        ///<summary>
        ///An Employees leave schedule is inserted to the database
        ///</summary>
        ///<param name="dtLeaveBucket">Pass a dtLeaveBucket datatable </param> 
        //----------------------------------------------------------------------------------------
        public string Insert(DataTable dtLeaveBucket,
                              string userId,
                              string employeeId,
                              string lslNo,
                              string fromDate,
                              string toDate,
                              string coveredBy,
                              double noOfDays,
                              string recommendBy,
                              string leaveStatus,
                              string remarks)
        {
            SerialHandler serialHandler = new SerialHandler();
            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;
            string leaveSheetId = "";

            try
            {
                mySqlCon.Open();

                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@userId", userId.Trim() == "" ? (object)DBNull.Value : userId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@lslNo", lslNo.Trim() == "" ? (object)DBNull.Value : lslNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@fromDate", fromDate.Trim() == "" ? (object)DBNull.Value : fromDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@toDate", toDate.Trim() == "" ? (object)DBNull.Value : toDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@coveredBy", coveredBy.Trim() == "" ? (object)DBNull.Value : coveredBy.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@noOfDays", noOfDays == 0 ? (object)DBNull.Value : noOfDays));
                mySqlCmd.Parameters.Add(new MySqlParameter("@recommendBy", recommendBy.Trim() == "" ? (object)DBNull.Value : recommendBy.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@leaveStatus", leaveStatus.Trim() == "" ? (object)DBNull.Value : leaveStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));


                
                //string checkExistance = "SELECT count(*) FROM LEAVE_SHEET where EMPLOYEE_ID=@employeeId and FROM_DATE=@fromDate and TO_DATE=@toDate and LEAVE_STATUS <>'0' and LEAVE_STATUS <>'9'";
                
                string commText = "";

                Boolean isDuplicateSheet = false;
                
                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                foreach (DataRow drr in dtLeaveBucket.Rows)
                {
                    if (drr["IS_DAY_OFF"].ToString().Trim() == Constants.LEAVE_IS_OFF_DAY_NO)
                    {

                        string checkExistance = "SELECT count(*) FROM EMPLOYEE_LEAVE_SCHEDULE " +
                                                " where EMPLOYEE_ID='" + employeeId.Trim() + "' and " +
                                                " LEAVE_DATE='" + drr["LEAVE_DATE"].ToString().Trim() + "' and " + 
                                                " TIME(FROM_TIME)=TIME('" + drr["FROM_TIME"].ToString().Trim() + "') and " + 
                                                " TIME(TO_TIME) =TIME('" + drr["TO_TIME"].ToString().Trim() + "') and " + 
                                                " LEAVE_STATUS <>'0' and LEAVE_STATUS <>'9'";
                        
                        mySqlCmd.CommandText = checkExistance;

                        int recCount = 0;

                        if (mySqlCmd.ExecuteScalar() != null)
                        {
                            recCount = int.Parse(mySqlCmd.ExecuteScalar().ToString());
                        }

                        if (recCount > 0)
                        {
                            isDuplicateSheet = true;
                        }
                    }
                }
                if (isDuplicateSheet == false)
                {
                    leaveSheetId = serialHandler.getserila(mySqlCon, Constants.LEAVE_SHEET_ID_STAMP);

                    mySqlCmd.Parameters.Add(new MySqlParameter("@leaveSheetId", leaveSheetId.Trim() == "" ? (object)DBNull.Value : leaveSheetId.Trim()));


                    commText =  " INSERT INTO LEAVE_SHEET(LEAVE_SHEET_ID,EMPLOYEE_ID,FROM_DATE,TO_DATE,COVERED_BY," +
                                " NO_OF_DAYS,ADDED_BY,ADDED_DATE,MODIFIED_BY,MODIFIED_DATE,LEAVE_SCHEME_LINE_NO,REMARKS, " +
                                " RECOMMEND_BY,LEAVE_STATUS) VALUES(@leaveSheetId,@employeeId,@fromDate,@toDate,@coveredBy," +
                                " @noOfDays,@userId,now(),@userId,now(),@lslNo,@remarks,@recommendBy,@leaveStatus)";

                    mySqlCmd.CommandText = commText;

                    mySqlCmd.ExecuteNonQuery();


                    foreach (DataRow dr in dtLeaveBucket.Rows)
                    {
                        if (dr["IS_DAY_OFF"].ToString().Trim() == Constants.LEAVE_IS_OFF_DAY_NO)
                        {
                            mySqlCmd.Parameters.Clear();
                            mySqlCmd.Parameters.Add(new MySqlParameter("@leaveSheetId", leaveSheetId.Trim() == "" ? (object)DBNull.Value : leaveSheetId.Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", dr["EMPLOYEE_ID"].ToString().Trim() == "" ? (object)DBNull.Value : dr["EMPLOYEE_ID"].ToString().Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@leaveDate", dr["LEAVE_DATE"].ToString().Trim() == "" ? (object)DBNull.Value : dr["LEAVE_DATE"].ToString().Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@leave_type_id", dr["LEAVE_TYPE"].ToString().Trim() == "" ? (object)DBNull.Value : dr["LEAVE_TYPE"].ToString().Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@fromTime", dr["FROM_TIME"].ToString().Trim() == "" ? (object)DBNull.Value : dr["FROM_TIME"].ToString().Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@toTime", dr["TO_TIME"].ToString().Trim() == "" ? (object)DBNull.Value : dr["TO_TIME"].ToString().Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@coveredBy", dr["COVERED_BY"].ToString().Trim() == "" ? (object)DBNull.Value : dr["COVERED_BY"].ToString().Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@recommandBy", dr["RECOMMEND_BY"].ToString().Trim() == "" ? (object)DBNull.Value : dr["RECOMMEND_BY"].ToString().Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@scheme_line_no", dr["SCHEME_LINE_NO"].ToString().Trim() == "" ? (object)DBNull.Value : dr["SCHEME_LINE_NO"].ToString().Trim()));

                            mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", dr["REMARKS"].ToString().Trim() == "" ? (object)DBNull.Value : dr["REMARKS"].ToString().Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@noOfDays", dr["NO_OF_DAYS"].ToString().Trim() == "" ? (object)DBNull.Value : dr["NO_OF_DAYS"].ToString().Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@isHalfDay", dr["IS_HALFDAY"].ToString().Trim() == "" ? (object)DBNull.Value : dr["IS_HALFDAY"].ToString().Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@leaveStatus", dr["LEAVE_STATUS"].ToString().Trim() == "" ? (object)DBNull.Value : dr["LEAVE_STATUS"].ToString().Trim()));
                            mySqlCmd.Parameters.Add(new MySqlParameter("@rosterId", dr["ROSTR_ID"].ToString().Trim() == "" ? (object)DBNull.Value : dr["ROSTR_ID"].ToString().Trim()));

                            mySqlCmd.Parameters.Add(new MySqlParameter("@userId", userId.Trim() == "" ? (object)DBNull.Value : userId.Trim()));

                            sMySqlString = " INSERT INTO EMPLOYEE_LEAVE_SCHEDULE(LEAVE_SHEET_ID,LEAVE_DATE,EMPLOYEE_ID,LEAVE_TYPE_ID,SCHEME_LINE_NO,FROM_TIME,TO_TIME,COVERED_BY," +
                                            "REMARKS,NO_OF_DAYS,IS_HALFDAY,LEAVE_STATUS,ADDED_DATE,ADDED_BY,MODIFIED_DATE,MODIFIED_BY," +
                                            "RECOMMAND_BY,ROSTR_ID) VALUES(@leaveSheetId,@leaveDate,@employeeId,@leave_type_id,@scheme_line_no,@fromTime,@toTime,@coveredBy," +
                                            "@remarks,@noOfDays,@isHalfDay,@leaveStatus,now(),@userId,now(),@userId,@recommandBy,@rosterId)";


                            mySqlCmd.CommandText = sMySqlString;

                            mySqlCmd.ExecuteNonQuery();
                        }

                    }
                }

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

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
            return leaveSheetId;
        }


        //----------------------------------------------------------------------------------------
        ///<summary>
        ///An Employees leave schedule is discarded
        ///</summary>
        ///<param name="leaveSheetId">Pass a leave sheet </param> 
        ///<param name="statusCode">Pass a status </param> 
        //----------------------------------------------------------------------------------------

        public Boolean updateStatus(string leaveSheetId, string statusCode, string userId)
        {
            Boolean blUpdated = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;


            try
            {
                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@leaveStatus", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@leaveSheetId", leaveSheetId.Trim() == "" ? (object)DBNull.Value : leaveSheetId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@userId", userId.Trim() == "" ? (object)DBNull.Value : userId.Trim()));


                if (statusCode.Trim().Equals(Constants.LEAVE_STATUS_DISCARDED))
                {
                    sMySqlString = " UPDATE EMPLOYEE_LEAVE_SCHEDULE SET LEAVE_STATUS=@leaveStatus,MODIFIED_DATE=now(),MODIFIED_BY=@userId,IS_SUMMARIZED='" + Constants.CON_IS_SUMMARIZED_NO + "'" +
                                   " WHERE LEAVE_SHEET_ID=@leaveSheetId";
                }
                else
                {
                    sMySqlString = " UPDATE EMPLOYEE_LEAVE_SCHEDULE SET LEAVE_STATUS=@leaveStatus,MODIFIED_DATE=now(),MODIFIED_BY=@userId " +
                                   " WHERE LEAVE_SHEET_ID=@leaveSheetId";
                }

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();


                sMySqlString = String.Empty;

                sMySqlString =  " UPDATE LEAVE_SHEET SET LEAVE_STATUS=@leaveStatus,MODIFIED_BY=@userId,MODIFIED_DATE=now()" +
                                " WHERE LEAVE_SHEET_ID=@leaveSheetId";

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


        public Boolean update(DataTable dtLeaveBucket,
                              string userId,
                              string employeeId,
                              string lslNo,
                              string fromDate,
                              string toDate,
                              string coveredBy,
                              double noOfDays,
                              string recommendBy,
                              string leaveStatus,
                              string remarks,
                              string leaveSheetId)
        {
            Boolean blUpdated = false;
            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;


            try
            {
                mySqlCon.Open();

                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@userId", userId.Trim() == "" ? (object)DBNull.Value : userId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@lslNo", lslNo.Trim() == "" ? (object)DBNull.Value : lslNo.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@fromDate", fromDate.Trim() == "" ? (object)DBNull.Value : fromDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@toDate", toDate.Trim() == "" ? (object)DBNull.Value : toDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@coveredBy", coveredBy.Trim() == "" ? (object)DBNull.Value : coveredBy.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@noOfDays", noOfDays == 0 ? (object)DBNull.Value : noOfDays));
                mySqlCmd.Parameters.Add(new MySqlParameter("@recommendBy", recommendBy.Trim() == "" ? (object)DBNull.Value : recommendBy.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@leaveStatus", leaveStatus.Trim() == "" ? (object)DBNull.Value : leaveStatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", remarks.Trim() == "" ? (object)DBNull.Value : remarks.Trim()));


                mySqlCmd.Parameters.Add(new MySqlParameter("@leaveSheetId", leaveSheetId.Trim() == "" ? (object)DBNull.Value : leaveSheetId.Trim()));


                sMySqlString = "";

                sMySqlString = " UPDATE LEAVE_SHEET SET FROM_DATE =@fromDate,TO_DATE=@toDate,COVERED_BY=@coveredBy," +
                               " NO_OF_DAYS=@noOfDays,MODIFIED_BY=@userId,MODIFIED_DATE=now(),REMARKS=@remarks, " +
                               " RECOMMEND_BY=@recommendBy WHERE LEAVE_SHEET_ID=@leaveSheetId";

                mySqlTrans = mySqlCon.BeginTransaction();
                mySqlCmd.Transaction = mySqlTrans;

                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                sMySqlString = "";

                sMySqlString = "DELETE FROM EMPLOYEE_LEAVE_SCHEDULE WHERE LEAVE_SHEET_ID =@leaveSheetId";

                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();


                foreach (DataRow dr in dtLeaveBucket.Rows)
                {
                    if (dr["IS_DAY_OFF"].ToString().Trim() == Constants.LEAVE_IS_OFF_DAY_NO)
                    {
                        mySqlCmd.Parameters.Clear();
                        mySqlCmd.Parameters.Add(new MySqlParameter("@leaveSheetId", leaveSheetId.Trim() == "" ? (object)DBNull.Value : leaveSheetId.Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", dr["EMPLOYEE_ID"].ToString().Trim() == "" ? (object)DBNull.Value : dr["EMPLOYEE_ID"].ToString().Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@leaveDate", dr["LEAVE_DATE"].ToString().Trim() == "" ? (object)DBNull.Value : dr["LEAVE_DATE"].ToString().Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@leave_type_id", dr["LEAVE_TYPE"].ToString().Trim() == "" ? (object)DBNull.Value : dr["LEAVE_TYPE"].ToString().Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@fromTime", dr["FROM_TIME"].ToString().Trim() == "" ? (object)DBNull.Value : dr["FROM_TIME"].ToString().Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@toTime", dr["TO_TIME"].ToString().Trim() == "" ? (object)DBNull.Value : dr["TO_TIME"].ToString().Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@coveredBy", dr["COVERED_BY"].ToString().Trim() == "" ? (object)DBNull.Value : dr["COVERED_BY"].ToString().Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@recommandBy", dr["RECOMMEND_BY"].ToString().Trim() == "" ? (object)DBNull.Value : dr["RECOMMEND_BY"].ToString().Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@scheme_line_no", dr["SCHEME_LINE_NO"].ToString().Trim() == "" ? (object)DBNull.Value : dr["SCHEME_LINE_NO"].ToString().Trim()));

                        mySqlCmd.Parameters.Add(new MySqlParameter("@remarks", dr["REMARKS"].ToString().Trim() == "" ? (object)DBNull.Value : dr["REMARKS"].ToString().Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@noOfDays", dr["NO_OF_DAYS"].ToString().Trim() == "" ? (object)DBNull.Value : dr["NO_OF_DAYS"].ToString().Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@isHalfDay", dr["IS_HALFDAY"].ToString().Trim() == "" ? (object)DBNull.Value : dr["IS_HALFDAY"].ToString().Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@leaveStatus", dr["LEAVE_STATUS"].ToString().Trim() == "" ? (object)DBNull.Value : dr["LEAVE_STATUS"].ToString().Trim()));
                        mySqlCmd.Parameters.Add(new MySqlParameter("@rosterId", dr["ROSTR_ID"].ToString().Trim() == "" ? (object)DBNull.Value : dr["ROSTR_ID"].ToString().Trim()));

                        mySqlCmd.Parameters.Add(new MySqlParameter("@userId", userId.Trim() == "" ? (object)DBNull.Value : userId.Trim()));

                        sMySqlString = "";
                        sMySqlString = " INSERT INTO EMPLOYEE_LEAVE_SCHEDULE(LEAVE_SHEET_ID,LEAVE_DATE,EMPLOYEE_ID,LEAVE_TYPE_ID,SCHEME_LINE_NO,FROM_TIME,TO_TIME,COVERED_BY," +
                                        "REMARKS,NO_OF_DAYS,IS_HALFDAY,LEAVE_STATUS,ADDED_DATE,ADDED_BY,MODIFIED_DATE,MODIFIED_BY," +
                                        "RECOMMAND_BY,ROSTR_ID) VALUES(@leaveSheetId,@leaveDate,@employeeId,@leave_type_id,@scheme_line_no,@fromTime,@toTime,@coveredBy," +
                                        "@remarks,@noOfDays,@isHalfDay,@leaveStatus,now(),@userId,now(),@userId,@recommandBy,@rosterId)";


                        mySqlCmd.CommandText = sMySqlString;

                        mySqlCmd.ExecuteNonQuery();
                    }

                }

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


        public DataTable getEmployeeLeveHistory(string employeeId, string sYear)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = @" SELECT els.EMPLOYEE_ID,DATE_FORMAT(els.LEAVE_DATE, '%Y/%m/%d') AS LEAVE_DATE,els.LEAVE_TYPE_ID,els.SCHEME_LINE_NO,els.FROM_TIME,
                                          els.TO_TIME,els.COVERED_BY,emp.KNOWN_NAME as COVERED_NAME ,
	                                      els.APPROVED_BY,empe.KNOWN_NAME as APPROVED_NAME,els.NO_OF_DAYS,  
	                                      case   when els.LEAVE_STATUS ='0' then 'Pending' 
	                                    	    when els.LEAVE_STATUS ='1' then 'Approved'  
	                                   	        when els.LEAVE_STATUS ='2' then 'Rejected' 
	                                     end as LEAVE_STATUS   
                                          FROM   EMPLOYEE_LEAVE_SCHEDULE els inner join EMPLOYEE emp on els.COVERED_BY = emp.EMPLOYEE_ID 
                                       	        inner join EMPLOYEE empe on els.TO_APPROVE = empe.EMPLOYEE_ID 
                                          where  CAST(Year(els.LEAVE_DATE) as CHAR)='" + sYear.Trim() + "' and  els.EMPLOYEE_ID='" + employeeId.Trim() + "'";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getLeavesToApprove(string employeeId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT els.EMPLOYEE_ID,emp.KNOWN_NAME empName,DATE_FORMAT(els.LEAVE_DATE, '%Y/%m/%d') AS LEAVE_DATE,els.LEAVE_TYPE_ID,els.SCHEME_LINE_NO,els.FROM_TIME, " +
                                        " 	   els.TO_TIME,empe.KNOWN_NAME as covName ,els.NO_OF_DAYS " +
                                        " FROM   EMPLOYEE_LEAVE_SCHEDULE els inner join EMPLOYEE emp on els.EMPLOYEE_ID = emp.EMPLOYEE_ID " +
                                        " 	   inner join EMPLOYEE empe on els.COVERED_BY = empe.EMPLOYEE_ID " +
                                        " where  els.LEAVE_STATUS='" + Constants.CON_LEAVE_PENDING_STATUS + "' and els.TO_APPROVE='" + employeeId.Trim() + "'";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow getLeaveDetailForAGivenLeave(string employeeId, string leaveDate)
        {
            try
            {
                DataRow dr = null;

                dataTable.Rows.Clear();
                string sMySqlString = " SELECT LEAVE_TYPE_ID,SCHEME_LINE_NO,FROM_TIME,TO_TIME,COVERED_BY,NO_OF_DAYS,TO_APPROVE,ifnull(REMARKS,'') as REMARKS " +
                                        " FROM EMPLOYEE_LEAVE_SCHEDULE " +
                                        " where EMPLOYEE_ID='" + employeeId.Trim() + "' and LEAVE_DATE='" + leaveDate.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
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

        public void approveRejectLeave(string statusCode, string employeeId, string leaveDate, string approveBy)
        {
            //Boolean isUpdated = false;          

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Clear();
                mySqlCmd.Parameters.Add(new MySqlParameter("@employeeId", employeeId.Trim() == "" ? (object)DBNull.Value : employeeId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@leaveDate", leaveDate.Trim() == "" ? (object)DBNull.Value : leaveDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@approveBy", approveBy.Trim() == "" ? (object)DBNull.Value : approveBy.Trim()));

                //sMySqlString = " update EMPLOYEE_LEAVE_SCHEDULE set APPROVED_BY='" + approveBy.Trim() + "',APPROVED_DATE=now(),LEAVE_STATUS='" + statusCode.Trim() + "'" +
                //                " where EMPLOYEE_ID='" + employeeId.Trim() + "' and LEAVE_DATE='" + leaveDate.Trim() + "'";

                sMySqlString = " update EMPLOYEE_LEAVE_SCHEDULE set APPROVED_BY=@approveBy,APPROVED_DATE=now(),LEAVE_STATUS=@statusCode" +
                                " where EMPLOYEE_ID=@employeeId and LEAVE_DATE=@leaveDate";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                // isUpdated = true;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }


        }


        public DataTable getACMLeavesSummary(string employeeId, string year)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT lt.LEAVE_TYPE_ID,lt.LEAVE_TYPE_NAME, IFNULL(lsi.NUMBER_OF_DAYS,0) NUMBER_OF_DAYS,ROUND(IFNULL(loj.leaves_taken,0),1) leaves_taken, " +
                                        " case when loj.leaves_taken is null then IFNULL(lsi.NUMBER_OF_DAYS,0) " +
                                        "      when loj.leaves_taken > 0 then (lsi.NUMBER_OF_DAYS - loj.leaves_taken) " +
                                        " end as Leaves_Remain " +
                                        " FROM EMPLOYEE_LEAVE_SCHEME as els inner join LEAVE_SCHEME_ITEM as lsi on els.LEAVE_SCHEME_ID = lsi.LEAVE_SCHEME_ID " +
                                        " 	 inner join LEAVE_TYPE as lt on lsi.LEAVE_TYPE_ID = lt.LEAVE_TYPE_ID " +
                                        " 	 left outer join ( " +
                                        " 	 select lty.LEAVE_TYPE_NAME,sum(elsh.NO_OF_DAYS) as leaves_taken " +
                                        " 	 from EMPLOYEE_LEAVE_SCHEDULE as elsh inner join LEAVE_TYPE as lty on elsh.LEAVE_TYPE_ID=lty.LEAVE_TYPE_ID " +
                                        " 	 where elsh.LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_REJECTED + "' AND elsh.LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_DISCARDED + "' AND" +
                                        "    CAST(YEAR(elsh.LEAVE_DATE) as CHAR) ='" + year.Trim() + "' and elsh.EMPLOYEE_ID ='" + employeeId.Trim() + "' " +
                                        " 	 group by lty.LEAVE_TYPE_NAME ) as loj on loj.LEAVE_TYPE_NAME = lt.LEAVE_TYPE_NAME " +
                                        " WHERE els.STATUS_CODE ='1' and lt.CATEGORY in ('A','C','M') and  els.EMPLOYEE_ID ='" + employeeId.Trim() + "'";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getACLeavesSummary(string employeeId, string year)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT lt.LEAVE_TYPE_ID,lt.LEAVE_TYPE_NAME, IFNULL(lsi.NUMBER_OF_DAYS,0) NUMBER_OF_DAYS,ROUND(IFNULL(loj.leaves_taken,0),1) leaves_taken, " +
                                        " case when loj.leaves_taken is null then IFNULL(lsi.NUMBER_OF_DAYS,0) " +
                                        "      when loj.leaves_taken > 0 then (lsi.NUMBER_OF_DAYS - loj.leaves_taken) " +
                                        " end as Leaves_Remain " +
                                        " FROM EMPLOYEE_LEAVE_SCHEME as els inner join LEAVE_SCHEME_ITEM as lsi on els.LEAVE_SCHEME_ID = lsi.LEAVE_SCHEME_ID " +
                                        " 	 inner join LEAVE_TYPE as lt on lsi.LEAVE_TYPE_ID = lt.LEAVE_TYPE_ID " +
                                        " 	 left outer join ( " +
                                        " 	 select lty.LEAVE_TYPE_NAME,sum(elsh.NO_OF_DAYS) as leaves_taken " +
                                        " 	 from EMPLOYEE_LEAVE_SCHEDULE as elsh inner join LEAVE_TYPE as lty on elsh.LEAVE_TYPE_ID=lty.LEAVE_TYPE_ID " +
                                        " 	 where elsh.LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_REJECTED + "' AND elsh.LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_DISCARDED + "' AND" +
                                        "    CAST(YEAR(elsh.LEAVE_DATE) as CHAR) ='" + year.Trim() + "' and elsh.EMPLOYEE_ID ='" + employeeId.Trim() + "' " +
                                        " 	 group by lty.LEAVE_TYPE_NAME ) as loj on loj.LEAVE_TYPE_NAME = lt.LEAVE_TYPE_NAME " +
                                        " WHERE els.STATUS_CODE ='1' and lt.CATEGORY in ('A','C') and  els.EMPLOYEE_ID ='" + employeeId.Trim() + "'";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataRow getWorkingTime(string employeeId, string sDate, string toDate)
        //{
        //    try
        //    {
        //        DataRow wTime = null;

        //        //Boolean IsRoster = isRosterEmployee(employeeId, toDate);

        //        wTime = getRosterWorkingTime(employeeId, sDate);

        //        if (wTime == null)
        //        {
        //            wTime = getCompanyWorkingTime(employeeId);
        //        }

        //        return wTime;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private Boolean isRosterEmployee(string employeeId, string toDate)
        {
            Boolean isRoster = false;

            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT DUTY_DATE " +
                                        " FROM EMPLOYEE_ROSTER_DATE " +
                                        " where DUTY_DATE > '" + toDate.Trim() + "' and EMPLOYEE_ID='" + employeeId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);

                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isRoster = true;
                }

                return isRoster;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public DataRow getCompanyWorkingTime(string employeeId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT com.WORK_HOURS_START as FROM_TIME,com.WORK_HOURS_END as TO_TIME " +
                                        " FROM COMPANY as com,EMPLOYEE as emp " +
                                        " WHERE emp.COMPANY_ID = com.COMPANY_ID and emp.EMPLOYEE_ID ='" + employeeId.Trim() + "'";

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

        public DataTable getRosterWorkingTime(string employeeId, string sDate)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString =  " SELECT erd.ROSTR_ID,ros.FROM_TIME,ros.TO_TIME,ros.NUM_DAYS,ros.ROSTER_TYPE  " +
                                        " FROM EMPLOYEE_ROSTER_DATE as erd,ROSTER as ros " +
                                        " where erd.ROSTR_ID = ros.ROSTR_ID and erd.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE +
                                        "' and erd.DUTY_DATE = '" + sDate.Trim() + "' and erd.EMPLOYEE_ID='" + employeeId.Trim() + "'";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);

                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable getLeavesHistory(string employeeId, string year)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT LEAVE_SHEET_ID,EMPLOYEE_ID,CAST(date(FROM_DATE) as CHAR) as FROM_DATE,CAST(date(TO_DATE) as CHAR) as TO_DATE,cast(NO_OF_DAYS as char) NO_OF_DAYS,LEAVE_STATUS, " +
                                        " case when LEAVE_STATUS='0' then 'Rejected' " +
                                        "      when LEAVE_STATUS='1' then 'Active' " +
                                        "      when LEAVE_STATUS='2' then 'Covered' " +
                                        " 	 when LEAVE_STATUS='3' then 'Recommanded' " +
                                        " 	 when LEAVE_STATUS='4' then 'Approved' " +
                                        " 	 when LEAVE_STATUS='9' then 'Discarded' " +
                                        " End as STATUS_CODE " +
                                        " FROM LEAVE_SHEET " +
                                        " Where EMPLOYEE_ID='" + employeeId.Trim() + "' and CAST(YEAR(FROM_DATE) as CHAR) ='" + year.Trim() + "'" +
                                        " order by FROM_DATE DESC";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean isOnLeave(string employeeId, string sDate)
        {
            Boolean isExist = false;
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT LEAVE_DATE " +
                                      " FROM EMPLOYEE_LEAVE_SCHEDULE " +
                                      " WHERE LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_REJECTED + "'" +
                                      " AND LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_DISCARDED + "'" +
                                      " AND LEAVE_DATE='" + sDate.Trim() +
                                      "' AND EMPLOYEE_ID='" + employeeId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExist = true;
                }

                return isExist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public Boolean isOnLeave(string employeeId, string sDate, string rosterID)
        {
            Boolean isExist = false;
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT LEAVE_DATE " +
                                      " FROM EMPLOYEE_LEAVE_SCHEDULE " +
                                      " WHERE LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_REJECTED + "'" +
                                      " AND LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_DISCARDED + "'" +
                                      " AND LEAVE_DATE='" + sDate.Trim() + "' " +
                                      " AND ROSTR_ID = '" + rosterID.Trim() + "' " +
                                      " AND EMPLOYEE_ID = '" + employeeId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    isExist = true;
                }

                return isExist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double getMonthlyShortLeavesTaken(string employeeId, string sYear, String sMonth)
        {
            double nShortLeaves = 0;

            mySqlCmd.CommandText = "SELECT count(LEAVE_TYPE_ID) " +
                                    " FROM EMPLOYEE_LEAVE_SCHEDULE " +
                                    " where CAST(Month(LEAVE_DATE) as char)  ='" + sMonth.Trim() + "' and CAST(Year(LEAVE_DATE) as char) ='" + sYear.Trim() + "' " +
                                    " and LEAVE_TYPE_ID='SL' and EMPLOYEE_ID='" + employeeId.Trim() + "'  and LEAVE_STATUS <> '0' and LEAVE_STATUS <> '9'";
            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    nShortLeaves = Double.Parse(mySqlCmd.ExecuteScalar().ToString());
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return nShortLeaves;
        }

        public void coverRecommandApproveLeaveSheet(string statusCode, string leaveSheetId,string preStatus)
        {
            //Boolean isUpdated = false;          

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Clear();
                
                mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@leaveSheetId", leaveSheetId.Trim() == "" ? (object)DBNull.Value : leaveSheetId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@preStatus", preStatus.Trim() == "" ? (object)DBNull.Value : preStatus.Trim()));
                //sMySqlString = " update EMPLOYEE_LEAVE_SCHEDULE set APPROVED_BY='" + approveBy.Trim() + "',APPROVED_DATE=now(),LEAVE_STATUS='" + statusCode.Trim() + "'" +
                //                " where EMPLOYEE_ID='" + employeeId.Trim() + "' and LEAVE_DATE='" + leaveDate.Trim() + "'";

                sMySqlString = " UPDATE LEAVE_SHEET SET LEAVE_STATUS =@statusCode WHERE LEAVE_SHEET_ID =@leaveSheetId and LEAVE_STATUS =@preStatus";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                sMySqlString = "";

                if(statusCode.Trim().Equals(Constants.LEAVE_STATUS_REJECTED))
                {
                    sMySqlString =  " update EMPLOYEE_LEAVE_SCHEDULE set LEAVE_STATUS=@statusCode,IS_SUMMARIZED='" + Constants.CON_IS_SUMMARIZED_NO + "'" +
                                    " where LEAVE_SHEET_ID =@leaveSheetId and LEAVE_STATUS =@preStatus";
                }
                else
                {
                    sMySqlString =  " update EMPLOYEE_LEAVE_SCHEDULE set LEAVE_STATUS=@statusCode" +
                                    " where LEAVE_SHEET_ID =@leaveSheetId and LEAVE_STATUS =@preStatus";
                }

                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                // isUpdated = true;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }
        }
        
        public void coverRecommandApproveLeaveSheet(string statusCode, string leaveSheetId)
        {
            //Boolean isUpdated = false;          

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@leaveSheetId", leaveSheetId.Trim() == "" ? (object)DBNull.Value : leaveSheetId.Trim()));
                //mySqlCmd.Parameters.Add(new MySqlParameter("@preStatus", preStatus.Trim() == "" ? (object)DBNull.Value : preStatus.Trim()));
                //sMySqlString = " update EMPLOYEE_LEAVE_SCHEDULE set APPROVED_BY='" + approveBy.Trim() + "',APPROVED_DATE=now(),LEAVE_STATUS='" + statusCode.Trim() + "'" +
                //                " where EMPLOYEE_ID='" + employeeId.Trim() + "' and LEAVE_DATE='" + leaveDate.Trim() + "'";

                sMySqlString = " UPDATE LEAVE_SHEET SET LEAVE_STATUS =@statusCode WHERE LEAVE_SHEET_ID =@leaveSheetId";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                sMySqlString = "";
                sMySqlString = " update EMPLOYEE_LEAVE_SCHEDULE set LEAVE_STATUS=@statusCode" +
                                " where LEAVE_SHEET_ID =@leaveSheetId";

                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                // isUpdated = true;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }
        }

        public void coverRecommandApproveLeaveSheet(string statusCode, string leaveSheetId,string approvedBy,string preStatus)
        {
            //Boolean isUpdated = false;          

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;


            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Clear();

                mySqlCmd.Parameters.Add(new MySqlParameter("@statusCode", statusCode.Trim() == "" ? (object)DBNull.Value : statusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@leaveSheetId", leaveSheetId.Trim() == "" ? (object)DBNull.Value : leaveSheetId.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@approvedBy", approvedBy.Trim() == "" ? (object)DBNull.Value : approvedBy.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@preStatus", preStatus.Trim() == "" ? (object)DBNull.Value : preStatus.Trim()));
                //sMySqlString = " update EMPLOYEE_LEAVE_SCHEDULE set APPROVED_BY='" + approveBy.Trim() + "',APPROVED_DATE=now(),LEAVE_STATUS='" + statusCode.Trim() + "'" +
                //                " where EMPLOYEE_ID='" + employeeId.Trim() + "' and LEAVE_DATE='" + leaveDate.Trim() + "'";

                sMySqlString = " UPDATE LEAVE_SHEET SET LEAVE_STATUS =@statusCode,APPROVED_BY=@approvedBy,APPROVED_DATE=now() WHERE LEAVE_SHEET_ID =@leaveSheetId and LEAVE_STATUS =@preStatus";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                sMySqlString = "";
                sMySqlString = " update EMPLOYEE_LEAVE_SCHEDULE set LEAVE_STATUS=@statusCode,APPROVED_BY=@approvedBy,APPROVED_DATE=now()" +
                                " where LEAVE_SHEET_ID =@leaveSheetId and LEAVE_STATUS =@preStatus";

                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                // isUpdated = true;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                throw ex;
            }
        }

        public DataTable getLeavesToCoverRecommand(string employeeId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT ls.LEAVE_SHEET_ID,ls.EMPLOYEE_ID,e.KNOWN_NAME empName, " +
                                        " CAST(DATE_FORMAT(ls.FROM_DATE, '%Y/%m/%d') as char) AS FROM_DATE,CAST(DATE_FORMAT(ls.TO_DATE, '%Y/%m/%d') as char) AS TO_DATE, " +
                                        " cast(ls.NO_OF_DAYS  as char) NO_OF_DAYS,ls.COVERED_BY,RECOMMEND_BY,ls.APPROVED_BY,ls.LEAVE_STATUS, " +
                                        " case " +
                                        " 	when LEAVE_STATUS='1' then 'Cover' " +
                                        " 	when LEAVE_STATUS='2' then 'Recommand' " +
                                        " 	when LEAVE_STATUS='3' then 'Approve' " +
                                        " end as Action_Need " +
                                        " FROM LEAVE_SHEET as ls,EMPLOYEE as e " +
                                        " where ls.EMPLOYEE_ID=e.EMPLOYEE_ID AND ls.LEAVE_STATUS = '" + Constants.LEAVE_STATUS_ACTIVE_VALUE + "' AND ls.COVERED_BY ='" + employeeId.Trim() + "'" +
                                        " UNION " +
                                        " SELECT ls.LEAVE_SHEET_ID,ls.EMPLOYEE_ID,e.KNOWN_NAME empName, " +
                                        " CAST(DATE_FORMAT(ls.FROM_DATE, '%Y/%m/%d') as char) AS FROM_DATE,CAST(DATE_FORMAT(ls.TO_DATE, '%Y/%m/%d') as char) AS TO_DATE, " +
                                        " cast(ls.NO_OF_DAYS  as char) NO_OF_DAYS,ls.COVERED_BY,RECOMMEND_BY,ls.APPROVED_BY,ls.LEAVE_STATUS, " +
                                        " case " +
                                        " 	when LEAVE_STATUS='1' then 'Cover' " +
                                        " 	when LEAVE_STATUS='2' then 'Recommand' " +
                                        " 	when LEAVE_STATUS='3' then 'Approve' " +
                                        " end as Action_Need " +
                                        " FROM LEAVE_SHEET as ls,EMPLOYEE as e " +
                                        " where ls.EMPLOYEE_ID=e.EMPLOYEE_ID AND  ls.LEAVE_STATUS = '" + Constants.LEAVE_STATUS_COVERED + "' AND ls.RECOMMEND_BY ='" + employeeId.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getLeavesToApproveByHR(string companyId)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT ls.LEAVE_SHEET_ID,ls.EMPLOYEE_ID,e.KNOWN_NAME empName, " +
                                        " CAST(DATE_FORMAT(ls.FROM_DATE, '%Y/%m/%d') as char) AS FROM_DATE,CAST(DATE_FORMAT(ls.TO_DATE, '%Y/%m/%d') as char) AS TO_DATE, " +
                                        " ls.NO_OF_DAYS,ls.COVERED_BY,emp.KNOWN_NAME coverdName,ls.RECOMMEND_BY, " +
                                        " empl.KNOWN_NAME recommandName,ls.APPROVED_BY,ls.LEAVE_STATUS, " +
                                        " case  " +
                                        " 	when LEAVE_STATUS='1' then 'Cover' " +
                                        " 	when LEAVE_STATUS='2' then 'Recommand' " +
                                        " 	when LEAVE_STATUS='3' then 'Approve' " +
                                        " end as Action_Need " +
                                        " FROM LEAVE_SHEET as ls,EMPLOYEE as e,EMPLOYEE as emp,EMPLOYEE as empl " +
                                        " where ls.EMPLOYEE_ID=e.EMPLOYEE_ID AND emp.EMPLOYEE_ID=ls.COVERED_BY " +
                                        " AND empl.EMPLOYEE_ID=ls.RECOMMEND_BY AND e.COMPANY_ID = '" + companyId.Trim() + "' " +
                                        " AND (ls.LEAVE_STATUS = '" + Constants.LEAVE_STATUS_RECOMMAND + "')";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getLeavesToApproveByHR()
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT ls.LEAVE_SHEET_ID,ls.EMPLOYEE_ID,e.KNOWN_NAME empName, " +
                                        " CAST(DATE_FORMAT(ls.FROM_DATE, '%Y/%m/%d') as char) AS FROM_DATE,CAST(DATE_FORMAT(ls.TO_DATE, '%Y/%m/%d') as char) AS TO_DATE, " +
                                        " ls.NO_OF_DAYS,ls.COVERED_BY,emp.KNOWN_NAME coverdName,ls.RECOMMEND_BY, " +
                                        " empl.KNOWN_NAME recommandName,ls.APPROVED_BY,ls.LEAVE_STATUS, " +
                                        " case  " +
                                        " 	when LEAVE_STATUS='1' then 'Cover' " +
                                        " 	when LEAVE_STATUS='2' then 'Recommand' " +
                                        " 	when LEAVE_STATUS='3' then 'Approve' " +
                                        " end as Action_Need " +
                                        " FROM LEAVE_SHEET as ls,EMPLOYEE as e,EMPLOYEE as emp,EMPLOYEE as empl " +
                                        " where ls.EMPLOYEE_ID=e.EMPLOYEE_ID AND emp.EMPLOYEE_ID=ls.COVERED_BY " +
                                        " AND empl.EMPLOYEE_ID=ls.RECOMMEND_BY" +
                                        " AND (ls.LEAVE_STATUS = '" + Constants.LEAVE_STATUS_RECOMMAND + "')";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getLeaveSheetDetails(string leaveSheetId)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = " SELECT LEAVE_SHEET_ID,LEAVE_DATE,LEAVE_TYPE_ID,FROM_TIME,TO_TIME, " +
                                      " case when IS_HALFDAY='F' then 'Full Day' " +
	                                  " when IS_HALFDAY='H' then 'Half Day' " +   
	                                  " when IS_HALFDAY='S' then 'Short Leave' " +                 
                                      " End as LEAVE_NATURE,NO_OF_DAYS " +
                                      " FROM EMPLOYEE_LEAVE_SCHEDULE " +
                                      " where LEAVE_SHEET_ID='" + leaveSheetId.Trim() + "' " +
                                      " order by LEAVE_DATE"; 

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow getEmployeeRecommandBy(string leaveSheetId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT 	EMPLOYEE_ID,RECOMMEND_BY,NO_OF_DAYS, " +
                                      " CAST(DATE_FORMAT(FROM_DATE, '%Y/%m/%d') as char) AS FROM_DATE,CAST(DATE_FORMAT(TO_DATE, '%Y/%m/%d') as char) AS TO_DATE,LEAVE_STATUS  " +
                                      " FROM 	LEAVE_SHEET where LEAVE_SHEET_ID = '" + leaveSheetId.Trim() + "'";
                 
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

        public DataRow getEmployeeCoveredBy(string leaveSheetId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT 	EMPLOYEE_ID,COVERED_BY,NO_OF_DAYS, " +
                                      " CAST(DATE_FORMAT(FROM_DATE, '%Y/%m/%d') as char) AS FROM_DATE,CAST(DATE_FORMAT(TO_DATE, '%Y/%m/%d') as char) AS TO_DATE,LEAVE_STATUS  " +
                                      " FROM 	LEAVE_SHEET where LEAVE_SHEET_ID = '" + leaveSheetId.Trim() + "'";

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


        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 04-03-2015
        // this function is written to used at webFrmLeveStatusView.aspx
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return leave statuses for a given employee and given duration
        ///<param name="employeeId">Pass a employeeid string to query </param>
        ///<param name="fromDate">Pass a fromDate string to query </param>
        ///<param name="toDate">Pass a toDate string to query </param>
        ///</summary>        
        //----------------------------------------------------------------------------------------

        public DataTable getEmployeeLeveStatusesForEmployee(string employeeId, string fromDate,string toDate,string statusCode)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = "";

                if (statusCode.Trim() == "")
                {
                    sMySqlString = " SELECT ls.LEAVE_SHEET_ID,ls.EMPLOYEE_ID,e.EPF_NO,concat(e.KNOWN_NAME) APPLICANT_NAME, " +
                                    " CAST(DATE_FORMAT(ls.FROM_DATE, '%Y/%m/%d') as char) AS FROM_DATE,CAST(DATE_FORMAT(ls.TO_DATE, '%Y/%m/%d') as char) AS TO_DATE, " +
                                    " ls.NO_OF_DAYS,ls.COVERED_BY,emp.KNOWN_NAME COVERED_BY_NAME,ls.RECOMMEND_BY, " +
                                    " empl.KNOWN_NAME RECOMMEND_BY_NAME, " +
                                    " case " +
                                    " 	when LEAVE_STATUS='0' then 'Rejected' " +
                                    " 	when LEAVE_STATUS='1' then 'Pending' " +
                                    " 	when LEAVE_STATUS='2' then 'Covered' " +
                                    " 	when LEAVE_STATUS='3' then 'Recommended' " +
                                    " 	when LEAVE_STATUS='4' then 'HR Approved' " +
                                    " 	when LEAVE_STATUS='9' then 'Discarded' " +
                                    " end as LEAVE_STATUS " +
                                    " FROM LEAVE_SHEET as ls,EMPLOYEE as e,EMPLOYEE as emp,EMPLOYEE as empl " +
                                    " where ls.EMPLOYEE_ID=e.EMPLOYEE_ID AND emp.EMPLOYEE_ID=ls.COVERED_BY " +
                                    " AND empl.EMPLOYEE_ID=ls.RECOMMEND_BY AND ls.FROM_DATE >= '" + fromDate.Trim() + "' " +
                                    " AND ls.FROM_DATE <= '" + toDate.Trim() + "' " +
                                    " AND ls.EMPLOYEE_ID = '" + employeeId.Trim() + "'" +
                                    " order by ls.FROM_DATE,cast(ls.LEAVE_STATUS as SIGNED)";
                }
                else
                {
                    sMySqlString = " SELECT ls.LEAVE_SHEET_ID,ls.EMPLOYEE_ID,e.EPF_NO,concat(e.KNOWN_NAME) APPLICANT_NAME, " +
                                    " CAST(DATE_FORMAT(ls.FROM_DATE, '%Y/%m/%d') as char) AS FROM_DATE,CAST(DATE_FORMAT(ls.TO_DATE, '%Y/%m/%d') as char) AS TO_DATE, " +
                                    " ls.NO_OF_DAYS,ls.COVERED_BY,emp.KNOWN_NAME COVERED_BY_NAME,ls.RECOMMEND_BY, " +
                                    " empl.KNOWN_NAME RECOMMEND_BY_NAME, " +
                                    " case " +
                                    " 	when LEAVE_STATUS='0' then 'Rejected' " +
                                    " 	when LEAVE_STATUS='1' then 'Pending' " +
                                    " 	when LEAVE_STATUS='2' then 'Covered' " +
                                    " 	when LEAVE_STATUS='3' then 'Recommended' " +
                                    " 	when LEAVE_STATUS='4' then 'HR Approved' " +
                                    " 	when LEAVE_STATUS='9' then 'Discarded' " +
                                    " end as LEAVE_STATUS " +
                                    " FROM LEAVE_SHEET as ls,EMPLOYEE as e,EMPLOYEE as emp,EMPLOYEE as empl " +
                                    " where ls.EMPLOYEE_ID=e.EMPLOYEE_ID AND emp.EMPLOYEE_ID=ls.COVERED_BY " +
                                    " AND empl.EMPLOYEE_ID=ls.RECOMMEND_BY AND ls.FROM_DATE >= '" + fromDate.Trim() + "' " +
                                    " AND ls.FROM_DATE <= '" + toDate.Trim() + "' " +
                                    " AND ls.EMPLOYEE_ID = '" + employeeId.Trim() + "' AND  ls.LEAVE_STATUS = '" + statusCode.Trim() + "'" +
                                    " order by ls.FROM_DATE,cast(ls.LEAVE_STATUS as SIGNED)";



                }
                    
                                       
   
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 04-03-2015
        // this function is written to used at webFrmLeveStatusView.aspx
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return leave statuses for a given employee and given duration
        ///<param name="companyId">Pass a companyId string to query </param>
        ///<param name="fromDate">Pass a fromDate string to query </param>
        ///<param name="toDate">Pass a toDate string to query </param>
        ///</summary>        
        //----------------------------------------------------------------------------------------

        public DataTable getEmployeeLeveStatusesForCompany(string companyId, string fromDate, string toDate, string statusCode)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = "";

                if (statusCode.Trim() == "")
                {

                    sMySqlString = " SELECT ls.LEAVE_SHEET_ID,ls.EMPLOYEE_ID,e.EPF_NO,concat(e.KNOWN_NAME) APPLICANT_NAME, " +
                                        " CAST(DATE_FORMAT(ls.FROM_DATE, '%Y/%m/%d') as char) AS FROM_DATE,CAST(DATE_FORMAT(ls.TO_DATE, '%Y/%m/%d') as char) AS TO_DATE, " +
                                        " ls.NO_OF_DAYS,ls.COVERED_BY,emp.KNOWN_NAME COVERED_BY_NAME,ls.RECOMMEND_BY, " +
                                        " empl.KNOWN_NAME RECOMMEND_BY_NAME, " +
                                        " case " +
                                        " 	when LEAVE_STATUS='0' then 'Rejected' " +
                                        " 	when LEAVE_STATUS='1' then 'Pending' " +
                                        " 	when LEAVE_STATUS='2' then 'Covered' " +
                                        " 	when LEAVE_STATUS='3' then 'Recommended' " +
                                        " 	when LEAVE_STATUS='4' then 'HR Approved' " +
                                        " 	when LEAVE_STATUS='9' then 'Discarded' " +
                                        " end as LEAVE_STATUS " +
                                        " FROM LEAVE_SHEET as ls,EMPLOYEE as e,EMPLOYEE as emp,EMPLOYEE as empl " +
                                        " where ls.EMPLOYEE_ID=e.EMPLOYEE_ID AND emp.EMPLOYEE_ID=ls.COVERED_BY " +
                                        " AND empl.EMPLOYEE_ID=ls.RECOMMEND_BY AND ls.FROM_DATE >= '" + fromDate.Trim() + "' " +
                                        " AND ls.FROM_DATE <= '" + toDate.Trim() + "' " +
                                        " AND e.COMPANY_ID = '" + companyId.Trim() + "' " +
                                        " order by ls.FROM_DATE,cast(ls.LEAVE_STATUS as SIGNED)";
                }
                else
                {
                    sMySqlString = " SELECT ls.LEAVE_SHEET_ID,ls.EMPLOYEE_ID,e.EPF_NO,concat(e.KNOWN_NAME) APPLICANT_NAME, " +
                                        " CAST(DATE_FORMAT(ls.FROM_DATE, '%Y/%m/%d') as char) AS FROM_DATE,CAST(DATE_FORMAT(ls.TO_DATE, '%Y/%m/%d') as char) AS TO_DATE, " +
                                        " ls.NO_OF_DAYS,ls.COVERED_BY,emp.KNOWN_NAME COVERED_BY_NAME,ls.RECOMMEND_BY, " +
                                        " empl.KNOWN_NAME RECOMMEND_BY_NAME, " +
                                        " case " +
                                        " 	when LEAVE_STATUS='0' then 'Rejected' " +
                                        " 	when LEAVE_STATUS='1' then 'Pending' " +
                                        " 	when LEAVE_STATUS='2' then 'Covered' " +
                                        " 	when LEAVE_STATUS='3' then 'Recommended' " +
                                        " 	when LEAVE_STATUS='4' then 'HR Approved' " +
                                        " 	when LEAVE_STATUS='9' then 'Discarded' " +
                                        " end as LEAVE_STATUS " +
                                        " FROM LEAVE_SHEET as ls,EMPLOYEE as e,EMPLOYEE as emp,EMPLOYEE as empl " +
                                        " where ls.EMPLOYEE_ID=e.EMPLOYEE_ID AND emp.EMPLOYEE_ID=ls.COVERED_BY " +
                                        " AND empl.EMPLOYEE_ID=ls.RECOMMEND_BY AND ls.FROM_DATE >= '" + fromDate.Trim() + "' " +
                                        " AND ls.FROM_DATE <= '" + toDate.Trim() + "' " +
                                        " AND e.COMPANY_ID = '" + companyId.Trim() + "' AND  ls.LEAVE_STATUS = '" + statusCode.Trim() + "'" +
                                        " order by ls.FROM_DATE,cast(ls.LEAVE_STATUS as SIGNED)";


                }


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 04-03-2015
        // this function is written to used at webFrmLeveStatusView.aspx
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return leave statuses for a given employee and given duration
        ///<param name="companyId">Pass a companyId string to query </param>
        ///<param name="deptId">Pass a deptId string to query </param>
        ///<param name="fromDate">Pass a fromDate string to query </param>
        ///<param name="toDate">Pass a toDate string to query </param>
        ///</summary>        
        //----------------------------------------------------------------------------------------

        public DataTable getEmployeeLeveStatusesForCompanyDepartment(string companyId, string deptId, string fromDate, string toDate, string statusCode)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = "";

                if (statusCode.Trim() == "")
                {

                    sMySqlString = " SELECT ls.LEAVE_SHEET_ID,ls.EMPLOYEE_ID,e.EPF_NO,concat(e.KNOWN_NAME) APPLICANT_NAME, " +
                                       " CAST(DATE_FORMAT(ls.FROM_DATE, '%Y/%m/%d') as char) AS FROM_DATE,CAST(DATE_FORMAT(ls.TO_DATE, '%Y/%m/%d') as char) AS TO_DATE, " +
                                       " ls.NO_OF_DAYS,ls.COVERED_BY,emp.KNOWN_NAME COVERED_BY_NAME,ls.RECOMMEND_BY, " +
                                       " empl.KNOWN_NAME RECOMMEND_BY_NAME, " +
                                       " case " +
                                       " 	when LEAVE_STATUS='0' then 'Rejected' " +
                                       " 	when LEAVE_STATUS='1' then 'Pending' " +
                                       " 	when LEAVE_STATUS='2' then 'Covered' " +
                                       " 	when LEAVE_STATUS='3' then 'Recommended' " +
                                       " 	when LEAVE_STATUS='4' then 'HR Approved' " +
                                       " 	when LEAVE_STATUS='9' then 'Discarded' " +
                                       " end as LEAVE_STATUS " +
                                       " FROM LEAVE_SHEET as ls,EMPLOYEE as e,EMPLOYEE as emp,EMPLOYEE as empl " +
                                       " where ls.EMPLOYEE_ID=e.EMPLOYEE_ID AND emp.EMPLOYEE_ID=ls.COVERED_BY " +
                                       " AND empl.EMPLOYEE_ID=ls.RECOMMEND_BY AND ls.FROM_DATE >= '" + fromDate.Trim() + "' " +
                                       " AND ls.FROM_DATE <= '" + toDate.Trim() + "' " +
                                       " AND e.COMPANY_ID = '" + companyId.Trim() + "' AND e.DEPT_ID = '" + deptId.Trim() + "' " +
                                       " order by ls.FROM_DATE,cast(ls.LEAVE_STATUS as SIGNED)";
                }
                else
                {
                    sMySqlString = " SELECT ls.LEAVE_SHEET_ID,ls.EMPLOYEE_ID,e.EPF_NO,concat(e.KNOWN_NAME) APPLICANT_NAME, " +
                                       " CAST(DATE_FORMAT(ls.FROM_DATE, '%Y/%m/%d') as char) AS FROM_DATE,CAST(DATE_FORMAT(ls.TO_DATE, '%Y/%m/%d') as char) AS TO_DATE, " +
                                       " ls.NO_OF_DAYS,ls.COVERED_BY,emp.KNOWN_NAME COVERED_BY_NAME,ls.RECOMMEND_BY, " +
                                       " empl.KNOWN_NAME RECOMMEND_BY_NAME, " +
                                       " case " +
                                       " 	when LEAVE_STATUS='0' then 'Rejected' " +
                                       " 	when LEAVE_STATUS='1' then 'Pending' " +
                                       " 	when LEAVE_STATUS='2' then 'Covered' " +
                                       " 	when LEAVE_STATUS='3' then 'Recommended' " +
                                       " 	when LEAVE_STATUS='4' then 'HR Approved' " +
                                       " 	when LEAVE_STATUS='9' then 'Discarded' " +
                                       " end as LEAVE_STATUS " +
                                       " FROM LEAVE_SHEET as ls,EMPLOYEE as e,EMPLOYEE as emp,EMPLOYEE as empl " +
                                       " where ls.EMPLOYEE_ID=e.EMPLOYEE_ID AND emp.EMPLOYEE_ID=ls.COVERED_BY " +
                                       " AND empl.EMPLOYEE_ID=ls.RECOMMEND_BY AND ls.FROM_DATE >= '" + fromDate.Trim() + "' " +
                                       " AND ls.FROM_DATE <= '" + toDate.Trim() + "' " +
                                       " AND e.COMPANY_ID = '" + companyId.Trim() + "' AND e.DEPT_ID = '" + deptId.Trim() + "'  AND  ls.LEAVE_STATUS = '" + statusCode.Trim() + "'" +
                                       " order by ls.FROM_DATE,cast(ls.LEAVE_STATUS as SIGNED)";


                }


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 06-03-2015
        // this function is written to used at webFrmLeveStatusView.aspx
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return leave statuses for a given employee and given duration
        ///<param name="companyId">Pass a companyId string to query </param>
        ///<param name="deptId">Pass a deptId string to query </param>
        ///<param name="division">Pass a division string to query </param>
        ///<param name="fromDate">Pass a fromDate string to query </param>
        ///<param name="toDate">Pass a toDate string to query </param>
        ///</summary>        
        //----------------------------------------------------------------------------------------

        public DataTable getEmployeeLeveStatusesForCompanyDepartmentDivision(string companyId, string deptId,string division, string fromDate, string toDate, string statusCode)
        {
            try
            {
                dataTable.Rows.Clear();

                string sMySqlString = "";

                if (statusCode.Trim() == "")
                {

                    sMySqlString =     " SELECT ls.LEAVE_SHEET_ID,ls.EMPLOYEE_ID,e.EPF_NO,concat(e.KNOWN_NAME) APPLICANT_NAME, " +
                                       " CAST(DATE_FORMAT(ls.FROM_DATE, '%Y/%m/%d') as char) AS FROM_DATE,CAST(DATE_FORMAT(ls.TO_DATE, '%Y/%m/%d') as char) AS TO_DATE, " +
                                       " ls.NO_OF_DAYS,ls.COVERED_BY,emp.KNOWN_NAME COVERED_BY_NAME,ls.RECOMMEND_BY, " +
                                       " empl.KNOWN_NAME RECOMMEND_BY_NAME, " +
                                       " case " +
                                       " 	when LEAVE_STATUS='0' then 'Rejected' " +
                                       " 	when LEAVE_STATUS='1' then 'Pending' " +
                                       " 	when LEAVE_STATUS='2' then 'Covered' " +
                                       " 	when LEAVE_STATUS='3' then 'Recommended' " +
                                       " 	when LEAVE_STATUS='4' then 'HR Approved' " +
                                       " 	when LEAVE_STATUS='9' then 'Discarded' " +
                                       " end as LEAVE_STATUS " +
                                       " FROM LEAVE_SHEET as ls,EMPLOYEE as e,EMPLOYEE as emp,EMPLOYEE as empl " +
                                       " where ls.EMPLOYEE_ID=e.EMPLOYEE_ID AND emp.EMPLOYEE_ID=ls.COVERED_BY " +
                                       " AND empl.EMPLOYEE_ID=ls.RECOMMEND_BY AND ls.FROM_DATE >= '" + fromDate.Trim() + "' " +
                                       " AND ls.FROM_DATE <= '" + toDate.Trim() + "' " +
                                       " AND e.COMPANY_ID = '" + companyId.Trim() + "' AND e.DEPT_ID = '" + deptId.Trim() + "' AND e.DIVISION_ID = '" + division.Trim() + "'" +
                                       " order by ls.FROM_DATE,cast(ls.LEAVE_STATUS as SIGNED)";
                }
                else
                {
                    sMySqlString = " SELECT ls.LEAVE_SHEET_ID,ls.EMPLOYEE_ID,e.EPF_NO,concat(e.KNOWN_NAME) APPLICANT_NAME, " +
                                       " CAST(DATE_FORMAT(ls.FROM_DATE, '%Y/%m/%d') as char) AS FROM_DATE,CAST(DATE_FORMAT(ls.TO_DATE, '%Y/%m/%d') as char) AS TO_DATE, " +
                                       " ls.NO_OF_DAYS,ls.COVERED_BY,emp.KNOWN_NAME COVERED_BY_NAME,ls.RECOMMEND_BY, " +
                                       " empl.KNOWN_NAME RECOMMEND_BY_NAME, " +
                                       " case " +
                                       " 	when LEAVE_STATUS='0' then 'Rejected' " +
                                       " 	when LEAVE_STATUS='1' then 'Pending' " +
                                       " 	when LEAVE_STATUS='2' then 'Covered' " +
                                       " 	when LEAVE_STATUS='3' then 'Recommended' " +
                                       " 	when LEAVE_STATUS='4' then 'HR Approved' " +
                                       " 	when LEAVE_STATUS='9' then 'Discarded' " +
                                       " end as LEAVE_STATUS " +
                                       " FROM LEAVE_SHEET as ls,EMPLOYEE as e,EMPLOYEE as emp,EMPLOYEE as empl " +
                                       " where ls.EMPLOYEE_ID=e.EMPLOYEE_ID AND emp.EMPLOYEE_ID=ls.COVERED_BY " +
                                       " AND empl.EMPLOYEE_ID=ls.RECOMMEND_BY AND ls.FROM_DATE >= '" + fromDate.Trim() + "' " +
                                       " AND ls.FROM_DATE <= '" + toDate.Trim() + "' " +
                                       " AND e.COMPANY_ID = '" + companyId.Trim() + "' AND e.DEPT_ID = '" + deptId.Trim() +
                                       "' AND e.DIVISION_ID = '" + division.Trim() + "' AND  ls.LEAVE_STATUS = '" + statusCode.Trim() + "'" +
                                       " order by ls.FROM_DATE,cast(ls.LEAVE_STATUS as SIGNED)";


                }


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //----------------------------------------------------------------------------------------
        // Anjana uduwaragoda on 24-09-2015
        // this function is written to used at webFrmLeveStatusView.aspx
        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Return short leave count for a given employee and given month
        ///<param name="employeeId">Pass a employeeId string to query </param>
        ///<param name="sMonth">Pass a month as yyyyMM string to query </param>
        
        ///</summary>        
        //----------------------------------------------------------------------------------------

        public decimal getShortLeaveCountForTheMonth(string employeeId,string sMonth)
        {

            decimal dShortLeaveCount = 0;

            mySqlCmd.CommandText = " SELECT count(LEAVE_DATE) FROM EMPLOYEE_LEAVE_SCHEDULE " +
                                   " where LEAVE_TYPE_ID='SL' and EMPLOYEE_ID='" + employeeId.Trim() +
                                   "' and EXTRACT(YEAR_MONTH FROM LEAVE_DATE) = '" + sMonth.Trim() + 
                                   "' and LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_REJECTED + "'" +
                                      " AND LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_DISCARDED + "'";               

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    dShortLeaveCount = Decimal.Parse(mySqlCmd.ExecuteScalar().ToString());
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return dShortLeaveCount;
        }

        public DataTable isLeaveExistForTheDay(string employeeId, string sDate)
        {

            try
            {
                DataTable dtLeaves = new DataTable();
                string sMySqlString = " SELECT LEAVE_DATE,LEAVE_TYPE_ID,NO_OF_DAYS,FROM_TIME,TO_TIME,COVERED_BY,RECOMMAND_BY,REMARKS,IS_HALFDAY,LEAVE_STATUS " +
                                      " FROM EMPLOYEE_LEAVE_SCHEDULE " +
                                      " WHERE LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_REJECTED + "'" +
                                      " AND LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_DISCARDED + "'" +
                                      " AND LEAVE_DATE='" + sDate.Trim() +
                                      "' AND EMPLOYEE_ID='" + employeeId.Trim() + "' and (ROSTR_ID is null)";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtLeaves);



                return dtLeaves;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable isLeaveExistForTheRoster(string employeeId, string sDate, string sRoster)
        {

            try
            {
                DataTable dtLeaves = new DataTable();
                string sMySqlString = " SELECT LEAVE_DATE,LEAVE_TYPE_ID,NO_OF_DAYS,FROM_TIME,TO_TIME,ROSTR_ID,COVERED_BY,RECOMMAND_BY,REMARKS,IS_HALFDAY,LEAVE_STATUS " +
                                      " FROM EMPLOYEE_LEAVE_SCHEDULE " +
                                      " WHERE LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_REJECTED + "'" +
                                      " AND LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_DISCARDED + "'" +
                                      " AND LEAVE_DATE='" + sDate.Trim() + "'" +
                                      " AND EMPLOYEE_ID='" + employeeId.Trim() + "' AND ROSTR_ID ='" + sRoster.Trim() + "'";

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dtLeaves);

                return dtLeaves;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getRosterWorkingTimeForRoster(string employeeId, string sDate, string sRosterId)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = " SELECT erd.ROSTR_ID,ros.FROM_TIME,ros.TO_TIME,ros.NUM_DAYS  " +
                                        " FROM EMPLOYEE_ROSTER_DATE as erd,ROSTER as ros " +
                                        " where erd.ROSTR_ID = ros.ROSTR_ID and erd.STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE +
                                        "' and erd.DUTY_DATE = '" + sDate.Trim() + "' and erd.EMPLOYEE_ID='" + employeeId.Trim() + "' and erd.ROSTR_ID='" + sRosterId.Trim() + "'";


                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);

                mySqlDa.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string getRosterType(string rosterId)
        {

            string rosterType = "";

            mySqlCmd.CommandText = "SELECT ROSTER_TYPE FROM ROSTER where ROSTR_ID = '" + rosterId.Trim() + "'";

            try
            {
                mySqlCon.Open();


                if (mySqlCmd.ExecuteScalar() != null)
                {
                    rosterType = mySqlCmd.ExecuteScalar().ToString();
                }

                mySqlCon.Close();

            }

            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();

                }
                throw ex;
            }

            return rosterType;
        }

    }
}
