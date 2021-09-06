<%@ Page Title="" Language="C#" MasterPageFile="~/Reports/ReportViewers/ReportMaster.Master"
    AutoEventWireup="true" CodeBehind="rptvTrainingSchedule.aspx.cs" Inherits="GroupHRIS.Reports.ReportViewers.rptvTrainingSchedule" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <rsweb:ReportViewer id="rptViewer" runat="server" width="100%" height="550px">
    </rsweb:ReportViewer>
</asp:Content>
