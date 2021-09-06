using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.Employee;
using DataHandler.MetaData;
using Common;
using NLog;
using GroupHRIS.Utility;

namespace GroupHRIS.Employee
{
    public partial class webFrmEmployeeBankAccount : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;


            log.Debug("IP:" + sIPAddress + "webFrmEmployeeBankAccount Page_Load");

            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {
                sUserId = Session["KeyUSER_ID"].ToString();
            }


            if (!IsPostBack)
            {
                Session["KeyCOMP_ID"] = Constants.CON_UNIVERSAL_COMPANY_CODE;

                fillBanks();

            }
            else
            {
                log.Debug("page load fillBankAccounts() call");

                //if ((hfEmpId.Value.Trim() == "") || (hfEmpId.Value.Trim() != hfEID.Value.Trim()))
                //{
                //    hfEmpId.Value = hfEID.Value.Trim();
                //    txtEmployeeId.Text = hfEID.Value.Trim();

                //    clearControl();
                    
                //    if (txtEmployeeId.Text != String.Empty)
                //    {
                //        string employeeId = txtEmployeeId.Text.Trim();

                //        fillBankAccounts(employeeId);
                //        Utility.Errorhandler.ClearError(lblMessage);
                //    }
                //}

                clearControl();

                if (hfCaller.Value == "txtEmployeeId")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtEmployeeId.Text = hfVal.Value;
                    }
                    if (txtEmployeeId.Text != "")
                    {
                        //Postback Methods
                        string employeeId = txtEmployeeId.Text.Trim();

                        fillBankAccounts(employeeId);
                        Utility.Errorhandler.ClearError(lblMessage);
                    }
                }

            }
        }

        protected void gvBankAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gvBankAccounts_SelectedIndexChanged()");

            try
            {
                txtEmployeeId.Text = gvBankAccounts.SelectedRow.Cells[0].Text.ToString().Trim();
                ddlBank.SelectedValue = gvBankAccounts.SelectedRow.Cells[1].Text.ToString().Trim();
                if (ddlBankBranch.Items.Count == 0) { fillBankBranches(gvBankAccounts.SelectedRow.Cells[1].Text.ToString().Trim()); }
                ddlBankBranch.SelectedValue = gvBankAccounts.SelectedRow.Cells[3].Text.ToString().Trim();
                txtAccountNo.Text = gvBankAccounts.SelectedRow.Cells[5].Text.ToString().Trim();

                if (gvBankAccounts.SelectedRow.Cells[6].Text.ToString().Trim().Equals(Constants.STATUS_ACTIVE_TAG))
                {
                    ddlStatus.SelectedValue = Constants.STATUS_ACTIVE_VALUE;
                }
                else if (gvBankAccounts.SelectedRow.Cells[6].Text.ToString().Trim().Equals(Constants.STATUS_INACTIVE_TAG))
                {
                    ddlStatus.SelectedValue = Constants.STATUS_INACTIVE_VALUE;
                }

                txtRemarks.Text = gvBankAccounts.SelectedRow.Cells[7].Text.ToString().Trim().Replace("&nbsp;", "");

                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                Utility.Errorhandler.ClearError(lblMessage);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        protected void gvBankAccounts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvBankAccounts_RowDataBound()");

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvBankAccounts, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlBank_SelectedIndexChanged()");

            if (ddlBank.SelectedValue != "")
            {
                fillBankBranches(ddlBank.SelectedValue.Trim());
            }
        }

        protected void imgBtnView_Click(object sender, ImageClickEventArgs e)
        {
            log.Debug("imgBtnView_Click()");

            if (txtEmployeeId.Text != String.Empty)
            {
                string employeeId = txtEmployeeId.Text.Trim();

                fillBankAccounts(employeeId);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            EmployeeBankAccountDataHandler employeeBankAccountDataHandler = new EmployeeBankAccountDataHandler();

            string employeeId = "";
            string bankId = "";
            string branchId = "";
            string accountNumber = "";
            string statusCode = "";
            string remarks = "";
            string addedBy = "";
            try
            {
                if (Session["bankAccount"] != null)
                {
                    DataTable bankAccounts = (DataTable)Session["bankAccount"];

                    DataRow[] accounts = bankAccounts.Select("ACCOUNT_STATUS ='Active'");

                    if ((accounts.Length > 0) && (ddlStatus.SelectedValue.Trim().Equals("1") && (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT) ))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "This employee already has an active bank account. Please Inactive it before adding new account", lblMessage);
                        return;
                    }
                    else if ((accounts.Length > 0) && (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Make sure employee does not have two active bank accounts. Please Inactive unused account", lblMessage);
                    }
                    else if((employeeBankAccountDataHandler.isAccountExist(ddlBank.SelectedValue.Trim(),ddlBankBranch.SelectedValue.Trim(),txtAccountNo.Text.Trim())) && (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Make sure employee does not have two active bank accounts. Please Inactive unused account", lblMessage);
                    }
                }

                employeeId = txtEmployeeId.Text;
                bankId = ddlBank.SelectedValue.Trim();
                branchId = ddlBankBranch.SelectedValue.Trim();
                accountNumber = txtAccountNo.Text.Trim();
                statusCode = ddlStatus.SelectedValue.Trim();
                remarks = txtRemarks.Text.Trim();
                addedBy = sUserId;

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Insert");

                    Boolean isInserted = employeeBankAccountDataHandler.Insert(employeeId, bankId, branchId, accountNumber, statusCode, remarks, addedBy);

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                    if (isInserted) 
                    { 
                        //thcripis.Page.ClientSt.RegisterStacrirtupSpt(this.GetType(), "Alert !..", "alert('Bank account is saved ..')", true); 
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Bank account is inserted", lblMessage);
                    }

                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Update");

                    Boolean isUpdated = employeeBankAccountDataHandler.Update(employeeId, bankId, branchId, accountNumber, statusCode, remarks, addedBy);

                    if (isUpdated) 
                    { 
                        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('Bank account is updated ..')", true); 
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Bank account is updated", lblMessage);
                    }

                }

                fillBankAccounts(employeeId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                employeeBankAccountDataHandler = null;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            log.Debug("btnCancel_Click()");
            clearControls();
        }

        private void clearControls()
        {
            log.Debug("clearControls()");

            GroupHRIS.Utility.Utils.clearControls(true, txtEmployeeId, txtAccountNo, txtRemarks, ddlBank, ddlBankBranch, ddlStatus);
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblMessage);
            gvBankAccounts.DataSource = null;
            gvBankAccounts.DataBind();
        }

        private void clearControl()
        {
            log.Debug("clearControl()");

            GroupHRIS.Utility.Utils.clearControls(true, txtAccountNo, txtRemarks, ddlBankBranch, ddlStatus);
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblMessage);
            gvBankAccounts.DataSource = null;
            gvBankAccounts.DataBind();
        }

        #region Private Methods

        //------------------------------------------------------------------------------
        ///<summary>
        ///Load all banks
        ///</summary>        
        //----------------------------------------------------------------------------------------


        private void fillBanks()
        {
            log.Debug("fillBanks()");

            BankDataHandler bankDataHandler = new BankDataHandler();
            DataTable banks = new DataTable();

            try
            {
                banks = bankDataHandler.populate().Copy();

                
                ddlBank.Items.Clear();

                if (banks.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlBank.Items.Add(Item);

                    foreach (DataRow dataRow in banks.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["BANK_NAME"].ToString();
                        listItem.Value = dataRow["BANK_ID"].ToString();

                        ddlBank.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                bankDataHandler = null;
                banks.Dispose();
            }

        }

        //------------------------------------------------------------------------------
        ///<summary>
        ///Load all bank branches for a given bank 
        ///</summary>
        ///<param name="bankId">Pass a bank id string to query </param>
        //----------------------------------------------------------------------------------------

        private void fillBankBranches(string bankId)
        {
            log.Debug("fillBankBranches()");

            BankBranchDataHandler bankBranchDataHandler = new BankBranchDataHandler();
            DataTable bankBranches = new DataTable();

            try
            {
                bankBranches = bankBranchDataHandler.populate(bankId).Copy();

                ddlBankBranch.Items.Clear();

                if (bankBranches.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlBankBranch.Items.Add(Item);

                    foreach (DataRow dataRow in bankBranches.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["BRANCH_NAME"].ToString();
                        listItem.Value = dataRow["BRANCH_ID"].ToString();

                        ddlBankBranch.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                bankBranchDataHandler = null;
                bankBranches.Dispose();
            }
        }

        //------------------------------------------------------------------------------
        ///<summary>
        ///Load all bank accounts for a given employee 
        ///</summary>
        ///<param name="employee_id">Pass a employee id string to query </param>
        //----------------------------------------------------------------------------------------
        private void fillBankAccounts(string employee_id)
        {
            log.Debug("fillBankAccounts()");

            EmployeeBankAccountDataHandler employeeBankAccountDataHandler = new EmployeeBankAccountDataHandler();
            DataTable bankAccount = new DataTable();

            try
            {
                bankAccount = employeeBankAccountDataHandler.populate(employee_id).Copy();

                gvBankAccounts.DataSource = bankAccount;
                gvBankAccounts.DataBind();

                Session["bankAccount"] = bankAccount;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                employeeBankAccountDataHandler = null;
                bankAccount.Dispose();
            }

        }

        #endregion

        

    }
}