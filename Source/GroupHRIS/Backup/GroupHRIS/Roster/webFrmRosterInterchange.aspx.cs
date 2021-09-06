using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.Roster;
using DataHandler.MetaData;
using DataHandler.EmployeeLeave;
using Common;
using NLog;

namespace GroupHRIS.Roster
{
    public partial class webFrmRosterInterchange : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string userID = "";
        private string keyEmpID = "";
        private string keyRole = "";
        public bool isSearchable = false;

        private bool bInterchangerMode = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {
                userID = Session["KeyUSER_ID"].ToString();
                keyEmpID = Session["KeyEMPLOYEE_ID"].ToString();
                keyRole = Session["KeyHRIS_ROLE"].ToString();
            }


            if (!IsPostBack)
            {
                //fillYears();

                if (Session["KeyUSER_ID"] != null)
                {
                    userID = Session["KeyUSER_ID"].ToString();
                    keyEmpID = Session["KeyEMPLOYEE_ID"].ToString();
                    keyRole = Session["KeyHRIS_ROLE"].ToString();
                }

                

            }
            else
            {
                txtEmployeeID.Text          = hfEmpID.Value;
                txtName.Text                = hfName.Value;

                txtInterchangerID.Text      = hfInterchanger.Value;
                txtInterchangerName.Text    = hfInterchangerName.Value;

                if (hfInterchangerMode.Value.Equals("true"))
                    bInterchangerMode = true;

                               

                //string parameter = Request["__EVENTARGUMENT"];
                //populateGrid();
            }


        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            RosterInterchangeDataHandler dhRosterInt = new RosterInterchangeDataHandler();
            LeaveScheduleDataHandler dhLeaveSchedule = new LeaveScheduleDataHandler();

            try
            {
                //--------------------------------------------------------------------
                //Check for overlapping (same-day) rosters
                //--------------------------------------------------------------------
                if (dhRosterInt.isOverlappingExistForInterchange(txtEmployeeID.Text,
                                                                 txtRosterID.Text,
                                                                 txtDate.Text,
                                                                 txtFromTime.Text,
                                                                 txtToTime.Text,
                                                                 txtInterchangerID.Text,
                                                                 txtRosterInterganger.Text,
                                                                 txtDateInterchanger.Text,
                                                                 txtFromTimeIntger.Text,
                                                                 txtToTimeInter.Text,
                                                                 txtReason.Text,
                                                                 userID, hfRosterType.Value.ToString(), hfInerchangerRosterType.Value.ToString()))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Overlapping roster found. Please check.", lblMsg);
                }

