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

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmEssayQuestion : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmRatingQuestion : Page_Load");

            if (!IsPostBack)
            {
                fillStatus(ddlStatus);
                fillNoOfAns();
                loadEssayQuestions();
            }

            if (hfCaller.Value == "txtEvalId")
            {
                clear();
                hfCaller.Value = "";
                txtEvalId.Text = hfVal.Value;
                lblEvalName.Text = hfEvalName.Value;
                loadEsssayQuestionsWithEval(hfVal.Value);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            Utility.Errorhandler.ClearError(lblMessage);
            EssayQuestionDataHandler EQDH = new EssayQuestionDataHandler();

            try
            {
                string evalId = txtEvalId.Text.Trim();
                string question = txtQuestion.Text;
                string noAns = ddlNoOfAns.SelectedItem.ToString();
                string status = ddlStatus.SelectedItem.Value.ToString();
                string addedBy = Session["KeyUSER_ID"].ToString();

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Insert");
                    bool isInsert = EQDH.Insert(evalId, question, noAns, status, addedBy);
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblMessage);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> update");
                    string quId = Session["QuestionId"].ToString();
                    bool isUpdate = EQDH.Update(quId, evalId, question, noAns, status, addedBy); ;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
                }
                loadEssayQuestions();
                clear();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                EQDH = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            clear();
        }

        protected void grdEssayQuestion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdEssayQuestion, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdEssayQuestion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("grdEssayQuestion_PageIndexChanging()");

            try
            {
                grdEssayQuestion.PageIndex = e.NewPageIndex;
                loadEssayQuestions();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
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

        public void fillNoOfAns()
        {
            try
            {
                ddlNoOfAns.Items.Clear();
                ddlNoOfAns.Items.Add(("").ToString());
                for (int i = 1; i <= 5; i++)
                {
                    ddlNoOfAns.Items.Add((i).ToString());
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        public void clear()
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            txtEvalId.Text = "";
            lblEvalName.Text = "";
            txtQuestion.Text = "";
            ddlNoOfAns.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
        }

        public void loadEssayQuestions()
        {
            EssayQuestionDataHandler EQDH = new EssayQuestionDataHandler();

            try
            {
                grdEssayQuestion.DataSource = EQDH.getAllQuestions(txtEvalId.Text);
                grdEssayQuestion.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                EQDH = null;
            }
        }

        public void loadEsssayQuestionsWithEval(string eval)
        {
            EssayQuestionDataHandler EQDH = new EssayQuestionDataHandler();

            try
            {
                grdEssayQuestion.DataSource = EQDH.getAllQuestionsWithEvaluation(eval);
                grdEssayQuestion.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                EQDH = null;
            }
        }

        protected void grdEssayQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                Utility.Errorhandler.ClearError(lblMessage);
                int SelectedIndex = grdEssayQuestion.SelectedIndex;

                txtEvalId.Text = Server.HtmlDecode(grdEssayQuestion.Rows[SelectedIndex].Cells[0].Text.ToString());
                txtQuestion.Text = Server.HtmlDecode(grdEssayQuestion.Rows[SelectedIndex].Cells[3].Text.ToString());
                ddlNoOfAns.SelectedValue = Server.HtmlDecode(grdEssayQuestion.Rows[SelectedIndex].Cells[4].Text.ToString());

                string status = Server.HtmlDecode(grdEssayQuestion.Rows[SelectedIndex].Cells[5].Text.ToString().Trim());
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));
                lblEvalName.Text = Server.HtmlDecode(grdEssayQuestion.Rows[SelectedIndex].Cells[1].Text.ToString());
                Session["QuestionId"] = Server.HtmlDecode(grdEssayQuestion.Rows[SelectedIndex].Cells[2].Text.ToString());
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }



    }
}