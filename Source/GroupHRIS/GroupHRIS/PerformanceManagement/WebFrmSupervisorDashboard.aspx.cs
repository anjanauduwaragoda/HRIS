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
    public partial class WebFrmSupervisorDashboard : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                try
                {
                    log.Debug("IP:" + Request.UserHostAddress + "WebFrmSupervisorDashboard : Page_Load");

                    hfSupervisorID.Value = (Session["KeyEMPLOYEE_ID"] as string).Trim();


                    SetFinancialYear();
                    LoadAssessmentYears();
                    //LoadAssessments(ddlAssessment);
                    LoadAssessmentStatus(ddlAssessmentStatus);
                    LoadAssessments(ddlAssessment);
                    //LoadEvaluationStatus(ddlEvaluationStatus);
                    //LoadGrid();

                    //loadEvaluationChart();
                    Session["DASHBOARD"] = "SUP";//For indentify the performance  evaluation user view


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
                log.Debug("webFrmEmployeeLeaveSheet : imgbtnSearch_Click()");
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
                log.Debug("WebFrmSupervisorDashboard : grdvAssessment_PageIndexChanging()");

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
            LoadAssessments(ddlAssessment);

            //LoadAssessmentStatus(ddlAssessmentStatus);
            //LoadAssessments(ddlAssessment);
        }

        protected void ddlAssessmentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAssessments(ddlAssessment);
            LoadEvaluationStatus(ddlEvaluationStatus);
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
            SupervisorDashboardDataHandler SDDH = new SupervisorDashboardDataHandler();
            DataTable dtYearOfAssessment = new DataTable();
            try
            {
                dtYearOfAssessment = SDDH.PopulateYears().Copy();
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
                SDDH = null;
                dtYearOfAssessment.Dispose();
            }
        }

        void LoadAssessmentStatus(DropDownList AssessmentStatusList)
        {
            try
            {
                AssessmentStatusList.Items.Clear();
                AssessmentStatusList.Items.Add(new ListItem("", ""));
                AssessmentStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_PENDING_TAG, Constants.ASSESSNEMT_PENDING_STATUS));
                AssessmentStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_ACTIVE_TAG, Constants.ASSESSNEMT_ACTIVE_STATUS));
                AssessmentStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_CLOSED_TAG, Constants.ASSESSNEMT_CLOSED_STATUS));
                AssessmentStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_OBSOLETE_TAG, Constants.ASSESSNEMT_OBSOLETE_STATUS));
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
        }

        void LoadEvaluationStatus(DropDownList EvaluationStatusList)
        {
            try
            {
                EvaluationStatusList.Items.Clear();
                EvaluationStatusList.Items.Add(new ListItem("", ""));

                if (ddlAssessmentStatus.SelectedIndex > 0)
                {
                    if (ddlAssessmentStatus.SelectedValue == Constants.ASSESSNEMT_PENDING_STATUS)
                    {
                        //EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_PENDING_TAG, Constants.ASSESSNEMT_PENDING_STATUS));
                    }
                    else if (ddlAssessmentStatus.SelectedValue == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    {
                        //EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_OBSOLETE_TAG, Constants.ASSESSNEMT_OBSOLETE_STATUS));
                    }
                    else if (ddlAssessmentStatus.SelectedValue == Constants.ASSESSNEMT_ACTIVE_STATUS)
                    {
                        EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_ACTIVE_TAG, Constants.ASSESSNEMT_ACTIVE_STATUS));
                        EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG, Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS));
                        EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG, Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS));
                        EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG, Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS));
                        EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG, Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS));
                        EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_CEO_FINALIZED_TAG, Constants.ASSESSNEMT_CEO_FINALIZED_STATUS));

                    }
                    else if (ddlAssessmentStatus.SelectedValue == Constants.ASSESSNEMT_CLOSED_STATUS)
                    {
                        EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_ACTIVE_TAG, Constants.ASSESSNEMT_ACTIVE_STATUS));
                        EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG, Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS));
                        EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG, Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS));
                        EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG, Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS));
                        EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG, Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS));
                        EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_CEO_FINALIZED_TAG, Constants.ASSESSNEMT_CEO_FINALIZED_STATUS));
                        //EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_CLOSED_TAG, Constants.ASSESSNEMT_CLOSED_STATUS));
                    }

                }

                //EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_ACTIVE_TAG, Constants.ASSESSNEMT_ACTIVE_STATUS));
                //EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG, Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS));
                //EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG, Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS));
                //EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG, Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS));
                //EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG, Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS));
                //EvaluationStatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_CEO_FINALIZED_TAG, Constants.ASSESSNEMT_CEO_FINALIZED_STATUS));

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
        }

        void LoadAssessments(DropDownList AssessmentList)
        {
            SupervisorDashboardDataHandler SDDH = new SupervisorDashboardDataHandler();
            DataTable dtAssessments = new DataTable();
            try
            {
                string user = (Session["KeyEMPLOYEE_ID"] as string).Trim();
                AssessmentList.Items.Clear();

                if (ddlAssessmentStatus.Items.Count == 0)
                {
                    dtAssessments = SDDH.PopulateAssessments(ddlYearOfAssessment.SelectedValue.ToString().Trim(), user).Copy();
                }
                else if (ddlAssessmentStatus.SelectedIndex == 0)
                {
                    dtAssessments = SDDH.PopulateAssessments(ddlYearOfAssessment.SelectedValue.ToString().Trim(), user).Copy();
                }
                else
                {
                    dtAssessments = SDDH.PopulateAssessments(ddlYearOfAssessment.SelectedValue.ToString().Trim(), ddlAssessmentStatus.SelectedValue.ToString(), user).Copy();
                }

                //dtAssessments = SDDH.PopulateAssessments(ddlYearOfAssessment.SelectedValue.ToString().Trim(), user).Copy();
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
                SDDH = null;
                dtAssessments.Dispose();
            }
        }

        void LoadGrid()
        {
            SupervisorDashboardDataHandler SDDH = new SupervisorDashboardDataHandler();
            DataTable dtAssessedEmployeeList = new DataTable();
            PasswordHandler crpto;

            try
            {
                //Convert Evaluation Status
                string EvaluationStatus = ddlEvaluationStatus.SelectedValue;
                if (EvaluationStatus == Constants.ASSESSNEMT_PENDING_STATUS)
                {
                    EvaluationStatus = Constants.ASSESSNEMT_ACTIVE_STATUS;
                }
                else if (EvaluationStatus == Constants.ASSESSNEMT_ACTIVE_STATUS)
                {
                    EvaluationStatus = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS;
                }
                //


                //dtAssessedEmployeeList = SetSuperviorStatus(SDDH.Populate(hfSupervisorID.Value, ddlYearOfAssessment.SelectedValue, ddlAssessmentStatus.SelectedValue, ddlAssessment.SelectedValue, EvaluationStatus)).Copy();


                dtAssessedEmployeeList = SDDH.Populate(hfSupervisorID.Value, ddlYearOfAssessment.SelectedValue, ddlAssessmentStatus.SelectedValue, ddlAssessment.SelectedValue, EvaluationStatus).Copy();
                //DataTable dtAssessments = new DataTable();

                //dtAssessments = SetSuperviorStatus(dtAssessedEmployeeList.Copy()).Copy();
                //dtAssessedEmployeeList = new DataTable();
                //dtAssessedEmployeeList = dtAssessments.Copy();
                //dtAssessments.Dispose();

                crpto = new PasswordHandler();

                //Session["dtDisplayDataList"] = dtAssessedEmployeeList.Copy();
                //DisplayCharts();

                //for (int i = 0; i < dtAssessedEmployeeList.Rows.Count; i++)
                //{
                //    string Title = dtAssessedEmployeeList.Rows[i]["TITLE"].ToString();
                //    string NameWithInitials = dtAssessedEmployeeList.Rows[i]["INITIALS_NAME"].ToString();
                //    dtAssessedEmployeeList.Rows[i]["INITIALS_NAME"] = Title + " " + NameWithInitials;

                //    string isGoalAssessmentInclude = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["INCLUDE_GOAL_ASSESSMENT"].ToString());
                //    string GoalAssessmentStatus = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["GOAL_ASSESSMENT_STATUS"].ToString());
                //    string isCompetencyAssessmentInclude = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["INCLUDE_COMPITANCY_ASSESSMENT"].ToString());
                //    string CompetencyAssessmentStatus = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["COMPETENCY_ASSESSMENT_STATUS"].ToString());
                //    string isSelfAssessmentInclude = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["INCLUDE_SELF_ASSESSMENT"].ToString());
                //    string SelfAssessmentStatus = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["SELF_ASSESSMENT_STATUS"].ToString());

                //    string AssessmentID = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["ASSESSMENT_ID"].ToString());
                //    string SubordinateID = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["EMPLOYEE_ID"].ToString());
                //    string YearOfAssessment = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["YEAR_OF_ASSESSMENT"].ToString());

                //    string Evaluation = HttpUtility.HtmlDecode(dtAssessedEmployeeList.Rows[i]["STATUS_CODE"].ToString());


                //    Session["isGoalAssessmentInclude"] = isGoalAssessmentInclude;
                //    Session["isCompetencyAssessmentInclude"] = isCompetencyAssessmentInclude;

                //    //Set Evaluation Status

                //    if (isSelfAssessmentInclude == Constants.STATUS_ACTIVE_VALUE)
                //    {
                //        Evaluation = PerformanceEvaluationStatus(GoalAssessmentStatus, CompetencyAssessmentStatus, SelfAssessmentStatus, Evaluation, true);

                //        //SetSupervisorStatus
                //        DataRow dr = dtAssessedEmployeeList.NewRow();
                //        dr = dtAssessedEmployeeList.Rows[i];

                //        for (int j = 0; j < dtAssessedEmployeeList.Rows[i].ItemArray.Length; j++)
                //        {
                //            dtAssessedEmployeeList.Rows[i][j] = SetSupervisorStatus(GoalAssessmentStatus, CompetencyAssessmentStatus, SelfAssessmentStatus, Evaluation, dr, true)[j];
                //        }
                //    }
                //    else
                //    {
                //        Evaluation = PerformanceEvaluationStatus(GoalAssessmentStatus, CompetencyAssessmentStatus, SelfAssessmentStatus, Evaluation, false);

                //        //SetSupervisorStatus
                //        DataRow dr = dtAssessedEmployeeList.NewRow();
                //        dr = dtAssessedEmployeeList.Rows[i];

                //        for (int j = 0; j < dtAssessedEmployeeList.Rows[i].ItemArray.Length; j++)
                //        {
                //            dtAssessedEmployeeList.Rows[i][j] = SetSupervisorStatus(GoalAssessmentStatus, CompetencyAssessmentStatus, SelfAssessmentStatus, Evaluation, dr, false)[j];
                //        }
                //    }


                //}


                //New Method 2016/11/25
                DataTable dtTemp = new DataTable();
                dtTemp = SetSuperviorStatus(dtAssessedEmployeeList.Copy()).Copy();
                dtAssessedEmployeeList = new DataTable();
                dtAssessedEmployeeList = dtTemp.Copy();
                dtTemp.Dispose();
                //

                grdvAssessment.DataSource = dtAssessedEmployeeList.Copy();
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
                SDDH = null;
                crpto = null;
            }
        }

        DataRow SetSupervisorStatus(string GoalAssessmentStatus, string CompetencyAssessmentStatus, string SelfAssessmentStatus, string EvaluationStatus, DataRow drAssessment, Boolean isSelfEvaluationInclude)
        {
            PasswordHandler crpto;
            try
            {
                log.Debug("SetSupervisorStatus");
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
                    }
                    else if (GoalAssessmentStatus == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorGoalAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_ACTIVE_TAG + @"</a>";
                        drAssessment["GOAL_ASSESSMENT_STATUS"] = hyperlink;
                    }
                    else if (GoalAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorGoalAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG + @"</a>";
                        drAssessment["GOAL_ASSESSMENT_STATUS"] = hyperlink;
                    }
                    else if (GoalAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorGoalAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG + @"</a>";
                        drAssessment["GOAL_ASSESSMENT_STATUS"] = hyperlink;
                    }
                    else if (GoalAssessmentStatus == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorGoalAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CEO_FINALIZED_TAG + @"</a>";
                        drAssessment["GOAL_ASSESSMENT_STATUS"] = hyperlink;
                    }
                    else if (GoalAssessmentStatus == Constants.ASSESSNEMT_CLOSED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorGoalAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CLOSED_TAG + @"</a>";
                        drAssessment["GOAL_ASSESSMENT_STATUS"] = hyperlink;
                    }
                    else if (GoalAssessmentStatus == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorGoalAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_OBSOLETE_TAG + @"</a>";
                        drAssessment["GOAL_ASSESSMENT_STATUS"] = hyperlink;
                    }
                }
                else
                {
                    drAssessment["GOAL_ASSESSMENT_STATUS"] = "N/A";
                }
                //--

                //Set Competency Assessment Status


                if ((Session["isCompetencyAssessmentInclude"] as string) == Constants.CON_ACTIVE_STATUS)
                {

                    if ((CompetencyAssessmentStatus == "") || (CompetencyAssessmentStatus == Constants.ASSESSNEMT_PENDING_STATUS) || (CompetencyAssessmentStatus == Constants.ASSESSNEMT_ACTIVE_STATUS))
                    {
                        drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = Constants.ASSESSNEMT_PENDING_TAG;
                    }
                    else if (CompetencyAssessmentStatus == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_ACTIVE_TAG + @"</a>";
                        drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = hyperlink;
                    }
                    else if (CompetencyAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG + @"</a>";
                        drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = hyperlink;
                    }
                    else if (CompetencyAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG + @"</a>";
                        drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = hyperlink;
                    }
                    else if (CompetencyAssessmentStatus == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CEO_FINALIZED_TAG + @"</a>";
                        drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = hyperlink;
                    }
                    else if (CompetencyAssessmentStatus == Constants.ASSESSNEMT_CLOSED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CLOSED_TAG + @"</a>";
                        drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = hyperlink;
                    }
                    else if (CompetencyAssessmentStatus == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_OBSOLETE_TAG + @"</a>";
                        drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = hyperlink;
                    }
                }
                else
                {
                    drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = "N/A";
                }
                //--

                //Set Self Assessment Status
                if (isSelfEvaluationInclude)
                {
                    if ((SelfAssessmentStatus == "") || (SelfAssessmentStatus == Constants.ASSESSNEMT_PENDING_STATUS) || (SelfAssessmentStatus == Constants.ASSESSNEMT_ACTIVE_STATUS))
                    {
                        drAssessment["SELF_ASSESSMENT_STATUS"] = Constants.ASSESSNEMT_PENDING_TAG;
                    }
                    else if (SelfAssessmentStatus == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorSelfAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_ACTIVE_TAG + @"</a>";
                        drAssessment["SELF_ASSESSMENT_STATUS"] = hyperlink;
                    }
                    else if (SelfAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorSelfAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG + @"</a>";
                        drAssessment["SELF_ASSESSMENT_STATUS"] = hyperlink;
                    }
                    else if (SelfAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorSelfAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG + @"</a>";
                        drAssessment["SELF_ASSESSMENT_STATUS"] = hyperlink;
                    }
                    else if (SelfAssessmentStatus == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorSelfAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CEO_FINALIZED_TAG + @"</a>";
                        drAssessment["SELF_ASSESSMENT_STATUS"] = hyperlink;
                    }
                    else if (SelfAssessmentStatus == Constants.ASSESSNEMT_CLOSED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorSelfAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CLOSED_TAG + @"</a>";
                        drAssessment["SELF_ASSESSMENT_STATUS"] = hyperlink;
                    }
                    else if (SelfAssessmentStatus == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorSelfAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_OBSOLETE_TAG + @"</a>";
                        drAssessment["SELF_ASSESSMENT_STATUS"] = hyperlink;
                    }
                }
                else
                {
                    drAssessment["SELF_ASSESSMENT_STATUS"] = "N/A";
                }
                //--

                //Set Performance Evaluation Status
                if (EvaluationStatus == Constants.ASSESSNEMT_PENDING_STATUS)
                {
                    drAssessment["STATUS_CODE"] = Constants.ASSESSNEMT_PENDING_TAG;
                }
                else if (EvaluationStatus == Constants.ASSESSNEMT_ACTIVE_STATUS)
                {
                    string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                    string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_ACTIVE_TAG + @"</a>";
                    drAssessment["STATUS_CODE"] = hyperlink;
                }
                else if (EvaluationStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                {
                    string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                    string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG + @"</a>";
                    drAssessment["STATUS_CODE"] = hyperlink;
                }
                else if (EvaluationStatus == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                {
                    string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                    string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG + @"</a>";
                    drAssessment["STATUS_CODE"] = hyperlink;
                }
                else if (EvaluationStatus == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                {
                    string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                    string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CEO_FINALIZED_TAG + @"</a>";
                    drAssessment["STATUS_CODE"] = hyperlink;
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
                }
                else if (EvaluationStatus == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                {
                    string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                    string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_OBSOLETE_TAG + @"</a>";
                    drAssessment["STATUS_CODE"] = hyperlink;
                }

                if ((IsSubordinateAgreed == Constants.CON_ACTIVE_STATUS) && (EvaluationStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS))
                {
                    string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                    string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG + @"</a>";
                    drAssessment["STATUS_CODE"] = hyperlink;
                }
                else if ((IsSubordinateAgreed == Constants.CON_INACTIVE_STATUS) && (EvaluationStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS))
                {
                    string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                    string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG + @"</a>";
                    drAssessment["STATUS_CODE"] = hyperlink;
                }
                //--

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
                if (isSelfAssessmentInclude)
                {
                    //Main
                    if (EvaluationStatus == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS)
                    {
                        Status = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS;
                        return Status;
                    }
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
                    //--

                    //Pending List
                    string pendigSet = Constants.ASSESSNEMT_PENDING_STATUS + Constants.ASSESSNEMT_ACTIVE_STATUS + Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS;

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
                    //--

                    //All in Same State

                    //if ((GoalAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS) && (GoalAssessmentStatus == CompetencyAssessmentStatus) && (CompetencyAssessmentStatus == SelfAssessmentStatus))
                    //{
                    //    Status = Constants.ASSESSNEMT_ACTIVE_STATUS;
                    //    return Status;
                    //}

                    //if ((GoalAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS) && (GoalAssessmentStatus == CompetencyAssessmentStatus) && (CompetencyAssessmentStatus == SelfAssessmentStatus))
                    //{
                    //    Status = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS;
                    //    return Status;
                    //}

                    //if ((GoalAssessmentStatus == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS) && (GoalAssessmentStatus == CompetencyAssessmentStatus) && (CompetencyAssessmentStatus == SelfAssessmentStatus))
                    //{
                    //    Status = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                    //    return Status;
                    //}
                    //--
                    string AvailableStates = GoalAssessmentStatus + CompetencyAssessmentStatus + SelfAssessmentStatus;

                    if (AvailableStates.Contains(Convert.ToChar(Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)))
                    {
                        Status = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS;
                        return Status;
                    }
                    else if (AvailableStates.Contains(Convert.ToChar(Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)))
                    {
                        Status = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;
                        return Status;
                    }
                    else if (AvailableStates.Contains(Convert.ToChar(Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)))
                    {
                        Status = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS;
                        return Status;
                    }
                    else if (AvailableStates.Contains(Convert.ToChar(Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)))
                    {
                        Status = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                        return Status;
                    }
                }
                else
                {
                    //Main
                    if (EvaluationStatus == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS)
                    {
                        Status = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS;
                        return Status;
                    }
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
                    //--

                    //Pending List
                    string pendigSet = Constants.ASSESSNEMT_PENDING_STATUS + Constants.ASSESSNEMT_ACTIVE_STATUS + Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS;

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
                    //--

                    //All in Same State

                    //if ((GoalAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS) && (GoalAssessmentStatus == CompetencyAssessmentStatus))
                    //{
                    //    Status = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;
                    //    return Status;
                    //}

                    //if ((GoalAssessmentStatus == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS) && (GoalAssessmentStatus == CompetencyAssessmentStatus))
                    //{
                    //    Status = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS;
                    //    return Status;
                    //}

                    //if ((GoalAssessmentStatus == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS) && (GoalAssessmentStatus == CompetencyAssessmentStatus))
                    //{
                    //    Status = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                    //    return Status;
                    //}
                    //--
                    string AvailableStates = GoalAssessmentStatus + CompetencyAssessmentStatus;

                    if (AvailableStates.Contains(Convert.ToChar(Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)))
                    {
                        Status = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS;
                        return Status;
                    }
                    else if (AvailableStates.Contains(Convert.ToChar(Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)))
                    {
                        Status = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;
                        return Status;
                    }
                    else if (AvailableStates.Contains(Convert.ToChar(Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)))
                    {
                        Status = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS;
                        return Status;
                    }
                    else if (AvailableStates.Contains(Convert.ToChar(Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)))
                    {
                        Status = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                        return Status;
                    }
                }
                ////SupervisorCompleted State
                //if (EvaluationStatus == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                //{
                //    Status = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;
                //    return Status;
                //}

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
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG)
                    {
                        StatusText = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
                    }
                    else if (StatusValue == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    {
                        StatusText = Constants.ASSESSNEMT_OBSOLETE_TAG;
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

                if (dtDisplayDataList.Rows.Count > 0)
                {
                    DispalyScript += JSFunction();


                    DispalyScript += loadCompetencyChart(dtDisplayDataList.Copy());
                    DispalyScript += loadGoalChart(dtDisplayDataList.Copy());
                    DispalyScript += loadSelfChart(dtDisplayDataList.Copy());
                    DispalyScript += loadEvaluationChart(dtDisplayDataList.Copy());

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

        DataTable SetSuperviorStatus(DataTable dtAssessmentDetails)
        {
            PasswordHandler crpto = new PasswordHandler();
            try
            {
                log.Debug("SetSuperviorStatus()");

                DataTable dtCharts = new DataTable();
                dtCharts = dtAssessmentDetails.Copy();

                for (int i = 0; i < dtAssessmentDetails.Rows.Count; i++)
                {
                    string ChartGoalAssessmentStatus = String.Empty;
                    string ChartCompetencyAssessmentStatus = String.Empty;
                    string ChartSelfAssessmentStatus = String.Empty;
                    string ChartPerformanceEvaluationStatus = String.Empty;

                    string AssessmentID = dtAssessmentDetails.Rows[i]["ASSESSMENT_ID"].ToString();
                    string SubordinateID = dtAssessmentDetails.Rows[i]["EMPLOYEE_ID"].ToString();
                    string YearOfAssessment = dtAssessmentDetails.Rows[i]["YEAR_OF_ASSESSMENT"].ToString();

                    string GoalAssessmentStatus = dtAssessmentDetails.Rows[i]["GOAL_ASSESSMENT_STATUS"].ToString();
                    string CompetencyAssessmentStatus = dtAssessmentDetails.Rows[i]["COMPETENCY_ASSESSMENT_STATUS"].ToString();
                    string SelfAssessmentStatus = dtAssessmentDetails.Rows[i]["SELF_ASSESSMENT_STATUS"].ToString();
                    string PerformanceEvaluationStatusCode = dtAssessmentDetails.Rows[i]["STATUS_CODE"].ToString();

                    string isGoalAssessmentInclude = dtAssessmentDetails.Rows[i]["INCLUDE_GOAL_ASSESSMENT"].ToString();
                    string isCompetencyAssessmentInclude = dtAssessmentDetails.Rows[i]["INCLUDE_COMPITANCY_ASSESSMENT"].ToString();
                    string isSelfAssessmentInclude = dtAssessmentDetails.Rows[i]["INCLUDE_SELF_ASSESSMENT"].ToString();

                    string isEmployeeAgreed = dtAssessmentDetails.Rows[i]["IS_EMPLOYEE_AGREED"].ToString();

                    if (isGoalAssessmentInclude == Constants.CON_INACTIVE_STATUS)
                    {
                        GoalAssessmentStatus = "N/A";
                        ChartGoalAssessmentStatus = "";
                    }
                    if (isCompetencyAssessmentInclude == Constants.CON_INACTIVE_STATUS)
                    {
                        CompetencyAssessmentStatus = "N/A";
                        ChartCompetencyAssessmentStatus = "";
                    }
                    if (isSelfAssessmentInclude == Constants.CON_INACTIVE_STATUS)
                    {
                        SelfAssessmentStatus = "N/A";
                        ChartSelfAssessmentStatus = "";
                    }


                    if ((isGoalAssessmentInclude == Constants.CON_ACTIVE_STATUS) && (GoalAssessmentStatus == String.Empty))
                    {
                        GoalAssessmentStatus = Constants.ASSESSNEMT_PENDING_TAG;
                        ChartGoalAssessmentStatus = Constants.ASSESSNEMT_PENDING_STATUS;
                    }
                    if ((isCompetencyAssessmentInclude == Constants.CON_ACTIVE_STATUS) && (CompetencyAssessmentStatus == String.Empty))
                    {
                        CompetencyAssessmentStatus = Constants.ASSESSNEMT_PENDING_TAG;
                        ChartCompetencyAssessmentStatus = Constants.ASSESSNEMT_PENDING_STATUS;
                    }
                    if ((isSelfAssessmentInclude == Constants.CON_ACTIVE_STATUS) && (SelfAssessmentStatus == String.Empty))
                    {
                        SelfAssessmentStatus = Constants.ASSESSNEMT_PENDING_TAG;
                        ChartSelfAssessmentStatus = Constants.ASSESSNEMT_PENDING_STATUS;
                    }

                    string GoalAssessmentStatusCopy = GoalAssessmentStatus;
                    string CompetencyAssessmentStatusCopy = CompetencyAssessmentStatus;
                    string SelfAssessmentStatusCopy = SelfAssessmentStatus;
                    string PerformanceEvaluationStatusCopy = PerformanceEvaluationStatusCode;


                    if (GoalAssessmentStatusCopy == Constants.ASSESSNEMT_ACTIVE_STATUS)
                    {
                        GoalAssessmentStatus = Constants.ASSESSNEMT_PENDING_TAG;
                        ChartGoalAssessmentStatus = Constants.ASSESSNEMT_PENDING_STATUS;
                    }
                    if (CompetencyAssessmentStatusCopy == Constants.ASSESSNEMT_ACTIVE_STATUS)
                    {
                        CompetencyAssessmentStatus = Constants.ASSESSNEMT_PENDING_TAG;
                        ChartCompetencyAssessmentStatus = Constants.ASSESSNEMT_PENDING_STATUS;
                    }
                    if (SelfAssessmentStatusCopy == Constants.ASSESSNEMT_ACTIVE_STATUS)
                    {
                        SelfAssessmentStatus = Constants.ASSESSNEMT_PENDING_TAG;
                        ChartSelfAssessmentStatus = Constants.ASSESSNEMT_PENDING_STATUS;
                    }




                    //Initialize Self Evaluation Status
                    PerformanceEvaluationStatusCode = Constants.ASSESSNEMT_PENDING_TAG;
                    ChartPerformanceEvaluationStatus = Constants.ASSESSNEMT_PENDING_STATUS;


                    //Assessment is assign to employee, employee has not complete yet.
                    //When assessments are in pending state
                    if (((GoalAssessmentStatusCopy == Constants.ASSESSNEMT_PENDING_STATUS) || (GoalAssessmentStatusCopy == "N/A")) && ((CompetencyAssessmentStatusCopy == Constants.ASSESSNEMT_PENDING_STATUS) || (CompetencyAssessmentStatusCopy == "N/A")) && ((SelfAssessmentStatusCopy == Constants.ASSESSNEMT_PENDING_STATUS) || (SelfAssessmentStatusCopy == "N/A")))
                    {
                        //PerformanceEvaluationStatusCode = Constants.ASSESSNEMT_PENDING_STATUS;
                        PerformanceEvaluationStatusCode = Constants.ASSESSNEMT_PENDING_TAG;
                        ChartPerformanceEvaluationStatus = Constants.ASSESSNEMT_PENDING_STATUS;
                    }

                    //Subordinate Finalized Assessment
                    if (GoalAssessmentStatusCopy == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        //GoalAssessmentStatus = Constants.ASSESSNEMT_ACTIVE_STATUS;
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorGoalAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_ACTIVE_TAG + @"</a>";
                        GoalAssessmentStatus = hyperlink;
                        ChartGoalAssessmentStatus = Constants.ASSESSNEMT_ACTIVE_STATUS;
                    }
                    if (CompetencyAssessmentStatusCopy == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        //GoalAssessmentStatus = Constants.ASSESSNEMT_ACTIVE_STATUS;
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_ACTIVE_TAG + @"</a>";
                        CompetencyAssessmentStatus = hyperlink;
                        ChartCompetencyAssessmentStatus = Constants.ASSESSNEMT_ACTIVE_STATUS;
                    }
                    if (SelfAssessmentStatusCopy == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        //SelfAssessmentStatus = Constants.ASSESSNEMT_ACTIVE_STATUS;
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorSelfAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_ACTIVE_TAG + @"</a>";
                        SelfAssessmentStatus = hyperlink;
                        ChartSelfAssessmentStatus = Constants.ASSESSNEMT_ACTIVE_STATUS;
                    }

                    if (((GoalAssessmentStatusCopy == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS) || (GoalAssessmentStatusCopy == "N/A")) && ((CompetencyAssessmentStatusCopy == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS) || (CompetencyAssessmentStatusCopy == "N/A")) && ((SelfAssessmentStatusCopy == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS) || (SelfAssessmentStatusCopy == "N/A")))
                    {
                        PerformanceEvaluationStatusCode = Constants.ASSESSNEMT_PENDING_TAG;
                        ChartPerformanceEvaluationStatus = Constants.ASSESSNEMT_PENDING_STATUS;
                    }
                    //

                    //Supervisor Completed Status
                    if (GoalAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        //GoalAssessmentStatus = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorGoalAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG + @"</a>";
                        //drAssessment["GOAL_ASSESSMENT_STATUS"] = hyperlink;
                        GoalAssessmentStatus = hyperlink;
                        ChartGoalAssessmentStatus = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;
                    }
                    if (CompetencyAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG + @"</a>";
                        //drAssessment["COMPETENCY_ASSESSMENT_STATUS"] = hyperlink;
                        CompetencyAssessmentStatus = hyperlink;
                        ChartCompetencyAssessmentStatus = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;
                        //CompetencyAssessmentStatus = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;                        

                    }
                    if (SelfAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorSelfAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG + @"</a>";
                        //drAssessment["SELF_ASSESSMENT_STATUS"] = hyperlink;
                        SelfAssessmentStatus = hyperlink;
                        ChartSelfAssessmentStatus = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;
                        //SelfAssessmentStatus = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;
                    }

                    if (((GoalAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS) || (GoalAssessmentStatusCopy == "N/A")) && ((CompetencyAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS) || (CompetencyAssessmentStatusCopy == "N/A")) && ((SelfAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS) || (SelfAssessmentStatusCopy == "N/A")) && (PerformanceEvaluationStatusCopy == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS))
                    {
                        string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_ACTIVE_TAG + @"</a>";
                        //drAssessment["STATUS_CODE"] = hyperlink;
                        PerformanceEvaluationStatusCode = hyperlink;
                        ChartPerformanceEvaluationStatus = Constants.ASSESSNEMT_ACTIVE_STATUS;
                        //PerformanceEvaluationStatusCode = Constants.ASSESSNEMT_ACTIVE_STATUS;
                    }
                    //

                    //Supervisor Complete Evaluation
                    if (((GoalAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS) || (GoalAssessmentStatusCopy == "N/A")) && ((CompetencyAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS) || (CompetencyAssessmentStatusCopy == "N/A")) && ((SelfAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS) || (SelfAssessmentStatusCopy == "N/A")) && (PerformanceEvaluationStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS))
                    {
                        string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG + @"</a>";
                        //drAssessment["STATUS_CODE"] = hyperlink;
                        PerformanceEvaluationStatusCode = hyperlink;
                        ChartPerformanceEvaluationStatus = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;
                        //PerformanceEvaluationStatusCode = Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS;
                    }
                    //

                    //Subordinate Agreed Status
                    if ((isEmployeeAgreed == Constants.CON_ACTIVE_STATUS) && (PerformanceEvaluationStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS))
                    {
                        if (((GoalAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS) || (GoalAssessmentStatusCopy == "N/A")) && ((CompetencyAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS) || (CompetencyAssessmentStatusCopy == "N/A")) && ((SelfAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS) || (SelfAssessmentStatusCopy == "N/A")))
                        {
                            string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                            string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG + @"</a>";
                            //drAssessment["STATUS_CODE"] = hyperlink;
                            PerformanceEvaluationStatusCode = hyperlink;
                            ChartPerformanceEvaluationStatus = Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG;
                            //PerformanceEvaluationStatusCode = Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG;
                        }
                        else
                        {
                            PerformanceEvaluationStatusCode = Constants.ASSESSNEMT_PENDING_TAG;
                            ChartPerformanceEvaluationStatus = Constants.ASSESSNEMT_PENDING_STATUS;
                        }
                    }
                    //

                    //Subordinate Disagreed Status
                    if ((isEmployeeAgreed == Constants.CON_INACTIVE_STATUS) && (PerformanceEvaluationStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS))
                    {
                        if (((GoalAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS) || (GoalAssessmentStatusCopy == "N/A")) && ((CompetencyAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS) || (CompetencyAssessmentStatusCopy == "N/A")) && ((SelfAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS) || (SelfAssessmentStatusCopy == "N/A")))
                        {
                            string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                            string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG + @"</a>";
                            //drAssessment["STATUS_CODE"] = hyperlink;
                            PerformanceEvaluationStatusCode = hyperlink;
                            ChartPerformanceEvaluationStatus = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
                            //PerformanceEvaluationStatusCode = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS;
                        }
                        else
                        {
                            PerformanceEvaluationStatusCode = Constants.ASSESSNEMT_PENDING_TAG;
                            ChartPerformanceEvaluationStatus = Constants.ASSESSNEMT_PENDING_STATUS;
                        }
                    }
                    //

                    //Supervisor Finalized Status


                    if (GoalAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        //GoalAssessmentStatus = Constants.ASSESSNEMT_ACTIVE_STATUS;
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorGoalAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG + @"</a>";
                        GoalAssessmentStatus = hyperlink;
                        ChartGoalAssessmentStatus = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS;
                    }
                    if (CompetencyAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        //GoalAssessmentStatus = Constants.ASSESSNEMT_ACTIVE_STATUS;
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG + @"</a>";
                        CompetencyAssessmentStatus = hyperlink;
                        ChartCompetencyAssessmentStatus = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS;
                    }
                    if (SelfAssessmentStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        //SelfAssessmentStatus = Constants.ASSESSNEMT_ACTIVE_STATUS;
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorSelfAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG + @"</a>";
                        SelfAssessmentStatus = hyperlink;
                        ChartSelfAssessmentStatus = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS;
                    }


                    if (PerformanceEvaluationStatusCopy == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG + @"</a>";
                        //drAssessment["STATUS_CODE"] = hyperlink;
                        PerformanceEvaluationStatusCode = hyperlink;
                        ChartPerformanceEvaluationStatus = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS;
                        //PerformanceEvaluationStatusCode = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS;
                    }
                    //

                    //CEO Finalized Status

                    if (GoalAssessmentStatusCopy == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        //GoalAssessmentStatus = Constants.ASSESSNEMT_ACTIVE_STATUS;
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorGoalAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CEO_FINALIZED_TAG + @"</a>";
                        GoalAssessmentStatus = hyperlink;
                        ChartGoalAssessmentStatus = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                    }
                    if (CompetencyAssessmentStatusCopy == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        //GoalAssessmentStatus = Constants.ASSESSNEMT_ACTIVE_STATUS;
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CEO_FINALIZED_TAG + @"</a>";
                        CompetencyAssessmentStatus = hyperlink;
                        ChartCompetencyAssessmentStatus = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                    }
                    if (SelfAssessmentStatusCopy == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        //SelfAssessmentStatus = Constants.ASSESSNEMT_ACTIVE_STATUS;
                        string js = @"open('/PerformanceManagement/WebFrmSupervisorSelfAssessment.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CEO_FINALIZED_TAG + @"</a>";
                        SelfAssessmentStatus = hyperlink;
                        ChartSelfAssessmentStatus = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                    }


                    if (PerformanceEvaluationStatusCopy == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CEO_FINALIZED_TAG + @"</a>";
                        //drAssessment["STATUS_CODE"] = hyperlink;
                        PerformanceEvaluationStatusCode = hyperlink;
                        ChartPerformanceEvaluationStatus = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                        //PerformanceEvaluationStatusCode = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                    }
                    //

                    //Evaluation Closed Status
                    if (PerformanceEvaluationStatusCopy == Constants.ASSESSNEMT_CLOSED_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_CLOSED_TAG + @"</a>";
                        //drAssessment["STATUS_CODE"] = hyperlink;
                        PerformanceEvaluationStatusCode = hyperlink;
                        ChartPerformanceEvaluationStatus = Constants.ASSESSNEMT_CLOSED_STATUS;
                        //PerformanceEvaluationStatusCode = Constants.ASSESSNEMT_CLOSED_STATUS;
                    }
                    //

                    //Evaluation Obsolete Status
                    if (PerformanceEvaluationStatusCopy == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    {
                        string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(AssessmentID) + @"&year=" + crpto.Encrypt(YearOfAssessment) + @"&employeeId=" + crpto.Encrypt(SubordinateID) + @"', 'window', 'resizable=no,width=1000,height=768,scrollbars=yes,top=50,left=200,status=yes')";
                        string hyperlink = @"<a style='text-decoration: none;' onclick=""" + js + @"""' href='#'>" + Constants.ASSESSNEMT_OBSOLETE_TAG + @"</a>";
                        //drAssessment["STATUS_CODE"] = hyperlink;
                        PerformanceEvaluationStatusCode = hyperlink;
                        ChartPerformanceEvaluationStatus = Constants.ASSESSNEMT_OBSOLETE_STATUS;
                        //PerformanceEvaluationStatusCode = Constants.ASSESSNEMT_OBSOLETE_STATUS;
                    }
                    //

                    dtAssessmentDetails.Rows[i]["GOAL_ASSESSMENT_STATUS"] = GoalAssessmentStatus;
                    dtAssessmentDetails.Rows[i]["COMPETENCY_ASSESSMENT_STATUS"] = CompetencyAssessmentStatus;
                    dtAssessmentDetails.Rows[i]["SELF_ASSESSMENT_STATUS"] = SelfAssessmentStatus;
                    dtAssessmentDetails.Rows[i]["STATUS_CODE"] = PerformanceEvaluationStatusCode;

                    //Chart Data Set
                    dtCharts.Rows[i]["GOAL_ASSESSMENT_STATUS"] = ChartGoalAssessmentStatus;
                    dtCharts.Rows[i]["COMPETENCY_ASSESSMENT_STATUS"] = ChartCompetencyAssessmentStatus;
                    dtCharts.Rows[i]["SELF_ASSESSMENT_STATUS"] = ChartSelfAssessmentStatus;
                    dtCharts.Rows[i]["STATUS_CODE"] = ChartPerformanceEvaluationStatus;
                }


                Session["dtDisplayDataList"] = dtCharts.Copy();
                DisplayCharts();
                dtCharts.Dispose();

                return dtAssessmentDetails;
            }
            catch (Exception ex)
            {
                log.Error("SetSuperviorStatus()" + ex.Message);
                throw ex;
            }
            finally
            {
                crpto = null;
            }
        }

        #endregion

    }
}