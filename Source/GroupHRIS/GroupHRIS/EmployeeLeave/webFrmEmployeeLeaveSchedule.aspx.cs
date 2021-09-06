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
//using HrisMail;
using Common;
using NLog;

namespace GroupHRIS.EmployeeLeave
{
    public partial class webFrmEmployeeLeaveSchedule : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;


            log.Debug("IP:" + sIPAddress + "webFrmEmployeeLeaveSchedule Page_Load");

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
                createLeaveBucket();
                fillHours(ddlFromHH);
                fillMinutes(ddlFromMM);
                fillHours(ddlToHH);
                fillMinutes(ddlToMM);
            }
            else 
            {
                if ((hfEmpId.Value.Trim() == "") || (hfEmpId.Value.Trim() != txtEmploeeId.Text.Trim()))
                {
                    hfEmpId.Value = txtEmploeeId.Text.Trim();
                    
                    DataTable leaveBucket_ = (DataTable)Session["leaveBucket"];
                    leaveBucket_.Rows.Clear();
                    Session["leaveBucket"] = leaveBucket_; 

                    fillLeaveInformation();
                }
            }

            //if(hfEmpId.Value.ToString().Trim()=="")
        }

        protected void imgBtnView_Click(object sender, ImageClickEventArgs e)
        {
            log.Debug("webFrmEmployeeLeaveSchedule Page_Load");

            EmployeeLeaveSchemeDataHandler employeeLeaveSchemeDataHandler = new EmployeeLeaveSchemeDataHandler();

            try
            {
                fillLeaveSummary(txtEmploeeId.Text.Trim());
                fillLeaveTypes(txtEmploeeId.Text.Trim());
                fillLeaveHistory(txtEmploeeId.Text.Trim());
                int lineNo = employeeLeaveSchemeDataHandler.getActiveLeaveSchemeLine(txtEmploeeId.Text.Trim());
                Session["SchemeLine"] = lineNo;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                employeeLeaveSchemeDataHandler = null;
                Utility.Errorhandler.ClearError(lblLeaveMessage);
            }
        }

        private void fillLeaveInformation()
        {
            EmployeeLeaveSchemeDataHandler employeeLeaveSchemeDataHandler = new EmployeeLeaveSchemeDataHandler();

            try
            {
                fillLeaveSummary(txtEmploeeId.Text.Trim());
                fillLeaveTypes(txtEmploeeId.Text.Trim());
                fillLeaveHistory(txtEmploeeId.Text.Trim());
                int lineNo = employeeLeaveSchemeDataHandler.getActiveLeaveSchemeLine(txtEmploeeId.Text.Trim());
                Session["SchemeLine"] = lineNo;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                employeeLeaveSchemeDataHandler = null;
                Utility.Errorhandler.ClearError(lblLeaveMessage);
            }
        }


        protected void ddlLeaveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblLeaveMessage);

        }

        #region Private Methods
        //------------------------------------------------------------------------------        
        ///<summary>
        ///Load active leave scheme
        ///<param name="employeeId">Pass a employeeid string to query </param>    
        ///</summary>
        //----------------------------------------------------------------------------------------

        private void fillLeaveTypes(string employeeId)
        {
            EmployeeLeaveSchemeDataHandler employeeLeaveSchemeDataHandler = new EmployeeLeaveSchemeDataHandler();
            DataTable leaveTypes = new DataTable();

            try
            {
                leaveTypes = employeeLeaveSchemeDataHandler.getLeaveTypesOfActiveLeaveScheme(employeeId.Trim());

                ddlLeaveType.Items.Clear();

                if (leaveTypes.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlLeaveType.Items.Add(Item);

                    foreach (DataRow dataRow in leaveTypes.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["LEAVE_TYPE_NAME"].ToString();
                        listItem.Value = dataRow["LEAVE_TYPE_ID"].ToString();

                        ddlLeaveType.Items.Add(listItem);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                employeeLeaveSchemeDataHandler = null;
                leaveTypes.Dispose();
            }
        }

        //------------------------------------------------------------------------------        
        ///<summary>
        ///Load fill leave summary for a given employee for a given year
        ///<param name="employeeId">Pass a employeeid string to query </param>  
        ///</summary>
        //----------------------------------------------------------------------------------------

        private void fillLeaveSummary(string employeeId)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            DataTable leaveSummary = new DataTable();

            try
            {
                string sYear = DateTime.Today.Year.ToString();

                leaveSummary = leaveScheduleDataHandler.getEmployeeLeveSummary(employeeId.Trim(), sYear).Copy();

                gvLeaveSummary.DataSource = leaveSummary;
                gvLeaveSummary.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                leaveScheduleDataHandler = null;
                leaveSummary.Dispose();
            }
        }

        //------------------------------------------------------------------------------        
        ///<summary>
        ///Load fill leave history for a given employee for a given year
        ///<param name="employeeId">Pass a employeeid string to query </param>  
        ///</summary>
        //----------------------------------------------------------------------------------------

        private void fillLeaveHistory(string employeeId)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            DataTable leaveHistory = new DataTable();

            try
            {
                string sYear = DateTime.Today.Year.ToString();

                leaveHistory = leaveScheduleDataHandler.getEmployeeLeveHistory(employeeId.Trim(), sYear).Copy();

                gvLeaveHistory.DataSource = leaveHistory;
                gvLeaveHistory.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                leaveScheduleDataHandler = null;
                leaveHistory.Dispose();
            }
        }


        //------------------------------------------------------------------------------        
        ///<summary>
        ///check availability of leaves for a given employee for a given year
        ///<param name="employeeId">Pass a employeeid string to query </param>  
        ///</summary>
        //----------------------------------------------------------------------------------------

        private Boolean checkLeaveAvailability(string employeeId, string leaveYear, string leaveCategory, string leaveTypeId)
        {

            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            LeaveConstrains leaveConstrains = new LeaveConstrains();

            decimal availableLeaves = 0;
            Boolean isAvailable = true;

            try
            {

                decimal leaveTaken = 0;

                if (leaveCategory.Trim().Equals(Constants.CON_ANNUAL_LEAVE_CATEGORY))
                {
                    leaveTaken = leaveScheduleDataHandler.getLeavesTaken(employeeId, leaveYear, leaveTypeId);
                    availableLeaves = leaveConstrains.availableAnnualLeaves(employeeId);
                }
                else if (leaveCategory.Trim().Equals(Constants.CON_CASUAL_LEAVE_CATEGORY))
                {
                    leaveTaken = leaveScheduleDataHandler.getLeavesTaken(employeeId, leaveYear, leaveTypeId);
                    availableLeaves = leaveConstrains.availableCasualLeaves(employeeId);
                }
                if (leaveTaken >= availableLeaves)
                {
                    isAvailable = false;
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

            return isAvailable;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            LeaveTypeDataHandler leaveTypeDataHandler = new LeaveTypeDataHandler();
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            LeaveConstrains leaveConstrains = new LeaveConstrains();
            decimal availableLeaves = 0;
            decimal leaveTaken = 0;

            try
            {
                Utility.Errorhandler.ClearError(lblLeaveMessage);

                if (txtEmploeeId.Text.Trim() != "")
                {
                    if (ddlLeaveType.SelectedValue.Trim() != "")
                    {
                        string leaveYear = DateTime.Today.Year.ToString();
                        string LeaveCategory = leaveTypeDataHandler.getLeaveCategory(ddlLeaveType.SelectedValue.Trim());

                        if (LeaveCategory.Trim().Equals(Constants.CON_ANNUAL_LEAVE_CATEGORY))
                        {
                            leaveTaken = leaveScheduleDataHandler.getLeavesTaken(txtEmploeeId.Text.Trim(), leaveYear, ddlLeaveType.SelectedValue.Trim());
                            availableLeaves = leaveConstrains.availableAnnualLeaves(txtEmploeeId.Text.Trim());

                            if (leaveTaken >= availableLeaves)
                            {
                                Utility.Errorhandler.GetError("2", "You have no " + ddlLeaveType.SelectedItem.Text.Trim() + "S", lblLeaveMessage);
                                return;
                            }
                        }
                        else if (LeaveCategory.Trim().Equals(Constants.CON_CASUAL_LEAVE_CATEGORY))
                        {
                            leaveTaken = leaveScheduleDataHandler.getLeavesTaken(txtEmploeeId.Text.Trim(), leaveYear, ddlLeaveType.SelectedValue.Trim());
                            availableLeaves = leaveConstrains.availableCasualLeaves(txtEmploeeId.Text.Trim());

                            decimal serviceMonths = leaveScheduleDataHandler.getServiceMonthsofEmployee(txtEmploeeId.Text.Trim());
                            int joinedYear = leaveScheduleDataHandler.getJoindYearofEmployee(txtEmploeeId.Text.Trim());

                            if ((serviceMonths <= 12) && (leaveYear.Trim().Equals(joinedYear.ToString())))
                            {
                                if (leaveTaken >= availableLeaves)
                                {
                                    Utility.Errorhandler.GetError("2", "You have no " + ddlLeaveType.SelectedItem.Text.Trim() + "S", lblLeaveMessage);
                                    return;
                                }
                                else if ((leaveTaken < availableLeaves) && (Decimal.Parse(ddlNature.SelectedValue.Trim()) > (availableLeaves - leaveTaken)))
                                {
                                    Utility.Errorhandler.GetError("2", "You have only " + (availableLeaves - leaveTaken).ToString() + " " + ddlLeaveType.SelectedItem.Text.Trim() + "S", lblLeaveMessage);
                                    return;
                                }
                            }
                            else if ((serviceMonths <= 12) && (joinedYear < int.Parse(leaveYear)))
                            {
                                if (leaveTaken >= availableLeaves)
                                {
                                    Utility.Errorhandler.GetError("2", "You have no " + ddlLeaveType.SelectedItem.Text.Trim() + "S", lblLeaveMessage);
                                    return;
                                }
                            }
                            else if ((serviceMonths > 12))
                            {
                                if (leaveTaken >= availableLeaves)
                                {
                                    Utility.Errorhandler.GetError("2", "You have no " + ddlLeaveType.SelectedItem.Text.Trim() + "S", lblLeaveMessage);
                                    return;
                                }
                                else if ((leaveTaken < availableLeaves) && (Decimal.Parse(ddlNature.SelectedValue.Trim()) > (availableLeaves - leaveTaken)))
                                {
                                    Utility.Errorhandler.GetError("2", "You have only " + (availableLeaves - leaveTaken).ToString() + " " + ddlLeaveType.SelectedItem.Text.Trim() + "S", lblLeaveMessage);
                                    return;
                                }
                            }
                        }

                        string employeeId = txtEmploeeId.Text.Trim();
                        string leaveDate = txtLeaveDate.Text.Trim();
                        string leaveTypeId = ddlLeaveType.SelectedValue.Trim();
                        string schemeLineNo = Session["SchemeLine"].ToString();
                        string fromTime = ddlFromHH.SelectedItem.Text.Trim() + "." + ddlFromMM.Text.Trim();
                        string toTime = ddlToHH.SelectedItem.Text.Trim() + "." + ddlToMM.Text.Trim();
                        string coveredBy = txtCoveredBy.Text.Trim();
                        string approvedBy = txtApprovedBy.Text.Trim();
                        string remarks = txtRemarks.Text.Trim();
                        string noOfDays = txtNoDays.Text.Trim();

                        string isHalfDay = "";

                        if (ddlNature.SelectedIndex == 1)
                        {
                            isHalfDay = "N";
                        }
                        else if (ddlNature.SelectedIndex == 2)
                        {
                            isHalfDay = "Y";
                        }


                        DataTable leaveBucket_ = new DataTable();
                        leaveBucket_ = (DataTable)Session["leaveBucket"];

                        if (leaveBucket_.Rows.Count > 0)
                        {
                            if (isLeaveExist(leaveBucket_, leaveDate))
                            {
                                Utility.Errorhandler.GetError("2", "You have already add a leave for the date " + leaveDate, lblLeaveMessage);
                                return;
                            }
                        }

                        DataRow oDataRow = leaveBucket_.NewRow();

                        oDataRow["EMPLOYEE_ID"] = employeeId;
                        oDataRow["LEAVE_DATE"] = leaveDate;
                        oDataRow["LEAVE_TYPE_ID"] = leaveTypeId;
                        oDataRow["SCHEME_LINE_NO"] = schemeLineNo;
                        oDataRow["FROM_TIME"] = fromTime;
                        oDataRow["TO_TIME"] = toTime;
                        oDataRow["COVERED_BY"] = coveredBy;
                        oDataRow["REMARKS"] = remarks;
                        oDataRow["NO_OF_DAYS"] = noOfDays;
                        oDataRow["IS_HALFDAY"] = isHalfDay;
                        oDataRow["LEAVE_STATUS"] = "0";
                        oDataRow["TO_APPROVE"] = approvedBy;

                        leaveBucket_.Rows.Add(oDataRow);

                        gvLeaveBucket.DataSource = leaveBucket_;
                        gvLeaveBucket.DataBind();

                        Session["leaveBucket"] = leaveBucket_;

                        if (Session["IsLeaveToBeUpdated"] != null)
                        {
                            Boolean isLeaveToBeUpdated = (Boolean)Session["IsLeaveToBeUpdated"];

                            if (isLeaveToBeUpdated)
                            {
                                btnAdd.Enabled = false;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Boolean isLeaveExist(DataTable leaveTable, string leaveDate)
        {
            Boolean leaveExist = false;

            DataRow[] leaves = leaveTable.Select("LEAVE_DATE ='" + leaveDate.Trim() + "'");

            if (leaves.Length > 0)
            {
                leaveExist = true;
            }

            return leaveExist;
        }

        private void createLeaveBucket()
        {
            DataTable leaveBucket = new DataTable();

            leaveBucket.Columns.Add("EMPLOYEE_ID");
            leaveBucket.Columns.Add("LEAVE_DATE");
            leaveBucket.Columns.Add("LEAVE_TYPE_ID");
            leaveBucket.Columns.Add("SCHEME_LINE_NO");
            leaveBucket.Columns.Add("FROM_TIME");
            leaveBucket.Columns.Add("TO_TIME");
            leaveBucket.Columns.Add("COVERED_BY");
            leaveBucket.Columns.Add("REMARKS");
            leaveBucket.Columns.Add("NO_OF_DAYS");
            leaveBucket.Columns.Add("IS_HALFDAY");
            leaveBucket.Columns.Add("LEAVE_STATUS");
            leaveBucket.Columns.Add("TO_APPROVE");

            Session["leaveBucket"] = leaveBucket;
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

        #endregion

        protected void ddlNumDays_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlNature_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlNature.SelectedItem.Text.Trim() != "")
            {
                txtNoDays.Text = (Double.Parse(ddlNature.SelectedValue)).ToString("#.#");
            }
        }

        protected void gvLeaveBucket_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvLeaveBucket, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void gvLeaveBucket_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable tempBucket = new DataTable();

            try
            {
                tempBucket = (DataTable)Session["leaveBucket"];
                tempBucket.Rows.RemoveAt(gvLeaveBucket.SelectedIndex);
                gvLeaveBucket.DataSource = tempBucket;
                gvLeaveBucket.DataBind();

                Session["leaveBucket"] = tempBucket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable leavesBucket = new DataTable();
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            if(txtEmploeeId.Text.Trim() == txtApprovedBy.Text.Trim()) 
            {
                Utility.Errorhandler.GetError("2", "Leave applicant and approved by are identical ", lblLeaveMessage);
                return;             
            }
            else if (txtEmploeeId.Text.Trim() == txtCoveredBy.Text.Trim())
            {
                Utility.Errorhandler.GetError("2", "Leave applicant and covered by are identical ", lblLeaveMessage);
                return;    
            }

            try
            {
                leavesBucket = (DataTable)Session["leaveBucket"];

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Insert");

                    Boolean isInserted = true;

                    //Boolean isInserted = leaveScheduleDataHandler.Insert(leavesBucket,sUserId);

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

                    string empName = employeeDataHandler.getEmployeeName(txtEmploeeId.Text.Trim());

                    foreach (DataRow dr in leavesBucket.Rows)
                    {
                        string sDate = dr["LEAVE_DATE"].ToString().Trim();

                        string sMail = employeeDataHandler.getEmployeeEmail(dr["TO_APPROVE"].ToString().Trim());

                        if (sMail.Trim() != "")
                        {
                            EmailHandler.SendDefaultEmail("Leave System", sMail, "", "Leave approval", getMailBody(empName, sDate));
                        }

                        string coveredMail = employeeDataHandler.getEmployeeEmail(dr["COVERED_BY"].ToString().Trim());

                        if (coveredMail.Trim() != "")
                        {
                            EmailHandler.SendDefaultEmail("Leave System", coveredMail, "", "Duty Covering", getMailBodyForCoverring(empName, sDate));
                        }
                    }

                    if (isInserted) { Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Leave(s) are inserted", lblLeaveMessage); }


                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Update");

                    //Boolean isUpdated = leaveScheduleDataHandler.update(leavesBucket_, hfPreviousDate.Value.ToString().Trim(), sUserId);

                    //if (isUpdated) { Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Leave is updated", lblLeaveMessage); }

                }

                fillLeaveHistory(txtEmploeeId.Text.Trim());
                fillLeaveSummary(txtEmploeeId.Text.Trim());
                gvLeaveBucket.DataSource = null;
                gvLeaveBucket.DataBind();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                leaveScheduleDataHandler = null;
            }
        }

        private StringBuilder getMailBody(string employeeName, string sDate)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append(employeeName + " has applyed a leave on " + sDate + "." + Environment.NewLine);
            stringBuilder.Append("Please approve it." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }

        private StringBuilder getMailBodyForCoverring(string employeeName, string sDate)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("You are covering the duties of " + employeeName + " on " + sDate + " since he/she is on leave day." + Environment.NewLine);
            stringBuilder.Append(Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append("This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            log.Debug("btnCancel_Click()");
            clear();
        }

        private void clear()
        {
            log.Debug("clear()");

            Utility.Errorhandler.ClearError(lblLeaveMessage);
            GroupHRIS.Utility.Utils.clearControls(true, txtEmploeeId, txtApprovedBy, txtCoveredBy, txtLeaveDate, txtNoDays, txtRemarks, ddlFromHH, ddlToHH, ddlFromMM, ddlToMM);
            hfEmpId.Value = "";
            gvLeaveBucket.DataSource = null;
            gvLeaveBucket.DataBind();
            gvLeaveSummary.DataSource = null;
            gvLeaveSummary.DataBind();
            gvLeaveHistory.DataSource = null;
            gvLeaveHistory.DataBind();
            btnAdd.Enabled = true;
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            
            DataTable leaveBucket_ = (DataTable)Session["leaveBucket"];
            leaveBucket_.Rows.Clear();
            Session["leaveBucket"] = leaveBucket_; 
        }

        protected void gvLeaveHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvLeaveHistory, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        protected void gvLeaveHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();

            try
            {
                string leaveStatus = gvLeaveHistory.SelectedRow.Cells[11].Text.Trim();

                if (leaveStatus.Trim() == "Pending")
                {
                    string employeeId = gvLeaveHistory.SelectedRow.Cells[0].Text.Trim();
                    string leaveDate = gvLeaveHistory.SelectedRow.Cells[1].Text.Trim();

                    hfPreviousDate.Value = leaveDate;

                    DataRow dr = leaveScheduleDataHandler.getLeaveDetailForAGivenLeave(employeeId, leaveDate);


                    if (dr != null)
                    {
                        txtCoveredBy.Text = dr["COVERED_BY"].ToString();
                        txtApprovedBy.Text = dr["TO_APPROVE"].ToString();
                        ddlLeaveType.SelectedValue = dr["LEAVE_TYPE_ID"].ToString();
                        txtNoDays.Text = dr["NO_OF_DAYS"].ToString().Trim();

                        double noOfDays = double.Parse(txtNoDays.Text.Trim());
                        if (noOfDays == Constants.CON_FULL_DAY)
                        {
                            ddlNature.SelectedIndex = 1;
                        }
                        else if (noOfDays == Constants.CON_HALF_DAY)
                        {
                            ddlNature.SelectedIndex = 2;
                        }
                        else if (noOfDays == Constants.CON_SL)
                        {
                            ddlNature.SelectedIndex = 3;
                        }

                        string[] fromTime = dr["FROM_TIME"].ToString().Split('.');
                        string[] toTime = dr["TO_TIME"].ToString().Split('.');

                        //ddlFromHH.SelectedItem.Text = fromTime[0].Trim();
                        //ddlFromMM.SelectedItem.Text = fromTime[1].Trim();

                        //ddlToHH.SelectedItem.Text = toTime[0].Trim();
                        //ddlToMM.SelectedItem.Text = toTime[1].Trim();

                        txtRemarks.Text = dr["REMARKS"].ToString();
                        txtLeaveDate.Text = leaveDate;
                    }

                    Boolean isLeaveToBeUpdated = true;

                    Session["IsLeaveToBeUpdated"] = isLeaveToBeUpdated;

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                    btnAdd.Enabled = true;

                    DataTable leaveBucket_ = (DataTable)Session["leaveBucket"];
                    leaveBucket_.Rows.Clear();
                    Session["leaveBucket"] = leaveBucket_; 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }






    }
}