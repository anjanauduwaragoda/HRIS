using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.TrainingAndDevelopment;
using System.Data;
using Common;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainingProgramSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadProgrameGridView();
                fillTrainingType();
                fillTrainingCategory();
                tblProgrameDetails.Visible = false;
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            tblProgrameDetails.Visible = false;
        }

        protected void gvAllProgrammes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
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

        protected void gvAllProgrammes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
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
            TrainingInstituteProgrameDataHandler trainingInstituteProgrameDataHandler = new TrainingInstituteProgrameDataHandler();
            try
            {
                int index = gvAllProgrammes.SelectedIndex;
                string programId = gvAllProgrammes.Rows[index].Cells[0].Text.ToString();

                DataTable dtProgramDetail = trainingInstituteProgrameDataHandler.getProgramById(programId);
                tblProgrameDetails.Visible = true;
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

        protected void ddlTrainingType_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
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

                hfProId.Value = gvAllProgrammes.SelectedRow.Cells[0].Text;
                hfProgram.Value = lblPrgmName.Text;

            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }


    }
}