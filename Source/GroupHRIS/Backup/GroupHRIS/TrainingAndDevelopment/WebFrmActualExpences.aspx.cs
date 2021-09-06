using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using Common;
using DataHandler.TrainingAndDevelopment;
using System.Data;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmActualExpences : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        private string details_add_text = "Save";
        private string details_update_text = "Update";

        private string isPaid_true_text = "1";
        private string isPaid_false_text = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmActualExpences : Page_Load");

            try
            {
                if (Session["KeyLOGOUT_STS"] == null)
                {
                    Response.Redirect("MainLogout.aspx", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login/SessionExpior.aspx", false);
            }

            if (!IsPostBack)
            {
                fillStatus(ddlTrStatus);
                fillStatus(ddlStatus);
                fillExpenseCategoryDropdown();
                hfRecordId.Value = "";
                hfExpenseId.Value = "";

                //btnExpenseUpdate.Visible = false;
            }
            if (hfCaller.Value == "txtTraining")
            {
                log.Debug("Select Training.");
                //clearAllTrainingDetails();

                clearExpenseMasterDetails();
                clearDetailExpensesInputFields();
                clearCompanyWiseExpenses();

                txtTraining.Text = hfVal.Value;
                lblTrainingName.Text = hfTrName.Value;
                lblProgramName.Text = hfProName.Value;


                populateActualExpense(hfVal.Value);
                //companyExpenseDetails(txtTraining.Text);
                //viewExpenceCategoryDetails(txtTraining.Text);
                //txtTrTotal.Text = readTotExistCost().ToString();
                //viewPerPersonCost();
                ////isRecordAlreadyExist();
                //hfCaller.Value = "";

                //if (grdCompanywiseExpence.Rows.Count > 0)
                //{
                //    disableFields(true);
                //}
                //else
                //{
                //    disableFields(false);
                //}
                hfCaller.Value = "";
            }
        }

        #region methodes

        public void fillStatus(DropDownList ddl)
        {
            log.Debug("fillStatus()");
            try
            {
                ddl.Items.Insert(0, new ListItem("", ""));
                ddl.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                ddl.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void fillExpenseCategoryDropdown()
        {
            log.Debug("fillExpenseCategoryDropdown()");
            DataTable dtCategory = new DataTable();
            ActualExpencesDataHandler actualExpencesDataHandler = new ActualExpencesDataHandler();
            try
            {
                dtCategory = actualExpencesDataHandler.getAllExpenseCategories();
                ddlcategory.Items.Add(new ListItem("", ""));

                for (int i = 0; i < dtCategory.Rows.Count; i++)
                {
                    string text = dtCategory.Rows[i]["CATEGORY_NAME"].ToString();
                    string value = dtCategory.Rows[i]["EXPENSE_CATEGORY_ID"].ToString();
                    ddlcategory.Items.Add(new ListItem(text, value));
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                actualExpencesDataHandler = null;
                dtCategory.Dispose();
            }
        }

        private void populateActualExpense(string trainingId)
        {
            log.Debug("populateActualExpense");
            ActualExpencesDataHandler actualExpencesDataHandler = new ActualExpencesDataHandler();
            DataTable dtActualExpenseHeader = new DataTable();
            DataTable dtActualExpenseCategoryDetail = new DataTable();
            DataTable dtActualExpenseCompany = new DataTable();

            try
            {
                

                dtActualExpenseHeader = actualExpencesDataHandler.getActualExpenseHeaderDetails(trainingId);
                if (dtActualExpenseHeader.Rows.Count == 1)
                {
                    populateHeaderDetails(dtActualExpenseHeader);
                    string expenceId = dtActualExpenseHeader.Rows[0]["AC_EXPENSE_ID"].ToString();
                    hfExpenseId.Value = expenceId;

                    dtActualExpenseCategoryDetail = actualExpencesDataHandler.getDetailedExpenses(expenceId);
                    populateDetailedExpenses(dtActualExpenseCategoryDetail);

                    btnDetailAdd.Text = details_update_text;
                    //btnExpenseUpdate.Visible = true;
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                actualExpencesDataHandler = null;
                dtActualExpenseCompany.Dispose();
                dtActualExpenseCategoryDetail.Dispose();
                dtActualExpenseHeader.Dispose();
            }
        }

        private void populateDetailedExpenses(DataTable dtActualExpenseCategoryDetails)
        {
            log.Debug("populateDetailedExpenses");
            try
            {
                if (dtActualExpenseCategoryDetails != null)
                {
                    gvCategoryDetails.DataSource = dtActualExpenseCategoryDetails;
                    gvCategoryDetails.DataBind();
                }
                else
                {
                    gvCategoryDetails.DataSource = null;
                    gvCategoryDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void populateHeaderDetails(DataTable dtActualExpenseHeader)
        {
            log.Debug("populateHeaderDetails");

            try
            {
                txtTrTotal.Text = dtActualExpenseHeader.Rows[0]["TOTAL_EXPENSE"].ToString();
                txtTotalDiscount.Text = dtActualExpenseHeader.Rows[0]["TOTAL_DISCOUNT"].ToString();
                txtGrandTotal.Text = dtActualExpenseHeader.Rows[0]["GRAND_TOTAL"].ToString();
                txtTrPerPersonCost.Text = dtActualExpenseHeader.Rows[0]["PER_PERSON_COST"].ToString();
                ddlTrStatus.SelectedValue = dtActualExpenseHeader.Rows[0]["STATUS_CODE"].ToString();
                txtTrDescription.Text = dtActualExpenseHeader.Rows[0]["DESCRIPTION"].ToString();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void processDetailedExpence()
        {
            log.Debug("processDetailedExpence");
            DataTable dtCompanyWiseParticipants = new DataTable();
            ActualExpencesDataHandler actualExpencesDataHandler = new ActualExpencesDataHandler();
            try
            {
                string selectedTraining = txtTraining.Text.ToString();
                string selectedExpenseCategory = ddlcategory.SelectedValue.ToString();

                //bool categoryExists = actualExpencesDataHandler.checkCategoryExistanceForExpense(selectedExpenseCategory, selectedTraining);

                //if (categoryExists && btnDetailAdd.Text == details_add_text)
                //{
                //    CommonVariables.MESSAGE_TEXT = "Expense category already exist";
                //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
                //    return;
                //}

                string detailedExpenseStatus = ddlStatus.SelectedValue.ToString();

                if (detailedExpenseStatus != Constants.STATUS_ACTIVE_VALUE && btnDetailAdd.Text == details_add_text)
                {
                    CommonVariables.MESSAGE_TEXT = "Cannot process an inactive expense";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
                    
                    return;
                }

                if (detailedExpenseStatus != Constants.STATUS_ACTIVE_VALUE && btnDetailAdd.Text == details_update_text)
                {
                    CommonVariables.MESSAGE_TEXT = "Company wise expenses removed";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);

                    gvCompanyWiseActualExpenses.DataSource = null;
                    gvCompanyWiseActualExpenses.DataBind();
                    Session["dtCompanyWiseExpenses"] = null;

                    return;
                }

                dtCompanyWiseParticipants = actualExpencesDataHandler.getCompanyWiseParticipantCount(selectedTraining);

                DataTable dtCompanyWiseExpenses = fillCompanyWiseActualExpensesDataTable(dtCompanyWiseParticipants);
                addCompanyExpensesToSession(dtCompanyWiseExpenses);
                fillCompanyWiseActualExpensesGrid();
                //disableDetailedExpenseFrom();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
            }
            finally
            {
                dtCompanyWiseParticipants.Dispose();
                actualExpencesDataHandler = null;
            }
        }

        //private void disableDetailedExpenseFrom()
        //{
        //    try
        //    {
        //        ddlcategory.Enabled = false;
        //        ddlStatus.Enabled = false; 
        //        txtDescription .Enabled = false;
        //        txtDiscount .Enabled = false;
        //        txtNetAmount .Enabled = false;
        //        txtCost .Enabled = false;
        //        txtRemark .Enabled = false;
        //        chkPaid.Enabled = false;
        //        btnProcess.Visible = false;

        //    }
        //    catch (Exception)
        //    {
                
        //        throw;
        //    }
        //}

        private void fillCompanyWiseActualExpensesGrid()
        {
            log.Debug("fillCompanyWiseActualExpensesGrid");
            DataTable dtCompanyWiseExpenses = new DataTable();
            try
            {
                if (Session["dtCompanyWiseExpenses"] != null)
                {
                    dtCompanyWiseExpenses = (Session["dtCompanyWiseExpenses"] as DataTable).Copy();
                    gvCompanyWiseActualExpenses.DataSource = dtCompanyWiseExpenses;
                    gvCompanyWiseActualExpenses.DataBind();
                }
                else 
                {
                    gvCompanyWiseActualExpenses.DataSource = null;
                    gvCompanyWiseActualExpenses.DataBind();
                }
                
                
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
            }
            finally
            {
                dtCompanyWiseExpenses.Dispose();
            }
        }

        private void addCompanyExpensesToSession(DataTable dtCompanyWiseExpenses)
        {
            log.Debug("addCompanyExpensesToSession");
            try
            {
                if (Session["dtCompanyWiseExpenses"] != null)
                {
                    Session.Remove("dtCompanyWiseExpenses");
                }

                Session["dtCompanyWiseExpenses"] = dtCompanyWiseExpenses;

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
            }
            finally
            {

                dtCompanyWiseExpenses.Dispose();
            }
        }

        private DataTable fillCompanyWiseActualExpensesDataTable(DataTable dtCompanyWiseParticipants)
        {
            log.Debug("fillCompanyWiseActualExpensesGridView");
            ActualExpencesDataHandler actualExpencesDataHandler = new ActualExpencesDataHandler();
            try
            {
                

                decimal netAmount = Convert.ToDecimal(txtNetAmount.Text.ToString());

                decimal totalParticipantCount = 0;
                foreach (DataRow company in dtCompanyWiseParticipants.Rows)
                {
                    totalParticipantCount += Convert.ToDecimal(company["PLANNED_PARTICIPANTS"].ToString());
                }

                dtCompanyWiseParticipants.Columns.Add("AMOUNT", typeof(Decimal));
                dtCompanyWiseParticipants.Columns.Add("REMARKS");
                foreach (DataRow company in dtCompanyWiseParticipants.Rows)
                {
                    //decimal amount = (netAmount / Convert.ToDecimal(company["SUM"].ToString())) * Convert.ToDecimal(company["PLANNED_PARTICIPANTS"].ToString());
                    company["AMOUNT"] = (netAmount / totalParticipantCount) * Convert.ToDecimal(company["PLANNED_PARTICIPANTS"].ToString());
                }

                return dtCompanyWiseParticipants;
                
            }
            catch (Exception ex)
            {
                
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
                throw;
            }
            finally
            {
                dtCompanyWiseParticipants.Dispose();
                actualExpencesDataHandler = null;
            }
        }

        private void clearDetailExpensesInputFields()
        {
            log.Debug("clearDetailExpensesInputFields");

            try
            {
                //Utility.Errorhandler.ClearError(lblDetailMessage);
                Utility.Utils.clearControls(true, ddlcategory, ddlStatus, txtDescription, txtDiscount, txtNetAmount, txtCost, txtRemark, chkPaid, txtExpencePaymentDescription);
                btnDetailAdd.Text = details_update_text;
                Utility.Errorhandler.ClearError(lblDetailMessage);
                hfRecordId.Value = "";
            }
            catch (Exception ex)
            {

                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblDetailMessage);
            }
        }

        private void clearCompanyWiseExpenses()
        {
            log.Debug("clearCompanyWiseExpenses");
            try
            {
                gvCompanyWiseActualExpenses.DataSource = null;
                gvCompanyWiseActualExpenses.DataBind();

                Session["dtCompanyWiseExpenses"] = null;
                Utility.Errorhandler.ClearError(lblErrorMsgCompany);

                hfRecordId.Value = "";
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblDetailMessage);
            }
        }

        private void fillDetailExpense(DataTable dtCategoryExpense)
        {
            log.Debug("fillDetailExpense()");
            try
            {
                DataRow expense = dtCategoryExpense.Rows[0];
                ddlcategory.SelectedValue = expense["EXPENSE_CATEGORY_ID"].ToString();
                txtDescription.Text = expense["DESCRIPTION"].ToString();
                txtRemark.Text = expense["REMARKS"].ToString();
                txtCost.Text = expense["AMOUNT"].ToString();
                txtDiscount.Text = expense["DISCOUNT"].ToString();
                txtNetAmount.Text = expense["NET_AMOUNT"].ToString();
                ddlStatus.SelectedValue = expense["STATUS_CODE"].ToString();
                if (expense["IS_PAID"].ToString() == Constants.STATUS_ACTIVE_VALUE)
                {
                    chkPaid.Checked = true;
                }
                else if (expense["IS_PAID"].ToString() == Constants.STATUS_INACTIVE_VALUE)
                {
                    chkPaid.Checked = false;
                }
                txtExpencePaymentDescription.Text = expense["PAYMENT_DESCRIPTION"].ToString();

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblDetailMessage);
            }
        }

        private void clearExpenseMasterDetails()
        {
            log.Debug("fillDetailExpense()");
            try
            {
                Utility.Utils.clearControls(false, txtTraining, lblTrainingName, lblProgramName, txtTrPerPersonCost, txtTrTotal, txtTotalDiscount, txtGrandTotal, txtTrDescription);
                Utility.Errorhandler.ClearError(lblErrorMsg);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        #endregion

        #region events

        protected void btnDetailAdd_Click(object sender, EventArgs e)
        {
            log.Debug("btnDetailAdd_Click");
            ActualExpencesDataHandler actualExpencesDataHandler = new ActualExpencesDataHandler();
            DataTable dtCompanyExpenses = new DataTable();

            try
            {
                Utility.Errorhandler.ClearError(lblDetailMessage);

                string addedUser = Session["KeyUSER_ID"].ToString();

                /// Expence category Detail ///
                string expCategory = ddlcategory.SelectedValue;
                string expCategoryName = ddlcategory.SelectedItem.Text;
                string description = txtDescription.Text;
                string remark = txtRemark.Text;
                string isPaid = isPaid_false_text;
                if (chkPaid.Checked == true)
                {
                    isPaid = isPaid_true_text;
                }

                string paymentDescription = txtExpencePaymentDescription.Text.ToString();

                decimal amount = 0;
                decimal discount = 0;
                decimal netAmount = 0;

                if (!String.IsNullOrEmpty(txtCost.Text.ToString()))
                {
                    amount = Convert.ToDecimal(txtCost.Text.ToString());
                }
                if (!String.IsNullOrEmpty(txtDiscount.Text.ToString()))
                {
                    discount = Convert.ToDecimal(txtDiscount.Text.ToString());
                }
                if (!String.IsNullOrEmpty(txtNetAmount.Text.ToString()))
                {
                    netAmount = Convert.ToDecimal(txtNetAmount.Text.ToString());
                }
                
                /// Master Details ///
                string traingId = txtTraining.Text;
                string trDescription = txtTrDescription.Text;
                //string trStatus = ddlTrStatus.SelectedValue;

                string trStatus = Constants.STATUS_ACTIVE_VALUE;
                string expenseId = hfExpenseId.Value.ToString();

                decimal totalDiscount = 0;
                decimal grandTotal = 0;
                decimal total = 0;
                decimal perPerson = 0;

                if (String.IsNullOrEmpty(txtTotalDiscount.Text.ToString()))
                {
                    totalDiscount = discount;
                }
                else 
                {
                    totalDiscount = Convert.ToDecimal(txtTotalDiscount.Text.ToString());
                }

                if (String.IsNullOrEmpty(txtGrandTotal.Text.ToString()))
                {
                    grandTotal = netAmount;
                }
                else
                {
                    grandTotal = Convert.ToDecimal(txtGrandTotal.Text.ToString());
                }

                if (String.IsNullOrEmpty(txtTrTotal.Text.ToString()))
                {
                    total = amount;
                }
                else
                {
                    total = Convert.ToDecimal(txtTrTotal.Text.ToString());
                }


                ///Company Details
                ///

                decimal companyTotal = 0;
                for (int i = 0; i < gvCompanyWiseActualExpenses.Columns.Count; i++)
                {
                    dtCompanyExpenses.Columns.Add("column" + i.ToString());
                }
                foreach (GridViewRow companyExpense in gvCompanyWiseActualExpenses.Rows)
                {
                    DataRow expense = dtCompanyExpenses.NewRow();
                   
                    expense["column0"] = companyExpense.Cells[0].Text;
                    expense["column1"] = companyExpense.Cells[1].Text;
                    expense["column2"] = companyExpense.Cells[2].Text;

                    TextBox remarks =  (companyExpense.Cells[3].FindControl("txtcompanyRemark") as TextBox);
                    expense["column3"] = remarks.Text;

                    TextBox compAmount = (companyExpense.Cells[3].FindControl("lblcmpAmount") as TextBox);
                    if (!String.IsNullOrEmpty(compAmount.Text.ToString()))
                    {
                        expense["column4"] = compAmount.Text.ToString();
                    }
                    else
                    {
                        expense["column4"] = "0.00";
                    }

                    companyTotal += Convert.ToDecimal(compAmount.Text.ToString());

                    dtCompanyExpenses.Rows.Add(expense);
                }

                

                string status = ddlStatus.SelectedValue;

                bool masterExpenseExist = actualExpencesDataHandler.checkExpenseExistance(traingId);
                string recordId = hfRecordId.Value.ToString();
                if (String.IsNullOrEmpty(recordId )) /// add button funtionality
                {

                    if (masterExpenseExist) /// update header table and insert new record to detail table
                    {
                        if (!String.IsNullOrEmpty(status) && amount != 0 && netAmount != 0) //// detail expense input fields aren't blank
                        {
                            if (companyTotal != netAmount)
                            {
                                if (companyTotal == 0)
                                {
                                    CommonVariables.MESSAGE_TEXT = "Expense must be processed.";
                                }
                                else
                                {
                                    CommonVariables.MESSAGE_TEXT = "Incorrect Company total. Please check or process again.";
                                }
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
                                return;
                            }

                            if (status == Constants.STATUS_ACTIVE_VALUE)
                            {
                                grandTotal += netAmount;
                                totalDiscount += discount;
                                total += amount;

                                int participantCount = actualExpencesDataHandler.getTrainingParticipantCount(traingId);
                                perPerson = grandTotal / participantCount;
                            }

                            decimal actualNetAmount = amount - discount;
                            if (actualNetAmount != netAmount)
                            {
                                CommonVariables.MESSAGE_TEXT = "Net Amount is incorrect";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
                                return;
                            }

                            bool isSaved = actualExpencesDataHandler.updateActualExpense_InsertDetails_InsertCompany(traingId,
                                expenseId,
                                total,
                                trDescription,
                                totalDiscount,
                                grandTotal,
                                perPerson,
                                trStatus,
                                expCategory,
                                description,
                                amount,
                                discount,
                                netAmount,
                                isPaid,
                                paymentDescription,
                                remark,
                                status,
                                addedUser,
                                dtCompanyExpenses);
                        }
                        else //// detail expense fields are blank
                        {
                            bool isUpdated = actualExpencesDataHandler.updateActualExpense(traingId, expenseId, trDescription, trStatus, addedUser);
                        }


                    }
                    else /// insert new records to both header and detail tables
                    {
                        if (!String.IsNullOrEmpty(status) && amount != 0 && netAmount != 0) //// detail expense input fields aren't blank
                        {
                            int participantCount = actualExpencesDataHandler.getTrainingParticipantCount(traingId);
                            perPerson = grandTotal / participantCount;

                            decimal actualNetAmount = amount - discount;
                            if (actualNetAmount != netAmount)
                            {
                                CommonVariables.MESSAGE_TEXT = "Net Amount is incorrect";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
                                return;
                            }
                            bool isSaved = actualExpencesDataHandler.insertActualExpense_InsertDetails_InsertCompany(traingId,
                               total,
                               trDescription,
                               totalDiscount,
                               grandTotal,
                               perPerson,
                               trStatus,
                               expCategory,
                               description,
                               amount,
                               discount,
                               netAmount,
                               isPaid,
                               paymentDescription,
                               remark,
                               status,
                               addedUser,
                               dtCompanyExpenses);
                        }
                        else //// detail expense fields are blank
                        {
                            bool isUpdated = actualExpencesDataHandler.updateActualExpense(traingId, expenseId, trDescription, trStatus, addedUser);
                        }
                    }



                    populateActualExpense(traingId);
                    clearDetailExpensesInputFields();
                    clearCompanyWiseExpenses();
                }
                else if (!String.IsNullOrEmpty(recordId))
                {
                    if (masterExpenseExist)
                    {

                        //string recordId = hfRecordId.Value.ToString();
                        DataTable previouseDetailForCategory = actualExpencesDataHandler.getCategoryDetail(recordId);
                        DataRow prevDetails = null;                        

                        if (previouseDetailForCategory.Rows.Count == 1)
                        {
                            prevDetails = previouseDetailForCategory.Rows[0];
                        }
                        if (status == Constants.STATUS_INACTIVE_VALUE)
                        {
                            if (prevDetails[8].ToString() == Constants.STATUS_ACTIVE_VALUE)
                            {
                                grandTotal -= Convert.ToDecimal(prevDetails[3].ToString());
                                totalDiscount -= Convert.ToDecimal(prevDetails[4].ToString());
                                total -= Convert.ToDecimal(prevDetails[2].ToString());

                                int participantCount = actualExpencesDataHandler.getTrainingParticipantCount(traingId);
                                perPerson = grandTotal / participantCount; 
                            }
                            dtCompanyExpenses = null;

                        }
                        if (status == Constants.STATUS_ACTIVE_VALUE)
                        {
                            if (companyTotal != netAmount)
                            {
                                CommonVariables.MESSAGE_TEXT = "Incorrect Company total. Please check or process again.";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
                                return;
                            }

                            if (prevDetails[8].ToString() == Constants.STATUS_ACTIVE_VALUE)
                            {
                                grandTotal -= Convert.ToDecimal(prevDetails[3].ToString());
                                totalDiscount -= Convert.ToDecimal(prevDetails[4].ToString());
                                total -= Convert.ToDecimal(prevDetails[2].ToString());
                            }

                            grandTotal += netAmount;
                            totalDiscount += discount;
                            total += amount;

                            int participantCount = actualExpencesDataHandler.getTrainingParticipantCount(traingId);
                            perPerson = grandTotal / participantCount;

                            
                        }

                        decimal actualNetAmount = amount - discount;
                        if (actualNetAmount != netAmount)
                        {
                            CommonVariables.MESSAGE_TEXT = "Net Amount is incorrect";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
                            return;
                        }

                        bool isUpdated = actualExpencesDataHandler.updateActualExpense_UpdatetDetails_UpdateCompany(traingId,
                            expenseId,
                            total,
                            trDescription,
                            totalDiscount,
                            grandTotal,
                            perPerson,
                            trStatus,
                            expCategory,
                            description,
                            amount,
                            discount,
                            netAmount,
                            isPaid,
                            paymentDescription,
                            remark,
                            status,
                            addedUser,
                            dtCompanyExpenses, recordId);


                        
                    }
                    populateActualExpense(traingId);
                    clearDetailExpensesInputFields();
                    clearCompanyWiseExpenses();

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                actualExpencesDataHandler = null;
            }
        }
     
        protected void btnProcess_Click(object sender, EventArgs e)
        {
            log.Debug("btnProcess_Click");

            try
            {
                Utility.Errorhandler.ClearError(lblErrorMsgCompany);

                decimal amount = 0;
                decimal discount = 0;
                decimal netAmount = 0;

                if (!String.IsNullOrEmpty(txtCost.Text.ToString()))
                {
                    amount = Convert.ToDecimal(txtCost.Text.ToString());
                }
                if (!String.IsNullOrEmpty(txtDiscount.Text.ToString()))
                {
                    discount = Convert.ToDecimal(txtDiscount.Text.ToString());
                }
                if (!String.IsNullOrEmpty(txtNetAmount.Text.ToString()))
                {
                    netAmount = Convert.ToDecimal(txtNetAmount.Text.ToString());
                }

                decimal actualNetAmount = amount - discount;
                if (actualNetAmount != netAmount)
                {
                    CommonVariables.MESSAGE_TEXT = "Net Amount is incorrect";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
                    return;
                }

                if (netAmount == 0)
                {
                    CommonVariables.MESSAGE_TEXT = "Net Amount should be greater than 0";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
                    return;
                }
                if (amount == 0)
                {
                    CommonVariables.MESSAGE_TEXT = "Amount should be greater than 0";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
                    return;
                }

                processDetailedExpence();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
            }
        }

        protected void gvCompanyWiseActualExpenses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvCompanyWiseActualExpenses_RowDataBound");

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataTable dtCompanyWiseExpenses = (Session["dtCompanyWiseExpenses"] as DataTable).Copy();

                    TextBox companyAmount = e.Row.FindControl("lblcmpAmount") as TextBox;
                    companyAmount.Text = dtCompanyWiseExpenses.Rows[e.Row.RowIndex]["AMOUNT"].ToString();

                    TextBox remarks = e.Row.FindControl("txtcompanyRemark") as TextBox;
                    remarks.Text = dtCompanyWiseExpenses.Rows[e.Row.RowIndex]["REMARKS"].ToString();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsgCompany);
            }
        }

        protected void gvCategoryDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvCategoryDetails_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvCategoryDetails, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblDetailMessage);
            }
        }

        protected void gvCategoryDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gvCategoryDetails_SelectedIndexChanged()");

            ActualExpencesDataHandler actualExpencesDataHandler = new ActualExpencesDataHandler();
            DataTable dtCategoryExpense = new DataTable();
            try
            {
                clearDetailExpensesInputFields();
                clearCompanyWiseExpenses();

                //btnExpenseUpdate.Visible = false;

                int index = gvCategoryDetails.SelectedIndex;
                string recordId = gvCategoryDetails.Rows[index].Cells[0].Text.ToString();
                string trainingId = txtTraining.Text.ToString();

                hfRecordId.Value = recordId;

                dtCategoryExpense = actualExpencesDataHandler.getCategoryDetail(recordId);
                if (dtCategoryExpense.Rows.Count == 1)
                {
                    fillDetailExpense(dtCategoryExpense);

                    DataTable dtCompanyWiseParticipants = actualExpencesDataHandler.getCompanyWiseParticipantCount(trainingId);
                    DataTable dtCompanyWiseExpense = actualExpencesDataHandler.getCompanyWiseExpense(recordId, trainingId);
                    addCompanyExpensesToSession(dtCompanyWiseExpense);
                    fillCompanyWiseActualExpensesGrid();

                    btnDetailAdd.Text = details_update_text;

                    dtCompanyWiseExpense.Dispose();
                    dtCompanyWiseParticipants.Dispose();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblDetailMessage);
            }
            finally
            {
                dtCategoryExpense.Dispose();
                actualExpencesDataHandler = null;
            }
        }

        protected void btnExpenseUpdate_Click(object sender, EventArgs e)
        {
            log.Debug("btnExpenseUpdate_Click()");

            ActualExpencesDataHandler actualExpencesDataHandler = new ActualExpencesDataHandler();
            try
            {
                string trainingId = txtTraining.Text.ToString();
                string expenseId = hfExpenseId.Value.ToString();
                string masterStatus = ddlTrStatus.SelectedValue.ToString();
                string masterDescription = txtTrDescription.Text.ToString();

                string addedUser = Session["KeyUSER_ID"].ToString();

                bool isUpdated = actualExpencesDataHandler.updateActualExpense(trainingId, expenseId, masterDescription, masterStatus, addedUser);

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                actualExpencesDataHandler = null;
            }
        }

        protected void btnDetailClear_Click(object sender, EventArgs e)
        {
            try
            {
                clearDetailExpensesInputFields();
                clearCompanyWiseExpenses();
                //btnExpenseUpdate.Visible = true;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblDetailMessage);
            }
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("btnClearAll_Click");

                clearExpenseMasterDetails();
                clearDetailExpensesInputFields();
                clearCompanyWiseExpenses();
                populateDetailedExpenses(null);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblDetailMessage);
            }
        }

        #endregion

        

        

        

        

        

        





    }
}