<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="InOutConfigurationReport.aspx.cs" Inherits="GroupHRIS.Reports.InOutConfigurationReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">

        var txb;
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=1366,height=768,scrollbars=yes,top=50,left=200,status=yes');

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;

            var id = document.getElementById(txb).value;
            document.getElementById("hfVal").value = id;

            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }

    </script>
    <style type="text/css">
        input, select, textarea 
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <span style="font-weight: 700">IN-OUT Configuration Report</span>
    <hr />
    <br />    
    <asp:ToolkitScriptManager runat="server">
    </asp:ToolkitScriptManager>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>--%>
            <table style="margin: auto;">
                <tr>
                    <td>
                        From Date
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" placeholder="DD/MM/YYYY" Width="250" runat="server"></asp:TextBox>
                       <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtFromDate"
                            FilterType="Custom,Numbers" ValidChars="/" runat="server">
                        </asp:FilteredTextBoxExtender>
                        <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txtFromDate" Format="dd/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtFromDate"
                            runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                            ErrorMessage="Invalid From Date [DD/MM/YYYY]" Text="*" ForeColor="Red" ValidationGroup="Main"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ErrorMessage="From Date is Required" ForeColor="Red" ControlToValidate="txtFromDate" ValidationGroup="Main" Text="*"></asp:RequiredFieldValidator>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        To Date
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" placeholder="DD/MM/YYYY" Width="250" runat="server"></asp:TextBox>
                       
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtToDate"
                            FilterType="Custom,Numbers" ValidChars="/" runat="server">
                        </asp:FilteredTextBoxExtender>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtToDate" Format="dd/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtToDate"
                            runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                            ErrorMessage="Invalid To Date  [DD/MM/YYYY]" Text="*" ForeColor="Red" ValidationGroup="Main"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ErrorMessage="To Date is Required" ForeColor="Red" ControlToValidate="txtToDate" ValidationGroup="Main" Text="*"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>Company</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList ID="ddlCompany" runat="server" Width="250" AutoPostBack="True" 
                            onselectedindexchanged="ddlCompany_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>Department</td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList ID="ddlDepartment" runat="server" AutoPostBack="True" Width="250" 
                            onselectedindexchanged="ddlDepartment_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                <td>Employee</td>
                <td>:</td>
                <td>
                <asp:TextBox ID="txtEmployee" ClientIDMode="Static" Width="250" ReadOnly="true" runat="server"></asp:TextBox>
                <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmployee')" />
                
            </td>
                <td></td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:Button ID="btnGenerate" ValidationGroup="Main" runat="server" Text="Generate Report" onclick="btnGenerate_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100" 
                            onclick="btnClear_Click" />
                    <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled"/>
                    <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblerror" runat="server" ></asp:Label>
                        <br />
                        <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <asp:Label ID="lblWait" runat="server" Text="Please Wait ..."></asp:Label>
                            </ProgressTemplate>
                        </asp:UpdateProgress>--%>
                        

                        <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Main" ForeColor="Red"
                            runat="server" />
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
</asp:Content>
