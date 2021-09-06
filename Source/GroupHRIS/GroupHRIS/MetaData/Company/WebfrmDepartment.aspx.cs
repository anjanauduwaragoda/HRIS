using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.MetaData;
using DataHandler.Employee;
using Common;

namespace GroupHRIS.MetaData
{
    public partial class WebfrmDepartment : System.Web.UI.Page
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
                        getCompID();
                        fillDepartment();
                    }
                    else
                    {
                        getCompID(Session["KeyCOMP_ID"].ToString().Trim());
                        fillDepartment();
                        ddlCompID.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                    }
                }  
                filStatus();
            }
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
  
        private void fillDepartment()
        {
            DepartmentDataHandler Department = new DepartmentDataHandler();
            DataTable DepScheme = new DataTable();
            try
            {
                string mCompCode = ddlCompID.SelectedValue;
                DepScheme = Department.populateByComId(mCompCode);
                GridView1.DataSource = DepScheme;
                GridView1.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                Department = null;
                DepScheme.Dispose();
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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            fillDepartment();
            clear();
        }

        protected void dpCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Utility.Errorhandler.ClearError(lblerror);
                clear();
                fillDepartment();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
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
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        public void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblerror);
            DepartmentDataHandler department = new DepartmentDataHandler();
            DataRow dataRow = null;
            try
            {
                string mDepID = GridView1.SelectedRow.Cells[0].Text;
                dataRow = department.populate(mDepID);

                if (dataRow != null)
                {
                    ddlCompID.Text = dataRow["COMPANY_ID"].ToString().Trim();
                    txtDepName.Text = dataRow["DEPT_NAME"].ToString().Trim();
                    txtLandNo.Text = dataRow["LAND_PHONE"].ToString().Trim();
                    txtDesc.Text = dataRow["DESCRIPTION"].ToString().Trim();
                    ddlStatus.SelectedValue = dataRow["STATUS_CODE"].ToString().Trim();
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
                dataRow = null;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
            Utility.Errorhandler.ClearError(lblerror);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Boolean ErrorFlag = false;
            string mComID = ddlCompID.Text.Trim().ToString();
            string mDepName = txtDepName.Text.Trim().ToString();
            string mLandNo = txtLandNo.Text.Trim().ToString();
            string mDesc = txtDesc.Text.Trim().ToString();
            string mStatus = ddlStatus.SelectedItem.Value.ToString();
            DepartmentDataHandler department = new DepartmentDataHandler();
            EmployeeDataHandler employee = new EmployeeDataHandler();
            DivisionDataHandler division = new DivisionDataHandler();
            DepartmentHeadDataHandler depHead = new DepartmentHeadDataHandler();
            DataTable dtDepartment = null;
            DataTable dtDivision = null;
            DataTable dtEmployee = null;
            DataTable dtDepHead = null;
            try
            {
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    dtDepartment = department.populateByName(mDepName, mComID);
                    if (dtDepartment.Rows.Count > 0)
                    {
                        CommonVariables.MESSAGE_TEXT = "Record already exists with : < " + mDepName + " >";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        department.Insert(mComID, mDepName, mLandNo, mDesc, mStatus);
                        CommonVariables.MESSAGE_TEXT = "Record(s) saved successfully.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        fillDepartment();
                        clear();
                    }
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string mDepID = GridView1.SelectedRow.Cells[0].Text;
                    dtDepartment = department.populateByNameID(mDepName, mDepID, mComID);
                    if (dtDepartment.Rows.Count > 0)
                    {
                        
                        CommonVariables.MESSAGE_TEXT = "Record already exists with : < " + mDepName + " >";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        if (mStatus == "0") /* check related records - when Inactivate a record*/
                        {
                            dtEmployee = employee.populateByDepId(mDepID);
                            if (dtEmployee.Rows.Count > 0)
                            {
                                ErrorFlag = true;
                                CommonVariables.MESSAGE_TEXT = "Department cannot be Inactive, related < Employee > records are available.";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            }
                            else
                            {
                                dtDivision = division.populateByDepIDActive(mDepID);
                                if (dtDivision.Rows.Count > 0)
                                {
                                    ErrorFlag = true;
                                    CommonVariables.MESSAGE_TEXT = "Department cannot be Inactive, related < Division > records are available.";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                                }
                                else
                                {
                                    dtDepHead = depHead.populateActiveData(mDepID);
                                    if (dtDepHead.Rows.Count > 0)
                                    {
                                        ErrorFlag = true;
                                        CommonVariables.MESSAGE_TEXT = "Department cannot be Inactive, related < Department Head > records are available.";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                                    }
                                }
                            }
                        }
                        if (ErrorFlag == false)
                        { 
                            department.Update(mDepID, mComID, mDepName, mLandNo, mDesc, mStatus);
                            CommonVariables.MESSAGE_TEXT = "Record(s) updated successfully.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                            fillDepartment();
                            clear();
                        }
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
                employee = null;
                department = null;
                division = null;
                depHead = null;
                dtDepHead = null;
                dtDepartment = null;
                dtDivision = null;
                dtEmployee = null;
            }
        }

        public void clear()
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            txtDepName.Text = "";
            txtLandNo.Text = "";
            txtDesc.Text = "";
            ddlStatus.SelectedIndex = -1;
            
        }
    }
}