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
    public partial class HigherEducation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string KeyEMPLOYEE_ID = Request.QueryString["mEmpProfileID"];
                //string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                display_Higher(KeyEMPLOYEE_ID);
            }
        }

        private void display_Higher(string KeyEMPLOYEE_ID)
        {
            HigherEducationDataHandler higherEducationDataHandler = new HigherEducationDataHandler();
            DataTable heducation = new DataTable();
            string mTable = "";

            string sINSTITUTE = "";
            string sPROGRAM = "";
            string sPROGRAME_NAME = "";
            string sDURATION = "";
            string sFROM_YEAR = "";
            string sTO_YEAR = "";
            string sREMARKS = "";
            string sSECTOR = "";
            string sGRADE = "";
            string sSTATUS_CODE = "";

            try
            {
                heducation = higherEducationDataHandler.populateEmpProfile(KeyEMPLOYEE_ID);
                for (int x = 0; x <= heducation.Rows.Count - 1; x++)
                {
                    sINSTITUTE = heducation.Rows[x]["INSTITUTE"].ToString();
                    sPROGRAM = heducation.Rows[x]["PROGRAM"].ToString();
                    sPROGRAME_NAME = heducation.Rows[x]["PROGRAME_NAME"].ToString();
                    sDURATION = heducation.Rows[x]["DURATION"].ToString();
                    sFROM_YEAR = heducation.Rows[x]["FROM_YEAR"].ToString();
                    sTO_YEAR = heducation.Rows[x]["TO_YEAR"].ToString();
                    sGRADE = heducation.Rows[x]["GRADE"].ToString();
                    sREMARKS = heducation.Rows[x]["REMARKS"].ToString();
                    sSECTOR = heducation.Rows[x]["SECTOR"].ToString();
                    sSTATUS_CODE = heducation.Rows[x]["STATUS_CODE"].ToString();

                    if (sSTATUS_CODE == Constants.STATUS_ACTIVE_VALUE)
                    {
                        mTable = "";
                        mTable = mTable + "<Table class='EmployeeTransferTB'>";
                        mTable = mTable + "<Tr><Td colspan='2' style='width:450px' class='EmployeeTransferTBTitle'>";
                        mTable = mTable + sINSTITUTE;
                        mTable = mTable + "</Td>";
                        mTable = mTable + "</Tr>";

                        mTable = mTable + "<Tr><Td class='EmployeeTransferTB' style='width:100px'>";
                        mTable = mTable + "Program";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td  class='EmployeeTransferTB' style='width:350px'>";
                        mTable = mTable + sPROGRAM;
                        mTable = mTable + "</Td>";
                        mTable = mTable + "</Tr>";

                        mTable = mTable + "<Tr><Td  class='EmployeeTransferTB'>";
                        mTable = mTable + "Program Name";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td  class='EmployeeTransferTB' style='width:350px'>";
                        mTable = mTable + sPROGRAME_NAME;
                        mTable = mTable + "</Td>";
                        mTable = mTable + "</Tr>";

                        mTable = mTable + "<Tr><Td class='EmployeeTransferTB'>";
                        mTable = mTable + "Duration";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td class='EmployeeTransferTB'  style='width:100px'>";
                        mTable = mTable + sDURATION;
                        mTable = mTable + "</Td>";
                        mTable = mTable + "</Tr>";

                        mTable = mTable + "<Tr><Td class='EmployeeTransferTB'>";
                        mTable = mTable + "Period";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td  class='EmployeeTransferTB' style='width:100px'>";
                        mTable = mTable + sFROM_YEAR + " - " + sTO_YEAR;
                        mTable = mTable + "</Td>";
                        mTable = mTable + "</Tr>";

                        mTable = mTable + "<Tr><Td class='EmployeeTransferTB' >";
                        mTable = mTable + "Grade";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td  class='EmployeeTransferTB' style='width:100px'>";
                        mTable = mTable + sGRADE;
                        mTable = mTable + "</Td>";
                        mTable = mTable + "</Tr>";

                        mTable = mTable + "<Tr><Td class='EmployeeTransferTB' >";
                        mTable = mTable + "Sector";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td  class='EmployeeTransferTB' style='width:100px'>";
                        mTable = mTable + sSECTOR;
                        mTable = mTable + "</Td>";
                        mTable = mTable + "</Tr>";

                        mTable = mTable + "<Tr><Td class='EmployeeTransferTB' >";
                        mTable = mTable + "Remark";
                        mTable = mTable + "</Td>";
                        mTable = mTable + "<Td  class='EmployeeTransferTB' style='width:100px'>";
                        mTable = mTable + sREMARKS;
                        mTable = mTable + "</Td>";
                        mTable = mTable + "</Tr>";

                        mTable = mTable + "</Table>";

                        if (x % 2 == 0)
                        {
                            LiteralHE1.Text = LiteralHE1.Text + mTable;
                            mTable = "";
                            LiteralHE1.Text = LiteralHE1.Text + "<br/>";
                        }
                        else
                        {
                            LiteralHE2.Text = LiteralHE2.Text + mTable;
                            mTable = "";
                            LiteralHE2.Text = LiteralHE2.Text + "<br/>";
                        }

                        
                        
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
                higherEducationDataHandler = null;
                heducation.Dispose();
                heducation = null;
            }

        }
    }
}