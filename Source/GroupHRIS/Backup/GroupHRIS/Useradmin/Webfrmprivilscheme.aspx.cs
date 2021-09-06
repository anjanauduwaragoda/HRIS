using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.UserAdministration;
using System.Data;
using Common;

namespace GroupHRIS.Useradmin
{
    public partial class Webfrmprivilscheme : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillMainNodes();
            }
        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();
            DataTable mMainprivilages = new DataTable();

            try
            {
                string Mmainnode = txtmainnodes.Text.Trim().ToString();
                string madddate = DateTime.Today.ToString("yyyy/MM/dd");
                string mlogUser = (string)(Session["KeyUSER_ID"]);
                string mCode = lblnodecode.Text.ToString();

                if (Mmainnode == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Privilege scheme cannot be blank";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {

                    mMainprivilages = UserAdministration.populateuserMainNodes(Mmainnode);
                    if (mMainprivilages.Rows.Count > 0)
                    {
                        CommonVariables.MESSAGE_TEXT = "Privilege scheme already exists";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        if (mCode == "")
                        {
                            UserAdministration.Insertmainnode(Mmainnode, mlogUser, madddate, Constants.STATUS_ACTIVE_VALUE);
                            CommonVariables.MESSAGE_TEXT = "Privilege scheme successfully  Saved";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {
                            UserAdministration.Updatemainnode(mCode, Mmainnode, mlogUser, madddate);
                            CommonVariables.MESSAGE_TEXT = "Privilege scheme successfully Updated";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                    }
                    fillMainNodes();
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                UserAdministration = null;
                mMainprivilages.Dispose();
                mMainprivilages = null;
            }
        }

        private void fillMainNodes()
        {
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();
            DataTable privischeme = new DataTable();

            try
            {
                privischeme = UserAdministration.populateuserMainNodes();
                GridView1.DataSource = privischeme;
                GridView1.DataBind();

                if (privischeme.Rows.Count >= 1)
                {
                    GridView1.HeaderRow.Cells[0].Text = "Privilege Code ";
                    GridView1.HeaderRow.Cells[1].Text = "Privilege  Remarks";
                    GridView1.HeaderRow.Cells[2].Text = "User Entered";
                    GridView1.HeaderRow.Cells[3].Text = "Date Added";
                    GridView1.HeaderRow.Cells[0].Width = 100;
                    GridView1.HeaderRow.Cells[1].Width = 300;
                    GridView1.HeaderRow.Cells[2].Width = 150;
                    GridView1.HeaderRow.Cells[3].Width = 150;
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                UserAdministration = null;
                privischeme.Dispose();
                privischeme = null;
            }

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            fillMainNodes();
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            btnupdate.Text = Constants.CON_SAVE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblerror);
            txtmainnodes.Text = "";
            lblnodecode.Text = "";
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Utility.Errorhandler.ClearError(lblerror);
                lblnodecode.Text = GridView1.SelectedRow.Cells[0].Text;
                txtmainnodes.Text = GridView1.SelectedRow.Cells[1].Text;
                btnupdate.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.GridView1, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

    }
}