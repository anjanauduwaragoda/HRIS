using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using Common;
using DataHandler.TrainingAndDevelopment;
using System.Data;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainingRequestOfTraining : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        #region Methods

        void LoadTraining()
        {
            TrainingRequestofTrainingDataHandler TRTDH = new TrainingRequestofTrainingDataHandler();
            DataTable dtTraining = new DataTable();
            try
            {
                dtTraining = TRTDH.Populate(txtTrainingID.Text.Trim()).Copy();

                if (dtTraining.Rows.Count > 0)
                {
                    lblTrainingName.Text = dtTraining.Rows[0]["TRAINING_NAME"].ToString();
                    lblTrainingCode.Text = dtTraining.Rows[0]["TRAINING_CODE"].ToString();
                    lblTrainingProgram.Text = dtTraining.Rows[0]["PROGRAM_NAME"].ToString();
                    lblTrainingType.Text = dtTraining.Rows[0]["TYPE_NAME"].ToString();
                    lblPlannedParticipants.Text = dtTraining.Rows[0]["PLANNED_PARTICIPANTS"].ToString();
                    lblPlannedStartDate.Text = dtTraining.Rows[0]["PLANNED_START_DATE"].ToString();
                    lblPlannedEndDate.Text = dtTraining.Rows[0]["PLANNED_END_DATE"].ToString();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                dtTraining.Dispose();
                TRTDH = null;
            }
        }

        void LoadTrainingRequestDetails()
        {
            TrainingRequestofTrainingDataHandler TRTDH = new TrainingRequestofTrainingDataHandler();
            DataTable dtTrainingRequest = new DataTable();
            try
            {
                if (grdvTrainingRequest.Rows.Count > 0)
                {
                    int index = grdvTrainingRequest.SelectedIndex;

                    for (int i = 0; i < grdvTrainingRequest.Rows.Count; i++)
                    {
                        if (i == index)
                        {
                            grdvTrainingRequest.Rows[i].Attributes.Add("style", "font-weight: bold; background-color: #C0C0C0; cursor:pointer;");
                        }
                        else
                        {
                            grdvTrainingRequest.Rows[i].Attributes.Add("style", " font-weight: normal; background-color: #FFFFFF; cursor:pointer;");
                        }
                    }
                }

                string TrainingRequestID = HttpUtility.HtmlDecode(grdvTrainingRequest.Rows[grdvTrainingRequest.SelectedIndex].Cells[0].Text).ToString();
                dtTrainingRequest = TRTDH.PopulateTrainingRequestDetails(TrainingRequestID).Copy();
                if (dtTrainingRequest.Rows.Count > 0)
                {
                    lblTrainingCategory.Text = dtTrainingRequest.Rows[0]["CATEGORY_NAME"].ToString();
                    lblRequestTrainingType.Text = dtTrainingRequest.Rows[0]["TYPE_NAME"].ToString();
                    lblCompany.Text = dtTrainingRequest.Rows[0]["COMP_NAME"].ToString();
                    lblDepartment.Text = dtTrainingRequest.Rows[0]["DEPT_NAME"].ToString();
                    lblDivision.Text = dtTrainingRequest.Rows[0]["DIV_NAME"].ToString();
                    lblBranch.Text = dtTrainingRequest.Rows[0]["BRANCH_NAME"].ToString();
                    lblTrainingRequestType.Text = dtTrainingRequest.Rows[0]["TYPE_NAME"].ToString();
                    lblEmployee.Text = dtTrainingRequest.Rows[0]["TITLE"].ToString() + " " + dtTrainingRequest.Rows[0]["INITIALS_NAME"].ToString();
                    lblDesignation.Text = dtTrainingRequest.Rows[0]["DESIGNATION_NAME"].ToString();
                    lblEmail.Text = dtTrainingRequest.Rows[0]["EMAIL"].ToString();
                    lblRequestReason.Text = dtTrainingRequest.Rows[0]["REASON"].ToString();
                    lbldescription.Text = dtTrainingRequest.Rows[0]["DESCRIPTION_OF_TRAINING"].ToString();
                    lblSkillsExpected.Text = dtTrainingRequest.Rows[0]["SKILLS_EXPECTED"].ToString();
                    NoOfParticipants.Text = dtTrainingRequest.Rows[0]["NUMBER_OF_PARTICIPANTS"].ToString();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                TRTDH = null;
            }
        }

        void ClearTrainingRequestDetails()
        {
            try
            {
                lblTrainingCategory.Text = String.Empty;
                lblRequestTrainingType.Text = String.Empty;
                lblCompany.Text = String.Empty;
                lblDepartment.Text = String.Empty;
                lblDivision.Text = String.Empty;
                lblBranch.Text = String.Empty;
                lblTrainingRequestType.Text = String.Empty;
                lblEmployee.Text = String.Empty;
                lblDesignation.Text = String.Empty;
                lblEmail.Text = String.Empty;
                lblRequestReason.Text = String.Empty;
                lbldescription.Text = String.Empty;
                lblSkillsExpected.Text = String.Empty;
                NoOfParticipants.Text = String.Empty;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {

            }
        }

        void LoadUnAssignRequests()
        {
            TrainingRequestofTrainingDataHandler TRTDH = new TrainingRequestofTrainingDataHandler();
            DataTable dtTrainingRequest = new DataTable();

            try
            {
                dtTrainingRequest = TRTDH.PopulateTrainingRequest().Copy();

                dtTrainingRequest.Columns.Add("IS_INCLUDE");
                dtTrainingRequest.Columns.Add("REMARKS");
                Session["dtTrainingRequest"] = dtTrainingRequest.Copy();

                grdvTrainingRequest.DataSource = dtTrainingRequest.Copy();
                grdvTrainingRequest.DataBind();

                if (dtTrainingRequest.Rows.Count > 0)
                {
                    btnAddToTraining.Visible = true;
                }
                else
                {
                    btnAddToTraining.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {

            }
        }

        void LoadAssignedTrainingRequest()
        {
            TrainingRequestofTrainingDataHandler TRTDH = new TrainingRequestofTrainingDataHandler();
            DataTable dtAssignedTrainingRequest = new DataTable();
            try
            {
                dtAssignedTrainingRequest = TRTDH.PopulateAssignedTrainingRequest(txtTrainingID.Text.Trim()).Copy();
                dtAssignedTrainingRequest.Columns.Add("IS_EXCLUDE");

                Session["dtAssignedTrainingRequest"] = dtAssignedTrainingRequest.Copy();
                grdvAssignedRequests.DataSource = dtAssignedTrainingRequest.Copy();
                grdvAssignedRequests.DataBind();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                TRTDH = null;
                dtAssignedTrainingRequest.Dispose();
            }
        }

        void DisplayDivs()
        {
            try
            {
                if ((txtTrainingID.Text != String.Empty) || (txtTrainingRequestID.Text != String.Empty))
                {
                    dv1.Visible = true;
                    dv2.Visible = true;
                    dv3.Visible = true;
                    Div4.Visible = true;
                    Div5.Visible = true;
                    Div6.Visible = true;
                }
                else
                {
                    dv1.Visible = false;
                    dv2.Visible = false;
                    dv3.Visible = false;
                    Div4.Visible = false;
                    Div5.Visible = false;
                    Div6.Visible = false;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sIPAddress = Request.UserHostAddress;
                log.Debug("IP:" + sIPAddress + "WebFrmTrainingRequestOfTraining : Page_Load");

                Utility.Errorhandler.ClearError(lblMessage);

                if (!IsPostBack)
                {
                    DataTable dtAssignedTrainingRequest = new DataTable();

                    dtAssignedTrainingRequest.Columns.Add("REQUEST_ID");
                    dtAssignedTrainingRequest.Columns.Add("DESCRIPTION_OF_TRAINING");
                    dtAssignedTrainingRequest.Columns.Add("IS_EXCLUDE");
                    dtAssignedTrainingRequest.Columns.Add("REMARKS");

                    Session["dtAssignedTrainingRequest"] = dtAssignedTrainingRequest.Copy();
                }

                if (IsPostBack)
                {
                    if (hfCaller.Value == "txtTrainingID")
                    {
                        hfCaller.Value = "";
                        if (hfVal.Value != "")
                        {
                            txtTrainingID.Text = hfVal.Value;
                        }
                        if (txtTrainingID.Text != "")
                        {
                            //Postback Methods
                            LoadTraining();
                            hfVal.Value = String.Empty;
                            LoadAssignedTrainingRequest();
                        }
                    }
                    if (hfCaller.Value == "txtTrainingRequestID")
                    {
                        hfCaller.Value = "";
                        if (hfVal.Value != "")
                        {
                            txtTrainingRequestID.Text = hfVal.Value;
                        }
                        if (txtTrainingRequestID.Text != "")
                        {
                            //Postback Methods
                            hfVal.Value = String.Empty;
                        }
                    }
                } 
                DisplayDivs();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Debug(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void chkisIncludeAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("chkisIncludeAll_CheckedChanged");

                Utility.Errorhandler.ClearError(lblMessage);

                DataTable dtTrainingRequest = new DataTable();
                dtTrainingRequest = (Session["dtTrainingRequest"] as DataTable).Copy();//IS_INCLUDE


                CheckBox chkisIncludeAll = (grdvTrainingRequest.HeaderRow.FindControl("chkisIncludeAll") as CheckBox);
                if (chkisIncludeAll.Checked == true)
                {
                    for (int i = 0; i < grdvTrainingRequest.Rows.Count; i++)
                    {
                        CheckBox chkisInclude = (grdvTrainingRequest.Rows[i].FindControl("chkisInclude") as CheckBox);
                        chkisInclude.Checked = true;
                        dtTrainingRequest.Rows[i]["IS_INCLUDE"] = Constants.CON_ACTIVE_STATUS;
                    }
                }
                else
                {
                    for (int i = 0; i < grdvTrainingRequest.Rows.Count; i++)
                    {
                        CheckBox chkisInclude = (grdvTrainingRequest.Rows[i].FindControl("chkisInclude") as CheckBox);
                        chkisInclude.Checked = false;
                        dtTrainingRequest.Rows[i]["IS_INCLUDE"] = Constants.CON_INACTIVE_STATUS;
                    }
                }

                Session["dtTrainingRequest"] = dtTrainingRequest.Copy();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Debug(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void chkisInclude_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("chkisInclude_CheckedChanged");

                Utility.Errorhandler.ClearError(lblMessage);

                DataTable dtTrainingRequest = new DataTable();
                dtTrainingRequest = (Session["dtTrainingRequest"] as DataTable).Copy();//IS_INCLUDE

                for (int i = 0; i < grdvTrainingRequest.Rows.Count; i++)
                {
                    CheckBox chkisInclude = (grdvTrainingRequest.Rows[i].FindControl("chkisInclude") as CheckBox);
                    string TrainingRequestID = HttpUtility.HtmlDecode(grdvTrainingRequest.Rows[i].Cells[0].Text).ToString().Trim();
                    DataRow[] drRequest = dtTrainingRequest.Select("REQUEST_ID = '" + TrainingRequestID + "'");
                    if (drRequest.Length > 0)
                    {
                        if (chkisInclude.Checked == true)
                        {
                            drRequest[0]["IS_INCLUDE"] = Constants.CON_ACTIVE_STATUS;
                        }
                        else
                        {
                            drRequest[0]["IS_INCLUDE"] = Constants.CON_INACTIVE_STATUS;
                        }
                    }
                }

                Session["dtTrainingRequest"] = dtTrainingRequest.Copy();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Debug(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void grdvTrainingRequest_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("grdvTrainingRequest_SelectedIndexChanged");

                Utility.Errorhandler.ClearError(lblMessage);

                LoadTrainingRequestDetails();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Debug(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void grdvTrainingRequest_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdvTrainingRequest, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");

                    string TrainingRequestID = e.Row.Cells[0].Text;

                    DataTable dtTrainingRequest = new DataTable();
                    dtTrainingRequest = (Session["dtTrainingRequest"] as DataTable).Copy();//IS_INCLUDE
                    DataRow[] drRequest = dtTrainingRequest.Select("REQUEST_ID = '" + TrainingRequestID + "'");

                    if (drRequest.Length > 0)
                    {
                        CheckBox chkisInclude = (e.Row.FindControl("chkisInclude") as CheckBox);
                        if (drRequest[0]["IS_INCLUDE"].ToString() == Constants.CON_ACTIVE_STATUS)
                        {
                            chkisInclude.Checked = true;
                        }
                        else
                        {
                            chkisInclude.Checked = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdvTrainingRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMessage);

            DataTable dtTrainingRequest = new DataTable();
            try
            {
                log.Debug("grdvTrainingRequest_PageIndexChanging");

                dtTrainingRequest = (Session["dtTrainingRequest"] as DataTable).Copy();
                grdvTrainingRequest.PageIndex = e.NewPageIndex;
                grdvTrainingRequest.DataSource = dtTrainingRequest.Copy();
                grdvTrainingRequest.DataBind();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Debug(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtTrainingRequest.Dispose();
            }
        }

        protected void chkUnAssignRequests_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("chkUnAssignRequests_CheckedChanged");
                Utility.Errorhandler.ClearError(lblMessage);

                if (chkUnAssignRequests.Checked == true)
                {
                    txtTrainingRequestID.Enabled = false;
                    imgRequestSearch.Visible = false;

                    LoadUnAssignRequests();
                }
                else
                {
                    txtTrainingRequestID.Enabled = true;
                    imgRequestSearch.Visible = true;

                    grdvTrainingRequest.DataSource = null;
                    grdvTrainingRequest.DataBind();

                    ClearTrainingRequestDetails();

                    btnAddToTraining.Visible = false;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Debug(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }// Load/unload Un Assign Training Requests

        protected void grdvAssignedRequests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string TrainingRequestID = e.Row.Cells[0].Text;

                    DataTable dtAssignedTrainingRequest = new DataTable();
                    dtAssignedTrainingRequest = (Session["dtAssignedTrainingRequest"] as DataTable).Copy();//IS_EXCLUDE
                    DataRow[] drRequest = dtAssignedTrainingRequest.Select("REQUEST_ID = '" + TrainingRequestID + "'");

                    if (drRequest.Length > 0)
                    {
                        CheckBox chkisInclude = (e.Row.FindControl("chkisExclude") as CheckBox);
                        if (drRequest[0]["IS_EXCLUDE"].ToString() == Constants.CON_ACTIVE_STATUS)
                        {
                            chkisInclude.Checked = true;
                        }
                        else
                        {
                            chkisInclude.Checked = false;
                        }
                    }

                    TextBox txtDescription = (e.Row.FindControl("txtDescription") as TextBox);
                    drRequest = dtAssignedTrainingRequest.Select("REQUEST_ID = '" + TrainingRequestID + "'");

                    if (drRequest[0]["REMARKS"].ToString().Trim() != String.Empty)
                    {
                        txtDescription.Text = drRequest[0]["REMARKS"].ToString();
                    }

                    drRequest[0]["REMARKS"] = txtDescription.Text.Trim();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdvAssignedRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                log.Debug("grdvAssignedRequests_PageIndexChanging");

                Utility.Errorhandler.ClearError(lblMessage);

                DataTable dtAssignedTrainingRequest = new DataTable();
                dtAssignedTrainingRequest = (Session["dtAssignedTrainingRequest"] as DataTable).Copy();

                for (int i = 0; i < grdvAssignedRequests.Rows.Count; i++)
                {
                    string RequestID = HttpUtility.HtmlDecode(grdvAssignedRequests.Rows[i].Cells[0].Text).Trim();
                    TextBox txtDescription = (grdvAssignedRequests.Rows[i].FindControl("txtDescription") as TextBox);

                    DataRow[] drRequest = dtAssignedTrainingRequest.Select("REQUEST_ID = '" + RequestID + "'");
                    drRequest[0]["REMARKS"] = txtDescription.Text.Trim();
                }

                grdvAssignedRequests.PageIndex = e.NewPageIndex;
                grdvAssignedRequests.DataSource = dtAssignedTrainingRequest.Copy();
                grdvAssignedRequests.DataBind();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Debug(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void chkisExcludeAll_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dtAssignedTrainingRequest = new DataTable();
            try
            {
                log.Debug("chkisExcludeAll_CheckedChanged");

                dtAssignedTrainingRequest = (Session["dtAssignedTrainingRequest"] as DataTable).Copy();//IS_EXCLUDE
                Utility.Errorhandler.ClearError(lblMessage);

                CheckBox chkisExcludeAll = (grdvAssignedRequests.HeaderRow.FindControl("chkisExcludeAll") as CheckBox);
                if (chkisExcludeAll.Checked == true)
                {
                    for (int i = 0; i < grdvAssignedRequests.Rows.Count; i++)
                    {
                        CheckBox chkisExclude = (grdvAssignedRequests.Rows[i].FindControl("chkisExclude") as CheckBox);
                        chkisExclude.Checked = true;
                        dtAssignedTrainingRequest.Rows[i]["IS_EXCLUDE"] = Constants.CON_ACTIVE_STATUS;
                    }
                }
                else
                {
                    for (int i = 0; i < grdvAssignedRequests.Rows.Count; i++)
                    {
                        CheckBox chkisExclude = (grdvAssignedRequests.Rows[i].FindControl("chkisExclude") as CheckBox);
                        chkisExclude.Checked = false;
                        dtAssignedTrainingRequest.Rows[i]["IS_EXCLUDE"] = Constants.CON_INACTIVE_STATUS;
                    }
                }
                Session["dtAssignedTrainingRequest"] = dtAssignedTrainingRequest.Copy();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Debug(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void chkisExclude_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("chkisExclude_CheckedChanged");

                Utility.Errorhandler.ClearError(lblMessage);

                DataTable dtAssignedTrainingRequest = new DataTable();
                dtAssignedTrainingRequest = (Session["dtAssignedTrainingRequest"] as DataTable).Copy();//IS_EXCLUDE

                for (int i = 0; i < grdvAssignedRequests.Rows.Count; i++)
                {
                    CheckBox chkisExclude = (grdvAssignedRequests.Rows[i].FindControl("chkisExclude") as CheckBox);
                    string TrainingRequestID = HttpUtility.HtmlDecode(grdvAssignedRequests.Rows[i].Cells[0].Text).ToString().Trim();
                    DataRow[] drRequest = dtAssignedTrainingRequest.Select("REQUEST_ID = '" + TrainingRequestID + "'");
                    if (drRequest.Length > 0)
                    {
                        if (chkisExclude.Checked == true)
                        {
                            drRequest[0]["IS_EXCLUDE"] = Constants.CON_ACTIVE_STATUS;
                        }
                        else
                        {
                            drRequest[0]["IS_EXCLUDE"] = Constants.CON_INACTIVE_STATUS;
                        }
                    }
                }

                Session["dtAssignedTrainingRequest"] = dtAssignedTrainingRequest.Copy();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Debug(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void btnAddToTraining_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("btnAddToTraining_Click");

                Utility.Errorhandler.ClearError(lblMessage);

                DataTable dtTrainingRequest = new DataTable();
                dtTrainingRequest = (Session["dtTrainingRequest"] as DataTable).Copy();//IS_INCLUDE

                Boolean isSelected = false;
                if (dtTrainingRequest.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTrainingRequest.Rows.Count; i++)
                    {
                        string isInclude = dtTrainingRequest.Rows[i]["IS_INCLUDE"].ToString();
                        if (isInclude == Constants.CON_ACTIVE_STATUS)
                        {
                            isSelected = true;
                        }
                    }
                    if (isSelected == false)
                    {
                        CommonVariables.MESSAGE_TEXT = "Please select at least one training request";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Training request(s) not found";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                    return;
                }

                if (txtTrainingID.Text == String.Empty)
                {
                    CommonVariables.MESSAGE_TEXT = "Please select a training";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                    return;
                }

                DataTable dtAssignedTrainingRequest = new DataTable();
                dtAssignedTrainingRequest = (Session["dtAssignedTrainingRequest"] as DataTable).Copy();

                for (int i = 0; i < dtTrainingRequest.Rows.Count; i++)
                {
                    string isInclude = dtTrainingRequest.Rows[i]["IS_INCLUDE"].ToString();
                    if (isInclude == Constants.CON_ACTIVE_STATUS)
                    {
                        string RequestID = dtTrainingRequest.Rows[i]["REQUEST_ID"].ToString();
                        DataRow[] AssignedRequest = dtAssignedTrainingRequest.Select("REQUEST_ID = '" + RequestID + "'");

                        if ((AssignedRequest.Length > 0) == false)
                        {
                            DataRow drNewAssignRecord = dtAssignedTrainingRequest.NewRow();
                            drNewAssignRecord["REQUEST_ID"] = dtTrainingRequest.Rows[i]["REQUEST_ID"].ToString();
                            drNewAssignRecord["DESCRIPTION_OF_TRAINING"] = dtTrainingRequest.Rows[i]["DESCRIPTION_OF_TRAINING"].ToString();
                            drNewAssignRecord["IS_EXCLUDE"] = Constants.CON_INACTIVE_STATUS;
                            dtAssignedTrainingRequest.Rows.Add(drNewAssignRecord);
                        }
                    }
                }
                Session["dtAssignedTrainingRequest"] = dtAssignedTrainingRequest.Copy();
                grdvAssignedRequests.DataSource = dtAssignedTrainingRequest.Copy();
                grdvAssignedRequests.DataBind();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Debug(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            TrainingRequestofTrainingDataHandler TRTDH = new TrainingRequestofTrainingDataHandler();
            DataTable dtAssignedTrainingRequest = new DataTable();
            try
            {
                log.Debug("btnSave_Click");

                Utility.Errorhandler.ClearError(lblMessage);

                dtAssignedTrainingRequest = (Session["dtAssignedTrainingRequest"] as DataTable).Copy();

                for (int i = 0; i < grdvAssignedRequests.Rows.Count; i++)
                {
                    string RequestID = HttpUtility.HtmlDecode(grdvAssignedRequests.Rows[i].Cells[0].Text).Trim();
                    TextBox txtDescription = (grdvAssignedRequests.Rows[i].FindControl("txtDescription") as TextBox);

                    DataRow[] drRequest = dtAssignedTrainingRequest.Select("REQUEST_ID = '" + RequestID + "'");
                    drRequest[0]["REMARKS"] = txtDescription.Text.Trim();
                }

                string TrainingID = txtTrainingID.Text.Trim();
                string AddedBy = (Session["KeyEMPLOYEE_ID"] as string);

                if (dtAssignedTrainingRequest.Rows.Count > 0)
                {
                    TRTDH.Insert(TrainingID, dtAssignedTrainingRequest.Copy(), AddedBy);
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = @"Training request is required";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                    return;
                }

                //Update unassign Requests Grid
                if (chkUnAssignRequests.Checked == true)
                {
                    txtTrainingRequestID.Enabled = false;
                    imgRequestSearch.Visible = false;

                    LoadUnAssignRequests();
                }
                else
                {
                    txtTrainingRequestID.Enabled = true;
                    imgRequestSearch.Visible = true;

                    grdvTrainingRequest.DataSource = null;
                    grdvTrainingRequest.DataBind();

                    ClearTrainingRequestDetails();

                    btnAddToTraining.Visible = false;
                }
                //--
                //Load Assigned Requests
                LoadAssignedTrainingRequest();
                //--


                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMessage);

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Debug(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TRTDH = null;
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("btnClear_Click");

                Utility.Utils.clearControls(false, txtTrainingID, txtTrainingRequestID, chkUnAssignRequests, lblTrainingName, lblTrainingCode, lblTrainingProgram, lblTrainingType, lblPlannedParticipants, lblPlannedStartDate, lblPlannedEndDate, lblTrainingCategory, lblRequestTrainingType, lblCompany, lblDepartment, lblDivision, lblBranch, lblTrainingRequestType, lblEmployee, lblDesignation, lblEmail, lblRequestReason, lbldescription, lblSkillsExpected, NoOfParticipants);
                Utility.Errorhandler.ClearError(lblMessage);
                btnAddToTraining.Visible = false;
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                dt1 = (Session["dtTrainingRequest"] as DataTable);
                if (dt1 != null)
                {
                    dt1.Rows.Clear();
                }
                Session["dtTrainingRequest"] = dt1;

                dt2 = (Session["dtAssignedTrainingRequest"] as DataTable);
                if (dt2 != null)
                {
                    dt2.Rows.Clear();
                }
                Session["dtAssignedTrainingRequest"] = dt2;

                grdvTrainingRequest.DataSource = null;
                grdvTrainingRequest.DataBind();

                grdvAssignedRequests.DataSource = null;
                grdvAssignedRequests.DataBind();

                DisplayDivs();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Debug(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        #endregion
    }
}