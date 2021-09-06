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
    public partial class WebFrmTraingRequest : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string toRecommand = txtRecPerson.Text.Trim();
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmTraingRequest : Page_Load");

            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {
                sUserId = Session["KeyUSER_ID"].ToString();
            }

            if (!IsPostBack)
            {
                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    string companyId = Session["KeyCOMP_ID"].ToString().Trim();
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        lblCompany.Visible = true;
                        ddlCompany.Visible = true;
                        ddlCompany.Enabled = true;
                        fillCompanies();
                    }
                    else
                    {

                        fillCompanies();

                        lblCompany.Visible = false;
                        ddlCompany.Visible = false;
                        ddlCompany.Enabled = false;
                        ddlCompany.SelectedValue =Session["KeyCOMP_ID"].ToString().Trim();
                        fillDepartment(Session["KeyCOMP_ID"].ToString().Trim());
                        fillBranches(Session["KeyCOMP_ID"].ToString().Trim());
                    }
                }

                fillCategories();
                fillRequestTypes();
                filStatus();

                fillYear(ddlFinancialYear);
                fillYear(ddlFYear);

                fillRecommendApprovalStatus();

                string sFinancialYear = getCurrentFinancialYear();

                loadRequests(Session["KeyEMPLOYEE_ID"].ToString().Trim(), sFinancialYear, String.Empty);

                if (Page.PreviousPage != null)
                {
                    string sRequestId = "";
                    string sAction = "";

                    sRequestId = ((HiddenField)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$hfReqId")).Value.Trim();
                    sAction = ((HiddenField)Page.PreviousPage.FindControl("ctl00$ContentPlaceHolder1$hfStatus")).Value.Trim();
                    hfAction.Value = "";
                    hfAction.Value = sAction.Trim();

                    setEnvironment(sRequestId, sAction);

                    //DataRow dataRow = getRecommendApprove(sRequestId);

                    //if (dataRow != null)
                    //{
                    //    if (Session["KeyEMPLOYEE_ID"].ToString().Trim() == dataRow["TO_RECOMMEND"].ToString().Trim())
                    //    {
                    //        if(sAction.Trim().Equals(Constants.CON_RECOMMAND_TEXT))
                    //        {
                    //            chkApproved.Enabled = false;
                    //            chkAppRejected.Enabled = false;
                    //            txtApprovedReason.ReadOnly = true;
                    //            txtApprovedDate.Enabled = false;
                    //        }
                    //    }
                    //    else if (Session["KeyEMPLOYEE_ID"].ToString().Trim() == dataRow["TO_APPROVE"].ToString().Trim())
                    //    {
                    //        if (sAction.Trim().Equals(Constants.CON_APPROVE_TEXT))
                    //        {
                    //            chkRecommended.Enabled = false;
                    //            chkRecRejected.Enabled = false;
                    //            txtRecommendedReason.ReadOnly = true;
                    //            txtRecDate.Enabled = false;
                    //        }
                    //    }
                    //}

                    lblRecommendation.Visible = true;
                    chkRecommended.Visible = true;
                    lblRecommendedReason.Visible = true;
                    txtRecommendedReason.Visible = true;
                    lblApproval.Visible = true;
                    chkApproved.Visible = true;
                    lblApprovedReason.Visible = true;
                    txtApprovedReason.Visible = true;

                    // Load data for recommend/approval

                    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

                    Utility.Errorhandler.ClearError(lblMessage);

                    loadRequestForEditing(sRequestId);


                }
                else
                {
                    //lblRecommendation.Visible = false;
                    //lblRecommendedReason.Visible = false;
                    //txtRecommendedReason.Enabled = false;
                    //lblApproval.Visible = false;
                    //lblApprovedReason.Visible = false;
                    //txtApprovedReason.Enabled = false;

                    chkRecommended.Enabled = false;
                    chkRecRejected.Enabled = false;
                    txtRecommendedReason.ReadOnly = true;
                    txtRecDate.Enabled = false;

                    chkApproved.Enabled = false;
                    chkAppRejected.Enabled = false;
                    txtApprovedReason.ReadOnly = true;
                    txtAppDate.Enabled = false;
                }
            }
            else
            {

                if (hfCaller.Value == "txtRecPerson")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtRecPerson.Text = hfVal.Value;
                        hfVal.Value = "";
                    }
                    if (txtRecPerson.Text != "")
                    {
                        //Postback Methods
                    }
                }
                if (hfCaller.Value == "txtToApprove")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtToApprove.Text = hfVal.Value;
                        hfVal.Value = "";
                    }
                    if (txtToApprove.Text != "")
                    {
                        //Postback Methods
                    }
                }
                if (hfCaller.Value == "txtTrRequestId")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtTrRequestId.Text = hfVal.Value;
                        hfVal.Value = "";
                    }
                    if (txtTrRequestId.Text != "")
                    {
                        //Postback Methods
                    }
                }
            }
        }

        private void setAllDisable()
        {
            chkRecommended.Enabled = false;
            chkRecRejected.Enabled = false;
            txtRecommendedReason.ReadOnly = true;
            txtRecDate.Enabled = false;

            chkApproved.Enabled = false;
            chkAppRejected.Enabled = false;
            txtApprovedReason.ReadOnly = true;
            txtAppDate.Enabled = false;
        }

        private void recommendEnable()
        {
            chkRecommended.Enabled = true;
            chkRecRejected.Enabled = true;
            txtRecommendedReason.ReadOnly = false;
            txtRecommendedReason.Enabled = true;
            txtRecDate.Enabled = true;
        }

        private void approveEnable()
        {
            chkApproved.Enabled = true;
            chkAppRejected.Enabled = true;
            txtApprovedReason.ReadOnly = false;
            txtApprovedReason.Enabled = true;
            txtAppDate.Enabled = true;
        }

        private void recommendDisable()
        {
            chkRecommended.Enabled = false;
            chkRecRejected.Enabled = false;
            txtRecommendedReason.ReadOnly = true;
            txtRecDate.Enabled = false;
        }

        private void approveDisable()
        {
            chkApproved.Enabled = false;
            chkAppRejected.Enabled = false;
            txtApprovedReason.ReadOnly = true;
            txtAppDate.Enabled = false;
        }

        /// <summary>
        /// added by chathuraA 2017/03/07
        /// </summary>
        private void disableMasterDataInputs()
        {
            ddlCompany.Enabled = false;
            ddlDepartment.Enabled = false;
            ddlDivision.Enabled = false;
            ddlBranch.Enabled = false;
            ddlCategory.Enabled = false;
            ddlSubcategory.Enabled = false;
            ddlRequestType.Enabled = false;
            txtDescription.Enabled = false;
            txtReason.Enabled = false;
            txtSkillsExpected.Enabled = false;
            txtParticipants.Enabled = false;
            txtDate.Enabled = false;
            txtRecPerson.Enabled = false;
            txtToApprove.Enabled = false;
            txtRemarks.Enabled = false;
            chkRecommended.Enabled = false;
            txtRecommendedReason.Enabled = false;
            chkApproved.Enabled = false;
            txtApprovedReason.Enabled = false;
            ddlStatus.Enabled = false;
            ddlFinancialYear.Enabled = false;
            txtAppDate.Enabled = false;
            txtRecDate.Enabled = false;
            chkRecRejected.Enabled = false;
            chkAppRejected.Enabled = false;
            //btnSave.Enabled = false;
            imgSearch1.Visible = false;
            imgSearch2.Visible = false;
        }
        /// <summary>
        /// added by chathuraA 2017/03/07
        /// </summary>
        private void enableMasterDataInputs()
        {
            ddlCompany.Enabled = true;
            ddlDepartment.Enabled = true;
            ddlDivision.Enabled = true;
            ddlBranch.Enabled = true;
            ddlCategory.Enabled = true;
            ddlSubcategory.Enabled = true;
            ddlRequestType.Enabled = true;
            txtDescription.Enabled = true;
            txtReason.Enabled = true;
            txtSkillsExpected.Enabled = true;
            txtParticipants.Enabled = true;
            txtDate.Enabled = true;
            txtRecPerson.Enabled = true;
            txtToApprove.Enabled = true;
            txtRemarks.Enabled = true;
            chkRecommended.Enabled = true;
            txtRecommendedReason.Enabled = true;
            chkApproved.Enabled = true;
            txtApprovedReason.Enabled = true;
            ddlStatus.Enabled = true;
            ddlFinancialYear.Enabled = true;
            txtAppDate.Enabled = true;
            txtRecDate.Enabled = true;
            chkRecRejected.Enabled = true;
            chkAppRejected.Enabled = true;
            //btnSave.Enabled = true;
            imgSearch1.Visible = false;
            imgSearch2.Visible = false;
        }

        private void setEnvironment(string sRequestId, string sAction)
        {
            try
            {
                setAllDisable();
                
                DataRow dataRow = getRecommendApprove(sRequestId);

                if (dataRow != null)
                {
                    if (Session["KeyEMPLOYEE_ID"].ToString().Trim() == dataRow["TO_RECOMMEND"].ToString().Trim())
                    {
                        if (sAction.Trim().Equals(Constants.CON_RECOMMAND_TEXT))
                        {
                            approveDisable();

                            recommendEnable();
                        }
                        else if ((dataRow["IS_RECOMENDED"].ToString().Trim().Equals(string.Empty)) && (dataRow["IS_APPROVED"].ToString().Trim().Equals(string.Empty)))
                        {
                            recommendEnable();
                        }
                    }

                    if (Session["KeyEMPLOYEE_ID"].ToString().Trim() == dataRow["TO_APPROVE"].ToString().Trim())
                    {
                        if (sAction.Trim().Equals(Constants.CON_APPROVE_TEXT))
                        {
                            recommendDisable();

                            approveEnable();

                        }
                        else if ((dataRow["IS_RECOMENDED"].ToString().Trim().Equals(Constants.CON_APPROVED)) && (dataRow["IS_APPROVED"].ToString().Trim().Equals(string.Empty)))
                        {
                            approveEnable();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataRow getRecommendApprove(string requestId)
        {
            TrainingRequestDataHandler trainingRequestDataHandler = new TrainingRequestDataHandler();

            try
            {
                DataRow dr = trainingRequestDataHandler.getRecommendedApprovedPerson(requestId.Trim());

                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                trainingRequestDataHandler = null;
            }
        }

        private void fillYear(DropDownList ddlYear)
        {
            try
            {
                ddlYear.Items.Clear();

                var currentYear = DateTime.Today.Year;
                var nextYear = currentYear + 1;
                ddlYear.Items.Add("");
                ListItem listItem = new ListItem(nextYear.ToString(), nextYear.ToString());
                ddlYear.Items.Add(listItem);

                for (int i = 0; i >= -5; i--)
                {
                    // Now just add an entry that's the current year plus the counter
                    ListItem li = new ListItem((currentYear + i).ToString(), (currentYear + i).ToString());
                    ddlYear.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void filStatus()
        {
            ddlStatus.Items.Clear();

            ListItem listItemBlank = new ListItem();
            listItemBlank.Text = "";
            listItemBlank.Value = "";
            ddlStatus.Items.Add(listItemBlank);

            ListItem listItemActive = new ListItem();
            listItemActive.Text = Constants.STATUS_ACTIVE_TAG;
            listItemActive.Value = Constants.STATUS_ACTIVE_VALUE;
            ddlStatus.Items.Add(listItemActive);

            ListItem listItemInActive = new ListItem();
            listItemInActive.Text = Constants.STATUS_INACTIVE_TAG;
            listItemInActive.Value = Constants.STATUS_INACTIVE_VALUE;
            ddlStatus.Items.Add(listItemInActive);

        }

        private void fillRecommendApprovalStatus()
        {
            ListItem listItemBlank = new ListItem();
            listItemBlank.Text = "";
            listItemBlank.Value = "";
            ddlRecStatus.Items.Add(listItemBlank);

            ListItem listItemPending = new ListItem();
            listItemPending.Text = Constants.CON_PENDING_STRING;
            listItemPending.Value = "2";
            ddlRecStatus.Items.Add(listItemPending);

            ListItem listItemRecommended = new ListItem();
            listItemRecommended.Text = Constants.CON_RECOMENDED_STRING;
            listItemRecommended.Value = "3";
            ddlRecStatus.Items.Add(listItemRecommended);

            ListItem listItemApproved = new ListItem();
            listItemApproved.Text = Constants.CON_APPROVED_STRING;
            listItemApproved.Value = Constants.CON_APPROVED;
            ddlRecStatus.Items.Add(listItemApproved);

            ListItem listItemRejected = new ListItem();
            listItemRejected.Text = Constants.CON_REJECTED_STRING;
            listItemRejected.Value = Constants.CON_REJECTED;
            ddlRecStatus.Items.Add(listItemRejected);

        }

        private void fillCompanies()
        {
            log.Debug("fillCompanies()");

            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable companies = new DataTable();
            TrainingRequestDataHandler trainingRequestDataHandler = new TrainingRequestDataHandler();

            try
            {
                
                companies = companyDataHandler.getCompanyIdCompName().Copy();
                
                ddlCompany.Items.Clear();

                if (companies.Rows.Count > 0)
                {

                    companies = trainingRequestDataHandler.getAllActiveCompanies().Copy();
                    Cache.Add("Companies", companies, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);

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

        private void fillCompanies(string sCompanyId)
        {
            log.Debug("fillCompanies(" + sCompanyId + ")");

            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable companies = new DataTable();

            try
            {
                companies = companyDataHandler.getCompanyIdCompName(sCompanyId).Copy();

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
        


        private void fillDepartment(string companyId)
        {
            log.Debug("fillDepartment() - companyId:" + companyId);

            DepartmentDataHandler departmentDataHandler = new DepartmentDataHandler();
            DataTable departments = new DataTable();

            try
            {
                if (Cache["Departments" + companyId.Trim()] != null)
                {
                    departments = ((DataTable)Cache["Departments" + companyId.Trim()]).Copy();
                }
                else
                {
                    departments = departmentDataHandler.getDepartmentIdDeptName(companyId).Copy();
                    Cache.Add("Departments" + companyId.Trim(), departments, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);

                }

                ddlDepartment.Items.Clear();

                if (departments.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlDepartment.Items.Add(Item);

                    foreach (DataRow dataRow in departments.Rows)
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
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                departmentDataHandler = null;
                departments.Dispose();
            }

        }

        private void fillDivisions(string departmentId)
        {
            log.Debug("fillDivisions() - departmentId:" + departmentId);

            DivisionDataHandler divisionDataHandler = new DivisionDataHandler();
            DataTable divisions = new DataTable();

            try
            {
                if (Cache["Divisions" + departmentId.Trim()] != null)
                {
                    divisions = ((DataTable)Cache["Divisions" + departmentId.Trim()]).Copy();
                }
                else
                {
                    divisions = divisionDataHandler.getDivisionIdDivName(departmentId).Copy();
                    Cache.Add("Divisions" + departmentId.Trim(), divisions, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }


                ddlDivision.Items.Clear();

                if (divisions.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlDivision.Items.Add(Item);

                    foreach (DataRow dataRow in divisions.Rows)
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
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                divisionDataHandler = null;
                divisions.Dispose();
            }
        }

        protected void fillBranches(string sCompanyId)
        {
            BranchCenterDataHandler branchCenterDataHandler = new BranchCenterDataHandler();
            DataTable branches = new DataTable();
            TrainingRequestDataHandler trainingRequestDataHandler = new TrainingRequestDataHandler();

            try
            {
                if (Cache["Branches" + sCompanyId.Trim()] != null)
                {
                    branches = ((DataTable)Cache["Branches" + sCompanyId.Trim()]).Copy();
                }
                else
                {
                    //branches = branchCenterDataHandler.getBranchIdBranchName(sCompanyId).Copy();
                    branches = trainingRequestDataHandler.getActiveBranchesForCompany(sCompanyId).Copy();
                    Cache.Add("Branches" + sCompanyId.Trim(), branches, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);

                }

                ddlBranch.Items.Clear();

                if (branches.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlBranch.Items.Add(Item);

                    foreach (DataRow dataRow in branches.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["BRANCH_NAME"].ToString();
                        listItem.Value = dataRow["BRANCH_ID"].ToString();

                        ddlBranch.Items.Add(listItem);
                    }
                }

                if (branches.Rows.Count > 0)
                {
                    ddlBranch.Enabled = true;
                }
                else
                {
                    ddlBranch.Enabled = false;
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
                branchCenterDataHandler = null;
            }
        }

        private void fillCategories()
        {
            TrainingCategoryDataHandler trainingCategoryDataHandler = new TrainingCategoryDataHandler();
            DataTable category = new DataTable();

            try
            {
                if (Cache["Category"] != null)
                {
                    category = ((DataTable)Cache["Category"]).Copy();
                }
                else
                {
                    category = trainingCategoryDataHandler.getCategoryNameAndId().Copy();
                    Cache.Add("Category", category, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }

                ddlCategory.Items.Clear();

                if (category.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlCategory.Items.Add(Item);

                    foreach (DataRow dataRow in category.Rows)
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
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                trainingCategoryDataHandler = null;
            }
        }


        private void fillSubcategories(string categoryId)
        {
            TrainingSubcategoryDataHandler trainingSubcategoryDataHandler = new TrainingSubcategoryDataHandler();
            DataTable subcategory = new DataTable();
            try
            {
                if (Cache["subcategory" + categoryId.Trim()] != null)
                {
                    subcategory = ((DataTable)Cache["subcategory" + categoryId.Trim()]).Copy();
                }
                else
                {
                    subcategory = trainingSubcategoryDataHandler.getSubcategoryNameAndId(categoryId).Copy();
                    Cache.Add("subcategory" + categoryId.Trim(), subcategory, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }

                ddlSubcategory.Items.Clear();

                if (subcategory.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlSubcategory.Items.Add(Item);

                    foreach (DataRow dataRow in subcategory.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["TYPE_NAME"].ToString();
                        listItem.Value = dataRow["TYPE_ID"].ToString();

                        ddlSubcategory.Items.Add(listItem);
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        private void fillRequestTypes()
        {
            RequestTypeDataHandler requestTypeDataHandler = new RequestTypeDataHandler();
            DataTable requestTypes = new DataTable();

            try
            {
                if (Cache["requestType"] != null)
                {
                    requestTypes = ((DataTable)Cache["requestType"]).Copy();
                }
                else
                {
                    requestTypes = requestTypeDataHandler.getRequestTypeNameAndId().Copy();
                    Cache.Add("requestTypes", requestTypes, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }

                ddlRequestType.Items.Clear();

                if (requestTypes.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlRequestType.Items.Add(Item);

                    foreach (DataRow dataRow in requestTypes.Rows)
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
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                requestTypeDataHandler = null;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            Utility.Errorhandler.ClearError(lblMessage);

            TrainingRequestDataHandler trainingRequestDataHandler = new TrainingRequestDataHandler();
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            string addedBy = "";
            string employeeId = "";
            string designation = "";
            string eMailAddress = "";
            string categoryId = "";
            string subcategoryId = "";
            string companyId = "";
            string departmentId = "";
            string divisionId = "";
            string branchId = "";
            string requestType = "";
            string requestedBy = "";
            string reason = "";
            string description = "";
            string skillsExpected = "";
            Int32 participants = 0;
            string remarks = "";
            string toRecommand = "";
            string toApprove = "";
            string recommendedBy = "";
            string recommendedReason = "";
            string isRecommended = "";
            string approvedBy = "";
            string approvedReason = "";
            string isApproved = "";
            string statusCode = "";
            string financialYear = "";
            string requestedDate = "";
            string recommendedDate = "";
            string approvedDate = "";
            Boolean blApproval = false;
            Boolean blRecomendation = false;

            try
            {
                if (hfAction.Value.Trim().Equals(Constants.CON_RECOMMAND_TEXT))
                {
                    if ((chkRecommended.Checked == false) && (chkRecRejected.Checked == false))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Recommendation is required", lblMessage);
                        return;
                    }
                    else if (txtRecDate.Text.Trim().Equals(String.Empty))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Recommended date is required", lblMessage);
                        return;
                    }
                    else if (txtRecommendedReason.Text.Trim().Equals(String.Empty))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Recommended/Rejected reason is required", lblMessage);
                        return;
                    }
                }
                else if (hfAction.Value.Trim().Equals(Constants.CON_APPROVE_TEXT))
                {
                    if ((chkApproved.Checked == false) && (chkAppRejected.Checked == false))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Approval is required", lblMessage);
                        return;
                    }
                    else if (txtAppDate.Text.Trim().Equals(String.Empty))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Approved date is required", lblMessage);
                        return;
                    }
                    else if (txtApprovedReason.Text.Trim().Equals(String.Empty))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Approved/Rejected reason is required", lblMessage);
                        return;
                    }
                }



                if (Session["KeyUSER_ID"] != null)
                {
                    addedBy = Session["KeyUSER_ID"].ToString();
                }

                if (Session["KeyEMPLOYEE_ID"] != null)
                {
                    employeeId = Session["KeyEMPLOYEE_ID"].ToString();

                    DataRow dr = employeeDataHandler.getEmployeeDesignationAndEmail(employeeId.Trim());

                    if (dr != null)
                    {
                        designation = dr["DESIGNATION_ID"].ToString().Trim();
                        eMailAddress = dr["EMAIL"].ToString().Trim();
                    }
                }

                categoryId = ddlCategory.SelectedValue.Trim();
                bool categoryIsActive = trainingRequestDataHandler.checkCategoryStatus(categoryId);
                if (!categoryIsActive)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Selected Category is Inactive", lblMessage);
                    return;
                }

                subcategoryId = ddlSubcategory.SelectedValue.Trim();
                bool subCategoryIsActive = trainingRequestDataHandler.checkSubCategoryStatus(subcategoryId);
                if (!subCategoryIsActive)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Selected Subcategory is Inactive", lblMessage);
                    return;
                }

                if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                {
                    companyId = ddlCompany.SelectedValue.Trim();
                }
                else
                {
                    companyId = Session["KeyCOMP_ID"].ToString().Trim();
                }

                departmentId = ddlDepartment.SelectedValue.Trim();
                divisionId = ddlDivision.SelectedValue.Trim();
                branchId = ddlBranch.SelectedValue.Trim();
                requestType = ddlRequestType.SelectedValue.Trim();
                requestedBy = employeeId.Trim();
                reason = txtReason.Text.Trim();
                description = txtDescription.Text.Trim();
                skillsExpected = txtSkillsExpected.Text.Trim();
                participants = Int32.Parse(txtParticipants.Text.Trim());
                remarks = txtRemarks.Text.Trim();
                toRecommand = txtRecPerson.Text.Trim();
                toApprove = txtToApprove.Text.Trim();
                recommendedBy = "";
                recommendedReason = txtRecommendedReason.Text.Trim();
                isRecommended = "";
                approvedBy = "";
                approvedReason = txtApprovedReason.Text.Trim();
                isApproved = "";
                statusCode = Constants.CON_ACTIVE_STATUS;
                financialYear = ddlFinancialYear.SelectedItem.Text.Trim();
                requestedDate = txtDate.Text.Trim();

                if (Convert.ToDateTime(requestedDate) > DateTime.Now)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Invalid Requested date", lblMessage);
                    return;
                }

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> Insert");
                    Boolean isInserted = trainingRequestDataHandler.Insert(categoryId, subcategoryId, companyId, departmentId, divisionId,
                                                                           branchId, requestType, requestedBy, designation, eMailAddress,
                                                                           reason, description, skillsExpected, participants, requestedDate, remarks,
                                                                           toRecommand, toApprove, financialYear, statusCode, addedBy);


                    if (isInserted)
                    {
                        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('Designation is saved ..')", true); 
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblMessage);
                        setAllDisable();
                    }

                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    log.Debug("btnSave_Click() -> update");
                    string requestId = "";


                    if (hfTrainingRequestId.Value.ToString().Trim() != "") { requestId = hfTrainingRequestId.Value.ToString().Trim(); }

                    if (requestId != "")
                    {
                        if (hfAction.Value.Trim() != String.Empty)
                        {
                            DataRow dr = getRecommendApprove(requestId);

                            if (dr != null)
                            {
                                if ((Session["KeyEMPLOYEE_ID"].ToString().Trim() == dr["TO_RECOMMEND"].ToString().Trim()) && (hfAction.Value.Trim().Equals(Constants.CON_RECOMMAND_TEXT)))
                                {
                                    recommendedBy = dr["TO_RECOMMEND"].ToString().Trim();
                                    blRecomendation = true;
                                }
                                else if ((Session["KeyEMPLOYEE_ID"].ToString().Trim() == dr["TO_APPROVE"].ToString().Trim()) && (hfAction.Value.Trim().Equals(Constants.CON_APPROVE_TEXT)))
                                {
                                    approvedBy = dr["TO_APPROVE"].ToString().Trim();
                                    blApproval = true;
                                }
                            }

                            if (chkApproved.Checked == true) { isApproved = Constants.CON_APPROVED; }
                            else if (chkAppRejected.Checked == true) { isApproved = Constants.CON_REJECTED; }

                            if (chkRecommended.Checked == true) { isRecommended = Constants.CON_APPROVED; }
                            else if (chkRecRejected.Checked == true) { isRecommended = Constants.CON_REJECTED; }
                            recommendedDate = txtRecDate.Text.Trim();
                            approvedDate = txtAppDate.Text.Trim();
                        }


                        Boolean isUpdated = trainingRequestDataHandler.Update(requestId, categoryId, subcategoryId, companyId, departmentId, divisionId,
                                                                       branchId, requestType,
                                                                       reason, description, skillsExpected, participants, requestedDate, remarks,
                                                                       toRecommand, toApprove, financialYear, statusCode, addedBy, isApproved, approvedReason,
                                                                       approvedDate, approvedBy, isRecommended, recommendedReason, recommendedDate, recommendedBy,
                                                                       blRecomendation, blApproval);
                        if (isUpdated)
                        {
                            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert !..", "alert('Competency Group is updated ..')", true); 
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
                            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                            setAllDisable();
                        }
                    }
                }

                clear();

                string sFinancialYear = getCurrentFinancialYear();
                loadRequests(Session["KeyEMPLOYEE_ID"].ToString().Trim(), sFinancialYear, String.Empty);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                trainingRequestDataHandler = null;
                employeeDataHandler = null;

            }
        }

        private void clear()
        {
            try
            {

                Utils.clearControls(true, ddlCompany, ddlDepartment, ddlDivision, ddlBranch, ddlCategory, ddlSubcategory, ddlRequestType,
                                    txtDescription, txtReason, txtSkillsExpected, txtParticipants, txtDate, txtRecPerson, txtToApprove,
                                    txtRemarks, chkRecommended, txtRecommendedReason, chkApproved, txtApprovedReason, ddlStatus, ddlFinancialYear,
                                    txtAppDate, txtRecDate, chkRecRejected, chkAppRejected);
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                enableMasterDataInputs();
                btnSave.Enabled = true;
            }
            catch (Exception ex)
            {

                log.Error(ex.Message);
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMessage);
            clear();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            TrainingRequestDataHandler trainingRequestDataHandler = new TrainingRequestDataHandler();
            try
            {
                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                Utility.Errorhandler.ClearError(lblMessage);

                string requestId = txtTrRequestId.Text.ToString();

                DataRow dr = trainingRequestDataHandler.getRequest(requestId);
                if (dr["IS_RECOMENDED"].ToString().Trim() == Constants.CON_APPROVED || dr["IS_RECOMENDED"].ToString().Trim() == Constants.CON_REJECTED)
                {
                    btnSave.Enabled = false;
                }
                else
                {
                    btnSave.Enabled = true;
                }

                loadRequestForEditing(requestId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                trainingRequestDataHandler = null;
            }
        }

        protected void btnSCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Utility.Utils.clearControls(false, txtTrRequestId);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlDepartment_SelectedIndexChanged()");

            if (ddlDepartment.SelectedValue != "")
            {
                fillDivisions(ddlDepartment.SelectedValue.Trim());
            }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
            {
                if (ddlCompany.SelectedValue != "")
                {
                    fillDepartment(ddlCompany.SelectedValue.Trim());
                    fillBranches(ddlCompany.SelectedValue.Trim());
                }
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedValue != "")
            {
                fillSubcategories(ddlCategory.SelectedValue.Trim());
            }
        }

        protected void chkRecommended_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRecommended.Checked == true) { chkRecRejected.Checked = false; }
            else if (chkRecommended.Checked == false) { chkRecRejected.Checked = true; }
        }

        protected void chkRecRejected_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRecRejected.Checked == true) { chkRecommended.Checked = false; }
            else if (chkRecRejected.Checked == false) { chkRecommended.Checked = true; }
        }

        protected void chkApproved_CheckedChanged(object sender, EventArgs e)
        {
            if (chkApproved.Checked == true) { chkAppRejected.Checked = false; }
            else if (chkApproved.Checked == false) { chkAppRejected.Checked = true; }
        }

        protected void chkAppRejected_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAppRejected.Checked == true) { chkApproved.Checked = false; }
            else if (chkAppRejected.Checked == false) { chkApproved.Checked = true; }
        }

        protected void loadRequests(string sRequestedBy, string sYear, string sReqStatus)
        {
            TrainingRequestDataHandler trainingRequestDataHandler = new TrainingRequestDataHandler();
            DataTable dataTable = new DataTable();

            try
            {
                dataTable = trainingRequestDataHandler.getPersonalRequests(sRequestedBy, sReqStatus, sYear).Copy();
                gvRequests.DataSource = dataTable;
                gvRequests.DataBind();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                trainingRequestDataHandler = null;
                dataTable.Dispose();
            }

        }

        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Utility.Errorhandler.ClearError(lblSearchMessage);
                if (ddlFYear.SelectedItem.ToString().Trim().Equals(String.Empty)) { Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Select financial year", lblSearchMessage); return; }

                loadRequests(Session["KeyEMPLOYEE_ID"].ToString().Trim(), ddlFYear.SelectedItem.Text.Trim(), ddlRecStatus.SelectedItem.Text.Trim());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string getCurrentFinancialYear()
        {
            String FinancialYear = String.Empty;
            try
            {
                System.DateTime dtfin = System.DateTime.Now;

                int CurrentFinyear = 0;

                DateTime finDate = DateTime.ParseExact(dtfin.Year + "-04-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                if (finDate > System.DateTime.Now)
                {
                    CurrentFinyear = dtfin.AddYears(-1).Year;
                    FinancialYear = CurrentFinyear.ToString();
                }
                else
                {
                    System.DateTime dt = System.DateTime.Now;

                    FinancialYear = dt.Year.ToString();
                }

                return FinancialYear;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        private void loadRequestForEditing(string sRequestId)
        {
            TrainingRequestDataHandler trainingRequestDataHandler = new TrainingRequestDataHandler();
            DataRow dr = null;

            try
            {
                enableMasterDataInputs();
                dr = trainingRequestDataHandler.getRequest(sRequestId.Trim());

                hfTrainingRequestId.Value = "";
                hfTrainingRequestId.Value = dr["REQUEST_ID"].ToString().Trim();

                if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                {
                    fillCompanies();
                    if (ddlCompany.Items.Count > 0) { ddlCompany.SelectedValue = dr["COMPANY_ID"].ToString().Trim(); }
                }
                else
                {
                    fillCompanies(dr["COMPANY_ID"].ToString().Trim());
                    if (ddlCompany.Items.Count > 0) { ddlCompany.SelectedValue = dr["COMPANY_ID"].ToString().Trim(); }
                }


                if (ddlDepartment.Items.Count == 0) 
                { 
                    fillDepartment(dr["COMPANY_ID"].ToString().Trim()); 
                }
                //if (ddlDepartment.Items.Count > 0) 
                //{
                //    string dept = dr["DEPARTMENT_ID"].ToString().Trim(); 
                //    ddlDepartment.SelectedValue = dept; 
                //}

                fillDepartment(dr["COMPANY_ID"].ToString().Trim()); 
                if (ddlDepartment.Items.Count > 0) 
                {
                    string deptId = dr["DEPARTMENT_ID"].ToString();
                    ddlDepartment.SelectedValue = dr["DEPARTMENT_ID"].ToString().Trim(); 
                }

                fillDivisions(dr["DEPARTMENT_ID"].ToString().Trim()); 
                if (ddlDivision.Items.Count > 0) { ddlDivision.SelectedValue = dr["DIVISION_ID"].ToString().Trim(); }

                fillBranches(dr["COMPANY_ID"].ToString().Trim()); 
                if (ddlBranch.Items.Count > 0) { ddlBranch.SelectedValue = dr["BRANCH_ID"].ToString().Trim(); }

                fillCategories(); 

                if (ddlCategory.Items.Count > 0) 
                {
                    if (Utils.isValueExistInDropDownList(dr["TRAINING_CATEGORY"].ToString().Trim(), ddlCategory))
                    {
                        ddlCategory.SelectedValue = dr["TRAINING_CATEGORY"].ToString().Trim();
                    }
                    else
                    {
                        fillTrainingCategoriesWithInactiveCategory(dr["TRAINING_CATEGORY"].ToString().Trim(), ddlCategory);
                        ddlCategory.SelectedValue = dr["TRAINING_CATEGORY"].ToString().Trim();                        
                    }
                    
                }

                fillSubcategories(dr["TRAINING_CATEGORY"].ToString().Trim());

                if (ddlSubcategory.Items.Count > 0)
                {
                    if (Utils.isValueExistInDropDownList(dr["TRAINING_SUB_CATEGORY_ID"].ToString().Trim(), ddlSubcategory))
                    {
                        ddlSubcategory.SelectedValue = dr["TRAINING_SUB_CATEGORY_ID"].ToString().Trim();
                    }
                    else
                    {
                        fillTrainingSubcategoriesWithInactiveSubcategory(dr["TRAINING_CATEGORY"].ToString().Trim(), dr["TRAINING_SUB_CATEGORY_ID"].ToString().Trim(), ddlSubcategory);
                        ddlSubcategory.SelectedValue = dr["TRAINING_SUB_CATEGORY_ID"].ToString().Trim();
                    }
                }
                else
                {
                    fillTrainingSubcategoriesWithInactiveSubcategory(dr["TRAINING_CATEGORY"].ToString().Trim(), dr["TRAINING_SUB_CATEGORY_ID"].ToString().Trim(), ddlSubcategory);
                    ddlSubcategory.SelectedValue = dr["TRAINING_SUB_CATEGORY_ID"].ToString().Trim();
                }

                fillRequestTypes(); 
                if (ddlRequestType.Items.Count > 0) { ddlRequestType.SelectedValue = dr["REQUEST_TYPE"].ToString().Trim(); }

                txtDescription.Text = dr["DESCRIPTION_OF_TRAINING"].ToString().Trim();
                txtSkillsExpected.Text = dr["SKILLS_EXPECTED"].ToString().Trim();
                txtReason.Text = dr["REASON"].ToString().Trim();
                txtParticipants.Text = dr["NUMBER_OF_PARTICIPANTS"].ToString().Trim();
                txtDate.Text = dr["REQUESTED_DATE"].ToString().Trim();
                txtRecPerson.Text = dr["TO_RECOMMEND"].ToString().Trim();
                txtToApprove.Text = dr["TO_APPROVE"].ToString().Trim();
                txtRemarks.Text = dr["REMARKS"].ToString().Trim();

                filStatus(); 
                if (ddlStatus.Items.Count > 0) { ddlStatus.SelectedValue = dr["STATUS_CODE"].ToString().Trim(); }

                fillYear(ddlFinancialYear); 
                if (ddlFinancialYear.Items.Count > 0) { ddlFinancialYear.SelectedValue = dr["FINANCIAL_YEAR"].ToString().Trim(); }


                if (dr["IS_RECOMENDED"].ToString().Trim() == Constants.CON_APPROVED) 
                { 
                    chkRecommended.Checked = true; 
                    disableMasterDataInputs(); 
                }
                else if (dr["IS_RECOMENDED"].ToString().Trim() == Constants.CON_REJECTED) 
                { 
                    chkRecRejected.Checked = true;
                    disableMasterDataInputs(); 
                }
                else { chkRecommended.Checked = false; chkRecRejected.Checked = false; }

                txtRecDate.Text = dr["RECOMENDED_DATE"].ToString().Trim();
                txtRecommendedReason.Text = dr["RECOMENDED_REASON"].ToString().Trim();

                if (dr["IS_APPROVED"].ToString().Trim() == Constants.CON_APPROVED) 
                { 
                    chkApproved.Checked = true; 
                    disableMasterDataInputs(); 
                }
                else if (dr["IS_APPROVED"].ToString().Trim() == Constants.CON_REJECTED) 
                { 
                    chkAppRejected.Checked = true;
                    disableMasterDataInputs(); 
                }
                else { chkApproved.Checked = false; chkAppRejected.Checked = false; }

                txtAppDate.Text = dr["APPROVED_DATE"].ToString().Trim();
                txtApprovedReason.Text = dr["APPROVED_REASON"].ToString().Trim();

                setEnvironment(hfTrainingRequestId.Value, hfAction.Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void fillTrainingCategoriesWithInactiveCategory(string sCategoryId, DropDownList dropDownList)
        {
            TrainingCategoryDataHandler trainingCategoryDataHandler = new TrainingCategoryDataHandler();
            DataTable dataTable = new DataTable();

            try
            {
                dataTable = trainingCategoryDataHandler.getCategories(Constants.CON_ACTIVE_STATUS, sCategoryId.Trim());

                dropDownList.Items.Clear();

                if (dataTable.Rows.Count > 0)
                {
                    ListItem listItemBlank = new ListItem();
                    listItemBlank.Text = "";
                    listItemBlank.Value = "";

                    dropDownList.Items.Add(listItemBlank);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dr["CATEGORY_NAME"].ToString();
                        listItem.Value = dr["TRAINING_CATEGORY_ID"].ToString();

                        dropDownList.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                trainingCategoryDataHandler = null;
                dataTable.Dispose();
            }
        }

        private void fillTrainingSubcategoriesWithInactiveSubcategory(string category,string subcategoryId,DropDownList dropDownList)
        {

            TrainingSubcategoryDataHandler trainingSubcategoryDataHandler = new TrainingSubcategoryDataHandler();
            DataTable subcategory = new DataTable();
            try
            {
                
                subcategory = trainingSubcategoryDataHandler.getSubcategoryNameAndIdWithInactiveSubcategory(category,subcategoryId).Copy();
                
                dropDownList.Items.Clear();

                if (subcategory.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    dropDownList.Items.Add(Item);

                    foreach (DataRow dataRow in subcategory.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["TYPE_NAME"].ToString();
                        listItem.Value = dataRow["TYPE_ID"].ToString();

                        dropDownList.Items.Add(listItem);
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }

        }        

        protected void gvRequests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvRequests, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void gvRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvRequests_PageIndexChanging()");

            try
            {
                gvRequests.PageIndex = e.NewPageIndex;

                string sFinancialYear = getCurrentFinancialYear();

                loadRequests(Session["KeyEMPLOYEE_ID"].ToString().Trim(), sFinancialYear, String.Empty);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        protected void gvRequests_SelectedIndexChanged(object sender, EventArgs e)
        {
            TrainingRequestDataHandler trainingRequestDataHandler = new TrainingRequestDataHandler();
            try
            {
                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                Utility.Errorhandler.ClearError(lblMessage);

                string requestId = gvRequests.SelectedRow.Cells[0].Text;

                

                DataRow dr = trainingRequestDataHandler.getRequest(requestId);
                if (dr["IS_RECOMENDED"].ToString().Trim() == Constants.CON_APPROVED || dr["IS_RECOMENDED"].ToString().Trim() == Constants.CON_REJECTED)
                {
                    btnSave.Enabled = false;
                }
                else
                {
                    btnSave.Enabled = true;
                }

                loadRequestForEditing(requestId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                trainingRequestDataHandler = null;
            }
        }

        protected void ddlFYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblSearchMessage);
        }

        protected void ddlRecStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblSearchMessage);
        }


    }
}