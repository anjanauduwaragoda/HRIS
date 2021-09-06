using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using DataHandler.HRISAlerts;

namespace GroupHRIS.HRISAlerts
{
    public partial class AlertPrivileges : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    Utility.Errorhandler.ClearError(lblMsg);
                    FillHRISUserDDL();
                }
                catch (Exception exp)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, lblMsg);
                }
            }
        }

        void FillHRISUserDDL()
        {
            ddlHrisRole.Items.Clear();

            HRISAlertsDataHandler OHADH = new HRISAlertsDataHandler();
            DataTable dt = new DataTable();
            dt = OHADH.populateCompanies().Copy();

            ddlHrisRole.Items.Add(new ListItem("", ""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string text = dt.Rows[i]["ROLE_NAME"].ToString();
                string value = dt.Rows[i]["ROLE_ID"].ToString();
                ddlHrisRole.Items.Add(new ListItem(text, value));
            }
        }

        void PopulateGrid()
        {
            try
            {
                Utility.Errorhandler.ClearError(lblMsg);

                HRISAlertsDataHandler OHADH = new HRISAlertsDataHandler();
                DataTable dt = new DataTable();
                dt = OHADH.populate(ddlHrisRole.SelectedValue.ToString().Trim()).Copy();
                Session["dt"] = dt.Copy();
                grdvReportGrid.DataSource = dt.Copy();
                grdvReportGrid.DataBind();
            }
            catch (Exception exp)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, lblMsg);
            }
        }

        protected void ddlHrisRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlHrisRole.SelectedIndex == 0)
            {
                Clear();
                return;
            }
            PopulateGrid();
        }

        protected void grdvReportGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int index = e.Row.RowIndex;
                    string status = (Session["dt"] as DataTable).Rows[index]["STATUS"].ToString();
                    if (status != "")
                    {
                        if (status == Constants.STATUS_ACTIVE_VALUE)
                        {
                            RadioButton rdbtnShow = (e.Row.FindControl("rdbtnShow") as RadioButton);
                            RadioButton rdbtnHide = (e.Row.FindControl("rdbtnHide") as RadioButton);

                            rdbtnShow.Checked = true;
                            rdbtnHide.Checked = false;
                        }
                        else
                        {
                            RadioButton rdbtnShow = (e.Row.FindControl("rdbtnShow") as RadioButton);
                            RadioButton rdbtnHide = (e.Row.FindControl("rdbtnHide") as RadioButton);

                            rdbtnShow.Checked = false;
                            rdbtnHide.Checked = true;
                        }
                    }
                    else
                    {
                        RadioButton rdbtnShow = (e.Row.FindControl("rdbtnShow") as RadioButton);
                        RadioButton rdbtnHide = (e.Row.FindControl("rdbtnHide") as RadioButton);

                        rdbtnShow.Checked = false;
                        rdbtnHide.Checked = true;
                    }
                }
            }
            catch (Exception exp)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, lblMsg);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Utility.Errorhandler.ClearError(lblMsg);

                DataTable dt = new DataTable();
                dt.Columns.Add("PRIVILEGE_ID");
                dt.Columns.Add("ALERT_ID");
                dt.Columns.Add("ALERT_NAME");
                dt.Columns.Add("STATUS");

                for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();

                    string PRIVILEGE_ID = "";
                    string ALERT_ID = "";
                    string ALERT_NAME = "";
                    string STATUS = "";

                    PRIVILEGE_ID = HttpUtility.HtmlDecode(grdvReportGrid.Rows[i].Cells[0].Text.Trim()).Trim();
                    ALERT_ID = HttpUtility.HtmlDecode(grdvReportGrid.Rows[i].Cells[1].Text.Trim()).Trim();
                    ALERT_NAME = HttpUtility.HtmlDecode(grdvReportGrid.Rows[i].Cells[2].Text.Trim()).Trim();

                    RadioButton rdbtnShow = (grdvReportGrid.Rows[i].FindControl("rdbtnShow") as RadioButton);
                    RadioButton rdbtnHide = (grdvReportGrid.Rows[i].FindControl("rdbtnHide") as RadioButton);

                    if ((rdbtnShow.Checked == true) && (rdbtnHide.Checked == false))
                    {
                        STATUS = Constants.STATUS_ACTIVE_VALUE;
                    }
                    else
                    {
                        STATUS = Constants.STATUS_INACTIVE_VALUE;
                    }

                    dr["PRIVILEGE_ID"] = PRIVILEGE_ID;
                    dr["ALERT_ID"] = ALERT_ID;
                    dr["ALERT_NAME"] = ALERT_NAME;
                    dr["STATUS"] = STATUS;

                    dt.Rows.Add(dr);
                }

                HRISAlertsDataHandler OHADH = new HRISAlertsDataHandler();
                OHADH.insert(ddlHrisRole.SelectedValue.ToString().Trim(), (Session["KeyUSER_ID"] as string).Trim(), dt.Copy());
                PopulateGrid();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_SAVED, lblMsg);
            }
            catch (Exception exp)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, lblMsg);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            try
            {
                Utility.Errorhandler.ClearError(lblMsg);

                if (ddlHrisRole.Items.Count > 0)
                {
                    ddlHrisRole.SelectedIndex = 0;
                }
                grdvReportGrid.DataSource = null;
                grdvReportGrid.DataBind();
            }
            catch (Exception exp)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, lblMsg);
            }
        }

        protected void rdSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);

            CheckBox rdSelectAll = ((CheckBox)grdvReportGrid.HeaderRow.FindControl("rdSelectAll"));


            if (rdSelectAll.Checked == true)
            {
                for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                {

                    ((CheckBox)grdvReportGrid.Rows[i].Cells[3].FindControl("rdbtnShow")).Checked = true;
                    ((CheckBox)grdvReportGrid.Rows[i].Cells[3].FindControl("rdbtnHide")).Checked = false;
                }
            }

        }

        protected void rdDeselectAll_CheckedChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);

            CheckBox rdDeselectAll = ((CheckBox)grdvReportGrid.HeaderRow.FindControl("rdDeselectAll"));


            if (rdDeselectAll.Checked == true)
            {
                for (int i = 0; i < grdvReportGrid.Rows.Count; i++)
                {

                    ((CheckBox)grdvReportGrid.Rows[i].Cells[3].FindControl("rdbtnShow")).Checked = false;
                    ((CheckBox)grdvReportGrid.Rows[i].Cells[3].FindControl("rdbtnHide")).Checked = true;
                }
            }
        }

        protected void rdbtnShow_CheckedChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);
        }

        protected void rdbtnHide_CheckedChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);
        }
    }
}