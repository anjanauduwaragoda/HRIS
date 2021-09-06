using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.Employee;
using DataHandler.EmployeeLeave;
using DataHandler.MetaData;
using Common;
using NLog;

namespace GroupHRIS.Employee
{
    public partial class webFrmEmployeeDesignation : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "webFrmEmployeeDesignation : Page_Load");


            if (Session["KeyLOGOUT_STS"].Equals("0"))
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            
            if (!IsPostBack)
            {
                ////[TEST]
                //Session["KeyCOMP_ID"] = Constants.CON_UNIVERSAL_COMPANY_CODE;

                ////Session["KeyCOMP_ID"] = "CP17";

                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        fillCompanies();
                        fillDesignations();
                    }
                    else
                    {
                        fillCompanies(Session["KeyCOMP_ID"].ToString().Trim());                                               
                        fillDesignations(Session["KeyCOMP_ID"].ToString().Trim());
                        ddlCompany.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                    }
                }  
            }

            
        }
      
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlCompany_SelectedIndexChanged()");

            if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
            {
                if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                {
                    fillDesignations(ddlCompany.SelectedValue.Trim());
                }
                else
                {                    
                    fillDesignations(Session["KeyCOMP_ID"].ToString().Trim());
                    //ddlCompany.SelectedIndex = 1;
                }
            }

            txtDesignation.Text = "";
            txtRemarks.Text = "";
            ddlStatus.SelectedIndex = 0;
            Utility.Errorhandler.ClearError(lblMessage);
        }

        //protected void gvDesignations_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        //{
        //    //string designationId1 = gvDesignations.SelectedRow.Cells[1].Text; 
        //    try
        //    {
        //        string designationId = gvDesignations.Rows[e.NewSelectedIndex].Cells[0].Text.ToString().Trim();

        //        fillDesignation(designationId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //protected void gvDesignations_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {
        //        if (e.CommandName.ToString().Equals("EditDesignation"))
        //        {
        //            Int32 index = Convert.ToInt32(e.CommandArgument);

        //            GridViewRow selectedRow = gvDesignations.Rows[index];

        //            string designationId = selectedRow.Cells[0].Text.ToString().Trim();

        //            fillDesignation(designationId);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            log.Debug("btnCancel_Click()");
            clear();
        }

       

        protected void gvDesignations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvDesignations_PageIndexChanging()");

            try
            {
                gvDesignations.PageIndex = e.NewPageIndex;

                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        if (ddlCompany.SelectedValue.Trim() == "")
                        {
                            fillDesignations();
                        }
                        else
                        {
                            fillDesignations(ddlCompany.SelectedValue.Trim());
                        }
                    }
                    else
                    {
                        fillDesignations(Session["KeyCOMP_ID"].ToString().Trim());
                    }
                }              

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            DesignationDataHandler designationDataHandler = new DesignationDataHandler();

            string addedBy = "";

            string designationName  = "";
            string remarks          = "";
            string statusCode       = "";
            string companyId        = "";

            try
            {
                companyId       = ddlCompany.SelectedItem.Value.ToString();
                designationName = txtDesignation.Text;
                remarks         = txtRemarks.Text;
                statusCode      = ddlStatus.SelectedItem.Value.ToString();

                if (Session["KeyUSER_ID"] != null)
                {
                    addedBy = Session["KeyUSER_ID"].ToString();
                }
                

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Insert");
                    Boolean isInserted = designationDataHandler.Insert(designationName, remarks, statusCode, companyId, addedBy);

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                    if (isInserted) 
                    { 
                        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('Designation is saved ..')", true); 
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Designation is saved", lblMessage);
                    } 
                        
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> update");
                    string designationId = "";

                    if(hfDesignationId.Value.ToString().Trim() != "") designationId = hfDesignationId.Value.ToString().Trim();

                    if(designationId != "")
                    {
                        Boolean isUpdated = designationDataHandler.Update(designationId, designationName, remarks, statusCode, companyId, addedBy);

                        if (isUpdated) 
                        { 
                            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('Designation is updated ..')", true); 
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Designation is updated", lblMessage);
                        }
                            
                    }

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                }

                fillDesignations(companyId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                designationDataHandler = null;
                //clear();
            }
        }

        protected void gvDesignations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvDesignations_RowDataBound()");

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvDesignations, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
                //btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        protected void gvDesignations_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gvDesignations_SelectedIndexChanged()");

            Utility.Errorhandler.ClearError(lblMessage);

            try
            {
                string designationId = gvDesignations.SelectedRow.Cells[0].Text.ToString().Trim();
                fillDesignation(designationId);
               
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        #region Private Methods
        //------------------------------------------------------------------------------


        private void fillDesignations()
        {
            log.Debug("fillDesignations() -> all designations");

            DesignationDataHandler designationDataHandler = new DesignationDataHandler();
            DataTable designations = new DataTable();

            try
            {
                designations = designationDataHandler.populate().Copy();

                gvDesignations.DataSource = designations;
                gvDesignations.DataBind();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                designationDataHandler = null;
                designations.Dispose();
            }
        }

        private void fillDesignations(string company_id)
        {
            log.Debug("fillDesignations() -> for given company_id");

            DesignationDataHandler designationDataHandler = new DesignationDataHandler();
            DataTable designations = new DataTable();

            try
            {
                designations = designationDataHandler.populate(company_id).Copy();

                gvDesignations.DataSource = designations;
                gvDesignations.DataBind();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                designationDataHandler = null;
                designations.Dispose();
            }

        }

        private void fillDesignation(string designationId)
        {
            log.Debug("fillDesignations() -> for given designationId");

            DesignationDataHandler designationDataHandler = new DesignationDataHandler();
            DataRow dataRow = null;

            try
            {
                dataRow = designationDataHandler.getDesignationDetails(designationId);

                if (dataRow != null)
                {
                    hfDesignationId.Value = dataRow["DESIGNATION_ID"].ToString().Trim();
                    ddlCompany.SelectedValue = dataRow["COMPANY_ID"].ToString().Trim();
                    txtDesignation.Text = dataRow["DESIGNATION_NAME"].ToString().Trim();
                    txtRemarks.Text = dataRow["REMARKS"].ToString().Trim();
                    ddlStatus.SelectedValue = dataRow["STATUS_CODE"].ToString().Trim();

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                designationDataHandler = null;
                dataRow = null;
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

            try
            {
                companies = companyDataHandler.getCompanyIdCompName().Copy();

                ddlCompany.Items.Clear();

                if (companies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlCompany.Items.Add(Item);

                    foreach (DataRow dataRow in companies.Rows)
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

                ddlCompany.Items.Clear();

                if (companies.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlCompany.Items.Add(Item);

                    foreach (DataRow dataRow in companies.Rows)
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
                throw ex;
            }
            finally
            {
                companyDataHandler = null;
                companies.Dispose();
            }

        }


        private void clear()
        {
            log.Debug("clear()");

            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;

            hfDesignationId.Value = "";
            ddlCompany.SelectedIndex = -1;
            txtDesignation.Text = "";
            txtRemarks.Text = "";
            ddlStatus.SelectedIndex = -1;
            Utility.Errorhandler.ClearError(lblMessage);
        }

        #endregion


    }
}