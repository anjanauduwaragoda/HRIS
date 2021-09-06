using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.TrainingAndDevelopment;
using System.Data;
using Common;
using NLog;
using GroupHRIS.Utility;
using DataHandler.Userlogin;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainingEvaluationDashBoard : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        string KeyEMPLOYEE_ID = "";
        private string evalId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmTrainingEvaluationDashBoard : Page_Load");
            
            if (!IsPostBack)
            {
                KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                getAvailableTrainingEvaluations(KeyEMPLOYEE_ID);
            }
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            log.Debug("btnSearch_Click");

            try
            {
                string assignee = ddlAssignee.SelectedValue;
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                string training = txtTraining.Text;
                string evaluation = ddlPrePostEvaluation.SelectedValue;
                string frmDate = txtFrmDate.Text;
                string toDate = txtToDate.Text;

                evaluationSearch(assignee, KeyEMPLOYEE_ID, training, evaluation, frmDate, toDate);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void grdEvaluation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdEvaluation, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void grdEvaluation_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("grdEvaluation_SelectedIndexChanged()");
            Errorhandler.ClearError(lblMessage);

            try
            {
                int SelectedIndex = grdEvaluation.SelectedIndex;
                string trainingId = Server.HtmlDecode(grdEvaluation.Rows[SelectedIndex].Cells[0].Text.ToString());
                evalId = Server.HtmlDecode(grdEvaluation.Rows[SelectedIndex].Cells[2].Text.ToString());
                Session["EvaluationId"] = evalId;
                viewLink();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdEvaluation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("grdEvaluation_PageIndexChanging");

            try
            {
                grdEvaluation.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);
                getAvailableTrainingEvaluations(KeyEMPLOYEE_ID);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }


        public void getAvailableTrainingEvaluations(string empId)
        {
            TrainingDashboardDataHandler TDDH = new TrainingDashboardDataHandler();
            DataTable dt = new DataTable();

            try
            {
                dt = TDDH.getAvailableTrainingEvaluations(empId);
                grdEvaluation.DataSource = dt;
                grdEvaluation.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TDDH = null;
                dt.Dispose();
            }
        }

        public void evaluationSearch(string assignee,string empId,string trainingName,string prePost,string frmdate,string toDate)
        {
            TrainingDashboardDataHandler TDDH = new TrainingDashboardDataHandler();
            DataTable dt = new DataTable();

            try
            {
                dt = TDDH.searchAvailableTrainingEvaluations(assignee, empId, trainingName, prePost, frmdate, toDate);
                grdEvaluation.DataSource = dt;
                grdEvaluation.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TDDH = null;
                dt.Dispose();
            }
        }

        public void viewLink()
        {
            PasswordHandler crpto = new PasswordHandler();

            try
            {
                evalId = Convert.ToString(Session["EvaluationId"]);
                string jsMCQ = @"open('/TrainingAndDevelopment/WebFrmViewMCQquestions.aspx?EvaluationId=" + crpto.Encrypt(Convert.ToString(Session["EvaluationId"])) + @"', 'window', 'resizable=no,width=1000,height=650,scrollbars=yes,top=5,left=200,status=yes')";
                string hyperlinkMCQ = @"<a onclick=""" + jsMCQ + @"""' href='#'>" + "Add/View MCQ Question" + @"</a>";
                lblMCQ.Text = hyperlinkMCQ;

                string jsEssy = @"open('/TrainingAndDevelopment/WebFrmEssayQuestions.aspx?EvaluationId=" + crpto.Encrypt(Convert.ToString(Session["EvaluationId"])) + @"', 'window', 'resizable=no,width=1000,height=650,scrollbars=yes,top=5,left=200,status=yes')";
                string hyperlinkEssy = @"<a onclick=""" + jsEssy + @"""' href='#'>" + "Add/View Essay Question" + @"</a>";
                lblEssay.Text = hyperlinkEssy;

                string jsRating = @"open('/TrainingAndDevelopment/WebFrmViewRatingQuestions.aspx?EvaluationId=" + crpto.Encrypt(Convert.ToString(Session["EvaluationId"])) + @"', 'window', 'resizable=no,width=1000,height=650,scrollbars=yes,top=5,left=200,status=yes')";
                string hyperlinkRAting = @"<a onclick=""" + jsRating + @"""' href='#'>" + "Add/View Rating Question" + @"</a>";
                lblRating.Text = hyperlinkRAting;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                crpto = null;
            }
        }

        public void labelVisibility(bool status)
        {
            lblMultipleChoiceQuestion.Visible = status;
            lblMCQ.Visible = status;
            lblEssayQuestion.Visible = status;
            lblEssay.Visible = status;
            lblRatingQuestion.Visible = status;
            lblRating.Visible = status;
        }
    }
}