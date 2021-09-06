using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Property;
using System.Data;
using Common;
using GroupHRIS.Utility;
using System.Drawing;
using DataHandler.Userlogin;

namespace GroupHRIS.Property
{
    public partial class wWebFrmEmployeePropertyDetails : System.Web.UI.Page
    {
        ViewBenefitsDataHandler oViewBenefitsDataHandler = new ViewBenefitsDataHandler();
        EmployeePropertyDetailsDataHandler oEmployeePropertyDetailsDataHandler = new EmployeePropertyDetailsDataHandler();
        PropertyDetailsSearchDataHandler oPropertyDetailsSearchDataHandle = new PropertyDetailsSearchDataHandler();
                            
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                createPropertyBucket();
            }

            if (IsPostBack)
            {
                string x = hfCaller.Value;

                string isTrue = hfisInclude.Value.ToString();
                DataTable dtProperties = (DataTable)Session["propertyBucket"];

                if (isTrue == "true" && dtProperties.Rows.Count > 0)
                {
                    Errorhandler.ClearError(lblMessage);
                    isEnable(true);

                    btnUpdate.Enabled = true;
                    if (btnUpdate.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {
                        txtAssignDate.Text = "";
                        txtReturnedDate.Text = "";
                        txtEmail.Text = "";
                        txtRemarks.Text = "";
                    }
                    btnUpdate.Text = Constants.CON_SAVE_BUTTON_TEXT;
                    grdViewBenefits.DataSource = oViewBenefitsDataHandler.viewBenefit(dtProperties);
                    grdViewBenefits.DataBind();
                    gvemployeeProperty.DataSource = oPropertyDetailsSearchDataHandle.GetEmployeeProperty(txtEmploeeId.Text);
                    gvemployeeProperty.DataBind();
                }
                else
                {
                    grdViewBenefits.DataSource = null;
                    grdViewBenefits.DataBind();
                    gvemployeeProperty.DataSource = oPropertyDetailsSearchDataHandle.GetEmployeeProperty(txtEmploeeId.Text);
                    gvemployeeProperty.DataBind();
                }

                if (hfCaller.Value == "txtEmploeeId")
                {
                    Errorhandler.ClearError(lblMessage);
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtEmploeeId.Text = hfVal.Value;
                        txtAssignDate.Text = "";
                        txtEmail.Text = "";
                        txtRemarks.Text = "";
                    }
                    if (txtEmploeeId.Text != "")
                    {
                        //Postback Methods
                        EmployeePropertyDetailsDataHandler oEmployeePropertyDetailsDataHandler = new EmployeePropertyDetailsDataHandler();
                        DataTable table = new DataTable();
                        string employeeId = txtEmploeeId.Text;
                        Session["empid"] = employeeId;

                        table = oEmployeePropertyDetailsDataHandler.getEmployeeName(employeeId);
                        lblEmployeeName.Text = table.Rows[0]["INITIALS_NAME"].ToString();
                        UpdatePropertyStatus();

                        gvemployeeProperty.DataSource = null;
                       gvemployeeProperty.DataSource = oPropertyDetailsSearchDataHandle.GetEmployeeProperty(employeeId);
                        gvemployeeProperty.DataBind();


                    }
                }
            }
        }

        protected void txtEmploeeId_TextChanged(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            string employeeId = txtEmploeeId.Text;

            table = oEmployeePropertyDetailsDataHandler.getEmployeeName(employeeId);
            lblEmployeeName.Text = table.Rows[0]["KNOWN_NAME"].ToString();
            UpdatePropertyStatus();

            gvemployeeProperty.DataSource = null;
            PropertyDetailsSearchDataHandler oPropertyDetailsSearchDataHandle = new PropertyDetailsSearchDataHandler();
            gvemployeeProperty.DataSource = oPropertyDetailsSearchDataHandle.GetEmployeeProperty(employeeId);
            gvemployeeProperty.DataBind();

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string employeeId = txtEmploeeId.Text;
            string name = lblEmployeeName.Text.Trim();
            string assignDate = txtAssignDate.Text.Trim();
            string returnedDate = txtReturnedDate.Text.Trim();
            string email = txtEmail.Text.Trim();
            string remarks = txtRemarks.Text.Trim();
            string logUser = Session["KeyUSER_ID"].ToString();
            DataTable dtProperties = (DataTable)Session["propertyBucket"];


            try
            {
                if (btnUpdate.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    if (dtProperties.Rows.Count == 0)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Property not selected", lblMessage);
                    }
                    else
                    {
                        Boolean isInsert = oEmployeePropertyDetailsDataHandler.insert(employeeId, dtProperties, assignDate, returnedDate, remarks, email, logUser);
                        UpdatePropertyStatus();
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", lblMessage);
                    }

                }
                else if (btnUpdate.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    if (Session["status"].ToString() != Constants.CON_RETURNED_TAG)
                    {
                        Boolean isUpdate = oEmployeePropertyDetailsDataHandler.update(assignDate, remarks, email, Session["lineId"].ToString());
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Updated", lblMessage);
                    }
                    else
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Can not update return property", lblMessage);
                    }
                }

                DataTable table = new DataTable();
                table = oEmployeePropertyDetailsDataHandler.getEmployeeName(employeeId);

                PropertyDetailsSearchDataHandler oPropertyDetailsSearchDataHandle = new PropertyDetailsSearchDataHandler();
                gvemployeeProperty.DataSource = oPropertyDetailsSearchDataHandle.GetEmployeeProperty(employeeId);
                gvemployeeProperty.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }

            clearPropertyBucket();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            isEnable(true);

            clear();
            clearPropertyBucket();
            gvemployeeProperty.DataSource = null;
            gvemployeeProperty.DataBind();

            grdViewBenefits.DataSource = null;
            grdViewBenefits.DataBind();
        }

        protected void menuBar_MenuItemClick(object sender, MenuEventArgs e)
        {
            string employeeId = txtEmploeeId.Text;

            DataTable propertyTable = new DataTable();
            propertyTable = oEmployeePropertyDetailsDataHandler.getDefaultPropertyList(employeeId);

            ListItem Item = new ListItem();
            foreach (DataRow dataRow in propertyTable.Rows)
            {
                Item.Value = dataRow["PROPERTY_TYPE_ID"].ToString();
                Item.Text = dataRow["DESCRIPTION"].ToString();
            }
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

        protected void gvemployeeProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            btnUpdate.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            grdViewBenefits.DataSource = null;
            grdViewBenefits.DataBind();

            int SelectedIndex = gvemployeeProperty.SelectedIndex;
            string assignDate = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());

            if (assignDate != null)
            {
                string[] dateArr = assignDate.Split('/', '-');
                assignDate = dateArr[2] + "/" + dateArr[1] + "/" + dateArr[0];
            }
            txtAssignDate.Text = assignDate;
            string returnDate = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());
            txtEmail.Text = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
            txtRemarks.Text = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[4].Text.ToString().Trim());

            Session["status"] = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[5].Text.ToString().Trim());
            string x = Session["status"].ToString();

            Session["lineId"] = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[8].Text.ToString().Trim());
            string i = Session["lineId"].ToString();

            if (Session["status"].ToString() == Constants.CON_UTILIZED_TAG )
            {
                isEnable(true);
            }
            else
            {
                isEnable(false);
            }

        }

        protected void gvemployeeProperty_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvemployeeProperty.PageIndex = e.NewPageIndex;
            gvemployeeProperty.DataSource = oPropertyDetailsSearchDataHandle.GetEmployeeProperty(txtEmploeeId.Text);
            gvemployeeProperty.DataBind();
        }

        #endregion

        #region methods

        private void isEnable(bool status)
        {
            txtAssignDate.Enabled = status;
            txtEmail.Enabled = status;
            txtRemarks.Enabled = status;
            btnUpdate.Enabled = status;
        }

        private void createPropertyBucket()
        {
            DataTable propertyBucket = new DataTable();
            propertyBucket.Columns.Add("PROPERTY_TYPE_ID", typeof(string));
            propertyBucket.Columns.Add("PROPERTY_ID", typeof(string));

            Session["propertyBucket"] = propertyBucket;
        }

        public void clear()
        {
            txtEmploeeId.Text = "";
            lblEmployeeName.Text = "";
            txtAssignDate.Text = "";
            txtReturnedDate.Text = "";
            txtEmail.Text = "";
            txtRemarks.Text = "";
            litDefaultProperty.Text = "";
        }

        private void clearPropertyBucket()
        {
            if (Session["propertyBucket"] != null)
            {
                DataTable dtProperty = (DataTable)Session["propertyBucket"];
                dtProperty.Rows.Clear();
            }
        }

        public void UpdatePropertyStatus()
        {
            EmployeePropertyDetailsDataHandler oEmployeePropertyDetailsDataHandler = new EmployeePropertyDetailsDataHandler();
            string employeeId = txtEmploeeId.Text;
            string TableString = "";
            DataTable propertyTable = new DataTable();
            propertyTable = oEmployeePropertyDetailsDataHandler.getDefaultPropertyList(employeeId).Copy();

            string companyId = oEmployeePropertyDetailsDataHandler.getEmployeeCompany(employeeId);

            DataTable dtProperties = (DataTable)Session["propertyBucket"];
            DataTable dtProperties2 = new DataTable();
            dtProperties2 = oEmployeePropertyDetailsDataHandler.getAssignProperties(employeeId).Copy();

            TableString += @"
                                <table style='width:100%;height:100%'>
                                    <tr>
                                        <th>Benefit Type</th>
                                        <th>Status</th>
                                      </tr>
                            ";

            for (int x = 0; x < propertyTable.Rows.Count; x++)
            {
                string typeId = propertyTable.Rows[x][0].ToString();
                string text = propertyTable.Rows[x][1].ToString();
                Label lbl = new Label();
                lbl.Text = "  Default ";
                foreach (DataRow dr in dtProperties2.Rows)
                {
                    string propertyId = dr["PROPERTY_TYPE_ID"].ToString();

                    if (propertyId == typeId)
                    {
                        lbl.Text = "  Assigned ";
                    }
                }
                TableString += "<tr><td>" + text + "</td><td>" + lbl.Text + "</td></tr>";
                //TableString += "<tr><td><a href=\"#" + "\"OnClick=\"window.open('../Property/WebFrmPropertySearch.aspx?TypeId=" + typeId + "','Search','width=300,height=100')" + "\"?TypeId=" + typeId + " style = \"text-decoration:none\"" + ">" + text + "</a></td><td>" + lbl.Text + "</td></tr>";
            }
            //TableString += "<tr><td><a href=\"#" + "\"OnClick=\"window.open('../Property/WebFrmPropertySearch.aspx?CompanyId=" + companyId + "','Search','width=300,height=100')" + "\"?style = \"text-decoration:none\"" + ">" + "Add Benefits ..." + "</a>";
            TableString += "<tr><td><a href=\"#" + "\"OnClick=\"openLOVWindow('../Property/WebFrmPropertySearch.aspx?CompanyId=" + companyId + "','Search','hfisInclude')" + "\"?style = \"text-decoration:none\"" + ">" + "Add Benefits ..." + "</a>";
            TableString += "<tr><td><a href=\"#" + "\"OnClick=\"window.open('../Property/WebFrmViewProperty.aspx?CompanyId=" + companyId + "','Search','width=300,height=100')" + "\"?style = \"text-decoration:none\"" + ">" + "View Added Benefits ..." + "</a>";

            DataTable availableProperty = new DataTable();
            availableProperty = oEmployeePropertyDetailsDataHandler.availabalePropertyLits(employeeId).Copy();

            for (int x = 0; x < availableProperty.Rows.Count; x++)
            {
                string typeId = availableProperty.Rows[x][2].ToString();
                string text = availableProperty.Rows[x][1].ToString();

                Label lbl = new Label();
                lbl.Text = " Not Assigned ";
                foreach (DataRow dr in dtProperties2.Rows)
                {
                    string propertyId = dr["PROPERTY_TYPE_ID"].ToString();
                    if (propertyId == typeId)
                    {
                        lbl.Text = "  Assigned ";
                    }
                }
                //TableString += "<tr><td><a href=\"#" + "\"OnClick=\"window.open('../Property/WebFrmPropertySearch.aspx?TypeId=" + typeId + "','Search','width=300,height=100')" + "\"?TypeId=" + typeId + " style = \"text-decoration:none\"" + ">" + text + "</a></td><td>" + lbl.Text + "</td></tr>";
            }
            TableString += @"
                                </table>
                            ";

            litDefaultProperty.Text = string.Empty;
            litDefaultProperty.Text = HttpUtility.HtmlDecode(TableString);
        }

        #endregion


    }
}