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
using System.Configuration;
using DomainConstraints;
using Common;
using NLog;

namespace GroupHRIS.EmployeeLeave
{
    public partial class WebFrmApprove : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Request.Browser.IsMobileDevice)
            //{
            //    btnYes.Width = 200;
            //    btnYes.Height = 80;
            //    btnYes.Font.Size = 20;

            //    btnNo.Width = 200;
            //    btnNo.Height = 80;
            //    btnNo.Font.Size = 20;
            //}

            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            PasswordHandler cripto = new PasswordHandler();

            string appRec   = cripto.Decrypt(Request.QueryString["ARF"]);
            string covRec   = cripto.Decrypt(Request.QueryString["CRF"]);
            string lsId     = cripto.Decrypt(Request.QueryString["LsId"]);
            string dbLeaveStatus = "";

            DataRow dataRow = leaveScheduleDataHandler.getEmployeeRecommandBy(lsId);

            if (dataRow != null)
            {                
                dbLeaveStatus = dataRow["LEAVE_STATUS"].ToString().Trim();
            }

            //--------------------------

            if (appRec.Trim() == Constants.LEAVE_SHEET_APPROVAL_FLAG)
            {

                //if ((dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_COVERED) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_RECOMMAND) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_APPROVED))
                //{
                //    lblerror.Text = Constants.CON_LEAVE_ALREADY_APPROVED_MESSAGE;
                //}
                //else if (dbLeaveStatus.Trim() == Constants.CON_LEAVE_REJECTED_STATUS)
                //{
                //    lblerror.Text = Constants.CON_LEAVE_ALREADY_REJECTED_MESSAGE;
                //}
                //else
                //{
                    //lblerror.Text = "Do you need to approve leave sheet?";
                //}

                if (dbLeaveStatus == Constants.LEAVE_STATUS_ACTIVE_VALUE)
                {
                    lblerror.Text = "Do you need to agree with leave Covering for this leave sheet?";
                }
                else if (dbLeaveStatus == Constants.LEAVE_STATUS_COVERED)
                {
                    lblerror.Text = "Do you need to recommand this leave sheet?";
                }

            }
            else if (appRec.Trim() == Constants.LEAVE_SHEET_REJECT_FLAG)
            {
                //if ((dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_COVERED) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_RECOMMAND) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_APPROVED))
                //{
                //    lblerror.Text = Constants.CON_LEAVE_ALREADY_APPROVED_MESSAGE;
                //}
                //else if (dbLeaveStatus.Trim() == Constants.CON_LEAVE_REJECTED_STATUS)
                //{
                //    lblerror.Text = Constants.CON_LEAVE_ALREADY_REJECTED_MESSAGE;
                //}
                //else
                //{
                    lblerror.Text = "Do you need to reject this leave sheet?";
                //}
            }


            //-------------------------------------------------------

            ////LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            ////EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            ////PasswordHandler cripto = new PasswordHandler();

            ////string newStatus    = "";
            ////string covRec       = "";            
            ////string appRec       = "";
            ////string lsId         = "";

            ////string applicantEmpId   = "";
            ////string recommandByEmpId = "";
            
            ////string applicantMailAddress     = "";
            ////string recommandByMailAddress   = "";

            ////string empName      = "";

            ////string fromDate     = "";
            ////string toDate       = "";
            ////string numDays      = "";
            ////string preStatus = "";
            ////string dbLeaveStatus = "";

            ////try
            ////{
            ////    lblcopyright.Text = CommonVariables.COPY_RIGHT_TEXT;
            ////    lblerror.Text = "";

            ////    lsId = cripto.Decrypt(Request.QueryString["LsId"]);
            ////    covRec = cripto.Decrypt(Request.QueryString["CRF"]);
            ////    appRec = cripto.Decrypt(Request.QueryString["ARF"]);

            ////    DataRow dataRow = leaveScheduleDataHandler.getEmployeeRecommandBy(lsId);

            ////    if (dataRow != null)
            ////    {
            ////        applicantEmpId = dataRow["EMPLOYEE_ID"].ToString().Trim();
            ////        recommandByEmpId = dataRow["RECOMMEND_BY"].ToString().Trim();
            ////        fromDate = dataRow["FROM_DATE"].ToString().Trim();
            ////        toDate = dataRow["TO_DATE"].ToString().Trim();
            ////        numDays = dataRow["NO_OF_DAYS"].ToString().Trim();
            ////        dbLeaveStatus = dataRow["LEAVE_STATUS"].ToString().Trim();
            ////    }


            ////    if (covRec.Trim() == Constants.LEAVE_SHEET_COVER_APPROVAL_FLAG)
            ////    {


            ////        preStatus = Constants.LEAVE_STATUS_ACTIVE_VALUE;

            ////        if (appRec.Trim() == Constants.LEAVE_SHEET_APPROVAL_FLAG)
            ////        {                       
            ////            newStatus = Constants.LEAVE_STATUS_COVERED;
            ////        }
            ////        else if (appRec.Trim() == Constants.LEAVE_SHEET_REJECT_FLAG)
            ////        {
            ////            newStatus = Constants.LEAVE_STATUS_REJECTED;
            ////        }
            ////    }
            ////    else if (covRec.Trim() == Constants.LEAVE_SHEET_RECOMMAND_APPROVAL_FLAG)
            ////    {
            ////        preStatus = Constants.LEAVE_STATUS_COVERED;

            ////        if (appRec.Trim() == Constants.LEAVE_SHEET_APPROVAL_FLAG)
            ////        {                        
            ////            newStatus = Constants.LEAVE_STATUS_RECOMMAND;
            ////        }
            ////        else if (appRec.Trim() == Constants.LEAVE_SHEET_REJECT_FLAG)
            ////        {
            ////            newStatus = Constants.LEAVE_STATUS_REJECTED;
            ////        }
            ////    }

            ////    leaveScheduleDataHandler.coverRecommandApproveLeaveSheet(newStatus, lsId, preStatus);
                 
            ////    empName = employeeDataHandler.getEmployeeName(applicantEmpId.Trim());
                
            ////    applicantMailAddress = employeeDataHandler.getEmployeeEmail(applicantEmpId.Trim());


            ////    if (covRec.Trim() == Constants.LEAVE_SHEET_COVER_APPROVAL_FLAG)
            ////    {
                    
            ////        recommandByMailAddress = employeeDataHandler.getEmployeeEmail(recommandByEmpId);

            ////        if (appRec.Trim() == Constants.LEAVE_SHEET_APPROVAL_FLAG)
            ////        {
            ////            if ((dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_COVERED) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_RECOMMAND) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_APPROVED))
            ////            {
            ////                lblerror.Text = Constants.CON_LEAVE_ALREADY_APPROVED_MESSAGE;  
            ////            }
            ////            else if (dbLeaveStatus.Trim() == Constants.CON_LEAVE_ALREADY_REJECTED_MESSAGE)
            ////            {
            ////                lblerror.Text = Constants.CON_LEAVE_ALREADY_APPROVED_MESSAGE;  
            ////            }
            ////            else
            ////            {
            ////                EmailHandler.SendDefaultEmailHtml("Leave System", recommandByMailAddress, "", "Leave Recommandation", getMailBodyHtml(empName, fromDate.Trim(), toDate.Trim(), numDays, Constants.CON_LEAVE_RECOMMEND, lsId.Trim()));
            ////                EmailHandler.SendDefaultEmail("Leave System", applicantMailAddress, "", "Leave Covering", getApprovedMessage(fromDate, toDate, newStatus));

            ////                lblerror.Text = Constants.CON_LEAVE_APPROVED_MESSAGE;  
            ////            }
                                                                      
            ////        }
            ////        else if (appRec.Trim() == Constants.LEAVE_SHEET_REJECT_FLAG)
            ////        {
            ////            if ((dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_COVERED) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_RECOMMAND) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_APPROVED))
            ////            {
            ////                lblerror.Text = Constants.CON_LEAVE_ALREADY_APPROVED_MESSAGE;  
            ////            }
            ////            else if (dbLeaveStatus.Trim() == Constants.CON_LEAVE_ALREADY_REJECTED_MESSAGE)
            ////            {
            ////                lblerror.Text = Constants.CON_LEAVE_ALREADY_APPROVED_MESSAGE;
            ////            }
            ////            else
            ////            {
            ////                EmailHandler.SendDefaultEmail("Leave System", applicantMailAddress, "", "Leave Rejection", getRejectedMessage(fromDate, toDate, Constants.LEAVE_SHEET_COVER_APPROVAL_FLAG));
            ////                lblerror.Text = Constants.CON_LEAVE_REJECTED_MESSAGE;
            ////            }
            ////        }
            ////    }
            ////    else if(covRec.Trim() == Constants.LEAVE_SHEET_RECOMMAND_APPROVAL_FLAG)
            ////    {
            ////        if (appRec.Trim() == Constants.LEAVE_SHEET_APPROVAL_FLAG)
            ////        {
            ////            if ((dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_RECOMMAND) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_APPROVED))
            ////            {
            ////                lblerror.Text = Constants.CON_LEAVE_ALREADY_APPROVED_MESSAGE;  
            ////            }
            ////            else if (dbLeaveStatus.Trim() == Constants.CON_LEAVE_ALREADY_REJECTED_MESSAGE)
            ////            {
            ////                lblerror.Text = Constants.CON_LEAVE_ALREADY_APPROVED_MESSAGE;
            ////            }
            ////            else
            ////            {                            
            ////                EmailHandler.SendDefaultEmail("Leave System", applicantMailAddress, "", "Leave Recommandation", getApprovedMessage(fromDate, toDate, newStatus));

            ////                lblerror.Text = Constants.CON_LEAVE_APPROVED_MESSAGE;
            ////            }
            ////        }
            ////        else if (appRec.Trim() == Constants.LEAVE_SHEET_REJECT_FLAG)
            ////        {
            ////            if ((dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_RECOMMAND) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_APPROVED))
            ////            {
            ////                lblerror.Text = Constants.CON_LEAVE_ALREADY_APPROVED_MESSAGE;  
            ////            }
            ////            else if (dbLeaveStatus.Trim() == Constants.CON_LEAVE_ALREADY_REJECTED_MESSAGE)
            ////            {
            ////                lblerror.Text = Constants.CON_LEAVE_ALREADY_APPROVED_MESSAGE;
            ////            }
            ////            else
            ////            {
            ////                EmailHandler.SendDefaultEmail("Leave System", applicantMailAddress, "", "Leave Rejection", getRejectedMessage(fromDate, toDate, Constants.LEAVE_SHEET_RECOMMAND_APPROVAL_FLAG));
            ////                lblerror.Text = Constants.CON_LEAVE_REJECTED_MESSAGE;
            ////            }
            ////        }                    
            ////    }  
            ////}
            ////catch (Exception ex)
            ////{
            ////    throw ex;
            ////}            

           
        }


        private StringBuilder getMailBodyHtml(string employeeName, string sFromDate, string sToDate, string noDays, char covRec, string lsId,string applicantEmpId,string recomandName)
        {
            log.Debug("webFrmEmployeeLeaveSheet : getMailBodyHtml()");

            PasswordHandler crpto = new PasswordHandler();

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine + "</br></br>");
            if (sFromDate.Trim() == sToDate.Trim())
            {
                //double dN = Math.Round(Constants.CON_SL,1);
                //double dc = Math.Ceiling(Constants.CON_SL);

                if (Double.Parse(noDays).Equals(Math.Round(Constants.CON_SL,1)))
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
                    stringBuilder.Append(employeeName + " has applied " + noDays.Trim() + " leaves from " + sFromDate + " to " + sToDate + "." + Environment.NewLine + "</br>");
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

        private StringBuilder getApprovedMessage(string fromDate, string toDate, string status)
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

        private StringBuilder getRejectedMessage(string fromDate, string toDate, string flag)
        {
            log.Debug("webFrmLeaveApproval : getRejectedMessage()");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);

            if (flag == Constants.LEAVE_SHEET_COVER_APPROVAL_FLAG)
            {
                stringBuilder.Append(" Your leave request from " + fromDate + " to " + toDate + " has been rjected by assigned covering person " + Environment.NewLine);
            }
            else if (flag == Constants.LEAVE_SHEET_RECOMMAND_APPROVAL_FLAG)
            {
                stringBuilder.Append(" Your request for leave covering from " + fromDate + " to " + toDate + " has been rjected by assigned recommending person " + Environment.NewLine);
            }

            stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            PasswordHandler cripto = new PasswordHandler();

            string newStatus = "";
            string covRec = "";
            string appRec = "";
            string lsId = "";

            string applicantEmpId   = "";
            string recommandByEmpId = "";
            string recommandByName  = "";

            string applicantMailAddress = "";
            string recommandByMailAddress = "";

            string empName = "";

            string fromDate = "";
            string toDate = "";
            string numDays = "";
            string preStatus = "";
            string dbLeaveStatus = "";

            try
            {
                lblcopyright.Text = CommonVariables.COPY_RIGHT_TEXT;
                lblerror.Text = "";

                lsId = cripto.Decrypt(Request.QueryString["LsId"]);
                covRec = cripto.Decrypt(Request.QueryString["CRF"]);
                appRec = cripto.Decrypt(Request.QueryString["ARF"]);

                DataRow dataRow = leaveScheduleDataHandler.getEmployeeRecommandBy(lsId);

                if (dataRow != null)
                {
                    applicantEmpId = dataRow["EMPLOYEE_ID"].ToString().Trim();
                    recommandByEmpId = dataRow["RECOMMEND_BY"].ToString().Trim();
                    fromDate = dataRow["FROM_DATE"].ToString().Trim();
                    toDate = dataRow["TO_DATE"].ToString().Trim();
                    numDays = dataRow["NO_OF_DAYS"].ToString().Trim();
                    dbLeaveStatus = dataRow["LEAVE_STATUS"].ToString().Trim();
                }

                recommandByName = employeeDataHandler.getEmployeeName(recommandByEmpId);

                if (covRec.Trim() == Constants.LEAVE_SHEET_COVER_APPROVAL_FLAG)
                {                    
                    preStatus = Constants.LEAVE_STATUS_ACTIVE_VALUE;

                    if (appRec.Trim() == Constants.LEAVE_SHEET_APPROVAL_FLAG)
                    {
                        newStatus = Constants.LEAVE_STATUS_COVERED;
                    }
                    else if (appRec.Trim() == Constants.LEAVE_SHEET_REJECT_FLAG)
                    {
                        newStatus = Constants.LEAVE_STATUS_REJECTED;
                    }
                }
                else if (covRec.Trim() == Constants.LEAVE_SHEET_RECOMMAND_APPROVAL_FLAG)
                {
                    preStatus = Constants.LEAVE_STATUS_COVERED;

                    if (appRec.Trim() == Constants.LEAVE_SHEET_APPROVAL_FLAG)
                    {
                        newStatus = Constants.LEAVE_STATUS_RECOMMAND;
                    }
                    else if (appRec.Trim() == Constants.LEAVE_SHEET_REJECT_FLAG)
                    {
                        newStatus = Constants.LEAVE_STATUS_REJECTED;
                    }
                }

                leaveScheduleDataHandler.coverRecommandApproveLeaveSheet(newStatus, lsId, preStatus);

                empName = employeeDataHandler.getEmployeeName(applicantEmpId.Trim());

                applicantMailAddress = employeeDataHandler.getEmployeeEmail(applicantEmpId.Trim());


                if (covRec.Trim() == Constants.LEAVE_SHEET_COVER_APPROVAL_FLAG)
                {

                    recommandByMailAddress = employeeDataHandler.getEmployeeEmail(recommandByEmpId);

                    if (appRec.Trim() == Constants.LEAVE_SHEET_APPROVAL_FLAG)
                    {
                        if ((dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_COVERED) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_RECOMMAND) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_APPROVED))
                        {
                            lblerror.Text = Constants.CON_LEAVE_ALREADY_AGREED_MESSAGE;
                        }
                        else if (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_REJECTED)
                        {
                            lblerror.Text = Constants.CON_LEAVE_ALREADY_REJECTED_MESSAGE;
                        }
                        else
                        {
                            DataRow dr = leaveScheduleDataHandler.getEmployeeCoveredBy(lsId);

                            string covByEmpId = "";
                            string covByName  = "";

                            if (dr != null)
                            {
                                covByEmpId = dr["COVERED_BY"].ToString().Trim();
                                covByName = employeeDataHandler.getEmployeeName(covByEmpId);

                            }

                            if (recommandByMailAddress.Trim() != "")
                            {
                                EmailHandler.SendDefaultEmailHtml("Leave System", recommandByMailAddress, "", "Leave Recommandation", getMailBodyHtml(empName, fromDate.Trim(), toDate.Trim(), numDays, Constants.CON_LEAVE_RECOMMEND, lsId.Trim(), applicantEmpId, covByName));
                            }
                            if (applicantMailAddress.Trim() != "")
                            {
                                EmailHandler.SendDefaultEmail("Leave System", applicantMailAddress, "", "Leave Covering", getApprovedMessage(fromDate, toDate, newStatus));
                            }
                            lblerror.Text = Constants.CON_LEAVE_APPROVED_MESSAGE;
                        }

                        btnYes.Visible = false;
                        btnNo.Visible = false;
                    }
                    else if (appRec.Trim() == Constants.LEAVE_SHEET_REJECT_FLAG)
                    {
                        if ((dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_COVERED) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_RECOMMAND) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_APPROVED))
                        {
                            lblerror.Text = Constants.CON_LEAVE_ALREADY_APPROVED_MESSAGE;
                        }
                        else if (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_REJECTED)
                        {
                            lblerror.Text = Constants.CON_LEAVE_ALREADY_REJECTED_MESSAGE;
                        }
                        else
                        {
                            if (applicantMailAddress.Trim() != "")
                            {
                                EmailHandler.SendDefaultEmail("Leave System", applicantMailAddress, "", "Leave Rejection", getRejectedMessage(fromDate, toDate, Constants.LEAVE_SHEET_COVER_APPROVAL_FLAG));
                            }
                            lblerror.Text = Constants.CON_LEAVE_REJECTED_MESSAGE;
                        }

                        btnYes.Visible = false;
                        btnNo.Visible = false;
                    }
                }
                else if (covRec.Trim() == Constants.LEAVE_SHEET_RECOMMAND_APPROVAL_FLAG)
                {
                    if (appRec.Trim() == Constants.LEAVE_SHEET_APPROVAL_FLAG)
                    {
                        if ((dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_RECOMMAND) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_APPROVED))
                        {
                            lblerror.Text = Constants.CON_LEAVE_ALREADY_APPROVED_MESSAGE;
                        }
                        else if (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_REJECTED)
                        {
                            lblerror.Text = Constants.CON_LEAVE_ALREADY_REJECTED_MESSAGE;
                        }
                        else
                        {
                            if (applicantMailAddress.Trim() != "")
                            {
                                EmailHandler.SendDefaultEmail("Leave System", applicantMailAddress, "", "Leave Recommandation", getApprovedMessage(fromDate, toDate, newStatus));
                            }
                            lblerror.Text = Constants.CON_LEAVE_APPROVED_MESSAGE;
                        }

                        btnYes.Visible = false;
                        btnNo.Visible = false;

                    }
                    else if (appRec.Trim() == Constants.LEAVE_SHEET_REJECT_FLAG)
                    {
                        if ((dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_RECOMMAND) || (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_APPROVED))
                        {
                            lblerror.Text = Constants.CON_LEAVE_ALREADY_APPROVED_MESSAGE;
                        }
                        else if (dbLeaveStatus.Trim() == Constants.LEAVE_STATUS_REJECTED)
                        {
                            lblerror.Text = Constants.CON_LEAVE_ALREADY_REJECTED_MESSAGE;
                        }
                        else
                        {
                            if (applicantMailAddress.Trim() != "")
                            {
                                EmailHandler.SendDefaultEmail("Leave System", applicantMailAddress, "", "Leave Rejection", getRejectedMessage(fromDate, toDate, Constants.LEAVE_SHEET_RECOMMAND_APPROVAL_FLAG));
                            }
                            DataRow dr = leaveScheduleDataHandler.getEmployeeCoveredBy(lsId);

                            string covByEmpId = "";
                            string covByMail = "";

                            if (dr != null)
                            {
                                covByEmpId = dr["COVERED_BY"].ToString().Trim();
                                covByMail = employeeDataHandler.getEmployeeEmail(covByEmpId);

                                if (covByMail.Trim() != "")
                                {
                                    EmailHandler.SendDefaultEmail("Leave System", covByMail, "", "Leave Rejection(Duty Covering for " + empName.Trim() + " )", getRejectedMessageForCoveredBy(fromDate, toDate, empName));
                                }                                
                            }
                            
                            lblerror.Text = Constants.CON_LEAVE_REJECTED_MESSAGE;
                        }

                        btnYes.Visible = false;
                        btnNo.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }


        private StringBuilder getRejectedMessageForCoveredBy(string fromDate, string toDate, string applicantName)
        {
            log.Debug("webFrmLeaveApproval : getRejectedMessage()");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(" Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);

            stringBuilder.Append(" Leave request of " + applicantName.Trim() + " from " + fromDate + " to " + toDate + " has been rjected by assigned recommending person " + Environment.NewLine);


            stringBuilder.Append(" Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append(" This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }

        //protected void btnNo_Click(object sender, EventArgs e)
        //{
        //    string sCript = "<script>parent.close_window();</script>";
        //    Response.Write("<script>parent.close_window();</script>");
        //    ClientScript.RegisterClientScriptBlock(typeof(Page), "closeTab", sCript, true);
        //}

        private string fillRequestedLeaves(string sLeaveSheetId)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            DataTable leaveRequested = new DataTable();
            string sTable = "";
            string sLType = "";
            string sLCount = "";
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