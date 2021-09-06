using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.TrainingAndDevelopment;
using System.Data;
using Common;
using GroupHRIS.Utility;
using NLog;
using DataHandler.Userlogin;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmMCQPopup : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            PasswordHandler crpto;
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmMultipleChoiceQuestion : Page_Load");

            if (!IsPostBack)
            {
                crpto = new PasswordHandler();
                fillStatus(ddlStatus);
                fillStatus(ddlAnsStatus);
                string programId = Request.QueryString["EvaluationId"];
                string evalName = Request.QueryString["evaluationName"];
                string test = Request.QueryString["remarks"];

                if (grdAnswer.Rows.Count > 0)
                {

                }
                else
                {
                    createAnswerBucket();
                }
                lblEvalName.Text = evalName;
                txtEvalId.Text = crpto.Decrypt(programId);
                grdAnswer.DataSource = null;
                grdAnswer.DataBind();

                if (test != null)
                {
                    tblVisible.Visible = false;
                }
                else
                {
                    tblVisible.Visible = true;
                }
            }

            if (hfCaller.Value == "txtEvalId")
            {
                Errorhandler.ClearError(lblMessage);
                clearAns();
                clear();
                hfCaller.Value = "";
                txtEvalId.Text = hfVal.Value;
                lblEvalName.Text = hfEvalName.Value;
                
            }
            loadQuestionGrid(txtEvalId.Text);
        }

        protected void btnAnsClear_Click(object sender, EventArgs e)
        {
            try
            {
                Errorhandler.ClearError(lblMessage);
                clearAns();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable ansdt = (DataTable)Session["createAnswerBucket"];

            try
            {
                log.Debug("btnAdd_Click()");
                Errorhandler.ClearError(lblMessage);

                string answer = txtAnswer.Text;
                string status = ddlAnsStatus.SelectedItem.Text;
                string isAnswer = "";


                if (chkIsAnswer.Checked == true)
                {
                    isAnswer = "Yes";
                }
                else
                {
                    isAnswer = "No";
                }

                foreach (DataRow drow in ansdt.Rows)
                {
                    string ansId = drow["ANSWER_ID"].ToString();
                    string isExist = drow["IS_ANSWER"].ToString();

                    if (isAnswer == "Yes" && ansId != Session["answerId"].ToString())
                    {
                        if (isExist == isAnswer)
                        {
                            Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Only one answer is possible", lblMessage);
                            return;
                        }
                    }

                }


                if (btnAdd.Text == Constants.CON_MODIFY_BUTTON_TEXT)
                {
                    foreach (DataRow dr in ansdt.Rows)
                    {
                        string ansId = dr["ANSWER_ID"].ToString();
                        //update existing dt
                        if (ansId == Session["answerId"].ToString())
                        {
                            DataRow drUpdateAnswer = ansdt.Select("ANSWER_ID ='" + ansId + "' ").First();

                            drUpdateAnswer["ANSWER"] = answer.Trim();
                            drUpdateAnswer["IS_ANSWER"] = isAnswer.Trim();
                            drUpdateAnswer["STATUS_CODE"] = status.Trim();
                        }
                    }
                }
                else
                {
                    DataRow dr = ansdt.NewRow();

                    dr["ANSWER_ID"] = (ansdt.Rows.Count + 1);
                    dr["ANSWER"] = answer.Trim();
                    dr["IS_ANSWER"] = isAnswer.Trim();
                    dr["STATUS_CODE"] = status.Trim();

                    ansdt.Rows.Add(dr);
                    Session["createAnswerBucket"] = ansdt;

                    lblChoice.Text = "Answer No : " + (ansdt.Rows.Count + 1);
                }
                fillAnswer();
                clearAns();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            MultipleChoiceQuestionDataHandler MCQDH = new MultipleChoiceQuestionDataHandler();
            DataTable dt = new DataTable();

            try
            {
                log.Debug("btnSave_Click()");
                string logUser = Session["KeyUSER_ID"].ToString();
                string evaluationId = txtEvalId.Text;
                string question = txtQuestion.Text;
                string choice = ddlChoice.SelectedItem.Text;
                string sts = ddlStatus.SelectedItem.Text;

                dt = (DataTable)Session["createAnswerBucket"];

                Errorhandler.ClearError(lblMessage);

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    Boolean isSuccess = MCQDH.Insert(evaluationId, question, choice, sts, dt, logUser);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblMessage);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    //string locationId = hfglId.Value.ToString();
                    string questionId = Session["QuestionId"].ToString();
                    Boolean isUpdate = MCQDH.Update(questionId, evaluationId, question, choice, sts, dt, logUser);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
                }
                loadQuestionGrid(txtEvalId.Text);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                MCQDH = null;
                dt.Dispose();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");
            Errorhandler.ClearError(lblMessage);
            Session["createAnswerBucket"] = null;
            clear();
            grdAnswer.DataSource = null;
            grdAnswer.DataBind();
        }

        protected void ddlChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlChoice.SelectedIndex != 0)
            {
                txtAnswer.Enabled = true;
                ddlAnsStatus.Enabled = true;
                chkIsAnswer.Enabled = true;
                btnAdd.Enabled = true;
                btnAnsClear.Enabled = true;

                lblChoice.Text = "Answer No : 1";
            }
        }

        protected void grdQuestion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("grdQuestion_PageIndexChanging()");

            try
            {
                grdQuestion.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);
                loadQuestionGrid(txtEvalId.Text);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdQuestion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdQuestion, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void grdQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("grdQuestion_SelectedIndexChanged()");
            Errorhandler.ClearError(lblMessage);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

            try
            {
                int SelectedIndex = grdQuestion.SelectedIndex;
                string questionId = Server.HtmlDecode(grdQuestion.Rows[SelectedIndex].Cells[0].Text.ToString());
                txtQuestion.Text = Server.HtmlDecode(grdQuestion.Rows[SelectedIndex].Cells[1].Text.ToString());
                ddlChoice.SelectedValue = Server.HtmlDecode(grdQuestion.Rows[SelectedIndex].Cells[2].Text.ToString());
                string status = Server.HtmlDecode(grdQuestion.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));

                loadAnswerGrid(txtEvalId.Text, questionId);
                enableAnswer(true);
                lblChoice.Text = "Answer No : " + (grdAnswer.Rows.Count + 1);
                Session["QuestionId"] = questionId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void grdAnswer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdAnswer, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void grdAnswer_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("grdAnswer_SelectedIndexChanged()");
            Errorhandler.ClearError(lblMessage);
            btnAdd.Text = Constants.CON_MODIFY_BUTTON_TEXT;

            try
            {
                int SelectedIndex = grdAnswer.SelectedIndex;
                string answerId = Server.HtmlDecode(grdAnswer.Rows[SelectedIndex].Cells[0].Text.ToString());
                txtAnswer.Text = Server.HtmlDecode(grdAnswer.Rows[SelectedIndex].Cells[1].Text.ToString());
                string isAnswer = Server.HtmlDecode(grdAnswer.Rows[SelectedIndex].Cells[2].Text.ToString());
                Session["answerId"] = answerId;

                if (isAnswer == "Yes")
                {
                    chkIsAnswer.Checked = true;
                }
                else
                {
                    chkIsAnswer.Checked = false;
                }

                string status = Server.HtmlDecode(grdAnswer.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
                ddlAnsStatus.SelectedIndex = ddlAnsStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));
                lblChoice.Text = "Answer No : " + answerId;
                //

            }
            catch (Exception)
            {

                throw;
            }

        }


        public void fillStatus(DropDownList ddl)
        {
            log.Debug("fillStatus()");
            try
            {
                ddl.Items.Insert(0, new ListItem("", ""));
                ddl.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                ddl.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void clearAns()
        {
            txtAnswer.Text = "";
            ddlAnsStatus.SelectedIndex = 0;
            chkIsAnswer.Checked = false;
            lblChoice.Text = "";
        }

        public void clear()
        {
            lblEvalName.Text = "";
            txtQuestion.Text = "";
            ddlStatus.SelectedIndex = 0;
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            btnAdd.Text = Constants.CON_ADD_BUTTON_TEXT;
            clearAns();
            ddlChoice.SelectedIndex = 0;
        }

        public void createAnswerBucket()
        {
            log.Debug("createAnswerBucket()");
            DataTable answerBucket = new DataTable();
            answerBucket.Columns.Add("ANSWER_ID", typeof(string));
            answerBucket.Columns.Add("ANSWER", typeof(string));
            answerBucket.Columns.Add("STATUS_CODE", typeof(string));
            answerBucket.Columns.Add("IS_ANSWER", typeof(string));

            Session["createAnswerBucket"] = answerBucket;
        }

        public void fillAnswer()
        {
            DataTable ansdt = new DataTable();
            ansdt = (DataTable)Session["createAnswerBucket"];

            grdAnswer.DataSource = ansdt;
            grdAnswer.DataBind();
        }

        public void loadQuestionGrid(string evalId)
        {
            MultipleChoiceQuestionDataHandler MCQDH = new MultipleChoiceQuestionDataHandler();
            DataTable questonDt = new DataTable();

            try
            {
                questonDt = MCQDH.getMCQ(evalId);
                grdQuestion.DataSource = questonDt;
                grdQuestion.DataBind();
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

        public void loadAnswerGrid(string evalId, string questionId)
        {
            MultipleChoiceQuestionDataHandler MCQDH = new MultipleChoiceQuestionDataHandler();
            DataTable questonDt = new DataTable();

            try
            {
                createAnswerBucket();
                questonDt = MCQDH.getMCQAnswers(evalId, questionId);
                Session["createAnswerBucket"] = questonDt;
                grdAnswer.DataSource = questonDt;
                grdAnswer.DataBind();
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

        public void enableAnswer(bool status)
        {
            txtAnswer.Enabled = status;
            ddlAnsStatus.Enabled = status;
            chkIsAnswer.Enabled = status;
            btnAdd.Enabled = status;
            btnAnsClear.Enabled = status;
        }


    }
}