using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Attendance;
using System.Data;
using Common;

namespace GroupHRIS.EmployeeProfile
{
    public partial class EmployeeSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            }
        }

        private void display_summary(string KeyEMPLOYEE_ID)
        {
            AttendanceSummaryHandler atendanceSummaryHandler = new AttendanceSummaryHandler();
            DataTable sAttendance = new DataTable();
            string mTable = "";

            DateTime  sFromdate = DateTime.Parse(txtfromdate.Text.ToString()) ;
            DateTime sTodate = DateTime.Parse(txttodate.Text.ToString());
            
            string sIN_DATE = "";
            string sIN_TIME = "";
            string sOUT_DATE = "";
            string sOUT_TIME = "";
            string sLATE_MINUTES = "";
            string sEARLY_MINUTES = "";
            string sNUMBER_OF_DAYS = "";
            string sIN_LOCATION = "";
            string sOUT_LOCATION = "";
            string sREMARK = "";
            string sOT_HOURS = "";
            string sIS_ABSENT = "";

            try
            {
                sAttendance = atendanceSummaryHandler.populateEmpProfile(sFromdate.ToString("yyyy/MM/dd"), sTodate.ToString("yyyy/MM/dd"), KeyEMPLOYEE_ID);
                for (int x = 0; x <= sAttendance.Rows.Count - 1; x++)
                {

                    if (x == 0)
                    {

                        mTable = "";
                        mTable = mTable + "<Table class='EmployeeTransferTB' style='width:800px'>";
                        mTable = mTable + "<Tr>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:80px'>";
                        mTable = mTable + "IN Date";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:80px'>";
                        mTable = mTable + "IN Time";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:100px'>";
                        mTable = mTable + "IN Location";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:80px'>";
                        mTable = mTable + "Late Minutes";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:80px'>";
                        mTable = mTable + "OUT Date";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:80px'>";
                        mTable = mTable + "OUT Time";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:100px'>";
                        mTable = mTable + "OUT Location";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:80px'>";
                        mTable = mTable + "Early Minutes";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:60px'>";
                        mTable = mTable + "Status";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:60px'>";
                        mTable = mTable + "Extra Minutes";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "</Tr>";
                    }

                    sIN_DATE = DateTime.Parse(sAttendance.Rows[x]["IN_DATE"].ToString()).ToString("yyyy/MM/dd");
                    sIN_TIME = sAttendance.Rows[x]["IN_TIME"].ToString();
                    sOUT_DATE = DateTime.Parse(sAttendance.Rows[x]["OUT_DATE"].ToString()).ToString("yyyy/MM/dd");
                    sOUT_TIME = sAttendance.Rows[x]["OUT_TIME"].ToString();
                    sLATE_MINUTES = sAttendance.Rows[x]["LATE_MINUTES"].ToString();
                    sEARLY_MINUTES = sAttendance.Rows[x]["EARLY_MINUTES"].ToString();
                    sNUMBER_OF_DAYS = sAttendance.Rows[x]["NUMBER_OF_DAYS"].ToString();
                    if (sNUMBER_OF_DAYS == "")
                    {
                        sNUMBER_OF_DAYS = "0";
                    }
                    sIN_LOCATION = sAttendance.Rows[x]["IN_LOCATION"].ToString();
                    sOUT_LOCATION = sAttendance.Rows[x]["OUT_LOCATION"].ToString();
                    sREMARK = sAttendance.Rows[x]["REMARK"].ToString();
                    sOT_HOURS = sAttendance.Rows[x]["OT_HOURS"].ToString();
                    sIS_ABSENT = sAttendance.Rows[x]["IS_ABSENT"].ToString();

                    if (double.Parse(sNUMBER_OF_DAYS) > 0)
                    {
                        // LEAVE
                        mTable = mTable + "<Tr bgcolor =#FFCC00>";
                    }
                    else if (sIS_ABSENT == "1") // ABSET
                    {
                        mTable = mTable + "<Tr bgcolor=#FF0000>";
                    }
                    else if (sIS_ABSENT == "2") // MISSING
                    {
                        mTable = mTable + "<Tr bgcolor=#009900>";
                    }
                    else
                    {
                        // OK
                        mTable = mTable + "<Tr bgcolor=#FFFFFF>";
                    }

                    mTable = mTable + "<Td class='EmployeeTransferTB' style='width:80px'>";
                    mTable = mTable + sIN_DATE;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td class='EmployeeTransferTB' style='width:80px'>";
                    mTable = mTable + sIN_TIME;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td class='EmployeeTransferTB' style='width:100px'>";
                    mTable = mTable + sIN_LOCATION;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td class='EmployeeTransferTB' style='width:80px'>";
                    mTable = mTable + sLATE_MINUTES;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td class='EmployeeTransferTB' style='width:80px'>";
                    mTable = mTable + sOUT_DATE;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td class='EmployeeTransferTB' style='width:80px'>";
                    mTable = mTable + sOUT_TIME;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td class='EmployeeTransferTB' style='width:100px'>";
                    mTable = mTable + sOUT_LOCATION;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td class='EmployeeTransferTB' style='width:80px'>";
                    mTable = mTable + sEARLY_MINUTES;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td class='EmployeeTransferTB' style='width:60px'>";
                    mTable = mTable + sNUMBER_OF_DAYS;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td class='EmployeeTransferTB' style='width:60px'>";
                    mTable = mTable + sOT_HOURS;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "</Tr>";

                    if (x == sAttendance.Rows.Count - 1)
                    {
                        mTable = mTable + "</Table>";
                        LiteralSE1.Text = LiteralSE1.Text + mTable;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                atendanceSummaryHandler = null;
                sAttendance.Dispose();
                sAttendance = null;
            }

        }

        protected void btnview_Click(object sender, EventArgs e)
        {
            try
            {
                //string KeyEMPLOYEE_ID = "EP000001";
                //string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                string KeyEMPLOYEE_ID = Request.QueryString["mEmpProfileID"];
                string sFromDate = txtfromdate.Text.ToString();
                string sToDate = txttodate.Text.ToString();
                LiteralSE1.Text = "";

                Utility.Errorhandler.ClearError(lblerror);

                if (sFromDate == "")
                {
                    CommonVariables.MESSAGE_TEXT = "From date can not balnk";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sToDate == "")
                {
                    CommonVariables.MESSAGE_TEXT = "To date can not balnk";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {
                    display_summary(KeyEMPLOYEE_ID);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }
    }
}