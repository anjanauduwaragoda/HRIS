using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.TrainingAndDevelopment;
using System.Data;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmFinancialYears : System.Web.UI.Page
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadStatus();
                    LoadData();
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            FinancialYearsDataHandler FYDH = new FinancialYearsDataHandler();
            try
            {
                string FinancialYear = txtFinancialYear.Text.Trim();
                string StartDate = txtStartDate.Text.Trim();
                string EndDate = txtEndDate.Text.Trim();
                string Description = txtDescription.Text.Trim();
                string Status = ddlStatusCode.SelectedValue.ToString().Trim();
                string Addedby = (Session["KeyEMPLOYEE_ID"] as string);

                DateTime SDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime EDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                StartDate = Convert.ToDateTime(SDate).ToString("yyyy-MM-dd");
                EndDate = Convert.ToDateTime(EDate).ToString("yyyy-MM-dd");
                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    if (FYDH.CheckFinancialYearExsistance(txtFinancialYear.Text.Trim()))
                    {
                        CommonVariables.MESSAGE_TEXT = "Financial year already exists.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);                        
                        return;
                    }
                    else
                    {
                        FYDH.Insert(FinancialYear, StartDate, EndDate, Description, Status, Addedby);
                        ClearFields();
                        LoadData();

                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMessage);
                    }
                }
                else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string OldFinancialYear = HttpUtility.HtmlDecode(grdvAssessmentGroup.Rows[grdvAssessmentGroup.SelectedIndex].Cells[0].Text).ToString().Trim();

                    if (FYDH.CheckFinancialYearExsistance(txtFinancialYear.Text.Trim(), OldFinancialYear))
                    {
                        CommonVariables.MESSAGE_TEXT = "Financial year already exists.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblMessage);
                        return;
                    }
                    else
                    {
                        FYDH.Update(FinancialYear, StartDate, EndDate, Description, Status, Addedby, OldFinancialYear);
                        ClearFields();
                        LoadData();

                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblMessage);
                    }
                }

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                FYDH = null;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFields();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

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

        protected void grdvAssessmentGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                string FinancialYear = HttpUtility.HtmlDecode(grdvAssessmentGroup.Rows[grdvAssessmentGroup.SelectedIndex].Cells[0].Text).ToString().Trim();
                string StartDate = HttpUtility.HtmlDecode(grdvAssessmentGroup.Rows[grdvAssessmentGroup.SelectedIndex].Cells[1].Text).ToString().Trim();
                string EndDate = HttpUtility.HtmlDecode(grdvAssessmentGroup.Rows[grdvAssessmentGroup.SelectedIndex].Cells[2].Text).ToString().Trim();
                string Description = HttpUtility.HtmlDecode(grdvAssessmentGroup.Rows[grdvAssessmentGroup.SelectedIndex].Cells[3].Text).ToString().Trim();
                string StatusCode = HttpUtility.HtmlDecode(grdvAssessmentGroup.Rows[grdvAssessmentGroup.SelectedIndex].Cells[4].Text).ToString().Trim();

                txtFinancialYear.Text = FinancialYear;
                txtStartDate.Text = StartDate;
                txtEndDate.Text = EndDate;
                txtDescription.Text = Description;
                ddlStatusCode.SelectedIndex = ddlStatusCode.Items.IndexOf(ddlStatusCode.Items.FindByText(StatusCode));
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                
            }
        }

        protected void grdvAssessmentGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            FinancialYearsDataHandler FYDH = new FinancialYearsDataHandler();
            DataTable dtFinancialYears = new DataTable();
            try
            {
                dtFinancialYears = FYDH.Populate().Copy();

                if (dtFinancialYears.Rows.Count > 0)
                {
                    for (int i = 0; i < dtFinancialYears.Rows.Count; i++)
                    {
                        string Status = dtFinancialYears.Rows[i]["STATUS_CODE"].ToString();
                        if (Status == Constants.CON_ACTIVE_STATUS)
                        {
                            dtFinancialYears.Rows[i]["STATUS_CODE"] = Constants.STATUS_ACTIVE_TAG;
                        }
                        else
                        {
                            dtFinancialYears.Rows[i]["STATUS_CODE"] = Constants.STATUS_INACTIVE_TAG;
                        }
                    }
                }

                grdvAssessmentGroup.PageIndex = e.NewPageIndex;
                grdvAssessmentGroup.DataSource = dtFinancialYears.Copy();
                grdvAssessmentGroup.DataBind();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                FYDH = null;
                dtFinancialYears.Dispose();
            }
        }

        #endregion

        #region Methods

        void LoadStatus()
        {
            try
            {
                ddlStatusCode.Items.Add(new ListItem("", ""));
                ddlStatusCode.Items.Add(new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                ddlStatusCode.Items.Add(new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {

            }
        }

        void ClearFields()
        {
            try
            {
                Utility.Utils.clearControls(true, txtFinancialYear, txtStartDate, txtEndDate, txtDescription, ddlStatusCode);
                Utility.Errorhandler.ClearError(lblMessage);
                btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                
            }
        }

        void LoadData()
        {
            FinancialYearsDataHandler FYDH = new FinancialYearsDataHandler();
            DataTable dtFinancialYears = new DataTable();
            try
            {
                dtFinancialYears = FYDH.Populate().Copy();

                if (dtFinancialYears.Rows.Count > 0)
                {
                    for (int i = 0; i < dtFinancialYears.Rows.Count; i++)
                    {
                        string Status = dtFinancialYears.Rows[i]["STATUS_CODE"].ToString();
                        if (Status == Constants.CON_ACTIVE_STATUS)
                        {
                            dtFinancialYears.Rows[i]["STATUS_CODE"] = Constants.STATUS_ACTIVE_TAG;
                        }
                        else
                        {
                            dtFinancialYears.Rows[i]["STATUS_CODE"] = Constants.STATUS_INACTIVE_TAG;
                        }
                    }
                }

                grdvAssessmentGroup.DataSource = dtFinancialYears.Copy();
                grdvAssessmentGroup.DataBind();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                FYDH = null;
                dtFinancialYears.Dispose();
            }
        }

        #endregion
    }
}