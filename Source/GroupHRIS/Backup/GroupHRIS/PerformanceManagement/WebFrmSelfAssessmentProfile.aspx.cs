using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using GroupHRIS.Utility;
using DataHandler.PerformanceManagement;
using Common;
using DataHandler.Utility;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmSelfAssessmentProfile : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            SelfAssessmentProfileDataHandler oSelfAssessmentProfileDataHandler = new SelfAssessmentProfileDataHandler();
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmSelfAssessmentProfile : Page_Load");

            if (!IsPostBack)
            {
                createQuestionBucket();
                fillGoals();
                fillStatus();
                grdAssessmentProfile.DataSource = oSelfAssessmentProfileDataHandler.getSelfAssessmentProfile();
                grdAssessmentProfile.DataBind();

                gvviewQuestions.DataSource = null;
                gvviewQuestions.DataBind();
            }

            if (IsPostBack)
            {
                string isTrue = hfisInclude.Value.ToString();
                DataTable dtQuestion = (DataTable)Session["questionBucket"];
                int x = 0;

                for (int i = 0; i < gvviewQuestions.Rows.Count; i++)
                {
                    CheckBox chk = (gvviewQuestions.Rows[i].Cells[3].FindControl("chkBxExclude") as CheckBox);
                    if ((gvviewQuestions.Rows[i].Cells[3].FindControl("chkBxExclude") as CheckBox).Checked == false)
                    {
                        x++;
                    }
                }
                if (gvviewQuestions.Rows.Count == x)
                {
                    isTrue = "popup";
                }
                if (dtQuestion.Rows.Count > 0)
                {
                    if (isTrue == "popup")
                    {
                        Errorhandler.ClearError(lblMessage);
                        gvviewQuestions.DataSource = Session["questionBucket"];
                        gvviewQuestions.DataBind();
                    }
                    else
                    {
                        dtQuestion.Clear();
                        for (int i = 0; i < gvviewQuestions.Rows.Count; i++)
                        {
                            CheckBox chk = (gvviewQuestions.Rows[i].Cells[3].FindControl("chkBxExclude") as CheckBox);

                            DataRow dtrow = dtQuestion.NewRow();

                            string QuestionId = gvviewQuestions.Rows[i].Cells[0].Text.ToString().Trim();
                            string Question = gvviewQuestions.Rows[i].Cells[1].Text.ToString().Trim();
                            string numOfAns = gvviewQuestions.Rows[i].Cells[2].Text.ToString().Trim();

                            if ((gvviewQuestions.Rows[i].Cells[3].FindControl("chkBxExclude") as CheckBox).Checked == true)
                            {
                                dtrow["QUESTION_ID"] = QuestionId;
                                dtrow["QUESTION"] = Question;
                                dtrow["NO_OF_ANSWERS"] = numOfAns;
                                dtrow["EXCLUDE"] = true;
                            }
                            else
                            {

                                dtrow["QUESTION_ID"] = QuestionId;
                                dtrow["QUESTION"] = Question;
                                dtrow["NO_OF_ANSWERS"] = numOfAns;
                                dtrow["EXCLUDE"] = false;
                            }

                            dtQuestion.Rows.Add(dtrow);

                        }
                    }
                    // hfisInclude.Value = "";
                }
                Session["questionBucket"] = dtQuestion;
                gvviewQuestions.DataSource = Session["questionBucket"];
                gvviewQuestions.DataBind();

            }

            viewQuestions();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string grpName = ddlAssessmentGroup.SelectedValue;
            string profileName = txtProName.Text;
            string description = txtDescription.Text;
            string status = ddlStatus.SelectedValue.ToString();
            string logUser = Session["KeyUSER_ID"].ToString();
            DataTable questionList = (DataTable)Session["questionBucket"];

            SelfAssessmentProfileDataHandler oSelfAssessmentProfileDataHandler = new SelfAssessmentProfileDataHandler();
            UtilsDataHandler oUtilsDataHandler = new UtilsDataHandler();

            Errorhandler.ClearError(lblMessage);

            if (questionList.Rows.Count < 1)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Select questions", lblMessage);
                return;
            }

            int excludeCount = 0;
            foreach (DataRow row in questionList.Rows)
            {
                if (row["EXCLUDE"].ToString() == "True")
                {
                    excludeCount++;
                }
            }

            if (questionList.Rows.Count == excludeCount)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Profile need at least one question", lblMessage);
                return;
            }

            
            try
            {

                Errorhandler.ClearError(lblMessage);
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    Boolean isExist = oUtilsDataHandler.isDuplicateExist(profileName, "PROFILE_NAME", "SELF_ASSESSMENT_PROFILE");

                    if (isExist)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Profile name already exists.", lblMessage);
                        return;
                    }
                    else
                    {
                        Boolean isSuccess = oSelfAssessmentProfileDataHandler.Insert(grpName, profileName, description, status, logUser, questionList);
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved.", lblMessage);
                    }
                }
                else
                    if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {

                        string pro_id = Session["proId"].ToString();
                        Boolean isExist = oUtilsDataHandler.isDuplicateExist(profileName, "PROFILE_NAME", "SELF_ASSESSMENT_PROFILE", pro_id, "SELF_ASSESSMENT_PROFILE_ID");
                        
                        if (isExist)
                        {
                            Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Profile name already exists.", lblMessage);
                            return;
                        }
                        if (status == "0")
                        {
                            bool isInactive = isCanInactive(pro_id);

                            if (!isInactive)
                            {
                                Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Can not inactive Self Assessment Profile.", lblMessage);
                                return;
                            }

                        }
                        Boolean isSuccess = oSelfAssessmentProfileDataHandler.Update(pro_id, grpName, profileName, description, status, logUser, questionList);
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated.", lblMessage);
                    }
                grdAssessmentProfile.DataSource = oSelfAssessmentProfileDataHandler.getSelfAssessmentProfile();
                grdAssessmentProfile.DataBind();

                foreach (DataRow row in questionList.Rows)
                {
                    if (row["EXCLUDE"].ToString() == "True")
                    {
                        SelfAssessmentProfileDataHandler oSelfAssessmentProfileDataHandlerx = new SelfAssessmentProfileDataHandler();
                        DataTable qtb = oSelfAssessmentProfileDataHandlerx.getAssessmentProfileQuestion(Session["proId"].ToString());
                        gvviewQuestions.DataSource = qtb;
                        gvviewQuestions.DataBind();

                        Session["questionBucket"] = qtb;
                    }

                }

                hfisInclude.Value = "";
                clear();
                //ddlgoalArea.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oSelfAssessmentProfileDataHandler = null;
            }
        }

        protected void grdAssessmentProfile_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdAssessmentProfile, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void grdAssessmentProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelfAssessmentProfileDataHandler oSelfAssessmentProfileDataHandler = new SelfAssessmentProfileDataHandler();

            try
            {
                Errorhandler.ClearError(lblMessage);
                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                
                int SelectedIndex = grdAssessmentProfile.SelectedIndex;
                string proId = Server.HtmlDecode(grdAssessmentProfile.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());
                Session["proId"] = proId.ToString();
                string proName = Server.HtmlDecode(grdAssessmentProfile.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
                txtProName.Text = proName.ToString();

                string assGroup = Server.HtmlDecode(grdAssessmentProfile.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());
                ddlAssessmentGroup.SelectedIndex = ddlAssessmentGroup.Items.IndexOf(ddlAssessmentGroup.Items.FindByText(assGroup));


                string description = Server.HtmlDecode(grdAssessmentProfile.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
                txtDescription.Text = description.ToString();

                string status = Server.HtmlDecode(grdAssessmentProfile.Rows[SelectedIndex].Cells[4].Text.ToString().Trim());
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));

                DataTable dtTable = oSelfAssessmentProfileDataHandler.getAssessmentProfileQuestion(proId);
                if (dtTable.Rows.Count > 0)
                {
                    Session["questionBucket"] = dtTable;
                    gvviewQuestions.DataSource = dtTable;
                    gvviewQuestions.DataBind();
                }
                else
                {
                    gvviewQuestions.DataSource = null;
                    gvviewQuestions.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oSelfAssessmentProfileDataHandler = null;
            }
        }

        protected void btnclear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
                Errorhandler.ClearError(lblMessage);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void gvviewQuestions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvviewQuestions, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                    string text = DataBinder.Eval(e.Row.DataItem, "QUESTION", string.Empty);
                    e.Row.Attributes.Add("id", e.Row.Cells[1].Text);
                    e.Row.Attributes.Add("onclick", "show('"+"Question : " + text + "')");

                    //e.Row.Attributes["onclick"] = "show(" + e.Row.RowIndex.ToString() + ");";
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void gvviewQuestions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvviewQuestions.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);
                gvviewQuestions.DataSource = Session["questionBucket"];
                gvviewQuestions.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdAssessmentProfile_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SelfAssessmentProfileDataHandler oSelfAssessmentProfileDataHandler = new SelfAssessmentProfileDataHandler();

            try
            {
                grdAssessmentProfile.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);
                grdAssessmentProfile.DataSource = oSelfAssessmentProfileDataHandler.getSelfAssessmentProfile();
                grdAssessmentProfile.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oSelfAssessmentProfileDataHandler = null;
            }
        }

        protected void ddlAssessmentGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelfAssessmentProfileDataHandler oSelfAssessmentProfileDataHandler = new SelfAssessmentProfileDataHandler();

            Errorhandler.ClearError(lblMessage);
            try
            {
                string assessmentGroupId = ddlAssessmentGroup.SelectedItem.Value;
                string isExist = oSelfAssessmentProfileDataHandler.getAssGroupId(assessmentGroupId);

                if (isExist != "")
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Assessment group already assign for Active Assessment Profile.", lblMessage);
                    enable(false);
                    return;
                }
                else
                {
                    enable(true);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void gvviewQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int SelectedIndex = gvviewQuestions.SelectedIndex;
                string question = Server.HtmlDecode(gvviewQuestions.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
                //popUpQuestion.GroupingText = question;

            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Methods

        public void fillGoals()
        {
            DataTable table = new DataTable();
            SelfAssessmentProfileDataHandler oSelfAssessmentProfileDataHandler = new SelfAssessmentProfileDataHandler();
            try
            {
                table = oSelfAssessmentProfileDataHandler.getAssessmentGroup().Copy();

                ddlAssessmentGroup.Items.Add(new ListItem("", ""));

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string text = table.Rows[i]["GROUP_NAME"].ToString();
                    string value = table.Rows[i]["GROUP_ID"].ToString();
                    ddlAssessmentGroup.Items.Add(new ListItem(text, value));
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                oSelfAssessmentProfileDataHandler = null;
                table.Dispose();
            }
           
        }

        public void fillStatus()
        {
            try
            {
                ddlStatus.Items.Insert(0, new ListItem("", ""));
                ddlStatus.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                ddlStatus.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void viewQuestions()
        {
            try
            {
                string isTrue = hfisInclude.Value.ToString();
                string TableString = "";
                TableString += @"<table style='width:100%;height:100%'> ";
                TableString += "<tr><td><a href=\"#" + "\"OnClick=\"openLOVWindow('../PerformanceManagement/WebFrmViewQuestions.aspx?','Search','hfisInclude')" + "\"?style = \"text-decoration:none\"" + ">" + "Add Questions" + "</a>";
                TableString += @"</table>";

                litDefaultQuestionList.Text = string.Empty;
                litDefaultQuestionList.Text = HttpUtility.HtmlDecode(TableString);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void createQuestionBucket()
        {
            DataTable questionBucket = new DataTable();
            questionBucket.Columns.Add("QUESTION_ID", typeof(string));
            questionBucket.Columns.Add("QUESTION", typeof(string));
            questionBucket.Columns.Add("NO_OF_ANSWERS", typeof(string));
            questionBucket.Columns.Add("EXCLUDE", typeof(string));

            Session["questionBucket"] = questionBucket;
        }

        public void clear()
        {
            ddlAssessmentGroup.SelectedIndex = 0;
            txtProName.Text = "";
            txtDescription.Text = "";
            ddlStatus.SelectedIndex = 0;
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;

            gvviewQuestions.DataSource = null;
            gvviewQuestions.DataBind();

            Session.Remove("questionBucket");
            createQuestionBucket();
            enable(true); 
        }

        public void enable(bool status)
        {
            txtProName.Enabled = status;
            txtDescription.Enabled = status;
            ddlStatus.Enabled = status;
            btnSave.Enabled = status;
        }

        public bool isCanInactive(string assProId)
        {   
            DataTable table = new DataTable();
            SelfAssessmentProfileDataHandler oSelfAssessmentProfileDataHandler = new SelfAssessmentProfileDataHandler();
            int count = 0;
            bool status = false;

            try
            {
                table = oSelfAssessmentProfileDataHandler.getAssessmentList(assProId);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string st = table.Rows[i]["STATUS_CODE"].ToString();

                    if (st == Constants.ASSESSNEMT_OBSOLETE_TAG || st == Constants.ASSESSNEMT_CLOSED_STATUS)
                    {
                        count = count + 1;
                    }
                }

                if (count == table.Rows.Count)
                {
                    status = true;
                }
                return status;
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                oSelfAssessmentProfileDataHandler = null;
                table.Dispose();
            }
        }

        #endregion




        public void toolTip()
        {
            
        }



    }
}