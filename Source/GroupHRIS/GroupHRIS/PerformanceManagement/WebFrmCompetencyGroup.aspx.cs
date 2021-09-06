using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using GroupHRIS.Utility;
using DataHandler;
using DataHandler.PerformanceManagement;
using System.Data;
using NLog;


namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmCompetencyGroup : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        
        #region Events
        
        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmCompetencyGroup : Page_Load");


            if (Session["KeyLOGOUT_STS"].Equals("0"))
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            
            if (!IsPostBack)
            {
                filStatus();
                fillCompetencyGroups();
               
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            Utility.Errorhandler.ClearError(lblMessage);

            CompetencyGroupDataHandler competencyGroupDataHandler = new CompetencyGroupDataHandler();
            CompetencyBankDataHandler competencyBankDataHandler  = new CompetencyBankDataHandler();

            string addedBy = "";

            string competencyGroupName = "";
            string description = "";
            string statusCode = "";
            string competencyGroupNameUpper = "";

            try
            {
                competencyGroupName = txtGroupName.Text.Trim();
                description = txtDescription.Text.Trim();        
                statusCode = ddlStatus.SelectedItem.Value.ToString();

                competencyGroupNameUpper = competencyGroupName.Replace(" ","").ToUpper().Trim();

                // check for duplicates
                // at saving
                if ((btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT) && (competencyGroupDataHandler.isCompetancyGroupExist(competencyGroupNameUpper))) { Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Competency Group Already Exist", lblMessage); return; }
                // at updating
                if ((btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT) && (competencyGroupDataHandler.isCompetancyGroupExist(competencyGroupNameUpper, hfCompetencyGroupId.Value.ToString().Trim()))) { Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Competency Group Already Exist", lblMessage); return; }

                if ((btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT) && (statusCode.Trim() == Constants.CON_INACTIVE_STATUS))
                {
                    if (competencyBankDataHandler.isCompetenciesExistForCompetencyGroup(hfCompetencyGroupId.Value.ToString().Trim()))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Not allowed to make inactive. Active competencies exist.", lblMessage); 
                        return;
                    }
                }


                if (Session["KeyUSER_ID"] != null)
                {
                    addedBy = Session["KeyUSER_ID"].ToString();
                }


                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Insert");
                    Boolean isInserted = competencyGroupDataHandler.Insert(competencyGroupName, description, statusCode, addedBy);

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                    if (isInserted)
                    {
                        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('Designation is saved ..')", true); 
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblMessage);
                    }

                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> update");
                    string competencyGroupId = "";

                    if (hfCompetencyGroupId.Value.ToString().Trim() != "") { competencyGroupId = hfCompetencyGroupId.Value.ToString().Trim(); }

                    if (competencyGroupId != "")
                    {
                        Boolean isUpdated = competencyGroupDataHandler.Update(competencyGroupId, competencyGroupName, description, statusCode, addedBy);

                        if (isUpdated)
                        {
                            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('Competency Group is updated ..')", true); 
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
                        }

                    }

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                }
                clear();
                fillCompetencyGroups();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                competencyGroupDataHandler = null;
                competencyBankDataHandler = null;                
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            log.Debug("btnCancel_Click()");
            clear();
            Utility.Errorhandler.ClearError(lblMessage);
        }

        protected void gvCGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblMessage);
            CompetencyGroupDataHandler competencyGroupDataHandler = new CompetencyGroupDataHandler();
            DataRow dataRow = null;
            hfCompetencyGroupId.Value = String.Empty;

            try
            {
                log.Debug("gvCGroups_SelectedIndexChanged()");
                string cGroupId = gvCGroups.SelectedRow.Cells[0].Text;
                
                dataRow = competencyGroupDataHandler.populate(cGroupId);

                if (dataRow != null)
                {
                    hfCompetencyGroupId.Value = dataRow["COMPETENCY_GROUP_ID"].ToString().Trim();
                    txtGroupName.Text = dataRow["COMPETENCY_GROUP_NAME"].ToString().Trim();
                    txtDescription.Text = dataRow["DESCRIPTION"].ToString().Trim();
                    ddlStatus.SelectedValue = dataRow["STATUS_CODE"].ToString().Trim();                    
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                competencyGroupDataHandler = null;
                dataRow = null;
            }
        }

        protected void gvCGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvCGroups, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void gvCGroups_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvDesignations_PageIndexChanging()");

            try
            {
                gvCGroups.PageIndex = e.NewPageIndex;
                fillCompetencyGroups();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }
        
        #endregion

        #region Methods

        private void filStatus()
        {
            ListItem listItemBlank = new ListItem();
            listItemBlank.Text = "";
            listItemBlank.Value = "";
            ddlStatus.Items.Add(listItemBlank);

            ListItem listItemActive = new ListItem();
            listItemActive.Text = Constants.STATUS_ACTIVE_TAG;
            listItemActive.Value = Constants.STATUS_ACTIVE_VALUE;
            ddlStatus.Items.Add(listItemActive);

            ListItem listItemInActive = new ListItem();
            listItemInActive.Text = Constants.STATUS_INACTIVE_TAG;
            listItemInActive.Value = Constants.STATUS_INACTIVE_VALUE;
            ddlStatus.Items.Add(listItemInActive);
        
        }

        private void fillCompetencyGroups()
        {
            CompetencyGroupDataHandler competencyGroupDataHandler = new CompetencyGroupDataHandler();
            DataTable groups = new DataTable();
            try
            {

                groups = competencyGroupDataHandler.populate();
                gvCGroups.DataSource = groups;
                gvCGroups.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                competencyGroupDataHandler = null;
                groups.Dispose();
            }
        }

        private void clear()
        {
            txtGroupName.Text = "";
            txtDescription.Text = "";
            ddlStatus.SelectedIndex = 0;
            hfCompetencyGroupId.Value = String.Empty;
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;

        }

        #endregion

    }
}