                //--------------------------------------------------------------------
                //Check for overlapping overnight (prev-day) rosters
                //--------------------------------------------------------------------
                else if (dhRosterInt.isOverNightOverlappingExistForInterchange(txtEmployeeID.Text,
                                                txtRosterID.Text,
                                                txtDate.Text,
                                                txtFromTime.Text,
                                                txtToTime.Text,
                                                txtInterchangerID.Text,
                                                txtRosterInterganger.Text,
                                                txtDateInterchanger.Text,
                                                txtFromTimeIntger.Text,
                                                txtToTimeInter.Text,
                                                txtReason.Text,
                                                userID, hfRosterType.Value.ToString(), hfInerchangerRosterType.Value.ToString()))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Overlapping overnight roster found for the previous day. Please check.", lblMsg);
                }

                //--------------------------------------------------------------------
                //Check whether the employee is on leave
                //--------------------------------------------------------------------
                else if (dhLeaveSchedule.isOnLeave(txtEmployeeID.Text, txtDate.Text, txtRosterID.Text))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Unable to interchange. Employee " + txtEmployeeID.Text + " is on leave on " + txtDate.Text + ".", lblMsg);
                }

                //--------------------------------------------------------------------
                //Check whether the interchanger is on leave
                //--------------------------------------------------------------------
                else if (dhLeaveSchedule.isOnLeave(txtInterchangerID.Text, txtDateInterchanger.Text, txtRosterInterganger.Text))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Unable to interchange. Employee " + txtEmployeeID.Text + " is on leave on " + txtDate.Text + ".", lblMsg);
                }


                else
                {
                    dhRosterInt.interchangeRoster(txtEmployeeID.Text,
                                                    txtRosterID.Text,
                                                    txtDate.Text,
                                                    txtFromTime.Text,
                                                    txtToTime.Text,
                                                    txtInterchangerID.Text,
                                                    txtRosterInterganger.Text,
                                                    txtDateInterchanger.Text,
                                                    txtFromTimeIntger.Text,
                                                    txtToTimeInter.Text,
                                                    txtReason.Text,
                                                    userID);

                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_INTERCHANGE, lblMsg);

                    clear();
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
                dhRosterInt     = null;
                dhRosterInt     = null;
                dhLeaveSchedule = null;


            }

            
        }



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clearAll();
        }


        private void clear()
        {
            GroupHRIS.Utility.Utils.clearControls(true, txtEmployeeID, txtName, txtDate, txtRosterID, txtFromTime, txtToTime,
                                                    txtInterchangerID, txtInterchangerName, txtDateInterchanger, txtRosterInterganger, txtFromTimeIntger, txtToTimeInter, txtReason);

            hfEmpID.Value = String.Empty;
            hfName.Value = String.Empty;

            hfInterchanger.Value = String.Empty;
            hfInterchangerName.Value = String.Empty;
            hfRosterType.Value = String.Empty;
            hfInerchangerRosterType.Value = String.Empty;
        
        }

        private void clearAll()
        {
            clear();

            gvRoster.DataSource = null;
            gvRoster.DataBind();

            Utility.Errorhandler.ClearError(lblMsg);
        }



        protected void gvRoster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRoster.PageIndex = e.NewPageIndex;
        }



        protected void gvRoster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvRoster, "Select$" + e.Row.RowIndex.ToString()));
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



        protected void gvRoster_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);

            try
            {
                if (bInterchangerMode)
                {
                    txtRosterInterganger.Text     = gvRoster.SelectedRow.Cells[0].Text;
                    txtFromTimeIntger.Text        = gvRoster.SelectedRow.Cells[3].Text;
                    txtToTimeInter.Text           = gvRoster.SelectedRow.Cells[4].Text;
                    hfInerchangerRosterType.Value = gvRoster.SelectedRow.Cells[5].Text; 
                }
                else
                {
                    txtRosterID.Text    = gvRoster.SelectedRow.Cells[0].Text;
                    txtFromTime.Text    = gvRoster.SelectedRow.Cells[3].Text;
                    txtToTime.Text      = gvRoster.SelectedRow.Cells[4].Text;
                    hfRosterType.Value  = gvRoster.SelectedRow.Cells[5].Text;
                }

                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
        }



        protected void ibtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            bInterchangerMode = false;
            hfInterchangerMode.Value = "false";

            populateGrid();
        }


        protected void ibtnAdd2_Click(object sender, ImageClickEventArgs e)
        {
            bInterchangerMode = true;
            hfInterchangerMode.Value = "true";

            populateGrid();
        }


        private void populateGrid()
        {
            EmpRosterAssignmentDataHandler dhRoster = new EmpRosterAssignmentDataHandler();
            DataTable dtRosterAssignments = new DataTable();

            string empId, rosterDate;

            if (bInterchangerMode)
            {
                empId       = txtInterchangerID.Text.Trim();
                rosterDate  = txtDateInterchanger.Text.Trim();
            }
            else
            {
                empId       = txtEmployeeID.Text.Trim();
                rosterDate  = txtDate.Text.Trim();
            }

            try
            {
                dtRosterAssignments = dhRoster.populate(empId, rosterDate);

                gvRoster.DataSource = dtRosterAssignments;
                gvRoster.DataBind();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
            finally
            {
                dhRoster = null;
                dtRosterAssignments.Dispose();
            }

        }






    }
}