<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmAssessmentGroup.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmAssessmentGroup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfSelectedAssessmentGroup" runat="server" />
            <span>Assessment Group Details</span><span style="font-weight: 700;"> </span>
            <hr />
            <table style="margin: auto; min-width: 700px;">
                <tr>
                    <td style="text-align: left;">
                        <table>
                            <tr>
                                <td style="text-align: right; vertical-align: text-top;">
                                    Group Name
                                </td>
                                <td style="vertical-align: text-top;">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGroupName" MaxLength="25" Width="200" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Group Name is Required"
                                        ForeColor="Red" Text="*" ValidationGroup="Main" ControlToValidate="txtGroupName"></asp:RequiredFieldValidator>
                                    <%--<asp:FilteredTextBoxExtender TargetControlID="txtGroupName" FilterType="LowercaseLetters,LowercaseLetters" ValidChars=" ,." InvalidChars="`,~,!,@,#,$,%,^,&,*,<,>,/,'" ID="FilteredTextBoxExtender1" runat="server">
                                    </asp:FilteredTextBoxExtender>--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; vertical-align: top;">
                                    Description
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDescription" Width="200" Style="height: 100px;" TextMode="MultiLine"
                                        runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; vertical-align: top;">
                                    Status
                                </td>
                                <td style="vertical-align: top;">
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStatus" Width="200" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Status is Required"
                                        ForeColor="Red" Text="*" ValidationGroup="Main" ControlToValidate="ddlStatus"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" ValidationGroup="Main" Text="Save" Width="98"
                                        OnClick="btnSave_Click" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" Width="99" OnClick="btnClear_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <table>
                            <tr>
                                <td align="left">
                                    Employee Roles
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grdvEmployeeRoles" PageSize="5" AutoGenerateColumns="false" Style="width: 300px;"
                                        AllowPaging="true" runat="server" OnPageIndexChanging="grdvEmployeeRoles_PageIndexChanging"
                                        OnRowDataBound="grdvEmployeeRoles_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ROLE_ID" HeaderText="Role ID" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn" />
                                            <asp:BoundField DataField="GROUP_ID" HeaderText="Group ID" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn" />
                                            <asp:BoundField DataField="GROUP_NAME" HeaderText="Group Name" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn" />
                                            <asp:BoundField DataField="ROLE_NAME" HeaderText="Role Name" />
                                            <asp:TemplateField HeaderText="Include">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkInclude" runat="server" AutoPostBack="true" OnCheckedChanged="chkInclude_CheckedChanged" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:UpdateProgress ID="upProgressBar" runat="server" style="margin: auto; width: auto">
                            <ProgressTemplate>
                                <img src="/Images/ProBar/720.GIF" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                        <br />
                        <table style="margin:auto;">
                            <tr>
                                <td style="text-align:left;"><asp:Label ID="lblProfiles" runat="server" Font-Size="10pt" ForeColor="Black"></asp:Label></td>
                            </tr>
                        </table>                        
                    </td>
                </tr>
            </table>
            <table style="margin: auto;">
                <tr>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Main" ForeColor="Red"
                            runat="server" />
                    </td>
                </tr>
            </table>
                        Assessment Groups
                        <hr />
            <table style="margin: auto; min-width: 700px;">
                <tr>
                    <td>
                        <asp:GridView ID="grdvAssessmentGroup" AutoGenerateColumns="false" AllowPaging="true"
                            PageSize="10" runat="server" OnPageIndexChanging="grdvAssessmentGroup_PageIndexChanging"
                            OnRowDataBound="grdvAssessmentGroup_RowDataBound" OnSelectedIndexChanged="grdvAssessmentGroup_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="GROUP_ID" HeaderText=" Group ID " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="GROUP_NAME" HeaderText=" Group Name " />
                                <asp:BoundField DataField="DESCRIPTION" HeaderText=" Description " />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status Code " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="STATUS_CODE_VALUE" HeaderText=" Status " />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
