using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using Common;
using System.IO;
using System.Data.OleDb;
using System.Data.Common;
using System.Data;
using DataHandler.PerformanceManagement;
using System.Drawing;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmUploadEmployeeGoals : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    log.Debug("IP:" + Request.UserHostAddress + "WebFrmUploadEmployeeGoals : Page_Load()");
                    Session["EmployeeGoalFilePath"] = String.Empty;
                    Session["ExcelHasErrors"] = "false";

                    Session["TemplateFileName"] = "UploadEmployeeGoals.xlsx";
                    Session["TemplateFilePath"] = Server.MapPath("~/TemplateDocuments/");
                }
            }
            catch (Exception exp)
            {
                log.Error("WebFrmUploadEmployeeGoals : Page_Load : " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        private DataTable LoadExcel(string FilePath)
        {
            DataSet ds = new DataSet();
            DataTable dtEmployeeGoals = new DataTable();
            try
            {
                log.Debug("WebFrmUploadEmployeeGoals : LoadExcel()");

                var connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=NO;TypeGuessRows=0;ImportMixedTypes=Text\"";

                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();

                    var sheets = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    using (var cmd = conn.CreateCommand())
                    {

                        cmd.CommandText = "SELECT * FROM [" + sheets.Rows[0]["TABLE_NAME"].ToString() + "] ";
                        //cmd.CommandText = "SELECT * FROM [" + sheets.Rows[sheet]["TABLE_NAME"].ToString() + "] ";

                        DataAdapter adapter = new OleDbDataAdapter(cmd);

                        adapter.Fill(ds);

                        if (ds.Tables[0].Rows.Count > 1)
                        {
                            dtEmployeeGoals.Columns.Add("EMPLOYEE_ID");
                            dtEmployeeGoals.Columns.Add("YEAR_OF_GOAL");
                            dtEmployeeGoals.Columns.Add("GROUP_NAME");
                            dtEmployeeGoals.Columns.Add("GOAL_AREA");
                            dtEmployeeGoals.Columns.Add("DESCRIPTION");
                            dtEmployeeGoals.Columns.Add("MEASUREMENTS");
                            dtEmployeeGoals.Columns.Add("WEIGHT");

                            //Adding Rows to the Data Table
                            for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
                            {
                                DataRow drEmployeeGoal = dtEmployeeGoals.NewRow();

                                drEmployeeGoal["EMPLOYEE_ID"] = ds.Tables[0].Rows[i][0].ToString().Trim();
                                drEmployeeGoal["YEAR_OF_GOAL"] = ds.Tables[0].Rows[i][1].ToString().Trim();
                                drEmployeeGoal["GROUP_NAME"] = ds.Tables[0].Rows[i][2].ToString().Trim();
                                drEmployeeGoal["GOAL_AREA"] = ds.Tables[0].Rows[i][3].ToString().Trim();
                                drEmployeeGoal["DESCRIPTION"] = ds.Tables[0].Rows[i][4].ToString().Trim();
                                drEmployeeGoal["MEASUREMENTS"] = ds.Tables[0].Rows[i][5].ToString().Trim();
                                drEmployeeGoal["WEIGHT"] = ds.Tables[0].Rows[i][6].ToString().Trim();

                                dtEmployeeGoals.Rows.Add(drEmployeeGoal);
                            }

                            //ProcessData(dtEmployeeGoals.Copy());
                            //grdvEmployeeGoals.DataSource = dtEmployeeGoals.Copy();
                            //grdvEmployeeGoals.DataBind();
                        }

                    }
                }

                return dtEmployeeGoals;

            }
            catch (Exception exp)
            {
                log.Error("WebFrmUploadEmployeeGoals : LoadExcel() : " + exp.Message);
                throw exp;
            }
            finally
            {
                ds.Dispose();
                dtEmployeeGoals.Dispose();
            }
        }

        private DataTable ProcessData(DataTable EmployeeGoals)
        {
            DataTable dtEmployeeGoals = new DataTable();
            DataTable dtDistinctEmployees = new DataTable();
            DataTable dtDistinctYears = new DataTable();
            DataTable dtDistinctGoalGroups = new DataTable();
            DataTable dtEmployeeInfo = new DataTable();
            DataTable dtGoalGroupInfo = new DataTable();
            UploadEmployeeGoalsDataHandler UEGDH = new UploadEmployeeGoalsDataHandler();
            try
            {
                Session["ExcelHasErrors"] = "false";

                log.Debug("WebFrmUploadEmployeeGoals : ProcessData()");

                dtEmployeeGoals = EmployeeGoals.Copy();

                //Sort by Employee ID and Year
                DataView dv = dtEmployeeGoals.DefaultView;
                dv.Sort = "EMPLOYEE_ID ASC, YEAR_OF_GOAL ASC";
                dtEmployeeGoals = dv.ToTable().Copy();

                //Mark Invalid Years, Weights
                dtEmployeeGoals.Columns.Add("INVALID_YEAR");
                dtEmployeeGoals.Columns.Add("INVALID_WEIGHT");

                for (int i = 0; i < dtEmployeeGoals.Rows.Count; i++)
                {
                    Int32 Year = 0;
                    double Weight = 0.0;

                    //Check Invalid Year
                    if (Int32.TryParse(dtEmployeeGoals.Rows[i]["YEAR_OF_GOAL"].ToString(), out Year) == false)
                    {
                        dtEmployeeGoals.Rows[i]["INVALID_YEAR"] = dtEmployeeGoals.Rows[i]["YEAR_OF_GOAL"].ToString();
                        Session["ExcelHasErrors"] = "true";
                    }

                    //Check Invalid Weight
                    if (Double.TryParse(dtEmployeeGoals.Rows[i]["WEIGHT"].ToString(), out Weight) == false)
                    {
                        dtEmployeeGoals.Rows[i]["INVALID_WEIGHT"] = dtEmployeeGoals.Rows[i]["WEIGHT"].ToString();
                        Session["ExcelHasErrors"] = "true";
                    }
                    else
                    {
                        if (Weight < 0)
                        {
                            dtEmployeeGoals.Rows[i]["INVALID_WEIGHT"] = dtEmployeeGoals.Rows[i]["WEIGHT"].ToString();
                            Session["ExcelHasErrors"] = "true";
                        }
                        else if (Weight > 100)
                        {
                            dtEmployeeGoals.Rows[i]["INVALID_WEIGHT"] = dtEmployeeGoals.Rows[i]["WEIGHT"].ToString();
                            Session["ExcelHasErrors"] = "true";
                        }
                    }                    
                }                


                //Get Distinct Employee IDs And Get Employee Names
                dv = new DataView(dtEmployeeGoals);
                dtDistinctEmployees = dv.ToTable(true, "EMPLOYEE_ID");

                if (dtDistinctEmployees.Rows.Count > 0)
                {
                    dtEmployeeInfo = UEGDH.PopulateEmployeeDetails(dtDistinctEmployees.Copy());

                    dtEmployeeGoals.Columns.Add("COMP_NAME");
                    dtEmployeeGoals.Columns.Add("EPF_NO");
                    dtEmployeeGoals.Columns.Add("EMP_NAME");

                    for (int i = 0; i < dtEmployeeInfo.Rows.Count; i++)
                    {
                        string EmployeeID = dtEmployeeInfo.Rows[i]["EMPLOYEE_ID"].ToString();
                        string CompanyName = dtEmployeeInfo.Rows[i]["COMP_NAME"].ToString();
                        string EPFNumber = dtEmployeeInfo.Rows[i]["EPF_NO"].ToString();
                        string EmployeeName = dtEmployeeInfo.Rows[i]["EMP_NAME"].ToString();

                        DataRow[] drEmployees = dtEmployeeGoals.Select("EMPLOYEE_ID = '" + EmployeeID + "'");

                        if (drEmployees.Length > 0)
                        {
                            for (int j = 0; j < drEmployees.Length; j++)
                            {
                                drEmployees[j]["COMP_NAME"] = CompanyName;
                                drEmployees[j]["EPF_NO"] = EPFNumber;
                                drEmployees[j]["EMP_NAME"] = EmployeeName;
                            }
                        }

                    }
                }

                //Set Cumulative Weights                
                dtEmployeeGoals.Columns.Add("CUM_WEIGHT");
                dtEmployeeGoals.Columns.Add("INVALID_CUM_WEIGHT");

                dv = new DataView(dtEmployeeGoals);
                dtDistinctYears = dv.ToTable(true, "YEAR_OF_GOAL");

                for (int i = 0; i < dtDistinctEmployees.Rows.Count; i++)
                {
                    for (int j = 0; j < dtDistinctYears.Rows.Count; j++)
                    {
                        DataRow[] drCumWeight = dtEmployeeGoals.Select("EMPLOYEE_ID = '" + dtDistinctEmployees.Rows[i]["EMPLOYEE_ID"].ToString() + "' AND YEAR_OF_GOAL = '" + dtDistinctYears.Rows[j]["YEAR_OF_GOAL"].ToString() + "'");

                        double CumWeight = 0.0;
                        for (int k = 0; k < drCumWeight.Length; k++)
                        {
                            CumWeight += Convert.ToDouble(drCumWeight[k]["WEIGHT"].ToString());
                        }

                        //for (int k = 0; k < drCumWeight.Length; k++)
                        //{
                        //    if ((CumWeight >= 0) && (CumWeight <= 100))
                        //    {
                        //        drCumWeight[k]["CUM_WEIGHT"] = CumWeight.ToString();
                        //    }
                        //    else
                        //    {
                        //        drCumWeight[k]["INVALID_CUM_WEIGHT"] = CumWeight.ToString();
                        //    }
                        //}
                        if (drCumWeight.Length > 0)
                        {
                            if ((CumWeight >= 0) && (CumWeight <= 100))
                            {
                                drCumWeight[(drCumWeight.Length - 1)]["CUM_WEIGHT"] = CumWeight.ToString();
                            }
                            else
                            {
                                drCumWeight[(drCumWeight.Length - 1)]["CUM_WEIGHT"] = CumWeight.ToString();
                                drCumWeight[(drCumWeight.Length - 1)]["INVALID_CUM_WEIGHT"] = CumWeight.ToString();
                                Session["ExcelHasErrors"] = "true";
                            }
                        }
                    }
                }

                //Goal Group Check
                dv = new DataView(dtEmployeeGoals);
                dtDistinctGoalGroups = dv.ToTable(true, "GROUP_NAME");

                dtGoalGroupInfo = UEGDH.PopulateGoalGorupDetails(dtDistinctGoalGroups.Copy()).Copy();
                if (dtGoalGroupInfo.Rows.Count > 0)
                {
                    dtEmployeeGoals.Columns.Add("GOAL_GROUP_ID");
                    dtEmployeeGoals.Columns.Add("STATUS_CODE");

                    for (int i = 0; i < dtGoalGroupInfo.Rows.Count; i++)
                    {
                        string GoalGroupName = dtGoalGroupInfo.Rows[i]["GROUP_NAME"].ToString();
                        string GoalGroupID = dtGoalGroupInfo.Rows[i]["GOAL_GROUP_ID"].ToString();
                        string GoalGroupStaus = dtGoalGroupInfo.Rows[i]["STATUS_CODE"].ToString();

                        DataRow[] drGoalGroups = dtEmployeeGoals.Select("GROUP_NAME = '" + GoalGroupName + "' ");

                        if (drGoalGroups.Length > 0)
                        {
                            for (int k = 0; k < drGoalGroups.Length; k++)
                            {
                                drGoalGroups[k]["GOAL_GROUP_ID"] = GoalGroupID;
                                drGoalGroups[k]["STATUS_CODE"] = GoalGroupStaus;
                            }
                        }
                    }
                }
                return dtEmployeeGoals;
            }
            catch (Exception ex)
            {
                log.Error("WebFrmUploadEmployeeGoals : ProcessData() : " + ex.Message);
                throw ex;
            }
            finally
            {
                dtEmployeeGoals.Dispose();
                dtDistinctEmployees.Dispose();
                dtEmployeeInfo.Dispose();
                dtDistinctYears.Dispose();
                dtDistinctGoalGroups.Dispose();
                dtGoalGroupInfo.Dispose();
                UEGDH = null;
            }
        }

        private DataTable PopulateInvalidCumWeights(DataTable EmployeeGoals)
        {
            try
            {
                DataRow[] drInvalidRows = EmployeeGoals.Select("INVALID_CUM_WEIGHT <> '' OR INVALID_CUM_WEIGHT IS NOT NULL");
                if (drInvalidRows.Length > 0)
                {
                    DataView dv = new DataView(drInvalidRows.CopyToDataTable().Copy());
                    return dv.ToTable(true, "EMPLOYEE_ID", "EMP_NAME", "EPF_NO", "COMP_NAME", "YEAR_OF_GOAL").Copy();
                }
                else
                {
                    DataTable dtEmptyTemplate = new DataTable();
                    dtEmptyTemplate.Columns.Add("EMPLOYEE_ID");
                    dtEmptyTemplate.Columns.Add("EMP_NAME");
                    dtEmptyTemplate.Columns.Add("EPF_NO");
                    dtEmptyTemplate.Columns.Add("COMP_NAME");
                    dtEmptyTemplate.Columns.Add("YEAR_OF_GOAL");
                    return dtEmptyTemplate;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { 
            
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmUploadEmployeeGoals : btnUpload_Click()");

                string FileName = "EmployeeGoals_" + (Session["KeyUSER_ID"] as string);
                string FileExtension = System.IO.Path.GetExtension(fuUploadExcel.FileName);

                Clear();

                if (File.Exists(Server.MapPath("~/PerformanceManagement/DOC/" + FileName + FileExtension)))
                {
                    File.Delete(Server.MapPath("~/PerformanceManagement/DOC/" + FileName + FileExtension));
                }

                fuUploadExcel.SaveAs(Server.MapPath("~/PerformanceManagement/DOC/" + FileName + FileExtension));

                Session["EmployeeGoalFilePath"] = "~/PerformanceManagement/DOC/" + FileName + FileExtension;
                hfPath.Value = "~/PerformanceManagement/DOC/" + FileName + FileExtension;

                lblFileName.Text = FileName + FileExtension;                

                //LoadExcel(Server.MapPath("~/PerformanceManagement/DOC/" + FileName + FileExtension));

            }
            catch (Exception exp)
            {
                log.Error("WebFrmUploadEmployeeGoals : btnUpload_Click() : " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        protected void btnProcessData_Click(object sender, EventArgs e)
        {
            DataTable ExcelDataTable = new DataTable();
            DataTable ProcessedData = new DataTable();
            DataTable InvalidWeights = new DataTable();

            try
            {
                log.Debug("WebFrmUploadEmployeeGoals : btnProcessData_Click()");

                string FilePath = (Session["EmployeeGoalFilePath"] as string);

                if ((FilePath == String.Empty) || (File.Exists(Server.MapPath(FilePath)) == false))
                {
                    CommonVariables.MESSAGE_TEXT = "Excel file does not exists.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                    return;
                }
                else
                {
                    ExcelDataTable = LoadExcel(Server.MapPath(FilePath)).Copy();
                    ProcessedData = ProcessData(ExcelDataTable.Copy()).Copy();
                    InvalidWeights = PopulateInvalidCumWeights(ProcessedData.Copy()).Copy();


                    ////Load Invalid Cumulative weights
                    //GrdvInvalidCumulativeWeights.DataSource = InvalidWeights.Copy();
                    //GrdvInvalidCumulativeWeights.DataBind();

                    //Load Main Grid
                    if (ProcessedData.Rows.Count > 0)
                    {
                        dvMain.Visible = true;
                        grdvEmployeeGoals.DataSource = ProcessedData.Copy();
                        grdvEmployeeGoals.DataBind();

                        Session["ProcessedData"] = ProcessedData.Copy();
                    }
                    else
                    {
                        dvMain.Visible = false;
                    }
                }
                

            }
            catch (Exception exp)
            {
                log.Error("WebFrmUploadEmployeeGoals : btnProcessData_Click() : " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                ExcelDataTable.Dispose();
                ProcessedData.Dispose();
                InvalidWeights.Dispose();
            }
        }

        protected void grdvEmployeeGoals_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    string InvalidYear = HttpUtility.HtmlDecode(e.Row.Cells[10].Text).Trim();
                    if (InvalidYear != String.Empty)
                    {
                        e.Row.BackColor = Color.FromName("#ffeb9c");
                        e.Row.Cells[1].BackColor = Color.Red;
                        e.Row.Cells[1].ToolTip = "Invalid Year";
                    }

                    string InvalidWeight = HttpUtility.HtmlDecode(e.Row.Cells[11].Text).Trim();
                    if (InvalidWeight != String.Empty)
                    {
                        e.Row.BackColor = Color.FromName("#ffeb9c");
                        e.Row.Cells[9].BackColor = Color.Red;
                        e.Row.Cells[9].ToolTip = "Invalid Weight";
                    }

                    string InvalidCumWeight = HttpUtility.HtmlDecode(e.Row.Cells[13].Text).Trim();
                    if (InvalidCumWeight != String.Empty)
                    {
                        //e.Row.BackColor = Color.FromName("#ffeb9c");
                        e.Row.Cells[12].BackColor = Color.Red;
                        e.Row.Cells[12].ToolTip = "Invalid Cumulative Weight";
                    }

                    string GoalGroupID = HttpUtility.HtmlDecode(e.Row.Cells[14].Text).Trim();
                    if (GoalGroupID == String.Empty)
                    {
                        e.Row.BackColor = Color.FromName("#ffeb9c");
                        e.Row.Cells[5].BackColor = Color.Red;
                        e.Row.Cells[5].ToolTip = "Invalid Goal Group";
                    }

                    string GoalGroupStatus = HttpUtility.HtmlDecode(e.Row.Cells[15].Text).Trim();
                    if (GoalGroupStatus == Constants.CON_INACTIVE_STATUS)
                    {
                        e.Row.BackColor = Color.FromName("#ffeb9c");
                        e.Row.Cells[5].BackColor = Color.Red;
                        e.Row.Cells[5].ToolTip = "Inactive Goal Group";
                    }

                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UploadEmployeeGoalsDataHandler UEGDH = new UploadEmployeeGoalsDataHandler();
            DataTable dtProcessedData = new DataTable();
            DataTable[] dtResponse = new DataTable[2];
            try
            {
                log.Debug("WebFrmUploadEmployeeGoals : btnSave_Click()");
                dtProcessedData = (Session["ProcessedData"] as DataTable).Copy();
                string AddedBy = (Session["KeyUSER_ID"] as string);
                string Erros = (Session["ExcelHasErrors"] as string);

                if (grdvEmployeeGoals.Rows.Count == 0)
                {
                    CommonVariables.MESSAGE_TEXT = "Please process the excel before saving";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                    return;
                }

                if (Erros == "false")
                {
                    dtResponse = UEGDH.Insert(dtProcessedData.Copy(), AddedBy);

                    Clear();

                    DataView dv = new DataView(dtResponse[0].Copy());
                    dtResponse[0] = dv.ToTable(true, "YEAR_OF_GOAL", "EMPLOYEE_ID", "EPF_NO", "EMP_NAME", "COMP_NAME").Copy();

                    dv = new DataView(dtResponse[1].Copy());
                    dtResponse[1] = dv.ToTable(true, "YEAR_OF_GOAL", "EMPLOYEE_ID", "EPF_NO", "EMP_NAME", "COMP_NAME", "FAILED_REASON").Copy();

                    dvSuccess.Visible = true;
                    grdvSuccess.DataSource = dtResponse[0].Copy();
                    grdvSuccess.DataBind();


                    dvError.Visible = true;
                    GrdvErros.DataSource = dtResponse[1].Copy();
                    GrdvErros.DataBind();

                    CommonVariables.MESSAGE_TEXT = grdvSuccess.Rows.Count + " " + CommonVariables.MESSAGE_STRING_SUCCESS_SAVED;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Please correct the excel sheet";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                    return;
                }
            }
            catch (Exception exp)
            {
                log.Error("WebFrmUploadEmployeeGoals : btnSave_Click() : " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                dtResponse = null;
                dtProcessedData.Dispose();
                UEGDH = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmUploadEmployeeGoals : btnClear_Click()");
                Clear();
            }
            catch (Exception exp)
            {
                log.Error("WebFrmUploadEmployeeGoals : btnClear_Click() : " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        void Clear()
        {
            try
            {
                log.Debug("WebFrmUploadEmployeeGoals : Clear()");

                if (File.Exists(Server.MapPath(hfPath.Value)))
                {
                    File.Delete(Server.MapPath(hfPath.Value));
                }
                lblFileName.Text = String.Empty;
                Utility.Errorhandler.ClearError(lblStatus);
                hfPath.Value = String.Empty;
                grdvEmployeeGoals.DataSource = null;
                grdvEmployeeGoals.DataBind();
                dvSuccess.Visible = false;
                dvError.Visible = false;
                dvMain.Visible = false;
            }
            catch (Exception ex)
            {
                log.Error("WebFrmUploadEmployeeGoals : Clear() : " + ex.Message);
                throw ex;
            }
            finally
            { 
            
            }
        }
    }
}