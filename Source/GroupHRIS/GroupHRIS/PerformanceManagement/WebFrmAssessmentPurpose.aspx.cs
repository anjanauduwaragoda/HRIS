using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using GroupHRIS.Utility;
using DataHandler.PerformanceManagement;
using System.Data;
using DataHandler.Utility;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmAssessmentPurpose : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmAssessmentPurpose : Page_Load");
            try
            {
                if (Session["KeyLOGOUT_STS"] == null)
                {
                    Response.Redirect("MainLogout.aspx", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login/SessionExpior.aspx", false);
            }
            if (!IsPostBack)
            {
                fillStatus();
                loadAssessmentPurposeGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            string PurposeName = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string status = ddlStatus.SelectedItem.Value.ToString();
            string addedUser = Session["KeyUSER_ID"].ToString();

            AssessmentPurposeDataHandler assessmentPurposeDataHandler = new AssessmentPurposeDataHandler();
            UtilsDataHandler utilsDataHandler = new UtilsDataHandler();
            try
            {


                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    if (utilsDataHandler.isDuplicateExist(PurposeName, "NAME", "ASSESSMENT_PURPOSE"))
                    {
                        CommonVariables.MESSAGE_TEXT = "Purpose name is already exist";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                    else
                    {

                        bool isSuccess = assessmentPurposeDataHandler.Insert(PurposeName, description, status, addedUser);
                        if (isSuccess)
                        {
                            CommonVariables.MESSAGE_TEXT = "Record(s) successfully saved.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            clearFrom();
                            loadAssessmentPurposeGrid();
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Record(s) could not be saved.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }
                    }
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string purposeId = AssessmentPurposeGrid.SelectedRow.Cells[0].Text;

                    if (utilsDataHandler.isDuplicateExist(PurposeName, "NAME", "ASSESSMENT_PURPOSE", purposeId, "PURPOSE_ID"))
                    {
                        CommonVariables.MESSAGE_TEXT = "Purpose name is already exist";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                    else
                    {
                        bool isSuccess = assessmentPurposeDataHandler.Update(purposeId, PurposeName, description, status, addedUser);
                        if (isSuccess)
                        {                            
                            CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            clearFrom();
                            loadAssessmentPurposeGrid();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                assessmentPurposeDataHandler = null;
                utilsDataHandler = null;
            
            }

            
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");
            try
            {
                clearFrom();
                Utility.Errorhandler.ClearError(lblErrorMsg);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void loadAssessmentPurposeGrid()
        {
            log.Debug("loadAssessmentPurposeGrid()");

            AssessmentPurposeDataHandler assessmentPurposeDataHandler = new AssessmentPurposeDataHandler();

            DataTable assesmentPurposeTable = new DataTable();
            try
            {
                assesmentPurposeTable = assessmentPurposeDataHandler.Populate();
                AssessmentPurposeGrid.DataSource = assesmentPurposeTable;
                AssessmentPurposeGrid.DataBind();
            }
            catch (Exception ex)
            {

                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }

            finally
            {
                assessmentPurposeDataHandler = null;                
            }

        }

        protected void AssessmentPurposeGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("AssessmentPurposeGrid_PageIndexChanging()");
            AssessmentPurposeGrid.PageIndex = e.NewPageIndex;
            loadAssessmentPurposeGrid();
            clearFrom();
            Utility.Errorhandler.ClearError(lblErrorMsg);
        }

        protected void AssessmentPurposeGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("AssessmentPurposeGrid_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.AssessmentPurposeGrid, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void AssessmentPurposeGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("AssessmentPurposeGrid_SelectedIndexChanged()");

            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblErrorMsg);
            AssessmentPurposeDataHandler assessmentPurposeDataHandler = new AssessmentPurposeDataHandler();
            DataRow dataRow = null;
            try
            {
                string purposeId = AssessmentPurposeGrid.SelectedRow.Cells[0].Text;
                dataRow = assessmentPurposeDataHandler.GetElementById(purposeId);

                if (dataRow != null)
                {
                    txtName.Text = dataRow["NAME"].ToString().Trim();
                    txtDescription.Text = dataRow["DESCRIPTION"].ToString().Trim();
                    string status = dataRow["STATUS_CODE"].ToString().Trim();
                    ddlStatus.SelectedValue = status;

                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                assessmentPurposeDataHandler = null;
                dataRow = null;
            }
        }
       
        private void fillStatus()
        {
            log.Debug("fillStatus()");
            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlStatus.Items.Add(listItemBlank);

                ListItem listItemActive = new ListItem();
                listItemActive.Text = Constants.STATUS_ACTIVE_TAG;
                listItemActive.Value = Constants.STATUS_ACTIVE_VALUE;
                ddlStatus.Items.Add(listItemActive);

                ListItem listItemInActive = new ListItem();
                listItemInActive.Text = Constants.STATUS_INACTIVE_TAG;
                listItemInActive.Value = Constants.STATUS_INACTIVE_VALUE;
                ddlStatus.Items.Add(listItemInActive);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void clearFrom()
        {
            log.Debug("clearFrom()");
            try
            {
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                Utils.clearControls(true, txtName, txtDescription, ddlStatus);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }

        }
    }
}