using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Common;
using System.Data;

namespace DataHandler.Employee
{
   public  class EmployeeProfileHandler: TemplateDataHandler 
    {

        // Insert hris user profile Photo
       public Boolean InsertEmployeePhoto(string sImageWidth, string sImageHeight, string sEmployeeID, string sImagePath, string sImageName)
       {
           Boolean blInserted = false;

           string sMySqlString = "";

           MySqlTransaction mySqlTrans = null;

           try
           {

               mySqlCon.Open();

               mySqlTrans = mySqlCon.BeginTransaction();

               string sImagestatus = Constants.STATUS_ACTIVE_VALUE;
               string sInactiveImagestatus = Constants.STATUS_INACTIVE_VALUE;


               mySqlCmd.Parameters.Add(new MySqlParameter("@sImageWidth", sImageWidth.Trim() == "" ? (object)DBNull.Value : sImageWidth.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@sImageHeight", sImageHeight.Trim() == "" ? (object)DBNull.Value : sImageHeight.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@sEmployeeID", sEmployeeID.Trim() == "" ? (object)DBNull.Value : sEmployeeID.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@sImagePath", sImagePath.Trim() == "" ? (object)DBNull.Value : sImagePath.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@sImageName", sImageName.Trim() == "" ? (object)DBNull.Value : sImageName.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@sImagestatus", sImagestatus.Trim() == "" ? (object)DBNull.Value : sImagestatus.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@sInactiveImagestatus", sInactiveImagestatus.Trim() == "" ? (object)DBNull.Value : sInactiveImagestatus.Trim()));

               sMySqlString = "UPDATE EMPLOYEE_PHOTO set IMAGES_STATUS=@sInactiveImagestatus where EMPLOYEE_ID=@sEmployeeID and IMAGES_STATUS=@sImagestatus";
               mySqlCmd.Transaction = mySqlTrans;
               mySqlCmd.CommandText = sMySqlString;
               mySqlCmd.ExecuteNonQuery();

               sMySqlString = "INSERT INTO EMPLOYEE_PHOTO(EMPLOYEE_ID,IMAGEPATH,IMAGENAME,IMAGEWIDTH,IMAGEHEIGHT,IMAGES_STATUS) " +
                              "VALUES(@sEmployeeID,@sImagePath,@sImageName,@sImageWidth,@sImageHeight,@sImagestatus)";
               mySqlCmd.Transaction = mySqlTrans;
               mySqlCmd.CommandText = sMySqlString;

               mySqlCmd.ExecuteNonQuery();

               mySqlTrans.Commit();

               mySqlCon.Close();
               mySqlTrans.Dispose();
               mySqlCmd.Dispose();

               blInserted = true;


           }
           catch (Exception ex)
           {
               if (mySqlCon.State == ConnectionState.Open)
               {
                   mySqlCon.Close();
               }

               mySqlTrans.Rollback();
               throw ex;
           }

           return blInserted;
       }

