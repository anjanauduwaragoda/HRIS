using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using GroupHRIS.Utility;
using System.Globalization;
using DataHandler.MetaData;
using System.Data;
using DataHandler.Attendance;
using DataHandler.Reports;
using DataHandler.Property;
using DataHandler.Employee;

namespace GroupHRIS.Reports.ReportFilter
{
    public partial class RptGenerator : System.Web.UI.Page
    {
        public static string mStrRepName = "";

        string mrepname = "";
        string mfromdate = "";
        string mtodate = "";
        string mcompcode = "";
        string mempcode = "";
        string employeeId = "";
        string mdeptcode = "";
        string statuscode = "";

        DateTime fromdate;
        DateTime todate;

        void populateEmployeeStatus()
        {
            lblStatusCode.Visible = true;
            ddlEmpStatus.Visible = true;

            ReportDataHandler oReportDataHandler = new ReportDataHandler();
            DataTable dt = new DataTable();
            dt = oReportDataHandler.populateEmployeeStatus();

            ddlEmpStatus.Items.Add(new ListItem("ALL", "0"));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string value = dt.Rows[i]["STATUS_CODE"].ToString();
                string display = dt.Rows[i]["DESCRIPTION"].ToString();

                ddlEmpStatus.Items.Add(new ListItem(display, value));
            }
        }

