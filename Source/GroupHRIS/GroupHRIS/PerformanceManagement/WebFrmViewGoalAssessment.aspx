<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmViewGoalAssessment.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmViewGoalAssessment" %>

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
    <title>Employee Goal Self Assessment</title>
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
        .hideGridColumn
        {
            display: none;
        }
    </style>
</head>
<body class="popupsearch">
    <form id="form1" runat="server">    
    <span style="font-weight: 700">Employee Goal Self Assessment</span>
    <hr />
    <asp:HiddenField ID="hfAssesmentID" runat="server" />
    <asp:HiddenField ID="hfEmployeeID" runat="server" />
    <asp:HiddenField ID="hfYearofAssessment" runat="server" />
    <table>
        <tr>
            <td>
                Total Achievements
            </td>
            <td>
                :
            </td>
            <td>
                <asp:Label ID="lblTotalAcievement" ClientIDMode="Static" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Total Planned
            </td>
            <td>
                :
            </td>
            <td>
                <asp:Label ID="lblTotalPlanned" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblGoalChart" runat="server" ></asp:Label>
    <%--<br />
    Assessment Instructions :- 
    <ol>
        <li>
            You can partially evaluate goals and save/update given scores (marks) without evaluating all the goals at one step.
        </li>
        <li>
            After evaluating all the goals you need to click finalize button to finish evaluation process. After finalize, you are not allowed to modify given scores (marks).
        </li>
    </ol>
    <br />
    <br />--%>
    <table style="margin: auto; width: 100%;">
        <tr>
            <td colspan="2">
                <asp:GridView ID="grdvReportGrid" ClientIDMode="Static" Style="width: 100%;" AllowPaging="false"
                    AutoGenerateColumns="false" runat="server">
                    <Columns>
                        <asp:BoundField DataField="ASSESSMENT_ID" HeaderText="Assessment ID" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="ASSESSMENT_TOKEN" HeaderText="Assessment Token" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="YEAR_OF_ASSESSMENT" HeaderText="Year of Assessment" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Employee ID" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="GOAL_ID" HeaderText="Goal ID" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="GOAL_AREA" HeaderText="Goal Area" />
                        <asp:BoundField DataField="MEASUREMENTS" HtmlEncode="false" HeaderText="Measurements" />
                        <asp:BoundField DataField="WEIGHT" ItemStyle-HorizontalAlign="Right" HeaderText="Planned %" />
                        <asp:TemplateField  ItemStyle-HorizontalAlign="Right" HeaderText="Achievement %">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAchievedWeight" style='text-align:right;' width="95%" Height="100%" runat="server"> </asp:TextBox>
                                <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Numbers" ValidChars="." TargetControlID="txtAchievedWeight" runat="server">
                                </asp:FilteredTextBoxExtender>--%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="SUPERVISOR_SCORE" ItemStyle-HorizontalAlign="Right" HtmlEncode="false" HeaderText="Supervisor's Score %" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td style="text-align: left;">
                <asp:Button ID="btnFinalize" runat="server" Width="125px" Text="Finalize" OnClick="btnFinalize_Click" />
            </td>
            <td style="text-align: right;">
                <asp:Button ID="btnSave" runat="server" Width="125px" Text="Save" OnClick="btnSave_Click" />
                </td>
        </tr>
    </table>
    <br />
    <table style="margin: auto;">
        <tr>
            <td>
                <asp:Label ID="lblStatus" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>