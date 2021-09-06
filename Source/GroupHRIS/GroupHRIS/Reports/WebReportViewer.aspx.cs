using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using DataHandler.Reports;
using Common;
using DataHandler.MetaData;
using System.ComponentModel;
using System.IO;
using System.Configuration;
using DataHandler.Property;
using DataHandler.Employee;

namespace GroupHRIS.Reports
{
    public partial class WebReportViewer : System.Web.UI.Page
    {
        
        private static string mrepname;
        private static string mfromdate;
        private static string mtodate;
        private static string mcompcode = "";
        private static string mempcode = "";
        private static string employeeId = "";
        private static string mdeptcode = "";
        private static string statuscode = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                string KeyUSER_ID = (string)(Session["KeyUSER_ID"]);

                if(!string.IsNullOrEmpty(KeyUSER_ID))
                {
                    Dictionary<string,string> rDictionary = new Dictionary<string,string>();
                    if (Session["report"] != null)
                    {
                        rDictionary = (Session["report"] as Dictionary<string, string>);

                        mrepname = rDictionary["ID"];
                        mfromdate = rDictionary["fDate"];
                        mtodate = rDictionary["tDate"];
                        mcompcode = rDictionary["cCode"];
                        mempcode = rDictionary["eCode"];

                        employeeId = rDictionary["eCode"];

                        mdeptcode = rDictionary["dCode"];
                        statuscode = rDictionary["sCode"];

                    }
                    else
                    {

                        mrepname = Request.QueryString["repname"];
                        mfromdate = Request.QueryString["fromdate"];
                        mtodate = Request.QueryString["todate"];
                        mcompcode = Request.QueryString["compcode"];
                        mempcode = Request.QueryString["empcode"];

                        employeeId = Request.QueryString["empID"];

                        mdeptcode = Request.QueryString["mdeptcode"];
                        statuscode = Request.QueryString["statuscode"];
                    }

                }
                else
                {
                    Response.Redirect("~/Login/SessionExpior.aspx", false);
                }
                //mrepname = "RE008";
                //mfromdate = "2015-01-01";
                //mtodate = "2015-01-20";
                //mcompcode = "CP01";
                //mempcode = "";

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (mrepname == "RE001")
                {
                    run_rep001();
                }
                else if (mrepname == "RE002")
                {
                    run_rep002();
                }
                else if (mrepname == "RE003")
                {
                    run_rep003();
                }
                else if (mrepname == "RE004")
                {
                    run_rep004();
                }
                else if (mrepname == "RE005")
                {
                    run_rep005();
                }
                else if (mrepname == "RE007")
                {
                    run_rep007();
                }
                else if (mrepname == "RE008")
                {
                    run_rep008();
                }
                else if (mrepname == "RE009")
                {
                    run_rep009();
                }
                else if (mrepname == "RE010")
                {
                    run_rep010();
                }
                else if (mrepname == "RE012")
                {
                    run_rep012();
                }
                else if (mrepname == "RE013")
                {
                    run_rep013();
                }
                else if (mrepname == "RE014")
                {
                    run_rep014();
                }
                else if (mrepname == "RE015")
                {
                    run_rep015();
                }
                else if (mrepname == "RE016")
                {
                    run_rep016();
                }
                else if (mrepname == "RE017")
                {
                    run_rep017();
                }
                else if (mrepname == "RE018")
                {
                    run_rep018();
                }
                else if (mrepname == "RE020")
                {
                    run_rep020();
                }
                else if (mrepname == "RE021")
                {
                    run_rep021();
                }
                else if (mrepname == "RE022")
                {
                    run_rep022();
                }
                else if (mrepname == "RE023")
                {
                    run_rep023();
                }
                else if (mrepname == "RE024")
                {
                    run_rep024();
                }
                else if (mrepname == "RE025")
                {
                    run_rep025();
                }
                else if (mrepname == "RE026")
                {
                    run_rep026();
                }
                else if (mrepname == "RE027")
                {
                    run_rep027();
                }
                else if (mrepname == "RE028")
                {
                    run_rep028();
                }
                else if (mrepname == "RE029")
                {
                    run_rep029();
                }
                else if (mrepname == "RE030")
                {
                    run_rep030();
                }
                else if (mrepname == "RE031")
                {
                    run_rep031();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Session["report"] = null;

            Response.Redirect("ReportGenerator.aspx", false);
        }

