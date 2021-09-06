using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.PerformanceManagement;
using Common;
using GroupHRIS.Utility;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmTaskDashBoard : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmTaskDashBoard : Page_Load");

            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            string KeyHRIS_ROLE = (string)Session["KeyHRIS_ROLE"];
            string isReporter = "";

            if (!IsPostBack)
            {
                fillYear();
                fillEmployeeStatus();
                fillSupervisorStatus();
                string year = ddlYear.SelectedValue;
                txtEmpId.Text = KeyEMPLOYEE_ID;

                fillEmployeeTasks(KeyEMPLOYEE_ID, year);
                disableSupervisor(false);

                if (KeyHRIS_ROLE == Constants.CON_COMMON_KeyHRIS_ROLE)
                {
                    lblEmp.Visible = false;
                    txtEmpId.Visible = false;
                    searchEmp.Visible = false;
                    disableSupervisor(false);
                }
                else
                {
                    lblEmp.Visible = true;
                    txtEmpId.Visible = true;
                    searchEmp.Visible = true;
                }
            }

            if (IsPostBack)
            {
                if (hfCaller.Value == "txtEmpId")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtEmpId.Text = hfVal.Value;
                    }
                    if (txtEmpId.Text != "")
                    {
                        isReporter = getReporter(KeyEMPLOYEE_ID, hfVal.Value);

                        if (isReporter != "")
                        {
                            fillEmployeeTasks(hfVal.Value, ddlYear.SelectedValue);

                            enableFields(false);
                            disableSupervisor(true);
                        }
                        else if(KeyEMPLOYEE_ID == hfVal.Value)
                        {
                            fillEmployeeTasks(KeyEMPLOYEE_ID, ddlYear.SelectedValue);
                            disableSupervisor(false);
                            enableFields(true);
                        }
                        else
                        {
                            //fillEmployeeTasks(hfVal.Value, ddlYear.SelectedValue);
                            grdTask.DataSource = null;
                            grdTask.DataBind();
                            disableSupervisor(false);
                        }
                        clear();
                    }
                }

               


                //if (isReporter != "")
                //{
                //    fillEmployeeTasks(hfVal.Value, ddlYear.SelectedValue);
                //    enableFields(false);
                //    disableSupervisor(true);
                //}
            }
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                string taskYear = ddlYear.SelectedValue;

                fillEmployeeTasks(KeyEMPLOYEE_ID, taskYear);

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            TaskDashboardDataHandler TDDH = new TaskDashboardDataHandler();

            string status = "0";
            string reason = txtReason.Text;
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            string taskYear = ddlYear.SelectedValue;

            if (hfTaskId.Value == "")
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Please select a Task", lblMessage);
                return;
            }

            if (chkApprove.Checked == true || chkReject.Checked == true)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Supervisor Confirmed/Disagreed, You can not change", lblMessage);
                return;
            }

            if (chkAgree.Checked == false && chkDisagree.Checked == false)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Please select Agree/Disagree", lblMessage);
                return;
            }

            if (chkDisagree.Checked == true && txtReason.Text.Trim() == "")
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Reason is required", lblMessage);
                return;
            }

            try
            {
                if (chkAgree.Checked == true)
                {
                    status = "1";
                }
                Boolean isSuccess = TDDH.Update(hfTaskId.Value, status, reason, KeyEMPLOYEE_ID);
                Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);

                if (txtEmpId.Text != "")
                {
                    fillEmployeeTasks(txtEmpId.Text, taskYear);
                }
                else
                {
                    fillEmployeeTasks(KeyEMPLOYEE_ID, taskYear);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TDDH = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
                hfTaskId.Value = "";
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdTask_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdTask, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMessage);
            }
        }

        protected void grdTask_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                grdTask.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);

                fillEmployeeTasks(KeyEMPLOYEE_ID, ddlYear.SelectedValue);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            TaskDashboardDataHandler TDDH = new TaskDashboardDataHandler();

            Errorhandler.ClearError(lblMessage);

            chkAgree.Checked = false;
            chkDisagree.Checked = false;
            txtReason.Text = "";

            try
            {
                int SelectedIndex       = grdTask.SelectedIndex;
                hfTaskId.Value          = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[1].Text.ToString());
                txtTask.Text            = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[2].Text.ToString());
                txtStartDate.Text       = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[3].Text.ToString());
                txtTargetDate.Text      = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[4].Text.ToString());
                txtDescription.Text     = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[5].Text.ToString());

                string goalId = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[0].Text.ToString());
                txtGoal.Text = TDDH.getGoal(goalId);
                string isAgree = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[6].Text.ToString());

                if (isAgree == "Agreed")
                {
                    chkAgree.Checked = true;
                }
                else if (isAgree == "Disagreed")
                {
                    chkDisagree.Checked = true;
                }

                txtReason.Text = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[7].Text.ToString());
                string isApprove = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[8].Text.ToString());
                if (isApprove == "Confirmed")
                {
                    chkApprove.Checked = true;
                    chkReject.Checked = false;
                }
                else if (isApprove == "Disagreed")
                {
                    chkReject.Checked = true;
                    chkApprove.Checked = false;
                }
                else
                {
                    chkApprove.Checked = false;
                    chkReject.Checked = false;
                }

                txtSupervisorComment.Text = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[9].Text.ToString());

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TDDH = null;
            }
        }

        protected void chkAgree_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkAgree.Checked == true)
                {
                    chkDisagree.Checked = false;
                }
                else 
                {
                    chkAgree.Checked = false;
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void chkDisagree_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkDisagree.Checked == true)
                {
                    chkAgree.Checked = false;
                }
                else
                {
                    chkDisagree.Checked = false;
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void chkApprove_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkApprove.Checked == true)
                {
                    chkReject.Checked = false;
                }
                else
                {
                    chkApprove.Checked = false;
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void chkReject_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkReject.Checked == true)
                {
                    chkApprove.Checked = false;
                }
                else
                {
                    chkReject.Checked = false;
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void btnsUpdate_Click(object sender, EventArgs e)
        {
            TaskDashboardDataHandler TDDH = new TaskDashboardDataHandler();

            try
            {
                string reason = txtSupervisorComment.Text;
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                string taskYear = ddlYear.SelectedValue;
                string status = "0";

                if (chkAgree.Checked != true && chkDisagree.Checked != true)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You can not Confirm/Reject untill subordinate Agree/Reject", lblMessage);
                    return;
                }

                if (chkDisagree.Checked == true && chkApprove.Checked == true)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You can not Confirm task", lblMessage);
                    return;
                }

                if (chkApprove.Checked == false && chkReject.Checked == false)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Please select Confirm/Reject", lblMessage);
                    return;
                }

                if (chkReject.Checked == true && txtSupervisorComment.Text.Trim() == "")
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Supervisor’s comment is required", lblMessage);
                    return;
                }

                if (chkApprove.Checked == true)
                {
                    status = "1";
                }

                if (hfTaskId.Value != "")
                {
                    Boolean isSuccess = TDDH.UpdateIsAgree(hfTaskId.Value, status, reason, KeyEMPLOYEE_ID);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);

                    //fillEmployeeTasks(KeyEMPLOYEE_ID, taskYear);
                    if (txtEmpId.Text != "")
                    {
                        fillEmployeeTasks(txtEmpId.Text, taskYear);
                    }
                    else
                    {
                        fillEmployeeTasks(KeyEMPLOYEE_ID, taskYear);
                    }
                }
                else
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Please select the Task", lblMessage);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TDDH = null;
            }


        }

        protected void btnsClear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
                txtSupervisorComment.Text = "";
                hfTaskId.Value = "";
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void ddlempStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            TaskDashboardDataHandler TDDH = new TaskDashboardDataHandler();
            try
            {
                ddlsupStatus.SelectedIndex = 0;
                string empStatus = ddlempStatus.SelectedValue.ToString();
                grdTask.DataSource = TDDH.getEmployeeTaskStatus(txtEmpId.Text, ddlYear.SelectedValue, empStatus);
                grdTask.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TDDH = null;
            }
        }

        protected void ddlsupStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            TaskDashboardDataHandler TDDH = new TaskDashboardDataHandler();
            try
            {
                ddlempStatus.SelectedIndex = 0;
                string supStatus = ddlsupStatus.SelectedValue.ToString();
                grdTask.DataSource = TDDH.getSupervisorTaskStatus(txtEmpId.Text, ddlYear.SelectedValue, supStatus);
                grdTask.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TDDH = null;
            }
        }

        protected void imgRefresh_Click(object sender, ImageClickEventArgs e)
        {
            TaskDashboardDataHandler TDDH = new TaskDashboardDataHandler();
            try
            {
                ddlsupStatus.SelectedIndex = 0;
                ddlempStatus.SelectedIndex = 0;

                if (txtEmpId.Text != "")
                {
                    grdTask.DataSource = TDDH.getEmployeeTask(txtEmpId.Text, ddlYear.SelectedValue);
                    grdTask.DataBind();
                }
                else
                {
                    string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                    grdTask.DataSource = TDDH.getEmployeeTask(KeyEMPLOYEE_ID, ddlYear.SelectedValue);
                    grdTask.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TDDH = null;
            }
        }



        private string getReporter(string sSupervisor, string sSubordinate)
        {
            TaskDashboardDataHandler TDDH = new TaskDashboardDataHandler();
            string sReporter = "";

            try
            {
                sReporter = TDDH.isReporter(sSubordinate,sSupervisor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                TDDH = null;
            }

            return sReporter;
        }

        protected void fillEmployeeTasks(string sEmployeeId, String sYear)
        {
            TaskDashboardDataHandler TDDH = new TaskDashboardDataHandler();

            try
            {
                grdTask.DataSource = TDDH.getEmployeeTask(sEmployeeId, sYear);
                grdTask.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                TDDH = null;
            }
        }

        public void clear()
        {
            txtGoal.Text = "";
            txtDescription.Text = "";
            txtReason.Text = "";
            txtStartDate.Text = "";
            txtTargetDate.Text = "";
            txtTask.Text = "";
            chkAgree.Checked = false;
            chkDisagree.Checked = false; 
            txtSupervisorComment.Text = "";
            chkApprove.Checked = false;
            chkReject.Checked = false;

            Errorhandler.ClearError(lblMessage);

        }

        public void fillYear()
        {
            try
            {
                ddlYear.Items.Clear();
                string currentYear = (CommonUtils.currentFinancialYear());
                int dt = Int32.Parse(currentYear);

                for (int i = 0; i >= -5; i--)
                {
                    // Now just add an entry that's the current year plus the counter
                    ddlYear.Items.Add((dt + i).ToString());
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        public void enableFields(bool status)
        {
            chkAgree.Enabled = status;
            chkDisagree.Enabled = status;
            txtReason.Enabled = status;
            btnSave.Visible = status;
            btnClear.Visible = status;
        }

        public void disableSupervisor(bool status)
        {
            txtSupervisorComment.Enabled = status;
            lblsComment.Enabled = status;
            btnsClear.Visible = status;
            btnsUpdate.Visible = status;
            chkApprove.Enabled = status;
            chkReject.Enabled = status;
        }

        public void fillEmployeeStatus()
        {
            try
            {
                ddlempStatus.Items.Insert(0, new ListItem("", ""));
                ddlempStatus.Items.Insert(1, new ListItem(Constants.STATUS_AGREE_TAG, Constants.STATUS_AGREE_VALUE));
                ddlempStatus.Items.Insert(2, new ListItem(Constants.STATUS_DISAGREE_TAG, Constants.STATUS_DISAGREE_VALUE));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void fillSupervisorStatus()
        {
            try
            {
                ddlsupStatus.Items.Insert(0, new ListItem("", ""));
                ddlsupStatus.Items.Insert(1, new ListItem(Constants.STATUS_CONFIRM_TAG, Constants.STATUS_CONFIRM_VALUE));
                ddlsupStatus.Items.Insert(2, new ListItem(Constants.STATUS_REJECT_TAG, Constants.STATUS_REJECT_VALUE));
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            TaskDashboardDataHandler TDDH = new TaskDashboardDataHandler();

            try
            {
                if (txtEmpId.Text != "")
                {
                    grdTask.DataSource = TDDH.getEmployeeTask(txtEmpId.Text, ddlYear.SelectedValue);
                    grdTask.DataBind();
                }
                else
                {
                    string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                    grdTask.DataSource = TDDH.getEmployeeTask(KeyEMPLOYEE_ID, ddlYear.SelectedValue);
                    grdTask.DataBind();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

    }
}