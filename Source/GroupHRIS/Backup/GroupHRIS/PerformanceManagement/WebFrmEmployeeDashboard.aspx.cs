using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.PerformanceManagement;
using GroupHRIS.Utility;
using Common;
using DataHandler.Userlogin;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmEmployeeDashboard : System.Web.UI.Page
    {
        public string urlstr = String.Empty;
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmEmployeeDashboard : Page_Load");
            EmployeeAssessmentDashboardDataHandler oEmployeeAssessmentDashboardDataHandler = new EmployeeAssessmentDashboardDataHandler();

            if (!IsPostBack)
            {
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                fillStatus();
                fillYear();
                assessmentTypedt();

                viewlatestAssessment(KeyEMPLOYEE_ID);
                getLatestAssmentStatus(KeyEMPLOYEE_ID);

                string assessYear = ddlYear.SelectedValue;
                grdAssessment.DataSource = oEmployeeAssessmentDashboardDataHandler.getAllAvailableAssessmentList(KeyEMPLOYEE_ID, assessYear);
                grdAssessment.DataBind();

                Session["DASHBOARD"] = "EMP";//For indentify the performance  evaluation user view
            }
        }

        protected void grdAssessment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdAssessment, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMessage);
            }
        }

        protected void grdAssessment_SelectedIndexChanged(object sender, EventArgs e)
        {
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            try
            {
                clearFields();

                int SelectedIndex = grdAssessment.SelectedIndex;
                string assessmentId = Server.HtmlDecode(grdAssessment.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());
                Session["assessmentId"] = assessmentId;

                string assessmentName = Server.HtmlDecode(grdAssessment.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
                lblAssessmentName.Text = assessmentName;

                string assessmentType = Server.HtmlDecode(grdAssessment.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());
                lblAssessmentType.Text = assessmentType;

                string assessmentStatus = Server.HtmlDecode(grdAssessment.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
                lbPerformanceEval.Text = assessmentStatus;

                string selfAssStatus = Server.HtmlDecode(grdAssessment.Rows[SelectedIndex].Cells[4].Text.ToString().Trim());
                hfSelfAssessment.Value = selfAssStatus;

                string CompetencyAssStatus = Server.HtmlDecode(grdAssessment.Rows[SelectedIndex].Cells[5].Text.ToString().Trim());
                hfCompetencyAss.Value = CompetencyAssStatus;

                string goalAssStatus = Server.HtmlDecode(grdAssessment.Rows[SelectedIndex].Cells[6].Text.ToString().Trim());
                hfgoalAssessment.Value = goalAssStatus;

                string yearId = ddlYear.SelectedValue;

                getAssessmentStatus(assessmentStatus, goalAssStatus, CompetencyAssStatus, selfAssStatus, assessmentId, KeyEMPLOYEE_ID, yearId);
                getAssessmentSelectedStatus();

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdAssessment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            EmployeeAssessmentDashboardDataHandler oEmployeeAssessmentDashboardDataHandler = new EmployeeAssessmentDashboardDataHandler();
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            string assessmentYear = ddlYear.SelectedValue;
            string assessmentStatus = ddlStatus.SelectedValue;
            string empStatus = "";

            if (assessmentStatus == "7" )
            {
                assessmentStatus = "3";
                empStatus = "0";
            }
            else if (assessmentStatus == "8")
            {
                assessmentStatus = "3";
                empStatus = "1";
            }
            else if (assessmentStatus == "3")
            {
                assessmentStatus = "3";
                empStatus = "null";
            }

            try
            {
                clearFields();
                clearLink();
                if (ddlStatus.SelectedIndex == 0)
                {
                    grdAssessment.DataSource = oEmployeeAssessmentDashboardDataHandler.getAllAvailableAssessmentList(KeyEMPLOYEE_ID, assessmentYear);
                    grdAssessment.DataBind();
                }
                else
                {
                    grdAssessment.DataSource = oEmployeeAssessmentDashboardDataHandler.getAvailableAssessmentList(KeyEMPLOYEE_ID, assessmentYear, assessmentStatus, empStatus);
                    grdAssessment.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oEmployeeAssessmentDashboardDataHandler = null;
            }
        }

        //protected void lbgoalAssessment_Click(object sender, EventArgs e)
        //{
        //    PasswordHandler crpto = new PasswordHandler();

        //    try
        //    {
        //        string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
        //        string assessmentId = (string)Session["assessmentId"];
        //        string yearId = ddlYear.SelectedValue;

        //        if (hfgoalAssessment.Value == "1")
        //        {
        //            string url = "../PerformanceManagement/WebFrmViewGoalAssessment.aspx?assmtId=" + crpto.Encrypt(assessmentId) + "&year=" + crpto.Encrypt(yearId) + "&employeeId="+crpto.Encrypt(KeyEMPLOYEE_ID)+"";
        //            string s = "window.open('" + url + "', 'popup_window', 'width=950,height=800,left=100,top=100,resizable=no');";
        //            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

        //        }
        //        else
        //        {
        //            lbgoalAssessment.Enabled = false;
        //        }

        //        //string TableString = "";
        //        //TableString += @"<table style='width:100%;height:100%'> ";
        //        //TableString += "<tr><td><a href=\"#" + "\"OnClick=\"window.open('../PerformanceManagement/WebFrmViewQuestions.aspx?','Search','hfisInclude')" + "\"?style = \"text-decoration:none\"" + "></a>";
        //        //TableString += @"</table>";

        //        ////lbgoalAssessment.Text = string.Empty;
        //        //HttpUtility.HtmlDecode(TableString);

        //    }
        //    catch (Exception ex)
        //    {
        //        CommonVariables.MESSAGE_TEXT = ex.Message;
        //        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
        //    }
        //}

        //protected void lbCompAssessment_Click(object sender, EventArgs e)
        //{
        //    PasswordHandler crpto = new PasswordHandler();
        //    try
        //    {   string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
        //        string assessmentId = (string)Session["assessmentId"];
        //        string yearId = ddlYear.SelectedValue;

        //        if (hfCompetencyAss.Value == "1")
        //        {
        //            string url = "../PerformanceManagement/WebFrmCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(assessmentId) + "&year=" + crpto.Encrypt(yearId) + "&employeeId="+crpto.Encrypt(KeyEMPLOYEE_ID)+"";
        //            string s = "window.open('" + url + "', 'popup_window', 'width=950,height=800,left=100,top=100,resizable=yes');";
        //            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        //        }
        //        else
        //        {
        //            lbCompAssessment.Enabled = false;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //protected void lbSelfAssessment_Click(object sender, EventArgs e)
        //{
        //    PasswordHandler crpto = new PasswordHandler();
        //    try
        //    {

        //        if (hfSelfAssessment.Value == "1")
        //        {
        //            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
        //            string assessmentId = (string)Session["assessmentId"];
        //            string yearId = ddlYear.SelectedValue;

        //            string url = "../PerformanceManagement/WebFrmViewSelfAssessment.aspx?assmtId=" + crpto.Encrypt(assessmentId) + "&year=" + crpto.Encrypt(yearId) + "";
        //            string s = "window.open('" + url + "', 'Search', 'width=950,height=800,left=100,top=100,resizable=no');";
        //            //HttpUtility.HtmlDecode(s);
        //            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        //        }
        //        else
        //        {
        //            lbSelfAssessment.Enabled = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonVariables.MESSAGE_TEXT = ex.Message;
        //        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
        //    }
        //    finally
        //    {
        //        crpto = null;
        //    }
        //}

        protected void lbPerformanceEval_Click(object sender, EventArgs e)
        {
            try
            {
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);

                string url = "../PerformanceManagement/WebFrmViewPerformanceEvaluation.aspx";
                string s = "window.open('" + url + "', 'popup_window', 'width=950,height=800,left=100,top=100,resizable=yes');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }



        public void assessmentTypedt()
        {
            DataTable questionBucket = new DataTable();
            questionBucket.Columns.Add("ASSESSMENT_TYPE", typeof(string));
            questionBucket.Columns.Add("AVAILABILITY", typeof(string));

            questionBucket.Rows.Add("Goal Assessment", "N/A");
            questionBucket.Rows.Add("Competency Self-Assessment", "N/A");
            questionBucket.Rows.Add("Self-Assessment Questioner", "N/A");
            questionBucket.Rows.Add("Performance Evaluation", "N/A");

            Session["assessmentBucket"] = questionBucket;
        }

        public void fillStatus()
        {
            try
            {
                ddlStatus.Items.Insert(0, new ListItem("", ""));
                ddlStatus.Items.Insert(1, new ListItem(Constants.ASSESSNEMT_ACTIVE_TAG, Constants.ASSESSNEMT_ACTIVE_STATUS));
                // ddlStatus.Items.Insert(2, new ListItem(Constants.ASSESSNEMT_OBSOLETE_TAG, Constants.ASSESSNEMT_OBSOLETE_STATUS));
                //ddlStatus.Items.Insert(2, new ListItem(Constants.ASSESSNEMT_PENDING_TAG, Constants.ASSESSNEMT_PENDING_STATUS));
                ddlStatus.Items.Insert(2, new ListItem(Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG, Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS));
                ddlStatus.Items.Insert(3, new ListItem(Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_TAG, Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS));
                ddlStatus.Items.Insert(4, new ListItem(Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG, Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS));
                ddlStatus.Items.Insert(5, new ListItem(Constants.ASSESSNEMT_CEO_FINALIZED_TAG, Constants.ASSESSNEMT_CEO_FINALIZED_STATUS));
                ddlStatus.Items.Insert(6, new ListItem(Constants.ASSESSNEMT_CLOSED_TAG, Constants.ASSESSNEMT_CLOSED_STATUS));
                ddlStatus.Items.Insert(7, new ListItem(Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG, Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS));
                ddlStatus.Items.Insert(8, new ListItem(Constants.ASSESSNEMT_SUBORDINATE_AGREE_TAG, Constants.ASSESSNEMT_SUBORDINATE_AGREE_STATUS));
                
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        public void fillYear()
        {
            try
            {
                ddlYear.Items.Clear();

                string currentYear = (CommonUtils.currentFinancialYear());
                int dt = Int32.Parse(currentYear);

                for (int i = 0; i >= -5; i--)
                {
                    // Now just add an entry that's the current year plus the counter
                    ddlYear.Items.Add((dt + i).ToString());
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        public void clearFields()
        {
            hfCompetencyAss.Value = "";
            hfgoalAssessment.Value = "";
            hfSelfAssessment.Value = "";

        }

        public void clearLink()
        {
            lblSelfAssessment.Text = "";
            lblCompAssessment.Text = "";
            lblgoalAssessment.Text = "";
            lbPerformanceEval.Text = "";

            lblAssessmentName.Text = "";
            lblAssessmentType.Text = "";
            litPurpose.Text = "";
        }

        public void viewlatestAssessment(string emp)
        {
            EmployeeAssessmentDashboardDataHandler oEmployeeAssessmentDashboardDataHandler = new EmployeeAssessmentDashboardDataHandler();
            DataTable dt = new DataTable();
            int count = 0;
            try
            {
                dt = oEmployeeAssessmentDashboardDataHandler.getLatestAssessment(emp);
                string TableString = "";

                TableString += @"<table style='width:100%;height:100%'>";

                if (dt.Rows.Count > 0)
                {
                    Session["LatestAssessment"] = dt.Rows[0]["ASSESSMENT_ID"].ToString();
                    lblAssessmentName.Text = dt.Rows[0]["ASSESSMENT_NAME"].ToString();
                    lblAssessmentType.Text = dt.Rows[0]["ASSESSMENT_TYPE_NAME"].ToString();

                    for (int x = 0; x < dt.Rows.Count; x++)
                    {
                        count = count + 1;
                        Label lbl = new Label();
                        lbl.Text = dt.Rows[x]["NAME"].ToString();
                        if (count == dt.Rows.Count)
                        {
                            TableString += "<tr><td rowspan=" + dt.Rows.Count + ">" + lbl.Text;
                        }
                        else
                        {
                            TableString += lbl.Text + " / </td></tr>";
                        }
                    }
                }
                TableString += @"</table>";

                litPurpose.Text = string.Empty;
                litPurpose.Text = HttpUtility.HtmlDecode(TableString);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oEmployeeAssessmentDashboardDataHandler = null;
                dt.Dispose();
            }
        }

        public void getAssessmentStatus(string assStatus, string goalAssStatus, string CompetencyAssStatus, string selfAssStatus, string assessmentId, string KeyEMPLOYEE_ID, string yearId)
        {
            EmployeeAssessmentDashboardDataHandler oEmployeeAssessmentDashboardDataHandler = new EmployeeAssessmentDashboardDataHandler();

            PasswordHandler crpto = new PasswordHandler();
            try
            {
                if (goalAssStatus == "1")
                {
                    string goalStatus = oEmployeeAssessmentDashboardDataHandler.getGoalStatus(assessmentId, KeyEMPLOYEE_ID, yearId);
                    string js = @"open('/PerformanceManagement/WebFrmViewGoalAssessment.aspx?assmtId=" + crpto.Encrypt(assessmentId) + @"&year=" + crpto.Encrypt(yearId) + "&employeeId=" + crpto.Encrypt(KeyEMPLOYEE_ID) + @"', 'window', 'resizable=no,width=1000,height=650,scrollbars=yes,top=5,left=200,status=yes')";

                    if (goalStatus == null || goalStatus == "")
                    {
                        string hyperlink = @"<a onclick=""" + js + @"""' href='#'>" + "Active" + @"</a>";
                        lblgoalAssessment.Text = hyperlink;
                    }
                    else
                    {
                        string hyperlink = @"<a onclick=""" + js + @"""' href='#'>" + goalStatus + @"</a>";
                        lblgoalAssessment.Text = hyperlink;
                    }

                }
                else
                {
                    lblgoalAssessment.Text = "N/A";
                }

                if (CompetencyAssStatus == "1")
                {
                    string competencyStatus = oEmployeeAssessmentDashboardDataHandler.getCompetencyStatus(assessmentId, KeyEMPLOYEE_ID, yearId);
                    string js = @"open('/PerformanceManagement/WebFrmCompetencyAssessment.aspx?assmtId=" + crpto.Encrypt(assessmentId) + @"&year=" + crpto.Encrypt(yearId) + "&employeeId=" + crpto.Encrypt(KeyEMPLOYEE_ID) + @"', 'window', 'resizable=no,width=1000,height=650,scrollbars=yes,top=5,left=200,status=yes')";

                    if (competencyStatus == null || competencyStatus == "")
                    {
                        string hyperlink = @"<a onclick=""" + js + @"""' href='#'>" + "Active" + @"</a>";
                        lblCompAssessment.Text = hyperlink;
                    }
                    else
                    {
                        string hyperlink = @"<a onclick=""" + js + @"""' href='#'>" + competencyStatus + @"</a>";

                        lblCompAssessment.Text = hyperlink;
                    }
                }
                else
                {
                    lblCompAssessment.Text = "N/A";
                }

                if (selfAssStatus == "1")
                {
                    string selfAssessStatus = oEmployeeAssessmentDashboardDataHandler.getSelfAssessmentStatus(assessmentId, KeyEMPLOYEE_ID, yearId);
                    string js = @"open('/PerformanceManagement/WebFrmViewSelfAssessment.aspx?assmtId=" + crpto.Encrypt(assessmentId) + @"&year=" + crpto.Encrypt(yearId) + @"', 'window', 'resizable=no,width=1000,height=650,scrollbars=yes,top=5,left=200,status=yes')";

                    if (selfAssessStatus == null || selfAssessStatus == "")
                    {
                        string hyperlink = @"<a onclick=""" + js + @"""' href='#'>" + "Active" + @"</a>";

                        lblSelfAssessment.Text = hyperlink;
                    }
                    else
                    {
                        string hyperlink = @"<a onclick=""" + js + @"""' href='#'>" + selfAssessStatus + @"</a>";

                        lblSelfAssessment.Text = hyperlink;

                    }
                }
                else
                {
                    lblSelfAssessment.Text = "N/A";
                }

                if (assStatus != "0")
                {
                    //string goalStatus = oEmployeeAssessmentDashboardDataHandler.getGoalStatus(assessmentId, KeyEMPLOYEE_ID, yearId);
                    string js = @"open('/PerformanceManagement/WebFrmEvaluationSummary.aspx?assmtId=" + crpto.Encrypt(assessmentId) + @"&year=" + crpto.Encrypt(yearId) + "&employeeId=" + crpto.Encrypt(KeyEMPLOYEE_ID) + @"', 'window', 'resizable=no,width=1000,height=650,scrollbars=yes,top=5,left=200,status=yes')";

                    if (assStatus == null || assStatus == "")
                    {
                        string hyperlink = @"<a onclick=""" + js + @"""' href='#'>" + "Active" + @"</a>";
                        lbPerformanceEval.Text = hyperlink;
                    }
                    else
                    {
                        if (assStatus == "Pending" || assStatus == "Active" || assStatus == "Obsolete" || assStatus == "Subordinate Finalized" || assStatus == "Supervisor Completed")
                        {
                            if (assStatus == "Supervisor Completed")
                            {
                                string isFeedback = oEmployeeAssessmentDashboardDataHandler.isFeedbackGiven(assessmentId, KeyEMPLOYEE_ID, yearId);
                                if (isFeedback != null || isFeedback != "")
                                {
                                    string hyperlink = @"<a onclick=""" + js + @"""' href='#'>" + "Active" + @"</a>";
                                    lbPerformanceEval.Text = hyperlink;
                                }
                                else
                                {
                                    lbPerformanceEval.Text = "Pending";
                                }
                            }
                            else
                            {
                                lbPerformanceEval.Text = "Pending";
                            }
                        }
                        else
                        {
                            string hyperlink = @"<a onclick=""" + js + @"""' href='#'>" + assStatus + @"</a>";
                            lbPerformanceEval.Text = hyperlink;
                        }
                    }

                }
                else
                {

                    lbPerformanceEval.Text = "N/A";

                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oEmployeeAssessmentDashboardDataHandler = null;
                crpto = null;
            }
        }

        public void getLatestAssmentStatus(string emp)
        {
            EmployeeAssessmentDashboardDataHandler oEmployeeAssessmentDashboardDataHandler = new EmployeeAssessmentDashboardDataHandler();
            DataTable dt = new DataTable();
            DataTable statusDt = new DataTable();
            try
            {
                string yearId = ddlYear.SelectedValue;
                dt = oEmployeeAssessmentDashboardDataHandler.getLatestAssessment(emp);

                if (dt.Rows.Count > 0)
                {
                    string assmentId = dt.Rows[0]["ASSESSMENT_ID"].ToString();
                    statusDt = oEmployeeAssessmentDashboardDataHandler.latestAssessmentAvalability(assmentId, emp);

                    if (statusDt.Rows.Count > 0)
                    {
                        string selfAss = dt.Rows[0]["INCLUDE_SELF_ASSESSMENT"].ToString();
                        string comAss = dt.Rows[0]["INCLUDE_COMPITANCY_ASSESSMENT"].ToString();
                        string goalfAss = dt.Rows[0]["INCLUDE_GOAL_ASSESSMENT"].ToString();
                        string performanceStatus = dt.Rows[0]["STATUS_CODE"].ToString();

                        lbPerformanceEval.Text = performanceStatus;
                        hfSelfAssessment.Value = selfAss;
                        hfCompetencyAss.Value = comAss;
                        hfgoalAssessment.Value = goalfAss;

                        getAssessmentStatus(performanceStatus, goalfAss, comAss, selfAss, assmentId, emp, yearId);
                        Session["assessmentId"] = assmentId;

                    }

                }


            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oEmployeeAssessmentDashboardDataHandler = null;
                dt.Dispose();
                statusDt.Dispose();
            }
        }

        public void getAssessmentSelectedStatus()
        {
            EmployeeAssessmentDashboardDataHandler oEmployeeAssessmentDashboardDataHandler = new EmployeeAssessmentDashboardDataHandler();
            DataTable dt = new DataTable();
            int count = 0;
            try
            {
                dt = oEmployeeAssessmentDashboardDataHandler.getAssessmentPurposes(Session["assessmentId"].ToString());

                string TableString = "";

                TableString += @"
                                <table style='width:100%;height:100%'>";

                if (dt.Rows.Count > 0)
                {
                    for (int x = 0; x < dt.Rows.Count; x++)
                    {
                        count = count + 1;
                        Label lbl = new Label();
                        lbl.Text = dt.Rows[x]["NAME"].ToString();
                        if (count == dt.Rows.Count)
                        {
                            TableString += "<tr><td rowspan=" + dt.Rows.Count + ">" + lbl.Text;
                        }
                        else
                        {
                            TableString += lbl.Text + " / </td></tr>";
                        }
                    }

                }
                TableString += @"
                                </table>
                            ";

                litPurpose.Text = string.Empty;
                litPurpose.Text = HttpUtility.HtmlDecode(TableString);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oEmployeeAssessmentDashboardDataHandler = null;
                dt.Dispose();
            }
        }


    }
}