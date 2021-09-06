using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Employee;
using DataHandler.Userlogin;
using System.Data;
using Common;

namespace GroupHRIS.Login
{
    public partial class Forgotpassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack ==false ){
                lblcopyright.Text = CommonVariables.COPY_RIGHT_TEXT.ToString();
                lnkbutton.Text = CommonVariables.LINK_TEXT.ToString();
            }
        }

        protected void lnkimgregister_Click(object sender, ImageClickEventArgs e)
        {
            EmployeeDataHandler employeedatahandeler = new EmployeeDataHandler();
            PasswordHandler passwordhandler = new PasswordHandler();
            EmailHandler emailhandler = new EmailHandler();
            LoginHandler loginHandler = new LoginHandler();
            DataTable empNic = new DataTable ();
            CommonVariables.MESSAGE_TEXT = "";

            try
            {
                string sUserName = txtuserid.Value.ToString().ToUpper().Trim();
                string sNic = txtnic.Value.ToString().ToUpper().Trim();
                string sEmail = txtemail.Value.ToString().Trim();

                Boolean sValidateEmail = emailhandler.IsValidEmailAddress(sEmail);
                if (sValidateEmail == false)
                {
                    CommonVariables.MESSAGE_TEXT =   " Invalid Email address";

                    if (sUserName == "")
                    {
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_TEXT + " / Invalid user name";
                    }
                    if (sNic == "")
                    {
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_TEXT + " / Invalid NIC";
                    }
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sUserName == "")
                {
                    CommonVariables.MESSAGE_TEXT = " Invalid user name";
                    if (sNic == "")
                    {
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_TEXT + " / Invalid NIC";
                    }
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sNic == "")
                {
                    CommonVariables.MESSAGE_TEXT = " Invalid NIC";
                    if (sUserName == "")
                    {
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_TEXT + " / Invalid user name";
                    }
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {
                    empNic = employeedatahandeler.populateEmpNIc(sNic);

                    if (empNic.Rows.Count > 0)
                    {
                        string sResUsername = empNic.Rows[0][2].ToString().ToUpper().Trim();
                        string sResEmail = empNic.Rows[0][1].ToString().Trim();
                        string madddate = DateTime.Today.ToString("yyyy/MM/dd");

                        if (sUserName != sResUsername)
                        {
                            CommonVariables.MESSAGE_TEXT = "Invalid user name for NIC " + sNic;
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sEmail != sResEmail)
                        {
                            CommonVariables.MESSAGE_TEXT = "Email address not belongs to user " + sUserName;
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {

                            string sNewpassWord = passwordhandler.GenerateNewPassword();
                            string sEncryptpassword = passwordhandler.Encrypt(sNewpassWord);


                            CommonVariables.EMAIL_SUBJECT = Constants.CON_PRODUCT + " Password Reminder email";
                            CommonVariables.EMAIL_BODY = " Your account " + sUserName + " has been Updated. " +
                                                         " Your New Password is " + sNewpassWord;
                            CommonVariables.EMAIL_BODY = CommonVariables.EMAIL_BODY + Environment.NewLine + " This is a system generated mail.";


                            loginHandler.Updatenewpassword(sResUsername, sEncryptpassword, Constants.STATUS_ACTIVE_VALUE, sResUsername, madddate);
                            emailhandler.SendEmailresetPassword(Constants.CON_SENDER, sEmail, CommonVariables.EMAIL_SUBJECT, CommonVariables.EMAIL_BODY);
                            CommonVariables.MESSAGE_TEXT = "Message Sent, Please check your email";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);


                        }
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "Invalid NIC for User Name " + sUserName;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                }

            }
            catch (Exception ex)
            {

                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);

            }
            finally
            {
                employeedatahandeler = null;
                passwordhandler = null;
                passwordhandler = null;
                empNic.Dispose();
                empNic = null;
            }
        }

        protected void lnkimgcancel_Click(object sender, ImageClickEventArgs e)
        {
            txtemail.Value = "";
            txtnic.Value = "";
            txtuserid.Value = "";
            Utility.Errorhandler.ClearError(lblerror);
        }
         
    }
}