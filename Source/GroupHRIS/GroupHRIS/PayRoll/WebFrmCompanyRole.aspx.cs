using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.Payroll;
using Common;
using GroupHRIS.Utility;

namespace GroupHRIS.PayRoll
{
    public partial class WebFrmCompanyRole : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlStatus.Items.Insert(0, new ListItem("", ""));
                ddlStatus.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                ddlStatus.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));


                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        fillCompanies();
                    }
                    else
                    {
                        fillCompanies(Session["KeyCOMP_ID"].ToString().Trim());
                        ddlCompany.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                    }
                }
                GetCompanyCategory();
                fillRole();
            }
            
            
        }

        public void GetCompanyCategory()
        {
            string[] enumNames = Enum.GetNames(typeof(Common.Constants.EmployeeType));
            ListItem Item = new ListItem();
            Item.Text = "";
            Item.Value = "";
            ddlOTCategory.Items.Add(Item);
            foreach (string item in enumNames)
            {
                //get the enum item value
                int value = (int)Enum.Parse(typeof(Common.Constants.EmployeeType), item);
                ListItem listItem = new ListItem(item, value.ToString());
                ddlOTCategory.Items.Add(listItem);
            }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            string company = ddlCompany.SelectedValue;
            //fillCompanyOTCategory(company);
            fillRole();
            CompanyRoleDataHandler oCompanyRoleDataHandler = new CompanyRoleDataHandler();
            gvCompanyRoleType.DataSource = oCompanyRoleDataHandler.GetAll(company).Copy();
            gvCompanyRoleType.DataBind();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string company = ddlCompany.SelectedValue;
            string otCategory = ddlOTCategory.SelectedItem.ToString();
            string role = ddlRole.SelectedValue;
            string chk = ddlStatus.SelectedValue;

            string logUser = Session["KeyUSER_ID"].ToString();

            //if (chkIsActive.Checked == true)
            //{
            //    chk = "1";
            //}

            try
            {
                Errorhandler.ClearError(StatusLabel);
                CompanyRoleDataHandler oCompanyRoleDataHandler = new CompanyRoleDataHandler();

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    Boolean Status = oCompanyRoleDataHandler.InsertRole(company, otCategory, role, chk, logUser);
                    gvCompanyRoleType.DataSource = oCompanyRoleDataHandler.GetAll(company).Copy();
                    gvCompanyRoleType.DataBind();
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Saved", StatusLabel);
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string oldCategory = Session["OldCategory"].ToString();
                    string oldRole = Session["OldRole"].ToString();

                    Boolean Status = oCompanyRoleDataHandler.UpdateRole(company, oldCategory, oldRole, chk, otCategory, role, logUser);
                    gvCompanyRoleType.DataSource = oCompanyRoleDataHandler.GetAll(company).Copy();
                    gvCompanyRoleType.DataBind();
                    if (Status)
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Updated", StatusLabel);
                    }
                    else
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Inactive Record Can't Update", StatusLabel);
                    }
                }

                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                ddlCompany.SelectedIndex = 0;
                ddlOTCategory.SelectedIndex = 0;
                ddlRole.SelectedIndex = 0;
                ddlStatus.SelectedIndex = 0;
            }
            catch (Exception)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Record Already Exists", StatusLabel);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlCompany.SelectedIndex = 0;
            ddlCompany.Enabled = true;
            gvCompanyRoleType.DataSource = null;
            gvCompanyRoleType.DataBind();
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            Clear();

            ddlOTCategory.Enabled = true;
            ddlRole.Enabled = true;
        }

        public void Clear()
        {
            Errorhandler.ClearError(StatusLabel);
            ddlOTCategory.SelectedIndex = 0;
            ddlRole.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0; 
        }

        private void fillCompanies()
        {
            CompanyRoleDataHandler companyRoleDataHandler = new CompanyRoleDataHandler();
            DataTable companies = new DataTable();

            try
            {
                if (Cache["Companies"] != null)
                {
                    companies = (DataTable)Cache["Companies"];
                }
                else
                {
                    companies = companyRoleDataHandler.getCompanyIdCompName().Copy();
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
                throw ex;
            }
            finally
            {
                companyRoleDataHandler = null;
                companies.Dispose();
            }
        }

        private void fillCompanies(string companyId)
        {
            CompanyRoleDataHandler companyRoleDataHandler = new CompanyRoleDataHandler();
            DataTable companies = new DataTable();

            try
            {
                companies = companyRoleDataHandler.getCompanyIdCompName(companyId).Copy();

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
                throw ex;
            }
            finally
            {
                companyRoleDataHandler = null;
                companies.Dispose();
            }

        }

        //private void fillCompanyOTCategory(string company)
        //{
        //    CompanyRoleDataHandler companyRoleDataHandler = new CompanyRoleDataHandler();
        //    DataTable role = new DataTable();
        //    try
        //    {
        //        role = companyRoleDataHandler.getCompanyOTCategory(company).Copy();
        //        ddlOTCategory.Items.Clear();

        //        if(role.Rows.Count  > 0)
        //        {
        //            ListItem Item = new ListItem();
        //            Item.Text = "";
        //            Item.Value = "";
        //            ddlOTCategory.Items.Add(Item);
        //            foreach (DataRow dataRow in role.Rows)
        //            {
        //                ListItem listItem = new ListItem();
        //                listItem.Text = dataRow["COMPANY_OT_CATEGORY_NAME"].ToString();
        //                listItem.Value = dataRow["ROLE_CATEGORY_ID"].ToString();

        //                ddlOTCategory.Items.Add(listItem);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        companyRoleDataHandler = null;
        //        role.Dispose();
        //    }
        //}

        private void fillRole()
        {
            CompanyRoleDataHandler companyRoleDataHandler = new CompanyRoleDataHandler();
            DataTable roles= new DataTable();

            try
            {
                roles = companyRoleDataHandler.getRole().Copy();

                ddlRole.Items.Clear();

                if (roles.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlRole.Items.Add(Item);

                    foreach (DataRow dataRow in roles.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["ROLE_NAME"].ToString();
                        listItem.Value = dataRow["ROLE_ID"].ToString();

                        ddlRole.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                companyRoleDataHandler = null;
                roles.Dispose();
            }
        }

        protected void gvCompanyRoleType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvCompanyRoleType, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, StatusLabel);
            }
        }

        protected void gvCompanyRoleType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string company = ddlCompany.SelectedValue;
            CompanyRoleDataHandler oCompanyRoleDataHandler = new CompanyRoleDataHandler();
            gvCompanyRoleType.PageIndex = e.NewPageIndex;
            gvCompanyRoleType.DataSource = oCompanyRoleDataHandler.GetAll(company);
            gvCompanyRoleType.DataBind();
            oCompanyRoleDataHandler = null;
        }

        protected void gvCompanyRoleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(StatusLabel);
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            int SelectedIndex = gvCompanyRoleType.SelectedIndex;

            string company = HttpUtility.HtmlDecode(gvCompanyRoleType.Rows[SelectedIndex].Cells[0].Text.Trim());
            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(company));
            ddlCompany.Enabled = false; 

            string OTCategory = HttpUtility.HtmlDecode(gvCompanyRoleType.Rows[SelectedIndex].Cells[1].Text.Trim());
            ddlOTCategory.SelectedIndex = ddlOTCategory.Items.IndexOf(ddlOTCategory.Items.FindByText(OTCategory));

            string Role = HttpUtility.HtmlDecode(gvCompanyRoleType.Rows[SelectedIndex].Cells[2].Text.Trim());
            ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByText(Role));

            //string chk = Server.HtmlDecode(gvCompanyRoleType.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());

            //if (chk == Constants.STATUS_ACTIVE_TAG)
            //{
            //    chkIsActive.Checked = true;
            //}
            //else
            //{
            //    chkIsActive.Checked = false;
            //}

            string status = Server.HtmlDecode(gvCompanyRoleType.Rows[SelectedIndex].Cells[3].Text.ToString().Trim());
            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));


            Session["OldCategory"] = ddlOTCategory.SelectedItem.Text;
            Session["OldRole"] = ddlRole.SelectedItem.Value;

            if (ddlStatus.SelectedIndex == 2)
            {
                ddlOTCategory.Enabled = false;
                ddlRole.Enabled = false;
            }
            else
            {
                ddlOTCategory.Enabled = true;
                ddlRole.Enabled = true;
            }
        }

        protected void ddlOTCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            string company = ddlCompany.SelectedItem.Value;
            string otCategory = ddlOTCategory.SelectedItem.Text;
            string role = ddlRole.SelectedItem.Value;

            CompanyRoleDataHandler oCompanyRoleDataHandler = new CompanyRoleDataHandler();
            Boolean Status = oCompanyRoleDataHandler.IsExist(company,otCategory,role);
            if (Status == false)
            {
                ddlRole.SelectedIndex = 0;
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Record Already Exists ", StatusLabel);
            }
            else 
            {
                Errorhandler.ClearError(StatusLabel);
            }
            
        }
    }
}