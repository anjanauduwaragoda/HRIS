using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using DataHandler.Reports;

namespace GroupHRIS.Reports.ReportViewers
{
    public partial class EmployeeSkillsReport : System.Web.UI.Page
    {
        void loadReport(DataTable ReportDataSet, Dictionary<string, string> ReportParameters, string reportDisplayName)
        {
            int paramCount = ReportParameters.Count;
            ReportParameter[] param = new ReportParameter[paramCount];
            for (int i = 0; i < paramCount; i++)
            {
                string key = ReportParameters.Keys.ElementAt(i);
                param[i] = new ReportParameter(key, ReportParameters[key]);
            }

            ReportDataSource rptscr = new ReportDataSource("DataSet1", ReportDataSet);
            rptViewer.LocalReport.DataSources.Clear();
            rptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(subreportProcessing);
            rptViewer.LocalReport.ReportPath = "Reports\\Reports\\EmployeeSkillsReport.rdlc";
            rptViewer.LocalReport.SetParameters(param);
            rptViewer.LocalReport.DataSources.Add(rptscr);
            rptViewer.LocalReport.Refresh();
            rptViewer.LocalReport.DisplayName = reportDisplayName;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    DataTable rptData = (DataTable)Session["rptDataSet"];
                    Dictionary<string, string> rptParam = (Dictionary<string, string>)Session["rptParamDict"];
                    string reportDisplayName = (string)Session["rptDisplayName"];
                    loadReport(rptData, rptParam, reportDisplayName);
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }
    }
}