using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.MetaData;
using System.Data;
using Common;
using DataHandler.Attendance;
using NLog;
using DataHandler.Userlogin;
using System.Threading;

namespace GroupHRIS.Attendance
{
    public partial class AttendanceSummary : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static string sCultryDate = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            log.Debug("AttendanceSummary : Page_Load()");

            if (!IsPostBack)
            {
                string KeyCOMP_ID = (string)(Session["KeyCOMP_ID"]);
                getCompID(KeyCOMP_ID);
                getCultryDate();
            }
        }

        private void getCultryDate()
        {
            log.Debug("AttendanceSummary : getCultryDate()");


            string sCultryDate = "";
            AttendanceSummaryHandler attendanceSummaryHandler = new AttendanceSummaryHandler();

            try
            {
                sCultryDate = attendanceSummaryHandler.getCultryDate();
                DateTime sSummaryDate = DateTime.Parse(sCultryDate.ToString());
                txtfromdate.Text = sSummaryDate.ToString("yyyy/MM/dd");
                txttodate.Text = sSummaryDate.ToString("yyyy/MM/dd");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                attendanceSummaryHandler = null;
            }


        }

        private void getCompID(string KeyCOMP_ID)
        {
            log.Debug("AttendanceSummary : getCompID()");


            CompanyDataHandler companynameid = new CompanyDataHandler();
            DataTable CompID = new DataTable();
            try
            {
                if (KeyCOMP_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    CompID = companynameid.getCompanyIdCompName();
                    ListItem lstItem = new ListItem();
                    lstItem.Text = Constants.CON_UNIVERSAL_COMPANY_NAME;
                    lstItem.Value = Constants.CON_UNIVERSAL_COMPANY_CODE;
                    dpCompID.Items.Add(lstItem);
                }
                else
                {
                    CompID = companynameid.getCompanyIdCompName(KeyCOMP_ID);
                }

                foreach (DataRow dataRow in CompID.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = dataRow["COMP_NAME"].ToString();
                    listItem.Value = dataRow["COMPANY_ID"].ToString();
                    dpCompID.Items.Add(listItem);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                companynameid = null;
                CompID.Dispose();
                CompID = null;
            }
        }


        protected void btngeneratecalendar_Click(object sender, EventArgs e)
        {
            log.Debug("AttendanceSummary : btngeneratecalendar_Click()");

            AttendanceSummaryHandler attendanceSummaryHandler = new AttendanceSummaryHandler();
            
            try
            {
                string sCompCode = dpCompID.SelectedValue.ToString();
                string sEmployee_ID = txtemployee.Text.ToString().Trim();
                DateTime mFromDate = Convert.ToDateTime(txtfromdate.Text.ToString());
                DateTime mToDate = Convert.ToDateTime(txttodate.Text.ToString());
                CommonVariables.MESSAGE_TEXT = "";

                // Run Clock IN/OUT Config

                Boolean isAttnapproval = attendanceSummaryHandler.execute_InOUTapprovals();
                if (!isAttnapproval == true)
                {
                    log.Error("AttendanceSummary : " + "execute_InOUTapprovals -> From Date: " + mFromDate + " To Date: " + mToDate);
                }

                Thread.Sleep(10);

                
                //Run Summary

                if (!sEmployee_ID.Equals(""))
                {
                    if (chkemployee.Checked == true)
                    {
                        Boolean bsummerzied = attendanceSummaryHandler.execute_summary_employee(sEmployee_ID, mFromDate, mToDate);
                        if (bsummerzied == true)
                        {
                            CommonVariables.MESSAGE_TEXT = "Summary successfully processed.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Unable to process summary.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "You have not selected an employee";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                }
                else
                {

                    ////////Process Backdated Leave
                    Boolean isBackdatedLeave = attendanceSummaryHandler.execute_BackdateLeave();
                    if (!isBackdatedLeave == true)
                    {
                        log.Error("AttendanceSummary : " + "execute_BackdateLeave ->  From Date: " + sCultryDate + " To Date: " + mToDate);
                    }

                    Thread.Sleep(10);

                    Boolean bsummerzied = attendanceSummaryHandler.execute_company(sCompCode, mFromDate, mToDate);
                    if (bsummerzied == true)
                    {
                        CommonVariables.MESSAGE_TEXT = "Summary successfully processed.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "Unable to process summary.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
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
                attendanceSummaryHandler = null;
            }
        }

        protected void chkemployee_CheckedChanged(object sender, EventArgs e)
        {
            if (chkemployee.Checked == false)
            {
                txtemployee.Text = "";
            }
        }

        protected void dpCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblerror);
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblerror);
            txtemployee.Text = "";
        }

        //private void sendemails()
        //{
        //    EmailHandler emailHandler = new EmailHandler();
        //    AttendanceSummaryHandler attendanceSummaryHandler = new AttendanceSummaryHandler();
        //    DataTable AttSendmail = new DataTable();
        //    DateTime mToDate = DateTime.Parse(sCultryDate);
        //    AttSendmail = attendanceSummaryHandler.SummaryAbsetList(mToDate);
        //    if (AttSendmail.Rows.Count > 1)
        //    {
        //        for (int i = 0; i < AttSendmail.Rows.Count; i++)
        //        {
        //            string IsabsetEmail = AttSendmail.Rows[i]["email"].ToString();
        //            string IsabsentName = AttSendmail.Rows[i]["initials_name"].ToString();
        //            string IsabsentDate = AttSendmail.Rows[i]["IN_DATE"].ToString();

        //            Boolean IssentMail = emailHandler.SendEmailtoAbsence(IsabsetEmail, IsabsentDate, IsabsentName);
        //            if (IssentMail == false)
        //            {
        //                log.Debug("sendemails-> " + "sent email to abset error" + IsabsentDate + " " + IsabsentName + " " + IsabsetEmail);
        //            }

        //        }
        //    }
        //}
    }
}