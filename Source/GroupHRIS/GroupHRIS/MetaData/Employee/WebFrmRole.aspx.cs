using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.MetaData;
using GroupHRIS.Utility;
using System.Net;
using Common;

namespace GroupHRIS.MetaData.Employee
{
    public partial class WebFrmRole : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Errorhandler.ClearError(StatusLabel);
                EmployeeRoleDataHandler oEmployeeRoleDataHandler = new EmployeeRoleDataHandler();
                SearchResultsGridView.DataSource = oEmployeeRoleDataHandler.GetEmployeeRoles();
                SearchResultsGridView.DataBind();
            }
            catch (Exception exp)
            {
                StatusLabel.Text = exp.Message;
            }

            if (!IsPostBack)
            {
                FillDropDown();
            }

        }

        protected void ToggleButton_Click(object sender, EventArgs e)
        {
            if (RoleNameTextBox.Text != "")
            {
                Errorhandler.ClearError(StatusLabel);
                EmployeeRoleDataHandler oEmployeeRoleDataHandler = new EmployeeRoleDataHandler();
                Boolean State = oEmployeeRoleDataHandler.CheckPrevRecord(RoleNameTextBox.Text);
                if (ToggleButton.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    if (State == false)
                    {
                        Boolean Status = oEmployeeRoleDataHandler.InsertEmployeeRole(RoleNameTextBox.Text, DescriptionTextBox.Text,StatusDropDownList.SelectedValue.ToString());
                        SearchResultsGridView.DataSource = oEmployeeRoleDataHandler.GetEmployeeRoles();
                        SearchResultsGridView.DataBind();
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", StatusLabel);
                    }
                    else
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Role Name already exists", StatusLabel);
                    }
                }
                else if (ToggleButton.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {

                    try
                    {
                        string StatusCode = EmployeeRoleIDLabel.Text;
                        string Description = DescriptionTextBox.Text;
                        Boolean Status = oEmployeeRoleDataHandler.UpdateEmployeeRole(RoleNameTextBox.Text, DescriptionTextBox.Text, EmployeeRoleIDLabel.Text, StatusDropDownList.SelectedValue.ToString());
                        SearchResultsGridView.DataSource = oEmployeeRoleDataHandler.GetEmployeeRoles();
                        SearchResultsGridView.DataBind();
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Updated", StatusLabel);
                    }
                    catch (Exception)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Update Faliure", StatusLabel);
                    }

                    //UpdateButton.Visible = false;
                    ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;
                    EmployeeRoleIDLabel.Text = "";
                    DescriptionTextBox.Text = "";
                    RoleNameTextBox.Text = "";
                }
            }
            else
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Role Name cannot be blank", StatusLabel);
            }

            //UpdateButton.Visible = false;
            EmployeeRoleIDLabel.Text = "";
            DescriptionTextBox.Text = "";
            RoleNameTextBox.Text = "";
            StatusDropDownList.SelectedIndex = 0;
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);
            EmployeeRoleDataHandler oEmployeeRoleDataHandler = new EmployeeRoleDataHandler();

            Errorhandler.ClearError(StatusLabel);
            //UpdateButton.Visible = false;
            ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;
            EmployeeRoleIDLabel.Text = "";
            DescriptionTextBox.Text = "";
            RoleNameTextBox.Text = "";
            StatusDropDownList.SelectedIndex = 0;
        }

        protected void SearchResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);
            //UpdateButton.Visible = true;
            ToggleButton.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            EmployeeRoleIDLabel.Text = "";
            DescriptionTextBox.Text = "";
            RoleNameTextBox.Text = "";

            int SelectedIndex = SearchResultsGridView.SelectedIndex;

            EmployeeRoleIDLabel.Text = WebUtility.HtmlDecode(SearchResultsGridView.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());
            RoleNameTextBox.Text = WebUtility.HtmlDecode(SearchResultsGridView.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
            DescriptionTextBox.Text = WebUtility.HtmlDecode(SearchResultsGridView.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());

            string Status = WebUtility.HtmlDecode(SearchResultsGridView.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
            if (Status == Constants.STATUS_ACTIVE_TAG)
            {
                StatusDropDownList.SelectedValue = Constants.STATUS_ACTIVE_VALUE;
            }
            else if (Status == Constants.STATUS_INACTIVE_TAG)
            {
                StatusDropDownList.SelectedValue = Constants.STATUS_INACTIVE_VALUE;
            }


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
            EmployeeRoleDataHandler oEmployeeRoleDataHandler = new EmployeeRoleDataHandler();
            SearchResultsGridView.PageIndex = e.NewPageIndex;
            SearchResultsGridView.DataSource = oEmployeeRoleDataHandler.GetEmployeeRoles();
            SearchResultsGridView.DataBind();
        }

        private void FillDropDown()
        {
            StatusDropDownList.Items.Insert(0, new ListItem("", ""));
            StatusDropDownList.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.STATUS_ACTIVE_VALUE));
            StatusDropDownList.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.STATUS_INACTIVE_VALUE));
        }
    }
}