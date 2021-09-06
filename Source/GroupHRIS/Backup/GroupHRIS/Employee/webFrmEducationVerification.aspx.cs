using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler.Employee;
using DataHandler.MetaData;
using DataHandler.Userlogin;
using System.Text;
using Common;
using NLog;

namespace GroupHRIS.Employee
{
    public partial class webFrmEducationVerification : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string userID = "";
        private string keyEmpID = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsgHr);
            if (Session["KeyLOGOUT_STS"] == null)
            {
                log.Debug("Session Expired");
                Response.Redirect("../Login/MainLogout.aspx", false);
            }
            else
            {
                userID = Session["KeyUSER_ID"].ToString();
                keyEmpID = Session["KeyEMPLOYEE_ID"].ToString();
            }


            if (!IsPostBack)
            {

                if (Session["KeyUSER_ID"] != null)
                {
                    userID = Session["KeyUSER_ID"].ToString();
                    keyEmpID = Session["KeyEMPLOYEE_ID"].ToString();
                    fillAttempts();
                }
            }
            else
            {
                txtEmployeeID.Text = hfEmpID.Value;
                txtName.Text = hfName.Value;
                populateSecEduGridOL();
                populateSecEduGridAL();
                populateHighEduGrid();
            }
        }

        private void fillAttempts()
        {
            for (int i = 1; i <= 3; i++)
            {
                ddlOLAttempt.Items.Add(i.ToString());
                ddlALAttempt.Items.Add(i.ToString());
            }
        }

        private void populateSecEduGridOL()
        {
            SecondaryEducationDataHandler dhSecEdu = new SecondaryEducationDataHandler();
            DataTable dtSecEdu = new DataTable();

            string iSAl = "";
            string sAttemp = "";

            try
            {
                iSAl = "N";
                sAttemp = ddlOLAttempt.SelectedItem.Text.ToString();

                dtSecEdu = dhSecEdu.populateOLAL(txtEmployeeID.Text, sAttemp, iSAl);
                gvSecEduOL.DataSource = dtSecEdu;
                gvSecEduOL.DataBind();
                if (dtSecEdu.Rows.Count > 0)
                {
                    btnol.Visible = true;
                    rdol.Visible = true;
                }
                else
                {
                    btnol.Visible = false ;
                    rdol.Visible = false;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsgOL);
            }
            finally
            {
                dhSecEdu = null;
                dtSecEdu.Dispose();
            }
        }

        private void populateSecEduGridAL()
        {
            SecondaryEducationDataHandler dhSecEdu = new SecondaryEducationDataHandler();
            DataTable dtSecEdu = new DataTable();

            string iSAl = "";
            string sAttemp = "";

            try
            {
                iSAl = "Y";
                sAttemp = ddlALAttempt.SelectedItem.Text.ToString();

                dtSecEdu = dhSecEdu.populateOLAL(txtEmployeeID.Text, sAttemp, iSAl);
                gvSecEduAL.DataSource = dtSecEdu;
                gvSecEduAL.DataBind();

                if (dtSecEdu.Rows.Count > 0)
                {
                    btnal.Visible = true;
                    rdal.Visible = true;
                }
                else
                {
                    btnal.Visible = false;
                    rdal.Visible = false;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsgAL);
            }
            finally
            {
                dhSecEdu = null;
                dtSecEdu.Dispose();
            }
        }

        private void populateHighEduGrid()
        {
            HigherEducationDataHandler dhHighEdu = new HigherEducationDataHandler();
            DataTable dtHighEdu = new DataTable();

            try
            {
                dtHighEdu = dhHighEdu.populateValid(txtEmployeeID.Text , Constants.STATUS_INACTIVE_VALUE);
                gvHighEdu.DataSource = dtHighEdu;
                gvHighEdu.DataBind();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsgHr);
            }
            finally
            {
                dhHighEdu = null;
                dtHighEdu.Dispose();
            }
        }

        protected void gvHighEdu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvHighEdu.PageIndex = e.NewPageIndex;
            populateHighEduGrid();
        }

        protected void lnkHighVeryfy_Click(object sender, EventArgs e)
        {
            HigherEducationDataHandler dhHighEdu = new HigherEducationDataHandler();
            DataTable dtSecEdu = new DataTable();
            bool bUpdated = false;

            clearlabels();

            GridViewRow grdRow = (GridViewRow)((LinkButton)sender).NamingContainer;
            string lineNo = grdRow.Cells[12].Text;
            string status = grdRow.Cells[10].Text;

            string VerifytRecord = @"
                                        <table style='border-collapse: collapse; border: 1px solid black;'>
                                            <tr style='border: 1px solid black;'>
                                                <td style='border: 1px solid black;'><b>INSTITUTE</b></td>
                                                <td style='border: 1px solid black;'><b>PROGRAM</b></td>
                                                <td style='border: 1px solid black;'><b>PROGRAME NAME</b></td>
                                                <td style='border: 1px solid black;'><b>SECTOR</b></td>
                                                <td style='border: 1px solid black;'><b>DURATION</b></td>
                                                <td style='border: 1px solid black;'><b>FROM YEAR</b></td>
                                                <td style='border: 1px solid black;'><b>TO YEAR</b></td>
                                                <td style='border: 1px solid black;'><b>GRADE</b></td>
                                            </tr>
                                            <tr style='border: 1px solid black;'>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[1].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[2].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[3].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[4].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[5].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[6].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[7].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[8].Text + @"</td>
                                            </tr>
                                        </table>
                                   ";

            Session["VerifytRecord"] = VerifytRecord;

            try
            {
                if (status.Equals(Constants.STATUS_ACTIVE_VALUE))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Record already in verified STATUS.", lblMsgHr);
                }

                else if (dhHighEdu.verify(txtEmployeeID.Text, lineNo, userID))
                {
                    bUpdated = true;
                    //SEND VERIFICATION EMAIL

                    EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
                    string sEmpID = txtEmployeeID.Text.ToString();
                    string mailAddress = employeeDataHandler.getEmployeeEmail(sEmpID);

                    if (mailAddress.Trim() != "")
                    {
                        EmailHandler.SendDefaultEmailWithHTML("Education Verification", mailAddress, "", "Accepted - Higher Education Verification", getHigherEducationVerifiedMailContent());
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsgHr);
            }
            finally
            {
                dhHighEdu = null;
                dtSecEdu.Dispose();
            }

            if (bUpdated)
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_VERIFIED, lblMsgHr);

            populateHighEduGrid();
        }


        protected void lnkHighReject_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsgHr);

            GridViewRow grdRow = (GridViewRow)((LinkButton)sender).NamingContainer;
            string lineNo = grdRow.Cells[12].Text;
            string status = grdRow.Cells[10].Text;

            Session["lineNo"] = lineNo;
            Session["status"] = status;

            string RejectRecord = @"
                                        <table style='border-collapse: collapse; border: 1px solid black;'>
                                            <tr style='border: 1px solid black;'>
                                                <td style='border: 1px solid black;'><b>INSTITUTE</b></td>
                                                <td style='border: 1px solid black;'><b>PROGRAM</b></td>
                                                <td style='border: 1px solid black;'><b>PROGRAME NAME</b></td>
                                                <td style='border: 1px solid black;'><b>SECTOR</b></td>
                                                <td style='border: 1px solid black;'><b>DURATION</b></td>
                                                <td style='border: 1px solid black;'><b>FROM YEAR</b></td>
                                                <td style='border: 1px solid black;'><b>TO YEAR</b></td>
                                                <td style='border: 1px solid black;'><b>GRADE</b></td>
                                            </tr>
                                            <tr style='border: 1px solid black;'>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[1].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[2].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[3].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[4].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[5].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[6].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[7].Text + @"</td>
                                                <td style='border: 1px solid black;'>" + grdRow.Cells[8].Text + @"</td>
                                            </tr>
                                        </table>
                                   ";

            //RejectRecord = HttpUtility.HtmlEncode(RejectRecord);

            Session["RejectRecord"] = RejectRecord;

            pnlRejectReason.Visible = true;
        }

        protected void ddlOLAttempt_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clearlabels();
                populateSecEduGridOL();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsgOL);
            }
        }

        protected void ddlALAttempt_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clearlabels();
                populateSecEduGridAL();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsgAL);
            }
        }

        protected void btnol_Click(object sender, EventArgs e)
        {
            SecondaryEducationDataHandler dhSecEdu = new SecondaryEducationDataHandler();
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            try
            {
                clearlabels();
                string sStatus = "";
                string sActionStatus = "";
                string sAttempt = ddlOLAttempt.SelectedItem.Text.ToString();
                string sIsOL = "N";
                string sEmpID = txtEmployeeID.Text.ToString();
                string mailAddress = "";

                Utility.Errorhandler.ClearError(lblMsgOL);

                if ((rdol.SelectedValue == "9") && (txtRejectedReason.Text.Trim() == ""))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, " Please provide rejected reason", lblMsgOL);
                    return;
                }

                mailAddress = employeeDataHandler.getEmployeeEmail(sEmpID);

                if (rdol.SelectedValue == "1"){
                    sActionStatus = "Verified";
                    sStatus = Constants.STATUS_ACTIVE_VALUE;
                }else{
                    sActionStatus = "Rejected";
                    sStatus = Constants.STATUS_OBSOLETE_VALUE;
                }

                Boolean isupdated = dhSecEdu.verifyEntireSet(sEmpID, sIsOL, sAttempt, sStatus.ToString(), keyEmpID);
                
                if(isupdated == true){

                    if ((txtRejectedReason.Text.Trim() != "") && (mailAddress.Trim() != ""))
                    {
                        EmailHandler.SendDefaultEmail("Secondary Education Verification", mailAddress, "", "Results Rejection O/L", getRejectionMailContent(false, txtRejectedReason.Text.Trim()));
                    }
                    else if((txtRejectedReason.Text.Trim() == "") && (mailAddress.Trim() != ""))
                    {
                        EmailHandler.SendDefaultEmail("Secondary Education Verification", mailAddress, "", "O/L Results", getVerifiedMailContent("O/L"));
                    }

                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "O/L Details " + sActionStatus, lblMsgOL);
                    populateSecEduGridOL();

                    txtRejectedReason.Text = "";
                    txtRejectedReason.Visible = false;
                    Label5.Visible = false;

                }else{
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Unable to Update Details.", lblMsgOL);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsgOL);
            }
            finally
            {
                dhSecEdu = null;
                employeeDataHandler = null;
            }
        }

        protected void btnal_Click(object sender, EventArgs e)
        {
            SecondaryEducationDataHandler dhSecEdu = new SecondaryEducationDataHandler();
            EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();

            try
            {
                clearlabels();
                string sActionStatus = "";
                string sStatus = "";
                string sAttempt = ddlALAttempt.SelectedItem.Text.ToString();
                string sIsOL = "Y";
                string sEmpID = txtEmployeeID.Text.ToString();
                string mailAddress = "";

                Utility.Errorhandler.ClearError(lblMsgAL);

                if((rdal.SelectedValue == "9") && (txtAlRejectedReason.Text.Trim()==""))
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, " Please provide rejected reason", lblMsgAL);
                    return;
                }

                mailAddress = employeeDataHandler.getEmployeeEmail(sEmpID);
                
                if (rdal.SelectedValue == "1")
                {
                    sActionStatus = "Verified";
                    sStatus = Constants.STATUS_ACTIVE_VALUE;
                }
                else
                {
                    sActionStatus = "Rejected";
                    sStatus = Constants.STATUS_OBSOLETE_VALUE;
                }

                Boolean isupdated = dhSecEdu.verifyEntireSet(sEmpID, sIsOL, sAttempt, sStatus, keyEmpID);
                
                if (isupdated == true)
                {
                    if ((txtAlRejectedReason.Text.Trim() != "") && (mailAddress.Trim() != ""))
                    {
                        EmailHandler.SendDefaultEmail("Secondary Education Verification", mailAddress, "", "Results Rejection A/L", getRejectionMailContent(true, txtAlRejectedReason.Text.Trim()));
                    }
                    else if ((txtRejectedReason.Text.Trim() == "") && (mailAddress.Trim() != ""))
                    {
                        EmailHandler.SendDefaultEmail("Secondary Education Verification", mailAddress, "", "A/L Results", getVerifiedMailContent("A/L"));
                    }
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "A/L Details " + sActionStatus, lblMsgAL);
                    populateSecEduGridAL();

                    txtAlRejectedReason.Text = "";
                    txtAlRejectedReason.Visible = false;
                    Label6.Visible = false;
                }
                else
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Unable to Update Details.", lblMsgAL);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsgAL);
            }
            finally
            {
                dhSecEdu = null;
                employeeDataHandler = null;
            }
        }

        private void clearlabels()
        {
            Utility.Errorhandler.ClearError(lblMsgAL);
            Utility.Errorhandler.ClearError(lblMsgOL);
            Utility.Errorhandler.ClearError(lblMsgHr);
        }

        protected void rdol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdol.SelectedValue == "9")
            {
                txtRejectedReason.Visible = true;
                Label5.Visible = true;
            }
            else
            {
                txtRejectedReason.Visible = false;
                Label5.Visible = false;
            }
        }

        protected void rdal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdal.SelectedValue == "9")
            {
                txtAlRejectedReason.Visible = true;
                Label6.Visible = true;
            }
            else
            {
                txtAlRejectedReason.Visible = false;
                Label6.Visible = false;
            }
        }


        private StringBuilder getRejectionMailContent(Boolean isAl, string reason)
        {
            log.Debug("getRejectionMailContent : getRejectionMailContent(Boolean isAl, string reason)");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(" Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);

            if (isAl == true)
            {
                stringBuilder.Append(" Your AL results has been rejected due to the reason stated below " + Environment.NewLine);
                stringBuilder.Append(reason.Trim() + Environment.NewLine + Environment.NewLine);
                
            }
            else if (isAl == false)
            {
                stringBuilder.Append(" Your OL results has been rejected due to the reason stated below " + Environment.NewLine);
                stringBuilder.Append(reason.Trim() + Environment.NewLine + Environment.NewLine);
            }

            stringBuilder.Append(" Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append(" This is a system generated mail." + Environment.NewLine);

            return stringBuilder;
        }

        private StringBuilder getHigherEducationRejectionMailContent(string reason)
        {
            log.Debug("getRejectionMailContent : getHigherEducationRejectionMailContent(string reason)");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(" Dear Sir/Madam <br/> <br/>");

            stringBuilder.Append(" Your Higher Education Record has been rejected due to the reason stated below,  <br/> <br/>");

            stringBuilder.Append(reason.Trim() + " <br/>");

            stringBuilder.Append(" <br/>");

            stringBuilder.Append((Session["RejectRecord"] as string));

            stringBuilder.Append(" <br/> <br/>");

            stringBuilder.Append(" Thank you." + " <br/> <br/>");
            stringBuilder.Append(" This is a system generated mail." + " <br/>");

            return stringBuilder;
        }

        private StringBuilder getVerifiedMailContent(String sAL_OL)
        {

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(" Dear Sir/Madam" + Environment.NewLine + Environment.NewLine);

            stringBuilder.Append(" Your " + sAL_OL.Trim() + " results has been verified and Approved " + Environment.NewLine);

            stringBuilder.Append(" Thank you." + Environment.NewLine + Environment.NewLine);
            stringBuilder.Append(" This is a system generated mail." + Environment.NewLine);            

            return stringBuilder;
        }

        private StringBuilder getHigherEducationVerifiedMailContent()
        {

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(" Dear Sir/Madam <br/> <br/>");

            stringBuilder.Append(" Your Higher Education Record has been Accepted  <br/> <br/>");

            stringBuilder.Append((Session["VerifytRecord"] as string));
            stringBuilder.Append(" <br/> <br/>");

            stringBuilder.Append(" Thank you. <br/> <br/>");
            stringBuilder.Append(" This is a system generated mail. <br/> <br/>");

            return stringBuilder;
        }

        protected void btnRejectSave_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsgHr);
            if (txtRejectReason.Text == String.Empty)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Reject Reason is Required", lblMsgHr);
                return;
            }
            else
            {
                EmployeeDataHandler employeeDataHandler = new EmployeeDataHandler();
                string sEmpID = txtEmployeeID.Text.ToString();
                string mailAddress = employeeDataHandler.getEmployeeEmail(sEmpID);


                HigherEducationDataHandler dhHighEdu = new HigherEducationDataHandler();
                DataTable dtSecEdu = new DataTable();
                bool bUpdated = false;

                clearlabels();                

                string lineNo = Session["lineNo"].ToString();
                string status = Session["status"].ToString();

                try
                {
                    if (status.Equals(Constants.STATUS_OBSOLETE_VALUE))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Record already in rejected STATUS.", lblMsgHr);
                    }

                    else if (dhHighEdu.reject(txtEmployeeID.Text, lineNo, userID, txtRejectReason.Text.Trim()))
                    {
                        bUpdated = true;
                        //ADD REJECT REASON AND REJECTED E-MAIL

                        if ((txtRejectReason.Text.Trim() != "") && (mailAddress.Trim() != ""))
                        {
                            EmailHandler.SendDefaultEmailWithHTML("Education Verification", mailAddress, "", "Rejected - Higher Education Verification", getHigherEducationRejectionMailContent(txtRejectReason.Text.Trim()));
                        }
                        txtRejectReason.Text = String.Empty;
                        pnlRejectReason.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMsgHr);
                }
                finally
                {
                    dhHighEdu = null;
                    dtSecEdu.Dispose();

                    txtRejectReason.Text = String.Empty;
                    pnlRejectReason.Visible = false;
                }

                if (bUpdated)
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_REJECTD, lblMsgHr);

                populateHighEduGrid();
            }
        }

        protected void btnRejectClear_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsgHr);
            txtRejectReason.Text = String.Empty;
        }

        protected void btnRejectCancel_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblMsgHr);
            txtRejectReason.Text = String.Empty;
            pnlRejectReason.Visible = false;
        }
    }
}