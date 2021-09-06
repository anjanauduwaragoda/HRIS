using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.UserAdministration ;
using System.Data;
using Common;
using DataHandler.Employee;
using System.IO;

namespace GroupHRIS.EmployeeProfile
{
    public partial class Editprofile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack){
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                get_employee(KeyEMPLOYEE_ID);
            }
        }

        private void get_employee(string KeyEMPLOYEE_ID)
        {

            UserAdministrationHandler userAdministrationHandler = new UserAdministrationHandler();
            DataRow  empProfile = null;

            try
            {
                empProfile = userAdministrationHandler.populateuserProfile(KeyEMPLOYEE_ID);

                if (empProfile != null){

                    DateTime mDob = DateTime.Parse(empProfile["DOB"].ToString());

                    lblcurrentaddress.Text = empProfile["CURRENT_ADDRESS"].ToString();
                    lbldepartment.Text = empProfile["DEPT_NAME"].ToString();
                    lbldivision.Text = empProfile["DIV_NAME"].ToString();
                    lblemail.Text = empProfile["EMAIL"].ToString();
                    lblepf.Text = empProfile["EPF_NO"].ToString();
                    lblnic.Text = empProfile["EMP_NIC"].ToString();
                    lbluserid.Text = empProfile["USER_ID"].ToString();
                    lblpermenanraddress.Text = empProfile["PERMANENT_ADDRESS"].ToString();
                    txtFirstName.Text = empProfile["FIRST_NAME"].ToString();
                    txtLastName.Text = empProfile["LAST_NAME"].ToString();
                    lblfirstname.Text = empProfile["FIRST_NAME"].ToString();
                    lbllastname.Text = empProfile["LAST_NAME"].ToString();
                    txtbdate.Text = mDob.ToString("yyyy/MM/dd");
                    ddlreligion.Text = empProfile["RELIGION"].ToString();
                    ddlMaritalStatus.Text = empProfile["MARITAL_STATUS"].ToString();
                    ddlGender.SelectedValue = empProfile["GENDER"].ToString();
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                userAdministrationHandler = null;
                empProfile = null;
            }
        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            string sFIRST_NAME = txtFirstName.Text.ToString().Trim();
            string sLAST_NAME = txtLastName.Text.ToString().Trim();

            string sBirthDay = txtbdate.Text.ToString().Trim();
            string sGender = ddlGender.SelectedValue.ToString();
            string sMaritalsts = ddlMaritalStatus.Text.ToString();
            string sReligion = ddlreligion.Text.ToString();

            string madddate = DateTime.Today.ToString("yyyy/MM/dd");
            string mlogUser = (string)(Session["KeyUSER_ID"]);

            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            UserAdministrationHandler userAdministrationHandler = new UserAdministrationHandler();

            try
            {
                if (sFIRST_NAME == "")
                {
                    CommonVariables.MESSAGE_TEXT = "First Name can not blank ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (sLAST_NAME == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Last Name can not blank ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {
                    userAdministrationHandler.UpdateHrisUserProfile(KeyEMPLOYEE_ID, sFIRST_NAME, sLAST_NAME, sBirthDay, sGender, sMaritalsts, sReligion, Constants.STATUS_ACTIVE_VALUE, mlogUser, madddate);
                    CommonVariables.MESSAGE_TEXT = "Profile Details Successfully Updated ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                employeeDataHandler = null;
                userAdministrationHandler = null;
            }
        }

        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            try
            {
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                get_employee(KeyEMPLOYEE_ID);
                Utility.Errorhandler.ClearError(lblerror);
            }
            catch (Exception ex)
            {
                
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }

        }

    }
}