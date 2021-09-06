using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.Userlogin;
using DataHandler.PerformanceManagement;
using System.Data;
using Common;
using DataHandler.Utility;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmSupervisorCompetencyAssessment : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        public String Questions = String.Empty;
        public String DisplayAssessmentName = String.Empty;
        public String DisplayAssessmentPurposes = String.Empty;
        public DataTable dtRatings = new DataTable();


        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            PasswordHandler crpto;
            try
            {
                sIPAddress = Request.UserHostAddress;
                log.Debug("IP:" + sIPAddress + "webFrmEmployee : Page_Load");

                if (!IsPostBack)
                {
                    crpto = new PasswordHandler();

                    string AssessmentID = Request.QueryString["assmtId"];
                    string YearOfAssessmentID = Request.QueryString["year"];
                    string EmployeeID = Request.QueryString["employeeId"];

                    hfAssessmentID.Value = crpto.Decrypt(AssessmentID);
                    hfYearOfAssessment.Value = crpto.Decrypt(YearOfAssessmentID);
                    hfEmployeeID.Value = crpto.Decrypt(EmployeeID);

                    LoadData();
                    FinalizeCheck();
                    DisplayFinalizeMessage();
                    string x = lblFinalizeNotice.Text;

                    Label1.Text = x;


                    SetUpdateButtonText();
                }

                LoadAssessmentPurposes();
                DisplayFinalizeMessage();
                DisplayChart();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                crpto = null;
            }
        }

        protected void grdvCompetencies_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlCompetencyRating = (e.Row.FindControl("ddlSupervisorRating") as DropDownList);

                ddlCompetencyRating.Items.Add(new ListItem("", ""));

                for (int i = 0; i < dtRatings.Rows.Count; i++)
                {
                    string text = dtRatings.Rows[i]["RATING"].ToString();
                    string value = dtRatings.Rows[i]["WEIGHT"].ToString();

                    ddlCompetencyRating.Items.Add(new ListItem(text, value));
                }

                string SupervisorWeight = HttpUtility.HtmlDecode(e.Row.Cells[7].Text).ToString().Trim();
                if (SupervisorWeight != String.Empty)
                {
                    ddlCompetencyRating.SelectedIndex = ddlCompetencyRating.Items.IndexOf(ddlCompetencyRating.Items.FindByValue(SupervisorWeight));
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblStatus);
            SupervisorCompetencyAssessmentDataHandler SCADH = new SupervisorCompetencyAssessmentDataHandler();
            DataTable dtCompetencies = new DataTable();
            try
            {
                log.Debug("btnSave_Click()");


                if (txtSupervisorComment.Text == String.Empty)
                {
                    Label1.Text = string.Empty;
                    CommonVariables.MESSAGE_TEXT = "Supervisor Comment(s) is Required";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                    return;
                }

                for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                {
                    DropDownList ddlSupervisorRating = (grdvCompetencies.Rows[i].FindControl("ddlSupervisorRating") as DropDownList);
                    if ((ddlSupervisorRating.SelectedIndex > 0) == false)
                    {
                        Label1.Text = string.Empty;
                        CommonVariables.MESSAGE_TEXT = "All ratings are required for complete the competency assessment";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblStatus);
                        return;
                    }
                }

                double TotalSupervisorScore = 0.0;
                string SupervisorComments = txtSupervisorComment.Text.Trim();
                string ModifiedBy = (Session["KeyEMPLOYEE_ID"] as string).Trim();
                string CompetencyProfileID = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[8].Text).ToString();
                string AssessmentToken = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[0].Text).ToString();
                string AssessmentID = hfAssessmentID.Value.ToString().Trim();
                string EmployeeID = hfEmployeeID.Value.ToString().Trim();
                string YearOfAssessment = hfYearOfAssessment.Value.ToString().Trim();


                DataTable dtEmployeeCompetencies = new DataTable();

                dtEmployeeCompetencies.Columns.Add("COMPETENCY_ID");
                dtEmployeeCompetencies.Columns.Add("SUPERVISOR_RATING");
                dtEmployeeCompetencies.Columns.Add("SUPERVISOR_WEIGHT");


                for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                {
                    string CompetencyID = String.Empty;
                    string SupervisorRating = String.Empty;
                    string SupervisorWeight = String.Empty;

                    DropDownList ddlCompetencyRating = (grdvCompetencies.Rows[i].FindControl("ddlSupervisorRating") as DropDownList);

                    CompetencyID = HttpUtility.HtmlDecode(grdvCompetencies.Rows[i].Cells[1].Text).ToString();

                    if (ddlCompetencyRating.SelectedIndex > 0)
                    {
                        double weight = 0.0;
                        if (Double.TryParse(ddlCompetencyRating.SelectedValue.ToString(), out weight))
                        {
                            TotalSupervisorScore += weight;

                            SupervisorRating = ddlCompetencyRating.SelectedItem.Text.Trim();
                            SupervisorWeight = ddlCompetencyRating.SelectedValue.Trim();
                        }
                        else
                        {
                            TotalSupervisorScore += 0.0;
                        }
                    }
                    else
                    {
                        TotalSupervisorScore += 0.0;
                    }

                    DataRow drEmployeeCompetency = dtEmployeeCompetencies.NewRow();

                    drEmployeeCompetency["COMPETENCY_ID"] = CompetencyID;
                    drEmployeeCompetency["SUPERVISOR_RATING"] = SupervisorRating;
                    drEmployeeCompetency["SUPERVISOR_WEIGHT"] = SupervisorWeight;

                    dtEmployeeCompetencies.Rows.Add(drEmployeeCompetency);
                }


                SCADH.Complete(TotalSupervisorScore.ToString(), SupervisorComments, CompetencyProfileID, AssessmentToken, AssessmentID, EmployeeID, YearOfAssessment, ModifiedBy, dtEmployeeCompetencies.Copy());
                if (btnSave.Text == Constants.CON_COMPLETE_BUTTON_TEXT)
                {
                    CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_COMPLETED;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);
                }
                LoadData();

                //Update Assessment Status
                //UtilsDataHandler UDH = new UtilsDataHandler();
                //UDH.updateAssessmentStatus(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value);

                DisplayFinalizeMessage();
                Label1.Text = String.Empty;
                lblFinalizeNotice.Text = String.Empty;
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SCADH = null;
                dtCompetencies.Dispose();
            }
        }

        protected void btnSaveCompetency_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblStatus);
            SupervisorCompetencyAssessmentDataHandler SCADH = new SupervisorCompetencyAssessmentDataHandler();
            DataTable dtCompetencies = new DataTable();
            try
            {
                log.Debug("btnSaveCompetency_Click()");

                double TotalSupervisorScore = 0.0;
                string SupervisorComments = txtSupervisorComment.Text.Trim();
                string ModifiedBy = (Session["KeyEMPLOYEE_ID"] as string).Trim();
                string CompetencyProfileID = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[8].Text).ToString();
                string AssessmentToken = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[0].Text).ToString();
                string AssessmentID = hfAssessmentID.Value.ToString().Trim();
                string EmployeeID = hfEmployeeID.Value.ToString().Trim();
                string YearOfAssessment = hfYearOfAssessment.Value.ToString().Trim();


                DataTable dtEmployeeCompetencies = new DataTable();

                dtEmployeeCompetencies.Columns.Add("COMPETENCY_ID");
                dtEmployeeCompetencies.Columns.Add("SUPERVISOR_RATING");
                dtEmployeeCompetencies.Columns.Add("SUPERVISOR_WEIGHT");


                for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                {
                    string CompetencyID = String.Empty;
                    string SupervisorRating = String.Empty;
                    string SupervisorWeight = String.Empty;

                    DropDownList ddlCompetencyRating = (grdvCompetencies.Rows[i].FindControl("ddlSupervisorRating") as DropDownList);

                    CompetencyID = HttpUtility.HtmlDecode(grdvCompetencies.Rows[i].Cells[1].Text).ToString();

                    if (ddlCompetencyRating.SelectedIndex > 0)
                    {
                        double weight = 0.0;
                        if (Double.TryParse(ddlCompetencyRating.SelectedValue.ToString(), out weight))
                        {
                            TotalSupervisorScore += weight;

                            SupervisorRating = ddlCompetencyRating.SelectedItem.Text.Trim();
                            SupervisorWeight = ddlCompetencyRating.SelectedValue.Trim();
                        }
                        else
                        {
                            TotalSupervisorScore += 0.0;
                        }
                    }
                    else
                    {
                        TotalSupervisorScore += 0.0;
                    }

                    DataRow drEmployeeCompetency = dtEmployeeCompetencies.NewRow();

                    drEmployeeCompetency["COMPETENCY_ID"] = CompetencyID;
                    drEmployeeCompetency["SUPERVISOR_RATING"] = SupervisorRating;
                    drEmployeeCompetency["SUPERVISOR_WEIGHT"] = SupervisorWeight;

                    dtEmployeeCompetencies.Rows.Add(drEmployeeCompetency);
                }


                SCADH.Insert(TotalSupervisorScore.ToString(), SupervisorComments, CompetencyProfileID, AssessmentToken, AssessmentID, EmployeeID, YearOfAssessment, ModifiedBy, dtEmployeeCompetencies.Copy());
                DisplayFinalizeMessage();
                
                if (btnSave.Text == Constants.CON_COMPLETE_BUTTON_TEXT)
                {
                    DisplayFinalizeMessage();
                    string x = lblFinalizeNotice.Text;
                    Label1.Text = x;
                    CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);
                }
                else
                {
                    DisplayFinalizeMessage();
                    string x = lblFinalizeNotice.Text;
                    Label1.Text = x;
                    CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);
                }
                //LoadData();
                SetUpdateButtonText();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SCADH = null;
                dtCompetencies.Dispose();
            }
        }

        protected void btnFinalize_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblStatus);
            SupervisorCompetencyAssessmentDataHandler SCADH = new SupervisorCompetencyAssessmentDataHandler();
            DataTable dtCompetencies = new DataTable();
            try
            {
                log.Debug("btnFinalize_Click()");

                double TotalSupervisorScore = 0.0;
                string SupervisorComments = txtSupervisorComment.Text.Trim();
                string ModifiedBy = (Session["KeyEMPLOYEE_ID"] as string).Trim();
                string CompetencyProfileID = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[8].Text).ToString();
                string AssessmentToken = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[0].Text).ToString();
                string AssessmentID = hfAssessmentID.Value.ToString().Trim();
                string EmployeeID = hfEmployeeID.Value.ToString().Trim();
                string YearOfAssessment = hfYearOfAssessment.Value.ToString().Trim();


                DataTable dtEmployeeCompetencies = new DataTable();

                dtEmployeeCompetencies.Columns.Add("COMPETENCY_ID");
                dtEmployeeCompetencies.Columns.Add("SUPERVISOR_RATING");
                dtEmployeeCompetencies.Columns.Add("SUPERVISOR_WEIGHT");


                for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                {
                    string CompetencyID = String.Empty;
                    string SupervisorRating = String.Empty;
                    string SupervisorWeight = String.Empty;

                    DropDownList ddlCompetencyRating = (grdvCompetencies.Rows[i].FindControl("ddlSupervisorRating") as DropDownList);

                    CompetencyID = HttpUtility.HtmlDecode(grdvCompetencies.Rows[i].Cells[1].Text).ToString();

                    if (ddlCompetencyRating.SelectedIndex > 0)
                    {
                        double weight = 0.0;
                        if (Double.TryParse(ddlCompetencyRating.SelectedValue.ToString(), out weight))
                        {
                            TotalSupervisorScore += weight;

                            SupervisorRating = ddlCompetencyRating.SelectedItem.Text.Trim();
                            SupervisorWeight = ddlCompetencyRating.SelectedValue.Trim();
                        }
                        else
                        {
                            TotalSupervisorScore += 0.0;
                        }
                    }
                    else
                    {
                        TotalSupervisorScore += 0.0;
                    }

                    DataRow drEmployeeCompetency = dtEmployeeCompetencies.NewRow();

                    drEmployeeCompetency["COMPETENCY_ID"] = CompetencyID;
                    drEmployeeCompetency["SUPERVISOR_RATING"] = SupervisorRating;
                    drEmployeeCompetency["SUPERVISOR_WEIGHT"] = SupervisorWeight;

                    dtEmployeeCompetencies.Rows.Add(drEmployeeCompetency);
                }


                SCADH.Finalize(TotalSupervisorScore.ToString(), SupervisorComments, CompetencyProfileID, AssessmentToken, AssessmentID, EmployeeID, YearOfAssessment, ModifiedBy, dtEmployeeCompetencies.Copy());

                CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_FINALIZED;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblStatus);

                LoadData();
                FinalizeCheck();
                DisplayFinalizeMessage();
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SCADH = null;
                dtCompetencies.Dispose();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");

            Utility.Errorhandler.ClearError(lblStatus);            
            ClearFields();
        }

        #endregion

        #region Methods

        void LoadAssessmentPurposes()
        {
            SupervisorCompetencyAssessmentDataHandler SEADH;
            DataTable AssessmentPurposes = new DataTable();
            DataTable SubordinatesInfo = new DataTable();
            try
            {
                log.Debug("LoadAssessmentPurposes()");

                SEADH = new SupervisorCompetencyAssessmentDataHandler();
                AssessmentPurposes = SEADH.PopulateAssessmentPurposes(hfAssessmentID.Value.Trim()).Copy();
                SubordinatesInfo = SEADH.PopulateSubordinatesInfo(hfEmployeeID.Value.Trim()).Copy();
                if ((AssessmentPurposes.Rows.Count > 0) && (SubordinatesInfo.Rows.Count > 0))
                {

                    string SubName = SubordinatesInfo.Rows[0]["TITLE"].ToString() +" "+ SubordinatesInfo.Rows[0]["INITIALS_NAME"].ToString();
                    string SubDesignation = SubordinatesInfo.Rows[0]["DESIGNATION_NAME"].ToString();
                    string SubRole = SubordinatesInfo.Rows[0]["ROLE_NAME"].ToString();

                    DisplayAssessmentName += @"
                                                    <table style='margin: auto;'>
                                                        <tr>
                                                            <td style='text-align: right;'>Assessment</td>
                                                            <td>:</td>
                                                            <td style='text-align: left;'>" + AssessmentPurposes.Rows[0]["ASSESSMENT_NAME"].ToString() + @"</td>
                                                        </tr>
                                                        <tr>
                                                            <td style='text-align: right;'>Employee</td>
                                                            <td>:</td>
                                                            <td style='text-align: left;'>" + SubName + @"</td>
                                                        </tr>
                                                        <tr>
                                                            <td style='text-align: right;'>Designation</td>
                                                            <td>:</td>
                                                            <td style='text-align: left;'>" + SubDesignation + @"</td>
                                                        </tr>
                                                        <tr>
                                                            <td style='text-align: right;'>Role</td>
                                                            <td>:</td>
                                                            <td style='text-align: left;'>" + SubRole + @"</td>
                                                        </tr>
                                                    </table>
                                                ";




                    //DisplayAssessmentName = AssessmentPurposes.Rows[0]["ASSESSMENT_NAME"].ToString();





                    DisplayAssessmentPurposes += "<table style='margin:auto;'><tr><td style='text-align:left;'>";
                    DisplayAssessmentPurposes += "<span>Assessment Purpose(s)</span>";
                    DisplayAssessmentPurposes += "<ul style='margin-top: 0px;'>";
                    for (int i = 0; i < AssessmentPurposes.Rows.Count; i++)
                    {
                        DisplayAssessmentPurposes += "<li>";
                        DisplayAssessmentPurposes += AssessmentPurposes.Rows[i]["NAME"].ToString();
                        DisplayAssessmentPurposes += "</li>";
                    }
                    DisplayAssessmentPurposes += "</ul>";
                    DisplayAssessmentPurposes += "</td></tr></table>";
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SEADH = null;
                AssessmentPurposes.Dispose();
                SubordinatesInfo.Dispose();
            }
        }

        void LoadData()
        {
            SupervisorCompetencyAssessmentDataHandler SCADH = new SupervisorCompetencyAssessmentDataHandler();
            DataTable dtCompetencies = new DataTable();
            try
            {
                log.Debug("LoadAssessmentPurposes()");

                LoadCompetencyRating();

                dtCompetencies = SCADH.Populate(hfAssessmentID.Value.ToString(), hfEmployeeID.Value.ToString(), hfYearOfAssessment.Value.ToString()).Copy();
                grdvCompetencies.DataSource = dtCompetencies.Copy();
                grdvCompetencies.DataBind();

                SetComments();
                SetUpdateButton();

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SCADH = null;
                dtCompetencies.Dispose();
            }
        }

        void LoadCompetencyRating()
        {
            SupervisorCompetencyAssessmentDataHandler SCADH = new SupervisorCompetencyAssessmentDataHandler();
            try
            {
                log.Debug("LoadCompetencyRating()");

                dtRatings = SCADH.PopulateProficiencyLevels(hfEmployeeID.Value.ToString()).Copy();
                lblAllocatedWeights.Text=String.Empty;
                if (dtRatings.Rows.Count > 0)
                {
                    lblAllocatedWeights.Text += @"<table style='margin:auto;width:100%;'>";
                    lblAllocatedWeights.Text += @"<tr>";

                    lblAllocatedWeights.Text += @"<th>Rating</th>";
                    lblAllocatedWeights.Text += @"<th>Weight</th>";
                    lblAllocatedWeights.Text += @"<th style='text-align:left;'>Description</th>";

                    lblAllocatedWeights.Text += @"</tr>";


                    //lblAllocatedWeights.Text += " | ";
                    for (int i = 0; i < dtRatings.Rows.Count; i++)
                    {
                        lblAllocatedWeights.Text += @"<tr>";

                        lblAllocatedWeights.Text += @"<td>" + dtRatings.Rows[i]["RATING"].ToString() + @"</td>";
                        lblAllocatedWeights.Text += @"<td>" + dtRatings.Rows[i]["WEIGHT"].ToString() + @"</td>";
                        lblAllocatedWeights.Text += @"<td style='text-align:left;'>" + dtRatings.Rows[i]["REMARKS"].ToString() + @"</td>";

                        lblAllocatedWeights.Text += @"</tr>";
                        //lblAllocatedWeights.Text += dtRatings.Rows[i]["RATING"].ToString() + " - " + dtRatings.Rows[i]["WEIGHT"].ToString() + " | ";
                    }


                    lblAllocatedWeights.Text += @"</table>";
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SCADH = null;
            }
        }

        void SetComments()
        {
            try
            {
                log.Debug("SetComments()");

                if (grdvCompetencies.Rows.Count > 0)
                {
                    string Comments = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[9].Text).ToString().Trim();
                    txtSupervisorComment.Text = Comments;
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        void SetUpdateButton()
        {
            log.Debug("SetUpdateButton()");

            Boolean isPreviousWeightsExists = false;
            Boolean isPreviousCommentsExists = false;

            for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
            {
                DropDownList ddlCompetencyRating = (grdvCompetencies.Rows[i].FindControl("ddlSupervisorRating") as DropDownList);
                if (ddlCompetencyRating.SelectedIndex > 0)
                {
                    isPreviousWeightsExists = true;
                    break;
                }
            }

            if (txtSupervisorComment.Text != String.Empty)
            {
                isPreviousCommentsExists = true;
            }

            //if ((isPreviousWeightsExists) || (isPreviousCommentsExists))
            //{
            //    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            //}
            //else
            //{
            //    btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            //}

        }

        void FinalizeCheck()
        {
            SupervisorCompetencyAssessmentDataHandler SCADH = new SupervisorCompetencyAssessmentDataHandler();
            try
            {
                log.Debug("FinalizeCheck()");

                string CompetencyProfileID = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[8].Text).ToString();
                string AssessmentToken = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[0].Text).ToString();
                string AssessmentID = hfAssessmentID.Value.ToString().Trim();
                string EmployeeID = hfEmployeeID.Value.ToString().Trim();
                string YearOfAssessment = hfYearOfAssessment.Value.ToString().Trim();

                if (SCADH.IsFinalized(hfAssessmentID.Value, hfEmployeeID.Value, hfYearOfAssessment.Value))
                {
                    for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                    {
                        DropDownList ddlCompetencyRating = (grdvCompetencies.Rows[i].FindControl("ddlSupervisorRating") as DropDownList);
                        ddlCompetencyRating.Enabled = false;
                        txtSupervisorComment.Enabled = false;

                        btnSave.Enabled = false;
                        btnSaveCompetency.Enabled = false;
                        btnClear.Enabled = false;
                    }
                }
                else
                {
                    for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                    {
                        DropDownList ddlCompetencyRating = (grdvCompetencies.Rows[i].FindControl("ddlSupervisorRating") as DropDownList);
                        ddlCompetencyRating.Enabled = true;
                        txtSupervisorComment.Enabled = true;

                        btnSave.Enabled = true;
                        btnSaveCompetency.Enabled = true;
                        btnClear.Enabled = true;
                    }
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }
        }

        void ClearFields()
        {
            try
            {
                log.Debug("ClearFields()");

                for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                {
                    DropDownList ddlCompetencyRating = (grdvCompetencies.Rows[i].FindControl("ddlSupervisorRating") as DropDownList);
                    ddlCompetencyRating.SelectedIndex = 0;
                }
                Utility.Utils.clearControls(false, txtSupervisorComment);
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {

            }

        }

        void DisplayFinalizeMessage()
        {
            SupervisorCompetencyAssessmentDataHandler SCADH = new SupervisorCompetencyAssessmentDataHandler();
            string SatusCode = String.Empty;
            try
            {
                log.Debug("DisplayFinalizeMessage()");
                Boolean DisplayStatus = true;
                SatusCode = SCADH.GetCompetencyAssessmentStatus(hfAssessmentID.Value, hfYearOfAssessment.Value, hfEmployeeID.Value);
                if (SatusCode == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                {
                    for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                    {
                        DropDownList ddlSupervisorRating = (grdvCompetencies.Rows[i].FindControl("ddlSupervisorRating") as DropDownList);
                        if (ddlSupervisorRating.SelectedIndex == 0)
                        {
                            DisplayStatus = false;
                        }
                    }


                    if (txtSupervisorComment.Text == String.Empty)
                    {
                        DisplayStatus = false;
                    }

                    if (DisplayStatus == true)
                    {
                        //Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Please complete the competency assessment.", lblFinalizeNotice);
                        lblFinalizeNotice.Text = "Please complete the competency assessment.";
                    }
                    else
                    {
                        Utility.Errorhandler.ClearError(lblFinalizeNotice);
                    }
                }
                else
                {
                    Utility.Errorhandler.ClearError(lblFinalizeNotice);
                }
            }
            catch (Exception exp)
            {
                log.Debug(exp.Message);
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblStatus);
            }
            finally
            {
                SCADH = null;
                SatusCode = String.Empty;
            }
        }

        void SetUpdateButtonText()
        {
            try
            {
                log.Debug("SetUpdateButtonText()");
                string Isupdate = String.Empty;
                for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                {
                    DropDownList ddlSupervisorRating  = (grdvCompetencies.Rows[i].FindControl("ddlSupervisorRating") as DropDownList);
                    if (ddlSupervisorRating.Items.Count > 0)
                    {
                        if (ddlSupervisorRating.SelectedIndex > 0)
                        {
                            Isupdate = "1";
                            break;
                        }
                    }
                }
                if (txtSupervisorComment.Text != String.Empty)
                {
                    Isupdate = "1";
                }


                if (Isupdate == "1")
                {
                    btnSaveCompetency.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                }
                else
                {
                    btnSaveCompetency.Text = Constants.CON_SAVE_BUTTON_TEXT;
                }
            }
            catch (Exception ex)
            {
                log.Error("SetUpdateButtonText()" + ex.Message);
                throw ex;
            }
        }

        void DisplayChart()
        {
            log.Debug("DisplayChart()");
            SupervisorCompetencyAssessmentDataHandler SCADH = new SupervisorCompetencyAssessmentDataHandler();
            DataTable dtPreviousCompetencies = new DataTable();
            try
            {
                dtPreviousCompetencies = SCADH.PopulatePreviousData(hfEmployeeID.Value).Copy();
                string JSLabel = String.Empty;
                string JSData = String.Empty;

                for (int i = 0; i < dtPreviousCompetencies.Rows.Count; i++)
                {
                    if (JSLabel == String.Empty)
                    {
                        JSLabel += "'" + dtPreviousCompetencies.Rows[i]["SUPERVISOR_COMPLETED_DATE"].ToString().Trim() + "'";
                    }
                    else
                    {
                        JSLabel += ", '" + dtPreviousCompetencies.Rows[i]["SUPERVISOR_COMPLETED_DATE"].ToString().Trim() + "'";
                    }

                    if (JSData == String.Empty)
                    {
                        JSData += "'" + dtPreviousCompetencies.Rows[i]["TOTAL_SUPERVISOR_SCORE"].ToString().Trim() + "'";
                    }
                    else
                    {
                        JSData += ", '" + dtPreviousCompetencies.Rows[i]["TOTAL_SUPERVISOR_SCORE"].ToString().Trim() + "'";
                    }
                }

                string ChartString = @"

                                        <table style='width:100%;'>
		                                    <tr>
			                                    <td style='vertical-align:top;'>
			                                     Assessment Instructions :-
                                                 <br/>
			                                    <ul>
				                                    <li>You can partially evaluate competencies and save/update given scores without evaluate all the competencies at one step.</li><br/><br/>
				                                    <li>After evaluating all competencies. You need to click complete button to complete the competency evaluation process.</li>
			                                    </ul>
			                                    </td>
                                                <td></td>
			                                    <td>
				                                    <div style='width:450px;'>
					                                    <canvas id='canvas'></canvas>
				                                    </div>
			                                    </td>
		                                    </tr>
	                                    </table
                                        <br>
                                        <br>
                                        <script>
                                            var config = {
                                                type: 'line',
                                                data: {
                                                    labels: [" + JSLabel + @"],
                                                    datasets: [{
                                                        label: 'Employee Competency Achievement(s)',
                                                        data: [" + JSData + @"],
                                                        fill: false,
                                                    }]
                                                },
                                                options: {
                                                    responsive: true,
                                                    hover: {
                                                        mode: 'dataset'
                                                    },
                                                    scales: {
                                                        xAxes: [{
                                                            display: true,
                                                            scaleLabel: {
                                                                display: true,
                                                                labelString: 'Year'
                                                            }
                                                        }],
                                                        yAxes: [{
                                                            display: true,
                                                            scaleLabel: {
                                                                display: true,
                                                                labelString: 'Score'
                                                            },
                                                            ticks: {
                                                                suggestedMin: 0,
                                                                suggestedMax: 100,
                                                            }
                                                        }]
                                                    }
                                                }
                                            };

                                            $.each(config.data.datasets, function(i, dataset) {
                                                dataset.borderColor =  'rgba(0,0,255,1)';
                                                dataset.backgroundColor =  'rgba(0,0,255,1)';
                                                dataset.pointBorderColor = 'rgba(255,0,0,1)';
                                                dataset.pointBackgroundColor =  'rgba(255,0,0,1)';
                                                dataset.pointBorderWidth = 1;
                                            });

                                            window.onload = function() {
                                                var ctx = document.getElementById('canvas').getContext('2d');
                                                window.myLine = new Chart(ctx, config);
                                            };

                                        </script>                                      

                                    ";

                lblGoalChart.Text = ChartString;
            }
            catch (Exception ex)
            {
                log.Error("DisplayChart() : " + ex.Message);
                throw ex;
            }
            finally
            {
                dtPreviousCompetencies.Dispose();
                SCADH = null;
            }
        }

        #endregion

    }
}