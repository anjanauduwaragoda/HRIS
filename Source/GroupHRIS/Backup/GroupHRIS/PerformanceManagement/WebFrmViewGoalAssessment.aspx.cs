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
    public partial class WebFrmViewGoalAssessment : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

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
                        setTotalWeights();
                    }
                }
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            EmployeeGoalSelfAssessmentDataHandler EGSADH = new EmployeeGoalSelfAssessmentDataHandler();
            try
            {
                log.Debug("btnSave_Click()");

                string assessmentID = hfAssesmentID.Value.ToString().Trim();
                string yearOfAssessment = hfYearofAssessment.Value.ToString().Trim();
                string employeeID = hfEmployeeID.Value.ToString().Trim();
                string assessmentToken = HttpUtility.HtmlDecode(grdvReportGrid.Rows[0].Cells[1].Text.Trim());
                if (assessmentToken == " ")
                {
                    assessmentToken = "";
                }
                List<ListItem> liList = new List<ListItem>();
                string user = (Session["KeyEMPLOYEE_ID"] as string).Trim();
                double totalSelfScore = 0.0;

                for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                {
                    TextBox txtAchievedWeightInit = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                    txtAchievedWeightInit.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");

                    string goalID = HttpUtility.HtmlDecode(grdvReportGrid.Rows[i].Cells[4].Text.Trim());
                    string achieved = (grdvReportGrid.Rows[i].Cells[8].FindControl("txtAchievedWeight") as TextBox).Text.Trim();

                    //Double Weight = 0.0;

                    //if (Double.TryParse(achieved, out Weight) == false)
                    //{
                    //    CommonVariables.MESSAGE_TEXT = "Invalid Achieved Progress for '" + HttpUtility.HtmlDecode(grdvReportGrid.Rows[i].Cells[5].Text.Trim()) + "'";
                    //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                    //    return;
                    //}


                    ListItem liAssessmentDetails = new ListItem(goalID, achieved);
                    liList.Add(liAssessmentDetails);

                    double achievedProg = 0.0;
                    if (!Double.TryParse(achieved, out achievedProg))
                    {
                        if (achieved != "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Invalid achievement";

                            TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                            txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ecb50c");


                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                            return;
                        }
                        else
                        {
                            TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                            txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");

                            totalSelfScore += 0.0;
                        }
                    }
                    else
                    {
                        if (achievedProg > Convert.ToDouble(HttpUtility.HtmlDecode(grdvReportGrid.Rows[i].Cells[7].Text.Trim())))
                        {
                            CommonVariables.MESSAGE_TEXT = "Achieved weight cannot be greater than planned weight.";

                            TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                            txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ecb50c");

                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                            return;
                        }
                        else
                        {
                            TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                            txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                        }
                        totalSelfScore += achievedProg;
                    }
                }
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    if (totalSelfScore == 0.0)
                    {
                        CommonVariables.MESSAGE_TEXT = "At least one achievement should be enter before save";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                        return;
                    }

                    if (EGSADH.Insert(assessmentID, yearOfAssessment, employeeID, assessmentToken, user, totalSelfScore, liList))
                    {
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);
                        loadGoalAssessment();
                        setButons();
                        setTotalWeights();
                    }
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    if (totalSelfScore == 0.0)
                    {
                        CommonVariables.MESSAGE_TEXT = "At least one achievement should be enter before update";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                        return;
                    }
                    string AssessmentToken = grdvReportGrid.Rows[0].Cells[1].Text.Trim();
                    if (EGSADH.Update(AssessmentToken, hfAssesmentID.Value, hfEmployeeID.Value, totalSelfScore.ToString(), user, liList))
                    {
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);
                        loadGoalAssessment();
                        setButons();
                        setTotalWeights();
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
        }

        protected void btnFinalize_Click(object sender, EventArgs e)
        {
            EmployeeGoalSelfAssessmentDataHandler EGSADH = new EmployeeGoalSelfAssessmentDataHandler();
            UtilsDataHandler UDH = new UtilsDataHandler();
            try
            {
                log.Debug("btnFinalize_Click()");
                
                string assessmentID = hfAssesmentID.Value.ToString().Trim();
                string yearOfAssessment = hfYearofAssessment.Value.ToString().Trim();
                string employeeID = hfEmployeeID.Value.ToString().Trim();
                string assessmentToken = HttpUtility.HtmlDecode(grdvReportGrid.Rows[0].Cells[1].Text.Trim());
                if (assessmentToken == " ")
                {
                    assessmentToken = "";
                }
                List<ListItem> liList = new List<ListItem>();
                string user = (Session["KeyEMPLOYEE_ID"] as string).Trim();
                double totalSelfScore = 0.0;

                for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                {
                    string goalID = HttpUtility.HtmlDecode(grdvReportGrid.Rows[i].Cells[4].Text.Trim());
                    string achieved = (grdvReportGrid.Rows[i].Cells[8].FindControl("txtAchievedWeight") as TextBox).Text.Trim();
                    ListItem liAssessmentDetails = new ListItem(goalID, achieved);
                    liList.Add(liAssessmentDetails);

                    double achievedProg = 0.0;
                    if (!Double.TryParse(achieved, out achievedProg))
                    {
                        if (achieved != "")
                        {
                            CommonVariables.MESSAGE_TEXT = "Invalid achievement";

                            TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                            txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ecb50c");


                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                            return;
                        }
                        else
                        {
                            totalSelfScore += 0.0;

                            CommonVariables.MESSAGE_TEXT = "Assessment cannot be finalize with empty achievements";

                            TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                            txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ecb50c");


                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                            return;
                        }
                    }
                    else
                    {
                        if (achievedProg > Convert.ToDouble(HttpUtility.HtmlDecode(grdvReportGrid.Rows[i].Cells[7].Text.Trim())))
                        {
                            CommonVariables.MESSAGE_TEXT = "Achieved weight should be less than planned weight.";

                            TextBox txtAchievedWeight = (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox);
                            txtAchievedWeight.BackColor = System.Drawing.ColorTranslator.FromHtml("#ecb50c");


                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                            return;
                        }
                        totalSelfScore += achievedProg;
                    }
                }


                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    if (EGSADH.InsertAndFinalize(assessmentID, yearOfAssessment, employeeID, assessmentToken, user, totalSelfScore, liList))
                    {
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_FINALIZED;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);
                        loadGoalAssessment();
                        setButons();
                        setTotalWeights();
                    }
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string AssessmentToken = grdvReportGrid.Rows[0].Cells[1].Text.Trim();
                    if (EGSADH.UpdateAndFinalize(assessmentToken, hfAssesmentID.Value, hfEmployeeID.Value, totalSelfScore.ToString().Trim(), user, liList))
                    {
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_FINALIZED;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);
                        loadGoalAssessment();
                        setButons();
                        setTotalWeights();
                    }
                }

                //Update Assessment Status
                UDH.updateAssessmentStatus(hfAssesmentID.Value, hfEmployeeID.Value, hfYearofAssessment.Value);
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                EGSADH = null;
                UDH = null;
            }
        }

        #endregion

        #region Methods

        void loadGoalAssessment()
        {
            DataTable dtEmployeeGoals = new DataTable();
            EmployeeGoalSelfAssessmentDataHandler EGSADH = new EmployeeGoalSelfAssessmentDataHandler();
            try
            {
                log.Debug("loadGoalAssessment()");

                if ((hfEmployeeID.Value.ToString() != "") && (hfAssesmentID.Value.ToString() != "") && (hfYearofAssessment.Value.ToString() != ""))
                {

                    if (EGSADH.isNewRecord(hfEmployeeID.Value.Trim(), hfAssesmentID.Value.Trim(), hfYearofAssessment.Value.Trim()))
                    {
                        btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                        string EmployeeID = hfEmployeeID.Value;
                        string YearOfGoal = hfYearofAssessment.Value;

                        dtEmployeeGoals = EGSADH.Populate(EmployeeID, YearOfGoal).Copy();
                        grdvReportGrid.DataSource = dtEmployeeGoals.Copy();
                        grdvReportGrid.DataBind();

                        grdvReportGrid.Columns[9].Visible = false;

                        double PlannedProgress = 0.0;
                        for (int i = 0; i < dtEmployeeGoals.Rows.Count; i++)
                        {
                            PlannedProgress += Convert.ToDouble(dtEmployeeGoals.Rows[i]["WEIGHT"].ToString());
                        }
                    }
                    else
                    {
                        btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                        dtEmployeeGoals = EGSADH.Populate(hfAssesmentID.Value, hfEmployeeID.Value, hfYearofAssessment.Value).Copy();
                        grdvReportGrid.DataSource = dtEmployeeGoals.Copy();
                        grdvReportGrid.DataBind();

                        for (int i = 0; i < dtEmployeeGoals.Rows.Count; i++)
                        {
                            string achivedProgress = dtEmployeeGoals.Rows[i]["EMPLOYEE_SELF_SCORE"].ToString();
                            (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox).Text = achivedProgress;
                        }
                    }

                    string isEmpty = String.Empty;
                    for (int i = 0; i < dtEmployeeGoals.Rows.Count; i++)//SUPERVISOR_SCORE
                    {
                        if (dtEmployeeGoals.Rows[i]["SUPERVISOR_SCORE"].ToString().Trim() != String.Empty)
                        {
                            isEmpty = "0";
                            break;
                        }
                    }

                    if (isEmpty == "0")
                    {
                        grdvReportGrid.Columns[9].Visible = true;
                    }
                    else
                    {
                        grdvReportGrid.Columns[9].Visible = false;
                    }

                    setButons();
                    //lblTotalPlanned.Text = PlannedProgress.ToString();
                }

                if (grdvReportGrid.Rows.Count > 0)
                {
                    btnFinalize.Visible = true;
                    btnSave.Visible = true;
                }
                else
                {
                    btnFinalize.Visible = false;
                    btnSave.Visible = false;
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
                EGSADH = null;
            }
        }

        void setButons()
        {
            EmployeeGoalSelfAssessmentDataHandler EGSADH = new EmployeeGoalSelfAssessmentDataHandler();
            try
            {
                log.Debug("setButons()");
                if (EGSADH.isFinalized(hfEmployeeID.Value, hfAssesmentID.Value, hfYearofAssessment.Value))
                {
                    btnSave.Enabled = false;
                    btnFinalize.Enabled = false;
                    for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                    {
                        (grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox).Enabled = false;
                    }
                }
                else
                {
                    btnSave.Enabled = true;
                    btnFinalize.Enabled = true;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                EGSADH = null;
            }

        }

        void setTotalWeights()
        {
            double Planned = 0.0;
            double achieved = 0.0;
            try
            {
                log.Debug("setTotalWeights()");
                for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                {
                    double TempPlanned = 0.0;
                    double Tempachieved = 0.0;
                    if (Double.TryParse(grdvReportGrid.Rows[i].Cells[7].Text.Trim(), out TempPlanned))
                    {
                        Planned += TempPlanned;
                    }
                    else
                    {
                        Planned += 0.0;
                    }
                    if (Double.TryParse((grdvReportGrid.Rows[i].FindControl("txtAchievedWeight") as TextBox).Text.Trim(), out Tempachieved))
                    {
                        achieved += Tempachieved;
                    }
                    else
                    {
                        achieved += 0.0;
                    }
                    lblTotalPlanned.Text = Convert.ToString(Planned) + "%";
                    lblTotalAcievement.Text = Convert.ToString(achieved) + "%";
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                Planned = 0.0;
                achieved = 0.0;
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
				                                    <li>You can partially evaluate goals and save/update given scores (marks) without evaluating all the goals at one step.</li><br/><br/>
				                                    <li>After evaluating all the goals you need to click finalize button to finish evaluation process. After finalize, you are not allowed to modify given scores (marks).</li>
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