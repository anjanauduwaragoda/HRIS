using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.TrainingAndDevelopment;
using Common;
using DataHandler.Userlogin;
using NLog;
using GroupHRIS.Utility;
using System.Globalization;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmPrePostEvaluation : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string evalId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmPrePostEvaluation : Page_Load");

            if (!IsPostBack)
            {
                fillStatus();
            }

            if (hfCaller.Value == "txtTraining")
            {
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                Errorhandler.ClearError(lblMessage);
                clear();

                txtTraining.Text = hfVal.Value;
                lblTraining.Text = hfTrName.Value;
                hfCaller.Value = "";
                loadEvaluationDetails(getprogrmId(txtTraining.Text));
                loadParticipantDetails(txtTraining.Text);

                viewExistTrainingData();
            }
        }

        protected void grdProEvaluation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        //protected void grdProEvaluation_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdProEvaluation, "Select$" + e.Row.RowIndex.ToString()));
        //            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
        //            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
        //            e.Row.Attributes.Add("style", "cursor:pointer;");

        //            //Label lblMCQ = (e.Row.FindControl("lblMcq") as Label);

        //            //lblMCQ.Text = Convert.ToString(lblMCQ);
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        CommonVariables.MESSAGE_TEXT = Ex.Message;
        //        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
        //    }
        //}

        //protected void grdProEvaluation_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int SelectedIndex = grdProEvaluation.SelectedIndex;

        //        Session["EvaluationId"] = Server.HtmlDecode(grdProEvaluation.Rows[SelectedIndex].Cells[0].Text.ToString());

        //        string isMcq = Server.HtmlDecode(grdProEvaluation.Rows[SelectedIndex].Cells[3].Text.ToString());
        //        if (isMcq == "Yes")
        //        {
        //            lblMCQ.Visible = true;
        //        }
        //        else
        //        {
        //            lblMCQ.Visible = false;
        //        }

        //        string isEssay = Server.HtmlDecode(grdProEvaluation.Rows[SelectedIndex].Cells[4].Text.ToString());
        //        if (isEssay == "Yes")
        //        {
        //            lblEssyQuestion.Visible = true;
        //        }
        //        else
        //        {
        //            lblEssyQuestion.Visible = false;
        //        }


        //        string isRating = Server.HtmlDecode(grdProEvaluation.Rows[SelectedIndex].Cells[5].Text.ToString());
        //        if (isRating == "Yes")
        //        {
        //            lblRatingQuestion.Visible = true;

        //        }
        //        else
        //        {
        //            lblRatingQuestion.Visible = false;
        //        }

        //        viewLink();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //protected void chkPostEval_CheckedChanged(object sender, EventArgs e)
        //{
        //    int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;

        //    CheckBox cb = (CheckBox)grdProEvaluation.Rows[selRowIndex].FindControl("chkPostEval");

        //    string evalId = Server.HtmlDecode(grdProEvaluation.Rows[selRowIndex].Cells[0].Text.ToString().Trim());
        //    if (cb.Checked == false)
        //    {
        //        evalId = "";
        //    }

        //    for (int i = 0; i < grdProEvaluation.Rows.Count; i++)
        //    {
        //        if (i != selRowIndex)
        //        {
        //            CheckBox chk = (grdProEvaluation.Rows[i].Cells[8].FindControl("chkPostEval") as CheckBox);
        //            chk.Checked = false;
        //        }
        //    }

        //    hfPostEvaluation.Value = evalId;
        //}

        //protected void chkPreEval_CheckedChanged(object sender, EventArgs e)
        //{
        //    int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;

        //    CheckBox cb = (CheckBox)grdProEvaluation.Rows[selRowIndex].FindControl("chkPreEval");

        //    string evalId = Server.HtmlDecode(grdProEvaluation.Rows[selRowIndex].Cells[0].Text.ToString().Trim());
        //    if (cb.Checked == false)
        //    {
        //        evalId = "";
        //    }

        //    for (int i = 0; i < grdProEvaluation.Rows.Count; i++)
        //    {
        //        if (i != selRowIndex)
        //        {
        //            CheckBox chk = (grdProEvaluation.Rows[i].Cells[7].FindControl("chkPreEval") as CheckBox);
        //            chk.Checked = false;
        //        }
        //    }

        //    hfPreEvaluation.Value = evalId;
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            PrePostEvaluationDataHandler PEDH = new PrePostEvaluationDataHandler();
            try
            {
                log.Debug("btnSave_Click()");
                string logUser = Session["KeyUSER_ID"].ToString();
                string trId = txtTraining.Text;
                string stDate = txtStartDate.Text;
                string eDate = txtEndDate.Text;
                string comment = txtComment.Text;
                string status = ddlStatus.SelectedItem.Value;
                string evaluation = "";
                string eval = hfProgramId.Value;

                if ((chkPreEvaluation.Checked == true) && chkPostEvaluation.Checked == true)
                {
                    evaluation = "0";
                }
                else if (chkPreEvaluation.Checked == true)
                {
                    evaluation = "1";
                }
                //DataTable evalList = new DataTable();
                //evalList = readEvaluationList(eval);
                //if (evalList.Rows.Count == 0)
                //{
                //    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Need to select evaluation", lblMessage);
                //    return;
                //}

                DataTable dtEmployee = readEmployeeList();
                if (dtEmployee.Rows.Count == 0)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Participants are required for evaluation", lblMessage);
                    return;
                }

                bool dateValidate = checkDatevalidation(stDate, eDate);

                if (!dateValidate)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Start date should be less than End date", lblMessage);
                    return;
                }


                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Insert");
                    bool isInsert = PEDH.Insert(trId, eval, evaluation, dtEmployee, stDate, eDate, status, comment, logUser);
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblMessage);
                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> update");
                    //When update delete and insert the data

                    bool isUpdate = PEDH.Update(trId, eval, evaluation, dtEmployee, stDate, eDate, status, comment, logUser); ;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);

                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PEDH = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                Errorhandler.ClearError(lblMessage);
                clear();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdParticipantDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdParticipantDetails, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void grdParticipantDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdParticipantDetails.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);

                loadParticipantDetails(txtTraining.Text);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void chkPostEvaluation_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkPreEvaluation.Checked == false)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Need to select Pre-Evaluation first", lblMessage);
                    return;
                }
                else
                {
                    Errorhandler.ClearError(lblMessage);
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void chkPreEvaluation_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkPreEvaluation.Checked == true)
                {
                    Errorhandler.ClearError(lblMessage);
                }
                else
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Need to select Pre-Evaluation first", lblMessage);
                    return;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void chkSupervisorEval_CheckedChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMessage);

            CheckBox rdSelectAll = ((CheckBox)grdParticipantDetails.HeaderRow.FindControl("chkHeaderSupervisor"));


            if (rdSelectAll.Checked == true)
            {
                ((CheckBox)grdParticipantDetails.HeaderRow.FindControl("chkHeaderEmp")).Checked = false;

                for (int i = 0; i < grdParticipantDetails.Rows.Count; i++)
                {

                    ((CheckBox)grdParticipantDetails.Rows[i].Cells[10].FindControl("chkSupervisor")).Checked = true;
                    ((CheckBox)grdParticipantDetails.Rows[i].Cells[10].FindControl("chkEmployee")).Checked = false;
                }
            }
            else
            {
                for (int i = 0; i < grdParticipantDetails.Rows.Count; i++)
                {

                    ((CheckBox)grdParticipantDetails.Rows[i].Cells[11].FindControl("chkSupervisor")).Checked = false;
                }
            }
        }

        protected void chkHeaderEmp_CheckedChanged1(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMessage);

            CheckBox rdSelectAll = ((CheckBox)grdParticipantDetails.HeaderRow.FindControl("chkHeaderEmp"));


            if (rdSelectAll.Checked == true)
            {
                ((CheckBox)grdParticipantDetails.HeaderRow.FindControl("chkHeaderSupervisor")).Checked = false;

                for (int i = 0; i < grdParticipantDetails.Rows.Count; i++)
                {

                    ((CheckBox)grdParticipantDetails.Rows[i].Cells[11].FindControl("chkEmployee")).Checked = true;
                    ((CheckBox)grdParticipantDetails.Rows[i].Cells[11].FindControl("chkSupervisor")).Checked = false;
                }
            }
            else
            {
                for (int i = 0; i < grdParticipantDetails.Rows.Count; i++)
                {

                    ((CheckBox)grdParticipantDetails.Rows[i].Cells[11].FindControl("chkEmployee")).Checked = false;

                }
            }
        }


        protected void chkSupervisor_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string x = e.ToString();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void chkEmployee_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {
                
                throw;
            }
        }


        //public void viewLink()
        //{
        //    PasswordHandler crpto = new PasswordHandler();
        //    string remarks = "hideTable";
        //    try
        //    {
        //        evalId = hfProgramId.Value; ;
        //        string jsMCQ = @"open('/TrainingAndDevelopment/WebFrmMCQPopup.aspx?EvaluationId=" + crpto.Encrypt(Convert.ToString(Session["EvaluationId"])) + @"&remarks=" + remarks + @"', 'window', 'resizable=no,width=1000,height=650,scrollbars=yes,top=5,left=200,status=yes')";
        //        string hyperlinkMCQ = @"<a onclick=""" + jsMCQ + @"""' href='#'>" + "Add/View MCQ Question" + @"</a>";
        //        lblMCQ.Text = hyperlinkMCQ;

        //        string jsEssy = @"open('/TrainingAndDevelopment/WebFrmEssayQuestionPopup.aspx?EvaluationId=" + crpto.Encrypt(Convert.ToString(Session["EvaluationId"])) + @"', 'window', 'resizable=no,width=1000,height=650,scrollbars=yes,top=5,left=200,status=yes')";
        //        string hyperlinkEssy = @"<a onclick=""" + jsEssy + @"""' href='#'>" + "Add/View Essay Question" + @"</a>";
        //        lblEssyQuestion.Text = hyperlinkEssy;

        //        string jsRating = @"open('/TrainingAndDevelopment/WebFrmRatingQuestionPopup.aspx?EvaluationId=" + crpto.Encrypt(Convert.ToString(Session["EvaluationId"])) + @"', 'window', 'resizable=no,width=1000,height=650,scrollbars=yes,top=5,left=200,status=yes')";
        //        string hyperlinkRAting = @"<a onclick=""" + jsRating + @"""' href='#'>" + "Add/View Rating Question" + @"</a>";
        //        lblRatingQuestion.Text = hyperlinkRAting;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        crpto = null;
        //    }
        //}

        public void loadEvaluationDetails(string trProgrmId)
        {
            PrePostEvaluationDataHandler PPEDH = new PrePostEvaluationDataHandler();
            DataTable dt = new DataTable();

            try
            {
                dt = PPEDH.getEvaluationDetails(trProgrmId);

                foreach (DataRow dr in dt.Rows)
                {
                    hfProgramId.Value = dr["EVALUATION_ID"].ToString();
                    lblEvaluationName.Text = dr["EVALUATION_NAME"].ToString();
                    lblMCQInclude.Text = dr["MCQ_INCLUDED"].ToString();
                    lblEssayQuestion.Text = dr["EQ_INCLUDED"].ToString();
                    lblRating.Text = dr["RQ_INCLUDED"].ToString();
                    lblRatingScheme.Text = dr["RS_NAME"].ToString();
                }

                //if (lblMCQInclude.Text == "Yes")
                //{
                //    lblMCQ.Visible = true;
                //}
                //else
                //{
                //    lblMCQ.Visible = false;
                //}

                //if (lblEssayQuestion.Text == "Yes")
                //{
                //    lblEssyQuestion.Visible = true;
                //}
                //else
                //{
                //    lblEssyQuestion.Visible = false;
                //}


                //if (lblRating.Text == "Yes")
                //{
                //    lblRatingQuestion.Visible = true;

                //}
                //else
                //{
                //    lblRatingQuestion.Visible = false;
                //}

                //viewLink();
                //grdProEvaluation.DataSource = dt;
                //grdProEvaluation.DataBind();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void loadParticipantDetails(string trProgrmId)
        {
            PrePostEvaluationDataHandler PPEDH = new PrePostEvaluationDataHandler();
            DataTable dt = new DataTable();

            try
            {
                dt = PPEDH.getParticipantDetails(trProgrmId);
                grdParticipantDetails.DataSource = dt;
                grdParticipantDetails.DataBind();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public String getprogrmId(string trId)
        {
            PrePostEvaluationDataHandler PPEDH = new PrePostEvaluationDataHandler();
            string proId = "";

            try
            {
                proId = PPEDH.getTrainingProgrm(trId);
            }
            catch (Exception)
            {
                throw;
            }
            return proId;
        }

        public void clear()
        {
            txtTraining.Text = "";
            lblTraining.Text = "";

            ddlStatus.SelectedIndex = 0;

            grdParticipantDetails.DataSource = null;
            grdParticipantDetails.DataBind();

            txtStartDate.Text = "";
            txtEndDate.Text = "";
            txtComment.Text = "";

            chkPreEvaluation.Checked = false;
            chkPostEvaluation.Checked = false;
        }

        public DataTable readEmployeeList()
        {
            try
            {
                createEmployeeDetailsBucket();

                DataTable dt = (DataTable)Session["employeeBucket"];

                for (int i = 0; i < grdParticipantDetails.Rows.Count; i++)
                {
                    string employeeId = grdParticipantDetails.Rows[i].Cells[2].Text.ToString().Trim();
                    string empCompany = grdParticipantDetails.Rows[i].Cells[0].Text.ToString().Trim();

                    DataRow dtrow = dt.NewRow();
                    dtrow["EMPLOYEE_ID"] = employeeId;
                    dtrow["COMPANY_ID"] = empCompany;

                    if (((CheckBox)grdParticipantDetails.Rows[i].Cells[11].FindControl("chkEmployee")).Checked == true)
                    {
                        dtrow["EMPLOYEE_EVALUATION"] = "1";
                    }
                    else
                    {
                        dtrow["EMPLOYEE_EVALUATION"] = "0";
                    }

                    if (((CheckBox)grdParticipantDetails.Rows[i].Cells[11].FindControl("chkSupervisor")).Checked == true)
                    {
                        dtrow["SUPERVISOR_EVALUATION"] = "1";
                    }
                    else
                    {
                        dtrow["SUPERVISOR_EVALUATION"] = "0";
                    }

                    dt.Rows.Add(dtrow);
                }

                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void createEmployeeDetailsBucket()
        {
            DataTable employeeBucket = new DataTable();
            employeeBucket.Columns.Add("EMPLOYEE_ID", typeof(string));
            employeeBucket.Columns.Add("COMPANY_ID", typeof(string));
            employeeBucket.Columns.Add("SUPERVISOR_EVALUATION", typeof(string));
            employeeBucket.Columns.Add("EMPLOYEE_EVALUATION", typeof(string));

            Session["employeeBucket"] = employeeBucket;
        }

        public void createEvaluationBucket()
        {
            DataTable evaluationBucket = new DataTable();
            evaluationBucket.Columns.Add("EVALUATION_ID", typeof(string));
            evaluationBucket.Columns.Add("IS_PRE_POST", typeof(string));

            Session["evaluationBucket"] = evaluationBucket;
        }

        public DataTable readEvaluationList()
        {
            try
            {
                createEvaluationBucket();

                DataTable dtEvaluation = (DataTable)Session["evaluationBucket"];

                string preEvaluation = hfPreEvaluation.Value;
                string postEvaluation = hfPostEvaluation.Value;

                if (preEvaluation != "")
                {
                    DataRow dtrow = dtEvaluation.NewRow();
                    dtrow["IS_PRE_POST"] = "1";
                    dtrow["EVALUATION_ID"] = preEvaluation;
                    dtEvaluation.Rows.Add(dtrow);
                }

                if (preEvaluation != "")
                {
                    DataRow dtrow = dtEvaluation.NewRow();
                    dtrow["IS_PRE_POST"] = "0";
                    dtrow["EVALUATION_ID"] = postEvaluation;
                    dtEvaluation.Rows.Add(dtrow);
                }

                return dtEvaluation;
            }
            catch (Exception ex)
            {

                throw ex;
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

        public DataTable readEvaluationList(string trainingId)
        {
            DataTable dtexist = new DataTable();
            PrePostEvaluationDataHandler PPEDH = new PrePostEvaluationDataHandler();

            try
            {
                dtexist = PPEDH.getExistData(trainingId);

                return dtexist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                PPEDH = null;
                dtexist.Dispose();
            }
        }

        public void viewExistTrainingData()
        {
            DataTable dt = new DataTable();

            try
            {
                dt = readEvaluationList(txtTraining.Text);

                if (dt.Rows.Count > 0)
                {
                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

                    foreach (DataRow dr in dt.Rows)
                    {
                        string evaluation = dr["EVALUATION_ID"].ToString();
                        string prePost = dr["IS_POST_EVALUATION"].ToString();
                        string empId = dr["EMPLOYEE_ID"].ToString();
                        string evaluator = dr["EVALUATOR"].ToString();
                        string comment = dr["COMMENTS"].ToString();
                        string startDate = dr["EVALUATION_START_DATE"].ToString();
                        string endDate = dr["EVALUATION_END_DATE"].ToString();
                        string status = dr["STATUS_CODE"].ToString();

                        for (int i = 0; i < grdParticipantDetails.Rows.Count; i++)
                        {
                            if (empId == evaluator)
                            {
                                if (grdParticipantDetails.Rows[i].Cells[2].Text.ToString() == empId)
                                {
                                    CheckBox chk = (grdParticipantDetails.Rows[i].Cells[10].FindControl("chkEmployee") as CheckBox);
                                    chk.Checked = true;
                                }
                            }
                            else
                            {
                                if (grdParticipantDetails.Rows[i].Cells[2].Text.ToString() == empId)
                                {
                                    CheckBox chk = (grdParticipantDetails.Rows[i].Cells[11].FindControl("chkSupervisor") as CheckBox);
                                    chk.Checked = true;
                                }
                            }
                        }

                        if (prePost == "1")
                        {
                            chkPreEvaluation.Checked = true;
                        }
                        if (prePost == "0")
                        {
                            chkPostEvaluation.Checked = true;
                        }

                        txtComment.Text = comment;

                        if (startDate != null)
                        {
                            string[] dateArr = startDate.Split('/', '-');
                            startDate = dateArr[0] + "/" + dateArr[1] + "/" + dateArr[2];
                        }

                        if (endDate != null)
                        {
                            string[] dateArr = endDate.Split('/', '-');
                            endDate = dateArr[0] + "/" + dateArr[1] + "/" + dateArr[2];
                        }

                        txtStartDate.Text = startDate;
                        txtEndDate.Text = endDate;
                        //ddlStatus.SelectedValue = status;
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));

                    }

                }
                else
                {
                    hfPreEvaluation.Value = "";
                    hfPostEvaluation.Value = "";
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                dt.Dispose();
            }
        }

        public bool checkDatevalidation(string stDate, string edDate)
        {
            bool isTrueDate = false;
            try
            {
                //isTrueDate = CommonUtils.isValidDateRange(stDate, edDate);
                DateTime dtFrom = DateTime.ParseExact(stDate.Trim(), "yyyy/MM/dd", CultureInfo.InvariantCulture); //DateTime.Parse(fromDate);
                DateTime dtTo = DateTime.ParseExact(edDate.Trim(), "yyyy/MM/dd", CultureInfo.InvariantCulture); //DateTime.Parse(toDate);

                if (dtFrom <= dtTo)
                {
                    isTrueDate = true;
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            return isTrueDate;
        }



    }
}