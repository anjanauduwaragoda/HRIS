<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmTrainingSubcategory.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainingSubcategory"
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
            <span>Training Subcategory Details</span>
            <hr />
            <br />
            <table>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label1" runat="server" Text="Training Category :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:DropDownList ID="ddlTrainingCategory" runat="server" Width="256px">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvTrainingCategory" runat="server" ErrorMessage="Training category is required"
                            ControlToValidate="ddlTrainingCategory" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                        <%--<asp:FilteredTextBoxExtender ID="fteCategoName" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Numbers,Custom"
                    ValidChars=" " TargetControlID="txtCategoryName">
                </asp:FilteredTextBoxExtender>--%>
                    </td>
                    <td class="styleTableCell6">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label2" runat="server" Text="Subcategory Name :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtSubcategory" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvSubcategory" runat="server" ErrorMessage="Subcategory is required"
                            ControlToValidate="txtSubcategory" ForeColor="Red" ValidationGroup="CGroup">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                        <asp:FilteredTextBoxExtender ID="fteSubCatName" runat="server" FilterType="LowercaseLetters, UppercaseLetters,Numbers,Custom"
                            ValidChars=" " TargetControlID="txtSubcategory">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td class="styleTableCell6">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label3" runat="server" Text="Description :"></asp:Label>
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
                    <td class="styleTableCell6">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label4" runat="server" Text="Status :"></asp:Label>
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
                    <td class="styleTableCell6">
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
                    <td class="styleTableCell6">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td colspan="5" style="text-align: left">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td colspan="5">
                        <asp:ValidationSummary ID="CGroup" runat="server" ForeColor="Red" ValidationGroup="CGroup" />
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2">
                        <asp:HiddenField ID="hfSubcategoryId" runat="server" />
                        <td class="styleTableCell3">
                        </td>
                        <td class="styleTableCell4">
                        </td>
                        <td class="styleTableCell5">
                        </td>
                        <td class="styleTableCell6">
                        </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2">
                        <asp:HiddenField ID="hfInactiveCategoryId" runat="server" />
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                    <td class="styleTableCell6">
                    </td>
                </tr>
            </table>
            <br />
            <span>Training Subcategories</span>
            <hr />
            <table style="margin: auto">
                <tr>
                    <td>
                        <asp:GridView ID="gvTrainingSubcategory" runat="server" AutoGenerateColumns="False"
                            Width="700px" OnPageIndexChanging="gvTrainingSubcategory_PageIndexChanging" OnRowDataBound="gvTrainingSubcategory_RowDataBound"
                            OnSelectedIndexChanged="gvTrainingSubcategory_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="CATEGORY_ID" HeaderText="Category Id" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn">
                                    <HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TYPE_ID" HeaderText="Type Id" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Category Name">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TYPE_NAME" HeaderText="Subcategory Name">
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
