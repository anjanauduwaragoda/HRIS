<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormEmployeeSelfTrainingEvaluation_Essay.aspx.cs"
    Inherits="GroupHRIS.TrainingAndDevelopment.WebFormEmployeeSelfTrainingEvaluation_Essay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
</head>
<body class="popupsearch">
    <form id="form1" runat="server">
    <div>
    <span>Training Evaluation - Essay Questions</span>
    <hr />
    </div>
    <div style="background-color: #aed6f1; width: 100%">
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
    <div id="questionDiv" runat="server">
        
    </div>
    <div>
         <table width="100%">
            <tr>
                <td style="text-align: left;">
                    <asp:Button ID="btnFinalized" runat="server" Text="Finalize" Width="100px" 
                        onclick="btnFinalized_Click" />
                </td>
                <td style="text-align: right;">
                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" 
                        onclick="btnSave_Click" />
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" colspan="2">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>

    <asp:HiddenField ID="hfEvaluationId" runat="server" />
    <asp:HiddenField ID="hfEmployeeId" runat="server" />
    <asp:HiddenField ID="hfIsPostEvaluation" runat="server" />
    <asp:HiddenField ID="hfTrainingId" runat="server" />

    </form>
    
</body>
</html>
