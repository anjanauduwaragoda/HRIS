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
    public partial class WebFrmMCQ : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        private void GenerateAnswerDataTable()
        {
            DataTable dtAnswers = new DataTable();
            try
            {
                dtAnswers.Columns.Add("EVALUATION_ID");
                dtAnswers.Columns.Add("MCQ_ID");
                dtAnswers.Columns.Add("TRAINING_ID");
                dtAnswers.Columns.Add("EMPLOYEE_ID");
                dtAnswers.Columns.Add("ANSWER_ID");

                Session["MCQ_ANS"] = dtAnswers.Copy();
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

        private void LoadEvaluationGrid()
        {
            MCQDataHandler MCQDH = new MCQDataHandler();
            try
            {
                string EmployeeID = (Session["KeyEMPLOYEE_ID"] as string);
                grdvTrainingEvaluations.DataSource = MCQDH.PopulateEvaluations(EmployeeID).Copy();
                grdvTrainingEvaluations.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MCQDH = null;
            }
        }

        //private void LoadQuestionPaper(string EvaluationID)
        //{
        //    MCQDataHandler MCQDH = new MCQDataHandler();
        //    DataTable dtMCQ = new DataTable();
        //    DataTable dtMCQAnswers = new DataTable();
        //    try
        //    {
        //        dtMCQ = MCQDH.PopulateMCQ(EvaluationID).Copy();
        //        dtMCQAnswers = MCQDH.PopulateMCQAnswers(EvaluationID).Copy();

        //        lblQuestionPaper.Text = String.Empty;


        //        lblQuestionPaper.Text += "<ol>";
        //        for (int i = 0; i < dtMCQ.Rows.Count; i++)
        //        {
        //            lblQuestionPaper.Text += "<li>";

        //            lblQuestionPaper.Text += dtMCQ.Rows[i]["QUESTION"].ToString();
        //            lblQuestionPaper.Text += "<asp:HiddenField ID='hf_" + i + "' runat='server' Value = '" + dtMCQ.Rows[i]["MCQ_ID"].ToString() + "' />";

        //            lblQuestionPaper.Text += "<ol>";
        //            DataRow[] Answers = dtMCQAnswers.Select("MCQ_ID = '" + dtMCQ.Rows[i]["MCQ_ID"].ToString() + "'");
        //            for (int j = 0; j < Answers.Length; j++)
        //            {
        //                lblQuestionPaper.Text += "<li>";

        //                lblQuestionPaper.Text += " <asp:RadioButton ID='rdbtn_" + Answers[j]["MCQ_ID"].ToString() + "_" + Answers[j]["ANSWER_ID"].ToString() + "' runat='server' Text='" + Answers[j]["ANSWER"].ToString() + "' GroupName = 'Q_" + Answers[j]["MCQ_ID"].ToString() + "'/> ";

        //                lblQuestionPaper.Text += "</li>";
        //            }
        //            lblQuestionPaper.Text += "</ol>";

        //            lblQuestionPaper.Text += "</li>";
        //            lblQuestionPaper.Text += "<br/>";
        //        }
        //        lblQuestionPaper.Text += "</ol>";


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dtMCQ.Dispose();
        //        dtMCQAnswers.Dispose();
        //        MCQDH = null;
        //    }
        //}

        private void LoadQuestionPaper(string EvaluationID, string TrainingID)
        {
            MCQDataHandler MCQDH = new MCQDataHandler();
            DataTable dtMCQ = new DataTable();
            DataTable dtMCQAnswers = new DataTable();
            try
            {
                dtMCQ = MCQDH.PopulateMCQ(EvaluationID).Copy();
                dtMCQAnswers = MCQDH.PopulateMCQAnswers(EvaluationID).Copy();

                string EmployeeID = (Session["KeyEMPLOYEE_ID"] as string);

                for (int i = 0; i < dtMCQ.Rows.Count; i++)
                {
                    HtmlGenericControl Question_li = new HtmlGenericControl("li");
                    Question_li.ID = "li_" + dtMCQ.Rows[i]["MCQ_ID"].ToString();
                    Question_li.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    olQpaper.Controls.Add(Question_li);

                    Label lblQuestion = new Label();
                    lblQuestion.ID = "lbl_" + dtMCQ.Rows[i]["MCQ_ID"].ToString();
                    lblQuestion.Text = dtMCQ.Rows[i]["QUESTION"].ToString();
                    lblQuestion.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    Question_li.Controls.Add(lblQuestion);

                    HtmlGenericControl Answers_ol = new HtmlGenericControl("ol");
                    Answers_ol.ID = "ol_" + dtMCQ.Rows[i]["MCQ_ID"].ToString();
                    Answers_ol.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    Question_li.Controls.Add(Answers_ol);

                    DataRow[] Answers = dtMCQAnswers.Select("MCQ_ID = '" + dtMCQ.Rows[i]["MCQ_ID"].ToString() + "'");
                    for (int j = 0; j < Answers.Length; j++)
                    {

                        HtmlGenericControl Answerli = new HtmlGenericControl("li");
                        Answerli.ID = "Ansli_" + dtMCQ.Rows[i]["MCQ_ID"].ToString() + "_" + Answers[j]["ANSWER_ID"].ToString();
                        Answerli.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                        Answers_ol.Controls.Add(Answerli);

                        RadioButton rdbtnMCQAnswer = new RadioButton();
                        rdbtnMCQAnswer.ID = "chk_" + EvaluationID + "_" + dtMCQ.Rows[i]["MCQ_ID"].ToString() + "_" + TrainingID + "_" + EmployeeID + "_" + Answers[j]["ANSWER_ID"].ToString();
                        rdbtnMCQAnswer.CheckedChanged += chkClick;
                        rdbtnMCQAnswer.AutoPostBack = true;
                        rdbtnMCQAnswer.Text = Answers[j]["ANSWER"].ToString();
                        rdbtnMCQAnswer.GroupName = dtMCQ.Rows[i]["MCQ_ID"].ToString();
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
                dtMCQ.Dispose();
                dtMCQAnswers.Dispose();
                MCQDH = null;
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
                log.Debug("IP:" + sIPAddress + "WebFrmMCQ : Page_Load() | User : " + sUserId);

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
                log.Error("WebFrmMCQ | Page_Load() : " + ex.Message);
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
                log.Debug("WebFrmMCQ | grdvTrainingEvaluations_RowDataBound()");

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
                log.Error("WebFrmMCQ | grdvTrainingEvaluations_RowDataBound() | " + ex.Message);
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
                log.Debug("WebFrmMCQ | grdvTrainingEvaluations_SelectedIndexChanged()");

                string EvaluationID = HttpUtility.HtmlDecode(grdvTrainingEvaluations.Rows[grdvTrainingEvaluations.SelectedIndex].Cells[0].Text).Trim();
                string TrainingID = HttpUtility.HtmlDecode(grdvTrainingEvaluations.Rows[grdvTrainingEvaluations.SelectedIndex].Cells[1].Text).Trim();
                LoadQuestionPaper(EvaluationID, TrainingID);
            }
            catch (Exception ex)
            {
                log.Error("WebFrmMCQ | grdvTrainingEvaluations_SelectedIndexChanged() | " + ex.Message);
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
                log.Debug("WebFrmMCQ | grdvTrainingEvaluations_PageIndexChanging()");

                grdvTrainingEvaluations.PageIndex = e.NewPageIndex;
                LoadEvaluationGrid();
            }
            catch (Exception ex)
            {
                log.Error("WebFrmMCQ | grdvTrainingEvaluations_PageIndexChanging() | " + ex.Message);
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void chkClick(object sender, EventArgs e)
        {
            DataTable MCQ_ANS = new DataTable();
            try
            {
                log.Debug("WebFrmMCQ | chkClick()");

                CheckBox chk = (sender as CheckBox);
                string ID = chk.ID;

                string[] IDArr = ID.Split('_');

                MCQ_ANS = (Session["MCQ_ANS"] as DataTable).Copy();
                DataRow dr = MCQ_ANS.NewRow();

                dr["EVALUATION_ID"] = IDArr[1];
                dr["MCQ_ID"] = IDArr[2];
                dr["TRAINING_ID"] = IDArr[3];
                dr["EMPLOYEE_ID"] = IDArr[4];
                dr["ANSWER_ID"] = IDArr[5];

                
                    DataRow[] MCQ_ANS_ARR = MCQ_ANS.Select("EVALUATION_ID = '" + IDArr[1] + "' AND MCQ_ID = '" + IDArr[2] + "' AND TRAINING_ID = '" + IDArr[3] + "' AND EMPLOYEE_ID = '" + IDArr[4] + "'");
                    foreach (DataRow dtr in MCQ_ANS_ARR)
                    {
                        dtr.Delete();
                    }
                    MCQ_ANS.Rows.Add(dr);
                

                Session["MCQ_ANS"] = MCQ_ANS.Copy();
            }
            catch (Exception ex)
            {
                log.Error("WebFrmMCQ | chkClick() | " + ex.Message);
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                MCQ_ANS.Dispose();
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            MCQDataHandler MCQDH = new MCQDataHandler();
            DataTable MCQ_ANS = new DataTable();
            try
            {
                log.Debug("WebFrmMCQ | btnSave_Click()");
                MCQ_ANS = (Session["MCQ_ANS"] as DataTable).Copy();
                MCQDH.Insert(MCQ_ANS.Copy());

                MCQ_ANS.Rows.Clear();
                Session["MCQ_ANS"] = MCQ_ANS.Copy();


                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED;
                Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            catch (Exception ex)
            {
                log.Error("WebFrmMCQ | btnSave_Click() | " + ex.Message);
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                MCQ_ANS.Dispose();
                MCQDH = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            DataTable MCQ_ANS = new DataTable();
            try
            {
                log.Debug("WebFrmMCQ | btnClear_Click()");
                MCQ_ANS = (Session["MCQ_ANS"] as DataTable).Copy();
                MCQ_ANS.Rows.Clear();
                Session["MCQ_ANS"] = MCQ_ANS.Copy();
                olQpaper.Controls.Clear();
            }
            catch (Exception ex)
            {
                log.Error("WebFrmMCQ | btnClear_Click() | " + ex.Message);
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                MCQ_ANS.Dispose();
            }

        }
    }
}