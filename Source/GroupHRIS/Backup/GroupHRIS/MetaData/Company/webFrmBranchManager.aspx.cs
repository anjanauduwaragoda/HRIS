using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.MetaData;
using DataHandler.Employee;
using Common;
using System.Data;

namespace GroupHRIS.MetaData
{
    public partial class webFrmBranchManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KeyLOGOUT_STS"].Equals("0"))
            {
            Response.Redirect("MainLogout.aspx", false);
            }
            if (!IsPostBack)
            {
                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        getCompId();
                    }
                    else
                    {
                        getCompId(Session["KeyCOMP_ID"].ToString().Trim());
                        ddlCompID.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                    }
                } 
                
                filStatus();
                //getBranchId();
                //filBranchManagerData();
            }

            if (IsPostBack)
            {
                if (hfCaller.Value == "txtEmpID")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtEmpID.Text = hfVal.Value;
                    }
                    if (txtEmpID.Text != "")
                    {
                        //Postback Methods
                    }
                }
            }
        }

        private void filStatus()
        {
            ListItem listItemBlank = new ListItem();
            listItemBlank.Text = "";
            listItemBlank.Value = "";
            ddlStatus.Items.Add(listItemBlank);

            ListItem listItemActive = new ListItem();
            listItemActive.Text = Constants.STATUS_ACTIVE_TAG;
            listItemActive.Value = Constants.STATUS_ACTIVE_VALUE;
            ddlStatus.Items.Add(listItemActive);

            ListItem listItemInActive = new ListItem();
            listItemInActive.Text = Constants.STATUS_INACTIVE_TAG;
            listItemInActive.Value = Constants.STATUS_INACTIVE_VALUE;
            ddlStatus.Items.Add(listItemInActive);
        }

        private void getCompId()
        {
            CompanyDataHandler company = new CompanyDataHandler();
            DataTable schCompnay = new DataTable();
            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlCompID.Items.Add(listItemBlank);

                schCompnay = company.populate();

                if (schCompnay.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in schCompnay.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();
                        ddlCompID.Items.Add(listItem);
                    }
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                company = null;
                schCompnay.Dispose();
            }
        }

        private void getCompId(string sComID)
        {
            CompanyDataHandler company = new CompanyDataHandler();
            DataTable schCompnay = new DataTable();
            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlCompID.Items.Add(listItemBlank);

                schCompnay = company.getCompanyIdCompName(sComID);

                if (schCompnay.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in schCompnay.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();
                        ddlCompID.Items.Add(listItem);
                    }
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                company = null;
                schCompnay.Dispose();
            }
        }

        private void filBranchManagerData()
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            BranchManagerDataHandler branchManager = new BranchManagerDataHandler();
            DataTable schBranchManager = new DataTable();
            try
            {
                string mBranchID = ddlBranchID.SelectedItem.Value.ToString();
                schBranchManager = branchManager.populateByBranchID(mBranchID);
                GridView1.DataSource = schBranchManager;
                GridView1.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                branchManager = null;
                schBranchManager.Dispose();
            }
        }

        private void getBranchId()
        {
            BranchCenterDataHandler branch = new BranchCenterDataHandler();
            DataTable schbranch = new DataTable();

            try
            {
                ddlBranchID.Items.Clear();
                string mComID = ddlCompID.SelectedItem.Value.ToString();
                schbranch = branch.populateBranch(mComID);

                if (schbranch.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in schbranch.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["BRANCH_NAME"].ToString();
                        listItem.Value = dataRow["BRANCH_ID"].ToString();

                        ddlBranchID.Items.Add(listItem);
                    }
                }
                else
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = "";
                    listItem.Value = "";

                    ddlBranchID.Items.Add(listItem);
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                branch = null;
                schbranch.Dispose();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string mCompID = ddlCompID.SelectedItem.Value.ToString();
            string mBranchID = ddlBranchID.SelectedItem.Value.ToString();
            string mEmpID = txtEmpID.Text.ToString();
            string mDateStart = txtDateStart.Text.ToString();
            string mDateEnd = txtDateEnd.Text.ToString();
            string mRemarks = txtRemarks.Text.ToString();
            string mStatus = ddlStatus.SelectedItem.Value.ToString();

            BranchManagerDataHandler branchManager = new BranchManagerDataHandler();
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            DataTable employee = null;
            DataTable ActiveManger = null;

            if (Utility.Utils.verifyDate(mDateStart) == false)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Date format incorrect(YYYY/MM/DD)", lblerror);
                return;
            }

            if (Utility.Utils.verifyDate(mDateEnd) == false)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Date format incorrect(YYYY/MM/DD)", lblerror);
                return;
            }

            try
            {
                /* validate employee Code */
                employee = employeeDataHandler.populateByEmpID(mEmpID, mCompID);
                if (employee.Rows.Count > 0)
                {
                    if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                    {
                        ActiveManger = branchManager.populateActiveData(mBranchID);
                        if (ActiveManger.Rows.Count > 0)
                        {
                            CommonVariables.MESSAGE_TEXT = "Please Inactive other records.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {
                            string mAddedUser = Session["KeyUSER_ID"].ToString();
                            branchManager.insert(mBranchID, mEmpID, mDateStart, mDateEnd, mRemarks, mStatus, mAddedUser);
                            CommonVariables.MESSAGE_TEXT = "Record(s) saved successfully.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                    }
                    else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {
                        string mLineNo = GridView1.SelectedRow.Cells[0].Text;
                        string mModifiyUser = Session["KeyUSER_ID"].ToString();
                        branchManager.update(mLineNo, mBranchID, mEmpID, mDateStart, mDateEnd, mRemarks, mStatus, mModifiyUser);
                        CommonVariables.MESSAGE_TEXT = "Record(s) modified successfully.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    clear();
                    filBranchManagerData();
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Employee Code or Employee's Company Code is invalid.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                employeeDataHandler = null;
                branchManager = null;
                employee.Dispose();
                employee = null;
                //ActiveManger.Dispose();
                //ActiveManger = null;
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblerror);

            BranchManagerDataHandler branchManager = new BranchManagerDataHandler();
            DataRow dataRow = null;
            try
            {
                string mLineNo = GridView1.SelectedRow.Cells[0].Text;
                dataRow = branchManager.populate(mLineNo);

                if (dataRow != null)
                {
                    ddlBranchID.SelectedValue = dataRow["BRANCH_ID"].ToString().Trim();
                    txtEmpID.Text = dataRow["EMPLOYEE_ID"].ToString().Trim();
                    txtDateStart.Text = dataRow["DATE_START"].ToString().Trim();
                    txtDateEnd.Text = dataRow["DATE_END"].ToString().Trim();
                    txtRemarks.Text = dataRow["REMARKS"].ToString().Trim();
                    ddlStatus.SelectedValue = dataRow["STATUS_CODE"].ToString().Trim();
                    if (ddlStatus.SelectedValue == Constants.STATUS_INACTIVE_VALUE)
                    {
                        btnSave.Enabled = false;
                    }
                    else
                    {
                        btnSave.Enabled = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                branchManager = null;
                dataRow = null;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.GridView1, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            filBranchManagerData();
            clear();
        }

        protected void clear()
        {
            ddlStatus.SelectedIndex = -1;
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            txtDateEnd.Text = "";
            txtDateStart.Text = "";
            txtEmpID.Text = "";
            txtRemarks.Text = "";
            btnSave.Enabled = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblerror);
            clear();
        }

        protected void ddlCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            clear();
            getBranchId();
            filBranchManagerData();
            Utility.Errorhandler.ClearError(lblerror);
        }

        protected void ddlBranchID_SelectedIndexChanged(object sender, EventArgs e)
        {
            clear();
            filBranchManagerData();
        }

    }
}