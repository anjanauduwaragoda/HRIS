<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmEvaluationSearch.aspx.cs" EnableEventValidation="false"
    Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmEvaluationSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Training Search</title>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function changeScreenSize() {

            _width = 950;
            _height = window.screen.availHeight - 20;

            window.resizeTo(_width, _height)
            window.focus();
        }

        function sendValueToParent_() {
            var sRetVal = document.getElementById("hfEvalId").value;

            window.opener.getValueFromChild(sRetVal);
            //alert(sRetVal);
            window.close();
        }

        function sendValueToParent() {
            var sEvalId = document.getElementById("hfEvalId").value;
            var sEvalName = document.getElementById("hfProgram").value;
                               //   alert(sEvalName);
            window.opener.getValueFromChild(sEvalId, sEvalName);

            window.close();
        }
    </script>
</head>
<body class="popupsearch" onload="changeScreenSize()">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <table>
    <tr><td></td><td></td></tr>
    <tr><td></td><td></td></tr>
    <tr><td></td><td></td></tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                <asp:Label ID="lblEvaluationId" runat="server" ForeColor="Blue"></asp:Label>
            </td>
            <td class="styleTableCell2">
                <asp:Label ID="lblEvalName" runat="server" ForeColor="Blue"></asp:Label>
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
            </td>
            <td class="styleTableCell2">
                <asp:Button ID="btnSelect" runat="server" Text="<< Select" 
                    OnClientClick="sendValueToParent()" />
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1" colspan="4">
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td align="center">
                <asp:GridView ID="grdProEvalData" runat="server" AutoGenerateColumns="false" 
                    onrowdatabound="grdProEvalData_RowDataBound" 
                    onselectedindexchanged="grdProEvalData_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="EVALUATION_ID" HeaderText="Evaluation ID" />
                        <asp:BoundField DataField="RS_NAME" HeaderText="Rating scheme" />
                        <asp:BoundField DataField="PROGRAM_NAME" HeaderText="Training Program" />
                        <asp:BoundField DataField="EVALUATION_NAME" HeaderText="Evaluation Name" />
                        <asp:BoundField DataField="MCQ_INCLUDED" HeaderText="MCQ Include" />
                        <asp:BoundField DataField="EQ_INCLUDED" HeaderText="EQ Include" />
                        <asp:BoundField DataField="RQ_INCLUDED" HeaderText="RQ Include" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>

     <div style="text-align: center;">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
        <asp:HiddenField ID="hfEvalId" runat="server" />
        <asp:HiddenField ID="hfProgram" runat="server" />
    </div>
    </form>
</body>
</html>
