using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using GroupHRIS.Utility;
using DataHandler.Attendance;

namespace GroupHRIS.Attendance
{
    public partial class AttendanceReconcile : System.Web.UI.Page
    {
        AttendanceReconcileDataHandler attendanceReconcile = new AttendanceReconcileDataHandler();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                tblAttendance.Visible = false;
            }
            
            //txtEmpId.Text = hfVal.Value;
            //lblempName.Text = attendanceReconcile.returnEmpName(hfVal.Value);

            //if ((string)Session["empId"] != hfVal.Value)
            //{
            //    grdAttendance.DataSource = null;
            //    grdAttendance.DataBind();
            //    tblAttendance.Visible = false;
            //}

            if (IsPostBack)
            {
                if (hfCaller.Value == "txtemployee")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtEmpId.Text = hfVal.Value;
                        lblempName.Text = attendanceReconcile.returnEmpName(hfVal.Value);
                    }
                    if (txtEmpId.Text != "")
                    {
                        //Postback Methods                        
                        grdAttendance.DataSource = null;
                        grdAttendance.DataBind();
                        tblAttendance.Visible = false;
                        txtFrmDate.Text = "";
                        txtToDate.Text = "";
                        Errorhandler.ClearError(lblmsg);
                    }
                }
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string empId = txtEmpId.Text.Trim();
                string frmDate = txtFrmDate.Text.Trim();
                string toDate = txtToDate.Text.Trim();

                bool status = CommonUtils.isValidDateRange(frmDate, toDate);

                Errorhandler.ClearError(lblmsg);

                if (status == true)
                {
                    DataTable attData = attendanceReconcile.reconcileAttendance(empId, frmDate, toDate);

                    if (attData.Rows.Count > 0)
                    {
                        grdAttendance.DataSource = attData;
                        grdAttendance.DataBind();
                        Session["empId"] = empId;
                    }
                    else
                    {
                        grdAttendance.DataSource = null;
                        grdAttendance.DataBind();
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "No data found for selected date range", lblmsg);
                        return;
                    }
                }
                else
                {
                    grdAttendance.DataSource = null;
                    grdAttendance.DataBind();
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Invalid Date Range ", lblmsg);
                    return;
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdAttendance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string empId = txtEmpId.Text.Trim();
            string frmDate = txtFrmDate.Text.Trim();
            string toDate = txtToDate.Text.Trim();

            try
            {
                DataTable attData = attendanceReconcile.reconcileAttendance(empId, frmDate, toDate);

                grdAttendance.PageIndex = e.NewPageIndex;
                grdAttendance.DataSource = attData;
                grdAttendance.DataBind();
                attendanceReconcile = null;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }

        }

        protected void grdAttendance_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Errorhandler.ClearError(lblMessage);
                tblAttendance.Visible = true;

                int SelectedIndex = grdAttendance.SelectedIndex;

                txtAttDate.Text = Server.HtmlDecode(grdAttendance.Rows[SelectedIndex].Cells[0].Text.ToString().Trim());
                txtAttTime.Text = Server.HtmlDecode(grdAttendance.Rows[SelectedIndex].Cells[1].Text.ToString().Trim());
                txtAttDirection.Text = Server.HtmlDecode(grdAttendance.Rows[SelectedIndex].Cells[2].Text.ToString().Trim());

                if (txtAttDirection.Text == "IN")
                {
                    trReson.Visible = false;
                }
                else
                {
                    trReson.Visible = true;
                    txtReason.Text = Server.HtmlDecode(grdAttendance.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
                }
                txtRemarks.Text = "";

                
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdAttendance, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void btnCancelS_Click(object sender, EventArgs e)
        {
            tblAttendance.Visible = false;
            Errorhandler.ClearError(lblMessage);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string empId = txtEmpId.Text.Trim();
                string attDate = txtAttDate.Text.Trim();
                string attTime = txtAttTime.Text.Trim();
                string direction = txtAttDirection.Text;
                string remarks = txtRemarks.Text;
                string user = Session["KeyUSER_ID"].ToString();

                bool isRemove = attendanceReconcile.removeAttendance(empId, attDate, attTime, direction,remarks, user);
                if (isRemove)
                {
                    DataTable attData = attendanceReconcile.reconcileAttendance(empId, txtFrmDate.Text, txtToDate.Text);
                    grdAttendance.DataSource = attData;
                    grdAttendance.DataBind();
                    CommonVariables.MESSAGE_TEXT = "Success fully removed";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMessage);
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Record not removed";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                }

                txtAttDate.Text = "";
                txtAttTime.Text = "";
                txtAttDirection.Text = "";
                txtReason.Text = "";
                txtRemarks.Text = "";
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
            tblAttendance.Visible = false;
            Errorhandler.ClearError(lblmsg);
        }

        public void clear()
        {
            grdAttendance.DataSource = null;
            grdAttendance.DataBind();
            txtEmpId.Text = "";
            lblempName.Text = "";
            txtFrmDate.Text = "";
            txtToDate.Text = "";
        }


    }
}