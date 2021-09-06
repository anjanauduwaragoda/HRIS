using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.TrainingAndDevelopment;
using System.Data;
using Common;
using NLog;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainerTraining : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        public string addTrainerText = "Add";
        public string updateTrainerText = "Update";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmTrainerTrainingPrograme : Page_Load");
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
                loadTrainersGridView();
                fillStatus();
                fillCompetenciesDropdown();
                tblTrainerDetails.Visible = false;
                if (Page.PreviousPage != null)
                {
                    hfSelectedProgrameId.Value = ((HiddenField)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$hfProgrameId")).Value.Trim().ToString();
                    loadProgrameDetails(hfSelectedProgrameId.Value);
                    //lblProgrameName.Text = ((TextBox)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$txtProgrameName")).Text.Trim().ToString();
                    //lblProgrameCode.Text = ((TextBox)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$txtProgrameCode")).Text.Trim().ToString();
                    //lblProgrameType.Text = ((DropDownList)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$ddlProgrameType")).SelectedItem.Text.Trim().ToString();
                    //"ctl00$ContentPlaceHolder1$txtAddress"
                }
            }
        }

        #region methodes

        private void fillCompetenciesDropdown()
        {
            log.Debug("fillCompetenciesDropdown()");
            DataTable dtCompetencies = new DataTable();
            TrainerInformationDataHandler trainerInformationDataHandler = new TrainerInformationDataHandler();
            try
            {

                dtCompetencies = trainerInformationDataHandler.getCompetencies();

                ListItem blankItem = new ListItem();
                blankItem.Value = "";
                blankItem.Text = "";
                ddlTrainerCompetency.Items.Add(blankItem);

                if (dtCompetencies.Rows.Count > 0)
                {
                    foreach (DataRow competency in dtCompetencies.Rows)
                    {
                        ListItem newItem = new ListItem();
                        newItem.Value = competency[0].ToString();
                        newItem.Text = competency[1].ToString();
                        ddlTrainerCompetency.Items.Add(newItem);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                dtCompetencies.Dispose();
                trainerInformationDataHandler = null;
            }
        }

        private void loadProgrameDetails(string programeId)
        {
            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            DataTable dtPrograme = new DataTable();

            try
            {
                dtPrograme = trainingProgramDataHandler.getProgrameById(programeId);
                lblProgrameCode.Text = dtPrograme.Rows[0]["PROGRAM_CODE"].ToString();
                lblProgrameName.Text = dtPrograme.Rows[0]["PROGRAM_NAME"].ToString();
                lblProgrameType.Text = (dtPrograme.Rows[0]["PROGRAM_TYPE"].ToString() == Constants.PROGRAME_TYPE_LONG_TERM_VALUE) ? Constants.PROGRAME_TYPE_LONG_TERM_TAG : Constants.PROGRAME_TYPE_SHORT_TERM_TAG;
                lblTrainingType.Text = dtPrograme.Rows[0]["TYPE_NAME"].ToString();

                loadAddedTrainersGridView();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                trainingProgramDataHandler = null;
                dtPrograme.Dispose();
            }
        }

        private void loadTrainersGridView()
        {
            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            DataTable dtTrainers = new DataTable();
            try
            {
                dtTrainers = trainingProgramDataHandler.getAllTrainers();
                gvTrainers.DataSource = dtTrainers;
                gvTrainers.DataBind();

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                trainingProgramDataHandler = null;
                dtTrainers.Dispose();
            }
        }

        private void loadTrainersGridView(DataTable dtFilteredTrainers)
        {
            log.Debug("loadTrainersGridView()");
            try
            {
                if (dtFilteredTrainers.Rows.Count > 0)
                {
                    gvTrainers.DataSource = dtFilteredTrainers;
                    gvTrainers.DataBind();
                }
                else
                {
                    gvTrainers.DataSource = null;
                    gvTrainers.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                dtFilteredTrainers.Dispose();
            }
        }

        private void fillTrainerDetails(DataTable dtTrainerDetails, DataTable dtTrainerCompetencies)
        {
            try
            {
                

                lblTrainerName.Text = dtTrainerDetails.Rows[0]["NAME_WITH_INITIALS"].ToString();
                lblTrainingNature.Text = dtTrainerDetails.Rows[0]["NATURE"].ToString();
                lblContactNo.Text = dtTrainerDetails.Rows[0]["CONTACT_MOBILE"].ToString();
                lblQualifications.Text = dtTrainerDetails.Rows[0]["QUALIFICATIONS"].ToString();
                lblIsExternal.Text = (dtTrainerDetails.Rows[0]["IS_EXTERNAL"].ToString() == Constants.TRAINER_EXTERNAL_VALUE) ? Constants.TRAINER_EXTERNAL_TAG : Constants.TRAINER_INTERNAL_TAG;
                //lblDescription.Text = dtTrainerDetails.Rows[0]["DESCRIPTION"].ToString();
                if (dtTrainerCompetencies.Rows.Count > 0)
                {
                    string competencyHtmlList = "";
                    //string competencyHtmlList = "<ul>";
                    foreach (DataRow competency in dtTrainerCompetencies.Rows)
                    {
                        if (competency[0].ToString() == (dtTrainerCompetencies.Rows.Count - 1).ToString())
                        {
                            competencyHtmlList += competency[1].ToString();
                        }
                        else
                        {
                            competencyHtmlList += competency[1].ToString() + "<br />";
                        }
                        // competencyHtmlList += "<li>" + competency[1].ToString() + "</li>";
                    }
                    //competencyHtmlList += "</ul>";
                    lblCompetencies.Text = competencyHtmlList;
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void fillStatus()
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

        private void loadAddedTrainersGridView()
        {
            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            DataTable dtAddedTrainers = new DataTable();

            try
            {
                string selectedProgrameId = hfSelectedProgrameId.Value.ToString();

                if (!String.IsNullOrEmpty(selectedProgrameId))
                {
                    dtAddedTrainers = trainingProgramDataHandler.getTrainersToPrograme(selectedProgrameId);
                    gvAddedTrainers.DataSource = dtAddedTrainers;
                    gvAddedTrainers.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                trainingProgramDataHandler = null;
                dtAddedTrainers.Dispose();
            }
        }

        private void HideDetailTable()
        {
            tblTrainerDetails.Visible = false;

        }

        private void onTrainerSelectedEvent()
        {
            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            DataTable dtTrainerDetails = new DataTable();
            DataTable dtTrainerCompetencies = new DataTable();
            try
            {
                Utility.Errorhandler.ClearError(lblErrorMsg);
                tblTrainerDetails.Visible = true;
                lblStatus.Visible = false;
                ddlStatus.Visible = false;

                int index = gvAddedTrainers.SelectedIndex;
                string trainerId = gvAddedTrainers.Rows[index].Cells[0].Text.Trim().ToString();

                hfTrainerId.Value = trainerId;
                dtTrainerDetails = trainingProgramDataHandler.getTrainerById(trainerId);
                dtTrainerCompetencies = trainingProgramDataHandler.getTrainerCompetenciesById(trainerId);
                fillTrainerDetails(dtTrainerDetails, dtTrainerCompetencies);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                trainingProgramDataHandler = null;
                dtTrainerDetails.Dispose();
                dtTrainerCompetencies.Dispose();
            }
        }

        private void clearTrainerDetails()
        {
            try
            {
                hfTrainerId.Value = "";
                Utility.Utils.clearControls(true, lblTrainerName, lblTrainingNature, lblContactNo, lblQualifications, lblIsExternal, lblCompetencies);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        #endregion

        #region events

        protected void gvTrainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gvTrainers_SelectedIndexChanged()");
            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            DataTable dtTrainerDetails = new DataTable();
            DataTable dtTrainerCompetencies = new DataTable();
            try
            {
                Utility.Errorhandler.ClearError(lblErrorMsg);
                clearTrainerDetails();
                tblTrainerDetails.Visible = true;
                lblStatus.Visible = false;
                ddlStatus.Visible = false;


                int index = gvTrainers.SelectedIndex;
                string trainerId = gvTrainers.Rows[index].Cells[0].Text.Trim().ToString();

                hfTrainerId.Value = trainerId;
                dtTrainerDetails = trainingProgramDataHandler.getTrainerById(trainerId);
                dtTrainerCompetencies = trainingProgramDataHandler.getTrainerCompetenciesById(trainerId);
                fillTrainerDetails(dtTrainerDetails, dtTrainerCompetencies);

                btnAdd.Text = addTrainerText;
                ddlStatus.SelectedValue = Constants.STATUS_ACTIVE_VALUE;
                Utility.Utils.clearControls(false, txtDescription, txtCost);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                trainingProgramDataHandler = null;
                dtTrainerDetails.Dispose();
                dtTrainerCompetencies.Dispose();
            }
        }

        protected void gvTrainers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            DataTable dtCompetencies = new DataTable();
            log.Debug("gvTrainers_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvTrainers, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");


                    string trainerId = e.Row.Cells[0].Text.ToString();
                    dtCompetencies = trainingProgramDataHandler.getTrainerCompetenciesById(trainerId);

                    string competencies = "";
                    if (dtCompetencies.Rows.Count > 0)
                    {
                        int i = 1;
                        foreach (DataRow competency in dtCompetencies.Rows)
                        {
                            competencies += competency["NAME"].ToString();
                            
                            if (i < dtCompetencies.Rows.Count)
                            {
                                competencies += ", ";
                            }
                            i++;
                        }
                    }
                    e.Row.Cells[4].ToolTip = competencies;
                    //e.Row.Cells[4].Attributes.Add("onClick",
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                trainingProgramDataHandler = null;
                dtCompetencies.Dispose();
            }
        }

        protected void gvTrainers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvTrainers_PageIndexChanging()");
            gvTrainers.PageIndex = e.NewPageIndex;
            loadTrainersGridView();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            log.Debug("btnAdd_Click()");

            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            try
            {
                string trainerId = hfTrainerId.Value.ToString();
                string programId = hfSelectedProgrameId.Value.ToString();
                string costPerSession = txtCost.Text.ToString();
                string description = txtDescription.Text.ToString();
                string addedUserId = Session["KeyUSER_ID"].ToString();

                if (String.IsNullOrEmpty(programId))
                {
                    CommonVariables.MESSAGE_TEXT = " Select programe before adding trainers";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    return;
                }
                if (btnAdd.Text.ToString() == addTrainerText)
                {
                    string status = Constants.STATUS_ACTIVE_VALUE;
                    bool isExist = trainingProgramDataHandler.checkTrainerExistanceForPrograme(trainerId, programId);
                    if (isExist)
                    {
                        CommonVariables.MESSAGE_TEXT = " Trainer already exist";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                    else
                    {
                        bool inserted = trainingProgramDataHandler.addTrainer(trainerId, programId, costPerSession, description, addedUserId, status);
                        if (inserted)
                        {
                            CommonVariables.MESSAGE_TEXT = " Trainer successfully added";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            loadAddedTrainersGridView();
                            HideDetailTable();
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = " Couldn't add the trainer";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }
                    }
                }
                else if (btnAdd.Text.ToString() == updateTrainerText)
                {
                    string status = ddlStatus.SelectedValue.ToString();
                    bool updated = trainingProgramDataHandler.updateTrainer(trainerId, programId, costPerSession, description, addedUserId, status);
                    if (updated)
                    {
                        CommonVariables.MESSAGE_TEXT = " Trainer successfully updated";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        loadAddedTrainersGridView();
                        HideDetailTable();
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = " Couldn't update the trainer";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                }


            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                trainingProgramDataHandler = null;
            }
        }

        protected void lblFindPrograme_Click(object sender, EventArgs e)
        {
            log.Debug("lblFindPrograme_Click()");
            //Server.Transfer("~/TrainingAndDevelopment/WebFrmTrainingPrograme.aspx");
            Response.Redirect("~/TrainingAndDevelopment/WebFrmTrainingPrograme.aspx");
        }

        protected void CloseButton1_Click(object sender, ImageClickEventArgs e)
        {
            log.Debug("ImageButton1_Click()");
            HideDetailTable();
            hfTrainerId.Value = "";
            Utility.Utils.clearControls(true, ddlStatus, txtCost, txtDescription);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");
            HideDetailTable();
            Utility.Utils.clearControls(true, ddlStatus, txtCost, txtDescription);
        }

        protected void gvAddedTrainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gvAddedTrainers_SelectedIndexChanged()");
            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            DataTable dtTrainerDetails = new DataTable();
            DataTable dtTrainerCompetencies = new DataTable();
            try
            {
                Utility.Errorhandler.ClearError(lblErrorMsg);
                tblTrainerDetails.Visible = true;
                lblStatus.Visible = true;
                ddlStatus.Visible = true;

                int index = gvAddedTrainers.SelectedIndex;
                string trainerId = gvAddedTrainers.Rows[index].Cells[0].Text.Trim().ToString();

                string programId = hfSelectedProgrameId.Value.ToString();

                hfTrainerId.Value = trainerId;
                dtTrainerDetails = trainingProgramDataHandler.getTrainerProgrameDetails(trainerId, programId);
                dtTrainerCompetencies = trainingProgramDataHandler.getTrainerCompetenciesById(trainerId);
                fillTrainerDetails(dtTrainerDetails, dtTrainerCompetencies);

                txtCost.Text = dtTrainerDetails.Rows[0]["COST_PER_SESSION"].ToString();
                txtDescription.Text = dtTrainerDetails.Rows[0]["DESCRIPTION"].ToString();
                ddlStatus.SelectedValue = dtTrainerDetails.Rows[0]["STATUS_CODE"].ToString();
                btnAdd.Text = updateTrainerText;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                trainingProgramDataHandler = null;
                dtTrainerCompetencies.Dispose();
                dtTrainerDetails.Dispose();
            }
        }

        protected void gvAddedTrainers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvAddedTrainers_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvAddedTrainers, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void gvAddedTrainers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvAddedTrainers_PageIndexChanging()");
            gvAddedTrainers.PageIndex = e.NewPageIndex;
            loadAddedTrainersGridView();
        }

        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            log.Debug("iBtnSearch_Click()");
            DataTable dtFilteredTrainers = new DataTable();
            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            try
            {
                string selectedCompetency = ddlTrainerCompetency.SelectedValue.ToString();
                if (String.IsNullOrEmpty(selectedCompetency))
                {
                    loadTrainersGridView();
                }
                else
                {
                    dtFilteredTrainers = trainingProgramDataHandler.getFilteredTrainers(selectedCompetency);
                    loadTrainersGridView(dtFilteredTrainers);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                dtFilteredTrainers.Dispose();
                trainingProgramDataHandler = null;
            }

        }

        #endregion



    }
}