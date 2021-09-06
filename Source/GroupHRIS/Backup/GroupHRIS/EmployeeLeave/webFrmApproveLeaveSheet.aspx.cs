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
using System.Configuration;
//using HrisMail;
using Common;
using NLog;

namespace GroupHRIS.EmployeeLeave
{
    public partial class webFrmApproveLeaveSheet : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        //private string sUserId = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            lbtnClear.Visible   = false;
            lblLSDetail.Visible = false;

            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "webFrmApproveLeaveSheet : Page_Load");

            if (!IsPostBack)
            {
                loadLeaveForApproval();
            }
        }

        private void loadLeaveForApproval()
        {
            log.Debug("webFrmApproveLeaveSheet : loadLeaveForApproval()");

            string approvedBy = "";

            if(Session["KeyEMPLOYEE_ID"] != null)
            {
                approvedBy = Session["KeyEMPLOYEE_ID"].ToString();
            }

            DataTable employeeLeave = new DataTable();

            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

            try
            {
                employeeLeave = leaveScheduleDataHandler.getLeavesToCoverRecommand(approvedBy).Copy();

                gvApproval.DataSource = employeeLeave;
                gvApproval.DataBind();

            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }

        }

        protected void gvApproval_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            log.Debug("webFrmApproveLeaveSheet : gvApproval_RowCommand()");

            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            string leaveSheetId     = "";           
            string leaveStatus      = "";
            string newStatus        = "";
            string employeeId       = "";
            string fromDate         = "";
            string toDate           = "";
            string numDays          = "";
            string preStatus        = "";
            string empName          = "";
            string recommandByName  = "";

            try
            {
                Int32 index = Convert.ToInt32(e.CommandArgument);

                GridViewRow selectedRow = gvApproval.Rows[index];

                leaveSheetId = selectedRow.Cells[0].Text.ToString().Trim();
                leaveStatus = selectedRow.Cells[9].Text.ToString().Trim();
                employeeId = selectedRow.Cells[1].Text.ToString().Trim();
                fromDate = selectedRow.Cells[3].Text.ToString().Trim();
                toDate = selectedRow.Cells[4].Text.ToString().Trim();
                numDays = selectedRow.Cells[5].Text.ToString().Trim();
                
                string mailAddress = employeeDataHandler.getEmployeeEmail(employeeId);

                if (e.CommandName.ToString().Equals("Approve"))
                {
                    if (leaveStatus == Constants.LEAVE_STATUS_ACTIVE_VALUE)
                    {
                        preStatus = Constants.LEAVE_STATUS_ACTIVE_VALUE;
                        newStatus = Constants.LEAVE_STATUS_COVERED;
                    }
                    else if (leaveStatus == Constants.LEAVE_STATUS_COVERED)
                    {
                        preStatus = Constants.LEAVE_STATUS_COVERED;
                        newStatus = Constants.LEAVE_STATUS_RECOMMAND;
                    }

                    leaveScheduleDataHandler.coverRecommandApproveLeaveSheet(newStatus, leaveSheetId, preStatus);
                    loadLeaveForApproval();

                    DataRow dataRow = leaveScheduleDataHandler.getEmployeeRecommandBy(leaveSheetId);

                    string recommandByEmpId = "";                    
                    string recommandByMailAddress = "";

                    if (dataRow != null)
                    {                        
                        recommandByEmpId = dataRow["RECOMMEND_BY"].ToString().Trim(); 
                    }

                    empName = employeeDataHandler.getEmployeeName(employeeId.Trim());

                    recommandByMailAddress  = employeeDataHandler.getEmployeeEmail(recommandByEmpId.Trim());
                    recommandByName         = employeeDataHandler.getEmployeeName(recommandByEmpId.Trim());

                    if (mailAddress.Trim() != "")
                    {
                        if (newStatus == Constants.LEAVE_STATUS_COVERED)
                        {
                            DataRow dr = leaveScheduleDataHandler.getEmployeeCoveredBy(leaveSheetId);

                            string covByEmpId = "";
                            string covByName = "";

                            if (dr != null)
                            {
                                covByEmpId = dr["COVERED_BY"].ToString().Trim();
                                covByName = employeeDataHandler.getEmployeeName(covByEmpId);

                            }
                            
                            if (recommandByMailAddress.Trim() != "")
                            {
                                EmailHandler.SendDefaultEmailHtml("Leave System", recommandByMailAddress, "", "Leave Recommandation", getMailBodyHtml(empName, fromDate.Trim(), toDate.Trim(), numDays, Constants.CON_LEAVE_RECOMMEND, leaveSheetId.Trim(), employeeId.Trim(), covByName));
                            }

                            EmailHandler.SendDefaultEmail("Leave System", mailAddress, "", "Leave Covering", getApprovedMessage(fromDate, toDate, newStatus));
                                                                                
                        }
                        else if (newStatus == Constants.LEAVE_STATUS_RECOMMAND)
                        {
                            EmailHandler.SendDefaultEmail("Leave System", mailAddress, "", "Leave GrivanceComments", getApprovedMessage(fromDate, toDate, newStatus));
                        }                        
                    }
                    
                }
                else if (e.CommandName.ToString().Equals("Reject"))
                {
                    if (leaveStatus == Constants.LEAVE_STATUS_ACTIVE_VALUE)
                    {
                        preStatus = Constants.LEAVE_STATUS_ACTIVE_VALUE;
                    }
                    else if (leaveStatus == Constants.LEAVE_STATUS_COVERED)
                    {
                        preStatus = Constants.LEAVE_STATUS_COVERED;
                    }


                    newStatus = Constants.LEAVE_STATUS_REJECTED;
                    leaveScheduleDataHandler.coverRecommandApproveLeaveSheet(newStatus, leaveSheetId, preStatus);
                    loadLeaveForApproval();

                    if (mailAddress.Trim() != "")
                    {
                        EmailHandler.SendDefaultEmail("Leave System", mailAddress, "", "Leave Rejection", getRejectedMessage(fromDate, toDate, leaveStatus));
                    }

                    DataRow dr = leaveScheduleDataHandler.getEmployeeCoveredBy(leaveSheetId);
                    
                    string covByEmpId = "";
                    string covByMail = "";

                    if(dr != null)
                    {
                        covByEmpId = dr["COVERED_BY"].ToString().Trim();
                        covByMail = employeeDataHandler.getEmployeeEmail(covByEmpId);                         
                    }

                    if (leaveStatus == Constants.LEAVE_STATUS_COVERED)
                    {
                        if (covByMail.Trim() != "")
                        {
                            EmailHandler.SendDefaultEmail("Leave System", covByMail, "", "Leave Rejection(Duty Covering for " + empName.Trim() + " )", getRejectedMessageForCoveredBy(fromDate, toDate, empName));
                        }
                    }

                }
                else if(e.CommandName.ToString().Equals("View"))
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
            }
        }

        ////protected void gvApproval_SelectedIndexChanged(object sender, EventArgs e)
        ////{

        ////    log.Debug("webFrmApproveLeaveSheet : gvApproval_SelectedIndexChanged()");

        ////    LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

        ////    try
        ////    {
        ////        string empLeaveSchemeId = gvApproval.SelectedRow.Cells[0].Text.ToString().Trim();

        ////        if (empLeaveSchemeId.Trim() != String.Empty)
        ////        {
        ////            DataTable lsDetails = new DataTable();
        ////            lsDetails = leaveScheduleDataHandler.getLeaveSheetDetails(empLeaveSchemeId).Copy();
        ////            gvLSDetails.DataSource = lsDetails;
        ////            gvLSDetails.DataBind();

        ////            if (lsDetails.Rows.Count > 0)
        ////            {
        ////                lbtnClear.Visible = true;
        ////                lblLSDetail.Visible = true;
        ////            }
        ////        }


        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        StringBuilder oError = Utility.Utils.ExceptionLog(ex);
        ////        log.Debug(oError.ToString());
        ////        throw ex;
        ////    }
        ////    finally
        ////    {
        ////        leaveScheduleDataHandler = null;
        ////    }
        ////}

        protected void gvApproval_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {                
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                e.Row.Attributes.Add("style", "cursor:pointer;");
            }
        }

        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            gvLSDetails.DataSource = null;
            gvLSDetails.DataBind();

            lblLSDetail.Visible = false;
            lbtnClear.Visible = false;
        }

        private StringBuilder getApprovedMessage(string fromDate,string toDate,string status)
        {
            log.Debug("webFrmLeaveApproval : gvLeaves_RowCommand()");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);
            if (status == Constants.LEAVE_STATUS_RECOMMAND)
            {
                stringBuilder.Append(" Your leave request from " + fromDate + " to " + toDate + " has been recommended " + Environment.NewLine);
            }
            else if (status == Constants.LEAVE_STATUS_COVERED)
            {
                stringBuilder.Append(" Your request for leave covering from " + fromDate + " to " + toDate + " has been agreed " + Environment.NewLine);
            }            
            stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }

        private StringBuilder getRejectedMessage(string fromDate, string toDate,string status)
        {
            log.Debug("webFrmLeaveApproval : getRejectedMessage()");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);

            if (status == Constants.LEAVE_STATUS_ACTIVE_VALUE)
            {
                stringBuilder.Append(" Your leave request from " + fromDate + " to " + toDate + " has been rjected by assigned covering person " + Environment.NewLine);
            }
            else if (status == Constants.LEAVE_STATUS_COVERED)
            {
                stringBuilder.Append(" Your request from " + fromDate + " to " + toDate + " has been rjected by assigned recommending person " + Environment.NewLine);
            }
            
            stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }

        private StringBuilder getRejectedMessageForCoveredBy(string fromDate, string toDate, string applicantName)
        {
            log.Debug("webFrmLeaveApproval : getRejectedMessage()");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(" Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);

            stringBuilder.Append(" Your leave request of " + applicantName.Trim() + " from " + fromDate + " to " + toDate + " has been rjected by assigned recommending person " + Environment.NewLine);
           

            stringBuilder.Append(" Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append(" This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }


        private StringBuilder getMailBodyHtml(string employeeName, string sFromDate, string sToDate, string noDays, char covRec, string lsId, string applicantEmpId, string recomandName)
        {
            log.Debug("webFrmEmployeeLeaveSheet : getMailBodyHtml()");

            PasswordHandler crpto = new PasswordHandler();
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine + "</br></br>");
            if (sFromDate.Trim() == sToDate.Trim())
            {
                if (Double.Parse(noDays).Equals(Constants.CON_SL))
                {
                    stringBuilder.Append(employeeName + " has applied a short leave on " + sFromDate + "." + Environment.NewLine + "</br>");
                }                
                else if (Double.Parse(noDays).Equals(Constants.CON_HALF_DAY))
                {
                    stringBuilder.Append(employeeName + " has applied a half day leave on " + sFromDate + "." + Environment.NewLine + "</br>");
                }
                else
                {
                    stringBuilder.Append(employeeName + " has applied a leave on " + sFromDate + "." + Environment.NewLine + "</br>");
                }
            }
            else
            {
                if ((Double.Parse(noDays) > Constants.CON_SL) && (Double.Parse(noDays) < Constants.CON_HALF_DAY))
                {
                    stringBuilder.Append(employeeName + " has applied short leave from " + sFromDate + " to " + sToDate + "." + Environment.NewLine + "</br>");
                }
                else
                {
                    stringBuilder.Append(employeeName + " has applied " + noDays.ToString() + " leaves from " + sFromDate + " to " + sToDate + "." + Environment.NewLine + "</br>");
                }
            }

            if (covRec == Constants.CON_LEAVE_COVER)
            {
                stringBuilder.Append("Please cover duties." + Environment.NewLine + Environment.NewLine + "</br></br>");
                //stringBuilder.Append(""Please <a href=\"http://www.example.com/login.aspx\">login</a>");
                string link1 = "Please <a href=\"http://" + ConfigurationManager.AppSettings["host_Port"] + "/EmployeeLeave/webFrmApprove.aspx" + "?LsId=" + crpto.Encrypt(lsId) + "&CRF=" + crpto.Encrypt(Constants.LEAVE_SHEET_COVER_APPROVAL_FLAG) + "&ARF=" + crpto.Encrypt(Constants.LEAVE_SHEET_APPROVAL_FLAG) + "\"><b>AGREE</b></a> or ";
                stringBuilder.Append(link1);
                string link2 = "</t><a href=\"http://" + ConfigurationManager.AppSettings["host_Port"] + "/EmployeeLeave/webFrmApprove.aspx" + "?LsId=" + crpto.Encrypt(lsId) + "&CRF=" + crpto.Encrypt(Constants.LEAVE_SHEET_COVER_APPROVAL_FLAG) + "&ARF=" + crpto.Encrypt(Constants.LEAVE_SHEET_REJECT_FLAG) + "\"><b>REJECT</b></a></br>";
                stringBuilder.Append(link2);
                
            }
            else if (covRec == Constants.CON_LEAVE_RECOMMEND)
            {
                stringBuilder.Append("</br>");
                stringBuilder.Append("Leave Requested");
                stringBuilder.Append("<hr>");
                stringBuilder.Append(fillRequestedLeaves(lsId));
                stringBuilder.Append("</br>");
                stringBuilder.Append("<hr>");
                stringBuilder.Append("</br>");
                stringBuilder.Append("Please recommand it." + Environment.NewLine + Environment.NewLine + "</br></br>");
                string link1 = "Please <a href=\"http://" + ConfigurationManager.AppSettings["host_Port"] + "/EmployeeLeave/webFrmApprove.aspx" + "?LsId=" + crpto.Encrypt(lsId) + "&CRF=" + crpto.Encrypt(Constants.LEAVE_SHEET_RECOMMAND_APPROVAL_FLAG) + "&ARF=" + crpto.Encrypt(Constants.LEAVE_SHEET_APPROVAL_FLAG) + "\"><b>APPROVE</b></a> or ";
                stringBuilder.Append(link1);
                string link2 = "</t><a href=\"http://" + ConfigurationManager.AppSettings["host_Port"] + "/EmployeeLeave/webFrmApprove.aspx" + "?LsId=" + crpto.Encrypt(lsId) + "&CRF=" + crpto.Encrypt(Constants.LEAVE_SHEET_RECOMMAND_APPROVAL_FLAG) + "&ARF=" + crpto.Encrypt(Constants.LEAVE_SHEET_REJECT_FLAG) + "\"><b>REJECT</b></a></br>";
                stringBuilder.Append(link2);
                stringBuilder.Append("</br>");
                stringBuilder.Append("Leave covered by Mr/Ms." + recomandName);
                stringBuilder.Append("</br>");
                stringBuilder.Append("</br>");
                stringBuilder.Append("Leave Summary");
                stringBuilder.Append("<hr>");
                stringBuilder.Append(fillLeaveSummary(applicantEmpId));
                stringBuilder.Append("<hr>");

            }
            stringBuilder.Append("</br>" + "Thank you." + Environment.NewLine + Environment.NewLine + "</br></br>");
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            crpto = null;

            return stringBuilder;
        }

        private string fillRequestedLeaves(string sLeaveSheetId)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            DataTable leaveRequested = new DataTable();
            string sTable   = "";
            string sLType   = "";
            string sLCount  = "";
            string sShortLeaves = "";
            try
            {
                sTable = sTable + "<Table>";
                sTable = sTable + "<Tr style='color:Green;'>";
                sTable = sTable + "<Td style ='width:140px'>LEAVE TYPE</Td>";
                sTable = sTable + "<Td style ='width:170px'>NO OF LEAVE REQUESTED</Td>";
                sTable = sTable + "</Tr>";

                leaveRequested = leaveScheduleDataHandler.getRequestedLeaves(sLeaveSheetId).Copy();
                sShortLeaves = leaveScheduleDataHandler.getRequestedShortLeaves(sLeaveSheetId);

                if (leaveRequested.Rows.Count > 0)
                {
                    foreach (DataRow dr in leaveRequested.Rows)
                    {
                        sLType = dr["LEAVE_TYPE_ID"].ToString();
                        if (sLType.Trim().Equals(Constants.CON_SHORT_LEAVE_LEAVE_ID))
                        {
                            sLCount = sShortLeaves;
                        }
                        else
                        {
                            sLCount = dr["NO_OF_DAYS"].ToString();
                        }

                        sTable = sTable + "<Tr >";
                        sTable = sTable + "<Td class='LeaveOnlineTableTR'>" + sLType + "</Td>";
                        sTable = sTable + "<Td class='LeaveOnlineTableTR'>" + sLCount + "</Td>";

                        sTable = sTable + "</Tr>";

                    }

                    sTable = sTable + "</Table>";
                }

                return sTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                leaveScheduleDataHandler = null;
            }
        }


        private string fillLeaveSummary(string employeeId)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            LeaveConstrains leaveConstrains = new LeaveConstrains();
            string mLeaveType = "";
            decimal mLeaveAllow = 0;
            decimal mLeaveTaken = 0;
            decimal mLeaveBalance = 0;
            string mTableString = "";
            DataTable leaveSummary = new DataTable();

            //StringBuilder leaveSummaryTable = new StringBuilder();

            try
            {

                string sYear = DateTime.Today.Year.ToString();
                string sMonth = DateTime.Today.Month.ToString();
                int iYear = DateTime.Today.Year;
                string sShortLeaves = "";

                leaveSummary = leaveScheduleDataHandler.getEmployeeLeveSummary(employeeId.Trim(), sYear).Copy();
                sShortLeaves = leaveScheduleDataHandler.getConsumedShortLeaves(sYear, sMonth, employeeId);

                decimal anualLeaves = leaveConstrains.availableAnnualLeaves(employeeId.Trim(), iYear);
                decimal casualLeaves = leaveConstrains.availableCasualLeaves(employeeId.Trim(), iYear);

                mTableString = "";

                mLeaveAllow = 0;

                mTableString = mTableString + "<Table>";
                mTableString = mTableString + "<Tr style='color:Green;'>";
                mTableString = mTableString + "<Td style ='width:140px'>TYPE</Td>";
                mTableString = mTableString + "<Td style ='width:90px'>ENTITLED</Td>";
                mTableString = mTableString + "<Td style ='width:90px'>TAKEN</Td>";
                mTableString = mTableString + "<Td style ='width:90px'>BALANCE</Td>";
                mTableString = mTableString + "</Tr>";

                foreach (DataRow dr in leaveSummary.Rows)
                {
                    mLeaveType = dr["LEAVE_TYPE_NAME"].ToString();

                    if (dr["LEAVE_TYPE_ID"].ToString() == Constants.CON_ANNUAL_LEAVE_ID)
                    {
                        mLeaveAllow = decimal.Parse(anualLeaves.ToString());


                        if (dr["leaves_taken"].ToString().Trim() != "")
                        {
                            mLeaveTaken = decimal.Parse(dr["leaves_taken"].ToString().Trim());
                        }

                    }
                    else if (dr["LEAVE_TYPE_ID"].ToString() == Constants.CON_CASUAL_LEAVE_ID)
                    {
                        mLeaveAllow = decimal.Parse(casualLeaves.ToString());

                        if (dr["leaves_taken"].ToString().Trim() != "")
                        {
                            mLeaveTaken = decimal.Parse(dr["leaves_taken"].ToString().Trim());
                        }

                    }
                    else if (dr["LEAVE_TYPE_ID"].ToString() == Constants.CON_SHORT_LEAVE_LEAVE_ID)
                    {
                        mLeaveAllow = 2;
                                              
                    }
                    else
                    {
                        if (dr["NUMBER_OF_DAYS"].ToString() == "")
                        {
                            mLeaveAllow = 0;
                        }
                        else if (dr["NUMBER_OF_DAYS"].ToString() == null)
                        {
                            mLeaveAllow = 0;
                        }
                        else
                        {
                            mLeaveAllow = decimal.Parse(dr["NUMBER_OF_DAYS"].ToString());
                        }

                    }

                    if (dr["leaves_taken"].ToString() == "")
                    {
                        mLeaveTaken = 0;
                        mLeaveBalance = decimal.Parse((mLeaveAllow - mLeaveTaken).ToString());
                    }
                    else if (dr["leaves_taken"].ToString() == null)
                    {
                        mLeaveTaken = 0;
                        mLeaveBalance = decimal.Parse((mLeaveAllow - mLeaveTaken).ToString());
                    }
                    else
                    {
                        if (dr["LEAVE_TYPE_ID"].ToString() == Constants.CON_SHORT_LEAVE_LEAVE_ID)
                        {
                            if (sShortLeaves.Trim().Equals(String.Empty))
                            {
                                mLeaveTaken = 0;
                            }
                            else
                            {
                                mLeaveTaken = decimal.Parse(sShortLeaves);
                            }

                            mLeaveBalance = decimal.Parse((mLeaveAllow - mLeaveTaken).ToString());
                        }
                        else
                        {
                            mLeaveTaken = decimal.Parse(dr["leaves_taken"].ToString());
                            mLeaveBalance = decimal.Parse((mLeaveAllow - mLeaveTaken).ToString());
                        }


                    }

                    if (mLeaveBalance < 0)
                    {
                        mLeaveBalance = 0;
                    }

                    mTableString = mTableString + "<Tr >";
                    mTableString = mTableString + "<Td class='LeaveOnlineTableTR'>" + mLeaveType + "</Td>";
                    mTableString = mTableString + "<Td class='LeaveOnlineTableTR'>" + mLeaveAllow + "</Td>";
                    mTableString = mTableString + "<Td class='LeaveOnlineTableTR'>" + mLeaveTaken + "</Td>";
                    mTableString = mTableString + "<Td class='LeaveOnlineTableTR'>" + mLeaveBalance + "</Td>";
                    mTableString = mTableString + "</Tr>";
                }

                mTableString = mTableString + "</Table>";

                return mTableString;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                leaveScheduleDataHandler = null;
                leaveConstrains = null;
                leaveSummary.Dispose();
            }
        }





    }
}