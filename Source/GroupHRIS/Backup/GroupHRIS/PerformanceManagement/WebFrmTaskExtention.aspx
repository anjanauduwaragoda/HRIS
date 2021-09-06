<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmTaskExtention.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmTaskExtention"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <script>
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=800,height=600,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;

            var id = document.getElementById(txb).value;
            document.getElementById("hfVal").value = id;
            //document.getElementById(ctl).value = sRetVal;

            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }
    </script>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span>Task Extension Details </span>
            <hr />
            <table>
                <tr>
                    <td class="styleTableCell1">
                        Employee Id :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmploeeId" runat="server" ReadOnly="true" ClientIDMode="Static"
                            Width="200px" AutoPostBack="True"></asp:TextBox>
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmploeeId')" />
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvEmployeeId" runat="server" Text=" * " ErrorMessage="Employee Id is required"
                            ControlToValidate="txtEmploeeId" ForeColor="Red" ValidationGroup="tExtention"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="lbltskName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Task Year :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" Width="205px" 
                            OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Task Year is Required "
                            Text="*" ControlToValidate="ddlYear" ForeColor="Red" ValidationGroup="task"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <%--  <tr><td align="right">Goal Area : </td><td>
        <asp:DropDownList ID="ddlgoalName" runat="server" Width="205px" 
            AutoPostBack="True" onselectedindexchanged="ddlgoalName_SelectedIndexChanged">
        </asp:DropDownList>
        
        </td><td><asp:RequiredFieldValidator ID="rfvgoalName" runat="server" 
            ErrorMessage="Goal Area is Required"  Text = "*" ControlToValidate = "ddlgoalName" ForeColor="Red" ValidationGroup = "tExtention"></asp:RequiredFieldValidator>
        </td></tr>--%>
                <tr>
                    <td class="styleTableCell1">
                        Target Date :
                    </td>
                    <td>
                        <asp:TextBox ID="txttargetDate" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Total Completion :
                    </td>
                    <td>
                        <asp:TextBox ID="txtTotalcompletion" runat="server" Width="200px" Enabled="false"> </asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Extension Date :
                    </td>
                    <td>
                        <asp:TextBox ID="txtextendedDate" runat="server" Width="200px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="ftetendedDate" FilterType="Custom, Numbers" ValidChars="/,-"
                            runat="server" TargetControlID="txtextendedDate">
                        </asp:FilteredTextBoxExtender>
                        <asp:CalendarExtender ID="ceextendedDate" runat="server" TargetControlID="txtextendedDate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Extended Date is Required"
                            Text="*" ValidationGroup="tExtention" ControlToValidate="txtextendedDate" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revextendedDate" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                            Text="*" ErrorMessage="Invalied Date(DD/MM/YYYY)" ValidationGroup="tExtention"
                            ControlToValidate="txtextendedDate" ForeColor="Red"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Reason :
                    </td>
                    <td>
                        <asp:TextBox ID="txtReason" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Reason is Required"
                            ValidationGroup="tExtention" Text="*" ControlToValidate="txtReason" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Status :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="205px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Status is Required"
                            ControlToValidate="ddlStatus" Text="*" ForeColor="Red" ValidationGroup="tExtention"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" ValidationGroup="tExtention"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
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
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
                        <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                        <asp:HiddenField ID="hfexDate" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                    </td>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="tExtention"
                            ForeColor="Red" />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="grdtsk" runat="server" AutoGenerateColumns="false" AllowPaging="false"
                Style="width: 315px; position: absolute; left: 780px; top: 170px;" OnRowDataBound="grdtsk_RowDataBound"
                OnSelectedIndexChanged="grdtsk_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="EXTENDED_DATE" HeaderText="Extended Date" />
                    <asp:BoundField DataField="REASON" HeaderText="Reason" />
                    <asp:BoundField DataField="TASK_EXTENTION_ID" HeaderText="Extention Id" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn" />
                    <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn" />
                </Columns>
                <EmptyDataTemplate>
                    NO TASK EXTENTION FOUND.
                </EmptyDataTemplate>
            </asp:GridView>
            <br />
            <span>Tasks </span>
            <hr />
            <table style="margin: auto">
                <tr>
                    <td>
                        <asp:GridView ID="grdtskExtention" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                            Style="margin: auto; width: 800px;" OnPageIndexChanging="grdtskExtention_PageIndexChanging"
                            OnRowDataBound="grdtskExtention_RowDataBound" OnSelectedIndexChanged="grdtskExtention_SelectedIndexChanged"
                            PageSize="5">
                            <Columns>
                                <%--<asp:BoundField DataField="TASK_EXTENTION_ID" HeaderText="Extention Id" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />--%>
                                <asp:BoundField DataField="TASK_NAME" HeaderText="Task Name" />
                                <asp:BoundField DataField="PLAN_START_DATE" HeaderText="Planed Start Date" HeaderStyle-Width="70px" />
                                <asp:BoundField DataField="ACTUAL_START_DATE" HeaderText="Actual Start Date" HeaderStyle-Width="70px" />
                                <asp:BoundField DataField="TARGET_DATE" HeaderText="Target Date" />
                                <asp:BoundField DataField="EXTENDED_DATE" HeaderText="Last Extended Date" />
                                <asp:BoundField DataField="TOTAL_COMPLETION" HeaderText="Total Completion(%)" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                                <asp:BoundField DataField="TASK_ID" HeaderText="Task Id" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
