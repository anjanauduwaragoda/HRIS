<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmRequestType.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmRequestType"
    EnableEventValidation="false" %>

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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <span>Request Type Details</span>
            <hr />
            <table>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label1" runat="server" Text="Request Type Name :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtRequestTypeName" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvRequestTypeName" runat="server" ErrorMessage="Request type name is required"
                            ControlToValidate="txtRequestTypeName" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                        <asp:FilteredTextBoxExtender ID="fteRequestTypeName" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Numbers,Custom"
                            ValidChars=" " TargetControlID="txtRequestTypeName">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label2" runat="server" Text="Description :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtDescription" runat="server" Width="250px" MaxLength="150" TextMode="MultiLine"></asp:TextBox>
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
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="125px">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ErrorMessage="Status is required"
                            ControlToValidate="ddlStatus" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" OnClick="btnSave_Click"
                            ValidationGroup="CGroup" />
                        <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="125px" OnClick="btnCancel_Click" />
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
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
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
                        <asp:ValidationSummary ID="vsCGroup" runat="server" ForeColor="Red" ValidationGroup="CGroup" />
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2">
                        <asp:HiddenField ID="hfRequestTypeId" runat="server" />
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
            <span>Request Types</span>
            <hr />
            <table style="margin: auto">
                <tr>
                    <td>
                        <asp:GridView ID="gvRequestType" runat="server" AutoGenerateColumns="False" Width="700px"
                            OnPageIndexChanging="gvRequestType_PageIndexChanging" OnRowDataBound="gvRequestType_RowDataBound"
                            OnSelectedIndexChanged="gvRequestType_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="REQUEST_TYPE_ID" HeaderText="Request Type Id" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn">
                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TYPE_NAME" HeaderText="Type Name">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="STATUS" HeaderText="Status">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
