<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmEmployeeType.aspx.cs" Inherits="GroupHRIS.MetaData.Employee.WebFrmEmployeeType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: large;
            margin: auto;
        }
    </style>
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span style="font-weight: 700;">Employee type</span><br />
            <hr />
            <br />
            <table style="margin: auto;">
                <tr>
                    <td style="text-align: right;">
                        Employee Type ID :
                    </td>
                    <td>
                        <asp:Label ID="EmployeeTypeIDlbl" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Type Name :
                    </td>
                    <td>
                        <asp:TextBox ID="TypeNameTextBox" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="TypeNameRequiredFieldValidator" ControlToValidate="TypeNameTextBox"
                            Text="*" ForeColor="Red" ValidationGroup="grp1" runat="server" ErrorMessage="Type Name is Required"></asp:RequiredFieldValidator>
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
                    <td>
                    </td>
                    <td style="text-align: left;">
                        <asp:Button ID="ToggleButton" runat="server" Text="Save" OnClick="ToggleButton_Click"
                            ValidationGroup="grp1" />
                        <asp:Button ID="ClearButton" runat="server" Text="Clear" OnClick="ClearButton_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td style="text-align: center;">
                        <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="grp1" ForeColor="Red"
                            runat="server" />
                        <asp:Label ID="StatusLabel" runat="server"> </asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <hr />
            <asp:GridView ID="SearchResultsGridView" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
                AllowPaging="True" OnRowDataBound="SearchResultsGridView_RowDataBound" OnSelectedIndexChanged="SearchResultsGridView_SelectedIndexChanged"
                OnPageIndexChanging="SearchResultsGridView_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="EMP_TYPE_ID" HeaderText=" EMPLOYEE TYPE ID " SortExpression="EMP_TYPE_ID" />
                    <asp:BoundField DataField="TYPE_NAME" HeaderText=" TYPE NAME " SortExpression="TYPE_NAME" />
                    <asp:BoundField DataField="DESCRIPTION" HeaderText=" DESCRIPTION " SortExpression="DESCRIPTION" />
                </Columns>
                <EmptyDataTemplate>
                    No Data Returned
                </EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>