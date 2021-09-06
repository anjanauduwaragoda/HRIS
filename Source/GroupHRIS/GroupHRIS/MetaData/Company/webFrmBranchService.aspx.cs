using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.MetaData;
using System.Data;

namespace GroupHRIS.MetaData
{
    public partial class webFrmBranchService : System.Web.UI.Page
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
                        getCompId();
                    }
                    else
                    {
                        getCompId(Session["KeyCOMP_ID"].ToString().Trim());
                        ddlCompID.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                    }
                } 
                 
                filStatus();
                filBranchCenter();
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
        private void filBranchCenter()
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            BranchCenterDataHandler branchCenter = new BranchCenterDataHandler();
            DataTable schBranchCenter = new DataTable();
            try
            {
                string mComID = ddlCompID.SelectedItem.Value.ToString();
                schBranchCenter = branchCenter.populate(mComID);
                GridView1.DataSource = schBranchCenter;
                GridView1.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                branchCenter = null;
                schBranchCenter.Dispose();
            }
        }

        private void getCompId()
        {
            CompanyDataHandler company = new CompanyDataHandler();
            DataTable schCompnay = new DataTable();

            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlCompID.Items.Add(listItemBlank);

                schCompnay = company.populate();

                if (schCompnay.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in schCompnay.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();
                        ddlCompID.Items.Add(listItem);
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
                company = null;
                schCompnay.Dispose();
            }
        }
        private void getCompId(string sComID)
        {
            CompanyDataHandler company = new CompanyDataHandler();
            DataTable schCompnay = new DataTable();

            try
            {
                schCompnay = company.getCompanyIdCompName(sComID);

                if (schCompnay.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in schCompnay.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();
                        ddlCompID.Items.Add(listItem);
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
                company = null;
                schCompnay.Dispose();
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            filBranchCenter();
            clear();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Boolean ErrorFlag = false;
            string mCompId = ddlCompID.SelectedItem.Value.ToString();
            string mBrName = txtBranchName.Text.ToString();
            string mAdd1 = txtBranchAdd1.Text.ToString();
            string mAdd2 = txtBranchAdd2.Text.ToString();
            string mAdd3 = txtBranchAdd3.Text.ToString();
            string mAdd4 = txtBranchAdd4.Text.ToString();
            string mLandNo1 = txtLandNo.Text.ToString();
            string mLandNo2 = txtLandNo2.Text.ToString();
            string mFaxNo = txtFaxNo.Text.ToString();
            string mStatus = ddlStatus.SelectedItem.Value.ToString();
            string mContactPerson = txtContactPerson.Text.ToString();
            string mBranchCode = txtbranchcode.Text.ToString();

            BranchCenterDataHandler branchCenter = new BranchCenterDataHandler();
            BranchManagerDataHandler brnachManager = new BranchManagerDataHandler();
            DataTable dtBranchManager = null;
            DataTable dtBranchCenter = null;

            Boolean isExistBranchCode = branchCenter.isBranchCodeExist(mBranchCode, mCompId);

            try
            {
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    if (isExistBranchCode)// && Session["branchCode"].ToString() != mBranchCode
                    {
                        CommonVariables.MESSAGE_TEXT = "Branch code already exist.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        return;
                    }

                    dtBranchCenter = branchCenter.populateByName(mBrName, mCompId);
                    if (dtBranchCenter.Rows.Count > 0)
                    {
                        CommonVariables.MESSAGE_TEXT = "Record already exists with : < " + mBrName + " >";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);

                    }
                    else
                    {
                        branchCenter.insert(mCompId, mBrName, mAdd1, mAdd2, mAdd3, mAdd4, mLandNo1, mLandNo2, mFaxNo, mStatus, mContactPerson, mBranchCode);
                        CommonVariables.MESSAGE_TEXT = "Record(s) saved successfully.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        clear();
                        filBranchCenter();
                    }
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    if (isExistBranchCode && Session["branchCode"].ToString() != mBranchCode)
                    {
                        CommonVariables.MESSAGE_TEXT = "Branch code already exist.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        return;
                    }

                    string mBranchID = hfBranchID.Value.ToString();
                    dtBranchCenter = branchCenter.populateByNameID(mBrName, mCompId, mBranchID);
                    if (dtBranchCenter.Rows.Count > 0)
                    {
                        CommonVariables.MESSAGE_TEXT = "Record already exists with : < " + mBrName + " >";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        if (mStatus == "0")
                        {
                            dtBranchManager = brnachManager.populateActiveData(mBranchID);
                            if (dtBranchManager.Rows.Count > 0)
                            {
                                ErrorFlag = true;
                                CommonVariables.MESSAGE_TEXT = "Branch cannot be Inactive, related < Branch Manager > records are available.";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            }
                        }

                        if (ErrorFlag == false)
                        {
                            branchCenter.update(mBranchID, mCompId, mBrName, mAdd1, mAdd2, mAdd3, mAdd4, mLandNo1, mLandNo2, mFaxNo, mStatus, mContactPerson, mBranchCode);
                            CommonVariables.MESSAGE_TEXT = "Record(s) modified successfully.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                            clear();
                            filBranchCenter();
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
                branchCenter = null;
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblerror);
            BranchCenterDataHandler branchCenterHandle = new BranchCenterDataHandler();
            DataRow dataRow = null;
            try
            {
                hfBranchID.Value = GridView1.SelectedRow.Cells[0].Text;
                dataRow = branchCenterHandle.getBranchCenterDetails(hfBranchID.Value);

                if (dataRow != null)
                {
                    ddlCompID.SelectedValue = dataRow["COMPANY_ID"].ToString().Trim();
                    txtBranchName.Text = dataRow["BRANCH_NAME"].ToString().Trim();
                    txtBranchAdd1.Text = dataRow["BRANCH_ADDRESS_LINE1"].ToString().Trim();
                    txtBranchAdd2.Text = dataRow["BRANCH_ADDRESS_LINE2"].ToString().Trim();
                    txtBranchAdd3.Text = dataRow["BRANCH_ADDRESS_LINE3"].ToString().Trim();
                    txtBranchAdd4.Text = dataRow["BRANCH_ADDRESS_LINE4"].ToString().Trim();
                    txtLandNo.Text = dataRow["LAND_PHONE1"].ToString().Trim();
                    txtLandNo2.Text = dataRow["LAND_PHONE2"].ToString().Trim();
                    txtFaxNo.Text = dataRow["FAX_NUMBER"].ToString().Trim();
                    txtbranchcode.Text = dataRow["BRANCH_CODE"].ToString().Trim();
                    ddlStatus.SelectedValue = dataRow["STATUS_CODE"].ToString().Trim();
                    txtContactPerson.Text = dataRow["CONTACT_PERSON"].ToString().Trim();

                    Session["branchCode"] = dataRow["BRANCH_CODE"].ToString().Trim();
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                branchCenterHandle = null;
                dataRow = null;
            }

        }

        private void clear()
        {
            txtBranchName.Text = "";
            txtBranchAdd1.Text = "";
            txtBranchAdd2.Text = "";
            txtBranchAdd3.Text = "";
            txtBranchAdd4.Text = "";
            txtContactPerson.Text = "";
            txtFaxNo.Text = "";
            txtLandNo.Text = "";
            txtLandNo2.Text = "";
            txtbranchcode.Text = "";
            ddlStatus.SelectedIndex = -1;
            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
            Utility.Errorhandler.ClearError(lblerror);
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

        protected void ddlCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            clear();
            filBranchCenter();
            Utility.Errorhandler.ClearError(lblerror);
        }
    }
}