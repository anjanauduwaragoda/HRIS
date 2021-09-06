using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using DataHandler.Utility;
using Common;
using GroupHRIS.Utility;
using NLog;
using DataHandler.TrainingAndDevelopment;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmRatingScheme: System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmRatingScheme : Page_Load"); 
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
                loadRatingSchemeGrid();
                fillStatus();
                initializeRatingLevelDataGrid();
            }


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            RatingSchemeDataHandler ratingSchemeDataHandler = new RatingSchemeDataHandler();
            UtilsDataHandler utilsDataHandler = new UtilsDataHandler();
            try
            {
                Utility.Errorhandler.ClearError(lblErrorMsg);

                string schemeName = txtName.Text.ToString().Trim();
                string remarks = txtDescription.Text.ToString().Trim();
                string status = ddlStatus.SelectedItem.Value.ToString();
                string addedUserId = Session["KeyUSER_ID"].ToString();
                DataTable ratingLevelDataTable = new DataTable();
                //if ()
                //{
                //    proficiencyLevelDataTable = (DataTable)Session["RatingLevelDataGrid"];
                //}

                if (Session["RatingLevelDataGrid"] != null )
                {
                    ratingLevelDataTable = (DataTable)Session["RatingLevelDataGrid"];
                    if (ratingLevelDataTable.Rows.Count > 0)
                    {
                        bool allInactiveLevels = true;
                        foreach (DataRow level in ratingLevelDataTable.Rows)
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
                                Boolean nameIsExsists = utilsDataHandler.isDuplicateExist(schemeName, "RS_NAME", "RATING_SCHEME");
                                if (nameIsExsists)
                                {
                                    CommonVariables.MESSAGE_TEXT = "Rating scheme name already exist";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                }
                                else
                                {
                                    Boolean insert = ratingSchemeDataHandler.Insert(schemeName, remarks, status, addedUserId, ratingLevelDataTable);
                                    if (insert)
                                    {
                                        CommonVariables.MESSAGE_TEXT = "Record(s) successfully saved.";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);

                                    }
                                    clearFrom();
                                    loadRatingSchemeGrid();
                                }
                            }
                            else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                            {
                                string schemeId = ratingSchemeGrid.SelectedRow.Cells[0].Text;
                                Boolean isUsedInEmployeeEvaluation = false;

                                string proficiencyScemeId = hf_selected_proficiencyScheme_id.Value.ToString();


                                isUsedInEmployeeEvaluation = ratingSchemeDataHandler.CheckUsageOfRatingLevel(proficiencyScemeId);
                                //bool isUsed = 
                                if (status == Constants.STATUS_ACTIVE_VALUE)
                                {
                                    Boolean nameIsExsists = utilsDataHandler.isDuplicateExist(schemeName, "RS_NAME", "RATING_SCHEME", schemeId, "RS_ID");
                                    if (nameIsExsists)
                                    {
                                        CommonVariables.MESSAGE_TEXT = "Rating scheme name already exist";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                    }
                                    else
                                    {
                                        Boolean updated = ratingSchemeDataHandler.Update(schemeId, schemeName, remarks, status, addedUserId, ratingLevelDataTable);
                                        if (updated)
                                        {
                                            CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated.";
                                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                        }
                                        clearFrom();
                                        loadRatingSchemeGrid();
                                    }
                                }
                                else
                                {
                                    isUsedInEmployeeEvaluation = ratingSchemeDataHandler.CheckUsageOfRatingLevel(proficiencyScemeId);
                                    if (!isUsedInEmployeeEvaluation)
                                    {
                                        Boolean nameIsExsists = utilsDataHandler.isDuplicateExist(schemeName, "RS_NAME", "RATING_SCHEME", schemeId, "RS_ID");
                                        if (nameIsExsists)
                                        {
                                            CommonVariables.MESSAGE_TEXT = "Rating scheme name already exist";
                                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                        }
                                        else
                                        {
                                            Boolean updated = ratingSchemeDataHandler.Update(schemeId, schemeName, remarks, status, addedUserId, ratingLevelDataTable);
                                            if (updated)
                                            {
                                                CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated.";
                                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                            }
                                            clearFrom();
                                            loadRatingSchemeGrid();
                                        }
                                    }
                                    else
                                    {
                                        CommonVariables.MESSAGE_TEXT = "Rating scheme is already used";
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
                ratingSchemeDataHandler = null;
                utilsDataHandler = null;
            }

            
        }

        protected void loadRatingSchemeGrid()
        {
            log.Debug("loadRatingSchemeGrid()");

            RatingSchemeDataHandler ratingSchemeDataHandler = new RatingSchemeDataHandler();
            DataTable dataTable = new DataTable();

            try
            {
                dataTable = ratingSchemeDataHandler.Populate();
                ratingSchemeGrid.DataSource = dataTable;
                ratingSchemeGrid.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                ratingSchemeDataHandler = null;
                dataTable.Dispose();
            }
        }

        protected void ratingSchemeGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            log.Debug("ProficiancySchemeGrid_PageIndexChanging()");
            try
            {
                ratingSchemeGrid.PageIndex = e.NewPageIndex;
                loadRatingSchemeGrid();
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

        protected void ratingSchemeGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("ProficiancySchemeGrid_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.ratingSchemeGrid, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void ratingSchemeGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ProficiancySchemeGrid_SelectedIndexChanged()");
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblErrorMsg);
            Utility.Errorhandler.ClearError(lblError);
            clearSchemeLevels();
            RatingSchemeDataHandler ratingSchemeDataHandler = new RatingSchemeDataHandler();
            DataRow schemeDataRow = null;
            DataTable proficiencyLevels = new DataTable();
            try
            {
                string proficiencySchemeId = ratingSchemeGrid.SelectedRow.Cells[0].Text;
                hf_selected_proficiencyScheme_id.Value = proficiencySchemeId;
                schemeDataRow = ratingSchemeDataHandler.PopulateSchemes(proficiencySchemeId);


                if (schemeDataRow != null)
                {
                    txtName.Text = schemeDataRow["RS_NAME"].ToString().Trim();
                    txtDescription.Text = schemeDataRow["DESCRIPTION"].ToString().Trim();
                    string status = schemeDataRow["STATUS_CODE"].ToString().Trim();
                    ddlStatus.SelectedValue = status;

                }


                proficiencyLevels = ratingSchemeDataHandler.PopulateProficiencyLevels(proficiencySchemeId);
                Session["RatingLevelDataGrid"] = proficiencyLevels;
                if (proficiencyLevels != null)
                {
                    gvRatingLevel.Width = 500;
                    gvRatingLevel.DataSource = (DataTable)Session["RatingLevelDataGrid"];
                    gvRatingLevel.DataBind();
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                ratingSchemeDataHandler = null;
                schemeDataRow = null;
            }

        }

        private void clearFrom()
        {
            log.Debug("clearFrom()");
            try
            {
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                Utils.clearControls(true, txtName, txtDescription, ddlStatus, txtRating, txtWeight, txtRemarks, ddlLevelStatus);
                txtRating.Enabled = true;
                btnRatingLevelSave.Text = "Add";
                Session["RatingLevelDataGrid"] = null;
                gvRatingLevel.DataSource = null;
                gvRatingLevel.DataBind();
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
                Session["RatingLevelDataGrid"] = null;
                initializeRatingLevelDataGrid();
                hf_selected_proficiencyScheme_id.Value = "";
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void initializeRatingLevelDataGrid()
        {
            log.Debug("initializeRatingLevelDataGrid()");

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

            Session["RatingLevelDataGrid"] = dt;
        }

        /// Manipulating Proficiency Level form begin /////

        protected void ratingLevel_btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("ratingLevel_btnSave_Click()");

            Utility.Errorhandler.ClearError(lblError);
            string rating = txtRating.Text.ToString().Trim();
            string weight = txtWeight.Text.ToString().Trim();
            string status = ddlLevelStatus.SelectedValue.ToString().Trim();
            string remarks = txtRemarks.Text.ToString().Trim();
            string description = txtLevelDescription.Text.ToString().Trim();

            RatingSchemeDataHandler ratingSchemeDataHandler = new RatingSchemeDataHandler();
            try
            {
                if (btnRatingLevelSave.Text == "Add")
                {
                    string ratingScemeId = hf_selected_proficiencyScheme_id.Value.ToString();

                    //Boolean isUsedInEmployeeEvaluation = false;


                    //isUsedInEmployeeEvaluation = ratingSchemeDataHandler.CheckUsageOfRatingLevel(proficiencyScemeId);


                    //if (!isUsedInEmployeeEvaluation)
                    // {
                        DataTable ratingLevelDataTable = new DataTable();
                        ratingLevelDataTable = (DataTable)Session["RatingLevelDataGrid"];

                        DataRow[] result = ratingLevelDataTable.Select("RATING = '" + rating + "' OR WEIGHT = '" + weight + "'");
                        if (result.Length == 0)
                        {
                            DataRow ratingLevelDataRow = ratingLevelDataTable.NewRow();
                            ratingLevelDataRow["RATING"] = rating;
                            ratingLevelDataRow["WEIGHT"] = weight;
                            if (status == Constants.STATUS_ACTIVE_VALUE)
                            {
                                ratingLevelDataRow["STATUS_CODE"] = Constants.STATUS_ACTIVE_TAG;
                            }
                            else if (status == Constants.STATUS_INACTIVE_VALUE)
                            {
                                ratingLevelDataRow["STATUS_CODE"] = Constants.STATUS_INACTIVE_TAG;
                            }
                            ratingLevelDataRow["REMARKS"] = remarks;
                            ratingLevelDataRow["DESCRIPTION"] = description;


                            ratingLevelDataTable.Rows.Add(ratingLevelDataRow);

                            gvRatingLevel.Width = 500;
                            gvRatingLevel.DataSource = ratingLevelDataTable;
                            gvRatingLevel.DataBind();
                            Session["RatingLevelDataGrid"] = ratingLevelDataTable;

                            //CommonVariables.MESSAGE_TEXT = "Record(s) saved successfully.";
                            //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblError);
                            clearSchemeLevels();
                            ratingLevelDataTable.Dispose();

                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Rating : ' " + rating + " ' or Weight :'" + weight + "' already exist";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblError);
                            clearSchemeLevels();
                        }
                    //}
                    //else
                    //{

                    //    CommonVariables.MESSAGE_TEXT = "Rating Scheme is already used";
                    //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblError);
                    //}

                }
                else if (btnRatingLevelSave.Text == "Update")
                {
                    string ratingScemeId = hf_selected_proficiencyScheme_id.Value.ToString();

                   //Boolean isUsedInEmployeeEvaluation = false;

                   // //ProficiencySchemeDataHandler proficiencySchemeDataHandler = new ProficiencySchemeDataHandler();
                   // isUsedInEmployeeEvaluation = ratingSchemeDataHandler.CheckUsageOfRatingLevel(proficiencyScemeId);
                   // if (!isUsedInEmployeeEvaluation)
                   // {

                        int selectedIndex = Convert.ToInt32(HiddenField_selected_index.Value);

                        DataTable ratingLevelDataTable = new DataTable();
                        ratingLevelDataTable = (DataTable)Session["RatingLevelDataGrid"];

                        DataRow[] result = ratingLevelDataTable.Select("RATING = '" + rating + "' OR WEIGHT = '" + weight + "'");
                        if (result.Length == 1)
                        {

                            ratingLevelDataTable.Rows[selectedIndex]["RATING"] = rating;
                            ratingLevelDataTable.Rows[selectedIndex]["WEIGHT"] = weight;
                            ratingLevelDataTable.Rows[selectedIndex]["DESCRIPTION"] = description;
                            ratingLevelDataTable.Rows[selectedIndex]["REMARKS"] = remarks;

                            if (status == Constants.STATUS_ACTIVE_VALUE)
                            {
                                ratingLevelDataTable.Rows[selectedIndex]["STATUS_CODE"] = Constants.STATUS_ACTIVE_TAG;
                            }
                            else if (status == Constants.STATUS_INACTIVE_VALUE)
                            {
                                ratingLevelDataTable.Rows[selectedIndex]["STATUS_CODE"] = Constants.STATUS_INACTIVE_TAG;
                            }

                            gvRatingLevel.DataSource = ratingLevelDataTable;
                            gvRatingLevel.DataBind();
                            Session["RatingLevelDataGrid"] = ratingLevelDataTable;

                            //CommonVariables.MESSAGE_TEXT = "Record(s) updated successfully.";
                            //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblError);
                            clearSchemeLevels();
                            ratingLevelDataTable.Dispose();
                        }
                        else
                        {

                            CommonVariables.MESSAGE_TEXT = "Rating : ' " + rating + " ' or Weight :'" + weight + "' already exist";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblError);
                            clearSchemeLevels();

                        }
                    //}
                    //else
                    //{

                    //    CommonVariables.MESSAGE_TEXT = "Rating Scheme is already used";
                    //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblError);
                    //}
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
                ratingSchemeDataHandler = null;
            }
        }

        protected void gvRatingLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("ProficiencyLevelGrid_PageIndexChanging()");

            try
            {
                gvRatingLevel.PageIndex = e.NewPageIndex;
                clearFrom();
                Utility.Errorhandler.ClearError(lblErrorMsg);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void gvRatingLevel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("ProficiencyLevelGrid_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvRatingLevel, "Select$" + e.Row.RowIndex.ToString()));
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

            Session["RatingLevelDataGrid"] = dt;

            gvRatingLevel.DataSource = (DataTable)Session["RatingLevelDataGrid"];
            gvRatingLevel.DataBind();
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
                btnRatingLevelSave.Text = "Add";
                Utils.clearControls(true, txtRating, txtRemarks, txtWeight, ddlLevelStatus, txtLevelDescription);
                txtRating.Enabled = true;
                //Session["RatingLevelDataGrid"] = null;
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void gvRatingLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ProficiencyLevelGridView_SelectedIndexChanged()");
            try
            {
                int index = gvRatingLevel.SelectedRow.RowIndex;

                HiddenField_selected_index.Value = index.ToString();
                txtRating.Text = gvRatingLevel.Rows[index].Cells[0].Text.ToString();
                txtWeight.Text = gvRatingLevel.Rows[index].Cells[1].Text.ToString();
                txtRemarks.Text = gvRatingLevel.Rows[index].Cells[2].Text.ToString().Replace("&nbsp;", "");
                txtLevelDescription.Text = gvRatingLevel.Rows[index].Cells[3].Text.ToString().Replace("&nbsp;", "");


                if (gvRatingLevel.Rows[index].Cells[4].Text.ToString() == Constants.STATUS_ACTIVE_TAG)
                {
                    ddlLevelStatus.SelectedValue = Constants.STATUS_ACTIVE_VALUE;
                }
                else if (gvRatingLevel.Rows[index].Cells[4].Text.ToString() == Constants.STATUS_INACTIVE_TAG)
                {
                    ddlLevelStatus.SelectedValue = Constants.STATUS_INACTIVE_VALUE;
                }


                btnRatingLevelSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
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