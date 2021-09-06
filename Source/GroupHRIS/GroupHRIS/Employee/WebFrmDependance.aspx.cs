using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DataHandler.Employee;
using System.Data;

namespace GroupHRIS.Employee
{
    public partial class WebFrmDependance : System.Web.UI.Page
    {
        void createDependentsDataTable()
        {
            DataTable dtDependents = new DataTable();
            dtDependents.Columns.Add("DEPENDANT_ID");
            dtDependents.Columns.Add("FULL_NAME");
            dtDependents.Columns.Add("NAME_WITH_INITIALS");
            dtDependents.Columns.Add("RELATIONSHIP_TO_EMPLOYEE");
            dtDependents.Columns.Add("GENDER");
            dtDependents.Columns.Add("DOB");
            dtDependents.Columns.Add("NIC");
            dtDependents.Columns.Add("OCCUPATION");
            dtDependents.Columns.Add("PLACE_OF_WORK");
            dtDependents.Columns.Add("CONTACT_NUMBER_MOBILE");
            dtDependents.Columns.Add("CONTACT_NUMBER_LAND");
            dtDependents.Columns.Add("IS_EMRGENCY_CONTACT");
            Session["dtDependents"] = dtDependents;
        }

        void loadFamilyMembers()
        {
            Dictionary<string, string> FamilyMembers = new Dictionary<string, string>();
            FamilyMembers = CommonUtils.FamilyMembers();

            ddlRelationshipToEmployee.Items.Add(new ListItem("", ""));

            foreach (KeyValuePair<string, string> entry in FamilyMembers)
            {
                string value = entry.Key;
                string text = entry.Value;
                ddlRelationshipToEmployee.Items.Add(new ListItem(text, value));
            }
        }

        void loadGender()
        {
            Dictionary<string, string> Gender = new Dictionary<string, string>();
            Gender.Add("M", "Male");
            Gender.Add("F", "Female");

            ddlGender.Items.Add(new ListItem("", ""));
            foreach (KeyValuePair<string, string> entry in Gender)
            {
                string value = entry.Key;
                string text = entry.Value;
                ddlGender.Items.Add(new ListItem(text, value));
            }
        }

        void PopulateDepentents()
        {
            EmployeeDependentDataHandler EPDDH = new EmployeeDependentDataHandler();
            DataTable dt = new DataTable();
            dt = EPDDH.PopulateDependents(txtemployee.Text.Trim()).Copy();

            Dictionary<string, string> FamilyMembers = new Dictionary<string, string>();
            FamilyMembers = CommonUtils.FamilyMembers();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string relationship = dt.Rows[i]["RELATIONSHIP_TO_EMPLOYEE"].ToString();
                dt.Rows[i]["RELATIONSHIP_TO_EMPLOYEE"] = FamilyMembers[relationship].ToString();

                string[] dob = dt.Rows[i]["DOB"].ToString().Split('-');
                dt.Rows[i]["DOB"] = dob[2] + "/" + dob[1] + "/" + dob[0];
            }

            gvDependents.DataSource = dt.Copy();
            gvDependents.DataBind();
        }

