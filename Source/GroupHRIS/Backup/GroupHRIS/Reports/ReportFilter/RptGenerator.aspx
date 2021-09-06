<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="RptGenerator.aspx.cs" Inherits="GroupHRIS.Reports.ReportFilter.RptGenerator" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/StyleReports.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        var txb;
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=1366,height=768,scrollbars=yes,top=50,left=200,status=yes');
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
    <table>
        <tr>
            <td>
                <span style="font-weight: 700">Report Generator</span>
            </td>
        </tr>
    </table>
    <hr />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        

    <div>
        <table style="margin:auto;">
            <tr>
                <td align="center" colspan="3">
                    
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Report Code :</td>
                <td>
                    <asp:Label ID="lblrepname" runat="server" Font-Bold="True"></asp:Label>
                </td>
                <td class="reportRightTD">
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Company :
                </td>
                <td>
                    <asp:DropDownList ID="dpCompID" runat="server" Width="200px" 
                        AutoPostBack="True" onselectedindexchanged="dpCompID_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="reportRightTD">
                    &nbsp;</td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <%--<asp:RadioButton ID="rdBtnDept" runat="server" AutoPostBack="true" 
                        Checked="true" Text="Filter By Department" GroupName="rdFilter" 
                        oncheckedchanged="rdBtnDept_CheckedChanged" />
                    <asp:RadioButton ID="rdBtnBrnch" runat="server" AutoPostBack="true" 
                        Text="Filter By Branch" GroupName="rdFilter" 
                        oncheckedchanged="rdBtnBrnch_CheckedChanged" />--%>
                    <asp:CheckBox ID="chkBranch" Text="Filter By Branch" AutoPostBack="true" 
                        runat="server" oncheckedchanged="chkBranch_CheckedChanged" />
                    <br />
                </td>
                <td></td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    <asp:Label ID="Label2" runat="server" Text="Department :"></asp:Label>
                    <asp:Label ID="Label1" runat="server" Visible="false" Text="Branch :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDepartment" runat="server" Width="200px">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlBranch" Visible="false" runat="server" Width="200px">
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    From Date :
                </td>
                <td>
                    <asp:TextBox ID="txtfromdate" runat="server" MaxLength="10" Width="150px" placeholder="DD/MM/YYYY" ></asp:TextBox>
                    <asp:CalendarExtender ID="cefromdate" runat="server" TargetControlID="txtfromdate" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="ftefromdate" runat="server" TargetControlID="txtfromdate" 
                        FilterType="Custom, Numbers" ValidChars="/">
                    </asp:FilteredTextBoxExtender>
                    <asp:RegularExpressionValidator ID="revfrmDate" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                        ErrorMessage="(DD/MM/YYYY)" ControlToValidate="txtfromdate" 
                        ForeColor="Red" ></asp:RegularExpressionValidator>
                </td>
                
                <td class="reportRightTD">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    To Date :
                </td>
                <td>
                    <asp:TextBox ID="txttodate" runat="server" MaxLength="10" placeholder="DD/MM/YYYY" Width="150px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    <asp:CalendarExtender ID="cetodate" runat="server" TargetControlID="txttodate" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txttodate" 
                        FilterType="Custom, Numbers" ValidChars="/">
                    </asp:FilteredTextBoxExtender>
                    
                    <asp:RegularExpressionValidator ID="revtodate" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                        ErrorMessage="(DD/MM/YYYY)" ControlToValidate="txttodate" 
                        ForeColor="Red" ></asp:RegularExpressionValidator>
                </td>
                <td class="reportRightTD">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Employee :
                </td>
                <td>
                    <asp:TextBox ID="txtemployee" runat="server" ClientIDMode="Static" ReadOnly="true" Width="150px"
                        onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    <img alt="" src="../../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('/Employee/webFrmEmployeeSearch.aspx','Search','txtemployee')" />
                   
                </td>
                <td class="reportRightTD">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    <asp:Label ID="lblStatusCode" runat="server" Visible="false" Text="Employee Status :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlEmpStatus" Visible="false" runat="server" Width="200px">
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>
            <tr>
                <td class="reportLeftTD">
                    <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled"/>
                    <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                </td>
                <td>
                    <asp:CheckBox ID="chkemployee" runat="server" AutoPostBack="True" Font-Bold="False"
                        ForeColor="Blue" Text="Selected Employee Only" />
                    <br />
                    <asp:HiddenField ID="hfRepID" runat="server" />
                </td>
                <td class="reportRightTD">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="reportLeftTD">
                    &nbsp;
                </td>
                <td>
                    <asp:UpdateProgress ID="upProgressBar" runat="server" style="margin: auto; width: auto">
                        <ProgressTemplate>
                            <img src="/Images/ProBar/720.GIF" alt="" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
                <td class="reportRightTD">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="reportLeftTD">
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click1" 
                        Text="Run Report" style="height: 26px" />
                    <asp:Button ID="btnClear" runat="server"  
                        Text="Clear" style="height: 26px" Width="100" onclick="btnClear_Click" />
                </td>
                <td class="reportRightTD">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
