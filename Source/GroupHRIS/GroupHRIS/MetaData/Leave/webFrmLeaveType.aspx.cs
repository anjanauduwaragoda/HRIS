using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using DataHandler.MetaData;
 

namespace GroupHRIS.MetaData
{
    public partial class webFrmLeaveType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KeyLOGOUT_STS"].Equals("0"))
            {
                Response.Redirect("MainLogout.aspx", false);
            }
            if (!IsPostBack == true) 
            {
                filLeaveType();
                filStatus();
            }
        }
        private void filStatus()
        {
            ListItem listItem = new ListItem();
            listItem.Text = "";
            listItem.Value = "";
            ddlStatus.Items.Add(listItem);

            ListItem listItemActive = new ListItem();
            listItemActive.Text = Constants.STATUS_ACTIVE_TAG;
            listItemActive.Value = Constants.STATUS_ACTIVE_VALUE;

            ddlStatus.Items.Add(listItemActive);

            ListItem listItemInActive = new ListItem();
            listItemInActive.Text = Constants.STATUS_INACTIVE_TAG;
            listItemInActive.Value = Constants.STATUS_INACTIVE_VALUE;

            ddlStatus.Items.Add(listItemInActive);
        }
        private void filLeaveType()
        {

            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            LeaveTypeDataHandler leaveType = new LeaveTypeDataHandler();
            DataTable schLeaveType = new DataTable();
            try
            {
                schLeaveType = leaveType.populate();
                GridView1.DataSource = schLeaveType;
                GridView1.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT  = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            filLeaveType();
            clear();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.GridView1, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblerror);
            //txtLeaveID.Enabled = false;
            txtLeaveID.BorderStyle = BorderStyle.None;
            txtLeaveID.ReadOnly = true;
            LeaveTypeDataHandler leaveType = new LeaveTypeDataHandler();
            DataRow dataRow = null;
            try
            {
                string mLeaveTypeID = GridView1.SelectedRow.Cells[0].Text;
                dataRow = leaveType.populate(mLeaveTypeID);

                if (dataRow != null)
                {
                    ddlStatus.SelectedValue = dataRow["STATUS_CODE"].ToString().Trim();
                    txtLeaveType.Text = dataRow["LEAVE_TYPE_NAME"].ToString().Trim();
                    txtLeaveID.Text = dataRow["LEAVE_TYPE_ID"].ToString().Trim();
                    ddlCategory.SelectedValue = dataRow["CATEGORY"].ToString().Trim();
                    
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Boolean ErrorFlag = false;
            string mLeaveType = txtLeaveType.Text.ToString();
            string mStatus = ddlStatus.SelectedItem.Value.ToString();
            string mLeaveId = txtLeaveID.Text.ToString();
            string mLeaveCategory = ddlCategory.SelectedItem.Value.ToUpper().ToString();
            LeaveTypeDataHandler leaveType = new LeaveTypeDataHandler();
            DataTable dtLeaveType = null;
            try
            {
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    dtLeaveType = leaveType.populateByName(mLeaveType);
                    if (dtLeaveType.Rows.Count > 0)
                    {
                        CommonVariables.MESSAGE_TEXT = "Record already exist with Leave Type : < " + mLeaveType + " >";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        dtLeaveType = leaveType.populateByID(mLeaveId);
                        if (dtLeaveType.Rows.Count > 0)
                        {
                            CommonVariables.MESSAGE_TEXT = "Record already exist with Leave Type ID : < " + mLeaveId + " >";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);

                        }
                        else
                        {
                            if (mLeaveCategory == "A" || mLeaveCategory == "M" || mLeaveCategory == "C")
                            {
                                dtLeaveType = leaveType.populateByCategory(mLeaveCategory);
                                if (dtLeaveType.Rows.Count > 0)
                                {
                                    ErrorFlag = true;
                                }
                            }

                            if (ErrorFlag == false)
                            {
                                leaveType.insert(mLeaveId, mLeaveType, mStatus, mLeaveCategory);
                                CommonVariables.MESSAGE_TEXT = "Record(s) saved successfully.";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                                clear();
                                filLeaveType();
                            }
                            else
                            {
                                CommonVariables.MESSAGE_TEXT = "Record already exist with Leave Cateogory : < " + ddlCategory.SelectedItem + " >";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            }
                        }
                    }
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string mLeaveTypeID = GridView1.SelectedRow.Cells[0].Text;
                    dtLeaveType = leaveType.populateByNameID(mLeaveType,mLeaveTypeID);
                    if (dtLeaveType.Rows.Count > 0)
                    {
                        CommonVariables.MESSAGE_TEXT = "Record already exist with : < " + mLeaveType + " >";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);

                    }
                    else
                    {
                        if (mLeaveCategory == "A" || mLeaveCategory == "M" || mLeaveCategory == "C") 
                        {
                            dtLeaveType = leaveType.populateByCategoryID(mLeaveCategory, mLeaveTypeID);
                            if (dtLeaveType.Rows.Count > 0)
                            {
                                ErrorFlag = true;
                            }
                        }

                        if (ErrorFlag == false)
                        {
                            leaveType.update(mLeaveTypeID, mLeaveType, mStatus, mLeaveCategory);
                            CommonVariables.MESSAGE_TEXT = "Record(s) modified successfully.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                            clear();
                            filLeaveType();
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Record already exist with Leave Cateogory : < " + ddlCategory.SelectedItem + " >";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                leaveType = null;
            }
        }

        private void clear() 
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            txtLeaveType.Text = "";
            txtLeaveID.Text = "";
            ddlStatus.SelectedIndex = -1;
            //txtLeaveID.Enabled = true;
            ddlCategory.SelectedIndex = -1;
            txtLeaveID.BorderStyle = BorderStyle.NotSet;
            txtLeaveID.ReadOnly = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
            Utility.Errorhandler.ClearError(lblerror);
        }
    }
}