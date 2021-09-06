using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Property;
using DataHandler.Userlogin;
using Common;
using System.Data;
using DataHandler.Payroll;
using GroupHRIS.Utility;

namespace GroupHRIS.Property
{
    public partial class WebFrmPropertySearch : System.Web.UI.Page
    {
        #region Events

        PropertyDetailsSearchDataHandler oPropertyDetailsSearchDataHandle = new PropertyDetailsSearchDataHandler();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Errorhandler.ClearError(lblMessage);

                string CompanyId = Page.Request.QueryString["CompanyId"];

                gvPropertyDetails.DataSource = oPropertyDetailsSearchDataHandle.GetPropertiesByTypeId(CompanyId);
                gvPropertyDetails.DataBind();
                getPropertyType();
                getStatus();
               // createPropertyBucket();
            }

            if (IsPostBack)
            {
                DataTable dt = (DataTable)Session["propertyBucket"];
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

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    Boolean isInsert = oPropertyDetails.InsertPropertyDetails(type, reference, model, serial, company, status, logUser);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", lblMessage);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string id = hfPropaertyId.Value;
                    Boolean isUpdate = oPropertyDetails.UpdateDetails(id, type, reference, model, serial, company, status, logUser);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Updated", lblMessage);
                }
                PropertyDetailsSearchDataHandler oPropertyDetailsSearchDataHandle = new PropertyDetailsSearchDataHandler();

