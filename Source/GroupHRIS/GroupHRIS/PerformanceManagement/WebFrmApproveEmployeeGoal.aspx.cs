using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.PerformanceManagement;
using GroupHRIS.Utility;
using System.Data;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmApproveEmployeeGoal : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmApproveEmployeeGoal : Page_Load");

            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            string KeyHRIS_ROLE = (string)Session["KeyHRIS_ROLE"];
            if (!IsPostBack)
            {
                fillYear();
                string year = ddlYear.SelectedValue;
                fillEmployeeGoals(KeyEMPLOYEE_ID, year);

                disableSupervisor(false);
                updateTotalWeight(KeyEMPLOYEE_ID, year);
                ifFinalize();
            }

            if (IsPostBack)
            {
                if (hfCaller.Value == "txtEmpId")
                {
                    lblTotWeight.Text = "";
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtEmpId.Text = hfVal.Value;
                        string isReporter = getReporter(KeyEMPLOYEE_ID, hfVal.Value);

                        if (isReporter != "")
                        {
                            fillEmployeeGoals(hfVal.Value, ddlYear.SelectedValue);
                            updateTotalWeight(hfVal.Value, ddlYear.SelectedValue);
                            enableEmployeeFields(false);
                            disableSupervisor(true);
                            clear();
                            ifFinalize();
                        }
                        else
                        {
                            grdGoal.DataSource = null;
                            grdGoal.DataBind();
                            disableSupervisor(false);
                            clear();
                        }
                    }

                }
            }


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

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                string taskYear = ddlYear.SelectedValue;

                fillEmployeeGoals(KeyEMPLOYEE_ID, taskYear);

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            GoalAgreementDataHandler GADH = new GoalAgreementDataHandler();

            string status = "0";
            string reason = txtReason.Text;
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            string taskYear = ddlYear.SelectedValue;

            try
            {
                if (chkAgree.Checked == true)
                {
                    status = "1";
                }
                if (hfGoalId.Value == "")
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Please select a Goal", lblMessage);
                    return;
                }

                if (chksConfirm.Checked == true || chksDisagree.Checked == true)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Cannot change the status after supervisor confirmed/ rejected", lblMessage);
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
                
                if (txtEmpId.Text != "")
                {
                    Boolean isSuccess = GADH.Update(hfGoalId.Value, status, reason, txtEmpId.Text, taskYear, KeyEMPLOYEE_ID);
                    fillEmployeeGoals(txtEmpId.Text, taskYear);
                    updateTotalWeight(txtEmpId.Text, taskYear);
                }
                else
                {
                    Boolean isSuccess = GADH.Update(hfGoalId.Value, status, reason, KeyEMPLOYEE_ID, taskYear, KeyEMPLOYEE_ID);
                    fillEmployeeGoals(KeyEMPLOYEE_ID, taskYear);
                    updateTotalWeight(KeyEMPLOYEE_ID, taskYear);
                } 
                Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                GADH = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            clear();
        }

        protected void btnsUpdate_Click(object sender, EventArgs e)
        {
            GoalAgreementDataHandler GADH = new GoalAgreementDataHandler();

            string status = "0";
            string reason = txtsComment.Text;
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            string taskYear = ddlYear.SelectedValue;

            try
            {
                if (chkAgree.Checked == false && chkDisagree.Checked == false)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Subordinate need to agree/reject the goal.", lblMessage);
                    return;
                }

                if (chkDisagree.Checked == true && chksConfirm.Checked == true)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Subordinate disagreed, Can not confim.", lblMessage);
                    return;
                }

                if (chksConfirm.Checked == false && chksDisagree.Checked == false)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Please select Confirm/Reject", lblMessage);
                    return;
                }

                if (chksDisagree.Checked == true && txtsComment.Text.Trim() == "")
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Rejected comment is required", lblMessage);
                    return;
                }

                if (chksConfirm.Checked == true)
                {
                    status = "1";
                }

                if (txtEmpId.Text != "" && hfGoalId.Value != "")
                {
                    Boolean isSuccess = GADH.UpdateSupervisor(hfGoalId.Value, status, reason, txtEmpId.Text, taskYear, KeyEMPLOYEE_ID);
                    fillEmployeeGoals(txtEmpId.Text, taskYear);
                    updateTotalWeight(txtEmpId.Text, taskYear);
                }
                else
                {
                    Boolean isSuccess = GADH.UpdateSupervisor(hfGoalId.Value, status, reason, KeyEMPLOYEE_ID, taskYear, KeyEMPLOYEE_ID);
                    fillEmployeeGoals(KeyEMPLOYEE_ID, taskYear);
                    updateTotalWeight(KeyEMPLOYEE_ID, taskYear);
                }
                Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                GADH = null;
            }
        }

        protected void btnsClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected void chkAgree_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkAgree.Checked == true)
                {
                    chkAgree.Checked = true;
                    chkDisagree.Checked = false;
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

        protected void chkDisagree_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkDisagree.Checked == true)
                {
                    chkDisagree.Checked = true;
                    chkAgree.Checked = false;
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

        protected void chksConfirm_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chksConfirm.Checked == true)
                {
                    chksConfirm.Checked = true;
                    chksDisagree.Checked = false;
                }
                else
                {
                    chksDisagree.Checked = false;
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void chksDisagree_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chksDisagree.Checked == true)
                {
                    chksDisagree.Checked = true;
                    chksConfirm.Checked = false;
                }
                else
                {
                    chksConfirm.Checked = false;
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdGoal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdGoal, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void grdGoal_SelectedIndexChanged(object sender, EventArgs e)
        {
            GoalAgreementDataHandler GADH = new GoalAgreementDataHandler();
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            string year = ddlYear.SelectedValue;
            Errorhandler.ClearError(lblMessage);

            chkAgree.Checked = false;
            chkDisagree.Checked = false;
            txtReason.Text = "";

            try
            {
                int SelectedIndex = grdGoal.SelectedIndex;
                string goalId = Server.HtmlDecode(grdGoal.Rows[SelectedIndex].Cells[0].Text.ToString());
                hfGoalId.Value = goalId;

                if (txtEmpId.Text != "")
                {
                    KeyEMPLOYEE_ID = txtEmpId.Text.Trim();
                }
                DataTable dtGoal = GADH.getGoalDetails(KeyEMPLOYEE_ID, year,goalId);

                txtGoalArea.Text = dtGoal.Rows[0]["GOAL_AREA"].ToString().Replace("<br />",Environment.NewLine);
                txtMeasurement.Text = dtGoal.Rows[0]["MEASUREMENTS"].ToString().Replace("<br />", Environment.NewLine);
                txtWeight.Text = dtGoal.Rows[0]["WEIGHT"].ToString().Replace("<br />", Environment.NewLine);
                txtDescription.Text = dtGoal.Rows[0]["DESCRIPTION"].ToString().Replace("<br />", Environment.NewLine);
                txtReason.Text = dtGoal.Rows[0]["EMPLOYEE_REASON"].ToString().Replace("<br />", Environment.NewLine);
                txtsComment.Text = dtGoal.Rows[0]["SUPERVISOR_REASON"].ToString().Replace("<br />", Environment.NewLine);

                string isAgree = Server.HtmlDecode(grdGoal.Rows[SelectedIndex].Cells[3].Text.ToString());

                if (isAgree == "Agreed")
                {
                    chkAgree.Checked = true;
                }
                else if (isAgree == "Disagreed")
                {
                    chkDisagree.Checked = true;
                }

                string isApprove = Server.HtmlDecode(grdGoal.Rows[SelectedIndex].Cells[4].Text.ToString());
                if (isApprove == "Confirmed")
                {
                    chksConfirm.Checked = true;
                    chksDisagree.Checked = false;
                }
                else if (isApprove == "Rejected")
                {
                    chksDisagree.Checked = true;
                    chksConfirm.Checked = false;
                }
                else
                {
                    chksConfirm.Checked = false;
                    chksDisagree.Checked = false;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void grdGoal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                grdGoal.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);

                fillEmployeeGoals(KeyEMPLOYEE_ID, ddlYear.SelectedValue);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }
        
        protected void btnlock_Click(object sender, EventArgs e)
        {
            GoalAgreementDataHandler GADH = new GoalAgreementDataHandler();

            try
            {
                string year = ddlYear.SelectedValue;
                string empId = (string)(Session["KeyEMPLOYEE_ID"]);

                if (txtEmpId.Text != "")
                {
                    empId = txtEmpId.Text;
                }

                List<string> goalList = listGoal(empId, year);
                bool status = GADH.isLock(goalList, empId, year);
                Boolean isFinalize = GADH.UpdateIsFinalize(goalList,empId, year);

                if (isFinalize)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) already Finalize", lblMessage);
                    
                }
                else
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully finalized", lblMessage);
                    btnlock.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                GADH = null;
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtEmpId.Text != "")
            {
                fillEmployeeGoals(txtEmpId.Text, ddlYear.SelectedValue);
            }
            else
            {
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                fillEmployeeGoals(KeyEMPLOYEE_ID, ddlYear.SelectedValue);
            }
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

        protected void fillEmployeeGoals(string empId,string year)
        {
            GoalAgreementDataHandler GADH = new GoalAgreementDataHandler();

            try
            {
                grdGoal.DataSource = GADH.getGoalList(empId, year);
                grdGoal.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                GADH = null;
            }
        }

        private string getReporter(string sSupervisor, string sSubordinate)
        {
            GoalAgreementDataHandler GADH = new GoalAgreementDataHandler();
            string sReporter = "";

            try
            {
                sReporter = GADH.isReporter(sSubordinate, sSupervisor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                GADH = null;
            }

            return sReporter;
        }

        public void enableEmployeeFields(bool status)
        {
            chkAgree.Enabled = status;
            chkDisagree.Enabled = status;
            txtReason.Enabled = status;
            btnUpdate.Visible = status;
            btnClear.Visible = status;
        }

        public void disableSupervisor(bool status)
        {
            chksConfirm.Enabled = status;
            chksDisagree.Enabled = status;
            txtsComment.Enabled = status;
            btnsUpdate.Visible = status;
            btnsClear.Visible = status;
            btnlock.Visible = status;
        }

        public void clear()
        {
            txtGoalArea.Text = "";
            txtMeasurement.Text = "";
            txtWeight.Text = "";
            txtDescription.Text = "";
            chkAgree.Checked = false;
            chkDisagree.Checked = false;
            txtReason.Text = "";
            chksConfirm.Checked = false;
            chksDisagree.Checked = false;
            txtsComment.Text = "";
            hfGoalId.Value = "";
        }

        public void updateTotalWeight(string empId, string year)
        {
            GoalAgreementDataHandler GADH = new GoalAgreementDataHandler();
            DataTable dt = new DataTable();

            double totWeight = 0;

            try
            {
                dt = GADH.getGoalList(empId, year);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string empAgree = dt.Rows[i]["EMPLOYEE_AGREE"].ToString();
                    string supervisorAgree = dt.Rows[i]["SUPERVISOR_AGREE"].ToString();

                    if (empAgree == "Agreed" && supervisorAgree == "Confirmed")
                    {
                        string weight = dt.Rows[i]["WEIGHT"].ToString();
                        //string goalId = dt.Rows[i]["GOAL_ID"].ToString();
                        totWeight = double.Parse(weight) + totWeight;
                    }
                }

                if (totWeight == 100)
                {
                    lblTotWeight.Text = "Total weight of the goals are equal to 100.";
                    lblTotWeight.ForeColor = System.Drawing.Color.Blue;
                    btnlock.Enabled = true;
                    
                }
                else
                {
                    lblTotWeight.Text = "Total weight of the goals are not equal to 100. Total weight is " + totWeight;
                    btnlock.Enabled = false;
                }
                
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public List<string> listGoal(string empId, string year)
        {
            GoalAgreementDataHandler GADH = new GoalAgreementDataHandler();
            DataTable dt = new DataTable();
            List<string> list = new List<string>();
            //double totWeight = 0;

            try
            {
                dt = GADH.getGoalList(empId, year);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string empAgree = dt.Rows[i]["EMPLOYEE_AGREE"].ToString();
                    string supervisorAgree = dt.Rows[i]["SUPERVISOR_AGREE"].ToString();

                    if (empAgree == "Agreed" && supervisorAgree == "Confirmed")
                    {
                        string weight = dt.Rows[i]["WEIGHT"].ToString();
                        string goalId = dt.Rows[i]["GOAL_ID"].ToString();
                        //totWeight = double.Parse(weight) + totWeight;
                        list.Add(goalId);
                    }
                }
                return list;
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void isFinalized(bool status)
        {
            //btnlock.Enabled = status;
            btnsUpdate.Enabled = status;
            btnsClear.Enabled = status;
            txtsComment.Enabled = status;
            chksConfirm.Enabled = status;
            chksDisagree.Enabled = status;
            btnUpdate.Enabled = status;
            btnClear.Enabled = status;
            txtReason.Enabled = status;
            chkDisagree.Enabled = status;
            chkAgree.Enabled = status;
        }

        public void ifFinalize()
        {

            GoalAgreementDataHandler GADH = new GoalAgreementDataHandler();
            DataTable dt = new DataTable();
            string year = ddlYear.SelectedValue;
            string empId = (string)(Session["KeyEMPLOYEE_ID"]);

            if (txtEmpId.Text != "")
            {
                empId = txtEmpId.Text;
            }
            double totWeight = 0;

            try
            {
                dt = GADH.getGoalList(empId, year);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string empAgree = dt.Rows[i]["EMPLOYEE_AGREE"].ToString();
                    string supervisorAgree = dt.Rows[i]["SUPERVISOR_AGREE"].ToString();

                    if (empAgree == "Agreed" && supervisorAgree == "Confirmed")
                    {
                        string weight = dt.Rows[i]["WEIGHT"].ToString();
                        totWeight = double.Parse(weight) + totWeight;
                    }
                }

                if (totWeight == 100)
                {
                    List<string> goalList = listGoal(empId, year);
                    //Check isLosk equal 1
                    bool status = GADH.isLock(goalList, empId, year);
                    if (status == true)
                    {
                        isFinalized(false);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        
    }
}