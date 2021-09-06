using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.MetaData;
using System.Data;
using Common;
using NLog;

namespace GroupHRIS.MetaData
{
    public partial class webFrmSalaryComponents : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "webFrmSalaryComponents : Page_Load");

            if (Session["KeyLOGOUT_STS"].Equals("0"))
            {
                log.Debug("Session Expired");
                Response.Redirect("MainLogout.aspx", false);
            }
            if (!IsPostBack == true)
            {
                filSalaryComponent();
                filStatus();
            }
        }

        private void filStatus()
        {
            log.Debug("filStatus()");

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

        private void filSalaryComponent()
        {
            log.Debug("filSalaryComponent()");

            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            SalaryComponentDataHandler salaryComponent = new SalaryComponentDataHandler();
            DataTable schSalComponent = new DataTable();
            try
            {
                schSalComponent = salaryComponent.populate();
                GridView1.DataSource = schSalComponent;
                GridView1.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("GridView1_SelectedIndexChanged()");
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblerror);
            SalaryComponentDataHandler salaryComponent = new SalaryComponentDataHandler();
            DataRow dataRow = null;
            try
            {
                string salaryComponentID = GridView1.SelectedRow.Cells[0].Text;
                dataRow = salaryComponent.populate(salaryComponentID);

                if (dataRow != null)
                {
                    txtRemarks.Text = dataRow["REMARKS"].ToString().Trim();
                    txtComponentName.Text = dataRow["COMPONENT_NAME"].ToString().Trim();
                    txtpayrollcode.Text  = dataRow["PAYROLL_CODE"].ToString().Trim();
                    ddlStatus.SelectedValue = dataRow["STATUS_CODE"].ToString().Trim();
                }
            }
            catch (Exception Ex)
            {
                log.Error(Ex.Message);
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("GridView1_SelectedIndexChanged()");
            
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    e.Row.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(this.GridView1, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                log.Error(Ex.Message);
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("GridView1_PageIndexChanging()");

            GridView1.PageIndex = e.NewPageIndex;
            filSalaryComponent();
            clear();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");

            clear();
            Utility.Errorhandler.ClearError(lblerror);
        }

        private void clear()
        {
            log.Debug("clear()");
            txtComponentName.Text = "";
            txtRemarks.Text = "";
            txtpayrollcode.Text = "";
            ddlStatus.SelectedIndex = -1;
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");
            string mSalaryComName = txtComponentName.Text.ToString();
            string mRemarks = txtRemarks.Text.ToString();
            string mStatus = ddlStatus.SelectedItem.Value.ToString();
            string sPayrollcode = txtpayrollcode.Text.ToString();

            SalaryComponentDataHandler salaryComponent = new SalaryComponentDataHandler();
            DataTable dtSalaryComponent = null;
            
            try
            {
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Insert");
                    dtSalaryComponent = salaryComponent.populateByName(mSalaryComName);
                    if (dtSalaryComponent.Rows.Count > 0)
                    {
                        CommonVariables.MESSAGE_TEXT = "Record already exist with : < " + mSalaryComName + " >";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        if (CheckAvailability() == true)
                        {
                            string mlogUser = Session["KeyUSER_ID"].ToString();
                            salaryComponent.insert(mSalaryComName, sPayrollcode, mRemarks, mStatus, mlogUser);
                            CommonVariables.MESSAGE_TEXT = "Record(s) saved successfully.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                            clear();
                            filSalaryComponent();
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Record already exist with payroll code : < " + txtpayrollcode.Text.Trim() + " >";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                    }
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Update");
                    string mSalaryComID = GridView1.SelectedRow.Cells[0].Text;
                    dtSalaryComponent = salaryComponent.populateByNameID(mSalaryComName, mSalaryComID);
                    if (dtSalaryComponent.Rows.Count > 0)
                    {
                        CommonVariables.MESSAGE_TEXT = "Record already exist with : < " + mSalaryComName + " >";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        if (CheckAvailability() == true)
                        {
                            string mModifyUser = Session["KeyUSER_ID"].ToString();
                            salaryComponent.update(mSalaryComID, mSalaryComName, sPayrollcode, mRemarks, mStatus, mModifyUser);
                            CommonVariables.MESSAGE_TEXT = "Record(s) modified successfully.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                            clear();
                            filSalaryComponent();
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Record already exist with payroll code : < " + txtpayrollcode.Text.Trim() + " >";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                    }
                }
                
            }
            catch (Exception Ex)
            {
                log.Error(Ex.Message);
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                salaryComponent = null;
            }
        }

        Boolean CheckAvailability()
        {
            string ComponentID = HttpUtility.HtmlDecode(GridView1.Rows[GridView1.SelectedIndex].Cells[0].Text.Trim());
            string NewPayrollCode = txtpayrollcode.Text.Trim();

            DataTable dt = new DataTable();
            SalaryComponentDataHandler SCDH = new SalaryComponentDataHandler();
            dt = SCDH.populateByPayRollCode(NewPayrollCode);

            DataRow[] result = dt.Select("COMPONENT_ID <> '" + ComponentID + "' AND PAYROLL_CODE = '" + NewPayrollCode + "'");
            if (result.Length > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}