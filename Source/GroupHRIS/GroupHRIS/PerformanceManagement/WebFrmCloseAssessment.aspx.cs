using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataHandler.PerformanceManagement;
using System.Data;
using Common;
using NLog;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmCloseAssessment : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        //public string js = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //drawCharts(null);
            sIPAddress = Request.UserHostAddress;

            log.Debug("IP:" + sIPAddress + "WebFrmCloseAssessment : Page_Load");
            try
            {
                if (Session["KeyLOGOUT_STS"] == null)
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
                if ((Session["KeyCOMP_ID"] != null) && (Session["KeyCOMP_ID"].ToString() != String.Empty))
                {
                    string comId = Session["KeyCOMP_ID"].ToString();
                    if (Session["KeyCOMP_ID"].ToString().Trim().Equals(Constants.CON_UNIVERSAL_COMPANY_CODE))
                    {
                        fillCompanyDropDown();
                    }
                    else
                    {
                        fillCompanyDropDown(Session["KeyCOMP_ID"].ToString().Trim());
                    }
                }
                fillYearDropdown();
                fillStatusDropDown();
                loadAssessmentGridView(null);
                detailTbl.Visible = false;
                assessmentsTbl.Visible = false;
                txtRemarks.Visible = true;
                tblGraphs.Visible = false;

                ///// 2016-10-25 /////
                //getCompletedAssessmentCount();
            }
        }


        #region methodes
        protected void fillCompanyDropDown()
        {
            CloseAssessmentDataHandler closeAssessmentDataHandler = new CloseAssessmentDataHandler();
            try
            {
                DataTable companyDataTable = closeAssessmentDataHandler.getAllActiveCompanies();

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                //ddlSearchCompany.Items.Add(listItemBlank);
                ddlCompany.Items.Add(listItemBlank);

                foreach (DataRow company in companyDataTable.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = company[1].ToString();
                    listItem.Value = company[0].ToString();
                    //ddlSearchCompany.Items.Add(listItem);
                    ddlCompany.Items.Add(listItem);
                }
                companyDataTable.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                closeAssessmentDataHandler = null;
            }
        }

        protected void fillCompanyDropDown(string companyId)
        {
            CloseAssessmentDataHandler closeAssessmentDataHandler = new CloseAssessmentDataHandler();
            DataTable companyDataTable = new DataTable();
            try
            {
                companyDataTable = closeAssessmentDataHandler.getCompanyById(companyId);

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlCompany.Items.Add(listItemBlank);

                foreach (DataRow company in companyDataTable.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = company[1].ToString();
                    listItem.Value = company[0].ToString();
                    ddlCompany.Items.Add(listItem);
                }

                ddlCompany.SelectedValue = companyId;
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                closeAssessmentDataHandler = null;
                companyDataTable.Dispose();
            }
        }

        protected void fillYearDropdown()
        {
            CloseAssessmentDataHandler closeAssessmentDataHandler = new CloseAssessmentDataHandler();
            try
            {
                DataTable yearDataTable = closeAssessmentDataHandler.getDistinctYears();

                string currentFinancialYear = getCurrentFinancialYear().ToString();
                DataRow[] existingEntry = yearDataTable.Select("YEAR_OF_ASSESSMENT ='" + currentFinancialYear + "'");

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlYear.Items.Add(listItemBlank);



                foreach (DataRow year in yearDataTable.Rows)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = year[0].ToString();
                    listItem.Value = year[0].ToString();
                    ddlYear.Items.Add(listItem);

                }
                if (existingEntry.Count() == 0)
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = currentFinancialYear;
                    listItem.Value = currentFinancialYear;
                    ddlYear.Items.Add(listItem);
                }

                

                ddlYear.SelectedValue = currentFinancialYear;
                yearDataTable.Dispose();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                closeAssessmentDataHandler = null;
            }
        }

        private void fillStatusDropDown()
        {
            try
            {

                ListItem listItemBlank = new ListItem();
                listItemBlank.Text = "";
                listItemBlank.Value = "";
                ddlStatus.Items.Add(listItemBlank);

                ListItem pendingItem = new ListItem();
                pendingItem.Text = Constants.ASSESSNEMT_PENDING_TAG;
                pendingItem.Value = Constants.ASSESSNEMT_PENDING_STATUS;
                ddlStatus.Items.Add(pendingItem);

                ListItem completedItem = new ListItem();
                completedItem.Text = "completed";
                completedItem.Value = Constants.ASSESSNEMT_CEO_FINALIZED_STATUS;
                ddlStatus.Items.Add(completedItem);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        //private void autoSelectCurrentFinancialYear()
        //{
        //    try
        //    {
        //        int currentFinancialYear = getCurrentFinancialYear();
        //        ddlYear.SelectedValue = currentFinancialYear.ToString();
        //    }
        //    catch (Exception Ex)
        //    {
        //        CommonVariables.MESSAGE_TEXT = Ex.Message;
        //        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
        //    }
        //}

        private int getCurrentFinancialYear()
        {
                int currentFinancialYear;

                var startingDayForThisYear = new DateTime(DateTime.Today.Year, 04, 01);
                if (DateTime.Compare(DateTime.Today, startingDayForThisYear) < 0) /// today belongs to previouse year
                {
                    currentFinancialYear = DateTime.Today.Year - 1;
                }
                else
                {
                    currentFinancialYear = DateTime.Today.Year;
                }

                return currentFinancialYear;
                       
        }

        //private DataTable getClosedAndCompletedAssessments(string companyId, string year)
        //{
        //    CloseAssessmentDataHandler closeAssessmentDataHandler = new CloseAssessmentDataHandler();
        //    DataTable dtClosedAndCompletedAssessments = new DataTable();
        //    try
        //    {
        //        dtClosedAndCompletedAssessments = closeAssessmentDataHandler.getClosedAndCompletedAssessmentsSummery(companyId, year);
        //        return dtClosedAndCompletedAssessments;
        //    }
        //    catch (Exception Ex)
        //    {
        //        CommonVariables.MESSAGE_TEXT = Ex.Message;
        //        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
        //        return null;
        //    }
        //    finally
        //    {
        //        closeAssessmentDataHandler = null;
        //        dtClosedAndCompletedAssessments.Dispose();
        //    }
        //}

        //private DataTable getUnclosedAndInCompleteAssessments(string companyId, string year)
        //{
        //    CloseAssessmentDataHandler closeAssessmentDataHandler = new CloseAssessmentDataHandler();
        //    DataTable dtUnclosedAndInCompleteAssessments = new DataTable();
        //    try
        //    {
        //        dtUnclosedAndInCompleteAssessments = closeAssessmentDataHandler.getUnclosedAndInCompleteAssessmentsSummery(companyId, year);
        //        return dtUnclosedAndInCompleteAssessments;
        //    }
        //    catch (Exception Ex)
        //    {
        //        CommonVariables.MESSAGE_TEXT = Ex.Message;
        //        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
        //        return null;
        //    }
        //    finally
        //    {
        //        closeAssessmentDataHandler = null;
        //        dtUnclosedAndInCompleteAssessments.Dispose();
        //    }
        //}

        private DataTable getAssessmentSummery(string companyId, string year)
        {
            CloseAssessmentDataHandler closeAssessmentDataHandler = new CloseAssessmentDataHandler();
            DataTable dtAssessmentSummery = new DataTable();
            try
            {
                dtAssessmentSummery = closeAssessmentDataHandler.getAssessmentSummery(companyId, year);
                return dtAssessmentSummery;
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                return null;
            }
            finally
            {
                closeAssessmentDataHandler = null;
                dtAssessmentSummery.Dispose();
            }
        }

        private void populateSummery(DataTable dtUclosedAssessments)
        {

        }

        private void drawCharts(DataTable assessmentSummery, int completedAssessmentCount)
        {
            try
            {
                int closedCount = 0;
                int completedCount = completedAssessmentCount;
                int pendingCount = 0;
                int activeCount = 0;
                string js = @"
                            <script>
                                var randomColorFactor = function() {
                                    return Math.round(Math.random() * 255);
                                };
                                var randomColor = function(opacity) {
                                    return 'rgba(' + randomColorFactor() + ',' + randomColorFactor() + ',' + randomColorFactor() + ',' + (opacity || '.7') + ')';
                                };

                                var config = {
                                    type: 'pie',
                                    data: {
                                        datasets: [{
                                            data: [";

                                            foreach (DataRow assessmentCount in assessmentSummery.Rows)
                                            {
                                                if (
                                                    //assessmentCount[0].ToString() == Constants.ASSESSNEMT_PENDING_STATUS || 
                                                    assessmentCount[0].ToString() == Constants.ASSESSNEMT_ACTIVE_STATUS
                                                    //|| assessmentCount[0].ToString() == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS
                                                    //|| assessmentCount[0].ToString() == Constants.ASSESSNEMT_SUBORDINATE_DISAGREE_STATUS
                                                    //|| assessmentCount[0].ToString() == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS
                                                    //|| assessmentCount[0].ToString() == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS
                                                    )
                                                {
                                                    //pendingCount += Convert.ToInt16(assessmentCount[1].ToString());
                                                    activeCount += Convert.ToInt16(assessmentCount[1].ToString());
                                                }
                                                //if (assessmentCount[0].ToString() == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                                                //{
                                                //    completedCount += Convert.ToInt16(assessmentCount[1].ToString());
                                                //}
                                                if (assessmentCount[0].ToString() == Constants.ASSESSNEMT_CLOSED_STATUS)
                                                {
                                                    closedCount += Convert.ToInt16(assessmentCount[1].ToString());
                                                }

                                            }
                                            pendingCount = activeCount - completedCount;

                                            js += pendingCount.ToString() + "," + completedCount.ToString() + "," + closedCount.ToString() + ",";


                                            js += @"   ],
                                            backgroundColor: [
                                                '#2980b9',
                                                '#1abc9c',
                                                '#f1c40f',
                    
                                            ],
                                            label: 'Dataset 1'
                                        }],
                                        labels: [";
                                            js += @"'Pending',
                                         'Completed',
                                         'Closed'";


                                            js += @"]
                                    },
                                    options: {
                                        responsive: true,
                                        legend: {
                                            position: 'left',
                                        },
                                        title: {
                                            display: false,
                                            text: ''
                                        },
                                        animation: false
                                    }
                                };

                                window.onload = function() {
                                    var ctx = document.getElementById('chart-area').getContext('2d');
                                    window.myDoughnut = new Chart(ctx, config);
                                    
                                    var ctx = document.getElementById('bar-chart-area').getContext('2d');
                                    window.myBar = new Chart(ctx, {
                                        type: 'bar',
                                        data: barChartData,
                                        options: {
                                            // Elements options apply to all of the options unless overridden in a dataset
                                            // In this case, we are setting the border of each bar to be 2px wide and green
                                            elements: {
                                                rectangle: {
                                                    borderWidth: 0,
                                                    borderColor: 'rgb(0, 0, 0)',
                                                    borderSkipped: 'bottom'
                                                }
                                            },
                                            responsive: true,
                                            legend: {
                                                position: 'right',
                                                display: false,
                                            },
                                            title: {
                                                //display: true,
                                                //text: 'Chart.js Bar Chart'
                                            },
                                            animation: false,
                                            scales: {
                                                        yAxes: [{
                                                        ticks: {
                                                            scaleIntegersOnly: true,
                                                            beginAtZero: true
                                                        }
                                                        }]
                                                    }
                                        }
                                    });
                                    
                                };



                            </script>            
                            ";
                jsLable.Text = js;

                string barJs = @"
                                <script>
                                        var randomColorFactor = function() {
                                            return Math.round(Math.random() * 255);
                                        };
                                        var randomColor = function() {
                                            return 'rgba(' + randomColorFactor() + ',' + randomColorFactor() + ',' + randomColorFactor() + ',.7)';
                                        };

                                        var barChartData = {
                                            labels: ['Pending', 'Completed', 'Closed'],
                                            datasets: [{
                                                label: 'Assessments',
                                                backgroundColor: 'rgba( 17, 120, 100,0.9)',
                                                data: [";
                                                barJs += pendingCount.ToString() + "," + completedCount.ToString() + "," + closedCount.ToString();
                                                barJs += @"]
                                            } 
			                                ]


                                        };

                                   

                                </script>
                                ";
                lblBarChartScript.Text = barJs;

                lblTotal1.Text = "Total no. of Assessments : " + Convert.ToString(pendingCount + closedCount + completedCount);
                lblTotal2.Text = lblTotal1.Text.ToString();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private void clearFields()
        {
            //Utility.Utils.clearControls(true, txtCeoFinalized, txtEmployeeFinalized, txtRemarks, txtSupervisorCompleted, txtSupervisorFinalized, txtTotalEmp, lblBarChartScript, jsLable, lblTotal1, lblTotal2);

            Utility.Utils.clearControls(true, txtCeoFinalized, txtEmployeeFinalized, txtRemarks, txtSupervisorCompleted, txtSupervisorFinalized, txtTotalEmp, lblAssessmentName, chkForceClose);
            
            //Utility.Errorhandler.ClearError(lblErrorMsg);
        }

        private void populateAssessments()
        {
            CloseAssessmentDataHandler closeAssessmentDataHandler = new CloseAssessmentDataHandler();
            DataTable dtAssessments = new DataTable();
            try
            {
                string selectedStatus = ddlStatus.SelectedValue.ToString();
                string companyId = ddlCompany.SelectedValue.ToString();
                string year = ddlYear.SelectedValue.ToString();

                List<string> completedAssessmentList = getCompletedAssessmentList(companyId, year);

                    if (selectedStatus == Constants.ASSESSNEMT_PENDING_STATUS)
                    {
                        dtAssessments = closeAssessmentDataHandler.getPendingOrCompletedAssessments(companyId, year, completedAssessmentList, Constants.ASSESSNEMT_PENDING_STATUS);
                    }
                    else if (selectedStatus == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        if (completedAssessmentList.Count > 0)
                        {
                            dtAssessments = closeAssessmentDataHandler.getPendingOrCompletedAssessments(companyId, year, completedAssessmentList, Constants.ASSESSNEMT_CEO_FINALIZED_STATUS);
                        }
                        
                    }

                //dtAssessments = getAssessments(selectedStatus, companyId, year);
                loadAssessmentGridView(dtAssessments);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                if (dtAssessments != null)
                {
                    dtAssessments.Dispose();
                }
                closeAssessmentDataHandler = null;
            }
        }

        private DataTable getAssessments(string status, string companyId, string year)
        {
            DataTable dtAssessments = new DataTable();
            CloseAssessmentDataHandler closeAssessmentDataHandler = new CloseAssessmentDataHandler();

            try
            {
                dtAssessments = closeAssessmentDataHandler.getAssessmentsByStatus(status, companyId, year);
                return dtAssessments;

            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                return null;
            }
            finally
            {
                dtAssessments.Dispose();
                closeAssessmentDataHandler = null;
            }
        }

        private void loadAssessmentGridView(DataTable assessment)
        {
            try
            {
                if (assessment != null)
                {
                    if (assessment.Rows.Count > 0)
                    {
                        gvAssessment.DataSource = assessment;
                        gvAssessment.DataBind();
                    }
                    else
                    {
                        gvAssessment.DataSource = null;
                        gvAssessment.DataBind();
                    }
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        private DataTable getStatusSummeryForAssessment(string assessmentId)
        {
            CloseAssessmentDataHandler closeAssessmentDataHandler = new CloseAssessmentDataHandler();
            DataTable assessmentStatusSummery = new DataTable();
            try
            {
                assessmentStatusSummery = closeAssessmentDataHandler.assessmentStatusSummery(assessmentId);
                return assessmentStatusSummery;
            }
            catch (Exception Ex)
            {

                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                return null;
            }
            finally
            {
                closeAssessmentDataHandler = null;
                assessmentStatusSummery.Dispose();
            }
        }

        private void fillStatusDetails(DataTable assessmentSummery)
        {
            try
            {
                int totalEmployees = 0;
                foreach (DataRow status in assessmentSummery.Rows)
                {
                    if (status[0].ToString() == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS)
                    {
                        txtCeoFinalized.Text = status[1].ToString();
                    }
                    if (status[0].ToString() == Constants.ASSESSNEMT_SUPERVISOR_FINALIZED_STATUS)
                    {
                        txtSupervisorFinalized.Text = status[1].ToString();
                    }
                    if (status[0].ToString() == Constants.ASSESSNEMT_SUPERVISOR_COMPLETED_STATUS)
                    {
                        txtSupervisorCompleted.Text = status[1].ToString();
                    }
                    if (status[0].ToString() == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
                    {
                        txtEmployeeFinalized.Text = status[1].ToString();
                    }
                    totalEmployees += Convert.ToInt16(status[1].ToString());
                }
                txtTotalEmp.Text = totalEmployees.ToString();
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        /// <summary>
        /// 2016-10-24 change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private DataTable getAssessmentCountForDistinctStatusInDistinctActiveAssessments(string companyId, string year)
        {
            CloseAssessmentDataHandler closeAssessmentDataHandler = new CloseAssessmentDataHandler();
            try
            {
                
                DataTable resultsTable = closeAssessmentDataHandler.getAssessmentCountForDistinctStatusInDistinctActiveAssessments(companyId, year);
                return resultsTable;
            }
            catch (Exception Ex)
            {

                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                return null;
            }
        }

        private List<string> getCompletedAssessmentList(string companyId, string year)
        {
            DataTable result = new DataTable();
            try
            {
                result = getAssessmentCountForDistinctStatusInDistinctActiveAssessments(companyId, year);
                DataRow[] assessmentsWhichHaveCompletedEvaluations = result.Select("STATUS ='" + Constants.ASSESSNEMT_CEO_FINALIZED_STATUS + "'");
                string assessmentId = "";
                List<string> completedAssessmentList = new List<string>();
                if (assessmentsWhichHaveCompletedEvaluations.Count() > 0)
                {
                    foreach (DataRow assessmentRow in assessmentsWhichHaveCompletedEvaluations)
                    {
                        assessmentId = assessmentRow[0].ToString();
                        DataRow[] anyAssessmentsInOtherStatus = result.Select("ASSESSMENT_ID ='" + assessmentId + "' and STATUS <> '" + Constants.ASSESSNEMT_CEO_FINALIZED_STATUS + "' ");
                        if (anyAssessmentsInOtherStatus.Count() == 0)
                        {
                            completedAssessmentList.Add(assessmentId);
                        }

                    }

                }

                int completedAssessmentCount = completedAssessmentList.Count();

                result.Dispose();
                return completedAssessmentList;
            }
            catch (Exception Ex)
            {

                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                return null;
            }
            finally
            {
                result.Dispose();
            }
        }

        #endregion

        #region events

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlCompany_SelectedIndexChanged()"); 
            clearFields();
            assessmentsTbl.Visible = false;
            tblGraphs.Visible = false;
            detailTbl.Visible = false;

        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            log.Debug("btnSearch_Click()");
            DataTable pendingAndClosedAssessmentSummery = new DataTable();
            
            try
            {
                string companyId = ddlCompany.SelectedValue.ToString();
                string year = ddlYear.SelectedValue.ToString();

                if (!String.IsNullOrEmpty(companyId) && !String.IsNullOrEmpty(year))
                {
                    Utility.Errorhandler.ClearError(lblErrorMsg2);
                    pendingAndClosedAssessmentSummery = getAssessmentSummery(companyId, year);
                    List<string> completedAssessmentList = getCompletedAssessmentList(companyId, year);
                    int completedAssessmentCount = completedAssessmentList.Count();

                    if (pendingAndClosedAssessmentSummery.Rows.Count > 0 || completedAssessmentCount > 0)
                    {

                        drawCharts(pendingAndClosedAssessmentSummery, completedAssessmentCount);
                        assessmentsTbl.Visible = true;
                        tblGraphs.Visible = true;
                        Utility.Utils.clearControls(true, ddlStatus);
                        gvAssessment.DataSource = null;
                        gvAssessment.DataBind();
                        Utility.Utils.clearControls(false, txtCeoFinalized, txtEmployeeFinalized, txtRemarks, txtSupervisorCompleted, txtSupervisorFinalized, txtTotalEmp, lblErrorMsg, chkForceClose, lblAssessmentName);
                        detailTbl.Visible = true;
                    }
                    else
                    {
                        CommonVariables.MESSAGE_TEXT = "No assessment found for the selected Company and Year";
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                    }
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Please select Company and Year before search";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg2);
                }

               
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                pendingAndClosedAssessmentSummery = null;
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("ddlStatus_SelectedIndexChanged()");
            try
            {
                clearFields();
                populateAssessments();


                
                //string selectedStatus = ddlStatus.SelectedValue.ToString();
                //string companyId = ddlCompany.SelectedValue.ToString();
                //string year = ddlYear.SelectedValue.ToString();

                //DataTable dtAssessments = getAssessments(selectedStatus, companyId, year);
                //loadAssessmentGridView(dtAssessments);
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void gvAssessment_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("gvAssessment_SelectedIndexChanged()");
            DataTable assessmentSummery = new DataTable();
            try
            {
                int index = gvAssessment.SelectedIndex;
                string assessmentId = gvAssessment.Rows[index].Cells[0].Text.ToString();
                hfAssessmentId.Value = assessmentId;

                string assessmentName = gvAssessment.Rows[index].Cells[1].Text.ToString();
                
                assessmentSummery = getStatusSummeryForAssessment(assessmentId);
                Utility.Utils.clearControls(false, txtCeoFinalized, txtEmployeeFinalized, txtRemarks, txtSupervisorCompleted, txtSupervisorFinalized, txtTotalEmp, chkForceClose, lblAssessmentName);
                Utility.Errorhandler.ClearError(lblErrorMsg);
                detailTbl.Visible = true;
                lblAssessmentName.Text = assessmentName;
                fillStatusDetails(assessmentSummery);
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally
            {
                assessmentSummery = null;
            }
        }

        protected void gvAssessment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            log.Debug("gvAssessment_RowDataBound()");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvAssessment, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void gvAssessment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("gvAssessment_PageIndexChanging()");
            gvAssessment.PageIndex = e.NewPageIndex;
            populateAssessments();

        }

        protected void chkForceClose_CheckedChanged(object sender, EventArgs e)
        {
            //CheckBox closeByForce = FindControl("chkForceClose") as CheckBox;
            //if (chkForceClose.Checked == true)
            //{
            //    txtRemarks.Visible = true;

            //}
            //else if (chkForceClose.Checked == false)
            //{
            //    txtRemarks.Visible = false;

            //}
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            log.Debug("btnClose_Click()");
            CloseAssessmentDataHandler closeAssessmentDataHandler = new CloseAssessmentDataHandler();
            try
            {
                string status = ddlStatus.SelectedValue.ToString();
                string userId = Session["KeyUSER_ID"].ToString();
                string reason = txtRemarks.Text.ToString();

                string assessmentId = hfAssessmentId.Value.ToString();
                if (!String.IsNullOrEmpty(assessmentId))
                {
                    if (status == Constants.ASSESSNEMT_CEO_FINALIZED_STATUS) /// close assessment
                    {

                        if (String.IsNullOrEmpty(reason))
                        {
                            reason = " Successfully Completed ";
                        }
                        bool isClosed = closeAssessmentDataHandler.closeAssessment(assessmentId, userId, reason);
                        if (isClosed)
                        {
                            CommonVariables.MESSAGE_TEXT = "Assessment Successfully Closed";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            hfAssessmentId.Value = "";
                            clearFields();
                            populateAssessments();

                        }
                        else
                        {
                            CommonVariables.MESSAGE_TEXT = "Couldn't close the assessment";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }
                    }
                    else /// close by force
                    {
                        if (chkForceClose.Checked == false)
                        {
                            CommonVariables.MESSAGE_TEXT = " Please select 'close by force' if you are closing an uncompleted assessment";
                            Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                        }
                        else
                        {
                            if (chkForceClose.Checked = true && !String.IsNullOrEmpty(txtRemarks.Text.ToString()))
                            {
                                //string assessmentId = hfAssessmentId.Value.ToString();
                                bool isClosed = closeAssessmentDataHandler.closeAssessment(assessmentId, userId, reason);
                                if (isClosed)
                                {
                                    CommonVariables.MESSAGE_TEXT = "Assessment Successfully Closed";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                    hfAssessmentId.Value = "";
                                    clearFields();
                                    populateAssessments();
                                }
                                else
                                {
                                    CommonVariables.MESSAGE_TEXT = "Couldn't close the assessment";
                                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                                }
                            }
                            else
                            {
                                CommonVariables.MESSAGE_TEXT = " Please enter a valid reason if you are closing an uncompleted assessment";
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                            }
                        }
                    }
                }
                else
                {
                    CommonVariables.MESSAGE_TEXT = "Select assessmet to close";
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
                }
            }
            catch (Exception Ex)
            {
                CommonVariables.MESSAGE_TEXT = Ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
            finally {

                closeAssessmentDataHandler = null;
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("btnClear_Click()");
            try
            {
                clearFields();
                //Utility.Utils.clearControls(true, ddlCompany, ddlYear, ddlStatus);
                hfAssessmentId.Value = "";
                //gvAssessment.DataSource = null;
                //gvAssessment.DataBind();
                detailTbl.Visible = true;
                assessmentsTbl.Visible = true;
                tblGraphs.Visible = true;
                Utility.Errorhandler.ClearError(lblErrorMsg);
            }
            catch (Exception Ex)
            {

                CommonVariables.MESSAGE_TEXT = Ex.Message.ToString();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, CommonVariables.MESSAGE_TEXT, lblErrorMsg);
            }
        }

        #endregion

        

        

        









    }
}