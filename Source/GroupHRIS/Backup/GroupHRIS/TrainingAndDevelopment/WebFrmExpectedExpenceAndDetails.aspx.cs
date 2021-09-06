using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.TrainingAndDevelopment;
using System.Data;
using GroupHRIS.Utility;
using NLog;

namespace GroupHRIS.TrainingAndDevelopment
{
    public partial class WebFrmExpectedExpenceAndDetails : System.Web.UI.Page
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private string sIPAddress = "";
        double tot = 0;
        string flag = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sIPAddress = Request.UserHostAddress;
            log.Debug("IP:" + sIPAddress + "WebFrmExpectedExpenceAndDetails : Page_Load");

            Errorhandler.ClearError(lblMessage);
            Errorhandler.ClearError(lblDetailMessage);

            if (!IsPostBack)
            {
                fillStatus(ddlTrStatus);
                fillStatus(ddlStatus);
                allTrainingCategory();
                createExpenseBucket();
            }

            if (hfCaller.Value == "txtTraining")
            {
                log.Debug("Select Training.");
                clearAllTrainingDetails();
                txtTraining.Text = hfVal.Value;
                lblTrainingName.Text = hfTrName.Value;
                lblProgramName.Text = hfProName.Value;

                companyExpenseDetails(txtTraining.Text);
                viewExpenceCategoryDetails(txtTraining.Text);
                txtTrTotal.Text = readTotExistCost().ToString();
                viewPerPersonCost();
                isRecordAlreadyExist();
                hfCaller.Value = "";

                if (grdCompanywiseExpence.Rows.Count > 0)
                {
                    disableFields(true);
                }
                else
                {
                    disableFields(false);
                }

            }
        }

        protected void grdCompanywiseExpence_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int count = participantCount();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (hfrecordVal.Value == "" || flag == "Click")
                {
                    DropDownList ddlcompStatus = (e.Row.FindControl("ddlcompStatus") as DropDownList);

                    ddlcompStatus.Items.Insert(0, new ListItem("", ""));
                    ddlcompStatus.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                    ddlcompStatus.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));
                    ddlcompStatus.SelectedIndex = 1;

                    if (count > 0 && txtCost.Text != "")
                    {
                        string comp_count = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "PLANNED_PARTICIPANTS"));

                        double amount = Double.Parse(txtCost.Text);
                        TextBox lblAmount = (e.Row.FindControl("lblcmpAmount") as TextBox);
                        lblAmount.Text = ((amount / count) * Int32.Parse(comp_count)).ToString();
                        tot = tot + (amount / count) * Int32.Parse(comp_count);
                    }
                }
                else
                {
                    // bindCompanyDetails(txtTraining.Text);
                    ExpectedExpenceDetails EED = new ExpectedExpenceDetails();
                    DataTable table = new DataTable();
                    DataTable dt = new DataTable();

                    table = EED.getExpectedExpenses(txtTraining.Text);
                    dt = EED.getCompanyWiseExpenceDetails(hfrecordVal.Value, txtTraining.Text);

                    DropDownList ddlcompStatus = (e.Row.FindControl("ddlcompStatus") as DropDownList);

                    ddlcompStatus.Items.Insert(0, new ListItem("", ""));
                    ddlcompStatus.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                    ddlcompStatus.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));

                    if (count > 0 && txtCost.Text != "")
                    {
                        string comp_count = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "PLANNED_PARTICIPANTS"));

                        double amount = Double.Parse(txtCost.Text);
                        TextBox lblAmount = (e.Row.FindControl("lblcmpAmount") as TextBox);
                        lblAmount.Text = table.Rows[e.Row.RowIndex][6].ToString();
                        //lblAmount.Text = ((amount / count) * Int32.Parse(comp_count)).ToString();
                        //tot = tot + Int32.Parse(lblAmount.Text);
                    }

                    //e.Row.Cells[3].Text = table.Rows[e.Row.RowIndex][7].ToString();
                    //TextBox txtremark = new TextBox();
                    TextBox txtremark = (e.Row.FindControl("txtcompanyRemark") as TextBox);
                    txtremark.Text = table.Rows[e.Row.RowIndex][7].ToString();
                    string st = table.Rows[e.Row.RowIndex][8].ToString();
                    ddlcompStatus.SelectedIndex = ddlcompStatus.Items.IndexOf(ddlcompStatus.Items.FindByText(st));

                }

            }

        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            log.Debug("btnProcess_Click()");
            try
            {
                flag = "Click";
                companyExpenseDetails(txtTraining.Text);
                viewExpenceCategoryDetails(txtTraining.Text);
                txtTrTotal.Text = readTotExistCost().ToString();
                viewPerPersonCost();
                isRecordAlreadyExist();

            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        protected void grdexpCategoryDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Debug("grdexpCategoryDetails_PageIndexChanging()");
            if (txtTraining.Text != "")
            {
                viewExpenceCategoryDetails(txtTraining.Text);
            }
        }

        protected void grdexpCategoryDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.grdexpCategoryDetails, "Select$" + e.Row.RowIndex.ToString()));
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle");
                    e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#CCCCCC'");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                }
            }
            catch (Exception ex)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, ex.Message, lblMessage);
            }
        }

        protected void grdexpCategoryDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("grdexpCategoryDetails_SelectedIndexChanged()");
            Errorhandler.ClearError(lblMessage);
            Errorhandler.ClearError(lblDetailMessage);
            btnDetailSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;

            int SelectedIndex = grdexpCategoryDetails.SelectedIndex;

            ddlcategory.SelectedValue = Server.HtmlDecode(grdexpCategoryDetails.Rows[SelectedIndex].Cells[0].Text.ToString());

            txtDescription.Text = Server.HtmlDecode(grdexpCategoryDetails.Rows[SelectedIndex].Cells[2].Text.ToString());
            txtRemark.Text = Server.HtmlDecode(grdexpCategoryDetails.Rows[SelectedIndex].Cells[4].Text.ToString());
            txtCost.Text = Server.HtmlDecode(grdexpCategoryDetails.Rows[SelectedIndex].Cells[3].Text.ToString());

            string status = Server.HtmlDecode(grdexpCategoryDetails.Rows[SelectedIndex].Cells[5].Text.ToString().Trim());
            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(status));

            hfrecordVal.Value = Server.HtmlDecode(grdexpCategoryDetails.Rows[SelectedIndex].Cells[6].Text.ToString());

            companyWiseExpenseDetails(hfrecordVal.Value, txtTraining.Text);

            //valiidateTotCost();
        }

        protected void btnDetailSave_Click(object sender, EventArgs e)
        {
            log.Debug("btnDetailSave_Click()");
            ExpectedExpenceDetails EED = new ExpectedExpenceDetails();
            DataTable tdCompDetails = new DataTable();

            try
            {
                string logUser = Session["KeyUSER_ID"].ToString();
                string traingId = txtTraining.Text;
                string trDescription = txtTrDescription.Text;
                string trStatus = ddlTrStatus.SelectedValue;
                string trRemark = txtTrRemark.Text;
                string perPerson = txtTrPerPersonCost.Text;
                string trTotCost = txtTrTotal.Text;

                //Expense Detail

                string expCategory = ddlcategory.SelectedValue;
                string expCategoryName = ddlcategory.SelectedItem.Text;
                string description = txtDescription.Text;
                string amount = txtCost.Text;
                string remark = txtRemark.Text;
                string status = ddlStatus.SelectedValue;


                //Company Expense Details
                readCompanyDetails();
                tdCompDetails = (DataTable)Session["expenseBucket"];
                //valiidateTotCost();
                if (txtCost.Text != "")
                {
                    if (tot != double.Parse(txtCost.Text))
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Company total cost not equal to estimated cost", lblDetailMessage);
                        return;
                    }
                    tot = 0;
                }


                if (btnDetailSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
                {
                    if (txtTrDescription.Text == "")
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Training descriptiton is required.", lblMessage);
                        return;
                    }

                    if (ddlTrStatus.SelectedIndex == 0)
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, "Training status is required.", lblMessage);
                        return;
                    }

                    //Insert data to Expected expense detail + Company Expense
                    Boolean isSuccess = EED.InsertDetails(traingId, trDescription, trStatus, trRemark, perPerson, trTotCost, tdCompDetails, expCategory, description, remark, amount, status, logUser);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblDetailMessage);
                }
                else if (btnDetailSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
                {
                    string recId = hfrecordVal.Value;
                    string expId = EED.isExistTraining(txtTraining.Text);
                    //Update data to Expected expense detail + Company Expense
                    Boolean isUpdate = EED.UpdateDetails(expId, traingId, trDescription, trStatus, trRemark, perPerson, trTotCost, recId, expCategory, description, amount, remark, status, logUser, tdCompDetails);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblDetailMessage);
                }
                viewExpenceCategoryDetails(txtTraining.Text);
                //to do this befor save data to data base
                txtTrTotal.Text = readTotExistCost().ToString();
                viewPerPersonCost();

                clearExpenceDetails();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnDetailClear_Click(object sender, EventArgs e)
        {
            try
            {
                clearAllTrainingDetails();
                clearExpenceDetails();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //protected void btnTrSave_Click(object sender, EventArgs e)
        //{
        //    log.Debug("btnTrSave_Click()");
        //    ExpectedExpenceDetails EED = new ExpectedExpenceDetails();
        //    try
        //    {
        //        string logUser = Session["KeyUSER_ID"].ToString();
        //        string traingId = txtTraining.Text;
        //        string trDescription = txtTrDescription.Text;
        //        string trStatus = ddlTrStatus.SelectedValue;
        //        string trRemark = txtTrRemark.Text;
        //        string perPerson = txtTrPerPersonCost.Text;
        //        string trTotCost = txtTrTotal.Text;

        //        if (btnTrSave.Text == Constants.CON_SAVE_BUTTON_TEXT)
        //        {
        //            Boolean isSuccess = EED.Insert(traingId, trDescription, trStatus, trRemark, perPerson, trTotCost, logUser);
        //            Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved", lblMessage);
        //        }
        //        else if (btnTrSave.Text == Constants.CON_UPDATE_BUTTON_TEXT)
        //        {
        //            string expId = EED.isExistTraining(txtTraining.Text);
        //            Boolean isSuccess = EED.Update(expId, traingId, trDescription, trStatus, trRemark, perPerson, trTotCost, logUser);
        //            Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated", lblMessage);
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //protected void btnTrClear_Click(object sender, EventArgs e)
        //{
        //    log.Debug("btnTrClear_Click()");
        //}

        public void fillStatus(DropDownList ddl)
        {
            log.Debug("fillStatus()");
            try
            {
                ddl.Items.Insert(0, new ListItem("", ""));
                ddl.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
                ddl.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void companyExpenseDetails(string trId)
        {
            log.Debug("companyExpenseDetails()");
            ExpectedExpenceDetails EED = new ExpectedExpenceDetails();
            DataTable table = new DataTable();

            try
            {
                table = EED.getExpectedExpenses(trId);
                grdCompanywiseExpence.DataSource = table;
                grdCompanywiseExpence.DataBind();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void createExpenseBucket()
        {
            DataTable expenseBucket = new DataTable();

            expenseBucket.Columns.Add("COMPANY_ID", typeof(string));
            expenseBucket.Columns.Add("COMP_NAME", typeof(string));
            expenseBucket.Columns.Add("PLANNED_PARTICIPANTS", typeof(string));
            expenseBucket.Columns.Add("REMARK", typeof(string));
            expenseBucket.Columns.Add("AMOUNT", typeof(string));
            expenseBucket.Columns.Add("STATUS_CODE", typeof(string));

            Session["expenseBucket"] = expenseBucket;
        }

        public void allTrainingCategory()
        {
            log.Debug("allTrainingCategory()");
            DataTable table = new DataTable();
            ExpectedExpenceDetails EEDH = new ExpectedExpenceDetails();
            try
            {
                table = EEDH.getAllExpCategory();
                ddlcategory.Items.Add(new ListItem("", ""));

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string text = table.Rows[i]["CATEGORY_NAME"].ToString();
                    string value = table.Rows[i]["EXPENSE_CATEGORY_ID"].ToString();
                    ddlcategory.Items.Add(new ListItem(text, value));
                }
            }
            catch (Exception ex)
            {
                CommonVariables.MESSAGE_TEXT = ex.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                EEDH = null;
                table.Dispose();
            }
        }

        public int participantCount()
        {
            int count = 0;
            ExpectedExpenceDetails EED = new ExpectedExpenceDetails();
            DataTable table = new DataTable();

            try
            {
                table = EED.getExpectedExpenses(txtTraining.Text);
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string empCount = table.Rows[i]["PLANNED_PARTICIPANTS"].ToString();
                    count = count + Int32.Parse(empCount);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                EED = null;
                table.Dispose();
            }
            return count;
        }

        public void viewExpenceCategoryDetails(string trId)
        {
            log.Debug("viewExpenceCategoryDetails()");
            ExpectedExpenceDetails EED = new ExpectedExpenceDetails();
            DataTable table = new DataTable();

            try
            {
                table = EED.getExpectedExpensesDetails(trId);
                grdexpCategoryDetails.DataSource = table;
                grdexpCategoryDetails.DataBind();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                table.Dispose();
                EED = null;
            }
        }

        public double readTotExistCost()
        {
            log.Debug("readTotExistCost()");
            double totAmount = 0;

            try
            {
                for (int i = 0; i < grdexpCategoryDetails.Rows.Count; i++)
                {
                    string amount = grdexpCategoryDetails.Rows[i].Cells[3].Text.ToString();
                    totAmount = totAmount + Double.Parse(amount);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return totAmount;
        }

        public void viewPerPersonCost()
        {
            try
            {
                log.Debug("viewPerPersonCost()");
                int count = participantCount();

                if (count > 0)
                {
                    txtTrPerPersonCost.Text = (Int32.Parse(txtTrTotal.Text) / count).ToString();
                }
                else
                {
                    txtTrPerPersonCost.Text = "0";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void isRecordAlreadyExist()
        {
            log.Debug("isRecordAlreadyExist()");
            ExpectedExpenceDetails EED = new ExpectedExpenceDetails();
            try
            {
                string isExist = EED.isExistTraining(txtTraining.Text);

                if (isExist != "")
                {
                    btnDetailSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                    viewExpenceDetails();
                }
                else
                {
                    txtTrDescription.Text = "";
                    ddlTrStatus.SelectedIndex = 0;
                    txtTrRemark.Text = "";
                    btnDetailSave.Text = Constants.CON_SAVE_BUTTON_TEXT;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                EED = null;
            }
        }

        public void viewExpenceDetails()
        {
            log.Debug("viewExpenceDetails()");
            ExpectedExpenceDetails EED = new ExpectedExpenceDetails();
            DataTable table = new DataTable();

            try
            {
                table = EED.getExpenceDetails(txtTraining.Text);
                txtTrDescription.Text = table.Rows[0]["DESCRIPTION"].ToString();
                txtTrRemark.Text = table.Rows[0]["REMARKS"].ToString();
                ddlTrStatus.SelectedValue = table.Rows[0]["STATUS_CODE"].ToString();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                table.Dispose();
                EED = null;
            }
        }

        public void readCompanyDetails()
        {

            try
            {
                log.Debug("readCompanyDetails()");
                //Session["expenseBucket"] = null;
                DataTable expenseBucket = (DataTable)Session["expenseBucket"];
                expenseBucket.Rows.Clear();
                for (int i = 0; i < grdCompanywiseExpence.Rows.Count; i++)
                {
                    DataRow dr = expenseBucket.NewRow();
                    dr["COMPANY_ID"] = grdCompanywiseExpence.Rows[i].Cells[0].Text;
                    dr["COMP_NAME"] = grdCompanywiseExpence.Rows[i].Cells[1].Text;
                    dr["PLANNED_PARTICIPANTS"] = grdCompanywiseExpence.Rows[i].Cells[2].Text;

                    TextBox txt = (TextBox)grdCompanywiseExpence.Rows[i].Cells[3].FindControl("txtcompanyRemark");
                    dr["REMARK"] = txt.Text;

                    TextBox lbl = (TextBox)grdCompanywiseExpence.Rows[i].Cells[4].FindControl("lblcmpAmount");
                    dr["AMOUNT"] = lbl.Text;
                    tot = tot + double.Parse(lbl.Text);

                    DropDownList ddl = (DropDownList)grdCompanywiseExpence.Rows[i].Cells[5].FindControl("ddlcompStatus");
                    dr["STATUS_CODE"] = ddl.SelectedValue;
                    expenseBucket.Rows.Add(dr);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void companyWiseExpenseDetails(string recId, string trainingId)
        {
            log.Debug("companyWiseExpenseDetails()");
            ExpectedExpenceDetails EED = new ExpectedExpenceDetails();
            DataTable dt = new DataTable();

            try
            {
                dt = EED.getCompanyWiseExpenceDetails(recId, trainingId);
                grdCompanywiseExpence.DataSource = dt;
                grdCompanywiseExpence.DataBind();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                EED = null;
                dt.Dispose();
            }
        }


        //public void bindCompanyDetails(string trId)
        //{

        //    ExpectedExpenceDetails EED = new ExpectedExpenceDetails();
        //    DataTable table = new DataTable();

        //    try
        //    {
        //        int count = participantCount();

        //        table = EED.getExpectedExpenses(trId);
        //        for (int i = 0; i < table.Rows.Count; i++)
        //        {
        //            DropDownList ddlcompStatus = (grdCompanywiseExpence.FindControl("ddlcompStatus") as DropDownList);

        //            ddlcompStatus.Items.Insert(0, new ListItem("", ""));
        //            ddlcompStatus.Items.Insert(1, new ListItem(Constants.STATUS_ACTIVE_TAG, Constants.CON_ACTIVE_STATUS));
        //            ddlcompStatus.Items.Insert(2, new ListItem(Constants.STATUS_INACTIVE_TAG, Constants.CON_INACTIVE_STATUS));


        //            if (count > 0 && txtCost.Text != "")
        //            {
        //                string comp_count = Convert.ToString(DataBinder.Eval(grdCompanywiseExpence.Rows[i], "PLANNED_PARTICIPANTS"));

        //                double amount = Double.Parse(txtCost.Text);
        //                Label lblAmount = (grdCompanywiseExpence.Rows[i].FindControl("lblcmpAmount") as Label);
        //                lblAmount.Text = ((amount / count) * Int32.Parse(comp_count)).ToString();

        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public void clearAllTrainingDetails()
        {
            log.Debug("clearAllTrainingDetails()");
            ddlcategory.SelectedIndex = 0;
            txtDescription.Text = "";
            txtRemark.Text = "";
            txtCost.Text = "";
            ddlStatus.Text = "";
            btnDetailSave.Text = Constants.CON_SAVE_BUTTON_TEXT;

            grdCompanywiseExpence.DataSource = null;
            grdCompanywiseExpence.DataBind();

            grdexpCategoryDetails.DataSource = null;
            grdexpCategoryDetails.DataBind();

            lblTrainingName.Text = "";
            lblProgramName.Text = "";
        }

        public void disableFields(bool status)
        {
            log.Debug("disableFields()");
            ddlcategory.Enabled = status;
            txtDescription.Enabled = status;
            txtRemark.Enabled = status;
            txtCost.Enabled = status;
            ddlStatus.Enabled = status;
            btnProcess.Enabled = status;
            btnDetailSave.Enabled = status;
            btnDetailClear.Enabled = status;
        }

        public void clearExpenceDetails()
        {
            try
            {
                ddlcategory.SelectedIndex = 0;
                txtDescription.Text = "";
                txtRemark.Text = "";
                txtCost.Text = "";
                ddlStatus.SelectedIndex = 0;

                grdCompanywiseExpence.DataSource = null;
                grdCompanywiseExpence.DataBind();

                flag = "";
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void valiidateTotCost()
        {
            try
            {
                if (txtCost.Text != "")
                {
                    if (tot != double.Parse(txtCost.Text))
                    {
                        Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Company total cost not equal to estimated cost", lblDetailMessage);
                        tot = 0;
                        return;
                    }
                    else
                    {
                        tot = 0;


                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}