        private void run_rep001()
        {
            Object thisLock = new Object();
            lock (thisLock)
            {

                ReportDataHandler reportDataHandler = new ReportDataHandler();
                DataTable dtreport = new DataTable();
                dtreport = reportDataHandler.populaterep0001(mfromdate, mtodate, mcompcode, mempcode, mdeptcode).Copy();

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
                    else if (mdeptcode != "")
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


                    Dictionary<string, string> paramdict = new Dictionary<string, string>();
                    paramdict.Add("headerpara", mrptheader);
                    paramdict.Add("subheaderpara", mrptsubheader);
                    paramdict.Add("Lawersubheaderpara", mrptLawerheader);

                    Session["rptDataSet"] = dtreport;
                    Session["rptParamDict"] = paramdict;
                    Response.Redirect("ReportViewers/ReportSummary.aspx");


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

        }

        private void run_rep002()
        {
            Object thisLock = new Object();
            lock (thisLock)
            {


                CompanyDataHandler companyDataHandler = new CompanyDataHandler();
                ReportDataHandler reportDataHandler = new ReportDataHandler();
                DataTable dtreport = new DataTable();

                try
                {
                    dtreport = reportDataHandler.populaterep0002(mfromdate, mtodate, mcompcode, mempcode, mdeptcode).Copy();

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

                    Dictionary<string, string> paramdict = new Dictionary<string, string>();
                    paramdict.Add("headerpara", mrptheader);
                    paramdict.Add("subheaderpara", mrptsubheader);
                    paramdict.Add("parmFromDate", sFromTodate);

                    Session["rptDataSet"] = dtreport;
                    Session["rptParamDict"] = paramdict;

                    Response.Redirect("ReportViewers/LateCommers.aspx");


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
        }

        private void run_rep003()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable dtreport = new DataTable();

            try
            {
                string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);

                string mrptheader = "Employee Details Report";
                string mrptsubheader = "";
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
                            dtreport = reportDataHandler.populaterep0003(mcompcode, mdeptcode).Copy();
                            ReportDataHandler oReportDataHandler = new ReportDataHandler();
                            DataTable dtbl = new DataTable();

                            dtbl = oReportDataHandler.populateDepartmentName(mdeptcode).Copy();
                            mrptsubheader =  companyname+"   |   Department : " + dtbl.Rows[0]["DEPT_NAME"].ToString();
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
                            mrptLawerheader = "Employee Status : " + (Session["EmployeeStaus"] as string).Trim();
                        }
                        else
                        {
                            dtreport = reportDataHandler.opopulaterep0003(mcompcode, mdeptcode, statuscode).Copy();
                            ReportDataHandler oReportDataHandler = new ReportDataHandler();
                            DataTable dtbl = new DataTable();

                            dtbl = oReportDataHandler.populateDepartmentName(mdeptcode).Copy();

                            mrptsubheader =  companyname+"   |   Department : " + dtbl.Rows[0]["DEPT_NAME"].ToString();
                            mrptLawerheader = "Employee Status : " + (Session["EmployeeStaus"] as string).Trim();
                        }
                    }
                    else
                    {
                        dtreport = reportDataHandler.opopulaterep0003IND(mempcode, statuscode).Copy();
                        ReportDataHandler oReportDataHandler = new ReportDataHandler();
                        DataTable dtbl = new DataTable();

                        dtbl = oReportDataHandler.populateEmployeeName(mempcode).Copy();

                        mrptsubheader = "Employee Name : " + dtbl.Rows[0]["FULL_NAME"].ToString();
                        mrptLawerheader = "Employee Status : " + (Session["EmployeeStaus"] as string).Trim();
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

                ReportViewer1.Reset();
                ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rptscr);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/Employee.rdlc");
                ReportParameter[] param = new ReportParameter[3];
                param[0] = new ReportParameter("headerpara", mrptheader);
                param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                param[2] = new ReportParameter("Lawersubheaderpara", mrptLawerheader);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep004()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtreport = new DataTable();

            try
            {
                dtreport = reportDataHandler.populaterep0004(mfromdate, mtodate, mcompcode).Copy();

                string mrptsubheader = "";
                string mrptheader = "Continuos Absent Report";
                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    mrptsubheader = "All Companies";
                }
                else
                {
                    mrptsubheader = "Selected Company";
                }
                string mrptLawerheader = "From : " + mfromdate.ToString() + " To : " + mtodate.ToString();

                ReportViewer1.Reset();
                ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rptscr);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/ContinuosAbsent.rdlc");
                ReportParameter[] param = new ReportParameter[3];
                param[0] = new ReportParameter("headerpara", mrptheader);
                param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                param[2] = new ReportParameter("Lawersubheaderpara", mrptLawerheader);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep005()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtreport = new DataTable();

