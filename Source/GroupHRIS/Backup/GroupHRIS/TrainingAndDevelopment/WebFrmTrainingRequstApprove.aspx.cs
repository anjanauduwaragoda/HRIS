using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.TrainingAndDevelopment;
using Common;
using NLog;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainingRequstApprove : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmTrainingRequstApprove : Page_Load");

            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);

            if (!IsPostBack)
            {
                fillYear();
                string year = ddlYear.SelectedValue;
                readRecomendCount(year, KeyEMPLOYEE_ID);
                readApprovedCount(year, KeyEMPLOYEE_ID);
                fillTrainingRequestGrid(year, KeyEMPLOYEE_ID);
            }
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string year = ddlYear.SelectedValue;
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                
                fillTrainingRequestGrid(year,KeyEMPLOYEE_ID);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }


        protected void grdTrainingRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTrainingRequest.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);
                string year = ddlYear.SelectedValue;
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);

                fillTrainingRequestGrid(year, KeyEMPLOYEE_ID);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdTrainingRequest_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdTrainingRequest, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void lbRecommend_Click(object sender, EventArgs e)
        {
            TrainingRequestApproveDataHandler TADH = new TrainingRequestApproveDataHandler();
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            string year = ddlYear.SelectedValue;

            try
            {
                grdTrainingRequest.DataSource = TADH.getRecommendRequest(year, KeyEMPLOYEE_ID);
                grdTrainingRequest.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TADH = null;
            }
        }

        protected void lbApprove_Click(object sender, EventArgs e)
        {
            TrainingRequestApproveDataHandler TADH = new TrainingRequestApproveDataHandler();
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            string year = ddlYear.SelectedValue;

            try
            {
                grdTrainingRequest.DataSource = TADH.getApproveRequest(year, KeyEMPLOYEE_ID);
                grdTrainingRequest.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TADH = null;
            }
        }

        //protected void grdTrainingRequest_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    int SelectedIndex = grdTrainingRequest.SelectedIndex;

        //    hfReqId.Value = Server.HtmlDecode(grdTrainingRequest.Rows[SelectedIndex].Cells[0].Text.ToString());
        //    hfStatus.Value = Server.HtmlDecode(grdTrainingRequest.Rows[SelectedIndex].Cells[4].Text.ToString());
        //}



        public void fillTrainingRequestGrid(string year,string empId)
        {
            TrainingRequestApproveDataHandler TADH = new TrainingRequestApproveDataHandler();

            try
            {
                grdTrainingRequest.DataSource = TADH.getTrainingAllRequest(year, empId);
                grdTrainingRequest.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TADH = null;
            }
        }

        public void fillYear()
        {
            try
            {
                ddlYear.Items.Clear();
                string currentYear = (CommonUtils.currentFinancialYear());
                int dt = Int32.Parse(currentYear);

                for (int i = 0; i >= -5; i--)
                {
                    // Now just add an entry that's the current year plus the counter
                    ddlYear.Items.Add((dt + i).ToString());
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        public void readRecomendCount(string year, string empId)
        {
            TrainingRequestApproveDataHandler TADH = new TrainingRequestApproveDataHandler();

            try
            {
                lblrPending.Text = TADH.getRecommendCount(empId, year).ToString();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TADH = null;
            }
        }

        public void readApprovedCount(string year, string empId)
        {
            TrainingRequestApproveDataHandler TADH = new TrainingRequestApproveDataHandler();

            try
            {
                lblapproveview.Text = TADH.getApprovedCount(empId, year).ToString();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TADH = null;
            }
        }

        protected void grdTrainingRequest_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            log.Debug("WebFrmTrainingRequstApprove : grdTrainingRequest_RowCommand");

            //string sToDoText = "";
            //string sRequestId = "";

            try
            {
                Int32 index = Convert.ToInt32(e.CommandArgument);

                GridViewRow selectedRow = grdTrainingRequest.Rows[index];

                //sRequestId = selectedRow.Cells[0].Text.ToString().Trim();
                //sToDoText  = selectedRow.Cells[4].Text.ToString().Trim();

                hfReqId.Value  = "";
                hfStatus.Value = "";

                hfReqId.Value  = Server.HtmlDecode(selectedRow.Cells[0].Text.ToString().Trim());
                hfStatus.Value = Server.HtmlDecode(selectedRow.Cells[4].Text.ToString().Trim());

                Server.Transfer("~/TrainingAndDevelopment/WebFrmTraingRequest.aspx");

            }
            catch (Exception ex)
            {                
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }

        }


    }
}