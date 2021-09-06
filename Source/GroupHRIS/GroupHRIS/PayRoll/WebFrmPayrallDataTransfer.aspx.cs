using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.Payroll;
using System.Data;
using MSSqlDataHandler;
using System.Data.SqlClient;
using GroupHRIS.Utility;

namespace GroupHRIS.PayRoll
{
    public partial class WebFrmPayrallDataTransfer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        fillCompanies();
                    }
                    else
                    {
                        fillCompanies(Session["KeyCOMP_ID"].ToString().Trim());
                        ddlcompany.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                    }
                }
                
            }
        }

        protected void ddlcompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            string company = ddlcompany.SelectedValue.ToString();
            gvPayrallTransfer.DataSource = null;
            Errorhandler.ClearError(lblMessage);

            PayrollTransactionDataHandler oPayrollTransactionDataHandler = new PayrollTransactionDataHandler();

            DataTable dataTable1 = new DataTable();
            dataTable1 = oPayrollTransactionDataHandler.GetCompanyTransactions(company).Copy();

            Session["dataTable"] = dataTable1;

            gvPayrallTransfer.DataSource = dataTable1;
            gvPayrallTransfer.DataBind();
        }

        private void fillCompanies()
        {
            CompanyRoleDataHandler companyRoleDataHandler = new CompanyRoleDataHandler();
            DataTable companies = new DataTable();

            try
            {
                if (Cache["Companies"] != null)
                {
                    companies = (DataTable)Cache["Companies"];
                }
                else
                {
                    companies = companyRoleDataHandler.getCompanyIdCompName().Copy();
                }

                ddlcompany.Items.Clear();

                if (companies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlcompany.Items.Add(Item);

                    foreach (DataRow dataRow in companies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlcompany.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                companyRoleDataHandler = null;
                companies.Dispose();
            }
        }

        private void fillCompanies(string companyId)
        {
            CompanyRoleDataHandler companyRoleDataHandler = new CompanyRoleDataHandler();
            DataTable companies = new DataTable();

            try
            {
                companies = companyRoleDataHandler.getCompanyIdCompName(companyId).Copy();

                ddlcompany.Items.Clear();

                if (companies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlcompany.Items.Add(Item);

                    foreach (DataRow dataRow in companies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlcompany.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                companyRoleDataHandler = null;
                companies.Dispose();
            }

        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            PostToPayRollDataHandler oPostToPayRollDataHandler = new PostToPayRollDataHandler(ddlcompany.SelectedValue);
            
            DataTable dataTable2 = new DataTable();
            dataTable2 = Session["dataTable"] as DataTable;

            if (dataTable2.Rows.Count > 0)
            {
                Boolean Status = oPostToPayRollDataHandler.TransferTransaction(ddlcompany.SelectedValue, dataTable2);
                Errorhandler.GetError("1", "Successfully Uploaded", lblMessage);
            }
            else 
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "No data found", lblMessage);
            }
            

        }


    }
}