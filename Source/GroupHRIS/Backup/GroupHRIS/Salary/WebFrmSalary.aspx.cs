using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.Salary;
using System.Data;
using DataHandler.MetaData;
using System.Net;
using NLog;
using System.Text;
using GroupHRIS.Utility;

namespace GroupHRIS.Salary
{
    public partial class WebFrmSalary : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private string sIPAddress = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmSalary : Page_Load");


            if (Session["KeyLOGOUT_STS"].Equals("0"))
            {
                Response.Redirect("MainLogout.aspx", false);
            }

            if (!IsPostBack)
            {
                fillDropDown();
                DataTable oDataTable = new DataTable();
                oDataTable.Columns.Add("COMPONENT_NAME");
                oDataTable.Columns.Add("AMOUNT");
                oDataTable.Columns.Add("STATUS_CODE");
                oDataTable.Columns.Add("ADDED_BY");
                oDataTable.Columns.Add("MODIFIED_BY");
                oDataTable.Columns.Add("EDITED");

                Session["SalaryComponent"] = oDataTable;

                StatusDropDownList.Items.Insert(0, new ListItem("", ""));
                StatusDropDownList.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.STATUS_ACTIVE_VALUE));
                StatusDropDownList.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.STATUS_INACTIVE_VALUE));

                Session["EmpID"] = "";

                List<string> StateList = new List<string>();
                Session["StateList"] = StateList;

            }
            else
            {
                Utility.Errorhandler.ClearError(lblerror);

                if (hfCaller.Value == "EmployeeIDTextBox")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        EmployeeIDTextBox.Text = hfVal.Value;
                    }
                    if (EmployeeIDTextBox.Text != "")
                    {
                        //Postback Methods
                        if (Session["EmpID"].ToString() != EmployeeIDTextBox.Text)
                        {
                            Session["EmpID"] = EmployeeIDTextBox.Text;
                            clear();
                            EmployeeIDTextBox.Text = Session["EmpID"].ToString();

                            SalaryDataHandler SDH = new SalaryDataHandler();
                            EmployeeNameLabel.Text = SDH.PopulateEmployeeName(EmployeeIDTextBox.Text);

                            fillSalary();
                            ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;
                        }
                    }
                }

                //if (Session["EmpID"].ToString() != EmployeeIDTextBox.Text)
                //{
                //    Session["EmpID"] = EmployeeIDTextBox.Text;
                //    clear();
                //    EmployeeIDTextBox.Text = Session["EmpID"].ToString();

                //    SalaryDataHandler SDH = new SalaryDataHandler();
                //    EmployeeNameLabel.Text = SDH.PopulateEmployeeName(EmployeeIDTextBox.Text);

                //    fillSalary();
                //    ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;
                //}
            }
            Utility.Errorhandler.ClearError(lblerror);
        }

        protected void CalculateTotalAmount()
        {
            log.Debug("CalculateTotalAmount()");
            double BudgetaryAmount = 0;
            double BasicAmount = 0;
            double OtherAmount = 0;
            double TotalAmount = 0;

            try
            {
                BasicAmount = Convert.ToDouble(BasicAmountTextBox.Text);
            }
            catch
            {
                BasicAmount = 0;
            }

            try
            {
                OtherAmount = Convert.ToDouble(OtherAmountTextBox.Text);
            }
            catch
            {
                OtherAmount = 0;
            }

            try
            {
                BudgetaryAmount = Convert.ToDouble(BudgetaryReliefAllowanceTextBox.Text);
            }
            catch
            {
                BudgetaryAmount = 0;
            }

            TotalAmount = BasicAmount + OtherAmount + BudgetaryAmount;
        }

        protected void ToggleButton_Click(object sender, EventArgs e)
        {
            log.Debug("ToggleButton_Click()");

            string SalaryID = SalaryIDLabel.Text.ToString();
            string EmployeeID = EmployeeIDTextBox.Text.ToString();
            SalaryDataHandler SDH = new SalaryDataHandler();
            string BasicAmount = BasicAmountTextBox.Text.ToString();
            string BudgetAmount = BudgetaryReliefAllowanceTextBox.Text.ToString();
            string WithEffectFrom = EffectFromTextBox.Text.ToString();
            string OtherAmount = OtherAmountTextBox.Text.ToString();
            string IsOtApplicable = OTDropDownList.SelectedValue.ToString();
            string Remarks = RemarksTextBox.Text.ToString();
            string StatusCode = StatusDropDownList.SelectedValue.ToString();
            string logUser = Session["KeyUSER_ID"].ToString();
            string AddedDate = DateTime.Today.ToString("yyyy/MM/dd");


            DataTable dt = (DataTable)Session["SalaryComponent"];
            Boolean State = SDH.CheckPrevSalary(EmployeeID);



            if ((OTDropDownList.SelectedItem.Text != "") || (StatusDropDownList.SelectedItem.Text != "")/**/)
            {
                Boolean DateValidate = Utils.verifyDate(WithEffectFrom);
                if (DateValidate == true)
                {
                    Errorhandler.ClearError(lblerror);
                    try
                    {
                        if (ToggleButton.Text == Constants.CON_SAVE_BUTTON_TEXT)
                        {
                            if (StatusDropDownList.SelectedItem.Text == Constants.STATUS_INACTIVE_TAG)
                            {
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Cannot Add Inactive Salary", lblerror);
                            }
                            else if (State == false)
                            {
                                SalaryDataHandler salary = new SalaryDataHandler();
                                try
                                {
                                    double chkAmt = Convert.ToDouble(BasicAmount);
                                    salary.Insert(EmployeeID, BasicAmount, BudgetAmount, OtherAmount, IsOtApplicable, WithEffectFrom, Remarks, StatusCode, logUser, AddedDate, dt);
                                    CommonVariables.MESSAGE_TEXT = "Record(s) saved successfully.";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);


                                    EmployeeIDTextBox.Text = EmployeeID;
                                    EmployeeNameLabel.Text = SDH.PopulateEmployeeName(EmployeeID);

                                    SalaryDataHandler oSalaryDataHandler = new SalaryDataHandler();
                                    SearchResultGridView.DataSource = oSalaryDataHandler.Populate(EmployeeIDTextBox.Text);
                                    SearchResultGridView.DataBind();


                                    SalaryIDLabel.Text = "";
                                    BasicAmountTextBox.Text = "";
                                    BudgetaryReliefAllowanceTextBox.Text = "";
                                    EffectFromTextBox.Text = "";
                                    OtherAmountTextBox.Text = "";
                                    TotalAmountTextBox.Text = "";
                                    RemarksTextBox.Text = "";
                                    OTDropDownList.SelectedValue = "";
                                    StatusDropDownList.SelectedValue = "";

                                    DataTable ndt = new DataTable();
                                    ndt.Columns.Add("COMPONENT_NAME");
                                    ndt.Columns.Add("AMOUNT");
                                    ndt.Columns.Add("STATUS_CODE");
                                    ndt.Columns.Add("ADDED_BY");
                                    ndt.Columns.Add("MODIFIED_BY");
                                    ndt.Columns.Add("EDITED");
                                    Session["SalaryComponent"] = ndt;
                                    SalaryComponentsGridView.DataSource = (DataTable)Session["SalaryComponent"];
                                    SalaryComponentsGridView.DataBind();

                                }
                                catch (Exception ex)
                                {
                                    StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                                    log.Debug(oError.ToString());
                                }
                            }
                            else
                            {
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Please Deactivate the Current Active Record", lblerror);
                            }
                        }
                        else if (ToggleButton.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                        {


                            SalaryDataHandler salary = new SalaryDataHandler();
                            try
                            {
                                double chkAmt = Convert.ToDouble(BasicAmount);

                                //List<string> StateList = new List<string>();
                                //StateList = (List<string>)Session["StateList"];

                                salary.Update(EmployeeID, BasicAmount, BudgetAmount, OtherAmount, IsOtApplicable, WithEffectFrom, Remarks, StatusCode, logUser, SalaryID, dt/*, StateList*/);
                                //fillSalary();

                                EmployeeIDTextBox.Text = EmployeeID;
                                EmployeeNameLabel.Text = SDH.PopulateEmployeeName(EmployeeID);


                                SalaryDataHandler oSalaryDataHandler = new SalaryDataHandler();
                                SearchResultGridView.DataSource = oSalaryDataHandler.Populate(EmployeeIDTextBox.Text);
                                SearchResultGridView.DataBind();

                                lblerror.Visible = true;
                                CommonVariables.MESSAGE_TEXT = "Record(s) modified successfully.";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);


                                SalaryIDLabel.Text = "";
                                BasicAmountTextBox.Text = "";
                                BudgetaryReliefAllowanceTextBox.Text = "";
                                EffectFromTextBox.Text = "";
                                OtherAmountTextBox.Text = "";
                                TotalAmountTextBox.Text = "";
                                RemarksTextBox.Text = "";
                                OTDropDownList.SelectedValue = "";
                                StatusDropDownList.SelectedValue = "";

                                DataTable ndt = new DataTable();
                                ndt.Columns.Add("COMPONENT_NAME");
                                ndt.Columns.Add("AMOUNT");
                                ndt.Columns.Add("STATUS_CODE");
                                ndt.Columns.Add("ADDED_BY");
                                ndt.Columns.Add("MODIFIED_BY");
                                ndt.Columns.Add("EDITED");
                                Session["SalaryComponent"] = ndt;
                                SalaryComponentsGridView.DataSource = (DataTable)Session["SalaryComponent"];
                                SalaryComponentsGridView.DataBind();
                            }
                            catch (Exception exp)
                            {
                                StringBuilder oError = Utility.Utils.ExceptionLog(exp);
                                log.Debug(oError.ToString());
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, lblerror);
                                //throw exp;
                            }
                        }
                        //fillSalary();
                        ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;

                    }
                    catch (Exception Ex)
                    {
                        StringBuilder oError = Utility.Utils.ExceptionLog(Ex);
                        log.Debug(oError.ToString());
                        CommonVariables.MESSAGE_TEXT = Ex.Message;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                }
                else if (DateValidate == false)
                {
                    CommonVariables.MESSAGE_TEXT = "Incorrect Date Format";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                }

            }

            ComponentNameDropDownList.SelectedIndex = 0;
            AmountTextBox.Text = "";
            SalaryComponentStatusDropDownList.SelectedIndex = 0;

        }

        protected void SearchResultGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("SearchResultGridView_RowDataBound()");

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.SearchResultGridView, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
        }

        protected void SearchResultGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            log.Debug("SearchResultGridView_SelectedIndexChanging()");

            try
            {
                SalaryIDLabel.Text = WebUtility.HtmlDecode(SearchResultGridView.Rows[e.NewSelectedIndex].Cells[0].Text.ToString().Trim());
                EmployeeIDTextBox.Text = WebUtility.HtmlDecode(SearchResultGridView.Rows[e.NewSelectedIndex].Cells[1].Text.ToString().Trim());


                SalaryDataHandler SDH = new SalaryDataHandler();
                EmployeeNameLabel.Text = SDH.PopulateEmployeeName(EmployeeIDTextBox.Text);

                BasicAmountTextBox.Text = WebUtility.HtmlDecode(SearchResultGridView.Rows[e.NewSelectedIndex].Cells[2].Text.ToString().Trim());
                BudgetaryReliefAllowanceTextBox.Text = WebUtility.HtmlDecode(SearchResultGridView.Rows[e.NewSelectedIndex].Cells[3].Text.ToString().Trim());



                EffectFromTextBox.Text = WebUtility.HtmlDecode(SearchResultGridView.Rows[e.NewSelectedIndex].Cells[6].Text.ToString().Trim());
                OtherAmountTextBox.Text = WebUtility.HtmlDecode(SearchResultGridView.Rows[e.NewSelectedIndex].Cells[4].Text.ToString().Trim());
                if (SearchResultGridView.Rows[e.NewSelectedIndex].Cells[5].Text.ToString().Trim() == "Yes")
                {
                    OTDropDownList.SelectedValue = "1";
                }
                else
                {
                    OTDropDownList.SelectedValue = "0";
                }
                RemarksTextBox.Text = WebUtility.HtmlDecode(SearchResultGridView.Rows[e.NewSelectedIndex].Cells[7].Text.ToString().Trim());

                if (SearchResultGridView.Rows[e.NewSelectedIndex].Cells[8].Text.ToString().Trim() == "Active")
                {
                    StatusDropDownList.SelectedValue = Constants.STATUS_ACTIVE_VALUE;
                    ToggleButton.Enabled = true;
                    EnabledForActive(true);
                }
                else
                {
                    StatusDropDownList.SelectedValue = Constants.STATUS_INACTIVE_VALUE;
                    ToggleButton.Enabled = false;
                    Enabled(false);
                }
                string SalaryID = WebUtility.HtmlDecode(SearchResultGridView.Rows[e.NewSelectedIndex].Cells[0].Text.ToString().Trim());

                SalaryDetailDataHandler oSalaryDetailDataHandler = new SalaryDetailDataHandler();
                Session["SalaryComponent"] = oSalaryDetailDataHandler.Populate(SalaryID);
                SalaryComponentsGridView.DataSource = (DataTable)Session["SalaryComponent"];
                SalaryComponentsGridView.DataBind();
                ToggleButton.Text = Constants.CON_UPDATE_BUTTON_TEXT;


                ComponentNameDropDownList.SelectedValue = "";
                AmountTextBox.Text = "";
                SalaryComponentStatusDropDownList.SelectedValue = "";


            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
            CalculateTotalSalary();
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            log.Debug("CancelButton_Click()");

            ToggleButton.Text = Constants.CON_SAVE_BUTTON_TEXT;
            clear();
            Enabled(true);
            SearchResultGridView.DataSource = null;
            SearchResultGridView.DataBind();
            EmployeeIDTextBox.Text = "";
            EmployeeNameLabel.Text = "";
            BudgetaryReliefAllowanceTextBox.Text = "";
            EffectFromTextBox.Text = "";
            Session["EmpID"] = "";
        }

        protected void AddSalaryComponentButton_Click(object sender, EventArgs e)
        {
            log.Debug("AddSalaryComponentButton_Click()");

            double AMT = Convert.ToDouble(AmountTextBox.Text);
            AmountTextBox.Text = String.Format("{0:00.00}", AMT);

            if (AddSalaryComponentButton.Text == "Update")
            {
                string[] SelectArr = new string[2];
                SelectArr = (string[])Session["CompSelect"];

                if ((SelectArr[0] == ComponentNameDropDownList.SelectedIndex.ToString()) && (SelectArr[1] == AmountTextBox.Text))
                {
                    AddSalaryComponentButton.Text = "Add";
                    int SelectedIndex = (int)Session["SelectedIndex"];


                    DataTable dataTable = new DataTable();
                    dataTable = (DataTable)Session["SalaryComponent"];

                    string OldStatus = dataTable.Rows[SelectedIndex]["STATUS_CODE"].ToString();
                    string ModifiedUser = Session["KeyUSER_ID"].ToString();

                    dataTable.Rows[SelectedIndex]["STATUS_CODE"] = SalaryComponentStatusDropDownList.SelectedItem.Text;
                    dataTable.Rows[SelectedIndex]["MODIFIED_BY"] = ModifiedUser;
                    dataTable.Rows[SelectedIndex]["EDITED"] = "Update";

                    ComponentNameDropDownList.SelectedValue = "";
                    SalaryComponentStatusDropDownList.SelectedValue = "";
                    AmountTextBox.Text = "";
                    ComponentNameDropDownList.Enabled = true;
                    AmountTextBox.Enabled = true;

                    SalaryComponentsGridView.DataSource = dataTable;
                    SalaryComponentsGridView.DataBind();

                    Session["SalaryComponent"] = dataTable;
                    //Updated
                    CalculateOtherPayments();
                    CalculateTotalSalary();
                }
                else
                {
                    if ((SelectArr[0] != ComponentNameDropDownList.SelectedIndex.ToString()) && (SelectArr[1] == AmountTextBox.Text))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Cannot Change Component Name", lblerror);
                    }
                    else if ((SelectArr[1] != AmountTextBox.Text) && (SelectArr[0] == ComponentNameDropDownList.SelectedIndex.ToString()))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Cannot Change Amount", lblerror);
                    }
                    else if ((SelectArr[0] != ComponentNameDropDownList.SelectedIndex.ToString()) && (SelectArr[1] != AmountTextBox.Text))
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Cannot Change Component Name and Amount", lblerror);
                    }
                }
            }
            else
            {
                DataTable oDataTable = new DataTable();
                oDataTable = (DataTable)Session["SalaryComponent"];

                DataRow[] result = oDataTable.Select("COMPONENT_NAME = '" + ComponentNameDropDownList.SelectedItem.Text + "'");
                if (result.Length == 0)
                {
                    DataRow oDataRow = oDataTable.NewRow();
                    oDataRow["COMPONENT_NAME"] = ComponentNameDropDownList.SelectedItem.Text;
                    oDataRow["AMOUNT"] = AmountTextBox.Text;
                    oDataRow["STATUS_CODE"] = SalaryComponentStatusDropDownList.SelectedItem.Text;

                    oDataRow["ADDED_BY"] = Session["KeyUSER_ID"].ToString();
                    oDataRow["MODIFIED_BY"] = Session["KeyUSER_ID"].ToString();
                    oDataRow["EDITED"] = "New";



                    try
                    {
                        double chkAmt = Convert.ToDouble(AmountTextBox.Text);
                        oDataTable.Rows.Add(oDataRow);
                    }
                    catch
                    {

                    }

                    SalaryComponentsGridView.DataSource = oDataTable;
                    SalaryComponentsGridView.DataBind();

                    Session["SalaryComponent"] = oDataTable;
                    CalculateOtherPayments();
                    CalculateTotalSalary();
                    lblerror = null;
                }
                else
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Duplicate Salary Component <" + ComponentNameDropDownList.SelectedItem.Text + ">", lblerror);
                }
            }
        }

        void CalculateOtherPayments()
        {
            log.Debug("CalculateOtherPayments()");

            try
            {
                DataTable oDataTable = new DataTable();
                oDataTable = (DataTable)Session["SalaryComponent"];

                int Count = oDataTable.Rows.Count;
                double TotalOtherAmount = 0;
                for (int i = 0; i < Count; i++)
                {
                    if (oDataTable.Rows[i]["STATUS_CODE"].ToString() == Constants.STATUS_ACTIVE_TAG)
                    {
                        TotalOtherAmount += Convert.ToDouble(oDataTable.Rows[i]["AMOUNT"].ToString());
                    }
                }
                OtherAmountTextBox.Text = TotalOtherAmount.ToString("0.00");
                String.Format("F2", OtherAmountTextBox.Text);
            }
            finally
            {
                ComponentNameDropDownList.SelectedValue = "";
                SalaryComponentStatusDropDownList.SelectedValue = "";
                AmountTextBox.Text = "";
                TotalAmountTextBox.Text = "";
            }
        }

        void clear()
        {
            log.Debug("clear()");

            SalaryIDLabel.Text = "";
            BasicAmountTextBox.Text = "";
            BudgetaryReliefAllowanceTextBox.Text = "";
            EffectFromTextBox.Text = "";
            OtherAmountTextBox.Text = "";
            OTDropDownList.SelectedValue = "";
            StatusDropDownList.SelectedValue = "";
            RemarksTextBox.Text = "";
            TotalAmountTextBox.Text = "";
            lblerror = null;


            DataTable dt = new DataTable();
            dt.Columns.Add("COMPONENT_NAME");
            dt.Columns.Add("AMOUNT");
            dt.Columns.Add("STATUS_CODE");
            dt.Columns.Add("ADDED_BY");
            dt.Columns.Add("MODIFIED_BY");
            dt.Columns.Add("EDITED");
            Session["SalaryComponent"] = dt;
            SalaryComponentsGridView.DataSource = (DataTable)Session["SalaryComponent"];
            SalaryComponentsGridView.DataBind();

            ComponentNameDropDownList.SelectedValue = "";
            SalaryComponentStatusDropDownList.SelectedValue = "";
            AmountTextBox.Text = "";
            ComponentNameDropDownList.Enabled = true;
            AmountTextBox.Enabled = true;
            AddSalaryComponentButton.Text = "Add";
            lblerror = new Label();
            Utility.Errorhandler.ClearError(lblerror);
        }

        void fillSalary()
        {
            log.Debug("fillSalary()");

            string EmployeeID = EmployeeIDTextBox.Text;

            Utility.Errorhandler.ClearError(lblerror);
            Enabled(true);
            SearchResultGridView.DataSource = null;
            SearchResultGridView.DataBind();


            EmployeeIDTextBox.Text = EmployeeID;

            SalaryDataHandler SDH = new SalaryDataHandler();
            EmployeeNameLabel.Text = SDH.PopulateEmployeeName(EmployeeIDTextBox.Text);

            SalaryDataHandler oSalaryDataHandler = new SalaryDataHandler();
            SearchResultGridView.DataSource = oSalaryDataHandler.Populate(EmployeeIDTextBox.Text);
            SearchResultGridView.DataBind();
        }

        void fillDropDown()
        {
            log.Debug("fillDropDown()");

            SalaryComponentDataHandler oSalaryComponentDataHandler = new SalaryComponentDataHandler();
            DataTable oDataTable = new DataTable();
            oDataTable = oSalaryComponentDataHandler.populateActive();

            for (int i = 0; i < oDataTable.Rows.Count; i++)
            {
                if (oDataTable.Rows[i][1].ToString() == "Basic Salary")
                {
                    oDataTable.Rows.RemoveAt(i);
                }
            }

            ComponentNameDropDownList.Items.Insert(0, new ListItem("", ""));
            for (int i = 1; i < oDataTable.Rows.Count; i++)
            {
                ComponentNameDropDownList.Items.Insert(i, new ListItem(oDataTable.Rows[i - 1][1].ToString(), oDataTable.Rows[i - 1][0].ToString()));
            }

            SalaryComponentStatusDropDownList.Items.Insert(0, new ListItem("", ""));
            SalaryComponentStatusDropDownList.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.STATUS_ACTIVE_VALUE));
            SalaryComponentStatusDropDownList.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.STATUS_INACTIVE_VALUE));

            OTDropDownList.Items.Insert(0, new ListItem("", ""));
            OTDropDownList.Items.Insert(1, new ListItem(Constants.OT_ACTIVE_TAG, Constants.OT_ACTIVE_VALUE));
            OTDropDownList.Items.Insert(2, new ListItem(Constants.OT_INACTIVE_TAG, Constants.OT_INACTIVE_VALUE));
        }

        protected void SalaryComponentsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("SalaryComponentsGridView_RowDataBound()");

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.SalaryComponentsGridView, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                StringBuilder oError = Utility.Utils.ExceptionLog(ex);
                log.Debug(oError.ToString());
                throw ex;
            }
        }

        protected void SalaryComponentsGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            log.Debug("SalaryComponentsGridView_SelectedIndexChanging()");

            Session["SelectedIndex"] = e.NewSelectedIndex;

            AddSalaryComponentButton.Text = "Update";

            ComponentNameDropDownList.SelectedIndex = ComponentNameDropDownList.Items.IndexOf(ComponentNameDropDownList.Items.FindByText(HttpUtility.HtmlDecode(SalaryComponentsGridView.Rows[e.NewSelectedIndex].Cells[0].Text)));

            AmountTextBox.Text = SalaryComponentsGridView.Rows[e.NewSelectedIndex].Cells[1].Text;

            SalaryComponentStatusDropDownList.SelectedIndex = SalaryComponentStatusDropDownList.Items.IndexOf(SalaryComponentStatusDropDownList.Items.FindByText(SalaryComponentsGridView.Rows[e.NewSelectedIndex].Cells[2].Text));

            string[] SelectArr = new string[2];
            SelectArr[0] = ComponentNameDropDownList.SelectedIndex.ToString();
            SelectArr[1] = AmountTextBox.Text;
            Session["CompSelect"] = SelectArr;
        }

        protected void HistoryImageButton_Click(object sender, ImageClickEventArgs e)
        {
            log.Debug("HistoryImageButton_Click()");

            fillSalary();
        }

        private void CalculateTotalSalary()
        {
            log.Debug("CalculateTotalSalary()");

            double BasicAmount = 0; ;
            double OtherAmount = 0;
            double BudgetaryAmount = 0;

            try
            { BasicAmount = Convert.ToDouble(BasicAmountTextBox.Text); }
            catch { BasicAmount = 0; }

            try { OtherAmount = Convert.ToDouble(OtherAmountTextBox.Text); }
            catch { OtherAmount = 0; }

            try { BudgetaryAmount = Convert.ToDouble(BudgetaryReliefAllowanceTextBox.Text); }
            catch { BudgetaryAmount = 0; }

            double TotalAmount = BasicAmount + OtherAmount + BudgetaryAmount;
            TotalAmountTextBox.Text = TotalAmount.ToString("0.00");
        }

        void Enabled(Boolean Status)
        {
            log.Debug("Enabled()");

            ToggleButton.Enabled = Status;
            OtherAmountTextBox.ReadOnly = !Status;
            TotalAmountTextBox.ReadOnly = !Status;
            AmountTextBox.ReadOnly = !Status;
        }

        void EnabledForActive(Boolean Status)
        {
            log.Debug("EnabledForActive()");

            AmountTextBox.ReadOnly = !Status;
            ToggleButton.Enabled = Status;
            OtherAmountTextBox.ReadOnly = Status;
            TotalAmountTextBox.ReadOnly = Status;
        }

        protected void SalaryComponentButton_Click(object sender, EventArgs e)
        {
            log.Debug("SalaryComponentButton_Click()");

            ComponentNameDropDownList.SelectedValue = "";
            AmountTextBox.Text = "";
            SalaryComponentStatusDropDownList.SelectedValue = "";
            AddSalaryComponentButton.Text = "Add";
        }

        protected void SearchResultGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("SearchResultGridView_PageIndexChanging()");

            SalaryDataHandler oSalaryDataHandler = new SalaryDataHandler();
            SearchResultGridView.PageIndex = e.NewPageIndex;
            SearchResultGridView.DataSource = oSalaryDataHandler.Populate(EmployeeIDTextBox.Text);
            SearchResultGridView.DataBind();
        }

    }
}