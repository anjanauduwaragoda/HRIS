using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Caching;
using DataHandler.Employee;
using DataHandler.MetaData;
using Common;
using NLog;
using GroupHRIS.Utility;

namespace GroupHRIS.Employee
{
    public partial class webFrmEmployee : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            //log.Debug("Test1", "XXX");

            sIPAddress = Request.UserHostAddress;

            
            log.Debug("IP:" + sIPAddress + "webFrmEmployee : Page_Load");

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
                //Session["KeyCOMP_ID"] = Constants.CON_UNIVERSAL_COMPANY_CODE;

                //Session["KeyCOMP_ID"] = "CP17";

                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        fillCompanies();

                        //if (ddlCompany.SelectedIndex > 0)
                        //{
                        //    getNextEpfNo(ddlCompany.SelectedValue.Trim());
                        //    fillCostCenter(ddlCompany.SelectedValue.Trim());
                        //    fillProfitCenter(ddlCompany.SelectedValue.Trim());
                        //}
                    }
                    else
                    {
                        fillCompanies(Session["KeyCOMP_ID"].ToString().Trim());
                        ddlCompany.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                        fillDepartment(Session["KeyCOMP_ID"].ToString().Trim());
                        fillDesignations(Session["KeyCOMP_ID"].ToString().Trim());
                        fillBranches(Session["KeyCOMP_ID"].ToString().Trim());
                        fillLocations(Session["KeyCOMP_ID"].ToString().Trim());
                        getNextEpfNo(Session["KeyCOMP_ID"].ToString().Trim());
                        //fillCostCenter(ddlCompany.SelectedValue.Trim());
                        //fillProfitCenter(ddlCompany.SelectedValue.Trim());
                    }
                }

                fillEmployeeStatus();
                fillEmployeeType();
                fillEmployeeRole();
                fillModificationCategory();
                fillOtSession();

            }
            else
            {

                if (hfCaller.Value == "txtEmployeeID")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtEmployeeID.Text = hfVal.Value;
                    }
                    if (txtEmployeeID.Text != "")
                    {
                        //Postback Methods
                    }
                }
                if (hfCaller.Value == "txtReportTo1")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtReportTo1.Text = hfVal.Value;
                    }
                    if (txtReportTo1.Text != "")
                    {
                        //Postback Methods
                    }
                }

                if (hfCaller.Value == "txtReportTo2")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtReportTo2.Text = hfVal.Value;
                    }
                    if (txtReportTo2.Text != "")
                    {
                        //Postback Methods
                    }
                }

                if (hfCaller.Value == "txtReportTo3")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtReportTo3.Text = hfVal.Value;
                    }
                    if (txtReportTo3.Text != "")
                    {
                        //Postback Methods
                    }
                }
            }
        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            string companyId = "";
            string departmentId = "";
            string divisionId = "";
            string status = "";
            string employeeTypeId = "";
            string employeeRoleId = "";
            string title = "";
            string initials = "";
            string firstName = "";
            string middleName = "";
            string lastName = "";
            string fullName = "";
            string gender = "";
            string nic = "";
            string passportNo = "";
            string maritalStatus = "";
            string nationality = "";
            string religon = "";
            string email = "";
            string epfNo = "";
            string etfNo = "";
            string dateOfBirth = "";
            string dateOfJoin = "";
            string permenentAddress = "";
            string currentAddress = "";
            string landPhone = "";
            string mobilePersonal = "";
            string mobileOffice = "";
            string fuelcardNo = "";
            string reportTo1 = "";
            string reportTo2 = "";
            string reportTo3 = "";
            string city = "";
            string distance = "";
            string remarks = "";
            string addedBy = ""; 
            string costCenter = "";
            string profitCenter = "";
            string isWelfare = "";
            //string attRegNo = "";
            string resignedDate = "";
            string designationId = "";
            string branchId = "";
            string locationId = "";

            string initialsName = "";
            string knownName = "";
            string modCatId = "";
            string probContEndDate = "";

            string isOTEligible = "";
            string isRoster = "";
            string isMailExclude = "";
            string oTSession = "";

            //OT Validation //2015-10-28 //CHATHURA
            if (OTValidation() == false)
            {
                return;
            }
            //

            try
            {
                Utility.Errorhandler.ClearError(lblMessage);

                if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                {
                    companyId = ddlCompany.SelectedValue.Trim();
                }
                else
                {
                    companyId = Session["KeyCOMP_ID"].ToString().Trim();
                }

                if (chkWelfare.Checked)
                {
                    isWelfare = Constants.CON_WELFARE_ACCEPTED;
                }
                else
                {
                    isWelfare = Constants.CON_WELFARE_REJECTED;
                }

                departmentId        = ddlDepartment.SelectedValue.Trim();                
                divisionId          = ddlDivision.SelectedValue.Trim();
                status              = ddlStatus.SelectedValue.Trim();
                employeeTypeId      = ddlEmployeeType.SelectedValue.Trim();
                employeeRoleId      = ddlEmployeeRole.SelectedValue.Trim();
                title               = ddlTitle.SelectedValue.Trim();
                initials            = txtInitials.Text.Trim();
                firstName           = txtFirstName.Text.Trim();
                middleName          = txtMiddleName.Text.Trim();
                lastName            = txtLastName.Text.Trim();;
                fullName            = txtFullName.Text.Trim();;
                gender              = ddlGender.SelectedValue.Trim();
                nic                 = txtNic.Text.Trim();
                passportNo          = txtPassportNo.Text.Trim();
                dateOfBirth         = txtDob.Text.Trim();                
                dateOfJoin          = txtDoj.Text.Trim();
                maritalStatus       = ddlMaritalStatus.Text.Trim();
                nationality         = txtNationality.Text.Trim();
                religon             = txtReligon.Text.Trim();
                email               = txtEmail.Text.Trim();
                epfNo               = txtEpfNo.Text.Trim();
                etfNo               = txtEtfNo.Text.Trim();               
                permenentAddress    = txtPermanentAddress.Text.Trim();
                currentAddress      = txtCurrentAddress.Text.Trim();
                landPhone           = txtLandPhone.Text.Trim();
                mobilePersonal      = txtMobilePersonal.Text.Trim();
                mobileOffice        = txtMobileOffice.Text.Trim();
                fuelcardNo          = txtFuelCardNo.Text.Trim();
                reportTo1           = txtReportTo1.Text.Trim();
                reportTo2           = txtReportTo2.Text.Trim();
                reportTo3           = txtReportTo3.Text.Trim();
                city                = txtCity.Text.Trim();
                distance            = txtDistance.Text.Trim();
                remarks             = txtRemarks.Text.Trim();
                profitCenter        = txtProfitCenter.Text.Trim();
                costCenter          = txtCostCenter.Text.Trim();
                //profitCenter        = ddlToCC.SelectedValue.Trim();
                //costCenter          = ddlToPC.SelectedValue.Trim();

                //string[] cArrRetrive = vprofitCenter.Split('-');
                //string[] pArrRetrive = profitCenter.Split('-');

                //string profitCenter = cArrRetrive[0];
                //string costCenter = pArrRetrive[0];

                //attRegNo            = txtAttRegNo.Text.Trim();
                resignedDate        = txtResignedDate.Text.Trim();

                designationId = ddlDesignation.SelectedValue.Trim();
                branchId = ddlBranch.SelectedValue.Trim();
                locationId = ddlLocation.SelectedValue.Trim();

                initialsName = txtNameInitials.Text.Trim();
                knownName = txtKnownName.Text.Trim();
                modCatId = ddlModCategory.SelectedValue;
                
                probContEndDate = txtProbConEndDate.Text.Trim();


                if (chkOT.Checked == true)
                {
                    isOTEligible = Constants.OT_ELIGIBLE.ToString();
                    oTSession = ddlOtSession.SelectedValue.Trim();
                }
                else if (chkOT.Checked == false)
                {
                    isOTEligible = Constants.OT_NOT_ELIGIBLE.ToString();
                }

                if (chkIsRoster.Checked == true)
                {
                    isRoster = Constants.ROSTER_EMPLOYEE;
                }
                else if (chkIsRoster.Checked == false)
                {
                    isRoster = Constants.REGULAR_EMPLOYEE;
                }

                if (chkMailExclude.Checked == true)
                {
                    isMailExclude = Constants.CON_MAIL_EXCLUDE;
                }
                else if (chkMailExclude.Checked == false)
                {
                    isMailExclude = Constants.CON_MAIL_INCLUDE;
                }

                if (Session["KeyUSER_ID"] != null)
                {
                    addedBy = Session["KeyUSER_ID"].ToString();
                }
                else
                {
                    addedBy = Constants.SYSTEM_USER;
                }

                if (Utility.Utils.verifyDate(dateOfBirth) == false)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Date of Birth format incorrect", lblMessage);
                    return;
                }

                if (Utility.Utils.verifyDate(probContEndDate) == false)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Probation/Contract End Date format incorrect", lblMessage);
                    return;
                }

                if (Utility.Utils.verifyDate(dateOfJoin) == false)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Date of Join format incorrect", lblMessage);
                    return;
                }

                
                if (Utility.Utils.verifyDate(resignedDate) == false)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Date of Resign format incorrect", lblMessage);
                    return;
                }

                if ((ddlStatus.SelectedValue == Constants.CON_EMPLOYEE_STATUS_RESIGN) && (txtResignedDate.Text.Trim() == ""))
                {
                    Utility.Errorhandler.ClearError(lblMessage);
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Date of Resign is required", lblMessage);
                    return;
                }

                if ((ddlStatus.SelectedValue != Constants.CON_EMPLOYEE_STATUS_RESIGN) && (txtResignedDate.Text.Trim() != ""))
                {
                    Utility.Errorhandler.ClearError(lblMessage);
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Change the Status to Resigned", lblMessage);
                    return;
                }

                if (employeeDataHandler.isEpfExist(companyId, epfNo) && (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "EPF No already exist", lblMessage);
                    return;
                }
                else if (employeeDataHandler.isEpfExist(companyId, epfNo, hfEmployeeId.Value.ToString().Trim()) && (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "EPF No already exist", lblMessage);
                    return;
                }

                if ((employeeDataHandler.isEtfExist(companyId, etfNo)) && (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "ETF No already exist", lblMessage);
                    return;
                }
                else if ((employeeDataHandler.isEtfExist(companyId, etfNo, hfEmployeeId.Value.ToString().Trim())) && (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "ETF No already exist", lblMessage);
                    return;
                }

                // Allow NIC Duplication Chathura Nawagamuwa 03/06/2017
                if (chkNICDuplication.Checked == false)
                {
                    if (employeeDataHandler.isNicExist(nic) && (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "NIC No already exist", lblMessage);
                        return;
                    }
                    else if ((employeeDataHandler.isNicExist(nic, hfEmployeeId.Value.ToString().Trim()) && (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "NIC No already exist", lblMessage);
                        return;
                    }
                }
                // --

                if ((reportTo1 != "") && (reportTo2 != "") && (reportTo1 == reportTo2)) 
                {

                    //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Please check Report to 1 and Report to 2 are identical ", lblMessage);

                    Utility.Errorhandler.GetError("2", "First Level Supervisor and Second Level Supervisor cannot be  identical", lblMessage);

                    return;
                }
                else if((reportTo1 != "") && (reportTo3 != "") && (reportTo1 == reportTo3))
                {

                    //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Please check Report to 1 and Report to 3 are identical ", lblMessage);

                    Utility.Errorhandler.GetError("2", "First Level Supervisor and Third Level Supervisor cannot be  identical ", lblMessage);

                    return;
                }
                else if((reportTo2 != "") && (reportTo3 != "") && (reportTo2 == reportTo3))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "First and Second Leval supervisors can not be identical", lblMessage);

                    return;
                }

                if ((btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT) && (ddlModCategory.SelectedItem.Text.Trim() == ""))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Modificatin category is empty", lblMessage);

                    return;
                }


                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Insert");

                    Boolean isInserted = employeeDataHandler.Insert(companyId, departmentId, divisionId, status, employeeTypeId, employeeRoleId, title, initials, firstName, 
                                                                    middleName, lastName, fullName, gender, nic, passportNo, dateOfBirth, dateOfJoin, maritalStatus, 
                                                                    nationality, religon, email, epfNo, etfNo, permenentAddress, currentAddress, landPhone,
                                                                    mobilePersonal, mobileOffice, fuelcardNo, reportTo1, reportTo2, reportTo3, city,
                                                                    distance, remarks, addedBy, costCenter, profitCenter, isWelfare, resignedDate,
                                                                    designationId, branchId, locationId, initialsName, knownName, modCatId, probContEndDate, isOTEligible, isRoster, isMailExclude, oTSession);
                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                    if (isInserted) 
                    { 
                        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('Employee is saved ..')", true); 
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Employee is saved", lblMessage);
                    }

                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Update");

                    string employeeId = "";

                    if (hfEmployeeId.Value.ToString().Trim() != "") employeeId = hfEmployeeId.Value.ToString().Trim();

                    if (employeeId != "")
                    {
                        Boolean isUpdated = employeeDataHandler.Update(employeeId, companyId, departmentId, divisionId, status, employeeTypeId, employeeRoleId, title, initials, firstName,
                                                                       middleName, lastName, fullName, gender, nic, passportNo, dateOfBirth, dateOfJoin, maritalStatus,
                                                                       nationality, religon, email, epfNo, etfNo, permenentAddress, currentAddress, landPhone,
                                                                       mobilePersonal, mobileOffice, fuelcardNo, reportTo1, reportTo2, reportTo3, city,
                                                                       distance, remarks, addedBy, costCenter, profitCenter, isWelfare, resignedDate,
                                                                       designationId, branchId, locationId, initialsName, knownName, modCatId, probContEndDate, isOTEligible, isRoster, isMailExclude, oTSession); 
                            


                        if (isUpdated) 
                        { 
                            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('Employee is updated ..')", true); 
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Employee is updated", lblMessage);
                        }

                    }

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                employeeDataHandler = null;
            }
        }        
       
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            log.Debug("btnCancel_Click()");

            clear();
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlCompany_SelectedIndexChanged()");

            lblNextEpfNo.Text = "";

            if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
            {
                if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                {
                    if (ddlCompany.SelectedValue != "")
                    {
                        fillDepartment(ddlCompany.SelectedValue.Trim());
                        fillDesignations(ddlCompany.SelectedValue.Trim());
                        fillBranches(ddlCompany.SelectedValue.Trim());
                        fillLocations(ddlCompany.SelectedValue.Trim());
                        getNextEpfNo(ddlCompany.SelectedValue.Trim());

                        ////TEMP COMENTED 2015-10-22 CHATHURA
                        //fillCostCenter(ddlCompany.SelectedValue.Trim());
                        //fillProfitCenter(ddlCompany.SelectedValue.Trim());
                        ////
                    }
                }
                else
                {
                    fillDepartment(Session["KeyCOMP_ID"].ToString());
                    fillDesignations(Session["KeyCOMP_ID"].ToString());
                    fillBranches(Session["KeyCOMP_ID"].ToString().Trim());
                    fillLocations(Session["KeyCOMP_ID"].ToString().Trim());
                    getNextEpfNo(Session["KeyCOMP_ID"].ToString().Trim());

                    //fillCostCenter(ddlCompany.SelectedValue.Trim());
                    //fillProfitCenter(ddlCompany.SelectedValue.Trim());
                }
            }
        }

        private void getNextEpfNo(String companyId)
        {
            log.Debug("getNextEpfNo() - companyId:" + companyId);
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            try
            {
                lblNextEpfNo.Text = "";
                lblNextEpfNo.Text = employeeDataHandler.getNextEpfNo(companyId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                employeeDataHandler = null;
            }

        }        
            
        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlDepartment_SelectedIndexChanged()");

            if (ddlDepartment.SelectedValue != "")
            {
                fillDivisions(ddlDepartment.SelectedValue.Trim());
            }
        }        

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            log.Debug("btnEdit_Click()");

            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            DataRow employee = null;
            try
            {
                employee = employeeDataHandler.populate(txtEmployeeID.Text.Trim());

                if (employee != null)
                {
                    hfEmployeeId.Value              = employee["EMPLOYEE_ID"].ToString();
                    ddlCompany.SelectedValue        = employee["COMPANY_ID"].ToString();
                    
                    ////2015-10-22 Temp Comented CHATHURA
                    //fillCostCenter(employee["COMPANY_ID"].ToString().Trim());
                    //if (ddlToCC.Items.Count > 0) {
                    //    ddlToCC.SelectedIndex = ddlToCC.Items.IndexOf(ddlToCC.Items.FindByValue(employee["PROFIT_CENTER"].ToString()));
                    //}
                    ////ddlToPC.SelectedValue = employee["PROFIT_CENTER"].ToString();

                    //fillProfitCenter(employee["COMPANY_ID"].ToString().Trim());
                    //if (ddlToPC.Items.Count > 0) {
                    //    ddlToPC.SelectedIndex = ddlToPC.Items.IndexOf(ddlToPC.Items.FindByValue(employee["COST_CENTER"].ToString()));
                    //}

                    txtCostCenter.Text = employee["PROFIT_CENTER"].ToString();
                    ////
                    fillDepartment(employee["COMPANY_ID"].ToString().Trim()); 
                    if (ddlDepartment.Items.Count > 0) { ddlDepartment.SelectedValue = employee["DEPT_ID"].ToString(); }
                    
                    fillDivisions(employee["DEPT_ID"].ToString().Trim()); 
                    if (ddlDivision.Items.Count > 0) { ddlDivision.SelectedValue = employee["DIVISION_ID"].ToString(); }
                    
                    if (ddlStatus.Items.Count == 0) { fillEmployeeStatus(); }
                    if (ddlStatus.Items.Count > 0) { ddlStatus.SelectedValue = employee["EMPLOYEE_STATUS"].ToString(); }
                    
                    if (ddlEmployeeType.Items.Count == 0) { fillEmployeeType(); }
                    if (ddlEmployeeType.Items.Count > 0) { ddlEmployeeType.SelectedValue = employee["EMP_TYPE_ID"].ToString(); }

                    if (ddlEmployeeRole.Items.Count == 0) { fillEmployeeRole(); }
                    if (ddlEmployeeRole.Items.Count > 0) { ddlEmployeeRole.SelectedValue = employee["ROLE_ID"].ToString(); }

                    fillBranches(employee["COMPANY_ID"].ToString().Trim());
                    if (ddlBranch.Items.Count > 0) { ddlBranch.SelectedValue = employee["BRANCH_ID"].ToString(); }

                    fillLocations(employee["COMPANY_ID"].ToString().Trim());
                    if (ddlLocation.Items.Count > 0) { ddlLocation.SelectedValue = employee["LOCATION_ID"].ToString(); }

                    fillDesignations(employee["COMPANY_ID"].ToString().Trim()); 
                    if (ddlDesignation.Items.Count > 0) {
                        ddlDesignation.SelectedValue = employee["DESIGNATION_ID"].ToString(); 
                    
                    }

                    if (ddlOtSession.Items.Count == 0) { fillOtSession();}
                    if (ddlOtSession.Items.Count > 0) 
                    {
                        if (employee["OT_SESSION"].ToString().Trim() != "")
                        {
                            ddlOtSession.SelectedValue = employee["OT_SESSION"].ToString();
                        }  
                    }

                    txtCostCenter.Text = employee["COST_CENTER"].ToString().Trim();
                    txtProfitCenter.Text = employee["PROFIT_CENTER"].ToString().Trim();

                    //try { ddlToCC.SelectedIndex = ddlToCC.Items.IndexOf(ddlToCC.Items.FindByValue(employee["COST_CENTER"].ToString().Trim())); }
                    //catch { }
                    //try { ddlToPC.SelectedIndex = ddlToPC.Items.IndexOf(ddlToPC.Items.FindByValue(employee["PROFIT_CENTER"].ToString().Trim())); }
                    //catch { }
                    

                    ddlTitle.SelectedValue          = employee["TITLE"].ToString();
                    txtInitials.Text                = employee["EMP_INITIALS"].ToString();
                    txtFirstName.Text               = employee["FIRST_NAME"].ToString();
                    txtMiddleName.Text              = employee["MIDDLE_NAMES"].ToString();
                    txtLastName.Text                = employee["LAST_NAME"].ToString();
                    txtFullName.Text                = employee["FULL_NAME"].ToString();
                    ddlGender.SelectedValue         = employee["GENDER"].ToString().Trim();
                    txtNic.Text                     = employee["EMP_NIC"].ToString();
                    txtPassportNo.Text              = employee["PASSPORT_NUMBER"].ToString();
                    txtDob.Text                     = Convert.ToDateTime(employee["DOB"].ToString()).ToString("yyyy/MM/dd");
                    txtDoj.Text                     = Convert.ToDateTime(employee["DOJ"].ToString()).ToString("yyyy/MM/dd");
                    string rdate = employee["RESIGNED_DATE"].ToString();
                    if (rdate != "")
                    {
                        txtResignedDate.Text = Convert.ToDateTime(employee["RESIGNED_DATE"].ToString()).ToString("yyyy/MM/dd");
                    }
                    ddlMaritalStatus.Text           = employee["MARITAL_STATUS"].ToString();
                    txtNationality.Text             = employee["NAIONALITY"].ToString();
                    txtReligon.Text                 = employee["RELIGION"].ToString();
                    txtEmail.Text                   = employee["EMAIL"].ToString();
                    txtEpfNo.Text                   = employee["EPF_NO"].ToString();
                    txtEtfNo.Text                   = employee["ETF_NO"].ToString();
                    txtPermanentAddress.Text        = employee["PERMANENT_ADDRESS"].ToString();
                    txtCurrentAddress.Text          = employee["CURRENT_ADDRESS"].ToString();
                    txtLandPhone.Text               = employee["LAND_PHONE"].ToString();
                    txtMobilePersonal.Text          = employee["MOBILE_PHONE_PERSONAL"].ToString();
                    txtMobileOffice.Text            = employee["MOBILE_PHONE_COMPANY"].ToString();
                    txtFuelCardNo.Text              = employee["FUEL_CARD_NUMBER"].ToString();
                    txtReportTo1.Text               = employee["REPORT_TO_1"].ToString();
                    txtReportTo2.Text               = employee["REPORT_TO_2"].ToString();
                    txtReportTo3.Text               = employee["REPORT_TO_3"].ToString();
                    txtCity.Text                    = employee["CITY"].ToString();
                    txtDistance.Text                = employee["DISTANCE_TO_OFFICE"].ToString();
                    txtRemarks.Text                 = employee["REMARKS"].ToString();
                    //ddlToCC.SelectedValue           = employee["COST_CENTER"].ToString();
                    //ddlToPC.SelectedValue           = employee["PROFIT_CENTER"].ToString();

                    //DateTime? probConEndDate = Convert.ToDateTime(employee["PROBATION_CONTRACT_ENDDATE"].ToString());

                    if (employee["PROBATION_CONTRACT_ENDDATE"] != DBNull.Value)
                    {                       
                        txtProbConEndDate.Text = Convert.ToDateTime(employee["PROBATION_CONTRACT_ENDDATE"].ToString()).ToString("yyyy/MM/dd");
                    }
                    else
                    {
                        txtProbConEndDate.Text = "";
                    }

                    txtAttRegNo.Text = employee["ATT_REG_NO"].ToString();

                    txtNameInitials.Text = employee["INITIALS_NAME"].ToString();
                    txtKnownName.Text = employee["KNOWN_NAME"].ToString();


                    if(employee["IS_WELFARE"].ToString().Equals(Constants.CON_WELFARE_ACCEPTED))
                    {
                        chkWelfare.Checked = true;
                    }
                    else if(employee["IS_WELFARE"].ToString().Equals(Constants.CON_WELFARE_REJECTED))
                    {
                        chkWelfare.Checked = false;
                    }

                    if (employee["IS_OT_ELIGIBLE"].ToString().Equals(Constants.OT_ELIGIBLE.ToString()))
                    {
                        chkOT.Checked = true;
                    }
                    else if (employee["IS_OT_ELIGIBLE"].ToString().Equals(Constants.OT_NOT_ELIGIBLE.ToString()))
                    {
                        chkOT.Checked = false;
                    }

                    if (employee["IS_ROSTER"].ToString().Equals(Constants.ROSTER_EMPLOYEE.ToString()))
                    {
                        chkIsRoster.Checked = true;
                    }
                    else if (employee["IS_ROSTER"].ToString().Equals(Constants.REGULAR_EMPLOYEE.ToString()))
                    {
                        chkIsRoster.Checked = false;
                    }
                    
                    if (employee["EXCLUDE_EMAIL"].ToString().Trim() ==Constants.CON_MAIL_EXCLUDE.ToString())
                    {
                        chkMailExclude.Checked = true;
                    }
                    else if (employee["EXCLUDE_EMAIL"].ToString().Trim() == Constants.CON_MAIL_INCLUDE.ToString())
                    {
                        chkMailExclude.Checked = false;
                    }

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                employeeDataHandler = null;
                employee = null;
            }
        }

        protected void btnSCancel_Click(object sender, EventArgs e)
        {
            log.Debug("btnSCancel_Click()");
            GroupHRIS.Utility.Utils.clearControls(false, txtEmployeeID);
        }

        #region Private Methods
        //------------------------------------------------------------------------------
        ///<summary>
        ///Load all companies 
        ///</summary>
        
        //----------------------------------------------------------------------------------------

        private void fillCompanies()
        {
            log.Debug("fillCompanies()");

            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable companies = new DataTable();

            try
            {
                if (Cache["Companies"] != null)
                {
                    companies = (DataTable)Cache["Companies"];
                }
                else
                {
                    companies = companyDataHandler.getCompanyIdCompName().Copy();
                    Cache.Add("Companies", companies, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);                    
                }                               

                ddlCompany.Items.Clear();

                if (companies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlCompany.Items.Add(Item);

                    foreach (DataRow dataRow in companies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlCompany.Items.Add(listItem);
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
                companyDataHandler = null;
                companies.Dispose();
            }
        }

        //------------------------------------------------------------------------------
        ///<summary>
        ///Load a companies 
        ///</summary>
        ///<param name="companyId">Pass a company id string to query </param>
        //----------------------------------------------------------------------------------------

        private void fillCompanies(string companyId)
        {
            log.Debug("fillCompanies() - companyId:" + companyId);

            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable companies = new DataTable();

            try
            {
                companies = companyDataHandler.getCompanyIdCompName(companyId).Copy();

                ddlCompany.Items.Clear();

                if (companies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlCompany.Items.Add(Item);

                    foreach (DataRow dataRow in companies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlCompany.Items.Add(listItem);
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
                companyDataHandler = null;
                companies.Dispose();
            }

        }

        //------------------------------------------------------------------------------
        ///<summary>
        ///Clear all textbox and dropdownlist controls or same in a control such as panel 
        ///</summary>       
        //----------------------------------------------------------------------------------------


        private void clear()
        {
            log.Debug("clear()");

            GroupHRIS.Utility.Utils.clearControls(true, ddlCompany, ddlDepartment, ddlDivision, ddlStatus, ddlEmployeeType, ddlEmployeeRole, ddlTitle, txtInitials,
                                txtFirstName, txtMiddleName, txtLastName, txtFullName, ddlGender, txtNic, txtPassportNo, txtDob,
                                txtDoj, ddlMaritalStatus, txtNationality, txtReligon, txtEmail, txtEpfNo, txtEtfNo, txtPermanentAddress,
                                txtCurrentAddress, txtLandPhone, txtMobilePersonal, txtMobileOffice, txtFuelCardNo, txtReportTo1,
                                txtReportTo2, txtResignedDate, txtReportTo3, txtCity, txtDistance, txtRemarks, txtCostCenter, txtProfitCenter, txtAttRegNo,
                                chkWelfare,txtKnownName,txtNameInitials,ddlBranch,ddlLocation,ddlModCategory,txtProbConEndDate,ddlDesignation,lblNextEpfNo,chkMailExclude,chkIsRoster,chkOT,ddlOtSession);
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblMessage);
        }

        //------------------------------------------------------------------------------
        ///<summary>
        ///fill ddlDepartment dropdownlist control with department id and name
        ///</summary>       
        //----------------------------------------------------------------------------------------

        private void fillDepartment(string companyId)
        {
            log.Debug("fillDepartment() - companyId:" + companyId);

            DepartmentDataHandler departmentDataHandler = new DepartmentDataHandler();
            DataTable departments = new DataTable();

            try
            {
                if (Cache["Departments" + companyId.Trim()] != null)
                {
                    departments = (DataTable)Cache["Departments" + companyId.Trim()];
                }
                else
                {
                    departments = departmentDataHandler.getDepartmentIdDeptName(companyId).Copy();
                    Cache.Add("Departments" + companyId.Trim(), departments, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                    
                }                

                ddlDepartment.Items.Clear();

                if (departments.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlDepartment.Items.Add(Item);

                    foreach (DataRow dataRow in departments.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DEPT_NAME"].ToString();
                        listItem.Value = dataRow["DEPT_ID"].ToString();

                        ddlDepartment.Items.Add(listItem);
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
                departmentDataHandler = null;
                departments.Dispose();
            }

        }

        //------------------------------------------------------------------------------
        ///<summary>
        ///Load all divisions for a given department 
        ///</summary>
        ///<param name="departmentId">Pass a department id string to query </param>
        //----------------------------------------------------------------------------------------

        private void fillDivisions(string departmentId)
        {
            log.Debug("fillDivisions() - departmentId:" + departmentId);

            DivisionDataHandler divisionDataHandler = new DivisionDataHandler();
            DataTable divisions = new DataTable();

            try
            {
                if (Cache["Divisions" + departmentId.Trim()] != null)
                {
                    divisions = (DataTable)Cache["Divisions" + departmentId.Trim()];
                }
                else
                {
                    divisions = divisionDataHandler.getDivisionIdDivName(departmentId).Copy();                    
                    Cache.Add("Divisions" + departmentId.Trim(), divisions, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }
                

                ddlDivision.Items.Clear();

                if (divisions.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlDivision.Items.Add(Item);

                    foreach (DataRow dataRow in divisions.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DIV_NAME"].ToString();
                        listItem.Value = dataRow["DIVISION_ID"].ToString();

                        ddlDivision.Items.Add(listItem);
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
                divisionDataHandler = null;
                divisions.Dispose();
            }

        }

        //------------------------------------------------------------------------------
        ///<summary>
        ///Load all employee STATUS 
        ///</summary>        
        //----------------------------------------------------------------------------------------

        private void fillEmployeeStatus()
        {
            log.Debug("fillEmployeeStatus()");

            EmployeeStatusDataHandler employeeStatusDataHandler = new EmployeeStatusDataHandler();
            DataTable employeeStatus = new DataTable();

            try
            {
                if (Cache["EmployeeStatus"] != null)
                {
                    employeeStatus = (DataTable)Cache["EmployeeStatus"];
                }
                else
                {
                    employeeStatus = employeeStatusDataHandler.populate().Copy();
                    Cache.Add("EmployeeStatus", employeeStatus, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                    
                }
                

                ddlStatus.Items.Clear();

                if (employeeStatus.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlStatus.Items.Add(Item);

                    foreach (DataRow dataRow in employeeStatus.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DESCRIPTION"].ToString();
                        listItem.Value = dataRow["STATUS_CODE"].ToString();

                        ddlStatus.Items.Add(listItem);
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
                employeeStatusDataHandler = null;
                employeeStatus.Dispose();
            }

        }

        //------------------------------------------------------------------------------
        ///<summary>
        ///Load all employee modification categories 
        ///</summary>        
        //----------------------------------------------------------------------------------------

        private void fillModificationCategory()
        {
            log.Debug("fillEmployeeStatus()");

            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            DataTable modiCategory = new DataTable();

            try
            {
                if (Cache["ModiCategory"] != null)
                {
                    modiCategory = (DataTable)Cache["ModiCategory"];
                }
                else
                {
                    modiCategory = employeeDataHandler.populateModificationCategory().Copy();
                    Cache.Add("ModiCategory", modiCategory, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);

                }


                ddlModCategory.Items.Clear();

                if (modiCategory.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlModCategory.Items.Add(Item);

                    foreach (DataRow dataRow in modiCategory.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DESCRIPTION"].ToString();
                        listItem.Value = dataRow["MOD_CAT_ID"].ToString();

                        ddlModCategory.Items.Add(listItem);
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
                employeeDataHandler = null;
                modiCategory.Dispose();
            }

        }


        //------------------------------------------------------------------------------
        ///<summary>
        ///Load all employee types
        ///</summary>       
        //----------------------------------------------------------------------------------------

        private void fillEmployeeType()
        {
            log.Debug("fillEmployeeType()");

            EmployeeTypeDataHandler employeeTypeDataHandler = new EmployeeTypeDataHandler();
            DataTable employeeType = new DataTable();

            try
            {
                if (Cache["EmployeeType"] != null)
                {
                    employeeType = (DataTable)Cache["EmployeeType"];
                }
                else
                {
                    employeeType = employeeTypeDataHandler.populate().Copy();
                    Cache.Add("EmployeeType", employeeType, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                   
                }
                

                ddlEmployeeType.Items.Clear();

                if (employeeType.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlEmployeeType.Items.Add(Item);

                    foreach (DataRow dataRow in employeeType.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["TYPE_NAME"].ToString();
                        listItem.Value = dataRow["EMP_TYPE_ID"].ToString();

                        ddlEmployeeType.Items.Add(listItem);
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
                employeeTypeDataHandler = null;
                employeeType.Dispose();
            }
        }

        //------------------------------------------------------------------------------
        ///<summary>
        ///Load all employee roles
        ///</summary>        
        //----------------------------------------------------------------------------------------

        private void fillEmployeeRole()
        {
            log.Debug("fillEmployeeRole()");

            EmployeeRoleDataHandler employeeRoleDataHandler = new EmployeeRoleDataHandler();
            DataTable employeeRole = new DataTable();

            try
            {
                if (Cache["EmployeeRole"] != null)
                {
                    employeeRole = (DataTable)Cache["EmployeeRole"];
                }
                else
                {
                    employeeRole = employeeRoleDataHandler.populate().Copy();
                    Cache.Add("EmployeeRole", employeeRole, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                    
                }
                               

                ddlEmployeeRole.Items.Clear();

                if (employeeRole.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlEmployeeRole.Items.Add(Item);

                    foreach (DataRow dataRow in employeeRole.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["ROLE_NAME"].ToString();
                        listItem.Value = dataRow["ROLE_ID"].ToString();

                        ddlEmployeeRole.Items.Add(listItem);
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
                employeeRoleDataHandler = null;
                employeeRole.Dispose();
            }
        }

        private void fillDesignations(string companyId)
        {
            log.Debug("fillDesignations() - companyId:" + companyId);

            DesignationDataHandler designationDataHandler = new DesignationDataHandler();
            DataTable designations = new DataTable();

            try
            {
                if (Cache["Designations" + companyId.Trim()] != null)
                {
                    designations = (DataTable)Cache["Designations" + companyId.Trim()];
                }
                else
                {
                    designations = designationDataHandler.getDesignationIdDesigName(companyId).Copy();
                    Cache.Add("Designations" + companyId.Trim(), designations, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);

                }

                ddlDesignation.Items.Clear();

                if (designations.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlDesignation.Items.Add(Item);

                    foreach (DataRow dataRow in designations.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DESIGNATION_NAME"].ToString();
                        listItem.Value = dataRow["DESIGNATION_ID"].ToString();

                        ddlDesignation.Items.Add(listItem);
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
                designationDataHandler = null;
                designations.Dispose();
            }

        }

        private void fillBranches(string companyId)
        {
            log.Debug("fillBranches() - companyId:" + companyId);

            BranchCenterDataHandler branchCenterDataHandler = new BranchCenterDataHandler();
            DataTable branches = new DataTable();

            try
            {
                if (Cache["Branches"+ companyId.Trim()] != null)
                {
                    branches = (DataTable)Cache["Branches" + companyId.Trim()];
                }
                else
                {
                    branches = branchCenterDataHandler.getBranchIdBranchName(companyId).Copy();
                    Cache.Add("Branches" + companyId.Trim(), branches, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);

                }

                ddlBranch.Items.Clear();

                if (branches.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlBranch.Items.Add(Item);

                    foreach (DataRow dataRow in branches.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["BRANCH_NAME"].ToString();
                        listItem.Value = dataRow["BRANCH_ID"].ToString();

                        ddlBranch.Items.Add(listItem);
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
                branchCenterDataHandler = null;
                branches.Dispose();
            }

        }

        private void fillLocations(string companyId)
        {
            log.Debug("fillLocations() - companyId:" + companyId);

            CompanyLocationDataHandler companyLocationDataHandler = new CompanyLocationDataHandler();
            DataTable locations = new DataTable();

            try
            {
                if (Cache["Locations" + companyId.Trim()] != null)
                {
                    locations = (DataTable)Cache["Locations" + companyId.Trim()];
                }
                else
                {
                    locations = companyLocationDataHandler.getLocationIdLocName(companyId).Copy();
                    Cache.Add("Locations" + companyId.Trim(), locations, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);

                }

                ddlLocation.Items.Clear();

                if (locations.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlLocation.Items.Add(Item);

                    foreach (DataRow dataRow in locations.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["LOCATION_NAME"].ToString();
                        listItem.Value = dataRow["LOCATION_ID"].ToString();

                        ddlLocation.Items.Add(listItem);
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
                companyLocationDataHandler = null;
                locations.Dispose();
            }

        }


        #endregion

        protected void ibtnApply_Click(object sender, ImageClickEventArgs e)
        {
            string sName = "";
            txtNameInitials.Text = "";

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

                    txtNameInitials.Text = sName;
                }
                else
                {
                    txtNameInitials.Text = txtFullName.Text.Trim();
                }
            }

        }

        protected void ibtnGet_Click(object sender, ImageClickEventArgs e)
        {
            txtEpfNo.Text = lblNextEpfNo.Text.Trim();
            txtEtfNo.Text = lblNextEpfNo.Text.Trim();
        }

        private void fillOtSession()
        {
            log.Debug("fillOtSession()");

            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            DataTable employeeOtSession = new DataTable();

            try
            {
                //if (Cache["EmployeeOT"] != null)
                //{
                //    employeeOtSession = (DataTable)Cache["EmployeeOT"];
                //}
                //else
                //{
                //    employeeOtSession = employeeDataHandler.getOtSessions().Copy();
                //    Cache.Add("EmployeeOT", employeeOtSession, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);

                //}

                employeeOtSession = employeeDataHandler.getOtSessions().Copy();

                ddlOtSession.Items.Clear();

                if (employeeOtSession.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlOtSession.Items.Add(Item);

                    foreach (DataRow dataRow in employeeOtSession.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DESCRIPTION"].ToString();
                        listItem.Value = dataRow["SESSOIN_ID"].ToString();

                        ddlOtSession.Items.Add(listItem);
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
                employeeDataHandler = null;
                employeeOtSession.Dispose();
            }

        }


        Boolean OTValidation()
        {
            if ((ddlOtSession.SelectedIndex == 0) && (chkOT.Checked == true))
            {
                Errorhandler.GetError("2", "OT Session is Required", lblMessage);
                return false;
            }
            else if ((ddlOtSession.SelectedIndex != 0) && (chkOT.Checked == false))
            {
                Errorhandler.GetError("2", "Tick 'Is OT Eligible'", lblMessage);
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void chkOT_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOT.Checked == true)
            {
                
            }
            else if(chkOT.Checked == false)
            {


            }




        }

        ////fill Cost centers

        //private void fillCostCenter(string companyId)
        //{

        //    EmployeeTransferDataHandler oDataHandler = new EmployeeTransferDataHandler();
        //    DataTable dtcostCenter = new DataTable();

        //    try
        //    {
        //        dtcostCenter = oDataHandler.getCostCenterByCompany(companyId);
        //        ddlToCC.Items.Clear();

        //        if (dtcostCenter.Rows.Count > 0)
        //        {
        //            ListItem Item = new ListItem();
        //            Item.Text = "";
        //            Item.Value = "";
        //            ddlToCC.Items.Add(Item);

        //            foreach (DataRow dataRow in dtcostCenter.Rows)
        //            {
        //                ListItem listItem = new ListItem();
        //                listItem.Text = dataRow["COST_PROFIT_CENTER_NAME"].ToString();
        //                listItem.Value = dataRow["COMP_COST_PROFIT_CENTER_CODE"].ToString();

        //                //ddlToCC.Items.Add(listItem.Value + " - " + listItem.Text);
        //                ddlToCC.Items.Add(new ListItem(listItem.Value + " - " + listItem.Text, listItem.Value));
                        
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        ////fill Profit center

        //private void fillProfitCenter(string companyId)
        //{
        //    EmployeeTransferDataHandler oDataHandler = new EmployeeTransferDataHandler();
        //    DataTable dtprofitCenter = new DataTable();

        //    try
        //    {
        //        dtprofitCenter = oDataHandler.getProfitCenterByCompany(companyId);
        //        ddlToPC.Items.Clear();

        //        if (dtprofitCenter.Rows.Count > 0)
        //        {
        //            ListItem Item = new ListItem();
        //            Item.Text = "";
        //            Item.Value = "";
        //            ddlToPC.Items.Add(Item);

        //            foreach (DataRow dataRow in dtprofitCenter.Rows)
        //            {
        //                ListItem listItem = new ListItem();
        //                listItem.Text = dataRow["COST_PROFIT_CENTER_NAME"].ToString();
        //                listItem.Value = dataRow["COMP_COST_PROFIT_CENTER_CODE"].ToString();

        //                ddlToPC.Items.Add(new ListItem(listItem.Value + " - " + listItem.Text, listItem.Value));
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        
    }
}