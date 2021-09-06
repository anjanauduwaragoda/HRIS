using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.TrainingAndDevelopment;
using Common;
using GroupHRIS.Utility;
using System.Data;
using DataHandler.Utility;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmExpenceCategory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadGrodData();
                fillStatus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string logUser = Session["KeyUSER_ID"].ToString();
            string name = txtName.Text;
            string description = txtDescription.Text;
            string status = ddlStatus.SelectedValue;

            ExpenceCategorDataHandler ECDH = new ExpenceCategorDataHandler();
            UtilsDataHandler UDH = new UtilsDataHandler();

            try
            {
                Errorhandler.ClearError(lblMessage);

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    bool isExist = UDH.isDuplicateExist(name, "CATEGORY_NAME", "EXPENSE_CATEGORY");
                    if (isExist)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Expense name already exist.", lblMessage);
                        return;
                    }

                    Boolean isSuccess = ECDH.Insert(name, description, status, logUser);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblMessage);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string categoryId = hfglId.Value.ToString();
                    bool isExist = UDH.isDuplicateExist(name, "CATEGORY_NAME", "EXPENSE_CATEGORY", categoryId, "EXPENSE_CATEGORY_ID");
                    if (isExist)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Expense name already exist.", lblMessage);
                        return;
                    }

                    Boolean isSuccess = ECDH.Update(categoryId, name, description, status, logUser);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
                }
                clear();
                loadGrodData();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            clear();
        }

        protected void grdExpenceCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            DataTable dt = new DataTable();

            try
            {
                int SelectedIndex = grdExpenceCategory.SelectedIndex;
                hfglId.Value = Server.HtmlDecode(grdExpenceCategory.Rows[SelectedIndex].Cells[0].Text.ToString());

                txtName.Text = Server.HtmlDecode(grdExpenceCategory.Rows[SelectedIndex].Cells[1].Text.ToString());

                txtDescription.Text = Server.HtmlDecode(grdExpenceCategory.Rows[SelectedIndex].Cells[2].Text.ToString());

                string status = Server.HtmlDecode(grdExpenceCategory.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdExpenceCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdExpenceCategory.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);

                loadGrodData();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdExpenceCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdExpenceCategory, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMessage);
            }
        }


        public void loadGrodData()
        {
            ExpenceCategorDataHandler ECDH = new ExpenceCategorDataHandler();
            try
            {
                grdExpenceCategory.DataSource = ECDH.getAllCategory();
                grdExpenceCategory.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        public void fillStatus()
        {
            try
            {
                ddlStatus.Items.Insert(0, new ListItem("", ""));
                ddlStatus.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                ddlStatus.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void clear()
        {
            
            txtName.Text = "";
            txtDescription.Text = "";
            ddlStatus.SelectedValue = "";
            hfglId.Value = "";
        }

    }
}