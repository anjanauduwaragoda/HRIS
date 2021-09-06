using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;

namespace GroupHRIS.Utility
{
    public class Utils
    {
        // Anjana Uduwaragoda 17/09/2014

        public static StringBuilder ExceptionLog(Exception exception)
        {
            StringBuilder sbExceptionMessage = new StringBuilder();
            try
            {
                do
                {
                    sbExceptionMessage.Append("Exception Type " + Environment.NewLine);
                    sbExceptionMessage.Append(exception.GetType().Name);
                    sbExceptionMessage.Append(Environment.NewLine);
                    sbExceptionMessage.Append("Message " + Environment.NewLine);
                    sbExceptionMessage.Append(exception.Message + Environment.NewLine);
                    sbExceptionMessage.Append("Stack Trace " + Environment.NewLine);
                    sbExceptionMessage.Append(exception.StackTrace + Environment.NewLine);

                    exception = exception.InnerException;
                }
                while (exception != null);

                return sbExceptionMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static Boolean verifyDate(String sDate)
        {
            Boolean isValid = true;

            if (sDate.Trim() != "")
            {
                String[] arrDate = sDate.Split('/');

                if (arrDate.Length != 3)
                {
                    isValid = false;
                }

                if (arrDate[0].Trim().Length != 4)
                {
                    isValid = false;
                }

                if (arrDate[1].Trim() != "")
                {
                    if ((int.Parse(arrDate[1].Trim()) > 12) || (int.Parse(arrDate[1].Trim()) < 1))
                    {
                        isValid = false;
                    }
                }

                if (arrDate[2].Trim() != "")
                {
                    if ((int.Parse(arrDate[2].Trim()) > 31) || (int.Parse(arrDate[1].Trim()) < 1))
                    {
                        isValid = false;
                    }
                }                
            }

            return isValid;

        }

        //
        // Anjana Uduwaragoda 16/12/2015
        public static Boolean verifyDateDDMMYYYY(String sDate)
        {
            Boolean isValid = true;

            if (sDate.Trim() != "")
            {
                String[] arrDate = sDate.Split('/');

                if (arrDate.Count() == 3)
                {

                    if (arrDate.Length != 3)
                    {
                        isValid = false;
                    }

                    if (arrDate[2].Trim().Length != 4)
                    {
                        isValid = false;
                    }
                    //else if (Int32.Parse(arrDate[2].Trim()) != DateTime.Today.Year)
                    //{
                    //    isValid = false;
                    //}

                    if (arrDate[1].Trim() != "")
                    {
                        if ((int.Parse(arrDate[1].Trim()) > 12) || (int.Parse(arrDate[1].Trim()) < 1))
                        {
                            isValid = false;
                        }
                    }

                    if (arrDate[0].Trim() != "")
                    {
                        if ((int.Parse(arrDate[0].Trim()) > 31) || (int.Parse(arrDate[0].Trim()) < 1))
                        {
                            isValid = false;
                        }
                    }
                }
                else
                {
                    isValid = false;
                }
            }

            return isValid;

        }







        // Anjana Uduwaragoda 17/06/2014
        // Function is implemented to clear given a dropdownlist, CheckBox, textbox, or all controls, or 
        // same controls attached to a control
        public static void clearControls(bool bClearddl, params Control[] control)
        {
            foreach (Control ctrl in control)
            {
                Label lb = null; // Clear Lables added by thanuja 2014.10.06
                TextBox tb = null;                
                CheckBox cbx = null;
                DropDownList ddl = null;
                Control ctrlFrame = null;
                HiddenField hf = null;

                if (ctrl.GetType() == typeof(Label)) // Clear Lables added by thanuja 2014.10.06
                {
                    lb = (Label)ctrl;
                    lb.Text = "";
                }
                else if (ctrl.GetType() == typeof(TextBox))
                {
                    tb = (TextBox)ctrl;
                    tb.Text ="";
                }
                else if (ctrl.GetType() == typeof(DropDownList))
                {
                    ddl = (DropDownList)ctrl;
                    if (bClearddl)
                    {
                        if (ddl.Items.Count > 0)
                        {
                            ddl.SelectedIndex = 0;
                        }
                    }
                }
                else if (ctrl.GetType() == typeof(CheckBox))
                {
                    cbx = (CheckBox)ctrl;
                    cbx.Checked = false;
                }               
                else if (ctrl is Control)
                {
                    ctrlFrame = (Control)ctrl;
                    foreach (Control con in ctrlFrame.Controls)
                    {

                        if (con.GetType() == typeof(Label))
                        {
                            lb = (Label)con;
                            lb.Text = "";
                        }
                        else if (con.GetType() == typeof(TextBox))
                        {
                            tb = (TextBox)con;
                            tb.Text = "";
                        }
                        else if (con.GetType() == typeof(DropDownList))
                        {
                            ddl = (DropDownList)con;
                            if (bClearddl)
                            {
                                if (ddl.Items.Count > 0)
                                {
                                    ddl.SelectedIndex = 0;
                                }
                            }
                        }
                        else if (con.GetType() == typeof(CheckBox))
                        {
                            cbx = (CheckBox)con;
                            cbx.Checked = false;
                        }
                        else if (con.GetType() == typeof(HiddenField))
                        {
                            hf = (HiddenField)con;
                            hf.Value = String.Empty;
                        }
                        
                    }
                }
            }
        }


        public static Boolean isValueExistInDropDownList(string sValue, DropDownList ddl )
        {
            Boolean blExist = false;

            try
            {
                foreach (ListItem li in ddl.Items)
                {
                    if (li.Value.ToString().Trim().Equals(sValue.Trim()))
                    {
                        blExist = true;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return blExist;

        }

        public static Boolean isTextExistInDropDownList(string sText, DropDownList ddl)
        {
            Boolean blExist = false;

            try
            {
                foreach (ListItem li in ddl.Items)
                {
                    if (li.Text.ToString().Trim().Equals(sText.Trim()))
                    {
                        blExist = true;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return blExist;
        }


        //Chathura Nawagamuwa 10/25/2016
        //Function is implement to check special characters in string
        public static Boolean isSpecialCharacterExists(string inputText)
        {
            Boolean Status = false;
            string SpecialChars = String.Empty;
            string MainString = String.Empty;
            try
            {
                SpecialChars = @"!@#$%^*()_+=][{}\|`~<>?";
                MainString = inputText.Trim();

                for (int i = 0; i < SpecialChars.Length; i++)
                {
                    if (MainString.Contains(SpecialChars[i]))
                    {
                        Status = true;
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                Status = true;
                throw exp;
            }
            finally
            {
                SpecialChars = String.Empty;
                MainString = String.Empty;
            }
            return Status;
        }
    }
}