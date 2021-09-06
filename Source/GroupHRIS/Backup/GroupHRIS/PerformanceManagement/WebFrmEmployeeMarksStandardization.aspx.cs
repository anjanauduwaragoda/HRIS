using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using Common;
using DataHandler.PerformanceManagement;
using System.Data;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmEmployeeMarksStandardization : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        #region Methods

        void SetFinancialYear()
        {
            try
            {
                System.DateTime dtfin = System.DateTime.Now;

                int CurrentFinyear = 0;

                DateTime finDate = DateTime.ParseExact(dtfin.Year + "-04-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                if (finDate > System.DateTime.Now)
                {
                    CurrentFinyear = dtfin.AddYears(-1).Year;
                    hfFinancialYearDetails.Value = " (From April 1, " + CurrentFinyear.ToString() + " To March 31, " + finDate.Year + ")";
                    hfFinancialYear.Value = CurrentFinyear.ToString();
                }
                else
                {
                    System.DateTime dt = System.DateTime.Now;
                    System.DateTime dtDetais = System.DateTime.Now;
                    hfFinancialYearDetails.Value = " (From April 1, " + dt.Year.ToString() + " To March 31, " + dtDetais.AddYears(1).Year + ")";
                    hfFinancialYear.Value = dt.Year.ToString();
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        void LoadAssessmentYears()
        {
            CEODashboadDataHandler CEODDH = new CEODashboadDataHandler();
            DataTable dtYearOfAssessment = new DataTable();
            try
            {
                dtYearOfAssessment = CEODDH.PopulateYears().Copy();
                for (int i = 0; i < dtYearOfAssessment.Rows.Count; i++)
                {
                    string Year = dtYearOfAssessment.Rows[i]["YEAR_OF_ASSESSMENT"].ToString();
                    ddlYearOfAssessment.Items.Add(new ListItem(Year, Year));
                }

                string CurrentYear = hfFinancialYear.Value;
                DataRow[] drCurrentYear = dtYearOfAssessment.Select("YEAR_OF_ASSESSMENT = '" + CurrentYear + "'");
                if (drCurrentYear.Length > 0)
                {
                    ddlYearOfAssessment.SelectedIndex = ddlYearOfAssessment.Items.IndexOf(ddlYearOfAssessment.Items.FindByValue(CurrentYear));
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                CEODDH = null;
                dtYearOfAssessment.Dispose();
            }
        }

        void LoadCompanies()
        {
            EmployeemarksStandardizationDataHandler ESDH = new EmployeemarksStandardizationDataHandler();
            DataTable dtCompanies = new DataTable();
            try
            {
                dtCompanies = ESDH.PopulateCompanies();

                ddlCompany.Items.Clear();
                ddlDepartment.Items.Clear();
                ddlDivision.Items.Clear();
                ddlAssessment.Items.Clear();

                ddlCompany.Items.Add(new ListItem("", ""));

                for (int i = 0; i < dtCompanies.Rows.Count; i++)
                {
                    string Text = dtCompanies.Rows[i]["COMP_NAME"].ToString();
                    string Value = dtCompanies.Rows[i]["COMPANY_ID"].ToString();

                    ddlCompany.Items.Add(new ListItem(Text, Value));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ESDH = null;
                dtCompanies.Dispose();
            }
        }

        void LoadStatus(DropDownList StatusList)
        {
            try
            {
                StatusList.Items.Clear();
                StatusList.Items.Add(new ListItem("", ""));
                StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_ACTIVE_TAG, Constants.ASSESSNEMT_ACTIVE_STATUS));
                StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_OBSOLETE_TAG, Constants.ASSESSNEMT_OBSOLETE_STATUS));
                StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_PENDING_TAG, Constants.ASSESSNEMT_PENDING_STATUS));
                //StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG, Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS));
                //StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG, Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS));
                //StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG, Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS));
                //StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_CEO_FINALIZED_TAG, Constants.ASSESSNEMT_CEO_FINALIZED_STATUS));
                StatusList.Items.Add(new ListItem(Constants.ASSESSNEMT_CLOSED_TAG, Constants.ASSESSNEMT_CLOSED_STATUS));
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
        }

        void LoadDepartments(DropDownList DepartmentList)
        {
            CEODashboadDataHandler CEODDH = new CEODashboadDataHandler();
            DataTable dtAssessments = new DataTable();
            try
            {
                DepartmentList.Items.Clear();
                ddlDivision.Items.Clear();
                ddlAssessment.Items.Clear();

                dtAssessments = CEODDH.PopulateDepartments(ddlCompany.SelectedValue).Copy();
                DepartmentList.Items.Add(new ListItem("", ""));
                for (int i = 0; i < dtAssessments.Rows.Count; i++)
                {
                    string Text = dtAssessments.Rows[i]["DEPT_NAME"].ToString();
                    string Value = dtAssessments.Rows[i]["DEPT_ID"].ToString();
                    DepartmentList.Items.Add(new ListItem(Text, Value));
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                CEODDH = null;
                dtAssessments.Dispose();
            }
        }

        void LoadDivisions()
        {
            EmployeemarksStandardizationDataHandler ESDH = new EmployeemarksStandardizationDataHandler();
            DataTable dtDivisions = new DataTable();
            try
            {
                if ((ddlDepartment.Items.Count > 0) && (ddlDepartment.SelectedIndex > 0))
                {
                    dtDivisions = ESDH.PopulateDivisions(ddlDepartment.SelectedValue);

                    ddlDivision.Items.Clear();
                    ddlDivision.Items.Add(new ListItem("", ""));

                    for (int i = 0; i < dtDivisions.Rows.Count; i++)
                    {
                        string Text = dtDivisions.Rows[i]["DIV_NAME"].ToString();
                        string Value = dtDivisions.Rows[i]["DIVISION_ID"].ToString();

                        ddlDivision.Items.Add(new ListItem(Text, Value));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ESDH = null;
                dtDivisions.Dispose();
            }
        }

        void LoadAssessments()
        {
            EmployeemarksStandardizationDataHandler ESDH = new EmployeemarksStandardizationDataHandler();
            DataTable dtAssessments = new DataTable();
            try
            {
                if (((ddlCompany.Items.Count > 0) && (ddlCompany.SelectedIndex > 0)) && ((ddlYearOfAssessment.Items.Count > 0) && (ddlYearOfAssessment.SelectedIndex >= 0)))
                {
                    dtAssessments = ESDH.PopulateAssessments(ddlCompany.SelectedValue, ddlYearOfAssessment.SelectedValue).Copy();

                    ddlAssessment.Items.Clear();
                    ddlAssessment.Items.Add(new ListItem("", ""));

                    for (int i = 0; i < dtAssessments.Rows.Count; i++)
                    {
                        string Text = dtAssessments.Rows[i]["ASSESSMENT_NAME"].ToString();
                        string Value = dtAssessments.Rows[i]["ASSESSMENT_ID"].ToString();

                        ddlAssessment.Items.Add(new ListItem(Text, Value));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ESDH = null;
                dtAssessments.Dispose();
            }
        }

        void LoadLoadEmployees()
        {
            EmployeemarksStandardizationDataHandler ESDH = new EmployeemarksStandardizationDataHandler();
            DataTable dtEmployeeMarks = new DataTable();
            try
            {
                dtEmployeeMarks = ESDH.PopulateEmployeeMarks(ddlCompany.SelectedValue, ddlYearOfAssessment.SelectedValue, ddlAssessment.SelectedValue, ddlDepartment.SelectedValue, ddlDivision.SelectedValue).Copy();

                grdvAssessment.DataSource = dtEmployeeMarks.Copy();
                grdvAssessment.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ESDH = null;
                dtEmployeeMarks.Dispose();
            }
        }

        void ClearGrid()
        {
            try
            {
                grdvAssessment.DataSource = null;
                grdvAssessment.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblStatus);
            if (!IsPostBack)
            {
                try
                {
                    log.Debug("IP:" + Request.UserHostAddress + "WebFrmEmployeeMarksStandardization : Page_Load");

                    SetFinancialYear();
                    LoadCompanies();
                    LoadAssessmentYears();

                    //LoadDepartments(ddlDepartment);
                }
                catch (Exception exp)
                {
                    log.Error("WebFrmEmployeeMarksStandardization : Page_Load | " + exp.Message);
                    CommonVariables.MESSAGE_TEXT = exp.Message;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
                }
                finally
                {

                }
            }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmEmployeeMarksStandardization : ddlCompany_SelectedIndexChanged()");
                ClearGrid();
                LoadDepartments(ddlDepartment);
                LoadAssessments();
            }
            catch (Exception exp)
            {
                log.Error("WebFrmEmployeeMarksStandardization : ddlCompany_SelectedIndexChanged | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmEmployeeMarksStandardization : ddlDepartment_SelectedIndexChanged()");
                ClearGrid();
                LoadDivisions();
            }
            catch (Exception exp)
            {
                log.Error("WebFrmEmployeeMarksStandardization : ddlDepartment_SelectedIndexChanged | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmEmployeeMarksStandardization : ddlYearOfAssessment_SelectedIndexChanged()");
                ClearGrid();
            }
            catch (Exception exp)
            {
                log.Error("WebFrmEmployeeMarksStandardization : ddlYearOfAssessment_SelectedIndexChanged | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        protected void ddlYearOfAssessment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmEmployeeMarksStandardization : ddlYearOfAssessment_SelectedIndexChanged()");
                ClearGrid();
                LoadAssessments();
            }
            catch (Exception exp)
            {
                log.Error("WebFrmEmployeeMarksStandardization : ddlYearOfAssessment_SelectedIndexChanged | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        protected void ddlAssessment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmEmployeeMarksStandardization : ddlYearOfAssessment_SelectedIndexChanged()");
                ClearGrid();
            }
            catch (Exception exp)
            {
                log.Error("WebFrmEmployeeMarksStandardization : ddlYearOfAssessment_SelectedIndexChanged | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        protected void grdvAssessment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string StandardizedTotalScore = HttpUtility.HtmlDecode(e.Row.Cells[6].Text).Trim();
                    TextBox txtTotalScore = (e.Row.FindControl("txtTotalScore") as TextBox);
                    txtTotalScore.Text = StandardizedTotalScore;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                log.Debug("WebFrmEmployeeMarksStandardization : imgbtnSearch_Click()");

                Utility.Errorhandler.ClearError(lblStatus);


                if (ddlAssessment.SelectedIndex <= 0)
                {
                    CommonVariables.MESSAGE_TEXT = "Assessment is Required";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                    return;
                }

                LoadLoadEmployees();
            }
            catch (Exception exp)
            {
                log.Error("WebFrmEmployeeMarksStandardization : imgbtnSearch_Click | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            EmployeemarksStandardizationDataHandler ESDH = new EmployeemarksStandardizationDataHandler();
            DataTable dtEmployeeMarks = new DataTable();
            try
            {
                log.Debug("WebFrmEmployeeMarksStandardization : btnSave_Click()");

                if (grdvAssessment.Rows.Count > 0)
                {

                    dtEmployeeMarks.Columns.Add("EMPLOYEE_ID");
                    dtEmployeeMarks.Columns.Add("TOTAL");

                    for (int i = 0; i < grdvAssessment.Rows.Count; i++)
                    {
                        string EmployeeID = HttpUtility.HtmlDecode(grdvAssessment.Rows[i].Cells[0].Text).Trim();

                        TextBox txtTotalScore = (grdvAssessment.Rows[i].FindControl("txtTotalScore") as TextBox);
                        txtTotalScore.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");

                        string TotalMarks = txtTotalScore.Text.Trim();

                        Double Marks = 0.0;
                        if (Double.TryParse(TotalMarks, out Marks) == false)
                        {
                            txtTotalScore.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFC300");
                            CommonVariables.MESSAGE_TEXT = "Invalid total score";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                            return;
                        }

                        DataRow dr = dtEmployeeMarks.NewRow();
                        dr["EMPLOYEE_ID"] = EmployeeID;
                        dr["TOTAL"] = TotalMarks;

                        dtEmployeeMarks.Rows.Add(dr);
                    }

                    string AssessmentID = ddlAssessment.SelectedValue.ToString().Trim();
                    string YearOfAssessment = ddlYearOfAssessment.SelectedValue.ToString().Trim();
                    string ModifiedBy = (Session["KeyUSER_ID"] as string);

                    ESDH.Update(dtEmployeeMarks.Copy(), AssessmentID, YearOfAssessment, ModifiedBy);

                    CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "No employee marks to modify";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                    return;
                }
            }
            catch (Exception exp)
            {
                log.Error("WebFrmEmployeeMarksStandardization : btnSave_Click | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                dtEmployeeMarks.Dispose();
                ESDH = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("WebFrmEmployeeMarksStandardization : btnClear_Click()");
                Utility.Utils.clearControls(true, ddlCompany, ddlYearOfAssessment);
                ddlDepartment.Items.Clear();
                ddlDivision.Items.Clear();
                ddlAssessment.Items.Clear();
                LoadLoadEmployees();

            }
            catch (Exception exp)
            {
                log.Error("WebFrmEmployeeMarksStandardization : btnClear_Click | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        #endregion

        protected void btnApply_Click(object sender, EventArgs e)
        {
            EmployeemarksStandardizationDataHandler ESDH = new EmployeemarksStandardizationDataHandler();
            try
            {
                log.Debug("WebFrmEmployeeMarksStandardization : btnClear_Click()");

                Utility.Errorhandler.ClearError(lblStatus);

                //Get the button that raised the event
                Button btn = (Button)sender;

                //Get the row that contains this button
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;


                TextBox txtTotalScore = (gvr.FindControl("txtTotalScore") as TextBox);
                Double Marks = 0.0;
                if (Double.TryParse(txtTotalScore.Text.Trim(), out Marks) == false)
                {
                    if (txtTotalScore.Text.Trim() != String.Empty)
                    {
                        txtTotalScore.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFC300");
                        CommonVariables.MESSAGE_TEXT = "Invalid total score";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                        return;
                    }
                }
                else
                {
                    if ((Marks >= 0) && (Marks <= 100))
                    {
                        txtTotalScore.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    }
                    else
                    {
                        txtTotalScore.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFC300");
                        CommonVariables.MESSAGE_TEXT = "Total Score should be between 0 and 100";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                        return;
                    }                    
                }

                string TotalMarks = txtTotalScore.Text.Trim();
                string EmployeeID = HttpUtility.HtmlDecode(gvr.Cells[0].Text).Trim();
                string AssessmentID = ddlAssessment.SelectedValue.ToString();
                string YearOfAssessment = ddlYearOfAssessment.SelectedValue.Trim();
                string ModifiedBy = (Session["KeyUSER_ID"] as string);

                ESDH.Update(TotalMarks, EmployeeID, AssessmentID, YearOfAssessment, ModifiedBy);

                //LoadLoadEmployees();

                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);

            }
            catch (Exception exp)
            {
                log.Error("WebFrmEmployeeMarksStandardization : btnApply_Click | " + exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                ESDH = null;
            }

        }
    }
}