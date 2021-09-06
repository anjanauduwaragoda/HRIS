using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.Property;
using Common;
using GroupHRIS.Utility;

namespace GroupHRIS.Property
{
    public partial class WebFrmViewProperty : System.Web.UI.Page
    {
        ViewBenefitsDataHandler oViewBenefitsDataHandler = new ViewBenefitsDataHandler();
        PropertyDetailsSearchDataHandler oPropertyDetailsSearchDataHandle = new PropertyDetailsSearchDataHandler();
            
        protected void Page_Load(object sender, EventArgs e)
        {
                        
            if (!IsPostBack)
            {
                try
                {
                    DataTable dtProperties = (DataTable)Session["propertyBucket"];
                    dtProperties = oViewBenefitsDataHandler.viewBenefit(dtProperties);

                    if (dtProperties.Rows.Count > 0)
                    {
                        btnExclude.Visible = true;
                    }
                    else
                    {
                        btnExclude.Visible = false;
                    }

                    grdViewBenefits.DataSource = dtProperties;
                    grdViewBenefits.DataBind();

                    if (Session["empid"].ToString() != null)
                    {
                        DataTable benefits = oPropertyDetailsSearchDataHandle.GetEmployeeUtilizedProperty(Session["empid"].ToString());
                        if (benefits.Rows.Count > 0)
                        {
                            tblBenefits.Visible = true;
                            gvemployeeProperty.DataSource = benefits;
                            gvemployeeProperty.DataBind();
                        }
                        else
                        {
                            tblBenefits.Visible = false;
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        protected void btnExclude_Click(object sender, EventArgs e)
        {
            try
            {
                createPropertyBucket();
                DataTable dt = (DataTable)Session["propertyBucket"];

                for (int i = 0; i < grdViewBenefits.Rows.Count; i++)
                {
                    //CheckBox chk = new CheckBox();
                    //chk = (grdViewBenefits.Rows[i].Cells[6].FindControl("chkBxSelect") as CheckBox);

                    if ((grdViewBenefits.Rows[i].Cells[6].FindControl("chkBxSelect") as CheckBox).Checked == false)
                    {
                        string propertyTypeId = grdViewBenefits.Rows[i].Cells[5].Text.ToString().Trim();
                        string typeId = grdViewBenefits.Rows[i].Cells[4].Text.ToString().Trim();
                        
                        DataRow dtrow = dt.NewRow();
                        dtrow["PROPERTY_TYPE_ID"] = propertyTypeId;
                        dtrow["PROPERTY_ID"] = typeId;
                       
                        dt.Rows.Add(dtrow);
                    }
                }
                Session["propertyBucket"] = dt;
                grdViewBenefits.DataSource = oViewBenefitsDataHandler.viewBenefit(dt);
                grdViewBenefits.DataBind();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void createPropertyBucket()
        {
            DataTable propertyBucket = new DataTable();
            propertyBucket.Columns.Add("PROPERTY_TYPE_ID", typeof(string));
            propertyBucket.Columns.Add("PROPERTY_ID", typeof(string));

            Session["propertyBucket"] = propertyBucket;
        }

        protected void gvemployeeProperty_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvemployeeProperty, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            string lineId = "";
            string propertyId = "";

            try
            {
                if (!string.IsNullOrEmpty(Session["lineId"] as string))
                {
                    lineId = Session["lineId"].ToString();
                    propertyId = Session["propertyId"].ToString();
                }
                string user = (string)(Session["KeyUSER_ID"]);
                string remarks = txtReason.Text;
                bool isUpdate = false;

                if (lineId != "" && propertyId != "")
                {
                    isUpdate = oViewBenefitsDataHandler.removeBenefit(lineId, user, propertyId, remarks);
                }
                
                if (isUpdate)
                {
                    gvemployeeProperty.DataSource = oPropertyDetailsSearchDataHandle.GetEmployeeUtilizedProperty(Session["empid"].ToString());
                    gvemployeeProperty.DataBind();
                    clearRemoved();
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Removed", lblMessage);
                }
                else
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Not Removed", lblMessage);
                }
            }
            catch (Exception ex)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMessage);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                clearRemoved();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void gvemployeeProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);

            try
            {
                int SelectedIndex = gvemployeeProperty.SelectedIndex;
                string name = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());
                txtName.Text = name;
                string aDate = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
                txtDate.Text = aDate;
                string mail = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
                txtMail.Text = mail;
                lblStatus.Text = Constants.CON_REMOVE_TAG;
                string lineId = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[8].Text.ToString().Trim());
                Session["lineId"] = lineId;
                string propertyId = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[6].Text.ToString().Trim());
                Session["propertyId"] = propertyId;

            }
            catch (Exception ex)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMessage);
            }
            
        }

        private void clearRemoved()
        {
            Session["lineId"] = "";
            Session["propertyId"] = "";
            txtName.Text = "";
            txtMail.Text = "";
            txtDate.Text = "";
            txtReason.Text = "";
            lblStatus.Text = "";

            Errorhandler.ClearError(lblMessage);
        }

    }
}