<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="WebFrmGoalGroups.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmGoalGroups" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        .style1
        {
            width: 253px;
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <span>Goal Group Details</span>
            <hr />
            <table>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label1" runat="server" Text="Goal Group Name :"></asp:Label>
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txtName" runat="server" Width="250px" MaxLength="150"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="fteName" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom"
                            ValidChars=" " TargetControlID="txtName">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                            ErrorMessage="Goal Group name is required " ForeColor="Red" ValidationGroup="vgGoalgroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label2" runat="server" Text="Description :"></asp:Label>
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txtDescription" runat="server" Width="250px" TextMode="MultiLine"
                            MaxLength="200"></asp:TextBox>
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
                        <asp:Label ID="Label3" runat="server" Text="Status :"></asp:Label>
                    </td>
                    <td class="style1">
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="256px">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlStatus"
                            ErrorMessage="Status is required" ForeColor="Red" ValidationGroup="vgGoalgroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="style1" style="text-align:center;" align="center">
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
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
                    <td class="style1">
                        <br />
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" ValidationGroup="vgGoalgroup"
                            Style="height: 26px; float: left; margin-right: 3px;" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="125px" Style="height: 26px;
                            float: left;" OnClick="btnClear_Click" />
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
                    <td colspan="4">
                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgGoalgroup"
                            ForeColor="Red" DisplayMode="BulletList" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td colspan="3">
                        Goal Groups
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center" class="style4">
                        <div class="stylediv">
                            <asp:GridView ID="GoalGroupGrid" runat="server" AllowPaging="True" OnPageIndexChanging="GoalGroupGrid_PageIndexChanging"
                                OnRowDataBound="GoalGroupGrid_RowDataBound" AutoGenerateColumns="False" OnSelectedIndexChanged="GoalGroupGrid_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="GOAL_GROUP_ID" HeaderText="Goal Group Id" HeaderStyle-CssClass="hideGridColumn"
                                        ItemStyle-CssClass="hideGridColumn" />
                                    <asp:BoundField DataField="GROUP_NAME" HeaderText="Goal Group Name" ItemStyle-Width="260px" />
                                    <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" ItemStyle-Width="350px" />
                                    <asp:BoundField DataField="STATUS_CODE1" HeaderText="Status" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        &nbsp;
                    </td>
                    <td class="styleMainTbRightTD">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
