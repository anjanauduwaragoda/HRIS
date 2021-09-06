using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.PerformanceManagement;
using System.Data;
using DataHandler.Userlogin;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmCEODashboard : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    log.Debug("IP:" + Request.UserHostAddress + "WebFrmCEODashboard : Page_Load");

                    hfSupervisorID.Value = (Session["KeyEMPLOYEE_ID"] as string).Trim();
                    SetFinancialYear();
                    LoadAssessmentYears();
                    LoadStatus(ddlAssessmentStatus);
                    LoadDepartments(ddlDepartment);
                    //LoadAssessments(ddlAssessment);
                    //LoadGrid();

                    Session["DASHBOARD"] = "CEO";//For indentify the performance  evaluation user view

                    if (grdvAssessment.Rows.Count > 0)
                    {
                        imgRefresh.Visible = true;
                    }
                    else
                    {
                        imgRefresh.Visible = false;
                    }
                }
                catch (Exception exp)
                {
                    CommonVariables.MESSAGE_TEXT = exp.Message;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
                }
                finally
                {

                }
            }
        }

        protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                log.Debug("WebFrmCEODashboard : imgbtnSearch_Click()");
                if (ddlAssessment.Items.Count > 0)
                {
                    if (ddlAssessment.SelectedIndex > 0)
                    {
                        LoadGrid();
                        if (grdvAssessment.Rows.Count > 0)
                        {
                            imgRefresh.Visible = true;
                        }
                        else
                        {
                            imgRefresh.Visible = false;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
        }

        protected void grdvAssessment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                log.Debug("WebFrmCEODashboard : grdvAssessment_PageIndexChanging()");

                grdvAssessment.PageIndex = e.NewPageIndex;
                LoadGrid();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        protected void ddlYearOfAssessment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStatus(ddlAssessmentStatus);
            LoadAssessments(ddlAssessment);
        }

        protected void ddlAssessmentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAssessments(ddlAssessment);
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDivisions(ddlDivision);
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        #endregion

        #region Methods

        void SetFinancialYear()
        {
            try
            {
                System.DateTime dtfin = System.DateTime.Now;

                int CurrentFinyear = 0;

                DateTime finDate = DateTime.ParseExact(dtfin.Year + "-04-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                if (finDate > System.DateTime.Now)
                {
                    CurrentFinyear = dtfin.AddYears(-1).Year;
                    hfFinancialYearDetails.Value = " (From April 1, " + CurrentFinyear.ToString() + " To March 31, " + finDate.Year + ")";
                    hfFinancialYear.Value = CurrentFinyear.ToString();
                }
                else
                {
                    System.DateTime dt = System.DateTime.Now;
                    System.DateTime dtDetais = System.DateTime.Now;
                    hfFinancialYearDetails.Value = " (From April 1, " + dt.Year.ToString() + " To March 31, " + dtDetais.AddYears(1).Year + ")";
                    hfFinancialYear.Value = dt.Year.ToString();
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        void LoadAssessmentYears()
        {
            CEODashboadDataHandler CEODDH = new CEODashboadDataHandler();
            DataTable dtYearOfAssessment = new DataTable();
            try
            {
                dtYearOfAssessment = CEODDH.PopulateYears().Copy();
                for (int i = 0; i < dtYearOfAssessment.Rows.Count; i++)
                {
                    string Year = dtYearOfAssessment.Rows[i]["YEAR_OF_ASSESSMENT"].ToString();
                    ddlYearOfAssessment.Items.Add(new ListItem(Year, Year));
                }

                string CurrentYear = hfFinancialYear.Value;
                DataRow[] drCurrentYear = dtYearOfAssessment.Select("YEAR_OF_ASSESSMENT = '" + CurrentYear + "'");
                if (drCurrentYear.Length > 0)
                {
                    ddlYearOfAssessment.SelectedIndex = ddlYearOfAssessment.Items.IndexOf(ddlYearOfAssessment.Items.FindByValue(CurrentYear));
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                CEODDH = null;
                dtYearOfAssessment.Dispose();
            }
        }

        void LoadStatus(DropDownList StatusList)
        {
            try
            {
                StatusList.Items.Clear();
                StatusList.Items.Add(new ListItem("", ""));
                StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_ACTIVE_TAG, Constants.ASSESSNEMT_ACTIVE_STATUS));
                StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_OBSOLETE_TAG, Constants.ASSESSNEMT_OBSOLETE_STATUS));
                StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_PENDING_TAG, Constants.ASSESSNEMT_PENDING_STATUS));
                //StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG, Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS));
                //StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG, Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS));
                //StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG, Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS));
                //StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_CEO_FINALIZED_TAG, Constants.ASSESSNEMT_CEO_FINALIZED_STATUS));
                StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_CLOSED_TAG, Constants.ASSESSNEMT_CLOSED_STATUS));
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
        }

        void LoadAssessments(DropDownList AssessmentList)
        {
            CEODashboadDataHandler CEODDH = new CEODashboadDataHandler();
            DataTable dtAssessments = new DataTable();
            try
            {
                string user = (Session["KeyEMPLOYEE_ID"] as string).Trim();
                AssessmentList.Items.Clear();
                dtAssessments = CEODDH.PopulateAssessments(ddlYearOfAssessment.SelectedValue.ToString().Trim(), ddlAssessmentStatus.SelectedValue.ToString(), user).Copy();
                AssessmentList.Items.Add(new ListItem("", ""));
                for (int i = 0; i < dtAssessments.Rows.Count; i++)
                {
                    string Text = dtAssessments.Rows[i]["ASSESSMENT_NAME"].ToString();
                    string Value = dtAssessments.Rows[i]["ASSESSMENT_ID"].ToString();
                    AssessmentList.Items.Add(new ListItem(Text, Value));
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                CEODDH = null;
                dtAssessments.Dispose();
            }
        }

        string CEOWorkingCompany()
        {
            CEODashboadDataHandler CDDH = new CEODashboadDataHandler();
            string company = String.Empty;
            string supervisorID = String.Empty;
            try
            {
                supervisorID = (Session["KeyEMPLOYEE_ID"] as string);
                company = CDDH.GetSupervisorWorkingCompany(supervisorID).Trim();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                CDDH = null;
                supervisorID = null;
            }
            return company;
        }

        void LoadDepartments(DropDownList DepartmentList)
        {
            CEODashboadDataHandler CEODDH = new CEODashboadDataHandler();
            DataTable dtAssessments = new DataTable();
            try
            {
                string user = (Session["KeyEMPLOYEE_ID"] as string).Trim();
                DepartmentList.Items.Clear();
                dtAssessments = CEODDH.PopulateDepartments(CEOWorkingCompany()).Copy();
                DepartmentList.Items.Add(new ListItem("", ""));
                for (int i = 0; i < dtAssessments.Rows.Count; i++)
                {
                    string Text = dtAssessments.Rows[i]["DEPT_NAME"].ToString();
                    string Value = dtAssessments.Rows[i]["DEPT_ID"].ToString();
                    DepartmentList.Items.Add(new ListItem(Text, Value));
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                CEODDH = null;
                dtAssessments.Dispose();
            }
        }

        void LoadDivisions(DropDownList DivisionList)
        {
            CEODashboadDataHandler CEODDH = new CEODashboadDataHandler();
            DataTable dtAssessments = new DataTable();
            try
            {
                string user = (Session["KeyEMPLOYEE_ID"] as string).Trim();
                DivisionList.Items.Clear();
                dtAssessments = CEODDH.PopulateDivisions(ddlDepartment.SelectedValue).Copy();
                DivisionList.Items.Add(new ListItem("", ""));
                for (int i = 0; i < dtAssessments.Rows.Count; i++)
                {
                    string Text = dtAssessments.Rows[i]["DIV_NAME"].ToString();
                    string Value = dtAssessments.Rows[i]["DIVISION_ID"].ToString();
                    DivisionList.Items.Add(new ListItem(Text, Value));
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                CEODDH = null;
                dtAssessments.Dispose();
            }
        }

        void LoadGrid()
        {
            CEODashboadDataHandler CEODDH = new CEODashboadDataHandler();
            DataTable dtAssessedEmployeeList = new DataTable();
            PasswordHandler crpto;

            try
            {
                string Department = String.Empty;
                string Division = String.Empty;

                if (ddlDepartment.Items.Count > 0)
                {
                    Department = ddlDepartment.SelectedValue;
                }
                if (ddlDivision.Items.Count > 0)
                {
                    Division = ddlDivision.SelectedValue;
                }

                string CEOEmpID = (Session["KeyEMPLOYEE_ID"] as string);
                dtAssessedEmployeeList = CEODDH.Populate(ddlYearOfAssessment.SelectedValue, ddlAssessmentStatus.SelectedValue, ddlAssessment.SelectedValue, CEOWorkingCompany(), Department, Division, CEOEmpID).Copy();
                //dtAssessedEmployeeList = CEODDH.Populate(ddlYearOfAssessment.SelectedValue, ddlAssessmentStatus.SelectedValue, ddlAssessment.SelectedValue, (Session["KeyEMPLOYEE_ID"] as string), Department, Division).Copy();

                crpto = new PasswordHandler();


                DataTable dtCharts = new DataTable();
                dtCharts = dtAssessedEmployeeList.Copy();

                for (int i = 0; i < dtAssessedEmployeeList.Rows.Count; i++)
                {
                    string Title = dtAssessedEmployeeList.Rows[i]["TITLE"].ToString();
                    string NameWithInitials = dtAssessedEmployeeList.Rows[i]["INITIALS_NAME"].ToString();
                    dtAssessedEmployeeList.Rows[i]["INITIALS_NAME"] = Title + " " + NameWithInitials;

                    string isGoalAssessmentInclude = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["INCLUDE_GOAL_ASSESSMENT"].ToString());
                    string GoalAssessmentStatus = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["GOAL_ASSESSMENT_STATUS"].ToString());
                    string isCompetencyAssessmentInclude = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["INCLUDE_COMPITANCY_ASSESSMENT"].ToString());
                    string CompetencyAssessmentStatus = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["COMPETENCY_ASSESSMENT_STATUS"].ToString());
                    string isSelfAssessmentInclude = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["INCLUDE_SELF_ASSESSMENT"].ToString());
                    string SelfAssessmentStatus = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["SELF_ASSESSMENT_STATUS"].ToString());

                    string AssessmentID = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["ASSESSMENT_ID"].ToString());
                    string SubordinateID = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["EMPLOYEE_ID"].ToString());
                    string YearOfAssessment = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["YEAR_OF_ASSESSMENT"].ToString());

                    string Evaluation = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["STATUS_CODE"].ToString());


                    Session["isGoalAssessmentInclude"] = isGoalAssessmentInclude;
                    Session["isCompetencyAssessmentInclude"] = isCompetencyAssessmentInclude;


                    //Set Evaluation Status
                    if (isSelfAssessmentInclude == Constants.STATUS_ACTIVE_VALUE)
                    {
                        Evaluation = PerformanceEvaluationStatus(GoalAssessmentStatus, CompetencyAssessmentStatus, SelfAssessmentStatus, Evaluation, true);

                        //SetSupervisorStatus
                        DataRow dr = dtAssessedEmployeeList.NewRow();
                        dr = dtAssessedEmployeeList.Rows[i];

                        for (int j = 0; j < dtAssessedEmployeeList.Rows[i].ItemArray.Length; j++)
                        {
                            dtAssessedEmployeeList.Rows[i][j] = SetCEOStatus(GoalAssessmentStatus, CompetencyAssessmentStatus, SelfAssessmentStatus, Evaluation, dr, true)[j];

                            string ChartCompetencyStatus = (Session["ChartCompetencyStatus"] as string);
                            string ChartGoalStatus = (Session["ChartGoalStatus"] as string);
                            string ChartSelfStatus = (Session["ChartSelfStatus"] as string);
                            string ChartPerformanceStatus = (Session["ChartPerformanceStatus"] as string);

                            dtCharts.Rows[i]["COMPETENCY_ASSESSMENT_STATUS"] = ChartCompetencyStatus;
                            dtCharts.Rows[i]["GOAL_ASSESSMENT_STATUS"] = ChartGoalStatus;
                            dtCharts.Rows[i]["SELF_ASSESSMENT_STATUS"] = ChartSelfStatus;
                            dtCharts.Rows[i]["STATUS_CODE"] = ChartPerformanceStatus;
                        }
                    }
                    else
                    {
                        Evaluation = PerformanceEvaluationStatus(GoalAssessmentStatus, CompetencyAssessmentStatus, SelfAssessmentStatus, Evaluation, false);

                        //SetSupervisorStatus
                        DataRow dr = dtAssessedEmployeeList.NewRow();
                        dr = dtAssessedEmployeeList.Rows[i];

                        for (int j = 0; j < dtAssessedEmployeeList.Rows[i].ItemArray.Length; j++)
                        {
                            dtAssessedEmployeeList.Rows[i][j] = SetCEOStatus(GoalAssessmentStatus, CompetencyAssessmentStatus, SelfAssessmentStatus, Evaluation, dr, false)[j];

                            string ChartCompetencyStatus = (Session["ChartCompetencyStatus"] as string);
                            string ChartGoalStatus = (Session["ChartGoalStatus"] as string);
                            string ChartSelfStatus = (Session["ChartSelfStatus"] as string);
                            string ChartPerformanceStatus = (Session["ChartPerformanceStatus"] as string);

                            dtCharts.Rows[i]["COMPETENCY_ASSESSMENT_STATUS"] = ChartCompetencyStatus;
                            dtCharts.Rows[i]["GOAL_ASSESSMENT_STATUS"] = ChartGoalStatus;
                            dtCharts.Rows[i]["SELF_ASSESSMENT_STATUS"] = ChartSelfStatus;
                            dtCharts.Rows[i]["STATUS_CODE"] = ChartPerformanceStatus;
                        }
                    }



                }


                Session["dtDisplayDataList"] = dtCharts.Copy();
                DisplayCharts();

                grdvAssessment.DataSource = dtAssessedEmployeeList;
                grdvAssessment.DataBind();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                dtAssessedEmployeeList.Dispose();
                CEODDH = null;
                crpto = null;
            }
        }

        DataRow SetCEOStatus(string GoalAssessmentStatus, string CompetencyAssessmentStatus, string SelfAssessmentStatus, string EvaluationStatus, DataRow drAssessment, Boolean isSelfAssessmentIncluded)
        {
            PasswordHandler crpto;
            try
            {
                string ChartCompetencyStatus = String.Empty;
                string ChartGoalStatus = String.Empty;
                string ChartSelfStatus = String.Empty;
                string ChartPerformanceStatus = String.Empty;

                log.Debug("SetCEOStatus");
                crpto = new PasswordHandler();

                string AssessmentID = HttpUtility.HtmlDecode(drAssessment["ASSESSMENT_ID"].ToString());
                string SubordinateID = HttpUtility.HtmlDecode(drAssessment["EMPLOYEE_ID"].ToString());
                string YearOfAssessment = HttpUtility.HtmlDecode(drAssessment["YEAR_OF_ASSESSMENT"].ToString());
                string IsSubordinateAgreed = HttpUtility.HtmlDecode(drAssessment["IS_EMPLOYEE_AGREED"].ToString());//For performance Evaluation

                //Set Goal Assessment Status
                if ((Session["isGoalAssessmentInclude"] as string) == Constants.CON_ACTIVE_STATUS)
                {
                    if ((GoalAssessmentStatus == "") || (GoalAssessmentStatus == Constants.ASSESSNEMT_PENDING_STATUS) || (GoalAssessmentStatus == Constants.ASSESSNEMT_ACTIVE_STATUS))
                    {
                        drAssessment["GOAL_ASSESSMENT_STATUS"] = Constants.ASSESSNEMT_PENDING_TAG;
                        ChartGoalStatus = Constants.ASSESSNEMT_PENDING_STATUS;
                    }
                    else if (GoalAssessmentStatus == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        drAssessment["GOAL_ASSESSMENT_STATUS"] = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG;
                        ChartGoalStatus = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS;
                    }
                    else if (GoalAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        drAssessment["GOAL_ASSESSMENT_STATUS"] = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG;
                        ChartGoalStatus = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;
                    }
                    else if (GoalAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmCEOGoalAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG + @"</a>";
                        drAssessment["GOAL_ASSESSMENT_STATUS"] = hyperlink;
                        ChartGoalStatus = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS;
                    }
                    else if (GoalAssessmentStatus == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmCEOGoalAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CEO_FINALIZED_TAG + @"</a>";
                        drAssessment["GOAL_ASSESSMENT_STATUS"] = hyperlink;
                        ChartGoalStatus = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                    }
                    else if (GoalAssessmentStatus == Constants.ASSESSNEMT_CLOSED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmCEOGoalAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CLOSED_TAG + @"</a>";
                        drAssessment["GOAL_ASSESSMENT_STATUS"] = hyperlink;
                        ChartGoalStatus = Constants.ASSESSNEMT_CLOSED_STATUS;
                    }
                    else if (GoalAssessmentStatus == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmCEOGoalAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_OBSOLETE_TAG + @"</a>";
                        drAssessment["GOAL_ASSESSMENT_STATUS"] = hyperlink;
                        ChartGoalStatus = Constants.ASSESSNEMT_OBSOLETE_STATUS;
                    }
                }
                else
                {
                    drAssessment["GOAL_ASSESSMENT_STATUS"] = "N/A";
                    ChartGoalStatus = "N/A";
                }
                //--

                //Set Competency Assessment Status


                if ((Session["isCompetencyAssessmentInclude"] as string) == Constants.CON_ACTIVE_STATUS)
                {

                    if ((CompetencyAssessmentStatus == "") || (CompetencyAssessmentStatus == Constants.ASSESSNEMT_PENDING_STATUS) || (CompetencyAssessmentStatus == Constants.ASSESSNEMT_ACTIVE_STATUS))
                    {
                        drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = Constants.ASSESSNEMT_PENDING_TAG;
                        ChartCompetencyStatus = Constants.ASSESSNEMT_PENDING_STATUS;
                    }
                    else if (CompetencyAssessmentStatus == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG;
                        ChartCompetencyStatus = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS;
                    }
                    else if (CompetencyAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG;
                        ChartCompetencyStatus = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;
                    }
                    else if (CompetencyAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmCEOCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG + @"</a>";
                        drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = hyperlink;
                        ChartCompetencyStatus = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS;
                    }
                    else if (CompetencyAssessmentStatus == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmCEOCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CEO_FINALIZED_TAG + @"</a>";
                        drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = hyperlink;
                        ChartCompetencyStatus = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                    }
                    else if (CompetencyAssessmentStatus == Constants.ASSESSNEMT_CLOSED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmCEOCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CLOSED_TAG + @"</a>";
                        drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = hyperlink;
                        ChartCompetencyStatus = Constants.ASSESSNEMT_CLOSED_STATUS;
                    }
                    else if (CompetencyAssessmentStatus == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmCEOCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_OBSOLETE_TAG + @"</a>";
                        drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = hyperlink;
                        ChartCompetencyStatus = Constants.ASSESSNEMT_OBSOLETE_STATUS;
                    }
                }
                else
                {
                    drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = "N/A";
                    ChartCompetencyStatus = "N/A";
                }
                //--

                //Set Self Assessment Status
                if (isSelfAssessmentIncluded)
                {
                    if ((SelfAssessmentStatus == "") || (SelfAssessmentStatus == Constants.ASSESSNEMT_PENDING_STATUS) || (SelfAssessmentStatus == Constants.ASSESSNEMT_ACTIVE_STATUS))
                    {
                        drAssessment["SELF_ASSESSMENT_STATUS"] = Constants.ASSESSNEMT_PENDING_TAG;
                        ChartSelfStatus = Constants.ASSESSNEMT_PENDING_STATUS;
                    }
                    else if (SelfAssessmentStatus == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        drAssessment["SELF_ASSESSMENT_STATUS"] = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG;
                        ChartSelfStatus = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS;
                    }
                    else if (SelfAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        drAssessment["SELF_ASSESSMENT_STATUS"] = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG;
                        ChartSelfStatus = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;
                    }
                    else if (SelfAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmCEOSelfAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG + @"</a>";
                        drAssessment["SELF_ASSESSMENT_STATUS"] = hyperlink;
                        ChartSelfStatus = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS;
                    }
                    else if (SelfAssessmentStatus == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmCEOSelfAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CEO_FINALIZED_TAG + @"</a>";
                        drAssessment["SELF_ASSESSMENT_STATUS"] = hyperlink;
                        ChartSelfStatus = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                    }
                    else if (SelfAssessmentStatus == Constants.ASSESSNEMT_CLOSED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmCEOSelfAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CLOSED_TAG + @"</a>";
                        drAssessment["SELF_ASSESSMENT_STATUS"] = hyperlink;
                        ChartSelfStatus = Constants.ASSESSNEMT_CLOSED_STATUS;
                    }
                    else if (SelfAssessmentStatus == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmCEOSelfAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_OBSOLETE_TAG + @"</a>";
                        drAssessment["SELF_ASSESSMENT_STATUS"] = hyperlink;
                        ChartSelfStatus = Constants.ASSESSNEMT_OBSOLETE_STATUS;
                    }
                }
                else
                {
                    drAssessment["SELF_ASSESSMENT_STATUS"] = "N/A";
                    ChartSelfStatus = "N/A";
                }
                //--

                //Set Performance Evaluation Status
                if (EvaluationStatus == Constants.ASSESSNEMT_PENDING_STATUS)
                {
                    drAssessment["STATUS_CODE"] = Constants.ASSESSNEMT_PENDING_TAG;
                    ChartPerformanceStatus = Constants.ASSESSNEMT_PENDING_STATUS;
                }
                else if (EvaluationStatus == Constants.ASSESSNEMT_ACTIVE_STATUS)
                {
                    string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                    string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_ACTIVE_TAG + @"</a>";
                    drAssessment["STATUS_CODE"] = hyperlink;
                    ChartPerformanceStatus = Constants.ASSESSNEMT_ACTIVE_STATUS;
                }
                else if (EvaluationStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                {
                    //string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                    //string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG + @"</a>";
                    //drAssessment["STATUS_CODE"] = hyperlink;
                    drAssessment["STATUS_CODE"] = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG;
                    ChartPerformanceStatus = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;
                }
                else if (EvaluationStatus == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                {
                    string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                    string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG + @"</a>";
                    drAssessment["STATUS_CODE"] = hyperlink;
                    ChartPerformanceStatus = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS;
                }
                else if (EvaluationStatus == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                {
                    string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                    string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CEO_FINALIZED_TAG + @"</a>";
                    drAssessment["STATUS_CODE"] = hyperlink;
                    ChartPerformanceStatus = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                }
                //else if (EvaluationStatus == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS)
                //{
                //    string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                //    string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG + @"</a>";
                //    drAssessment["STATUS_CODE"] = hyperlink;
                //}
                else if (EvaluationStatus == Constants.ASSESSNEMT_CLOSED_STATUS)
                {
                    string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                    string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CLOSED_TAG + @"</a>";
                    drAssessment["STATUS_CODE"] = hyperlink;
                    ChartPerformanceStatus = Constants.ASSESSNEMT_CLOSED_STATUS;
                }
                else if (EvaluationStatus == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                {
                    string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                    string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_OBSOLETE_TAG + @"</a>";
                    drAssessment["STATUS_CODE"] = hyperlink;
                    ChartPerformanceStatus = Constants.ASSESSNEMT_OBSOLETE_STATUS;
                }

                if ((IsSubordinateAgreed == Constants.CON_ACTIVE_STATUS) && (EvaluationStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS))
                {
                    //string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                    //string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG + @"</a>";
                    drAssessment["STATUS_CODE"] = Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG;
                    ChartPerformanceStatus = Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG;
                }
                else if ((IsSubordinateAgreed == Constants.CON_INACTIVE_STATUS) && (EvaluationStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS))
                {
                    //string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                    //string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG + @"</a>";
                    drAssessment["STATUS_CODE"] = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
                    ChartPerformanceStatus = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
                }
                //--
                Session["ChartCompetencyStatus"] = ChartCompetencyStatus;
                Session["ChartGoalStatus"] = ChartGoalStatus;
                Session["ChartSelfStatus"] = ChartSelfStatus;
                Session["ChartPerformanceStatus"] = ChartPerformanceStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                crpto = null;
            }
            return drAssessment;
        }

        string PerformanceEvaluationStatus(string GoalAssessmentStatus, string CompetencyAssessmentStatus, string SelfAssessmentStatus, string EvaluationStatus, Boolean isSelfAssessmentInclude)
        {
            string Status = String.Empty;
            try
            {
                if (EvaluationStatus == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                {
                    Status = Constants.ASSESSNEMT_ACTIVE_STATUS;
                    return Status;
                }

                if (EvaluationStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                {
                    Status = EvaluationStatus;
                    return Status;
                }



                if (isSelfAssessmentInclude)
                {
                    if (EvaluationStatus == Constants.ASSESSNEMT_CLOSED_STATUS)
                    {
                        Status = Constants.ASSESSNEMT_CLOSED_STATUS;
                        return Status;
                    }
                    if (EvaluationStatus == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    {
                        Status = Constants.ASSESSNEMT_OBSOLETE_STATUS;
                        return Status;
                    }

                    string pendigSet = Constants.ASSESSNEMT_PENDING_STATUS + Constants.ASSESSNEMT_ACTIVE_STATUS + Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS + Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS + Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS;

                    if (GoalAssessmentStatus != String.Empty)
                    {
                        if (pendigSet.Contains(Convert.ToChar(GoalAssessmentStatus)))
                        {
                            Status = Constants.ASSESSNEMT_PENDING_STATUS;
                            return Status;
                        }
                    }
                    else
                    {
                        Status = Constants.ASSESSNEMT_PENDING_STATUS;
                        return Status;
                    }

                    if (CompetencyAssessmentStatus != String.Empty)
                    {
                        if (pendigSet.Contains(Convert.ToChar(CompetencyAssessmentStatus)))
                        {
                            Status = Constants.ASSESSNEMT_PENDING_STATUS;
                            return Status;
                        }
                    }
                    else
                    {
                        Status = Constants.ASSESSNEMT_PENDING_STATUS;
                        return Status;
                    }

                    if (SelfAssessmentStatus != String.Empty)
                    {
                        if (pendigSet.Contains(Convert.ToChar(SelfAssessmentStatus)))
                        {
                            Status = Constants.ASSESSNEMT_PENDING_STATUS;
                            return Status;
                        }
                    }
                    else
                    {
                        Status = Constants.ASSESSNEMT_PENDING_STATUS;
                        return Status;
                    }

                    if ((GoalAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS) && (GoalAssessmentStatus == CompetencyAssessmentStatus) && (CompetencyAssessmentStatus == SelfAssessmentStatus))
                    {
                        Status = Constants.ASSESSNEMT_ACTIVE_STATUS;
                        return Status;
                    }

                    if ((GoalAssessmentStatus == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS) && (GoalAssessmentStatus == CompetencyAssessmentStatus) && (CompetencyAssessmentStatus == SelfAssessmentStatus))
                    {
                        Status = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                        return Status;
                    }




                    string CEOActiveSet = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS + Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;

                    if (GoalAssessmentStatus != String.Empty)
                    {
                        if (CEOActiveSet.Contains(Convert.ToChar(GoalAssessmentStatus)))
                        {
                            Status = Constants.ASSESSNEMT_ACTIVE_STATUS;
                            return Status;
                        }
                    }
                    else
                    {
                        Status = Constants.ASSESSNEMT_PENDING_STATUS;
                        return Status;
                    }

                    if (CompetencyAssessmentStatus != String.Empty)
                    {
                        if (CEOActiveSet.Contains(Convert.ToChar(CompetencyAssessmentStatus)))
                        {
                            Status = Constants.ASSESSNEMT_ACTIVE_STATUS;
                            return Status;
                        }
                    }
                    else
                    {
                        Status = Constants.ASSESSNEMT_PENDING_STATUS;
                        return Status;
                    }

                    if (SelfAssessmentStatus != String.Empty)
                    {
                        if (CEOActiveSet.Contains(Convert.ToChar(SelfAssessmentStatus)))
                        {
                            Status = Constants.ASSESSNEMT_ACTIVE_STATUS;
                            return Status;
                        }
                    }
                    else
                    {
                        Status = Constants.ASSESSNEMT_PENDING_STATUS;
                        return Status;
                    }
                }
                else
                {
                    if (EvaluationStatus == Constants.ASSESSNEMT_CLOSED_STATUS)
                    {
                        Status = Constants.ASSESSNEMT_CLOSED_STATUS;
                        return Status;
                    }
                    if (EvaluationStatus == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    {
                        Status = Constants.ASSESSNEMT_OBSOLETE_STATUS;
                        return Status;
                    }

                    string pendigSet = Constants.ASSESSNEMT_PENDING_STATUS + Constants.ASSESSNEMT_ACTIVE_STATUS + Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS + Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;

                    if (GoalAssessmentStatus != String.Empty)
                    {
                        if (pendigSet.Contains(Convert.ToChar(GoalAssessmentStatus)))
                        {
                            Status = Constants.ASSESSNEMT_PENDING_STATUS;
                            return Status;
                        }
                    }
                    else
                    {
                        Status = Constants.ASSESSNEMT_PENDING_STATUS;
                        return Status;
                    }

                    if (CompetencyAssessmentStatus != String.Empty)
                    {
                        if (pendigSet.Contains(Convert.ToChar(CompetencyAssessmentStatus)))
                        {
                            Status = Constants.ASSESSNEMT_PENDING_STATUS;
                            return Status;
                        }
                    }
                    else
                    {
                        Status = Constants.ASSESSNEMT_PENDING_STATUS;
                        return Status;
                    }

                    if ((GoalAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS) && (GoalAssessmentStatus == CompetencyAssessmentStatus))
                    {
                        Status = Constants.ASSESSNEMT_ACTIVE_STATUS;
                        return Status;
                    }

                    if ((GoalAssessmentStatus == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS) && (GoalAssessmentStatus == CompetencyAssessmentStatus))
                    {
                        Status = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                        return Status;
                    }




                    string CEOActiveSet = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS + Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;

                    if (GoalAssessmentStatus != String.Empty)
                    {
                        if (CEOActiveSet.Contains(Convert.ToChar(GoalAssessmentStatus)))
                        {
                            Status = Constants.ASSESSNEMT_ACTIVE_STATUS;
                            return Status;
                        }
                    }
                    else
                    {
                        Status = Constants.ASSESSNEMT_PENDING_STATUS;
                        return Status;
                    }

                    if (CompetencyAssessmentStatus != String.Empty)
                    {
                        if (CEOActiveSet.Contains(Convert.ToChar(CompetencyAssessmentStatus)))
                        {
                            Status = Constants.ASSESSNEMT_ACTIVE_STATUS;
                            return Status;
                        }
                    }
                    else
                    {
                        Status = Constants.ASSESSNEMT_PENDING_STATUS;
                        return Status;
                    }
                }
                
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                
            }
            return Status;
        }

        string JSFunction()
        {
            string JS = String.Empty;
            try
            {
                JS = @"<script> window.onload = function() {
                                                var ctx = document.getElementById('chart-area').getContext('2d');
                                                window.myDoughnut = new Chart(ctx, config);
                                                var ctx2 = document.getElementById('chart-area2').getContext('2d');
                                                window.myDoughnut2 = new Chart(ctx2, config2);
                                                var ctx3 = document.getElementById('chart-area3').getContext('2d');
                                                window.myDoughnut = new Chart(ctx3, config3);
                                                var ctx4 = document.getElementById('chart-area4').getContext('2d');
                                                window.myDoughnut = new Chart(ctx4, config4);
                                            }; 
                                            function DisplayCharts() {
                                                var ctx = document.getElementById('chart-area').getContext('2d');
                                                window.myDoughnut = new Chart(ctx, config);
                                                var ctx2 = document.getElementById('chart-area2').getContext('2d');
                                                window.myDoughnut2 = new Chart(ctx2, config2);
                                                var ctx3 = document.getElementById('chart-area3').getContext('2d');
                                                window.myDoughnut = new Chart(ctx3, config3);
                                                var ctx4 = document.getElementById('chart-area4').getContext('2d');
                                                window.myDoughnut = new Chart(ctx4, config4);              
                                            }

                                </script>";
                return JS;

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
            return JS;
        }

        string loadGoalChart(DataTable dtGoalAssessmentDetails)
        {
            SupervisorDashboardDataHandler SDDH = new SupervisorDashboardDataHandler();
            DataTable CompetencyAssessment = new DataTable();
            string GoalChartJS = String.Empty;
            try
            {

                CompetencyAssessment.Columns.Add("STATUS_TEXT");
                CompetencyAssessment.Columns.Add("STATUS_VALUE");

                for (int i = 0; i < dtGoalAssessmentDetails.Rows.Count; i++)
                {
                    string StatusValue = dtGoalAssessmentDetails.Rows[i]["GOAL_ASSESSMENT_STATUS"].ToString().Trim();
                    string StatusText = String.Empty;

                    if ((StatusValue == "") || (StatusValue == Constants.ASSESSNEMT_PENDING_STATUS))
                    {
                        if ((StatusValue == "") && (Constants.STATUS_INACTIVE_VALUE == dtGoalAssessmentDetails.Rows[i]["INCLUDE_GOAL_ASSESSMENT"].ToString().Trim()))
                        {
                            StatusText = "N/A";
                            StatusValue = "N/A";
                        }
                        else
                        {
                            StatusText = Constants.ASSESSNEMT_PENDING_TAG;
                            StatusValue = Constants.ASSESSNEMT_PENDING_STATUS;
                        }
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_ACTIVE_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_PENDING_TAG;
                        StatusValue = Constants.ASSESSNEMT_PENDING_STATUS;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_CEO_FINALIZED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_CLOSED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_CLOSED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_OBSOLETE_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
                    }
                    else
                    {
                        StatusText = "N/A";
                        StatusValue = "N/A";
                    }



                    DataRow drCompetencyStatus = CompetencyAssessment.NewRow();

                    drCompetencyStatus["STATUS_TEXT"] = StatusText;
                    drCompetencyStatus["STATUS_VALUE"] = StatusValue;

                    CompetencyAssessment.Rows.Add(drCompetencyStatus);
                }


                DataView view = new DataView(CompetencyAssessment.Copy());
                DataTable distinctValues = view.ToTable(true, "STATUS_TEXT", "STATUS_VALUE");
                DataTable TempCompetencyAssessment = new DataTable();
                TempCompetencyAssessment = CompetencyAssessment.Copy();

                TempCompetencyAssessment.Rows.Clear();

                for (int i = 0; i < distinctValues.Rows.Count; i++)
                {
                    DataRow[] drArr = CompetencyAssessment.Select("STATUS_TEXT = '" + distinctValues.Rows[i]["STATUS_TEXT"].ToString() + "'");
                    DataRow dr = TempCompetencyAssessment.NewRow();

                    dr["STATUS_TEXT"] = distinctValues.Rows[i]["STATUS_TEXT"].ToString();
                    dr["STATUS_VALUE"] = drArr.Length.ToString();

                    TempCompetencyAssessment.Rows.Add(dr);
                }

                dtGoalAssessmentDetails = new DataTable();
                dtGoalAssessmentDetails = TempCompetencyAssessment.Copy();

                string JS = @"
                                       <script>
                                            

                                            var config2 = {
                                                type: 'pie',
                                                data: {
                                                    datasets: [{
                                                        data: [";
                for (int i = 0; i < dtGoalAssessmentDetails.Rows.Count; i++)
                {
                    string EmployeeCount = dtGoalAssessmentDetails.Rows[i]["STATUS_VALUE"].ToString();

                    JS += EmployeeCount + @",";
                }
                JS += @"],
                                                        backgroundColor: [";
                for (int i = 0; i < dtGoalAssessmentDetails.Rows.Count; i++)
                {
                    string StatusName = dtGoalAssessmentDetails.Rows[i]["STATUS_TEXT"].ToString();
                    if (StatusName == Constants.ASSESSNEMT_PENDING_TAG)
                    {
                        JS += Constants.ASSESSNEMT_PENDING_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_ACTIVE_TAG)
                    {
                        JS += Constants.ASSESSNEMT_ACTIVE_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_CEO_FINALIZED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_CEO_FINALIZED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_CLOSED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_CLOSED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_OBSOLETE_TAG)
                    {
                        JS += Constants.ASSESSNEMT_OBSOLETE_COLOR + ",";
                    }
                    else if (StatusName == "N/A")
                    {
                        JS += Constants.ASSESSNEMT_NA_COLOR + ",";
                    }
                    else
                    {
                        JS += @"randomColor(),";
                    }
                }
                JS += @" ],
                                                        label: 'Goal Assessment'
                                                    }],
                                                    labels: [";
                for (int i = 0; i < dtGoalAssessmentDetails.Rows.Count; i++)
                {
                    string StatusName = dtGoalAssessmentDetails.Rows[i]["STATUS_TEXT"].ToString();

                    JS += @"'" + StatusName + @"',";
                }
                JS += @" ]
                                                },
                                                options: {
                                                    responsive: true,
                                                    legend: {
                                                        position: 'right',
                                                    },
                                                    title: {
                                                        display: true,
                                                        text: 'Goal Assessment'
                                                    },
                                                    animation: {
                                                        position: 'left',
                                                        animateScale: true,
                                                        animateRotate: true
                                                    }
                                                }
                                            };

                                            
                                            </script>
                                
                             ";

                GoalChartJS = JS;
                return GoalChartJS;



            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SDDH = null;
                dtGoalAssessmentDetails.Dispose();
            }
            return GoalChartJS;
        }

        string loadCompetencyChart(DataTable dtCompetencyAssessmentDetails)
        {
            SupervisorDashboardDataHandler SDDH = new SupervisorDashboardDataHandler();
            DataTable CompetencyAssessment = new DataTable();
            string CompetencyChartJS = String.Empty;
            try
            {

                CompetencyAssessment.Columns.Add("STATUS_TEXT");
                CompetencyAssessment.Columns.Add("STATUS_VALUE");

                for (int i = 0; i < dtCompetencyAssessmentDetails.Rows.Count; i++)
                {
                    string StatusValue = dtCompetencyAssessmentDetails.Rows[i]["COMPETENCY_ASSESSMENT_STATUS"].ToString().Trim();
                    string StatusText = String.Empty;

                    if ((StatusValue == "") || (StatusValue == Constants.ASSESSNEMT_PENDING_STATUS))
                    {
                        if ((StatusValue == "") && (Constants.STATUS_INACTIVE_VALUE == dtCompetencyAssessmentDetails.Rows[i]["INCLUDE_COMPITANCY_ASSESSMENT"].ToString().Trim()))
                        {
                            StatusText = "N/A";
                            StatusValue = "N/A";
                        }
                        else
                        {
                            StatusText = Constants.ASSESSNEMT_PENDING_TAG;
                            StatusValue = Constants.ASSESSNEMT_PENDING_STATUS;
                        }
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_ACTIVE_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_PENDING_TAG;
                        StatusValue = Constants.ASSESSNEMT_PENDING_STATUS;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_CEO_FINALIZED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_CLOSED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_CLOSED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_OBSOLETE_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
                    }
                    else
                    {
                        StatusText = "N/A";
                        StatusValue = "N/A";
                    }



                    DataRow drCompetencyStatus = CompetencyAssessment.NewRow();

                    drCompetencyStatus["STATUS_TEXT"] = StatusText;
                    drCompetencyStatus["STATUS_VALUE"] = StatusValue;

                    CompetencyAssessment.Rows.Add(drCompetencyStatus);
                }


                DataView view = new DataView(CompetencyAssessment.Copy());
                DataTable distinctValues = view.ToTable(true, "STATUS_TEXT", "STATUS_VALUE");
                DataTable TempCompetencyAssessment = new DataTable();
                TempCompetencyAssessment = CompetencyAssessment.Copy();

                TempCompetencyAssessment.Rows.Clear();

                for (int i = 0; i < distinctValues.Rows.Count; i++)
                {
                    DataRow[] drArr = CompetencyAssessment.Select("STATUS_TEXT = '" + distinctValues.Rows[i]["STATUS_TEXT"].ToString() + "'");
                    DataRow dr = TempCompetencyAssessment.NewRow();

                    dr["STATUS_TEXT"] = distinctValues.Rows[i]["STATUS_TEXT"].ToString();
                    dr["STATUS_VALUE"] = drArr.Length.ToString();

                    TempCompetencyAssessment.Rows.Add(dr);
                }

                dtCompetencyAssessmentDetails = new DataTable();
                dtCompetencyAssessmentDetails = TempCompetencyAssessment.Copy();

                string JS = @"
                                       <script>
                                            var randomScalingFactor = function() {
                                                return Math.round(Math.random() * 100);
                                            };
                                            var randomColorFactor = function() {
                                                return Math.round(Math.random() * 255);
                                            };
                                            var randomColor = function(opacity) {
                                                return 'rgba(' + randomColorFactor() + ',' + randomColorFactor() + ',' + randomColorFactor() + ',' + (opacity || '.7') + ')';
                                            };

                                            var config = {
                                                type: 'pie',
                                                data: {
                                                    datasets: [{
                                                        data: [";
                for (int i = 0; i < dtCompetencyAssessmentDetails.Rows.Count; i++)
                {
                    string EmployeeCount = dtCompetencyAssessmentDetails.Rows[i]["STATUS_VALUE"].ToString();

                    JS += EmployeeCount + @",";
                }
                JS += @"],
                                                        backgroundColor: [";
                for (int i = 0; i < dtCompetencyAssessmentDetails.Rows.Count; i++)
                {
                    string StatusName = dtCompetencyAssessmentDetails.Rows[i]["STATUS_TEXT"].ToString();
                    if (StatusName == Constants.ASSESSNEMT_PENDING_TAG)
                    {
                        JS += Constants.ASSESSNEMT_PENDING_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_ACTIVE_TAG)
                    {
                        JS += Constants.ASSESSNEMT_ACTIVE_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_CEO_FINALIZED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_CEO_FINALIZED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_CLOSED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_CLOSED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_OBSOLETE_TAG)
                    {
                        JS += Constants.ASSESSNEMT_OBSOLETE_COLOR + ",";
                    }
                    else if (StatusName == "N/A")
                    {
                        JS += Constants.ASSESSNEMT_NA_COLOR + ",";
                    }
                    else
                    {
                        JS += @"randomColor(),";
                    }
                }
                JS += @" ],
                                                        label: 'Competency Assessment'
                                                    }],
                                                    labels: [";
                for (int i = 0; i < dtCompetencyAssessmentDetails.Rows.Count; i++)
                {
                    string StatusName = dtCompetencyAssessmentDetails.Rows[i]["STATUS_TEXT"].ToString();

                    JS += @"'" + StatusName + @"',";
                }
                JS += @" ]
                                                },
                                                options: {
                                                    responsive: true,
                                                    legend: {
                                                        position: 'right',
                                                    },
                                                    title: {
                                                        display: true,
                                                        text: 'Competency Assessment'
                                                    },
                                                    animation: {
                                                        position: 'left',
                                                        animateScale: true,
                                                        animateRotate: true
                                                    }
                                                }
                                            };

                                            
                                            </script>                                
                             ";

                CompetencyChartJS = JS;
                return CompetencyChartJS;
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SDDH = null;
                dtCompetencyAssessmentDetails.Dispose();
            }
            return CompetencyChartJS;
        }

        string loadSelfChart(DataTable dtSelfAssessmentDetails)
        {
            string SelfChartJS = String.Empty;
            SupervisorDashboardDataHandler SDDH = new SupervisorDashboardDataHandler();
            DataTable CompetencyAssessment = new DataTable();
            try
            {

                CompetencyAssessment.Columns.Add("STATUS_TEXT");
                CompetencyAssessment.Columns.Add("STATUS_VALUE");

                for (int i = 0; i < dtSelfAssessmentDetails.Rows.Count; i++)
                {
                    string StatusValue = dtSelfAssessmentDetails.Rows[i]["SELF_ASSESSMENT_STATUS"].ToString().Trim();
                    string StatusText = String.Empty;

                    if ((StatusValue == "") || (StatusValue == Constants.ASSESSNEMT_PENDING_STATUS))
                    {
                        if ((StatusValue == "") && (Constants.STATUS_INACTIVE_VALUE == dtSelfAssessmentDetails.Rows[i]["INCLUDE_SELF_ASSESSMENT"].ToString().Trim()))
                        {
                            StatusText = "N/A";
                            StatusValue = "N/A";
                        }
                        else
                        {
                            StatusText = Constants.ASSESSNEMT_PENDING_TAG;
                            StatusValue = Constants.ASSESSNEMT_PENDING_STATUS;
                        }
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_ACTIVE_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_PENDING_TAG;
                        StatusValue = Constants.ASSESSNEMT_PENDING_STATUS;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_CEO_FINALIZED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_CLOSED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_CLOSED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_OBSOLETE_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
                    }
                    else
                    {
                        StatusText = "N/A";
                        StatusValue = "N/A";
                    }



                    DataRow drCompetencyStatus = CompetencyAssessment.NewRow();

                    drCompetencyStatus["STATUS_TEXT"] = StatusText;
                    drCompetencyStatus["STATUS_VALUE"] = StatusValue;

                    CompetencyAssessment.Rows.Add(drCompetencyStatus);
                }


                DataView view = new DataView(CompetencyAssessment.Copy());
                DataTable distinctValues = view.ToTable(true, "STATUS_TEXT", "STATUS_VALUE");
                DataTable TempCompetencyAssessment = new DataTable();
                TempCompetencyAssessment = CompetencyAssessment.Copy();

                TempCompetencyAssessment.Rows.Clear();

                for (int i = 0; i < distinctValues.Rows.Count; i++)
                {
                    DataRow[] drArr = CompetencyAssessment.Select("STATUS_TEXT = '" + distinctValues.Rows[i]["STATUS_TEXT"].ToString() + "'");
                    DataRow dr = TempCompetencyAssessment.NewRow();

                    dr["STATUS_TEXT"] = distinctValues.Rows[i]["STATUS_TEXT"].ToString();
                    dr["STATUS_VALUE"] = drArr.Length.ToString();

                    TempCompetencyAssessment.Rows.Add(dr);
                }

                dtSelfAssessmentDetails = new DataTable();
                dtSelfAssessmentDetails = TempCompetencyAssessment.Copy();

                string JS = @"
                                       <script>
                                            var randomScalingFactor = function() {
                                                return Math.round(Math.random() * 100);
                                            };
                                            var randomColorFactor = function() {
                                                return Math.round(Math.random() * 255);
                                            };
                                            var randomColor = function(opacity) {
                                                return 'rgba(' + randomColorFactor() + ',' + randomColorFactor() + ',' + randomColorFactor() + ',' + (opacity || '.7') + ')';
                                            };

                                            var config3 = {
                                                type: 'pie',
                                                data: {
                                                    datasets: [{
                                                        data: [";
                for (int i = 0; i < dtSelfAssessmentDetails.Rows.Count; i++)
                {
                    string EmployeeCount = dtSelfAssessmentDetails.Rows[i]["STATUS_VALUE"].ToString();

                    JS += EmployeeCount + @",";
                }
                JS += @"],
                                                        backgroundColor: [";
                for (int i = 0; i < dtSelfAssessmentDetails.Rows.Count; i++)
                {
                    string StatusName = dtSelfAssessmentDetails.Rows[i]["STATUS_TEXT"].ToString();
                    if (StatusName == Constants.ASSESSNEMT_PENDING_TAG)
                    {
                        JS += Constants.ASSESSNEMT_PENDING_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_ACTIVE_TAG)
                    {
                        JS += Constants.ASSESSNEMT_ACTIVE_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_CEO_FINALIZED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_CEO_FINALIZED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_CLOSED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_CLOSED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_OBSOLETE_TAG)
                    {
                        JS += Constants.ASSESSNEMT_OBSOLETE_COLOR + ",";
                    }
                    else if (StatusName == "N/A")
                    {
                        JS += Constants.ASSESSNEMT_NA_COLOR + ",";
                    }
                    else
                    {
                        JS += @"randomColor(),";
                    }
                }
                JS += @" ],
                                                        label: 'Self Assessment'
                                                    }],
                                                    labels: [";
                for (int i = 0; i < dtSelfAssessmentDetails.Rows.Count; i++)
                {
                    string StatusName = dtSelfAssessmentDetails.Rows[i]["STATUS_TEXT"].ToString();

                    JS += @"'" + StatusName + @"',";
                }
                JS += @" ]
                                                },
                                                options: {
                                                    responsive: true,
                                                    legend: {
                                                        position: 'right',
                                                    },
                                                    title: {
                                                        display: true,
                                                        text: 'Self Assessment'
                                                    },
                                                    animation: {
                                                        position: 'left',
                                                        animateScale: true,
                                                        animateRotate: true
                                                    }
                                                }
                                            };

                                            
                                            </script>
                                
                             ";

                SelfChartJS = JS;
                return SelfChartJS;


            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SDDH = null;
                dtSelfAssessmentDetails.Dispose();
            }
            return SelfChartJS;
        }

        string loadEvaluationChart(DataTable dtEvaluationDetails)
        {
            string EvaluationChartJS = String.Empty;

            SupervisorDashboardDataHandler SDDH = new SupervisorDashboardDataHandler();
            DataTable CompetencyAssessment = new DataTable();
            try
            {

                CompetencyAssessment.Columns.Add("STATUS_TEXT");
                CompetencyAssessment.Columns.Add("STATUS_VALUE");

                for (int i = 0; i < dtEvaluationDetails.Rows.Count; i++)
                {
                    string StatusValue = dtEvaluationDetails.Rows[i]["STATUS_CODE"].ToString().Trim();
                    string StatusText = String.Empty;

                    if ((StatusValue == "") || (StatusValue == Constants.ASSESSNEMT_PENDING_STATUS))
                    {
                        //if ((StatusValue == "") && (Constants.STATUS_INACTIVE_VALUE == dtCompetencyAssessmentEmployeeMarks.Rows[i]["INCLUDE_SELF_ASSESSMENT"].ToString().Trim()))
                        //{
                        //    StatusText = "N/A";
                        //    StatusValue = "N/A";
                        //}
                        //else
                        //{
                        StatusText = Constants.ASSESSNEMT_PENDING_TAG;
                        StatusValue = Constants.ASSESSNEMT_PENDING_STATUS;
                        //}
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_ACTIVE_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_PENDING_TAG;
                        StatusValue = Constants.ASSESSNEMT_PENDING_STATUS;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_CEO_FINALIZED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_CLOSED_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_CLOSED_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_OBSOLETE_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
                    }
                    else
                    {
                        StatusText = "N/A";
                        StatusValue = "N/A";
                    }



                    DataRow drCompetencyStatus = CompetencyAssessment.NewRow();

                    drCompetencyStatus["STATUS_TEXT"] = StatusText;
                    drCompetencyStatus["STATUS_VALUE"] = StatusValue;

                    CompetencyAssessment.Rows.Add(drCompetencyStatus);
                }


                DataView view = new DataView(CompetencyAssessment.Copy());
                DataTable distinctValues = view.ToTable(true, "STATUS_TEXT", "STATUS_VALUE");
                DataTable TempCompetencyAssessment = new DataTable();
                TempCompetencyAssessment = CompetencyAssessment.Copy();

                TempCompetencyAssessment.Rows.Clear();

                for (int i = 0; i < distinctValues.Rows.Count; i++)
                {
                    DataRow[] drArr = CompetencyAssessment.Select("STATUS_TEXT = '" + distinctValues.Rows[i]["STATUS_TEXT"].ToString() + "'");
                    DataRow dr = TempCompetencyAssessment.NewRow();

                    dr["STATUS_TEXT"] = distinctValues.Rows[i]["STATUS_TEXT"].ToString();
                    dr["STATUS_VALUE"] = drArr.Length.ToString();

                    TempCompetencyAssessment.Rows.Add(dr);
                }

                dtEvaluationDetails = new DataTable();
                dtEvaluationDetails = TempCompetencyAssessment.Copy();

                string JS = @"
                                       <script>
                                            var randomScalingFactor = function() {
                                                return Math.round(Math.random() * 100);
                                            };
                                            var randomColorFactor = function() {
                                                return Math.round(Math.random() * 255);
                                            };
                                            var randomColor = function(opacity) {
                                                return 'rgba(' + randomColorFactor() + ',' + randomColorFactor() + ',' + randomColorFactor() + ',' + (opacity || '.7') + ')';
                                            };

                                            var config4 = {
                                                type: 'pie',
                                                data: {
                                                    datasets: [{
                                                        data: [";
                for (int i = 0; i < dtEvaluationDetails.Rows.Count; i++)
                {
                    string EmployeeCount = dtEvaluationDetails.Rows[i]["STATUS_VALUE"].ToString();

                    JS += EmployeeCount + @",";
                }
                JS += @"],
                                                        backgroundColor: [";
                for (int i = 0; i < dtEvaluationDetails.Rows.Count; i++)
                {
                    string StatusName = dtEvaluationDetails.Rows[i]["STATUS_TEXT"].ToString();
                    if (StatusName == Constants.ASSESSNEMT_PENDING_TAG)
                    {
                        JS += Constants.ASSESSNEMT_PENDING_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_ACTIVE_TAG)
                    {
                        JS += Constants.ASSESSNEMT_ACTIVE_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG)
                    {
                        JS += Constants.ASSESSNEMT_SUBORDINATE_AGREE_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_CEO_FINALIZED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_CEO_FINALIZED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_CLOSED_TAG)
                    {
                        JS += Constants.ASSESSNEMT_CLOSED_COLOR + ",";
                    }
                    else if (StatusName == Constants.ASSESSNEMT_OBSOLETE_TAG)
                    {
                        JS += Constants.ASSESSNEMT_OBSOLETE_COLOR + ",";
                    }
                    else if (StatusName == "N/A")
                    {
                        JS += Constants.ASSESSNEMT_NA_COLOR + ",";
                    }
                    else
                    {
                        JS += @"randomColor(),";
                    }
                }
                JS += @" ],
                                                        label: 'Performance Evaluation'
                                                    }],
                                                    labels: [";
                for (int i = 0; i < dtEvaluationDetails.Rows.Count; i++)
                {
                    string StatusName = dtEvaluationDetails.Rows[i]["STATUS_TEXT"].ToString();

                    JS += @"'" + StatusName + @"',";
                }
                JS += @" ]
                                                },
                                                options: {
                                                    responsive: true,
                                                    legend: {
                                                        position: 'right',
                                                    },
                                                    title: {
                                                        display: true,
                                                        text: 'Performance Evaluation'
                                                    },
                                                    animation: {
                                                        position: 'left',
                                                        animateScale: true,
                                                        animateRotate: true
                                                    }
                                                }
                                            };

                                            
                                            </script>
                                
                             ";

                EvaluationChartJS = JS;
                return EvaluationChartJS;


            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SDDH = null;
                dtEvaluationDetails.Dispose();
            }
            return EvaluationChartJS;
        }

        void DisplayCharts()
        {
            DataTable dtDisplayDataList = new DataTable();
            string DispalyScript = String.Empty;
            try
            {
                dtDisplayDataList = (Session["dtDisplayDataList"] as DataTable).Copy();

                DispalyScript += JSFunction();


                DispalyScript += loadCompetencyChart(dtDisplayDataList.Copy());
                DispalyScript += loadGoalChart(dtDisplayDataList.Copy());
                DispalyScript += loadSelfChart(dtDisplayDataList.Copy());
                DispalyScript += loadEvaluationChart(dtDisplayDataList.Copy());
                if (dtDisplayDataList.Rows.Count > 0)
                {
                    lblChart.Text = DispalyScript;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "somekey", "DisplayCharts()", true);
                }
                else
                {
                    lblChart.Text = String.Empty;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                DispalyScript = String.Empty;
                dtDisplayDataList.Dispose();
            }
        }

        #endregion        
    }
}