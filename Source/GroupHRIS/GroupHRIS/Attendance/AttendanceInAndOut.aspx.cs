using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.MetaData;
using System.Data;
using Common;
using DataHandler.Employee;
using DataHandler.Attendance;
using GroupHRIS.Utility;
using System.Text;
using DataHandler.Userlogin;
using DataHandler;
using System.Configuration;
using System.IO;
using System.Web.Mail;
using System.Globalization;

namespace GroupHRIS.Attendance
{
    public partial class AttendanceInAndOut : System.Web.UI.Page
    {
        public static string x = "";

        AttendanceInAndOutDataHandler attendanceDataHandler = new AttendanceInAndOutDataHandler();
        CheckBox[] chkArr;


        DateTime dateValue;
        DateTimeOffset dateOffsetValue;

        public static List<DateTime> list;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                string KeyCOMP_ID = (string)(Session["KeyCOMP_ID"]);
                string KeyHRIS_ROLE = (string)Session["KeyHRIS_ROLE"];

                lblEmpName.Text = attendanceDataHandler.getEmpName(KeyEMPLOYEE_ID);
                txtemployee.Text = KeyEMPLOYEE_ID;
                fillHours(ddlFromHH);
                fillMinutes(ddlFromMM);
                fillHours(ddlOutHH);
                fillMinutes(ddlOutMM);
                fillHours(ddlMultipleInHH);
                fillHours(ddlMultipleOutHH);
                fillMinutes(ddlMultipleInMM);
                fillMinutes(ddlMultipleOutMM);
                ddlinout.Enabled = false;
                //getCompID(KeyCOMP_ID);
                getSupervisor(KeyEMPLOYEE_ID);
                txtCompID.Text = attendanceDataHandler.getCompanyName(KeyEMPLOYEE_ID);

                fillBrnachlocations(ddlMultipleInLocation);
                fillBrnachlocations(ddlMultipleOutLocation);
                fillBrnachlocations(dpInlocation);
                fillBrnachlocations(ddlOutLocation);

                inStatus(false);
                outStatus(false);

                createInOutBucket();
                isExcludeBucket();

                list = new List<DateTime>();
                Session["SelectedDates"] = list;

                displayattendance(txtemployee.Text);

                if (KeyHRIS_ROLE == Constants.CON_COMMON_KeyHRIS_ROLE)
                {
                    txtemployee.Enabled = false;
                    searchEmp.Visible = false;
                }
                else
                {
                    txtemployee.Enabled = false;
                    searchEmp.Visible = true;
                }
            }

            lblhistory.Text = "";
            lblnote.Text = "";



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

                string sataus = attendanceDataHandler.activeEmployee(hfVal.Value);
                Session["status"] = sataus;

                Errorhandler.ClearError(lblmsg);

