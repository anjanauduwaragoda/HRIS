<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmUploadEmployeeGoals.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmUploadEmployeeGoals" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <span>Upload Employee Goals</span><span style="font-weight: 700;"> </span>
    <hr />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <table style="margin: auto; width: 600px;">
        <tr>
            <td>
                <asp:HyperLink ID="hypExcelTemplate" NavigateUrl="~/DocumentDownload/FileDownloader.ashx" runat="server">Download Excel Template</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td style="text-align: right;">
                Employee Goals Excel File [.xls/.xlsx]
            </td>
            <td>
                <asp:FileUpload ID="fuUploadExcel" runat="server" />
            </td>
            <td style="text-align: right;">
                <asp:Button ID="btnUpload" Width="125px" runat="server" Text="Upload" OnClick="btnUpload_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfPath" runat="server" />
            <table style="margin: auto; width: 600px;">
                <tr>
                    <td style="width: 78%;">
                        <asp:Label ID="lblFileName" runat="server" Style="color: #3333CC; font-weight: 700"></asp:Label>
                    </td>
                    <td style="text-align: right;">
                        <asp:Button ID="btnProcessData" Width="125px" runat="server" Text="Process Excel"
                            OnClick="btnProcessData_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: right;">
                        <asp:Button ID="btnSave" Width="125px" runat="server" Text="Save" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" Width="125px" runat="server" Text="Clear" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <%--<br />--%>
            <%--Invalid Cumulative Weights
            <hr />
            <table style="margin: auto;">
                <tr>
                    <td>
                        <asp:GridView ID="GrdvInvalidCumulativeWeights" Style="width: 850px;" AutoGenerateColumns="false"
                            runat="server">
                            <Columns>
                                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText=" Employee ID " />
                                <asp:BoundField DataField="YEAR_OF_GOAL" HeaderText=" Year " />
                                <asp:BoundField DataField="EMP_NAME" HeaderText=" Employee " />
                                <asp:BoundField DataField="EPF_NO" HeaderText=" EPF " />
                                <asp:BoundField DataField="COMP_NAME" HeaderText=" Company " />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>--%>
            <div id="dvMain" runat="server" visible="false">
            <br />
            Employee Goals
            <hr />
            </div>
            <table style="margin: auto;">
                <tr>
                    <td>
                        <asp:GridView ID="grdvEmployeeGoals" Style="width: 850px;" AutoGenerateColumns="false"
                            runat="server" OnRowDataBound="grdvEmployeeGoals_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText=" Employee ID " />
                                <asp:BoundField DataField="YEAR_OF_GOAL" HeaderText=" Year " />
                                <asp:BoundField DataField="COMP_NAME" HeaderText=" Company " />
                                <asp:BoundField DataField="EPF_NO" HeaderText=" EPF " />
                                <asp:BoundField DataField="EMP_NAME" HeaderText=" Employee " />
                                <asp:BoundField DataField="GROUP_NAME" HeaderText=" Goal Group " />
                                <asp:BoundField DataField="GOAL_AREA" HeaderText=" Goal " />
                                <asp:BoundField DataField="DESCRIPTION" HeaderText=" Description " />
                                <asp:BoundField DataField="MEASUREMENTS" HeaderText=" Measurements " />
                                <asp:BoundField DataField="WEIGHT" HeaderText=" Weight " />
                                <asp:BoundField DataField="INVALID_YEAR" HeaderText=" Invalid Year " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="INVALID_WEIGHT" HeaderText=" Invalid Weight " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="CUM_WEIGHT" HeaderText=" Cumulative Weight " />
                                <asp:BoundField DataField="INVALID_CUM_WEIGHT" HeaderText=" Invalid Cum Weight "
                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="GOAL_GROUP_ID" HeaderText=" Goal Group ID " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status " HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>

            <div id="dvSuccess" visible="false" runat="server">
                <br />
                Success Records
                <hr />
                <table style="margin: auto;">
                    <tr>
                        <td>
                            <asp:GridView ID="grdvSuccess" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="YEAR_OF_GOAL" HeaderText="Year" />
                                    <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Employee ID" />
                                    <asp:BoundField DataField="EPF_NO" HeaderText="EPF" />
                                    <asp:BoundField DataField="EMP_NAME" HeaderText="Employee" />
                                    <asp:BoundField DataField="COMP_NAME" HeaderText="Company" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvError" visible="false" runat="server">
                <br />
                Faild Records
                <hr />
                <table style="margin: auto;">
                    <tr>
                        <td>
                            <asp:GridView ID="GrdvErros" AutoGenerateColumns="false" runat="server">
                                <Columns>
                                    <asp:BoundField DataField="YEAR_OF_GOAL" HeaderText="Year" />
                                    <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Employee ID" />
                                    <asp:BoundField DataField="EPF_NO" HeaderText="EPF" />
                                    <asp:BoundField DataField="EMP_NAME" HeaderText="Employee" />
                                    <asp:BoundField DataField="COMP_NAME" HeaderText="Company" />
                                    <asp:BoundField DataField="FAILED_REASON" HeaderText="Reason" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
