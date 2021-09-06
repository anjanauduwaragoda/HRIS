using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Userlogin;
using DataHandler.Attendance;
using System.Data;
using System.Text;

namespace GroupHRIS.Attendance
{
    public partial class AttendanceReject : System.Web.UI.Page
    {

        PasswordHandler cripto = new PasswordHandler();
        AttendanceInAndOutDataHandler attInOut = new AttendanceInAndOutDataHandler();
        string empemail = "";
        string AttLogId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AttLogId = cripto.Decrypt(Request.QueryString["AttLogId"]);

                DataTable dtAttInOut = attInOut.getByAttLogId(AttLogId);

                //bool Status = attInOut.updateApproved(dtAttInOut, AttLogId, "1");
                if (dtAttInOut.Rows.Count > 0)
                {
                    grdpending.DataSource = dtAttInOut;
                    grdpending.DataBind();
                }
                else
                {
                    grdpending.DataSource = null;
                    grdpending.DataBind();
                    lblPending.Visible = false;
                }

                DataTable dtObsoleted = attInOut.getObsoletedRecords(AttLogId);

                if (dtObsoleted.Rows.Count > 0)
                {
                    grdObsolete.DataSource = dtObsoleted;
                    grdObsolete.DataBind();
                }
                else
                {
                    grdObsolete.DataSource = null;
                    grdObsolete.DataBind();
                    lblObsoleted.Visible = false;
                }

            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                empemail = cripto.Decrypt(Request.QueryString["EmpMail"]);
                AttLogId = cripto.Decrypt(Request.QueryString["AttLogId"]);

                DataTable dtAttInOut = attInOut.getByAttLogId(AttLogId);

                bool Status = attInOut.updateApproved(dtAttInOut, AttLogId, "9");

                if (Status)
                {
                    EmailHandler.SendDefaultEmailHtml("System mail ", empemail, "", "In/Out Rejection", getDiscardedMessage());
                    lblerror.Text = "Not Approved!";
                    btnReject.Visible = false;
                }
                else if (Status == false)
                {
                    lblerror.Text = "Already Approved The Request OR User Have Cancel The Request <br/> Thank You !";
                    btnReject.Visible = false;
                    btnExit.Visible = false;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
           
      
        }

        private StringBuilder getDiscardedMessage()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("<br/>Dear Sir/Madam,<br/><br/>" + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("Your attendance request has been discarded.<br/><br/>" + Environment.NewLine);
            stringBuilder.Append("Thank you.<br/><br/>" + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
        }
    }
}