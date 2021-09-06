using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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



        public static bool isValidDateRange(string fromDate, string toDate)
        {
            bool isValid = false;

            try
            {
                DateTime dtFrom = DateTime.Parse(fromDate);
                DateTime dtTo = DateTime.Parse(toDate);

                if (dtFrom <= dtTo)
                    isValid = true;
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


    }
}
