using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.PerformanceManagement;
using System.Data;
using DataHandler.Utility;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmAssessmentGroup : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                sIPAddress = Request.UserHostAddress;
                log.Debug("IP:" + sIPAddress + "webFrmEmployee : Page_Load");

                loadStatus();
                loadGrid();

                loadEmployeeRoles();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtEmployeeRoles = new DataTable();
            try
            {
                log.Debug("btnSave_Click()");

                Utility.Errorhandler.ClearError(lblerror);
                lblProfiles.Text = String.Empty;
                //mapEmployeeRoleAssessmentGroups();

                if (isSpecialCharacterExists())
                {
                    CommonVariables.MESSAGE_TEXT = "Special characters are not allowed for Group Name.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    return;
                }

                string GroupName = txtGroupName.Text.Trim();
                string Description = txtDescription.Text.Trim();
                string Status = ddlStatus.SelectedValue.ToString();
                string user = (Session["KeyEMPLOYEE_ID"] as string).Trim();

                AssessmentGroupDataHandler OAGDH = new AssessmentGroupDataHandler();
                UtilsDataHandler utilsDataHandler = new UtilsDataHandler();

                

                dtEmployeeRoles = (Session["dtEmployeeRoles"] as DataTable).DefaultView.ToTable(true).Copy();

                if (ddlStatus.SelectedValue == Constants.CON_INACTIVE_STATUS)
                {
                    if (isActiveAssessmentsExists())
                    {
                        CommonVariables.MESSAGE_TEXT = "Cannot make inactive. Active Assessment profile(s) exist";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        return;
                    }
                    else
                    {
                        lblProfiles.Text = String.Empty;
                    }
                }


                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    if (OAGDH.CheckAssessmentGroupNameExsistance(txtGroupName.Text.Trim()))
                    {
                        CommonVariables.MESSAGE_TEXT = "Group name already exists.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        return;
                    }


                    //string group_ID = dtEmployeeRoles.Rows[0]["GROUP_ID"].ToString();
                    DataRow[] drEmployeeRoles = dtEmployeeRoles.Select("GROUP_ID IS NULL AND IS_SELECTED = '" + Constants.CON_ACTIVE_STATUS + "'");

                    if (drEmployeeRoles.Length > 0)
                    {
                        OAGDH.Insert(GroupName, Description, Status, user, drEmployeeRoles);
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);

                        if (Status == Constants.CON_INACTIVE_STATUS)
                        {
                            CommonVariables.MESSAGE_TEXT = "Cannot assign employee roles to inactive assessment group";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "Please Select at least one employee role.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        return;
                    }
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string GroupID = grdvAssessmentGroup.Rows[grdvAssessmentGroup.SelectedIndex].Cells[0].Text;
                    DataRow[] drEmptyRoles = dtEmployeeRoles.Select("GROUP_ID = '" + GroupID + "' AND IS_SELECTED = '"+Constants.CON_ACTIVE_STATUS+"'");
                    if (drEmptyRoles.Length == 0)
                    {
                        CommonVariables.MESSAGE_TEXT = "Please Select at least one employee role.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        return;
                    }

                    string AssessmentGroupID = grdvAssessmentGroup.Rows[grdvAssessmentGroup.SelectedIndex].Cells[0].Text;

                    if (OAGDH.CheckAssessmentGroupNameExsistance(txtGroupName.Text.Trim(), AssessmentGroupID))
                    {
                        CommonVariables.MESSAGE_TEXT = "Group name already exists.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        return;
                    }


                    DataRow[] drEmployeeRoles = dtEmployeeRoles.Select("GROUP_ID = '" + AssessmentGroupID + "' AND IS_SELECTED = '" + Constants.CON_ACTIVE_STATUS + "'");

                    OAGDH.Update(GroupName, Description, Status, user, AssessmentGroupID, drEmployeeRoles);
                    CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);

                    if (Status == Constants.CON_INACTIVE_STATUS)
                    {
                        CommonVariables.MESSAGE_TEXT = "Cannot assign employee roles to inactive assessment group";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                }

                hfSelectedAssessmentGroup.Value = String.Empty;
                clearFields();
                loadGrid();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                dtEmployeeRoles.Dispose();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("btnClear_Click()");

                hfSelectedAssessmentGroup.Value = String.Empty;
                Utility.Errorhandler.ClearError(lblerror);
                clearFields();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void grdvAssessmentGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                log.Debug("grdvAssessmentGroup_SelectedIndexChanged()");

                Utility.Errorhandler.ClearError(lblerror);
                clearFields();
                hfSelectedAssessmentGroup.Value = HttpUtility.HtmlDecode(grdvAssessmentGroup.Rows[grdvAssessmentGroup.SelectedIndex].Cells[0].Text);

                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

                txtGroupName.Text = HttpUtility.HtmlDecode(grdvAssessmentGroup.Rows[grdvAssessmentGroup.SelectedIndex].Cells[1].Text);
                txtDescription.Text = HttpUtility.HtmlDecode(grdvAssessmentGroup.Rows[grdvAssessmentGroup.SelectedIndex].Cells[2].Text);
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(grdvAssessmentGroup.Rows[grdvAssessmentGroup.SelectedIndex].Cells[3].Text));



                for (int i = 0; i < grdvEmployeeRoles.Rows.Count; i++)
                {
                    string groupID = HttpUtility.HtmlDecode(grdvEmployeeRoles.Rows[i].Cells[1].Text).Replace(" ", String.Empty).Trim();
                    string roleID = HttpUtility.HtmlDecode(grdvEmployeeRoles.Rows[i].Cells[0].Text).Replace(" ", String.Empty).Trim();

                    if (groupID != "")
                    {
                        if (groupID != hfSelectedAssessmentGroup.Value)
                        {
                            CheckBox chkIsInclude = (grdvEmployeeRoles.Rows[i].FindControl("chkInclude") as CheckBox);
                            chkIsInclude.Checked = true;
                            chkIsInclude.Enabled = false;
                            grdvEmployeeRoles.Rows[i].ToolTip = grdvEmployeeRoles.Rows[i].Cells[3].Text + " is already assigned to " + grdvEmployeeRoles.Rows[i].Cells[2].Text;
                        }
                        else
                        {
                            CheckBox chkIsInclude = (grdvEmployeeRoles.Rows[i].FindControl("chkInclude") as CheckBox);
                            chkIsInclude.Checked = true;
                            chkIsInclude.Enabled = true;
                            grdvEmployeeRoles.Rows[i].ToolTip = null;
                        }
                    }
                    else
                    {
                        CheckBox chkIsInclude = (grdvEmployeeRoles.Rows[i].FindControl("chkInclude") as CheckBox);
                        chkIsInclude.Checked = false;
                        chkIsInclude.Enabled = true;
                        grdvEmployeeRoles.Rows[i].ToolTip = null;
                    }
                    //set temp checked check boxes

                    DataRow[] dr = (Session["dtEmployeeRoles"] as DataTable).Copy().Select("ROLE_ID = '" + roleID + "'");

                    if (dr.Length > 0)
                    {
                        foreach (DataRow drt in dr)
                        {
                            CheckBox chkIsInclude = (grdvEmployeeRoles.Rows[i].FindControl("chkInclude") as CheckBox);

                            if ((chkIsInclude.Enabled == true) && (chkIsInclude.Checked == false))
                            {
                                if (drt["IS_SELECTED"].ToString() == Constants.CON_ACTIVE_STATUS)
                                {
                                    //if (hfSelectedAssessmentGroup.Value == drt["GROUP_ID"])
                                    chkIsInclude.Checked = true;
                                }
                                else
                                {
                                    chkIsInclude.Checked = false;
                                }
                            }
                        }
                    }
                }

           
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void grdvAssessmentGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdvAssessmentGroup, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdvAssessmentGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                log.Debug("grdvAssessmentGroup_PageIndexChanging()");

                lblProfiles.Text = String.Empty;
                Utility.Errorhandler.ClearError(lblerror);
                hfSelectedAssessmentGroup.Value = String.Empty;

                AssessmentGroupDataHandler oAGDH = new AssessmentGroupDataHandler();
                DataTable dtAGroups = new DataTable();

                grdvAssessmentGroup.PageIndex = e.NewPageIndex;
                dtAGroups = oAGDH.Populate();
                grdvAssessmentGroup.DataSource = dtAGroups.Copy();
                grdvAssessmentGroup.DataBind();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void grdvEmployeeRoles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Utility.Errorhandler.ClearError(lblerror);
            try
            {
                
            }
            catch (Exception exp)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, lblerror);
            }
            finally
            { 
            
            }
        }

        protected void grdvEmployeeRoles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                log.Debug("grdvEmployeeRoles_PageIndexChanging()");

                grdvEmployeeRoles.PageIndex = e.NewPageIndex;
                grdvEmployeeRoles.DataSource = (Session["dtEmployeeRoles"] as DataTable).Copy();
                grdvEmployeeRoles.DataBind();


                for (int i = 0; i < grdvEmployeeRoles.Rows.Count; i++)
                {
                    string groupID = HttpUtility.HtmlDecode(grdvEmployeeRoles.Rows[i].Cells[1].Text).Replace(" ", String.Empty).Trim();
                    string roleID = HttpUtility.HtmlDecode(grdvEmployeeRoles.Rows[i].Cells[0].Text).Replace(" ", String.Empty).Trim();

                    if (groupID != "")
                    {
                        if (groupID != hfSelectedAssessmentGroup.Value)
                        {
                            CheckBox chkIsInclude = (grdvEmployeeRoles.Rows[i].FindControl("chkInclude") as CheckBox);
                            chkIsInclude.Checked = true;
                            chkIsInclude.Enabled = false;
                            grdvEmployeeRoles.Rows[i].ToolTip = grdvEmployeeRoles.Rows[i].Cells[3].Text + " is already assigned to " + grdvEmployeeRoles.Rows[i].Cells[2].Text;
                        }
                        else
                        {
                            DataRow[] drSelect = (Session["dtEmployeeRoles"] as DataTable).Copy().Select("ROLE_ID = '" + roleID + "'");

                            CheckBox chkIsInclude = (grdvEmployeeRoles.Rows[i].FindControl("chkInclude") as CheckBox);
                            grdvEmployeeRoles.Rows[i].ToolTip = null;

                            foreach(DataRow drt in drSelect)
                            {
                                if (drt["IS_SELECTED"].ToString() == Constants.CON_ACTIVE_STATUS)
                                {
                                    chkIsInclude.Checked = true;
                                    chkIsInclude.Enabled = true;
                                }
                                else
                                {
                                    chkIsInclude.Checked = false;
                                    chkIsInclude.Enabled = true;
                                }
                            }                            
                        }
                    }
                    else
                    {
                        CheckBox chkIsInclude = (grdvEmployeeRoles.Rows[i].FindControl("chkInclude") as CheckBox);
                        chkIsInclude.Checked = false;
                        chkIsInclude.Enabled = true;
                        grdvEmployeeRoles.Rows[i].ToolTip = null;
                    }
                    //set temp checked check boxes

                    DataRow[] dr = (Session["dtEmployeeRoles"] as DataTable).Copy().Select("ROLE_ID = '" + roleID + "'");
                    
                    if (dr.Length > 0)
                    {
                        foreach (DataRow drt in dr)
                        {
                            CheckBox chkIsInclude = (grdvEmployeeRoles.Rows[i].FindControl("chkInclude") as CheckBox);
                            
                            if ((chkIsInclude.Enabled == true) && (chkIsInclude.Checked == false))
                            {
                                if (drt["IS_SELECTED"].ToString() == Constants.CON_ACTIVE_STATUS)
                                {
                                    //if (hfSelectedAssessmentGroup.Value == drt["GROUP_ID"])
                                    chkIsInclude.Checked = true;
                                }
                                else
                                {
                                    chkIsInclude.Checked = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                
            }
        }

        protected void chkInclude_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dtEmployeeRoles = new DataTable();
            try
            {
                log.Debug("chkInclude_CheckedChanged()");

                dtEmployeeRoles = (Session["dtEmployeeRoles"] as DataTable).Copy();
                for (int i = 0; i < grdvEmployeeRoles.Rows.Count; i++)
                {
                    CheckBox chkIsInclude = (grdvEmployeeRoles.Rows[i].FindControl("chkInclude") as CheckBox);

                    string roleID = HttpUtility.HtmlDecode(grdvEmployeeRoles.Rows[i].Cells[0].Text).Replace(" ", String.Empty).Trim();


                    string groupID = hfSelectedAssessmentGroup.Value;

                    if (groupID == String.Empty)
                    {
                        groupID = HttpUtility.HtmlDecode(grdvEmployeeRoles.Rows[i].Cells[1].Text).Replace(" ", String.Empty).Trim();
                    }

                    if (chkIsInclude.Checked == true)
                    {

                        if (groupID != "")
                        {
                            DataRow[] drRoles = dtEmployeeRoles.Select("ROLE_ID = '" + roleID + "'");
                            if (drRoles.Length > 0)
                            {
                                foreach (DataRow drt in drRoles)
                                {

                                    if (drt["GROUP_ID"].ToString() == "")
                                    {
                                        drt["GROUP_ID"] = groupID;
                                    }
                                    drt["IS_SELECTED"] = Constants.CON_ACTIVE_STATUS;
                                }
                            }
                        }
                        else
                        {
                            DataRow[] drRoles = dtEmployeeRoles.Select("ROLE_ID = '" + roleID + "'");
                            if (drRoles.Length > 0)
                            {
                                foreach (DataRow drt in drRoles)
                                {
                                    drt["IS_SELECTED"] = Constants.CON_ACTIVE_STATUS;
                                }
                            }
                        }
                    }
                    else
                    {
                        DataRow[] drRoles = dtEmployeeRoles.Select("ROLE_ID = '" + roleID + "'");
                        if (drRoles.Length > 0)
                        {
                            foreach (DataRow drt in drRoles)
                            {
                                drt["IS_SELECTED"] = Constants.CON_INACTIVE_STATUS;
                            }
                        }
                    }
                }
                Session["dtEmployeeRoles"] = dtEmployeeRoles.Copy();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                dtEmployeeRoles.Dispose();
            }
        }

        #endregion

        #region Methods

        void loadGrid()
        {
            try
            {
                log.Debug("loadGrid()");

                //Utility.Errorhandler.ClearError(lblerror);

                AssessmentGroupDataHandler oAGDH = new AssessmentGroupDataHandler();
                DataTable dtAGroups = new DataTable();

                dtAGroups = oAGDH.Populate();

                grdvAssessmentGroup.DataSource = dtAGroups.Copy();
                grdvAssessmentGroup.DataBind();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }

        }

        void clearFields()
        {
            try
            {
                log.Debug("clearFields()");
                
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                txtGroupName.Text = String.Empty;
                txtDescription.Text = String.Empty;
                lblProfiles.Text = String.Empty;
                ddlStatus.SelectedIndex = 0;
                loadEmployeeRoles();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        void loadStatus()
        {
            log.Debug("loadStatus()");
                
            Utility.Errorhandler.ClearError(lblerror);

            ddlStatus.Items.Add(new ListItem("", ""));
            ddlStatus.Items.Add(new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.STATUS_ACTIVE_VALUE));
            ddlStatus.Items.Add(new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.STATUS_INACTIVE_VALUE));
        }

        void loadEmployeeRoles()
        {
            AssessmentGroupDataHandler AGDH = new AssessmentGroupDataHandler();
            DataTable dtEmployeeRoles = new DataTable();
            try
            {
                log.Debug("loadEmployeeRoles()");
             
                dtEmployeeRoles = AGDH.PopulateEmployeeRoles().Copy();
                dtEmployeeRoles.Columns.Add("IS_SELECTED");

                //Checked Already Assigned Roles
                for (int i = 0; i < dtEmployeeRoles.Rows.Count; i++)
                {
                    string GroupID = dtEmployeeRoles.Rows[i]["GROUP_ID"].ToString().Trim();
                    if (GroupID != "")
                    {
                        dtEmployeeRoles.Rows[i]["IS_SELECTED"] = Constants.CON_ACTIVE_STATUS;
                    }
                    else
                    {
                        dtEmployeeRoles.Rows[i]["IS_SELECTED"] = Constants.CON_INACTIVE_STATUS;
                    }
                }
                //

                Session["dtEmployeeRoles"] = dtEmployeeRoles.Copy();
                
                grdvEmployeeRoles.DataSource = dtEmployeeRoles.Copy();
                grdvEmployeeRoles.DataBind();


                for (int i = 0; i < grdvEmployeeRoles.Rows.Count; i++)
                {
                    string groupID = HttpUtility.HtmlDecode(grdvEmployeeRoles.Rows[i].Cells[1].Text).Replace(" ", String.Empty).Trim();
                    
                    if (groupID != "")
                    {
                        CheckBox chkIsInclude = (grdvEmployeeRoles.Rows[i].FindControl("chkInclude") as CheckBox);
                        chkIsInclude.Checked = true;
                        chkIsInclude.Enabled = false;
                        grdvEmployeeRoles.Rows[i].ToolTip = grdvEmployeeRoles.Rows[i].Cells[3].Text + " is already assigned to " + grdvEmployeeRoles.Rows[i].Cells[2].Text;
                    }
                    else
                    {
                        CheckBox chkIsInclude = (grdvEmployeeRoles.Rows[i].FindControl("chkInclude") as CheckBox);
                        chkIsInclude.Checked = false;
                        chkIsInclude.Enabled = true;
                        grdvEmployeeRoles.Rows[i].ToolTip = null;
                    }
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                AGDH = null;
                dtEmployeeRoles.Dispose();
            }
        }

        Boolean isSpecialCharacterExists()
        {
            Boolean Status = false;
            string SpecialChars = String.Empty;
            string MainString = String.Empty;
            try
            {
                log.Debug("isSpecialCharacterExists()");
             
                SpecialChars = @"!@#$%^*()_+=][{}\|`~<>?";
                MainString = txtGroupName.Text.Trim();

                for (int i = 0; i < SpecialChars.Length; i++)
                {
                    if (MainString.Contains(SpecialChars[i]))
                    {
                        Status = true;
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                Status = true;
                throw exp;
            }
            finally
            {
                SpecialChars = String.Empty;
                MainString = String.Empty;
            }
            return Status;
        }

        Boolean isActiveAssessmentsExists()
        {
            lblProfiles.Text = String.Empty;
            Boolean Status = false;
            AssessmentGroupDataHandler AGDH = new AssessmentGroupDataHandler();
            DataTable dtCompetencyProfile = new DataTable();
            DataTable dtSelfAssessmentProfile = new DataTable();
            String GroupID = String.Empty;
            try
            {
                log.Debug("isActiveAssessmentsExists()");
             
                GroupID = grdvAssessmentGroup.Rows[grdvAssessmentGroup.SelectedIndex].Cells[0].Text.Trim();
                dtCompetencyProfile = AGDH.PopulateActiveCompetencyProfiles(GroupID).Copy();
                dtSelfAssessmentProfile = AGDH.PopulateActiveSelfAssessmentProfiles(GroupID).Copy();

                if (dtCompetencyProfile.Rows.Count > 0)
                {
                    Status = true;
                    lblProfiles.Text += @"Active Competency Assessment Profile(s)<ul>";
                    for (int i = 0; i < dtCompetencyProfile.Rows.Count; i++)
                    {
                        lblProfiles.Text += @"<li>" + dtCompetencyProfile.Rows[i]["PROFILE_NAME"].ToString()+@"</li>";
                    }
                    lblProfiles.Text += @"</ul>";
                }


                if (dtSelfAssessmentProfile.Rows.Count > 0)
                {
                    Status = true;
                    lblProfiles.Text += @"Active Self Assessment Profile(s)<ul>";
                    for (int i = 0; i < dtSelfAssessmentProfile.Rows.Count; i++)
                    {
                        lblProfiles.Text += @"<li>" + dtSelfAssessmentProfile.Rows[i]["PROFILE_NAME"].ToString() + @"</li>";
                    }
                    lblProfiles.Text += @"</ul>";
                }
            }
            catch (Exception exp)
            {
                Status = true;
                throw exp;
            }
            finally
            {
                dtCompetencyProfile.Dispose();
                dtSelfAssessmentProfile.Dispose();
                GroupID = String.Empty;
            }
            return Status;
        }

        #endregion
    }
}