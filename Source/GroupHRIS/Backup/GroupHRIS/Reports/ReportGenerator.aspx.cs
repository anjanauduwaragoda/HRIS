using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.Reports;
using System.Data;
using DataHandler.MetaData;
using DataHandler.Attendance;
using System.Globalization;
using GroupHRIS.Utility;

namespace GroupHRIS.Reports
{
    public partial class ReportGenerator : System.Web.UI.Page
    {
        public static string mStrRepName = "";

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
                    if (!string.IsNullOrEmpty(Request.QueryString["mRepName"]))
                    {
                        hfRepID.Value = String.Empty;
                        hfRepID.Value = Request.QueryString["mRepName"];
                        Session["SessionRep_Code"] = Request.QueryString["mRepName"];
                        mStrRepName = (string)Session["SessionRep_Code"];
                    }
                    lblrepname.Text = mStrRepName;
                    getCompID(KeyCOMP_ID);
                    getCultryDate();

                    if (mStrRepName == "RE003")
                    {
                        populateEmployeeStatus();
                    }
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

        protected void Button1_Click1(object sender, EventArgs e)
        {
            Session["report"] = null;
            
            Dictionary<string, string> rDictionary = new Dictionary<string, string>();
            
            try
            {
                DateTime fromdate = DateTime.ParseExact(txtfromdate.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime todate = DateTime.ParseExact(txttodate.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
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

                if (chkemployee.Checked == true)
                {
                    empcode = txtemployee.Text.ToString();
                }

                if (hfRepID.Value == "RE001")//hfRepID.Value
                    //if (mStrRepName == "RE001")
                {
                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + ddlDepartment.SelectedValue, false);
                    
                    rDictionary.Add("ID","RE001");
                    rDictionary.Add("fDate", fromdate.ToString("yyyy-MM-dd"));
                    rDictionary.Add("tDate", todate.ToString("yyyy-MM-dd"));
                    rDictionary.Add("cCode", mCompCode);
                    rDictionary.Add("eCode", empcode);
                    rDictionary.Add("dCode", ddlDepartment.SelectedValue);
                    rDictionary.Add("sCode", "");
                    Session["report"] = rDictionary;
                }
                else if (hfRepID.Value == "RE002")
                {
                    if (fromdate < todate || fromdate == todate)
                    {



                        Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + hfRepID.Value.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
                    }
                    else
                    {
                        Utility.Errorhandler.GetError("2", "From date is grater than To date", lblerror);
                        return;
                    }

                }
                else if (mStrRepName == "RE003")
                {
                    if (chkemployee.Checked == false)
                    {
                        txtemployee.Text = String.Empty;
                    }
                    Session["EmployeeStaus"] = ddlEmpStatus.SelectedItem.Text.Trim();
                    Response.Redirect("WebReportViewer.aspx?compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&mdeptcode=" + ddlDepartment.SelectedValue + "&empcode=" + txtemployee.Text.Trim()+"&statuscode="+ddlEmpStatus.SelectedValue, false);
                }
                else if (mStrRepName == "RE004")
                {
                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                }
                else if (mStrRepName == "RE005")
                {
                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                }
                else if (mStrRepName == "RE007")
                {
                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                }
                else if (mStrRepName == "RE008")
                {
                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                }
                else if (mStrRepName == "RE009")
                {
                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                }
                else if (mStrRepName == "RE010")
                {
                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                }
                else if (mStrRepName == "RE012")
                {
                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
                }
                else if (mStrRepName == "RE013")
                {
                    if (fromdate < todate || fromdate == todate)
                    {
                        Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
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
                        Response.Redirect("WebReportViewer.aspx?compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                    }
                    else
                    {
                        Utility.Errorhandler.GetError("2", "You can not take this report for Employee and Department", lblerror);
                        return;
                    }
                }
                else if (mStrRepName == "RE015")
                {
                    Response.Redirect("WebReportViewer.aspx?compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);
                }
                else if (mStrRepName == "RE016")
                {
                    Response.Redirect("WebReportViewer.aspx?compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode, false);
                }
                else if (mStrRepName == "RE017")
                {
                    if (fromdate < todate || fromdate == todate)
                    {
                        Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
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
                        Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&mdeptcode=" + mdepCode + "&empcode=" + empcode, false);
                    }
                }
                else if (mStrRepName == "RE020")
                {
                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode, false);
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
                        Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
                    }
                    
                }
                else if (mStrRepName == "RE022")
                {

                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
                    
                }
                else if (mStrRepName == "RE023")
                {
                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
                   
                }
                else if (mStrRepName == "RE024")
                {
                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
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
                        Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode, false);
                    }
                }
                else if (mStrRepName == "RE026")
                {
                    
                    if ((todate - fromdate).TotalDays > 1)
                    {
                        Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
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

                    //if ((txtemployee.Text.Trim() == "") || (chkemployee.Checked==false))
                    //{
                    //    Utility.Errorhandler.GetError("2", "You can take this report only for an individual Employee", lblerror);
                    //    return;
                    //}                    
                    //else
                    //{
                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empID=" + txtemployee.Text.Trim() + "&mdeptcode=" + mdepCode + "&empcode=" + empcode, false);
                    //}
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

                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
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

                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);

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
                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim(), false);

                }
                else if (mStrRepName == "RE031")
                {

                    Response.Redirect("WebReportViewer.aspx?fromdate=" + fromdate.ToString("yyyy-MM-dd") + "&todate=" + todate.ToString("yyyy-MM-dd") + "&compcode=" + mCompCode + "&repname=" + mStrRepName.ToString().Trim() + "&empcode=" + empcode + "&mdeptcode=" + mdepCode, false);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void dpCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateDepartments(dpCompID.SelectedValue);
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
    }
}