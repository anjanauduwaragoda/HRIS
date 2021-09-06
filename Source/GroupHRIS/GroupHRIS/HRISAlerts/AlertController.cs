using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.HRISAlerts;
using System.Data;
using Common;

namespace GroupHRIS.HRISAlerts
{
    public class AlertController
    {
        string employeeID = String.Empty;
        string roleID = String.Empty;
        string companyID = String.Empty;
        TreeView TrvNotifications = null;
        

        public AlertController(string EmployeeID, string RoleID, string CompanyID, TreeView treeView)
        {
            employeeID = EmployeeID;
            roleID = RoleID;
            companyID = CompanyID;
            TrvNotifications = treeView;
        }

        public string getBirthDayAlerts()
        {
            HRISAlertsDataHandler HADH = new HRISAlertsDataHandler();

            if (HADH.CheckRoleEligibility(Constants.CON_BIRTHDAY_ALERT_ID, roleID) == false)
            {
                return "You do not have sufficient permission to view this page";                
            }
            else
            {
                DataTable dt = new DataTable();
                dt = HADH.BirthdayAlert(companyID).Copy();
                if (dt.Rows.Count > 0)
                {
                    string TITLE = String.Empty;
                    string INITIALS_NAME = String.Empty;
                    string COMPANY = String.Empty;
                    string DEPARTMENT = String.Empty;

                    string birthdayCake = @" <table style='border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000; width:100%;'> <tr> <td style='width:55px;'> <img src='../Images/Alerts/cookie.png' width='50' /> </td> <td> <table> <tr> <td> <b>";
                    string AFTER_TITLE_INITIALS_NAME = @" </b> </td> </tr> <tr> <td>";
                    string AFTER_COMPANY = @" - ";
                    string AFTER_DEPARTMENT = @" </td> </tr> </table> </td> </tr> </table><br/>";

                    string output = String.Empty;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TITLE = dt.Rows[i]["TITLE"].ToString();
                        INITIALS_NAME = dt.Rows[i]["INITIALS_NAME"].ToString();
                        COMPANY = dt.Rows[i]["COMPANY"].ToString();
                        DEPARTMENT = dt.Rows[i]["DEPARTMENT"].ToString();

                        output += birthdayCake + TITLE + " " + INITIALS_NAME + AFTER_TITLE_INITIALS_NAME + COMPANY + AFTER_COMPANY + DEPARTMENT + AFTER_DEPARTMENT;
                    }

                    return output;
                }
                else
                {
                    return "No one's birthday is today.";
                }
            }
        }

