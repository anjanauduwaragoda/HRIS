<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="webFrmEmployeeSecondment.aspx.cs" Inherits="GroupHRIS.Employee.webFrmEmployeeSecondment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function openLOVWindow(file, window, ctlName) {
            childWindow = open(file, window, 'resizable=no,width=950,height=770,scrollbars=yes,top=50,left=200,status=yes');
            document.getElementById("hfCaller").value = ctlName;
        }


        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;
        }

        function getValueFromChild(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName, sBranchId, sBranchName, sCC, sPC) {
            document.getElementById("txtEmployeeID").value = sEmpId;
            document.getElementById("txtName").value = sName;
            document.getElementById("txtFromCompanyCode").value = sCompanyCode;
            document.getElementById("txtFromCompanyName").value = sCompanyName;
            document.getElementById("txtFromDepartmentID").value = sDepartmentId;
            document.getElementById("txtFromDepartmentName").value = sDepartmentName;
            document.getElementById("txtFromDivisionID").value = sDivisionId;
            document.getElementById("txtFromDivisionName").value = sDivisionName;

            document.getElementById("txtFromBranchID").value = sBranchId;
            document.getElementById("txtFromBranchName").value = sBranchName;
            document.getElementById("txtFromCC").value = sCC;
            document.getElementById("txtFromPC").value = sPC;

            writeToHiddenFields(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName, sBranchId, sBranchName, sCC, sPC);

            DoPostBack();
        }

        function writeToHiddenFields(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName, sBranchId, sBranchName, sCC, sPC) {
            document.getElementById("hfEmpID").value = sEmpId;
            document.getElementById("hfName").value = sName;
            document.getElementById("hfCompanyCode").value = sCompanyCode;
            document.getElementById("hfCompanyName").value = sCompanyName;
            document.getElementById("hfDepartmentID").value = sDepartmentId;
            document.getElementById("hfDepartmentName").value = sDepartmentName;
            document.getElementById("hfDivisionID").value = sDivisionId;
            document.getElementById("hfDivisionName").value = sDivisionName;

            document.getElementById("hfBranchID").value = sBranchId;
            document.getElementById("hfBranchName").value = sBranchName;
            document.getElementById("hfCC").value = sCC;
            document.getElementById("hfPC").value = sPC;
        }

        function DoPostBack() {
            __doPostBack("txtEmployeeID", "TextChanged");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <span>Employee Secondment</span>
    <hr />
    <br />
    <table class="styleMainTb">
        <tr>
            <td width="30%" align="right">
                &nbsp;<asp:Label ID="Label1" runat="server" Text="Employee Id" AssociatedControlID="txtEmployeeID"></asp:Label>
            </td>
            <td width="40%" align="left">
                <asp:TextBox ID="txtEmployeeID" runat="server" Width="250px" ClientIDMode="Static"
                    ViewStateMode="Enabled" ReadOnly="True" AutoPostBack="True"></asp:TextBox>
                &nbsp;
                <img alt="Search for an Employee" src="../Images/Common/Search.jpg" height="20px"
                    width="20px" onclick="openLOVWindow('webFrmEmployeeSearch.aspx','Search','txtEmployeeID')" />
            </td>
            <td width="30%" align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmployeeID"
                    ErrorMessage="Employee Id is required" ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label2" runat="server" Text="Name " AssociatedControlID="txtName"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtName" runat="server" Width="250px" ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;<asp:Label ID="Label3" runat="server" Text="Base Company" AssociatedControlID="txtFromCompanyName"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFromCompanyCode" runat="server" Width="65px" ClientIDMode="Static"
                    ReadOnly="True"></asp:TextBox>
                <asp:TextBox ID="txtFromCompanyName" runat="server" Width="177px" ClientIDMode="Static"
                    ReadOnly="True"></asp:TextBox>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;<asp:Label ID="Label4" runat="server" Text="Base Department" AssociatedControlID="txtFromDepartmentName"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFromDepartmentID" runat="server" Width="65px" ClientIDMode="Static"
                    ReadOnly="True"></asp:TextBox>
                <asp:TextBox ID="txtFromDepartmentName" runat="server" Width="177px" ClientIDMode="Static"
                    ReadOnly="True"></asp:TextBox>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;<asp:Label ID="Label5" runat="server" Text="Base Division" AssociatedControlID="txtFromDivisionName"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFromDivisionID" runat="server" Width="65px" ClientIDMode="Static"
                    ReadOnly="True"></asp:TextBox>
                <asp:TextBox ID="txtFromDivisionName" runat="server" Width="177px" ClientIDMode="Static"
                    ReadOnly="True"></asp:TextBox>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;<asp:Label ID="Label12" runat="server" Text="Base Branch" AssociatedControlID="txtFromDivisionName"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFromBranchID" runat="server" Width="65px" ClientIDMode="Static"
                    ReadOnly="True"></asp:TextBox>
                <asp:TextBox ID="txtFromBranchName" runat="server" Width="177px" ClientIDMode="Static"
                    ReadOnly="True"></asp:TextBox>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;<asp:Label ID="Label13" runat="server" Text="Base Cost Center" AssociatedControlID="txtFromCC"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFromCC" runat="server" Width="250px" ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;<asp:Label ID="Label14" runat="server" Text="Base Profit Center" AssociatedControlID="txtFromPC"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFromPC" runat="server" Width="250px" ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;<asp:Label ID="Label6" runat="server" Text="Secondment Company" AssociatedControlID="ddlSecCompany"></asp:Label>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSecCompany" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlToCompany_SelectedIndexChanged"
                    Width="256px">
                </asp:DropDownList>
            </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlSecCompany"
                    ErrorMessage="Secondment Company is required." ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;<asp:Label ID="Label7" runat="server" Text="Secondment Department" AssociatedControlID="ddlSecDepartment"></asp:Label>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSecDepartment" runat="server" Width="256px" TabIndex="1"
                    OnSelectedIndexChanged="ddlToDepartment_SelectedIndexChanged" AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlSecDepartment"
                    ErrorMessage="Secondment Department is required." ValidationGroup="vgSubmit"
                    ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;<asp:Label ID="Label8" runat="server" Text="Secondment Division" AssociatedControlID="ddlSecDivision"></asp:Label>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSecDivision" runat="server" Width="256px" TabIndex="2">
                </asp:DropDownList>
            </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlSecDivision"
                    ErrorMessage="Secondment Division is required." ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;<asp:Label ID="Label15" runat="server" Text="Secondment Branch" AssociatedControlID="ddlToBranch"></asp:Label>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlToBranch" runat="server" Width="256px" TabIndex="2">
                </asp:DropDownList>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;<asp:Label ID="Label16" runat="server" Text="Secondment Cost Center" AssociatedControlID="ddlToCC"></asp:Label>
            </td>
            <td align="left">
                <%--<asp:TextBox ID="txtToCC" runat="server" Width="250px" ClientIDMode="Static" MaxLength="10"></asp:TextBox>--%>
                <asp:DropDownList ID="ddlToCC" runat="server" Width="256px">
                </asp:DropDownList>
                <%--<asp:FilteredTextBoxExtender ID="fttxtToCC" runat="server" TargetControlID="txtToCC"
                    FilterType="Numbers" ValidChars="">
                </asp:FilteredTextBoxExtender>--%>
            </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlToCC"
                    ErrorMessage="To Cost Center  is required." ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;<asp:Label ID="Label17" runat="server" Text="Secondment Profit Center" AssociatedControlID="ddlToPC"></asp:Label>
            </td>
            <td align="left">
                <%--<asp:TextBox ID="txtToPC" runat="server" Width="250px" ClientIDMode="Static" MaxLength="10"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="fttxtToPC" runat="server" TargetControlID="txtToPC"
                    FilterType="Numbers" ValidChars="">
                </asp:FilteredTextBoxExtender>--%>
                <asp:DropDownList ID="ddlToPC" runat="server" Width="256px">
                </asp:DropDownList>
            </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlToPC"
                    ErrorMessage="To Profit Center  is required." ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label9" runat="server" Text="Starting Date" AssociatedControlID="txtFromDate"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                <asp:CalendarExtender ID="ceStartDate" runat="server" TargetControlID="txtFromDate" Format="yyyy/MM/dd">
                </asp:CalendarExtender>                     
               <%-- Format="yyyy/MM/dd"--%>
                <asp:FilteredTextBoxExtender ID="fteDob" runat="server" TargetControlID="txtFromDate"
                    FilterType="Custom, Numbers" ValidChars="/">
                </asp:FilteredTextBoxExtender>
            </td>
            <td align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFromDate"
                    ErrorMessage="From Date is required." ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label11" runat="server" Text="End Date" AssociatedControlID="txtEndDate"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtEndDate" runat="server" MaxLength="10" Width="250px" ToolTip="Please enter the ending date of the secondment"></asp:TextBox>
                <asp:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" TargetControlID="txtEndDate" Format="yyyy/MM/dd">
                </asp:CalendarExtender>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtEndDate"
                    FilterType="Custom, Numbers" ValidChars="/">
                </asp:FilteredTextBoxExtender>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label10" runat="server" Text="Remarks" AssociatedControlID="txtRemarks"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtRemarks" runat="server" MaxLength="45" Rows="2" TextMode="MultiLine"
                    Width="250px"></asp:TextBox>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:HiddenField ID="hfEmpID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfName" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfCompanyCode" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfCompanyName" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfDepartmentID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfDepartmentName" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfDivisionID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfDivisionName" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfBranchID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfBranchName" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfCC" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfPC" runat="server" ClientIDMode="Static" />
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
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" OnClick="btnSave_Click"
                    ValidationGroup="vgSubmit" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="125px" OnClick="btnCancel_Click" />
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
                <asp:ValidationSummary ID="vsSubmit" runat="server" ValidationGroup="vgSubmit" ForeColor="Red" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblMsg" runat="server" Font-Size="10pt" ForeColor="Blue"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <span>List of Employee Secondments</span>
    <hr />
    <br />
    <div>
        <asp:GridView ID="gvSecondments" runat="server" AutoGenerateColumns="False" Width="100%"
            PageSize="20" OnPageIndexChanging="gvSecondments_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Emp. ID" />
                <asp:BoundField DataField="COMPANY_ID" HeaderText="Comp Code" />
                <asp:BoundField DataField="COMP_NAME" HeaderText="Company" />
                <asp:BoundField DataField="DEPT_ID" HeaderText="Dept ID" />
                <asp:BoundField DataField="DEPT_NAME" HeaderText="Department" />
                <asp:BoundField DataField="DIVISION_ID" HeaderText="Div ID" />
                <asp:BoundField DataField="BRANCH_ID" HeaderText="Branch" />
                <asp:BoundField DataField="COST_CENTER" HeaderText="Cost Center" />
                <asp:BoundField DataField="PROFIT_CENTER" HeaderText="Prof. Center" />
                <asp:BoundField DataField="DIV_NAME" HeaderText="Div Name" />
                <asp:BoundField DataField="FROM_DATE" HeaderText="From Date" DataFormatString="{0:yyyy/MM/dd}" />
                <asp:BoundField DataField="END_DATE" HeaderText="End Date" DataFormatString="{0:yyyy/MM/dd}" />
                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
