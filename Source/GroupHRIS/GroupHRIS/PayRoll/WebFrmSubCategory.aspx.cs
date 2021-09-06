using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GroupHRIS.Utility;
using DataHandler.Payroll;
using Common;
using System.Data;

namespace GroupHRIS.PayRoll
{
    public partial class WebFrmSubCategory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SubCategoryDataHandler oSubCategoryDataHandler = new SubCategoryDataHandler();

            txtSubcategory.Enabled = true;
            ddlCategory.Enabled = true;
            try
            {
                Errorhandler.ClearError(StatusLabel);
                gvSubcategory.DataSource = oSubCategoryDataHandler.GetSubCategories();
                gvSubcategory.DataBind();

                if (!IsPostBack)
                {
                    ddlStstus.Items.Insert(0, new ListItem("", ""));
                    ddlStstus.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                    ddlStstus.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));

                    fillCaegory();
                }

            }
            catch (Exception exp)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, StatusLabel);
            }
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string Category = ddlCategory.SelectedValue;
            string subCategory = txtSubcategory.Text;
            string subCategoryRemaks = txtSubcategoryRemarks.Text;
            string StatusCode = ddlStstus.SelectedValue.ToString();
            string logUser = Session["KeyUSER_ID"].ToString();

            if (txtSubcategory.Text != "")
            {
                try
                {
                    Errorhandler.ClearError(StatusLabel);
                    SubCategoryDataHandler oSubCategoryDataHandler = new SubCategoryDataHandler();

                    if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                    {
                        Boolean Status = oSubCategoryDataHandler.InsertSubCategory(subCategory, subCategoryRemaks, StatusCode, logUser,  Category);
                        gvSubcategory.DataSource = oSubCategoryDataHandler.GetSubCategories();
                        gvSubcategory.DataBind();

                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", StatusLabel);
                    }
                    else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {
                        string subcategoryUpdate = Session["Subcategory"].ToString();
                        Boolean Status = oSubCategoryDataHandler.UpdateSubCategory(subcategoryUpdate,subCategory, subCategoryRemaks, StatusCode, logUser, Category);
                        gvSubcategory.DataSource = oSubCategoryDataHandler.GetSubCategories();
                        gvSubcategory.DataBind();
                        if (Status)
                        {
                            Errorhandler.GetError("1", " Successfully Updated ", StatusLabel);
                        }
                        else
                        {
                            Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, " Can't Update Inactive Subcategory ", StatusLabel);
                        }
                    }
                }
                catch (Exception)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Record Already Exists", StatusLabel);
                }

                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                txtSubcategory.Text = "";
                txtSubcategoryRemarks.Text = "";
                ddlStstus.SelectedIndex = 0;
                ddlCategory.SelectedIndex = 0;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            ddlCategory.SelectedIndex = 0;
            txtSubcategory.Text = "";
            txtSubcategoryRemarks.Text = "";
            ddlStstus.SelectedIndex = 0;

            txtSubcategory.Enabled = true;
            txtSubcategoryRemarks.Enabled = true;
        }

        protected void gvSubcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            txtSubcategory.Text = "";
            ddlCategory.Enabled = false;

            int iIndex = 0;
            int iPageIndex = 0;
            int SelecttedIndex = gvSubcategory.SelectedIndex;
            iPageIndex = gvSubcategory.PageIndex;

            iIndex = (gvSubcategory.PageSize * iPageIndex) + SelecttedIndex;

            //gvSubcategory.PageIndex = e.NewPageIndex;
            
            txtSubcategory.Text = Server.HtmlDecode(gvSubcategory.Rows[SelecttedIndex].Cells[1].Text.ToString().Trim());
            Session["Subcategory"] = txtSubcategory.Text;
            txtSubcategoryRemarks.Text = Server.HtmlDecode(gvSubcategory.Rows[SelecttedIndex].Cells[2].Text.ToString().Trim());
            string status = Server.HtmlDecode(gvSubcategory.Rows[SelecttedIndex].Cells[3].Text.ToString().Trim());
            ddlStstus.SelectedIndex = ddlStstus.Items.IndexOf(ddlStstus.Items.FindByText(status));
            string category = Server.HtmlDecode(gvSubcategory.Rows[SelecttedIndex].Cells[0].Text.ToString().Trim());
            ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByText(category));

            if (ddlStstus.SelectedIndex == 2)
            {
                txtSubcategory.Enabled = false;
                txtSubcategoryRemarks.Enabled = false;

            }
            else
            {
                txtSubcategory.Enabled = true;
                txtSubcategoryRemarks.Enabled = true;
            }
        }

        protected void gvSubcategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvSubcategory, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, StatusLabel);
            }
        }

        protected void gvSubcategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SubCategoryDataHandler oSubCategoryDataHandler = new SubCategoryDataHandler();

            gvSubcategory.PageIndex = e.NewPageIndex;
            gvSubcategory.DataSource = oSubCategoryDataHandler.GetSubCategories();
            gvSubcategory.DataBind();
        }

        public void fillCaegory()
        {
            SubCategoryDataHandler oSubCategoryDataHandler = new SubCategoryDataHandler();
            DataTable table = new DataTable();
            table = oSubCategoryDataHandler.GetCategories().Copy();

            ddlCategory.Items.Add(new ListItem("", ""));

            for (int i = 0; i < table.Rows.Count; i++)
            {
                string text = table.Rows[i]["CATEGORY"].ToString();
                ddlCategory.Items.Add(new ListItem(text));
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            SubCategoryDataHandler oSubCategoryDataHandler = new SubCategoryDataHandler();

            //GetSubcategoryGrid
            DataTable dt = oSubCategoryDataHandler.GetSubcategoryGrid(ddlCategory.SelectedValue);
            if (dt.Rows.Count > 0)
            {
                lblmsg.Visible = false;
                gvSubcategory.DataSource = oSubCategoryDataHandler.GetSubcategoryGrid(ddlCategory.SelectedValue);
                gvSubcategory.DataBind();
            }
            else
            {
                lblmsg.Visible = true;
                lblmsg.Text = "No Records To Display";
                gvSubcategory.DataSource = null;
                gvSubcategory.DataBind();
            }
        }

    }
}