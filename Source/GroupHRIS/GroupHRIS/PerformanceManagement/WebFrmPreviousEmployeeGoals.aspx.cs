using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.PerformanceManagement;
using System.Data;
using Common;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmPreviousEmployeeGoals : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillYears();
                setBalancedAndTotalWeights();
                lblEmployeeName.Text = (Session["lblEmployeeName"] as string);
            }
        }

        void fillYears()
        {
            try
            {
                EmployeeGoalDataHandler oEGDH = new EmployeeGoalDataHandler();
                DataTable dtYear = new DataTable();
                dtYear = oEGDH.PopulateYearofGoals((Session["SelectedEmployeeID"] as string), (Session["FinYear"] as string));

                ddlYear.Items.Add(new ListItem("", ""));

                for (int i = 0; i < dtYear.Rows.Count; i++)
                {
                    ddlYear.Items.Add(new ListItem(dtYear.Rows[i]["YEAR_OF_GOAL"].ToString(), dtYear.Rows[i]["YEAR_OF_GOAL"].ToString()));
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
        }

        void loadGrid()
        {
            EmployeeGoalDataHandler oEGDH = new EmployeeGoalDataHandler();
            DataTable dtEGoals = new DataTable();

            dtEGoals = oEGDH.Populate((Session["SelectedEmployeeID"] as string), ddlYear.SelectedValue.ToString());



            grdvEmployeeGoals.DataSource = dtEGoals.Copy();
            grdvEmployeeGoals.DataBind();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            loadGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeeGoalDataHandler oEGDH = new EmployeeGoalDataHandler();
                List<int> Selectedindex = new List<int>();


                string SelectedEmplyeeID = (Session["SelectedEmployeeID"] as string);
                string CurrentFinYear = Common.CommonUtils.currentFinancialYear();
                for (int i = 0; i < grdvEmployeeGoals.Rows.Count; i++)
                {
                    CheckBox chkInclude = (grdvEmployeeGoals.Rows[i].FindControl("chkisInclude") as CheckBox);
                    if (chkInclude.Checked == true)
                    {
                        string GoalArea = HttpUtility.HtmlDecode(grdvEmployeeGoals.Rows[i].Cells[3].Text.Trim());
                        if (oEGDH.CheckGoalAreaExsistance(SelectedEmplyeeID, CurrentFinYear, GoalArea))
                        {
                            CommonVariables.MESSAGE_TEXT = "Selected goal area(s) already exists for current year.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }

                        double SelectedWeight = 0.0;
                        TextBox txtEmployeeWeight = (grdvEmployeeGoals.Rows[i].FindControl("txtEmployeeWeight") as TextBox);
                        Double.TryParse(txtEmployeeWeight.Text, out SelectedWeight);
                        if ((SelectedWeight > 0) == false)
                        {
                            CommonVariables.MESSAGE_TEXT = "Selected goal weight(s) can not be '0'.";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                            return;
                        }
                    }

                }


                for (int i = 0; i < grdvEmployeeGoals.Rows.Count; i++)
                {
                    //string goalID = grdvEmployeeGoals.Rows[i].Cells[8].Text;

                    CheckBox chkInclude = (grdvEmployeeGoals.Rows[i].FindControl("chkisInclude") as CheckBox);
                    if (chkInclude.Checked == true)
                    {
                        Selectedindex.Add(i);
                    }
                }

                double selectedGoalWeights = 0;
                if (Selectedindex.Count > 0)
                {
                    for (int i = 0; i < Selectedindex.Count; i++)
                    {
                        TextBox txtEmployeeWeight = (grdvEmployeeGoals.Rows[Selectedindex[i]].FindControl("txtEmployeeWeight") as TextBox);
                        selectedGoalWeights += Convert.ToDouble(txtEmployeeWeight.Text);
                    }


                    // TOTAL WEIGHT VALIDATION
                    double Wght = Convert.ToDouble(selectedGoalWeights);


                    double currentyExistWeight = oEGDH.getCurrentTotalWeight((Session["SelectedEmployeeID"] as string), (Session["FinYear"] as string));

                    double newTotalWeight = currentyExistWeight + Wght;

                    if (newTotalWeight > 100)
                    {
                        CommonVariables.MESSAGE_TEXT = "Total Weight Should be Less than or Equal to 100 %";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblerror);
                        return;
                    }
                    // --


                    DataTable dtNewGoals = new DataTable();
                    dtNewGoals.Columns.Add("EmployeeID");
                    dtNewGoals.Columns.Add("yearOfGoal");
                    dtNewGoals.Columns.Add("GoalGroup");
                    dtNewGoals.Columns.Add("GoalArea");
                    dtNewGoals.Columns.Add("Remarks");
                    dtNewGoals.Columns.Add("Measurement");
                    dtNewGoals.Columns.Add("Weight");
                    dtNewGoals.Columns.Add("Status");
                    dtNewGoals.Columns.Add("addedBy");




                    for (int i = 0; i < Selectedindex.Count; i++)
                    {
                        DataRow drGoal = dtNewGoals.NewRow();

                        drGoal["EmployeeID"] = grdvEmployeeGoals.Rows[Selectedindex[i]].Cells[0].Text;
                        drGoal["yearOfGoal"] = (Session["FinYear"] as string);
                        drGoal["GoalGroup"] = grdvEmployeeGoals.Rows[Selectedindex[i]].Cells[2].Text;
                        drGoal["GoalArea"] = grdvEmployeeGoals.Rows[Selectedindex[i]].Cells[3].Text;
                        drGoal["Remarks"] = grdvEmployeeGoals.Rows[Selectedindex[i]].Cells[4].Text;
                        drGoal["Measurement"] = grdvEmployeeGoals.Rows[Selectedindex[i]].Cells[5].Text;

                        TextBox txtEmployeeWeight = (grdvEmployeeGoals.Rows[Selectedindex[i]].FindControl("txtEmployeeWeight") as TextBox);
                        drGoal["Weight"] = txtEmployeeWeight.Text;
                        //drGoal["Weight"] = grdvEmployeeGoals.Rows[Selectedindex[i]].Cells[6].Text;


                        drGoal["Status"] = grdvEmployeeGoals.Rows[Selectedindex[i]].Cells[7].Text;
                        drGoal["addedBy"] = (Session["KeyEMPLOYEE_ID"] as string).Trim();

                        dtNewGoals.Rows.Add(drGoal);
                        //selectedGoalWeights += Convert.ToDouble(grdvEmployeeGoals.Rows[Selectedindex[i]].Cells[6].Text);
                    }


                    Boolean isInserted = oEGDH.Insert(dtNewGoals.Copy());
                    if (isInserted)
                    {
                        //clearFields();
                        loadGrid();
                        CommonVariables.MESSAGE_TEXT = CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED;
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblerror);

                        hfStatus.Value = "Saved";
                    }

                }


                setBalancedAndTotalWeights();
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {

            }
        }

        protected void chkisIncludeAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkisIncludeAll = (grdvEmployeeGoals.HeaderRow.FindControl("chkisIncludeAll") as CheckBox);
            if (chkisIncludeAll.Checked == true)
            {
                for (int i = 0; i < grdvEmployeeGoals.Rows.Count; i++)
                {
                    CheckBox chkisInclude = (grdvEmployeeGoals.Rows[i].FindControl("chkisInclude") as CheckBox);
                    chkisInclude.Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < grdvEmployeeGoals.Rows.Count; i++)
                {
                    CheckBox chkisInclude = (grdvEmployeeGoals.Rows[i].FindControl("chkisInclude") as CheckBox);
                    chkisInclude.Checked = false;
                }
            }
            setBalancedAndTotalWeights();
        }


        void setBalancedAndTotalWeights()
        {
            EmployeeGoalDataHandler oEGDH = new EmployeeGoalDataHandler();
            DataTable dtEGoals = new DataTable();
            try
            {
                dtEGoals = oEGDH.Populate((Session["SelectedEmployeeID"] as string).Trim(), (Session["FinYear"] as string).Trim());

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
                    lblBalanceWeightValue.Text = balanceWeight.ToString() + "%";
                    lblTotalWeightValue.Text = totalWeight.ToString() + "%";

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

        protected void chkisInclude_CheckedChanged(object sender, EventArgs e)
        {
            checkWeights();
            setBalancedAndTotalWeights();
        }

        void checkWeights()
        {
            try
            {
                string cumVal = (lblTotalWeightValue.Text.Replace("%", "")).Trim();

                //double cumulativeWeight = Convert.ToDouble(cumVal);
                double cumulativeWeight = 0.0;

                Double.TryParse("100", out cumulativeWeight);

                double checkedWeights = 0.0;

                for (int i = 0; i < grdvEmployeeGoals.Rows.Count; i++)
                {
                    CheckBox chkisInclude = (grdvEmployeeGoals.Rows[i].FindControl("chkisInclude") as CheckBox);
                    TextBox txtEmployeeWeight = (grdvEmployeeGoals.Rows[i].FindControl("txtEmployeeWeight") as TextBox);

                    if (chkisInclude.Checked == true)
                    {
                        double tempWeght = 0.0;
                        if (Double.TryParse(txtEmployeeWeight.Text, out tempWeght))
                        {
                            checkedWeights += tempWeght;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (checkedWeights > cumulativeWeight)
                {
                    CommonVariables.MESSAGE_TEXT = "Total Weight Should be Less than or Equal to 100 %";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
                }



            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {

            }
        }

        protected void grdvEmployeeGoals_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtEmployeeWeight = (e.Row.FindControl("txtEmployeeWeight") as TextBox);
                    txtEmployeeWeight.Text = e.Row.Cells[11].Text;
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblerror);
            }
            finally
            {

            }
        }

    }
}