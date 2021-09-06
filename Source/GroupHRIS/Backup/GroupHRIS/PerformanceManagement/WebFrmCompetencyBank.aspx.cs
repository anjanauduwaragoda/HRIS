using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.PerformanceManagement;
using System.Data;
using GroupHRIS.Utility;
using DataHandler.Utility;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmCompetencyBank : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmCompetencyBank : Page_Load"); 
            try
            {
                if (Session["KeyLOGOUT_STS"] == null)
                {
                    Response.Redirect("MainLogout.aspx", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login/SessionExpior.aspx", false);
            }

            if (!IsPostBack)
            {
                fillStatusDropDown();
                fillCompetencyGroupDropDown();
                fillCompetencyGridView();
            }

        }

        #region Methodes

        private void fillStatusDropDown()
        {
            try
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
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void fillCompetencyGroupDropDown()
        {
            CompetencyBankDataHandler competencyBankDataHandler = new CompetencyBankDataHandler();
            //competencyBankDataHandler.getAllActiveCompetencyGroups();

            DataTable competencyGroupDataTable = competencyBankDataHandler.getAllActiveCompetencyGroups();

            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlCompetencyGroup.Items.Add(listItemBlank);

                if (competencyGroupDataTable.Rows.Count > 0)
                {
                    foreach (DataRow group in competencyGroupDataTable.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = group[1].ToString();
                        listItem.Value = group[0].ToString();
                        ddlCompetencyGroup.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                competencyBankDataHandler = null;
                competencyGroupDataTable.Dispose();
            }
        }

        private void initializeGridView()
        {
            DataTable blankDataTable = new DataTable();

            try
            {
                blankDataTable.Columns.Add("COMPETENCY_ID", typeof(String));
                blankDataTable.Columns.Add("NAME", typeof(String));
                blankDataTable.Columns.Add("COMPETENCY_GROUP_ID", typeof(String));
                blankDataTable.Columns.Add("COMPETENCY_GROUP", typeof(String));
                blankDataTable.Columns.Add("DESCRIPTION", typeof(String));
                blankDataTable.Columns.Add("STATUS_CODE", typeof(String));

                gvCompetencyBank.DataSource = blankDataTable;
                gvCompetencyBank.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                blankDataTable.Dispose();
            }
            
        }      

        private void fillCompetencyGridView()
        {
            CompetencyBankDataHandler competencyBankDataHandler = new CompetencyBankDataHandler();
            DataTable competencyDataTable = new DataTable();
            try
            {
                competencyDataTable = competencyBankDataHandler.getAllCompetencies();

                if (competencyDataTable.Rows.Count > 0)
                {
                    gvCompetencyBank.DataSource = competencyDataTable;
                    gvCompetencyBank.DataBind();
                }
                else
                {
                    // initializeGridView();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                competencyBankDataHandler = null;
                competencyDataTable.Dispose();
            }

        }

        private void clearControls()
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            Utils.clearControls(true,txtDescription, txtName, ddlCompetencyGroup, ddlStatus);
        }

        private void addInactiveCompetencyGroup(DropDownList ddlName, string itemText, string itemValue)
        {
            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = itemText;
                newItem.Value = itemValue;
                ddlName.Items.Add(newItem);
                ddlName.SelectedValue = itemValue;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }


        }
        
        #endregion

        #region Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");
            string name = txtName.Text.ToString().Trim();
            string description = txtDescription.Text.Trim(); ;
            string groupId = ddlCompetencyGroup.SelectedValue.ToString().Trim();
            string status = ddlStatus.SelectedValue.ToString().Trim();
            string userId = Session["KeyUSER_ID"].ToString();

            CompetencyBankDataHandler competencyBankDataHandler = new CompetencyBankDataHandler();
            UtilsDataHandler utilsDataHandler = new UtilsDataHandler();

            try
            {
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {

                    Boolean isNameExists = utilsDataHandler.isDuplicateExist(name, "COMPETENCY_NAME", "COMPETENCY_BANK");

                    if (isNameExists)
                    {

                        CommonVariables.MESSAGE_TEXT = "Competency name already exists ";

                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                    else
                    {
                        Boolean isInserted = competencyBankDataHandler.insert(name, description, groupId, status, userId);
                        if (isInserted)
                        {
                            CommonVariables.MESSAGE_TEXT = "Record(s) successfully saved.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Record(s) could not be inserted.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }

                        clearControls();
                    }
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string competencyId = hfCompetencyId.Value.ToString().Trim();
                    DataTable usedCompetencyAssessments = competencyBankDataHandler.getUsedCompetencyAssessments(competencyId);

                    bool isStatusUpdatable = false;
                    bool isAllUpdatable = false;

                    if (usedCompetencyAssessments.Rows.Count > 0)
                    {

                        foreach (DataRow assessmentToken in usedCompetencyAssessments.Rows)
                        {
                            isStatusUpdatable = true;
                            if (assessmentToken["STATUS_CODE"] != Constants.ASSESSNEMT_CEO_FINALIZED_STATUS && assessmentToken["STATUS_CODE"] != Constants.ASSESSNEMT_OBSOLETE_STATUS
                                && assessmentToken["STATUS_CODE"] != Constants.ASSESSNEMT_CLOSED_STATUS)
                            {
                                isStatusUpdatable = false;
                            }
                        }
                    }
                    else
                    {
                        isAllUpdatable = true;

                    }
                    Boolean isNameExists = utilsDataHandler.isDuplicateExist(name, "COMPETENCY_NAME", "COMPETENCY_BANK", competencyId, "COMPETENCY_ID");

                    if (isNameExists)
                    {

                        CommonVariables.MESSAGE_TEXT = "Competency name already exist";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                    else
                    {
                        CompetencyGroupDataHandler competencyGroupDataHandler = new CompetencyGroupDataHandler();
                        DataRow competencyGroupDetails = competencyGroupDataHandler.populate(groupId);

                        if (competencyGroupDetails["STATUS_CODE"].ToString() == Constants.STATUS_ACTIVE_VALUE)
                        {

                            Boolean isUpdated = false;
                            if (isAllUpdatable)
                            {
                                isUpdated = competencyBankDataHandler.update(competencyId, name, description, groupId, status, userId);
                                if (isUpdated)
                                {
                                    CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated.";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                }
                                else
                                {
                                    CommonVariables.MESSAGE_TEXT = "Record(s) could not be updated.";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                }
                            }
                            else
                            {
                                if (isStatusUpdatable || status == Constants.STATUS_ACTIVE_VALUE)
                                {
                                    isUpdated = competencyBankDataHandler.update(competencyId, status, userId);
                                    if (isUpdated)
                                    {
                                        CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated.";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                    }
                                    else
                                    {
                                        CommonVariables.MESSAGE_TEXT = "Record(s) could not be updated.";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                    }
                                }
                                else
                                {
                                    CommonVariables.MESSAGE_TEXT = "Cannot update.Ongoing assessments available";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                }
                            }
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Selected Competency Group is inactive";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }



                        clearControls();
                        hfCompetencyId.Value = null;
                    }




                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                competencyBankDataHandler = null;
                utilsDataHandler = null;
            }
            fillCompetencyGridView();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");
            try
            {
                Utility.Errorhandler.ClearError(lblErrorMsg);
                hfCompetencyId.Value = "";
                clearControls();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void gvCompetencyBank_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvCompetencyBank_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvCompetencyBank, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void gvCompetencyBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gvCompetencyBank_SelectedIndexChanged()");
            try
            {
                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                Utility.Errorhandler.ClearError(lblErrorMsg);

                int index = gvCompetencyBank.SelectedIndex;

                hfCompetencyId.Value = gvCompetencyBank.Rows[index].Cells[0].Text.ToString().Trim();
                txtName.Text = gvCompetencyBank.Rows[index].Cells[1].Text.ToString().Trim();
                //ddlCompetencyGroup.SelectedValue = gvCompetencyBank.Rows[index].Cells[2].Text.ToString();

                string competencyGroupId = gvCompetencyBank.Rows[index].Cells[2].Text.ToString();
                string competencyGroupName = gvCompetencyBank.Rows[index].Cells[3].Text.ToString();

                bool groupIsExist = Utils.isValueExistInDropDownList(competencyGroupId, ddlCompetencyGroup);

                if (groupIsExist)
                {
                    ddlCompetencyGroup.SelectedValue = gvCompetencyBank.Rows[index].Cells[2].Text.ToString();
                }
                else
                {
                    addInactiveCompetencyGroup(ddlCompetencyGroup, competencyGroupName, competencyGroupId);
                }


                //string description = gvCompetencyBank.Rows[index].Cells[4].Text.ToString().Trim();
                txtDescription.Text = gvCompetencyBank.Rows[index].Cells[4].Text.ToString().Trim().Replace("&nbsp;", "");

                if (gvCompetencyBank.Rows[index].Cells[5].Text.ToString() == Constants.STATUS_ACTIVE_TAG)
                {
                    ddlStatus.SelectedValue = Constants.STATUS_ACTIVE_VALUE;
                }
                else if (gvCompetencyBank.Rows[index].Cells[5].Text.ToString() == Constants.STATUS_INACTIVE_TAG)
                {
                    ddlStatus.SelectedValue = Constants.STATUS_INACTIVE_VALUE;
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void gvCompetencyBank_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvCompetencyBank_PageIndexChanging()");
            try
            {
                gvCompetencyBank.PageIndex = e.NewPageIndex;
                fillCompetencyGridView();
                Utility.Errorhandler.ClearError(lblErrorMsg);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }
        #endregion

        

        

        

    }
}