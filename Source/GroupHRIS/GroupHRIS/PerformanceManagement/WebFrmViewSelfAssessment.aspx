<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmViewSelfAssessment.aspx.cs"
    Inherits="GroupHRIS.PerformanceManagement.WebFrmViewSelfAssessment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Self-Assessment</title>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            window.opener.displayData("op");
            childWindow = open(file, window, 'resizable=no,width=800,height=600,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

            document.getElementById("hfCaller").value = ctlName;
        }
        function changeScreenSize() {
            //_width = window.screen.availWidth - 10;
            _width = 950;
            _height = window.screen.availHeight - 20;

            window.moveTo(60, 10);
            window.resizeTo(_width, _height)
            window.focus();
        }

        //        function dosposeWindow() {
        //            DoPostBack();
        //            window.close();
        //        }

        function sendValueToParent() {
            try {
                var x = 'popup';
                window.opener.displayData(x);
                window.close();
            }
            catch (err) {
                alert(err.Message);
            }
        }


        function DoPostBack() {
            __doPostBack();
        }

    </script>
</head>
<body class="popupsearch">
    <div>
        <form id="form1" runat="server" style="overflow: auto">
        <span style="font-weight: 700">Self - Assessment</span>
        <hr />
        <br />
        <div style="background-color: #aed6f1; width: 850px">
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Assessment Instructions -
            <br />
            <ul>
                <li>Write your answers in simple sentences to convey your idea clearly and unambiguesly
                    to the reviewer.</li>
                <li>Answer all questions and provide number of answers required for each question.</li>
                <li>You can partialy complete answering questions and save answers without completing
                    the whole assessment in one step.</li>
                <li>After completing assessment you need to finalized and thereafter you are not allowed
                    to answer or modification of the provided answers.</li>
            </ul>
            <br />
        </div>
        <br />
        <div style="width: 850px">
            <asp:Label ID="lbltbl" runat="server" Text="Label"></asp:Label>
        </div>
        <br />
        <table width="850px">
            <tr>
                <td style="text-align: left;">
                    <asp:Button ID="btnFinalized" runat="server" Text="Finalize" Width="100px" OnClick="btnFinalized_Click" />
                </td>
                <td style="text-align: right;">
                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" OnClick="btnSave_Click" />
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" colspan="2">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        </form>
    </div>
</body>
</html>
