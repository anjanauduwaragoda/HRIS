using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.TrainingAndDevelopment;
using Common;
using GroupHRIS.Utility;
using System.Data;
using DataHandler.Utility;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainerCompetency : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getTrainerCompetency();
                fillStatus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string logUser = Session["KeyUSER_ID"].ToString();
            string competencyArea = txtCompetencyArea.Text;
            string description = txtDescription.Text;
            string status = ddlStatus.SelectedValue;

            TrainerCompetencyAreaDataHandler TCAH = new TrainerCompetencyAreaDataHandler();
            UtilsDataHandler UDH = new UtilsDataHandler();

            try
            {
                Errorhandler.ClearError(lblMessage);

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    bool isExist = UDH.isDuplicateExist(competencyArea, "NAME", "TRAINER_COMPETENCY_AREA");
                    if (isExist)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Competency Area already exist.", lblMessage);
                        return;
                    }

                    Boolean isSuccess = TCAH.Insert(competencyArea, description, status, logUser);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblMessage);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string competancyId = hfglId.Value.ToString();
                    bool isExist = UDH.isDuplicateExist(competencyArea, "NAME", "TRAINER_COMPETENCY_AREA", competancyId, "COMPETENCY_ID");
                    if (isExist)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Competency Area already exist.", lblMessage);
                        return;
                    }

                    Boolean isSuccess = TCAH.Update(competancyId, competencyArea, description, status, logUser);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
                }
                getTrainerCompetency();
                clear();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
            Errorhandler.ClearError(lblMessage);
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
        }

        protected void grdTrainerCompetency_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdTrainerCompetency, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void grdTrainerCompetency_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            DataTable dt = new DataTable();

            try
            {
                int SelectedIndex = grdTrainerCompetency.SelectedIndex;
                hfglId.Value = Server.HtmlDecode(grdTrainerCompetency.Rows[SelectedIndex].Cells[0].Text.ToString());
                txtCompetencyArea.Text = Server.HtmlDecode(grdTrainerCompetency.Rows[SelectedIndex].Cells[1].Text.ToString());
                txtDescription.Text = Server.HtmlDecode(grdTrainerCompetency.Rows[SelectedIndex].Cells[2].Text.ToString());
                string status = Server.HtmlDecode(grdTrainerCompetency.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdTrainerCompetency_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTrainerCompetency.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);
                getTrainerCompetency();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }


        public void getTrainerCompetency()
        {
            TrainerCompetencyAreaDataHandler TCAD = new TrainerCompetencyAreaDataHandler();

            try
            {
                grdTrainerCompetency.DataSource = TCAD.getAllCompetencies();
                grdTrainerCompetency.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TCAD = null;
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

        public void clear()
        {
            txtCompetencyArea.Text = "";
            txtDescription.Text = "";
            ddlStatus.SelectedValue = "";
            hfglId.Value = "";
        }

    }
}