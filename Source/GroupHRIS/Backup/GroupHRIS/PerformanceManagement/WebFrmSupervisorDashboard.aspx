<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmSupervisorDashboard.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmSupervisorDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/StyleEmpSearch.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/Charts/Chart.bundle.js" type="text/javascript"></script>
    <script src="../Scripts/Charts/jquery.min.js" type="text/javascript"></script>
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        canvas
        {
            -moz-user-select: none;
            -webkit-user-select: none;
            -ms-user-select: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <span>Supervisor Dashboard</span><span style="font-weight: 700;"> </span>
    <hr />
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <asp:Label ID="lblChart" runat="server"></asp:Label>
    <asp:HiddenField ID="hfSupervisorID" runat="server" />
    <asp:HiddenField ID="hfFinancialYear" runat="server" />
    <asp:HiddenField ID="hfFinancialYearDetails" runat="server" />
    <table style="margin: auto; height: 50px; padding-left: 10px; padding-right: 10px;
        padding-top: 5px;" class="divsearch">
        <tr>
            <td>
                Year of Assessment :
            </td>
            <td>
                <asp:DropDownList ID="ddlYearOfAssessment" Width="175px" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlYearOfAssessment_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td style="width: 30px;">
            </td>
            <td>
                Assessment Status :
            </td>
            <td>
                <asp:DropDownList ID="ddlAssessmentStatus" Width="175px" runat="server" OnSelectedIndexChanged="ddlAssessmentStatus_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <%--<td style="width: 30px;">
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        &nbsp;</td>--%>
            <td style="width: 30px;">
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="text-align: right;">
                Assessment Name :
            </td>
            <td>
                <asp:DropDownList ID="ddlAssessment" Width="175px" runat="server">
                </asp:DropDownList>
            </td>
            <td>
            </td>
            <td style="text-align: right;">
                Evaluation Status :
            </td>
            <td>
                <asp:DropDownList ID="ddlEvaluationStatus" Width="175px" runat="server">
                </asp:DropDownList>
            </td>
            <%--<td></td>
                    <td></td>
                    <td></td>--%>
            <td>
            </td>
            <td>
                <asp:ImageButton ID="imgbtnSearch" runat="server" ToolTip="Search" ImageUrl="~/Images/Common/user-search-icon.png"
                    OnClick="imgbtnSearch_Click" />
            </td>
        </tr>
    </table>
    <table style="margin: auto; width: 700px;">
        <tr>
            <td style="text-align: left;">
                <div id="canvas-holder" style="text-align: left; max-width: 400px; max-height: 400px;">
                    <canvas id="chart-area2" style="text-align: left; margin-left: 0px;" />
                </div>
            </td>
            <td style="text-align: left;">
                <div id="canvas-holder" style="text-align: left; max-width: 400px; max-height: 400px;">
                    <canvas id="chart-area" style="text-align: left; margin-left: 0px;" />
                </div>
            </td>
        </tr>
        <tr>
            <td style="text-align: left;">
                <div id="canvas-holder" style="text-align: left; max-width: 400px; max-height: 400px;">
                    <canvas id="chart-area3" style="text-align: left; margin-left: 0px;" />
                </div>
            </td>
            <td style="text-align: left;">
                <div id="canvas-holder" style="text-align: left; max-width: 400px; max-height: 400px;">
                    <canvas id="chart-area4" style="text-align: left; margin-left: 0px;" />
                </div>
            </td>
        </tr>
    </table>
    <table style="margin: auto;">
        <tr>
            <td>
                <asp:GridView ID="grdvAssessment" AutoGenerateColumns="false" AllowPaging="true"
                    PageSize="10" runat="server" OnPageIndexChanging="grdvAssessment_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="ASSESSMENT_ID" HeaderText=" Assessment ID " HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn">
                            <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                            <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="EMPLOYEE_ID" HeaderText=" Employee ID " HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn">
                            <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                            <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="INITIALS_NAME" HeaderText=" Employee " />
                        <asp:BoundField DataField="YEAR_OF_ASSESSMENT" HeaderText=" Year of Assessment "
                            HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                            <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                            <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="GOAL_ASSESSMENT_STATUS" HtmlEncode="False" HeaderText=" Goal Assessment " />
                        <asp:BoundField DataField="COMPETENCY_ASSESSMENT_STATUS" HtmlEncode="False" HeaderText=" Competency Assessment " />
                        <asp:BoundField DataField="SELF_ASSESSMENT_STATUS" HtmlEncode="False" HeaderText=" Self Assessment " />
                        <asp:BoundField DataField="STATUS_CODE" HtmlEncode="False" HeaderText=" Evaluation " />
                    </Columns>
                </asp:GridView>
            </td>
            <td style="text-align:left;vertical-align:top;">            
                <asp:ImageButton ID="imgRefresh" Width="20px" OnClick="imgbtnSearch_Click" ToolTip="Refresh" ImageUrl="~/Images/Common/refresh.png" runat="server" />            
            </td>
        </tr>
        <tr>
            <td style="text-align: center;">
                <asp:Label ID="lblStatus" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: center;">
                <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>--%>
            </td>
        </tr>
    </table>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
