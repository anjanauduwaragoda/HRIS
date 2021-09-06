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
    public partial class WebFrmGoalGroups : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmGoalGroups : Page_Load"); 
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
                fillStatus();
                LoadGoalGroupGrid();
            }
        }

        private void fillStatus()
        {
            log.Debug("fillStatus()");

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
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");
            string goalGroupName = txtName.Text;
            string description = txtDescription.Text;
            string status = ddlStatus.SelectedItem.Value.ToString();
            string addedUserId = Session["KeyUSER_ID"].ToString();
            string ggNameUpper = "";



            GoalGroupDataHandler goalGroup = new GoalGroupDataHandler();
            UtilsDataHandler utilsDataHandler = new UtilsDataHandler();

            ggNameUpper = goalGroupName.Replace(" ", "").ToUpper().Trim();

            try
            {
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    //Boolean nameIsExsists = goalGroup.CheckGoalNameExsistance(ggNameUpper);
                    Boolean nameIsExsists = utilsDataHandler.isDuplicateExist(goalGroupName, "GROUP_NAME", "GOAL_GROUP");

                    if (nameIsExsists)
                    {
                        CommonVariables.MESSAGE_TEXT = "Goal group name already exist";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                    else
                    {
                        goalGroup.Insert(goalGroupName, description, status, addedUserId);
                        CommonVariables.MESSAGE_TEXT = "Record(s) successfully saved.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        clearFrom();
                        LoadGoalGroupGrid();
                    }
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string goalGroupId = GoalGroupGrid.SelectedRow.Cells[0].Text;
                    bool isUsed = false;
                    if (status == Constants.STATUS_INACTIVE_VALUE)
                    {
                        isUsed = goalGroup.IsUsed(goalGroupId);
                    }


                    if (!isUsed)
                    {
                        //Boolean nameIsExsists = goalGroup.CheckGoalNameExsistance(ggNameUpper, goalGroupId);
                        Boolean nameIsExsists = utilsDataHandler.isDuplicateExist(goalGroupName, "GROUP_NAME", "GOAL_GROUP", goalGroupId, "GOAL_GROUP_ID");
                        if (nameIsExsists)
                        {
                            CommonVariables.MESSAGE_TEXT = "Goal group name already exist";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }
                        else
                        {
                            goalGroup.Update(goalGroupId, goalGroupName, description, status, addedUserId);
                            CommonVariables.MESSAGE_TEXT = "Record(s)successfully updated.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            clearFrom();
                            LoadGoalGroupGrid();
                        }
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "Goal group is used in employee goals";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                goalGroup = null;
                utilsDataHandler = null;
            }


            //clearFrom();
            //LoadGoalGroupGrid();

        }

        protected void LoadGoalGroupGrid()
        {
            log.Debug("LoadGoalGroupGrid()");

            GoalGroupDataHandler goalGroupDataHandler = new GoalGroupDataHandler();
            DataTable dataTable = new DataTable();

            try
            {
                dataTable = goalGroupDataHandler.Populate();
                GoalGroupGrid.DataSource = dataTable;
                GoalGroupGrid.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                goalGroupDataHandler = null;
                dataTable.Dispose();
            }
        }

        private void clearFrom()
        {
            log.Debug("clearFrom()");

            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            Utils.clearControls(true, txtName, txtDescription, ddlStatus);

        }

        protected void GoalGroupGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            log.Debug("GoalGroupGrid_PageIndexChanging()");
            GoalGroupGrid.PageIndex = e.NewPageIndex;
            LoadGoalGroupGrid();
            clearFrom();
            Utility.Errorhandler.ClearError(lblErrorMsg);
        }

        protected void GoalGroupGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("GoalGroupGrid_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.GoalGroupGrid, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void GoalGroupGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("GoalGroupGrid_SelectedIndexChanged()");

            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblErrorMsg);
            GoalGroupDataHandler goalGroup = new GoalGroupDataHandler();
            DataRow dataRow = null;

            try
            {
                string goalGroupId = GoalGroupGrid.SelectedRow.Cells[0].Text;
                dataRow = goalGroup.populate(goalGroupId);

                if (dataRow != null)
                {
                    txtName.Text = dataRow["GROUP_NAME"].ToString().Trim();
                    txtDescription.Text = dataRow["DESCRIPTION"].ToString().Trim();
                    string status = dataRow["STATUS_CODE"].ToString().Trim();
                    ddlStatus.SelectedValue = status;
                    

                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                goalGroup = null;
                dataRow = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");

            clearFrom();
            Utility.Errorhandler.ClearError(lblErrorMsg);
        }

    }
}