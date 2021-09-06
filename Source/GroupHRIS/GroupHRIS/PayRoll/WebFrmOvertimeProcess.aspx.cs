using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Payroll;
using System.Data;
using GroupHRIS.Utility;
using Common;
using System.Globalization;

namespace GroupHRIS.PayRoll
{
    public partial class WebFrmOvertimeProcess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);

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
                        GetCompany();
                        GetEmployeeName();
                        LoadAmounts();
                    }
                }
            }
            else
            {
                LoadYears();
                LoadMonths();
            }
        }

        private void LoadYears()
        {
            ddlYear.Items.Clear();
            int currentYear = DateTime.Now.Year;
            //ddlYear.Items.Add(new ListItem("", ""));
            for (int i = currentYear; i >= (currentYear - 50); i--)
            {
                ddlYear.Items.Add(new ListItem(i.ToString().Trim(), i.ToString().Trim()));
            }
        }

        private void LoadMonths()
        {
            //ddlMonth.Items.Add(new ListItem("", ""));

            ddlMonth.Items.Clear();

            for (int i = 1; i <= 12; i++)
            {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                ddlMonth.Items.Add(new ListItem(monthName, i.ToString()));
            }

            if (System.DateTime.Now.Month != 1)
            {
                ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(((System.DateTime.Now.Month) - 1).ToString().Trim()));
            }
            else
            {
                ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByValue(((System.DateTime.Now.Year) - 1).ToString().Trim()));
                ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue("12"));
            }
        }

        private void LoadAmounts()
        {
            OvertimeProcessDataHandler OPDH = new OvertimeProcessDataHandler();
            try
            {
                if (txtEmploeeId.Text != String.Empty)
                {
                    string YearMonth = ddlYear.SelectedItem.Text.Trim() + ddlMonth.SelectedValue.ToString().PadLeft(2, '0');
                    //string YearMonth = ddlYear.SelectedItem.Text.Trim() + ddlMonth.SelectedValue.ToString().PadLeft(2);
                    Boolean isNormalOTEnabled = false;
                    Boolean isDoubleOTEnabled = false;
                    Boolean isAttendaceIncentiveEnabled = false;

                    string NOTPreAddedBy = string.Empty;
                    string NOTPreAddedDate = string.Empty;

                    string DOTPreAddedBy = string.Empty;
                    string DOTPreAddedDate = string.Empty;

                    string AIPreAddedBy = string.Empty;
                    string AIPreAddedDate = string.Empty;

                    //Load Normal OT
                    txtNormalOT.Text = OPDH.GetAmount(YearMonth, txtEmploeeId.Text.Trim(), Constants.CON_OT_CATEGORY_OVERTIME_TAG, Constants.CON_OT_CATEGORY_OVERTIME_NORMAL_OT_ID, out isNormalOTEnabled, out NOTPreAddedBy, out NOTPreAddedDate).Trim();
                    //Load Double OT
                    txtDoubleOT.Text = OPDH.GetAmount(YearMonth, txtEmploeeId.Text.Trim(), Constants.CON_OT_CATEGORY_OVERTIME_TAG, Constants.CON_OT_CATEGORY_OVERTIME_DOUBLE_OT_ID, out isDoubleOTEnabled, out DOTPreAddedBy, out DOTPreAddedDate).Trim();
                    //Load Attendance Incentive
                    txtAttendanceIncentive.Text = OPDH.GetAmount(YearMonth, txtEmploeeId.Text.Trim(), Constants.CON_OT_CATEGORY_ALLOWANCE_TAG, Constants.CON_OT_CATEGORY_ATTENDANCE_INCENTIVE_ID, out isAttendaceIncentiveEnabled, out AIPreAddedBy, out AIPreAddedDate).Trim();
                    //Load Remarks
                    txtRemarks.Text = OPDH.GetRemarks(txtEmploeeId.Text.Trim(), YearMonth);


                    Session["PreviousNormalOT"] = txtNormalOT.Text.Trim();
                    Session["PreviousDoubleOT"] = txtDoubleOT.Text.Trim();
                    Session["PreviousAI"] = txtAttendanceIncentive.Text.Trim();

                    Session["NOTPreAddedBy"] = NOTPreAddedBy;
                    Session["NOTPreAddedDate"] = NOTPreAddedDate;

                    Session["DOTPreAddedBy"] = DOTPreAddedBy;
                    Session["DOTPreAddedDate"] = DOTPreAddedDate;

                    Session["AIPreAddedBy"] = AIPreAddedBy;
                    Session["AIPreAddedDate"] = AIPreAddedDate;


                    if (isNormalOTEnabled)
                    {
                        Session["isNormalOTEnabled"] = "1";
                    }
                    else
                    {
                        Session["isNormalOTEnabled"] = "0";
                    }

                    if (isDoubleOTEnabled)
                    {
                        Session["isDoubleOTEnabled"] = "1";
                    }
                    else
                    {
                        Session["isDoubleOTEnabled"] = "0";
                    }

                    if (isAttendaceIncentiveEnabled)
                    {
                        Session["isAttendaceIncentiveEnabled"] = "1";
                    }
                    else
                    {
                        Session["isAttendaceIncentiveEnabled"] = "0";
                    }


                    OTCategoryCheck();
                }
            }
            catch (Exception ex)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, StatusLabel);
            }
            finally
            {
                OPDH = null;
            }
        }

        private void OTCategoryCheck()
        {
            OvertimeProcessDataHandler OPDH = new OvertimeProcessDataHandler();
            try
            {
                string OTRoleCategory = OPDH.GetRoleOTCategory(txtEmploeeId.Text.Trim());
                if (OTRoleCategory == Constants.CON_ROLE_OT_CATEGORY_MANAGERIAL)
                {
                    txtNormalOT.Enabled = false;
                    txtDoubleOT.Enabled = false;
                    txtAttendanceIncentive.Enabled = false;


                    rbtnNOTActive.Enabled = false;
                    rbtnNOTDeactive.Enabled = false;
                    rbtnDOTActive.Enabled = false;
                    rbtnDOTDeactive.Enabled = false;

                    rbtnAttendanceIncentiveActive.Enabled = false;
                    rbtnAttendanceIncentiveDeactive.Enabled = false;

                }
                else if (OTRoleCategory == Constants.CON_ROLE_OT_CATEGORY_EXECUTIVE)
                {
                    txtNormalOT.Enabled = false;
                    txtDoubleOT.Enabled = false;
                    txtAttendanceIncentive.Enabled = true;


                    rbtnNOTActive.Enabled = false;
                    rbtnNOTDeactive.Enabled = false;
                    rbtnDOTActive.Enabled = false;
                    rbtnDOTDeactive.Enabled = false;

                    rbtnAttendanceIncentiveActive.Enabled = true;
                    rbtnAttendanceIncentiveDeactive.Enabled = true;
                }
                else if (OTRoleCategory == Constants.CON_ROLE_OT_CATEGORY_NON_EXECUTIVE)
                {
                    txtNormalOT.Enabled = true;
                    txtDoubleOT.Enabled = true;
                    txtAttendanceIncentive.Enabled = false;


                    rbtnNOTActive.Enabled = true;
                    rbtnNOTDeactive.Enabled = true;
                    rbtnDOTActive.Enabled = true;
                    rbtnDOTDeactive.Enabled = true;

                    rbtnAttendanceIncentiveActive.Enabled = false;
                    rbtnAttendanceIncentiveDeactive.Enabled = false;
                }
                else
                {
                    txtNormalOT.Enabled = false;
                    txtDoubleOT.Enabled = false;
                    txtAttendanceIncentive.Enabled = false;


                    rbtnNOTActive.Enabled = false;
                    rbtnNOTDeactive.Enabled = false;
                    rbtnDOTActive.Enabled = false;
                    rbtnDOTDeactive.Enabled = false;

                    rbtnAttendanceIncentiveActive.Enabled = false;
                    rbtnAttendanceIncentiveDeactive.Enabled = false;
                }


                if ((Session["isNormalOTEnabled"] as string) == "0")
                {
                    //txtNormalOT.Enabled = false;
                    //rbtnNOTActive.Enabled = false;
                    //rbtnNOTDeactive.Enabled = false;

                    rbtnNOTActive.Checked = false;
                    rbtnNOTDeactive.Checked = true;

                }
                else
                {
                    rbtnNOTActive.Checked = true;
                    rbtnNOTDeactive.Checked = false;
                }

                if ((Session["isDoubleOTEnabled"] as string) == "0")
                {
                    //txtDoubleOT.Enabled = false;
                    //rbtnDOTActive.Enabled = false;
                    //rbtnDOTDeactive.Enabled = false;

                    rbtnDOTActive.Checked = false;
                    rbtnDOTDeactive.Checked = true;
                }
                else
                {
                    rbtnDOTActive.Checked = true;
                    rbtnDOTDeactive.Checked = false;
                }

                if ((Session["isAttendaceIncentiveEnabled"] as string) == "0")
                {
                    //txtAttendanceIncentive.Enabled = false;
                    //rbtnAttendanceIncentiveActive.Enabled = false;
                    //rbtnAttendanceIncentiveDeactive.Enabled = false;

                    rbtnAttendanceIncentiveActive.Checked = false;
                    rbtnAttendanceIncentiveDeactive.Checked = true;
                }
                else
                {
                    rbtnAttendanceIncentiveActive.Checked = true;
                    rbtnAttendanceIncentiveDeactive.Checked = false;
                }

                //bool isNormalOTEnabled = (Session["isNormalOTEnabled"] as bool);
                //Session["isDoubleOTEnabled"] = isDoubleOTEnabled;
                //Session["isAttendaceIncentiveEnabled"] = isAttendaceIncentiveEnabled;


                if (rbtnNOTDeactive.Enabled)
                {
                    if (rbtnNOTDeactive.Checked)
                    {
                        txtNormalOT.Enabled = false;
                    }
                    else
                    {
                        txtNormalOT.Enabled = true;
                    }
                }

                if (rbtnDOTDeactive.Enabled)
                {
                    if (rbtnDOTDeactive.Checked)
                    {
                        txtDoubleOT.Enabled = false;
                    }
                    else
                    {
                        txtDoubleOT.Enabled = true;
                    }
                }

                if (rbtnAttendanceIncentiveDeactive.Enabled)
                {
                    if (rbtnAttendanceIncentiveDeactive.Checked)
                    {
                        txtAttendanceIncentive.Enabled = false;
                    }
                    else
                    {
                        txtAttendanceIncentive.Enabled = true;
                    }
                }


            }
            catch (Exception ex)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, StatusLabel);
            }
            finally
            {
                OPDH = null;
            }
        }

        protected void txtEmploeeId_TextChanged(object sender, EventArgs e)
        {
            GetCompany();
            GetEmployeeName();

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            OvertimeProcessDataHandler OPDH = new OvertimeProcessDataHandler();

            string empId = txtEmploeeId.Text.ToString();
            string logUser = Session["KeyUSER_ID"].ToString();
            string remarks = txtRemarks.Text.ToString();
            string EPF = String.Empty;

            try
            {
                Errorhandler.ClearError(StatusLabel);
                EPF = OPDH.GetEPFNumber(empId).Trim();

                string TransMonth = ddlYear.SelectedItem.Text.Trim() + ddlMonth.SelectedValue.ToString().PadLeft(2, '0');
                string NormalOTAmount = txtNormalOT.Text.Trim();
                string DoubleOTAmount = txtDoubleOT.Text.Trim();
                string AttendanceIncentiveAmount = txtAttendanceIncentive.Text.Trim();
                Boolean IsNormalOTEnable = rbtnNOTActive.Checked;
                Boolean IsDoubleOTEnable = rbtnDOTActive.Checked;
                Boolean IsAttendanceIncentiveEnable = rbtnAttendanceIncentiveActive.Checked;
                string Remarks = txtRemarks.Text.Trim();
                string ModifiedBy = (Session["KeyUSER_ID"] as string);


                string PreviousNormalOT = (Session["PreviousNormalOT"] as string);
                string PreviousDoubleOT = (Session["PreviousDoubleOT"] as string);
                string PreviousAI = (Session["PreviousAI"] as string);

                string NOTPreAddedBy = (Session["NOTPreAddedBy"] as string);
                string NOTPreAddedDate = (Session["NOTPreAddedDate"] as string);

                string DOTPreAddedBy = (Session["DOTPreAddedBy"] as string);
                string DOTPreAddedDate = (Session["DOTPreAddedDate"] as string);

                string AIPreAddedBy = (Session["AIPreAddedBy"] as string);
                string AIPreAddedDate = (Session["AIPreAddedDate"] as string);


                OPDH.Insert(TransMonth, empId, EPF, NormalOTAmount, DoubleOTAmount, AttendanceIncentiveAmount, IsNormalOTEnable, IsDoubleOTEnable, IsAttendanceIncentiveEnable, Remarks, ModifiedBy, PreviousNormalOT, PreviousDoubleOT, PreviousAI, NOTPreAddedBy, NOTPreAddedDate, DOTPreAddedBy, DOTPreAddedDate, AIPreAddedBy, AIPreAddedDate);

                //Boolean status = oOvertimeProcessDataHandler.UpdateTransaction(empId, amount, remarks, logUser, "category", "subcategory");
                Errorhandler.GetError("1", "Successfully Updated", StatusLabel);
            }
            catch (Exception ex)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, StatusLabel);
            }
            finally
            {
                OPDH = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);


            txtEmploeeId.Text = String.Empty;
            lblCompany.Text = String.Empty;
            lblEmployeeName.Text = String.Empty;
            txtNormalOT.Text = String.Empty;
            txtDoubleOT.Text = String.Empty;
            txtAttendanceIncentive.Text = String.Empty;

            txtNormalOT.Enabled = true;
            txtDoubleOT.Enabled = true;
            txtAttendanceIncentive.Enabled = true;

            rbtnNOTActive.Checked = true;
            rbtnDOTActive.Checked = true;
            rbtnAttendanceIncentiveActive.Checked = true;
            rbtnNOTActive.Enabled = true;
            rbtnDOTActive.Enabled = true;
            rbtnAttendanceIncentiveActive.Enabled = true;

            rbtnNOTDeactive.Checked = false;
            rbtnDOTDeactive.Checked = false;
            rbtnAttendanceIncentiveDeactive.Checked = false;
            rbtnNOTDeactive.Enabled = true;
            rbtnDOTDeactive.Enabled = true;
            rbtnAttendanceIncentiveDeactive.Enabled = true;

            txtRemarks.Text = String.Empty;
            LoadYears();
            LoadMonths();

        }

        public void GetCompany()
        {
            string empId = txtEmploeeId.Text;
            OvertimeProcessDataHandler oOvertimeProcessDataHandler = new OvertimeProcessDataHandler();
            DataTable table = new DataTable();
            table = oOvertimeProcessDataHandler.GetEmployeeCompany(empId);
            lblCompany.Text = table.Rows[0]["COMP_NAME"].ToString();
        }

        public void GetEmployeeName()
        {
            string empId = txtEmploeeId.Text;
            OvertimeProcessDataHandler oOvertimeProcessDataHandler = new OvertimeProcessDataHandler();
            DataTable table = new DataTable();
            table = oOvertimeProcessDataHandler.GetEmployeeName(empId);
            lblEmployeeName.Text = table.Rows[0]["INITIALS_NAME"].ToString();
        }

        public void Status(Boolean status)
        {


        }

        protected void gvOvertimeProcess_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void lblViewSummary_Click(object sender, EventArgs e)
        {
            //ExportToCSV.
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadAmounts();
            }
            catch (Exception ex)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, StatusLabel);
            }
            finally
            {

            }
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadAmounts();
            }
            catch (Exception ex)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, StatusLabel);
            }
            finally
            {

            }
        }

        protected void rbtnNOTActive_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnNOTActive.Checked == true)
            {
                txtNormalOT.Enabled = true;
            }
            else
            {
                txtNormalOT.Enabled = false;
            }
        }

        protected void rbtnNOTDeactive_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnNOTActive.Checked == true)
            {
                txtNormalOT.Enabled = true;
            }
            else
            {
                txtNormalOT.Enabled = false;
            }
        }

        protected void rbtnDOTActive_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnDOTActive.Checked == true)
            {
                txtDoubleOT.Enabled = true;
            }
            else
            {
                txtDoubleOT.Enabled = false;
            }
        }

        protected void rbtnDOTDeactive_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnDOTActive.Checked == true)
            {
                txtDoubleOT.Enabled = true;
            }
            else
            {
                txtDoubleOT.Enabled = false;
            }
        }

        protected void rbtnAttendanceIncentiveActive_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnAttendanceIncentiveActive.Checked == true)
            {
                txtAttendanceIncentive.Enabled = true;
            }
            else
            {
                txtAttendanceIncentive.Enabled = false;
            }
        }

        protected void rbtnAttendanceIncentiveDeactive_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnAttendanceIncentiveActive.Checked == true)
            {
                txtAttendanceIncentive.Enabled = true;
            }
            else
            {
                txtAttendanceIncentive.Enabled = false;
            }
        }
    }
}