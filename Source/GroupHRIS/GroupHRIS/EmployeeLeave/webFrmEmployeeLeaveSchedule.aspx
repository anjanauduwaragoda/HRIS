<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="webFrmEmployeeLeaveSchedule.aspx.cs" Inherits="GroupHRIS.EmployeeLeave.webFrmEmployeeLeaveSchedule"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function openLOVWindow(file, window, ctlName) {
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
            //if (childWindow.opener == null) childWindow.opener = self;

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sRetVal) {

            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;

           
        }

        function getValueFromChild(sEmpId, sName, sCompanyId, sDepartmentId, sDivisionId) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sEmpId;

            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="tksmEmpleeLeaveShedule" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <span>Employee Leave Schedules</span>
    <hr />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="styleTable">
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label1" runat="server" Text="Employee Id"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtEmploeeId" runat="server" CssClass="styleTableCell2TextBox" MaxLength="8"
                            ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmploeeId')" />
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvEmployeeId" runat="server" ErrorMessage="Employee Id is required"
                            ControlToValidate="txtEmploeeId" ForeColor="Red" ValidationGroup="elSchedue">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                        &nbsp;</td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td class="styleTableCell2">
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfEmpId" runat="server" />
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
            </table>
            <br />
            <span>Leave Summary</span>
            <hr />
            <asp:GridView ID="gvLeaveSummary" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="LEAVE_TYPE_NAME" HeaderText="LEAVE TYPE NAME" />
                    <asp:BoundField DataField="NUMBER_OF_DAYS" HeaderText="NUMBER OF DAYS">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="leaves_taken" HeaderText="LEAVES TAKEN">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Leves_Remain" HeaderText="LEAVES REMAIN">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Label ID="lblLeaveMessage" runat="server"></asp:Label>
            <br />
            <br />
            <span>Enter Leave Details</span>
            <hr />
            <table>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label5" runat="server" Text="Covered By"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtCoveredBy" runat="server" CssClass="styleTableCell2TextBox" MaxLength="8"
                            ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtCoveredBy')" />
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvCoveredBy" runat="server" ErrorMessage="Covered by Required"
                            ControlToValidate="txtCoveredBy" ForeColor="Red" ValidationGroup="addGrid">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label6" runat="server" Text="Approved By"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:TextBox ID="txtApprovedBy" runat="server" CssClass="styleTableCell2TextBox"
                            MaxLength="8" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtApprovedBy')" />
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvApprovedBy" runat="server" ErrorMessage="Approved by Required"
                            ControlToValidate="txtApprovedBy" ForeColor="Red" ValidationGroup="addGrid">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label2" runat="server" Text="Leave Type"></asp:Label>
                    </td>
                    <td class="styleTableCell2">
                        <asp:DropDownList ID="ddlLeaveType" runat="server" CssClass="styleDdl" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlLeaveType_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvLeaveType" runat="server" ErrorMessage="Leave Type Required"
                            ControlToValidate="ddlLeaveType" ForeColor="Red" ValidationGroup="addGrid">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label3" runat="server" Text="Nature"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:DropDownList ID="ddlNature" runat="server" CssClass="styleDdlSmall" AutoPostBack="True"
                            Width="150px" OnSelectedIndexChanged="ddlNature_SelectedIndexChanged">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="1">Full Day</asp:ListItem>
                            <asp:ListItem Value="0.5">Half Day</asp:ListItem>
                            <asp:ListItem Value="0.25">SL</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvNature" runat="server" ErrorMessage="Nature is Required"
                            ControlToValidate="ddlNature" ForeColor="Red" ValidationGroup="addGrid">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label14" runat="server" Text="No of Days"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:TextBox ID="txtNoDays" runat="server" CssClass="styletxtSmall" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvNoDays" runat="server" ErrorMessage="Number of days Required"
                            ControlToValidate="txtNoDays" ForeColor="Red" ValidationGroup="addGrid">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label7" runat="server" Text="From Time"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:Label ID="Label8" runat="server" Text="HH"></asp:Label>
                        <asp:DropDownList ID="ddlFromHH" runat="server" CssClass="styleDdlhh">
                        </asp:DropDownList>
                        <asp:Label ID="Label9" runat="server" Text="MM"></asp:Label>
                        <asp:DropDownList ID="ddlFromMM" runat="server" CssClass="styleDdlhh">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                        <asp:RequiredFieldValidator ID="rfvFromHH" runat="server" ErrorMessage="From Time HH Required"
                            ControlToValidate="ddlFromHH" ForeColor="Red" ValidationGroup="addGrid">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvFromMM" runat="server" ErrorMessage="From Time MM Required"
                            ControlToValidate="ddlFromMM" ForeColor="Red" ValidationGroup="addGrid">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label10" runat="server" Text="To Time"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:Label ID="Label11" runat="server" Text="HH"></asp:Label>
                        <asp:DropDownList ID="ddlToHH" runat="server" CssClass="styleDdlhh">
                        </asp:DropDownList>
                        <asp:Label ID="Label12" runat="server" Text="MM"></asp:Label>
                        <asp:DropDownList ID="ddlToMM" runat="server" CssClass="styleDdlhh">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell3">
                        <asp:RequiredFieldValidator ID="rfvToHH" runat="server" ErrorMessage="To Time HH Required"
                            ControlToValidate="ddlToHH" ForeColor="Red" ValidationGroup="addGrid">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvToMM" runat="server" ErrorMessage="To Time MM Required"
                            ControlToValidate="ddlToMM" ForeColor="Red" ValidationGroup="addGrid">*</asp:RequiredFieldValidator>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label13" runat="server" Text="Reason"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="styleTableCell2TextBox" MaxLength="100"
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        &nbsp;</td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        <asp:Label ID="Label4" runat="server" Text="Date"></asp:Label>
                    </td>
                    <td class="styleTableCell2" style="text-align: left">
                        <asp:TextBox ID="txtLeaveDate" runat="server" Width="150px"></asp:TextBox>
                        <asp:CalendarExtender ID="ceLeaveDate" runat="server" TargetControlID="txtLeaveDate"
                            Format="yyyy/MM/dd">
                        </asp:CalendarExtender>
                        <asp:Button ID="btnAdd" runat="server" Text="&gt;&gt;&gt;" OnClick="btnAdd_Click"
                            ValidationGroup="addGrid" />
                    </td>
                    <td class="styleTableCell3">
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Leave Date Required"
                            ControlToValidate="txtLeaveDate" ForeColor="Red" ValidationGroup="addGrid">*</asp:RequiredFieldValidator>
                        <asp:FilteredTextBoxExtender ID="fteLeaveDate" runat="server" TargetControlID="txtLeaveDate"
                            FilterType="Custom, Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td class="styleTableCell5">
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px; text-align: right;">
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" ValidationGroup="elSchedue"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="125px" OnClick="btnCancel_Click" />
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px; text-align: right;">
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:HiddenField ID="hfPreviousDate" runat="server" />                        
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
            </table>
            <br />
            <asp:ValidationSummary ID="vsSchedue" runat="server" ForeColor="Red" 
                ValidationGroup="elSchedue" />
            <asp:ValidationSummary ID="vsAddGrid" runat="server" ForeColor="Red" 
                ValidationGroup="addGrid" />
            <br />
            <hr />
            <asp:GridView ID="gvLeaveBucket" runat="server" AutoGenerateColumns="false" ToolTip="Remove"
                OnSelectedIndexChanged="gvLeaveBucket_SelectedIndexChanged" OnRowDataBound="gvLeaveBucket_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="EMPLOYEE_ID" />
                    <asp:BoundField DataField="LEAVE_DATE" HeaderText="LEAVE_DATE" />
                    <asp:BoundField DataField="LEAVE_TYPE_ID" HeaderText="LEAVE_TYPE_ID" />
                    <asp:BoundField DataField="NO_OF_DAYS" HeaderText="NO_OF_DAYS" />
                </Columns>
            </asp:GridView>
            <br />
            <span>Leave History for This Year</span>
            <br />
            <br />
            <asp:GridView ID="gvLeaveHistory" runat="server" 
                onrowdatabound="gvLeaveHistory_RowDataBound" 
                onselectedindexchanged="gvLeaveHistory_SelectedIndexChanged">
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
