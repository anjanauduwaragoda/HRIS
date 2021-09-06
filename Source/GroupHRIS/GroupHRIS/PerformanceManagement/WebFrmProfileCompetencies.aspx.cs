using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.PerformanceManagement;
using System.Data;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmProfileCompetencies : System.Web.UI.Page
    {
        public int selectedPage { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
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
                fillCompetencyGroupDropDown();
                if (Session["AllCompetencies"] == null)
                {
                    resetAllCompetenciesSession();
                    getAllCompetencies();                    
                }
                getCompetenciesForSelectedGroup();
                fillProficiencySchemeTable();
            }            
        }

        #region events

        protected void ddlCompetencyGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string groupId = ddlCompetencyGroup.SelectedValue.ToString().Trim();
            getCompetenciesForSelectedGroup();
        }

        protected void gvCompetency_RowDataBound(object sender, GridViewRowEventArgs e)
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
                string rate =selectedRow[0][4].ToString();
                string weight = selectedRow[0][5].ToString();
                string include = selectedRow[0][6].ToString();
                if (rate != "0" && rate != "")
                {
                    DropDownList proficiencyLevel = (e.Row.FindControl("ddlProficiencyLevels") as DropDownList);
                    proficiencyLevel.SelectedValue = weight;
                }
                if (include == "1")
                {
                    CheckBox includeChildCheckBox = (e.Row.FindControl("includeChildCheckBox") as CheckBox);
                    includeChildCheckBox.Checked = true;
                }

            }
        }

        protected void gvCompetency_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCompetency.PageIndex = e.NewPageIndex;
            getCompetenciesForSelectedGroup();
        }

        protected void ddlProficiencyLevels_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlLevelSelect = (DropDownList)sender;
            string selectedWeight = ddlLevelSelect.SelectedValue.ToString();
            string selectedRate = ddlLevelSelect.SelectedItem.Text.ToString();

            GridViewRow gvr = (GridViewRow)ddlLevelSelect.NamingContainer;
            string competencyId = gvCompetency.Rows[gvr.RowIndex].Cells[0].Text;

            DataTable competencies = (Session["AllCompetencies"] as DataTable).Copy();
            DataRow[] selectedCompetency = competencies.Select("COMPETENCY_ID ='" + competencyId + "'");
            selectedCompetency[0][4] = selectedRate;
            selectedCompetency[0][5] = selectedWeight;
            if (selectedRate != "")
            {
                selectedCompetency[0][6] = "1";
            }
            else
            {
                selectedCompetency[0][6] = "0";
            }

            Session["AllCompetencies"] = competencies;
            //gvCompetency.DataSource = competencies;
            //gvCompetency.DataBind();
            getCompetenciesForSelectedGroup();
        }

        protected void includeChildCheckBox_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox includeCheck = (CheckBox)sender;
            GridViewRow gvr = (GridViewRow)includeCheck.NamingContainer;
            string competencyId = gvCompetency.Rows[gvr.RowIndex].Cells[0].Text;
            
            DataTable allCompetencies = (Session["AllCompetencies"] as DataTable).Copy();
            DataRow[] selectedRow = allCompetencies.Select("COMPETENCY_ID = '" + competencyId + "'");

            if (includeCheck.Checked == true)
            {                
                string rate = selectedRow[0][4].ToString();
                string weight = selectedRow[0][5].ToString();

                if (rate != "0" && weight != "0" && rate != "" && weight != "")
                {
                    selectedRow[0][6] = "1";
                }
                else
                {
                    includeCheck.Checked = false;
                }
            }
            else if (includeCheck.Checked == false)
            {
                selectedRow[0][4] = "0";
                selectedRow[0][5] = "0";
                selectedRow[0][6] = "0";
            }

            Session["AllCompetencies"] = allCompetencies;

            getCompetenciesForSelectedGroup();

        }

        protected void btnAddCompetencies_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region methodes

        private void fillCompetencyGroupDropDown()
        {
            CompetencyProfileDataHandler competencyProfileDataHandler = new CompetencyProfileDataHandler();
            DataTable dtCompetencyGroups = competencyProfileDataHandler.getAllActiveCompetencyGroups();

            ddlCompetencyGroup.Items.Add(new ListItem("", ""));

            for (int i = 0; i < dtCompetencyGroups.Rows.Count; i++)
            {
                string text = dtCompetencyGroups.Rows[i]["COMPETENCY_GROUP_NAME"].ToString();
                string value = dtCompetencyGroups.Rows[i]["COMPETENCY_GROUP_ID"].ToString();
                ddlCompetencyGroup.Items.Add(new ListItem(text, value));
            }
            competencyProfileDataHandler = null;
            dtCompetencyGroups.Dispose();
        }

        private void getCompetenciesForSelectedGroup()
        { 

            string groupId = ddlCompetencyGroup.SelectedValue.ToString().Trim();

            DataTable dtCompetencies = new DataTable();

            try
            {
                if (Session["AllCompetencies"] != null)
                {
                    dtCompetencies = (Session["AllCompetencies"] as DataTable).Copy();
                    getAllCompetencies();

                    DataTable allCompetencis = (Session["AllCompetencies"] as DataTable).Copy();
                    foreach (DataRow competency in allCompetencis.Rows)
                    {
                        DataRow[] exsistingRow = dtCompetencies.Select("COMPETENCY_ID ='" + competency[0] + "'");
                        if (exsistingRow.Count() > 0)
                        {
                            //allCompetencis.Rows.Remove(exsistingRow[0]);

                            competency["COMPETENCY_ID"] = exsistingRow[0]["COMPETENCY_ID"].ToString();
                            competency["COMPETENCY_GROUP_ID"] = exsistingRow[0]["COMPETENCY_GROUP_ID"].ToString();
                            competency["COMPETENCY_NAME"] = exsistingRow[0]["COMPETENCY_NAME"].ToString();
                            competency["DESCRIPTION"] = exsistingRow[0]["DESCRIPTION"].ToString();
                            competency["EXPECTED_PROFICIENCY_RATING"] = exsistingRow[0]["EXPECTED_PROFICIENCY_RATING"].ToString();
                            competency["EXPECTED_PROFICIENCY_WEIGHT"] = exsistingRow[0]["EXPECTED_PROFICIENCY_WEIGHT"].ToString();
                            competency["INCLUDE"] = exsistingRow[0]["INCLUDE"].ToString();
                        }
                    }

                    if (!String.IsNullOrEmpty(groupId))
                    {
                        allCompetencis.DefaultView.RowFilter = "COMPETENCY_GROUP_ID ='" + groupId + "'";
                    }

                    if (allCompetencis.Rows.Count > 0)
                    {
                        Session["AllCompetencies"] = allCompetencis;
                        gvCompetency.DataSource = allCompetencis;
                        gvCompetency.DataBind();
                        dtCompetencies.Dispose();
                    }

                    //allCompetencis.Merge(dtCompetencies);
                }
            }
            catch (Exception )
            {
                throw;
            }
            

        }

        private void getAllCompetencies()
        {
            try
            {
                CompetencyProfileDataHandler competencyProfileDataHandler = new CompetencyProfileDataHandler();
                DataTable dtCompetencies = new DataTable();
                dtCompetencies = competencyProfileDataHandler.getAllCompetencies();

                DataTable allCompetencies = (Session["AllCompetencies"] as DataTable).Clone();
                foreach (DataRow competency in dtCompetencies.Rows)
                {
                    DataRow newRow = allCompetencies.NewRow();
                    newRow["COMPETENCY_ID"] = competency["COMPETENCY_ID"].ToString();
                    newRow["COMPETENCY_GROUP_ID"] = competency["COMPETENCY_GROUP_ID"].ToString();
                    newRow["COMPETENCY_NAME"] = competency["COMPETENCY_NAME"].ToString();
                    newRow["DESCRIPTION"] = competency["DESCRIPTION"].ToString();
                    newRow["EXPECTED_PROFICIENCY_RATING"] = "0";
                    newRow["EXPECTED_PROFICIENCY_WEIGHT"] = "0";
                    newRow["INCLUDE"] = "0";
                    allCompetencies.Rows.Add(newRow);
                }

                Session["AllCompetencies"] = allCompetencies;

                competencyProfileDataHandler = null;
                dtCompetencies.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void fillExpectedLevels(DropDownList ddlLevels)
        {
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
            catch (Exception)
            {
                
                throw;
            }
            
        }

        private void resetAllCompetenciesSession()
        {
            DataTable dtCompetencies = new DataTable();

            dtCompetencies.Columns.Add("COMPETENCY_ID", typeof(String));
            dtCompetencies.Columns.Add("COMPETENCY_GROUP_ID", typeof(String));
            dtCompetencies.Columns.Add("COMPETENCY_NAME", typeof(String));
            dtCompetencies.Columns.Add("DESCRIPTION", typeof(String));
            dtCompetencies.Columns.Add("EXPECTED_PROFICIENCY_RATING", typeof(String));
            dtCompetencies.Columns.Add("EXPECTED_PROFICIENCY_WEIGHT", typeof(String));
            dtCompetencies.Columns.Add("INCLUDE", typeof(String));

            Session["AllCompetencies"] = dtCompetencies;
        }

        private void fillProficiencySchemeTable()
        { 
            DataTable levels = (Session["ProficiencyLevels"] as DataTable).Copy();
            //gvProficiencyScheme.DataSource = levels;
            //gvProficiencyScheme.DataBind();

            for (int i = 0; i < levels.Rows.Count; i++)
            {
                string text = levels.Rows[i]["RATING"].ToString();
                string value = levels.Rows[i]["WEIGHT"].ToString();

                Label lblRating = new Label();
                lblRating.Text = " " + text + " " + "-" + " " + value + " ";
                if (i < levels.Rows.Count - 1)
                { 
                   lblRating.Text =  lblRating.Text.ToString()+"| ";
                }
                
                ratingScheme.Controls.Add(lblRating);
                
            }

            
        }
       
        #endregion

        

        



        

      
    }
}