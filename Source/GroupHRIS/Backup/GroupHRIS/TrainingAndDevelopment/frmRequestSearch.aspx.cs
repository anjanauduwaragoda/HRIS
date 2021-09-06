using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using GroupHRIS.Utility;
using DataHandler;
using DataHandler.TrainingAndDevelopment;
using DataHandler.Utility;
using DataHandler.MetaData;
using DataHandler.Employee;
using System.Web.Caching;
using System.Data;
using NLog;


namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class frmRequestSearch : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            log.Debug("Page_Load():frmRequestSearch");

            //string referrer = Request.UrlReferrer.ToString();

            if ((Session["KeyLOGOUT_STS"] == null) || (Session["KeyLOGOUT_STS"].Equals("0")))
            {
                log.Debug("Session Expired");
                //lblerror.Text = "Session Expired. Please log in and try again.";
                Response.Redirect("~/Login/MainLogout.aspx", false);
            }
            if (!IsPostBack)
            {
                CreateMultipleSelectTable();
                fillTrainingRequestCategories();
                fillTrainingRequestTypes();

                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {

                        fillCompanies();
                    }
                    else
                    {

                        fillCompanies(Session["KeyCOMP_ID"].ToString().Trim());
                        //fillDepartment(Session["KeyCOMP_ID"].ToString().Trim());
                        //fillBranches(Session["KeyCOMP_ID"].ToString().Trim());
                    }
                }
            }
        }

        private void fillCompanies()
        {
            log.Debug("fillCompanies()");

            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable companies = new DataTable();

            try
            {
                if (Cache["Companies"] != null)
                {
                    companies = ((DataTable)Cache["Companies"]).Copy();
                }
                else
                {
                    companies = companyDataHandler.getCompanyIdCompName().Copy();
                    Cache.Add("Companies", companies, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }

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
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                companyDataHandler = null;
                companies.Dispose();
            }
        }

        private void fillCompanies(string companyId)
        {
            log.Debug("fillCompanies() - companyId:" + companyId);

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

        private void fillDepartments(string companyId)
        {
            log.Debug("fillDepartments() - companyId:" + companyId);

            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable dtDepartments = new DataTable();

            try
            {
                dtDepartments = companyDataHandler.getDepartments(companyId).Copy();

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
                companyDataHandler = null;
                dtDepartments.Dispose();
            }

        }

        private void fillDivisions(string DepartmentID)
        {
            log.Debug("fillDivisions() - DepartmentID:" + DepartmentID);

            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable dtDivisions = new DataTable();

            try
            {
                dtDivisions = companyDataHandler.getDivisions(DepartmentID).Copy();

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
                companyDataHandler = null;
                dtDivisions.Dispose();
            }

        }

        private void fillCompanyBranches(string companyId)
        {
            log.Debug("fillCompanyBranches() - companyId:" + companyId);

            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable dtCompanyBranches = new DataTable();

            try
            {
                dtCompanyBranches = companyDataHandler.getCompanyBranches(companyId).Copy();

                ddlBranch.Items.Clear();

                if (dtCompanyBranches.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlBranch.Items.Add(Item);

                    foreach (DataRow dataRow in dtCompanyBranches.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["BRANCH_NAME"].ToString();
                        listItem.Value = dataRow["BRANCH_ID"].ToString();

                        ddlBranch.Items.Add(listItem);
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
                dtCompanyBranches.Dispose();
            }

        }

        private void fillTrainingRequestCategories()
        {
            log.Debug("fillCompanyBranches()");

            TrainingRequestDataHandler trainingRequestDataHandler = new TrainingRequestDataHandler();
            DataTable dtRequestCategories = new DataTable();

            try
            {
                dtRequestCategories = trainingRequestDataHandler.getRequestsCategories().Copy();

                ddlCategory.Items.Clear();

                if (dtRequestCategories.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlCategory.Items.Add(Item);

                    foreach (DataRow dataRow in dtRequestCategories.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["CATEGORY_NAME"].ToString();
                        listItem.Value = dataRow["TRAINING_CATEGORY_ID"].ToString();

                        ddlCategory.Items.Add(listItem);
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
                trainingRequestDataHandler = null;
                dtRequestCategories.Dispose();
            }

        }

        private void fillTrainingRequestTypes()
        {
            log.Debug("fillCompanyBranches()");

            TrainingRequestDataHandler trainingRequestDataHandler = new TrainingRequestDataHandler();
            DataTable dtRequestTypes = new DataTable();

            try
            {
                dtRequestTypes = trainingRequestDataHandler.getRequestsTypes().Copy();

                ddlRequestType.Items.Clear();

                if (dtRequestTypes.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlRequestType.Items.Add(Item);

                    foreach (DataRow dataRow in dtRequestTypes.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["TYPE_NAME"].ToString();
                        listItem.Value = dataRow["REQUEST_TYPE_ID"].ToString();

                        ddlRequestType.Items.Add(listItem);
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
                trainingRequestDataHandler = null;
                dtRequestTypes.Dispose();
            }

        }

        private void LoadGrid()
        {
            log.Debug("LoadGrid()");

            TrainingRequestDataHandler trainingRequestDataHandler = new TrainingRequestDataHandler();
            DataTable dtRequests = new DataTable();

            try
            {
                dtRequests = trainingRequestDataHandler.getRequests(ddlCompany.SelectedValue, ddlDepartment.SelectedValue, ddlDivision.SelectedValue, ddlBranch.SelectedValue, ddlCategory.SelectedValue, ddlRequestType.SelectedValue).Copy();

                if (dtRequests.Rows.Count > 0)
                {
                    grdTrainingRequests.DataSource = dtRequests.Copy();
                    grdTrainingRequests.DataBind();
                }
                else
                {
                    grdTrainingRequests.DataSource = null;
                    grdTrainingRequests.DataBind();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                trainingRequestDataHandler = null;
                dtRequests.Dispose();
            }

        }

        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                LoadGrid();
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        protected void iBtnReset_Click(object sender, ImageClickEventArgs e)
        {

        }

        private void clear()
        {
            try
            {
                Utility.Utils.clearControls(true, ddlCompany, ddlDepartment, ddlDivision, ddlRequestType, ddlCategory, ddlBranch);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlDivision.Items.Clear();
                fillDepartments(ddlCompany.SelectedValue);
                fillCompanyBranches(ddlCompany.SelectedValue);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                fillDivisions(ddlDepartment.SelectedValue);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        protected void grdTrainingRequests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dtRequest = new DataTable(); 
            try
            {                
                dtRequest = (Session["dtRequest"] as DataTable).Copy();
                try
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdTrainingRequests, "Select$" + e.Row.RowIndex.ToString()));
                        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                        e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                        e.Row.Attributes.Add("style", "cursor:pointer;");

                        CheckBox chkInclude = (e.Row.FindControl("chkInclude") as CheckBox);
                        if (chkMultipleReq.Checked == true)
                        {
                            chkInclude.Enabled = true;
                        }
                        else
                        {
                            chkInclude.Enabled = false;
                        }

                        string RequestID = HttpUtility.HtmlDecode(e.Row.Cells[0].Text).Trim();

                        DataRow[] drArr = dtRequest.Select("REQUEST_ID = '" + RequestID + "'");
                        if (drArr.Length > 0)
                        {
                            chkInclude.Checked = true;
                        }
                        else
                        {
                            chkInclude.Checked = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMessage);
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        protected void grdTrainingRequests_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtRequestID.Text = HttpUtility.HtmlDecode(grdTrainingRequests.Rows[grdTrainingRequests.SelectedIndex].Cells[0].Text).Trim();
                txtDescription.Text = HttpUtility.HtmlDecode(grdTrainingRequests.Rows[grdTrainingRequests.SelectedIndex].Cells[1].Text).Trim();
                btnSelect.Visible = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdTrainingRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdTrainingRequests.PageIndex = e.NewPageIndex;
                LoadGrid();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateMultipleSelectTable()
        {
            DataTable dtRequest = new DataTable();
            try
            {
                dtRequest.Columns.Add("REQUEST_ID");
                dtRequest.Columns.Add("DESCRIPTION_OF_TRAINING");
                dtRequest.Columns.Add("REMARKS");

                Session["dtRequest"] = dtRequest.Copy();
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            finally
            {
                dtRequest.Dispose();
            }            
        }

        protected void chkInclude_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dtRequest = new DataTable();
            try
            {
                dtRequest = (Session["dtRequest"] as DataTable).Copy();

                for (int i = 0; i < grdTrainingRequests.Rows.Count; i++)
                {
                    CheckBox chkInclude = (grdTrainingRequests.Rows[i].FindControl("chkInclude") as CheckBox);

                    if (chkInclude.Checked)
                    {
                        DataRow dr = dtRequest.NewRow();
                        dr["REQUEST_ID"] = HttpUtility.HtmlDecode(grdTrainingRequests.Rows[i].Cells[0].Text).Trim();
                        dr["DESCRIPTION_OF_TRAINING"] = HttpUtility.HtmlDecode(grdTrainingRequests.Rows[i].Cells[1].Text).Trim();
                        dr["REMARKS"] = HttpUtility.HtmlDecode(grdTrainingRequests.Rows[i].Cells[3].Text).Trim();

                        DataRow[] drArr = dtRequest.Select("REQUEST_ID = '" + HttpUtility.HtmlDecode(grdTrainingRequests.Rows[i].Cells[0].Text).Trim() + "'");
                        if (drArr.Length == 0)
                        {
                            dtRequest.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        string RequestID = HttpUtility.HtmlDecode(grdTrainingRequests.Rows[i].Cells[0].Text).Trim();
                        DataRow[] drArr = dtRequest.Select("REQUEST_ID = '" + RequestID + "'");
                        foreach (DataRow dr in drArr)
                        {
                            dr.Delete();
                        }
                    }
                }

                if (dtRequest.Rows.Count > 0)
                {
                    btnSelect.Visible = true;
                }
                else
                {
                    btnSelect.Visible = false;
                }

                Session["dtRequest"] = dtRequest.Copy();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtRequest.Dispose();
            }
        }

        protected void chkMultipleReq_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dtRequest = new DataTable();
                dtRequest = (Session["dtRequest"] as DataTable).Copy();
                dtRequest.Rows.Clear();
                Session["dtRequest"] = dtRequest.Copy();

                for (int i = 0; i < grdTrainingRequests.Rows.Count; i++)
                {
                    CheckBox chkInclude = (grdTrainingRequests.Rows[i].FindControl("chkInclude") as CheckBox);

                    if (chkMultipleReq.Checked == false)
                    {
                        chkInclude.Enabled = false;
                        chkInclude.Checked = false;
                    }
                    else
                    {
                        chkInclude.Enabled = true;
                        chkInclude.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        

    }
}