using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
namespace GroupHRIS.Login
{
    public partial class MainLogout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = "You have Successfully log out !";
                lblcopyright.Text = CommonVariables.COPY_RIGHT_TEXT.ToString();
                lnkbutton.Text = CommonVariables.LINK_TEXT.ToString();
                string backgroundImage = Constants.CON_DEFAULT_BG_IMAGE;
                MainLogOutBody.Style.Add("background-image", "" + backgroundImage + "");
            }
            catch (Exception ex)
            {

                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }
    }
}