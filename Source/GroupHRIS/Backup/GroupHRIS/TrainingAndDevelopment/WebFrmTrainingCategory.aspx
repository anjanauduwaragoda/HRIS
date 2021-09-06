<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmTrainingCategory.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainingCategory" %>

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
            <span>Training Category Details</span>
            <hr />
            <table>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label1" runat="server" Text="Category Name :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtCategoryName" runat="server" Width="250px" MaxLength="150"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvCategoryName" runat="server" ErrorMessage="Category name is required"
                            ControlToValidate="txtCategoryName" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                        <asp:FilteredTextBoxExtender ID="fteCategoName" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Numbers,Custom"
                            ValidChars=" " TargetControlID="txtCategoryName">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label2" runat="server" Text="Description :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtDescription" runat="server" Width="250px" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
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
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Status is required"
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
                    <td class="styleTableCell2" style="text-align: left" colspan="4">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td colspan="4" style="text-align: left">
                        <asp:ValidationSummary ID="vsCGroup" runat="server" ForeColor="Red" ValidationGroup="CGroup"
                            Style="text-align: left" />
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2">
                        <asp:HiddenField ID="hfCategoryId" runat="server" />
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
                        <asp:HiddenField ID="hfPrevStatus" runat="server" />
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
            <span>Training Categories</span>
            <hr />
            <table style="margin: auto">
                <tr>
                    <td>
                        <asp:GridView ID="gvTrainingCategory" runat="server" AutoGenerateColumns="False"
                            Width="700px" OnPageIndexChanging="gvTrainingCategory_PageIndexChanging" OnRowDataBound="gvTrainingCategory_RowDataBound"
                            OnSelectedIndexChanged="gvTrainingCategory_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="TRAINING_CATEGORY_ID" HeaderText="Training Category" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn">
                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Category Name">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CATEGORY_DESCRIPTION" HeaderText="Description">
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
