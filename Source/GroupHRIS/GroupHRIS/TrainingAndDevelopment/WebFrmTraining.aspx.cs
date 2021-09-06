using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.TrainingAndDevelopment;
using System.Data;
using NLog;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class Training : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sIPAddress = Request.UserHostAddress;
                log.Debug("IP:" + sIPAddress + "Training : Page_Load");

                Utility.Errorhandler.ClearError(lblMessage);

                if (!IsPostBack)
                {
                    LoadTrainingPrograms();
                    LoadTrainingTypes();
                    LoadMinutes(ddlPlannedTotalMins);
                    LoadMinutes(ddlActualTotalMins);
                    LoadStatus();
                    LoadTrainings();

                    //Training Company
                    LoadCompanies();
                    LoadCompanyStatus();
                    CreateTrainingCompanyDataTable();

                    //Training Trainers
                    LoadTrainerStatus();
                    CreateTrainingTrainerDataTable();

                }
                else
                {
                    if (hfCaller.Value == "txtTrainingID")
                    {
                        hfCaller.Value = "";
                        if (hfVal.Value != "")
                        {
                            txtTrainingID.Text = hfVal.Value;
                            hfVal.Value = String.Empty;
                        }
                        if (txtTrainingID.Text != "")
                        {
                            //Postback Methods

                            btnCompanyAdd.Text = "Add";
                            btnTrainerAdd.Text = "Add";
                        }
                    }

                    if (hfCaller.Value == "txtTrainerID")
                    {
                        hfCaller.Value = "";
                        if (hfVal.Value != "")
                        {
                            txtTrainerID.Text = hfVal.Value;
                            hfVal.Value = String.Empty;
                        }
                        if (txtTrainerID.Text != "")
                        {
                            //Postback Methods
                            lblTrainerName.Text = hfTrnName.Value.ToString();

                            btnTrainerAdd.Text = "Add";
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string TrainingName = String.Empty;
            string TrainingCode = String.Empty;
            string TrainingProgramID = String.Empty;
            string TrainingTypeID = String.Empty;
            string PlannedStartDate = String.Empty;
            string PlannedEndDate = String.Empty;
            string PlannedTotalHours = String.Empty;
            string PlannedParticipants = String.Empty;
            string ActualStartDate = String.Empty;
            string ActualEndDate = String.Empty;
            string ActualTotalHours = String.Empty;
            string ActualParticipants = String.Empty;
            string IsOutOfBudget = String.Empty;
            string IsPostponed = String.Empty;
            string PostponedReason = String.Empty;
            string StatusCode = String.Empty;
            string AddedBy = String.Empty;

            TrainingDataHandler TDH = new TrainingDataHandler();
            DataTable dtTrainingCompanies = new DataTable();
            DataTable dtTrainingTrainers = new DataTable();

            try
            {
                log.Debug("btnSave_Click()");

                if (isInvalidCharExists())
                {
                    CommonVariables.MESSAGE_TEXT = "Invalid training name";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                    return;
                }

                TrainingName = txtTrainingName.Text.Trim();
                TrainingCode = txtTrainingCode.Text.Trim();
                if (ddlTrainingProgram.Items.Count > 0)
                {
                    TrainingProgramID = ddlTrainingProgram.SelectedValue;
                }
                if (ddlTrainingType.Items.Count > 0)
                {
                    TrainingTypeID = ddlTrainingType.SelectedValue;
                }


                PlannedStartDate = txtPlannedStartDate.Text;
                PlannedEndDate = txtPlannedEndDate.Text;

                DateTime PSDate = System.DateTime.Now;
                DateTime PEDate = System.DateTime.Now;
                
                if (PlannedStartDate != String.Empty)
                {
                    PSDate = DateTime.ParseExact(PlannedStartDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                if (PlannedEndDate != String.Empty)
                {
                    PEDate = DateTime.ParseExact(PlannedEndDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                if ((PlannedStartDate != String.Empty) && (PlannedEndDate != String.Empty))
                {
                    if (PSDate > PEDate)
                    {
                        CommonVariables.MESSAGE_TEXT = "Planned start date should be less than the planned end date.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }
                }

                if (PlannedStartDate != String.Empty)
                {
                    PlannedStartDate = Convert.ToDateTime(PSDate).ToString("yyyy-MM-dd");
                }
                if (PlannedEndDate != String.Empty)
                {
                    PlannedEndDate = Convert.ToDateTime(PEDate).ToString("yyyy-MM-dd");
                }

                PlannedTotalHours = txtPlannedTotalHours.Text.Trim();

                if (PlannedTotalHours.Length > 0)
                {
                    PlannedTotalHours = txtPlannedTotalHours.Text.Trim() + ".";
                    if (ddlPlannedTotalMins.SelectedIndex > 0)
                    {
                        PlannedTotalHours += ddlPlannedTotalMins.SelectedValue;
                    }
                    else
                    {
                        PlannedTotalHours += "00";
                    }

                    decimal TempPlannedTotalHours;
                    if (decimal.TryParse(PlannedTotalHours, out TempPlannedTotalHours))
                    {
                        PlannedTotalHours = TempPlannedTotalHours.ToString();
                    }
                    else
                    {
                        PlannedTotalHours = String.Empty;
                    }
                }

                //PlannedParticipants = txtPlannedParticipants.Text;

                ActualStartDate = txtActualStartDate.Text;
                ActualEndDate = txtActualEndDate.Text;

                DateTime ASDate = System.DateTime.Now;
                DateTime AEDate = System.DateTime.Now;
                if (ActualStartDate != String.Empty)
                {
                    ASDate = DateTime.ParseExact(ActualStartDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                if (ActualEndDate != String.Empty)
                {
                    AEDate = DateTime.ParseExact(ActualEndDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                if ((ActualStartDate != String.Empty) && (ActualEndDate != String.Empty))
                {
                    if (ASDate > AEDate)
                    {
                        CommonVariables.MESSAGE_TEXT = "Actual start date should be less than the actual end date.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }
                }
                if (ActualStartDate != String.Empty)
                {
                    ActualStartDate = Convert.ToDateTime(ASDate).ToString("yyyy-MM-dd");
                }
                if (ActualEndDate != String.Empty)
                {
                    ActualEndDate = Convert.ToDateTime(AEDate).ToString("yyyy-MM-dd");
                }
                ActualTotalHours = txtActualTotalHours.Text;

                if (ActualTotalHours.Length > 0)
                {
                    ActualTotalHours = txtActualTotalHours.Text.Trim() + ".";
                    if (ddlActualTotalMins.SelectedIndex > 0)
                    {
                        ActualTotalHours += ddlActualTotalMins.SelectedValue;
                    }
                    else
                    {
                        ActualTotalHours += "00";
                    }

                    decimal TempActualTotalHours;
                    if (decimal.TryParse(ActualTotalHours, out TempActualTotalHours))
                    {
                        ActualTotalHours = TempActualTotalHours.ToString();
                    }
                    else
                    {
                        ActualTotalHours = String.Empty;
                    }
                }

                //ActualParticipants = txtActualParticipants.Text;
                if (chkIsOutOfBudget.Checked == true)
                {
                    IsOutOfBudget = Constants.CON_ACTIVE_STATUS;
                }
                else
                {
                    IsOutOfBudget = Constants.CON_INACTIVE_STATUS;
                }
                if (chkIsPostponed.Checked == true)
                {
                    IsPostponed = Constants.CON_ACTIVE_STATUS;
                    PostponedReason = txtPostpanedReason.Text;
                }
                else
                {
                    IsPostponed = Constants.CON_INACTIVE_STATUS;
                }
                if (ddlStatusCode.Items.Count > 0)
                {
                    StatusCode = ddlStatusCode.SelectedValue;
                }
                AddedBy = (Session["KeyEMPLOYEE_ID"] as string);

                dtTrainingCompanies = (Session["dtTrainingCompanies"] as DataTable).Copy();
                dtTrainingTrainers = (Session["dtTrainingTrainers"] as DataTable).Copy();

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {

                    if (TDH.CheckTrainingNameExsistance(TrainingName.Trim()))
                    {
                        CommonVariables.MESSAGE_TEXT = "Training name already exists.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }

                    TDH.Insert(TrainingName, TrainingCode, TrainingProgramID, TrainingTypeID, PlannedStartDate, PlannedEndDate, PlannedTotalHours, PlannedParticipants, ActualStartDate, ActualEndDate, ActualTotalHours, ActualParticipants, IsOutOfBudget, IsPostponed, PostponedReason, StatusCode, AddedBy, dtTrainingCompanies.Copy(), dtTrainingTrainers.Copy());
                    ClearFields();
                    CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_SAVED;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMessage);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string TrainingID = String.Empty;

                    if (txtTrainingID.Text != String.Empty)
                    {
                        TrainingID = txtTrainingID.Text.Trim();
                    }
                    else
                    {
                        TrainingID = grdvTraining.Rows[grdvTraining.SelectedIndex].Cells[0].Text.Trim();
                    }

                    if (TDH.CheckTrainingNameExsistance(TrainingName.Trim(), TrainingID))
                    {
                        CommonVariables.MESSAGE_TEXT = "Training name already exists.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }

                    TDH.Update(TrainingName, TrainingCode, TrainingProgramID, TrainingTypeID, PlannedStartDate, PlannedEndDate, PlannedTotalHours, PlannedParticipants, ActualStartDate, ActualEndDate, ActualTotalHours, ActualParticipants, IsOutOfBudget, IsPostponed, PostponedReason, StatusCode, AddedBy, TrainingID, dtTrainingCompanies.Copy(), dtTrainingTrainers.Copy());
                    ClearFields();
                    CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMessage);
                }

                LoadTrainings();

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtTrainingCompanies.Dispose();
                TrainingName = String.Empty;
                TrainingCode = String.Empty;
                TrainingProgramID = String.Empty;
                TrainingTypeID = String.Empty;
                PlannedStartDate = String.Empty;
                PlannedEndDate = String.Empty;
                PlannedTotalHours = String.Empty;
                PlannedParticipants = String.Empty;
                ActualStartDate = String.Empty;
                ActualEndDate = String.Empty;
                ActualTotalHours = String.Empty;
                ActualParticipants = String.Empty;
                IsOutOfBudget = String.Empty;
                IsPostponed = String.Empty;
                PostponedReason = String.Empty;
                StatusCode = String.Empty;
                AddedBy = String.Empty;
                TDH = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");
            ClearFields();
        }

        protected void chkIsPostponed_CheckedChanged(object sender, EventArgs e)
        {
            log.Debug("chkIsPostponed_CheckedChanged()");
            SetPostponedTextBox();
        }

        protected void grdvTraining_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtTrainings = new DataTable();

            try
            {
                log.Debug("grdvTraining_SelectedIndexChanged()");

                txtTrainingID.Text = String.Empty;

                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                
                string TrainingID = grdvTraining.Rows[grdvTraining.SelectedIndex].Cells[0].Text;

                dtTrainings = (Session["dtTrainings"] as DataTable).Copy();
                DataRow[] drTrainings = dtTrainings.Select("TRAINING_ID = '" + TrainingID + "'");

                if (drTrainings.Length > 0)
                {
                    txtTrainingName.Text = drTrainings[0]["TRAINING_NAME"].ToString();
                    txtTrainingCode.Text = drTrainings[0]["TRAINING_CODE"].ToString();
                    ddlTrainingProgram.SelectedIndex = ddlTrainingProgram.Items.IndexOf(ddlTrainingProgram.Items.FindByValue(drTrainings[0]["TRAINING_PROGRAM_ID"].ToString()));
                    ddlTrainingType.SelectedIndex = ddlTrainingType.Items.IndexOf(ddlTrainingType.Items.FindByValue(drTrainings[0]["TRAINING_TYPE"].ToString()));
                    txtPlannedStartDate.Text = drTrainings[0]["PLANNED_START_DATE"].ToString();
                    txtPlannedEndDate.Text = drTrainings[0]["PLANNED_END_DATE"].ToString();

                    if (drTrainings[0]["PLANNED_TOTAL_HOURS"].ToString() != String.Empty)
                    {
                        string[] PlannedTotalDuration = drTrainings[0]["PLANNED_TOTAL_HOURS"].ToString().Split('.');
                        txtPlannedTotalHours.Text = PlannedTotalDuration[0];
                        if (PlannedTotalDuration.Length > 1)
                        {
                            ddlPlannedTotalMins.SelectedIndex = ddlPlannedTotalMins.Items.IndexOf(ddlPlannedTotalMins.Items.FindByValue(PlannedTotalDuration[1]));
                        }
                        else
                        {
                            ddlPlannedTotalMins.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        txtPlannedTotalHours.Text = String.Empty;
                        ddlPlannedTotalMins.SelectedIndex = 0;
                    }

                    //txtPlannedParticipants.Text = drTrainings[0]["PLANNED_PARTICIPANTS"].ToString();
                    txtActualStartDate.Text = drTrainings[0]["ACTUAL_START_DATE"].ToString();
                    txtActualEndDate.Text = drTrainings[0]["ACTUAL_END_DATE"].ToString();

                    if (drTrainings[0]["ACTUAL_TOTAL_HOURS"].ToString() != String.Empty)
                    {
                        string[] ActualTotalDuration = drTrainings[0]["ACTUAL_TOTAL_HOURS"].ToString().Split('.');
                        txtActualTotalHours.Text = ActualTotalDuration[0];
                        if (ActualTotalDuration.Length > 1)
                        {
                            ddlActualTotalMins.SelectedIndex = ddlActualTotalMins.Items.IndexOf(ddlActualTotalMins.Items.FindByValue(ActualTotalDuration[1]));
                        }
                        else
                        {
                            ddlActualTotalMins.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        txtActualTotalHours.Text = String.Empty;
                        ddlActualTotalMins.SelectedIndex = 0;
                    }

                    //txtActualParticipants.Text = drTrainings[0]["ACTUAL_PARTICIPANTS"].ToString();

                    if (drTrainings[0]["IS_OUT_OF_BUDGET"].ToString() == Constants.CON_ACTIVE_STATUS)
                    {
                        chkIsOutOfBudget.Checked = true;
                    }
                    else
                    {
                        chkIsOutOfBudget.Checked = false;
                    }

                    if (drTrainings[0]["IS_POSTPONED"].ToString() == Constants.CON_ACTIVE_STATUS)
                    {
                        chkIsPostponed.Checked = true;
                        txtPostpanedReason.Enabled = true;
                        txtPostpanedReason.Text = drTrainings[0]["POSTPONED_REASON"].ToString();
                    }
                    else
                    {
                        chkIsPostponed.Checked = false;
                        txtPostpanedReason.Enabled = false;
                        txtPostpanedReason.Text = String.Empty;
                    }

                    string StatusCode = drTrainings[0]["STATUS_CODE"].ToString();
                    ddlStatusCode.SelectedIndex = ddlStatusCode.Items.IndexOf(ddlStatusCode.Items.FindByValue(StatusCode));

                    //Load Training Companies & Training Trainers
                    if (TrainingID != String.Empty)
                    {
                        LoadTrainingCompanies(TrainingID);
                        LoadTrainingTrainers(TrainingID);
                    }
                    
                }

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtTrainings.Dispose();
            }
        }

        protected void grdvTraining_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdvTraining, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdvTraining_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {    
            TrainingDataHandler TDH = new TrainingDataHandler();
            DataTable dtTrainings = new DataTable();
            try
            {
                log.Debug("grdvTraining_PageIndexChanging()");

                dtTrainings = TDH.Populate().Copy();
                grdvTraining.PageIndex = e.NewPageIndex;
                grdvTraining.DataSource = dtTrainings.Copy();
                grdvTraining.DataBind();
                Session["dtTrainings"] = dtTrainings.Copy();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtTrainings.Dispose();
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            TrainingDataHandler TDH = new TrainingDataHandler();
            DataTable dtTrainings = new DataTable();

            try
            {
                log.Debug("btnEdit_Click");
                if (txtTrainingID.Text != String.Empty)
                {
                    string TrainingID = txtTrainingID.Text;

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

                    dtTrainings = TDH.Populate(TrainingID).Copy();
                    DataRow[] drTrainings = dtTrainings.Select("TRAINING_ID = '" + TrainingID + "'");

                    if (drTrainings.Length > 0)
                    {
                        txtTrainingName.Text = drTrainings[0]["TRAINING_NAME"].ToString();
                        txtTrainingCode.Text = drTrainings[0]["TRAINING_CODE"].ToString();
                        ddlTrainingProgram.SelectedIndex = ddlTrainingProgram.Items.IndexOf(ddlTrainingProgram.Items.FindByValue(drTrainings[0]["TRAINING_PROGRAM_ID"].ToString()));
                        ddlTrainingType.SelectedIndex = ddlTrainingType.Items.IndexOf(ddlTrainingType.Items.FindByValue(drTrainings[0]["TRAINING_TYPE"].ToString()));
                        txtPlannedStartDate.Text = drTrainings[0]["PLANNED_START_DATE"].ToString();
                        txtPlannedEndDate.Text = drTrainings[0]["PLANNED_END_DATE"].ToString();

                        if (drTrainings[0]["PLANNED_TOTAL_HOURS"].ToString() != String.Empty)
                        {
                            string[] PlannedTotalDuration = drTrainings[0]["PLANNED_TOTAL_HOURS"].ToString().Split('.');
                            txtPlannedTotalHours.Text = PlannedTotalDuration[0];
                            if (PlannedTotalDuration.Length > 1)
                            {
                                ddlPlannedTotalMins.SelectedIndex = ddlPlannedTotalMins.Items.IndexOf(ddlPlannedTotalMins.Items.FindByValue(PlannedTotalDuration[1]));
                            }
                            else
                            {
                                ddlPlannedTotalMins.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            txtPlannedTotalHours.Text = String.Empty;
                            ddlPlannedTotalMins.SelectedIndex = 0;
                        }

                        //txtPlannedParticipants.Text = drTrainings[0]["PLANNED_PARTICIPANTS"].ToString();
                        txtActualStartDate.Text = drTrainings[0]["ACTUAL_START_DATE"].ToString();
                        txtActualEndDate.Text = drTrainings[0]["ACTUAL_END_DATE"].ToString();

                        if (drTrainings[0]["ACTUAL_TOTAL_HOURS"].ToString() != String.Empty)
                        {
                            string[] ActualTotalDuration = drTrainings[0]["ACTUAL_TOTAL_HOURS"].ToString().Split('.');
                            txtActualTotalHours.Text = ActualTotalDuration[0];
                            if (ActualTotalDuration.Length > 1)
                            {
                                ddlActualTotalMins.SelectedIndex = ddlActualTotalMins.Items.IndexOf(ddlActualTotalMins.Items.FindByValue(ActualTotalDuration[1]));
                            }
                            else
                            {
                                ddlActualTotalMins.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            txtActualTotalHours.Text = String.Empty;
                            ddlActualTotalMins.SelectedIndex = 0;
                        }

                        //txtActualParticipants.Text = drTrainings[0]["ACTUAL_PARTICIPANTS"].ToString();

                        if (drTrainings[0]["IS_OUT_OF_BUDGET"].ToString() == Constants.CON_ACTIVE_STATUS)
                        {
                            chkIsOutOfBudget.Checked = true;
                        }
                        else
                        {
                            chkIsOutOfBudget.Checked = false;
                        }

                        if (drTrainings[0]["IS_POSTPONED"].ToString() == Constants.CON_ACTIVE_STATUS)
                        {
                            chkIsPostponed.Checked = true;
                            txtPostpanedReason.Enabled = true;
                            txtPostpanedReason.Text = drTrainings[0]["POSTPONED_REASON"].ToString();
                        }
                        else
                        {
                            chkIsPostponed.Checked = false;
                            txtPostpanedReason.Enabled = false;
                            txtPostpanedReason.Text = String.Empty;
                        }

                        string StatusCode = drTrainings[0]["STATUS_CODE"].ToString();
                        ddlStatusCode.SelectedIndex = ddlStatusCode.Items.IndexOf(ddlStatusCode.Items.FindByValue(StatusCode));
                    }
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TDH = null;
                dtTrainings.Dispose();
            }
        }

        protected void btnEditClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnEditClear_Click");
            txtTrainingID.Text = String.Empty;
        }

        //Training Company Events
        protected void btnCompanyAdd_Click(object sender, EventArgs e)
        {
            DataTable dtTrainingCompanies = new DataTable(); 
            try
            {
                log.Debug("btnCompanyAdd_Click()");
                Boolean Update = false;

                dtTrainingCompanies = (Session["dtTrainingCompanies"] as DataTable).Copy();

                DataRow drTrainingCompany = dtTrainingCompanies.NewRow();

                string CompanyID = ddlCompany.SelectedValue.ToString().Trim();
                //if (CompanyID == String.Empty)
                //{
                //    CommonVariables.MESSAGE_TEXT = "Company is Required";
                //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                //    return;
                //}
                string CompanyName = ddlCompany.SelectedItem.Text.Trim();
                string PlannedParticipants = txtPlannedCompanyparticipants.Text.Trim();
                string ActualParticipants = txtActualCompanyParticipants.Text.Trim();
                string Description = txtDescription.Text.Trim();
                string Status = ddlTrainingCompanyStatus.SelectedValue.ToString().Trim();

                drTrainingCompany["COMPANY_ID"] = CompanyID;
                drTrainingCompany["COMP_NAME"] = CompanyName;
                drTrainingCompany["PLANNED_PARTICIPANTS"] = PlannedParticipants;
                drTrainingCompany["ACTUAL_PARTICIPANTS"] = ActualParticipants;
                drTrainingCompany["DESCRIPTION"] = Description;
                drTrainingCompany["STATUS_CODE"] = Status;

                DataRow[] drCheckExistance = dtTrainingCompanies.Select("COMPANY_ID = '" + CompanyID + "'");
                if (drCheckExistance.Length > 0)
                {
                    if (btnCompanyAdd.Text == "Add")
                    {
                        CommonVariables.MESSAGE_TEXT = "Record Already Exists";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }
                    else
                    {
                        string SelectedCompanyID = HttpUtility.HtmlDecode(grdvCompany.Rows[grdvCompany.SelectedIndex].Cells[0].Text).Trim();
                        string NewCompanyID = ddlCompany.SelectedValue.Trim();

                        if (SelectedCompanyID != NewCompanyID)
                        {
                            CommonVariables.MESSAGE_TEXT = "Record Already Exists";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                            return;
                        }
                        else
                        {
                            Update = true;
                        }
                    }
                }
                if (Update == false)
                {
                    dtTrainingCompanies.Rows.Add(drTrainingCompany);
                }
                else
                {
                    drCheckExistance[0]["COMPANY_ID"] = ddlCompany.SelectedValue.Trim();
                    drCheckExistance[0]["COMP_NAME"] = ddlCompany.SelectedItem.Text.Trim();
                    drCheckExistance[0]["PLANNED_PARTICIPANTS"] = txtPlannedCompanyparticipants.Text.Trim();
                    drCheckExistance[0]["ACTUAL_PARTICIPANTS"] = txtActualCompanyParticipants.Text.Trim();
                    drCheckExistance[0]["DESCRIPTION"] = txtDescription.Text.Trim();
                    drCheckExistance[0]["STATUS_CODE"] = ddlTrainingCompanyStatus.SelectedValue.Trim();
                }

                Session["dtTrainingCompanies"] = dtTrainingCompanies.Copy();

                grdvCompany.DataSource = dtTrainingCompanies.Copy();
                grdvCompany.DataBind();

                ClearCompanySub();
            }
            catch (Exception exp)
            {
                log.Error("btnCompanyAdd_Click() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtTrainingCompanies.Dispose();
            }
        }

        protected void btnCompanyClear_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("btnCompanyClear_Click()");
                ClearCompanySub();
            }
            catch (Exception exp)
            {
                log.Error("btnCompanyClear_Click()");
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            { 
            
            }
        }

        protected void chkExclude_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dtTrainingCompanies = new DataTable();
            try
            {
                log.Debug("chkExclude_CheckedChanged()");
                dtTrainingCompanies = (Session["dtTrainingCompanies"] as DataTable).Copy();

                for (int i = 0; i < grdvCompany.Rows.Count; i++)
                {
                    CheckBox chkExclude = (grdvCompany.Rows[i].FindControl("chkExclude") as CheckBox);

                    if (chkExclude.Checked == true)
                    {
                        string CompanyID = HttpUtility.HtmlDecode(grdvCompany.Rows[i].Cells[0].Text).Trim();

                        DataRow[] drCompany = dtTrainingCompanies.Select("COMPANY_ID = '" + CompanyID + "'");
                        foreach (DataRow dr in drCompany)
                        {
                            dr.Delete();
                        }
                        
                    }
                }

                Session["dtTrainingCompanies"] = dtTrainingCompanies.Copy();
                grdvCompany.DataSource = dtTrainingCompanies.Copy();
                grdvCompany.DataBind();
            }
            catch (Exception exp)
            {
                log.Error("chkExclude_CheckedChanged() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtTrainingCompanies.Dispose();
            }
        }

        protected void grdvCompany_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("OnClick", this.ClientScript.GetPostBackEventReference(this.grdvCompany, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor=this.originalstyle");
                e.Row.Attributes.Add("OnMouseOver", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                e.Row.Attributes.Add("Style", "Cursor:Pointer;");
            }
        }

        protected void grdvCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtTrainingCompanies = new DataTable();
            try
            {
                log.Debug("grdvCompany_SelectedIndexChanged()");
                dtTrainingCompanies = (Session["dtTrainingCompanies"] as DataTable).Copy();

                ClearCompanySub();

                string CompanyID = HttpUtility.HtmlDecode(grdvCompany.Rows[grdvCompany.SelectedIndex].Cells[0].Text).Trim();

                DataRow[] drCompany = dtTrainingCompanies.Select("COMPANY_ID = '" + CompanyID + "'");
                if (drCompany.Length > 0)
                {
                    btnCompanyAdd.Text = "Update";

                    string PlannedParticipants = drCompany[0]["PLANNED_PARTICIPANTS"].ToString();
                    string ActualParticipants = drCompany[0]["ACTUAL_PARTICIPANTS"].ToString();
                    string Description = drCompany[0]["DESCRIPTION"].ToString();
                    string StatusCode = drCompany[0]["STATUS_CODE"].ToString();

                    ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(CompanyID));
                    txtPlannedCompanyparticipants.Text = PlannedParticipants.Trim();
                    txtActualCompanyParticipants.Text = ActualParticipants.Trim();
                    txtDescription.Text = Description.Trim();
                    ddlTrainingCompanyStatus.SelectedIndex = ddlTrainingCompanyStatus.Items.IndexOf(ddlTrainingCompanyStatus.Items.FindByValue(StatusCode));
                }
            }
            catch (Exception exp)
            {
                log.Error("grdvCompany_SelectedIndexChanged() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtTrainingCompanies.Dispose();
            }
        }

        protected void grdvCompany_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dtTrainingCompanies = new DataTable();
            try
            {
                log.Debug("Training | grdvCompany_PageIndexChanging()");
                grdvCompany.PageIndex = e.NewPageIndex;

                dtTrainingCompanies = (Session["dtTrainingCompanies"] as DataTable).Copy();
                grdvCompany.DataSource = dtTrainingCompanies.Copy();
                grdvCompany.DataBind();
            }
            catch (Exception exp)
            {
                log.Error("Training | grdvCompany_PageIndexChanging() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtTrainingCompanies.Dispose();
            }

        }

        //Training Trainer Events

        protected void btnTrainerAdd_Click(object sender, EventArgs e)
        {
            DataTable dtTrainingTrainers = new DataTable();
            try
            {
                log.Debug("Training | btnTrainerAdd_Click()");
                Boolean Update = false;

                dtTrainingTrainers = (Session["dtTrainingTrainers"] as DataTable).Copy();
                DataRow dr = dtTrainingTrainers.NewRow();

                dr["TRAINER_ID"] = txtTrainerID.Text.Trim();
                dr["NAME_WITH_INITIALS"] = lblTrainerName.Text.Trim();
                dr["SELECTED_REASON"] = txtSelectedReason.Text.Trim();
                dr["STATUS_CODE"] = ddlTrainerStatus.SelectedValue.Trim();

                DataRow[] drCheckExistance = dtTrainingTrainers.Select("TRAINER_ID = '" + txtTrainerID.Text.Trim() + "'");

                if (drCheckExistance.Length > 0)
                {
                    if (btnTrainerAdd.Text == "Add")
                    {
                        CommonVariables.MESSAGE_TEXT = "Record Already Exists";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }
                    else
                    {
                        string SelectedTrainerID = HttpUtility.HtmlDecode(grdvTrainers.Rows[grdvTrainers.SelectedIndex].Cells[1].Text).Trim();
                        string NewTrainerID = txtTrainerID.Text.Trim();

                        if (SelectedTrainerID != NewTrainerID)
                        {
                            CommonVariables.MESSAGE_TEXT = "Record Already Exists";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                            return;
                        }
                        else
                        {
                            Update = true;
                        }
                    }
                }
                if (Update == false)
                {
                    dtTrainingTrainers.Rows.Add(dr);
                }
                else
                {
                    drCheckExistance[0]["TRAINER_ID"] = txtTrainerID.Text.Trim();
                    drCheckExistance[0]["NAME_WITH_INITIALS"] = lblTrainerName.Text.Trim();
                    drCheckExistance[0]["SELECTED_REASON"] = txtSelectedReason.Text.Trim();
                    drCheckExistance[0]["STATUS_CODE"] = ddlTrainerStatus.SelectedValue.Trim();
                }

                
                Session["dtTrainingTrainers"] = dtTrainingTrainers.Copy();

                grdvTrainers.DataSource = dtTrainingTrainers.Copy();
                grdvTrainers.DataBind();

                ClearTrainerSub();

            }
            catch (Exception exp)
            {
                log.Error("Training | btnTrainerAdd_Click() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtTrainingTrainers.Dispose();
            }
        }

        protected void btnTrainerClear_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("Training | btnTrainerClear_Click()");

                ClearTrainerSub();
            }
            catch (Exception exp)
            {
                log.Error("Training | btnTrainerClear_Click() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void chkTrainerExclude_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dtTrainingTrainers = new DataTable();
            try
            {
                log.Debug("Training | chkTrainerExclude_CheckedChanged()");
                dtTrainingTrainers = (Session["dtTrainingTrainers"] as DataTable).Copy();

                for (int i = 0; i < grdvTrainers.Rows.Count; i++)
                {
                    CheckBox chkTrainerExclude = (grdvTrainers.Rows[i].FindControl("chkTrainerExclude") as CheckBox);
                    if (chkTrainerExclude.Checked == true)
                    {
                        string TrainerID = HttpUtility.HtmlDecode(grdvTrainers.Rows[i].Cells[1].Text).Trim();
                        DataRow[] drArr = dtTrainingTrainers.Select("TRAINER_ID = '" + TrainerID + "'");
                        
                        foreach (DataRow dr in drArr)
                        {
                            dr.Delete();
                        }
                    }

                }

                Session["dtTrainingTrainers"] = dtTrainingTrainers.Copy();

                grdvTrainers.DataSource = dtTrainingTrainers.Copy();
                grdvTrainers.DataBind();
            }
            catch (Exception exp)
            {
                log.Error("Training | chkTrainerExclude_CheckedChanged() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtTrainingTrainers.Dispose();
            }
        }

        protected void grdvTrainers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("OnClick", this.ClientScript.GetPostBackEventReference(this.grdvTrainers, "Select$" + e.Row.RowIndex));
                    e.Row.Attributes.Add("Style", "Cursor:Pointer");
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

        protected void grdvTrainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Error("Training | grdvTrainers_SelectedIndexChanged()");

                ClearTrainerSub();

                string TrainerID = HttpUtility.HtmlDecode(grdvTrainers.Rows[grdvTrainers.SelectedIndex].Cells[1].Text).Trim();
                string TrainerName = HttpUtility.HtmlDecode(grdvTrainers.Rows[grdvTrainers.SelectedIndex].Cells[2].Text).Trim();
                string SelectedReason = HttpUtility.HtmlDecode(grdvTrainers.Rows[grdvTrainers.SelectedIndex].Cells[3].Text).Trim();
                string Status = HttpUtility.HtmlDecode(grdvTrainers.Rows[grdvTrainers.SelectedIndex].Cells[4].Text).Trim();

                txtTrainerID.Text = TrainerID.Trim();
                lblTrainerName.Text = TrainerName.Trim();
                txtSelectedReason.Text = SelectedReason.Trim();
                ddlTrainerStatus.SelectedIndex = ddlTrainerStatus.Items.IndexOf(ddlTrainerStatus.Items.FindByValue(Status.Trim()));

                btnTrainerAdd.Text = "Update";
            }
            catch (Exception exp)
            {
                log.Error("Training | chkTrainerExclude_CheckedChanged() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load Trainer Status to TrainerStatus Drop DownList
        /// </summary>
        void LoadTrainerStatus()
        {
            try
            {
                ddlTrainerStatus.Items.Clear();

                ddlTrainerStatus.Items.Add(new ListItem("", ""));
                ddlTrainerStatus.Items.Add(new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                ddlTrainerStatus.Items.Add(new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { 
            
            }
        }

        void CreateTrainingCompanyDataTable()
        {
            DataTable dtTrainingCompanies = new DataTable();
            try
            {
                log.Debug("CreateTrainingCompanyDataTable()");

                dtTrainingCompanies.Columns.Add("TRAINING_ID");
                dtTrainingCompanies.Columns.Add("COMPANY_ID");
                dtTrainingCompanies.Columns.Add("COMP_NAME");
                dtTrainingCompanies.Columns.Add("PLANNED_PARTICIPANTS");
                dtTrainingCompanies.Columns.Add("ACTUAL_PARTICIPANTS");
                dtTrainingCompanies.Columns.Add("DESCRIPTION");
                dtTrainingCompanies.Columns.Add("STATUS_CODE");

                Session["dtTrainingCompanies"] = dtTrainingCompanies.Copy();
            }
            catch (Exception ex)
            {
                log.Error("CreateTrainingCompanyDataTable() | " + ex.Message);
                throw ex;
            }
            finally
            {
                dtTrainingCompanies.Dispose();
            }
        }

        void CreateTrainingTrainerDataTable()
        {
            DataTable dtTrainingTrainers = new DataTable();
            try
            {
                log.Debug("CreateTrainingTrainerDataTable()");

                dtTrainingTrainers.Columns.Add("TRAINING_ID");
                dtTrainingTrainers.Columns.Add("TRAINER_ID");
                dtTrainingTrainers.Columns.Add("NAME_WITH_INITIALS");
                dtTrainingTrainers.Columns.Add("SELECTED_REASON");
                dtTrainingTrainers.Columns.Add("STATUS_CODE");

                Session["dtTrainingTrainers"] = dtTrainingTrainers.Copy();
            }
            catch (Exception ex)
            {
                log.Error("CreateTrainingTrainerDataTable() | " + ex.Message);
                throw ex;
            }
            finally
            {
                dtTrainingTrainers.Dispose();
            }
        }

        void LoadCompanies()
        {
            TrainingDataHandler TDH = new TrainingDataHandler();
            DataTable dtCompanies = new DataTable();
            try
            {
                log.Debug("LoadCompanies()");
                ddlCompany.Items.Clear();

                ddlCompany.Items.Add(new ListItem("", ""));

                dtCompanies = TDH.PopulateCompanies().Copy();

                for (int i = 0; i < dtCompanies.Rows.Count; i++)
                {
                    string Text = dtCompanies.Rows[i]["COMP_NAME"].ToString();
                    string Value = dtCompanies.Rows[i]["COMPANY_ID"].ToString();

                    ddlCompany.Items.Add(new ListItem(Text, Value));
                }
            }
            catch (Exception ex)
            {
                log.Error("LoadCompanies() | " + ex.Message);
                throw ex;
            }
            finally
            {
                TDH = null;
            }
        }

        void LoadCompanyStatus()
        {
            try
            {
                log.Debug("LoadCompanyStatus()");
                ddlTrainingCompanyStatus.Items.Clear();

                ddlTrainingCompanyStatus.Items.Add(new ListItem("", ""));

                ddlTrainingCompanyStatus.Items.Add(new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                ddlTrainingCompanyStatus.Items.Add(new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));
            }
            catch (Exception ex)
            {
                log.Error("LoadCompanyStatus() | " + ex.Message);         
                throw ex;
            }
        }

        Boolean isInvalidCharExists()
        {
            Boolean Status = false;
            try
            {
                log.Debug("CharCheck()");
                string SpecialCharSet = "!@#$%^&*()_+=`~'";
                string CheckString = txtTrainingName.Text.Trim();

                for (int i = 0; i < SpecialCharSet.Length; i++)
                {
                    char SelectedChar = SpecialCharSet[i];

                    Status = CheckString.Contains(SelectedChar);
                    if (Status)
                    {
                        return Status;
                    }
                }
                return Status;
            }
            catch (Exception exp)
            {
                log.Error("CharCheck() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
                return Status;
            }
            finally
            {
                
            }
        }

        void ClearFields()
        {
            DataTable dtTrainingCompanies = new DataTable();
            DataTable dtTrainingTrainers = new DataTable();
            try
            {
                log.Debug("ClearFields()");

                Utility.Utils.clearControls(true, txtTrainingName, txtTrainingCode, ddlTrainingProgram, ddlTrainingType, txtPlannedStartDate, txtPlannedEndDate, txtPlannedTotalHours, /*txtPlannedParticipants,*/ txtActualStartDate, txtActualEndDate, txtActualTotalHours, /*txtActualParticipants,*/ chkIsOutOfBudget, chkIsPostponed, txtPostpanedReason, ddlStatusCode, ddlActualTotalMins, ddlPlannedTotalMins);
                txtPostpanedReason.Enabled = false;
                Utility.Errorhandler.ClearError(lblMessage);
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                ClearCompanySub();
                ClearTrainerSub();

                dtTrainingCompanies = (Session["dtTrainingCompanies"] as DataTable).Copy();
                dtTrainingTrainers = (Session["dtTrainingTrainers"] as DataTable).Copy();

                dtTrainingCompanies.Rows.Clear();
                dtTrainingTrainers.Rows.Clear();

                Session["dtTrainingCompanies"] = dtTrainingCompanies.Copy();
                Session["dtTrainingTrainers"] = dtTrainingTrainers.Copy();

                grdvCompany.DataSource = null;
                grdvCompany.DataBind();

                grdvTrainers.DataSource = null;
                grdvTrainers.DataBind();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtTrainingCompanies.Dispose();
                dtTrainingTrainers.Dispose();
            }
        }

        void ClearCompanySub()
        {
            try
            {
                log.Debug("ClearCompanySub()");
                Utility.Utils.clearControls(true, ddlCompany, txtPlannedCompanyparticipants, txtActualCompanyParticipants, txtDescription, ddlTrainingCompanyStatus);
                btnCompanyAdd.Text = "Add";
            }
            catch (Exception ex)
            {
                log.Error("ClearCompanySub() | " + ex.Message);
                throw ex;
            }
            finally
            { 
            
            }
        }

        void ClearTrainerSub()
        {
            try
            {
                log.Debug("ClearCompanySub()");
                Utility.Utils.clearControls(true, txtTrainerID, lblTrainerName, txtSelectedReason, ddlTrainerStatus);
                btnTrainerAdd.Text = "Add";
            }
            catch (Exception ex)
            {
                log.Error("ClearCompanySub() | " + ex.Message);
                throw ex;
            }
            finally
            {

            }
        }

        void LoadMinutes(DropDownList ddlMins)
        {
            try
            {
                log.Debug("LoadMinutes()");

                ddlMins.Items.Clear();
                ddlMins.Items.Add(new ListItem("", ""));
                for (int i = 0; i < 60; i++)
                {
                    ddlMins.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
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

        void SetPostponedTextBox()
        {
            try
            {
                log.Debug("SetPostponedTextBox()");

                if (chkIsPostponed.Checked == true)
                {
                    RequiredFieldValidator2.ControlToValidate = "txtPostpanedReason";
                    txtPostpanedReason.Enabled = true;
                }
                else
                {
                    RequiredFieldValidator2.ControlToValidate = "txtTempPostponedReason";
                    txtPostpanedReason.Enabled = false;
                    txtPostpanedReason.Text = String.Empty;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void LoadTrainingPrograms()
        {
            TrainingDataHandler TDH = new TrainingDataHandler();
            DataTable dtTrainingPrograms = new DataTable();
            try
            {
                log.Debug("LoadTrainingPrograms()");

                ddlTrainingProgram.Items.Clear();
                dtTrainingPrograms = TDH.PopulateTrainingProgramTypes();

                if (dtTrainingPrograms.Rows.Count > 0)
                {
                    ddlTrainingProgram.Items.Add(new ListItem("", ""));

                    for (int i = 0; i < dtTrainingPrograms.Rows.Count; i++)
                    {
                        string Text = dtTrainingPrograms.Rows[i]["PROGRAM_NAME"].ToString();
                        string Value = dtTrainingPrograms.Rows[i]["PROGRAM_ID"].ToString();

                        ddlTrainingProgram.Items.Add(new ListItem(Text, Value));
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                TDH = null;
                dtTrainingPrograms.Dispose();
            }
        }

        void LoadTrainingTypes()
        {
            TrainingDataHandler TDH = new TrainingDataHandler();
            DataTable dtTrainingTypes = new DataTable();
            try
            {
                log.Debug("LoadTrainingTypes()");

                ddlTrainingType.Items.Clear();
                dtTrainingTypes = TDH.PopulateTrainingTypes();

                if (dtTrainingTypes.Rows.Count > 0)
                {
                    ddlTrainingType.Items.Add(new ListItem("", ""));

                    for (int i = 0; i < dtTrainingTypes.Rows.Count; i++)
                    {
                        string Text = dtTrainingTypes.Rows[i]["TYPE_NAME"].ToString();
                        string Value = dtTrainingTypes.Rows[i]["TRAINING_TYPE_ID"].ToString();

                        ddlTrainingType.Items.Add(new ListItem(Text, Value));
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                TDH = null;
                dtTrainingTypes.Dispose();
            }
        }

        void LoadStatus()
        {
            try
            {
                log.Debug("LoadStatus()");

                ddlStatusCode.Items.Clear();
                ddlStatusCode.Items.Add(new ListItem("", ""));
                ddlStatusCode.Items.Add(new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                ddlStatusCode.Items.Add(new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void LoadTrainings()
        {
            TrainingDataHandler TDH = new TrainingDataHandler();
            DataTable dtTrainings = new DataTable();
            try
            {
                log.Debug("LoadTrainings()");

                dtTrainings = TDH.Populate().Copy();
                grdvTraining.DataSource = dtTrainings.Copy();
                grdvTraining.DataBind();
                Session["dtTrainings"] = dtTrainings.Copy();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                TDH = null;
                dtTrainings.Dispose();
            }
        }

        void LoadTrainingCompanies(string TrainingID)
        {
            TrainingDataHandler TDH = new TrainingDataHandler();
            DataTable dtTrainingCompanies = new DataTable(); 
            try
            {
                log.Debug("Training | LoadTrainingCompanies()");

                dtTrainingCompanies = TDH.PopulateTrainingCompanies(TrainingID).Copy();

                Session["dtTrainingCompanies"] = dtTrainingCompanies.Copy();
                grdvCompany.DataSource = dtTrainingCompanies.Copy();
                grdvCompany.DataBind();
            }
            catch (Exception ex)
            {
                log.Error("Training | LoadTrainingCompanies()");
                throw ex;
            }
            finally
            {
                TDH = null;
                dtTrainingCompanies.Dispose();
            }
        }

        void LoadTrainingTrainers(string TrainingID)
        {
            TrainingDataHandler TDH = new TrainingDataHandler();
            DataTable dtTrainingTrainers = new DataTable();
            try
            {
                log.Debug("Training | LoadTrainingTrainers()");

                dtTrainingTrainers = TDH.PopulateTrainingTrainers(TrainingID).Copy();

                Session["dtTrainingTrainers"] = dtTrainingTrainers.Copy();
                grdvTrainers.DataSource = dtTrainingTrainers.Copy();
                grdvTrainers.DataBind();
            }
            catch (Exception ex)
            {
                log.Error("Training | LoadTrainingTrainers()");
                throw ex;
            }
            finally
            {
                TDH = null;
                dtTrainingTrainers.Dispose();
            }
        }

        #endregion     
    }
}