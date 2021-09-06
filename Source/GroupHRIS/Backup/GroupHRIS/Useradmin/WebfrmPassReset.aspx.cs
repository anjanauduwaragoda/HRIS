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
using DataHandler.Userlogin;

namespace GroupHRIS.Useradmin
{
    public partial class WebfrmPassReset : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string KeyCOMP_ID = (string)(Session["KeyCOMP_ID"]);
                getCompID(KeyCOMP_ID);
                fillDepartment();

            }
        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();
            PasswordHandler passwordhandler = new PasswordHandler();
            EmailHandler emailhandler = new EmailHandler();
            DataTable hrisuser = new DataTable();

            try
            {
                lblerror.ForeColor = System.Drawing.Color.Red;
                string mEmployeeId = lblemployeeid.Text.ToString().Trim().ToUpper();
                string mUserid = lbluserid.Text.ToString().Trim().ToUpper();
                string mfirstName = lblfirstname.Text.Trim().ToString();
                string mLastName = lbllastname.Text.Trim().ToString();
                string madddate = DateTime.Today.ToString("yyyy/MM/dd");
                string mlogUser = (string)(Session["KeyUSER_ID"]);
                string sEmail = lblemail.Text.ToString().Trim();

                if (mEmployeeId == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Employee Not Selected ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (mUserid == "")
                {
                    CommonVariables.MESSAGE_TEXT = "User ID Can not Blank ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (mfirstName == "")
                {
                    CommonVariables.MESSAGE_TEXT = "First Name Can not Blank ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (mLastName == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Last Name Can not Blank ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {
                    if (sEmail.Replace("-","") != "")
                    {
                        string sNewpassWord = passwordhandler.GenerateNewPassword();
                        string pw = txtNewPassword.Text.Trim();

                        //string sEncryptpassword = passwordhandler.Encrypt(sNewpassWord);
                        string sEncryptpassword = passwordhandler.Encrypt(pw);
                        
                        
                        UserAdministration.UpdateHrisUser(sEncryptpassword, mEmployeeId, "1", mlogUser, madddate);
                        CommonVariables.EMAIL_SUBJECT = Constants.CON_PRODUCT + " Password Re-set Confirmation ";
                        CommonVariables.EMAIL_BODY = " Password has been changed for user account " + mUserid +
                                                     " Your New Password is " + pw;
                        CommonVariables.EMAIL_BODY = CommonVariables.EMAIL_BODY + Environment.NewLine + "This is a system generated mail.";

                        emailhandler.SendRegisterEmail(Constants.CON_SENDER, sEmail, CommonVariables.EMAIL_SUBJECT, CommonVariables.EMAIL_BODY);

                        CommonVariables.MESSAGE_TEXT = "Password of User " + mUserid + " Successfully Reset ";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        UserAdministration.UpdateHrisUser(Constants.CON_DEFAULT_PASSWORD, mEmployeeId, Constants.STATUS_ACTIVE_VALUE, mlogUser, madddate);
                        CommonVariables.MESSAGE_TEXT = "Password of User " + mUserid + " Successfully Reset ";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
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
                string mCompCode = dpCompID.SelectedValue;
                string mDeptCode = dpDeptCode.SelectedValue;
                string mEmpNic = txtnicno.Text.ToString();
                //string mLastName = lbllastname.Text.ToString();
                string mLastName = txtlastname.Text.ToString();

                hrisuser = UserAdministration.populateHrisUser(mEPFno, mCompCode, mDeptCode, mEmpNic, mLastName);

                GridView1.DataSource = hrisuser;
                GridView1.DataBind();

                if (hrisuser.Rows.Count >= 1)
                {
                    GridView1.HeaderRow.Cells[0].Text = "Employee ID";
                    GridView1.HeaderRow.Cells[1].Text = "E.P.F. No";
                    GridView1.HeaderRow.Cells[2].Text = "User ID";
                    GridView1.HeaderRow.Cells[3].Text = "First Name";
                    GridView1.HeaderRow.Cells[4].Text = "Known Name";
                    GridView1.HeaderRow.Cells[5].Text = "Company ID";
                    GridView1.HeaderRow.Cells[6].Text = "Email";
                    GridView1.HeaderRow.Cells[0].Width = 100;
                    GridView1.HeaderRow.Cells[1].Width = 150;
                    GridView1.HeaderRow.Cells[2].Width = 100;
                    GridView1.HeaderRow.Cells[3].Width = 300;
                    GridView1.HeaderRow.Cells[4].Width = 300;
                    GridView1.HeaderRow.Cells[5].Width = 200;
                    GridView1.HeaderRow.Cells[6].Width = 200;
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
            CompanyDataHandler company = new CompanyDataHandler();
            DataTable CompID = new DataTable();
            try
            {
                if (KeyCOMP_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    ListItem listItemBlank = new ListItem();
                    listItemBlank.Text = "";
                    listItemBlank.Value = "";
                    dpCompID.Items.Add(listItemBlank);
                    CompID = company.getCompanyIdCompName();
                }
                else
                {
                    CompID = company.getCompanyIdCompName(KeyCOMP_ID);
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
                company = null;
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
                string mCompCode = dpCompID.SelectedValue;
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
            clter_text();
            fillDepartment();
        }

        protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                clter_text();
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
                clter_text();
                lblfirstname.Text = GridView1.SelectedRow.Cells[3].Text;
                lbllastname.Text = GridView1.SelectedRow.Cells[4].Text;
                lbluserid.Text = GridView1.SelectedRow.Cells[2].Text;
                lblemployeeid.Text = GridView1.SelectedRow.Cells[0].Text;
                lblemail.Text = GridView1.SelectedRow.Cells[6].Text;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        private void clter_text()
        {
            lblemployeeid.Text = "";
            Utility.Errorhandler.ClearError(lblerror);
            lblfirstname.Text = "";
            lbllastname.Text = "";
            lbluserid.Text = "";
            lblemail.Text = "";
            txtNewPassword.Text = txtConfirmPassword.Text = "";
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            clter_text();
        }
    }
}