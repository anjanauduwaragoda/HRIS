using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler;
using DataHandler.PerformanceManagement;
using System.Data;
using NLog;
using Common;
using GroupHRIS.Utility;
namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmAssessment : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        public int selectedPage { get; set; }
        //// "1" if checked ; "0" if not

        //// "true" if available ; "false" if not

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmAssessment : Page_Load");

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

                loadAssessmentTypeDropDown();
                loadAssessmentPurposeDropDown();
                loadAssessmentPurposeStatusDropDown();

                fillYearDropdown();

                string year = getFinancialYear();
                lblYearOfAssessment.Text = year;

                fillStatus();

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
                //loadCompanyDropDown();
                Session.Remove("AssessmentPurposeSession");

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
                AddeddEmloyeeDataTable.Columns.Add("report_to_1", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("role", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("designation_id", typeof(String));

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
                selectedEmployees.Columns.Add("report_to_1", typeof(String));
                selectedEmployees.Columns.Add("role", typeof(String));
                selectedEmployees.Columns.Add("designation_id", typeof(String));

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
                            newEmployee["report_to_1"] = employee["REPORT_TO_1"].ToString();
                            newEmployee["role"] = employee["ROLE"].ToString();
                            newEmployee["designation_id"] = employee["DESIGNATION_ID"].ToString();

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

        protected void loadAssessmentTypeDropDown()
        {
            log.Debug("webFrmAssessment : loadAssessmentTypeDropDown()");

            AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
            DataTable assessmentTypeDataTable = new DataTable();
            try
            {
                assessmentTypeDataTable = assessmentDataHandler.getAllActiveAssessmentTypes();

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlAssessmentType.Items.Add(listItemBlank);

                foreach (DataRow resultRow in assessmentTypeDataTable.Rows)
                {
                    ListItem assessmentType = new ListItem();
                    assessmentType.Text = resultRow["ASSESSMENT_TYPE_NAME"].ToString();
                    assessmentType.Value = resultRow["ASSESSMENT_TYPE_ID"].ToString();
                    ddlAssessmentType.Items.Add(assessmentType);
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                assessmentDataHandler = null;
                assessmentTypeDataTable.Dispose();
            }
        }

        protected void loadAssessmentPurposeDropDown()
        {
            log.Debug("webFrmAssessment : loadAssessmentPurposeDropDown()");
            AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
            DataTable assessmentPurposeDataTable = new DataTable();

            try
            {
                assessmentPurposeDataTable = assessmentDataHandler.getAllActiveAssessmentPurpose();

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlAssessmentPurpose.Items.Add(listItemBlank);

                foreach (DataRow resultRow in assessmentPurposeDataTable.Rows)
                {
                    ListItem assessmentPurpose = new ListItem();
                    assessmentPurpose.Text = resultRow["NAME"].ToString();
                    assessmentPurpose.Value = resultRow["PURPOSE_ID"].ToString();
                    ddlAssessmentPurpose.Items.Add(assessmentPurpose);
                }

            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                assessmentDataHandler = null;
                assessmentPurposeDataTable.Dispose();
            }

        }

        protected void loadAssessmentPurposeStatusDropDown()
        {
            ListItem listItemBlank = new ListItem();
            listItemBlank.Text = "";
            listItemBlank.Value = "";
            ddlPurposeStatus.Items.Add(listItemBlank);
            //ddlPurposeStatus.Items.Add(listItemBlank);

            ListItem listItemActive = new ListItem();
            listItemActive.Text = Constants.STATUS_ACTIVE_TAG;
            listItemActive.Value = Constants.STATUS_ACTIVE_VALUE;
            ddlPurposeStatus.Items.Add(listItemActive);


            ListItem listItemInActive = new ListItem();
            listItemInActive.Text = Constants.STATUS_INACTIVE_TAG;
            listItemInActive.Value = Constants.STATUS_INACTIVE_VALUE;
            ddlPurposeStatus.Items.Add(listItemInActive);
        }

        protected void linkAddAssessedEmployees_click(object sender, EventArgs e)
        {
            log.Debug("webFrmAssessment : linkAddAssessedEmployees_click()");

            try
            {
                string companyId = ddlCompany.SelectedValue.ToString();
                if (String.IsNullOrEmpty(companyId))
                {
                    CommonVariables.MESSAGE_TEXT = "Please select a company";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void loadAssessmentGridView()
        {
            log.Debug("webFrmAssessment : loadAssessmentGridView()");

            AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
            DataTable assessmentDataTable = new DataTable();
            try
            {
                

                string userCompanyId = "";
                if (!Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                {
                    userCompanyId = Session["KeyCOMP_ID"].ToString();
                }

                assessmentDataTable = assessmentDataHandler.getAllAssessments(userCompanyId);

                DataView dataView = new DataView(assessmentDataTable);

                string company = ddlCompany.SelectedValue.ToString();
                string year = ddlYear.SelectedValue.ToString();
                string status = ddlStatus.SelectedItem.Text.ToString();


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
                    dataView.RowFilter = "COMPANY_ID ='" + company + "'";
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
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                assessmentDataHandler = null;
                assessmentDataTable.Dispose();
            }
        }

        protected void gridViewAssessment_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("loadAssessmentGridView()");

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
            log.Debug("webFrmAssessment : gridViewAssessment_SelectedIndexChanged()");

                AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
                DataTable assessmentDetailsDataTable = new DataTable();
                DataTable purposesDataTable = new DataTable();
                DataTable assessedEmployeesDataTable = new DataTable();
                try
                {


                    Session.Remove("ChildWindowGridData");
                    Session.Remove("AddedEmployees");
                    //clearAllControls();

                    ddlAssessmentType.Items.Clear();
                    loadAssessmentTypeDropDown();

                    reloadEmployeesSessionWithEmptyDataTable();

                    GridViewSelectedEmployees.DataSource = null;
                    GridViewSelectedEmployees.DataBind();

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                    int selectedIndex = GridViewAssessment.SelectedIndex;
                    string assessmentId = GridViewAssessment.Rows[selectedIndex].Cells[0].Text.ToString();
                    hiddenAssessmentId.Value = assessmentId;

                    
                    assessmentDetailsDataTable = assessmentDataHandler.getAssessmentById(assessmentId);
                    purposesDataTable = assessmentDataHandler.getPurposesForAssessment(assessmentId);
                    assessedEmployeesDataTable = assessmentDataHandler.getEmployeesForAssessment(assessmentId);

                    Session["UPDATE_assessmentDetailsDataTable"] = assessmentDetailsDataTable;
                    Session["UPDATE_purposesDataTable"] = purposesDataTable;
                    Session["UPDATE_assessedEmployeesDataTable"] = assessedEmployeesDataTable;

                    Session["AssessmentPurposeSession"] = purposesDataTable;

                    PurposeGridView.Width = 250;
                    PurposeGridView.DataSource = (DataTable)Session["AssessmentPurposeSession"];
                    PurposeGridView.DataBind();

                    Session["ChildWindowGridData"] = assessedEmployeesDataTable;


                    ddlYear.SelectedValue = assessmentDetailsDataTable.Rows[0][3].ToString();
                    txtName.Text = assessmentDetailsDataTable.Rows[0][1].ToString();
                    txtRemarks.Text = assessmentDetailsDataTable.Rows[0][4].ToString();
                    //ddlAssessmentType.SelectedValue = assessmentDetailsDataTable.Rows[0][6].ToString();

                    string assessmentTypeId = assessmentDetailsDataTable.Rows[0][6].ToString();
                    string assessmentTypeName = assessmentDetailsDataTable.Rows[0][5].ToString();
                    bool typeIsExist = Utils.isValueExistInDropDownList(assessmentTypeId, ddlAssessmentType);

                    if (!typeIsExist)
                    {
                        addInactiveAssessmentType(ddlAssessmentType, assessmentTypeName, assessmentTypeId);
                    }
                    else
                    {
                        ddlAssessmentType.SelectedValue = assessmentDetailsDataTable.Rows[0][6].ToString();
                    }

                    txtCutOffDate.Text = assessmentDetailsDataTable.Rows[0]["EXPECTED_COMPLETION_DATE"].ToString();




                    string status = assessmentDetailsDataTable.Rows[0][7].ToString();
                    if (status != Constants.ASSESSNEMT_PENDING_STATUS)
                    {
                        if (status == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                        {
                            ddlStatus.Items.Clear();
                            addTemporaryItemToDropDown(ddlStatus, Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG, Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS);
                            // disableFields();
                        }
                        if (status == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS)
                        {
                            ddlStatus.Items.Clear();
                            addTemporaryItemToDropDown(ddlStatus, Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG, Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS);
                            // disableFields();
                        }
                        if (status == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                        {
                            ddlStatus.Items.Clear();
                            addTemporaryItemToDropDown(ddlStatus, Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG, Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS);
                            // disableFields();
                        }
                        if (status == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                        {
                            ddlStatus.Items.Clear();
                            addTemporaryItemToDropDown(ddlStatus, Constants.ASSESSNEMT_CEO_FINALIZED_TAG, Constants.ASSESSNEMT_CEO_FINALIZED_STATUS);
                            // disableFields();                   
                        }

                        if (status == Constants.ASSESSNEMT_ACTIVE_STATUS)
                        {
                            ddlStatus.Items.Clear();
                            addTemporaryItemToDropDown(ddlStatus, Constants.ASSESSNEMT_ACTIVE_TAG, Constants.ASSESSNEMT_ACTIVE_STATUS);
                            //ddlStatus.SelectedValue = status;
                            //disableFields();
                        }
                        if (status == Constants.ASSESSNEMT_CLOSED_STATUS)
                        {
                            ddlStatus.Items.Clear();
                            addTemporaryItemToDropDown(ddlStatus, Constants.ASSESSNEMT_CLOSED_TAG, Constants.ASSESSNEMT_CLOSED_STATUS);
                            ddlStatus.Enabled = false;
                            
                            //ddlStatus.SelectedValue = status;
                            //disableFields();
                        }
                        if (status == Constants.ASSESSNEMT_OBSOLETE_STATUS)
                        {
                            ddlStatus.Items.Clear();
                            //addTemporaryItemToDropDown(ddlStatus, Constants.ASSESSNEMT_CEO_FINALIZED_TAG, Constants.ASSESSNEMT_CEO_FINALIZED_STATUS);
                            ListItem obsoleteItem = new ListItem();
                            obsoleteItem.Text = Constants.ASSESSNEMT_OBSOLETE_TAG;
                            obsoleteItem.Value = Constants.ASSESSNEMT_OBSOLETE_STATUS;
                            ddlStatus.Items.Add(obsoleteItem);

                            ddlStatus.SelectedValue = status;
                            //ddlStatus.Enabled = false;
                        }
                        disableFields();

                        //ddlStatus.Enabled = false;
                    }
                    else
                    {
                        ddlStatus.Enabled = true;
                        ddlStatus.Items.Clear();
                        fillStatus();
                        ddlStatus.SelectedValue = status;
                        enableFields();
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
                                newEmployee["report_to_1"] = employee["REPORT_TO_1"].ToString();
                                newEmployee["role"] = employee["ROLE"].ToString();
                                newEmployee["designation_id"] = employee["DESIGNATION_ID"].ToString();

                                selectedEmployees.Rows.Add(newEmployee);
                            }
                        }
                    }

                    Session["AddedEmployees"] = selectedEmployees;
                    loadSelectedEmployeesGridView();
                    selectedEmployees.Dispose();
                    //assessmentDetailsDataTable.Dispose();
                    //purposesDataTable.Dispose();
                    //assessedEmployeesDataTable.Dispose();
                    //assessmentDataHandler = null;

                }
                catch (Exception Ex)
                {
                    CommonVariables.MESSAGE_TEXT = Ex.Message;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                }
                finally
                { 
                    assessmentDetailsDataTable.Dispose();
                    purposesDataTable.Dispose();
                    assessedEmployeesDataTable.Dispose();
                    assessmentDataHandler = null;   
                }
        }

        private void addInactiveAssessmentType(DropDownList ddlName, string itemText, string itemValue)
        {
            log.Debug("addInactiveAssessmentType()");
            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = itemText;
                newItem.Value = itemValue;
                //newItem.Attributes.Add("disabled", "disabled");
                ddlName.Items.Add(newItem);
                ddlName.SelectedValue = itemValue;
                //ddlName.Items[ddlName.Items.Count - 1].Attributes.Add("disabled", "disabled");
                //newItem.Attributes.Add("disabled", "disabled");
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
           
        }

        private void addTemporaryItemToDropDown(DropDownList ddlName, string itemText, string itemValue)
        {
            log.Debug("addTemporaryItemToDropDown()");
            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = itemText;
                newItem.Value = itemValue;
                ddlName.Items.Add(newItem);
                ddlStatus.SelectedValue = itemValue;

                ListItem obsoleteItem = new ListItem();
                obsoleteItem.Text = Constants.ASSESSNEMT_OBSOLETE_TAG;
                obsoleteItem.Value = Constants.ASSESSNEMT_OBSOLETE_STATUS;
                ddlName.Items.Add(obsoleteItem);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }

        }

        private void disableFields()
        {
            log.Debug("disableFields()");
            txtName.Enabled = false;
            txtRemarks.Enabled = false;
            txtCutOffDate.Enabled = false;
            ddlAssessmentType.Enabled = false;
            ddlAssessmentPurpose.Enabled = false;
            ddlPurposeStatus.Enabled = false;
            btnAddPurpose.Enabled = false;
            btnClearPurpose.Enabled = false;
            GridViewSelectedEmployees.Enabled = false;
            LinkButton1.Visible = false;
        }

        private void enableFields()
        {
            log.Debug("enableFields()");
            txtName.Enabled = true;
            txtRemarks.Enabled = true;
            txtCutOffDate.Enabled = true;
            ddlAssessmentType.Enabled = true;
            ddlAssessmentPurpose.Enabled = true;
            ddlPurposeStatus.Enabled = true;
            btnAddPurpose.Enabled = true;
            btnClearPurpose.Enabled = true;
            GridViewSelectedEmployees.Enabled = true;
            LinkButton1.Visible = true;

        }

        protected void gridViewAssessment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("webFrmAssessment : gridViewAssessment_PageIndexChanging()");

            GridViewAssessment.PageIndex = e.NewPageIndex;
            //clearAllControls();
            loadAssessmentGridView();
            Utility.Errorhandler.ClearError(lblErrorMsg2);
            Utility.Errorhandler.ClearError(lblErrorMsg);
        }

        //protected void loadCompanyDropDown()
        //{
        //    AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
        //    DataTable companyDataTable = assessmentDataHandler.getAllActiveCompanies();

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
        //    assessmentDataHandler = null;
        //}

        //protected void loadDepartmentListForSelectedCompanyDropDown(string selectedCompanyId)
        //{
        //    AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
        //    DataTable departmentDataTable = assessmentDataHandler.getActiveDepartmentsForCompany(selectedCompanyId);

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
        //    assessmentDataHandler = null;
        //}

        //protected void loadDevisionListForSelectedDepartmentDropDown(string selectedDepartmentId)
        //{
        //    AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
        //    DataTable devisionDataTable = assessmentDataHandler.getActiveDevisionsForDepartment(selectedDepartmentId);

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
        //    assessmentDataHandler = null;
        //}

        protected void btnAddPurpose_Click(object sender, EventArgs e)
        {
            log.Debug("webFrmAssessment : btnAddPurpose_Click()");

            try
            {
                Utility.Errorhandler.ClearError(lblErrorMsg);
                AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();

                string assessmentPurposeId = ddlAssessmentPurpose.SelectedValue.ToString();
                string assessmentPurposeStatus = ddlPurposeStatus.SelectedValue.ToString();
                DataTable assessmentPurposeTable = (DataTable)Session["AssessmentPurposeSession"];

                string buttonAction = "";
                if (btnAddPurpose.Text == "Add")
                {
                    buttonAction = "Add";
                    if (checkPurposeExistance(assessmentPurposeId, assessmentPurposeTable, buttonAction))
                    {
                        CommonVariables.MESSAGE_TEXT = "Purpose already exists";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                    else
                    {
                        DataRow assessmentPurposeDetailsFromDb = assessmentDataHandler.getPurposeDetailsById(assessmentPurposeId);

                        DataRow assessmentPurposeDataRow = assessmentPurposeTable.NewRow();
                        assessmentPurposeDataRow["PURPOSE_ID"] = assessmentPurposeId;
                        assessmentPurposeDataRow["NAME"] = assessmentPurposeDetailsFromDb["NAME"];
                        //assessmentPurposeDataRow["DESCRIPTION"] = assessmentPurposeDetailsFromDb["DESCRIPTION"];
                        if (assessmentPurposeStatus == "0")
                        {
                            assessmentPurposeDataRow["STATUS_CODE"] = "Inactive";
                        }
                        else if (assessmentPurposeStatus == "1")
                        {
                            assessmentPurposeDataRow["STATUS_CODE"] = "Active";
                        }

                        assessmentPurposeTable.Rows.Add(assessmentPurposeDataRow);
                        PurposeGridView.Width = 278;
                        PurposeGridView.DataSource = assessmentPurposeTable;
                        PurposeGridView.DataBind();
                        Session["AssessmentPurposeSession"] = assessmentPurposeTable;

                        //CommonVariables.MESSAGE_TEXT = "Record(s) added successfully.";
                        //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                }
                else if (btnAddPurpose.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    int selectedIndex = Convert.ToInt32(HiddenSelectedIndex.Value);

                    if (checkPurposeExistance(assessmentPurposeId, assessmentPurposeTable, buttonAction))
                    {
                        CommonVariables.MESSAGE_TEXT = "Purpose already exists";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                    else
                    {
                        DataRow assessmentPurposeDetailsFromDb = assessmentDataHandler.getPurposeDetailsById(assessmentPurposeId);

                        DataRow existingRow = assessmentPurposeTable.Rows[selectedIndex];
                        //existingRow.Delete();

                        // DataRow assessmentPurposeDataRow = assessmentPurposeTable.NewRow();
                        existingRow["PURPOSE_ID"] = assessmentPurposeId;
                        existingRow["NAME"] = assessmentPurposeDetailsFromDb["NAME"];
                        //existingRow["DESCRIPTION"] = assessmentPurposeDetailsFromDb["DESCRIPTION"];
                        if (assessmentPurposeStatus == "0")
                        {
                            existingRow["STATUS_CODE"] = "Inactive";
                        }
                        else if (assessmentPurposeStatus == "1")
                        {
                            existingRow["STATUS_CODE"] = "Active";
                        }

                        //assessmentPurposeTable.Rows.Add(assessmentPurposeDataRow);
                        PurposeGridView.Width = 278;
                        PurposeGridView.DataSource = assessmentPurposeTable;
                        PurposeGridView.DataBind();
                        Session["AssessmentPurposeSession"] = assessmentPurposeTable;
                        //CommonVariables.MESSAGE_TEXT = "Record(s) updated successfully.";
                        //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                }
                assessmentPurposeTable.Dispose();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                clearPurposeControls();
            }
        }

        protected void PurposeGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("webFrmAssessment : PurposeGridView_SelectedIndexChanged()");

            try
            {
                int index = PurposeGridView.SelectedIndex;
                btnAddPurpose.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                HiddenSelectedIndex.Value = index.ToString();

                string purpose = PurposeGridView.Rows[index].Cells[0].Text.ToString().Trim();
                ddlAssessmentPurpose.SelectedValue = purpose;
                ddlAssessmentPurpose.Enabled = false;

                string purposeStatus = PurposeGridView.Rows[index].Cells[2].Text.ToString().Trim();
                if (purposeStatus == "Active")
                {
                    ddlPurposeStatus.SelectedValue = "1";
                }
                else if (purposeStatus == "Inactive")
                {
                    ddlPurposeStatus.SelectedValue = "0";
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }

        }

        protected void PurposeGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.PurposeGridView, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void PurposeGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PurposeGridView.PageIndex = e.NewPageIndex;
            clearPurposeControls();
            Utility.Errorhandler.ClearError(lblErrorMsg);
        }

        protected Boolean checkPurposeExistance(string assessmentPurposeId, DataTable assessmentPurposeTable, string action)
        {
            log.Debug("webFrmAssessment : checkPurposeExistance()");

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

        protected void clearPurposeControls()
        {
            btnAddPurpose.Text = "Add";
            Utils.clearControls(true, ddlAssessmentPurpose, ddlPurposeStatus);
            ddlAssessmentPurpose.Enabled = true;
        }

        protected void btnClearPurpose_Click(object sender, EventArgs e)
        {
            clearPurposeControls();
            Utility.Errorhandler.ClearError(lblErrorMsg);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("webFrmAssessment : btnSave_Click()");

            AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();

            try
            {
                DataTable purposeDataTable = (DataTable)Session["AssessmentPurposeSession"];

                string activeStatus = "Active";
                DataRow[] activePurposes = purposeDataTable.Select("STATUS_CODE = '" + activeStatus + "'");
                bool allInactivePurposes = true;
                if (activePurposes.Count() > 0)
                {
                    allInactivePurposes = false;
                }


                excludeEmployees();
                DataTable assessedEmployees = (DataTable)Session["AddedEmployees"];

                string assessmentName = txtName.Text.ToString().Trim();
                string assessmentType = ddlAssessmentType.SelectedValue.ToString();
                string cutOffDate = txtCutOffDate.Text.ToString();

                int result = DateTime.Compare(Convert.ToDateTime(cutOffDate), DateTime.Now);
                if (result < 0)
                {
                    CommonVariables.MESSAGE_TEXT = "Invalid cutoff date.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                    return;
                }

                string remarks = txtRemarks.Text.ToString().Trim();
                //string year = lblYearOfAssessment.Text.ToString();
                string year = ddlYear.SelectedValue.ToString();
                string status = ddlStatus.SelectedValue.ToString();
                string addedUserId = Session["KeyUSER_ID"].ToString();
                //string companyId = hiddenSelectedCompanyId.Value.ToString();
                //Session["AddeddEmployeeCompanyId"] = hiddenSelectedCompanyId.Value.ToString();
                string companyId = Session["AddeddEmployeeCompanyId"].ToString();

                if (purposeDataTable.Rows.Count > 0 && assessedEmployees.Rows.Count > 0 && allInactivePurposes == false)
                {
                    if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                    {
                        if (status == Constants.ASSESSNEMT_PENDING_STATUS)
                        {
                            if (year == getFinancialYear())
                            {
                                //AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
                                Boolean nameIsExsists = assessmentDataHandler.checkAssessmentNameExistance(assessmentName);
                                if (nameIsExsists)
                                {
                                    CommonVariables.MESSAGE_TEXT = "Assessment name already exist";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                                }
                                else
                                {
                                    bool isInserted = assessmentDataHandler.insert(purposeDataTable, assessedEmployees, assessmentName, assessmentType, remarks, year, status, addedUserId, companyId, cutOffDate);
                                    if (isInserted)
                                    {
                                        //Response.Redirect(Request.RawUrl);
                                        reloadEmployeesSessionWithEmptyDataTable();
                                        reloadPurposeSessionWithEmptyDataTable();
                                        clearAllControls();
                                        CommonVariables.MESSAGE_TEXT = "Record(s) successfully saved.";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);

                                    }
                                    else
                                    {
                                        CommonVariables.MESSAGE_TEXT = "Record(s) could not be saved.";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                                    }
                                }
                            }
                            else
                            {
                                CommonVariables.MESSAGE_TEXT = " Year of assessment should be currrent financial year";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                            }
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = " Initial status of an assessment should be 'Pending'";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                        }

                        loadAssessmentGridView();

                    }
                    else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {
                        string assessmentId = hiddenAssessmentId.Value.ToString();
                        string prevStatus = hiddenPreviouseStatus.Value.ToString();

                        if (hiddenPreviouseAssessmentYear.Value.ToString() == getFinancialYear() && getFinancialYear() == year)
                        {
                            //AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();

                            //if (prevStatus == Constants.ASSESSNEMT_PENDING_STATUS)
                            //{

                            Boolean nameIsExsists = assessmentDataHandler.checkAssessmentNameExistance(assessmentName, assessmentId);
                            if (nameIsExsists)
                            {
                                CommonVariables.MESSAGE_TEXT = "Assessment name already exist";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                            }
                            else
                            {
                                DataRow assessmentTypeDetail = assessmentDataHandler.getAssessmentTypeById(assessmentType);
                                if (assessmentTypeDetail[1].ToString() == Constants.STATUS_ACTIVE_VALUE)
                                {
                                    if (status != Constants.ASSESSNEMT_CLOSED_STATUS)
                                    {
                                        bool isUpdated = assessmentDataHandler.update(assessmentId, purposeDataTable, assessedEmployees, assessmentName, assessmentType, remarks, year, status, addedUserId, companyId, cutOffDate);

                                        if (isUpdated)
                                        {
                                            //Session["AddedEmployees"] = null;
                                            reloadEmployeesSessionWithEmptyDataTable();
                                            reloadPurposeSessionWithEmptyDataTable();
                                            clearAllControls(); ;
                                            CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated.";
                                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                                        }
                                    }
                                    else
                                    {
                                        CommonVariables.MESSAGE_TEXT = "Cannot update a closed assessment";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                                    }
                                }
                                else
                                {

                                    CommonVariables.MESSAGE_TEXT = "Selected assessment type is inactive";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                                }

                            }
                            //}

                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Year of assessment does not match to current financial year";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                        }


                        loadAssessmentGridView();
                    }
                }
                else
                {
                    //Session["ChildWindowGridData"] = null;
                    //Session["AddedEmployees"] = null;

                    //reloadEmployeesSessionWithEmptyDataTable();
                    //string assessmentId = hiddenAssessmentId.Value.ToString();

                    //AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
                    //DataTable assessedEmployeesDataTable = assessmentDataHandler.getEmployeesForAssessment(assessmentId);
                    //DataTable selectedEmployees = new DataTable();
                    //selectedEmployees = (DataTable)Session["AddedEmployees"];

                    //DataTable allEmployees = new DataTable();

                    //Session["ChildWindowGridData"] = assessedEmployeesDataTable;
                    //allEmployees = (DataTable)Session["ChildWindowGridData"];

                    //foreach (DataRow employee in allEmployees.Rows)
                    //{
                    //    string include = employee["INCLUDE"].ToString();
                    //    if (include == "1")
                    //    {
                    //        DataRow[] existingEntry = selectedEmployees.Select("emp_id ='" + employee["EMPLOYEE_ID"].ToString() + "'");
                    //        if (existingEntry.Count() == 0)
                    //        {
                    //            DataRow newEmployee = selectedEmployees.NewRow();
                    //            newEmployee["emp_id"] = employee["EMPLOYEE_ID"].ToString();
                    //            newEmployee["emp_name"] = employee["KNOWN_NAME"].ToString();
                    //            newEmployee["epf_no"] = employee["EPF_NO"].ToString();
                    //            newEmployee["exclude"] = "0";
                    //            newEmployee["goal"] = employee["GOAL"].ToString();
                    //            newEmployee["competency"] = employee["COMPETENCY"].ToString();
                    //            newEmployee["self"] = employee["SELF"].ToString();
                    //            newEmployee["company_id"] = employee["COMPANY_ID"].ToString();
                    //            newEmployee["dept_id"] = employee["DEPT_ID"].ToString();
                    //            newEmployee["division_id"] = employee["DIVISION_ID"].ToString();
                    //            newEmployee["report_to_1"] = employee["REPORT_TO_1"].ToString();
                    //            newEmployee["role"] = employee["ROLE"].ToString();
                    //            newEmployee["designation_id"] = employee["DESIGNATION_ID"].ToString();

                    //            selectedEmployees.Rows.Add(newEmployee);
                    //        }
                    //        ///
                    //        /// adding data To main window
                    //        ///
                    //        //EmployeeDataTable empDataTbl = new EmployeeDataTable();
                    //        //empDataTbl.selectedEmployeeGridAvailable = false;
                    //        //empDataTbl = null;

                    //    }
                    //}

                    //loadSelectedEmployeesGridView();

                    //CommonVariables.MESSAGE_TEXT = "Assessed Employees and Purposes are required";
                    //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);

                    if (purposeDataTable.Rows.Count == 0 && assessedEmployees.Rows.Count > 0)
                    {
                        CommonVariables.MESSAGE_TEXT = "Assessment Purposes are required";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                    }
                    if (purposeDataTable.Rows.Count > 0 && assessedEmployees.Rows.Count == 0)
                    {
                        CommonVariables.MESSAGE_TEXT = "Assessed employees are required";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                    }
                    if (purposeDataTable.Rows.Count == 0 && assessedEmployees.Rows.Count == 0)
                    {
                        CommonVariables.MESSAGE_TEXT = "Assessed Employees and Purposes are required";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                    }
                    if (allInactivePurposes == true)
                    {
                        CommonVariables.MESSAGE_TEXT = "Atleast one active purpose is required";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                    }
                }
                purposeDataTable.Dispose();
                assessedEmployees.Dispose();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
            }
            finally
            {
                assessmentDataHandler = null;
            }
	    }
	

        string getFinancialYear()
        {
            log.Debug("webFrmAssessment : getFinancialYear()");

            ////finYearStrtDate
            
                System.DateTime dtfin = System.DateTime.Now;

                int CurrentFinyear = 0;
                string CurrentFinYearDetails = String.Empty;

                DateTime finDate = DateTime.ParseExact(dtfin.Year + "-03-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                if (finDate >= System.DateTime.Now)
                {
                    CurrentFinyear = dtfin.AddYears(-1).Year;
                    CurrentFinYearDetails = " (From March 31, " + CurrentFinyear.ToString() + " To April 1, " + finDate.Year + ")";

                    //Session["FinYear"] = CurrentFinyear.ToString();

                    //return CurrentFinyear.ToString() + CurrentFinYearDetails;
                    //lblYearOfAssessment.Text = CurrentFinyear.ToString();
                    return CurrentFinyear.ToString();
                }
                else
                {
                    System.DateTime dt = System.DateTime.Now;
                    System.DateTime dtDetais = System.DateTime.Now;

                    string finYearDetails = " (From March 31, " + dt.Year.ToString() + " To April 1, " + dtDetais.AddYears(1).Year + ")";

                    //Session["FinYear"] = dt.Year.ToString();

                    //return dt.Year.ToString() + Environment.NewLine + finYearDetails;
                    //lblYearOfAssessment.Text = dt.Year.ToString();
                    return dt.Year.ToString();
                }
            
            

        }

        private void fillStatus()
        {
            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlStatus.Items.Add(listItemBlank);

                //ListItem listItemActive = new ListItem();
                //listItemActive.Text = Constants.ASSESSNEMT_ACTIVE_TAG;
                //listItemActive.Value = Constants.ASSESSNEMT_ACTIVE_STATUS;
                //ddlStatus.Items.Add(listItemActive);

                ListItem listItemPending = new ListItem();
                listItemPending.Text = Constants.ASSESSNEMT_PENDING_TAG;
                listItemPending.Value = Constants.ASSESSNEMT_PENDING_STATUS;
                ddlStatus.Items.Add(listItemPending);

                ListItem listItemObsolete = new ListItem();
                listItemObsolete.Text = Constants.ASSESSNEMT_OBSOLETE_TAG;
                listItemObsolete.Value = Constants.ASSESSNEMT_OBSOLETE_STATUS;
                ddlStatus.Items.Add(listItemObsolete);

                //ListItem listItemSubordinateFinalized = new ListItem();
                //listItemSubordinateFinalized.Text = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_TAG;
                //listItemSubordinateFinalized.Value = Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS;
                //ddlStatus.Items.Add(listItemSubordinateFinalized);

                //ListItem listItemSubordinateDisagree = new ListItem();
                //listItemSubordinateDisagree.Text = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_TAG;
                //listItemSubordinateDisagree.Value = Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS;
                //ddlStatus.Items.Add(listItemSubordinateDisagree);

                //ListItem listItemSupervisorFinalized = new ListItem();
                //listItemSupervisorFinalized.Text = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_TAG;
                //listItemSupervisorFinalized.Value = Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS;
                //ddlStatus.Items.Add(listItemSupervisorFinalized);

                //ListItem listItemCEOFinalized = new ListItem();
                //listItemCEOFinalized.Text = Constants.ASSESSNEMT_CEO_FINALIZED_TAG;
                //listItemCEOFinalized.Value = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                //ddlStatus.Items.Add(listItemCEOFinalized);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

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
            log.Debug("webFrmAssessment : loadSelectedEmployeesGridView()");

            DataTable selectedEmployees1 = new DataTable();
            try
            {
                selectedEmployees1 = (Session["AddedEmployees"] as DataTable).Copy();

                //DataView view = selectedEmployees1.DefaultView;
                //view.Sort = "EPF_NO ASC";
                //DataTable sortedTable = view.ToTable();

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
                //EmployeeDataTable empDataTbl = new EmployeeDataTable();
                //empDataTbl.selectedEmployeeGridAvailable = true;
                //empDataTbl = null;
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                selectedEmployees1.Dispose();
            }
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
                selectedEmployees.Dispose();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                selectedEmployees.Dispose();
            }
        }

        protected void excludeHeaderCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("excludeHeaderCheckBox_OnCheckedChanged()");

            try
            {
                CheckBox selectAll = (CheckBox)GridViewSelectedEmployees.HeaderRow.FindControl("excludeHeaderCheckBox");
                if (selectAll.Checked == true)
                {
                    DataTable addedEmployees = new DataTable();
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
                    DataTable addedEmployees = new DataTable();
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
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
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
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally {
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
                    int dataTableIndex = index + (pageNo) * 20;

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
                        }
                    }
                    else if (goal == "0")
                    {
                        CheckBox goalChild = (e.Row.FindControl("goalChildCheckBox") as CheckBox);
                        goalChild.Checked = false;
                    }

                    if (self == "1")
                    {
                        string empId = e.Row.Cells[0].Text;
                        CheckBox selfChild;
                        if (empId == emId)
                        {
                            selfChild = (e.Row.FindControl("selfChildCheckBox") as CheckBox);
                            selfChild.Checked = true;
                        }
                    }
                    else if (self == "0")
                    {
                        CheckBox selfChild = (e.Row.FindControl("selfChildCheckBox") as CheckBox);
                        selfChild.Checked = false;
                    }

                    if (competency == "1")
                    {
                        string empId = e.Row.Cells[0].Text;
                        CheckBox competencyChild;
                        if (empId == emId)
                        {
                            competencyChild = (e.Row.FindControl("competencyChildCheckBox") as CheckBox);
                            competencyChild.Checked = true;
                        }
                    }
                    else if (competency == "0")
                    {
                        CheckBox competencyChild = (e.Row.FindControl("competencyChildCheckBox") as CheckBox);
                        competencyChild.Checked = false;
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
            try
            {
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
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void btnExclude_Click(object sender, EventArgs e)
        {
            log.Debug("btnExclude_Click()");
            DataTable selectedEmployees = new DataTable();
            try
            {
                selectedEmployees = (DataTable)Session["AddedEmployees"];

                //foreach (DataRow employee in selectedEmployees.Rows)
                //{
                //    if (employee[2].ToString() == "1")
                //    {
                //        selectedEmployees.Rows.Remove(employee);
                //        selectedEmployees.AcceptChanges();
                //    }


                //}

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
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
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
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                selectedEmployees.Dispose();
            }

        }

        protected void clearAllControls()
        {
            log.Debug("clearAllControls()");
            try
            {
                Utils.clearControls(true, txtName, txtRemarks, txtCutOffDate, ddlAssessmentType, ddlAssessmentPurpose, ddlPurposeStatus, ddlStatus, ddlCompany, ddlYear);
                Utility.Errorhandler.ClearError(lblErrorMsg);
                Utility.Errorhandler.ClearError(lblErrorMsg2);
                PurposeGridView.DataSource = null;
                PurposeGridView.DataBind();

                GridViewSelectedEmployees.DataSource = null;
                GridViewSelectedEmployees.DataBind();
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                btnAddPurpose.Text = "Add";

                ddlStatus.Items.Clear();
                fillStatus();



                enableFields();
                loadAssessmentGridView();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");
            hiddenAssessmentId.Value = null;
            Session["ChildWindowGridData"] = null;
            Session["AddedEmployees"] = null;
            reloadEmployeesSessionWithEmptyDataTable();
            reloadPurposeSessionWithEmptyDataTable();
            clearAllControls();

            ddlAssessmentType.Items.Clear();
            loadAssessmentTypeDropDown();

            //Session["AddeddEmployeeCompanyId"] = null;            
        }

        protected void reloadEmployeesSessionWithEmptyDataTable()
        {
            log.Debug("reloadEmployeesSessionWithEmptyDataTable()");

            try
            {
                Session.Remove("ChildWindowGridData");
                Session.Remove("AddedEmployees");

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
                AddeddEmloyeeDataTable.Columns.Add("report_to_1", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("role", typeof(String));
                AddeddEmloyeeDataTable.Columns.Add("designation_id", typeof(String));

                Session["AddedEmployees"] = AddeddEmloyeeDataTable;
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void reloadPurposeSessionWithEmptyDataTable()
        {
            log.Debug("reloadPurposeSessionWithEmptyDataTable()");

            try
            {
                Session.Remove("AssessmentPurposeSession");
                DataTable assessmentPurposesDataTable = new DataTable();
                assessmentPurposesDataTable.Columns.Add("INDEX");
                assessmentPurposesDataTable.Columns.Add("PURPOSE_ID");
                assessmentPurposesDataTable.Columns.Add("NAME");
                //assessmentPurposesDataTable.Columns.Add("DESCRIPTION");
                assessmentPurposesDataTable.Columns.Add("STATUS_CODE");

                Session["AssessmentPurposeSession"] = assessmentPurposesDataTable;
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void goalHeaderCheckBox_OnCheckedChanged(Object sender, EventArgs e) 
        {
            log.Debug("goalHeaderCheckBox_OnCheckedChanged()");

            try
            {
                CheckBox selectAll = (CheckBox)GridViewSelectedEmployees.HeaderRow.FindControl("goalHeaderCheckBox");
                if (selectAll.Checked == true)
                {
                    DataTable addedEmployees = new DataTable();
                    addedEmployees = (DataTable)Session["AddedEmployees"];
                    foreach (DataRow employee in addedEmployees.Rows)
                    {
                        bool isGoalAvailable = checkGoalAvailabilityForEmployee(employee[0].ToString());
                        if (isGoalAvailable)
                        {
                            employee["goal"] = "1";
                        }
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
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }

        }

        protected void goalChildCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("goalChildCheckBox_OnCheckedChanged()");

            DataTable selectedEmployees = new DataTable();
            try
            {
                CheckBox goalChildCheck = (CheckBox)sender;
                GridViewRow gvr = (GridViewRow)goalChildCheck.NamingContainer;
                //string id = employeeGridView.Rows[gvr.RowIndex].ToString();
                string employeeId = GridViewSelectedEmployees.Rows[gvr.RowIndex].Cells[0].Text;

                bool isGoalAvailable = checkGoalAvailabilityForEmployee(employeeId);

                selectedEmployees = (DataTable)Session["AddedEmployees"];

                DataRow[] employee = selectedEmployees.Select("emp_id ='" + employeeId + "'");
                if (goalChildCheck.Checked)
                {
                    if (isGoalAvailable)
                    {
                        employee[0]["goal"] = "1";
                    }
                    else
                    {
                        //CommonVariables.MESSAGE_TEXT = " No goals assigned for the employee";
                        //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                    }
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
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                selectedEmployees.Dispose();
            }
        }

        protected void competencyHeaderCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("competencyHeaderCheckBox_OnCheckedChanged()");

            try
            {
                CheckBox selectAll = (CheckBox)GridViewSelectedEmployees.HeaderRow.FindControl("competencyHeaderCheckBox");
                if (selectAll.Checked == true)
                {
                    DataTable addedEmployees = new DataTable();
                    addedEmployees = (DataTable)Session["AddedEmployees"];
                    foreach (DataRow employee in addedEmployees.Rows)
                    {
                        bool activeCompetencyProfileAvailable = checkCompetencyProfileAvailabilityForEmployee(employee[0].ToString());

                        if (activeCompetencyProfileAvailable == true)
                        {

                            employee["competency"] = "1";
                            
                        }
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
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }

        }

        protected void competencyChildCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("competencyChildCheckBox_OnCheckedChanged()");
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
                    bool activeCompetencyProfileAvailable = checkCompetencyProfileAvailabilityForEmployee(employeeId);

                    if (activeCompetencyProfileAvailable == true)
                    {
                        employee[0]["competency"] = "1";
                    }
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
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                selectedEmployees.Dispose();
            }
        }

        protected void selfHeaderCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("selfHeaderCheckBox_OnCheckedChanged()"); 

            try
            {
                CheckBox selectAll = (CheckBox)GridViewSelectedEmployees.HeaderRow.FindControl("selfHeaderCheckBox");
                if (selectAll.Checked == true)
                {
                    DataTable addedEmployees = new DataTable();
                    addedEmployees = (DataTable)Session["AddedEmployees"];
                    foreach (DataRow employee in addedEmployees.Rows)
                    {
                        bool selfAssessmentProfileAvailable = checkSelfAssessmentProfileAvailabilityForEmployee(employee[0].ToString());
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
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
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
                    bool selfAssessmentProfileAvailable = checkSelfAssessmentProfileAvailabilityForEmployee(employeeId);
                    if (selfAssessmentProfileAvailable == true)
                    {
                        employee[0]["self"] = "1";
                    }
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
                
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
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
            AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
            DataTable companyDataTable = assessmentDataHandler.getAllActiveCompanies();

            try
            {
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
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                companyDataTable = null;
                assessmentDataHandler = null;
            }
        }

        protected void fillCompanyDropDown(string companyId)
        {
            log.Debug("fillCompanyDropDown()");

            AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
            DataTable companyDataTable = new DataTable();

            try
            {
                companyDataTable = assessmentDataHandler.getCompanyById(companyId);
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                //ddlSearchCompany.Items.Add(listItemBlank);
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

                //ddlSearchCompany.SelectedValue = companyId;
                ddlCompany.SelectedValue = companyId;
                Session["AddeddEmployeeCompanyId"] = companyId;
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                companyDataTable = null;
                assessmentDataHandler = null;
            }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlCompany_SelectedIndexChanged()");

            loadAssessmentGridView();
            Session["AddeddEmployeeCompanyId"] = ddlCompany.SelectedValue.ToString();
            Session.Remove("AddedEmployees");
            reloadEmployeesSessionWithEmptyDataTable();
            GridViewSelectedEmployees.DataSource = (Session["AddedEmployees"] as DataTable).Copy();
            GridViewSelectedEmployees.DataBind();

        }

        protected void fillYearDropdown()
        {
            log.Debug("fillYearDropdown()");
            AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
            DataTable yearDataTable = new DataTable();

            try
            {
                yearDataTable = assessmentDataHandler.getDistinctYears();
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
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                assessmentDataHandler = null;                 
                yearDataTable.Dispose();
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlYear_SelectedIndexChanged()");
            loadAssessmentGridView();
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlStatus_SelectedIndexChanged()");
            loadAssessmentGridView();
        }

        private bool checkGoalAvailabilityForEmployee(string employeeId)
        {
            log.Debug("checkGoalAvailabilityForEmployee()");
            //DataTable dtFinalizedGoal = new DataTable();
            AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
            try
            {
                string year = CommonUtils.currentFinancialYear();
                bool isAvailable = assessmentDataHandler.checkFinalizedGoalAvailability(employeeId, year);
                return isAvailable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                assessmentDataHandler = null;
            }
        }

        private bool checkCompetencyProfileAvailabilityForEmployee(string employeeId)
        {
            log.Debug("checkCompetencyProfileAvailabilityForEmployee()");
            AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
            try
            {
                bool isAvailable = assessmentDataHandler.checkCompetencyProfileAvailability(employeeId);
                return isAvailable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                assessmentDataHandler = null;
            }
        }

        private bool checkSelfAssessmentProfileAvailabilityForEmployee(string employeeId)
        {
            log.Debug("checkSelfAssessmentProfileAvailabilityForEmployee()");
            AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
            try
            {
                bool isAvailable = assessmentDataHandler.checkSelfAssessmentProfileAvailability(employeeId);
                return isAvailable;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                assessmentDataHandler = null;
            }
        }
    }
}