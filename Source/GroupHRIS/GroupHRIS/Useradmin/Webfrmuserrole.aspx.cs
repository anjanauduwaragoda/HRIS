using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.UserAdministration;
using System.Data;
using Common;
using NLog;

namespace GroupHRIS.Useradmin
{
    public partial class Webfrmuserrole : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillroles();
                btnupdate.Text = Constants.CON_SAVE_BUTTON_TEXT;
            }
        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();
            DataTable userrole = new DataTable();

            try
            {
                string muserroledefault = ddlrolecategory.SelectedValue.ToString();
                string muserroledefaultName = ddlrolecategory.SelectedItem.Text.ToString().Trim().ToUpper();
                string muserroleCode = lblrefcode.Text.Trim().ToString();
                string muserroleStatus = ddlrolestatus.SelectedValue.ToString();
                string muserrole = txtuserrole.Text.Trim().ToString().Trim();
                string madddate = DateTime.Today.ToString("yyyy/MM/dd");
                string mlogUser = (string)(Session["KeyUSER_ID"]);

                if (muserrole == "")
                {
                    CommonVariables.MESSAGE_TEXT = "User Role / Remarks is blank ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (muserroledefaultName == "")
                {
                    CommonVariables.MESSAGE_TEXT = "User Role Category is blank ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (muserroleStatus == "")
                {
                    CommonVariables.MESSAGE_TEXT = "User Role Status is blank ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {
                    
                    if (muserroleCode.Equals(""))
                    {
                            userrole = UserAdministration.populateuserRolesName(muserrole);
                            if (userrole.Rows.Count > 0)
                            {
                                CommonVariables.MESSAGE_TEXT = "User role " + userrole.Rows[0][0].ToString() + " already exist.";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            }
                            else
                            {
                                UserAdministration.InsertUserRole(muserrole, mlogUser, madddate, muserroleStatus, muserroledefault);
                                fillroles();
                                CommonVariables.MESSAGE_TEXT = "User Role Successfully Saved.";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                            }
                    }
                    else
                    {
                        userrole = UserAdministration.populateuserRoles(muserroleCode);
                        if (userrole.Rows.Count > 0 && muserroleStatus == Constants.STATUS_INACTIVE_VALUE )
                        {
                            CommonVariables.MESSAGE_TEXT = "Can not Inactivate user role " + muserrole;
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {

                            if (hfuserrole.Value.ToUpper() == muserrole.ToUpper())
                            {
                                UserAdministration.UpdateUserRole(muserroleCode, muserrole, mlogUser, madddate, muserroleStatus, muserroledefault);
                                fillroles();
                                CommonVariables.MESSAGE_TEXT = "User Role Successfully Updated.";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                            }
                            else
                            {
                                userrole = UserAdministration.populateuserRolesName(muserrole);
                                if (userrole.Rows.Count > 0)
                                {
                                    CommonVariables.MESSAGE_TEXT = "User role " + userrole.Rows[0][0].ToString() + " already exist.";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                                }
                                else
                                {
                                    UserAdministration.UpdateUserRole(muserroleCode, muserrole, mlogUser, madddate, muserroleStatus, muserroledefault);
                                    fillroles();
                                    CommonVariables.MESSAGE_TEXT = "User Role Successfully Updated.";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);

                                }

                            }
                        }
                    }

                    fillroles();
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
                userrole.Dispose();
                userrole = null;
            }
        }

        private void cleartext()
        {
            lblrefcode.Text = "";
            Utility.Errorhandler.ClearError(lblerror);
            txtuserrole.Text = "";

        }
        

        private void fillroles()
        {
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();
            DataTable userrole = new DataTable();

            try
            {
                userrole = UserAdministration.populateuserRoles();
                GridView1.DataSource = userrole;
                GridView1.DataBind();

                if (userrole.Rows.Count >= 1)
                {

                GridView1.HeaderRow.Cells[0].Text = "Role Code";
                GridView1.HeaderRow.Cells[1].Text = "Role Remarks";
                GridView1.HeaderRow.Cells[2].Text = "Status";
                GridView1.HeaderRow.Cells[3].Text = "Role Categoty";
                GridView1.HeaderRow.Cells[0].Width = 100;
                GridView1.HeaderRow.Cells[1].Width = 300;
                GridView1.HeaderRow.Cells[2].Width = 150;
                GridView1.HeaderRow.Cells[3].Width = 200;
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
                userrole.Dispose();
                userrole = null;
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            fillroles();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cleartext();
                ddlrolecategory.ClearSelection();
                ddlrolestatus.ClearSelection();
                lblrefcode.Text = GridView1.SelectedRow.Cells[0].Text;
                txtuserrole.Text = GridView1.SelectedRow.Cells[1].Text;
                hfuserrole.Value = GridView1.SelectedRow.Cells[1].Text;

                if (GridView1.SelectedRow.Cells[2].Text.ToString().Trim().Replace("-", "") != "")
                {
                    ddlrolestatus.Items.FindByText(GridView1.SelectedRow.Cells[2].Text.ToString().Trim()).Selected = true;
                }
                if (GridView1.SelectedRow.Cells[3].Text.ToString().Trim().Replace("-", "") != "")
                {
                    ddlrolecategory.Items.FindByText(GridView1.SelectedRow.Cells[3].Text.ToString().Trim()).Selected = true;
                }
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
            cleartext();
            btnupdate.Text = Constants.CON_SAVE_BUTTON_TEXT;
            ddlrolestatus.SelectedIndex = 0;
            ddlrolecategory.SelectedIndex = 0;
            hfuserrole.Value = "";
        }
    }
}