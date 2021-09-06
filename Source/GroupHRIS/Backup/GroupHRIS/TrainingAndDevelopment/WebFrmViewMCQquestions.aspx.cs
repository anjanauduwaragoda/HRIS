using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using DataHandler.Userlogin;
using System.Data;
using Common;
using DataHandler.TrainingAndDevelopment;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmViewMCQquestions : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        string evaluationId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            PasswordHandler crpto;
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmMultipleChoiceQuestion : Page_Load");

            if (!IsPostBack)
            {
                crpto = new PasswordHandler();
                string programId = Request.QueryString["EvaluationId"];
                evaluationId = crpto.Decrypt(programId);
                createMCQBucket();

                GenerateTable();
            }
        }

        protected void btnFinalize_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                readAnswers();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void createMCQBucket()
        {
            log.Debug("createMCQBucket()");
            DataTable mcqBucket = new DataTable();
            try
            {
                mcqBucket.Columns.Add("EVALUATION_ID", typeof(string));
                mcqBucket.Columns.Add("TRAINING_ID", typeof(string));
                mcqBucket.Columns.Add("EMPLOYEE_ID", typeof(string));//
                mcqBucket.Columns.Add("MCQ_ID", typeof(string));
                mcqBucket.Columns.Add("ANSWER_ID", typeof(string));

                Session["MCQBucket"] = mcqBucket;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                mcqBucket.Dispose();
            }
        }

        private void GenerateTable()
        {
            log.Debug("GenerateTable()");
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);

            viewMCQDataHandler VMQH = new viewMCQDataHandler();
            DataTable dt = new DataTable();

            //Creat the Table and Add it to the Page
            Table table = new Table();
            table.ID = "Table1";
            table.Width = 850;

            lbltbl.Controls.Add(table);

            try
            {
                dt = VMQH.getMCQ(evaluationId);
                string val = "";
                
                int x = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string mcqId = dt.Rows[i][0].ToString();
                    string choices = dt.Rows[i][1].ToString();
                    string question = dt.Rows[i][2].ToString();
                    string answerId = dt.Rows[i][3].ToString();
                    string answer = dt.Rows[i][4].ToString();
                    
                    int count = Int32.Parse(choices);
                    
                    if (val != mcqId)
                    {
                        x = x + 1;
                        TableRow row = new TableRow();
                        TableCell cell = new TableCell();

                        Label lbl = new Label();
                        lbl.ID = "lbl_" + mcqId;
                        lbl.Width = 850;
                        lbl.Text = (x) + ") " + question + "<br />";

                        cell.Controls.Add(lbl);
                        row.Cells.Add(cell);

                        RadioButtonList rbl = new RadioButtonList();

                        for (int j = 0; j < count; j++)
                        {
                            rbl.ID = mcqId;

                            rbl.Items.Add(new ListItem((dt.Rows[i + j][4].ToString()), (dt.Rows[i + j][3].ToString())));

                            //rbl.Items.Add(dt.Rows[i + j][4].ToString());
                            cell.Controls.Add(rbl);
                            row.Cells.Add(cell);
                        }
                        table.Rows.Add(row);
                        val = mcqId;

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
                VMQH = null;
                dt.Dispose();
            }

        }

        private void readAnswers()
        {
            viewMCQDataHandler VMQH = new viewMCQDataHandler();
            DataTable dt = new DataTable();
            PasswordHandler crpto = new PasswordHandler();

            try
            {
                //GenerateTable();
                createMCQBucket();
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                string programId = Request.QueryString["EvaluationId"];
                evaluationId = crpto.Decrypt(programId);
                dt = VMQH.getMCQ(evaluationId);
                string val = "";
                int x = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string mcqId = dt.Rows[i][0].ToString();
                    string choices = dt.Rows[i][1].ToString();
                    string question = dt.Rows[i][2].ToString();
                    string answerId = dt.Rows[i][3].ToString();
                    string answer = dt.Rows[i][4].ToString();

                    int count = Int32.Parse(choices);

                    x = x + 1;
                    if (val != mcqId)
                    {
                        TableRow row = new TableRow();
                        TableCell cell = new TableCell();

                        Label lbl = new Label();
                        lbl.ID = "lbl_" + mcqId;
                        RadioButtonList rbl = new RadioButtonList();
                        string item = "";

                        for (int j = 0; j < count; j++)
                        {
                            rbl.ID = mcqId;

                            if (rbl.Items.FindByValue("1").Selected == true)
                            {
                                item = rbl.SelectedValue;
                            }
                            //string value = "";
                            //bool isChecked = rbl.S;
                            //if (isChecked)
                            //    value = rbl.Text;
                            //else
                            //    value = rbl.Text;
                        }
                        
                        val = mcqId;
                    
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
                VMQH = null;
                dt.Dispose();
            }
        }
    }
}