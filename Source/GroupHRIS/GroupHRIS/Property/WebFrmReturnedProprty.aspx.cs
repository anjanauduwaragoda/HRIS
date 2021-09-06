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
using DataHandler.Userlogin;
using System.Configuration;

namespace GroupHRIS.Property
{
    public partial class WebFrmReturnedProprty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               GetStatus();
            }
            if (IsPostBack)
            {
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
                        DataTable table = new DataTable();
                        string employeeId = txtEmploeeId.Text.Trim();

                        ReturnedPropertyDataHandler oReturnedPropertyDataHandler = new ReturnedPropertyDataHandler();
                        table = oReturnedPropertyDataHandler.getEmployeeName(employeeId);
                        lblEmployeeName.Text = table.Rows[0]["INITIALS_NAME"].ToString();
                        clear();
                        gvemployeeProperty.DataSource = oReturnedPropertyDataHandler.getEmployeeProperty(employeeId);
                        gvemployeeProperty.DataBind();
                    }
                }
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
            try
            {
                Errorhandler.ClearError(lblMessage);
                btnsave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

                int SelectedIndex = gvemployeeProperty.SelectedIndex;
                hfAssignId.Value = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());
                hfPropertyId.Value = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
                hfmail.Value = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[5].Text.ToString().Trim());

                txtPropertyName.Text = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());
                txtPropertyName.Enabled = false;
                txtAssignDate.Text = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
                txtAssignDate.Enabled = false;
                txtReturnedDate.Text = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[4].Text.ToString().Trim());
                
                string status = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[6].Text.ToString().Trim());
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));

                txtRemarks.Text = Server.HtmlDecode(gvemployeeProperty.Rows[SelectedIndex].Cells[7].Text.ToString().Trim());

                if (status == Constants.CON_RETURNED_TAG || status == Constants.CON_REMOVE_TAG)
                {
                    txtReturnedDate.Enabled = false;
                    ddlStatus.Enabled = false;
                    btnsave.Enabled = false;
                }
                else
                {
                    txtReturnedDate.Enabled = true;
                    ddlStatus.Enabled = true;
                    btnsave.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMessage);
            }
        }

        protected void gvemployeeProperty_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string employeeId = txtEmploeeId.Text.Trim();
            gvemployeeProperty.PageIndex = e.NewPageIndex;
            ReturnedPropertyDataHandler oReturnedPropertyDataHandler = new ReturnedPropertyDataHandler();
            gvemployeeProperty.DataSource = oReturnedPropertyDataHandler.getEmployeeProperty(employeeId);
            gvemployeeProperty.DataBind();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            btnsave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            txtEmploeeId.Text = "";
            lblEmployeeName.Text = "";
            txtPropertyName.Text = "";
            txtAssignDate.Text = "";
            txtReturnedDate.Text = "";
            ddlStatus.SelectedIndex = 0;
            txtRemarks.Text = "";
            txtReturnedDate.Enabled = false;
            ddlStatus.Enabled = false;

            gvemployeeProperty.DataSource = null;
            gvemployeeProperty.DataBind();
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            ReturnedPropertyDataHandler oReturnedPropertyDataHandler = new ReturnedPropertyDataHandler();
            try
            {
                string employeeId = txtEmploeeId.Text.Trim();
                string propertyId = hfPropertyId.Value;
                string assignId = hfAssignId.Value;
                string returnDate = txtReturnedDate.Text;
                //DateTime txtMyDate = DateTime.ParseExact(txtReturnedDate.Text, "M/d/yyyy", CultureInfo.InvariantCulture);
                string status = ddlStatus.SelectedValue;
                string remarks = txtRemarks.Text.Trim();
                string logUser = Session["KeyUSER_ID"].ToString();

                if (ddlStatus.SelectedValue != Constants.CON_RETURNED_STATUS)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Update Benefit Status to Return ", lblMessage);
                    return;
                }

                if (btnsave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    Boolean isValied = CommonUtils.isValidTimeRange(txtAssignDate.Text,returnDate);
                    if (isValied)
                    {
                        Boolean IsUpdated = oReturnedPropertyDataHandler.update(assignId, propertyId, returnDate, status, remarks, logUser);
                        EmailHandler.SendHTMLMail("Return Benefit(s) ", hfmail.Value, "Benefit Return", sendMail());
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Updated", lblMessage);
                    }
                    else
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Invalid date range", lblMessage);
                    }
                }
                gvemployeeProperty.DataSource = oReturnedPropertyDataHandler.getEmployeeProperty(employeeId);
                gvemployeeProperty.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        public void GetStatus()
        {
            ddlStatus.Items.Insert(0, new ListItem("", ""));
            ddlStatus.Items.Insert(1, new ListItem(Constants.CON_RETURNED_TAG, Constants.CON_RETURNED_STATUS));
            ddlStatus.Items.Insert(2, new ListItem(Constants.CON_UTILIZED_TAG, Constants.CON_UTILIZED_STATUS));
            //ddlStatus.Items.Insert(3, new ListItem(Constants.CON_DISCARD_TAG, Constants.CON_DISCARD_STATUS));
        }

        public void clear()
        {
            Errorhandler.ClearError(lblMessage);
            txtPropertyName.Text = "";
            txtAssignDate.Text = "";
            txtReturnedDate.Text = "";
            ddlStatus.SelectedIndex = 0;
            txtRemarks.Text = "";
        }

        public string sendMail()
        {
            PasswordHandler crpto = new PasswordHandler();
            ReturnedPropertyDataHandler oReturnedPropertyDataHandler = new ReturnedPropertyDataHandler();
            string name = "";
            string email = hfmail.Value;
            string var1 = String.Empty;


            var1 = "Dear Mr/Ms " + name + "," + Environment.NewLine + Environment.NewLine + "</br></br>";
            var1 += lblEmployeeName.Text + " have been returned following property. " + Environment.NewLine + Environment.NewLine + Environment.NewLine + "</br></br>";

            DataTable filteredData = oReturnedPropertyDataHandler.returnProperty(hfAssignId.Value, hfPropertyId.Value, txtEmploeeId.Text);

            DataTable dt = new DataTable();
            dt.Columns.Add("Benefit");
            dt.Columns.Add("RefereneNo");
            dt.Columns.Add("Model");
            dt.Columns.Add("Serial");
            dt.Columns.Add("AssignDate");
            dt.Columns.Add("ReturnDate");

            for (int i = 0; i < filteredData.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Benefit"] = filteredData.Rows[i]["DESCRIPTION"].ToString();
                dr["RefereneNo"] = filteredData.Rows[i]["REFERENCE_NO"].ToString();
                dr["Model"] = filteredData.Rows[i]["MODEL"].ToString();
                dr["Serial"] = filteredData.Rows[i]["SERIAL_NO"].ToString();
                dr["AssignDate"] = filteredData.Rows[i]["ASSIGNED_DATE"].ToString();
                dr["ReturnDate"] = filteredData.Rows[i]["RETURNED_DATE"].ToString();
                dt.Rows.Add(dr);
            }

            
            filteredData = dt;
            string var = "<table style='border: 1px solid black;border-collapse: collapse;'>";
            //add header row
            var += "<tr>";
            for (int i = 0; i < filteredData.Columns.Count; i++)
                var += "<th  style='border: 1px solid black;border-collapse: collapse;'>&nbsp;&nbsp;" + filteredData.Columns[i].ColumnName + "</th>";
            var += "</tr>";
            //add rows
            for (int i = 0; i < filteredData.Rows.Count; i++)
            {
                var += "<tr>";
                for (int j = 0; j < filteredData.Columns.Count; j++)
                    var += "<td  style='border: 1px solid black;border-collapse: collapse;'>&nbsp;&nbsp;&nbsp;" + filteredData.Rows[i][j].ToString() + "</td>";
                var += "</tr>";
            }
            var += "</table>";

            var1 += var;
            
            var1 += Environment.NewLine + "</br></br>Thank You.</br></br>";
            var1 += Environment.NewLine + "This is a system generated mail." + Environment.NewLine;

            return var1;
        }

    }
}