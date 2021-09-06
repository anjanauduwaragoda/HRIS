using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;

namespace GroupHRIS.Reports.ReportViewers
{
    public partial class LeaveBalanceReport : System.Web.UI.Page
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
            rptViewer.LocalReport.ReportPath = "Reports\\Reports\\LeaveBalanceReport.rdlc";
            rptViewer.LocalReport.SetParameters(param);
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