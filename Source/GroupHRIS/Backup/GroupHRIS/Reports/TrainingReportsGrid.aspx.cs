using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using Common;
using DataHandler.Reports;
using System.Data;

namespace GroupHRIS.Reports
{
    public partial class TrainingReportsGrid : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "TrainingReportsGrid : Page_Load");

            if (!IsPostBack)
            {
                loadTrainingReportGrid();
            }
        }

        private void loadTrainingReportGrid()
        {
            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtTrainingReports = new DataTable();
            try
            {
                log.Debug("TrainingReportsGrid : loadTrainingReportGrid()");
                dtTrainingReports = reportDataHandler.getAllTainingReports();

                if (dtTrainingReports.Rows.Count > 0)
                {
                    gvTrainingReports.DataSource = dtTrainingReports;
                    gvTrainingReports.DataBind();
                }
                else
                {
                    gvTrainingReports.DataSource = null;
                    gvTrainingReports.DataBind();
                }
            }
            catch (Exception ex)
            {
                log.Debug("TrainingReportsGrid : loadTrainingReportGrid()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                reportDataHandler = null;
                dtTrainingReports.Dispose();
            }
        }

        protected void gvTrainingReports_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                log.Debug("TrainingReportsGrid : gvTrainingReports_RowDataBound()");

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvTrainingReports, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }

            }
            catch (Exception ex)
            {
                log.Debug("TrainingReportsGrid : gvTrainingReports_RowDataBound()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void gvTrainingReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("TrainingReportsGrid : gvTrainingReports_SelectedIndexChanged()");

                string selectedReportCode = gvTrainingReports.SelectedRow.Cells[1].Text.ToString();
                Session["SelectedReportCode"] = selectedReportCode;
                Response.Redirect("ReportFilter/TainingReportsGenerator.aspx");
            }
            catch (Exception ex)
            {
                log.Debug("TrainingReportsGrid : gvTrainingReports_SelectedIndexChanged()");
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

    }
}