using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using GroupHRIS.Utility;
using DataHandler;
using DataHandler.TrainingAndDevelopment;
using DataHandler.Utility;
using System.Data;
using NLog;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainingSubcategory : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";


        protected void Page_Load(object sender, EventArgs e)
        {

            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmTrainingSubcategory : Page_Load");


            if (Session["KeyLOGOUT_STS"].Equals("0"))
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }

            if (!IsPostBack)
            {
                filStatus();
                fillTrainingCategories();
                fillTrainingSubcategories();
            }
        }

        private void filStatus()
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

        private void fillTrainingCategories()
        {
            TrainingCategoryDataHandler trainingCategoryDataHandler = new TrainingCategoryDataHandler();
            DataTable dataTable = new DataTable();

            try
            {
                dataTable = trainingCategoryDataHandler.getCategories();

                ddlTrainingCategory.Items.Clear();

                if (dataTable.Rows.Count > 0)
                {
                    ListItem listItemBlank = new ListItem();
                    listItemBlank.Text = "";
                    listItemBlank.Value = "";

                    ddlTrainingCategory.Items.Add(listItemBlank);

                    foreach(DataRow dr in dataTable.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dr["CATEGORY_NAME"].ToString();
                        listItem.Value = dr["TRAINING_CATEGORY_ID"].ToString();
                        
                        ddlTrainingCategory.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                trainingCategoryDataHandler = null;
                dataTable.Dispose();
            }
        }

        private void fillTrainingCategoriesWithInactiveCategory(string sCategoryId)
        {
            TrainingCategoryDataHandler trainingCategoryDataHandler = new TrainingCategoryDataHandler();
            DataTable dataTable = new DataTable();

            try
            {
                dataTable = trainingCategoryDataHandler.getCategories(Constants.CON_ACTIVE_STATUS, sCategoryId.Trim());

                ddlTrainingCategory.Items.Clear();

                if (dataTable.Rows.Count > 0)
                {
                    ListItem listItemBlank = new ListItem();
                    listItemBlank.Text = "";
                    listItemBlank.Value = "";

                    ddlTrainingCategory.Items.Add(listItemBlank);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dr["CATEGORY_NAME"].ToString();
                        listItem.Value = dr["TRAINING_CATEGORY_ID"].ToString();

                        ddlTrainingCategory.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                trainingCategoryDataHandler = null;
                dataTable.Dispose();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            Utility.Errorhandler.ClearError(lblMessage);

            TrainingSubcategoryDataHandler trainingSubcategoryDataHandler = new TrainingSubcategoryDataHandler();
            UtilsDataHandler utilsDataHandler = new UtilsDataHandler();

            string addedBy      = String.Empty;
            string sCategoryId  = String.Empty;
            string sTypeName    = String.Empty;
            string sDescription = String.Empty;
            string sStatus      = String.Empty;

            try
            {
                sCategoryId     = ddlTrainingCategory.SelectedValue.Trim();
                sTypeName       = txtSubcategory.Text.Trim();
                sDescription    = txtDescription.Text.Trim();
                sStatus         = ddlStatus.SelectedValue;
                
                if (Session["KeyUSER_ID"] != null)
                {
                    addedBy = Session["KeyUSER_ID"].ToString();
                }

                // check for duplicates
                // at saving
                if ((btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT) && (utilsDataHandler.isDuplicateExist(sTypeName, "TYPE_NAME", "TRAINING_SUB_CATEGORY"))) { Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Subcategory name already exist", lblMessage); return; }
                
                // at updating ---------------------
                if ((btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT) && (utilsDataHandler.isDuplicateExist(sTypeName, "TYPE_NAME", "TRAINING_SUB_CATEGORY", hfSubcategoryId.Value.ToString(), "TYPE_ID"))) { Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Subcategory name already exist", lblMessage); return; }

                if ((btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT) && (sCategoryId.Trim() == hfInactiveCategoryId.Value.Trim())) { Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Not allowed to update with an inactive Training category.", lblMessage); return; }

                //------------------------------------------------
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Insert");
                    Boolean isInserted = trainingSubcategoryDataHandler.Insert(sCategoryId, sTypeName, sDescription, sStatus, addedBy);

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                    if (isInserted)
                    {
                        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('Designation is saved ..')", true); 
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblMessage);
                    }

                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> update");
                    string subCategoryId = "";

                    if (hfSubcategoryId.Value.ToString().Trim() != "") { subCategoryId = hfSubcategoryId.Value.ToString().Trim(); }

                    if (subCategoryId != "")
                    {
                        Boolean isUpdated = trainingSubcategoryDataHandler.Update(subCategoryId, sCategoryId, sTypeName, sDescription, sStatus, addedBy);

                        if (isUpdated)
                        {
                            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('Competency Group is updated ..')", true); 
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
                        }
                    }

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                }
                
                fillTrainingCategories();
                clear();

                if (ddlTrainingCategory.SelectedValue.Trim() == String.Empty)
                {
                    fillTrainingSubcategories();
                }
                else
                {
                    string sCatId = ddlTrainingCategory.SelectedValue.Trim();
                    fillTrainingSubcategories(sCatId);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                trainingSubcategoryDataHandler = null;
                utilsDataHandler = null;
            }

        }

        private void clear()
        {
            Utils.clearControls(true, ddlStatus, ddlTrainingCategory, txtDescription, txtSubcategory, hfInactiveCategoryId,hfSubcategoryId);
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            
        }



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
            fillTrainingCategories();
            Utility.Errorhandler.ClearError(lblMessage);
        }

        public void fillTrainingSubcategories()
        {
            TrainingSubcategoryDataHandler trainingSubcategoryDataHandler = new TrainingSubcategoryDataHandler();
            DataTable dataTable = new DataTable();

            try
            {

                dataTable = trainingSubcategoryDataHandler.populate().Copy();
                
                gvTrainingSubcategory.DataSource = dataTable;
                gvTrainingSubcategory.DataBind();

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                trainingSubcategoryDataHandler = null;
                dataTable.Dispose();
            }
        }

        public void fillTrainingSubcategories(String sCategoryId)
        {
            TrainingSubcategoryDataHandler trainingSubcategoryDataHandler = new TrainingSubcategoryDataHandler();
            DataTable dataTable = new DataTable();

            try
            {
                string categoryId = sCategoryId;

                dataTable = trainingSubcategoryDataHandler.populate(categoryId).Copy();

                gvTrainingSubcategory.DataSource = dataTable;
                gvTrainingSubcategory.DataBind();

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                trainingSubcategoryDataHandler = null;
                dataTable.Dispose();
            }
        }

        protected void gvTrainingSubcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblMessage);
            TrainingSubcategoryDataHandler trainingSubcategoryDataHandler = new TrainingSubcategoryDataHandler();
            DataRow dataRow = null;
            hfSubcategoryId.Value = String.Empty;

            try
            {
                string subCatId = gvTrainingSubcategory.SelectedRow.Cells[1].Text;

                dataRow = trainingSubcategoryDataHandler.getSubCategory(subCatId);

                if (dataRow != null)
                {
                    if (Utils.isValueExistInDropDownList(dataRow["CATEGORY_ID"].ToString().Trim(), ddlTrainingCategory))
                    {
                        ddlTrainingCategory.SelectedValue = dataRow["CATEGORY_ID"].ToString().Trim();
                    }
                    else
                    {
                        fillTrainingCategoriesWithInactiveCategory(dataRow["CATEGORY_ID"].ToString().Trim());
                        ddlTrainingCategory.SelectedValue = dataRow["CATEGORY_ID"].ToString().Trim();
                        hfInactiveCategoryId.Value = dataRow["CATEGORY_ID"].ToString().Trim();
                    }

                    hfSubcategoryId.Value = dataRow["TYPE_ID"].ToString().Trim();
                    txtSubcategory.Text = dataRow["TYPE_NAME"].ToString().Trim();
                    txtDescription.Text = dataRow["DESCRIPTION"].ToString().Trim();
                    ddlStatus.SelectedValue = dataRow["STATUS_CODE"].ToString().Trim();
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                trainingSubcategoryDataHandler = null;
                dataRow = null;
            }
        }

        protected void gvTrainingSubcategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvTrainingSubcategory, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void gvTrainingSubcategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTrainingSubcategory.SelectedIndex = e.NewPageIndex;

            if (ddlTrainingCategory.SelectedValue.Trim() == String.Empty)
            {
                fillTrainingSubcategories();
            }
            else
            {
                string sCategoryId = ddlTrainingCategory.SelectedValue.Trim();
                fillTrainingSubcategories(sCategoryId);
            }
        }





    }
}