            try
            {
                dtreport = reportDataHandler.populaterep0005(mfromdate, mtodate, mcompcode).Copy();

                string mrptsubheader = "";
                string mrptheader = "Cadre Report";
                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    mrptsubheader = "ALL Companies";
                }
                else
                {
                    mrptsubheader = "Selected Company";
                }

                string mrptLawerheader = " As at " + mtodate.ToString();

                ReportViewer1.Reset();
                ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rptscr);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/Cadre.rdlc");
                ReportParameter[] param = new ReportParameter[3];
                param[0] = new ReportParameter("headerpara", mrptheader);
                param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                param[2] = new ReportParameter("Lawersubheaderpara", mrptLawerheader);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep007()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtreport = new DataTable();
            DataTable dtreportSub = new DataTable();

            try
            {
                dtreport = reportDataHandler.populaterep0007(mcompcode, mfromdate, mtodate).Copy();
                dtreportSub = reportDataHandler.populaterep0007_Sub(mcompcode, mfromdate, mtodate).Copy();

                string mrptsubheader = "";
                string mrptheader = "New Recruitment Report";
                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    mrptsubheader = "ALL Companies";
                }
                else
                {
                    mrptsubheader = "Selected Company";
                }
                string mrptLawerheader = "From : " + mfromdate.ToString() + " To : " + mtodate.ToString();

