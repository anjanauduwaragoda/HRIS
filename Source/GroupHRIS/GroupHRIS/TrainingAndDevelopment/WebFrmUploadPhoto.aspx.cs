using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.IO;
using GroupHRIS.Utility;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmUploadPhoto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["KeyLOGOUT_STS"] == null)
                {
                    Response.Redirect("MainLogout.aspx", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login/SessionExpior.aspx", false);
            }

            if (!IsPostBack)
            { 
                imgRemoveImage.Visible = false; 
            }

            if (IsPostBack && fuPhoto.HasFile != false)
            {
                priviewImageAtBrows();
            }  
        }

        private void priviewImageAtBrows()
        {
            try
            {
                Stream fs;
                BinaryReader br;
                Byte[] bytes = null;

                if (fuPhoto.HasFile)
                {
                    Errorhandler.ClearError(lblErrorMsgPhoto);

                    string filePath = fuPhoto.PostedFile.FileName;
                    //hfImageFile.Value = filePath;
                    string filename = Path.GetFileName(filePath);
                    string ext = Path.GetExtension(filename);
                    string contenttype = String.Empty;
                    int fileSize = fuPhoto.PostedFile.ContentLength;
                    if (fileSize > 500000)
                    {
                        CommonVariables.MESSAGE_TEXT = "File size exceeds the limit";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsgPhoto);
                        return;
                    }

                    fs = fuPhoto.PostedFile.InputStream;
                    br = new BinaryReader(fs);
                    bytes = br.ReadBytes((Int32)fs.Length);
                    Session["byteArray"] = bytes;
                }
                else
                {
                    Array.Clear(bytes, 0, bytes.Length);
                }

                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                imgPhoto.ImageUrl = "data:image/jpg;base64," + base64String;

                
                fuPhoto.Visible = false;
                imgRemoveImage.Visible = true;

                //Session["uploadedFile"] = fuPhoto.PostedFile;


            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsgPhoto);
            }
        }

        private void removeImage()
        {
            try
            {
                
                fuPhoto.Visible = true;
                imgPhoto.ImageUrl = "../Images/Add_Person.png";
                imgRemoveImage.Visible = false;
                Session["byteArray"] = null;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsgPhoto);
            }

        }

        protected void lbRemoveImage_Click(object sender, EventArgs e)
        {
            try
            {
                
                fuPhoto.Visible = true;
                removeImage();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsgPhoto);
            }
        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {

        }


    }
}