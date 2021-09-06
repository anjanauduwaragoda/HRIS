using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.TrainingAndDevelopment;
using System.Data;
using NLog;
using Common;
using DataHandler.Userlogin;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmProgramEvaluation : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string evalId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmProgramEvaluation : Page_Load");

            if (!IsPostBack)
            {
                getRatingScheme();
                fillStatus(ddlStatus);
                ddlScheme.Enabled = false;
                linkVisible(false);
            }

            if (hfCaller.Value == "txtTraining")
            {
                clear();
                log.Debug("Select Training.");
                txtTraining.Text = hfVal.Value;
                lblTraining.Text = hfTrName.Value;
                hfCaller.Value = "";
                loadEvaluationDetails(txtTraining.Text);
            }

            //viewLink(false);

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            Utility.Errorhandler.ClearError(lblMessage);
            ProgramEvaluationDataHandler PEDH = new ProgramEvaluationDataHandler();

            try
            {
                string trainingId = txtTraining.Text;
                string evalName = txtEvalName.Text;
                string ismcq = chkValue(chkMcq.Checked);
                string isEssay = chkValue(chkEssay.Checked);
                string isRq = chkValue(chkRating.Checked);
                string status = ddlStatus.SelectedItem.Value;
                string rtScheme = "";

                if (isRq == "1")
                {
                    rtScheme = ddlScheme.SelectedValue;
                }

                string addedBy = Session["KeyUSER_ID"].ToString();

                int activeCount = PEDH.getActiveEvaluation(trainingId);

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    if (activeCount != 0)
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Active Evaluation is exist.", lblMessage);
                        return;
                    }

                    log.Debug("btnSave_Click() -> Insert");
                    bool isInsert = PEDH.Insert(rtScheme, trainingId, evalName, ismcq, isEssay, isRq, status, addedBy);
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved,Now You can add MCQ,Essay and Rating Questions to Evaluation.", lblMessage);
                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                    viewLink(true);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string x = Session["status"].ToString();
                    if (activeCount >= 1 && (status == "1" && Session["status"].ToString() == "Inactive"))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Active Evaluation is exist.", lblMessage);
                        return;
                    }
                    log.Debug("btnSave_Click() -> update");
                    string proEvalId = Session["EvaluationId"].ToString();
                    bool isUpdate = PEDH.Update(proEvalId, rtScheme, trainingId, evalName, ismcq, isEssay, isRq, status, addedBy); ;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
                    clear();
                }
                
                loadEvaluationDetails(txtTraining.Text);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
            }
        }

        protected void grdProEvaluation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdProEvaluation, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void grdProEvaluation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("grdProEvaluation_PageIndexChanging()");

            try
            {
                grdProEvaluation.PageIndex = e.NewPageIndex;
                loadEvaluationDetails(txtTraining.Text);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        protected void grdProEvaluation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProgramEvaluationDataHandler PEDH = new ProgramEvaluationDataHandler();
            try
            {
                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                Utility.Errorhandler.ClearError(lblMessage);
                int SelectedIndex = grdProEvaluation.SelectedIndex;

                Session["EvaluationId"] = Server.HtmlDecode(grdProEvaluation.Rows[SelectedIndex].Cells[0].Text.ToString());
                //string x = Convert.ToString(Session["EvaluationId"]);
                //txtTraining.Text = Server.HtmlDecode(grdProEvaluation.Rows[SelectedIndex].Cells[0].Text.ToString());
                txtEvalName.Text = Server.HtmlDecode(grdProEvaluation.Rows[SelectedIndex].Cells[2].Text.ToString());

                string isMcq = Server.HtmlDecode(grdProEvaluation.Rows[SelectedIndex].Cells[3].Text.ToString());
                if (isMcq == "Yes")
                {
                    lblMCQ.Visible = true;
                    chkMcq.Checked = true;
                }
                else
                {
                    lblMCQ.Visible = false;
                    chkMcq.Checked = false;
                }

                string isEssay = Server.HtmlDecode(grdProEvaluation.Rows[SelectedIndex].Cells[4].Text.ToString());
                if (isEssay == "Yes")
                {
                    lblEssyQuestion.Visible = true;
                    chkEssay.Checked = true;
                }
                else
                {
                    lblEssyQuestion.Visible = false;
                    chkEssay.Checked = false;
                }

                ddlScheme.Items.Clear();
                getRatingScheme();

                string isRating = Server.HtmlDecode(grdProEvaluation.Rows[SelectedIndex].Cells[5].Text.ToString());
                if (isRating == "Yes")
                {
                    lblRatingQuestion.Visible = true;
                    chkRating.Checked = true;
                    ddlScheme.Enabled = true;
                    string scheme = Server.HtmlDecode(grdProEvaluation.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
                    //ddlScheme.SelectedItem.Text = scheme;

                    ddlScheme.SelectedIndex = ddlScheme.Items.IndexOf(ddlScheme.Items.FindByText(scheme));
                    string x = ddlScheme.SelectedValue;

                    grdRatingDetails.DataSource = PEDH.getRatingSchemeDetails(ddlScheme.SelectedValue);
                    grdRatingDetails.DataBind();
                }
                else
                {
                    lblRatingQuestion.Visible = false;
                    chkRating.Checked = false;
                    ddlScheme.Enabled = false;

                    grdRatingDetails.DataSource = null;
                    grdRatingDetails.DataBind();
                }

                string status = Server.HtmlDecode(grdProEvaluation.Rows[SelectedIndex].Cells[6].Text.ToString().Trim());
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));
                Session["status"] = status;

                //if (grdProEvaluation.Rows.Count > 0)
                //{
                //    linkVisible(true);
                //}
                //else
                //{
                //    linkVisible(false);
                //}

                viewLink(true);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMessage);
            clear();
        }

        protected void chkMcq_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkMcq.Checked)
                {
                    lblMCQ.Visible = true;
                }
                else
                {
                    lblMCQ.Visible = false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void chkEssay_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkEssay.Checked)
                {
                    lblEssyQuestion.Visible = true;
                }
                else
                {
                    lblEssyQuestion.Visible = false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void chkRating_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkRating.Checked)
                {
                    ddlScheme.Enabled = true;
                    lblRatingQuestion.Visible = true;
                }
                else
                {
                    ddlScheme.Enabled = false;
                    lblRatingQuestion.Visible = false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ddlScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProgramEvaluationDataHandler PEDH = new ProgramEvaluationDataHandler();
            DataTable dt = new DataTable();

            try
            {
                dt = PEDH.getRatingSchemeDetails(ddlScheme.SelectedValue);
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


        public void loadEvaluationDetails(string trProgrmId)
        {
            ProgramEvaluationDataHandler PEDH = new ProgramEvaluationDataHandler();
            DataTable dt = new DataTable();

            try
            {
                dt = PEDH.getEvaluationDetails(trProgrmId);
                grdProEvaluation.DataSource = dt;
                grdProEvaluation.DataBind();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void getRatingScheme()
        {
            log.Debug("getRatingScheme()");
            ProgramEvaluationDataHandler PEDH = new ProgramEvaluationDataHandler();
            DataTable dt = new DataTable();

            try
            {
                dt = PEDH.getRatingScheme();
                ddlScheme.Items.Add(new ListItem("", ""));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string text = dt.Rows[i]["RS_NAME"].ToString();
                    string value = dt.Rows[i]["RS_ID"].ToString();
                    ddlScheme.Items.Add(new ListItem(text, value));
                }
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

        public string chkValue(bool status)
        {
            string val = "0";

            try
            {
                if (status)
                {
                    val = "1";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return val;
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

        public void clear()
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            txtEvalName.Text = "";
            chkMcq.Checked = false;
            chkEssay.Checked = false;
            chkRating.Checked = false;
            ddlScheme.Items.Clear();
            getRatingScheme();
            ddlScheme.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            ddlScheme.Enabled = false;
            Session["EvaluationId"] = null;
            lblMCQ.Text = "";
            lblEssyQuestion.Text = "";
            lblRatingQuestion.Text = "";

            grdRatingDetails.DataSource = null;
            grdRatingDetails.DataBind();
        }

        public void viewLink(bool status)
        {
            PasswordHandler crpto = new PasswordHandler();

            try
            {
                evalId = Convert.ToString(Session["EvaluationId"]);
                string evaluationName = txtEvalName.Text;

                if (evalId != "" && chkMcq.Checked == true)
                {
                    string jsMCQ = @"open('/TrainingAndDevelopment/WebFrmMCQPopup.aspx?EvaluationId=" + crpto.Encrypt(Convert.ToString(Session["EvaluationId"])) + @"&evaluationName=" + evaluationName + @"', 'window', 'resizable=no,width=1000,height=650,scrollbars=yes,top=5,left=200,status=yes')";
                    string hyperlinkMCQ = @"<a onclick=""" + jsMCQ + @"""' href='#'>" + "Add/View MCQ Question" + @"</a>";
                    lblMCQ.Visible = true;
                    lblMCQ.Text = hyperlinkMCQ;
                }
                else
                {
                    lblMCQ.Visible = false;
                }

                if (evalId != "" && chkEssay.Checked == true)
                {
                    string jsEssy = @"open('/TrainingAndDevelopment/WebFrmEssayQuestionPopup.aspx?EvaluationId=" + crpto.Encrypt(Convert.ToString(Session["EvaluationId"])) + @"&evaluationName=" + evaluationName + @"', 'window', 'resizable=no,width=1000,height=650,scrollbars=yes,top=5,left=200,status=yes')";
                    string hyperlinkEssy = @"<a onclick=""" + jsEssy + @"""' href='#'>" + "Add/View Essay Question" + @"</a>";
                    lblEssyQuestion.Visible = true;
                    lblEssyQuestion.Text = hyperlinkEssy;
                }
                else
                {
                    lblEssyQuestion.Visible = false;
                }

                if (evalId != "" && chkRating.Checked == true)
                {
                    string ratingScheme = "";
                    string schemeName = "";

                    if (chkRating.Checked == true)
                    {
                        if (ddlScheme.SelectedValue != null)
                        {
                            ratingScheme = ddlScheme.SelectedValue;
                            schemeName = ddlScheme.SelectedItem.Text;
                        }
                        else
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Please Select the Rating Scheme", lblMessage);
                         return;
                        }
                    }

                    
                    string jsRating = @"open('/TrainingAndDevelopment/WebFrmRatingQuestionPopup.aspx?EvaluationId=" + crpto.Encrypt(Convert.ToString(Session["EvaluationId"])) + @"&evaluationName=" + evaluationName + @"&ratingScheme=" + ratingScheme + @"&SchemeName=" + schemeName + @"', 'window', 'resizable=no,width=1000,height=650,scrollbars=yes,top=5,left=200,status=yes')";
                    string hyperlinkRAting = @"<a onclick=""" + jsRating + @"""' href='#'>" + "Add/View Rating Question" + @"</a>";
                    lblRatingQuestion.Visible = true;
                    lblRatingQuestion.Text = hyperlinkRAting;
                }
                else
                {
                    lblRatingQuestion.Visible = false;
                }
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

        public void linkVisible(bool status)
        {
            lblMCQ.Visible = status;
            lblEssyQuestion.Visible = status;
            lblRatingQuestion.Visible = status;
        }




    }
}