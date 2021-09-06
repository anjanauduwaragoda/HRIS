using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using Common;
using DataHandler.TrainingAndDevelopment;
using System.Data;
using System.Net;
using DataHandler.Userlogin;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFormEmployeeSelfTrainingEvaluation : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        #region events

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFormEmployeeSelfTrainingEvaluation : Page_Load");

            if(!IsPostBack)
            {
                if (Session["KeyEMPLOYEE_ID"] != null)
                {
                    string loggedUserId = Session["KeyEMPLOYEE_ID"].ToString();
                    populateEvaluationGridView(loggedUserId);
                }
            }
        }

        private void populateEvaluationGridView(string loggedUserId)
        {
            log.Debug("WebFormEmployeeSelfTrainingEvaluation : populateEvaluationGridView");
            
            DataTable dtEvaluations = new DataTable();
            EmployeeSelfTrainigEvaluationDashboardDataHandler employeeSelfTrainigEvaluationDataHandler = new EmployeeSelfTrainigEvaluationDashboardDataHandler();
            PasswordHandler crpto = new PasswordHandler();
            try
            {
                dtEvaluations = employeeSelfTrainigEvaluationDataHandler.getAllEvaluations(loggedUserId);
                dtEvaluations.Columns.Add("MCQ", typeof(string));
                dtEvaluations.Columns.Add("ESSAY", typeof(string));
                dtEvaluations.Columns.Add("RATING", typeof(string));

                foreach (DataRow evaluation in dtEvaluations.Rows)
                {
                    string evaluationId = evaluation["EVALUATION_ID"].ToString();
                    string trainingId = evaluation["TRAINING_ID"].ToString();
                    string isPostEvaluation = evaluation["POST_EVALUATION"].ToString();

                    if (!String.IsNullOrEmpty(evaluation["MCQ_INCLUDED"].ToString()))
                    {
                        if (evaluation["MCQ_INCLUDED"].ToString() == Constants.CON_ACTIVE_STATUS)
                        {
                            //int finalizedAnswerCount = employeeSelfTrainigEvaluationDataHandler.getFinalizedAnswerCount("TE_MCQ_ANSWERS", evaluation["EVALUATION_ID"].ToString(), evaluation["TRAINING_ID"].ToString(), loggedUserId);

                            //if (finalizedAnswerCount > 0)
                            //{
                            //    evaluation["MCQ"] = @"<a style='text-decoration: none;' href='#'> Complete </a>";
                            //}
                            //else if (finalizedAnswerCount == 0)
                            //{
                            //    evaluation["MCQ"] = @"<a style='text-decoration: none;' href='#'> Pending </a>";
                            //}
                        }
                        else 
                        {
                            evaluation["MCQ"] = "N/A";
                        }
                    }
                    if (!String.IsNullOrEmpty(evaluation["EQ_INCLUDED"].ToString()))
                    {
                        if (evaluation["EQ_INCLUDED"].ToString() == Constants.CON_ACTIVE_STATUS)
                        {
                            int finalizedAnswerCount = employeeSelfTrainigEvaluationDataHandler.getFinalizedAnswerCount("TE_EQ_ANSWERS", evaluation["EVALUATION_ID"].ToString(), evaluation["TRAINING_ID"].ToString(), loggedUserId, isPostEvaluation);
                            if (finalizedAnswerCount > 0)
                            {
                                string isFinalizedStatus = Constants.STATUS_ACTIVE_VALUE;
                                string onclick = @"open('/TrainingAndDevelopment/WebFormEmployeeSelfTrainingEvaluation_Essay.aspx?evaluationId=" + crpto.Encrypt(evaluationId) + "&employeeId=" + crpto.Encrypt(loggedUserId) + "&trainingId=" + crpto.Encrypt(trainingId) + "&isFinalizedStatus=" + crpto.Encrypt(isFinalizedStatus) + "&isPostEvaluation=" + crpto.Encrypt(isPostEvaluation) + " ', 'window', 'resizable=no,width=1000,height=650,scrollbars=yes,top=5,left=200,status=yes')";
                                evaluation["ESSAY"] = @"<a onclick=""" + onclick + @"""' href='#'>" + "Completed" + @"</a>";
                            }
                            else if (finalizedAnswerCount == 0)
                            {
                                string isFinalizedStatus = Constants.STATUS_INACTIVE_VALUE;
                                string onclick = @"open('/TrainingAndDevelopment/WebFormEmployeeSelfTrainingEvaluation_Essay.aspx?evaluationId=" + crpto.Encrypt(evaluationId) + "&employeeId=" + crpto.Encrypt(loggedUserId) + "&trainingId=" + crpto.Encrypt(trainingId) + "&isFinalizedStatus=" + crpto.Encrypt(isFinalizedStatus) + "&isPostEvaluation=" + crpto.Encrypt(isPostEvaluation) + " ', 'window', 'resizable=no,width=1000,height=650,scrollbars=yes,top=5,left=200,status=yes')";
                                evaluation["ESSAY"] = @"<a onclick=""" + onclick + @"""' href='#'>" + "Pending" + @"</a>";
                            }
                        }
                        else
                        {
                            evaluation["ESSAY"] = "N/A";
                        }
                    }
                    if (!String.IsNullOrEmpty(evaluation["RQ_INCLUDED"].ToString()))
                    {
                        if (evaluation["RQ_INCLUDED"].ToString() == Constants.CON_ACTIVE_STATUS)
                        {
                            //int finalizedAnswerCount = employeeSelfTrainigEvaluationDataHandler.getFinalizedAnswerCount("TE_RATING_QUESTION_ANSWERS", evaluation["EVALUATION_ID"].ToString(), evaluation["TRAINING_ID"].ToString(), loggedUserId);
                            //if (finalizedAnswerCount > 0)
                            //{
                            //    evaluation["RATING"] = "<a href='#'> Completed </a>";
                            //}
                            //else if (finalizedAnswerCount == 0)
                            //{
                            //    evaluation["RATING"] = "<a href='#'> Pending </a>";
                            //}
                        }
                        else
                        {
                            evaluation["RATING"] = "N/A";
                        }
                    }
                }
                gvEvaluations.DataSource = dtEvaluations;
                gvEvaluations.DataBind();
            }
            catch (Exception ex)
            {

                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                dtEvaluations.Dispose();
                employeeSelfTrainigEvaluationDataHandler = null;
            }
        }

        protected void gvEvaluations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvEvaluations, "Select$" + e.Row.RowIndex.ToString()));
                    //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    //e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    //e.Row.Attributes.Add("style", "cursor:pointer;");


                    //if (e.Row.Cells[7].Text.ToString() == "Completed")
                    //{
                    //    HyperLink essayLink = (HyperLink)(e.Row.FindControl("EssayLink"));
                    //    essayLink.Text = "Completed";
                    //    essayLink.NavigateUrl = "#";
                    //}
                    //else if (e.Row.Cells[7].Text.ToString() == "Pending")
                    //{
                    //    HyperLink essayLink = (HyperLink)(e.Row.FindControl("EssayLink"));
                    //    essayLink.Text = "Pending";
                    //    essayLink.NavigateUrl = "#";
                    //}
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        #endregion


    }
}