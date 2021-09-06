using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.PerformanceManagement;
using System.Data;
using System.Drawing;
using DataHandler.Userlogin;
using GroupHRIS.Utility;
using Common;
using DataHandler.Utility;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmViewSelfAssessment : System.Web.UI.Page
    {

        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            ViewSelfAssessmentProfileDataHandler oViewSelfAssessmentProfileDataHandler = new ViewSelfAssessmentProfileDataHandler();
            PasswordHandler cripto = new PasswordHandler();
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            string assmtId = "";
            string assmtYear = "";

            GenerateTable();

            if (!IsPostBack)
            {
                sIPAddress = Request.UserHostAddress;
                log.Debug("IP:" + sIPAddress + "WebFrmViewSelfAssessment : Page_Load");

                assmtId = cripto.Decrypt(Request.QueryString["assmtId"]);
                assmtYear = cripto.Decrypt(Request.QueryString["year"]);
                DataTable existAssessmentDt = new DataTable();
                existAssessmentDt = oViewSelfAssessmentProfileDataHandler.getexistAssessment(KeyEMPLOYEE_ID, assmtId, assmtYear).Copy();

                //get page status
                if (existAssessmentDt.Rows.Count > 0)
                {
                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                    string token = existAssessmentDt.Rows[0]["ASSESSMENT_TOKEN"].ToString();
                    string assmtStatus = oViewSelfAssessmentProfileDataHandler.getStatus(token);

                    if (assmtStatus != Constants.ASSESSNEMT_ACTIVE_STATUS)
                    {
                        createAnswerBucket();
                        loadExistAnswer();
                        readFinalizedAnswers();
                        btnFinalized.Enabled = false;
                        btnSave.Enabled = false;
                    }
                    else
                    {
                        createAnswerBucket();
                        loadExistAnswer();
                    }
                }

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");
            ViewSelfAssessmentProfileDataHandler oViewSelfAssessmentProfileDataHandler = new ViewSelfAssessmentProfileDataHandler();
            PasswordHandler cripto = new PasswordHandler();
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            string assmtId = "";
            string assmtYear = "";

            try
            {
                assmtId = cripto.Decrypt(Request.QueryString["assmtId"]);
                assmtYear = cripto.Decrypt(Request.QueryString["year"]);

                DataTable existAssessmentDt = new DataTable();
                existAssessmentDt = oViewSelfAssessmentProfileDataHandler.getexistAssessment(KeyEMPLOYEE_ID, assmtId, assmtYear).Copy();

                readAnswers();
                DataTable dtAnswers = (DataTable)Session["answerBucket"];

                Boolean isCompleted = readAnswersAtLeastOneToSave();
                if (isCompleted)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "At least one answer should be enter for each question before save", lblMessage);
                    return;
                }

                if (existAssessmentDt.Rows.Count > 0)
                {
                    string token = existAssessmentDt.Rows[0]["ASSESSMENT_TOKEN"].ToString();
                    Boolean isSuccess = oViewSelfAssessmentProfileDataHandler.InsertifExist(assmtId, dtAnswers, KeyEMPLOYEE_ID, assmtYear, token);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
                }
                else
                {
                    Boolean isSuccess = oViewSelfAssessmentProfileDataHandler.Insert(assmtId, dtAnswers, KeyEMPLOYEE_ID, assmtYear);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblMessage);
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oViewSelfAssessmentProfileDataHandler = null;
                cripto = null;
            }
        }

        protected void btnFinalized_Click(object sender, EventArgs e)
        {
            log.Debug("btnFinalized_Click()");
            ViewSelfAssessmentProfileDataHandler oViewSelfAssessmentProfileDataHandler = new ViewSelfAssessmentProfileDataHandler();
            PasswordHandler cripto = new PasswordHandler();
            UtilsDataHandler oUtilsDataHandler = new UtilsDataHandler();

            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);

            DataTable existAssessmentDt = new DataTable();
            string assmtId = "";
            string assmtYear = "";

            try
            {
                assmtId = cripto.Decrypt(Request.QueryString["assmtId"]);
                assmtYear = cripto.Decrypt(Request.QueryString["year"]);

                existAssessmentDt = oViewSelfAssessmentProfileDataHandler.getexistAssessment(KeyEMPLOYEE_ID, assmtId, assmtYear).Copy();


                Boolean isCompleted = readAnswersAtLeastOneToSave();
                if (isCompleted)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "At least one answer should be enter before save", lblMessage);
                    return;
                }

                if (existAssessmentDt.Rows.Count > 0)
                {
                    Boolean isCompletedans = readAnswersValid();
                    if (isCompletedans)
                    {
                        string token = existAssessmentDt.Rows[0]["ASSESSMENT_TOKEN"].ToString();
                        Boolean isSuccess = oViewSelfAssessmentProfileDataHandler.FinalizedifExist(token, assmtYear);
                        Boolean isFinalized = oUtilsDataHandler.updateAssessmentStatus(assmtId, KeyEMPLOYEE_ID, assmtYear);
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully finalized", lblMessage);
                        readFinalizedAnswers();
                    }
                    else
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "To finalized at least one answer is required for each question. ", lblMessage);
                    }
                }
                else
                {
                    readAnswers();
                    DataTable dtAnswers = (DataTable)Session["answerBucket"];


                    Boolean isSuccessSave = oViewSelfAssessmentProfileDataHandler.Insert(assmtId, dtAnswers, KeyEMPLOYEE_ID, assmtYear);
                    Boolean isCompletedans = readAnswersValid();
                    if (isCompletedans && isSuccessSave)
                    {
                        
                        DataTable existAssessment = new DataTable();
                        existAssessmentDt = oViewSelfAssessmentProfileDataHandler.getexistAssessment(KeyEMPLOYEE_ID, assmtId, assmtYear).Copy();

                        string token = existAssessmentDt.Rows[0]["ASSESSMENT_TOKEN"].ToString();
                        Boolean isSuccessEx = oViewSelfAssessmentProfileDataHandler.FinalizedifExist(token, assmtYear);
                        
                        Boolean isFinalized = oUtilsDataHandler.updateAssessmentStatus(assmtId, KeyEMPLOYEE_ID, assmtYear);
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully finalized", lblMessage);
                        readFinalizedAnswers();
                    }

                    //CommonVariables.MESSAGE_TEXT = "No saved data to update.";
                    //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oViewSelfAssessmentProfileDataHandler = null;
                cripto = null;
                existAssessmentDt.Dispose();
            }

        }



        //private void AddControls(string question, int controlNumber, int v)
        //{
        //    var newPanel = new Panel();
        //    var newLabel = new Label();

        //    newLabel.ID = "lblQuestion_" + v;
        //    newLabel.Text = question;

        //    for (int i = 1; i < controlNumber; i++)
        //    {
        //        var newTextbox = new TextBox();

        //        newTextbox.ID = "TextBox_" + controlNumber;
        //        newTextbox.TextMode = TextBoxMode.MultiLine;
        //        newTextbox.Width = 600;

        //        newPanel.Controls.Add(newTextbox);
        //    }
        //    form1.Controls.Add(newPanel);
        //}

        private void GenerateTable()
        {
            log.Debug("GenerateTable()");
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            ViewSelfAssessmentProfileDataHandler oViewSelfAssessmentProfileDataHandler = new ViewSelfAssessmentProfileDataHandler();

            //Creat the Table and Add it to the Page
            Table table = new Table();
            table.ID = "Table1";
            table.Width = 850;

            lbltbl.Controls.Add(table);

            DataTable dt = oViewSelfAssessmentProfileDataHandler.getSelfAssessmentProfileQuestions(KeyEMPLOYEE_ID);

            try
            {
                // Now iterate through the table and add your controls 
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string profileId = dt.Rows[i][4].ToString();
                    string questionId = dt.Rows[i][6].ToString();
                    string question = dt.Rows[i][8].ToString();
                    string noOfAns = dt.Rows[i][7].ToString();

                    int count = Int32.Parse(noOfAns);

                    TableRow row = new TableRow();
                    TableCell cell = new TableCell();

                    Label lbl = new Label();
                    lbl.ID = "lbl_" + i;
                    lbl.Width = 850;
                    lbl.Text = (i + 1) + ") " + question + "<br />(" + noOfAns + " answer(s) required.)";

                    cell.Controls.Add(lbl);
                    row.Cells.Add(cell);

                    //
                    TableRow tRow = new TableRow();
                    table.Rows.Add(tRow);
                    TableRow tRow1 = new TableRow();
                    table.Rows.Add(tRow1);
                    TableRow tRow2 = new TableRow();
                    table.Rows.Add(tRow2);
                    TableRow tRow3 = new TableRow();
                    table.Rows.Add(tRow3);

                    for (int j = 0; j < count; j++)
                    {

                        TableCell cell2 = new TableCell();
                        TextBox tb = new TextBox();

                        // Set a unique ID for each TextBox added
                        tb.ID = "TextBoxRow_" + i + "Col_" + j + "_" + questionId;
                        tb.TextMode = TextBoxMode.MultiLine;
                        tb.Width = 850;
                        tb.MaxLength = 500;
                        //tb.ForeColor = Color.Gray;
                        //tb.Text = "Type here... ";

                        Label lbl1 = new Label();
                        lbl1.ID = "lbl1_" + i + "Col_" + j;
                        lbl1.Width = 850;
                        lbl1.Text = "Answer " + (j + 1) + " ";
                        //lbl1.ForeColor = Color.Gray;
                        cell.Controls.Add(lbl1);

                        // Add the control to the TableCell
                        cell.Controls.Add(tb);
                        // Add the TableCell to the TableRow
                        row.Cells.Add(cell);
                    }

                    // Add the TableRow to the Table
                    table.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oViewSelfAssessmentProfileDataHandler = null;
                dt.Dispose();
            }
        }

        private void readAnswers()
        {
            log.Debug("readAnswers()");
            ViewSelfAssessmentProfileDataHandler oViewSelfAssessmentProfileDataHandler = new ViewSelfAssessmentProfileDataHandler();

            try
            {
                createAnswerBucket();
                DataTable dtAnswers = (DataTable)Session["answerBucket"];
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);

                DataTable dt = oViewSelfAssessmentProfileDataHandler.getSelfAssessmentProfileQuestions(KeyEMPLOYEE_ID);

                // Now iterate through the table and add your controls 
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string profileId = dt.Rows[i][4].ToString();
                    string questionId = dt.Rows[i][6].ToString();
                    string noOfAns = dt.Rows[i][7].ToString();

                    int count = Int32.Parse(noOfAns);

                    for (int j = 0; j < count; j++)
                    {
                        TextBox tb = new TextBox();
                        string controlerId = "TextBoxRow_" + i.ToString() + "Col_" + j.ToString() + "_" + questionId;
                        tb = (TextBox)FindControl(controlerId);

                        // tb = (cell.FindControl(controlerId.ToString()) as TextBox);
                        string answer = tb.Text.ToString(); //You have the data now
                        DataRow dtrow = dtAnswers.NewRow();
                        dtrow["SELF_ASSESSMENT_PROFILE_ID"] = profileId;
                        dtrow["QUESTION_ID"] = questionId;
                        dtrow["NO_OF_ANSWERS"] = j;
                        dtrow["ANSWER"] = answer;

                        dtAnswers.Rows.Add(dtrow);

                    }

                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oViewSelfAssessmentProfileDataHandler = null;
            }
        }

        private void loadExistAnswer()
        {
            DataTable dtAnswers = (DataTable)Session["answerBucket"];
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            ViewSelfAssessmentProfileDataHandler oViewSelfAssessmentProfileDataHandler = new ViewSelfAssessmentProfileDataHandler();
            PasswordHandler cripto = new PasswordHandler();
            string assmtId = "";
            string assmtYear = "";

            try
            {
                assmtId = cripto.Decrypt(Request.QueryString["assmtId"]);
                assmtYear = cripto.Decrypt(Request.QueryString["year"]);
                DataTable existAssessmentDt = new DataTable();
                existAssessmentDt = oViewSelfAssessmentProfileDataHandler.getexistAssessment(KeyEMPLOYEE_ID, assmtId, assmtYear).Copy();

                if (existAssessmentDt.Rows.Count > 0)
                {
                    string token = existAssessmentDt.Rows[0]["ASSESSMENT_TOKEN"].ToString();

                    DataTable answerDt = new DataTable();
                    answerDt = oViewSelfAssessmentProfileDataHandler.getexistAssessmentQuestions(token).Copy();

                    int existAns = 0;

                    //read answe count for change button status
                    for (int i = 0; i < answerDt.Rows.Count; i++)
                    {
                        string ansCount = answerDt.Rows[i]["ANSWER"].ToString();
                        if (ansCount != "")
                        {
                            existAns = existAns + 1;
                        }
                    }

                    if (existAns > 0)
                    {
                        btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                    }


                    DataTable dt = new DataTable();
                    dt = oViewSelfAssessmentProfileDataHandler.getSelfAssessmentProfileQuestions(KeyEMPLOYEE_ID).Copy();

                    int x = 0;

                    // Now iterate through the table and add your controls 
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string profileId = dt.Rows[i][4].ToString();
                        string questionId = dt.Rows[i][6].ToString();
                        string noOfAns = dt.Rows[i][7].ToString();

                        int count = Int32.Parse(noOfAns);

                        for (int j = 0; j < count; j++)
                        {
                            TextBox tb = new TextBox();
                            string controlerId = "TextBoxRow_" + i.ToString() + "Col_" + j.ToString() + "_" + questionId;
                            tb = (TextBox)FindControl(controlerId);

                            string existAnswer = answerDt.Rows[j + x][2].ToString();
                            string answer = existAnswer; //You have the data now
                            tb.Text = answer;

                        }
                        x = x + count;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oViewSelfAssessmentProfileDataHandler = null;
                cripto = null;
                dtAnswers.Dispose();
            }
        }

        private void createAnswerBucket()
        {
            log.Debug("createAnswerBucket()");
            DataTable answerBucket = new DataTable();
            try
            {
                answerBucket.Columns.Add("SELF_ASSESSMENT_PROFILE_ID", typeof(string));
                answerBucket.Columns.Add("QUESTION_ID", typeof(string));
                answerBucket.Columns.Add("NO_OF_ANSWERS", typeof(string));//
                answerBucket.Columns.Add("ANSWER", typeof(string));

                Session["answerBucket"] = answerBucket;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                answerBucket.Dispose();
            }
        }

        private void readFinalizedAnswers()
        {
            log.Debug("readFinalizedAnswers()");
            ViewSelfAssessmentProfileDataHandler oViewSelfAssessmentProfileDataHandler = new ViewSelfAssessmentProfileDataHandler();
            DataTable dt = new DataTable();
            try
            {
                DataTable dtAnswers = (DataTable)Session["answerBucket"];
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);

                dt = oViewSelfAssessmentProfileDataHandler.getSelfAssessmentProfileQuestions(KeyEMPLOYEE_ID);

                // Now iterate through the table and add your controls 
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string profileId = dt.Rows[i][4].ToString();
                    string questionId = dt.Rows[i][6].ToString();
                    string noOfAns = dt.Rows[i][7].ToString();

                    int count = Int32.Parse(noOfAns);

                    for (int j = 0; j < count; j++)
                    {
                        TextBox tb = new TextBox();
                        string controlerId = "TextBoxRow_" + i.ToString() + "Col_" + j.ToString() + "_" + questionId;
                        tb = (TextBox)FindControl(controlerId);

                        // tb = (cell.FindControl(controlerId.ToString()) as TextBox);
                        string answer = tb.Text.ToString().Trim(); //You have the data now
                        tb.Text = answer;
                        tb.Enabled = false;


                    }


                }
                btnFinalized.Enabled = false;
                btnSave.Enabled = false;

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oViewSelfAssessmentProfileDataHandler = null;
                dt.Dispose();
            }
        }

      
        private Boolean readAnswersValid()
        {
            log.Debug("readAnswersValid()");
            ViewSelfAssessmentProfileDataHandler oViewSelfAssessmentProfileDataHandler = new ViewSelfAssessmentProfileDataHandler();

            Boolean status = true;
            try
            {
                createAnswerBucket();
                DataTable dtAnswers = (DataTable)Session["answerBucket"];
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);

                DataTable dt = oViewSelfAssessmentProfileDataHandler.getSelfAssessmentProfileQuestions(KeyEMPLOYEE_ID);

                // Now iterate through the table and add your controls 
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string profileId = dt.Rows[i][4].ToString();
                    string questionId = dt.Rows[i][6].ToString();
                    string noOfAns = dt.Rows[i][7].ToString();
                    int validateAnsCount = 0;
                    int count = Int32.Parse(noOfAns);

                    for (int j = 0; j < count; j++)
                    {
                        TextBox tb = new TextBox();
                        string controlerId = "TextBoxRow_" + i.ToString() + "Col_" + j.ToString() + "_" + questionId;
                        tb = (TextBox)FindControl(controlerId);

                        // tb = (cell.FindControl(controlerId.ToString()) as TextBox);
                        string answer = tb.Text.ToString(); //You have the data now
                        DataRow dtrow = dtAnswers.NewRow();
                        dtrow["SELF_ASSESSMENT_PROFILE_ID"] = profileId;
                        dtrow["QUESTION_ID"] = questionId;
                        dtrow["NO_OF_ANSWERS"] = j;
                        dtrow["ANSWER"] = answer;

                        dtAnswers.Rows.Add(dtrow);

                        if (answer.Trim() != "")
                        {
                            //tb.Text = answer;
                        }
                        else
                        {
                            validateAnsCount = validateAnsCount + 1;
                        }
                    }

                    if (validateAnsCount == count)
                    {
                        status = false;
                        dt.Dispose();
                        dtAnswers.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oViewSelfAssessmentProfileDataHandler = null;
            }

            return status;
        }

        private Boolean readAnswersAtLeastOneToSave()
        {
            ViewSelfAssessmentProfileDataHandler oViewSelfAssessmentProfileDataHandler = new ViewSelfAssessmentProfileDataHandler();

            Boolean status = false;
            try
            {
                createAnswerBucket();
                DataTable dtAnswers = (DataTable)Session["answerBucket"];
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                int qCount = 0;
                DataTable dt = oViewSelfAssessmentProfileDataHandler.getSelfAssessmentProfileQuestions(KeyEMPLOYEE_ID);

                // Now iterate through the table and add your controls 
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string profileId = dt.Rows[i][4].ToString();
                    string questionId = dt.Rows[i][6].ToString();
                    string noOfAns = dt.Rows[i][7].ToString();
                    int validateAnsCount = 0;
                    int count = Int32.Parse(noOfAns);

                    for (int j = 0; j < count; j++)
                    {
                        TextBox tb = new TextBox();
                        string controlerId = "TextBoxRow_" + i.ToString() + "Col_" + j.ToString() + "_" + questionId;
                        tb = (TextBox)FindControl(controlerId);

                        // tb = (cell.FindControl(controlerId.ToString()) as TextBox);
                        string answer = tb.Text.ToString(); //You have the data now
                        DataRow dtrow = dtAnswers.NewRow();
                        dtrow["SELF_ASSESSMENT_PROFILE_ID"] = profileId;
                        dtrow["QUESTION_ID"] = questionId;
                        dtrow["NO_OF_ANSWERS"] = j;
                        dtrow["ANSWER"] = answer;

                        dtAnswers.Rows.Add(dtrow);

                        if (answer.Trim() != "")
                        {
                            //tb.Text = answer;
                        }
                        else
                        {
                            validateAnsCount = validateAnsCount + 1;
                        }
                    }

                    if (validateAnsCount == count)
                    {
                        qCount = qCount + 1;
                    }

                    if (qCount == dt.Rows.Count)
                    {
                        status = true;
                    }
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oViewSelfAssessmentProfileDataHandler = null;
                
            }

            return status;
        }

    }
}