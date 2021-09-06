using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.Employee;
using DataHandler.MetaData;
using Common;
using NLog;

namespace GroupHRIS.Employee
{
    public partial class webFrmEmployeeSearch : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            log.Debug("Page_Load()");

            //string referrer = Request.UrlReferrer.ToString();

            if ((Session["KeyLOGOUT_STS"] == null) || (Session["KeyLOGOUT_STS"].Equals("0")))
            {
                log.Debug("Session Expired");
                //lblerror.Text = "Session Expired. Please log in and try again.";
                Response.Redirect("~/Login/MainLogout.aspx", false);
            }
            else if (!IsPostBack)
            {

                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    //-------------------------------------------------------------------------------------
                    //if current user's company is the global company; show all companies in the drop-down
                    //-------------------------------------------------------------------------------------
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        fillCompanies();
                    }
                    else
                    {
                        fillCompany(Session["KeyCOMP_ID"].ToString().Trim());
                        //ddlCompany.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                    }

                    fillEmployeeStatus();
                    //fillDesignations(String.Empty);
                }
            }

        }



        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlCompany_SelectedIndexChanged()");

            clearDeapartment();
            clearDivision();
            //clearStatus();
            clearEPF();
            clearNIC();
            clearDesgnation();
            clearSearchName();

            if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
            {
                if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                {
                    if (ddlCompany.SelectedValue != "")
                    {
                        fillDepartment(ddlCompany.SelectedValue.Trim());
                        fillDesignations(ddlCompany.SelectedValue.Trim());
                    }
                }
                else
                {
                    fillDepartment(Session["KeyCOMP_ID"].ToString());
                    fillDesignations(Session["KeyCOMP_ID"].ToString());
                }

            }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlDepartment_SelectedIndexChanged()");

            clearDivision();
            //clearStatus();
            clearEPF();
            clearNIC();


            if (ddlDepartment.SelectedValue != "")
            {
                fillDivisions(ddlDepartment.SelectedValue.Trim());
            }
        }


        protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearEPF();
            clearNIC();
        }



        protected void gvEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvEmployee, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lblerror.Text = ex.Message;
            }
        }


        protected void gvEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblEmployee.Visible     = true;
                btnSelect.Visible       = true;

                txtEmployeeNo.Text      = gvEmployee.SelectedRow.Cells[0].Text;
                txtEmpName.Text         = gvEmployee.SelectedRow.Cells[3].Text.removeInvalidHTMLChars();

                hfName.Value            = gvEmployee.SelectedRow.Cells[3].Text.removeInvalidHTMLChars();
                hfCompanyCode.Value     = gvEmployee.SelectedRow.Cells[7].Text.removeInvalidHTMLChars();
                hfCompanyName.Value     = gvEmployee.SelectedRow.Cells[8].Text.removeInvalidHTMLChars();
                hfDepartmentID.Value    = gvEmployee.SelectedRow.Cells[9].Text.removeInvalidHTMLChars();
                hfDepartmentName.Value  = gvEmployee.SelectedRow.Cells[10].Text.removeInvalidHTMLChars();
                hfDivisionID.Value      = gvEmployee.SelectedRow.Cells[11].Text.removeInvalidHTMLChars();
                hfDivisionName.Value    = gvEmployee.SelectedRow.Cells[12].Text.removeInvalidHTMLChars();

                //2014-09-24
                hfBranchID.Value        = gvEmployee.SelectedRow.Cells[13].Text.removeInvalidHTMLChars();
                hfBranchName.Value      = gvEmployee.SelectedRow.Cells[14].Text.removeInvalidHTMLChars();
                hfCC.Value              = gvEmployee.SelectedRow.Cells[15].Text.removeInvalidHTMLChars();
                hfPC.Value              = gvEmployee.SelectedRow.Cells[16].Text.removeInvalidHTMLChars();

                //2015-08-03

                hfEPF.Value = gvEmployee.SelectedRow.Cells[1].Text.removeInvalidHTMLChars();
                hfDesignation.Value = gvEmployee.SelectedRow.Cells[17].Text.removeInvalidHTMLChars();
                hfDesigName.Value = gvEmployee.SelectedRow.Cells[18].Text.removeInvalidHTMLChars();

                // 2016-11-02 Anjana Uduwaragoda
                hfTitle.Value = gvEmployee.SelectedRow.Cells[2].Text.removeInvalidHTMLChars();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lblerror.Text = ex.Message;
            }
        }


        protected void gvEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEmployee.PageIndex = e.NewPageIndex;
            fillEmployees();
        }

        protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            fillEmployees();
        }

        private void clearGridView()
        {
            gvEmployee.DataSource = null;
            gvEmployee.DataBind();
        }


        protected void txtEPFNo_TextChanged(object sender, EventArgs e)
        {
            //clearCompany();
            clearDeapartment();
            clearDivision();
            clearStatus();
            clearDesgnation();
            clearSearchName();
        }

        protected void txtNIC_TextChanged(object sender, EventArgs e)
        {
            clearCompany();
            clearDeapartment();
            clearDivision();
            clearStatus();
            clearDesgnation();
            clearSearchName();
        }


        #endregion

        #region Private Methods
        //------------------------------------------------------------------------------
        ///<summary>
        ///Load all companies 
        ///</summary>
        //------------------------------------------------------------------------------
        private void fillCompanies()
        {
            log.Debug("fillCompanies()");

            CompanyDataHandler dhCompany = new CompanyDataHandler();
            DataTable dtCompanies = new DataTable();

            try
            {
                dtCompanies = dhCompany.getCompanyIdCompName().Copy();

                ddlCompany.Items.Clear();

                if (dtCompanies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlCompany.Items.Add(Item);

                    foreach (DataRow dataRow in dtCompanies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlCompany.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lblerror.Text = ex.Message;
                throw ex;
            }
            finally
            {
                dhCompany = null;
                dtCompanies.Dispose();
            }

        }



        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Load only the company which end user is assigned to 
        ///</summary>
        //----------------------------------------------------------------------------------------
        private void fillCompany(string companyId)
        {
            log.Debug("fillCompanies() - companyId:" + companyId);

            CompanyDataHandler dhCompany = new CompanyDataHandler();
            DataTable dtCompanies = new DataTable();

            try
            {
                dtCompanies = dhCompany.getCompanyIdCompName(companyId).Copy();

                ddlCompany.Items.Clear();

                if (dtCompanies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlCompany.Items.Add(Item);

                    foreach (DataRow dataRow in dtCompanies.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["COMP_NAME"].ToString();
                        listItem.Value = dataRow["COMPANY_ID"].ToString();

                        ddlCompany.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lblerror.Text = ex.Message;
                throw ex;
            }
            finally
            {
                dhCompany = null;
                dtCompanies.Dispose();
            }

        }



        //------------------------------------------------------------------------------
        ///<summary>
        ///Load departments for a given company 
        ///</summary>
        //------------------------------------------------------------------------------
        private void fillDepartment(string companyId)
        {
            log.Debug("fillDepartment() - companyId:" + companyId);

            DepartmentDataHandler dhDepartment = new DepartmentDataHandler();
            DataTable dtDepartments = new DataTable();

            try
            {
                dtDepartments = dhDepartment.getDepartmentIdDeptName(companyId).Copy();

                ddlDepartment.Items.Clear();

                if (dtDepartments.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlDepartment.Items.Add(Item);

                    foreach (DataRow dataRow in dtDepartments.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DEPT_NAME"].ToString();
                        listItem.Value = dataRow["DEPT_ID"].ToString();

                        ddlDepartment.Items.Add(listItem);
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
                dhDepartment = null;
                dtDepartments.Dispose();
            }

        }



        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Load divisions for a given department
        ///</summary>
        //----------------------------------------------------------------------------------------
        private void fillDivisions(string departmentId)
        {
            log.Debug("fillDivisions() - departmentId:" + departmentId);

            DivisionDataHandler dhDivisionr = new DivisionDataHandler();
            DataTable dtDivisions = new DataTable();

            try
            {
                dtDivisions = dhDivisionr.getDivisionIdDivName(departmentId).Copy();

                ddlDivision.Items.Clear();

                if (dtDivisions.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlDivision.Items.Add(Item);

                    foreach (DataRow dataRow in dtDivisions.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DIV_NAME"].ToString();
                        listItem.Value = dataRow["DIVISION_ID"].ToString();

                        ddlDivision.Items.Add(listItem);
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
                dhDivisionr = null;
                dtDivisions.Dispose();
            }

        }



        //------------------------------------------------------------------------------
        ///<summary>
        ///Load Employee Status 
        ///</summary>
        //------------------------------------------------------------------------------
        private void fillEmployeeStatus()
        {
            log.Debug("fillEmployeeStatus():" );

            EmployeeStatusDataHandler dhStatus = new EmployeeStatusDataHandler();
            DataTable dtStatus = new DataTable();

            try
            {
                dtStatus = dhStatus.populate().Copy();

                ddlStatus.Items.Clear();

                if (dtStatus.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlStatus.Items.Add(Item);

                    foreach (DataRow dataRow in dtStatus.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DESCRIPTION"].ToString();
                        listItem.Value = dataRow["STATUS_CODE"].ToString();

                        ddlStatus.Items.Add(listItem);
                    }

                    ddlStatus.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                dhStatus = null;
                dtStatus.Dispose();
            }

        }



        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Load designations
        ///</summary>
        //----------------------------------------------------------------------------------------
        private void fillDesignations(string companyID)
        {
            log.Debug("fillDesignations() - companyID:" + companyID);

            DesignationDataHandler dhDesignation = new DesignationDataHandler();
            DataTable dtDesignation = new DataTable();

            try
            {
                if(companyID.Trim().Length > 0)
                    dtDesignation = dhDesignation.populate(companyID).Copy();
                else
                    dtDesignation = dhDesignation.populate().Copy();


                ddlDesignation.Items.Clear();

                if (dtDesignation.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlDesignation.Items.Add(Item);

                    foreach (DataRow dataRow in dtDesignation.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DESIGNATION_NAME"].ToString();
                        listItem.Value = dataRow["DESIGNATION_ID"].ToString();

                        ddlDesignation.Items.Add(listItem);
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
                dhDesignation = null;
                dtDesignation.Dispose();
            }

        }

        

        //----------------------------------------------------------------------------------------
        ///<summary>
        ///Fill Employee GridView according to the search criteria
        ///</summary>
        //----------------------------------------------------------------------------------------
        private void fillEmployees()
        {

            EmployeeSearchDataHandler dhEmpSearch = new EmployeeSearchDataHandler();
            DataTable dtUsers = new DataTable();

            try
            {
                string sEPFNo           = txtEPFNo.Text.Trim();
                string sNICNo           = txtNIC.Text.Trim();
                string sCompanyId       = ddlCompany.SelectedValue.Trim();
                string sDepartmentId    = ddlDepartment.SelectedValue.Trim();
                string sDivisionId      = ddlDivision.SelectedValue.Trim();
                string sEmployeeStatus  = ddlStatus.SelectedValue.Trim();

                //2014-09-30
                string sSearchName      = txtSearchName.Text.Trim();
                string sDesignationId     = ddlDesignation.SelectedValue.Trim();


                if (sEPFNo.Length > 0) //Ignores all the other search criterias
                {
                    dtUsers = dhEmpSearch.populateByEPF(sEPFNo);
                }

                else if (sNICNo.Length > 0) //Ignores all the other search criterias
                {
                    dtUsers = dhEmpSearch.populateByNIC(sNICNo);
                }

                else if (sDivisionId.Length > 0)
                {
                    dtUsers = dhEmpSearch.populate(sCompanyId, sDepartmentId, sDivisionId, sEmployeeStatus, sSearchName, sDesignationId);
                }

                else if (sDepartmentId.Length > 0)
                {
                    dtUsers = dhEmpSearch.populate(sCompanyId, sDepartmentId, sEmployeeStatus, sSearchName, sDesignationId);
                }

                else if (sCompanyId.Length > 0)
                {
                    dtUsers = dhEmpSearch.populate(sCompanyId, sEmployeeStatus, sSearchName, sDesignationId);
                }

                else if (sCompanyId.Length == 0)
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        dtUsers = dhEmpSearch.populate(sEmployeeStatus, sSearchName, sDesignationId);
                    }
                    else
                        lblMsg.Text = "Please select a company from the list";
                }

                gvEmployee.DataSource = dtUsers;
                gvEmployee.DataBind();

                if (dtUsers.Rows.Count >= 1)
                {
                    gvEmployee.HeaderRow.Cells[0].Width = 80;
                    gvEmployee.HeaderRow.Cells[1].Width = 80;
                    gvEmployee.HeaderRow.Cells[2].Width = 80;
                    gvEmployee.HeaderRow.Cells[3].Width = 80;
                    gvEmployee.HeaderRow.Cells[4].Width = 80;
                    gvEmployee.HeaderRow.Cells[8].Width = 250;
                    gvEmployee.HeaderRow.Cells[10].Width = 100;
                    gvEmployee.HeaderRow.Cells[12].Width = 100;
                    gvEmployee.HeaderRow.Cells[14].Width = 100;
                    gvEmployee.HeaderRow.Cells[15].Width = 100;
                    gvEmployee.HeaderRow.Cells[16].Width = 100;
                    gvEmployee.HeaderRow.Cells[17].Width = 100;
                }
                /*
                if (dtUsers.Rows.Count >= 1)
                {
                    gvEmployee.HeaderRow.Cells[0].Text = "Emp. ID";
                    gvEmployee.HeaderRow.Cells[1].Text = "EPF";
                    gvEmployee.HeaderRow.Cells[2].Text = "Title";
                    gvEmployee.HeaderRow.Cells[3].Text = "Known Name";

                    gvEmployee.HeaderRow.Cells[4].Text = "NIC";
                    gvEmployee.HeaderRow.Cells[5].Text = "Status Code";
                    gvEmployee.HeaderRow.Cells[6].Text = "Status";
                    gvEmployee.HeaderRow.Cells[7].Text = "Comp. Code";
                    gvEmployee.HeaderRow.Cells[8].Text = "Company";
                    gvEmployee.HeaderRow.Cells[9].Text = "Dept ID";
                    gvEmployee.HeaderRow.Cells[10].Text = "Department";
                    gvEmployee.HeaderRow.Cells[11].Text = "Div ID";
                    gvEmployee.HeaderRow.Cells[12].Text = "Division";
                    
                    //2014-09-24
                    gvEmployee.HeaderRow.Cells[13].Text = "Branch ID";
                    gvEmployee.HeaderRow.Cells[14].Text = "Branch Name";
                    gvEmployee.HeaderRow.Cells[15].Text = "Cost Center";
                    gvEmployee.HeaderRow.Cells[16].Text = "Profit Center";

                    gvEmployee.HeaderRow.Cells[16].Text = "Desig. ID";
                    gvEmployee.HeaderRow.Cells[16].Text = "Desig.";
                }
                */

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lblerror.Text = ex.Message;
            }
            finally
            {
                dhEmpSearch = null;
                dtUsers.Dispose();
            }

        }


        private void clearCompany()
        {
            ddlCompany.SelectedIndex = -1;
        }


        private void clearDeapartment()
        {
            ddlDepartment.Items.Clear();
            ddlDepartment.SelectedIndex = -1;
        }


        private void clearDivision()
        {
            ddlDivision.Items.Clear();
            ddlDivision.SelectedIndex = -1;
        }


        private void clearStatus()
        {
            ddlStatus.SelectedIndex = -1;
        }


        private void clearEPF()
        {
            txtEPFNo.Text = "";
        }


        private void clearNIC()
        {
            txtNIC.Text = "";
        }


        private void clearDesgnation()
        {
            ddlDesignation.Items.Clear();
            ddlDesignation.SelectedIndex = -1;
        }

        private void clearSearchName()
        {
            txtSearchName.Text = "";
        }



        #endregion
    }
}