using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using DataHandler.PerformanceManagement;
using GroupHRIS.Utility;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmCompetencyProfile : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        public int selectedPage { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmCompetencyProfile : Page_Load"); 
            string x = HiddenProfileId.Value;

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
                fillStatus();
                fillAssessmentGroup();
                fillProficiencyScheme();
                loadCompetencyProfileGridView();
                Session["AllCompetencies"] = null;
                Session["SelectedCompetencies"] = null;
                loadSelectedCompetenciesGridView();
                Session["statusUpdatable"] = "";
                Session["partiallyUpdatable"] = "";
            }
            if (IsPostBack)
            {

                if (HiddenDataCaptured.Value.ToString() == "1")
                {
                    loadSelectedCompetenciesGridView();
                }
                
            }
        }

        #region parentFormMethodes

        protected void fillStatus()
        {
            try
            {
                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlProfileStatus.Items.Add(listItemBlank);

                ListItem listItemActive = new ListItem();
                listItemActive.Text = Constants.STATUS_ACTIVE_TAG;
                listItemActive.Value = Constants.STATUS_ACTIVE_VALUE;
                
                ddlProfileStatus.Items.Add(listItemActive);

                ListItem listItemInActive = new ListItem();
                listItemInActive.Text = Constants.STATUS_INACTIVE_TAG;
                listItemInActive.Value = Constants.STATUS_INACTIVE_VALUE;
                ddlProfileStatus.Items.Add(listItemInActive);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void fillAssessmentGroup()
        {
            DataTable assessmentGrouptable = new DataTable();
           
            CompetencyProfileDataHandler competencyProfileDataHandler = new CompetencyProfileDataHandler();
            try
            {
                assessmentGrouptable = competencyProfileDataHandler.getActiveAssessmentGroups();

                ddlAssessmentGroup.Items.Add(new ListItem("", ""));

                for (int i = 0; i < assessmentGrouptable.Rows.Count; i++)
                {
                    string text = assessmentGrouptable.Rows[i]["GROUP_NAME"].ToString();
                    string value = assessmentGrouptable.Rows[i]["GROUP_ID"].ToString();
                    ddlAssessmentGroup.Items.Add(new ListItem(text, value));
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);

            }
            finally
            {
                competencyProfileDataHandler = null;
                assessmentGrouptable.Dispose();           
            }

            
        }

        protected void fillProficiencyScheme()
        { 
            CompetencyProfileDataHandler competencyProfileDataHandler = new CompetencyProfileDataHandler();
            DataTable SchemeDataTable = competencyProfileDataHandler.getAllActiveProficiencySchemes();

            try
            {
                ddlProficiencyScheme.Items.Add(new ListItem("", ""));
                if (SchemeDataTable.Rows.Count > 0)
                {
                    foreach (DataRow scheme in SchemeDataTable.Rows)
                    {
                        string text = scheme["SCHEME_NAME"].ToString();
                        string value = scheme["SCHEME_ID"].ToString();
                        ddlProficiencyScheme.Items.Add(new ListItem(text, value));
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                competencyProfileDataHandler = null;
                SchemeDataTable.Dispose();
            }
            
        }

        protected void loadCompetencyProfileGridView()
        { 
            CompetencyProfileDataHandler competencyProfileDataHandler = new CompetencyProfileDataHandler();

            try
            {
                DataTable dtCompetencyProfiles = competencyProfileDataHandler.getAllCompetencyProfiles();
                if (dtCompetencyProfiles.Rows.Count > 0)
                {
                    gvCompetencyProfile.Width = 850;
                    gvCompetencyProfile.DataSource = dtCompetencyProfiles;
                    gvCompetencyProfile.DataBind();
                    dtCompetencyProfiles.Dispose();
                }
                else
                {
                    gvCompetencyProfile.DataSource = null;
                    gvCompetencyProfile.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);               
            }
            finally
            {
                competencyProfileDataHandler = null;
            }
        }

        protected void clear()
        {
            try
            {
                Utility.Utils.clearControls(true, ddlAssessmentGroup, ddlProficiencyScheme, ddlProfileStatus, txtName, txtDescription);

                HiddenDataCaptured.Value = "";
                HiddenProfileId.Value = "";
                Session["ProficiencyLevels"] = null;
                Session["AllCompetencies"] = null;
                Session["ddlSelectedProficiencyScheme"] = null;
                Session["statusUpdatable"] = "";
                Session["partiallyUpdatable"] = "";
                gvSelectedCompetencies.DataSource = null;
                gvSelectedCompetencies.DataBind();
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected DataTable getCompetenciesByProfileId(string competencyProfileId)
        {
            CompetencyProfileDataHandler competencyProfileDataHandler = new CompetencyProfileDataHandler();
            DataTable dtCompetencies = new DataTable();

            try
            {
                dtCompetencies = competencyProfileDataHandler.getCompetenciesByProfileId(competencyProfileId);
                return dtCompetencies;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                return null;
            }
            finally
            {
                competencyProfileDataHandler = null;
                dtCompetencies.Dispose();
            }
        }

        #endregion
        
        #region parentFormEvents

        protected void ddlProficiencyScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlProficiencyScheme_SelectedIndexChanged()");
            try
            {
                Session["AllCompetencies"] = null;
                loadSelectedCompetenciesGridView();

                string schemeId = ddlProficiencyScheme.SelectedValue.ToString();
                Session["ddlSelectedProficiencyScheme"] = schemeId;
                getProficiencyLevelsForScheme(schemeId);
                Utility.Errorhandler.ClearError(lblErrorMsg);

                
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                
            }
        }

        protected void getProficiencyLevelsForScheme(string schemeId)
        {
            log.Debug("getProficiencyLevelsForScheme()");

            CompetencyProfileDataHandler competencyProfileDataHandler = new CompetencyProfileDataHandler();
            DataTable proficiencyLevels = new DataTable();
            try
            {
                proficiencyLevels = competencyProfileDataHandler.getProficiencyLevels(schemeId);
                Session["ProficiencyLevels"] = proficiencyLevels;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                competencyProfileDataHandler = null;
                proficiencyLevels.Dispose();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            string name = txtName.Text.ToString();
            string description = txtDescription.Text.ToString();
            string proficiencySchmId = ddlProficiencyScheme.SelectedValue.ToString();
            string assessmentGroupId = ddlAssessmentGroup.SelectedValue.ToString();
            string status = ddlProfileStatus.SelectedValue.ToString();
            string addedUserId = Session["KeyUSER_ID"].ToString();
            DataTable competencies = getIncludedCompetencies();
            CompetencyProfileDataHandler competencyProfileDataHandler = new CompetencyProfileDataHandler();
            
            try
            {

                if (competencies.Rows.Count > 0)
                {
                    if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                    {
                        

                        bool assessmentGroupIsUsed = competencyProfileDataHandler.CheckUsageOfAssessmentGroup(assessmentGroupId);
                        if (assessmentGroupIsUsed)
                        {
                            CommonVariables.MESSAGE_TEXT = "Assessment Group already used";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }
                        else
                        {
                            bool nameExists = competencyProfileDataHandler.CheckCompetencyProfileNameExsistance(name);

                            if (nameExists)
                            {
                                CommonVariables.MESSAGE_TEXT = "Profile name already exists";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            }
                            else
                            {
                                bool isInserted = competencyProfileDataHandler.insert(name, description, proficiencySchmId, assessmentGroupId, status, competencies, addedUserId);
                                if (isInserted)
                                {
                                    CommonVariables.MESSAGE_TEXT = "Record(s) successfully saved";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                    clear();
                                }
                            }
                        }
                        //competencyProfileDataHandler = null;
                    }

                    if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {
                       // CompetencyProfileDataHandler competencyProfileDataHandler = new CompetencyProfileDataHandler();

                        //string profileId = HiddenProfileId.Value.ToString();

                        int selectedIndex = gvCompetencyProfile.SelectedIndex;
                        string profileId = gvCompetencyProfile.Rows[selectedIndex].Cells[0].Text.ToString();

                        string isStatusUpdatable = Session["statusUpdatable"].ToString();
                        string isPartiallyUpdatable = Session["partiallyUpdatable"].ToString();


                        if (isPartiallyUpdatable == "False") /// competency profile is never been used. user can change any thing 
                        {

                            bool assessmentGroupIsUsed = competencyProfileDataHandler.CheckUsageOfAssessmentGroup(assessmentGroupId, profileId);
                            if (assessmentGroupIsUsed)
                            {
                                CommonVariables.MESSAGE_TEXT = "Assessment Group already used";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            }
                            else
                            {
                                bool nameExists = competencyProfileDataHandler.CheckCompetencyProfileNameExsistance(name, profileId);

                                if (nameExists)
                                {
                                    CommonVariables.MESSAGE_TEXT = "Profile name already exists";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                }
                                else
                                {
                                    ProficiencySchemeDataHandler proficiencySchemeDataHandler = new ProficiencySchemeDataHandler();
                                    DataRow proficiencySchemeDetails = proficiencySchemeDataHandler.PopulateSchemes(proficiencySchmId);
                                    if (proficiencySchemeDetails[3].ToString() == Constants.STATUS_ACTIVE_VALUE)
                                    {

                                        DataRow assessmentGroupDetail = competencyProfileDataHandler.getAssessmentGroupById(assessmentGroupId);
                                        if (assessmentGroupDetail[0].ToString() == Constants.STATUS_ACTIVE_VALUE)
                                        {

                                            bool isUpdated = competencyProfileDataHandler.update(profileId, name, description, proficiencySchmId, assessmentGroupId, status, competencies, addedUserId);
                                            if (isUpdated)
                                            {
                                                CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated";
                                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                                clear();
                                            }
                                            else
                                            {
                                                CommonVariables.MESSAGE_TEXT = "Could not update the record ";
                                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                            }
                                        }
                                        else
                                        {
                                            CommonVariables.MESSAGE_TEXT = " Please select an active assessment group";
                                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                        }
                                    }
                                    else
                                    {
                                        CommonVariables.MESSAGE_TEXT = " Please select an active proficiency scheme";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                    }
                                    proficiencySchemeDataHandler = null;
                                    proficiencySchemeDetails = null;

                                }
                            }
                            //competencyProfileDataHandler = null;
                        }
                        else if(isPartiallyUpdatable == "True") // either name or status or both can change
                        {
                            if (isStatusUpdatable == "True") /// all assessments closed. user can make it inactive or change the name
                            {
                                bool nameExists = competencyProfileDataHandler.CheckCompetencyProfileNameExsistance(name, profileId);

                                if (nameExists)
                                {
                                    CommonVariables.MESSAGE_TEXT = "Profile name already exists";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                }
                                else
                                {
                                    bool isUpdated = competencyProfileDataHandler.update(profileId, name, status, addedUserId);
                                    if (isUpdated)
                                    {
                                        CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                        clear();

                                    }
                                    else
                                    {
                                        CommonVariables.MESSAGE_TEXT = "Could not update the record ";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                    }
                                }
                            }
                            else if (isStatusUpdatable == "False") // ongoing assessments availabale, user can only change the name
                            {
                                if (status == Constants.STATUS_INACTIVE_VALUE)
                                {
                                    CommonVariables.MESSAGE_TEXT = "Not allowed to make inactive. Active assessments exist";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                }
                                else
                                {

                                    bool isUpdated = competencyProfileDataHandler.update(profileId, name, addedUserId);
                                    if (isUpdated)
                                    {
                                        CommonVariables.MESSAGE_TEXT = "Record(s) successfully updated";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                        clear();

                                    }
                                    else
                                    {
                                        CommonVariables.MESSAGE_TEXT = "Could not update the record ";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Competencies are required";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                    loadSelectedCompetenciesGridView();
                }
            }
            catch (Exception ex)
            {

                //throw;
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                clear();
            }
            finally
            {
                competencyProfileDataHandler = null;
            }
            loadCompetencyProfileGridView();
            
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");
            Utility.Errorhandler.ClearError(lblErrorMsg);
            clear();
        }

        protected void gvCompetencyProfile_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvCompetencyProfile_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvCompetencyProfile, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void gvCompetencyProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gvCompetencyProfile_SelectedIndexChanged()");
            
            try
            {
                clear();

                ddlProficiencyScheme.Items.Clear();
                fillProficiencyScheme();

                ddlAssessmentGroup.Items.Clear();
                fillAssessmentGroup();

                Utility.Errorhandler.ClearError(lblErrorMsg);
                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                int selectedIndex = gvCompetencyProfile.SelectedIndex;
                string selectedProfileId = gvCompetencyProfile.Rows[selectedIndex].Cells[0].Text.ToString();
                HiddenProfileId.Value = selectedProfileId;
                ///////// start fill form fields //////
                txtName.Text = gvCompetencyProfile.Rows[selectedIndex].Cells[1].Text.ToString();


                //////// begin assessment group ddl /////

                //ddlAssessmentGroup.SelectedValue = gvCompetencyProfile.Rows[selectedIndex].Cells[2].Text.ToString();

                string groupId = gvCompetencyProfile.Rows[selectedIndex].Cells[2].Text.ToString();
                string groupName = gvCompetencyProfile.Rows[selectedIndex].Cells[3].Text.ToString();

                bool groupIsExist = Utils.isValueExistInDropDownList(groupId, ddlAssessmentGroup);

                if (!groupIsExist)
                {

                    addInactiveProficiencyScheme(ddlAssessmentGroup, groupName, groupId);
                }
                else
                {
                    ddlAssessmentGroup.SelectedValue = groupId;
                }
                //////// end assessment group ddl /////

                //////// begin Proficiency Scheme ddl /////
                string proficiencySchemeId = gvCompetencyProfile.Rows[selectedIndex].Cells[4].Text.ToString();
                string proficiencySchemeName = gvCompetencyProfile.Rows[selectedIndex].Cells[5].Text.ToString();

                bool schemeIsExist = Utils.isValueExistInDropDownList(proficiencySchemeId, ddlProficiencyScheme);

                if (!schemeIsExist)
                {
                    
                    addInactiveProficiencyScheme(ddlProficiencyScheme, proficiencySchemeName, proficiencySchemeId);
                }
                else
                {
                    ddlProficiencyScheme.SelectedValue = proficiencySchemeId;
                }

                //ddlProficiencyScheme.SelectedValue = proficiencySchemeId;

                /////////////////// end aProficiency Scheme ddl /////

                txtDescription.Text = HttpUtility.HtmlDecode(gvCompetencyProfile.Rows[selectedIndex].Cells[6].Text.ToString());

                string status = gvCompetencyProfile.Rows[selectedIndex].Cells[7].Text.ToString();
                if (status == Constants.STATUS_ACTIVE_TAG)
                {
                    ddlProfileStatus.SelectedValue = Constants.STATUS_ACTIVE_VALUE;
                }
                else if (status == Constants.STATUS_INACTIVE_TAG)
                {
                    ddlProfileStatus.SelectedValue = Constants.STATUS_INACTIVE_VALUE;
                }

                //////// end fill form fields ///////////

                DataTable dtCompetenciesForSelectedProfile = getCompetenciesByProfileId(selectedProfileId);
                Session["AllCompetencies"] = dtCompetenciesForSelectedProfile;
                Session["ddlSelectedProficiencyScheme"] = proficiencySchemeId;
                getProficiencyLevelsForScheme(proficiencySchemeId);
                bindDataToCompetenciesGridView(dtCompetenciesForSelectedProfile);

                CompetencyProfileDataHandler competencyProfileDataHandler = new CompetencyProfileDataHandler();
                DataTable dtUsedCompetencyAssessments = competencyProfileDataHandler.getUsedCompetencyAssessments(selectedProfileId);

                if (dtUsedCompetencyAssessments != null)
                {
                    Session["partiallyUpdatable"] = "False";
                    Session["statusUpdatable"] = "";

                    if (dtUsedCompetencyAssessments.Rows.Count > 0)
                    {
                        bool partiallyUpdatable = true;
                        bool statusUpdatable = true;
                        //bool nameUpdatable = false;
                        foreach (DataRow competencyAssessment in dtUsedCompetencyAssessments.Rows)
                        {
                            if (competencyAssessment[3] != Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                            {
                                statusUpdatable = false;
                            }
                        }

                        Session["partiallyUpdatable"] = partiallyUpdatable;
                        Session["statusUpdatable"] = statusUpdatable;

                    }
                    
                }
                competencyProfileDataHandler = null;
                dtUsedCompetencyAssessments.Dispose();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }

        }

        //private void disableFields()
        //{
        //    ddlProficiencyScheme.Enabled = false;
        //    ddlAssessmentGroup.Enabled = false;
        //    LinkButton1.Enabled = false;
        //    //gvSelectedCompetencies.Enabled = false;
        //    //gvSelectedCompetencies.EnableSortingAndPagingCallbacks = true;
        //    CheckBox exclude = gvSelectedCompetencies.FindControl("excludeChildCheckBox") as CheckBox;
        //    exclude.Enabled = false;
        //}
        private void addInactiveProficiencyScheme(DropDownList ddlName, string itemText, string itemValue)
        {
            log.Debug("addInactiveProficiencyScheme()");
            try
            {
                ListItem newItem = new ListItem();
                newItem.Text = itemText;
                newItem.Value = itemValue;
                //newItem.Attributes.Add("disabled", "disabled");
                ddlName.Items.Add(newItem);
                ddlName.SelectedValue = itemValue;
                // ddlName.Items[ddlName.Items.Count - 1].Attributes.Add("disabled", "disabled");
                //newItem.Attributes.Add("disabled", "disabled");
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }

        }

        protected void gvCompetencyProfile_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvCompetencyProfile_PageIndexChanging()");
            gvCompetencyProfile.PageIndex = e.NewPageIndex;
            loadCompetencyProfileGridView();
        }

        protected void LinkButtonSelectCompetencies_Click(object sender, EventArgs e)
        {
            log.Debug("LinkButtonSelectCompetencies_Click()");
            try
            {
                string schemeId = ddlProficiencyScheme.SelectedValue.ToString();

                if (String.IsNullOrEmpty(schemeId))
                {
                    CommonVariables.MESSAGE_TEXT = " Select a proficiency scheme ";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        #endregion

        #region SelectedCompetenciesGridView

        protected void ddlProficiencyLevels_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlProficiencyLevels_SelectedIndexChanged()");
            try
            {
                DropDownList ddlLevelSelect = (DropDownList)sender;
                string selectedWeight = ddlLevelSelect.SelectedValue.ToString();
                string selectedRate = ddlLevelSelect.SelectedItem.Text.ToString();

                GridViewRow gvr = (GridViewRow)ddlLevelSelect.NamingContainer;
                string competencyId = gvSelectedCompetencies.Rows[gvr.RowIndex].Cells[0].Text;

                DataTable competencies = (Session["AllCompetencies"] as DataTable).Copy();
                DataRow[] selectedCompetency = competencies.Select("COMPETENCY_ID ='" + competencyId + "'");
                selectedCompetency[0][4] = selectedRate;

                if (String.IsNullOrEmpty(selectedWeight))
                {
                    selectedCompetency[0][5] = DBNull.Value;
                }
                else
                {
                    selectedCompetency[0][5] = selectedWeight;
                }
                if (selectedRate != "")
                {
                    selectedCompetency[0][6] = "1";
                }
                else
                {
                    selectedCompetency[0][6] = "0";
                }

                Session["AllCompetencies"] = competencies;

                loadSelectedCompetenciesGridView();
                competencies.Dispose();
            }
            catch (Exception ex)
            {
                
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            
        }

        protected void gvCompetency_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvCompetency_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string competencyId = e.Row.Cells[0].Text.ToString();
                    int index = e.Row.RowIndex;
                    int pageNo = selectedPage;
                    int dataTableIndex = index + (pageNo) * 10;

                    DropDownList ddlLevels = (e.Row.FindControl("ddlProficiencyLevels") as DropDownList);
                    fillExpectedLevels(ddlLevels);

                    DataTable allCompetencies = (Session["AllCompetencies"] as DataTable).Copy();

                    DataRow[] selectedRow = allCompetencies.Select("COMPETENCY_ID ='" + competencyId + "'");
                    string rate = selectedRow[0][4].ToString();
                    string weight = selectedRow[0][5].ToString();
                    string include = selectedRow[0][6].ToString();
                    if (rate != "0" && rate != "")
                    {
                        DropDownList proficiencyLevel = (e.Row.FindControl("ddlProficiencyLevels") as DropDownList);
                        proficiencyLevel.SelectedValue = weight;
                    }
                    if (include == "1")
                    {
                        CheckBox excludeChildCheckBox = (e.Row.FindControl("excludeChildCheckBox") as CheckBox);
                        excludeChildCheckBox.Checked = false;
                    }
                    else if (include == "0")
                    {
                        CheckBox excludeChildCheckBox = (e.Row.FindControl("excludeChildCheckBox") as CheckBox);
                        excludeChildCheckBox.Checked = true;
                    }
                    allCompetencies.Dispose();
                }
            }
            catch (Exception ex)
            {
                
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void gvCompetency_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvCompetency_PageIndexChanging()");
            try
            {
                gvSelectedCompetencies.PageIndex = e.NewPageIndex;
                //loadSelectedCompetenciesGridView();
                //checkExcludeChildCheckBoxes();

                DataTable dtSlectetCompetencies = (Session["AllCompetencies"] as DataTable).Copy();
                bindDataToCompetenciesGridView(dtSlectetCompetencies);
                dtSlectetCompetencies.Dispose();
            }
            catch (Exception ex)
            {

                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void fillExpectedLevels(DropDownList ddlLevels)
        {
            log.Debug("fillExpectedLevels()");
            try
            {
                DataTable levels = (Session["ProficiencyLevels"] as DataTable).Copy();

                ddlLevels.Items.Add(new ListItem("", ""));

                for (int i = 0; i < levels.Rows.Count; i++)
                {
                    string text = levels.Rows[i]["RATING"].ToString();
                    string value = levels.Rows[i]["WEIGHT"].ToString();

                    ddlLevels.Items.Add(new ListItem(text, value));
                }

                levels.Dispose();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }

        }

        protected DataTable getIncludedCompetencies()
        {
            log.Debug("getIncludedCompetencies()");
            
            DataTable allCompetencies = new DataTable();
            try
            {
                if (Session["AllCompetencies"] != null)
                {
                    allCompetencies = (Session["AllCompetencies"] as DataTable).Copy();

                    DataTable dtSlectetCompetencies = new DataTable();
                    dtSlectetCompetencies.Columns.Add("COMPETENCY_ID", typeof(String));
                    dtSlectetCompetencies.Columns.Add("COMPETENCY_GROUP_ID", typeof(String));
                    dtSlectetCompetencies.Columns.Add("COMPETENCY_NAME", typeof(String));
                    dtSlectetCompetencies.Columns.Add("DESCRIPTION", typeof(String));
                    dtSlectetCompetencies.Columns.Add("EXPECTED_PROFICIENCY_RATING", typeof(String));
                    dtSlectetCompetencies.Columns.Add("EXPECTED_PROFICIENCY_WEIGHT", typeof(String));
                    dtSlectetCompetencies.Columns.Add("INCLUDE", typeof(String));

                    foreach (DataRow competency in allCompetencies.Rows)
                    {
                        if (competency["INCLUDE"].ToString() == "1")
                        {
                            DataRow newRow = dtSlectetCompetencies.NewRow();
                            newRow[0] = competency[0].ToString();
                            newRow[1] = competency[1].ToString();
                            newRow[2] = competency[2].ToString();
                            newRow[3] = competency[3].ToString();
                            newRow[4] = competency[4].ToString();
                            newRow[5] = competency[5].ToString();
                            newRow[6] = competency[6].ToString();

                            dtSlectetCompetencies.Rows.Add(newRow);
                        }
                    }
                    Session["AllCompetencies"] = dtSlectetCompetencies;
                    return dtSlectetCompetencies;
                }
                else
                {
                    DataTable dtSlectetCompetencies = new DataTable();
                    dtSlectetCompetencies.Columns.Add("COMPETENCY_ID", typeof(String));
                    dtSlectetCompetencies.Columns.Add("COMPETENCY_GROUP_ID", typeof(String));
                    dtSlectetCompetencies.Columns.Add("COMPETENCY_NAME", typeof(String));
                    dtSlectetCompetencies.Columns.Add("DESCRIPTION", typeof(String));
                    dtSlectetCompetencies.Columns.Add("EXPECTED_PROFICIENCY_RATING", typeof(String));
                    dtSlectetCompetencies.Columns.Add("EXPECTED_PROFICIENCY_WEIGHT", typeof(String));
                    dtSlectetCompetencies.Columns.Add("INCLUDE", typeof(String));
                    return dtSlectetCompetencies;
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                return null;
            }
            finally
            {
                allCompetencies.Dispose();
            }
        }

        protected void loadSelectedCompetenciesGridView()
        {
            log.Debug("loadSelectedCompetenciesGridView()");

            try
            {
                if (Session["AllCompetencies"] != null)
                {
                    DataTable dtSlectetCompetencies = getIncludedCompetencies();
                    //DataTable dtSlectetCompetencies = (Session["AllCompetencies"] as DataTable).Copy();
                    bindDataToCompetenciesGridView(dtSlectetCompetencies);
                    HiddenDataCaptured.Value = "0";
                }
                else
                {
                    gvSelectedCompetencies.DataSource = null;
                    gvSelectedCompetencies.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void bindDataToCompetenciesGridView(DataTable data)
        {
            log.Debug("bindDataToCompetenciesGridView()");
            try
            {
                if (data.Rows.Count > 0)
                {
                    gvSelectedCompetencies.Width = 430;
                    gvSelectedCompetencies.DataSource = data;
                    gvSelectedCompetencies.DataBind();
                }
                else 
                {
                    gvSelectedCompetencies.DataSource = null;
                    gvSelectedCompetencies.DataBind();
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        protected void excludeChildCheckBox_OnCheckedChanged(object sender, EventArgs e)
        {
            log.Debug("excludeChildCheckBox_OnCheckedChanged()");
            try
            {
                CheckBox excludeCheck = (CheckBox)sender;
                GridViewRow gvr = (GridViewRow)excludeCheck.NamingContainer;
                string competencyId = gvSelectedCompetencies.Rows[gvr.RowIndex].Cells[0].Text;

                DataTable allCompetencies = (Session["AllCompetencies"] as DataTable).Copy();
                DataRow[] selectedRow = allCompetencies.Select("COMPETENCY_ID = '" + competencyId + "'");

                if (excludeCheck.Checked == false)
                {
                    string rate = selectedRow[0][4].ToString();
                    string weight = selectedRow[0][5].ToString();

                    if (rate != "0" && weight != "0" && rate != "" && weight != "")
                    {
                        selectedRow[0][6] = "1";
                    }
                    else
                    {
                        excludeCheck.Checked = false;
                    }
                }
                else if (excludeCheck.Checked == true)
                {
                    selectedRow[0][4] = "0";
                    selectedRow[0][5] = "0";
                    selectedRow[0][6] = "0";
                }

                Session["AllCompetencies"] = allCompetencies;
                allCompetencies.Dispose();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }

        }

        protected void excludeHeaderCheckBox_OnCheckedChanged(object sender, EventArgs e)
        {
            log.Debug("excludeHeaderCheckBox_OnCheckedChanged()");
            try
            {
                CheckBox excludeCheck = (gvSelectedCompetencies.HeaderRow.FindControl("excludeHeaderCheckBox") as CheckBox);
                DataTable allCompetencies = (Session["AllCompetencies"] as DataTable).Copy();

                foreach (DataRow selectedRow in allCompetencies.Rows)
                {
                    if (excludeCheck.Checked == false)
                    {
                        string rate = selectedRow[4].ToString();
                        string weight = selectedRow[5].ToString();

                        if (rate != "0" && weight != "0" && rate != "" && weight != "")
                        {
                            selectedRow[6] = "1";
                        }
                        else
                        {
                            excludeCheck.Checked = false;
                        }
                    }
                    else if (excludeCheck.Checked == true)
                    {
                        selectedRow[4] = "0";
                        selectedRow[5] = "0";
                        selectedRow[6] = "0";
                    }
                }
                Session["AllCompetencies"] = allCompetencies;
                gvSelectedCompetencies.DataSource = allCompetencies;
                gvSelectedCompetencies.DataBind();
                //checkExcludeChildCheckBoxes();   
                allCompetencies.Dispose();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }       
        }

        protected void checkExcludeChildCheckBoxes()
        {
            log.Debug("checkExcludeChildCheckBoxes()");
            try
            {
                CheckBox excludeCheck = (gvSelectedCompetencies.HeaderRow.FindControl("excludeHeaderCheckBox") as CheckBox);
                int recordsPerPage = gvSelectedCompetencies.Rows.Count;

                for (int i = 0; i < recordsPerPage; i++)
                {
                    if (excludeCheck.Checked == true)
                    {
                        CheckBox child = (CheckBox)gvSelectedCompetencies.Rows[i].FindControl("excludeChildCheckBox");
                        child.Checked = true;
                    }
                    if (excludeCheck.Checked == false)
                    {
                        CheckBox child = (CheckBox)gvSelectedCompetencies.Rows[i].FindControl("excludeChildCheckBox");
                        child.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }
       
        #endregion

        

        

        

        



    }
}