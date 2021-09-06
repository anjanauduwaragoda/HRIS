<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmReturnedProprty.aspx.cs" Inherits="GroupHRIS.Property.WebFrmReturnedProprty" %>

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
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        .style1
        {
            text-align: right;
            height: 70px;
        }
        .style2
        {
            height: 70px;
        }
    </style>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <span style="font-weight: 700">Update Returned Nonfinancial Benifits</span><br />
    <hr />
    <br />
    <table>
        <tr>
            <td style="width: 200px; text-align: right">
                Employee :
            </td>
            <td style="width: 250px; text-align: left">
                <asp:TextBox ID="txtEmploeeId" runat="server" ClientIDMode="Static" Width="200px"
                   AutoPostBack="True" ReadOnly="True"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmployeeId" runat="server" Text=" * " ErrorMessage="Employee Id is required"
                    ControlToValidate="txtEmploeeId" ForeColor="Red" ValidationGroup="Property"></asp:RequiredFieldValidator>
                <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmploeeId')" />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Employee Name :
            </td>
            <td>
                <asp:Label ID="lblEmployeeName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Benefit Name :
            </td>
            <td>
                <asp:TextBox ID="txtPropertyName" runat="server" Width="200px" Enabled="False"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Assigned Date :
            </td>
            <td>
                <asp:TextBox ID="txtAssignDate" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Property" ErrorMessage="Assign date is required"
                    Text=" * " ForeColor="Red" ControlToValidate="txtAssignDate"></asp:RequiredFieldValidator>
                <asp:CalendarExtender ID="ceAssignDate" runat="server" TargetControlID="txtAssignDate"
                    Format="dd/MM/yyyy">
                </asp:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Returned Date :
            </td>
            <td>
                <asp:TextBox ID="txtReturnedDate" runat="server" Width="200px" onkeydown="return (event.keyCode!=13);"
                    AutoPostBack="True" Enabled="False"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Returned date is required "
                    ValidationGroup="Property" Text=" * " ForeColor="Red" ControlToValidate="txtReturnedDate"></asp:RequiredFieldValidator>
                <asp:CalendarExtender ID="ceReturnedDate" runat="server" TargetControlID="txtReturnedDate"
                    Format="yyyy-MM-dd">
                </asp:CalendarExtender>
                <asp:FilteredTextBoxExtender ID="fteRuturnedDate" FilterType="Custom, Numbers" ValidChars="-"
                    runat="server" TargetControlID="txtReturnedDate">
                </asp:FilteredTextBoxExtender>
                <asp:RegularExpressionValidator ID="revfrmDate" runat="server" ValidationExpression="(19|20)\d\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])"
                    Text="*"  ErrorMessage="Invalid date format" ValidationGroup="Property" ControlToValidate="txtReturnedDate" 
                        ForeColor="Red" ></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Benefit Status :
            </td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server" Width="205px" Enabled="False">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Remarks :
            </td>
            <td class="style6">
                <asp:TextBox ID="txtRemarks" Width="200px" runat="server" Enabled="False"></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td class="style1">
                <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
            </td>
            <td class="style2">
                <asp:Button ID="btnsave" runat="server" Text="Update" Width="100px" ValidationGroup="Property"
                    OnClick="btnsave_Click" Enabled="False" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
            </td>
            <td class="style2">
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
            </td>
            <td>
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="hfAssignId" runat="server" />
                <asp:HiddenField ID="hfPropertyId" runat="server" />
                <asp:HiddenField ID="hfmail" runat="server" />
            </td>
            <td colspan="2">
                <asp:Label ID="StatusLabel" runat="server"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Property"
                    ForeColor="RED" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="gvemployeeProperty" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
        AllowPaging="true" OnRowDataBound="gvemployeeProperty_RowDataBound" OnPageIndexChanging="gvemployeeProperty_PageIndexChanging"
        OnSelectedIndexChanged="gvemployeeProperty_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="LINE_ID" HeaderText=" Assign Id " />
            <asp:BoundField DataField="PROPERTY_ID" HeaderText=" Employee Benefit Id " HeaderStyle-CssClass="hideGridColumn"
                ItemStyle-CssClass="hideGridColumn" />
            <asp:BoundField DataField="DESCRIPTION" HeaderText=" Benefit Name " />
            <asp:BoundField DataField="ASSIGNED_DATE" HeaderText=" Assign Date " />
            <asp:BoundField DataField="RETURNED_DATE" HeaderText=" Return Date " />
            <asp:BoundField DataField="CLEARANCE_MAIL" HeaderText=" Clearance mail " />
            <asp:BoundField DataField="PROPERTY_STATUS" HeaderText=" Benefit Status " />
            <asp:BoundField DataField="REMARKS" HeaderText=" Remarks " />
        </Columns>
    </asp:GridView>
</asp:Content>
