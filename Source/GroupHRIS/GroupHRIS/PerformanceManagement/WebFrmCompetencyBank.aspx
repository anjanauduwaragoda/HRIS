<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="WebFrmCompetencyBank.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmCompetencyBank" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hiddencol
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
            <span>Competency Details</span>
            <hr />
            <table>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label1" runat="server" Text="Competency Name :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtName" runat="server" Width="250px" MaxLength="150"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="ftName" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom"
                            ValidChars=" " TargetControlID="txtName">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtName"
                            ErrorMessage="Competency name is required " ForeColor="Red" ValidationGroup="vgCompetency">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label4" runat="server" Text="Competency Group :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:DropDownList ID="ddlCompetencyGroup" runat="server" Width="256px">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlCompetencyGroup"
                            ErrorMessage="Competency group is required " ForeColor="Red" ValidationGroup="vgCompetency">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label2" runat="server" Text="Description :"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtDescription" runat="server" Width="250px" TextMode="MultiLine"
                            MaxLength="500"></asp:TextBox>
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
                    <td class="styleTableCell2">
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="256px">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlStatus"
                            ErrorMessage="Status is required" ForeColor="Red" ValidationGroup="vgCompetency">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2" style="text-align: center;">
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
                    <td class="styleTableCell2">
                        <br />
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" ValidationGroup="vgCompetency"
                            OnClick="btnSave_Click" Style="float: left;" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="125px" OnClick="btnClear_Click"
                            Style="margin-right: 0; float: right;" />
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        &nbsp;
                    </td>
                    <td class="styleMainTbRightTD" colspan="4">
                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgCompetency"
                            ForeColor="Red" DisplayMode="BulletList" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Competencies
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <%--OnPageIndexChanging="AssessmentPurposeGrid_PageIndexChanging"
                        OnRowDataBound="AssessmentPurposeGrid_RowDataBound"  OnSelectedIndexChanged="AssessmentPurposeGrid_SelectedIndexChanged"--%>
                    <td colspan="3" align="center" class="style4">
                        <div class="stylediv">
                            <asp:GridView ID="gvCompetencyBank" runat="server" AllowPaging="True" Style="width: 800px;"
                                AutoGenerateColumns="False" PageSize="10" OnRowDataBound="gvCompetencyBank_RowDataBound"
                                OnSelectedIndexChanged="gvCompetencyBank_SelectedIndexChanged" OnPageIndexChanging="gvCompetencyBank_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="COMPETENCY_ID" HeaderText="Competency Id" ItemStyle-CssClass="hiddencol"
                                        HeaderStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="COMPETENCY_NAME" HeaderText="Competency Name" />
                                    <asp:BoundField DataField="COMPETENCY_GROUP_ID" HeaderText="Competency Group Id"
                                        ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="COMPETENCY_GROUP_NAME" HeaderText="Competency Group" />
                                    <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" ItemStyle-Width="300px" />
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
                    <td class="styleMainTbRightTD">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfCompetencyId" runat="server" ClientIDMode="Static" />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
