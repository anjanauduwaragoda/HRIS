using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using DataHandler.TrainingAndDevelopment;
using System.Data;
using Common;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainingAttendance : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sIPAddress = Request.UserHostAddress;
                log.Debug("IP:" + sIPAddress + "WebFrmTrainingAttendance : Page_Load");
                Utility.Errorhandler.ClearError(lblMessage);
                if (!IsPostBack)
                {
                    loadTrainingType();
                    loadTrainingPrograms();
                    loadTrainingStatus();
                }
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingAttendance | Page_Load | " + ex.Message);
                throw ex;
            }
            finally
            { 
            
            }
        }

        protected void btnSearchTraining_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainingAttendance : btnSearchTraining_Click");
                loadTrainings();
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingAttendance | btnSearchTraining_Click | " + ex.Message);
                throw ex;
            }
            finally
            {

            }
        }

        protected void grdvTraining_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainingAttendance | grdvTraining_RowDataBound()");

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdvTraining, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingAttendance | grdvTraining_RowDataBound() | " + ex.Message);
                throw ex;
            }
            finally
            {

            }
        }

        protected void grdvTraining_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainingAttendance | grdvTraining_SelectedIndexChanged()");

                ClearScheduleDetails();

                string TrainingID = HttpUtility.HtmlDecode(grdvTraining.Rows[grdvTraining.SelectedIndex].Cells[0].Text).Trim();
                lblTrainingName.Text = HttpUtility.HtmlDecode(grdvTraining.Rows[grdvTraining.SelectedIndex].Cells[1].Text).Trim();
                loadTrainingSchedules(TrainingID);
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingAttendance | grdvTraining_SelectedIndexChanged() | " + ex.Message);
                throw ex;
            }
            finally
            {

            }
        }

        protected void grdvTraining_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainingAttendance | grdvTraining_PageIndexChanging()");

                ClearScheduleDetails();

                grdvTraining.PageIndex = e.NewPageIndex;
                loadTrainings();
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingAttendance | grdvTraining_PageIndexChanging() | " + ex.Message);
                throw ex;
            }
            finally
            {

            }
        }

        protected void ddlTrainingSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainingAttendance | ddlTrainingSchedule_SelectedIndexChanged()");
                loadTrainingAttendance(HttpUtility.HtmlDecode(grdvTraining.Rows[grdvTraining.SelectedIndex].Cells[0].Text).Trim(), ddlTrainingSchedule.SelectedValue.Trim());
                
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingAttendance | ddlTrainingSchedule_SelectedIndexChanged() | " + ex.Message);
                throw ex;
            }
            finally
            {

            }
        }

        protected void grdvTrainingAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainingAttendance | grdvTrainingAttendance_RowDataBound()");

                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    string ActualDate = e.Row.Cells[3].Text.ToString();
                    string ArrivedTime = e.Row.Cells[5].Text.ToString();
                    string DepartureTime = e.Row.Cells[7].Text.ToString();
                    string Remarks = e.Row.Cells[8].Text.ToString();
                    string isAttend = e.Row.Cells[12].Text.ToString();


                    string[] ArrivedTimeArr = ArrivedTime.Split(':');
                    string[] DepartureTimeArr = DepartureTime.Split(':');                    

                    if (ActualDate != "")
                    {
                        DropDownList ddlActualDateYear = (e.Row.FindControl("ddlActualDateYear") as DropDownList);
                        DropDownList ddlActualDateMonth = (e.Row.FindControl("ddlActualDateMonth") as DropDownList);
                        DropDownList ddlActualDateDate = (e.Row.FindControl("ddlActualDateDate") as DropDownList);

                        TextBox txtRemarks = (e.Row.FindControl("txtRemarks") as TextBox);
                        txtRemarks.Text = HttpUtility.HtmlDecode(Remarks).Trim(); 


                        DateTime dtActualDate = DateTime.ParseExact(ActualDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        string ActualYear = dtActualDate.Year.ToString();
                        string ActualMonth = dtActualDate.Month.ToString();
                        int ActualDte = dtActualDate.Day;


                        LoadYears(ActualYear, ddlActualDateYear);
                        LoadMonth(ActualMonth, ddlActualDateMonth);
                        LoadDate(ActualDte.ToString(), dtActualDate.Month, dtActualDate.Year, ddlActualDateDate);

                        DropDownList ddlArrivedTimeH = (e.Row.FindControl("ddlArrivedTimeH") as DropDownList);
                        DropDownList ddlArrivedTimeM = (e.Row.FindControl("ddlArrivedTimeM") as DropDownList);
                        DropDownList ddlDepartureTimeH = (e.Row.FindControl("ddlDepartureTimeH") as DropDownList);
                        DropDownList ddlDepartureTimeM = (e.Row.FindControl("ddlDepartureTimeM") as DropDownList);

                        LoadHours(ddlArrivedTimeH, ArrivedTimeArr[0].ToString());
                        LoadHours(ddlDepartureTimeH, DepartureTimeArr[0].ToString());

                        LoadMinutes(ddlArrivedTimeM, ArrivedTimeArr[1].ToString());
                        LoadMinutes(ddlDepartureTimeM, DepartureTimeArr[1].ToString());

                        if (isAttend == Constants.CON_ACTIVE_STATUS)
                        {
                            CheckBox chkIsAttendance = (e.Row.FindControl("chkIsAttendance") as CheckBox);
                            chkIsAttendance.Checked = true;
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingAttendance | grdvTrainingAttendance_RowDataBound() | " + ex.Message);
                throw ex;
            }
            finally
            {

            }
        }

        protected void ddlActualDateYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlActualDateYear = (sender as DropDownList);
                GridViewRow row = (GridViewRow)ddlActualDateYear.NamingContainer;

                DropDownList ddlActualDateMonth = (row.FindControl("ddlActualDateMonth") as DropDownList);
                DropDownList ddlActualDateDate = (row.FindControl("ddlActualDateDate") as DropDownList);

                LoadDate("", Convert.ToInt32(ddlActualDateMonth.SelectedItem.Text.Trim()), Convert.ToInt32(ddlActualDateYear.SelectedItem.Text.Trim()), ddlActualDateDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }

        protected void ddlActualDateMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlActualDateMonth = (sender as DropDownList);
                GridViewRow row = (GridViewRow)ddlActualDateMonth.NamingContainer;

                DropDownList ddlActualDateYear = (row.FindControl("ddlActualDateYear") as DropDownList);
                DropDownList ddlActualDateDate = (row.FindControl("ddlActualDateDate") as DropDownList);

                LoadDate("", Convert.ToInt32(ddlActualDateMonth.SelectedItem.Text.Trim()), Convert.ToInt32(ddlActualDateYear.SelectedItem.Text.Trim()), ddlActualDateDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        void loadTrainingType()
        {
            TrainingAttendanceDataHandler TADH = new TrainingAttendanceDataHandler();
            DataTable dtTrainingType = new DataTable();
            try
            {
                log.Debug("WebFrmTrainingAttendance | loadTrainingType()");

                dtTrainingType = TADH.PopulateTrainingType().Copy();

                ddlTrainingType.Items.Clear();

                ddlTrainingType.Items.Add(new ListItem("", ""));
                foreach (DataRow dr in dtTrainingType.Rows)
                {
                    string Value = dr["TRAINING_TYPE_ID"].ToString();
                    string Text = dr["TYPE_NAME"].ToString();

                    ddlTrainingType.Items.Add(new ListItem(Text, Value));
                }
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingAttendance | loadTrainingType() | " + ex.Message);
                throw ex;
            }
            finally
            {
                dtTrainingType.Dispose();
                TADH = null;
            }
        }

        void loadTrainingPrograms()
        {
            TrainingAttendanceDataHandler TADH = new TrainingAttendanceDataHandler();
            DataTable dtTrainingPrograms = new DataTable();
            try
            {
                log.Debug("WebFrmTrainingAttendance | loadTrainingPrograms()");

                dtTrainingPrograms = TADH.PopulateTrainingPrograms().Copy();

                ddlTrainingProgram.Items.Clear(); 
                
                ddlTrainingProgram.Items.Add(new ListItem("", ""));
                foreach (DataRow dr in dtTrainingPrograms.Rows)
                {
                    string Value = dr["PROGRAM_ID"].ToString();
                    string Text = dr["PROGRAM_NAME"].ToString();

                    ddlTrainingProgram.Items.Add(new ListItem(Text, Value));
                }
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingAttendance | loadTrainingPrograms() | " + ex.Message);
                throw ex;
            }
            finally
            {
                dtTrainingPrograms.Dispose();
                TADH = null;
            }
        }

        void loadTrainingStatus()
        {
            try
            {
                log.Debug("WebFrmTrainingAttendance | loadTrainingStatus()");

                ddlTrainingStatus.Items.Clear();

                ddlTrainingStatus.Items.Add(new ListItem("", ""));
                ddlTrainingStatus.Items.Add(new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                ddlTrainingStatus.Items.Add(new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingAttendance | loadTrainingStatus() | " + ex.Message);
                throw ex;
            }
            finally
            {
                
            }
        }

        void loadTrainings()
        {
            TrainingAttendanceDataHandler TADH = new TrainingAttendanceDataHandler();
            DataTable dtTrainingPrograms = new DataTable();
            try
            {
                log.Debug("WebFrmTrainingAttendance | loadTrainingPrograms()");

                dtTrainingPrograms = TADH.PopulateTrainings(ddlTrainingType.SelectedValue.Trim(), txtTrainingCode.Text.Trim(), ddlTrainingProgram.SelectedValue.Trim(), ddlTrainingStatus.SelectedValue.Trim());
                grdvTraining.DataSource = dtTrainingPrograms.Copy();
                grdvTraining.DataBind();
               
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingAttendance | loadTrainingPrograms() | " + ex.Message);
                throw ex;
            }
            finally
            {
                dtTrainingPrograms.Dispose();
                TADH = null;
            }
        }

        void loadTrainingSchedules(string TrainingID)
        {
            TrainingAttendanceDataHandler TADH = new TrainingAttendanceDataHandler();
            DataTable dtTrainingSchedule = new DataTable();
            try
            {
                log.Debug("WebFrmTrainingAttendance | loadTrainingSchedules()");

                dtTrainingSchedule = TADH.PopulateTrainingSchedule(TrainingID).Copy();

                ddlTrainingSchedule.Items.Clear();

                ddlTrainingSchedule.Items.Add(new ListItem("", ""));
                foreach (DataRow dr in dtTrainingSchedule.Rows)
                {
                    string Value = dr["RECORD_ID"].ToString();
                    string Text = dr["DATE_TIME"].ToString();

                    ddlTrainingSchedule.Items.Add(new ListItem(Text, Value));
                }
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingAttendance | loadTrainingSchedules() | " + ex.Message);
                throw ex;
            }
            finally
            {
                dtTrainingSchedule.Dispose();
                TADH = null;
            }
        }

        void ClearScheduleDetails()
        {
            try
            {
                log.Debug("WebFrmTrainingAttendance | ClearScheduleDetails()");

                Utility.Utils.clearControls(true, lblTrainingName, ddlTrainingSchedule);
                ddlTrainingSchedule.Items.Clear();
                grdvTrainingAttendance.DataSource = null;
                grdvTrainingAttendance.DataBind();
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingAttendance | ClearScheduleDetails() | " + ex.Message);
                throw ex;
            }
            finally
            {

            }
        }

        void loadTrainingAttendance(string TrainingID,string TrainingScheduleID)
        {
            TrainingAttendanceDataHandler TADH = new TrainingAttendanceDataHandler();
            DataTable dtAttendance = new DataTable();
            try
            {
                dtAttendance = TADH.PopulateTrainingAttendance(TrainingID, TrainingScheduleID).Copy();
                grdvTrainingAttendance.DataSource = dtAttendance.Copy();
                grdvTrainingAttendance.DataBind();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtAttendance.Dispose();
                TADH = null;
            }
        }

        void LoadYears(string ActualYear, DropDownList Years)
        {
           try
            {
                Years.Items.Clear();
                List<string> L = new List<string>();               

                int CurrentYear = System.DateTime.Now.Year;
                int ActYear = 0;
                int.TryParse(ActualYear, out ActYear);

                int i = 0;
                while (i < 5)
                {
                    if (CurrentYear != ActYear)
                    {
                        if (ActYear != 0)
                        {
                            if (i == 0)
                            {
                                L.Add(ActualYear);
                            }
                            else
                            {
                                L.Add((ActYear - i).ToString());
                                L.Add((ActYear + i).ToString());
                            }
                        }
                    }
                    i++;
                }

                L.Sort();
                if (L.Count == 0)
                {
                    L.Add(ActualYear);
                }

                Years.Items.Add(new ListItem("", ""));
                foreach (var l in L)
                {
                    Years.Items.Add(l);
                }

                Years.SelectedIndex = Years.Items.IndexOf(Years.Items.FindByText(ActualYear));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { 
            
            }
        }

        void LoadMonth(string ActualMonth, DropDownList Months)
        {
            try
            {
                Months.Items.Clear();

                List<string> L = new List<string>();

                int i = 1;
                while (i <= 12)
                {
                    L.Add(i.ToString());
                    i++;
                }

                //L.Sort();

                Months.Items.Add(new ListItem("", ""));
                foreach (var l in L)
                {
                    Months.Items.Add(l);
                }

                Months.SelectedIndex = Months.Items.IndexOf(Months.Items.FindByText(ActualMonth));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        void LoadDate(string ActualDate,int Month, int Year, DropDownList Dates)
        {
            try
            {
                Dates.Items.Clear();

                List<string> L = new List<string>();

                int i = 1;
                while (i <= DateTime.DaysInMonth(Year, Month))
                {
                    L.Add(i.ToString());
                    i++;
                }

                //L.Sort();

                Dates.Items.Add(new ListItem("", ""));
                foreach (var l in L)
                {
                    Dates.Items.Add(l);
                }

                Dates.SelectedIndex = Dates.Items.IndexOf(Dates.Items.FindByText(ActualDate));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        void LoadHours(DropDownList ddlHours,string CurrentHour)
        {
            try
            {
                ddlHours.Items.Clear();
                ddlHours.Items.Add(new ListItem("", ""));
                int i = 0;
                while (i < 24)
                {
                    ddlHours.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    i++;
                }

                ddlHours.SelectedIndex = ddlHours.Items.IndexOf(ddlHours.Items.FindByValue(CurrentHour));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { 
            
            }
        }

        void LoadMinutes(DropDownList ddlMinutes, string CurrentMinute)
        {
            try
            {
                ddlMinutes.Items.Clear();

                ddlMinutes.Items.Clear();
                ddlMinutes.Items.Add(new ListItem("", ""));
                int i = 0;
                while (i < 60)
                {
                    ddlMinutes.Items.Add(new ListItem(i.ToString().PadLeft(2, '0'), i.ToString().PadLeft(2, '0')));
                    i++;
                }

                ddlMinutes.SelectedIndex = ddlMinutes.Items.IndexOf(ddlMinutes.Items.FindByValue(CurrentMinute));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            TrainingAttendanceDataHandler TADH = new TrainingAttendanceDataHandler();
            try
            {

                string TrainingID = HttpUtility.HtmlDecode(grdvTraining.Rows[grdvTraining.SelectedIndex].Cells[0].Text).Trim();
                string TrainingScheduleID = ddlTrainingSchedule.SelectedValue.ToString();
                string StatusCode = Constants.CON_ACTIVE_STATUS;
                string AddedBy = (Session["KeyUSER_ID"] as string);

                DataTable dt = new DataTable();
                dt.Columns.Add("TRAINING_ID");
                dt.Columns.Add("TRAINING_SCHEDULE_ID");
                dt.Columns.Add("EMPLOYEE_ID");
                dt.Columns.Add("ACTUAL_DATE");
                dt.Columns.Add("ARRIVED_TIME");
                dt.Columns.Add("DEPARTURE_TIME");
                dt.Columns.Add("REMARKS");
                dt.Columns.Add("STATUS_CODE");
                dt.Columns.Add("IS_ATTEND");

                int i = 0;

                while (i < grdvTrainingAttendance.Rows.Count)
                {
                    DataRow dr = dt.NewRow();

                    string EmployeeID = HttpUtility.HtmlDecode(grdvTrainingAttendance.Rows[i].Cells[0].Text).Trim();
                    string EmployeeName = HttpUtility.HtmlDecode(grdvTrainingAttendance.Rows[i].Cells[1].Text).Trim();

                    //string ActualDate = HttpUtility.HtmlDecode(grdvTrainingAttendance.Rows[i].Cells[2].Text).Trim();
                    string ActualDate = String.Empty;
                    ActualDate += (grdvTrainingAttendance.Rows[i].FindControl("ddlActualDateYear") as DropDownList).SelectedItem.Text;
                    ActualDate += "-" + (grdvTrainingAttendance.Rows[i].FindControl("ddlActualDateMonth") as DropDownList).SelectedItem.Text;
                    ActualDate += "-" + (grdvTrainingAttendance.Rows[i].FindControl("ddlActualDateDate") as DropDownList).SelectedItem.Text;


                    //string ArrivedTime = HttpUtility.HtmlDecode(grdvTrainingAttendance.Rows[i].Cells[4].Text).Trim();
                    string ArrivedTime = String.Empty;
                    ArrivedTime += (grdvTrainingAttendance.Rows[i].FindControl("ddlArrivedTimeH") as DropDownList).SelectedItem.Text;
                    ArrivedTime += ":" + (grdvTrainingAttendance.Rows[i].FindControl("ddlArrivedTimeM") as DropDownList).SelectedItem.Text;


                    //string DepartureTime = HttpUtility.HtmlDecode(grdvTrainingAttendance.Rows[i].Cells[6].Text).Trim();
                    string DepartureTime = String.Empty;
                    DepartureTime += (grdvTrainingAttendance.Rows[i].FindControl("ddlDepartureTimeH") as DropDownList).SelectedItem.Text;
                    DepartureTime += ":" + (grdvTrainingAttendance.Rows[i].FindControl("ddlDepartureTimeM") as DropDownList).SelectedItem.Text;

                    string Remarks = (grdvTrainingAttendance.Rows[i].FindControl("txtRemarks") as TextBox).Text.Trim();

                    CheckBox chkExclude = (grdvTrainingAttendance.Rows[i].FindControl("chkExclude") as CheckBox);
                    CheckBox chkIsAttendance = (grdvTrainingAttendance.Rows[i].FindControl("chkIsAttendance") as CheckBox);


                    dr["TRAINING_ID"] = TrainingID;
                    dr["TRAINING_SCHEDULE_ID"] = TrainingScheduleID;
                    dr["EMPLOYEE_ID"] = EmployeeID;
                    dr["ACTUAL_DATE"] = ActualDate;
                    dr["ARRIVED_TIME"] = ArrivedTime;
                    dr["DEPARTURE_TIME"] = DepartureTime;
                    dr["REMARKS"] = Remarks;
                    dr["STATUS_CODE"] = StatusCode;

                    if (chkIsAttendance.Checked == true)
                    {
                        dr["IS_ATTEND"] = Constants.CON_ACTIVE_STATUS;
                    }
                    else
                    {
                        dr["IS_ATTEND"] = Constants.CON_INACTIVE_STATUS;
                    }
                    
                    if (chkExclude.Checked == false)
                    {
                        dt.Rows.Add(dr);
                    }
                    i++;
                }

                TADH.Insert(dt.Copy(), AddedBy);

                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMessage);

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TADH = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFields();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                
            }
        }

        private void ClearFields()
        {
            try
            {
                Utility.Utils.clearControls(true, ddlTrainingType, txtTrainingCode, ddlTrainingProgram, ddlTrainingStatus, lblTrainingName, ddlTrainingSchedule);
                
                grdvTraining.DataSource = null;
                grdvTraining.DataBind();

                grdvTrainingAttendance.DataSource = null;
                grdvTrainingAttendance.DataBind();

                Utility.Errorhandler.ClearError(lblMessage);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}