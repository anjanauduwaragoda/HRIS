using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using DataHandler.Userlogin;
using DataHandler.PerformanceManagement;
using DataHandler.Utility;
using NLog;
using System.Text;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmEvaluationSummary : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            PasswordHandler crpto;
            try
            {
                sIPAddress = Request.UserHostAddress;
                log.Debug("IP:" + sIPAddress + "WebFrmCompetencyGroup : Page_Load");


                if (!IsPostBack)
                {
                    crpto = new PasswordHandler();

                    string AssessmentID = Request.QueryString["assmtId"];
                    string YearOfAssessmentID = Request.QueryString["year"];
                    string EmployeeID = Request.QueryString["employeeId"];

                    hfAssessmentID.Value = crpto.Decrypt(AssessmentID);
                    hfYearOfAssessment.Value = crpto.Decrypt(YearOfAssessmentID);
                    hfEmployeeID.Value = crpto.Decrypt(EmployeeID);

                    LoadEvaluationDetails();
                    LoadAssessmentPurposes();
                    LoadSubordinateDetails();
                    LoadSupervisorDetails();
                    LoadGoalAssessmentMarks();
                    LoadProficiencyLevels();
                    LoadCompetencyAssessmentMarks();
                    LoadAverages();
                    LoadSupervisorComments();
                    LoadEmployeeComments();
                    LoadCEOComments();
                    SelectUser();

                    if ((rbtFeedbackGivenEmpYes.Checked == false) && (rbtFeedbackGivenEmpNo.Checked == false))
                    {
                        rbtFeedbackGivenEmpYes.Checked = true;
                    }

                    //isEmployeeAgreed();
                    //isCEOFinalized();
                }
                DisplayChart();
            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                crpto = null;
            }
        }

        protected void btnSupervisorComplete_Click(object sender, EventArgs e)
        {
            UtilsDataHandler UDH = new UtilsDataHandler();
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                log.Debug("btnSupervisorComplete_Click");

                Utility.Errorhandler.ClearError(lblSupervisorMessage);

                string EmployeeAgreeStatus = PEDH.EmployeeAgreedStatus(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value);
                Boolean isSupervisorCompleted = PEDH.IsSupervisorCompleted(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value);

                if ((EmployeeAgreeStatus == Constants.CON_ACTIVE_STATUS) || (EmployeeAgreeStatus == Constants.CON_INACTIVE_STATUS))
                {
                    if (txtConsequenceDisagreements.Text.Trim() == String.Empty)
                    {
                        CommonVariables.MESSAGE_TEXT = "Consequence disagreement comment is required";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblSupervisorMessage);
                        return;
                    }
                }

                SaveSupervisorComments();
                SupCompleteEvaluation();

                //Update Assessment Status
                UDH.updateAssessmentStatus(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value);

                //Send Email
                if (rbtFeedbackGivenEmpYes.Checked == true)
                {
                    SendSupervisorCompletedEmail();
                }

                SupervisorView();
            }
            catch (Exception exp)
            {
                log.Error(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblSupervisorMessage);
            }
            finally
            {
                UDH = null;
                PEDH = null;
            }

        }

        protected void btnEmployeeComplete_Click(object sender, EventArgs e)
        {
            UtilsDataHandler UDH = new UtilsDataHandler();
            try
            {
                log.Debug("btnEmployeeComplete_Click");
                Utility.Errorhandler.ClearError(lblSubordinateMessage);
                //Supervisor Complete Check

                if (rbtEmployeeDisagreed.Checked == true)
                {
                    if (txtEmployeeDisagreeComment.Text.ToString().Trim() == String.Empty)
                    {
                        CommonVariables.MESSAGE_TEXT = "Disagree reason is required";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblSubordinateMessage);
                        return;
                    }
                    else
                    {
                        SaveEmployeeComments();
                    }
                }
                else
                {
                    SaveEmployeeComments();
                }


                //Update Assessment Status
                UDH.updateAssessmentStatus(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value);

                EmployeeView();

                //Send Emails
                if (rbtEmployeeAgreed.Checked == true)
                {
                    SendSubordinateAgreedEmail();
                }
                else if (rbtEmployeeDisagreed.Checked == true)
                {
                    SendSubordinateDisagreedEmail();
                }

                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_COMPLETED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblSubordinateMessage);
            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblSubordinateMessage);
            }
            finally
            {
                UDH = null;
            }
        }

        protected void btnFinalizeSupervisorEvaluation_Click(object sender, EventArgs e)
        {
            UtilsDataHandler UDH = new UtilsDataHandler();
            try
            {

                log.Debug("btnFinalizeSupervisorEvaluation_Click");
                Utility.Errorhandler.ClearError(lblSupervisorMessage);
                SupFinalizeEvaluation();
                
                

                isEmployeeAgreed();

                //Update Assessment Status

                UDH.updateAssessmentStatus(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value);

                SelectUser();

                //Send Email
                SendSupervisorFinalizedEmail();
            }
            catch (Exception exp)
            {
                log.Error(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblSupervisorMessage);
            }
            finally
            {
                UDH = null;
            }
        }

        protected void btnCEOComplete_Click(object sender, EventArgs e)
        {
            log.Debug("btnCEOComplete_Click");
            try
            {

                double Increment = 0.0;
                if (chkIncrementGranted.Checked == true)
                {
                    if (txtIncrementGranted.Text != String.Empty)
                    {
                        if (double.TryParse(txtIncrementGranted.Text, out Increment) == false)
                        {
                            CommonVariables.MESSAGE_TEXT = "Invalid Increment";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                            return;
                        }
                    }
                }


                double ReviewMonths = 0.0;
                if (chkToBeReviewed.Checked == true)
                {
                    if (txtToBeReviewed.Text != String.Empty)
                    {
                        if (double.TryParse(txtToBeReviewed.Text, out ReviewMonths) == false)
                        {
                            CommonVariables.MESSAGE_TEXT = "Invalid Review Months";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                            return;
                        }
                    }
                }


                double ProbationExtension = 0.0;
                if (chkProbationExtended.Checked == true)
                {
                    if (txtProbationExtended.Text != String.Empty)
                    {
                        if (double.TryParse(txtProbationExtended.Text, out ProbationExtension) == false)
                        {
                            CommonVariables.MESSAGE_TEXT = "Invalid Probation Extension";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                            return;
                        }
                    }
                }

                SaveCEOComments();
            }
            catch (Exception ex)
            {
                log.Error("btnCEOComplete_Click" + ex.Message);
                throw ex;
            }
            finally
            { 
            
            }
        }

        protected void btnCEOFinalize_Click(object sender, EventArgs e)
        {
            UtilsDataHandler UDH = new UtilsDataHandler();
            try
            {
                log.Debug("btnCEOFinalize_Click");

                double Increment = 0.0;
                if (chkIncrementGranted.Checked == true)
                {
                    if (txtIncrementGranted.Text != String.Empty)
                    {
                        if (double.TryParse(txtIncrementGranted.Text, out Increment) == false)
                        {
                            CommonVariables.MESSAGE_TEXT = "Invalid Increment";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                            return;
                        }
                    }
                }


                double ReviewMonths = 0.0;
                if (chkToBeReviewed.Checked == true)
                {
                    if (txtToBeReviewed.Text != String.Empty)
                    {
                        if (double.TryParse(txtToBeReviewed.Text, out ReviewMonths) == false)
                        {
                            CommonVariables.MESSAGE_TEXT = "Invalid Review Months";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                            return;
                        }
                    }
                }


                double ProbationExtension = 0.0;
                if (chkProbationExtended.Checked == true)
                {
                    if (txtProbationExtended.Text != String.Empty)
                    {
                        if (double.TryParse(txtProbationExtended.Text, out ProbationExtension) == false)
                        {
                            CommonVariables.MESSAGE_TEXT = "Invalid Probation Extension";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                            return;
                        }
                    }
                }



                if (chkIncrementGranted.Checked == true)
                {
                    if (txtIncrementGranted.Text == String.Empty)
                    {
                        CommonVariables.MESSAGE_TEXT = "Increment is Required";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }
                }

                if (chkToBeReviewed.Checked == true)
                {
                    if (txtToBeReviewed.Text == String.Empty)
                    {
                        CommonVariables.MESSAGE_TEXT = "Review Months are Required";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }
                }

                if (chkEmploymentConfrimed.Checked == true)
                {
                    if ((rbtEmploymentConfrimedYes.Checked == false) && (rbtEmploymentConfrimedNo.Checked == false))
                    {
                        CommonVariables.MESSAGE_TEXT = "Employment confirmed, yes or no is Required";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }
                }
                //
                if (chkProbationExtended.Checked == true)
                {
                    if (txtProbationExtended.Text == String.Empty)
                    {
                        CommonVariables.MESSAGE_TEXT = "Probation extended months are required";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }
                }

                if (chkEndTraining.Checked == true)
                {
                    if ((rbtEndTrainingYes.Checked == false) && (rbtEndTrainingNo.Checked == false))
                    {
                        CommonVariables.MESSAGE_TEXT = "End training yes or no is required";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }
                }


                if (txtCEOComments.Text.ToString().Trim() == String.Empty)
                {
                    CommonVariables.MESSAGE_TEXT = "CEO/COO comment is required";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                    return;
                }
                else
                {
                    FinalizeCEOComments();
                    CEOFinalizeEvaluation();
                    //Update Assessment Status

                    UDH.updateAssessmentStatus(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value);
                    SelectUser();

                    //Send Email
                    SendCEOFinalizedEmail();
                }
            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                UDH = null;
            }
        }

        protected void btnSupervisorClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnSupervisorClear_Click");
            SupervisorClear();
        }

        protected void btnEmployeeClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnEmployeeClear_Click");
            EmployeeClear();
        }

        protected void btnCEOClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnEmployeeClear_Click");
            CEOClear();
        }

        protected void rbtEmployeeAgreed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("rbtEmployeeAgreed_CheckedChanged");
                if (rbtEmployeeAgreed.Checked == true)
                {
                    txtEmployeeDisagreeComment.Enabled = false;
                    //txtConsequenceDisagreements.Enabled = false;
                }
                else
                {
                    txtEmployeeDisagreeComment.Enabled = true;
                    //txtConsequenceDisagreements.Enabled = true;
                }

            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void rbtEmployeeDisagreed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("rbtEmployeeDisagreed_CheckedChanged");
                if (rbtEmployeeDisagreed.Checked == true)
                {
                    txtEmployeeDisagreeComment.Enabled = true;
                    //txtConsequenceDisagreements.Enabled = true;
                }
                else
                {
                    txtEmployeeDisagreeComment.Enabled = false;
                    //txtConsequenceDisagreements.Enabled = false;
                }

            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void chkIncrementGranted_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("chkIncrementGranted_CheckedChanged");
                if (chkIncrementGranted.Checked == false)
                {
                    Utility.Utils.clearControls(false, txtIncrementGranted);
                }

            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void chkToBeReviewed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("chkToBeReviewed_CheckedChanged");
                if (chkToBeReviewed.Checked == false)
                {
                    Utility.Utils.clearControls(false, txtToBeReviewed);
                }

            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void chkEmploymentConfrimed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("chkEmploymentConfrimed_CheckedChanged");
                if (chkEmploymentConfrimed.Checked == false)
                {
                    rbtEmploymentConfrimedYes.Checked = false;
                    rbtEmploymentConfrimedNo.Checked = false;
                }

            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void rbtEmploymentConfrimedYes_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("rbtEmploymentConfrimedYes_CheckedChanged");
                if (chkEmploymentConfrimed.Checked == false)
                {
                    rbtEmploymentConfrimedYes.Checked = false;
                    rbtEmploymentConfrimedNo.Checked = false;
                }

            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void rbtEmploymentConfrimedNo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("rbtEmploymentConfrimedNo_CheckedChanged");
                if (chkEmploymentConfrimed.Checked == false)
                {
                    rbtEmploymentConfrimedYes.Checked = false;
                    rbtEmploymentConfrimedNo.Checked = false;
                }

            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void chkEndTraining_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("chkEndTraining_CheckedChanged");
                if (chkEndTraining.Checked == false)
                {
                    rbtEndTrainingYes.Checked = false;
                    rbtEndTrainingNo.Checked = false;
                }

            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void rbtEndTrainingYes_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("rbtEndTrainingYes_CheckedChanged");
                if (chkEndTraining.Checked == false)
                {
                    rbtEndTrainingYes.Checked = false;
                    rbtEndTrainingNo.Checked = false;
                }

            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void rbtEndTrainingNo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("rbtEndTrainingNo_CheckedChanged");
                if (chkEndTraining.Checked == false)
                {
                    rbtEndTrainingYes.Checked = false;
                    rbtEndTrainingNo.Checked = false;
                }

            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void chkProbationExtended_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("chkProbationExtended_CheckedChanged");
                if (chkProbationExtended.Checked == false)
                {
                    Utility.Utils.clearControls(false, txtProbationExtended);
                }

            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        #endregion

        #region Methods

        void LoadEvaluationDetails()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtEvaluationDetails = new DataTable();
            try
            {
                dtEvaluationDetails = PEDH.PopulateAssessmentDetails(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value).Copy();
                if (dtEvaluationDetails.Rows.Count > 0)
                {
                    lblEvaluationName.Text = dtEvaluationDetails.Rows[0]["ASSESSMENT_NAME"].ToString();
                    lblEvaluationType.Text = dtEvaluationDetails.Rows[0]["ASSESSMENT_TYPE_NAME"].ToString();
                }
                else
                {
                    lblEvaluationName.Text = String.Empty;
                    lblEvaluationType.Text = String.Empty;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
                dtEvaluationDetails.Dispose();
            }
        }

        void LoadAssessmentPurposes()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtAssessmentPurposes = new DataTable();
            lblAssessmentPurposes.Text = String.Empty;
            try
            {
                dtAssessmentPurposes = PEDH.PopulateAssessmentPurposes(hfAssessmentID.Value).Copy();
                if (dtAssessmentPurposes.Rows.Count > 0)
                {
                    lblAssessmentPurposes.Text = @"<ul>";
                    for (int i = 0; i < dtAssessmentPurposes.Rows.Count; i++)
                    {
                        lblAssessmentPurposes.Text += @"<li>" + dtAssessmentPurposes.Rows[i]["NAME"].ToString() + @"</li>";
                    }
                    lblAssessmentPurposes.Text += @"</ul>";
                }
                else
                {
                    lblAssessmentPurposes.Text = String.Empty;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
                dtAssessmentPurposes.Dispose();
            }
        }

        void LoadSubordinateDetails()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtSubordinateDetails = new DataTable();
            lblSubordinateName.Text = String.Empty;
            lblSubordinateDesignation.Text = String.Empty;
            try
            {
                dtSubordinateDetails = PEDH.PopulateEmployeeDetails(hfEmployeeID.Value).Copy();
                if (dtSubordinateDetails.Rows.Count > 0)
                {
                    lblSubordinateName.Text = dtSubordinateDetails.Rows[0]["TITLE"].ToString() + " " + dtSubordinateDetails.Rows[0]["INITIALS_NAME"].ToString();
                    lblSubordinateDesignation.Text = dtSubordinateDetails.Rows[0]["DESIGNATION_NAME"].ToString();
                }
                else
                {
                    lblSubordinateName.Text = String.Empty;
                    lblSubordinateDesignation.Text = String.Empty;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
                dtSubordinateDetails.Dispose();
            }
        }

        void LoadSupervisorDetails()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtSupervisorDetails = new DataTable();

            lblSupervisorName.Text = String.Empty;
            lblSupervisorDesignation.Text = String.Empty;

            try
            {
                dtSupervisorDetails = PEDH.PopulateSupervisorDetails(hfEmployeeID.Value, hfAssessmentID.Value, hfYearOfAssessment.Value).Copy();
                if (dtSupervisorDetails.Rows.Count > 0)
                {
                    lblSupervisorName.Text = dtSupervisorDetails.Rows[0]["TITLE"].ToString() + " " + dtSupervisorDetails.Rows[0]["INITIALS_NAME"].ToString();
                    lblSupervisorDesignation.Text = dtSupervisorDetails.Rows[0]["DESIGNATION_NAME"].ToString();
                }
                else
                {
                    lblSupervisorName.Text = String.Empty;
                    lblSupervisorDesignation.Text = String.Empty;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
                dtSupervisorDetails.Dispose();
            }
        }

        void LoadGoalAssessmentMarks()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtGoalAssessmentMarks = new DataTable();

            lblSubordinatesTotal.Text = String.Empty;
            lblSupervisorsTotal.Text = String.Empty;

            try
            {
                dtGoalAssessmentMarks = PEDH.PopulateGoalAssessmentMarks(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value).Copy();
                if (dtGoalAssessmentMarks.Rows.Count > 0)
                {
                    lblSubordinatesTotal.Text = dtGoalAssessmentMarks.Rows[0]["TOTAL_SELF_SCORE"].ToString();
                    lblSupervisorsTotal.Text = dtGoalAssessmentMarks.Rows[0]["TOTAL_SUPERVISOR_SCORE"].ToString();
                }
                else
                {
                    lblSubordinatesTotal.Text = String.Empty;
                    lblSupervisorsTotal.Text = String.Empty;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
                dtGoalAssessmentMarks.Dispose();
            }
        }

        void LoadCompetencyAssessmentMarks()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtCompetencyAssessmentEmployeeMarks = new DataTable();
            DataTable dtCompetencyAssessmentSupervisorMarks = new DataTable();
            DataTable dtCompetencyAssessmentMarks = new DataTable();
            List<string> lstRatingTypes = new List<string>();
            List<string> lstDistinctRatingTypes = new List<string>();
            List<string> lstTempRatingTypes = new List<string>();
            List<string> lstTempDistinctRatingTypes = new List<string>();
            Double SubWeight = 0.0;
            Double SubWeightCount = 0.0;
            Double SupWeight = 0.0;
            Double SupWeightCount = 0.0;
            Double TotalSubWeight = 0.0;
            Double TotalSupWeight = 0.0;
            Double TempTotalSubWeight = 0.0;
            Double TempTotalSupWeight = 0.0;
            string CompetencyTable = String.Empty;

            try
            {
                dtCompetencyAssessmentMarks.Columns.Add("MARKS");
                dtCompetencyAssessmentMarks.Columns.Add("SUBORDINATE");
                dtCompetencyAssessmentMarks.Columns.Add("SUPERVISOR");
                dtCompetencyAssessmentMarks.Columns.Add("SUBORDINATE_MARKS");
                dtCompetencyAssessmentMarks.Columns.Add("SUPERVISOR_MARKS");

                dtCompetencyAssessmentEmployeeMarks = PEDH.PopulateEmployeeCompetencyAssessmentMarks(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value).Copy();
                dtCompetencyAssessmentSupervisorMarks = PEDH.PopulateSupervisorCompetencyAssessmentMarks(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value).Copy();

                for (int i = 0; i < dtCompetencyAssessmentEmployeeMarks.Rows.Count; i++)
                {
                    lstRatingTypes.Add("Number of " + dtCompetencyAssessmentEmployeeMarks.Rows[i]["EMPLOYEE_RATING"].ToString() + "'s X " + dtCompetencyAssessmentEmployeeMarks.Rows[i]["EMPLOYEE_WEIGHT"].ToString());
                    lstTempRatingTypes.Add(dtCompetencyAssessmentEmployeeMarks.Rows[i]["EMPLOYEE_RATING"].ToString());
                }

                for (int i = 0; i < dtCompetencyAssessmentSupervisorMarks.Rows.Count; i++)
                {
                    lstRatingTypes.Add("Number of " + dtCompetencyAssessmentSupervisorMarks.Rows[i]["SUPERVISOR_RATING"].ToString() + "'s X " + dtCompetencyAssessmentSupervisorMarks.Rows[i]["SUPERVISOR_WEIGHT"].ToString());
                    lstTempRatingTypes.Add(dtCompetencyAssessmentSupervisorMarks.Rows[i]["SUPERVISOR_RATING"].ToString());
                }

                lstDistinctRatingTypes.AddRange(lstRatingTypes.Distinct());
                lstTempDistinctRatingTypes.AddRange(lstTempRatingTypes.Distinct());


                for (int i = 0; i < lstDistinctRatingTypes.Count; i++)
                {
                    SubWeight = 0.0;
                    SubWeightCount = 0.0;
                    SupWeight = 0.0;
                    SupWeightCount = 0.0;

                    DataRow drMarks = dtCompetencyAssessmentMarks.NewRow();
                    DataRow[] EmpMrkArr = dtCompetencyAssessmentEmployeeMarks.Select("EMPLOYEE_RATING = '" + lstTempDistinctRatingTypes[i] + "'");
                    DataRow[] SupMrkArr = dtCompetencyAssessmentSupervisorMarks.Select("SUPERVISOR_RATING = '" + lstTempDistinctRatingTypes[i] + "'");

                    drMarks["MARKS"] = lstDistinctRatingTypes[i];
                    if (EmpMrkArr.Length > 0)
                    {
                        Double.TryParse(EmpMrkArr[0]["EMPLOYEE_WEIGHT"].ToString(), out SubWeight);
                        Double.TryParse(EmpMrkArr[0]["WEIGHT_COUNT"].ToString(), out SubWeightCount);
                        drMarks["SUBORDINATE"] = SubWeight.ToString() + " X " + SubWeightCount.ToString() + " = " + Convert.ToString(SubWeight * SubWeightCount).PadLeft(2, '0');
                    }

                    drMarks["SUBORDINATE_MARKS"] = Convert.ToString(SubWeight * SubWeightCount);

                    if (SupMrkArr.Length > 0)
                    {
                        Double.TryParse(SupMrkArr[0]["SUPERVISOR_WEIGHT"].ToString(), out SupWeight);
                        Double.TryParse(SupMrkArr[0]["WEIGHT_COUNT"].ToString(), out SupWeightCount);
                        drMarks["SUPERVISOR"] = SupWeight.ToString() + " X " + SupWeightCount.ToString() + " = " + Convert.ToString(SupWeight * SupWeightCount).PadLeft(2, '0');
                    }

                    drMarks["SUPERVISOR_MARKS"] = Convert.ToString(SupWeight * SupWeightCount);



                    if ((EmpMrkArr.Length > 0) == false)
                    {
                        drMarks["SUBORDINATE"] = SupWeight.ToString() + " X " + SubWeightCount.ToString() + " = " + Convert.ToString(SupWeight * SubWeightCount).PadLeft(2, '0');
                    }
                    if ((SupMrkArr.Length > 0) == false)
                    {
                        drMarks["SUPERVISOR"] = SubWeight.ToString() + " X " + SupWeightCount.ToString() + " = " + Convert.ToString(SubWeight * SupWeightCount).PadLeft(2, '0');
                    }

                    dtCompetencyAssessmentMarks.Rows.Add(drMarks);
                }

                for (int i = 0; i < dtCompetencyAssessmentMarks.Rows.Count; i++)
                {
                    CompetencyTable += @"<tr>";
                    CompetencyTable += @"<td>" + dtCompetencyAssessmentMarks.Rows[i]["MARKS"].ToString() + @"</td>";
                    CompetencyTable += @"<td style='text-align: right;'>" + dtCompetencyAssessmentMarks.Rows[i]["SUBORDINATE"].ToString() + @"</td>";
                    CompetencyTable += @"<td style='text-align: right;'>" + dtCompetencyAssessmentMarks.Rows[i]["SUPERVISOR"].ToString() + @"</td>";
                    CompetencyTable += @"</tr>";
                }

                lblCompetencyTable.Text = CompetencyTable;

                for (int i = 0; i < dtCompetencyAssessmentMarks.Rows.Count; i++)
                {
                    TempTotalSubWeight = 0.0;
                    TempTotalSupWeight = 0.0;

                    if (Double.TryParse(dtCompetencyAssessmentMarks.Rows[i]["SUBORDINATE_MARKS"].ToString(), out TempTotalSubWeight))
                    {
                        TotalSubWeight += TempTotalSubWeight;
                    }

                    if (Double.TryParse(dtCompetencyAssessmentMarks.Rows[i]["SUPERVISOR_MARKS"].ToString(), out TempTotalSupWeight))
                    {
                        TotalSupWeight += TempTotalSupWeight;
                    }
                }
                lblSubTotalCompetency.Text = TotalSubWeight.ToString();
                lblSupTotalCompetency.Text = TotalSupWeight.ToString();

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
                dtCompetencyAssessmentEmployeeMarks.Dispose();
                dtCompetencyAssessmentSupervisorMarks.Dispose();
                dtCompetencyAssessmentMarks.Dispose();
                lstRatingTypes = null;
                lstDistinctRatingTypes = null;
                lstTempRatingTypes = null;
                lstTempDistinctRatingTypes = null;
                SubWeight = 0.0;
                SubWeightCount = 0.0;
                SupWeight = 0.0;
                SupWeightCount = 0.0;
                TotalSubWeight = 0.0;
                TotalSupWeight = 0.0;
                TempTotalSubWeight = 0.0;
                TempTotalSupWeight = 0.0;
                CompetencyTable = String.Empty;
            }
        }

        void LoadProficiencyLevels()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtProficiencyLevels = new DataTable();
            try
            {
                log.Debug("LoadProficiencyLevels");
                dtProficiencyLevels = PEDH.PopulateProficiencyLevels(hfEmployeeID.Value).Copy();

                if (dtProficiencyLevels.Rows.Count > 0)
                {
                    string TableHTML = String.Empty;
                    for (int i = 0; i < dtProficiencyLevels.Rows.Count; i++)
                    {
                        TableHTML += @"<tr>";
                        TableHTML += @"<td>" + dtProficiencyLevels.Rows[i]["RATING"].ToString() + @"</td>";
                        TableHTML += @"<td>" + dtProficiencyLevels.Rows[i]["REMARKS"].ToString() + @"</td>";
                        TableHTML += @"</tr>";
                    }
                    lblProficiencyLevels.Text = TableHTML;
                }
            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
                dtProficiencyLevels.Dispose();
            }
        }

        void LoadAverages()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtProficiencyLevels = new DataTable();
            try
            {
                log.Debug("LoadAverages");
                double SubordinateKPITotal = 0.0;
                double SupervisorKPITotal = 0.0;
                double SubordinateCompetencyTotal = 0.0;
                double SupervisorCompetencyTotal = 0.0;

                double SubordinateAverage = 0.0;
                double SupervisorAverage = 0.0;

                Double.TryParse(lblSubordinatesTotal.Text, out SubordinateKPITotal);
                Double.TryParse(lblSupervisorsTotal.Text, out SupervisorKPITotal);
                Double.TryParse(lblSubTotalCompetency.Text, out SubordinateCompetencyTotal);
                Double.TryParse(lblSupTotalCompetency.Text, out SupervisorCompetencyTotal);

                if ((SubordinateKPITotal + SubordinateCompetencyTotal) > 0)
                {
                    SubordinateAverage = ((SubordinateKPITotal + SubordinateCompetencyTotal) / 2);
                }
                if ((SupervisorKPITotal + SupervisorCompetencyTotal) > 0)
                {
                    SupervisorAverage = ((SupervisorKPITotal + SupervisorCompetencyTotal) / 2);
                }

                lblSubAvgForm.Text = @"(" + SubordinateKPITotal + @"+" + SubordinateCompetencyTotal + @")/2";
                lblSupAvgForm.Text = @"(" + SupervisorKPITotal + @"+" + SupervisorCompetencyTotal + @")/2";

                lblSubordinateAverage.Text = SubordinateAverage.ToString();
                lblSupervisorAverage.Text = SupervisorAverage.ToString();
            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
                dtProficiencyLevels.Dispose();
            }
        }

        void LoadSupervisorComments()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtSupervisorComments = new DataTable();
            try
            {
                string AssessmentID = hfAssessmentID.Value;
                string EmployeeID = hfEmployeeID.Value;
                string YearOfAssessment = hfYearOfAssessment.Value;

                dtSupervisorComments = PEDH.PopulateSupervisorComments(AssessmentID, EmployeeID, YearOfAssessment).Copy();

                if (dtSupervisorComments.Rows.Count > 0)
                {
                    string SupervisorComments = dtSupervisorComments.Rows[0]["APPRASER_COMMENTS"].ToString();
                    string Recommendation = dtSupervisorComments.Rows[0]["RECOMMENDATION"].ToString();
                    string TrainingNeeds = dtSupervisorComments.Rows[0]["TRAINING_NEEDS"].ToString();
                    string IsFeedbackSubmitted = dtSupervisorComments.Rows[0]["IS_FEEDBACK_SUBMITTED"].ToString();

                    txtSupervisorComment.Text = HttpUtility.HtmlDecode(SupervisorComments).ToString().Trim();
                    txtSupervisorRecommendation.Text = HttpUtility.HtmlDecode(Recommendation).ToString().Trim();
                    txtTrainingNeeds.Text = HttpUtility.HtmlDecode(TrainingNeeds).ToString().Trim();

                    if (HttpUtility.HtmlDecode(IsFeedbackSubmitted).ToString().Trim() == Constants.CON_ACTIVE_STATUS)
                    {
                        rbtFeedbackGivenEmpYes.Checked = true;
                        rbtFeedbackGivenEmpNo.Checked = false;
                    }
                    else
                    {
                        rbtFeedbackGivenEmpYes.Checked = false;
                        rbtFeedbackGivenEmpNo.Checked = true;
                    }
                }

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
                dtSupervisorComments.Dispose();
            }
        }

        void SaveSupervisorComments()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                string AssessmentID = hfAssessmentID.Value;
                string EmployeeID = hfEmployeeID.Value;
                string YearOfAssessment = hfYearOfAssessment.Value;
                string SupervisorComments = txtSupervisorComment.Text.Trim();
                string SupervisorRecommendation = txtSupervisorRecommendation.Text.Trim();
                string TrainingNeeds = txtTrainingNeeds.Text.Trim();
                string GrivanceComments = txtConsequenceDisagreements.Text.Trim();
                Boolean IsFeedbackSubmitted = false;
                IsFeedbackSubmitted = rbtFeedbackGivenEmpYes.Checked;

                PEDH.UpdateSupervisorComments(SupervisorComments, SupervisorRecommendation, TrainingNeeds, IsFeedbackSubmitted, AssessmentID, EmployeeID, YearOfAssessment, GrivanceComments);

                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_COMPLETED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblSupervisorMessage);

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblSupervisorMessage);
            }
            finally
            {
                PEDH = null;
            }
        }

        void LoadEmployeeComments()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtSupervisorComments = new DataTable();
            try
            {
                string AssessmentID = hfAssessmentID.Value;
                string EmployeeID = hfEmployeeID.Value;
                string YearOfAssessment = hfYearOfAssessment.Value;

                dtSupervisorComments = PEDH.PopulateEmployeeComments(AssessmentID, EmployeeID, YearOfAssessment).Copy();

                if (dtSupervisorComments.Rows.Count > 0)
                {
                    string Disagreements = dtSupervisorComments.Rows[0]["DISAGREEMENTS"].ToString();
                    string GrivanceComments = dtSupervisorComments.Rows[0]["GRIVANCE_COMMENTS"].ToString();
                    string IsEmployeeAgreed = dtSupervisorComments.Rows[0]["IS_EMPLOYEE_AGREED"].ToString();

                    txtEmployeeDisagreeComment.Text = HttpUtility.HtmlDecode(Disagreements).ToString().Trim();
                    txtConsequenceDisagreements.Text = HttpUtility.HtmlDecode(GrivanceComments).ToString().Trim();

                    if ((HttpUtility.HtmlDecode(IsEmployeeAgreed).ToString().Trim() == Constants.CON_ACTIVE_STATUS)||(HttpUtility.HtmlDecode(IsEmployeeAgreed).ToString().Trim()==String.Empty))
                    {
                        rbtEmployeeAgreed.Checked = true;
                        rbtEmployeeDisagreed.Checked = false;
                        txtEmployeeDisagreeComment.Enabled = false;
                    }
                    else
                    {
                        rbtEmployeeAgreed.Checked = false;
                        rbtEmployeeDisagreed.Checked = true;
                        txtEmployeeDisagreeComment.Enabled = true;
                    }





                    if (rbtEmployeeAgreed.Checked == true)
                    {
                        txtEmployeeDisagreeComment.Enabled = false;
                        txtConsequenceDisagreements.Enabled = false;
                    }
                    else
                    {
                        txtEmployeeDisagreeComment.Enabled = true;
                        txtConsequenceDisagreements.Enabled = true;
                    }
                }

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
                dtSupervisorComments.Dispose();
            }
        }

        void SaveEmployeeComments()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                string AssessmentID = hfAssessmentID.Value;
                string EmployeeID = hfEmployeeID.Value;
                string YearOfAssessment = hfYearOfAssessment.Value;
                string Disagreements = txtEmployeeDisagreeComment.Text.Trim();
                //string GrivanceComments = txtConsequenceDisagreements.Text.Trim();
                Boolean IsEmployeeAgreed = false;
                IsEmployeeAgreed = rbtEmployeeAgreed.Checked;


                PEDH.UpdateEmployeeComments(Disagreements, IsEmployeeAgreed, AssessmentID, EmployeeID, YearOfAssessment);

                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblSubordinateMessage);

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblSubordinateMessage);
            }
            finally
            {
                PEDH = null;
            }
        }

        void LoadCEOComments()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtSupervisorComments = new DataTable();
            try
            {
                string AssessmentID = hfAssessmentID.Value;
                string EmployeeID = hfEmployeeID.Value;
                string YearOfAssessment = hfYearOfAssessment.Value;

                dtSupervisorComments = PEDH.PopulateCEOComments(AssessmentID, EmployeeID, YearOfAssessment).Copy();

                if (dtSupervisorComments.Rows.Count > 0)
                {
                    string CEOComments = dtSupervisorComments.Rows[0]["CEO_COMMENTS"].ToString();
                    string IsIncrementGranted = dtSupervisorComments.Rows[0]["IS_INCREMENT_GRANTED"].ToString();
                    string IncrementPercentage = dtSupervisorComments.Rows[0]["INCREMENT_PERCENTAGE"].ToString();
                    string IsToBeReviewed = dtSupervisorComments.Rows[0]["TO_BE_REVIEWED"].ToString();
                    string ReviewMonths = dtSupervisorComments.Rows[0]["REVIEW_MONTHS"].ToString();
                    string IsConfrimed = dtSupervisorComments.Rows[0]["IS_CONFRIMED"].ToString();
                    string IsTrainingEnd = dtSupervisorComments.Rows[0]["IS_TRAINING_END"].ToString();
                    string IsProbationExtended = dtSupervisorComments.Rows[0]["IS_PROBATION_EXTENDED"].ToString();
                    string ExtendedMonths = dtSupervisorComments.Rows[0]["EXTENDED_MONTHS"].ToString();

                    txtCEOComments.Text = HttpUtility.HtmlDecode(CEOComments).ToString().Trim();

                    if (HttpUtility.HtmlDecode(IsIncrementGranted).ToString().Trim() == Constants.CON_ACTIVE_STATUS)
                    {
                        chkIncrementGranted.Checked = true;
                        txtIncrementGranted.Text = HttpUtility.HtmlDecode(IncrementPercentage).ToString().Trim();
                    }
                    else
                    {
                        chkIncrementGranted.Checked = false;
                        txtIncrementGranted.Text = String.Empty;
                    }


                    if (HttpUtility.HtmlDecode(IsToBeReviewed).ToString().Trim() == Constants.CON_ACTIVE_STATUS)
                    {
                        chkToBeReviewed.Checked = true;
                        txtToBeReviewed.Text = HttpUtility.HtmlDecode(ReviewMonths).ToString().Trim();
                    }
                    else
                    {
                        chkToBeReviewed.Checked = false;
                        txtToBeReviewed.Text = String.Empty;
                    }


                    if (HttpUtility.HtmlDecode(IsConfrimed).ToString().Trim() == Constants.CON_ACTIVE_STATUS)
                    {
                        chkEmploymentConfrimed.Checked = true;
                        rbtEmploymentConfrimedYes.Checked = true;
                        rbtEmploymentConfrimedNo.Checked = false;
                    }
                    else
                    {
                        chkEmploymentConfrimed.Checked = false;
                        rbtEmploymentConfrimedYes.Checked = false;
                        rbtEmploymentConfrimedNo.Checked = true;
                    }


                    if (HttpUtility.HtmlDecode(IsTrainingEnd).ToString().Trim() == Constants.CON_ACTIVE_STATUS)
                    {
                        chkEndTraining.Checked = true;
                        rbtEndTrainingYes.Checked = true;
                        rbtEndTrainingNo.Checked = false;
                    }
                    else
                    {
                        chkEndTraining.Checked = false;
                        rbtEndTrainingYes.Checked = false;
                        rbtEndTrainingNo.Checked = true;
                    }


                    if (HttpUtility.HtmlDecode(IsProbationExtended).ToString().Trim() == Constants.CON_ACTIVE_STATUS)
                    {
                        chkProbationExtended.Checked = true;
                        txtProbationExtended.Text = HttpUtility.HtmlDecode(ExtendedMonths).ToString().Trim();
                    }
                    else
                    {
                        chkProbationExtended.Checked = false;
                        txtProbationExtended.Text = String.Empty;
                    }
                }

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
                dtSupervisorComments.Dispose();
            }
        }

        void SaveCEOComments()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                string AssessmentID = hfAssessmentID.Value;
                string EmployeeID = hfEmployeeID.Value;
                string YearOfAssessment = hfYearOfAssessment.Value;

                string CEOComments = txtCEOComments.Text.Trim();

                Boolean isIncrementGranted = chkIncrementGranted.Checked;
                string IncrementPercentage = txtIncrementGranted.Text.Trim();

                Boolean ToBeReviewed = chkToBeReviewed.Checked;
                string ToBeReviewedMonths = txtToBeReviewed.Text.Trim();

                Boolean EmploymentConfrimed = chkIncrementGranted.Checked;
                Boolean EmploymentConfrimedYes = rbtEmploymentConfrimedYes.Checked;
                Boolean EmploymentConfrimedNo = rbtEmploymentConfrimedNo.Checked;

                Boolean EndTraining = chkEndTraining.Checked;
                Boolean EndTrainingYes = rbtEndTrainingYes.Checked;
                Boolean EndTrainingNo = rbtEndTrainingNo.Checked;

                Boolean ProbationExtended = chkProbationExtended.Checked;
                string ProbationExtendedMonths = txtProbationExtended.Text;

                


                PEDH.UpdateCEOComments(CEOComments, isIncrementGranted, IncrementPercentage, ToBeReviewed, ToBeReviewedMonths, EmploymentConfrimedYes, EndTrainingYes, ProbationExtended, ProbationExtendedMonths, AssessmentID, EmployeeID, YearOfAssessment);

                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_COMPLETED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMessage);

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
            }
        }

        void FinalizeCEOComments()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                string AssessmentID = hfAssessmentID.Value;
                string EmployeeID = hfEmployeeID.Value;
                string YearOfAssessment = hfYearOfAssessment.Value;

                string CEOComments = txtCEOComments.Text.Trim();

                Boolean isIncrementGranted = chkIncrementGranted.Checked;
                string IncrementPercentage = txtIncrementGranted.Text.Trim();

                Boolean ToBeReviewed = chkToBeReviewed.Checked;
                string ToBeReviewedMonths = txtToBeReviewed.Text.Trim();

                Boolean EmploymentConfrimed = chkIncrementGranted.Checked;
                Boolean EmploymentConfrimedYes = rbtEmploymentConfrimedYes.Checked;
                Boolean EmploymentConfrimedNo = rbtEmploymentConfrimedNo.Checked;

                Boolean EndTraining = chkEndTraining.Checked;
                Boolean EndTrainingYes = rbtEndTrainingYes.Checked;
                Boolean EndTrainingNo = rbtEndTrainingNo.Checked;

                Boolean ProbationExtended = chkProbationExtended.Checked;
                string ProbationExtendedMonths = txtProbationExtended.Text;

                PEDH.FinalizeCEOComments(CEOComments, isIncrementGranted, IncrementPercentage, ToBeReviewed, ToBeReviewedMonths, EmploymentConfrimedYes, EndTrainingYes, ProbationExtended, ProbationExtendedMonths, AssessmentID, EmployeeID, YearOfAssessment);

                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_FINALIZED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMessage);

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
            }
        }

        void SupCompleteEvaluation()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                PEDH.SupervisorCompleteEvaluation(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value);
                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_COMPLETED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblSupervisorMessage);
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblSupervisorMessage);
            }
            finally
            {
                PEDH = null;
            }
        }

        void SupFinalizeEvaluation()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                //double TotalGoalScore;
                //double TotalCompetencyScore;
                //double TotalScore;

                //if ((Double.TryParse(lblSupervisorsTotal.Text.Trim(), out TotalGoalScore)) && (Double.TryParse(lblSupTotalCompetency.Text.Trim(), out TotalCompetencyScore)) && (Double.TryParse(lblSupervisorAverage.Text.Trim(), out TotalScore)))
                //{
                //    PEDH.SupervisorFinalizeEvaluation(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value, lblSupTotalCompetency.Text.Trim(), lblSupervisorsTotal.Text.Trim(), lblSupervisorAverage.Text.Trim());

                //    CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_FINALIZED;
                //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblSupervisorMessage);
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}

                PEDH.SupervisorFinalizeEvaluation(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value, lblSupTotalCompetency.Text.Trim(), lblSupervisorsTotal.Text.Trim(), lblSupervisorAverage.Text.Trim());

                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_FINALIZED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblSupervisorMessage);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                PEDH = null;
            }
        }

        void CEOFinalizeEvaluation()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                PEDH.CEOFinalizeEvaluation(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value);
                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_FINALIZED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
            }
        }

        void SupervisorClear()
        {
            try
            {
                Utility.Utils.clearControls(false, txtSupervisorComment, txtSupervisorRecommendation, txtTrainingNeeds);
                rbtFeedbackGivenEmpYes.Checked = false;
                rbtFeedbackGivenEmpNo.Checked = false;
                Utility.Errorhandler.ClearError(lblSupervisorMessage);
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblSupervisorMessage);
            }
            finally
            {

            }
        }

        void EmployeeClear()
        {
            try
            {
                Utility.Utils.clearControls(false, txtEmployeeDisagreeComment, txtConsequenceDisagreements);
                txtEmployeeDisagreeComment.Enabled = true;
                txtConsequenceDisagreements.Enabled = true;
                rbtEmployeeAgreed.Checked = false;
                rbtEmployeeDisagreed.Checked = false;
                Utility.Errorhandler.ClearError(lblSubordinateMessage);
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblSubordinateMessage);
            }
            finally
            {

            }
        }

        void CEOClear()
        {
            try
            {
                Utility.Utils.clearControls(false, txtCEOComments, chkIncrementGranted, txtIncrementGranted, chkToBeReviewed, txtToBeReviewed, chkEmploymentConfrimed, chkEndTraining, chkProbationExtended, txtProbationExtended);
                rbtEmploymentConfrimedYes.Checked = false;
                rbtEmploymentConfrimedNo.Checked = false;
                rbtEndTrainingYes.Checked = false;
                rbtEndTrainingNo.Checked = false;

                Utility.Errorhandler.ClearError(lblMessage);
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        void EmployeeView()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                //Enable Subordinate Controls

                rbtEmployeeAgreed.Enabled = true;
                rbtEmployeeDisagreed.Enabled = true;

                if (rbtEmployeeAgreed.Checked == true)
                {
                    txtEmployeeDisagreeComment.Enabled = false;
                }
                else
                {
                    txtEmployeeDisagreeComment.Enabled = true;
                }

                btnEmployeeComplete.Enabled = true;
                btnEmployeeClear.Enabled = true;


                //Disable Supervisor Controls

                txtSupervisorComment.Enabled = false;
                txtSupervisorRecommendation.Enabled = false;
                txtTrainingNeeds.Enabled = false;
                txtConsequenceDisagreements.Enabled = false;

                rbtFeedbackGivenEmpYes.Enabled = false;
                rbtFeedbackGivenEmpNo.Enabled = false;

                btnSupervisorComplete.Enabled = false;
                btnSupervisorClear.Enabled = false;
                btnFinalizeSupervisorEvaluation.Enabled = false;

                //Disable CEO Controls

                txtCEOComments.Enabled = false;

                chkIncrementGranted.Enabled = false;
                txtIncrementGranted.Enabled = false;
                chkToBeReviewed.Enabled = false;
                txtToBeReviewed.Enabled = false;
                chkEmploymentConfrimed.Enabled = false;
                rbtEmploymentConfrimedYes.Enabled = false;
                rbtEmploymentConfrimedNo.Enabled = false;
                chkEndTraining.Enabled = false;
                rbtEndTrainingYes.Enabled = false;
                rbtEndTrainingNo.Enabled = false;
                chkProbationExtended.Enabled = false;
                txtProbationExtended.Enabled = false;

                btnCEOComplete.Enabled = false;
                btnCEOClear.Enabled = false;
                btnCEOFinalize.Enabled = false;


                string EmployeeAgreeStatus = PEDH.EmployeeAgreedStatus(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value);
                Boolean isSupervisorCompleted = PEDH.IsSupervisorCompleted(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value);

                if (isSupervisorCompleted)
                {
                    if ((EmployeeAgreeStatus == Constants.CON_ACTIVE_STATUS) || ((EmployeeAgreeStatus == Constants.CON_INACTIVE_STATUS)))
                    {
                        rbtEmployeeAgreed.Enabled = false;
                        rbtEmployeeDisagreed.Enabled = false;

                        txtEmployeeDisagreeComment.Enabled = false;

                        btnEmployeeComplete.Enabled = false;
                        btnEmployeeClear.Enabled = false;
                    }
                    else if (EmployeeAgreeStatus == String.Empty)
                    {
                        rbtEmployeeAgreed.Enabled = true;
                        rbtEmployeeDisagreed.Enabled = true;

                        txtEmployeeDisagreeComment.Enabled = false;

                        btnEmployeeComplete.Enabled = true;
                        btnEmployeeClear.Enabled = true;
                    }
                }



                if (PEDH.IsSupervisorFinalized(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value) == true)
                {
                    //When Supervisor Finalized.
                    rbtEmployeeAgreed.Enabled = false;
                    rbtEmployeeDisagreed.Enabled = false;

                    txtEmployeeDisagreeComment.Enabled = false;
                    txtConsequenceDisagreements.Enabled = false;

                    btnEmployeeComplete.Enabled = false;
                    btnEmployeeClear.Enabled = false;
                }

                if (PEDH.IsCEOFinalized(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value) == true)
                {
                    rbtEmployeeAgreed.Enabled = false;
                    rbtEmployeeDisagreed.Enabled = false;

                    txtEmployeeDisagreeComment.Enabled = false;

                    btnEmployeeComplete.Enabled = false;
                    btnEmployeeClear.Enabled = false;
                }


                if (PEDH.IsClosedOrObsolete(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value) == true)
                {
                    ClosedAndObsoleteView();
                }

                isCEOFinalized();
                btnCEOFinalize.Enabled = false;


                if (rbtEmployeeDisagreed.Checked == true)
                {
                    btnCEOFinalize.Enabled = false;
                    btnFinalizeSupervisorEvaluation.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
            }
        }

        void SupervisorView()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                //Disable Subordinate Controls

                rbtEmployeeAgreed.Enabled = false;
                rbtEmployeeDisagreed.Enabled = false;

                txtEmployeeDisagreeComment.Enabled = false;

                btnEmployeeComplete.Enabled = false;
                btnEmployeeClear.Enabled = false;

                //Enable Supervisor Controls

                txtSupervisorComment.Enabled = true;
                txtSupervisorRecommendation.Enabled = true;
                txtTrainingNeeds.Enabled = true;
                txtConsequenceDisagreements.Enabled = true;

                if (txtEmployeeDisagreeComment.Text == String.Empty)
                {
                    txtConsequenceDisagreements.Enabled = false;
                }
                else
                {
                    txtConsequenceDisagreements.Enabled = true;
                }

                rbtFeedbackGivenEmpYes.Enabled = true;
                rbtFeedbackGivenEmpNo.Enabled = true;

                btnSupervisorComplete.Enabled = true;
                btnSupervisorClear.Enabled = true;
                btnFinalizeSupervisorEvaluation.Enabled = true;


                //Disable CEO Controls

                txtCEOComments.Enabled = false;

                chkIncrementGranted.Enabled = false;
                txtIncrementGranted.Enabled = false;
                chkToBeReviewed.Enabled = false;
                txtToBeReviewed.Enabled = false;
                chkEmploymentConfrimed.Enabled = false;
                rbtEmploymentConfrimedYes.Enabled = false;
                rbtEmploymentConfrimedNo.Enabled = false;
                chkEndTraining.Enabled = false;
                rbtEndTrainingYes.Enabled = false;
                rbtEndTrainingNo.Enabled = false;
                chkProbationExtended.Enabled = false;
                txtProbationExtended.Enabled = false;

                btnCEOComplete.Enabled = false;
                btnCEOClear.Enabled = false;
                btnCEOFinalize.Enabled = false;



                isEmployeeAgreed();


                string EmployeeAgreeStatus = PEDH.EmployeeAgreedStatus(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value);
                Boolean isSupervisorCompleted = PEDH.IsSupervisorCompleted(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value);

                if (isSupervisorCompleted)
                {
                    if (EmployeeAgreeStatus == String.Empty)
                    {
                        txtSupervisorComment.Enabled = false;
                        txtSupervisorRecommendation.Enabled = false;
                        txtTrainingNeeds.Enabled = false;
                        txtConsequenceDisagreements.Enabled = false;

                        rbtFeedbackGivenEmpYes.Enabled = false;
                        rbtFeedbackGivenEmpNo.Enabled = false;

                        btnSupervisorComplete.Enabled = false;
                        btnSupervisorClear.Enabled = false;
                        btnFinalizeSupervisorEvaluation.Enabled = false;
                    }
                    else
                    {
                        if (EmployeeAgreeStatus == Constants.CON_ACTIVE_STATUS)
                        {
                            txtSupervisorComment.Enabled = false;
                            txtSupervisorRecommendation.Enabled = false;
                            txtTrainingNeeds.Enabled = false;
                            txtConsequenceDisagreements.Enabled = false;

                            rbtFeedbackGivenEmpYes.Enabled = false;
                            rbtFeedbackGivenEmpNo.Enabled = false;

                            btnSupervisorComplete.Enabled = false;
                            btnSupervisorClear.Enabled = false;
                        }
                        else
                        {
                            txtSupervisorComment.Enabled = true;
                            txtSupervisorRecommendation.Enabled = true;
                            txtTrainingNeeds.Enabled = true;
                            txtConsequenceDisagreements.Enabled = true;
                            if (txtEmployeeDisagreeComment.Text == String.Empty)
                            {
                                txtConsequenceDisagreements.Enabled = false;
                            }
                            else
                            {
                                txtConsequenceDisagreements.Enabled = true;
                            }

                            rbtFeedbackGivenEmpYes.Enabled = true;
                            rbtFeedbackGivenEmpNo.Enabled = true;

                            btnSupervisorComplete.Enabled = true;
                            btnSupervisorClear.Enabled = true;
                            btnFinalizeSupervisorEvaluation.Enabled = true;
                        }
                    }
                }


                if (PEDH.IsCEOFinalized(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value) == true)
                {
                    //When CEO Finalized.
                    txtSupervisorComment.Enabled = false;
                    txtSupervisorRecommendation.Enabled = false;
                    txtTrainingNeeds.Enabled = false;
                    txtConsequenceDisagreements.Enabled = false;

                    rbtFeedbackGivenEmpYes.Enabled = false;
                    rbtFeedbackGivenEmpNo.Enabled = false;

                    btnSupervisorComplete.Enabled = false;
                    btnSupervisorClear.Enabled = false;
                    btnFinalizeSupervisorEvaluation.Enabled = false; 
                    btnCEOFinalize.Enabled = false;
                }

                if (PEDH.IsClosedOrObsolete(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value) == true)
                {
                    ClosedAndObsoleteView();
                }

                if (rbtEmployeeDisagreed.Checked == true)
                {
                    btnCEOFinalize.Enabled = false;
                    btnFinalizeSupervisorEvaluation.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
            }
        }

        void CEOView()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                //Disable Subordinate Controls

                rbtEmployeeAgreed.Enabled = false;
                rbtEmployeeDisagreed.Enabled = false;

                txtEmployeeDisagreeComment.Enabled = false;

                btnEmployeeComplete.Enabled = false;
                btnEmployeeClear.Enabled = false;

                //Disable Supervisor Controls

                txtSupervisorComment.Enabled = false;
                txtSupervisorRecommendation.Enabled = false;
                txtTrainingNeeds.Enabled = false;
                txtConsequenceDisagreements.Enabled = false;

                rbtFeedbackGivenEmpYes.Enabled = false;
                rbtFeedbackGivenEmpNo.Enabled = false;

                btnSupervisorComplete.Enabled = false;
                btnSupervisorClear.Enabled = false;
                btnFinalizeSupervisorEvaluation.Enabled = false;

                //Enable CEO Controls

                txtCEOComments.Enabled = true;

                chkIncrementGranted.Enabled = true;
                txtIncrementGranted.Enabled = true;
                chkToBeReviewed.Enabled = true;
                txtToBeReviewed.Enabled = true;
                chkEmploymentConfrimed.Enabled = true;
                rbtEmploymentConfrimedYes.Enabled = true;
                rbtEmploymentConfrimedNo.Enabled = true;
                chkEndTraining.Enabled = true;
                rbtEndTrainingYes.Enabled = true;
                rbtEndTrainingNo.Enabled = true;
                chkProbationExtended.Enabled = true;
                txtProbationExtended.Enabled = true;

                btnCEOComplete.Enabled = true;
                btnCEOClear.Enabled = true;
                btnCEOFinalize.Enabled = true;

                isCEOFinalized();

                if (PEDH.IsClosedOrObsolete(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value) == true)
                {
                    ClosedAndObsoleteView();
                }

                if (rbtEmployeeDisagreed.Checked == true)
                {
                    btnCEOFinalize.Enabled = false;
                    btnFinalizeSupervisorEvaluation.Enabled = false;
                }

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
            }
        }

        void DisableViews()
        {
            try
            {
                //Disable Subordinate Controls

                rbtEmployeeAgreed.Enabled = false;
                rbtEmployeeDisagreed.Enabled = false;

                txtEmployeeDisagreeComment.Enabled = false;
                txtConsequenceDisagreements.Enabled = false;

                btnEmployeeComplete.Enabled = false;
                btnEmployeeClear.Enabled = false;

                //Disable Supervisor Controls

                txtSupervisorComment.Enabled = false;
                txtSupervisorRecommendation.Enabled = false;
                txtTrainingNeeds.Enabled = false;

                rbtFeedbackGivenEmpYes.Enabled = false;
                rbtFeedbackGivenEmpNo.Enabled = false;

                btnSupervisorComplete.Enabled = false;
                btnSupervisorClear.Enabled = false;

                //Disable CEO Controls

                txtCEOComments.Enabled = false;

                chkIncrementGranted.Enabled = false;
                txtIncrementGranted.Enabled = false;
                chkToBeReviewed.Enabled = false;
                txtToBeReviewed.Enabled = false;
                chkEmploymentConfrimed.Enabled = false;
                rbtEmploymentConfrimedYes.Enabled = false;
                rbtEmploymentConfrimedNo.Enabled = false;
                chkEndTraining.Enabled = false;
                rbtEndTrainingYes.Enabled = false;
                rbtEndTrainingNo.Enabled = false;
                chkProbationExtended.Enabled = false;
                txtProbationExtended.Enabled = false;

                btnCEOComplete.Enabled = false;
                btnCEOClear.Enabled = false;

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        void SelectUser()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                string LoggedUser = (Session["KeyEMPLOYEE_ID"] as string).Trim();

                string EmployeeID = hfEmployeeID.Value.Trim();
                string Supervisor = PEDH.GetAppraser(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value).ToString().Trim();
                string CEO = PEDH.GetCEO(hfEmployeeID.Value).ToString().Trim();

                Boolean DisableView = true;

                //if (LoggedUser == EmployeeID)
                //{
                //    DisableView = false;
                //    EmployeeView();
                //}

                //if (LoggedUser == Supervisor)
                //{
                //    DisableView = false;
                //    SupervisorView();
                //}

                //if (LoggedUser == CEO)
                //{
                //    DisableView = false;
                //    CEOView();
                //}
                //New Indetification Method
                string DashBoardType = (Session["DASHBOARD"] as string);

                if (DashBoardType == "EMP")
                {
                    DisableView = false;
                    EmployeeView();
                }
                else if (DashBoardType == "SUP")
                {
                    DisableView = false;
                    SupervisorView();
                }
                else if (DashBoardType == "CEO")
                {
                    DisableView = false;
                    CEOView();
                }
                //--
                if (DisableView)
                {
                    DisableViews();
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
            }
        }

        void isEmployeeAgreed()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                if (PEDH.IsEmployeeAgreed(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value))
                {
                    if (PEDH.IsSupervisorFinalized(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value))
                    {
                        btnFinalizeSupervisorEvaluation.Enabled = false;
                        btnSupervisorComplete.Enabled = false;
                        btnSupervisorClear.Enabled = false;
                    }
                    else
                    {
                        btnFinalizeSupervisorEvaluation.Enabled = true;
                    }
                }
                else
                {
                    btnFinalizeSupervisorEvaluation.Enabled = false;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblSupervisorMessage);
            }
            finally
            {
                PEDH = null;
            }
        }

        void isCEOFinalized()
        {
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                if (PEDH.IsCEOFinalized(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value))
                {
                    btnCEOFinalize.Enabled = false;

                    btnCEOComplete.Enabled = false;
                    btnCEOClear.Enabled = false;
                    txtCEOComments.Enabled = false;
                    chkIncrementGranted.Enabled = false;
                    chkToBeReviewed.Enabled = false;
                    chkEmploymentConfrimed.Enabled = false;
                    chkEndTraining.Enabled = false;
                    chkProbationExtended.Enabled = false;
                    txtIncrementGranted.Enabled = false;
                    txtToBeReviewed.Enabled = false;
                    rbtEmploymentConfrimedYes.Enabled = false;
                    rbtEmploymentConfrimedNo.Enabled = false;
                    rbtEndTrainingYes.Enabled = false;
                    rbtEndTrainingNo.Enabled = false;
                    txtProbationExtended.Enabled = false;
                }
                else
                {
                    btnCEOFinalize.Enabled = true;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
            }
        }

        void ClosedAndObsoleteView()
        {
            try
            {
                //Disable Subordinate Controls

                rbtEmployeeAgreed.Enabled = false;
                rbtEmployeeDisagreed.Enabled = false;

                txtEmployeeDisagreeComment.Enabled = false;
                txtConsequenceDisagreements.Enabled = false;

                btnEmployeeComplete.Enabled = false;
                btnEmployeeClear.Enabled = false;

                //Disable Supervisor Controls

                txtSupervisorComment.Enabled = false;
                txtSupervisorRecommendation.Enabled = false;
                txtTrainingNeeds.Enabled = false;

                rbtFeedbackGivenEmpYes.Enabled = false;
                rbtFeedbackGivenEmpNo.Enabled = false;

                btnSupervisorComplete.Enabled = false;
                btnSupervisorClear.Enabled = false;
                btnFinalizeSupervisorEvaluation.Enabled = false;

                //Disable CEO Controls

                txtCEOComments.Enabled = false;

                chkIncrementGranted.Enabled = false;
                txtIncrementGranted.Enabled = false;
                chkToBeReviewed.Enabled = false;
                txtToBeReviewed.Enabled = false;
                chkEmploymentConfrimed.Enabled = false;
                rbtEmploymentConfrimedYes.Enabled = false;
                rbtEmploymentConfrimedNo.Enabled = false;
                chkEndTraining.Enabled = false;
                rbtEndTrainingYes.Enabled = false;
                rbtEndTrainingNo.Enabled = false;
                chkProbationExtended.Enabled = false;
                txtProbationExtended.Enabled = false;

                btnCEOComplete.Enabled = false;
                btnCEOClear.Enabled = false;
                btnCEOFinalize.Enabled = false;

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        void SendSupervisorCompletedEmail()
        {
            StringBuilder stringBuilder = new StringBuilder();
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                log.Debug("SendSupervisorCompletedEmail()");
                string SubordinateEmail = GetEmailAddress(hfEmployeeID.Value);

                if (SubordinateEmail != String.Empty)
                {
                    stringBuilder.Append("Dear Sir/Madam," + "<br /><br />");
                    stringBuilder.Append("Supervisor has completed your performance evaluation. Please carefully review it and provide your");
                    stringBuilder.Append("<br />");
                    stringBuilder.Append("consent. If you agree please select agreed option. If disagreed, select disagree option and make sure to");
                    stringBuilder.Append("<br />");
                    stringBuilder.Append("provide valid reasons for you disagreement.");
                    stringBuilder.Append("<br />");
                    stringBuilder.Append("Please submit your feedback within the day itself you received this notice to finish your ");
                    stringBuilder.Append("<br />");
                    stringBuilder.Append("evaluation on or before the performance evaluation cutoff date which has already informed you. ");
                    stringBuilder.Append("<br /><br />");
                    stringBuilder.Append("Thank you." + "<br />");
                    stringBuilder.Append("This is a system generated mail." + "<br />");

                    EmailHandler.SendDefaultEmailWithHTML("Performance Evaluation", SubordinateEmail, "", "Consent on Supervisor Feedback", stringBuilder);
                }
            }
            catch (Exception ex)
            {
                log.Error("SendSupervisorCompletedEmail()" + ex.Message);
                throw ex;
            }
            finally
            {
                PEDH = null;
                stringBuilder = null;
            }
        }//Modified

        void SendSupervisorFinalizedEmail()
        {
            StringBuilder stringBuilder = new StringBuilder();
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtCEODetails = new DataTable();
            try
            {
                log.Debug("SendSupervisorFinalizedEmail()");
                //Send Email to Subordinate
                string SubordinateEmail = GetEmailAddress(hfEmployeeID.Value);                
                if (SubordinateEmail != String.Empty)
                {
                    stringBuilder.Append("Dear Sir/Madam, <br /> <br />");
                    stringBuilder.Append("This is to notify that supervisor has finalized your performance evaluation (" + lblEvaluationName.Text + ").");
                    stringBuilder.Append("<br /> <br />" );
                    stringBuilder.Append("Thank you."+ "<br />");
                    stringBuilder.Append("This is a system generated mail." + "<br />");

                    EmailHandler.SendDefaultEmailWithHTML("Performance Evaluation", SubordinateEmail, "", "Performance Evaluation Notification – Supervisor Finalized", stringBuilder);
                }

                ////Send Email to CEO
                //dtCEODetails = PEDH.PopulateCEODetails(hfEmployeeID.Value);
                //string CutOffDate = PEDH.GetCutOffDate(hfAssessmentID.Value);
                //if (dtCEODetails.Rows.Count > 0)
                //{
                //    string CEOName = dtCEODetails.Rows[0]["TITLE"].ToString() + " " + dtCEODetails.Rows[0]["INITIALS_NAME"].ToString();
                //    string CEOEmail = dtCEODetails.Rows[0]["EMAIL"].ToString();

                //    if (CEOEmail != String.Empty)
                //    {
                //        stringBuilder = new StringBuilder();

                //        stringBuilder.Append("Dear Sir/Madam, <br /> <br />");
                //        stringBuilder.Append("Supervisor has finalized Mr. /Ms. " + PEDH.GetEmployeeName(hfEmployeeID.Value) + "’s performance evaluation. <br />");
                //        stringBuilder.Append("dully provide your feedback to complete the evaluation on or before the cutoff date. <br />");
                //        stringBuilder.Append("Details of the evaluation is as follows. <br /><br />");
                //        stringBuilder.Append("Assessment Name : " + lblEvaluationName.Text + "<br />");
                //        stringBuilder.Append("Assessment Type : " + lblEvaluationType.Text + "<br />");
                //        stringBuilder.Append("Assessment Purposes : " + lblAssessmentPurposes.Text + "<br />");
                //        stringBuilder.Append("Cutoff Date : " + CutOffDate + "<br /><br />");
                //        stringBuilder.Append("Thank you. <br />");
                //        stringBuilder.Append("This is a system generated mail." + "<br />");

                //        EmailHandler.SendDefaultEmailWithHTML("Performance Evaluation", CEOEmail, "", PEDH.GetEmployeeName(hfEmployeeID.Value) + "’s – Performance Evaluation", stringBuilder);
                //    }
                //}
            }
            catch (Exception ex)
            {
                log.Error("SendSupervisorFinalizedEmail()" + ex.Message);
                throw ex;
            }
            finally
            {
                PEDH = null;
                stringBuilder = null;
                dtCEODetails.Dispose();
            }
        }//Modified

        void SendSubordinateAgreedEmail()
        {
            StringBuilder stringBuilder = new StringBuilder();
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtSupervisor = new DataTable();
            try
            {
                log.Debug("SendSubordinateAgreedEmail()");
                string SubordinateEmail = GetEmailAddress(hfEmployeeID.Value);

                dtSupervisor = PEDH.PopulateSupervisorDetails(hfEmployeeID.Value, hfAssessmentID.Value, hfYearOfAssessment.Value).Copy();
                if (dtSupervisor.Rows.Count > 0)
                {
                    string SupervisorEmployeeID = dtSupervisor.Rows[0]["EMPLOYEE_ID"].ToString();
                    string SupervisorEmail = dtSupervisor.Rows[0]["EMAIL"].ToString();



                    if (SupervisorEmail != String.Empty)
                    {
                        stringBuilder.Append("Dear Sir/Madam,<br /><br />");
                        stringBuilder.Append(PEDH.GetEmployeeName(hfEmployeeID.Value) + " has agreed with your feedback. Now you can finalized his/her <br />");
                        stringBuilder.Append("performance evaluation. Please duly finalize the evaluation to forward it to CEO/COO. <br />");
                        stringBuilder.Append("Your quick response will be helpful to finish performance evaluation <br /> on or before the performance evaluation cutoff date. <br /><br />");
                        stringBuilder.Append("Assessment Name : " + lblEvaluationName.Text + "<br />" + "<br />");
                        stringBuilder.Append("<br />" + "<br />");
                        stringBuilder.Append("Thank you. <br />");
                        stringBuilder.Append("This is a system generated mail.");

                        EmailHandler.SendDefaultEmailWithHTML("Performance Evaluation", SupervisorEmail, "", PEDH.GetEmployeeName(hfEmployeeID.Value)+"’s Consent – Agreed with Your Feedback", stringBuilder);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("SendSubordinateAgreedEmail()" + ex.Message);
                throw ex;
            }
            finally
            {
                dtSupervisor.Dispose();
                PEDH = null;
                stringBuilder = null;
            }
        }//Modified

        void SendSubordinateDisagreedEmail()
        {
            StringBuilder stringBuilder = new StringBuilder();
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtSupervisor = new DataTable();
            try
            {
                log.Debug("SendSubordinateDisagreedEmail()");
                string SubordinateEmail = GetEmailAddress(hfEmployeeID.Value);

                dtSupervisor = PEDH.PopulateSupervisorDetails(hfEmployeeID.Value, hfAssessmentID.Value, hfYearOfAssessment.Value).Copy();
                if (dtSupervisor.Rows.Count > 0)
                {
                    string SupervisorEmployeeID = dtSupervisor.Rows[0]["EMPLOYEE_ID"].ToString();
                    string SupervisorEmail = dtSupervisor.Rows[0]["EMAIL"].ToString();

                    if (SupervisorEmail != String.Empty)
                    {
                        stringBuilder.Append("Dear Sir/Madam," + "<br />" + "<br />");
                        stringBuilder.Append(PEDH.GetEmployeeName(hfEmployeeID.Value) + " has disagreed with your feedback. Please discuss grievances subjected for the  <br />");
                        stringBuilder.Append("disagreement and dully complete this performance evaluation to finish it on or before the performance <br />");
                        stringBuilder.Append("evaluation cutoff date. <br />");
                        stringBuilder.Append("Thank you." + "<br />" + "<br />");
                        stringBuilder.Append("This is a system generated mail." + "<br />");

                        EmailHandler.SendDefaultEmailWithHTML("Performance Evaluation", SupervisorEmail, "", PEDH.GetEmployeeName(hfEmployeeID.Value) + "’s Consent – Disagreed with Your Feedback", stringBuilder);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("SendSubordinateDisagreedEmail()" + ex.Message);
                throw ex;
            }
            finally
            {
                dtSupervisor.Dispose();
                PEDH = null;
                stringBuilder = null;
            }
        }//Modified

        void SendCEOFinalizedEmail()
        {
            StringBuilder stringBuilder = new StringBuilder();
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtCEODetails = new DataTable();
            DataTable dtSupervisor = new DataTable();
            try
            {
                log.Debug("SendCEOFinalizedEmail()");
                string SubordinateEmail = GetEmailAddress(hfEmployeeID.Value);

                //Send Email to Subordinate
                if (SubordinateEmail != String.Empty)
                {
                    stringBuilder.Append("Dear Sir/Madam, <br /> <br />");
                    stringBuilder.Append("This is to notify that CEO/COO has finalized your performance evaluation (" + lblEvaluationName.Text + ").<br /><br />");
                    stringBuilder.Append("Thank you. <br />");
                    stringBuilder.Append("This is a system generated mail." + "<br />");

                    EmailHandler.SendDefaultEmailWithHTML("Performance Evaluation", SubordinateEmail, "", "Performance Evaluation Notification – CEO Finalized", stringBuilder);
                }

                //Send Email to Supervisor
                dtCEODetails = PEDH.PopulateCEODetails(hfEmployeeID.Value);
                dtSupervisor = PEDH.PopulateSupervisorDetails(hfEmployeeID.Value, hfAssessmentID.Value, hfYearOfAssessment.Value).Copy();

                if ((dtCEODetails.Rows.Count > 0) && (dtSupervisor.Rows.Count > 0))
                {
                    //string CEOName = dtCEODetails.Rows[0]["TITLE"].ToString() + " " + dtCEODetails.Rows[0]["INITIALS_NAME"].ToString();
                    //string CEOEmail = dtCEODetails.Rows[0]["EMAIL"].ToString();
                    string SupervisorName = dtSupervisor.Rows[0]["TITLE"].ToString() + " " + dtSupervisor.Rows[0]["INITIALS_NAME"].ToString();
                    string SupervisorEmail = dtSupervisor.Rows[0]["EMAIL"].ToString();

                    if (SupervisorEmail != String.Empty)
                    {
                        stringBuilder = new StringBuilder();

                        stringBuilder.Append("Dear Sir/Madam, <br /> <br />");
                        stringBuilder.Append("CEO has finalized " + PEDH.GetEmployeeName(hfEmployeeID.Value) + "'s performance evaluation(" + lblEvaluationName.Text + "). Please review it. <br /><br />");
                        stringBuilder.Append("Thank you. <br /> <br />");
                        stringBuilder.Append("This is a system generated mail." + "<br />");

                        EmailHandler.SendDefaultEmailWithHTML("Performance Evaluation", SupervisorEmail, "", "Performance Evaluation Notification – CEO Finalized", stringBuilder);
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error("SendCEOFinalizedEmail()" + ex.Message);
                throw ex;
            }
            finally
            {
                dtCEODetails.Dispose();
                dtSupervisor.Dispose();
                PEDH = null;
                stringBuilder = null;
            }
        }//Modified

        string GetEmailAddress(string EmployeeID)
        {
            string EmailAddress = String.Empty;
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            try
            {
                log.Debug("GetEmailAddress()");
                EmailAddress = PEDH.GetEmailAddress(EmployeeID);
            }
            catch (Exception ex)
            {
                log.Error("GetEmailAddress()" + ex.Message);
                throw ex;
            }
            finally
            {
                PEDH = null;
            }
            return EmailAddress;
        }

        void DisplayChart()
        {
            log.Debug("DisplayChart()");
            PerformanceEvaluationDataHandler PEDH = new PerformanceEvaluationDataHandler();
            DataTable dtPreviousCompetencies = new DataTable();
            try
            {
                dtPreviousCompetencies = PEDH.PopulatePreviousData(hfEmployeeID.Value).Copy();
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

                                        
					                                    <canvas id='canvas'></canvas>
                                        <br>
                                        <br>
                                        <script>
                                            var config = {
                                                type: 'line',
                                                data: {
                                                    labels: [" + JSLabel + @"],
                                                    datasets: [{
                                                        label: 'Employee Achievement(s)',
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
                                                ctx.canvas.width = 800;
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
                PEDH = null;
            }
        }

        #endregion

    }
}