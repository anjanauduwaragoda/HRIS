<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation = "false" AutoEventWireup="true" CodeBehind="webFrmDivisionHead.aspx.cs" Inherits="GroupHRIS.MetaData.webFrmDivisionHead" %>
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
             <td colspan = "2"  > 
            <span style="font-weight: 700">Division Head Details</span>
                </td>
            
        </tr>
        <tr>
            <td class="styleMainTbLeftTD" colspan="2">
                <hr /></td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Company :</td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlCompID" runat="server" Height="20px" Width="400px" 
                    AutoPostBack="True" onselectedindexchanged="ddlCompID_SelectedIndexChanged" 
                       >
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Department : </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlDepID" runat="server" Height="20px" Width="400px" 
                    AutoPostBack="True" 
                    onselectedindexchanged="ddlDepID_SelectedIndexChanged"  >
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Division :</td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlDivId" runat="server" Height="20px" Width="400px" 
                    AutoPostBack="True" onselectedindexchanged="ddlDivId_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvDivId" runat="server" 
                    ControlToValidate="ddlDivId" ErrorMessage="Division ID is required." 
                    ForeColor="Red" ValidationGroup="vgDivHeadInfo">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Employee :</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtEmpID" runat="server"  ClientIDMode ="Static" Width="120px" ReadOnly="true"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmpID" runat="server" 
                    ControlToValidate="txtEmpID" ErrorMessage="Employee Id is required." 
                    ForeColor="Red" ValidationGroup="vgDivHeadInfo">*</asp:RequiredFieldValidator>
                   <img alt="" src="../../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../../Employee/webFrmEmployeeSearch.aspx','Search','txtEmpID')" />

                   
            </td>
            
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Start Date : </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtDateStart" runat="server" Width="120px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDateStart" runat="server" 
                    ControlToValidate="txtDateStart" ErrorMessage="Start Date is required." 
                    ForeColor="Red" ValidationGroup="vgDivHeadInfo">*</asp:RequiredFieldValidator>
                    <asp:CalendarExtender ID="ceDateStart" runat="server" TargetControlID="txtDateStart" Format="yyyy/MM/dd">
                    </asp:CalendarExtender> 
                    <asp:FilteredTextBoxExtender ID="fteDateStart" runat="server" TargetControlID="txtDateStart" FilterType="Custom, Numbers" ValidChars="/" >
                    </asp:FilteredTextBoxExtender>
                <asp:CompareValidator ID="cvDateStart" runat="server" 
                    ControlToCompare="txtDateStart" ControlToValidate="txtDateEnd" 
                    ErrorMessage="Start Date cannot be greater than End Date." ForeColor="Red" 
                    ValidationGroup="vgDivHeadInfo" Operator="GreaterThan">*</asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                End Date :</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtDateEnd" runat="server" Width="120px"></asp:TextBox>
                    <asp:CalendarExtender ID="ceDateEnd" runat="server" TargetControlID="txtDateEnd" Format="yyyy/MM/dd">
                    </asp:CalendarExtender> 
                    <asp:FilteredTextBoxExtender ID="fteDateEnd" runat="server" TargetControlID="txtDateEnd" FilterType="Custom, Numbers" ValidChars="/" >
                    </asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Remarks :</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtRemarks" runat="server" Width="400px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Status :</td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlStatus" runat="server" Height="20px" Width="120px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvStatus" runat="server" 
                    ControlToValidate="ddlStatus" ErrorMessage="Status in required." 
                    ForeColor="Red" ValidationGroup="vgDivHeadInfo">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" 
                    onclick="btnSave_Click" ValidationGroup="vgDivHeadInfo" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="100px" 
                    onclick="btnCancel_Click" />
            </td>
        </tr>
        <tr>
           
            <td class="styleMainTbLeftTD">
                <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" 
                    ViewStateMode="Enabled" /> 
                     <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />             
                </td>
                <td class="styleMainTbRightTD">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" 
                    ValidationGroup="vgDivHeadInfo" />
                
                </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD" colspan="2">
                <hr /></td>
        </tr>
        <tr>
            <td  colspan="2" align="center"            >
             <div class="stylediv">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                     onselectedindexchanged="GridView1_SelectedIndexChanged" AllowPaging="True" onpageindexchanging="GridView1_PageIndexChanging" 
                     onrowdatabound="GridView1_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="LINE_NO" HeaderText="Line No" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="DIVISION_ID" HeaderText="Division ID" />
                        <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Employee ID" />
                        <asp:BoundField DataField="FIRST_NAME" HeaderText="Employee Name" />
                        <asp:BoundField DataField="DATE_START" HeaderText="Date Start" />
                        <asp:BoundField DataField="DATE_END" HeaderText="Date End" />
                        <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                    </Columns>
                </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
