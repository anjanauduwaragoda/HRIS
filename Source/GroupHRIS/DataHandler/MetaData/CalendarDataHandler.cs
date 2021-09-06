using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;

namespace DataHandler.MetaData
{
    public class CalendarDataHandler : TemplateDataHandler 
    {

        public DataTable populateHolidayTypes()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT concat(DATE_TYPE,'-', DESCRIPTION) as DATETYPE FROM CALENDAR_DATETYPE  WHERE  STATUS = '" + Constants.STATUS_ACTIVE_VALUE + "' ORDER BY DATE_TYPE";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateHolidays()
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DATE_TYPE,DESCRIPTION,CALCOLOR,  CASE " +
                                        " when STATUS='0' then '" + Constants.STATUS_INACTIVE_TAG + "' " +
                                        " when STATUS='1' then '" + Constants.STATUS_ACTIVE_TAG + "'  " +
                                        " End as STATUS FROM CALENDAR_DATETYPE ORDER BY DATE_TYPE";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string populateHolidays(string sDateType)
        {

            string sHolidaytype = "";
            mySqlCmd.CommandText = "SELECT DESCRIPTION FROM CALENDAR_DATETYPE  WHERE DATE_TYPE='" + sDateType.Trim().ToUpper() + "'";// and STATUS = '" + Constants.STATUS_ACTIVE_VALUE + "'";
            
            try
            {
                mySqlCon.Open();
                if (mySqlCmd.ExecuteScalar() != null)
                {
                    sHolidaytype = mySqlCmd.ExecuteScalar().ToString();
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
            return sHolidaytype;
        }

        public DataTable populateCompanyHolidays(string sComp_Code,string sYear)
        {
            try
            {
                string sCalendarDateType = Constants.CON_CALENDER_WROK_DAY_CODE;
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DATE_FORMAT(CALENDAR_DATE,'%Y-%m-%d') as CALENDAR_DATE,DESCRIPTION,CALCOLOR FROM COMPANY_CALENDAR,CALENDAR_DATETYPE" +
                                      " WHERE COMPANY_CALENDAR.DATE_TYPE = CALENDAR_DATETYPE.DATE_TYPE and COMPANY_ID ='" + sComp_Code + "' and DATE_FORMAT(CALENDAR_DATE,'%Y') = '" + sYear + "' and COMPANY_CALENDAR.DATE_TYPE <> '" +  sCalendarDateType + "'" +
                                       " and CALENDAR_DATETYPE.STATUS='" + Constants.CON_ACTIVE_STATUS + "' ORDER BY CALENDAR_DATE";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public string populateCompanyHolidaysForCalendar(string sComp_Code, string sCdate)
        //{
            
        //    string sHolidayColor = "";
        //    string sCalendarDateType = Constants.CON_CALENDER_WROK_DAY_CODE;
        //    mySqlCmd.CommandText =  "SELECT CALCOLOR FROM COMPANY_CALENDAR,CALENDAR_DATETYPE" +
        //                              " WHERE COMPANY_CALENDAR.DATE_TYPE = CALENDAR_DATETYPE.DATE_TYPE and COMPANY_ID ='" + sComp_Code + "' and CALENDAR_DATE = '" + sCdate + "' and COMPANY_CALENDAR.DATE_TYPE <> '" + sCalendarDateType + "' ORDER BY CALENDAR_DATE";
            
        //    try
        //    {
        //        mySqlCon.Open();
        //        if (mySqlCmd.ExecuteScalar() != null)
        //        {
        //            sHolidayColor = mySqlCmd.ExecuteScalar().ToString();
        //        }
        //        mySqlCon.Close();
        //    }

        //    catch (Exception ex)
        //    {
        //        if (mySqlCon.State == ConnectionState.Open)
        //        {
        //            mySqlCon.Close();
        //        }
        //        throw ex;
        //    }
        //    return sHolidayColor;
        //}


        public DataTable populateCompanyHolidaysForCalendar(string sComp_Code, string sYear)
        {

            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT CALENDAR_DATE as cCALENDAR_DATE,CALCOLOR FROM COMPANY_CALENDAR,CALENDAR_DATETYPE" +
                                      " WHERE COMPANY_CALENDAR.DATE_TYPE = CALENDAR_DATETYPE.DATE_TYPE and COMPANY_ID ='" + sComp_Code + "' and DATE_FORMAT(CALENDAR_DATE,'%Y') = '" + sYear + "'  and COMPANY_CALENDAR.DATE_TYPE <> '" + Constants.CON_CALENDER_WROK_DAY_CODE + "' ORDER BY CALENDAR_DATE";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable populateCompanyCalendar(string sComp_Code, string sYear)
        {
            try
            {
                dataTable.Rows.Clear();
                string sMySqlString = "SELECT DATE_FORMAT(CALENDAR_DATE,'%Y-%m-%d') as CALENDAR_DATE,DESCRIPTION FROM COMPANY_CALENDAR,CALENDAR_DATETYPE" +
                                      " WHERE COMPANY_CALENDAR.DATE_TYPE = CALENDAR_DATETYPE.DATE_TYPE and COMPANY_ID ='" + sComp_Code + "' and DATE_FORMAT(CALENDAR_DATE,'%Y') = '" + sYear + "' ORDER BY CALENDAR_DATE";
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(sMySqlString, mySqlCon);
                mySqlDa.Fill(dataTable);
                return dataTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataRow populateCompanyHolidayLeave(string sComp_Code, string sCaldate)
        {
            try
            {
                dataTable.Rows.Clear();
                DataRow dr = null;
                string sMySqlString = "SELECT COMPANY_ID,DATE_STATUS FROM COMPANY_CALENDAR,CALENDAR_DATETYPE" +
                                      " WHERE COMPANY_CALENDAR.DATE_TYPE = CALENDAR_DATETYPE.DATE_TYPE and COMPANY_ID ='" + sComp_Code + "' and CALENDAR_DATE = '" + sCaldate + "' and COMPANY_CALENDAR.DATE_TYPE = '" + Constants.CON_CALENDER_WROK_DAY_CODE + "' ORDER BY CALENDAR_DATE";
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

        public Boolean InsertCompanyCalendar(string sComp_Code, DateTime sFromdate , DateTime sTodate)
        {
            Boolean blInserted = false;

            string sMySqlString = "";
            DateTime sCalendardate  ;
            string sCalDate = "";
            string sCalendarDateType = Constants.CON_CALENDER_WROK_DAY_CODE;
            string strDate = "";
            string sCalendarDateStatus ="";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                TimeSpan totDays = sTodate - sFromdate;
                int datediff = totDays.Days;


                for (int idaycount = 0; idaycount <= datediff; idaycount++)
                {
                    sCalendardate = sFromdate.AddDays(idaycount);
                    sCalDate = sCalendardate.ToString("yyyy/MM/dd");
                    strDate = sCalendardate.DayOfWeek.ToString();
                    sCalendarDateStatus = strDate.Substring(0, 2).ToUpper();

                    mySqlCmd.Parameters.Add(new MySqlParameter("@sComp_Code", sComp_Code.Trim() == "" ? (object)DBNull.Value : sComp_Code.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sCalDate", sCalDate.Trim() == "" ? (object)DBNull.Value : sCalDate.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sCalendarDateType", sCalendarDateType.Trim() == "" ? (object)DBNull.Value : sCalendarDateType.Trim()));
                    mySqlCmd.Parameters.Add(new MySqlParameter("@sCalendarDateStatus", sCalendarDateStatus.Trim() == "" ? (object)DBNull.Value : sCalendarDateStatus.Trim()));


                    sMySqlString = "INSERT INTO COMPANY_CALENDAR(COMPANY_ID,CALENDAR_DATE,DATE_TYPE,DATE_STATUS) " +
                                   "VALUES(@sComp_Code,@sCalDate,@sCalendarDateType,@sCalendarDateStatus)";


                    mySqlCmd.Transaction = mySqlTrans;
                    mySqlCmd.CommandText = sMySqlString;

                    mySqlCmd.ExecuteNonQuery();
                    mySqlCmd.Parameters.Clear();
                }

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

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

            return blInserted;
        }

        public Boolean DeleteCompanyCalendar(string sComp_Code, DateTime sFromdate, DateTime sTodate)
        {
            Boolean blInserted = false;

            string sMySqlString = "";
            string sCalFromDate = sFromdate.ToString("yyyy/MM/dd");
            string sCalToDate = sTodate.ToString("yyyy/MM/dd");
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                TimeSpan totDays = sTodate - sFromdate;
                int datediff = totDays.Days;

                mySqlCmd.Parameters.Add(new MySqlParameter("@sComp_Code", sComp_Code.Trim() == "" ? (object)DBNull.Value : sComp_Code.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sCalfRoMDate", sCalFromDate.Trim() == "" ? (object)DBNull.Value : sCalFromDate.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sCalToDate", sCalToDate.Trim() == "" ? (object)DBNull.Value : sCalToDate.Trim()));

                sMySqlString = "DELETE FROM COMPANY_CALENDAR WHERE COMPANY_ID=@sComp_Code AND CALENDAR_DATE>=@sCalFromDate AND CALENDAR_DATE<=@sCalToDate";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();
                mySqlCmd.Parameters.Clear();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

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

            return blInserted;
        }


        public Boolean UpdateCompanyCalendar(string sComp_Code, string mHoliday, string mDatetype)
        {
            Boolean blInserted = false;

            string sMySqlString = "";
            MySqlTransaction mySqlTrans = null;

            try
            {
                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();


                mySqlCmd.Parameters.Add(new MySqlParameter("@sComp_Code", sComp_Code.Trim() == "" ? (object)DBNull.Value : sComp_Code.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@mHoliday", mHoliday.Trim() == "" ? (object)DBNull.Value : mHoliday.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@mDatetype", mDatetype.Trim() == "" ? (object)DBNull.Value : mDatetype.Trim()));

                sMySqlString = "UPDATE COMPANY_CALENDAR SET DATE_TYPE=@mDatetype WHERE COMPANY_ID=@sComp_Code AND CALENDAR_DATE=@mHoliday";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();
                mySqlCmd.Parameters.Clear();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

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

            return blInserted;
        }


        public Boolean InsertCompanyHoliday(string sDateType, string sHolidaytype, string scolor, string sStatus)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sDateType", sDateType.Trim().ToUpper() == "" ? (object)DBNull.Value : sDateType.Trim().ToUpper()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sHolidaytype", sHolidaytype.Trim() == "" ? (object)DBNull.Value : sHolidaytype.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@scolor", scolor.Trim() == "" ? (object)DBNull.Value : scolor.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));

                sMySqlString = "INSERT INTO CALENDAR_DATETYPE(DATE_TYPE,DESCRIPTION,CALCOLOR,STATUS) " +
                               "VALUES(@sDateType,@sHolidaytype,@scolor,@sStatus)";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

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

            return blInserted;
        }

        public Boolean UpdateCompanyHoliday(string sDateType, string sHolidaytype, string scolor, string sStatus)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                mySqlCon.Open();

                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sDateType", sDateType.Trim().ToUpper() == "" ? (object)DBNull.Value : sDateType.Trim().ToUpper()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sHolidaytype", sHolidaytype.Trim() == "" ? (object)DBNull.Value : sHolidaytype.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@scolor", scolor.Trim() == "" ? (object)DBNull.Value : scolor.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sStatus", sStatus.Trim() == "" ? (object)DBNull.Value : sStatus.Trim()));

                sMySqlString = "UPDATE CALENDAR_DATETYPE SET DESCRIPTION=@sHolidaytype,CALCOLOR=@scolor,STATUS=@sStatus WHERE DATE_TYPE=@sDateType";

                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;

                mySqlCmd.ExecuteNonQuery();

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

            return blInserted;
        }
    }
}
