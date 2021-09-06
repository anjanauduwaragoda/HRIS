<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmTrainerCompetency.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainerCompetency" %>

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
    <span>Trainer Competency Details</span><hr />
    <table>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                Competency Area :
            </td>
            <td>
                <asp:TextBox ID="txtCompetencyArea" runat="server" Width="200px"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" FilterType="Custom, Numbers,LowercaseLetters, UppercaseLetters"
                            ValidChars="/,-,' '" runat="server" TargetControlID="txtCompetencyArea">
                        </asp:FilteredTextBoxExtender>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Competency Area is required"
                    Text="*" ControlToValidate="txtCompetencyArea" ForeColor="Red" ValidationGroup="competency"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                Description :
            </td>
            <td>
                <asp:TextBox ID="txtDescription" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                Status :
            </td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server" Width="205px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Status is required"
                    Text="*" ControlToValidate="ddlStatus" ForeColor="Red" ValidationGroup="competency"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" OnClick="btnSave_Click"
                    ValidationGroup="competency" />
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
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="competency" />
            </td>
        </tr>
        <tr><td></td><td><asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                        <ProgressTemplate>
                                            <img src="../Images/ProBar/720.GIF" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress></td></tr>
    </table>
    <span>Trainer Competencies </span>
    <hr />
    <table style="width: 100%;">
        <tr>
            <td align="center">
                <asp:GridView ID="grdTrainerCompetency" runat="server" AutoGenerateColumns="false"
                    AllowPaging="true" OnPageIndexChanging="grdTrainerCompetency_PageIndexChanging"
                    OnRowDataBound="grdTrainerCompetency_RowDataBound" OnSelectedIndexChanged="grdTrainerCompetency_SelectedIndexChanged"
                    PageSize="8">
                    <Columns>
                        <asp:BoundField DataField="COMPETENCY_ID" HeaderText="Competency Id" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="NAME" HeaderText="Competency Area" />
                        <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                        <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                    </Columns>
                    <EmptyDataTemplate>
                        NO COMPETENCY FOUND.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:HiddenField ID="hfglId" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            </td>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
