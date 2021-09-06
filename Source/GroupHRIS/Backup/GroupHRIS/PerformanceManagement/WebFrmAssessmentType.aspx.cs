using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using GroupHRIS.Utility;
using DataHandler;
using DataHandler.Utility;
using DataHandler.PerformanceManagement;
using System.Data;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmAssessmentType : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmAssessmentType : Page_Load");

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
                loadAssessmentTypeGrid();
            }
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            string assesmentTypeName = txtName.Text;
            string description = txtDescription.Text;
            string status = ddlStatus.SelectedItem.Value.ToString();
            string addedUser = Session["KeyUSER_ID"].ToString();

            AssessmentTypeDataHandler assessmentTypeDataHandler = new AssessmentTypeDataHandler();
            UtilsDataHandler utilsDataHandler = new UtilsDataHandler();


            try
            {
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    if (utilsDataHandler.isDuplicateExist(assesmentTypeName, "ASSESSMENT_TYPE_NAME", "ASSESSMENT_TYPE"))
                    {
                        CommonVariables.MESSAGE_TEXT = "Assessment type already exist";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                    else
                    {

                        if (assessmentTypeDataHandler.Insert(assesmentTypeName, description, status, addedUser))
                        {
                            CommonVariables.MESSAGE_TEXT = "Record(s) successfully saved .";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }
                        clearFrom();
                        loadAssessmentTypeGrid();
                    }
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string assessmentTypeId = AssessmentTypeGrid.SelectedRow.Cells[0].Text;
                    DataTable dtUsedAssessment = assessmentTypeDataHandler.getUsedAssessments(assessmentTypeId);



                    if (dtUsedAssessment.Rows.Count > 0)
                    {
                        if (status != Constants.ASSESSNEMT_ACTIVE_STATUS)
                        {
                            DataRow[] unclosedAssessments = dtUsedAssessment.Select("STATUS_CODE <>'" + Constants.ASSESSNEMT_CLOSED_STATUS + "' and STATUS_CODE <>'" + Constants.ASSESSNEMT_OBSOLETE_STATUS + "' ");
                            if (unclosedAssessments.Count() > 0)
                            {

                                CommonVariables.MESSAGE_TEXT = "Cannot inactivate. Unclosed assessments exist";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            }
                            else
                            {
                                if (utilsDataHandler.isDuplicateExist(assesmentTypeName, "ASSESSMENT_TYPE_NAME", "ASSESSMENT_TYPE", assessmentTypeId, "ASSESSMENT_TYPE_ID"))
                                {
                                    CommonVariables.MESSAGE_TEXT = "Assessment type already exist";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                }
                                else
                                {
                                    assessmentTypeDataHandler.Update(assessmentTypeId, assesmentTypeName, description, status, addedUser);
                                    CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated.";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                    clearFrom();
                                    loadAssessmentTypeGrid();
                                }
                            }
                        }
                        //// activate assessment type
                        else
                        {
                            if (utilsDataHandler.isDuplicateExist(assesmentTypeName, "ASSESSMENT_TYPE_NAME", "ASSESSMENT_TYPE", assessmentTypeId, "ASSESSMENT_TYPE_ID"))
                            {
                                CommonVariables.MESSAGE_TEXT = "Assessment type already exist";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            }
                            else
                            {                                
                                assessmentTypeDataHandler.Update(assessmentTypeId, status, addedUser);
                                CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated.";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                clearFrom();
                                loadAssessmentTypeGrid();
                            }
                        }
                    }
                    else
                    {
                        if (utilsDataHandler.isDuplicateExist(assesmentTypeName, "ASSESSMENT_TYPE_NAME", "ASSESSMENT_TYPE", assessmentTypeId, "ASSESSMENT_TYPE_ID"))
                        {
                            CommonVariables.MESSAGE_TEXT = "Assessment type already exist";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }
                        else
                        {
                            assessmentTypeDataHandler.Update(assessmentTypeId, assesmentTypeName, description, status, addedUser);
                            CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            clearFrom();
                            loadAssessmentTypeGrid();
                        } 
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }                       
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");

            clearFrom();
            Utility.Errorhandler.ClearError(lblErrorMsg);
        }       

        protected void loadAssessmentTypeGrid()
        {
            log.Debug("loadAssessmentTypeGrid()");
            AssessmentTypeDataHandler assessmentTypeDataHandler = new AssessmentTypeDataHandler();

            DataTable assesmentTypeTable = new DataTable();
            try
            {
                assesmentTypeTable = assessmentTypeDataHandler.Populate();
                AssessmentTypeGrid.DataSource = assesmentTypeTable;
                AssessmentTypeGrid.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }

            finally
            {
                assessmentTypeDataHandler = null;
                assesmentTypeTable.Dispose();
            }

        }

        protected void AssessmentTypeGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("AssessmentTypeGrid_PageIndexChanging()");
            AssessmentTypeGrid.PageIndex = e.NewPageIndex;
            loadAssessmentTypeGrid();
            clearFrom();
            Utility.Errorhandler.ClearError(lblErrorMsg);
        }
       
        protected void AssessmentTypeGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("AssessmentTypeGrid_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.AssessmentTypeGrid, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void AssessmentTypeGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("AssessmentTypeGrid_SelectedIndexChanged()");
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblErrorMsg);
            AssessmentTypeDataHandler assessmentTypeDataHandler = new AssessmentTypeDataHandler();
            DataRow dataRow = null;
            try
            {
                string assessmentTypeId = AssessmentTypeGrid.SelectedRow.Cells[0].Text;
                dataRow = assessmentTypeDataHandler.GetElementById(assessmentTypeId);

                if (dataRow != null)
                {
                    txtName.Text = dataRow["ASSESSMENT_TYPE_NAME"].ToString().Trim();
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
                assessmentTypeDataHandler = null;
                dataRow = null;
            }
        }

        private void fillStatus()
        {
            log.Debug("fillStatus()");
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

        private void clearFrom()
        {
            log.Debug("clearFrom()");

            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            Utils.clearControls(true, txtName, txtDescription, ddlStatus);
        }
    }
}