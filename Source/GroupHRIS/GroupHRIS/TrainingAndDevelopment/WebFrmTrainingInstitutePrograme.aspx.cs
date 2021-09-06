using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.TrainingAndDevelopment;
using Common;
using NLog;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainingInstitutePrograme : System.Web.UI.Page
    {
        string btnAddProgramText = "Add";
        string btnUpdateStatusText = "Update";

        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmTrainingInstitutePrograme : Page_Load");
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
                initializeInstituteData();
                
                fillTrainingType();
                fillTrainingCategory();

                hfInstituteId.Value = "";

                if (Page.PreviousPage != null)
                {
                    hfInstituteId.Value = ((TextBox)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$txtInstituteId")).Text.Trim();
                    lblName.Text = ((TextBox)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$txtInstituteName")).Text.Trim();
                    lblContact.Text = ((TextBox)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$txtContact_1")).Text.Trim();
                    lblAddress.Text = ((TextBox)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$txtAddress")).Text.Trim();                   
                }
                loadAddedProgramesGridView();
            }
        }
        
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
            TrainingInstituteProgrameDataHandler trainingInstituteProgrameDataHandler = new TrainingInstituteProgrameDataHandler();
            try
            {
                int index = gvAllProgrammes.SelectedIndex;
                string programId = gvAllProgrammes.Rows[index].Cells[0].Text.ToString();

                DataTable dtProgramDetail = trainingInstituteProgrameDataHandler.getProgramById(programId);
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
                trainingInstituteProgrameDataHandler = null;
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
            TrainingInstituteProgrameDataHandler trainingInstituteProgrameDataHandler = new TrainingInstituteProgrameDataHandler();
            try
            {
                int index = gvAddedProgrames.SelectedIndex;
                string programId = gvAddedProgrames.Rows[index].Cells[0].Text.ToString();

                string programStatus = gvAddedProgrames.Rows[index].Cells[3].Text.ToString();

                DataTable dtProgramDetail = trainingInstituteProgrameDataHandler.getProgramById(programId);
                tblProgrameDetails.Visible = true;
                lblStatus.Visible = true;
                ddlStatus.Visible = true;

                if (programStatus == Constants.STATUS_ACTIVE_TAG)
                {
                    ddlStatus.SelectedValue = Constants.STATUS_ACTIVE_VALUE;
                }
                if (programStatus == Constants.STATUS_INACTIVE_TAG)
                {
                    ddlStatus.SelectedValue = Constants.STATUS_INACTIVE_VALUE;
                }
                btnAdd.Text = btnUpdateStatusText;
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
                trainingInstituteProgrameDataHandler = null;
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
            TrainingInstituteProgrameDataHandler trainingInstituteProgrameDataHandler = new TrainingInstituteProgrameDataHandler();
            try
            {
                
                string instituteId = hfInstituteId.Value.ToString();
                string addedUserId = Session["KeyUSER_ID"].ToString();
                string status = "";
                if(btnAdd.Text == btnAddProgramText)
                {
                    int index = gvAllProgrammes.SelectedIndex;
                    string programId = gvAllProgrammes.Rows[index].Cells[0].Text.ToString();
                    status = Constants.STATUS_ACTIVE_VALUE;

                    bool programExsist = trainingInstituteProgrameDataHandler.checkProgrameExistance(instituteId, programId);
                    if (!programExsist)
                    {
                        bool inserted = trainingInstituteProgrameDataHandler.addProgrameToInstitute(instituteId, programId, status, addedUserId);
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
                    bool updated = trainingInstituteProgrameDataHandler.addProgrameStatusInInstitute(instituteId, programId2, status, addedUserId);
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
                trainingInstituteProgrameDataHandler = null;
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
                string categoryId = ddlTrainingCategory.SelectedValue.ToString();
                fillSubcategoryDropdown(categoryId);
                
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void lbFindInstitute_Click(object sender, EventArgs e)
        {
            log.Debug("lbFindInstitute_Click()");
            Server.Transfer("~/TrainingAndDevelopment/WebFrmTrainingInstitutes.aspx");
        }

        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            log.Debug("iBtnSearch_Click()");
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
        #endregion

        #region methodes

        private void initializeInstituteData()
        {
            try
            {
                //lblName.Text = "Sample Name";
                //lblAddress.Text = "Sample Address";
                //hfInstituteId.Value = "TI0000000002";
            }
            catch (Exception Ex)
            {
                
                throw;
            }
        }

        private void loadProgrameGridView()
        {
            TrainingInstituteProgrameDataHandler trainingInstituteProgrameDataHandler = new TrainingInstituteProgrameDataHandler();
            DataTable dtPrograms = new DataTable();

            try
            {
                dtPrograms = trainingInstituteProgrameDataHandler.getAllPrograms();
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
                trainingInstituteProgrameDataHandler = null;
                dtPrograms.Dispose();
            }
            
        }

        private void populateProgramDetails(DataTable dtProgramDetail)
        {
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

        private void loadAddedProgramesGridView()
        {
            TrainingInstituteProgrameDataHandler trainingInstituteProgrameDataHandler = new TrainingInstituteProgrameDataHandler();
            DataTable dtAddeddProgrames = new DataTable();

            try
            {
                string instituteId = hfInstituteId.Value.ToString();
                dtAddeddProgrames = trainingInstituteProgrameDataHandler.getAddedProgrames(instituteId);

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
                trainingInstituteProgrameDataHandler = null;
                dtAddeddProgrames.Dispose();
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

        private void fillTrainingType()
        {
            TrainingInstituteProgrameDataHandler trainingInstituteProgrameDataHandler = new TrainingInstituteProgrameDataHandler();
            DataTable dtTrainingTypes = new DataTable();
            try
            {
                dtTrainingTypes = trainingInstituteProgrameDataHandler.getTrainingTypes();

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
            catch (Exception)
            {

                throw;
            }
            finally
            {
                trainingInstituteProgrameDataHandler = null;
                dtTrainingTypes.Dispose();
            }
        }

        private void fillTrainingCategory()
        {
            TrainingInstituteProgrameDataHandler trainingInstituteProgrameDataHandler = new TrainingInstituteProgrameDataHandler();
            DataTable dtTrainingCategories = new DataTable();
            try
            {
                dtTrainingCategories = trainingInstituteProgrameDataHandler.getTrainingCategories();

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
                trainingInstituteProgrameDataHandler = null;
                dtTrainingCategories.Dispose();
            }
        }

        private void fillSubcategoryDropdown(string categoryId)
        {
            log.Debug("fillSubcategoryDropdown()");
            TrainingInstituteProgrameDataHandler trainingInstituteProgrameDataHandler = new TrainingInstituteProgrameDataHandler();
            DataTable dtSubcategory = new DataTable();
            try
            {
                ddlTrainingSubcategorySearch.Items.Clear();
                dtSubcategory = trainingInstituteProgrameDataHandler.getSubcategoriesForCategory(categoryId);

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlTrainingSubcategorySearch.Items.Add(listItemBlank);

                if (dtSubcategory != null)
                {
                    if (dtSubcategory.Rows.Count > 0)
                    {
                        foreach (DataRow type in dtSubcategory.Rows)
                        {
                            ListItem newItem = new ListItem();
                            newItem.Text = type[1].ToString();
                            newItem.Value = type[0].ToString();
                            ddlTrainingSubcategorySearch.Items.Add(newItem);

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
                trainingInstituteProgrameDataHandler = null;
                dtSubcategory.Dispose();
            }
        }

        private void filterProgrames()
        {

            DataTable dtFilteredResult = new DataTable();
            TrainingInstituteProgrameDataHandler trainingInstituteProgrameDataHandler = new TrainingInstituteProgrameDataHandler();

            try
            {
                string selectedType = ddlTrainingType.SelectedValue.ToString();
                string selectedCategory = ddlTrainingCategory.SelectedValue.ToString();
                string selectedSubcategory = ddlTrainingSubcategorySearch.SelectedValue.ToString();

                dtFilteredResult = trainingInstituteProgrameDataHandler.filterProgrames(selectedType, selectedCategory, selectedSubcategory);
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
                trainingInstituteProgrameDataHandler = null;
                dtFilteredResult.Dispose();
            }
        }
        #endregion

        

        

        

        
        

        

        

        





        

        

        
    }
}