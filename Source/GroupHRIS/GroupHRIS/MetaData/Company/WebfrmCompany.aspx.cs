
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataHandler;
using System.Text.RegularExpressions;
using DataHandler.MetaData;
using DataHandler.Employee;
using Common;

namespace GroupHRIS.MetaData
{
    public partial class WebfrmCompany : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["KeyLOGOUT_STS"].Equals("0"))
                {
                    Response.Redirect("MainLogout.aspx", false);
                }
            }
             catch 
            {
                Response.Redirect("~/Login/SessionExpior.aspx", false);
            }
            if (!IsPostBack)
            {
                fillCompany();
                loadHours();
                loadMinutes();
                filStatus();

                //if (chkSaturday.Checked == true)
                //{
                //    saturdayFeilds(true);
                //}
                //else
                //{
                //    saturdayFeilds(false);
                //}
            }
        }

        private void filStatus()
        {
            ListItem listItemBlank = new ListItem();
            listItemBlank.Text = "";
            listItemBlank.Value = "";
            ddlStsCode.Items.Add(listItemBlank);

            ListItem listItemActive = new ListItem();
            listItemActive.Text = Constants.STATUS_ACTIVE_TAG;
            listItemActive.Value = Constants.STATUS_ACTIVE_VALUE;
            ddlStsCode.Items.Add(listItemActive);

            ListItem listItemInActive = new ListItem();
            listItemInActive.Text = Constants.STATUS_INACTIVE_TAG;
            listItemInActive.Value = Constants.STATUS_INACTIVE_VALUE;
            ddlStsCode.Items.Add(listItemInActive);
        }

        private void fillCompany() 
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            
            CompanyDataHandler company = new CompanyDataHandler();
            DataTable compScheme = new DataTable();
            try
            {
                compScheme = company.populate();
                GridView1.DataSource = compScheme;
                GridView1.DataBind();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                company = null;
                compScheme.Dispose();
            }
        }

        private void loadHours()
        {
            string sHH = "";

            ddlSStartHour.Items.Add("");
            ddlSEndHour.Items.Add("");

            for (int i = 0; i <= 23; i++)
            {
                sHH = i.ToString().PadLeft(2, '0');

                ddlStartHour.Items.Add(sHH);
                ddlEndHour.Items.Add(sHH);
                ddlSStartHour.Items.Add(sHH);
                ddlSEndHour.Items.Add(sHH);
            }

            ddlStartHour.SelectedIndex = 8;
            ddlEndHour.SelectedIndex = 17;
            ddlSStartHour.SelectedIndex = 0;
            ddlSEndHour.SelectedIndex = 0;
        }
        
        private void loadMinutes()
        {
            string sHH = "";

            ddlSStartMinute.Items.Add("");
            ddlSEndMinute.Items.Add("");

            for (int i = 0; i <= 59; i++)
            {
                sHH = i.ToString().PadLeft(2, '0');

                ddlStartMinute.Items.Add(sHH);
                ddlEndMinute.Items.Add(sHH);
                ddlSStartMinute.Items.Add(sHH);
                ddlSEndMinute.Items.Add(sHH);
            }
            ddlStartMinute.SelectedIndex = 30;
            ddlEndMinute.SelectedIndex = 30;
            ddlSStartMinute.SelectedIndex = 0;
            ddlSEndMinute.SelectedIndex = 0;
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            fillCompany();
            clear();
            Utility.Errorhandler.ClearError(lblerror);
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
            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
            Utility.Errorhandler.ClearError(lblerror);
            CompanyDataHandler company = new CompanyDataHandler();
            DataRow dataRow = null;
            try
            {
                string mComID = GridView1.SelectedRow.Cells[0].Text;
                dataRow = company.populate(mComID);

                if (dataRow != null)
                {
                    txthremail.Text = dataRow["HR_EMAILS"].ToString().Trim();
                    txtComName.Text = dataRow["COMP_NAME"].ToString().Trim();
                    txtAddress1.Text = dataRow["COMP_ADDRESS_LINE1"].ToString().Trim();
                    txtAddress2.Text = dataRow["COMP_ADDRESS_LINE2"].ToString().Trim();
                    txtAddress3.Text = dataRow["COMP_ADDRESS_LINE3"].ToString().Trim();
                    txtAddress4.Text = dataRow["COMP_ADDRESS_LINE4"].ToString().Trim();
                    txtLandNo1.Text = dataRow["LAND_PHONE1"].ToString().Trim();
                    txtLandNo2.Text = dataRow["LAND_PHONE2"].ToString().Trim();
                    txtEmail.Text = dataRow["EMAIL_ADDRESS"].ToString().Trim();
                    txtVission.Text = dataRow["VISION"].ToString().Trim();
                    txtMission.Text = dataRow["MISSION"].ToString().Trim();
                    ddlStsCode.SelectedValue = dataRow["STATUS_CODE"].ToString().Trim();
                    txtFaxNo.Text = dataRow["FAX_NUMBER"].ToString().Trim();
                    string sStart = dataRow["WORK_HOURS_START"].ToString().Trim();
                    ddlStartHour.SelectedValue = sStart.Substring(0, 2).ToString();
                    ddlStartMinute.SelectedValue = sStart.Substring(3, 2).ToString();

                    string sEnd = dataRow["WORK_HOURS_END"].ToString().Trim();
                    ddlEndHour.SelectedValue = sEnd.Substring(0, 2).ToString();
                    ddlEndMinute.SelectedValue = sEnd.Substring(3, 2).ToString();

                    txtSAPId.Text = dataRow["COMP_SAP_ID"].ToString().Trim();
                    txtEPFNo.Text = dataRow["EMPLOYER_EPF"].ToString().Trim();
                    txtMotto.Text = dataRow["COMPANY_MOTTO"].ToString().Trim();
                    txtBusinessType.Text = dataRow["BUSINESS_TYPE"].ToString().Trim();

                    string SATWORK_HOURS_START = dataRow["SATWORK_HOURS_START"].ToString().Trim();
                    string SATWORK_HOURS_END = dataRow["SATWORK_HOURS_END"].ToString().Trim();

                    if ((SATWORK_HOURS_START != "") || (SATWORK_HOURS_END != ""))
                    {

                        string[] satStart = SATWORK_HOURS_START.Split(':');
                        string[] satEnd = SATWORK_HOURS_END.Split(':');

                        string SAT_START_HOUR;
                        string SAT_START_MIN;

                        string SAT_END_HOUR;
                        string SAT_END_MIN;

                        if (SATWORK_HOURS_START != "")
                        {
                            SAT_START_HOUR = satStart[0].ToString().PadLeft(2, '0');
                            SAT_START_MIN = satStart[1].ToString().PadLeft(2, '0');

                            ddlSStartHour.SelectedIndex = ddlSStartHour.Items.IndexOf(ddlSStartHour.Items.FindByValue(SAT_START_HOUR));
                            ddlSStartMinute.SelectedIndex = ddlSStartMinute.Items.IndexOf(ddlSStartMinute.Items.FindByValue(SAT_START_MIN));
                        }
                        if (SATWORK_HOURS_END != "")
                        {
                            SAT_END_HOUR = satEnd[0].ToString().PadLeft(2, '0');
                            SAT_END_MIN = satEnd[1].ToString().PadLeft(2, '0');

                            ddlSEndHour.SelectedIndex = ddlSEndHour.Items.IndexOf(ddlSEndHour.Items.FindByValue(SAT_END_HOUR));
                            ddlSEndMinute.SelectedIndex = ddlSEndMinute.Items.IndexOf(ddlSEndMinute.Items.FindByValue(SAT_END_MIN));
                        }
                    }
                    else
                    {
                        ddlSStartHour.SelectedIndex = 0;
                        ddlSStartMinute.SelectedIndex = 0;
                        ddlSEndHour.SelectedIndex = 0;
                        ddlSEndMinute.SelectedIndex = 0;
                    }

                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                company = null;
                dataRow = null;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
            Utility.Errorhandler.ClearError(lblerror);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string mComName = txtComName.Text.Trim().ToString();
            string mAdd1 = txtAddress1.Text.Trim().ToString();
            string mAdd2 = txtAddress2.Text.Trim().ToString();
            string mAdd3 = txtAddress3.Text.Trim().ToString();
            string mAdd4 = txtAddress4.Text.Trim().ToString();
            string mLandNo1 = txtLandNo1.Text.Trim().ToString();
            string mLandNo2 = txtLandNo2.Text.Trim().ToString();
            string mEmail = txtEmail.Text.Trim().ToString();
            string mFaxNo = txtFaxNo.Text.Trim().ToString();
            string mWrkHrSt = ddlStartHour.SelectedValue + ":" + ddlStartMinute.SelectedValue;
            string mWrkHrEn = ddlEndHour.SelectedValue + ":" + ddlEndMinute.SelectedValue;
            string mSAPId = txtSAPId.Text.Trim().ToString();
            string mEPFNo = txtEPFNo.Text.Trim().ToString();
            string mVission = txtVission.Text.Trim().ToString();
            string mMission = txtMission.Text.Trim().ToString();
            string mMotto = txtMotto.Text.Trim().ToString();
            string mstsCode = ddlStsCode.SelectedItem.Value.ToString();
            string mBusinessType = txtBusinessType.Text.Trim().ToString();
            string mHremail = txthremail.Text.ToString().Trim();

            CompanyDataHandler company = new CompanyDataHandler();
            EmployeeDataHandler employee = new EmployeeDataHandler();
            DepartmentDataHandler department = new DepartmentDataHandler();
            BranchCenterDataHandler branchCenter = new BranchCenterDataHandler();

            DataTable dtBranchCenter = null; 
            DataTable dtEmployee = null;
            DataTable dtCompany = null;
            DataTable dtDepartment = null;

            if (ddlStartHour.SelectedValue == "00" || ddlEndHour.SelectedValue == "00")
            {
                CommonVariables.MESSAGE_TEXT = "Work Hour Start or Work Hour End cannot be blank.";
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            else
            {
                try
                {
                    if (ddlSStartHour.SelectedIndex > ddlSEndHour.SelectedIndex)
                    {
                        CommonVariables.MESSAGE_TEXT = "Saturday Start Time is Greater Than Saturday End Time.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                        return;
                    }


                    if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                    {
                        dtCompany = company.populateByName(mComName);
                        if (dtCompany.Rows.Count > 0)
                        {
                            CommonVariables.MESSAGE_TEXT = "Record already exists with Company : < " + mComName + " >";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {
                            dtCompany = company.populateBySapID(mSAPId);
                            if (dtCompany.Rows.Count > 0)
                            {
                                CommonVariables.MESSAGE_TEXT = "Record already exists with SAP ID: < " + mSAPId + " >";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            }
                            else
                            {

                                dtCompany = company.populateByEPFNo(mEPFNo);
                                if (dtCompany.Rows.Count > 0)
                                {
                                    CommonVariables.MESSAGE_TEXT = "Record already exists with EPF No: < " + mEPFNo + " >";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);

                                }
                                else
                                {
                                    string mlogUser = Session["KeyUSER_ID"].ToString();

                                    //SATURDAY WORKING HOURS //CODE ADDED BY CHATHURA 2015-12-09
                                    string isSatWork = String.Empty;
                                    string satStrtH = String.Empty;
                                    string satStrM = String.Empty;
                                    string satEndH = String.Empty;
                                    string satEndM = String.Empty;

                                    //if (chkSaturday.Checked == true)
                                    //{
                                    //    isSatWork = "1";
                                    //}
                                    //else
                                    //{
                                    //    isSatWork = "0";
                                    //}

                                    satStrtH = ddlSStartHour.SelectedValue;
                                    satStrM = ddlSStartMinute.SelectedValue;
                                    satEndH = ddlSEndHour.SelectedValue;
                                    satEndM = ddlSEndMinute.SelectedValue;

                                    string satStartTime = "";
                                    string satEndTime = "";

                                    if ((ddlSStartHour.SelectedIndex == 0) && (ddlSStartMinute.SelectedIndex == 0))
                                    {
                                        satStartTime = "";
                                    }
                                    else
                                    {
                                        if ((ddlSStartHour.SelectedIndex == 0) || (ddlSStartMinute.SelectedIndex == 0))
                                        {
                                            CommonVariables.MESSAGE_TEXT = "Incomplete Saturday Work Hour Start";
                                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                                            return;
                                        }
                                        else
                                        {
                                            satStartTime = satStrtH + ":" + satStrM;
                                        }
                                    }

                                    if ((ddlSEndHour.SelectedIndex == 0) && (ddlSEndMinute.SelectedIndex == 0))
                                    {
                                        satEndTime = "";
                                    }
                                    else
                                    {
                                        if ((ddlSEndHour.SelectedIndex == 0) || (ddlSEndMinute.SelectedIndex == 0))
                                        {
                                            CommonVariables.MESSAGE_TEXT = "Incomplete Saturday Work Hour End";
                                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                                            return;
                                        }
                                        else
                                        {
                                            satEndTime = satEndH + ":" + satEndM;
                                        }
                                    }

                                    //
                                    company.Insert(mComName, mAdd1, mAdd2, mAdd3, mAdd4, mLandNo1, mLandNo2, mEmail, mstsCode, mFaxNo, mWrkHrSt, mWrkHrEn, mSAPId, mEPFNo, mVission, mMission, mMotto, mlogUser, mBusinessType, mHremail, satStartTime, satEndTime);
                                    CommonVariables.MESSAGE_TEXT = "Record(s) saved successfully.";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                                    fillCompany();
                                    clear();
                                }
                            }
                        }
                    }

                    else if (btnSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                    {
                        string mCompID = GridView1.SelectedRow.Cells[0].Text;

                        dtCompany = company.populateByNameID(mComName, mCompID);
                        if (dtCompany.Rows.Count > 0)
                        {
                            CommonVariables.MESSAGE_TEXT = "Record already exists with company: < " + mComName + " >";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                        else
                        {
                            dtCompany = company.populateBySapCompID(mSAPId, mCompID);
                            if (dtCompany.Rows.Count > 0)
                            {
                                CommonVariables.MESSAGE_TEXT = "Record already exists with SAP ID: < " + mSAPId + " >";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            }
                            else
                            {
                                dtCompany = company.populateByEPFCompID(mEPFNo, mCompID);
                                if (dtCompany.Rows.Count > 0)
                                {
                                    CommonVariables.MESSAGE_TEXT = "Record already exist with EPF No: < " + mEPFNo + " >";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                                }
                                else
                                {
                                    if (mstsCode == "0") /* check related records */
                                    {
                                        dtEmployee = employee.populateByComID(mCompID);
                                        if (dtEmployee.Rows.Count > 0)
                                        {
                                            CommonVariables.MESSAGE_TEXT = "Cannot Inactivate.Company related < Employee > records are avaialble.";
                                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                                        }
                                        else
                                        {
                                            dtDepartment = department.populateByComIDActive(mCompID);
                                            if (dtDepartment.Rows.Count > 0)
                                            {
                                                CommonVariables.MESSAGE_TEXT = "Cannot Inactivate.Company related < Department > records are avaialble.";
                                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                                            }
                                            else
                                            {
                                                dtBranchCenter = branchCenter.populateByComIDActive(mCompID);
                                                if (dtBranchCenter.Rows.Count > 0)
                                                {
                                                    CommonVariables.MESSAGE_TEXT = "Cannot Inactivate.Company related < Branch/Center > records are avaialble.";
                                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                                                }
                                                else
                                                {
                                                    string mModifyUser = Session["KeyUSER_ID"].ToString();
                                                    //SATURDAY WORKING HOURS //CODE ADDED BY CHATHURA 2015-12-09
                                                    string isSatWork = String.Empty;
                                                    string satStrtH = String.Empty;
                                                    string satStrM = String.Empty;
                                                    string satEndH = String.Empty;
                                                    string satEndM = String.Empty;

                                                    //if (chkSaturday.Checked == true)
                                                    //{
                                                    //    isSatWork = "1";
                                                    //}
                                                    //else
                                                    //{
                                                    //    isSatWork = "0";
                                                    //}

                                                    satStrtH = ddlSStartHour.SelectedValue;
                                                    satStrM = ddlSStartMinute.SelectedValue;
                                                    satEndH = ddlSEndHour.SelectedValue;
                                                    satEndM = ddlSEndMinute.SelectedValue;

                                                    string satStartTime = "";
                                                    string satEndTime = "";

                                                    if ((ddlSStartHour.SelectedIndex == 0) && (ddlSStartMinute.SelectedIndex == 0))
                                                    {
                                                        satStartTime = "";
                                                    }
                                                    else
                                                    {
                                                        if ((ddlSStartHour.SelectedIndex == 0) || (ddlSStartMinute.SelectedIndex == 0))
                                                        {
                                                            CommonVariables.MESSAGE_TEXT = "Incomplete Saturday Work Hour Start";
                                                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            satStartTime = satStrtH + ":" + satStrM;
                                                        }
                                                    }

                                                    if ((ddlSEndHour.SelectedIndex == 0) && (ddlSEndMinute.SelectedIndex == 0))
                                                    {
                                                        satEndTime = "";
                                                    }
                                                    else
                                                    {
                                                        if ((ddlSEndHour.SelectedIndex == 0) || (ddlSEndMinute.SelectedIndex == 0))
                                                        {
                                                            CommonVariables.MESSAGE_TEXT = "Incomplete Saturday Work Hour End";
                                                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            satEndTime = satEndH + ":" + satEndM;
                                                        }
                                                    }



                                                    //
                                                    company.Update(mCompID, mComName, mAdd1, mAdd2, mAdd3, mAdd4, mLandNo1, mLandNo2, mEmail, mstsCode, mFaxNo, mWrkHrSt, mWrkHrEn, mSAPId, mEPFNo, mVission, mMission, mMotto, mModifyUser, mBusinessType, mHremail, satStartTime, satEndTime);
                                                    CommonVariables.MESSAGE_TEXT = "Record(s) updated successfully.";
                                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                                                    fillCompany();
                                                    clear();
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string mModifyUser = Session["KeyUSER_ID"].ToString();
                                        //SATURDAY WORKING HOURS //CODE ADDED BY CHATHURA 2015-12-09
                                        string isSatWork = String.Empty;
                                        string satStrtH = String.Empty;
                                        string satStrM = String.Empty;
                                        string satEndH = String.Empty;
                                        string satEndM = String.Empty;

                                        //if (chkSaturday.Checked == true)
                                        //{
                                        //    isSatWork = "1";
                                        //}
                                        //else
                                        //{
                                        //    isSatWork = "0";
                                        //}

                                        satStrtH = ddlSStartHour.SelectedValue;
                                        satStrM = ddlSStartMinute.SelectedValue;
                                        satEndH = ddlSEndHour.SelectedValue;
                                        satEndM = ddlSEndMinute.SelectedValue;


                                        string satStartTime = "";
                                        string satEndTime = "";

                                        if ((ddlSStartHour.SelectedIndex == 0) && (ddlSStartMinute.SelectedIndex == 0))
                                        {
                                            satStartTime = "";
                                        }
                                        else
                                        {
                                            if ((ddlSStartHour.SelectedIndex == 0) || (ddlSStartMinute.SelectedIndex == 0))
                                            {
                                                CommonVariables.MESSAGE_TEXT = "Incomplete Saturday Work Hour Start";
                                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                                                return;
                                            }
                                            else
                                            {
                                                satStartTime = satStrtH + ":" + satStrM;
                                            }
                                        }

                                        if ((ddlSEndHour.SelectedIndex == 0) && (ddlSEndMinute.SelectedIndex == 0))
                                        {
                                            satEndTime = "";
                                        }
                                        else
                                        {
                                            if ((ddlSEndHour.SelectedIndex == 0) || (ddlSEndMinute.SelectedIndex == 0))
                                            {
                                                CommonVariables.MESSAGE_TEXT = "Incomplete Saturday Work Hour End";
                                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                                                return;
                                            }
                                            else
                                            {
                                                satEndTime = satEndH + ":" + satEndM;
                                            }
                                        }



                                        //
                                        company.Update(mCompID, mComName, mAdd1, mAdd2, mAdd3, mAdd4, mLandNo1, mLandNo2, mEmail, mstsCode, mFaxNo, mWrkHrSt, mWrkHrEn, mSAPId, mEPFNo, mVission, mMission, mMotto, mModifyUser, mBusinessType, mHremail, satStartTime, satEndTime);
                                        CommonVariables.MESSAGE_TEXT = "Record(s) updated successfully.";
                                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                                        fillCompany();
                                        clear();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    CommonVariables.MESSAGE_TEXT = Ex.Message;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                }
                finally
                {
                    company = null;
                    employee = null;
                    department = null;
                    branchCenter = null;
                    dtCompany = null;
                    dtBranchCenter = null;
                    dtEmployee = null;
                    dtDepartment = null;
                }
            }
        }  

        private void clear() 
        {
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
            txtComName.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtAddress4.Text = "";
            txtLandNo1.Text = "";
            txtLandNo2.Text = "";
            txtEmail.Text = "";
            txtVission.Text = "";
            txtMission.Text = "";
            txtFaxNo.Text = "";
            txtBusinessType.Text = "";
            ddlStartHour.SelectedIndex = -1;
            ddlStartMinute.SelectedValue = "00";
            ddlEndHour.SelectedIndex = -1;
            ddlEndMinute.SelectedValue = "00";
            txtSAPId.Text = "";
            txtEPFNo.Text = "";
            txtMotto.Text = "";
            txthremail.Text = "";
            ddlStsCode.SelectedIndex = -1;

            //saturdayFeilds(false);
            //chkSaturday.Checked = false;

            ddlSStartHour.SelectedIndex = 0;
            ddlSEndHour.SelectedIndex = 0;
            ddlSStartMinute.SelectedIndex = 0;
            ddlSEndMinute.SelectedIndex = 0;
        }

        void saturdayFeilds(Boolean Status)
        {
            //lblSaturdayWorkStart.Visible = Status;
            //ddlSStartHour.Visible = Status;
            //ddlSStartSC.Visible = Status;
            //ddlSStartMinute.Visible = Status;
            //lblSStart.Visible = Status;
            //lblSaturdayWorkEnd.Visible = Status;
            //ddlSEndHour.Visible = Status;
            //ddlSEndSC.Visible = Status;
            //ddlSEndMinute.Visible = Status;
            //lblSEnd.Visible = Status;
        }

        //protected void chkSaturday_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkSaturday.Checked == true)
        //    {
        //        saturdayFeilds(true);
        //    }
        //    else
        //    {
        //        saturdayFeilds(false);
        //    }
        //}
    }
}