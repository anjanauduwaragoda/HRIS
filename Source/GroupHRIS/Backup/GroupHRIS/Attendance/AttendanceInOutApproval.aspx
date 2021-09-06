<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="AttendanceInOutApproval.aspx.cs" Inherits="GroupHRIS.Attendance.AttendanceInOutApproval"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <span style="font-weight: 700">HRIS Employee Attendance Approval</span>
    <br />
    <br />
    <div>
        <table style="width:100%">
            <tr>
                <td align="center" style="padding: 10px;width:100%">
                    <asp:GridView ID="gvLeaves" runat="server" AutoGenerateColumns="False" OnRowCommand="gvAttendance_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Employee Id"
                            ItemStyle-CssClass="hideGridColumn">
                                <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                                <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="EMPNAME" HeaderText="Employee Name" ItemStyle-Width="150px" />
                            <asp:BoundField DataField="ATT_DATE" HeaderText="Date" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="ATT_TIME" HeaderText="Time" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="COMPANY" HeaderText="Company"  ItemStyle-Width="400px" />
                            <asp:BoundField DataField="DIRECTION" HeaderText="IN / OUT " ItemStyle-Width="100px" />
                            <asp:BoundField DataField="COMPANY_ID" HeaderText="Company Id" HeaderStyle-CssClass="hideGridColumn"
                                ItemStyle-CssClass="hideGridColumn">
                                <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                                <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="REASON_CODE" HeaderText="REASON CODE" HeaderStyle-CssClass="hideGridColumn"
                                ItemStyle-CssClass="hideGridColumn">
                                <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                                <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="BRANCH_ID" HeaderText="Branch Id" HeaderStyle-CssClass="hideGridColumn"
                                ItemStyle-CssClass="hideGridColumn">
                                <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                                <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DIRECTIONINOUT" HeaderText="INOUT" HeaderStyle-CssClass="hideGridColumn"
                                ItemStyle-CssClass="hideGridColumn">
                                <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                                <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="REASON" HeaderText="Reason" ItemStyle-Width="300px" />
                            <asp:BoundField DataField="STATUS" HeaderText="Status" ItemStyle-Width="100px" />
                            <asp:ButtonField HeaderText="Approve" Text="Approve" CommandName="Approve" ItemStyle-Width="100px" />
                            <asp:ButtonField HeaderText="Reject" Text="Reject" CommandName="Reject" ItemStyle-Width="100px" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
