using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Userlogin;
using DataHandler.Employee;
using DataHandler.EmployeeLeave;
using System.Data;
using DataHandler.MetaData;
using Common;
using System.Web.UI.DataVisualization.Charting;
using DataHandler.Reminders;
using GroupHRIS.HRISAlerts;

namespace GroupHRIS.Login
{
    public partial class Hrismain : System.Web.UI.Page
    {
        public string BirthdayOutput = String.Empty;
        public string NotificationOutput = String.Empty;
        public string LEAVE_DETAILS = String.Empty;
        public string LEAVE_TAKEN_DETAILS = String.Empty;
        public string LEAVE_BALANCE_DETAILS = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                string KeyUSER_ID = (string)(Session["KeyUSER_ID"]);
                string KeyUSER_FIRSTNAME = (string)(Session["KeyUSER_FIRSTNAME"]);
                string KeyHRIS_ROLE = (string)(Session["KeyHRIS_ROLE"]);
                string KeyCOMP_ID = (string)(Session["KeyCOMP_ID"]);
                string KeyEMPLOYEE_GENDER = (string)(Session["KeyEMPLOYEE_GENDER"]);

                get_employee(KeyEMPLOYEE_ID);
                get_funarea(KeyCOMP_ID);
                get_employeephoto(KeyEMPLOYEE_ID, KeyEMPLOYEE_GENDER);
                get_LeaveDetails(KeyEMPLOYEE_ID);
                GetScroller(KeyEMPLOYEE_ID);  

