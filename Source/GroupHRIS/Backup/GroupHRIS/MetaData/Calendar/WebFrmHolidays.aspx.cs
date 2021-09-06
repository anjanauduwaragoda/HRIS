using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.MetaData;
using System.Data;
using Common;
using System.Drawing;

namespace GroupHRIS.MetaData.Calendar
{
    public partial class WebFrmHolidays : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                get_holidayTypes();
            }
        }


        private void get_holidayTypes()
        {
            CalendarDataHandler calendarDataHandler = new CalendarDataHandler();
            DataTable holidayTypes = calendarDataHandler.populateHolidays();

            try
            {
                if (holidayTypes.Rows.Count > 0)
                {
                    GridView1.DataSource = holidayTypes;
                    GridView1.DataBind();

                    GridView1.HeaderRow.Cells[0].Text = "Date Type Code";
                    GridView1.HeaderRow.Cells[1].Text = "Holiday Type / Remarks";
                    GridView1.HeaderRow.Cells[2].Text = "Color";
                    GridView1.HeaderRow.Cells[3].Text = "Status";
                    GridView1.HeaderRow.Cells[0].Width = 100;
                    GridView1.HeaderRow.Cells[1].Width = 350;
                    GridView1.HeaderRow.Cells[2].Width = 50;
                    GridView1.HeaderRow.Cells[3].Width = 50;

                    for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
                    {
                        string ScellColor = GridView1.Rows[i].Cells[2].Text.ToString();
                        Color col = ColorTranslator.FromHtml(String.Format("#{0}", ScellColor));
                        //ScellColor = System.Drawing.ColorTranslator.ToHtml(col);
                        GridView1.Rows[i].Cells[2].BackColor = col;
                    }

                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                calendarDataHandler = null;
                holidayTypes.Dispose();
                holidayTypes = null;
            }


        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            CalendarDataHandler calendarDataHandler = new CalendarDataHandler();
            string sHolidaytype = txthoidaytype.Text.ToString().Trim();
            string sDateType = txtdatetype.Text.ToString().Trim();
            string sClacolor = txtcolor.Text.ToString();
            string sStatus = ddlrolestatus.SelectedValue.ToString();
            try
            {
                if (btnsave.Text == "Save")
                {
                    string sHoliday = calendarDataHandler.populateHolidays(sDateType);
                    if (sHoliday == "")
                    {
                        Boolean blInserted = calendarDataHandler.InsertCompanyHoliday(sDateType, sHolidaytype, sClacolor, sStatus);
                        if (blInserted == true)
                        {
                            get_holidayTypes();
                            CommonVariables.MESSAGE_TEXT = "Holiday Type saved";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Unable to save Holiday Type";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                    }
                }
                else
                {
                    Boolean blInserted = calendarDataHandler.UpdateCompanyHoliday(sDateType, sHolidaytype, sClacolor, sStatus);
                    if (blInserted == true)
                    {
                        get_holidayTypes();
                        CommonVariables.MESSAGE_TEXT = "Holiday Type updated";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "Unable to update Holiday Type";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                }

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                calendarDataHandler = null;
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            get_holidayTypes();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.GridView1, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Utility.Errorhandler.ClearError(lblerror);
                ddlrolestatus.ClearSelection();
                txtdatetype.Text = GridView1.SelectedRow.Cells[0].Text;
                txthoidaytype.Text = GridView1.SelectedRow.Cells[1].Text;
                txtcolor.Text = GridView1.SelectedRow.Cells[2].Text;
                ddlrolestatus.Items.FindByText(GridView1.SelectedRow.Cells[3].Text.ToString().Trim()).Selected = true;
                btnsave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            }
            catch (Exception ex )
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
           
        }

        protected void btnclear_Click(object sender, EventArgs e)
        {
            btnsave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            txtdatetype.Text = "";
            txthoidaytype.Text = "";
            txtcolor.Text = "";
            Utility.Errorhandler.ClearError(lblerror);
        }

    }
}