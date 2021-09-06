using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using DataHandler.Attendance;
using Common;
using NLog;
using System.Data.OleDb;
using DataHandler.MetaData;
using DataHandler.Employee;

namespace GroupHRIS.Attendance
{
    public partial class webFrmAttendanceUploading : System.Web.UI.Page
    {
        OleDbConnection OLEDBCon;
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";
        string Filesavepath = "C:\\Temp\\Upload";
        private static DataTable DtExcel = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "webFrmEmployeeLeaveSheet : Page_Load");

            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {    
                sUserId = Session["KeyUSER_ID"].ToString();
            }

            if (!IsPostBack)
            {   
                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        fillCompanies();
                    }
                    else
                    {
                        fillCompanies(Session["KeyCOMP_ID"].ToString().Trim());
                        dlCompany.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                    }
                }
            }
        }

        //------------------------------------------------------------------------------
        ///<summary>
        ///Load all companies 
        ///</summary>

        //----------------------------------------------------------------------------------------
        private void fillCompanies()
        {
            log.Debug("fillCompanies() -> all companies");

            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable companies = new DataTable();
            Utility.Errorhandler.ClearError(lblMessage);
            txtContent.Text = "";
            try
            {
                companies = companyDataHandler.getCompanyIdCompName().Copy();

                dlCompany.Items.Clear();

                if (companies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    dlCompany.Items.Add(Item);

                    foreach (DataRow dataRow in companies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        dlCompany.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                companyDataHandler = null;
                companies.Dispose();
            }

        }

        //------------------------------------------------------------------------------
        ///<summary>
        ///Load a companies 
        ///</summary>
        ///<param name="companyId">Pass a company id string to query </param>
        //----------------------------------------------------------------------------------------

        private void fillCompanies(string companyId)
        {
            log.Debug("fillCompanies() -> for a given company_Id");

            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable companies = new DataTable();

            try
            {
                companies = companyDataHandler.getCompanyIdCompName(companyId).Copy();

                dlCompany.Items.Clear();

                if (companies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    dlCompany.Items.Add(Item);

                    foreach (DataRow dataRow in companies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        dlCompany.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                companyDataHandler = null;
                companies.Dispose();
            }

        }



        protected void btnUpload_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter dataAdapter = null;

            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();


            Utility.Errorhandler.ClearError(lblMessage);
            string excelSource = "";
            
            string companyId = "";
            List<DataHandler.Attendance.Attendance> attendance = new List<DataHandler.Attendance.Attendance>();
            try
            {
                employeeDataHandler = new EmployeeDataHandler();

                companyId = dlCompany.SelectedValue;


                if (FileUploader.HasFile)
                {
                    String fileName = FileUploader.PostedFile.FileName;

                    AttendanceDataHandler attendanceDataHandler = new AttendanceDataHandler();

                    if (veryfyFileType(fileName) && veryfyFileName(fileName, companyId))
                    {


                        Stream stream = default(Stream);
                        stream = FileUploader.PostedFile.InputStream;
                        StreamReader streamReader = new StreamReader(stream);

                        string line = "";

                        StringBuilder stringBuilder = new StringBuilder();

                        stringBuilder.Append("Company " + dlCompany.SelectedValue + " " + dlCompany.SelectedItem + Environment.NewLine + Environment.NewLine);

                        while ((line = streamReader.ReadLine()) != null)
                        {


                            if (line.Trim() != String.Empty)
                            {
                                stringBuilder.Append(line + Environment.NewLine);

                                String[] record;

                                DataHandler.Attendance.Attendance oAttendance = new DataHandler.Attendance.Attendance();

                                if (companyId == Constants.CON_SMJ_COMPANY_ID)
                                {
                                    record = line.Split(',');
                                    
                                    if (record[1].Trim().Length == Constants.CON_SMJ_EMPNO_LENGTH)
                                    {
                                        
                                        string[] sDate = record[3].Trim().Split('/');

                                        int iYear = int.Parse(sDate[0].Trim());
                                        int iMonth = int.Parse(sDate[1].Trim());
                                        int iDate = int.Parse(sDate[2].Trim());

                                        string[] sTime = record[4].Trim().Split(':');

                                        int iHour = int.Parse(sTime[0].Trim());
                                        int iMinute = int.Parse(sTime[1].Trim());
                                        int iSecond = int.Parse(sTime[2].Trim());

                                        DateTime dtAttDateTime = new DateTime(iYear, iMonth, iDate, iHour, iMinute, iSecond);

                                        string empId = "";

                                        if (record[1].Trim().Length < 6)
                                        {
                                            empId = record[1].Trim().PadLeft(6, '0');
                                        }
                                        else
                                        {
                                            empId = record[1].Trim();
                                        }

                                        oAttendance.Employee_id = Constants.EMPLOYEE_ID_STAMP + empId.Trim();

                                        string sCompId = "";

                                        sCompId = employeeDataHandler.getEmployeeCompany(Constants.EMPLOYEE_ID_STAMP + empId.Trim());

                                        oAttendance.Att_Date = record[3].Trim();
                                        oAttendance.Att_Time = record[4].Trim();
                                        oAttendance.Direction = record[2].Trim();
                                        oAttendance.Com_Id = sCompId.Trim();
                                        oAttendance.Branch_Id = record[0].Trim();
                                        oAttendance.Reason_Code = record[2].Trim();
                                        oAttendance.AttDateTime = dtAttDateTime;

                                        attendance.Add(oAttendance);
                                    }
                                }
                                else if (companyId == Constants.CON_SFS_COMPANY_ID)
                                {
                                    record = line.Split(' ');
                                    if (record[1].Trim().Length == Constants.CON_SFS_EMPNO_LENGTH)
                                    {
                                        string[] sDate = record[2].Trim().Split('-');

                                        int iYear = int.Parse(sDate[0].Trim());
                                        int iMonth = int.Parse(sDate[1].Trim());
                                        int iDate = int.Parse(sDate[2].Trim());

                                        string[] sTime = record[3].Trim().Split(':');

                                        int iHour = int.Parse(sTime[0].Trim());
                                        int iMinute = int.Parse(sTime[1].Trim());
                                        int iSecond = int.Parse(sTime[2].Trim());

                                        DateTime dtAttDateTime = new DateTime(iYear, iMonth, iDate, iHour, iMinute, iSecond);

                                        string empId = "";

                                        if (record[1].Trim().Length < 6)
                                        {
                                            empId = record[1].Trim().PadLeft(6, '0');
                                        }
                                        else
                                        {
                                            empId = record[1].Trim();
                                        }

                                        oAttendance.Employee_id = Constants.EMPLOYEE_ID_STAMP + empId.Trim();

                                        string sCompId = "";

                                        sCompId = employeeDataHandler.getEmployeeCompany(Constants.EMPLOYEE_ID_STAMP + empId.Trim());


                                        oAttendance.Att_Date = record[2].Trim();
                                        oAttendance.Att_Time = record[3].Trim();
                                        oAttendance.Direction = record[4].Trim();
                                        oAttendance.Com_Id = sCompId.Trim();
                                        oAttendance.Branch_Id = record[0].Trim();
                                        oAttendance.Reason_Code = record[4].Trim();
                                        oAttendance.AttDateTime = dtAttDateTime;

                                        attendance.Add(oAttendance);
                                    }
                                }
                                else if (companyId == Constants.CON_SSL_COMPANY_ID)
                                {
                                    //record = line.Split('\t');
                                    //if (record != null)
                                    //{
                                        record = line.Split(',');
                                        if (record[1].Trim().Length == Constants.CON_SSL_EMPNO_LENGTH)
                                        {
                                            //string sRec = line.ToString();
                                            //string f0 = record[0].Trim();
                                            //string f1 = record[1].Trim();
                                            //string f2 = record[2].Trim();
                                            //string f3 = record[3].Trim();
                                            //string f4 = record[4].Trim();
                                            //string date_ = record[2].Trim();
                                            //string[] Date = record[1].Trim().Split(' ');
                                            //string sDate = Date[0].Trim();
                                            //string Time_ = Date[1].Trim();
                                            //string[] sDate_ = sDate.Split('-');

                                            string[] sDate_ = record[3].Trim().Split('/');

                                            int iYear = int.Parse(sDate_[0].Trim());
                                            int iMonth = int.Parse(sDate_[1].Trim());
                                            int iDate = int.Parse(sDate_[2].Trim());

                                            string[] sTime_ = record[4].Trim().Split(':');

                                            int iHour = int.Parse(sTime_[0].Trim());
                                            int iMinute = int.Parse(sTime_[1].Trim());
                                            int iSecond = int.Parse(sTime_[2].Trim());

                                            DateTime dtAttDateTime = new DateTime(iYear, iMonth, iDate, iHour, iMinute, iSecond);

                                            string empId = "";

                                            if (record[1].Trim().Length < 6)
                                            {
                                                empId = record[1].Trim().PadLeft(6, '0');
                                            }
                                            else
                                            {
                                                empId = record[1].Trim();
                                            }

                                            oAttendance.Employee_id = Constants.EMPLOYEE_ID_STAMP + empId.Trim();

                                            string sCompId = "";

                                            sCompId = employeeDataHandler.getEmployeeCompany(Constants.EMPLOYEE_ID_STAMP + empId.Trim());


                                            oAttendance.Att_Date = record[3].Trim();
                                            oAttendance.Att_Time = record[4].Trim();
                                            oAttendance.Direction = record[2].Trim();
                                            oAttendance.Com_Id = sCompId.Trim();
                                            oAttendance.Branch_Id = record[0].Trim();
                                            oAttendance.Reason_Code = record[2].Trim();
                                            oAttendance.AttDateTime = dtAttDateTime;

                                            attendance.Add(oAttendance);
                                        }
                                    //}
                                }

                            }

                        }

                        streamReader.Close();

                        txtContent.Text = stringBuilder.ToString();



                        if (attendanceDataHandler.Insert(attendance) == true)
                        {
                            //lblMessage.Text = "File Uploaded ";
                            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('File Uploaded ..')", true);
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Attendance file is uploaded", lblMessage);
                        }
                        else
                        {
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You are trying to uploade an invalid file", lblMessage);
                        }
                    }
                    else if (veryfyFileExcelType(fileName,companyId))
                    {
                        // loding data from excel file
                        StringBuilder stringBuilder = new StringBuilder();
                        string line = "";

                        stringBuilder.Append("Company " + dlCompany.SelectedValue + " " + dlCompany.SelectedItem + Environment.NewLine + Environment.NewLine);

                        DataHandler.Attendance.Attendance oAttendance = new DataHandler.Attendance.Attendance();
                        string sFN = FileUploader.PostedFile.FileName.ToString();
                        
                        FileInfo fi = new FileInfo(fileName);
                        string extention = fi.Extension;

                        excelSource = Filesavepath + "\\" + fileName;

                        if ((System.IO.File.Exists(excelSource)))
                        {
                            System.IO.File.Delete(excelSource);
                        }

                        FileUploader.SaveAs(excelSource);

                        string conString = "";

                        if (extention.ToLower().Equals(".xls"))
                        {
                            conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelSource + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        else if (extention.ToLower().Equals(".xlsx"))
                        {
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelSource + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';";
                        }

                        OLEDBCon = new OleDbConnection(conString);

                        //GetSheetNames(excelSource);
                        //string query = String.Format("select * from [{0}]", ddlexcelsheet.SelectedItem);
                        string query = String.Format("SELECT * FROM [Sheet1$]");
                        dataAdapter = new OleDbDataAdapter(query, OLEDBCon);
                        DtExcel = new DataTable();
                        dataAdapter.Fill(DtExcel);

                        if (DtExcel.Rows.Count > 0)
                        {
                            foreach (DataRow dr in DtExcel.Rows)
                            {
                                string[] sDate = dr[6].ToString().Trim().Split('-');

                                int iYear = int.Parse(sDate[0].Trim());
                                int iMonth = int.Parse(sDate[1].Trim());
                                int iDate = int.Parse(sDate[2].Trim());

                                string StartTime = dr[7].ToString().Trim();
                                DateTime dt = new DateTime();
                                try { dt = Convert.ToDateTime(StartTime); }
                                catch (FormatException) { /*dt = Convert.ToDateTime("12:00 AM"); */}
                                StartTime = dt.ToString("HH:mm");

                                string[] sTime = StartTime.Trim().Split(':');

                                int iHour = int.Parse(sTime[0].Trim());
                                int iMinute = int.Parse(sTime[1].Trim());
                                int iSecond = 0;

                                DateTime dtAttDateTime = new DateTime(iYear, iMonth, iDate, iHour, iMinute, iSecond);

                                oAttendance.Employee_id = Constants.EMPLOYEE_ID_STAMP + dr[3].ToString().Trim().PadLeft(6,'0');

                                string sCompId = "";

                                sCompId = employeeDataHandler.getEmployeeCompany(Constants.EMPLOYEE_ID_STAMP + dr[3].ToString().Trim().PadLeft(6, '0'));
                                 
                                oAttendance.Att_Date = dr[6].ToString().Trim();
                                oAttendance.Att_Time = StartTime;
                                oAttendance.Direction = "1";
                                oAttendance.Com_Id = sCompId.Trim();
                                oAttendance.Branch_Id = dr[4].ToString().Trim();
                                oAttendance.Reason_Code = "1";
                                oAttendance.AttDateTime = dtAttDateTime;

                                attendance.Add(oAttendance);

                                line = Constants.EMPLOYEE_ID_STAMP + dr[3].ToString().Trim().PadLeft(6,'0') + ',' + dr[6].ToString().Trim() + ',' +
                                       StartTime.Trim() + ',' + "1" + ',' + companyId.Trim() + ',' + dr[4].ToString().Trim() + ',' + "1" + dtAttDateTime.ToString();

                                stringBuilder.Append(line + Environment.NewLine);
                            }

                        }
                        
                        txtContent.Text = stringBuilder.ToString();

                        //if (attendanceDataHandler.Insert(attendance) == true)
                        //{
                        //    //lblMessage.Text = "File Uploaded ";
                        //    //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('File Uploaded ..')", true);
                        //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Attendance file is uploaded", lblMessage);
                        //}
                        //else
                        //{
                        //    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You are trying to uploade an invalid file", lblMessage);
                        //}

                       
                    }
                    else
                    {
                        //lblMessage.Text = "You are trying to uploade an invalid file";
                       // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('You are trying to uploade an invalid file ..')", true);
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "You are trying to uploade an invalid file", lblMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] GetSheetNames(string excelFileName)
        {
            DataTable dt = null;
            String conStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + excelFileName + ";Extended Properties=Excel 8.0;";
            OLEDBCon = new OleDbConnection(conStr);
            OLEDBCon.Open();
            dt = OLEDBCon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (dt == null)
            {
                return null;
            }

            String[] excelSheetNames = new String[dt.Rows.Count];
            int i = 0;

            foreach (DataRow row in dt.Rows)
            {
                excelSheetNames[i] = row["TABLE_NAME"].ToString();
                i++;
            }

            dt.Dispose();

            return excelSheetNames;
        }


        private bool veryfyFileExcelType()
        {
            throw new NotImplementedException();
        }

        protected Boolean veryfyFileExcelType(String fileName,String companyId)
        {
            Boolean isFileValid = false;

            if (fileName.Trim() != "")
            {
                if ((companyId == Constants.CON_SAPPHIRE_COMPANY_ID) || (companyId == Constants.CON_CONCORD_COMPANY_ID))
                {
                    FileInfo fi = new FileInfo(fileName);
                    string extention = fi.Extension;

                    if ((extention.ToLower().Equals(".xls")) || (extention.ToLower().Equals(".xlsx")))
                    {
                        isFileValid = true;
                    }
                }
            }

            return isFileValid;
        }


        protected Boolean veryfyFileType(String fileName)
        {
            Boolean isFileValid = false;

            if (fileName.Trim() != "")
            {
                FileInfo fi = new FileInfo(fileName);
                string extention = fi.Extension;

                if (extention.ToLower().Equals(".txt"))
                {
                    isFileValid = true;
                }                
            }
            
            return isFileValid;
        }

        protected Boolean veryfyFileName(String fileName, String company_Id)
        {
            Boolean isFileValid = false;


            if ((fileName.Substring(0, 3) == "TNA") && (company_Id == Constants.CON_SMJ_COMPANY_ID))
            {
                isFileValid = true;
            }
            else if ((fileName.Substring(0, 4) == DateTime.Today.Year.ToString()) && (company_Id == Constants.CON_SFS_COMPANY_ID))
            {
                isFileValid = true;
            }
            else if ((fileName.Substring(0, 3) == "TNA") && (company_Id == Constants.CON_SSL_COMPANY_ID))
            {
                isFileValid = true;
            }
            return isFileValid;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMessage);
            txtContent.Text = "";

        }                

    }
}