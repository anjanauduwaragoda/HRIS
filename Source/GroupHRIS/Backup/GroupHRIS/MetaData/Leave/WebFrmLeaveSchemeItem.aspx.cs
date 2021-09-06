using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.MetaData;
using System.Data;

namespace GroupHRIS.MetaData.Leave
{
    public partial class WebFrmLeaveSchemeItem : System.Web.UI.Page
    {
        DataTable dataTable;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LeaveTypeDataHandler leaveType = new LeaveTypeDataHandler();
                    LeaveSchemeTypeDropDownList.DataSource = leaveType.populate();
                    LeaveSchemeTypeDropDownList.DataTextField = "LEAVE_TYPE_NAME";
                    LeaveSchemeTypeDropDownList.DataValueField = "LEAVE_TYPE_ID";
                    LeaveSchemeTypeDropDownList.DataBind();

                    dataTable = new DataTable();
                    dataTable.Columns.Add("Leave Scheme Type");
                    dataTable.Columns.Add("Number of Days");
                    dataTable.Columns.Add("Remarks");

                    Session["dt"] = dataTable;

                }
            }
            catch (Exception exp)
            {
                Response.Write(exp.Message);
            }
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            if (NumberOfDaysTextBox.Text != "")
            {
                dataTable = (DataTable)Session["dt"];

                DataRow dataRow = dataTable.NewRow();
                dataRow["Leave Scheme Type"] = LeaveSchemeTypeDropDownList.SelectedValue;
                dataRow["Number of Days"] = NumberOfDaysTextBox.Text;
                dataRow["Remarks"] = RemarksTextBox.Text;
                dataTable.Rows.Add(dataRow);

                LeaveSchemeItemGridView.DataSource = dataTable;
                LeaveSchemeItemGridView.DataBind();

                NumberOfDaysTextBox.Text = "";
                RemarksTextBox.Text = "";

                Session["dt"] = dataTable;

                if (dataTable.Rows.Count == 0)
                {
                    ButtonPanel.Visible = false;
                }
                else
                {
                    ButtonPanel.Visible = true;
                }
            }
        }
    }
}