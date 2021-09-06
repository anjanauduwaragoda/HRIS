using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using GroupHRIS.Utility;
using DataHandler.MetaData;

namespace GroupHRIS.MetaData.Company
{
    public partial class webFrmCompanyPropertyCategory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Errorhandler.ClearError(lblMessage);
                PropertyCategoryDataHandler oPropertyCategoryDataHandler = new PropertyCategoryDataHandler();
                gvProperty.DataSource = oPropertyCategoryDataHandler.GetAllProperties();
                gvProperty.DataBind();

            }
            catch (Exception exp)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, lblMessage);
            }
            if (!IsPostBack)
            {
                ddlStstus.Items.Insert(0, new ListItem("", ""));
                ddlStstus.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                ddlStstus.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtProperty.Text;
            string status = ddlStstus.SelectedItem.ToString();
            string logUser = Session["KeyUSER_ID"].ToString();
            try
            {
                PropertyCategoryDataHandler oPropertyCategoryDataHandler = new PropertyCategoryDataHandler();

                if (txtProperty.Text != null)
                {
                    if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                    {
                        Boolean Status = oPropertyCategoryDataHandler.InsertProperty(name, status, logUser);
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", lblMessage);
                    }
                    else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {

                        string id = lblPropertyId.Text;
                        gvProperty.DataSource = oPropertyCategoryDataHandler.UpdateProperty(name,status,logUser,id);
                        gvProperty.DataBind();
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Updated", lblMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Record Alredy Exist", lblMessage);
                throw ex;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            txtProperty.Text = "";
            lblPropertyId.Text = "";
            ddlStstus.SelectedIndex = 0;
        }

        protected void gvProperty_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvProperty, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void gvProperty_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PropertyCategoryDataHandler oPropertyCategoryDataHandler = new PropertyCategoryDataHandler();
            gvProperty.PageIndex = e.NewPageIndex;
            gvProperty.DataBind();
            oPropertyCategoryDataHandler = null;
        }

        protected void gvProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

            int SelectedIndex = gvProperty.SelectedIndex;

            lblPropertyId.Text = Server.HtmlDecode(gvProperty.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());

            txtProperty.Text = Server.HtmlDecode(gvProperty.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());

            string status = Server.HtmlDecode(gvProperty.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());
            ddlStstus.SelectedIndex = ddlStstus.Items.IndexOf(ddlStstus.Items.FindByText(status));


        }

    }
}