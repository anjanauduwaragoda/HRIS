<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="webFrmEmployeeBankAccount.aspx.cs" Inherits="GroupHRIS.Employee.webFrmEmployeeBankAccount" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpBankAcc.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        var txb;
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=800,height=600,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;

            var id = document.getElementById(txb).value;
            document.getElementById("hfVal").value = id;
            //document.getElementById(ctl).value = sRetVal;

            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }
    </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <span>Employee Bank Account Information</span>
    <hr />
    <asp:HiddenField ID="hfEID" runat="server" />
    <table class="styleTable">
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                <asp:Label ID="Label1" runat="server" Text="Employee Id"></asp:Label>
            </td>
            <td class="styleTableCell2">
                <asp:TextBox ID="txtEmployeeId" ReadOnly="true" runat="server" CssClass="styleTableCell2TextBox"
                    MaxLength="8" ValidationGroup="vgBankAccount" ClientIDMode="Static"></asp:TextBox>
            </td>
            <td class="styleTableCell3">
                <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('webFrmEmployeeSearch.aspx','Search','txtEmployeeId')" />
            </td>
            <td class="styleTableCell4">
                <asp:RequiredFieldValidator ID="rfvEmployeeId" runat="server" ErrorMessage="Employee Id is required"
                    ControlToValidate="txtEmployeeId" ForeColor="#CC0000" 
                    ValidationGroup="vgBankAccount">*</asp:RequiredFieldValidator>                
            </td>
            <td class="styleTableCell5">
                &nbsp;</td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                <asp:Label ID="Label2" runat="server" Text="Bank"></asp:Label>
            </td>
            <td class="styleTableCell2">
                <asp:DropDownList ID="ddlBank" runat="server" CssClass="styleDdl" 
                    ValidationGroup="vgBankAccount" AutoPostBack="True" 
                    onselectedindexchanged="ddlBank_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
                <asp:RequiredFieldValidator ID="rfvBank" runat="server" ErrorMessage="Bank is required"
                    ControlToValidate="ddlBank" ForeColor="#CC0000" 
                    ValidationGroup="vgBankAccount">*</asp:RequiredFieldValidator>
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                <asp:Label ID="Label3" runat="server" Text="Bank Branch"></asp:Label>
            </td>
            <td class="styleTableCell2">
                <asp:DropDownList ID="ddlBankBranch" runat="server" CssClass="styleDdl" ValidationGroup="vgBankAccount">
                </asp:DropDownList>
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
                <asp:RequiredFieldValidator ID="rfvEmployeeId1" runat="server" ErrorMessage="Bank branch is required"
                    ControlToValidate="ddlBankBranch" ForeColor="#CC0000" 
                    ValidationGroup="vgBankAccount">*</asp:RequiredFieldValidator>
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                <asp:Label ID="Label4" runat="server" Text="Account No"></asp:Label>
            </td>
            <td class="styleTableCell2">
                <asp:TextBox ID="txtAccountNo" runat="server" CssClass="styleTableCell2TextBox" MaxLength="30"
                    ValidationGroup="vgBankAccount"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="fteAccNumber" runat="server" TargetControlID="txtAccountNo" FilterType="Custom, Numbers" ValidChars="0123456789" >
                </asp:FilteredTextBoxExtender>      
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
                <asp:RequiredFieldValidator ID="rfvEmployeeId2" runat="server" ErrorMessage="Account  No is required"
                    ControlToValidate="txtAccountNo" ForeColor="#CC0000" 
                    ValidationGroup="vgBankAccount">*</asp:RequiredFieldValidator>
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                <asp:Label ID="Label5" runat="server" Text="Status"></asp:Label>
            </td>
            <td class="styleTableCell2">
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="styleDdl" ValidationGroup="vgBankAccount">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Status is required"
                    ControlToValidate="ddlStatus" ForeColor="#CC0000" 
                    ValidationGroup="vgBankAccount">*</asp:RequiredFieldValidator>
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                <asp:Label ID="Label6" runat="server" Text="Remarks"></asp:Label>
            </td>
            <td class="styleTableCell2">
                <asp:TextBox ID="txtRemarks" runat="server" CssClass="styleTableCell2TextBox" MaxLength="45"
                    TextMode="MultiLine" ValidationGroup="vgBankAccount"></asp:TextBox>
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
            </td>
            <td class="styleTableCell2">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" 
                    ValidationGroup="vgBankAccount" onclick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="125px" 
                    onclick="btnCancel_Click" />
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">                
            </td>
            <td class="styleTableCell2">
                <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" 
                    ViewStateMode="Enabled" />   
                    <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />  
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">                
            </td>
            <td class="styleTableCell2">
                <asp:HiddenField ID="hfEmpId" runat="server" ClientIDMode="Static" 
                    ViewStateMode="Enabled" />     
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
    </table>
    <br />
    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
    <br />
    <asp:ValidationSummary ID="vsBankAccount" runat="server" 
        ValidationGroup="vgBankAccount" ForeColor="#CC0000"/>    
    <asp:ValidationSummary ID="vsEmpId" runat="server" ValidationGroup="vgEmpId" 
        ForeColor="#CC3300" />
    <br />
    <span>Employee Bank Account Details</span>
    <hr />
    <br />
    <asp:GridView ID="gvBankAccounts" runat="server" AutoGenerateColumns="False" 
        onrowdatabound="gvBankAccounts_RowDataBound" 
        onselectedindexchanged="gvBankAccounts_SelectedIndexChanged" Width="100%">
        <Columns>
            <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="EMPLOYEE_ID" />
            <asp:BoundField DataField="BANK_ID" HeaderText="BANK_ID" />
            <asp:BoundField DataField="BANK_NAME" HeaderText="BANK_NAME" >
            <ItemStyle Width="100px" />
            </asp:BoundField>
            <asp:BoundField DataField="BRANCH_ID" HeaderText="BRANCH_ID" />
            <asp:BoundField DataField="BRANCH_NAME" HeaderText="BRANCH_NAME" />
            <asp:BoundField DataField="BANK_ACCOUNT_NUMBER" 
                HeaderText="BANK_ACCOUNT_NUMBER" />
            <asp:BoundField DataField="ACCOUNT_STATUS" HeaderText="ACCOUNT_STATUS" />
            <asp:BoundField DataField="REMARKS" HeaderText="REMARKS" >
            <ItemStyle Width="50px" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
</asp:Content>