        void populateDepartments(string CompanyCode)
        {
            if (dpCompID.SelectedValue == Constants.CON_UNIVERSAL_COMPANY_CODE)
            {
                ddlDepartment.Items.Clear();
            }
            else
            {
                ddlDepartment.Items.Clear();

                ReportDataHandler oReportDataHandler = new ReportDataHandler();
                DataTable dt = new DataTable();
                dt = oReportDataHandler.populateDepartments(CompanyCode);

                ddlDepartment.Items.Add(new ListItem("", ""));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string value = dt.Rows[i]["DEPT_ID"].ToString();
                    string display = dt.Rows[i]["DEPT_NAME"].ToString();

                    ddlDepartment.Items.Add(new ListItem(display, value));
                }
            }
        }

        void populateBranch(string CompanyCode)
        {
            if (dpCompID.SelectedValue == Constants.CON_UNIVERSAL_COMPANY_CODE)
            {
                ddlBranch.Items.Clear();
            }
            else
            {
                ddlBranch.Items.Clear();

                ReportDataHandler oReportDataHandler = new ReportDataHandler();
                DataTable dt = new DataTable();
                dt = oReportDataHandler.populateBranch(CompanyCode);

                ddlBranch.Items.Add(new ListItem("", ""));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string value = dt.Rows[i]["BRANCH_ID"].ToString();
                    string display = dt.Rows[i]["BRANCH_NAME"].ToString();

                    ddlBranch.Items.Add(new ListItem(display, value));
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string KeyCOMP_ID = (string)(Session["KeyCOMP_ID"]);
                mStrRepName = (string)Session["SessionRep_Code"];
                string KeyUSER_ID = (string)(Session["KeyUSER_ID"]);
                //string KeyCOMP_ID = Constants.CON_UNIVERSAL_COMPANY_CODE;
                //mStrRepName = "RE002";

                if (!string.IsNullOrEmpty(KeyUSER_ID))
                {
                    //if (!string.IsNullOrEmpty(Request.QueryString["mRepName"]))
                    //{
                    //    hfRepID.Value = String.Empty;
                    //    hfRepID.Value = Request.QueryString["mRepName"];
                    //    Session["SessionRep_Code"] = Request.QueryString["mRepName"];
                    //    mStrRepName = (string)Session["SessionRep_Code"];
                    //}

                    hfRepID.Value = String.Empty;
                    hfRepID.Value = (string)Session["RptID"];
                    Session["SessionRep_Code"] = Session["RptID"];
                    mStrRepName = (string)Session["SessionRep_Code"];


                    lblrepname.Text = mStrRepName;
                    DispalyReportName();
                    getCompID(KeyCOMP_ID);
                    getCultryDate();

                    if (mStrRepName == "RE003")
                    {
                        populateEmployeeStatus();
                    }
                    RadioButtonVisbility();
                }
                else
                {
                    Response.Redirect("~/Login/SessionExpior.aspx", false);
                }
            }
            if (IsPostBack)
            {
                if (hfCaller.Value == "txtemployee")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtemployee.Text = hfVal.Value;
                    }
                    if (txtemployee.Text != "")
                    {
                        //Postback Methods
                    }
                }
            }
        }

        private void getCultryDate()
        {
            string sCultryDate = "";
            AttendanceSummaryHandler attendanceSummaryHandler = new AttendanceSummaryHandler();

            try
            {
                sCultryDate = attendanceSummaryHandler.getCultryDate();
                DateTime sSummaryDate = DateTime.Parse(sCultryDate.ToString());
                txtfromdate.Text = sSummaryDate.ToString("dd/MM/yyyy");
                txttodate.Text = sSummaryDate.ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                attendanceSummaryHandler = null;
            }

        }

        private void getCompID(string KeyCOMP_ID)
        {
            CompanyDataHandler companynameid = new CompanyDataHandler();
            DataTable CompID = new DataTable();
            try
            {
                if (KeyCOMP_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    CompID = companynameid.getCompanyIdCompName();
                    ListItem lstItem = new ListItem();
                    lstItem.Text = Constants.CON_UNIVERSAL_COMPANY_NAME;
                    lstItem.Value = Constants.CON_UNIVERSAL_COMPANY_CODE;
                    dpCompID.Items.Add(lstItem);
                }
                else
                {
                    ListItem lstItem = new ListItem();
                    lstItem.Text = "";
                    lstItem.Value = "";
                    dpCompID.Items.Add(lstItem);
                    CompID = companynameid.getCompanyIdCompName(KeyCOMP_ID);
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
                companynameid = null;
                CompID.Dispose();
                CompID = null;
            }
        }

        protected void dpCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateDepartments(dpCompID.SelectedValue);
            populateBranch(dpCompID.SelectedValue);
            RadioButtonVisbility();
        }

        void RadioButtonVisbility()
        {
            if (DeptBranchFunction() == false)
            {
                chkBranch.Visible = false;

                Label1.Visible = false;
                ddlBranch.Visible = false;

                Label2.Visible = true;
                ddlDepartment.Visible = true;

                return;
            }

            if (dpCompID.SelectedIndex == 0)
            {
                chkBranch.Visible = false;
            }
            else
            {
                chkBranch.Visible = true;

                Label2.Visible = true;
                Label1.Visible = false;

                ddlDepartment.Visible = true;
                ddlBranch.Visible = false;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            dpCompID.SelectedIndex = 0;
            ddlDepartment.Items.Clear();
            txtfromdate.Text = txttodate.Text = txtemployee.Text = String.Empty;
            txtfromdate.Text = txttodate.Text = DateTime.Today.Date.ToString("dd/MM/yyyy");
            try { ddlEmpStatus.SelectedIndex = 0; }
            catch { }
            chkemployee.Checked = false;
            Errorhandler.ClearError(lblerror);
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            Session["report"] = null;

            Dictionary<string, string> rDictionary = new Dictionary<string, string>();

            try
            {
                fromdate = DateTime.ParseExact(txtfromdate.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                todate = DateTime.ParseExact(txttodate.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (fromdate > todate)
                {
                    Utility.Errorhandler.GetError("2", "From date is greater than To date", lblerror);
                    return;
                }

                if (chkemployee.Checked == false && txtemployee.Text.Trim() != "")
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Tick Selected Employee Only", lblerror);
                    return;
                }

                //DateTime fromdate = Convert.ToDateTime(txtfromdate.Text.ToString());
                //DateTime todate = Convert.ToDateTime(txttodate.Text.ToString());
                string mCompCode = dpCompID.SelectedValue.ToString();
                string mdepCode = ddlDepartment.SelectedValue.ToString();

                string empcode = "";

                if (todate < fromdate)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Invalid date range", lblerror);
                    return;
                }

                if (ddlBranch.SelectedValue == "" && ddlBranch.Visible == true)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Select the Branch", lblerror);
                    return;
                }

                if (chkemployee.Checked == true)
                {
                    empcode = txtemployee.Text.ToString();
                }

                mrepname = "";
                mfromdate = "";
                mtodate = "";
                mcompcode = "";
                mempcode = "";
                employeeId = "";
                mdeptcode = "";
                statuscode = "";


                mrepname = hfRepID.Value;
                mfromdate = fromdate.ToString("yyyy-MM-dd");
                mtodate = todate.ToString("yyyy-MM-dd");
                mcompcode = mCompCode;
                mempcode = empcode;
                employeeId = empcode;
                mdeptcode = ddlDepartment.SelectedValue;
                statuscode = "";


                if (hfRepID.Value == "RE001")
                {
                    rep001();
                }
                else if (hfRepID.Value == "RE002")
                {
                    rep002();
                }
                else if (mStrRepName == "RE003")
                {
                    statuscode = ddlEmpStatus.SelectedValue;
                    rep003();
                }
                else if (mStrRepName == "RE004")
                {
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                }
                else if (mStrRepName == "RE005")
                {
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                }
                else if (mStrRepName == "RE007")
                {
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                }
                else if (mStrRepName == "RE008")
                {
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                }
                else if (mStrRepName == "RE009")
                {
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                }
                else if (mStrRepName == "RE010")
                {
                    rep010();
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                }
                else if (mStrRepName == "RE012")
                {
                    rep012();
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
                }
                else if (mStrRepName == "RE013")
                {
                    if (fromdate < todate || fromdate == todate)
                    {
                        rep013();
                        //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
                    }
                    else
                    {
                        Utility.Errorhandler.GetError("2", "From date is grater than To date", lblerror);
                        return;
                    }
                }
                else if (mStrRepName == "RE014")
                {
                    if (empcode == "" & mdepCode == "")
                    {
                        rep014();
                        //Response.Redirect("WebReportViewer.aspx?compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                    }
                    else
                    {
                        Utility.Errorhandler.GetError("2", "You can not take this report for Employee and Department", lblerror);
                        return;
                    }
                }
                else if (mStrRepName == "RE015")
                {
                    rep015();
                    //Response.Redirect("WebReportViewer.aspx?compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                }
                else if (mStrRepName == "RE016")
                {
                    rep016();
                    //Response.Redirect("WebReportViewer.aspx?compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode, false);
                }
                else if (mStrRepName == "RE017")
                {
                    if (fromdate < todate || fromdate == todate)
                    {
                        rep017();
                        //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
                    }
                    else
                    {
                        Utility.Errorhandler.GetError("2", "From date is grater than To date", lblerror);
                        return;
                    }
                }
                else if (mStrRepName == "RE018")
                {
                    if (!string.IsNullOrEmpty(empcode))
                    {
                        Utility.Errorhandler.GetError("2", "You can not take this report for Employee", lblerror);
                        return;
                    }
                    else
                    {
                        rep018();
                        //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&mdeptcode=" + mdepCode + "&empcode=" + empcode, false);
                    }
                }
                else if (mStrRepName == "RE020")
                {
                    rep020();
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode, false);
                }
                else if (mStrRepName == "RE021")
                {
                    if (mCompCode == Constants.CON_UNIVERSAL_COMPANY_CODE && empcode == "")
                    {
                        Utility.Errorhandler.GetError("2", "You can not take this report for All Company", lblerror);
                        return;
                    }

                    else
                    {
                        rep021();
                        //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
                    }

                }
                else if (mStrRepName == "RE022")
                {
                    rep022();
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);

                }
                else if (mStrRepName == "RE023")
                {
                    rep023();
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);

                }
                else if (mStrRepName == "RE024")
                {
                    rep024();
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
                }
                else if (mStrRepName == "RE025")
                {
                    if ((mCompCode.Trim() == Constants.CON_UNIVERSAL_COMPANY_CODE) && (string.IsNullOrEmpty(empcode)))
                    {
                        Utility.Errorhandler.GetError("2", "You can not take this report for all companies", lblerror);
                        return;
                    }
                    else if ((mCompCode.Trim() == Constants.CON_UNIVERSAL_COMPANY_CODE) && (!string.IsNullOrEmpty(empcode)))
                    {
                        Utility.Errorhandler.GetError("2", "You can not take this report for Employee", lblerror);
                        return;
                    }
                    else if (!string.IsNullOrEmpty(mdepCode))
                    {
                        Utility.Errorhandler.GetError("2", "You can not take this report for a Department", lblerror);
                        return;
                    }
                    else
                    {
                        rep025();
                        //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode, false);
                    }
                }
                else if (mStrRepName == "RE026")
                {
                    if ((todate - fromdate).TotalDays > 1)
                    {
                        rep026();
                        //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
                    }
                    else
                    {
                        Utility.Errorhandler.GetError("2", " Minimum Date Different Must Be 3 ", lblerror);
                        return;
                    }
                }
                else if (mStrRepName == "RE027")
                {
                    //DateTime mFromDate = Convert.ToDateTime(txtfromdate.Text.ToString());
                    //DateTime mToDate = Convert.ToDateTime(txttodate.Text.ToString());

                    if (todate < fromdate)
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Invalid date range", lblerror);
                        return;
                    }

                    if (chkemployee.Checked == false && txtemployee.Text.Trim() != "")
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Selected Employee Only", lblerror);
                        return;
                    }
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empID=" + txtemployee.Text.Trim() + "&mdeptcode=" + mdepCode + "&empcode=" + empcode, false);
                    rep027();
                }
                else if (mStrRepName == "RE028")
                {

                    Dictionary<string, string> dcOffDayWork = new Dictionary<string, string>();

                    if (chkemployee.Checked == true)
                    {
                        dcOffDayWork.Add("EmployeeID", txtemployee.Text.Trim());
                    }
                    else
                    {
                        dcOffDayWork.Add("EmployeeID", String.Empty);
                    }
                    dcOffDayWork.Add("CompanyID", dpCompID.SelectedValue);
                    dcOffDayWork.Add("CompanyName", dpCompID.SelectedItem.Text.Trim());
                    try
                    {
                        dcOffDayWork.Add("DepartmentID", ddlDepartment.SelectedValue);
                    }
                    catch
                    {
                        dcOffDayWork.Add("DepartmentID", String.Empty);
                    }
                    try
                    {
                        dcOffDayWork.Add("DepartmentName", ddlDepartment.SelectedItem.Text.Trim());
                    }
                    catch
                    {
                        dcOffDayWork.Add("DepartmentName", String.Empty);
                    }
                    dcOffDayWork.Add("FromDate", fromdate.ToString("yyyy-MM-dd").Trim());
                    dcOffDayWork.Add("ToDate", todate.ToString("yyyy-MM-dd").Trim());
                    Session["OffDayWork"] = dcOffDayWork;

                    rep028();
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
                }
                else if (mStrRepName == "RE029")
                {
                    Utility.Errorhandler.ClearError(lblerror);

                    Dictionary<string, string> dcStaffStrength = new Dictionary<string, string>();

                    if (chkemployee.Checked == true)
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Employee wise report is not provided", lblerror);
                        return;
                    }
                    if (ddlDepartment.SelectedIndex > 0)
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Department wise report is not provided", lblerror);
                        return;
                    }


                    dcStaffStrength.Add("CompanyID", dpCompID.SelectedValue);
                    dcStaffStrength.Add("CompanyName", dpCompID.SelectedItem.Text.Trim());
                    dcStaffStrength.Add("FromDate", fromdate.ToString("yyyy-MM-dd").Trim());
                    dcStaffStrength.Add("ToDate", todate.ToString("yyyy-MM-dd").Trim());
                    Session["StaffStrength"] = dcStaffStrength;

                    rep029();
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);

                }

                else if (mStrRepName == "RE030")
                {

                    Dictionary<string, string> dcEmployeeSkills = new Dictionary<string, string>();

                    if (chkemployee.Checked == true)
                    {
                        dcEmployeeSkills.Add("EmployeeID", txtemployee.Text.Trim());
                    }
                    else
                    {
                        dcEmployeeSkills.Add("EmployeeID", String.Empty);
                    }
                    dcEmployeeSkills.Add("CompanyID", dpCompID.SelectedValue);
                    dcEmployeeSkills.Add("CompanyName", dpCompID.SelectedItem.Text.Trim());
                    try
                    {
                        dcEmployeeSkills.Add("DepartmentID", ddlDepartment.SelectedValue);
                    }
                    catch
                    {
                        dcEmployeeSkills.Add("DepartmentID", String.Empty);
                    }
                    try
                    {
                        dcEmployeeSkills.Add("DepartmentName", ddlDepartment.SelectedItem.Text.Trim());
                    }
                    catch
                    {
                        dcEmployeeSkills.Add("DepartmentName", String.Empty);
                    }
                    Session["EmployeeSkills"] = dcEmployeeSkills;
                    rep030();
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);

                }
                else if (mStrRepName == "RE031")
                {
                    rep031();
                    //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
                }
                else if (mStrRepName == "RE032")
                {
                    rep032();
                }
                else if (mStrRepName == "RE033")
                {
                    rep033();
                }
                else if (mStrRepName == "RE034")
                {
                    rep034();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        //ADD REPORT IDs AND COMPANY IDs to BRANCH FILTER
        Boolean DeptBranchFunction()
        {
            string SelectedCompany = dpCompID.SelectedValue.Trim();
            string ReportID = hfRepID.Value;

            //Separate report ID's With "_"
            string ValidReports = "RE001_RE003_RE032_RE033_RE022_RE024_RE031_RE013_RE002_RE031";

            //Separate Company ID's With "_"
            string ValidCompanies = "CP07_CP63_CP03";

            bool IsValidReport = Contains(ValidReports, ReportID, StringComparison.OrdinalIgnoreCase);
            bool IsValidCompany = Contains(ValidCompanies, SelectedCompany, StringComparison.OrdinalIgnoreCase);

            if ((IsValidReport == true) && (IsValidCompany == true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }//ADD REPORT IDs AND COMPANY IDs to BRANCH FILTER

        bool Contains(string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        void DispalyReportName()
        {
            ReportDataHandler RDH = new ReportDataHandler();
            lblrepname.Text = lblrepname.Text.Trim() + " - " + RDH.getReportName(lblrepname.Text.Trim());
        }


        DataTable ChangeTimeSpan(DataTable dt)
        {
            DataTable DTNew = new DataTable();


            DTNew.Columns.Add("IN_DATE", typeof(DateTime));
            DTNew.Columns.Add("IN_TIME");
            DTNew.Columns.Add("LATE_MINUTES", System.Type.GetType("System.TimeSpan"));
            DTNew.Columns.Add("IN_LOCATION");
            DTNew.Columns.Add("OUT_DATE", typeof(DateTime));
            DTNew.Columns.Add("OUT_TIME");
            DTNew.Columns.Add("EARLY_MINUTES", System.Type.GetType("System.TimeSpan"));
            DTNew.Columns.Add("OUT_LOCATION");
            DTNew.Columns.Add("COMPANY_ID");
            DTNew.Columns.Add("EMPLOYEE_ID");
            DTNew.Columns.Add("OT_HOURS", System.Type.GetType("System.TimeSpan"));
            DTNew.Columns.Add("REMARK");
            DTNew.Columns.Add("INITIALS_NAME");
            DTNew.Columns.Add("KNOWN_NAME");
            DTNew.Columns.Add("EPF_NO");
            DTNew.Columns.Add("COMP_NAME");
            DTNew.Columns.Add("DESIGNATION_NAME");
            DTNew.Columns.Add("DEPT_NAME");
            DTNew.Columns.Add("DIV_NAME");
            DTNew.Columns.Add("BRANCH_NAME");
            DTNew.Columns.Add("LATE_TOTAL");
            DTNew.Columns.Add("EARLY_TOTAL");
            DTNew.Columns.Add("EXTRA_TOTAL");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = DTNew.NewRow();
                dr["IN_DATE"] = dt.Rows[i]["IN_DATE"];
                dr["IN_TIME"] = dt.Rows[i]["IN_TIME"];
                dr["LATE_MINUTES"] = new TimeSpan(0, 0, 0);
                dr["IN_LOCATION"] = dt.Rows[i]["IN_LOCATION"];
                dr["OUT_DATE"] = dt.Rows[i]["OUT_DATE"];
                dr["OUT_TIME"] = dt.Rows[i]["OUT_TIME"];
                dr["EARLY_MINUTES"] = new TimeSpan(0, 0, 0);
                dr["COMPANY_ID"] = dt.Rows[i]["COMPANY_ID"];
                dr["EMPLOYEE_ID"] = dt.Rows[i]["EMPLOYEE_ID"];
                dr["OT_HOURS"] = new TimeSpan(0, 0, 0);
                dr["REMARK"] = dt.Rows[i]["REMARK"];
                dr["INITIALS_NAME"] = dt.Rows[i]["INITIALS_NAME"];
                dr["KNOWN_NAME"] = dt.Rows[i]["KNOWN_NAME"];
                dr["EPF_NO"] = dt.Rows[i]["EPF_NO"];
                dr["COMP_NAME"] = dt.Rows[i]["COMP_NAME"];
                dr["DESIGNATION_NAME"] = dt.Rows[i]["DESIGNATION_NAME"];
                dr["DEPT_NAME"] = dt.Rows[i]["DEPT_NAME"];
                dr["DIV_NAME"] = dt.Rows[i]["DIV_NAME"];
                dr["BRANCH_NAME"] = dt.Rows[i]["BRANCH_NAME"];
                DTNew.Rows.Add(dr);
            }



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string lateMinutes = dt.Rows[i]["LATE_MINUTES"].ToString();
                if (lateMinutes == "")
                {
                    lateMinutes = "0.0";
                }

                string[] lateMinutesArr = lateMinutes.Split('.');
                if (lateMinutesArr.Length > 1)
                {
                    //int Hrs = 0;
                    //int Mins = Convert.ToInt32(lateMinutesArr[0]);

                    if (lateMinutesArr[1].Length == 1)
                    {
                        lateMinutesArr[1] += "0";
                    }

                    //int Secs = Convert.ToInt32((Convert.ToDouble(lateMinutesArr[1]) / 100.0) * 60);
                    DTNew.Rows[i]["LATE_MINUTES"] = new TimeSpan(0, Convert.ToInt32(lateMinutesArr[0]), Convert.ToInt32((Convert.ToDouble(lateMinutesArr[1]) / 100.0) * 60));
                }
                else
                {
                    DTNew.Rows[i]["LATE_MINUTES"] = new TimeSpan(0, 0, 0);
                }

                string earlyMinutes = dt.Rows[i]["EARLY_MINUTES"].ToString();
                if (earlyMinutes == "")
                {
                    earlyMinutes = "0.0";
                }
                string[] earlyMinutesArr = earlyMinutes.Split('.');
                if (earlyMinutesArr.Length > 1)
                {
                    if (earlyMinutesArr[1].Length == 1)
                    {
                        earlyMinutesArr[1] += "0";
                    }

                    DTNew.Rows[i]["EARLY_MINUTES"] = new TimeSpan(0, Convert.ToInt32(earlyMinutesArr[0]), Convert.ToInt32((Convert.ToDouble(earlyMinutesArr[1]) / 100.0) * 60));
                }
                else
                {
                    DTNew.Rows[i]["EARLY_MINUTES"] = new TimeSpan(0, 0, 0);
                }

                string extraMinutes = dt.Rows[i]["OT_HOURS"].ToString();
                if (extraMinutes == "")
                {
                    extraMinutes = "0.0";
                }
                string[] extraMinutesArr = extraMinutes.Split('.');
                if (extraMinutesArr.Length > 1)
                {
                    if (extraMinutesArr[1].Length == 1)
                    {
                        extraMinutesArr[1] += "0";
                    }

                    DTNew.Rows[i]["OT_HOURS"] = new TimeSpan(0, Convert.ToInt32(extraMinutesArr[0]), Convert.ToInt32((Convert.ToDouble(extraMinutesArr[1]) / 100.0) * 60));
                }
                else
                {
                    DTNew.Rows[i]["OT_HOURS"] = new TimeSpan(0, 0, 0);
                }
            }

            dt = new DataTable();
            dt = DTNew;


            return dt;
        }

        void rep001()
        {
            if (ddlBranch.Visible == true)
            {
                mdeptcode = "N/A";
            }

            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtreport = new DataTable();

            if (ddlDepartment.Visible == true)
            {
                dtreport = reportDataHandler.populaterep0001(mfromdate, mtodate, mcompcode, mempcode, mdeptcode).Copy();
            }
            else
            {
                dtreport = reportDataHandler.populaterep0001(mfromdate, mtodate, mcompcode, mempcode, ddlBranch.SelectedValue.Trim()).Copy();
            }

            try
            {
                string mrptheader = "Attendance Summary Report";
                string mrptsubheader = " ";
                string mrptLawerheader = " ";


                if (mempcode != "")
                {
                    mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    if (mfromdate != mtodate)
                    {
                        mrptLawerheader = "From : " + mfromdate + " To : " + mtodate;
                    }
                    else
                    {
                        mrptLawerheader = "Date : " + mfromdate;
                    }
                }
                else if ((mdeptcode != "") && (mdeptcode != "N/A"))
                {
                    mrptsubheader = reportDataHandler.populateCompanyName(mcompcode) + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString() + Environment.NewLine + Environment.NewLine;
                    if (mfromdate != mtodate)
                    {
                        mrptLawerheader = "From : " + mfromdate + " To : " + mtodate;
                    }
                    else
                    {
                        mrptLawerheader = "Date : " + mfromdate;
                    }
                }
                else if (mcompcode != "CP00")
                {
                    mrptsubheader = reportDataHandler.populateCompanyName(mcompcode);
                    if (mfromdate != mtodate)
                    {
                        mrptLawerheader = "From : " + mfromdate + " To : " + mtodate;
                    }
                    else
                    {
                        mrptLawerheader = "Date : " + mfromdate;
                    }
                }
                else
                {
                    mrptsubheader = "All Companies";
                    if (mfromdate != mtodate)
                    {
                        mrptLawerheader = "From : " + mfromdate + " To : " + mtodate;
                    }
                    else
                    {
                        mrptLawerheader = "Date : " + mfromdate;
                    }
                }

                DataTable dttbl = new DataTable();

                dttbl = ChangeTimeSpan(dtreport);
                dtreport = new DataTable();
                dtreport = dttbl;

                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("headerpara", mrptheader);
                paramdict.Add("subheaderpara", mrptsubheader);
                paramdict.Add("Lawersubheaderpara", mrptLawerheader);

                Session["rptDataSet"] = dtreport;
                Session["rptParamDict"] = paramdict;
                Response.Redirect("~/Reports/ReportViewers/ReportSummary.aspx");


            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                reportDataHandler = null;
                dtreport.Dispose();
                dtreport = null;
            }

            //Response.Redirect("ReportViewers/ReportSummary.aspx");
        }

        void rep002()
        {
            if (ddlBranch.Visible == true)
            {
                mdeptcode = "N/A";
            }

            if (fromdate < todate || fromdate == todate)
            {

                CompanyDataHandler companyDataHandler = new CompanyDataHandler();
                ReportDataHandler reportDataHandler = new ReportDataHandler();
                DataTable dtreport = new DataTable();

                try
                {
                    if (ddlDepartment.Visible == true)
                    {
                        dtreport = reportDataHandler.populaterep0002(mfromdate, mtodate, mcompcode, mempcode, mdeptcode).Copy();
                    }
                    else
                    {
                        dtreport = reportDataHandler.populaterep0002(mfromdate, mtodate, mcompcode, mempcode, ddlBranch.SelectedValue.Trim()).Copy();
                    }

                    string mrptsubheader = "";
                    string mrptheader = "Late Comers Report";
                    if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                    {
                        if (mempcode == "")
                        {
                            mrptsubheader = "All Companies";
                        }
                        else
                        {
                            mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                        }
                    }
                    else
                    {

                        string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);
                        if (mempcode == "" && mdeptcode == "")
                        {
                            mrptsubheader = companyname;
                        }
                        else if (mempcode != "" && mdeptcode == "")
                        {
                            mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                        }
                        else if (mdeptcode != "" && mdeptcode == Constants.DEPARTMENT_ID_STAMP)
                        {
                            mrptsubheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                        }
                        else if (mdeptcode != "" && mdeptcode != Constants.DEPARTMENT_ID_STAMP)
                        {
                            mrptsubheader = companyname + Environment.NewLine + Environment.NewLine + "Branch : " + ddlBranch.SelectedItem;
                        }
                        else
                        {
                            mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                        }
                    }

                    string sFromTodate = "";
                    if (mfromdate == mtodate)
                    {
                        sFromTodate = "Date : " + mfromdate;
                    }
                    else
                    {
                        sFromTodate = "From : " + mfromdate + " To : " + mtodate;
                    }

                    Dictionary<string, string> paramdict = new Dictionary<string, string>();
                    paramdict.Add("headerpara", mrptheader);
                    paramdict.Add("subheaderpara", mrptsubheader);
                    paramdict.Add("parmFromDate", sFromTodate);

                    Session["rptDataSet"] = dtreport;
                    Session["rptParamDict"] = paramdict;

                    Response.Redirect("~/Reports/ReportViewers/LateCommers.aspx");


                }
                catch (Exception ex)
                {
                    CommonVariables.MESSAGE_TEXT = ex.Message;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                finally
                {
                    reportDataHandler = null;
                    dtreport.Dispose();
                    dtreport = null;
                }

                //Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + hfRepID.Value.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
            }
            else
            {
                Utility.Errorhandler.GetError("2", "From date is grater than To date", lblerror);
                return;
            }
        }

        void rep003()
        {
            if (ddlBranch.Visible == true)
            {
                mdeptcode = "N/A";
            }

            if (chkemployee.Checked == false)
            {
                txtemployee.Text = String.Empty;
            }

            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable dtreport = new DataTable();

            try
            {
                string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);

                string mrptheader = "Employee Details Report";
                string mrptsubheader = " ";
                string mrptLawerheader = "All Employees";


                if (statuscode == "0")
                {
                    if (mempcode == "")
                    {
                        if (mdeptcode == "")
                        {
                            dtreport = reportDataHandler.populaterep0003(mcompcode).Copy();
                            mrptsubheader = companyname;
                        }
                        else
                        {
                            if ((ddlDepartment.SelectedIndex == 0) && (ddlBranch.SelectedIndex == 0))
                            {
                                dtreport = reportDataHandler.populaterep0003(mcompcode).Copy();
                                mrptsubheader = companyname;
                            }
                            else
                            {
                                if (ddlDepartment.Visible == true)
                                {
                                    dtreport = reportDataHandler.populaterep0003(mcompcode, mdeptcode).Copy();
                                    ReportDataHandler oReportDataHandler = new ReportDataHandler();
                                    DataTable dtbl = new DataTable();

                                    dtbl = oReportDataHandler.populateDepartmentName(mdeptcode).Copy();
                                    mrptsubheader = companyname + "   |   Department : " + dtbl.Rows[0]["DEPT_NAME"].ToString();
                                }
                                else
                                {
                                    dtreport = reportDataHandler.populaterep0003(mcompcode, ddlBranch.SelectedValue.Trim()).Copy();
                                    ReportDataHandler oReportDataHandler = new ReportDataHandler();
                                    DataTable dtbl = new DataTable();

                                    //dtbl = oReportDataHandler.populateDepartmentName(mdeptcode).Copy();
                                    mrptsubheader = companyname + "   |   Branch : " + ddlBranch.SelectedItem.Text.Trim();
                                }
                            }
                        }
                    }
                    else
                    {
                        dtreport = reportDataHandler.populaterep0003IND(mempcode).Copy();
                        ReportDataHandler oReportDataHandler = new ReportDataHandler();
                        DataTable dtbl = new DataTable();

                        dtbl = oReportDataHandler.populateEmployeeName(mempcode).Copy();

                        mrptsubheader = "Employee Name : " + dtbl.Rows[0]["FULL_NAME"].ToString();
                        mrptLawerheader = " ";
                    }
                }
                else
                {
                    if (mempcode == "")
                    {
                        if (mdeptcode == "")
                        {
                            dtreport = reportDataHandler.opopulaterep0003(mcompcode, statuscode).Copy();
                            mrptsubheader = companyname;
                            mrptLawerheader = "Employee Status : " + ddlEmpStatus.SelectedItem.Text.Trim();
                        }
                        else
                        {
                            if ((ddlDepartment.SelectedIndex == 0) && (ddlBranch.SelectedIndex == 0))
                            {
                                dtreport = reportDataHandler.opopulaterep0003(mcompcode, statuscode).Copy();
                                mrptsubheader = companyname;
                                mrptLawerheader = "Employee Status : " + ddlEmpStatus.SelectedItem.Text.Trim();
                            }
                            else
                            {
                                if (ddlDepartment.Visible == true)
                                {
                                    dtreport = reportDataHandler.opopulaterep0003(mcompcode, mdeptcode, statuscode).Copy();
                                    ReportDataHandler oReportDataHandler = new ReportDataHandler();
                                    DataTable dtbl = new DataTable();

                                    dtbl = oReportDataHandler.populateDepartmentName(mdeptcode).Copy();

                                    mrptsubheader = companyname + "   |   Department : " + dtbl.Rows[0]["DEPT_NAME"].ToString();
                                    mrptLawerheader = "Employee Status : " + ddlEmpStatus.SelectedItem.Text.Trim();
                                }
                                else
                                {
                                    dtreport = reportDataHandler.opopulaterep0003(mcompcode, ddlBranch.SelectedValue.Trim(), statuscode).Copy();
                                    ReportDataHandler oReportDataHandler = new ReportDataHandler();
                                    //DataTable dtbl = new DataTable();

                                    //dtbl = oReportDataHandler.populateDepartmentName(mdeptcode).Copy();

                                    mrptsubheader = companyname + "   |   Branch : " + ddlBranch.SelectedItem.Text.Trim();
                                    mrptLawerheader = "Employee Status : " + ddlEmpStatus.SelectedItem.Text.Trim();
                                }
                            }
                        }
                    }
                    else
                    {
                        dtreport = reportDataHandler.opopulaterep0003IND(mempcode, statuscode).Copy();
                        ReportDataHandler oReportDataHandler = new ReportDataHandler();
                        DataTable dtbl = new DataTable();

                        dtbl = oReportDataHandler.populateEmployeeName(mempcode).Copy();

                        mrptsubheader = "Employee Name : " + dtbl.Rows[0]["FULL_NAME"].ToString();
                        mrptLawerheader = "Employee Status : " + ddlEmpStatus.SelectedItem.Text.Trim();
                    }
                }

                ReportDataHandler oRptDH = new ReportDataHandler();
                dtreport = oRptDH.getEmployeeReportDataTable(dtreport.Copy()).Copy();


                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        mrptsubheader = "All Companies";
                    }
                    else
                    {

                        ReportDataHandler oReportDataHandler = new ReportDataHandler();
                        DataTable dtbl = new DataTable();
                        dtbl = oReportDataHandler.populateEmployeeName(mempcode).Copy();
                        mrptsubheader = "Employee Name : " + dtbl.Rows[0]["FULL_NAME"].ToString();
                    }
                }

                //ReportViewer1.Reset();
                //ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rptscr);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/Employee.rdlc");
                //ReportParameter[] param = new ReportParameter[3];
                //param[0] = new ReportParameter("headerpara", mrptheader);
                //param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                //param[2] = new ReportParameter("Lawersubheaderpara", mrptLawerheader);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();



                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("headerpara", mrptheader);
                paramdict.Add("subheaderpara", mrptsubheader);
                paramdict.Add("Lawersubheaderpara", mrptLawerheader);

                Session["rptDataSet"] = dtreport;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = mrptheader;

                Response.Redirect("~/Reports/ReportViewers/Employee.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                reportDataHandler = null;
                dtreport.Dispose();
                dtreport = null;
            }
        }

        void rep010()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable EMPLOYMENT_TYPE_CADRE_ANALYSIS = new DataTable();
            DataTable GENDER_CADRE_ANALYSIS = new DataTable();
            DataTable DESIGNATION_ANALYSIS_CADRE_ANALYSIS = new DataTable();
            DataTable STAFF_CATEGORY_CADRE_ANALYSIS = new DataTable();
            DataTable STAFF_TENURE_CADRE_ANALYSIS = new DataTable();
            try
            {
                string SubHeader = String.Empty;
                string LowerHeader = String.Empty;
                string CompID = dpCompID.SelectedValue;

                if (dpCompID.SelectedValue == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    SubHeader = "All Companies";
                    EMPLOYMENT_TYPE_CADRE_ANALYSIS = reportDataHandler.populaterep0010EmploymanetTypeAllCompany().Copy();
                    GENDER_CADRE_ANALYSIS = reportDataHandler.populaterep0010GenderAllCompany().Copy();
                    DESIGNATION_ANALYSIS_CADRE_ANALYSIS = reportDataHandler.populaterep0010DesignationAnalysisAllCompany().Copy();
                    STAFF_CATEGORY_CADRE_ANALYSIS = reportDataHandler.populaterep0010StaffCategoryAllCompany().Copy();
                    STAFF_TENURE_CADRE_ANALYSIS = reportDataHandler.populaterep0010StaffTenureAllCompany().Copy();
                }
                else
                {
                    SubHeader = dpCompID.SelectedItem.Text;


                    EMPLOYMENT_TYPE_CADRE_ANALYSIS = reportDataHandler.populaterep0010EmploymanetTypeINDCompany(CompID).Copy();
                    GENDER_CADRE_ANALYSIS = reportDataHandler.populaterep0010GenderINDCompany(CompID).Copy();
                    DESIGNATION_ANALYSIS_CADRE_ANALYSIS = reportDataHandler.populaterep0010DesignationAnalysisINDCompany(CompID).Copy();
                    STAFF_CATEGORY_CADRE_ANALYSIS = reportDataHandler.populaterep0010StaffCategoryINDCompany(CompID).Copy();
                    STAFF_TENURE_CADRE_ANALYSIS = reportDataHandler.populaterep0010StaffTenureINDCompany(CompID).Copy();
                }


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("SubHeader", SubHeader);
                paramdict.Add("LowerHeader", LowerHeader);

                Session["rptDataSet1"] = EMPLOYMENT_TYPE_CADRE_ANALYSIS;
                Session["rptDataSet2"] = GENDER_CADRE_ANALYSIS;
                Session["rptDataSet3"] = DESIGNATION_ANALYSIS_CADRE_ANALYSIS;
                Session["rptDataSet4"] = STAFF_CATEGORY_CADRE_ANALYSIS;
                Session["rptDataSet5"] = STAFF_TENURE_CADRE_ANALYSIS;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Cadre Analysis Report";

                Response.Redirect("~/Reports/ReportViewers/rptCadreAnalysisReport.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                reportDataHandler = null;
            }
        }

        void rep010_OLD()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtDesignationAnalysis = new DataTable();
            DataTable dtEmployeetype = new DataTable();
            DataTable dtGenderAnalysis = new DataTable();
            DataTable dtStaffStrength = new DataTable();
            DataTable dtStaffTenure = new DataTable();

            try
            {
                dtDesignationAnalysis = reportDataHandler.populaterep0010_DesignationAnalysis(mcompcode, mfromdate, mtodate).Copy();
                dtEmployeetype = reportDataHandler.populaterep0010_Employeetype(mcompcode, mfromdate, mtodate).Copy();
                dtGenderAnalysis = reportDataHandler.populaterep0010_GenderAnalysis(mcompcode, mfromdate, mtodate).Copy();
                dtStaffStrength = reportDataHandler.populaterep0010_StaffStrength(mcompcode, mfromdate, mtodate).Copy();
                dtStaffStrength = reportDataHandler.populaterep0010_StaffStrength(mcompcode, mfromdate, mtodate).Copy();
                dtStaffTenure = reportDataHandler.populaterep0010_StaffTenure(mcompcode, mfromdate, mtodate).Copy();

                string mrptsubheader = "";
                string mrptheader = "Cadre Analysis Report";
                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    mrptsubheader = "ALL Companies";
                }
                else
                {
                    mrptsubheader = "Selected Company";
                }

                string mrptLawerheader = "From " + mfromdate.ToString() + " To " + mtodate.ToString();

                //ReportViewer1.Reset();
                //ReportDataSource rptscr_dtEmployeetype = new ReportDataSource("DataSet1", dtEmployeetype);
                //ReportDataSource rptscr_dtGenderAnalysis = new ReportDataSource("DataSet2", dtGenderAnalysis);
                //ReportDataSource rptscr_dtDesignationAnalysis = new ReportDataSource("DataSet3", dtDesignationAnalysis);
                //ReportDataSource rptscr_dtStaffStrength = new ReportDataSource("DataSet4", dtStaffStrength);
                //ReportDataSource rptscr_dtStaffTenure = new ReportDataSource("DataSet5", dtStaffTenure);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rptscr_dtDesignationAnalysis);
                //ReportViewer1.LocalReport.DataSources.Add(rptscr_dtEmployeetype);
                //ReportViewer1.LocalReport.DataSources.Add(rptscr_dtGenderAnalysis);
                //ReportViewer1.LocalReport.DataSources.Add(rptscr_dtStaffStrength);
                //ReportViewer1.LocalReport.DataSources.Add(rptscr_dtStaffTenure);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/StaffStrenth.rdlc");
                //ReportParameter[] param = new ReportParameter[3];
                //param[0] = new ReportParameter("headerpara", mrptheader);
                //param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                //param[2] = new ReportParameter("Lawersubheaderpara", mrptLawerheader);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("headerpara", mrptheader);
                paramdict.Add("subheaderpara", mrptsubheader);
                paramdict.Add("Lawersubheaderpara", mrptLawerheader);

                Session["rptDataSet1"] = dtEmployeetype;
                Session["rptDataSet2"] = dtGenderAnalysis;
                Session["rptDataSet3"] = dtDesignationAnalysis;
                Session["rptDataSet4"] = dtStaffStrength;
                Session["rptDataSet5"] = dtStaffTenure;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Cadre Analysis Report";

                Response.Redirect("~/Reports/ReportViewers/StaffStrenth.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                reportDataHandler = null;
                dtDesignationAnalysis.Dispose();
                dtDesignationAnalysis = null;
                dtEmployeetype.Dispose();
                dtEmployeetype = null;
                dtGenderAnalysis.Dispose();
                dtGenderAnalysis = null;
                dtStaffStrength.Dispose();
                dtStaffStrength = null;
            }
        }

        void rep012()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable dtUOAbsent = new DataTable();

            string mrptsubheader = "";

            try
            {
                dtUOAbsent = reportDataHandler.populaterep0012_UOAbsent(mcompcode, mfromdate, mtodate, mempcode, mdeptcode).Copy();

                int iCount = dtUOAbsent.Rows.Count;

                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        mrptsubheader = "All Companies";
                    }
                    else
                    {
                        mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }
                else
                {
                    string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);
                    if (mempcode == "" && mdeptcode == "")
                    {
                        mrptsubheader = companyname;
                    }
                    else if (mempcode != "" && mdeptcode == "")
                    {
                        mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                    else if (mdeptcode != "")
                    {
                        mrptsubheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                    }
                    else
                    {
                        mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }

                string sFromTodate = "";

                if (mfromdate == mtodate)
                {
                    sFromTodate = "Date : " + mfromdate;
                }
                else
                {
                    sFromTodate = "From : " + mfromdate + " To : " + mtodate;
                }
                //ReportViewer1.Reset();
                //ReportDataSource rdsUOAbsent = new ReportDataSource("DataSet1", dtUOAbsent);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rdsUOAbsent);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptUOAbsentReport.rdlc");
                //ReportParameter[] param = new ReportParameter[2];
                //param[0] = new ReportParameter("rpCompany", mrptsubheader);
                //param[1] = new ReportParameter("rpFrom", sFromTodate);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("rpCompany", mrptsubheader);
                paramdict.Add("rpFrom", sFromTodate);

                Session["rptDataSet"] = dtUOAbsent;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Absent Report";

                Response.Redirect("~/Reports/ReportViewers/rptUOAbsentReport.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                //reportDataHandler = null;
                //dtUOAbsent.Dispose();
            }
        }

        void rep013()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable odtOt = new DataTable();

            string mrptsubheader = "";

            try
            {
                if (ddlBranch.Visible == true)
                {
                    mdeptcode = ddlBranch.SelectedValue.ToString().Trim();
                }
                else
                {
                    mdeptcode = ddlDepartment.SelectedValue.ToString().Trim();
                }

                odtOt = reportDataHandler.populaterep0013_OT(mcompcode, mfromdate, mtodate, mempcode, mdeptcode).Copy();

                int iCount = odtOt.Rows.Count;
                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        mrptsubheader = "All Companies";
                    }
                    else
                    {
                        mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }
                else
                {
                    string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);

                    if (mempcode == "" && mdeptcode == "")
                    {
                        mrptsubheader = companyname;
                    }
                    else if (mempcode != "" && mdeptcode == "")
                    {
                        mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                    else if (mdeptcode != "")
                    {
                        string prefix = mdeptcode[0].ToString() + mdeptcode[1].ToString();

                        if (prefix == Constants.DEPARTMENT_ID_STAMP)
                        {
                            mrptsubheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                        }
                        else
                        {
                            mrptsubheader = companyname + Environment.NewLine + Environment.NewLine + "Branch : " + reportDataHandler.populateBranchName(mdeptcode).Rows[0]["BRANCH_NAME"].ToString();
                        }
                    }
                    else
                    {
                        mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }

                //ReportViewer1.Reset();
                //ReportDataSource rdsOT = new ReportDataSource("DataSet1", odtOt);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rdsOT);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptOTReport.rdlc");
                //ReportParameter[] param = new ReportParameter[2];
                DateTime dtDate = DateTime.Today.AddMonths(-1);

                string sDate = "";

                if (mfromdate == mtodate)
                {
                    sDate = "Date : " + mfromdate;
                }
                else
                {
                    sDate = "From : " + mfromdate + " To : " + mtodate;
                }

                //param[0] = new ReportParameter("paramCompany", mrptsubheader);
                //param[1] = new ReportParameter("paramMonth", sDate);
                ////param[2] = new ReportParameter("rpTo", mtodate);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("paramCompany", mrptsubheader);
                paramdict.Add("paramMonth", sDate);

                Session["rptDataSet"] = odtOt;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Employee Extra Hours Worked";

                Response.Redirect("~/Reports/ReportViewers/rptOTReport.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                //reportDataHandler = null;
                //dtUOAbsent.Dispose();

            }
        }

        void rep014()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable companyProperty = new DataTable();

            try
            {
                string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);

                companyProperty = reportDataHandler.populaterep0014_Property(mcompcode).Copy();

                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    companyname = "All Company ";
                }
                else
                {
                    companyname = companyname;
                }
                int iCount = companyProperty.Rows.Count;

                //ReportViewer1.Reset();
                //ReportDataSource rdsProperty = new ReportDataSource("DataSet1", companyProperty);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/CompanyProperties.rdlc");
                //ReportParameter[] param = new ReportParameter[1];

                //param[0] = new ReportParameter("paraCompany", companyname);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("paraCompany", companyname);

                Session["rptDataSet"] = companyProperty;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Company Property Report";

                Response.Redirect("~/Reports/ReportViewers/CompanyProperties.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {

            }
        }

        void rep015()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable companyProperty = new DataTable();

            try
            {
                string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);

                companyProperty = reportDataHandler.populaterep0015_Property(mcompcode).Copy();

                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    companyname = "All Company ";
                }
                else
                {
                    companyname = companyname;
                }

                int iCount = companyProperty.Rows.Count;

                //ReportViewer1.Reset();
                //ReportDataSource rdsProperty = new ReportDataSource("DataSet1", companyProperty);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/AvailableProperties.rdlc");
                //ReportParameter[] param = new ReportParameter[1];

                //param[0] = new ReportParameter("companyName", companyname);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("companyName", companyname);

                Session["rptDataSet"] = companyProperty;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Available properties in Company";

                Response.Redirect("~/Reports/ReportViewers/AvailableProperties.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {

            }
        }

        void rep016()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            EmployeePropertyDetailsDataHandler employeePropertyDetailsDataHandler = new EmployeePropertyDetailsDataHandler();
            DataTable companyProperty = new DataTable();

            try
            {
                //string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);
                string employeeName = employeePropertyDetailsDataHandler.getEmployeeNameForProperty(mempcode);

                companyProperty = reportDataHandler.populaterep0016_Property(mcompcode, mempcode).Copy();

                int iCount = companyProperty.Rows.Count;

                //ReportViewer1.Reset();
                //ReportDataSource rdsOT = new ReportDataSource("DataSet1", companyProperty);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rdsOT);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/EmployeeProperties.rdlc");
                //ReportParameter[] param = new ReportParameter[1];

                //param[0] = new ReportParameter("paraEmployeeName", employeeName);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("paraEmployeeName", employeeName);

                Session["rptDataSet"] = companyProperty;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Employee property Report";

                Response.Redirect("~/Reports/ReportViewers/EmployeeProperties.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {

            }
        }

        void rep017()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable employeeResignation = new DataTable();
            string mrptsubheader = "";
            try
            {
                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        employeeResignation = reportDataHandler.populaterep0017_EmpResignationGroupByCompany(mfromdate, mtodate).Copy();
                    }
                    else
                    {
                        employeeResignation = reportDataHandler.populaterep0017_EmpResignationByEmployee(mempcode, mfromdate, mtodate).Copy();
                    }
                }
                else
                {
                    if (mempcode == "")
                    {
                        if (mdeptcode == "")
                        {
                            employeeResignation = reportDataHandler.populaterep0017_EmpResignation(mcompcode, mfromdate, mtodate).Copy();
                        }
                        else
                        {
                            employeeResignation = reportDataHandler.populaterep0017_EmpResignationByDepartment(mdeptcode, mfromdate, mtodate).Copy();
                        }
                    }
                    else
                    {
                        employeeResignation = reportDataHandler.populaterep0017_EmpResignationByEmployee(mempcode, mfromdate, mtodate).Copy();
                    }

                }

                int iCount = employeeResignation.Rows.Count;

                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        mrptsubheader = "All Companies";
                    }
                    else
                    {
                        mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }
                else
                {
                    string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);
                    if (mempcode == "" && mdeptcode == "")
                    {
                        mrptsubheader = companyname;
                    }
                    else if (mempcode != "" && mdeptcode == "")
                    {
                        mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                    else if (mdeptcode != "")
                    {
                        mrptsubheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                    }
                    else
                    {
                        mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }
                string sFromTodate = "";
                if (mfromdate == mtodate)
                {
                    if (mempcode == "")
                    {
                        sFromTodate = "Date : " + mfromdate;
                    }
                    else
                    {
                        sFromTodate = " ";
                    }
                }
                else
                {
                    sFromTodate = "From : " + mfromdate + " To : " + mtodate;
                }

                //ReportViewer1.Reset();
                //ReportDataSource rdsProperty = new ReportDataSource("DataSet1", employeeResignation);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/EmployeeResignation.rdlc");
                //ReportParameter[] param = new ReportParameter[2];

                ////param[0] = new ReportParameter("paramCompanyName", companyname);
                //param[0] = new ReportParameter("paramSubHeader", mrptsubheader);
                //param[1] = new ReportParameter("paramFromtoDate", sFromTodate);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("paramSubHeader", mrptsubheader);
                paramdict.Add("paramFromtoDate", sFromTodate);

                Session["rptDataSet"] = employeeResignation;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Employee Resignation Report";

                Response.Redirect("~/Reports/ReportViewers/EmployeeResignation.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {

            }
        }

        void rep018()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable employeeResignation = new DataTable();
            string mrptsubheader = "";

            try
            {

                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mdeptcode == "")
                    {
                        employeeResignation = reportDataHandler.populaterep0018_NewRecrutmentsGroupByCompany(mfromdate, mtodate).Copy();
                    }
                }
                else
                {
                    if (mdeptcode == "")
                    {
                        employeeResignation = reportDataHandler.populaterep0018_NewRecrutments(mcompcode, mfromdate, mtodate).Copy();
                    }
                    else
                    {
                        employeeResignation = reportDataHandler.populaterep0018_NewRecrutmentsByEmployee(mdeptcode, mfromdate, mtodate).Copy();
                    }

                }

                int iCount = employeeResignation.Rows.Count;

                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    mrptsubheader = "All Companies";
                }
                else
                {
                    string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);
                    if (mempcode == "" && mdeptcode == "")
                    {
                        mrptsubheader = companyname;
                    }
                    else if (mempcode != "" && mdeptcode == "")
                    {
                        mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                    else if (mdeptcode != "")
                    {
                        mrptsubheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                    }
                    else
                    {
                        mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }


                //ReportViewer1.Reset();
                //ReportDataSource rdsProperty = new ReportDataSource("DataSet1", employeeResignation);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/EmployeeNewRecrutments.rdlc");
                //ReportParameter[] param = new ReportParameter[2];

                string sDate = "";
                if (mfromdate == mtodate)
                {
                    sDate = "Date : " + mfromdate;
                }
                else
                {
                    sDate = "From : " + mfromdate + " To : " + mtodate;
                }
                //param[0] = new ReportParameter("paramCompany", mrptsubheader);
                //param[1] = new ReportParameter("paramMonth", sDate);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("paramCompany", mrptsubheader);
                paramdict.Add("paramMonth", sDate);

                Session["rptDataSet"] = employeeResignation;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Employee Recruitment Report";

                Response.Redirect("~/Reports/ReportViewers/EmployeeNewRecrutments.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {

            }
        }

        void rep020()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable employeeDetail = new DataTable();
            string currentDate = DateTime.Today.ToString("dd-MM-yyyy");
            try
            {
                string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);

                employeeDetail = reportDataHandler.populaterep0020_LetterOfEmployeement(mempcode).Copy();

                int iCount = employeeDetail.Rows.Count;

                //ReportViewer1.Reset();
                //ReportDataSource rdsProperty = new ReportDataSource("DataSet1", employeeDetail);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptLetterOfEmployeements.rdlc");
                //ReportParameter[] param = new ReportParameter[1];

                //param[0] = new ReportParameter("parmCurrentDate", currentDate);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("parmCurrentDate", currentDate);

                Session["rptDataSet"] = employeeDetail;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Letter of Employment";

                Response.Redirect("~/Reports/ReportViewers/rptLetterOfEmployeements.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {

            }
        }

        void rep021()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable employeeRosterDetail = new DataTable();
            DataTable dateTable = new DataTable();

            string currentDate = DateTime.Today.ToString("dd-MM-yyyy");

            try
            {
                string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);
                string mrptsubheader = "";
                employeeRosterDetail = reportDataHandler.populaterep0021_MonthlyRoster(mcompcode, mdeptcode, mempcode, mfromdate, mtodate).Copy();
                //dateTable = reportDataHandler.populateDate(mfromdate);
                string sDate = "";
                int iCount = employeeRosterDetail.Rows.Count;

                if (mdeptcode == "")
                {
                    if (mempcode == "")
                    {
                        //company id
                        mrptsubheader = companyname;
                    }
                    else
                    {
                        //employee id
                        string companyByEmp = reportDataHandler.getEmployeeCompany(mempcode);
                        mrptsubheader = companyByEmp + Environment.NewLine + Environment.NewLine + reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }
                else
                {
                    //dept id
                    mrptsubheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                }

                //ReportViewer1.Reset();
                //ReportDataSource rdsProperty = new ReportDataSource("DataSet2", employeeRosterDetail);
                //ReportDataSource rdsProperty2 = new ReportDataSource("DataSet1", dateTable);

                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                //ReportViewer1.LocalReport.DataSources.Add(rdsProperty2);

                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptRoster.rdlc");
                if (mfromdate == mtodate)
                {
                    sDate = "Date : " + mfromdate;
                }
                else
                {
                    sDate = "From : " + mfromdate + " To : " + mtodate;
                }

                //ReportParameter[] param = new ReportParameter[2];
                //param[0] = new ReportParameter("parmCompanyName", mrptsubheader);
                //param[1] = new ReportParameter("parmtodate", sDate);

                //ReportViewer1.LocalReport.SetParameters(param);

                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("parmCompanyName", mrptsubheader);
                paramdict.Add("parmtodate", sDate);

                Session["employeeRosterDetail"] = employeeRosterDetail;
                Session["dateTable"] = dateTable;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Duty Roster Report";

                Response.Redirect("~/Reports/ReportViewers/rptRoster.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {

            }
        }

        void rep022()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable employeeMissingDetail = new DataTable();
            string currentDate = DateTime.Today.ToString("dd-MM-yyyy");
            string mrptNameheader = "";

            try
            {

                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        employeeMissingDetail = reportDataHandler.populaterep0022_MissingEmployee(mcompcode, mfromdate, mtodate).Copy();
                    }
                    else
                    {
                        employeeMissingDetail = reportDataHandler.populaterep0022_MissingEmployeeForEmployee(mfromdate, mtodate, mempcode).Copy();
                    }
                }
                else
                {
                    if (mempcode == "")
                    {
                        if (chkBranch.Checked == true)
                        {
                            //populate data with branch
                            employeeMissingDetail = reportDataHandler.populaterep0022_MissingEmployeeForBranch(ddlBranch.SelectedValue.Trim(), mfromdate, mtodate).Copy();
                        }
                        else
                        {
                            if (mdeptcode == "")
                            {
                                employeeMissingDetail = reportDataHandler.populaterep0022_MissingEmployeeForCompany(mcompcode, mfromdate, mtodate).Copy();
                            }
                            else
                            {
                                employeeMissingDetail = reportDataHandler.populaterep0022_MissingEmployeeForDepartment(mdeptcode, mfromdate, mtodate).Copy();
                            }
                        }
                    }
                    else
                    {
                        employeeMissingDetail = reportDataHandler.populaterep0022_MissingEmployeeForEmployee(mfromdate, mtodate, mempcode).Copy();
                    }
                }

                int iCount = employeeMissingDetail.Rows.Count;

                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        mrptNameheader = "All Companies";
                    }
                    else
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }

                }
                else
                {
                    string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);
                    if (mempcode == "" && mdeptcode == "" && ddlBranch.SelectedValue.Trim() == "")
                    {
                        mrptNameheader = companyname;
                    }
                    else if (mempcode != "" && mdeptcode == "")
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                    else if (mdeptcode != "")
                    {
                        mrptNameheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                    }
                    else if (ddlBranch.SelectedValue.Trim() != "")
                    {
                        mrptNameheader = companyname + Environment.NewLine + Environment.NewLine + "Branch : " + ddlBranch.SelectedItem;
                    }
                    else
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }

                //ReportViewer1.Reset();
                //ReportDataSource rdsProperty = new ReportDataSource("DataSet1", employeeMissingDetail);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptMissingEmployeements.rdlc");
                //ReportParameter[] param = new ReportParameter[3];

                string mrptsubheader = "";
                if (mfromdate == mtodate)
                {
                    mrptsubheader = "Date : " + mfromdate;
                }
                else
                {
                    mrptsubheader = "From : " + mfromdate + " To : " + mtodate;
                }

                //param[0] = new ReportParameter("paramCurrentDate", currentDate);
                //param[1] = new ReportParameter("pramCompany", mrptNameheader);
                //param[2] = new ReportParameter("parmFromDate", mrptsubheader);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("paramCurrentDate", currentDate);
                paramdict.Add("pramCompany", mrptNameheader);
                paramdict.Add("parmFromDate", mrptsubheader);

                Session["rptDataSet"] = employeeMissingDetail;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Missing IN - OUT Report";

                Response.Redirect("~/Reports/ReportViewers/rptMissingEmployeements.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {

            }
        }

        void rep023()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable employeeEarlyOff = new DataTable();
            string mrptNameheader = "";
            try
            {
                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        employeeEarlyOff = reportDataHandler.populaterep0023_EarlyOff(mcompcode, mfromdate, mtodate).Copy();
                    }
                    else
                    {
                        employeeEarlyOff = reportDataHandler.populaterep0023_EarlyOffByEmployee(mempcode, mfromdate, mtodate).Copy();
                    }
                }
                else
                {
                    if (mempcode == "")
                    {
                        if (mdeptcode == "")
                        {
                            employeeEarlyOff = reportDataHandler.populaterep0023_EarlyOffByCompany(mcompcode, mfromdate, mtodate).Copy();
                        }
                        else
                        {
                            employeeEarlyOff = reportDataHandler.populaterep0023_EarlyOffByDepartment(mdeptcode, mfromdate, mtodate).Copy();
                        }
                    }
                    else
                    {
                        employeeEarlyOff = reportDataHandler.populaterep0023_EarlyOffByEmployee(mempcode, mfromdate, mtodate).Copy();
                    }
                }

                int iCount = employeeEarlyOff.Rows.Count;

                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        mrptNameheader = "All Companies";
                    }
                    else
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }

                }
                else
                {
                    string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);
                    if (mempcode == "" && mdeptcode == "")
                    {
                        mrptNameheader = companyname;
                    }
                    else if (mempcode != "" && mdeptcode == "")
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                    else if (mdeptcode != "")
                    {
                        mrptNameheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                    }
                    else
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }

                string sFromTodate = "";
                if (mfromdate == mtodate)
                {
                    sFromTodate = "Date : " + mfromdate;
                }
                else
                {
                    sFromTodate = "From : " + mfromdate + " To : " + mtodate;
                }
                //ReportViewer1.Reset();
                //ReportDataSource rdsProperty = new ReportDataSource("DataSet1", employeeEarlyOff);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptEarlyOffReport.rdlc");
                //ReportParameter[] param = new ReportParameter[2];

                //param[0] = new ReportParameter("paramCompany", mrptNameheader);
                //param[1] = new ReportParameter("parmFromDate", sFromTodate);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("paramCompany", mrptNameheader);
                paramdict.Add("parmFromDate", sFromTodate);

                Session["rptDataSet"] = employeeEarlyOff;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Missing IN - OUT Report";

                Response.Redirect("~/Reports/ReportViewers/rptEarlyOffReport.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {

            }
        }

        void rep024()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable nopayDataTable = new DataTable();
            string mrptNameheader = "";

            if (ddlBranch.Visible == true)
            {
                mdeptcode = "N/A";
            }

            try
            {
                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        nopayDataTable = reportDataHandler.populaterep0024_Nopay(mcompcode, mfromdate, mtodate).Copy();
                    }
                    else
                    {
                        nopayDataTable = reportDataHandler.populaterep0024_NopayForEmployee(mempcode, mfromdate, mtodate).Copy();
                    }
                }
                else
                {
                    if (mempcode == "")
                    {
                        if (chkBranch.Checked == true)
                        {
                            //populate data with branch
                            nopayDataTable = reportDataHandler.populaterep0024_NopayForBranch(ddlBranch.SelectedValue.Trim(), mfromdate, mtodate).Copy();
                        }
                        else
                        {
                            if (mdeptcode == "")
                            {
                                nopayDataTable = reportDataHandler.populaterep0024_NopayForCompany(mcompcode, mfromdate, mtodate).Copy();
                            }
                            else
                            {
                                nopayDataTable = reportDataHandler.populaterep0024_NopayForDepartment(mdeptcode, mfromdate, mtodate).Copy();
                            }
                        }
                    }
                    else
                    {
                        nopayDataTable = reportDataHandler.populaterep0024_NopayForEmployee(mempcode, mfromdate, mtodate).Copy();
                    }
                }

                string sFromTodate = "";

                if (mfromdate == mtodate)
                {
                    sFromTodate = "Date : " + mfromdate;
                }
                else
                {
                    sFromTodate = "From : " + mfromdate + " To : " + mtodate;
                }
                int iCount = nopayDataTable.Rows.Count;

                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        mrptNameheader = "All Companies";
                    }
                    else
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }

                }
                else
                {

                    string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);
                    if (mempcode == "" && mdeptcode == "")
                    {
                        mrptNameheader = companyname;
                    }
                    else if (mempcode != "" && mdeptcode == "")
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                    else if (mdeptcode != "" && mdeptcode == Constants.DEPARTMENT_ID_STAMP)
                    {
                        mrptNameheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                    }
                    else if (mdeptcode != "" && mdeptcode != Constants.DEPARTMENT_ID_STAMP)
                    {
                        mrptNameheader = companyname + Environment.NewLine + Environment.NewLine + "Branch : " + ddlBranch.SelectedItem;
                    }
                    else
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }

                //ReportViewer1.Reset();
                //ReportDataSource rdsProperty = new ReportDataSource("DataSet1", nopayDataTable);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptNopay.rdlc");

                //ReportParameter[] param = new ReportParameter[2];
                //param[0] = new ReportParameter("paramCompany", mrptNameheader);
                //param[1] = new ReportParameter("prmFromDate", sFromTodate);
                //ReportViewer1.LocalReport.SetParameters(param);

                //ReportViewer1.LocalReport.Refresh();

                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("paramCompany", mrptNameheader);
                paramdict.Add("prmFromDate", sFromTodate);

                Session["rptDataSet"] = nopayDataTable;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "No Pay Report";

                Response.Redirect("~/Reports/ReportViewers/rptNopay.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {

            }
        }

        void rep025()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable attRegDataTable = new DataTable();


            string currentDate = "";
            try
            {
                if (mfromdate == mtodate)
                {
                    currentDate = "Date : " + mfromdate;
                }
                else
                {
                    currentDate = "From : " + mfromdate + " To : " + mtodate;
                }

                string companyName = companyDataHandler.getCompanyNameByCompanyId(mcompcode);

                //string deptName = "";

                //if ((mdeptcode.Trim() != "") || (mdeptcode.Trim() != null))
                //{
                //    deptName = reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                //}

                attRegDataTable = reportDataHandler.populaterep0025_AttReg(mcompcode, mfromdate, mtodate).Copy();
                int icount = attRegDataTable.Rows.Count;

                //ReportViewer1.Reset();
                //ReportDataSource rdsAttreg = new ReportDataSource("DataSet1", attRegDataTable);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rdsAttreg);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptAttendanceRegistration.rdlc");

                //ReportParameter[] param = new ReportParameter[2];
                //param[0] = new ReportParameter("paramDate", currentDate);
                //param[1] = new ReportParameter("paramCompany", companyName);
                ////param[2] = new ReportParameter("paramDepartment", deptName);


                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("paramDate", currentDate);
                paramdict.Add("paramCompany", companyName);

                Session["rptDataSet"] = attRegDataTable;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Attendance Device Registration Report";

                Response.Redirect("~/Reports/ReportViewers/rptAttendanceRegistration.aspx");

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {

            }
        }

        void rep026()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable continuousDataTable = new DataTable();
            string mrptNameheader = "";
            string currentDate = DateTime.Today.ToString("dd-MM-yyyy");

            try
            {
                continuousDataTable = reportDataHandler.populaterep0026_ContinuousAbsent(mcompcode, mdeptcode, mfromdate, mtodate, mempcode).Copy();

                DateTime startDate = new DateTime();
                string id = null;
                string v_company = null;
                string v_branch = null;
                string v_department = null;
                string v_division = null;
                string v_epf = null;
                string v_name = null;

                int count = 0;
                int hCount = 0;

                foreach (DataRow dataRow in continuousDataTable.Rows)
                {
                    string company = dataRow["COMP_NAME"].ToString();
                    string branch = dataRow["BRANCH_NAME"].ToString();
                    string department = dataRow["DEPT_NAME"].ToString();
                    string division = dataRow["DIV_NAME"].ToString();
                    string empId = dataRow["EMPLOYEE_ID"].ToString();
                    string epf = dataRow["EPF_NO"].ToString();
                    string name = dataRow["INITIALS_NAME"].ToString();
                    string remarks = dataRow["REMARK"].ToString();
                    DateTime indate = Convert.ToDateTime(dataRow["IN_DATE"].ToString());


                    if (startDate == DateTime.MinValue)
                    {
                        if (remarks == "Working Day")
                        {
                            startDate = indate.AddDays(1);
                            count = count + 1;
                            id = empId;
                            v_company = company;
                            v_branch = branch;
                            v_department = department;
                            v_division = division;
                            v_epf = epf;
                            v_name = name;
                        }
                    }
                    else if (startDate == indate && id == empId && remarks == "Working Day")
                    {
                        count = count + 1;
                        startDate = startDate.AddDays(1);
                    }
                    //else if ((remarks == "Other Holiday" || remarks == "Poya") && startDate == indate )
                    else if (remarks != "Working Day" && startDate == indate)
                    {
                        startDate = startDate.AddDays(1);
                        hCount = hCount + 1;
                    }
                    else if (startDate != indate || id != empId)
                    {
                        if (count >= 3)
                        {
                            DateTime fromDate = startDate.AddDays(-(count + hCount));
                            DateTime toDate = startDate.AddDays(-1);

                            bool status = reportDataHandler.Insert(v_company, v_branch, v_department, v_division, id, v_epf, v_name, count, fromDate, toDate);
                        }

                        startDate = indate.AddDays(1);
                        id = empId;
                        v_branch = branch;
                        v_department = department;
                        v_division = division;
                        v_epf = epf;
                        v_name = name;
                        v_company = company;

                        if (remarks != "Working Day")
                        {
                            count = 0;
                            hCount = 1;
                        }
                        else
                        {
                            count = 1;
                            hCount = 0;
                        }
                    }

                }

                if (count >= 3)
                {
                    bool status = reportDataHandler.Insert(v_company, v_branch, v_department, v_division, id, v_epf, v_name, count, startDate.AddDays(-count), startDate.AddDays(-1));
                }

                continuousDataTable = new DataTable();
                continuousDataTable = reportDataHandler.tempContinuousAbsent().Copy();

                int iCount = continuousDataTable.Rows.Count;

                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        mrptNameheader = "All Companies";
                    }
                    else
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }
                else
                {
                    string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);
                    if (mempcode == "" && mdeptcode == "")
                    {
                        mrptNameheader = companyname;
                    }
                    else if (mempcode != "" && mdeptcode == "")
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                    else if (mdeptcode != "")
                    {
                        mrptNameheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                    }
                    else
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }

                string sFromTodate = "";
                if (mfromdate == mtodate)
                {
                    sFromTodate = "Date : " + mfromdate;
                }
                else
                {
                    sFromTodate = "From : " + mfromdate + " To : " + mtodate;
                }

                //ReportViewer1.Reset();
                //ReportDataSource rdsProperty = new ReportDataSource("DataSet1", continuousDataTable);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptContinuousAbsent.rdlc");

                //ReportParameter[] param = new ReportParameter[2];
                //param[0] = new ReportParameter("paramDate", sFromTodate);
                //param[1] = new ReportParameter("paramCompany", mrptNameheader);
                //ReportViewer1.LocalReport.SetParameters(param);

                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("paramDate", sFromTodate);
                paramdict.Add("paramCompany", mrptNameheader);

                Session["rptDataSet"] = continuousDataTable;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Continuous Absent Report";

                Response.Redirect("~/Reports/ReportViewers/rptContinuousAbsent.aspx");

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        void rep027()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            DataTable leaveDetail = new DataTable();

            string currentDate = DateTime.Today.ToString("dd-MM-yyyy");

            try
            {
                //string companyName  = employeeDataHandler.getCompanyNameByEmployeeId(employeeId);
                //string employeeName = employeeDataHandler.getEmployeeName(employeeId);
                //string deptName     = employeeDataHandler.getDepartmentNameByEmployeeId(employeeId);
                //string branchName   = employeeDataHandler.getBranchNameByEmployeeId(employeeId);
                //string dateRange = "From " + mfromdate.Trim() + " To " + mtodate.Trim();

                string sFromTodate = "";
                string mrptNameheader = "";

                if (mfromdate == mtodate)
                {
                    sFromTodate = "Date : " + mfromdate;
                }
                else
                {
                    sFromTodate = "From : " + mfromdate + " To : " + mtodate;
                }

                leaveDetail = reportDataHandler.populaterep0027_LeaveDetailofAnEmployee(mcompcode, mdeptcode, employeeId, mfromdate, mtodate).Copy();

                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    if (mempcode == "")
                    {
                        mrptNameheader = "All Companies";
                    }
                    else
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }

                }
                else
                {

                    string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);
                    if (mempcode == "" && mdeptcode == "")
                    {
                        mrptNameheader = companyname;
                    }
                    else if (mempcode != "" && mdeptcode == "")
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                    else if (mdeptcode != "")
                    {
                        mrptNameheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                    }
                    else
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }


                //ReportViewer1.Reset();
                //ReportDataSource rdsLeaves = new ReportDataSource("DataSet1", leaveDetail);

                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rdsLeaves);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptEmployeeLeaveDetails.rdlc");

                //ReportParameter[] param = new ReportParameter[2];
                //param[0] = new ReportParameter("mrptNameheader", mrptNameheader);
                ////param[1] = new ReportParameter("pDepartment", "Department : " + deptName.Trim());
                ////param[2] = new ReportParameter("pBranch", "Branch : " + branchName.Trim());
                ////param[3] = new ReportParameter("pEmployee", employeeName);
                //param[1] = new ReportParameter("pDateRange", sFromTodate);

                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("mrptNameheader", mrptNameheader);
                paramdict.Add("pDateRange", sFromTodate);

                Session["rptDataSet"] = leaveDetail;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Detail Leave Report";

                Response.Redirect("~/Reports/ReportViewers/rptEmployeeLeaveDetails.aspx");

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {

            }
        }

        void rep028()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtreport = new DataTable();

            try
            {
                string mrptheader = "Off Day Work Report";
                string mrptsubheader = "";
                string mrptLawerheader = "All Companies";

                Dictionary<string, string> dcOffDayWork = new Dictionary<string, string>();
                dcOffDayWork = (Dictionary<string, string>)Session["OffDayWork"];


                string EmployeeID = dcOffDayWork["EmployeeID"];
                string CompanyID = dcOffDayWork["CompanyID"];
                string CompanyName = dcOffDayWork["CompanyName"];
                string DepartmentID = dcOffDayWork["DepartmentID"];
                string DepartmentName = dcOffDayWork["DepartmentName"];
                string FromDate = dcOffDayWork["FromDate"];
                string ToDate = dcOffDayWork["ToDate"];
                if (FromDate == ToDate)
                {
                    mrptsubheader = "Date : " + FromDate.Trim();
                }
                else
                {
                    mrptsubheader = "From : " + FromDate.Trim() + "  To : " + ToDate.Trim();
                }

                if (EmployeeID != "")//Individual Report
                {
                    dtreport = reportDataHandler.populaterep0028IND(EmployeeID.Trim(), FromDate.Trim(), ToDate.Trim());
                    DataTable dtbl = new DataTable();
                    dtbl = reportDataHandler.populateEmployeeName(mempcode).Copy();
                    mrptLawerheader = "Employee Name : " + dtbl.Rows[0]["FULL_NAME"].ToString();
                }
                else if (CompanyID != "" && DepartmentID == "")//Company Wise Reports
                {
                    if (CompanyID == Constants.CON_UNIVERSAL_COMPANY_CODE)//All Company Report
                    {
                        dtreport = reportDataHandler.populaterep0028AllCompany(FromDate.Trim(), ToDate.Trim());
                    }
                    else//Selected Company Report
                    {
                        dtreport = reportDataHandler.populaterep0028INDCompany(CompanyID.Trim(), FromDate.Trim(), ToDate.Trim());
                        mrptLawerheader = CompanyName.Trim();
                    }
                }
                else if (((CompanyID != "") || (CompanyID != Constants.CON_UNIVERSAL_COMPANY_CODE)) && DepartmentID != "")//Department Wise Reports
                {
                    dtreport = reportDataHandler.populaterep0028INDDepartment(DepartmentID.Trim(), FromDate.Trim(), ToDate.Trim());
                    mrptLawerheader = CompanyName.Trim() + Environment.NewLine + Environment.NewLine + "  Department : " + DepartmentName.Trim();
                }

                //ReportViewer1.Reset();
                //ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rptscr);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/OffDayWorkReport.rdlc");
                //ReportParameter[] param = new ReportParameter[3];
                //param[0] = new ReportParameter("headerpara", mrptheader);
                //param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                //param[2] = new ReportParameter("Lawersubheaderpara", mrptLawerheader);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("headerpara", mrptheader);
                paramdict.Add("subheaderpara", mrptsubheader);
                paramdict.Add("Lawersubheaderpara", mrptLawerheader);

                Session["rptDataSet"] = dtreport;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = mrptheader;

                Response.Redirect("~/Reports/ReportViewers/OffDayWorkReport.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                reportDataHandler = null;
                dtreport.Dispose();
                dtreport = null;
            }
        }

        void rep029()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtreport = new DataTable();

            try
            {
                //string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);

                string mrptheader = "Cadre Report";
                string mrptsubheader = "";
                string mrptLawerheader = "";

                Dictionary<string, string> dcStaffStrength = new Dictionary<string, string>();
                dcStaffStrength = (Dictionary<string, string>)Session["StaffStrength"];


                string CompanyID = dcStaffStrength["CompanyID"];
                string CompanyName = dcStaffStrength["CompanyName"];
                string FromDate = dcStaffStrength["FromDate"];
                string ToDate = dcStaffStrength["ToDate"];

                if (CompanyID != "")//Company Wise Reports
                {
                    if (CompanyID == Constants.CON_UNIVERSAL_COMPANY_CODE)//All Company Report
                    {
                        //// Method 01
                        //dtreport = reportDataHandler.getAllCompany().Copy();

                        //DataTable dtbl = new DataTable();
                        //string CompID = dtreport.Rows[0]["COMPANY_ID"].ToString();

                        //dtbl = reportDataHandler.populaterep0029INDCompany(CompID, FromDate.Trim(), ToDate.Trim()).Copy();

                        //for (int i = 1; i < dtreport.Rows.Count; i++)
                        //{
                        //    CompID = dtreport.Rows[i]["COMPANY_ID"].ToString();
                        //    dtbl.Merge(reportDataHandler.populaterep0029INDCompany(CompID, FromDate.Trim(), ToDate.Trim()).Copy());
                        //}

                        //dtreport = new DataTable();
                        //dtreport = dtbl.Copy();

                        ////Method 02
                        dtreport = reportDataHandler.populaterep0029Company(FromDate.Trim(), ToDate.Trim()).Copy();

                        if (FromDate.Trim() == ToDate.Trim())
                        {
                            mrptsubheader = "Date : " + FromDate.Trim();
                        }
                        else
                        {
                            mrptsubheader = "From : " + FromDate.Trim() + "  To : " + ToDate.Trim();
                        }

                        mrptLawerheader = "All Companies";
                    }
                    else//Selected Company Report
                    {
                        dtreport = reportDataHandler.populaterep0029INDCompany(CompanyID.Trim(), FromDate.Trim(), ToDate.Trim()).Copy();
                        if (FromDate.Trim() == ToDate.Trim())
                        {
                            mrptsubheader = "Date : " + FromDate.Trim();
                        }
                        else
                        {
                            mrptsubheader = "From : " + FromDate.Trim() + "  To : " + ToDate.Trim();
                        }
                        mrptLawerheader = CompanyName.Trim();
                    }
                }

                //ReportViewer1.Reset();
                //ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rptscr);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptStaffStrength.rdlc");
                //ReportParameter[] param = new ReportParameter[3];
                //param[0] = new ReportParameter("headerpara", mrptheader);
                //param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                //param[2] = new ReportParameter("Lawersubheaderpara", mrptLawerheader);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("headerpara", mrptheader);
                paramdict.Add("subheaderpara", mrptsubheader);
                paramdict.Add("Lawersubheaderpara", mrptLawerheader);

                Session["rptDataSet"] = dtreport;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = mrptheader;

                Response.Redirect("~/Reports/ReportViewers/rptStaffStrength.aspx");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                reportDataHandler = null;
                dtreport.Dispose();
                dtreport = null;
            }
        }

        void rep030()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtreport = new DataTable();

            try
            {
                //string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);

                string mrptheader = "Employee Skills Report";
                string mrptsubheader = " ";
                string mrptLawerheader = " ";

                Dictionary<string, string> dcEmployeeSkills = new Dictionary<string, string>();
                dcEmployeeSkills = (Dictionary<string, string>)Session["EmployeeSkills"];


                string EmployeeID = dcEmployeeSkills["EmployeeID"];
                string CompanyID = dcEmployeeSkills["CompanyID"];
                string CompanyName = dcEmployeeSkills["CompanyName"];
                string DepartmentID = dcEmployeeSkills["DepartmentID"];
                string DepartmentName = dcEmployeeSkills["DepartmentName"];

                if (EmployeeID != "")//Individual Report
                {
                    dtreport = reportDataHandler.populaterep0030IND(EmployeeID.Trim());
                    //mrptsubheader = "From : ";
                    DataTable dtbl = new DataTable();
                    dtbl = reportDataHandler.populateEmployeeName(EmployeeID).Copy();
                    mrptsubheader = dtbl.Rows[0]["FULL_NAME"].ToString();
                }
                else if (CompanyID != "" && DepartmentID == "")//Company Wise Reports
                {
                    if (CompanyID == Constants.CON_UNIVERSAL_COMPANY_CODE)//All Company Report
                    {
                        dtreport = reportDataHandler.populaterep0030All().Copy();
                        mrptsubheader = "All Companies";
                    }
                    else//Selected Company Report
                    {
                        dtreport = reportDataHandler.populaterep0030Company(CompanyID.Trim());
                        mrptsubheader = CompanyName;
                        //mrptLawerheader = "";
                    }
                }
                else if (((CompanyID != "") || (CompanyID != Constants.CON_UNIVERSAL_COMPANY_CODE)) && DepartmentID != "")//Department Wise Reports
                {
                    dtreport = reportDataHandler.populaterep0030Department(DepartmentID.Trim());
                    mrptsubheader = CompanyName;
                    mrptLawerheader = DepartmentName;
                }

                //ReportViewer1.Reset();
                //ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rptscr);
                //ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(subreportProcessing);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/EmployeeSkillsReport.rdlc");
                //ReportParameter[] param = new ReportParameter[3];
                //param[0] = new ReportParameter("headerpara", mrptheader);
                //param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                //param[2] = new ReportParameter("Lawersubheaderpara", mrptLawerheader);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("headerpara", mrptheader);
                paramdict.Add("subheaderpara", mrptsubheader);
                paramdict.Add("Lawersubheaderpara", mrptLawerheader);

                Session["rptDataSet"] = dtreport;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = mrptheader;

                Response.Redirect("~/Reports/ReportViewers/EmployeeSkillsReport.aspx");

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        void rep031()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable overTimeDataTable = new DataTable();
            string mrptsubheader = "";
            string sFromTodate = "";

            if (ddlBranch.Visible == true)
            {
                mdeptcode = "N/A";
            }

            try
            {

                if (mcompcode == Constants.CON_ETI_COMPANY_ID)
                {
                    string FromYearMonth = txtfromdate.Text.Trim();
                    string ToYearMonth = txttodate.Text.Trim();


                    DateTime FromDate = DateTime.ParseExact(FromYearMonth, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime ToDate = DateTime.ParseExact(ToYearMonth, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    FromYearMonth = FromDate.Year.ToString() + FromDate.Month.ToString().PadLeft(2, '0');
                    ToYearMonth = ToDate.Year.ToString() + ToDate.Month.ToString().PadLeft(2, '0');

                    if (chkemployee.Checked)
                    {
                        overTimeDataTable = reportDataHandler.populaterep0031ETI_OT_INDIVIDUAL(FromYearMonth, ToYearMonth, txtemployee.Text.Trim()).Copy();
                        if (overTimeDataTable.Rows.Count > 0)
                        {
                            mrptsubheader = dpCompID.SelectedItem.Text.Trim() + Environment.NewLine + Environment.NewLine + "Employee : " + overTimeDataTable.Rows[0]["NAME"].ToString().Trim();
                        }
                        else
                        {
                            mrptsubheader = dpCompID.SelectedItem.Text.Trim();
                        }
                    }
                    else
                    {
                        if (chkBranch.Checked)
                        {
                            if (dpCompID.SelectedIndex > 0)
                            {
                                string CompanyID = dpCompID.SelectedValue.ToString().Trim();
                                string BranchID = ddlBranch.SelectedValue.ToString().Trim();

                                overTimeDataTable = reportDataHandler.populaterep0031ETI_OT_BRANCH(FromYearMonth, ToYearMonth, CompanyID, BranchID).Copy();
                                mrptsubheader = dpCompID.SelectedItem.Text.Trim() + Environment.NewLine + Environment.NewLine + "Branch : " + ddlBranch.SelectedItem.Text.ToString();
                            }
                            else
                            {
                                string CompanyID = dpCompID.SelectedValue.ToString().Trim();

                                overTimeDataTable = reportDataHandler.populaterep0031ETI_OT_ALL(FromYearMonth, ToYearMonth, CompanyID).Copy();
                                mrptsubheader = dpCompID.SelectedItem.Text.Trim();
                            }
                        }
                        else
                        {
                            if (ddlDepartment.SelectedIndex > 0)
                            {
                                string CompanyID = dpCompID.SelectedValue.ToString().Trim();
                                string DepartmentID = ddlDepartment.SelectedValue.ToString().Trim();

                                overTimeDataTable = reportDataHandler.populaterep0031ETI_OT_DEPARTMENT(FromYearMonth, ToYearMonth, CompanyID, DepartmentID).Copy();
                                mrptsubheader = dpCompID.SelectedItem.Text.Trim() + Environment.NewLine + Environment.NewLine + "Department : " + ddlDepartment.SelectedItem.Text.ToString();
                            }
                            else
                            {
                                string CompanyID = dpCompID.SelectedValue.ToString().Trim();

                                overTimeDataTable = reportDataHandler.populaterep0031ETI_OT_ALL(FromYearMonth, ToYearMonth, CompanyID).Copy();
                                mrptsubheader = dpCompID.SelectedItem.Text.Trim();

                            }
                        }
                    }

                    //FromYearMonth = FromDate.Year.ToString() + FromDate.Month.ToString().PadLeft(2, '0');
                    //ToYearMonth = ToDate.Year.ToString() + ToDate.Month.ToString().PadLeft(2, '0');
                    if (FromYearMonth == ToYearMonth)
                    {
                        sFromTodate = "Month : " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(FromDate.Month) + " " + FromDate.Year.ToString();
                    }
                    else
                    {
                        sFromTodate = "From : " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(FromDate.Month) + " " + FromDate.Year.ToString() + "    To : " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(ToDate.Month) + " " + ToDate.Year.ToString();
                    }

                    Dictionary<string, string> paramdict = new Dictionary<string, string>();
                    paramdict.Add("rpCompany", mrptsubheader);
                    paramdict.Add("rpFrom", sFromTodate);

                    Session["rptDataSet"] = overTimeDataTable;
                    Session["rptParamDict"] = paramdict;
                    Session["rptDisplayName"] = "Overtime Payments and Special Payments";

                    Response.Redirect("~/Reports/ReportViewers/rptETIOTReport.aspx");

                }
                else
                {

                    if (ddlBranch.Visible == false)
                    {
                        overTimeDataTable = reportDataHandler.populaterep0031Overtime(mcompcode, mfromdate, mtodate, mempcode, mdeptcode).Copy();
                    }
                    else
                    {
                        overTimeDataTable = reportDataHandler.populaterep0031Overtime(mcompcode, mfromdate, mtodate, mempcode, ddlBranch.SelectedValue).Copy();

                    }

                    int count = overTimeDataTable.Rows.Count;
                    if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                    {
                        if (mempcode == "")
                        {
                            mrptsubheader = "All Companies";
                        }
                        else
                        {
                            mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                        }
                    }
                    else
                    {
                        string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);

                        if (mempcode == "" && mdeptcode == "")
                        {
                            mrptsubheader = companyname;
                        }
                        else if (mempcode != "" && mdeptcode == "")
                        {
                            mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                        }
                        else if (mdeptcode != "" && mdeptcode == Constants.DEPARTMENT_ID_STAMP)
                        {
                            mrptsubheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                        }
                        else if (mdeptcode != "" && mdeptcode != Constants.DEPARTMENT_ID_STAMP)
                        {
                            mrptsubheader = companyname + Environment.NewLine + Environment.NewLine + "Branch : " + ddlBranch.SelectedItem;
                        }
                        else
                        {
                            mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                        }
                    }

                    if (mfromdate == mtodate)
                    {
                        sFromTodate = "Date : " + mfromdate;
                    }
                    else
                    {
                        sFromTodate = "From : " + mfromdate + " To : " + mtodate;
                    }


                    Dictionary<string, string> paramdict = new Dictionary<string, string>();
                    paramdict.Add("rpCompany", mrptsubheader);
                    paramdict.Add("rpFrom", sFromTodate);

                    Session["rptDataSet"] = overTimeDataTable;
                    Session["rptParamDict"] = paramdict;
                    Session["rptDisplayName"] = "Over Time Report";

                    Response.Redirect("~/Reports/ReportViewers/rptOvertimeReport.aspx");


                }

                

               

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }

            finally
            {
                reportDataHandler = null;
                overTimeDataTable.Dispose();
                overTimeDataTable = null;
            }
        }

        void rep032()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dt = new DataTable();
            string headerpara = "Attendance Transfer Log";
            string subheaderpara = String.Empty;
            string Lawersubheaderpara = String.Empty;


            string fromDate = DateTime.ParseExact(txtfromdate.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string toDate = DateTime.ParseExact(txttodate.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            if (fromDate == toDate)
            {
                Lawersubheaderpara = "Date : " + fromDate.Trim();
            }
            else
            {
                Lawersubheaderpara = "From : " + fromDate.Trim() + "  To : " + toDate.Trim();
            }

            if (dpCompID.SelectedValue == Constants.CON_UNIVERSAL_COMPANY_CODE)
            {
                subheaderpara = "All Companies";
                dt = reportDataHandler.populaterep0032AllCompany(fromDate, toDate).Copy();
            }
            else
            {
                if ((chkBranch.Visible == true) && (chkBranch.Checked == true) && (ddlBranch.SelectedIndex > 0))
                {
                    subheaderpara = "Company : " + dpCompID.SelectedItem.Text.Trim() + "  |  Branch : " + ddlBranch.SelectedItem.Text.Trim();
                    dt = reportDataHandler.populaterep0032Branch(fromDate, toDate, ddlBranch.SelectedValue.Trim()).Copy();
                }
                else
                {
                    subheaderpara = dpCompID.SelectedItem.Text.Trim();
                    dt = reportDataHandler.populaterep0032Company(fromDate, toDate, dpCompID.SelectedValue.Trim()).Copy();
                }
            }




            Dictionary<string, string> paramdict = new Dictionary<string, string>();
            paramdict.Add("headerpara", headerpara);
            paramdict.Add("subheaderpara", subheaderpara);
            paramdict.Add("Lawersubheaderpara", Lawersubheaderpara);

            Session["rptDataSet"] = dt;
            Session["rptParamDict"] = paramdict;
            Session["rptDisplayName"] = "Attendance Transfer Log";

            Response.Redirect("~/Reports/ReportViewers/AttendanceTransferLogReport.aspx");
        }

        void rep033()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dt = new DataTable();
            string headerpara = "Raw Attendance Report";
            string subheaderpara = String.Empty;
            string Lawersubheaderpara = String.Empty;


            string fromDate = DateTime.ParseExact(txtfromdate.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string toDate = DateTime.ParseExact(txttodate.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            if (fromDate == toDate)
            {
                Lawersubheaderpara = "Date : " + fromDate.Trim();
            }
            else
            {
                Lawersubheaderpara = "From : " + fromDate.Trim() + "  To : " + toDate.Trim();
            }

            if ((chkemployee.Checked == true) && (txtemployee.Text != ""))
            {
                dt = reportDataHandler.populaterep0033Employee(fromDate, toDate, txtemployee.Text.Trim()).Copy();
                subheaderpara = dt.Rows[0]["EMP_NAME"].ToString();
            }
            else
            {
                if (dpCompID.SelectedValue == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    subheaderpara = "All Companies";
                    dt = reportDataHandler.populaterep0033AllCompany(fromDate, toDate).Copy();
                }
                else
                {
                    subheaderpara = dpCompID.SelectedItem.Text.Trim();
                    dt = reportDataHandler.populaterep0033Company(fromDate, toDate, dpCompID.SelectedValue.Trim()).Copy();

                    if ((chkBranch.Visible == true) && (chkBranch.Checked == true) && (ddlBranch.SelectedIndex > 0))
                    {
                        subheaderpara = "Company : " + dpCompID.SelectedItem.Text.Trim() + "  |  Branch : " + ddlBranch.SelectedItem.Text.Trim();
                        dt = reportDataHandler.populaterep0033Branch(fromDate, toDate, ddlBranch.SelectedValue.Trim()).Copy();
                    }
                    if ((ddlDepartment.Visible == true) && (ddlDepartment.SelectedIndex > 0))
                    {
                        subheaderpara = "Company : " + dpCompID.SelectedItem.Text.Trim() + "  |  Department : " + ddlDepartment.SelectedItem.Text.Trim();
                        dt = reportDataHandler.populaterep0033Department(fromDate, toDate, ddlDepartment.SelectedValue.Trim()).Copy();
                    }
                }
            }


            //SWAPPING ATTENDANCE DIRECTIONS FOR SSL & EAPH [REASON : UNABLE TO CONFIGURE FINGER PRINT DEVICE]  //2016-03-07  //CHATHURA
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string companyID = dt.Rows[i]["COMPANY_ID"].ToString();

                Boolean isValidCompany = false;

                if (companyID == Constants.CON_SSL_COMPANY_ID)
                {
                    if (Constants.CON_ATTENDANCE_DIRECTION_SWAPPED_SSL == true)
                    {
                        isValidCompany = true;
                    }
                }
                else if (companyID == Constants.CON_EAPH_COMPANY_ID)
                {
                    if (Constants.CON_ATTENDANCE_DIRECTION_SWAPPED_EAPH == true)
                    {
                        isValidCompany = true;
                    }
                }

                if (isValidCompany == true)
                {
                    string EmployeeID = dt.Rows[i]["EMPLOYEE_ID"].ToString();
                    string Date = dt.Rows[i]["ATT_DATE"].ToString();
                    string Time = dt.Rows[i]["ATT_TIME"].ToString();

                    if (SwapDirection(EmployeeID, Date, Time) == true)
                    {
                        string ditectionValue = dt.Rows[i]["DIRECTION"].ToString();
                        string ditectionText = dt.Rows[i]["DIRECTION_NAME"].ToString();

                        if (ditectionValue == Constants.CON_DIRECTION_OUT_VALUE)
                        {
                            dt.Rows[i]["DIRECTION"] = Constants.CON_DIRECTION_IN_VALUE;
                            dt.Rows[i]["DIRECTION_NAME"] = Constants.CON_DIRECTION_IN_TEXT;
                        }
                        else
                        {
                            dt.Rows[i]["DIRECTION"] = Constants.CON_DIRECTION_OUT_VALUE;
                            dt.Rows[i]["DIRECTION_NAME"] = Constants.CON_DIRECTION_OUT_TEXT;
                        }
                    }
                }
            }
            //



            //if (dpCompID.SelectedValue == Constants.CON_UNIVERSAL_COMPANY_CODE)
            //{
            //    subheaderpara = "All Companies";
            //    dt = reportDataHandler.populaterep0033AllCompany(fromDate, toDate).Copy();
            //}
            //else
            //{
            //    if ((chkBranch.Visible == true) && (chkBranch.Checked == true) && (ddlBranch.SelectedIndex > 0))
            //    {
            //        subheaderpara = "Company : " + dpCompID.SelectedItem.Text.Trim() + "  |  Branch : " + ddlBranch.SelectedItem.Text.Trim();
            //        dt = reportDataHandler.populaterep0033Branch(fromDate, toDate, ddlBranch.SelectedValue.Trim()).Copy();
            //    }
            //    else
            //    {
            //        if (ddlDepartment.SelectedIndex > 0)
            //        {
            //            subheaderpara = dpCompID.SelectedItem.Text.Trim();
            //            dt = reportDataHandler.populaterep0033Department(fromDate, toDate, ddlDepartment.SelectedValue.Trim()).Copy();
            //        }
            //        else
            //        {
            //            subheaderpara = dpCompID.SelectedItem.Text.Trim();
            //            dt = reportDataHandler.populaterep0033Company(fromDate, toDate, dpCompID.SelectedValue.Trim()).Copy();
            //        }
            //    }
            //}




            Dictionary<string, string> paramdict = new Dictionary<string, string>();
            paramdict.Add("headerpara", headerpara);
            paramdict.Add("subheaderpara", subheaderpara);
            paramdict.Add("Lawersubheaderpara", Lawersubheaderpara);

            Session["rptDataSet"] = dt;
            Session["rptParamDict"] = paramdict;
            Session["rptDisplayName"] = "Raw Attendance Report";

            Response.Redirect("~/Reports/ReportViewers/RawAttendanceReport.aspx");
        }

        void rep034()
        {

            string headerpara = "Trining Needs Report";
            string subheaderpara = "";
            string Lawersubheaderpara = "";
            
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtResult = new DataTable();
            DataTable dtTrainingNeeds = new DataTable();

            string company = String.Empty;
            string department = String.Empty;
            string fromDate = String.Empty;
            string toDate = String.Empty;
            try
            {
                if (!String.IsNullOrEmpty(dpCompID.SelectedValue.ToString()))
                {
                    string companyName = String.Empty;
                    if (dpCompID.SelectedValue.ToString() != Constants.CON_UNIVERSAL_COMPANY_CODE)
                    {
                        company = dpCompID.SelectedValue.ToString();
                        companyName = reportDataHandler.populateCompanyName(dpCompID.SelectedValue.ToString());
                    }
                    else
                    {
                        companyName = "All Companies ";
                    }
                    subheaderpara = " Company : '" + companyName + "' ";
                }
                else
                {
                    company = "";
                }

                if (!String.IsNullOrEmpty(ddlDepartment.SelectedValue.ToString()))
                {
                    dtResult = reportDataHandler.populateDepartmentName(ddlDepartment.SelectedValue.ToString());
                    department = dtResult.Rows[0][0].ToString();

                    subheaderpara += "| Department : '" + department + "' ";

                }
                else
                {
                    department = "";
                }

                if (!String.IsNullOrEmpty(txtfromdate.Text.ToString()))
                {
                    fromDate = txtfromdate.Text.ToString();
                    Lawersubheaderpara = " From : '" + fromDate + "' | ";
                }
                else
                {
                    fromDate = "";
                }
                if (!String.IsNullOrEmpty(txttodate.Text.ToString()))
                {
                    toDate = txttodate.Text.ToString();
                    Lawersubheaderpara += " To : '" + toDate + "'  ";
                }
                else
                {
                    toDate = "";
                }


                //dtTrainingNeeds = reportDataHandler.getTrainingRequest(company, department, fromDate,toDate);

                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("headerpara", headerpara);
                paramdict.Add("subheaderpara", subheaderpara);
                paramdict.Add("Lawersubheaderpara", Lawersubheaderpara);

                Session["rptDataSet"] = dtTrainingNeeds;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = " Training Needs Report";

                Response.Redirect("~/Reports/ReportViewers/rptvTrainingNeeds.aspx");

 
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                reportDataHandler = null;
                dtResult.Dispose();
                dtTrainingNeeds.Dispose();
            }
        }

        Boolean SwapDirection(string EmployeeID, string Date, string Time)
        {
            ReportDataHandler RDH = new ReportDataHandler();
            return !RDH.isConfigAttendance(EmployeeID, Date, Time);
        }

        void RadioButtonCheck()
        {
            if (chkBranch.Checked == false)
            {
                Label2.Visible = true;
                ddlDepartment.Visible = true;

                Label1.Visible = false;
                ddlBranch.Visible = false;
            }
            else
            {
                Label2.Visible = false;
                ddlDepartment.Visible = false;

                Label1.Visible = true;
                ddlBranch.Visible = true;
            }
        }

        protected void rdBtnDept_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonCheck();
        }

        protected void rdBtnBrnch_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonCheck();
        }

        protected void chkBranch_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonCheck();
        }
    }
}