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
    public partial class WebFrmEmployeeType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Errorhandler.ClearError(StatusLabel);
                EmployeeTypeDataHandler oEmployeeTypeDataHandler = new EmployeeTypeDataHandler();
                SearchResultsGridView.DataSource = oEmployeeTypeDataHandler.GetEmployeeTypes();
                SearchResultsGridView.DataBind();

            }
            catch (Exception exp)
            {
                Errorhandler.GetError("0", exp.Message, StatusLabel);
            }
        }

        protected void ToggleButton_Click(object sender, EventArgs e)
        {
                           
                    Errorhandler.ClearError(StatusLabel);
                    EmployeeTypeDataHandler oEmployeeTypeDataHandler = new EmployeeTypeDataHandler();

                    if (ToggleButton.Text == Constants.CON_SAVE_BUTTON_TEXT)
                    {
                        Boolean State = oEmployeeTypeDataHandler.CheckPrevRecord(TypeNameTextBox.Text);
                        if (State == false)
                        {
                            Boolean Status = oEmployeeTypeDataHandler.InsertEmployeeType(TypeNameTextBox.Text, DescriptionTextBox.Text);
                            SearchResultsGridView.DataSource = oEmployeeTypeDataHandler.GetEmployeeTypes();
                            SearchResultsGridView.DataBind();
                            Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", StatusLabel);
                        }
                        else
                        {
                            Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Employee Type Name already exists", StatusLabel);
                        }
                    }
                    else if (ToggleButton.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {

                        try
                        {
                            string StatusCode = EmployeeTypeIDlbl.Text;
                            string Description = DescriptionTextBox.Text;
                            Boolean Status = oEmployeeTypeDataHandler.UpdateEmployeeType(TypeNameTextBox.Text, DescriptionTextBox.Text, EmployeeTypeIDlbl.Text);
                            SearchResultsGridView.DataSource = oEmployeeTypeDataHandler.GetEmployeeTypes();
                            SearchResultsGridView.DataBind();
                            Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Updated", StatusLabel);
                        }
                        catch (Exception)
                        {
                            Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Update Faliure", StatusLabel);
                        }

                        //UpdateButton.Visible = false;
                        ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;
                        EmployeeTypeIDlbl.Text = "";
                        DescriptionTextBox.Text = "";
                        TypeNameTextBox.Text = "";
                    }

            //UpdateButton.Visible = false;
            EmployeeTypeIDlbl.Text = "";
            DescriptionTextBox.Text = "";
            TypeNameTextBox.Text = "";
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);
            EmployeeTypeDataHandler oEmployeeTypeDataHandler = new EmployeeTypeDataHandler();

            Errorhandler.ClearError(StatusLabel);
            //UpdateButton.Visible = false;
            ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;
            EmployeeTypeIDlbl.Text = "";
            DescriptionTextBox.Text = "";
            TypeNameTextBox.Text = "";
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

        protected void SearchResultsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Errorhandler.ClearError(StatusLabel);
                //UpdateButton.Visible = true;
                ToggleButton.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                EmployeeTypeIDlbl.Text = "";
                DescriptionTextBox.Text = "";
                TypeNameTextBox.Text = "";

                int SelectedIndex = SearchResultsGridView.SelectedIndex;

                EmployeeTypeIDlbl.Text = WebUtility.HtmlDecode(SearchResultsGridView.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());
                TypeNameTextBox.Text = WebUtility.HtmlDecode(SearchResultsGridView.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
                DescriptionTextBox.Text = WebUtility.HtmlDecode(SearchResultsGridView.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void SearchResultsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            EmployeeTypeDataHandler oEmployeeTypeDataHandler = new EmployeeTypeDataHandler();
            SearchResultsGridView.DataSource = oEmployeeTypeDataHandler.GetEmployeeTypes();
            SearchResultsGridView.PageIndex = e.NewPageIndex;
            SearchResultsGridView.DataBind();
        }

    }
}