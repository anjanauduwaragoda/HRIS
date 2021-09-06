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
    public partial class webFrmModifyRosterAssignments : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string userID = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            //btnObsolete.Visible = false;

            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {
                userID = Session["KeyUSER_ID"].ToString();
            }


            if (!IsPostBack)
            {

                if (Session["KeyUSER_ID"] != null)
                {
                    userID = Session["KeyUSER_ID"].ToString();
                }
            }
            else
            {
                txtEmployeeID.Text = hfEmpID.Value;
                txtName.Text = hfName.Value;

                //string parameter = Request["__EVENTARGUMENT"];

                //populateGrid();
            }
        }

        private void populateGrid()
        {
            EmpRosterAssignmentDataHandler dhRosterAsgn = new EmpRosterAssignmentDataHandler();
            DataTable dtRosterAsgn = new DataTable();
            //Utility.Errorhandler.ClearError(lblMsg);
            if(!IsPostBack)
            {
                createRosterData();
            }

            try
            {
                dtRosterAsgn = dhRosterAsgn.populate(txtEmployeeID.Text.Trim(), txtFromDate.Text.Trim(), txtToDate.Text.Trim());
                
                gvRosterAssignments1.DataSource = dtRosterAsgn;
                gvRosterAssignments1.DataBind();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
            finally
            {
                dhRosterAsgn = null;
                dtRosterAsgn.Dispose();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void clear()
        {
            GroupHRIS.Utility.Utils.clearControls(true,txtEmployeeID, txtName, txtFromDate, txtToDate);

            Utility.Errorhandler.ClearError(lblMsg);

            gvRosterAssignments1.DataSource = null;
            gvRosterAssignments1.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsg);

            if(!CommonUtils.isValidDateRange(txtFromDate.Text.Trim(), txtToDate.Text.Trim()))
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "From Date should be earlier than To Date", lblMsg);
            else
                populateGrid();
                btnObsolete.Visible = true;

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
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsg);
            }
             
        }

        protected void gvRosterAssignments_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvRosterAssignments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            EmpRosterAssignmentDataHandler dhRosterAsgn = new EmpRosterAssignmentDataHandler();
            gvRosterAssignments1.PageIndex = e.NewPageIndex;
            gvRosterAssignments1.DataSource = dhRosterAsgn.populate(txtEmployeeID.Text.Trim(), txtFromDate.Text.Trim(), txtToDate.Text.Trim());
            gvRosterAssignments1.DataBind();
            dhRosterAsgn = null;
        }

       
        protected void btnObsolete_Click(object sender, EventArgs e)
        {
            createRosterData();
            EmpRosterAssignmentDataHandler dhRosterAsgn = new EmpRosterAssignmentDataHandler();
            DataTable dt = (DataTable)Session["rosterData"];

            for (int i = 0; i < gvRosterAssignments1.Rows.Count; i++)
            {
                CheckBox chkb = (CheckBox)gvRosterAssignments1.Rows[i].Cells[0].FindControl("chkBxSelect");

                if (chkb.Checked)
                {
                    string rosterID = gvRosterAssignments1.Rows[i].Cells[1].Text;
                    string dutyDate = gvRosterAssignments1.Rows[i].Cells[2].Text;
                    string interchangeNum = gvRosterAssignments1.Rows[i].Cells[8].Text;
                    string status = gvRosterAssignments1.Rows[i].Cells[12].Text;

                    DataRow dtrow = dt.NewRow();
                    dtrow["ROSTR_ID"] = rosterID;
                    dtrow["DUTY_DATE"] = dutyDate;
                    dtrow["INTERCHANGE_NUMBER"] = interchangeNum;
                    dtrow["STATUS_CODE"] = status;

                    dt.Rows.Add(dtrow);

                }

            }
            try
            {
                Boolean isObsolete = dhRosterAsgn.isObsolete(dt, txtEmployeeID.Text.Trim(), Constants.CON_NOT_SUMMARIZED, userID);
                populateGrid();

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void createRosterData()
        {
            DataTable rosterData = new DataTable();

            rosterData.Columns.Add("ROSTR_ID", typeof(System.String));
            rosterData.Columns.Add("DUTY_DATE", typeof(System.String));
            rosterData.Columns.Add("INTERCHANGE_NUMBER", typeof(System.String));
            rosterData.Columns.Add("STATUS_CODE", typeof(System.String));

            Session["rosterData"] = rosterData;
        }


    }
}