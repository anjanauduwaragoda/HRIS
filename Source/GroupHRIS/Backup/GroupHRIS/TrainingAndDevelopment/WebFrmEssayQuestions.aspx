<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmEssayQuestions.aspx.cs"
    Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmEssayQuestions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
<link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
<link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
<head runat="server">
    <title></title>
</head>
<body class="popupsearch">
    <form id="form1" runat="server">
    <div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <br />
                <span>Essay Question</span>
                <hr />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
