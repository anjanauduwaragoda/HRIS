using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using NLog;
using DataHandler.TrainingAndDevelopment;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmEvaluationSearch : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmEvaluationSearch : Page_Load");

            filterAllEvaluation();
        }

        protected void grdProEvalData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdProEvalData, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void grdProEvalData_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("grdTrainingDetails_SelectedIndexChanged()");

            try
            {
                int SelectedIndex = grdProEvalData.SelectedIndex;
                btnSelect.Visible = true;
                lblEvaluationId.Text = grdProEvalData.SelectedRow.Cells[0].Text;
                hfEvalId.Value = grdProEvalData.SelectedRow.Cells[0].Text;
                lblEvalName.Text = grdProEvalData.SelectedRow.Cells[3].Text;
                hfProgram.Value = grdProEvalData.SelectedRow.Cells[3].Text; ;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        public void filterAllEvaluation()
        {
            ProgramEvaluationSearchDataHandler PES = new ProgramEvaluationSearchDataHandler();

            try
            {
                grdProEvalData.DataSource = PES.getAllPrograms();
                grdProEvalData.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                PES = null;
            }
        }

    }
}