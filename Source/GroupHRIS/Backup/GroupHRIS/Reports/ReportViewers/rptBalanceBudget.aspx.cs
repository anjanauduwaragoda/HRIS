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
    public partial class rptBalanceBudget : System.Web.UI.Page
    {
        void loadReport(DataTable ReportDataSet1, DataTable ReportDataSet2, DataTable ReportDataSet3, Dictionary<string, string> ReportParameters)
        {
            int paramCount = ReportParameters.Count;
            ReportParameter[] param = new ReportParameter[paramCount];
            for (int i = 0; i < paramCount; i++)
            {
                string key = ReportParameters.Keys.ElementAt(i);
                param[i] = new ReportParameter(key, ReportParameters[key]);
            }

            ReportDataSource rptscr1 = new ReportDataSource("DataSet1", ReportDataSet1);
            ReportDataSource rptscr2 = new ReportDataSource("DataSet2", ReportDataSet2);
            ReportDataSource rptscr3 = new ReportDataSource("DataSet3", ReportDataSet3);

            rptViewer.LocalReport.DataSources.Clear();
            rptViewer.LocalReport.ReportPath = "Reports\\Reports\\rptBalanceBudget.rdlc";
            rptViewer.LocalReport.SetParameters(param);
            rptViewer.LocalReport.DataSources.Add(rptscr1);
            rptViewer.LocalReport.DataSources.Add(rptscr2);
            rptViewer.LocalReport.DataSources.Add(rptscr3);
            rptViewer.LocalReport.Refresh();
            rptViewer.LocalReport.DisplayName = "Late Commers Report";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    DataTable rptData1 = (DataTable)Session["rptDataSet1"];
                    DataTable rptData2 = (DataTable)Session["rptDataSet2"];
                    DataTable rptData3 = (DataTable)Session["rptDataSet3"];
                    Dictionary<string, string> rptParam = (Dictionary<string, string>)Session["rptParamDict"];
                    loadReport(rptData1, rptData2, rptData3, rptParam);
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }
    }
}