                ReportViewer1.Reset();
                ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
                ReportDataSource rptscr_Sub = new ReportDataSource("DataSet2", dtreportSub);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rptscr);
                ReportViewer1.LocalReport.DataSources.Add(rptscr_Sub);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/newRecruitment.rdlc");
                ReportParameter[] param = new ReportParameter[3];
                param[0] = new ReportParameter("headerpara", mrptheader);
                param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                param[2] = new ReportParameter("Lawersubheaderpara", mrptLawerheader);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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
                dtreportSub.Dispose();
                dtreportSub = null;
            }

        }

        private void run_rep008()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtreport = new DataTable();
            dtreport = reportDataHandler.populaterep0008(mfromdate, mtodate, mcompcode, mempcode).Copy();

            try
            {
                string mrptheader = "OT Summary Report";
                string mrptsubheader = "From " + mfromdate + " To " + mtodate;
                if (mempcode == "")
                {
                    mempcode = "ALL";
                }
                string mrptLawerheader = "Employee : " + mempcode;

                ReportViewer1.Reset();
                ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rptscr);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/OTSummary.rdlc");
                ReportParameter[] param = new ReportParameter[3];
                param[0] = new ReportParameter("headerpara", mrptheader);
                param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                param[2] = new ReportParameter("Lawersubheaderpara", mrptLawerheader);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep009()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtreport = new DataTable();
            DataTable dtreportSub = new DataTable();

            try
            {
                dtreport = reportDataHandler.populaterep0009(mcompcode, mfromdate, mtodate).Copy();
                dtreportSub = reportDataHandler.populaterep0009_Sub(mcompcode, mfromdate, mtodate).Copy();

                string mrptsubheader = "";
                string mrptheader = "Resignation Report";
                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    mrptsubheader = "ALL Companies";
                }
                else
                {
                    mrptsubheader = "Selected Company";
                }
                string mrptLawerheader = "From " + mfromdate.ToString() + " To " + mtodate.ToString();

                ReportViewer1.Reset();
                ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
                ReportDataSource rptscr_Sub = new ReportDataSource("DataSet2", dtreportSub);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rptscr);
                ReportViewer1.LocalReport.DataSources.Add(rptscr_Sub);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/Resignation.rdlc");
                ReportParameter[] param = new ReportParameter[3];
                param[0] = new ReportParameter("headerpara", mrptheader);
                param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                param[2] = new ReportParameter("Lawersubheaderpara", mrptLawerheader);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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
                dtreportSub.Dispose();
                dtreportSub = null;
            }

        }

        private void run_rep010()
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
                string mrptheader = "Staff Strenth Report";
                if (mcompcode == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    mrptsubheader = "ALL Companies";
                }
                else
                {
                    mrptsubheader = "Selected Company";
                }

                string mrptLawerheader = "From " + mfromdate.ToString() + " To " + mtodate.ToString();

                ReportViewer1.Reset();
                ReportDataSource rptscr_dtEmployeetype = new ReportDataSource("DataSet1", dtEmployeetype);
                ReportDataSource rptscr_dtGenderAnalysis = new ReportDataSource("DataSet2", dtGenderAnalysis);
                ReportDataSource rptscr_dtDesignationAnalysis = new ReportDataSource("DataSet3", dtDesignationAnalysis);
                ReportDataSource rptscr_dtStaffStrength = new ReportDataSource("DataSet4", dtStaffStrength);
                ReportDataSource rptscr_dtStaffTenure = new ReportDataSource("DataSet5", dtStaffTenure);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rptscr_dtDesignationAnalysis);
                ReportViewer1.LocalReport.DataSources.Add(rptscr_dtEmployeetype);
                ReportViewer1.LocalReport.DataSources.Add(rptscr_dtGenderAnalysis);
                ReportViewer1.LocalReport.DataSources.Add(rptscr_dtStaffStrength);
                ReportViewer1.LocalReport.DataSources.Add(rptscr_dtStaffTenure);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/StaffStrenth.rdlc");
                ReportParameter[] param = new ReportParameter[3];
                param[0] = new ReportParameter("headerpara", mrptheader);
                param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                param[2] = new ReportParameter("Lawersubheaderpara", mrptLawerheader);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep012()
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
                ReportViewer1.Reset();
                ReportDataSource rdsUOAbsent = new ReportDataSource("DataSet1", dtUOAbsent);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsUOAbsent);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptUOAbsentReport.rdlc");
                ReportParameter[] param = new ReportParameter[2];
                param[0] = new ReportParameter("rpCompany", mrptsubheader);
                param[1] = new ReportParameter("rpFrom", sFromTodate);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep013()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable odtOt = new DataTable();

            string mrptsubheader = "";

            try
            {

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
                        mrptsubheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                    }
                    else
                    {
                        mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }

                ReportViewer1.Reset();
                ReportDataSource rdsOT = new ReportDataSource("DataSet1", odtOt);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsOT);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptOTReport.rdlc");
                ReportParameter[] param = new ReportParameter[2];
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

                param[0] = new ReportParameter("paramCompany", mrptsubheader);               
                param[1] = new ReportParameter("paramMonth", sDate);
                //param[2] = new ReportParameter("rpTo", mtodate);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep014()
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

                ReportViewer1.Reset();
                ReportDataSource rdsProperty = new ReportDataSource("DataSet1", companyProperty);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/CompanyProperties.rdlc");
                ReportParameter[] param = new ReportParameter[1];

                param[0] = new ReportParameter("paraCompany", companyname);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep015()
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

                ReportViewer1.Reset();
                ReportDataSource rdsProperty = new ReportDataSource("DataSet1", companyProperty);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/AvailableProperties.rdlc");
                ReportParameter[] param = new ReportParameter[1];

                param[0] = new ReportParameter("companyName", companyname);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep016()
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

                ReportViewer1.Reset();
                ReportDataSource rdsOT = new ReportDataSource("DataSet1", companyProperty);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsOT);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/EmployeeProperties.rdlc");
                ReportParameter[] param = new ReportParameter[1];

                param[0] = new ReportParameter("paraEmployeeName", employeeName);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep017()
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

                    //companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);
                    //if (mempcode != "" && mdeptcode == "")
                    //{
                    //    mrptsubheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    //}
                    //else if (mdeptcode != "")
                    //{
                    //    mrptsubheader = reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString() + " Department";
                    //}

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
                string sFromTodate =""; 
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

                ReportViewer1.Reset();
                ReportDataSource rdsProperty = new ReportDataSource("DataSet1", employeeResignation);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/EmployeeResignation.rdlc");
                ReportParameter[] param = new ReportParameter[2];

                //param[0] = new ReportParameter("paramCompanyName", companyname);
                param[0] = new ReportParameter("paramSubHeader", mrptsubheader);
                param[1] = new ReportParameter("paramFromtoDate", sFromTodate);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep018()
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


                ReportViewer1.Reset();
                ReportDataSource rdsProperty = new ReportDataSource("DataSet1", employeeResignation);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/EmployeeNewRecrutments.rdlc");
                ReportParameter[] param = new ReportParameter[2];

                string sDate = "";
                if (mfromdate == mtodate)
                {
                    sDate = "Date : " + mfromdate;
                }
                else
                {
                    sDate = "From : " + mfromdate + " To : " + mtodate;
                }
                param[0] = new ReportParameter("paramCompany", mrptsubheader);
                param[1] = new ReportParameter("paramMonth", sDate);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep020()
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

                ReportViewer1.Reset();
                ReportDataSource rdsProperty = new ReportDataSource("DataSet1", employeeDetail);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptLetterOfEmployeements.rdlc");
                ReportParameter[] param = new ReportParameter[1];

                param[0] = new ReportParameter("parmCurrentDate", currentDate);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep021()
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
                employeeRosterDetail = reportDataHandler.populaterep0021_MonthlyRoster(mcompcode,mdeptcode,mempcode,mfromdate, mtodate).Copy();
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
                        mrptsubheader =  companyByEmp + Environment.NewLine + Environment.NewLine + reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }
                else
                {
                    //dept id
                    mrptsubheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                }
                
                ReportViewer1.Reset();
                ReportDataSource rdsProperty = new ReportDataSource("DataSet2", employeeRosterDetail);
                ReportDataSource rdsProperty2 = new ReportDataSource("DataSet1", dateTable);

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                ReportViewer1.LocalReport.DataSources.Add(rdsProperty2);

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptRoster.rdlc");
                if (mfromdate == mtodate)
                {
                    sDate = "Date : " + mfromdate;
                }
                else
                {
                    sDate = "From : " + mfromdate + " To : " + mtodate;
                }

                ReportParameter[] param = new ReportParameter[2];
                param[0] = new ReportParameter("parmCompanyName", mrptsubheader);
                param[1] = new ReportParameter("parmtodate", sDate);

                ReportViewer1.LocalReport.SetParameters(param);

                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep022()
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
                        if (mdeptcode == "")
                        {
                            employeeMissingDetail = reportDataHandler.populaterep0022_MissingEmployeeForCompany(mcompcode, mfromdate, mtodate).Copy();
                        }
                        else
                        {
                            employeeMissingDetail = reportDataHandler.populaterep0022_MissingEmployeeForDepartment(mdeptcode, mfromdate, mtodate).Copy();
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

                ReportViewer1.Reset();
                ReportDataSource rdsProperty = new ReportDataSource("DataSet1", employeeMissingDetail);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptMissingEmployeements.rdlc");
                ReportParameter[] param = new ReportParameter[3];

                string mrptsubheader = "";
                if (mfromdate == mtodate)
                {
                    mrptsubheader = "Date : " + mfromdate;
                }
                else
                {
                    mrptsubheader = "From : " + mfromdate + " To : " + mtodate;
                }

                param[0] = new ReportParameter("paramCurrentDate", currentDate);
                param[1] = new ReportParameter("pramCompany", mrptNameheader);
                param[2] = new ReportParameter("parmFromDate", mrptsubheader);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep023()
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
                ReportViewer1.Reset();
                ReportDataSource rdsProperty = new ReportDataSource("DataSet1", employeeEarlyOff);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptEarlyOffReport.rdlc");
                ReportParameter[] param = new ReportParameter[2];

                param[0] = new ReportParameter("paramCompany", mrptNameheader);
                param[1] = new ReportParameter("parmFromDate", sFromTodate);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep024()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable nopayDataTable = new DataTable();
            string mrptNameheader = "";

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
                        if (mdeptcode == "")
                        {
                            nopayDataTable = reportDataHandler.populaterep0024_NopayForCompany(mcompcode, mfromdate, mtodate).Copy();
                        }
                        else
                        {
                            nopayDataTable = reportDataHandler.populaterep0024_NopayForDepartment(mdeptcode, mfromdate, mtodate).Copy();
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
                    else if (mdeptcode != "")
                    {
                        mrptNameheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
                    }
                    else
                    {
                        mrptNameheader = reportDataHandler.populateEmployeeName(mempcode).Rows[0]["FULL_NAME"].ToString();
                    }
                }

                ReportViewer1.Reset();
                ReportDataSource rdsProperty = new ReportDataSource("DataSet1", nopayDataTable);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptNopay.rdlc");

                ReportParameter[] param = new ReportParameter[2];
                param[0] = new ReportParameter("paramCompany", mrptNameheader);
                param[1] = new ReportParameter("prmFromDate", sFromTodate);
                ReportViewer1.LocalReport.SetParameters(param);

                ReportViewer1.LocalReport.Refresh();

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

        private void run_rep025()
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

                ReportViewer1.Reset();
                ReportDataSource rdsAttreg = new ReportDataSource("DataSet1", attRegDataTable);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsAttreg);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptAttendanceRegistration.rdlc");

                ReportParameter[] param = new ReportParameter[2];
                param[0] = new ReportParameter("paramDate", currentDate);
                param[1] = new ReportParameter("paramCompany", companyName);
                //param[2] = new ReportParameter("paramDepartment", deptName);


                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();

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

        private void run_rep026()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable continuousDataTable = new DataTable();
            string mrptNameheader = "";
            string currentDate = DateTime.Today.ToString("dd-MM-yyyy");

            try
            {
                continuousDataTable = reportDataHandler.populaterep0026_ContinuousAbsent(mcompcode,mdeptcode, mfromdate, mtodate,mempcode).Copy();
                
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


                    if (startDate == DateTime.MinValue )
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
                            DateTime fromDate = startDate.AddDays(-(count+hCount));
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

                ReportViewer1.Reset();
                ReportDataSource rdsProperty = new ReportDataSource("DataSet1", continuousDataTable);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsProperty);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptContinuousAbsent.rdlc");

                ReportParameter[] param = new ReportParameter[2];
                param[0] = new ReportParameter("paramDate", sFromTodate);
                param[1] = new ReportParameter("paramCompany", mrptNameheader);
                ReportViewer1.LocalReport.SetParameters(param);

                ReportViewer1.LocalReport.Refresh();

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        private void run_rep027()
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
                    sFromTodate ="Date : " + mfromdate;
                }
                else
                {
                    sFromTodate = "From : " + mfromdate + " To : " + mtodate;
                }

                leaveDetail = reportDataHandler.populaterep0027_LeaveDetailofAnEmployee(mcompcode,mdeptcode,employeeId, mfromdate, mtodate).Copy();

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


                ReportViewer1.Reset();
                ReportDataSource rdsLeaves = new ReportDataSource("DataSet1", leaveDetail);
                
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsLeaves);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptEmployeeLeaveDetails.rdlc");

                ReportParameter[] param = new ReportParameter[2];
                param[0] = new ReportParameter("mrptNameheader", mrptNameheader);
                //param[1] = new ReportParameter("pDepartment", "Department : " + deptName.Trim());
                //param[2] = new ReportParameter("pBranch", "Branch : " + branchName.Trim());
                //param[3] = new ReportParameter("pEmployee", employeeName);
                param[1] = new ReportParameter("pDateRange", sFromTodate);

                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();

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

        private void run_rep028()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            //CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable dtreport = new DataTable();

            try
            {
                //string companyname = companyDataHandler.getCompanyNameByCompanyId(mcompcode);

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
                    //mrptsubheader = "From : " + FromDate.Trim() + "  To : " + ToDate.Trim();
                    DataTable dtbl = new DataTable();
                    dtbl = reportDataHandler.populateEmployeeName(mempcode).Copy();
                    mrptLawerheader = "Employee Name : " + dtbl.Rows[0]["FULL_NAME"].ToString();
                }
                else if (CompanyID != "" && DepartmentID == "")//Company Wise Reports
                {
                    if (CompanyID == Constants.CON_UNIVERSAL_COMPANY_CODE)//All Company Report
                    {
                        dtreport = reportDataHandler.populaterep0028AllCompany(FromDate.Trim(), ToDate.Trim());
                        //mrptsubheader = "From : " + FromDate.Trim() + "  To : " + ToDate.Trim();
                    }
                    else//Selected Company Report
                    {
                        dtreport = reportDataHandler.populaterep0028INDCompany(CompanyID.Trim(), FromDate.Trim(), ToDate.Trim());
                        //mrptsubheader = "From : " + FromDate.Trim() + "  To : " + ToDate.Trim();
                        mrptLawerheader = CompanyName.Trim();
                    }
                }
                else if (((CompanyID != "") || (CompanyID != Constants.CON_UNIVERSAL_COMPANY_CODE)) && DepartmentID != "")//Department Wise Reports
                {
                    dtreport = reportDataHandler.populaterep0028INDDepartment(DepartmentID.Trim(), FromDate.Trim(), ToDate.Trim());
                    //mrptsubheader = "From : " + FromDate.Trim() + "  To : " + ToDate.Trim();
                    mrptLawerheader =  CompanyName.Trim() + Environment.NewLine + Environment.NewLine + "  Department : " + DepartmentName.Trim();
                }

                ReportViewer1.Reset();
                ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rptscr);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/OffDayWorkReport.rdlc");
                ReportParameter[] param = new ReportParameter[3];
                param[0] = new ReportParameter("headerpara", mrptheader);
                param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                param[2] = new ReportParameter("Lawersubheaderpara", mrptLawerheader);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep029()
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
                        mrptLawerheader =  CompanyName.Trim();
                    }
                }

                ReportViewer1.Reset();
                ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rptscr);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptStaffStrength.rdlc");
                ReportParameter[] param = new ReportParameter[3];
                param[0] = new ReportParameter("headerpara", mrptheader);
                param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                param[2] = new ReportParameter("Lawersubheaderpara", mrptLawerheader);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
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

        private void run_rep030()
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

                ReportViewer1.Reset();
                ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rptscr);
                ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(subreportProcessing);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/EmployeeSkillsReport.rdlc");
                ReportParameter[] param = new ReportParameter[3];
                param[0] = new ReportParameter("headerpara", mrptheader);
                param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                param[2] = new ReportParameter("Lawersubheaderpara", mrptLawerheader);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        private void run_rep031()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable overTimeDataTable = new DataTable();
            string mrptsubheader = "";
            string sFromTodate = "";

            try
            {
                
                overTimeDataTable = reportDataHandler.populaterep0031Overtime(mcompcode, mfromdate, mtodate, mempcode, mdeptcode).Copy();

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
                    else if (mdeptcode != "")
                    {
                        mrptsubheader = companyname + Environment.NewLine + Environment.NewLine + "Department : " + reportDataHandler.populateDepartmentName(mdeptcode).Rows[0]["DEPT_NAME"].ToString();
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

                ReportViewer1.Reset();
                ReportDataSource rdsUOAbsent = new ReportDataSource("DataSet1", overTimeDataTable);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdsUOAbsent);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/rptOvertimeReport.rdlc");
                ReportParameter[] param = new ReportParameter[2];
                param[0] = new ReportParameter("rpCompany", mrptsubheader);
                param[1] = new ReportParameter("rpFrom", sFromTodate);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();


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

        private DataTable getPreviousExperience(string EmpID)
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            return reportDataHandler.populaterep0030PreviousExperience(EmpID).Copy();
        }

        private DataTable getHighEducation(string EmpID)
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            return reportDataHandler.populaterep0030HigherEducation(EmpID).Copy();
        }

        private DataTable getSecondaryEducation(string EmpID)
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            return reportDataHandler.populaterep0030SecondaryEducation(EmpID).Copy();
        }

        public void subreportProcessing(object sender, SubreportProcessingEventArgs e)
       {
           if (e.ReportPath == "EmployeePreviousExperience")
           {
               ReportDataHandler reportDataHandler = new ReportDataHandler();
               string EMPLOYEE_ID = e.Parameters["EMPLOYEE_ID"].Values[0].ToString();
               DataTable dtreport = getPreviousExperience(EMPLOYEE_ID.Trim()).Copy();
               ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
               e.DataSources.Add(rptscr);
           }
           else if (e.ReportPath == "EmployeeHigherEducation")
           {
               ReportDataHandler reportDataHandler = new ReportDataHandler();
               string EMPLOYEE_ID = e.Parameters["EMPLOYEE_ID"].Values[0].ToString();
               DataTable dtreport = getHighEducation(EMPLOYEE_ID.Trim()).Copy();
               ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
               e.DataSources.Add(rptscr);
           }
           else if (e.ReportPath == "EmployeeSecondaryEducation")
           {
               ReportDataHandler reportDataHandler = new ReportDataHandler();
               string EMPLOYEE_ID = e.Parameters["EMPLOYEE_ID"].Values[0].ToString();
               DataTable dtreport = getSecondaryEducation(EMPLOYEE_ID.Trim()).Copy();
               ReportDataSource rptscr = new ReportDataSource("DataSet1", dtreport);
               e.DataSources.Add(rptscr);
           }
        }

    }
}