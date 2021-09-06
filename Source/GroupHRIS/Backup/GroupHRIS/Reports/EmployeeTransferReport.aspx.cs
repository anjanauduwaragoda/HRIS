using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Reports;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Text;
using DataHandler.MetaData;
using GroupHRIS.Utility;
using Common;

namespace GroupHRIS.Reports
{
    public partial class EmployeeTransferReport : System.Web.UI.Page
    {
        void fillCompanyddl()
        {
            EmployeeTransferReportDataHandler ETRDH = new EmployeeTransferReportDataHandler();
            DataTable dt = new DataTable();
            dt = ETRDH.PopulateCompany().Copy();

            ddlFromCompany.Items.Clear();
            ddlToCompany.Items.Clear();

            ddlFromCompany.Items.Add(new ListItem("", ""));
            ddlToCompany.Items.Add(new ListItem("", ""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string value = dt.Rows[i]["COMPANY_ID"].ToString();
                string text = dt.Rows[i]["COMP_NAME"].ToString();

                ddlFromCompany.Items.Add(new ListItem(text, value));
                ddlToCompany.Items.Add(new ListItem(text, value));
            }
        }
        
        void fillFromDepartmentddl(string CompanyID)
        {
            EmployeeTransferReportDataHandler ETRDH = new EmployeeTransferReportDataHandler();
            DataTable dt = new DataTable();
            dt = ETRDH.PopulateDepartment(CompanyID).Copy();

            ddlFromDepartment.Items.Clear();

            ddlFromDepartment.Items.Add(new ListItem("", ""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string value = dt.Rows[i]["DEPT_ID"].ToString();
                string text = dt.Rows[i]["DEPT_NAME"].ToString();

                ddlFromDepartment.Items.Add(new ListItem(text, value));
            }
        }

        void fillToDepartmentddl(string CompanyID)
        {
            EmployeeTransferReportDataHandler ETRDH = new EmployeeTransferReportDataHandler();
            DataTable dt = new DataTable();
            dt = ETRDH.PopulateDepartment(CompanyID).Copy();

            ddlToDepartment.Items.Clear();

            ddlToDepartment.Items.Add(new ListItem("", ""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string value = dt.Rows[i]["DEPT_ID"].ToString();
                string text = dt.Rows[i]["DEPT_NAME"].ToString();

                ddlToDepartment.Items.Add(new ListItem(text, value));
            }
        }

        void fillFromDivisionddl(string DivisionID)
        {
            EmployeeTransferReportDataHandler ETRDH = new EmployeeTransferReportDataHandler();
            DataTable dt = new DataTable();
            dt = ETRDH.PopulateDivision(DivisionID).Copy();

            ddlFromDivision.Items.Clear();

            ddlFromDivision.Items.Add(new ListItem("", ""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string value = dt.Rows[i]["DIVISION_ID"].ToString();
                string text = dt.Rows[i]["DIV_NAME"].ToString();

                ddlFromDivision.Items.Add(new ListItem(text, value));
            }
        }

        void fillToDivisionddl(string DivisionID)
        {
            EmployeeTransferReportDataHandler ETRDH = new EmployeeTransferReportDataHandler();
            DataTable dt = new DataTable();
            dt = ETRDH.PopulateDivision(DivisionID).Copy();

            ddlToDivision.Items.Clear();

            ddlToDivision.Items.Add(new ListItem("", ""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string value = dt.Rows[i]["DIVISION_ID"].ToString();
                string text = dt.Rows[i]["DIV_NAME"].ToString();

                ddlToDivision.Items.Add(new ListItem(text, value));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillCompanyddl();
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
                        ReportViewer1.Reset();
                        ReportViewer1.LocalReport.Refresh();
                        Errorhandler.ClearError(lblMsg);
                    }
                }
            }
        }

        protected void ddlFromCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillFromDepartmentddl(ddlFromCompany.SelectedValue);
            fillFromDivisionddl(ddlFromDepartment.SelectedValue);


            ReportViewer1.Reset();
            ReportViewer1.LocalReport.Refresh();
            Errorhandler.ClearError(lblMsg);
        }

        protected void ddlToCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillToDepartmentddl(ddlToCompany.SelectedValue);
            fillToDivisionddl(ddlToDepartment.SelectedValue);

            ReportViewer1.Reset();
            ReportViewer1.LocalReport.Refresh();
            Errorhandler.ClearError(lblMsg);
        }

        protected void ddlFromDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillFromDivisionddl(ddlFromDepartment.SelectedValue);
            ReportViewer1.Reset();
            ReportViewer1.LocalReport.Refresh();
            Errorhandler.ClearError(lblMsg);
        }

