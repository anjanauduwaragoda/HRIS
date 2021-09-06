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
    public partial class webFrmEmpRosterAssignment : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string userID = "";
        private string compCode = "";



        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {
                userID      = Session["KeyUSER_ID"].ToString();
            }


            if (!IsPostBack)
            {
                if (Session["KeyUSER_ID"] != null)
                {
                    userID      = Session["KeyUSER_ID"].ToString();
                }
            }
            else
            {
                txtEmployeeID.Text  = hfEmpID.Value;
                txtName.Text        = hfName.Value;
                compCode            = hfCompCode.Value;

                string parameter = Request["__EVENTARGUMENT"];

                if ((parameter.Equals("TextChanged")) && (compCode.Trim().Length > 0))
                {
                    fillRosters();
                    txtDate.Text = String.Empty;
                }

                populateGrid();
            }
            
        }


        private void fillRosters()
        {
            log.Debug("fillRosters()");

            RosterDataHandler dhRosters = new RosterDataHandler();
            DataTable dtRosters = new DataTable();

            try
            {
                dtRosters = dhRosters.populateForDropDown(compCode);

                ddlRosterID.Items.Clear();

                if (dtRosters.Rows.Count > 0)
                {
                    ListItem Item = new ListItem();
                    Item.Text = "";
                    Item.Value = "";
                    ddlRosterID.Items.Add(Item);

                    foreach (DataRow dataRow in dtRosters.Rows)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Text   = dataRow["ROSTER_DESC"].ToString();
                        listItem.Value  = dataRow["ROSTR_ID"].ToString();

                        ddlRosterID.Items.Add(listItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //lblerror.Text = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
                throw ex;
            }
            finally
            {
                dhRosters = null;
                dtRosters.Dispose();
            }

        }


        private void populateGrid()
        {
            EmpRosterAssignmentDataHandler dhRosterAsgn = new EmpRosterAssignmentDataHandler();
            DataTable dtRosterAsgn = new DataTable();

            try
            {
                dtRosterAsgn = dhRosterAsgn.populateRostersModifiedToday(txtEmployeeID.Text.Trim());

                gvRosterAssignments.DataSource = dtRosterAsgn;
                gvRosterAssignments.DataBind();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //lblerror.Text = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
            finally
            {
                dhRosterAsgn = null;
                dtRosterAsgn.Dispose();
            }
        }




        protected void ddlRosterID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        protected void btnSave_Click(object sender, EventArgs e)
        {

        }



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
        }


        private void clear()
        {
            GroupHRIS.Utility.Utils.clearControls(true, txtEmployeeID, txtName, ddlRosterID, txtDate);

            gvRosterAssignments.DataSource = null;
            gvRosterAssignments.DataBind();

            Utility.Errorhandler.ClearError(lblMsg);
        }


        protected void gvRosterAssignments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRosterAssignments.PageIndex = e.NewPageIndex;
        }



        protected void gvRosterAssignments_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        protected void gvRosterAssignments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvRosterAssignments, "Select$" + e.Row.RowIndex.ToString()));
                    //e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvRosterAssignments, e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                //lblerror.Text = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
        }



        protected void btnAdd_Click(object sender, EventArgs e)
        {
            EmpRosterAssignmentDataHandler dhEmpRoster = new EmpRosterAssignmentDataHandler();
            RosterDataHandler dhRoster = new RosterDataHandler();
            string sRosterType = "";
            try
            {
                Utility.Errorhandler.ClearError(lblerror);

                List<string> range = dhRoster.getTimeRangeForRoster(ddlRosterID.SelectedValue.Trim());

                sRosterType = range[2];

                if ((sRosterType == "1") && (dhEmpRoster.checkForOverlappingRostersRegular(txtEmployeeID.Text.Trim(), txtDate.Text.Trim(), range[0], range[1])))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "An overlapping roster assignment found.", lblMsg);
                }
                else if ((sRosterType == "2") && (dhEmpRoster.checkForOverlappingRostersOverNight(txtEmployeeID.Text.Trim(), txtDate.Text.Trim(), range[0], range[1])))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "An overlapping overnight roster assignment found for the previous day.", lblMsg);
                }
                else
                {
                    dhEmpRoster.Insert(ddlRosterID.SelectedValue.Trim(),
                    txtEmployeeID.Text.Trim(),
                    txtDate.Text.Trim(),
                    Constants.CON_DB_FALSE_CHAR,
                    Constants.CON_NOT_SUMMARIZED,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    userID);

                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED, lblMsg);
                }



            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                dhEmpRoster = null;
                dhRoster = null;
            }

            populateGrid();
        }



        protected void gvRosterAssignments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString().Equals("Obsolete"))
            {
                obsolete(e);
            }
        }



        private void obsolete(GridViewCommandEventArgs e)
        {
            EmpRosterAssignmentDataHandler dhRosterAsgn = new EmpRosterAssignmentDataHandler();
            RosterInterchangeDataHandler dhInter = new RosterInterchangeDataHandler();

            DataTable dtSecEdu = new DataTable();
            bool bUpdated = false;
            bool bOppositeUpdated = false;

            Utility.Errorhandler.ClearError(lblMsg);


            //GridViewRow grdRow = (GridViewRow)((LinkButton)sender).NamingContainer;
            Int32 index = Convert.ToInt32(e.CommandArgument);

            GridViewRow grdRow = gvRosterAssignments.Rows[index];

            string rosterID = grdRow.Cells[1].Text;
            string dutyDate = grdRow.Cells[2].Text;
            string interchangeNum = grdRow.Cells[8].Text;
            string status   = grdRow.Cells[12].Text;

            string oppositeEmpOfInterchange = "";


            try
            {
                if (status.Equals(Constants.STATUS_INACTIVE_VALUE))
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "You are not allowed to obsolete an inactive roster.", lblMsg);

                else if (dhRosterAsgn.obsolete(rosterID, txtEmployeeID.Text.Trim(), dutyDate, Constants.CON_NOT_SUMMARIZED, userID, status))
                    bUpdated = true;


                if ((interchangeNum.Trim().Length > 0) && (status.Equals(Constants.STATUS_ACTIVE_VALUE)))
                {
                    oppositeEmpOfInterchange = dhInter.getOppositeEmpOfInterchange(txtEmployeeID.Text.Trim(), interchangeNum);

                    if (oppositeEmpOfInterchange.Trim().Length > 0)
                        bOppositeUpdated = dhRosterAsgn.obsoleteInterchange(interchangeNum, oppositeEmpOfInterchange, Constants.CON_NOT_SUMMARIZED, userID);

                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
            finally
            {
                dhRosterAsgn = null;
                dhInter = null;

                dtSecEdu.Dispose();
            }

            if ((bUpdated) && (bOppositeUpdated))
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_OBSOLETED_INT + " (Employee:" + oppositeEmpOfInterchange + ")", lblMsg);
            else if (bUpdated)
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_OBSOLETED, lblMsg);


            populateGrid();
        }




    }
}