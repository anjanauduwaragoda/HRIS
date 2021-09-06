<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmViewMCQquestions.aspx.cs"
    Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmViewMCQquestions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
    <script type="text/javascript">
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sEvalId, sEvalName) {
            var ctl = document.getElementById("hfCaller").value;

            document.getElementById("hfVal").value = sEvalId;
            document.getElementById("hfEvalName").value = sEvalName;
            // alert(hfEvalName);
            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }
    </script>
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
                <span>Multiple Choice Question</span>
                <hr />
                <div style="width: 850px">
                    <asp:Label ID="lbltbl" runat="server"></asp:Label></div>
                <br />
                <div style="width: 850px; text-align: center;">
                    <asp:Button ID="btnFinalize" runat="server" Text="Finalize" Width="100px" 
                        onclick="btnFinalize_Click" /><asp:Button
                        ID="btnSave" runat="server" Text="Save" Width="100px" 
                        onclick="btnSave_Click" /></div>
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
        <asp:HiddenField ID="hfEvalName" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
    </div>
    </form>
</body>
</html>
