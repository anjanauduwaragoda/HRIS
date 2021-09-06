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
    public partial class Webfrmhrisusers : System.Web.UI.Page
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
            EmailHandler emailhandler = new EmailHandler();
            PasswordHandler passwordhandler = new PasswordHandler();
            UserAdministrationHandler UserAdministration = new UserAdministrationHandler();
            DataTable hrisuser = new DataTable();

            try
            {
                Boolean lbexists = true;
                string mCompanyId = dpWorkCompID.SelectedValue.ToString();
                string mCompanyName = dpWorkCompID.SelectedItem.Text.ToString();
                string mEmployeeId = lblemployeeid.Text.ToString().Trim().ToUpper();
                string mUserid = txtuserid.Text.ToString().Trim().ToUpper(); 
                string mfirstName =  lblfirstname.Text.Trim().ToString(); 
                string mLastName =  lbllastname.Text.Trim().ToString(); 
                string madddate = DateTime.Today.ToString("yyyy/MM/dd");
                string mlogUser = (string)(Session["KeyUSER_ID"]);
                string sEmail = lblemail.Text.ToString().Trim();


                if (mEmployeeId == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Employee Not Selected. ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (mUserid == "")
                {
                    CommonVariables.MESSAGE_TEXT = "User ID Can not Blank. ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (mCompanyName == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Working Comapny Can not Blank. ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {

                    hrisuser = UserAdministration.populateHrisUser(mEmployeeId, Constants.STATUS_ACTIVE_VALUE);
                    if (hrisuser.Rows.Count >= 1)
                    {
                        lbexists = false;
                        UserAdministration.UpdateHrisUserCompany(mCompanyId, mEmployeeId, Constants.STATUS_ACTIVE_VALUE, mlogUser, madddate);
                        CommonVariables.MESSAGE_TEXT = "Functional Area " + mCompanyName + " successfully assigned. ";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    }

                    if (lbexists == true)
                    {
                        hrisuser = UserAdministration.populateHrisUser(mUserid);
                        if (hrisuser.Rows.Count >= 1)
                        {
                            lbexists = false;
                            CommonVariables.MESSAGE_TEXT = "User ID already exists. ";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {
                            if (sEmail != "")
                            {
                                string sNewpassWord = passwordhandler.GenerateNewPassword();
                                string sEncryptpassword = passwordhandler.Encrypt(sNewpassWord);
                                UserAdministration.InsertHrisUser(mCompanyId, sEncryptpassword, mEmployeeId, mUserid, mfirstName, mLastName, Constants.STATUS_ACTIVE_VALUE, mlogUser, madddate);
                                CommonVariables.EMAIL_SUBJECT = Constants.CON_PRODUCT + " Registration Confirmation ";
                                CommonVariables.EMAIL_BODY = " Thank you for registering with " + Constants.CON_PRODUCT +
                                                                " Your registration has been received and Your account " + mUserid + " has been created. " +
                                                                " Your Initial Password is " + sNewpassWord;
                                CommonVariables.EMAIL_BODY = CommonVariables.EMAIL_BODY + Environment.NewLine + "This is a system generated mail.";

                                emailhandler.SendRegisterEmail(Constants.CON_SENDER, sEmail, CommonVariables.EMAIL_SUBJECT, CommonVariables.EMAIL_BODY);
                                CommonVariables.MESSAGE_TEXT = "User ID " + mUserid + " Successfully Updated ";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                            }
                            else
                            {
                                UserAdministration.InsertHrisUser(mCompanyId, Constants.CON_DEFAULT_PASSWORD, mEmployeeId, mUserid, mfirstName, mLastName, Constants.STATUS_ACTIVE_VALUE, mlogUser, madddate);
                                CommonVariables.MESSAGE_TEXT = "User ID " + mUserid + " Successfully Updated ";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                            }
                        }
                    }
                    fillhrisuser();
                    txtuserid.Enabled = false;
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

                hrisuser = UserAdministration.populateHrisUser(mEPFno, mCompCode, mDeptCode, mEmpNic, mLastName);

                GridView1.DataSource = hrisuser;
                GridView1.DataBind();
                    
                if (hrisuser.Rows.Count >= 1)
                    {
                        GridView1.HeaderRow.Cells[0].Text = "Employee ID";
                        GridView1.HeaderRow.Cells[1].Text = "E.P.F. No";
                        GridView1.HeaderRow.Cells[2].Text = "User ID";
                        GridView1.HeaderRow.Cells[3].Text = "Name With Initials";
                        GridView1.HeaderRow.Cells[4].Text = "Known Name";
                        GridView1.HeaderRow.Cells[5].Text = "Working Company";
                        GridView1.HeaderRow.Cells[6].Text = "Email";
                        GridView1.HeaderRow.Cells[0].Width = 100;
                        GridView1.HeaderRow.Cells[1].Width = 150;
                        GridView1.HeaderRow.Cells[2].Width = 100;
                        GridView1.HeaderRow.Cells[3].Width = 300;
                        GridView1.HeaderRow.Cells[4].Width = 300;
                        GridView1.HeaderRow.Cells[5].Width = 200;
                        GridView1.HeaderRow.Cells[6].Width = 150;
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
                CompID = company.getCompanyIdCompName();

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                dpCompID.Items.Add(listItemBlank);

                ListItem ItemWork = new ListItem();
                ItemWork.Text = "";
                ItemWork.Value = "";
                dpWorkCompID.Items.Add(ItemWork);

                ListItem Item = new ListItem();
                Item.Text = Constants.CON_UNIVERSAL_COMPANY_NAME;
                Item.Value = Constants.CON_UNIVERSAL_COMPANY_CODE;
                dpWorkCompID.Items.Add(Item);

                foreach (DataRow dataRow in CompID.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = dataRow["COMP_NAME"].ToString();
                    listItem.Value = dataRow["COMPANY_ID"].ToString();
                    dpWorkCompID.Items.Add(listItem);
                }

                if (KeyCOMP_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
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
                lblemployeeid.Text = GridView1.SelectedRow.Cells[0].Text.Trim();
                lblfirstname.Text = GridView1.SelectedRow.Cells[3].Text.Trim();
                lbllastname.Text = GridView1.SelectedRow.Cells[4].Text.Trim();
                lblemail.Text = GridView1.SelectedRow.Cells[6].Text.Trim().Replace("-", "");
                txtuserid.Text = GridView1.SelectedRow.Cells[2].Text.Trim().Trim().Replace("-", "");

                if (GridView1.SelectedRow.Cells[2].Text.Trim().Trim().Replace("-", "") == "")
                {
                    txtuserid.Enabled = true;
                }
                else
                {
                    txtuserid.Enabled = false;
                }


                if (GridView1.SelectedRow.Cells[5].Text.Trim().Replace("-", "") != "")
                {
                    dpWorkCompID.SelectedValue = GridView1.SelectedRow.Cells[5].Text.Trim();
                }
                else
                {
                    dpWorkCompID.SelectedValue = dpCompID.SelectedValue ;
                    
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        private void clear_text()
        {
            lblemployeeid.Text = "";
            lblfirstname.Text = "";
            lbllastname.Text = "";
            txtuserid.Text = "";
            lblemail.Text = "";
            Utility.Errorhandler.ClearError(lblerror);
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            clear_text();
            btnupdate.Text = Constants.CON_SAVE_BUTTON_TEXT;
        }

    }
}