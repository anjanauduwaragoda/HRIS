<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmEmployeeDashboard.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmEmployeeDashboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <%--<script language="javascript" type="text/javascript">
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=800,height=600,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

            document.getElementById("hfCaller").value = ctlName;
        }
    </script>--%>
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        .style1
        {
            width: 80px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <span>Employee Dashboard </span>
    <hr />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="styleMainTb" style="text-align: center;">
                <tr>
                    <td class="divsearch">
                        <table>
                            <tr>
                                <td>
                                    Year :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlYear" runat="server" Width="180px">
                                    </asp:DropDownList>
                                </td>
                                <td class="style1" style="text-align: right;">
                                    <asp:Label ID="Label1" runat="server" Text="Status :"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="180px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%--<asp:Button ID="btnSearch" runat="server" Text="Search" Width="100px" OnClick="btnSearch_Click" />--%>
                                    <asp:ImageButton ID="imgbtnSearch" runat="server" ImageUrl="~/Images/Common/user-search-icon.png"
                                        OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%">
                <tr>
                    <td style="width: 450px;">
                        <div style="display: block; height: 150px; background-color: #aed6f1;">
                            <table width="450px">
                                <tr style="text-align: center; width: 450px;">
                                    <td colspan="2">
                                        <b>Assessment Details</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleTableCell1" style="vertical-align:top;">
                                        Assessment Name :
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAssessmentName" runat="server" Width="250px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleTableCell1" style="vertical-align:top;">
                                        Assessment Type :
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAssessmentType" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleTableCell1" style="vertical-align:top;">
                                        Purpose :
                                    </td>
                                    <td>
                                        <asp:Literal ID="litPurpose" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td style="width: 450px; vertical-align: text-top; background-color: #aed6f1;">
                        <div style="display: block; height: 150px">
                            <table width="450px">
                                <tr style="text-align: center; width: 450px;">
                                    <td colspan="2">
                                        <b>Availability</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleTableCell1">
                                        Goal Assessment :
                                    </td>
                                    <td>
                                        <%--<asp:LinkButton ID="lbgoalAssessment" runat="server" OnClick="lbgoalAssessment_Click"
                                    ToolTip="View Goal Assessment"></asp:LinkButton>--%>
                                        <asp:Label ID="lblgoalAssessment" runat="server" Width="250px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleTableCell1">
                                        Competency Assessment :
                                    </td>
                                    <td>
                                        <%--<asp:LinkButton ID="lbCompAssessment" runat="server" OnClick="lbCompAssessment_Click"
                                    ToolTip="View Competency Assessment"></asp:LinkButton>--%>
                                        <asp:Label ID="lblCompAssessment" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleTableCell1">
                                        Self Assessment :
                                    </td>
                                    <td>
                                        <%--<asp:LinkButton ID="lbSelfAssessment" runat="server" OnClick="lbSelfAssessment_Click"
                                    ToolTip="View Self Assessment"></asp:LinkButton>--%>
                                        <asp:Label ID="lblSelfAssessment" runat="server" EnableTheming="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleTableCell1">
                                        Performance Evaluation :
                                    </td>
                                    <td>
                                        <asp:Label ID="lbPerformanceEval" runat="server"></asp:Label>
                                        <%--<asp:LinkButton ID="lbPerformanceEval" runat="server" OnClick="lbPerformanceEval_Click"
                                    ToolTip="View Performance Evaluation"></asp:LinkButton>--%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            <span>Assessments</span>
            <hr />
            <table style="margin: auto">
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grdAssessment" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="grdAssessment_PageIndexChanging"
                            OnRowDataBound="grdAssessment_RowDataBound" OnSelectedIndexChanged="grdAssessment_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="ASSESSMENT_ID" HeaderText="Assessment Id" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="ASSESSMENT_NAME" HeaderText="Assessment" />
                                <asp:BoundField DataField="ASSESSMENT_TYPE_NAME" HeaderText="Assessment Type" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                                <asp:BoundField DataField="INCLUDE_SELF_ASSESSMENT" HeaderText="Self Assessment"
                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="INCLUDE_COMPITANCY_ASSESSMENT" HeaderText="Competency assessment"
                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="INCLUDE_GOAL_ASSESSMENT" HeaderText="Goal Assessment"
                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                            </Columns>
                            <EmptyDataTemplate>
                                NO ASSESSMENT FOUND.
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hfgoalAssessment" runat="server" />
                        <asp:HiddenField ID="hfCompetencyAss" runat="server" />
                        <asp:HiddenField ID="hfSelfAssessment" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
