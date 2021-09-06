using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using DataHandler.TrainingAndDevelopment;
using System.Data;
using DataHandler.Userlogin;
using System.Collections;
using Common;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFormEmployeeSelfTrainingEvaluation_Essay : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        #region events

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFormEmployeeSelfTrainingEvaluation_Essay : Page_Load");

            PasswordHandler crpto = new PasswordHandler();

            string evaluationId = crpto.Decrypt(Request.QueryString["evaluationId"].ToString());
            hfEvaluationId.Value = evaluationId;
            hfEmployeeId.Value = crpto.Decrypt(Request.QueryString["employeeId"].ToString());
            hfTrainingId.Value = crpto.Decrypt(Request.QueryString["trainingId"].ToString());
            hfIsPostEvaluation.Value = crpto.Decrypt(Request.QueryString["isPostEvaluation"].ToString());

            generateQuestionPaper(evaluationId);
            fillTextboxesWithAnswers();

            string isFinalizedStatus = crpto.Decrypt(Request.QueryString["isFinalizedStatus"].ToString());
            if (isFinalizedStatus == Constants.STATUS_ACTIVE_VALUE)
            {
                disableControls();
            }

        }

        protected void btnFinalized_Click(object sender, EventArgs e)
        {
            EmployeeSelfTrainingEvaluation_EssayDataHandler employeeSelfTrainingEvaluation_EssayDataHandler = new EmployeeSelfTrainingEvaluation_EssayDataHandler();
            try
            {
                DataTable dtQuestions = employeeSelfTrainingEvaluation_EssayDataHandler.getEssayQuestions(hfEvaluationId.Value.ToString());
                Dictionary<string, Array> answerDictionary = new Dictionary<string, Array>();

                foreach (DataRow question in dtQuestions.Rows)
                {
                    int numberOfAnswers = Convert.ToInt32(question["NO_OF_ANSWERS"].ToString());
                    string[] answersArray = new string[numberOfAnswers];

                    for (int i = 0; i < numberOfAnswers; i++)
                    {
                        string textBoxId = question["EQ_ID"].ToString() + "A" + (i + 1);
                        TextBox textBox = new TextBox();
                        textBox = (TextBox)questionDiv.FindControl(textBoxId);
                        string answer = textBox.Text.ToString();
                        answersArray[i] = answer;
                    }
                    answerDictionary.Add(question["EQ_ID"].ToString(), answersArray);
                }

                string employeeId = hfEmployeeId.Value.ToString();
                string evaluationId = hfEvaluationId.Value.ToString();
                string trainingId = hfTrainingId.Value.ToString();

                string isFinalizedStatus = Constants.STATUS_ACTIVE_VALUE;

                bool inserted = employeeSelfTrainingEvaluation_EssayDataHandler.insertAnswers(employeeId, evaluationId, trainingId, answerDictionary, isFinalizedStatus);

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                employeeSelfTrainingEvaluation_EssayDataHandler = null;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            EmployeeSelfTrainingEvaluation_EssayDataHandler employeeSelfTrainingEvaluation_EssayDataHandler = new EmployeeSelfTrainingEvaluation_EssayDataHandler();
            try
            {
                DataTable dtQuestions = employeeSelfTrainingEvaluation_EssayDataHandler.getEssayQuestions(hfEvaluationId.Value.ToString());
                Dictionary<string, Array> answerDictionary = new Dictionary<string, Array>();

                foreach (DataRow question in dtQuestions.Rows)
                {
                    int numberOfAnswers = Convert.ToInt32(question["NO_OF_ANSWERS"].ToString());
                    string[] answersArray = new string[numberOfAnswers];

                    for (int i = 0; i < numberOfAnswers; i++)
                    {
                        string textBoxId = question["EQ_ID"].ToString() + "A" + (i + 1);
                        TextBox textBox = new TextBox();
                        textBox = (TextBox)questionDiv.FindControl(textBoxId);
                        string answer = textBox.Text.ToString();
                        answersArray[i] = answer;
                    }
                    answerDictionary.Add(question["EQ_ID"].ToString(), answersArray);
                }

                string employeeId = hfEmployeeId.Value.ToString();
                string evaluationId = hfEvaluationId.Value.ToString();
                string trainingId = hfTrainingId.Value.ToString();

                string isFinalizedStatus = Constants.STATUS_INACTIVE_VALUE; 

                bool inserted = employeeSelfTrainingEvaluation_EssayDataHandler.insertAnswers(employeeId, evaluationId, trainingId, answerDictionary, isFinalizedStatus);

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                employeeSelfTrainingEvaluation_EssayDataHandler = null;
            }
        }

        #endregion

        #region methodes

        private void generateQuestionPaper(string evaluationId)
        {
            log.Debug("WebFormEmployeeSelfTrainingEvaluation_Essay : generateQuestionPaper");
            try
            {
                EmployeeSelfTrainingEvaluation_EssayDataHandler employeeSelfTrainingEvaluation_EssayDataHandler = new EmployeeSelfTrainingEvaluation_EssayDataHandler();

                DataTable dtQuestions = employeeSelfTrainingEvaluation_EssayDataHandler.getEssayQuestions(evaluationId);
                int questionNumber = 1;
                foreach (DataRow question in dtQuestions.Rows)
                {
                    Table table = new Table();
                    table.Style.Add("width", "100%");
                    TableRow trQuestion = new TableRow();
                    TableCell tcQuestion = new TableCell();

                    tcQuestion.Text = questionNumber + ".) " + question["QUESTION"].ToString();

                    trQuestion.Cells.Add(tcQuestion);
                    table.Rows.Add(trQuestion);

                    for (int i = 0; i < Convert.ToInt64(question["NO_OF_ANSWERS"].ToString()); i++)
                    {
                        TableRow trAnswer = new TableRow();
                        TableCell tcAnswer = new TableCell();

                        TextBox tbAnswer = new TextBox();
                        tbAnswer.ID = question["EQ_ID"].ToString() + "A" + (i + 1);
                        tbAnswer.Style.Add("width", "80%");
                        tbAnswer.Attributes.Add("placeholder", "Answer " + (i + 1));
                        tbAnswer.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                        tbAnswer.TextMode = TextBoxMode.MultiLine;
                        tbAnswer.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                        tcAnswer.Controls.Add(tbAnswer);
                        trAnswer.Cells.Add(tcAnswer);
                        table.Rows.Add(trAnswer);
                    }

                    questionDiv.Controls.Add(table);
                    questionDiv.Controls.Add(new LiteralControl("<br />"));
                    questionNumber++;

                }

                dtQuestions.Dispose();
                employeeSelfTrainingEvaluation_EssayDataHandler = null;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }
        }

        private void fillTextboxesWithAnswers()
        {
            log.Debug("WebFormEmployeeSelfTrainingEvaluation_Essay : fillTextboxesWithAnswers");
            EmployeeSelfTrainingEvaluation_EssayDataHandler employeeSelfTrainingEvaluation_EssayDataHandler = new EmployeeSelfTrainingEvaluation_EssayDataHandler();
            DataTable dtEssayQuestions = new DataTable();
            DataTable dtEssayAnswers = new DataTable();

            try
            {
                string employeeId = hfEmployeeId.Value.ToString();
                string evaluationId = hfEvaluationId.Value.ToString();
                string trainingId = hfTrainingId.Value.ToString();
                string isPostEvaluation = hfIsPostEvaluation.Value.ToString();

                dtEssayAnswers = employeeSelfTrainingEvaluation_EssayDataHandler.getEssayAnswers(employeeId, evaluationId, trainingId, isPostEvaluation);
                dtEssayQuestions = employeeSelfTrainingEvaluation_EssayDataHandler.getEssayQuestions(evaluationId);

                foreach (DataRow question in dtEssayQuestions.Rows)
                {
                    string questionId = question["EQ_ID"].ToString();
                    string expression = "[ESSAY_QUESTION_ID]='" + questionId + "' ";
                    DataRow[] answers = dtEssayAnswers.Select(expression);

                    for (int i = 0; i < Convert.ToInt32(question["NO_OF_ANSWERS"].ToString()); i++)
                    {
                        string textBoxId = question["EQ_ID"].ToString() + "A" + (i + 1);
                        TextBox answerTb = (TextBox)FindControl(textBoxId);

                        answerTb.Text = answers[i]["ANSWER"].ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void disableControls()
        {
            EmployeeSelfTrainingEvaluation_EssayDataHandler employeeSelfTrainingEvaluation_EssayDataHandler = new EmployeeSelfTrainingEvaluation_EssayDataHandler();

            try
            {
                string evaluationId = hfEvaluationId.Value.ToString();
                DataTable dtQuestions = employeeSelfTrainingEvaluation_EssayDataHandler.getEssayQuestions(evaluationId);

                foreach (DataRow question in dtQuestions.Rows)
                {
                    string questionId = question["EQ_ID"].ToString();

                    for (int i = 0; i < Convert.ToInt32(question["NO_OF_ANSWERS"].ToString()); i++)
                    {
                        string textBoxId = question["EQ_ID"].ToString() + "A" + (i + 1);
                        TextBox answerTb = (TextBox)FindControl(textBoxId);

                        answerTb.ReadOnly = true;
                    }
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