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
    public partial class webFrmEmployeeTransfer : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string userID = "";

        void clearFields()
        {
            //txtEmployeeID.Text = String.Empty;
            txtName.Text = String.Empty;
            txtFromFirstLevelSupervisor.Text = String.Empty;
            txtFromFirstLevelSupervisorName.Text = String.Empty;
            txtFromSecondLevelSupervisor.Text = String.Empty;
            txtFromSecondLevelSupervisorName.Text = String.Empty;
            txtFromThirdLevelSupervisor.Text = String.Empty;
            txtFromThirdLevelSupervisorName.Text = String.Empty;
            txtFromCompanyCode.Text = String.Empty;
            txtFromCompanyName.Text = String.Empty;

            txtFromDepartmentID.Text = String.Empty;
            txtFromDepartmentName.Text = String.Empty;
            txtFromDivisionID.Text = String.Empty;
            txtFromDivisionName.Text = String.Empty;
            txtFromBranchName.Text = String.Empty;
            txtfrmEPF.Text = String.Empty;
            txtfrmETF.Text = String.Empty;
            txtfrmDesignation.Text = String.Empty;
            txtDesignationName.Text = String.Empty;
            txtFromCC.Text = String.Empty;

            txtfccName.Text = String.Empty;
            txtFromPC.Text = String.Empty;
            txtfpcName.Text = String.Empty;
            lblNextEpfNo.Text = String.Empty;
            txtToEPF.Text = String.Empty;
            txtToFirstLevelSupervisor.Text = String.Empty;
            txtToSecondLevelSupervisor.Text = String.Empty;
            txtToThirdLevelSupervisor.Text = String.Empty;
            txtStartDate.Text = String.Empty;
            txtRemarks.Text = String.Empty;

            lbltRpt1.Text = String.Empty;
            lbltRpt2.Text = String.Empty;
            lbltRpt3.Text = String.Empty;

            try { ddlToCompany.SelectedIndex = 0; }
            catch { }
            try { ddlToDepartment.SelectedIndex = 0; }
            catch { }
            try { ddlToDivision.SelectedIndex = 0; }
            catch { }
            try { ddlToBranch.SelectedIndex = 0; }
            catch { }

        }

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

                if (hfCaller.Value == "txtEmployeeID")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtEmployeeID.Text = hfVal.Value;
                    }
                    if (txtEmployeeID.Text != "")
                    {
                        Utility.Errorhandler.ClearError(lblMsg);
                        string empID = txtEmployeeID.Text;
                        clearAll();
                        txtEmployeeID.Text = empID;
                        btnSave.Text = "Save";
                        populateGrid();
                        //Postback Methods
                        checkActiveStatus();

                        EmployeeTransferDataHandler oEmployeeTransferDataHandler = new EmployeeTransferDataHandler();
                        txtName.Text = oEmployeeTransferDataHandler.getEmpName(txtEmployeeID.Text.Trim());

                        DataTable dtFromReportTo = new DataTable();
                        dtFromReportTo = oEmployeeTransferDataHandler.populateSupervisors(txtEmployeeID.Text.Trim());
                        txtFromFirstLevelSupervisor.Text = dtFromReportTo.Rows[0]["REPORT_TO_1"].ToString();
                        txtFromFirstLevelSupervisorName.Text = oEmployeeTransferDataHandler.getEmpName(txtFromFirstLevelSupervisor.Text.Trim());
                        txtFromSecondLevelSupervisor.Text = dtFromReportTo.Rows[0]["REPORT_TO_2"].ToString();
                        txtFromSecondLevelSupervisorName.Text = oEmployeeTransferDataHandler.getEmpName(txtFromSecondLevelSupervisor.Text.Trim());
                        txtFromThirdLevelSupervisor.Text = dtFromReportTo.Rows[0]["REPORT_TO_3"].ToString();
                        txtFromThirdLevelSupervisorName.Text = oEmployeeTransferDataHandler.getEmpName(txtFromThirdLevelSupervisor.Text.Trim());

                        DataTable dtFromCodes = new DataTable();
                        dtFromCodes = oEmployeeTransferDataHandler.populateFromDataCodes(txtEmployeeID.Text.Trim());

                        txtFromCompanyCode.Text = dtFromCodes.Rows[0]["COMPANY_ID"].ToString();
                        txtFromCompanyName.Text = oEmployeeTransferDataHandler.getCompanyName(txtFromCompanyCode.Text.Trim());

                        txtFromDepartmentID.Text = dtFromCodes.Rows[0]["DEPT_ID"].ToString();
                        txtFromDepartmentName.Text = oEmployeeTransferDataHandler.getDepartmentName(txtFromDepartmentID.Text.Trim());

                        txtFromDivisionID.Text = dtFromCodes.Rows[0]["DIVISION_ID"].ToString();
                        txtFromDivisionName.Text = oEmployeeTransferDataHandler.getDivisionName(txtFromDivisionID.Text.Trim());

                        txtFromBranchID.Text = dtFromCodes.Rows[0]["BRANCH_ID"].ToString();
                        txtFromBranchName.Text = oEmployeeTransferDataHandler.getBranchName(txtFromBranchID.Text.Trim());

                        txtfrmEPF.Text = dtFromCodes.Rows[0]["EPF_NO"].ToString();
                        txtfrmETF.Text = dtFromCodes.Rows[0]["ETF_NO"].ToString();

                        txtfrmDesignation.Text = dtFromCodes.Rows[0]["DESIGNATION_ID"].ToString();
                        txtDesignationName.Text = oEmployeeTransferDataHandler.getDesignationName(txtfrmDesignation.Text.Trim());

                        txtFromCC.Text = dtFromCodes.Rows[0]["COST_CENTER"].ToString();
                        txtfccName.Text = oEmployeeTransferDataHandler.getCCPCName(txtFromCC.Text.Trim());

                        txtFromPC.Text = dtFromCodes.Rows[0]["PROFIT_CENTER"].ToString();
                        txtfpcName.Text = oEmployeeTransferDataHandler.getCCPCName(txtFromPC.Text.Trim());

                    }
                }
                if (hfCaller.Value == "txtToFirstLevelSupervisor")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtToFirstLevelSupervisor.Text = hfVal.Value;
                    }
                    if (txtToFirstLevelSupervisor.Text != "")
                    {
                        //Postback Methods
                        EmployeeTransferDataHandler oEmployeeTransferDataHandler = new EmployeeTransferDataHandler();
                        lbltRpt1.Text = oEmployeeTransferDataHandler.getEmpName(txtToFirstLevelSupervisor.Text.Trim());
                    }
                }
                if (hfCaller.Value == "txtToSecondLevelSupervisor")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtToSecondLevelSupervisor.Text = hfVal.Value;
                    }
                    if (txtToSecondLevelSupervisor.Text != "")
                    {
                        //Postback Methods
                        EmployeeTransferDataHandler oEmployeeTransferDataHandler = new EmployeeTransferDataHandler();
                        lbltRpt2.Text = oEmployeeTransferDataHandler.getEmpName(txtToSecondLevelSupervisor.Text.Trim());
                    }
                }
                if (hfCaller.Value == "txtToThirdLevelSupervisor")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtToThirdLevelSupervisor.Text = hfVal.Value;
                    }
                    if (txtToThirdLevelSupervisor.Text != "")
                    {
                        //Postback Methods
                        EmployeeTransferDataHandler oEmployeeTransferDataHandler = new EmployeeTransferDataHandler();
                        lbltRpt3.Text = oEmployeeTransferDataHandler.getEmpName(txtToThirdLevelSupervisor.Text.Trim());
                    }
                }








                //txtEmployeeID.Text          = hfEmpID.Value;
                //txtName.Text                = hfName.Value;
                //txtFromCompanyCode.Text     = hfCompanyCode.Value;
                //txtFromCompanyName.Text     = hfCompanyName.Value;
                //txtFromDepartmentID.Text    = hfDepartmentID.Value;
                //txtFromDepartmentName.Text  = hfDepartmentName.Value;
                //txtFromDivisionID.Text      = hfDivisionID.Value;
                //txtFromDivisionName.Text    = hfDivisionName.Value;

                //txtFromBranchID.Text        = hfBranchID.Value;
                //txtFromBranchName.Text      = hfBranchName.Value;
                //txtFromCC.Text              = hfCC.Value;
                //txtFromPC.Text              = hfPC.Value;

                //txtfrmEPF.Text              = hfEPF.Value;
                //txtfrmDesignation.Text      = hfDesignation.Value;
                //txtDesignationName.Text = hfDesigName.Value;

                //EmployeeTransferDataHandler oEmployeeTransferDataHandler = new EmployeeTransferDataHandler();
                //txtfrmETF.Text = oEmployeeTransferDataHandler.getETF(txtEmployeeID.Text.Trim());

                //txtfccName.Text = oEmployeeTransferDataHandler.getCCPC(txtFromCC.Text.Trim());
                //txtfpcName.Text = oEmployeeTransferDataHandler.getCCPC(txtFromPC.Text.Trim());


                //DataTable dtFromReportTo = new DataTable();
                //dtFromReportTo = oEmployeeTransferDataHandler.populateSupervisors(txtEmployeeID.Text.Trim());
                //txtFromFirstLevelSupervisor.Text = dtFromReportTo.Rows[0]["REPORT_TO_1"].ToString();
                //txtFromFirstLevelSupervisorName.Text = oEmployeeTransferDataHandler.getEmpName(txtFromFirstLevelSupervisor.Text.Trim());
                //txtFromSecondLevelSupervisor.Text = dtFromReportTo.Rows[0]["REPORT_TO_2"].ToString();
                //txtFromSecondLevelSupervisorName.Text = oEmployeeTransferDataHandler.getEmpName(txtFromSecondLevelSupervisor.Text.Trim());
                //txtFromThirdLevelSupervisor.Text = dtFromReportTo.Rows[0]["REPORT_TO_3"].ToString();
                //txtFromThirdLevelSupervisorName.Text = oEmployeeTransferDataHandler.getEmpName(txtFromThirdLevelSupervisor.Text.Trim());


                //populateGrid();

                //string parameter = Request["__EVENTARGUMENT"];

                //if (parameter.Equals("TextChanged"))
                //    Utility.Errorhandler.ClearError(lblMsg);
            }
                        
        }

        void checkActiveStatus()
        {
            EmployeeTransferDataHandler oEmployeeTransferDataHandler = new EmployeeTransferDataHandler();
            string statusCode = oEmployeeTransferDataHandler.getEmployeeStatus(txtEmployeeID.Text.Trim());
            if (statusCode != "ACTIVE")
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You Cannot Transfer "+statusCode + " Employees", lblMsg);
            }
            else
            {
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

                ddlToCompany.Items.Clear();

                if (dtCompanies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlToCompany.Items.Add(Item);

                    foreach (DataRow dataRow in dtCompanies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlToCompany.Items.Add(listItem);
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

        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Load only the company which end user is assigned to 
        ///</summary>
        //----------------------------------------------------------------------------------------
        private void fillCompany(string companyId)
        {
            log.Debug("fillCompanies() - companyId:" + companyId);

            CompanyDataHandler dhCompany = new CompanyDataHandler();
            DataTable dtCompanies = new DataTable();

            try
            {
                dtCompanies = dhCompany.getCompanyIdCompName(companyId).Copy();

                ddlToCompany.Items.Clear();

                if (dtCompanies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlToCompany.Items.Add(Item);

                    foreach (DataRow dataRow in dtCompanies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlToCompany.Items.Add(listItem);
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

                ddlToDepartment.Items.Clear();

                if (dtDepartments.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlToDepartment.Items.Add(Item);

                    foreach (DataRow dataRow in dtDepartments.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DEPT_NAME"].ToString();
                        listItem.Value = dataRow["DEPT_ID"].ToString();

                        ddlToDepartment.Items.Add(listItem);
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

                ddlToDivision.Items.Clear();

                if (dtDivisions.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlToDivision.Items.Add(Item);

                    foreach (DataRow dataRow in dtDivisions.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DIV_NAME"].ToString();
                        listItem.Value = dataRow["DIVISION_ID"].ToString();

                        ddlToDivision.Items.Add(listItem);
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

        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Fill Designation for a given company
        ///</summary>
        //----------------------------------------------------------------------------------------
        private void fillDesignations(string companyId)
        {
            log.Debug("fillDesignations() - companyId:" + companyId);

            DesignationEmployeeDataHandler odesignation = new DesignationEmployeeDataHandler();
            DataTable dtDesignation = new DataTable();

            try
            {
                dtDesignation = odesignation.getDesignationsForCompany(companyId).Copy();

                ddlToDesignation.Items.Clear();

                if (dtDesignation.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlToDesignation.Items.Add(Item);

                    foreach (DataRow dataRow in dtDesignation.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DESIGNATION_NAME"].ToString();
                        listItem.Value = dataRow["DESIGNATION_ID"].ToString();

                        ddlToDesignation.Items.Add(listItem);
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
                odesignation = null;
                dtDesignation.Dispose();
            }

        }

        protected void ddlToCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateToCompany();
            if (ddlToCompany.SelectedIndex == 0)
            {
                lblNextEpfNo.Text = String.Empty;
            }
                txtToEPF.Text = String.Empty;
                txtToFirstLevelSupervisor.Text = String.Empty;
                lbltRpt1.Text = String.Empty;
                txtToSecondLevelSupervisor.Text = String.Empty;
                lbltRpt2.Text = String.Empty;
                txtToThirdLevelSupervisor.Text = String.Empty;
                lbltRpt3.Text = String.Empty;
                txtStartDate.Text = String.Empty;
                txtRemarks.Text = String.Empty;
                txtToEPF.Text = String.Empty;
                txtStartDate.Text = String.Empty;
            
        }

        void populateToCompany()
        {
            log.Debug("ddlCompany_SelectedIndexChanged()");

            Utility.Errorhandler.ClearError(lblMsg);


            if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
            {
                fillDepartment(ddlToCompany.SelectedValue.Trim());
                fillBranches(ddlToCompany.SelectedValue.Trim());
                fillDesignations(ddlToCompany.SelectedValue.Trim());
                getNextEpfNo(ddlToCompany.SelectedValue.Trim());

                fillCostCenter(ddlToCompany.SelectedValue.Trim());
                fillProfitCenter(ddlToCompany.SelectedValue.Trim());

                ddlToDivision.Items.Clear();
            }

            //hfToCompCode.Value = ddlToCompany.SelectedValue.Trim();
        }

        void populateToDepartment()
        {
            if (ddlToDepartment.SelectedValue != "")
            {
                fillDivisions(ddlToDepartment.SelectedValue.Trim());
            }
        }

        protected void ddlToDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateToDepartment();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);
            clearAll();
        }

        private void clear()
        {
            //GroupHRIS.Utility.Utils.clearControls(true, txtFromCompanyCode, txtFromCompanyName, txtFromDepartmentID,
            //                                    txtFromDepartmentName, txtFromDivisionID, txtFromDivisionName, txtFromBranchID, txtFromBranchName, txtFromCC, txtFromPC,
            //                                    ddlToCompany, ddlToDepartment, ddlToDivision, ddlToBranch, ddlToCC, ddlToPC, txtStartDate, txtRemarks,txtfrmEPF,txtfrmDesignation,txtToEPF,txtDesignationName,ddlToDesignation,lblNextEpfNo);

            
            txtFromFirstLevelSupervisor.Text = String.Empty;

            txtFromFirstLevelSupervisorName.Text = String.Empty;
            txtFromSecondLevelSupervisor.Text = String.Empty;
            txtFromSecondLevelSupervisorName.Text = String.Empty;
            txtFromThirdLevelSupervisor.Text = String.Empty;
            txtFromThirdLevelSupervisorName.Text = String.Empty;
            txtFromCompanyCode.Text = String.Empty;
            txtFromCompanyName.Text = String.Empty;
            txtFromDepartmentID.Text = String.Empty;
            txtFromDepartmentName.Text = String.Empty;
            txtFromDivisionID.Text = String.Empty;

            txtFromDivisionName.Text = String.Empty;
            txtFromBranchID.Text = String.Empty;
            txtFromBranchName.Text = String.Empty;
            lblNextEpfNo.Text = String.Empty;
            txtfrmEPF.Text = String.Empty;
            txtfrmETF.Text = String.Empty;
            txtfrmDesignation.Text = String.Empty;
            txtDesignationName.Text = String.Empty;
            txtFromCC.Text = String.Empty;
            txtfccName.Text = String.Empty;
            txtFromPC.Text = String.Empty;

            txtfpcName.Text = String.Empty;
            txtToEPF.Text = String.Empty;
            txtToFirstLevelSupervisor.Text = String.Empty;
            lbltRpt1.Text = String.Empty;
            txtToSecondLevelSupervisor.Text = String.Empty;
            lbltRpt2.Text = String.Empty;
            txtToThirdLevelSupervisor.Text = String.Empty;
            lbltRpt3.Text = String.Empty;
            txtStartDate.Text = String.Empty;
            txtRemarks.Text = String.Empty;

            ddlToCompany.SelectedIndex = 0;
            ddlToDepartment.Items.Clear();
            ddlToDivision.Items.Clear();
            ddlToBranch.Items.Clear();
            ddlToDesignation.Items.Clear();
            ddlToCC.Items.Clear();
            ddlToPC.Items.Clear();



            btnSave.Text = "Save";
        }

        private void clearAll()
        {
            //GroupHRIS.Utility.Utils.clearControls(true, txtFromCompanyCode, txtFromCompanyName, txtFromDepartmentID,
            //                                    txtFromDepartmentName, txtFromDivisionID, txtFromDivisionName, txtFromBranchID, txtFromBranchName, txtFromCC, txtFromPC,
            //                                    ddlToCompany, ddlToDepartment, ddlToDivision, ddlToBranch, ddlToCC, ddlToPC, txtStartDate, txtRemarks,txtfrmEPF,txtfrmDesignation,txtToEPF,txtDesignationName,ddlToDesignation,lblNextEpfNo);

            txtEmployeeID.Text = String.Empty;
            txtName.Text = String.Empty;


            txtFromFirstLevelSupervisor.Text = String.Empty;

            txtFromFirstLevelSupervisorName.Text = String.Empty;
            txtFromSecondLevelSupervisor.Text = String.Empty;
            txtFromSecondLevelSupervisorName.Text = String.Empty;
            txtFromThirdLevelSupervisor.Text = String.Empty;
            txtFromThirdLevelSupervisorName.Text = String.Empty;
            txtFromCompanyCode.Text = String.Empty;
            txtFromCompanyName.Text = String.Empty;
            txtFromDepartmentID.Text = String.Empty;
            txtFromDepartmentName.Text = String.Empty;
            txtFromDivisionID.Text = String.Empty;

            txtFromDivisionName.Text = String.Empty;
            txtFromBranchID.Text = String.Empty;
            txtFromBranchName.Text = String.Empty;
            lblNextEpfNo.Text = String.Empty;
            txtfrmEPF.Text = String.Empty;
            txtfrmETF.Text = String.Empty;
            txtfrmDesignation.Text = String.Empty;
            txtDesignationName.Text = String.Empty;
            txtFromCC.Text = String.Empty;
            txtfccName.Text = String.Empty;
            txtFromPC.Text = String.Empty;

            txtfpcName.Text = String.Empty;
            txtToEPF.Text = String.Empty;
            txtToFirstLevelSupervisor.Text = String.Empty;
            lbltRpt1.Text = String.Empty;
            txtToSecondLevelSupervisor.Text = String.Empty;
            lbltRpt2.Text = String.Empty;
            txtToThirdLevelSupervisor.Text = String.Empty;
            lbltRpt3.Text = String.Empty;
            txtStartDate.Text = String.Empty;
            txtRemarks.Text = String.Empty;

            ddlToCompany.SelectedIndex = 0;
            ddlToDepartment.Items.Clear();
            ddlToDivision.Items.Clear();
            ddlToBranch.Items.Clear();
            ddlToDesignation.Items.Clear();
            ddlToCC.Items.Clear();
            ddlToPC.Items.Clear();

            clearGrid();

            btnSave.Text = "Save";
        }

        void clearGrid()
        {
            gvEmpTrans.DataSource = null;
            gvEmpTrans.DataBind();
        }

        bool validateSupervisor()
        {
            if ((txtToFirstLevelSupervisor.Text == txtToSecondLevelSupervisor.Text) && (txtToSecondLevelSupervisor.Text == txtToThirdLevelSupervisor.Text))
            {
                if ((txtToFirstLevelSupervisor.Text == "") && (txtToSecondLevelSupervisor.Text == "") && (txtToThirdLevelSupervisor.Text == ""))
                {
                    return true;
                }
                else
                {
                    Errorhandler.GetError("2", "First, Second and Third Level Supervisors Cannot be Identical", lblMsg);
                    return false;
                }
            }
            else if (txtToFirstLevelSupervisor.Text == txtToSecondLevelSupervisor.Text)
            {
                if ((txtToFirstLevelSupervisor.Text == "") && (txtToSecondLevelSupervisor.Text == ""))
                {
                    return true;
                }
                else
                {
                    Errorhandler.GetError("2", "First and Second Level Supervisors Cannot be Identical", lblMsg);
                    return false;
                }
            }
            else if (txtToSecondLevelSupervisor.Text == txtToThirdLevelSupervisor.Text)
            {
                if ((txtToSecondLevelSupervisor.Text == "") && (txtToThirdLevelSupervisor.Text == ""))
                {
                    return true;
                }
                else
                {
                    Errorhandler.GetError("2", "Second and Third Level Supervisors Cannot be Identical", lblMsg);
                    return false;
                }
            }
            else if (txtToFirstLevelSupervisor.Text == txtToThirdLevelSupervisor.Text)
            {
                if ((txtToFirstLevelSupervisor.Text == "") && (txtToThirdLevelSupervisor.Text == ""))
                {
                    return true;
                }
                else
                {
                    Errorhandler.GetError("2", "First and Third Level Supervisors Cannot be Identical", lblMsg);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            EmployeeDataHandler dhEmployee = new EmployeeDataHandler();
            EmployeeTransferDataHandler dhEmpTrans = new EmployeeTransferDataHandler();

            Utility.Errorhandler.ClearError(lblMsg);

            if ((txtToSecondLevelSupervisor.Text != "") || (txtToThirdLevelSupervisor.Text != ""))
            {
                if (txtToFirstLevelSupervisor.Text == "")
                {
                    txtToSecondLevelSupervisor.Text = txtToThirdLevelSupervisor.Text = txtToFirstLevelSupervisor.Text = String.Empty;
                    lbltRpt1.Text = lbltRpt2.Text = lbltRpt3.Text = String.Empty;
                    Errorhandler.GetError("2", "FirstLevel Supervisor is Required for Assign Second or Thired level Supervisors", lblMsg);
                    return;
                }
            }

            if (validateSupervisor() == false)
            {
                return;
            }


            if (btnSave.Text == "Update")
            {
                EmployeeTransferDataHandler oEmployeeTransferDataHandler = new EmployeeTransferDataHandler();
                try
                {
                    string transferID = gvEmpTrans.Rows[gvEmpTrans.SelectedIndex].Cells[0].Text;
                    string date = txtStartDate.Text.Trim();
                    string[] dateArr = date.Split('/');
                    date = dateArr[2] + "/" + dateArr[1] + "/" + dateArr[0];

                    string loggeduser = (string)Session["KeyUSER_ID"];
                    oEmployeeTransferDataHandler.update(txtEmployeeID.Text.Trim(), transferID.Trim(), ddlToCompany.SelectedValue.Trim(), ddlToDepartment.SelectedValue.Trim(), ddlToDivision.SelectedValue.Trim(), ddlToBranch.SelectedValue.Trim(), txtToEPF.Text.Trim(), txtToEPF.Text.Trim(), ddlToDesignation.SelectedValue.Trim(), ddlToCC.SelectedValue.Trim(), ddlToPC.SelectedValue.Trim(), txtToFirstLevelSupervisor.Text.Trim(), txtToSecondLevelSupervisor.Text.Trim(), txtToThirdLevelSupervisor.Text.Trim(), date.Trim(), txtRemarks.Text.Trim(), loggeduser);

                    clear();
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED, lblMsg);
                    btnSave.Text = "Save";
                    populateGrid();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return;
            }


            try
            {
                //Check whether the employee is Active or not
                if (!dhEmployee.isActiveEmployee(txtEmployeeID.Text.Trim()))
                {
                    lblMsg.Text = String.Empty;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "This employee is not in ACTIVE state.", lblMsg);
                }
                else
                {

                    string date = txtStartDate.Text.Trim();
                    string[] dateArr = date.Split('/');
                    date = dateArr[2] + "/" + dateArr[1] + "/" + dateArr[0];

                    dhEmpTrans.Insert(txtEmployeeID.Text.Trim(),
                                        txtFromCompanyCode.Text.Trim(),
                                        txtFromDepartmentID.Text.Trim(),
                                        txtFromDivisionID.Text.Trim(),
                                        ddlToCompany.SelectedItem.Value,
                                        ddlToDepartment.SelectedItem.Value,
                                        ddlToDivision.SelectedItem.Value,
                                        date,
                                        txtRemarks.Text.Trim(),
                                        userID,
                                        Constants.TRANSFER_TYPE_ORDINARY,
                                        txtFromBranchID.Text.Trim(),
                                        txtFromCC.Text.Trim(),
                                        txtFromPC.Text.Trim(),
                                        ddlToBranch.SelectedItem.Value,
                                        ddlToCC.SelectedValue.Trim(),
                                        ddlToPC.SelectedValue.Trim(),
                                        txtfrmEPF.Text.Trim(),
                                        txtfrmDesignation.Text.Trim(),
                                        txtToEPF.Text.Trim(),
                                        ddlToDesignation.SelectedItem.Value,
                                        txtfrmETF.Text.Trim(), 
                                        txtFromFirstLevelSupervisor.Text.Trim(),
                                        txtFromSecondLevelSupervisor.Text.Trim(),
                                        txtFromThirdLevelSupervisor.Text.Trim(),
                                        txtToFirstLevelSupervisor.Text.Trim(),
                                        txtToSecondLevelSupervisor.Text.Trim(),
                                        txtToThirdLevelSupervisor.Text.Trim());

                    clear();
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED, lblMsg);
                }
                populateGrid();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                dhEmpTrans = null;
            }
        }

        private void populateGrid()
        {
            EmployeeTransferDataHandler dhEmpTrans = new EmployeeTransferDataHandler();
            DataTable dtEmpTrans = new DataTable();

            try
            {
                dtEmpTrans = dhEmpTrans.populate(txtEmployeeID.Text);

                gvEmpTrans.DataSource = dtEmpTrans;
                gvEmpTrans.DataBind();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //lblerror.Text = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
            finally
            {
                dhEmpTrans = null;
                dtEmpTrans.Dispose();
            }
        }

        protected void ibtnGet_Click(object sender, ImageClickEventArgs e)
        {
            txtToEPF.Text = lblNextEpfNo.Text.Trim();
        }

        private void getNextEpfNo(String companyId)
        {
            log.Debug("getNextEpfNo() - companyId:" + companyId);
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            try
            {
                lblNextEpfNo.Text = "";
                lblNextEpfNo.Text = employeeDataHandler.getNextEpfNo(companyId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                employeeDataHandler = null;
            }

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

        protected void gvEmpTrans_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMsg);

            string transferID = gvEmpTrans.Rows[gvEmpTrans.SelectedIndex].Cells[0].Text;

            int[] tIDArr = new int[gvEmpTrans.Rows.Count];

            for (int i = 0; i < gvEmpTrans.Rows.Count; i++)
            {
                tIDArr[i] = Convert.ToInt32(gvEmpTrans.Rows[i].Cells[0].Text.Trim());
            }

            if (Convert.ToInt32(transferID) == tIDArr.Max())
            {
                EmployeeTransferDataHandler oEmployeeTransferDataHandler = new EmployeeTransferDataHandler();
                DataTable dt = new DataTable();
                DataTable dtfrom = new DataTable();
                dtfrom = oEmployeeTransferDataHandler.populateFromtransferDetails(transferID.Trim()).Copy();
                dt = oEmployeeTransferDataHandler.populateTotransferDetails(transferID.Trim()).Copy();

                if ((dtfrom.Rows.Count > 0) && (dt.Rows.Count > 0))
                {
                    //From Data

                    txtEmployeeID.Text = dtfrom.Rows[0]["EMPLOYEE_ID"].ToString();
                    //txtName.Text = dtfrom.Rows[0]["EMP_NAME"].ToString();
                    txtName.Text = oEmployeeTransferDataHandler.getEmpName(txtEmployeeID.Text);

                    txtFromFirstLevelSupervisor.Text = dtfrom.Rows[0]["FROM_RPT_1"].ToString();
                    //txtFromFirstLevelSupervisorName.Text = dtfrom.Rows[0]["FROM_RPT_1_NAME"].ToString();
                    txtFromFirstLevelSupervisorName.Text = oEmployeeTransferDataHandler.getEmpName(txtFromFirstLevelSupervisor.Text);

                    txtFromSecondLevelSupervisor.Text = dtfrom.Rows[0]["FROM_RPT_2"].ToString();
                    //txtFromSecondLevelSupervisorName.Text = dtfrom.Rows[0]["FROM_RPT_2_NAME"].ToString();
                    txtFromSecondLevelSupervisorName.Text = oEmployeeTransferDataHandler.getEmpName(txtFromSecondLevelSupervisor.Text);

                    txtFromThirdLevelSupervisor.Text = dtfrom.Rows[0]["FROM_RPT_3"].ToString();
                    //txtFromThirdLevelSupervisorName.Text = dtfrom.Rows[0]["FROM_RPT_3_NAME"].ToString();
                    txtFromThirdLevelSupervisorName.Text = oEmployeeTransferDataHandler.getEmpName(txtFromThirdLevelSupervisor.Text);

                    txtFromCompanyCode.Text = dtfrom.Rows[0]["FROM_COMPANY_ID"].ToString();
                    txtFromCompanyName.Text = dtfrom.Rows[0]["FROM_COMPANY_NAME"].ToString();

                    txtFromDepartmentID.Text = dtfrom.Rows[0]["FROM_DEPT_ID"].ToString();
                    txtFromDepartmentName.Text = dtfrom.Rows[0]["FROM_DEPT_NAME"].ToString();

                    txtFromDivisionID.Text = dtfrom.Rows[0]["FROM_DIVISION_ID"].ToString();
                    txtFromDivisionName.Text = dtfrom.Rows[0]["FROM_DIVISION_NAME"].ToString();

                    txtFromBranchID.Text = dtfrom.Rows[0]["FROM_BRANCH_ID"].ToString();
                    txtFromBranchName.Text = dtfrom.Rows[0]["FROM_BRANCH_NAME"].ToString();

                    txtfrmEPF.Text = dtfrom.Rows[0]["FROM_EPF"].ToString();
                    txtfrmETF.Text = dtfrom.Rows[0]["FROM_ETF"].ToString();

                    txtfrmDesignation.Text = dtfrom.Rows[0]["FROM_DESIGNATION"].ToString();
                    txtDesignationName.Text = dtfrom.Rows[0]["FROM_DESIGNATION_NAME"].ToString();

                    txtFromCC.Text = dtfrom.Rows[0]["FROM_CC"].ToString();
                    txtfccName.Text = dtfrom.Rows[0]["FROM_CC_NAME"].ToString();

                    txtFromPC.Text = dtfrom.Rows[0]["FROM_PC"].ToString();
                    txtfpcName.Text = dtfrom.Rows[0]["FROM_PC_NAME"].ToString();

                    //To Data

                    string compid = dt.Rows[0]["TO_COMPANY_ID"].ToString();
                    string deptid = dt.Rows[0]["TO_DEPT_ID"].ToString();
                    string divid = dt.Rows[0]["TO_DIVISION_ID"].ToString();
                    string brnchid = dt.Rows[0]["TO_BRANCH_ID"].ToString();
                    string designationid = dt.Rows[0]["TO_DESIGNATION"].ToString();
                    string tocc = dt.Rows[0]["TO_CC"].ToString();
                    string topc = dt.Rows[0]["TO_PC"].ToString();
                    
                    
                    ddlToCompany.SelectedIndex = ddlToCompany.Items.IndexOf(ddlToCompany.Items.FindByValue(compid));                    
                    populateToCompany();
                    ddlToDepartment.SelectedIndex = ddlToDepartment.Items.IndexOf(ddlToDepartment.Items.FindByValue(deptid));
                    populateToDepartment();
                    ddlToDivision.SelectedIndex = ddlToDivision.Items.IndexOf(ddlToDivision.Items.FindByValue(divid));
                    ddlToBranch.SelectedIndex = ddlToBranch.Items.IndexOf(ddlToBranch.Items.FindByValue(brnchid));
                    ddlToDesignation.SelectedIndex = ddlToDesignation.Items.IndexOf(ddlToDesignation.Items.FindByValue(designationid));
                    ddlToCC.SelectedIndex = ddlToCC.Items.IndexOf(ddlToCC.Items.FindByValue(tocc));
                    ddlToPC.SelectedIndex = ddlToPC.Items.IndexOf(ddlToPC.Items.FindByValue(topc));                    
                    txtToEPF.Text = dt.Rows[0]["TO_EPF"].ToString();
                    txtToFirstLevelSupervisor.Text = dt.Rows[0]["TO_RPT_1"].ToString();
                    //lbltRpt1.Text = dt.Rows[0]["TO_RPT_1_NAME"].ToString();
                    lbltRpt1.Text = oEmployeeTransferDataHandler.getEmpName(txtToFirstLevelSupervisor.Text);
                    txtToSecondLevelSupervisor.Text = dt.Rows[0]["TO_RPT_2"].ToString();
                    //lbltRpt2.Text = dt.Rows[0]["TO_RPT_2_NAME"].ToString();
                    lbltRpt2.Text = oEmployeeTransferDataHandler.getEmpName(txtToSecondLevelSupervisor.Text);
                    txtToThirdLevelSupervisor.Text = dt.Rows[0]["TO_RPT_3"].ToString();
                    //lbltRpt3.Text = dt.Rows[0]["TO_RPT_3_NAME"].ToString();
                    lbltRpt3.Text = oEmployeeTransferDataHandler.getEmpName(txtToThirdLevelSupervisor.Text);

                    string date = dt.Rows[0]["START_DATE"].ToString();
                    string[] dateArr = date.Split('-');
                    date = dateArr[2] + "/" + dateArr[1] + "/" + dateArr[0];

                    txtStartDate.Text = date;



                    txtRemarks.Text = dt.Rows[0]["REMARKS"].ToString();

                    btnSave.Text = "Update";
                }
                else
                {
                    Errorhandler.GetError("0", "No Data Record(s)", lblMsg);
                }
            }
            else
            {
                Errorhandler.GetError("2", "You Can Update Only Latest Transfer Record", lblMsg);
            }
        }

        protected void gvEmpTrans_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvEmpTrans, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        protected void ddlToDivision_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}