using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Userlogin;
using DataHandler.Attendance;
using System.Data;
using System.Web.Mail;
using System.Text;
using System.IO;

namespace GroupHRIS.Attendance
{
    public partial class AttendanceApprove : System.Web.UI.Page
    {

        PasswordHandler cripto = new PasswordHandler();
        AttendanceInAndOutDataHandler attInOut = new AttendanceInAndOutDataHandler();

        string AttLogId = "";
        string empemail = "";

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

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {

                AttLogId = cripto.Decrypt(Request.QueryString["AttLogId"]);
                empemail = cripto.Decrypt(Request.QueryString["EmpMail"]);

                DataTable dtAttInOut = attInOut.getByAttLogId(AttLogId);


                bool Status = attInOut.updateApproved(dtAttInOut, AttLogId,"1");

                if (Status)
                {

                    DataTable dtObsoleted = attInOut.getObsoletedRecords(AttLogId);

                    EmailHandler.SendDefaultEmailHtml("System mail ", empemail, "","Missing In/Out Approvel", getApprovedMessage(dtAttInOut, dtObsoleted));

                   
                    lblerror.Text = "Attendance Approved <br/> Thank You !";
                    btnApprove.Visible = false;
                    btnExit.Visible = false;
                }
                else if(Status== false)
                {
                    lblerror.Text = "Already Approved The Request OR User Have Cancel The Request <br/> Thank You !";
                    btnApprove.Visible = false;
                    btnExit.Visible = false;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private StringBuilder getApprovedMessage(DataTable dtAttInOut ,DataTable dtObsoleted)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DATE");
            dt.Columns.Add("TIME");
            dt.Columns.Add("DIRECTION");            

            for (int i = 0; i < dtAttInOut.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["DATE"] = dtAttInOut.Rows[i]["ATT_DATE"].ToString();
                dr["TIME"] = dtAttInOut.Rows[i]["ATT_TIME"].ToString();
                dr["DIRECTION"] = dtAttInOut.Rows[i]["DIRECTION"].ToString();
                dt.Rows.Add(dr);
            }

            dtAttInOut = new DataTable();
            dtAttInOut = dt;

            StringBuilder stringBuilder = new StringBuilder();

            string var = "<table style='border: 1px solid black;border-collapse: collapse;'>";
            //add header row
            var += "<tr>";
            for (int i = 0; i < dtAttInOut.Columns.Count; i++)
                var += "<th  style='border: 1px solid black;border-collapse: collapse;'>&nbsp;&nbsp;" + dtAttInOut.Columns[i].ColumnName + "</th>";
            var += "</tr>";
            //add rows
            for (int i = 0; i < dtAttInOut.Rows.Count; i++)
            {
                var += "<tr>";
                for (int j = 0; j < dtAttInOut.Columns.Count; j++)
                    var += "<td  style='border: 1px solid black;border-collapse: collapse;'>&nbsp;&nbsp;&nbsp;" + dtAttInOut.Rows[i][j].ToString() + "</td>";
                var += "</tr>";
            }
            var += "</table>";

            //Obsoleted Record table
            DataTable dto = new DataTable();
            dto.Columns.Add("DATE");
            dto.Columns.Add("TIME");
            dto.Columns.Add("DIRECTION");

            for (int i = 0; i < dtObsoleted.Rows.Count; i++)
            {
                DataRow dr = dto.NewRow();
                dr["DATE"] = dtObsoleted.Rows[i]["ATT_DATE"].ToString();
                dr["TIME"] = dtObsoleted.Rows[i]["ATT_TIME"].ToString();
                dr["DIRECTION"] = dtObsoleted.Rows[i]["DIRECTION"].ToString();
                dto.Rows.Add(dr);
            }

            dtObsoleted = new DataTable();
            dtObsoleted = dto;
            string varo = "";
            StringBuilder ostringBuilder = new StringBuilder();

            if (dto.Rows.Count > 0)
            {
                varo = "<table style='border: 1px solid black;border-collapse: collapse;'>";
                //add header row
                varo += "<tr>";
                for (int i = 0; i < dtObsoleted.Columns.Count; i++)
                    varo += "<th  style='border: 1px solid black;border-collapse: collapse;'>&nbsp;&nbsp;" + dtObsoleted.Columns[i].ColumnName + "</th>";
                varo += "</tr>";
                //add rows
                for (int i = 0; i < dtObsoleted.Rows.Count; i++)
                {
                    varo += "<tr>";
                    for (int j = 0; j < dtObsoleted.Columns.Count; j++)
                        varo += "<td  style='border: 1px solid black;border-collapse: collapse;'>&nbsp;&nbsp;&nbsp;" + dtObsoleted.Rows[i][j].ToString() + "</td>";
                    varo += "</tr>";
                }
                varo += "</table>";
            }


            stringBuilder.Append("<br/>Dear Sir/Madam,<br/><br/>" + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("Your fillowing attendance request has been recommended.<br/><br/>" + Environment.NewLine);
            stringBuilder.Append(var + "<br/><br/>" + Environment.NewLine);

            if (dto.Rows.Count > 0)
            {
                stringBuilder.Append("You have been obsoleted following records.<br/><br/>" + Environment.NewLine);
                stringBuilder.Append(varo + "<br/><br/>" + Environment.NewLine);
            }
            stringBuilder.Append("Thank you.<br/><br/>" + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
        }

      

    }
}