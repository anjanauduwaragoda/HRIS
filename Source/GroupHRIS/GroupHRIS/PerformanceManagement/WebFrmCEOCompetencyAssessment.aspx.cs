using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Userlogin;
using DataHandler.PerformanceManagement;
using System.Data;
using Common;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmCEOCompetencyAssessment : System.Web.UI.Page
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
                if (!IsPostBack)
                {
                    sIPAddress = Request.UserHostAddress;
                    log.Debug("IP:" + sIPAddress + "webFrmEmployee : Page_Load");

                    crpto = new PasswordHandler();

                    string AssessmentID = Request.QueryString["assmtId"];
                    string YearOfAssessmentID = Request.QueryString["year"];
                    string EmployeeID = Request.QueryString["employeeId"];

                    hfAssessmentID.Value = crpto.Decrypt(AssessmentID);
                    hfYearOfAssessment.Value = crpto.Decrypt(YearOfAssessmentID);
                    hfEmployeeID.Value = crpto.Decrypt(EmployeeID);

                    LoadData();
                    FinalizeCheck();

                    LoadSupervisorName();
                    LoadCompetencyLevelDetails();
                }

                LoadAssessmentPurposes();
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

        protected void grdvCompetencies_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    DropDownList ddlCompetencyRating = (e.Row.FindControl("ddlSupervisorRating") as DropDownList);

            //    ddlCompetencyRating.Items.Add(new ListItem("", ""));

            //    for (int i = 0; i < dtRatings.Rows.Count; i++)
            //    {
            //        string text = dtRatings.Rows[i]["RATING"].ToString();
            //        string value = dtRatings.Rows[i]["WEIGHT"].ToString();

            //        ddlCompetencyRating.Items.Add(new ListItem(text, value));
            //    }

            //    string SupervisorWeight = HttpUtility.HtmlDecode(e.Row.Cells[6].Text).ToString().Trim();
            //    if (SupervisorWeight != String.Empty)
            //    {
            //        ddlCompetencyRating.SelectedIndex = ddlCompetencyRating.Items.IndexOf(ddlCompetencyRating.Items.FindByValue(SupervisorWeight));
            //    }
            //}
        }

        protected void btnFinalize_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblStatus);
            CEOCompetencyAssessmentDataHandler CEOCADH = new CEOCompetencyAssessmentDataHandler();
            DataTable dtCompetencies = new DataTable();
            try
            {
                log.Debug("btnFinalize_Click()");

                double TotalSupervisorScore = 0.0;
                //string Disagreements = txtSupervisorComment.Text.Trim();
                string ModifiedBy = (Session["KeyEMPLOYEE_ID"] as string).Trim();
                string CompetencyProfileID = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[8].Text).ToString();
                string AssessmentToken = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[0].Text).ToString();
                string AssessmentID = hfAssessmentID.Value.ToString().Trim();
                string EmployeeID = hfEmployeeID.Value.ToString().Trim();
                string YearOfAssessment = hfYearOfAssessment.Value.ToString().Trim();


                DataTable dtEmployeeCompetencies = new DataTable();

                dtEmployeeCompetencies.Columns.Add("COMPETENCY_ID");
                dtEmployeeCompetencies.Columns.Add("SUPERVISOR_RATING");
                dtEmployeeCompetencies.Columns.Add("SUPERVISOR_WEIGHT");


                for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                {
                    string CompetencyID = String.Empty;
                    string SupervisorRating = String.Empty;
                    string SupervisorWeight = String.Empty;

                    DropDownList ddlCompetencyRating = (grdvCompetencies.Rows[i].FindControl("ddlSupervisorRating") as DropDownList);

                    CompetencyID = HttpUtility.HtmlDecode(grdvCompetencies.Rows[i].Cells[1].Text).ToString();

                    if (ddlCompetencyRating.SelectedIndex > 0)
                    {
                        double weight = 0.0;
                        if (Double.TryParse(ddlCompetencyRating.SelectedValue.ToString(), out weight))
                        {
                            TotalSupervisorScore += weight;

                            SupervisorRating = ddlCompetencyRating.SelectedItem.Text.Trim();
                            SupervisorWeight = ddlCompetencyRating.SelectedValue.Trim();
                        }
                        else
                        {
                            TotalSupervisorScore += 0.0;
                        }
                    }
                    else
                    {
                        TotalSupervisorScore += 0.0;
                    }

                    DataRow drEmployeeCompetency = dtEmployeeCompetencies.NewRow();

                    drEmployeeCompetency["COMPETENCY_ID"] = CompetencyID;
                    drEmployeeCompetency["SUPERVISOR_RATING"] = SupervisorRating;
                    drEmployeeCompetency["SUPERVISOR_WEIGHT"] = SupervisorWeight;

                    dtEmployeeCompetencies.Rows.Add(drEmployeeCompetency);
                }


                CEOCADH.Finalize(TotalSupervisorScore.ToString(), CompetencyProfileID, AssessmentToken, AssessmentID, EmployeeID, YearOfAssessment, ModifiedBy, dtEmployeeCompetencies.Copy());

                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_FINALIZED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);

                LoadData();
                FinalizeCheck();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                CEOCADH = null;
                dtCompetencies.Dispose();
            }
        }

        #endregion

        #region Methods

        void LoadAssessmentPurposes()
        {
            CEOCompetencyAssessmentDataHandler CEOCADH;
            DataTable AssessmentPurposes = new DataTable();
            DataTable SubordinatesInfo = new DataTable();
            try
            {
                CEOCADH = new CEOCompetencyAssessmentDataHandler();
                AssessmentPurposes = CEOCADH.PopulateAssessmentPurposes(hfAssessmentID.Value.Trim()).Copy();
                SubordinatesInfo = CEOCADH.PopulateSubordinatesInfo(hfEmployeeID.Value.Trim()).Copy();

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
                CEOCADH = null;
                AssessmentPurposes.Dispose();
                SubordinatesInfo.Dispose();
            }
        }

        void LoadData()
        {
            CEOCompetencyAssessmentDataHandler CEOCADH = new CEOCompetencyAssessmentDataHandler();
            DataTable dtCompetencies = new DataTable();
            try
            {
                LoadCompetencyRating();

                dtCompetencies = CEOCADH.Populate(hfAssessmentID.Value.ToString(), hfEmployeeID.Value.ToString(), hfYearOfAssessment.Value.ToString()).Copy();
                grdvCompetencies.DataSource = dtCompetencies.Copy();
                grdvCompetencies.DataBind();

                SetComments();
                //SetUpdateButton();

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                CEOCADH = null;
                dtCompetencies.Dispose();
            }
        }

        void LoadCompetencyRating()
        {
            CEOCompetencyAssessmentDataHandler CEOADH = new CEOCompetencyAssessmentDataHandler();
            try
            {
                dtRatings = CEOADH.PopulateRatings(hfAssessmentID.Value.ToString()).Copy();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                CEOADH = null;
            }
        }

        void FinalizeCheck()
        {
            CEOCompetencyAssessmentDataHandler CEOCADH = new CEOCompetencyAssessmentDataHandler();
            try
            {
                string CompetencyProfileID = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[8].Text).ToString();
                string AssessmentToken = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[0].Text).ToString();
                string AssessmentID = hfAssessmentID.Value.ToString().Trim();
                string EmployeeID = hfEmployeeID.Value.ToString().Trim();
                string YearOfAssessment = hfYearOfAssessment.Value.ToString().Trim();
                if (CEOCADH.IsSupervisorFinalized(CompetencyProfileID, AssessmentToken, AssessmentID, EmployeeID, YearOfAssessment))
                {
                    for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                    {
                        DropDownList ddlCompetencyRating = (grdvCompetencies.Rows[i].FindControl("ddlSupervisorRating") as DropDownList);
                        //ddlCompetencyRating.Enabled = true;
                    }
                    //txtSupervisorComment.Enabled = true;
                    //btnFinalize.Enabled = true;
                    //btnSave.Enabled = true;
                    //btnClear.Enabled = true;
                }
                else
                {
                    for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                    {
                        DropDownList ddlCompetencyRating = (grdvCompetencies.Rows[i].FindControl("ddlSupervisorRating") as DropDownList);
                        //ddlCompetencyRating.Enabled = false;
                    }
                    //txtSupervisorComment.Enabled = false;
                    //btnFinalize.Enabled = false;
                    //btnSave.Enabled = false;
                    //btnClear.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                CEOCADH = null;
            }
        }

        void ClearFields()
        {
            try
            {
                for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                {
                    DropDownList ddlCompetencyRating = (grdvCompetencies.Rows[i].FindControl("ddlSupervisorRating") as DropDownList);
                    ddlCompetencyRating.SelectedIndex = 0;
                }
                //Utility.Utils.clearControls(false, txtSupervisorComment);
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

                if (grdvCompetencies.Rows.Count > 0)
                {
                    string Comments = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[9].Text).ToString().Trim();
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
                SupervisorName = SSARDH.GetSupervisorName(hfAssessmentID.Value, hfYearOfAssessment.Value, hfEmployeeID.Value);
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

        void LoadCompetencyLevelDetails()
        {
            SupervisorCompetencyAssessmentDataHandler SCADH = new SupervisorCompetencyAssessmentDataHandler();
            try
            {
                log.Debug("LoadCompetencyRating()");

                dtRatings = SCADH.PopulateProficiencyLevels(hfEmployeeID.Value.ToString()).Copy();
                lblAllocatedWeights.Text = String.Empty;
                if (dtRatings.Rows.Count > 0)
                {
                    lblAllocatedWeights.Text += @"<table style='margin:auto;width:100%;'>";
                    lblAllocatedWeights.Text += @"<tr>";

                    lblAllocatedWeights.Text += @"<th>Rating</th>";
                    lblAllocatedWeights.Text += @"<th>Weight</th>";
                    lblAllocatedWeights.Text += @"<th style='text-align:left;'>Description</th>";

                    lblAllocatedWeights.Text += @"</tr>";


                    //lblAllocatedWeights.Text += " | ";
                    for (int i = 0; i < dtRatings.Rows.Count; i++)
                    {
                        lblAllocatedWeights.Text += @"<tr>";

                        lblAllocatedWeights.Text += @"<td>" + dtRatings.Rows[i]["RATING"].ToString() + @"</td>";
                        lblAllocatedWeights.Text += @"<td>" + dtRatings.Rows[i]["WEIGHT"].ToString() + @"</td>";
                        lblAllocatedWeights.Text += @"<td style='text-align:left;'>" + dtRatings.Rows[i]["REMARKS"].ToString() + @"</td>";

                        lblAllocatedWeights.Text += @"</tr>";
                        //lblAllocatedWeights.Text += dtRatings.Rows[i]["RATING"].ToString() + " - " + dtRatings.Rows[i]["WEIGHT"].ToString() + " | ";
                    }


                    lblAllocatedWeights.Text += @"</table>";
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SCADH = null;
            }
        }

        void DisplayChart()
        {
            log.Debug("DisplayChart()");
            SupervisorCompetencyAssessmentDataHandler SCADH = new SupervisorCompetencyAssessmentDataHandler();
            DataTable dtPreviousCompetencies = new DataTable();
            try
            {
                dtPreviousCompetencies = SCADH.PopulatePreviousData(hfEmployeeID.Value).Copy();
                string JSLabel = String.Empty;
                string JSData = String.Empty;

                for (int i = 0; i < dtPreviousCompetencies.Rows.Count; i++)
                {
                    if (JSLabel == String.Empty)
                    {
                        JSLabel += "'" + dtPreviousCompetencies.Rows[i]["SUPERVISOR_COMPLETED_DATE"].ToString().Trim() + "'";
                    }
                    else
                    {
                        JSLabel += ", '" + dtPreviousCompetencies.Rows[i]["SUPERVISOR_COMPLETED_DATE"].ToString().Trim() + "'";
                    }

                    if (JSData == String.Empty)
                    {
                        JSData += "'" + dtPreviousCompetencies.Rows[i]["TOTAL_SUPERVISOR_SCORE"].ToString().Trim() + "'";
                    }
                    else
                    {
                        JSData += ", '" + dtPreviousCompetencies.Rows[i]["TOTAL_SUPERVISOR_SCORE"].ToString().Trim() + "'";
                    }
                }

                string ChartString = @"

                                        <table style='width:100%;'>
		                                    <tr>
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
                                                        label: 'Employee Competency Achievement(s)',
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
                                                ctx.canvas.width = 695;
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
                dtPreviousCompetencies.Dispose();
                SCADH = null;
            }
        }

        #endregion
    }
}