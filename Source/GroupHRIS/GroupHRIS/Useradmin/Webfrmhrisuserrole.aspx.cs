using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.UserAdministration;
using DataHandler.MetaData;
using System.Data;
using Common;

namespace GroupHRIS.Useradmin
{
    public partial class Webfrmhrisuserrole : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetUserRoles();
                string KeyCOMP_ID = (string)(Session["KeyCOMP_ID"]);
                getCompID(KeyCOMP_ID);
                fillDepartment();
            }
        }

        private void GetUserRoles()
        {
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();
            DataTable userrole = new DataTable();

            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlUserrole.Items.Add(listItemBlank);

                userrole = UserAdministration.populateuserRoles();
                foreach (DataRow dataRow in userrole.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = dataRow["ROLE_NAME"].ToString();
                    listItem.Value = dataRow["ROLE_ID"].ToString();
                    ddlUserrole.Items.Add(listItem);
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
        protected void btnupdate_Click(object sender, EventArgs e)
        {
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();
            DataTable hrisuser = new DataTable();

            try
            {
                lblerror.ForeColor = System.Drawing.Color.Red;
                string mEmployeeId = lblemployeeid.Text.ToString().Trim().ToUpper();
                string mUserid = lbluserid.Text.ToString().Trim().ToUpper();
                string mROLE_ID = ddlUserrole.SelectedValue;
                string mdescription = txtdescription.Text.Trim().ToString();
                string madddate = DateTime.Today.ToString("yyyy/MM/dd");
                string mlogUser = (string)(Session["KeyUSER_ID"]);

                if (mEmployeeId == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Employee Not Selected ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (mdescription == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Remarks cannot be blank ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (mUserid == "")
                {
                    CommonVariables.MESSAGE_TEXT = "User ID cannot be Blank ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (mROLE_ID == "")
                {
                    CommonVariables.MESSAGE_TEXT = "User Role cannot be Blank ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {
                    
                    hrisuser = UserAdministration.populateHrisUser(mEmployeeId, Constants.STATUS_ACTIVE_VALUE);
                    if (hrisuser.Rows.Count >= 1)
                    {
                        UserAdministration.InsertHrisUserRole(mUserid, mROLE_ID, mdescription, Constants.STATUS_ACTIVE_VALUE, mlogUser, madddate);
                        CommonVariables.MESSAGE_TEXT = "User Role  Successfully Assigned To  " + mUserid + ".";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "User ID " + mUserid + " Not Found, Cannot Assign a User Role.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }

                    fillhrisuser();
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
                hrisuser.Dispose();
                hrisuser = null;
            }
        }

        private void fillhrisuser()
        {
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();
            DataTable hrisuser = new DataTable();

            try
            {
                string mEPFno = txtepfno.Text.ToString().Trim();
                string mCompCode = dpCompID.SelectedValue.ToString();
                string mDeptCode = dpDeptCode.SelectedValue.ToString();
                string mEmpNic = txtnicno.Text.ToString();
                string mLastName = txtlastname.Text.ToString();

                hrisuser = UserAdministration.populateHrisUserRole(mEPFno, mCompCode, mDeptCode, mEmpNic, mLastName);

                GridView1.DataSource = hrisuser;
                GridView1.DataBind();

                if (hrisuser.Rows.Count >= 1)
                {
                    GridView1.HeaderRow.Cells[0].Text = "Employee ID";
                    GridView1.HeaderRow.Cells[1].Text = "E.P.F. No";
                    GridView1.HeaderRow.Cells[2].Text = "User ID";
                    GridView1.HeaderRow.Cells[3].Text = "Employee Name";
                    GridView1.HeaderRow.Cells[4].Text = "User Role";
                    GridView1.HeaderRow.Cells[5].Text = "Remarks";
                    GridView1.HeaderRow.Cells[0].Width = 150;
                    GridView1.HeaderRow.Cells[1].Width = 100;
                    GridView1.HeaderRow.Cells[2].Width = 100;
                    GridView1.HeaderRow.Cells[3].Width = 200;
                    GridView1.HeaderRow.Cells[4].Width = 200;
                    GridView1.HeaderRow.Cells[5].Width = 300;
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
                hrisuser.Dispose();
                hrisuser = null;
            }

        }

        private void getCompID(string KeyCOMP_ID)
        {
            CompanyDataHandler companynameid = new CompanyDataHandler();
            DataTable CompID = new DataTable();
            try
            {

                if (KeyCOMP_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    CompID = companynameid.getCompanyIdCompName();
                    ListItem listItemBlank = new ListItem();
                    listItemBlank.Text = "";
                    listItemBlank.Value = "";
                    dpCompID.Items.Add(listItemBlank);
                }
                else
                {
                    CompID = companynameid.getCompanyIdCompName(KeyCOMP_ID);
                }

                foreach (DataRow dataRow in CompID.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = dataRow["COMP_NAME"].ToString();
                    listItem.Value = dataRow["COMPANY_ID"].ToString();
                    dpCompID.Items.Add(listItem);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                companynameid = null;
                CompID.Dispose();
                CompID = null;
            }
        }

        private void fillDepartment()
        {
            DepartmentDataHandler Department = new DepartmentDataHandler();
            DataTable DepScheme = new DataTable();
            try
            {
                dpDeptCode.Items.Clear();
                string mCompCode = dpCompID.SelectedValue.ToString();
                DepScheme = Department.populateByComId(mCompCode);

                ListItem Item = new ListItem();
                Item.Text = "";
                Item.Value = "";
                dpDeptCode.Items.Add(Item);

                foreach (DataRow dataRow in DepScheme.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = dataRow["DEPT_NAME"].ToString();
                    listItem.Value = dataRow["DEPT_ID"].ToString();
                    dpDeptCode.Items.Add(listItem);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                Department = null;
                DepScheme.Dispose();
                DepScheme = null;
            }
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            fillhrisuser();
        }

        protected void dpCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            clear_text();
            fillDepartment();
        }

        protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                clear_text();
                fillhrisuser();
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

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clear_text();
                btnupdate.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                ddlUserrole.ClearSelection();
                btnupdate.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                lblemployeeid.Text = GridView1.SelectedRow.Cells[0].Text;
                lblname.Text = GridView1.SelectedRow.Cells[3].Text;
                lbluserid.Text = GridView1.SelectedRow.Cells[2].Text;
                txtdescription.Text = GridView1.SelectedRow.Cells[5].Text.ToString().Replace("-", "");
                if (GridView1.SelectedRow.Cells[4].Text.ToString().Trim().Replace("-", "") != "")
                {
                    ddlUserrole.Items.FindByText(GridView1.SelectedRow.Cells[4].Text.ToString().Trim()).Selected = true;
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
            clear_text();
            btnupdate.Text = Constants.CON_SAVE_BUTTON_TEXT;

        }

        private void clear_text()
        {
            Utility.Errorhandler.ClearError(lblerror);
            txtdescription.Text = "";
            lblemployeeid.Text = "";
            lblname.Text = "";
            lbluserid.Text = "";
        }

    }
}