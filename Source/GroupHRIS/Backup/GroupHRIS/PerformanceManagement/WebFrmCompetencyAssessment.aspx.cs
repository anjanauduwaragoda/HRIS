using System;
using System.Web;
using System.Web.UI.WebControls;
using DataHandler.PerformanceManagement;
using System.Data;
using DataHandler.Userlogin;
using GroupHRIS.Utility;
using Common;
using DataHandler.Utility;

namespace GroupHRIS.PerformanceManagement
{
    public partial class WebFrmCompetencyAssessment : System.Web.UI.Page
    {
        DataTable dtRatings = new DataTable();
        DataTable dtexistData = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {

            ViewEmployeeCompetencyProfileDataHandler oVCPDataHandler = new ViewEmployeeCompetencyProfileDataHandler();
            PasswordHandler cripto = new PasswordHandler();

            string assmtId = "";
            string assmtYear = "";

            createBucket();
            getProfileCompetencies();
            getEmpDetails();

            if (!IsPostBack)
            {

                try
                {
                    string empId = cripto.Decrypt(Request.QueryString["employeeId"]);
                    assmtId = cripto.Decrypt(Request.QueryString["assmtId"]);
                    assmtYear = cripto.Decrypt(Request.QueryString["year"]);

                    //string compProId = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[4].Text).ToString();
                    String isAssmtTokenExist = oVCPDataHandler.getAssessmentToken(assmtId, assmtYear, empId);

                    if (isAssmtTokenExist != "")
                    {
                        btnSave.Text = Constants.CON_UPDATE_BUTTON_TEXT;
                        LoadExistData(assmtId, empId, assmtYear, isAssmtTokenExist);
                        isFinalised(assmtId, empId, assmtYear);
                    }
                    else
                    {
                        LoadData(assmtId, empId, assmtYear);
                    }
                    DisplayChart();
                }
                catch (Exception exp)
                {
                    CommonVariables.MESSAGE_TEXT = exp.Message;
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ViewEmployeeCompetencyProfileDataHandler oVCPDataHandler = new ViewEmployeeCompetencyProfileDataHandler();
            PasswordHandler cripto = new PasswordHandler();

            string assmtId = "";
            string assmtYear = "";

            try
            {
                string empId = cripto.Decrypt(Request.QueryString["employeeId"]);
                assmtId = cripto.Decrypt(Request.QueryString["assmtId"]);
                assmtYear = cripto.Decrypt(Request.QueryString["year"]);
                string compProId = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[5].Text).ToString();

                String isAssmtTokenExist = oVCPDataHandler.getAssessmentToken(assmtId, assmtYear, empId);

                readCompetencies();
                DataTable dtAnswers = (DataTable)Session["competency"];

                bool isSelect = isFill();
                if (isSelect == false)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Select competencies.", lblMessage);
                    return;
                }

                if (isAssmtTokenExist != "")
                {
                    //Update existing data
                    Boolean isSuccess = oVCPDataHandler.InsertifExist(isAssmtTokenExist, dtAnswers);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully updated.", lblMessage);
                }
                else
                {
                    //Initialy Save data
                    Boolean isSuccess = oVCPDataHandler.Insert(compProId, assmtId, empId, assmtYear, dtAnswers);
                    Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Record(s) successfully saved.", lblMessage);
                }
                //isFinalised(assmtId, empId, assmtYear);

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oVCPDataHandler = null;
                cripto = null;
            }
        }

