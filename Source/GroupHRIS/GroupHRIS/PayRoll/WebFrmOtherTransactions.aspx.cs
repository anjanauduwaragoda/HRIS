using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.MetaData;
using System.Data;
using Common;
using DataHandler.Payroll;
using GroupHRIS.Utility;
using System.Globalization;

namespace GroupHRIS.PayRoll
{
    public partial class WebFrmOtherTransactions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (hfCaller.Value == "txtEmploeeId")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtEmploeeId.Text = hfVal.Value;
                    }
                    if (txtEmploeeId.Text != "")
                    {
                        //Postback Methods
                        txtEmploeeId.Enabled = false;
                        var month = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
                        lblTransactionMonth.Text = month;


                        GetAllTransactions();
                        GetCompany();
                        GetEmployeeName();
                        GetEmployeeEPF();
                        fillCategory();

                    }
                }
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillSubcategory();
        }

        protected void ddlSubcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTypeId();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clearData();

            ddlCategory.Items.Clear();
            ddlSubcategory.Items.Clear();

            lblEPFNo.Text = "";
            lblCompany.Text = "";
            lblEmployeeName.Text = "";
            txtEmploeeId.Text = "";

            Errorhandler.ClearError(StatusLabel);
            gvTransactions.DataSource = null;
            gvTransactions.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string company = lblCompany.Text;
            string empId = txtEmploeeId.Text.ToString();
            string category = ddlCategory.SelectedValue.ToString();
            string subcategory = ddlSubcategory.SelectedValue.ToString();
            string typeId = lblTypeId.Text;
            string amount = txtAmount.Text.ToString();
            string logUser = Session["KeyUSER_ID"].ToString();
            
            try
            {
                Errorhandler.ClearError(StatusLabel);
                OtherTransactionsDataHandler oOtherTransactionsDataHandler = new OtherTransactionsDataHandler();

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    Boolean status = oOtherTransactionsDataHandler.Insert(empId, category, subcategory, typeId, amount, logUser);
                    GetAllTransactions();
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", StatusLabel);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string updateCategory = Session["Category"].ToString();
                    string updateSubcategory = Session["Subcategory"].ToString();

                    Boolean Status = oOtherTransactionsDataHandler.UpdateOtherTransactions(updateCategory, updateSubcategory, empId, category, subcategory, typeId, amount, logUser);
                    GetAllTransactions();
                    Errorhandler.GetError("1", "Successfully Updated", StatusLabel);
                }
                clearData();

            }
            catch (Exception)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Record Alredy Exist", StatusLabel);
            }
        }

        protected void txtEmploeeId_TextChanged(object sender, EventArgs e)
        {
            ddlCategory.Items.Clear();
            ddlSubcategory.Items.Clear();
            GetAllTransactions();
            GetCompany();
            GetEmployeeName();
            GetEmployeeEPF();
            fillCategory();
        }

        protected void gvTransactions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvTransactions, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, StatusLabel);
            }
        }

        protected void gvTransactions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTransactions.PageIndex = e.NewPageIndex;
            gvTransactions.DataBind();
        }

        protected void gvTransactions_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            GetCompany();
            GetEmployeeName();
            
            int SelectedIndex = gvTransactions.SelectedIndex;

            fillCategory(); 
            ddlCategory.SelectedValue = Server.HtmlDecode(gvTransactions.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());
            
            fillSubcategory();
            ddlSubcategory.SelectedValue = Server.HtmlDecode(gvTransactions.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
            lblTypeId.Text = Server.HtmlDecode(gvTransactions.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());
            txtAmount.Text = Server.HtmlDecode(gvTransactions.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
            Status(true);
            Session["Subcategory"] = ddlSubcategory.SelectedValue.ToString();
            Session["Category"] = ddlCategory.SelectedValue.ToString();
        }

        public void fillCategory()
        {
            ddlCategory.Items.Clear();
            string company = lblCompany.Text;
            lblTypeId.Text = "";
            OtherTransactionsDataHandler oOtherTransactionsDataHandler = new OtherTransactionsDataHandler();
            DataTable table = new DataTable();
            table = oOtherTransactionsDataHandler.GetCategories(hfcompanyId.Value).Copy();

            ddlCategory.Items.Add(new ListItem("", ""));

            for (int i = 0; i < table.Rows.Count; i++)
            {
                string text = table.Rows[i]["CATEGORY"].ToString();
                ddlCategory.Items.Add(new ListItem(text));
            }
        }

        public void fillSubcategory()
        {

            lblTypeId.Text = "";
            ddlSubcategory.Items.Clear();
            string category = ddlCategory.SelectedValue.ToString();

            OtherTransactionsDataHandler oOtherTransactionsDataHandler = new OtherTransactionsDataHandler();
            DataTable table = new DataTable();
            table = oOtherTransactionsDataHandler.GetSubcategories(category).Copy();

            ddlSubcategory.Items.Add(new ListItem("", ""));

            for (int i = 0; i < table.Rows.Count; i++)
            {
                string text = table.Rows[i]["SUB_CATEGORY"].ToString();
                ddlSubcategory.Items.Add(new ListItem(text));
            }
        }

        public void GetTypeId()
        {
            string company = lblCompany.Text;
            string category = ddlCategory.SelectedValue.ToString();
            string subcategory = ddlSubcategory.SelectedValue.ToString();

            OtherTransactionsDataHandler oOtherTransactionsDataHandler = new OtherTransactionsDataHandler();
            DataTable table = new DataTable();
            table = oOtherTransactionsDataHandler.GetTypeId(company, category, subcategory);

            if (table.Rows.Count > 0)
            {
                lblTypeId.Text = table.Rows[0]["SUB_CAT_TYPE_ID"].ToString();
            }
            else if (table.Rows.Count == 0)
            {
                lblTypeId.Text = "0";
            }
        }

        public void GetCompany()
        {
            string empId = txtEmploeeId.Text;
            OtherTransactionsDataHandler oOtherTransactionsDataHandler = new OtherTransactionsDataHandler();
            DataTable table = new DataTable();
            table = oOtherTransactionsDataHandler.GetEmployeeCompany(empId);
            lblCompany.Text = table.Rows[0]["COMP_NAME"].ToString();
            hfcompanyId.Value = table.Rows[0]["COMPANY_ID"].ToString();
        }

        public void GetEmployeeName()
        {
            string empId = txtEmploeeId.Text;
            OtherTransactionsDataHandler oOtherTransactionsDataHandler = new OtherTransactionsDataHandler();
            DataTable table = new DataTable();
            table = oOtherTransactionsDataHandler.GetEmployeeName(empId);
            lblEmployeeName.Text = table.Rows[0]["INITIALS_NAME"].ToString();
        }

        public void GetEmployeeEPF()
        {
            string empId = txtEmploeeId.Text;
            OtherTransactionsDataHandler oOtherTransactionsDataHandler = new OtherTransactionsDataHandler();
            DataTable table = new DataTable();
            table = oOtherTransactionsDataHandler.GetEmployeeEPF(empId);
            lblEPFNo.Text = table.Rows[0]["EPF_NO"].ToString();
        }

        public void GetAllTransactions()
        {
            string empId = txtEmploeeId.Text;
            OtherTransactionsDataHandler oOtherTransactionsDataHandler = new OtherTransactionsDataHandler();
            DataTable table = new DataTable();
            table = oOtherTransactionsDataHandler.GetEmployeeData(empId).Copy();

            gvTransactions.DataSource = table;
            gvTransactions.DataBind();
        }

        public void Status(Boolean status)
        {
            txtEmploeeId.Enabled = status;
            ddlCategory.Enabled = status;
            ddlSubcategory.Enabled = status;

        }

        public void clearData()
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            Status(true);
            txtAmount.Text = "";
            lblTypeId.Text = "";
            
            ddlCategory.SelectedIndex = 0;
            ddlSubcategory.SelectedIndex = 0;
        }

    }
}