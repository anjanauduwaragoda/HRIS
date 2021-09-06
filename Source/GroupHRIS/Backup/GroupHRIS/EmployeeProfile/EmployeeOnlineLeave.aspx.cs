using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Diagnostics;
using DataHandler.EmployeeLeave;
using DataHandler.MetaData;
using DataHandler.Employee;
using DataHandler.Userlogin;
using DomainConstraints;
using Common;
using NLog;
using System.Drawing;
using System.Configuration;

namespace GroupHRIS.EmployeeProfile
{
    public partial class EmployeeOnlineLeave : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            lbtnClear.Visible = false;
            lblLSDetails.Visible = false;

            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "EmployeeOnlineLeave : Page_Load");

            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {
                sUserId = Session["KeyUSER_ID"].ToString();
                txtEmploeeId.Text = Session["KeyEMPLOYEE_ID"].ToString();
            }

            if (!IsPostBack)
            {
                createLeaveBucket();
                fillLeaveSummary(txtEmploeeId.Text.Trim());
                fillHours(ddlFromHH);
                fillMinutes(ddlFromMM);
                fillHours(ddlToHH);
                fillMinutes(ddlToMM);
                fillHistory();

                if (hfrecombyid.Value.ToString().Trim() == "")
                {
                    getSupervisor(txtEmploeeId.Text.Trim());
                    string getrecbyname = getNames(txtRecommendBy.Text.ToString());
                    txtrecbybyname.Text = getrecbyname;
                    hfrecombyname.Value = getrecbyname;
                }
                else
                {
                    string getrecbyname = getNames(hfrecombyid.Value.ToString());
                    txtrecbybyname.Text = getrecbyname;
                    hfrecombyname.Value = getrecbyname;
                }
            }
            else
            {
                if ((hfEmpId.Value.Trim() == "") || (hfEmpId.Value.Trim() != txtEmploeeId.Text.Trim()))
                {
                    hfEmpId.Value = txtEmploeeId.Text.Trim();
                    hfcoveredbyid.Value = txtCoveredBy.Text.ToString();
                    //hfCaller.Value = txtRecommendBy.Text.ToString();
                    DataTable leaveBucket_ = (DataTable)Session["leaveBucket"];
                    leaveBucket_.Rows.Clear();
                    Session["leaveBucket"] = leaveBucket_;

                    clearSessionTables();
                    //fillLeaveSummary(hfEmpId.Value.Trim());
                    //getSupervisor(hfEmpId.Value);

                    txtcoveredbyname.Text = hfcoveredbyname.Value.ToString();
                    txtrecbybyname.Text = hfrecombyname.Value.ToString();

                    fillHistory();
                }
            }
        }

        private void fillLeaveSummary(string employeeId)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            LeaveConstrains leaveConstrains = new LeaveConstrains();
            string mLeaveType = "";
            decimal mLeaveAllow = 0;
            decimal mLeaveTaken = 0;
            decimal mLeaveBalance = 0;
            string mTableString = "";
            DataTable leaveSummary = new DataTable();
            double shortLeaveTakent = 0;

            try
            {

                string sYear = DateTime.Today.Year.ToString();
                int iYear = DateTime.Today.Year;

                leaveSummary = leaveScheduleDataHandler.getEmployeeLeveSummary(employeeId.Trim(), sYear).Copy();

                decimal anualLeaves = leaveConstrains.availableAnnualLeaves(txtEmploeeId.Text.Trim(), iYear);
                decimal casualLeaves = leaveConstrains.availableCasualLeaves(txtEmploeeId.Text.Trim(), iYear);

                mTableString = "";
                Literal1.Text = "";
                mLeaveAllow = 0;

                mTableString = mTableString + "<Table class='LeaveOnlineTable'>";
                mTableString = mTableString + "<Tr style='color:Green;font-weight:bold;' class='LeaveOnlineTableTR'>";
                mTableString = mTableString + "<Td class='LeaveOnlineTableTR' style ='width:120px'>TYPE</Td>";
                mTableString = mTableString + "<Td class='LeaveOnlineTableTR' style ='width:60px'>ENTITLED</Td>";
                mTableString = mTableString + "<Td class='LeaveOnlineTableTR' style ='width:60px'>TAKEN</Td>";
                mTableString = mTableString + "<Td class='LeaveOnlineTableTR' style ='width:60px'>BALANCE</Td>";
                mTableString = mTableString + "</Tr>";

                foreach (DataRow dr in leaveSummary.Rows)
                {
                    mLeaveType = dr["LEAVE_TYPE_NAME"].ToString();

                    if (dr["LEAVE_TYPE_ID"].ToString() == Constants.CON_ANNUAL_LEAVE_ID)
                    {
                        mLeaveAllow = decimal.Parse(anualLeaves.ToString());
                        
                        if (dr["leaves_taken"].ToString().Trim() != "")
                        {
                            mLeaveTaken = decimal.Parse(dr["leaves_taken"].ToString().Trim());
                        }

                    }
                    else if (dr["LEAVE_TYPE_ID"].ToString() == Constants.CON_CASUAL_LEAVE_ID)
                    {
                        mLeaveAllow = decimal.Parse(casualLeaves.ToString());

                        if (dr["leaves_taken"].ToString().Trim() != "")
                        {
                            mLeaveTaken = decimal.Parse(dr["leaves_taken"].ToString().Trim());
                        }

                    }
                    // short leave
                    else if (dr["LEAVE_TYPE_ID"].ToString() == Constants.CON_SHORT_LEAVE_LEAVE_ID)
                    {
                        mLeaveAllow = Convert.ToDecimal(Constants.MONTHLY_SHORT_LEAVE_LIMIT);
                                                
                        shortLeaveTakent = leaveScheduleDataHandler.getMonthlyShortLeavesTaken(txtEmploeeId.Text.Trim(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());

                        mLeaveTaken = Convert.ToDecimal(shortLeaveTakent);
                                                
                    }
                    else
                    {
                        if (dr["NUMBER_OF_DAYS"].ToString() == "")
                        {
                            mLeaveAllow = 0;
                        }
                        else if (dr["NUMBER_OF_DAYS"].ToString() == null)
                        {
                            mLeaveAllow = 0;
                        }
                        else
                        {
                            mLeaveAllow = decimal.Parse(dr["NUMBER_OF_DAYS"].ToString());
                        }
                        
                    }

                    if (dr["leaves_taken"].ToString() == "")
                    {
                        mLeaveTaken = 0;
                        mLeaveBalance = decimal.Parse((mLeaveAllow - mLeaveTaken).ToString());
                    }
                    else if (dr["leaves_taken"].ToString() == null)
                    {
                        mLeaveTaken = 0;
                        mLeaveBalance = decimal.Parse((mLeaveAllow - mLeaveTaken).ToString());
                    }
                    else
                    {
                        mLeaveTaken = decimal.Parse(dr["leaves_taken"].ToString());
                        mLeaveBalance = decimal.Parse((mLeaveAllow - mLeaveTaken).ToString());

                    }

                    if (mLeaveBalance < 0)
                    {
                        mLeaveBalance = 0;
                    }

                    mTableString = mTableString + "<Tr >";
                    mTableString = mTableString + "<Td class='LeaveOnlineTableTR'>" + mLeaveType + "</Td>";
                    mTableString = mTableString + "<Td class='LeaveOnlineTableTR'>" + mLeaveAllow + "</Td>";
                    mTableString = mTableString + "<Td class='LeaveOnlineTableTR'>" + mLeaveTaken + "</Td>";
                    mTableString = mTableString + "<Td class='LeaveOnlineTableTR'>" + mLeaveBalance + "</Td>";
                    mTableString = mTableString + "</Tr>";
                }

                mTableString = mTableString + "</Table>";
                Literal1.Text = mTableString;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                leaveScheduleDataHandler = null;
                leaveConstrains = null;
                leaveSummary.Dispose();
            }
        }

        private void getSupervisor(string employeeId)
        {
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            try
            {
                txtRecommendBy.Text = employeeDataHandler.getEmployeeSupervisor(employeeId);
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

        private string getNames(string employeeId)
        {
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            string EmployeeName = "";
            try
            {
                //txtRecommendBy.Text = employeeDataHandler.getEmployeeSupervisor(employeeId);
                EmployeeName = employeeDataHandler.getEmployeeName(employeeId);
                //txtrecbybyname.Text = employeeDataHandler.getEmployeeName(hfCaller.Value.ToString());
                //txtcoveredbyname.Text = employeeDataHandler.getEmployeeName(hfcoveredbyid.Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                employeeDataHandler = null;
            }

            return EmployeeName;
        }

        private void fillHours(DropDownList ddl)
        {

            ListItem Item = new ListItem();
            Item.Text = "";
            ddl.Items.Add(Item);

            for (int i = 1; i <= 24; i++)
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

        protected void btnApply_Click(object sender, EventArgs e)
        {
            log.Debug("EmployeeOnlineLeave : btnApply_Click()");

            EmployeeLeaveSchemeDataHandler employeeLeaveSchemeDataHandler = new EmployeeLeaveSchemeDataHandler();
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

            DateTime mFromDate = Convert.ToDateTime(txtFDate.Text.ToString());
            DateTime mToDate = Convert.ToDateTime(txtTDate.Text.ToString());
            TimeSpan ts = mToDate - mFromDate;
            double ndays = 0;

            try
            {


                if ((txtCoveredBy.Text.Trim().Substring(0, 2).ToUpper() != Constants.CON_EMPOLYEE_PREFIX) || (txtCoveredBy.Text.Trim().Length != Constants.CON_EMPOLYEE_ID_LENGTH) || (txtCoveredBy.Text.Trim().Substring(0, 2).ToUpper().Length != Constants.CON_EMPOLYEE_PREFIX.Length))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Coverd By is invalid", lblMessage);
                    return;
                }

                if ((txtRecommendBy.Text.Trim().Substring(0, 2).ToUpper() != Constants.CON_EMPOLYEE_PREFIX) || (txtRecommendBy.Text.Trim().Length != Constants.CON_EMPOLYEE_ID_LENGTH) || (txtRecommendBy.Text.Trim().Substring(0, 2).ToUpper().Length != Constants.CON_EMPOLYEE_PREFIX.Length))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Recommend By is invalid", lblMessage);
                    return;
                }

                Utility.Errorhandler.ClearError(lblMessage);

                string sYear = DateTime.Today.Year.ToString();

                ndays = ts.TotalDays + 1;

                int lineNo = employeeLeaveSchemeDataHandler.getActiveLeaveSchemeLine(txtEmploeeId.Text.Trim());
                Session["SchemeLine"] = lineNo;

                DataTable leaveBucket_ = new DataTable();

                if (Session["leaveBucket"] != null)
                {
                    leaveBucket_ = (DataTable)Session["leaveBucket"];
                }
                else
                {
                    createLeaveBucket();
                    leaveBucket_ = (DataTable)Session["leaveBucket"];
                }

                if (mFromDate.Year != mToDate.Year)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "From Date and To Date should be in the same year", lblMessage);
                    return;
                }

                if ((chkhalfDay.Checked == true) && (chkSL.Checked == true))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Both Half Day and Short Leave not allowed", lblMessage);
                    return;
                }

                if ((mToDate != mFromDate) && (chkhalfDay.Checked == true))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "From and To dates should identical", lblMessage);
                    return;
                }

                if ((mToDate != mFromDate) && (chkSL.Checked == true))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "From and To dates should identical", lblMessage);
                    return;
                }

                if ((mToDate < mFromDate) && (chkSL.Checked != true) && (chkhalfDay.Checked != true))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Invalid date range", lblMessage);
                    return;
                }


                if ((chkhalfDay.Checked == true) || (chkSL.Checked == true))
                {
                    if ((ddlFromHH.SelectedItem.Text.Trim() == "") || (ddlFromMM.SelectedItem.Text.Trim() == "") || (ddlToHH.SelectedItem.Text.Trim() == "") || (ddlToMM.SelectedItem.Text.Trim() == ""))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Select From Time and To Time correctlly", lblMessage);
                        return;
                    }
                }

                if (txtEmploeeId.Text.Trim().Equals(txtCoveredBy.Text.Trim()))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Employee Id and Covered By can not be same", lblMessage);
                    return;
                }

                if (txtEmploeeId.Text.Trim().Equals(txtRecommendBy.Text.Trim()))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Employee Id and Recommand By can not be same", lblMessage);
                    return;
                }

                if ((mToDate >= mFromDate) && (chkhalfDay.Checked == false) && (chkSL.Checked == false))
                {
                    for (int i = 0; i < ndays; i++)
                    {

                        DateTime mNextDate = mFromDate.AddDays(i);

                        if (isLeaveExist(leaveBucket_, mNextDate.ToString("yyyy/MM/dd")) != true)
                        {
                            if (leaveScheduleDataHandler.isOnLeave(txtCoveredBy.Text.Trim(), mNextDate.ToString("yyyy/MM/dd")) == true)
                            {
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Covering person is leave on " + mNextDate.ToString("yyyy/MM/dd"), lblMessage);
                                return;
                            }

                            if (leaveScheduleDataHandler.isOnLeave(txtEmploeeId.Text.Trim(), mNextDate.ToString("yyyy/MM/dd")) == true)
                            {
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You are leave on " + mNextDate.ToString("yyyy/MM/dd"), lblMessage);
                                return;
                            }

                            DataTable dtRosterTime = leaveScheduleDataHandler.getRosterWorkingTime(txtEmploeeId.Text.Trim(), mNextDate.ToString("yyyy/MM/dd"));

                            if (dtRosterTime.Rows.Count > 0)
                            {
                                foreach (DataRow drRTime in dtRosterTime.Rows)
                                {
                                    DataRow oDataRow = leaveBucket_.NewRow();

                                    oDataRow["EMPLOYEE_ID"] = txtEmploeeId.Text.Trim();
                                    oDataRow["LEAVE_DATE"] = mNextDate.ToString("yyyy/MM/dd");
                                    oDataRow["LEAVE_TYPE"] = "";
                                    if (drRTime != null)
                                    {
                                        oDataRow["ROSTR_ID"] = drRTime["ROSTR_ID"].ToString();
                                        oDataRow["FROM_TIME"] = drRTime["FROM_TIME"].ToString();
                                        oDataRow["TO_TIME"] = drRTime["TO_TIME"].ToString();
                                    }
                                    else
                                    {
                                        oDataRow["ROSTR_ID"] = "";
                                        oDataRow["FROM_TIME"] = "";
                                        oDataRow["TO_TIME"] = "";
                                    }
                                    oDataRow["COVERED_BY"] = txtCoveredBy.Text;
                                    oDataRow["RECOMMEND_BY"] = txtRecommendBy.Text;
                                    oDataRow["SCHEME_LINE_NO"] = lineNo.ToString();
                                    oDataRow["REMARKS"] = "";
                                    if (drRTime != null)
                                    {
                                        oDataRow["NO_OF_DAYS"] = double.Parse(drRTime["NUM_DAYS"].ToString());
                                    }
                                    else
                                    {
                                        oDataRow["NO_OF_DAYS"] = Constants.CON_FULL_DAY;
                                    }

                                    oDataRow["IS_HALFDAY"] = Constants.CON_FULL_DAY_FLAG;
                                    oDataRow["LEAVE_STATUS"] = Constants.LEAVE_STATUS_ACTIVE_VALUE;
                                    oDataRow["IS_DAY_OFF"] = Constants.LEAVE_IS_OFF_DAY_NO;

                                    leaveBucket_.Rows.Add(oDataRow);
                                }

                                dtRosterTime.Dispose();
                            }
                            else
                            {
                                DataRow drTime = leaveScheduleDataHandler.getCompanyWorkingTime(txtEmploeeId.Text.Trim());

                                DataRow oDataRow = leaveBucket_.NewRow();

                                oDataRow["EMPLOYEE_ID"] = txtEmploeeId.Text.Trim();
                                oDataRow["LEAVE_DATE"] = mNextDate.ToString("yyyy/MM/dd");
                                oDataRow["LEAVE_TYPE"] = "";
                                if (drTime != null)
                                {
                                    oDataRow["ROSTR_ID"] = "";
                                    oDataRow["FROM_TIME"] = drTime["FROM_TIME"].ToString();
                                    oDataRow["TO_TIME"] = drTime["TO_TIME"].ToString();
                                }
                                else
                                {
                                    oDataRow["ROSTR_ID"] = "";
                                    oDataRow["FROM_TIME"] = "";
                                    oDataRow["TO_TIME"] = "";
                                }
                                oDataRow["COVERED_BY"] = txtCoveredBy.Text;
                                oDataRow["RECOMMEND_BY"] = txtRecommendBy.Text;
                                oDataRow["SCHEME_LINE_NO"] = lineNo.ToString();
                                oDataRow["REMARKS"] = "";
                                oDataRow["NO_OF_DAYS"] = Constants.CON_FULL_DAY;
                                oDataRow["IS_HALFDAY"] = Constants.CON_FULL_DAY_FLAG;
                                oDataRow["LEAVE_STATUS"] = Constants.LEAVE_STATUS_ACTIVE_VALUE;
                                oDataRow["IS_DAY_OFF"] = Constants.LEAVE_IS_OFF_DAY_NO;

                                leaveBucket_.Rows.Add(oDataRow);

                                drTime = null;
                            }
                        }

                    }

                }
                else if ((mToDate == mFromDate) && (chkhalfDay.Checked == true))
                {

                    if (isLeaveExist(leaveBucket_, txtFDate.Text.Trim()) != true)
                    {
                        if (leaveScheduleDataHandler.isOnLeave(txtCoveredBy.Text.Trim(), txtFDate.Text.Trim()) == true)
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Covering person is leave on " + txtFDate.Text.Trim(), lblMessage);
                            return;
                        }

                        string fromTime = ddlFromHH.SelectedItem.Text.Trim() + ":" + ddlFromMM.Text.Trim();
                        string toTime = ddlToHH.SelectedItem.Text.Trim() + ":" + ddlToMM.Text.Trim();

                        TimeSpan tsFrom = new TimeSpan(Int32.Parse(ddlFromHH.SelectedItem.Text.Trim()), Int32.Parse(ddlFromMM.SelectedItem.Text.Trim()), 0);
                        TimeSpan tsTo = new TimeSpan(Int32.Parse(ddlToHH.SelectedItem.Text.Trim()), Int32.Parse(ddlToMM.SelectedItem.Text.Trim()), 0);

                        if (tsFrom > tsTo)
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "From time should less than To time", lblMessage);
                            return;
                        }

                        DataTable dtRosterTime = leaveScheduleDataHandler.getRosterWorkingTime(txtEmploeeId.Text.Trim(), txtFDate.Text.Trim());

                        if (dtRosterTime.Rows.Count > 0)
                        {
                            foreach (DataRow drRTime in dtRosterTime.Rows)
                            {
                                DataRow oDataRow = leaveBucket_.NewRow();

                                oDataRow["EMPLOYEE_ID"] = txtEmploeeId.Text.Trim();
                                oDataRow["LEAVE_DATE"] = txtFDate.Text.Trim();
                                oDataRow["LEAVE_TYPE"] = "";
                                if (drRTime != null)
                                {
                                    oDataRow["ROSTR_ID"] = drRTime["ROSTR_ID"].ToString();
                                }
                                else
                                {
                                    oDataRow["ROSTR_ID"] = "";
                                }

                                oDataRow["FROM_TIME"] = fromTime;
                                oDataRow["TO_TIME"] = toTime;
                                oDataRow["COVERED_BY"] = txtCoveredBy.Text;
                                oDataRow["RECOMMEND_BY"] = txtRecommendBy.Text;
                                oDataRow["SCHEME_LINE_NO"] = lineNo.ToString();
                                oDataRow["REMARKS"] = "";
                                oDataRow["NO_OF_DAYS"] = Constants.CON_HALF_DAY;
                                oDataRow["IS_HALFDAY"] = Constants.CON_HALF_DAY_FLAG;
                                oDataRow["LEAVE_STATUS"] = Constants.LEAVE_STATUS_ACTIVE_VALUE;
                                oDataRow["IS_DAY_OFF"] = Constants.LEAVE_IS_OFF_DAY_NO;

                                leaveBucket_.Rows.Add(oDataRow);
                            }

                            dtRosterTime.Dispose();
                        }
                        else
                        {
                            DataRow oDataRow = leaveBucket_.NewRow();

                            oDataRow["EMPLOYEE_ID"] = txtEmploeeId.Text.Trim();
                            oDataRow["LEAVE_DATE"] = txtFDate.Text.Trim();
                            oDataRow["LEAVE_TYPE"] = "";
                            oDataRow["ROSTR_ID"] = "";
                            oDataRow["FROM_TIME"] = fromTime;
                            oDataRow["TO_TIME"] = toTime;
                            oDataRow["COVERED_BY"] = txtCoveredBy.Text;
                            oDataRow["RECOMMEND_BY"] = txtRecommendBy.Text;
                            oDataRow["SCHEME_LINE_NO"] = lineNo.ToString();
                            oDataRow["REMARKS"] = "";
                            oDataRow["NO_OF_DAYS"] = Constants.CON_HALF_DAY;
                            oDataRow["IS_HALFDAY"] = Constants.CON_HALF_DAY_FLAG;
                            oDataRow["LEAVE_STATUS"] = Constants.LEAVE_STATUS_ACTIVE_VALUE;
                            oDataRow["IS_DAY_OFF"] = Constants.LEAVE_IS_OFF_DAY_NO;

                            leaveBucket_.Rows.Add(oDataRow);

                        }
                    }

                }
                else if ((mToDate == mFromDate) && (chkSL.Checked == true))
                {
                    if (isLeaveExist(leaveBucket_, txtFDate.Text.Trim()) != true)
                    {
                        if (leaveScheduleDataHandler.isOnLeave(txtCoveredBy.Text.Trim(), txtFDate.Text.Trim()) == true)
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Covering person is leave on " + txtFDate.Text.Trim(), lblMessage);
                            return;
                        }

                        string fromTime = ddlFromHH.SelectedItem.Text.Trim() + ":" + ddlFromMM.SelectedItem.Text.Trim();
                        string toTime = ddlToHH.SelectedItem.Text.Trim() + ":" + ddlToMM.SelectedItem.Text.Trim();

                        TimeSpan tsFrom = new TimeSpan(Int32.Parse(ddlFromHH.SelectedItem.Text.Trim()), Int32.Parse(ddlFromMM.SelectedItem.Text.Trim()), 0);
                        TimeSpan tsTo = new TimeSpan(Int32.Parse(ddlToHH.SelectedItem.Text.Trim()), Int32.Parse(ddlToMM.SelectedItem.Text.Trim()), 0);

                        if (tsFrom > tsTo)
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "From time should less than To time", lblMessage);
                            return;
                        }

                        DataTable dtRosterTime = leaveScheduleDataHandler.getRosterWorkingTime(txtEmploeeId.Text.Trim(), txtFDate.Text.Trim());

                        if (dtRosterTime.Rows.Count > 0)
                        {
                            foreach (DataRow drRow in dtRosterTime.Rows)
                            {
                                DataRow oDataRow = leaveBucket_.NewRow();

                                oDataRow["EMPLOYEE_ID"] = txtEmploeeId.Text.Trim();
                                oDataRow["LEAVE_DATE"] = txtFDate.Text.Trim();
                                oDataRow["LEAVE_TYPE"] = "";
                                if (drRow != null)
                                {
                                    oDataRow["ROSTR_ID"] = drRow["ROSTR_ID"].ToString();
                                }
                                else
                                {
                                    oDataRow["ROSTR_ID"] = "";
                                }
                                oDataRow["FROM_TIME"] = fromTime;
                                oDataRow["TO_TIME"] = toTime;
                                oDataRow["COVERED_BY"] = txtCoveredBy.Text;
                                oDataRow["RECOMMEND_BY"] = txtRecommendBy.Text;
                                oDataRow["SCHEME_LINE_NO"] = lineNo.ToString();
                                oDataRow["REMARKS"] = "";
                                oDataRow["NO_OF_DAYS"] = Constants.CON_SL;
                                oDataRow["IS_HALFDAY"] = Constants.CON_SHORT_LEAVE_FLAG;
                                oDataRow["LEAVE_STATUS"] = Constants.LEAVE_STATUS_ACTIVE_VALUE;
                                oDataRow["IS_DAY_OFF"] = Constants.LEAVE_IS_OFF_DAY_NO;

                                leaveBucket_.Rows.Add(oDataRow);
                            }

                            dtRosterTime.Dispose();
                        }
                        else
                        {
                            DataRow oDataRow = leaveBucket_.NewRow();

                            oDataRow["EMPLOYEE_ID"] = txtEmploeeId.Text.Trim();
                            oDataRow["LEAVE_DATE"] = txtFDate.Text.Trim();
                            oDataRow["LEAVE_TYPE"] = "";
                            oDataRow["ROSTR_ID"] = "";
                            oDataRow["FROM_TIME"] = fromTime;
                            oDataRow["TO_TIME"] = toTime;
                            oDataRow["COVERED_BY"] = txtCoveredBy.Text;
                            oDataRow["RECOMMEND_BY"] = txtRecommendBy.Text;
                            oDataRow["SCHEME_LINE_NO"] = lineNo.ToString();
                            oDataRow["REMARKS"] = "";
                            oDataRow["NO_OF_DAYS"] = Constants.CON_SL;
                            oDataRow["IS_HALFDAY"] = Constants.CON_SHORT_LEAVE_FLAG;
                            oDataRow["LEAVE_STATUS"] = Constants.LEAVE_STATUS_ACTIVE_VALUE;
                            oDataRow["IS_DAY_OFF"] = Constants.LEAVE_IS_OFF_DAY_NO;

                            leaveBucket_.Rows.Add(oDataRow);

                        }
                    }
                }


                string sCount = leaveBucket_.Rows.Count.ToString();

                //------------
                leaveBucket_.DefaultView.Sort = "LEAVE_DATE";
                leaveBucket_ = leaveBucket_.DefaultView.ToTable();
                //---------

                Session["leaveBucket"] = leaveBucket_;

                gvLeaveSheet.DataSource = leaveBucket_;
                gvLeaveSheet.DataBind();

            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
            finally
            {
                employeeLeaveSchemeDataHandler = null;
            }

        }

        private void createLeaveBucket()
        {

            log.Debug("EmployeeOnlineLeave : createLeaveBucket()");

            DataTable leaveBucket = new DataTable();

            leaveBucket.Columns.Add("EMPLOYEE_ID", typeof(string));
            leaveBucket.Columns.Add("LEAVE_DATE", typeof(string));
            leaveBucket.Columns.Add("LEAVE_TYPE", typeof(string));
            leaveBucket.Columns.Add("ROSTR_ID", typeof(string));
            leaveBucket.Columns.Add("FROM_TIME", typeof(string));
            leaveBucket.Columns.Add("TO_TIME", typeof(string));
            leaveBucket.Columns.Add("COVERED_BY", typeof(string));
            leaveBucket.Columns.Add("RECOMMEND_BY", typeof(string));
            leaveBucket.Columns.Add("SCHEME_LINE_NO", typeof(string));
            leaveBucket.Columns.Add("REMARKS", typeof(string));
            leaveBucket.Columns.Add("NO_OF_DAYS", typeof(double));
            leaveBucket.Columns.Add("IS_HALFDAY", typeof(string));
            leaveBucket.Columns.Add("LEAVE_STATUS", typeof(string));
            leaveBucket.Columns.Add("IS_DAY_OFF", typeof(string));

            Session["leaveBucket"] = leaveBucket;

        }

        private Boolean isLeaveExist(DataTable leaveTable, string leaveDate)
        {
            Boolean leaveExist = false;

            if (leaveTable.Rows.Count > 0)
            {

                DataRow[] leaves = leaveTable.Select("LEAVE_DATE ='" + leaveDate.Trim() + "'");

                if (leaves.Length > 0)
                {
                    leaveExist = true;
                }
            }
            return leaveExist;
        }

        private Boolean isLeaveTypeNotSelected(DataTable leaveTable)
        {

            Boolean leveTypeNotSelected = false;

            if (leaveTable.Rows.Count > 0)
            {

                DataRow[] leaves = leaveTable.Select("LEAVE_TYPE ='' AND IS_DAY_OFF='0'");

                if (leaves.Length > 0)
                {
                    leveTypeNotSelected = true;
                }
            }

            return leveTypeNotSelected;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("EmployeeOnlineLeave : btnSave_Click()");

            if (gvLeaveSheet.Rows.Count <= 0)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Apply leaves first ", lblMessage);
                return;
            }

            EmployeeLeaveSchemeDataHandler employeeLeaveSchemeDataHandler = new EmployeeLeaveSchemeDataHandler();
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            LeaveConstrains leaveConstrains = new LeaveConstrains();
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            DateTime mFromDate = Convert.ToDateTime(txtFDate.Text.ToString());
            DateTime mToDate = Convert.ToDateTime(txtTDate.Text.ToString());
            TimeSpan ts = mToDate - mFromDate;
            double ndays = 0;

            DataTable acmLeaves = new DataTable();
            DataTable acLeaves = new DataTable();

            DataTable leaveBucket_ = new DataTable();

            string userId = String.Empty;
            string lslNo = String.Empty;

            try
            {
                Utility.Errorhandler.ClearError(lblMessage);

                if (Session["KeyUSER_ID"] != null)
                {
                    userId = Session["KeyUSER_ID"].ToString();
                }
                else
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Your session expired", lblMessage);
                    return;
                }

                if (Session["SchemeLine"] != null)
                {
                    lslNo = Session["SchemeLine"].ToString();
                }
                else
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Your session expired", lblMessage);
                    return;
                }

                if (Session["leaveBucket"] != null)
                {
                    leaveBucket_ = (DataTable)Session["leaveBucket"];
                }
                else
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Your session expired", lblMessage);
                    return;
                }

                if (leaveBucket_.Rows.Count == 0)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Please apply leaves first", lblMessage);
                    return;
                }

                Boolean isTypeNotSelected = isLeaveTypeNotSelected(leaveBucket_);

                if (isTypeNotSelected)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Please select a leave type or exclude", lblMessage);
                    return;
                }


                string sFromDate = "";
                string sToDate = "";

                if (leaveBucket_.Rows.Count > 0)
                {
                    sFromDate = gvLeaveSheet.Rows[0].Cells[1].Text.Trim();
                    sToDate = gvLeaveSheet.Rows[gvLeaveSheet.Rows.Count - 1].Cells[1].Text.Trim();

                }

                string sYear = mFromDate.Year.ToString();

                ndays = ts.TotalDays + 1;

                acmLeaves.Rows.Clear();

                if ((Session["leavesACM"] != null) && (((DataTable)Session["leavesACM"]).Rows.Count > 0))
                {
                    acmLeaves = ((DataTable)Session["leavesACM"]).Copy();
                }
                else
                {
                    acmLeaves = leaveScheduleDataHandler.getACMLeavesSummary(txtEmploeeId.Text.Trim(), sYear);
                    Session["leavesACM"] = acmLeaves;
                }

                int iYear = mFromDate.Year;

                decimal anualLeaves = leaveConstrains.availableAnnualLeaves(txtEmploeeId.Text.Trim(), iYear);
                decimal casualLeaves = leaveConstrains.availableCasualLeaves(txtEmploeeId.Text.Trim(), iYear);

                string sAnualAplied = "";
                string sCasualApplied = "";

                double anualAplied = 0;
                double casualApplied = 0;

                sAnualAplied = leaveBucket_.Compute("Sum(NO_OF_DAYS)", "LEAVE_TYPE = 'ANNUAL' and IS_DAY_OFF = 0").ToString();
                sCasualApplied = leaveBucket_.Compute("Sum(NO_OF_DAYS)", "LEAVE_TYPE = 'CASUAL' and IS_DAY_OFF = 0").ToString();

                if (sAnualAplied.Trim() != "") { anualAplied = Double.Parse(sAnualAplied.Trim()); }
                if (sCasualApplied.Trim() != "") { casualApplied = Double.Parse(sCasualApplied.Trim()); }

                string sTotalLeaves = "";
                double totalLeaves = 0;

                sTotalLeaves = leaveBucket_.Compute("Sum(NO_OF_DAYS)", "IS_DAY_OFF = 0").ToString();

                if (sTotalLeaves.Trim() != "") { totalLeaves = Double.Parse(sTotalLeaves.Trim()); }

                double noOfTotalLeavesApplied = 0;

                if (totalLeaves > 0)
                {
                    noOfTotalLeavesApplied = totalLeaves;
                }

                if (noOfTotalLeavesApplied == 0)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You haven't applied any leaves", lblMessage);
                    return;
                }

                acLeaves.Rows.Clear();

                if ((Session["leavesAC"] != null) && (((DataTable)Session["leavesAC"]).Rows.Count > 0))
                {
                    acLeaves = ((DataTable)Session["leavesAC"]).Copy();
                }
                else
                {
                    acLeaves = leaveScheduleDataHandler.getACLeavesSummary(txtEmploeeId.Text.Trim(), sYear).Copy();
                    Session["leavesAC"] = acLeaves;
                }

                string sCasualLeaveTaken = "";
                string sAnnualLeaveTaken = "";

                sCasualLeaveTaken = acLeaves.Compute("Sum(leaves_taken)", "LEAVE_TYPE_ID ='CASUAL'").ToString();
                sAnnualLeaveTaken = acLeaves.Compute("Sum(leaves_taken)", "LEAVE_TYPE_ID ='ANNUAL'").ToString();

                double casualLeaveTaken = 0;
                double annualLeaveTaken = 0;

                if (sCasualLeaveTaken.Trim() != "") { casualLeaveTaken = Double.Parse(sCasualLeaveTaken.Trim()); }
                if (sAnnualLeaveTaken.Trim() != "") { annualLeaveTaken = Double.Parse(sAnnualLeaveTaken.Trim()); }

                string sShortLeavesApplied = "";
                double shortLeavesApplied = 0;

                sShortLeavesApplied = leaveBucket_.Compute("Sum(NO_OF_DAYS)", "LEAVE_TYPE = 'SL' and IS_DAY_OFF = 0").ToString();
                if (sShortLeavesApplied.Trim() != "") { shortLeavesApplied = Double.Parse(sShortLeavesApplied.Trim()); }

                double shortLeaveTakent = leaveScheduleDataHandler.getMonthlyShortLeavesTaken(txtEmploeeId.Text.Trim(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());

                if ((shortLeavesApplied > 0) && (shortLeavesApplied + shortLeaveTakent > Constants.MONTHLY_SHORT_LEAVE_LIMIT))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You have only " + (Constants.MONTHLY_SHORT_LEAVE_LIMIT).ToString() + " short leaves", lblMessage);
                    return;
                }

                //double casualLeaveTaken = Double.Parse(acLeaves.Compute("Sum(leaves_taken)", "LEAVE_TYPE_ID ='CASUAL'").ToString());
                //double annualLeaveTaken = Double.Parse(acLeaves.Compute("Sum(leaves_taken)", "LEAVE_TYPE_ID ='ANNUAL'").ToString());
                ////double leaveRemain = Double.Parse(acLeaves.Compute("Sum(leaves_taken)", "").ToString());
                ////double leaveTaken = Double.Parse(acLeaves.Compute("Sum(leaves_taken)", "").ToString());

                if ((casualApplied + casualLeaveTaken) > double.Parse(casualLeaves.ToString()))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You have only " + (double.Parse(casualLeaves.ToString()) - casualLeaveTaken).ToString() + " CASUAL leaves", lblMessage);
                    return;
                }

                if ((anualAplied + annualLeaveTaken) > double.Parse(anualLeaves.ToString()))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You have only " + (double.Parse(anualLeaves.ToString()) - annualLeaveTaken).ToString() + " ANNUAL leaves", lblMessage);
                    return;
                }
                //----
                // following code takes minimum and maximum dates from table
                string minDate = leaveBucket_.Compute("min(LEAVE_DATE)", "IS_DAY_OFF='0'").ToString();
                string maxDate = leaveBucket_.Compute("max(LEAVE_DATE)", "IS_DAY_OFF='0'").ToString();
                //
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    log.Debug("EmployeeOnlineLeave : btnSave_Click() -> Insert");

                    String leaveSheetId = leaveScheduleDataHandler.Insert(leaveBucket_, userId, txtEmploeeId.Text.Trim(), lslNo, minDate.Trim(), maxDate.Trim(),
                                         txtCoveredBy.Text.Trim(), totalLeaves, txtRecommendBy.Text.Trim(), Constants.LEAVE_STATUS_ACTIVE_VALUE, txtReason.Text.Trim());
                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

                    if (leaveSheetId != "")
                    {
                        hfLeaveSheetId.Value = "";
                        hfLeaveSheetId.Value = leaveSheetId;
                        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('Employee is saved ..')", true); 
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Leave sheet is saved", lblMessage);
                    }


                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    log.Debug("EmployeeOnlineLeave : btnSave_Click() -> Update");

                    if (hfLeaveSheetId.Value == "")
                    {
                        Utility.Errorhandler.GetError("2", "Leave sheet can not be updated", lblMessage);
                    }

                    Boolean isUpdated = leaveScheduleDataHandler.update(leaveBucket_, userId, txtEmploeeId.Text.Trim(), lslNo, minDate.Trim(), maxDate.Trim(),
                                         txtCoveredBy.Text.Trim(), totalLeaves, txtRecommendBy.Text.Trim(), Constants.LEAVE_STATUS_ACTIVE_VALUE, txtReason.Text.Trim(), hfLeaveSheetId.Value.ToString());
                    if (isUpdated)
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Leave sheet is updated", lblMessage);
                    }

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                }

                fillHistory();

                //////string sMail = employeeDataHandler.getEmployeeEmail(txtRecommendBy.Text.Trim());


                //////if (sMail.Trim() != "")
                //////{

                //////    EmailHandler.SendDefaultEmail("Leave System", sMail, "", "Leave recommandation", getMailBody(empName, minDate.Trim(), maxDate.Trim(), sTotalLeaves, Constants.CON_LEAVE_RECOMMEND));
                //////}

                string empName = employeeDataHandler.getEmployeeName(txtEmploeeId.Text.Trim());
                string coveredMail = employeeDataHandler.getEmployeeEmail(txtCoveredBy.Text.Trim());

                if (coveredMail.Trim() != "")
                {
                    //EmailHandler.SendDefaultEmail("Leave System", coveredMail, "", "Duty Covering", getMailBody(empName, txtFDate.Text.Trim(), txtTDate.Text.Trim(), sTotalLeaves, Constants.CON_LEAVE_COVER));
                    EmailHandler.SendDefaultEmailHtml("Leave System", coveredMail, "", "Duty Covering", getMailBodyHtml(empName, txtFDate.Text.Trim(), txtTDate.Text.Trim(), sTotalLeaves, Constants.CON_LEAVE_COVER, hfLeaveSheetId.Value.ToString()));
                }
            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
            finally
            {
                employeeLeaveSchemeDataHandler = null;
                leaveScheduleDataHandler = null;
                leaveConstrains = null;
            }
        }

        private StringBuilder getMailBodyHtml(string employeeName, string sFromDate, string sToDate, string noDays, char covRec, string lsId)
        {
            log.Debug("EmployeeOnlineLeave : getMailBodyHtml()");

            PasswordHandler crpto = new PasswordHandler();
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine + "</br></br>");
            if (sFromDate.Trim() == sToDate.Trim())
            {
                stringBuilder.Append(employeeName + " has applyed a leave on " + sFromDate + "." + Environment.NewLine + "</br>");
            }
            else
            {
                stringBuilder.Append(employeeName + " has applyed " + noDays.ToString() + " leaves from " + sFromDate + " to " + sToDate + "." + Environment.NewLine + "</br>");
            }

            if (covRec == Constants.CON_LEAVE_COVER)
            {
                stringBuilder.Append("Please cover duties." + Environment.NewLine + Environment.NewLine + "</br></br>");
                //stringBuilder.Append(""Please <a href=\"http://www.example.com/login.aspx\">login</a>");
                string link1 = "Please <a href=\"http://" + ConfigurationManager.AppSettings["host_Port"] + "/EmployeeLeave/webFrmApprove.aspx" + "?LsId=" + crpto.Encrypt(lsId) + "&CRF=" + crpto.Encrypt(Constants.LEAVE_SHEET_COVER_APPROVAL_FLAG) + "&ARF=" + crpto.Encrypt(Constants.LEAVE_SHEET_APPROVAL_FLAG) + "\"><b>AGREE</b></a> or ";
                stringBuilder.Append(link1);
                string link2 = "</t><a href=\"http://" + ConfigurationManager.AppSettings["host_Port"] + "/EmployeeLeave/webFrmApprove.aspx" + "?LsId=" + crpto.Encrypt(lsId) + "&CRF=" + crpto.Encrypt(Constants.LEAVE_SHEET_COVER_APPROVAL_FLAG) + "&ARF=" + crpto.Encrypt(Constants.LEAVE_SHEET_REJECT_FLAG) + "\"><b>REJECT</b></a></br>";
                stringBuilder.Append(link2);


            }
            else if (covRec == Constants.CON_LEAVE_RECOMMEND)
            {
                stringBuilder.Append("Please recommand it." + Environment.NewLine + Environment.NewLine + "</br></br>");
                string link1 = "Please <a href=\"http://localhost:10641/EmployeeLeave/webFrmApprove.aspx" + "?LsId=" + crpto.Encrypt(lsId) + "&CRF=" + crpto.Encrypt(Constants.LEAVE_SHEET_COVER_APPROVAL_FLAG) + "&ARF=" + crpto.Encrypt(Constants.LEAVE_SHEET_APPROVAL_FLAG) + "\"><b>APPROVE</b></a> or ";
                stringBuilder.Append(link1);
                string link2 = "</t><a href=\"http://localhost:10641/EmployeeLeave/webFrmApprove.aspx" + "?LsId=" + crpto.Encrypt(lsId) + "&CRF=" + crpto.Encrypt(Constants.LEAVE_SHEET_COVER_APPROVAL_FLAG) + "&ARF=" + crpto.Encrypt(Constants.LEAVE_SHEET_REJECT_FLAG) + "\"><b>REJECT</b></a></br>";
                stringBuilder.Append(link2);
            }
            stringBuilder.Append("</br>" + "Thank you." + Environment.NewLine + Environment.NewLine + "</br></br>");
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }

        protected void fillHistory()
        {
            log.Debug("EmployeeOnlineLeave : fillHistory()");

            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            try
            {
                string sYear = DateTime.Today.Year.ToString();
                gvLeaveHistory.DataSource = leaveScheduleDataHandler.getLeavesHistory(txtEmploeeId.Text.Trim(), sYear);
                gvLeaveHistory.DataBind();
            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
            finally
            {
                leaveScheduleDataHandler = null;
            }
        }

        protected void chkIS_DAY_OFF_OnCheckedChanged(object sender, EventArgs e)
        {
            log.Debug("EmployeeOnlineLeave : chkIS_DAY_OFF_OnCheckedChanged()");

            int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
            CheckBox cb = (CheckBox)gvLeaveSheet.Rows[selRowIndex].FindControl("chkIS_DAY_OFF");

            DataTable leaveBucket_ = new DataTable();


            if (cb.Checked == true)
            {
                if (Session["leaveBucket"] != null)
                {
                    leaveBucket_ = (DataTable)Session["leaveBucket"];
                    leaveBucket_.Rows[selRowIndex][13] = Constants.LEAVE_IS_OFF_DAY_YES;

                    Session["leaveBucket"] = leaveBucket_;

                    gvLeaveSheet.DataSource = leaveBucket_;
                    gvLeaveSheet.DataBind();

                }
                //Perform your logic
            }
            else if (cb.Checked == false)
            {
                leaveBucket_ = (DataTable)Session["leaveBucket"];
                leaveBucket_.Rows[selRowIndex][13] = Constants.LEAVE_IS_OFF_DAY_NO;

                Session["leaveBucket"] = leaveBucket_;

                gvLeaveSheet.DataSource = leaveBucket_;
                gvLeaveSheet.DataBind();

            }
        }

        protected void chkDiscard_OnCheckedChanged(object sender, EventArgs e)
        {
            log.Debug("EmployeeOnlineLeave : chkDiscard_OnCheckedChanged()");

            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

            int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
            CheckBox cb = (CheckBox)gvLeaveHistory.Rows[selRowIndex].FindControl("chkDiscard");

            string userId = "";

            try
            {
                if (Session["KeyUSER_ID"] != null)
                {
                    userId = Session["KeyUSER_ID"].ToString();
                }
                else
                {
                    Utility.Errorhandler.GetError("2", "Your session expired", lblMessage);
                    return;
                }

                if (cb.Checked)
                {
                    string leaveSheetId = gvLeaveHistory.Rows[selRowIndex].Cells[0].Text.Trim();
                    Boolean isDiscarded = leaveScheduleDataHandler.updateStatus(leaveSheetId, Constants.LEAVE_STATUS_DISCARDED, userId);

                    if (isDiscarded)
                    {
                        Utility.Errorhandler.GetError("2", "Leave sheet is discarded", lblMessage);
                    }

                    fillHistory();
                }
            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }

        }

        protected void ddlLeaveType_Update(object sender, EventArgs e)
        {
            log.Debug("EmployeeOnlineLeave : ddlLeaveType_Update()");
            try
            {
                Utility.Errorhandler.ClearError(lblMessage);

                int selRowIndex = ((GridViewRow)(((DropDownList)sender).Parent.Parent)).RowIndex;
                DropDownList ddlLeaveTypes = (DropDownList)gvLeaveSheet.Rows[selRowIndex].FindControl("ddlLeaveType");

                String leaveTypeId = "";

                if (ddlLeaveTypes.SelectedIndex != 0)
                {
                    leaveTypeId = ddlLeaveTypes.SelectedValue.Trim();

                    if (gvLeaveSheet.Rows[selRowIndex].Cells[4].Text.Trim() != "")
                    {
                        double dNoDays = double.Parse(gvLeaveSheet.Rows[selRowIndex].Cells[4].Text.Trim());

                        if ((dNoDays == 1) && (leaveTypeId == Constants.CON_SHORT_LEAVE_LEAVE_ID))
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "A SHORT LEAVE can not be assign to a full day", lblMessage);
                            return;
                        }
                        else if ((dNoDays == 0.5) && (leaveTypeId == Constants.CON_SHORT_LEAVE_LEAVE_ID))
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "A SHORT LEAVE can not be assign to a half day", lblMessage);
                            return;
                        }
                        else if ((leaveTypeId != Constants.CON_SHORT_LEAVE_LEAVE_ID) && (dNoDays == 0.25))
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Only SHORT LEAVE can be assigned ", lblMessage);
                            return;
                        }
                    }


                    if (Session["leaveBucket"] != null)
                    {
                        DataTable leaveBucket_ = new DataTable();
                        leaveBucket_ = (DataTable)Session["leaveBucket"];
                        leaveBucket_.Rows[selRowIndex][2] = leaveTypeId;

                        Session["leaveBucket"] = leaveBucket_;

                        gvLeaveSheet.DataSource = leaveBucket_;
                        gvLeaveSheet.DataBind();

                    }
                }
            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
        }

        protected void gvLeaveSheet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("EmployeeOnlineLeave : gvLeaveSheet_RowDataBound()");

            EmployeeLeaveSchemeDataHandler employeeLeaveSchemeDataHandler = new EmployeeLeaveSchemeDataHandler();

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataTable leaveTypes = new DataTable();

                    leaveTypes = employeeLeaveSchemeDataHandler.getLeaveTypesOfActiveLeaveSchemeForOnline(txtEmploeeId.Text.Trim()).Copy();

                    //Find the DropDownList in the Row
                    DropDownList ddlLeaveTypes = (e.Row.FindControl("ddlLeaveType") as DropDownList);
                    ddlLeaveTypes.DataSource = leaveTypes;
                    ddlLeaveTypes.DataTextField = "LEAVE_TYPE_NAME";
                    ddlLeaveTypes.DataValueField = "LEAVE_TYPE_ID";
                    ddlLeaveTypes.DataBind();

                    //Add Default Item in the DropDownList
                    ddlLeaveTypes.Items.Insert(0, new ListItem("Please select"));

                    if (Session["leaveBucket"] != null)
                    {
                        DataTable dtTemp = (DataTable)Session["leaveBucket"];
                        int iIndex = e.Row.RowIndex;

                        if (dtTemp.Rows[iIndex][2].ToString().Trim() != "")
                        {
                            ddlLeaveTypes.SelectedValue = dtTemp.Rows[iIndex][2].ToString().Trim();
                        }
                    }


                    //Select the Country of Customer in DropDownList
                    //string country = (e.Row.FindControl("lblCountry") as Label).Text;
                    //ddlCountries.Items.FindByValue(country).Selected = true;
                }
            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }

        }

        protected void gvLeaveHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("EmployeeOnlineLeave : gvLeaveHistory_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkDiscard = (CheckBox)e.Row.FindControl("chkDiscard");

                    if ((e.Row.Cells[5].Text.Trim() == Constants.LEAVE_STATUS_ACTIVE_VALUE) || (e.Row.Cells[5].Text.Trim() == Constants.LEAVE_STATUS_RECOMMAND))
                    {
                        chkDiscard.Enabled = true;
                    }
                    else if ((e.Row.Cells[5].Text.Trim() == Constants.LEAVE_STATUS_APPROVED) || (e.Row.Cells[5].Text.Trim() == Constants.LEAVE_STATUS_REJECTED) || (e.Row.Cells[5].Text.Trim() == Constants.LEAVE_STATUS_DISCARDED))
                    {
                        chkDiscard.Enabled = false;
                        e.Row.BackColor = Color.LightSkyBlue;
                    }

                    //foreach (TableCell cell in e.Row.Cells)
                    //{
                    //    string CompHoliday = e.Row.Cells[3].Text.ToString();

                    //    if (CompHoliday == Constants.CON_CALENDER_NON_WROK_DAY)
                    //    {
                    //        cell.BackColor = Color.LightSkyBlue;
                    //        theDropDownList.Enabled = false;
                    //        chkDiscard.Enabled = false;
                    //    }
                    //}

                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvLeaveHistory, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }




            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("EmployeeOnlineLeave : btnClear_Click()");
            clear();
        }

        protected void clear()
        {
            log.Debug("EmployeeOnlineLeave : clear()");

            try
            {
                Utility.Utils.clearControls(false, txtEmploeeId, txtCoveredBy, txtFDate, txtReason, txtRecommendBy, txtcoveredbyname, txtrecbybyname, txtTDate, chkhalfDay, chkSL);

                gvLeaveSummary.DataSource = null;
                gvLeaveSummary.DataBind();

                gvLeaveSheet.DataSource = null;
                gvLeaveSheet.DataBind();

                gvLeaveHistory.DataSource = null;
                gvLeaveHistory.DataBind();

                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;

                Utility.Errorhandler.ClearError(lblMessage);

                hfEmpId.Value = "";

                if (Session["leaveBucket"] != null)
                {
                    DataTable leaveBucket_ = new DataTable();
                    leaveBucket_ = (DataTable)Session["leaveBucket"];

                    leaveBucket_.Clear();
                    Session["leaveBucket"] = leaveBucket_;
                }

                clearSessionTables();
            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
        }

        protected void clearSessionTables()
        {
            log.Debug("EmployeeOnlineLeave : clearSessionTables()");

            if (Session["leavesACM"] != null)
            {
                DataTable dtLeaveACM = new DataTable();
                dtLeaveACM = (DataTable)Session["leavesACM"];
                dtLeaveACM.Clear();
                Session["leavesACM"] = dtLeaveACM;

            }

            if (Session["leavesAC"] != null)
            {
                DataTable dtLeaveAC = new DataTable();
                dtLeaveAC = (DataTable)Session["leavesACM"];
                dtLeaveAC.Clear();
                Session["leavesAC"] = dtLeaveAC;
            }

        }

        protected void gvLeaveHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("EmployeeOnlineLeave : gvLeaveHistory_SelectedIndexChanged()");

            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

            try
            {
                string empLeaveSchemeId = gvLeaveHistory.SelectedRow.Cells[0].Text.ToString().Trim();

                if (empLeaveSchemeId.Trim() != String.Empty)
                {
                    DataTable lsDetails = new DataTable();
                    lsDetails = leaveScheduleDataHandler.getLeaveSheetDetails(empLeaveSchemeId).Copy();
                    gvLeaveSheetDetails.DataSource = lsDetails;
                    gvLeaveSheetDetails.DataBind();

                    if (lsDetails.Rows.Count > 0)
                    {
                        lbtnClear.Visible = true;
                        lblLSDetails.Visible = true;
                    }
                }


            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
            finally
            {
                leaveScheduleDataHandler = null;
            }
        }

        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            gvLeaveSheetDetails.DataSource = null;
            gvLeaveSheetDetails.DataBind();

            lblLSDetails.Visible = false;
            lbtnClear.Visible = false;
        }
    }
}