using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.Employee;
using System.Data;

namespace GroupHRIS.EmployeeProfile
{
    public partial class EmployeeTransfers : System.Web.UI.Page
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

            EmployeeTransferDataHandler employeeTransferDataHandler = new EmployeeTransferDataHandler();
            DataTable sTransfers = new DataTable();
            string mTable = "";

            string sTrsts = "";
            string sTrnID = "";
            string sTrnStrDate = "";
            string sCOMP_NAME = "";
            string sDEPT_NAME = "";
            string sDIV_NAME = "";
            string sCC = "";
            string sPC = "";
            string sREMARKS = "";
            string sNTrnID = "";

            try
            {
                mTable = "";
                sTransfers = employeeTransferDataHandler.populateEmpProfile(KeyEMPLOYEE_ID);
                for (int x = 0; x <= sTransfers.Rows.Count - 1; x++)
                {
                   
                    sTrsts = sTransfers.Rows[x]["Trsts"].ToString();
                    sTrnID = sTransfers.Rows[x]["TRANS_ID"].ToString();
                    sTrnStrDate = DateTime.Parse(sTransfers.Rows[x]["start_date"].ToString()).ToString("yyyy/MM/dd");
                    sCOMP_NAME = sTransfers.Rows[x]["COMP_NAME"].ToString();
                    sDEPT_NAME = sTransfers.Rows[x]["DEPT_NAME"].ToString();
                    sDIV_NAME = sTransfers.Rows[x]["DIV_NAME"].ToString();
                    sCC = sTransfers.Rows[x]["FROM_CC"].ToString();
                    sPC = sTransfers.Rows[x]["FROM_PC"].ToString();
                    sREMARKS = sTransfers.Rows[x]["REMARKS"].ToString();

                        if (sTrnID != sNTrnID)
                        {
                            mTable = mTable + "<Table class='EmployeeTransferTB' style='width:850px'>";
                            mTable = mTable + "<Tr>";
                            mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:70px;' >";
                            mTable = mTable + "Status";
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:80px'>";
                            mTable = mTable + "Date";
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:200px' >";
                            mTable = mTable + "Company";
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:200px' >";
                            mTable = mTable + "Department";
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:200px' >";
                            mTable = mTable + "Division";
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:50px' >";
                            mTable = mTable + "PC";
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTBTitle' style='width:50px' >";
                            mTable = mTable + "CC";
                            mTable = mTable + "</Td>";
                            mTable = mTable + "</Tr>";

                            mTable = mTable + "<Tr>";
                            mTable = mTable + "<Td class='EmployeeTransferTB' >";
                            mTable = mTable + "From ";
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTB' >";
                            mTable = mTable + sTrnStrDate;
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTB' >";
                            mTable = mTable + sCOMP_NAME;
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTB' >";
                            mTable = mTable + sDEPT_NAME;
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTB' >";
                            mTable = mTable + sDIV_NAME;
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTB' >";
                            mTable = mTable + sCC;
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTB' >";
                            mTable = mTable + sPC;
                            mTable = mTable + "</Td>";
                            mTable = mTable + "</Tr>";
                           
                        }
                        else
                        {
                            mTable = mTable + "<Tr>";
                            mTable = mTable + "<Td class='EmployeeTransferTB' >";
                            mTable = mTable + "To ";
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTB' >";
                            mTable = mTable + "";
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTB' >";
                            mTable = mTable + sCOMP_NAME;
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTB' >";
                            mTable = mTable + sDEPT_NAME;
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTB' >";
                            mTable = mTable + sDIV_NAME;
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTB' >";
                            mTable = mTable + sCC;
                            mTable = mTable + "</Td>";
                            mTable = mTable + "<Td class='EmployeeTransferTB' >";
                            mTable = mTable + sPC;
                            mTable = mTable + "</Td>";
                            mTable = mTable + "</Tr>";
                            mTable = mTable + "</Table>";
                            mTable = mTable + "<Br / >";
                        }

                        sNTrnID = sTrnID;
                        
                }


                LiteralSE1.Text = LiteralSE1.Text + mTable;
                mTable = "";

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                employeeTransferDataHandler = null;
                sTransfers.Dispose();
                sTransfers = null;
            }

        }
    }
}