        protected void btnFinalized_Click(object sender, EventArgs e)
        {
            ViewEmployeeCompetencyProfileDataHandler oVCPDataHandler = new ViewEmployeeCompetencyProfileDataHandler();
            PasswordHandler cripto = new PasswordHandler();
            UtilsDataHandler oUtilsDataHandler = new UtilsDataHandler();

            string assmtId = "";
            string assmtYear = "";
            try
            {
                double TotalEmployeeScore = 0.0;
                int count = 0;

                readCompetencies();
                string empId = cripto.Decrypt(Request.QueryString["employeeId"]);
                DataTable dtAnswers = (DataTable)Session["competency"];
                string logUser = Session["KeyUSER_ID"].ToString();
                assmtId = cripto.Decrypt(Request.QueryString["assmtId"]);
                assmtYear = cripto.Decrypt(Request.QueryString["year"]);
                string KeyEMPLOYEE_ID = (string)(Session["KeyEMPLOYEE_ID"]);
                string AssessmentToken = null;

                AssessmentToken = oVCPDataHandler.getAssessmentToken(assmtId, assmtYear, empId);


                for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                {
                    string CompetencyID = String.Empty;

                    DropDownList ddlCompetencyRating = (grdvCompetencies.Rows[i].FindControl("ddlEmployeeRating") as DropDownList);

                    CompetencyID = HttpUtility.HtmlDecode(grdvCompetencies.Rows[i].Cells[1].Text).ToString();
                    if (ddlCompetencyRating.SelectedIndex > 0)
                    {
                        count = count + 1;
                        double weight = 0.0;
                        if (Double.TryParse(ddlCompetencyRating.SelectedValue.ToString(), out weight))
                        {
                            TotalEmployeeScore += weight;
                        }
                        else
                        {
                            TotalEmployeeScore += 0.0;
                        }
                    }
                    else
                    {
                        TotalEmployeeScore += 0.0;
                    }
                }

                if (count != grdvCompetencies.Rows.Count)
                {
                    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Select rating(s) for all competencies.", lblMessage);
                    return;
                }

                if (AssessmentToken != null && AssessmentToken != "")
                {

                    Boolean isSuccessSave = oVCPDataHandler.InsertifExist(AssessmentToken, dtAnswers);
                    //Boolean isSuccess = oVCPDataHandler.FinalizedifExist(AssessmentToken, TotalEmployeeScore, logUser);

                    ////Update Assessment Status
                    //Boolean isFinalized = oUtilsDataHandler.updateAssessmentStatus(assmtId, KeyEMPLOYEE_ID, assmtYear);
                    //Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully finalized.", lblMessage);
                    //isFinalised(assmtId, empId, assmtYear);
                }
                else
                {
                    string compProId = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[5].Text).ToString();

                    //Initialy Save data
                    Boolean isSuccessInit = oVCPDataHandler.Insert(compProId, assmtId, empId, assmtYear, dtAnswers);
                    AssessmentToken = oVCPDataHandler.getAssessmentToken(assmtId, assmtYear, empId);
                    //Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Befor finalized need to save records.", lblMessage);

                }

                Boolean isSuccess = oVCPDataHandler.FinalizedifExist(AssessmentToken, TotalEmployeeScore, logUser);

                //Update Assessment Status
                Boolean isFinalized = oUtilsDataHandler.updateAssessmentStatus(assmtId, KeyEMPLOYEE_ID, assmtYear);
                isFinalised(assmtId, empId, assmtYear);
                Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, "Successfully finalized.", lblMessage);

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oVCPDataHandler = null;
                cripto = null;
            }
        }

        public void getProfileCompetencies()
        {
            ViewEmployeeCompetencyProfileDataHandler oViewEmployeeCompetencyProfileDataHandler = new ViewEmployeeCompetencyProfileDataHandler();

            try
            {
                string TableStringFactors = "";

                string userRole = oViewEmployeeCompetencyProfileDataHandler.getEmpRole(Session["KeyEMPLOYEE_ID"].ToString());
                DataTable dt = oViewEmployeeCompetencyProfileDataHandler.getProfileCompetencies(userRole);
                TableStringFactors += @"<style>
                                        table, th, td {
                                            border: 1px solid black;
                                            border-collapse: collapse;
                                            
                                        }
                                        .beta table,.beta th,.beta td {
                                            border: none;
                                        }
                                        </style>";

                TableStringFactors += @"<table style=""border:none;"" class= ""beta"" width = ""350px""><tr><td></td></tr> ";

                TableStringFactors += @"<tr><th >" + "PROFICIENCY" + "</th><th>" + "WEIGHT" + "</th><th>" + "DESCRIPTION" + "</th></tr>";

                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    string proficiency = dt.Rows[x][9].ToString();
                    string weight = dt.Rows[x][10].ToString();
                    string description = dt.Rows[x][11].ToString();

                    TableStringFactors += @"<tr><td>" + proficiency + "</td><td>" + weight + "</td><td>" + description + "</td></tr>";

                }

                TableStringFactors += @"</table>
                            ";

                lblCriteria.Text = string.Empty;
                lblCriteria.Text = HttpUtility.HtmlDecode(TableStringFactors);


                //createTable(userRole);


            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oViewEmployeeCompetencyProfileDataHandler = null;
            }
        }

        public void getEmpDetails()
        {
            ViewEmployeeCompetencyProfileDataHandler oViewEmployeeCompetencyProfileDataHandler = new ViewEmployeeCompetencyProfileDataHandler();

            try
            {
                string TableString = "";
                DataTable dt = oViewEmployeeCompetencyProfileDataHandler.getempDetails(Session["KeyEMPLOYEE_ID"].ToString());

                string empId = dt.Rows[0]["EMPLOYEE_ID"].ToString();
                string name = dt.Rows[0]["FULL_NAME"].ToString();
                string role = dt.Rows[0]["ROLE_NAME"].ToString();
                string company = dt.Rows[0]["COMP_NAME"].ToString();

                TableString += @"<table class= ""beta"" width = ""350px""><tr><td></td><td></td></tr><br/> ";
                TableString += @"<tr><td  Style=""text-align:right;""></td><td  Style=""text-align:left;""></td></tr>";
                TableString += @"<tr><th Style=""text-align:center;"" colspan = ""2""> " + name + "</th></tr>";

                TableString += @"<tr><td Style=""text-align:center;"" colspan = ""2"">Role : " + role + "</td></tr>";
                TableString += @"<tr><td Style=""text-align:center;"" colspan = ""2"">" + company + "</td></tr>";

                TableString += @"<tr><td  Style=""text-align:right;""></td><td  Style=""text-align:left;""></td></tr>";
                //TableString += @"<tr><td Style=""text-align:right;"">Role : </td><td  Style=""text-align:left;"">" + role + "</td></tr>";
                //TableString += @"<tr><td Style=""text-align:right;"">Company : </td><td  Style=""text-align:left;"">" + company + "</td></tr>";
                TableString += @"</table>";

                lblDetails.Text = string.Empty;
                lblDetails.Text = HttpUtility.HtmlDecode(TableString);


            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        //public void fillProLevel()
        //{
        //    DataTable table = new DataTable();
        //    ViewEmployeeCompetencyProfileDataHandler oViewEmployeeCompetencyProfileDataHandler = new ViewEmployeeCompetencyProfileDataHandler();
        //    string userRole = oViewEmployeeCompetencyProfileDataHandler.getEmpRole(Session["KeyEMPLOYEE_ID"].ToString());

        //    table = oViewEmployeeCompetencyProfileDataHandler.getCompetencieLevel(userRole).Copy();
        //    DropDownList ddlLevel = new DropDownList();
        //    ddlLevel.Items.Add(new ListItem("", ""));

        //    for (int i = 0; i < table.Rows.Count; i++)
        //    {
        //        string text = table.Rows[i]["RATING"].ToString();
        //        string value = table.Rows[i]["WEIGHT"].ToString();
        //        ddlLevel.Items.Add(new ListItem(text, value));
        //    }
        //}

        //public void createTable(string userRole)
        //{
        //    ViewEmployeeCompetencyProfileDataHandler oViewEmployeeCompetencyProfileDataHandler = new ViewEmployeeCompetencyProfileDataHandler();

        //    try
        //    {
        //        Table table = new Table();
        //        table.ID = "Table1";
        //        table.Width = 900;
        //        lbltbl.Controls.Add(table);
        //        //table.BackImageUrl = "../Images/Common/pagerow.png";

        //        DataTable dtCompetencyBucket = (DataTable)Session["competencyBucket"];

        //        DataTable dtCompitence = oViewEmployeeCompetencyProfileDataHandler.getExpectedProfileCompetencieRatings(userRole);

        //        //TableRow drowx = new TableRow();
        //        //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
        //        //img.ImageUrl = "../Images/Common/pagerow.png";
        //        //drowx.Controls.Add(img);
        //        //table.Rows.Add(drowx);

        //        TableRow drow = new TableRow();
        //        drow.BorderWidth = 1;

        //        TableCell dcell1 = new TableCell();
        //        TableCell dcell2 = new TableCell();
        //        TableCell dcell3 = new TableCell();
        //        TableCell dcell4 = new TableCell();

        //        dcell1.Text = " FACTORES AND BEST STANDED/SCENARIO ";
        //        drow.Cells.Add(dcell1);
        //        dcell4.Text = " DESCROPTION ";
        //        drow.Cells.Add(dcell4);
        //        dcell2.Text = " EXPECTED RATING ";
        //        drow.Cells.Add(dcell2);
        //        dcell3.Text = " EMPLOYEE RATING ";
        //        drow.Cells.Add(dcell3);
        //        table.Rows.Add(drow);

        //        DataTable dtTable = oViewEmployeeCompetencyProfileDataHandler.getCompetencieLevel(userRole).Copy();


        //        DropDownList ddlList = new DropDownList();

        //        ddlList.Items.Clear();
        //        ddlList.Items.Add(new ListItem("", ""));
        //        ddlList.Width = 100;

        //        for (int i = 0; i < dtTable.Rows.Count; i++)
        //        {
        //            string text = dtTable.Rows[i]["RATING"].ToString();
        //            string value = dtTable.Rows[i]["WEIGHT"].ToString();
        //            ddlList.Items.Add(new ListItem(text, value));
        //        }


        //        for (int x = 0; x < dtCompitence.Rows.Count; x++)
        //        {
        //            DataRow dtrow = dtCompetencyBucket.NewRow();

        //            TableRow row = new TableRow();
        //            //ddlList.ID = "ddlList_" + x;

        //            string compitencyId = dtCompitence.Rows[x][1].ToString();
        //            string compitencyName = dtCompitence.Rows[x][2].ToString();
        //            string expRating = dtCompitence.Rows[x][3].ToString();
        //            string descript = dtCompitence.Rows[x][5].ToString();
        //            Session["competencyProId"] = dtCompitence.Rows[x][6].ToString(); ;

        //            TableCell cell1 = new TableCell();
        //            TableCell cell2 = new TableCell();
        //            TableCell cell3 = new TableCell();
        //            TableCell cell4 = new TableCell();

        //            cell1.Text = (x+1) + ". " + compitencyName;
        //            row.Cells.Add(cell1);

        //            cell4.Text = descript;
        //            row.Cells.Add(cell4);

        //            cell2.Text = expRating;
        //            row.Cells.Add(cell2);

        //            cell3.Controls.Add(ddlList);
        //            row.Cells.Add(cell3);

        //            table.Rows.Add(row);


        //            dtrow["COMPETENCY_ID"] = compitencyId;
        //            dtrow["EMPLOYEE_RATING"] = ddlList.ID;
        //            dtrow["EMPLOYEE_WEIGHT"] = ddlList.ID;

        //            dtCompetencyBucket.Rows.Add(dtrow);

        //        }

        //        Session["competencyBucket"] = dtCompetencyBucket;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {
        //        oViewEmployeeCompetencyProfileDataHandler = null;
        //    }
        //}

        //public void createDataBucket()
        //{
        //    DataTable questionBucket = new DataTable();
        //    questionBucket.Columns.Add("COMPETENCY_ID", typeof(string));
        //    questionBucket.Columns.Add("EMPLOYEE_RATING", typeof(string));
        //    questionBucket.Columns.Add("EMPLOYEE_WEIGHT", typeof(string));

        //    Session["competencyBucket"] = questionBucket;
        //}

        //public void readSelectedCompetencies()
        //{
        //    try
        //    {

        //        DataTable dtEmpCompetencyTable = (DataTable)Session["competencyBucket"];

        //        DataTable dtC = (DataTable)Session["competency"];

        //        for (int i = 0; i < dtEmpCompetencyTable.Rows.Count; i++)
        //        {
        //            string compId = dtEmpCompetencyTable.Rows[i]["COMPETENCY_ID"].ToString();
        //            string ddlId = dtEmpCompetencyTable.Rows[i]["EMPLOYEE_RATING"].ToString();

        //            DropDownList ddlList = new DropDownList();
        //            string controlerId = ddlId;
        //            ddlList = (DropDownList)FindControl(controlerId);

        //            string rating = ddlList.SelectedItem.ToString();
        //            string weight = ddlList.SelectedValue.ToString();

        //            DataRow dtrow = dtC.NewRow();
        //            dtrow["COMPETENCY_ID"] = compId;
        //            dtrow["EMPLOYEE_RATING"] = rating;
        //            dtrow["EMPLOYEE_WEIGHT"] = weight;

        //            dtC.Rows.Add(dtrow);
        //        }
        //        Session["competency"] = dtC;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public void createBucket()
        {
            DataTable questionBucket = new DataTable();
            questionBucket.Columns.Add("COMPETENCY_ID", typeof(string));
            questionBucket.Columns.Add("EMPLOYEE_RATING", typeof(string));
            questionBucket.Columns.Add("EMPLOYEE_WEIGHT", typeof(string));

            Session["competency"] = questionBucket;
        }

        //public void viewCompetency()
        //{
        //    ViewEmployeeCompetencyProfileDataHandler oVCPDataHandler = new ViewEmployeeCompetencyProfileDataHandler();
        //    PasswordHandler cripto = new PasswordHandler();

        //    string assmtId = "";
        //    string assmtYear = "";

        //    try
        //    {
        //        string compProId = Session["competencyProId"].ToString();
        //        string empId = cripto.Decrypt(Request.QueryString["employeeId"]);
        //        assmtId = cripto.Decrypt(Request.QueryString["assmtId"]);
        //        assmtYear = cripto.Decrypt(Request.QueryString["year"]);

        //        String isAssmtTokenExist = oVCPDataHandler.getAssessmentToken(compProId, assmtId, assmtYear, empId);
        //        DataTable dt = oVCPDataHandler.getAssessmentDetails(isAssmtTokenExist);
        //        String isFStatus = oVCPDataHandler.getAssessmentStatus(compProId, assmtId, assmtYear, empId);

        //        DataTable table = new DataTable();
        //        ViewEmployeeCompetencyProfileDataHandler oViewEmployeeCompetencyProfileDataHandler = new ViewEmployeeCompetencyProfileDataHandler();
        //        string userRole = oViewEmployeeCompetencyProfileDataHandler.getEmpRole(Session["KeyEMPLOYEE_ID"].ToString());

        //        table = oViewEmployeeCompetencyProfileDataHandler.getCompetencieLevel(userRole).Copy();
        //        DropDownList ddlLevel = new DropDownList();
        //        ddlLevel.Items.Add(new ListItem("", ""));

        //        for (int i = 0; i < table.Rows.Count; i++)
        //        {
        //            string controlerIdx = "ddlList_" + i;
        //            ddlLevel.ID = controlerIdx;
        //            string text = table.Rows[i]["RATING"].ToString();
        //            string value = table.Rows[i]["WEIGHT"].ToString();
        //            ddlLevel.Items.Add(new ListItem(text, value));
        //        }

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            string compId = dt.Rows[i]["COMPETENCY_ID"].ToString();
        //            string empRating = dt.Rows[i]["EMPLOYEE_RATING"].ToString();
        //            string empWeight = dt.Rows[i]["EMPLOYEE_WEIGHT"].ToString();


        //            if (ddlLevel.ID == "ddlList_" + i)
        //            {
        //                ddlLevel.SelectedIndex = ddlLevel.Items.IndexOf(ddlLevel.Items.FindByText(empRating));
        //            }



        //            //string controlerId = "ddlList_" + i;
        //            //ddlLevel = (DropDownList)FindControl(controlerId);

        //            //ddlLevel.SelectedItem.Text = empRating;

        //            if (isFStatus == Constants.ASSESSNEMT_SUBORDINATE_FINALIZED_STATUS)
        //            {
        //                ddlLevel.Enabled = false;
        //                btnSave.Enabled = false;
        //                btnFinalized.Enabled = false;
        //            }
        //        }
        //        Session["competency"] = dt;

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public void LoadData(string assId, string empId, string assYer)
        {
            ViewEmployeeCompetencyProfileDataHandler oVCPDataHandler = new ViewEmployeeCompetencyProfileDataHandler();
            DataTable dtCompetencies = new DataTable();

            try
            {

                string userRole = oVCPDataHandler.getEmpRole(Session["KeyEMPLOYEE_ID"].ToString());
                LoadCompetencyRating(userRole);
                grdvCompetencies.DataSource = oVCPDataHandler.Populate(userRole, assId, empId, assYer);
                grdvCompetencies.DataBind();

                if (grdvCompetencies.Rows.Count > 0)
                {
                    string compProId = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[5].Text).ToString();
                    //isFinalised(assId, empId, assYer);
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oVCPDataHandler = null;
            }
        }

        public void LoadExistData(string assId, string empId, string assYer, string token)
        {
            ViewEmployeeCompetencyProfileDataHandler oVCPDataHandler = new ViewEmployeeCompetencyProfileDataHandler();

            try
            {
                string userRole = oVCPDataHandler.getEmpRole(Session["KeyEMPLOYEE_ID"].ToString());
                LoadCompetencyRating(userRole);

                dtexistData = oVCPDataHandler.populateIfExist(userRole, token).Copy();

                grdvCompetencies.DataSource = dtexistData;
                grdvCompetencies.DataBind();

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oVCPDataHandler = null;
            }
        }

        public void LoadCompetencyRating(string assmentId)
        {
            ViewEmployeeCompetencyProfileDataHandler oVCPDataHandler = new ViewEmployeeCompetencyProfileDataHandler();


            try
            {
                dtRatings = oVCPDataHandler.PopulateRatings(assmentId);

            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oVCPDataHandler = null;
            }
        }

        protected void grdvCompetencies_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlCompetencyRating = (e.Row.FindControl("lblMcq") as DropDownList);

                    ddlCompetencyRating.Items.Add(new ListItem("", ""));

                    for (int i = 0; i < dtRatings.Rows.Count; i++)
                    {
                        string text = dtRatings.Rows[i]["RATING"].ToString();
                        string value = dtRatings.Rows[i]["WEIGHT"].ToString();

                        ddlCompetencyRating.Items.Add(new ListItem(text, value));
                    }

                    //


                    if (dtexistData.Rows.Count > 0)
                    {
                        //string employeeWeight = HttpUtility.HtmlDecode(e.Row.Cells[2].Text).ToString().Trim();

                        int rowIndex = e.Row.RowIndex;
                        //GridViewRow row = grdvCompetencies.Rows[rowIndex];

                        string employeeWeight = dtexistData.Rows[rowIndex][7].ToString();

                        if (employeeWeight != String.Empty)
                        {
                            ddlCompetencyRating.SelectedIndex = ddlCompetencyRating.Items.IndexOf(ddlCompetencyRating.Items.FindByText(employeeWeight));
                        }
                    }
                    else
                    {
                        //e.Row.Cells[5].Visible = false;
                        grdvCompetencies.Columns[5].Visible = false;
                    }
                }
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
        }

        public void isFinalised(string assId, string empId, string assYer)
        {

            ViewEmployeeCompetencyProfileDataHandler oVCPDataHandler = new ViewEmployeeCompetencyProfileDataHandler();
            try
            {
                if (grdvCompetencies.Rows.Count > 0)
                {
                    string compProId = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[4].Text).ToString();
                    String isFStatus = oVCPDataHandler.getAssessmentStatus(compProId, assId, assYer, empId);

                    if (isFStatus != Constants.ASSESSNEMT_ACTIVE_STATUS)
                    {
                        for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                        {
                            var ddl = grdvCompetencies.Rows[i].FindControl("ddlEmployeeRating") as DropDownList;
                            ddl.Enabled = false;
                        }

                        btnSave.Enabled = false;
                        btnFinalized.Enabled = false;
                    }
                }
                //else
                //{
                //    Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Befor finalized need to save records.", lblMessage);
                //    return;
                //}
            }
            catch (Exception exp)
            {
                CommonVariables.MESSAGE_TEXT = exp.Message;
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, CommonVariables.MESSAGE_TEXT, lblMessage);
            }
            finally
            {
                oVCPDataHandler = null;
            }
        }

        public void readCompetencies()
        {
            ViewEmployeeCompetencyProfileDataHandler oVCPDataHandler = new ViewEmployeeCompetencyProfileDataHandler();
            PasswordHandler cripto = new PasswordHandler();

            string assmtId = "";
            string assmtYear = "";
            try
            {
                string empId = cripto.Decrypt(Request.QueryString["employeeId"]);
                assmtId = cripto.Decrypt(Request.QueryString["assmtId"]);
                assmtYear = cripto.Decrypt(Request.QueryString["year"]);
                string compProId = HttpUtility.HtmlDecode(grdvCompetencies.Rows[0].Cells[4].Text).ToString();

                DataTable dtAnswers = (DataTable)Session["competency"];

                String isAssmtTokenExist = oVCPDataHandler.getAssessmentToken(assmtId, assmtYear, empId);

                for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                {

                    string CompetencyID = String.Empty;
                    string employeeRating = String.Empty;
                    string employeeWeight = String.Empty;

                    DropDownList ddlCompetencyRating = (grdvCompetencies.Rows[i].FindControl("ddlEmployeeRating") as DropDownList);

                    CompetencyID = HttpUtility.HtmlDecode(grdvCompetencies.Rows[i].Cells[0].Text).ToString();
                    if (ddlCompetencyRating.SelectedIndex > 0)
                    {
                        employeeRating = ddlCompetencyRating.SelectedItem.Text.Trim();
                        employeeWeight = ddlCompetencyRating.SelectedValue.Trim();
                    }

                    DataRow dtRow = dtAnswers.NewRow();

                    dtRow["COMPETENCY_ID"] = CompetencyID;
                    dtRow["EMPLOYEE_RATING"] = employeeRating;
                    dtRow["EMPLOYEE_WEIGHT"] = employeeWeight;

                    dtAnswers.Rows.Add(dtRow);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool isFill()
        {
            bool status = false;
            int count = 0;
            try
            {
                for (int i = 0; i < grdvCompetencies.Rows.Count; i++)
                {
                    string CompetencyID = String.Empty;

                    DropDownList ddlCompetencyRating = (grdvCompetencies.Rows[i].FindControl("ddlEmployeeRating") as DropDownList);

                    CompetencyID = HttpUtility.HtmlDecode(grdvCompetencies.Rows[i].Cells[1].Text).ToString();
                    if (ddlCompetencyRating.SelectedIndex > 0)
                    {
                        count = count + 1;
                    }
                }

                if (count > 0)
                {
                    status = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return status;
        }

        void DisplayChart()
        {
            PasswordHandler cripto = new PasswordHandler();
            SupervisorCompetencyAssessmentDataHandler SCADH = new SupervisorCompetencyAssessmentDataHandler();
            DataTable dtPreviousCompetencies = new DataTable();
            try
            {
                string empId = cripto.Decrypt(Request.QueryString["employeeId"]);
                dtPreviousCompetencies = SCADH.PopulatePreviousData(empId).Copy();
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
				                                    <li>You can partially evaluate competiences and save/update given ratings without evaluating all the competencies at the one step.  </li><br/><br/>
				                                    <li>After evaluating all the competencies you need to click finalize button to finalize evaluation process. After finalize you are not allow to modifies competencies.</li>
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
                throw ex;
            }
            finally
            {
                dtPreviousCompetencies.Dispose();
                SCADH = null;
                cripto = null;
            }
        }
    }
}