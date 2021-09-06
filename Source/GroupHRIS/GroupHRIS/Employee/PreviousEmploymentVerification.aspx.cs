using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.Employee;
using System.Data;
using System.Text;
using DataHandler.Userlogin;

namespace GroupHRIS.Employee
{
    public partial class PreviousEmploymentVerification : System.Web.UI.Page
    {
        void fillCompanyDDL()
        {
            PreviousExperienceVerificationDataHandler PEVDH = new PreviousExperienceVerificationDataHandler();
            DataTable dt = new DataTable();

            ddlCompany.Items.Clear();

            string companyID = (Session["KeyCOMP_ID"] as string);
            dt = PEVDH.populateCompanies(companyID).Copy();

            ddlCompany.Items.Add(new ListItem("", ""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string text = dt.Rows[i]["COMP_NAME"].ToString();
                string value = dt.Rows[i]["COMPANY_ID"].ToString();

                ddlCompany.Items.Add(new ListItem(text, value));
            }
        }

        void fillDepartmentDDL()
        {
            Utility.Errorhandler.ClearError(lblStatus);
            if (ddlCompany.SelectedIndex == 0)
            {
                ddlDepartment.Items.Clear();
                ddlDivision.Items.Clear();
            }
            else
            {
                string companyID = (Session["KeyCOMP_ID"] as string);
                if (companyID != Constants.CON_UNIVERSAL_COMPANY_CODE)
                {
                    PreviousExperienceVerificationDataHandler PEVDH = new PreviousExperienceVerificationDataHandler();
                    DataTable dt = new DataTable();

                    ddlDepartment.Items.Clear();

                    dt = PEVDH.populateDepartments(companyID).Copy();

                    ddlDepartment.Items.Add(new ListItem("", ""));


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string text = dt.Rows[i]["DEPT_NAME"].ToString();
                        string value = dt.Rows[i]["DEPT_ID"].ToString();

                        ddlDepartment.Items.Add(new ListItem(text, value));
                    }
                }
                if (ddlCompany.SelectedIndex > 0)
                {
                    PreviousExperienceVerificationDataHandler PEVDH = new PreviousExperienceVerificationDataHandler();
                    DataTable dt = new DataTable();

                    ddlDepartment.Items.Clear();

                    dt = PEVDH.populateDepartments(ddlCompany.SelectedValue).Copy();

                    ddlDepartment.Items.Add(new ListItem("", ""));


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string text = dt.Rows[i]["DEPT_NAME"].ToString();
                        string value = dt.Rows[i]["DEPT_ID"].ToString();

                        ddlDepartment.Items.Add(new ListItem(text, value));
                    }
                }
            }
        }

