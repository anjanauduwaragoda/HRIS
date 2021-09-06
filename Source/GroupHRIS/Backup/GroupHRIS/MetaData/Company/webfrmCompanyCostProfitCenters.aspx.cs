using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.MetaData;
using System.Data;
using Common;
using GroupHRIS.Utility;

namespace GroupHRIS.MetaData.Company
{
    public partial class webfrmCompanyCostProfitCenters : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CompanyDataHandler oCompanyDataHandler = new CompanyDataHandler();
                CostProfitCenterDataHandler oCostProfit = new CostProfitCenterDataHandler();
                DataTable dataTable = new DataTable();

                dataTable = oCompanyDataHandler.getCompanyIdCompName();

                ddlcompany.Items.Insert(0, new ListItem("", ""));
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string Value = dataTable.Rows[i]["COMPANY_ID"].ToString();
                    string Text = dataTable.Rows[i]["COMP_NAME"].ToString();
                    ddlcompany.Items.Insert(i + 1, new ListItem(Text, Value));
                }
                btnSave.Enabled = false;
                btnClear.Enabled = false;

                FillStatus();
                FillCostProfit();
                costProfitCenter();
                gvCostProfitCenter.DataSource = oCostProfit.GetGidData();
                gvCostProfitCenter.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string company = ddlcompany.SelectedValue.ToString();
            string cpCenter = ddlCostProfit.SelectedValue.ToString();
            string code = txtcode.Text.Trim();
            string name = txtName.Text.Trim();
            string status = ddlStatus.SelectedValue.ToString();
            string logUser = Session["KeyUSER_ID"].ToString();

            CostProfitCenterDataHandler oCostProfit = new CostProfitCenterDataHandler();

            try
            {
                DataTable dataTable = (DataTable)Session["dataBucket"];

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    //Boolean isInsert = oCostProfit.Insert(company, cpCenter, code,name, STATUS, logUser);

                    Boolean isInsert = oCostProfit.Insert(dataTable, logUser);
                    if (isInsert == true)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", lblMessage);
                    }
                    else
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Error occured ", lblMessage);
                    }
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {

                    string sesstionCode = Session["sesstionCode"].ToString();
                    //Get the status of the selected cost center and check status != selectedcostcenterStatus
                    string cpStatus = oCostProfit.getStatus(sesstionCode);
                    if (status == "1" || status != cpStatus)
                    {
                        Boolean isUpdate = oCostProfit.Update(company, cpCenter, code, sesstionCode, name, status, logUser);
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Updated", lblMessage);
                    }
                    else
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Can't Update Inactive Cost/Profit Center", lblMessage);
                    }
                }


                gvAddcostProfitCenter.DataSource = null;
                gvAddcostProfitCenter.DataBind();

                gvCostProfitCenter.DataSource = oCostProfit.GetCostProfitCenterByCompany(ddlcompany.SelectedValue);
                gvCostProfitCenter.DataBind();

                cleardateBucket();
            }
            catch (Exception ex)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Cost/Profit center Code '" + code + "' Already Exist ", lblMessage);
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            cleardateBucket();

            gvAddcostProfitCenter.DataSource = null;
            gvAddcostProfitCenter.DataBind();

            btnAdd.Enabled = true;
            btnAddclear.Enabled = true;

            btnSave.Enabled = false;
            btnClear.Enabled = false;


            ddlcompany.Enabled = true;
            ddlCostProfit.Enabled = true;
            txtcode.Enabled = true;

            CostProfitCenterDataHandler oCostProfit = new CostProfitCenterDataHandler();
            gvCostProfitCenter.DataSource = oCostProfit.GetGidData();
            gvCostProfitCenter.DataBind();
        }

        public void Clear()
        {
            cleardateBucket();
            Errorhandler.ClearError(lblMessage);
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            ddlcompany.SelectedIndex = 0;
            ddlCostProfit.SelectedIndex = 0;
            txtcode.Text = "";
            ddlStatus.SelectedIndex = 0;
            txtName.Text = "";

        }

        private void FillStatus()
        {
            ddlStatus.Items.Insert(0, new ListItem("", ""));
            ddlStatus.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.STATUS_ACTIVE_VALUE));
            ddlStatus.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.STATUS_INACTIVE_VALUE));
        }

        private void FillCostProfit()
        {
            ddlCostProfit.Items.Insert(0, new ListItem("", ""));
            ddlCostProfit.Items.Insert(1, new ListItem(Constants.STATUS_COST_TAG, Constants.STATUS_COST_VALUE));
            ddlCostProfit.Items.Insert(2, new ListItem(Constants.STATUS_PROFIT_TAG, Constants.STATUS_PROFIT_VALUE));
        }

        protected void gvCostProfitCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            btnAdd.Enabled = false;
            btnAddclear.Enabled = false;
            int SelectedIndex = gvCostProfitCenter.SelectedIndex;
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            ddlcompany.Enabled = false;
            ddlCostProfit.Enabled = false;
            //txtcode.Enabled = false;


            string company = Server.HtmlDecode(gvCostProfitCenter.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());
            ddlcompany.SelectedIndex = ddlcompany.Items.IndexOf(ddlcompany.Items.FindByText(company));
           // txtCategory.Text = Server.HtmlDecode(gvCategory.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());
            string center = Server.HtmlDecode(gvCostProfitCenter.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
            ddlCostProfit.SelectedIndex = ddlCostProfit.Items.IndexOf(ddlCostProfit.Items.FindByText(center));

            txtcode.Text = Server.HtmlDecode(gvCostProfitCenter.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());
            txtName.Text = Server.HtmlDecode(gvCostProfitCenter.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());

            string status = Server.HtmlDecode(gvCostProfitCenter.Rows[SelectedIndex].Cells[4].Text.ToString().Trim());
            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));

            Session["sesstionCode"] = txtcode.Text.Trim();
        }

        protected void gvCostProfitCenter_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CostProfitCenterDataHandler oCostProfit = new CostProfitCenterDataHandler();
            gvCostProfitCenter.PageIndex = e.NewPageIndex;
            gvCostProfitCenter.DataSource = oCostProfit.GetGidData();
            gvCostProfitCenter.DataBind();
            oCostProfit = null;
        }

        protected void gvCostProfitCenter_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvCostProfitCenter, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        private void costProfitCenter()
        {
            DataTable costProfitBucket = new DataTable();

            costProfitBucket.Columns.Add("COMP_COST_PROFIT_CENTER_CODE", typeof(string));
            costProfitBucket.Columns.Add("COST_PROFIT_CENTER_NAME", typeof(string));
            costProfitBucket.Columns.Add("IS_PROFIT_CENTER", typeof(string));
            costProfitBucket.Columns.Add("COMPANY_ID", typeof(string));
            costProfitBucket.Columns.Add("STATUS_CODE", typeof(string));
            costProfitBucket.Columns.Add("IS_EXCLUDE", typeof(string));

            costProfitBucket.PrimaryKey = new[] { costProfitBucket.Columns["COMP_COST_PROFIT_CENTER_CODE"] };

            Session["dataBucket"] = costProfitBucket;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            string company = ddlcompany.SelectedValue.ToString();
            string cpCenter = ddlCostProfit.SelectedValue.ToString();
            string code = txtcode.Text.Trim();
            string name = txtName.Text.Trim();
            string status = ddlStatus.SelectedValue.ToString();
            string logUser = Session["KeyUSER_ID"].ToString();

            btnSave.Enabled = true;
            btnClear.Enabled = true;

            try
            {
                DataTable dtTable = (DataTable)Session["dataBucket"];

                DataRow dr = dtTable.NewRow();
                dr["COMPANY_ID"] = company.Trim();
                dr["IS_PROFIT_CENTER"] = cpCenter.Trim();
                dr["COMP_COST_PROFIT_CENTER_CODE"] = code.Trim();
                dr["COST_PROFIT_CENTER_NAME"] = name.Trim();
                dr["STATUS_CODE"] = status.Trim();

                dtTable.Rows.Add(dr);
            }
            catch (Exception)
            {
                
                throw;
            }

            fillCostProfitCenterGrid();
        }

        public void fillCostProfitCenterGrid()
        {
            DataTable dtTable = new DataTable();
            dtTable = (DataTable)Session["dataBucket"];

            foreach (DataRow dr in dtTable.Rows)
            {
                string costProfit = dr["IS_PROFIT_CENTER"].ToString();

                if (costProfit == "1")
                {
                    dr["IS_PROFIT_CENTER"] = "Profit Center";//
                }
                else if (costProfit == "0")
                {
                    dr["IS_PROFIT_CENTER"] = "Cost Center";
                }
            }
            gvAddcostProfitCenter.DataSource = dtTable;
            gvAddcostProfitCenter.DataBind();
        }

        protected void btnAddclear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void cleardateBucket()
        {
            if (Session["dataBucket"] != null)
            {
                DataTable dtdates = (DataTable)Session["dataBucket"];
                dtdates.Rows.Clear();
            }
        }

        protected void EXCLUDE_OnCheckedChanged(object sender, EventArgs e)
        {
            int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
            CheckBox cb = (CheckBox)gvAddcostProfitCenter.Rows[selRowIndex].FindControl("EXCLUDE");

            DataTable dateBucket_ = new DataTable();
            int iIndex = 0;
            int iPageIndex = 0;

            iPageIndex = gvAddcostProfitCenter.PageIndex;

            iIndex = (gvAddcostProfitCenter.PageSize * iPageIndex) + selRowIndex;

            if (cb.Checked == true)
            {
                if (Session["dataBucket"] != null)
                {
                    dateBucket_ = (Session["dataBucket"] as DataTable).Copy();
                    dateBucket_.Rows[iIndex][5] = Constants.CON_ROSTER_EXCLUDE_YES;

                    Session["dataBucket"] = dateBucket_.Copy();

                    gvAddcostProfitCenter.DataSource = dateBucket_;
                    gvAddcostProfitCenter.DataBind();

                }
            }
            else if (cb.Checked == false)
            {
                if (Session["dateBucket"] != null)
                {
                    dateBucket_ = (Session["dateBucket"] as DataTable).Copy();
                    dateBucket_.Rows[selRowIndex][5] = Constants.CON_ROSTER_EXCLUDE_NO;

                    Session["dateBucket"] = dateBucket_.Copy();

                    gvAddcostProfitCenter.DataSource = dateBucket_;
                    gvAddcostProfitCenter.DataBind();
                }
            }
        }

        protected void ddlcompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            //get cost frofit center according to company
            CostProfitCenterDataHandler oCostProfitCenterDataHandler = new CostProfitCenterDataHandler();
            gvCostProfitCenter.DataSource = null;

            gvCostProfitCenter.DataSource = oCostProfitCenterDataHandler.GetCostProfitCenterByCompany(ddlcompany.SelectedValue);
            gvCostProfitCenter.DataBind();

        }

    }
}