 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.Roster;
using DataHandler.MetaData;
using DataHandler.Employee;
using Common;
using NLog;
using System.Drawing;

namespace GroupHRIS.Roster
{
    public partial class webFrmMultipleRosterAssignment : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string userID = "";
        private string compCode = "";
        private string sIPAddress = "";

        public static List<DateTime> list;

        CheckBox[] chkArr;

        public static string x = "";

        public Boolean IsChecked(string DayofWeek)
        {
            Boolean status = false;
            for (int i = 0; i < chkArr.Length; i++)
            {
                if (DayofWeek == chkArr[i].Text)
                {
                    if (chkArr[i].Checked == true)
                    {
                        status = true;
                        break;
                    }
                }
            }
            return status;
        }

        void Clear()
        {
            calRoster.SelectedDates.Clear();
            list.Clear();
            x = "";
            for (int i = 0; i < chkArr.Length; i++)
            {
                chkArr[i].Checked = false;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "webFrmEmployee : Page_Load");

            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {
                userID = Session["KeyUSER_ID"].ToString();
            }

            if (IsPostBack)
            {
                chkArr = new CheckBox[7];

                chkArr[0] = chkSunday;
                chkArr[1] = chkMonday;
                chkArr[2] = chkTuesday;
                chkArr[3] = chkWednesday;
                chkArr[4] = chkThursday;
                chkArr[5] = chkFriday;
                chkArr[6] = chkSaturday;

            }


            if (!IsPostBack)
            {
                //Session["KeyCOMP_ID"] = Constants.CON_UNIVERSAL_COMPANY_CODE;

                //Session["KeyCOMP_ID"] = "CP17";

                list = new List<DateTime>();
                Session["SelectedDates"] = list;
                createRosterBucket();
                createSavedRosters();
            }
            else
            {
                //if ((hfEmpId.Value.Trim() == "") || (hfEmpId.Value.Trim() != txtEmploeeId.Text.Trim()))
                //{
                //    clearControls();

                //    hfEmpId.Value = txtEmploeeId.Text.Trim();

                //    //clearForEmployee();

                //    lblName.Text = getEmployeeName(txtEmploeeId.Text.Trim());
                //    getCompanyInformation(txtEmploeeId.Text.Trim());
                //    compCode = hfCompCode.Value;
                //    fillRosters();
                //}

                if (hfCaller.Value == "txtEmploeeId")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtEmploeeId.Text = hfVal.Value;
                    }
                    if (txtEmploeeId.Text != "")
                    {
                        //Postback Methods
                        clearControls();

                        hfEmpId.Value = txtEmploeeId.Text.Trim();

                        //clearForEmployee();

                        lblName.Text = getEmployeeName(txtEmploeeId.Text.Trim());
                        getCompanyInformation(txtEmploeeId.Text.Trim());
                        compCode = hfCompCode.Value;
                        fillRosters();
                    }
                }
            }

        }

        private void createRosterBucket()
        {
            log.Debug("webFrmEmployeeLeaveSheet : createRosterBucket()");

            DataTable rosterBucket = new DataTable();

            rosterBucket.Columns.Add("ROSTR_ID", typeof(string));
            rosterBucket.Columns.Add("ROSTR_TIME", typeof(string));
            rosterBucket.Columns.Add("DUTY_DATE", typeof(string));
            rosterBucket.Columns.Add("SYSTEM_FEEDBACK", typeof(string));
            rosterBucket.Columns.Add("IS_EXCLUDE", typeof(string));
            rosterBucket.Columns.Add("FROM", typeof(Int32));
            rosterBucket.Columns.Add("TO", typeof(Int32));
            rosterBucket.Columns.Add("ROSTER_TYPE", typeof(string));

            rosterBucket.PrimaryKey = new[] { rosterBucket.Columns["ROSTR_ID"], rosterBucket.Columns["ROSTR_TIME"], rosterBucket.Columns["DUTY_DATE"] };

            Session["rosterBucket"] = rosterBucket;

        }

        private void createSavedRosters()
        {
            DataTable savedRosters = new DataTable();
            savedRosters.Columns.Add("ROSTR_ID", typeof(string));
            savedRosters.Columns.Add("ROSTR_TIME", typeof(string));
            savedRosters.Columns.Add("DUTY_DATE", typeof(string));
            savedRosters.PrimaryKey = new[] { savedRosters.Columns["ROSTR_ID"], savedRosters.Columns["ROSTR_TIME"], savedRosters.Columns["DUTY_DATE"] };

            Session["savedRosters"] = savedRosters;
        }

        private void fillRosters()
        {
            log.Debug("fillRosters()");

            RosterDataHandler dhRosters = new RosterDataHandler();
            DataTable dtRosters = new DataTable();

            try
            {
                dtRosters = dhRosters.populateForDropDown(compCode);

                ddlRosterID.Items.Clear();

                if (dtRosters.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlRosterID.Items.Add(Item);

                    foreach (DataRow dataRow in dtRosters.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["ROSTER_DESC"].ToString();
                        listItem.Value = dataRow["ROSTR_ID"].ToString();

                        ddlRosterID.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                dhRosters = null;
                dtRosters.Dispose();
            }

        }

        private string getEmployeeName(string employeeId)
        {
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            try
            {
                string eName = employeeDataHandler.getEmployeeName(employeeId);

                employeeDataHandler = null;

                return eName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void getCompanyInformation(string txtEmploeeId)
        {

            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            try
            {
                DataRow dr = employeeDataHandler.getEmployeeCompanyAndName(txtEmploeeId);

                if (dr != null)
                {
                    hfCompCode.Value = dr["COMPANY_ID"].ToString();
                    lblCompany.Text = dr["COMP_NAME"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void calRoster_SelectionChanged(object sender, EventArgs e)
        {
            list = (Session["SelectedDates"] as List<DateTime>);

            if (list.Count != 2)
            {
                list.Add(calRoster.SelectedDate);
                Session["SelectedDates"] = list;
            }

            if (list.Count == 2)
            {
                List<DateTime> newList = new List<DateTime>();

                for (DateTime date = list[0]; date <= list[1]; date = date.AddDays(1))
                {
                    newList.Add(date);
                    //Calendar1.SelectedDates.Add(date);
                }

                foreach (DateTime dt in newList)
                {
                    calRoster.SelectedDates.Add(dt);
                }
                list.Clear();
                Session["NewSelectedDates"] = newList;
            }

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            EmpRosterAssignmentDataHandler dhEmpRoster = new EmpRosterAssignmentDataHandler();
            RosterDataHandler dhRoster = new RosterDataHandler();

            Utility.Errorhandler.ClearError(lblMessage);

            string employeeId = "";
            string rosterId = "";
            string roster = "";

            DataTable rosterTemp = new DataTable();
            rosterTemp.Columns.Add("ROSTR_ID", typeof(string));
            rosterTemp.Columns.Add("ROSTR_TIME", typeof(string));
            rosterTemp.Columns.Add("DUTY_DATE", typeof(string));
            rosterTemp.Columns.Add("SYSTEM_FEEDBACK", typeof(string));
            rosterTemp.Columns.Add("IS_EXCLUDE", typeof(string));
            rosterTemp.Columns.Add("FROM", typeof(Int32));
            rosterTemp.Columns.Add("TO", typeof(Int32));
            rosterTemp.Columns.Add("ROSTER_TYPE", typeof(string));

            rosterTemp.PrimaryKey = new[] { rosterTemp.Columns["ROSTR_ID"], rosterTemp.Columns["ROSTR_TIME"], rosterTemp.Columns["DUTY_DATE"] };

            try
            {
                if (txtEmploeeId.Text != "")
                {
                    employeeId = txtEmploeeId.Text.Trim();
                }

                if (ddlRosterID.Text.Trim() != "")
                {
                    rosterId = ddlRosterID.SelectedItem.Value;
                    roster = ddlRosterID.SelectedItem.Text.Trim();
                }

                List<DateTime> newList = (List<DateTime>)Session["NewSelectedDates"];

                if (newList.Count == 0)
                {
                    return;
                }

                List<string> range = dhRoster.getTimeRangeForRoster(rosterId.Trim());

                string sFrom = range[0].Replace(":", "");
                string sTo = range[1].Replace(":", "");
                string sRosterType = range[2].Trim();

                Int32 iFrom = Int32.Parse(sFrom);
                Int32 iTo = Int32.Parse(sTo);

                DataTable dtRoster = (Session["rosterBucket"] as DataTable).Copy();

                foreach (DateTime dt in newList.Distinct())
                {
                    Boolean checkedDate = IsChecked(dt.DayOfWeek.ToString());

                    if (checkedDate == false)
                    {
                        DataRow dr = rosterTemp.NewRow();

                        string sDate = dt.ToString("yyyy-MM-dd");
                        DateTime dtDate = dt;
                        string nextDate = dtDate.AddDays(1).ToString("yyyy-MM-dd");
                        string prevDate = dtDate.AddDays(-1).ToString("yyyy-MM-dd");

                        if (isExist(rosterId.Trim(), roster.Trim(), sDate, dtRoster) == false)
                        {
                            DataTable dtTable = Session["rosterBucket"] as DataTable;

                            dr["ROSTR_ID"] = rosterId.Trim();
                            dr["ROSTR_TIME"] = roster.Trim();
                            dr["DUTY_DATE"] = sDate.Trim();

                            if((sRosterType == "1") &&  (dhEmpRoster.checkForOverlappingRostersRegular(employeeId.Trim(), sDate.Trim(), range[0], range[1])))
                            {
                                dr["SYSTEM_FEEDBACK"] = "Overlapping with a Roster in System";
                                dr["IS_EXCLUDE"] = Constants.CON_ROSTER_EXCLUDE_YES;
                            }
                            else if ((sRosterType == "2") && (dhEmpRoster.checkForOverlappingRostersOverNight(employeeId.Trim(), sDate.Trim(), range[0], range[1])))
                            {
                                //Boolean overlapRegular = dhEmpRoster.checkForOverlappingRostersRegular(employeeId.Trim(), sDate.Trim(), range[0], range[1]);
                                //Boolean overlapOverNight = dhEmpRoster.checkForOverlappingRostersOverNight(employeeId.Trim(), sDate.Trim(), range[0]);

                                //if ((overlapRegular == true) || (overlapOverNight == true))
                                //{
                                    dr["SYSTEM_FEEDBACK"] = "Overlapping with a Roster in System";
                                    dr["IS_EXCLUDE"] = Constants.CON_ROSTER_EXCLUDE_YES;
                                //}
                            }
                            else if ((sRosterType == "1") && (isOverlap(iFrom, iTo, sDate.Trim(), nextDate.Trim(), prevDate, (Session["rosterBucket"] as DataTable))))      
                            {
                                dr["SYSTEM_FEEDBACK"] = "Overlapping with a Current Roster";
                                dr["IS_EXCLUDE"] = Constants.CON_ROSTER_EXCLUDE_YES;
                            }
                            else if ((sRosterType == "2") && (isOverlapOverNight(iFrom, iTo, sDate.Trim(), nextDate.Trim(), prevDate, (Session["rosterBucket"] as DataTable))))
                            {
                                dr["SYSTEM_FEEDBACK"] = "Overlapping with a Current Roster";
                                dr["IS_EXCLUDE"] = Constants.CON_ROSTER_EXCLUDE_YES;
                            }
                            else
                            {
                                dr["SYSTEM_FEEDBACK"] = "";
                                dr["IS_EXCLUDE"] = Constants.CON_ROSTER_EXCLUDE_NO;
                            }


                            dr["FROM"] = iFrom;
                            dr["TO"] = iTo;
                            dr["ROSTER_TYPE"] = sRosterType;

                            rosterTemp.Rows.Add(dr);
                        }
                    }

                }                

                if (dtRoster.Rows.Count > 0)
                {
                    if (rosterTemp.Rows.Count > 0) { dtRoster.Merge(rosterTemp); }
                }
                else
                {
                    if (rosterTemp.Rows.Count > 0) { dtRoster = rosterTemp.Copy(); }
                }

                Session["rosterBucket"] = dtRoster;

                fillGridViewRoster();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Boolean isOverlapOverNight(Int32 fromtime, Int32 totime, string sDate, string nextDate,string prevDate ,DataTable dtTemp)
        {
            Boolean overlaps = false;

            DataTable dtBench = new DataTable();

            dtBench = dtTemp.Copy();

            if (dtBench.Rows.Count > 0)
            {

                DataRow[] datarow = dtBench.Select("TO > " + fromtime + " AND DUTY_DATE ='" + sDate.Trim() + "'");

                if (datarow.Length > 0) { overlaps = true; }

                DataRow[] datarow1 = dtBench.Select("FROM < " + totime + " AND DUTY_DATE ='" + nextDate.Trim() + "'");

                if (datarow1.Length > 0) { overlaps = true; }

                DataRow[] dataRow2 = dtBench.Select("TO > " + fromtime + " AND  DUTY_DATE ='" + prevDate.Trim() + "' AND ROSTER_TYPE ='2'");

                if (dataRow2.Length > 0) { overlaps = true; }

                DataRow[] datarow3 = dtBench.Select("FROM > " + fromtime + " AND TO > " + totime + " AND DUTY_DATE ='" + sDate.Trim() + "' AND ROSTER_TYPE ='2'");

                if (datarow3.Length > 0) { overlaps = true; }

                DataRow[] datarow4 = dtBench.Select("FROM < " + fromtime + " AND TO > " + totime + " AND DUTY_DATE ='" + sDate.Trim() + "' AND ROSTER_TYPE ='2'");

                if (datarow4.Length > 0) { overlaps = true; }

                //DataRow[] datarow5 = dtBench.Select("FROM <= " + fromtime + " AND TO >= " + totime + " AND DUTY_DATE ='" + sDate.Trim() + "' AND ROSTER_TYPE ='2'");

                //if (datarow5.Length > 0) { overlaps = true; }

                //DataRow[] datarow6 = dtBench.Select("FROM >= " + fromtime + " AND FROM > " + totime + " AND TO >= " + totime + " AND DUTY_DATE ='" + sDate.Trim() + "' AND ROSTER_TYPE ='2'");

                //if (datarow6.Length > 0) { overlaps = true; }

            }
            return overlaps;
        }


        private Boolean isOverlap(Int32 fromtime, Int32 totime, string sDate, string nextDate, string prevDate, DataTable dtTemp)
        {
            Boolean overlaps = false;

            DataTable dtBench = new DataTable();

            dtBench = dtTemp.Copy();

            if (dtBench.Rows.Count > 0)
            {

                //DataRow[] datarow = dtBench.Select("TO > " + fromtime + " AND TO > " + totime + " AND DUTY_DATE ='" + sDate.Trim() + "'");

                //if (datarow.Length > 0) { overlaps = true; }

                //DataRow[] datarow1 = dtBench.Select("FROM < " + totime + " AND FROM > " + fromtime + " AND DUTY_DATE ='" + sDate.Trim() + "'");

                //if (datarow1.Length > 0) { overlaps = true; }

                //DataRow[] dataRow2 = dtBench.Select("FROM > " + fromtime + " AND TO > " + totime + " AND DUTY_DATE ='" + sDate.Trim() + "'");

                //if (dataRow2.Length > 0) { overlaps = true; }

                //DataRow[] datarow3 = dtBench.Select("FROM < " + fromtime + " AND TO < " + totime + " AND DUTY_DATE ='" + sDate.Trim() + "'");

                //if (datarow3.Length > 0) { overlaps = true; }

                //DataRow[] dataRow4 = dtBench.Select("TO > " + fromtime + " AND TO < " + totime + " AND DUTY_DATE ='" + prevDate.Trim() + "' AND ROSTER_TYPE ='2'");

                //if (dataRow4.Length > 0) { overlaps = true; }

                // case 1
                DataRow[] datarow = dtBench.Select("FROM < " + fromtime + " AND " + fromtime + "  < TO  AND TO < " + totime + " AND DUTY_DATE ='" + sDate.Trim() + "' AND ROSTER_TYPE ='1'");

                if (datarow.Length > 0) { overlaps = true; }

                // case 2
                DataRow[] datarow1 = dtBench.Select("FROM > " + fromtime + " AND FROM < " + totime + " AND  TO > " + totime + " AND DUTY_DATE ='" + sDate.Trim() + "' AND ROSTER_TYPE ='1'");

                if (datarow1.Length > 0) { overlaps = true; }

                // case 3
                DataRow[] dataRow2 = dtBench.Select("FROM <= " + fromtime + " AND TO >= " + totime + " AND  DUTY_DATE ='" + sDate.Trim() + "' AND ROSTER_TYPE ='1'");

                if (dataRow2.Length > 0) { overlaps = true; }

                // case 4
                DataRow[] datarow3 = dtBench.Select("FROM > " + fromtime + " AND TO < " + totime + " AND DUTY_DATE ='" + sDate.Trim() + "' AND ROSTER_TYPE ='1'");

                if (datarow3.Length > 0) { overlaps = true; }

                // case 5
                DataRow[] dataRow4 = dtBench.Select("TO > " + fromtime + " AND TO < " + totime + " AND DUTY_DATE ='" + prevDate.Trim() + "' AND ROSTER_TYPE ='2'");

                if (dataRow4.Length > 0) { overlaps = true; }

                // case 6 check for over lapping with over night rosters

                DataRow[] dataRow5 = dtBench.Select("FROM < " + fromtime + " AND FROM < " + totime + " AND DUTY_DATE ='" + sDate.Trim() + "' AND ROSTER_TYPE ='2'");

                if (dataRow5.Length > 0) { overlaps = true; }

                // case 7 check for over lapping with over night rosters

                DataRow[] dataRow6 = dtBench.Select("FROM > " + fromtime + " AND FROM < " + totime + " AND DUTY_DATE ='" + sDate.Trim() + "' AND ROSTER_TYPE ='2'");

                if (dataRow6.Length > 0) { overlaps = true; }

            }

            return overlaps;
        }

        private Boolean isExist(string rosterId, string rosterTime, string sDate, DataTable dtTemp)
        {
            Boolean isExist = false;

            DataTable dtBench = new DataTable();

            dtBench = dtTemp.Copy();

            if (dtBench.Rows.Count > 0)
            {

                DataRow[] datarow = dtBench.Select("ROSTR_ID = '" + rosterId.Trim() + "' AND ROSTR_TIME = '" + rosterTime.Trim() + "' AND DUTY_DATE ='" + sDate.Trim() + "'");

                if (datarow.Length > 0) { isExist = true; }

            }

            return isExist;
        }

        private void fillGridViewRoster()
        {
            DataTable roster = new DataTable();
            roster = (DataTable)Session["rosterBucket"];

            gvRoster.DataSource = roster;
            gvRoster.DataBind();

        }







        public object[] dr { get; set; }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void clear()
        {
            calRoster.SelectedDates.Clear();
            list.Clear();
            x = "";
            for (int i = 0; i < chkArr.Length; i++)
            {
                chkArr[i].Checked = false;
            }

            Utility.Errorhandler.ClearError(lblMessage);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtRosterDates = new DataTable();
            EmpRosterAssignmentDataHandler empRosterAssignmentDataHandler = new EmpRosterAssignmentDataHandler();

            string employeeId = "";

            try
            {
                Utility.Errorhandler.ClearError(lblMessage);

                employeeId = txtEmploeeId.Text.Trim();

                dtRosterDates = (DataTable)Session["rosterBucket"];

                DataRow[] result = dtRosterDates.Select("IS_EXCLUDE = '0'");

                if (result.Length == 0)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "No records to save", lblMessage);

                    return;
                }

                if (dtRosterDates.Rows.Count > 0)
                {
                    DataTable dtSavedRosters = empRosterAssignmentDataHandler.InsertMultipleRoster(employeeId, dtRosterDates, Constants.CON_DB_FALSE_CHAR,
                                               Constants.CON_NOT_SUMMARIZED,
                                               String.Empty,
                                               String.Empty,
                                               String.Empty,
                                               userID).Copy();




                    if (dtSavedRosters.Rows.Count > 0)
                    {
                        DataTable dataTable = (DataTable)Session["savedRosters"];

                        if (dataTable.Rows.Count > 0)
                        {
                            dataTable.Merge(dtSavedRosters);
                        }
                        else
                        {
                            dataTable = dtSavedRosters.Copy();
                        }

                        Session["savedRosters"] = dataTable.Copy();

                        gvSavedRosters.DataSource = dataTable;

                        gvSavedRosters.DataBind();

                        clearRosterBucketAndTray();

                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED, lblMessage);
                    }
                    else
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Error Encounted", lblMessage);
                    }

                }
                else
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "First Add Rosters to the Shedule ", lblMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnClean_Click(object sender, EventArgs e)
        {
            txtEmploeeId.Text = "";
            lblCompany.Text = "";
            lblName.Text = "";

            if (ddlRosterID.Items.Count > 0)
            {
                ddlRosterID.SelectedIndex = 0;
            }

            clearControls();

        }

        private void clearControls()
        {
            Utility.Errorhandler.ClearError(lblMessage);
            gvRoster.DataSource = null;
            gvRoster.DataBind();

            gvSavedRosters.DataSource = null;
            gvSavedRosters.DataBind();

            

            if (Session["rosterBucket"] != null)
            {
                DataTable rosterBucket_ = new DataTable();
                rosterBucket_ = (DataTable)Session["rosterBucket"];

                rosterBucket_.Clear();
                Session["rosterBucket"] = rosterBucket_;
            }

            if (Session["savedRosters"] != null)
            {
                DataTable savedRosters_ = new DataTable();
                savedRosters_ = (DataTable)Session["savedRosters"];

                savedRosters_.Clear();
                Session["savedRosters"] = savedRosters_;
            }

            clear();
        }

        private void clearRosterBucketAndTray()
        {
            if (Session["rosterBucket"] != null)
            {
                DataTable rosterBucket_ = new DataTable();
                rosterBucket_ = (DataTable)Session["rosterBucket"];

                rosterBucket_.Clear();
                Session["rosterBucket"] = rosterBucket_;
            }

            gvRoster.DataSource = null;
            gvRoster.DataBind();
        }

        protected void gvRoster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkExclude = (CheckBox)e.Row.FindControl("EXCLUDE");
                    string cellContent = HttpUtility.HtmlDecode(e.Row.Cells[3].Text.Trim()).Trim();
                    if (cellContent != String.Empty)
                    {
                        chkExclude.Enabled = false;
                        //e.Row.BackColor = Color.LightSkyBlue;
                    }
                    else
                    {
                        chkExclude.Enabled = true;
                        // e.Row.BackColor = Color.White;
                    }

                    ////////foreach (TableCell cell in e.Row.Cells)
                    ////////{
                    ////////    string CompHoliday = e.Row.Cells[3].Text.ToString();

                    ////////    if (CompHoliday == Constants.CON_CALENDER_NON_WROK_DAY)
                    ////////    {
                    ////////        cell.BackColor = Color.LightSkyBlue;
                    ////////        theDropDownList.Enabled = false;
                    ////////        chkDiscard.Enabled = false;
                    ////////    }
                    ////////}

                    //e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvRoster, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }




            }
            catch (Exception ex)
            {
                log.Debug(ex.InnerException.ToString());
                throw ex;
            }
        }

        protected void gvSavedRosters_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSavedRosters.PageIndex = e.NewPageIndex;

                DataTable datatable = (Session["savedRosters"] as DataTable).Copy();

                gvSavedRosters.DataSource = datatable;
                gvSavedRosters.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gvRoster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvRoster.PageIndex = e.NewPageIndex;

                DataTable datatable = (Session["rosterBucket"] as DataTable).Copy();

                gvRoster.DataSource = datatable;
                gvRoster.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        protected void EXCLUDE_OnCheckedChanged(object sender, EventArgs e)
        {
            log.Debug("webFrmMultipleRosterAssignment : EXCLUDE_OnCheckedChanged()");

            int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
            CheckBox cb = (CheckBox)gvRoster.Rows[selRowIndex].FindControl("EXCLUDE");

            DataTable rosterBucket_ = new DataTable();

            int iIndex = 0;
            int iPageIndex = 0;

            iPageIndex = gvRoster.PageIndex;

            iIndex = (gvRoster.PageSize * iPageIndex) + selRowIndex;


            if (cb.Checked == true)
            {
                if (Session["rosterBucket"] != null)
                {
                    rosterBucket_ = (Session["rosterBucket"] as DataTable).Copy();
                    rosterBucket_.Rows[iIndex][4] = Constants.CON_ROSTER_EXCLUDE_YES;

                    Session["rosterBucket"] = rosterBucket_.Copy();

                    gvRoster.DataSource = rosterBucket_;
                    gvRoster.DataBind();

                }
                //Perform your logic
            }
            else if (cb.Checked == false)
            {
                if (Session["rosterBucket"] != null)
                {
                    rosterBucket_ = (Session["rosterBucket"] as DataTable).Copy();
                    rosterBucket_.Rows[iIndex][4] = Constants.CON_ROSTER_EXCLUDE_NO;

                    Session["rosterBucket"] = rosterBucket_.Copy();

                    gvRoster.DataSource = rosterBucket_;
                    gvRoster.DataBind();
                }


            }
        }






    }
}