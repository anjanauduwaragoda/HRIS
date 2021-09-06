<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFrmCEOCompetencyAssessment.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmCEOCompetencyAssessment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Supervisor Competency Assessment</title>
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
        .hideGridColumn
        {
            display: none;
        }
        .style1
        {
            height: 18px;
        }
        .style2
        {
            height: 22px;
        }
    </style>
</head>
<body onload="changeScreenSize()">
    <form id="form1" runat="server">
    <div class="Title">
        <span style="font-weight: 700">CEO/COO Competency Assessment Review</span>
    </div>
    <asp:HiddenField ID="hfAssessmentID" runat="server" />
    <asp:HiddenField ID="hfYearOfAssessment" runat="server" />
    <asp:HiddenField ID="hfEmployeeID" runat="server" />
    <table style="margin: auto; min-width: 700px;">
    <tr>
        <td>
            <asp:Label ID="lblGoalChart" runat="server" ></asp:Label>
        </td>
    </tr>
    </table>    
    <table style="margin: auto; min-width: 700px;">
        <tr>
            <td class="Question" style="vertical-align: middle; text-align: center; width: 50%;
                height: 150px;">
                <%=DisplayAssessmentName%>
            </td>
            <td colspan="2" class="Question" style="vertical-align: middle; text-align: center;
                width: 50%;">
                <%=DisplayAssessmentPurposes%>
            </td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td colspan="2" class="style2" style="text-align:center;">
                <asp:Label ID="lblAllocatedWeights" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="grdvCompetencies" ClientIDMode="Static" Style="width: 100%;" AllowPaging="false"
                    AutoGenerateColumns="false" runat="server" OnRowDataBound="grdvCompetencies_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="ASSESSMENT_TOKEN" HeaderText="Assessment Token" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="COMPETENCY_ID" HeaderText="Competency ID" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="COMPETENCY_NAME" HeaderText="Competency" />
                        <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                        <asp:BoundField DataField="EXPECTED_PROFICIENCY_RATING" ItemStyle-HorizontalAlign="Center" HeaderText="Expected Rating" />
                        <asp:BoundField DataField="EMPLOYEE_RATING" ItemStyle-HorizontalAlign="Center" HeaderText="Employee Rating" />
                        
                        <asp:BoundField DataField="SUPERVISOR_RATING" ItemStyle-HorizontalAlign="Center" HeaderText="Supervisor Rating" />
                        <%--<asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Supervisor Rating">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlSupervisorRating" Enabled="false" Width="100%" runat="server">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>--%>
                        <asp:BoundField DataField="SUPERVISOR_WEIGHT" HeaderText="Supervisor Weight" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="COMPETENCY_PROFILE_ID" HeaderText="Competency Profile ID"
                            HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="COMMENTS" HeaderText="Supervisor Comments" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="style2">
            Supervisor Name : 
                        <asp:Label ID="lblSupervisorName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                
                <strong>Supervisor Comment(s):</strong>
                <hr />
                <asp:TextBox ID="txtSupervisorComments" style="text-align:left;vertical-align:top;" Width="100%" Height="75px" Enabled="false" runat="server"></asp:TextBox>
                
                
            </td>
        </tr>
        <tr>
            <td colspan="2">
            
            </td>
            <%--<td style="vertical-align: bottom;">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtSupervisorComment"
                    runat="server" ForeColor="Red" Text="*" ValidationGroup="Main" ErrorMessage="Supervisor Comment(s) is Required"></asp:RequiredFieldValidator>
            </td>--%>
        </tr>
        <%--<tr>
            <td style="text-align: left;">
            </td>
            <td style="text-align: right;">
                <asp:Button ID="btnFinalize" runat="server" ValidationGroup="Main" Text="Finalize" Width="125px" OnClick="btnFinalize_Click" />
            </td>
        </tr>--%>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Label ID="lblStatus" runat="server"></asp:Label>
                <%--<table style="margin: auto;">
                    <tr>
                        <td style="text-align: left;">
                            <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Main" ForeColor="Red"
                                runat="server" />
                        </td>
                    </tr>
                </table>--%>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>