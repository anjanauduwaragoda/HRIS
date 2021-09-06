using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GroupHRIS.Reports.ReportViewers
{
    public partial class ReportMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                    string KeyUSER_FIRSTNAME = (string)(Session["KeyUSER_FIRSTNAME"]); 
                    lblwelcome.Text = "Welcome ! " + KeyUSER_FIRSTNAME + "   ";
                }
                catch
                {
                    logoutme();
                    Response.Redirect("~/Login/MainLogout.aspx", false);
                }
            }
        }

        protected void imgbtnlogout_Click(object sender, ImageClickEventArgs e)
        {
            logoutme();
            Response.Redirect("~/Login/MainLogout.aspx", false);
        }

        private void logoutme()
        {
            Session["KeyLOGOUT_STS"] = "0";
            Session["KeyEMPLOYEE_ID"] = "";
            Session["KeyUSER_ID"] = "";
            Session["KeyUSER_FIRSTNAME"] = "";
            Session["KeyHRIS_ROLE"] = "";
            Session["KeyCOMP_ID"] = "";
            //Session["KeyAccessData"] = null;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
            {
                Response.Redirect((string)refUrl);
            }
        }

    }
}