using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Userlogin;
using Common;

namespace GroupHRIS.Login
{
    public partial class Webfrmpassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblcopyright.Text = CommonVariables.COPY_RIGHT_TEXT.ToString();
                lnkbutton.Text = CommonVariables.LINK_TEXT.ToString();

                string KeyUSER_ID = (string)(Session["KeyUSER_ID"]);
                if (Session["KeyUSER_ID"] == null)
                {
                    CommonVariables.MESSAGE_TEXT = "You are not allowed to access this , Please Contact HR Deparment ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    lbluserid.Text = "";
                }
                else
                {
                    lbluserid.Text = KeyUSER_ID;
                }
            } 
        }

         
        protected void lnkimgregister_Click(object sender, ImageClickEventArgs e)
        {
            PasswordHandler passwordhandler = new PasswordHandler();
            LoginHandler loginhandler = new LoginHandler();

            try
            {
                string password1  = txtpassword1.Value.ToString();
                string password2 = txtpassword2.Value.ToString();
                string madddate = DateTime.Today.ToString("yyyy/MM/dd");
                string mlogUser = (string)(Session["KeyUSER_ID"]);

                if (password1.ToUpper().Trim() == "" || password2.ToUpper().Trim() == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Password cannot be blank !";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (password1.ToUpper().Trim() == password2.ToUpper().Trim())
                {
                    string newpassword = passwordhandler.Encrypt(password1.ToUpper().Trim());
                    if (newpassword != "")
                    {
                        Boolean newlogin = loginhandler.Updatenewpassword(mlogUser, newpassword, Constants.STATUS_ACTIVE_VALUE, mlogUser, madddate);
                        CommonVariables.MESSAGE_TEXT = "Password Successfully Updated !";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        Response.Redirect("MainLogin.aspx", false);
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "Password Encription Invalid, Please contact Administrator";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Invalid Password / Password Not Equal !";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }

                
            }
            catch (Exception ex)
            {
                passwordhandler = null;
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally {
                passwordhandler = null;
                loginhandler = null;
            }
        }

        protected void lnkimgcancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblerror.Text  = "";
                lblerror.Visible= false ;
                txtpassword1.Value = "";
                txtpassword2.Value = "";
                txtpassword1.Focus();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

    }
}