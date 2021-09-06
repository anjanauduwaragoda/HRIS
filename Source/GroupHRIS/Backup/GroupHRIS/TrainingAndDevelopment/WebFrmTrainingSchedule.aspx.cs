using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.TrainingAndDevelopment;
using System.Data;
using NLog;
using System.Globalization;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainingSchedule : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sIPAddress = Request.UserHostAddress;
                log.Debug("IP:" + sIPAddress + "WebFrmTrainingSchedule : Page_Load");

                if (!IsPostBack)
                {
                    LoadTrainingLocations();

                    LoadHours(ddlPlannedFromTimeHrs);
                    LoadHours(ddlPlannedToTimeHrs);
                    LoadHours(ddlActualFromTimeHours);
                    LoadHours(ddlActualToTimeHours);

                    LoadMinutes(ddlPlannedFromTimeMinutes);
                    LoadMinutes(ddlPlannedToTimeMins);
                    LoadMinutes(ddlActualFromTimeTimeMins);
                    LoadMinutes(ddlActualToTimeMins);

                    LoadStatus();
                }
                else
                {
                    if (hfCaller.Value == "txtTrainingID")
                    {
                        hfCaller.Value = "";
                        if (hfVal.Value != "")
                        {
                            txtTrainingID.Text = hfVal.Value;
                        }
                        if (txtTrainingID.Text != "")
                        {
                            //Postback Methods
                            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                            LoadGrid();
                            LoadTrainers(txtTrainingID.Text.Trim());
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                log.Error(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            TrainingScheduleDataHandler TSDH = new TrainingScheduleDataHandler();
            try
            {
                log.Debug("btnSave_Click");

                string TrainingID = txtTrainingID.Text.Trim();

                string PlannedScheduledDate = txtPlannedlDate.Text.Trim();
                DateTime PDate = System.DateTime.Now;
                if (PlannedScheduledDate != String.Empty)
                {
                    PDate = DateTime.ParseExact(PlannedScheduledDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    PlannedScheduledDate = Convert.ToDateTime(PDate).ToString("yyyy-MM-dd");
                }

                string ActualDate = txtActualDate.Text.Trim();
                DateTime ADate = System.DateTime.Now;
                if (ActualDate != String.Empty)
                {
                    ADate = DateTime.ParseExact(ActualDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    ActualDate = Convert.ToDateTime(ADate).ToString("yyyy-MM-dd");
                }

                string PlannedFromTime = ddlPlannedFromTimeHrs.SelectedValue.ToString().Trim() + ":" + ddlPlannedFromTimeMinutes.SelectedValue.ToString().Trim();
                if (PlannedFromTime.Length == 1) { PlannedFromTime = String.Empty; }

                string PlannedToTime = ddlPlannedToTimeHrs.SelectedValue.ToString().Trim() + ":" + ddlPlannedToTimeMins.SelectedValue.ToString().Trim();
                if (PlannedToTime.Length == 1) { PlannedToTime = String.Empty; }

                if ((PlannedFromTime.Length > 1) && (PlannedToTime.Length > 1))
                {
                    DateTime FromTime = DateTime.ParseExact(PlannedFromTime, "HH:mm", CultureInfo.InvariantCulture);
                    DateTime ToTime = DateTime.ParseExact(PlannedToTime, "HH:mm", CultureInfo.InvariantCulture);

                    if (FromTime > ToTime)
                    {
                        CommonVariables.MESSAGE_TEXT = "Planned from time should be less than the planned to time";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }
                }

                string LocationID = ddlLocation.SelectedValue.ToString().Trim();
                string ActualFromTime = ddlActualFromTimeHours.SelectedValue.ToString().Trim() + ":" + ddlActualFromTimeTimeMins.SelectedValue.ToString().Trim();
                if (ActualFromTime.Length == 1) { ActualFromTime = String.Empty; }

                string ActualToTime = ddlActualToTimeHours.SelectedValue.ToString().Trim() + ":" + ddlActualToTimeMins.SelectedValue.ToString().Trim();
                if (ActualToTime.Length == 1) { ActualToTime = String.Empty; }

                if ((ActualFromTime.Length > 1) && (ActualToTime.Length > 1))
                {
                    DateTime FromTime = DateTime.ParseExact(ActualFromTime, "HH:mm", CultureInfo.InvariantCulture);
                    DateTime ToTime = DateTime.ParseExact(ActualToTime, "HH:mm", CultureInfo.InvariantCulture);

                    if (FromTime > ToTime)
                    {
                        CommonVariables.MESSAGE_TEXT = "Actual from time should be less than the actual to time";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }
                }

                string StatusCode = ddlSatusCode.SelectedValue.ToString().Trim();
                string LoggedUser = (Session["KeyEMPLOYEE_ID"] as string).Trim();

                string TrainerID = ddlTrainer.SelectedValue.Trim();

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    if (isValidRecord(TrainingID, PlannedScheduledDate, PlannedFromTime, PlannedToTime, LocationID, true, null))
                    {
                        TSDH.Insert(TrainingID, PlannedScheduledDate, ActualDate, PlannedFromTime, PlannedToTime, LocationID, ActualFromTime, ActualToTime, StatusCode, LoggedUser, TrainerID);
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "Record already exists";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }

                    Clear();
                    LoadGrid();
                    CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMessage);
                }
                else if(btnSave.Text== Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string RecordID = HttpUtility.HtmlDecode(grdvTrainingSchedule.Rows[grdvTrainingSchedule.SelectedIndex].Cells[0].Text).Trim();

                    if (isValidRecord(TrainingID, PlannedScheduledDate, PlannedFromTime, PlannedToTime, LocationID, false, RecordID))
                    {
                        TSDH.Update(TrainingID, PlannedScheduledDate, ActualDate, PlannedFromTime, PlannedToTime, LocationID, ActualFromTime, ActualToTime, StatusCode, LoggedUser, RecordID, TrainerID);
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "Training schedule is overlap with currenty exist schedule.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }

                    Clear();
                    LoadGrid();
                    CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMessage);
                }

            }
            catch (Exception exp)
            {
                log.Error(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TSDH = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        protected void grdvTrainingSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {


                    string PlannedFromTime = HttpUtility.HtmlDecode(e.Row.Cells[6].Text.Trim());
                    if (PlannedFromTime.Length > 1) { string[] arr = PlannedFromTime.Split(':'); PlannedFromTime = arr[0] + ":" + arr[1]; e.Row.Cells[6].Text = PlannedFromTime; }

                    string PlannedToTime = HttpUtility.HtmlDecode(e.Row.Cells[7].Text.Trim());
                    if (PlannedToTime.Length > 1) { string[] arr = PlannedToTime.Split(':'); PlannedToTime = arr[0] + ":" + arr[1]; e.Row.Cells[7].Text = PlannedToTime; }

                    string ActualFromTime = HttpUtility.HtmlDecode(e.Row.Cells[8].Text.Trim());
                    if (ActualFromTime.Length > 1) { string[] arr = ActualFromTime.Split(':'); ActualFromTime = arr[0] + ":" + arr[1]; e.Row.Cells[8].Text = ActualFromTime; }

                    string ActualToTime = HttpUtility.HtmlDecode(e.Row.Cells[9].Text.Trim());
                    if (ActualToTime.Length > 1) { string[] arr = ActualToTime.Split(':'); ActualToTime = arr[0] + ":" + arr[1]; e.Row.Cells[9].Text = ActualToTime; }



                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdvTrainingSchedule, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdvTrainingSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

                string TrainingID = HttpUtility.HtmlDecode(grdvTrainingSchedule.Rows[grdvTrainingSchedule.SelectedIndex].Cells[1].Text).Trim();
                string PlannedScheduledDate = HttpUtility.HtmlDecode(grdvTrainingSchedule.Rows[grdvTrainingSchedule.SelectedIndex].Cells[3].Text).Trim();
                string ActualDate = HttpUtility.HtmlDecode(grdvTrainingSchedule.Rows[grdvTrainingSchedule.SelectedIndex].Cells[4].Text).Trim();
                string PlannedFromTime = HttpUtility.HtmlDecode(grdvTrainingSchedule.Rows[grdvTrainingSchedule.SelectedIndex].Cells[6].Text).Trim();
                string PlannedToTime = HttpUtility.HtmlDecode(grdvTrainingSchedule.Rows[grdvTrainingSchedule.SelectedIndex].Cells[7].Text).Trim();
                string Location = HttpUtility.HtmlDecode(grdvTrainingSchedule.Rows[grdvTrainingSchedule.SelectedIndex].Cells[5].Text).Trim();
                string ActualFromTime = HttpUtility.HtmlDecode(grdvTrainingSchedule.Rows[grdvTrainingSchedule.SelectedIndex].Cells[8].Text).Trim();
                string ActualToTime = HttpUtility.HtmlDecode(grdvTrainingSchedule.Rows[grdvTrainingSchedule.SelectedIndex].Cells[9].Text).Trim();
                string Status = HttpUtility.HtmlDecode(grdvTrainingSchedule.Rows[grdvTrainingSchedule.SelectedIndex].Cells[10].Text).Trim();
                string TrainerID = HttpUtility.HtmlDecode(grdvTrainingSchedule.Rows[grdvTrainingSchedule.SelectedIndex].Cells[11].Text).Trim();


                txtTrainingID.Text = TrainingID.Trim();
                txtPlannedlDate.Text = PlannedScheduledDate.Trim();
                txtActualDate.Text = ActualDate.Trim();

                if (ddlTrainer.Items.Count > 0)
                {
                    if (TrainerID != "")
                    {
                        ddlTrainer.SelectedIndex = ddlTrainer.Items.IndexOf(ddlTrainer.Items.FindByValue(TrainerID));
                    }
                }

                if (PlannedFromTime.Length > 1)
                {
                    string[] PlannedFromTimeArr = PlannedFromTime.Split(':');
                    ddlPlannedFromTimeHrs.SelectedIndex = ddlPlannedFromTimeHrs.Items.IndexOf(ddlPlannedFromTimeHrs.Items.FindByValue(PlannedFromTimeArr[0]));
                    ddlPlannedFromTimeMinutes.SelectedIndex = ddlPlannedFromTimeMinutes.Items.IndexOf(ddlPlannedFromTimeMinutes.Items.FindByValue(PlannedFromTimeArr[1]));
                }
                else
                {
                    ddlPlannedFromTimeHrs.SelectedIndex = 0;
                    ddlPlannedFromTimeMinutes.SelectedIndex = 0;
                }

                if (PlannedToTime.Length > 1)
                {
                    string[] PlannedToTimeArr = PlannedToTime.Split(':');
                    ddlPlannedToTimeHrs.SelectedIndex = ddlPlannedToTimeHrs.Items.IndexOf(ddlPlannedToTimeHrs.Items.FindByValue(PlannedToTimeArr[0]));
                    ddlPlannedToTimeMins.SelectedIndex = ddlPlannedToTimeMins.Items.IndexOf(ddlPlannedToTimeMins.Items.FindByValue(PlannedToTimeArr[1]));
                }
                else
                {
                    ddlPlannedToTimeHrs.SelectedIndex = 0;
                    ddlPlannedToTimeMins.SelectedIndex = 0;
                }

                ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByText(Location));

                if (ActualFromTime.Length > 1)
                {
                    string[] ActualFromTimeArr = ActualFromTime.Split(':');
                    ddlActualFromTimeHours.SelectedIndex = ddlActualFromTimeHours.Items.IndexOf(ddlActualFromTimeHours.Items.FindByValue(ActualFromTimeArr[0]));
                    ddlActualFromTimeTimeMins.SelectedIndex = ddlActualFromTimeTimeMins.Items.IndexOf(ddlActualFromTimeTimeMins.Items.FindByValue(ActualFromTimeArr[1]));
                }
                else
                {
                    ddlActualFromTimeHours.SelectedIndex = 0;
                    ddlActualFromTimeTimeMins.SelectedIndex = 0;
                }

                if (ActualToTime.Length > 1)
                {
                    string[] ActualToTimeArr = ActualToTime.Split(':');
                    ddlActualToTimeHours.SelectedIndex = ddlActualToTimeHours.Items.IndexOf(ddlActualToTimeHours.Items.FindByValue(ActualToTimeArr[0]));
                    ddlActualToTimeMins.SelectedIndex = ddlActualToTimeMins.Items.IndexOf(ddlActualToTimeMins.Items.FindByValue(ActualToTimeArr[1]));
                }
                else
                {
                    ddlActualToTimeHours.SelectedIndex = 0;
                    ddlActualToTimeMins.SelectedIndex = 0;
                }

                ddlSatusCode.SelectedIndex = ddlSatusCode.Items.IndexOf(ddlSatusCode.Items.FindByText(Status));
            }
            catch (Exception exp)
            {
                log.Error(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                
            }
        }

        protected void grdvTrainingSchedule_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdvTrainingSchedule.PageIndex = e.NewPageIndex;
                LoadGrid();
            }
            catch (Exception exp)
            {
                log.Error(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
            
        }

        void LoadTrainingLocations()
        {
            TrainingScheduleDataHandler TSDH = new TrainingScheduleDataHandler();
            DataTable dtTrainingLocations = new DataTable();
            try
            {
                dtTrainingLocations = TSDH.PopulateTrainingLocations().Copy();
                if (dtTrainingLocations.Rows.Count > 0)
                {
                    ddlLocation.Items.Add(new ListItem("", ""));

                    for (int i = 0; i < dtTrainingLocations.Rows.Count; i++)
                    {
                        string Value = dtTrainingLocations.Rows[i]["LOCATION_ID"].ToString();
                        string Text = dtTrainingLocations.Rows[i]["LOCATION_NAME"].ToString();
                        ddlLocation.Items.Add(new ListItem(Text, Value));
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                dtTrainingLocations.Dispose();
                TSDH = null;
            }
        }

        void LoadMinutes(DropDownList ddlMins)
        {
            try
            {
                log.Debug("LoadMinutes()");

                ddlMins.Items.Clear();
                ddlMins.Items.Add(new ListItem("", ""));
                for (int i = 0; i < 60; i++)
                {
                    ddlMins.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
                }
            }
            catch (Exception ex)
            {
                log.Error("LoadMinutes()" + ex.Message);
                throw ex;
            }
            finally
            {

            }

        }

        void LoadHours(DropDownList ddlHrs)
        {
            try
            {
                log.Debug("LoadHours()");

                ddlHrs.Items.Clear();
                ddlHrs.Items.Add(new ListItem("", ""));
                for (int i = 0; i < 24; i++)
                {
                    ddlHrs.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
                }
            }
            catch (Exception ex)
            {
                log.Error("LoadHours()" + ex.Message);
                throw ex;
            }
            finally
            {

            }

        }

        void LoadStatus()
        {
            try
            {
                log.Debug("LoadStatus()");

                ddlSatusCode.Items.Add(new ListItem("", ""));
                ddlSatusCode.Items.Add(new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                ddlSatusCode.Items.Add(new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));
            }
            catch (Exception ex)
            {
                log.Error("LoadStatus()" + ex.Message);
                throw ex;
            }
            finally
            { 
                
            }
        }

        void LoadGrid()
        {
            TrainingScheduleDataHandler TSDH = new TrainingScheduleDataHandler();
            DataTable dtTrainingSchedules = new DataTable();
            try
            {
                log.Debug("LoadGrid()");
                string TrainingID = txtTrainingID.Text.Trim();
                dtTrainingSchedules = TSDH.Populate(TrainingID).Copy();
                grdvTrainingSchedule.DataSource = dtTrainingSchedules.Copy();
                grdvTrainingSchedule.DataBind();
            }
            catch (Exception ex)
            {
                log.Error("LoadGrid()" + ex.Message);
                throw ex;
            }
            finally
            {
                dtTrainingSchedules.Dispose();
                TSDH = null;
            }
        }

        void LoadTrainers(string TrainingID)
        {
            TrainingScheduleDataHandler TSDH = new TrainingScheduleDataHandler();
            DataTable dtTrainers = new DataTable();
            try
            {
                dtTrainers = TSDH.PopulateTrainers(TrainingID).Copy();
                if (dtTrainers.Rows.Count > 0)
                {
                    ddlTrainer.Items.Add(new ListItem("", ""));

                    for (int i = 0; i < dtTrainers.Rows.Count; i++)
                    {
                        string Value = dtTrainers.Rows[i]["TRAINER_ID"].ToString();
                        string Text = dtTrainers.Rows[i]["NAME_WITH_INITIALS"].ToString();
                        ddlTrainer.Items.Add(new ListItem(Text, Value));
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                dtTrainers.Dispose();
                TSDH = null;
            }
        }

        void ClearAll()
        {
            try
            {
                log.Debug("ClearAll()");

                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;

                Utility.Errorhandler.ClearError(lblMessage);
                ddlTrainer.Items.Clear();
                
                Utility.Utils.clearControls(true, txtTrainingID, ddlTrainer, txtPlannedlDate, txtActualDate, ddlPlannedFromTimeHrs, ddlPlannedFromTimeMinutes, ddlPlannedToTimeHrs, ddlPlannedToTimeMins, ddlLocation, ddlActualFromTimeHours, ddlActualFromTimeTimeMins, ddlActualToTimeHours, ddlActualToTimeMins, ddlSatusCode);
                
                grdvTrainingSchedule.DataSource = null;
                grdvTrainingSchedule.DataBind();
            }
            catch (Exception ex)
            {
                log.Error("ClearAll()" + ex.Message);
                throw ex;
            }
            finally
            { 
            
            }
        }

        void Clear()
        {
            try
            {
                log.Debug("Clear()");

                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;

                Utility.Errorhandler.ClearError(lblMessage);

                Utility.Utils.clearControls(true, txtPlannedlDate, ddlTrainer, txtActualDate, ddlPlannedFromTimeHrs, ddlPlannedFromTimeMinutes, ddlPlannedToTimeHrs, ddlPlannedToTimeMins, ddlLocation, ddlActualFromTimeHours, ddlActualFromTimeTimeMins, ddlActualToTimeHours, ddlActualToTimeMins, ddlSatusCode);
            }
            catch (Exception ex)
            {
                log.Error("Clear()" + ex.Message);
                throw ex;
            }
            finally
            {

            }
        }

        Boolean isValidRecord(string TrainingID, string PlannedScheduledDate, string PlannedFromTime, string PlannedToTime, string LocationID, Boolean isInsert, string RecordID)
        {
            Boolean Status = false;
            TrainingScheduleDataHandler TSDH = new TrainingScheduleDataHandler();
            DataTable dtTrainingSchedules = new DataTable();
            try
            {
                if (isInsert)
                {
                    dtTrainingSchedules = TSDH.PopulateDuplicateRecords(TrainingID, PlannedScheduledDate, PlannedFromTime, PlannedToTime, LocationID).Copy();
                    if (dtTrainingSchedules.Rows.Count > 0)
                    {
                        Status = false;
                    }
                    else
                    {
                        Status = true;
                    }
                }
                else
                {
                    dtTrainingSchedules = TSDH.PopulateDuplicateRecords(TrainingID, PlannedScheduledDate, PlannedFromTime, PlannedToTime, LocationID, RecordID).Copy();
                    if (dtTrainingSchedules.Rows.Count > 0)
                    {
                        Status = false;
                    }
                    else
                    {
                        Status = true;
                    }
                }

                log.Debug("isValidRecord()");
            }
            catch (Exception ex)
            {
                log.Error("isValidRecord()" + ex.Message);
                throw ex;
            }
            finally
            {
                TSDH = null;
                dtTrainingSchedules.Dispose();
            }
            return Status;
        }
    }
}