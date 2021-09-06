using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Data;
using DataHandler.Employee;
using DataHandler.MetaData;
using DataHandler.EmployeeLeave;
using Common;
using NLog;
using GroupHRIS.Utility;

namespace GroupHRIS.EmployeeLeave
{
    public partial class webFrmLeveStatusView : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            
            lbtnClear.Visible = false;
            lblLSDetail.Visible = false;

            log.Debug("IP:" + sIPAddress + "webFrmEmployee : Page_Load");

            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {
                sUserId = Session["KeyUSER_ID"].ToString();
            }

            if (!IsPostBack)
            {
                //Session["KeyCOMP_ID"] = Constants.CON_UNIVERSAL_COMPANY_CODE;

                //Session["KeyCOMP_ID"] = "CP17";

               


                fillStatus();

                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        fillCompanies();
                    }
                    else
                    {
                        fillCompanies(Session["KeyCOMP_ID"].ToString().Trim());
                        ddlCompany.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                        fillDepartment(Session["KeyCOMP_ID"].ToString().Trim());
                        
                    }
                }                

            }
            else
            {

                if (hfCaller.Value == "txtEmploeeId")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtEmploeeId.Text = hfVal.Value;
                    }
                    if (txtEmploeeId.Text != "")
                    {
                        //Postback Methods
                        clearForEmployee();

                        hfEmpId.Value = txtEmploeeId.Text.Trim();

                        lblName.Text = getEmployeeName(txtEmploeeId.Text.Trim());
                    }
                }
                

                //if ((hfEmpId.Value.Trim() == "") || (hfEmpId.Value.Trim() != txtEmploeeId.Text.Trim()))
                //{
                //    if (txtEmploeeId.Text.Trim() != "")
                //    {
                //        if (txtEmploeeId.Text.Trim() != hfEmpId.Value.ToString().Trim()) { clearForEmployee(); }

                //    }

                //    hfEmpId.Value = txtEmploeeId.Text.Trim();

                //    //clearForEmployee();

                   


                //    lblName.Text = getEmployeeName(txtEmploeeId.Text.Trim());      
              
                //}
            }
            
            
        }

        private string getEmployeeName(string employeeId)
        {
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            try
            {
                string eName = employeeDataHandler.getEmployeeName(employeeId);

                return eName;
            }
            catch (Exception ex)
            {
                throw ex;
            }

           

        }

        private void fillDepartment(string companyId)
        {
            log.Debug("fillDepartment() - companyId:" + companyId);

            DepartmentDataHandler departmentDataHandler = new DepartmentDataHandler();
            DataTable departments = new DataTable();

            try
            {
                if (Cache["Departments" + companyId.Trim()] != null)
                {
                    departments = (DataTable)Cache["Departments" + companyId.Trim()];
                }
                else
                {
                    departments = departmentDataHandler.getDepartmentIdDeptName(companyId).Copy();
                    Cache.Add("Departments" + companyId.Trim(), departments, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);

                }

                ddlDepartment.Items.Clear();

                if (departments.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlDepartment.Items.Add(Item);

                    foreach (DataRow dataRow in departments.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DEPT_NAME"].ToString();
                        listItem.Value = dataRow["DEPT_ID"].ToString();

                        ddlDepartment.Items.Add(listItem);
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
                departmentDataHandler = null;
                departments.Dispose();
            }

        }

        private void fillCompanies(string companyId)
        {
            log.Debug("fillCompanies() - companyId:" + companyId);

            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable companies = new DataTable();

            try
            {
                companies = companyDataHandler.getCompanyIdCompName(companyId).Copy();

                ddlCompany.Items.Clear();

                if (companies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlCompany.Items.Add(Item);

                    foreach (DataRow dataRow in companies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlCompany.Items.Add(listItem);
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
                companyDataHandler = null;
                companies.Dispose();
            }

        }

        private void fillCompanies()
        {
            log.Debug("fillCompanies()");

            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable companies = new DataTable();

            try
            {
                if (Cache["Companies"] != null)
                {
                    companies = (DataTable)Cache["Companies"];
                }
                else
                {
                    companies = companyDataHandler.getCompanyIdCompName().Copy();
                    Cache.Add("Companies", companies, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }

                ddlCompany.Items.Clear();

                if (companies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlCompany.Items.Add(Item);

                    foreach (DataRow dataRow in companies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlCompany.Items.Add(listItem);
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
                companyDataHandler = null;
                companies.Dispose();
            }
        }

        private void fillStatus()
        {
            ListItem listItem0 = new ListItem("All", "");
            ddlStatus.Items.Add(listItem0);

            ListItem listItem00 = new ListItem("Rejected", "0");
            ddlStatus.Items.Add(listItem00);

            ListItem listItem1 = new ListItem("Pending","1");
            ddlStatus.Items.Add(listItem1);
            
            ListItem listItem2 = new ListItem("Covered", "2");
            ddlStatus.Items.Add(listItem2);

            ListItem listItem3 = new ListItem("Recommended", "3");
            ddlStatus.Items.Add(listItem3);

            ListItem listItem4 = new ListItem("HR Approved", "4");
            ddlStatus.Items.Add(listItem4);

            ListItem listItem5 = new ListItem("Discarded", "9");
            ddlStatus.Items.Add(listItem5);
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlCompany_SelectedIndexChanged()");

            if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
            {
                clearForCompany();

                if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                {
                    if (ddlCompany.SelectedValue != "")
                    {
                        fillDepartment(ddlCompany.SelectedValue.Trim());                        
                    }
                }
                else
                {
                    fillDepartment(Session["KeyCOMP_ID"].ToString());                    
                }
            }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlDepartment_SelectedIndexChanged()");

            if (ddlDepartment.SelectedValue != "")
            {
                gvLeaves.DataSource = null;
                gvLeaves.DataBind();

                fillDivisions(ddlDepartment.SelectedValue.Trim());
            }
        }

        private void fillDivisions(string departmentId)
        {
            log.Debug("fillDivisions() - departmentId:" + departmentId);

            DivisionDataHandler divisionDataHandler = new DivisionDataHandler();
            DataTable divisions = new DataTable();

            try
            {
                if (Cache["Divisions" + departmentId.Trim()] != null)
                {
                    divisions = (DataTable)Cache["Divisions" + departmentId.Trim()];
                }
                else
                {
                    divisions = divisionDataHandler.getDivisionIdDivName(departmentId).Copy();
                    Cache.Add("Divisions" + departmentId.Trim(), divisions, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }


                ddlDivision.Items.Clear();

                if (divisions.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlDivision.Items.Add(Item);

                    foreach (DataRow dataRow in divisions.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DIV_NAME"].ToString();
                        listItem.Value = dataRow["DIVISION_ID"].ToString();

                        ddlDivision.Items.Add(listItem);
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
                divisionDataHandler = null;
                divisions.Dispose();
            }

        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            string emploee      = "";
            //string fromDate     = "";
            //string toDate       = "";
            string companyId    = "";
            string department   = "";
            string division     = "";
            //string STATUS       = "";

            DataTable dtLeaves = new DataTable();
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

            try
            {
                DateTime mFromDate = Convert.ToDateTime(txtFromDate.Text.ToString());
                DateTime mToDate = Convert.ToDateTime(txtToDate.Text.ToString());

                if (mFromDate > mToDate)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "From Date should be less than or equal to To Date ", lblMessage);
                    return;
                }

                if (txtEmploeeId.Text.Trim() != "")
                {
                    emploee = txtEmploeeId.Text.Trim();
                    dtLeaves.Clear();
                    dtLeaves = leaveScheduleDataHandler.getEmployeeLeveStatusesForEmployee(emploee, txtFromDate.Text.Trim(), txtToDate.Text.Trim(),ddlStatus.SelectedValue.Trim()).Copy();

                }
                else if ((ddlCompany.SelectedValue.Trim() != "") && (ddlDepartment.SelectedValue.Trim() == "") && (ddlDivision.SelectedValue.Trim() == ""))
                {
                    // when select only company

                    companyId = ddlCompany.SelectedValue.Trim();

                    dtLeaves.Clear();

                    dtLeaves = leaveScheduleDataHandler.getEmployeeLeveStatusesForCompany(companyId, txtFromDate.Text.Trim(), txtToDate.Text.Trim(), ddlStatus.SelectedValue.Trim()).Copy();
                    
                }
                else if ((ddlCompany.SelectedValue.Trim() != "") && (ddlDepartment.SelectedValue.Trim() != "") && (ddlDivision.SelectedValue.Trim() == ""))
                {
                    // when select  company and department

                    companyId = ddlCompany.SelectedValue.Trim();
                    department = ddlDepartment.SelectedValue.Trim();

                    dtLeaves.Clear();

                    dtLeaves = leaveScheduleDataHandler.getEmployeeLeveStatusesForCompanyDepartment(companyId,department, txtFromDate.Text.Trim(), txtToDate.Text.Trim(), ddlStatus.SelectedValue.Trim()).Copy();
                    
                }
                else if ((ddlCompany.SelectedValue.Trim() != "") && (ddlDepartment.SelectedValue.Trim() != "") && (ddlDivision.SelectedValue.Trim() != ""))
                {
                    // when select  company and department and division

                    companyId    = ddlCompany.SelectedValue.Trim();
                    department   = ddlDepartment.SelectedValue.Trim();
                    division     = ddlDivision.SelectedValue.Trim();

                    dtLeaves.Clear();

                    dtLeaves = leaveScheduleDataHandler.getEmployeeLeveStatusesForCompanyDepartmentDivision(companyId, department,division, txtFromDate.Text.Trim(), txtToDate.Text.Trim(), ddlStatus.SelectedValue.Trim()).Copy();

                }

                gvLeaves.DataSource = dtLeaves;
                gvLeaves.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gvLeaves_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                e.Row.Attributes.Add("style", "cursor:pointer;");
            }
        }

        protected void gvLeaves_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            string leaveSheetId = "";

            try
            {
                Int32 index = Convert.ToInt32(e.CommandArgument);

                GridViewRow selectedRow = gvLeaves.Rows[index];

                leaveSheetId = selectedRow.Cells[0].Text.ToString().Trim();

                if (e.CommandName.ToString().Equals("View"))
                {
                    if (leaveSheetId.Trim() != String.Empty)
                    {
                        DataTable lsDetails = new DataTable();
                        lsDetails = leaveScheduleDataHandler.getLeaveSheetDetails(leaveSheetId).Copy();
                        gvLSDetails.DataSource = lsDetails;
                        gvLSDetails.DataBind();

                        if (lsDetails.Rows.Count > 0)
                        {
                            lbtnClear.Visible = true;
                            lblLSDetail.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            clearLeaveDetail();
        }

        private void clearLeaveDetail()
        {
            gvLSDetails.DataSource = null;
            gvLSDetails.DataBind();

            lblLSDetail.Visible = false;
            lbtnClear.Visible = false;
        }

        protected void gvLeaves_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvLeaves_PageIndexChanging()");

            string emploee = "";
            //string fromDate = "";
            //string toDate = "";
            string companyId = "";
            string department = "";
            string division = "";
            //string STATUS = "";

            try
            {
                DataTable dtLeaves = new DataTable();
                LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

                gvLeaves.PageIndex = e.NewPageIndex;

                if (txtEmploeeId.Text.Trim() != "")
                {
                    emploee = txtEmploeeId.Text.Trim();
                    dtLeaves.Clear();
                    dtLeaves = leaveScheduleDataHandler.getEmployeeLeveStatusesForEmployee(emploee, txtFromDate.Text.Trim(), txtToDate.Text.Trim(), ddlStatus.SelectedValue.Trim()).Copy();

                }
                else if ((ddlCompany.SelectedValue.Trim() != "") && (ddlDepartment.SelectedValue.Trim() == "") && (ddlDivision.SelectedValue.Trim() == ""))
                {
                    // when select only company

                    companyId = ddlCompany.SelectedValue.Trim();

                    dtLeaves.Clear();

                    dtLeaves = leaveScheduleDataHandler.getEmployeeLeveStatusesForCompany(companyId, txtFromDate.Text.Trim(), txtToDate.Text.Trim(), ddlStatus.SelectedValue.Trim()).Copy();

                }
                else if ((ddlCompany.SelectedValue.Trim() != "") && (ddlDepartment.SelectedValue.Trim() != "") && (ddlDivision.SelectedValue.Trim() == ""))
                {
                    // when select  company and department

                    companyId = ddlCompany.SelectedValue.Trim();
                    department = ddlDepartment.SelectedValue.Trim();

                    dtLeaves.Clear();

                    dtLeaves = leaveScheduleDataHandler.getEmployeeLeveStatusesForCompanyDepartment(companyId, department, txtFromDate.Text.Trim(), txtToDate.Text.Trim(), ddlStatus.SelectedValue.Trim()).Copy();

                }
                else if ((ddlCompany.SelectedValue.Trim() != "") && (ddlDepartment.SelectedValue.Trim() != "") && (ddlDivision.SelectedValue.Trim() != ""))
                {
                    // when select  company and department and division

                    companyId = ddlCompany.SelectedValue.Trim();
                    department = ddlDepartment.SelectedValue.Trim();
                    division = ddlDivision.SelectedValue.Trim();

                    dtLeaves.Clear();

                    dtLeaves = leaveScheduleDataHandler.getEmployeeLeveStatusesForCompanyDepartmentDivision(companyId, department, division, txtFromDate.Text.Trim(), txtToDate.Text.Trim(), ddlStatus.SelectedValue.Trim()).Copy();

                }

                gvLeaves.DataSource = dtLeaves;
                gvLeaves.DataBind();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clearForm();
        }

        private void clearForm()
        {
            clearLeaveDetail();

            txtEmploeeId.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            if (ddlCompany.Items.Count > 0)
            {
                ddlCompany.SelectedIndex = 0;
            }
            if (ddlDepartment.Items.Count > 0)
            {
                ddlDepartment.SelectedIndex = 0;
            }
            if (ddlDivision.Items.Count > 0)
            {
                ddlDivision.SelectedIndex = 0;
            }
            if (ddlStatus.Items.Count > 0)
            {
                ddlStatus.SelectedIndex = 0;
            }
            Utility.Errorhandler.ClearError(lblMessage);
            lblName.Text = "";
            lbtnClear.Visible = false;

            gvLeaves.DataSource = null;
            gvLeaves.DataBind();
        }

        private void clearForEmployee()
        {
            clearLeaveDetail();
            txtFromDate.Text = "";
            txtToDate.Text = "";
            if (ddlCompany.Items.Count > 0)
            {
                ddlCompany.SelectedIndex = 0;
            }
            if (ddlDepartment.Items.Count > 0)
            {
                ddlDepartment.SelectedIndex = 0;
            }
            if (ddlDivision.Items.Count > 0)
            {
                ddlDivision.SelectedIndex = 0;
            }
            if (ddlStatus.Items.Count > 0)
            {
                ddlStatus.SelectedIndex = 0;
            }
            Utility.Errorhandler.ClearError(lblMessage);
            lblName.Text = "";
            lbtnClear.Visible = false;
        }

        private void clearForCompany()
        {
            clearLeaveDetail();
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtEmploeeId.Text = "";
            hfEmpId.Value = "";

            if (ddlDepartment.Items.Count > 0)
            {
                ddlDepartment.SelectedIndex = 0;
            }
            if (ddlDivision.Items.Count > 0)
            {
                ddlDivision.SelectedIndex = 0;
            }
            if (ddlStatus.Items.Count > 0)
            {
                ddlStatus.SelectedIndex = 0;
            }
            Utility.Errorhandler.ClearError(lblMessage);
            lblName.Text = "";
            

            gvLeaves.DataSource = null;
            gvLeaves.DataBind();
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvLeaves.DataSource = null;
            gvLeaves.DataBind();
        }
       
    }
}