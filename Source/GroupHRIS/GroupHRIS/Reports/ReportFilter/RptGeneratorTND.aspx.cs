using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.Reports;
using System.Data;

namespace GroupHRIS.Reports.ReportFilter
{
    public partial class RptGeneratorTND : System.Web.UI.Page
    {
        public static string mStrRepName = "";

        private void LoadReport_RE038(string TrainingID)
        {
            ReportDataHandler RDH = new ReportDataHandler();
            DataTable ExpenseHeader = new DataTable();
            DataTable ExpectedExpenseDetails = new DataTable();
            DataTable ActualExpenseDetails = new DataTable();
            string prmTrainingName = String.Empty;

            try
            {
                ExpenseHeader = RDH.GetExpenseHeader(TrainingID).Copy();
                ExpectedExpenseDetails = RDH.GetExpectedExpenseDetails(TrainingID).Copy();
                ActualExpenseDetails = RDH.GetActualExpenseDetails(TrainingID).Copy();

                prmTrainingName = lblTrainingName.Text.Trim();

                Dictionary<string, string> paramdict = new Dictionary<string, string>();
                paramdict.Add("prmTrainingName", prmTrainingName);

                Session["rptDataSet1"] = ExpenseHeader;
                Session["rptDataSet2"] = ExpectedExpenseDetails;
                Session["rptDataSet3"] = ActualExpenseDetails;
                Session["rptParamDict"] = paramdict;
                Session["rptDisplayName"] = "Balance Budget Report";

                Response.Redirect("~/Reports/ReportViewers/rptBalanceBudget.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                RDH = null;
                ExpenseHeader.Dispose();
                ExpectedExpenseDetails.Dispose();
                ActualExpenseDetails.Dispose();
                prmTrainingName = String.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblerror);

            if (!IsPostBack)
            {
                string KeyUSER_ID = (string)(Session["KeyUSER_ID"]);

                if (!string.IsNullOrEmpty(KeyUSER_ID))
                {
                    lblrepname.Text = (Session["RptID"] as string);
                }
            }
            else
            {
                if (hfCaller.Value == "txtTrainingID")
                {
                    hfCaller.Value = "";
                    if (hfVal.Value != "")
                    {
                        txtTrainingID.Text = hfVal.Value;
                    }
                    if (txtTrainingID.Text != "")
                    {
                        //Postback Methods
                        lblTrainingName.Text = (Session["TrainingName"] as string);
                    }
                    else
                    {
                        lblTrainingName.Text = String.Empty;
                    }
                }
            }
        }

        

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblrepname.Text == "RE038")
                {
                    LoadReport_RE038(txtTrainingID.Text.Trim());
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

        protected void btnClear_Click(object sender, EventArgs e)
        {

        }
    }
}