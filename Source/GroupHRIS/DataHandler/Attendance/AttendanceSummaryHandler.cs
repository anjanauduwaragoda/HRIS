using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;
using System.Globalization;
using NLog;

namespace DataHandler.Attendance
{
    public class AttendanceSummaryHandler : TemplateDataHandler
    {
        private static DateTime? mRosterTimeIN;
        private static DateTime? mRosterimeOUT;
        private static DateTime mOverNight = DateTime.Parse("00:00:00");
        private static Logger log = LogManager.GetCurrentClassLogger();

        public int getAttendanceCount(DateTime ATT_DATE)
        {
            log.Debug("getAbsentListCount ->");
            int sTotCount = 0;

            mySqlCmd.CommandText = "SELECT  Count(EMPLOYEE_ID) as TotCount from ATTENDANCE WHERE ATT_DATE = '" + ATT_DATE.ToString("yyyy-MM-dd") + "'";
            mySqlCon.Open();

            if (mySqlCmd.ExecuteScalar() != null)
            {
                sTotCount = int.Parse(mySqlCmd.ExecuteScalar().ToString());
            }

            mySqlCon.Close();

            return sTotCount;
        }

        public DataTable SummaryAbsetList(DateTime sIN_DATE)
        {
            log.Debug("sendemailtoabset -> " + "send email to abset " + sIN_DATE);

            try
            {
                string sMySqlString = "";
                dataTable = new DataTable();

                sMySqlString = "SELECT IN_DATE as IN_DATE,known_name as known_name,initials_name as initials_name,email as email FROM ATTENDANCE_SUMMARY a ,EMPLOYEE e " +
                     "WHERE a.EMPLOYEE_ID = e.EMPLOYEE_ID " +
                     "AND IN_DATE >= '" + Convert.ToDateTime(sIN_DATE).ToString("yyyy/MM/dd") + "' AND IN_DATE <= '" + Convert.ToDateTime(sIN_DATE).ToString("yyyy/MM/dd") + "' " +
                     "AND IS_ABSENT ='" + Constants.STATUS_ACTIVE_VALUE + "' " +
                     "AND NUMBER_OF_DAYS is null " +
                     "AND EXCLUDE_EMAIL = '" + Constants.STATUS_INACTIVE_VALUE + "' " +
                     "AND substring(REMARK,1,1) = '" + Constants.CON_CALENDER_WROK_DAY_CODE + "' " +
                     "AND email is not null order by IN_DATE,a.EMPLOYEE_ID ";
                MySqlDataAdapter mySqlDaRoster = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDaRoster.Fill(dataTable);
            }
            catch (Exception)
            {

                throw;
            }

            return dataTable; ;
        }


        public Boolean updateSummaryLog(DateTime mfromDate, string mdescrip, string mEmpid)
        {
            log.Debug("updateSummaryLog -> ");

            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@mfromDate", mfromDate == null ? (object)DBNull.Value : mfromDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("@mdescrip", mdescrip.Trim() == "" ? (object)DBNull.Value : mdescrip.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@mEmpid", mEmpid.Trim() == "" ? (object)DBNull.Value : mEmpid.Trim()));

                sMySqlString = "INSERT INTO ATTENDANCE_SUMMARY_LOG(SummaryDate,Remarks,Employeeid) " +
                                "VALUES(@mfromDate,@mdescrip,@mEmpid)";
                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();
                mySqlCmd.Parameters.Clear();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                mySqlCmd.Parameters.Clear();

