<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="webFrmBranchManager.aspx.cs" Inherits="GroupHRIS.MetaData.webFrmBranchManager" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
       
    </style>
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
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <table class="styleMainTb">
        <tr>
            <td>
                <span style="font-weight: 700; font-size: small">Branch Manager Details </span>
            </td>
            <td class="styleMainTbRightTD">
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Company :
            </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlCompID" runat="server" Height="20px" Width="400px" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlCompID_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Branch :
            </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlBranchID" runat="server" Height="20px" Width="400px" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlBranchID_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Branch ID is required."
                    ForeColor="Red" ValidationGroup="vgBranchManager" ControlToValidate="ddlBranchID">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Employee :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtEmpID" runat="server" ClientIDMode="Static" Width="120px" ReadOnly= "true"></asp:TextBox>
                <img alt="" src="../../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../../Employee/webFrmEmployeeSearch.aspx','Search','txtEmpID')" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Employee Id is required."
                    ForeColor="Red" ValidationGroup="vgBranchManager" 
                    ControlToValidate="txtEmpID">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Start Date : 
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtDateStart" runat="server" Width="120px"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtDateStart"
                    ControlToValidate="txtDateEnd" ErrorMessage="Start Date cannot be greater than End Date ."
                    ForeColor="Red" Operator="GreaterThan" ValidationGroup="vgBranchManager">*</asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDateStart"
                    ErrorMessage="Date Start is required." ForeColor="Red" ValidationGroup="vgBranchManager">*</asp:RequiredFieldValidator>
                <asp:CalendarExtender ID="ceDateStart" runat="server" TargetControlID="txtDateStart"
                    Format="yyyy/MM/dd">
                </asp:CalendarExtender>
                <asp:FilteredTextBoxExtender ID="fteDateStart" runat="server" TargetControlID="txtDateStart"
                    FilterType="Custom, Numbers" ValidChars="/">
                </asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                End Date :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtDateEnd" runat="server" Width="120px" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                <asp:CalendarExtender ID="ceDateEnd" runat="server" TargetControlID="txtDateEnd"
                    Format="yyyy/MM/dd">
                </asp:CalendarExtender>
                <asp:FilteredTextBoxExtender ID="fteDateEnd" runat="server" TargetControlID="txtDateEnd"
                    FilterType="Custom, Numbers" ValidChars="/">
                </asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Remarks :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtRemarks" runat="server" Width="400px" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Status :
            </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlStatus" runat="server" Height="20px" Width="120px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlStatus"
                    ErrorMessage="Status Code is required." ForeColor="Red" ValidationGroup="vgBranchManager">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
            </td>
            <td class="styleMainTbRightTD">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled"/>
                    <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            </td>
            <td class="styleMainTbRightTD">
                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="vgBranchManager"
                    Width="100px" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="100px" 
                    OnClick="btnCancel_Click" />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                
            </td>
            <td class="styleMainTbRightTD">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="vgBranchManager" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <div>
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                        OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                        AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="LINE_NO" HeaderText="Line No" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                            <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Employee ID" />
                            <asp:BoundField DataField="BRANCH_ID" HeaderText="Branch ID" />
                            <asp:BoundField DataField="BRANCH_NAME" HeaderText="Name" />
                            <asp:BoundField DataField="DATE_START" HeaderText="Date Start" />
                            <asp:BoundField DataField="DATE_END" HeaderText="Date End" />
                            <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                        </Columns>
                        <PagerSettings PageButtonCount="2" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
            </td>
            <td class="styleMainTbRightTD">
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;
            </td>
            <td class="styleMainTbRightTD">
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
