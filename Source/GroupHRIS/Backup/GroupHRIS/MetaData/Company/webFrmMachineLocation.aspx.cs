using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.MetaData;
using DataHandler.Employee;
using Common;

namespace GroupHRIS.MetaData.Company
{
    public partial class webFrmMachineLocation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KeyLOGOUT_STS"].Equals("0"))
            {
                Response.Redirect("MainLogout.aspx", false);
            }
            if (!IsPostBack)
            {
                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        getCompID();
                    }
                    else
                    {
                        getCompID(Session["KeyCOMP_ID"].ToString().Trim());
                        ddlCompCode.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                        populateBranch(Session["KeyCOMP_ID"].ToString().Trim());
                    }
                } 
                filStatus();
                filMachineLocation();
            }
        }


        private void populateBranch(String companyId)
        {
            BranchCenterDataHandler branchCenterDataHandler = new BranchCenterDataHandler();

            DataTable dataTable = new DataTable();

            try
            {                
                dataTable.Rows.Clear();
                ddlLocation.Items.Clear();

                dataTable = branchCenterDataHandler.getBranchesForCompany(companyId).Copy();

                ListItem listItem1 = new ListItem();
                listItem1.Text = "";
                listItem1.Value = "";

                ddlLocation.Items.Add(listItem1);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text  = dataRow["BRANCH_NAME"].ToString();
                        listItem.Value = dataRow["BRANCH_ID"].ToString();

                        ddlLocation.Items.Add(listItem);
                    }
                }

            }
            catch (Exception ex)
            {                
                throw ex;
            }
            finally
            {
                branchCenterDataHandler = null;
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

        private void getCompID()
        {
            CompanyDataHandler company = new CompanyDataHandler();
            DataTable schCompID = new DataTable();

            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlCompCode.Items.Add(listItemBlank);

                schCompID = company.populate();
                if (schCompID.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in schCompID.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlCompCode.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                company = null;
                schCompID.Dispose();
            }
        }

        private void getCompID(string sComID)
        {
            CompanyDataHandler company = new CompanyDataHandler();
            DataTable schCompID = new DataTable();

            try
            {
                schCompID = company.getCompanyIdCompName(sComID);
                if (schCompID.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in schCompID.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlCompCode.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                company = null;
                schCompID.Dispose();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string mCompID      = ddlCompCode.SelectedItem.Value.ToString();
            string mMachineID   = txtMachineID.Text.ToString();
            string mLocation    = ddlLocation.SelectedValue.ToString();
            string mBrandName   = txtBrandName.Text.ToString();
            string mVendor      = txtVendor.Text.ToString();
            string mContactNo   = txtContactNo.Text.ToString();
            string mIPAddress   = txtIPAddress.Text.ToString();
            string mStatus      = ddlStatus.SelectedItem.Value.ToString();
            string mUser = Session["KeyUSER_ID"].ToString();
            MachineLocationDataHandler machineLocation = new MachineLocationDataHandler();
            DataTable dtMachineLocation = null;
            
            if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
            {
                dtMachineLocation = machineLocation.populateByComMacID(mCompID, mMachineID);
                if (dtMachineLocation.Rows.Count > 0)
                {
                    CommonVariables.MESSAGE_TEXT = "Record already exists with Company Code: < " + mCompID + " > and Machine ID: < " + mMachineID + " > "  ;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);                                
                }
                else
                {
                    Boolean isIpExist = machineLocation.isIPExist(txtIPAddress.Text.Trim());
                    if (isIpExist == false)
                    {
                        machineLocation.insert(mCompID, mMachineID, mLocation, mBrandName, mVendor, mContactNo, mIPAddress, mStatus, mUser);
                        CommonVariables.MESSAGE_TEXT = "Record(s) saved successfully.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        filMachineLocation();
                        clear();
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "IP Address Already Exists ";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                }
            }
            else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
            {
                machineLocation.update(mCompID, mMachineID, mLocation, mBrandName, mVendor, mContactNo, mIPAddress, mStatus, mUser);
                CommonVariables.MESSAGE_TEXT = "Record(s) updated successfully.";
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                filMachineLocation();
                clear();
            }
            
        }

        private void filMachineLocation() 
        {
            MachineLocationDataHandler dhMachineLocation = new MachineLocationDataHandler();
            DataTable dtMachine = new DataTable();
            try
            {
                string mCompCode = ddlCompCode.SelectedValue;
                dtMachine = dhMachineLocation.populate(mCompCode);
                GridView1.DataSource = dtMachine;
                GridView1.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                dhMachineLocation = null;
                dtMachine.Dispose();
            }
        }

        protected void ddlCompCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblerror);
            filMachineLocation();
            clear();
            string compId = ddlCompCode.SelectedValue.ToString();
            populateBranch(compId);

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.GridView1, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtMachineID.Enabled = false;
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblerror);
            txtMachineID.ReadOnly = true;
            txtMachineID.BorderStyle = BorderStyle.None;
            MachineLocationDataHandler machineLocation = new MachineLocationDataHandler();
            DataRow dataRow = null;
            try
            {
                string mComID = GridView1.SelectedRow.Cells[0].Text;
                string mMachineID = GridView1.SelectedRow.Cells[1].Text;
                dataRow = machineLocation.populateByComID(mComID, mMachineID);
                if (dataRow != null)
                {
                    txtMachineID.Text = dataRow["MACHINE_ID"].ToString().Trim();
                    //ddlLocation.SelectedValue = dataRow["LOCATION"].ToString().Trim();
                    ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByValue(dataRow["LOCATION"].ToString().Trim()));
                    txtBrandName.Text = dataRow["BRAND_NAME"].ToString().Trim();
                    txtVendor.Text = dataRow["VENDOR_NAME"].ToString().Trim();
                    txtContactNo.Text = dataRow["CONTACT_NO"].ToString().Trim();
                    txtIPAddress.Text = dataRow["IP_ADDRESS"].ToString().Trim();
                    ddlStatus.SelectedValue = dataRow["STATUS_CODE"].ToString().Trim();
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                machineLocation = null;
                dataRow = null;
            }
        }

        public void clear()
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            txtBrandName.Text = "";
            txtContactNo.Text = "";
            txtIPAddress.Text = "";
            ddlLocation.SelectedIndex = 0;
            txtMachineID.Text = "";
            txtVendor.Text = "";
            //txtMachineID.Enabled = true;
            ddlStatus.SelectedIndex = -1;
            txtMachineID.ReadOnly = false;
            txtMachineID.BorderStyle = BorderStyle.NotSet;

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
            ddlCompCode.SelectedIndex = 0;
            GridView1.DataSource = null;
            GridView1.DataBind();
            Utility.Errorhandler.ClearError(lblerror);
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Utility.Errorhandler.ClearError(lblerror);
            GridView1.PageIndex = e.NewPageIndex;
            filMachineLocation();
            clear();
        }
    }
}