                blInserted = true;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                mySqlTrans.Rollback();
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
            return blInserted;
        }


        public string getCultryDate()
        {
            log.Debug("getCultryDate ->");
            string sCultryDateTime = "";

            mySqlCmd.CommandText = "SELECT  NOW() from dual";
            mySqlCon.Open();

            if (mySqlCmd.ExecuteScalar() != null)
            {
                sCultryDateTime = mySqlCmd.ExecuteScalar().ToString();
            }

            mySqlCon.Close();

            return sCultryDateTime;
        }

        public string populateCompanyHoliday(string sDateType)
        {
            log.Debug("populateCompanyHoliday -> " + sDateType);

            string sCompanyHoliday = "";
            mySqlCmd.CommandText = "SELECT DESCRIPTION from CALENDAR_DATETYPE where DATE_TYPE = '" + sDateType + "'";

            try
            {

                mySqlCon.Open();

                if (mySqlCmd.ExecuteScalar() != null)
                {
                    sCompanyHoliday = mySqlCmd.ExecuteScalar().ToString();
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
            return sCompanyHoliday;
        }

        public DataTable populateEmpProfile(string sFrom, string sTo, string sEMPLOYEE_ID)
        {
            log.Debug("populateEmpProfile -> " + "Employee: " + sEMPLOYEE_ID + "Date : " + sFrom);


            try
            {
                string sMySqlString = "";
                dataTable = new DataTable();

                sMySqlString = "SELECT IN_DATE,IN_TIME,OUT_DATE,OUT_TIME,LATE_MINUTES,EARLY_MINUTES,NUMBER_OF_DAYS,IN_LOCATION,OUT_LOCATION,REMARK,OT_HOURS,IS_ABSENT " +
                                " FROM ATTENDANCE_SUMMARY " +
                                " where IN_DATE >= '" + sFrom.Trim() + "' and IN_DATE <= '" + sTo.Trim() + "' and EMPLOYEE_ID = '" + sEMPLOYEE_ID.Trim() + "' order by IN_DATE ";
                MySqlDataAdapter mySqlDaRoster = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDaRoster.Fill(dataTable);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataTable;
        }

        public DataTable populateAttEmployee(string sEmployee_ID, string sNextDate)
        {
            log.Debug("populateAttEmployee -> " + "Employee: " + sEmployee_ID + "Date : " + sNextDate);

            try
            {
                string sMySqlString = "";
                dataTable = new DataTable();

                string sStatus_Code = Constants.STATUS_ACTIVE_VALUE;

                sMySqlString = " select e.company_id as COMPANY_ID,r.from_time as WORK_HOURS_START,r.to_time as WORK_HOURS_END ,erd.ROSTR_ID as ROSTR_ID,r.ROSTER_TYPE as ROSTER_TYPE ,'" + Constants.CON_IS_ROSTER + "' as ISROSTER,e.DEPT_ID as DEPT_ID,e.DIVISION_ID as DIVISION_ID" +
                                      " from EMPLOYEE_ROSTER_DATE erd , ROSTER r , EMPLOYEE e " +
                                      " where r.ROSTR_ID = erd.ROSTR_ID " +
                                      " and erd.EMPLOYEE_ID = e.EMPLOYEE_ID " +
                                      " and erd.DUTY_DATE = '" + sNextDate + "' " +
                                      " and erd.EMPLOYEE_ID = '" + sEmployee_ID + "' and e.STATUS_CODE ='" + sStatus_Code + "' order by  r.from_time asc ";

                MySqlDataAdapter mySqlDaRoster = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDaRoster.Fill(dataTable);
                mySqlDaRoster.Dispose();
                mySqlDaRoster = null;

                if (dataTable.Rows.Count == 0)
                {
                    dataTable = new DataTable();
                    sMySqlString = " select e.COMPANY_ID as COMPANY_ID,c.WORK_HOURS_START as WORK_HOURS_START,c.WORK_HOURS_END as WORK_HOURS_END,'" + Constants.CON_NO_ROSTER_ID + "' as ROSTR_ID,cc.DATE_TYPE as ROSTER_TYPE, '" + Constants.CON_IS_NORMAL + "' as ISROSTER,e.DEPT_ID as DEPT_ID,e.DIVISION_ID as DIVISION_ID" +
                                       " from EMPLOYEE e , COMPANY c , COMPANY_CALENDAR cc " +
                                       " where e.COMPANY_ID = c.COMPANY_ID  " +
                                       " and c.COMPANY_ID = cc.COMPANY_ID  " +
                                       " and cc.CALENDAR_DATE = '" + sNextDate + "' " +
                                       " and e.EMPLOYEE_ID = '" + sEmployee_ID + "' and e.STATUS_CODE ='" + sStatus_Code + "'";

                    MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                    mySqlDa.Fill(dataTable);
                }

                mySqlCmd.Parameters.Clear();

                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow populateAttDateINCaseEarly(string sCALENDAR_DATE, string sEMPLOYEE_ID, string sIsroster, DateTime sAttDEF_TIMEIN, DateTime sAttDEF_TIMEOUT, int sRosterNo, int iRecoredCount, string sRosterType)
        {
            log.Debug("populateAttDateINCaseEarly -> " + "Employee: " + sEMPLOYEE_ID + "Date : " + sCALENDAR_DATE);


            string sMySqlString = "";
            MySqlDataAdapter mySqlDa = null;
            DataRow dataRow = null;

            try
            {
                dataTable = new DataTable();
                sMySqlString = "SELECT max(atn.ATT_TIME) as ATT_TIME,atn.BRANCH_ID  as BRANCH_ID from ATTENDANCE atn " +
                                " where atn.ATT_DATE = '" + sCALENDAR_DATE + "' and atn.EMPLOYEE_ID = '" + sEMPLOYEE_ID + "' and atn.DIRECTION = '" + Constants.CON_ATTENDANCE_IN.ToString() + "'  " +
                                " and STR_TO_DATE(atn.ATT_TIME,'%T') <= '" + sAttDEF_TIMEIN.ToString("HH:mm:ss") + "'  group by atn.EMPLOYEE_ID,atn.ATT_DATE order by ATT_TIME asc";
                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if ((dataTable.Rows.Count > 0) && (dataTable.Rows[0]["ATT_TIME"] != DBNull.Value))
                {
                    dataRow = dataTable.Rows[0];
                    if (mRosterTimeIN == DateTime.Parse(dataRow["ATT_TIME"].ToString()))
                    {
                        dataRow = null;
                    }
                    else
                    {
                        dataRow = dataTable.Rows[0];
                        mRosterTimeIN = DateTime.Parse(dataRow["ATT_TIME"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataRow;

        }

        public DataRow populateAttDateINCaseLate(string sCALENDAR_DATE, string sEMPLOYEE_ID, string sIsroster, DateTime sAttDEF_TIMEIN, DateTime sAttDEF_TIMEOUT, int sRosterNo, int iRecoredCount, string sRosterType)
        {
            log.Debug("populateAttDateINCaseLate -> " + "Employee: " + sEMPLOYEE_ID + "Date : " + sCALENDAR_DATE);

            string sMySqlString = "";
            MySqlDataAdapter mySqlDa = null;
            DataRow dataRow = null;

            try
            {

                if (sRosterType == Constants.CON_ROSTER_OVER_NIGHT_CODE)
                {
                    dataTable = new DataTable();
                    sMySqlString = "SELECT min(atn.ATT_TIME) as ATT_TIME,atn.BRANCH_ID  as BRANCH_ID from ATTENDANCE atn " +
                                " where atn.ATT_DATE = '" + sCALENDAR_DATE + "' and atn.EMPLOYEE_ID = '" + sEMPLOYEE_ID + "' and atn.DIRECTION = '" + Constants.CON_ATTENDANCE_IN.ToString() + "'  " +
                               " and STR_TO_DATE(atn.ATT_TIME,'%T') > '" + sAttDEF_TIMEIN.ToString("HH:mm:ss") + "' group by atn.EMPLOYEE_ID,atn.ATT_DATE order by ATT_TIME asc";
                    mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                    mySqlDa.Fill(dataTable);
                    if ((dataTable.Rows.Count > 0) && (dataTable.Rows[0]["ATT_TIME"] != DBNull.Value))
                    {
                        dataRow = dataTable.Rows[0];
                        if (mRosterTimeIN == DateTime.Parse(dataRow["ATT_TIME"].ToString()))
                        {
                            dataRow = null;
                        }
                        else
                        {
                            dataRow = dataTable.Rows[0];
                            mRosterTimeIN = DateTime.Parse(dataRow["ATT_TIME"].ToString());
                        }
                    }
                }
                else
                {
                    dataTable = new DataTable();
                    sMySqlString = "SELECT min(atn.ATT_TIME) as ATT_TIME,atn.BRANCH_ID  as BRANCH_ID from ATTENDANCE atn " +
                                " where atn.ATT_DATE = '" + sCALENDAR_DATE + "' and atn.EMPLOYEE_ID = '" + sEMPLOYEE_ID + "' and atn.DIRECTION = '" + Constants.CON_ATTENDANCE_IN.ToString() + "'  " +
                               " and STR_TO_DATE(atn.ATT_TIME,'%T') > '" + sAttDEF_TIMEIN.ToString("HH:mm:ss") + "' and STR_TO_DATE(atn.ATT_TIME,'%T') <= '" + sAttDEF_TIMEOUT.ToString("HH:mm:ss") + "' group by atn.EMPLOYEE_ID,atn.ATT_DATE order by ATT_TIME asc";
                    mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                    mySqlDa.Fill(dataTable);
                    if ((dataTable.Rows.Count > 0) && (dataTable.Rows[0]["ATT_TIME"] != DBNull.Value))
                    {
                        dataRow = dataTable.Rows[0];
                        if (mRosterTimeIN == DateTime.Parse(dataRow["ATT_TIME"].ToString()))
                        {
                            dataRow = null;
                        }
                        else
                        {
                            dataRow = dataTable.Rows[0];
                            mRosterTimeIN = DateTime.Parse(dataRow["ATT_TIME"].ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataRow;

        }

        public DataRow populateAttDateOUTCaseEarly(string sCALENDAR_DATE, string sEMPLOYEE_ID, string sIsroster, DateTime sAttDEF_TIMEIN, DateTime sAttDEF_TIMEOUT, int sRosterNo, int iRecoredCount, string sRosterType)
        {
            log.Debug("populateAttDateOUTCaseEarly -> " + "Employee: " + sEMPLOYEE_ID + "Date : " + sCALENDAR_DATE);

            string sMySqlString = "";
            MySqlDataAdapter mySqlDa = null;
            DataRow dataRow = null;

            try
            {
                dataTable = new DataTable();

                if (sRosterType == Constants.CON_ROSTER_OVER_NIGHT_CODE)
                {
                    sMySqlString = "SELECT max(atn.ATT_TIME) as ATT_TIME,atn.BRANCH_ID   as BRANCH_ID from ATTENDANCE atn " +
                                            " where atn.ATT_DATE = '" + sCALENDAR_DATE + "' and atn.EMPLOYEE_ID = '" + sEMPLOYEE_ID + "' and atn.DIRECTION = '" + Constants.CON_ATTENDANCE_OUT.ToString() + "'  " +
                                            " and  STR_TO_DATE(atn.ATT_TIME,'%T') < '" + sAttDEF_TIMEOUT.ToString("HH:mm:ss") + "'  group by atn.EMPLOYEE_ID,atn.ATT_DATE  order by ATT_TIME asc";
                    mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                    mySqlDa.Fill(dataTable);
                    if ((dataTable.Rows.Count > 0) && (dataTable.Rows[0]["ATT_TIME"] != DBNull.Value))
                    {
                        dataRow = dataTable.Rows[0];
                        if (mRosterimeOUT == DateTime.Parse(dataRow["ATT_TIME"].ToString()))
                        {
                            dataTable.Rows.Clear();
                            //Get previous Day
                            DateTime mPreviWday = DateTime.Parse(sCALENDAR_DATE.ToString());
                            mPreviWday = mPreviWday.AddDays(-1);

                            sMySqlString = "SELECT max(atn.ATT_TIME) as ATT_TIME,atn.BRANCH_ID   as BRANCH_ID from ATTENDANCE atn " +
                                            " where atn.ATT_DATE = '" + mPreviWday.ToString("yyyy/MM/dd") + "' and atn.EMPLOYEE_ID = '" + sEMPLOYEE_ID + "' and atn.DIRECTION = '" + Constants.CON_ATTENDANCE_OUT.ToString() + "'  " +
                                            " and  STR_TO_DATE(atn.ATT_TIME,'%T') > '" + sAttDEF_TIMEIN.ToString("HH:mm:ss") + "'  group by atn.EMPLOYEE_ID,atn.ATT_DATE  order by ATT_TIME asc";
                            mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                            mySqlDa.Fill(dataTable);
                            if ((dataTable.Rows.Count > 0) && (dataTable.Rows[0]["ATT_TIME"] != DBNull.Value))
                            {
                                dataRow = dataTable.Rows[0];
                                if (mRosterimeOUT == DateTime.Parse(dataRow["ATT_TIME"].ToString()))
                                {
                                    dataRow = null;
                                }
                                else
                                {
                                    dataRow = dataTable.Rows[0];
                                    mRosterimeOUT = DateTime.Parse(dataRow["ATT_TIME"].ToString());
                                }
                            }

                        }
                        else
                        {
                            dataRow = dataTable.Rows[0];
                            mRosterimeOUT = DateTime.Parse(dataRow["ATT_TIME"].ToString());
                        }
                    }
                }
                else
                {
                    sMySqlString = "SELECT max(atn.ATT_TIME) as ATT_TIME,atn.BRANCH_ID   as BRANCH_ID from ATTENDANCE atn " +
                                            " where atn.ATT_DATE = '" + sCALENDAR_DATE + "' and atn.EMPLOYEE_ID = '" + sEMPLOYEE_ID + "' and atn.DIRECTION = '" + Constants.CON_ATTENDANCE_OUT.ToString() + "'  " +
                                            " and STR_TO_DATE(atn.ATT_TIME,'%T') > '" + sAttDEF_TIMEIN.ToString("HH:mm:ss") + "' and STR_TO_DATE(atn.ATT_TIME,'%T') <= '" + sAttDEF_TIMEOUT.ToString("HH:mm:ss") + "' group by atn.EMPLOYEE_ID,atn.ATT_DATE  order by ATT_TIME asc";
                    mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                    mySqlDa.Fill(dataTable);
                    if ((dataTable.Rows.Count > 0) && (dataTable.Rows[0]["ATT_TIME"] != DBNull.Value))
                    {
                        dataRow = dataTable.Rows[0];
                        if (mRosterimeOUT == DateTime.Parse(dataRow["ATT_TIME"].ToString()))
                        {
                            dataRow = null;
                        }
                        else
                        {
                            dataRow = dataTable.Rows[0];
                            mRosterimeOUT = DateTime.Parse(dataRow["ATT_TIME"].ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataRow;

        }

        public DataRow populateAttDateOUTCaseLate(string sCALENDAR_DATE, string sEMPLOYEE_ID, string sIsroster, DateTime sAttDEF_TIMEIN, DateTime sAttDEF_TIMEOUT, int sRosterNo, int iRecoredCount, string sRosterType)
        {
            log.Debug("populateAttDateOUTCaseLate -> " + "Employee: " + sEMPLOYEE_ID + "Date : " + sCALENDAR_DATE);

            string sMySqlString = "";
            MySqlDataAdapter mySqlDa = null;
            DataRow dataRow = null;

            try
            {
                dataTable = new DataTable();
                sMySqlString = "SELECT min(atn.ATT_TIME) as ATT_TIME,atn.BRANCH_ID  as BRANCH_ID from ATTENDANCE atn " +
                                    " where atn.ATT_DATE = '" + sCALENDAR_DATE + "' and atn.EMPLOYEE_ID = '" + sEMPLOYEE_ID + "' and atn.DIRECTION = '" + Constants.CON_ATTENDANCE_OUT.ToString() + "'  " +
                                    " and STR_TO_DATE(atn.ATT_TIME,'%T') > '" + sAttDEF_TIMEOUT.ToString("HH:mm:ss") + "' group by atn.EMPLOYEE_ID,atn.ATT_DATE order by ATT_TIME asc";
                mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);

                if ((dataTable.Rows.Count > 0) && (dataTable.Rows[0]["ATT_TIME"] != DBNull.Value))
                {
                    dataRow = dataTable.Rows[0];
                    if (mRosterimeOUT == DateTime.Parse(dataRow["ATT_TIME"].ToString()))
                    {
                        dataRow = null;
                    }
                    else
                    {
                        dataRow = dataTable.Rows[0];
                        mRosterimeOUT = DateTime.Parse(dataRow["ATT_TIME"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataRow;

        }

        public DataRow populateAttDateIN(string sCALENDAR_DATE, string sEMPLOYEE_ID, string sIsroster, DateTime sAttDEF_TIMEIN, DateTime sAttDEF_TIMEOUT, int sRosterNo, int iRecoredCount, string sRosterType)
        {
            log.Debug("populateAttDateIN -> " + "Employee: " + sEMPLOYEE_ID + "Date : " + sCALENDAR_DATE);

            string sMySqlString = "";
            MySqlDataAdapter mySqlDa = null;
            DataRow dataRow = null;

            try
            {
                dataTable = new DataTable();

                if (sIsroster == Constants.CON_IS_NORMAL)
                {
                    sMySqlString = "SELECT ATT_TIME,BRANCH_ID  from ATTENDANCE where ATT_DATE = '" + sCALENDAR_DATE + "' and EMPLOYEE_ID = '" + sEMPLOYEE_ID + "' order by ATT_TIME asc limit 1";
                    mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                    mySqlDa.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        dataRow = dataTable.Rows[0];
                    }

                }
                else
                {
                    dataRow = populateAttDateINCaseEarly(sCALENDAR_DATE, sEMPLOYEE_ID, sIsroster, sAttDEF_TIMEIN, sAttDEF_TIMEOUT, sRosterNo, iRecoredCount, sRosterType);
                    if (dataRow == null)
                    {
                        dataRow = populateAttDateINCaseLate(sCALENDAR_DATE, sEMPLOYEE_ID, sIsroster, sAttDEF_TIMEIN, sAttDEF_TIMEOUT, sRosterNo, iRecoredCount, sRosterType);
                    }

                }

                mySqlCmd.Parameters.Clear();

                return dataRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow populateAttDateOUT(string sCALENDAR_DATE, string sEMPLOYEE_ID, string sIsroster, DateTime sAttDEF_TIMEIN, DateTime sAttDEF_TIMEOUT, int sRosterNo, int iRecoredCount, string sRosterType)
        {
            log.Debug("populateAttDateOUT -> " + "Employee: " + sEMPLOYEE_ID + "Date : " + sCALENDAR_DATE);

            string sMySqlString = "";
            MySqlDataAdapter mySqlDa = null;
            DataRow dataRow = null;

            try
            {
                dataTable = new DataTable();

                if (sIsroster == Constants.CON_IS_NORMAL)
                {
                    sMySqlString = "SELECT ATT_TIME,BRANCH_ID  from ATTENDANCE where ATT_DATE = '" + sCALENDAR_DATE + "' and EMPLOYEE_ID = '" + sEMPLOYEE_ID + "' order by ATT_TIME desc limit 1";
                    mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                    mySqlDa.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        dataRow = dataTable.Rows[0];
                    }

                }
                else
                {
                    dataRow = populateAttDateOUTCaseEarly(sCALENDAR_DATE, sEMPLOYEE_ID, sIsroster, sAttDEF_TIMEIN, sAttDEF_TIMEOUT, sRosterNo, iRecoredCount, sRosterType);
                    if (dataRow == null)
                    {
                        dataRow = populateAttDateOUTCaseLate(sCALENDAR_DATE, sEMPLOYEE_ID, sIsroster, sAttDEF_TIMEIN, sAttDEF_TIMEOUT, sRosterNo, iRecoredCount, sRosterType);
                    }

                }

                mySqlCmd.Parameters.Clear();

                return dataRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String populateAttLateMinEarlymin(DateTime sATT_TIME, DateTime sDEF_TIME)
        {
            log.Debug("populateAttLateMinEarlymin -> ");

            try
            {
                TimeSpan ts = sATT_TIME.Subtract(sDEF_TIME);
                string sTimeDiff = ts.TotalMinutes.ToString();
                return sTimeDiff;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String populateAbsetStatus(string sIsroster, string sRosterType, string sIN_TIME, string sOUT_TIME)
        {
            log.Debug("populateAbsetStatus -> ");

            string sISABSENT = Constants.STATUS_INACTIVE_VALUE;

            try
            {

                if (sIsroster == Constants.CON_IS_NORMAL && sRosterType == Constants.CON_CALENDER_WROK_DAY_CODE)
                {
                    if (sIN_TIME == "" && sOUT_TIME == "")
                    {
                        sISABSENT = Constants.STATUS_ACTIVE_VALUE;
                    }
                    else if (sIN_TIME == "" && sOUT_TIME != "")
                    {
                        sISABSENT = Constants.STATUS_MISSING_VALUE;
                    }
                    else if (sIN_TIME != "" && sOUT_TIME == "")
                    {
                        sISABSENT = Constants.STATUS_MISSING_VALUE;
                    }
                    else
                    {
                        sISABSENT = Constants.STATUS_INACTIVE_VALUE;
                    }
                }
                else if (sIsroster == Constants.CON_IS_ROSTER)
                {
                    if (sIN_TIME == "" && sOUT_TIME == "")
                    {
                        sISABSENT = Constants.STATUS_ACTIVE_VALUE;
                    }
                    else if (sIN_TIME == "" && sOUT_TIME != "")
                    {
                        sISABSENT = Constants.STATUS_MISSING_VALUE;
                    }
                    else if (sIN_TIME != "" && sOUT_TIME == "")
                    {
                        sISABSENT = Constants.STATUS_MISSING_VALUE;
                    }
                    else
                    {
                        sISABSENT = Constants.STATUS_INACTIVE_VALUE;
                    }
                }

                return sISABSENT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string populateAttLeaveDays(string sEMPLOYEE_ID, string sCALENDAR_DATE)
        {
            log.Debug("populateAttLeaveDays -> " + "Employee: " + sEMPLOYEE_ID + "Date : " + sCALENDAR_DATE);

            string mLeave = "0";
            mySqlCmd.CommandText = "SELECT sum(NO_OF_DAYS ) as NO_OF_DAYS  from EMPLOYEE_LEAVE_SCHEDULE  where EMPLOYEE_ID = '" + sEMPLOYEE_ID + "'  and Leave_DATE  = '" + sCALENDAR_DATE + "' and (LEAVE_STATUS <>  '" + Constants.LEAVE_STATUS_REJECTED + "'  and LEAVE_STATUS <> '" + Constants.LEAVE_STATUS_DISCARDED + "' )";

            try
            {
                mySqlCon.Open();
                if (mySqlCmd.ExecuteScalar() != null)
                {
                    mLeave = mySqlCmd.ExecuteScalar().ToString();
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
            return mLeave;
        }

        public string populateLocation(string sCOMPANY_ID, string sBRANCH_CODE)
        {
            log.Debug("AttendanceSummaryHandler : " + "populateLocation -> " + "Company: " + sCOMPANY_ID + "BRANCH_CODE : " + sBRANCH_CODE);

            string mLocation = "";
            mySqlCmd.CommandText = "SELECT BRANCH_NAME FROM COMPANY_BRANCH  where COMPANY_ID = '" + sCOMPANY_ID + "' and BRANCH_CODE = '" + sBRANCH_CODE + "'";

            try
            {
                mySqlCon.Open();
                if (mySqlCmd.ExecuteScalar() != null)
                {
                    mLocation = mySqlCmd.ExecuteScalar().ToString();
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
            return mLocation;
        }

        public Boolean InsertAttSummary(DataTable mAttenSummaryTB, string sADDED_BY, string ADDED_DATE)
        {
            log.Debug("InsertAttSummary -> ");

            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                for (int x = 0; x < mAttenSummaryTB.Rows.Count; x++)
                {

                    string sCompanyCode = mAttenSummaryTB.Rows[x][0].ToString();
                    string sEMPLOYEE_ID = mAttenSummaryTB.Rows[x][1].ToString();
                    string sCALANDER_DATE = mAttenSummaryTB.Rows[x][2].ToString();
                    string sIN_DATE = mAttenSummaryTB.Rows[x][3].ToString();
                    string sIN_TIME = mAttenSummaryTB.Rows[x][4].ToString();
                    string sOUT_DATE = mAttenSummaryTB.Rows[x][5].ToString();
                    string sOUT_TIME = mAttenSummaryTB.Rows[x][6].ToString();
                    string sLATE_MINUTES = mAttenSummaryTB.Rows[x][7].ToString();
                    string sEARLY_MINUTES = mAttenSummaryTB.Rows[x][8].ToString();
                    string sNUMBER_OF_DAYS = mAttenSummaryTB.Rows[x][9].ToString();
                    string sIN_LOCATION = mAttenSummaryTB.Rows[x][10].ToString();
                    string sOUT_LOCATION = mAttenSummaryTB.Rows[x][11].ToString();
                    string sREMARK = mAttenSummaryTB.Rows[x][12].ToString();
                    string sOT = mAttenSummaryTB.Rows[x][13].ToString();
                    string sISROSTRE = mAttenSummaryTB.Rows[x][14].ToString();
                    string sDEPT_ID = mAttenSummaryTB.Rows[x][15].ToString();
                    string sDIVISION_ID = mAttenSummaryTB.Rows[x][16].ToString();
                    string sISABSENT = mAttenSummaryTB.Rows[x][17].ToString();
                    string sRosterID = mAttenSummaryTB.Rows[x][18].ToString();
                    string sM_OT_HOURS = mAttenSummaryTB.Rows[x][19].ToString();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@sCompanyCode", sCompanyCode.Trim() == "" ? (object)DBNull.Value : sCompanyCode.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sEMPLOYEE_ID", sEMPLOYEE_ID.Trim() == "" ? (object)DBNull.Value : sEMPLOYEE_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sCALANDER_DATE", sCALANDER_DATE.Trim() == "" ? (object)DBNull.Value : sCALANDER_DATE.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sIN_DATE", sIN_DATE.Trim() == "" ? (object)DBNull.Value : sIN_DATE.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sIN_TIME", sIN_TIME.Trim() == "" ? (object)DBNull.Value : sIN_TIME.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sOUT_DATE", sOUT_DATE.Trim() == "" ? (object)DBNull.Value : sOUT_DATE.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sOUT_TIME", sOUT_TIME.Trim() == "" ? (object)DBNull.Value : sOUT_TIME.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sLATE_MINUTES", sLATE_MINUTES.Trim() == "" ? (object)DBNull.Value : sLATE_MINUTES.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sEARLY_MINUTES", sEARLY_MINUTES.Trim() == "" ? (object)DBNull.Value : sEARLY_MINUTES.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sNUMBER_OF_DAYS", sNUMBER_OF_DAYS.Trim() == "" ? (object)DBNull.Value : sNUMBER_OF_DAYS.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sIN_LOCATION", sIN_LOCATION.Trim() == "" ? (object)DBNull.Value : sIN_LOCATION.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sOUT_LOCATION", sOUT_LOCATION.Trim() == "" ? (object)DBNull.Value : sOUT_LOCATION.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sREMARK", sREMARK.Trim() == "" ? (object)DBNull.Value : sREMARK.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sOT", sOT.Trim() == "" ? (object)DBNull.Value : sOT.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sDEPT_ID", sDEPT_ID.Trim() == "" ? (object)DBNull.Value : sDEPT_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sDIVISION_ID", sDIVISION_ID.Trim() == "" ? (object)DBNull.Value : sDIVISION_ID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sISABSENT", sISABSENT.Trim() == "" ? (object)DBNull.Value : sISABSENT.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sRosterID", sRosterID.Trim() == "" ? (object)DBNull.Value : sRosterID.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sM_OT_HOURS", sM_OT_HOURS.Trim() == "" ? (object)DBNull.Value : sM_OT_HOURS.Trim()));

                    sMySqlString = "DELETE FROM ATTENDANCE_SUMMARY WHERE EMPLOYEE_ID = '" + sEMPLOYEE_ID + "' AND IN_DATE = '" + Convert.ToDateTime(sIN_DATE).ToString("yyyy/MM/dd") + "'";
                    mySqlCmd.Transaction = mySqlTrans;
                    mySqlCmd.CommandText = sMySqlString;
                    mySqlCmd.ExecuteNonQuery();

                    if (sISROSTRE == Constants.CON_IS_ROSTER)
                    {
                        sMySqlString = "UPDATE EMPLOYEE_ROSTER_DATE SET IS_SUMMARIZED='" + Constants.CON_SUMMARIZED + "' WHERE EMPLOYEE_ID = '" + sEMPLOYEE_ID + "' AND DUTY_DATE = '" + Convert.ToDateTime(sIN_DATE).ToString("yyyy/MM/dd") + "' and STATUS_CODE = '" + Constants.STATUS_ACTIVE_VALUE + "' ";
                        mySqlCmd.Transaction = mySqlTrans;
                        mySqlCmd.CommandText = sMySqlString;
                        mySqlCmd.ExecuteNonQuery();
                    }

                    // update EMPLOYEE_LEAVE_SCHEDULE table as IS_SUMMARIZED true.

                    sMySqlString = "UPDATE EMPLOYEE_LEAVE_SCHEDULE SET IS_SUMMARIZED='" + Constants.CON_SUMMARIZED + "' WHERE EMPLOYEE_ID = '" + sEMPLOYEE_ID + "' AND LEAVE_DATE = '" + Convert.ToDateTime(sIN_DATE).ToString("yyyy/MM/dd") + "' and IS_SUMMARIZED='" + Constants.CON_NOT_SUMMARIZED + "' ";
                    mySqlCmd.Transaction = mySqlTrans;
                    mySqlCmd.CommandText = sMySqlString;
                    mySqlCmd.ExecuteNonQuery();

                    sMySqlString = "INSERT INTO ATTENDANCE_SUMMARY(COMPANY_ID,EMPLOYEE_ID,IN_DATE,IN_TIME,OUT_DATE,OUT_TIME,LATE_MINUTES,EARLY_MINUTES,NUMBER_OF_DAYS,IN_LOCATION,OUT_LOCATION,REMARK,OT_HOURS,DEPT_ID,DIVISION_ID,IS_ABSENT,ROSTER_ID,M_OT_HOURS) " +
                                   "VALUES(@sCompanyCode,@sEMPLOYEE_ID,@sIN_DATE,@sIN_TIME,@sOUT_DATE,@sOUT_TIME,@sLATE_MINUTES,@sEARLY_MINUTES,@sNUMBER_OF_DAYS,@sIN_LOCATION,@sOUT_LOCATION,@sREMARK,@sOT,@sDEPT_ID,@sDIVISION_ID,@sISABSENT,@sRosterID,@sM_OT_HOURS)";
                    mySqlCmd.Transaction = mySqlTrans;
                    mySqlCmd.CommandText = sMySqlString;
                    mySqlCmd.ExecuteNonQuery();
                    mySqlCmd.Parameters.Clear();

                }

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                mySqlCmd.Parameters.Clear();

                blInserted = true;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                mySqlTrans.Rollback();
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
            return blInserted;
        }

        public DataTable populateEmployeesINCompnay(string sCompCode)
        {
            log.Debug("populateEmployeesINCompnay -> CompCode: " + sCompCode);

            try
            {
                string sMySqlString = "";
                dataTable = new DataTable();

                if (sCompCode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    sMySqlString = " select EMPLOYEE_ID as EMPLOYEE_ID from  EMPLOYEE  " +
                                     " WHERE STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' and EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "'  order by EMPLOYEE_ID ";
                }
                else
                {
                    sMySqlString = " select EMPLOYEE_ID as EMPLOYEE_ID from  EMPLOYEE " +
                                     " WHERE COMPANY_ID = '" + sCompCode + "' and STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' and EMPLOYEE_STATUS = '" + Constants.CON_EMPLOYEE_STATUS_ACTIVE + "'  order by EMPLOYEE_ID ";
                }

                MySqlDataAdapter mySqlDaRoster = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDaRoster.Fill(dataTable);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataTable;
        }

        // Back Date leave : not summerized -> not summerized data populate

        public DataTable populateNotSummerizedLeave(string sStatusCode)
        {
            log.Debug("AttendanceSummaryHandler : " + "populateNotSummerizedLeave -> " + "sStatusCode: " + sStatusCode);

            try
            {
                string sMySqlString = "";
                dataTable = new DataTable();

                sMySqlString = "SELECT EMPLOYEE_ID,LEAVE_DATE " +
                                " FROM EMPLOYEE_LEAVE_SCHEDULE  " +
                                " where IS_SUMMARIZED='" + sStatusCode.Trim() + "' order by EMPLOYEE_ID,LEAVE_DATE ";
                MySqlDataAdapter mySqlDaRoster = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDaRoster.Fill(dataTable);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataTable;
        }


        // Back Date leave : not summerized -> Execute Backdated Leave

        public Boolean execute_BackdateLeave()
        {

            log.Debug("AttendanceSummaryHandler : " + "execute_BackdateLeave -> ");
            Boolean bsummerzied = false;
            string sNotSummaryEmpid = "";
            DateTime mFromDate;
            DateTime mToDate;
            DataTable NotSummerizedLeaveTB = new DataTable();

            NotSummerizedLeaveTB = populateNotSummerizedLeave(Constants.CON_NOT_SUMMARIZED).Copy();

            try
            {

                for (int sumTB = 0; sumTB < NotSummerizedLeaveTB.Rows.Count; sumTB++)
                {
                    sNotSummaryEmpid = NotSummerizedLeaveTB.Rows[sumTB]["EMPLOYEE_ID"].ToString();
                    mFromDate = DateTime.Parse(NotSummerizedLeaveTB.Rows[sumTB]["LEAVE_DATE"].ToString());
                    mToDate = DateTime.Parse(NotSummerizedLeaveTB.Rows[sumTB]["LEAVE_DATE"].ToString());

                    Boolean isEmpUpdated = execute_summary_employee(sNotSummaryEmpid, mFromDate, mToDate);
                    if (!isEmpUpdated == true)
                    {
                        bsummerzied = false;
                        log.Error("execute_BackdateLeave -> Employee : " + sNotSummaryEmpid + " Leave Date: " + mFromDate);
                    }
                    else
                    {
                        bsummerzied = true;
                    }
                }

                return bsummerzied;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                NotSummerizedLeaveTB.Dispose();
                NotSummerizedLeaveTB = null;
            }

        }

        public DataTable populateInOUTapprovals(string sStatusCode)
        {
            log.Debug("AttendanceSummaryHandler : " + "populateInOUTapprovals -> " + "sStatusCode: " + sStatusCode);


            try
            {
                string sMySqlString = "";
                dataTable = new DataTable();

                sMySqlString = "SELECT EMPLOYEE_ID,ATT_DATE " +
                                " FROM ATTENDANCE " +
                                " where STATUS_CODE = '" + sStatusCode.Trim() + "' order by EMPLOYEE_ID,ATT_DATE ";
                MySqlDataAdapter mySqlDaRoster = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDaRoster.Fill(dataTable);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataTable;
        }



        public Boolean updateAttendanceInOUTapprovals(string mfromDate, string sStatusCode, string mEmpid)
        {
            log.Debug("AttendanceSummaryHandler : " + "updateAttendanceInOUTapprovals -> ");

            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@mfromDate", mfromDate == null ? (object)DBNull.Value : mfromDate));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatusCode", sStatusCode.Trim() == "" ? (object)DBNull.Value : sStatusCode.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@mEmpid", mEmpid.Trim() == "" ? (object)DBNull.Value : mEmpid.Trim()));

                sMySqlString = "UPDATE ATTENDANCE SET STATUS_CODE='' WHERE ATT_DATE=@mfromDate AND EMPLOYEE_ID=@mEmpid AND STATUS_CODE=@sStatusCode";
                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();
                mySqlCmd.Parameters.Clear();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();
                mySqlCmd.Parameters.Clear();

                blInserted = true;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                mySqlTrans.Rollback();
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
            return blInserted;
        }

        public Boolean execute_InOUTapprovals()
        {

            log.Debug("AttendanceSummaryHandler : " + "execute_InOUTapprovals -> ");
            Boolean bsummerzied = false;
            string sSummaryEmpid = "";
            DateTime mFromDate;
            DateTime mToDate;
            DataTable ApprovalSummaryTB = new DataTable();
            ApprovalSummaryTB = populateInOUTapprovals(Constants.STATUS_ACTIVE_VALUE).Copy();

            try
            {

                for (int sumTB = 0; sumTB < ApprovalSummaryTB.Rows.Count; sumTB++)
                {
                    sSummaryEmpid = ApprovalSummaryTB.Rows[sumTB]["EMPLOYEE_ID"].ToString();
                    mFromDate = DateTime.Parse(ApprovalSummaryTB.Rows[sumTB]["ATT_DATE"].ToString());
                    mToDate = DateTime.Parse(ApprovalSummaryTB.Rows[sumTB]["ATT_DATE"].ToString());

                    Boolean isEmpUpdated = execute_summary_employee(sSummaryEmpid, mFromDate, mToDate);
                    if (!isEmpUpdated == true)
                    {
                        log.Error("execute_InOUTapprovals -> Employee : " + sSummaryEmpid + " From Date: " + mFromDate + " To Date: " + mToDate);
                    }
                    else
                    {
                        Boolean isAttenUpdate = updateAttendanceInOUTapprovals(mFromDate.ToString("yyyy-MM-dd"), Constants.STATUS_ACTIVE_VALUE, sSummaryEmpid);
                        if (isAttenUpdate == true)
                        {
                            bsummerzied = true;
                        }
                        else
                        {
                            log.Error("updateAttendanceInOUTapprovals -> Employee : " + sSummaryEmpid + " From Date: " + mFromDate + " To Date: " + mToDate);
                        }
                    }
                }

                return bsummerzied;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ApprovalSummaryTB.Dispose();
                ApprovalSummaryTB = null;
            }

        }

        public Boolean execute_company(string sCompCode, DateTime mFromDate, DateTime mToDate)
        {
            log.Debug("execute_company -> CompCode: " + sCompCode + " From Date: " + mFromDate + " To Date: " + mToDate);
            Boolean bsummerzied = false;
            string sSummaryEmpid = "";
            DataTable CompanySummaryTB = new DataTable();
            CompanySummaryTB = populateEmployeesINCompnay(sCompCode).Copy();

            try
            {

                for (int sumTB = 0; sumTB < CompanySummaryTB.Rows.Count; sumTB++)
                {
                    sSummaryEmpid = CompanySummaryTB.Rows[sumTB]["EMPLOYEE_ID"].ToString();
                    Boolean isEmpUpdated = execute_summary_employee(sSummaryEmpid, mFromDate, mToDate);
                    if (!isEmpUpdated == true)
                    {
                        log.Error("execute_summary_employee -> Employee : " + sSummaryEmpid + " From Date: " + mFromDate + " To Date: " + mToDate);
                    }
                }

                return bsummerzied;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CompanySummaryTB.Dispose();
                CompanySummaryTB = null;
            }

        }


        public Boolean execute_summary_employee(string sEmployee_ID, DateTime mFromDate, DateTime mToDate)
        {

            Boolean bsummerzied = false;
            DateTime mNextDate = new DateTime();
            DataTable AttEmployee = new DataTable();
            DataRow AttDateIN = null;
            DataRow AttDateOUT = null;

            string madddate = DateTime.Today.ToString("yyyy/MM/dd");
            string mlogUser = "";

            DateTime sAttDEF_TIMEIN;
            DateTime sAttDEF_TIMEOUT;

            double ndays = 0;
            double nleavedays = 0;
            int iRecoredCount = 0;

            DataTable SummaryTable = new DataTable("AttnSummary");
            SummaryTable.Columns.Add("sCOMPANY_ID");
            SummaryTable.Columns.Add("sEMPLOYEE_ID");
            SummaryTable.Columns.Add("sCALANDER_DATE");
            SummaryTable.Columns.Add("sIN_DATE");
            SummaryTable.Columns.Add("sIN_TIME");
            SummaryTable.Columns.Add("sOUT_DATE");
            SummaryTable.Columns.Add("sOUT_TIME");
            SummaryTable.Columns.Add("sLATE_MINUTES");
            SummaryTable.Columns.Add("sEARLY_MINUTES");
            SummaryTable.Columns.Add("sNUMBER_OF_DAYS");
            SummaryTable.Columns.Add("sIN_LOCATION");
            SummaryTable.Columns.Add("sOUT_LOCATION");
            SummaryTable.Columns.Add("sREMARK");
            SummaryTable.Columns.Add("sOT");
            SummaryTable.Columns.Add("sISROSTER");
            SummaryTable.Columns.Add("sDEPT_ID");
            SummaryTable.Columns.Add("sDIVISION_ID");
            SummaryTable.Columns.Add("sISABSENT");
            SummaryTable.Columns.Add("sROSTERID");
            SummaryTable.Columns.Add("M_OT_HOURS");

            try
            {

                string sCompanyCode = "";
                string sCALANDER_DATE = "";
                string sIN_DATE = "";
                string sIN_TIME = "";
                string sIN_LOCATION = "";
                string sOUT_DATE = "";
                string sOUT_TIME = "";
                string sOUT_LOCATION = "";
                string sAttLateMin = "";
                string sAttEarlyMin = "";
                string sAttLeaveDays = "";
                string sRosterType = "";
                string sIsroster = "";
                string sRemark = "";
                string sOT = "";
                string sDEPT_ID = "";
                string sDIVISION_ID = "";
                string sISABSENT = "";
                string sRoster_ID = "";
                string sM_OT_HOURS = "";

                TimeSpan ts = mToDate - mFromDate;
                ndays = ts.TotalDays;

                for (int i = 0; i <= ndays; i++)
                {
                    mNextDate = mFromDate.AddDays(i);
                    AttEmployee = populateAttEmployee(sEmployee_ID, mNextDate.ToString("yyyy/MM/dd")).Copy();
                    iRecoredCount = AttEmployee.Rows.Count;
                    mRosterTimeIN = null;
                    mRosterimeOUT = null;

                    for (int aRow = 0; aRow < iRecoredCount; aRow++)
                    {
                        sCALANDER_DATE = mNextDate.ToString("yyyy/MM/dd");
                        sIN_DATE = mNextDate.ToString("yyyy/MM/dd");
                        sCompanyCode = AttEmployee.Rows[aRow]["COMPANY_ID"].ToString();
                        sAttDEF_TIMEIN = Convert.ToDateTime(AttEmployee.Rows[aRow]["WORK_HOURS_START"].ToString());
                        sAttDEF_TIMEOUT = Convert.ToDateTime(AttEmployee.Rows[aRow]["WORK_HOURS_END"].ToString());
                        sRosterType = AttEmployee.Rows[aRow]["ROSTER_TYPE"].ToString();
                        sIsroster = AttEmployee.Rows[aRow]["ISROSTER"].ToString();
                        sRoster_ID = AttEmployee.Rows[aRow]["ROSTR_ID"].ToString();
                        sDEPT_ID = AttEmployee.Rows[aRow]["DEPT_ID"].ToString();
                        sDIVISION_ID = AttEmployee.Rows[aRow]["DIVISION_ID"].ToString();

                        // get datetype for non roster employee holiday, working days etc as remark.
                        if (sIsroster == Constants.CON_IS_NORMAL)
                        {
                            sRemark = populateCompanyHoliday(sRosterType);
                        }

                        if (sRosterType == Constants.CON_ROSTER_OVER_NIGHT_CODE)
                        {
                            // for over night roster
                            sOUT_DATE = mNextDate.AddDays(1).ToString("yyyy/MM/dd");
                        }
                        else
                        {
                            // for day roster
                            sOUT_DATE = mNextDate.ToString("yyyy/MM/dd");
                        }

                        // get in Time 
                        AttDateIN = populateAttDateIN(sCALANDER_DATE, sEmployee_ID, sIsroster, sAttDEF_TIMEIN, sAttDEF_TIMEOUT, aRow, iRecoredCount, sRosterType);
                        if (AttDateIN != null)
                        {
                            sIN_TIME = AttDateIN["ATT_TIME"].ToString();
                            sIN_LOCATION = populateLocation(sCompanyCode, AttDateIN["BRANCH_ID"].ToString());

                        }

                        // Get out Time, Over night outdate = mNextDate + 1  and normal outdate = mNextDate
                        AttDateOUT = populateAttDateOUT(sOUT_DATE, sEmployee_ID, sIsroster, sAttDEF_TIMEIN, sAttDEF_TIMEOUT, aRow, iRecoredCount, sRosterType);
                        if (AttDateOUT != null)
                        {
                            sOUT_TIME = AttDateOUT["ATT_TIME"].ToString();
                            sOUT_LOCATION = populateLocation(sCompanyCode, AttDateOUT["BRANCH_ID"].ToString());
                        }


                        if (sIN_TIME == sOUT_TIME)
                        {
                            //sIN_TIME = "";
                            sOUT_TIME = "";
                        }

                        // get late minutes
                        if (sIN_TIME != "")
                        {
                            if (Convert.ToDateTime(sIN_TIME.ToString()) > sAttDEF_TIMEIN)
                            {
                                sAttLateMin = populateAttLateMinEarlymin(Convert.ToDateTime(sIN_TIME.ToString()), sAttDEF_TIMEIN);
                            }
                            else
                            {
                                sM_OT_HOURS = populateAttLateMinEarlymin(sAttDEF_TIMEIN, Convert.ToDateTime(sIN_TIME.ToString()));
                            }
                        }

                        // get early leave minutes and OT Minutes
                        if (sOUT_TIME != "")
                        {
                            if (sAttDEF_TIMEOUT > Convert.ToDateTime(sOUT_TIME.ToString()))
                            {
                                sAttEarlyMin = populateAttLateMinEarlymin(sAttDEF_TIMEOUT, Convert.ToDateTime(sOUT_TIME.ToString()));
                            }
                            else
                            {
                                sOT = populateAttLateMinEarlymin(Convert.ToDateTime(sOUT_TIME.ToString()), sAttDEF_TIMEOUT);
                                sAttEarlyMin = "";
                            }
                        }

                        // get leave status
                        sAttLeaveDays = populateAttLeaveDays(sEmployee_ID, sCALANDER_DATE);

                        //set Absent status
                        if (sAttLeaveDays == "")
                        {
                            sAttLeaveDays = "0";
                        }

                        nleavedays = double.Parse(sAttLeaveDays);

                        if (nleavedays < 1)
                        {
                            sISABSENT = populateAbsetStatus(sIsroster, sRosterType, sIN_TIME, sOUT_TIME);
                        }
                        else
                        {
                            sISABSENT = Constants.STATUS_INACTIVE_VALUE;
                        }

                        // update to temporary table
                        SummaryTable.Rows.Add(sCompanyCode, sEmployee_ID, sCALANDER_DATE, sIN_DATE, sIN_TIME, sOUT_DATE, sOUT_TIME, sAttLateMin, sAttEarlyMin, sAttLeaveDays, sIN_LOCATION, sOUT_LOCATION, sRemark, sOT, sIsroster, sDEPT_ID, sDIVISION_ID, sISABSENT, sRoster_ID, sM_OT_HOURS);
                        sCALANDER_DATE = "";
                        sIN_DATE = "";
                        sIN_TIME = "";
                        sIN_LOCATION = "";
                        sOUT_DATE = "";
                        sOUT_TIME = "";
                        sOUT_LOCATION = "";
                        sAttLateMin = "";
                        sAttEarlyMin = "";
                        sAttLeaveDays = "";
                        sRosterType = "";
                        sIsroster = "";
                        sRemark = "";
                        sOT = "";
                        sDEPT_ID = "";
                        sDIVISION_ID = "";
                        sISABSENT = "";
                        sRoster_ID = "";
                        sM_OT_HOURS = "";
                    }

                }

                // update attendance summary table
                Boolean blInserted = InsertAttSummary(SummaryTable, mlogUser, madddate);
                if (blInserted == true)
                {
                    bsummerzied = true;
                }
                else
                {
                    bsummerzied = false;
                }

            }
            catch (Exception ex)
            {
                log.Debug("execute_Summary -> sEmployee_ID: " + sEmployee_ID + " From Date: " + mFromDate + " To Date: " + mToDate);
                bsummerzied = false;
                throw ex;
            }
            finally
            {
                AttEmployee.Dispose();
                AttEmployee = null;
                SummaryTable.Dispose();
                SummaryTable = null;
                AttDateIN = null;
                AttDateOUT = null;
            }

            return bsummerzied;
        }
    }
}
