using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using DataHandler.MetaData;
using GroupHRIS.Utility;

 
namespace GroupHRIS.MetaData
{
    public partial class webFrmLeaveScheme : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KeyLOGOUT_STS"].Equals("0"))
            {
                Response.Redirect("MainLogout.aspx", false);
            }
            if (!IsPostBack == true)
            {
                filLeaveScheme();

                LeaveTypeDataHandler leaveType = new LeaveTypeDataHandler();
                DataTable leaveTypeDataTable = new DataTable();
                leaveTypeDataTable = leaveType.populate();

                LeaveSchemeTypeDropDownList.Items.Insert(0, new ListItem("", ""));
                for (int i = 0; i < leaveTypeDataTable.Rows.Count; i++)
                {
                    LeaveSchemeTypeDropDownList.Items.Insert(i+1, new ListItem(leaveTypeDataTable.Rows[i][1].ToString(),leaveTypeDataTable.Rows[i][0].ToString()));
                }


                DataTable AddLeaveSchemeItem = new DataTable();

                AddLeaveSchemeItem.Columns.Add("Leave Scheme Type");
                AddLeaveSchemeItem.Columns.Add("Number of Days");
                AddLeaveSchemeItem.Columns.Add("Remarks");

                Session["AddLeaveSchemeItem"] = AddLeaveSchemeItem;
            }



        }

        private void filLeaveScheme()
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            LeaveSchemeDataHandler leaveScheme = new LeaveSchemeDataHandler();
            DataTable schLeaveScheme = new DataTable();
            try
            {
                schLeaveScheme = leaveScheme.getLeaveSchemes();
                for(int i = 0; i < schLeaveScheme.Rows.Count; i++)
                {
                    if (schLeaveScheme.Rows[i][2].ToString() == Constants.STATUS_ACTIVE_VALUE)
                    {
                        schLeaveScheme.Rows[i][2] = Constants.STATUS_ACTIVE_TAG;
                    }
                    else
                    {
                        schLeaveScheme.Rows[i][2] = Constants.STATUS_INACTIVE_TAG;
                    }
                }
                GridView1.DataSource = schLeaveScheme;
                Session["schLeaveScheme"] = schLeaveScheme;
                GridView1.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string mLeaveType = txtSchemeName.Text.ToString();
            string mStatus = ddlStatus.SelectedItem.Value.ToString();


            DataTable dataTable = new DataTable();
            dataTable = (DataTable)Session["AddLeaveSchemeItem"];
            if (dataTable.Rows.Count > 0)
            {
                try
                {
                    Errorhandler.ClearError(lblerror);
                    DataTable LeaveShemeItem = new DataTable();
                    LeaveShemeItem = (DataTable)Session["AddLeaveSchemeItem"];

                    if ((btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT) && (LeaveShemeItem.Rows.Count > 0))
                    {
                        LeaveSchemeDataHandler leaveScheme = new LeaveSchemeDataHandler();
                        Boolean State = leaveScheme.CheckPrevRecord(mLeaveType);
                        if (State == false)
                        {
                            leaveScheme.insert(mLeaveType, mStatus, LeaveShemeItem);
                            CommonVariables.MESSAGE_TEXT = "Record(s) saved successfully.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Record already exists", lblerror);
                        }
                    }
                    else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {
                        string mLeaveTypeID = GridView1.SelectedRow.Cells[0].Text;
                        LeaveSchemeDataHandler leaveScheme = new LeaveSchemeDataHandler();

                        DataTable dt = new DataTable();

                        dt = leaveScheme.CheckAssignEmployees(mLeaveTypeID);
                        if (dt.Rows.Count == 0)
                        {
                            leaveScheme.update(mLeaveTypeID, mLeaveType, mStatus, LeaveShemeItem);
                            CommonVariables.MESSAGE_TEXT = "Record(s) modified successfully.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        if (dt.Rows.Count > 0)
                        {
                            string Emps = "";
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Emps += dt.Rows[i][0].ToString() + "<br/>";
                            }

                            lblerror.Text = "Update Failed. Some Employees are Assign to Leave Scheme &lt" + txtSchemeName.Text.ToString() + "&gt";
                            //lblerror.Text += "Following Employees are Assign to Leave Scheme &lt" + txtSchemeName.Text.ToString() + "&gt <br/>";
                            //lblerror.Text += Emps;

                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, lblerror.Text, lblerror);
                        }
                    }
                    clear();
                    filLeaveScheme();
                }
                catch (Exception Ex)
                {
                    CommonVariables.MESSAGE_TEXT = Ex.Message;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                }
            }
            else
            {
                lblerror.Text = "NO Leave Scheme Type(s) for Leave Scheme < " + txtSchemeName.Text + " >";
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, lblerror.Text, lblerror);
            }
        }

        private void clear()
        {
            txtSchemeName.Text = "";
            ddlStatus.SelectedIndex = -1;
            LeaveSchemeItemGridView.DataSource = null;
            LeaveSchemeItemGridView.DataBind();
            LeaveSchemeTypeDropDownList.SelectedValue = "";
            NumberOfDaysTextBox.Text = "";
            RemarksTextBox.Text = "";
            //btnCancel.Visible = false;
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Errorhandler.ClearError(lblerror);
            LeaveSchemeDataHandler leaveScheme = new LeaveSchemeDataHandler();
            DataRow dataRow = null;
            try
            {
                string mLeaveSchemeID = GridView1.SelectedRow.Cells[0].Text;
                dataRow = leaveScheme.populate(mLeaveSchemeID);

                if (dataRow != null)
                {
                    ddlStatus.SelectedValue = dataRow["STATUS_CODE"].ToString().Trim();
                    if (ddlStatus.SelectedValue == "0")
                    {
                        btnSave.Enabled = false;
                    }
                    else
                    {
                        btnSave.Enabled = true;
                    }
                    txtSchemeName.Text = dataRow["LEAVE_SCHEM_NAME"].ToString().Trim();
                    btnCancel.Visible = true;
                }

                LeaveSchemeItemGridView.DataSource = leaveScheme.populateSchemeItems(mLeaveSchemeID);
                LeaveSchemeItemGridView.DataBind();
                Session["AddLeaveSchemeItem"] = leaveScheme.populateSchemeItems(mLeaveSchemeID);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
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
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            int x = LeaveSchemeTypeDropDownList.Items.Count;
            Boolean ExistStatus = false;
            for (int i = 1; i < LeaveSchemeTypeDropDownList.Items.Count; i++)
            {
                for (int j = 0; j < LeaveSchemeItemGridView.Rows.Count; j++)
                {
                    if (LeaveSchemeItemGridView.Rows[j].Cells[0].Text == LeaveSchemeTypeDropDownList.SelectedValue)
                    {
                        ExistStatus = true;
                    }
                }
            }

            if (ExistStatus == false)
            {
                if (LeaveSchemeTypeDropDownList.SelectedItem.Text != "")
                {
                    try
                    {
                        Errorhandler.ClearError(lblerror);
                        DataTable dataTable = new DataTable();
                        dataTable = (DataTable)Session["AddLeaveSchemeItem"];


                        DataTable dtCloned = dataTable.Clone();
                        dtCloned.Columns["Number of Days"].DataType = typeof(string);
                        foreach (DataRow row in dataTable.Rows)
                        {
                            dtCloned.ImportRow(row);
                        }


                        DataRow dataRow = dtCloned.NewRow();
                        //dataRow["Leave Scheme Type"] = LeaveSchemeTypeDropDownList.SelectedItem.Text;
                        dataRow["Leave Type"] = LeaveSchemeTypeDropDownList.SelectedItem.Text;
                        //Leave Type
                        dataRow["Number of Days"] = NumberOfDaysTextBox.Text.Trim();
                        dataRow["Remarks"] = RemarksTextBox.Text;

                        //DataRow[] dr = dtCloned.Select("[Leave Scheme Type]='" + LeaveSchemeTypeDropDownList.SelectedItem.Text + "'");
                        DataRow[] dr = dtCloned.Select("[Leave Type] = '" + LeaveSchemeTypeDropDownList.SelectedItem.Text + "'");
                        if (dr.Length == 0)
                        {
                            dtCloned.Rows.Add(dataRow);
                        }
                        else
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Leave Scheme Type Already Exists <" + LeaveSchemeTypeDropDownList.SelectedItem.Text + ">", lblerror);
                        }

                        LeaveSchemeItemGridView.DataSource = dtCloned;
                        LeaveSchemeItemGridView.DataBind();
                        Session["AddLeaveSchemeItem"] = dtCloned;

                    }
                    catch (Exception Ex)
                    {
                        CommonVariables.MESSAGE_TEXT = Ex.Message;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                }
            }

            LeaveSchemeTypeDropDownList.SelectedIndex = 0;
            RemarksTextBox.Text = "";
            NumberOfDaysTextBox.Text = "";

        }

        protected void LeaveSchemeItemGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.LeaveSchemeItemGridView, "Select$" + e.Row.RowIndex.ToString()));
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void LeaveSchemeItemGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable = (DataTable)Session["AddLeaveSchemeItem"];
            dataTable.Rows.RemoveAt(e.NewSelectedIndex);

            LeaveSchemeItemGridView.DataSource = dataTable;
            LeaveSchemeItemGridView.DataBind();
            Session["AddLeaveSchemeItem"] = dataTable;

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            
            Errorhandler.ClearError(lblerror);
            //btnCancel.Visible = false;
            txtSchemeName.Text = null;
            ddlStatus.SelectedValue = "";
            LeaveSchemeTypeDropDownList.SelectedValue = "";
            NumberOfDaysTextBox.Text = null;
            RemarksTextBox.Text = null;
            DataTable dt = new DataTable();
             dt=(DataTable)Session["AddLeaveSchemeItem"];
             dt.Rows.Clear();
             Session["AddLeaveSchemeItem"] = dt;
            LeaveSchemeItemGridView.DataSource = null;
            LeaveSchemeItemGridView.DataBind();
            btnSave.Text = "Save";
            btnSave.Enabled = true;
        }

        protected void LeaveSchemeItemGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                LeaveSchemeItemGridView.DataSource = (DataTable)Session["AddLeaveSchemeItem"];
                LeaveSchemeItemGridView.PageIndex = e.NewPageIndex;
                LeaveSchemeItemGridView.DataBind();
            }
            catch { }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.DataSource = (DataTable)Session["schLeaveScheme"];
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }
    }
}