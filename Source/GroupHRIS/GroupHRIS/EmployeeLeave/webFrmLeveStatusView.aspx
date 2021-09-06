<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="webFrmLeveStatusView.aspx.cs" Inherits="GroupHRIS.EmployeeLeave.webFrmLeveStatusView"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

//        function openLOVWindow(file, window, ctlName) {
//            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
//            //if (childWindow.opener == null) childWindow.opener = self;

//            document.getElementById("hfCaller").value = ctlName;
//        }

//        function getValueFromChild(sRetVal) {

//            var ctl = document.getElementById("hfCaller").value;
//            document.getElementById(ctl).value = sRetVal;


//        }

//        function getValueFromChild(sEmpId, sName, sCompanyId, sDepartmentId, sDivisionId) {
//            var ctl = document.getElementById("hfCaller").value;
//            document.getElementById(ctl).value = sEmpId;

//            DoPostBack();
//        }

//        function DoPostBack() {
//            __doPostBack();
        //        }

        var txb;
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
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br>
            <span>Employee Leaves View(Pending/Covered/Recommended/Rejected/Discarded)</span>
            <hr />
            <table>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label4" runat="server" Style="text-align: right" Text="Employee"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:TextBox ID="txtEmploeeId" runat="server" ClientIDMode="Static" ReadOnly="true" CssClass="styleTableCell2TextBox"
                            MaxLength="8"></asp:TextBox>
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfEmpId" runat="server" />
                        <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                    </td>
                    <td class="styleTableCell3">
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmploeeId')" />
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                    <td style="width: 300px">
                        <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label5" runat="server" Text="Company"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:DropDownList ID="ddlCompany" runat="server" Width="256px" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label6" runat="server" Text="Department"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:DropDownList ID="ddlDepartment" runat="server" Width="256px" TabIndex="1" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label7" runat="server" Text="Division"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:DropDownList ID="ddlDivision" runat="server" Width="256px" TabIndex="2" 
                            onselectedindexchanged="ddlDivision_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label1" runat="server" Style="text-align: right" Text="From Date"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="styleTableCell2TextBox"></asp:TextBox>
                        <asp:CalendarExtender ID="ceFDate" runat="server" Format="yyyy/MM/dd" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                    <td class="styleTableCell4" style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate"
                            ErrorMessage="From Date is required" ForeColor="Red" ValidationGroup="show">*</asp:RequiredFieldValidator>
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        <asp:Label ID="Label2" runat="server" Text="To Date"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="styleTableCell2TextBox"></asp:TextBox>
                        <asp:CalendarExtender ID="ceToDate" runat="server" Format="yyyy/MM/dd" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                    <td class="styleTableCell4" style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                            ErrorMessage="To Date is required" ForeColor="Red" ValidationGroup="show">*</asp:RequiredFieldValidator>
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        <asp:Label ID="Label3" runat="server" Text="Status"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="250px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td style="text-align: right">
                        <asp:Button ID="btnShow" runat="server" Text="Show" Width="125px" ValidationGroup="show"
                            OnClick="btnShow_Click" Style="height: 26px" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="125px" 
                            Style="height: 26px" onclick="btnClear_Click" />
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblMessage" runat="server" Text="" style="margin-left:200px"></asp:Label>
            <br />
            <asp:ValidationSummary ID="vsShow" runat="server" ValidationGroup="show" 
                ForeColor="Red" />
            <br />
            <asp:GridView ID="gvLeaves" runat="server" AutoGenerateColumns="False" OnRowCommand="gvLeaves_RowCommand"
                OnRowDataBound="gvLeaves_RowDataBound" AllowPaging="True" 
                onpageindexchanging="gvLeaves_PageIndexChanging" PageSize="20">
                <Columns>
                    <asp:BoundField DataField="LEAVE_SHEET_ID" HeaderText="LEAVE_SHEET_ID" />
                    <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="EMPLOYEE_ID" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn">
                        <HeaderStyle CssClass="hideGridColumn" />
                        <ItemStyle CssClass="hideGridColumn" />
                    </asp:BoundField>
                    <asp:BoundField DataField="EPF_NO" HeaderText="EPF_NO" />
                    <asp:BoundField DataField="APPLICANT_NAME" HeaderText="APPLICANT_NAME" />
                    <asp:BoundField DataField="FROM_DATE" HeaderText="FROM_DATE" />
                    <asp:BoundField DataField="TO_DATE" HeaderText="TO_DATE" />
                    <asp:BoundField DataField="NO_OF_DAYS" HeaderText="NO_OF_DAYS" />
                    <asp:BoundField DataField="COVERED_BY" HeaderText="COVERED_BY" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn">
                        <HeaderStyle CssClass="hideGridColumn" />
                        <ItemStyle CssClass="hideGridColumn" />
                    </asp:BoundField>
                    <asp:BoundField DataField="COVERED_BY_NAME" HeaderText="COVERED_BY_NAME" />
                    <asp:BoundField DataField="RECOMMEND_BY" HeaderText="RECOMMEND_BY" HeaderStyle-CssClass="hideGridColumn"
                        ItemStyle-CssClass="hideGridColumn">
                        <HeaderStyle CssClass="hideGridColumn" />
                        <ItemStyle CssClass="hideGridColumn" />
                    </asp:BoundField>
                    <asp:BoundField DataField="RECOMMEND_BY_NAME" HeaderText="RECOMMEND_BY_NAME" />
                    <asp:BoundField DataField="LEAVE_STATUS" HeaderText="LEAVE_STATUS" />
                    <asp:ButtonField CommandName="View" HeaderText="VIEW_DETAILS" Text="View" />
                </Columns>
            </asp:GridView>
            <br />
            <asp:Label ID="lblLSDetail" runat="server" Text="Leave Sheet Details"></asp:Label>
            <hr />
            <asp:GridView ID="gvLSDetails" runat="server">
    </asp:GridView>
    <br />
            <asp:LinkButton ID="lbtnClear" runat="server" onclick="lbtnClear_Click">Clear Leave Sheet Details</asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
