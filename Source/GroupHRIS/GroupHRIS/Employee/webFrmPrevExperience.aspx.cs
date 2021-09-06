using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.Employee;
using DataHandler.MetaData;
using DataHandler.UserAdministration;
using Common;
using NLog;
using System.Text;
using DataHandler.Userlogin;

namespace GroupHRIS.Employee
{
    public partial class webFrmPrevExperience : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string userID = "";
        private string keyEmpID = "";
        private string keyRole = "";
        public bool isSearchable = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }

            if (!IsPostBack)
            {
                if (Session["KeyUSER_ID"] != null)
                {
                    userID      = Session["KeyUSER_ID"].ToString();
                    keyEmpID    = Session["KeyEMPLOYEE_ID"].ToString();
                    keyRole = Session["KeyHRIS_ROLE"].ToString();

                    Session["UserRole"] = keyRole;
                    Session["employeeID"] = keyEmpID;

                    txtEmployeeID.Text = keyEmpID;
                    populateGrid();

                    PrevExperienceDataHandler dhPrevExp = new PrevExperienceDataHandler();
                    txtName.Text = dhPrevExp.getEmployeeName(keyEmpID);

                    Session["txtName"] = txtName.Text;                  
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

                        Utility.Errorhandler.ClearError(lblMsg);
                        string employeeID = txtEmployeeID.Text;
                        clearFull();
                        btnSave.Enabled = true;
                        gvPrevEmp.DataSource = null;
                        gvPrevEmp.DataBind();

                        txtEmployeeID.Text = employeeID;
                        PreviousExperienceVerificationDataHandler PEVDH = new PreviousExperienceVerificationDataHandler();
                        txtName.Text = PEVDH.GetKnownNameFromEmployeeID(employeeID);
                        
                        //string parameter = Request["__EVENTARGUMENT"];

                        //if (parameter.Equals("TextChanged"))
                        //    Utility.Errorhandler.ClearError(lblMsg);

                        populateGrid();
                        btnSave.Enabled = true;
                    }
                }
            }

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
                    txtName.Text = empName;

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
            PrevExperienceDataHandler dhPrevExp = new PrevExperienceDataHandler();

            Utility.Errorhandler.ClearError(lblMsg);


            //Check wheather FromDate <= ToDate
            if (!CommonUtils.isValidDateRange(txtFromDate.Text.Trim(), txtToDate.Text.Trim()))
            {
                lblMsg.Text = String.Empty;
                
                //lblerror.Text = "Invalid Date Range. From Date should be a earlier date than the To Date.";
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Invalid Date Range ", lblMsg);
                return;
            }


            try
            {
                userID = Session["KeyUSER_ID"].ToString();


                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    dhPrevExp.Insert(txtEmployeeID.Text.Trim(),
                                    txtOrganization.Text.Trim(),
                                    txtAddress.Text.Trim(),
                                    txtDesignation.Text.Trim(),
                                    txtPhoneNumber.Text.Trim(),
                                    txtFromDate.Text.Trim(),
                                    txtToDate.Text.Trim(),
                                    txtRemarks.Text.Trim(),
                                    userID);

                    

                    //lblMsg.Text = "Record successfully added.";

                    //EMAIL METHODS

                    DataTable dtPrevExp = new DataTable();
                    dtPrevExp = dhPrevExp.getHrEmail(txtEmployeeID.Text.Trim());

                    if (dtPrevExp.Rows.Count > 0)
                    {
                        string HREmailAddress = dtPrevExp.Rows[0]["HR_EMAILS"].ToString();
                        string Title = dtPrevExp.Rows[0]["TITLE"].ToString();
                        string KnownName = dtPrevExp.Rows[0]["KNOWN_NAME"].ToString();
                        if (Title != "")
                        {
                            KnownName = Title + " " + KnownName;
                        }
                        string EmpNic = dtPrevExp.Rows[0]["EMP_NIC"].ToString();
                        string EPF = dtPrevExp.Rows[0]["EPF_NO"].ToString();
                        string CompanyName = dtPrevExp.Rows[0]["COMP_NAME"].ToString();

                        StringBuilder emailDeatails = getMailBody(KnownName, EmpNic, EPF, CompanyName);

                        EmailHandler.SendDefaultEmail(Constants.CON_SYSTEM_NAME, HREmailAddress, String.Empty, CommonVariables.EMAIL_SUBJECT_PRV_EXP_ADDED, emailDeatails);
                    }   


                    clear();
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED, lblMsg);

                    lblerror.Text = String.Empty;
                }

                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    dhPrevExp.Update(txtEmployeeID.Text.Trim(),
                                    hfLineNo.Value,
                                    txtOrganization.Text.Trim(),
                                    txtAddress.Text.Trim(),
                                    txtDesignation.Text.Trim(),
                                    txtPhoneNumber.Text.Trim(),
                                    txtFromDate.Text.Trim(),
                                    txtToDate.Text.Trim(),
                                    txtRemarks.Text.Trim(),
                                    userID);

                    clear();
                    //lblMsg.Text = "Record successfully updated.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED, lblMsg);

                    lblerror.Text = String.Empty;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                dhPrevExp = null;
            }

            populateGrid();
        }

        private StringBuilder getMailBody(string employeeName, string nic, string epf, string company)
        {
            StringBuilder mailBody = new StringBuilder();

            mailBody.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);
            mailBody.Append(employeeName + " has inserted new Previous Experience Record and pending for your verification." + Environment.NewLine + Environment.NewLine);
            mailBody.Append("NIC No. : " + nic + Environment.NewLine + Environment.NewLine);
            mailBody.Append("EPF No. : " + epf + Environment.NewLine + Environment.NewLine);
            mailBody.Append("Company : " + company + Environment.NewLine + Environment.NewLine);
            mailBody.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            mailBody.Append("[This is a system generated mail.]" + Environment.NewLine);

            return mailBody;
        }

        private void clear()
        {
            GroupHRIS.Utility.Utils.clearControls(true, txtOrganization, txtAddress, txtDesignation, txtFromDate, txtToDate, txtRemarks, txtPhoneNumber);
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
        }

        private void clearFull()
        {


            setEmployeeSearchability();
            if (isSearchable == true)
            {
                txtEmployeeID.Text = "";
                txtName.Text = "";
            }

            //txtEmployeeID.Text = "";
            //txtName.Text = "";
            txtOrganization.Text = "";
            txtAddress.Text = "";
            txtDesignation.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtRemarks.Text = "";
            txtPhoneNumber.Text = "";

            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);

            setEmployeeSearchability();
            if (isSearchable == true)
            {
                gvPrevEmp.DataSource = null;
                gvPrevEmp.DataBind();
            }

            clearFull();
            btnSave.Enabled = true;
            
        }

        protected void gvPrevEmp_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);

            try
            {
                txtOrganization.Text    = gvPrevEmp.SelectedRow.Cells[1].Text.removeInvalidHTMLChars();
                txtDesignation.Text     = gvPrevEmp.SelectedRow.Cells[2].Text.removeInvalidHTMLChars();
                txtAddress.Text         = gvPrevEmp.SelectedRow.Cells[3].Text.removeInvalidHTMLChars();
                txtFromDate.Text        = gvPrevEmp.SelectedRow.Cells[4].Text.removeInvalidHTMLChars();
                txtToDate.Text          = gvPrevEmp.SelectedRow.Cells[5].Text.removeInvalidHTMLChars();
                txtRemarks.Text         = gvPrevEmp.SelectedRow.Cells[6].Text.removeInvalidHTMLChars();
                hfLineNo.Value          = gvPrevEmp.SelectedRow.Cells[7].Text.removeInvalidHTMLChars();
                txtPhoneNumber.Text     = gvPrevEmp.SelectedRow.Cells[8].Text.removeInvalidHTMLChars();

                string status = gvPrevEmp.SelectedRow.Cells[9].Text.removeInvalidHTMLChars();

                if (status != Constants.CON_PREVIOUS_EMPLOYMENT_PENDING_TEXT)
                {
                    btnSave.Enabled = false;
                }
                else
                {
                    btnSave.Enabled = true;
                }


                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //lblerror.Text = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
        }

        protected void gvPrevEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPrevEmp.PageIndex = e.NewPageIndex;
            populateGrid();
            clearOnPageIndexChanging();
        }

        protected void gvPrevEmp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvPrevEmp, "Select$" + e.Row.RowIndex.ToString()));
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

        private void populateGrid()
        {
            PrevExperienceDataHandler dhPrevEmp = new PrevExperienceDataHandler();
            DataTable dtPrevEmp = new DataTable();

            try
            {
                dtPrevEmp = dhPrevEmp.populate(txtEmployeeID.Text).Copy();

                for (int i = 0; i < dtPrevEmp.Rows.Count; i++)
                {
                    string status = dtPrevEmp.Rows[i]["RECORD_STATUS"].ToString();

                    if (status == Constants.CON_PREVIOUS_EMPLOYMENT_PENDING_CODE)
                    {
                        dtPrevEmp.Rows[i]["RECORD_STATUS"] = Constants.CON_PREVIOUS_EMPLOYMENT_PENDING_TEXT;
                    }
                    else if (status == Constants.CON_PREVIOUS_EMPLOYMENT_APPROVED_CODE)
                    {
                        dtPrevEmp.Rows[i]["RECORD_STATUS"] = Constants.CON_PREVIOUS_EMPLOYMENT_APPROVED_TEXT;
                    }
                    else if (status == Constants.CON_PREVIOUS_EMPLOYMENT_REJECTED_CODE)
                    {
                        dtPrevEmp.Rows[i]["RECORD_STATUS"] = Constants.CON_PREVIOUS_EMPLOYMENT_REJECTED_TEXT;
                    }
                }

                gvPrevEmp.DataSource = dtPrevEmp;
                gvPrevEmp.DataBind();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //lblerror.Text = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
            finally
            {
                dhPrevEmp = null;
                dtPrevEmp.Dispose();
            }
        }

        void clearOnPageIndexChanging()
        {
            txtOrganization.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtDesignation.Text = "";
            txtPhoneNumber.Text = "";
            txtAddress.Text = "";
            txtRemarks.Text = "";
            btnSave.Enabled = true;
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
        }
    }
}