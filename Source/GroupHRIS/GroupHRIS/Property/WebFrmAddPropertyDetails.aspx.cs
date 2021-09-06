using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Property;
using System.Data;
using DataHandler.Payroll;
using Common;
using GroupHRIS.Utility;

namespace GroupHRIS.Property
{
    public partial class WebFrmAddPropertyDetails : System.Web.UI.Page
    {
        #region events

        protected void Page_Load(object sender, EventArgs e)
        {
            PropertyDetailsDataHandler oPropertyDetails = new PropertyDetailsDataHandler();
            try
            {
                if (!IsPostBack)
                {
                    Errorhandler.ClearError(lblMessage);
                    getPropertyType();
                    fillCompanies();
                    gvPropertyDetails.DataSource = oPropertyDetails.GetPropertyDetails();
                    gvPropertyDetails.DataBind();

                }
            }
            catch (Exception exp)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, lblMessage);
            }

            if (!IsPostBack)
            {
                getStatus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string type = ddlPropertyType.SelectedValue;
            string reference = txtReference.Text;
            string model = txtModel.Text;
            string serial = txtSerial.Text;
            string company = ddlCompany.SelectedValue;
            string status = ddlStstus.SelectedValue.ToString();
            string logUser = Session["KeyUSER_ID"].ToString();

            try
            {
                PropertyDetailsDataHandler oPropertyDetails = new PropertyDetailsDataHandler();

                bool isExist = oPropertyDetails.IsExistRefSerial(reference, serial);

                
                    if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                    {
                        if (isExist == true)
                        {
                            Boolean isInsert = oPropertyDetails.InsertPropertyDetails(type, reference, model, serial, company, status, logUser);
                            Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", lblMessage);
                        }
                        else
                        {
                            txtReference.Text = "";
                            txtSerial.Text = "";
                            Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Reference No/Serial Alredy Exist", lblMessage);
                            return;
                        }
                    }
                    else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {
                        string id = hfPropaertyId.Value;
                        string statusCode = oPropertyDetails.getStatus(id,type);

                        if (statusCode != Constants.CON_ASSIGNED_STATUS)
                        {
                            Boolean isUpdate = oPropertyDetails.UpdateDetails(id, type, reference, model, serial, company, status, logUser);
                            Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Updated", lblMessage);
                        }
                        else
                        {
                            status = Constants.CON_ASSIGNED_STATUS;
                            Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Can not update Assigned Benefit(s)", lblMessage);
                            return;
                        }
                    }
                
                
                gvPropertyDetails.DataSource = oPropertyDetails.GetPropertyDetails();
                gvPropertyDetails.DataBind();
                clear();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            clear();
        }

        protected void gvPropertyDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvPropertyDetails, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void gvPropertyDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PropertyDetailsDataHandler oPropertyDetails = new PropertyDetailsDataHandler();
            gvPropertyDetails.PageIndex = e.NewPageIndex;
            gvPropertyDetails.DataSource = oPropertyDetails.GetPropertyDetails();
            gvPropertyDetails.DataBind();
            oPropertyDetails = null;
        }

        protected void gvPropertyDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

            int SelectedIndex = gvPropertyDetails.SelectedIndex;

            hfPropaertyId.Value = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());

            string property = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
            ddlPropertyType.SelectedIndex = ddlPropertyType.Items.IndexOf(ddlPropertyType.Items.FindByText(property));

            txtReference.Text = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());
            txtModel.Text = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
            txtSerial.Text = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[4].Text.ToString().Trim());

            string company = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[5].Text.ToString().Trim());
            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByText(company));

            string status = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[6].Text.ToString().Trim());
            ddlStstus.SelectedIndex = ddlStstus.Items.IndexOf(ddlStstus.Items.FindByText(status));

            if (status == "Assigned")
            {
                ddlStstus.SelectedValue = "1";
                ddlStstus.Visible = false;
                lblstatus.Text = status;
            }
            else
            {
                ddlStstus.Visible = true;
                lblstatus.Text = "";
            }


        }

        protected void ddlPropertyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string propertyId = ddlPropertyType.SelectedValue;
            PropertyDetailsDataHandler oPropertyDetails = new PropertyDetailsDataHandler();
            gvPropertyDetails.DataSource = oPropertyDetails.GetPropertyDetailsForSelectedProperty(propertyId);
            gvPropertyDetails.DataBind();
            Errorhandler.ClearError(lblMessage);
        }

        #endregion

        #region  methods

        public void getStatus()
        {
            ddlStstus.Items.Insert(0, new ListItem("", ""));
            ddlStstus.Items.Insert(1, new ListItem(Constants.CON_AVILABLE_TAG, Constants.CON_AVILABLE_STATUS));
            //ddlStstus.Items.Insert(2, new ListItem(Constants.CON_ASSIGNED_TAG, Constants.CON_ASSIGNED_STATUS));
            ddlStstus.Items.Insert(2, new ListItem(Constants.CON_DISPOSED_TAG, Constants.CON_DISPOSED_STATUS));
        }

        public void clear()
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            ddlPropertyType.SelectedIndex = 0;
            txtReference.Text = "";
            txtModel.Text = "";
            txtSerial.Text = "";
            ddlCompany.SelectedIndex = 0;
            ddlStstus.SelectedIndex = 0;
            ddlStstus.Visible = true;

            lblstatus.Text = "";

            PropertyDetailsDataHandler oPropertyDetails = new PropertyDetailsDataHandler();
            gvPropertyDetails.DataSource = oPropertyDetails.GetPropertyDetails();
            gvPropertyDetails.DataBind();
        }

        private void getPropertyType()
        {
            PropertyDetailsDataHandler oPropertyDetails = new PropertyDetailsDataHandler();
            DataTable properties = new DataTable();

            try
            {
                ddlPropertyType.Items.Clear();

                properties = oPropertyDetails.GetPropertytypes().Copy();

                if (properties.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlPropertyType.Items.Add(Item);

                    foreach (DataRow dataRow in properties.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DESCRIPTION"].ToString();
                        listItem.Value = dataRow["TYPE_ID"].ToString();

                        ddlPropertyType.Items.Add(listItem);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void fillCompanies()
        {
            CompanyOTCategoryDataHandler companyOTCategoryDataHandler = new CompanyOTCategoryDataHandler();
            DataTable companies = new DataTable();

            try
            {
                companies = companyOTCategoryDataHandler.getCompanyIdCompName().Copy();

                ddlCompany.Items.Clear();

                if (companies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlCompany.Items.Add(Item);

                    foreach (DataRow dataRow in companies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlCompany.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                companyOTCategoryDataHandler = null;
                companies.Dispose();
            }
        }

        #endregion

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string propertyId = ddlPropertyType.SelectedValue;
                string company = ddlCompany.SelectedValue;

                PropertyDetailsDataHandler oPropertyDetails = new PropertyDetailsDataHandler();
                if (propertyId != "" && company != "")
                {
                    gvPropertyDetails.DataSource = oPropertyDetails.GetPropertyDetailsForCompany(company, propertyId);
                    gvPropertyDetails.DataBind();
                }
                else if (company != "" && propertyId == "")
                {
                    gvPropertyDetails.DataSource = oPropertyDetails.GetAllPropertyDetailsForCompany(company);
                    gvPropertyDetails.DataBind();
                }
            }
            catch (Exception )
            {
                
                throw ;
            }

        }

    }
}