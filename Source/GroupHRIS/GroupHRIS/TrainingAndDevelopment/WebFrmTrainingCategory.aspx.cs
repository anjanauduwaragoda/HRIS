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
    public partial class WebFrmTrainingCategory : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmTrainingCategory : Page_Load");


            if (Session["KeyLOGOUT_STS"].Equals("0"))
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }

            if (!IsPostBack)
            {
                filStatus();
                fillTrainingCategories();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            Utility.Errorhandler.ClearError(lblMessage);

            TrainingCategoryDataHandler trainingCategoryDataHandler = new TrainingCategoryDataHandler();
            UtilsDataHandler utilsDataHandler = new UtilsDataHandler();
            TrainingSubcategoryDataHandler trainingSubcategoryDataHandler = new TrainingSubcategoryDataHandler();

            string addedBy = "";

            string categoryName = "";
            string description = "";
            string statusCode = "";
            

            try
            {
                categoryName = txtCategoryName.Text.Trim();
                description = txtDescription.Text.Trim();
                statusCode = ddlStatus.SelectedItem.Value.ToString();
                              

                // check for duplicates
                // at saving
                if ((btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT) && (utilsDataHandler.isDuplicateExist(categoryName,"CATEGORY_NAME","TRAINING_CATEGORY"))) { Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Category name already exist", lblMessage); return; }
                // at updating
                if ((btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT) && (utilsDataHandler.isDuplicateExist(categoryName, "CATEGORY_NAME", "TRAINING_CATEGORY",hfCategoryId.Value.ToString(),"TRAINING_CATEGORY_ID"))) { Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Category name already exist", lblMessage); return; }

                // at updating avoid status change from active to inactive if active subcategories exist
                if(btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    if ((hfPrevStatus.Value.Trim().Equals(Constants.CON_ACTIVE_STATUS)) && (ddlStatus.SelectedValue.Trim().Equals(Constants.CON_INACTIVE_STATUS)) && (trainingSubcategoryDataHandler.isActiveSubcategoryExist(hfCategoryId.Value.ToString().Trim())))
                    { 
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Can not make Inactive. Active subcategories exist", lblMessage);
                        return;
                    }
                }

                if (Session["KeyUSER_ID"] != null)
                {
                    addedBy = Session["KeyUSER_ID"].ToString();
                }

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Insert");
                    Boolean isInserted = trainingCategoryDataHandler.Insert(categoryName, description, statusCode, addedBy);

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
                    string categoryGroupId = "";

                    if (hfCategoryId.Value.ToString().Trim() != "") { categoryGroupId = hfCategoryId.Value.ToString().Trim(); }

                    if (categoryGroupId != "")
                    {
                        Boolean isUpdated = trainingCategoryDataHandler.Update(categoryGroupId, categoryName, description, statusCode, addedBy);

                        if (isUpdated)
                        {
                            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('Competency Group is updated ..')", true); 
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
                        }

                    }

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                }

                clear();
                fillTrainingCategories();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                trainingCategoryDataHandler = null;
                utilsDataHandler = null;
            }
        }

        

        private void fillTrainingCategories()
        {
            TrainingCategoryDataHandler trainingCategoryDataHandler = new TrainingCategoryDataHandler();
            DataTable trainingCategories = new DataTable();
            try
            {

                trainingCategories = trainingCategoryDataHandler.populate();
                gvTrainingCategory.DataSource = trainingCategories;
                gvTrainingCategory.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                trainingCategoryDataHandler = null;
                trainingCategories.Dispose();
            }

        }


        private void clear()
        {
            Utils.clearControls(true, txtCategoryName, txtDescription, hfCategoryId, hfPrevStatus,ddlStatus);            
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;

        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
            Utility.Errorhandler.ClearError(lblMessage);
        }

        protected void gvTrainingCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvTrainingCategory, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void gvTrainingCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvTrainingCategory_PageIndexChanging()");

            try
            {
                gvTrainingCategory.PageIndex = e.NewPageIndex;
                fillTrainingCategories();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        protected void gvTrainingCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblMessage);
            TrainingCategoryDataHandler trainingCategoryDataHandler = new TrainingCategoryDataHandler();
            DataRow dataRow = null;
            hfCategoryId.Value = String.Empty;

            try
            {
                string tCategoryId = gvTrainingCategory.SelectedRow.Cells[0].Text;

                dataRow = trainingCategoryDataHandler.populate(tCategoryId);

                if (dataRow != null)
                {
                    hfCategoryId.Value = dataRow["TRAINING_CATEGORY_ID"].ToString().Trim();                    
                    txtCategoryName.Text = dataRow["CATEGORY_NAME"].ToString().Trim();
                    txtDescription.Text = dataRow["CATEGORY_DESCRIPTION"].ToString().Trim();
                    ddlStatus.SelectedValue = dataRow["STATUS_CODE"].ToString().Trim();
                    hfPrevStatus.Value = dataRow["STATUS_CODE"].ToString().Trim();
                }

            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                trainingCategoryDataHandler = null;
                dataRow = null;
            }
        }
    }
}