        void fillDivisionDDL()
        {
            Utility.Errorhandler.ClearError(lblStatus);
            if (ddlDepartment.SelectedIndex == 0)
            {
                ddlDivision.Items.Clear();
            }
            else
            {

                PreviousExperienceVerificationDataHandler PEVDH = new PreviousExperienceVerificationDataHandler();
                DataTable dt = new DataTable();

                ddlDivision.Items.Clear();

                string DepartmentID = ddlDepartment.SelectedValue;

                dt = PEVDH.populateDivisions(DepartmentID).Copy();

                ddlDivision.Items.Add(new ListItem("", ""));


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string text = dt.Rows[i]["DIV_NAME"].ToString();
                    string value = dt.Rows[i]["DIVISION_ID"].ToString();

                    ddlDivision.Items.Add(new ListItem(text, value));
                }
            }
        }

        void fillStatusDDL()
        {
            Utility.Errorhandler.ClearError(lblStatus);
            ddlExperienceStatus.Items.Add(new ListItem("", ""));
            ddlExperienceStatus.Items.Add(new ListItem(Constants.CON_PREVIOUS_EMPLOYMENT_PENDING_TEXT, Constants.CON_PREVIOUS_EMPLOYMENT_PENDING_CODE));
            ddlExperienceStatus.Items.Add(new ListItem(Constants.CON_PREVIOUS_EMPLOYMENT_APPROVED_TEXT, Constants.CON_PREVIOUS_EMPLOYMENT_APPROVED_CODE));
            ddlExperienceStatus.Items.Add(new ListItem(Constants.CON_PREVIOUS_EMPLOYMENT_REJECTED_TEXT, Constants.CON_PREVIOUS_EMPLOYMENT_REJECTED_CODE));
        }

        void loadEmployeeGrid()
        {
            PreviousExperienceVerificationDataHandler PEVDH = new PreviousExperienceVerificationDataHandler();
            DataTable dt = new DataTable();

            Utility.Errorhandler.ClearError(lblStatus);

            string CompanyID = (string)Session["KeyCOMP_ID"];

            //if (ddlExperienceStatus.SelectedIndex != 0)
            //{
            //    if (CompanyID == Constants.CON_UNIVERSAL_COMPANY_CODE)
            //    {
            //        if (txtemployee.Text == String.Empty)
            //        {
            //            dt = PEVDH.populateEmployee(ddlExperienceStatus.SelectedValue).Copy();
            //        }
            //        else
            //        {
            //            dt = PEVDH.populateEmployeeIND(ddlExperienceStatus.SelectedValue, txtemployee.Text).Copy();
            //        }
            //    }
            //    else
            //    {
            //        if (txtemployee.Text == String.Empty)
            //        {
            //            dt = PEVDH.populateEmployee(ddlExperienceStatus.SelectedValue, CompanyID).Copy();
            //        }
            //        else
            //        {
            //            dt = PEVDH.populateEmployeeINDCompany(ddlExperienceStatus.SelectedValue, CompanyID, txtemployee.Text).Copy();
            //        }
            //    }
            //}
            //else
            //{
            //    if (CompanyID == Constants.CON_UNIVERSAL_COMPANY_CODE)
            //    {
            //        if (txtemployee.Text == String.Empty)
            //        {
            //            dt = PEVDH.populateEmployee().Copy();
            //        }
            //        else
            //        {
            //            dt = PEVDH.populateEmployeeINDStateless(txtemployee.Text).Copy();
            //        }
            //    }
            //    else
            //    {
            //        if (txtemployee.Text == String.Empty)
            //        {
            //            dt = PEVDH.populateEmployeeStateLess(CompanyID).Copy();
            //        }
            //        else
            //        {
            //            dt = PEVDH.populateEmployeeINDCompanyStateless(CompanyID, txtemployee.Text).Copy();
            //        }
            //    }
            //}

            if ((CompanyID != Constants.CON_UNIVERSAL_COMPANY_CODE) && (ddlCompany.SelectedIndex == 0))
            {
                ddlCompany.SelectedIndex = 1;
                fillDepartmentDDL();
            }

            string companyID = "";
            if (ddlCompany.Items.Count > 0)
            {
                companyID = ddlCompany.SelectedValue;
            }

            string departmentID = "";
            if (ddlDepartment.Items.Count > 0)
            {
                departmentID = ddlDepartment.SelectedValue;
            }

            string divisionID = "";
            if (ddlDivision.Items.Count > 0)
            {
                divisionID = ddlDivision.SelectedValue;
            }

            string employeeID = "";
            if (txtemployee.Text != "")
            {
                employeeID = txtemployee.Text.Trim();
            }

            string status = "";
            if (ddlExperienceStatus.Items.Count > 0)
            {
                if (ddlExperienceStatus.SelectedIndex != 0)
                {
                    status = ddlExperienceStatus.SelectedValue;
                }
            }

            if (txtemployee.Text == "")
            {
                if (ddlCompany.SelectedIndex == 0)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Please Select Employee or Company", lblStatus);
                    return;
                }
            }

            dt = PEVDH.populate(companyID, departmentID, divisionID, employeeID, status).Copy();

            grdvEmployee.DataSource = dt;
            grdvEmployee.DataBind();

            lockRecords();
        }

        void previousEmployments(string employeeID)
        {
            PreviousExperienceVerificationDataHandler PEVDH = new PreviousExperienceVerificationDataHandler();
            DataTable dt = new DataTable();
            //dt = PEVDH.populateEmployments(employeeID).Copy();           
            if (ddlExperienceStatus.SelectedIndex == 0)
            {
                dt = PEVDH.populateEmploymentStateLess(employeeID).Copy();
            }
            else
            {
                dt = PEVDH.populateEmployments(employeeID, ddlExperienceStatus.SelectedValue).Copy();
            }
            Session["previousEmployments"] = dt;

            grdvPreviousEmployment.DataSource = dt;
            grdvPreviousEmployment.DataBind();
            lblPreviousExperience.Visible = true;

            lockRecords();

            if (dt.Rows.Count > 0)
            {
                btnSave.Visible = true;
                btnClearAll.Visible = true;
            }
            else
            {
                btnSave.Visible = false;
                btnClearAll.Visible = false; 
                lblPreviousExperience.Visible = false;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillStatusDDL();
                fillCompanyDDL();
                ddlExperienceStatus.SelectedIndex = 1;
                //loadEmployeeGrid();
            }

            if (IsPostBack)
            {
                if (hfCaller.Value == "txtemployee")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtemployee.Text = hfVal.Value;
                    }
                    if (txtemployee.Text != "")
                    {
                        //Postback Methods
                        Utility.Errorhandler.ClearError(lblStatus);
                    }
                }
            }
        }

        protected void grdvEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            string employeeID = grdvEmployee.Rows[grdvEmployee.SelectedIndex].Cells[0].Text;
            string epf = grdvEmployee.Rows[grdvEmployee.SelectedIndex].Cells[1].Text;
            Session["employeeID"] = employeeID;
            previousEmployments(employeeID);
        }

        protected void grdvEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdvEmployee, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void grdvEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdvEmployee.PageIndex = e.NewPageIndex;
            loadEmployeeGrid();
            Utility.Errorhandler.ClearError(lblStatus);
            grdvPreviousEmployment.DataSource = null;
            grdvPreviousEmployment.DataBind();
            lblPreviousExperience.Visible = false;
        }

        protected void rdVerify_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < grdvPreviousEmployment.Rows.Count; i++)
            {
                RadioButton rdBtn = ((RadioButton)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("rdVerify"));
                RadioButton rdBtnRj = ((RadioButton)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("rdReject"));
                CheckBox chkBx = ((CheckBox)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("chkServiceLetter"));
                TextBox txReject = ((TextBox)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("txtRejectReason"));
                RequiredFieldValidator rqValidator = ((RequiredFieldValidator)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("RequiredFieldValidator1"));

                if ((rdBtn.Checked == true) || (rdBtnRj.Checked == true))
                {

                    if ((chkBx.Checked == true) && (rdBtn.Checked == false))
                    {
                        chkBx.Checked = false;
                    }

                    if (rdBtn.Checked == true)
                    {
                        chkBx.Visible = true;
                        txReject.Visible = false;
                        rqValidator.ValidationGroup = "MainX";
                    }
                    else
                    {
                        chkBx.Checked = false;
                        chkBx.Visible = false;
                        txReject.Visible = true;
                        rqValidator.ValidationGroup = "Main";
                    }
                }

                if ((rdBtn.Checked != true) && (rdBtnRj.Checked != true))
                {
                    rqValidator.ValidationGroup = "MainX";
                }
            }
        }

        protected void rdReject_CheckedChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblStatus);

            for (int i = 0; i < grdvPreviousEmployment.Rows.Count; i++)
            {
                RadioButton rdBtn = ((RadioButton)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("rdVerify"));
                RadioButton rdBtnRj = ((RadioButton)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("rdReject"));
                CheckBox chkBx = ((CheckBox)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("chkServiceLetter"));
                TextBox txReject = ((TextBox)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("txtRejectReason"));
                RequiredFieldValidator rqValidator = ((RequiredFieldValidator)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("RequiredFieldValidator1"));

                if ((rdBtn.Checked == true) || (rdBtnRj.Checked == true))
                {
                    if ((chkBx.Checked == true) && (rdBtn.Checked == false))
                    {
                        chkBx.Checked = false;
                    }

                    if (rdBtn.Checked == true)
                    {
                        chkBx.Visible = true;
                        txReject.Visible = false;
                        rqValidator.ValidationGroup = "MainX";
                    }
                    else
                    {
                        chkBx.Checked = false;
                        chkBx.Visible = false;
                        txReject.Visible = true;
                        rqValidator.ValidationGroup = "Main";
                    }
                }

                if ((rdBtn.Checked != true) && (rdBtnRj.Checked != true))
                {
                    rqValidator.ValidationGroup = "MainX";
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Utility.Errorhandler.ClearError(lblStatus);

                if (grdvPreviousEmployment.Rows.Count > 0)
                {
                    PreviousExperienceVerificationDataHandler PEVDH = new PreviousExperienceVerificationDataHandler();

                    string loginUser = (string)Session["KeyUSER_ID"];
                    DataTable dtVerify = new DataTable();
                    dtVerify.Columns.Add("LineNumber");
                    dtVerify.Columns.Add("AssessmentStatusCode");
                    dtVerify.Columns.Add("ServiceLetter");
                    dtVerify.Columns.Add("RejectReason");



                    string VerifytRecord = @"
                                        <table style='border-collapse: collapse; border: 1px solid black;'>
                                            <tr style='border: 1px solid black;'>
                                                <td style='border: 1px solid black; padding: 5px;'>DESIGNATION</td>
                                                <td style='border: 1px solid black; padding: 5px;'>ORGANIZATION</td>
                                                <td style='border: 1px solid black; padding: 5px;'>FROM DATE</td>
                                                <td style='border: 1px solid black; padding: 5px;'>TO DATE	</td>
                                                <td style='border: 1px solid black; padding: 5px;'>PHONE NUMBER</td>
                                                <td style='border: 1px solid black; padding: 5px;'>ADDRESS</td>
                                                <td style='border: 1px solid black; padding: 5px;'>VERIFICATION STATUS</td>
                                            </tr>       
                                   ";


                    for (int i = 0; i < grdvPreviousEmployment.Rows.Count; i++)
                    {
                        DataRow dr = dtVerify.NewRow();

                        string LINE_NO = grdvPreviousEmployment.Rows[i].Cells[0].Text;
                        RadioButton rdVerify = ((RadioButton)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("rdVerify"));
                        RadioButton rdBtnRj = ((RadioButton)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("rdReject"));
                        CheckBox chkServiceLetter = ((CheckBox)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("chkServiceLetter"));
                        TextBox txReject = ((TextBox)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("txtRejectReason"));

                        if ((rdVerify.Checked != true) && (rdBtnRj.Checked != true))
                        {

                        }
                        else
                        {

                            string status = Constants.CON_PREVIOUS_EMPLOYMENT_PENDING_CODE;
                            string serviceLetter = String.Empty;

                            if (rdVerify.Checked == true)
                            {
                                status = Constants.CON_PREVIOUS_EMPLOYMENT_APPROVED_CODE;

                                if (chkServiceLetter.Checked == true)
                                {
                                    serviceLetter = Constants.CON_VERIFIED_WITH_SERVICE_LETTER;
                                }
                                else
                                {
                                    serviceLetter = Constants.CON_VERIFIED_WITHOUT_SERVICE_LETTER;
                                }
                            }
                            else
                            {
                                status = Constants.CON_PREVIOUS_EMPLOYMENT_REJECTED_CODE;
                            }

                            dr["LineNumber"] = LINE_NO;
                            dr["AssessmentStatusCode"] = status;
                            dr["ServiceLetter"] = serviceLetter;
                            if (txReject.Visible == true)
                            {
                                dr["RejectReason"] = txReject.Text;
                            }
                            else
                            {
                                dr["RejectReason"] = "";
                            }

                            dtVerify.Rows.Add(dr);


                            VerifytRecord += @"
                                                <tr style='border: 1px solid black;'>
                                                <td style='border: 1px solid black;'>" + grdvPreviousEmployment.Rows[i].Cells[1].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdvPreviousEmployment.Rows[i].Cells[2].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdvPreviousEmployment.Rows[i].Cells[3].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdvPreviousEmployment.Rows[i].Cells[4].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdvPreviousEmployment.Rows[i].Cells[5].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdvPreviousEmployment.Rows[i].Cells[6].Text + @"</td>";
                            if (rdVerify.Checked == true)
                            {
                                if (chkServiceLetter.Checked == true)
                                {
                                    VerifytRecord += @"<td style='border: 1px solid black;'>Accepted with Service Letter</td>
                                            </tr>
                                            ";
                                }
                                else
                                {
                                    VerifytRecord += @"<td style='border: 1px solid black;'>Accepted without Service Letter</td>
                                            </tr>
                                            ";
                                }
                            }
                            else
                            {
                                VerifytRecord += @"<td style='border: 1px solid black;'>Rejected. due to, <br/> " + txReject.Text + @"</td>
                                            </tr>
                                            ";
                            }
                        }
                    }

                    VerifytRecord += " </table>";
                    Session["VerifytRecord"] = VerifytRecord;

                    PEVDH.Update(loginUser, dtVerify);

                    DataTable EmailDT = new DataTable();
                    EmailDT = PEVDH.getEmailFromEmployeeID((Session["employeeID"] as string)).Copy();
                    if (EmailDT.Rows.Count > 0)
                    {
                        //Send Verification Summary Email
                        EmailHandler.SendDefaultEmailWithHTML("Experience Verification", EmailDT.Rows[0]["EMAIL"].ToString(), "", "Previous Experience Review Summary", getHigherEducationVerifiedMailContent());
                    }


                    string employeeID =  (Session["employeeID"] as string);
                    previousEmployments(employeeID);

                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_VERIFIED, lblStatus);

                    if (grdvPreviousEmployment.Rows.Count == 0)
                    {
                        btnSave.Visible = false;
                        btnClearAll.Visible = false; 
                        lblPreviousExperience.Visible = false;
                    }

                }
            }
            catch (Exception exp)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, lblStatus);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (ddlCompany.Items.Count > 0)
            {
                ddlCompany.SelectedIndex = 0;
            }
            if (ddlDepartment.Items.Count > 0)
            {
                ddlDepartment.SelectedIndex = 0;
            }
            if (ddlDivision.Items.Count > 0)
            {
                ddlDivision.SelectedIndex = 0;
            }
            ddlExperienceStatus.SelectedIndex = 1;
            txtemployee.Text = String.Empty;
            Utility.Errorhandler.ClearError(lblStatus);

            //loadEmployeeGrid();

            grdvEmployee.DataSource = null;
            grdvEmployee.DataBind();
            grdvPreviousEmployment.DataSource = null;
            grdvPreviousEmployment.DataBind();
            lblPreviousExperience.Visible = false;

            btnSave.Enabled = true;

            btnSave.Visible = false;
            btnClearAll.Visible = false; 
            lblPreviousExperience.Visible = false;

            ddlDepartment.Items.Clear();
            ddlDivision.Items.Clear();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblStatus);
            grdvPreviousEmployment.DataSource = null;
            grdvPreviousEmployment.DataBind();
            lblPreviousExperience.Visible = false;

            grdvEmployee.Columns[3].Visible = true;//COMPANY
            grdvEmployee.Columns[4].Visible = true;//DEPARTMENT
            grdvEmployee.Columns[5].Visible = true;//DIVISION
            loadEmployeeGrid();

            ControlGridColumnsByFilters();

            btnSave.Visible = false;
            btnClearAll.Visible = false; 
            lblPreviousExperience.Visible = false;
        }

        protected void grdvPreviousEmployment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Utility.Errorhandler.ClearError(lblStatus);
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int index = e.Row.RowIndex;
                    string status = (Session["previousEmployments"] as DataTable).Rows[index]["RECORD_STATUS"].ToString();
                    string verify = (Session["previousEmployments"] as DataTable).Rows[index]["VERIFIED_BY_SERVICE_LETTER"].ToString();

                    if (status != Constants.CON_PREVIOUS_EMPLOYMENT_PENDING_CODE)
                    {
                        RadioButton rdVerify = (e.Row.FindControl("rdVerify") as RadioButton);
                        RadioButton rdReject = (e.Row.FindControl("rdReject") as RadioButton);
                        CheckBox chkServiceLetter = (e.Row.FindControl("chkServiceLetter") as CheckBox);

                        if (status == Constants.CON_PREVIOUS_EMPLOYMENT_APPROVED_CODE)
                        {

                            rdVerify.Checked = true;
                            rdReject.Checked = false;

                            if (verify == Constants.CON_VERIFIED_WITH_SERVICE_LETTER)
                            {
                                chkServiceLetter.Checked = true;
                            }
                            else
                            {
                                chkServiceLetter.Checked = false;
                            }
                        }
                        else
                        {
                            rdVerify.Checked = false;
                            rdReject.Checked = true;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, lblStatus);
            }
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblStatus);

            //txtemployee.Text = String.Empty;
            ddlExperienceStatus.SelectedIndex = 1;
            loadEmployeeGrid();

            grdvPreviousEmployment.DataSource = null;
            grdvPreviousEmployment.DataBind();
            lblPreviousExperience.Visible = false;

            lockRecords();

            btnSave.Visible = false;
            btnClearAll.Visible = false; 
            lblPreviousExperience.Visible = false;
        }

        protected void chkServiceLetter_CheckedChanged(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblStatus);

            for (int i = 0; i < grdvPreviousEmployment.Rows.Count; i++)
            {
                RadioButton rdBtn = ((RadioButton)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("rdVerify"));
                RadioButton rdBtnRj = ((RadioButton)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("rdReject"));
                CheckBox chkBx = ((CheckBox)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("chkServiceLetter"));
                TextBox txReject = ((TextBox)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("txtRejectReason"));
                RequiredFieldValidator rqValidator = ((RequiredFieldValidator)grdvPreviousEmployment.Rows[i].Cells[7].FindControl("RequiredFieldValidator1"));

                if ((chkBx.Checked == true) && (rdBtn.Checked == false))
                {
                    chkBx.Checked = false;
                }

                if (rdBtn.Checked == true)
                {
                    chkBx.Visible = true;
                }
                else
                {
                    chkBx.Checked = false;
                    chkBx.Visible = false;
                }
            }
        }

        private StringBuilder getHigherEducationVerifiedMailContent()
        {

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(" Dear Sir/Madam, <br/> <br/>");

            stringBuilder.Append(" Your Previous Experience Record(s) has been verified  <br/> <br/>");

            stringBuilder.Append((Session["VerifytRecord"] as string));
            stringBuilder.Append(" <br/> <br/>");

            stringBuilder.Append(" Thank you. <br/> <br/>");
            stringBuilder.Append(" This is a system generated mail. <br/> <br/>");

            return stringBuilder;
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillDepartmentDDL();
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillDivisionDDL();
        }

        protected void ddlExperienceStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lockRecords();

            //Utility.Errorhandler.ClearError(lblStatus);
            //grdvPreviousEmployment.DataSource = null;
            //grdvPreviousEmployment.DataBind();
            //loadEmployeeGrid();
        }

        void lockRecords()
        {
            if (ddlExperienceStatus.SelectedValue != Constants.CON_PREVIOUS_EMPLOYMENT_PENDING_CODE)
            {
                btnSave.Enabled = false;
                grdvPreviousEmployment.Columns[7].Visible = false;
            }
            else
            {
                btnSave.Enabled = true;
                grdvPreviousEmployment.Columns[7].Visible = true;
            }
        }

        void ControlGridColumnsByFilters()
        {
            grdvEmployee.Columns[3].Visible = false;//COMPANY
            grdvEmployee.Columns[4].Visible = false;//DEPARTMENT
            grdvEmployee.Columns[5].Visible = false;//DIVISION


            if (ddlCompany.SelectedIndex == 0)
            {
                grdvEmployee.Columns[3].Visible = true;//COMPANY
                grdvEmployee.Columns[4].Visible = false;//DEPARTMENT
                grdvEmployee.Columns[5].Visible = false;//DIVISION
            }
            if (ddlCompany.SelectedIndex > 0)
            {
                grdvEmployee.Columns[3].Visible = false;//COMPANY
                grdvEmployee.Columns[4].Visible = true;//DEPARTMENT
                grdvEmployee.Columns[5].Visible = false;//DIVISION
            }
            if (ddlDepartment.SelectedIndex > 0)
            {
                grdvEmployee.Columns[3].Visible = false;//COMPANY
                grdvEmployee.Columns[4].Visible = false;//DEPARTMENT
                grdvEmployee.Columns[5].Visible = true;//DIVISION
            }
            if (txtemployee.Text!="")
            {
                grdvEmployee.Columns[3].Visible = true;//COMPANY
                grdvEmployee.Columns[4].Visible = false;//DEPARTMENT
                grdvEmployee.Columns[5].Visible = false;//DIVISION
            }
        }
    }
}