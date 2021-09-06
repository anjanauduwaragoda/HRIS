<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="webFrmSecondaryEducation.aspx.cs" Inherits="GroupHRIS.Employee.webFrmSecondaryEducation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function openLOVWindow(file, window, ctlName) {
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
            document.getElementById("hfCaller").value = ctlName;
        }


        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;
        }

        function getValueFromChild(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName) {
            document.getElementById("txtEmployeeID").value = sEmpId;
            document.getElementById("txtName").value = sName;

            writeToHiddenFields(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName);

            DoPostBack();
        }

        function writeToHiddenFields(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName) {
            document.getElementById("hfEmpID").value = sEmpId;
            document.getElementById("hfName").value = sName;
        }

        function DoPostBack() {
            __doPostBack("txtEmployeeID", "TextChanged");
        }



    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <span>Secondary Education</span>
    <hr />
    <br />

    <table class="styleMainTb">
        <tr>
            <td width="30%" align="right">
                &nbsp;<asp:Label ID="Label1" runat="server" 
                    Text="Employee Id" AssociatedControlID="txtEmployeeID"></asp:Label>
            </td>
            <td width="40%" align="left">
                <asp:TextBox ID="txtEmployeeID" runat="server" Width="250px" ClientIDMode="Static"
                    ViewStateMode="Enabled" ReadOnly="True" AutoPostBack="True"></asp:TextBox>
                &nbsp;
                <%if(isSearchable)%>
                <%{%>
                <img alt="Search for an Employee" src="../Images/Common/Search.jpg" height="20px"
                    width="20px" 
                    onclick="openLOVWindow('webFrmEmployeeSearch.aspx','Search','txtEmployeeID')" 
                    id="imgSearch" />
                <%}%>
            </td>
            <td width="30%" align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmployeeID"
                    ErrorMessage="Employee Id is required." ForeColor="Red" 
                    ValidationGroup="vgSubmit">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label2" runat="server" Text="Name " AssociatedControlID="txtName"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtName" runat="server" Width="250px" ClientIDMode="Static" 
                    ReadOnly="True" BorderStyle="None" ForeColor="Blue"></asp:TextBox>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <strong>G.C.E. O/L</strong>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label19" runat="server" Text="Year"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlYearOL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label29" runat="server" Text="Attempt"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlOLAttempt" runat="server" Width="54px" AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                School
            </td>
            <td>
                <asp:TextBox ID="txtScoolOL" runat="server" Width="250px" MaxLength="45"></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label3" runat="server" Text="Subject 1"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubject1OL" runat="server" Width="187px" MaxLength="45"></asp:TextBox>
                <asp:Label ID="Label4" runat="server" Text=" / "></asp:Label>
                <asp:DropDownList ID="ddlGrade1OL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label5" runat="server" Text="Subject 2"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubject2OL" runat="server" Width="187px" MaxLength="45"></asp:TextBox>
                <asp:Label ID="Label6" runat="server" Text=" / "></asp:Label>
                <asp:DropDownList ID="ddlGrade2OL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label7" runat="server" Text="Subject 3 "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubject3OL" runat="server" Width="187px" MaxLength="45"></asp:TextBox>
                <asp:Label ID="Label8" runat="server" Text=" / "></asp:Label>
                <asp:DropDownList ID="ddlGrade3OL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label9" runat="server" Text="Subject 4 "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubject4OL" runat="server" Width="187px" MaxLength="45"></asp:TextBox>
                <asp:Label ID="Label10" runat="server" Text=" / "></asp:Label>
                <asp:DropDownList ID="ddlGrade4OL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label11" runat="server" Text="Subject 5 "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubject5OL" runat="server" Width="187px" MaxLength="45"></asp:TextBox>
                <asp:Label ID="Label12" runat="server" Text=" / "></asp:Label>
                <asp:DropDownList ID="ddlGrade5OL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label13" runat="server" Text="Subject 6"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubject6OL" runat="server" Width="187px" MaxLength="45"></asp:TextBox>
                <asp:Label ID="Label14" runat="server" Text=" / "></asp:Label>
                <asp:DropDownList ID="ddlGrade6OL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label15" runat="server" Text="Subject 7"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubject7OL" runat="server" Width="187px" MaxLength="45"></asp:TextBox>
                <asp:Label ID="Label16" runat="server" Text=" / "></asp:Label>
                <asp:DropDownList ID="ddlGrade7OL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label17" runat="server" Text="Subject 8"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubject8OL" runat="server" Width="187px" MaxLength="45"></asp:TextBox>
                <asp:Label ID="Label18" runat="server" Text=" / "></asp:Label>
                <asp:DropDownList ID="ddlGrade8OL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>

        <tr>
            <td align="right">
                <asp:Label ID="Label31" runat="server" Text="Subject 9"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubject9OL" runat="server" Width="187px" MaxLength="45"></asp:TextBox>
                <asp:Label ID="Label32" runat="server" Text=" / "></asp:Label>
                <asp:DropDownList ID="ddlGrade9OL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>

        <tr>
            <td align="right">
                <asp:Label ID="Label33" runat="server" Text="Subject 10"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubject10OL" runat="server" Width="187px" MaxLength="45"></asp:TextBox>
                <asp:Label ID="Label34" runat="server" Text=" / "></asp:Label>
                <asp:DropDownList ID="ddlGrade10OL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td align="left">
                <asp:Button ID="btnOlSave" runat="server" Text="Save" Width="125px" 
                     ValidationGroup="vgSubmit" onclick="btnOlSave_Click" />
                <asp:Button ID="btnOlCancel" runat="server" Text="Clear" Width="125px" onclick="btnOlCancel_Click" 
                    />
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <strong>G.C.E. A/L</strong>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label20" runat="server" Text="Year"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlYearAL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label30" runat="server" Text="Attempt"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlALAttempt" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                School
            </td>
            <td>
                <asp:TextBox ID="txtSchoolAL" runat="server" Width="250px" MaxLength="45"></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label21" runat="server" Text="Subject 1"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubject1AL" runat="server" Width="187px" MaxLength="45"></asp:TextBox>
                <asp:Label ID="Label22" runat="server" Text=" / "></asp:Label>
                <asp:DropDownList ID="ddlGrade1AL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label23" runat="server" Text="Subject 2"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubject2AL" runat="server" Width="187px" MaxLength="45"></asp:TextBox>
                <asp:Label ID="Label24" runat="server" Text=" / "></asp:Label>
                <asp:DropDownList ID="ddlGrade2AL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label25" runat="server" Text="Subject 3"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubject3AL" runat="server" Width="187px" MaxLength="45"></asp:TextBox>
                <asp:Label ID="Label26" runat="server" Text=" / "></asp:Label>
                <asp:DropDownList ID="ddlGrade3AL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label27" runat="server" Text="Subject 4 "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSubject4AL" runat="server" Width="187px" MaxLength="45"></asp:TextBox>
                <asp:Label ID="Label28" runat="server" Text=" / "></asp:Label>
                <asp:DropDownList ID="ddlGrade4AL" runat="server" Width="54px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td>
                <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfEmpID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfName" runat="server" ClientIDMode="Static" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td align="left">
                <asp:Button ID="btnAlSave" runat="server" Text="Save" Width="125px" 
                    OnClick="btnAlSave_Click" ValidationGroup="vgSubmit" />
                <asp:Button ID="btnAlCancel" runat="server" Text="Clear" Width="125px" 
                    OnClick="btnAlCancel_Click" />
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
    </table>


    <table class="styleMainTb">
        <tr>
            <td align="center">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    <tr> 
        <td align="left">
            <asp:ValidationSummary ID="vsSubmit" runat="server" 
                ValidationGroup="vgSubmit" ForeColor="Red" />
        </td>
    </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblMsg" runat="server" Font-Size="10pt" ForeColor="Blue"></asp:Label>
            </td>
        </tr>
    </table>


    <br />
    <span>List of Secondary Education Records OL</span>
    <hr />
    <br />

    <div>
        <asp:GridView ID="gvSecEduOL" runat="server" AutoGenerateColumns="False" Width="100%"
            AllowPaging="True" PageSize="12" 
            OnPageIndexChanging="gvSecEduOL_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Emp. ID" />
                <asp:BoundField DataField="IS_AL" HeaderText="is AL?" />
                <asp:BoundField DataField="ATTEMPT" HeaderText="Attempt" />
                <asp:BoundField DataField="ATTEMPTED_YEAR" HeaderText="Year" />
                <asp:BoundField DataField="SCHOOL" HeaderText="School" />
                <asp:BoundField DataField="SUBJECT_NAME" HeaderText="Subject" />
                <asp:BoundField DataField="GRADE" HeaderText="Grade" />
                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status Code" />
                <asp:BoundField DataField="STATUS_DESC" HeaderText="Status" />
            </Columns>
        </asp:GridView>
    </div>

    <br />
    <br />
    <span>List of Secondary Education Records AL</span>
    <hr />
    <br />

    <div>
        <asp:GridView ID="gvSecEduAL" runat="server" AutoGenerateColumns="False" Width="100%"
            AllowPaging="True" PageSize="12" 
            OnPageIndexChanging="gvSecEduAL_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Emp. ID" />
                <asp:BoundField DataField="IS_AL" HeaderText="is AL?" />
                <asp:BoundField DataField="ATTEMPT" HeaderText="Attempt" />
                <asp:BoundField DataField="ATTEMPTED_YEAR" HeaderText="Year" />
                <asp:BoundField DataField="SCHOOL" HeaderText="School" />
                <asp:BoundField DataField="SUBJECT_NAME" HeaderText="Subject" />
                <asp:BoundField DataField="GRADE" HeaderText="Grade" />
                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status Code" />
                <asp:BoundField DataField="STATUS_DESC" HeaderText="Status" />
            </Columns>
        </asp:GridView>
    </div>

    <br />
    <br />
    <br />
    <span>List of <b>Obsoleted / Rejected</b> Secondary Education Records</span>
    <hr />
    <br />
    <div>
        <asp:GridView ID="gvSecEduObsolete" runat="server" AutoGenerateColumns="False" Width="100%"
            AllowPaging="True" PageSize="12" 
            OnPageIndexChanging="gvSecEduObsolete_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Emp. ID" />
                <asp:BoundField DataField="IS_AL" HeaderText="is AL?" />
                <asp:BoundField DataField="ATTEMPT" HeaderText="Attempt" />
                <asp:BoundField DataField="ATTEMPTED_YEAR" HeaderText="Year" />
                <asp:BoundField DataField="SCHOOL" HeaderText="School" />
                <asp:BoundField DataField="SUBJECT_NAME" HeaderText="Subject" />
                <asp:BoundField DataField="GRADE" HeaderText="Grade" />
                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status Code" />
                <asp:BoundField DataField="STATUS_DESC" HeaderText="Status" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
