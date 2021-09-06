using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Reports;
using System.Data;
using Microsoft.Reporting.WebForms;
using Common;

namespace GroupHRIS.Reports
{
    public partial class LateAttendanceReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillComapanyDDL();
            }
        }

        void fillComapanyDDL()
        {
            LateAttendanceDataHandler LADH = new LateAttendanceDataHandler();
            DataTable dt = new DataTable();
            dt = LADH.populateCompanies().Copy();

            ddlCompany.Items.Clear();

            ddlCompany.Items.Add(new ListItem("", ""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string text = dt.Rows[i]["COMP_NAME"].ToString();
                string value = dt.Rows[i]["COMPANY_ID"].ToString();
                ddlCompany.Items.Add(new ListItem(text, value));            
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            LateAttendanceDataHandler LADH = new LateAttendanceDataHandler();
            DataTable dt = new DataTable();


            try
            {
                string mrptheader = "";
                string mrptsubheader = "";

                if (ddlCompany.SelectedIndex == 0)
                {
                    dt = LADH.populateAllByDate(txtDate.Text.Trim()).Copy();
                    mrptheader = "Late Attendance & Early Departure Report";
                    mrptsubheader = "Date : " + txtDate.Text.Trim();
                }
                else if (ddlCompany.SelectedIndex != 0)
                {
                    dt = LADH.populateAllByDateNCompany(txtDate.Text.Trim(),ddlCompany.SelectedValue).Copy();
                    mrptheader = "Late Attendance & Early Departure Report";
                    mrptsubheader = "Company : " + ddlCompany.SelectedItem.Text.Trim() + Environment.NewLine + Environment.NewLine + "Date : " + txtDate.Text.Trim();
                }

                ReportViewer1.Reset();
                ReportDataSource rptscr = new ReportDataSource("DataSet1", dt);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rptscr);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/LateAttendance.rdlc");
                ReportParameter[] param = new ReportParameter[2];
                param[0] = new ReportParameter("headerpara", mrptheader);
                param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                ReportViewer1.LocalReport.SetParameters(param);
                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
                //CommonVariables.MESSAGE_TEXT = ex.Message;
                //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }        
        }
    }
}