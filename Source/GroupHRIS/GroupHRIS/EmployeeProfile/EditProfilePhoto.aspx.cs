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
    public partial class EditProfilePhoto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack ){
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                get_employeephoto(KeyEMPLOYEE_ID);
            }
        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
            string mfolderpath = Constants.CON_DEFAULT_EEMPLOYEE_IMAGE_PATH;
            string path = "";
            string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);

            string mImageWidth = txtphotowidth.Text.ToString();
            string mImageHeight = txtphotoheight.Text.ToString();

            string madddate = DateTime.Today.ToString("yyyy/MM/dd");
            string mlogUser = (string)(Session["KeyUSER_ID"]);

            EmployeeProfileHandler employeeProfileHandler = new EmployeeProfileHandler();

            try
            {
                if (mImageWidth == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Photo Width is invalid ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (mImageHeight == "")
                {
                    CommonVariables.MESSAGE_TEXT = "Photo Height is invalid ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else if (FileUpload1.HasFile)
                {
                    path = Server.MapPath(mfolderpath);
                    FileUpload1.SaveAs(path + FileUpload1.FileName);
                    employeeProfileHandler.InsertEmployeePhoto(mImageWidth, mImageHeight, KeyEMPLOYEE_ID, mfolderpath, FileUpload1.FileName);
                    CommonVariables.MESSAGE_TEXT = "Photo Successfully Saved ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {
                    employeeProfileHandler.UpdateEmployeePhoto(mImageWidth, mImageHeight, KeyEMPLOYEE_ID,  Constants.STATUS_ACTIVE_VALUE);
                    CommonVariables.MESSAGE_TEXT = "Photo Successfully Updated ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                }
            }
            catch (Exception ex)
            {
                employeeProfileHandler = null;
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
                employeePhoto = employeeDataHandler.populateEmpPhoto(KeyEMPLOYEE_ID);
                if (employeePhoto.Rows.Count > 0)
                {
                    DataRow dr = employeePhoto.Rows[0];
                    string imagepath = dr["IMAGEPATH"].ToString() + dr["IMAGENAME"].ToString();
                    imgme.ImageUrl = imagepath;
                    imgme.Width = int.Parse(dr["IMAGEWIDTH"].ToString());
                    imgme.Height = int.Parse(dr["IMAGEHEIGHT"].ToString());
                    txtphotowidth.Text = dr["IMAGEWIDTH"].ToString();
                    txtphotoheight.Text = dr["IMAGEHEIGHT"].ToString();
                    dr = null;
                }
                else
                {
                    txtphotowidth.Text = Constants.CON_DEFAULT_IMAGE_WIDTH;
                    txtphotoheight.Text = Constants.CON_DEFAULT_IMAGE_HEIGHT;
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