                if (hfCaller.Value == "txtemployee")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "" && sataus == "ACTIVE")
                    {
                        Errorhandler.ClearError(lblerror);
                        //GridViewhide.DataSource = null;
                        //GridViewhide.DataBind();

                        txtemployee.Text = hfVal.Value;
                        displayattendance(txtemployee.Text);
                    }
                    else
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, attendanceDataHandler.getEmpName(hfVal.Value) + " is an " + sataus + " Employee", lblmsg);
                        return;
                    }
                    if (txtemployee.Text != "")
                    {
                        //Postback Methods
                        txtemployee.Text = hfVal.Value;
                        txtCompID.Text = attendanceDataHandler.getCompanyName(hfVal.Value);
                        //ddlCompID.SelectedValue = attendanceDataHandler.getCompany(hfVal.Value);
                        fillBrnachlocations(ddlMultipleInLocation);
                        fillBrnachlocations(ddlMultipleOutLocation);
                        fillBrnachlocations(dpInlocation);
                        fillBrnachlocations(ddlOutLocation);
                        lblEmpName.Text = attendanceDataHandler.getEmpName(hfVal.Value);
                        hfCaller.Value = "";

                        string supervisor = attendanceDataHandler.getSupervisor(txtemployee.Text);
                        txtRecommendBy.Text = supervisor;
                        txtRecommendByName.Text = attendanceDataHandler.getEmpName(supervisor);
                    }
                }

                if (hfCaller.Value == "txtRecommendBy")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "" && hfVal.Value != txtemployee.Text)
                    {
                        if (sataus == "ACTIVE")
                        {
                            txtRecommendBy.Text = hfVal.Value;
                            txtRecommendBy.Text = hfVal.Value;
                            txtRecommendByName.Text = attendanceDataHandler.getEmpName(hfVal.Value);
                        }
                        else
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, attendanceDataHandler.getEmpName(hfVal.Value) + " is an " + sataus + " Employee", lblmsg);
                            return;
                        }
                    }
                    else
                    {
                        lblmsg.Text = "Recomended can not be employee ";
                        return;
                    }

                    if (txtRecommendBy.Text != "")
                    {
                        //Postback Methods
                    }

                }


            }


            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(Constants.CON_ATTENDANCE_VIEW_PERIOD);

            string[] startDatein = startDate.ToShortDateString().Split('/');
            string startDateSt = (startDatein[0] + "/" + startDatein[1] + "/" + startDatein[2]);

            string[] endDateTo = endDate.ToShortDateString().Split('/');
            string endDateEnd = (endDateTo[0] + "/" + endDateTo[1] + "/" + endDateTo[2]);

            DataTable dtAttendDates = (DataTable)Session["dateBucket"];

            //DateTime fromdate = DateTime.ParseExact(startDateSt, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime toDatel = DateTime.ParseExact(endDateEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (grdRequests.Rows.Count > 0 || dtAttendDates.Rows.Count > 0)
            {
                lblhistory.Text = "Record History From : " + endDate.ToString("yyyy/MM/dd").Trim() + " To : " + startDate.ToString("yyyy/MM/dd").Trim();
                lblnote.Text = " * Approve/Reject Records Can not be Obsolete." + "<br />" + " * To Obsolete Tick/Select Records & Click Obsolete";
                btnRequest.Visible = true;

            }
            else
            {
                btnRequest.Visible = false;
            }

            enableCheckBox();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            AttendanceInAndOutDataHandler attendanceDataHandler = new AttendanceInAndOutDataHandler();

            try
            {
                DataTable dtAttendDates = (DataTable)Session["dateBucket"];
                DataRow[] result = dtAttendDates.Select("IS_EXCLUDE = '0'");

                if (txtemployee.Text == txtRecommendBy.Text)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Employee & Recommend by can't be equal", lblerror);
                    return;
                }

                //if (Session["status"] != "ACTIVE")
                //{
                //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Inactive Employee", lblerror);
                //    return;
                //}

                if (result.Length == 0)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "No records to save", lblerror);
                    return;
                }
                //get Serial to insert bulk records
                string attLogId = attendanceDataHandler.getSerial();

                //Restriction added for future dates configuration // Chathura Nawagamuwa 2017-10-18

                foreach (DataRow dtr in dtAttendDates.Rows)
                {
                    if (dtr["IS_EXCLUDE"].ToString() == "0")
                    {
                        string AttDate = dtr["ATT_DATE"].ToString();// Eg. 28/11/2017 
                        string AttTime = dtr["ATT_TIME"].ToString();// Eg. 15:15:00

                        DateTime dt = DateTime.ParseExact(AttDate + " " + AttTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                        if (System.DateTime.Now < dt)
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Cannot add In/Out configurations for future dates/times.", lblerror);
                            return;
                        }


                    }
                }


                if (dtAttendDates.Rows.Count > 0)
                {
                    //Get Recomender name & email
                    DataTable dtRecomender = attendanceDataHandler.getRecomendByName(txtRecommendBy.Text.ToString());
                    //Get Employee Name
                    string dtEmployee = attendanceDataHandler.getName(txtemployee.Text.ToString());

                    string empMail = attendanceDataHandler.getEmail(txtemployee.Text.ToString());
                    //Get E-mail Body
                    //getMailBodyHtml(dtRecomender, dtAttendDates, dtEmployee);

                    string name = "";
                    string email = "";
                    foreach (DataRow dr in dtRecomender.Rows)
                    {
                        name = dr["KNOWN_NAME"].ToString();
                        email = dr["EMAIL"].ToString();
                    }

                    Boolean isInsert = attendanceDataHandler.Insert(dtAttendDates, attLogId, txtemployee.Text);
                    if (isInsert)
                    {
                        if (txtRecommendBy.Text.ToString() != "" & email != "")
                        {
                            EmailHandler.SendHTMLMail("Group HRIS - Missing In/Out ", email, "Missing In/Out Configuration", sendMail(dtRecomender, dtAttendDates, dtEmployee, attLogId, empMail));
                        }
                        else
                        {
                            lblmail.Text = "Not Sent E-mail To Recommender.";
                        }
                        lblRoster.Visible = false;
                        ddlRoster.Visible = false;
                        lblReguler.Visible = false;
                        lblregulerTime.Visible = false;
                        gvAttendance.DataSource = null;
                        ddlinout.Enabled = false;
                        gvAttendance.DataBind();
                        clearDates();
                        clearMultipleDays();
                        cleardateBucket();
                        displayattendance(txtemployee.Text);
                        enableCheckBox();
                        inStatus(false);
                        outStatus(false);
                        //Session.Clear();

                        list = new List<DateTime>();
                        Session["SelectedDates"] = list;
                        createInOutBucket();

                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED, lblerror);

                    }
                    else
                    {

                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Duplicate entry", lblerror);

                    }

                }

            }
            catch (Exception)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Duplicate entry", lblerror);

            }
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblerror);
            gvAttendance.DataSource = null;
            gvAttendance.DataBind();
            clearDates();
            clearMultipleDays();
            cleardateBucket();
            //GridViewhide.DataSource = null;
            //GridViewhide.DataBind();

            lblRoster.Visible = false;
            ddlRoster.Visible = false;
            lblReguler.Visible = false;
            lblregulerTime.Visible = false;

            ddlinout.Enabled = false;

            grdattendance.DataSource = null;
            grdattendance.DataBind();
            Errorhandler.ClearError(lblmail);
        }

        protected void dpCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                fillBrnachlocations(dpInlocation);
                fillBrnachlocations(ddlOutLocation);
                fillBrnachlocations(ddlMultipleInLocation);
                fillBrnachlocations(ddlMultipleOutLocation);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        //private void getCompID(string KeyCOMP_ID)
        //{

        //    CompanyDataHandler companynameid = new CompanyDataHandler();
        //    DataTable CompID = new DataTable();
        //    try
        //    {
        //        if (KeyCOMP_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
        //        {
        //            CompID = companynameid.getCompanyIdCompName();
        //            ListItem lstItem = new ListItem();
        //            lstItem.Text = Constants.CON_UNIVERSAL_COMPANY_NAME;
        //            lstItem.Value = Constants.CON_UNIVERSAL_COMPANY_CODE;
        //            //ddlCompID.Items.Add(lstItem);
        //        }
        //        else
        //        {
        //            CompID = companynameid.getCompanyIdCompName(KeyCOMP_ID);
        //        }

        //        foreach (DataRow dataRow in CompID.Rows)
        //        {
        //            ListItem listItem = new ListItem();
        //            listItem.Text = dataRow["COMP_NAME"].ToString();
        //            listItem.Value = dataRow["COMPANY_ID"].ToString();
        //            //ddlCompID.Items.Add(listItem);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonVariables.MESSAGE_TEXT = ex.Message;
        //        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
        //    }
        //    finally
        //    {
        //        companynameid = null;
        //        CompID.Dispose();
        //        CompID = null;
        //    }
        //}

        private void getSupervisor(string employeeId)
        {
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            string sRecommendBy = "";

            try
            {
                sRecommendBy = employeeDataHandler.getEmployeeSupervisor(employeeId);
                txtRecommendByName.Text = employeeDataHandler.getEmployeeName(sRecommendBy);
                txtRecommendBy.Text = sRecommendBy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                employeeDataHandler = null;
            }
        }

        protected void calDates_SelectionChanged(object sender, EventArgs e)
        {

            list = (Session["SelectedDates"] as List<DateTime>);

            //if (list == null)
            //{
            //    list = new List<DateTime>();
            //    Session["SelectedDates"] = list;
            //}

            if (list.Count != 2)
            {
                list.Add(calDates.SelectedDate);
                Session["SelectedDates"] = list;
            }

            if (list.Count == 2)
            {
                List<DateTime> newList = new List<DateTime>();

                for (DateTime date = list[0]; date <= list[1]; date = date.AddDays(1))
                {
                    newList.Add(date);
                }

                foreach (DateTime dt in newList)
                {
                    calDates.SelectedDates.Add(dt);
                }
                list.Clear();
                Session["NewSelectedDates"] = newList;

            }
        }

        protected void gvAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvAttendance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAttendance.PageIndex = e.NewPageIndex;

                DataTable datatable = (Session["dateBucket"] as DataTable).Copy();

                gvAttendance.DataSource = datatable;
                gvAttendance.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlinout_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtCompID.Text != "")
            {
                if (ddlinout.SelectedValue == "2")
                {
                    inStatus(true);
                    outStatus(true);
                    fillBrnachlocations(dpInlocation);
                    fillBrnachlocations(ddlOutLocation);
                }
                else if (ddlinout.SelectedValue == "1")
                {
                    fillBrnachlocations(dpInlocation);
                    inStatus(true);
                    outStatus(false);
                }
                else if (ddlinout.SelectedValue == "0")
                {
                    fillBrnachlocations(ddlOutLocation);
                    inStatus(false);
                    outStatus(true);
                }

                //txtfromdate.Text = "";
                dpInlocation.SelectedIndex = 0;
                ddlFromHH.SelectedIndex = 0;
                ddlFromMM.SelectedIndex = 0;
                txtOutDate.Text = "";
                ddlOutLocation.SelectedIndex = 0;
                ddlOutHH.SelectedIndex = 0;
                ddlOutMM.SelectedIndex = 0;
                ddlReason1.SelectedIndex = 0;
                txtRemarks1.Text = "";
            }
            else
            {
                ddlinout.SelectedIndex = 0;
                CommonVariables.MESSAGE_TEXT = "Select Company";
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
            }

            //

            AttendanceInAndOutDataHandler inOut = new AttendanceInAndOutDataHandler();
            string employeeId = txtemployee.Text;

            string isRoster = inOut.isRoster(employeeId);
            string rDate = txtDate.Text.ToString().Trim();

            string[] DateArr = rDate.Split('/');
            rDate = DateArr[2] + "-" + DateArr[1] + "-" + DateArr[0];

            if (isRoster == "1")
            {
                DataTable rosterDate = inOut.roster(employeeId, rDate);
                if (rosterDate.Rows.Count == 0)
                {
                    lblRoster.Visible = false;
                    ddlRoster.Visible = false;
                    lblReguler.Visible = true;
                    lblregulerTime.Visible = true;
                    string inDate = txtDate.Text.ToString().Trim();

                    DataTable companyTime = inOut.rosterCompanyWorkingTime(employeeId);

                    //string var = Convert.ToDateTime(inDate).DayOfWeek.ToString();
                    string inTime = "";
                    string outTime = "";

                    dateValue = DateTime.ParseExact(inDate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    string dateval = dateValue.ToString("dddd");

                    foreach (DataRow dataRow in companyTime.Rows)
                    {
                        if (dateval == Constants.DATE_VALUE)
                        {
                            lblregulerTime.Text = dataRow["SATWORK_HOURS_START"].ToString() + " - " + dataRow["SATWORK_HOURS_END"].ToString();
                            inTime = dataRow["SATWORK_HOURS_START"].ToString();
                            outTime = dataRow["SATWORK_HOURS_END"].ToString();
                        }
                        else
                        {
                            lblregulerTime.Text = dataRow["WORK_HOURS_START"].ToString() + " - " + dataRow["WORK_HOURS_END"].ToString();
                            inTime = dataRow["WORK_HOURS_START"].ToString();
                            outTime = dataRow["WORK_HOURS_END"].ToString();
                        }
                    }

                    if (ddlinout.SelectedValue == "2")
                    {
                        if (inTime != "" && outTime != "")
                        {
                            string[] inArr = inTime.Split(':');
                            string inH = inArr[0];
                            string inM = inArr[1];

                            string[] outArr = outTime.Split(':');
                            string outH = outArr[0];
                            string outM = outArr[1];

                            ddlFromHH.Text = inH;
                            ddlFromMM.Text = inM;

                            ddlOutHH.Text = outH;
                            ddlOutMM.Text = outM;

                            txtOutDate.Text = inDate.ToString().Trim();
                        }
                        
                    }
                    else if (ddlinout.SelectedValue == "1")
                    {
                        if (inTime != "")
                        {
                            string[] inArr = inTime.Split(':');
                            string inH = inArr[0];
                            string inM = inArr[1];

                            ddlFromHH.Text = inH;
                            ddlFromMM.Text = inM;
                        }
                        
                    }
                    else if (ddlinout.SelectedValue == "0")
                    {
                        if (outTime != "")
                        {
                            string[] outArr = outTime.Split(':');
                            string outH = outArr[0];
                            string outM = outArr[1];

                            ddlOutHH.Text = outH;
                            ddlOutMM.Text = outM;

                            txtOutDate.Text = inDate.ToString().Trim();
                        }
                        
                    }
                }
                else
                {
                    ddlRoster.Items.Clear();

                    lblRoster.Visible = true;
                    ddlRoster.Visible = true;
                    lblReguler.Visible = false;
                    lblregulerTime.Visible = false;
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlRoster.Items.Add(Item);

                    foreach (DataRow dataRow in rosterDate.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["FROM_TIME"].ToString() + " - " + dataRow["TO_TIME"].ToString();
                        listItem.Value = dataRow["ROSTR_ID"].ToString();
                        ddlRoster.Items.Add(listItem);
                    }
                }

            }
            else
            {
                DataTable regulerTime = inOut.reguler(employeeId);
                string inDate = txtDate.Text.ToString().Trim();

                //string var = Convert.ToDateTime(inDate).DayOfWeek.ToString();
                string inTime = "";
                string outTime = "";

                dateValue = DateTime.ParseExact(inDate.Trim(),"dd/MM/yyyy", CultureInfo.InvariantCulture);

                string dateval = dateValue.ToString("dddd");

                foreach (DataRow dataRow in regulerTime.Rows)
                {
                    if (dateval == Constants.DATE_VALUE)
                    {
                        lblregulerTime.Text = dataRow["SATWORK_HOURS_START"].ToString() + " - " + dataRow["SATWORK_HOURS_END"].ToString();
                        inTime = dataRow["SATWORK_HOURS_START"].ToString();
                        outTime = dataRow["SATWORK_HOURS_END"].ToString();
                    }
                    else
                    {
                        lblregulerTime.Text = dataRow["WORK_HOURS_START"].ToString() + " - " + dataRow["WORK_HOURS_END"].ToString();
                        inTime = dataRow["WORK_HOURS_START"].ToString();
                        outTime = dataRow["WORK_HOURS_END"].ToString();
                    }
                }

                if (ddlinout.SelectedValue == "2")
                {
                    if (inTime != "" && outTime != "")
                    {
                        string[] inArr = inTime.Split(':');
                        string inH = inArr[0];
                        string inM = inArr[1];

                        string[] outArr = outTime.Split(':');
                        string outH = outArr[0];
                        string outM = outArr[1];

                        ddlFromHH.Text = inH;
                        ddlFromMM.Text = inM;

                        ddlOutHH.Text = outH;
                        ddlOutMM.Text = outM;

                        txtOutDate.Text = inDate.ToString().Trim();
                    }
                    
                }
                else if (ddlinout.SelectedValue == "1")
                {
                    if (inTime != "")
                    {
                        string[] inArr = inTime.Split(':');
                        string inH = inArr[0];
                        string inM = inArr[1];

                        ddlFromHH.Text = inH;
                        ddlFromMM.Text = inM;
                    }
                    
                }
                else if (ddlinout.SelectedValue == "0")
                {

                    if (outTime != "")
                    {
                        string[] outArr = outTime.Split(':');
                        string outH = outArr[0];
                        string outM = outArr[1];

                        ddlOutHH.Text = outH;
                        ddlOutMM.Text = outM;

                        txtOutDate.Text = inDate.ToString().Trim();
                    }
                }

                lblReguler.Visible = true;
                lblregulerTime.Visible = true;
                lblRoster.Visible = false;
                ddlRoster.Visible = false;

            }
            grdattendance.DataSource = null;
            DataTable attendance = inOut.attendance(employeeId, rDate);

            inTimeBucket();
            DataTable dt = (DataTable)Session["timeBucket"];

            foreach (DataRow drTime in attendance.Rows)
            {
                string intime = drTime["ATT_TIME"].ToString();

                DataRow dtrow = dt.NewRow();
                dtrow["ATT_TIME"] = intime;
                dt.Rows.Add(dtrow);
            }

            grdattendance.DataSource = dt;
            grdattendance.DataBind();

        }

        protected void btnAddMultipleDates_Click(object sender, EventArgs e)
        {
            AttendanceInAndOutDataHandler attendanceDataHandler = new AttendanceInAndOutDataHandler();
            Errorhandler.ClearError(lblerror);

            string sEmpcode = txtemployee.Text.ToString().Trim();
            string sCompID = attendanceDataHandler.getCompanyId(txtemployee.Text); //txtCompID.Text.ToString();

            string sMultipleInLocation = ddlMultipleInLocation.SelectedValue;
            string sMultipleInHH = ddlMultipleInHH.Text.ToString();
            string sMultipleInMM = ddlMultipleInMM.Text.ToString();
            string sMultipleOutLocation = ddlMultipleOutLocation.SelectedValue;
            string sMultipleOutHH = ddlMultipleOutHH.Text.ToString();
            string sMultipleOutMM = ddlMultipleOutMM.Text.ToString();

            string sReasonCode = ddlreason.SelectedValue.ToString();
            string sReason = ddlreason.SelectedItem.ToString();
            string sRecommendby = txtRecommendBy.Text.ToString();
            string sRemark = txtremark.Text.ToString();

            string sMultipleInTime = sMultipleInHH + ":" + sMultipleInMM + ":" + "00";
            string sMultipleOutTime = sMultipleOutHH + ":" + sMultipleOutMM + ":" + "00";


            DateTime startDate = DateTime.Parse(sMultipleInTime);//DateTime.Parse(sDate);
            DateTime endDate = DateTime.Parse(sMultipleOutTime); //DateTime.Parse(eDate);

            TimeSpan elapsed = endDate.Subtract(startDate);

            if (elapsed < TimeSpan.Zero)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "In Time should be grater than Out Time", lblerror);
                return;
            }

            try
            {
                if (sMultipleInLocation == "")
                {
                    CommonVariables.MESSAGE_TEXT = "In Location not selected.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sMultipleInHH == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Time not selected.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sMultipleInMM == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Time not selected.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sMultipleOutLocation == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Out Location not selected.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sMultipleOutHH == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Time not selected.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sMultipleOutMM == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Time not selected.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sRemark == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Remarks not set.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {
                    DataTable inOutDateTable = (DataTable)Session["dateBucket"];

                    List<DateTime> newList = (List<DateTime>)Session["NewSelectedDates"];

                    if (newList.Count == 0)
                    {
                        return;
                    }

                    DataTable dtAttendence = (Session["dateBucket"] as DataTable).Copy();

                    foreach (DateTime dt in newList.Distinct())
                    {
                        Boolean checkedDate = IsChecked(dt.DayOfWeek.ToString());

                        if (checkedDate == false)
                        {
                            DataRow dr = inOutDateTable.NewRow();
                            DataRow drOut = inOutDateTable.NewRow();

                            string sDate = dt.ToString("dd/MM/yyyy");
                            DateTime dtDate = dt;
                            string nextDate = dtDate.AddDays(1).ToString("dd/MM/yyyy");
                            string prevDate = dtDate.AddDays(-1).ToString("dd/MM/yyyy");

                            bool status = isExist(sDate, sMultipleInTime, sMultipleOutTime);
                            bool dbInStatus = attendanceDataHandler.isRecordExist(txtemployee.Text, sDate, sMultipleInTime);
                            bool dbOutStatus = attendanceDataHandler.isRecordExist(txtemployee.Text, sDate, sMultipleOutTime);

                            if (status == false)
                            {
                                if (sMultipleInTime != null && dbInStatus == false)
                                {
                                    dr["EMPLOYEE_ID"] = sEmpcode.Trim();
                                    dr["ATT_DATE"] = sDate.Trim();
                                    dr["COMPANY_ID"] = sCompID.Trim();
                                    dr["BRANCH_ID"] = sMultipleInLocation.Trim();
                                    dr["REASON_CODE"] = "0";
                                    dr["OFFICE_CODE"] = sRecommendby.Trim();
                                    dr["STATUS_CODE"] = Constants.STATUS_INACTIVE_VALUE;
                                    dr["REASON"] = sReason.Trim();
                                    dr["REMARK"] = sRemark.Trim();
                                    dr["IS_EXCLUDE"] = Constants.CON_ROSTER_EXCLUDE_NO;
                                    dr["DIRECTION"] = "1";
                                    dr["ATT_TIME"] = sMultipleInTime.Trim();

                                    inOutDateTable.Rows.Add(dr);
                                }

                                if (sMultipleOutTime != null && dbOutStatus == false)
                                {
                                    drOut["EMPLOYEE_ID"] = sEmpcode.Trim();
                                    drOut["ATT_DATE"] = sDate.Trim();
                                    drOut["COMPANY_ID"] = sCompID.Trim();
                                    drOut["BRANCH_ID"] = sMultipleOutLocation.Trim();
                                    drOut["REASON_CODE"] = sReasonCode.Trim();
                                    drOut["OFFICE_CODE"] = sRecommendby.Trim();
                                    drOut["STATUS_CODE"] = Constants.STATUS_INACTIVE_VALUE;
                                    drOut["REASON"] = sReason.Trim();
                                    drOut["REMARK"] = sRemark.Trim();
                                    drOut["IS_EXCLUDE"] = Constants.CON_ROSTER_EXCLUDE_NO;
                                    drOut["DIRECTION"] = "0";
                                    drOut["ATT_TIME"] = sMultipleOutTime.Trim();

                                    inOutDateTable.Rows.Add(drOut);
                                }
                            }
                        }
                    }

                    if (dtAttendence.Rows.Count > 0)
                    {
                        if (inOutDateTable.Rows.Count > 0) { dtAttendence.Merge(inOutDateTable); }
                    }
                    else
                    {
                        if (inOutDateTable.Rows.Count > 0) { dtAttendence = inOutDateTable.Copy(); }
                    }

                    Session["dateBucket"] = dtAttendence;

                    fillGridDutyDates();
                }
            }
            catch (Exception)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Date Range Not Selected", lblerror);
            }
        }

        protected void btnDeselectMultipleDates_Click1(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblerror);
            clearMultipleDays();
        }

        protected void btnclr_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblerror);

            ddlinout.Enabled = false;
            clearDates();
            inStatus(false);
            outStatus(false);

            lblRoster.Visible = false;
            ddlRoster.Visible = false;
            lblReguler.Visible = false;
            lblregulerTime.Visible = false;

            grdattendance.DataSource = null;
            grdattendance.DataBind();
        }
        
        //protected void grdRequests_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    AttendanceInAndOutDataHandler attendanceDataHandler = new AttendanceInAndOutDataHandler();

        //    try
        //    {
        //        Int32 index = Convert.ToInt32(e.CommandArgument);

        //        GridViewRow selectedRow = grdRequests.Rows[index];

        //        string sEmpcode = selectedRow.Cells[0].Text.ToString().Trim();
        //        string sAttDate = selectedRow.Cells[1].Text.ToString().Trim();
        //        string sAttTime = selectedRow.Cells[2].Text.ToString().Trim();
        //        string sDirection = selectedRow.Cells[3].Text.ToString().Trim();
        //        string sCompID = selectedRow.Cells[4].Text.ToString().Trim();
        //        string sAttLocation = selectedRow.Cells[5].Text.ToString().Trim();
        //        string sReasonCode = selectedRow.Cells[6].Text.ToString().Trim();

        //        if (e.CommandName.ToString().Equals("cancelrow"))
        //        {
        //            Boolean isupdated = attendanceDataHandler.UpdateAttendanceLog(sEmpcode, sCompID, sDirection, sAttDate, sAttLocation, sAttTime, sReasonCode, Constants.STATUS_CANCEL_VALUE);
        //            if (isupdated == true)
        //            {
        //                displayattendance(sEmpcode);
        //                CommonVariables.MESSAGE_TEXT = "Attendance successfully Cancelled.";
        //                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //        //CommonVariables.MESSAGE_TEXT = ex.Message;
        //        //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
        //    }
        //    finally
        //    {
        //        attendanceDataHandler = null;
        //    }
        //}

        protected void grdRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                AttendanceInAndOutDataHandler attendanceDataHandler = new AttendanceInAndOutDataHandler();
                DateTime MfromDate = DateTime.Today.AddDays(Constants.CON_ATTENDANCE_VIEW_PERIOD);
                //string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                //string KeyEMPLOYEE_ID = txtemployee.Text;
                grdRequests.PageIndex = e.NewPageIndex;
                grdRequests.DataSource = attendanceDataHandler.populateAttendance(txtemployee.Text, MfromDate);
                grdRequests.DataBind();
                enableCheckBox();
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected void EXCLUDE_OnCheckedChanged(object sender, EventArgs e)
        {
            int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
            CheckBox cb = (CheckBox)gvAttendance.Rows[selRowIndex].FindControl("EXCLUDE");

            DataTable dateBucket_ = new DataTable();
            int iIndex = 0;
            int iPageIndex = 0;

            iPageIndex = gvAttendance.PageIndex;

            iIndex = (gvAttendance.PageSize * iPageIndex) + selRowIndex;

            if (cb.Checked == true)
            {
                if (Session["dateBucket"] != null)
                {
                    dateBucket_ = (Session["dateBucket"] as DataTable).Copy();
                    dateBucket_.Rows[iIndex][11] = Constants.CON_ROSTER_EXCLUDE_YES;

                    Session["dateBucket"] = dateBucket_.Copy();

                    gvAttendance.DataSource = dateBucket_;
                    gvAttendance.DataBind();

                }
            }
            else if (cb.Checked == false)
            {
                if (Session["dateBucket"] != null)
                {
                    dateBucket_ = (Session["dateBucket"] as DataTable).Copy();
                    dateBucket_.Rows[iIndex][11] = Constants.CON_ROSTER_EXCLUDE_NO;

                    Session["dateBucket"] = dateBucket_.Copy();

                    gvAttendance.DataSource = dateBucket_;
                    gvAttendance.DataBind();

                }
                //((CheckBox)gvAttendance.Rows[selRowIndex].Cells[3].FindControl("EXCLUDE")).Checked = false;
            }
        }

        protected void EXCLUDE_OnCheckedChangedRequest(object sender, EventArgs e)
        {
            int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
            CheckBox cb = (CheckBox)grdRequests.Rows[selRowIndex].FindControl("chkBxSelect");

            DateTime MfromDate = DateTime.Today.AddDays(Constants.CON_ATTENDANCE_VIEW_PERIOD);

            DataTable table = attendanceDataHandler.populateAttendance(txtemployee.Text, MfromDate);
            DataTable dtinout = (DataTable)Session["dateBucket"];

            foreach (DataRow drc in table.Rows)
            {
                DataRow dr = dtinout.NewRow();

                dr["EMPLOYEE_ID"] = drc["EMPLOYEE_ID"].ToString();
                dr["ATT_DATE"] = drc["ATT_DATE"].ToString();
                dr["ATT_TIME"] = drc["ATT_TIME"].ToString();
                dr["DIRECTION"] = drc["DIRECTION"].ToString();
                dr["STATUS_CODE"] = drc["STATUS"].ToString();
                dr["REASON"] = drc["REASON"].ToString();
                dr["IS_EXCLUDE"] = Constants.CON_ROSTER_EXCLUDE_NO;
                dtinout.Rows.Add(dr);
            }

            DataTable dateBucket_ = new DataTable();
            int iIndex = 0;
            int iPageIndex = 0;

            iPageIndex = grdRequests.PageIndex;

            iIndex = (grdRequests.PageSize * iPageIndex) + selRowIndex;

            if (cb.Checked == true)
            {
                if (Session["dateBucket"] != null)
                {
                    dateBucket_ = (Session["dateBucket"] as DataTable).Copy();
                    dateBucket_.Rows[iIndex][11] = Constants.CON_ROSTER_EXCLUDE_YES;

                    Session["dateBucket"] = dateBucket_.Copy();

                    grdRequests.DataSource = dateBucket_;
                    grdRequests.DataBind();

                }
            }
            else if (cb.Checked == false)
            {
                if (Session["dateBucket"] != null)
                {
                    dateBucket_ = (Session["dateBucket"] as DataTable).Copy();
                    dateBucket_.Rows[iIndex][11] = Constants.CON_ROSTER_EXCLUDE_NO;

                    Session["dateBucket"] = dateBucket_.Copy();

                    grdRequests.DataSource = dateBucket_;
                    grdRequests.DataBind();

                }
                //((CheckBox)gvAttendance.Rows[selRowIndex].Cells[3].FindControl("EXCLUDE")).Checked = false;
            }
        }

        protected void btnRequest_Click(object sender, EventArgs e)
        {
            AttendanceInAndOutDataHandler inoutDataHandler = new AttendanceInAndOutDataHandler();
            createUpdateBucket();
            DataTable dt = (DataTable)Session["updateBucket"];
            try
            {
                for (int i = 0; i < grdRequests.Rows.Count; i++)
                {
                    if ((grdRequests.Rows[i].Cells[7].FindControl("chkBxSelect") as CheckBox).Checked == true)
                    {
                        string sEmpcode = grdRequests.Rows[i].Cells[0].Text.ToString().Trim();
                        string date = grdRequests.Rows[i].Cells[1].Text.ToString().Trim();
                        string time = grdRequests.Rows[i].Cells[2].Text.ToString().Trim();
                        string direction = grdRequests.Rows[i].Cells[3].Text.ToString().Trim();
                        string logId = grdRequests.Rows[i].Cells[8].Text.ToString().Trim();

                        DataRow dtrow = dt.NewRow();
                        dtrow["EMPLOYEE_ID"] = sEmpcode;
                        dtrow["ATT_DATE"] = date;
                        dtrow["ATT_TIME"] = time;
                        dtrow["DIRECTION"] = direction;
                        dtrow["ATT_LOG_ID"] = logId;

                        dt.Rows.Add(dtrow);
                    }
                    else
                    {

                    }

                }
                if (dt.Rows.Count > 0)
                {
                    // Update attendance log table
                    Boolean statusUpdate = inoutDataHandler.updateCancel(dt, txtemployee.Text);

                    displayattendance(txtemployee.Text);
                    enableCheckBox();
                    if (statusUpdate)
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully Obsoleted. ", lblerror);
                    }
                    else
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Record(s) Already Approved/Rejected. ", lblerror);
                    }
                }
                else
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "No Record(s) To Cancel. ", lblerror);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected void chkBxHeader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerChkBox = ((CheckBox)grdRequests.HeaderRow.FindControl("chkBxHeader"));

            if (headerChkBox.Checked == true)
            {
                for (int i = 0; i < grdRequests.Rows.Count; i++)
                {
                    string status = grdRequests.Rows[i].Cells[5].Text;
                    if (status == "Pending")
                    {
                        ((CheckBox)grdRequests.Rows[i].Cells[7].FindControl("chkBxSelect")).Checked = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < grdRequests.Rows.Count; i++)
                {

                    ((CheckBox)grdRequests.Rows[i].Cells[7].FindControl("chkBxSelect")).Checked = false;

                }
            }

        }


        #endregion



        #region Methods

        public void clearDates()
        {
            ddlinout.SelectedIndex = 0;
            txtDate.Text = "";
            dpInlocation.SelectedIndex = 0;
            ddlFromHH.SelectedIndex = 0;
            ddlFromMM.SelectedIndex = 0;
            txtOutDate.Text = "";
            ddlOutLocation.SelectedIndex = 0;
            ddlOutHH.SelectedIndex = 0;
            ddlOutMM.SelectedIndex = 0;
            ddlReason1.SelectedIndex = 0;
            txtRemarks1.Text = "";
        }

        public void clearMultipleDays()
        {
            clear();
            ddlMultipleInLocation.SelectedIndex = 0;
            ddlMultipleInHH.SelectedIndex = 0;
            ddlMultipleInMM.SelectedIndex = 0;
            ddlMultipleOutLocation.SelectedIndex = 0;
            ddlMultipleOutHH.SelectedIndex = 0;
            ddlMultipleOutMM.SelectedIndex = 0;
            ddlreason.SelectedIndex = 0;
            txtremark.Text = "";
        }

        private void clear()
        {
            calDates.SelectedDates.Clear();
            list.Clear();
            x = "";
            for (int i = 0; i < chkArr.Length; i++)
            {
                chkArr[i].Checked = false;
            }

            Utility.Errorhandler.ClearError(lblerror);
        }

        private void fillBrnachlocations(DropDownList ddl)
        {
            AttendanceInAndOutDataHandler attdataHandler = new AttendanceInAndOutDataHandler();
            BranchCenterDataHandler branchCenterDataHandler = new BranchCenterDataHandler();
            DataTable dtcompbranch = new DataTable();
            try
            {
                ddl.Items.Clear();
                string mCompCode = attdataHandler.getCompanyId(txtemployee.Text);
                dtcompbranch = branchCenterDataHandler.getBranchesForCompany(mCompCode);

                ListItem Item = new ListItem();
                Item.Text = "";
                Item.Value = "";
                ddl.Items.Add(Item);

                foreach (DataRow dataRow in dtcompbranch.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = dataRow["BRANCH_NAME"].ToString();
                    listItem.Value = dataRow["BRANCH_ID"].ToString();
                    ddl.Items.Add(listItem);
                }

            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                branchCenterDataHandler = null;
                dtcompbranch.Dispose();
            }
        }

        private void fillHours(DropDownList ddl)
        {
            ListItem Item = new ListItem();
            Item.Text = "";
            ddl.Items.Add(Item);

            for (int i = 0; i <= 23; i++)
            {
                ListItem listItem = new ListItem();
                listItem.Text = i.ToString("0#");
                ddl.Items.Add(listItem);
            }
        }

        private void fillMinutes(DropDownList ddl)
        {
            ListItem Item = new ListItem();
            Item.Text = "";
            ddl.Items.Add(Item);

            for (int i = 0; i <= 59; i++)
            {
                ListItem listItem = new ListItem();
                listItem.Text = i.ToString("0#");
                ddl.Items.Add(listItem);
            }
        }

        private void createInOutBucket()
        {
            DataTable inOutBucket = new DataTable();

            inOutBucket.Columns.Add("EMPLOYEE_ID", typeof(string));
            inOutBucket.Columns.Add("ATT_DATE", typeof(string));
            inOutBucket.Columns.Add("ATT_TIME", typeof(string));
            inOutBucket.Columns.Add("COMPANY_ID", typeof(string));
            inOutBucket.Columns.Add("BRANCH_ID", typeof(string));
            inOutBucket.Columns.Add("DIRECTION", typeof(string));
            inOutBucket.Columns.Add("REASON_CODE", typeof(string));
            inOutBucket.Columns.Add("OFFICE_CODE", typeof(string));
            inOutBucket.Columns.Add("STATUS_CODE", typeof(string));
            inOutBucket.Columns.Add("REASON", typeof(string));
            inOutBucket.Columns.Add("REMARK", typeof(string));
            inOutBucket.Columns.Add("IS_EXCLUDE", typeof(string));

            inOutBucket.PrimaryKey = new[] { inOutBucket.Columns["ATT_DATE"], inOutBucket.Columns["ATT_TIME"], inOutBucket.Columns["DIRECTION"] };

            Session["dateBucket"] = inOutBucket;
        }

        private void isExcludeBucket()
        {
            DataTable isExcludetBucket = new DataTable();
            isExcludetBucket.Columns.Add("ATT_LOG_ID", typeof(string));
            isExcludetBucket.Columns.Add("EMPLOYEE_ID", typeof(string));
            isExcludetBucket.Columns.Add("ATT_DATE", typeof(string));
            isExcludetBucket.Columns.Add("ATT_TIME", typeof(string));
            isExcludetBucket.Columns.Add("DIRECTION", typeof(string));
            isExcludetBucket.Columns.Add("REASON", typeof(string));
            isExcludetBucket.Columns.Add("STATUS_CODE", typeof(string));
            isExcludetBucket.Columns.Add("IS_EXCLUDE", typeof(string));

            isExcludetBucket.PrimaryKey = new[] { isExcludetBucket.Columns["ATT_DATE"], isExcludetBucket.Columns["ATT_TIME"], isExcludetBucket.Columns["DIRECTION"] };

            Session["isExcludeBucket"] = isExcludetBucket;
        }

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

        public void inStatus(bool status)
        {
            //txtDate.Enabled = status;
            //ddlinout.Enabled = status;
            dpInlocation.Enabled = status;
            ddlFromHH.Enabled = status;
            ddlFromMM.Enabled = status;
        }

        public void outStatus(bool status)
        {
            txtOutDate.Enabled = status;
            ddlOutLocation.Enabled = status;
            ddlOutHH.Enabled = status;
            ddlOutMM.Enabled = status;
        }

        private void fillGridDutyDates()
        {
            DataTable dutyDates = new DataTable();
            dutyDates = (DataTable)Session["dateBucket"];

            foreach (DataRow dr in dutyDates.Rows)
            {
                string direction = dr["DIRECTION"].ToString();

                if (direction == "1")
                {
                    dr["DIRECTION"] = "IN";
                }
                else if (direction == "0")
                {
                    dr["DIRECTION"] = "OUT";
                }
            }


            gvAttendance.DataSource = dutyDates;
            gvAttendance.DataBind();

        }

        public Boolean isExist(string sAttInDate, string sAttInTime, string sAttOutTime)
        {
            DataTable dtinout = (DataTable)Session["dateBucket"];
            bool exists = false;

            foreach (DataRow drc in dtinout.Rows)
            {
                string attDate = drc["ATT_DATE"].ToString();
                string attTime = drc["ATT_TIME"].ToString();
                string dirction = drc["DIRECTION"].ToString();

                if (sAttInDate == attDate && (attTime == sAttInTime || attTime == sAttOutTime))
                {
                    CommonVariables.MESSAGE_TEXT = attDate + " is Already Exists.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    exists = true;
                }
            }
            return exists;
        }

        //public Boolean isExistDB(string name,string sAttDate, string sAttTime)
        //{
        //    //employee_id,date,direction
        //    AttendanceInAndOutDataHandler inOutDataHandler = new AttendanceInAndOutDataHandler();

        //    bool exists = false;
        //    DataTable dtinout = inOutDataHandler.getRecords(name,sAttDate,sAttTime);

        //    string[] dateArr = sAttDate.Split('/');
        //    sAttDate = dateArr[2] + "-" + dateArr[1] + "-" + dateArr[0];

        //    foreach (DataRow drc in dtinout.Rows)
        //    {
        //        string empid = drc["EMPLOYEE_ID"].ToString();
        //        string attDate = drc["ATT_DATE"].ToString();
        //        string attTime = drc["ATT_TIME"].ToString();

        //        //string[] dateArr = attDate.Split('-');
        //        //sAttInDate = dateArr[2] + "/" + dateArr[1] + "/" + dateArr[0];

        //        if (sAttDate == attDate && (attTime == sAttTime || attTime == sAttTime))
        //        {
        //            CommonVariables.MESSAGE_TEXT = attDate + " - " + attTime + " is Already Exists.";
        //            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
        //            return exists = true;
        //        }
        //    }
        //    return exists;
        //}

        public Boolean isAvailable()
        {
            bool exist = false;
            AttendanceInAndOutDataHandler inOutDataHandler = new AttendanceInAndOutDataHandler();
            DataTable table = inOutDataHandler.getRecordsByEmployee(txtemployee.Text).Copy();

            DataTable dtinout = (DataTable)Session["dateBucket"];

            foreach (DataRow dr in table.Rows)
            {
                string dtempid = dr["EMPLOYEE_ID"].ToString();
                string dtattDate = dr["ATT_DATE"].ToString();
                string dtdirection = dr["DIRECTION"].ToString();

                foreach (DataRow drc in dtinout.Rows)
                {
                    string empid = drc["EMPLOYEE_ID"].ToString();
                    string attDate = drc["ATT_DATE"].ToString();
                    string direction = drc["DIRECTION"].ToString();

                    if (dtempid == empid && dtattDate == attDate && dtdirection == direction)
                    {
                        //
                    }

                    exist = true;
                }
            }

            return exist;
        }

        private void cleardateBucket()
        {
            if (Session["dateBucket"] != null)
            {
                DataTable dtdates = (DataTable)Session["dateBucket"];
                dtdates.Rows.Clear();
            }
        }

        public string sendMail(DataTable dtRecomender, DataTable dtAttendance, string empName, string attLogId, string empemail)
        {
            PasswordHandler crpto = new PasswordHandler();
            string name = "";
            string email = "";
            string var1 = String.Empty;


            //Get data rows without excluded data to pass e-mail
            DataTable filteredData = dtAttendance.Select("IS_EXCLUDE = 0").CopyToDataTable();

            foreach (DataRow dr in dtRecomender.Rows)
            {
                name = dr["KNOWN_NAME"].ToString();
                email = dr["EMAIL"].ToString();
            }
            //var1 = "<b><u> In/Out Approvel </u></b><br/><br/>";
            var1 = "Dear Mr/Ms " + name + "," + Environment.NewLine + Environment.NewLine + "</br></br>";
            var1 += empName + " need you to approve following missing IN/OUT records. " + Environment.NewLine + Environment.NewLine + Environment.NewLine + "</br></br>";

            //GridViewhide.DataSource = filteredData.Copy();
            //GridViewhide.DataBind();

            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //GridViewhide.RenderControl(hw);
            //string varTbl = sb.ToString();
            //var1 += varTbl;

            DataTable dt = new DataTable();
            dt.Columns.Add("IN/OUT DATE");
            dt.Columns.Add("TIME");
            dt.Columns.Add("DIRECTION");
            dt.Columns.Add("REMARK");

            for (int i = 0; i < filteredData.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["IN/OUT DATE"] = filteredData.Rows[i]["ATT_DATE"].ToString();
                dr["TIME"] = filteredData.Rows[i]["ATT_TIME"].ToString();
                dr["DIRECTION"] = filteredData.Rows[i]["DIRECTION"].ToString();
                dr["REMARK"] = filteredData.Rows[i]["REMARK"].ToString();
                dt.Rows.Add(dr);
            }

            filteredData = new DataTable();
            filteredData = dt;

           // StringBuilder stringBuilder = new StringBuilder();

            string var = "<table style='border: 1px solid black;border-collapse: collapse;'>";
            //add header row
            var += "<tr>";
            for (int i = 0; i < filteredData.Columns.Count; i++)
                var += "<th  style='border: 1px solid black;border-collapse: collapse;'>&nbsp;&nbsp;" + filteredData.Columns[i].ColumnName + "</th>";
            var += "</tr>";
            //add rows
            for (int i = 0; i < filteredData.Rows.Count; i++)
            {
                var += "<tr>";
                for (int j = 0; j < filteredData.Columns.Count; j++)
                    var += "<td  style='border: 1px solid black;border-collapse: collapse;'>&nbsp;&nbsp;&nbsp;" + filteredData.Rows[i][j].ToString() + "</td>";
                var += "</tr>";
            }
            var += "</table>";

            var1 += var;
            //var1 += Environment.NewLine + Environment.NewLine + "</br></br>Please approve duties.";

            string link1 = "</br></br>Please <a href=\"http://" + ConfigurationManager.AppSettings["host_Port"] + "/Attendance/AttendanceApprove.aspx" + "?AttLogId=" + crpto.Encrypt(attLogId) + "&EmpMail=" + crpto.Encrypt(empemail) + "&CAA=" + crpto.Encrypt(Constants.CON_ATTENDANCE_APPROVE) + "\"><b>APPROVE</b></a> or ";
            var1 += link1;
            string link2 = "</t><a href=\"http://" + ConfigurationManager.AppSettings["host_Port"] + "/Attendance/AttendanceReject.aspx" + "?AttLogId=" + crpto.Encrypt(attLogId) + "&EmpMail=" + crpto.Encrypt(empemail) + "&CAR=" + crpto.Encrypt(Constants.CON_ATTENDANCE_REJECT) + "\"><b>REJECT</b></a> ";
            var1 += link2;

            var1 += "duty dates." + Environment.NewLine + "</br></br>Thank You.</br></br>";
            var1 += Environment.NewLine + "This is a system generated mail." + Environment.NewLine;

            return var1;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        private void displayattendance(string KeyEMP_ID)
        {

            AttendanceInAndOutDataHandler attendanceDataHandler = new AttendanceInAndOutDataHandler();
            DataTable DtAttendance = new DataTable();
            DateTime MfromDate = DateTime.Today.AddDays(Constants.CON_ATTENDANCE_VIEW_PERIOD);

            try
            {
                DtAttendance = attendanceDataHandler.populateAttendance(KeyEMP_ID, MfromDate);
                grdRequests.DataSource = DtAttendance;
                grdRequests.DataBind();

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                attendanceDataHandler = null;
                DtAttendance.Dispose();
                DtAttendance = null;
            }
        }

        public void enableCheckBox()
        {
            for (int i = 0; i < grdRequests.Rows.Count; i++)
            {
                string status = grdRequests.Rows[i].Cells[5].Text;
                if (status == "Pending")
                {
                    ((CheckBox)grdRequests.Rows[i].Cells[7].FindControl("chkBxSelect")).Enabled = true;
                }
                else
                {
                    ((CheckBox)grdRequests.Rows[i].Cells[7].FindControl("chkBxSelect")).Enabled = false;
                }
            }
        }

        public void createUpdateBucket()
        {
            DataTable updateBucket = new DataTable();

            updateBucket.Columns.Add("ATT_LOG_ID", typeof(string));
            updateBucket.Columns.Add("EMPLOYEE_ID", typeof(string));
            updateBucket.Columns.Add("ATT_DATE", typeof(string));
            updateBucket.Columns.Add("ATT_TIME", typeof(string));
            updateBucket.Columns.Add("DIRECTION", typeof(string));

            Session["updateBucket"] = updateBucket;
        }

        public void inTimeBucket()
        {
            DataTable timeBucket = new DataTable();

            timeBucket.Columns.Add("ATT_TIME", typeof(string));

            Session["timeBucket"] = timeBucket;
        }

        #endregion

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblerror);
            ddlinout.Enabled = true;

            //
            Boolean statusDate = Utils.verifyDateDDMMYYYY(txtDate.Text);
            DateTime txtMyDate = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (statusDate == true)
            {
                if (txtMyDate >= DateTime.Now.Date)
                {
                    CommonVariables.MESSAGE_TEXT = "Invalied Date(Should be less than Current Date)";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    return;
                }

                if (txtCompID.Text != "")
                {
                    if (ddlinout.SelectedValue == "2")
                    {
                        inStatus(true);
                        outStatus(true);
                        fillBrnachlocations(dpInlocation);
                        fillBrnachlocations(ddlOutLocation);
                    }
                    else if (ddlinout.SelectedValue == "1")
                    {
                        fillBrnachlocations(dpInlocation);
                        inStatus(true);
                        outStatus(false);
                    }
                    else if (ddlinout.SelectedValue == "0")
                    {
                        fillBrnachlocations(ddlOutLocation);
                        inStatus(false);
                        outStatus(true);
                    }

                    //txtfromdate.Text = "";
                    dpInlocation.SelectedIndex = 0;
                    ddlFromHH.SelectedIndex = 0;
                    ddlFromMM.SelectedIndex = 0;
                    txtOutDate.Text = "";
                    ddlOutLocation.SelectedIndex = 0;
                    ddlOutHH.SelectedIndex = 0;
                    ddlOutMM.SelectedIndex = 0;
                    ddlReason1.SelectedIndex = 0;
                    txtRemarks1.Text = "";
                }
                else
                {
                    ddlinout.SelectedIndex = 0;
                    CommonVariables.MESSAGE_TEXT = "Select Company";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }

                //

                AttendanceInAndOutDataHandler inOut = new AttendanceInAndOutDataHandler();
                string employeeId = txtemployee.Text;

                string isRoster = inOut.isRoster(employeeId);
                string rDate = txtDate.Text.ToString().Trim();

                string[] DateArr = rDate.Split('/');
                rDate = DateArr[2] + "-" + DateArr[1] + "-" + DateArr[0];

                if (isRoster == "1")
                {
                    DataTable rosterDate = inOut.roster(employeeId, rDate);
                    if (rosterDate.Rows.Count == 0)
                    {
                        lblRoster.Visible = false;
                        ddlRoster.Visible = false;
                        lblReguler.Visible = true;
                        lblregulerTime.Visible = true;
                        string inDate = txtDate.Text.ToString().Trim();

                        DataTable companyTime = inOut.rosterCompanyWorkingTime(employeeId);

                        dateValue = DateTime.ParseExact(inDate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture.DateTimeFormat);

                        
                        string dateval = dateValue.ToString("dddd");
                        string inTime = "";
                        string outTime = "";

                        foreach (DataRow dataRow in companyTime.Rows)
                        {
                            if (dateval == Constants.DATE_VALUE)
                            {
                                lblregulerTime.Text = dataRow["SATWORK_HOURS_START"].ToString() + " - " + dataRow["SATWORK_HOURS_END"].ToString();
                                inTime = dataRow["SATWORK_HOURS_START"].ToString();
                                outTime = dataRow["SATWORK_HOURS_END"].ToString();
                            }
                            else
                            {
                                lblregulerTime.Text = dataRow["WORK_HOURS_START"].ToString() + " - " + dataRow["WORK_HOURS_END"].ToString();
                                inTime = dataRow["WORK_HOURS_START"].ToString();
                                outTime = dataRow["WORK_HOURS_END"].ToString();
                            }
                        }

                        if (ddlinout.SelectedValue == "2")
                        {
                            if (inTime != "" && outTime != "")
                            {
                                string[] inArr = inTime.Split(':');
                                string inH = inArr[0];
                                string inM = inArr[1];

                                string[] outArr = outTime.Split(':');
                                string outH = outArr[0];
                                string outM = outArr[1];

                                ddlFromHH.Text = inH;
                                ddlFromMM.Text = inM;

                                ddlOutHH.Text = outH;
                                ddlOutMM.Text = outM;

                                txtOutDate.Text = inDate.ToString().Trim();
                            }
                            
                        }
                        else if (ddlinout.SelectedValue == "1")
                        {
                            if(inTime != "")
                            {
                                string[] inArr = inTime.Split(':');
                                string inH = inArr[0];
                                string inM = inArr[1];

                                ddlFromHH.Text = inH;
                                ddlFromMM.Text = inM;
                            }
                        }
                        else if (ddlinout.SelectedValue == "0")
                        {
                            if(outTime != "")
                            {
                                string[] outArr = outTime.Split(':');
                                string outH = outArr[0];
                                string outM = outArr[1];

                                ddlOutHH.Text = outH;
                                ddlOutMM.Text = outM;

                                txtOutDate.Text = inDate.ToString().Trim();
                            }
                        }
                    }
                    else
                    {
                        ddlRoster.Items.Clear();

                        lblRoster.Visible = true;
                        ddlRoster.Visible = true;
                        lblReguler.Visible = false;
                        lblregulerTime.Visible = false;
                        ListItem Item = new ListItem();
                        Item.Text = "";
                        Item.Value = "";
                        ddlRoster.Items.Add(Item);

                        foreach (DataRow dataRow in rosterDate.Rows)
                        {
                            ListItem listItem = new ListItem();
                            listItem.Text = dataRow["FROM_TIME"].ToString() + " - " + dataRow["TO_TIME"].ToString();
                            listItem.Value = dataRow["ROSTR_ID"].ToString();
                            ddlRoster.Items.Add(listItem);
                        }
                    }

                }
                else
                {
                    DataTable regulerTime = inOut.reguler(employeeId);
                    string inDate = txtDate.Text.ToString().Trim();

                    //foreach (DataRow dataRow in regulerTime.Rows)
                    //{
                    //    lblregulerTime.Text = dataRow["WORK_HOURS_START"].ToString() + " - " + dataRow["WORK_HOURS_END"].ToString();
                    //}

                    string inTime = "";
                    string outTime = "";

                    dateValue = DateTime.ParseExact(inDate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    string dateval = dateValue.ToString("dddd");

                    foreach (DataRow dataRow in regulerTime.Rows)
                    {
                        if (dateval == Constants.DATE_VALUE)
                        {
                            lblregulerTime.Text = dataRow["SATWORK_HOURS_START"].ToString() + " - " + dataRow["SATWORK_HOURS_END"].ToString();
                            inTime = dataRow["SATWORK_HOURS_START"].ToString();
                            outTime = dataRow["SATWORK_HOURS_END"].ToString();
                        }
                        else
                        {
                            lblregulerTime.Text = dataRow["WORK_HOURS_START"].ToString() + " - " + dataRow["WORK_HOURS_END"].ToString();
                            inTime = dataRow["WORK_HOURS_START"].ToString();
                            outTime = dataRow["WORK_HOURS_END"].ToString();
                        }
                    }

                    if (ddlinout.SelectedValue == "2")
                    {
                       if(inTime != "" && outTime != "")
                       {
                            string[] inArr = inTime.Split(':');
                            string inH = inArr[0];
                            string inM = inArr[1];

                            string[] outArr = outTime.Split(':');
                            string outH = outArr[0];
                            string outM = outArr[1];

                            ddlFromHH.Text = inH;
                            ddlFromMM.Text = inM;

                            ddlOutHH.Text = outH;
                            ddlOutMM.Text = outM;

                            txtOutDate.Text = inDate.ToString().Trim();
                        }
                    }
                    else if (ddlinout.SelectedValue == "1")
                    {
                        if(inTime != "")
                        {

                            string[] inArr = inTime.Split(':');
                            string inH = inArr[0];
                            string inM = inArr[1];

                            ddlFromHH.Text = inH;
                            ddlFromMM.Text = inM;
                        }
                    }
                    else if (ddlinout.SelectedValue == "0")
                    {
                        if(outTime != "")
                        {
                            string[] outArr = outTime.Split(':');
                            string outH = outArr[0];
                            string outM = outArr[1];

                            ddlOutHH.Text = outH;
                            ddlOutMM.Text = outM;

                            txtOutDate.Text = inDate.ToString().Trim();
                        }
                    }

                    lblReguler.Visible = true;
                    lblregulerTime.Visible = true;
                    lblRoster.Visible = false;
                    ddlRoster.Visible = false;

                }
                grdattendance.DataSource = null;
                DataTable attendance = inOut.attendance(employeeId, rDate);

                inTimeBucket();
                DataTable dt = (DataTable)Session["timeBucket"];

                foreach (DataRow drTime in attendance.Rows)
                {
                    string intime = drTime["ATT_TIME"].ToString();

                    DataRow dtrow = dt.NewRow();
                    dtrow["ATT_TIME"] = intime;
                    dt.Rows.Add(dtrow);
                }

                grdattendance.DataSource = dt;
                grdattendance.DataBind();
            }
            else
            {
                CommonVariables.MESSAGE_TEXT = "Invalied Date - (DD/MM/YYYY)";
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void ddlRoster_SelectedIndexChanged(object sender, EventArgs e)
        {
            AttendanceInAndOutDataHandler inOut = new AttendanceInAndOutDataHandler();
            string inDate = txtDate.Text.ToString().Trim();
            string rosterId = ddlRoster.SelectedValue.ToString();
            DataTable rosterTime = inOut.rosterInOut(rosterId);

            if (ddlinout.SelectedValue == "2")
            {
                //inout
                foreach (DataRow dataRow in rosterTime.Rows)
                {
                    string inTime = dataRow["FROM_TIME"].ToString();
                    string outTime = dataRow["TO_TIME"].ToString();
                    string description = dataRow["ROSTER_TYPE"].ToString();

                    string[] inArr = inTime.Split(':');
                    string inH = inArr[0];
                    string inM = inArr[1];

                    string[] outArr = outTime.Split(':');
                    string outH = outArr[0];
                    string outM = outArr[1];

                    ddlFromHH.Text = inH;
                    ddlFromMM.Text = inM;

                    ddlOutHH.Text = outH;
                    ddlOutMM.Text = outM;

                    if (description == "1")
                    {
                        txtOutDate.Text = inDate.ToString().Trim();
                    }
                    else
                    {
                        // DateTime fromdate = DateTime.ParseExact(txtfromdate.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        DateTime dt = DateTime.ParseExact(inDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        dt = dt.AddDays(1);
                        //DateTime dt = Convert.ToDateTime(inDate).AddDays(1);
                        //txtOutDate.Text = dt.ToShortDateString();
                        txtOutDate.Text = dt.ToString("dd/MM/yyyy");
                    }
                }

            }
            else if (ddlinout.SelectedValue == "1")
            {
                //In
                foreach (DataRow dataRow in rosterTime.Rows)
                {
                    string inTime = dataRow["FROM_TIME"].ToString();
                    string description = dataRow["ROSTER_TYPE"].ToString();

                    string[] inArr = inTime.Split(':');
                    string inH = inArr[0];
                    string inM = inArr[1];

                    ddlFromHH.Text = inH;
                    ddlFromMM.Text = inM;

                    //if (description == "1")
                    //{
                    //    txtOutDate.Text = inDate.ToString().Trim();
                    //}
                    //else
                    //{
                    //    DateTime dt = Convert.ToDateTime(inDate).AddDays(1);
                    //    txtOutDate.Text = dt.ToString().Trim();
                    //}
                }
            }
            else if (ddlinout.SelectedValue == "0")
            {
                //Out
                foreach (DataRow dataRow in rosterTime.Rows)
                {
                    string outTime = dataRow["TO_TIME"].ToString();
                    string description = dataRow["ROSTER_TYPE"].ToString();

                    string[] outArr = outTime.Split(':');
                    string outH = outArr[0];
                    string outM = outArr[1];

                    ddlOutHH.Text = outH;
                    ddlOutMM.Text = outM;

                    if (description == "1")
                    {
                        txtOutDate.Text = inDate.ToString().Trim();
                    }
                    else
                    {
                        DateTime dt = Convert.ToDateTime(inDate).AddDays(1);
                        txtOutDate.Text = dt.ToString().Trim();
                    }
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblerror);
            bool status = false;
            bool dbStatusIn = false;
            string sEmpcode = txtemployee.Text.ToString().Trim();
            string sCompID = attendanceDataHandler.getCompanyId(txtemployee.Text);
            string sAttInDate = txtDate.Text.ToString().Trim();
            string sAttOutDate = txtOutDate.Text.ToString().Trim();
            string sAttInLocation = dpInlocation.SelectedValue;
            string sAttOutLocation = ddlOutLocation.SelectedValue;
            string sInHH = ddlFromHH.Text.ToString();
            string sInMM = ddlFromMM.Text.ToString();
            string sOutHH = ddlOutHH.Text.ToString();
            string sOutMM = ddlOutMM.Text.ToString();
            string sInout = ddlinout.SelectedValue.ToString();
            string sReasonCode = ddlReason1.SelectedValue.ToString();
            string sReason = ddlReason1.SelectedItem.ToString();
            string sRecommendby = txtRecommendBy.Text.ToString();
            string sRemark = txtRemarks1.Text.ToString();

            string sAttInTime = sInHH + ":" + sInMM + ":" + "00";
            string sAttOutTime = sOutHH + ":" + sOutMM + ":" + "00";

            DataTable dtinout = (DataTable)Session["dateBucket"];

            AttendanceInAndOutDataHandler attDataHandler = new AttendanceInAndOutDataHandler();

            if (dtinout == null)
            {
                createInOutBucket();
                dtinout = (DataTable)Session["dateBucket"];
            }
            DataRow dr = dtinout.NewRow();

            dr["EMPLOYEE_ID"] = sEmpcode.Trim();
            dr["COMPANY_ID"] = sCompID.Trim();
            //dr["REASON_CODE"] = sReasonCode.Trim();
            dr["OFFICE_CODE"] = sRecommendby.Trim();
            dr["STATUS_CODE"] = Constants.STATUS_INACTIVE_VALUE;
            dr["REASON"] = sReason.Trim();
            dr["REMARK"] = sRemark.Trim();
            dr["IS_EXCLUDE"] = Constants.CON_ROSTER_EXCLUDE_NO;

            Boolean statusDate = Utils.verifyDateDDMMYYYY(txtDate.Text);
            if (statusDate == false)
            {
                CommonVariables.MESSAGE_TEXT = "Invalied Date - (DD/MM/YYYY)";
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                return;
            }

            if (sAttInDate == "")
            {
                CommonVariables.MESSAGE_TEXT = "In Date not selected.";
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            else
            {
                if (sInout != "")
                {
                    if (sInout == "1")
                    {
                        if (sAttInDate == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "In Date not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sAttInLocation == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Location not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sInHH == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Time not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sInMM == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Time not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sRemark == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Remarks is required.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {
                            status = isExist(sAttInDate, sAttInTime, sAttOutTime);
                            dbStatusIn = attDataHandler.isRecordExist(txtemployee.Text, sAttInDate, sAttInTime);
                            if ((status == false) && (dbStatusIn == false))
                            {
                                dr["ATT_DATE"] = sAttInDate.Trim();
                                dr["BRANCH_ID"] = dpInlocation.SelectedValue;
                                dr["DIRECTION"] = sInout.Trim();
                                dr["ATT_TIME"] = sAttInTime.Trim();
                                dr["REASON_CODE"] = "0";
                                dtinout.Rows.Add(dr);
                            }
                            else
                            {
                                CommonVariables.MESSAGE_TEXT = sAttInDate + " - " + sAttInTime + " is Already Exists.";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            }
                        }
                    }
                    else if (sInout == "0")
                    {
                        if (sAttOutDate == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Out Date not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sAttOutLocation == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Location not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sOutHH == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Time not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sOutMM == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Time not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sRemark == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Remarks is required.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {
                            status = isExist(sAttOutDate, sAttInTime, sAttOutTime);

                            dbStatusIn = attDataHandler.isRecordExist(txtemployee.Text, sAttOutDate, sAttOutTime);
                            if ((status == false) && (dbStatusIn == false))
                            {
                                dr["ATT_DATE"] = sAttOutDate.Trim();
                                dr["BRANCH_ID"] = ddlOutLocation.SelectedValue;
                                dr["DIRECTION"] = sInout.Trim();
                                dr["ATT_TIME"] = sAttOutTime.Trim();
                                dr["REASON_CODE"] = sReasonCode.Trim();
                                dtinout.Rows.Add(dr);
                            }
                            else
                            {
                                CommonVariables.MESSAGE_TEXT = sAttInDate + " - " + sAttInTime + " is Already Exists.";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            }
                        }
                    }
                    else
                    {
                        if (sAttInDate == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "In Date not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sAttInLocation == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Location not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sInHH == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Time not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sInMM == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Time not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sAttOutDate == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Out Date not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sAttOutLocation == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Location not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sOutHH == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Time not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sOutMM == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Time not selected.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else if (sRemark == "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Remarks is required.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {

                            string sDate = sAttInDate + " " + sInHH + ":" + sInMM;
                            string eDate = sAttOutDate + " " + sOutHH + ":" + sOutMM;

                            DateTime startDate = DateTime.ParseExact(sDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);//DateTime.Parse(sDate);
                            DateTime endDate = DateTime.ParseExact(eDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture); //DateTime.Parse(eDate);

                            TimeSpan elapsed = endDate.Subtract(startDate);

                            if (elapsed < TimeSpan.Zero)
                            {
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "In Time should be greater than Out Time", lblerror);
                                return;
                            }


                            status = isExist(sAttInDate, sAttInTime, sAttOutTime);

                            Boolean isInExist = attDataHandler.isRecordExist(txtemployee.Text, sAttInDate, sAttInTime);
                            Boolean isOutExsist = attDataHandler.isRecordExist(txtemployee.Text, sAttOutDate, sAttOutTime);


                            //dbStatusIn = isExistDB(txtemployee.Text, sAttInDate, sAttInTime);
                            //Boolean dbStatusOut = isExistDB(txtemployee.Text, sAttOutDate, sAttInTime);
                            if ((status == false) && (isInExist == false))
                            {
                                dr["ATT_DATE"] = sAttInDate.Trim();
                                dr["BRANCH_ID"] = dpInlocation.SelectedValue;
                                dr["DIRECTION"] = "1";
                                dr["ATT_TIME"] = sAttInTime.Trim();
                                dr["REASON_CODE"] = "0";
                                dtinout.Rows.Add(dr);
                            }
                            else
                            {
                                CommonVariables.MESSAGE_TEXT = sAttInDate + " - " + sAttInTime + " is Already Exists.";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            }


                            if ((status == false) && (isOutExsist == false))
                            {

                                DataRow drnew = dtinout.NewRow();

                                drnew["EMPLOYEE_ID"] = sEmpcode.Trim();
                                drnew["ATT_DATE"] = sAttOutDate.Trim();
                                drnew["COMPANY_ID"] = sCompID.Trim();
                                drnew["REASON_CODE"] = sReasonCode.Trim();
                                drnew["OFFICE_CODE"] = sRecommendby.Trim();
                                drnew["STATUS_CODE"] = Constants.STATUS_INACTIVE_VALUE;
                                drnew["REASON"] = sReason.Trim();
                                drnew["REMARK"] = sRemark.Trim();
                                drnew["BRANCH_ID"] = ddlOutLocation.SelectedValue;
                                drnew["DIRECTION"] = "0";
                                drnew["ATT_TIME"] = sAttOutTime.Trim();
                                drnew["IS_EXCLUDE"] = Constants.CON_ROSTER_EXCLUDE_NO;
                                dtinout.Rows.Add(drnew);
                            }
                            else
                            {
                                CommonVariables.MESSAGE_TEXT = sAttOutDate + " - " + sAttOutTime + " is Already Exists.";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            }

                        }

                    }

                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Select IN / OUT.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
            }
            Session["dateBucket"] = dtinout;

            fillGridDutyDates();

        }

        protected void btnDeselect_Click(object sender, EventArgs e)
        {
            clear();
        }

       


    }
}