        void Save()
        {
            DataTable dtDependents = new DataTable();
            dtDependents = (Session["dtDependents"] as DataTable).Copy();

            DataRow DR = dtDependents.NewRow();

            DR["FULL_NAME"] = txtFullName.Text.Trim();
            DR["NAME_WITH_INITIALS"] = txtInitialsName.Text.Trim();
            DR["RELATIONSHIP_TO_EMPLOYEE"] = ddlRelationshipToEmployee.SelectedValue.ToString().Trim();
            DR["GENDER"] = ddlGender.SelectedValue.ToString().Trim();

            string[] dob = txtDOB.Text.Split('/');
            DR["DOB"] = dob[2] + "-" + dob[1] + "-" + dob[0];
            //DR["DOB"] = txtDOB.Text.Trim();
            DR["NIC"] = txtNIC.Text.Trim();
            DR["OCCUPATION"] = txtOccupation.Text.Trim();
            DR["PLACE_OF_WORK"] = txtPlaceOfWork.Text.Trim();
            DR["CONTACT_NUMBER_MOBILE"] = txtMobileNumber.Text.Trim();
            DR["CONTACT_NUMBER_LAND"] = txtLandPhoneNumber.Text.Trim();

            string EmergencyContact = String.Empty;
            if (chkEmergencyContact.Checked == true)
            {
                EmergencyContact = "1";
            }
            else
            {
                EmergencyContact = "0";
            }

            DR["IS_EMRGENCY_CONTACT"] = EmergencyContact;

            dtDependents.Rows.Add(DR);

            Session["dtDependents"] = dtDependents;

            string EmployeeID = txtemployee.Text.Trim();
            string LoggedUser = (Session["KeyUSER_ID"] as string).Trim();

            EmployeeDependentDataHandler EDDH = new EmployeeDependentDataHandler();
            try
            {
                Utility.Errorhandler.ClearError(lblStatus);
                EDDH.Insert(EmployeeID, LoggedUser, dtDependents.Copy());
                PopulateDepentents();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_INSERTED, lblStatus);
            }
            catch (Exception exp)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, lblStatus);
            }
            finally
            {
                dtDependents = (Session["dtDependents"] as DataTable).Copy();
                dtDependents.Rows.Clear();
                Session["dtDependents"] = dtDependents;
            }
        }

        void Update()
        {
            DataTable dtDependents = new DataTable();
            dtDependents = (Session["dtDependents"] as DataTable).Copy();

            DataRow DR = dtDependents.NewRow();

            DR["FULL_NAME"] = txtFullName.Text.Trim();
            DR["NAME_WITH_INITIALS"] = txtInitialsName.Text.Trim();
            DR["RELATIONSHIP_TO_EMPLOYEE"] = ddlRelationshipToEmployee.SelectedValue.ToString().Trim();
            DR["GENDER"] = ddlGender.SelectedValue.ToString().Trim();

            string[] dob = txtDOB.Text.Split('/');
            DR["DOB"] = dob[2] + "-" + dob[1] + "-" + dob[0];
            //DR["DOB"] = txtDOB.Text.Trim();
            DR["NIC"] = txtNIC.Text.Trim();
            DR["OCCUPATION"] = txtOccupation.Text.Trim();
            DR["PLACE_OF_WORK"] = txtPlaceOfWork.Text.Trim();
            DR["CONTACT_NUMBER_MOBILE"] = txtMobileNumber.Text.Trim();
            DR["CONTACT_NUMBER_LAND"] = txtLandPhoneNumber.Text.Trim();

            string EmergencyContact = String.Empty;
            if (chkEmergencyContact.Checked == true)
            {
                EmergencyContact = "1";
            }
            else
            {
                EmergencyContact = "0";
            }

            DR["IS_EMRGENCY_CONTACT"] = EmergencyContact;

            dtDependents.Rows.Add(DR);

            Session["dtDependents"] = dtDependents;

            string EmployeeID = txtemployee.Text.Trim();
            string LoggedUser = (Session["KeyUSER_ID"] as string).Trim();

            EmployeeDependentDataHandler EDDH = new EmployeeDependentDataHandler();
            try
            {

                int i = gvDependents.SelectedIndex;
                string dependentID = gvDependents.Rows[i].Cells[0].Text;

                Utility.Errorhandler.ClearError(lblStatus);
                EDDH.Update(EmployeeID, LoggedUser, dtDependents.Copy(), dependentID);
                PopulateDepentents();
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_SUCCESS, CommonVariables.MESSAGE_STRING_SUCCESS_UPDATED, lblStatus);
            }
            catch (Exception exp)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, lblStatus);
            }
            finally
            {
                dtDependents = (Session["dtDependents"] as DataTable).Copy();
                dtDependents.Rows.Clear();
                Session["dtDependents"] = dtDependents;
            }
        }

        void clear()
        {
            txtFullName.Text = String.Empty;
            txtInitialsName.Text = String.Empty;
            ddlRelationshipToEmployee.SelectedIndex = 0;
            ddlGender.SelectedIndex = 0;
            txtDOB.Text = String.Empty;
            txtNIC.Text = String.Empty;
            txtOccupation.Text = String.Empty;
            txtPlaceOfWork.Text = String.Empty;
            txtMobileNumber.Text = String.Empty;
            txtLandPhoneNumber.Text = String.Empty;
            chkEmergencyContact.Checked = false;
            if (isActiveEmployee() == false)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Inactive Employee Selected", lblStatus);
                btnSave.Enabled = false;
                return;
            }
            else
            {
                btnSave.Enabled = true;
            }
            btnSave.Text = "Save";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadFamilyMembers();
                loadGender();
                createDependentsDataTable();
            }
            if (IsPostBack)
            {
                spouseUnlock();

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
                        try
                        {
                            clear();
                            Utility.Errorhandler.ClearError(lblStatus);

                            EmployeeDependentDataHandler EDDH = new EmployeeDependentDataHandler();
                            lblInitialsName.Text = EDDH.getNameWithInitials(txtemployee.Text.Trim());
                            PopulateDepentents();

                            if ((isActiveEmployee() == false) && (txtemployee.Text != ""))
                            {
                                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Inactive Employee Selected", lblStatus);
                                btnSave.Enabled = false;
                                return;
                            }
                            else
                            {
                                btnSave.Enabled = true;
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }

        protected void ibtnApply_Click(object sender, ImageClickEventArgs e)
        {
            if (txtFullName.Text != "")
            {
                string[] txtArr = txtFullName.Text.Trim().Split(' ');
                string nameWithInitials = String.Empty;

                for (int i = 0; i < txtArr.Length; i++)
                {
                    if (i != ((txtArr.Length) - 1))
                    {
                        if (txtArr[i].ToString() != "")
                        {
                            nameWithInitials += txtArr[i][0] + ".";
                        }
                    }
                    else
                    {
                        nameWithInitials += " " + txtArr[((txtArr.Length) - 1)];
                    }
                }

                txtInitialsName.Text = nameWithInitials.Trim();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if ((ddlRelationshipToEmployee.SelectedValue == "FM000001") && (txtNIC.Text == ""))
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Spouse NIC is Required", lblStatus);
                return;
            }

            if (ddlRelationshipToEmployee.SelectedValue == "FM000001")
            {
                if (spouseContactNumber() == false)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Spouse Contact Number is Required", lblStatus);
                    return;
                }
            }

            if (EmergencyContactNumber() == false)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Contact Number is Required", lblStatus);
                return;
            }

            //ParentAlreadyExistsValidation
            if (isParentsNSpouseNotExists() == false)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Relationship Already Exists", lblStatus);
                return;
            }

            if (btnSave.Text == "Save")
            {
                //NIC Validation
                if (txtNIC.Text != "")
                {
                    if (nicCheck() == false)
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Invalid NIC Format", lblStatus);
                        return;
                    }
                }

                //NIC Exisists Check
                if (isNICNotExists() == false)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "NIC Already Exists", lblStatus);
                    return;
                }

                //Mobile Number Length Check
                if (PhoneNumberLengthCheck(txtMobileNumber.Text.Trim()) == false)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Invalid Mobile Number", lblStatus);
                    return;
                }

                //Invalid Land Phone Number Length Check
                if (PhoneNumberLengthCheck(txtLandPhoneNumber.Text.Trim()) == false)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Invalid Land Phone Number", lblStatus);
                    return;
                }

                Save();
            }
            else
            {
                //NIC Validation
                if (txtNIC.Text != "")
                {
                    if (nicCheck() == false)
                    {
                        Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Invalid NIC Format", lblStatus);
                        return;
                    }
                }

                //NIC Exisists Check
                if (isNICNotExists() == false)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "NIC Already Exists", lblStatus);
                    return;
                }

                //Mobile Number Length Check
                if (PhoneNumberLengthCheck(txtMobileNumber.Text.Trim()) == false)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Invalid Mobile Number", lblStatus);
                    return;
                }

                //Invalid Land Phone Number Length Check
                if (PhoneNumberLengthCheck(txtLandPhoneNumber.Text.Trim()) == false)
                {
                    Utility.Errorhandler.GetError(CommonVariables.MESSAGE_WARNING, "Invalid Land Phone Number", lblStatus);
                    return;
                }

                Update();
            }
            clear();
            spouseUnlock();
        }

        protected void gvDependents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onClick", ClientScript.GetPostBackEventReference(this.gvDependents, "Select$" + e.Row.RowIndex.ToString()));
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

        protected void gvDependents_SelectedIndexChanged(object sender, EventArgs e)
        {

            clear();
            Utility.Errorhandler.ClearError(lblStatus);
            int i = gvDependents.SelectedIndex;
            string dependentID = gvDependents.Rows[i].Cells[0].Text;


            EmployeeDependentDataHandler EPDDH = new EmployeeDependentDataHandler();
            DataTable dt = new DataTable();
            dt = EPDDH.PopulateDependents(txtemployee.Text.Trim()).Copy();


            DataRow[] result = dt.Select("DEPENDANT_ID = '" + dependentID.Trim() + "'");



            string fullName = HttpUtility.HtmlDecode(result[0]["FULL_NAME"].ToString()).Trim();
            string NameWithInitials = HttpUtility.HtmlDecode(gvDependents.Rows[i].Cells[1].Text).Trim();
            string RelationshipToEmploye = HttpUtility.HtmlDecode(gvDependents.Rows[i].Cells[2].Text).Trim();
            string Gender = HttpUtility.HtmlDecode(gvDependents.Rows[i].Cells[3].Text).Trim();
            string DOB = HttpUtility.HtmlDecode(gvDependents.Rows[i].Cells[4].Text).Trim();
            string NIC = HttpUtility.HtmlDecode(gvDependents.Rows[i].Cells[5].Text).Trim();
            string Occupation = HttpUtility.HtmlDecode(result[0]["OCCUPATION"].ToString()).Trim();
            string PlaceOfWork = HttpUtility.HtmlDecode(result[0]["PLACE_OF_WORK"].ToString()).Trim();
            string MobileNumber = HttpUtility.HtmlDecode(gvDependents.Rows[i].Cells[6].Text).Trim();
            string LandPhoneNumber = HttpUtility.HtmlDecode(gvDependents.Rows[i].Cells[7].Text).Trim();
            string isEmegencyContact = HttpUtility.HtmlDecode(gvDependents.Rows[i].Cells[8].Text).Trim();

            txtFullName.Text = fullName;
            txtInitialsName.Text = NameWithInitials;
            ddlRelationshipToEmployee.SelectedIndex = ddlRelationshipToEmployee.Items.IndexOf(ddlRelationshipToEmployee.Items.FindByText(RelationshipToEmploye));
            ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByValue(Gender));
            txtDOB.Text = DOB;
            txtNIC.Text = NIC;
            txtOccupation.Text = Occupation;
            txtPlaceOfWork.Text = PlaceOfWork;
            txtMobileNumber.Text = MobileNumber;
            txtLandPhoneNumber.Text = LandPhoneNumber;
            if (isEmegencyContact == "YES")
            {
                chkEmergencyContact.Checked = true;
            }
            else
            {
                chkEmergencyContact.Checked = false;
            }

            btnSave.Text = "Update";

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Utility.Errorhandler.ClearError(lblStatus);
            clear();   
            spouseUnlock();         
        }

        Boolean nicCheck()
        {
            Boolean Staus = false;
            string NIC = txtNIC.Text.Trim();
            
            if (NIC.Length != 10)
            {
                return false;
            }
            else
            {
                Staus = true;
            }

            if ((NIC[9] == 'V') || (NIC[9] == 'v') || (NIC[9] == 'X') || (NIC[9] == 'x'))
            {
                Staus = true;
            }
            else
            {
                return false;
            }

            for (int i = 0; i < 9; i++)
            {
                try
                {
                    int x = Convert.ToInt32(NIC[i].ToString());
                }
                catch
                {
                    return false;
                }
            }
            return Staus;
        }

        Boolean isNICNotExists()
        {
            Boolean Status = true;
            Boolean isUpdateRecord = false;
            Boolean rowSelected = false;

            if (txtNIC.Text == "")
            {
                return true;
            }

            txtNIC.Text = txtNIC.Text.ToUpper();

            int selIndex = gvDependents.SelectedIndex;
            if (selIndex >= 0)
            {
                rowSelected = true;
            }
            else
            {
                rowSelected = false;
            }

            if (btnSave.Text == "Update")
            {
                isUpdateRecord = true;
            }
            else
            {
                isUpdateRecord = false;
            }

            for (int i = 0; i < gvDependents.Rows.Count; i++)
            {
                string nic = gvDependents.Rows[i].Cells[5].Text;

                if (txtNIC.Text.Trim() == nic.Trim())
                {
                    if ((rowSelected == true) && (isUpdateRecord == true))
                    {
                        string a = gvDependents.Rows[i].Cells[5].Text;
                        string b = gvDependents.Rows[gvDependents.SelectedIndex].Cells[5].Text;

                        //if (gvDependents.Rows[i].Cells[5].Text == gvDependents.Rows[gvDependents.SelectedIndex].Cells[5].Text)
                        //if (a == b)
                        if (i == gvDependents.SelectedIndex)
                        {
                            Status = true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return Status;
        }

        Boolean PhoneNumberLengthCheck(string Number)
        {
            if (Number != "")
            {
                if (Number.Length != 10)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        Boolean spouseContactNumber()
        {
            Boolean Status = false;

            string mobile = txtMobileNumber.Text.Trim();
            string land = txtLandPhoneNumber.Text.Trim();

            if ((mobile == "") && (land == ""))
            {
                return false;
            }
            else
            {
                Status = true;
            }
            return Status;
        }

        Boolean EmergencyContactNumber()
        {
            Boolean Status = false;

            if (chkEmergencyContact.Checked == true)
            {
                string mobile = txtMobileNumber.Text.Trim();
                string land = txtLandPhoneNumber.Text.Trim();

                if ((mobile == "") && (land == ""))
                {
                    return false;
                }
                else
                {
                    Status = true;
                }
            }
            else
            {
                Status = true;
            }

            return Status;
        }

        Boolean isParentsNSpouseNotExists()
        {
            Boolean Status = true;
            Boolean isUpdateRecord = false;
            Boolean rowSelected = false;

            string Father = Constants.FATHER_TEXT;
            string Mother = Constants.MOTHER_TEXT;
            string Spouse = Constants.SPOUSE_TEXT;

            int selIndex = gvDependents.SelectedIndex;
            if (selIndex >= 0)
            {
                rowSelected = true;
            }
            else
            {
                rowSelected = false;
            }

            if (btnSave.Text == "Update")
            {
                isUpdateRecord = true;
            }
            else
            {
                isUpdateRecord = false;
            }
            if ((ddlRelationshipToEmployee.SelectedItem.Text.Trim() == Father) || (ddlRelationshipToEmployee.SelectedItem.Text.Trim() == Mother) || (ddlRelationshipToEmployee.SelectedItem.Text.Trim() == Spouse))
            {
                for (int i = 0; i < gvDependents.Rows.Count; i++)
                {
                    string relationship = gvDependents.Rows[i].Cells[2].Text;

                    if(ddlRelationshipToEmployee.SelectedItem.Text == relationship)
                    {
                        if ((rowSelected == true) && (isUpdateRecord == true))
                        {
                            if (i == gvDependents.SelectedIndex)
                            {
                                Status = true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return Status;
        }

        void GenderSelector()
        {
            if (ddlRelationshipToEmployee.SelectedIndex > 0)
            {
                //Male
                if (Constants.FATHER_ID == ddlRelationshipToEmployee.SelectedValue)
                {
                    ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByValue("M"));
                }
                if (Constants.SON_ID == ddlRelationshipToEmployee.SelectedValue)
                {
                    ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByValue("M"));
                }
                if (Constants.GRAND_FATHER_ID == ddlRelationshipToEmployee.SelectedValue)
                {
                    ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByValue("M"));
                }
                if (Constants.GRAND_SON_ID == ddlRelationshipToEmployee.SelectedValue)
                {
                    ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByValue("M"));
                }

                //Female
                if (Constants.MOTHER_ID == ddlRelationshipToEmployee.SelectedValue)
                {
                    ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByValue("F"));
                }
                if (Constants.DAUGHTER_ID == ddlRelationshipToEmployee.SelectedValue)
                {
                    ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByValue("F"));
                }
                if (Constants.GRAND_MOTHER_ID == ddlRelationshipToEmployee.SelectedValue)
                {
                    ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByValue("F"));
                }
                if (Constants.GRAND_DAUGHTER_ID == ddlRelationshipToEmployee.SelectedValue)
                {
                    ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByValue("F"));
                }
            }
            if (ddlRelationshipToEmployee.SelectedIndex == 0)
            {
                ddlGender.SelectedIndex = 0;
            }
        }

        protected void ddlRelationshipToEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenderSelector();
            spouseUnlock();
        }

        Boolean isActiveEmployee()
        {
            Boolean Status = false;
            EmployeeDependentDataHandler EDDH = new EmployeeDependentDataHandler();
            Status = EDDH.IsEmployeeActive(txtemployee.Text.Trim());

            if (txtemployee.Text == "")
            {
                Status = true;
            }

            return Status;
        }

        void spouseUnlock()
        {
            try
            {
                //Utility.Errorhandler.ClearError(lblStatus);
                if (ddlRelationshipToEmployee.SelectedValue == Constants.SPOUSE_ID)
                {
                    ddlGender.Enabled = true;
                }
                else
                {
                    ddlGender.Enabled = false;
                }

                if (ddlRelationshipToEmployee.SelectedIndex == 0)
                {
                    ddlGender.Enabled = true;
                }
            }
            catch (Exception exp)
            {
                Utility.Errorhandler.GetError(CommonVariables.MESSAGE_ERROR, exp.Message, lblStatus);
            }
        }
    }
}