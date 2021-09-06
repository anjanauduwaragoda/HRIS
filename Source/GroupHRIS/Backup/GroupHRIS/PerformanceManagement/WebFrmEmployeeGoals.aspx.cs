using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.PerformanceManagement;
using Common;
using System.Data;
using DataHandler.Utility;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmEmployeeGoals : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        private string sUserId = "";

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                sIPAddress = Request.UserHostAddress;
                log.Debug("IP:" + sIPAddress + "webFrmEmployee : Page_Load");

                txtEmployee.Text = (Session["KeyEMPLOYEE_ID"] as string);
                Session["SelectedEmployeeID"] = txtEmployee.Text.Trim();
                loadFinancialYearDropDown();
                EmployeeGoalDataHandler oEGDH = new EmployeeGoalDataHandler();
                lblEmployeeName.Text = oEGDH.getEmployeeName(txtEmployee.Text.Trim());
                Session["lblEmployeeName"] = lblEmployeeName.Text;
                loadGrid();
                previousGoalCheck();
                setBalancedAndTotalWeights();
            }



            if (isCommonUser())
            {
                EmpSearch.Visible = false;
            }
            else
            {
                EmpSearch.Visible = true;
            }


            if (IsPostBack)
            {
                if (hfCaller.Value == "txtEmployee")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtEmployee.Text = hfVal.Value;
                    }
                    if (txtEmployee.Text != "")
                    {
                        Utility.Errorhandler.ClearError(lblerror);
                        //Postback Methods
                        try
                        {
                            if (hfVerify.Value != "")
                            {
                                clearFields();
                                Session["SelectedEmployeeID"] = txtEmployee.Text.Trim();
                                loadFinancialYearDropDown();
                                EmployeeGoalDataHandler oEGDH = new EmployeeGoalDataHandler();
                                lblEmployeeName.Text = oEGDH.getEmployeeName(txtEmployee.Text.Trim());
                                Session["lblEmployeeName"] = lblEmployeeName.Text;
                                loadGrid();
                                previousGoalCheck();
                                setBalancedAndTotalWeights();


                                //clearFields();
                                loadGrid();
                            }
                        }
                        catch (Exception ex)
                        {
                            CommonVariables.MESSAGE_TEXT = ex.Message;
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                    }
                }

                if (hfCaller.Value == "hfupfdategoal")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        hfupfdategoal.Value = hfVal.Value;
                    }
                    if (hfupfdategoal.Value != "")
                    {
                        Utility.Errorhandler.ClearError(lblerror);
                        //Postback Methods
                        try
                        {
                            loadGrid();
                            setBalancedAndTotalWeights();
                        }
                        catch (Exception ex)
                        {
                            CommonVariables.MESSAGE_TEXT = ex.Message;
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                        }
                    }
                }
            }
            else
            {
                Session["FinYear"] = String.Empty;
                lblYearofGoal.Text = loadFinancialYear();
                loadGoalGruops();
                loadStatus();
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnSave_Click()");

            EmployeeGoalDataHandler oEGDH = new EmployeeGoalDataHandler();
            UtilsDataHandler utilsDataHandler = new UtilsDataHandler();

            Utility.Errorhandler.ClearError(lblerror);

            System.DateTime dtime = System.DateTime.Now;

            string EmployeeID = txtEmployee.Text.Trim();
            string yearOfGoal = (Session["FinYear"] as string).Trim();
            string GoalGroup = ddlGoalGroup.SelectedValue.Trim();
            string GoalArea = txtGoalArea.Text.Trim();
            string Description = txtDescription.Text.Trim();
            string Measurement = txtMeasurements.Text.Trim().Replace("\n", "<br />"); 
            string Weight = txtWeights.Text.Trim();

            double TempWeight = 0.0;
            if (Double.TryParse(Weight, out TempWeight))
            {
                if (TempWeight <= 0.0)
                {
                    if (ddlStatus.SelectedValue == Constants.CON_ACTIVE_STATUS)
                    {
                        CommonVariables.MESSAGE_TEXT = @"Weight should be greater than '0'.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        return;
                    }
                }
            }
            else
            {
                CommonVariables.MESSAGE_TEXT = @"Invalid weight.";
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                return;
            }

            string Status = ddlStatus.SelectedValue.Trim();
            string addedBy = (Session["KeyEMPLOYEE_ID"] as string).Trim();


            try
            {
                if (Utility.Utils.isSpecialCharacterExists(txtGoalArea.Text.Trim()))
                {

                    CommonVariables.MESSAGE_TEXT = @"Invalid goal area.";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                    return;
                }

                if (btnSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {

                    if (oEGDH.CheckGoalAreaExsistance(txtEmployee.Text.Trim(), ddlYear.SelectedValue.Trim(), txtGoalArea.Text.Trim()))
                    {
                        CommonVariables.MESSAGE_TEXT = "Goal area already exists.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        return;
                    }

                    // TOTAL WEIGHT VALIDATION
                    if (ddlStatus.SelectedValue == Constants.STATUS_ACTIVE_VALUE)
                    {
                        double Wght = Convert.ToDouble(Weight);


                        double currentyExistWeight = oEGDH.getCurrentTotalWeight(EmployeeID, yearOfGoal);

                        double newTotalWeight = currentyExistWeight + Wght;

                        if (newTotalWeight > 100)
                        {
                            CommonVariables.MESSAGE_TEXT = "Total weight should be less than or equal to 100 %";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                    }
                    // --

                    Boolean isInserted = oEGDH.Insert(EmployeeID, yearOfGoal, GoalGroup, GoalArea, Description, Measurement, Weight, Status, addedBy);
                    if (isInserted)
                    {
                        clearFields();
                        loadGrid();
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                    }
                }
                else
                {
                    int SelectedIndex = grdvEmployeeGoals.SelectedIndex;
                    string GoalID = grdvEmployeeGoals.Rows[SelectedIndex].Cells[8].Text;


                    if (oEGDH.isApprovedGoal(GoalID))
                    {
                        CommonVariables.MESSAGE_TEXT = @"Finalized employee goals cannot be changed.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        return;
                    }

                    if (oEGDH.CheckGoalAreaExsistance(txtEmployee.Text.Trim(), ddlYear.SelectedValue.Trim(), txtGoalArea.Text.Trim(),GoalID))
                    {
                        CommonVariables.MESSAGE_TEXT = "Goal area already exists.";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        return;
                    }

                    // TOTAL WEIGHT VALIDATION

                    if (ddlStatus.SelectedValue == Constants.STATUS_ACTIVE_VALUE)
                    {
                        double Wght = Convert.ToDouble(Weight);


                        double currentyExistWeight = oEGDH.getCurrentTotalWeight(EmployeeID, yearOfGoal);
                        double updatedWeight = Wght;
                        double beforeweight = Convert.ToDouble(grdvEmployeeGoals.Rows[grdvEmployeeGoals.SelectedIndex].Cells[6].Text);

                        double changedAmount = updatedWeight - beforeweight;
                        double newTotalWeight = currentyExistWeight + changedAmount;

                        if (grdvEmployeeGoals.Rows[SelectedIndex].Cells[7].Text == Constants.STATUS_INACTIVE_VALUE)
                        {
                            newTotalWeight = currentyExistWeight + Wght;
                        }


                        if (newTotalWeight > 100)
                        {
                            CommonVariables.MESSAGE_TEXT = "Total weight should be less than or equal to 100 %";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                    }
                    // --

                    Boolean isUpdated = oEGDH.Update(EmployeeID, yearOfGoal, GoalGroup, GoalArea, Description, Measurement, Weight, Status, addedBy, GoalID);
                    if (isUpdated)
                    {
                        clearFields();
                        loadGrid();
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
                        btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                    }
                }
                setBalancedAndTotalWeights();

                ddlGoalGroup.SelectedIndex = 0;
                ddlStatus.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                utilsDataHandler = null;
            }
                
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");

            clearFields();
            ddlGoalGroup.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            btnSave.Text = Constants.CON_SAVE_BUTTON_TEXT;

            if (!isCommonUser())
            {
                Utility.Utils.clearControls(false, txtEmployee, lblEmployeeName);
                ddlYear.Items.Clear();
                grdvEmployeeGoals.DataSource = null;
                grdvEmployeeGoals.DataBind();
                Session["FinYear"] = String.Empty;
                lblYearofGoal.Text = loadFinancialYear();
                //loadGoalGruops();
                //loadStatus();
                previousGoalCheck();
                setBalancedAndTotalWeights();
            }
        }

        protected void grdvEmployeeGoals_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdvEmployeeGoals, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void grdvEmployeeGoals_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("grdvEmployeeGoals_SelectedIndexChanged()");

            Utility.Errorhandler.ClearError(lblerror);
            int SelectedIndex = grdvEmployeeGoals.SelectedIndex;             

            string GoalGroup = HttpUtility.HtmlDecode(grdvEmployeeGoals.Rows[SelectedIndex].Cells[2].Text);
            ddlGoalGroup.SelectedIndex = ddlGoalGroup.Items.IndexOf(ddlGoalGroup.Items.FindByValue(GoalGroup));

            //string EmployeeID = dtEGoals.Rows[0]["EMPLOYEE_ID"].ToString();
            //string yearOfGoal = dtEGoals.Rows[0]["YEAR_OF_GOAL"].ToString();

            txtGoalArea.Text = HttpUtility.HtmlDecode(grdvEmployeeGoals.Rows[SelectedIndex].Cells[10].Text);
            txtDescription.Text = HttpUtility.HtmlDecode(grdvEmployeeGoals.Rows[SelectedIndex].Cells[4].Text);
            txtMeasurements.Text = HttpUtility.HtmlDecode(grdvEmployeeGoals.Rows[SelectedIndex].Cells[5].Text.Replace("&lt;br /&gt;", Environment.NewLine));
            txtWeights.Text = HttpUtility.HtmlDecode(grdvEmployeeGoals.Rows[SelectedIndex].Cells[6].Text);
            string Status = HttpUtility.HtmlDecode(grdvEmployeeGoals.Rows[SelectedIndex].Cells[7].Text);
            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(Status));

            btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
        }

        protected void grdvEmployeeGoals_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("grdvEmployeeGoals_PageIndexChanging()");
            //clearFields();

            //EmployeeGoalDataHandler oEGDH = new EmployeeGoalDataHandler();
            //DataTable dtEGoals = new DataTable();
            //string SelectedYear = ddlYear.SelectedValue;
            //string CurrentYear = loadFinancialYearWithoutDetails();


            //if (SelectedYear != CurrentYear)
            //{
            //    dtEGoals = oEGDH.Populate(txtEmployee.Text.Trim(), ddlYear.SelectedValue.Trim());
            //}
            //else
            //{

            //    dtEGoals = oEGDH.Populate(txtEmployee.Text.Trim(), (Session["FinYear"] as string).Trim());
            //}

            //grdvEmployeeGoals.PageIndex = e.NewPageIndex;
            //grdvEmployeeGoals.DataSource = dtEGoals.Copy();
            //grdvEmployeeGoals.DataBind();


            string SelectedYear = ddlYear.SelectedValue;
            string CurrentYear = loadFinancialYearWithoutDetails();


            grdvEmployeeGoals.DataSource = ReturnGrid(ddlYear.SelectedValue).Copy();
            grdvEmployeeGoals.PageIndex = e.NewPageIndex;
            grdvEmployeeGoals.DataBind();

            if (SelectedYear != CurrentYear)
            {
                lblYearofGoal.Text = loadPreviousFinancialYear(ddlYear.SelectedValue);
                btnSave.Enabled = false;
            }
            else
            {
                lblYearofGoal.Text = loadFinancialYear();
                btnSave.Enabled = true;
            //    loadGrid();
            }

        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlYear_SelectedIndexChanged()");

            Utility.Errorhandler.ClearError(lblerror);

            string SelectedYear = ddlYear.SelectedValue;
            string CurrentYear = loadFinancialYearWithoutDetails();


            if (SelectedYear != CurrentYear)
            {
                lblYearofGoal.Text = loadPreviousFinancialYear(ddlYear.SelectedValue);
                btnSave.Enabled = false;
                loadGrid(ddlYear.SelectedValue);
            }
            else
            {
                lblYearofGoal.Text = loadFinancialYear();
                btnSave.Enabled = true;
                loadGrid();
            }
            setBalancedAndTotalWeights();
        }

        #endregion

        #region Methods

        string loadFinancialYear()
        {
            String FinancialYear = String.Empty;
            try
            {
                log.Debug("loadFinancialYear()");

                ////finYearStrtDate
                System.DateTime dtfin = System.DateTime.Now;

                int CurrentFinyear = 0;
                string CurrentFinYearDetails = String.Empty;

                DateTime finDate = DateTime.ParseExact(dtfin.Year + "-04-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                if (finDate > System.DateTime.Now)
                {
                    CurrentFinyear = dtfin.AddYears(-1).Year;
                    CurrentFinYearDetails = " (From April 1, " + CurrentFinyear.ToString() + " To March 31, " + finDate.Year + ")";
                    Session["FinYear"] = CurrentFinyear.ToString();

                    FinancialYear = CurrentFinyear.ToString() + CurrentFinYearDetails;
                }
                else
                {
                    System.DateTime dt = System.DateTime.Now;
                    System.DateTime dtDetais = System.DateTime.Now;
                    string finYearDetails = " (From April 1, " + dt.Year.ToString() + " To March 31, " + dtDetais.AddYears(1).Year + ")";
                    Session["FinYear"] = dt.Year.ToString();

                    FinancialYear = dt.Year.ToString() + Environment.NewLine + finYearDetails;
                }
                return FinancialYear;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);

                return FinancialYear;
            }
            finally
            {
                FinancialYear = String.Empty;
            }
        }

        string loadPreviousFinancialYear(string Year)
        {
            String FinancialYear = String.Empty;
            try
            {
                log.Debug("loadPreviousFinancialYear()");

                ////finYearStrtDate

                DateTime SelectedYearDT;
                if (!DateTime.TryParse("01/01/" + Year, out SelectedYearDT))
                {
                    // handle parse failure
                }
                else
                {
                    FinancialYear = SelectedYearDT.Year + " (From April 1, " + SelectedYearDT.Year.ToString() + " To March 31, " + SelectedYearDT.AddYears(1).Year + ")";
                }

                return FinancialYear;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);

                return FinancialYear;
            }
            finally
            {
                FinancialYear = String.Empty;
            }
        }

        string loadFinancialYearWithoutDetails()
        {
            String FinancialYear = String.Empty;
            try
            {
                log.Debug("loadFinancialYearWithoutDetails()");

                ////finYearStrtDate
                System.DateTime dtfin = System.DateTime.Now;

                int CurrentFinyear = 0;
                //string CurrentFinYearDetails = String.Empty;

                DateTime finDate = DateTime.ParseExact(dtfin.Year + "-04-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                if (finDate > System.DateTime.Now)
                {
                    CurrentFinyear = dtfin.AddYears(-1).Year;
                    //CurrentFinYearDetails = " (From April 1, " + CurrentFinyear.ToString() + " To March 31, " + finDate.Year + ")";
                    Session["FinYear"] = CurrentFinyear.ToString();

                    FinancialYear = CurrentFinyear.ToString();// +CurrentFinYearDetails;
                }
                else
                {
                    System.DateTime dt = System.DateTime.Now;
                    System.DateTime dtDetais = System.DateTime.Now;
                    //string finYearDetails = " (From April 1, " + dt.Year.ToString() + " To March 31, " + dtDetais.AddYears(1).Year + ")";
                    Session["FinYear"] = dt.Year.ToString();

                    FinancialYear = dt.Year.ToString();// +Environment.NewLine + finYearDetails;
                }
                return FinancialYear;
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);

                return FinancialYear;
            }
            finally
            {
                FinancialYear = String.Empty;
            }
        }

        void loadFinancialYearDropDown()
        {
            EmployeeGoalDataHandler EGDH = new EmployeeGoalDataHandler();
            DataTable dtFinancialYears = new DataTable();
            try
            {
                log.Debug("loadFinancialYearDropDown()");

                string CurrFinyear = loadFinancialYearWithoutDetails();

                dtFinancialYears = EGDH.PopulateFinancialYears(txtEmployee.Text.Trim()).Copy();
                ddlYear.Items.Clear();
                if (dtFinancialYears.Rows.Count > 0)
                {
                    DataRow[] drArr = dtFinancialYears.Select("YEAR_OF_GOAL = '" + CurrFinyear + "'");
                    if (drArr.Length == 0)
                    {
                        ddlYear.Items.Add(new ListItem(CurrFinyear, CurrFinyear));
                    }

                    for (int i = 0; i < dtFinancialYears.Rows.Count; i++)
                    {
                        string year = dtFinancialYears.Rows[i]["YEAR_OF_GOAL"].ToString();
                        ddlYear.Items.Add(new ListItem(year, year));
                    }
                }
                else
                {
                    ddlYear.Items.Add(new ListItem(CurrFinyear, CurrFinyear));
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                EGDH = null;
                dtFinancialYears.Dispose();
            }
        }

        void loadGoalGruops()
        {
            log.Debug("loadGoalGruops()");

            EmployeeGoalDataHandler oEGDH = new EmployeeGoalDataHandler();
            DataTable dt = new DataTable();
            dt = oEGDH.PopulateGoalGroups();

            ddlGoalGroup.Items.Add(new ListItem("", ""));
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string value=dt.Rows[i][0].ToString();
                string text=dt.Rows[i][1].ToString();
                ddlGoalGroup.Items.Add(new ListItem(text, value));
            }
        }

        void loadStatus()
        {
            log.Debug("loadStatus()");

            ddlStatus.Items.Add(new ListItem("", ""));
            ddlStatus.Items.Add(new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.STATUS_ACTIVE_VALUE));
            ddlStatus.Items.Add(new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.STATUS_INACTIVE_VALUE));
        }

        void loadGrid()
        {
            log.Debug("loadGrid()");

            EmployeeGoalDataHandler oEGDH = new EmployeeGoalDataHandler();
            DataTable dtEGoals = new DataTable();

            dtEGoals = oEGDH.Populate(txtEmployee.Text.Trim(), (Session["FinYear"] as string).Trim());

            grdvEmployeeGoals.DataSource = dtEGoals.Copy();
            grdvEmployeeGoals.DataBind();
        }

        void loadGrid(string Year)
        {
            log.Debug("loadGrid()");

            EmployeeGoalDataHandler oEGDH = new EmployeeGoalDataHandler();
            DataTable dtEGoals = new DataTable();

            dtEGoals = oEGDH.Populate(txtEmployee.Text.Trim(), Year.Trim());

            grdvEmployeeGoals.DataSource = dtEGoals.Copy();
            grdvEmployeeGoals.DataBind();
        }

        DataTable ReturnGrid(string Year)
        {
            log.Debug("ReturnGrid()");

            EmployeeGoalDataHandler oEGDH = new EmployeeGoalDataHandler();
            DataTable dtEGoals = new DataTable();
            try
            {

            dtEGoals = oEGDH.Populate(txtEmployee.Text.Trim(), Year.Trim());

            return dtEGoals;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void clearFields()
        {
            log.Debug("clearFields()");
            //lblEmployeeName.Text = String.Empty;
            //lblYearofGoal.Text = String.Empty;
            txtGoalArea.Text = String.Empty;
            txtDescription.Text = String.Empty;
            txtMeasurements.Text = String.Empty;
            txtWeights.Text = String.Empty;
            Utility.Errorhandler.ClearError(lblerror);

            lblYearofGoal.Text = loadFinancialYear();
            btnSave.Enabled = true;

            string CurrentYear = loadFinancialYearWithoutDetails();
            ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByValue(CurrentYear));

            if (txtEmployee.Text != "")
            {
                loadGrid();
                previousGoalCheck();
            }
        }

        void previousGoalCheck()
        {
            log.Debug("clearFields()");

            EmployeeGoalDataHandler oEGDH = new EmployeeGoalDataHandler();
            if (oEGDH.isPreviousGoalAvailable(txtEmployee.Text.Trim(), (Session["FinYear"] as string)))
            {
                HistoryImageButton.Visible = true;
            }
            else
            {
                HistoryImageButton.Visible = false;
            }
        }

        void setBalancedAndTotalWeights()
        {
            log.Debug("setBalancedAndTotalWeights()");

            EmployeeGoalDataHandler oEGDH = new EmployeeGoalDataHandler();
            DataTable dtEGoals = new DataTable();
            try
            {
                //Utility.Utils.clearControls(true, ddlGoalGroup, txtGoalArea, txtDescription, txtMeasurements, txtWeights, ddlStatus);

                string SelectedYear = ddlYear.SelectedValue;
                string CurrentYear = loadFinancialYearWithoutDetails();

                if (SelectedYear == CurrentYear)
                {
                    dtEGoals = oEGDH.Populate(txtEmployee.Text.Trim(), (Session["FinYear"] as string).Trim());
                }
                else
                {
                    dtEGoals = oEGDH.Populate(txtEmployee.Text.Trim(), ddlYear.SelectedValue.Trim());
                }

                if (dtEGoals.Rows.Count > 0)
                {
                    DataRow[] drActiveGoals = dtEGoals.Select("STATUS_CODE = '" + Constants.CON_ACTIVE_STATUS + "'");

                    double totalWeight = 0.0;
                    double balanceWeight = 0.0;

                    for (int i = 0; i < drActiveGoals.Length; i++)
                    {
                        double weight = 0.0;
                        if (Double.TryParse(drActiveGoals[i]["WEIGHT"].ToString(), out weight))
                        {
                            totalWeight += weight;
                        }
                        else
                        {
                            totalWeight += 0.0;
                        }
                    }
                    balanceWeight = 100.00 - totalWeight;


                    lblBalaceWeightText.Visible = true;
                    lblTotalWeightText.Visible = true;
                    lblBalanceWeightValue.Visible = true;
                    lblTotalWeightValue.Visible = true;
                    lblMaximumWeightText.Visible = true;
                    lblMaximumWeightValue.Visible = true;
                    lblBalanceWeightValue.Text = balanceWeight.ToString() + "%";
                    lblTotalWeightValue.Text = totalWeight.ToString() + "%";

                }
                else
                {
                    lblBalaceWeightText.Visible = false;
                    lblTotalWeightText.Visible = false;
                    lblBalanceWeightValue.Visible = false;
                    lblTotalWeightValue.Visible = false;
                    lblMaximumWeightText.Visible = false;
                    lblMaximumWeightValue.Visible = false;
                    lblBalanceWeightValue.Text = String.Empty + "%";
                    lblTotalWeightValue.Text = String.Empty + "%";
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {
                oEGDH = null;
                dtEGoals.Dispose();
            }
        }

        Boolean isCommonUser()
        {
            Boolean Status = true;
            try
            {
                log.Debug("isCommonUser()");

                if ((Session["KeyHRIS_ROLE"] as string) == Constants.CON_COMMON_KeyHRIS_ROLE)
                {
                    Status = true;
                }
                else
                {
                    Status = false;
                }
            }
            catch 
            {
                Status = true;
            }
            return Status;
        }

        #endregion
    }
}