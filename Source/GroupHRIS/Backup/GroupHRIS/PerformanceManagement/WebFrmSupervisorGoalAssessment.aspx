<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmSupervisorGoalAssessment.aspx.cs"
    Inherits="GroupHRIS.PerformanceManagement.WebFrmSupervisorGoalAssessment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script language="javascript" type="text/javascript">

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
    <title>Supervisor Goal Assessment</title>
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/Charts/Chart.bundle.js" type="text/javascript"></script>
    <script src="../Scripts/Charts/jquery.min.js" type="text/javascript"></script>
    <style>
    canvas{
        -moz-user-select: none;
        -webkit-user-select: none;
        -ms-user-select: none;
    }
    </style>
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
        .hideGridColumn
        {
            display: none;
        }
        .style1
        {
            color: #FF0000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Title">
        <span style="font-weight: 700">Supervisor Goal Assessment Review</span>
    </div>
    <asp:HiddenField ID="hfAssesmentID" runat="server" />
    <asp:HiddenField ID="hfEmployeeID" runat="server" />
    <asp:HiddenField ID="hfYearofAssessment" runat="server" />
    <asp:Label ID="lblGoalChart" runat="server" ></asp:Label>
    <%--<br />
    <br />
    Assessment Instructions :-
    <ul>
        <li>You can partially evaluate gaols and save/update given scores without evaluate all the goals at one step.</li>
        <li>After evaluating all annual goals. You need to click complete button to complete the goal evaluation process.</li>
    </ul>--%>
    <table style="margin: auto; width: 100%;">
        <tr>
            <td style="width: 33.3%; text-align: center;">
                Subordinate&#39;s Score :
                <asp:Label ID="lblSubScore" ClientIDMode="Static" runat="server"></asp:Label>
            </td>
            <td colspan="2" style="width: 33.3%; text-align: center;">
                Supervisor&#39;s Score :
                <asp:Label ID="lblSupScore" ClientIDMode="Static" runat="server"></asp:Label>
            </td>
            <td style="width: 33.3%; text-align: center;">
                Total Planned :
                <asp:Label ID="lblTotalPlanned" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="Question" style="vertical-align: middle; text-align: center; width: 50%;
                height: 150px;" colspan="2">
                <%=DisplayAssessmentName%>
            </td>
            <td colspan="2" class="Question" style="vertical-align: middle; text-align: center;
                width: 50%;">
                <%=DisplayAssessmentPurposes%>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:GridView ID="grdvReportGrid" ClientIDMode="Static" Style="width: 100%;" AllowPaging="false"
                    AutoGenerateColumns="false" runat="server" OnRowDataBound="grdvReportGrid_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="ASSESSMENT_TOKEN" HeaderText="Assessment Token" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="GOAL_ID" HeaderText="Goal ID" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="GOAL_AREA" ItemStyle-VerticalAlign="Top" ItemStyle-Width="25%"
                            HtmlEncode="false" HeaderText="Goal Area" />
                        <asp:BoundField DataField="MEASUREMENTS" ItemStyle-VerticalAlign="Top" ItemStyle-Width="30%"
                            HtmlEncode="false" HeaderText="Measurements" />
                        <asp:BoundField DataField="EXPECTED_WEIGHT" ItemStyle-VerticalAlign="Top" ItemStyle-Width="10%"
                            ItemStyle-HorizontalAlign="Right" HeaderText="Planned %" />
                        <asp:BoundField DataField="EMPLOYEE_SELF_SCORE" ItemStyle-VerticalAlign="Top" ItemStyle-Width="15%"
                            ItemStyle-HorizontalAlign="Right" HeaderText="Subordinate's Score %" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top"
                            ItemStyle-Width="10%" HeaderText="Supervisor's Score%">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAchievedWeight" MaxLength="6" Style='text-align: right;' Width="95%"
                                    Height="100%" runat="server"> </asp:TextBox>
                                <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Numbers" ValidChars="." TargetControlID="txtAchievedWeight" runat="server">
                                </asp:FilteredTextBoxExtender>--%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="SUPERVISOR_SCORE" HeaderText="Supervisor Score" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <strong>Supervisor Comment(s) :</strong>
                <hr />
                <asp:TextBox ID="txtSupervisorComment" Width="100%" Height="100px" TextMode="MultiLine"
                    runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left;" colspan="2">
                <asp:Button ID="btnComplete" runat="server" Width="125px" Text="Complete" OnClick="btnComplete_Click" />
            </td>
            <td style="text-align: right;" colspan="2">
                <asp:Button ID="btnSave" runat="server" Width="125px" Text="Save" OnClick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" Width="125px" Text="Clear" OnClick="btnClear_Click" />
            </td>
        </tr>
    </table>
    <br />
    <table style="margin: auto;">
        <tr>
            <td>
                <asp:Label ID="Label1" ForeColor="Red" runat="server"></asp:Label>
                <asp:Label ID="lblFinalizeNotice" Visible="false" ForeColor="Red" runat="server"></asp:Label>
                <br />
                <br />
                <asp:Label ID="lblStatus" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
