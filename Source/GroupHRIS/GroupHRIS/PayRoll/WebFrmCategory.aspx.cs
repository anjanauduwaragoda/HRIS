using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Payroll;
using Common;
using GroupHRIS.Utility;
using System.Globalization;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;

namespace GroupHRIS.PayRoll
{
    public partial class WebFrmCategory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    getStatus(ddlStstus);
                }

                Errorhandler.ClearError(lblMessage);
                loadGrid();
                txtCategory.Enabled = true;
            }
            catch (Exception exp)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, lblMessage);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string category = txtCategory.Text;
            string remarks = txtRemarks.Text;
            string StatusCode = ddlStstus.SelectedValue.ToString();
            string logUser = Session["KeyUSER_ID"].ToString();
            string chk = "0";

            if (chkOtherTransactions.Checked == true)
            {
                chk = "1";
            }

            if (txtCategory.Text != "")
            {
                try
                {
                    CategoryDataHandler oCategoryDataHandler = new CategoryDataHandler();

                    if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                    {

                        Boolean Status = oCategoryDataHandler.InsertCategory(category, remarks, StatusCode, logUser, chk);
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", lblMessage);
                    }
                    else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {
                        string categoryUpdate = Session["Category"].ToString();
                        Boolean Status = oCategoryDataHandler.UpdateCategory(categoryUpdate, category, remarks, StatusCode, logUser, chk);
                        
                        if (Status)
                        {
                            Errorhandler.GetError("1", "Successfully Updated", lblMessage);
                        }
                        else
                        {
                            Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, " Can't Update Inactive Category ", lblMessage);
                        }
                    }
                    loadGrid();
                    txtCategory.Enabled = true;
                }
                catch (Exception ex)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Record Already Exists ", lblMessage);
                }

            }
            ClearData();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            txtCategory.Text = "";
            txtRemarks.Text = "";
            ddlStstus.SelectedIndex = 0;
            chkOtherTransactions.Checked = false;

            txtRemarks.Enabled = true;
            chkOtherTransactions.Enabled = true;
        }

        protected void gvCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvCategory, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void gvCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CategoryDataHandler oCategoryDataHandler = new CategoryDataHandler();
            gvCategory.PageIndex = e.NewPageIndex;
            gvCategory.DataBind();
            oCategoryDataHandler = null;
        }

        protected void gvCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

            txtCategory.Text = "";
            txtRemarks.Text = "";
            int SelectedIndex = gvCategory.SelectedIndex;
            txtCategory.Text = Server.HtmlDecode(gvCategory.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());
            Session["Category"] = txtCategory.Text;
            txtCategory.Enabled = false;
            txtRemarks.Text = Server.HtmlDecode(gvCategory.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());

            string status = Server.HtmlDecode(gvCategory.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());
            ddlStstus.SelectedIndex = ddlStstus.Items.IndexOf(ddlStstus.Items.FindByText(status));

            string chk = Server.HtmlDecode(gvCategory.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
            if (chk == "Yes")
            {
                chkOtherTransactions.Checked = true;
            }
            else
            {
                chkOtherTransactions.Checked = false;
            }

            if (ddlStstus.SelectedIndex == 2)
            {
                txtRemarks.Enabled = false;
                chkOtherTransactions.Enabled = false;
            }
            else
            {
                txtRemarks.Enabled = true;
                chkOtherTransactions.Enabled = true;
            }

        }

        protected void txtCategory_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtCategory.Text, "^[a-zA-Z]"))
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "This textbox accepts only alphabetical characters", lblMessage);
                txtCategory.Text.Remove(txtCategory.Text.Length - 1);
            }
        }


        public void getStatus(DropDownList ddl)
        {
            ddl.Items.Insert(0, new ListItem("", ""));
            ddl.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
            ddl.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));

        }

        public void loadGrid()
        {
            CategoryDataHandler oCategoryDataHandler = new CategoryDataHandler();
            DataTable dt = new DataTable();
            try
            {
                dt = oCategoryDataHandler.GetCategories();
                gvCategory.DataSource = dt;
                gvCategory.DataBind();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                dt.Dispose();
                oCategoryDataHandler = null;
            }

        }

        public void ClearData()
        {
            txtCategory.Text = "";
            txtRemarks.Text = "";
            ddlStstus.SelectedIndex = 0;
            chkOtherTransactions.Checked = false;
            txtCategory.Enabled = true;
        }

    }
}