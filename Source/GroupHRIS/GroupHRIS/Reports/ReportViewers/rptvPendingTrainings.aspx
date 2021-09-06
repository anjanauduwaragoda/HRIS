<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/ReportViewers/ReportMaster.Master" AutoEventWireup="true" CodeBehind="rptvPendingTrainings.aspx.cs" Inherits="GroupHRIS.Reports.ReportViewers.rptvPendingTrainings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <rsweb:ReportViewer ID="rptViewer" runat="server"  Width="100%" Height="550px">
    </rsweb:ReportViewer>
</asp:Content>
