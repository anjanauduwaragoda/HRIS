<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="webFrmAttendanceUploading.aspx.cs" Inherits="GroupHRIS.Attendance.webFrmAttendanceUploading" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <table>
        <tr>
            <td style="text-align: right; width: 300">
                <asp:Label ID="Label1" runat="server" Text="Company"></asp:Label>
            </td>
            <td style="text-align: left; width: 300">
                <asp:DropDownList ID="dlCompany" runat="server" Width="300px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ErrorMessage="Company is required"
                    ControlToValidate="dlCompany" ForeColor="Red" ValidationGroup="gUpload">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 300">
                <asp:Label ID="Label2" runat="server" Text="Select File"></asp:Label>
            </td>
            <td style="text-align: left; width: 300">
                <asp:FileUpload ID="FileUploader" runat="server" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 300">
            </td>
            <td style="text-align: left; width: 300">
                <asp:Button ID="Button1" runat="server" Text="Upload" OnClick="btnUpload_Click" Width="100px"
                    ValidationGroup="gUpload" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" Width="100px" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            </td>
            <td>
            </td>
        </tr>        
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Height="350px" Width="509px"  onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="gUpload" />   
</asp:Content>
