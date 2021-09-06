using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using System.Data;
using System.Text;
using Common;
using DataHandler.Userlogin;
using DataHandler.Attendance;
using DataHandler.Employee;

namespace GroupHRIS.Attendance
{
    public partial class AttendanceInOutApproval : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sapprovedBy = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "AttendanceInOutApproval : Page_Load");

            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {
                sapprovedBy = Session["KeyEMPLOYEE_ID"].ToString();
            }

            if (!IsPostBack)
            {
                loadInoutForApproval(sapprovedBy);
            }
        }

        private void loadInoutForApproval(string approvedBy)
        {
            log.Debug("AttendanceInOutApproval : loadInoutForApproval()");

            DataTable employeeinout = new DataTable();
            AttendanceDataHandler attendanceDataHandler = new AttendanceDataHandler();
            DataTable DtAttendance = new DataTable();
            DateTime MfromDate = DateTime.Today.AddDays(Constants.CON_ATTENDANCE_VIEW_PERIOD);

            try
            {
                employeeinout = attendanceDataHandler.populateAttendanceOfficer(approvedBy, MfromDate).Copy();
                gvLeaves.DataSource = employeeinout;
                gvLeaves.DataBind();

            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
            finally
            {
                attendanceDataHandler = null;
                employeeinout.Dispose();
                employeeinout = null;
            }

        }

        protected void gvAttendance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            log.Debug("AttendanceInOutApproval : gvAttendance_RowCommand()");

            AttendanceDataHandler attendanceDataHandler = new AttendanceDataHandler();
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            try
            {
                Int32 index = Convert.ToInt32(e.CommandArgument);

                GridViewRow selectedRow = gvLeaves.Rows[index];

                string sEmpcode = selectedRow.Cells[0].Text.ToString().Trim();
                string sEmpName = selectedRow.Cells[1].Text.ToString().Trim();
                string sCompID = selectedRow.Cells[6].Text.ToString().Trim();
                string sAttDate = selectedRow.Cells[2].Text.ToString().Trim();
                string sAttLocation = selectedRow.Cells[8].Text.ToString().Trim();
                string sAttTime = selectedRow.Cells[3].Text.ToString().Trim();
                string sReasonCode = selectedRow.Cells[7].Text.ToString().Trim();
                string sDirection = selectedRow.Cells[9].Text.ToString().Trim();
                string sINOUT = selectedRow.Cells[5].Text.ToString().Trim();

                string mailAddress = employeeDataHandler.getEmployeeEmail(sEmpcode);

                if (e.CommandName.ToString().Equals("Approve"))
                {
                    Boolean isupdated = attendanceDataHandler.UpdateAttendanceLog(sEmpcode, sCompID, sDirection, sAttDate, sAttLocation, sAttTime, sReasonCode, Constants.STATUS_ACTIVE_VALUE);
                    if (mailAddress.Trim() != "")
                    {
                        EmailHandler.SendDefaultEmail(Constants.CON_SYSTEM_NAME, mailAddress,"", "IN/OUT approval", getApprovedMessage(sEmpName, sINOUT, sAttDate, sAttTime));
                    }
                    loadInoutForApproval(sapprovedBy);
                }
                else if (e.CommandName.ToString().Equals("Reject"))
                {
                    Boolean isupdated = attendanceDataHandler.UpdateAttendanceLog(sEmpcode, sCompID, sDirection, sAttDate, sAttLocation, sAttTime, sReasonCode, Constants.STATUS_OBSOLETE_VALUE);
                    if (mailAddress.Trim() != "")
                    {
                        EmailHandler.SendDefaultEmail(Constants.CON_SYSTEM_NAME, mailAddress, "", "IN/OUT Rejection", getRejectedMessage(sEmpName, sINOUT, sAttDate, sAttTime));
                    }
                    loadInoutForApproval(sapprovedBy);
                }
            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
            finally
            {
                attendanceDataHandler = null;
                employeeDataHandler = null;
            }
        }

        private StringBuilder getApprovedMessage(string sEmpName, string sINOUT, string sAttDate, string sAttTime)
        {
            log.Debug("AttendanceInOutApproval : getApprovedMessage()");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear " + sEmpName + "," + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Your Clock " + sINOUT + " time " + sAttTime + " on " + sAttDate + " has been approved " + Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }

        private StringBuilder getRejectedMessage(string sEmpName, string sINOUT, string sAttDate, string sAttTime)
        {
            log.Debug("AttendanceInOutApproval : getRejectedMessage()");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear " + sEmpName + "," + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Your Clock " + sINOUT + " time " + sAttTime + " on " + sAttDate + " has been rejected " + Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }
    }
}