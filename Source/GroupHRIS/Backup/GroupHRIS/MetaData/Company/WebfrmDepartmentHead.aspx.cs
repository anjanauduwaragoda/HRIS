using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler;
using DataHandler.Employee;
using System.Text.RegularExpressions;
using DataHandler.MetaData;
using Common;


namespace GroupHRIS.MetaData
{
    public partial class WebfrmDepartmentHead : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KeyLOGOUT_STS"].Equals("0"))
            {
                Response.Redirect("MainLogout.aspx", false);
            }
            if (!IsPostBack ) 
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
                        fillDepartment();
                        ddlCompID.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                    }
                }  

                filStatus();
                fillDepartment();
                fillDepHead();
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
                else
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = "";
                    listItem.Value = "";

                    ddlCompID.Items.Add(listItem);
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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            fillDepHead();
            clear();
        }

        private void fillDepHead()
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            DepartmentHeadDataHandler depHead = new DepartmentHeadDataHandler();
            DataTable depHeadScheme = new DataTable();
            try
            {
                string mDepID = ddlDepID.SelectedItem.Value.ToString();
                depHeadScheme = depHead.populate(mDepID);
                GridView1.DataSource = depHeadScheme;
                GridView1.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                depHead = null;
                depHeadScheme.Dispose();
            }
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
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        private void fillDepartment()
        {
            DivisionDataHandler department = new DivisionDataHandler();
            DataTable schDepartment = new DataTable();

            try
            {
                ddlDepID.Items.Clear();
                string mComID = ddlCompID.SelectedItem.Value.ToString();
                schDepartment = department.populateByComId(mComID);
 
                if (schDepartment.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in schDepartment.Rows)
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
                lblerror.Text = Ex.Message;
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                department = null;
                schDepartment.Dispose();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string mCompId = ddlCompID.SelectedItem.Value.ToString();
            string mDepId = ddlDepID.SelectedItem.Value.ToString();
            string mEmpID = txtEmpID.Text.ToString();
            string mDateStart = txtDateStart.Text.ToString();
            string mDateEnd = txtDateEnd.Text.ToString();
            string mRemarks = txtRemarks.Text.ToString();
            string mStatus = ddlStatus.SelectedItem.Value.ToString();
            DepartmentHeadDataHandler depHandle = new DepartmentHeadDataHandler();
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            DataTable ActiveHead = null;
            DataTable  employee = null;
            try
            {
                if (Utility.Utils.verifyDate(mDateStart) == false)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Date format incorrect (YYYY/MM/DD)", lblerror);
                    return;
                }

                if (Utility.Utils.verifyDate(mDateEnd) == false)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Date format incorrect (YYYY/MM/DD)", lblerror);
                    return;
                }
                /* validate employee Code */ // VALIDATION REMOVED DUE TO HR REQUEST //2015-11-04 //CHATHURA
                //employee = employeeDataHandler.populateByEmpID(mEmpID, mCompId);
                //if (employee.Rows.Count > 0)
                //{
                    if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                    {
                        ActiveHead = depHandle.populateActiveData(mDepId);
                        if (ActiveHead.Rows.Count > 0)
                        {
                            CommonVariables.MESSAGE_TEXT = "Please Inactive other records.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {
                            string mlogUser = Session["KeyUSER_ID"].ToString();
                            depHandle.insert(mDepId, mEmpID, mDateStart, mDateEnd, mRemarks, mStatus, mlogUser);
                            CommonVariables.MESSAGE_TEXT = "Record(s) saved successfully.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                    }
                    else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {
                        string mLineNo = hfLineNo.Value.ToString();
                        string mlogUser = Session["KeyUSER_ID"].ToString();
                        depHandle.update(mLineNo, mDepId, mEmpID, mDateStart, mDateEnd, mRemarks, mStatus, mlogUser);
                        CommonVariables.MESSAGE_TEXT = "Record(s) modified successfully.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    fillDepHead();
                    clear();
                //}
                //else
                //{
                //    CommonVariables.MESSAGE_TEXT = "Employee Code or Employee's Company Code is invalid. ";
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
                depHandle = null;
                employeeDataHandler = null;
                ActiveHead = null;
                //ActiveHead.Dispose();
                //employee.Dispose();
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblerror);
            DepartmentHeadDataHandler depHandle = new DepartmentHeadDataHandler();
            DataRow dataRow = null;
            try
            {
                string lineNo = GridView1.SelectedRow.Cells[0].Text;
                dataRow = depHandle.getDepHeadDetails(lineNo);

                hfLineNo.Value = dataRow["LINE_NO"].ToString().Trim();
                txtRemarks.Text = dataRow["REMARKS"].ToString().Trim();
                ddlDepID.SelectedValue = dataRow["DEPT_ID"].ToString().Trim();
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

        private void clear() 
        {
            txtDateEnd.Text = "";
            txtDateStart.Text = "";
            txtEmpID.Text = "";
            txtRemarks.Text = "";
            ddlStatus.SelectedIndex = -1;
            btnSave.Enabled = true;
            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            clear();
            Utility.Errorhandler.ClearError(lblerror);
        }

        protected void ddlCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            clear();
            fillDepartment();
            fillDepHead();
            Utility.Errorhandler.ClearError(lblerror);
        }

        protected void ddlDepID_SelectedIndexChanged(object sender, EventArgs e)
        {
            clear();
            fillDepHead();
            Utility.Errorhandler.ClearError(lblerror);
        }
    }
}