using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.Userlogin;
using System.Data;
using DataHandler.PerformanceManagement;
using System.Web.Services;
using DataHandler.Utility;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmSupervisorGoalAssessment : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        public String Questions = String.Empty;
        public String DisplayAssessmentName = String.Empty;
        public String DisplayAssessmentPurposes = String.Empty;
        public DataTable dtRatings = new DataTable();

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            PasswordHandler crpto;
            try
            {
                sIPAddress = Request.UserHostAddress;
                log.Debug("IP:" + sIPAddress + "webFrmEmployee : Page_Load");

                if (Session["KeyLOGOUT_STS"].Equals("0"))
                {
                    CommonVariables.MESSAGE_TEXT = "Session Expired";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                }
                else
                {
                    if (!IsPostBack)
                    {
                        crpto = new PasswordHandler();

                        string AssessmentID = Request.QueryString["assmtId"];
                        string EmployeeID = Request.QueryString["employeeId"];
                        string YearOfAssessmentID = Request.QueryString["year"];

                        hfAssesmentID.Value = crpto.Decrypt(AssessmentID);
                        hfEmployeeID.Value = crpto.Decrypt(EmployeeID);
                        hfYearofAssessment.Value = crpto.Decrypt(YearOfAssessmentID);

                        loadGoalAssessment();
                        StatusCheck();
                        setWeights();

                        DisplayFinalizeMessage();
                        string x = lblFinalizeNotice.Text;

                        Label1.Text = x;

                    }
                    LoadAssessmentPurposes();
                }
                DisplayFinalizeMessage();

                DisplayChart();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                crpto = null;
            }
        }

        protected void grdvReportGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                log.Debug("grdvReportGrid_RowDataBound()");

                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    e.Row.Cells[7].Text = e.Row.Cells[7].Text.Replace("\r\n", "<br />");


                    string SupervisorScore = HttpUtility.HtmlDecode(e.Row.Cells[7].Text).Trim().Replace(" ", "");
                    TextBox txtAchievedWeight = (e.Row.FindControl("txtAchievedWeight") as TextBox);
                    txtAchievedWeight.Text = SupervisorScore;

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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SupervisorGoalAssessmentDataHandler SGADH = new SupervisorGoalAssessmentDataHandler();
            DataTable dtEmployeeGoals = new DataTable();
            try
            {
                log.Debug("btnSave_Click()");

                setWeights();

                string AssessmentToken = grdvReportGrid.Rows[0].Cells[0].Text.Trim();
                string Comments = txtSupervisorComment.Text.Trim();
                string ModifiedBy = (Session["KeyEMPLOYEE_ID"] as string);
                string SubordinatesEmployeeID = hfEmployeeID.Value.Trim();
                string AssessmentID = hfAssesmentID.Value.Trim();
                string YearOfAssessment = hfYearofAssessment.Value.Trim();


                dtEmployeeGoals.Columns.Add("GOAL_ID");
                dtEmployeeGoals.Columns.Add("SUPERVISOR_SCORE");

                double TotalSupervisorAchievment = 0.0;

                for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                {
                    string GoalID = grdvReportGrid.Rows[i].Cells[1].Text.Trim();

                    TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                    txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");

                    string SupervisorScore = txtAchievedWeight.Text.Trim();

                    if (SupervisorScore != String.Empty)
                    {
                        double tempSupervisorScore = 0.0;
                        if (Double.TryParse(SupervisorScore, out tempSupervisorScore))
                        {
                            TotalSupervisorAchievment += tempSupervisorScore;
                        }
                        else
                        {
                            Label1.Text = string.Empty;
                            txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ecb50c");
                            CommonVariables.MESSAGE_TEXT = "Invalid Achievement";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                            return;
                        }
                    }

                    string PlannedScore = grdvReportGrid.Rows[i].Cells[4].Text.Trim();
                    if (SupervisorScore != String.Empty)
                    {
                        if (Convert.ToDouble(PlannedScore) < Convert.ToDouble(SupervisorScore))
                        {
                            Label1.Text = string.Empty;
                            txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ecb50c");
                            CommonVariables.MESSAGE_TEXT = "Supervisor's score should be less than or equal to planned score";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                            return;
                        }
                    }

                    DataRow drGoalScore = dtEmployeeGoals.NewRow();
                    drGoalScore["GOAL_ID"] = GoalID;
                    drGoalScore["SUPERVISOR_SCORE"] = SupervisorScore;
                    dtEmployeeGoals.Rows.Add(drGoalScore);
                }

                Boolean isAllEmpty = true;
                for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                {
                    TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                    if (txtAchievedWeight.Text != String.Empty)
                    {
                        isAllEmpty = false;
                        break;
                    }
                }
                if ((isAllEmpty == true) || (TotalSupervisorAchievment == 0.0))
                {
                    Label1.Text = string.Empty;
                    CommonVariables.MESSAGE_TEXT = "At least one achievement should be enter before save";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                    return;
                }

                SGADH.Insert(AssessmentToken, Comments, ModifiedBy, SubordinatesEmployeeID, AssessmentID, YearOfAssessment, dtEmployeeGoals.Copy());
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    DisplayFinalizeMessage();
                    string x = lblFinalizeNotice.Text;

                    Label1.Text = x;
                    CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);
                }
                else
                {
                    DisplayFinalizeMessage();
                    string x = lblFinalizeNotice.Text;

                    Label1.Text = x;
                    CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);
                }

                loadGoalAssessment();
                StatusCheck();
                setWeights();

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SGADH = null;
                dtEmployeeGoals.Dispose();
            }
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            SupervisorGoalAssessmentDataHandler SGADH = new SupervisorGoalAssessmentDataHandler();
            DataTable dtEmployeeGoals = new DataTable();
            try
            {
                log.Debug("btnComplete_Click()");

                for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                {
                    TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                    txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                    if (txtAchievedWeight.Text == String.Empty)
                    {
                        Label1.Text = string.Empty;
                        CommonVariables.MESSAGE_TEXT = "All weights are required to complete the goal assessment.";
                        txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ecb50c");

                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                        return;
                    }
                }

                for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                {
                    TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                    txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                    string PlannedWeight = HttpUtility.HtmlDecode(grdvReportGrid.Rows[i].Cells[4].Text).Trim();

                    double SupervisorWeight = 0.0;
                    double MaxWeight = 0.0;

                    Double.TryParse(txtAchievedWeight.Text, out SupervisorWeight);
                    Double.TryParse(PlannedWeight, out MaxWeight);

                    if (MaxWeight < SupervisorWeight)
                    {
                        Label1.Text = string.Empty;
                        txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ecb50c");
                        CommonVariables.MESSAGE_TEXT = "Supervisor's score should be less than or equal to planned score.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                        return;
                    }
                }

                if (txtSupervisorComment.Text == String.Empty)
                {
                    Label1.Text = string.Empty;
                    CommonVariables.MESSAGE_TEXT = "Supervisor comment is required";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                    return;
                }

                setWeights();

                string AssessmentToken = grdvReportGrid.Rows[0].Cells[0].Text.Trim();
                string Comments = txtSupervisorComment.Text.Trim();
                string ModifiedBy = (Session["KeyEMPLOYEE_ID"] as string);
                string SubordinatesEmployeeID = hfEmployeeID.Value.Trim();
                string AssessmentID = hfAssesmentID.Value.Trim();
                string YearOfAssessment = hfYearofAssessment.Value.Trim();


                dtEmployeeGoals.Columns.Add("GOAL_ID");
                dtEmployeeGoals.Columns.Add("SUPERVISOR_SCORE");

                for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                {
                    string GoalID = grdvReportGrid.Rows[i].Cells[1].Text.Trim();
                    TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                    string SupervisorScore = txtAchievedWeight.Text.Trim();

                    DataRow drGoalScore = dtEmployeeGoals.NewRow();
                    drGoalScore["GOAL_ID"] = GoalID;
                    drGoalScore["SUPERVISOR_SCORE"] = SupervisorScore;
                    dtEmployeeGoals.Rows.Add(drGoalScore);

                    double temp = 0.0;
                    if (Double.TryParse(txtAchievedWeight.Text.Trim(), out temp))
                    {

                    }
                    else
                    {
                        Label1.Text = string.Empty;
                        txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ecb50c");
                        CommonVariables.MESSAGE_TEXT = "Invalid Achievement";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);                        
                        return;
                    }

                }

                if (IsEmpty())
                {
                    Label1.Text = string.Empty;
                    CommonVariables.MESSAGE_TEXT = "Supervisor Weights are Required";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                    return;
                }

                //txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ecb50c");
                //CommonVariables.MESSAGE_TEXT = "Invalid Achievement";

                SGADH.Complete(AssessmentToken, Comments, ModifiedBy, SubordinatesEmployeeID, AssessmentID, YearOfAssessment, dtEmployeeGoals.Copy());

                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_COMPLETED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);

                //Update Assessment Status
                //UtilsDataHandler UDH = new UtilsDataHandler();
                //UDH.updateAssessmentStatus(hfAssesmentID.Value, hfEmployeeID.Value, hfYearofAssessment.Value);

                DisplayFinalizeMessage();

                Label1.Text = String.Empty;
                lblFinalizeNotice.Text = String.Empty;

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SGADH = null;
                dtEmployeeGoals.Dispose();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("btnClear_Click()");

                Utility.Errorhandler.ClearError(lblStatus);
                for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                {
                    TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                    txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                    txtAchievedWeight.Text = String.Empty;
                    txtSupervisorComment.Text = String.Empty;
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

        #endregion

        #region Methods

        void loadGoalAssessment()
        {
            DataTable dtEmployeeGoals = new DataTable();
            SupervisorGoalAssessmentDataHandler SGADH = new SupervisorGoalAssessmentDataHandler();
            try
            {
                log.Debug("loadGoalAssessment()");

                if ((hfEmployeeID.Value.ToString() != "") && (hfAssesmentID.Value.ToString() != "") && (hfYearofAssessment.Value.ToString() != ""))
                {
                    dtEmployeeGoals = SGADH.Populate(hfAssesmentID.Value.ToString(), hfEmployeeID.Value.ToString(), hfYearofAssessment.Value.ToString()).Copy();
                    grdvReportGrid.DataSource = dtEmployeeGoals.Copy();
                    grdvReportGrid.DataBind();
                    Boolean isSupervisorWeightNotEmpty = false;
                    for (int i = 0; i < dtEmployeeGoals.Rows.Count; i++)
                    {
                        string supGoal = dtEmployeeGoals.Rows[i]["SUPERVISOR_SCORE"].ToString();
                        if (supGoal != String.Empty)
                        {
                            isSupervisorWeightNotEmpty = true;
                            break;
                        }
                    }

                    if (isSupervisorWeightNotEmpty == true)
                    {
                        btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                    }
                    else
                    {
                        btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                    }

                    if (dtEmployeeGoals.Rows.Count > 0)
                    {
                        txtSupervisorComment.Text = dtEmployeeGoals.Rows[0]["COMMENTS"].ToString();

                        txtSupervisorComment.Visible = true;
                        btnComplete.Visible = true;
                        btnSave.Visible = true;
                        btnClear.Visible = true;
                    }
                    else
                    {
                        txtSupervisorComment.Visible = false;
                        btnComplete.Visible = false;
                        btnSave.Visible = false;
                        btnClear.Visible = false;
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
                dtEmployeeGoals.Dispose();
                SGADH = null;
            }
        }

        void setWeights()
        {
            double PlannedTotal = 0.0;
            double SubordinatesTotal = 0.0;
            double SupervisorTotal = 0.0;
            try
            {
                log.Debug("setWeights()");

                for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                {
                    TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);


                    double PlannedGoalScore = 0.0;
                    if (Double.TryParse((grdvReportGrid.Rows[i].Cells[4].Text), out PlannedGoalScore))
                    {
                        PlannedTotal += PlannedGoalScore;
                    }
                    double SubordinatesGoalScore = 0.0;
                    if (Double.TryParse((grdvReportGrid.Rows[i].Cells[5].Text), out SubordinatesGoalScore))
                    {
                        SubordinatesTotal += SubordinatesGoalScore;
                    }
                    double SupervisorGoalScore = 0.0;
                    if (Double.TryParse(txtAchievedWeight.Text, out SupervisorGoalScore))
                    {
                        SupervisorTotal += SupervisorGoalScore;
                    }
                }

                lblSubScore.Text = SubordinatesTotal.ToString();
                lblSupScore.Text = SupervisorTotal.ToString();
                lblTotalPlanned.Text = PlannedTotal.ToString();

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

        void StatusCheck()
        {
            SupervisorGoalAssessmentDataHandler SHADH = new SupervisorGoalAssessmentDataHandler();
            try
            {
                log.Debug("StatusCheck()");

                string AssessmentID = hfAssesmentID.Value.Trim();
                string EmployeeID = hfEmployeeID.Value.Trim();
                string YearOfAssessment = hfYearofAssessment.Value.Trim();

                if (SHADH.IsSubordinateFinalized(AssessmentID, EmployeeID, YearOfAssessment))
                {
                    for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                    {
                        TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                        txtAchievedWeight.Enabled = false;
                    }
                    txtSupervisorComment.Enabled = false;
                    btnComplete.Enabled = false;
                    btnSave.Enabled = false;
                    btnClear.Enabled = false;
                }
                else
                {
                    for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                    {
                        TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                        txtAchievedWeight.Enabled = true;
                    }
                    txtSupervisorComment.Enabled = true;
                    btnComplete.Enabled = true;
                    btnClear.Enabled = true;
                    btnSave.Enabled = true;
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

        Boolean IsEmpty()
        {
            Boolean isEmpty = true;
            try
            {
                log.Debug("IsEmpty()");

                Utility.Errorhandler.ClearError(lblStatus);
                double SupervisorTotalScore = 0.0;

                for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                {
                    TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                    double SupervisorGoalScore = 0.0;
                    if (Double.TryParse(txtAchievedWeight.Text, out SupervisorGoalScore))
                    {
                        SupervisorTotalScore += SupervisorGoalScore;
                    }
                }

                if (SupervisorTotalScore > 0)
                {
                    isEmpty = false;
                }
                else
                {
                    isEmpty = true;
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
            return isEmpty;
        }

        void LoadAssessmentPurposes()
        {
            SupervisorCompetencyAssessmentDataHandler SEADH;
            DataTable AssessmentPurposes = new DataTable();
            DataTable SubordinatesInfo = new DataTable();
            try
            {
                log.Debug("LoadAssessmentPurposes()");

                SEADH = new SupervisorCompetencyAssessmentDataHandler();
                AssessmentPurposes = SEADH.PopulateAssessmentPurposes(hfAssesmentID.Value.Trim()).Copy();
                SubordinatesInfo = SEADH.PopulateSubordinatesInfo(hfEmployeeID.Value.Trim()).Copy();
                if ((AssessmentPurposes.Rows.Count > 0) && (SubordinatesInfo.Rows.Count > 0))
                {

                    string SubName = SubordinatesInfo.Rows[0]["TITLE"].ToString() + " " + SubordinatesInfo.Rows[0]["INITIALS_NAME"].ToString();
                    string SubDesignation = SubordinatesInfo.Rows[0]["DESIGNATION_NAME"].ToString();
                    string SubRole = SubordinatesInfo.Rows[0]["ROLE_NAME"].ToString();

                    DisplayAssessmentName += @"
                                                    <table style='margin: auto;'>
                                                        <tr>
                                                            <td style='text-align: right;'>Assessment</td>
                                                            <td>:</td>
                                                            <td style='text-align: left;'>" + AssessmentPurposes.Rows[0]["ASSESSMENT_NAME"].ToString() + @"</td>
                                                        </tr>
                                                        <tr>
                                                            <td style='text-align: right;'>Employee</td>
                                                            <td>:</td>
                                                            <td style='text-align: left;'>" + SubName + @"</td>
                                                        </tr>
                                                        <tr>
                                                            <td style='text-align: right;'>Designation</td>
                                                            <td>:</td>
                                                            <td style='text-align: left;'>" + SubDesignation + @"</td>
                                                        </tr>
                                                        <tr>
                                                            <td style='text-align: right;'>Role</td>
                                                            <td>:</td>
                                                            <td style='text-align: left;'>" + SubRole + @"</td>
                                                        </tr>
                                                    </table>
                                                ";




                    //DisplayAssessmentName = AssessmentPurposes.Rows[0]["ASSESSMENT_NAME"].ToString();





                    DisplayAssessmentPurposes += "<table style='margin:auto;'><tr><td style='text-align:left;'>";
                    DisplayAssessmentPurposes += "<span>Assessment Purpose(s)</span>";
                    DisplayAssessmentPurposes += "<ul style='margin-top: 0px;'>";
                    for (int i = 0; i < AssessmentPurposes.Rows.Count; i++)
                    {
                        DisplayAssessmentPurposes += "<li>";
                        DisplayAssessmentPurposes += AssessmentPurposes.Rows[i]["NAME"].ToString();
                        DisplayAssessmentPurposes += "</li>";
                    }
                    DisplayAssessmentPurposes += "</ul>";
                    DisplayAssessmentPurposes += "</td></tr></table>";
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SEADH = null;
                AssessmentPurposes.Dispose();
                SubordinatesInfo.Dispose();
            }
        }

        void DisplayFinalizeMessage()
        {
            SupervisorGoalAssessmentDataHandler SGADH = new SupervisorGoalAssessmentDataHandler();
            string SatusCode = String.Empty;
            try
            {
                log.Debug("DisplayFinalizeMessage()");
                Boolean DisplayStatus = true;
                SatusCode = SGADH.GetGoalAssessmentStatus(hfAssesmentID.Value, hfYearofAssessment.Value, hfEmployeeID.Value);
                if (SatusCode == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                {
                    for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                    {
                        TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                        if (txtAchievedWeight.Text == String.Empty)
                        {
                            DisplayStatus = false;
                        }
                    }

                    for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                    {
                        TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                        string PlannedWeight = HttpUtility.HtmlDecode(grdvReportGrid.Rows[i].Cells[4].Text).Trim();

                        double SupervisorWeight = 0.0;
                        double MaxWeight = 0.0;

                        Double.TryParse(txtAchievedWeight.Text, out SupervisorWeight);
                        Double.TryParse(PlannedWeight, out MaxWeight);

                        if (MaxWeight < SupervisorWeight)
                        {
                            DisplayStatus = false;
                        }
                    }

                    if (txtSupervisorComment.Text == String.Empty)
                    {
                        DisplayStatus = false;
                    }

                    if (DisplayStatus == true)
                    {
                        //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Please complete the goal assessment.", lblFinalizeNotice);
                        lblFinalizeNotice.Text = "Please complete the goal assessment.";
                    }
                    else
                    {
                        Utility.Errorhandler.ClearError(lblFinalizeNotice);
                    }
                }
                else
                {
                    Utility.Errorhandler.ClearError(lblFinalizeNotice);
                }
            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SGADH = null;
            }
        }

        void DisplayChart()
        {
            log.Debug("DisplayChart()");
            SupervisorGoalAssessmentDataHandler SGADH = new SupervisorGoalAssessmentDataHandler();
            DataTable dtPreviousGoals = new DataTable();
            try
            {
                dtPreviousGoals = SGADH.PopulatePreviousData(hfEmployeeID.Value).Copy();
                string JSLabel = String.Empty;
                string JSData = String.Empty;

                for (int i = 0; i < dtPreviousGoals.Rows.Count; i++)
                {
                    if (JSLabel == String.Empty)
                    {
                        JSLabel += "'" + dtPreviousGoals.Rows[i]["SUPERVISOR_COMPLETED_DATE"].ToString().Trim() + "'";
                    }
                    else
                    {
                        JSLabel += ", '" + dtPreviousGoals.Rows[i]["SUPERVISOR_COMPLETED_DATE"].ToString().Trim() + "'";
                    }

                    if (JSData == String.Empty)
                    {
                        JSData += "'" + dtPreviousGoals.Rows[i]["TOTAL_SUPERVISOR_SCORE"].ToString().Trim() + "'";
                    }
                    else
                    {
                        JSData += ", '" + dtPreviousGoals.Rows[i]["TOTAL_SUPERVISOR_SCORE"].ToString().Trim() + "'";
                    }
                }

                string ChartString = @"

                                        <table style='width:100%;'>
		                                    <tr>
			                                    <td style='vertical-align:top;'>
			                                     Assessment Instructions :-
                                                 <br/>
			                                    <ul>
				                                    <li>You can partially evaluate gaols and save/update given scores without evaluate all the goals at one step.</li><br/><br/>
				                                    <li>After evaluating all annual goals. You need to click complete button to complete the goal evaluation process.</li>
			                                    </ul>
			                                    </td>
                                                <td></td>
			                                    <td>
				                                    <div style='width:450px;'>
					                                    <canvas id='canvas'></canvas>
				                                    </div>
			                                    </td>
		                                    </tr>
	                                    </table
                                        <br>
                                        <br>
                                        <script>
                                            var config = {
                                                type: 'line',
                                                data: {
                                                    labels: [" + JSLabel + @"],
                                                    datasets: [{
                                                        label: 'Employee Goal Achievement(s)',
                                                        data: [" + JSData + @"],
                                                        fill: false,
                                                    }]
                                                },
                                                options: {
                                                    responsive: true,
                                                    hover: {
                                                        mode: 'dataset'
                                                    },
                                                    scales: {
                                                        xAxes: [{
                                                            display: true,
                                                            scaleLabel: {
                                                                display: true,
                                                                labelString: 'Year'
                                                            }
                                                        }],
                                                        yAxes: [{
                                                            display: true,
                                                            scaleLabel: {
                                                                display: true,
                                                                labelString: 'Score'
                                                            },
                                                            ticks: {
                                                                suggestedMin: 0,
                                                                suggestedMax: 100,
                                                            }
                                                        }]
                                                    }
                                                }
                                            };

                                            $.each(config.data.datasets, function(i, dataset) {
                                                dataset.borderColor =  'rgba(0,0,255,1)';
                                                dataset.backgroundColor =  'rgba(0,0,255,1)';
                                                dataset.pointBorderColor = 'rgba(255,0,0,1)';
                                                dataset.pointBackgroundColor =  'rgba(255,0,0,1)';
                                                dataset.pointBorderWidth = 1;
                                            });

                                            window.onload = function() {
                                                var ctx = document.getElementById('canvas').getContext('2d');
                                                window.myLine = new Chart(ctx, config);
                                            };

                                        </script>                                      

                                    ";

                lblGoalChart.Text = ChartString;
            }
            catch (Exception ex)
            {
                log.Error("DisplayChart() : " + ex.Message);
                throw ex;
            }
            finally
            {
                dtPreviousGoals.Dispose();
                SGADH = null;
            }
        }

        #endregion

    }
}