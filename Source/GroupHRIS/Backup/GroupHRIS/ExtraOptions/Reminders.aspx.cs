using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.Reminders;
using System.Data;

namespace GroupHRIS.ExtraOptions
{
    public partial class Reminders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack){
                get_reminders();
            }
        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
            ReminderDataHandler reminderDataHandler = new ReminderDataHandler();

            try
            {
                string sid = lblcode.Text.ToString().Trim();
                string sRemDate = txtbdate.Text.ToString();
                string sRemiderDescrip = txtdescription.Text.ToString().Trim();
                string sRemStatus = txtstatus.SelectedValue.ToString();
                string mlogUser = (string)(Session["KeyEMPLOYEE_ID"]);

                if(sRemDate == ""){
                     CommonVariables.MESSAGE_TEXT = "Reminder date is invalid.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }else if (sRemiderDescrip == ""){
                     CommonVariables.MESSAGE_TEXT = "Reminder details can not be blank.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }else{

                    if (sid == "")
                    {
                        Boolean isupdated = reminderDataHandler.InsertReminder(sRemDate, sRemiderDescrip, sRemStatus, mlogUser);
                        if (isupdated == true)
                        {
                            get_reminders();
                            CommonVariables.MESSAGE_TEXT = "Reminder successfully saved.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                            clear_text();
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Unable to save reminder";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                    }
                    else
                    {
                        Boolean isupdated = reminderDataHandler.UpdateReminder(sid,sRemDate,sRemiderDescrip, sRemStatus, mlogUser );
                        if (isupdated == true)
                        {
                            get_reminders();
                            CommonVariables.MESSAGE_TEXT = "Reminder successfully updated.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                            clear_text();
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Unable to update reminder";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
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
                reminderDataHandler = null;
            }
        }

        private void get_reminders()
        {
            string mlogUser = (string)(Session["KeyEMPLOYEE_ID"]);
            ReminderDataHandler reminderDataHandler = new ReminderDataHandler();
            string ResCultryDateTime = reminderDataHandler.getCultryDateTime();
            DataTable reminderdata = reminderDataHandler.populateRemindersUser(mlogUser, DateTime.Parse(ResCultryDateTime));

            try
            {
                GridView1.DataSource = reminderdata;
                GridView1.DataBind();

                //if (reminderdata.Rows.Count >= 1)
                //{
                //    GridView1.HeaderRow.Cells[0].Text = "ID No.";
                //    GridView1.HeaderRow.Cells[1].Text = "Expire Date";
                //    GridView1.HeaderRow.Cells[2].Text = "Desription / Reminder";
                //    GridView1.HeaderRow.Cells[0].Width = 50;
                //    GridView1.HeaderRow.Cells[1].Width = 50;
                //    GridView1.HeaderRow.Cells[2].Width = 200;
                //}
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                reminderDataHandler = null;
                reminderdata.Dispose();
                reminderdata = null;
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnupdate.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            lblcode.Text = GridView1.SelectedRow.Cells[0].Text.Trim();
            txtbdate.Text = GridView1.SelectedRow.Cells[1].Text.Trim();
            txtdescription.Text = GridView1.SelectedRow.Cells[2].Text.Trim();
            Utility.Errorhandler.ClearError(lblerror);

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            get_reminders();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.GridView1, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        private void clear_text()
        {
            lblcode.Text = "";
            txtdescription.Text = "";
            txtbdate.Text = "";
            btnupdate.Text = Constants.CON_SAVE_BUTTON_TEXT;
        }

        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblerror);
            clear_text();
        }
    }
}