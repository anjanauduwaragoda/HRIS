using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.MetaData;
using Common;
using GroupHRIS.Utility;

namespace GroupHRIS.MetaData.Bank
{
    public partial class WebFrmBank : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                Errorhandler.ClearError(StatusLabel);
                BankDataHandler oBankDataHandler = new BankDataHandler();
                SearchResultsGridView.DataSource = oBankDataHandler.GetBanks();
                SearchResultsGridView.DataBind();
            }
            catch (Exception exp)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, StatusLabel);
            }
        }

        protected void ToggleButton_Click(object sender, EventArgs e)
        {
            BankDataHandler oBankDataHandler = new BankDataHandler();

            if ((BankIDTextBox.Text != "") && (BankNameTextBox.Text != ""))
            {
                string bankName = BankNameTextBox.Text.Trim().ToUpper().Replace(" ","");
                
                bool bankNameExist = oBankDataHandler.isBankExist(bankName);

                if (bankNameExist)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Bank name already exist.", StatusLabel);
                    return;
                }

                try
                {
                    Errorhandler.ClearError(StatusLabel);

                    if (ToggleButton.Text == Constants.CON_SAVE_BUTTON_TEXT)
                    {
                        Boolean Status = oBankDataHandler.InsertBank(BankIDTextBox.Text, BankNameTextBox.Text);
                        SearchResultsGridView.DataSource = oBankDataHandler.GetBanks();
                        SearchResultsGridView.DataBind();
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", StatusLabel);
                    }
                    else if (ToggleButton.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {
                        try
                        {
                            string StatusCode = BankIDTextBox.Text;
                            string Description = BankNameTextBox.Text;
                            Boolean Status = oBankDataHandler.UpdateBank(BankNameTextBox.Text, BankIDTextBox.Text);
                            SearchResultsGridView.DataSource = oBankDataHandler.GetBanks();
                            SearchResultsGridView.DataBind();
                            Errorhandler.GetError("1", "Successfully Updated", StatusLabel);
                        }
                        catch (Exception)
                        {
                            Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Update Faliure", StatusLabel);
                        }

                        BankIDTextBox.ReadOnly = false;
                        ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;
                        BankIDTextBox.Text = "";
                        BankNameTextBox.Text = "";
                    }

                }
                catch (Exception exp)
                {
                    StatusLabel.Text = exp.Message;
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Record Already Exists", StatusLabel);
                }
            }
            else
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Bank ID and Bank Name cannot be blank", StatusLabel);
            }
            BankIDTextBox.ReadOnly = false;
            BankIDTextBox.Text = "";
            BankNameTextBox.Text = "";
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);
            BankDataHandler oBankDataHandler = new BankDataHandler();

            Errorhandler.ClearError(StatusLabel);
            BankIDTextBox.ReadOnly = false;
            ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;
            BankIDTextBox.Text = "";
            BankNameTextBox.Text = "";
            BankIDTextBox.ReadOnly = false;
            //BankIDTextBox.BorderStyle = BorderStyle.NotSet;
        }

        protected void SearchResultsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BankDataHandler oBankDataHandler = new BankDataHandler();
            SearchResultsGridView.PageIndex = e.NewPageIndex;
            SearchResultsGridView.DataSource = oBankDataHandler.SearchBankByBankIDOrBankName("");
            SearchResultsGridView.DataBind();
            oBankDataHandler = null;
        }

        protected void SearchResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);

            //BankIDTextBox.ReadOnly = true;
            ToggleButton.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            BankIDTextBox.Text = "";
            BankNameTextBox.Text = "";
            int SelecttedIndex = SearchResultsGridView.SelectedIndex;
            BankIDTextBox.Text = Server.HtmlDecode(SearchResultsGridView.Rows[SelecttedIndex].Cells[0].Text.ToString().Trim());
            BankNameTextBox.Text = Server.HtmlDecode(SearchResultsGridView.Rows[SelecttedIndex].Cells[1].Text.ToString().Trim());
            //BankIDTextBox.BorderStyle = BorderStyle.None;
            BankIDTextBox.ReadOnly = true;
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
    }
}