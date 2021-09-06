using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using Common;
using DataHandler.TrainingAndDevelopment;
using System.Data;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainerSearch : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Utility.Errorhandler.ClearError(lblMessage);

                if (!IsPostBack)
                {
                    sIPAddress = Request.UserHostAddress;
                    log.Debug("IP:" + sIPAddress + "WebFrmTrainerSearch : Page_Load");

                }
            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainerSearch | Page_Load() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            { 
            
            }
        }

        protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainerSearch | imgbtnSearch_Click()");
                LoadGrid();
            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainerSearch | imgbtnSearch_Click() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainerSearch | btnSelect_Click()");
            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainerSearch | btnSelect_Click() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void grdvTrainers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainerSearch | grdvTrainers_RowDataBound()");

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("OnClick", this.ClientScript.GetPostBackEventReference(this.grdvTrainers, "select$" + e.Row.RowIndex.ToString()));
                    //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.grdvTrainers, "Select$" + e.Row.RowIndex);
                    e.Row.Attributes.Add("Style", "cursor:pointer");
                }

                
            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainerSearch | grdvTrainers_RowDataBound() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void grdvTrainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainerSearch | grdvTrainers_SelectedIndexChanged()");
                txtTrId.Text = HttpUtility.HtmlDecode(grdvTrainers.Rows[grdvTrainers.SelectedIndex].Cells[0].Text).Trim();
                hfTrainerName.Value = HttpUtility.HtmlDecode(grdvTrainers.Rows[grdvTrainers.SelectedIndex].Cells[1].Text).Trim();
                btnSelect.Visible = true;
            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainerSearch | grdvTrainers_SelectedIndexChanged() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void grdvTrainers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainerSearch | grdvTrainers_PageIndexChanging()");
                grdvTrainers.PageIndex = e.NewPageIndex;
                LoadGrid();
            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainerSearch | grdvTrainers_PageIndexChanging() | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        void LoadGrid()
        {
            TrainerInformationDataHandler TIDH = new TrainerInformationDataHandler();
            DataTable dtTrainers = new DataTable();
            Boolean IsExternalTrainer = false;
            string Name = String.Empty;
            string NIC = String.Empty;
            string MobileNumber = String.Empty;
            try
            {
                log.Debug("WebFrmTrainerSearch | LoadGrid()");

                Name = txtName.Text.Trim();
                NIC = txtNIC.Text.Trim();
                MobileNumber = txtContactNumber.Text.Trim();

                if (chkIsExternal.Checked == true)
                {
                    IsExternalTrainer = true;
                }
                else
                {
                    IsExternalTrainer = false;
                }

                dtTrainers = TIDH.PopulateTrainers(Name, NIC, MobileNumber, IsExternalTrainer).Copy();

                grdvTrainers.DataSource = dtTrainers.Copy();
                grdvTrainers.DataBind();

            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainerSearch | LoadGrid() | " + ex.Message);
                throw ex;
            }
            finally
            {
                MobileNumber = String.Empty;
                NIC = String.Empty;
                Name = String.Empty;
                IsExternalTrainer = false;
                dtTrainers.Dispose();
                TIDH = null;
            }
        }
    }
}