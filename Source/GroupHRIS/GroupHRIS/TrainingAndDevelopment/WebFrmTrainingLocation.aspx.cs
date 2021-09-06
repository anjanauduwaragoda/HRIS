using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.TrainingAndDevelopment;
using System.Data;
using Common;
using GroupHRIS.Utility;
using NLog;
using DataHandler.Utility;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainingLocation : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmTrainingLocation : Page_Load");

            try
            {
                if (!IsPostBack)
                {
                    loadProvince(ddlProvince);
                    loadProvince(ddlProvincesearch);
                    loadBank();
                    fillStatus();
                    getAllLocation();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");
            string logUser = Session["KeyUSER_ID"].ToString();
            string location = txtLocation.Text;
            string address = txtAddress.Text;
            string contact1 = txtContact1.Text.Trim();
            string contact2 = txtContact2.Text.Trim();
            string email = txtEmail.Text;
            string capacity = txtCapacity.Text;
            string description = txtDescription.Text;
            string bank = ddlBank.SelectedValue;
            string branch = ddlBankBranch.SelectedValue;
            string accno = txtAccNo.Text;
            string instruction = txtInstruction.Text;
            string status = ddlStatus.SelectedValue;
            string province = ddlProvince.SelectedValue;
            string district = ddlDistrict.SelectedValue;

            TrainingLocationDataHandler TLDH = new TrainingLocationDataHandler();
            UtilsDataHandler UDH = new UtilsDataHandler();

            try
            {
                Errorhandler.ClearError(lblMessage);


                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    bool isExist = UDH.isDuplicateExist(location, "LOCATION_NAME", "TRAINING_LOCATIONS");
                    if (isExist)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Location name already exist.", lblMessage);
                        return;
                    }

                    Boolean isSuccess = TLDH.Insert(location, address, contact1, contact2, email, capacity, description, bank, branch, accno, instruction, status, logUser, province, district);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblMessage);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string locationId = hfglId.Value.ToString();
                    bool isExist = UDH.isDuplicateExist(location, "LOCATION_NAME", "TRAINING_LOCATIONS", locationId, "LOCATION_ID");
                    if (isExist)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Location name already exist.", lblMessage);
                        return;
                    }

                    Boolean isSuccess = TLDH.Update(locationId, location, address, contact1, contact2, email, capacity, description, bank, branch, accno, instruction, status, logUser, province, district);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
                }
                getAllLocation();
                clear();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");
            Errorhandler.ClearError(lblMessage);
            clear();
        }

        protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlBank_SelectedIndexChanged()");
            try
            {
                loadBankBranch();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("grdLocation_SelectedIndexChanged()");
            TrainingLocationDataHandler TLDH = new TrainingLocationDataHandler();

            Errorhandler.ClearError(lblMessage);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            DataTable dt = new DataTable();

            try
            {
                int SelectedIndex = grdLocation.SelectedIndex;
                hfglId.Value = Server.HtmlDecode(grdLocation.Rows[SelectedIndex].Cells[0].Text.ToString());
                dt = TLDH.getSelectedLocation(hfglId.Value);

                txtLocation.Text = dt.Rows[0][0].ToString();
                txtAddress.Text = dt.Rows[0][1].ToString();
                txtContact1.Text = dt.Rows[0][2].ToString();
                txtContact2.Text = dt.Rows[0][3].ToString();
                txtEmail.Text = dt.Rows[0][4].ToString();
                txtCapacity.Text = dt.Rows[0][5].ToString();
                txtDescription.Text = dt.Rows[0][6].ToString();
                ddlBank.SelectedValue = dt.Rows[0][7].ToString();
                loadBankBranch();
                ddlBankBranch.SelectedValue = dt.Rows[0][8].ToString();
                txtAccNo.Text = dt.Rows[0][9].ToString();
                txtInstruction.Text = dt.Rows[0][10].ToString();
                ddlStatus.SelectedValue = dt.Rows[0][11].ToString();
                ddlProvince.SelectedValue = dt.Rows[0][12].ToString();
                loadDistrct(ddlDistrict, ddlProvince);
                ddlDistrict.SelectedValue = dt.Rows[0][13].ToString();

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TLDH = null;
            }
        }

        protected void grdLocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdLocation.PageIndex = e.NewPageIndex;
                Utility.Errorhandler.ClearError(lblMessage);

                getAllLocation();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdLocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdLocation, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            log.Debug("btnSearch_Click()");

            try
            {
                filterLocation();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlProvince_SelectedIndexChanged()");
            try
            {
                loadDistrct(ddlDistrict, ddlProvince);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void ddlProvincesearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlProvincesearch_SelectedIndexChanged()");
            try
            {
                loadDistrct(ddlDistricSearch, ddlProvincesearch);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }


        public void loadBank()
        {
            TrainingLocationDataHandler TLDH = new TrainingLocationDataHandler();
            DataTable dtBank = new DataTable();

            try
            {
                dtBank = TLDH.getBank();

                ddlBank.Items.Clear();

                if (dtBank.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlBank.Items.Add(Item);

                    foreach (DataRow dataRow in dtBank.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["BANK_NAME"].ToString();
                        listItem.Value = dataRow["BANK_ID"].ToString();

                        ddlBank.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TLDH = null;
                dtBank.Dispose();
            }
        }

        public void loadBankBranch()
        {
            TrainingLocationDataHandler TLDH = new TrainingLocationDataHandler();
            DataTable dtBankBranch = new DataTable();

            try
            {
                dtBankBranch = TLDH.getBankBranch(ddlBank.SelectedValue);

                ddlBankBranch.Items.Clear();

                if (dtBankBranch.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlBankBranch.Items.Add(Item);

                    foreach (DataRow dataRow in dtBankBranch.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["BRANCH_NAME"].ToString();
                        listItem.Value = dataRow["BRANCH_ID"].ToString();

                        ddlBankBranch.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TLDH = null;
                dtBankBranch.Dispose();
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

        public void getAllLocation()
        {

            TrainingLocationDataHandler TLDH = new TrainingLocationDataHandler();

            try
            {
                grdLocation.DataSource = TLDH.getAllLocation();
                grdLocation.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TLDH = null;
            }
        }

        public void clear()
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            txtLocation.Text = "";
            txtAddress.Text = "";
            txtContact1.Text = "";
            txtContact2.Text = "";
            txtEmail.Text = "";
            txtCapacity.Text = "";
            txtDescription.Text = "";
            ddlBank.SelectedValue = "";
            ddlBankBranch.Items.Clear();
            txtAccNo.Text = "";
            txtInstruction.Text = "";
            ddlStatus.SelectedValue = "";
            hfglId.Value = "";
            ddlProvince.SelectedIndex = 0;
            ddlDistrict.Items.Clear();
        }

        public void loadProvince(DropDownList ddl)
        {
            TrainingLocationDataHandler TLDH = new TrainingLocationDataHandler();
            DataTable dtprovince = new DataTable();

            try
            {
                dtprovince = TLDH.getProvince();

                ddl.Items.Clear();

                if (dtprovince.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddl.Items.Add(Item);

                    foreach (DataRow dataRow in dtprovince.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["PROVINCE_NAME"].ToString();
                        listItem.Value = dataRow["PROVINCE_ID"].ToString();

                        ddl.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TLDH = null;
                dtprovince.Dispose();
            }
        }

        public void loadDistrct(DropDownList ddl, DropDownList ddls)
        {
            TrainingLocationDataHandler TLDH = new TrainingLocationDataHandler();
            DataTable dtdistrct = new DataTable();

            try
            {
                dtdistrct = TLDH.getDistrict(ddls.SelectedValue);

                ddl.Items.Clear();

                if (dtdistrct.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddl.Items.Add(Item);

                    foreach (DataRow dataRow in dtdistrct.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DISTRICT_NAME"].ToString();
                        listItem.Value = dataRow["DISTRICT_ID"].ToString();

                        ddl.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TLDH = null;
                dtdistrct.Dispose();
            }
        }

        public void filterLocation()
        {
            TrainingLocationDataHandler TLDH = new TrainingLocationDataHandler();

            string province = ddlProvincesearch.SelectedValue;
            string district = ddlDistricSearch.SelectedValue;

            try
            {
                grdLocation.DataSource = TLDH.filterTrainingLocation(province, district);
                grdLocation.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                TLDH = null;
            }
        }

    }
}