using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using System.Data;
using DataHandler.TrainingAndDevelopment;
using Common;
using GroupHRIS.Utility;
using DataHandler.Utility;
using System.IO;
using System.Drawing;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainerInformation : System.Web.UI.Page
    {
        private string addCompetencyText = "Add";
        private string updateCompetencyText = "Update";

        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmTrainerInformation : Page_Load");

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
                fillStatus();
                fillBankDropdown();
                fillTrainingNatureDropdown();
                fillTrainingNatureFilterDropdown();
                fillInternalExternalDropdown();
                fillCompetenciesDropdown();
                fillCompetencyStatus();
                initializeCompetencySession();
                fillTrainersGridView();
                
                imgRemoveImage.Visible = false;
            }

            //var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];
            //if (httpPostedFile != null)
            //{
            //    priviewImageAtBrows();
            //    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles"), httpPostedFile.FileName);

            //    // Save the uploaded file to "UploadedFiles" folder
            //    httpPostedFile.SaveAs(fileSavePath);
            //}
            if (HiddenDataCaptured.Value.ToString() == "1")
            {
                priviewImageAtBrows();
            }           
        }

        

        #region methodes

        private void fillStatus()
        {
            try
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
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void fillBankDropdown()
        {
            log.Debug("fillBankDropdown()");

            DataTable dtBank = new DataTable();
            TrainerInformationDataHandler trainerInformationDataHandler = new TrainerInformationDataHandler();
            try
            {
                dtBank = trainerInformationDataHandler.getAllBanks();

                ddlBank.Items.Clear();

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
                trainerInformationDataHandler = null;
            }
        }

        private void fillBranchDropdown(string bankId)
        {
            log.Debug("fillBranchDropdown()");
            DataTable dtBranch = new DataTable();
            TrainerInformationDataHandler trainerInformationDataHandler = new TrainerInformationDataHandler();
            try
            {

                dtBranch = trainerInformationDataHandler.getBranchesForSelectedBank(bankId);

                ddlBranch.Items.Clear();

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
                trainerInformationDataHandler = null;
            }
        }

        private void fillTrainingNatureDropdown()
        {
            log.Debug("fillTrainingNatureDropdown()");
            DataTable dtNature = new DataTable();
            TrainerInformationDataHandler trainerInformationDataHandler = new TrainerInformationDataHandler();
            try
            {

                dtNature = trainerInformationDataHandler.getTrainingNature();

                ListItem blankItem = new ListItem();
                blankItem.Value = "";
                blankItem.Text = "";
                ddlNature.Items.Add(blankItem);

                if (dtNature.Rows.Count > 0)
                {
                    foreach (DataRow nature in dtNature.Rows)
                    {
                        ListItem newItem = new ListItem();
                        newItem.Value = nature[0].ToString();
                        newItem.Text = nature[1].ToString();
                        ddlNature.Items.Add(newItem);
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
                dtNature.Dispose();
                trainerInformationDataHandler = null;
            }
        }

        private void fillTrainingNatureFilterDropdown()
        {
            log.Debug("fillTrainingNatureFilterDropdown()");
            DataTable dtNature = new DataTable();
            TrainerInformationDataHandler trainerInformationDataHandler = new TrainerInformationDataHandler();
            try
            {

                dtNature = trainerInformationDataHandler.getAllTrainingNature();

                ListItem blankItem = new ListItem();
                blankItem.Value = "";
                blankItem.Text = "";
                ddlTrainingNatureFilter.Items.Add(blankItem);

                if (dtNature.Rows.Count > 0)
                {
                    foreach (DataRow nature in dtNature.Rows)
                    {
                        ListItem newItem = new ListItem();
                        newItem.Value = nature[0].ToString();
                        newItem.Text = nature[1].ToString();
                        ddlTrainingNatureFilter.Items.Add(newItem);
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
                dtNature.Dispose();
                trainerInformationDataHandler = null;
            }
        }

        private void fillInternalExternalDropdown()
        {
            log.Debug("fillInternalExternalDropdown()");
            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlInternalExternal.Items.Add(listItemBlank);

                ListItem listItemInternal = new ListItem();
                listItemInternal.Text = Constants.TRAINER_INTERNAL_TAG;
                listItemInternal.Value = Constants.TRAINER_INTERNAL_VALUE;
                ddlInternalExternal.Items.Add(listItemInternal);

                ListItem listItemExternal = new ListItem();
                listItemExternal.Text = Constants.TRAINER_EXTERNAL_TAG;
                listItemExternal.Value = Constants.TRAINER_EXTERNAL_VALUE;
                ddlInternalExternal.Items.Add(listItemExternal);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void fillCompetenciesDropdown()
        {
            log.Debug("fillCompetenciesDropdown()");
            DataTable dtCompetencies = new DataTable();
            TrainerInformationDataHandler trainerInformationDataHandler = new TrainerInformationDataHandler();
            try
            {

                dtCompetencies = trainerInformationDataHandler.getCompetencies();

                ListItem blankItem = new ListItem();
                blankItem.Value = "";
                blankItem.Text = "";
                ddlCompetency.Items.Add(blankItem);

                if (dtCompetencies.Rows.Count > 0)
                {
                    foreach (DataRow competency in dtCompetencies.Rows)
                    {
                        ListItem newItem = new ListItem();
                        newItem.Value = competency[0].ToString();
                        newItem.Text = competency[1].ToString();
                        ddlCompetency.Items.Add(newItem);
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
                dtCompetencies.Dispose();
                trainerInformationDataHandler = null;
            }
        }

        private void fillCompetencyStatus()
        {
            log.Debug("fillCompetencyStatus()");
            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlCompetencyStatus.Items.Add(listItemBlank);

                ListItem listItemActive = new ListItem();
                listItemActive.Text = Constants.STATUS_ACTIVE_TAG;
                listItemActive.Value = Constants.STATUS_ACTIVE_VALUE;
                ddlCompetencyStatus.Items.Add(listItemActive);

                ListItem listItemInActive = new ListItem();
                listItemInActive.Text = Constants.STATUS_INACTIVE_TAG;
                listItemInActive.Value = Constants.STATUS_INACTIVE_VALUE;
                ddlCompetencyStatus.Items.Add(listItemInActive);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void initializeCompetencySession()
        {
            log.Debug("initializeCompetencySession()");
            DataTable dtCompetencies = new DataTable();
            try
            {

                dtCompetencies.Columns.Add("COMPETENCY_ID", typeof(string));
                dtCompetencies.Columns.Add("NAME", typeof(string));
                dtCompetencies.Columns.Add("DESCRIPTION", typeof(string));
                dtCompetencies.Columns.Add("STATUS_CODE", typeof(string));
                dtCompetencies.Columns.Add("STATUS_TEXT", typeof(string));

                Session["dtCompetencies"] = dtCompetencies;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                dtCompetencies.Dispose();
            }
        }

        private void fillCompetencyGridView()
        {
            log.Debug("fillCompetencyGridView()");
            DataTable dtCompetencies = new DataTable();
            try
            {
                if (Session["dtCompetencies"] != null)
                {
                    dtCompetencies = (Session["dtCompetencies"] as DataTable).Copy();
                    if (dtCompetencies.Rows.Count > 0)
                    {
                        gvCompetencies.DataSource = dtCompetencies;
                        gvCompetencies.DataBind();
                    }
                    else
                    {
                        gvCompetencies.DataSource = null;
                        gvCompetencies.DataBind();
                    }
                }
                else
                {
                    gvCompetencies.DataSource = null;
                    gvCompetencies.DataBind();
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                dtCompetencies.Dispose();
            }

        }

        private void clearCompetencyInputFields()
        {
            log.Debug("clearCompetencyInputFields()");
            try
            {
                Utility.Utils.clearControls(true, ddlCompetency, ddlCompetencyStatus,txtCompetencyDesc);
                hfCompetencyId.Value = "";
                btnAddComp.Text = addCompetencyText;
                ddlCompetency.Enabled = true;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
        }

        private void fillTrainersGridView()
        {
            log.Debug("fillTrainersGridView()");

            DataTable dtTrainers = new DataTable();
            TrainerInformationDataHandler trainerInformationDataHandler = new TrainerInformationDataHandler();
            try
            {
                dtTrainers = trainerInformationDataHandler.getAllTrainers();

                string selectedNature = ddlTrainingNatureFilter.SelectedValue.ToString();
                if (!String.IsNullOrEmpty(selectedNature))
                {
                    DataView filteredDv = new DataView(dtTrainers);
                    filteredDv.RowFilter = "TRAINING_NATURE_ID like '" + selectedNature + "%'";
                    dtTrainers = filteredDv.ToTable();
                }
                gvTrainers.DataSource = dtTrainers;
                gvTrainers.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                trainerInformationDataHandler = null;
                dtTrainers.Dispose();
            }
        }

        private void fillTrainerDetails(DataTable dtTrainerDetails)
        {
            log.Debug("fillTrainerDetails()");
            try
            {
                if (dtTrainerDetails.Rows.Count > 0)
                {
                    ddlNature.Items.Clear();
                    fillTrainingNatureDropdown();

                    hfTrainerId.Value = dtTrainerDetails.Rows[0]["TRAINER_ID"].ToString();
                    txtName.Text = dtTrainerDetails.Rows[0]["NAME_WITH_INITIALS"].ToString();
                    txtFullName.Text = dtTrainerDetails.Rows[0]["FULL_NAME"].ToString();
                    //fuPhoto.FileName = dtTrainerDetails.Rows[0][""].ToString();
                    txtNIC.Text = dtTrainerDetails.Rows[0]["NIC"].ToString();
                    txtLandline.Text = dtTrainerDetails.Rows[0]["CONTACT_LAND"].ToString();
                    txtMobile.Text = dtTrainerDetails.Rows[0]["CONTACT_MOBILE"].ToString();
                    txtAddress.Text = dtTrainerDetails.Rows[0]["ADDRESS"].ToString();
                    ddlBank.SelectedValue = dtTrainerDetails.Rows[0]["BANK_ID"].ToString();

                    fillBranchDropdown(dtTrainerDetails.Rows[0]["BANK_ID"].ToString());
                    ddlBranch.SelectedValue = dtTrainerDetails.Rows[0]["BANK_BRANCH_ID"].ToString();

                    txtAccount.Text = dtTrainerDetails.Rows[0]["ACCOUNT_NUMBER"].ToString();
                    txtPaymentInstruction.Text = dtTrainerDetails.Rows[0]["PAYMENT_INSTRUCTIONS"].ToString();
                    txtDescription.Text = dtTrainerDetails.Rows[0]["DESCRIPTION"].ToString();
                    txtQualifications.Text = dtTrainerDetails.Rows[0]["QUALIFICATIONS"].ToString();
                    ddlInternalExternal.SelectedValue = dtTrainerDetails.Rows[0]["IS_EXTERNAL"].ToString();
                    ddlStatus.SelectedValue = dtTrainerDetails.Rows[0]["STATUS_CODE"].ToString();

                    bool natureIsExist = Utils.isValueExistInDropDownList(dtTrainerDetails.Rows[0]["TRAINING_NATURE_ID"].ToString(), ddlNature);
                    if (natureIsExist)
                    {
                        ddlNature.SelectedValue = dtTrainerDetails.Rows[0]["TRAINING_NATURE_ID"].ToString();
                    }
                    else
                    { 
                        ddlNature.Items.Add( new ListItem(dtTrainerDetails.Rows[0]["NATURE_NAME"].ToString(),dtTrainerDetails.Rows[0]["TRAINING_NATURE_ID"].ToString()));
                        ddlNature.SelectedValue = dtTrainerDetails.Rows[0]["TRAINING_NATURE_ID"].ToString();
                    }

                    if (!String.IsNullOrEmpty(dtTrainerDetails.Rows[0]["PHOTO"].ToString()))
                    {
                        HiddenDataCaptured.Value = "0";
                        byte[] photo = (byte[])dtTrainerDetails.Rows[0]["PHOTO"];
                        if (photo != null)
                        {
                            displayTrainerPhoto(photo);
                        }
                    }
                    else
                    {
                        HiddenDataCaptured.Value = "0";
                    }

                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }
        
        private void clearTrainerFields()
        {
            log.Debug("clearTrainerFields()");
            try
            {
                Utility.Utils.clearControls(true, ddlBank, ddlBranch, ddlStatus, ddlNature, ddlInternalExternal, txtAccount,
                                                    txtAddress, txtDescription, txtFullName, txtLandline, txtMobile,
                                                    txtName, txtNIC, txtPaymentInstruction, txtQualifications);
                hfTrainerId.Value = "";
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                Session.Remove("dtCompetencies");
                Session.Remove("byteArray");
                HiddenDataCaptured.Value = "0";
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }
        
        private void fillCompetencies(DataTable dtAvailableCompetencies)
        {
            log.Debug("fillCompetencies()");
            DataTable dtCompetencies = new DataTable();
            try
            {
                if (Session["dtCompetencies"] == null)
                {
                    initializeCompetencySession();
                }
                //dtCompetencies = (Session["dtCompetencies"] as DataTable).Copy();
                Session["dtCompetencies"] = dtAvailableCompetencies;
                fillCompetencyGridView();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                dtCompetencies.Dispose();
            }
        }

        private void priviewImageAtBrows()
        {
            try
            {
                //Stream fs;
                //BinaryReader br;
                Byte[] bytes = null;

                //if (fuPhoto.HasFile)
                //{
                //    Errorhandler.ClearError(lblErrorMsgPhoto);

                //    string filePath = fuPhoto.PostedFile.FileName;
                //    //hfImageFile.Value = filePath;
                //    string filename = Path.GetFileName(filePath);
                //    string ext = Path.GetExtension(filename);
                //    string contenttype = String.Empty;
                //    int fileSize = fuPhoto.PostedFile.ContentLength;
                //    if (fileSize > 500000)
                //    {
                //        CommonVariables.MESSAGE_TEXT = "File size exceeds the limit";
                //        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsgPhoto);
                //        return;
                //    }

                //    fs = fuPhoto.PostedFile.InputStream;
                //    br = new BinaryReader(fs);
                //    bytes = br.ReadBytes((Int32)fs.Length);
                //    Session["byteArray"] = bytes;
                //}
                //else
                //{
                //    Array.Clear(bytes, 0, bytes.Length);
                //}
                bytes = (Session["byteArray"] as Byte[]);
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                imgPhoto.ImageUrl = "data:image/jpg;base64," + base64String;

                uploaderRow.Visible = true;
                
                imgRemoveImage.Visible = true;

                //Session["uploadedFile"] = fuPhoto.PostedFile;


            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void removeImage()
        {
            try
            {
                uploaderRow.Visible = true;
                
                imgPhoto.ImageUrl = "../Images/Add_Person.png";
                imgRemoveImage.Visible = false;
                Session["byteArray"] = null;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            
        }

        private void displayTrainerPhoto(byte [] photo)
        {
            try
            {

                string base64String = Convert.ToBase64String(photo, 0, photo.Length);
                imgPhoto.ImageUrl = "data:image/jpg;base64," + base64String;

                uploaderRow.Visible = true;
                
                imgRemoveImage.Visible = true;


            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        #endregion

        #region event handlers

        protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlBank_SelectedIndexChanged()");
            try
            {
                ddlBranch.Items.Clear();

                string bankId = ddlBank.SelectedValue.ToString();
                fillBranchDropdown(bankId);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            TrainerInformationDataHandler trainerInformationDataHandler = new TrainerInformationDataHandler();
            UtilsDataHandler utilsDataHandler = new UtilsDataHandler();
            try
            {
                string nameInitial = txtName.Text.ToString().Trim();
                string nameFull = txtFullName.Text.ToString().Trim();
                //string imageUrl = fuPhoto.FileName.ToString().Trim();
                string nic = txtNIC.Text.ToString().Trim();
                string landline = txtLandline.Text.ToString().Trim();
                string mobile = txtMobile.Text.ToString().Trim();
                string address = txtAddress.Text.ToString().Trim();
                string bank = ddlBank.SelectedValue.ToString();
                string branch = ddlBranch.SelectedValue.ToString();
                string accountNo = txtAccount.Text.ToString().Trim();
                string payment = txtPaymentInstruction.Text.ToString().Trim();
                string description = txtDescription.Text.ToString().Trim();
                string qualifications = txtQualifications.Text.ToString().Trim();
                string nature = ddlNature.SelectedValue.ToString();
                string internalExternal = ddlInternalExternal.SelectedValue.ToString();
                string status = ddlStatus.SelectedValue.ToString();
                string addedUserId = Session["KeyUSER_ID"].ToString();

                Byte[] bytes = null;
                if (Session["byteArray"] != null)
                {
                    bytes = (Session["byteArray"] as Byte[]);
                }

                DataTable dtCompetencies = (Session["dtCompetencies"] as DataTable).Copy();
                DataRow[] activeCompetencies = dtCompetencies.Select("STATUS_CODE ='" + Constants.STATUS_ACTIVE_VALUE + "' ");
                if(activeCompetencies.Count() == 0)
                {
                    CommonVariables.MESSAGE_TEXT = "No active competencies selected";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    return;
                }

                
                if (btnSave.Text.ToString() == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    bool isNicExists = utilsDataHandler.isDuplicateExist(nic, "NIC", "TRAINER_INFROMATION");
                    if (isNicExists)
                    {
                        CommonVariables.MESSAGE_TEXT = "NIC already exist";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        return;
                    }
                    bool inserted = trainerInformationDataHandler.insertTrainer(nameInitial, nameFull, bytes, nic, internalExternal, landline, mobile, address, bank, branch, accountNo, payment, description, qualifications, nature, status, addedUserId, dtCompetencies);
                    if (inserted)
                    {
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_SAVED;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        clearCompetencyInputFields();
                        clearTrainerFields();
                        fillTrainersGridView();
                        fillCompetencyGridView();
                        removeImage();
                    }
                    else 
                    {
                        CommonVariables.MESSAGE_TEXT = " Trainer couldn't save";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }

                }
                else if (btnSave.Text.ToString() == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string selectedTrainerId = hfTrainerId.Value.ToString();

                    bool isNicExists = utilsDataHandler.isDuplicateExist(nic, "NIC", "TRAINER_INFROMATION", selectedTrainerId, "TRAINER_ID");
                    if (isNicExists)
                    {
                        CommonVariables.MESSAGE_TEXT = "NIC is already exist";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        return;
                    }
                    bool updated = trainerInformationDataHandler.updateTrainer(selectedTrainerId, nameInitial, nameFull, bytes, nic, internalExternal, landline, 
                                                                                mobile, address, bank, branch, accountNo, payment, description, qualifications, 
                                                                                nature, status, addedUserId, dtCompetencies);
                    if (updated)
                    {
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        clearCompetencyInputFields();
                        clearTrainerFields();
                        fillTrainersGridView();
                        removeImage();
                        fillCompetencyGridView();
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = " Trainer couldn't update";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
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
                trainerInformationDataHandler = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");
            try
            {
                clearCompetencyInputFields();
                clearTrainerFields();
                removeImage();
                Errorhandler.ClearError(lblErrorMsg);
                Errorhandler.ClearError(lblErrorMsg2);
                Errorhandler.ClearError(lblErrorMsgPhoto);
                fillCompetencyGridView();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void btnAddComp_Click(object sender, EventArgs e)
        {
            log.Debug("btnAddComp_Click()");
            DataTable dtCompetencies = new DataTable();
            try
            {
                Errorhandler.ClearError(lblErrorMsg2);
                string competencyId = ddlCompetency.SelectedValue.ToString();
                string name = ddlCompetency.SelectedItem.Text.ToString();
                string status = ddlCompetencyStatus.SelectedValue.ToString();
                string statusText = ddlCompetencyStatus.SelectedItem.Text.ToString();
                string description = txtCompetencyDesc.Text.ToString().Trim();
                if (btnAddComp.Text.ToString() == addCompetencyText)
                {
                    string selectedCompetency = competencyId;

                    if (Session["dtCompetencies"] == null)
                    {
                        initializeCompetencySession();
                    }
                    dtCompetencies = (Session["dtCompetencies"] as DataTable).Copy();
                    
                    DataRow[] selectedCompetencyRow = dtCompetencies.Select("COMPETENCY_ID = '" + selectedCompetency + "' ");


                    if (selectedCompetencyRow.Count() == 0)
                    {
                        DataRow newComp = dtCompetencies.NewRow();
                        newComp["COMPETENCY_ID"] = competencyId;
                        newComp["NAME"] = name;
                        newComp["DESCRIPTION"] = description;
                        newComp["STATUS_CODE"] = status;
                        newComp["STATUS_TEXT"] = statusText;

                        dtCompetencies.Rows.Add(newComp);

                        Session["dtCompetencies"] = dtCompetencies;
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "Competency alredy exist";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                    }
                    fillCompetencyGridView();

                    clearCompetencyInputFields();
                    dtCompetencies.Dispose();

                }
                else if (btnAddComp.Text.ToString() == updateCompetencyText)
                {
                    string selectedCompetency = hfCompetencyId.Value.ToString();

                    dtCompetencies = (Session["dtCompetencies"] as DataTable).Copy();
                    DataRow selectedCompetencyRow = dtCompetencies.Select("COMPETENCY_ID = '" + selectedCompetency + "' ").FirstOrDefault();

                    //selectedCompetencyRow["COMPETENCY_ID"] = competencyId;
                    selectedCompetencyRow["DESCRIPTION"] = description;
                    selectedCompetencyRow["STATUS_CODE"] = status;
                    selectedCompetencyRow["STATUS_TEXT"] = statusText;

                    Session["dtCompetencies"] = dtCompetencies;
                    fillCompetencyGridView();

                    clearCompetencyInputFields();
                    dtCompetencies.Dispose();
                }


            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                dtCompetencies.Dispose();
            }
        }

        protected void btnClearComp_Click(object sender, EventArgs e)
        {
            log.Debug("btnClearComp_Click()");
            try
            {
                clearCompetencyInputFields();
                Utility.Errorhandler.ClearError(lblErrorMsg2);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
        }

        protected void gvCompetencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gvCompetencies_SelectedIndexChanged()");
            DataTable dtCompetencies = new DataTable();
            try
            {
                int selectedIndex = gvCompetencies.SelectedIndex;
                string competencyId = gvCompetencies.Rows[selectedIndex].Cells[0].Text.ToString();
                string statusCode = gvCompetencies.Rows[selectedIndex].Cells[2].Text.ToString();

                ddlCompetency.SelectedValue = competencyId;
                ddlCompetencyStatus.SelectedValue = statusCode;
                hfCompetencyId.Value = competencyId;

                dtCompetencies = (Session["dtCompetencies"] as DataTable).Copy();
                txtCompetencyDesc.Text = dtCompetencies.Rows[selectedIndex]["DESCRIPTION"].ToString();

                ddlCompetency.Enabled = false;
                btnAddComp.Text = updateCompetencyText;

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                dtCompetencies.Dispose();
            }
        }

        protected void gvCompetencies_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvCompetencies_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvCompetencies, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
        }

        protected void gvCompetencies_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvCompetencies_PageIndexChanging()");
            try
            {
                gvCompetencies.PageIndex = e.NewPageIndex;
                fillCompetencyGridView();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
        }

        protected void gvTrainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gvTrainers_SelectedIndexChanged()");

            TrainerInformationDataHandler trainerInformationDataHandler = new TrainerInformationDataHandler();
            DataTable dtTrainerDetails = new DataTable();
            DataTable dtAvailableCompetencies = new DataTable();
            try
            {
                clearTrainerFields();
                clearCompetencyInputFields();
                removeImage();
                fillCompetencyGridView();
                Errorhandler.ClearError(lblErrorMsg);
                Errorhandler.ClearError(lblErrorMsg2);
                Errorhandler.ClearError(lblErrorMsgPhoto);
                
                int selectedIndex = gvTrainers.SelectedIndex;
                string trainerId = gvTrainers.Rows[selectedIndex].Cells[0].Text.ToString();

                dtTrainerDetails = trainerInformationDataHandler.getTrainerById(trainerId);
                fillTrainerDetails(dtTrainerDetails);

                dtAvailableCompetencies = trainerInformationDataHandler.getCompetenciesByTrainerId(trainerId);
                fillCompetencies(dtAvailableCompetencies);

                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                trainerInformationDataHandler = null;
                dtTrainerDetails.Dispose();
                dtAvailableCompetencies.Dispose();
            }
        }       

        protected void gvTrainers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvTrainers_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvTrainers, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void gvTrainers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvTrainers_PageIndexChanging()");
            try
            {
                gvTrainers.PageIndex = e.NewPageIndex;
                fillTrainersGridView();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void lbAddViewProgrames_Click(object sender, EventArgs e)
        {
            log.Debug("lbFindInstitute_Click()");
            Server.Transfer("~/TrainingAndDevelopment/WebFrmAddProgramesToTrainer.aspx");
        }

        protected void lbRemoveImage_Click(object sender, EventArgs e)
        {
            try
            {
                uploaderRow.Visible = true;
                
                removeImage();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void ddlTrainingNatureFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                fillTrainersGridView();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void ibtnApply_Click(object sender, ImageClickEventArgs e)
        {
            string sName = "";
            txtName.Text = "";

            if (txtFullName.Text.Trim() != String.Empty)
            {
                string[] fName = txtFullName.Text.Trim().Split(' ');

                if (fName.Length > 1)
                {
                    for (int intI = 0; intI < fName.Length; intI++)
                    {
                        if (intI == (fName.Length - 1))
                        {
                            sName = sName + fName[intI];
                        }
                        else
                        {
                            sName = sName + fName[intI].Substring(0, 1).ToUpper() + ".";
                        }
                    }

                    txtName.Text = sName;
                }
                else
                {
                    txtName.Text = txtFullName.Text.Trim();
                }
            }

        }

        #endregion

        

        



        

        



        

        
        

        

        
    }
}