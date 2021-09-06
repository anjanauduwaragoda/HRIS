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
    public partial class Webfrmprivilsubscheme : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GeMainNodes();
                fillsubNodes();
            }
        }


        private void GeMainNodes()
        {
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();
            DataTable privischeme = new DataTable();

            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlMainnodes.Items.Add(listItemBlank);

                privischeme = UserAdministration.populateuserMainNodes();
                for (int x = 0; x < privischeme.Rows.Count  ; x++ )
                {
                    ddlMainnodes.Items.Add(privischeme.Rows[x][0].ToString() + "-" + privischeme.Rows[x][1].ToString());
                }
                GridView1.DataSource = privischeme;
                GridView1.DataBind();
                if (privischeme.Rows.Count > 1)
                {
                    GridView1.HeaderRow.Cells[0].Text = "Code";
                    GridView1.HeaderRow.Cells[1].Text = "Remarks";
                    GridView1.HeaderRow.Cells[2].Text = "Re-Direct Url";
                    GridView1.HeaderRow.Cells[0].Width = 100;
                    GridView1.HeaderRow.Cells[1].Width = 350;
                    GridView1.HeaderRow.Cells[2].Width = 350;
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


        private void fillsubNodes()
        {
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();
            DataTable privisubscheme = new DataTable();
            string mMainNode = ddlMainnodes.SelectedItem.Text.ToString();
            try
            {
                if (mMainNode != "")
                {
                    privisubscheme = UserAdministration.populateuserSubNodes(ddlMainnodes.SelectedItem.Text.Substring(0, 8).ToString());
                    GridView1.DataSource = privisubscheme;
                    GridView1.DataBind();
                    if (privisubscheme.Rows.Count > 1)
                    {
                        GridView1.HeaderRow.Cells[0].Text = "Code";
                        GridView1.HeaderRow.Cells[1].Text = "Remarks";
                        GridView1.HeaderRow.Cells[2].Text = "Re-Direct Url";
                        GridView1.HeaderRow.Cells[0].Width = 100;
                        GridView1.HeaderRow.Cells[1].Width = 350;
                        GridView1.HeaderRow.Cells[2].Width = 350;
                    }
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
                privisubscheme.Dispose();
                privisubscheme = null;
            }

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            fillsubNodes();
        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();
            DataTable mPrivilagesub = new DataTable();

            try
            {
                string Mmainnode = ddlMainnodes.SelectedItem.Text.ToString();
                string MsubNodeCode = lblsubnodecode.Text.ToString().Trim();
                string MsubNode = txtsubnode.Text.ToString().Trim();
                string redurl = txturl.Text.ToString().Trim();
                string madddate = DateTime.Today.ToString("yyyy/MM/dd");
                string mlogUser = (string)(Session["KeyUSER_ID"]);


                if (Mmainnode == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Privilege scheme is invalid";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (MsubNode == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Sub Privilege scheme is invalid";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (redurl == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Re-Direct Url is invalid";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {
                    Mmainnode = ddlMainnodes.SelectedItem.Text.Substring(0, 8).ToString();

                    if (hfsubprivilage.Value.ToUpper() == MsubNode.ToString().ToUpper())
                    {
                        UserAdministration.Insertsubnode(MsubNodeCode, Mmainnode, MsubNode, redurl, mlogUser, madddate, "A");
                        CommonVariables.MESSAGE_TEXT = "Sub Privilege scheme Successfully Updated";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        mPrivilagesub = UserAdministration.populateuserSubNodesName(MsubNode);
                        if (mPrivilagesub.Rows.Count > 0)
                        {
                            CommonVariables.MESSAGE_TEXT = "Sub Privilege scheme already exists";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {
                            UserAdministration.Insertsubnode(MsubNodeCode, Mmainnode, MsubNode, redurl, mlogUser, madddate, "A");
                            CommonVariables.MESSAGE_TEXT = "Sub Privilege scheme Successfully Updated";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                    }

                    fillsubNodes();

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
            }
        }

        protected void ddlMainnodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clearText();
                fillsubNodes();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clearText();
                lblsubnodecode.Text = GridView1.SelectedRow.Cells[0].Text;
                txtsubnode.Text = GridView1.SelectedRow.Cells[1].Text;
                hfsubprivilage.Value = GridView1.SelectedRow.Cells[1].Text;
                txturl.Text = GridView1.SelectedRow.Cells[2].Text;
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

        protected void btnclose_Click(object sender, EventArgs e)
        {
            clearText();
            btnupdate.Text = Constants.CON_SAVE_BUTTON_TEXT;

        }


        private void clearText()
        {
            txtsubnode.Text = "";
            hfsubprivilage.Value = "";
            txturl.Text = "";
            lblsubnodecode.Text = "";
            Utility.Errorhandler.ClearError(lblerror);
        }

    }
}