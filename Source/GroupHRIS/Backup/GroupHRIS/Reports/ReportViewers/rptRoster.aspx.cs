using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace GroupHRIS.Reports.ReportViewers
{
    public partial class rptRoster : System.Web.UI.Page
    {
        void loadReport(DataTable employeeRosterDetail, DataTable dateTable, Dictionary<string, string> ReportParameters, string reportDisplayName)
        {
            int paramCount = ReportParameters.Count;
            ReportParameter[] param = new ReportParameter[paramCount];
            for (int i = 0; i < paramCount; i++)
            {
                string key = ReportParameters.Keys.ElementAt(i);
                param[i] = new ReportParameter(key, ReportParameters[key]);
            }

            ReportDataSource rptscr1 = new ReportDataSource("DataSet2", employeeRosterDetail);
            ReportDataSource rptscr = new ReportDataSource("DataSet1", dateTable);
            rptViewer.LocalReport.DataSources.Clear();
            rptViewer.LocalReport.ReportPath = "Reports\\Reports\\rptRoster.rdlc";
            rptViewer.LocalReport.SetParameters(param);
            rptViewer.LocalReport.DataSources.Add(rptscr1);
            rptViewer.LocalReport.DataSources.Add(rptscr);
            rptViewer.LocalReport.Refresh();
            rptViewer.LocalReport.DisplayName = reportDisplayName;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    DataTable employeeRosterDetail = (DataTable)Session["employeeRosterDetail"];
                    DataTable dateTable = (DataTable)Session["dateTable"];
                    Dictionary<string, string> rptParam = (Dictionary<string, string>)Session["rptParamDict"];
                    string reportDisplayName = (string)Session["rptDisplayName"];
                    loadReport(employeeRosterDetail, dateTable, rptParam, reportDisplayName);
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }
    }
}