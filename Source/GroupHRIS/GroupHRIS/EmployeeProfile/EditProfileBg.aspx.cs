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
    public partial class EditProfileBg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                get_employeephoto(KeyEMPLOYEE_ID);
            }
        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
            string mfolderpath = Constants.CON_DEFAULT_BG_IMAGE_PATH.Replace("~","");
            string path = "";
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            string mFontcolor = txttopiccolor.Text.ToString();
            string mfontsize = ddlfontsize.SelectedItem.Text.ToString();
            string mfontBold = Constants.CON_DEFAULT_FONT_BOLD;
            string mfontitalic = Constants.CON_DEFAULT_FONT_ITALIC;

            EmployeeProfileHandler employeeProfileHandler = new EmployeeProfileHandler();

            try
            {
                if (chkfontbold.Checked == false )
                {
                    mfontBold = "N";
                }

                if (chkfontitalic.Checked == false)
                {
                    mfontitalic = "N";
                }
                if (mFontcolor.Trim() == "")
                {
                    mFontcolor = "000000";
                }

                if (FileUpload1.HasFile)
                {
                    path = Server.MapPath(mfolderpath);
                    FileUpload1.SaveAs(path + FileUpload1.FileName);
                    employeeProfileHandler.InsertEmployeeBgImage(mFontcolor, mfontsize, mfontBold, mfontitalic,KeyEMPLOYEE_ID, mfolderpath, FileUpload1.FileName, Constants.STATUS_ACTIVE_VALUE);
                    CommonVariables.MESSAGE_TEXT = "Background Image Successfully Saved ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {
                    if (chkdefault.Checked == true)
                    {
                        employeeProfileHandler.UpdateEmployeeBgImageDefault(KeyEMPLOYEE_ID, Constants.STATUS_ACTIVE_VALUE);
                        CommonVariables.MESSAGE_TEXT = "Background set to Default Style ";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "Background Image not selected ";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
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
                employeeProfileHandler = null;
            }

        }

        private void get_employeephoto(string KeyEMPLOYEE_ID)
        {
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            DataTable employeePhoto = new DataTable();

            try
            {
                employeePhoto = employeeDataHandler.populateEmpBgImage(KeyEMPLOYEE_ID);
                if (employeePhoto.Rows.Count > 0)
                {
                    DataRow dr = employeePhoto.Rows[0];
                    string imagepath = dr["IMAGEPATH"].ToString() + dr["IMAGENAME"].ToString();
                    imgme.ImageUrl = imagepath;
                    imgme.Width = 300;
                    imgme.Height = 250;
                    dr = null;
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
                employeePhoto.Dispose();
                employeePhoto = null;

            }
        }

        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblerror);
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
            get_employeephoto(KeyEMPLOYEE_ID);
        }
    }
}