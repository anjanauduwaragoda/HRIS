<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="EmployeeTransferReport.aspx.cs" Inherits="GroupHRIS.Reports.EmployeeTransferReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <span style="font-weight: 700">Employee Transfer Report</span>
    <hr />
    <br />
    <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />    
    <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
                        <%--<asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />--%>
    <table style="margin: auto;" >
        <tr>
            <td>
                Employee
            </td>
            <td>
                :
            </td>
            <td>
                <asp:TextBox ID="txtEmployee" ClientIDMode="Static" Width="250" ReadOnly="true" runat="server"></asp:TextBox>
                <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmployee')" />
            </td>
            <td>
                
            </td>
            <td>
                
            </td>
            <td></td>
        </tr>
        <tr>
        <td>From Date</td>
        <td>:</td>
        <td>
                <asp:TextBox ID="txtTransferDate" placeholder="DD/MM/YYYY" Width="250" style="padding:none;" runat="server"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtTransferDate"
                    FilterType="Custom,Numbers" ValidChars="/" runat="server">
                </asp:FilteredTextBoxExtender>
                <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txtTransferDate" Format="dd/MM/yyyy"
                    runat="server">
                </asp:CalendarExtender>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtTransferDate"
                    runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                    ErrorMessage="Incorrect Date Format (From Date)" Text="*" ForeColor="Red"
                    ValidationGroup="Main"></asp:RegularExpressionValidator>
            </td>
        <td>To Date</td>
        <td>:</td>
        <td>
                <asp:TextBox ID="txtToDate" placeholder="DD/MM/YYYY" Width="250" runat="server"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderA" TargetControlID="txtToDate"
                    FilterType="Custom,Numbers" ValidChars="/" runat="server">
                </asp:FilteredTextBoxExtender>
                <asp:CalendarExtender ID="CalendarExtendera" 
                TargetControlID="txtToDate" Format="dd/MM/yyyy"
                    runat="server">
                </asp:CalendarExtender>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" 
                runat="server" ControlToValidate="txtToDate" 
                ErrorMessage="Incorrect Date Format (To Date)" ForeColor="Red" Text="*" 
                ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$" 
                ValidationGroup="Main"></asp:RegularExpressionValidator>
            </td>
        </tr>        
        <tr>
            <td>
                From
                Company
            </td>
            <td>
                :
            </td>
            <td>
                <asp:DropDownList ID="ddlFromCompany" CssClass="ddl" runat="server" Width="250" AutoPostBack="True" OnSelectedIndexChanged="ddlFromCompany_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                To
                Company
            </td>
            <td>
                :
            </td>
            <td>
                <asp:DropDownList ID="ddlToCompany" CssClass="ddl" runat="server" Width="250" AutoPostBack="True" OnSelectedIndexChanged="ddlToCompany_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                From
                Department
            </td>
            <td>
                :
            </td>
            <td>
                <asp:DropDownList ID="ddlFromDepartment" CssClass="ddl" Width="250" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFromDepartment_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                To
                Department
            </td>
            <td>
                :
            </td>
            <td>
                <asp:DropDownList ID="ddlToDepartment" CssClass="ddl" runat="server" Width="250" AutoPostBack="True" OnSelectedIndexChanged="ddlToDepartment_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                From
                Division
            </td>
            <td>
                :
            </td>
            <td>
                <asp:DropDownList ID="ddlFromDivision" CssClass="ddl" Width="250" 
                    runat="server" AutoPostBack="True" 
                    onselectedindexchanged="ddlFromDivision_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                To
                Division
            </td>
            <td>
                :
            </td>
            <td>
                <asp:DropDownList ID="ddlToDivision" CssClass="ddl" Width="250" runat="server" 
                    AutoPostBack="True" onselectedindexchanged="ddlToDivision_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="6">
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td style="text-align:left;">
                <asp:Button ID="btnGenerate" runat="server" ValidationGroup="Main" Text="Generate Report" onclick="btnGenerate_Click" />
                <asp:Button ID="btnclear" runat="server" Text="Clear" 
                    onclick="btnclear_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="6" style="text-align:center">
                <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="../Images/ProBar/720.GIF" />
                    </ProgressTemplate>
                </asp:UpdateProgress>--%>
                <asp:Label ID="lblMsg" runat="server" ></asp:Label>
                <table style="margin:auto;">
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="Main" ForeColor="Red" runat="server" />
                        </td>
                    </tr>
                </table>
                
            </td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td><%--Date of join--%></td>
            <td><%--:--%></td>
            <td>
                <asp:TextBox ID="txtDOJ" runat="server" Visible="false" placeholder='YYYY-MM-DD'></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID='txtDOJ' Format='yyyy-MM-dd' runat="server">
                </asp:CalendarExtender>
                <br />
            </td>
        </tr>
        <tr>
            <td><%--From Cost Center--%></td>
            <td><%--:--%></td>
            <td>
                <asp:TextBox ID="txtFromCC" Visible="false" runat="server"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Numbers" TargetControlID='txtFromCC' runat="server">
                </asp:FilteredTextBoxExtender>
            </td>
            <td><%--To Cost Center--%></td>
            <td><%--:--%></td>
            <td>
                <asp:TextBox ID="txtToCC" Visible="false" runat="server"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" FilterType="Numbers" TargetControlID='txtToCC' runat="server">
                </asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td><%--From Profit Center--%></td>
            <td><%--:--%></td>
            <td>
                <asp:TextBox ID="txtFromPC" Visible="false" runat="server"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" FilterType="Numbers" TargetControlID='txtFromPC' runat="server">
                </asp:FilteredTextBoxExtender>
            </td>
            <td><%--To Profit Center--%></td>
            <td><%--:--%></td>
            <td>
                <asp:TextBox ID="txtToPC" Visible="false" runat="server"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" FilterType="Numbers" TargetControlID='txtToPC' runat="server">
                </asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td><%--From Employer EPF--%></td>
            <td><%--:--%></td>
            <td>
                <asp:TextBox ID="txtFromEmployerEPF" Visible="false" runat="server"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" FilterType="Numbers" TargetControlID='txtFromEmployerEPF' runat="server">
                </asp:FilteredTextBoxExtender>
            </td>
            <td><%--To Employer EPF--%></td>
            <td><%--:--%></td>
            <td>
                <asp:TextBox ID="txtToEmployerEPF" Visible="false" runat="server"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" FilterType="Numbers" TargetControlID='txtToEmployerEPF' runat="server">
                </asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <%--<tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>--%>
    </table>
    
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <table style="margin: auto;">
        <tr>
            <td>
                <rsweb:reportviewer id="ReportViewer1" width="850" height="500" runat="server"  AsyncRendering="false" >
                </rsweb:reportviewer>


            </td>
        </tr>
    </table>

</asp:Content>
