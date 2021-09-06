using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.PerformanceManagement;
using System.Data;
using GroupHRIS.Utility;
using Common;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmViewQuestions : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            ViewQuestionDataHandler oViewQuestionDataHandler = new ViewQuestionDataHandler();
            try
            {
                if (!IsPostBack)
                {
                    createQuestionBucketSelected();
                    //Check the sesstion
                    DataTable dt = (DataTable)Session["questionBucket"];

                    DataTable allQ = oViewQuestionDataHandler.getAllQuestins();

                    if (dt.Rows.Count > 0)
                    {

                        DataTable dtSelected = (DataTable)Session["selectedQuestionBucket"];

                        for (int i = 0; i < allQ.Rows.Count; i++)
                        {
                            string QuestionId = allQ.Rows[i][0].ToString().Trim();
                            string Question = allQ.Rows[i][1].ToString().Trim();
                            string remarks = allQ.Rows[i][2].ToString().Trim();

                            DataRow[] result = dt.Select("QUESTION_ID = '" + QuestionId + "'");

                            if (result.Length > 0)
                            {
                                string numOfAns = "";
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    if (QuestionId == dt.Rows[j][0].ToString().Trim())
                                    {
                                        numOfAns = dt.Rows[j][2].ToString().Trim();
                                    }
                                }
                                DataRow dtrow = dtSelected.NewRow();
                                dtrow["QUESTION_ID"] = QuestionId;
                                dtrow["QUESTION"] = Question;
                                dtrow["NO_OF_ANSWERS"] = numOfAns;
                                dtrow["EXCLUDE"] = true;
                                dtrow["REMARKS"] = remarks;

                                dtSelected.Rows.Add(dtrow);
                            }
                            else
                            {
                                DataRow dtrow = dtSelected.NewRow();
                                dtrow["QUESTION_ID"] = QuestionId;
                                dtrow["QUESTION"] = Question;
                                dtrow["NO_OF_ANSWERS"] = "";
                                dtrow["EXCLUDE"] = false;
                                dtrow["REMARKS"] = remarks;

                                dtSelected.Rows.Add(dtrow);
                            }

                        }
                        grdAssessmentQuestion.DataSource = dtSelected;
                        grdAssessmentQuestion.DataBind();
                    }
                    else
                    {
                        DataTable dtSelected = (DataTable)Session["selectedQuestionBucket"];

                        for (int i = 0; i < allQ.Rows.Count; i++)
                        {
                            string QuestionId = allQ.Rows[i][0].ToString().Trim();
                            string Question = allQ.Rows[i][1].ToString().Trim();
                            string remarks = allQ.Rows[i][2].ToString().Trim();

                            DataRow dtrow = dtSelected.NewRow();
                            dtrow["QUESTION_ID"] = QuestionId;
                            dtrow["QUESTION"] = Question;
                            dtrow["NO_OF_ANSWERS"] = "";
                            dtrow["EXCLUDE"] = false;
                            dtrow["REMARKS"] = remarks;

                            dtSelected.Rows.Add(dtrow);
                        }
                        grdAssessmentQuestion.DataSource = dtSelected;
                        grdAssessmentQuestion.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessagex);
            }
            finally
            {
                oViewQuestionDataHandler = null;
            }

            if (IsPostBack)
            {
                DataTable dt = (DataTable)Session["questionBucket"];
                string x = hfDataTable.Value;

            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ViewQuestionDataHandler oViewQuestionDataHandler = new ViewQuestionDataHandler();
            Utility.Errorhandler.ClearError(lblMessagex);
            try
            {
                DataTable dt = (DataTable)Session["questionBucket"];
                DataTable allQ = oViewQuestionDataHandler.getAllQuestins();

                dt.Rows.Clear();

                //int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
                //CheckBox cb = (CheckBox)grdAssessmentQuestion.Rows[selRowIndex].FindControl("chkBxSelect");

                //int iIndex = 0;
                //int iPageIndex = 0;

                //iPageIndex = grdAssessmentQuestion.PageIndex;

                //iIndex = (grdAssessmentQuestion.PageSize * iPageIndex) + selRowIndex;

                for (int i = 0; i < grdAssessmentQuestion.Rows.Count; i++)
                {
                    if ((grdAssessmentQuestion.Rows[i].FindControl("chkBxSelect") as CheckBox).Checked == true)
                    {
                        if ((grdAssessmentQuestion.Rows[i].FindControl("ddlCount") as DropDownList).SelectedIndex == 0)
                        {
                            Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Select Number of Answers", lblMessagex);
                            return;
                        }
                    }
                }

                for (int i = 0; i < grdAssessmentQuestion.Rows.Count; i++)
                {
                    CheckBox chk = (grdAssessmentQuestion.Rows[i].Cells[3].FindControl("chkBxSelect") as CheckBox);

                    if ((grdAssessmentQuestion.Rows[i].FindControl("chkBxSelect") as CheckBox).Checked == true)
                    {
                        string QuestionId = grdAssessmentQuestion.Rows[i].Cells[0].Text.ToString().Trim();
                        string Question = grdAssessmentQuestion.Rows[i].Cells[1].Text.ToString().Trim();
                        DropDownList ddl = (DropDownList)grdAssessmentQuestion.Rows[i].Cells[2].FindControl("ddlCount");
                        string numOfAns = ddl.SelectedValue;

                        // if (dt.Rows.Count > 0)
                        //{
                        //DataRow[] result = dt.Select("QUESTION_ID = '" + QuestionId + "'");

                        //if (result.Length > 0)
                        //{
                        //    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Record already exist", lblMessagex);
                        //    chk.Checked = false;
                        //    return;
                        //}
                        //else
                        //{
                        //        DataRow dtrow = dt.NewRow();
                        //        dtrow["QUESTION_ID"] = QuestionId;
                        //        dtrow["QUESTION"] = Question;
                        //        dtrow["NO_OF_ANSWERS"] = numOfAns;
                        //        dtrow["EXCLUDE"] = false;

                        //        dt.Rows.Add(dtrow);
                        //    //}
                        //}
                        //else
                        //{
                        DataRow dtrow = dt.NewRow();
                        dtrow["QUESTION_ID"] = QuestionId;
                        dtrow["QUESTION"] = Question;
                        dtrow["NO_OF_ANSWERS"] = numOfAns;
                        dtrow["EXCLUDE"] = false;

                        dt.Rows.Add(dtrow);
                        //}
                        chk.Checked = false;
                        ddl.SelectedIndex = 0;
                    }
                }
                Session["questionBucket"] = dt;
            }

            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessagex);
            }
            finally
            {
                oViewQuestionDataHandler = null;
            }

        }

        protected void grdAssessmentQuestion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessagex);
            }
        }

        protected void grdAssessmentQuestion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewQuestionDataHandler oViewQuestionDataHandler = new ViewQuestionDataHandler();
            try
            {
                DataTable dt1 = (DataTable)Session["selectedQuestionBucket"];
                grdAssessmentQuestion.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessagex);

                if (dt1.Rows.Count > 0)
                {
                    grdAssessmentQuestion.DataSource = dt1;
                    grdAssessmentQuestion.DataBind();
                }
                else
                {
                    grdAssessmentQuestion.DataSource = oViewQuestionDataHandler.getAllQuestins();
                    grdAssessmentQuestion.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessagex);
            }
            finally
            {
                oViewQuestionDataHandler = null;
            }
        }

        public void createQuestionBucketSelected()
        {
            DataTable questionBucket = new DataTable();
            questionBucket.Columns.Add("QUESTION_ID", typeof(string));
            questionBucket.Columns.Add("QUESTION", typeof(string));
            questionBucket.Columns.Add("NO_OF_ANSWERS", typeof(string));
            questionBucket.Columns.Add("EXCLUDE", typeof(string));
            questionBucket.Columns.Add("REMARKS", typeof(string));

            Session["selectedQuestionBucket"] = questionBucket;
        }


    }
}