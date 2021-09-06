using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Diagnostics;
using DataHandler.EmployeeLeave;
using DataHandler.MetaData;
using DataHandler.Employee;
using DataHandler.Userlogin;
using DomainConstraints;
//using HrisMail;
using Common;
using NLog;

namespace GroupHRIS.EmployeeLeave
{
    public partial class webFrmLeaveApproval : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "webFrmLeaveApproval : Page_Load");

            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {
                sUserId = Session["KeyUSER_ID"].ToString();
            }

            if (!IsPostBack)
            {
                loadLeaveForApproval();
            }
        }

        private void loadLeaveForApproval()
        {
            log.Debug("webFrmLeaveApproval : loadLeaveForApproval()");

            string approvedBy = Session["KeyEMPLOYEE_ID"].ToString();
            DataTable employeeLeave = new DataTable();
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

            try
            {
                employeeLeave = leaveScheduleDataHandler.getLeavesToApprove(approvedBy).Copy();

                gvLeaves.DataSource = employeeLeave;
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
                leaveScheduleDataHandler = null;
            }

        }

        protected void gvLeaves_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            log.Debug("webFrmLeaveApproval : gvLeaves_RowCommand()");

            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            string employeeId   = "";
            string leaveDate    = "";
            string approvedBy   = "";
            string leaveStatus  = "";

            try
            {
                Int32 index = Convert.ToInt32(e.CommandArgument);

                GridViewRow selectedRow = gvLeaves.Rows[index];

                employeeId = selectedRow.Cells[0].Text.ToString().Trim();
                leaveDate = selectedRow.Cells[2].Text.ToString().Trim();
                approvedBy = Session["KeyEMPLOYEE_ID"].ToString();
                string mailAddress = employeeDataHandler.getEmployeeEmail(employeeId);

                if (e.CommandName.ToString().Equals("Approve"))
                {
                    leaveStatus = Constants.CON_LEAVE_APPROVED_STATUS;
                    leaveScheduleDataHandler.approveRejectLeave(leaveStatus, employeeId, leaveDate, approvedBy);
                    loadLeaveForApproval();

                    if (mailAddress.Trim() != "")
                    {

                        EmailHandler.SendDefaultEmail("Leave System", mailAddress, "", "Leave approval", getApprovedMessage(leaveDate));
                    }

                 
                }
                else if (e.CommandName.ToString().Equals("Reject"))
                {
                    leaveStatus = Constants.CON_LEAVE_REJECTED_STATUS;
                    leaveScheduleDataHandler.approveRejectLeave(leaveStatus, employeeId, leaveDate, approvedBy);
                    loadLeaveForApproval();

                    if (mailAddress.Trim() != "")
                    {
                        EmailHandler.SendDefaultEmail("Leave System", mailAddress, "", "Leave approval", getRejectedMessage(leaveDate));
                    }
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
                leaveScheduleDataHandler = null;
            }
        }

        private StringBuilder getApprovedMessage(string sDate)
        {
            log.Debug("webFrmLeaveApproval : gvLeaves_RowCommand()");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append(" Your leave request on " + sDate + " has been approved " + Environment.NewLine);            
            stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }

        private StringBuilder getRejectedMessage(string sDate)
        {
            log.Debug("webFrmLeaveApproval : getRejectedMessage()");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append(" Your leave request on " + sDate + " has been rejected " + Environment.NewLine);
            stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }
    }
}