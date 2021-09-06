using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Employee ;
using DataHandler.UserAdministration;
using System.Data;
using NLog;
using Common;
//using HrisMail;
using DataHandler.Userlogin;

namespace GroupHRIS.Login
{
    public partial class Register : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
          
        protected void Page_Load(object sender, EventArgs e)
        {
            log.Debug("Page_Load");

            if(!IsPostBack ){
                lblstsmessage.Text = "";
                lblcopyright.Text = CommonVariables.COPY_RIGHT_TEXT;
                lnkbutton.Text = CommonVariables.LINK_TEXT;
                //string eEmpNic = Request.QueryString["donkey"].ToString().Trim();
                string eEmpNic = Session["donkey"].ToString().Trim();


                PasswordHandler passwordhandeler = new PasswordHandler();
                string DecryptsEmpNic = passwordhandeler.Decrypt(eEmpNic) + Session["lkey"].ToString().Trim();

                passwordhandeler = null;
                get_empdetails(DecryptsEmpNic);
            }
        }

        private void get_empdetails(string sEmpNic)
        {
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            DataTable DTemployee = new DataTable();
            try
            {
                DTemployee  = employeeDataHandler.populateEmpNIcName(sEmpNic, "1");
                if (DTemployee.Rows.Count > 0)
                {
                    lblemployeeid.Text = DTemployee.Rows[0][0].ToString();
                    lblnic.Text = DTemployee.Rows[0][1].ToString();
                    lblepfno.Text = DTemployee.Rows[0][2].ToString();
                    lblname.Text = DTemployee.Rows[0][3].ToString();
                    lblemail.Text = DTemployee.Rows[0][4].ToString();
                    lbluserid.Text = DTemployee.Rows[0][5].ToString();
                    lblcompanycode.Text = DTemployee.Rows[0][6].ToString();

                }
                else
                {
                    lblemployeeid.Text = "";
                    lblnic.Text = "";
                    lblepfno.Text = "";
                    lblname.Text = "";
                    lblemail.Text = "";
                    lbluserid.Text = "";
                    lblstsmessage.Text = "";
                    lblcompanycode.Text = "";
                    CommonVariables.MESSAGE_TEXT = "NIC Details not Found [ Invalid NIC ]";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError( CommonVariables.MESSAGE_ERROR,CommonVariables.MESSAGE_TEXT , lblerror);
            }
            finally
            {
                employeeDataHandler = null;
                DTemployee.Dispose();
                DTemployee = null;
            }
        }

        protected void lnkbtnsendreq_Click(object sender, ImageClickEventArgs e)
        {
            UserAdministrationHandler userAdministrationHandler = new UserAdministrationHandler();
            EmailHandler emailhandler = new EmailHandler();
            DataTable userrole = new DataTable();
            DataTable Hrisuser = new DataTable();

            try
            {
                //string eEmpPass = Request.QueryString["monkey"];
                string eEmpPass = Session["monkey"].ToString().Trim();
                string mCompanyID = lblcompanycode.Text.ToString().Trim().ToUpper();
                string mEmployeeId = lblemployeeid.Text.ToString().Trim().ToUpper();
                string mUserid = txtuserid.Text.ToString().Trim().ToUpper();
                string mfirstName = txtfiratname.Text.Trim().ToString();
                string mLastName = txtlastname.Text.Trim().ToString();
                string mSendeMail = txtemail.Text.Trim().ToString().Trim();
                string madddate = DateTime.Today.ToString("yyyy/MM/dd");
                string mlogUser = mfirstName;


                if (lbluserid.Text.Trim() != "")
                {
                    CommonVariables.MESSAGE_TEXT = " You have an User ID , cannot register again. ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {

                    if (mUserid.Trim() == "")
                    {
                        CommonVariables.MESSAGE_TEXT = "User ID cannot be  blank.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else if (mfirstName.Trim() == "")
                    {
                        CommonVariables.MESSAGE_TEXT = "First Name cannot be  blank.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else if (mLastName.Trim() == "")
                    {
                        CommonVariables.MESSAGE_TEXT = "Last Name cannot be  blank.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else if (mSendeMail.Trim() == "")
                    {
                        CommonVariables.MESSAGE_TEXT = "Email address cannot be  blank.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {

                        Boolean sValidateEmail = emailhandler.IsValidEmailAddress(mSendeMail);
                        if (sValidateEmail == false)
                        {
                            CommonVariables.MESSAGE_TEXT = "Invalid email address";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);

                        }else {

                                string sHrisuser = txtuserid.Text.ToString().Trim().ToUpper();
                                Hrisuser = userAdministrationHandler.populateHrisUser(sHrisuser);
                                if (Hrisuser.Rows.Count > 0)
                                {
                                    CommonVariables.MESSAGE_TEXT = "User ID already exist , Please Use a different [ User ID ]";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                                }
                                else
                                {

                                    userrole = userAdministrationHandler.populateuserRolesDefault();
                                    if (userrole.Rows.Count > 0)
                                    {
                                        string commonUserrole = userrole.Rows[0]["ROLE_ID"].ToString();
                                        CommonVariables.COMMON_TEXT = "User Registration";

                                        Boolean blInserted = userAdministrationHandler.InsertHrisUser(mCompanyID, eEmpPass, mEmployeeId, mUserid, mfirstName, mLastName, "1", mlogUser, madddate);
                                        if (blInserted == true)
                                        {
                                            Boolean blInsertedRole = userAdministrationHandler.InsertHrisUserRole(mUserid, commonUserrole, CommonVariables.COMMON_TEXT, "1", mlogUser, madddate);

                                            CommonVariables.EMAIL_SUBJECT = Constants.CON_PRODUCT + " Registration Confirmation ";
                                            CommonVariables.EMAIL_BODY = " Thank you for registering with " + Constants.CON_PRODUCT +
                                                                         " Your registration has been received and Your account " + mUserid + " has been created. ";
                                            CommonVariables.EMAIL_BODY = CommonVariables.EMAIL_BODY + Environment.NewLine + " This is a system generated mail.";
                                            emailhandler.SendRegisterEmail(Constants.CON_SENDER, mSendeMail, CommonVariables.EMAIL_SUBJECT, CommonVariables.EMAIL_BODY);
                                            CommonVariables.MESSAGE_TEXT = "User ID " + sHrisuser + " Successfully Registered !";
                                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                                            CommonVariables.EMAIL_BODY = "";
                                            CommonVariables.EMAIL_BODY = " Thank you for registering with " + Constants.CON_PRODUCT +
                                                                         " Your registration has been received and Your account " + mUserid + " has been created. ";
                                            lblstsmessage.Text = CommonVariables.EMAIL_BODY;
                                        }
                                    }
                                    else
                                    {
                                        CommonVariables.MESSAGE_TEXT = "Common User Role not available, please contact Administrator. ";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);

                                    }
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                userAdministrationHandler = null;
                Hrisuser.Dispose();
                userrole.Dispose();
                Hrisuser = null;
                userrole = null;
            }
        }

    }
}