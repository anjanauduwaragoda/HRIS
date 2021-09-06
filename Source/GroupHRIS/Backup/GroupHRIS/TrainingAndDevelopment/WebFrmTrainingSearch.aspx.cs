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

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainingSearch : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmTrainingSearch : Page_Load");

            if (!IsPostBack)
            {
                //getTrainingProgram();
                //getTrainingType();
                
            }

            filterAllTraining();
        }

        protected void grdTrainingDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdTrainingDetails, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdTrainingDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("grdTrainingDetails_SelectedIndexChanged()");

            try
            {
                int SelectedIndex = grdTrainingDetails.SelectedIndex;
                btnSelect.Visible = true;
                txtTrId.Text = grdTrainingDetails.SelectedRow.Cells[0].Text;
                hfTrainingName.Value = grdTrainingDetails.SelectedRow.Cells[3].Text;
                Session["TrainingName"] = grdTrainingDetails.SelectedRow.Cells[3].Text;

                hfProgram.Value = grdTrainingDetails.SelectedRow.Cells[1].Text;
                txtTrainingName.Text = grdTrainingDetails.SelectedRow.Cells[3].Text;
                
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            log.Debug("btnSearch_Click()");

            try
            {
                txtTrId.Text = "";
                txtTrainingName.Text = "";
                filterAllTraining();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }



        public void allTraining()
        {
            TrainingSearchDataHandler TSDH = new TrainingSearchDataHandler();
            
            try
            {
                grdTrainingDetails.DataSource = TSDH.getAllTraining();
                grdTrainingDetails.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        public void filterAllTraining()
        {
            TrainingSearchDataHandler TSDH = new TrainingSearchDataHandler();
            string stDate = txtStDate.Text;
            string endDate = txtEndDate.Text;
            string name = txtTraining.Text;
            string code = txtCode.Text;
            //string year = ddlTYear.SelectedValue;

            try
            {
                grdTrainingDetails.DataSource = TSDH.filterTraining(stDate, endDate, name, code);
                grdTrainingDetails.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TSDH = null;
            }
        }

        public void allTrainingByType(string typeId)
        {
            TrainingSearchDataHandler TSDH = new TrainingSearchDataHandler();
            try
            {
                grdTrainingDetails.DataSource = TSDH.getTrainingByType(typeId);
                grdTrainingDetails.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TSDH = null;
            }
        }


        //public void getTrainingProgram()
        //{
        //    TrainingSearchDataHandler TSDH = new TrainingSearchDataHandler();
        //    DataTable table = new DataTable();

        //    try
        //    {
        //        table = TSDH.getTrainingProgrms();
        //        ddlTProgram.Items.Add(new ListItem("", ""));

        //        for (int i = 0; i < table.Rows.Count; i++)
        //        {
        //            string text = table.Rows[i]["PROGRAM_NAME"].ToString();
        //            string value = table.Rows[i]["PROGRAM_ID"].ToString();
        //            ddlTProgram.Items.Add(new ListItem(text, value));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonVariables.MESSAGE_TEXT = ex.Message;
        //        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
        //    }
        //    finally
        //    {
        //        TSDH = null;
        //        table.Dispose();
        //    }
        //}

        //public void getTrainingType()
        //{
        //    TrainingSearchDataHandler TSDH = new TrainingSearchDataHandler();
        //    DataTable table = new DataTable();

        //    try
        //    {
        //        table = TSDH.getTrainingType();
        //        ddlType.Items.Add(new ListItem("", ""));

        //        for (int i = 0; i < table.Rows.Count; i++)
        //        {
        //            string text = table.Rows[i]["TYPE_NAME"].ToString();
        //            string value = table.Rows[i]["TRAINING_TYPE_ID"].ToString();
        //            ddlType.Items.Add(new ListItem(text, value));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonVariables.MESSAGE_TEXT = ex.Message;
        //        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
        //    }
        //    finally
        //    {
        //        TSDH = null;
        //        table.Dispose();
        //    }
        //}


    }
}