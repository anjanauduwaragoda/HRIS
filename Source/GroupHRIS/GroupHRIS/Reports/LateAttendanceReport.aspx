<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="LateAttendanceReport.aspx.cs" Inherits="GroupHRIS.Reports.LateAttendanceReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <span style="font-weight: 700">Late Attendance Report</span>
    <hr />
    <br />    
    <asp:ToolkitScriptManager runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="margin: auto;">
                <tr>
                    <td>Company</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList ID="ddlCompany" style="width:100%;" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        Date
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDate" style="width:100%;" placeholder="DD/MM/YYYY" runat="server"></asp:TextBox>
                        
                    </td>
                    <td><asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Text="*"
                            ForeColor="Red" ControlToValidate="txtDate" ErrorMessage="Date is Required" ValidationGroup="Main"></asp:RequiredFieldValidator>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtDate"
                            FilterType="Custom,Numbers" ValidChars="/" runat="server">
                        </asp:FilteredTextBoxExtender>
                        <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txtDate" Format="dd/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtDate"
                            runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                            ErrorMessage="Incorrect Date Format" Text="*" ForeColor="Red" ValidationGroup="Main"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td style="text-align:right;">
                        <asp:Button ID="btnGenerate" runat="server" onclick="btnGenerate_Click" 
                            Text="Generate Report" ValidationGroup="Main" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Main" ForeColor="Red"
                            runat="server" />
                    </td>
                </tr>
            </table>
            <table style="margin: auto;">
                <tr>
                    <td>
                        <rsweb:ReportViewer ID="ReportViewer1" Width="850" Height="700" runat="server">
                        </rsweb:ReportViewer>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>