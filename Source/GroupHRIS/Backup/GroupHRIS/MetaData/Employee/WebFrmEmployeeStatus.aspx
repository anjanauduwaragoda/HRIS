<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmEmployeeStatus.aspx.cs" Inherits="GroupHRIS.MetaData.Employee.WebFrmEmployeeStatus" %>

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
            <span style="font-weight: 700;">Employee Status</span><br />
            <hr />
            <br />
            <table style="margin: auto;">
                <tr>
                    <td style="text-align: right;">
                        Status Code :
                    </td>
                    <td>
                        <asp:Label ID="StatusCodeLabel" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Status Name :
                    </td>
                    <td>
                        <asp:TextBox ID="StatusNameTextBox" runat="server" ValidationGroup="grp1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="StatusNameRequiredFieldValidator" ControlToValidate="StatusNameTextBox"
                            ValidationGroup="grp1" runat="server" ErrorMessage="Status Name is Required"
                            ForeColor="Red" Text="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td style="text-align: left;">
                        <asp:Button ID="ToggleButton" runat="server" Text="Save" ValidationGroup="grp1" OnClick="ToggleButton_Click" />
                        <asp:Button ID="ClearButton" runat="server" Text="Clear" OnClick="ClearButton_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="StatusLabel" runat="server"> </asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" ForeColor="Red" runat="server" ValidationGroup="grp1" />
                    </td>
                </tr>
            </table>
            <hr />
            <br />
            <asp:GridView ID="SearchResultsGridView" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
                AllowPaging="true" OnRowDataBound="SearchResultsGridView_RowDataBound" OnSelectedIndexChanged="SearchResultsGridView_SelectedIndexChanged"
                OnPageIndexChanging="SearchResultsGridView_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status Code " SortExpression="STATUS_CODE" />
                    <asp:BoundField DataField="DESCRIPTION" HeaderText=" Description " SortExpression="DESCRIPTION" />
                </Columns>
                <EmptyDataTemplate>
                    No Data Returned
                </EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
