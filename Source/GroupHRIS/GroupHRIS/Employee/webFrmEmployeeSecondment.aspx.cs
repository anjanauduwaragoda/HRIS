using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.Employee;
using DataHandler.MetaData;
using Common;
using NLog;
using GroupHRIS.Utility;

namespace GroupHRIS.Employee
{
    public partial class webFrmEmployeeSecondment : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string userID = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {
                userID = Session["KeyUSER_ID"].ToString();
            }


            if (!IsPostBack)
            {
                fillCompanies();

                if (Session["KeyUSER_ID"] != null)
                {
                    userID = Session["KeyUSER_ID"].ToString();
                }
            }
            else
            {
                txtEmployeeID.Text          = hfEmpID.Value;
                txtName.Text                = hfName.Value;
                txtFromCompanyCode.Text     = hfCompanyCode.Value;
                txtFromCompanyName.Text     = hfCompanyName.Value;
                txtFromDepartmentID.Text    = hfDepartmentID.Value;
                txtFromDepartmentName.Text  = hfDepartmentName.Value;
                txtFromDivisionID.Text      = hfDivisionID.Value;
                txtFromDivisionName.Text    = hfDivisionName.Value;

                txtFromBranchID.Text        = hfBranchID.Value;
                txtFromBranchName.Text      = hfBranchName.Value;
                txtFromCC.Text              = hfCC.Value;
                txtFromPC.Text              = hfPC.Value;

                populateGrid();

                string parameter = Request["__EVENTARGUMENT"];

                if (parameter.Equals("TextChanged"))
                    Utility.Errorhandler.ClearError(lblMsg);
            }
        }



        //------------------------------------------------------------------------------
        ///<summary>
        ///Load all companies 
        ///</summary>
        //------------------------------------------------------------------------------
        private void fillCompanies()
        {
            log.Debug("fillCompanies()");

            CompanyDataHandler dhCompany = new CompanyDataHandler();
            DataTable dtCompanies = new DataTable();

            try
            {
                dtCompanies = dhCompany.getCompanyIdCompName().Copy();

                ddlSecCompany.Items.Clear();

                if (dtCompanies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlSecCompany.Items.Add(Item);

                    foreach (DataRow dataRow in dtCompanies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlSecCompany.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //lblerror.Text = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
                throw ex;
            }
            finally
            {
                dhCompany = null;
                dtCompanies.Dispose();
            }

        }

        protected void ddlToCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlToCompany_SelectedIndexChanged()");

            if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
            {
                fillDepartment(ddlSecCompany.SelectedValue.Trim());
                fillBranches(ddlSecCompany.SelectedValue.Trim());
                fillCostCenter(ddlSecCompany.SelectedValue.Trim());
                fillProfitCenter(ddlSecCompany.SelectedValue.Trim());
            }

            populateGrid();
            checkActiveSecondmentExists();


        }

        protected void ddlToDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSecDepartment.SelectedValue != "")
            {
                fillDivisions(ddlSecDepartment.SelectedValue.Trim());
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            EmployeeDataHandler dhEmp = new EmployeeDataHandler();
            EmployeeSecondmentDataHandler dhEmpSec = new EmployeeSecondmentDataHandler();

            Utility.Errorhandler.ClearError(lblMsg);

            try
            {
                //Check whether the employee is Active or not
                if (!dhEmp.isActiveEmployee(txtEmployeeID.Text.Trim()))
                {
                    lblMsg.Text = String.Empty;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "This employee is not in ACTIVE state.", lblMsg);
                }

                //Check wheather FromDate <= ToDate
                else if ((txtEndDate.Text.Trim().Length > 0) && (!CommonUtils.isValidDateRange(txtFromDate.Text.Trim(), txtEndDate.Text.Trim())))
                {
                    lblMsg.Text = String.Empty;
                    //lblerror.Text = "Invalid Date Range. From Date should be a earlier date than the End Date.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Invalid Date Range", lblMsg);
                }

                //Check for overlapping secondments
                else if (dhEmpSec.isNonOverlappingDateRange(txtEmployeeID.Text.Trim(), txtFromDate.Text.Trim(), txtEndDate.Text.Trim()))
                {
                    dhEmpSec.Insert(txtEmployeeID.Text.Trim(),
                                      ddlSecCompany.SelectedValue.Trim(),
                                      ddlSecDepartment.SelectedValue.Trim(),
                                      ddlSecDivision.SelectedValue.Trim(),
                                      ddlToBranch.SelectedValue.Trim(),
                                      ddlToCC.SelectedValue.Trim(),
                                      ddlToPC.SelectedValue.Trim(),
                                      txtFromDate.Text.Trim(),
                                      txtEndDate.Text.Trim(),
                                      txtRemarks.Text.Trim(),
                                      userID);

                    clear();
                    lblerror.Text = String.Empty;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED, lblMsg);
                }
                else
                {
                    lblMsg.Text = String.Empty;
                    //lblerror.Text = "Another secondment exists for this employee with an overlapping date range.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Overlapping secondment exists ", lblMsg);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
                throw ex;
            }
            finally
            {
                dhEmpSec    = null;
                dhEmp       = null;
            }

            populateGrid();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);
            clearAll();
        }

        private void clear()
        {
            GroupHRIS.Utility.Utils.clearControls(true, ddlSecCompany, ddlSecDepartment, ddlSecDivision, ddlToBranch, ddlToCC, ddlToPC, txtFromDate, txtEndDate, txtRemarks);

            gvSecondments.DataSource = null;
            gvSecondments.DataBind();
        }


        private void clearAll()
        {
            GroupHRIS.Utility.Utils.clearControls(true, txtEmployeeID, txtName, ddlSecCompany, ddlSecDepartment, ddlToBranch, ddlToCC, ddlToPC, ddlSecDivision, txtFromDivisionName, txtFromDate, txtEndDate, txtRemarks, txtFromCompanyCode, txtFromCompanyName, txtFromDepartmentID, txtFromDepartmentName, txtFromDivisionID, txtFromDivisionName, txtFromPC, txtFromCC, txtFromBranchName,txtFromBranchID);

            gvSecondments.DataSource = null;
            gvSecondments.DataBind();
        }

        //fill Cost centers

        private void fillCostCenter(string companyId)
        {

            EmployeeTransferDataHandler oDataHandler = new EmployeeTransferDataHandler();
            DataTable dtcostCenter = new DataTable();

            try
            {
                dtcostCenter = oDataHandler.getCostCenterByCompany(companyId);
                ddlToCC.Items.Clear();

                if (dtcostCenter.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlToCC.Items.Add(Item);

                    foreach (DataRow dataRow in dtcostCenter.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COST_PROFIT_CENTER_NAME"].ToString();
                        listItem.Value = dataRow["COMP_COST_PROFIT_CENTER_CODE"].ToString();

                        ddlToCC.Items.Add(new ListItem(listItem.Value + " - " + listItem.Text, listItem.Value));
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        //fill Profit center

        private void fillProfitCenter(string companyId)
        {
            EmployeeTransferDataHandler oDataHandler = new EmployeeTransferDataHandler();
            DataTable dtprofitCenter = new DataTable();

            try
            {
                dtprofitCenter = oDataHandler.getProfitCenterByCompany(companyId);
                ddlToPC.Items.Clear();

                if (dtprofitCenter.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlToPC.Items.Add(Item);

                    foreach (DataRow dataRow in dtprofitCenter.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COST_PROFIT_CENTER_NAME"].ToString();
                        listItem.Value = dataRow["COMP_COST_PROFIT_CENTER_CODE"].ToString();

                        ddlToPC.Items.Add(new ListItem(listItem.Value + " - " + listItem.Text, listItem.Value));
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        //------------------------------------------------------------------------------
        ///<summary>
        ///Load departments for a given company 
        ///</summary>
        //------------------------------------------------------------------------------
        private void fillDepartment(string companyId)
        {
            log.Debug("fillDepartment() - companyId:" + companyId);

            DepartmentDataHandler dhDepartment = new DepartmentDataHandler();
            DataTable dtDepartments = new DataTable();

            try
            {
                dtDepartments = dhDepartment.getDepartmentIdDeptName(companyId).Copy();

                ddlSecDepartment.Items.Clear();

                if (dtDepartments.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlSecDepartment.Items.Add(Item);

                    foreach (DataRow dataRow in dtDepartments.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DEPT_NAME"].ToString();
                        listItem.Value = dataRow["DEPT_ID"].ToString();

                        ddlSecDepartment.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                dhDepartment = null;
                dtDepartments.Dispose();
            }

        }



        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Load divisions for a given department
        ///</summary>
        //----------------------------------------------------------------------------------------
        private void fillDivisions(string departmentId)
        {
            log.Debug("fillDivisions() - departmentId:" + departmentId);

            DivisionDataHandler dhDivisionr = new DivisionDataHandler();
            DataTable dtDivisions = new DataTable();

            try
            {
                dtDivisions = dhDivisionr.getDivisionIdDivName(departmentId).Copy();

                ddlSecDivision.Items.Clear();

                if (dtDivisions.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlSecDivision.Items.Add(Item);

                    foreach (DataRow dataRow in dtDivisions.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DIV_NAME"].ToString();
                        listItem.Value = dataRow["DIVISION_ID"].ToString();

                        ddlSecDivision.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                dhDivisionr = null;
                dtDivisions.Dispose();
            }

        }

        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Load branches for a given company
        ///</summary>
        //----------------------------------------------------------------------------------------
        private void fillBranches(string companyId)
        {
            log.Debug("fillBranches() - companyId:" + companyId);

            BranchCenterDataHandler dhBranch = new BranchCenterDataHandler();
            DataTable dtBranch = new DataTable();

            try
            {
                dtBranch = dhBranch.getBranchesForCompany(companyId).Copy();

                ddlToBranch.Items.Clear();

                if (dtBranch.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlToBranch.Items.Add(Item);

                    foreach (DataRow dataRow in dtBranch.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["BRANCH_NAME"].ToString();
                        listItem.Value = dataRow["BRANCH_ID"].ToString();

                        ddlToBranch.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                dhBranch = null;
                dtBranch.Dispose();
            }

        }



        private void populateGrid()
        {
            EmployeeSecondmentDataHandler dhEmp = new EmployeeSecondmentDataHandler();
            DataTable dtUsers = new DataTable();

            try
            {
                dtUsers = dhEmp.populate(txtEmployeeID.Text);

                gvSecondments.DataSource = dtUsers;
                gvSecondments.DataBind();


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //lblerror.Text = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
            finally
            {
                dhEmp = null;
                dtUsers.Dispose();
            }
        }


        private void checkActiveSecondmentExists()
        {
            EmployeeSecondmentDataHandler dhSecondment = new EmployeeSecondmentDataHandler();
            DataRow drSecondment = null;

            try
            {
                drSecondment = dhSecondment.getActiveSecondment(txtEmployeeID.Text.Trim());

                if ((drSecondment != null) && (!drSecondment.IsNull("COMPANY_ID")))
                {
                    //lblMsg.Text = "There is a active secondment in company code " + drSecondment["COMPANY_ID"].ToString() + " for this employee.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "There is a active secondment in company code " + drSecondment["COMPANY_ID"].ToString() + " for this employee.", lblMsg);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //lblerror.Text = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
            finally
            {
                dhSecondment = null;
                drSecondment = null;
            }
        }

        protected void gvSecondments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSecondments.PageIndex = e.NewPageIndex;
            populateGrid();
        }



    }
}