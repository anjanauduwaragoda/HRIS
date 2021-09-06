<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="AlertPrivileges.aspx.cs" Inherits="GroupHRIS.HRISAlerts.AlertPrivileges" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table>
        <tr>
            <td>
                <span style="font-weight: 700">Assign Alert privilages</span>
            </td>
        </tr>
    </table>
    <hr />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="margin: auto;">
                <tr>
                    <td>
                        HRIS Account Role :
                        <asp:DropDownList ID="ddlHrisRole" runat="server" AutoPostBack="True" Width="250"
                            OnSelectedIndexChanged="ddlHrisRole_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="HRIS Role is Required"
                            ForeColor="Red" ControlToValidate="ddlHrisRole" Text="*" ValidationGroup="Main"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="width: 700px;">
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="grdvReportGrid" AllowPaging="false" AutoGenerateColumns="false"
                            runat="server" OnRowDataBound="grdvReportGrid_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="PRIVILEGE_ID" HeaderText="PRIVILEGE ID" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="ALERT_ID" HeaderText="ALERT ID" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="ALERT_NAME" HeaderText="ALERT NAME" />
                                <asp:TemplateField HeaderText="STATUS">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblExclude" runat="server" Text="STATUS"></asp:Label><br />
                                        <asp:RadioButton ID="rdSelectAll" GroupName="hdr" AutoPostBack="true" Text="Grant All"
                                            runat="server" OnCheckedChanged="rdSelectAll_CheckedChanged" />
                                        <asp:RadioButton ID="rdDeselectAll" GroupName="hdr" AutoPostBack="true" Text="Revoke All"
                                            runat="server" OnCheckedChanged="rdDeselectAll_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:RadioButton ID="rdbtnShow" Text="Grant" GroupName="rdbtn" runat="server" oncheckedchanged="rdbtnShow_CheckedChanged" />
                                        &nbsp;
                                        <asp:RadioButton ID="rdbtnHide" Text="Revoke" GroupName="rdbtn" runat="server" oncheckedchanged="rdbtnHide_CheckedChanged" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>                        
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center;">
                        <br />
                        <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="Main" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center;">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <br />
                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: left;">
                        <asp:ValidationSummary ID="ValidationSummary1" ForeColor="Red" ValidationGroup="Main"
                            runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>