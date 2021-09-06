using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using DataHandler.PerformanceManagement;
using System.Data;
using Common;
using DataHandler.Userlogin;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmCEOSelfAssessment : System.Web.UI.Page
    {
        public String Questions = String.Empty;
        public String DisplayAssessmentName = String.Empty;
        public String DisplayAssessmentPurposes = String.Empty;
        private static Logger log = LogManager.GetCurrentClassLogger();

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblStatus);
            if (!IsPostBack)
            {
                PasswordHandler crpto;
                try
                {
                    crpto = new PasswordHandler();

                    string AssessmentID = Request.QueryString["assmtId"];
                    string YearOfAssessmentID = Request.QueryString["year"];
                    string EmployeeID = Request.QueryString["employeeId"];

                    hfAssessmentID.Value = crpto.Decrypt(AssessmentID);
                    hfYearOfAssessment.Value = crpto.Decrypt(YearOfAssessmentID);
                    hfEmployeeID.Value = crpto.Decrypt(EmployeeID);

                    LoadEmployeeSelfAssessmentDetails();
                    LoadAssessmentPurposes();
                    LoadEmployeeSelfAssessmentQuestions();

                    LoadPreviousComments();
                    LoadSupervisorName();
                    //EnableComments();
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
            else
            {
                try
                {
                    LoadEmployeeSelfAssessmentDetails();
                    LoadAssessmentPurposes();
                    LoadEmployeeSelfAssessmentQuestions();
                    //LoadPreviousComments();
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

        #endregion

        #region Methods

        void LoadEmployeeSelfAssessmentDetails()
        {
            SupervisorSelfAssessmentReviewDatahandler SSARDH;
            DataTable dtEmployeeSelfAssessmentDetails = new DataTable();
            try
            {
                SSARDH = new SupervisorSelfAssessmentReviewDatahandler();
                dtEmployeeSelfAssessmentDetails = SSARDH.PopulateSelfAssessmentDetails(hfAssessmentID.Value.Trim(), hfYearOfAssessment.Value.Trim(), hfEmployeeID.Value.Trim()).Copy();
                if (dtEmployeeSelfAssessmentDetails.Rows.Count > 0)
                {
                    hfAssessmentToken.Value = dtEmployeeSelfAssessmentDetails.Rows[0]["ASSESSMENT_TOKEN"].ToString();
                    hfSelfAssessmentProfileID.Value = dtEmployeeSelfAssessmentDetails.Rows[0]["SELF_ASSESSMENT_PROFILE_ID"].ToString();
                    hfStatusCode.Value = dtEmployeeSelfAssessmentDetails.Rows[0]["STATUS_CODE"].ToString();
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SSARDH = null;
                dtEmployeeSelfAssessmentDetails.Dispose();
            }
        }

        void LoadAssessmentPurposes()
        {
            SupervisorCompetencyAssessmentDataHandler SEADH;
            DataTable AssessmentPurposes = new DataTable();
            DataTable SubordinatesInfo = new DataTable();
            try
            {
                SEADH = new SupervisorCompetencyAssessmentDataHandler();
                AssessmentPurposes = SEADH.PopulateAssessmentPurposes(hfAssessmentID.Value.Trim()).Copy();
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

        void LoadEmployeeSelfAssessmentQuestions()
        {
            SupervisorSelfAssessmentReviewDatahandler SSARDH;
            DataTable dtEmployeeSelfAssessmentQuestions = new DataTable();
            DataTable dtEmployeeSelfAssessmentAnswers = new DataTable();
            try
            {
                SSARDH = new SupervisorSelfAssessmentReviewDatahandler();
                dtEmployeeSelfAssessmentQuestions = SSARDH.PopulateAnswersWithRequiredAnswerCount(hfSelfAssessmentProfileID.Value.Trim()).Copy();
                dtEmployeeSelfAssessmentAnswers = SSARDH.PopulateEmployeeAnswers(hfAssessmentToken.Value.Trim()).Copy();
                if (dtEmployeeSelfAssessmentQuestions.Rows.Count > 0)
                {
                    for (int i = 0; i < dtEmployeeSelfAssessmentQuestions.Rows.Count; i++)
                    {
                        Questions += "<div style='padding:10px;' class='Question'>";
                        Questions += "Q" + (i + 1) + ". " + dtEmployeeSelfAssessmentQuestions.Rows[i]["QUESTION"].ToString();
                        Questions += "</div>";


                        string QuestionID = dtEmployeeSelfAssessmentQuestions.Rows[i]["QUESTION_ID"].ToString();

                        int NumberOfAnswers = 0;
                        if (!Int32.TryParse(dtEmployeeSelfAssessmentQuestions.Rows[i]["NO_OF_ANSWERS"].ToString(), out NumberOfAnswers))
                        {
                            NumberOfAnswers = 0;
                        }
                        DataRow[] Answers = dtEmployeeSelfAssessmentAnswers.Select("QUESTION_ID = '" + QuestionID + "'");
                        int AnswerCount = Answers.Length;

                        Questions += @"<ol>";
                        for (int j = 0; j < Answers.Length; j++)
                        {
                            if (Answers[j]["ANSWER"].ToString().Trim() != "")
                            {
                                Questions += "<div class='Answer'>";
                                Questions += @"<li style='list-style-type: none;'>";
                                Questions += @"A" + (j + 1) + @". " + Answers[j]["ANSWER"].ToString().Trim();
                                Questions += @"</li>";
                                Questions += "</div>";
                            }
                        }
                        Questions += @"</ol>";



                        Questions += "<br/>";
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
                SSARDH = null;
                dtEmployeeSelfAssessmentQuestions.Dispose();
                dtEmployeeSelfAssessmentAnswers.Dispose();
            }
        }

        void LoadPreviousComments()
        {
            SupervisorSelfAssessmentReviewDatahandler SSARDH;
            DataTable dtPreviousComments = new DataTable();
            try
            {
                SSARDH = new SupervisorSelfAssessmentReviewDatahandler();
                dtPreviousComments = SSARDH.PopulatePreviousComments(hfAssessmentID.Value.Trim(), hfEmployeeID.Value.Trim(), hfYearOfAssessment.Value.Trim()).Copy();
                if (dtPreviousComments.Rows.Count > 0)
                {
                    string Comment = dtPreviousComments.Rows[0]["COMMENTS"].ToString();
                    if (Comment == "")
                    {
                        //btnSave.Text = Constants.CON_COMPLETE_BUTTON_TEXT;
                    }
                    else
                    {
                        //btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                        txtSupervisorComments.Text = dtPreviousComments.Rows[0]["COMMENTS"].ToString();
                    }
                }
                else
                {
                    //btnSave.Text = Constants.CON_COMPLETE_BUTTON_TEXT;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SSARDH = null;
                dtPreviousComments.Dispose();
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

        #endregion

    }
}