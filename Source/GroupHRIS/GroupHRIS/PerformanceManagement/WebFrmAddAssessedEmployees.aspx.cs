using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.PerformanceManagement;
using System.Data;
using DataHandler.Employee;
using Common;
using NLog;



namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmAddAssessedEmployees : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        //EmployeeDataTable empClass = new EmployeeDataTable();
        public int selectedPage { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmAddAssessedEmployees : Page_Load");
            try
            {
                if ((Session["KeyLOGOUT_STS"] == null)|| (Session["KeyLOGOUT_STS"].Equals("0")))
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
                btnAdd.Visible = false;
                //loadCompanyDropDown();
                selectedPage = employeeGridView.PageIndex;

                if (!IsPostBack)
                {
                    if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                    {
                        string comId = Session["KeyCOMP_ID"].ToString();
                        if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                        {
                            //loadCompanyDropDown();
                            loadDepartmentListForSelectedCompanyDropDown(Session["AddeddEmployeeCompanyId"].ToString());
                        }
                        else
                        {
                            //loadCompanyDropDown(Session["KeyCOMP_ID"].ToString().Trim());
                            loadDepartmentListForSelectedCompanyDropDown(Session["AddeddEmployeeCompanyId"].ToString());
                        }
                        setSelectedCompanyName(Session["AddeddEmployeeCompanyId"].ToString());
                    }
                    getEmployees();
                    btnAdd.Visible = true;
                }
            }

            
            

        }

        //protected void loadCompanyDropDown()
        //{
        //    AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
        //    DataTable companyDataTable = assessmentDataHandler.getAllActiveCompanies();

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

        //    string comId = Session["AddeddEmployeeCompanyId"].ToString();
        //    if (!String.IsNullOrEmpty(comId))
        //    {
        //        ddlCompany.SelectedValue = Session["AddeddEmployeeCompanyId"].ToString();
        //        getEmployees();
        //        btnAdd.Visible = true;
        //    }
            

        //    companyDataTable = null;
        //    assessmentDataHandler = null;
        //}

        //protected void loadCompanyDropDown(string companyId)
        //{
        //    AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
        //    DataTable companyDataTable = assessmentDataHandler.getCompanyById(companyId);

        //    //ListItem listItemSelectCompany = new ListItem();
        //    //listItemSelectCompany.Text = "Select Company";
        //    //listItemSelectCompany.Value = "";
        //    //ddlCompany.Items.Add(listItemSelectCompany);

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

        protected void setSelectedCompanyName(string selectedCompanyId)
        {
            log.Debug("setSelectedCompanyName()"); 
            AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
            DataTable dataRowCompanyName = new DataTable();
            try
            {                
                dataRowCompanyName = assessmentDataHandler.getCompanyById(selectedCompanyId);
                lblCompanyName.Text = dataRowCompanyName.Rows[0][1].ToString();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                assessmentDataHandler = null;
                dataRowCompanyName.Dispose();
            }
        }

        protected void loadDepartmentListForSelectedCompanyDropDown(string selectedCompanyId)
        {
            log.Debug("loadDepartmentListForSelectedCompanyDropDown()");
            AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
            DataTable departmentDataTable = new DataTable();

            //ListItem listItemSelectDepartment = new ListItem();
            //listItemSelectDepartment.Text = "Select Department";
            //listItemSelectDepartment.Value = "";
            //ddlDepartment.Items.Add(listItemSelectDepartment);

            try
            {
                departmentDataTable = assessmentDataHandler.getActiveDepartmentsForCompany(selectedCompanyId);

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlDepartment.Items.Add(listItemBlank);

                foreach (DataRow department in departmentDataTable.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = department[1].ToString();
                    listItem.Value = department[0].ToString();
                    ddlDepartment.Items.Add(listItem);
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                departmentDataTable = null;
                assessmentDataHandler = null;
            }
        }

        protected void loadDevisionListForSelectedDepartmentDropDown(string selectedDepartmentId)
        {
            log.Debug("loadDevisionListForSelectedDepartmentDropDown()");
            AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
            DataTable devisionDataTable = new DataTable();

            try
            {
                //ListItem listItemSelectDevision = new ListItem();
                //listItemSelectDevision.Text = "Select Devision";
                //listItemSelectDevision.Value = "";
                //ddlDevision.Items.Add(listItemSelectDevision);
                devisionDataTable = assessmentDataHandler.getActiveDevisionsForDepartment(selectedDepartmentId);

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlDevision.Items.Add(listItemBlank);

                foreach (DataRow devision in devisionDataTable.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = devision[1].ToString();
                    listItem.Value = devision[0].ToString();
                    ddlDevision.Items.Add(listItem);
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                devisionDataTable = null;
                assessmentDataHandler = null;
            }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlCompany_SelectedIndexChanged()");
            try
            {
                ddlDepartment.Items.Clear();
                ddlDevision.Items.Clear();
                //string selectedCompanyId = ddlCompany.SelectedValue.ToString().Trim();
                //loadDepartmentListForSelectedCompanyDropDown(selectedCompanyId);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlDepartment_SelectedIndexChanged()");
            try
            {
                ddlDevision.Items.Clear();
                string selectedDepartmentId = ddlDepartment.SelectedValue.ToString().Trim();
                loadDevisionListForSelectedDepartmentDropDown(selectedDepartmentId);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            log.Debug("imgbtnSearch_Click()");
            try
            {
                addIncludedEmployeesToAddeddEmployeeSession();
                getEmployees();
                btnAdd.Visible = true;
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void getEmployees()
        {
            log.Debug("getEmployees()");
            EmployeeSearchDataHandler employeeSearchDataHandler = new EmployeeSearchDataHandler();
            AssessmentDataHandler assessmentDataHandler = new AssessmentDataHandler();
            DataTable employeeDataTable = new DataTable();

            try
            {
                //string companyId = ddlCompany.SelectedValue.ToString().Trim();
                string companyId = Session["AddeddEmployeeCompanyId"].ToString();
                string departmentId = ddlDepartment.SelectedValue.ToString().Trim();
                string divisionId = ddlDevision.SelectedValue.ToString().Trim();
                string epfNo = txtEpf.Text.ToString();

                string sStatus = "";
                string sName = "";
                string sDesignationId = "";


                employeeDataTable.Columns.Add("Goal", typeof(String));

                if (!String.IsNullOrEmpty(epfNo))
                {
                    employeeDataTable = assessmentDataHandler.populateByEPF(epfNo);
                    //DataView view = employeeDataTable.DefaultView;
                    //view.Sort = "EPF_NO ASC";
                    //DataTable sortedDataTable = view.ToTable();
                    //addAllEmployeesToSession(sortedDataTable);
                    addAllEmployeesToSession(employeeDataTable);
                }
                else
                {
                    if (!String.IsNullOrEmpty(companyId) && !String.IsNullOrEmpty(departmentId) && !String.IsNullOrEmpty(divisionId))
                    {
                        //////// used when company , department and division available
                        employeeDataTable = assessmentDataHandler.populate(companyId, departmentId, divisionId, sStatus, sName, sDesignationId);
                        //DataView view = employeeDataTable.DefaultView;
                        //view.Sort = "EPF_NO ASC";
                        //DataTable sortedDataTable = view.ToTable();
                        //addAllEmployeesToSession(sortedDataTable);
                        addAllEmployeesToSession(employeeDataTable);
                    }
                    if (!String.IsNullOrEmpty(companyId) && !String.IsNullOrEmpty(departmentId) && String.IsNullOrEmpty(divisionId))
                    {
                        //////// used when company and department available
                        employeeDataTable = assessmentDataHandler.populate(companyId, departmentId, sStatus, sName, sDesignationId);
                        //DataView view = employeeDataTable.DefaultView;
                        //view.Sort = "EPF_NO ASC";
                        //DataTable sortedDataTable = view.ToTable();
                        //addAllEmployeesToSession(sortedDataTable);
                        addAllEmployeesToSession(employeeDataTable);

                    }
                    if (!String.IsNullOrEmpty(companyId) && String.IsNullOrEmpty(departmentId) && String.IsNullOrEmpty(divisionId))
                    {
                        //////// used when only company is available
                        employeeDataTable = assessmentDataHandler.populate(companyId, sStatus, sName, sDesignationId);
                        //DataView view = employeeDataTable.DefaultView;
                        //view.Sort = "EPF_NO ASC";
                        //DataTable sortedDataTable = view.ToTable();
                        //addAllEmployeesToSession(employeeDataTable);
                        addAllEmployeesToSession(employeeDataTable);

                    }
                    if (String.IsNullOrEmpty(companyId) && String.IsNullOrEmpty(departmentId) && String.IsNullOrEmpty(divisionId) && String.IsNullOrEmpty(epfNo))
                    {
                        employeeDataTable = assessmentDataHandler.populate(sStatus, sName, sDesignationId);
                        //DataView view = employeeDataTable.DefaultView;
                        //view.Sort = "EPF_NO ASC";
                        //DataTable sortedDataTable = view.ToTable();
                        //addAllEmployeesToSession(sortedDataTable);
                        addAllEmployeesToSession(employeeDataTable);

                    }
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                 employeeSearchDataHandler = null;
                 assessmentDataHandler = null;
                 employeeDataTable.Dispose();
            }

        }

        //protected void getAppraserId(string )

        protected void addAllEmployeesToSession(DataTable employees)
        {
            log.Debug("addAllEmployeesToSession()");

            DataTable existingEmployees = new DataTable();
            DataTable allEmployees = new DataTable();

            try
            {
                existingEmployees = (Session["AddedEmployees"] as DataTable).Copy();

                allEmployees.Columns.Add("EMPLOYEE_ID", typeof(String));
                allEmployees.Columns.Add("EPF_NO", typeof(String));
                allEmployees.Columns.Add("TITLE", typeof(String));
                allEmployees.Columns.Add("KNOWN_NAME", typeof(String));
                allEmployees.Columns.Add("GOAL", typeof(String));
                allEmployees.Columns.Add("COMPETENCY", typeof(String));
                allEmployees.Columns.Add("SELF", typeof(String));
                allEmployees.Columns.Add("INCLUDE", typeof(String));
                allEmployees.Columns.Add("COMPANY_ID", typeof(String));
                allEmployees.Columns.Add("DEPT_ID", typeof(String));
                allEmployees.Columns.Add("DIVISION_ID", typeof(String));
                allEmployees.Columns.Add("REPORT_TO_1", typeof(String));
                allEmployees.Columns.Add("ROLE", typeof(String));
                allEmployees.Columns.Add("DESIGNATION_ID", typeof(String));

                foreach (DataRow employee in employees.Rows)
                {
                    DataRow[] existingEmployee = existingEmployees.Select("emp_id ='" + employee["EMPLOYEE_ID"].ToString() + "'");

                    DataRow newRow = allEmployees.NewRow();
                    newRow["EMPLOYEE_ID"] = employee["EMPLOYEE_ID"];
                    newRow["EPF_NO"] = employee["EPF_NO"];
                    newRow["TITLE"] = employee["TITLE"];
                    newRow["KNOWN_NAME"] = employee["KNOWN_NAME"];
                    newRow["COMPANY_ID"] = employee["COMPANY_ID"];
                    newRow["DEPT_ID"] = employee["DEPT_ID"];
                    newRow["DIVISION_ID"] = employee["DIVISION_ID"];
                    newRow["REPORT_TO_1"] = employee["REPORT_TO_1"];
                    newRow["ROLE"] = employee["ROLE"];
                    newRow["DESIGNATION_ID"] = employee["DESIGNATION_ID"];

                    if (existingEmployee.Count() > 0)
                    {
                        if (existingEmployee[0]["exclude"].ToString() == "0")
                        {
                            newRow["INCLUDE"] = "1";
                            foreach (DataRow row in existingEmployee)
                            {
                                newRow["SELF"] = row["self"].ToString();
                                newRow["COMPETENCY"] = row["competency"].ToString();
                                newRow["GOAL"] = row["goal"].ToString();

                                if (row["self"].ToString() == "0" && row["competency"].ToString() == "0" && row["goal"].ToString() == "0")
                                {
                                    newRow["INCLUDE"] = "0";
                                    row.Delete();
                                }
                            }
                        }
                        else
                        {
                            newRow["INCLUDE"] = "0";
                            newRow["SELF"] = "0";
                            newRow["COMPETENCY"] = "0";
                            newRow["GOAL"] = "0";
                        }


                        //newRow["SELF"] = "1";
                        //newRow["COMPETENCY"] = "1";
                        //newRow["GOAL"] = "1";
                    }
                    else
                    {
                        newRow["INCLUDE"] = "0";
                        newRow["SELF"] = "0";
                        newRow["COMPETENCY"] = "0";
                        newRow["GOAL"] = "0";
                    }


                    //newRow["EMPLOYEE_ID"] = employee["EMPLOYEE_ID"];

                    allEmployees.Rows.Add(newRow);
                }

                Session["ChildWindowGridData"] = allEmployees;
                Session["AddedEmployees"] = existingEmployees;
                employeeGridView.DataSource = allEmployees;
                employeeGridView.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                 existingEmployees.Dispose();
                 allEmployees.Dispose();
            }
        }

        protected void addIncludedEmployeesToAddeddEmployeeSession()
        {
            log.Debug("addIncludedEmployeesToAddeddEmployeeSession()");
            DataTable selectedEmployees = new DataTable();
            DataTable allEmployees = new DataTable();
            try
            {
                

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

                

                allEmployees = (DataTable)Session["ChildWindowGridData"];

                DataTable prevSelected = (Session["AddedEmployees"] as DataTable).Copy();
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
                }
                if (Session["AddedEmployees"] != null)
                {
                    //DataTable prevSelected = (Session["AddedEmployees"] as DataTable).Copy();


                    selectedEmployees.Merge(prevSelected);
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
                allEmployees.Dispose();
            }
        }

        protected void goalHeaderCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {

            log.Debug("goalHeaderCheckBox_OnCheckedChanged()");

            try
            {
                CheckBox selectAll = ((CheckBox)employeeGridView.HeaderRow.FindControl("goalHeaderCheckBox"));
                if (selectAll.Checked == true)
                {
                    DataTable allEmployees = new DataTable();
                    allEmployees = (DataTable)Session["ChildWindowGridData"];
                    foreach (DataRow employee in allEmployees.Rows)
                    {
                        bool isGoalAvailable = checkGoalAvailabilityForEmployee(employee[0].ToString());
                        if (isGoalAvailable)
                        {
                            employee[4] = "1";
                            employee[7] = "1";
                        }
                    }


                    Session["ChildWindowGridData"] = allEmployees;
                    selectedPage = employeeGridView.PageIndex;
                    employeeGridView.DataSource = allEmployees;
                    employeeGridView.DataBind();

                    allChecked_Include();
                    allEmployees.Dispose();
                }
                else if (selectAll.Checked == false)
                {
                    DataTable allEmployees = new DataTable();
                    allEmployees = (DataTable)Session["ChildWindowGridData"];

                    foreach (DataRow employee in allEmployees.Rows)
                    {
                        employee[4] = "0";
                        if (employee["SELF"].ToString() == "0" && employee["COMPETENCY"].ToString() == "0")
                        {
                            employee["INCLUDE"] = "0";
                        }
                    }


                    Session["ChildWindowGridData"] = allEmployees;
                    selectedPage = employeeGridView.PageIndex;
                    employeeGridView.DataSource = allEmployees;
                    employeeGridView.DataBind();

                    allChecked_Include();
                    allEmployees.Dispose();
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
            DataTable allEmployees = new DataTable();
            try
            {
                Utility.Errorhandler.ClearError(lblErrorMsg);

                CheckBox goalChildCheck = (CheckBox)sender;
                GridViewRow gvr = (GridViewRow)goalChildCheck.NamingContainer;
                //string id = employeeGridView.Rows[gvr.RowIndex].ToString();
                string employeeId = employeeGridView.Rows[gvr.RowIndex].Cells[0].Text;
                bool isGoalAvailable = checkGoalAvailabilityForEmployee(employeeId);

                
                allEmployees = (DataTable)Session["ChildWindowGridData"];

                DataTable addeddEmployees = (Session["AddedEmployees"] as DataTable).Copy();
                DataRow[] addedEmployee = addeddEmployees.Select("emp_id ='" + employeeId + "'");

                DataRow[] employee = allEmployees.Select("EMPLOYEE_ID ='" + employeeId + "'");
                if (goalChildCheck.Checked)
                {
                    if (isGoalAvailable)
                    {
                        employee[0]["GOAL"] = "1";
                        if (addedEmployee.Count() > 0)
                        {
                            addedEmployee[0]["goal"] = "1";
                        }
                        employee[0]["INCLUDE"] = "1";
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = " No goals assigned for the employee";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                }
                else if (goalChildCheck.Checked == false)
                {
                    employee[0]["GOAL"] = "0";
                    if (addedEmployee.Count() > 0)
                    {
                        addedEmployee[0]["goal"] = "0";
                    }

                    if (employee[0]["SELF"] == "0" && employee[0]["COMPETENCY"] == "0")
                    {
                        employee[0]["INCLUDE"] = "0";

                        if (addedEmployee.Count() > 0)
                        {
                            addedEmployee[0]["exclude"] = "1";
                        }
                    }
                }
                Session["ChildWindowGridData"] = allEmployees;
                Session["AddedEmployees"] = addeddEmployees;

                selectedPage = employeeGridView.PageIndex;
                employeeGridView.DataSource = allEmployees;
                employeeGridView.DataBind();

                allChecked_Include();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                 allEmployees.Dispose();
            }
        }
       
        protected void competencyHeaderCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("competencyHeaderCheckBox_OnCheckedChanged()");
            try
            {

                CheckBox selectAll = ((CheckBox)employeeGridView.HeaderRow.FindControl("competencyHeaderCheckBox"));
                if (selectAll.Checked == true)
                {
                    DataTable allEmployees = new DataTable();
                    allEmployees = (DataTable)Session["ChildWindowGridData"];
                    foreach (DataRow employee in allEmployees.Rows)
                    {
                        bool competencyProfileAvailable = checkCompetencyProfileAvailabilityForEmployee(employee[0].ToString());
                        if (competencyProfileAvailable == true)
                        {
                            employee[5] = "1";
                            employee[7] = "1";
                        }
                    }


                    Session["ChildWindowGridData"] = allEmployees;
                    selectedPage = employeeGridView.PageIndex;
                    employeeGridView.DataSource = allEmployees;
                    employeeGridView.DataBind();

                    allChecked_Include();
                    allEmployees.Dispose();
                }
                else if (selectAll.Checked == false)
                {
                    DataTable allEmployees = new DataTable();
                    allEmployees = (DataTable)Session["ChildWindowGridData"];

                    foreach (DataRow employee in allEmployees.Rows)
                    {
                        employee[5] = "0";
                        if (employee["SELF"].ToString() == "0" && employee["GOAL"].ToString() == "0")
                        {
                            employee["INCLUDE"] = "0";
                        }
                    }


                    Session["ChildWindowGridData"] = allEmployees;
                    selectedPage = employeeGridView.PageIndex;
                    employeeGridView.DataSource = allEmployees;
                    employeeGridView.DataBind();

                    allChecked_Include();
                    allEmployees.Dispose();
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
            DataTable allEmployees = new DataTable();
            DataTable addeddEmployees = new DataTable();
            try
            {
                Utility.Errorhandler.ClearError(lblErrorMsg);

                CheckBox competencyChildCheck = (CheckBox)sender;
                GridViewRow gvr = (GridViewRow)competencyChildCheck.NamingContainer;
                //string id = employeeGridView.Rows[gvr.RowIndex].ToString();
                string employeeId = employeeGridView.Rows[gvr.RowIndex].Cells[0].Text;
                

                allEmployees = (DataTable)Session["ChildWindowGridData"];

                addeddEmployees = (Session["AddedEmployees"] as DataTable).Copy();
                DataRow[] addedEmployee = addeddEmployees.Select("emp_id ='" + employeeId + "'");

                DataRow[] employee = allEmployees.Select("EMPLOYEE_ID ='" + employeeId + "'");
                if (competencyChildCheck.Checked)
                {
                    bool competencyProfileAvailable = checkCompetencyProfileAvailabilityForEmployee(employeeId);
                    if (competencyProfileAvailable == true)
                    {
                        employee[0]["COMPETENCY"] = "1";
                        if (addedEmployee.Count() > 0)
                        {
                            addedEmployee[0]["competency"] = "1";
                        }
                        employee[0]["INCLUDE"] = "1"; 
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = " No active competency profile assigned to the employee";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                }
                else if (competencyChildCheck.Checked == false)
                {
                    employee[0]["COMPETENCY"] = "0";
                    if (addedEmployee.Count() > 0)
                    {
                        addedEmployee[0]["competency"] = "0";
                    }
                    if (employee[0]["SELF"] == "0" && employee[0]["GOAL"] == "0")
                    {
                        employee[0]["INCLUDE"] = "0";
                        if (addedEmployee.Count() > 0)
                        {
                            addedEmployee[0]["exclude"] = "1";
                        }

                    }

                }
                Session["ChildWindowGridData"] = allEmployees;
                Session["AddedEmployees"] = addeddEmployees;

                selectedPage = employeeGridView.PageIndex;
                employeeGridView.DataSource = allEmployees;
                employeeGridView.DataBind();

                allChecked_Include();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                allEmployees.Dispose();
                addeddEmployees.Dispose();
            }
        }

        protected void selfHeaderCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("selfHeaderCheckBox_OnCheckedChanged()");

            try
            {
                CheckBox selectAll = ((CheckBox)employeeGridView.HeaderRow.FindControl("selfHeaderCheckBox"));
                if (selectAll.Checked == true)
                {
                    //for (int i = 0; i < employeeGridView.Rows.Count; i++)
                    //{
                    //    ((CheckBox)employeeGridView.Rows[i].Cells[5].FindControl("CheckBox3")).Checked = true;
                    //    //empClass.MyProperty.Rows[i]["Goal"] = "1";
                    //}
                    DataTable allEmployees = new DataTable();
                    allEmployees = (DataTable)Session["ChildWindowGridData"];
                    foreach (DataRow employee in allEmployees.Rows)
                    {
                        bool selfAssessmentProfileAvailable = checkSelfAssessmentProfileAvailabilityForEmployee(employee[0].ToString());

                        if (selfAssessmentProfileAvailable == true)
                        {
                            employee[6] = "1";
                            employee[7] = "1";
                        }
                    }


                    Session["ChildWindowGridData"] = allEmployees;
                    selectedPage = employeeGridView.PageIndex;
                    employeeGridView.DataSource = allEmployees;
                    employeeGridView.DataBind();

                    allChecked_Include();
                    allEmployees.Dispose();
                }
                else if (selectAll.Checked == false)
                {
                    //for (int i = 0; i < employeeGridView.Rows.Count; i++)
                    //{
                    //    ((CheckBox)employeeGridView.Rows[i].Cells[5].FindControl("CheckBox3")).Checked = false;
                    //}

                    DataTable allEmployees = new DataTable();
                    allEmployees = (DataTable)Session["ChildWindowGridData"];

                    foreach (DataRow employee in allEmployees.Rows)
                    {
                        employee[6] = "0";
                        if (employee["GOAL"].ToString() == "0" && employee["COMPETENCY"].ToString() == "0")
                        {
                            employee["INCLUDE"] = "0";
                        }
                    }


                    Session["ChildWindowGridData"] = allEmployees;
                    selectedPage = employeeGridView.PageIndex;
                    employeeGridView.DataSource = allEmployees;
                    employeeGridView.DataBind();

                    allChecked_Include();
                    allEmployees.Dispose();
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
            DataTable allEmployees = new DataTable();
            DataTable addeddEmployees = new DataTable();
            try
            {
                Utility.Errorhandler.ClearError(lblErrorMsg);

                CheckBox selfChildCheck = (CheckBox)sender;
                GridViewRow gvr = (GridViewRow)selfChildCheck.NamingContainer;
                //string id = employeeGridView.Rows[gvr.RowIndex].ToString();
                string employeeId = employeeGridView.Rows[gvr.RowIndex].Cells[0].Text;

                allEmployees = (DataTable)Session["ChildWindowGridData"];

                addeddEmployees = (Session["AddedEmployees"] as DataTable).Copy();
                DataRow[] addedEmployee = addeddEmployees.Select("emp_id ='" + employeeId + "'");

                DataRow[] employee = allEmployees.Select("EMPLOYEE_ID ='" + employeeId + "'");
                if (selfChildCheck.Checked)
                {
                    bool selfAssessmentProfileAvailable = checkSelfAssessmentProfileAvailabilityForEmployee(employeeId);

                    if (selfAssessmentProfileAvailable == true)
                    {
                        employee[0]["SELF"] = "1";
                        if (addedEmployee.Count() > 0)
                        {
                            addedEmployee[0]["self"] = "1";
                        }
                        employee[0]["INCLUDE"] = "1";
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = " No active self assessment profile assigned to the employee";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    }
                }
                else if (selfChildCheck.Checked == false)
                {
                    employee[0]["SELF"] = "0";
                    if (addedEmployee.Count() > 0)
                    {
                        addedEmployee[0]["self"] = "0";
                    }

                    if (employee[0]["GOAL"].ToString() == "0" && employee[0]["COMPETENCY"].ToString() == "0")
                    {
                        employee[0]["INCLUDE"] = "0";
                        if (addedEmployee.Count() > 0)
                        {
                            addedEmployee[0]["exclude"] = "1";
                        }

                    }

                }
                Session["ChildWindowGridData"] = allEmployees;
                Session["AddedEmployees"] = addeddEmployees;
                selectedPage = employeeGridView.PageIndex;
                employeeGridView.DataSource = allEmployees;
                employeeGridView.DataBind();

                allChecked_Include();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            { 
            allEmployees.Dispose();
            addeddEmployees.Dispose();
            }
        }

        protected void includeHeaderCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("includeHeaderCheckBox_OnCheckedChanged()");

            try
            {
                CheckBox selectAll = ((CheckBox)employeeGridView.HeaderRow.FindControl("includeHeaderCheckBox"));
                if (selectAll.Checked == true)
                {
                    DataTable allEmployees = new DataTable();
                    allEmployees = (DataTable)Session["ChildWindowGridData"];

                    foreach (DataRow employee in allEmployees.Rows)
                    {
                        //employee[7] = "1";
                        bool isGoalAvailable = checkGoalAvailabilityForEmployee(employee[0].ToString());
                        if (isGoalAvailable)
                        {
                            employee[7] = "1";
                            employee[4] = "1";
                        }
                        bool competencyProfileAvailable = checkCompetencyProfileAvailabilityForEmployee(employee[0].ToString());
                        if (competencyProfileAvailable == true)
                        {
                            employee[7] = "1";
                            employee[5] = "1";
                        }
                    }


                    Session["ChildWindowGridData"] = allEmployees;
                    selectedPage = employeeGridView.PageIndex;
                    employeeGridView.DataSource = allEmployees;
                    employeeGridView.DataBind();

                    allChecked_Include();
                    allEmployees.Dispose();
                }
                else if (selectAll.Checked == false)
                {

                    for (int i = 0; i < employeeGridView.Rows.Count; i++)
                    {
                        ((CheckBox)employeeGridView.Rows[i].Cells[7].FindControl("includeChildCheckBox")).Checked = false;
                    }
                    DataTable allEmployees = new DataTable();
                    allEmployees = (DataTable)Session["ChildWindowGridData"];

                    foreach (DataRow employee in allEmployees.Rows)
                    {
                        employee[7] = "0";

                        employee[5] = "0";
                        employee[4] = "0";
                        employee[6] = "0";
                    }

                    Session["ChildWindowGridData"] = allEmployees;

                    selectedPage = employeeGridView.PageIndex;
                    employeeGridView.DataSource = allEmployees;
                    employeeGridView.DataBind();

                    allEmployees.Dispose();
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            
        }
        
        protected void includeChildCheckBox_OnCheckedChanged(Object sender, EventArgs e)
        {
            log.Debug("includeChildCheckBox_OnCheckedChanged()");
            DataTable allEmployees = new DataTable();
            DataTable addeddEmployees = new DataTable();
            try
            {
                CheckBox includeChildCheck = (CheckBox)sender;
                GridViewRow gvr = (GridViewRow)includeChildCheck.NamingContainer;
                //string id = employeeGridView.Rows[gvr.RowIndex].ToString();
                string employeeId = employeeGridView.Rows[gvr.RowIndex].Cells[0].Text;


                allEmployees = (DataTable)Session["ChildWindowGridData"];

                DataRow[] employee = allEmployees.Select("EMPLOYEE_ID ='" + employeeId + "'");

                addeddEmployees = (Session["AddedEmployees"] as DataTable).Copy();
                DataRow[] addedEmployee = addeddEmployees.Select("emp_id ='" + employeeId + "'");

                if (includeChildCheck.Checked)
                {
                    

                    bool isGoalAvailable = checkGoalAvailabilityForEmployee(employeeId);
                    if (isGoalAvailable)
                    {
                        employee[0]["INCLUDE"] = "1";
                        employee[0]["GOAL"] = "1";
                    }
                    bool competencyProfileAvailable = checkCompetencyProfileAvailabilityForEmployee(employeeId);
                    if (competencyProfileAvailable == true)
                    {
                        employee[0]["INCLUDE"] = "1";
                        employee[0]["COMPETENCY"] = "1";
                    }
                }
                else if (includeChildCheck.Checked == false)
                {
                    employee[0]["INCLUDE"] = "0";

                    employee[0]["GOAL"] = "0";
                    employee[0]["COMPETENCY"] = "0";
                    employee[0]["SELF"] = "0";
                    if (addedEmployee.Count() > 0)
                    {
                        addedEmployee[0]["exclude"] = "1";
                    }
                }
                Session["ChildWindowGridData"] = allEmployees;
                Session["AddedEmployees"] = addeddEmployees;
                selectedPage = employeeGridView.PageIndex;
                employeeGridView.DataSource = allEmployees;
                employeeGridView.DataBind();

                allChecked_Include();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            { 
            allEmployees.Dispose();
            addeddEmployees.Dispose();
            }
        }

        protected void allChecked_Include()
        {
            log.Debug("allChecked_Include()");
            DataTable allEmployees = new DataTable();
            try
            {
                allEmployees = (DataTable)Session["ChildWindowGridData"];

                Boolean allIncludeChecked = true;
                Boolean allSelfChecked = true;
                Boolean allCompetencyChecked = true;
                Boolean allGoalChecked = true;

                foreach (DataRow employee in allEmployees.Rows)
                {
                    if (employee[7].ToString() == "0")
                    {
                        allIncludeChecked = false;
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

                if (allIncludeChecked == true)
                {
                    CheckBox includeHeader = (CheckBox)employeeGridView.HeaderRow.FindControl("includeHeaderCheckBox");
                    includeHeader.Checked = true;
                }
                if (allSelfChecked == true)
                {
                    CheckBox selfHeader = (CheckBox)employeeGridView.HeaderRow.FindControl("selfHeaderCheckBox");
                    selfHeader.Checked = true;
                }
                if (allCompetencyChecked == true)
                {
                    CheckBox competencyHeader = (CheckBox)employeeGridView.HeaderRow.FindControl("competencyHeaderCheckBox");
                    competencyHeader.Checked = true;
                }
                if (allGoalChecked == true)
                {
                    CheckBox goalHeader = (CheckBox)employeeGridView.HeaderRow.FindControl("goalHeaderCheckBox");
                    goalHeader.Checked = true;
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                allEmployees.Dispose();
            }
        }

        //protected void gridViewCurrentDataSnap()
        //{
        //    //DataTable data = (DataTable)(employeeGridView.DataSource);
            

        //    empClass.MyProperty = (DataTable)(employeeGridView.DataSource);

        //}

        protected void populateGrid()
        {
            //employeeGridView.DataSource = empClass.MyProperty;
            //employeeGridView.DataBind();
        }

        protected void employeeGridView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void employeeGridView_RowDataBound(object sender, GridViewRowEventArgs e)

        {
            log.Debug("employeeGridView_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                   
                    int index = e.Row.RowIndex;

                    DataTable dt1 = new DataTable();
                    dt1=(Session["ChildWindowGridData"] as DataTable).Copy();


                    int pageNo = selectedPage;
                    int dataTableIndex = index + (pageNo) * 10;

                    string include = (Session["ChildWindowGridData"] as DataTable).Rows[dataTableIndex]["INCLUDE"].ToString();
                    string self = (Session["ChildWindowGridData"] as DataTable).Rows[dataTableIndex]["SELF"].ToString();
                    string competency = (Session["ChildWindowGridData"] as DataTable).Rows[dataTableIndex]["COMPETENCY"].ToString();
                    string goal = (Session["ChildWindowGridData"] as DataTable).Rows[dataTableIndex]["GOAL"].ToString();

                    string emId = (Session["ChildWindowGridData"] as DataTable).Rows[dataTableIndex]["EMPLOYEE_ID"].ToString();
                    
                    if (include == "1")
                    {
                        string empId = e.Row.Cells[0].Text;
                        CheckBox includeChild;
                        if (empId == emId)
                        {
                            includeChild = (e.Row.FindControl("includeChildCheckBox") as CheckBox);
                            includeChild.Checked = true;
                        }
                        
                        
                    }
                    else if (include == "0")
                    {
                        CheckBox includeChild = (e.Row.FindControl("includeChildCheckBox") as CheckBox);
                        includeChild.Checked = false;                      
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
                }

            }
            catch (Exception Ex)
            {
                CheckBox includeChild = (e.Row.FindControl("includeChildCheckBox") as CheckBox);
                includeChild.Checked = false;
                
            }
        }

        protected void employeeGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("employeeGridView_PageIndexChanging()");

            //gridViewCurrentDataSnap();
            employeeGridView.PageIndex = e.NewPageIndex;
            selectedPage = employeeGridView.PageIndex;
            //populateGrid();

            //getEmployees();
            employeeGridView.DataSource = null;
            employeeGridView.DataBind();

            employeeGridView.DataSource = (DataTable)Session["ChildWindowGridData"];
            employeeGridView.DataBind();

            allChecked_Include();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            log.Debug("btnAdd_Click()");
            DataTable selectedEmployees = new DataTable();
            

            DataTable allEmployees = new DataTable();
            try
            {
                selectedEmployees = (DataTable)Session["AddedEmployees"];
                allEmployees = (DataTable)Session["ChildWindowGridData"];

                foreach (DataRow employee in allEmployees.Rows)
                {
                    string include = employee["INCLUDE"].ToString();
                    if (include == "1")
                    {
                        DataRow newEmployee = selectedEmployees.NewRow();
                        newEmployee["emp_id"] = employee["EMPLOYEE_ID"].ToString();
                        newEmployee["emp_name"] = employee["KNOWN_NAME"].ToString();
                        newEmployee["exclude"] = "0";
                        selectedEmployees.Rows.Add(newEmployee);
                        ///
                        /// adding data To main window
                        ///
                    }
                }
                //for (int i = 0; i < employeeGridView.Rows.Count; i++)
                //{
                //    DataRow newRow = selectedEmployees.NewRow();
                //    newRow["emp_id"] = employeeGridView.Rows[i].Cells[0];
                //    newRow["emp_name"] = employeeGridView.Rows[i].Cells[2];
                //    newRow["exclude"] = "0";
                //    selectedEmployees.Rows.Add(newRow);
                //}

                Session["AddedEmployees"] = selectedEmployees;
                Session["ChildWindowGridData"] = null;
                //empClass.callParentMethode();
                // if (isSuccess)
                // {
                string close = @"<script type='text/javascript'>
                                window.returnValue = true;
                                window.close();
                                
                                </script>";
                base.Response.Write(close);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                selectedEmployees.Dispose();
                allEmployees.Dispose();
            }
            //}

            //window.opener.location.reload(); 
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