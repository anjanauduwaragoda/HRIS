<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="AttendanceReconcile.aspx.cs" Inherits="GroupHRIS.Attendance.AttendanceReconcile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        .style4
        {
            width: 200px;
        }
        .style5
        {
            height: 21px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function openLOVWindow(file, window, ctlName) {
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sEmpId, sName) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById("hfVal").value = sEmpId
            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span style="font-weight: 700">Reconcile Attendance</span>
            <hr />
            <table align="center" width="700px">
                <tr>
                    <td>
                        <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
                        <table style="margin: auto; min-width: 700;">
                            <tr>
                                <td style="text-align: right;">
                                    Employee ID :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmpId" runat="server" Width="200px" AutoPostBack="True" ReadOnly="True"></asp:TextBox>
                                    <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','search','txtemployee')" />
                                    <asp:RequiredFieldValidator ID="rfvempId" runat="server" ControlToValidate="txtEmpId"
                                        ErrorMessage="Employee ID is required" ForeColor="Red" Text="*" ValidationGroup="attendance"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblempName" runat="server" Style="color: #0000FF; font-weight: 700"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    From Date :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFrmDate" runat="server" Width="200px"></asp:TextBox>
                                    <asp:CalendarExtender ID="cefrmDate" runat="server" TargetControlID="txtFrmDate"
                                        Format="yyyy/MM/dd">
                                    </asp:CalendarExtender>
                                    <asp:FilteredTextBoxExtender ID="ftfDate" runat="server" TargetControlID="txtFrmDate"
                                        FilterType="Custom, Numbers" ValidChars="/">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFrmDate"
                                        ErrorMessage="From date is required" ForeColor="Red" Text="*" ValidationGroup="attendance"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    (YYYY/MM/DD)
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    To Date :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" Width="200px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                                        Format="yyyy/MM/dd">
                                    </asp:CalendarExtender>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtToDate"
                                        FilterType="Custom, Numbers" ValidChars="/">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtToDate"
                                        ErrorMessage="To date is required" ForeColor="Red" Text="*" ValidationGroup="attendance"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                   (YYYY/MM/DD)
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="attendance"
                                        Width="100px" OnClick="btnSearch_Click" />
                                    <asp:Button ID="btnCleasr" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                        <ProgressTemplate>
                                            <img src="../Images/ProBar/720.GIF" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblmsg" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="RED" ValidationGroup="attendance" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grdAttendance" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
                            PageSize="10" AllowPaging="True" OnPageIndexChanging="grdAttendance_PageIndexChanging"
                            OnRowDataBound="grdAttendance_RowDataBound" OnSelectedIndexChanged="grdAttendance_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="ATT_DATE" HeaderText="Attendance Date" />
                                <asp:BoundField DataField="ATT_TIME" HeaderText="Attendance Time" />
                                <asp:BoundField DataField="DIRECTION" HeaderText="Direction" />
                                <asp:BoundField DataField="REASON_CODE" HeaderText="Reason" />
                                <%--<asp:BoundField DataField="COMPANY_ID" HeaderText="Company" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="BRANCH_ID" HeaderText="Branch" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />--%>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <br />
                        <table style="margin: auto; min-width: 700;" id="tblAttendance" runat="server">
                            <tr>
                                <td style="text-align: right;">
                                    Attendance Date :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAttDate" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Attendance Time :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAttTime" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Direction :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAttDirection" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trReson">
                                <td style="text-align: right;">
                                    Reason :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReason" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                                    <%--<asp:DropDownList ID="ddlReason" runat="server" Width="200px" Enabled="False">
                <asp:ListItem Value="2">Official Reason</asp:ListItem>
                    <asp:ListItem Value="3">Personal Reason</asp:ListItem>
                </asp:DropDownList>--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Remark :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" runat="server" Width="200px" Height="60px" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvRemarks" runat="server" ControlToValidate="txtRemarks"
                                        ErrorMessage="Remarks is required" ForeColor="Red" Text="*" ValidationGroup="remove"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnRemove" runat="server" Text="Remove" Width="100px" OnClick="btnRemove_Click"
                                        ValidationGroup="remove" />
                                    <asp:Button ID="btnCancelS" runat="server" Text="Cancel" Width="100px" OnClick="btnCancelS_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblMessage" runat="server" Style="display: inline-block; text-align: center;"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ForeColor="RED" ValidationGroup="remove" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
