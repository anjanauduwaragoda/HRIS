using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using DataHandler.Employee;

namespace GroupHRIS.EmployeeProfile
{
    public partial class PreviousEmployment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //string KeyEMPLOYEE_ID = "EP000001";
                //string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                string KeyEMPLOYEE_ID = Request.QueryString["mEmpProfileID"];
                display_secondary(KeyEMPLOYEE_ID);
            }
        }

        private void display_secondary(string KeyEMPLOYEE_ID)
        {
            PrevExperienceDataHandler prevExperienceDataHandler = new PrevExperienceDataHandler();
            DataTable sWorkexp = new DataTable();
            string mTable = "";

            string sORGANIZATION = "";
            string sADDRESS = "";
            string sDESIGNATION = "";
            string sFROM_DATE = "";
            string sTO_DATE = "";
            string sREMARKS = "";

            try
            {
                sWorkexp = prevExperienceDataHandler.populateEmpProfile(KeyEMPLOYEE_ID);
                for (int x = 0; x <= sWorkexp.Rows.Count - 1; x++)
                {
                    sORGANIZATION = sWorkexp.Rows[x]["ORGANIZATION"].ToString();
                    sADDRESS = sWorkexp.Rows[x]["ADDRESS"].ToString();
                    sDESIGNATION = sWorkexp.Rows[x]["DESIGNATION"].ToString();
                    sFROM_DATE = sWorkexp.Rows[x]["FROM_DATE"].ToString();
                    sTO_DATE = sWorkexp.Rows[x]["TO_DATE"].ToString();
                    sREMARKS = sWorkexp.Rows[x]["REMARKS"].ToString();

                    mTable = "";
                    mTable = mTable + "<Table class='EmployeeTransferTB' >";
                    mTable = mTable + "<Tr><Td class='EmployeeTransferTBTitle' colspan='2' style='width:450px'>";
                    mTable = mTable + "From : " + sFROM_DATE + " - " + sTO_DATE;
                    mTable = mTable + "</Tr>";
                    mTable = mTable + "</Td>";

                    mTable = mTable + "<Tr><Td class='EmployeeTransferTB' style='width:100px'>";
                    mTable = mTable + "Organization";
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td  class='EmployeeTransferTB' >";
                    mTable = mTable + sORGANIZATION;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "</Tr>";

                    mTable = mTable + "<Tr><Td class='EmployeeTransferTB' style='width:100px'>";
                    mTable = mTable + "Designation";
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td class='EmployeeTransferTB' >";
                    mTable = mTable + sDESIGNATION;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "</Tr>";

                    mTable = mTable + "<Tr><Td class='EmployeeTransferTB' style='width:100px'>";
                    mTable = mTable + "Address";
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td class='EmployeeTransferTB' >";
                    mTable = mTable + sADDRESS;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "</Tr>";

                    mTable = mTable + "<Tr><Td class='EmployeeTransferTB' style='width:100px'>";
                    mTable = mTable + "Remarks";
                    mTable = mTable + "</Td>";
                    mTable = mTable + "<Td class='EmployeeTransferTB' >";
                    mTable = mTable + sREMARKS;
                    mTable = mTable + "</Td>";
                    mTable = mTable + "</Tr>";

                    mTable = mTable + "</Table>";


                    if (x % 2 == 0)
                    {
                        LiteralSE1.Text = LiteralSE1.Text + mTable;
                        mTable = "";
                        LiteralSE1.Text = LiteralSE1.Text + "<br/>";
                    }
                    else
                    {
                        LiteralSE2.Text = LiteralSE2.Text + mTable;
                        mTable = "";
                        LiteralSE2.Text = LiteralSE2.Text + "<br/>";
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
                prevExperienceDataHandler = null;
                sWorkexp.Dispose();
                sWorkexp = null;
            }

        }
    }
}