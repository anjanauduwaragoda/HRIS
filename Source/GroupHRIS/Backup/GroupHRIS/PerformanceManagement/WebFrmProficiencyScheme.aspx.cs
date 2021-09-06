using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.PerformanceManagement;
using DataHandler.Utility;
using Common;
using GroupHRIS.Utility;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmProficiencySchemeC : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmProficiencySchemeC : Page_Load"); 
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
                loadProficiancySchemeGrid();
                fillStatus();
                initializeProficiencyLevelDataGrid();
            }


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            ProficiencySchemeDataHandler proficiencySchemeDataHandler = new ProficiencySchemeDataHandler();
            UtilsDataHandler utilsDataHandler = new UtilsDataHandler();
            try
            {
                Utility.Errorhandler.ClearError(lblErrorMsg);

                string schemeName = txtName.Text.ToString().Trim();
                string remarks = txtDescription.Text.ToString().Trim();
                string status = ddlStatus.SelectedItem.Value.ToString();
                string addedUserId = Session["KeyUSER_ID"].ToString();
                DataTable proficiencyLevelDataTable = new DataTable();
                //if ()
                //{
                //    proficiencyLevelDataTable = (DataTable)Session["ProficiencyLevelDataGrid"];
                //}

                if (Session["ProficiencyLevelDataGrid"] != null )
                {
                    proficiencyLevelDataTable = (DataTable)Session["ProficiencyLevelDataGrid"];
                    if (proficiencyLevelDataTable.Rows.Count > 0)
                    {
                        bool allInactiveLevels = true;
                        foreach (DataRow level in proficiencyLevelDataTable.Rows)
                        {

                            if (level[3].ToString() == "Active")
                            {
                                allInactiveLevels = false;
                            }
                        }
                        if (allInactiveLevels == false)
                        {

                            if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                            {
                                Boolean nameIsExsists = utilsDataHandler.isDuplicateExist(schemeName, "SCHEME_NAME", "PROFICIENCY_SCHEME");
                                if (nameIsExsists)
                                {
                                    CommonVariables.MESSAGE_TEXT = "Proficiency scheme name already exist";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                }
                                else
                                {
                                    Boolean insert = proficiencySchemeDataHandler.Insert(schemeName, remarks, status, addedUserId, proficiencyLevelDataTable);
                                    if (insert)
                                    {
                                        CommonVariables.MESSAGE_TEXT = "Record(s) successfully saved.";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);

                                    }
                                    clearFrom();
                                    loadProficiancySchemeGrid();
                                }
                            }
                            else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                            {
                                string schemeId = proficiancySchemeGrid.SelectedRow.Cells[0].Text;
                                Boolean isUsedInEmployeeEvaluation = false;

                                string proficiencyScemeId = hf_selected_proficiencyScheme_id.Value.ToString();


                                isUsedInEmployeeEvaluation = proficiencySchemeDataHandler.CheckUsageOfProficiencyLevel(proficiencyScemeId);
                                //bool isUsed = 
                                if (status == Constants.STATUS_ACTIVE_VALUE)
                                {
                                    Boolean nameIsExsists = utilsDataHandler.isDuplicateExist(schemeName, "SCHEME_NAME", "PROFICIENCY_SCHEME", schemeId, "SCHEME_ID");
                                    if (nameIsExsists)
                                    {
                                        CommonVariables.MESSAGE_TEXT = "Proficiency scheme name already exist";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                    }
                                    else
                                    {
                                        Boolean updated = proficiencySchemeDataHandler.Update(schemeId, schemeName, remarks, status, addedUserId, proficiencyLevelDataTable);
                                        if (updated)
                                        {
                                            CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated.";
                                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                        }
                                        clearFrom();
                                        loadProficiancySchemeGrid();
                                    }
                                }
                                else
                                {
                                    isUsedInEmployeeEvaluation = proficiencySchemeDataHandler.CheckUsageOfProficiencyLevel(proficiencyScemeId);
                                    if (!isUsedInEmployeeEvaluation)
                                    {
                                        Boolean nameIsExsists = utilsDataHandler.isDuplicateExist(schemeName, "SCHEME_NAME", "PROFICIENCY_SCHEME", schemeId, "SCHEME_ID");
                                        if (nameIsExsists)
                                        {
                                            CommonVariables.MESSAGE_TEXT = "Proficiency scheme name already exist";
                                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                        }
                                        else
                                        {
                                            Boolean updated = proficiencySchemeDataHandler.Update(schemeId, schemeName, remarks, status, addedUserId, proficiencyLevelDataTable);
                                            if (updated)
                                            {
                                                CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated.";
                                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                            }
                                            clearFrom();
                                            loadProficiancySchemeGrid();
                                        }
                                    }
                                    else
                                    {
                                        CommonVariables.MESSAGE_TEXT = "Proficiency scheme is already used";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                    }
                                }
                            }
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Atleast one active rating is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "Ratings are required";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Ratings Levels are required";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                }
            }
            catch (Exception ex)
            { 
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                proficiencySchemeDataHandler = null;
                utilsDataHandler = null;
            }

            
        }

        protected void loadProficiancySchemeGrid()
        {
            log.Debug("loadProficiancySchemeGrid()");

            ProficiencySchemeDataHandler proficiencySchemeDataHandler = new ProficiencySchemeDataHandler();
            DataTable dataTable = new DataTable();

            try
            {
                dataTable = proficiencySchemeDataHandler.Populate();
                proficiancySchemeGrid.DataSource = dataTable;
                proficiancySchemeGrid.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                proficiencySchemeDataHandler = null;
                dataTable.Dispose();
            }
        }

        protected void ProficiancySchemeGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            log.Debug("ProficiancySchemeGrid_PageIndexChanging()");
            try
            {
                proficiancySchemeGrid.PageIndex = e.NewPageIndex;
                loadProficiancySchemeGrid();
                clearFrom();
                Utility.Errorhandler.ClearError(lblErrorMsg);
                Utility.Errorhandler.ClearError(lblError);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void ProficiancySchemeGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("ProficiancySchemeGrid_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.proficiancySchemeGrid, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void ProficiancySchemeGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ProficiancySchemeGrid_SelectedIndexChanged()");
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblErrorMsg);
            Utility.Errorhandler.ClearError(lblError);
            clearSchemeLevels();
            ProficiencySchemeDataHandler proficiencySchemeDataHandler = new ProficiencySchemeDataHandler();
            DataRow schemeDataRow = null;
            DataTable proficiencyLevels = new DataTable();
            try
            {
                string proficiencySchemeId = proficiancySchemeGrid.SelectedRow.Cells[0].Text;
                hf_selected_proficiencyScheme_id.Value = proficiencySchemeId;
                schemeDataRow = proficiencySchemeDataHandler.PopulateSchemes(proficiencySchemeId);


                if (schemeDataRow != null)
                {
                    txtName.Text = schemeDataRow["SCHEME_NAME"].ToString().Trim();
                    txtDescription.Text = schemeDataRow["REMARKS"].ToString().Trim();
                    string status = schemeDataRow["STATUS_CODE"].ToString().Trim();
                    ddlStatus.SelectedValue = status;

                }


                proficiencyLevels = proficiencySchemeDataHandler.PopulateProficiencyLevels(proficiencySchemeId);
                Session["ProficiencyLevelDataGrid"] = proficiencyLevels;
                if (proficiencyLevels != null)
                {
                    ProficiencyLevelGridView.Width = 500;
                    ProficiencyLevelGridView.DataSource = (DataTable)Session["ProficiencyLevelDataGrid"];
                    ProficiencyLevelGridView.DataBind();
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                proficiencySchemeDataHandler = null;
                schemeDataRow = null;
            }

        }

        private void clearFrom()
        {
            log.Debug("clearFrom()");
            try
            {
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                Utils.clearControls(true, txtName, txtDescription, ddlStatus, txtRating, txtWeight, txtRemarks, ddlLevelStatus, txtLevelDescription);
                txtRating.Enabled = true;
                btnProficiencyLevelSave.Text = "Add";
                Session["ProficiencyLevelDataGrid"] = null;
                ProficiencyLevelGridView.DataSource = null;
                ProficiencyLevelGridView.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        } //// clear the main form

        private void fillStatus()
        {
            log.Debug("fillStatus()");
            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlStatus.Items.Add(listItemBlank);
                ddlLevelStatus.Items.Add(listItemBlank);

                ListItem listItemActive = new ListItem();
                listItemActive.Text = Constants.STATUS_ACTIVE_TAG;
                listItemActive.Value = Constants.STATUS_ACTIVE_VALUE;
                ddlStatus.Items.Add(listItemActive);
                ddlLevelStatus.Items.Add(listItemActive);

                ListItem listItemInActive = new ListItem();
                listItemInActive.Text = Constants.STATUS_INACTIVE_TAG;
                listItemInActive.Value = Constants.STATUS_INACTIVE_VALUE;
                ddlStatus.Items.Add(listItemInActive);
                ddlLevelStatus.Items.Add(listItemInActive);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");
            try
            {
                clearFrom();
                Utility.Errorhandler.ClearError(lblErrorMsg);
                Utility.Errorhandler.ClearError(lblError);
                Session["ProficiencyLevelDataGrid"] = null;
                initializeProficiencyLevelDataGrid();
                hf_selected_proficiencyScheme_id.Value = "";
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void initializeProficiencyLevelDataGrid()
        {
            log.Debug("initializeProficiencyLevelDataGrid()");

            DataTable dt = new DataTable();
            dt.Columns.Add("SCHEME_ID");
            dt.Columns.Add("RATING");
            dt.Columns.Add("WEIGHT");
            dt.Columns.Add("STATUS_CODE");           
            dt.Columns.Add("REMARKS");
            dt.Columns.Add("DESCRIPTION");
            dt.Columns.Add("ADDED_BY");
            dt.Columns.Add("ADDED_DATE");
            dt.Columns.Add("MODIFIED_BY");
            dt.Columns.Add("MODIFIED_DATE");

            Session["ProficiencyLevelDataGrid"] = dt;
        }

        /// Manipulating Proficiency Level form begin /////

        protected void proficiencyLevel_btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("proficiencyLevel_btnSave_Click()");

            Utility.Errorhandler.ClearError(lblError);
            string rating = txtRating.Text.ToString().Trim();
            string weight = txtWeight.Text.ToString().Trim();
            string status = ddlLevelStatus.SelectedValue.ToString().Trim();
            string remarks = txtRemarks.Text.ToString().Trim();
            string levelDescription = txtLevelDescription.Text.ToString().Trim();

            ProficiencySchemeDataHandler proficiencySchemeDataHandler = new ProficiencySchemeDataHandler();
            try
            {
                if (btnProficiencyLevelSave.Text == "Add")
                {
                    string proficiencyScemeId = hf_selected_proficiencyScheme_id.Value.ToString();

                    Boolean isUsedInEmployeeEvaluation = false;

                    
                    isUsedInEmployeeEvaluation = proficiencySchemeDataHandler.CheckUsageOfProficiencyLevel(proficiencyScemeId);


                    if (!isUsedInEmployeeEvaluation)
                    {
                        DataTable proficiencyLevelDataTable = new DataTable();
                        proficiencyLevelDataTable = (DataTable)Session["ProficiencyLevelDataGrid"];

                        DataRow[] result = proficiencyLevelDataTable.Select("RATING = '" + rating + "' OR WEIGHT = '" + weight + "'");
                        if (result.Length == 0)
                        {
                            DataRow proficiencyLevelDataRow = proficiencyLevelDataTable.NewRow();
                            proficiencyLevelDataRow["RATING"] = rating;
                            proficiencyLevelDataRow["WEIGHT"] = weight;
                            if (status == Constants.STATUS_ACTIVE_VALUE)
                            {
                                proficiencyLevelDataRow["STATUS_CODE"] = Constants.STATUS_ACTIVE_TAG;
                            }
                            else if (status == Constants.STATUS_INACTIVE_VALUE)
                            {
                                proficiencyLevelDataRow["STATUS_CODE"] = Constants.STATUS_INACTIVE_TAG;
                            }
                            proficiencyLevelDataRow["REMARKS"] = remarks;
                            proficiencyLevelDataRow["DESCRIPTION"] = levelDescription;

                            proficiencyLevelDataTable.Rows.Add(proficiencyLevelDataRow);

                            ProficiencyLevelGridView.Width = 500;
                            ProficiencyLevelGridView.DataSource = proficiencyLevelDataTable;
                            ProficiencyLevelGridView.DataBind();
                            Session["ProficiencyLevelDataGrid"] = proficiencyLevelDataTable;

                            //CommonVariables.MESSAGE_TEXT = "Record(s) saved successfully.";
                            //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblError);
                            clearSchemeLevels();

                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Rating : ' " + rating + " ' or Weight :'" + weight + "' already exist";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblError);
                            clearSchemeLevels();
                        }
                    }
                    else
                    {

                        CommonVariables.MESSAGE_TEXT = "Proficiency Scheme is already used";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblError);
                    }

                }
                else if (btnProficiencyLevelSave.Text == "Update")
                {
                    string proficiencyScemeId = hf_selected_proficiencyScheme_id.Value.ToString();

                    Boolean isUsedInEmployeeEvaluation = false;

                    //ProficiencySchemeDataHandler proficiencySchemeDataHandler = new ProficiencySchemeDataHandler();
                    isUsedInEmployeeEvaluation = proficiencySchemeDataHandler.CheckUsageOfProficiencyLevel(proficiencyScemeId);
                    if (!isUsedInEmployeeEvaluation)
                    {

                        int selectedIndex = Convert.ToInt32(HiddenField_selected_index.Value);

                        DataTable proficiencyLevelDataTable = new DataTable();
                        proficiencyLevelDataTable = (DataTable)Session["ProficiencyLevelDataGrid"];

                        DataRow[] result = proficiencyLevelDataTable.Select("RATING = '" + rating + "' OR WEIGHT = '" + weight + "'");
                        if (result.Length == 1)
                        {

                            proficiencyLevelDataTable.Rows[selectedIndex]["RATING"] = rating;
                            proficiencyLevelDataTable.Rows[selectedIndex]["WEIGHT"] = weight;
                            proficiencyLevelDataTable.Rows[selectedIndex]["REMARKS"] = remarks;
                            proficiencyLevelDataTable.Rows[selectedIndex]["DESCRIPTION"] = levelDescription;

                            if (status == Constants.STATUS_ACTIVE_VALUE)
                            {
                                proficiencyLevelDataTable.Rows[selectedIndex]["STATUS_CODE"] = Constants.STATUS_ACTIVE_TAG;
                            }
                            else if (status == Constants.STATUS_INACTIVE_VALUE)
                            {
                                proficiencyLevelDataTable.Rows[selectedIndex]["STATUS_CODE"] = Constants.STATUS_INACTIVE_TAG;
                            }

                            ProficiencyLevelGridView.DataSource = proficiencyLevelDataTable;
                            ProficiencyLevelGridView.DataBind();
                            Session["ProficiencyLevelDataGrid"] = proficiencyLevelDataTable;

                            //CommonVariables.MESSAGE_TEXT = "Record(s) updated successfully.";
                            //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblError);
                            clearSchemeLevels();

                        }
                        else
                        {

                            CommonVariables.MESSAGE_TEXT = "Rating : ' " + rating + " ' or Weight :'" + weight + "' already exist";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblError);
                            clearSchemeLevels();

                        }
                    }
                    else
                    {

                        CommonVariables.MESSAGE_TEXT = "Proficiency Scheme is already used";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblError);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblError);
                throw;
            }
            finally
            {
                proficiencySchemeDataHandler = null;
            }
        }

        protected void ProficiencyLevelGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("ProficiencyLevelGrid_PageIndexChanging()");

            try
            {
                ProficiencyLevelGridView.PageIndex = e.NewPageIndex;
                clearFrom();
                Utility.Errorhandler.ClearError(lblErrorMsg);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void ProficiencyLevelGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("ProficiencyLevelGrid_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.ProficiencyLevelGridView, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void initializeProficiencyLevelDataTable()
        {
            log.Debug("initializeProficiencyLevelDataTable()");

            DataTable dt = new DataTable();
            dt.Columns.Add("SCHEME_ID");
            dt.Columns.Add("RATING");
            dt.Columns.Add("WEIGHT");
            dt.Columns.Add("STATUS_CODE");
            dt.Columns.Add("REMARKS");
            dt.Columns.Add("DESCRIPTION");
            dt.Columns.Add("ADDED_BY");
            dt.Columns.Add("ADDED_DATE");
            dt.Columns.Add("MODIFIED_BY");
            dt.Columns.Add("MODIFIED_DATE");

            Session["ProficiencyLevelDataGrid"] = dt;

            ProficiencyLevelGridView.DataSource = (DataTable)Session["ProficiencyLevelDataGrid"];
            ProficiencyLevelGridView.DataBind();
        }

        protected void ProficiencyScheme_btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("ProficiencyScheme_btnClear_Click()");
            try
            {
                clearSchemeLevels();
                Utility.Errorhandler.ClearError(lblError);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void clearSchemeLevels()
        {
            log.Debug("clearSchemeLevels()");
            try
            {
                btnProficiencyLevelSave.Text = "Add";
                Utils.clearControls(true, txtRating, txtRemarks, txtWeight, ddlLevelStatus, txtLevelDescription);
                txtRating.Enabled = true;
                //Session["ProficiencyLevelDataGrid"] = null;
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void ProficiencyLevelGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ProficiencyLevelGridView_SelectedIndexChanged()");
            try
            {
                int index = ProficiencyLevelGridView.SelectedRow.RowIndex;

                HiddenField_selected_index.Value = index.ToString();
                txtRating.Text = ProficiencyLevelGridView.Rows[index].Cells[0].Text.ToString();
                txtWeight.Text = ProficiencyLevelGridView.Rows[index].Cells[1].Text.ToString();
                txtRemarks.Text = ProficiencyLevelGridView.Rows[index].Cells[2].Text.ToString().Replace("&nbsp;", "");
                txtLevelDescription.Text = ProficiencyLevelGridView.Rows[index].Cells[3].Text.ToString().Replace("&nbsp;", "");


                if (ProficiencyLevelGridView.Rows[index].Cells[4].Text.ToString() == Constants.STATUS_ACTIVE_TAG)
                {
                    ddlLevelStatus.SelectedValue = Constants.STATUS_ACTIVE_VALUE;
                }
                else if (ProficiencyLevelGridView.Rows[index].Cells[4].Text.ToString() == Constants.STATUS_INACTIVE_TAG)
                {
                    ddlLevelStatus.SelectedValue = Constants.STATUS_INACTIVE_VALUE;
                }


                btnProficiencyLevelSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                txtRating.Enabled = false;
                Utility.Errorhandler.ClearError(lblError);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        /// Manipulating Proficiency Level form ends /////
    }
}