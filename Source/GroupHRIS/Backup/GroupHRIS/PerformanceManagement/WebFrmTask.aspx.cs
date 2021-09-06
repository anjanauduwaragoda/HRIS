using System;
using System.Web.UI.WebControls;
using DataHandler.PerformanceManagement;
using Common;
using GroupHRIS.Utility;
using System.Text.RegularExpressions;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmTask : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sIPAddress = Request.UserHostAddress;
                log.Debug("IP:" + sIPAddress + "WebFrmTask : Page_Load");

                Utility.Errorhandler.ClearError(lblMessage);

                string KeyHRIS_ROLE = (string)Session["KeyHRIS_ROLE"];
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);

                if (!IsPostBack)
                {
                    fillYear();
                    fillStatus();
                    clear();

                    grdGoalArea.DataSource = null;
                    grdGoalArea.DataBind();

                    if (KeyHRIS_ROLE == Constants.CON_COMMON_KeyHRIS_ROLE)
                    {
                        txtEmploeeId.Enabled = false;
                        txtEmploeeId.Text = KeyEMPLOYEE_ID;
                        searchEmp.Visible = false;
                        fillGoalList(KeyEMPLOYEE_ID, ddlYear.SelectedValue);
                    }
                    else
                    {
                        txtEmploeeId.Enabled = false;
                        searchEmp.Visible = true;
                    }
                    fillTasks(txtEmploeeId.Text.ToString(), ddlYear.SelectedValue);
                }

                if (hfCaller.Value == "txtEmploeeId")
                {
                    hfCaller.Value = "";
                    Errorhandler.ClearError(lblMessage);
                    txtEmploeeId.Text = hfVal.Value;
                    clear();
                    fillGoalList(txtEmploeeId.Text.ToString(), ddlYear.SelectedValue);
                    fillTasks(txtEmploeeId.Text.ToString(), ddlYear.SelectedValue);
                }

                if (txtEmploeeId.Text != "")
                {

                }


            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");
            string logUser = Session["KeyUSER_ID"].ToString();
            string taskName = txtTaskname.Text;
            string goalName = hfglId.Value;
            string year = ddlYear.SelectedValue.ToString();
            string description = txtDescription.Text;
            string tDate = txtTargetDate.Text;
            string status = ddlStatus.SelectedValue.ToString();
            string empID = txtEmploeeId.Text;
            string planedStDate = txtPlanStDate.Text;
            string actualStDate = txtactualStDate.Text;
            string remarks = txtRemarks.Text;

            TaskDataHandler oTaskDataHandler = new TaskDataHandler();

            int diffDate = checkDatevalidation(tDate); //ts.Days;//int NoOfDays=dt.Days;

            int diffPlanedDt = checkDatevalidation(planedStDate);
            int diffActualDt = checkDatevalidation(actualStDate);

            try
            {
                Errorhandler.ClearError(lblMessage);

                if (hfglId.Value == "")
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Need to select goal area", lblMessage);
                    return;
                }

                if (diffDate < diffPlanedDt)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Target date should be greater than Planed start date", lblMessage);
                    return;
                }

                if (CommonUtils.currentFinancialYear().ToString() != year)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Need to select current financial year", lblMessage);
                    return;
                }

                if (diffDate < 0 || diffPlanedDt < 0)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Planed start date can not less than current date", lblMessage);
                    return;
                }
                

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    Boolean isSuccess = oTaskDataHandler.Insert(taskName, goalName, year, description, tDate, status, logUser, planedStDate, actualStDate, remarks);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblMessage);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    if (Session["supervisorAgree"].ToString() != "Pending")
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Can not update,Supervisor Approved/Rejected", lblMessage);
                        return;
                    }

                    string taskId = Session["tskId"].ToString();
                    Boolean isSuccess = oTaskDataHandler.Update(taskId, taskName, goalName, year, description, tDate, status, logUser, actualStDate, remarks);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
                }
                clear();
                fillTasks(empID, year);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oTaskDataHandler = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");
            string KeyHRIS_ROLE = (string)Session["KeyHRIS_ROLE"];
            try
            {
                clear();
                updateDisable(true);

                txtTargetDate.Enabled = true;

                if (KeyHRIS_ROLE != Constants.CON_COMMON_KeyHRIS_ROLE)
                {
                    txtEmploeeId.Text = "";

                    grdTask.DataSource = null;
                    grdTask.DataBind();

                    grdGoalArea.DataSource = null;
                    grdGoalArea.DataBind();
                }
                Errorhandler.ClearError(lblMessage);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdTask_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("grdTask_PageIndexChanging()");
            TaskDataHandler oTaskDataHandler = new TaskDataHandler();
            try
            {
                grdTask.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);

                fillTasks(txtEmploeeId.Text.ToString(), ddlYear.SelectedValue);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oTaskDataHandler = null;
            }

        }

        protected void grdTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("grdTask_SelectedIndexChanged()");
            TaskDataHandler oTaskDataHandler = new TaskDataHandler();
            Errorhandler.ClearError(lblMessage);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

            try
            {
                int SelectedIndex = grdTask.SelectedIndex;
                hfglId.Value = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[11].Text.ToString());

                //fillGoalList(txtEmploeeId.Text.ToString(), ddlYear.SelectedValue);

                for (int i = 0; i < grdGoalArea.Rows.Count; i++)
                {
                    if (grdGoalArea.Rows[i].Cells[0].Text.ToString() == hfglId.Value)
                    {
                        CheckBox chk = (grdGoalArea.Rows[i].Cells[2].FindControl("chkBxSelect") as CheckBox);
                        chk.Checked = true;
                    }
                    else
                    {
                        CheckBox chk = (grdGoalArea.Rows[i].Cells[2].FindControl("chkBxSelect") as CheckBox);
                        chk.Checked = false;
                    }
                }

                txtTaskname.Text = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[1].Text.ToString());
                ddlYear.SelectedValue = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[2].Text.ToString());
                string trgetDate = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[5].Text.ToString().Trim());

                if (trgetDate != null)
                {
                    string[] dateArr = trgetDate.Split('/', '-');
                    trgetDate = dateArr[2] + "/" + dateArr[1] + "/" + dateArr[0];
                }
                txtTargetDate.Text = trgetDate;

                string planStDate = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
                planStDate = Regex.Replace(planStDate, @"\s", "");
                if (planStDate != null && planStDate != "")
                {
                    string[] dateArr = planStDate.Split('/', '-');
                    planStDate = dateArr[2] + "/" + dateArr[1] + "/" + dateArr[0];
                }
                txtPlanStDate.Text = planStDate;

                string actualStDate = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[4].Text.ToString().Trim());
                actualStDate = Regex.Replace(actualStDate, @"\s", "");
                if (actualStDate.Trim() != null && actualStDate != "")
                {
                    string[] dateArr = actualStDate.Split('/', '-');
                    actualStDate = dateArr[2] + "/" + dateArr[1] + "/" + dateArr[0];
                }
                txtactualStDate.Text = actualStDate;

                string extTargetDate = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[6].Text.ToString());
                extTargetDate = Regex.Replace(extTargetDate, @"\s", "");
                if (extTargetDate != null && extTargetDate != "")
                {
                    string[] dateArr = extTargetDate.Split('/', '-');
                    extTargetDate = dateArr[2] + "/" + dateArr[1] + "/" + dateArr[0];
                }
                lblExtendedTDate.Text = extTargetDate;

                //lblExtendedTDate.Text = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[6].Text.ToString());

                lblCompletion.Text = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[7].Text.ToString());


                txtDescription.Text = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[8].Text.ToString().Trim());

                string status = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[9].Text.ToString().Trim());
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));

                Session["TskId"] = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[10].Text.ToString().Trim());

                if (lblCompletion.Text.ToString().Trim() != "")
                {
                    double numVal = Double.Parse(lblCompletion.Text.ToString());

                    if (numVal == 100)
                    {
                        updateDisable(false);
                    }
                    else
                    {
                        updateDisable(true);
                    }
                }


                string extDate = lblExtendedTDate.Text.ToString().Trim();
                if (extDate != "")
                {
                    txtTargetDate.Enabled = false;
                }
                else
                {
                    txtTargetDate.Enabled = true;
                }
                //txtPlanStDate.Enabled = false;
                txtRemarks.Text = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[12].Text.ToString().Trim());
                Session["supervisorAgree"] = Server.HtmlDecode(grdTask.Rows[SelectedIndex].Cells[14].Text.ToString().Trim());
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oTaskDataHandler = null;
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

        protected void chkBxSelect_CheckedChanged(object sender, EventArgs e)
        {
            log.Debug("chkBxSelect_CheckedChanged()");
            TaskDataHandler oTaskDataHandler = new TaskDataHandler();

            try
            {
                int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;

                CheckBox cb = (CheckBox)grdGoalArea.Rows[selRowIndex].FindControl("chkBxSelect");

                string galName = Server.HtmlDecode(grdGoalArea.Rows[selRowIndex].Cells[0].Text.ToString().Trim());
                if (cb.Checked == false)
                {
                    galName = "";
                }
                grdTask.DataSource = oTaskDataHandler.GetTasksForSelectedGoal(galName, txtEmploeeId.Text);
                grdTask.DataBind();

                for (int i = 0; i < grdGoalArea.Rows.Count; i++)
                {
                    if (i != selRowIndex)
                    {
                        CheckBox chk = (grdGoalArea.Rows[i].Cells[2].FindControl("chkBxSelect") as CheckBox);
                        chk.Checked = false;
                    }
                }

                hfglId.Value = galName;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oTaskDataHandler = null;
            }


        }

        protected void grdGoalArea_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("grdGoalArea_PageIndexChanging()");
            TaskDataHandler oTaskDataHandler = new TaskDataHandler();
            try
            {
                grdGoalArea.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);

                fillGoalList(txtEmploeeId.Text.ToString(), ddlYear.SelectedValue);

                //grdTask.DataSource = null;
                //grdTask.DataBind();

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oTaskDataHandler = null;
            }
        }

        protected void linkviewAlltsk_Click(object sender, EventArgs e)
        {
            log.Debug("linkviewAlltsk_Click()");
            TaskDataHandler oTaskDataHandler = new TaskDataHandler();
            try
            {
                clearcheckBox();
                if (txtEmploeeId.Text != "")
                {
                    Errorhandler.ClearError(lblMessage);

                    fillTasks(txtEmploeeId.Text.ToString(), ddlYear.SelectedValue);

                    if (grdTask.Rows.Count == 0)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "No Task Found", lblMessage);
                    }
                }
                else
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Select Employee", lblMessage);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oTaskDataHandler = null;
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlYear_SelectedIndexChanged()");
            TaskDataHandler oTaskDataHandler = new TaskDataHandler();
            try
            {
                txtTargetDate.Enabled = true;
                updateDisable(true);

                fillGoalList(txtEmploeeId.Text.ToString(), ddlYear.SelectedValue);

                fillTasks(txtEmploeeId.Text.ToString(), ddlYear.SelectedValue);

                clear();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oTaskDataHandler = null;
            }
        }

        #endregion


        #region Methods

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
            catch (Exception)
            {

                throw;
            }
        }

        public void fillStatus()
        {
            try
            {
                ddlStatus.Items.Insert(0, new ListItem("", ""));
                ddlStatus.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                ddlStatus.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void clear()
        {

            hfCaller.Value = "";
            hfglId.Value = "";
            txtTaskname.Text = "";
            txtTargetDate.Text = "";
            txtDescription.Text = "";
            ddlStatus.SelectedIndex = 0;
            lblCompletion.Text = "";
            lblExtendedTDate.Text = "";
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            btnSave.Enabled = true;
            txtPlanStDate.Enabled = true;
            txtPlanStDate.Text = "";
            txtactualStDate.Text = "";
            txtRemarks.Text = "";
        }

        public void updateDisable(bool status)
        {
            txtTaskname.Enabled = status;
            txtTargetDate.Enabled = status;
            txtDescription.Enabled = status;
            ddlStatus.Enabled = status;
            btnSave.Enabled = status;
        }

        public int checkDatevalidation(string dt)
        {
            int diffDate = 0;
            try
            {
                DateTime startdate;
                DateTime enddate;
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                startdate = DateTime.ParseExact(dt, "dd/MM/yyyy", theCultureInfo);
                enddate = DateTime.Now;
                TimeSpan ts = startdate.Subtract(enddate);
                diffDate = ts.Days;//int NoOfDays=dt.Days;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            return diffDate;
        }

        private void fillGoalList(string sEmployeeId, string sYear)
        {
            TaskDataHandler oTaskDataHandler = new TaskDataHandler();
            try
            {
                grdGoalArea.DataSource = oTaskDataHandler.getGoalList(sEmployeeId, sYear).Copy();
                grdGoalArea.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oTaskDataHandler = null;
            }

        }

        private void fillTasks(string sEmployeeId, string sYear)
        {
            TaskDataHandler oTaskDataHandler = new TaskDataHandler();
            try
            {
                grdTask.DataSource = oTaskDataHandler.GetTasks(sEmployeeId, sYear);
                grdTask.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oTaskDataHandler = null;
            }

        }

        private void clearcheckBox()
        {
            try
            {
                for (int i = 0; i < grdGoalArea.Rows.Count; i++)
                {
                    CheckBox chk = (grdGoalArea.Rows[i].Cells[2].FindControl("chkBxSelect") as CheckBox);
                    chk.Checked = false;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion





    }
}