using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Reports;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace GroupHRIS.Reports
{
    public partial class LeaveBalanceReport : System.Web.UI.Page
    {
        void fillDDLYear()
        {
            int CurrentYear = Convert.ToInt32(DateTime.Now.Year.ToString());
            
            ddlYear.Items.Clear();

            ddlYear.Items.Add(CurrentYear.ToString());
            ddlYear.Items.Add((CurrentYear - 1).ToString());
            ddlYear.Items.Add((CurrentYear - 2).ToString());
            ddlYear.Items.Add((CurrentYear - 3).ToString());
            ddlYear.Items.Add((CurrentYear - 4).ToString());   
        }

        void fillDDLCompany()
        {
            LeaveBalanceReportDataHandler LBRDH = new LeaveBalanceReportDataHandler();
            DataTable dt = new DataTable();

            dt = LBRDH.PopulateCompany().Copy();

            ddlCompany.Items.Clear();
            ddlCompany.Items.Add(new ListItem("All Companies", ""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string value = dt.Rows[i]["COMPANY_ID"].ToString();
                string text = dt.Rows[i]["COMP_NAME"].ToString();
                ddlCompany.Items.Add(new ListItem(text, value)); 
            }
        }

        void fillDDLDepartment(string CompanyID)
        {
            LeaveBalanceReportDataHandler LBRDH = new LeaveBalanceReportDataHandler();
            DataTable dt = new DataTable();

            dt = LBRDH.PopulateDepartments(CompanyID).Copy();

            ddlDepartment.Items.Clear();
            ddlDepartment.Items.Add(new ListItem("", ""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string value = dt.Rows[i]["DEPT_ID"].ToString();
                string text = dt.Rows[i]["DEPT_NAME"].ToString();
                ddlDepartment.Items.Add(new ListItem(text, value));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillDDLYear();
                fillDDLCompany();
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
                        ddlCompany.SelectedIndex = 0;
                        ddlDepartment.Items.Clear();
                        //Postback Methods
                    }
                }
            }
        }




        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string mrptsubheader = " ";

            LeaveBalanceReportDataHandler LBRDH = new LeaveBalanceReportDataHandler();
            DataTable DT = new DataTable();
            if (txtEmployee.Text == String.Empty)
            {
                if (ddlCompany.SelectedIndex == 0)
                {
                    DT = LBRDH.Populate(ddlYear.SelectedItem.Text.Trim()).Copy();
                    mrptsubheader = "All Companies" + Environment.NewLine + Environment.NewLine + "All Employees";
                }
                else if ((ddlCompany.SelectedIndex != 0) && (ddlDepartment.SelectedIndex == 0))
                {
                    //All Employees in a Company
                    mrptsubheader = "Company : " + ddlCompany.SelectedItem.Text.Trim(); 
                    DT = LBRDH.PopulateByCompany(txtEmployee.Text.Trim(), ddlYear.SelectedItem.Text.Trim(),ddlCompany.SelectedValue).Copy();
                }
                else if (ddlDepartment.SelectedIndex != 0)
                {
                    //All Employees in a Department 
                    mrptsubheader = "Company : " + ddlCompany.SelectedItem.Text.Trim() + "    Department : " + ddlDepartment.SelectedItem.Text.Trim(); 
                    DT = LBRDH.PopulateByDepartment(txtEmployee.Text.Trim(), ddlYear.SelectedItem.Text.Trim(), ddlDepartment.SelectedValue).Copy();
                }
            }
            else
            {
                mrptsubheader = "Company : " + LBRDH.GetCompanyName(txtEmployee.Text.Trim()); 
                DT = LBRDH.Populate(txtEmployee.Text.Trim(), ddlYear.SelectedItem.Text.Trim()).Copy();
            }

            //Change DataTypes of the data table columns
            DataTable dtCloned = DT.Clone();
            dtCloned.Columns["NUMBER_OF_DAYS"].DataType = typeof(string);
            dtCloned.Columns["LEAVE_TAKEN"].DataType = typeof(string);
            dtCloned.Columns["BALANCE"].DataType = typeof(string);
            foreach (DataRow row in DT.Rows)
            {
                dtCloned.ImportRow(row);
            }
            DT = new DataTable();
            DT = dtCloned.Copy();
            //


            ////Reconfigure Short leave
            DataRow[] customerRow = DT.Select("LEAVE_TYPE_NAME = 'SHORT LEAVE'");
            try
            {
                //customerRow[0]["NUMBER_OF_DAYS"] = (Convert.ToDouble(customerRow[0]["NUMBER_OF_DAYS"]) * 4).ToString();
                //customerRow[0]["LEAVE_TAKEN"] = (Convert.ToDouble(customerRow[0]["LEAVE_TAKEN"]) * 4).ToString();
                //customerRow[0]["BALANCE"] = (Convert.ToDouble(customerRow[0]["BALANCE"]) * 4).ToString();

                for (int i = 0; i < customerRow.Length; i++)
                {
                    //2015/09/10 
                    //customerRow[i]["NUMBER_OF_DAYS"] = (Convert.ToDouble(customerRow[i]["NUMBER_OF_DAYS"])).ToString(); //2015/09/15
                    //customerRow[i]["NUMBER_OF_DAYS"] = "2";
                    customerRow[i]["NUMBER_OF_DAYS"] = " ";
                    customerRow[i]["LEAVE_TAKEN"] = (Convert.ToDouble(customerRow[i]["LEAVE_TAKEN"]) * 4).ToString();
                    //customerRow[i]["BALANCE"] = ((Convert.ToDouble(customerRow[i]["NUMBER_OF_DAYS"])) - (Convert.ToDouble(customerRow[i]["LEAVE_TAKEN"]))).ToString();
                    customerRow[i]["BALANCE"] = " ";
                }
            }
            catch
            { }
            ////
            
            try
            {
                string mrptheader = "Leave Balance Report";

                //if (txtEmployee.Text.Trim() == "")
                //{
                //    mrptsubheader = " ";
                //}
                //else
                //{
                //  //  mrptsubheader = "Employee Name : " + LBRDH.GetEmployeeName(txtEmployee.Text.Trim()).Trim();
                //}

                //ReportViewer1.Reset();
                //ReportDataSource rptscr = new ReportDataSource("DataSet1", DT);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rptscr);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/LeaveBalanceReport.rdlc");
                //ReportParameter[] param = new ReportParameter[2];
                //param[0] = new ReportParameter("headerpara", mrptheader + " - " + ddlYear.SelectedItem.Text);
                //param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("headerpara", mrptheader + " - " + ddlYear.SelectedItem.Text);
                paramdict.Add("subheaderpara", mrptsubheader);

                Session["rptDataSet"] = DT;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Leave Balance Report";

                Response.Redirect("~/Reports/ReportViewers/LeaveBalanceReport.aspx");
            }
            catch (Exception ex)
            {
                //throw ex;

                //CommonVariables.MESSAGE_TEXT = ex.Message;
                //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlDepartment.Items.Clear();
            fillDDLDepartment(ddlCompany.SelectedValue);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtEmployee.Text = String.Empty;
            try
            {
                ddlYear.SelectedIndex = 0;
            }
            catch
            { }

            try
            {
                ddlCompany.SelectedIndex = 0;
            }
            catch
            { }

            try
            {
                ddlDepartment.SelectedIndex = 0;
            }
            catch
            { }
            ReportViewer1.Reset();
        }
    }
}