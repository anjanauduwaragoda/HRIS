<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebReportViewer.aspx.cs" EnableEventValidation = "false"
    Inherits="GroupHRIS.Reports.WebReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/Stylepopup.css" rel="stylesheet" type="text/css" />
   
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1"  EnablePageMethods="true" EnablePartialRendering="true" 
            runat="server">
        </asp:ScriptManager>
    </div>
    <div>
        <table style="width: 100%">
            <tr>
                <td style="width: 50%; text-align: left; padding: 10px">
                    <asp:Button ID="Button1" runat="server" Text="Run Report" OnClick="Button1_Click" />
                </td>
                <td style="width: 50%; text-align: right; padding: 10px">
                    <asp:Button ID="Button2" runat="server" Text=" << Back" OnClick="Button2_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div class="EmpProfilePhotoTD" align="center">
        <asp:Label ID="lblerror" runat="server" BorderColor="White" ForeColor="Red"></asp:Label>
    </div>
    <div>
        <%--<rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="#6699FF"
            ShowBackButton="true" DocumentMapCollapsed="True" Height="600px" AsyncRendering="False" onreportrefresh="rrvREXReport_ReportRefresh">
        </rsweb:ReportViewer>--%>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" BackColor="#6699FF" ShowPrintButton = "true" hasprintbutton="true" 
            ShowBackButton="true" DocumentMapCollapsed="True" Height="500px" AsyncRendering="False">
        </rsweb:ReportViewer>
    </div>
    </form>


</body>
</html>
