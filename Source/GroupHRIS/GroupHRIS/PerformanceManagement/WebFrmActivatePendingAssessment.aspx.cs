using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.PerformanceManagement;
using System.Data;
using Common;
using GroupHRIS.Utility;
using System.Text;
using NLog;
using DataHandler.Userlogin;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmActivatePendingAssessment : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        public int selectedPage { get; set; }
        //// "1" if checked ; "0" if not

        //// "true" if available ; "false" if not

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmActivatePendingAssessment : Page_Load");
            try
            {
                if (Session["KeyLOGOUT_STS"] == null)
                {
                    Response.Redirect("MainLogout.aspx", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login/SessionExpior.aspx", false);
            }
            if (!IsPostBack)
            {
                Session["AddeddEmployeeCompanyId"] = "";
                //loadAssessmentTypeDropDown();
                //loadAssessmentPurposeDropDown();
                //loadAssessmentPurposeStatusDropDown();

                fillYearDropdown();

                string year = getFinancialYear();
                //lblYearOfAssessment.Text = year;

                //fillStatus();

                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    string comId = Session["KeyCOMP_ID"].ToString();
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        fillCompanyDropDown();
                    }
                    else
                    {
                        fillCompanyDropDown(Session["KeyCOMP_ID"].ToString().Trim());
                    }
                }
                loadAssessmentGridView();
                getPendingAssessmentSummery();

                //loadCompanyDropDown();

                DataTable assessmentPurposesDataTable = new DataTable();
                assessmentPurposesDataTable.Columns.Add("INDEX");
                assessmentPurposesDataTable.Columns.Add("PURPOSE_ID");
                assessmentPurposesDataTable.Columns.Add("NAME");
                //assessmentPurposesDataTable.Columns.Add("DESCRIPTION");
                assessmentPurposesDataTable.Columns.Add("STATUS_CODE");

                Session["AssessmentPurposeSession"] = assessmentPurposesDataTable;

                DataTable AddeddEmloyeeDataTable = new DataTable();
                AddeddEmloyeeDataTable.Columns.Add("emp_id", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("emp_name", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("exclude", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("epf_no", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("goal", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("competency", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("self", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("company_id", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("dept_id", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("division_id", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("email", typeof(String));

                Session["AddedEmployees"] = AddeddEmloyeeDataTable;

                GridViewSelectedEmployees.DataSource = (Session["AddedEmployees"] as DataTable).Copy();
                GridViewSelectedEmployees.DataBind();

                
            }

            if (HiddenField1.Value.ToString() == "dataCaptured")
            {
                //Session.Remove("AddedEmployees");
                DataTable prevSelected = (Session["AddedEmployees"] as DataTable).Copy();
                DataTable selectedEmployees = new DataTable();

                selectedEmployees.Columns.Add("emp_id", typeof(String));
                selectedEmployees.Columns.Add("emp_name", typeof(String));
                selectedEmployees.Columns.Add("exclude", typeof(String));
                selectedEmployees.Columns.Add("epf_no", typeof(String));
                selectedEmployees.Columns.Add("goal", typeof(String));
                selectedEmployees.Columns.Add("competency", typeof(String));
                selectedEmployees.Columns.Add("self", typeof(String));
                selectedEmployees.Columns.Add("company_id", typeof(String));
                selectedEmployees.Columns.Add("dept_id", typeof(String));
                selectedEmployees.Columns.Add("division_id", typeof(String));
                selectedEmployees.Columns.Add("email", typeof(String));

                DataTable allEmployees = new DataTable();

                allEmployees = (DataTable)Session["ChildWindowGridData"];

                foreach (DataRow employee in allEmployees.Rows)
                {
                    string include = employee["INCLUDE"].ToString();
                    if (include == "1")
                    {
                        DataRow[] result = prevSelected.Select("emp_id ='" + employee["EMPLOYEE_ID"].ToString() + "'");
                        if (result.Count() == 0)
                        {
                            DataRow newEmployee = selectedEmployees.NewRow();
                            newEmployee["emp_id"] = employee["EMPLOYEE_ID"].ToString();
                            newEmployee["emp_name"] = employee["KNOWN_NAME"].ToString();
                            newEmployee["epf_no"] = employee["EPF_NO"].ToString();
                            newEmployee["exclude"] = "0";
                            newEmployee["goal"] = employee["GOAL"].ToString();
                            newEmployee["competency"] = employee["COMPETENCY"].ToString();
                            newEmployee["self"] = employee["SELF"].ToString();
                            newEmployee["company_id"] = employee["COMPANY_ID"].ToString();
                            newEmployee["dept_id"] = employee["DEPT_ID"].ToString();
                            newEmployee["division_id"] = employee["DIVISION_ID"].ToString();
                            newEmployee["email"] = employee["EMAIL"].ToString();
                            selectedEmployees.Rows.Add(newEmployee);
                        }
                    }
                    else if (include == "0")
                    {

                    }
                }


                if (Session["AddedEmployees"] != null)
                {
                    //DataTable prevSelected = (Session["AddedEmployees"] as DataTable).Copy();


                    selectedEmployees.Merge(prevSelected);
                }

                for (int i = selectedEmployees.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = selectedEmployees.Rows[i];
                    if (dr["exclude"] == "1")
                        dr.Delete();
                }


                Session["ChildWindowGridData"] = null;

                Session["AddedEmployees"] = selectedEmployees;
                HiddenField1.Value = "dataReleased";
                loadSelectedEmployeesGridView();
            }
            

        }

        //protected void loadAssessmentTypeDropDown()
        //{
        //    log.Debug("webFrmAssessment : loadAssessmentTypeDropDown()");

        //    ActivateAssessmentDataHandler ActivateAssessmentDataHandler = new ActivateAssessmentDataHandler();
        //    DataTable assessmentTypeDataTable = new DataTable();
        //    assessmentTypeDataTable = ActivateAssessmentDataHandler.getAllActiveAssessmentTypes();

        //    ListItem listItemBlank = new ListItem();
        //    listItemBlank.Text = "";
        //    listItemBlank.Value = "";
        //    lblAssessmentType.Items.Add(listItemBlank);

        //    foreach (DataRow resultRow in assessmentTypeDataTable.Rows)
        //    {
        //        ListItem assessmentType = new ListItem();
        //        assessmentType.Text = resultRow["ASSESSMENT_TYPE_NAME"].ToString();
        //        assessmentType.Value = resultRow["ASSESSMENT_TYPE_ID"].ToString();
        //        lblAssessmentType.Items.Add(assessmentType);
        //    }

        //    ActivateAssessmentDataHandler = null;
        //    assessmentTypeDataTable.Dispose();
        //}

        //protected void loadAssessmentPurposeDropDown()
        //{
        //    ActivateAssessmentDataHandler ActivateAssessmentDataHandler = new ActivateAssessmentDataHandler();
        //    DataTable assessmentPurposeDataTable = new DataTable();

        //    assessmentPurposeDataTable = ActivateAssessmentDataHandler.getAllActiveAssessmentPurpose();

        //    ListItem listItemBlank = new ListItem();
        //    listItemBlank.Text = "";
        //    listItemBlank.Value = "";
        //    ddlAssessmentPurpose.Items.Add(listItemBlank);

        //    foreach (DataRow resultRow in assessmentPurposeDataTable.Rows)
        //    {
        //        ListItem assessmentPurpose = new ListItem();
        //        assessmentPurpose.Text = resultRow["NAME"].ToString();
        //        assessmentPurpose.Value = resultRow["PURPOSE_ID"].ToString();
        //        ddlAssessmentPurpose.Items.Add(assessmentPurpose);
        //    }

        //    ActivateAssessmentDataHandler = null;
        //    assessmentPurposeDataTable.Dispose();

        //}

        //protected void loadAssessmentPurposeStatusDropDown()
        //{
        //    ListItem listItemBlank = new ListItem();
        //    listItemBlank.Text = "";
        //    listItemBlank.Value = "";
        //    ddlPurposeStatus.Items.Add(listItemBlank);
        //    //ddlPurposeStatus.Items.Add(listItemBlank);

        //    ListItem listItemActive = new ListItem();
        //    listItemActive.Text = Constants.STATUS_ACTIVE_TAG;
        //    listItemActive.Value = Constants.STATUS_ACTIVE_VALUE;
        //    ddlPurposeStatus.Items.Add(listItemActive);


        //    ListItem listItemInActive = new ListItem();
        //    listItemInActive.Text = Constants.STATUS_INACTIVE_TAG;
        //    listItemInActive.Value = Constants.STATUS_INACTIVE_VALUE;
        //    ddlPurposeStatus.Items.Add(listItemInActive);
        //}

        protected void loadAssessmentGridView()
        {
            log.Debug("loadAssessmentGridView()");
            ActivateAssessmentDataHandler activateAssessmentDataHandler = new ActivateAssessmentDataHandler();
            DataTable assessmentDataTable = activateAssessmentDataHandler.getAllAssessments();

            try
            {
                DataView dataView = new DataView(assessmentDataTable);

                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    string comId = Session["KeyCOMP_ID"].ToString();
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        //ddlCompany.Items.Clear();
                        //fillCompanyDropDown();
                    }
                    else
                    {
                        ddlCompany.Items.Clear();
                        fillCompanyDropDown(Session["KeyCOMP_ID"].ToString().Trim());
                    }
                }

                string company = ddlCompany.SelectedValue.ToString();
                string year = ddlYear.SelectedValue.ToString();
                string status = lblStatus.Text.ToString();

                //string searchYear = txtSearchYear.Text.ToString().Trim();
                //string searchCompany = ddlSearchCompany.SelectedValue.ToString();

                //if (!String.IsNullOrEmpty(searchYear) && !String.IsNullOrEmpty(searchCompany))
                //{
                //    assessmentDataTable.DefaultView.RowFilter = "YEAR_OF_ASSESSMENT like '" + searchYear + "%'" + " AND COMPANY_ID like '" + searchCompany + "%'";
                //}
                //if (!String.IsNullOrEmpty(searchYear) && String.IsNullOrEmpty(searchCompany))
                //{
                //    assessmentDataTable.DefaultView.RowFilter = "YEAR_OF_ASSESSMENT like '" + searchYear + "%'";
                //}
                //if (String.IsNullOrEmpty(searchYear) && !String.IsNullOrEmpty(searchCompany))
                //{
                //    assessmentDataTable.DefaultView.RowFilter = "COMPANY_ID like '" + searchCompany + "%'";
                //}

                if (!String.IsNullOrEmpty(company) && !String.IsNullOrEmpty(year) && !String.IsNullOrEmpty(status)) /// all available
                {
                    dataView.RowFilter = "COMPANY_ID like '" + company + "%'" + " AND YEAR_OF_ASSESSMENT like '" + year + "%'" + " AND STATUS_CODE like '" + status + "%'";
                }
                if (String.IsNullOrEmpty(company) && !String.IsNullOrEmpty(year) && !String.IsNullOrEmpty(status)) /// company is null
                {
                    dataView.RowFilter = " YEAR_OF_ASSESSMENT like '" + year + "%'" + " AND STATUS_CODE like '" + status + "%'";
                }
                if (!String.IsNullOrEmpty(company) && String.IsNullOrEmpty(year) && !String.IsNullOrEmpty(status))  /// year is null
                {
                    dataView.RowFilter = "COMPANY_ID like '" + company + "%'" + " AND STATUS_CODE like '" + status + "%'";
                }
                if (!String.IsNullOrEmpty(company) && !String.IsNullOrEmpty(year) && String.IsNullOrEmpty(status)) /// status is null
                {
                    dataView.RowFilter = "COMPANY_ID like '" + company + "%'" + " AND YEAR_OF_ASSESSMENT like '" + year + "%'";
                }
                if (String.IsNullOrEmpty(company) && String.IsNullOrEmpty(year) && !String.IsNullOrEmpty(status))  /// company and year null
                {
                    dataView.RowFilter = " STATUS_CODE like '" + status + "%'";
                }
                if (String.IsNullOrEmpty(company) && !String.IsNullOrEmpty(year) && String.IsNullOrEmpty(status)) /// company and status null
                {
                    dataView.RowFilter = "YEAR_OF_ASSESSMENT like '" + year + "%'";
                }
                if (!String.IsNullOrEmpty(company) && String.IsNullOrEmpty(year) && String.IsNullOrEmpty(status)) /// year and status null
                {
                    dataView.RowFilter = "COMPANY_ID like '" + company + "%'";
                }


                //if (String.IsNullOrEmpty(searchYear) && String.IsNullOrEmpty(searchCompany))
                //{
                //    assessmentDataTable.DefaultView.RowFilter = "YEAR_OF_ASSESSMENT like '" + searchYear + "%'" + " AND COMPANY_ID like '" + searchCompany + "%'";
                //}
                assessmentDataTable = dataView.ToTable();

                GridViewAssessment.Width = 400;
                GridViewAssessment.DataSource = assessmentDataTable;
                GridViewAssessment.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                assessmentDataTable = null;
                activateAssessmentDataHandler = null;
            }
        }

        protected void gridViewAssessment_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gridViewAssessment_OnRowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.GridViewAssessment, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void gridViewAssessment_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gridViewAssessment_SelectedIndexChanged()");
            ActivateAssessmentDataHandler activateAssessmentDataHandler = new ActivateAssessmentDataHandler();
            try
            {
                Session.Remove("ChildWindowGridData");
                Session.Remove("AddedEmployees");
                //clearAllControls();
                Utils.clearControls(false, txtName, txtRemarks, lblAssessmentType, lblErrorMsg2, lblPurposesList);
                reloadEmployeesSessionWithEmptyDataTable();

                GridViewSelectedEmployees.DataSource = null;
                GridViewSelectedEmployees.DataBind();


                //btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                int selectedIndex = GridViewAssessment.SelectedIndex;
                string assessmentId = GridViewAssessment.Rows[selectedIndex].Cells[0].Text.ToString();
                hiddenAssessmentId.Value = assessmentId;

                
                DataTable assessmentDetailsDataTable = activateAssessmentDataHandler.getAssessmentById(assessmentId);
                DataTable purposesDataTable = activateAssessmentDataHandler.getPurposesForAssessment(assessmentId);
                DataTable assessedEmployeesDataTable = activateAssessmentDataHandler.getEmployeesForAssessment(assessmentId);

                Session["UPDATE_assessmentDetailsDataTable"] = assessmentDetailsDataTable;
                Session["UPDATE_purposesDataTable"] = purposesDataTable;
                Session["UPDATE_assessedEmployeesDataTable"] = assessedEmployeesDataTable;

                Session["AssessmentPurposeSession"] = purposesDataTable;

                //PurposeGridView.Width = 278;
                //PurposeGridView.DataSource = (DataTable)Session["AssessmentPurposeSession"];
                //PurposeGridView.DataBind();


                foreach (DataRow purpose in purposesDataTable.Rows)
                {

                    lblPurposesList.Text = "<li>" + purpose[1].ToString() + "</li>" + lblPurposesList.Text.ToString();
                    //.Text = purpose[1].ToString();

                }
                purposeUl.InnerHtml = HttpUtility.HtmlDecode(lblPurposesList.Text.ToString());


                Session["ChildWindowGridData"] = assessedEmployeesDataTable;


                ddlYear.SelectedValue = assessmentDetailsDataTable.Rows[0][3].ToString();
                txtName.Text = assessmentDetailsDataTable.Rows[0][1].ToString();
                txtRemarks.Text = assessmentDetailsDataTable.Rows[0][4].ToString();
                lblAssessmentType.Text = assessmentDetailsDataTable.Rows[0][5].ToString();
                string status = assessmentDetailsDataTable.Rows[0][7].ToString();
                if (status != Constants.ASSESSNEMT_PENDING_STATUS)
                {
                    //if (status == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    //{
                    //    lblStatus.Items.Clear();
                    //    addTemporaryItemToDropDown(lblStatus, Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG, Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS);
                    //   // disableFields();
                    //}
                    //if (status == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS)
                    //{
                    //    lblStatus.Items.Clear();           
                    //    addTemporaryItemToDropDown(lblStatus, Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG, Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS);
                    //   // disableFields();
                    //}
                    //if (status == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    //{
                    //    lblStatus.Items.Clear();
                    //    addTemporaryItemToDropDown(lblStatus, Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG, Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS);
                    //   // disableFields();
                    //}
                    //if (status == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    //{
                    //    lblStatus.Items.Clear();
                    //    addTemporaryItemToDropDown(lblStatus, Constants.ASSESSNEMT_CEO_FINALIZED_TAG, Constants.ASSESSNEMT_CEO_FINALIZED_STATUS);
                    //   // disableFields();                   
                    //}

                    //if (status == Constants.ASSESSNEMT_ACTIVE_STATUS)
                    //{
                    //    lblStatus.Items.Clear();
                    //    addTemporaryItemToDropDown(lblStatus, Constants.ASSESSNEMT_ACTIVE_TAG, Constants.ASSESSNEMT_ACTIVE_STATUS);
                    //    //lblStatus.SelectedValue = status;
                    //    //disableFields();
                    //}
                    //if (status == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                    //{
                    //    lblStatus.Items.Clear();
                    //    //addTemporaryItemToDropDown(lblStatus, Constants.ASSESSNEMT_CEO_FINALIZED_TAG, Constants.ASSESSNEMT_CEO_FINALIZED_STATUS);
                    //    ListItem obsoleteItem = new ListItem();
                    //    obsoleteItem.Text = Constants.ASSESSNEMT_OBSOLETE_TAG;
                    //    obsoleteItem.Value = Constants.ASSESSNEMT_OBSOLETE_STATUS;
                    //    lblStatus.Items.Add(obsoleteItem);

                    //    lblStatus.SelectedValue = status;
                    //    //lblStatus.Enabled = false;
                    //}
                    //disableFields();
                    ////lblStatus.Enabled = false;
                }
                else
                {
                    //lblStatus.Enabled = true;
                    //lblStatus.Items.Clear();
                    //fillStatus();
                    lblStatus.Text = Constants.ASSESSNEMT_PENDING_TAG;
                    disableFields();
                    //enableFields();
                }

                hiddenPreviouseAssessmentYear.Value = assessmentDetailsDataTable.Rows[0][3].ToString();
                hiddenPreviouseStatus.Value = assessmentDetailsDataTable.Rows[0][7].ToString();

                string compId = assessmentDetailsDataTable.Rows[0][2].ToString();
                Session["AddeddEmployeeCompanyId"] = compId;

                ddlCompany.SelectedValue = compId;
                DataTable selectedEmployees = new DataTable();
                selectedEmployees = (Session["AddedEmployees"] as DataTable).Copy();

                DataTable allEmployees = new DataTable();


                allEmployees = (DataTable)Session["ChildWindowGridData"];

                foreach (DataRow employee in allEmployees.Rows)
                {
                    string include = employee["INCLUDE"].ToString();
                    if (include == "1")
                    {
                        DataRow[] existingEntry = selectedEmployees.Select("emp_id ='" + employee["EMPLOYEE_ID"].ToString() + "'");
                        if (existingEntry.Count() == 0)
                        {
                            DataRow newEmployee = selectedEmployees.NewRow();
                            newEmployee["emp_id"] = employee["EMPLOYEE_ID"].ToString();
                            newEmployee["emp_name"] = employee["KNOWN_NAME"].ToString();
                            newEmployee["epf_no"] = employee["EPF_NO"].ToString();
                            newEmployee["exclude"] = "0";
                            newEmployee["goal"] = employee["GOAL"].ToString();
                            newEmployee["competency"] = employee["COMPETENCY"].ToString();
                            newEmployee["self"] = employee["SELF"].ToString();
                            newEmployee["company_id"] = employee["COMPANY_ID"].ToString();
                            newEmployee["dept_id"] = employee["DEPT_ID"].ToString();
                            newEmployee["division_id"] = employee["DIVISION_ID"].ToString();
                            newEmployee["email"] = employee["EMAIL"].ToString();

                            selectedEmployees.Rows.Add(newEmployee);
                        }
                    }
                }

                Session["AddedEmployees"] = selectedEmployees;
                loadSelectedEmployeesGridView();

                assessmentDetailsDataTable = null;
                purposesDataTable = null;
                assessedEmployeesDataTable = null;
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                activateAssessmentDataHandler = null;
                
            }
        }

        //private void addTemporaryItemToDropDown(DropDownList ddlName, string itemText, string itemValue)
        //{
        //    ListItem newItem = new ListItem();
        //    newItem.Text = itemText;
        //    newItem.Value = itemValue;
        //    ddlName.Items.Add(newItem);
        //    lblStatus.SelectedValue = itemValue;

        //    ListItem obsoleteItem = new ListItem();
        //    obsoleteItem.Text = Constants.ASSESSNEMT_OBSOLETE_TAG;
        //    obsoleteItem.Value = Constants.ASSESSNEMT_OBSOLETE_STATUS;
        //    ddlName.Items.Add(obsoleteItem);


        //}

        private void disableFields()
        {
            log.Debug("disableFields()");
            try
            {
                txtName.Enabled = false;
                txtRemarks.Enabled = false;
                lblAssessmentType.Enabled = false;
                //ddlAssessmentPurpose.Enabled = false;
                //ddlPurposeStatus.Enabled = false;
                //btnAddPurpose.Enabled = false;
                //btnClearPurpose.Enabled = false;
               // GridViewSelectedEmployees.Enabled = false;
                //LinkButton1.Visible = false;
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
        }

        private void enableFields()
        {
            log.Debug("enableFields()");
            try
            {
                txtName.Enabled = true;
                txtRemarks.Enabled = true;
                lblAssessmentType.Enabled = true;
                //ddlAssessmentPurpose.Enabled = true;
                //ddlPurposeStatus.Enabled = true;
                //btnAddPurpose.Enabled = true;
                //btnClearPurpose.Enabled = true;
                GridViewSelectedEmployees.Enabled = true;
                //LinkButton1.Visible = true;
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }

        }

        protected void gridViewAssessment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gridViewAssessment_PageIndexChanging()");
            try
            {
                GridViewAssessment.PageIndex = e.NewPageIndex;
                //clearAllControls();
                loadAssessmentGridView();
                Utility.Errorhandler.ClearError(lblErrorMsg2);
                //Utility.Errorhandler.ClearError(lblErrorMsg);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
        }

        //protected void loadCompanyDropDown()
        //{
        //    ActivateAssessmentDataHandler ActivateAssessmentDataHandler = new ActivateAssessmentDataHandler();
        //    DataTable companyDataTable = ActivateAssessmentDataHandler.getAllActiveCompanies();

        //    ListItem listItemSelectCompany = new ListItem();
        //    listItemSelectCompany.Text = "Select Company";
        //    listItemSelectCompany.Value = "";
        //    ddlCompany.Items.Add(listItemSelectCompany);

        //    ListItem listItemBlank = new ListItem();
        //    listItemBlank.Text = "";
        //    listItemBlank.Value = "";
        //    ddlCompany.Items.Add(listItemBlank);

        //    foreach (DataRow company in companyDataTable.Rows)
        //    {
        //        ListItem listItem = new ListItem();
        //        listItem.Text = company[1].ToString();
        //        listItem.Value = company[0].ToString(); 
        //        ddlCompany.Items.Add(listItem);
        //    }

        //    companyDataTable = null;
        //    ActivateAssessmentDataHandler = null;
        //}

        //protected void loadDepartmentListForSelectedCompanyDropDown(string selectedCompanyId)
        //{
        //    ActivateAssessmentDataHandler ActivateAssessmentDataHandler = new ActivateAssessmentDataHandler();
        //    DataTable departmentDataTable = ActivateAssessmentDataHandler.getActiveDepartmentsForCompany(selectedCompanyId);

        //    ListItem listItemSelectDepartment = new ListItem();
        //    listItemSelectDepartment.Text = "Select Department";
        //    listItemSelectDepartment.Value = "";
        //    ddlDepartment.Items.Add(listItemSelectDepartment);

        //    ListItem listItemBlank = new ListItem();
        //    listItemBlank.Text = "";
        //    listItemBlank.Value = "";
        //    ddlDepartment.Items.Add(listItemBlank);

        //    foreach (DataRow department in departmentDataTable.Rows)
        //    {
        //        ListItem listItem = new ListItem();
        //        listItem.Text = department[1].ToString();
        //        listItem.Value = department[0].ToString();
        //        ddlDepartment.Items.Add(listItem);
        //    }
        //    departmentDataTable = null;
        //    ActivateAssessmentDataHandler = null;
        //}

        //protected void loadDevisionListForSelectedDepartmentDropDown(string selectedDepartmentId)
        //{
        //    ActivateAssessmentDataHandler ActivateAssessmentDataHandler = new ActivateAssessmentDataHandler();
        //    DataTable devisionDataTable = ActivateAssessmentDataHandler.getActiveDevisionsForDepartment(selectedDepartmentId);

        //    ListItem listItemSelectDevision = new ListItem();
        //    listItemSelectDevision.Text = "Select Devision";
        //    listItemSelectDevision.Value = "";
        //    ddlDevision.Items.Add(listItemSelectDevision);

        //    ListItem listItemBlank = new ListItem();
        //    listItemBlank.Text = "";
        //    listItemBlank.Value = "";
        //    ddlDevision.Items.Add(listItemBlank);

        //    foreach (DataRow devision in devisionDataTable.Rows)
        //    {
        //        ListItem listItem = new ListItem();
        //        listItem.Text = devision[1].ToString();
        //        listItem.Value = devision[0].ToString();
        //        ddlDevision.Items.Add(listItem);
        //    }
        //    devisionDataTable = null;
        //    ActivateAssessmentDataHandler = null;
        //}

        //protected void btnAddPurpose_Click(object sender, EventArgs e)
        //{
        //    Utility.Errorhandler.ClearError(lblErrorMsg);
        //    ActivateAssessmentDataHandler ActivateAssessmentDataHandler = new ActivateAssessmentDataHandler();

        //    string assessmentPurposeId = ddlAssessmentPurpose.SelectedValue.ToString();
        //    string assessmentPurposeStatus = ddlPurposeStatus.SelectedValue.ToString();
        //    DataTable assessmentPurposeTable = (DataTable)Session["AssessmentPurposeSession"];

        //    string buttonAction = "";
        //    if (btnAddPurpose.Text == "Add")
        //    {
        //        buttonAction = "Add";
        //        if (checkPurposeExistance(assessmentPurposeId, assessmentPurposeTable, buttonAction))
        //        {
        //            CommonVariables.MESSAGE_TEXT = "Purpose already exists";
        //            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
        //        }
        //        else
        //        {
        //            DataRow assessmentPurposeDetailsFromDb = ActivateAssessmentDataHandler.getPurposeDetailsById(assessmentPurposeId);

        //            DataRow assessmentPurposeDataRow = assessmentPurposeTable.NewRow();
        //            assessmentPurposeDataRow["PURPOSE_ID"] = assessmentPurposeId;
        //            assessmentPurposeDataRow["NAME"] = assessmentPurposeDetailsFromDb["NAME"];
        //            //assessmentPurposeDataRow["DESCRIPTION"] = assessmentPurposeDetailsFromDb["DESCRIPTION"];
        //            if (assessmentPurposeStatus == "0")
        //            {
        //                assessmentPurposeDataRow["STATUS_CODE"] = "Inactive";
        //            }
        //            else if (assessmentPurposeStatus == "1")
        //            {
        //                assessmentPurposeDataRow["STATUS_CODE"] = "Active";
        //            }

        //            assessmentPurposeTable.Rows.Add(assessmentPurposeDataRow);
        //            PurposeGridView.Width = 278;
        //            PurposeGridView.DataSource = assessmentPurposeTable;
        //            PurposeGridView.DataBind();
        //            Session["AssessmentPurposeSession"] = assessmentPurposeTable;

        //            //CommonVariables.MESSAGE_TEXT = "Record(s) added successfully.";
        //            //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
        //        }
        //    }
        //    else if (btnAddPurpose.Text == Constants.CON_UPDATE_BUTTON_TEXT)
        //    {
        //        int selectedIndex = Convert.ToInt32(HiddenSelectedIndex.Value);

        //        if (checkPurposeExistance(assessmentPurposeId, assessmentPurposeTable, buttonAction))
        //        {
        //            CommonVariables.MESSAGE_TEXT = "Purpose already exists";
        //            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
        //        }
        //        else
        //        {
        //            DataRow assessmentPurposeDetailsFromDb = ActivateAssessmentDataHandler.getPurposeDetailsById(assessmentPurposeId);

        //            DataRow existingRow = assessmentPurposeTable.Rows[selectedIndex];
        //            //existingRow.Delete();

        //            // DataRow assessmentPurposeDataRow = assessmentPurposeTable.NewRow();
        //            existingRow["PURPOSE_ID"] = assessmentPurposeId;
        //            existingRow["NAME"] = assessmentPurposeDetailsFromDb["NAME"];
        //            //existingRow["DESCRIPTION"] = assessmentPurposeDetailsFromDb["DESCRIPTION"];
        //            if (assessmentPurposeStatus == "0")
        //            {
        //                existingRow["STATUS_CODE"] = "Inactive";
        //            }
        //            else if (assessmentPurposeStatus == "1")
        //            {
        //                existingRow["STATUS_CODE"] = "Active";
        //            }

        //            //assessmentPurposeTable.Rows.Add(assessmentPurposeDataRow);
        //            PurposeGridView.Width = 278;
        //            PurposeGridView.DataSource = assessmentPurposeTable;
        //            PurposeGridView.DataBind();
        //            Session["AssessmentPurposeSession"] = assessmentPurposeTable;
        //            CommonVariables.MESSAGE_TEXT = "Record(s) updated successfully.";
        //            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
        //        }
        //    }


        //    //clearPurposeControls();
        //}

        //protected void PurposeGridView_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int index = PurposeGridView.SelectedIndex;
        //    btnAddPurpose.Text = Constants.CON_UPDATE_BUTTON_TEXT;
        //    HiddenSelectedIndex.Value = index.ToString();

        //    string purpose = PurposeGridView.Rows[index].Cells[0].Text.ToString().Trim();
        //    ddlAssessmentPurpose.SelectedValue = purpose;
        //    ddlAssessmentPurpose.Enabled = false;

        //    string purposeStatus = PurposeGridView.Rows[index].Cells[2].Text.ToString().Trim();
        //    if (purposeStatus == "Active")
        //    {
        //        ddlPurposeStatus.SelectedValue = "1";
        //    }
        //    else if (purposeStatus == "Inactive")
        //    {
        //        ddlPurposeStatus.SelectedValue = "0";
        //    }

        //}

        //protected void PurposeGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.PurposeGridView, "Select$" + e.Row.RowIndex.ToString()));
        //            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
        //            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
        //            e.Row.Attributes.Add("style", "cursor:pointer;");
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        CommonVariables.MESSAGE_TEXT = Ex.Message;
        //        // Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
        //    }
        //}

        protected void PurposeGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //PurposeGridView.PageIndex = e.NewPageIndex;
            //clearPurposeControls();
            //Utility.Errorhandler.ClearError(lblErrorMsg);
        }

        protected Boolean checkPurposeExistance(string assessmentPurposeId, DataTable assessmentPurposeTable, string action)
        {
            Boolean isExists = false;
            DataRow[] results = assessmentPurposeTable.Select("PURPOSE_ID ='" + assessmentPurposeId + "'");
            if (action == "Add")
            {
                if (results.Length > 0)
                {
                    isExists = true;
                }
            }
            else if (action == "Update")
            {
                if (results.Length == 1)
                {
                    isExists = true;
                }
            }
            return isExists;
        }

        //protected void clearPurposeControls()
        //{
        //    btnAddPurpose.Text = "Add";
        //    Utils.clearControls(true, ddlAssessmentPurpose, ddlPurposeStatus);
        //    ddlAssessmentPurpose.Enabled = true;
        //}

        protected void btnClearPurpose_Click(object sender, EventArgs e)
        {
            //clearPurposeControls();
            //Utility.Errorhandler.ClearError(lblErrorMsg);
        }

        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    DataTable purposeDataTable = (DataTable)Session["AssessmentPurposeSession"];

        //    excludeEmployees();
        //    DataTable assessedEmployees = (DataTable)Session["AddedEmployees"];

        //    string assessmentName = lblName.Text.ToString().Trim();
        //    string assessmentType = lblAssessmentType.SelectedValue.ToString();
        //    string remarks = lblRemarks.Text.ToString().Trim();
        //    //string year = lblYearOfAssessment.Text.ToString();
        //    string year = ddlYear.SelectedValue.ToString();
        //    string status = lblStatus.SelectedValue.ToString();
        //    string addedUserId = Session["KeyUSER_ID"].ToString();
        //    //string companyId = hiddenSelectedCompanyId.Value.ToString();
        //    //Session["AddeddEmployeeCompanyId"] = hiddenSelectedCompanyId.Value.ToString();
        //    string companyId = Session["AddeddEmployeeCompanyId"].ToString();

        //    if (purposeDataTable.Rows.Count > 0 && assessedEmployees.Rows.Count > 0)
        //    {
        //        if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
        //        {
        //            if (status == Constants.ASSESSNEMT_PENDING_STATUS)
        //            {
        //                if (year == getFinancialYear())
        //                {
        //                    ActivateAssessmentDataHandler ActivateAssessmentDataHandler = new ActivateAssessmentDataHandler();
        //                    Boolean nameIsExsists = ActivateAssessmentDataHandler.checkAssessmentNameExistance(assessmentName);
        //                    if (nameIsExsists)
        //                    {
        //                        CommonVariables.MESSAGE_TEXT = "Assessment Name already exists";
        //                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
        //                    }
        //                    else
        //                    {
        //                        bool isInserted = ActivateAssessmentDataHandler.insert(purposeDataTable, assessedEmployees, assessmentName, assessmentType, remarks, year, status, addedUserId, companyId);
        //                        if (isInserted)
        //                        {
        //                            // Response.Redirect(Request.RawUrl);
        //                            reloadEmployeesSessionWithEmptyDataTable();
        //                            reloadPurposeSessionWithEmptyDataTable();
        //                            clearAllControls();
        //                            CommonVariables.MESSAGE_TEXT = "Record(s) saved successfully.";
        //                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);

        //                        }
        //                        else
        //                        {
        //                            CommonVariables.MESSAGE_TEXT = "Record(s) could not be saved.";
        //                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    CommonVariables.MESSAGE_TEXT = " Year of assessment should be currrent financial year";
        //                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
        //                }
        //            }
        //            else
        //            {
        //                CommonVariables.MESSAGE_TEXT = " Initial status of an assessment should be 'Pending'";
        //                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
        //            }

        //            loadAssessmentGridView();

        //        }
        //        else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
        //        {
        //            string assessmentId = hiddenAssessmentId.Value.ToString();
        //            string prevStatus = hiddenPreviouseStatus.Value.ToString();

        //            if (hiddenPreviouseAssessmentYear.Value.ToString() == getFinancialYear() && getFinancialYear() == year)
        //            {
        //                ActivateAssessmentDataHandler ActivateAssessmentDataHandler = new ActivateAssessmentDataHandler();

        //                //if (prevStatus == Constants.ASSESSNEMT_PENDING_STATUS)
        //                //{

        //                    Boolean nameIsExsists = ActivateAssessmentDataHandler.checkAssessmentNameExistance(assessmentName, assessmentId);
        //                    if (nameIsExsists)
        //                    {
        //                        CommonVariables.MESSAGE_TEXT = "Record(s) already exists with name '" + assessmentName + "'";
        //                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
        //                    }
        //                    else
        //                    {
        //                        bool isUpdated = ActivateAssessmentDataHandler.update(assessmentId, purposeDataTable, assessedEmployees, assessmentName, assessmentType, remarks, year, status, addedUserId, companyId);

        //                        if (isUpdated)
        //                        {
        //                            //Session["AddedEmployees"] = null;
        //                            reloadEmployeesSessionWithEmptyDataTable();
        //                            reloadPurposeSessionWithEmptyDataTable();
        //                            clearAllControls(); ;
        //                            CommonVariables.MESSAGE_TEXT = "Record(s) updated successfully.";
        //                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
        //                        }
        //                    }
        //                //}

        //            }
        //            else
        //            {
        //                CommonVariables.MESSAGE_TEXT = "Year of assessment does not match to current financial year";
        //                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
        //            }


        //            loadAssessmentGridView();
        //        }
        //    }
        //    else
        //    {
        //        Session["ChildWindowGridData"] = null;
        //        Session["AddedEmployees"] = null;

        //        reloadEmployeesSessionWithEmptyDataTable();
        //        string assessmentId = hiddenAssessmentId.Value.ToString();

        //        ActivateAssessmentDataHandler ActivateAssessmentDataHandler = new ActivateAssessmentDataHandler();
        //        DataTable assessedEmployeesDataTable = ActivateAssessmentDataHandler.getEmployeesForAssessment(assessmentId);
        //        DataTable selectedEmployees = new DataTable();
        //        selectedEmployees = (DataTable)Session["AddedEmployees"];

        //        DataTable allEmployees = new DataTable();

        //        Session["ChildWindowGridData"] = assessedEmployeesDataTable;
        //        allEmployees = (DataTable)Session["ChildWindowGridData"];

        //        foreach (DataRow employee in allEmployees.Rows)
        //        {
        //            string include = employee["INCLUDE"].ToString();
        //            if (include == "1")
        //            {
        //                DataRow[] existingEntry = selectedEmployees.Select("emp_id ='" + employee["EMPLOYEE_ID"].ToString() + "'");
        //                if (existingEntry.Count() == 0)
        //                {
        //                    DataRow newEmployee = selectedEmployees.NewRow();
        //                    newEmployee["emp_id"] = employee["EMPLOYEE_ID"].ToString();
        //                    newEmployee["emp_name"] = employee["KNOWN_NAME"].ToString();
        //                    newEmployee["epf_no"] = employee["EPF_NO"].ToString();
        //                    newEmployee["exclude"] = "0";
        //                    newEmployee["goal"] = employee["GOAL"].ToString();
        //                    newEmployee["competency"] = employee["COMPETENCY"].ToString();
        //                    newEmployee["self"] = employee["SELF"].ToString();
        //                    newEmployee["company_id"] = employee["COMPANY_ID"].ToString();
        //                    newEmployee["dept_id"] = employee["DEPT_ID"].ToString();
        //                    newEmployee["division_id"] = employee["DIVISION_ID"].ToString();
        //                    selectedEmployees.Rows.Add(newEmployee);
        //                }
        //                ///
        //                /// adding data To main window
        //                ///
        //                //EmployeeDataTable empDataTbl = new EmployeeDataTable();
        //                //empDataTbl.selectedEmployeeGridAvailable = false;
        //                //empDataTbl = null;

        //            }
        //        }

        //        loadSelectedEmployeesGridView();

        //        CommonVariables.MESSAGE_TEXT = "Assessed Employees and Purposes are required";
        //        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
        //    }
        //}

        protected string getFinancialYear()
        {
            log.Debug("getFinancialYear()");

            ////finYearStrtDate
            try
            {
                System.DateTime dtfin = System.DateTime.Now;

                int CurrentFinyear = 0;
                string CurrentFinYearDetails = String.Empty;

                DateTime finDate = DateTime.ParseExact(dtfin.Year + "-03-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                if (finDate >= System.DateTime.Now)
                {
                    CurrentFinyear = dtfin.AddYears(-1).Year;
                    CurrentFinYearDetails = " (From March 31, " + CurrentFinyear.ToString() + " To April 1, " + finDate.Year + ")";
                    return CurrentFinyear.ToString();
                }
                else
                {
                    System.DateTime dt = System.DateTime.Now;
                    System.DateTime dtDetais = System.DateTime.Now;

                    string finYearDetails = " (From March 31, " + dt.Year.ToString() + " To April 1, " + dtDetais.AddYears(1).Year + ")";
                    return dt.Year.ToString();
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                return null;
            }

        }

        //private void fillStatus()
        //{
        //    ListItem listItemBlank = new ListItem();
        //    listItemBlank.Text = "";
        //    listItemBlank.Value = "";
        //    lblStatus.Items.Add(listItemBlank);

        //    //ListItem listItemActive = new ListItem();
        //    //listItemActive.Text = Constants.ASSESSNEMT_ACTIVE_TAG;
        //    //listItemActive.Value = Constants.ASSESSNEMT_ACTIVE_STATUS;
        //    //lblStatus.Items.Add(listItemActive);

        //    ListItem listItemPending = new ListItem();
        //    listItemPending.Text = Constants.ASSESSNEMT_PENDING_TAG;
        //    listItemPending.Value = Constants.ASSESSNEMT_PENDING_STATUS;
        //    lblStatus.Items.Add(listItemPending);

        //    ListItem listItemObsolete = new ListItem();
        //    listItemObsolete.Text = Constants.ASSESSNEMT_OBSOLETE_TAG ;
        //    listItemObsolete.Value = Constants.ASSESSNEMT_OBSOLETE_STATUS;
        //    lblStatus.Items.Add(listItemObsolete);

        //    //ListItem listItemSubordinateFinalized = new ListItem();
        //    //listItemSubordinateFinalized.Text = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG;
        //    //listItemSubordinateFinalized.Value = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS;
        //    //lblStatus.Items.Add(listItemSubordinateFinalized);

        //    //ListItem listItemSubordinateDisagree = new ListItem();
        //    //listItemSubordinateDisagree.Text = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
        //    //listItemSubordinateDisagree.Value = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS;
        //    //lblStatus.Items.Add(listItemSubordinateDisagree);

        //    //ListItem listItemSupervisorFinalized = new ListItem();
        //    //listItemSupervisorFinalized.Text = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG;
        //    //listItemSupervisorFinalized.Value = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS;
        //    //lblStatus.Items.Add(listItemSupervisorFinalized);

        //    //ListItem listItemCEOFinalized = new ListItem();
        //    //listItemCEOFinalized.Text = Constants.ASSESSNEMT_CEO_FINALIZED_TAG;
        //    //listItemCEOFinalized.Value = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
        //    //lblStatus.Items.Add(listItemCEOFinalized);
        //}

        //protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ddlDepartment.Items.Clear();
        //    string selectedCompanyId = ddlCompany.SelectedValue.ToString().Trim();
        //    loadDepartmentListForSelectedCompanyDropDown(selectedCompanyId);
        //}

        //protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ddlDevision.Items.Clear();
        //    string selectedDepartmentId = ddlDepartment.SelectedValue.ToString().Trim();
        //    loadDevisionListForSelectedDepartmentDropDown(selectedDepartmentId);
        //}


        /// <summary>
        /// Assessed employees grid view manipulations
        /// </summary>
        protected void loadSelectedEmployeesGridView()
        {
            log.Debug("loadSelectedEmployeesGridView()");
            DataTable selectedEmployees1 = new DataTable();
            try
            {

                selectedEmployees1 = (Session["AddedEmployees"] as DataTable).Copy(); ;

                //GridViewSelectedEmployees.Width = 400;
                GridViewSelectedEmployees.PageIndex = 0;
                GridViewSelectedEmployees.DataSource = selectedEmployees1;
                GridViewSelectedEmployees.DataBind();
                //GridViewSelectedEmployees.PageIndex = 0;


                if (selectedEmployees1.Rows.Count > 0)
                {
                    allChecked_Exclude();
                }
                Utility.Errorhandler.ClearError(lblErrorMsg2);
                selectedEmployees1 = null;
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            { selectedEmployees1 = null; }
        }

        protected void excludeChildCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("excludeChildCheckBox_OnCheckedChanged()");

            DataTable selectedEmployees = new DataTable();
            try
            {
                CheckBox excludeChildCheck = (CheckBox)sender;
                GridViewRow gvr = (GridViewRow)excludeChildCheck.NamingContainer;
                //string id = employeeGridView.Rows[gvr.RowIndex].ToString();
                string employeeId = GridViewSelectedEmployees.Rows[gvr.RowIndex].Cells[0].Text;


                selectedEmployees = (DataTable)Session["AddedEmployees"];

                DataRow[] employee = selectedEmployees.Select("emp_id ='" + employeeId + "'");
                if (excludeChildCheck.Checked)
                {
                    employee[0]["exclude"] = "1";
                }
                else if (excludeChildCheck.Checked == false)
                {
                    employee[0]["exclude"] = "0";

                }
                Session["AddedEmployees"] = selectedEmployees;
                selectedPage = GridViewSelectedEmployees.PageIndex;
                GridViewSelectedEmployees.DataSource = selectedEmployees;
                GridViewSelectedEmployees.DataBind();
                allChecked_Exclude();
                
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                selectedEmployees.Dispose();
            }
        }

        protected void excludeHeaderCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("excludeHeaderCheckBox_OnCheckedChanged()");

            CheckBox selectAll = (CheckBox)GridViewSelectedEmployees.HeaderRow.FindControl("excludeHeaderCheckBox");
            DataTable addedEmployees = new DataTable();

            try
            {
                if (selectAll.Checked == true)
                {
                    addedEmployees = (DataTable)Session["AddedEmployees"];
                    foreach (DataRow employee in addedEmployees.Rows)
                    {
                        employee["exclude"] = "1";
                    }
                    Session["AddedEmployees"] = addedEmployees;

                    selectedPage = GridViewSelectedEmployees.PageIndex;
                    GridViewSelectedEmployees.DataSource = addedEmployees;
                    GridViewSelectedEmployees.DataBind();

                    allChecked_Exclude();
                    addedEmployees.Dispose();
                }
                else if (selectAll.Checked == false)
                {
                    addedEmployees = (DataTable)Session["AddedEmployees"];
                    foreach (DataRow employee in addedEmployees.Rows)
                    {
                        employee["exclude"] = "0";
                    }
                    Session["AddedEmployees"] = addedEmployees;

                    selectedPage = GridViewSelectedEmployees.PageIndex;
                    GridViewSelectedEmployees.DataSource = addedEmployees;
                    GridViewSelectedEmployees.DataBind();

                    allChecked_Exclude();

                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                addedEmployees = null;
                
            }
        }

        protected void allChecked_Exclude()
        {
            log.Debug("allChecked_Exclude()");

            DataTable addedEmployees = new DataTable();
            try
            {
                addedEmployees = (DataTable)Session["AddedEmployees"];

                Boolean allExcludeChecked = true;
                Boolean allSelfChecked = true;
                Boolean allCompetencyChecked = true;
                Boolean allGoalChecked = true;

                foreach (DataRow employee in addedEmployees.Rows)
                {
                    if (employee[2].ToString() == "0")
                    {
                        allExcludeChecked = false;
                    }
                    if (employee[6].ToString() == "0")
                    {
                        allSelfChecked = false;
                    }
                    if (employee[5].ToString() == "0")
                    {
                        allCompetencyChecked = false;
                    }
                    if (employee[4].ToString() == "0")
                    {
                        allGoalChecked = false;
                    }
                }

                if (allExcludeChecked == true)
                {
                    CheckBox excludeHeader = (CheckBox)GridViewSelectedEmployees.HeaderRow.FindControl("excludeHeaderCheckBox");
                    excludeHeader.Checked = true;
                }
                if (allSelfChecked == true)
                {
                    CheckBox selfHeader = (CheckBox)GridViewSelectedEmployees.HeaderRow.FindControl("selfHeaderCheckBox");
                    selfHeader.Checked = true;
                }
                if (allCompetencyChecked == true)
                {
                    CheckBox competencyHeader = (CheckBox)GridViewSelectedEmployees.HeaderRow.FindControl("competencyHeaderCheckBox");
                    competencyHeader.Checked = true;
                }
                if (allGoalChecked == true)
                {
                    CheckBox goalHeader = (CheckBox)GridViewSelectedEmployees.HeaderRow.FindControl("goalHeaderCheckBox");
                    goalHeader.Checked = true;
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                addedEmployees.Dispose();
            }
        }

        protected void gridViewSelectedEmployees_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gridViewSelectedEmployees_OnRowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    int index = e.Row.RowIndex;

                    DataTable dt1 = new DataTable();
                    dt1 = (Session["AddedEmployees"] as DataTable);

                    int pageNo = selectedPage;
                    int dataTableIndex = index + (pageNo) * 10;

                    string exclude = (Session["AddedEmployees"] as DataTable).Rows[dataTableIndex]["exclude"].ToString();
                    string self = (Session["AddedEmployees"] as DataTable).Rows[dataTableIndex]["self"].ToString();
                    string competency = (Session["AddedEmployees"] as DataTable).Rows[dataTableIndex]["competency"].ToString();
                    string goal = (Session["AddedEmployees"] as DataTable).Rows[dataTableIndex]["goal"].ToString();

                    string emId = (Session["AddedEmployees"] as DataTable).Rows[dataTableIndex]["emp_id"].ToString();

                    if (exclude == "1")
                    {
                        string empId = e.Row.Cells[0].Text;
                        CheckBox excludeChild;
                        if (empId == emId)
                        {
                            excludeChild = (e.Row.FindControl("excludeChildCheckBox") as CheckBox);
                            excludeChild.Checked = true;
                        }
                    }
                    else if (exclude == "0")
                    {
                        CheckBox excludeChild = (e.Row.FindControl("excludeChildCheckBox") as CheckBox);
                        excludeChild.Checked = false;
                    }
                    if (goal == "1")
                    {
                        string empId = e.Row.Cells[0].Text;
                        CheckBox goalChild;
                        if (empId == emId)
                        {
                            goalChild = (e.Row.FindControl("goalChildCheckBox") as CheckBox);
                            goalChild.Checked = true;
                            goalChild.Enabled = false;
                        }
                    }
                    else if (goal == "0")
                    {
                        CheckBox goalChild = (e.Row.FindControl("goalChildCheckBox") as CheckBox);
                        goalChild.Checked = false;
                        goalChild.Enabled = false;
                    }

                    if (self == "1")
                    {
                        string empId = e.Row.Cells[0].Text;
                        CheckBox selfChild;
                        if (empId == emId)
                        {
                            selfChild = (e.Row.FindControl("selfChildCheckBox") as CheckBox);
                            selfChild.Checked = true;
                            selfChild.Enabled = false;

                        }
                    }
                    else if (self == "0")
                    {
                        CheckBox selfChild = (e.Row.FindControl("selfChildCheckBox") as CheckBox);
                        selfChild.Checked = false;
                        selfChild.Enabled = false;

                    }

                    if (competency == "1")
                    {
                        string empId = e.Row.Cells[0].Text;
                        CheckBox competencyChild;
                        if (empId == emId)
                        {
                            competencyChild = (e.Row.FindControl("competencyChildCheckBox") as CheckBox);
                            competencyChild.Checked = true;
                            competencyChild.Enabled = false;

                        }
                    }
                    else if (competency == "0")
                    {
                        CheckBox competencyChild = (e.Row.FindControl("competencyChildCheckBox") as CheckBox);
                        competencyChild.Checked = false;
                        competencyChild.Enabled = false;

                    }
                }
            }
            catch (Exception)
            {
                CheckBox excludeChild = (e.Row.FindControl("excludeChildCheckBox") as CheckBox);
                excludeChild.Checked = false;
            }
        }

        protected void gridViewSelectedEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gridViewSelectedEmployees_PageIndexChanging()");
            //gridViewCurrentDataSnap();
            GridViewSelectedEmployees.PageIndex = e.NewPageIndex;
            selectedPage = GridViewSelectedEmployees.PageIndex;
            //populateGrid();

            //getEmployees();
            GridViewSelectedEmployees.DataSource = null;
            GridViewSelectedEmployees.DataBind();

            GridViewSelectedEmployees.DataSource = (DataTable)Session["AddedEmployees"];
            GridViewSelectedEmployees.DataBind();

            allChecked_Exclude();
        }

        protected void btnExclude_Click(object sender, EventArgs e)
        {
            log.Debug("btnExclude_Click()");
            DataTable selectedEmployees = new DataTable();
            try
            {
                selectedEmployees = (DataTable)Session["AddedEmployees"];

                for (int i = selectedEmployees.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow employee = selectedEmployees.Rows[i];
                    if (employee[2].ToString() == "1")
                    {
                        selectedEmployees.Rows.Remove(employee);
                    }
                }

                Session["AddedEmployees"] = selectedEmployees;


                GridViewSelectedEmployees.DataSource = selectedEmployees;
                GridViewSelectedEmployees.DataBind();

                allChecked_Exclude();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                selectedEmployees.Dispose();
            }
        }

        protected void excludeEmployees()
        {
            log.Debug("excludeEmployees()");
            DataTable selectedEmployees = new DataTable();
            try
            {
                selectedEmployees = (DataTable)Session["AddedEmployees"];

                for (int i = selectedEmployees.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow employee = selectedEmployees.Rows[i];
                    if (employee[2].ToString() == "1")
                    {
                        selectedEmployees.Rows.Remove(employee);
                    }
                    else if (employee[4].ToString() == "0" && employee[5].ToString() == "0" && employee[6].ToString() == "0")
                    {
                        selectedEmployees.Rows.Remove(employee);
                    }
                }

                Session["AddedEmployees"] = selectedEmployees;
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                selectedEmployees.Dispose();
            }

        }
        /// <summary>
        /// this methode will 
        /// </summary>
        //protected void reverse_excludeEmployees() 
        //{
        //    DataTable selectedEmployees = new DataTable();
        //    selectedEmployees = (DataTable)Session["AddedEmployees"];

        //    for (int i = selectedEmployees.Rows.Count - 1; i >= 0; i--)
        //    {
        //        DataRow employee = selectedEmployees.Rows[i];
        //        if (employee[2].ToString() == "0")
        //        {
        //            selectedEmployees.Rows.Remove(employee);
        //        }
        //    }

        //    Session["AddedEmployees"] = selectedEmployees;

        //}

        protected void clearAllControls()
        {
            log.Debug("clearAllControls()");

            Utils.clearControls(true, txtName, txtRemarks, lblAssessmentType, lblStatus, ddlCompany, ddlYear, lblPurposesList);
            purposeUl.InnerHtml = HttpUtility.HtmlDecode(lblPurposesList.Text.ToString());

            //Utility.Errorhandler.ClearError(lblErrorMsg);
            //Utility.Errorhandler.ClearError(lblErrorMsg2);
            //PurposeGridView.DataSource = null;
            //PurposeGridView.DataBind();

            GridViewSelectedEmployees.DataSource = null;
            GridViewSelectedEmployees.DataBind();
            //btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            //btnAddPurpose.Text = "Add";

            //lblStatus.Items.Clear();
            //fillStatus();



            
            
            loadAssessmentGridView();
            enableFields();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");

            hiddenAssessmentId.Value = "";
            Session["ChildWindowGridData"] = null;
            Session["AddedEmployees"] = null;
            reloadEmployeesSessionWithEmptyDataTable();
            reloadPurposeSessionWithEmptyDataTable();
            clearAllControls();
            Utility.Errorhandler.ClearError(lblErrorMsg2);
            //Session["AddeddEmployeeCompanyId"] = null;            
        }

        protected void reloadEmployeesSessionWithEmptyDataTable()
        {
            log.Debug("reloadEmployeesSessionWithEmptyDataTable()");
            DataTable AddeddEmloyeeDataTable = new DataTable();
            try
            {
                Session.Remove("ChildWindowGridData");
                Session.Remove("AddedEmployees");

                AddeddEmloyeeDataTable.Columns.Add("emp_id", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("emp_name", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("exclude", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("epf_no", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("goal", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("competency", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("self", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("company_id", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("dept_id", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("division_id", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("email", typeof(String));

                Session["AddedEmployees"] = AddeddEmloyeeDataTable;
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                AddeddEmloyeeDataTable.Dispose();
            }
        }

        protected void reloadPurposeSessionWithEmptyDataTable()
        {
            log.Debug("reloadPurposeSessionWithEmptyDataTable()");

            DataTable assessmentPurposesDataTable = new DataTable();
            assessmentPurposesDataTable.Columns.Add("INDEX");
            assessmentPurposesDataTable.Columns.Add("PURPOSE_ID");
            assessmentPurposesDataTable.Columns.Add("NAME");
            //assessmentPurposesDataTable.Columns.Add("DESCRIPTION");
            assessmentPurposesDataTable.Columns.Add("STATUS_CODE");

            Session["AssessmentPurposeSession"] = assessmentPurposesDataTable;
        }

        protected void goalHeaderCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("goalHeaderCheckBox_OnCheckedChanged()");
            DataTable addedEmployees = new DataTable();
            try
            {
                CheckBox selectAll = (CheckBox)GridViewSelectedEmployees.HeaderRow.FindControl("goalHeaderCheckBox");
                if (selectAll.Checked == true)
                {

                    addedEmployees = (DataTable)Session["AddedEmployees"];
                    foreach (DataRow employee in addedEmployees.Rows)
                    {
                        employee["goal"] = "1";
                    }
                    Session["AddedEmployees"] = addedEmployees;

                    selectedPage = GridViewSelectedEmployees.PageIndex;
                    GridViewSelectedEmployees.DataSource = addedEmployees;
                    GridViewSelectedEmployees.DataBind();

                    allChecked_Exclude();
                    addedEmployees.Dispose();
                }
                else if (selectAll.Checked == false)
                {

                    addedEmployees = (DataTable)Session["AddedEmployees"];
                    foreach (DataRow employee in addedEmployees.Rows)
                    {
                        employee["goal"] = "0";
                    }
                    Session["AddedEmployees"] = addedEmployees;

                    selectedPage = GridViewSelectedEmployees.PageIndex;
                    GridViewSelectedEmployees.DataSource = addedEmployees;
                    GridViewSelectedEmployees.DataBind();

                    allChecked_Exclude();

                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                addedEmployees.Dispose();
            }

        }

        protected void goalChildCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("goalChildCheckBox_OnCheckedChanged()");

            CheckBox goalChildCheck = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)goalChildCheck.NamingContainer;
            //string id = employeeGridView.Rows[gvr.RowIndex].ToString();
            string employeeId = GridViewSelectedEmployees.Rows[gvr.RowIndex].Cells[0].Text;

            DataTable selectedEmployees = new DataTable();
            selectedEmployees = (DataTable)Session["AddedEmployees"];

            DataRow[] employee = selectedEmployees.Select("emp_id ='" + employeeId + "'");
            if (goalChildCheck.Checked)
            {
                employee[0]["goal"] = "1";
            }
            else if (goalChildCheck.Checked == false)
            {
                employee[0]["goal"] = "0";

            }
            Session["AddedEmployees"] = selectedEmployees;
            selectedPage = GridViewSelectedEmployees.PageIndex;
            GridViewSelectedEmployees.DataSource = selectedEmployees;
            GridViewSelectedEmployees.DataBind();
            allChecked_Exclude();
            selectedEmployees.Dispose();
        }

        protected void competencyHeaderCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("competencyHeaderCheckBox_OnCheckedChanged()");

            CheckBox selectAll = (CheckBox)GridViewSelectedEmployees.HeaderRow.FindControl("competencyHeaderCheckBox");
            if (selectAll.Checked == true)
            {
                DataTable addedEmployees = new DataTable();
                addedEmployees = (DataTable)Session["AddedEmployees"];
                foreach (DataRow employee in addedEmployees.Rows)
                {
                    employee["competency"] = "1";
                }
                Session["AddedEmployees"] = addedEmployees;

                selectedPage = GridViewSelectedEmployees.PageIndex;
                GridViewSelectedEmployees.DataSource = addedEmployees;
                GridViewSelectedEmployees.DataBind();

                allChecked_Exclude();
                addedEmployees.Dispose();
            }
            else if (selectAll.Checked == false)
            {
                DataTable addedEmployees = new DataTable();
                addedEmployees = (DataTable)Session["AddedEmployees"];
                foreach (DataRow employee in addedEmployees.Rows)
                {
                    employee["competency"] = "0";
                }
                Session["AddedEmployees"] = addedEmployees;

                selectedPage = GridViewSelectedEmployees.PageIndex;
                GridViewSelectedEmployees.DataSource = addedEmployees;
                GridViewSelectedEmployees.DataBind();

                allChecked_Exclude();

            }

        }

        protected void competencyChildCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("competencyChildCheckBox_OnCheckedChanged()");

            CheckBox goalChildCheck = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)goalChildCheck.NamingContainer;
            //string id = employeeGridView.Rows[gvr.RowIndex].ToString();
            string employeeId = GridViewSelectedEmployees.Rows[gvr.RowIndex].Cells[0].Text;

            DataTable selectedEmployees = new DataTable();
            selectedEmployees = (DataTable)Session["AddedEmployees"];

            DataRow[] employee = selectedEmployees.Select("emp_id ='" + employeeId + "'");
            if (goalChildCheck.Checked)
            {
                employee[0]["competency"] = "1";
            }
            else if (goalChildCheck.Checked == false)
            {
                employee[0]["competency"] = "0";

            }
            Session["AddedEmployees"] = selectedEmployees;
            selectedPage = GridViewSelectedEmployees.PageIndex;
            GridViewSelectedEmployees.DataSource = selectedEmployees;
            GridViewSelectedEmployees.DataBind();
            allChecked_Exclude();
            selectedEmployees.Dispose();
        }

        protected void selfHeaderCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("selfHeaderCheckBox_OnCheckedChanged()");

            CheckBox selectAll = (CheckBox)GridViewSelectedEmployees.HeaderRow.FindControl("selfHeaderCheckBox");
            if (selectAll.Checked == true)
            {
                DataTable addedEmployees = new DataTable();
                addedEmployees = (DataTable)Session["AddedEmployees"];
                foreach (DataRow employee in addedEmployees.Rows)
                {
                    employee["self"] = "1";
                }
                Session["AddedEmployees"] = addedEmployees;

                selectedPage = GridViewSelectedEmployees.PageIndex;
                GridViewSelectedEmployees.DataSource = addedEmployees;
                GridViewSelectedEmployees.DataBind();

                allChecked_Exclude();
                addedEmployees.Dispose();
            }
            else if (selectAll.Checked == false)
            {
                DataTable addedEmployees = new DataTable();
                addedEmployees = (DataTable)Session["AddedEmployees"];
                foreach (DataRow employee in addedEmployees.Rows)
                {
                    employee["self"] = "0";
                }
                Session["AddedEmployees"] = addedEmployees;

                selectedPage = GridViewSelectedEmployees.PageIndex;
                GridViewSelectedEmployees.DataSource = addedEmployees;
                GridViewSelectedEmployees.DataBind();

                allChecked_Exclude();

            }

        }

        protected void selfChildCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("selfChildCheckBox_OnCheckedChanged()");

            DataTable selectedEmployees = new DataTable();
            try
            {
                CheckBox goalChildCheck = (CheckBox)sender;
                GridViewRow gvr = (GridViewRow)goalChildCheck.NamingContainer;
                //string id = employeeGridView.Rows[gvr.RowIndex].ToString();
                string employeeId = GridViewSelectedEmployees.Rows[gvr.RowIndex].Cells[0].Text;

                
                selectedEmployees = (DataTable)Session["AddedEmployees"];

                DataRow[] employee = selectedEmployees.Select("emp_id ='" + employeeId + "'");
                if (goalChildCheck.Checked)
                {
                    employee[0]["self"] = "1";
                }
                else if (goalChildCheck.Checked == false)
                {
                    employee[0]["self"] = "0";

                }
                Session["AddedEmployees"] = selectedEmployees;
                selectedPage = GridViewSelectedEmployees.PageIndex;
                GridViewSelectedEmployees.DataSource = selectedEmployees;
                GridViewSelectedEmployees.DataBind();
                allChecked_Exclude();
                selectedEmployees.Dispose();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally {
                selectedEmployees.Dispose();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            log.Debug("btnSearch_Click()");

            loadAssessmentGridView();
        }

        protected void fillCompanyDropDown()
        {
            log.Debug("fillCompanyDropDown()");
            ActivateAssessmentDataHandler ActivateAssessmentDataHandler = new ActivateAssessmentDataHandler();
            DataTable companyDataTable = new DataTable();
            try
            {

                companyDataTable = ActivateAssessmentDataHandler.getAllActiveCompanies();

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                //ddlSearchCompany.Items.Add(listItemBlank);
                ddlCompany.Items.Add(listItemBlank);

                foreach (DataRow company in companyDataTable.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = company[1].ToString();
                    listItem.Value = company[0].ToString();
                    //ddlSearchCompany.Items.Add(listItem);
                    ddlCompany.Items.Add(listItem);
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                companyDataTable = null;
                ActivateAssessmentDataHandler = null;
            }
        }

        protected void fillCompanyDropDown(string companyId)
        {
            log.Debug("fillCompanyDropDown()");
            ActivateAssessmentDataHandler ActivateAssessmentDataHandler = new ActivateAssessmentDataHandler();
            DataTable companyDataTable = new DataTable();
            try
            {
                companyDataTable = ActivateAssessmentDataHandler.getCompanyById(companyId);

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";

                ddlCompany.Items.Add(listItemBlank);
                foreach (DataRow company in companyDataTable.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = company[1].ToString();
                    listItem.Value = company[0].ToString();
                    ddlCompany.Items.Add(listItem);
                }

                ddlCompany.SelectedValue = companyId;
                
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                ActivateAssessmentDataHandler = null;
                companyDataTable.Dispose();
            }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlCompany_SelectedIndexChanged()");
            try
            {
                hiddenAssessmentId.Value = "";
                loadAssessmentGridView();
                Session["AddeddEmployeeCompanyId"] = ddlCompany.SelectedValue.ToString();
                Session.Remove("AddedEmployees");
                reloadEmployeesSessionWithEmptyDataTable();
                GridViewSelectedEmployees.DataSource = (Session["AddedEmployees"] as DataTable).Copy();
                GridViewSelectedEmployees.DataBind();

                Utils.clearControls(true, txtName, txtRemarks, lblAssessmentType, lblStatus, lblPurposesList);
                purposeUl.InnerHtml = HttpUtility.HtmlDecode(lblPurposesList.Text.ToString());

                Utility.Errorhandler.ClearError(lblErrorMsg2);

                GridViewSelectedEmployees.DataSource = null;
                GridViewSelectedEmployees.DataBind();

                enableFields();
                loadAssessmentGridView();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }

        }

        protected void fillYearDropdown()
        {
            log.Debug("fillYearDropdown()");
            ActivateAssessmentDataHandler ActivateAssessmentDataHandler = new ActivateAssessmentDataHandler();
            DataTable yearDataTable = new DataTable();
            try
            {
                yearDataTable = ActivateAssessmentDataHandler.getDistinctYears();

                string currentFinancialYear = getFinancialYear();
                DataRow[] existingEntry = yearDataTable.Select("YEAR_OF_ASSESSMENT ='" + currentFinancialYear + "'");

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlYear.Items.Add(listItemBlank);



                foreach (DataRow year in yearDataTable.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = year[0].ToString();
                    listItem.Value = year[0].ToString();
                    ddlYear.Items.Add(listItem);

                }
                if (existingEntry.Count() == 0)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = currentFinancialYear;
                    listItem.Value = currentFinancialYear;
                    ddlYear.Items.Add(listItem);
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                ActivateAssessmentDataHandler = null;
                yearDataTable.Dispose();
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlYear_SelectedIndexChanged()");
            loadAssessmentGridView();
            hiddenAssessmentId.Value = "";
        }

        protected void lblStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("lblStatus_SelectedIndexChanged()");
            loadAssessmentGridView();
        }

        protected void btnActivate_Click(object sender, EventArgs e)
        {
            log.Debug("btnActivate_Click()");
            string assessmentId = hiddenAssessmentId.Value.ToString();
            ActivateAssessmentDataHandler activateAssessmentDataHandler = new ActivateAssessmentDataHandler();

            try
            {
                DataTable assessedEmployeesDataTable = activateAssessmentDataHandler.getEmployeesForAssessment(assessmentId);
                DataTable assessmentDetailsDataTable = activateAssessmentDataHandler.getAssessmentById(assessmentId);
                DataTable assessmentPurposesDataTable = activateAssessmentDataHandler.getPurposesForAssessment(assessmentId);

                if (!String.IsNullOrEmpty(assessmentId))
                {
                    string addedUserId = Session["KeyUSER_ID"].ToString();

                    //string expectedCompletionDate = DateTime.Now.AddDays(Constants.CON_ASSESSMENT_ASSESSED_EMPLOYEE_COMPLETION_DURATION).Date.ToString("yyyy-MM-dd");
                    bool isActivated = activateAssessmentDataHandler.ActivateAssessment(assessmentId, assessedEmployeesDataTable, addedUserId);

                    if (isActivated == true)
                    {
                        sendEmailsToAssessedEmployees(assessedEmployeesDataTable, assessmentDetailsDataTable, assessmentPurposesDataTable);

                        CommonVariables.MESSAGE_TEXT = "Assessment activated successfully";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                        clearAllControls();
                        loadAssessmentGridView();
                        getPendingAssessmentSummery();
                    }
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Please select an assessment";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                }
                assessedEmployeesDataTable.Dispose();
                assessmentDetailsDataTable.Dispose();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                activateAssessmentDataHandler = null;
            }
        }

        protected void sendEmailsToAssessedEmployees(DataTable assessedEmployees, DataTable assessmentDetailsDataTable, DataTable assessmentPurposesDataTable)
        {
            log.Debug("sendEmailsToAssessedEmployees()");
            try
            {
                foreach (DataRow employee in assessedEmployees.Rows)
                {
                    string emailAddress = employee[4].ToString();
                    EmailHandler.SendHTMLMail("Performance Evaluation System", emailAddress, "Performance Evaluation Notice", getMessageBody(assessmentDetailsDataTable, assessmentPurposesDataTable));
                }
            }
            catch (Exception)
            {
                CommonVariables.MESSAGE_TEXT = "Emails could not be sent";
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
        }

        protected string getMessageBody(DataTable assessmentDetails, DataTable assessmentPurposesDataTable)
        {
            log.Debug("getMessageBody()");

            StringBuilder msgString = new StringBuilder();

            foreach (DataRow assessment in assessmentDetails.Rows)
            {
                msgString.Append("Dear Sir/Madam, <br />" + Environment.NewLine + Environment.NewLine);
                msgString.Append("<br />" + Environment.NewLine);
                msgString.Append("You have been assigned to a performance evaluation process and details are as follows <br />" + Environment.NewLine + Environment.NewLine);
                msgString.Append("<br />" + Environment.NewLine);
                msgString.Append("Assessment Name : " + assessment["ASSESSMENT_NAME"] + "<br />" + Environment.NewLine);
                msgString.Append("Assessment Type : " + assessment["ASSESSMENT_TYPE_NAME"] + "<br />" + Environment.NewLine);

                if (assessmentPurposesDataTable.Rows.Count > 0)
                {
                    msgString.Append("Assessment Purposes : " + Environment.NewLine);
                    foreach (DataRow purpose in assessmentPurposesDataTable.Rows)
                    {
                        if (purpose["STATUS_CODE"].ToString() != Constants.STATUS_INACTIVE_TAG)
                        {
                            if (assessmentPurposesDataTable.Rows.IndexOf(purpose) != 0)
                            {
                                msgString.Append("&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp; " + Environment.NewLine);
                            }
                            else 
                            {
                                msgString.Append("&nbsp;&nbsp;" + Environment.NewLine);
                            }
                            msgString.Append("-" + purpose["NAME"] + "<br />" + Environment.NewLine);
                        }
                    }
                }
                msgString.Append("<br />" + Environment.NewLine);
                msgString.Append("Please duly complete your parts of the evaluation within 5 days of this notice." + "<br />" + Environment.NewLine + Environment.NewLine);
                msgString.Append("<br />" + Environment.NewLine);
                msgString.Append("Noticed Date :" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "<br />" + Environment.NewLine);
                msgString.Append("Time :" + DateTime.Now.ToShortTimeString() + "<br />" + Environment.NewLine);
                msgString.Append("<br />" + Environment.NewLine);
                msgString.Append("Thank you." + "<br />" + Environment.NewLine + Environment.NewLine);
                msgString.Append("This is a system generated mail." + "<br />" + Environment.NewLine);
            }
            return msgString.ToString();
        }

        protected void getPendingAssessmentSummery()
        {
            log.Debug("getPendingAssessmentSummery()");
            ActivateAssessmentDataHandler activateAssessmentDataHandler = new ActivateAssessmentDataHandler();
            DataTable pendingAssessmentDataTable = new DataTable();
            try
            {
                string company = ddlCompany.SelectedValue.ToString();
                string year = ddlYear.SelectedValue.ToString();

                pendingAssessmentDataTable = activateAssessmentDataHandler.getPendingAssessmentsSummery(year, company);

                if (pendingAssessmentDataTable.Rows.Count > 0)
                {
                    //foreach (DataRow assessment in pendingAssessmentDataTable.Rows)
                    //{
                    //    //TableRow tRow = new TableRow();
                    //    //TableCell tCellCompany = new TableCell();
                    //    //TableCell tCellYear = new TableCell();
                    //    //TableCell tCellCount = new TableCell();

                    //    //tCellCompany.Text = assessment[1].ToString();
                    //    //tCellYear.Text = assessment[2].ToString();
                    //    //tCellCount.Text = assessment[3].ToString();

                    //    //tCellCount.HorizontalAlign = HorizontalAlign.Center;
                    //    //tCellYear.HorizontalAlign = HorizontalAlign.Center;

                    //    //tRow.Cells.Add(tCellCompany);
                    //    //tRow.Cells.Add(tCellYear);
                    //    //tRow.Cells.Add(tCellCount);

                    //    //summeryTable.Rows.Add(tRow);
                    //    //string s = "<tr> <td> "+  assessment[1].ToString() + "</td><td> "+  assessment[2].ToString() + "</td><td> "+  assessment[3].ToString() + "</td></tr>";

                    //    //summeryTableBody.InnerHtml = HttpUtility.HtmlDecode(s);
                    //}

                    gvSummery.DataSource = pendingAssessmentDataTable;
                    gvSummery.DataBind();

                }
                else
                {
                    gvSummery.DataSource = null;
                    gvSummery.DataBind();
                }

            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                activateAssessmentDataHandler = null;
            }
        }

        protected void gvSummery_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvSummery_PageIndexChanging()");
            gvSummery.PageIndex = e.NewPageIndex;
            getPendingAssessmentSummery();
        }
        

    }
}