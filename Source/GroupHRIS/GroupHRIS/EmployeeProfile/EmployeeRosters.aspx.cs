using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Roster;
using System.Data;
using Common;

namespace GroupHRIS.EmployeeProfile
{
    public partial class EmployeeRosters : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string KeyEMPLOYEE_ID = Request.QueryString["mEmpProfileID"];
            }
        }

        private void display_rosetr(string KeyEMPLOYEE_ID)
        {
            EmpRosterAssignmentDataHandler empRosterAssignmentDataHandler = new EmpRosterAssignmentDataHandler();
            DataTable sRoster = new DataTable();
            string mTable = "";

            DateTime  sFromdate = DateTime.Parse(txtfromdate.Text.ToString()) ;
            DateTime sTodate = DateTime.Parse(txttodate.Text.ToString());
            string sDutyDate = "";
            string sIsSummerized = "";
            string sInterChange = "";
            string sFirstName = "";
            string sFromTime = "";
            string sToTime = "";
            string sDescription = "";

            try
            {
                sRoster = empRosterAssignmentDataHandler.populateEmpProfile(KeyEMPLOYEE_ID, sFromdate.ToString("yyyy/MM/dd"), sTodate.ToString("yyyy/MM/dd"));
                for (int x = 0; x <= sRoster.Rows.Count - 1; x++)
                {

                    if (x == 0)
                    {
                        mTable = "";
                        mTable = mTable + "<Table class='EmployeeTransferTB' style='width:700px'>";
                        mTable = mTable + "<Tr>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle'>";
                        mTable = mTable + "Duty Date";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle'>";
                        mTable = mTable + "IS Updated";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle'>";
                        mTable = mTable + "Inter Change";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle'>";
                        mTable = mTable + "Covered By";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle'>";
                        mTable = mTable + "Roster Time";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTBTitle'>";
                        mTable = mTable + "Roster Type";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "</Tr>";
                    }

                    sDutyDate = sRoster.Rows[x]["DUTY_DATE"].ToString();
                    sIsSummerized = sRoster.Rows[x]["IS_SUMMARIZED"].ToString();
                    sInterChange = sRoster.Rows[x]["INTERCHANGE_NUMBER"].ToString();
                    sFirstName = sRoster.Rows[x]["FIRST_NAME"].ToString();
                    sFromTime = sRoster.Rows[x]["FROM_TIME"].ToString();
                    sToTime = sRoster.Rows[x]["TO_TIME"].ToString();
                    sDescription = sRoster.Rows[x]["DESCRIP"].ToString();

                    mTable = mTable + "<Tr>";
                    mTable = mTable + "<Td class='EmployeeTransferTB' style='width:80px'>";
                    mTable = mTable + sDutyDate;
                    mTable = mTable + "</Td>";
                    
                    if (sIsSummerized != "N")
                    {
                        mTable = mTable + "<Td class='EmployeeTransferTB' style='width:80px'>";
                        mTable = mTable + sIsSummerized;
                        mTable = mTable + "</Td>";
                    }
                    else
                    {
                        mTable = mTable + "<Td class='EmployeeTransferTB' style='width:80px'>";
                        mTable = mTable + sIsSummerized;
                        mTable = mTable + "</Td>";
                    }
                    

                    if (sInterChange != "")
                    {
                        mTable = mTable + "<Td class='EmployeeTransferTB' style='width:100px;background-color:#00CC00'>";
                        mTable = mTable + sInterChange;
                        mTable = mTable + "</Td>";
                    }
                    else
                    {
                        mTable = mTable + "<Td class='EmployeeTransferTB' style='width:100px'>";
                        mTable = mTable + sInterChange;
                        mTable = mTable + "</Td>";
                    }

                    mTable = mTable + "<Td class='EmployeeTransferTB' style='width:100px'>";
                    mTable = mTable + sFirstName;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td class='EmployeeTransferTB' style='width:100px'>";
                    mTable = mTable + sFromTime + " - " + sToTime;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td class='EmployeeTransferTB' style='width:150px'>";
                    mTable = mTable + sDescription;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "</Tr>";

                    if (x == sRoster.Rows.Count - 1)
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
                empRosterAssignmentDataHandler = null;
                sRoster.Dispose();
                sRoster = null;
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
                    display_rosetr(KeyEMPLOYEE_ID);
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