using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.TrainingAndDevelopment;
using System.Data;
using GroupHRIS.Utility;
using DataHandler.Utility;
using NLog;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainingPrograme : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        #region events

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmTrainingPrograme : Page_Load");
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
                fillTrainingCategory();
                fillTrainingType();
                fillProgrameType();
                fillTrainingProgrameGridView();
                fillMinutes();
                initializeOutcomeDataTableSession();
                //lbAddViewTrainers.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");
            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            UtilsDataHandler utilsDataHandler = new UtilsDataHandler();
            try
            {
                string programeCode = txtProgrameCode.Text.ToString();
                string programeName = txtProgrameName.Text.ToString();
                string trainigType = ddlTrainingType.SelectedValue.ToString();
                string trainigCategory = ddlTrainingCategory.SelectedValue.ToString();
                string trainingSubcategory = ddlTrainingSubcategory.SelectedValue.ToString();
                string hours = txtDurationHrs.Text.ToString();
                string minuts = ddlDurationMins.SelectedItem.Text.ToString();
                string programType = ddlProgrameType.SelectedValue.ToString();
                int minBatch = 0;
                if (!String.IsNullOrEmpty(txtMinBatch.Text.ToString()))
                {
                    minBatch = Convert.ToInt16(txtMinBatch.Text.ToString());
                }
                int maxBatch = Convert.ToInt16(txtMaxBatch.Text.ToString());
                string description = txtDescription.Text.ToString();
                string objectives = txtObjectives.Text.ToString();

                string trainigStatus = ddlStatus.SelectedValue.ToString();
                string addedUserId = Session["KeyUSER_ID"].ToString();

                DataTable dtOutcomes = (Session["dtOutCome"] as DataTable).Copy();
                DataRow[] activeOutcomes = dtOutcomes.Select("outcomeStatus ='" + Constants.STATUS_ACTIVE_VALUE + "' ");

                if (activeOutcomes.Count() > 0)
                {
                    if (btnSave.Text.ToString() == Constants.CON_SAVE_BUTTON_TEXT)
                    {
                        Boolean nameIsExsists = utilsDataHandler.isDuplicateExist(programeName, "PROGRAM_NAME", "TRAINING_PROGRAM");
                        if (nameIsExsists)
                        {
                            CommonVariables.MESSAGE_TEXT = "Program name already exist";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }
                        else
                        {

                            if (String.IsNullOrEmpty(hours) && minuts == "00")
                            {
                                CommonVariables.MESSAGE_TEXT = "Duration is required";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            }
                            else
                            {
                                decimal duration = convertDurationToHours(hours, minuts);

                                bool isInserted = trainingProgramDataHandler.Insert(programeCode, programeName, trainigType, trainigCategory, trainingSubcategory, duration, programType, minBatch, maxBatch, description, objectives, trainigStatus, addedUserId, dtOutcomes);
                                if (isInserted)
                                {
                                    CommonVariables.MESSAGE_TEXT = "Record(s) successfully saved.";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                    fillTrainingProgrameGridView();
                                    clearControls_subForm();
                                    clearControls_main();
                                }
                                else
                                {
                                    CommonVariables.MESSAGE_TEXT = "Record(s) couldn't save.";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                }

                            }
                        }

                    }
                    else if (btnSave.Text.ToString() == Constants.CON_UPDATE_BUTTON_TEXT)
                    {
                        string programeId = hfProgrameId.Value.ToString();
                        Boolean nameIsExsists = utilsDataHandler.isDuplicateExist(programeName, "PROGRAM_NAME", "TRAINING_PROGRAM", programeId, "PROGRAM_ID");
                        if (nameIsExsists)
                        {
                            CommonVariables.MESSAGE_TEXT = "Institute name already exist";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(hours) && minuts == "00")
                            {
                                CommonVariables.MESSAGE_TEXT = "Duration is required";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            }
                            else
                            {
                                decimal duration = convertDurationToHours(hours, minuts);

                                bool isUpdated = trainingProgramDataHandler.Update(programeId, programeCode, programeName, trainigType, trainigCategory, trainingSubcategory, duration, programType, minBatch, maxBatch, description, objectives, trainigStatus, addedUserId, dtOutcomes);
                                if (isUpdated)
                                {
                                    CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated.";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                    fillTrainingProgrameGridView();
                                    clearControls_subForm();
                                    clearControls_main();


                                }
                                else
                                {
                                    CommonVariables.MESSAGE_TEXT = "Record(s) couldn't save.";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                }

                            }
                        }
                    }

                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Atleast one active outcome is required";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                }

            }
            catch (Exception ex)
            {

                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                trainingProgramDataHandler = null;
            }
        }

        protected void gvTrainingProgrames_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvTrainingProgrames_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvTrainingProgrames, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void gvTrainingProgrames_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gvTrainingProgrames_SelectedIndexChanged()");

            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            DataTable dtProgramDetails = new DataTable();
            DataTable dtOutcomes = new DataTable();
            try
            {
                Utility.Errorhandler.ClearError(lblErrorMsg);
                Utility.Errorhandler.ClearError(lblErrorMsg2);
                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

                int index = gvTrainingProgrames.SelectedIndex;
                string selectedProgrameId = gvTrainingProgrames.Rows[index].Cells[0].Text.ToString();

                hfProgrameId.Value = selectedProgrameId;

                dtProgramDetails = trainingProgramDataHandler.getProgrameById(selectedProgrameId);
                dtOutcomes = trainingProgramDataHandler.getOutcomesByProgrameId(selectedProgrameId);

                fillProgramDetails(dtProgramDetails);
                fillProgrameOutComesSessionTable(dtOutcomes);
                loadOutComesGridView();
                clearControls_subForm();

                lbAddViewTrainers.Visible = true;

            }
            catch (Exception ex)
            {

                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                trainingProgramDataHandler = null;
                dtProgramDetails.Dispose();
                dtOutcomes.Dispose();
            }
        }

        protected void gvTrainingProgrames_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvTrainingProgrames_PageIndexChanging()");
            try
            {
                gvTrainingProgrames.PageIndex = e.NewPageIndex;

                string selectedType = ddlTrainingTypeSearch.SelectedValue.ToString();
                string selectedCategory = ddlTrainingCategorySearch.SelectedValue.ToString();
                string selectedSubcategory = ddlTrainingSubcategorySearch.SelectedValue.ToString();

                if (String.IsNullOrEmpty(selectedCategory) && String.IsNullOrEmpty(selectedType) && String.IsNullOrEmpty(selectedSubcategory))
                {
                    fillTrainingProgrameGridView();
                }
                else
                {
                    filterProgrames();
                }

            }
            catch (Exception ex)
            {

                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void btnAddOutcome_Click(object sender, EventArgs e)
        {
            log.Debug("btnAddOutcome_Click()");
            DataTable dtOutcomes = new DataTable();

            try
            {
                Utility.Errorhandler.ClearError(lblErrorMsg2);

                string outcome = txtOutCome.Text.ToString();
                string outcomeStatus = ddlOutcomeStatus.SelectedValue.ToString();
                string outcomeStatusText = ddlOutcomeStatus.SelectedItem.Text.ToString();

                if (Session["dtOutCome"] != null)
                {
                    if ((Session["dtOutCome"] as DataTable).Copy() != null)
                    {
                        dtOutcomes = (Session["dtOutCome"] as DataTable).Copy();
                    }
                    else
                    {
                        initializeOutcomeDataTableSession();
                        dtOutcomes = (Session["dtOutCome"] as DataTable).Copy();
                    }
                }
                else
                {
                    initializeOutcomeDataTableSession();
                    dtOutcomes = (Session["dtOutCome"] as DataTable).Copy();
                }




                if (btnAddOutcome.Text.ToString() == "Add")
                {
                    bool isExist = false;
                    foreach (DataRow outcomeRow in dtOutcomes.Rows)
                    {
                        if (outcomeRow["outcome"].ToString().Replace(" ", "").ToLower() == outcome.Replace(" ", "").ToLower())
                        {
                            isExist = true;
                            break;
                        }
                    }

                    //DataRow [] existingRowsWithSameName = dtOutcomes.Select("outcome ='" + outcome + "' ");
                    if (isExist == true)
                    {
                        CommonVariables.MESSAGE_TEXT = "Outcome already exist.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                    }
                    else
                    {
                        DataRow newRow = dtOutcomes.NewRow();
                        newRow["outcome"] = outcome;
                        newRow["outcomeStatus"] = outcomeStatus;
                        newRow["outcomeStatusText"] = outcomeStatusText;
                        newRow["tempId"] = dtOutcomes.Rows.Count;
                        dtOutcomes.Rows.Add(newRow);
                        Session["dtOutCome"] = dtOutcomes;
                    }
                }
                else if (btnAddOutcome.Text.ToString() == "Update")
                {
                    string outcomeTempId = hfOutcomeTempId.Value.ToString();

                    bool isExist = false;
                    foreach (DataRow outcomeRow in dtOutcomes.Rows)
                    {
                        if (outcomeRow["outcome"].ToString().Replace(" ", "").ToLower() == outcome.Replace(" ", "").ToLower() && outcomeRow["tempId"].ToString() != outcomeTempId)
                        {
                            isExist = true;
                            break;
                        }
                    }

                    //DataRow [] existingRowsWithSameName = dtOutcomes.Select("outcome ='" + outcome + "' ");
                    if (isExist == true)
                    {
                        CommonVariables.MESSAGE_TEXT = "Outcome already exist.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                    }
                    else
                    {
                        DataRow selectedRow = dtOutcomes.Select("tempId =" + outcomeTempId).First();

                        selectedRow["outcome"] = outcome;
                        selectedRow["outcomeStatus"] = outcomeStatus;
                        selectedRow["outcomeStatusText"] = outcomeStatusText;
                        Session["dtOutCome"] = dtOutcomes;
                    }
                }
                loadOutComesGridView();
                clearControls_subForm();
                hfOutcomeTempId.Value = "";

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                dtOutcomes.Dispose();
            }
        }

        protected void btnClearOutcome_Click(object sender, EventArgs e)
        {
            log.Debug("btnClearOutcome_Click()");

            try
            {
                Utility.Utils.clearControls(true, txtOutCome, ddlOutcomeStatus);
                Utility.Errorhandler.ClearError(lblErrorMsg2);
                btnAddOutcome.Text = "Add";
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");

            try
            {
                clearControls_subForm();
                clearControls_main();
                Utility.Errorhandler.ClearError(lblErrorMsg);
                gvOutcomes.DataSource = null;
                gvOutcomes.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void lbAddViewTrainers_Click(object sender, EventArgs e)
        {
            log.Debug("lbAddViewTrainers_Click()");

            string selectedPrograme = hfProgrameId.Value.ToString();
            if (!String.IsNullOrEmpty(selectedPrograme))
            {
                Server.Transfer("~/TrainingAndDevelopment/WebFrmTrainerTrainingPrograme.aspx");
            }
            else
            {
                CommonVariables.MESSAGE_TEXT = "Select program before add trainers ";
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }

        }

        protected void gvOutcomes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvOutcomes_PageIndexChanging()");

            DataTable dtOutcomes = new DataTable();
            try
            {
                gvOutcomes.PageIndex = e.NewPageIndex;
                dtOutcomes = (Session["dtOutCome"] as DataTable).Copy();
                fillProgrameOutComesSessionTable(dtOutcomes);
                loadOutComesGridView();

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                dtOutcomes.Dispose();
            }
        }

        protected void gvOutcomes_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gvOutcomes_SelectedIndexChanged()");

            DataTable dtOutcomes = new DataTable();
            try
            {
                Utility.Errorhandler.ClearError(lblErrorMsg2);
                btnAddOutcome.Text = "Update";

                int index = gvOutcomes.SelectedIndex;
                int outcomeTempId = Convert.ToInt16(gvOutcomes.Rows[index].Cells[4].Text.ToString());
                hfOutcomeTempId.Value = outcomeTempId.ToString();

                dtOutcomes = (Session["dtOutCome"] as DataTable).Copy();
                DataRow selectedRow = dtOutcomes.Select("tempId =" + outcomeTempId).First();

                txtOutCome.Text = selectedRow["outcome"].ToString();
                ddlOutcomeStatus.SelectedValue = selectedRow["outcomeStatus"].ToString();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                dtOutcomes.Dispose();
            }
        }

        protected void gvOutcomes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvOutcomes_RowDataBound()");

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvOutcomes, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
        }

        protected void ddlTrainingTypeSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlTrainingTypeSearch_SelectedIndexChanged()");
            try
            {
                //filterProgrames();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void ddlTrainingCategorySearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlTrainingCategorySearch_SelectedIndexChanged()");
            try
            {
                //filterProgrames();
                string categoryId = ddlTrainingCategorySearch.SelectedValue.ToString();
                fillTrainingSubCategorySearchDropdown(categoryId);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            log.Debug("iBtnSearch_Click()");
            try
            {
                gvTrainingProgrames.PageIndex = 0;
                filterProgrames();
            }
            catch (Exception)
            {

                throw;
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

        #endregion
        
        #region methodes

        private void filterProgrames()
        {

            DataTable dtFilteredResult = new DataTable();
            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();

            try
            {
                string selectedType = ddlTrainingTypeSearch.SelectedValue.ToString();
                string selectedCategory = ddlTrainingCategorySearch.SelectedValue.ToString();
                string selectedSubcategory = ddlTrainingSubcategorySearch.SelectedValue.ToString();

                dtFilteredResult = trainingProgramDataHandler.filterProgrames(selectedType, selectedCategory, selectedSubcategory);
                if (dtFilteredResult != null)
                {
                    if (dtFilteredResult.Rows.Count > 0)
                    {
                        gvTrainingProgrames.DataSource = dtFilteredResult;
                        gvTrainingProgrames.DataBind();
                    }
                    else
                    {
                        gvTrainingProgrames.DataSource = null;
                        gvTrainingProgrames.DataBind();
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
                trainingProgramDataHandler = null;
                dtFilteredResult.Dispose();
            }
        }

        private void fillStatus()
        {
            ListItem listItemBlank = new ListItem();
            listItemBlank.Text = "";
            listItemBlank.Value = "";
            ddlStatus.Items.Add(listItemBlank);
            ddlOutcomeStatus.Items.Add(listItemBlank);

            ListItem listItemActive = new ListItem();
            listItemActive.Text = Constants.STATUS_ACTIVE_TAG;
            listItemActive.Value = Constants.STATUS_ACTIVE_VALUE;
            ddlStatus.Items.Add(listItemActive);
            ddlOutcomeStatus.Items.Add(listItemActive);

            ListItem listItemInActive = new ListItem();
            listItemInActive.Text = Constants.STATUS_INACTIVE_TAG;
            listItemInActive.Value = Constants.STATUS_INACTIVE_VALUE;
            ddlStatus.Items.Add(listItemInActive);
            ddlOutcomeStatus.Items.Add(listItemInActive);
        }

        private void fillMinutes()
        {
            try
            {
                //ListItem listItemBlank = new ListItem();
                //listItemBlank.Text = "";
                //listItemBlank.Value = "";
                //ddlDurationMins.Items.Add(listItemBlank);

                for (int i = 0; i < 60; i++)
                {
                    ListItem listItemActive = new ListItem();
                    listItemActive.Text = i.ToString("00");
                    listItemActive.Value = i.ToString();
                    ddlDurationMins.Items.Add(listItemActive);
                }
                ddlDurationMins.SelectedValue = "0";
            }
            catch (Exception ex)
            {

                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void fillTrainingType()
        {
            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            DataTable dtTrainingTypes = new DataTable();
            try
            {
                dtTrainingTypes = trainingProgramDataHandler.getTrainingTypes();

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlTrainingType.Items.Add(listItemBlank);
                ddlTrainingTypeSearch.Items.Add(listItemBlank);

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
                            ddlTrainingTypeSearch.Items.Add(newItem);

                        }
                    }
                }

            }
            catch (Exception ex)
            {

                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally {
                trainingProgramDataHandler = null;
                dtTrainingTypes.Dispose();
            }
        }

        private void fillTrainingCategory()
        {
            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            DataTable dtTrainingCategories = new DataTable();
            try
            {
                dtTrainingCategories = trainingProgramDataHandler.getTrainingCategories();

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlTrainingCategory.Items.Add(listItemBlank);
                ddlTrainingCategorySearch.Items.Add(listItemBlank);

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
                            ddlTrainingCategorySearch.Items.Add(newItem);

                        }
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
                trainingProgramDataHandler = null;
                dtTrainingCategories.Dispose();
            }
        }

        private void fillSubcategoryDropdown(string categoryId)
        {
            log.Debug("fillSubcategoryDropdown()");
            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            DataTable dtSubcategory = new DataTable();
            try
            {
                ddlTrainingSubcategory.Items.Clear();
                dtSubcategory = trainingProgramDataHandler.getSubcategoriesForCategory(categoryId);

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlTrainingSubcategory.Items.Add(listItemBlank);

                if (dtSubcategory != null)
                {
                    if (dtSubcategory.Rows.Count > 0)
                    {
                        foreach (DataRow type in dtSubcategory.Rows)
                        {
                            ListItem newItem = new ListItem();
                            newItem.Text = type[1].ToString();
                            newItem.Value = type[0].ToString();
                            ddlTrainingSubcategory.Items.Add(newItem);

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
                trainingProgramDataHandler = null;
                dtSubcategory.Dispose();
            }
        }

        private void fillProgrameType()
        {
            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlProgrameType.Items.Add(listItemBlank);

                ListItem itemLongTerm = new ListItem();
                itemLongTerm.Text = Constants.PROGRAME_TYPE_LONG_TERM_TAG;
                itemLongTerm.Value = Constants.PROGRAME_TYPE_LONG_TERM_VALUE;
                ddlProgrameType.Items.Add(itemLongTerm);

                ListItem itemShortTerm = new ListItem();
                itemShortTerm.Text = Constants.PROGRAME_TYPE_SHORT_TERM_TAG;
                itemShortTerm.Value = Constants.PROGRAME_TYPE_SHORT_TERM_VALUE;
                ddlProgrameType.Items.Add(itemShortTerm);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void fillTrainingProgrameGridView()
        {
            try
            {
                DataTable dtTrainingProgrames = getAllTrainingProgrames();
                if (dtTrainingProgrames != null)
                {
                    if (dtTrainingProgrames.Rows.Count > 0)
                    {
                        gvTrainingProgrames.DataSource = dtTrainingProgrames;
                        gvTrainingProgrames.DataBind();
                    }
                }
                else
                {
                    gvTrainingProgrames.DataSource = null;
                    gvTrainingProgrames.DataBind();
                }
                dtTrainingProgrames.Dispose();
            }
            catch (Exception ex)
            {

                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private DataTable getAllTrainingProgrames()
        {
            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            DataTable dtProgrames = new DataTable();
            try
            {
                dtProgrames = trainingProgramDataHandler.getAllTrainingProgrames();
                return dtProgrames;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                return null;
            }
            finally
            {
                trainingProgramDataHandler = null;
                dtProgrames.Dispose();
            }
        }

        private void fillProgramDetails(DataTable dtProgramDetails)
        {
            try
            {
                hfProgrameId.Value = dtProgramDetails.Rows[0]["PROGRAM_ID"].ToString();
                txtProgrameCode.Text = dtProgramDetails.Rows[0]["PROGRAM_CODE"].ToString();
                txtProgrameName.Text = dtProgramDetails.Rows[0]["PROGRAM_NAME"].ToString();


                ddlProgrameType.SelectedValue = dtProgramDetails.Rows[0]["PROGRAM_TYPE"].ToString();
                txtMinBatch.Text = dtProgramDetails.Rows[0]["MINIMUM_BATCH_SIZE"].ToString();
                txtMaxBatch.Text = dtProgramDetails.Rows[0]["MAXIMUM_BATCH_SIZE"].ToString();
                txtDescription.Text = dtProgramDetails.Rows[0]["DESCRIPTION"].ToString();
                txtObjectives.Text = dtProgramDetails.Rows[0]["OBJECTIVES"].ToString();
                ddlTrainingType.SelectedValue = dtProgramDetails.Rows[0]["TRAINING_TYPE_ID"].ToString();
                ddlTrainingCategory.SelectedValue = dtProgramDetails.Rows[0]["TRAINING_CATEGORY_ID"].ToString();

                fillSubcategoryDropdown(ddlTrainingCategory.SelectedValue.ToString());
                ddlTrainingSubcategory.SelectedValue = dtProgramDetails.Rows[0]["TRAINING_SUBCATEGORY"].ToString();
                ddlStatus.SelectedValue = dtProgramDetails.Rows[0]["STATUS_CODE"].ToString();

                //ddlTrainingCategory.SelectedValue = dtProgramDetails.Rows[0]["TRAINING_CATEGORY_ID"].ToString();

                string durationFromDb = dtProgramDetails.Rows[0]["PROGRAM_DURATION"].ToString();
                if (durationFromDb.Contains("."))
                {
                    string[] result = durationFromDb.Split('.');
                    string hours = result[0];
                    string minuts = Math.Round(Convert.ToDecimal("0." + result[1]) * 60, 0, MidpointRounding.AwayFromZero).ToString();

                    txtDurationHrs.Text = hours;
                    ddlDurationMins.SelectedValue = minuts;
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private decimal convertDurationToHours(string hours, string minuts)
        {
            decimal totalMinuts = decimal.Parse(hours) * 60 + decimal.Parse(minuts);
            decimal totalHours = decimal.Round(totalMinuts / 60, 2);
            return totalHours;
        }

        /// <summary>
        /// clear controls in main form without sub form
        /// </summary>
        private void clearControls_main()
        {
            Utility.Utils.clearControls(true, txtDescription, txtObjectives, txtDurationHrs, txtMaxBatch, txtMinBatch, txtProgrameCode, txtProgrameName, ddlDurationMins, ddlProgrameType, ddlTrainingType, ddlTrainingCategory, ddlTrainingSubcategory, ddlStatus);
            hfProgrameId.Value = "";
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;

            Session["dtOutCome"] = null;
            gvOutcomes.DataSource = null;
            gvOutcomes.DataBind();

            lbAddViewTrainers.Visible = false;
        }
        /// <summary>
        /// clear controls in training outcomes sub form
        /// </summary>
        private void clearControls_subForm()
        {
            Utility.Utils.clearControls(true, ddlOutcomeStatus, txtOutCome);
            Utility.Errorhandler.ClearError(lblErrorMsg2);
            hfOutcomeTempId.Value = "";
            btnAddOutcome.Text = "Add";
        }

        private void initializeOutcomeDataTableSession()
        {
            try
            {
                if (Session["dtOutCome"] != null)
                {
                    Session.Remove("dtOutCome");
                }
                DataTable dtOutcomes = new DataTable();
                dtOutcomes.Columns.Add("outcomeId");
                dtOutcomes.Columns.Add("outcome");
                dtOutcomes.Columns.Add("outcomeStatus");
                dtOutcomes.Columns.Add("outcomeStatusText");
                dtOutcomes.Columns.Add("tempId", typeof(int));

                Session["dtOutCome"] = dtOutcomes;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void loadOutComesGridView()
        {
            DataTable dtOutcomes = new DataTable();
            //TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            try
            {
                dtOutcomes = (Session["dtOutCome"] as DataTable).Copy();
                if (dtOutcomes.Rows.Count > 0)
                {
                    gvOutcomes.DataSource = dtOutcomes;
                    gvOutcomes.DataBind();
                }
                else
                {
                    gvOutcomes.DataSource = null;
                    gvOutcomes.DataBind();
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                dtOutcomes.Dispose();
            }
        }

        private void fillProgrameOutComesSessionTable(DataTable dtOutcomes)
        {
            DataTable dtExistingOutcomes = new DataTable();
            try
            {
                initializeOutcomeDataTableSession();
                dtExistingOutcomes = (Session["dtOutCome"] as DataTable).Copy();
                int i = 0;
                foreach (DataRow outcome in dtOutcomes.Rows)
                {
                    DataRow newRow = dtExistingOutcomes.NewRow();
                    newRow["outcome"] = outcome["outcome"].ToString();
                    newRow["outcomeStatus"] = outcome["outcomeStatus"].ToString();
                    newRow["outcomeId"] = outcome["outcomeId"].ToString();
                    newRow["tempId"] = i;

                    if (outcome["outcomeStatus"].ToString() == Constants.STATUS_ACTIVE_VALUE)
                    {
                        newRow["outcomeStatusText"] = Constants.STATUS_ACTIVE_TAG;
                    }
                    else if (outcome["outcomeStatus"].ToString() == Constants.STATUS_INACTIVE_VALUE)
                    {
                        newRow["outcomeStatusText"] = Constants.STATUS_INACTIVE_TAG;
                    }
                    dtExistingOutcomes.Rows.Add(newRow);
                    i++;
                }
                Session["dtOutCome"] = dtExistingOutcomes;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void fillTrainingSubCategorySearchDropdown(string categoryId)
        {
            log.Debug("fillSubcategorySearchDropdown()");
            TrainingProgramDataHandler trainingProgramDataHandler = new TrainingProgramDataHandler();
            DataTable dtSubcategory = new DataTable();
            try
            {
                ddlTrainingSubcategorySearch.Items.Clear();
                dtSubcategory = trainingProgramDataHandler.getSubcategoriesForCategory(categoryId);

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
                trainingProgramDataHandler = null;
                dtSubcategory.Dispose();
            }
        }

        #endregion




        

        

        




        











    }

}