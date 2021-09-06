<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="true"
    CodeBehind="WebFrmSupervisorSelfAssessment.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmSupervisorSelfAssessment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Review Employee Self Assessment</title>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function changeScreenSize() {
            _width = 950;
            _height = window.screen.availHeight - 20;

            window.moveTo(200, 10);
            window.resizeTo(_width, _height)
            window.focus();
        }
    </script>
    <style type="text/css">
        input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
        
        .InformationBox
        {
            background: rgb(165,200,255);
        }
        .Question
        {
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            border-radius: 3px;
            background: rgb(165,204,255);
        }
        .Answer
        {
            padding-left: 20px;
            padding-right: 10px;
            padding-top: 10px;
            padding-bottom: 10px;
            margin-top: 5px;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            border-radius: 3px;
            background: rgb(216,216,216);
        }
    </style>
</head>
<body onload="changeScreenSize()">
    <form id="form1" runat="server">
    <div class="Title">
        <span style="font-weight: 700">Employee Self - Assessment Review</span>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfAssessmentID" runat="server" />
            <asp:HiddenField ID="hfYearOfAssessment" runat="server" />
            <asp:HiddenField ID="hfEmployeeID" runat="server" />
            <asp:HiddenField ID="hfAssessmentToken" runat="server" />
            <asp:HiddenField ID="hfSelfAssessmentProfileID" runat="server" />
            <asp:HiddenField ID="hfStatusCode" runat="server" />
            <br />
            Assessment Instructions :-
            <ul>
                <li>After completing assessment you need to complete and thereafter you are not allowed to add new comment(s) or modification of the provided comment(s).</li>
            </ul>
            <br />
            <br />
            <table style="margin: auto; min-width: 700px;">
                <tr>
                    <td style="height: 10px;">
                    </td>
                    <td colspan="2">
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="Question" style="vertical-align: middle; text-align: center;
                        width: 50%; height: 150px;">
                        <%=DisplayAssessmentName%>
                    </td>
                    <td colspan="2" class="Question" style="vertical-align: middle; text-align: center;
                        width: 50%;">
                        <%=DisplayAssessmentPurposes%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="height: 50px;">
                    </td>
                    <td colspan="2">
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <%=Questions%>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="2">
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="2">
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;" colspan="4">
                        <b>Supervisor Comment(s) :</b>
                        <hr />
                        <asp:TextBox ID="txtSupervisorComments" Height="100px" Width="100%" TextMode="MultiLine"
                            runat="server"></asp:TextBox>
                    </td>
                    <td style="vertical-align: bottom;">
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*"
                            ValidationGroup="Main" ControlToValidate="txtSupervisorComments" ForeColor="Red"
                            ErrorMessage="Supervisor Comment(s) is Required"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Complete" 
                            Width="125px" />
                        <asp:Button ID="btnFinalize" Visible="false" runat="server" ValidationGroup="Main"
                            Width="125px" Text="Finalize" OnClick="btnFinalize_Click" />
                    </td>
                    <td colspan="2">
                    </td>
                    <td style="text-align: right;">
                        <asp:Button ID="btnSelfAssessmentSave" runat="server" Width="125px" ValidationGroup="Main"
                            Text="Save" OnClick="btnSelfAssessmentSave_Click" />
                        <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear" Width="125px" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table style="margin: auto;">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" ForeColor="Red" runat="server"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblFinalizeNotice" Visible="false" ForeColor="Red" runat="server"></asp:Label>
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                        <ProgressTemplate>
                                            <img src="../Images/ProBar/720.GIF" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <asp:ValidationSummary ID="ValidationSummary1" ForeColor="Red" ValidationGroup="Main"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
