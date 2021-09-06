using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using DataHandler.Userlogin;
using Common;
using DataHandler.Attendance;
using DataHandler.Employee;
using System.Data;
using NLog;

namespace GroupHRIS.Attendance
{
    public partial class AttendanceApproveReject : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sapprovedBy = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "AttendanceInOutApproval : Page_Load");

            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {
                sapprovedBy = Session["KeyEMPLOYEE_ID"].ToString();
            }

            if (!IsPostBack)
            {
                loadInoutForApproval(sapprovedBy);
            }
        }

        private void loadInoutForApproval(string approvedBy)
        {
            log.Debug("AttendanceInOutApproval : loadInoutForApproval()");

            DataTable employeeinout = new DataTable();
            AttendanceDataHandler attendanceDataHandler = new AttendanceDataHandler();
            DataTable DtAttendance = new DataTable();
            DateTime MfromDate = DateTime.Today.AddDays(Constants.CON_ATTENDANCE_VIEW_PERIOD);

            try
            {
                employeeinout = attendanceDataHandler.populateAttendanceOfficer(approvedBy, MfromDate).Copy();

                if (employeeinout.Rows.Count > 0)
                {
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else
                {
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }

                gvLeaves.DataSource = employeeinout;
                gvLeaves.DataBind();

            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
            finally
            {
                attendanceDataHandler = null;
                employeeinout.Dispose();
                employeeinout = null;
            }

        }

        protected void chkBxHeaderApprove_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerChkBox = ((CheckBox)gvLeaves.HeaderRow.FindControl("chkBxHeaderApprove"));

            if (headerChkBox.Checked == true)
            {
                for (int i = 0; i < gvLeaves.Rows.Count; i++)
                {
                    ((CheckBox)gvLeaves.Rows[i].Cells[7].FindControl("chkBxSelectReject")).Checked = false;
                    ((CheckBox)gvLeaves.Rows[i].Cells[7].FindControl("chkBxSelectApprove")).Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < gvLeaves.Rows.Count; i++)
                {
                    ((CheckBox)gvLeaves.Rows[i].Cells[7].FindControl("chkBxSelectApprove")).Checked = false;
                }
            }
        }

        protected void chkBxHeaderReject_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerChkBox = ((CheckBox)gvLeaves.HeaderRow.FindControl("chkBxHeaderReject"));

            if (headerChkBox.Checked == true)
            {
                for (int i = 0; i < gvLeaves.Rows.Count; i++)
                {
                    ((CheckBox)gvLeaves.Rows[i].Cells[7].FindControl("chkBxSelectApprove")).Checked = false;
                    ((CheckBox)gvLeaves.Rows[i].Cells[7].FindControl("chkBxSelectReject")).Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < gvLeaves.Rows.Count; i++)
                {
                    ((CheckBox)gvLeaves.Rows[i].Cells[7].FindControl("chkBxSelectReject")).Checked = false;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            createUpdateBucket();
            createUpdateRejectBucket();

            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            AttendanceApproveRejectDataHandler attendanceApproveReject = new AttendanceApproveRejectDataHandler();

            AttendanceDataHandler attendanceDataHandler = new AttendanceDataHandler();

            DataTable dtApprove = (DataTable)Session["updateBucket"];

            DataTable dtReject = (DataTable)Session["updateRejectBucket"];

            try
            {
                for (int i = 0; i < gvLeaves.Rows.Count; i++)
                {
                    if ((gvLeaves.Rows[i].Cells[12].FindControl("chkBxSelectApprove") as CheckBox).Checked == true)
                    {
                        string sEmpcode = gvLeaves.Rows[i].Cells[0].Text.ToString().Trim();
                        string sEmpName = gvLeaves.Rows[i].Cells[1].Text.ToString().Trim();
                        string sAttDate = gvLeaves.Rows[i].Cells[2].Text.ToString().Trim();
                        string sAttTime = gvLeaves.Rows[i].Cells[3].Text.ToString().Trim();
                        string company = gvLeaves.Rows[i].Cells[4].Text.ToString().Trim();
                        string sINOUT = gvLeaves.Rows[i].Cells[5].Text.ToString().Trim();
                        string sCompID = gvLeaves.Rows[i].Cells[6].Text.ToString().Trim();
                        string sReasonCode = gvLeaves.Rows[i].Cells[7].Text.ToString().Trim();
                        string sAttLocation = gvLeaves.Rows[i].Cells[8].Text.ToString().Trim();
                        string sDirection = gvLeaves.Rows[i].Cells[9].Text.ToString().Trim();
                        string reason = gvLeaves.Rows[i].Cells[10].Text.ToString().Trim();
                        string status = gvLeaves.Rows[i].Cells[11].Text.ToString().Trim();

                        string mailAddress = employeeDataHandler.getEmployeeEmail(sEmpcode);

                        DataRow dtrow = dtApprove.NewRow();

                        dtrow["EMPLOYEE_ID"] = sEmpcode;
                        dtrow["EMPNAME"] = sEmpName;
                        dtrow["ATT_DATE"] = sAttDate;
                        dtrow["ATT_TIME"] = sAttTime;
                        dtrow["COMPANY"] = company;
                        dtrow["COMPANY_ID"] = sCompID;
                        dtrow["REASON_CODE"] = sReasonCode;
                        dtrow["DIRECTION"] = sDirection;
                        dtrow["REASON"] = reason;
                        dtrow["BRANCH_ID"] = sAttLocation;
                        dtrow["STATUS"] = status;
                        dtrow["EMAIL"] = mailAddress;

                        dtApprove.Rows.Add(dtrow);

                    }
                    else if ((gvLeaves.Rows[i].Cells[12].FindControl("chkBxSelectReject") as CheckBox).Checked == true)
                    {
                        string sEmpcode = gvLeaves.Rows[i].Cells[0].Text.ToString().Trim();
                        string sEmpName = gvLeaves.Rows[i].Cells[1].Text.ToString().Trim();
                        string sAttDate = gvLeaves.Rows[i].Cells[2].Text.ToString().Trim();
                        string sAttTime = gvLeaves.Rows[i].Cells[3].Text.ToString().Trim();
                        string company = gvLeaves.Rows[i].Cells[4].Text.ToString().Trim();
                        string sINOUT = gvLeaves.Rows[i].Cells[5].Text.ToString().Trim();
                        string sCompID = gvLeaves.Rows[i].Cells[6].Text.ToString().Trim();
                        string sReasonCode = gvLeaves.Rows[i].Cells[7].Text.ToString().Trim();
                        string sAttLocation = gvLeaves.Rows[i].Cells[8].Text.ToString().Trim();
                        string sDirection = gvLeaves.Rows[i].Cells[9].Text.ToString().Trim();
                        string reason = gvLeaves.Rows[i].Cells[10].Text.ToString().Trim();
                        string status = gvLeaves.Rows[i].Cells[11].Text.ToString().Trim();

                        string mailAddress = employeeDataHandler.getEmployeeEmail(sEmpcode);

                        DataRow dtrow = dtReject.NewRow();

                        dtrow["EMPLOYEE_ID"] = sEmpcode;
                        dtrow["EMPNAME"] = sEmpName;
                        dtrow["ATT_DATE"] = sAttDate;
                        dtrow["ATT_TIME"] = sAttTime;
                        dtrow["COMPANY"] = company;
                        dtrow["COMPANY_ID"] = sCompID;
                        dtrow["REASON_CODE"] = sReasonCode;
                        dtrow["DIRECTION"] = sDirection;
                        dtrow["REASON"] = reason;
                        dtrow["BRANCH_ID"] = sAttLocation;
                        dtrow["STATUS"] = status;
                        dtrow["EMAIL"] = mailAddress;

                        dtReject.Rows.Add(dtrow);

                    }
                }

                if (dtApprove.Rows.Count > 0)
                {
                    Boolean isUpdated = attendanceApproveReject.UpdateAttendanceLog(dtApprove);
                    if (isUpdated == true)
                    {
                        createDataBUcket();

                        string preEmpId = "";
                        DataTable mailBucket = (DataTable)Session["mailBucket"]; 

                        if (dtApprove.Rows.Count > 0)
                        {

                            foreach (DataRow dr in dtApprove.Rows)
                            {
                                string empid = dr["EMPLOYEE_ID"].ToString();
                                if (preEmpId == "")
                                {
                                    preEmpId = empid;

                                    DataRow dtrow = mailBucket.NewRow();

                                    dtrow["EMPLOYEE_ID"] = dr["EMPLOYEE_ID"].ToString();
                                    dtrow["EMPNAME"] = dr["EMPNAME"].ToString();
                                    dtrow["ATT_DATE"] = dr["ATT_DATE"].ToString();
                                    dtrow["ATT_TIME"] = dr["ATT_TIME"].ToString();
                                    dtrow["DIRECTION"] = dr["DIRECTION"].ToString();

                                    mailBucket.Rows.Add(dtrow);

                                }
                                else if (preEmpId == empid)
                                {
                                    DataRow dtrow = mailBucket.NewRow();

                                    dtrow["EMPLOYEE_ID"] = dr["EMPLOYEE_ID"].ToString();
                                    dtrow["EMPNAME"] = dr["EMPNAME"].ToString();
                                    dtrow["ATT_DATE"] = dr["ATT_DATE"].ToString();
                                    dtrow["ATT_TIME"] = dr["ATT_TIME"].ToString();
                                    dtrow["DIRECTION"] = dr["DIRECTION"].ToString();

                                    mailBucket.Rows.Add(dtrow);
                                }
                                else
                                {
                                    string email = employeeDataHandler.getEmployeeEmail(preEmpId);
                                    string empName = employeeDataHandler.getEmployeeKnownName(preEmpId);
                            
                                    preEmpId = "";
                                    //send e mail & clear data bucket
                                    if (email.Trim() != "")
                                    {
                                        EmailHandler.SendHTMLMail("Group HRIS - Missing In/Out Approve", email, "Missing In/Out Approve", getApprovedMessage(mailBucket, email, empName));
                                    }
                                    mailBucket.Rows.Clear();

                                    preEmpId = dr["EMPLOYEE_ID"].ToString();
                                    DataRow dtrow = mailBucket.NewRow();

                                    dtrow["EMPLOYEE_ID"] = dr["EMPLOYEE_ID"].ToString();
                                    dtrow["EMPNAME"] = dr["EMPNAME"].ToString();
                                    dtrow["ATT_DATE"] = dr["ATT_DATE"].ToString();
                                    dtrow["ATT_TIME"] = dr["ATT_TIME"].ToString();
                                    dtrow["DIRECTION"] = dr["DIRECTION"].ToString();

                                    mailBucket.Rows.Add(dtrow);
                                }
                            }
                        }
                        if (preEmpId != "")
                        {
                            string email = employeeDataHandler.getEmployeeEmail(preEmpId);
                            string empName = employeeDataHandler.getEmployeeKnownName(preEmpId);
                            
                            //send e mail & clear data bucket
                            if (email.Trim() != "")
                            {
                                EmailHandler.SendHTMLMail("Group HRIS - Missing In/Out Approvel ", email, "Missing In/Out Approvel", getApprovedMessage(mailBucket, email, empName));
                            }

                            preEmpId = "";
                            mailBucket.Rows.Clear();
                        }

                        loadInoutForApproval(sapprovedBy);
                    }
                }

                if (dtReject.Rows.Count > 0)
                {
                    Boolean isUpdated = attendanceApproveReject.UpdateAttendanceLogRejection(dtReject);
                    if (isUpdated == true)
                    {
                        createDataBUcket();

                        string preEmpId = "";
                        DataTable mailBucket = (DataTable)Session["mailBucket"];

                        if (dtReject.Rows.Count > 0)
                        {

                            foreach (DataRow dr in dtReject.Rows)
                            {
                                string empid = dr["EMPLOYEE_ID"].ToString();
                                if (preEmpId == "")
                                {
                                    preEmpId = empid;

                                    DataRow dtrow = mailBucket.NewRow();

                                    dtrow["EMPLOYEE_ID"] = dr["EMPLOYEE_ID"].ToString();
                                    dtrow["EMPNAME"] = dr["EMPNAME"].ToString();
                                    dtrow["ATT_DATE"] = dr["ATT_DATE"].ToString();
                                    dtrow["ATT_TIME"] = dr["ATT_TIME"].ToString();
                                    dtrow["DIRECTION"] = dr["DIRECTION"].ToString();

                                    mailBucket.Rows.Add(dtrow);

                                }
                                else if (preEmpId == empid)
                                {
                                    DataRow dtrow = mailBucket.NewRow();

                                    dtrow["EMPLOYEE_ID"] = dr["EMPLOYEE_ID"].ToString();
                                    dtrow["EMPNAME"] = dr["EMPNAME"].ToString();
                                    dtrow["ATT_DATE"] = dr["ATT_DATE"].ToString();
                                    dtrow["ATT_TIME"] = dr["ATT_TIME"].ToString();
                                    dtrow["DIRECTION"] = dr["DIRECTION"].ToString();

                                    mailBucket.Rows.Add(dtrow);
                                }
                                else
                                {
                                    string email = employeeDataHandler.getEmployeeEmail(preEmpId);
                                    string empName = employeeDataHandler.getEmployeeKnownName(preEmpId);

                                    //send e mail & clear data bucket
                                    if (email.Trim() != "")
                                    {
                                        EmailHandler.SendHTMLMail("Group HRIS - Missing In/Out Rejection", email, "Missing In/Out Rejection", getRejectedMessage(mailBucket, email, empName));
                                    }
                                    mailBucket.Rows.Clear();

                                    preEmpId = dr["EMPLOYEE_ID"].ToString();
                                    DataRow dtrow = mailBucket.NewRow();

                                    dtrow["EMPLOYEE_ID"] = dr["EMPLOYEE_ID"].ToString();
                                    dtrow["EMPNAME"] = dr["EMPNAME"].ToString();
                                    dtrow["ATT_DATE"] = dr["ATT_DATE"].ToString();
                                    dtrow["ATT_TIME"] = dr["ATT_TIME"].ToString();
                                    dtrow["DIRECTION"] = dr["DIRECTION"].ToString();

                                    mailBucket.Rows.Add(dtrow);
                                }
                            }
                        }
                        if (preEmpId != "")
                        {
                            string email = employeeDataHandler.getEmployeeEmail(preEmpId);
                            string empName = employeeDataHandler.getEmployeeKnownName(preEmpId);

                            //send e mail & clear data bucket
                            if (email.Trim() != "")
                            {
                                EmailHandler.SendHTMLMail("Group HRIS - Missing In/Out Rejection ", email, "Missing In/Out Rejection", getRejectedMessage(mailBucket, email, empName));
                            }

                            preEmpId = "";
                            mailBucket.Rows.Clear();
                        }

                        loadInoutForApproval(sapprovedBy);
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
                attendanceDataHandler = null;
                employeeDataHandler = null;
            }
        }

        public string getApprovedMessage(DataTable mailTable, string emailadd, string empName)
        {
            PasswordHandler crpto = new PasswordHandler();
            //string empName = "";
            string email = "";
            string var1 = String.Empty;

            var1 = "Dear Mr/Ms " + empName + "," + Environment.NewLine + Environment.NewLine + "</br></br>";
            var1 += "Your following IN/OUT Records has been Approved" + Environment.NewLine + Environment.NewLine + "</br></br>";

            DataTable dt = new DataTable();
            dt.Columns.Add("IN/OUT DATE");
            dt.Columns.Add("TIME");
            dt.Columns.Add("DIRECTION");

            for (int i = 0; i < mailTable.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["IN/OUT DATE"] = mailTable.Rows[i]["ATT_DATE"].ToString();
                dr["TIME"] = mailTable.Rows[i]["ATT_TIME"].ToString();
                dr["DIRECTION"] = mailTable.Rows[i]["DIRECTION"].ToString();
                dt.Rows.Add(dr);
            }

            mailTable = new DataTable();
            mailTable = dt;

            // StringBuilder stringBuilder = new StringBuilder();

            string var = "<table style='border: 1px solid black;border-collapse: collapse;'>";
            //add header row
            var += "<tr>";
            for (int i = 0; i < mailTable.Columns.Count; i++)
                var += "<th  style='border: 1px solid black;border-collapse: collapse;'>&nbsp;&nbsp;" + mailTable.Columns[i].ColumnName + "</th>";
            var += "</tr>";
            //add rows
            for (int i = 0; i < mailTable.Rows.Count; i++)
            {
                var += "<tr>";
                for (int j = 0; j < mailTable.Columns.Count; j++)
                    var += "<td  style='border: 1px solid black;border-collapse: collapse;'>&nbsp;&nbsp;&nbsp;" + mailTable.Rows[i][j].ToString() + "</td>";
                var += "</tr>";
            }
            var += "</table>";

            var1 += var;

            var1 += Environment.NewLine + "This is a system generated mail." + Environment.NewLine + "<br/>";
            var1 += Environment.NewLine + "Thank You" + Environment.NewLine;

            return var1;
        }

        public string getRejectedMessage(DataTable mailTable, string emailadd, string empName)
        {
            PasswordHandler crpto = new PasswordHandler();
            //string empName = "";
            string email = "";
            string var1 = String.Empty;

            var1 = "Dear Mr/Ms " + empName + "," + Environment.NewLine + Environment.NewLine + "</br></br>";
            var1 += "Your following IN/OUT Records has been Rejected" + Environment.NewLine + Environment.NewLine + "</br></br>";

            DataTable dt = new DataTable();
            dt.Columns.Add("IN/OUT DATE");
            dt.Columns.Add("TIME");
            dt.Columns.Add("DIRECTION");

            for (int i = 0; i < mailTable.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["IN/OUT DATE"] = mailTable.Rows[i]["ATT_DATE"].ToString();
                dr["TIME"] = mailTable.Rows[i]["ATT_TIME"].ToString();
                dr["DIRECTION"] = mailTable.Rows[i]["DIRECTION"].ToString();
                dt.Rows.Add(dr);
            }

            mailTable = new DataTable();
            mailTable = dt;

            // StringBuilder stringBuilder = new StringBuilder();

            string var = "<table style='border: 1px solid black;border-collapse: collapse;'>";
            //add header row
            var += "<tr>";
            for (int i = 0; i < mailTable.Columns.Count; i++)
                var += "<th  style='border: 1px solid black;border-collapse: collapse;'>&nbsp;&nbsp;" + mailTable.Columns[i].ColumnName + "</th>";
            var += "</tr>";
            //add rows
            for (int i = 0; i < mailTable.Rows.Count; i++)
            {
                var += "<tr>";
                for (int j = 0; j < mailTable.Columns.Count; j++)
                    var += "<td  style='border: 1px solid black;border-collapse: collapse;'>&nbsp;&nbsp;&nbsp;" + mailTable.Rows[i][j].ToString() + "</td>";
                var += "</tr>";
            }
            var += "</table>";

            var1 += var;

            var1 += Environment.NewLine + "This is a system generated mail." + Environment.NewLine + "<br/>";
            var1 += Environment.NewLine + "Thank You" + Environment.NewLine;

            return var1;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ((CheckBox)gvLeaves.HeaderRow.FindControl("chkBxHeaderReject")).Checked = false;
            ((CheckBox)gvLeaves.HeaderRow.FindControl("chkBxHeaderApprove")).Checked = false;

            for (int i = 0; i < gvLeaves.Rows.Count; i++)
            {
                ((CheckBox)gvLeaves.Rows[i].Cells[7].FindControl("chkBxSelectReject")).Checked = false;
                ((CheckBox)gvLeaves.Rows[i].Cells[7].FindControl("chkBxSelectApprove")).Checked = false;
            }
        }

        public void createUpdateBucket()
        {
            DataTable updateBucket = new DataTable();

            updateBucket.Columns.Add("EMPLOYEE_ID", typeof(string));
            updateBucket.Columns.Add("EMPNAME", typeof(string));
            updateBucket.Columns.Add("ATT_DATE", typeof(string));
            updateBucket.Columns.Add("ATT_TIME", typeof(string));
            updateBucket.Columns.Add("COMPANY", typeof(string));
            updateBucket.Columns.Add("DIRECTION", typeof(string));
            updateBucket.Columns.Add("COMPANY_ID", typeof(string));
            updateBucket.Columns.Add("REASON_CODE", typeof(string));
            updateBucket.Columns.Add("BRANCH_ID", typeof(string));
            //updateBucket.Columns.Add("DIRECTIONINOUT", typeof(string));
            updateBucket.Columns.Add("REASON", typeof(string));
            updateBucket.Columns.Add("STATUS", typeof(string));
            updateBucket.Columns.Add("EMAIL", typeof(string));

            Session["updateBucket"] = updateBucket;
        }

        public void createUpdateRejectBucket()
        {
            DataTable updateRejectBucket = new DataTable();

            updateRejectBucket.Columns.Add("EMPLOYEE_ID", typeof(string));
            updateRejectBucket.Columns.Add("EMPNAME", typeof(string));
            updateRejectBucket.Columns.Add("ATT_DATE", typeof(string));
            updateRejectBucket.Columns.Add("ATT_TIME", typeof(string));
            updateRejectBucket.Columns.Add("COMPANY", typeof(string));
            updateRejectBucket.Columns.Add("DIRECTION", typeof(string));
            updateRejectBucket.Columns.Add("COMPANY_ID", typeof(string));
            updateRejectBucket.Columns.Add("REASON_CODE", typeof(string));
            updateRejectBucket.Columns.Add("BRANCH_ID", typeof(string));
            //updateRejectBucket.Columns.Add("DIRECTIONINOUT", typeof(string));
            updateRejectBucket.Columns.Add("REASON", typeof(string));
            updateRejectBucket.Columns.Add("STATUS", typeof(string));
            updateRejectBucket.Columns.Add("EMAIL", typeof(string));

            Session["updateRejectBucket"] = updateRejectBucket;
        }

        public void createDataBUcket()
        {
            DataTable mailBucket = new DataTable();

            mailBucket.Columns.Add("EMPLOYEE_ID", typeof(string));
            mailBucket.Columns.Add("EMPNAME", typeof(string));
            mailBucket.Columns.Add("ATT_DATE", typeof(string));
            mailBucket.Columns.Add("ATT_TIME", typeof(string));
            mailBucket.Columns.Add("DIRECTION", typeof(string));

            Session["mailBucket"] = mailBucket;
        }

        protected void gvLeaves_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable employeeinout = new DataTable();
                AttendanceDataHandler attendanceDataHandler = new AttendanceDataHandler();
                DataTable DtAttendance = new DataTable();
                DateTime MfromDate = DateTime.Today.AddDays(Constants.CON_ATTENDANCE_VIEW_PERIOD);

                gvLeaves.PageIndex = e.NewPageIndex;
                gvLeaves.DataSource = attendanceDataHandler.populateAttendanceOfficer(Session["KeyEMPLOYEE_ID"].ToString(), MfromDate).Copy();
                gvLeaves.DataBind();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}