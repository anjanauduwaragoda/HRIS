using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.Payroll;
using Common;
using GroupHRIS.Utility;

namespace GroupHRIS.PayRoll
{
    public partial class WebFrmCompanyOTCategory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CompanyOTCategoryDataHandler companyOTCategoryDataHandler = new CompanyOTCategoryDataHandler();
                
            if (!IsPostBack)
            {
                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        fillCompanies();
                    }
                    else
                    {
                        fillCompanies(Session["KeyCOMP_ID"].ToString().Trim());
                        ddlCompany.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                    }
                }

                //gvCompanyOTCategory.DataSource = companyOTCategoryDataHandler.LoadDataGrid();
                //gvCompanyOTCategory.DataBind();

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string company = ddlCompany.SelectedValue;
            string oTCategory = txtOTcategory.Text;
            string description = txtDescription.Text;
            string logUser = Session["KeyUSER_ID"].ToString();

            string chk = "0";

            if (chkIsActive.Checked == true)
            {
                chk = "1";
            }

            try
            {

                Errorhandler.ClearError(StatusLabel);
                CompanyOTCategoryDataHandler companyOTCategoryDataHandler = new CompanyOTCategoryDataHandler();
                gvCompanyOTCategory.DataSource = null;

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    Boolean Status = companyOTCategoryDataHandler.InsertCompanyOTCategory(company, oTCategory, description, logUser, chk);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", StatusLabel);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string id = Session["ID"].ToString();
                    Boolean Status = companyOTCategoryDataHandler.UpdateCompanyOTCategory(company, oTCategory, description, logUser, chk, id);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Updated", StatusLabel);
                }
                gvCompanyOTCategory.DataSource = companyOTCategoryDataHandler.LoadDataGrid(company);
                gvCompanyOTCategory.DataBind();
            }
            catch (Exception)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Update Faliure", StatusLabel);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            ddlCompany.SelectedIndex = 0;
            gvCompanyOTCategory.DataSource = null;
            gvCompanyOTCategory.DataBind();

        }

        public void Clear()
        {
            Errorhandler.ClearError(StatusLabel);
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            txtOTcategory.Text = "";
            txtDescription.Text = "";
            chkIsActive.Checked = false;
        }

        protected void gvCompanyOTCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CompanyOTCategoryDataHandler oCompanyOTCategoryDataHandler = new CompanyOTCategoryDataHandler();
            gvCompanyOTCategory.PageIndex = e.NewPageIndex;
            gvCompanyOTCategory.DataSource = oCompanyOTCategoryDataHandler.LoadDataGrid();
            gvCompanyOTCategory.DataBind();
            oCompanyOTCategoryDataHandler = null;
        }

        protected void gvCompanyOTCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            Errorhandler.ClearError(StatusLabel);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            int SelectedIndex = gvCompanyOTCategory.SelectedIndex;

            string company = HttpUtility.HtmlDecode(gvCompanyOTCategory.Rows[SelectedIndex].Cells[0].Text.Trim());
            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(company));

            txtOTcategory.Text = Server.HtmlDecode(gvCompanyOTCategory.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
            
            txtDescription.Text = Server.HtmlDecode(gvCompanyOTCategory.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());

            string chk = Server.HtmlDecode(gvCompanyOTCategory.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
            if (chk == Constants.STATUS_ACTIVE_TAG)
            {
                chkIsActive.Checked = true;
            }
            else
            {
                chkIsActive.Checked = false;
            }

            string OTId = Server.HtmlDecode(gvCompanyOTCategory.Rows[SelectedIndex].Cells[4].Text.ToString().Trim());
            Session["ID"] = OTId;
        }

        protected void gvCompanyOTCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvCompanyOTCategory, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, StatusLabel);
            }
        }

        private void fillCompanies()
        {
            CompanyOTCategoryDataHandler companyOTCategoryDataHandler = new CompanyOTCategoryDataHandler();
            DataTable companies = new DataTable();

            try
            {
                if (Cache["Companies"] != null)
                {
                    companies = (DataTable)Cache["Companies"];
                }
                else
                {
                    companies = companyOTCategoryDataHandler.getCompanyIdCompName().Copy();
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
                companyOTCategoryDataHandler = null;
                companies.Dispose();
            }
        }

        private void fillCompanies(string companyId)
        {
            CompanyOTCategoryDataHandler companyOTCategoryDataHandler = new CompanyOTCategoryDataHandler();
            DataTable companies = new DataTable();

            try
            {
                companies = companyOTCategoryDataHandler.getCompanyIdCompName(companyId).Copy();

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
                companyOTCategoryDataHandler = null;
                companies.Dispose();
            }

        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear();
            gvCompanyOTCategory.DataSource = null;
            string company = ddlCompany.SelectedValue.ToString();

            CompanyOTCategoryDataHandler oCompanyOTCategoryDataHandler = new CompanyOTCategoryDataHandler();
            
            gvCompanyOTCategory.DataSource = oCompanyOTCategoryDataHandler.LoadDataGrid(company);
            gvCompanyOTCategory.DataBind();
        }

    }
}