       // update hris user profile Photo 
       public Boolean UpdateEmployeePhoto(string sImageWidth, string sImageHeight, string sEMPLOYEE_ID, string sSTATUS_CODE)
        {
            Boolean blInserted = false;

            string sMySqlString = "";

            MySqlTransaction mySqlTrans = null;

            try
            {

                string sImagestatus = Constants.STATUS_ACTIVE_VALUE;

                mySqlCon.Open();
                mySqlTrans = mySqlCon.BeginTransaction();

                mySqlCmd.Parameters.Add(new MySqlParameter("@sImageWidth", sImageWidth.Trim() == "" ? (object)DBNull.Value : sImageWidth.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sImageHeight", sImageHeight.Trim() == "" ? (object)DBNull.Value : sImageHeight.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sImagestatus", sImagestatus.Trim() == "" ? (object)DBNull.Value : sImagestatus.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sEMPLOYEE_ID", sEMPLOYEE_ID.Trim() == "" ? (object)DBNull.Value : sEMPLOYEE_ID.Trim()));
                mySqlCmd.Parameters.Add(new MySqlParameter("@sSTATUS_CODE", sSTATUS_CODE.Trim() == "" ? (object)DBNull.Value : sSTATUS_CODE.Trim()));

                sMySqlString = "UPDATE EMPLOYEE_PHOTO set IMAGEWIDTH=@sImageWidth, IMAGEHEIGHT=@sImageHeight where EMPLOYEE_ID=@sEMPLOYEE_ID and IMAGES_STATUS=@sImagestatus";
                mySqlCmd.Transaction = mySqlTrans;
                mySqlCmd.CommandText = sMySqlString;
                mySqlCmd.ExecuteNonQuery();

                mySqlTrans.Commit();

                mySqlCon.Close();
                mySqlTrans.Dispose();
                mySqlCmd.Dispose();

                blInserted = true;

            }
            catch (Exception ex)
            {
                if (mySqlCon.State == ConnectionState.Open)
                {
                    mySqlCon.Close();
                }

                mySqlTrans.Rollback();
                throw ex;
            }
            finally
            {
                mySqlCmd.Parameters.Clear();
            }
            return blInserted;
        }
       
       // Insert hris user profile Backgroung Image
       public Boolean InsertEmployeeBgImage(string mFontcolor, string mfontsize, string mfontBold, string mfontitalic, string sEmployeeID, string sImagePath, string sImageName, string sSTATUS_CODE)
       {
           Boolean blInserted = false;

           string sMySqlString = "";

           MySqlTransaction mySqlTrans = null;

           try
           {

               mySqlCon.Open();

               mySqlTrans = mySqlCon.BeginTransaction();

               string sImagestatus = Constants.STATUS_INACTIVE_VALUE;

               mySqlCmd.Parameters.Add(new MySqlParameter("@mFontcolor", mFontcolor.Trim() == "" ? (object)DBNull.Value : mFontcolor.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@mfontsize", mfontsize.Trim() == "" ? (object)DBNull.Value : mfontsize.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@mfontBold", mfontBold.Trim() == "" ? (object)DBNull.Value : mfontBold.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@mfontitalic", mfontitalic.Trim() == "" ? (object)DBNull.Value : mfontitalic.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@sEmployeeID", sEmployeeID.Trim() == "" ? (object)DBNull.Value : sEmployeeID.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@sImagePath", sImagePath.Trim() == "" ? (object)DBNull.Value : sImagePath.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@sImageName", sImageName.Trim() == "" ? (object)DBNull.Value : sImageName.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@sImagestatus", sImagestatus.Trim() == "" ? (object)DBNull.Value : sImagestatus.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@sSTATUS_CODE", sSTATUS_CODE.Trim() == "" ? (object)DBNull.Value : sSTATUS_CODE.Trim()));

               sMySqlString = "UPDATE EMPLOYEE_PROFILE set IMAGES_STATUS=@sImagestatus where EMPLOYEE_ID=@sEmployeeID and IMAGES_STATUS=@sSTATUS_CODE";
               mySqlCmd.Transaction = mySqlTrans;
               mySqlCmd.CommandText = sMySqlString;
               mySqlCmd.ExecuteNonQuery();

               sMySqlString = "INSERT INTO EMPLOYEE_PROFILE(MENU_FONT_COLOR,MENU_FONT_SIZE,MENU_FONT_BLOD,MENU_FONT_ITALIC,EMPLOYEE_ID,IMAGEPATH,IMAGENAME,IMAGES_STATUS) " +
                              "VALUES(@mFontcolor,@mfontsize,@mfontBold,@mfontitalic,@sEmployeeID,@sImagePath,@sImageName,@sSTATUS_CODE)";
               mySqlCmd.Transaction = mySqlTrans;
               mySqlCmd.CommandText = sMySqlString;

               mySqlCmd.ExecuteNonQuery();

               mySqlTrans.Commit();

               mySqlCon.Close();
               mySqlTrans.Dispose();
               mySqlCmd.Dispose();

               blInserted = true;


           }
           catch (Exception ex)
           {
               if (mySqlCon.State == ConnectionState.Open)
               {
                   mySqlCon.Close();
               }

               mySqlTrans.Rollback();
               throw ex;
           }

           return blInserted;
       }

       // set Default Background Style
       public Boolean UpdateEmployeeBgImageDefault(string sEmployeeID, string sSTATUS_CODE)
       {
           Boolean blInserted = false;

           string sMySqlString = "";

           MySqlTransaction mySqlTrans = null;

           try
           {

               mySqlCon.Open();

               mySqlTrans = mySqlCon.BeginTransaction();

               string sImagestatus = Constants.STATUS_INACTIVE_VALUE;

               mySqlCmd.Parameters.Add(new MySqlParameter("@sEmployeeID", sEmployeeID.Trim() == "" ? (object)DBNull.Value : sEmployeeID.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@sImagestatus", sImagestatus.Trim() == "" ? (object)DBNull.Value : sImagestatus.Trim()));
               mySqlCmd.Parameters.Add(new MySqlParameter("@sSTATUS_CODE", sSTATUS_CODE.Trim() == "" ? (object)DBNull.Value : sSTATUS_CODE.Trim()));

               sMySqlString = "UPDATE EMPLOYEE_PROFILE set IMAGES_STATUS=@sImagestatus where EMPLOYEE_ID=@sEmployeeID and IMAGES_STATUS=@sSTATUS_CODE";
               mySqlCmd.Transaction = mySqlTrans;
               mySqlCmd.CommandText = sMySqlString;
               mySqlCmd.ExecuteNonQuery();

               mySqlTrans.Commit();

               mySqlCon.Close();
               mySqlTrans.Dispose();
               mySqlCmd.Dispose();

               blInserted = true;


           }
           catch (Exception ex)
           {
               if (mySqlCon.State == ConnectionState.Open)
               {
                   mySqlCon.Close();
               }

               mySqlTrans.Rollback();
               throw ex;
           }

           return blInserted;
       }
    }
}
