using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.PerformanceManagement;
using System.Data;
using Common;
using GroupHRIS.Utility;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmTaskProgress : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        #region Functions

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmTaskProgress : Page_Load");

            if (!IsPostBack)
            {
                fillStatus();
                fillYear();
                enable(false);
            }
            try
            {
                if (hfCaller.Value == "txtEmploeeId")
                {
                    hfCaller.Value = "";
                    txtEmploeeId.Text = hfVal.Value;

                    taskProgress(txtEmploeeId.Text.ToString(), ddlYear.SelectedValue.ToString());
                    Clear();

                    Errorhandler.ClearError(lblMessage);
                    grdprogress.DataSource = null;
                    grdprogress.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }

        }

        protected void grdtskProgress_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdtskProgress.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);

                taskProgress(txtEmploeeId.Text.ToString(), ddlYear.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdtskProgress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdtskProgress, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string tskName = Session["tskId"].ToString();
            string obDate = txtObservedDate.Text;
            string obProgress = txtProgress.Text;
            string obRemarks = txtRemarks.Text;
            string status = ddlStatus.SelectedValue.ToString();
            string logUser = Session["KeyUSER_ID"].ToString();

            TaskProgressDataHandler oTaskProgressDataHandler = new TaskProgressDataHandler();

            try
            {
                DateTime now = DateTime.Now;
                DateTime dt1 = Convert.ToDateTime(CommonUtils.dateFormatChange(obDate));
                DateTime pstDate = Convert.ToDateTime(CommonUtils.dateFormatChange(Session["planDate"].ToString())); //Session["planDate"]

                double numVal = Double.Parse(obProgress);

                if (numVal > 100)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Progress Should be less than 100 ", lblMessage);
                    return;
                }
                int extCount = maxDate();

                if ((dt1 > now && dt1 != now))
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Observed date should be greater than last Start date and less than or equal to current date", lblMessage);
                    return;
                }

                if (pstDate > dt1)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Observed date should be greater than Plan Start date", lblMessage);
                    return;
                }


                Errorhandler.ClearError(lblMessage);

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    string maxProgress = oTaskProgressDataHandler.getMaxProgress(Session["tskId"].ToString());
                    if (maxProgress != "")
                    {
                        double proVal = Double.Parse(maxProgress);
                        if (numVal < proVal)
                        {
                            Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Progress should be greater than max progress", lblMessage);
                            return;
                        }
                        if (proVal == 100)
                        {
                            Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Task is already completed.", lblMessage);
                            return;
                        }
                    }

                    Boolean isSuccess = oTaskProgressDataHandler.Insert(tskName, obDate, obProgress, obRemarks, status, logUser);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblMessage);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string maxBeforProgress = oTaskProgressDataHandler.getMaxBeforProgress(Session["tskId"].ToString());
                    if (maxBeforProgress != "")
                    {
                        double proVal = Double.Parse(maxBeforProgress);
                        if (numVal < proVal)
                        {
                            Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Progress Should be grater than last max progress.", lblMessage);
                            return;
                        }
                    }
                    string lineId = Session["LineId"].ToString();
                    string taskId = Session["tskId"].ToString();
                    Boolean isSuccess = oTaskProgressDataHandler.Update(lineId, taskId, obDate, obProgress, obRemarks, status, logUser);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
                }
                taskProgress(txtEmploeeId.Text.ToString(), ddlYear.SelectedValue.ToString());

                grdprogress.DataSource = oTaskProgressDataHandler.getTaskProgressList(Session["tskId"].ToString());
                grdprogress.DataBind();

                Clear();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oTaskProgressDataHandler = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
                txtEmploeeId.Text = "";
                Errorhandler.ClearError(lblMessage);
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;

                grdtskProgress.DataSource = null;
                grdtskProgress.DataBind();

                grdprogress.DataSource = null;
                grdprogress.DataBind();

                lbltskName.Text = "";
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            TaskProgressDataHandler oTaskProgressDataHandler = new TaskProgressDataHandler();
            try
            {
                DataTable dt = oTaskProgressDataHandler.getTaskProgress(txtEmploeeId.Text.ToString(), ddlYear.SelectedValue.ToString());
                if (dt.Rows.Count > 0)
                {
                    grdtskProgress.DataSource = dt;
                    grdtskProgress.DataBind();
                }
                else
                {
                    grdtskProgress.DataSource = null;
                    grdtskProgress.DataBind();
                }
                Clear();
                lbltskName.Text = "";
                Errorhandler.ClearError(lblMessage);
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;

                grdprogress.DataSource = null;
                grdprogress.DataBind();

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oTaskProgressDataHandler = null;
            }
        }

        protected void grdprogress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdprogress, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void grdtskProgress_SelectedIndexChanged1(object sender, EventArgs e)
        {
            TaskProgressDataHandler oTaskProgressDataHandler = new TaskProgressDataHandler();
            try
            {
                Errorhandler.ClearError(lblMessage);
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                enable(true);
                txtObservedDate.Text = "";
                txtProgress.Text = "";
                txtRemarks.Text = "";

                int SelectedIndex = grdtskProgress.SelectedIndex;
                Session["tskId"] = Server.HtmlDecode(grdtskProgress.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());
                string tskName = Server.HtmlDecode(grdtskProgress.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
                string targetDate = Server.HtmlDecode(grdtskProgress.Rows[SelectedIndex].Cells[4].Text.ToString().Trim());

                if (targetDate != null)
                {
                    targetDate = CommonUtils.dateFormatChange(targetDate);
                }
                txttargetDate.Text = targetDate;

                string planDate = Server.HtmlDecode(grdtskProgress.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());

                if (planDate != null)
                {
                    planDate = CommonUtils.dateFormatChange(planDate);
                }
                Session["planDate"] = planDate;

                //string status = Server.HtmlDecode(grdtskProgress.Rows[SelectedIndex].Cells[5].Text.ToString().Trim());
                //ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));

                grdprogress.DataSource = oTaskProgressDataHandler.getTaskProgressList(Session["tskId"].ToString());
                grdprogress.DataBind();

                lbltskName.Text = "Task Name: " + tskName;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oTaskProgressDataHandler = null;
            }

        }

        protected void grdprogress_SelectedIndexChanged1(object sender, EventArgs e)
        {
            TaskProgressDataHandler oTaskProgressDataHandler = new TaskProgressDataHandler();

            try
            {
                Errorhandler.ClearError(lblMessage);
                int SelectedIndex = grdprogress.SelectedIndex;

                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

                string observDate = Server.HtmlDecode(grdprogress.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());

                if (observDate != null)
                {
                    observDate = CommonUtils.dateFormatChange(observDate);
                }
                txtObservedDate.Text = observDate;
                txtProgress.Text = Server.HtmlDecode(grdprogress.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
                Session["LineId"] = Server.HtmlDecode(grdprogress.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());
                txtRemarks.Text = Server.HtmlDecode(grdprogress.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());

                string status = Server.HtmlDecode(grdprogress.Rows[SelectedIndex].Cells[4].Text.ToString().Trim());
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(status));

                string maxProgress = oTaskProgressDataHandler.getMaxProgress(Session["tskId"].ToString());

                //double maxprog = Double.Parse(maxProgress);
                //double txtpro = Double.Parse(txtObservedDate.Text);

                if (txtProgress.Text == maxProgress)
                {
                    enable(true);
                }
                else
                {
                    enable(false);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdprogress_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TaskProgressDataHandler oTaskProgressDataHandler = new TaskProgressDataHandler();
            try
            {
                grdprogress.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);

                grdprogress.DataSource = oTaskProgressDataHandler.getTaskProgressList(Session["tskId"].ToString());
                grdprogress.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oTaskProgressDataHandler = null;
            }
        }

        #endregion


        #region Methods

        protected void taskProgress(string empId, string year)
        {
            TaskProgressDataHandler oTaskProgressDataHandler = new TaskProgressDataHandler();
            try
            {
                grdtskProgress.DataSource = oTaskProgressDataHandler.getTaskProgress(empId, year); ;
                grdtskProgress.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oTaskProgressDataHandler = null;
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

        public void Clear()
        {
            //ddlgoalArea.SelectedIndex = 0;
            //ddlTaskname.SelectedIndex = 0;
            txtObservedDate.Text = "";
            txtProgress.Text = "";
            txtRemarks.Text = "";
            ddlStatus.SelectedIndex = 0;
            txttargetDate.Text = "";
            enable(false);
            //ddlTaskname.Enabled = true;

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
            catch (Exception)
            {

                throw;
            }
        }

        public void enable(bool status)
        {
            txtObservedDate.Enabled = status;
            txtProgress.Enabled = status;
            txtRemarks.Enabled = status;
            ddlStatus.Enabled = status;
            btnSave.Enabled = status;
        }

        public int maxDate()
        {
            TaskProgressDataHandler oTaskProgressDataHandler = new TaskProgressDataHandler();
            int extCount = 0;
            try
            {
                string tskName = Session["tskId"].ToString();
                string extendedDate = txtObservedDate.Text;

                // String targetDate = oTaskExtentionDataHandler.getTargetDate(txtEmploeeId.Text, tskName);
                String maxExTargetDate = oTaskProgressDataHandler.getMaxTargetDate(txtEmploeeId.Text, tskName);

                DateTime dt1 = Convert.ToDateTime(CommonUtils.dateFormatChange(extendedDate));
                if (maxExTargetDate.Trim() != "")
                {
                    DateTime maxTdt = Convert.ToDateTime(maxExTargetDate);
                    TimeSpan tsMax = dt1.Subtract(maxTdt);
                    string vlidateMaxDate = tsMax.Days.ToString();
                    extCount = Int32.Parse(vlidateMaxDate);
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                oTaskProgressDataHandler = null;
            }
            return extCount;
        }

        #endregion



    }
}