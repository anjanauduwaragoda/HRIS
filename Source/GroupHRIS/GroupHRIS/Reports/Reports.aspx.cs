using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Reports;
using System.Data;
using Common;
using DataHandler.MetaData;
using DataHandler.Attendance;

namespace GroupHRIS.Reports
{
    public partial class Reports : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //string KeyCOMP_ID = "CP01";
                string KeyCOMP_ID = (string)(Session["KeyCOMP_ID"]);
                getreports();
            }
        }

        private void getreports()
        {

            ReportDataHandler reportDataHandler = new ReportDataHandler();
            DataTable dtreports = reportDataHandler.populateReports((Session["KeyHRIS_ROLE"] as string));

            try
            {

                GridView1.DataSource = dtreports;
                GridView1.DataBind();

                if (dtreports.Rows.Count > 0)
                {
                    GridView1.HeaderRow.Cells[0].Text = "Report Code";
                    GridView1.HeaderRow.Cells[1].Text = "Report Name";
                    GridView1.HeaderRow.Cells[2].Text = "Action";
                    GridView1.HeaderRow.Cells[0].Width = 100;
                    GridView1.HeaderRow.Cells[1].Width = 450;
                    GridView1.HeaderRow.Cells[2].Width = 150;
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
                dtreports.Dispose();
                dtreports = null;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string sReportCode = e.Row.Cells[0].Text.ToString();
                HyperLink mreportLink = (HyperLink)(e.Row.FindControl("hlkreport"));
                
                //mreportLink.NavigateUrl = "javascript:MyPopUpWin('ReportGenerator.aspx?mRepName=" + sReportCode + "','1366','768')";
                mreportLink.NavigateUrl = "ReportGenerator.aspx?mRepName=" + sReportCode;
                mreportLink.Target = "_blank"; 
            }

        }

    }
}