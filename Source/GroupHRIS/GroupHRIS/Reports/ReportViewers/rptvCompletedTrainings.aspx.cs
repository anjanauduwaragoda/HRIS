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
    public partial class rptvCompletedTrainings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    DataTable rptData = (DataTable)Session["rptDataSet"];
                    Dictionary<string, string> rptParam = (Dictionary<string, string>)Session["rptParamDict"];
                    string reportDisplayName = (string)Session["rptDisplayName"];
                    showReport(rptData, rptParam, reportDisplayName);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        void showReport(DataTable dtTrainingNeeds, Dictionary<string, string> ReportParameters, string reportDisplayName)
        {
            try
            {
                //dtTrainingNeeds = trainingNeedsReport.getAllRequest();

                int paramCount = ReportParameters.Count;
                ReportParameter[] param = new ReportParameter[paramCount];
                for (int i = 0; i < paramCount; i++)
                {
                    string key = ReportParameters.Keys.ElementAt(i);
                    param[i] = new ReportParameter(key, ReportParameters[key]);
                }

                ReportDataSource rptscr = new ReportDataSource("DataSet1", dtTrainingNeeds);
                rptViewer.LocalReport.DataSources.Clear();
                rptViewer.LocalReport.ReportPath = "Reports\\Reports\\rptCompletedTrainings.rdlc";
                rptViewer.LocalReport.SetParameters(param);
                rptViewer.LocalReport.DataSources.Add(rptscr);
                rptViewer.LocalReport.Refresh();
                rptViewer.LocalReport.DisplayName = reportDisplayName;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }


}