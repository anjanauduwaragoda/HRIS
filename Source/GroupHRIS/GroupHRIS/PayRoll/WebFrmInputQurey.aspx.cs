using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Common;
using System.Web.UI.WebControls;
using GroupHRIS.Utility;
using DataHandler.Payroll;

namespace GroupHRIS.PayRoll
{
    public partial class WebFrmInputQurey : System.Web.UI.Page
    {

        InsertProcedure insertProcedure = new InsertProcedure();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvsprocedure.DataSource = insertProcedure.populate();
                gvsprocedure.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string spName = txtQuery.Text;
            string remarks = txtRemarks.Text;
            string logUser = Session["KeyUSER_ID"].ToString();

            try
            {
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    Boolean Status = insertProcedure.Insert(spName, remarks, logUser);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string spId = Session["spId"].ToString();
                    Boolean Status = insertProcedure.Update(spId, spName, remarks, logUser);
                }

                gvsprocedure.DataSource = insertProcedure.populate();
                gvsprocedure.DataBind();

                txtQuery.Text = "";
                txtRemarks.Text = "";
            }
            catch (Exception)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Record Already Exist", StatusLabel);
                
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Errorhandler.ClearError(StatusLabel);
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                txtQuery.Text = "";
                txtRemarks.Text = "";
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void gvsprocedure_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvsprocedure.PageIndex = e.NewPageIndex;
            gvsprocedure.DataSource = insertProcedure.populate();
            gvsprocedure.DataBind();
            insertProcedure = null;
        }

        protected void gvsprocedure_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvsprocedure, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, StatusLabel);
            }
        }

        protected void gvsprocedure_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

            int SelectedIndex = e.NewSelectedIndex;

            Session["spId"] = Server.HtmlDecode(gvsprocedure.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());

            txtQuery.Text = Server.HtmlDecode(gvsprocedure.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());

            txtRemarks.Text = Server.HtmlDecode(gvsprocedure.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());
        
        }

    }
}