using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using DataHandler.EmployeeLeave;
using DataHandler.MetaData;
using Common;
using NLog;
using System.Net;
using GroupHRIS.Utility;

namespace GroupHRIS.EmployeeLeave
{
    public partial class webFrmEmployeeLeaveScheme : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "webFrmEmployeeLeaveScheme : Page_Load");

            if (!IsPostBack)
            {
                fillLeaveSchemes();
                lbtnClear.Visible = false;
            }
            else
            {
                Utility.Errorhandler.ClearError(lblMessage);
                Utility.Errorhandler.ClearError(lblSchemeName);
                lbtnClear.Visible = false;

                //if ((hfEmpId.Value.Trim() == "") || (hfEmpId.Value.Trim() != txtEmploeeId.Text.Trim()))
                //{
                //    hfEmpId.Value = txtEmploeeId.Text.Trim();
                //    log.Debug("fillLeaveSchemes()");
                //    fillLeaveSchemes(txtEmploeeId.Text.Trim());

                //    gvScheme.DataSource = null;
                //    gvScheme.DataBind();

                //    ddlLeaveScheme.SelectedIndex = 0;

                //    ddlStatus.SelectedIndex = 0;

                //    txtRemarks.Text = "";
                    
                //    gvSchemeDetail.DataSource = null;
                //    gvSchemeDetail.DataBind();
                //}

                if (hfCaller.Value == "txtEmploeeId")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtEmploeeId.Text = hfVal.Value;
                    }
                    if (txtEmploeeId.Text != "")
                    {
                        //Postback Methods
                        hfEmpId.Value = txtEmploeeId.Text.Trim();
                        log.Debug("fillLeaveSchemes()");
                        fillLeaveSchemes(txtEmploeeId.Text.Trim());

                        gvScheme.DataSource = null;
                        gvScheme.DataBind();

                        ddlLeaveScheme.SelectedIndex = 0;

                        ddlStatus.SelectedIndex = 0;

                        txtRemarks.Text = "";

                        gvSchemeDetail.DataSource = null;
                        gvSchemeDetail.DataBind();
                    }
                }

            }
        }

        protected void ddlLeaveScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("webFrmEmployeeLeaveScheme : ddlLeaveScheme_SelectedIndexChanged()");

            EmployeeLeaveSchemeDataHandler employeeLeaveSchemeDataHandler = new EmployeeLeaveSchemeDataHandler();
            try{
                Utility.Errorhandler.ClearError(lblMessage); 
                if (ddlLeaveScheme.SelectedValue.Trim() != "")
                {
                    fillLeaveSchemeItems(ddlLeaveScheme.SelectedValue.Trim(), gvScheme);
                    if((txtEmploeeId.Text.Trim() != "") && (employeeLeaveSchemeDataHandler.isSchemeExist(txtEmploeeId.Text.Trim(),ddlLeaveScheme.SelectedValue.Trim())))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, ddlLeaveScheme.SelectedItem.Text.Trim() + " is already exist and Active", lblMessage);
                    }
                    else if((txtEmploeeId.Text.Trim() != "") && (employeeLeaveSchemeDataHandler.isActiveSchemeExist(txtEmploeeId.Text.Trim())))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Active leave scheme is already exist.", lblMessage);
                    }

                    if (btnSave.Text.Trim().Equals(Constants.CON_UPDATE_BUTTON_TEXT))
                    {
                        btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                        Utility.Errorhandler.ClearError(lblMessage);
                        ddlStatus.SelectedIndex = 0;
                    }
                }


                gvSchemeDetail.DataSource = null;
                gvSchemeDetail.DataBind();
            }
            catch(Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
            finally
            {
                employeeLeaveSchemeDataHandler = null;
            }
        }

        protected void imgBtnView_Click(object sender, ImageClickEventArgs e)
        {
            log.Debug("webFrmEmployeeLeaveScheme : imgBtnView_Click()");


            if (txtEmploeeId.Text.Trim() != "")
            {
                fillLeaveSchemes(txtEmploeeId.Text.Trim());
            }
        }

        protected void gvLeaveSchemeHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("webFrmEmployeeLeaveScheme : gvLeaveSchemeHistory_RowDataBound()");

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvLeaveSchemeHistory, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }

            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
        }

        protected void gvLeaveSchemeHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("webFrmEmployeeLeaveScheme : gvLeaveSchemeHistory_SelectedIndexChanged()");

            try
            {
                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                Utility.Errorhandler.ClearError(lblMessage);
                hfLineNo.Value = "";
                
                string empLeaveSchemeId = gvLeaveSchemeHistory.SelectedRow.Cells[0].Text.ToString().Trim();
                string schemeName = gvLeaveSchemeHistory.SelectedRow.Cells[1].Text.ToString().Trim();

                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, schemeName.Trim(), lblSchemeName);

                fillLeaveSchemeItems(empLeaveSchemeId, gvSchemeDetail);
                ddlLeaveScheme.SelectedValue = empLeaveSchemeId.Trim();
                if (gvLeaveSchemeHistory.SelectedRow.Cells[2].Text.ToString().Trim().Equals("Active"))
                {
                    ddlStatus.SelectedValue = "1";
                }
                else if (gvLeaveSchemeHistory.SelectedRow.Cells[2].Text.ToString().Trim().Equals("Inactive"))
                {
                    ddlStatus.SelectedValue = "0";
                }
                txtRemarks.Text = WebUtility.HtmlDecode(gvLeaveSchemeHistory.SelectedRow.Cells[3].Text.ToString().Trim());
                hfLineNo.Value  = gvLeaveSchemeHistory.SelectedRow.Cells[4].Text.ToString().Trim();

                lbtnClear.Visible = true;
                
            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("webFrmEmployeeLeaveScheme : btnSave_Click()");

            EmployeeLeaveSchemeDataHandler employeeLeaveSchemeDataHandler = new EmployeeLeaveSchemeDataHandler();
            
            string employeeId       = "";
            string remarks          = "";
            string statusCode       = "";
            string leaveSchemeId    = "";

            if ((txtEmploeeId.Text.Trim() != "") && (employeeLeaveSchemeDataHandler.isSchemeExist(txtEmploeeId.Text.Trim(), ddlLeaveScheme.SelectedValue.Trim())) && (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT))
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, ddlLeaveScheme.SelectedItem.Text.Trim() + " is already exist and Active", lblMessage);
                return;
            }
            else if ((txtEmploeeId.Text.Trim() != "") && (employeeLeaveSchemeDataHandler.isActiveSchemeExist(txtEmploeeId.Text.Trim())) && (ddlStatus.SelectedValue == Constants.STATUS_ACTIVE_VALUE))
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING," Active leave scheme is already exist ", lblMessage);
                return;
            }

            try
            {
                employeeId      = txtEmploeeId.Text.Trim();
                leaveSchemeId   = ddlLeaveScheme.SelectedValue;
                statusCode      = ddlStatus.SelectedValue;
                remarks         = txtRemarks.Text.Trim();

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    log.Debug("webFrmEmployeeLeaveScheme : btnSave_Click() -> Insert");
                    Boolean isInserted = employeeLeaveSchemeDataHandler.Insert(employeeId,leaveSchemeId,statusCode,remarks);

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                    if (isInserted) 
                    { 
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS,"Leave scheme is saved", lblMessage);
                    }

                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    log.Debug("webFrmEmployeeLeaveScheme : btnSave_Click() -> update");
                    
                    string lineNo = "";

                    if (hfLineNo.Value.ToString().Trim() != "") lineNo = hfLineNo.Value.ToString().Trim();

                    if (lineNo != "")
                    {
                        Boolean isUpdated = employeeLeaveSchemeDataHandler.Update(employeeId, leaveSchemeId, statusCode, remarks, lineNo);

                        if (isUpdated)
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Leave scheme is changed", lblMessage);
                        }
                    }
                    else
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Some information is missing. Try again", lblMessage);
                    }
                }

                fillLeaveSchemes(txtEmploeeId.Text.Trim());

                gvSchemeDetail.DataSource = null;
                gvSchemeDetail.DataBind();
            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
            finally
            {
                employeeLeaveSchemeDataHandler = null;
               
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("webFrmEmployeeLeaveScheme : btnClear_Click()");

            clear();
        }

        #region Private Methods

        //------------------------------------------------------------------------------
        ///<summary>
        ///Load all leave schemes for a given employee
        ///</summary>
        //----------------------------------------------------------------------------------------

        private void fillLeaveSchemes(string employeeId)
        {
            log.Debug("webFrmEmployeeLeaveScheme : fillLeaveSchemes(string employeeId)");

            EmployeeLeaveSchemeDataHandler employeeLeaveSchemeDataHandler = new EmployeeLeaveSchemeDataHandler();
            DataTable employeeleaveSchemes = new DataTable();

            try
            {
                employeeleaveSchemes = employeeLeaveSchemeDataHandler.getEmployeeLeveSchemes(employeeId).Copy();

                gvLeaveSchemeHistory.DataSource = employeeleaveSchemes;
                gvLeaveSchemeHistory.DataBind();
            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
            finally
            {
                employeeLeaveSchemeDataHandler = null;
                employeeleaveSchemes.Dispose();
            }
        }


        //------------------------------------------------------------------------------
        ///<summary>
        ///Load all leave schemes
        ///</summary>
        //----------------------------------------------------------------------------------------

        private void fillLeaveSchemes()
        {
            log.Debug("webFrmEmployeeLeaveScheme : fillLeaveSchemes()");

            LeaveSchemeDataHandler leaveSchemeDataHandler = new LeaveSchemeDataHandler();
            DataTable leaveSchemes = new DataTable();
           
            try
            {
                leaveSchemes = leaveSchemeDataHandler.getLeaveSchemes().Copy();

                ddlLeaveScheme.Items.Clear();

                if (leaveSchemes.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlLeaveScheme.Items.Add(Item);

                    foreach (DataRow dataRow in leaveSchemes.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["LEAVE_SCHEM_NAME"].ToString();
                        listItem.Value = dataRow["LEAVE_SCHEME_ID"].ToString();

                        ddlLeaveScheme.Items.Add(listItem);
                    }
                }


            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
            finally
            {
                leaveSchemeDataHandler = null;
                leaveSchemes.Dispose();
            }
        }

        //------------------------------------------------------------------------------
        ///<summary>
        ///Load all leave scheme items to a grid for a given leave scheme id, and a grid
        ///</summary>
        //----------------------------------------------------------------------------------------

        private void fillLeaveSchemeItems(String leaveSchemeId,GridView gvParaGrid)
        {
            log.Debug("webFrmEmployeeLeaveScheme : fillLeaveSchemeItems()");

            LeaveSchemeItemsDataHandler leaveSchemeItemsDataHandler = new LeaveSchemeItemsDataHandler();
            DataTable leaveSchemeItems = new DataTable();

            try
            {
                leaveSchemeItems = leaveSchemeItemsDataHandler.getLeveSchemeItems(leaveSchemeId.Trim()).Copy();
                gvParaGrid.DataSource = leaveSchemeItems;
                gvParaGrid.DataBind();
            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
            finally
            {
                leaveSchemeItemsDataHandler = null;
                leaveSchemeItems.Dispose();
            }
        }

        private void clear()
        {
            log.Debug("webFrmEmployeeLeaveScheme : clear()");

            try
            {
                Utility.Errorhandler.ClearError(lblMessage);
                Utility.Errorhandler.ClearError(lblSchemeName);
                hfEmpId.Value = "";
                GroupHRIS.Utility.Utils.clearControls(true, ddlLeaveScheme, ddlStatus, txtEmploeeId, txtRemarks);
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                gvLeaveSchemeHistory.DataSource = null;
                gvLeaveSchemeHistory.DataBind();
                gvSchemeDetail.DataSource = null;
                gvSchemeDetail.DataBind();
                gvScheme.DataSource = null;
                gvScheme.DataBind();

                lbtnClear.Visible = false;


            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
        }

        #endregion

        protected void gvSchemeDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvSchemeDetail.DataSource = null;
            gvSchemeDetail.DataBind();
        }

        protected void gvSchemeDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("webFrmEmployeeLeaveScheme : gvLeaveSchemeHistory_RowDataBound()");

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvSchemeDetail, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }

            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
        }

        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            log.Debug("webFrmEmployeeLeaveScheme : lbtnClear_Click()");

            try
            {
                Utility.Errorhandler.ClearError(lblSchemeName);
                gvSchemeDetail.DataSource = null;
                gvSchemeDetail.DataBind();
                lbtnClear.Visible = false;
            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
        }

        

        

        

       
        

        
    }
}