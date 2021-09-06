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
    public partial class webFrmSecondaryEducation : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string userID   = "";
        private string keyEmpID = "";
        private string keyRole  = "";
        public bool isSearchable = false;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {
                userID      = Session["KeyUSER_ID"].ToString();
                keyEmpID    = Session["KeyEMPLOYEE_ID"].ToString();
                keyRole     = Session["KeyHRIS_ROLE"].ToString();
            }


            if (!IsPostBack)
            {
                fillYears();
                fillGrades();
                fillAttempts();

                if (Session["KeyUSER_ID"] != null)
                {
                    userID      = Session["KeyUSER_ID"].ToString();
                    keyEmpID    = Session["KeyEMPLOYEE_ID"].ToString();
                    keyRole     = Session["KeyHRIS_ROLE"].ToString();
                  
                }
            }
            else
            {
                txtEmployeeID.Text  = hfEmpID.Value;
                txtName.Text        = hfName.Value;

                //string parameter = Request["__EVENTARGUMENT"];
                populateGrid(false);
                populateGrid(true);
                populateGridObserlete();
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
                if (dhUserAdmin.isDefaultRole(keyRole))
                {
                    txtEmployeeID.Text = keyEmpID;

                    dr = dhEmp.populate(keyEmpID);
                    txtName.Text = dr["FULL_NAME"].ToString();

                    hfEmpID.Value   = keyEmpID;
                    hfName.Value    = txtName.Text;

                    isSearchable = false;

                    //If sign-up user, load the grid with keyempid;
                    populateGridInitial();
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
        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);

            string companyId = "";
            string receiverEMail = "";

            if (customValidate())
            {
                List<SecondaryEducation> SecondaryEducationList = new List<SecondaryEducation>();
                SecondaryEducationDataHandler dhSecEdu;

                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject1OL.Text, ddlGrade1OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject2OL.Text, ddlGrade2OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject3OL.Text, ddlGrade3OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject4OL.Text, ddlGrade4OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject5OL.Text, ddlGrade5OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject6OL.Text, ddlGrade6OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject7OL.Text, ddlGrade7OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject8OL.Text, ddlGrade8OL.SelectedItem.Text));
                //2014-11-05
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject9OL.Text, ddlGrade9OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject10OL.Text, ddlGrade10OL.SelectedItem.Text));


                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtSchoolAL.Text, ddlYearAL.SelectedItem.Text, ddlALAttempt.SelectedItem.Text, true, txtSubject1AL.Text, ddlGrade1AL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtSchoolAL.Text, ddlYearAL.SelectedItem.Text, ddlALAttempt.SelectedItem.Text, true, txtSubject2AL.Text, ddlGrade2AL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtSchoolAL.Text, ddlYearAL.SelectedItem.Text, ddlALAttempt.SelectedItem.Text, true, txtSubject3AL.Text, ddlGrade3AL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtSchoolAL.Text, ddlYearAL.SelectedItem.Text, ddlALAttempt.SelectedItem.Text, true, txtSubject4AL.Text, ddlGrade4AL.SelectedItem.Text));

                try
                {
                    dhSecEdu = new SecondaryEducationDataHandler();

                    if (dhSecEdu.Save(SecondaryEducationList, userID))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_SAVED, lblMsg);

                        //Get Company ID from Emp
                        companyId = getCompanyForEmployee(txtEmployeeID.Text);

                        //Get HR email address for company
                        if (companyId.Trim().Length > 0)
                            receiverEMail = getHREmail(companyId);



                        DataTable dtEmailInfo = new DataTable();
                        HigherEducationDataHandler dhHighEdu = new HigherEducationDataHandler();
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


                        if (receiverEMail != "")
                        {
                            //Send email to HR
                            EmailHandler.SendDefaultEmail(Constants.CON_SYSTEM_NAME, receiverEMail, String.Empty, CommonVariables.EMAIL_SUBJECT_SEC_EDU_ADDED, emailDeatails);
                        }
                    }

                    clear();
                    //populateGrid();

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
                    dhSecEdu = null;
                }

            }
        }


        private StringBuilder getMailBody(string employeeName, string nic, string epf, string company)
        {
            StringBuilder mailBody = new StringBuilder();

            mailBody.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);
            mailBody.Append(employeeName + " has inserted new secondary education qualifications and pending for your verification." + Environment.NewLine + Environment.NewLine);
            mailBody.Append("NIC No. : " + nic + Environment.NewLine + Environment.NewLine);
            mailBody.Append("EPF No. : " + epf + Environment.NewLine + Environment.NewLine);
            mailBody.Append("Company : " + company + Environment.NewLine + Environment.NewLine);
            mailBody.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            mailBody.Append("[This is a system generated mail.]" + Environment.NewLine);

            return mailBody;
        }



        private void clear()
        {
            GroupHRIS.Utility.Utils.clearControls(true, ddlYearOL, ddlOLAttempt, txtScoolOL, 
                                                        txtSubject1OL, txtSubject2OL, txtSubject3OL, txtSubject4OL, txtSubject5OL, txtSubject6OL, txtSubject7OL, txtSubject8OL,txtSubject9OL,txtSubject10OL,
                                                        ddlGrade1OL, ddlGrade2OL, ddlGrade3OL, ddlGrade4OL, ddlGrade5OL, ddlGrade6OL, ddlGrade7OL, ddlGrade8OL, ddlGrade9OL, ddlGrade10OL,
                                                        ddlYearAL, ddlALAttempt, txtSchoolAL, 
                                                        txtSubject1AL, txtSubject2AL, txtSubject3AL, txtSubject4AL, 
                                                        ddlGrade1AL, ddlGrade2AL, ddlGrade3AL, ddlGrade4AL);

        }

        private void clear(Boolean isAl)
        {
            if (isAl == false)
            {
                GroupHRIS.Utility.Utils.clearControls(true, ddlYearOL, ddlOLAttempt, txtScoolOL,
                                                            txtSubject1OL, txtSubject2OL, txtSubject3OL, txtSubject4OL, txtSubject5OL, txtSubject6OL, txtSubject7OL, txtSubject8OL, txtSubject9OL, txtSubject10OL,
                                                            ddlGrade1OL, ddlGrade2OL, ddlGrade3OL, ddlGrade4OL, ddlGrade5OL, ddlGrade6OL, ddlGrade7OL, ddlGrade8OL, ddlGrade9OL, ddlGrade10OL);                                                            
            }
            else if (isAl == true)
            {
                GroupHRIS.Utility.Utils.clearControls(true, ddlYearAL, ddlALAttempt, txtSchoolAL,
                                                           txtSubject1AL, txtSubject2AL, txtSubject3AL, txtSubject4AL,
                                                           ddlGrade1AL, ddlGrade2AL, ddlGrade3AL, ddlGrade4AL);
            }

        }



        private void clearAll()
        {
            clear();

            //Commented due to QA feedback
            /*
            txtEmployeeID.Text = String.Empty;
            txtName.Text = String.Empty;

            gvSecEdu.DataSource = null;
            gvSecEdu.DataBind();
            */
        }

        private void clearAll(Boolean isAl)
        {
            clear(isAl);           
        }



        private bool customValidate()
        {
            bool bRetVal = false;

            if ((txtSubject1OL.Text.Trim().Length > 0) || (txtSubject2OL.Text.Trim().Length > 0) || (txtSubject3OL.Text.Trim().Length > 0) || (txtSubject4OL.Text.Trim().Length > 0) || (txtSubject5OL.Text.Trim().Length > 0) || (txtSubject6OL.Text.Trim().Length > 0) || (txtSubject7OL.Text.Trim().Length > 0) || (txtSubject8OL.Text.Trim().Length > 0))
            {
                if (ddlYearOL.SelectedIndex == 0)
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Year (G.C.E O/L) should not be empty.", lblMsg);
                else if (ddlOLAttempt.SelectedIndex == 0)
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Attempt (G.C.E O/L) should not be empty.", lblMsg);
                else if (txtScoolOL.Text.Trim().Length == 0)
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "School (G.C.E O/L) should not be empty.", lblMsg);
                else
                    bRetVal = true;
            }

            else if ((txtSubject1AL.Text.Trim().Length > 0) || (txtSubject2AL.Text.Trim().Length > 0) || (txtSubject3AL.Text.Trim().Length > 0) || (txtSubject4AL.Text.Trim().Length > 0))
            {
                if (ddlYearAL.SelectedIndex == 0)
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Year (G.C.E A/L) should not be empty.", lblMsg);
                else if (ddlALAttempt.SelectedIndex == 0)
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Attempt (G.C.E A/L) should not be empty.", lblMsg);
                else if (txtSchoolAL.Text.Trim().Length == 0)
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "School (G.C.E A/L) should not be empty.", lblMsg);
                else
                    bRetVal = true;
            }

            else
                bRetVal = true;

            return bRetVal;
        }

        private bool customValidate(Boolean isAl)
        {
            bool bRetVal = false;

            if (isAl == false)
            {
                if ((txtSubject1OL.Text.Trim().Length > 0) || (txtSubject2OL.Text.Trim().Length > 0) || (txtSubject3OL.Text.Trim().Length > 0) || (txtSubject4OL.Text.Trim().Length > 0) || (txtSubject5OL.Text.Trim().Length > 0) || (txtSubject6OL.Text.Trim().Length > 0) || (txtSubject7OL.Text.Trim().Length > 0) || (txtSubject8OL.Text.Trim().Length > 0))
                {
                    if (ddlYearOL.SelectedIndex == 0)
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Year (G.C.E O/L) should not be empty.", lblMsg);
                    else if (ddlOLAttempt.SelectedIndex == 0)
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Attempt (G.C.E O/L) should not be empty.", lblMsg);
                    else if (txtScoolOL.Text.Trim().Length == 0)
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "School (G.C.E O/L) should not be empty.", lblMsg);
                    else
                        bRetVal = true;
                }
            }
            else if(isAl == true)
            {
                if ((txtSubject1AL.Text.Trim().Length > 0) || (txtSubject2AL.Text.Trim().Length > 0) || (txtSubject3AL.Text.Trim().Length > 0) || (txtSubject4AL.Text.Trim().Length > 0))
                {
                    if (ddlYearAL.SelectedIndex == 0)
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Year (G.C.E A/L) should not be empty.", lblMsg);
                    else if (ddlALAttempt.SelectedIndex == 0)
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Attempt (G.C.E A/L) should not be empty.", lblMsg);
                    else if (txtSchoolAL.Text.Trim().Length == 0)
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "School (G.C.E A/L) should not be empty.", lblMsg);
                    else
                        bRetVal = true;
                }
            }
            else
                bRetVal = true;

            return bRetVal;
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);
            clearAll();
        }



        private void fillYears()
        {
            ddlYearOL.Items.Add("");
            ddlYearAL.Items.Add("");

            for (int i = DateTime.Today.Year; i >= (DateTime.Today.Year - Constants.CON_DROP_DOWN_NUM_YEARS); i--)
            {
                ddlYearOL.Items.Add(i.ToString());
                ddlYearAL.Items.Add(i.ToString());
            }
        }



        private void fillGrades()
        {
            ddlGrade1OL.DataSource = Constants.CON_SECONDARY_EDUCATION_GRADES;
            ddlGrade1OL.DataBind();

            ddlGrade2OL.DataSource = Constants.CON_SECONDARY_EDUCATION_GRADES;
            ddlGrade2OL.DataBind();

            ddlGrade3OL.DataSource = Constants.CON_SECONDARY_EDUCATION_GRADES;
            ddlGrade3OL.DataBind();

            ddlGrade4OL.DataSource = Constants.CON_SECONDARY_EDUCATION_GRADES;
            ddlGrade4OL.DataBind();

            ddlGrade5OL.DataSource = Constants.CON_SECONDARY_EDUCATION_GRADES;
            ddlGrade5OL.DataBind();

            ddlGrade6OL.DataSource = Constants.CON_SECONDARY_EDUCATION_GRADES;
            ddlGrade6OL.DataBind();

            ddlGrade7OL.DataSource = Constants.CON_SECONDARY_EDUCATION_GRADES;
            ddlGrade7OL.DataBind();

            ddlGrade8OL.DataSource = Constants.CON_SECONDARY_EDUCATION_GRADES;
            ddlGrade8OL.DataBind();

            ddlGrade9OL.DataSource = Constants.CON_SECONDARY_EDUCATION_GRADES;
            ddlGrade9OL.DataBind();

            ddlGrade10OL.DataSource = Constants.CON_SECONDARY_EDUCATION_GRADES;
            ddlGrade10OL.DataBind();


            ddlGrade1AL.DataSource = Constants.CON_SECONDARY_EDUCATION_GRADES;
            ddlGrade1AL.DataBind();

            ddlGrade2AL.DataSource = Constants.CON_SECONDARY_EDUCATION_GRADES;
            ddlGrade2AL.DataBind();

            ddlGrade3AL.DataSource = Constants.CON_SECONDARY_EDUCATION_GRADES;
            ddlGrade3AL.DataBind();

            ddlGrade4AL.DataSource = Constants.CON_SECONDARY_EDUCATION_GRADES;
            ddlGrade4AL.DataBind();
        }



        private void fillAttempts()
        {
            ddlOLAttempt.Items.Add("");
            ddlALAttempt.Items.Add("");

            for (int i = 1; i <= 3; i++)
            {
                ddlOLAttempt.Items.Add(i.ToString());
                ddlALAttempt.Items.Add(i.ToString());
            }
        }



        private void populate()
        {
            SecondaryEducationDataHandler dhSecEdu = new SecondaryEducationDataHandler();

            DataTable dtSecEdu = new DataTable();

            List<SecondaryEducation> SecondaryEducationList = new List<SecondaryEducation>();
            bool bIsAL = false;

            try
            {
                dtSecEdu = dhSecEdu.populateValid(txtEmployeeID.Text);

                foreach (DataRow row in dtSecEdu.Rows)
                {
                    if (row["IS_AL"].ToString().Equals("Y"))
                        bIsAL = true;

                    SecondaryEducationList.Add(new SecondaryEducation(row["EMPLOYEE_ID"].ToString(), row["SCHOOL"].ToString(), row["ATTEMPTED_YEAR"].ToString(), row["ATTEMPT"].ToString(), bIsAL, row["SUBJECT_NAME"].ToString(), row["GRADE"].ToString()));
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
            finally
            {
                dhSecEdu = null;
                dtSecEdu.Dispose();
            }

        }

        private void populateGridObserlete()
        {
            SecondaryEducationDataHandler dhSecEdu = new SecondaryEducationDataHandler();
            DataTable dtSecEdu = new DataTable();

            try
            {

                //Obsolete Recs
                dtSecEdu = dhSecEdu.populateObsolete(txtEmployeeID.Text);

                gvSecEduObsolete.DataSource = dtSecEdu;
                gvSecEduObsolete.DataBind();


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
            finally
            {
                dhSecEdu = null;
                dtSecEdu.Dispose();
            }
        }


        private void populateGrid(Boolean isAl)
        {
            SecondaryEducationDataHandler dhSecEdu = new SecondaryEducationDataHandler();
            DataTable dtSecEdu = new DataTable();

            try
            {
                dtSecEdu = dhSecEdu.populateValid(txtEmployeeID.Text, isAl);

                if (isAl == true)
                {
                    gvSecEduAL.DataSource = dtSecEdu;
                    gvSecEduAL.DataBind();
                }
                else
                {
                    gvSecEduOL.DataSource = dtSecEdu;
                    gvSecEduOL.DataBind();
                }


                ////Obsolete Recs
                //dtSecEdu = dhSecEdu.populateObsolete(txtEmployeeID.Text);

                //gvSecEduObsolete.DataSource = dtSecEdu;
                //gvSecEduObsolete.DataBind();


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
            finally
            {
                dhSecEdu = null;
                dtSecEdu.Dispose();
            }
        }


        private void populateGridInitial()
        {
            SecondaryEducationDataHandler dhSecEdu = new SecondaryEducationDataHandler();
            DataTable dtSecEdu = new DataTable();

            try
            {
                dtSecEdu = dhSecEdu.populateValid(keyEmpID,false);

                gvSecEduOL.DataSource = dtSecEdu;
                gvSecEduOL.DataBind();

                dtSecEdu.Clear();

                dtSecEdu = dhSecEdu.populateValid(keyEmpID, true);

                gvSecEduAL.DataSource = dtSecEdu;
                gvSecEduAL.DataBind();



                //Obsolete Recs
                dtSecEdu.Clear();

                dtSecEdu = dhSecEdu.populateObsolete(keyEmpID);

                gvSecEduObsolete.DataSource = dtSecEdu;
                gvSecEduObsolete.DataBind();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
            finally
            {
                dhSecEdu = null;
                dtSecEdu.Dispose();
            }
        }


        //protected void gvSecEdu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvSecEdu.PageIndex = e.NewPageIndex;
        //    populateGrid();
        //}


        //protected void gvSecEduObs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvSecEduObsolete.PageIndex = e.NewPageIndex;
        //    populateGrid();
        //}

        protected void btnAlSave_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);

            string companyId = "";
            string receiverEMail = "";

            if (customValidate())
            {
                List<SecondaryEducation> SecondaryEducationList = new List<SecondaryEducation>();
                SecondaryEducationDataHandler dhSecEdu;
                               

                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtSchoolAL.Text, ddlYearAL.SelectedItem.Text, ddlALAttempt.SelectedItem.Text, true, txtSubject1AL.Text, ddlGrade1AL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtSchoolAL.Text, ddlYearAL.SelectedItem.Text, ddlALAttempt.SelectedItem.Text, true, txtSubject2AL.Text, ddlGrade2AL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtSchoolAL.Text, ddlYearAL.SelectedItem.Text, ddlALAttempt.SelectedItem.Text, true, txtSubject3AL.Text, ddlGrade3AL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtSchoolAL.Text, ddlYearAL.SelectedItem.Text, ddlALAttempt.SelectedItem.Text, true, txtSubject4AL.Text, ddlGrade4AL.SelectedItem.Text));

                try
                {
                    dhSecEdu = new SecondaryEducationDataHandler();

                    if (dhSecEdu.Save(SecondaryEducationList, userID))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_SAVED, lblMsg);

                        //Get Company ID from Emp
                        companyId = getCompanyForEmployee(txtEmployeeID.Text);

                        //Get HR email address for company
                        if (companyId.Trim().Length > 0)
                            receiverEMail = getHREmail(companyId);


                        DataTable dtEmailInfo = new DataTable();
                        HigherEducationDataHandler dhHighEdu = new HigherEducationDataHandler();
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

                        if (receiverEMail != "")
                        {
                            //Send email to HR
                            EmailHandler.SendDefaultEmail(Constants.CON_SYSTEM_NAME, receiverEMail, String.Empty, CommonVariables.EMAIL_SUBJECT_SEC_EDU_ADDED + " (A/L)", emailDeatails);
                        }
                    }

                    clear(true);
                    populateGrid(true);

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
                    dhSecEdu = null;
                }

            }
        }

        protected void btnAlCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnOlSave_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);

            string companyId = "";
            string receiverEMail = "";

            if (customValidate(false))
            {
                List<SecondaryEducation> SecondaryEducationList = new List<SecondaryEducation>();
                SecondaryEducationDataHandler dhSecEdu;

                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject1OL.Text, ddlGrade1OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject2OL.Text, ddlGrade2OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject3OL.Text, ddlGrade3OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject4OL.Text, ddlGrade4OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject5OL.Text, ddlGrade5OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject6OL.Text, ddlGrade6OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject7OL.Text, ddlGrade7OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject8OL.Text, ddlGrade8OL.SelectedItem.Text));
                //2014-11-05
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject9OL.Text, ddlGrade9OL.SelectedItem.Text));
                SecondaryEducationList.Add(new SecondaryEducation(txtEmployeeID.Text, txtScoolOL.Text, ddlYearOL.SelectedItem.Text, ddlOLAttempt.SelectedItem.Text, false, txtSubject10OL.Text, ddlGrade10OL.SelectedItem.Text));

                try
                {
                    dhSecEdu = new SecondaryEducationDataHandler();

                    if (dhSecEdu.Save(SecondaryEducationList, userID))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_SAVED, lblMsg);

                        //Get Company ID from Emp
                        companyId = getCompanyForEmployee(txtEmployeeID.Text);

                        //Get HR email address for company
                        if (companyId.Trim().Length > 0)
                            receiverEMail = getHREmail(companyId);


                        DataTable dtEmailInfo = new DataTable();
                        HigherEducationDataHandler dhHighEdu = new HigherEducationDataHandler();
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

                        if (receiverEMail != "")
                        {
                            //Send email to HR
                            EmailHandler.SendDefaultEmail(Constants.CON_SYSTEM_NAME, receiverEMail, String.Empty, CommonVariables.EMAIL_SUBJECT_SEC_EDU_ADDED + " (O/L)", emailDeatails);
                        }
                    }

                    clear(false);
                    populateGrid(false);

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
                    dhSecEdu = null;
                }
            }

        }

        protected void btnOlCancel_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);
            clearAll();
        }

        protected void gvSecEduOL_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSecEduOL.PageIndex = e.NewPageIndex;
            populateGrid(false);
        }

        

        protected void gvSecEduAL_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSecEduOL.PageIndex = e.NewPageIndex;
            populateGrid(true);
        }

       

        protected void gvSecEduObsolete_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSecEduObsolete.PageIndex = e.NewPageIndex;
            populateGridObserlete();
        }

       






    }
}