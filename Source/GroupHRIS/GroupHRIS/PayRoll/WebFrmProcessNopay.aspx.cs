using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using DataHandler.MetaData;
using System.Data;
using Common;
using DataHandler.Payroll;
using GroupHRIS.Utility;

namespace GroupHRIS.PayRoll
{
    public partial class WebFrmProcessNopay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var month = DateTime.Now.AddMonths(-1).ToString("MMMM", CultureInfo.InvariantCulture);

            DateTime Month = DateTime.Now.AddMonths(-1);
            month = Month.ToString("MMMM", CultureInfo.InvariantCulture) + " " + Month.Year.ToString();

            lblMonth.Text = month;
            //lblmsg.Text = "";
            if (!IsPostBack)
            {
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
                        LoadOT();
                    }
                }
            }
        }

        //protected void btnNoPay_Click(object sender, EventArgs e)
        //{
        //    string company = ddlCompany.SelectedValue;
        //    string month = DateTime.Today.ToString("yyyy/MM/dd");
        //    string logUser = Session["KeyUSER_ID"].ToString();
        //    lblmsg.Text = "Nopay Information";
        //    this.gvOvertime.DataSource = null;
        //    gvOvertime.DataBind();

        //    try
        //    {
        //        ProcessDataHandler oProcessDataHandler = new ProcessDataHandler();
        //        Boolean Status = oProcessDataHandler.InsertNoPay(company, month, logUser);
        //        //Bind data to gvNopay
                
        //        gvNopay.DataSource = oProcessDataHandler.GetTransactionNopay(company).Copy();
                
        //        DataTable table = gvNopay.DataSource as DataTable;
        //        int count = table.Rows.Count;
        //        if (count > 0)
        //        {
        //            gvNopay.DataBind();
        //            Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Processed Nopay", lblMessage);
        //        }
        //        else
        //        {
        //            Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "No Data Found ", lblMessage);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Nopay Process Faliure", lblMessage);
        //    }

        //}

        protected void btnOT_Click(object sender, EventArgs e)
        {
            string company = ddlCompany.SelectedValue;
            string month = DateTime.Today.ToString("yyyy/MM/dd");
            string logUser = Session["KeyUSER_ID"].ToString();
            //lblmsg.Text = "Overtime Information";
            //this.gvNopay.DataSource = null;
            //gvNopay.DataBind();
            Errorhandler.ClearError(lblMessage);
            try
            {
                if (ddlCompany.Items.Count > 0)
                {
                    if (ddlCompany.SelectedIndex > 0)
                    {
                        ProcessDataHandler oProcessDataHandler = new ProcessDataHandler();
                        

                        if (ddlCompany.SelectedValue.ToString() == Constants.CON_ETI_COMPANY_ID)
                        {
                            DataTable dtTransactions = new DataTable();

                            dtTransactions = oProcessDataHandler.ProcessETIOvertime(logUser).Copy();

                            if (dtTransactions.Rows.Count > 0)
                            {
                                gvOvertime.DataSource = dtTransactions.Copy();
                                gvOvertime.DataBind();

                                Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Processed", lblMessage);
                            }
                            else
                            {
                                Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "No Overtime Data found ", lblMessage);
                            }
                        }
                        else
                        {
                            //Boolean Status = oProcessDataHandler.InsertOT(company, month, logUser);
                            ////Bind data to gvOvertime
                            //gvOvertime.DataSource = oProcessDataHandler.GetTransactionOT(company).Copy();
                            //DataTable dt = gvOvertime.DataSource as DataTable;
                            //int count = dt.Rows.Count;

                            //if (count == 0)
                            //{
                            //    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "No Overtime Data found ", lblMessage);
                            //}
                            //else
                            //{
                            //    gvOvertime.DataBind();
                            //    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully Processed OT ", lblMessage);
                            //}
                        }
                    }
                }


                
            }
            catch (Exception ex)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMessage);
            }
        }

        private void fillCompanies()
        {
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable companies = new DataTable();

            try
            {
                if (Cache["Companies"] != null)
                {
                    companies = (DataTable)Cache["Companies"];
                }
                else
                {
                    companies = companyDataHandler.getCompanyIdCompName().Copy();
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
                companyDataHandler = null;
                companies.Dispose();
            }
        }

        private void fillCompanies(string companyId)
        {
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            DataTable companies = new DataTable();

            try
            {
                companies = companyDataHandler.getCompanyIdCompName(companyId).Copy();

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
                companyDataHandler = null;
                companies.Dispose();
            }

        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            Errorhandler.ClearError(lblMessage);
            //lblmsg.Text = "";
            //this.gvNopay.DataSource = null;
            //gvNopay.DataBind();

            LoadOT();
        }

        void LoadOT()
        {
            DataTable dtProcessedData = new DataTable();
            DataTable dtModifiedData = new DataTable();
            ProcessDataHandler PDH = new ProcessDataHandler();
            try
            {
                Errorhandler.ClearError(lblMessage);

                DateTime Month = DateTime.Now.AddMonths(-1);
                string TransMonth = Month.Year.ToString() + Month.Month.ToString().PadLeft(2,'0');
                string CompanyID = ddlCompany.SelectedValue.ToString().Trim();

                dtProcessedData = PDH.GetProcessedOT(TransMonth, CompanyID).Copy();
                gvOvertime.DataSource = dtProcessedData.Copy();
                gvOvertime.DataBind();

                if (dtProcessedData.Rows.Count > 0)
                {
                    //dtModifiedData = PDH.GetModifiedOT(TransMonth, CompanyID).Copy();

                    //if (dtModifiedData.Rows.Count > 0)
                    //{

                    //    DateTime MsgMonth = DateTime.Now.AddMonths(-1);
                    //    string Msgmonth = Month.ToString("MMMM", CultureInfo.InvariantCulture) + " " + Month.Year.ToString();

                    //    string Message = "Re-Process will lost the adjusted overtime and special payment data for '" + Msgmonth + "'";
                    //    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, Message, lblMessage);
                    //}

                    tblProcess.Visible = false;
                }
                else
                {
                    tblProcess.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMessage);
            }
            finally
            {
                dtProcessedData = null;
                dtModifiedData = null;
                PDH = null;
            }
        }


        //protected void gvNopay_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
        //            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
        //            e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvNopay, "Select$" + e.Row.RowIndex.ToString()));
        //            e.Row.Attributes.Add("style", "cursor:pointer;");
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        CommonVariables.MESSAGE_TEXT = Ex.Message;
        //        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
        //    }
        //}

        //protected void gvNopay_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    string company = ddlCompany.SelectedValue;
        //    ProcessDataHandler oProcessDataHandler = new ProcessDataHandler();
        //    gvNopay.PageIndex = e.NewPageIndex;
        //    gvNopay.DataSource = oProcessDataHandler.GetTransactionNopay(company);
        //    gvNopay.DataBind();
        //    oProcessDataHandler = null;
        //}

        protected void gvOvertime_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvOvertime, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void gvOvertime_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string company = ddlCompany.SelectedValue;
            ProcessDataHandler oProcessDataHandler = new ProcessDataHandler();
            gvOvertime.PageIndex = e.NewPageIndex;
            gvOvertime.DataSource = oProcessDataHandler.GetTransactionOT(company);
            gvOvertime.DataBind();
            //oProcessDataHandler = null;
        }


    }
}