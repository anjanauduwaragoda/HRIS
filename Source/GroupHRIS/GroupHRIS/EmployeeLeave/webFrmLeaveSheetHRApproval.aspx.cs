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
    public partial class webFrmLeaveSheetHRApproval : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            lbtnClear.Visible = false;
            lblLSDetail.Visible = false;

            sIPAddress = Request.UserHostAddress;
            log.Debug("webFrmLeaveSheetHRApproval : Page_Load");

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
                loadData();
            }
        }

        protected void loadData()
        {
            log.Debug("webFrmLeaveSheetHRApproval : loadData()");

            string companyId = String.Empty;
            string companyName = String.Empty;

            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            DataTable leaves = new DataTable();

            try
            {
                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        lblCompany.Text = "All Companies";
                        companyId = Constants.CON_UNIVERSAL_COMPANY_CODE;
                        leaves = leaveScheduleDataHandler.getLeavesToApproveByHR();
                        gvLeaves.DataSource = leaves;
                        gvLeaves.DataBind();
                    }
                    else
                    {
                        companyId = Session["KeyCOMP_ID"].ToString().Trim();
                        companyName = companyDataHandler.getCompanyNameByCompanyId(companyId);
                        lblCompany.Text = companyName;
                        leaves = leaveScheduleDataHandler.getLeavesToApproveByHR(companyId.Trim());
                        gvLeaves.DataSource = leaves;
                        gvLeaves.DataBind();
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
                companyDataHandler = null;
                leaveScheduleDataHandler = null;
                leaves.Dispose();

            }
        }

        protected void gvLeaves_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            log.Debug("webFrmLeaveSheetHRApproval : gvLeaves_RowCommand()");

            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            string leaveSheetId = "";
            string leaveStatus = "";
            string newStatus = "";
            string employeeId = "";
            string fromDate = "";
            string toDate = "";
            string approvedBy = "";
            string preStatus = "";

            try
            {
                if (Session["KeyEMPLOYEE_ID"] != null)
                {
                    approvedBy = Session["KeyEMPLOYEE_ID"].ToString();
                }

                Int32 index = Convert.ToInt32(e.CommandArgument);

                GridViewRow selectedRow = gvLeaves.Rows[index];

                leaveSheetId = selectedRow.Cells[0].Text.ToString().Trim();
                leaveStatus = selectedRow.Cells[8].Text.ToString().Trim();
                employeeId = selectedRow.Cells[1].Text.ToString().Trim();
                fromDate = selectedRow.Cells[3].Text.ToString().Trim();
                toDate = selectedRow.Cells[4].Text.ToString().Trim();

                string mailAddress = employeeDataHandler.getEmployeeEmail(employeeId);

                preStatus = Constants.LEAVE_STATUS_RECOMMAND;


                if (e.CommandName.ToString().Equals("Accept"))
                {
                    if (leaveStatus == Constants.LEAVE_STATUS_RECOMMAND)
                    {
                        newStatus = Constants.LEAVE_STATUS_APPROVED;

                        leaveScheduleDataHandler.coverRecommandApproveLeaveSheet(newStatus, leaveSheetId, approvedBy, preStatus);
                        loadData(); 
                    }

                    if (mailAddress.Trim() != "")
                    {

                        EmailHandler.SendDefaultEmail("Leave System", mailAddress, "", "Leave Approval", getApprovedMessage(fromDate, toDate));
                    }

                }
                else if (e.CommandName.ToString().Equals("Reject"))
                {
                    newStatus = Constants.LEAVE_STATUS_REJECTED;
                    leaveScheduleDataHandler.coverRecommandApproveLeaveSheet(newStatus, leaveSheetId, approvedBy, preStatus);
                    loadData();

                    if (mailAddress.Trim() != "")
                    {
                        EmailHandler.SendDefaultEmail("Leave System", mailAddress, "", "Leave Rejection", getRejectedMessage(fromDate, toDate));
                    }
                }
                else if (e.CommandName.ToString().Equals("View"))
                {
                    if (leaveSheetId.Trim() != String.Empty)
                    {
                        DataTable lsDetails = new DataTable();
                        lsDetails = leaveScheduleDataHandler.getLeaveSheetDetails(leaveSheetId).Copy();
                        gvLSDetails.DataSource = lsDetails;
                        gvLSDetails.DataBind();

                        if (lsDetails.Rows.Count > 0)
                        {
                            lbtnClear.Visible = true;
                            lblLSDetail.Visible = true;
                        }
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
                employeeDataHandler = null;
            }
        }

        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            gvLSDetails.DataSource = null;
            gvLSDetails.DataBind();

            lblLSDetail.Visible = false;
            lbtnClear.Visible = false;
        }

        private StringBuilder getApprovedMessage(string fromDate, string toDate)
        {
            log.Debug("webFrmLeaveApproval : gvLeaves_RowCommand()");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append(" Your leave request from " + fromDate + " to " + toDate + " has been approved by HR " + Environment.NewLine);
            
            stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }

        private StringBuilder getRejectedMessage(string fromDate, string toDate)
        {
            log.Debug("webFrmLeaveApproval : getRejectedMessage()");

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);                      
            stringBuilder.Append(" Your leave request from " + fromDate + " to " + toDate + " has been rjected by HR " + Environment.NewLine);
            stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }


    }
}