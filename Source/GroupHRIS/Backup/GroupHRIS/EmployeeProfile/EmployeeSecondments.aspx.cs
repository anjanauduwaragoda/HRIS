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
    public partial class EmployeeSecondments : System.Web.UI.Page
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
            EmployeeSecondmentDataHandler employeeSecondmentDataHandler = new EmployeeSecondmentDataHandler();
            DataTable sSecondments = new DataTable();
            string mTable = "";

            string sCOMP_NAME = "";
            string sDEPT_NAME = "";
            string sDIV_NAME = "";
            string sFROM_DATE = "";
            string sEND_DATE = "";
            string sREMARKS = "";

            try
            {
                sSecondments = employeeSecondmentDataHandler.populateEmpProfile(KeyEMPLOYEE_ID);
                for (int x = 0; x <= sSecondments.Rows.Count - 1; x++)
                {
                    sCOMP_NAME = sSecondments.Rows[x]["COMP_NAME"].ToString();
                    sDEPT_NAME = sSecondments.Rows[x]["DEPT_NAME"].ToString();
                    sDIV_NAME = sSecondments.Rows[x]["DIV_NAME"].ToString();
                    sFROM_DATE = sSecondments.Rows[x]["FROM_DATE"].ToString();
                    sEND_DATE = sSecondments.Rows[x]["END_DATE"].ToString();
                    sREMARKS = sSecondments.Rows[x]["REMARKS"].ToString();

                        mTable = "";
                        mTable = mTable + "<Table class='EmployeeTransferTB'>";
                        mTable = mTable + "<Tr><Td class='EmployeeTransferTBTitle' colspan='2' style='width:450px'>";
                        mTable = mTable + "From : " + sFROM_DATE + " - " + sEND_DATE ;
                        mTable = mTable + "</Td>";
                        mTable = mTable + "</Tr>";

                        mTable = mTable + "<Tr><Td class='EmployeeTransferTB' style='width:100px'>";
                        mTable = mTable + "Company";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTB' >";
                        mTable = mTable + sCOMP_NAME;
                        mTable = mTable + "</Td>";
                        mTable = mTable + "</Tr>";

                        mTable = mTable + "<Tr><Td class='EmployeeTransferTB' style='width:100px'>";
                        mTable = mTable + "Department";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTB' >";
                        mTable = mTable + sDEPT_NAME;
                        mTable = mTable + "</Td>";
                        mTable = mTable + "</Tr>";

                        mTable = mTable + "<Tr><Td class='EmployeeTransferTB' style='width:100px'>";
                        mTable = mTable + "Division";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTB' >";
                        mTable = mTable + sDIV_NAME;
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
                employeeSecondmentDataHandler = null;
                sSecondments.Dispose();
                sSecondments = null;
            }

        }
    }
}