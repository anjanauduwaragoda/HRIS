<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/Mastermain.Master"
    CodeBehind="WebFrmOvertimeProcess.aspx.cs" Inherits="GroupHRIS.PayRoll.WebFrmOvertimeProcess"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <span style="font-weight: 700">Over Time Adjustments</span><br />
            <hr />
            <br />
            <table class="styleMainTb">
                <tr>
                    <td class="styleMainTbLeftTD">
                        Employee Id :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmploeeId" runat="server" ClientIDMode="Static" ReadOnly="true"
                            OnTextChanged="txtEmploeeId_TextChanged" Width="170px" AutoPostBack="True"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvEmployeeId" runat="server" ErrorMessage="Employee Id is required"
                            ControlToValidate="txtEmploeeId" ForeColor="Red" ValidationGroup="Transaction">*</asp:RequiredFieldValidator>
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmploeeId')" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Company Name :
                    </td>
                    <td class="style3">
                        <asp:Label ID="lblCompany" runat="server"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Employee Name :
                    </td>
                    <td class="style3">
                        <asp:Label ID="lblEmployeeName" runat="server"></asp:Label>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Month :
                    </td>
                    <td class="style6" width="200">
                        <asp:DropDownList ID="ddlYear" AutoPostBack="true" runat="server" 
                            onselectedindexchanged="ddlYear_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlMonth" AutoPostBack="true" runat="server" 
                            onselectedindexchanged="ddlMonth_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Normal OT :
                    </td>
                    <td class="style6">
                        <asp:TextBox ID="txtNormalOT" Width="170px" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RadioButton ID="rbtnNOTActive" text = "Active" AutoPostBack="true" 
                            GroupName = "NOT" Checked="true" runat="server" 
                            oncheckedchanged="rbtnNOTActive_CheckedChanged" />
                        <asp:RadioButton ID="rbtnNOTDeactive" Text = "Inactive" AutoPostBack="true" 
                            GroupName = "NOT" runat="server" 
                            oncheckedchanged="rbtnNOTDeactive_CheckedChanged" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Double OT :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDoubleOT" Width="170px" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RadioButton ID="rbtnDOTActive" text = "Active" AutoPostBack="true" 
                            GroupName = "DOT" Checked="true" runat="server" 
                            oncheckedchanged="rbtnDOTActive_CheckedChanged" />
                        <asp:RadioButton ID="rbtnDOTDeactive" Text = "Inactive" AutoPostBack="true" 
                            GroupName = "DOT" runat="server" 
                            oncheckedchanged="rbtnDOTDeactive_CheckedChanged" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Attendance Incentive :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAttendanceIncentive" Width="170px" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RadioButton ID="rbtnAttendanceIncentiveActive" AutoPostBack="true" 
                            text = "Active" GroupName = "AI" Checked="true" runat="server" 
                            oncheckedchanged="rbtnAttendanceIncentiveActive_CheckedChanged" />
                        <asp:RadioButton ID="rbtnAttendanceIncentiveDeactive" AutoPostBack="true" 
                            Text = "Inactive" GroupName = "AI" runat="server" 
                            oncheckedchanged="rbtnAttendanceIncentiveDeactive_CheckedChanged" />
                    </td>
                    <td>
                    </td>
                </tr>
                
                <tr>
                    <td class="styleMainTbLeftTD">
                        Remarks :
                    </td>
                    <td class="style6">
                        <asp:TextBox ID="txtRemarks" runat="server" Width="170px" Height="49px" TextMode="MultiLine"
                            Style="resize: none"></asp:TextBox>
                    </td>
                    <td style="vertical-align:top;">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Remarks required"
                            ControlToValidate="txtRemarks" ForeColor="Red" ValidationGroup="Transaction">*</asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
                    </td>
                    <td class="style4" colspan="2">
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Width="100px" ValidationGroup="Transaction"
                            OnClick="btnUpdate_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="StatusLabel" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Transaction"
                ForeColor="RED" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>