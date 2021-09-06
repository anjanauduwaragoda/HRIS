using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.EmployeeLeave;
using DataHandler.Roster;
using DataHandler.MetaData;
using System.Data;
using Common;
using System.Drawing;

namespace GroupHRIS.EmployeeProfile
{
    public partial class LeaveManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                string KeyCOMP_ID = (string)(Session["KeyCOMP_ID"]);
                get_LeaveDetails(KeyEMPLOYEE_ID);
            }
        }

        private void get_LeaveDetails(string empId)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            string sYear = DateTime.Today.ToString("yyyy-MM-dd").Substring(0, 4);
            DataTable employeeLeave = null;
            Random random = new Random();

            try
            {
                employeeLeave = leaveScheduleDataHandler.getEmployeeLeveSummaryChart(empId, sYear);
                if (employeeLeave != null)
                {

                    int icount = employeeLeave.Rows.Count;
                    string mleaveType = "";
                    double mleaveNo = 0;
                    double mleaveTaken = 0;
                    double mleaveBalance = 0;

                    for (int i = 0; i <= icount - 1; i++)
                    {
                        mleaveType = employeeLeave.Rows[i][0].ToString();
                        mleaveNo = double.Parse(employeeLeave.Rows[i][1].ToString());
                        mleaveTaken = double.Parse(employeeLeave.Rows[i][2].ToString());
                        mleaveBalance = mleaveNo - mleaveTaken;
                        chrtLeaveBalance.Series["noofleave"].Points.AddXY(mleaveType, mleaveBalance);
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
                leaveScheduleDataHandler = null;
                employeeLeave.Dispose();
                employeeLeave = null;

            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox thecheckbox = (CheckBox)e.Row.FindControl("chkmark");
                DropDownList theDropDownList = (DropDownList)e.Row.FindControl("ddlscore");
                theDropDownList.Items.Add("Full Day");
                theDropDownList.Items.Add("Half Day");
                theDropDownList.Items.Add("Short Leave");
                theDropDownList.Visible = true;

                foreach (TableCell cell in e.Row.Cells)
                {
                    string CompHoliday = e.Row.Cells[3].Text.ToString();

                    if (CompHoliday == Constants.CON_CALENDER_NON_WROK_DAY)
                    {
                        cell.BackColor = Color.LightSkyBlue;
                        theDropDownList.Enabled = false;
                        thecheckbox.Enabled = false;
                    }
                }
            }

        }

        protected void btngeneratecalendar_Click(object sender, EventArgs e)
        {

            EmpRosterAssignmentDataHandler empRosterAssignmentDataHandler = new EmpRosterAssignmentDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            CalendarDataHandler calendarDataHandler = new CalendarDataHandler();

            DataRow rosterLeave = null;
            DataRow companyLeave = null;
            DataRow companyHoliday = null;
            DataTable TBLeave = new DataTable("TbLeave");
            DateTime mFromDate = Convert.ToDateTime(txtfromdate.Text.ToString());
            DateTime mToDate = Convert.ToDateTime(txttodate.Text.ToString());
            DateTime mNextDate = new DateTime();
            string Companyinout = "";
            string Rosterinout = "";
            string CompHoliday = "";
            double ndays = 0;

            TimeSpan ts = mToDate - mFromDate;
            ndays = ts.TotalDays;

            try
            {

                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                string KeyCOMP_ID = (string)(Session["KeyCOMP_ID"]);

                companyLeave = companyDataHandler.getCompanyINOUT(KeyCOMP_ID);
                if (companyLeave != null)
                {
                    Companyinout = companyLeave["WORK_HOURS_START"].ToString() + " - " + companyLeave["WORK_HOURS_END"].ToString();
                }
                else
                {
                    Companyinout = "00.00" + " - " + "00.00";
                }

                TBLeave.Columns.Add("LeaveDate");
                TBLeave.Columns.Add("Fromto");
                TBLeave.Columns.Add("Dayofweek");
                TBLeave.Columns.Add("Isholiday");


                for (int i = 0; i <= ndays; i++)
                {
                    mNextDate = mFromDate.AddDays(i);
                    DataRow dr = TBLeave.NewRow();
                    Rosterinout = "";

                    rosterLeave = empRosterAssignmentDataHandler.populateRosterDate(KeyEMPLOYEE_ID, mNextDate.ToString("yyyy-MM-dd"), Constants.STATUS_ACTIVE_VALUE);
                    if (rosterLeave != null)
                    {
                        Rosterinout = rosterLeave["FROM_TIME"].ToString() + " - " + rosterLeave["TO_TIME"].ToString();
                    }

                    if (Rosterinout != "")
                    {
                        dr["Fromto"] = Rosterinout;
                    }
                    else
                    {
                        companyHoliday = calendarDataHandler.populateCompanyHolidayLeave(KeyCOMP_ID, mNextDate.ToString("yyyy-MM-dd"));
                        if (companyHoliday != null)
                        {
                            CompHoliday = Constants.CON_CALENDER_WROK_DAY;
                        }
                        else
                        {
                            CompHoliday = Constants.CON_CALENDER_NON_WROK_DAY;
                        }

                        dr["Fromto"] = Companyinout;
                    }

                    dr["LeaveDate"] = mNextDate.ToString("yyyy-MM-dd");
                    dr["Dayofweek"] = mNextDate.ToString("dddd");
                    dr["Isholiday"] = CompHoliday;
                    
                    TBLeave.Rows.Add(dr);

                }

                GridView2.DataSource = TBLeave;
                GridView2.DataBind();

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                empRosterAssignmentDataHandler = null;
                companyDataHandler = null;
                calendarDataHandler = null;
                TBLeave.Dispose();
                TBLeave = null;
                rosterLeave = null;
                companyLeave = null;
                companyHoliday = null;
            }
        }

    }
}