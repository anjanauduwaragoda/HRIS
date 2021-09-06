using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using GroupHRIS.Utility;
using DataHandler.Payroll;
using System.Data;

namespace GroupHRIS.PayRoll
{
    public partial class WebFrmCompanySubcategoryTypes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Errorhandler.ClearError(StatusLabel);
                CompanySubCategoryTypeDataHandler oCompanySubCategoryTypeDataHandler = new CompanySubCategoryTypeDataHandler();

                if (!IsPostBack)
                {
                    ddlStatus.Items.Insert(0, new ListItem("", ""));
                    ddlStatus.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                    ddlStatus.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));

                    if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                    {
                        if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                        {
                            fillCompanies();
                            gvCompanySubcategoryType.DataSource = oCompanySubCategoryTypeDataHandler.Populate();
                            gvCompanySubcategoryType.DataBind();
                        }
                        else
                        {
                            fillCompanies(Session["KeyCOMP_ID"].ToString().Trim());
                            ddlCompany.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                        }
                    }

                    fillCaegory();
                    fillTypeId();
                }
            }
            catch (Exception exp)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, StatusLabel);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string company = ddlCompany.SelectedValue;
            string category = ddlCategory.SelectedValue.ToString();
            string subcategory = ddlSubcategory.SelectedValue.ToString();
            string type = ddlTypeId.SelectedValue.ToString();
            string status = ddlStatus.SelectedValue.ToString();
            string logUser = Session["KeyUSER_ID"].ToString();

            try
            {
                Errorhandler.ClearError(StatusLabel);
                CompanySubCategoryTypeDataHandler oCompanySubCategoryTypeDataHandler = new CompanySubCategoryTypeDataHandler();

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    Boolean Status = oCompanySubCategoryTypeDataHandler.InsertSubCategory(company, category, subcategory, type, status, logUser);
                    gvCompanySubcategoryType.DataSource = oCompanySubCategoryTypeDataHandler.Populate(company);
                    gvCompanySubcategoryType.DataBind();
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", StatusLabel);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    Boolean Status = oCompanySubCategoryTypeDataHandler.UpdateSubcatogoryTypes(company, category, subcategory, type, status, logUser);
                    gvCompanySubcategoryType.Columns[0].Visible = false;
                    gvCompanySubcategoryType.DataSource = oCompanySubCategoryTypeDataHandler.Populate(company);
                    gvCompanySubcategoryType.DataBind();
                    if (Status)
                    {
                        Errorhandler.GetError("1", "Successfully Updated", StatusLabel);
                    }
                    else
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, " Can't Update Inactive Categoryategory ", StatusLabel);
                    }
                }

                lblCompanyName.Text = ddlCompany.SelectedItem.ToString();
                Clear();
            }
            catch (Exception ex)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Record Already Exists ", StatusLabel);
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            gvCompanySubcategoryType.Columns[0].Visible = true;
            CompanySubCategoryTypeDataHandler oCompanySubCategoryTypeDataHandler = new CompanySubCategoryTypeDataHandler();
            gvCompanySubcategoryType.DataSource = oCompanySubCategoryTypeDataHandler.Populate();
            gvCompanySubcategoryType.DataBind();

            Errorhandler.ClearError(StatusLabel);
            Clear();

            lblCompanyName.Text = "";
            ddlCompany.SelectedIndex = 0;

            ddlCompany.Enabled = true;
            ddlCategory.Enabled = true;
            ddlSubcategory.Enabled = true;

            ddlTypeId.Enabled = true;
        }

        public void Clear()
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            ddlCategory.SelectedIndex = 0;
            ddlSubcategory.Items.Clear();
            ddlSubcategory.SelectedIndex = -1;
            ddlTypeId.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSubcategory.Items.Clear();
            string category = ddlCategory.SelectedValue.ToString();
            CompanySubCategoryTypeDataHandler oCompanySubCategoryTypeDataHandler = new CompanySubCategoryTypeDataHandler();
            DataTable dt = new DataTable();
            dt = oCompanySubCategoryTypeDataHandler.GetSubcategories(category).Copy();

            ddlSubcategory.Items.Add(new ListItem(""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string text = dt.Rows[i]["SUB_CATEGORY"].ToString();
                ddlSubcategory.Items.Add(new ListItem(text));
            }
        }

        protected void gvCompanySubcategoryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            ddlCompany.Enabled = false;
            ddlCategory.Enabled = false;
            ddlSubcategory.Enabled = false;

            int SelecttedIndex = gvCompanySubcategoryType.SelectedIndex;
            if (gvCompanySubcategoryType.Columns[0].Visible == true)
            {
                string company = Server.HtmlDecode(gvCompanySubcategoryType.Rows[SelecttedIndex].Cells[0].Text.ToString().Trim());
                ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByText(company));
            }
            else
            {
                string company = lblCompanyName.Text;
                ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByText(company));
            }

            string category = Server.HtmlDecode(gvCompanySubcategoryType.Rows[SelecttedIndex].Cells[1].Text.ToString().Trim());
            ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByText(category));

            string subcategory = Server.HtmlDecode(gvCompanySubcategoryType.Rows[SelecttedIndex].Cells[2].Text.ToString().Trim());
            ddlSubcategory.SelectedIndex = ddlSubcategory.Items.IndexOf(ddlSubcategory.Items.FindByText(subcategory));

            string type = Server.HtmlDecode(gvCompanySubcategoryType.Rows[SelecttedIndex].Cells[3].Text.ToString().Trim());
            ddlTypeId.SelectedIndex = ddlTypeId.Items.IndexOf(ddlTypeId.Items.FindByText(type));

            string status = Server.HtmlDecode(gvCompanySubcategoryType.Rows[SelecttedIndex].Cells[4].Text.ToString().Trim());
            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));

            ddlTypeId.Enabled = true;

            //------------------------------------------------------------
            ddlSubcategory.Items.Clear();
            category = ddlCategory.SelectedValue.ToString();
            CompanySubCategoryTypeDataHandler oCompanySubCategoryTypeDataHandler = new CompanySubCategoryTypeDataHandler();
            DataTable dt = new DataTable();
            dt = oCompanySubCategoryTypeDataHandler.GetSubcategories(category).Copy();

            ddlSubcategory.Items.Add(new ListItem(""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string text = dt.Rows[i]["SUB_CATEGORY"].ToString();
                ddlSubcategory.Items.Add(new ListItem(text));
            }
            ddlSubcategory.SelectedIndex = ddlSubcategory.Items.IndexOf(ddlSubcategory.Items.FindByText(subcategory));

            if (ddlStatus.SelectedIndex == 2)
            {
                ddlTypeId.Enabled = false;
            }
            else
            {
                ddlTypeId.Enabled = true;
            }
        }

        protected void gvCompanySubcategoryType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CompanySubCategoryTypeDataHandler oCompanySubCategoryTypeDataHandler = new CompanySubCategoryTypeDataHandler();

            gvCompanySubcategoryType.PageIndex = e.NewPageIndex;
            gvCompanySubcategoryType.DataSource = oCompanySubCategoryTypeDataHandler.Populate();
            gvCompanySubcategoryType.DataBind();
        }

        protected void gvCompanySubcategoryType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvCompanySubcategoryType, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, StatusLabel);
            }
        }

        protected void ddlSubcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string company = ddlCompany.SelectedValue;
            string category = ddlCategory.SelectedValue.ToString();
            string subcategory = ddlSubcategory.SelectedValue.ToString();
            // string type = ddlTypeId.SelectedValue.ToString();


            CompanySubCategoryTypeDataHandler oCompanySubCategoryTypeDataHandler = new CompanySubCategoryTypeDataHandler();
            Boolean Status = oCompanySubCategoryTypeDataHandler.IsExist(company, category, subcategory);
            if (Status == false)
            {
                ddlTypeId.SelectedIndex = 0;
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Record Already Exists ", StatusLabel);
                ddlTypeId.Enabled = false;
            }
            else
            {
                ddlTypeId.Enabled = true;
            }
        }

        protected void ddlTypeId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Errorhandler.ClearError(StatusLabel);
                string company = ddlCompany.SelectedValue;
                string category = ddlCategory.SelectedValue.ToString();
                //string subcategory = ddlSubcategory.SelectedValue.ToString();
                string type = ddlTypeId.SelectedValue.ToString();

                Errorhandler.ClearError(StatusLabel);
                CompanySubCategoryTypeDataHandler oCompanySubCategoryTypeDataHandler = new CompanySubCategoryTypeDataHandler();
                DataTable table = new DataTable();
                table = oCompanySubCategoryTypeDataHandler.ExistingTypeId(company, category, type).Copy();

                if (table.Rows.Count > 0)
                {
                    string var = table.Rows[0]["SUB_CATEGORY"].ToString();
                    Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, " Type ID Already Exists for " + var , StatusLabel);
                }
                else
                {

                }
            }
            catch (Exception)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Update Faliure", StatusLabel);
            }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);
            CompanySubCategoryTypeDataHandler oCompanySubCategoryTypeDataHandler = new CompanySubCategoryTypeDataHandler();
            
            if (ddlCompany.SelectedIndex == 0)
            {
                
                lblCompanyName.Text = "";
                gvCompanySubcategoryType.Columns[0].Visible = true;
                gvCompanySubcategoryType.DataSource = oCompanySubCategoryTypeDataHandler.Populate();
                gvCompanySubcategoryType.DataBind();
            }
            else
            {
                string company = ddlCompany.SelectedValue;
                lblCompanyName.Text = ddlCompany.SelectedItem.ToString();
                DataTable dr = oCompanySubCategoryTypeDataHandler.Populate(company);
                if (dr.Rows.Count > 0)
                {
                    lblmsg.Visible = false;
                    gvCompanySubcategoryType.Columns[0].Visible = false;
                    gvCompanySubcategoryType.DataSource = dr;
                    gvCompanySubcategoryType.DataBind();
                }
                else
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "No Records To Display";
                    gvCompanySubcategoryType.DataSource = null;
                    gvCompanySubcategoryType.DataBind();
                }
            }
        }

        private void fillCompanies()
        {
            CompanySubCategoryTypeDataHandler companySubCategoryTypeDataHandler = new CompanySubCategoryTypeDataHandler();
            DataTable companies = new DataTable();

            try
            {
                if (Cache["Companies"] != null)
                {
                    companies = (DataTable)Cache["Companies"];
                }
                else
                {
                    companies = companySubCategoryTypeDataHandler.getCompanyIdCompName().Copy();
                }

                ddlCompany.Items.Clear();

                if (companies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlCompany.Items.Add(Item);

                    foreach (DataRow dataRow in companies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlCompany.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                companySubCategoryTypeDataHandler = null;
                companies.Dispose();
            }
        }

        private void fillCompanies(string companyId)
        {
            CompanySubCategoryTypeDataHandler companySubCategoryTypeDataHandler = new CompanySubCategoryTypeDataHandler();
            DataTable companies = new DataTable();

            try
            {
                companies = companySubCategoryTypeDataHandler.getCompanyIdCompName(companyId).Copy();

                ddlCompany.Items.Clear();

                if (companies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlCompany.Items.Add(Item);

                    foreach (DataRow dataRow in companies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlCompany.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                companySubCategoryTypeDataHandler = null;
                companies.Dispose();
            }

        }

        public void fillCaegory()
        {
            CompanySubCategoryTypeDataHandler oCompanySubCategoryTypeDataHandler = new CompanySubCategoryTypeDataHandler();
            DataTable table = new DataTable();
            table = oCompanySubCategoryTypeDataHandler.GetCategories().Copy();

            ddlCategory.Items.Add(new ListItem("", ""));

            for (int i = 0; i < table.Rows.Count; i++)
            {
                string text = table.Rows[i]["CATEGORY"].ToString();
                ddlCategory.Items.Add(new ListItem(text));
            }
        }

        public void fillTypeId()
        {
            ddlTypeId.Items.Clear();
            ddlTypeId.Items.Add(string.Empty);
            for (int i = 1; i <= 30; i++)
            {
                ddlTypeId.Items.Add(i.ToString());
            }
        }

    }
}