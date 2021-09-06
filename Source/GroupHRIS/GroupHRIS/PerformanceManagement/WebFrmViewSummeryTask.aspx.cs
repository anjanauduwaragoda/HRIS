using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.PerformanceManagement;
using NLog;
using Common;
using System.Data;
using DataHandler.Userlogin;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmViewSummeryTask : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";

        //string empId = "";
        //string year = "";
        string empId = "EP000524";
        string year = "2016";
        protected void Page_Load(object sender, EventArgs e)
        {
            PasswordHandler cripto = new PasswordHandler();

            try
            {
                sIPAddress = Request.UserHostAddress;
                log.Debug("IP:" + sIPAddress + "WebFrmViewSummeryTask : Page_Load");

                if (!IsPostBack)
                {
                    //empId = cripto.Decrypt(Request.QueryString["empId"]);
                    //year = cripto.Decrypt(Request.QueryString["year"]);
                    //empId = "EP000524";
                    //year = "2016";
                    lblEmployee.Text = empId;
                    lblYear.Text = year;

                    loadTaskGrid(empId, year);
                }

                Utility.Errorhandler.ClearError(lblMessage);

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                cripto = null;
            }
        }

        protected void grdTaskList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("grdTaskList_RowDataBound()");

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdTaskList, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void grdTaskList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("grdTaskList_PageIndexChanging()");
            try
            {
                grdTaskList.PageIndex = e.NewPageIndex;

                loadTaskGrid(empId, year);
                Utility.Errorhandler.ClearError(lblMessage);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdTaskList_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("grdTaskList_SelectedIndexChanged()");
            try
            {
                int SelectedIndex = grdTaskList.SelectedIndex;
                string task = Server.HtmlDecode(grdTaskList.Rows[SelectedIndex].Cells[1].Text.ToString());

                loadExtention(task);
                loadProgress(task);
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }



        public void loadTaskGrid(string empId,string year)
        {
            ViewTaskDetailsDataHandler VTDH = new ViewTaskDetailsDataHandler();
            DataTable dataTable = new DataTable();

            try
            {
                dataTable = VTDH.Populate(empId, year).Copy();
                grdTaskList.DataSource = dataTable;
                grdTaskList.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                VTDH = null;
                dataTable.Dispose();
            }
        }

        public void loadExtention(string taskId)
        {
            ViewTaskDetailsDataHandler VTDH = new ViewTaskDetailsDataHandler();

            try
            {
                grdtskExtention.DataSource = VTDH.PopulateTaskExtentions(taskId);
                grdtskExtention.DataBind();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                VTDH = null;
            }
        }

        public void loadProgress(string taskId)
        {
            ViewTaskDetailsDataHandler VTDH = new ViewTaskDetailsDataHandler();

            try
            {
                grdTaskProgress.DataSource = VTDH.PopulateTaskProgress(taskId);
                grdTaskProgress.DataBind();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                VTDH = null;
            }
        }

    }
}