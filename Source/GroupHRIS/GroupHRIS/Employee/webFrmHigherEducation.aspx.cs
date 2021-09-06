using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using DataHandler.Employee;
using DataHandler.MetaData;
using DataHandler.UserAdministration;
using DataHandler.Userlogin;
using Common;
using NLog;

namespace GroupHRIS.Employee
{
    public partial class webFrmHigherEducation : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string userID = "";
        private string keyEmpID = "";
        private string keyRole = "";
        public bool isSearchable = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);

            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }

            if (!IsPostBack)
            {
                fillYears();

                if (Session["KeyUSER_ID"] != null)
                {
                    userID      = Session["KeyUSER_ID"].ToString();
                    keyEmpID    = Session["KeyEMPLOYEE_ID"].ToString();
                    keyRole     = Session["KeyHRIS_ROLE"].ToString();

                    Session["UserRole"] = keyRole;
                    Session["employeeID"] = keyEmpID;

                    txtEmployeeID.Text = keyEmpID;

                    PrevExperienceDataHandler dhPrevExp = new PrevExperienceDataHandler();
                    txtName.Text = dhPrevExp.getEmployeeName(keyEmpID);

                    Session["txtName"] = txtName.Text;                   

                    populateGrid();
                }
            }

            if (IsPostBack)
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

                        string EmployeeID = txtEmployeeID.Text;

                        PreviousExperienceVerificationDataHandler PEVDH = new PreviousExperienceVerificationDataHandler();
                        txtName.Text = PEVDH.GetKnownNameFromEmployeeID(EmployeeID);

                        string Name = txtName.Text;

                        clear();


                        txtEmployeeID.Text = EmployeeID;
                        txtName.Text = Name;

                        string parameter = Request["__EVENTARGUMENT"];

                        if (parameter.Equals("TextChanged"))
                        {
                            Utility.Errorhandler.ClearError(lblMsg);
                        }

                        populateGrid();
                    }
                }
            }


            //if (hfCaller.Value == "txtEmployeeID")
            //{
            //    hfCaller.Value = "";
            //    if (hfVal.Value != "")
            //    {
            //        txtEmployeeID.Text = hfVal.Value;
            //    }
            //    if (txtEmployeeID.Text != "")
            //    {
            //        //Postback Methods

            //        string EmployeeID = hfEmpID.Value;
            //        string Name = hfName.Value;

            //        clear();

            //        txtEmployeeID.Text = EmployeeID;
            //        txtName.Text = Name;

            //        string parameter = Request["__EVENTARGUMENT"];

            //        if (parameter.Equals("TextChanged"))
            //        {
            //            Utility.Errorhandler.ClearError(lblMsg);
            //        }

            //        populateGrid();
            //    }
            //}

            setEmployeeSearchability();
        }

        private void setEmployeeSearchability()
        {
            UserAdministrationHandler dhUserAdmin = new UserAdministrationHandler();
            EmployeeDataHandler dhEmp = new EmployeeDataHandler();

            DataRow dr = null;

            try
            {
                string userRole = (Session["UserRole"] as string);
                string employeeID = (Session["employeeID"] as string);
                string empName = (Session["txtName"] as string);


                if (dhUserAdmin.isDefaultRole(userRole))
                {
                    txtEmployeeID.Text = employeeID;

                    dr = dhEmp.populate(employeeID);
                    txtName.Text = dr["FULL_NAME"].ToString();



                    hfEmpID.Value = employeeID;
                    hfName.Value = empName;

                    isSearchable = false;
                }
                else
                {
                    isSearchable = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //lblerror.Text = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);

                throw ex;
            }
            finally
            {
                dhUserAdmin = null;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string companyId        = "";
            string receiverEMail    = "";

            HigherEducationDataHandler dhHighEdu = new HigherEducationDataHandler();

            Utility.Errorhandler.ClearError(lblMsg);
            string sDuration = "";

            if ((txtDurationYears.Text.Trim().Length > 0) && (txtDurationMonths.Text.Trim().Length > 0))
            { sDuration = txtDurationYears.Text.Trim() + Constants.CON_YEAR_TOKEN + txtDurationMonths.Text.Trim(); }
            else if (txtDurationYears.Text.Trim().Length > 0)
            { sDuration = txtDurationYears.Text.Trim() + Constants.CON_YEAR_TOKEN; }
            else if (txtDurationMonths.Text.Trim().Length > 0)
            { sDuration = "0" + Constants.CON_YEAR_TOKEN + txtDurationMonths.Text.Trim(); }

            try
            {
                userID = Session["KeyUSER_ID"].ToString();

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    dhHighEdu.Insert(txtEmployeeID.Text.Trim(),
                                    txtInstitute.Text.Trim(),
                                    txtProgram.Text.Trim(),
                                    txtProgramName.Text.Trim(),
                                    txtSector.Text.Trim(),
                                    sDuration,
                                    ddlFromYear.SelectedItem.Text,
                                    ddlToYear.SelectedItem.Text,
                                    txtGrade.Text.Trim(),
                                    txtRemarks.Text.Trim(),
                                    userID);

                    //Get Company ID from Emp
                    companyId = getCompanyForEmployee(txtEmployeeID.Text);


                    DataTable dtEmailInfo = new DataTable();
                    dtEmailInfo = dhHighEdu.populateEmailInfromation(txtEmployeeID.Text.Trim());
                    string EmpName = String.Empty;
                    string EmpNIC = String.Empty;
                    string EmpEPF = String.Empty;
                    string EmpCompany = String.Empty;

                    if (dtEmailInfo.Rows.Count > 0)
                    {
                        EmpName = dtEmailInfo.Rows[0]["TITLE"].ToString().Trim() + " " + dtEmailInfo.Rows[0]["INITIALS_NAME"].ToString().Trim();
                        EmpNIC = dtEmailInfo.Rows[0]["EMP_NIC"].ToString().Trim();
                        EmpEPF = dtEmailInfo.Rows[0]["EPF_NO"].ToString().Trim();
                        EmpCompany = dtEmailInfo.Rows[0]["COMPANY_NAME"].ToString().Trim();
                    }


                    StringBuilder emailDeatails = getMailBody(EmpName, EmpNIC, EmpEPF, EmpCompany);


                    

                    //Get HR email address for company
                    if (companyId.Trim().Length > 0)
                    {
                        receiverEMail = getHREmail(companyId);
                    }
                    //Send email to HR
                    EmailHandler.SendDefaultEmail(Constants.CON_SYSTEM_NAME, receiverEMail, String.Empty, CommonVariables.EMAIL_SUBJECT_HIG_EDU_ADDED, emailDeatails);

                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_SAVED, lblMsg);

                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    dhHighEdu.Update(txtEmployeeID.Text.Trim(),
                                    hfLineNo.Value.Trim(),
                                    txtInstitute.Text.Trim(),
                                    txtProgram.Text.Trim(),
                                    txtProgramName.Text.Trim(),
                                    txtSector.Text.Trim(),
                                    sDuration,
                                    ddlFromYear.SelectedItem.Text,
                                    ddlToYear.SelectedItem.Text,
                                    txtGrade.Text.Trim(),
                                    txtRemarks.Text.Trim(),
                                    userID);

                    //Get Company ID from Emp

                    DataTable dtEmailInfo = new DataTable();
                    dtEmailInfo = dhHighEdu.populateEmailInfromation(txtEmployeeID.Text.Trim());
                    string EmpName = String.Empty;
                    string EmpNIC = String.Empty;
                    string EmpEPF = String.Empty;
                    string EmpCompany = String.Empty;

                    if (dtEmailInfo.Rows.Count > 0)
                    {
                        EmpName = dtEmailInfo.Rows[0]["TITLE"].ToString().Trim() + " " + dtEmailInfo.Rows[0]["INITIALS_NAME"].ToString().Trim();
                        EmpNIC = dtEmailInfo.Rows[0]["EMP_NIC"].ToString().Trim();
                        EmpEPF = dtEmailInfo.Rows[0]["EPF_NO"].ToString().Trim();
                        EmpCompany = dtEmailInfo.Rows[0]["COMPANY_NAME"].ToString().Trim();
                    }


                    StringBuilder emailDeatails = getMailBody(EmpName, EmpNIC, EmpEPF, EmpCompany);


                    companyId = getCompanyForEmployee(txtEmployeeID.Text);

                    //Get HR email address for company
                    if (companyId.Trim().Length > 0)
                        receiverEMail = getHREmail(companyId);

                    //Send email to HR
                    EmailHandler.SendDefaultEmail(Constants.CON_SYSTEM_NAME, receiverEMail, String.Empty, CommonVariables.EMAIL_SUBJECT_HIG_EDU_ADDED, emailDeatails);

                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED, lblMsg);
                }

                clear();
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                dhHighEdu = null;
            }

            populateGrid();
        }

        private StringBuilder getMailBody(string employeeName,string nic,string epf,string company)
        {
            StringBuilder mailBody = new StringBuilder();

            mailBody.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);
            mailBody.Append(employeeName + " has inserted or updated new higher education qualifications and pending for your verification." + Environment.NewLine + Environment.NewLine);
            mailBody.Append("NIC No. : " + nic + Environment.NewLine + Environment.NewLine);
            mailBody.Append("EPF No. : " + epf + Environment.NewLine + Environment.NewLine);
            mailBody.Append("Company : " + company + Environment.NewLine + Environment.NewLine);
            mailBody.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            mailBody.Append("[This is a system generated mail.]" + Environment.NewLine);

            return mailBody;
        }

        private string getCompanyForEmployee(string employeeId)
        {
            EmployeeDataHandler dhEmp;
            String companyId = "";

            try
            {
                dhEmp = new EmployeeDataHandler();
                companyId = companyId = dhEmp.getCompany(employeeId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
                throw ex;
            }
            finally
            {
                dhEmp = null;
            }

            return companyId;
        }

        private string getHREmail(string companyId)
        {
            CompanyDataHandler dhCompany;
            string mailAddresses = "";

            try
            {
                dhCompany = new CompanyDataHandler();
                mailAddresses = companyId = dhCompany.getHREmailAddressesForCompany(companyId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
                throw ex;
            }
            finally
            {
                dhCompany = null;
            }

            return mailAddresses;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);
            clear();

            setEmployeeSearchability();
            if (isSearchable == true)
            {
                txtEmployeeID.Text = "";
                txtName.Text = "";
                gvHighEdu.DataSource = null;
                gvHighEdu.DataBind();
            }

        }

        protected void gvSecEdu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHighEdu.PageIndex = e.NewPageIndex;
            populateGrid();
            clearOnPageIndexChanging();
        }

        private void fillYears()
        {
            ddlFromYear.Items.Add("");
            ddlToYear.Items.Add("");

            for (int i = DateTime.Today.Year; i >= (DateTime.Today.Year - Constants.CON_DROP_DOWN_NUM_YEARS); i--)
            {
                ddlFromYear.Items.Add(i.ToString());
                ddlToYear.Items.Add(i.ToString());
            }
        }

        private void populateGrid()
        {
            HigherEducationDataHandler dhHighEdu = new HigherEducationDataHandler();
            DataTable dtHighEdu = new DataTable();

            try
            {
                dtHighEdu = dhHighEdu.populate(txtEmployeeID.Text);

                gvHighEdu.DataSource = dtHighEdu;
                gvHighEdu.DataBind();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lblerror.Text = ex.Message;
            }
            finally
            {
                dhHighEdu = null;
                dtHighEdu.Dispose();
            }
        }

        private void clear()
        {
            btnSave.Enabled = true;
            GroupHRIS.Utility.Utils.clearControls(true, ddlFromYear, ddlToYear, txtDurationYears, txtDurationMonths, txtGrade, txtInstitute, txtProgram, txtProgramName, txtSector, txtRemarks);
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
        }

        protected void gvHighEdu_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);

            string sDuration = "";
            string[] duration = new string[2]; 

            try
            {
                txtInstitute.Text               = gvHighEdu.SelectedRow.Cells[1].Text.removeInvalidHTMLChars();
                txtProgram.Text                 = gvHighEdu.SelectedRow.Cells[2].Text.removeInvalidHTMLChars();
                txtProgramName.Text             = gvHighEdu.SelectedRow.Cells[3].Text.removeInvalidHTMLChars();
                txtSector.Text                  = gvHighEdu.SelectedRow.Cells[4].Text.removeInvalidHTMLChars();
                sDuration                       = gvHighEdu.SelectedRow.Cells[5].Text.removeInvalidHTMLChars();
                ddlFromYear.SelectedValue       = gvHighEdu.SelectedRow.Cells[6].Text;
                ddlToYear.SelectedValue         = gvHighEdu.SelectedRow.Cells[7].Text.removeInvalidHTMLChars();
                txtGrade.Text                   = gvHighEdu.SelectedRow.Cells[8].Text.removeInvalidHTMLChars();
                txtRemarks.Text                 = gvHighEdu.SelectedRow.Cells[9].Text.removeInvalidHTMLChars();
                hfLineNo.Value                  = gvHighEdu.SelectedRow.Cells[11].Text.removeInvalidHTMLChars();


                if (gvHighEdu.SelectedRow.Cells[12].Text.removeInvalidHTMLChars() != "Pending")
                {
                    btnSave.Enabled = false;
                }
                else
                {
                    btnSave.Enabled = true;
                }


                if(sDuration != null)
                {
                    if ((sDuration.Trim().Length > 0) && (sDuration.IndexOf(Constants.CON_YEAR_TOKEN) > 0))
                        duration = sDuration.Split(Constants.CON_YEAR_TOKEN);

                    txtDurationYears.Text = duration[0];
                    if(duration.Length > 1)
                        txtDurationMonths.Text = duration[1];

                }

                btnSave.Text                    = Constants.CON_UPDATE_BUTTON_TEXT;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //lblerror.Text = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
        }

        protected void gvHighEdu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvHighEdu, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //lblerror.Text = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
        }

        void clearOnPageIndexChanging()
        {
            txtInstitute.Text = "";
            txtProgram.Text = "";
            txtProgramName.Text = "";
            txtSector.Text = "";
            txtDurationYears.Text = "";
            txtDurationMonths.Text = "";
            if (ddlFromYear.Items.Count > 0)
            {
                ddlFromYear.SelectedIndex = 0;
            }
            if (ddlToYear.Items.Count > 0)
            {
                ddlToYear.SelectedIndex = 0;
            }
            txtGrade.Text = "";
            txtRemarks.Text = "";
            btnSave.Enabled = true;
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
        }

    }
}