<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmRole.aspx.cs" Inherits="GroupHRIS.MetaData.Employee.WebFrmRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            font-size: large;
            margin: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span style="font-weight: 700;">Employee Role</span><br />
            <hr />
            <br />
            <table style="margin: auto;">
                <tr>
                    <td style="text-align: right;">
                        Employee Role ID :
                    </td>
                    <td>
                        <asp:Label ID="EmployeeRoleIDLabel" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Role Name :
                    </td>
                    <td>
                        <asp:TextBox ID="RoleNameTextBox" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RoleNameTRequiredFieldValidator" runat="server" 
                            ErrorMessage="Role Name is Required" Text="*" ForeColor="Red" ControlToValidate="RoleNameTextBox" ValidationGroup="sVal"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Description :
                    </td>
                    <td>
                        <asp:TextBox ID="DescriptionTextBox" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Status :
                    </td>
                    <td>
                        <asp:DropDownList ID="StatusDropDownList" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="StatusRequiredFieldValidator" runat="server" 
                            ErrorMessage="Status is Required" Text="*" ForeColor="Red" ControlToValidate="StatusDropDownList" ValidationGroup="sVal"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td style="text-align: left;">
                        <asp:Button ID="ToggleButton" runat="server" Text="Save" ValidationGroup="sVal" OnClick="ToggleButton_Click" />
                        <asp:Button ID="ClearButton" runat="server" Text="Clear" OnClick="ClearButton_Click" />
                    </td>
                </tr>
                <tr>
                    <td />
                    <td />
                    <asp:Label ID="StatusLabel" runat="server"> </asp:Label>
                    <asp:ValidationSummary ID="RoleValidationSummary" ForeColor="Red" ValidationGroup="sVal" runat="server" />
                </tr>
            </table>
            <hr />
            <br />
            <asp:GridView ID="SearchResultsGridView" runat="server" Style="margin: auto;" AutoGenerateColumns="False"
                AllowPaging="true" OnRowDataBound="SearchResultsGridView_RowDataBound" OnSelectedIndexChanged="SearchResultsGridView_SelectedIndexChanged"
                OnPageIndexChanging="SearchResultsGridView_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="ROLE_ID" HeaderText=" Role ID " SortExpression="ROLE_ID" />
                    <asp:BoundField DataField="ROLE_NAME" HeaderText=" Role Name " SortExpression="ROLE_NAME" />
                    <asp:BoundField DataField="DESCRIPTION" HeaderText=" Description " SortExpression="DESCRIPTION" />
                    <asp:BoundField DataField="STATUS" HeaderText=" Status " SortExpression="STATUS" />
                </Columns>
                <EmptyDataTemplate>
                    No Data Returned
                </EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>