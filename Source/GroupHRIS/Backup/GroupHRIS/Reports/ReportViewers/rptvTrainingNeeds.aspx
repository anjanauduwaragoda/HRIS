<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/ReportViewers/ReportMaster.Master" AutoEventWireup="true" CodeBehind="rptvTrainingNeeds.aspx.cs" Inherits="GroupHRIS.Reports.ReportViewers.rptvTrainingNeeds" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--<asp:Button ID="btnGenerateReport" runat="server" Text="Generate Report" 
        onclick="btnGenerateReport_Click" />--%>
    <rsweb:ReportViewer ID="rptViewer" runat="server"  Width="100%" Height="550px">
    </rsweb:ReportViewer>
    
</asp:Content>
