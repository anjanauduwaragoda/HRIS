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

namespace GroupHRIS.EmployeeLeave
{
    public partial class webFrmEmployeeLeaveSheet : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private string sIPAddress = "";

        private string sUserId = "";

        private string KeyEMPLOYEE_ID = "";
        private string KeyCOMP_ID = "";
        private string KeyHRIS_ROLE = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            lbtnClear.Visible = false;
            lblLSDetails.Visible = false;

            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "webFrmEmployeeLeaveSheet : Page_Load");

            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {
                sUserId = Session["KeyUSER_ID"].ToString();
            }

            if (!IsPostBack)
            {
                KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                KeyCOMP_ID = (string)(Session["KeyCOMP_ID"]);
                KeyHRIS_ROLE = (string)Session["KeyHRIS_ROLE"];

                
                createLeaveBucket();
                fillHours(ddlFromHH);
                fillMinutes(ddlFromMM);
                fillHours(ddlToHH);
                fillMinutes(ddlToMM);

                /**/if (KeyHRIS_ROLE == Constants.CON_COMMON_KeyHRIS_ROLE)
                {
                    txtEmploeeId.ReadOnly = true;
                    Search.Visible = false;
                    txtEmploeeId.Text = KeyEMPLOYEE_ID;
                    fillLeaveSummary(txtEmploeeId.Text.Trim());
                    getSupervisor(txtEmploeeId.Text.Trim());
                    fillHistory();

                    //Load Employee Name | 02/02/2017 | Chathura
                    lblEmployeeName.Text = LoadEmployeeName(txtEmploeeId.Text.Trim());
                    //
                }
                else
                {
                    txtEmploeeId.Enabled = true;
                    Search.Visible = true;
                    txtEmploeeId.Text = KeyEMPLOYEE_ID;
                    fillLeaveSummary(txtEmploeeId.Text.Trim());
                    getSupervisor(txtEmploeeId.Text.Trim());
                    fillHistory();
                
                    //Load Employee Name | 02/02/2017 | Chathura
                    lblEmployeeName.Text = LoadEmployeeName(txtEmploeeId.Text.Trim());
                    //
                }/**/
            }
            else
            {
                Utility.Errorhandler.ClearError(lblMessage);

                //if ((hfEmpId.Value.Trim() == "") || (hfEmpId.Value.Trim() != txtEmploeeId.Text.Trim()))
                //{
                //    hfEmpId.Value = txtEmploeeId.Text.Trim();

                //    DataTable leaveBucket_ = (DataTable)Session["leaveBucket"];
                //    leaveBucket_.Rows.Clear();
                //    Session["leaveBucket"] = leaveBucket_;

                //    clearSessionTables();

                //    fillLeaveSummary(txtEmploeeId.Text.Trim());
                //    getSupervisor(txtEmploeeId.Text.Trim());
                //    fillHistory();
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
                        clear();
                        txtEmploeeId.Text = hfVal.Value;
                        hfEmpId.Value = txtEmploeeId.Text.Trim();

                        DataTable leaveBucket_ = (DataTable)Session["leaveBucket"];
                        leaveBucket_.Rows.Clear();
                        Session["leaveBucket"] = leaveBucket_;

                        clearSessionTables();

                        fillLeaveSummary(txtEmploeeId.Text.Trim());
                        getSupervisor(txtEmploeeId.Text.Trim());

                        //if (KeyHRIS_ROLE != Constants.CON_COMMON_KeyHRIS_ROLE)
                        //{
                            fillHistory();
                        //}


                        //Load Employee Name | 02/02/2017 | Chathura
                            lblEmployeeName.Text = LoadEmployeeName(txtEmploeeId.Text.Trim());
                        //

                    }
                }
                if (hfCaller.Value == "txtCoveredBy")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtCoveredBy.Text = hfVal.Value;
                    }
                    if (txtCoveredBy.Text != "")
                    {
                        //Postback Methods

                        //Load Covered By Name | 02/02/2017 | Chathura
                        lblCoveredByName.Text = LoadEmployeeName(txtCoveredBy.Text.Trim());
                        //
                    }
                }
                if (hfCaller.Value == "txtRecommendBy")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtRecommendBy.Text = hfVal.Value;
                    }
                    if (txtRecommendBy.Text != "")
                    {
                        //Postback Methods

                        //Load Recomended By Name | 02/02/2017 | Chathura
                        lblRecomendedByName.Text = LoadEmployeeName(txtRecommendBy.Text.Trim());
                        //
                    }
                }
            }
        }

        private void fillLeaveSummary(string employeeId)
        {
            log.Debug("webFrmEmployeeLeaveSheet : fillLeaveSummary()");

            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            LeaveConstrains leaveConstrains = new LeaveConstrains();

            DataTable leaveSummary = new DataTable();

            try
            {
                DataTable dtSummary = new DataTable();

                dtSummary.Columns.Add("LEAVE_TYPE_NAME", typeof(string));
                dtSummary.Columns.Add("NUMBER_OF_DAYS", typeof(string));
                dtSummary.Columns.Add("leaves_taken", typeof(string));
                dtSummary.Columns.Add("Leves_Remain", typeof(string));
                
                string sYear = DateTime.Today.Year.ToString();
                int iYear = DateTime.Today.Year;

                leaveSummary = leaveScheduleDataHandler.getEmployeeLeveSummary(employeeId.Trim(), sYear).Copy();

                decimal anualLeaves = leaveConstrains.availableAnnualLeaves(txtEmploeeId.Text.Trim(), iYear);
                decimal casualLeaves = leaveConstrains.availableCasualLeaves(txtEmploeeId.Text.Trim(), iYear);

                double shortLeaveTakent = 0;

                foreach (DataRow dr in leaveSummary.Rows)
                {
                    DataRow oDataRow = dtSummary.NewRow();
                    
                    oDataRow["LEAVE_TYPE_NAME"]= dr["LEAVE_TYPE_NAME"].ToString();

                    if (dr["LEAVE_TYPE_ID"].ToString() == Constants.CON_ANNUAL_LEAVE_ID)
                    {
                         oDataRow["NUMBER_OF_DAYS"] = anualLeaves.ToString();
                         
                         decimal leavesTaken = 0;

                         if (dr["leaves_taken"].ToString().Trim() != "")
                         {
                             leavesTaken = decimal.Parse(dr["leaves_taken"].ToString().Trim());
                         }
                         
                         oDataRow["Leves_Remain"] = (anualLeaves - leavesTaken).ToString(); 

                    }
                    else if (dr["LEAVE_TYPE_ID"].ToString() == Constants.CON_CASUAL_LEAVE_ID)
                    {
                        oDataRow["NUMBER_OF_DAYS"] = casualLeaves.ToString();

                        decimal leavesTaken = 0;

                        if (dr["leaves_taken"].ToString().Trim() != "")
                        {
                            leavesTaken = decimal.Parse(dr["leaves_taken"].ToString().Trim());
                        }

                        oDataRow["Leves_Remain"] = (casualLeaves - leavesTaken).ToString();
                    }
                        // short leave
                    else if (dr["LEAVE_TYPE_ID"].ToString() == Constants.CON_SHORT_LEAVE_LEAVE_ID)
                    {
                        oDataRow["NUMBER_OF_DAYS"] = Constants.MONTHLY_SHORT_LEAVE_LIMIT;

                        double leavesRemain = 0;
                        shortLeaveTakent = leaveScheduleDataHandler.getMonthlyShortLeavesTaken(txtEmploeeId.Text.Trim(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());

                        leavesRemain = Constants.MONTHLY_SHORT_LEAVE_LIMIT - shortLeaveTakent;

                        oDataRow["Leves_Remain"] = leavesRemain.ToString();
                    }

                    else
                    {
                        oDataRow["NUMBER_OF_DAYS"] = dr["NUMBER_OF_DAYS"].ToString();
                    }

                    if (dr["LEAVE_TYPE_ID"].ToString() == Constants.CON_SHORT_LEAVE_LEAVE_ID)
                    {
                        oDataRow["leaves_taken"] = shortLeaveTakent;
                    }
                    else
                    {
                        oDataRow["leaves_taken"] = dr["leaves_taken"].ToString();
                    }

                    dtSummary.Rows.Add(oDataRow);
                }
                if (txtEmploeeId.Text.Trim()== "")
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Select an employee", lblMessage);
                    return;
                }
                else if (dtSummary.Rows.Count == 0)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Assign a leave scheme to employee", lblMessage);
                    return;
                }

                gvLeaveSummary.DataSource = dtSummary;
                gvLeaveSummary.DataBind();

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
                leaveConstrains = null;
                leaveSummary.Dispose();
            }
        }        

        private void getSupervisor(string employeeId)
        {
            log.Debug("webFrmEmployeeLeaveSheet : getSupervisor()");

            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            try
            {
                txtRecommendBy.Text = employeeDataHandler.getEmployeeSupervisor(employeeId);


                //Load Recomended By Name | 02/02/2017 | Chathura
                lblRecomendedByName.Text = LoadEmployeeName(txtRecommendBy.Text.Trim());
                //
            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError);
                throw ex;
            }
            finally
            {
                employeeDataHandler = null;
            }
        }

        private void fillHours(DropDownList ddl)
        {
            log.Debug("webFrmEmployeeLeaveSheet : fillHours()");

            ListItem Item = new ListItem();
            Item.Text = "";
            ddl.Items.Add(Item);

            for (int i = 1; i < 24; i++)
            {
                ListItem listItem = new ListItem();
                listItem.Text = i.ToString("0#");
                ddl.Items.Add(listItem);
            }
        }

        private void fillMinutes(DropDownList ddl)
        {
            log.Debug("webFrmEmployeeLeaveSheet : fillMinutes()");

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
            log.Debug("webFrmEmployeeLeaveSheet : btnApply_Click()");

            EmployeeLeaveSchemeDataHandler employeeLeaveSchemeDataHandler = new EmployeeLeaveSchemeDataHandler();
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            
            DateTime mFromDate = Convert.ToDateTime(txtFDate.Text.ToString());
            DateTime mToDate = Convert.ToDateTime(txtTDate.Text.ToString());
            TimeSpan ts = mToDate - mFromDate;
            double ndays = 0;
                       
            try
            {
                btnSave.Enabled = true;

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

                if ((chkhalfDay.Checked == true) && ((rbM.Checked == false) && (rbE.Checked == false)))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Select half day is on first half or second half of the day", lblMessage);
                    return;
                    
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

                if ((mToDate >= mFromDate) && (chkhalfDay.Checked == false) && (chkSL.Checked==false))
                {                    
                    for (int i = 0; i < ndays; i++)
                    {
                       
                        DateTime mNextDate = mFromDate.AddDays(i);
                        //put leave existance check on leavebucket
                        //if (isLeaveExist(leaveBucket_, mNextDate.ToString("yyyy/MM/dd")) != true)
                        //{
                            //if (leaveScheduleDataHandler.isOnLeave(txtCoveredBy.Text.Trim(),mNextDate.ToString("yyyy/MM/dd")) == true)
                            //{
                            //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Covering person is leave on " + mNextDate.ToString("yyyy/MM/dd"), lblMessage);
                            //    return;
                            //}

                            //if (leaveScheduleDataHandler.isOnLeave(txtEmploeeId.Text.Trim(), mNextDate.ToString("yyyy/MM/dd")) == true)
                            //{
                            //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You are leave on " + mNextDate.ToString("yyyy/MM/dd"), lblMessage);
                            //    return;
                            //}
                                                       
                            DataTable dtRosterTime = leaveScheduleDataHandler.getRosterWorkingTime(txtEmploeeId.Text.Trim(),mNextDate.ToString("yyyy/MM/dd"));

                            if (dtRosterTime.Rows.Count > 0)
                            {
                                foreach (DataRow drRTime in dtRosterTime.Rows)
                                {
                                    DataTable dtRLeaves = new DataTable();
                                    DataTable dtCoveringPerson = new DataTable();

                                    dtCoveringPerson = leaveScheduleDataHandler.isLeaveExistForTheRoster(txtCoveredBy.Text.Trim(), mNextDate.ToString("yyyy/MM/dd"), drRTime["ROSTR_ID"].ToString());

                                    if (dtCoveringPerson.Rows.Count > 0)
                                    {
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Covering person is leave on " + mNextDate.ToString("yyyy/MM/dd") + " From " + drRTime["FROM_TIME"].ToString() + " To " + drRTime["TO_TIME"].ToString(), lblMessage);
                                        return;
                                    }

                                    if(isLeaveExistForRoster(leaveBucket_, mNextDate.ToString("yyyy/MM/dd"),drRTime["ROSTR_ID"].ToString()) != true)
                                    {   
                                        dtRLeaves = leaveScheduleDataHandler.isLeaveExistForTheRoster(txtEmploeeId.Text.Trim(), mNextDate.ToString("yyyy/MM/dd"), drRTime["ROSTR_ID"].ToString());

                                        DataRow oDataRow = leaveBucket_.NewRow();

                                        if (dtRLeaves.Rows.Count > 0)
                                        {
                                            // if leave exist that data is displayed from this code

                                            DataRow drRow = dtRLeaves.Rows[0];

                                            oDataRow["EMPLOYEE_ID"] = txtEmploeeId.Text.Trim();
                                            oDataRow["LEAVE_DATE"] = mNextDate.ToString("yyyy/MM/dd");
                                            oDataRow["LEAVE_TYPE"] = drRow["LEAVE_TYPE_ID"].ToString();

                                            if (drRTime != null)
                                            {
                                                oDataRow["ROSTR_ID"] = drRTime["ROSTR_ID"].ToString();
                                                oDataRow["FROM_TIME"] = drRow["FROM_TIME"].ToString();
                                                oDataRow["TO_TIME"] = drRow["TO_TIME"].ToString();
                                            }
                                            else
                                            {
                                                oDataRow["ROSTR_ID"] = "";
                                                oDataRow["FROM_TIME"] = "";
                                                oDataRow["TO_TIME"] = "";
                                            }
                                            oDataRow["COVERED_BY"] = drRow["COVERED_BY"].ToString(); ;
                                            oDataRow["RECOMMEND_BY"] = drRow["RECOMMAND_BY"].ToString(); 
                                            oDataRow["SCHEME_LINE_NO"] = lineNo.ToString();
                                            oDataRow["REMARKS"] = "";
                                            oDataRow["NO_OF_DAYS"] = double.Parse(drRow["NO_OF_DAYS"].ToString());

                                            oDataRow["IS_HALFDAY"] = drRow["IS_HALFDAY"].ToString();
                                            oDataRow["LEAVE_STATUS"] = drRow["LEAVE_STATUS"].ToString(); 
                                            oDataRow["IS_DAY_OFF"] = Constants.LEAVE_IS_OFF_DAY_YES;


                                            if (drRTime != null)
                                            {
                                                oDataRow["COM_ROS_TIME"] = drRow["FROM_TIME"].ToString().Trim() + '-' + drRow["TO_TIME"].ToString().Trim();
                                                //////string[] sAFrom = drRow["FROM_TIME"].ToString().Trim().Split(':');
                                                //////string sFrom = sAFrom[0].Trim() + sAFrom[1].Trim();
                                                //////oDataRow["FROM"] = Convert.ToInt32(sFrom);

                                                //////string[] sATo = drRow["TO_TIME"].ToString().Trim().Split(':');
                                                //////string sTo = sATo[0].Trim() + sATo[1].Trim();
                                                //////oDataRow["TO"] = Convert.ToInt32(sTo);
                                            }
                                            ////oDataRow["NLEAVE"] = "";
                                            oDataRow["STATUS"] = Constants.CON_ON_LEAVE;

                                            leaveBucket_.Rows.Add(oDataRow);

                                        }
                                        else
                                        {
                                            // new record is taken here 

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


                                            if (drRTime != null)
                                            {
                                                oDataRow["COM_ROS_TIME"] = drRTime["FROM_TIME"].ToString().Trim() + '-' + drRTime["TO_TIME"].ToString().Trim();
                                                //////string[] sAFrom = drRTime["FROM_TIME"].ToString().Trim().Split(':');
                                                //////string sFrom = sAFrom[0].Trim() + sAFrom[1].Trim();
                                                //////oDataRow["FROM"] = Convert.ToInt32(sFrom);

                                                //////string[] sATo = drRTime["TO_TIME"].ToString().Trim().Split(':');
                                                //////string sTo = sATo[0].Trim() + sATo[1].Trim();
                                                //////oDataRow["TO"] = Convert.ToInt32(sTo);
                                            }
                                            //////oDataRow["NLEAVE"] = Constants.CON_NLEAVE_FDAY;
                                            oDataRow["STATUS"] = "";

                                            leaveBucket_.Rows.Add(oDataRow);
                                        }

                                        dtRLeaves.Dispose();
                                        dtCoveringPerson.Dispose();
                                    }
                                }

                                dtRosterTime.Dispose();
                            }
                            else
                            {
                                DataTable dtCLeaves = new DataTable();
                                DataTable dtCCoveringPerson = new DataTable();

                                dtCCoveringPerson = leaveScheduleDataHandler.isLeaveExistForTheDay(txtCoveredBy.Text.Trim(), mNextDate.ToString("yyyy/MM/dd"));

                                if (dtCCoveringPerson.Rows.Count > 0)
                                {
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Covering person is leave on " + mNextDate.ToString("yyyy/MM/dd"), lblMessage);
                                    return;
                                }

                                if (isLeaveExist(leaveBucket_, mNextDate.ToString("yyyy/MM/dd")) != true)
                                {
                                    dtCLeaves = leaveScheduleDataHandler.isLeaveExistForTheDay(txtEmploeeId.Text.Trim(), mNextDate.ToString("yyyy/MM/dd"));

                                    DataRow drTime = leaveScheduleDataHandler.getCompanyWorkingTime(txtEmploeeId.Text.Trim());

                                    DataRow oDataRow = leaveBucket_.NewRow();

                                    // details of existing leave is shown here
                                    if (dtCLeaves.Rows.Count > 0)
                                    {
                                        DataRow drRow = dtCLeaves.Rows[0];


                                        oDataRow["EMPLOYEE_ID"] = txtEmploeeId.Text.Trim();
                                        oDataRow["LEAVE_DATE"] = mNextDate.ToString("yyyy/MM/dd");
                                        oDataRow["LEAVE_TYPE"] = "";

                                        if (drTime != null)
                                        {
                                            oDataRow["ROSTR_ID"] = "";
                                            oDataRow["FROM_TIME"] = drRow["FROM_TIME"].ToString();
                                            oDataRow["TO_TIME"] = drRow["TO_TIME"].ToString();
                                        }
                                        else
                                        {
                                            oDataRow["ROSTR_ID"] = "";
                                            oDataRow["FROM_TIME"] = "";
                                            oDataRow["TO_TIME"] = "";
                                        }
                                        oDataRow["COVERED_BY"] = drRow["COVERED_BY"].ToString(); ;
                                        oDataRow["RECOMMEND_BY"] = drRow["RECOMMAND_BY"].ToString();
                                        oDataRow["SCHEME_LINE_NO"] = lineNo.ToString();
                                        oDataRow["REMARKS"] = "";
                                        oDataRow["NO_OF_DAYS"] = double.Parse(drRow["NO_OF_DAYS"].ToString());

                                        oDataRow["IS_HALFDAY"] = drRow["IS_HALFDAY"].ToString();
                                        oDataRow["LEAVE_STATUS"] = drRow["LEAVE_STATUS"].ToString();
                                        oDataRow["IS_DAY_OFF"] = Constants.LEAVE_IS_OFF_DAY_YES;


                                        if (drTime != null)
                                        {
                                            oDataRow["COM_ROS_TIME"] = drTime["FROM_TIME"].ToString().Trim() + '-' + drTime["TO_TIME"].ToString().Trim();

                                            //////string[] sAFrom = drTime["FROM_TIME"].ToString().Trim().Split(':');
                                            //////string sFrom = sAFrom[0].Trim() + sAFrom[1].Trim();
                                            //////oDataRow["FROM"] = Convert.ToInt32(sFrom);

                                            //////string[] sATo = drTime["TO_TIME"].ToString().Trim().Split(':');
                                            //////string sTo = sATo[0].Trim() + sATo[1].Trim();
                                            //////oDataRow["TO"] = Convert.ToInt32(sTo);
                                        }

                                        ////oDataRow["NLEAVE"] = Constants.CON_NLEAVE_FDAY;
                                        oDataRow["STATUS"] = Constants.CON_ON_LEAVE;

                                        leaveBucket_.Rows.Add(oDataRow);

                                    }
                                    else
                                    {
                                        // adding new leave

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


                                        if (drTime != null)
                                        {
                                            oDataRow["COM_ROS_TIME"] = drTime["FROM_TIME"].ToString().Trim() + '-' + drTime["TO_TIME"].ToString().Trim();

                                            //////string[] sAFrom = drTime["FROM_TIME"].ToString().Trim().Split(':');
                                            //////string sFrom = sAFrom[0].Trim() + sAFrom[1].Trim();
                                            //////oDataRow["FROM"] = Convert.ToInt32(sFrom);

                                            //////string[] sATo = drTime["TO_TIME"].ToString().Trim().Split(':');
                                            //////string sTo = sATo[0].Trim() + sATo[1].Trim();
                                            //////oDataRow["TO"] = Convert.ToInt32(sTo);
                                        }

                                        ////oDataRow["NLEAVE"] = Constants.CON_NLEAVE_FDAY;
                                        oDataRow["STATUS"] = "";

                                        leaveBucket_.Rows.Add(oDataRow);
                                    }

                                    drTime = null;
                                }
                            }                     

                    }

                }
                else if ((mToDate == mFromDate) && (chkhalfDay.Checked == true))
                {
                    Utility.Errorhandler.ClearError(lblMessage);

                    if ((ddlRoster.Items.Count > 0) && (ddlRoster.SelectedItem.Text.Trim() == String.Empty))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Select Roster for the half day leave", lblMessage);
                        return;
                    }

                    

                    // checking leaves of covering person for the roster/day

                    if ((ddlRoster.Items.Count >0) && (ddlRoster.SelectedItem.Text.Trim() != String.Empty)) 
                    {
                        DataTable dtCoveringPerson = new DataTable();

                        dtCoveringPerson = leaveScheduleDataHandler.isLeaveExistForTheRoster(txtCoveredBy.Text.Trim(), txtFDate.Text.Trim(), ddlRoster.SelectedItem.Value.Trim());
                        
                        if (dtCoveringPerson.Rows.Count > 0)
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Covering person is leave on " + txtFDate.Text.Trim() + " for roster " + ddlRoster.SelectedItem.Text.Trim(), lblMessage);
                            return;
                        }
                    }                    
                    else if (leaveScheduleDataHandler.isOnLeave(txtCoveredBy.Text.Trim(), txtFDate.Text.Trim()) == true)
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Covering person is leave on " + txtFDate.Text.Trim(), lblMessage);
                        return;
                    }

                    // checking leaves of leave applicant for the roster/day
                    ////if (ddlRoster.SelectedItem.Text.Trim() != String.Empty)
                    ////{
                    ////    DataTable dtemployeeOnLeave = new DataTable();

                    ////    dtemployeeOnLeave = leaveScheduleDataHandler.isLeaveExistForTheRoster(txtEmploeeId.Text.Trim(), txtFDate.Text.Trim(), ddlRoster.SelectedItem.Value.Trim());

                    ////    if (dtemployeeOnLeave.Rows.Count > 0)
                    ////    {
                    ////        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You are leave on " + txtFDate.Text.Trim() + " for roster " + ddlRoster.SelectedItem.Text.Trim(), lblMessage);
                    ////        return;
                    ////    }
                    ////}
                    ////else if (leaveScheduleDataHandler.isOnLeave(txtEmploeeId.Text.Trim(), txtFDate.Text.Trim()) == true)
                    ////{
                    ////    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You are leave on " + txtFDate.Text.Trim(), lblMessage);
                    ////    return;
                    ////}

                    string fromTime = ddlFromHH.SelectedItem.Text.Trim() + ":" + ddlFromMM.Text.Trim();
                    string toTime = ddlToHH.SelectedItem.Text.Trim() + ":" + ddlToMM.Text.Trim();

                    TimeSpan tsFrom = new TimeSpan(Int32.Parse(ddlFromHH.SelectedItem.Text.Trim()), Int32.Parse(ddlFromMM.SelectedItem.Text.Trim()), 0);
                    TimeSpan tsTo = new TimeSpan(Int32.Parse(ddlToHH.SelectedItem.Text.Trim()), Int32.Parse(ddlToMM.SelectedItem.Text.Trim()), 0);
                        
                    // for roster change validation based on the roster type
                    string rType = "";
                    if ((ddlRoster.Items.Count > 0) && (ddlRoster.SelectedItem.Text.Trim() != String.Empty))
                    {
                        rType = leaveScheduleDataHandler.getRosterType(ddlRoster.SelectedItem.Value.Trim());
                    }

                    if ((rType.Trim().Equals(Constants.CON_REGULAR_ROSTER_TYPE)) && (tsFrom > tsTo))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "From time should less than To time", lblMessage);
                        return;
                    }
                    else if ((rType.Trim().Equals("")) && (tsFrom > tsTo))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "From time should less than To time", lblMessage);
                        return;
                    }
                            
                    //if (tsFrom > tsTo)
                    //{
                    //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "From time should less than To time", lblMessage);
                    //    return;
                    //}

                    DataTable dtRosterTime = leaveScheduleDataHandler.getRosterWorkingTime(txtEmploeeId.Text.Trim(), txtFDate.Text.Trim());

                    if ((dtRosterTime.Rows.Count > 0) && (ddlRoster.SelectedItem.Text.Trim() != String.Empty))
                    {
                        if (isLeaveExistForRoster(leaveBucket_, txtFDate.Text.Trim(), ddlRoster.SelectedItem.Value.Trim()) != true)
                        {
                            string sFromTo = ddlRoster.SelectedItem.Text.Trim();
                            string[] arrFromTo = sFromTo.Split('-');
                            string sArrFrom = arrFromTo[0].Trim();
                            string sArrTo = arrFromTo[1].Trim();
                            string[] fromT = sArrFrom.Split(':');
                            string[] toT = sArrTo.Split(':');

                            TimeSpan tsRStart = new TimeSpan(Int32.Parse(fromT[0].ToString()), Int32.Parse(fromT[1].ToString()), 0);
                            TimeSpan tsREnd = new TimeSpan(Int32.Parse(toT[0].ToString()), Int32.Parse(toT[1].ToString()), 0);
                                                       

                            if ((tsFrom < tsRStart) && (tsTo > tsRStart) && (tsTo < tsREnd))
                            {
                                if((rbM.Checked == true) && (tsFrom != tsRStart))
                                {
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should start at " + sArrFrom + " and within the roster", lblMessage);
                                    return;
                                }
                                else if ((rbE.Checked == true) && (tsREnd != tsTo))
                                {
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should end at " + sArrTo + " and within the roster", lblMessage);
                                    return;
                                }
                            }
                            else if ((tsFrom > tsRStart) && (tsFrom < tsTo) && (tsTo > tsREnd))
                            {
                                if ((rbM.Checked == true) && (tsFrom != tsRStart))
                                {
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should start at " + sArrFrom + " and within the roster", lblMessage);
                                    return;
                                }
                                else if ((rbE.Checked == true) && (tsREnd != tsTo))
                                {
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should end at " + sArrTo + " and within the roster", lblMessage);
                                    return;
                                }
                            }
                            else if ((tsFrom < tsRStart) && (tsREnd < tsTo))
                            {
                                if ((rbM.Checked == true) && (tsFrom != tsRStart))
                                {
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should start at " + sArrFrom + " and within the roster", lblMessage);
                                    return;
                                }
                                else if ((rbE.Checked == true) && (tsREnd != tsTo))
                                {
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should end at " + sArrTo + " and within the roster", lblMessage);
                                    return;
                                }
                            }
                            else if ((tsFrom != tsRStart) || (tsTo != tsREnd))
                            {
                                if ((rbM.Checked == true) && (tsFrom != tsRStart))
                                {
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should start at " + sArrFrom + " and within the roster", lblMessage);
                                    return;
                                }
                                else if ((rbE.Checked == true) && (tsREnd != tsTo))
                                {
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should end at " + sArrTo + " and within the roster", lblMessage);
                                    return;
                                }
                            }

                            DataRow oDataRow = leaveBucket_.NewRow();

                            DataTable dtemployeeOnLeave = new DataTable();

                            dtemployeeOnLeave = leaveScheduleDataHandler.isLeaveExistForTheRoster(txtEmploeeId.Text.Trim(), txtFDate.Text.Trim(), ddlRoster.SelectedItem.Value.Trim());

                            if (dtemployeeOnLeave.Rows.Count > 0)
                            {
                                // if there is a leave that is added and excluded

                                DataTable dtRWT = leaveScheduleDataHandler.getRosterWorkingTimeForRoster(txtEmploeeId.Text.Trim(), txtFDate.Text.Trim(), ddlRoster.SelectedItem.Value.ToString().Trim());
                                DataRow drWT = dtRWT.Rows[0];

                                DataRow drRow = dtemployeeOnLeave.Rows[0];

                                oDataRow["EMPLOYEE_ID"] = txtEmploeeId.Text.Trim();
                                oDataRow["LEAVE_DATE"] = txtFDate.Text.Trim();
                                oDataRow["LEAVE_TYPE"] = drRow["LEAVE_TYPE_ID"].ToString(); 
                                if (ddlRoster.SelectedItem.Text.Trim() != String.Empty)
                                {
                                    oDataRow["ROSTR_ID"] = ddlRoster.SelectedItem.Value.ToString().Trim();
                                }
                                else
                                {
                                    oDataRow["ROSTR_ID"] = "";
                                }

                                oDataRow["FROM_TIME"] = drRow["FROM_TIME"].ToString();
                                oDataRow["TO_TIME"] = drRow["TO_TIME"].ToString();
                                oDataRow["COVERED_BY"] = drRow["COVERED_BY"].ToString();
                                oDataRow["RECOMMEND_BY"] = drRow["RECOMMAND_BY"].ToString();
                                oDataRow["SCHEME_LINE_NO"] = lineNo.ToString();
                                oDataRow["REMARKS"] = "";
                                oDataRow["NO_OF_DAYS"] = double.Parse(drRow["NO_OF_DAYS"].ToString());
                                oDataRow["IS_HALFDAY"] = drRow["IS_HALFDAY"].ToString();
                                oDataRow["LEAVE_STATUS"] = drRow["LEAVE_STATUS"].ToString();
                                oDataRow["IS_DAY_OFF"] = Constants.LEAVE_IS_OFF_DAY_YES;                                

                                oDataRow["COM_ROS_TIME"] = drWT["FROM_TIME"].ToString().Trim() + '-' + drWT["TO_TIME"].ToString().Trim();


                                //////string[] sAFrom = drWT["FROM_TIME"].ToString().Trim().Split(':');
                                //////string sFrom = sAFrom[0].Trim() + sAFrom[1].Trim();
                                //////oDataRow["FROM"] = Convert.ToInt32(sFrom);
                                //////string[] sATo = drWT["TO_TIME"].ToString().Trim().Split(':');
                                //////string sTo = sATo[0].Trim() + sATo[1].Trim();
                                //////oDataRow["TO"] = Convert.ToInt32(sTo);

                                //////if (rbM.Checked == true)
                                //////{
                                //////    oDataRow["NLEAVE"] = Constants.CON_NLEAVE_MHALF;
                                //////}
                                //////else if (rbE.Checked == true)
                                //////{
                                //////    oDataRow["NLEAVE"] = Constants.CON_NLEAVE_EHALF;
                                //////}

                                oDataRow["STATUS"] = Constants.CON_ON_LEAVE;

                                leaveBucket_.Rows.Add(oDataRow);
                            }
                            else
                            {
                                // new leave record is added

                                oDataRow["EMPLOYEE_ID"] = txtEmploeeId.Text.Trim();
                                oDataRow["LEAVE_DATE"] = txtFDate.Text.Trim();
                                oDataRow["LEAVE_TYPE"] = "";
                                if (ddlRoster.SelectedItem.Text.Trim() != String.Empty)
                                {
                                    oDataRow["ROSTR_ID"] = ddlRoster.SelectedItem.Value.ToString().Trim();

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

                                oDataRow["COM_ROS_TIME"] = sArrFrom.Trim() + '-' + sArrTo.Trim();


                                //////string[] sAFrom = sArrFrom.Trim().Split(':');
                                //////string sFrom = sAFrom[0].Trim() + sAFrom[1].Trim();
                                //////oDataRow["FROM"] = Convert.ToInt32(sFrom);

                                //////string[] sATo = sArrTo.Trim().Split(':');
                                //////string sTo = sATo[0].Trim() + sATo[1].Trim();
                                //////oDataRow["TO"] = Convert.ToInt32(sTo);

                                //////if (rbM.Checked == true)
                                //////{
                                //////    oDataRow["NLEAVE"] = Constants.CON_NLEAVE_MHALF;
                                //////}
                                //////else if (rbE.Checked == true)
                                //////{
                                //////    oDataRow["NLEAVE"] = Constants.CON_NLEAVE_EHALF;
                                //////}

                                oDataRow["STATUS"] = "";

                                leaveBucket_.Rows.Add(oDataRow);
                            }

                            
                            
                        
                            // 2015/11/01 Anjana Uduwaragoda


                            ////////foreach (DataRow drRTime in dtRosterTime.Rows)
                            ////////{
                            ////////    DataRow oDataRow = leaveBucket_.NewRow();

                            ////////    oDataRow["EMPLOYEE_ID"] = txtEmploeeId.Text.Trim();
                            ////////    oDataRow["LEAVE_DATE"] = txtFDate.Text.Trim();
                            ////////    oDataRow["LEAVE_TYPE"] = "";

                            ////////    if (drRTime != null)
                            ////////    {
                            ////////        oDataRow["ROSTR_ID"] = drRTime["ROSTR_ID"].ToString();                                    

                            ////////    }
                            ////////    else
                            ////////    {
                            ////////        oDataRow["ROSTR_ID"] = "";
                            ////////    }

                            ////////    oDataRow["FROM_TIME"] = fromTime;
                            ////////    oDataRow["TO_TIME"] = toTime;
                            ////////    oDataRow["COVERED_BY"] = txtCoveredBy.Text;
                            ////////    oDataRow["RECOMMEND_BY"] = txtRecommendBy.Text;
                            ////////    oDataRow["SCHEME_LINE_NO"] = lineNo.ToString();
                            ////////    oDataRow["REMARKS"] = "";
                            ////////    oDataRow["NO_OF_DAYS"] = Constants.CON_HALF_DAY;
                            ////////    oDataRow["IS_HALFDAY"] = Constants.CON_HALF_DAY_FLAG;
                            ////////    oDataRow["LEAVE_STATUS"] = Constants.LEAVE_STATUS_ACTIVE_VALUE;
                            ////////    oDataRow["IS_DAY_OFF"] = Constants.LEAVE_IS_OFF_DAY_NO;

                            ////////    if (drRTime != null)
                            ////////    {

                            ////////        string[] fromT = drRTime["FROM_TIME"].ToString().Split(':');
                            ////////        string[] toT = drRTime["TO_TIME"].ToString().Split(':');

                            ////////        TimeSpan tsRStart = new TimeSpan(Int32.Parse(fromT[0].ToString()), Int32.Parse(fromT[1].ToString()), 0);
                            ////////        TimeSpan tsREnd = new TimeSpan(Int32.Parse(toT[0].ToString()), Int32.Parse(toT[1].ToString()), 0);

                            ////////        if ((tsFrom < tsRStart) && (tsTo > tsRStart) && (tsTo < tsREnd))
                            ////////        {                                        
                            ////////            oDataRow["LEAVE_TIME"] = Constants.CON_INVALID_TIME; 
                            ////////        }
                            ////////        else if ((tsFrom > tsRStart) && (tsFrom < tsTo) && (tsTo > tsREnd))
                            ////////        {
                            ////////            oDataRow["LEAVE_TIME"] = Constants.CON_INVALID_TIME;
                            ////////        }
                            ////////        else if ((tsFrom < tsRStart) && (tsREnd < tsTo))
                            ////////        {
                            ////////            oDataRow["LEAVE_TIME"] = Constants.CON_INVALID_TIME;
                            ////////        }
                            ////////        else if ((tsFrom != tsRStart) && (tsTo != tsREnd))
                            ////////        {
                            ////////            oDataRow["LEAVE_TIME"] = Constants.CON_INVALID_TIME;
                            ////////        }
                            ////////        else
                            ////////        {
                            ////////            oDataRow["LEAVE_TIME"] = Constants.CON_VALID_TIME;
                            ////////        }

                            ////////        oDataRow["COM_ROS_TIME"] = drRTime["FROM_TIME"].ToString().Trim() + '-' + drRTime["TO_TIME"].ToString().Trim();

                            ////////    }


                            ////////    leaveBucket_.Rows.Add(oDataRow);
                            ////////}

                           
                            dtRosterTime.Dispose();
                        }
                    }
                    else
                    {
                        DataRow drTime = leaveScheduleDataHandler.getCompanyWorkingTime(txtEmploeeId.Text.Trim());
                        
                        if (isLeaveExist(leaveBucket_, txtFDate.Text.Trim()) != true)
                        {

                            if (drTime != null)
                            {
                                string[] fromTim = drTime["FROM_TIME"].ToString().Split(':');
                                string[] toTim = drTime["TO_TIME"].ToString().Split(':');

                                TimeSpan tsCStart = new TimeSpan(Int32.Parse(fromTim[0].ToString()), Int32.Parse(fromTim[1].ToString()), 0);
                                TimeSpan tsCEnd = new TimeSpan(Int32.Parse(toTim[0].ToString()), Int32.Parse(toTim[1].ToString()), 0);

                                if ((tsFrom < tsCStart) && (tsTo > tsCStart) && (tsTo < tsCEnd))
                                {
                                    if ((rbM.Checked == true) && (tsFrom != tsCStart))
                                    {
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should start at " + drTime["FROM_TIME"].ToString() + " and within working time", lblMessage);
                                        return;
                                    }
                                    else if ((rbE.Checked == true) && (tsTo != tsCEnd))
                                    {
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should end at " + drTime["TO_TIME"].ToString() + " and within working time", lblMessage);
                                        return;
                                    }
                                }
                                else if ((tsFrom > tsCStart) && (tsFrom < tsTo) && (tsTo > tsCEnd))
                                {
                                    if ((rbM.Checked == true) && (tsFrom != tsCStart))
                                    {
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should start at " + drTime["FROM_TIME"].ToString() + " and within working time", lblMessage);
                                        return;
                                    }
                                    else if ((rbE.Checked == true) && (tsTo != tsCEnd))
                                    {
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should end at " + drTime["TO_TIME"].ToString() + " and within working time", lblMessage);
                                        return;
                                    }
                                }
                                else if ((tsFrom < tsCStart) && (tsCEnd < tsTo))
                                {
                                    if ((rbM.Checked == true) && (tsFrom != tsCStart))
                                    {
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should start at " + drTime["FROM_TIME"].ToString() + " and within working time", lblMessage);
                                        return;
                                    }
                                    else if ((rbE.Checked == true) && (tsTo != tsCEnd))
                                    {
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should end at " + drTime["TO_TIME"].ToString() + " and within working time", lblMessage);
                                        return;
                                    }
                                }
                                else if ((tsFrom != tsCStart) || (tsTo != tsCEnd))
                                {
                                    if ((rbM.Checked == true) && (tsFrom != tsCStart))
                                    {
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should start at " + drTime["FROM_TIME"].ToString() + " and within the roster", lblMessage);
                                        return;
                                    }
                                    else if ((rbE.Checked == true) && (tsTo != tsCEnd))
                                    {
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Half day should end at " + drTime["TO_TIME"].ToString() + " and within the roster", lblMessage);
                                        return;
                                    }
                                }

                                DataRow oDataRow = leaveBucket_.NewRow();

                                DataTable dtELeaves = new DataTable();
                                dtELeaves = leaveScheduleDataHandler.isLeaveExistForTheDay(txtEmploeeId.Text.Trim(), txtFDate.Text.Trim());

                                if (dtELeaves.Rows.Count > 0)
                                {
                                    // add details of existing leave

                                    DataRow drRow = dtELeaves.Rows[0];

                                    oDataRow["EMPLOYEE_ID"] = txtEmploeeId.Text.Trim();
                                    oDataRow["LEAVE_DATE"] = txtFDate.Text.Trim();
                                    oDataRow["LEAVE_TYPE"] = drRow["LEAVE_TYPE_ID"].ToString();

                                    oDataRow["ROSTR_ID"] = "";
                                    
                                    oDataRow["FROM_TIME"] = drRow["FROM_TIME"].ToString();
                                    oDataRow["TO_TIME"] = drRow["TO_TIME"].ToString();
                                    oDataRow["COVERED_BY"] = drRow["COVERED_BY"].ToString();
                                    oDataRow["RECOMMEND_BY"] = drRow["RECOMMAND_BY"].ToString();
                                    oDataRow["SCHEME_LINE_NO"] = lineNo.ToString();
                                    oDataRow["REMARKS"] = "";
                                    oDataRow["NO_OF_DAYS"] = double.Parse(drRow["NO_OF_DAYS"].ToString());
                                    oDataRow["IS_HALFDAY"] = drRow["IS_HALFDAY"].ToString();
                                    oDataRow["LEAVE_STATUS"] = drRow["LEAVE_STATUS"].ToString();
                                    oDataRow["IS_DAY_OFF"] = Constants.LEAVE_IS_OFF_DAY_YES;

                                    oDataRow["COM_ROS_TIME"] = drTime["FROM_TIME"].ToString().Trim() + '-' + drTime["TO_TIME"].ToString().Trim();
                                    oDataRow["STATUS"] = Constants.CON_ON_LEAVE;

                                    leaveBucket_.Rows.Add(oDataRow);

                                }
                                else
                                {
                                    // add new leave record

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

                                    oDataRow["COM_ROS_TIME"] = drTime["FROM_TIME"].ToString().Trim() + '-' + drTime["TO_TIME"].ToString().Trim();

                                    //////string[] sAFrom = drTime["FROM_TIME"].ToString().Trim().Split(':');
                                    //////string sFrom = sAFrom[0].Trim() + sAFrom[1].Trim();
                                    //////oDataRow["FROM"] = Convert.ToInt32(sFrom);

                                    //////string[] sATo = drTime["TO_TIME"].ToString().Trim().Split(':');
                                    //////string sTo = sATo[0].Trim() + sATo[1].Trim();
                                    //////oDataRow["TO"] = Convert.ToInt32(sTo);

                                    //////if (rbM.Checked == true)
                                    //////{
                                    //////    oDataRow["NLEAVE"] = Constants.CON_NLEAVE_MHALF;
                                    //////}
                                    //////else if (rbE.Checked == true)
                                    //////{
                                    //////    oDataRow["NLEAVE"] = Constants.CON_NLEAVE_EHALF;
                                    //////}

                                    oDataRow["STATUS"] = "";

                                    leaveBucket_.Rows.Add(oDataRow);  
                                }                                                                                                                             
                            }                          

                            drTime = null;
                        }
                    }
                   

                }
                else if ((mToDate == mFromDate) && (chkSL.Checked == true))
                {
                    if ((ddlRoster.Items.Count > 0) && (ddlRoster.SelectedItem.Text.Trim() == String.Empty))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Select Roster for the short leave", lblMessage);
                        return;
                    }


                    // checking leaves of covering person for the roster/day

                    if ((ddlRoster.Items.Count > 0) && (ddlRoster.SelectedItem.Text.Trim() != String.Empty))
                    {
                        DataTable dtCoveringPerson = new DataTable();

                        dtCoveringPerson = leaveScheduleDataHandler.isLeaveExistForTheRoster(txtCoveredBy.Text.Trim(), txtFDate.Text.Trim(), ddlRoster.SelectedItem.Value.Trim());

                        if (dtCoveringPerson.Rows.Count > 0)
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Covering person is leave on " + txtFDate.Text.Trim() + " for roster " + ddlRoster.SelectedItem.Text.Trim(), lblMessage);
                            return;
                        }
                    }
                    else if (leaveScheduleDataHandler.isOnLeave(txtCoveredBy.Text.Trim(), txtFDate.Text.Trim()) == true)
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Covering person is leave on " + txtFDate.Text.Trim(), lblMessage);
                        return;
                    }

                    string sShortLeaveDate = mFromDate.ToString("yyyyMM");

                    decimal sSLCount = leaveScheduleDataHandler.getShortLeaveCountForTheMonth(txtEmploeeId.Text.Trim(), sShortLeaveDate);

                    if (sSLCount >= 2)
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You have already taken your Short Leaves available for the month", lblMessage);
                        return;
                    }

                    if ((ddlRoster.Items.Count > 0) && (ddlRoster.SelectedItem.Text.Trim() == String.Empty))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Select Roster for the short leave", lblMessage);
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

                    TimeSpan timDif = tsTo.Subtract(tsFrom);

                    TimeSpan tsAllowed = new TimeSpan(1, 30, 0);

                    if (timDif > tsAllowed)
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Short Leave is only one and half hours", lblMessage);
                        return;
                    }

                    DataTable dtRosterTime = leaveScheduleDataHandler.getRosterWorkingTime(txtEmploeeId.Text.Trim(), txtFDate.Text.Trim());



                    if ((dtRosterTime.Rows.Count > 0) && (ddlRoster.SelectedItem.Text.Trim() != String.Empty))
                    {
                        if (isLeaveExistForRoster(leaveBucket_, txtFDate.Text.Trim(), ddlRoster.SelectedItem.Value.Trim()) != true)
                        {

                            string sFromTo = ddlRoster.SelectedItem.Text.Trim();
                            string[] arrFromTo = sFromTo.Split('-');
                            string sArrFrom = arrFromTo[0].Trim();
                            string sArrTo = arrFromTo[1].Trim();
                            string[] fromT = sArrFrom.Split(':');
                            string[] toT = sArrTo.Split(':');

                            TimeSpan tsRStart = new TimeSpan(Int32.Parse(fromT[0].ToString()), Int32.Parse(fromT[1].ToString()), 0);
                            TimeSpan tsREnd = new TimeSpan(Int32.Parse(toT[0].ToString()), Int32.Parse(toT[1].ToString()), 0);

                            if ((tsFrom < tsRStart) && (tsTo > tsRStart) && (tsTo < tsREnd))
                            {
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Short leave should be within " + sArrFrom + " and " + sArrTo, lblMessage);
                                return;
                            }
                            else if ((tsFrom > tsRStart) && (tsFrom < tsTo) && (tsTo > tsREnd))
                            {
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Short leave should be within " + sArrFrom + " and " + sArrTo, lblMessage);
                                return;
                            }
                            else if ((tsFrom < tsRStart) && (tsREnd < tsTo))
                            {
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Short leave should be within " + sArrFrom + " and " + sArrTo, lblMessage);
                                return;
                            }


                            DataRow oDataRow = leaveBucket_.NewRow();

                            DataTable dtemployeeOnLeave = new DataTable();

                            dtemployeeOnLeave = leaveScheduleDataHandler.isLeaveExistForTheRoster(txtEmploeeId.Text.Trim(), txtFDate.Text.Trim(), ddlRoster.SelectedItem.Value.Trim());

                            if (dtemployeeOnLeave.Rows.Count > 0)
                            {
                                // if there is a leave that is added and excluded

                                DataTable dtRWT = leaveScheduleDataHandler.getRosterWorkingTimeForRoster(txtEmploeeId.Text.Trim(), txtFDate.Text.Trim(), ddlRoster.SelectedItem.Value.ToString().Trim());
                                DataRow drWT = dtRWT.Rows[0];

                                DataRow drRow = dtemployeeOnLeave.Rows[0];

                                oDataRow["EMPLOYEE_ID"] = txtEmploeeId.Text.Trim();
                                oDataRow["LEAVE_DATE"] = txtFDate.Text.Trim();
                                oDataRow["LEAVE_TYPE"] = drRow["LEAVE_TYPE_ID"].ToString();
                                if (ddlRoster.SelectedItem.Text.Trim() != String.Empty)
                                {
                                    oDataRow["ROSTR_ID"] = ddlRoster.SelectedItem.Value.ToString().Trim();
                                }
                                else
                                {
                                    oDataRow["ROSTR_ID"] = "";
                                }

                                oDataRow["FROM_TIME"] = drRow["FROM_TIME"].ToString();
                                oDataRow["TO_TIME"] = drRow["TO_TIME"].ToString();
                                oDataRow["COVERED_BY"] = drRow["COVERED_BY"].ToString();
                                oDataRow["RECOMMEND_BY"] = drRow["RECOMMAND_BY"].ToString();
                                oDataRow["SCHEME_LINE_NO"] = lineNo.ToString();
                                oDataRow["REMARKS"] = "";
                                oDataRow["NO_OF_DAYS"] = double.Parse(drRow["NO_OF_DAYS"].ToString());
                                oDataRow["IS_HALFDAY"] = drRow["IS_HALFDAY"].ToString();
                                oDataRow["LEAVE_STATUS"] = drRow["LEAVE_STATUS"].ToString();
                                oDataRow["IS_DAY_OFF"] = Constants.LEAVE_IS_OFF_DAY_YES;

                                oDataRow["COM_ROS_TIME"] = drWT["FROM_TIME"].ToString().Trim() + '-' + drWT["TO_TIME"].ToString().Trim();
                                oDataRow["STATUS"] = Constants.CON_ON_LEAVE;

                                leaveBucket_.Rows.Add(oDataRow);
                            }
                            else
                            {
                                // new record is added

                                oDataRow["EMPLOYEE_ID"] = txtEmploeeId.Text.Trim();
                                oDataRow["LEAVE_DATE"] = txtFDate.Text.Trim();
                                oDataRow["LEAVE_TYPE"] = "";

                                if (ddlRoster.SelectedItem.Text.Trim() != String.Empty)
                                {
                                    oDataRow["ROSTR_ID"] = ddlRoster.SelectedItem.Value.ToString().Trim();

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

                                oDataRow["COM_ROS_TIME"] = sArrFrom.Trim() + '-' + sArrTo.Trim();

                                //////string[] sAFrom = sArrFrom.Trim().Split(':');
                                //////string sFrom = sAFrom[0].Trim() + sAFrom[1].Trim();
                                //////oDataRow["FROM"] = Convert.ToInt32(sFrom);

                                //////string[] sATo = sArrTo.Trim().Split(':');
                                //////string sTo = sATo[0].Trim() + sATo[1].Trim();
                                //////oDataRow["TO"] = Convert.ToInt32(sTo);

                                //////oDataRow["NLEAVE"] = Constants.CON_NLEAVE_SL;

                                oDataRow["STATUS"] = "";


                                leaveBucket_.Rows.Add(oDataRow);                                
                            }
                            
                            dtRosterTime.Dispose();
                        }
                    }
                    else
                    {
                            DataRow drTime = leaveScheduleDataHandler.getCompanyWorkingTime(txtEmploeeId.Text.Trim());

                            if (isLeaveExist(leaveBucket_, txtFDate.Text.Trim()) != true)
                            {

                                if (drTime != null)
                                {
                                    string[] fromTim = drTime["FROM_TIME"].ToString().Split(':');
                                    string[] toTim = drTime["TO_TIME"].ToString().Split(':');

                                    TimeSpan tsCStart = new TimeSpan(Int32.Parse(fromTim[0].ToString()), Int32.Parse(fromTim[1].ToString()), 0);
                                    TimeSpan tsCEnd = new TimeSpan(Int32.Parse(toTim[0].ToString()), Int32.Parse(toTim[1].ToString()), 0);

                                    if ((tsFrom < tsCStart) && (tsTo > tsCStart) && (tsTo < tsCEnd))
                                    {
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Short leave should be within " + drTime["FROM_TIME"].ToString() + " and " + drTime["TO_TIME"].ToString(), lblMessage);
                                        return;
                                    }
                                    else if ((tsFrom > tsCStart) && (tsFrom < tsTo) && (tsTo > tsCEnd))
                                    {
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Short leave should be within " + drTime["FROM_TIME"].ToString() + " and " + drTime["TO_TIME"].ToString(), lblMessage);
                                        return;
                                    }
                                    else if ((tsFrom < tsCStart) && (tsCEnd < tsTo))
                                    {
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Short leave should be within " + drTime["FROM_TIME"].ToString() + " and " + drTime["TO_TIME"].ToString(), lblMessage);
                                        return;
                                    }

                                    DataRow oDataRow = leaveBucket_.NewRow();

                                    DataTable dtELeaves = new DataTable();
                                    dtELeaves = leaveScheduleDataHandler.isLeaveExistForTheDay(txtEmploeeId.Text.Trim(), txtFDate.Text.Trim());

                                    if (dtELeaves.Rows.Count > 0)
                                    {
                                        // add existing leave record
                                        DataRow drRow = dtELeaves.Rows[0];

                                        oDataRow["EMPLOYEE_ID"] = txtEmploeeId.Text.Trim();
                                        oDataRow["LEAVE_DATE"] = txtFDate.Text.Trim();
                                        oDataRow["LEAVE_TYPE"] = drRow["LEAVE_TYPE_ID"].ToString();

                                        oDataRow["ROSTR_ID"] = "";

                                        oDataRow["FROM_TIME"] = drRow["FROM_TIME"].ToString();
                                        oDataRow["TO_TIME"] = drRow["TO_TIME"].ToString();
                                        oDataRow["COVERED_BY"] = drRow["COVERED_BY"].ToString();
                                        oDataRow["RECOMMEND_BY"] = drRow["RECOMMAND_BY"].ToString();
                                        oDataRow["SCHEME_LINE_NO"] = lineNo.ToString();
                                        oDataRow["REMARKS"] = "";
                                        oDataRow["NO_OF_DAYS"] = double.Parse(drRow["NO_OF_DAYS"].ToString());
                                        oDataRow["IS_HALFDAY"] = drRow["IS_HALFDAY"].ToString();
                                        oDataRow["LEAVE_STATUS"] = drRow["LEAVE_STATUS"].ToString();
                                        oDataRow["IS_DAY_OFF"] = Constants.LEAVE_IS_OFF_DAY_YES;

                                        oDataRow["COM_ROS_TIME"] = drTime["FROM_TIME"].ToString().Trim() + '-' + drTime["TO_TIME"].ToString().Trim();

                                        //////string[] sAFrom = drTime["FROM_TIME"].ToString().Trim().Split(':');
                                        //////string sFrom = sAFrom[0].Trim() + sAFrom[1].Trim();
                                        //////oDataRow["FROM"] = Convert.ToInt32(sFrom);

                                        //////string[] sATo = drTime["TO_TIME"].ToString().Trim().Split(':');
                                        //////string sTo = sATo[0].Trim() + sATo[1].Trim();
                                        //////oDataRow["TO"] = Convert.ToInt32(sTo);

                                        //////oDataRow["NLEAVE"] = Constants.CON_NLEAVE_SL;
                                        
                                        oDataRow["STATUS"] = Constants.CON_ON_LEAVE;

                                        leaveBucket_.Rows.Add(oDataRow);

                                    }
                                    else
                                    {

                                       // add new leave record
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

                                        oDataRow["COM_ROS_TIME"] = drTime["FROM_TIME"].ToString().Trim() + '-' + drTime["TO_TIME"].ToString().Trim();

                                        //////string[] sAFrom = drTime["FROM_TIME"].ToString().Trim().Split(':');
                                        //////string sFrom = sAFrom[0].Trim() + sAFrom[1].Trim();
                                        //////oDataRow["FROM"] = Convert.ToInt32(sFrom);

                                        //////string[] sATo = drTime["TO_TIME"].ToString().Trim().Split(':');
                                        //////string sTo = sATo[0].Trim() + sATo[1].Trim();
                                        //////oDataRow["TO"] = Convert.ToInt32(sTo);

                                        //////oDataRow["NLEAVE"] = Constants.CON_NLEAVE_SL;

                                        oDataRow["STATUS"] = "";

                                        leaveBucket_.Rows.Add(oDataRow);
                                    }
                                }

                                drTime = null;
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

                ddlRoster.Items.Clear();
                lblSelectRoster.Visible = false;
                ddlRoster.Visible = false;

                lblWorkingTime.Visible = false;
                lblCompanyTime.Visible = false;

                chkhalfDay.Checked = false;
                chkSL.Checked = false;
                rbE.Checked = false;
                rbM.Checked = false;

               ddlFromHH.SelectedIndex = 0;
               ddlFromMM.SelectedIndex = 0;
               ddlToHH.SelectedIndex = 0;
               ddlToMM.SelectedIndex = 0;

            }
            catch(Exception ex)
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
            log.Debug("webFrmEmployeeLeaveSheet : createLeaveBucket()");

            DataTable leaveBucket = new DataTable();

            leaveBucket.Columns.Add("EMPLOYEE_ID", typeof(string));
            leaveBucket.Columns.Add("LEAVE_DATE", typeof(string));
            leaveBucket.Columns.Add("LEAVE_TYPE", typeof(string));
            leaveBucket.Columns.Add("ROSTR_ID", typeof(string));            
            leaveBucket.Columns.Add("FROM_TIME", typeof(string));
            leaveBucket.Columns.Add("TO_TIME",typeof(string));
            leaveBucket.Columns.Add("COVERED_BY", typeof(string));
            leaveBucket.Columns.Add("RECOMMEND_BY", typeof(string));
            leaveBucket.Columns.Add("SCHEME_LINE_NO", typeof(string));
            leaveBucket.Columns.Add("REMARKS", typeof(string));
            leaveBucket.Columns.Add("NO_OF_DAYS", typeof(double));
            leaveBucket.Columns.Add("IS_HALFDAY", typeof(string));
            leaveBucket.Columns.Add("LEAVE_STATUS", typeof(string));
            leaveBucket.Columns.Add("IS_DAY_OFF", typeof(string));            
            leaveBucket.Columns.Add("COM_ROS_TIME", typeof(string));
            //////leaveBucket.Columns.Add("FROM", typeof(Int32));
            //////leaveBucket.Columns.Add("TO", typeof(Int32));
            //////leaveBucket.Columns.Add("NLEAVE", typeof(string));
            leaveBucket.Columns.Add("STATUS", typeof(string));

            // rosterTemp.PrimaryKey = new[] { rosterTemp.Columns["ROSTR_ID"], rosterTemp.Columns["ROSTR_TIME"], rosterTemp.Columns["DUTY_DATE"] };

            Session["leaveBucket"] = leaveBucket;
        }

        //private Boolean isDateNotOrdered(DataTable leaveTable, string leaveDate)
        //{
        //    Boolean isNotOrdered = false;

        //    if (leaveTable.Rows.Count > 0)
        //    {

        //        DataRow[] leaves = leaveTable.Select("LEAVE_DATE ='" + leaveDate.Trim() + "'");

        //        if (leaves.Length > 0)
        //        {
        //            leaveExist = true;
        //        }
        //    }
        //    return leaveExist;
        //}

        //private Boolean is_InvalidLeaveTimeExist(DataTable leaveTable)
        //{
        //    //log.Debug("webFrmEmployeeLeaveSheet : is_InvalidLeaveTimeExist()");
            
        //    Boolean invalidExist = false;

        //    if (leaveTable.Rows.Count > 0)
        //    {

        //        DataRow[] leaves = leaveTable.Select("IS_DAY_OFF ='" + Constants.LEAVE_IS_OFF_DAY_NO.Trim() + "' AND LEAVE_TIME ='" + Constants.CON_INVALID_TIME.Trim() + "'");

        //        if (leaves.Length > 0)
        //        {
        //            invalidExist = true;
        //        }
        //    }

        //    return invalidExist;
            

        //}

        
        // this method is used to cheeck the existance of a fullday leave in leave bucket 

        private Boolean isLeaveExistForRoster(DataTable leaveTable, string leaveDate,String sRoster)
        {
            //log.Debug("webFrmEmployeeLeaveSheet : isLeaveExist()");

            Boolean leaveExist = false;

            if (leaveTable.Rows.Count > 0)
            {

                DataRow[] leaves = leaveTable.Select("LEAVE_DATE ='" + leaveDate.Trim() + "' and ROSTR_ID='" + sRoster.Trim() + "'");

                if (leaves.Length > 0)
                {
                    leaveExist = true;
                }
            }
            return leaveExist;
        }

        private Boolean isLeaveExist(DataTable leaveTable, string leaveDate)
        {
            log.Debug("webFrmEmployeeLeaveSheet : isLeaveExist()");

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
            log.Debug("webFrmEmployeeLeaveSheet : isLeaveTypeNotSelected()");

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
            btnSave.Enabled = false;

            log.Debug("webFrmEmployeeLeaveSheet : btnSave_Click()");

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
            string lslNo  = String.Empty;

            try
            {
                Utility.Errorhandler.ClearError(lblMessage);

                if(Session["KeyUSER_ID"] != null)
                {
                    userId = Session["KeyUSER_ID"].ToString();
                }
                else
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Your session expired", lblMessage);
                    return;
                }

                if(Session["SchemeLine"] != null)
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

                ////////if (is_InvalidLeaveTimeExist(leaveBucket_) == true)
                ////////{
                ////////    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Exclude leaves with Invalid Leave Time", lblMessage);
                ////////    return;
                ////////}

                string sFromDate    = "";
                string sToDate      = "";

                if (leaveBucket_.Rows.Count > 0)
                {
                    sFromDate = gvLeaveSheet.Rows[0].Cells[1].Text.Trim();
                    sToDate = gvLeaveSheet.Rows[gvLeaveSheet.Rows.Count-1].Cells[1].Text.Trim();

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

                decimal anualLeaves     = leaveConstrains.availableAnnualLeaves(txtEmploeeId.Text.Trim(), iYear);
                decimal casualLeaves    = leaveConstrains.availableCasualLeaves(txtEmploeeId.Text.Trim(), iYear);

                string sAnualAplied = "";
                string sCasualApplied = "";

                double anualAplied = 0;
                double casualApplied = 0;

                sAnualAplied    = leaveBucket_.Compute("Sum(NO_OF_DAYS)", "LEAVE_TYPE = 'ANNUAL' and IS_DAY_OFF = 0").ToString();
                sCasualApplied  = leaveBucket_.Compute("Sum(NO_OF_DAYS)", "LEAVE_TYPE = 'CASUAL' and IS_DAY_OFF = 0").ToString();

                if (sAnualAplied.Trim() != "") { anualAplied = Double.Parse(sAnualAplied.Trim()); }
                if(sCasualApplied.Trim() != ""){casualApplied = Double.Parse(sCasualApplied.Trim());}

                string sTotalLeaves = "";
                double totalLeaves = 0;

                sTotalLeaves = leaveBucket_.Compute("Sum(NO_OF_DAYS)", "IS_DAY_OFF = 0").ToString();

                if(sTotalLeaves.Trim() != ""){totalLeaves = Double.Parse(sTotalLeaves.Trim());}


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

                double casualLeaveTaken =  0;
                double annualLeaveTaken = 0;
                
                if(sCasualLeaveTaken.Trim() != ""){casualLeaveTaken = Double.Parse(sCasualLeaveTaken.Trim());}
                if(sAnnualLeaveTaken.Trim() != ""){annualLeaveTaken = Double.Parse(sAnnualLeaveTaken.Trim());}

                string sShortLeavesApplied = "";
                double shortLeavesApplied = 0;                 

                sShortLeavesApplied = leaveBucket_.Compute("count(NO_OF_DAYS)", "LEAVE_TYPE = 'SL' and IS_DAY_OFF = 0").ToString();
                if(sShortLeavesApplied.Trim() != ""){shortLeavesApplied = Double.Parse(sShortLeavesApplied.Trim());}
               
                double shortLeaveTakent = leaveScheduleDataHandler.getMonthlyShortLeavesTaken(txtEmploeeId.Text.Trim(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());

                if ((shortLeavesApplied > 0) && (shortLeavesApplied + shortLeaveTakent > Constants.MONTHLY_SHORT_LEAVE_LIMIT))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You have only " + (Constants.MONTHLY_SHORT_LEAVE_LIMIT).ToString() + " short leaves", lblMessage);
                    return;
                }

                //else if ((shortLeavesApplied + shortLeaveTakent) == Constants.MONTHLY_SHORT_LEAVE_LIMIT)
                //{
                //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You have no more short leaves", lblMessage);
                //    return;
                //}

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
                String leaveSheetId = "";
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {                                                            
                    log.Debug("webFrmEmployeeLeaveSheet : btnSave_Click() -> Insert");

                    leaveSheetId = "";
                    leaveSheetId = leaveScheduleDataHandler.Insert(leaveBucket_, userId, txtEmploeeId.Text.Trim(), lslNo, minDate.Trim(), maxDate.Trim(),
                                         txtCoveredBy.Text.Trim(), totalLeaves, txtRecommendBy.Text.Trim(), Constants.LEAVE_STATUS_ACTIVE_VALUE, txtReason.Text.Trim());
                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

                    if (leaveSheetId != "")
                    {
                        hfLeaveSheetId.Value = "";
                        hfLeaveSheetId.Value = leaveSheetId;
                        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('Employee is saved ..')", true); 
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Leave sheet is saved", lblMessage);
                    }
                    else
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Leave sheet is exist for the same date range", lblMessage);
                    }
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    log.Debug("webFrmEmployeeLeaveSheet : btnSave_Click() -> Update");

                    if (hfLeaveSheetId.Value == "")
                    {
                        Utility.Errorhandler.GetError("2", "Leave sheet can not be updated", lblMessage);
                    }

                    Boolean isUpdated = leaveScheduleDataHandler.update(leaveBucket_, userId, txtEmploeeId.Text.Trim(), lslNo, minDate.Trim(), maxDate.Trim(),
                                         txtCoveredBy.Text.Trim(), totalLeaves, txtRecommendBy.Text.Trim(), Constants.LEAVE_STATUS_ACTIVE_VALUE, txtReason.Text.Trim(),hfLeaveSheetId.Value.ToString());
                    if (isUpdated)
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Leave sheet is updated", lblMessage);
                    }

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                }
                
                fillHistory();
                //if ((string)Session["KeyHRIS_ROLE"] != Constants.CON_COMMON_KeyHRIS_ROLE)
                //{
                //    fillHistory();
                //}

                //////string sMail = employeeDataHandler.getEmployeeEmail(txtRecommendBy.Text.Trim());
                

                //////if (sMail.Trim() != "")
                //////{

                //////    EmailHandler.SendDefaultEmail("Leave System", sMail, "", "Leave recommandation", getMailBody(empName, minDate.Trim(), maxDate.Trim(), sTotalLeaves, Constants.CON_LEAVE_RECOMMEND));
                //////}

                string empName = employeeDataHandler.getEmployeeName(txtEmploeeId.Text.Trim());
                string coveredMail = employeeDataHandler.getEmployeeEmail(txtCoveredBy.Text.Trim());

                if ((coveredMail.Trim() != "") && (leaveSheetId != ""))
                {
                    //EmailHandler.SendDefaultEmail("Leave System", coveredMail, "", "Duty Covering", getMailBody(empName, txtFDate.Text.Trim(), txtTDate.Text.Trim(), sTotalLeaves, Constants.CON_LEAVE_COVER));
                    EmailHandler.SendDefaultEmailHtml("Leave System", coveredMail, "", "Duty Covering", getMailBodyHtml(empName, minDate.Trim(), maxDate.Trim(), sTotalLeaves, Constants.CON_LEAVE_COVER, hfLeaveSheetId.Value.ToString()));
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
                ddlRoster.Items.Clear();
                lblSelectRoster.Visible = false;
                ddlRoster.Visible = false;

                lblWorkingTime.Visible = false;
                lblCompanyTime.Visible = false;

                chkhalfDay.Checked = false;
                chkSL.Checked = false;
                rbE.Checked = false;
                rbM.Checked = false;

                ddlFromHH.SelectedIndex = 0;
                ddlFromMM.SelectedIndex = 0;
                ddlToHH.SelectedIndex = 0;
                ddlToMM.SelectedIndex = 0;

                employeeLeaveSchemeDataHandler = null;
                leaveScheduleDataHandler = null;
                leaveConstrains = null;
                btnSave.Enabled = true;
            }
        }

        //private StringBuilder getMailBody(string employeeName, string sFromDate, string sToDate,string noDays,char covRec)
        //{
        //    log.Debug("webFrmEmployeeLeaveSheet : getMailBody()");

        //    StringBuilder stringBuilder = new StringBuilder();

        //    stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);
        //    if (sFromDate.Trim() == sToDate.Trim())
        //    {
        //        stringBuilder.Append(employeeName + " has applyed a leave on " + sFromDate + "." + Environment.NewLine);
        //    }
        //    else
        //    {
        //        stringBuilder.Append(employeeName + " has applyed leaves from " + sFromDate + " to " + sToDate + "." + Environment.NewLine);
        //    }

        //    if (covRec == Constants.CON_LEAVE_COVER)
        //    {
        //        stringBuilder.Append("Please cover duties." + Environment.NewLine + Environment.NewLine);
        //    }
        //    else if (covRec == Constants.CON_LEAVE_RECOMMEND)
        //    {
        //        stringBuilder.Append("Please recommand it." + Environment.NewLine + Environment.NewLine);
        //    }
        //    stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
        //    stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

        //    return stringBuilder;
        //}

        //private StringBuilder getMailBodyHalfDay(string employeeName, string sFromDate, string sFromTime, string sToTime)
        //{
        //    log.Debug("webFrmEmployeeLeaveSheet : getMailBodyHalfDay()");

        //    StringBuilder stringBuilder = new StringBuilder();

        //    stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);

        //    stringBuilder.Append(employeeName + " has applyed a halfday leave from " + sFromTime + " to " + sToTime + " on " + sFromDate + "." + Environment.NewLine);
            

        //    stringBuilder.Append("Please recommand/approve it." + Environment.NewLine + Environment.NewLine);
        //    stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
        //    stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

        //    return stringBuilder;
        //}

        //private StringBuilder getMailBodyShortLeave(string employeeName, string sFromDate, string sFromTime, string sToTime)
        //{
        //    log.Debug("webFrmEmployeeLeaveSheet : getMailBodyShortLeave()");

        //    StringBuilder stringBuilder = new StringBuilder();

        //    stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);

        //    stringBuilder.Append(employeeName + " has applyed a short leave from " + sFromTime + " to " + sToTime + " on " + sFromDate + "." + Environment.NewLine);
            
        //    stringBuilder.Append("Please recommand/approve it." + Environment.NewLine + Environment.NewLine);
        //    stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
        //    stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

        //    return stringBuilder;
        //}

        ////private StringBuilder getMailBodyForCoverring(string employeeName, string sFromDate, string sToDate, string noDays)
        ////{
        ////    log.Debug("webFrmEmployeeLeaveSheet : getMailBodyForCoverring()");

        ////    StringBuilder stringBuilder = new StringBuilder();

        ////    stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);
        ////    if (sFromDate.Trim() == sToDate.Trim())
        ////    {
        ////        stringBuilder.Append("You are covering the duties of " + employeeName + " on " + sFromDate + "." + Environment.NewLine);
        ////    }
        ////    else
        ////    {
        ////        stringBuilder.Append("You are covering the duties of " + employeeName + " from " + sFromDate + " to " + sToDate + "." + Environment.NewLine);
        ////    }
            
        ////    stringBuilder.Append(Environment.NewLine + Environment.NewLine);
        ////    stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
        ////    stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

        ////    return stringBuilder;
        ////}
        
        protected void fillHistory()
        {
            log.Debug("webFrmEmployeeLeaveSheet : fillHistory()");

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
            log.Debug("webFrmEmployeeLeaveSheet : chkIS_DAY_OFF_OnCheckedChanged()");

            int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
            CheckBox cb = (CheckBox)gvLeaveSheet.Rows[selRowIndex].FindControl("chkIS_DAY_OFF");

            DataTable leaveBucket_ = new DataTable();


            if (cb.Checked==true)
            {
                if(Session["leaveBucket"] != null)
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
            log.Debug("webFrmEmployeeLeaveSheet : chkDiscard_OnCheckedChanged()");

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
                    if ((string)Session["KeyHRIS_ROLE"] != Constants.CON_COMMON_KeyHRIS_ROLE)
                    {
                        string leaveSheetId = gvLeaveHistory.Rows[selRowIndex].Cells[0].Text.Trim();
                        Boolean isDiscarded = leaveScheduleDataHandler.updateStatus(leaveSheetId, Constants.LEAVE_STATUS_DISCARDED, userId);

                        if (isDiscarded)
                        {
                            Utility.Errorhandler.GetError("2", "Leave sheet is discarded", lblMessage);
                        }

                        fillHistory();
                    }
                    else
                    {
                        cb.Checked = false;
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

        protected void ddlLeaveType_Update(object sender, EventArgs e)
        {
            log.Debug("webFrmEmployeeLeaveSheet : ddlLeaveType_Update()");
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
                        else if ((leaveTypeId != Constants.CON_SHORT_LEAVE_LEAVE_ID) && (dNoDays < 0.5))
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
            log.Debug("webFrmEmployeeLeaveSheet : gvLeaveSheet_RowDataBound()");

            EmployeeLeaveSchemeDataHandler employeeLeaveSchemeDataHandler = new EmployeeLeaveSchemeDataHandler();

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkDayOff = (CheckBox)e.Row.FindControl("chkIS_DAY_OFF");
                    DropDownList dropDownList = (DropDownList)e.Row.FindControl("ddlLeaveType");

                    if (e.Row.Cells[15].Text.Trim() == Constants.CON_ON_LEAVE)
                    {
                        chkDayOff.Enabled = false;
                        e.Row.BackColor = Color.LightSkyBlue;
                        dropDownList.Enabled = false;
                    }

                    DataTable leaveTypes = new DataTable();

                    leaveTypes = employeeLeaveSchemeDataHandler.getLeaveTypesOfActiveLeaveScheme(txtEmploeeId.Text.Trim()).Copy();

                    //Find the DropDownList in the Row
                    DropDownList ddlLeaveTypes = (e.Row.FindControl("ddlLeaveType") as DropDownList);
                    ddlLeaveTypes.DataSource = leaveTypes;
                    ddlLeaveTypes.DataTextField = "LEAVE_TYPE_NAME";
                    ddlLeaveTypes.DataValueField = "LEAVE_TYPE_ID";
                    ddlLeaveTypes.DataBind();

                    //Add Default Item in the DropDownList
                    ddlLeaveTypes.Items.Insert(0, new ListItem("Please select"));

                    if(Session["leaveBucket"] != null)
                    {
                        DataTable dtTemp = (DataTable)Session["leaveBucket"];
                        int iIndex = e.Row.RowIndex;

                        if (dtTemp.Rows[iIndex][2].ToString().Trim() != "")
                        {
                            ddlLeaveTypes.SelectedValue = dtTemp.Rows[iIndex][2].ToString().Trim();
                        }
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

        protected void gvLeaveHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("webFrmEmployeeLeaveSheet : gvLeaveHistory_RowDataBound()");
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
            log.Debug("webFrmEmployeeLeaveSheet : btnClear_Click()");
            clear();
        }

        protected void clear()
        {
            log.Debug("webFrmEmployeeLeaveSheet : clear()");

            try
            {
                Utility.Utils.clearControls(false, txtEmploeeId, txtCoveredBy, txtFDate, txtReason, txtRecommendBy, txtTDate, chkhalfDay, chkSL, lblEmployeeName, lblCoveredByName, lblRecomendedByName);

                gvLeaveSummary.DataSource = null;
                gvLeaveSummary.DataBind();

                gvLeaveSheet.DataSource = null;
                gvLeaveSheet.DataBind();

                gvLeaveHistory.DataSource = null;
                gvLeaveHistory.DataBind();

                gvLeaveSheetDetails.DataSource = null;
                gvLeaveSheetDetails.DataBind();

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

                ddlRoster.Items.Clear();
                lblSelectRoster.Visible = false;
                ddlRoster.Visible = false;

                lblWorkingTime.Visible = false;
                lblCompanyTime.Visible = false;

                chkhalfDay.Checked = false;
                chkSL.Checked = false;
                rbE.Checked = false;
                rbM.Checked = false;

                ddlFromHH.SelectedIndex = 0;
                ddlFromMM.SelectedIndex = 0;
                ddlToHH.SelectedIndex = 0;
                ddlToMM.SelectedIndex = 0;

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
            log.Debug("webFrmEmployeeLeaveSheet : clearSessionTables()");
 
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
            log.Debug("webFrmEmployeeLeaveSheet : gvLeaveHistory_SelectedIndexChanged()");

            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

            try
            {      
                string empLeaveSchemeId = gvLeaveHistory.SelectedRow.Cells[0].Text.ToString().Trim();

                if(empLeaveSchemeId.Trim() != String.Empty)
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
        
        //protected void chkSL_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkSL.Checked == true)
        //    {
        //        chkhalfDay.Checked = false;
        //    }
        //}

        //protected void chkhalfDay_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkhalfDay.Checked == true)
        //    {
        //        chkSL.Checked = false;
        //    }
        //}
        
        private StringBuilder getMailBodyHtml(string employeeName, string sFromDate, string sToDate, string noDays, char covRec,string lsId)
        {
            log.Debug("webFrmEmployeeLeaveSheet : getMailBodyHtml()");

            PasswordHandler crpto = new PasswordHandler();
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine + "</br></br>");
            if (sFromDate.Trim() == sToDate.Trim())
            {
                if (Double.Parse(noDays).Equals(Constants.CON_SL))
                {
                    stringBuilder.Append(employeeName + " has applyed a short leave on " + sFromDate + "." + Environment.NewLine + "</br>");
                }
                else if (Double.Parse(noDays).Equals(Constants.CON_HALF_DAY))
                {
                    stringBuilder.Append(employeeName + " has applyed a half day leave on " + sFromDate + "." + Environment.NewLine + "</br>");
                }
                else
                {
                    stringBuilder.Append(employeeName + " has applyed a leave on " + sFromDate + "." + Environment.NewLine + "</br>");
                }
            }
            else
            {
                stringBuilder.Append(employeeName + " has applyed " + noDays.ToString() + " leave from " + sFromDate + " to " + sToDate + "." + Environment.NewLine + "</br>");
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

        protected void rbM_CheckedChanged(object sender, EventArgs e)
        {
            if ((chkhalfDay.Checked==true) && (rbM.Checked == true))
            {
                rbE.Checked = false;
            }
            else if ((chkhalfDay.Checked == true) && (rbM.Checked == false))
            {
                rbE.Checked = true;
            }
            else
            {
                rbE.Checked = false;
                rbM.Checked = false;
            }
        }

        protected void rbE_CheckedChanged(object sender, EventArgs e)
        {
            if ((chkhalfDay.Checked==true) && (rbE.Checked == true))
            {
                rbM.Checked = false;
            }
            else if ((chkhalfDay.Checked==true) && (rbE.Checked == false))
            {
                rbM.Checked = true;
            }
            else
            {
                rbE.Checked = false;
                rbM.Checked = false;
            }
        }

        protected void chkhalfDay_CheckedChanged(object sender, EventArgs e)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

            if (chkhalfDay.Checked == true)
            {
                try
                {
                    ddlRoster.Items.Clear();
                    lblSelectRoster.Visible = false;
                    ddlRoster.Visible = false;

                    lblWorkingTime.Visible = false;
                    lblCompanyTime.Visible = false;

                    chkSL.Checked = false;

                    DataTable dtRosterTime = leaveScheduleDataHandler.getRosterWorkingTime(txtEmploeeId.Text.Trim(), txtFDate.Text.Trim());
                    ddlRoster.Items.Clear();


                    if (dtRosterTime.Rows.Count > 0)
                    {
                        lblSelectRoster.Visible = true;
                        ddlRoster.Visible = true;

                        ListItem Item = new ListItem();
                        Item.Text = "";
                        Item.Value = "";
                        ddlRoster.Items.Add(Item);

                        foreach (DataRow dataRow in dtRosterTime.Rows)
                        {
                            ListItem listItem = new ListItem();
                            listItem.Text = dataRow["FROM_TIME"].ToString() + "-" + dataRow["TO_TIME"].ToString();
                            listItem.Value = dataRow["ROSTR_ID"].ToString();

                            ddlRoster.Items.Add(listItem);
                        }
                    }
                    else
                    {
                        DataRow drTime = leaveScheduleDataHandler.getCompanyWorkingTime(txtEmploeeId.Text.Trim());

                        if (drTime != null)
                        {
                            lblWorkingTime.Visible = true;
                            lblCompanyTime.Visible = true;

                            string sTime = drTime["FROM_TIME"].ToString() + "-" + drTime["TO_TIME"].ToString();
                            lblCompanyTime.Text = sTime;
                        }

                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    leaveScheduleDataHandler = null;
                }

            }
            else if (chkhalfDay.Checked == false)
            {
                ddlRoster.Items.Clear();
                lblSelectRoster.Visible = false;
                ddlRoster.Visible = false;

                lblWorkingTime.Visible = false;
                lblCompanyTime.Visible = false;

            }

        }

        protected void chkSL_CheckedChanged(object sender, EventArgs e)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

            if (chkSL.Checked == true)
            {
                
                ddlRoster.Items.Clear();
                lblSelectRoster.Visible = false;
                ddlRoster.Visible = false;

                lblWorkingTime.Visible = false;
                lblCompanyTime.Visible = false;


                chkhalfDay.Checked = false;
                rbM.Checked = false;
                rbE.Checked = false;

                try
                {
                    DataTable dtRosterTime = leaveScheduleDataHandler.getRosterWorkingTime(txtEmploeeId.Text.Trim(), txtFDate.Text.Trim());
                    ddlRoster.Items.Clear();

                    if (dtRosterTime.Rows.Count > 0)
                    {
                        lblSelectRoster.Visible = true;
                        ddlRoster.Visible = true;

                        ListItem Item = new ListItem();
                        Item.Text = "";
                        Item.Value = "";
                        ddlRoster.Items.Add(Item);

                        foreach (DataRow dataRow in dtRosterTime.Rows)
                        {
                            ListItem listItem = new ListItem();
                            listItem.Text = dataRow["FROM_TIME"].ToString() + "-" + dataRow["TO_TIME"].ToString();
                            listItem.Value = dataRow["ROSTR_ID"].ToString();

                            ddlRoster.Items.Add(listItem);
                        }
                    }
                    else
                    {
                        DataRow drTime = leaveScheduleDataHandler.getCompanyWorkingTime(txtEmploeeId.Text.Trim());

                        if (drTime != null)
                        {
                            lblWorkingTime.Visible = true;
                            lblCompanyTime.Visible = true;

                            string sTime = drTime["FROM_TIME"].ToString() + "-" + drTime["TO_TIME"].ToString();
                            lblCompanyTime.Text = sTime;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    leaveScheduleDataHandler = null;
                }

            }
            else if (chkSL.Checked == false)
            {
                ddlRoster.Items.Clear();
                lblSelectRoster.Visible = false;
                ddlRoster.Visible = false;

                lblWorkingTime.Visible = false;
                lblCompanyTime.Visible = false;

            }
        }

        private string LoadEmployeeName(string EmployeeID)
        {
            EmployeeLeaveHandler ELDH = new EmployeeLeaveHandler();
            string EmployeeName = String.Empty;
            try
            {
                log.Debug("webFrmEmployeeLeaveSheet : LoadEmployeeName()");

                EmployeeName = ELDH.getEmployeeName(EmployeeID);

                return EmployeeName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                EmployeeName = String.Empty;
                ELDH = null;
            }
        }
    }
}