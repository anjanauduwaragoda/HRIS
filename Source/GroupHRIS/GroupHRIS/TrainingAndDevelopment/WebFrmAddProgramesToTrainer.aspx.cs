using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using DataHandler.TrainingAndDevelopment;
using NLog;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmAddProgramesToTrainer : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        string btnAddProgramText = "Add";
        string btnUpdateStatusText = "Update";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmCompetencyGroup : Page_Load");
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
                loadProgrameGridView();
                fillStatus();
                tblProgrameDetails.Visible = false;
                

                fillTrainingType();
                fillTrainingCategory();

                hfTrainerId.Value = "";
                //initializeTrainerData();
                if (Page.PreviousPage != null)
                {
                    initializeTrainerData();
                    //hfTrainerId.Value = ((TextBox)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$txtInstituteId")).Text.Trim();
                    //lblName.Text = ((TextBox)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$txtInstituteName")).Text.Trim();
                    //lblContact.Text = ((TextBox)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$txtContact_1")).Text.Trim();
                    //lblAddress.Text = ((TextBox)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$txtAddress")).Text.Trim();
                }
                loadAddedProgramesGridView();
            }
        }

        #region methodes

        private void initializeTrainerData()
        {
            TrainerInformationDataHandler trainerInformationDataHandler = new TrainerInformationDataHandler();
            DataTable dtTrainerDetails = new DataTable();

            log.Debug("initializeTrainerData()");
            try
            {
                lblName.Text = "Sample Name";
                lblAddress.Text = "Sample Address";
                hfTrainerId.Value = ((HiddenField)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$hfTrainerId")).Value.ToString().Trim();

                dtTrainerDetails = trainerInformationDataHandler.getTrainerById(hfTrainerId.Value.ToString());
                lblName.Text = dtTrainerDetails.Rows[0]["NAME_WITH_INITIALS"].ToString();

                lblContact.Text = dtTrainerDetails.Rows[0]["CONTACT_MOBILE"].ToString();
                lblAddress.Text = dtTrainerDetails.Rows[0]["ADDRESS"].ToString();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                trainerInformationDataHandler = null;
                dtTrainerDetails.Dispose();
            }
        }

        private void loadProgrameGridView()
        {
            log.Debug("loadProgrameGridView()");

            AddProgramesToTrainerDataHandler addProgramesToTrainerDataHandler = new AddProgramesToTrainerDataHandler();
            DataTable dtPrograms = new DataTable();

            try
            {
                dtPrograms = addProgramesToTrainerDataHandler.getAllPrograms();
                gvAllProgrammes.DataSource = dtPrograms;
                gvAllProgrammes.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                addProgramesToTrainerDataHandler = null;
                dtPrograms.Dispose();
            }

        }

        private void populateProgramDetails(DataTable dtProgramDetail)
        {
            log.Debug("populateProgramDetails()");
            try
            {
                DataRow programDetails = dtProgramDetail.Rows[0];
                lblCode.Text = programDetails["PROGRAM_CODE"].ToString();
                lblPrgmName.Text = programDetails["PROGRAM_NAME"].ToString();
                lblDesc.Text = programDetails["DESCRIPTION"].ToString();
                lblMaximum.Text = programDetails["MAXIMUM_BATCH_SIZE"].ToString();
                lblMinimum.Text = programDetails["MINIMUM_BATCH_SIZE"].ToString();
                lblDuration.Text = programDetails["PROGRAM_DURATION"].ToString() + " Hrs";
                lblObjectives.Text = programDetails["OBJECTIVES"].ToString();
                lblType.Text = programDetails["PROGRAM_TYPE"].ToString();

            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void populateTrainerProgrameDetail(DataTable dtTrainerProgramDetail)
        {
            log.Debug("populateTrainerProgrameDetail()");
            try
            {
                txtDescription.Text = dtTrainerProgramDetail.Rows[0]["DESCRIPTION"].ToString();
                txtCost.Text = dtTrainerProgramDetail.Rows[0]["COST_PER_SESSION"].ToString();
                ddlStatus.SelectedValue = dtTrainerProgramDetail.Rows[0]["STATUS_CODE"].ToString();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void loadAddedProgramesGridView()
        {
            log.Debug("loadAddedProgramesGridView()");

            AddProgramesToTrainerDataHandler addProgramesToTrainerDataHandler = new AddProgramesToTrainerDataHandler();
            DataTable dtAddeddProgrames = new DataTable();

            try
            {
                string trainerId = hfTrainerId.Value.ToString();
                dtAddeddProgrames = addProgramesToTrainerDataHandler.getAddedProgrames(trainerId);

                gvAddedProgrames.DataSource = dtAddeddProgrames;
                gvAddedProgrames.DataBind();

            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                addProgramesToTrainerDataHandler = null;
                dtAddeddProgrames.Dispose();
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

        private void fillTrainingType()
        {
            log.Debug("fillTrainingType()");

            AddProgramesToTrainerDataHandler addProgramesToTrainerDataHandler = new AddProgramesToTrainerDataHandler();
            DataTable dtTrainingTypes = new DataTable();
            try
            {
                dtTrainingTypes = addProgramesToTrainerDataHandler.getTrainingTypes();

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlTrainingType.Items.Add(listItemBlank);

                if (dtTrainingTypes != null)
                {
                    if (dtTrainingTypes.Rows.Count > 0)
                    {
                        foreach (DataRow type in dtTrainingTypes.Rows)
                        {
                            ListItem newItem = new ListItem();
                            newItem.Text = type[1].ToString();
                            newItem.Value = type[0].ToString();
                            ddlTrainingType.Items.Add(newItem);
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                addProgramesToTrainerDataHandler = null;
                dtTrainingTypes.Dispose();
            }
        }

        private void fillTrainingCategory()
        {
            log.Debug("fillTrainingCategory()");

            AddProgramesToTrainerDataHandler addProgramesToTrainerDataHandler = new AddProgramesToTrainerDataHandler();
            DataTable dtTrainingCategories = new DataTable();
            try
            {
                dtTrainingCategories = addProgramesToTrainerDataHandler.getTrainingCategories();

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlTrainingCategory.Items.Add(listItemBlank);

                if (dtTrainingCategories != null)
                {
                    if (dtTrainingCategories.Rows.Count > 0)
                    {
                        foreach (DataRow type in dtTrainingCategories.Rows)
                        {
                            ListItem newItem = new ListItem();
                            newItem.Text = type[1].ToString();
                            newItem.Value = type[0].ToString();
                            ddlTrainingCategory.Items.Add(newItem);
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                addProgramesToTrainerDataHandler = null;
                dtTrainingCategories.Dispose();
            }
        }

        private void filterProgrames()
        {
            log.Debug("filterProgrames()");

            DataTable dtFilteredResult = new DataTable();
            AddProgramesToTrainerDataHandler addProgramesToTrainerDataHandler = new AddProgramesToTrainerDataHandler();


            try
            {
                string selectedType = ddlTrainingType.SelectedValue.ToString();
                string selectedCategory = ddlTrainingCategory.SelectedValue.ToString();

                dtFilteredResult = addProgramesToTrainerDataHandler.filterProgrames(selectedType, selectedCategory);
                if (dtFilteredResult != null)
                {
                    if (dtFilteredResult.Rows.Count > 0)
                    {
                        gvAllProgrammes.DataSource = dtFilteredResult;
                        gvAllProgrammes.DataBind();
                    }
                    else
                    {
                        gvAllProgrammes.DataSource = null;
                        gvAllProgrammes.DataBind();
                    }
                }

            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                addProgramesToTrainerDataHandler = null;
                dtFilteredResult.Dispose();
            }
        }
        #endregion

        #region events
        /// All Programmes gridview events begin ///
        protected void gvAllProgrammes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvAllProgrammes_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvAllProgrammes, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void gvAllProgrammes_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gvAllProgrammes_SelectedIndexChanged()");

            AddProgramesToTrainerDataHandler addProgramesToTrainerDataHandler = new AddProgramesToTrainerDataHandler();
            DataTable dtProgramDetail = new DataTable();
            try
            {
                int index = gvAllProgrammes.SelectedIndex;
                string programId = gvAllProgrammes.Rows[index].Cells[0].Text.ToString();

                dtProgramDetail = addProgramesToTrainerDataHandler.getProgramById(programId);
                tblProgrameDetails.Visible = true;
                lblStatus.Visible = false;
                ddlStatus.Visible = false;
                btnAdd.Text = btnAddProgramText;
                populateProgramDetails(dtProgramDetail);
                Utility.Errorhandler.ClearError(lblErrorMsg);

            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                dtProgramDetail.Dispose();
                addProgramesToTrainerDataHandler = null;
            }


        }

        protected void gvAllProgrammes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvAllProgrammes_PageIndexChanging()");

            try
            {
                gvAllProgrammes.PageIndex = e.NewPageIndex;
                loadProgrameGridView();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }
        /// All Programmes gridview events end ///

        /// Added Programmes gridview events begin ///
        protected void gvAddedProgrames_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvAddedProgrames_RowDataBound()");

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvAddedProgrames, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void gvAddedProgrames_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gvAddedProgrames_SelectedIndexChanged()");

            AddProgramesToTrainerDataHandler addProgramesToTrainerDataHandler = new AddProgramesToTrainerDataHandler();
            try
            {
                int index = gvAddedProgrames.SelectedIndex;
                string programId = gvAddedProgrames.Rows[index].Cells[0].Text.ToString();
                
                string programStatus = gvAddedProgrames.Rows[index].Cells[3].Text.ToString();

                DataTable dtProgramDetail = addProgramesToTrainerDataHandler.getProgramById(programId);
                tblProgrameDetails.Visible = true;
                lblStatus.Visible = true;
                ddlStatus.Visible = true;
                
                string trainerId = hfTrainerId.Value.ToString();
                DataTable dtTrainerProgramDetail = addProgramesToTrainerDataHandler.getTrainerProgrameDetails(trainerId, programId);

                //if (programStatus == Constants.STATUS_ACTIVE_TAG)
                //{
                //    ddlStatus.SelectedValue = Constants.STATUS_ACTIVE_VALUE;
                //}
                //if (programStatus == Constants.STATUS_INACTIVE_TAG)
                //{
                //    ddlStatus.SelectedValue = Constants.STATUS_INACTIVE_VALUE;
                //}
                btnAdd.Text = btnUpdateStatusText;
                populateProgramDetails(dtProgramDetail);
                populateTrainerProgrameDetail(dtTrainerProgramDetail);
                Utility.Errorhandler.ClearError(lblErrorMsg);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                addProgramesToTrainerDataHandler = null;
            }
        }

        protected void gvAddedProgrames_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvAddedProgrames_PageIndexChanging()");
            try
            {
                gvAddedProgrames.PageIndex = e.NewPageIndex;
                loadAddedProgramesGridView();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }
        /// Added Programmes gridview events end ///
        /// 
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            log.Debug("ImageButton1_Click()");

            tblProgrameDetails.Visible = false;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            log.Debug("btnAdd_Click()");

            AddProgramesToTrainerDataHandler addProgramesToTrainerDataHandler = new AddProgramesToTrainerDataHandler();
            try
            {

                string trainerId = hfTrainerId.Value.ToString();
                string addedUserId = Session["KeyUSER_ID"].ToString();
                string status = "";
                string costPerSession = txtCost.Text.ToString();
                string description = txtDescription.Text.ToString();

                if (btnAdd.Text == btnAddProgramText)
                {
                    int index = gvAllProgrammes.SelectedIndex;
                    string programId = gvAllProgrammes.Rows[index].Cells[0].Text.ToString();
                    status = Constants.STATUS_ACTIVE_VALUE;

                    bool programExsist = addProgramesToTrainerDataHandler.checkProgrameExistance(trainerId, programId);
                    if (!programExsist)
                    {
                        bool inserted = addProgramesToTrainerDataHandler.addProgrameToTrainer(trainerId, programId, costPerSession, description, status, addedUserId);
                        if (inserted)
                        {
                            CommonVariables.MESSAGE_TEXT = " Programe Successfully added";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            tblProgrameDetails.Visible = false;
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = " Couldn't add the programe";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = " Program already exist";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                    loadAddedProgramesGridView();
                }
                else if (btnAdd.Text == btnUpdateStatusText)
                {
                    int index = gvAddedProgrames.SelectedIndex;
                    string programId2 = gvAddedProgrames.Rows[index].Cells[0].Text.ToString();
                    status = ddlStatus.SelectedValue;
                    bool updated = addProgramesToTrainerDataHandler.addProgrameStatusInTrainer(trainerId, programId2, costPerSession, description, status, addedUserId);
                    if (updated)
                    {
                        CommonVariables.MESSAGE_TEXT = " Programe Successfully updated";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        tblProgrameDetails.Visible = false;
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = " Couldn't update the programe";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                    loadAddedProgramesGridView();
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                addProgramesToTrainerDataHandler = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");
            tblProgrameDetails.Visible = false;
        }

        protected void ddlTrainingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlTrainingType_SelectedIndexChanged()");
            try
            {
                filterProgrames();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void ddlTrainingCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlTrainingCategory_SelectedIndexChanged()");
            try
            {
                filterProgrames();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        //protected void lbFindInstitute_Click(object sender, EventArgs e)
        //{
        //    log.Debug("lbFindInstitute_Click()");
        //    Server.Transfer("~/TrainingAndDevelopment/WebFrmTrainingInstitutes.aspx");
        //}
        #endregion
    }
}