        public void getLeaveCoveringAlerts()
        {
            HRISAlertsDataHandler HADH = new HRISAlertsDataHandler();
            string notifications = String.Empty;

            if (HADH.CheckRoleEligibility(Constants.CON_LEAVE_ALERT_ID, roleID) == false)
            {
                //return "";
                return;
            }
            DataTable dt = new DataTable();
            dt = HADH.LeavePendingForCoveringAlert(employeeID).Copy();

            if (dt.Rows.Count == 0)
            {
                //return "";
                return;
            }
            else
            {
                TreeNode ParentNode = new TreeNode();
                ParentNode.Text = "Leave Covering (" + dt.Rows.Count+")";
                TrvNotifications.Nodes.Add(ParentNode);
                ParentNode.Collapse();

                //notifications += "<li><a style='text-decoration: none; color: #FFFFFF' href='../EmployeeLeave/webFrmApproveLeaveSheet.aspx'> <b>Leave Covering</b> </a>";
                //notifications += "<ul>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TreeNode ChlidNode = new TreeNode();
                    ChlidNode.Text = dt.Rows[i]["TITLE"].ToString() + " " + dt.Rows[i]["INITIALS_NAME"].ToString() + " | " + dt.Rows[i]["LEAVE_DATE"].ToString();
                    ChlidNode.NavigateUrl = "../EmployeeLeave/webFrmApproveLeaveSheet.aspx";
                    ParentNode.ChildNodes.Add(ChlidNode);

                    //notifications += "<li>";
                    //notifications += "<a style='text-decoration: none; color: #FFFFFF' href='../EmployeeLeave/webFrmApproveLeaveSheet.aspx'>" + dt.Rows[i]["TITLE"].ToString() + " " + dt.Rows[i]["INITIALS_NAME"].ToString() + " | " + dt.Rows[i]["LEAVE_DATE"].ToString() + "</a>";
                    //notifications += "</li>";
                }
                //notifications += "</ul>";
                //notifications += "</li>";

                //return notifications;
            }
        }

        public void getLeaveApproveAlerts()
        {
            HRISAlertsDataHandler HADH = new HRISAlertsDataHandler();
            string notifications = String.Empty;

            if (HADH.CheckRoleEligibility(Constants.CON_LEAVE_ALERT_ID, roleID) == false)
            {
                //return "";
                return;
            }
            DataTable dt = new DataTable();
            dt = HADH.LeavePendingForRecommendAlert(employeeID).Copy();

            if (dt.Rows.Count == 0)
            {
                //return "";
                return;
            }
            else
            {
                TreeNode ParentNode = new TreeNode();
                ParentNode.Text = "Leave Approve (" + dt.Rows.Count + ")";
                TrvNotifications.Nodes.Add(ParentNode);
                ParentNode.Collapse();

                //notifications += "<li><a style='text-decoration: none; color: #FFFFFF' href='../EmployeeLeave/webFrmApproveLeaveSheet.aspx'> <b>Leave Approve</b> </a>";
                //notifications += "<ul>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TreeNode ChlidNode = new TreeNode();
                    ChlidNode.Text = dt.Rows[i]["TITLE"].ToString() + " " + dt.Rows[i]["INITIALS_NAME"].ToString() + " | " + dt.Rows[i]["LEAVE_DATE"].ToString();
                    ChlidNode.NavigateUrl = "../EmployeeLeave/webFrmApproveLeaveSheet.aspx";
                    ParentNode.ChildNodes.Add(ChlidNode);

                    //notifications += "<li>";
                    //notifications += "<a style='text-decoration: none; color: #FFFFFF' href='../EmployeeLeave/webFrmApproveLeaveSheet.aspx'>" + dt.Rows[i]["TITLE"].ToString() + " " + dt.Rows[i]["INITIALS_NAME"].ToString() + " | " + dt.Rows[i]["LEAVE_DATE"].ToString() + "</a>";
                    //notifications += "</li>";
                }
                //notifications += "</ul>";
                //notifications += "</li>";

                //return notifications;
            }
        }

        public void getCurrentYearCompanyCalendarMissingAlerts()
        {
            HRISAlertsDataHandler HADH = new HRISAlertsDataHandler();
            string notifications = String.Empty;

            if (HADH.CheckRoleEligibility(Constants.CON_COMPANY_CALENDAR_ALERT_ID, roleID) == false)
            {
                //return "";
                return;
            }
            DataTable dt = new DataTable();
            dt = HADH.CurrentYearCompanyCalenderAlert(companyID).Copy();

            if (dt.Rows.Count == 0)
            {
                //return "";
                return;
            }
            else
            {
                TreeNode ParentNode = new TreeNode();
                ParentNode.Text = System.DateTime.Now.Year + " Company Calendar Missing Companies"+" (" + dt.Rows.Count + ")";
                TrvNotifications.Nodes.Add(ParentNode);
                ParentNode.Collapse();

                //notifications += "<li><a style='text-decoration: none; color: #FFFFFF' href='../MetaData/Calendar/WebFrmCalendar.aspx'> <b>" + System.DateTime.Now.Year + " Company Calendar Missing Companies</b> </a>";
                //notifications += "<ul>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TreeNode ChlidNode = new TreeNode();
                    ChlidNode.Text = dt.Rows[i]["COMP_NAME"].ToString();
                    ChlidNode.NavigateUrl = "../MetaData/Calendar/WebFrmCalendar.aspx";
                    ParentNode.ChildNodes.Add(ChlidNode);

                    //notifications += "<li>";
                    //notifications += "<a style='text-decoration: none; color: #FFFFFF' href='../MetaData/Calendar/WebFrmCalendar.aspx'>" + dt.Rows[i]["COMP_NAME"].ToString() + "</a>";
                    //notifications += "</li>";                
                }
                //notifications += "</ul>";
                //notifications += "</li>";

                //return notifications;
            }
        }

        public void getNextYearCompanyCalendarReminderAlerts()
        {
            HRISAlertsDataHandler HADH = new HRISAlertsDataHandler();
            string notifications = String.Empty;

            if (HADH.CheckRoleEligibility(Constants.CON_COMPANY_CALENDAR_ALERT_ID, roleID) == false)
            {
                //return "";
                return;
            }
            DataTable dt = new DataTable();
            dt = HADH.NextYearCompanyCalenderAlert(companyID).Copy();

            if (dt.Rows.Count == 0)
            {
                //return "";
                return;
            }
            else
            {
                TreeNode ParentNode = new TreeNode();
                ParentNode.Text = (Convert.ToInt32(System.DateTime.Now.Year)+1) + " Company Calendar Missing Companies" + " (" + dt.Rows.Count + ")";
                TrvNotifications.Nodes.Add(ParentNode);
                ParentNode.Collapse();

                //notifications += "<li><a style='text-decoration: none; color: #FFFFFF' href='../MetaData/Calendar/WebFrmCalendar.aspx'> <b>" + (Convert.ToInt32(System.DateTime.Now.Year)+1) + " Company Calendar Missing Companies</b> </a>";
                //notifications += "<ul>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TreeNode ChlidNode = new TreeNode();
                    ChlidNode.Text = dt.Rows[i]["COMP_NAME"].ToString();
                    ChlidNode.NavigateUrl = "../MetaData/Calendar/WebFrmCalendar.aspx";
                    ParentNode.ChildNodes.Add(ChlidNode);

                    //notifications += "<li>";
                    //notifications += "<a style='text-decoration: none; color: #FFFFFF' href='../MetaData/Calendar/WebFrmCalendar.aspx'>" + dt.Rows[i]["COMP_NAME"].ToString() + "</a>";
                    //notifications += "</li>";
                }
                //notifications += "</ul>";
                //notifications += "</li>";

                //return notifications;
            }
        }

        public void getReportTo1InactiveAlerts()
        {
            HRISAlertsDataHandler HADH = new HRISAlertsDataHandler();
            string notifications = String.Empty;

            if (HADH.CheckRoleEligibility(Constants.CON_SUPERVISOR_INACTIVE_ALERT_ID, roleID) == false)
            {
                //return "";
                return;
            }
            DataTable dt = new DataTable();
            dt = HADH.ReportTo1InactiveAlert(companyID).Copy();

            if (dt.Rows.Count == 0)
            {
                //return "";
                return;
            }
            else
            {
                TreeNode ParentNode = new TreeNode();
                ParentNode.Text = "Supervisor 1 Inactive Employees";
                TrvNotifications.Nodes.Add(ParentNode);
                ParentNode.Collapse();

                //notifications += "<li> <b> Supervisor 1 Inactive Employees</b> ";
                //notifications += "<ul>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    //notifications += "<li>";
                    //notifications += dt.Rows[i]["TITLE"].ToString() + " " + dt.Rows[i]["INITIALS_NAME"].ToString() + " - " + dt.Rows[i]["COMP_NAME"].ToString();

                    DataTable dtSub = new DataTable();

                    string EmpID = dt.Rows[i]["EMPLOYEE_ID"].ToString();
                    dtSub = HADH.ReportTo1InactiveEmployees(EmpID).Copy();


                    TreeNode Child1 = new TreeNode();
                    Child1.Text = dt.Rows[i]["TITLE"].ToString() + " " + dt.Rows[i]["INITIALS_NAME"].ToString() + " - " + dt.Rows[i]["COMP_NAME"].ToString() + " (" + dtSub.Rows.Count + ")";
                    Child1.Collapse();
                    ParentNode.ChildNodes.Add(Child1);

                    //notifications += "<ul>";
                    for (int j = 0; j < dtSub.Rows.Count; j++)
                    {
                        TreeNode Child2 = new TreeNode();
                        Child2.Text = dtSub.Rows[j]["EPF_NO"].ToString() + " | " + dtSub.Rows[j]["TITLE"].ToString() + " " + dtSub.Rows[j]["INITIALS_NAME"].ToString() + " - " + dtSub.Rows[j]["COMP_NAME"].ToString();
                        Child1.NavigateUrl = "../Employee/webFrmEmployee.aspx";
                        Child1.Collapse();
                        Child1.ChildNodes.Add(Child2);

                        //notifications += "<li>";
                        //notifications += "<a style='text-decoration: none; color: #FFFFFF' href='../Employee/webFrmEmployee.aspx'>" + dtSub.Rows[j]["EPF_NO"].ToString() + " | " + dtSub.Rows[j]["TITLE"].ToString() + " " + dtSub.Rows[j]["INITIALS_NAME"].ToString() + " - " + dtSub.Rows[j]["COMP_NAME"].ToString() + "</a>";
                        //notifications += "</li>";                    
                    }
                    //notifications += "</ul>";

                    //notifications += "</li>";
                }
                //notifications += "</ul>";
                //notifications += "</li>";

                //return notifications;
            }
        }

        public void getReportTo2InactiveAlerts()
        {
            HRISAlertsDataHandler HADH = new HRISAlertsDataHandler();
            string notifications = String.Empty;

            if (HADH.CheckRoleEligibility(Constants.CON_SUPERVISOR_INACTIVE_ALERT_ID, roleID) == false)
            {
                //return "";
                return;
            }
            DataTable dt = new DataTable();
            dt = HADH.ReportTo2InactiveAlert(companyID).Copy();

            if (dt.Rows.Count == 0)
            {
                //return "";
                return;
            }
            else
            {
                TreeNode ParentNode = new TreeNode();
                ParentNode.Text = "Supervisor 2 Inactive Employees";
                TrvNotifications.Nodes.Add(ParentNode);
                ParentNode.Collapse();

                //notifications += "<li> <b> Supervisor 2 Inactive Employees</b> ";
                //notifications += "<ul>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //notifications += "<li>";
                    //notifications += dt.Rows[i]["TITLE"].ToString() + " " + dt.Rows[i]["INITIALS_NAME"].ToString() + " - " + dt.Rows[i]["COMP_NAME"].ToString();

                    DataTable dtSub = new DataTable();

                    string EmpID = dt.Rows[i]["EMPLOYEE_ID"].ToString();
                    dtSub = HADH.ReportTo2InactiveEmployees(EmpID).Copy();


                    TreeNode Child1 = new TreeNode();
                    Child1.Text = dt.Rows[i]["TITLE"].ToString() + " " + dt.Rows[i]["INITIALS_NAME"].ToString() + " - " + dt.Rows[i]["COMP_NAME"].ToString() + " (" + dtSub.Rows.Count + ")";
                    Child1.Collapse();
                    ParentNode.ChildNodes.Add(Child1);

                    //notifications += "<ul>";
                    for (int j = 0; j < dtSub.Rows.Count; j++)
                    {
                        TreeNode Child2 = new TreeNode();
                        Child2.Text = dtSub.Rows[j]["EPF_NO"].ToString() + " | " + dtSub.Rows[j]["TITLE"].ToString() + " " + dtSub.Rows[j]["INITIALS_NAME"].ToString() + " - " + dtSub.Rows[j]["COMP_NAME"].ToString();
                        Child1.NavigateUrl = "../Employee/webFrmEmployee.aspx";
                        Child1.Collapse();
                        Child1.ChildNodes.Add(Child2);

                        //notifications += "<li>";
                        //notifications += "<a style='text-decoration: none; color: #FFFFFF' href='../Employee/webFrmEmployee.aspx'>" + dtSub.Rows[j]["EPF_NO"].ToString() + " | " + dtSub.Rows[j]["TITLE"].ToString() + " " + dtSub.Rows[j]["INITIALS_NAME"].ToString() + " - " + dtSub.Rows[j]["COMP_NAME"].ToString() + "</a>";
                        //notifications += "</li>";
                    }
                    //notifications += "</ul>";

                    //notifications += "</li>";
                }
                //notifications += "</ul>";
                //notifications += "</li>";

                //return notifications;
            }
        }

        public void getReportTo3InactiveAlerts()
        {
            HRISAlertsDataHandler HADH = new HRISAlertsDataHandler();
            string notifications = String.Empty;

            if (HADH.CheckRoleEligibility(Constants.CON_SUPERVISOR_INACTIVE_ALERT_ID, roleID) == false)
            {
                //return "";
                return;
            }
            DataTable dt = new DataTable();
            dt = HADH.ReportTo3InactiveAlert(companyID).Copy();

            if (dt.Rows.Count == 0)
            {
                //return "";
                return;
            }
            else
            {
                TreeNode ParentNode = new TreeNode();
                ParentNode.Text = "Supervisor 3 Inactive Employees";
                TrvNotifications.Nodes.Add(ParentNode);
                ParentNode.Collapse();

                //notifications += "<li> <b> Supervisor 3 Inactive Employees</b> ";
                //notifications += "<ul>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //notifications += "<li>";
                    //notifications += dt.Rows[i]["TITLE"].ToString() + " " + dt.Rows[i]["INITIALS_NAME"].ToString() + " - " + dt.Rows[i]["COMP_NAME"].ToString();

                    DataTable dtSub = new DataTable();

                    string EmpID = dt.Rows[i]["EMPLOYEE_ID"].ToString();
                    dtSub = HADH.ReportTo3InactiveEmployees(EmpID).Copy();


                    TreeNode Child1 = new TreeNode();
                    Child1.Text = dt.Rows[i]["TITLE"].ToString() + " " + dt.Rows[i]["INITIALS_NAME"].ToString() + " - " + dt.Rows[i]["COMP_NAME"].ToString() + " (" + dtSub.Rows.Count + ")";
                    Child1.Collapse();
                    ParentNode.ChildNodes.Add(Child1);

                    //notifications += "<ul>";
                    for (int j = 0; j < dtSub.Rows.Count; j++)
                    {
                        TreeNode Child2 = new TreeNode();
                        Child2.Text = dtSub.Rows[j]["EPF_NO"].ToString() + " | " + dtSub.Rows[j]["TITLE"].ToString() + " " + dtSub.Rows[j]["INITIALS_NAME"].ToString() + " - " + dtSub.Rows[j]["COMP_NAME"].ToString();
                        Child1.NavigateUrl = "../Employee/webFrmEmployee.aspx";
                        Child1.Collapse();
                        Child1.ChildNodes.Add(Child2);

                        //notifications += "<li>";
                        //notifications += "<a style='text-decoration: none; color: #FFFFFF' href='../Employee/webFrmEmployee.aspx'>" + dtSub.Rows[j]["EPF_NO"].ToString() + " | " + dtSub.Rows[j]["TITLE"].ToString() + " " + dtSub.Rows[j]["INITIALS_NAME"].ToString() + " - " + dtSub.Rows[j]["COMP_NAME"].ToString() + "</a>";
                        //notifications += "</li>";
                    }
                    //notifications += "</ul>";

                    //notifications += "</li>";
                }
                //notifications += "</ul>";
                //notifications += "</li>";

                //return notifications;
            }
        }

        public void getNonRosterEmployeesOnRosterAlerts()
        {
            HRISAlertsDataHandler HADH = new HRISAlertsDataHandler();
            string notifications = String.Empty;

            if (HADH.CheckRoleEligibility(Constants.CON_NON_ROSTER_EMPLOYEE_ALERT_ID, roleID) == false)
            {
                return;
            }
            DataTable dt = new DataTable();
            dt = HADH.NonRosterEmployeesOnRoseterHeader(companyID).Copy();

            if (dt.Rows.Count == 0)
            {
                return;
            }
            else
            {
                TreeNode ParentNode = new TreeNode();
                ParentNode.Text = "Non Roster Employees on Rosters";
                TrvNotifications.Nodes.Add(ParentNode);
                ParentNode.Collapse();

                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    DataTable dtSub = new DataTable();

                    string CompID = dt.Rows[a]["COMPANY_ID"].ToString();
                    dtSub = HADH.NonRosterEmployeesOnRoseterNames(CompID).Copy();


                    TreeNode Child1 = new TreeNode();
                    Child1.Text = dt.Rows[a]["COMP_NAME"].ToString() + " (" + dtSub.Rows.Count + ")";
                    Child1.Collapse();
                    ParentNode.ChildNodes.Add(Child1);

                    for (int b = 0; b < dtSub.Rows.Count; b++)
                    {
                        TreeNode Child2 = new TreeNode();

                        string name = String.Empty;

                        if ((dtSub.Rows[b]["TITLE"] as string) != "")
                        {
                            name = dtSub.Rows[b]["TITLE"].ToString() + " " + dtSub.Rows[b]["KNOWN_NAME"].ToString();
                        }
                        else
                        {
                            name = dtSub.Rows[b]["KNOWN_NAME"].ToString();
                        }

                        Child2.Text = name + " - " + dtSub.Rows[b]["DEPT_NAME"].ToString();
                        //Child1.NavigateUrl = "../Employee/webFrmEmployee.aspx";
                        Child1.Collapse();
                        Child1.ChildNodes.Add(Child2);

                        DataTable dtSub1 = new DataTable();
                        string EmployeeID = dtSub.Rows[b]["EMPLOYEE_ID"].ToString();
                        dtSub1 = HADH.NonRosterEmployeesOnRoseterDetails(EmployeeID).Copy();

                        for (int c = 0; c < dtSub1.Rows.Count; c++)
                        {
                            TreeNode Child3 = new TreeNode();
                            Child3.Text = dtSub1.Rows[c]["DUTY_DATE"].ToString() + " | " + dtSub1.Rows[c]["FROM_TIME"].ToString() + " - " + dtSub1.Rows[c]["TO_TIME"].ToString();
                            //Child1.NavigateUrl = "../Employee/webFrmEmployee.aspx";
                            Child2.Collapse();
                            Child2.ChildNodes.Add(Child3);
                        }

                    }
                }














                //TreeNode ParentNode = new TreeNode();
                //ParentNode.Text = "Non Roster Employees on Rosters";
                //TrvNotifications.Nodes.Add(ParentNode);
                //ParentNode.Collapse();

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    //DataTable dtSub = new DataTable();

                //    //string CompID = dt.Rows[i]["COMPANY_ID"].ToString();
                //    //dtSub = HADH.NonRosterEmployeesOnRoseterDetails(CompID).Copy();


                //    //TreeNode Child1 = new TreeNode();
                //    //Child1.Text = dt.Rows[i]["COMP_NAME"].ToString() + " (" + dtSub.Rows.Count + ")";
                //    //Child1.Collapse();
                //    //ParentNode.ChildNodes.Add(Child1);

                //    //for (int j = 0; j < dtSub.Rows.Count; j++)
                //    //{
                //    //    TreeNode Child2 = new TreeNode();

                //    //    string name = String.Empty;

                //    //    if ((dtSub.Rows[j]["TITLE"] as string) != "")
                //    //    {
                //    //        name = dtSub.Rows[j]["TITLE"].ToString() + " " + dtSub.Rows[j]["KNOWN_NAME"].ToString();
                //    //    }
                //    //    else
                //    //    {
                //    //        name = dtSub.Rows[j]["KNOWN_NAME"].ToString();
                //    //    }

                //    //    Child2.Text = name + " - " + dtSub.Rows[j]["DEPT_NAME"].ToString() + " - " + dtSub.Rows[j]["DUTY_DATE"].ToString() + " | " + dtSub.Rows[j]["FROM_TIME"].ToString() + " - " + dtSub.Rows[j]["TO_TIME"].ToString();
                //    //    //Child1.NavigateUrl = "../Employee/webFrmEmployee.aspx";
                //    //    Child1.Collapse();
                //    //    Child1.ChildNodes.Add(Child2);
                //    //}
                //}
            }
        }
    }
}