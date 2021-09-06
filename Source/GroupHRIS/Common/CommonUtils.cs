using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Common
{
    public static class CommonUtils
    {
        private static string htmlSpace         = "&nbsp;";
        
        private static string htmlSingleQuote   = "&#39;";
        private static string singleQuoteChar   = "'";

        private static string htmlDblQuote      = "&quot;";
        private static string dblQuoteChar      = "\"";

        private static string htmlAmp           = "&amp;";
        private static string ampChar           = "&";

        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Removes special HTML characters from strings. Can be called as a String Extention  
        ///</summary>
        ///<param name="text">String with probable HTML characters</param>
        //----------------------------------------------------------------------------------------
        public static string removeInvalidHTMLChars(this string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                text = text.Replace(htmlSpace, String.Empty);
                text = text.Replace(htmlSingleQuote, singleQuoteChar);
                text = text.Replace(htmlDblQuote, dblQuoteChar);
                text = text.Replace(htmlAmp, ampChar);
            }

            return text;
        }


        /* YASINTHA */

        public static bool isValidDateRange(string fromDate, string toDate)
        {
            bool isValid = false;

            try
            {
                //DateTime dtFrom = DateTime.ParseExact(fromDate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture); //DateTime.Parse(fromDate);
                //DateTime dtTo = DateTime.ParseExact(toDate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture); //DateTime.Parse(toDate);


                DateTime dtFrom = DateTime.ParseExact(fromDate.Trim(), "yyyy/MM/dd", CultureInfo.InvariantCulture); //DateTime.Parse(fromDate);
                DateTime dtTo = DateTime.ParseExact(toDate.Trim(), "yyyy/MM/dd", CultureInfo.InvariantCulture); //DateTime.Parse(toDate);

                if (dtFrom <= dtTo)
                {
                    isValid = true;
                }
            }
            catch (Exception) { }


            return isValid;
        }



        public static bool isValidTimeRange(string fromTime, string toTime)
        {
            bool isValid = false;

            try
            {
                DateTime dtFrom = DateTime.Parse(fromTime);
                DateTime dtTo = DateTime.Parse(toTime);

                if (dtFrom < dtTo)
                    isValid = true;
            }
            catch (Exception) { }


            return isValid;
        }


        public static Dictionary<string, string> FamilyMembers()
        {
            Dictionary<string, string> FamilyMembers = new Dictionary<string, string>();

            FamilyMembers.Add(Constants.SPOUSE_ID, Constants.SPOUSE_TEXT);
            FamilyMembers.Add(Constants.SON_ID, Constants.SON_TEXT);
            FamilyMembers.Add(Constants.DAUGHTER_ID, Constants.DAUGHTER_TEXT);
            FamilyMembers.Add(Constants.FATHER_ID, Constants.FATHER_TEXT);
            FamilyMembers.Add(Constants.MOTHER_ID, Constants.MOTHER_TEXT);
            FamilyMembers.Add(Constants.GRAND_SON_ID, Constants.GRAND_SON_TEXT);
            FamilyMembers.Add(Constants.GRAND_DAUGHTER_ID, Constants.GRAND_DAUGHTER_TEXT);
            FamilyMembers.Add(Constants.GRAND_FATHER_ID, Constants.GRAND_FATHER_TEXT);
            FamilyMembers.Add(Constants.GRAND_MOTHER_ID, Constants.GRAND_MOTHER_TEXT);
            return FamilyMembers;
        }

      /**
       * **** Yasintha******
       * change date format dd/MM/yyyy to yyyy/MM/dd
       * 
       **/

        public static string dateFormatChange(string dateformat)
        {
            //DateTime dateValue = DateTime.ParseExact(dateformat.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //dateformat = dateValue.ToString().Trim();

            if (dateformat != null)
            {
                string[] dateArr = dateformat.Split('/','-');
                dateformat = dateArr[2] + "-" + dateArr[1] + "-" + dateArr[0];
            }
            return dateformat;
        }


        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Returns Current Financial Year  
        ///</summary>
        //----------------------------------------------------------------------------------------
        public static string currentFinancialYear()
        {
            String FinancialYear = String.Empty;
            try
            {
                System.DateTime dtfin = System.DateTime.Now;

                int CurrentFinyear = 0;

                DateTime finDate = DateTime.ParseExact(dtfin.Year + "-04-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                if (finDate > System.DateTime.Now)
                {
                    CurrentFinyear = dtfin.AddYears(-1).Year;                    
                    FinancialYear = CurrentFinyear.ToString();
                }
                else
                {
                    System.DateTime dt = System.DateTime.Now;                    
                    FinancialYear = dt.Year.ToString();
                }
                return FinancialYear;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                FinancialYear = String.Empty;
            }
        }

    }
}
