<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="AttendanceApproveReject.aspx.cs" Inherits="GroupHRIS.Attendance.AttendanceApproveReject" %>

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
        <table>
            <tr>
                <td>
                    <asp:GridView ID="gvLeaves" runat="server" AutoGenerateColumns="False" 
                        AllowPaging="true" PageSize = "10" 
                        onpageindexchanging="gvLeaves_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Employee Id" ItemStyle-CssClass="hideGridColumn">
                                <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                                <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="EMPNAME" HeaderText="Employee Name" ItemStyle-Width="150px" />
                            <asp:BoundField DataField="ATT_DATE" HeaderText="Date" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="ATT_TIME" HeaderText="Time" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="COMPANY" HeaderText="Company" ItemStyle-Width="400px" />
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
                            <asp:BoundField DataField="REASON" HeaderText="Reason" ItemStyle-Width="600px" />
                            <asp:BoundField DataField="STATUS" HeaderText="Status" ItemStyle-Width="100px" />
                           
                            <asp:TemplateField HeaderText="App/Remove">
                                    <HeaderTemplate><asp:Label ID="lblExclude" runat="server"  Width="150px"></asp:Label><br />
                                        <asp:RadioButton ID="chkBxHeaderApprove" GroupName="hdr" AutoPostBack="true" Text="Approve"
                                            runat="server" OnCheckedChanged="chkBxHeaderApprove_CheckedChanged" />
                                            &nbsp;&nbsp;
                                        <asp:RadioButton ID="chkBxHeaderReject" GroupName="hdr" AutoPostBack="true" Text="Reject"
                                            runat="server" OnCheckedChanged="chkBxHeaderReject_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:RadioButton ID="chkBxSelectApprove" Text="Approve" GroupName="rdbtn" runat="server" />
                                        &nbsp;&nbsp;
                                        <asp:RadioButton ID="chkBxSelectReject" Text="Reject" GroupName="rdbtn" runat="server"/>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">

                    <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click" />
                </td>
            </tr>
        </table>
</asp:Content>
