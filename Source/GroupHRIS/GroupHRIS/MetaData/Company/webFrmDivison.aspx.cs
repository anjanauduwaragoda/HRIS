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
    public partial class WebfrmDivison : System.Web.UI.Page
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
                        ddlCompID.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                    }
                }  
                
                getDepId();
                filStatus();
                fillDivision();
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Boolean ErrorFlag = false;
            string mDepID = ddlDepID.Text.Trim().ToString();
            string mDivName = txtDivName.Text.Trim().ToString();
            string mDesc = txtDesc.Text.Trim().ToString();
            string mLandNo = txtLandNo.Text.Trim().ToString();
            string mCostCen = txtCostCenter.Text.Trim().ToString();
            string mProCen = txtProfitCenter.Text.Trim().ToString();
            string mStatus = ddlStatus.SelectedItem.Value.ToString();
            EmployeeDataHandler employee = new EmployeeDataHandler();
            DivisionDataHandler division = new DivisionDataHandler();
            DivisionHeadDatahandler divHead = new DivisionHeadDatahandler();
            DataTable dtDivision = null;
            DataTable dtEmployee = null;
            DataTable dtDivHead = null;
            try
            {
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    dtDivision = division.populateByName(mDivName, mDepID);
                    if (dtDivision.Rows.Count > 0)
                    {
                        CommonVariables.MESSAGE_TEXT = "Record already exists with : < " + mDivName + " >";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        division.insert(mDepID, mDivName, mDesc, mLandNo, mStatus, mCostCen, mProCen);
                        CommonVariables.MESSAGE_TEXT = "Record(s) saved successfully.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        fillDivision();
                        clear();
                    }
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT) 
                {
                    string mDivID = GridView1.SelectedRow.Cells[0].Text;
                    dtDivision = division.populateByNameID(mDivName, mDivID, mDepID);
                    if (dtDivision.Rows.Count > 0)
                    {
                        CommonVariables.MESSAGE_TEXT = "Record already exists with : < " + mDivName + " >";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        if (mStatus == "0")
                        {
                            dtEmployee = employee.populateByComID(mDivID);
                            if (dtEmployee.Rows.Count > 0)
                            {
                                ErrorFlag = true;
                                CommonVariables.MESSAGE_TEXT = "Division cannot be Inactive, related < Employee > records are available.";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            }
                            else
                            {
                                dtDivHead = divHead.populateActiveData(mDivID);
                                if (dtDivHead.Rows.Count > 0)
                                {
                                    ErrorFlag = true;
                                    CommonVariables.MESSAGE_TEXT = "Division cannot be Inactive, related  < Division Head > records are available.";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                                }
                            }
                        }
                        if (ErrorFlag == false)
                        {
                            division.update(mDivID, mDepID, mDivName, mDesc, mLandNo, mStatus, mCostCen, mProCen);
                            CommonVariables.MESSAGE_TEXT = "Record(s) updated successfully.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                            fillDivision();
                            clear();
                        }
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
                division = null;
                employee = null;
                divHead = null;
                dtEmployee = null;
                dtDivHead = null;
                dtDivision = null;
            }
        }

        private void fillDivision()
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            DivisionDataHandler Division = new DivisionDataHandler();
            DataTable divScheme = new DataTable();
            try
            {
                string mDepartment = ddlDepID.SelectedItem.Value.ToString();

                divScheme = Division.populategrid(mDepartment);
                GridView1.DataSource = divScheme;
                GridView1.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            fillDivision();
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
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblerror);
            DivisionDataHandler division = new DivisionDataHandler();
            DataRow dataRow = null;
            try
            {
                string mDivID = GridView1.SelectedRow.Cells[0].Text;
                dataRow = division.populate(mDivID);

                ddlDepID.SelectedValue = dataRow["DEPT_ID"].ToString().Trim();
                txtDivName.Text = dataRow["DIV_NAME"].ToString().Trim();
                txtDesc.Text = dataRow["DESCRIPTION"].ToString().Trim();
                txtLandNo.Text = dataRow["LAND_PHONE"].ToString().Trim();
                ddlStatus.SelectedValue = dataRow["STATUS_CODE"].ToString().Trim();
                txtCostCenter.Text = dataRow["COST_CENTER_CODE"].ToString().Trim();
                txtProfitCenter.Text = dataRow["PROFIT_CENTER_CODE"].ToString().Trim();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        private void clear() 
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            ddlStatus.SelectedIndex = -1;
            txtDivName.Text = "";
            txtDesc.Text = "";
            txtCostCenter.Text = "";
            txtProfitCenter.Text = "";
            txtLandNo.Text = "";

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
            Utility.Errorhandler.ClearError(lblerror);
        }

        protected void ddlCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            clear();
            getDepId();
            fillDivision();
            Utility.Errorhandler.ClearError(lblerror);
        }

        protected void ddlDepID_SelectedIndexChanged(object sender, EventArgs e)
        {
            clear();
            fillDivision();
            Utility.Errorhandler.ClearError(lblerror);
        }
    }
}