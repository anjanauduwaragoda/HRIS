using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Reports;
using System.Data;
using Common;

namespace GroupHRIS.Reports
{
    public partial class BranchReportGrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                    GridView1.HeaderRow.Cells[0].Width = 100;
                    GridView1.HeaderRow.Cells[1].Width = 450;
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
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.GridView1, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ReportID = GridView1.Rows[GridView1.SelectedIndex].Cells[0].Text;
            Session["RptID"] = ReportID;
            if (ReportID == "RE038")
            {
                Response.Redirect("ReportFilter/RptGeneratorTND.aspx");
            }
            else
            {
                Response.Redirect("ReportFilter/RptGenerator.aspx");
            }
        }
    }
}