using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupHRIS.DocumentDownload
{
    /// <summary>
    /// Summary description for FileDownloader
    /// </summary>
    public class FileDownloader : IHttpHandler, System.Web.SessionState.IRequiresSessionState 
    {

        public void ProcessRequest(HttpContext context)
        {
            string fileName = (context.Session["TemplateFileName"] as string);
            string filePath = (context.Session["TemplateFilePath"] as string);

            context.Response.Clear();
            context.Response.ContentType = "application/xlsx";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            context.Response.TransmitFile(filePath + fileName);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}