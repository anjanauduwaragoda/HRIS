using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.TrainingAndDevelopment;
using Common;
using GroupHRIS.Utility;
using NLog;
using DataHandler.Userlogin;
using System.Data;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmRatingQuestionPopup : System.Web.UI.Page
    {

        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            PasswordHandler crpto;
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmRatingQuestion : Page_Load");
            
            Errorhandler.ClearError(lblMessage);

            if (!IsPostBack)
            {
                crpto = new PasswordHandler();
                fillStatus(ddlStatus);
                //fillNoOfAns();
                loadRatingQuestions(txtEvalId.Text);

                string programId = Request.QueryString["EvaluationId"];
                string programName = Request.QueryString["evaluationName"];
                string ratingScheme = Request.QueryString["ratingScheme"]; //SchemeName
                string schemename = Request.QueryString["SchemeName"];

                txtEvalId.Text = crpto.Decrypt(programId);
                lblEvalName.Text = programName;

                if (ratingScheme == "")
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Please Select the Rating Scheme", lblMessage);
                    btnSave.Enabled = false;
                    btnClear.Enabled = false;
                    return;
                }

                viewRatingSchemeDetails();
            }

            if (hfCaller.Value == "txtEvalId")
            {
                clear();
                hfCaller.Value = "";
                txtEvalId.Text = hfVal.Value;
                lblEvalName.Text = hfEvalName.Value;
               
            }

            loadRatingQuestionsWithEval(txtEvalId.Text);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("btnClear_Click()");
                Errorhandler.ClearError(lblMessage);
                clear();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            Utility.Errorhandler.ClearError(lblMessage);
            RatingQuestionDataHandler RQDH = new RatingQuestionDataHandler();

            try
            {
                string evalId = txtEvalId.Text.Trim();
                string question = txtQuestion.Text;
                //string noAns = ddlNoOfAns.SelectedItem.ToString();
                string status = ddlStatus.SelectedItem.Value.ToString();
                string addedBy = Session["KeyUSER_ID"].ToString();

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Insert");
                    bool isInsert = RQDH.Insert(evalId, question, status, addedBy);
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblMessage);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> update");
                    string quId = Session["QuestionId"].ToString();
                    bool isUpdate = RQDH.Update(quId, evalId, question, status, addedBy); ;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
                }
                clear();
                loadRatingQuestions(txtEvalId.Text);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                RQDH = null;
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
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdQuestion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("grdQuestion_PageIndexChanging()");

            try
            {
                grdQuestion.PageIndex = e.NewPageIndex;
                loadRatingQuestions(txtEvalId.Text);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        protected void grdQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                Utility.Errorhandler.ClearError(lblMessage);
                int SelectedIndex = grdQuestion.SelectedIndex;

                txtEvalId.Text = Server.HtmlDecode(grdQuestion.Rows[SelectedIndex].Cells[0].Text.ToString());
                txtQuestion.Text = Server.HtmlDecode(grdQuestion.Rows[SelectedIndex].Cells[2].Text.ToString());
                //ddlNoOfAns.SelectedValue = Server.HtmlDecode(grdQuestion.Rows[SelectedIndex].Cells[3].Text.ToString());

                string status = Server.HtmlDecode(grdQuestion.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));

                Session["QuestionId"] = Server.HtmlDecode(grdQuestion.Rows[SelectedIndex].Cells[1].Text.ToString());
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
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

        //public void fillNoOfAns()
        //{
        //    try
        //    {
        //        log.Debug("fillNoOfAns()");
        //        ddlNoOfAns.Items.Clear();
        //        ddlNoOfAns.Items.Add(("").ToString());
        //        for (int i = 1; i <= 5; i++)
        //        {
        //            ddlNoOfAns.Items.Add((i).ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonVariables.MESSAGE_TEXT = ex.Message;
        //        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
        //    }
        //}

        public void loadRatingQuestions(string evalId)
        {
            RatingQuestionDataHandler RQDH = new RatingQuestionDataHandler();
            log.Debug("loadRatingQuestions()");
            try
            {
                grdQuestion.DataSource = RQDH.getAllQuestions(evalId);
                grdQuestion.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                RQDH = null;
            }
        }

        public void clear()
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            //txtEvalId.Text = "";
            //lblEvalName.Text = "";
            txtQuestion.Text = "";
            //ddlNoOfAns.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
        }

        public void loadRatingQuestionsWithEval(string eval)
        {
            RatingQuestionDataHandler RQDH = new RatingQuestionDataHandler();

            try
            {
                grdQuestion.DataSource = RQDH.getAllQuestionsWithEvaluation(eval);
                grdQuestion.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                RQDH = null;
            }
        }

        public void viewRatingSchemeDetails()
        {
            ProgramEvaluationDataHandler PEDH = new ProgramEvaluationDataHandler();
            DataTable dt = new DataTable();

            try
            {
                dt = PEDH.getRatingSchemeDetails(Request.QueryString["ratingScheme"]);
                grdRatingDetails.DataSource = dt;
                grdRatingDetails.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
                dt.Dispose();
            }
        }

    }
}