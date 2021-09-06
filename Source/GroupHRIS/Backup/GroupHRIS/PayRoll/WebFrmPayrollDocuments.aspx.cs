using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using DataHandler.MetaData;
using System.Data;
using Common;
using DataHandler.Payroll;
using GroupHRIS.Utility;
using DataHandler.Userlogin;
namespace GroupHRIS.PayRoll
{
    public partial class WebFrmPayrollDocuments : System.Web.UI.Page
    {
        private void LoadCompanies()
        {
            PayrollDataHandler PDH = new PayrollDataHandler();
            DataTable dtCompanies = new DataTable();
            try
            {
                ddlCompany.Items.Clear();

                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        dtCompanies = PDH.getCompany().Copy();
                    }
                    else
                    {
                        dtCompanies = PDH.getCompany(Session["KeyCOMP_ID"].ToString().Trim()).Copy();
                    }

                    if (dtCompanies.Rows.Count > 0)
                    {
                        ddlCompany.Items.Add(new ListItem("", ""));
                        foreach (DataRow dr in dtCompanies.Rows)
                        {
                            string Text = dr["COMP_NAME"].ToString();
                            string Value = dr["COMPANY_ID"].ToString();

                            ddlCompany.Items.Add(new ListItem(Text, Value));
                        }

                        if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_ETI_COMPANY_ID))
                        {
                            ddlCompany.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                PDH = null;
                dtCompanies.Dispose();
            }
        }

        private void LoadYears()
        {
            ddlYear.Items.Clear();
            int currentYear = DateTime.Now.Year;
            //ddlYear.Items.Add(new ListItem("", ""));
            for (int i = currentYear; i >= (currentYear - 50); i--)
            {
                ddlYear.Items.Add(new ListItem(i.ToString().Trim(), i.ToString().Trim()));
            }
        }

        private void LoadMonths()
        {
            //ddlMonth.Items.Add(new ListItem("", ""));

            ddlMonth.Items.Clear();

            for (int i = 1; i <= 12; i++)
            {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                ddlMonth.Items.Add(new ListItem(monthName, i.ToString()));
            }

            if (System.DateTime.Now.Month != 1)
            {
                ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(((System.DateTime.Now.Month) - 1).ToString().Trim()));
            }
            else
            {
                ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByValue(((System.DateTime.Now.Year) - 1).ToString().Trim()));
                ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue("12"));
            }
        }

        private void LoadPayrollData()
        {
            PayrollDataHandler PDH = new PayrollDataHandler();
            DataTable dtPayrollData = new DataTable();
            PasswordHandler PH = new PasswordHandler();

            try
            {
                string CompanyID = ddlCompany.SelectedValue.ToString().Trim();
                string TransMonth = ddlYear.SelectedValue.ToString().Trim() + ddlMonth.SelectedValue.ToString().Trim().PadLeft(2, '0');

                dtPayrollData = PDH.GetPayrollDetails(CompanyID, TransMonth).Copy();

                grdvPayrollData.DataSource = dtPayrollData.Copy();
                grdvPayrollData.DataBind();

                

                if (dtPayrollData.Rows.Count > 0)
                {
                    string FileName = ddlCompany.SelectedItem.Text.Replace(' ', '_') + "_" + ddlMonth.SelectedItem.Text + "_" + ddlYear.SelectedItem.Text + ".xlsx";
                    string MappedPath = Server.MapPath("~/PayRoll");
                    hyplnkDownload.Text = FileName;
                    hyplnkDownload.NavigateUrl = "~/PayRoll/PayrollDownload.ashx?C=" + PH.Encrypt(CompanyID) + "&T=" + PH.Encrypt(TransMonth) + "&F=" + PH.Encrypt(FileName) + "&P=" + PH.Encrypt(MappedPath);
                }
                else
                {
                    hyplnkDownload.Text = "No Records Found";
                    hyplnkDownload.NavigateUrl = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                PDH = null;
                PH = null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadCompanies();
                    LoadYears();
                    LoadMonths();
                    LoadPayrollData();
                }
            }
            catch (Exception ex)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMessage);
            }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadPayrollData();
            }
            catch (Exception ex)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMessage);
            }
            finally
            { 
            
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadPayrollData();
            }
            catch (Exception ex)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMessage);
            }
            finally
            {

            }
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadPayrollData();
            }
            catch (Exception ex)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMessage);
            }
            finally
            {

            }
        }


    }
}