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
    public partial class WebFrmMapTrainingRequests : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sIPAddress = Request.UserHostAddress;
                log.Debug("IP:" + sIPAddress + "WebFrmMapTrainingRequests : Page_Load");
                Utility.Errorhandler.ClearError(lblMessage);

                if (IsPostBack)
                {
                    if (hfCaller.Value == "txtSearchTraining")
                    {
                        hfCaller.Value = "";
                        if (hfVal.Value != "")
                        {
                            txtSearchTraining.Text = hfVal.Value;
                        }
                        if (txtSearchTraining.Text != "")
                        {
                            //Postback Methods
                            LoadTraining();
                            hfVal.Value = String.Empty;
                            LoadTrainingCompanies(txtSearchTraining.Text);

                            LoadSavedTrainingRequests(txtSearchTraining.Text);
                        }
                    }
                    if (hfCaller.Value == "txtTrainingRequests")
                    {
                        hfCaller.Value = "";
                        if (hfVal.Value != "")
                        {
                            txtTrainingRequests.Text = hfVal.Value;
                        }
                        if (txtTrainingRequests.Text != "")
                        {
                            //Postback Methods
                            hfVal.Value = String.Empty;

                            if (txtSearchTraining.Text == String.Empty)
                            {
                                CommonVariables.MESSAGE_TEXT = "Training is Required";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                                return;
                            }

                            if (Session["dtRequest"] != null)
                            {
                                DataTable dtRequest = new DataTable();
                                dtRequest = (Session["dtRequest"] as DataTable).Copy();

                                if (dtRequest.Rows.Count > 0)
                                {
                                    string TrainingIDs = String.Empty;

                                    for (int i = 0; i < dtRequest.Rows.Count; i++)
                                    {
                                        if (TrainingIDs != String.Empty)
                                        {
                                            TrainingIDs += ", " + dtRequest.Rows[i]["REQUEST_ID"].ToString();
                                        }
                                        else
                                        {
                                            TrainingIDs += dtRequest.Rows[i]["REQUEST_ID"].ToString();
                                        }
                                    }

                                    txtTrainingRequests.Text = TrainingIDs;
                                    LoadTrainingRequestGrid(dtRequest.Copy());
                                    return;
                                }
                            }

                            LoadTrainingRequestGrid(txtTrainingRequests.Text);                            
                        }
                    }
                }

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Error(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        void LoadTraining()
        {
            TrainingRequestofTrainingDataHandler TRTDH = new TrainingRequestofTrainingDataHandler();
            DataTable dtTraining = new DataTable();
            try
            {
                log.Debug("WebFrmMapTrainingRequests | LoadTraining()");

                dtTraining = TRTDH.Populate(txtSearchTraining.Text.Trim()).Copy();

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
                log.Error("WebFrmMapTrainingRequests | LoadTraining() | " + ex.Message);
                throw ex;
            }
            finally
            {
                dtTraining.Dispose();
                TRTDH = null;
            }
        }

        protected void chkisIncludeAll_CheckedChanged(object sender, EventArgs e)
        {
            DataTable SelectedTrainingRequests = new DataTable();
            try
            {
                log.Debug("WebFrmMapTrainingRequests | chkisIncludeAll_CheckedChanged()");

                CheckBox chkisIncludeAll = (grdvTrainingRequest.HeaderRow.FindControl("chkisIncludeAll") as CheckBox);

                SelectedTrainingRequests = (Session["SelectedTrainingRequests"] as DataTable).Copy();
                
                for (int i = 0; i < SelectedTrainingRequests.Rows.Count; i++)
                {
                    if (chkisIncludeAll.Checked)
                    {
                        SelectedTrainingRequests.Rows[i]["isExclude"] = Constants.CON_ACTIVE_STATUS;
                    }
                    else
                    {
                        SelectedTrainingRequests.Rows[i]["isExclude"] = Constants.CON_INACTIVE_STATUS;
                    }
                }

                Session["SelectedTrainingRequests"] = SelectedTrainingRequests.Copy();

                grdvTrainingRequest.DataSource = SelectedTrainingRequests.Copy();
                grdvTrainingRequest.DataBind();
            }
            catch (Exception exp)
            {
                log.Debug("WebFrmMapTrainingRequests | chkisIncludeAll_CheckedChanged() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Error(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void chkisInclude_CheckedChanged(object sender, EventArgs e)
        {
            DataTable SelectedTrainingRequests = new DataTable();
            try
            {
                log.Debug("WebFrmMapTrainingRequests | chkisInclude_CheckedChanged()");

                SelectedTrainingRequests = (Session["SelectedTrainingRequests"] as DataTable).Copy();

                for (int i = 0; i < grdvTrainingRequest.Rows.Count; i++)
                {
                    CheckBox chkisInclude = (grdvTrainingRequest.Rows[i].FindControl("chkisInclude") as CheckBox);

                    if (chkisInclude.Checked == true)
                    {
                        string RequestID = HttpUtility.HtmlDecode(grdvTrainingRequest.Rows[i].Cells[0].Text).Trim();
                        DataRow[] drArr = SelectedTrainingRequests.Select("REQUEST_ID = '" + RequestID + "'");

                        foreach (DataRow dr in drArr)
                        {
                            dr["isExclude"] = Constants.CON_ACTIVE_STATUS;
                        }
                    }
                    else
                    {
                        string RequestID = HttpUtility.HtmlDecode(grdvTrainingRequest.Rows[i].Cells[0].Text).Trim();
                        DataRow[] drArr = SelectedTrainingRequests.Select("REQUEST_ID = '" + RequestID + "'");

                        foreach (DataRow dr in drArr)
                        {
                            dr["isExclude"] = Constants.CON_INACTIVE_STATUS;
                        }
                    }
                }

                Session["SelectedTrainingRequests"] = SelectedTrainingRequests.Copy();
            }
            catch (Exception exp)
            {
                log.Debug("WebFrmMapTrainingRequests | chkisInclude_CheckedChanged() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Error(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        /// <summary>
        /// Load Single Training Request to the Training Request Gridview
        /// </summary>
        /// <param name="TrainingRequestID">The Training Request ID to Load Data</param>
        private void LoadTrainingRequestGrid(string TrainingRequestID)
        {
            TrainingRequestofTrainingDataHandler TRTDH = new TrainingRequestofTrainingDataHandler();
            DataTable dtTrainingRequest = new DataTable();
            try
            {
                log.Debug("WebFrmMapTrainingRequests | LoadTrainingRequestGrid()");

                dtTrainingRequest = TRTDH.SearchTrainingRequest(TrainingRequestID).Copy();

                dtTrainingRequest.Columns.Add("isExclude");
                grdvTrainingRequest.DataSource = dtTrainingRequest.Copy();
                grdvTrainingRequest.DataBind();

                Session["SelectedTrainingRequests"] = dtTrainingRequest.Copy();
            }
            catch (Exception ex)
            {
                log.Debug("WebFrmMapTrainingRequests | LoadTrainingRequestGrid() | " + ex.Message);
                throw ex;
            }
            finally
            { 
            
            }
        }

        /// <summary>
        /// Load Multiple Training Requests to the Training Request Grid View
        /// </summary>
        /// <param name="TrainingRequests">The Data Table to Load the Grid View</param>
        private void LoadTrainingRequestGrid(DataTable TrainingRequests)
        {
            try
            {
                log.Debug("WebFrmMapTrainingRequests | LoadTrainingRequestGrid()");

                TrainingRequests.Columns.Add("isExclude");
                grdvTrainingRequest.DataSource = TrainingRequests.Copy();
                grdvTrainingRequest.DataBind();

                Session["SelectedTrainingRequests"] = TrainingRequests.Copy();
            }
            catch (Exception ex)
            {
                log.Debug("WebFrmMapTrainingRequests | LoadTrainingRequestGrid() | " + ex.Message);
                throw ex;
            }
            finally
            {

            }
        }

        protected void grdvTrainingRequest_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                log.Debug("WebFrmMapTrainingRequests | grdvTrainingRequest_RowDataBound()");

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    DataTable SelectedTrainingRequests = new DataTable();
                    SelectedTrainingRequests = (Session["SelectedTrainingRequests"] as DataTable).Copy();

                    int RowCount = SelectedTrainingRequests.Rows.Count;
                    DataRow[] drArr = SelectedTrainingRequests.Select("isExclude  = '" + Constants.CON_ACTIVE_STATUS + "'");

                    if (RowCount == drArr.Length)
                    {
                        CheckBox chkisIncludeAll = (e.Row.Cells[3].FindControl("chkisIncludeAll") as CheckBox);
                        chkisIncludeAll.Checked = true;
                    }
                    else
                    {
                        CheckBox chkisIncludeAll = (e.Row.Cells[3].FindControl("chkisIncludeAll") as CheckBox);
                        chkisIncludeAll.Checked = false;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkisInclude = (e.Row.FindControl("chkisInclude") as CheckBox);
                    string isExclude = HttpUtility.HtmlDecode(e.Row.Cells[4].Text).Trim();

                    if (isExclude == Constants.CON_ACTIVE_STATUS)
                    {
                        chkisInclude.Checked = true;
                    }
                    else
                    {
                        chkisInclude.Checked = false;
                    }

                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdvTrainingRequest, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");

                }

            }
            catch (Exception exp)
            {
                //log.Debug("WebFrmMapTrainingRequests | grdvTrainingRequest_RowDataBound() | " + exp.Message);
                //CommonVariables.MESSAGE_TEXT = exp.Message;
                //log.Error(exp.Message);
                //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void bntSave_Click(object sender, EventArgs e)
        {
            TrainingRequestofTrainingDataHandler TRTDH = new TrainingRequestofTrainingDataHandler();
            DataTable SelectedTrainingRequests = new DataTable();
            try
            {
                log.Debug("WebFrmMapTrainingRequests | bntSave_Click()");



                string TrainingID = txtSearchTraining.Text.Trim();
                SelectedTrainingRequests = (Session["SelectedTrainingRequests"] as DataTable).Copy();
                string AddedBy = (Session["KeyUSER_ID"] as string);

                TRTDH.Insert(TrainingID, SelectedTrainingRequests.Copy(), AddedBy);

                Clear();

                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_SAVED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMessage);

            }
            catch (Exception exp)
            {
                log.Debug("WebFrmMapTrainingRequests | bntSave_Click() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Error(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmMapTrainingRequests | bntSave_Click()");
                Clear();
            }
            catch (Exception exp)
            {
                log.Debug("WebFrmMapTrainingRequests | bntSave_Click() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Error(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        void LoadTrainingCompanies(string TrainingID)
        {
            TrainingRequestofTrainingDataHandler TRTDH = new TrainingRequestofTrainingDataHandler();
            DataTable dtTrainingCompanies = new DataTable();
            try
            {
                log.Debug("WebFrmMapTrainingRequests | LoadTrainingCompanies()");

                dtTrainingCompanies = TRTDH.PopulateTrainingCompanies(TrainingID).Copy();
                grdvTrainingCompanies.DataSource = dtTrainingCompanies.Copy();
                grdvTrainingCompanies.DataBind();
            }
            catch (Exception exp)
            {
                log.Debug("WebFrmMapTrainingRequests | LoadTrainingCompanies() | " + exp.Message);
                throw exp;
            }
            finally
            {
                TRTDH = null;
                dtTrainingCompanies.Dispose();
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

        protected void grdvTrainingRequest_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmMapTrainingRequests | grdvTrainingRequest_SelectedIndexChanged()");
                LoadTrainingRequestDetails();
            }
            catch (Exception exp)
            {
                log.Debug("WebFrmMapTrainingRequests | grdvTrainingRequest_SelectedIndexChanged() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                log.Error(exp.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        void LoadSavedTrainingRequests(string TrainingID)
        {
            TrainingRequestofTrainingDataHandler TRTDH = new TrainingRequestofTrainingDataHandler();
            DataTable SelectedTrainingRequests = new DataTable();
            try
            {
                SelectedTrainingRequests = TRTDH.PopulateSavedTrainingRequests(TrainingID).Copy();
                if (SelectedTrainingRequests.Rows.Count > 0)
                {
                    Session["SelectedTrainingRequests"] = SelectedTrainingRequests.Copy();
                    grdvTrainingRequest.DataSource = SelectedTrainingRequests.Copy();
                    grdvTrainingRequest.DataBind();
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

        void Clear()
        {
            try
            {
                log.Debug("WebFrmMapTrainingRequests | Clear()");

                Utility.Utils.clearControls(false, txtSearchTraining, txtTrainingRequests, lblTrainingName, lblTrainingCode, lblTrainingProgram, lblTrainingType, lblPlannedParticipants, lblPlannedStartDate, lblPlannedEndDate, lblTrainingCategory, lblRequestTrainingType, lblCompany, lblDepartment, lblDivision, lblBranch, lblTrainingRequestType, lblEmployee, lblDesignation, lblEmail, lblRequestReason, lbldescription, lblSkillsExpected, NoOfParticipants);
                Utility.Errorhandler.ClearError(lblMessage);
                grdvTrainingCompanies.DataSource = null;
                grdvTrainingCompanies.DataBind();

                grdvTrainingRequest.DataSource = null;
                grdvTrainingRequest.DataBind();
            }
            catch (Exception exp)
            {
                log.Error("WebFrmMapTrainingRequests | Clear() | " + exp.Message);
                throw exp;
            }
            finally
            {

            }
        }
    }
}