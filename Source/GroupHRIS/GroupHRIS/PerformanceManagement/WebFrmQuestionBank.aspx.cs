using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.PerformanceManagement;
using System.Data;
using DataHandler.Utility;
using NLog;


namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmQuestionBank : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                sIPAddress = Request.UserHostAddress;
                log.Debug("IP:" + sIPAddress + "webFrmEmployee : Page_Load");

                loadStatus();
                loadGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("btnSave_Click()");

                Utility.Errorhandler.ClearError(lblerror);


                string Question = txtQuestion.Text.Trim();
                string Remarks = txtRemarks.Text.Trim();
                string Status = ddlStatus.SelectedValue.ToString();
                string user = (Session["KeyEMPLOYEE_ID"] as string).Trim();

                QuestionBankDataHandler OQBDH = new QuestionBankDataHandler();
                UtilsDataHandler utilsDataHandler = new UtilsDataHandler();

                if (ddlStatus.SelectedValue.ToString().Trim() == Constants.CON_INACTIVE_STATUS)
                {
                    if (inactiveCheck() == false)
                    {
                        return;
                    }
                }


                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {

                    Boolean nameIsExsists = utilsDataHandler.isDuplicateExist(Question, "QUESTION", "QUESTIONNAIRE_BANK");
                    if (nameIsExsists)
                    {
                        CommonVariables.MESSAGE_TEXT = "Question already exists";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        return;
                    }
                    else
                    {
                        OQBDH.Insert(Question, Remarks, Status, user);
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    }                    
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string questionBankID = grdvQuestionBank.SelectedRow.Cells[0].Text;

                    Boolean nameIsExsists = utilsDataHandler.isDuplicateExist(Question, "QUESTION", "QUESTIONNAIRE_BANK", questionBankID,"QUESTION_ID");

                    if (nameIsExsists)
                    {
                        CommonVariables.MESSAGE_TEXT = "Question already exists";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        return;
                    }
                    else
                    {
                        string QuestionBankID = grdvQuestionBank.Rows[grdvQuestionBank.SelectedIndex].Cells[0].Text;
                        OQBDH.Update(Question, Remarks, Status, user, QuestionBankID);
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    }                
                    
                }

                clearFields();
                loadGrid();

                OQBDH = null;
                utilsDataHandler = null;
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }

            
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("btnClear_Click()");
                Utility.Errorhandler.ClearError(lblerror);
                clearFields();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void grdvQuestionBank_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                log.Debug("grdvQuestionBank_PageIndexChanging()");
                Utility.Errorhandler.ClearError(lblerror);

                QuestionBankDataHandler OQBDH = new QuestionBankDataHandler();
                DataTable dtQBanks = new DataTable();

                grdvQuestionBank.PageIndex = e.NewPageIndex;

                dtQBanks = OQBDH.Populate();

                grdvQuestionBank.DataSource = dtQBanks.Copy();
                grdvQuestionBank.DataBind();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void grdvQuestionBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("grdvQuestionBank_SelectedIndexChanged()");
                Utility.Errorhandler.ClearError(lblerror);
                lblAssessmentProfiles.Text = String.Empty;
                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

                txtQuestion.Text = HttpUtility.HtmlDecode(grdvQuestionBank.Rows[grdvQuestionBank.SelectedIndex].Cells[1].Text);
                txtRemarks.Text = HttpUtility.HtmlDecode(grdvQuestionBank.Rows[grdvQuestionBank.SelectedIndex].Cells[2].Text);
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(grdvQuestionBank.Rows[grdvQuestionBank.SelectedIndex].Cells[3].Text));
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void grdvQuestionBank_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdvQuestionBank, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Methods

        Boolean inactiveCheck()
        {
            Boolean Status = false;
            QuestionBankDataHandler OQBDH = new QuestionBankDataHandler();
            DataTable dtAssessmentProfiles = new DataTable();
            try
            {
                log.Debug("inactiveCheck()");
                lblAssessmentProfiles.Text = String.Empty;

                string QuestionID = HttpUtility.HtmlDecode(grdvQuestionBank.Rows[grdvQuestionBank.SelectedIndex].Cells[0].Text.Trim()).ToString().Trim();
                dtAssessmentProfiles = OQBDH.PopulateActiveSelfAssessmentProfiles(QuestionID.Trim()).Copy();
                if (dtAssessmentProfiles.Rows.Count > 0)
                {
                    CommonVariables.MESSAGE_TEXT = @"Cannot make inactive. Active self-assessment profile(s) exist.";

                    lblAssessmentProfiles.Text = "<br/>Active Self Assessment Profile(s),";
                    lblAssessmentProfiles.Text += "<ul>";
                    for (int i = 0; i < dtAssessmentProfiles.Rows.Count; i++)
                    {
                        lblAssessmentProfiles.Text += "<li>"+dtAssessmentProfiles.Rows[i]["PROFILE_NAME"].ToString() + "</li>";
                    }
                    lblAssessmentProfiles.Text += "</ul>";

                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);

                    Status = false;
                }
                else
                {
                    Status = true;
                    lblAssessmentProfiles.Text = String.Empty;
                }
            }
            catch (Exception exp)
            {
                Status = false;
                throw exp;
                //CommonVariables.MESSAGE_TEXT = exp.Message;
                //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                OQBDH = null;
                dtAssessmentProfiles.Dispose();
            }
            return Status;
        }

        void loadGrid()
        {
            try
            {
                log.Debug("loadGrid()");
                //Utility.Errorhandler.ClearError(lblerror);

                QuestionBankDataHandler OQBDH = new QuestionBankDataHandler();
                DataTable dtAGroups = new DataTable();

                dtAGroups = OQBDH.Populate();

                grdvQuestionBank.DataSource = dtAGroups.Copy();
                grdvQuestionBank.DataBind();


            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }

        }

        void loadStatus()
        {
            log.Debug("loadStatus()");
            Utility.Errorhandler.ClearError(lblerror);

            ddlStatus.Items.Add(new ListItem("", ""));
            ddlStatus.Items.Add(new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.STATUS_ACTIVE_VALUE));
            ddlStatus.Items.Add(new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.STATUS_INACTIVE_VALUE));
        }

        void clearFields()
        {
            try
            {
                log.Debug("clearFields()");
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                txtQuestion.Text = String.Empty;
                txtRemarks.Text = String.Empty;
                lblAssessmentProfiles.Text = String.Empty;
                ddlStatus.SelectedIndex = 0;
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        #endregion
    }
}