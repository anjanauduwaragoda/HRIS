<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmProficiencyScheme.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmProficiencySchemeC" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .cell1
        {
            width: 250px;
            text-align: right;
        }
        
        .cell2
        {
            width: 250px; /*text-align:right;*/
        }
        .innerCaption
        {
            width: 75px;
            text-align: right;
        }
        .innerInputCell
        {
            width: 190px;
        }
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
            Proficiency Scheme Details
            <hr />
            <table>
                <tr>
                    <td class="cell1">
                        <asp:Label ID="Label1" runat="server" Text="Name :"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                            ErrorMessage="Proficiency Scheme name is required " ForeColor="Red" ValidationGroup="vgProficiencyScheme">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="cell1">
                        <asp:Label ID="Label2" runat="server" Text="Remarks :"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescription" runat="server" Width="250px" TextMode="MultiLine"
                            MaxLength="150"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDescription"
                            ErrorMessage="Remark is required " ForeColor="Red" ValidationGroup="vgProficiencyScheme">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="cell1">
                        <asp:Label ID="Label3" runat="server" Text="Status :"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="256px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlStatus"
                            ErrorMessage="Status is required" ForeColor="Red" ValidationGroup="vgProficiencyScheme">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="cell1">
                    </td>
                    <td>
                        Rating
                        <hr style="width: 530px;" align="left" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="cell1">
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td class="innerCaption">
                                    <asp:Label ID="Label4" runat="server" Text=""> Rating :</asp:Label>
                                </td>
                                <td class="innerInputCell">
                                    <%--<asp:TextBox ID="TextBox3" runat="server" Width="170px"></asp:TextBox>--%>
                                    <asp:TextBox ID="txtRating" runat="server" MaxLength="1" Width="180px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ControlToValidate="txtRating" ID="rfvRating" runat="server"
                                        ErrorMessage="Rating is required" ValidationGroup="vgProficiencyLevels" ForeColor="Red">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="innerCaption">
                                    <asp:Label ID="Label5" runat="server" Text=""> Status :</asp:Label>
                                </td>
                                <td class="innerInputCell">
                                    <asp:DropDownList ID="ddlLevelStatus" runat="server" Width="186px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ControlToValidate="ddlLevelStatus" ID="rfvStatus" runat="server"
                                        ErrorMessage="Status is required" ValidationGroup="vgProficiencyLevels" ForeColor="Red">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="innerCaption" style="vertical-align: top;">
                                    <asp:Label ID="Label6" runat="server" Text="">Weight :</asp:Label>
                                </td>
                                <td class="innerInputCell" style="vertical-align: top;">
                                    <asp:TextBox ID="txtWeight" runat="server" Width="180px"></asp:TextBox>
                                </td>
                                <td style="vertical-align: top;">
                                    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtWeight"
                                runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                                ValidationGroup="vgProficiencyLevels" ForeColor="Red">*</asp:RegularExpressionValidator>--%>
                                    <asp:FilteredTextBoxExtender ID="fteWeight" runat="server" FilterType="Numbers, Custom"
                                        ValidChars="." TargetControlID="txtWeight">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ControlToValidate="txtWeight" ID="rfvWeight" runat="server"
                                        ErrorMessage="Weight is required" ValidationGroup="vgProficiencyLevels" ForeColor="Red">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="innerCaption" style="vertical-align: top;">
                                </td>
                                <td class="innerInputCell" style="vertical-align: top;">
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="innerCaption" style="vertical-align: top;">
                                    <asp:Label ID="Label8" runat="server" Text="Description :"></asp:Label>
                                </td>
                                <td class="innerInputCell" style="vertical-align: top;">
                                    <asp:TextBox ID="txtLevelDescription" runat="server" TextMode="MultiLine" MaxLength="100"
                                        Width="180px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="vgProficiencyLevels"
                                        ControlToValidate="txtLevelDescription" ForeColor="Red" runat="server" ErrorMessage="Description is required">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="innerCaption" style="vertical-align: top;">
                                    <asp:Label ID="Label7" runat="server" Text="Remarks :"></asp:Label>
                                </td>
                                <td class="innerInputCell" style="vertical-align: top;">
                                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" MaxLength="75" Width="180px"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="innerCaption" style="vertical-align: top;">
                                </td>
                                <td class="innerInputCell" style="vertical-align: top;">
                                </td>
                                <td>
                                </td>
                                <td class="innerCaption" style="vertical-align: top;">
                                </td>
                                <td class="innerInputCell" style="vertical-align: top;">
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr align="left">
                                <td colspan="2" align="left">
                                    <asp:Button ID="btnProficiencyLevelSave" runat="server" Text="Add" Width="62px" ValidationGroup="vgProficiencyLevels"
                                        Style="height: 26px; float: left; margin-top: 16px;" OnClick="proficiencyLevel_btnSave_Click" />
                                    <asp:Button ID="Button4" runat="server" Text="Reset" Width="62px" Style="height: 26px;
                                        float: left; margin-top: 16px;" OnClick="ProficiencyScheme_btnClear_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                    <asp:ValidationSummary ID="ProficiencyLevelsSummery" DisplayMode="BulletList" ForeColor="Red"
                                        runat="server" ValidationGroup="vgProficiencyLevels" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <br />
                        <asp:GridView ID="ProficiencyLevelGridView" runat="server" Style="width: 530px;"
                            AllowPaging="True" OnPageIndexChanging="ProficiencyLevelGrid_PageIndexChanging"
                            OnRowDataBound="ProficiencyLevelGrid_RowDataBound" AutoGenerateColumns="False"
                            OnSelectedIndexChanged="ProficiencyLevelGridView_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="RATING" HeaderText="Rating" />
                                <asp:BoundField DataField="WEIGHT" HeaderText="Weight" />
                                <asp:BoundField DataField="REMARKS" HeaderText="Remarks" ItemStyle-CssClass="hiddencol"
                                    HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                            </Columns>
                        </asp:GridView>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="cell1">
                    </td>
                    <td>
                        <hr style="width: 530px" align="left" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="cell1">
                    </td>
                    <td style="padding-left: 95px;">
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td style="padding-left: 4px;">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" ValidationGroup="vgProficiencyScheme"
                            Style="height: 26px; float: left;" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="125px" Style="height: 26px;
                            float: left;" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td class="style2">
                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgProficiencyScheme"
                            ForeColor="Red" DisplayMode="BulletList" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td colspan="2">
                        Proficiency Schemes
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:GridView ID="proficiancySchemeGrid" runat="server" AllowPaging="True" OnPageIndexChanging="ProficiancySchemeGrid_PageIndexChanging"
                            OnRowDataBound="ProficiancySchemeGrid_RowDataBound" AutoGenerateColumns="False"
                            OnSelectedIndexChanged="ProficiancySchemeGrid_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="SCHEME_ID" HeaderText="Scheme Id" ItemStyle-CssClass="hiddencol"
                                    HeaderStyle-CssClass="hiddencol" />
                                <asp:BoundField DataField="SCHEME_NAME" HeaderText="Scheme Name" />
                                <asp:BoundField DataField="REMARKS" HeaderText="Remarks" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HiddenField_selected_index" runat="server" />
            <asp:HiddenField ID="hf_selected_proficiencyScheme_id" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
