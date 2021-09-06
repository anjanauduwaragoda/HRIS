using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Employee;
using System.Data;
using DataHandler.MetaData;
using Common;

namespace GroupHRIS.EmployeeProfile
{
    public partial class EmployeeManagement : System.Web.UI.Page
    {
        public static string mEmpProfileID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mEmpProfileID = (string)(Session["KeyEMPLOYEE_ID"]);
                get_employee(mEmpProfileID);
            }
        }

        private void get_employee(string empid)
        {
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);

            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            DivisionDataHandler divisionDataHandler = new DivisionDataHandler();
            DepartmentDataHandler departmentDataHandler = new DepartmentDataHandler();
            DataRow employee = null;
            DataRow division = null;
            DataRow department = null;

            try
            {
                
                employee = employeeDataHandler.populate(empid.ToString());
                if (employee != null)
                {

                    string sdeptcode = employee["DEPT_ID"].ToString();
                    string sdivcode = employee["DIVISION_ID"].ToString();

                    lblemployeeid.Text = empid.ToString();
                    lblfullname.Text = employee["TITLE"].ToString() + employee["FULL_NAME"].ToString();
                    lblinitials.Text = employee["EMP_INITIALS"].ToString();
                    lblfirstname.Text = employee["FIRST_NAME"].ToString();
                    lblmiddlename.Text = employee["MIDDLE_NAMES"].ToString();
                    lbllastname.Text = employee["LAST_NAME"].ToString();
                    lblgender.Text = employee["GENDER"].ToString().Trim();
                    lblnic.Text = employee["EMP_NIC"].ToString();
                    lblpassport.Text = employee["PASSPORT_NUMBER"].ToString();
                    lbldob.Text = Convert.ToDateTime(employee["DOB"].ToString()).ToString("yyyy/MM/dd");
                    lbldoj.Text = Convert.ToDateTime(employee["DOJ"].ToString()).ToString("yyyy/MM/dd");
                    lblmaritalstaus.Text = employee["MARITAL_STATUS"].ToString();
                    lblnationality.Text = employee["NAIONALITY"].ToString();
                    lblreligion.Text = employee["RELIGION"].ToString();
                    lblemail.Text = employee["EMAIL"].ToString();

                    lblepf.Text = employee["EPF_NO"].ToString();
                    lblpermenetaddress.Text = employee["PERMANENT_ADDRESS"].ToString();
                    lblcurrentaddress.Text = employee["CURRENT_ADDRESS"].ToString();
                    lbllandphone.Text = employee["LAND_PHONE"].ToString();
                    lblmobpersonal.Text = employee["MOBILE_PHONE_PERSONAL"].ToString();
                    lblcompmobile.Text = employee["MOBILE_PHONE_COMPANY"].ToString();
                    lblfualcardno.Text = employee["FUEL_CARD_NUMBER"].ToString();
                    lblcity.Text = employee["CITY"].ToString();
                    lbldistance.Text = employee["DISTANCE_TO_OFFICE"].ToString();
                    lblcostcenter.Text = employee["COST_CENTER"].ToString();
                    lblprofitcenter.Text = employee["PROFIT_CENTER"].ToString();

                    department = departmentDataHandler.populate(sdeptcode);

                    if (department != null)
                    {
                        lbldepartment.Text = department["DEPT_NAME"].ToString();
                    }

                    division = divisionDataHandler.getDivisionName(sdivcode);
                    if (division != null)
                    {
                        lbldivision.Text = division["DIV_NAME"].ToString();
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
                employeeDataHandler = null;
                divisionDataHandler = null;
                departmentDataHandler = null;
                employee = null;
                department = null;
                division = null;

            }
            

        }
    }
}