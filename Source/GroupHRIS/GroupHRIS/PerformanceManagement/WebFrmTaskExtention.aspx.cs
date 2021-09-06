using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.PerformanceManagement;
using System.Data;
using GroupHRIS.Utility;
using Common;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmTaskExtention : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmTaskExtention : Page_Load");

            if (!IsPostBack)
            {
                fillStatus();
                fillYear();
                Session["tskId"] = "";
            }
            try
            {
                if (hfCaller.Value == "txtEmploeeId")
                {
                    hfCaller.Value = "";
                    txtEmploeeId.Text = hfVal.Value;

                    taskExtention(txtEmploeeId.Text,ddlYear.SelectedValue);

                    grdtsk.DataSource = null;
                    grdtsk.DataBind();
                    clear();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtEmploeeId.Text = "";
                clear();
                txtTotalcompletion.Text = "";
                txttargetDate.Text = "";
                Session["tskId"] = "";

                Errorhandler.ClearError(lblMessage);
                grdtsk.DataSource = null;
                grdtsk.DataBind();

                grdtskExtention.DataSource = null;
                grdtskExtention.DataBind();

                lbltskName.Text = "";
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string tskName = Session["tskId"].ToString();
            string extendedDate = txtextendedDate.Text;
            string reason = txtReason.Text;
            string status = ddlStatus.SelectedValue.ToString();
            string logUser = Session["KeyUSER_ID"].ToString();
            string year = ddlYear.SelectedValue;

            TaskExtentionDataHandler oTaskExtentionDataHandler = new TaskExtentionDataHandler();
            
            TaskProgressDataHandler oTaskProgressDataHandler = new TaskProgressDataHandler();

            if (tskName == "")
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Task should be select", lblMessage);
                return;
            }

            
            try
            {
                Errorhandler.ClearError(lblMessage);

                String targetDate = oTaskExtentionDataHandler.getTargetDate(txtEmploeeId.Text, tskName);
                String maxExTargetDate = oTaskExtentionDataHandler.getMaxTargetDate(txtEmploeeId.Text, tskName);

                DateTime dt = Convert.ToDateTime((targetDate));
                DateTime dt1 = Convert.ToDateTime(CommonUtils.dateFormatChange(extendedDate));
                TimeSpan ts = dt1.Subtract(dt);
                string vlidateDate = ts.Days.ToString();
                int numVal = Int32.Parse(vlidateDate);

                //DateTime maxTdt = Convert.ToDateTime(maxExTargetDate);
                //TimeSpan tsMax = dt1.Subtract(maxTdt);
                //string vlidateMaxDate = tsMax.Days.ToString();
                //int extCount = Int32.Parse(vlidateMaxDate);

                if (numVal < 0)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Extended date cannot less than target date.", lblMessage);
                    return;
                }

                DateTime now = DateTime.Now;

                if (dt1 < now)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Extended date should be grater than or equal to current date.", lblMessage);
                    return;
                }

                string maxProgress = oTaskProgressDataHandler.getMaxProgress(Session["tskId"].ToString());
                if (maxProgress != "")
                {
                    double proVal = Double.Parse(maxProgress);

                    if (proVal == 100)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Task is already completed.", lblMessage);
                        return;
                    }
                }

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {

                    String mxTargetDate = oTaskExtentionDataHandler.getMaxTargetDate(txtEmploeeId.Text, tskName);

                    if (mxTargetDate != "")
                    {
                        int extCount = maxDate(mxTargetDate);

                        if (extCount <= 0)
                        {
                            Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Extended date should be greater than extended target date", lblMessage);
                            return;
                        }
                    }

                    Boolean isSuccess = oTaskExtentionDataHandler.Insert(tskName, extendedDate, reason, status, logUser);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblMessage);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    int extCount = secondMaxDate();
                    

                    if (extCount < 0)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Extended date cannot less than last extended target date.", lblMessage);
                        return;
                    }
                    string tskExtId = Session["TaskExtId"].ToString();
                    string taskId = Session["tskId"].ToString();
                    Boolean isSuccess = oTaskExtentionDataHandler.Update(taskId, tskExtId, extendedDate, reason, status, logUser);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);

                }

                clear();
                taskExtention(txtEmploeeId.Text.ToString(), year);;

                grdtsk.DataSource = oTaskExtentionDataHandler.getTaskExtentionList(tskName);
                grdtsk.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdtskExtention_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdtskExtention.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);
                taskExtention(txtEmploeeId.Text.ToString(), ddlYear.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }

        }

        protected void grdtskExtention_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdtskExtention, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void grdtskExtention_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            try
            {
                txtReason.Text = "";
                txtextendedDate.Text = "";
                ddlStatus.SelectedIndex = 0;

                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                int SelectedIndex = grdtskExtention.SelectedIndex;
                //Session["TaskExtId"] = Server.HtmlDecode(grdtskExtention.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());

                string tskName = Server.HtmlDecode(grdtskExtention.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());

                txttargetDate.Text = Server.HtmlDecode(grdtskExtention.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
                txtTotalcompletion.Text = Server.HtmlDecode(grdtskExtention.Rows[SelectedIndex].Cells[5].Text.ToString().Trim());

                string extendedDate = Server.HtmlDecode(grdtskExtention.Rows[SelectedIndex].Cells[4].Text.ToString().Trim());

                if (extendedDate.Trim() != "")
                {
                    hfexDate.Value = CommonUtils.dateFormatChange(extendedDate);
                }
                else
                {
                    hfexDate.Value = "";
                }

                //string status = Server.HtmlDecode(grdtskExtention.Rows[SelectedIndex].Cells[5].Text.ToString().Trim());
                //ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));

                string tskId = Server.HtmlDecode(grdtskExtention.Rows[SelectedIndex].Cells[7].Text.ToString().Trim());
                Session["tskId"] = tskId.ToString();
                TaskExtentionDataHandler oTaskExtentionDataHandler = new TaskExtentionDataHandler();

                grdtsk.DataSource = oTaskExtentionDataHandler.getTaskExtentionList(tskId);
                grdtsk.DataBind();

                lbltskName.Text = "Task Name: " + tskName;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }

        }

        protected void ddlTaskName_SelectedIndexChanged(object sender, EventArgs e)
        {
            TaskExtentionDataHandler oTaskExtentionDataHandler = new TaskExtentionDataHandler();

            try
            {
                txttargetDate.Text = oTaskExtentionDataHandler.getTargetDate(txtEmploeeId.Text, lbltskName.Text);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oTaskExtentionDataHandler = null;
            }
        }

        protected void grdtsk_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdtsk, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void grdtsk_SelectedIndexChanged(object sender, EventArgs e)
        {
            TaskExtentionDataHandler oTaskExtentionDataHandler = new TaskExtentionDataHandler();

            try
            {
                Errorhandler.ClearError(lblMessage);
                int SelectedIndex = grdtsk.SelectedIndex;
                string tskName = Session["tskId"].ToString();

                string extendedDate = Server.HtmlDecode(grdtsk.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());

                txtextendedDate.Text = CommonUtils.dateFormatChange(extendedDate);

                txtReason.Text = Server.HtmlDecode(grdtsk.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
                String maxExTargetDate = oTaskExtentionDataHandler.getMaxTargetDate(txtEmploeeId.Text, tskName);
                hfexDate.Value = maxExTargetDate;

                if (hfexDate.Value == extendedDate || hfexDate.Value == null || hfexDate.Value == "")
                {
                    txtextendedDate.Enabled = true;
                    txtReason.Enabled = true;
                    btnSave.Enabled = true;
                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                }
                else
                {
                    txtextendedDate.Enabled = false;
                    txtReason.Enabled = false;
                    btnSave.Enabled = false;
                    btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                }

                Session["TaskExtId"] = Server.HtmlDecode(grdtsk.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());
                
                string status = Server.HtmlDecode(grdtsk.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(status));

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oTaskExtentionDataHandler = null;
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                taskExtention(txtEmploeeId.Text, ddlYear.SelectedValue);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
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

        public void clear()
        {
            txtTotalcompletion.Text = "";
            txttargetDate.Text = "";
            ddlStatus.SelectedIndex = 0;
            txtextendedDate.Text = "";
            txtReason.Text = "";
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            btnSave.Enabled = true;

            grdtsk.DataSource = null;
            grdtsk.DataBind();
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

        public int maxDate(string mxTargetDate)
        {
            TaskExtentionDataHandler oTaskExtentionDataHandler = new TaskExtentionDataHandler();
            int extCount = 0;
            try
            {
                string tskName = Session["tskId"].ToString();
                string extendedDate = txtextendedDate.Text;

                DateTime dt1 = Convert.ToDateTime(CommonUtils.dateFormatChange(extendedDate));
                if (mxTargetDate.Trim() != "")
                {
                    DateTime maxTdt = Convert.ToDateTime(mxTargetDate);
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
                oTaskExtentionDataHandler = null;
            }
            return extCount;
        }

        public int secondMaxDate()
        {
            TaskExtentionDataHandler oTaskExtentionDataHandler = new TaskExtentionDataHandler();
            int extCount = 0;
            try
            {
                string tskName = Session["tskId"].ToString();
                string extendedDate = txtextendedDate.Text;

                String targetDate = oTaskExtentionDataHandler.getTargetDate(txtEmploeeId.Text, tskName);
                String maxExTargetDate = oTaskExtentionDataHandler.getSecondMaxTargetDate(txtEmploeeId.Text, tskName);

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
                oTaskExtentionDataHandler = null;
            }
            return extCount;
        }

        protected void taskExtention(string empId, string year)
        {
            TaskExtentionDataHandler oTaskExtentionDataHandler = new TaskExtentionDataHandler();
            
            try
            {
                clear();
                grdtskExtention.DataSource = oTaskExtentionDataHandler.GetTasksExtentions(empId, year);
                grdtskExtention.DataBind();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                oTaskExtentionDataHandler = null;
            }
        }


        #endregion

    }
}