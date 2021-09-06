<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="webFrmEmployeeDesignation.aspx.cs" Inherits="GroupHRIS.Employee.webFrmEmployeeDesignation" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />
<span>Employee Designations</span>
<hr />
    <table style="width: 476px">
        <tr style="width: 476px">
            <td>
            </td>
            <td>
                <asp:HiddenField ID="hfDesignationId" runat="server"  />
            </td>
            <td>
            </td>
        </tr>
        <tr style="width: 476px">
            <td style="width: 200px; text-align: right">
                <asp:Label ID="Label1" runat="server" Text="Company "></asp:Label>
            </td>
            <td style="width: 256px; text-align: right">
                <asp:DropDownList ID="ddlCompany" runat="server" Width="256px" 
                    OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" 
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td style ="width:20px;text-align:left">
                <asp:RequiredFieldValidator ID="rfvCompanyId" runat="server" ErrorMessage="Company Id is required"
                    ControlToValidate="ddlCompany" ValidationGroup="vgDesignation" 
                    ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr style="width: 476px; text-align: right">
            <td style="width: 200px">
                <asp:Label ID="Label2" runat="server" Text="Designation Name"></asp:Label>
            </td>
            <td style="width: 256px">
                <asp:TextBox ID="txtDesignation" runat="server" Width="250px" MaxLength="110" ></asp:TextBox>
            </td>
            <td style ="width:20px;text-align:left" >
                <asp:RequiredFieldValidator ID="rfvDesignationName" runat="server" ErrorMessage="Designation Name is required"
                    ControlToValidate="txtDesignation" ValidationGroup="vgDesignation" 
                    ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr style="width: 476px; text-align: right">
            <td style="width: 200px">
                <asp:Label ID="Label3" runat="server" Text="Remarks"></asp:Label>
            </td>
            <td style="width: 256px">
                <asp:TextBox ID="txtRemarks" runat="server" Width="250px" MaxLength="50"></asp:TextBox>
            </td>
            <td style ="width:20px">
            </td>
        </tr>
        <tr style="width: 476px; text-align: right">
            <td style="width: 200px">
                <asp:Label ID="Label4" runat="server" Text="Status"></asp:Label>
            </td>
            <td style="width: 256px">
                <asp:DropDownList ID="ddlStatus" runat="server" Width="256px">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style ="width:20px;text-align:left" >
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlStatus"
                    ErrorMessage="Status is required" ValidationGroup="vgDesignation" 
                    ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr style="width: 476px">
            <td style="width: 200px">
            </td>
            <td style="width: 256px">
            </td>
            <td style="width: 20px">
            </td>
        </tr>
        <tr style="width: 476px">
            <td style="width: 200px">
            </td>
            <td style="width: 256px">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" 
                    onclick="btnSave_Click" ValidationGroup="vgDesignation" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="125px" 
                    onclick="btnCancel_Click" />
            </td>
            <td style ="width:20px">
            </td>
        </tr>
        <tr style="width: 476px">
            <td>
            </td>
            <td colspan="2">
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <asp:ValidationSummary ID="vsDesignation" runat="server" 
        ValidationGroup="vgDesignation" ForeColor="Red" />
    <br />
    <span>Designation Details</span>
    <hr />
    <br />
    <asp:GridView ID="gvDesignations" runat="server" Width="470px" AutoGenerateColumns="False"
        AllowPaging="true"  onpageindexchanging="gvDesignations_PageIndexChanging" 
        onrowdatabound="gvDesignations_RowDataBound" onselectedindexchanged="gvDesignations_SelectedIndexChanged"
        >  
        <Columns>
            <asp:BoundField DataField="DESIGNATION_ID" HeaderText="DESIGNATION_ID" />
            <asp:BoundField DataField="DESIGNATION_NAME" HeaderText="DESIGNATION_NAME" />
            <asp:BoundField DataField="STATUS" HeaderText="STATUS" />
            <asp:BoundField DataField="COMPANY" HeaderText="COMPANY" />            
        </Columns>
    </asp:GridView>
</asp:Content>
