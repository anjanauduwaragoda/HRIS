using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using DataHandler.MetaData;
using System.Data;
using Common;
using System.Data.OleDb;
using DataHandler.Employee;
using System.Data.Common;


namespace GroupHRIS.Employee
{
    public partial class WebFrmDataUpload : System.Web.UI.Page
    {
        OleDbConnection OLEDBCon = new OleDbConnection();
        private static Logger log = LogManager.GetCurrentClassLogger();
        string Filesavepath = "C:\\Temp\\Upload";
        private static DataTable DtExcel = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            log.Debug("AttendanceUpload : Page_Load()");

            if (!IsPostBack)
            {
                string KeyCOMP_ID = (string)(Session["KeyCOMP_ID"]);
                getCompID(KeyCOMP_ID);

                Session["TemplateFileName"] = "UploadEmployees.xlsx";
                Session["TemplateFilePath"] = Server.MapPath("~/TemplateDocuments/");
            }
        }

        private void getCompID(string KeyCOMP_ID)
        {
            log.Debug("AttendanceUpload : getCompID()");

            CompanyDataHandler companynameid = new CompanyDataHandler();
            DataTable CompID = new DataTable();
            try
            {
                if (KeyCOMP_ID == Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    CompID = companynameid.getCompanyIdCompName();
                    ListItem lstItem = new ListItem();
                    lstItem.Text = Constants.CON_UNIVERSAL_COMPANY_NAME;
                    lstItem.Value = Constants.CON_UNIVERSAL_COMPANY_CODE;
                    dpCompID.Items.Add(lstItem);
                }
                else
                {
                    CompID = companynameid.getCompanyIdCompName(KeyCOMP_ID);
                }

                foreach (DataRow dataRow in CompID.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = dataRow["COMP_NAME"].ToString();
                    listItem.Value = dataRow["COMPANY_ID"].ToString();
                    dpCompID.Items.Add(listItem);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                companynameid = null;
                CompID.Dispose();
                CompID = null;
            }
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            try
            {
                Utility.Errorhandler.ClearError(lblerror);
                DtExcel = null;
                GridView1.DataSource = DtExcel;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void btnupload_Click(object sender, EventArgs e)
        {
            log.Debug("AttendanceUpload : btnupload_Click()");

            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
            DepartmentDataHandler departmentDataHandler = new DepartmentDataHandler();
            DivisionDataHandler divisionDataHandler = new DivisionDataHandler();
            EmployeeRoleDataHandler employeeRoleDataHandler = new EmployeeRoleDataHandler();
            EmployeeTypeDataHandler employeeTypeDataHandler = new EmployeeTypeDataHandler();
            DesignationDataHandler designationDataHandler = new DesignationDataHandler();
            CompanyLocationDataHandler companyLocationDataHandler = new CompanyLocationDataHandler();
            BranchCenterDataHandler branchCenterDataHandler = new BranchCenterDataHandler();

            DataTable UploadTable = new DataTable("UploadSummary");
            UploadTable.Columns.Add("sNIC");
            UploadTable.Columns.Add("sKNOWNNAME");
            UploadTable.Columns.Add("sEPFNO");
            UploadTable.Columns.Add("sDESCRIP");

            string sErrorMessage = "";
            string EMP_NIC = "";
            string EPF_NO = "";
            string KNOWN_NAME = "";
            string PROBATION_CONTRACT_ENDDATE = "";

            try
            {
                if (DtExcel.Rows.Count > 1)
                {
                    for (int i = 0; i < DtExcel.Rows.Count; i++)
                    {
                        string EMPLOYEE_ID = DtExcel.Rows[i][0].ToString().Trim();
                        string COMPANY_ID = DtExcel.Rows[i][1].ToString().Trim();
                        if (String.IsNullOrEmpty(COMPANY_ID))
                        {
                            CommonVariables.MESSAGE_TEXT = "Company ID is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        string DEPT_ID = DtExcel.Rows[i][2].ToString().Trim();
                        if (String.IsNullOrEmpty(DEPT_ID))
                        {
                            CommonVariables.MESSAGE_TEXT = "Department ID is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        string DIVISION_ID = DtExcel.Rows[i][3].ToString().Trim();
                        if (String.IsNullOrEmpty(DIVISION_ID))
                        {
                            CommonVariables.MESSAGE_TEXT = "Division ID is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        string EMPLOYEE_STATUS = DtExcel.Rows[i][4].ToString().Trim();
                        if (String.IsNullOrEmpty(EMPLOYEE_STATUS))
                        {
                            CommonVariables.MESSAGE_TEXT = "Employee Status is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        string EMP_TYPE_ID = DtExcel.Rows[i][5].ToString().Trim();
                        if (String.IsNullOrEmpty(EMPLOYEE_STATUS))
                        {
                            CommonVariables.MESSAGE_TEXT = "Employee Type ID is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        string ROLE_ID = DtExcel.Rows[i][6].ToString().Trim();
                        if (String.IsNullOrEmpty(ROLE_ID))
                        {
                            CommonVariables.MESSAGE_TEXT = "Role ID is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        string TITLE = DtExcel.Rows[i][7].ToString().Trim();
                        if (String.IsNullOrEmpty(TITLE))
                        {
                            CommonVariables.MESSAGE_TEXT = "Title is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        string FULL_NAME = DtExcel.Rows[i][8].ToString().Trim();
                        if (String.IsNullOrEmpty(FULL_NAME))
                        {
                            CommonVariables.MESSAGE_TEXT = "Full Name is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        string GENDER = DtExcel.Rows[i][9].ToString().Trim().Substring(0, 1);
                        if (String.IsNullOrEmpty(GENDER))
                        {
                            CommonVariables.MESSAGE_TEXT = "Gender is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        EMP_NIC = DtExcel.Rows[i][10].ToString().Trim();
                        if (String.IsNullOrEmpty(EMP_NIC))
                        {
                            CommonVariables.MESSAGE_TEXT = "NIC No. is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }

                        string PASSPORT_NUMBER = DtExcel.Rows[i][11].ToString().Trim();


                        string DOBN = DtExcel.Rows[i][12].ToString().Trim();
                        string DOB = String.Empty;
                        if (String.IsNullOrEmpty(DOBN))
                        {
                            CommonVariables.MESSAGE_TEXT = "Date of Birth is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        else
                        {
                            DOB = DateTime.Parse(DOBN).ToString("yyyy-MM-dd");
                        }

                        string DOJN = DtExcel.Rows[i][13].ToString().Trim();
                        string DOJ = String.Empty;
                        if (String.IsNullOrEmpty(DOJN))
                        {
                            CommonVariables.MESSAGE_TEXT = "Date of Joined is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        else
                        {
                            DOJ = DateTime.Parse(DOJN).ToString("yyyy-MM-dd");
                        }

                        string MARITAL_STATUS = DtExcel.Rows[i][14].ToString().Trim();
                        if (String.IsNullOrEmpty(MARITAL_STATUS))
                        {
                            CommonVariables.MESSAGE_TEXT = "Marital Status is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        string NAIONALITY = DtExcel.Rows[i][15].ToString().Trim();
                        string RELIGION = DtExcel.Rows[i][16].ToString().Trim();
                        string EMAIL = DtExcel.Rows[i][17].ToString().Trim();
                        EPF_NO = DtExcel.Rows[i][18].ToString().Trim();
                        if (String.IsNullOrEmpty(EPF_NO))
                        {
                            CommonVariables.MESSAGE_TEXT = "EPF No. is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        string ETF_NO = DtExcel.Rows[i][19].ToString().Trim();
                        if (String.IsNullOrEmpty(ETF_NO))
                        {
                            CommonVariables.MESSAGE_TEXT = "ETF No. is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        string PERMANENT_ADDRESS = DtExcel.Rows[i][20].ToString().Trim();
                        string CURRENT_ADDRESS = DtExcel.Rows[i][21].ToString().Trim();
                        string LAND_PHONE = DtExcel.Rows[i][22].ToString().Trim().Replace("-", "");
                        string MOBILE_PHONE_PERSONAL = DtExcel.Rows[i][23].ToString().Trim().Replace("-", "");
                        string MOBILE_PHONE_COMPANY = DtExcel.Rows[i][24].ToString().Trim().Replace("-", "");

                        string FUEL_CARD_NUMBER = DtExcel.Rows[i][25].ToString().Trim();
                        string REPORT_TO_1 = "";
                        string REPORT_TO_2 = "";
                        string REPORT_TO_3 = "";
                        string CITY = DtExcel.Rows[i][29].ToString().Trim();
                        string DISTANCE_TO_OFFICE = DtExcel.Rows[i][30].ToString().Trim();
                        string REMARKS = DtExcel.Rows[i][31].ToString().Trim();
                        string COST_CENTER = DtExcel.Rows[i][32].ToString().Trim();
                        string PROFIT_CENTER = DtExcel.Rows[i][33].ToString().Trim();
                        string IS_WELFARE = DtExcel.Rows[i][34].ToString().Trim();
                        string BRANCH_ID = DtExcel.Rows[i][35].ToString().Trim();
                        if (String.IsNullOrEmpty(BRANCH_ID))
                        {
                            CommonVariables.MESSAGE_TEXT = "Brainch ID is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        string LOCATION_ID = DtExcel.Rows[i][36].ToString().Trim();
                        string DESIGNATION_ID = DtExcel.Rows[i][37].ToString().Trim();
                        if (String.IsNullOrEmpty(DESIGNATION_ID))
                        {
                            CommonVariables.MESSAGE_TEXT = "Designation ID is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        string INITIALS_NAME = DtExcel.Rows[i][38].ToString().Trim();
                        if (String.IsNullOrEmpty(INITIALS_NAME))
                        {
                            CommonVariables.MESSAGE_TEXT = "Name with initials is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                        KNOWN_NAME = DtExcel.Rows[i][39].ToString().Trim();
                        if (String.IsNullOrEmpty(KNOWN_NAME))
                        {
                            CommonVariables.MESSAGE_TEXT = "Known Name is required";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }

                        if (DtExcel.Rows[i][40].ToString().Trim() != "")
                        {
                            DateTime PROBATION_CONTRACT_ENDDATEDT = DateTime.Parse(DtExcel.Rows[i][40].ToString().Trim());
                            PROBATION_CONTRACT_ENDDATE = PROBATION_CONTRACT_ENDDATEDT.ToString("yyyy-MM-dd");
                        }

                        string MOD_CAT_ID = "";
                        string RESIGNED_DATE = "";
                        string EMP_INITIALS = "";
                        string FIRST_NAME = "";
                        string MIDDLE_NAMES = "";
                        string LAST_NAME = "";
                        string ADDED_BY = Session["KeyUSER_ID"].ToString();

                        //validate NIC
                        if (employeeDataHandler.isNicExist(EMP_NIC))
                        {
                            sErrorMessage = " NIC Already Exist.";
                            UploadTable.Rows.Add(EMP_NIC, KNOWN_NAME, EPF_NO, sErrorMessage);
                        }
                        else if (employeeDataHandler.isEpfExist(COMPANY_ID, EPF_NO))
                        {
                            sErrorMessage = " E.P.F. No. already Exist in Company.";
                            UploadTable.Rows.Add(EMP_NIC, KNOWN_NAME, EPF_NO, sErrorMessage);
                        }
                        else if (!employeeTypeDataHandler.isEmpTypeExist(EMP_TYPE_ID))
                        {
                            sErrorMessage = " Invalid employee type Id.";
                            UploadTable.Rows.Add(EMP_NIC, KNOWN_NAME, EPF_NO, sErrorMessage);
                        }
                        else if (!employeeRoleDataHandler.isRoleIDExist(ROLE_ID))
                        {
                            sErrorMessage = " Invalid employee role Id.";
                            UploadTable.Rows.Add(EMP_NIC, KNOWN_NAME, EPF_NO, sErrorMessage);
                        }
                        else if (!designationDataHandler.isDesigIDExist(COMPANY_ID, DESIGNATION_ID))
                        {
                            sErrorMessage = " Invalid employee designation Id.";
                            UploadTable.Rows.Add(EMP_NIC, KNOWN_NAME, EPF_NO, sErrorMessage);
                        }
                        //else if (!companyLocationDataHandler.isCompanyLocationExist(COMPANY_ID, LOCATION_ID))
                        //{
                        //    sErrorMessage = " Invalid employee company location Id.";
                        //    UploadTable.Rows.Add(EMP_NIC, KNOWN_NAME, EPF_NO, sErrorMessage);
                        //}
                        else if (!branchCenterDataHandler.isCompanyBranchExist(COMPANY_ID, BRANCH_ID))
                        {
                            sErrorMessage = " Invalid employee branch location Id.";
                            UploadTable.Rows.Add(EMP_NIC, KNOWN_NAME, EPF_NO, sErrorMessage);
                        }
                        else if (!departmentDataHandler.populate(COMPANY_ID, DEPT_ID))
                        {
                            sErrorMessage = " Department " + DEPT_ID + " not belongs to this Company. " + COMPANY_ID;
                            UploadTable.Rows.Add(EMP_NIC, KNOWN_NAME, EPF_NO, sErrorMessage);
                        }
                        else if (!divisionDataHandler.populate(DEPT_ID, DIVISION_ID))
                        {
                            sErrorMessage = " Division " + DIVISION_ID + " not belongs to this department. " + DEPT_ID;
                            UploadTable.Rows.Add(EMP_NIC, KNOWN_NAME, EPF_NO, sErrorMessage);
                        }
                        else
                        {
                            String sOtSession = "";

                            Boolean isInserted = employeeDataHandler.Insert(COMPANY_ID, DEPT_ID, DIVISION_ID, EMPLOYEE_STATUS, EMP_TYPE_ID, ROLE_ID, TITLE, EMP_INITIALS, FIRST_NAME,
                                                                    MIDDLE_NAMES, LAST_NAME, FULL_NAME, GENDER, EMP_NIC, PASSPORT_NUMBER, DOB, DOJ, MARITAL_STATUS,
                                                                    NAIONALITY, RELIGION, EMAIL, EPF_NO, ETF_NO, PERMANENT_ADDRESS, CURRENT_ADDRESS, LAND_PHONE,
                                                                    MOBILE_PHONE_PERSONAL, MOBILE_PHONE_COMPANY, FUEL_CARD_NUMBER, REPORT_TO_1, REPORT_TO_2, REPORT_TO_3, CITY,
                                                                    DISTANCE_TO_OFFICE, REMARKS, ADDED_BY, COST_CENTER, PROFIT_CENTER, IS_WELFARE, RESIGNED_DATE,
                                                                    DESIGNATION_ID, BRANCH_ID, LOCATION_ID, INITIALS_NAME, KNOWN_NAME, MOD_CAT_ID, PROBATION_CONTRACT_ENDDATE, Constants.OT_NOT_ELIGIBLE.ToString(), Constants.REGULAR_EMPLOYEE, Constants.CON_MAIL_INCLUDE,sOtSession);
                            if (!isInserted)
                            {
                                sErrorMessage = "Error 1 : Unable to upload employee. " + EMP_NIC;
                                UploadTable.Rows.Add(EMP_NIC, KNOWN_NAME, EPF_NO, sErrorMessage);
                            }
                            else
                            {
                                sErrorMessage = "OK. " + EMP_NIC;
                                UploadTable.Rows.Add(EMP_NIC, KNOWN_NAME, EPF_NO, sErrorMessage);

                            }

                        }

                    }

                    CommonVariables.MESSAGE_TEXT = "Data Upload Completed.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);

                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Excel File / Sheet data not fetched.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }
            }
            catch (Exception)
            {
                sErrorMessage = "Error 2 : Unable to process employee data. ";
                UploadTable.Rows.Add(EMP_NIC, KNOWN_NAME, EPF_NO, sErrorMessage);
                CommonVariables.MESSAGE_TEXT = sErrorMessage;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                GridView1.DataSource = UploadTable.Copy();
                GridView1.DataBind();

                employeeDataHandler = null;
                departmentDataHandler = null;
                divisionDataHandler = null;
                employeeRoleDataHandler = null;
                employeeTypeDataHandler = null;
                designationDataHandler = null;
                companyLocationDataHandler = null;
                branchCenterDataHandler = null;

                UploadTable.Dispose();
                UploadTable = null;

            }
        }


        private void create_folder()
        {
            if (!System.IO.Directory.Exists(Filesavepath))
            {
                System.IO.Directory.CreateDirectory(Filesavepath);
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

        protected void btnloaddata_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter dataAdapter = null;
            string strFileName = "";
            string excelSource = "";

            try
            {
                create_folder();
                DataSet dsExcel = new DataSet();
                if (FileUpload1.HasFile)
                {
                    strFileName = FileUpload1.PostedFile.FileName.ToString();
                    excelSource = Filesavepath + "\\" + strFileName;
                    if ((System.IO.File.Exists(excelSource)))
                    {
                        System.IO.File.Delete(excelSource);
                    }

                    FileUpload1.SaveAs(excelSource);

                    GetSheetNames(excelSource);
                    //string query = String.Format("select * from [{0}]", ddlexcelsheet.SelectedItem);

                    var connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelSource + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=NO;TypeGuessRows=0;ImportMixedTypes=Text\"";

                    string query = String.Format("SELECT * FROM [Sheet1$]");
                    dataAdapter = new OleDbDataAdapter(query, OLEDBCon);

                    DtExcel = new DataTable();
                    

                    dataAdapter.Fill(dsExcel);
                    DtExcel = dsExcel.Tables[0];
                    CommonVariables.MESSAGE_TEXT = "Data Successfully uploaded , Click <Process> to save data. ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Excel File / Sheet not selected.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                //DtExcel.Dispose();
                //dataAdapter.Dispose();
                OLEDBCon.Close();
                OLEDBCon.Dispose();
            }

        }

        protected void dpCompID_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblerror);
        }
    }
}