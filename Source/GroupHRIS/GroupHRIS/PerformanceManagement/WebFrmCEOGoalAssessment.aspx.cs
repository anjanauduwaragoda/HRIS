using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using DataHandler.PerformanceManagement;
using DataHandler.Userlogin;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmCEOGoalAssessment : System.Web.UI.Page
    {
        public String Questions = String.Empty;
        public String DisplayAssessmentName = String.Empty;
        public String DisplayAssessmentPurposes = String.Empty;
        public DataTable dtRatings = new DataTable();

        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            PasswordHandler crpto;
            try
            {
                log.Debug("Page_Load()");

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
                        //StatusCheck();
                        setWeights();
                        LoadSupervisorName();
                    }
                    LoadAssessmentPurposes();
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

        protected void grdvReportGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

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

                    txtSupervisorComments.Text = dtEmployeeGoals.Rows[0]["COMMENTS"].ToString();

                    //SetComments();
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
                    if (Double.TryParse((grdvReportGrid.Rows[i].Cells[6].Text), out SupervisorGoalScore))
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

        void SetComments()
        {
            try
            {
                log.Debug("SetComments()");

                if (grdvReportGrid.Rows.Count > 0)
                {
                    string Comments = HttpUtility.HtmlDecode(grdvReportGrid.Rows[0].Cells[8].Text).ToString().Trim();
                    txtSupervisorComments.Text = Comments;
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

        void LoadSupervisorName()
        {
            SupervisorSelfAssessmentReviewDatahandler SSARDH = new SupervisorSelfAssessmentReviewDatahandler();
            string SupervisorName = String.Empty;
            try
            {
                SupervisorName = SSARDH.GetSupervisorName(hfAssesmentID.Value, hfYearofAssessment.Value, hfEmployeeID.Value);
                lblSupervisorName.Text = SupervisorName;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                SSARDH = null;
                SupervisorName = String.Empty;

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

                                        
					                                    <canvas id='canvas'></canvas>
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
                                                    responsive: false,
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
                                                ctx.canvas.width = 945;
                                                ctx.canvas.height = 200;
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