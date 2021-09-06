using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.TrainingAndDevelopment;
using System.Data;
using NLog;
using Common;
using DataHandler;
using DataHandler.Utility;
using GroupHRIS.Utility;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainingInstitutes : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        
        #region events

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmTrainingCategory : Page_Load");

            if (Session["KeyLOGOUT_STS"].Equals("0"))
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }

            if (!IsPostBack)
            {
                fillBankDropdown();
                fillStatus();
                populateInstituteGrid();
                tblSelectedInstitute.Visible = false;

                if (Page.PreviousPage != null)
                {
                    string instituteId = "";
                    instituteId = ((HiddenField)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$hfInstituteId")).Value.Trim();
                    fillInstituteDataForUpdate(instituteId);
                }
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");

            clearFromControls();
            hfInstituteId.Value = "";
            Errorhandler.ClearError(lblErrorMsg);
            Utils.clearControls(false, txtInstituteId, txtInstituteName);
            tblSelectedInstitute.Visible = false;
            hfInstituteId.Value = "";
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");
            try
            {
                string name = txtName.Text.ToString();
                string address = txtAddress.Text.ToString();
                string contact_1 = txtContact_1.Text.ToString();
                string contact_2 = txtContact_2.Text.ToString();
                string email = txtEmail.Text.ToString();
                string bank = ddlBank.SelectedValue.ToString();
                string branch = ddlBranch.SelectedValue.ToString();
                string accountNo = txtAccount.Text.ToString();
                string paymentInstructions = txtPaymentInstruction.Text.ToString();
                string status = ddlStatus.SelectedValue.ToString();
                string addedUserId = Session["KeyUSER_ID"].ToString();


                UtilsDataHandler utilsDataHandler = new UtilsDataHandler();
                TrainingInstituteDataHandler trainingInstituteDataHandler = new TrainingInstituteDataHandler();

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    Boolean nameIsExsists = utilsDataHandler.isDuplicateExist(name, "INSTITUTE_NAME", "TRAINING_INSTITUTES");
                    if (nameIsExsists)
                    {
                        CommonVariables.MESSAGE_TEXT = "Institute name already exist";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                    else
                    {

                        bool isInserted = trainingInstituteDataHandler.Insert(name, address, contact_1, contact_2, email, bank, branch, accountNo, paymentInstructions, status, addedUserId);
                        if (isInserted)
                        {
                            CommonVariables.MESSAGE_TEXT = "Record(s) successfully saved.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            clearFromControls();
                            hfInstituteId.Value = "";
                            populateInstituteGrid();
                            Utils.clearControls(false, txtInstituteId, txtInstituteName);
                            tblSelectedInstitute.Visible = false;
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Record(s) couldn't save.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }

                    }

                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string instituteId = hfInstituteId.Value.ToString();

                    Boolean nameIsExsists = utilsDataHandler.isDuplicateExist(name, "INSTITUTE_NAME", "TRAINING_INSTITUTES", instituteId, "INSTITUTE_ID");
                    if (nameIsExsists)
                    {
                        CommonVariables.MESSAGE_TEXT = "Institute name already exist";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                    else
                    {

                        bool isInserted = trainingInstituteDataHandler.Update(instituteId,name, address, contact_1, contact_2, email, bank, branch, accountNo, paymentInstructions, status, addedUserId);
                        if (isInserted)
                        {
                            CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            clearFromControls();
                            hfInstituteId.Value = "";
                            populateInstituteGrid();
                            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                            Utils.clearControls(false, txtInstituteId, txtInstituteName);
                            tblSelectedInstitute.Visible = false;
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Record(s) couldn't update.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }

                    }


                    
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }
       
        protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlBank_SelectedIndexChanged()");
            ddlBranch.Items.Clear();
            if (ddlBank.SelectedValue.ToString() != "")
            {
                string bankId = ddlBank.SelectedValue.ToString();
                fillBranchDropdown(bankId);
            }
        }

        protected void gvInstitutes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvInstitutes_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvInstitutes, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void gvInstitutes_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gvInstitutes_SelectedIndexChanged()");
            try
            {
                string instituteId = gvInstitutes.SelectedRow.Cells[0].Text;
                fillInstituteDataForUpdate(instituteId);                
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void gvInstitutes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvInstitutes_PageIndexChanging()");
            try
            {
                gvInstitutes.PageIndex = e.NewPageIndex;
                populateInstituteGrid();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void fillInstituteDataForUpdate(string sInstituteId)
        {
            log.Debug("fillInstituteDataForUpdate()");
            TrainingInstituteDataHandler trainingInstituteDataHandler = new TrainingInstituteDataHandler();
            DataRow dataRow = null;

            try
            {
                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                Utility.Errorhandler.ClearError(lblErrorMsg);

                dataRow = trainingInstituteDataHandler.getInstituteById(sInstituteId);

                if (dataRow != null)
                {
                    hfInstituteId.Value = dataRow["INSTITUTE_ID"].ToString().Trim();
                    txtName.Text = dataRow["INSTITUTE_NAME"].ToString().Trim();
                    txtAddress.Text = dataRow["INSTITUTE_ADDRESS"].ToString().Trim();
                    txtContact_1.Text = dataRow["CONTACT_NO_1"].ToString().Trim();
                    txtContact_2.Text = dataRow["CONTACT_NO_2"].ToString().Trim();
                    txtEmail.Text = dataRow["EMAIL_ADDRESS"].ToString().Trim();

                    ddlBank.SelectedValue = dataRow["BANK_ID"].ToString().Trim();

                    fillBranchDropdown(dataRow["BANK_ID"].ToString().Trim());
                    ddlBranch.SelectedValue = dataRow["BANK_BRANCH_ID"].ToString().Trim();

                    txtAccount.Text = dataRow["ACCOUNT_NUMBER"].ToString().Trim();
                    txtPaymentInstruction.Text = dataRow["PAYMENT_INSTRUCTIONS"].ToString().Trim();

                    string status = dataRow["STATUS_CODE"].ToString().Trim();
                    ddlStatus.SelectedValue = status;


                    tblSelectedInstitute.Visible = true;
                    txtInstituteId.Text = dataRow["INSTITUTE_ID"].ToString().Trim();
                    txtInstituteName.Text = dataRow["INSTITUTE_NAME"].ToString().Trim();
                }

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally
            {
                trainingInstituteDataHandler = null;
                dataRow = null;
            }
        }

        protected void btnInstituteClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnInstituteClear_Click()");
            try
            {
                Utils.clearControls(false, txtInstituteId, txtInstituteName);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void btnAddPrograms_Click(object sender, EventArgs e)
        {
            log.Debug("btnAddPrograms_Click()");
            Server.Transfer("~/TrainingAndDevelopment/WebFrmTrainingInstitutePrograme.aspx");
        }

        #endregion

        #region methodes

        private void fillStatus()
        {
            ListItem listItemBlank = new ListItem();
            listItemBlank.Text = "";
            listItemBlank.Value = "";
            ddlStatus.Items.Add(listItemBlank);

            ListItem listItemActive = new ListItem();
            listItemActive.Text = Constants.STATUS_ACTIVE_TAG;
            listItemActive.Value = Constants.STATUS_ACTIVE_VALUE;
            ddlStatus.Items.Add(listItemActive);

            ListItem listItemInActive = new ListItem();
            listItemInActive.Text = Constants.STATUS_INACTIVE_TAG;
            listItemInActive.Value = Constants.STATUS_INACTIVE_VALUE;
            ddlStatus.Items.Add(listItemInActive);
        }

        private void fillBankDropdown()
        {
            DataTable dtBank = new DataTable();
            TrainingInstituteDataHandler trainingInstituteDataHandler = new TrainingInstituteDataHandler();
            try
            {
                dtBank = trainingInstituteDataHandler.getAllBanks();

                ListItem blankItem = new ListItem();
                blankItem.Value = "";
                blankItem.Text = "";
                ddlBank.Items.Add(blankItem);

                if (dtBank.Rows.Count > 0)
                {
                    foreach (DataRow bank in dtBank.Rows)
                    {
                        ListItem newItem = new ListItem();
                        newItem.Value = bank[0].ToString();
                        newItem.Text = bank[1].ToString();
                        ddlBank.Items.Add(newItem);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                dtBank.Dispose();
                trainingInstituteDataHandler = null;
            }
        }
     
        private void fillBranchDropdown(string bankId)
        {
            DataTable dtBranch = new DataTable();
            TrainingInstituteDataHandler trainingInstituteDataHandler = new TrainingInstituteDataHandler();
            try
            {
                
                dtBranch = trainingInstituteDataHandler.getBranchesForSelectedBank(bankId);

                ListItem blankItem = new ListItem();
                blankItem.Value = "";
                blankItem.Text = "";
                ddlBranch.Items.Add(blankItem);

                if (dtBranch.Rows.Count > 0)
                {
                    foreach (DataRow branch in dtBranch.Rows)
                    {
                        ListItem newItem = new ListItem();
                        newItem.Value = branch[0].ToString();
                        newItem.Text = branch[1].ToString();
                        ddlBranch.Items.Add(newItem);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                 dtBranch.Dispose();
                 trainingInstituteDataHandler = null;
            }
        }

        private void clearFromControls()
        {
            Utility.Utils.clearControls(true, txtName,txtAddress,txtContact_1,txtContact_2,txtEmail,txtAccount,txtPaymentInstruction,ddlBank, ddlBranch, ddlStatus);
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
        }

        private void populateInstituteGrid()
        {
            TrainingInstituteDataHandler trainingInstituteDataHandler = new TrainingInstituteDataHandler();
            DataTable dtInstitutes = new DataTable();

            try
            {
                dtInstitutes = trainingInstituteDataHandler.getAllInstitutes();
                if (dtInstitutes.Rows.Count > 0)
                {
                    gvInstitutes.DataSource = dtInstitutes;
                    gvInstitutes.DataBind();
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                dtInstitutes.Dispose();
                trainingInstituteDataHandler = null;
            }
        }
       
        #endregion

        

        

        



        

        



        


    }
}