using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.Userlogin;

namespace GroupHRIS
{
    public partial class Changepassword : System.Web.UI.Page
    {
    

        protected void imgbtncancel_Click(object sender, ImageClickEventArgs e)
        {
            txtpassword1.Value = "";
            txtpassword2.Value = "";
            Utility.Errorhandler.ClearError(lblerror);
        }

        protected void imgbtnupdate_Click(object sender, ImageClickEventArgs e)
        {

            PasswordHandler passwordhandler = new PasswordHandler();
            LoginHandler loginhandler = new LoginHandler();

            try
            {
                string password1 = txtpassword1.Value.ToString();
                string password2 = txtpassword2.Value.ToString();
                string madddate = DateTime.Today.ToString("yyyy/MM/dd");
                string mlogUser = (string)(Session["KeyUSER_ID"]);

                if (password1.ToUpper().Trim() == "" || password2.ToUpper().Trim() == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Password cannot be  blank !";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (password1.ToUpper().Trim() == password2.ToUpper().Trim())
                {
                    string newpassword = passwordhandler.Encrypt(password1.ToUpper().Trim());
                    if (newpassword != "")
                    {
                        Boolean newlogin = loginhandler.Updatenewpassword(mlogUser, newpassword, Constants.STATUS_ACTIVE_VALUE, mlogUser, madddate);
                        CommonVariables.MESSAGE_TEXT = "Password Successfully Changed !";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "Password Encription Invalid, Please contact Administrator";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Invalid Passwords / Passwords Not Equal !";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }


            }
            catch (Exception ex)
            {
                passwordhandler = null;
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                passwordhandler = null;
                loginhandler = null;
            }


        }
    }
}