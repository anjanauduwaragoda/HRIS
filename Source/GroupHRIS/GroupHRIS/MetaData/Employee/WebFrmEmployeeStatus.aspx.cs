using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.MetaData;
using GroupHRIS.Utility;
using Common;

namespace GroupHRIS.MetaData.Employee
{
    public partial class WebFrmEmployeeStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Errorhandler.ClearError(StatusLabel);
                EmployeeStatusDataHandler oEmployeeStatusDataHandler = new EmployeeStatusDataHandler();
                SearchResultsGridView.DataSource = oEmployeeStatusDataHandler.GetEmployeeStatus();
                SearchResultsGridView.DataBind();
            }
            catch (Exception exp)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, StatusLabel);
            }

        }

        protected void ToggleButton_Click(object sender, EventArgs e)
        {

            try
            {
                Errorhandler.ClearError(StatusLabel);
                EmployeeStatusDataHandler oEmployeeStatusDataHandler = new EmployeeStatusDataHandler();

                if (ToggleButton.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    string Description = StatusNameTextBox.Text;
                    Boolean Status = oEmployeeStatusDataHandler.InsertEmployeeStatus(Description);
                    SearchResultsGridView.DataSource = oEmployeeStatusDataHandler.GetEmployeeStatus();
                    SearchResultsGridView.DataBind();
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", StatusLabel);
                }
                else if (ToggleButton.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {

                    try
                    {
                        string StatusCode = StatusCodeLabel.Text;
                        string Description = StatusNameTextBox.Text;
                        Boolean Status = oEmployeeStatusDataHandler.UpdateEmployeeStatus(Description, StatusCode);
                        SearchResultsGridView.DataSource = oEmployeeStatusDataHandler.GetEmployeeStatus();
                        SearchResultsGridView.DataBind();
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Updated", StatusLabel);
                    }
                    catch (Exception)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Update Faliure", StatusLabel);
                    }

                    //UpdateButton.Visible = false;
                    ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;
                    StatusCodeLabel.Text = "";
                    StatusNameTextBox.Text = "";
                }
            }
            catch (Exception exp)
            {
                StatusLabel.Text = exp.Message;
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Record already exists", StatusLabel);
            }

            //UpdateButton.Visible = false;
            StatusCodeLabel.Text = "";
            StatusNameTextBox.Text = "";
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);
            EmployeeStatusDataHandler oEmployeeStatusDataHandler = new EmployeeStatusDataHandler();
            Errorhandler.ClearError(StatusLabel);
            //UpdateButton.Visible = false;
            ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;
            StatusCodeLabel.Text = "";
            StatusNameTextBox.Text = "";
        }

        protected void SearchResultsGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);
            //UpdateButton.Visible = true;
            ToggleButton.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            StatusCodeLabel.Text = "";
            StatusNameTextBox.Text = "";
            StatusCodeLabel.Text = SearchResultsGridView.Rows[e.NewSelectedIndex].Cells[0].Text.ToString().Trim();
            StatusNameTextBox.Text = SearchResultsGridView.Rows[e.NewSelectedIndex].Cells[1].Text.ToString().Trim();
        }

        protected void SearchResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);
            //UpdateButton.Visible = true;
            ToggleButton.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            StatusCodeLabel.Text = "";
            StatusNameTextBox.Text = "";
            int SelectedIndex = SearchResultsGridView.SelectedIndex;
            StatusCodeLabel.Text = SearchResultsGridView.Rows[SelectedIndex].Cells[0].Text.ToString().Trim();
            StatusNameTextBox.Text = SearchResultsGridView.Rows[SelectedIndex].Cells[1].Text.ToString().Trim();
        }

        protected void SearchResultsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.SearchResultsGridView, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void SearchResultsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            EmployeeStatusDataHandler oEmployeeStatusDataHandler = new EmployeeStatusDataHandler();
            SearchResultsGridView.DataSource = oEmployeeStatusDataHandler.GetEmployeeStatus();
            SearchResultsGridView.PageIndex = e.NewPageIndex;
            SearchResultsGridView.DataBind();
        }
    }
}