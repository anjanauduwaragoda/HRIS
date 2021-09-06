using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Userlogin;
using DataHandler.Employee;
using Common;
using System.Data;

namespace GroupHRIS.Login
{
    public partial class MainLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblcopyright.Text = CommonVariables.COPY_RIGHT_TEXT.ToString();
            lnkbutton.Text = CommonVariables.LINK_TEXT.ToString();
            string backgroundImage = Constants.CON_DEFAULT_BG_IMAGE;
            lnkimgsignin.ImageUrl = "/Images/Login/loginmini.png";
            lnkimgregister.ImageUrl = "/Images/Login/joinnow.png";
            MainLoginBody.Style.Add("background-image", "" + backgroundImage + "");
        }

        protected void lnkimgsignin_Click(object sender, ImageClickEventArgs e)
        {
            string sUserid = txtuserid.Value.ToString().Trim().ToUpper();
            string spassword = txtpasswordlogin.Value.ToString().Trim().ToUpper();
            string savepassword = "";

            LoginHandler mlogin = new LoginHandler();
            PasswordHandler mpasswordHandle = new PasswordHandler();

            DataTable UsersessionDataRow = mlogin.populateUserSessions(sUserid);

            try
            {

                if (UsersessionDataRow.Rows.Count >= 1)
                {
                    savepassword = UsersessionDataRow.Rows[0][2].ToString();

                    if (savepassword == "0")
                    {
                        Session["KeyUSER_ID"] = sUserid;
                        Response.Redirect("Webfrmpassword.aspx", false);
                    }
                    else
                    {
                        string encruptpass = mpasswordHandle.Decrypt(savepassword).ToUpper();
                        if (encruptpass == spassword)
                        {
                            Session["KeyLOGOUT_STS"] = "1";
                            Session["KeyEMPLOYEE_ID"] = UsersessionDataRow.Rows[0][0].ToString();
                            Session["KeyUSER_ID"] = UsersessionDataRow.Rows[0][1].ToString();
                            Session["KeyUSER_FIRSTNAME"] = UsersessionDataRow.Rows[0][3].ToString();
                            Session["KeyHRIS_ROLE"] = UsersessionDataRow.Rows[0][4].ToString();
                            Session["KeyCOMP_ID"] = UsersessionDataRow.Rows[0][5].ToString();
                            Session["KeyEMPLOYEE_GENDER"] = UsersessionDataRow.Rows[0][6].ToString();
                            Response.Redirect("~/Login/Hrismain.aspx", false);
                        }
                        else
                        {
                            Session["KeyLOGOUT_STS"] = "0";
                            Session["KeyEMPLOYEE_ID"] = "";
                            Session["KeyUSER_ID"] = "";
                            Session["KeyUSER_FIRSTNAME"] = "";
                            Session["KeyHRIS_ROLE"] = "";
                            Session["KeyCOMP_ID"] = "";
                            Session["KeyEMPLOYEE_GENDER"] = "";
                            CommonVariables.MESSAGE_TEXT = "Login Failed / Invalid Password !";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);

                        }
                    }
                }
                else if (sUserid == Constants.CON_ADMIN_USER.ToString() && spassword == Constants.CON_ADMIN_PASSSWORD.ToString().ToUpper())
                {
                    Session["KeyLOGOUT_STS"] = "1";
                    Session["KeyEMPLOYEE_ID"] = Constants.CON_ADMIN_KeyEMPLOYEE_ID.ToString();
                    Session["KeyUSER_ID"] = Constants.CON_ADMIN_USER.ToString();
                    Session["KeyUSER_FIRSTNAME"] = Constants.CON_ADMIN_KeyUSER_FIRSTNAME.ToString();
                    Session["KeyHRIS_ROLE"] = Constants.CON_ADMIN_KeyHRIS_ROLE.ToString();
                    Session["KeyCOMP_ID"] = Constants.CON_UNIVERSAL_COMPANY_CODE.ToString();
                    Session["KeyEMPLOYEE_GENDER"] = Constants.CON_ADMIN_KeyEMPLOYEE_GENDER.ToString();
                    Response.Redirect("~/Login/Hrismain.aspx", false);
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Login Failed / Emplyee not Found";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }

            }
            catch (Exception ex)
            {
                Session["KeyLOGOUT_STS"] = "0";
                Session["KeyEMPLOYEE_ID"] = "";
                Session["KeyUSER_ID"] = "";
                Session["KeyUSER_FIRSTNAME"] = "";
                Session["KeyHRIS_ROLE"] = "";
                Session["KeyCOMP_ID"] = "";
                Session["KeyEMPLOYEE_GENDER"] = "";
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                mlogin = null;
                mpasswordHandle = null;
                UsersessionDataRow.Dispose();
                UsersessionDataRow = null;
            }
        }

        protected void lnkimgregister_Click(object sender, ImageClickEventArgs e)
        {

            string sFirstName = txtname.Value.ToString().Trim();
            string sEmpNic = txtnic.Value.ToString().ToUpper().Trim();
            string sEmpNicLastChar = txtnic.Value.ToString().ToUpper().Trim();
            string sPassword = txtpassword.Value.ToString().Trim();

            try
            {
                if (sFirstName == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Invalid Name [ Firstname cannot be  blank ]";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sEmpNic == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Invalid NIC [ NIC cannot be  blank ]";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sEmpNic.Length != 10)
                {
                    CommonVariables.MESSAGE_TEXT = "Invalid NIC [ NIC must contain 10 charachters ]";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sPassword == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Invalid Password [ Password cannot be blank ]";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {
                    sEmpNic = txtnic.Value.ToString().ToUpper().Trim().Substring(0, 9);
                    sEmpNicLastChar = txtnic.Value.ToString().ToUpper().Trim().Substring(9, 1);
                    PasswordHandler passwordhandeler = new PasswordHandler();
                    string EncryptsEmpNic = passwordhandeler.Encrypt(sEmpNic);
                    string EncryptsEmpPassword = passwordhandeler.Encrypt(sPassword);
                    passwordhandeler = null;
                    Session["donkey"] = EncryptsEmpNic;
                    Session["monkey"] = EncryptsEmpPassword;
                    Session["lkey"] = sEmpNicLastChar;
                    //Response.Redirect("/Login/Register.aspx?donkey=" + EncryptsEmpNic + "&monkey=" + EncryptsEmpPassword + "&lkey=" + sEmpNicLastChar, false);
                    Response.Redirect("/Login/Register.aspx", false);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);

            }
        }
    }
}