        protected void ddlToDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillToDivisionddl(ddlToDepartment.SelectedValue);
            ReportViewer1.Reset();
            ReportViewer1.LocalReport.Refresh();
            Errorhandler.ClearError(lblMsg);
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMsg);


            //DateTime fdt = Convert.ToDateTime(txtTransferDate);
            //DateTime tdt = Convert.ToDateTime(txtToDate);
            try
            {
                string[] dtfromarr = txtTransferDate.Text.Trim().Split('/');
                string[] dttomarr = txtToDate.Text.Trim().Split('/');

                DateTime dtfrom1 = Convert.ToDateTime(dtfromarr[2] + '-' + dtfromarr[1] + '-' + dtfromarr[0]);
                DateTime dtto1 = Convert.ToDateTime(dttomarr[2] + '-' + dttomarr[1] + '-' + dttomarr[0]);

                string fromdate1 = dtfromarr[2] + '-' + dtfromarr[1] + '-' + dtfromarr[0];
                string todate1 = dttomarr[2] + '-' + dttomarr[1] + '-' + dttomarr[0];

                if (dtfrom1 > dtto1)
                {
                    CommonVariables.MESSAGE_TEXT = "From date is greater than To date";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMsg);
                    return;
                }
            }
            catch { }
            ReportViewer1.Reset();
            ReportViewer1.LocalReport.Refresh();

            Boolean isEmpID = false;
            Boolean isDate = false;
            Boolean isToDate = false;
            Boolean isFromCompany = false;
            Boolean isFromDepartment = false;
            Boolean isFromDivision = false;
            Boolean isToCompany = false;
            Boolean isToDepartment = false;
            Boolean isToDivision = false;
            Boolean isFromCC = false;
            Boolean isToCC = false;
            Boolean isFromPC = false;
            Boolean isToPC = false;
            Boolean isFromEmployerEPF = false;
            Boolean isToEmployerEPF = false;
            Boolean isDOJ = false;

            string whereString = "";
            Boolean[] whereArr = new Boolean[16];


            EmployeeTransferReportDataHandler ETRDH = new EmployeeTransferReportDataHandler();
            DataTable dt = new DataTable();

            string mrptheader = "Employee Transfer Report";
            string mrptsubheader = " ";

            if (txtEmployee.Text.Trim() != "")
            {
                isEmpID = true;
            }
            if (txtTransferDate.Text.Trim() != "")
            {
                isDate = true;
            }
            if (txtToDate.Text.Trim() != "")
            {
                isToDate = true;
            }
            if (ddlFromCompany.SelectedIndex > 0)
            {
                isFromCompany = true;
            }
            if (ddlFromDepartment.SelectedIndex > 0)
            {
                isFromDepartment = true;
            }
            if (ddlFromDivision.SelectedIndex > 0)
            {
                isFromDivision = true;
            }
            if (ddlToCompany.SelectedIndex > 0)
            {
                isToCompany = true;
            }
            if (ddlToDepartment.SelectedIndex > 0)
            {
                isToDepartment = true;
            }
            if (ddlToDivision.SelectedIndex > 0)
            {
                isToDivision = true;
            }
            if (txtFromCC.Text.Trim().Length > 0)
            {
                isFromCC = true;
            }
            if (txtToCC.Text.Trim().Length > 0)
            {
                isToCC = true;
            }
            if (txtFromPC.Text.Trim().Length > 0)
            {
                isFromPC = true;
            }
            if (txtToPC.Text.Trim().Length > 0)
            {
                isToPC = true;
            }
            if (txtFromEmployerEPF.Text.Trim().Length > 0)
            {
                isFromEmployerEPF = true;
            }
            if (txtToEmployerEPF.Text.Trim().Length > 0)
            {
                isToEmployerEPF = true;
            }
            if (txtDOJ.Text.Trim().Length > 0)
            {
                isDOJ = true;
            }


            whereArr[0] = isEmpID;
            whereArr[1] = isDate;
            whereArr[2] = isFromCompany;
            whereArr[3] = isFromDepartment;
            whereArr[4] = isFromDivision;
            whereArr[5] = isToCompany;
            whereArr[6] = isToDepartment;
            whereArr[7] = isToDivision;
            whereArr[8] = isFromCC;
            whereArr[9] = isToCC;
            whereArr[10] = isFromPC;
            whereArr[11] = isToPC;
            whereArr[12] = isFromEmployerEPF;
            whereArr[13] = isToEmployerEPF;
            whereArr[14] = isDOJ;
            whereArr[15] = isToDate;
            
            string pcompany = String.Empty;
            string pdepartment = String.Empty;
            string pdivision = String.Empty;
            string fromtodate = String.Empty;
            string employeeName = String.Empty;          


            for (int i = 0; i < whereArr.Length; i++)
            {
                if (whereArr[i] == true)
                {
                    whereString = " ";
                    break;
                }
            }

            if (isEmpID == true)
            {
                whereString += " ET.EMPLOYEE_ID = '" + txtEmployee.Text.Trim() + "' AND";
                
                //parameter
                employeeName = ETRDH.getEmployeeName(txtEmployee.Text.Trim());
            }

            if ((isDate == true) && (isToDate == false))
            {
                string date = txtTransferDate.Text.Trim();
                string[] dateArr = date.Split('/');
                date = dateArr[2] + "/" + dateArr[1] + "/" + dateArr[0];

                whereString += " ET.START_DATE >= '" + date.Trim() + "' AND";

                //parameter
                fromtodate = "From : " + dateArr[2] + "-" + dateArr[1] + "-" + dateArr[0];
                //fromtodate = fromtodate.Insert(7, "</b>").Insert(0, "<b>");
            }

            if ((isDate == false) && (isToDate == true))
            {
                string date = txtToDate.Text.Trim();
                string[] dateArr = date.Split('/');
                date = dateArr[2] + "/" + dateArr[1] + "/" + dateArr[0];

                whereString += " ET.START_DATE <= '" + date.Trim() + "' AND";

                //parameter
                fromtodate = "To : " + dateArr[2] + "-" + dateArr[1] + "-" + dateArr[0];
                //fromtodate = fromtodate.Insert(5, "</b>").Insert(0, "<b>");

                
            }

            if ((isDate == true) && (isToDate == true))
            {
                string date = txtTransferDate.Text.Trim();
                string[] dateArr = date.Split('/');
                date = dateArr[2] + "/" + dateArr[1] + "/" + dateArr[0];


                string todate = txtToDate.Text.Trim();
                string[] todateArr = todate.Split('/');
                todate = todateArr[2] + "/" + todateArr[1] + "/" + todateArr[0];


                whereString += " ET.START_DATE >= '" + date.Trim() + "' AND ET.START_DATE <= '" + todate.Trim() + "' AND";
                mrptsubheader = " From " + dateArr[2] + "-" + dateArr[1] + "-" + dateArr[0] + " To " + todateArr[2] + "-" + todateArr[1] + "-" + todateArr[0];

                //parameter
                string frm = "From : " + dateArr[2] + "-" + dateArr[1] + "-" + dateArr[0];
                //frm = frm.Insert(7, "</b>").Insert(0, "<b>");
                string to = "   To : " + todateArr[2] + "-" + todateArr[1] + "-" + todateArr[0];
                //to = to.Insert(7, "</b>").Insert(0, "<b>");
                fromtodate = frm + to;

                if (date == todate)
                {
                    fromtodate = "Date : " + dateArr[2] + "-" + dateArr[1] + "-" + dateArr[0];
                }


            }

            if (isFromCompany == true)
            {
                whereString += " ET.FROM_COMPANY_ID = '" + ddlFromCompany.SelectedValue.Trim() + "' AND";

                //parameter
                pcompany = "From Company : " + ddlFromCompany.SelectedItem.Text.Trim();
                //pcompany = pcompany.Insert(14, "</b>").Insert(0, "<b>");
            }

            if (isFromDepartment == true)
            {
                whereString += " ET.FROM_DEPT_ID = '" + ddlFromDepartment.SelectedValue.Trim() + "' AND";

                //parameter
                pdepartment = "From Department : " + ddlFromDepartment.SelectedItem.Text.Trim();
                //pdepartment = pdepartment.Insert(17, "</b>").Insert(0, "<b>");
            }

            if (isFromDivision == true)
            {
                whereString += " ET.FROM_DIVISION_ID = '" + ddlFromDivision.SelectedValue.Trim() + "' AND";

                //parameter
                pdivision = "From Division : " + ddlFromDivision.SelectedItem.Text.Trim();
                //pdivision = pdivision.Insert(15, "</b>").Insert(0, "<b>");
            }

            if (isToCompany == true)
            {
                whereString += " ET.TO_COMPANY_ID = '" + ddlToCompany.SelectedValue.Trim() + "' AND";

                //parameter
                if(pcompany==String.Empty)
                {
                    pcompany = "To Company : " + ddlToCompany.SelectedItem.Text.Trim();
                    //pcompany = pcompany.Insert(12, "</b>").Insert(0, "<b>");
                }
                else
                {
                    pcompany += "      To Company : " + ddlToCompany.SelectedItem.Text.Trim();
                    //pcompany = pcompany.Insert(18, "</b>").Insert(0, "<b>");
                }
            }

            if (isToDepartment == true)
            {
                whereString += " ET.TO_DEPT_ID = '" + ddlToDepartment.SelectedValue.Trim() + "' AND";

                //parameter
                if(pdepartment==String.Empty)
                {
                    pdepartment = "To Department : " + ddlToDepartment.SelectedItem.Text.Trim();
                    //pdepartment = pdepartment.Insert(15, "</b>").Insert(0, "<b>");
                }
                else
                {
                    pdepartment += "      To Department : " + ddlToDepartment.SelectedItem.Text.Trim();
                    //pdepartment = pdepartment.Insert(21, "</b>").Insert(0, "<b>");
                }
            }

            if (isToDivision == true)
            {
                whereString += " ET.TO_DIVISION_ID = '" + ddlToDivision.SelectedValue.Trim() + "' AND";

                //parameter
                if (pdivision == String.Empty)
                {
                    pdivision = "To Division : " + ddlToDivision.SelectedItem.Text.Trim();
                    //pdivision = pdivision.Insert(13, "</b>").Insert(0, "<b>");
                }
                else
                {
                    pdivision += "      To Division : " + ddlToDivision.SelectedItem.Text.Trim();
                    //pdivision = pdivision.Insert(19, "</b>").Insert(0, "<b>");
                }
            }

            if (isFromCC == true)
            {
                whereString += " ET.FROM_CC = '" + txtFromCC.Text.Trim() + "' AND";
            }

            if (isToCC == true)
            {
                whereString += " ET.TO_CC = '" + txtToCC.Text.Trim() + "' AND";
            }

            if (isFromPC == true)
            {
                whereString += " ET.FROM_PC = '" + txtFromPC.Text.Trim() + "' AND";
            }

            if (isToPC == true)
            {
                whereString += " ET.TO_PC = '" + txtToPC.Text.Trim() + "' AND";
            }

            if (isFromEmployerEPF == true)
            {
                whereString += " CF.EMPLOYER_EPF = '" + txtFromEmployerEPF.Text.Trim() + "' AND";
            }

            if (isToEmployerEPF == true)
            {
                whereString += " CT.EMPLOYER_EPF = '" + txtToEmployerEPF.Text.Trim() + "' AND";
            }

            if (isDOJ == true)
            {
                whereString += " E.DOJ = '" + txtDOJ.Text.Trim() + "' AND";
            }

            if (whereString.Length > 3)
            {
                whereString = " AND " + whereString.Remove(whereString.Length - 3);
            }

            dt = configureReportTo(ETRDH.PopulateEmployee(txtEmployee.Text.Trim(), whereString).Copy()).Copy();

            string[] paramArr = new string[5];
            paramArr[0] = pcompany;
            paramArr[1] = pdepartment;
            paramArr[2] = pdivision;
            paramArr[3] = employeeName;
            paramArr[4] = fromtodate;


            mrptsubheader = "";
            //Configure Parameters
            for (int i = 0; i < paramArr.Length; i++)
            {
                if (paramArr[i] != String.Empty)
                {
                    mrptsubheader += paramArr[i] + Environment.NewLine + Environment.NewLine;
                }
            }
            //

            try
            {
                //ReportViewer1.Reset();
                //ReportDataSource rptscr = new ReportDataSource("DataSet1", dt);
                //ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.LocalReport.DataSources.Add(rptscr);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/Reports/EmployeeTransferReport.rdlc");
                //ReportParameter[] param = new ReportParameter[2];
                //param[0] = new ReportParameter("headerpara", mrptheader);
                //param[1] = new ReportParameter("subheaderpara", mrptsubheader);
                //ReportViewer1.LocalReport.SetParameters(param);
                //ReportViewer1.LocalReport.Refresh();


                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("headerpara", mrptheader);
                paramdict.Add("subheaderpara", mrptsubheader);

                Session["rptDataSet"] = dt;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Employee Transfer Report";

                Response.Redirect("~/Reports/ReportViewers/EmployeeTransferReport.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable configureReportTo(DataTable dataTable)
        {
            string FROM_RPT_1 = String.Empty;
            string FROM_RPT_2 = String.Empty;
            string FROM_RPT_3 = String.Empty;
            string TO_RPT_1 = String.Empty;
            string TO_RPT_2 = String.Empty;
            string TO_RPT_3 = String.Empty;

            ReportDataHandler oReportDataHandler = new ReportDataHandler();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                FROM_RPT_1 = dataTable.Rows[i]["FROM_RPT_1"].ToString();
                dataTable.Rows[i]["FROM_RPT_1"] = oReportDataHandler.getEmployeeName(FROM_RPT_1);

                FROM_RPT_2 = dataTable.Rows[i]["FROM_RPT_2"].ToString();
                dataTable.Rows[i]["FROM_RPT_2"] = oReportDataHandler.getEmployeeName(FROM_RPT_2);

                FROM_RPT_3 = dataTable.Rows[i]["FROM_RPT_3"].ToString();
                dataTable.Rows[i]["FROM_RPT_3"] = oReportDataHandler.getEmployeeName(FROM_RPT_3);

                TO_RPT_1 = dataTable.Rows[i]["TO_RPT_1"].ToString();
                dataTable.Rows[i]["TO_RPT_1"] = oReportDataHandler.getEmployeeName(TO_RPT_1);

                TO_RPT_2 = dataTable.Rows[i]["TO_RPT_2"].ToString();
                dataTable.Rows[i]["TO_RPT_2"] = oReportDataHandler.getEmployeeName(TO_RPT_2);

                TO_RPT_3 = dataTable.Rows[i]["TO_RPT_3"].ToString();
                dataTable.Rows[i]["TO_RPT_3"] = oReportDataHandler.getEmployeeName(TO_RPT_3);
            }

            return dataTable;
        }

        protected void btnclear_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMsg);
            ReportViewer1.Reset();
            ReportViewer1.LocalReport.Refresh();

            txtEmployee.Text = String.Empty;
            txtTransferDate.Text = String.Empty;
            txtToDate.Text = String.Empty;
            txtFromCC.Text = String.Empty;
            txtFromPC.Text = String.Empty;
            txtFromEmployerEPF.Text = String.Empty;
            txtToCC.Text = String.Empty;
            txtToPC.Text = String.Empty;
            txtToEmployerEPF.Text = String.Empty;
            txtDOJ.Text = String.Empty;

            try { ddlFromCompany.SelectedIndex = 0; }
            catch { }
            try { ddlFromDepartment.SelectedIndex = 0; }
            catch { }
            try { ddlFromDivision.SelectedIndex = 0; }
            catch { }
            try { ddlToCompany.SelectedIndex = 0; }
            catch { }
            try { ddlToDepartment.SelectedIndex = 0; }
            catch { }
            try { ddlToDivision.SelectedIndex = 0; }
            catch { }
        }

        protected void ddlFromDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReportViewer1.Reset();
            ReportViewer1.LocalReport.Refresh();
            Errorhandler.ClearError(lblMsg);
        }

        protected void ddlToDivision_SelectedIndexChanged(object sender, EventArgs e)
        {

            ReportViewer1.Reset();
            ReportViewer1.LocalReport.Refresh();
            Errorhandler.ClearError(lblMsg);
        }

    }
}