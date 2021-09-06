using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.Roster;
using DataHandler.MetaData;
using Common;
using NLog;

namespace GroupHRIS.Roster
{
    public partial class webFrmRoster : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static string userId = "";


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["KeyUSER_ID"] != null)
                {
                    userId = Session["KeyUSER_ID"].ToString();
                }

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
                    ddlCompany.SelectedValue = Session["KeyCOMP_ID"].ToString().Trim();
                }

                fillRosterTypes();
                loadHours();
                loadMins();
                loadFlexiHours();
                loadNumDays();
                loadStatus();

            }

            /*
            else
            {
                if(hfCompCode.Value.Trim().Length > 0)
                    ddlCompany.SelectedValue = hfCompCode.Value;
            }
            */

            populateGrid();

        }




        protected void btnSave_Click(object sender, EventArgs e)
        {
            RosterDataHandler dhRoster = new RosterDataHandler();

            Utility.Errorhandler.ClearError(lblMsg);

            try
            {
                TimeSpan tsFrom = new TimeSpan(Int32.Parse(ddlFromTimeHH.SelectedValue.Trim()),Int32.Parse(ddlFromTimeMM.SelectedValue.Trim()),0);
                TimeSpan tsTo = new TimeSpan(Int32.Parse(ddlToTimeHH.SelectedValue), Int32.Parse(ddlToTimeMM.SelectedValue), 0);

                if ((tsTo > tsFrom) && (ddlRosterType.SelectedValue == "2"))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "From Time should be less than To Time in Overnight rosters", lblMsg);
                    return;
                }

                //------------------------------------------------
                //Roster Type <> OverNight AND FromTime < ToTime
                //------------------------------------------------
                //if ( (!ddlRosterType.SelectedValue.Equals(Constants.CON_ROSTER_TYPE_OVER_NIGHT)) && (!CommonUtils.isValidTimeRange(ddlFromTimeHH.SelectedValue + ":" + ddlFromTimeMM.SelectedValue, ddlToTimeHH.SelectedValue + ":" + ddlToTimeMM.SelectedValue)) )
                if ((!ddlRosterType.SelectedValue.Equals(Constants.CON_ROSTER_OVER_NIGHT_CODE)) && (!CommonUtils.isValidTimeRange(ddlFromTimeHH.SelectedValue + ":" + ddlFromTimeMM.SelectedValue, ddlToTimeHH.SelectedValue + ":" + ddlToTimeMM.SelectedValue)))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "From Time should be earlier than To Time in Non-Overnight rosters", lblMsg);
                    return;
                }


                //------------------------------------------------
                //Check Roster Already Exists 
                //------------------------------------------------
                else if ( (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT) && (dhRoster.isAlreadyExists(ddlCompany.SelectedValue.Trim(), ddlFromTimeHH.SelectedValue + ":" + ddlFromTimeMM.SelectedValue, ddlToTimeHH.SelectedValue + ":" + ddlToTimeMM.SelectedValue)))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Roster already exists for this company.", lblMsg);
                    return;
                } 



                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    dhRoster.Insert(ddlCompany.SelectedValue.Trim(),
                                    ddlFromTimeHH.SelectedValue + ":" + ddlFromTimeMM.SelectedValue,
                                    ddlToTimeHH.SelectedValue + ":" + ddlToTimeMM.SelectedValue,
                                    ddlFlexibleHrs.SelectedValue + ":" + ddlFlexibleMins.SelectedValue,
                                    ddlRosterType.SelectedValue,
                                    userId,
                                    ddlStatus.SelectedValue,
                                    ddlNumDays.SelectedValue);

                    clearRest();
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED, lblMsg);
                }

                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    dhRoster.Update(hfRosterID.Value,
                                    ddlCompany.SelectedValue.Trim(),
                                    ddlFromTimeHH.SelectedValue + ":" + ddlFromTimeMM.SelectedValue,
                                    ddlToTimeHH.SelectedValue + ":" + ddlToTimeMM.SelectedValue,
                                    ddlFlexibleHrs.SelectedValue + ":" + ddlFlexibleMins.SelectedValue,
                                    ddlRosterType.SelectedValue,
                                    userId,
                                    ddlStatus.SelectedValue,
                                    ddlNumDays.SelectedValue);

                    clearRest();
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED, lblMsg);
                }

                hfCompCode.Value = ddlCompany.SelectedValue.Trim();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                dhRoster = null;
            }

            populateGrid();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
        }


        private void clear()
        {
            //GroupHRIS.Utility.Utils.clearControls(true, ddlCompany, ddlRosterType, ddlFromTimeHH, ddlFromTimeMM, ddlToTimeHH, ddlToTimeMM, ddlFlexibleHrs, ddlFlexibleMins, cbIsActive);
            GroupHRIS.Utility.Utils.clearControls(true, ddlCompany, ddlRosterType, ddlFromTimeHH, ddlFromTimeMM, ddlToTimeHH, ddlToTimeMM, ddlFlexibleHrs, ddlFlexibleMins);

            Utility.Errorhandler.ClearError(lblMsg);

            gvRosters.DataSource = null;
            gvRosters.DataBind();

            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
        }


        private void clearRest()
        {
            GroupHRIS.Utility.Utils.clearControls(true, ddlRosterType, ddlFromTimeHH, ddlFromTimeMM, ddlToTimeHH, ddlToTimeMM, ddlFlexibleHrs, ddlFlexibleMins);

            Utility.Errorhandler.ClearError(lblMsg);

            gvRosters.DataSource = null;
            gvRosters.DataBind();

            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
        }


        private void populateGrid()
        {

            RosterDataHandler dhRoster = new RosterDataHandler();
            DataTable dtUsers = new DataTable();

            //Utility.Errorhandler.ClearError(lblMsg);

            try
            {
                if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                {
                    if(ddlCompany.SelectedIndex == 0)
                        dtUsers = dhRoster.populate();
                    else
                        dtUsers = dhRoster.populate(ddlCompany.SelectedValue.Trim());
                }
                else
                {
                    dtUsers = dhRoster.populate(Session["KeyCOMP_ID"].ToString().Trim());
                }

                gvRosters.DataSource = dtUsers;
                gvRosters.DataBind();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
            finally
            {
                dhRoster = null;
                dtUsers.Dispose();
            }
            
        }


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
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
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
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
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
        ///Get Available Roster Types 
        ///</summary>
        //------------------------------------------------------------------------------
        private void fillRosterTypes()
        {
            log.Debug("fillRosterTypes()");

            RosterDataHandler dhRosters = new RosterDataHandler();
            DataTable dtRosters = new DataTable();

            try
            {
                dtRosters = dhRosters.getRosterTypes();

                ddlRosterType.Items.Clear();

                if (dtRosters.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlRosterType.Items.Add(Item);

                    foreach (DataRow dataRow in dtRosters.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text = dataRow["DESCRIPTION"].ToString();
                        listItem.Value = dataRow["ROSTER_TYPE"].ToString();

                        ddlRosterType.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
                throw ex;
            }
            finally
            {
                dhRosters = null;
                dtRosters.Dispose();
            }

        }



        private void loadHours()
        {
            string sHH = "";

            for(int i=0; i <= 23; i++)
            {
                sHH = i.ToString().PadLeft(2, '0');

                ddlFromTimeHH.Items.Add(sHH);
                ddlToTimeHH.Items.Add(sHH);
            }
        }


        private void loadMins()
        {
            string sMM = "";

            for (int i = 0; i <= 59; i++)
            {
                sMM = i.ToString().PadLeft(2, '0');

                ddlFromTimeMM.Items.Add(sMM);
                ddlToTimeMM.Items.Add(sMM);
                ddlFlexibleMins.Items.Add(sMM);
            }
        }


        private void loadFlexiHours()
        {
            string sHH = "";

            for (int i = 0; i <= 2; i++)
            {
                sHH = i.ToString().PadLeft(2, '0');

                ddlFlexibleHrs.Items.Add(sHH);
            }
        }


        private void loadNumDays()
        {
            ddlNumDays.Items.Add("1.0");
            ddlNumDays.Items.Add("0.5");
        }


        private void loadStatus()
        {
            ddlStatus.Items.Add(Constants.STATUS_ACTIVE_TAG);
            ddlStatus.Items.Add(Constants.STATUS_INACTIVE_TAG);

            ddlStatus.SelectedValue = Constants.STATUS_ACTIVE_TAG;
        }


        protected void gvRosters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvRosters, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
        }

        protected void gvRosters_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);

            try
            {
                hfRosterID.Value                = gvRosters.SelectedRow.Cells[0].Text;
                ddlCompany.SelectedValue        = gvRosters.SelectedRow.Cells[1].Text;
                ddlRosterType.SelectedValue     = gvRosters.SelectedRow.Cells[2].Text;

                string[] fromTime = gvRosters.SelectedRow.Cells[4].Text.Split(Constants.CON_SEPARATING_DELIMITERS);
                ddlFromTimeHH.SelectedValue = fromTime[0];
                ddlFromTimeMM.SelectedValue = fromTime[1];

                string[] toTime = gvRosters.SelectedRow.Cells[5].Text.Split(Constants.CON_SEPARATING_DELIMITERS);
                ddlToTimeHH.SelectedValue = toTime[0];
                ddlToTimeMM.SelectedValue = toTime[1];

                string[] flexTime = gvRosters.SelectedRow.Cells[7].Text.Split(Constants.CON_SEPARATING_DELIMITERS);
                ddlFlexibleHrs.SelectedValue  = flexTime[0];
                ddlFlexibleMins.SelectedValue = flexTime[1];

                ddlNumDays.SelectedValue = gvRosters.SelectedRow.Cells[8].Text;

                if (gvRosters.SelectedRow.Cells[9].Text.Equals(Constants.STATUS_ACTIVE_VALUE))
                    ddlStatus.SelectedValue = Constants.STATUS_ACTIVE_TAG;
                else
                    ddlStatus.SelectedValue = Constants.STATUS_INACTIVE_TAG;


                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
        }

        protected void gvRosters_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRosters.PageIndex = e.NewPageIndex;
            populateGrid();
        }

        protected void ddlCompany_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //clearRest();
            Utility.Errorhandler.ClearError(lblMsg);
        }

        protected void ddlRosterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);
        }



    }

}