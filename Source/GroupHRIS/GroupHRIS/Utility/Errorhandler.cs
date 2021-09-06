using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using Common;

namespace GroupHRIS.Utility
{
    public class Errorhandler
    {
        public static void GetError(string ErrorNature, string ErrorText,  Control control)
        {
            Label lbl = null ;
            if (control.GetType() == typeof(Label))
            {
                lbl = (Label)control;

                if (ErrorNature == "0")
                {
                    lbl.BackColor = System.Drawing.Color.Pink ;
                    lbl.ForeColor = System.Drawing.Color.Red;
                    lbl.BorderColor = System.Drawing.Color.Red;
                    lbl.BorderWidth = 1;
                    lbl.Height = 25;
                    lbl.Visible = true;
                    
                    lbl.BorderStyle = BorderStyle.Solid;
                    lbl.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + ErrorText + "&nbsp;&nbsp;&nbsp;";
                    lbl.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Images/Common/error.png"); 
                }
                else  if (ErrorNature == "1")
                {
                    lbl.BackColor = System.Drawing.Color.LightGreen;
                    lbl.ForeColor = System.Drawing.Color.Green;
                    lbl.BorderColor = System.Drawing.Color.Green;
                    lbl.BorderWidth = 1;
                    lbl.Height = 25;
                    lbl.Visible = true;
                    
                    lbl.BorderStyle = BorderStyle.Solid;
                    lbl.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + ErrorText + "&nbsp;&nbsp;&nbsp;";
                    lbl.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Images/Common/success.png"); 
                }
                else if (ErrorNature == "2")
                {
                    lbl.BackColor = System.Drawing.Color.Gold;
                    lbl.ForeColor = System.Drawing.Color.Maroon;
                    lbl.BorderColor = System.Drawing.Color.Maroon;
                    lbl.BorderWidth = 1;
                    lbl.Height = 25;
                    lbl.Visible = true;

                    lbl.BorderStyle = BorderStyle.Solid;
                    lbl.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + ErrorText + "&nbsp;&nbsp;&nbsp;";
                    lbl.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Images/Common/Warning.png"); 
                }

            }
        }

        public static void ClearError(Control control)
        {
            Label lbl = null;

            if (control.GetType() == typeof(Label))
            {
                lbl = (Label)control;

                    lbl.BackColor = System.Drawing.Color.White;
                    lbl.ForeColor = System.Drawing.Color.White;
                    lbl.BorderColor = System.Drawing.Color.White;
                    lbl.Text = "";
                    lbl.Visible = false;

            }
        }
    }
}