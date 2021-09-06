<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmExpenceCategory.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmExpenceCategory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <span>Expence Category Details</span><hr />
    <table>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                Category Name :
            </td>
            <td>
                <asp:TextBox ID="txtName" runat="server" Width="200px" MaxLength="150"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom, Numbers,LowercaseLetters, UppercaseLetters"
                            ValidChars="/,-,' '" runat="server" TargetControlID="txtName">
                        </asp:FilteredTextBoxExtender>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Expense Category name is required"
                    Text="*" ForeColor="Red" ValidationGroup="exCategory" ControlToValidate="txtName"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                Description :
            </td>
            <td>
                <asp:TextBox ID="txtDescription" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                Status :
            </td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server" Width="205px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Status is required"
                    Text="*" ForeColor="Red" ValidationGroup="exCategory" ControlToValidate="ddlStatus"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" OnClick="btnSave_Click"
                    ValidationGroup="exCategory" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="exCategory" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="../Images/ProBar/720.GIF" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
    </table>
    <br />
    <span>Expence Category </span>
    <hr />
    <table width="100%">
        <tr>
            <td align="center">
                <asp:GridView ID="grdExpenceCategory" runat="server" AutoGenerateColumns="false"
                    AllowPaging="true" OnPageIndexChanging="grdExpenceCategory_PageIndexChanging"
                    OnRowDataBound="grdExpenceCategory_RowDataBound" OnSelectedIndexChanged="grdExpenceCategory_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="EXPENSE_CATEGORY_ID" HeaderText="Expence Category Id"
                            HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Category Name" />
                        <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                        <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="hfglId" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            </td>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
