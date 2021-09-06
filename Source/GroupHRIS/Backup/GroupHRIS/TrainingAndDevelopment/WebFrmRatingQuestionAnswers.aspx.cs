using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using Common;
using GroupHRIS.Utility;
using DataHandler.TrainingAndDevelopment;
using System.Data;
using System.Web.UI.HtmlControls;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmRatingQuestionAnswers : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        private void LoadEvaluationGrid()
        {
            RatingQuestionAnswersDataHandler RQADH = new RatingQuestionAnswersDataHandler();
            try
            {
                string EmployeeID = (Session["KeyEMPLOYEE_ID"] as string);
                grdvTrainingEvaluations.DataSource = RQADH.PopulateEvaluations(EmployeeID).Copy();
                grdvTrainingEvaluations.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                RQADH = null;
            }
        }

        private void LoadQuestionPaper(string EvaluationID, string TrainingID)
        {
            RatingQuestionAnswersDataHandler RQDH = new RatingQuestionAnswersDataHandler();
            DataTable dtRQ = new DataTable();
            DataTable dtRQAnswers = new DataTable();
            try
            {
                dtRQ = RQDH.PopulateRatingQuestions(EvaluationID).Copy();
                dtRQAnswers = RQDH.PopulateRQAnswers(EvaluationID).Copy();

                string EmployeeID = (Session["KeyEMPLOYEE_ID"] as string);

                for (int i = 0; i < dtRQ.Rows.Count; i++)
                {
                    HtmlGenericControl Question_li = new HtmlGenericControl("li");
                    Question_li.ID = "li_" + dtRQ.Rows[i]["RQ_ID"].ToString();
                    Question_li.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    olQpaper.Controls.Add(Question_li);

                    Label lblQuestion = new Label();
                    lblQuestion.ID = "lbl_" + dtRQ.Rows[i]["RQ_ID"].ToString();
                    lblQuestion.Text = dtRQ.Rows[i]["QUESTION"].ToString();
                    lblQuestion.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    Question_li.Controls.Add(lblQuestion);

                    HtmlGenericControl Answers_ol = new HtmlGenericControl("ol");
                    Answers_ol.ID = "ol_" + dtRQ.Rows[i]["RQ_ID"].ToString();
                    Answers_ol.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    Question_li.Controls.Add(Answers_ol);

                    DataRow[] Answers = dtRQAnswers.Select("");
                    for (int j = 0; j < Answers.Length; j++)
                    {

                        HtmlGenericControl Answerli = new HtmlGenericControl("li");
                        Answerli.ID = "Ansli_" + dtRQ.Rows[i]["RQ_ID"].ToString() + "_" + Answers[j]["RS_ID"].ToString() + "_" + Answers[j]["RATING"].ToString();
                        Answerli.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                        Answers_ol.Controls.Add(Answerli);

                        RadioButton rdbtnMCQAnswer = new RadioButton();
                        rdbtnMCQAnswer.ID = "chk_" + EvaluationID + "_" + dtRQ.Rows[i]["RQ_ID"].ToString() + "_" + TrainingID + "_" + EmployeeID + "_" + Answers[j]["RATING"].ToString();
                        rdbtnMCQAnswer.CheckedChanged += chkClick;
                        rdbtnMCQAnswer.AutoPostBack = true;
                        rdbtnMCQAnswer.Text = Answers[j]["RATING"].ToString();
                        rdbtnMCQAnswer.GroupName = dtRQ.Rows[i]["RQ_ID"].ToString();
                        rdbtnMCQAnswer.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                        Answerli.Controls.Add(rdbtnMCQAnswer);
                    }

                    HtmlGenericControl br = new HtmlGenericControl("br");
                    br.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    olQpaper.Controls.Add(br);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtRQ.Dispose();
                dtRQAnswers.Dispose();
                RQDH = null;
            }
        }

        private void GenerateAnswerDataTable()
        {
            DataTable dtAnswers = new DataTable();
            try
            {
                dtAnswers.Columns.Add("EVALUATION_ID");
                dtAnswers.Columns.Add("RQ_ID");
                dtAnswers.Columns.Add("TRAINING_ID");
                dtAnswers.Columns.Add("EMPLOYEE_ID");
                dtAnswers.Columns.Add("ANSWER_ID");

                Session["RQ_ANS"] = dtAnswers.Copy();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtAnswers.Dispose();
            }
        }

        protected void chkClick(object sender, EventArgs e)
        {
            DataTable RQ_ANS = new DataTable();
            try
            {
                log.Debug("WebFrmRatingQuestionAnswers | chkClick()");

                CheckBox chk = (sender as CheckBox);
                string ID = chk.ID;

                string[] IDArr = ID.Split('_');

                RQ_ANS = (Session["RQ_ANS"] as DataTable).Copy();
                DataRow dr = RQ_ANS.NewRow();

                dr["EVALUATION_ID"] = IDArr[1];
                dr["RQ_ID"] = IDArr[2];
                dr["TRAINING_ID"] = IDArr[3];
                dr["EMPLOYEE_ID"] = IDArr[4];
                dr["ANSWER_ID"] = IDArr[5];


                DataRow[] RQ_ANS_ARR = RQ_ANS.Select("EVALUATION_ID = '" + IDArr[1] + "' AND RQ_ID = '" + IDArr[2] + "' AND TRAINING_ID = '" + IDArr[3] + "' AND EMPLOYEE_ID = '" + IDArr[4] + "'");
                foreach (DataRow dtr in RQ_ANS_ARR)
                {
                    dtr.Delete();
                }
                RQ_ANS.Rows.Add(dr);


                Session["RQ_ANS"] = RQ_ANS.Copy();
            }
            catch (Exception ex)
            {
                log.Error("WebFrmRatingQuestionAnswers | chkClick() | " + ex.Message);
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                RQ_ANS.Dispose();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);
                if (grdvTrainingEvaluations.SelectedIndex >= 0)
                {
                    string EvaluationID = HttpUtility.HtmlDecode(grdvTrainingEvaluations.Rows[grdvTrainingEvaluations.SelectedIndex].Cells[0].Text).Trim();
                    string TrainingID = HttpUtility.HtmlDecode(grdvTrainingEvaluations.Rows[grdvTrainingEvaluations.SelectedIndex].Cells[1].Text).Trim();

                    if (EvaluationID != String.Empty)
                    {
                        LoadQuestionPaper(EvaluationID, TrainingID);
                    }
                }
            }
            catch
            {

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sIPAddress = Request.UserHostAddress;
                sUserId = (Session["USER_ID"] as string);
                log.Debug("IP:" + sIPAddress + "WebFrmRatingQuestionAnswers : Page_Load() | User : " + sUserId);

                if (!IsPostBack)
                {
                    LoadEvaluationGrid();
                    GenerateAnswerDataTable();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                log.Error("WebFrmRatingQuestionAnswers | Page_Load() : " + ex.Message);
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void grdvTrainingEvaluations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                log.Debug("WebFrmRatingQuestionAnswers | grdvTrainingEvaluations_RowDataBound()");

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdvTrainingEvaluations, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }

            }
            catch (Exception ex)
            {
                log.Error("WebFrmRatingQuestionAnswers | grdvTrainingEvaluations_RowDataBound() | " + ex.Message);
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void grdvTrainingEvaluations_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmRatingQuestionAnswers | grdvTrainingEvaluations_SelectedIndexChanged()");

                string EvaluationID = HttpUtility.HtmlDecode(grdvTrainingEvaluations.Rows[grdvTrainingEvaluations.SelectedIndex].Cells[0].Text).Trim();
                string TrainingID = HttpUtility.HtmlDecode(grdvTrainingEvaluations.Rows[grdvTrainingEvaluations.SelectedIndex].Cells[1].Text).Trim();
                LoadQuestionPaper(EvaluationID, TrainingID);
            }
            catch (Exception ex)
            {
                log.Error("WebFrmRatingQuestionAnswers | grdvTrainingEvaluations_SelectedIndexChanged() | " + ex.Message);
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void grdvTrainingEvaluations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                log.Debug("WebFrmRatingQuestionAnswers | grdvTrainingEvaluations_PageIndexChanging()");

                grdvTrainingEvaluations.PageIndex = e.NewPageIndex;
                LoadEvaluationGrid();
            }
            catch (Exception ex)
            {
                log.Error("WebFrmRatingQuestionAnswers | grdvTrainingEvaluations_PageIndexChanging() | " + ex.Message);
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            RatingQuestionAnswersDataHandler RQADH = new RatingQuestionAnswersDataHandler();
            DataTable RQ_ANS = new DataTable();
            try
            {
                log.Debug("WebFrmRatingQuestionAnswers | btnSave_Click()");
                RQ_ANS = (Session["RQ_ANS"] as DataTable).Copy();

                RQADH.Insert(RQ_ANS.Copy());

                RQ_ANS.Rows.Clear();
                Session["RQ_ANS"] = RQ_ANS.Copy();

                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED;
                Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMessage);
               
            }
            catch (Exception ex)
            {
                log.Error("WebFrmRatingQuestionAnswers | btnSave_Click() | " + ex.Message);
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                RQ_ANS.Dispose();
                RQADH = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            DataTable RQ_ANS = new DataTable();
            try
            {
                log.Debug("WebFrmRatingQuestionAnswers | btnClear_Click()");

                RQ_ANS = (Session["RQ_ANS"] as DataTable).Copy();
                RQ_ANS.Rows.Clear();
                Session["RQ_ANS"] = RQ_ANS.Copy();
                olQpaper.Controls.Clear();
            }
            catch (Exception ex)
            {
                log.Error("WebFrmRatingQuestionAnswers | btnClear_Click() | " + ex.Message);
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                RQ_ANS.Dispose();
            }
        }
    }
}