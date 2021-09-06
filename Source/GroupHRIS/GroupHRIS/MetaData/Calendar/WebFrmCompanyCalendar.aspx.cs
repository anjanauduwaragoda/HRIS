using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.MetaData;
using System.Data;

namespace GroupHRIS.MetaData.Calendar
{
    public partial class WebFrmCompanyCalendar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack ){
                string myear = DateTime.Today.Year.ToString();
                string Compny_Code = (string)(Session["paramCompny_Code"]);
                display_calendar(Compny_Code, myear);
            }
        }

        private void display_calendar(string Compny_Code,string myear)
        {

            CalendarDataHandler calendarDataHandler = new CalendarDataHandler();
            
            DataTable sCalender = new DataTable();
            string mTable = "";
            string scalendarDate = "";
            string sdescription = "";
            string sdatetype = "";
            LiteralSE1.Text = "";

            sCalender = calendarDataHandler.populateCompanyCalendar(Compny_Code, myear);

            try
            {
                for (int x = 0; x <= sCalender.Rows.Count - 1; x++)
                {

                    if (x == 0)
                    {
                        mTable = "";
                        mTable = mTable + "<Table style='width:800px;border-width:thin;border-style:solid;border-color:darkblue'>";
                        mTable = mTable + "<Tr>";
                        mTable = mTable + "<Td style='border-width:thin;border-style:solid;border-color:dodgerblue;background-color:Silver'>";
                        mTable = mTable + "CALENDAR DATE";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td style='border-width:thin;border-style:solid;border-color:dodgerblue;background-color:Silver'>";
                        mTable = mTable + "DESCRIPTION";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td style='border-width:thin;border-style:solid;border-color:dodgerblue;background-color:Silver'>";
                        mTable = mTable + "DAY OF WEEK";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "</Tr>";
                    }

                    scalendarDate = sCalender.Rows[x]["CALENDAR_DATE"].ToString();
                    sdescription = sCalender.Rows[x]["DESCRIPTION"].ToString();
                    sdatetype = DateTime.Parse(scalendarDate).ToString("dddd");

                    mTable = mTable + "<Tr>";
                    mTable = mTable + "<Td style='width:100px;border-width:thin;border-style:solid;border-color:dodgerblue'>";
                    mTable = mTable + scalendarDate;
                    mTable = mTable + "</Td>";

                    mTable = mTable + "<Td style='width:100px;border-width:thin;border-style:solid;border-color:dodgerblue'>";
                    mTable = mTable + sdescription;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td style='width:100px;border-width:thin;border-style:solid;border-color:dodgerblue'>";
                    mTable = mTable + sdatetype;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "</Tr>";

                    if (x == sCalender.Rows.Count - 1)
                    {
                        mTable = mTable + "</Table>";
                        LiteralSE1.Text = LiteralSE1.Text + mTable;
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
                calendarDataHandler = null;
                sCalender.Dispose();
                sCalender = null;
            }

        }
    }
}