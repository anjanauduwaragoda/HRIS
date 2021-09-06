using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Employee;
using System.Data;
using Common;

namespace GroupHRIS.EmployeeProfile
{
    public partial class SecondaryEducation : System.Web.UI.Page
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
            SecondaryEducationDataHandler secondaryEducationDataHandler = new SecondaryEducationDataHandler();
            DataTable seducation = new DataTable();
            DataTable sIsAlattemps = new DataTable();
            DataTable sIsAlattempsCopy = new DataTable();

            string mTableAL = "";
            string mTableOL = "";
            string isAl = "";
            string sAttempt = "";
            string sSchool = "";
            string sYear = "";
            string sEducation = "";
            string sGrade = "";

            try
            {
                sIsAlattempsCopy = secondaryEducationDataHandler.populateEmpIsAlCount(KeyEMPLOYEE_ID);
                sIsAlattemps = sIsAlattempsCopy.Copy();
                for (int x = 0; x <= sIsAlattemps.Rows.Count - 1; x++)
                {
                    isAl = sIsAlattemps.Rows[x]["IS_AL"].ToString();
                    sAttempt = sIsAlattemps.Rows[x]["ATTEMPT"].ToString();
                    sYear = sIsAlattemps.Rows[x]["ATTEMPTED_YEAR"].ToString();
                    sSchool = sIsAlattemps.Rows[x]["SCHOOL"].ToString();

                    if (isAl.Equals("Y"))
                    {
                        seducation = secondaryEducationDataHandler.populateEmpProfile(KeyEMPLOYEE_ID, isAl, sAttempt);
                        if (seducation.Rows.Count > 0)
                        {
                            mTableAL = "";
                            mTableAL = mTableAL + "<Table class='EmployeeTransferTB'>";
                            mTableAL = mTableAL + "<Tr><Td class='EmployeeTransferTBTitle' colspan='2' style='width:250px'>";
                            mTableAL = mTableAL + sSchool + " (" + sYear + ")";
                            mTableAL = mTableAL + "</Td><Tr/>";

                            for (int i = 0; i <= seducation.Rows.Count - 1; i++)
                            {
                                sEducation = seducation.Rows[i]["SUBJECT_NAME"].ToString();
                                sGrade = seducation.Rows[i]["GRADE"].ToString();
                                mTableAL = mTableAL + "<Tr><Td class='EmployeeTransferTB'>";
                                mTableAL = mTableAL + sEducation;
                                mTableAL = mTableAL + "</Td>";
                                mTableAL = mTableAL + "<Td  class='EmployeeTransferTB'>";
                                mTableAL = mTableAL + sGrade;
                                mTableAL = mTableAL + "</Td></Tr>";
                            }

                            mTableAL = mTableAL + "</Table>";
                            LiteralAL.Text = LiteralAL.Text + mTableAL;
                            LiteralAL.Text = LiteralAL.Text + "<br/>";
                        }

                    }
                    else if (isAl.Equals("N"))
                    {
                        seducation = secondaryEducationDataHandler.populateEmpProfile(KeyEMPLOYEE_ID, isAl, sAttempt);
                        if (seducation.Rows.Count > 0)
                        {
                            mTableOL = "";
                            mTableOL = mTableOL + "<Table  class='EmployeeTransferTB'>";
                            mTableOL = mTableOL + "<Tr><Td  class='EmployeeTransferTBTitle' colspan='2' style='width:250px'>";
                            mTableOL = mTableOL + sSchool + " (" + sYear + ")";
                            mTableOL = mTableOL + "</Td><Tr/>";

                            for (int i = 0; i <= seducation.Rows.Count - 1; i++)
                            {
                                sEducation = seducation.Rows[i]["SUBJECT_NAME"].ToString();
                                sGrade = seducation.Rows[i]["GRADE"].ToString();

                                mTableOL = mTableOL + "<Tr><Td class='EmployeeTransferTB'>";
                                mTableOL = mTableOL + sEducation;
                                mTableOL = mTableOL + "</Td>";
                                mTableOL = mTableOL + "<Td class='EmployeeTransferTB'>";
                                mTableOL = mTableOL + sGrade;
                                mTableOL = mTableOL + "</Td></Tr>";
                            }

                            mTableOL = mTableOL + "</Table>";
                            LiteralOL.Text = LiteralOL.Text + mTableOL;
                            LiteralOL.Text = LiteralOL.Text + "<br/>";
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
                secondaryEducationDataHandler = null;
                seducation.Dispose();
                seducation = null;
                sIsAlattemps.Dispose();
                sIsAlattemps = null;
                sIsAlattempsCopy.Dispose();
                sIsAlattempsCopy = null;
            }

        }

    }
}