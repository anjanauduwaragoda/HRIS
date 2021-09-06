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
    public partial class InOutConfigurationReport : System.Web.UI.Page
    {
        void fillCompanyDDL()
        {
            ddlCompany.Items.Clear();

            InOutConfigurationReportDataHandler IOCRDH = new InOutConfigurationReportDataHandler();
            DataTable dt = new DataTable();
            dt = IOCRDH.populateCompany();

            ddlCompany.Items.Add(new ListItem("All Companies", ""));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string text = dt.Rows[i]["COMP_NAME"].ToString();
                string value = dt.Rows[i]["COMPANY_ID"].ToString();

                ddlCompany.Items.Add(new ListItem(text, value));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillCompanyDDL();
            }

            if (IsPostBack)
            {
                if (hfCaller.Value == "txtEmployee")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtEmployee.Text = hfVal.Value;
                    }
                    if (txtEmployee.Text != "")
                    {
                        //Postback Methods
                        ddlCompany.SelectedIndex = 0;
                        ddlDepartment.Items.Clear();
                        Utility.Errorhandler.ClearError(lblerror);
                        ReportViewer1.Reset();
                        ReportViewer1.LocalReport.Refresh();
                    }
                }
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblerror);
            ReportViewer1.Reset();
            ReportViewer1.LocalReport.Refresh();

            string deptpara = "";
            InOutConfigurationReportDataHandler IOCRDH = new InOutConfigurationReportDataHandler();
            DataTable dt = new DataTable();

            //DD/MM/YYYY
            Utility.Errorhandler.ClearError(lblerror);
            string[] dtfromarr = txtFromDate.Text.Trim().Split('/');
            string[] dttomarr = txtToDate.Text.Trim().Split('/');

            DateTime dtfrom = Convert.ToDateTime(dtfromarr[2] + '-' + dtfromarr[1] + '-' + dtfromarr[0]);
            DateTime dtto = Convert.ToDateTime(dttomarr[2] + '-' + dttomarr[1] + '-' + dttomarr[0]);

            string fromdate = dtfromarr[2] + '-' + dtfromarr[1] + '-' + dtfromarr[0];
            string todate = dttomarr[2] + '-' + dttomarr[1] + '-' + dttomarr[0];

            if (dtfrom > dtto)
            {
                CommonVariables.MESSAGE_TEXT = "From date is greater than To date";
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);

                
                ReportViewer1.Reset();
                ReportViewer1.LocalReport.Refresh();
                return;
            }

            if ((ddlCompany.SelectedIndex == 0) && (txtEmployee.Text==""))
            {
                dt = IOCRDH.populateAll(fromdate, todate).Copy();

                try
                {
                    string mrptheader = "IN-OUT Configuration Report";
                    string mrptsubheader = "All Companies";
                    string lowerheaderpara =  "";
                    if (fromdate != todate)
                    {
                        deptpara = "From : " + fromdate + "   To : " + todate;
                    }
                    else
                    {
                        deptpara = "Date : " + fromdate;
                    }

                    //ReportViewer1.Reset();
                    //ReportDataSource rptscr = new ReportDataSource("DataSet1", dt);
                    //ReportViewer1.LocalReport.DataSources.Clear();
                    //ReportViewer1.LocalReport.DataSources.Add(rptscr);
                    //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/InOutConfigurationReport.rdlc");
                    //ReportParameter[] param = new ReportParameter[3];
                    //param[0] = new ReportParameter("headerpara", mrptheader);
                    //param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                    //param[2] = new ReportParameter("deptpara", deptpara);
                    //ReportViewer1.LocalReport.SetParameters(param);
                    //ReportViewer1.LocalReport.Refresh();


                    Dictionary<string, string> paramdict = new Dictionary<string, string>();
                    paramdict.Add("headerpara", mrptheader);
                    paramdict.Add("subheaderpara", mrptsubheader);
                    paramdict.Add("deptpara", deptpara);

                    Session["rptDataSet"] = dt;
                    Session["rptParamDict"] = paramdict;
                    Session["rptDisplayName"] = "IN - OUT Configuration Report";

                    Response.Redirect("~/Reports/ReportViewers/InOutConfigurationReport.aspx");
                }
                catch (Exception ex)
                {
                    throw ex;
                    //CommonVariables.MESSAGE_TEXT = ex.Message;
                    //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                }
            }
            else
            {
                string mrptheader = "IN-OUT Configuration Report";
                string mrptsubheader = "";
                string lowerheaderpara = "";
                

                if (txtEmployee.Text != "")
                {
                    string Name = IOCRDH.employeeName(txtEmployee.Text.Trim());


                    dt = IOCRDH.populateIND(fromdate, todate, txtEmployee.Text).Copy();
                    mrptsubheader = Name;
                    if (fromdate != todate)
                    {
                        deptpara = "From : " + fromdate + "   To : " + todate;
                    }
                    else
                    {
                        deptpara = "Date : " + fromdate;
                    }
                }
                else
                {

                    if (ddlDepartment.SelectedIndex == 0)//Company Wise
                    {
                        dt = IOCRDH.populate(fromdate, todate, ddlCompany.SelectedValue).Copy();

                        mrptsubheader = ddlCompany.SelectedItem.Text.Trim();
                        if (fromdate != todate)
                        {
                            deptpara = "From : " + fromdate + "   To : " + todate;
                        }
                        else
                        {
                            deptpara = "Date : " + fromdate;
                        }
                    }
                    else//Department Wise
                    {
                        dt = IOCRDH.populateDep(fromdate, todate, ddlDepartment.SelectedValue).Copy();

                        mrptsubheader = ddlCompany.SelectedItem.Text.Trim();
                        deptpara = "Department : " + ddlDepartment.SelectedItem.Text;
                        if (fromdate != todate)
                        {
                            lowerheaderpara = "From : " + fromdate + "   To : " + todate;
                        }
                        else
                        {
                            lowerheaderpara = "Date : " + fromdate;
                        }
                    }
                }

                try
                {

                    //ReportViewer1.Reset();
                    //ReportDataSource rptscr = new ReportDataSource("DataSet1", dt);
                    //ReportViewer1.LocalReport.DataSources.Clear();
                    //ReportViewer1.LocalReport.DataSources.Add(rptscr);
                    //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/InOutConfigurationReport.rdlc");
                    //ReportParameter[] param = new ReportParameter[4];
                    //param[0] = new ReportParameter("headerpara", mrptheader);
                    //param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                    //param[2] = new ReportParameter("deptpara", deptpara);
                    //param[3] = new ReportParameter("lowerheaderpara", lowerheaderpara);
                    //ReportViewer1.LocalReport.SetParameters(param);
                    //ReportViewer1.LocalReport.Refresh();


                    Dictionary<string, string> paramdict = new Dictionary<string, string>();
                    paramdict.Add("headerpara", mrptheader);
                    paramdict.Add("subheaderpara", mrptsubheader);
                    paramdict.Add("deptpara", deptpara);
                    paramdict.Add("lowerheaderpara", lowerheaderpara);

                    Session["rptDataSet"] = dt;
                    Session["rptParamDict"] = paramdict;
                    Session["rptDisplayName"] = "IN - OUT Configuration Report";

                    Response.Redirect("~/Reports/ReportViewers/InOutConfigurationReport.aspx");
                }
                catch (Exception ex)
                {
                    throw ex; 
                    //CommonVariables.MESSAGE_TEXT = ex.Message;
                    //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblerror);
            txtFromDate.Text = txtToDate.Text = txtEmployee.Text = String.Empty;
            ddlDepartment.Items.Clear();
            try { ddlCompany.SelectedIndex = 0; }
            catch { }

            ReportViewer1.Reset();
            ReportViewer1.LocalReport.Refresh();
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {

            ReportViewer1.Reset();
            ReportViewer1.LocalReport.Refresh();

            if (ddlCompany.SelectedIndex > 0)
            {
                InOutConfigurationReportDataHandler IOCRDH = new InOutConfigurationReportDataHandler();
                DataTable dt = new DataTable();
                dt = IOCRDH.populateDepartments(ddlCompany.SelectedValue).Copy();
                ddlDepartment.Items.Clear();

                ddlDepartment.Items.Add(new ListItem("", ""));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string text = dt.Rows[i]["DEPT_NAME"].ToString();
                    string value = dt.Rows[i]["DEPT_ID"].ToString();

                    ddlDepartment.Items.Add(new ListItem(text, value));
                }
            }
            else
            {
                ddlDepartment.Items.Clear();
            }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {

            ReportViewer1.Reset();
            ReportViewer1.LocalReport.Refresh();
        }
    }
}