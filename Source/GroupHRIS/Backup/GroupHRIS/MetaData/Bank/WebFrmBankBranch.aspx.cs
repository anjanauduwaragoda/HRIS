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

namespace GroupHRIS.MetaData.Bank
{
    public partial class WebFrmBankBranch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Errorhandler.ClearError(StatusLabel);
                //BankBranchDataHandler oBankBranchDataHandler = new BankBranchDataHandler();
                //SearchResultsGridView.DataSource = oBankBranchDataHandler.GetBankBranches();
                //SearchResultsGridView.DataBind();

                if (!IsPostBack)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    BankDropDownList.Items.Add(Item);
                    BankDataHandler oBankDataHandler = new BankDataHandler();

                    DataTable Banks = new DataTable();
                    Banks = oBankDataHandler.GetBanks();

                    foreach (DataRow dataRow in Banks.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["BANK_NAME"].ToString();
                        listItem.Value = dataRow["BANK_ID"].ToString();

                        BankDropDownList.Items.Add(listItem);
                    }
                }

            }
            catch (Exception exp)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, StatusLabel);
            }
        }

        protected void SearchResultsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BankBranchDataHandler oBankBranchDataHandler = new BankBranchDataHandler();
            SearchResultsGridView.PageIndex = e.NewPageIndex;
            SearchResultsGridView.DataSource = oBankBranchDataHandler.SearchBankBranch();
            SearchResultsGridView.DataBind();
            oBankBranchDataHandler = null;            
        }

        protected void ToggleButton_Click(object sender, EventArgs e)
        {
            BranchIDTextBox.Enabled = true;
                try
                {
                    Errorhandler.ClearError(StatusLabel);
                    BankBranchDataHandler oBankBranchDataHandler = new BankBranchDataHandler();

                    bool isExistBranch = oBankBranchDataHandler.isBanBranchkExist(BankDropDownList.SelectedValue, BranchTextBox.Text.ToUpper().Replace(" ",""));
                    if (isExistBranch)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Branch name for selected company is Exists", StatusLabel);
                        return;
                    }

                    if (ToggleButton.Text == Constants.CON_SAVE_BUTTON_TEXT)
                    {

                        Boolean CheckRecord = oBankBranchDataHandler.CheckRecords(BankDropDownList.SelectedItem.Text, BranchIDTextBox.Text);
                        if (CheckRecord == false)
                        {
                            Boolean Status = oBankBranchDataHandler.InsertBankBranch(BranchIDTextBox.Text, BankDropDownList.SelectedValue, BranchTextBox.Text, AddressTextBox.Text, LandPhone1TextBox.Text, LandPhone2TextBox.Text);
                            string BankID = BankDropDownList.SelectedValue.ToString();
                            SearchResultsGridView.DataSource = oBankBranchDataHandler.GetBankBranches(BankID);
                            SearchResultsGridView.DataBind();
                            //BankDropDownList.SelectedIndex = BankDropDownList.Items.IndexOf(BankDropDownList.Items.FindByText(""));
                            Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", StatusLabel);
                        }
                        else
                        {
                            Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Record Already Exists", StatusLabel);
                        }
                    }
                    else if (ToggleButton.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {
                        Errorhandler.ClearError(StatusLabel);

                        try
                        {
                            Boolean Status = oBankBranchDataHandler.UpdateBankBranch(BankDropDownList.SelectedValue.ToString(), BranchTextBox.Text, AddressTextBox.Text, LandPhone1TextBox.Text, LandPhone2TextBox.Text, BranchIDTextBox.Text);
                            string BankID = BankDropDownList.SelectedValue.ToString();
                            SearchResultsGridView.DataSource = oBankBranchDataHandler.GetBankBranches(BankID);
                            SearchResultsGridView.DataBind();
                            //BankDropDownList.SelectedIndex = BankDropDownList.Items.IndexOf(BankDropDownList.Items.FindByText(""));
                            Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Updated", StatusLabel);
                        }
                        catch (Exception exp)
                        {
                            StatusLabel.Text = exp.Message;
                            Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Update Faliure", StatusLabel);
                        }

                        BankDropDownList.Enabled = false;
                        //BranchIDTextBox.ReadOnly = true;
                        //UpdateButton.Visible = false;
                        ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;
                        BranchTextBox.Text = AddressTextBox.Text = LandPhone1TextBox.Text = LandPhone2TextBox.Text = BranchIDTextBox.Text = "";
                        BankDropDownList.Enabled = true;
                        //BranchIDTextBox.ReadOnly = false;
                        BankDropDownList.SelectedIndex = BankDropDownList.Items.IndexOf(BankDropDownList.Items.FindByText(""));
                    }

                }
                catch (Exception exp)
                {
                    throw exp;
                }

            BankDropDownList.Enabled = true;
            //BranchIDTextBox.ReadOnly = false;
            //UpdateButton.Visible = false;
            BranchTextBox.Text = BranchIDTextBox.Text = AddressTextBox.Text = LandPhone1TextBox.Text = LandPhone2TextBox.Text = "";
            BankDropDownList.SelectedIndex = BankDropDownList.Items.IndexOf(BankDropDownList.Items.FindByText(""));
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            BranchIDTextBox.Enabled = true;
            Errorhandler.ClearError(StatusLabel);
            BankBranchDataHandler oBankBranchDataHandler = new BankBranchDataHandler();

            //UpdateButton.Visible = false;
            ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;
            BranchTextBox.Text = AddressTextBox.Text = LandPhone1TextBox.Text = LandPhone2TextBox.Text = BranchIDTextBox.Text = "";
            BankDropDownList.Enabled = true;
            BankDropDownList.SelectedValue = "";
            SearchResultsGridView.DataSource = null;
            SearchResultsGridView.DataBind();

        }

        protected void SearchResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);

            BankDropDownList.Enabled = true;
            //BranchIDTextBox.ReadOnly = false;
            //UpdateButton.Visible = true;
            ToggleButton.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            int SelectedIndex = SearchResultsGridView.SelectedIndex;
            BranchTextBox.Text = AddressTextBox.Text = BranchIDTextBox.Text = LandPhone1TextBox.Text = LandPhone2TextBox.Text = "";

            BranchIDTextBox.Text = Server.HtmlDecode(SearchResultsGridView.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());

            string BankId = Server.HtmlDecode(SearchResultsGridView.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());

            BankDropDownList.SelectedIndex = BankDropDownList.Items.IndexOf(BankDropDownList.Items.FindByText(BankId));


            BranchTextBox.Text = Server.HtmlDecode(SearchResultsGridView.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
            AddressTextBox.Text = Server.HtmlDecode(SearchResultsGridView.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
            LandPhone1TextBox.Text = Server.HtmlDecode(SearchResultsGridView.Rows[SelectedIndex].Cells[4].Text.ToString().Trim());
            LandPhone2TextBox.Text = Server.HtmlDecode(SearchResultsGridView.Rows[SelectedIndex].Cells[5].Text.ToString().Trim());
            BankDropDownList.Enabled = false;
            BranchIDTextBox.Enabled = false;
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

        protected void BranchIDHiddenField_ValueChanged(object sender, EventArgs e)
        {

        }

        protected void BankDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string BankID = BankDropDownList.SelectedValue.ToString();
            BankBranchDataHandler oBankBranchDataHandler = new BankBranchDataHandler();
            SearchResultsGridView.DataSource = oBankBranchDataHandler.GetBankBranches(BankID);
            SearchResultsGridView.DataBind();
        }
    }
}