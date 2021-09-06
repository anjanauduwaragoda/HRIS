using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataHandler.Userlogin;
using DataHandler.Payroll;
using System.Data;
using System.Runtime.InteropServices;
using System.IO;
using SpreadsheetLight;



namespace GroupHRIS.PayRoll
{
    /// <summary>
    /// Summary description for PayrollDownload
    /// </summary>
    public class PayrollDownload : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            System.Web.HttpRequest request = System.Web.HttpContext.Current.Request;
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;

            PasswordHandler PH = new PasswordHandler();
            PayrollDataHandler PDH = new PayrollDataHandler();
            DataTable dtPayrollData = new DataTable();
            string MappedPath = String.Empty;


            try
            {
                string CompanyID = request.QueryString["C"];
                string TransMonth = request.QueryString["T"];
                string FileName = request.QueryString["F"];
                MappedPath = request.QueryString["P"];

                CompanyID = PH.Decrypt(CompanyID);
                TransMonth = PH.Decrypt(TransMonth);
                FileName = PH.Decrypt(FileName);
                MappedPath = PH.Decrypt(MappedPath);

                dtPayrollData = PDH.GetPayrollDetails(CompanyID, TransMonth).Copy();


                SLDocument sl = new SLDocument();

                for (int i = 0; i < dtPayrollData.Rows.Count; i++)
                {
                    sl.SetCellValue("A" + (i + 1), dtPayrollData.Rows[i]["CATEGORY"].ToString());
                    sl.SetCellValue("B" + (i + 1), dtPayrollData.Rows[i]["EPF_NO"].ToString());
                    sl.SetCellValueNumeric("C" + (i + 1), dtPayrollData.Rows[i]["TYPE_ID"].ToString());
                    sl.SetCellValueNumeric("D" + (i + 1), dtPayrollData.Rows[i]["FINALIZED_AMOUNT"].ToString());
                }


                //Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                //if (xlApp == null)
                //{
                //    return;
                //}

                //Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                //Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                //object misValue = System.Reflection.Missing.Value;

                //xlWorkBook = xlApp.Workbooks.Add(misValue);
                //xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                ////xlWorkSheet.Cells[1, 1] = "ID";
                ////xlWorkSheet.Cells[1, 2] = "Name";
                ////xlWorkSheet.Cells[2, 1] = "1";
                ////xlWorkSheet.Cells[2, 2] = "One";
                ////xlWorkSheet.Cells[3, 1] = "2";
                ////xlWorkSheet.Cells[3, 2] = "Two";

                //for (int i = 0; i < dtPayrollData.Rows.Count; i++)
                //{
                //    xlWorkSheet.Cells[(i + 1), 1] = dtPayrollData.Rows[i]["CATEGORY"].ToString();
                //    xlWorkSheet.Cells[(i + 1), 2] = dtPayrollData.Rows[i]["EPF_NO"].ToString();
                //    xlWorkSheet.Cells[(i + 1), 3] = dtPayrollData.Rows[i]["TYPE_ID"].ToString();
                //    xlWorkSheet.Cells[(i + 1), 4] = dtPayrollData.Rows[i]["FINALIZED_AMOUNT"].ToString();
                //}


                if (Directory.Exists(Path.Combine(MappedPath, "PayrollDocuments")))
                {
                    Directory.Delete(Path.Combine(MappedPath, "PayrollDocuments"), true);
                }

                Directory.CreateDirectory(Path.Combine(MappedPath, "PayrollDocuments"));


                if (File.Exists(Path.Combine(MappedPath, "PayrollDocuments") + "/" + FileName))
                {
                    File.Delete(Path.Combine(MappedPath, "PayrollDocuments") + "/" + FileName);
                }


                sl.SaveAs(Path.Combine(MappedPath, "PayrollDocuments") + "/" + FileName);


                //xlWorkBook.SaveAs(Path.Combine(MappedPath,"PayrollDocuments")+"/" + FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                //xlWorkBook.Close(true, misValue, misValue);
                //xlApp.Quit();

                //Marshal.ReleaseComObject(xlWorkSheet);
                //Marshal.ReleaseComObject(xlWorkBook);
                //Marshal.ReleaseComObject(xlApp);

                ////MessageBox.Show("Excel file created , you can find the file d:\\csharp-Excel.xls");

                byte[] data = File.ReadAllBytes(Path.Combine(MappedPath, "PayrollDocuments") + "/" + FileName);

                if (data != null)
                {

                    response.Clear();
                    response.ClearHeaders();
                    response.ClearContent();
                    response.ContentType = "application/xls";

                    response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
                    response.AddHeader("content-length", data.Length.ToString());

                    response.BinaryWrite(data);
                    response.End();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                try
                {
                    Directory.Delete(Path.Combine(MappedPath, "PayrollDocuments"), true);
                }
                catch
                { 
                
                }

                PH = null;
                PDH = null;
                dtPayrollData.Dispose();
            }
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