using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.MetaData;
using DataHandler.Employee;
using System.Data;

namespace GroupHRIS.MetaData
{
    public partial class webFrmDivisionHead : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KeyLOGOUT_STS"].Equals("0"))
            {
                Response.Redirect("MainLogout.aspx", false);
            }

            if (!IsPostBack  )
            {
                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        getCompID();
                    }
                    else
                    {
                        getCompID(Session["KeyCOMP_ID"].ToString().Trim());
                        ddlCompID.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                    }
                }  
                filStatus();
                getDepId();
                filDivision();
                fillDivHead();
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
        private void getCompID()
        {
            CompanyDataHandler company = new CompanyDataHandler();
            DataTable schCompID = new DataTable();

            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlCompID.Items.Add(listItemBlank);

                schCompID = company.populate();

                if (schCompID.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in schCompID.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlCompID.Items.Add(listItem);
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
                company = null;
                schCompID.Dispose();
            }
        }
        private void getCompID(string sComID)
        {
            CompanyDataHandler company = new CompanyDataHandler();
            DataTable schCompID = new DataTable();

            try
            {
                schCompID = company.getCompanyIdCompName(sComID);

                if (schCompID.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in schCompID.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlCompID.Items.Add(listItem);
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
                company = null;
                schCompID.Dispose();
            }
        }
        private void getDepId()
        {
            DivisionDataHandler department = new DivisionDataHandler();
            DataTable schDepID = new DataTable();

            try
            {
                ddlDepID.Items.Clear();
                string sCompID = ddlCompID.SelectedItem.Value.ToString();
                schDepID = department.populateByComId(sCompID);

                if (schDepID.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in schDepID.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DEPT_NAME"].ToString();
                        listItem.Value = dataRow["DEPT_ID"].ToString();
                        ddlDepID.Items.Add(listItem);
                    }
                }
                else
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = "";
                    listItem.Value = "";
                    ddlDepID.Items.Add(listItem);
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                department = null;
                schDepID.Dispose();
            }
        }
        private void fillDivHead()
        {
            
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            DivisionHeadDatahandler divDatahandle = new DivisionHeadDatahandler();
            DataTable schDivHead = new DataTable();
            try
            {
                string mDivID = ddlDivId.SelectedItem.Value.ToString();
                schDivHead = divDatahandle.populate(mDivID);
                GridView1.DataSource = schDivHead;
                GridView1.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                divDatahandle = null;
                schDivHead.Dispose();
            }
        }
        private void filDivision()
        {
            DivisionDataHandler division = new DivisionDataHandler();
            DataTable schDivision = new DataTable();
            try
            {
                ddlDivId.Items.Clear();
                string mDeptID = ddlDepID.SelectedItem.Value.ToString();
                schDivision = division.populateByDepID(mDeptID);

                if (schDivision.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in schDivision.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DIV_NAME"].ToString();
                        listItem.Value = dataRow["DIVISION_ID"].ToString();

                        ddlDivId.Items.Add(listItem);
                    }
                }
                else
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = "";
                    listItem.Value = "";

                    ddlDivId.Items.Add(listItem);
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                division = null;
                schDivision.Dispose();
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblerror);
            DivisionHeadDatahandler depHandle = new DivisionHeadDatahandler();
            DataRow dataRow = null;
            try
            {
                string lineNo = GridView1.SelectedRow.Cells[0].Text;
                dataRow = depHandle.getDivHeadDetails(lineNo);

                if (dataRow != null)
                {
                    txtRemarks.Text = dataRow["REMARKS"].ToString().Trim();
                    ddlDivId.SelectedValue = dataRow["DIVISION_ID"].ToString().Trim();
                    txtEmpID.Text = dataRow["EMPLOYEE_ID"].ToString().Trim();
                    txtDateEnd.Text = dataRow["Date_END"].ToString().Trim();
                    txtDateStart.Text = dataRow["Date_START"].ToString().Trim();
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
                depHandle = null;
                dataRow = null;
            }
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            string mCompID = ddlCompID.SelectedItem.Value.ToString();
            string mDivId = ddlDivId.SelectedItem.Value.ToString();
            string mEmpID = txtEmpID.Text.ToString();
            string mDateStart = txtDateStart.Text.ToString();
            string mDateEnd = txtDateEnd.Text.ToString();
            string mRemarks = txtRemarks.Text.ToString();
            string mStatus = ddlStatus.SelectedItem.Value.ToString();
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            DivisionHeadDatahandler divHandle = new DivisionHeadDatahandler();
            DataTable ActiveHead = null;
            try
            {
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

                /* validate employee Code */ //VALIDATION REMOVED DUE TO HR REQUEST //2015-11-05 //CHATHURA
                //DataTable employee = null;
                //employee = employeeDataHandler.populateByEmpID(mEmpID, mCompID);
                //if (employee.Rows.Count > 0)
                //{
                    if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                    {
                        ActiveHead = divHandle.populateActiveData(mDivId);
                        if (ActiveHead.Rows.Count > 0)
                        {
                            CommonVariables.MESSAGE_TEXT = "Please Inactive other records.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {
                            string mlogUser = Session["KeyUSER_ID"].ToString();
                            divHandle.insert(mDivId, mEmpID, mDateStart, mDateEnd, mRemarks, mStatus, mlogUser);
                            CommonVariables.MESSAGE_TEXT = "Record(s) saved successfully.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                    }
                    else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {
                        string mLineNo = GridView1.SelectedRow.Cells[0].Text;
                        string mlogUser = Session["KeyUSER_ID"].ToString();
                        divHandle.update(mLineNo, mDivId, mEmpID, mDateStart, mDateEnd, mRemarks, mStatus, mlogUser);
                        CommonVariables.MESSAGE_TEXT = "Record(s) modified successfully.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    clear();
                    fillDivHead();
                //}
                //else
                //{
                //    CommonVariables.MESSAGE_TEXT = "Employee Code or Employee's Company Code is invalid.";
                //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                //}
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                employeeDataHandler = null;
                divHandle = null;
                // ActiveHead.Dispose();
            }
        }

        private void clear()
        {
            txtDateEnd.Text = "";
            txtDateStart.Text = "";
            txtEmpID.Text = "";
            txtRemarks.Text = "";
            ddlStatus.SelectedIndex = -1;
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            btnSave.Enabled = true;

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
            Utility.Errorhandler.ClearError(lblerror);
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            fillDivHead();
            clear();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(this.GridView1, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void ddlDivId_SelectedIndexChanged(object sender, EventArgs e)
        {
            clear();
            fillDivHead();
        }

        protected void ddlCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            clear();
            getDepId();
            filDivision();
            fillDivHead();
            Utility.Errorhandler.ClearError(lblerror);
        }

        protected void ddlDepID_SelectedIndexChanged(object sender, EventArgs e)
        {
            clear();
            filDivision();
            fillDivHead();
        }
    }
}