                gvPropertyDetails.DataSource = oPropertyDetailsSearchDataHandle.GetPropertiesByTypeId(Page.Request.QueryString["TypeId"]);
                gvPropertyDetails.DataBind();
                clear();
            }
            catch (Exception ex)
            {
                //Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Record Alredy Exist", lblMessage);
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            clear();
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
           // DataTable dtTable = (DataTable)Session["propertyBucket"];

            //int SelectedIndex = gvPropertyDetails.SelectedIndex;
            string propertyId = (string)Session["PId"]; //Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());
            string PropertyTypeId = (string)Session["PTId"]; //Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());

            //if (Session["propertyBucket"] != null)
            //{
            //    DataTable dtProperty = (DataTable)Session["propertyBucket"];

            //    if (dtProperty.Rows.Count == 0)
            //    {
            //        DataRow drProperty = dtProperty.NewRow();
            //        drProperty["PROPERTY_TYPE_ID"] = PropertyTypeId;
            //        drProperty["PROPERTY_ID"] = propertyId;
            //        dtProperty.Rows.Add(drProperty);
            //    }
            //    else
            //    {
            //        foreach (DataRow drc in dtProperty.Rows)
            //        {
            //            string proId = drc["PROPERTY_ID"].ToString();

            //            if (proId != propertyId)
            //            {
            //                DataRow drProperty = dtProperty.NewRow();
            //                drProperty["PROPERTY_TYPE_ID"] = PropertyTypeId;
            //                drProperty["PROPERTY_ID"] = propertyId;
            //                dtProperty.Rows.Add(drProperty);
            //            }
            //        }
            //    }
            //    Session["propertyBucket"] = dtProperty;
            //}
            hfDataTable.Value = Session["propertyBucket"].ToString();
        }

        protected void gvPropertyDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPropertyDetails.PageIndex = e.NewPageIndex;
            PropertyDetailsSearchDataHandler oPropertyDetailsSearchDataHandle = new PropertyDetailsSearchDataHandler();
            gvPropertyDetails.DataSource = oPropertyDetailsSearchDataHandle.GetPropertyDetails();
            gvPropertyDetails.DataBind();
        }

        protected void gvPropertyDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                int SelectedIndex = gvPropertyDetails.SelectedIndex;
                string propertyId = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());
                string PropertyTypeId = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());

                Session["PId"] = propertyId;
                Session["PTId"] = PropertyTypeId;

                //if (Session["propertyBucket"] != null)
                //{
                //    DataTable dtProperty = (DataTable)Session["propertyBucket"];

                //    DataRow drProperty = dtProperty.NewRow();
                //    drProperty["PROPERTY_TYPE_ID"] = PropertyTypeId;
                //    drProperty["PROPERTY_ID"] = propertyId;
                //    dtProperty.Rows.Add(drProperty);

                //    Session["propertyBucket"] = dtProperty;
                //}

               
                hfPropaertyId.Value = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());

                string property = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());
                ddlPropertyType.SelectedIndex = ddlPropertyType.Items.IndexOf(ddlPropertyType.Items.FindByText(property));

                txtReference.Text = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
                txtModel.Text = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[4].Text.ToString().Trim());
                txtSerial.Text = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[5].Text.ToString().Trim());

                string company = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[6].Text.ToString().Trim());
                ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByText(company));

                string status = Server.HtmlDecode(gvPropertyDetails.Rows[SelectedIndex].Cells[7].Text.ToString().Trim());
                ddlStstus.SelectedIndex = ddlStstus.Items.IndexOf(ddlStstus.Items.FindByText(status));

            }
            catch (Exception ex)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMessage);
            }
        }

        protected void gvPropertyDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvPropertyDetails, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void ddlbenefit_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMessagex);
            string type = ddlbenefit.SelectedValue;
            string CompanyId = Page.Request.QueryString["CompanyId"];

            DataTable benefitData = oPropertyDetailsSearchDataHandle.GetSelectedProperty(type, CompanyId);

            if (benefitData.Rows.Count > 0)
            {
                gvPropertyDetails.DataSource = benefitData;
                gvPropertyDetails.DataBind();
            }
            else
            {
                gvPropertyDetails.DataSource = null;
                gvPropertyDetails.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ViewBenefitsDataHandler oViewBenefitsDataHandler = new ViewBenefitsDataHandler();
            Utility.Errorhandler.ClearError(lblMessagex);
            try
            {
                DataTable dt = (DataTable)Session["propertyBucket"];

                for (int i = 0; i < gvPropertyDetails.Rows.Count; i++)
                {
                    CheckBox chk = gvPropertyDetails.Rows[i].Cells[8].FindControl("chkBxSelect") as CheckBox;

                    if ((gvPropertyDetails.Rows[i].Cells[8].FindControl("chkBxSelect") as CheckBox).Checked == true)
                    {
                        string propertyTypeId = gvPropertyDetails.Rows[i].Cells[1].Text.ToString().Trim();
                        string typeId = gvPropertyDetails.Rows[i].Cells[0].Text.ToString().Trim();

                        if (dt.Rows.Count > 0)
                        {
                            DataRow[] result = dt.Select("PROPERTY_TYPE_ID = '" + propertyTypeId + "' AND PROPERTY_ID = '" + typeId + "'");

                            if (result.Length > 0)
                            {
                                Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Record already exist", lblMessagex);
                                chk.Checked = false;
                                return;
                            }
                            else
                            {
                                DataRow dtrow = dt.NewRow();
                                dtrow["PROPERTY_TYPE_ID"] = propertyTypeId;
                                dtrow["PROPERTY_ID"] = typeId;

                                dt.Rows.Add(dtrow);
                            }
                        }
                        else
                        {
                            DataRow dtrow = dt.NewRow();
                            dtrow["PROPERTY_TYPE_ID"] = propertyTypeId;
                            dtrow["PROPERTY_ID"] = typeId;

                            dt.Rows.Add(dtrow);
                        }
                        chk.Checked = false;
                    }
                    
                }
                Session["propertyBucket"] = dt;
                gvAddedBenefits.DataSource = oViewBenefitsDataHandler.viewBenefit(dt);
                gvAddedBenefits.DataBind();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        #endregion

        #region methods

        private void getPropertyType()
        {
            PropertyDetailsDataHandler oPropertyDetails = new PropertyDetailsDataHandler();
            DataTable properties = new DataTable();

            try
            {
                ddlbenefit.Items.Clear();

                properties = oPropertyDetails.GetPropertytypes().Copy();

                if (properties.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlbenefit.Items.Add(Item);

                    foreach (DataRow dataRow in properties.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DESCRIPTION"].ToString();
                        listItem.Value = dataRow["TYPE_ID"].ToString();

                        ddlbenefit.Items.Add(listItem);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void getStatus()
        {
            ddlStstus.Items.Insert(0, new ListItem("", ""));
            ddlStstus.Items.Insert(1, new ListItem(Constants.CON_AVILABLE_TAG, Constants.CON_AVILABLE_STATUS));
            ddlStstus.Items.Insert(2, new ListItem(Constants.CON_ASSIGNED_TAG, Constants.CON_ASSIGNED_STATUS));
            ddlStstus.Items.Insert(3, new ListItem(Constants.CON_DISPOSED_TAG, Constants.CON_DISPOSED_STATUS));
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
        }

        #endregion


    }
}