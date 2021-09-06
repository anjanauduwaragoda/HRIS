<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="webFrmLeaveScheme.aspx.cs" Inherits="GroupHRIS.MetaData.webFrmLeaveScheme" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="styleMainTb">
                <tr>
                    <td colspan="4">
                        <span style="font-weight: 700">Leave Scheme Details</span> &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        &nbsp;
                    </td>
                    <td class="styleMainTbRightTD" colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Leave Scheme Name :
                    </td>
                    <td class="styleMainTbRightTD" colspan="3">
                        <asp:TextBox ID="txtSchemeName" runat="server" Width="250px" Height="20px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfShemeName" runat="server" ErrorMessage="Scheme Name is required."
                            ForeColor="Red" ValidationGroup="vgLeaveSchemeInfo" ControlToValidate="txtSchemeName">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Status :
                    </td>
                    <td class="styleMainTbRightTD" colspan="3">
                        <asp:DropDownList ID="ddlStatus" runat="server" Height="20px" Width="250px">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="1">Active</asp:ListItem>
                            <asp:ListItem Value="0">InActive</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfStatus" runat="server" ErrorMessage="Status is required."
                            ForeColor="Red" ValidationGroup="vgLeaveSchemeInfo" ControlToValidate="ddlStatus">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td class="styleMainTbRightTD" colspan="3">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Leave Type :
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:DropDownList ID="LeaveSchemeTypeDropDownList" Width="250px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="styleMainTbRightTD" style="text-align: right;">
                    </td>
                    <td class="styleMainTbRightTD">
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbRightTD" style="text-align: right;">
                        Number of Days :
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:TextBox ID="NumberOfDaysTextBox" Width="250px" runat="server"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="NumberOfDaysFilteredTextBoxExtender" TargetControlID="NumberOfDaysTextBox"
                            FilterType="Numbers" runat="server">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Remarks :
                    </td>
                    <td class="styleMainTbRightTD" colspan="3">
                        <asp:TextBox ID="RemarksTextBox" Width="250px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="AddButton" runat="server" Width="100px" Text="Add" OnClick="AddButton_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td class="styleMainTbRightTD" colspan="3">
                        <asp:GridView ID="LeaveSchemeItemGridView" AllowPaging="true" runat="server" ToolTip="Remove"
                            OnRowDataBound="LeaveSchemeItemGridView_RowDataBound" OnSelectedIndexChanging="LeaveSchemeItemGridView_SelectedIndexChanging"
                            OnPageIndexChanging="LeaveSchemeItemGridView_PageIndexChanging">
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td class="styleMainTbRightTD" colspan="3">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        &nbsp;
                    </td>
                    <td class="styleMainTbRightTD" colspan="3">
                        <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="vgLeaveSchemeInfo"
                            Width="100px" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="100px" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        &nbsp;
                    </td>
                    <td class="styleMainTbRightTD" colspan="3">
                        <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        &nbsp;
                    </td>
                    <td class="styleMainTbRightTD" colspan="3">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="vgLeaveSchemeInfo" />
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        &nbsp;
                    </td>
                    <td class="styleMainTbRightTD" colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <div class="stylediv">
                            <asp:GridView ID="GridView1" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                OnPageIndexChanging="GridView1_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="LEAVE_SCHEME_ID" HeaderText="Scheme ID" />
                                    <asp:BoundField DataField="LEAVE_SCHEM_NAME" HeaderText="Scheme Name" />
                                    <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        &nbsp;
                    </td>
                    <td class="styleMainTbRightTD" colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        &nbsp;
                    </td>
                    <td class="styleMainTbRightTD" colspan="3">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>