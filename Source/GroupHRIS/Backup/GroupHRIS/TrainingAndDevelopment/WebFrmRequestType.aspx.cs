using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using GroupHRIS.Utility;
using DataHandler;
using DataHandler.TrainingAndDevelopment;
using DataHandler.Utility;
using System.Data;
using NLog;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmRequestType : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmRequestType : Page_Load");


            if (Session["KeyLOGOUT_STS"].Equals("0"))
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }

            if (!IsPostBack)
            {
                filStatus();
                fillRequestType();
            }
        }


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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            Utility.Errorhandler.ClearError(lblMessage);

            RequestTypeDataHandler requestTypeDataHandler = new RequestTypeDataHandler();
            UtilsDataHandler utilsDataHandler = new UtilsDataHandler();

            string addedBy = "";

            string requestTypeName = "";
            string description = "";
            string statusCode = "";


            try
            {
                requestTypeName = txtRequestTypeName.Text.Trim();
                description = txtDescription.Text.Trim();
                statusCode = ddlStatus.SelectedItem.Value.ToString();



                // check for duplicates
                // at saving
                if ((btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT) && (utilsDataHandler.isDuplicateExist(requestTypeName, "TYPE_NAME", "REQUEST_TYPE"))) { Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Request type name already exist", lblMessage); return; }
                // at updating
                if ((btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT) && (utilsDataHandler.isDuplicateExist(requestTypeName, "TYPE_NAME", "REQUEST_TYPE", hfRequestTypeId.Value.ToString(), "REQUEST_TYPE_ID"))) { Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Request type name already exist", lblMessage); return; }

                if (Session["KeyUSER_ID"] != null)
                {
                    addedBy = Session["KeyUSER_ID"].ToString();
                }


                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Insert");
                    Boolean isInserted = requestTypeDataHandler.Insert(requestTypeName, description, statusCode, addedBy);

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
                    string requestTypeId = "";

                    if (hfRequestTypeId.Value.ToString().Trim() != "") { requestTypeId = hfRequestTypeId.Value.ToString().Trim(); }

                    if (requestTypeId != "")
                    {
                        Boolean isUpdated = requestTypeDataHandler.Update(requestTypeId, requestTypeName, description, statusCode, addedBy);

                        if (isUpdated)
                        {
                            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('Competency Group is updated ..')", true); 
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
                        }
                    }

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                }
                clear();
                fillRequestType();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                requestTypeDataHandler = null;
                utilsDataHandler = null;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
            Utility.Errorhandler.ClearError(lblMessage);
        }

        private void clear()
        {
            Utils.clearControls(true, txtRequestTypeName, txtDescription, hfRequestTypeId, ddlStatus);
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;

        }

        private void fillRequestType()
        {
            RequestTypeDataHandler requestTypeDataHandler = new RequestTypeDataHandler();
            DataTable requestTypes = new DataTable();
            try
            {

                requestTypes = requestTypeDataHandler.populate();
                gvRequestType.DataSource = requestTypes;
                gvRequestType.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                requestTypeDataHandler = null;
                requestTypes.Dispose();
            }
        }

        protected void gvRequestType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvRequestType, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void gvRequestType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvTrainingCategory_PageIndexChanging()");

            try
            {
                gvRequestType.PageIndex = e.NewPageIndex;
                fillRequestType();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        protected void gvRequestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblMessage);
            RequestTypeDataHandler requestTypeDataHandler = new RequestTypeDataHandler();
            DataRow dataRow = null;
            hfRequestTypeId.Value = String.Empty;

            try
            {
                string requestTypeId = gvRequestType.SelectedRow.Cells[0].Text;

                dataRow = requestTypeDataHandler.populate(requestTypeId);

                if (dataRow != null)
                {
                    hfRequestTypeId.Value = dataRow["REQUEST_TYPE_ID"].ToString().Trim();
                    txtRequestTypeName.Text = dataRow["TYPE_NAME"].ToString().Trim();
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
                requestTypeDataHandler = null;
                dataRow = null;
            }
        }








    }
}