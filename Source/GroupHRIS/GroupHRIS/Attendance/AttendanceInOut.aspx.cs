using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using Common;
using DataHandler.Attendance;
using DataHandler.MetaData;
using System.Data;
using DataHandler.Employee;

namespace GroupHRIS.Attendance
{
    public partial class AttendanceInOut : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            log.Debug("AttendanceInOut : Page_Load()");

            if(!IsPostBack){
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                string KeyCOMP_ID = (string)(Session["KeyCOMP_ID"]);
                txtemployee.Text = KeyEMPLOYEE_ID;
                fillHours(ddlFromHH);
                fillMinutes(ddlFromMM);
                getCompID(KeyCOMP_ID);
                fillBrnachlocations();
                getSupervisor(KeyEMPLOYEE_ID);
                displayattendance(KeyEMPLOYEE_ID);
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
                }
            }
        }

        private void fillBrnachlocations()
        {
            log.Debug("AttendanceInOut : fillbrnachlocations()");

            BranchCenterDataHandler branchCenterDataHandler = new BranchCenterDataHandler();
            DataTable dtcompbranch = new DataTable();
            try
            {
                dplocation.Items.Clear();
                string mCompCode = dpCompID.SelectedValue.ToString();
                dtcompbranch = branchCenterDataHandler.getBranchesForCompany(mCompCode);
                foreach (DataRow dataRow in dtcompbranch.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = dataRow["BRANCH_NAME"].ToString();
                    listItem.Value = dataRow["BRANCH_ID"].ToString();
                    dplocation.Items.Add(listItem);
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

        private void getSupervisor(string employeeId)
        {
            log.Debug("AttendanceInOut : getSupervisor()");

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

        private void fillHours(DropDownList ddl)
        {
            log.Debug("AttendanceInOut : fillHours()");

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
            log.Debug("AttendanceInOut : fillMinutes()");

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

        private void getCompID(string KeyCOMP_ID)
        {
            log.Debug("AttendanceInOut : getCompID()");

            CompanyDataHandler companynameid = new CompanyDataHandler();
            DataTable CompID = new DataTable();
            try
            {
                if (KeyCOMP_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    CompID = companynameid.getCompanyIdCompName();
                    ListItem lstItem = new ListItem();
                    lstItem.Text = Constants.CON_UNIVERSAL_COMPANY_NAME;
                    lstItem.Value = Constants.CON_UNIVERSAL_COMPANY_CODE;
                    dpCompID.Items.Add(lstItem);
                }
                else
                {
                    CompID = companynameid.getCompanyIdCompName(KeyCOMP_ID);
                }

                foreach (DataRow dataRow in CompID.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = dataRow["COMP_NAME"].ToString();
                    listItem.Value = dataRow["COMPANY_ID"].ToString();
                    dpCompID.Items.Add(listItem);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                companynameid = null;
                CompID.Dispose();
                CompID = null;
            }
        }

        protected void btngeneratecalendar_Click(object sender, EventArgs e)
        {
            log.Debug("AttendanceInOut : btngeneratecalendar_Click()");

            AttendanceDataHandler attendanceDataHandler = new AttendanceDataHandler();

            try
            {
                string sEmpcode = txtemployee.Text.ToString().Trim();
                string sCompID = dpCompID.SelectedValue.ToString();
                string sAttDate = txtfromdate.Text.ToString().Trim().Replace("/","-");
                string sAttLocation = dplocation.SelectedValue.ToString();
                string sHH = ddlFromHH.Text.ToString();
                string sMM = ddlFromMM.Text.ToString();
                string sInout = ddlinout.SelectedValue.ToString();
                string sReasonCode = ddlreason.SelectedValue.ToString();
                string sReason = ddlreason.SelectedItem.ToString();
                string sRecommendby = txtRecommendBy.Text.ToString();
                string sRemark = txtremark.Text.ToString();

                if (sEmpcode == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Employee not selected.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sCompID == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Company not selected.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sAttDate == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Date can not be blank.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sAttLocation == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Location can not be blank.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sHH == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Time Hours can not be blank";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sMM == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Time Minutes can not be blank";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {
                    string sAttTime = sHH + ":" + sMM + ":" + "00";
                    Boolean isupdated = attendanceDataHandler.InsertAttendanceLog(sEmpcode, sEmpcode, sCompID, sInout, sAttDate, sAttLocation, sAttTime, sReasonCode, sReason, sRecommendby, Constants.STATUS_INACTIVE_VALUE, sRemark);
                    if (isupdated == true)
                    {
                        displayattendance(sEmpcode);
                        CommonVariables.MESSAGE_TEXT = "Attendance successfully saved.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "Attendance not updated.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                }
            }
            catch (Exception ex)
            {
                
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void dpCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                fillBrnachlocations(); 
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            txtfromdate.Text = "";
            txtremark.Text = "";
            Utility.Errorhandler.ClearError(lblerror);
        }

        private void displayattendance(string KeyEMP_ID)
        {
            log.Debug("AttendanceInOut : displayattendance()");

            AttendanceDataHandler attendanceDataHandler = new AttendanceDataHandler();
            DataTable DtAttendance = new DataTable();
            DateTime MfromDate = DateTime.Today.AddDays(Constants.CON_ATTENDANCE_VIEW_PERIOD);

            try
            {
                DtAttendance = attendanceDataHandler.populateAttendance(KeyEMP_ID, MfromDate);
                GridView1.DataSource = DtAttendance;
                GridView1.DataBind();

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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            GridView1.PageIndex = e.NewPageIndex;
            displayattendance(KeyEMPLOYEE_ID);

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            log.Debug("AttendanceInOut : GridView1_RowCommand()");

            AttendanceDataHandler attendanceDataHandler = new AttendanceDataHandler();

            try
            {
                Int32 index = Convert.ToInt32(e.CommandArgument);

                GridViewRow selectedRow = GridView1.Rows[index];

                string sEmpcode = selectedRow.Cells[0].Text.ToString().Trim();
                string sAttDate = selectedRow.Cells[1].Text.ToString().Trim();
                string sAttTime = selectedRow.Cells[2].Text.ToString().Trim();
                string sDirection = selectedRow.Cells[3].Text.ToString().Trim();
                string sCompID = selectedRow.Cells[4].Text.ToString().Trim();
                string sAttLocation = selectedRow.Cells[5].Text.ToString().Trim();
                string sReasonCode = selectedRow.Cells[6].Text.ToString().Trim();

                if (e.CommandName.ToString().Equals("cancelrow"))
                {
                    Boolean isupdated = attendanceDataHandler.UpdateAttendanceLog(sEmpcode, sCompID, sDirection, sAttDate, sAttLocation, sAttTime, sReasonCode, Constants.STATUS_CANCEL_VALUE);
                    if (isupdated == true)
                    {
                        displayattendance(sEmpcode);
                        CommonVariables.MESSAGE_TEXT = "Attendance successfully Cancelled.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
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
                attendanceDataHandler = null;
            }
        }


     }
}