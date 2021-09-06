<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="LeaveBalanceReport.aspx.cs" Inherits="GroupHRIS.Reports.LeaveBalanceReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">

//    function openLOVWindow(file, window, ctlName) {
//        childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
//        //if (childWindow.opener == null) childWindow.opener = self;

//        document.getElementById("hfCaller").value = ctlName;
//    }

//    function getValueFromChild(sRetVal) {
//        var ctl = document.getElementById("hfCaller").value;
//        document.getElementById(ctl).value = sRetVal;
//    }

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <span style="font-weight: 700">Leave Balace Report</span>
    <hr />
    <br />    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>--%>
    <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                        <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
            <table style="margin: auto;">
                <tr>
                    <td>
                        Employee
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmployee" Width="250" ReadOnly="true" ClientIDMode="Static" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ValidationGroup="Main" ControlToValidate="txtEmployee" 
                            ErrorMessage="Invalid Employee ID Format" ForeColor="Red" Text="*" 
                            ValidationExpression="[a-zA-Z]{2}\d{6}"></asp:RegularExpressionValidator>

                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmployee')" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>Year</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList ID="ddlYear" Width="250" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>Company</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList ID="ddlCompany" Width="250" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="ddlCompany_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>Department</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList ID="ddlDepartment" Width="250" runat="server" >
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:Button ID="btnGenerate" ValidationGroup="Main" runat="server" Text="Generate Report" onclick="btnGenerate_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" 
                            onclick="btnClear_Click" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan='4'>
                        <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Main" ForeColor="Red" runat="server" />
                    </td>
                </tr>
            </table>
            <table style="margin: auto;">
                <tr>
                    <td>
                        <rsweb:ReportViewer ID="ReportViewer1" Width="850" Height="500" runat="server">
                        </rsweb:ReportViewer>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
</asp:Content>
