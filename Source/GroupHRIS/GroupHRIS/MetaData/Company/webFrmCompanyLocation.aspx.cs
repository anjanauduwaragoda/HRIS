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
    public partial class webFrmCompanyLocation : System.Web.UI.Page
    {
        CompanyLocationDataHandler oCompanyLocationDataHandler;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                oCompanyLocationDataHandler = new CompanyLocationDataHandler();
                DataTable dataTable = new DataTable();

                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        dataTable = oCompanyLocationDataHandler.PopulateCompany().Copy();

                        //SearchResultsGridView.DataSource = oCompanyLocationDataHandler.Populate().Copy();
                        //SearchResultsGridView.DataBind();
                    }
                    else
                    {
                        dataTable = oCompanyLocationDataHandler.PopulateCompany(Session["KeyCOMP_ID"].ToString().Trim()).Copy();

                        SearchResultsGridView.DataSource = oCompanyLocationDataHandler.Populate(Session["KeyCOMP_ID"].ToString().Trim()).Copy();
                        SearchResultsGridView.DataBind();
                    }
                }

                CompanyDropDownList.Items.Insert(0, new ListItem("", ""));
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string Value = dataTable.Rows[i]["COMPANY_ID"].ToString();
                    string Text = dataTable.Rows[i]["COMP_NAME"].ToString();
                    CompanyDropDownList.Items.Insert(i + 1, new ListItem(Text, Value));
                }

                FillDropDown();

            }
        }

        protected void ToggleButton_Click(object sender, EventArgs e)
        {
            string CompanyID = CompanyDropDownList.SelectedValue.ToString();
            string Location = LocationTextBox.Text;
            string Address = AddressTextBox.Text;
            string ContactNumber = ContactNumberTextBox.Text;
            string Remarks = RemarksTextBox.Text;
            string Status = StatusDropDownList.SelectedValue.ToString();

            Utility.Errorhandler.ClearError(StatusLabel);

            if (ToggleButton.Text == Constants.CON_SAVE_BUTTON_TEXT)
            {
                try
                {

                    oCompanyLocationDataHandler = new CompanyLocationDataHandler();

                    string locationName = LocationTextBox.Text.Trim().ToUpper().Replace(" ", "");

                    bool locationNameExist = oCompanyLocationDataHandler.isCompanyLocationExist(CompanyID,locationName);

                    if (locationNameExist)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Location name already exist.", StatusLabel);
                        return;
                    }


                    if (oCompanyLocationDataHandler.isCompanyLocationExist(CompanyDropDownList.SelectedValue.Trim(), LocationTextBox.Text.Trim()) == false)
                    {
                        oCompanyLocationDataHandler.Insert(Location, Address, ContactNumber, Remarks, CompanyID, Status);

                        ReloadGrid();
                        ClearInputFields();

                        CommonVariables.MESSAGE_TEXT = "Record saved successfully.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, StatusLabel);

                    }
                    else
                    {
                        Errorhandler.GetError("2", "Record Already Exists", StatusLabel);
                    }
                }
                catch (Exception ex)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, StatusLabel);
                }

            }
            else if (ToggleButton.Text == Constants.CON_UPDATE_BUTTON_TEXT)
            {
                try
                {
                    string LocationID = LocationIDLabel.Text;
                    oCompanyLocationDataHandler = new CompanyLocationDataHandler();
                    oCompanyLocationDataHandler.Update(Location, Address, ContactNumber, Remarks, CompanyID, LocationID, Status);

                    ReloadGrid();
                    ClearInputFields();

                    CommonVariables.MESSAGE_TEXT = "Record modified successfully.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, StatusLabel);
                }
                catch (Exception ex)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, StatusLabel);
                }

            }

            ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;
            ClearInputFields();
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(StatusLabel);
            ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;
            ClearInputFields();
            SearchResultsGridView.DataSource = null;
            SearchResultsGridView.DataBind();
        }

        private void ClearInputFields()
        {
            CompanyDropDownList.SelectedIndex = 0;
            LocationTextBox.Text = "";
            StatusDropDownList.SelectedIndex = 0;
            AddressTextBox.Text = "";
            ContactNumberTextBox.Text = "";
            RemarksTextBox.Text = "";
            LocationIDLabel.Text = "";

        }

        protected void SearchResultsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            oCompanyLocationDataHandler = new CompanyLocationDataHandler();

            if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
            {
                if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                {
                    SearchResultsGridView.PageIndex = e.NewPageIndex;
                    SearchResultsGridView.DataSource = oCompanyLocationDataHandler.Populate().Copy();
                    SearchResultsGridView.DataBind();
                }
                else
                {
                    SearchResultsGridView.PageIndex = e.NewPageIndex;
                    SearchResultsGridView.DataSource = oCompanyLocationDataHandler.Populate(Session["KeyCOMP_ID"].ToString().Trim()).Copy();
                    SearchResultsGridView.DataBind();
                }
            }
        }

        protected void SearchResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.SearchResultsGridView, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, StatusLabel);
            }
        }

        protected void SearchResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleButton.Text = Constants.CON_UPDATE_BUTTON_TEXT;

            string LocationID = Server.HtmlDecode(SearchResultsGridView.Rows[SearchResultsGridView.SelectedIndex].Cells[0].Text);
            string LocationName = Server.HtmlDecode(SearchResultsGridView.Rows[SearchResultsGridView.SelectedIndex].Cells[1].Text);
            string Address = Server.HtmlDecode(SearchResultsGridView.Rows[SearchResultsGridView.SelectedIndex].Cells[2].Text);
            string ContactNumber = Server.HtmlDecode(SearchResultsGridView.Rows[SearchResultsGridView.SelectedIndex].Cells[3].Text);
            string Remarks = Server.HtmlDecode(SearchResultsGridView.Rows[SearchResultsGridView.SelectedIndex].Cells[4].Text);
            string CompanyName = Server.HtmlDecode(SearchResultsGridView.Rows[SearchResultsGridView.SelectedIndex].Cells[5].Text);
            string Status = Server.HtmlDecode(SearchResultsGridView.Rows[SearchResultsGridView.SelectedIndex].Cells[6].Text);
            if (Status == Constants.STATUS_ACTIVE_TAG)
            {
                StatusDropDownList.SelectedValue = Constants.STATUS_ACTIVE_VALUE;
            }
            else if (Status == Constants.STATUS_INACTIVE_TAG)
            {
                StatusDropDownList.SelectedValue = Constants.STATUS_INACTIVE_VALUE;
            }

            LocationIDLabel.Text = LocationID;
            CompanyDropDownList.SelectedIndex = CompanyDropDownList.Items.IndexOf(CompanyDropDownList.Items.FindByText(CompanyName));
            LocationTextBox.Text = LocationName;
            AddressTextBox.Text = Address;
            ContactNumberTextBox.Text = ContactNumber;
            RemarksTextBox.Text = Remarks;
            Utility.Errorhandler.ClearError(StatusLabel);

        }

        private void ReloadGrid()
        {
            oCompanyLocationDataHandler = new CompanyLocationDataHandler();
            string CompanyId = CompanyDropDownList.SelectedValue.ToString();
            if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
            {
                if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                {
                    SearchResultsGridView.DataSource = oCompanyLocationDataHandler.Populate(CompanyId).Copy();
                    SearchResultsGridView.DataBind();
                }
                else
                {
                    SearchResultsGridView.DataSource = oCompanyLocationDataHandler.Populate(Session["KeyCOMP_ID"].ToString().Trim()).Copy();
                    SearchResultsGridView.DataBind();
                }
            }
        }

        protected void CompanyDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(StatusLabel);
            if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
            {
                string CompanyID = CompanyDropDownList.SelectedValue.ToString();
                DataTable dataTable = new DataTable();
                oCompanyLocationDataHandler = new CompanyLocationDataHandler();
                dataTable = oCompanyLocationDataHandler.Populate(CompanyID).Copy();
                SearchResultsGridView.DataSource = dataTable;
                SearchResultsGridView.DataBind();
            }
            Utility.Errorhandler.ClearError(StatusLabel);
            int SectedIndex = CompanyDropDownList.SelectedIndex;
            ClearInputFields();
            CompanyDropDownList.SelectedIndex = SectedIndex;
        }

        private void FillDropDown()
        {
            StatusDropDownList.Items.Insert(0, new ListItem("", ""));
            StatusDropDownList.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.STATUS_ACTIVE_VALUE));
            StatusDropDownList.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.STATUS_INACTIVE_VALUE));
        }
    }
}