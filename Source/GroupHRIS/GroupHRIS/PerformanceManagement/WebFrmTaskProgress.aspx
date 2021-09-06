<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmTaskProgress.aspx.cs" Inherits="GroupHRIS.PerformanceManagement.WebFrmTaskProgress" %>

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
            <span>Task Progress Details </span>
            <hr />
            <table>
                <tr>
                    <td class="styleTableCell1">
                        Employee Id :
                    </td>
                    <td style="width: 250px; text-align: left">
                        <asp:TextBox ID="txtEmploeeId" runat="server" ReadOnly="true" ClientIDMode="Static"
                            Width="200px" AutoPostBack="True"></asp:TextBox>
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmploeeId')" />
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvEmployeeId" runat="server" Text=" * " ErrorMessage="Employee Id is Required"
                            ControlToValidate="txtEmploeeId" ForeColor="Red" ValidationGroup="tProgress"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="lbltskName" runat="server"></asp:Label>
                    </td>
                </tr>
                <%--<tr>
            <td align="right">
                Goal Area :
            </td>
            <td>
                <asp:DropDownList ID="ddlgoalArea" runat="server" Width="205px" 
                    OnSelectedIndexChanged="ddlgoalArea_SelectedIndexChanged" AutoPostBack="True">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ErrorMessage="Goal Name is Required" Text = "*" ValidationGroup="tProgress"
                    ControlToValidate = "ddlgoalArea" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>--%>
                <%--  <tr>
            <td class="styleTableCell1">
                Task Name :
            </td>
            <td>
                <asp:DropDownList ID="ddlTaskname" runat="server" Width="205px" OnSelectedIndexChanged="ddlTaskname_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="tProgress"
                    ErrorMessage="Task Name is Required" Text="*" ForeColor="Red" ControlToValidate="ddlTaskname"></asp:RequiredFieldValidator>
            </td>
        </tr>--%>
                <tr>
                    <td class="styleTableCell1">
                        Task Year :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" Width="205px" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Task Year is Required "
                            Text="*" ControlToValidate="ddlYear" ForeColor="Red" ValidationGroup="task"></asp:RequiredFieldValidator>
                    </td>
                </tr>
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
                        Observed Date :
                    </td>
                    <td>
                        <asp:TextBox ID="txtObservedDate" runat="server" Width="200px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="ftobDate" FilterType="Custom, Numbers" ValidChars="/,-"
                            runat="server" TargetControlID="txtObservedDate">
                        </asp:FilteredTextBoxExtender>
                        <asp:CalendarExtender ID="ceobDate" runat="server" TargetControlID="txtObservedDate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Observed Date is Required"
                            Text="*" ControlToValidate="txtObservedDate" ForeColor="Red" ValidationGroup="tProgress"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revextendedDate" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                            Text="*" ErrorMessage="Invalied Date(DD/MM/YYYY)" ValidationGroup="tProgress"
                            ControlToValidate="txtObservedDate" ForeColor="Red"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Progress :
                    </td>
                    <td>
                        <%--<asp:MaskedEditExtender ID="mskSuperPhone" runat="server"
                   TargetControlID="txtProgress"
                   ClearMaskOnLostFocus ="false"
                   MaskType="None"
                   Mask="(999)999-9999" 
                   MessageValidatorTip="true"
                   InputDirection="LeftToRight"
                   ErrorTooltipEnabled="True"></asp:MaskedEditExtender>--%>
                        <asp:TextBox ID="txtProgress" runat="server" Width="200px" MaxLength="5"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom, Numbers"
                            ValidChars="." runat="server" TargetControlID="txtProgress">
                        </asp:FilteredTextBoxExtender>
                        <b>
                            <asp:Label ID="Label1" runat="server" Text=" % "></asp:Label></b>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="tProgress"
                            ErrorMessage="Progress is Required" Text="*" ForeColor="Red" ControlToValidate="txtProgress"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Remarks :
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemarks" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td>
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
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlStatus"
                            ErrorMessage="Status is Required" Text="*" ForeColor="Red" ValidationGroup="tProgress"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" OnClick="btnSave_Click"
                            ValidationGroup="tProgress" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                    </td>
                    <td>
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
                    </td>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="tProgress"
                            ForeColor="Red" />
                    </td>
                </tr>
            </table>
            <%-- <div Style="width: 100px;position: absolute;
        left: 734px; top: 150px;">
    <span>Task Progress</span></div>--%>
            <asp:GridView ID="grdprogress" runat="server" AutoGenerateColumns="false" Style="width: 310px;
                position: absolute; left: 780px; top: 173px;" OnRowDataBound="grdprogress_RowDataBound"
                AllowPaging="true" OnSelectedIndexChanged="grdprogress_SelectedIndexChanged1"
                OnPageIndexChanging="grdprogress_PageIndexChanging" PageSize="5">
                <Columns>
                    <asp:BoundField DataField="OBSERVED_DATE" HeaderText="Observed Date" />
                    <asp:BoundField DataField="PROGRESS" HeaderText="Progress (%)" />
                    <asp:BoundField DataField="LINE_NO" HeaderText="Line Id" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn" />
                    <asp:BoundField DataField="REMARKS" HeaderText="Remark" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn" />
                    <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn" />
                </Columns>
                <EmptyDataTemplate>
                    NO TASK PROGRESS FOUND.
                </EmptyDataTemplate>
            </asp:GridView>
            <br />
            <span>Tasks </span>
            <hr />
            <table style="margin: auto">
                <tr>
                    <td>
                        <asp:GridView ID="grdtskProgress" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                            Style="margin: auto; width: 800px;" PageSize="5" OnPageIndexChanging="grdtskProgress_PageIndexChanging"
                            OnRowDataBound="grdtskProgress_RowDataBound" OnSelectedIndexChanged="grdtskProgress_SelectedIndexChanged1">
                            <Columns>
                                <%--<asp:BoundField DataField="LINE_NO" HeaderText="Line No" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />--%>
                                <asp:BoundField DataField="TASK_ID" HeaderText="Task Id" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="TASK_NAME" HeaderText="Task Name" />
                                <asp:BoundField DataField="PLAN_START_DATE" HeaderText="Planed Start Date" HeaderStyle-Width="70px" />
                                <asp:BoundField DataField="ACTUAL_START_DATE" HeaderText="Actual Start Date" HeaderStyle-Width="70px" />
                                <asp:BoundField DataField="TARGET_DATE" HeaderText="Target Date" />
                                <asp:BoundField DataField="PROGRESS" HeaderText="Progress (%)" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
