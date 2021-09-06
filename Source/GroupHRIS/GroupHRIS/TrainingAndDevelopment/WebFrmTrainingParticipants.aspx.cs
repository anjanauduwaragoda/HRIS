using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using Common;
using DataHandler.TrainingAndDevelopment;
using System.Data;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmTrainingParticipants : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        void loadTrainingDetails()
        {
            TrainingParticipantsDataHandler TPDH = new TrainingParticipantsDataHandler();
            DataTable dtTrainingDetails = new DataTable();

            string TrainingID = String.Empty;
            string TrainingName = String.Empty;
            string TrainingCode = String.Empty;
            string ProgramName = String.Empty;
            string TrainingType = String.Empty;
            string PlannedParticipants = String.Empty;
            string PlannedStartDate = String.Empty;
            string PlannedEndDate = String.Empty;
            string PlannedTotalHours = String.Empty;
            string StatusCode = String.Empty;

            try
            {
                log.Debug("WebFrmTrainingParticipants | loadTrainingDetails()");

                TrainingID = txtTrainingID.Text.Trim();

                dtTrainingDetails = TPDH.PopulateTrainings(TrainingID).Copy();
                if (dtTrainingDetails.Rows.Count > 0)
                {
                    TrainingName = dtTrainingDetails.Rows[0]["TRAINING_NAME"].ToString().Trim();
                    TrainingCode = dtTrainingDetails.Rows[0]["TRAINING_CODE"].ToString().Trim();
                    ProgramName = dtTrainingDetails.Rows[0]["PROGRAM_NAME"].ToString().Trim();
                    TrainingType = dtTrainingDetails.Rows[0]["TRAINING_TYPE"].ToString().Trim();
                    PlannedParticipants = dtTrainingDetails.Rows[0]["PLANNED_PARTICIPANTS"].ToString().Trim();
                    PlannedStartDate = dtTrainingDetails.Rows[0]["PLANNED_START_DATE"].ToString().Trim();
                    PlannedEndDate = dtTrainingDetails.Rows[0]["PLANNED_END_DATE"].ToString().Trim();
                    PlannedTotalHours = dtTrainingDetails.Rows[0]["PLANNED_TOTAL_HOURS"].ToString().Trim();
                    StatusCode = dtTrainingDetails.Rows[0]["STATUS_CODE"].ToString().Trim();

                    lblTrainingName.Text = TrainingName;
                    lblTrainingCode.Text = TrainingCode;
                    lblProgramName.Text = ProgramName;
                    lblTrainingType.Text = TrainingType;
                    lblPlannedParticipants.Text = PlannedParticipants;
                    lblPlannedStartDate.Text = PlannedStartDate;
                    lblPlannedEndDate.Text = PlannedEndDate;
                    lblPlannedTotalHours.Text = PlannedTotalHours;
                    lblTrainingStatus.Text = StatusCode;
                }

            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingParticipants | loadTrainingDetails() | " + ex.Message);
                throw ex;
            }
            finally
            {
                TrainingID = String.Empty;
                TrainingName = String.Empty;
                TrainingCode = String.Empty;
                ProgramName = String.Empty;
                TrainingType = String.Empty;
                PlannedParticipants = String.Empty;
                PlannedStartDate = String.Empty;
                PlannedEndDate = String.Empty;
                PlannedTotalHours = String.Empty;
                StatusCode = String.Empty;
                dtTrainingDetails.Dispose();
                TPDH = null;
            }
        }

        void createTrainingCompanyTable()
        {
            DataTable dtTrainingCompanies = new DataTable();
            try
            {
                log.Debug("WebFrmTrainingParticipants | createTrainingCompanyTable()");

                dtTrainingCompanies.Columns.Add("COMPANY_ID");
                dtTrainingCompanies.Columns.Add("COMP_NAME");
                dtTrainingCompanies.Columns.Add("PLANNED_PARTICIPANTS");
                dtTrainingCompanies.Columns.Add("SELECTED_PARTICIPANTS");

                Session["dtTrainingCompanies"] = dtTrainingCompanies.Copy();
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingParticipants | createTrainingCompanyTable() | " + ex.Message);
                throw ex;
            }
            finally
            {
                dtTrainingCompanies.Dispose();
            }
        }

        void createSelectedEmployeesTable()
        {
            DataTable dtSelectedEmployees = new DataTable();
            try
            {
                log.Debug("WebFrmTrainingParticipants | createSelectedEmployeesTable()");

                dtSelectedEmployees.Columns.Add("COMPANY_ID");
                dtSelectedEmployees.Columns.Add("EMPLOYEE_ID");
                dtSelectedEmployees.Columns.Add("EMPLOYEE_NAME");
                dtSelectedEmployees.Columns.Add("DESIGNATION_NAME");

                Session["dtSelectedEmployees"] = dtSelectedEmployees.Copy();
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingParticipants | createSelectedEmployeesTable() | " + ex.Message);
                throw ex;
            }
            finally
            {
                dtSelectedEmployees.Dispose();
            }
        }

        void LoadTrainingCompany()
        {
            TrainingParticipantsDataHandler TPDH = new TrainingParticipantsDataHandler();

            DataTable dtTrainingCompanies = new DataTable();
            DataTable dtTrainingCompanyTable = new DataTable();

            string TrainingID = String.Empty;
            try
            {
                log.Debug("WebFrmTrainingParticipants | LoadTrainingCompany()");
                TrainingID = txtTrainingID.Text.Trim();

                dtTrainingCompanyTable = (Session["dtTrainingCompanies"] as DataTable).Copy();
                dtTrainingCompanies = TPDH.PopulateTrainingCompanies(TrainingID).Copy();

                for (int i = 0; i < dtTrainingCompanies.Rows.Count; i++)
                {
                    DataRow dr = dtTrainingCompanyTable.NewRow();
                    dr["COMPANY_ID"] = dtTrainingCompanies.Rows[i]["COMPANY_ID"].ToString();
                    dr["COMP_NAME"] = dtTrainingCompanies.Rows[i]["COMP_NAME"].ToString();
                    dr["PLANNED_PARTICIPANTS"] = dtTrainingCompanies.Rows[i]["PLANNED_PARTICIPANTS"].ToString();
                    dtTrainingCompanyTable.Rows.Add(dr);
                }

                Session["dtTrainingCompanies"] = dtTrainingCompanyTable.Copy();

                grdvCompanyDetails.DataSource = dtTrainingCompanyTable.Copy();
                grdvCompanyDetails.DataBind();
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingParticipants | LoadTrainingCompany() | " + ex.Message);
                throw ex;
            }
            finally
            {
                TrainingID = String.Empty;
                dtTrainingCompanies.Dispose();
                dtTrainingCompanyTable.Dispose();
                TPDH = null;
            }
        }

        void LoadCompanies()
        {
            TrainingParticipantsDataHandler TPDH = new TrainingParticipantsDataHandler();

            DataTable dtCompanies = new DataTable();
            try
            {
                log.Debug("WebFrmTrainingParticipants | LoadCompanies()");

                dtCompanies = TPDH.PopulateTrainingCompanies(txtTrainingID.Text.Trim()).Copy();

                ddlCompanySearch.Items.Clear();
                ddlCompanySearch.Items.Add(new ListItem("", ""));

                for (int i = 0; i < dtCompanies.Rows.Count; i++)
                {
                    string Text = dtCompanies.Rows[i]["COMP_NAME"].ToString();
                    string Value = dtCompanies.Rows[i]["COMPANY_ID"].ToString();

                    ddlCompanySearch.Items.Add(new ListItem(Text, Value));
                }
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingParticipants | LoadCompanies() | " + ex.Message);
                throw ex;
            }
            finally
            {
                dtCompanies.Dispose();
                TPDH = null;
            }
        }

        void LoadDepartments()
        {
            TrainingParticipantsDataHandler TPDH = new TrainingParticipantsDataHandler();
            DataTable dtDepartments = new DataTable();
            try
            {
                log.Debug("WebFrmTrainingParticipants | LoadDepartments()");
                string CompanyID = ddlCompanySearch.SelectedValue.ToString();
                dtDepartments = TPDH.PopulateDepartments(CompanyID).Copy();

                ddlDepartmentSearch.Items.Clear();
                ddlDepartmentSearch.Items.Add(new ListItem("", ""));

                for (int i = 0; i < dtDepartments.Rows.Count; i++)
                {
                    string Text = dtDepartments.Rows[i]["DEPT_NAME"].ToString();
                    string Value = dtDepartments.Rows[i]["DEPT_ID"].ToString();

                    ddlDepartmentSearch.Items.Add(new ListItem(Text, Value));
                }
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingParticipants | LoadDepartments() | " + ex.Message);
                throw ex;
            }
            finally
            {
                dtDepartments.Dispose();
                TPDH = null;
            }
        }

        void LoadDivisions()
        {
            TrainingParticipantsDataHandler TPDH = new TrainingParticipantsDataHandler();
            DataTable dtDivisions = new DataTable();
            try
            {
                log.Debug("WebFrmTrainingParticipants | LoadDivisions()");
                string DepartmentID = ddlDepartmentSearch.SelectedValue.ToString();
                dtDivisions = TPDH.PopulateDivisions(DepartmentID).Copy();

                ddlDivisionSearch.Items.Clear();
                ddlDivisionSearch.Items.Add(new ListItem("", ""));

                for (int i = 0; i < dtDivisions.Rows.Count; i++)
                {
                    string Text = dtDivisions.Rows[i]["DIV_NAME"].ToString();
                    string Value = dtDivisions.Rows[i]["DIVISION_ID"].ToString();

                    ddlDivisionSearch.Items.Add(new ListItem(Text, Value));
                }
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingParticipants | LoadDivisions() | " + ex.Message);
                throw ex;
            }
            finally
            {
                dtDivisions.Dispose();
                TPDH = null;
            }
        }

        void LoadBranches()
        {
            TrainingParticipantsDataHandler TPDH = new TrainingParticipantsDataHandler();
            DataTable dtBranches = new DataTable();
            try
            {
                log.Debug("WebFrmTrainingParticipants | LoadDepartments()");
                string CompanyID = ddlCompanySearch.SelectedValue.ToString();
                dtBranches = TPDH.PopulateBranches(CompanyID).Copy();

                ddlBranchSearch.Items.Clear();
                ddlBranchSearch.Items.Add(new ListItem("", ""));

                for (int i = 0; i < dtBranches.Rows.Count; i++)
                {
                    string Text = dtBranches.Rows[i]["BRANCH_NAME"].ToString();
                    string Value = dtBranches.Rows[i]["BRANCH_ID"].ToString();

                    ddlBranchSearch.Items.Add(new ListItem(Text, Value));
                }
            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingParticipants | LoadDepartments() | " + ex.Message);
                throw ex;
            }
            finally
            {
                dtBranches.Dispose();
                TPDH = null;
            }
        }

        void LoadSelectedEmployeeGrid()
        {
            TrainingParticipantsDataHandler TPDH = new TrainingParticipantsDataHandler();
            DataTable dtSelectedEmployees = new DataTable();
            try
            {
                log.Debug("WebFrmTrainingParticipants | LoadSelectedEmployeeGrid()");

                string Company = ddlCompanySearch.SelectedValue.ToString();
                string Department = ddlDepartmentSearch.SelectedValue.ToString();
                string Division = ddlDivisionSearch.SelectedValue.ToString();
                string Branch = ddlBranchSearch.SelectedValue.ToString();

                dtSelectedEmployees = TPDH.PopulateEmployees(Company, Department, Division, Branch).Copy();

                grdvSelectedEmployees.DataSource = dtSelectedEmployees.Copy();
                grdvSelectedEmployees.DataBind();

            }
            catch (Exception ex)
            {
                log.Error("WebFrmTrainingParticipants | LoadSelectedEmployeeGrid() | " + ex.Message);
                throw ex;
            }
            finally
            {
                dtSelectedEmployees.Dispose();
                TPDH = null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (IsPostBack)
                {
                    sIPAddress = Request.UserHostAddress;
                    log.Debug("IP:" + sIPAddress + "WebFrmTrainingParticipants : Page_Load");

                    Utility.Errorhandler.ClearError(lblMsg);


                    if (hfCaller.Value == "txtTrainingID")
                    {
                        hfCaller.Value = "";
                        if (hfVal.Value != "")
                        {
                            txtTrainingID.Text = hfVal.Value;
                        }
                        if (txtTrainingID.Text != "")
                        {
                            hfVal.Value = String.Empty;
                            //Postback Methods
                            loadTrainingDetails();
                            LoadTrainingCompany();
                            LoadCompanies();
                            LoadTrainingParticipants();
                        }
                    }

                    //if (hfCaller.Value == "txtEmployeeID")
                    //{
                    //    hfCaller.Value = "";
                    //    if (hfVal.Value != "")
                    //    {
                    //        txtEmployeeID.Text = hfVal.Value;
                    //    }
                    //    if (txtEmployeeID.Text != "")
                    //    {
                    //        hfVal.Value = String.Empty;
                    //        //Postback Methods
                    //    }
                    //}
                }
                else
                {
                    createTrainingCompanyTable();
                    createSelectedEmployeesTable();
                    //LoadCompanies();
                }
            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainingParticipants | Page_Load | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMsg);
            }
            finally
            {

            }
        }

        protected void ddlCompanySearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainingParticipants | ddlCompanySearch_SelectedIndexChanged()");

                Utility.Errorhandler.ClearError(lblMsg);

                LoadDepartments();
                ddlDivisionSearch.Items.Clear();
                LoadBranches();
            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainingParticipants | ddlCompanySearch_SelectedIndexChanged | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMsg);
            }
            finally
            {

            }
        }

        protected void ddlDepartmentSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainingParticipants | ddlDepartmentSearch_SelectedIndexChanged()");

                Utility.Errorhandler.ClearError(lblMsg);

                LoadDivisions();
            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainingParticipants | ddlDepartmentSearch_SelectedIndexChanged | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMsg);
            }
            finally
            {

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainingParticipants | btnSearch_Click()");

                Utility.Errorhandler.ClearError(lblMsg);

                if (txtTrainingID.Text.Trim() != String.Empty)
                {
                    if (ddlCompanySearch.SelectedIndex == 0)
                    {
                        CommonVariables.MESSAGE_TEXT = "Please select a company to search";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMsg);
                        return;
                    }
                    LoadSelectedEmployeeGrid();
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Please select a training";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMsg);
                }
            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainingParticipants | btnSearch_Click | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMsg);
            }
            finally
            {

            }
        }

        protected void chkEmployeeIncludeAll_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dtSelectedEmployees = new DataTable();
            DataTable dtTrainingCompanies = new DataTable();
            try
            {
                log.Debug("WebFrmTrainingParticipants | btnSearch_Click()");
                Utility.Errorhandler.ClearError(lblMsg);

                CheckBox chkEmployeeIncludeAll = (grdvSelectedEmployees.HeaderRow.FindControl("chkEmployeeIncludeAll") as CheckBox);


                for (int i = 0; i < grdvSelectedEmployees.Rows.Count; i++)
                {
                    CheckBox chkEmployeeInclude = (grdvSelectedEmployees.Rows[i].FindControl("chkEmployeeInclude") as CheckBox);

                    if (chkEmployeeIncludeAll.Checked == true)
                    {
                        chkEmployeeInclude.Checked = true;
                    }
                    else
                    {
                        chkEmployeeInclude.Checked = false;
                    }
                }


                dtSelectedEmployees = (Session["dtSelectedEmployees"] as DataTable).Copy();
                dtTrainingCompanies = (Session["dtTrainingCompanies"] as DataTable).Copy();

                for (int i = 0; i < grdvSelectedEmployees.Rows.Count; i++)
                {
                    CheckBox chkEmployeeInclude = (grdvSelectedEmployees.Rows[i].FindControl("chkEmployeeInclude") as CheckBox);

                    if (chkEmployeeInclude.Checked == true)
                    {
                        string EmployeeID = HttpUtility.HtmlDecode(grdvSelectedEmployees.Rows[i].Cells[1].Text).Trim();
                        DataRow[] drArr = dtSelectedEmployees.Select("EMPLOYEE_ID = '" + EmployeeID + "'");

                        if (drArr.Length == 0)
                        {
                            string CompanyID = HttpUtility.HtmlDecode(grdvSelectedEmployees.Rows[i].Cells[0].Text).Trim();
                            string EmployeeName = HttpUtility.HtmlDecode(grdvSelectedEmployees.Rows[i].Cells[2].Text).Trim();
                            string Designation = HttpUtility.HtmlDecode(grdvSelectedEmployees.Rows[i].Cells[3].Text).Trim();

                            DataRow dr = dtSelectedEmployees.NewRow();

                            dr["COMPANY_ID"] = CompanyID;
                            dr["EMPLOYEE_ID"] = EmployeeID;
                            dr["EMPLOYEE_NAME"] = EmployeeName;
                            dr["DESIGNATION_NAME"] = Designation;

                            dtSelectedEmployees.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        string EmployeeID = HttpUtility.HtmlDecode(grdvSelectedEmployees.Rows[i].Cells[1].Text).Trim();
                        DataRow[] drArr = dtSelectedEmployees.Select("EMPLOYEE_ID = '" + EmployeeID + "'");
                        foreach (DataRow dr in drArr)
                        {
                            dr.Delete();
                        }
                    }
                }

                Session["dtSelectedEmployees"] = dtSelectedEmployees.Copy();

                //Update dtTrainingCompanies DataTable

                for (int i = 0; i < dtTrainingCompanies.Rows.Count; i++)
                {
                    string CompanyID = dtTrainingCompanies.Rows[i]["COMPANY_ID"].ToString();

                    DataRow[] drArrCompCount = dtSelectedEmployees.Select("COMPANY_ID = '" + CompanyID + "'");

                    string EmployeeCount = drArrCompCount.Length.ToString();

                    dtTrainingCompanies.Rows[i]["SELECTED_PARTICIPANTS"] = EmployeeCount;
                }

                Session["dtTrainingCompanies"] = dtTrainingCompanies.Copy();

                grdvCompanyDetails.DataSource = dtTrainingCompanies.Copy();
                grdvCompanyDetails.DataBind();


            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainingParticipants | btnSearch_Click | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMsg);
            }
            finally
            {
                dtSelectedEmployees.Dispose();
                dtTrainingCompanies.Dispose();
            }
        }

        protected void chkEmployeeInclude_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dtSelectedEmployees = new DataTable();
            DataTable dtTrainingCompanies = new DataTable();
            try
            {
                log.Debug("WebFrmTrainingParticipants | btnSearch_Click()");

                Utility.Errorhandler.ClearError(lblMsg);

                dtSelectedEmployees = (Session["dtSelectedEmployees"] as DataTable).Copy();
                dtTrainingCompanies = (Session["dtTrainingCompanies"] as DataTable).Copy();

                for (int i = 0; i < grdvSelectedEmployees.Rows.Count; i++)
                {
                    CheckBox chkEmployeeInclude = (grdvSelectedEmployees.Rows[i].FindControl("chkEmployeeInclude") as CheckBox);

                    if (chkEmployeeInclude.Checked == true)
                    {
                        string EmployeeID = HttpUtility.HtmlDecode(grdvSelectedEmployees.Rows[i].Cells[1].Text).Trim();
                        DataRow[] drArr = dtSelectedEmployees.Select("EMPLOYEE_ID = '" + EmployeeID + "'");

                        if (drArr.Length == 0)
                        {
                            string CompanyID = HttpUtility.HtmlDecode(grdvSelectedEmployees.Rows[i].Cells[0].Text).Trim();
                            string EmployeeName = HttpUtility.HtmlDecode(grdvSelectedEmployees.Rows[i].Cells[2].Text).Trim();
                            string Designation = HttpUtility.HtmlDecode(grdvSelectedEmployees.Rows[i].Cells[3].Text).Trim();

                            DataRow dr = dtSelectedEmployees.NewRow();

                            dr["COMPANY_ID"] = CompanyID;
                            dr["EMPLOYEE_ID"] = EmployeeID;
                            dr["EMPLOYEE_NAME"] = EmployeeName;
                            dr["DESIGNATION_NAME"] = Designation;

                            dtSelectedEmployees.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        string EmployeeID = HttpUtility.HtmlDecode(grdvSelectedEmployees.Rows[i].Cells[1].Text).Trim();
                        DataRow[] drArr = dtSelectedEmployees.Select("EMPLOYEE_ID = '" + EmployeeID + "'");
                        foreach (DataRow dr in drArr)
                        {
                            dr.Delete();
                        }
                    }
                }

                Session["dtSelectedEmployees"] = dtSelectedEmployees.Copy();

                //Update dtTrainingCompanies DataTable

                for (int i = 0; i < dtTrainingCompanies.Rows.Count; i++)
                {
                    string CompanyID = dtTrainingCompanies.Rows[i]["COMPANY_ID"].ToString();

                    DataRow[] drArrCompCount = dtSelectedEmployees.Select("COMPANY_ID = '" + CompanyID + "'");

                    string EmployeeCount = drArrCompCount.Length.ToString();

                    dtTrainingCompanies.Rows[i]["SELECTED_PARTICIPANTS"] = EmployeeCount;
                }

                Session["dtTrainingCompanies"] = dtTrainingCompanies.Copy();

                grdvCompanyDetails.DataSource = dtTrainingCompanies.Copy();
                grdvCompanyDetails.DataBind();
            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainingParticipants | btnSearch_Click | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMsg);
            }
            finally
            {
                dtSelectedEmployees.Dispose();
                dtTrainingCompanies.Dispose();
            }
        }

        protected void grdvSelectedEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainingParticipants | grdvSelectedEmployees_PageIndexChanging()");

                Utility.Errorhandler.ClearError(lblMsg);

                grdvSelectedEmployees.PageIndex = e.NewPageIndex;
                LoadSelectedEmployeeGrid();

                bool AllChecked = true;

                for (int i = 0; i < grdvSelectedEmployees.Rows.Count; i++)
                {
                    CheckBox chkEmployeeInclude = (grdvSelectedEmployees.Rows[i].FindControl("chkEmployeeInclude") as CheckBox);
                    if (chkEmployeeInclude.Checked == false)
                    {
                        AllChecked = false;
                    }
                }

                if (AllChecked)
                {
                    CheckBox chkEmployeeIncludeAll = (grdvSelectedEmployees.HeaderRow.FindControl("chkEmployeeIncludeAll") as CheckBox);
                    chkEmployeeIncludeAll.Checked = true;
                }
                else
                {
                    CheckBox chkEmployeeIncludeAll = (grdvSelectedEmployees.HeaderRow.FindControl("chkEmployeeIncludeAll") as CheckBox);
                    chkEmployeeIncludeAll.Checked = false;
                }

            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainingParticipants | grdvSelectedEmployees_PageIndexChanging | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMsg);
            }
            finally
            {

            }
        }

        protected void grdvSelectedEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
        {            
            try
            {  
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string EmployeeID = HttpUtility.HtmlDecode(e.Row.Cells[1].Text).Trim();
                    DataRow[] drArr = (Session["dtSelectedEmployees"] as DataTable).Select("EMPLOYEE_ID = '" + EmployeeID + "'");
                    if (drArr.Length > 0)
                    {
                        CheckBox chkEmployeeInclude = (e.Row.FindControl("chkEmployeeInclude") as CheckBox);
                        chkEmployeeInclude.Checked = true;
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
            TrainingParticipantsDataHandler TPDH = new TrainingParticipantsDataHandler();
            DataTable dtSelectedEmployees = new DataTable();
            try
            {
                log.Debug("WebFrmTrainingParticipants | btnSave_Click()");

                Utility.Errorhandler.ClearError(lblMsg);

                string TrainingID = txtTrainingID.Text.Trim();
                string AddedBy = (Session["KeyUSER_ID"] as string).Trim();
                dtSelectedEmployees = (Session["dtSelectedEmployees"] as DataTable).Copy();
                string[] Employees = new string[dtSelectedEmployees.Rows.Count];

                for (int i = 0; i < dtSelectedEmployees.Rows.Count; i++)
                {
                    Employees[i] = dtSelectedEmployees.Rows[i]["EMPLOYEE_ID"].ToString();
                }

                TPDH.Insert(TrainingID, Employees, AddedBy);
                Clear();

                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_SAVED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMsg);

            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainingParticipants | btnSave_Click | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMsg);
            }
            finally
            {
                dtSelectedEmployees.Dispose();
                TPDH = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainingParticipants | btnClear_Click()");
                Clear();
            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainingParticipants | btnClear_Click | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMsg);
            }
            finally
            {

            }
        }

        void Clear()
        {
            try
            {
                createSelectedEmployeesTable();
                createTrainingCompanyTable();
                Utility.Utils.clearControls(true, txtTrainingID, lblTrainingName, lblTrainingCode, lblProgramName, lblTrainingType, lblPlannedParticipants, lblPlannedStartDate, lblPlannedEndDate, lblPlannedTotalHours, lblTrainingStatus);
                ddlCompanySearch.Items.Clear();
                ddlDepartmentSearch.Items.Clear();
                ddlDivisionSearch.Items.Clear();
                ddlBranchSearch.Items.Clear();
                Utility.Errorhandler.ClearError(lblMsg);

                grdvCompanyDetails.DataSource = null;
                grdvCompanyDetails.DataBind();

                grdvSelectedEmployees.DataSource = null;
                grdvSelectedEmployees.DataBind();

                grdvTrainingParticipants.DataSource = null;
                grdvTrainingParticipants.DataBind();

            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        void LoadTrainingParticipants()
        {
            TrainingParticipantsDataHandler TPDH = new TrainingParticipantsDataHandler();
            DataTable dtParticipants = new DataTable();
            DataTable dtSelectedEmployees = new DataTable();
            DataTable dtTrainingCompanies = new DataTable();
            try
            {
                log.Debug("WebFrmTrainingParticipants | LoadTrainingParticipants()");

                dtSelectedEmployees = (Session["dtSelectedEmployees"] as DataTable).Copy();
                dtTrainingCompanies = (Session["dtTrainingCompanies"] as DataTable).Copy();

                dtParticipants = TPDH.PopulateTrainingParticipants(txtTrainingID.Text.Trim()).Copy();
                grdvTrainingParticipants.DataSource = dtParticipants.Copy();
                grdvTrainingParticipants.DataBind();

                for (int i = 0; i < dtParticipants.Rows.Count; i++)
                {
                    DataRow dr = dtSelectedEmployees.NewRow();

                    dr["COMPANY_ID"]=dtParticipants.Rows[i]["COMPANY_ID"].ToString();
                    dr["EMPLOYEE_ID"] = dtParticipants.Rows[i]["EMPLOYEE_ID"].ToString();
                    dr["EMPLOYEE_NAME"] = dtParticipants.Rows[i]["EMP_NAME"].ToString();
                    dr["DESIGNATION_NAME"] = dtParticipants.Rows[i]["DESIGNATION_NAME"].ToString();

                    if ((dtSelectedEmployees.Select("EMPLOYEE_ID = '" + dtParticipants.Rows[i]["EMPLOYEE_ID"].ToString() + "'") as DataRow[]).Length == 0)
                    {
                        dtSelectedEmployees.Rows.Add(dr);
                    }
                }
                Session["dtSelectedEmployees"] = dtSelectedEmployees.Copy();
                Session["dtParticipants"] = dtParticipants.Copy();


                //Update dtTrainingCompanies DataTable

                for (int i = 0; i < dtTrainingCompanies.Rows.Count; i++)
                {
                    string CompanyID = dtTrainingCompanies.Rows[i]["COMPANY_ID"].ToString();

                    DataRow[] drArrCompCount = dtSelectedEmployees.Select("COMPANY_ID = '" + CompanyID + "'");

                    string EmployeeCount = drArrCompCount.Length.ToString();

                    dtTrainingCompanies.Rows[i]["SELECTED_PARTICIPANTS"] = EmployeeCount;
                }

                Session["dtTrainingCompanies"] = dtTrainingCompanies.Copy();

                grdvCompanyDetails.DataSource = dtTrainingCompanies.Copy();
                grdvCompanyDetails.DataBind();

            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainingParticipants | LoadTrainingParticipants | " + exp.Message);
                throw exp;
            }
            finally
            {
                TPDH = null;
                dtParticipants.Dispose();
            }
        }

        protected void grdvTrainingParticipants_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                log.Debug("WebFrmTrainingParticipants | grdvTrainingParticipants_PageIndexChanging()");

                Utility.Errorhandler.ClearError(lblMsg);

                grdvTrainingParticipants.PageIndex = e.NewPageIndex;
                LoadTrainingParticipants();



            }
            catch (Exception exp)
            {
                log.Error("WebFrmTrainingParticipants | grdvTrainingParticipants_PageIndexChanging | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMsg);
            }
            finally
            {
                
            }
        }
    }
}