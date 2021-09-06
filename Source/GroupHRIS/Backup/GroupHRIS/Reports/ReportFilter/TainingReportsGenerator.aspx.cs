using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using Common;
using System.Data;
using DataHandler.Reports;
using System.Reflection;

namespace GroupHRIS.Reports.ReportFilter
{
    public partial class TainingReportsGenerator : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        #region events

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "TainingReportsGenerator : Page_Load");

            if (!IsPostBack)
            {
                hfReportCode.Value = Session["SelectedReportCode"].ToString();
                fillCompanyDropdown();
                fillFinancialYears();
                disableUnwantedFilters(Session["SelectedReportCode"].ToString());
            }
            else
            {
                if (hfCaller.Value == "txtRecPerson")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtEmployeeID.Text = hfVal.Value;
                    }
                    //if (hfActivatorUserId.Value.ToString() != "")
                    //{
                    //    //Postback Methods

                    //}
                    hfVal.Value = "";
                }
            }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("TainingReportsGenerator : ddlCompany_SelectedIndexChanged()");
                fillDepartmentDropdown();
                fillBranchDropdown();
            }
            catch (Exception ex)
            {
                log.Debug("TainingReportsGenerator : ddlCompany_SelectedIndexChanged()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("TainingReportsGenerator : btnGenerate_Click()");

                string companyId = ddlCompany.SelectedValue.ToString();
                string departmentId = ddlDepartment.SelectedValue.ToString();
                string divisionId = ddlDivision.SelectedValue.ToString();
                string branchId = ddlBranch.SelectedValue.ToString();
                string financialYear = ddlFinancialYear.SelectedValue.ToString();
                string employeeId = txtEmployeeID.Text.ToString();

                string departmentHeadStatus = String.Empty;
                if (rbDeptHeadRecommend.Checked == true)
                {
                    departmentHeadStatus = Constants.STATUS_ACTIVE_VALUE;
                }
                else if (rbDeptHeadReject.Checked == true)
                {
                    departmentHeadStatus = Constants.STATUS_INACTIVE_VALUE;
                }

                string ceoStatus = String.Empty;
                if (rbCEORecommend.Checked == true)
                {
                    ceoStatus = Constants.STATUS_ACTIVE_VALUE;
                }
                else if (rbCEORecommend.Checked == true)
                {
                    ceoStatus = Constants.STATUS_INACTIVE_VALUE;
                }

                string trainingStatus = ddlTrainingStatus.SelectedValue.ToString();
                string fromDate = txtFromDate.Text.ToString();
                string toDate = txtToDate.Text.ToString();

                Dictionary<string, string> filterParameters = new Dictionary<string, string>();
                filterParameters.Add("companyId", companyId);
                filterParameters.Add("departmentId", departmentId);
                filterParameters.Add("divisionId", divisionId);
                filterParameters.Add("branchId", branchId);
                filterParameters.Add("financialYear", financialYear);
                filterParameters.Add("employeeId", employeeId);
                filterParameters.Add("departmentHeadStatus", departmentHeadStatus);
                filterParameters.Add("ceoStatus", ceoStatus);
                filterParameters.Add("trainingStatus", trainingStatus);
                filterParameters.Add("fromDate", fromDate);
                filterParameters.Add("toDate", toDate);

                Session["Filters"] = filterParameters;

                generateReport(hfReportCode.Value.ToString());
                
            }
            catch (Exception ex)
            {
                log.Debug("TainingReportsGenerator : btnGenerate_Click()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("TainingReportsGenerator : ddlDepartment_SelectedIndexChanged()");
                fillDivisionDropdown();
            }
            catch (Exception ex)
            {
                log.Debug("TainingReportsGenerator : ddlDepartment_SelectedIndexChanged()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }


        #endregion

        #region methodes

        private void fillCompanyDropdown()
        {
            log.Debug("TainingReportsGenerator : fillCompanyDropdown()");
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtCompany = new DataTable();
            try
            {

                dtCompany = reportDataHandler.getAllCompany();
                if (dtCompany.Rows.Count > 0)
                {
                    ListItem emptyItem = new ListItem();
                    emptyItem.Value = "";
                    emptyItem.Text = "";
                    ddlCompany.Items.Add(emptyItem);

                    foreach (DataRow company in dtCompany.Rows)
                    {
                        ddlCompany.Items.Add(new ListItem(company["COMP_NAME"].ToString(), company["COMPANY_ID"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("TainingReportsGenerator : fillCompanyDropdown()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtCompany.Dispose();
                reportDataHandler = null;
            }
        }

        private void fillDepartmentDropdown()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtDepartments = new DataTable();
            try
            {
                log.Debug("TainingReportsGenerator : fillDepartmentDropdown()");

                ddlDepartment.Items.Clear();

                string selectedCompany = ddlCompany.SelectedValue.ToString();
                dtDepartments = reportDataHandler.populateDepartments(selectedCompany);

                ddlDepartment.Items.Add(new ListItem("", ""));

                if (dtDepartments.Rows.Count > 0)
                {
                    foreach (DataRow department in dtDepartments.Rows)
                    {
                        ddlDepartment.Items.Add(new ListItem(department["DEPT_NAME"].ToString(), department["DEPT_ID"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("TainingReportsGenerator : fillDepartmentDropdown()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtDepartments.Dispose();
                reportDataHandler = null;
            }
        }

        private void fillBranchDropdown()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtBranches = new DataTable();
            try
            {
                log.Debug("TainingReportsGenerator : fillBranchDropdown()");

                ddlBranch.Items.Clear();

                string selectedCompany = ddlCompany.SelectedValue.ToString();
                dtBranches = reportDataHandler.populateBranch(selectedCompany);

                ddlBranch.Items.Add(new ListItem("", ""));

                if (dtBranches.Rows.Count > 0)
                {
                    foreach (DataRow branch in dtBranches.Rows)
                    {
                        ddlBranch.Items.Add(new ListItem(branch["BRANCH_NAME"].ToString(), branch["BRANCH_ID"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("TainingReportsGenerator : fillBranchDropdown()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtBranches.Dispose();
                reportDataHandler = null;
            }
        }

        private void fillDivisionDropdown()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtDivisions = new DataTable();
            try
            {
                log.Debug("TainingReportsGenerator : fillDivisionDropdown()");

                ddlDivision.Items.Clear();

                string selectedDepartment = ddlDepartment.SelectedValue.ToString();
                dtDivisions = reportDataHandler.populateDivisions(selectedDepartment);

                ddlDivision.Items.Add(new ListItem("", ""));

                if (dtDivisions.Rows.Count > 0)
                {
                    foreach (DataRow division in dtDivisions.Rows)
                    {
                        ddlDivision.Items.Add(new ListItem(division["DIV_NAME"].ToString(), division["DIVISION_ID"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("TainingReportsGenerator : fillDivisionDropdown()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtDivisions.Dispose();
                reportDataHandler = null;
            }
        }

        private void fillFinancialYears()
        {
            try
            {
                log.Debug("TainingReportsGenerator : fillFinancialYears()");
                int currentYear = Convert.ToInt16(CommonUtils.currentFinancialYear());

                ddlFinancialYear.Items.Add(new ListItem("", ""));

                for (int i = currentYear - 10; i < currentYear + 1; i++)
                {
                    ddlFinancialYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    
                }
            }
            catch (Exception ex)
            {
                log.Debug("TainingReportsGenerator : fillFinancialYears()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        private void disableUnwantedFilters(string reportCode)
        {
            try
            {
                log.Debug("TainingReportsGenerator : hideUnwantedFilters()");

                if (reportCode == "RE036")
                {
                    rbCEORecommend.Enabled = false;
                    rbCEOReject.Enabled = false;
                    rbDeptHeadRecommend.Enabled = false;
                    rbDeptHeadReject.Enabled = false;
                    ddlFinancialYear.Enabled = false;
                    ddlTrainingStatus.Enabled = false;
                    //txtFromDate.Enabled = false;
                    //txtToDate.Enabled = false;
                    ddlFinancialYear.Enabled = false;
                }

                if (reportCode == "RE037")
                {
                    rbCEORecommend.Enabled = false;
                    rbCEOReject.Enabled = false;
                    rbDeptHeadRecommend.Enabled = false;
                    rbDeptHeadReject.Enabled = false;
                    ddlFinancialYear.Enabled = false;
                    ddlTrainingStatus.Enabled = false;
                    //txtFromDate.Enabled = false;
                    //txtToDate.Enabled = false;
                    ddlFinancialYear.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                log.Debug("TainingReportsGenerator : hideUnwantedFilters()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        private void generateReport(string reportCode)
        {
            try
            {
                log.Debug("TainingReportsGenerator : generateReport()");

                Type thisType = this.GetType();
                //Type thisType = typeof(TainingReportsGenerator);
                MethodInfo theMethod = thisType.GetMethod(reportCode);
                theMethod.Invoke(this, null);
            }
            catch (Exception ex)
            {
                log.Debug("TainingReportsGenerator : generateReport()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        public void RE034()
        {
            try
            {
                log.Debug("TainingReportsGenerator : RE034()");

                string headerpara = "Training Needs Report";
                string subheaderpara = "";
                string Lawersubheaderpara = "";

                ReportDataHandler reportDataHandler = new ReportDataHandler();
                DataTable dtResult = new DataTable();
                DataTable dtTrainingNeeds = new DataTable();

                Dictionary<string, string> filterParameters = (Dictionary<string, string>)(Session["Filters"]);

                string companyId = filterParameters["companyId"];
                string departmentId = filterParameters["departmentId"];
                string divisionId = filterParameters["divisionId"];
                string branchId = filterParameters["branchId"];
                string financialYear = filterParameters["financialYear"];
                string employeeId = filterParameters["employeeId"];
                //string divisionId = 
                string fromDate = filterParameters["fromDate"];
                string toDate = filterParameters["toDate"];
                string departmentHeadStatus = filterParameters["departmentHeadStatus"];
                string ceoStatus = filterParameters["ceoStatus"];


                if (!String.IsNullOrEmpty(companyId))
                {
                    string companyName = String.Empty;
                    if (companyId != Constants.CON_UNIVERSAL_COMPANY_CODE)
                    {
                        //company = dpCompID.SelectedValue.ToString();
                        companyName = reportDataHandler.populateCompanyName(companyId);
                    }
                    else
                    {
                        companyName = "All Companies ";
                    }
                    subheaderpara = " Company : " + companyName + " ";
                }
                else
                {
                    subheaderpara = " Company : All Companies";
                    companyId = "";
                }

                if (!String.IsNullOrEmpty(departmentId))
                {
                    dtResult = reportDataHandler.populateDepartmentName(departmentId);
                    string departmentName = dtResult.Rows[0][0].ToString();

                    subheaderpara += "| Department : " + departmentName + " ";

                }
                else
                {
                    subheaderpara += " | Department : All Departments";
                    departmentId = "";
                }

                if (!String.IsNullOrEmpty(divisionId))
                {
                    dtResult = reportDataHandler.populateDivisionName(divisionId);
                    string divisionName = dtResult.Rows[0][0].ToString();

                    subheaderpara += " | Division : " + divisionName + " ";

                }
                else
                {
                    subheaderpara += " | Division : All Divisions";
                    departmentId = "";
                }

                if (!String.IsNullOrEmpty(branchId))
                {
                    dtResult = reportDataHandler.populateBranchName(branchId);
                    string branchName = dtResult.Rows[0][0].ToString();

                    subheaderpara += " | Branch : " + branchName + " ";

                }
                else
                {
                    subheaderpara += " | Branch : All Branches";
                    departmentId = "";
                }

                if (!String.IsNullOrEmpty(financialYear))
                {

                    subheaderpara += " | Financial Year : " + financialYear + " ";
                }

                if (!String.IsNullOrEmpty(employeeId))
                {
                    dtResult = reportDataHandler.populateEmployeeName(employeeId);
                    string employeeName = dtResult.Rows[0][0].ToString();

                    subheaderpara += "| Requested By : " + employeeName + " ";
                }

                if (!String.IsNullOrEmpty(departmentHeadStatus))
                {
                    string status = "";
                    if (departmentHeadStatus == Constants.CON_ACTIVE_STATUS)
                    {
                        status = " Recommended";
                    }
                    else if (departmentHeadStatus == Constants.CON_INACTIVE_STATUS)
                    {
                        status = " Rejected";
                    }
                    Lawersubheaderpara += " | Department Head Status : " + status + " ";
                }
                else
                {
                    departmentHeadStatus = "";
                }

                if (!String.IsNullOrEmpty(ceoStatus))
                {
                    string status = "";
                    if (ceoStatus == Constants.CON_ACTIVE_STATUS)
                    {
                        status = " Recommended";
                    }
                    else if (ceoStatus == Constants.CON_INACTIVE_STATUS)
                    {
                        status = " Rejected";
                    }
                    Lawersubheaderpara += " | CEO Head Status : " + status + " ";
                }
                else
                {
                    ceoStatus = "";
                }

                if (!String.IsNullOrEmpty(fromDate))
                {
                    Lawersubheaderpara = " From : " + fromDate + " ";
                }
                else
                {
                    fromDate = "";
                }

                if (!String.IsNullOrEmpty(toDate))
                {
                    Lawersubheaderpara += " | To : " + toDate + "  ";
                }
                else
                {
                    toDate = "";
                }

                


                dtTrainingNeeds = reportDataHandler.getTrainingRequest(companyId, departmentId, divisionId, branchId, financialYear, employeeId, fromDate, toDate, departmentHeadStatus, ceoStatus);

                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("headerpara", headerpara);
                paramdict.Add("subheaderpara", subheaderpara);
                paramdict.Add("Lawersubheaderpara", Lawersubheaderpara);

                Session["rptDataSet"] = dtTrainingNeeds;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = " Training Needs Report";

                Response.Redirect("~/Reports/ReportViewers/rptvTrainingNeeds.aspx");
            }
            catch (Exception ex)
            {
                log.Debug("TainingReportsGenerator : RE034()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        public void RE035()
        {
            try
            {
                log.Debug("TainingReportsGenerator : RE035()");

                string headerpara = "Training Schedule Report";
                string subheaderpara = "";
                string Lawersubheaderpara = "";

                ReportDataHandler reportDataHandler = new ReportDataHandler();
                DataTable dtResult = new DataTable();
                DataTable dtTrainingNeeds = new DataTable();

                Dictionary<string, string> filterParameters = (Dictionary<string, string>)(Session["Filters"]);

                string companyId = filterParameters["companyId"];
                string departmentId = filterParameters["departmentId"];
                string divisionId = filterParameters["divisionId"];
                string branchId = filterParameters["branchId"];
                string financialYear = filterParameters["financialYear"];
                string employeeId = filterParameters["employeeId"];
                //string divisionId = 
                string fromDate = filterParameters["fromDate"];
                string toDate = filterParameters["toDate"];
                string departmentHeadStatus = filterParameters["departmentHeadStatus"];
                string ceoStatus = filterParameters["ceoStatus"];


                if (!String.IsNullOrEmpty(companyId))
                {
                    string companyName = String.Empty;
                    if (companyId != Constants.CON_UNIVERSAL_COMPANY_CODE)
                    {
                        //company = dpCompID.SelectedValue.ToString();
                        companyName = reportDataHandler.populateCompanyName(companyId);
                    }
                    else
                    {
                        companyName = "All Companies ";
                    }
                    subheaderpara = " Company : " + companyName + " ";
                }
                else
                {
                    subheaderpara = " Company : All Companies";
                    companyId = "";
                }

                if (!String.IsNullOrEmpty(departmentId))
                {
                    dtResult = reportDataHandler.populateDepartmentName(departmentId);
                    string departmentName = dtResult.Rows[0][0].ToString();

                    subheaderpara += "| Department : " + departmentName + " ";

                }
                else
                {
                    subheaderpara += " | Department : All Departments";
                    departmentId = "";
                }

                if (!String.IsNullOrEmpty(divisionId))
                {
                    dtResult = reportDataHandler.populateDivisionName(divisionId);
                    string divisionName = dtResult.Rows[0][0].ToString();

                    subheaderpara += " | Division : " + divisionName + " ";

                }
                else
                {
                    subheaderpara += " | Division : All Divisions";
                    divisionId = "";
                }

                if (!String.IsNullOrEmpty(branchId))
                {
                    dtResult = reportDataHandler.populateBranchName(branchId);
                    string branchName = dtResult.Rows[0][0].ToString();

                    subheaderpara += " | Branch : " + branchName + " ";

                }
                else
                {
                    subheaderpara += " | Branch : All Branches";
                    branchId = "";
                }

                if (!String.IsNullOrEmpty(financialYear))
                {

                    subheaderpara += " | Financial Year : " + financialYear + " ";
                }

                if (!String.IsNullOrEmpty(employeeId))
                {
                    dtResult = reportDataHandler.populateEmployeeName(employeeId);
                    string employeeName = dtResult.Rows[0][0].ToString();

                    subheaderpara += "| Requested By : " + employeeName + " ";
                }

                if (!String.IsNullOrEmpty(departmentHeadStatus))
                {
                    string status = "";
                    if (departmentHeadStatus == Constants.CON_ACTIVE_STATUS)
                    {
                        status = " Recommended";
                    }
                    else if (departmentHeadStatus == Constants.CON_INACTIVE_STATUS)
                    {
                        status = " Rejected";
                    }
                    Lawersubheaderpara += " | Department Head Status : " + status + " ";
                }
                else
                {
                    departmentHeadStatus = "";
                }

                if (!String.IsNullOrEmpty(ceoStatus))
                {
                    string status = "";
                    if (ceoStatus == Constants.CON_ACTIVE_STATUS)
                    {
                        status = " Recommended";
                    }
                    else if (ceoStatus == Constants.CON_INACTIVE_STATUS)
                    {
                        status = " Rejected";
                    }
                    Lawersubheaderpara += " | CEO Head Status : " + status + " ";
                }
                else
                {
                    ceoStatus = "";
                }

                if (!String.IsNullOrEmpty(fromDate))
                {
                    Lawersubheaderpara = " From : " + fromDate + " ";
                }
                else
                {
                    fromDate = "";
                }

                if (!String.IsNullOrEmpty(toDate))
                {
                    Lawersubheaderpara += " | To : " + toDate + "  ";
                }
                else
                {
                    toDate = "";
                }

                dtTrainingNeeds = reportDataHandler.getTrainingSchedules(companyId, departmentId, divisionId, branchId, financialYear, employeeId, fromDate, toDate, departmentHeadStatus, ceoStatus);

                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("headerpara", headerpara);
                paramdict.Add("subheaderpara", subheaderpara);
                paramdict.Add("Lawersubheaderpara", Lawersubheaderpara);

                Session["rptDataSet"] = dtTrainingNeeds;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = " Training Schedule Report";

                Response.Redirect("~/Reports/ReportViewers/rptvTrainingSchedule.aspx");
            }
            catch (Exception ex)
            {
                log.Debug("TainingReportsGenerator : RE035()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        public void RE036()
        {
            try
            {
                log.Debug("TainingReportsGenerator : RE036()");

                string headerpara = "Completed Trainings Report";
                string subheaderpara = "";
                string Lawersubheaderpara = "";

                ReportDataHandler reportDataHandler = new ReportDataHandler();
                DataTable dtResult = new DataTable();
                DataTable dtCompletedTrainings = new DataTable();

                Dictionary<string, string> filterParameters = (Dictionary<string, string>)(Session["Filters"]);

                string companyId = filterParameters["companyId"];
                string departmentId = filterParameters["departmentId"];
                string divisionId = filterParameters["divisionId"];
                string branchId = filterParameters["branchId"];
                string financialYear = filterParameters["financialYear"];
                string employeeId = filterParameters["employeeId"];
                //string divisionId = 
                string fromDate = filterParameters["fromDate"];
                string toDate = filterParameters["toDate"];
                string departmentHeadStatus = filterParameters["departmentHeadStatus"];
                string ceoStatus = filterParameters["ceoStatus"];


                if (!String.IsNullOrEmpty(companyId))
                {
                    string companyName = String.Empty;
                    if (companyId != Constants.CON_UNIVERSAL_COMPANY_CODE)
                    {
                        //company = dpCompID.SelectedValue.ToString();
                        companyName = reportDataHandler.populateCompanyName(companyId);
                    }
                    else
                    {
                        companyName = "All Companies ";
                    }
                    subheaderpara = " Company : " + companyName + " ";
                }
                else
                {
                    subheaderpara = " Company : All Companies";
                    companyId = "";
                }

                if (!String.IsNullOrEmpty(departmentId))
                {
                    dtResult = reportDataHandler.populateDepartmentName(departmentId);
                    string departmentName = dtResult.Rows[0][0].ToString();

                    subheaderpara += "| Department : " + departmentName + " ";

                }
                else
                {
                    subheaderpara += " | Department : All Departments";
                    departmentId = "";
                }

                if (!String.IsNullOrEmpty(divisionId))
                {
                    dtResult = reportDataHandler.populateDivisionName(divisionId);
                    string divisionName = dtResult.Rows[0][0].ToString();

                    subheaderpara += " | Division : " + divisionName + " ";

                }
                else
                {
                    subheaderpara += " | Division : All Divisions";
                    divisionId = "";
                }

                if (!String.IsNullOrEmpty(branchId))
                {
                    dtResult = reportDataHandler.populateBranchName(branchId);
                    string branchName = dtResult.Rows[0][0].ToString();

                    subheaderpara += " | Branch : " + branchName + " ";

                }
                else
                {
                    subheaderpara += " | Branch : All Branches";
                    branchId = "";
                }

                if (!String.IsNullOrEmpty(financialYear))
                {

                    subheaderpara += " | Financial Year : " + financialYear + " ";
                }

                if (!String.IsNullOrEmpty(employeeId))
                {
                    dtResult = reportDataHandler.populateEmployeeName(employeeId);
                    string employeeName = dtResult.Rows[0][0].ToString();

                    subheaderpara += "| Requested By : " + employeeName + " ";
                }

                if (!String.IsNullOrEmpty(departmentHeadStatus))
                {
                    string status = "";
                    if (departmentHeadStatus == Constants.CON_ACTIVE_STATUS)
                    {
                        status = " Recommended";
                    }
                    else if (departmentHeadStatus == Constants.CON_INACTIVE_STATUS)
                    {
                        status = " Rejected";
                    }
                    Lawersubheaderpara += " | Department Head Status : " + status + " ";
                }
                else
                {
                    departmentHeadStatus = "";
                }

                if (!String.IsNullOrEmpty(ceoStatus))
                {
                    string status = "";
                    if (ceoStatus == Constants.CON_ACTIVE_STATUS)
                    {
                        status = " Recommended";
                    }
                    else if (ceoStatus == Constants.CON_INACTIVE_STATUS)
                    {
                        status = " Rejected";
                    }
                    Lawersubheaderpara += " | CEO Head Status : " + status + " ";
                }
                else
                {
                    ceoStatus = "";
                }

                if (!String.IsNullOrEmpty(fromDate))
                {
                    Lawersubheaderpara = " From : " + fromDate + " ";
                }
                else
                {
                    fromDate = "";
                }

                if (!String.IsNullOrEmpty(toDate))
                {
                    Lawersubheaderpara += " | To : " + toDate + "  ";
                }
                else
                {
                    toDate = "";
                }

                dtCompletedTrainings = reportDataHandler.getCompletedTraining(companyId, departmentId, divisionId, branchId, financialYear, employeeId, fromDate, toDate, departmentHeadStatus, ceoStatus);

                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("headerpara", headerpara);
                paramdict.Add("subheaderpara", subheaderpara);
                paramdict.Add("Lawersubheaderpara", Lawersubheaderpara);

                Session["rptDataSet"] = dtCompletedTrainings;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Completed Training Report";

                Response.Redirect("~/Reports/ReportViewers/rptvCompletedTrainings.aspx");
            }
            catch (Exception ex)
            {
                log.Debug("TainingReportsGenerator : RE036()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        public void RE037()
        {
            try
            {
                log.Debug("TainingReportsGenerator : RE037()");

                string headerpara = "Pending Trainings Report";
                string subheaderpara = "";
                string Lawersubheaderpara = "";

                ReportDataHandler reportDataHandler = new ReportDataHandler();
                DataTable dtResult = new DataTable();
                DataTable dtCompletedTrainings = new DataTable();

                Dictionary<string, string> filterParameters = (Dictionary<string, string>)(Session["Filters"]);

                string companyId = filterParameters["companyId"];
                string departmentId = filterParameters["departmentId"];
                string divisionId = filterParameters["divisionId"];
                string branchId = filterParameters["branchId"];
                string financialYear = filterParameters["financialYear"];
                string employeeId = filterParameters["employeeId"];
                //string divisionId = 
                string fromDate = filterParameters["fromDate"];
                string toDate = filterParameters["toDate"];
                string departmentHeadStatus = filterParameters["departmentHeadStatus"];
                string ceoStatus = filterParameters["ceoStatus"];


                if (!String.IsNullOrEmpty(companyId))
                {
                    string companyName = String.Empty;
                    if (companyId != Constants.CON_UNIVERSAL_COMPANY_CODE)
                    {
                        //company = dpCompID.SelectedValue.ToString();
                        companyName = reportDataHandler.populateCompanyName(companyId);
                    }
                    else
                    {
                        companyName = "All Companies ";
                    }
                    subheaderpara = " Company : " + companyName + " ";
                }
                else
                {
                    subheaderpara = " Company : All Companies";
                    companyId = "";
                }

                if (!String.IsNullOrEmpty(departmentId))
                {
                    dtResult = reportDataHandler.populateDepartmentName(departmentId);
                    string departmentName = dtResult.Rows[0][0].ToString();

                    subheaderpara += "| Department : " + departmentName + " ";

                }
                else
                {
                    subheaderpara += " | Department : All Departments";
                    departmentId = "";
                }

                if (!String.IsNullOrEmpty(divisionId))
                {
                    dtResult = reportDataHandler.populateDivisionName(divisionId);
                    string divisionName = dtResult.Rows[0][0].ToString();

                    subheaderpara += " | Division : " + divisionName + " ";

                }
                else
                {
                    subheaderpara += " | Division : All Divisions";
                    divisionId = "";
                }

                if (!String.IsNullOrEmpty(branchId))
                {
                    dtResult = reportDataHandler.populateBranchName(branchId);
                    string branchName = dtResult.Rows[0][0].ToString();

                    subheaderpara += " | Branch : " + branchName + " ";

                }
                else
                {
                    subheaderpara += " | Branch : All Branches";
                    branchId = "";
                }

                if (!String.IsNullOrEmpty(financialYear))
                {

                    subheaderpara += " | Financial Year : " + financialYear + " ";
                }

                if (!String.IsNullOrEmpty(employeeId))
                {
                    dtResult = reportDataHandler.populateEmployeeName(employeeId);
                    string employeeName = dtResult.Rows[0][0].ToString();

                    subheaderpara += "| Requested By : " + employeeName + " ";
                }

                if (!String.IsNullOrEmpty(departmentHeadStatus))
                {
                    string status = "";
                    if (departmentHeadStatus == Constants.CON_ACTIVE_STATUS)
                    {
                        status = " Recommended";
                    }
                    else if (departmentHeadStatus == Constants.CON_INACTIVE_STATUS)
                    {
                        status = " Rejected";
                    }
                    Lawersubheaderpara += " | Department Head Status : " + status + " ";
                }
                else
                {
                    departmentHeadStatus = "";
                }

                if (!String.IsNullOrEmpty(ceoStatus))
                {
                    string status = "";
                    if (ceoStatus == Constants.CON_ACTIVE_STATUS)
                    {
                        status = " Recommended";
                    }
                    else if (ceoStatus == Constants.CON_INACTIVE_STATUS)
                    {
                        status = " Rejected";
                    }
                    Lawersubheaderpara += " | CEO Head Status : " + status + " ";
                }
                else
                {
                    ceoStatus = "";
                }

                if (!String.IsNullOrEmpty(fromDate))
                {
                    Lawersubheaderpara = " From : " + fromDate + " ";
                }
                else
                {
                    fromDate = "";
                }

                if (!String.IsNullOrEmpty(toDate))
                {
                    Lawersubheaderpara += " | To : " + toDate + "  ";
                }
                else
                {
                    toDate = "";
                }

                dtCompletedTrainings = reportDataHandler.getPendingTraining(companyId, departmentId, divisionId, branchId, financialYear, employeeId, fromDate, toDate, departmentHeadStatus, ceoStatus);

                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("headerpara", headerpara);
                paramdict.Add("subheaderpara", subheaderpara);
                paramdict.Add("Lawersubheaderpara", Lawersubheaderpara);

                Session["rptDataSet"] = dtCompletedTrainings;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Pending Training Report";

                Response.Redirect("~/Reports/ReportViewers/rptvPendingTrainings.aspx");
            }
            catch (Exception ex)
            {
                log.Debug("TainingReportsGenerator : RE037()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        #endregion
        
    }
}