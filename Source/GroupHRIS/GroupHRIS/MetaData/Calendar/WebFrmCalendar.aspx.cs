using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using Common;
using System.Data;
using DataHandler.MetaData;
using System.Drawing;
using GroupHRIS.Utility;

namespace GroupHRIS.MetaData.Calendar
{
    public partial class WebFrmCalendar : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static string Compny_Code = "";
        private static Boolean Is_DayRender = false;
        DataTable holiday = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            log.Debug("Page_Load");
            Session["paramCompny_Code"] = "";
            if (!IsPostBack)
            {
                Compny_Code = (string)(Session["KeyCOMP_ID"]);
                Session["paramCompny_Code"] = (string)(Session["KeyCOMP_ID"]);
                getCompID(Compny_Code);
                get_holidayTypes();
                get_CompanyHolidays();
                get_holidaysforcalendar();
            }
        }


        private void getCompID(string Compny_Code)
        {
            CompanyDataHandler company = new CompanyDataHandler();
            DataTable schCompID = new DataTable();

            try
            {
                if (Compny_Code == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    schCompID = company.getCompanyIdCompName();
                    ListItem listItemBlank = new ListItem();
                    listItemBlank.Text = "";
                    listItemBlank.Value = "";
                    ddlCompID.Items.Add(listItemBlank);
                }
                else
                {
                    ListItem listItemBlank = new ListItem(); 
                    listItemBlank.Text = "";
                    listItemBlank.Value = "";
                    ddlCompID.Items.Add(listItemBlank);
                    schCompID = company.getCompanyIdCompName(Compny_Code);
                }

                if (schCompID.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in schCompID.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlCompID.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                company = null;
                schCompID.Dispose();
                schCompID = null;
            }

        }

        private void get_CompanyHolidays()
        {
            string myear = DateTime.Today.Year.ToString();
            string Compny_Code = ddlCompID.SelectedValue.ToString();
            string mTable = "";
            string scalendarDate = "";
            string sdescription = "";
            string sdatetype = "";
            string scolor = "";
            ltlHolidays.Text = "";

            CalendarDataHandler calendarDataHandler = new CalendarDataHandler();
            DataTable CompanyHolidays = calendarDataHandler.populateCompanyHolidays(Compny_Code, myear);
            if (CompanyHolidays.Rows.Count > 0)
            {

                for (int x = 0; x <= CompanyHolidays.Rows.Count - 1; x++)
                {

                    if (x == 0)
                    {
                        mTable = "";
                        mTable = mTable + "<Table style='width:500px;border-width:thin;border-style:solid;border-color:darkblue'>";
                        mTable = mTable + "<Tr>";
                        mTable = mTable + "<Td style='width:150px;border-width:thin;border-style:solid;border-color:dodgerblue;background-color:Silver'>";
                        mTable = mTable + "DATE";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td style='width:150px;border-width:thin;border-style:solid;border-color:dodgerblue;background-color:Silver'>";
                        mTable = mTable + "DESCRIPTION";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td style='width:150px;border-width:thin;border-style:solid;border-color:dodgerblue;background-color:Silver'>";
                        mTable = mTable + "DAY OF WEEK";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td style='width:50px;border-width:thin;border-style:solid;border-color:dodgerblue;background-color:Silver'>";
                        mTable = mTable + "COLOR";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "</Tr>";
                    }

                    scalendarDate = CompanyHolidays.Rows[x]["CALENDAR_DATE"].ToString();
                    sdescription = CompanyHolidays.Rows[x]["DESCRIPTION"].ToString();
                    sdatetype = DateTime.Parse(scalendarDate).ToString("dddd");

                    Color col = ColorTranslator.FromHtml(String.Format("#{0}", CompanyHolidays.Rows[x]["CALCOLOR"].ToString()));
                    scolor = System.Drawing.ColorTranslator.ToHtml(col);

                    mTable = mTable + "<Tr>";
                    mTable = mTable + "<Td style='border-width:thin;border-style:solid;border-color:dodgerblue'>";
                    mTable = mTable + scalendarDate;
                    mTable = mTable + "</Td>";

                    mTable = mTable + "<Td style='border-width:thin;border-style:solid;border-color:dodgerblue'>";
                    mTable = mTable + sdescription;
                    mTable = mTable + "</Td>";

                    mTable = mTable + "<Td style='border-width:thin;border-style:solid;border-color:dodgerblue'>";
                    mTable = mTable + sdatetype;
                    mTable = mTable + "</Td>";

                    mTable = mTable + "<Td style='border-width:thin;border-style:solid;border-color:dodgerblue;background-color:" + scolor.ToString() + "'>";
                    mTable = mTable + "";
                    mTable = mTable + "</Td>";
                    mTable = mTable + "</Tr>";

                    if (x == CompanyHolidays.Rows.Count - 1)
                    {
                        mTable = mTable + "</Table>";
                        ltlHolidays.Text = ltlHolidays.Text + mTable;
                    }
                }
            }
        }
        private void get_holidayTypes()
        {
            CalendarDataHandler calendarDataHandler = new CalendarDataHandler();
            DataTable holidayTypes = calendarDataHandler.populateHolidayTypes();
            if (holidayTypes.Rows.Count > 0)
            {
                for (int x = 0; x < holidayTypes.Rows.Count; x++)
                {
                    ddlholidaytype.Items.Add(holidayTypes.Rows[x][0].ToString());
                }
            }

            calendarDataHandler = null;
            ddlholidaytype.Dispose();
            ddlholidaytype = null;

        }

        protected void btngeneratecalendar_Click(object sender, EventArgs e)
        {
            CalendarDataHandler calendarDataHandler = new CalendarDataHandler();

            try
            {
                
                if (Utility.Utils.verifyDate(txttodate.Text) == false)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Date format incorrect for To date (YYYY/MM/DD)", lblerror);
                    return;
                }

                if (Utility.Utils.verifyDate(txtfromdate.Text) == false)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Date format incorrect for From date(YYYY/MM/DD)", lblerror);
                    return;
                }

                string mCompcode = ddlCompID.SelectedValue.ToString();
                DateTime mToDate = Convert.ToDateTime(txttodate.Text.ToString().Trim());
                DateTime mFromDate = Convert.ToDateTime(txtfromdate.Text.ToString().Trim());

                double dateDiff = (mToDate - mFromDate).TotalDays;
                if(dateDiff < 0)
                {
                    CommonVariables.MESSAGE_TEXT = "Invalied date range.(From time > To time)";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    return;
                }

                Boolean blDeleted = calendarDataHandler.DeleteCompanyCalendar(mCompcode, mFromDate, mToDate);
                Boolean blInserted = calendarDataHandler.InsertCompanyCalendar(mCompcode, mFromDate, mToDate);
                if (blInserted == true)
                {
                    CommonVariables.MESSAGE_TEXT = "Calendar Successfully Updated from " + mFromDate.ToString("yyyy/MM/dd") + " to " + mToDate.ToString("yyyy/MM/dd");
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    get_CompanyHolidays();
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                calendarDataHandler = null;
            }
        }

        protected void btnholiday_Click(object sender, EventArgs e)
        {
            string mCompcode = ddlCompID.SelectedValue.ToString();
            string mHoliday = lblday.Text.ToString();
            string mdatetype = ddlholidaytype.SelectedItem.Text.ToString().Substring(0, 1);

            CalendarDataHandler calendarDataHandler = new CalendarDataHandler();

            if (Utility.Utils.verifyDate(lblday.Text) == false)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Date format incorrect.(YYYY/MM/DD)", lblmsg);
                return;
            }

            try
            {

                if (mHoliday.Equals(""))
                {
                   CommonVariables.MESSAGE_TEXT = "Holiday Day is required.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {
                    Boolean blUpdated = calendarDataHandler.UpdateCompanyCalendar(mCompcode, mHoliday, mdatetype);
                    if (blUpdated == true)
                    {
                        CommonVariables.MESSAGE_TEXT = mHoliday.ToString() + " Updated as " + ddlholidaytype.SelectedItem.Text.ToString().Substring(1);
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        get_CompanyHolidays();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }

        }

        protected void ddlCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtfromdate.Text = "";
            txttodate.Text = "";
            get_holidaysforcalendar();
            Utility.Errorhandler.ClearError(lblerror);
            Session["paramCompny_Code"] = ddlCompID.SelectedValue.ToString();
            get_CompanyHolidays();
            Is_DayRender = true;

        }

        private void get_holidaysforcalendar()
        {
            CalendarDataHandler calendarDataHandler = new CalendarDataHandler();

            try
            {
                string myear = DateTime.Today.Year.ToString();
                string Compny_Code = ddlCompID.SelectedValue.ToString();
                holiday = calendarDataHandler.populateCompanyHolidaysForCalendar(Compny_Code, myear).Copy();

            }
            catch (Exception ex )
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                calendarDataHandler = null;
            }

        }

        protected void calcompany_DayRender(object sender, DayRenderEventArgs e)
        {
            
            string sHtmlColor = "";

            try
            {
                string sDate = e.Day.Date.Date.ToString("yyyy/MM/dd");
                if (Is_DayRender == true )
                {
                    if (holiday != null)
                    {
                        DataRow[] result = holiday.Select("cCALENDAR_DATE = '" + sDate + "' ");
                        foreach (DataRow row in result)
                        {
                            string scolor = row["CALCOLOR"].ToString();
                            Color col = ColorTranslator.FromHtml(String.Format("#{0}", scolor));
                            sHtmlColor = System.Drawing.ColorTranslator.ToHtml(col);
                            e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml(sHtmlColor);
                            e.Cell.BorderColor = System.Drawing.Color.Black;
                            e.Cell.BorderWidth = 1;

                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void calcompany_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            get_holidaysforcalendar();
            Is_DayRender = true ;
        }


    }
}