<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmCloseAssessment.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmCloseAssessment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/Charts/Chart.bundle.js" type="text/javascript"></script>
    <script src="../Scripts/Charts/jquery.min.js" type="text/javascript"></script>
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        .leftCol
        {
            max-width: 230px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <%--<%=js %>--%>
    <br />
    <span>Close Assessment </span>
    <hr />
    <asp:Label ID="jsLable" runat="server" Text="" ClientIDMode="Static"></asp:Label>
    <asp:Label ID="lblBarChartScript" runat="server" Text="" ClientIDMode="Static"></asp:Label>

            <table class="styleMainTb" style="text-align: center;">
                <tr>
                    <td class="divsearch">
                        <table>
                            <tr>
                                <td>
                                    Company :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCompany" runat="server" Width="252px" AutoPostBack="True"
                                        ClientIDMode="Static" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Year :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlYear" runat="server" Width="152px" AutoPostBack="True">
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
    <div style="">
        <table width="100%" runat="server" id="tblGraphs" clientidmode="Static">
            <tr>
                <td style="min-width: 49%; background-color: #f2f4f4;" align="center">
                    <br />
                    <canvas id="chart-area" style="max-width: 300px; max-height: 400px;"></canvas>
                    <br />
                    <b>
                        <asp:Label ID="lblTotal1" runat="server" Text=""></asp:Label>
                    </b>
                    <br />
                    <br />
                </td>
                <td style="background-color: white;">
                </td>
                <td style="min-width: 49%; background-color: #f2f4f4; vertical-align: middle;" align="center">
                    <br />
                    <canvas id="bar-chart-area" style="max-width: 300px; max-height: 400px; min-width: 300px;
                        vertical-align: middle;"></canvas>
                    <br />
                    <b>
                        <asp:Label ID="lblTotal2" runat="server" Text=""></asp:Label>
                    </b>
                    <br />
                    <br />
                </td>
            </tr>
        </table>
        <br />
    </div>
    <br />
    <table width="100%">
        <tr>
            <td align="center">
                <asp:Label ID="lblErrorMsg2" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                                        <ProgressTemplate>
                                            <img src="../Images/ProBar/720.GIF" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
            </td>
        </tr>
    </table>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%" runat="server" clientidmode="Static" id="assessmentsTbl">
                <tr>
                    <td class="leftCol">
                        Status :
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="125px" ClientIDMode="Static"
                            OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <b><asp:Label ID="lblAssessmentName" runat="server" Text=""></asp:Label></b>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="min-width: 430px;" valign="top">
                        <asp:GridView ID="gvAssessment" runat="server" AutoGenerateColumns="false" Style="width: 425px;"
                            OnRowDataBound="gvAssessment_RowDataBound" OnSelectedIndexChanged="gvAssessment_SelectedIndexChanged"
                            AllowPaging="true" PageSize="8" OnPageIndexChanging="gvAssessment_PageIndexChanging"
                            EmptyDataText="No Assessments found">
                            <Columns>
                                <asp:BoundField DataField="ASSESSMENT_ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="ASSESSMENT_NAME" HeaderText="Assessment" />
                                <asp:BoundField DataField="ASSESSMENT_TYPE_NAME" HeaderText="Type" />
                                <%--<asp:BoundField DataField="STATUS_CODE" HeaderText="Status" ItemStyle-Width="100px"/>--%>
                                <asp:BoundField DataField="COUNT" HeaderText="Emp.Count" />
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td>
                    </td>
                    <td valign="top" colspan="2" align="left">
                        <table id="detailTbl" runat="server" clientidmode="Static">
                            
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label1" runat="server" Text="Total no. of Employees :"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalEmp" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label2" runat="server" Text="CEO Finalized :"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCeoFinalized" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label4" runat="server" Text="Supervisor Finalized :"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSupervisorFinalized" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label5" runat="server" Text="Supervisor Completed :"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSupervisorCompleted" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label3" runat="server" Text="Employee Finalized :"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmployeeFinalized" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="right">
                                    Close by force &nbsp;<asp:CheckBox ID="chkForceClose" runat="server" AutoPostBack="true"
                                        ClientIDMode="Static" OnCheckedChanged="chkForceClose_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtRemarks" placeholder=" Add reason(s) for closing" runat="server"
                                        TextMode="MultiLine" Width="296px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                        <ProgressTemplate>
                                            
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:Button ID="btnClose" runat="server" Text="Close Assessment" OnClick="btnClose_Click" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="right" style="padding-right: 58px;">
                        <asp:Label ID="lblErrorMsg" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfAssessmentId" ClientIDMode="Static" runat="server" />
            </ContentTemplate>
    </asp:UpdatePanel>
        

</asp:Content>