                loadAlerts();              
            }
        }

        private void get_company(string companyId)
        {

            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataRow  eCompany = null;

            try
            {
                    eCompany = companyDataHandler.getCompanyOtheDetails(companyId);
                    if (eCompany != null)
                    {
                        string cCOMP_NAME = eCompany["COMP_NAME"].ToString();
                        string cCOMP_ADDRESS_LINE1 = eCompany["COMP_ADDRESS_LINE1"].ToString();
                        string cCOMP_ADDRESS_LINE2 = eCompany["COMP_ADDRESS_LINE2"].ToString();
                        string cCOMP_ADDRESS_LINE3 = eCompany["COMP_ADDRESS_LINE3"].ToString();
                        string cCOMP_ADDRESS_LINE4 = eCompany["COMP_ADDRESS_LINE4"].ToString();
                        string LAND_PHONE1 = eCompany["LAND_PHONE1"].ToString();
                        string LAND_PHONE2 = eCompany["LAND_PHONE2"].ToString();
                        string FAX_NUMBER = eCompany["FAX_NUMBER"].ToString();
                        string WORK_HOURS_START = eCompany["WORK_HOURS_START"].ToString();
                        string WORK_HOURS_END = eCompany["WORK_HOURS_END"].ToString();

                    }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                companyDataHandler = null;
                eCompany = null;
            }
        }

        private void get_funarea(string companyId)
        {

            //CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            //DataRow eCompany = null;

            //try
            //{
            //    if (companyId.Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
            //    {
            //        lblfunarea.Text = "I am Working for " + Constants.CON_UNIVERSAL_COMPANY_NAME;
            //    }
            //    else
            //    {
            //        eCompany = companyDataHandler.getCompanyOtheDetails(companyId);
            //        if (eCompany != null)
            //        {
            //            string cCOMP_NAME = eCompany["COMP_NAME"].ToString();
            //            lblfunarea.Text = "I am Working for " + cCOMP_NAME;
            //        }
            //        else
            //        {
            //            lblfunarea.Text = "Functional Area Not Found";
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{

            //    CommonVariables.MESSAGE_TEXT = ex.Message;
            //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            //}
            //finally
            //{
            //    companyDataHandler = null;
            //    eCompany = null;
            //}
        }

        private void get_employeephoto(string KeyEMPLOYEE_ID, string KeyEMPLOYEE_GENDER)
        {
            //EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            //DataTable employeePhoto = new DataTable();

            //try
            //{
            //    employeePhoto = employeeDataHandler.populateEmpPhoto(KeyEMPLOYEE_ID);
            //    if (employeePhoto.Rows.Count > 0)
            //    {
            //        DataRow dr = employeePhoto.Rows[0];
            //        string imagepath = dr["IMAGEPATH"].ToString() + dr["IMAGENAME"].ToString();
            //        imgme.ImageUrl = imagepath;
            //        imgme.Width = int.Parse(dr["IMAGEWIDTH"].ToString());
            //        imgme.Height = int.Parse(dr["IMAGEHEIGHT"].ToString());
            //        dr = null;
            //    }
            //    else
            //    {
            //        if (KeyEMPLOYEE_GENDER == Constants.CON_GENDER_FEMALE)
            //        {
            //            imgme.ImageUrl = Constants.CON_DEFAULT_FEMALE_IMAGE_PATH;
            //            imgme.Width = 128;
            //            imgme.Height = 150;
            //        }
            //        else if (KeyEMPLOYEE_GENDER == Constants.CON_GENDER_MALE)
            //        {
            //            imgme.ImageUrl = Constants.CON_DEFAULT_MALE_IMAGE_PATH;
            //            imgme.Width = 128;
            //            imgme.Height = 150;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //    CommonVariables.MESSAGE_TEXT = ex.Message;
            //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            //}
            //finally
            //{
            //    employeeDataHandler = null;
            //    employeePhoto.Dispose();
            //    employeePhoto = null;

            //}
        }

        private void get_employee(string empId)
        {
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            DataRow employeeDT = null;

            try
            {
                employeeDT = employeeDataHandler.populate(empId);
                if (employeeDT != null)
                {
                    DateTime eDOB = Convert.ToDateTime(employeeDT["DOB"].ToString());
                    DateTime eDOJ = Convert.ToDateTime(employeeDT["DOJ"].ToString());
                    string eCOMPANY_ID = employeeDT["COMPANY_ID"].ToString();
                    string eFIRST_NAME = employeeDT["FIRST_NAME"].ToString();
                    string eEMP_INITIALS = employeeDT["EMP_INITIALS"].ToString();
                    string eLAST_NAME = employeeDT["LAST_NAME"].ToString();
                    string eEMP_NIC = employeeDT["EMP_NIC"].ToString();
                    string eEMAIL = employeeDT["EMAIL"].ToString();
                    string eEPF_NO = employeeDT["EPF_NO"].ToString();
                    string ePERMANENT_ADDRESS = employeeDT["PERMANENT_ADDRESS"].ToString();
                    string eMOBILE_PHONE_PERSONAL = employeeDT["MOBILE_PHONE_PERSONAL"].ToString();
                    string eMOBILE_PHONE_COMPANY = employeeDT["MOBILE_PHONE_COMPANY"].ToString();

                    //lbladdress.Text = ePERMANENT_ADDRESS;
                    //lblcompmob.Text ="Company Mobile No. " +  eMOBILE_PHONE_COMPANY;
                    //lbldob.Text = "I was Born in " + eDOB.ToString("yyyy/MM/dd");
                    //lbldoj.Text = "Member Since " + eDOJ.ToString("yyyy/MM/dd");
                    //lblemail.Text = "Email Address " + eEMAIL;
                    //lblepf.Text = "E.P.F. No. " + eEPF_NO;
                    //lblfirstname.Text = eFIRST_NAME.ToUpper();
                    //lbllastname.Text = eLAST_NAME.ToUpper();
                    //lblnic.Text = "NIC No. " + eEMP_NIC;
                    //lblpermob.Text = "Personal Mobile No. " + eMOBILE_PHONE_PERSONAL;

                    get_company(eCOMPANY_ID);
                }
            }
            catch (Exception ex)
            {

                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                employeeDataHandler = null;
                employeeDT = null;
            }
        }

        private void GetScroller(string empId)
        {
            string strMARQUEE1 = "";

            strMARQUEE1 = strMARQUEE1 + "<div id='scroller'>";
            strMARQUEE1 = strMARQUEE1 + "<div id='wn'>";
            strMARQUEE1 = strMARQUEE1 + "<div id='lyr'>";

            ReminderDataHandler reminderDataHandler = new ReminderDataHandler();
            string ResCultryDateTime = reminderDataHandler.getCultryDateTime();
            DataTable reminderdata = reminderDataHandler.populateReminders (empId , DateTime.Parse(ResCultryDateTime));

            try
            {
                if (reminderdata.Rows.Count > 0)
                {
                    for (int i = 0; i < reminderdata.Rows.Count;i++ )
                    {
                        strMARQUEE1 = strMARQUEE1 + "<div class='block'>" + reminderdata.Rows[i][0].ToString() + "</div>";
                    }
                    strMARQUEE1 = strMARQUEE1 + "<div id='rpt' class='block'>" + reminderdata.Rows[0][0].ToString() + "</div>";
                    strMARQUEE1 = strMARQUEE1 + "</div></div></div>";
                    //lblremscroller.BackColor = System.Drawing.Color.Transparent;
                    lblremscroller.Text = strMARQUEE1;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                reminderDataHandler = null;
                reminderdata.Dispose();
                reminderdata = null;
            }
        }

        string leaveDetailsJS(string Lables, string Values)
        {
            string JS =@"

                            <script>

                                    var barChartData = {
                                        labels: [" + Lables + @"],
                                        datasets: [
	  			                            {
	  			                                fillColor: 'rgba(0,0,255,0.5)',
	  			                                strokeColor: 'rgba(0,0,255,0.8)',
	  			                                highlightFill: 'rgba(0,0,255,0.75)',
	  			                                highlightStroke: 'rgba(0,0,255,1)',
	  			                                data: [" + Values + @"]
	  			                            }
	  		                            ]

	                             };


		                </script>

                        ";

            return JS;
        }

        string leaveTakenJS(string Lables, string Values)
        {
            string JS = @"

                            <script>

                                    var barChartData1 = {
                                        labels: [" + Lables + @"],
                                        datasets: [
	  			                            {
	  			                                fillColor: 'rgba(255,165,0,0.5)',
	  			                                strokeColor: 'rgba(255,165,0,0.8)',
	  			                                highlightFill: 'rgba(255,165,0,0.75)',
	  			                                highlightStroke: 'rgba(255,165,0,1)',
	  			                                data: [" + Values + @"]
	  			                            }
	  		                            ]

	                             };


		                </script>

                        ";

            return JS;
        }

        string leaveBalanceJS(string Lables, string Values)
        {
            string JS = @"

                            <script>

                                    var barChartData2 = {
                                        labels: [" + Lables + @"],
                                        datasets: [
	  			                            {
	  			                                fillColor: 'rgba(0,255,0,0.5)',
	  			                                strokeColor: 'rgba(0,255,0,0.8)',
	  			                                highlightFill: 'rgba(0,255,0,0.75)',
	  			                                highlightStroke: 'rgba(0,255,0,1)',
	  			                                data: [" + Values + @"]
	  			                            }
	  		                            ]

	                             };

                                    window.onload = function () {
                                        var ctx2 = document.getElementById('canvasLeaveBalanceDetails').getContext('2d');

                                        var chart = new Chart(ctx2).HorizontalBar(barChartData2, {
                                            responsive: true,
                                            barShowStroke: true,
                                            scaleBeginAtZero: true,
                                            scaleShowHorizontalLines: true,
                                            scaleOverride : true,
                                            scaleSteps : 4,
                                            scaleStepWidth : 5,
                                            scaleStartValue : 0 
                                        });

                                        var ctx1 = document.getElementById('canvasLeaveTakenDetails').getContext('2d');

                                        var chart = new Chart(ctx1).HorizontalBar(barChartData1, {
                                            responsive: true,
                                            barShowStroke: true,
                                            scaleBeginAtZero: true,
                                            scaleShowHorizontalLines: true,
                                            scaleOverride : true,
                                            scaleSteps : 4,
                                            scaleStepWidth : 5,
                                            scaleStartValue : 0 
                                        });

                                        var ctx = document.getElementById('canvasLeaveDetails').getContext('2d');

                                        var chart = new Chart(ctx).HorizontalBar(barChartData, {
                                            responsive: true,
                                            barShowStroke: true,
                                            scaleBeginAtZero: true,
                                            scaleShowHorizontalLines: true,
                                            scaleOverride : true,
                                            scaleSteps : 4,
                                            scaleStepWidth : 5,
                                            scaleStartValue : 0 
                                        });



                                    };

		                </script>

                        ";
            
            return JS;
        }

        private void get_LeaveDetails(string empId)
        {
            LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
            string sYear = DateTime.Today.ToString("yyyy/MM/dd").Substring(0, 4);

            DataTable employeeLeave = null;

            try
            {
                employeeLeave = leaveScheduleDataHandler.getEmployeeLeveSummaryChart(empId, sYear);
                if (employeeLeave != null)
                {

                    int icount = employeeLeave.Rows.Count;
                    string mleaveType = "";
                    double mleaveNo = 0;
                    double mleaveTaken = 0;
                    double mleaveBalance = 0;

                    string LeaveTypes = String.Empty;
                    string EntitleLeaves = String.Empty;
                    string TakenLeaves = String.Empty;
                    string BlanceLeaves = String.Empty;


                    for (int i = 0; i <= icount - 1; i++)
                    {
                        mleaveType = employeeLeave.Rows[i][0].ToString();
                        LeaveTypes += "'" + mleaveType + "', ";

                        mleaveNo = double.Parse(employeeLeave.Rows[i][1].ToString());
                        EntitleLeaves += "" + mleaveNo + ", ";

                        mleaveTaken = double.Parse(employeeLeave.Rows[i][2].ToString());
                        TakenLeaves += "" + mleaveTaken + ", ";

                        mleaveBalance = mleaveNo - mleaveTaken;
                        BlanceLeaves += "" + mleaveBalance + ", ";

                        //chrtLeaveDetails.Series["noofleave"].Points.AddXY(mleaveType, mleaveNo);
                        //chrtLeaveTaken.Series["noofleave"].Points.AddXY(mleaveType, mleaveTaken);
                        //chrtLeaveBalance.Series["noofleave"].Points.AddXY(mleaveType, mleaveBalance);
                    }



                    if (LeaveTypes != "")
                    {
                        LeaveTypes = LeaveTypes.Remove(LeaveTypes.Length - 2);
                    }
                    if (EntitleLeaves != "")
                    {
                        EntitleLeaves = EntitleLeaves.Remove(EntitleLeaves.Length - 2);
                    }
                    if (TakenLeaves != "")
                    {
                        TakenLeaves = TakenLeaves.Remove(TakenLeaves.Length - 2);
                    }
                    if (BlanceLeaves != "")
                    {
                        BlanceLeaves = BlanceLeaves.Remove(BlanceLeaves.Length - 2);
                    }

                    LEAVE_DETAILS = leaveDetailsJS(LeaveTypes, EntitleLeaves);
                    LEAVE_TAKEN_DETAILS = leaveTakenJS(LeaveTypes, TakenLeaves);
                    LEAVE_BALANCE_DETAILS = leaveBalanceJS(LeaveTypes, BlanceLeaves);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                leaveScheduleDataHandler = null;
                employeeLeave = new DataTable();
                employeeLeave = null;
            }
        }

        //private void get_LeaveDetails(string empId)
        //{
        //    LeaveScheduleDataHandler leaveScheduleDataHandler = new LeaveScheduleDataHandler();
        //    string sYear = DateTime.Today.ToString("yyyy/MM/dd").Substring(0, 4);

        //    DataTable employeeLeave = null;

        //    try
        //    {
        //        employeeLeave = leaveScheduleDataHandler.getEmployeeLeveSummaryChart(empId, sYear);
        //        if (employeeLeave != null)
        //        {

        //            int icount = employeeLeave.Rows.Count;
        //            string mleaveType = "";
        //            double mleaveNo = 0;
        //            double mleaveTaken = 0;
        //            double mleaveBalance = 0;

        //            for (int i = 0; i <= icount - 1; i++)
        //            {
        //                mleaveType = employeeLeave.Rows[i][0].ToString();
        //                mleaveNo = double.Parse(employeeLeave.Rows[i][1].ToString());
        //                mleaveTaken = double.Parse(employeeLeave.Rows[i][2].ToString());
        //                mleaveBalance = mleaveNo - mleaveTaken;
        //                chrtLeaveDetails.Series["noofleave"].Points.AddXY(mleaveType, mleaveNo);
        //                chrtLeaveTaken.Series["noofleave"].Points.AddXY(mleaveType, mleaveTaken);
        //                chrtLeaveBalance.Series["noofleave"].Points.AddXY(mleaveType, mleaveBalance);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        CommonVariables.MESSAGE_TEXT = ex.Message;
        //        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
        //    }
        //    finally
        //    {
        //        leaveScheduleDataHandler = null;
        //        employeeLeave.Dispose();
        //        employeeLeave = null;

        //    }
        //}

        void loadAlerts()
        {
            trvNotification.Nodes.Clear();

            string EmployeeID = (Session["KeyEMPLOYEE_ID"] as string);
            string RoleID = (Session["KeyHRIS_ROLE"] as string);
            string CompanyID = (Session["KeyCOMP_ID"] as string);

            AlertController AC = new AlertController(EmployeeID, RoleID, CompanyID, trvNotification);

            //Birthday Alerts
            BirthdayOutput = AC.getBirthDayAlerts();

            AC.getLeaveCoveringAlerts();
            AC.getLeaveApproveAlerts();
            AC.getCurrentYearCompanyCalendarMissingAlerts();
            AC.getNextYearCompanyCalendarReminderAlerts();
            AC.getReportTo1InactiveAlerts();
            AC.getReportTo2InactiveAlerts();
            AC.getReportTo3InactiveAlerts();
            AC.getNonRosterEmployeesOnRosterAlerts();
            //NotificationOutput += AC.getLeaveCoveringAlerts();
            //NotificationOutput += AC.getLeaveApproveAlerts();
            //NotificationOutput += AC.getCurrentYearCompanyCalendarMissingAlerts();
            //NotificationOutput += AC.getNeatYearCompanyCalendarReminderAlerts();
            //NotificationOutput += AC.getReportTo1InactiveAlerts();
            //NotificationOutput += AC.getReportTo2InactiveAlerts();
            //NotificationOutput += AC.getReportTo3InactiveAlerts();


            if (trvNotification.Nodes.Count == 0)
            {
                //NotificationOutput = "No item found";
                TreeNode ParentNode = new TreeNode();
                ParentNode.Text = "No item found";
                trvNotification.Nodes.Add(ParentNode);
                ParentNode.Collapse();
            }
        }

       

     }
}