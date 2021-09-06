using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.Payroll;
using System.Data;
using GroupHRIS.Utility;

namespace GroupHRIS.PayRoll
{
    public partial class WebFrmMeargeCompany : System.Web.UI.Page
    {

        MeargeCompanyDataHandler meargeCompany = new MeargeCompanyDataHandler();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        fillCompanies();
                        gvMergeCompany.DataSource = meargeCompany.populate();
                        gvMergeCompany.DataBind();
                    }
                    else
                    {
                        fillCompanies(Session["KeyCOMP_ID"].ToString().Trim());
                        ddlCompany.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                    }
                }

                fillSP();
            }


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string company = ddlCompany.SelectedValue.ToString();
            string queryId = ddlQuery.SelectedValue.ToString();
            string remarks = txtRemarks.Text;
            string logUser = Session["KeyUSER_ID"].ToString();

            try
            {

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    Boolean Status = meargeCompany.Insert(company, queryId, remarks, logUser);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string meargeId = Session["meargeId"].ToString();
                    Boolean Status = meargeCompany.Update(meargeId,company, queryId, remarks, logUser);
                }

                gvMergeCompany.DataSource = meargeCompany.populate();
                gvMergeCompany.DataBind();

                ddlCompany.SelectedIndex = 0;
                ddlQuery.SelectedIndex = 0;
                txtRemarks.Text = "";
            }
            catch (Exception)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, " Company is already exist. ", StatusLabel);
              
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Errorhandler.ClearError(StatusLabel);
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                ddlCompany.SelectedIndex = 0;
                ddlQuery.SelectedIndex = 0;
                txtRemarks.Text = "";
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void grdMergeCompany_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMergeCompany.PageIndex = e.NewPageIndex;
            gvMergeCompany.DataSource = meargeCompany.populate();
            gvMergeCompany.DataBind();
            meargeCompany = null;
        }

        protected void grdMergeCompany_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvMergeCompany, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, StatusLabel);
            }
        }

        protected void grdMergeCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

            int SelectedIndex = gvMergeCompany.SelectedIndex;
            Session["meargeId"] = Server.HtmlDecode(gvMergeCompany.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());

            string company = HttpUtility.HtmlDecode(gvMergeCompany.Rows[SelectedIndex].Cells[1].Text.Trim());
            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByText(company));

            string query = HttpUtility.HtmlDecode(gvMergeCompany.Rows[SelectedIndex].Cells[2].Text.Trim());
            ddlQuery.SelectedIndex = ddlQuery.Items.IndexOf(ddlQuery.Items.FindByText(query));

            txtRemarks.Text = Server.HtmlDecode(gvMergeCompany.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
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

        private void fillSP()
        {
            DataTable storedProcedure = new DataTable();
            storedProcedure = meargeCompany.getStoredProcedure();
            ddlQuery.Items.Clear();

            if (storedProcedure.Rows.Count > 0)
            {
                ListItem Item = new ListItem();
                Item.Text = "";
                Item.Value = "";
                ddlQuery.Items.Add(Item);

                foreach (DataRow dataRow in storedProcedure.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = dataRow["PROCEDURE_NAME"].ToString();
                    listItem.Value = dataRow["PROCEDURE_ID"].ToString();

                    ddlQuery.Items.Add(listItem);
                }
            }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlQuery.SelectedIndex = 0;
            txtRemarks.Text = "";

            string company = ddlCompany.SelectedValue.ToString();

        }

    }
}