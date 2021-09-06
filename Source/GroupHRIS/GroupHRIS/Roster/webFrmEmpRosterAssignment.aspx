<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="webFrmEmpRosterAssignment.aspx.cs" Inherits="GroupHRIS.Roster.webFrmEmpRosterAssignment" EnableEventValidation ="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function openLOVWindow(file, window, ctlName) {
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;
        }

        function getValueFromChild(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName) {
            document.getElementById("txtEmployeeID").value = sEmpId;
            document.getElementById("txtName").value = sName;

            writeToHiddenFields(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName);

            DoPostBack();
        }

        function writeToHiddenFields(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName) {
            document.getElementById("hfEmpID").value = sEmpId;
            document.getElementById("hfName").value = sName;
            document.getElementById("hfCompCode").value = sCompanyCode;
            
        }

        function DoPostBack() {
            __doPostBack("txtEmployeeID", "TextChanged");
        }


    </script>

    <style type="text/css">
    .hideGridColumn
    {
        display:none;
    }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>

    <br />
    <span>Roster Assignment</span>
    <hr />
    <br /> 


        <table class="styleMainTb">
        <tr>
            <td width="30%" align="right">
                &nbsp;<asp:Label ID="Label1" runat="server" Text="Employee" AssociatedControlID="txtEmployeeID"></asp:Label>
            </td>
            <td width="40%" align="left">
                <asp:TextBox ID="txtEmployeeID" runat="server" Width="250px" ClientIDMode="Static"
                    ViewStateMode="Enabled" ReadOnly="True" AutoPostBack="True" 
                    ValidationGroup="vgSubmit"></asp:TextBox>
                &nbsp;

                <img alt="Search for an Employee" src="../Images/Common/Search.jpg" height="20px"
                    width="20px" 
                    onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmployeeID')" 
                    id="imgSearch" />

            </td>
            <td width="30%" align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmployeeID"
                    ErrorMessage="Employee ID is required." ForeColor="Red" 
                    ValidationGroup="vgSubmit">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label2" runat="server" Text="Name " AssociatedControlID="txtName"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtName" runat="server" Width="250px" ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>





        <tr>
            <td align="right">
                <asp:Label ID="Label3" runat="server" Text="Roster" 
                    AssociatedControlID="ddlRosterID"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlRosterID" runat="server" 
                     Width="256px" onselectedindexchanged="ddlRosterID_SelectedIndexChanged" 
                    ValidationGroup="vgSubmit">
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlRosterID"
                    ErrorMessage="Roster is required." ForeColor="Red" 
                    ValidationGroup="vgSubmit">*</asp:RequiredFieldValidator>
            </td>
        </tr>





        <tr>
            <td align="right">
                <asp:Label ID="Label4" runat="server" Text="Date" AssociatedControlID="txtDate"></asp:Label>
            </td>
            <td>
                             
                <asp:TextBox ID="txtDate" runat="server" MaxLength="10" Width="250px" 
                    ValidationGroup="vgSubmit" ></asp:TextBox>

                <asp:CalendarExtender ID="ceDate" runat="server" TargetControlID="txtDate" 
                    Format="yyyy/MM/dd">
                </asp:CalendarExtender>

                <asp:FilteredTextBoxExtender ID="ftDate" runat="server" TargetControlID="txtDate" FilterType="Custom, Numbers" ValidChars="/" >
                </asp:FilteredTextBoxExtender> 

                &nbsp;&nbsp;</td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDate"
                    ErrorMessage="Date is required." ForeColor="Red" 
                    ValidationGroup="vgSubmit">*</asp:RequiredFieldValidator>
            </td>
        </tr>





        <tr>
            <td align="right">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>


        <tr>
            <td  align="right">&nbsp;</td>

            <td  align="left">
                <asp:Button ID="btnAdd" runat="server" onclick="btnAdd_Click"  ValidationGroup="vgSubmit"
                    Text="Save" Width="125px" />
                             
                <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="125px" 
                    onclick="btnCancel_Click" />
            </td>

            <td  align="left">
                &nbsp;</td>
        </tr>

        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td>
                <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfEmpID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfCompCode" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfName" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfLineNo" runat="server" ClientIDMode="Static" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>

        </table>


    <table  class="styleMainTb">
    <tr>
        <td align="center">
            <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:Label ID="lblMsg" runat="server" Font-Size="10pt" ForeColor="Blue"></asp:Label>
        </td>
    </tr>
    <tr> 
        <td align="left">
            <asp:ValidationSummary ID="vsSubmit" runat="server" 
                ValidationGroup="vgSubmit" ForeColor="Red" />
        </td>
    </tr>
    </table>


    <br />
    <span>List of Roster Assignments</span>
    <hr />
    <br />

    <div>
    
        <asp:GridView ID="gvRosterAssignments" runat="server" AutoGenerateColumns="False" 
            Width="100%" PageSize="20" 
            onpageindexchanging="gvRosterAssignments_PageIndexChanging" 
            onrowdatabound="gvRosterAssignments_RowDataBound" 
            onselectedindexchanged="gvRosterAssignments_SelectedIndexChanged" 
            onrowcommand="gvRosterAssignments_RowCommand" >
            <Columns>
                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Emp ID" />
                <asp:BoundField DataField="ROSTR_ID" HeaderText="Roster ID" />

                <asp:BoundField DataField="DUTY_DATE" HeaderText="Duty Date"  DataFormatString="{0:yyyy/MM/dd}" />
                <asp:BoundField DataField="FROM_TIME" HeaderText="From Time" />
                <asp:BoundField DataField="TO_TIME" HeaderText="To Time" />
                <asp:BoundField DataField="description" HeaderText="Roster Type" />

                <asp:BoundField DataField="IS_OFF_DAY" HeaderText="Off Day?" 
                    HeaderStyle-CssClass = "hideGridColumn" ItemStyle-CssClass="hideGridColumn" >
                    <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>

                    <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="IS_SUMMARIZED" HeaderText="Summarized" Visible="False" />
                <asp:BoundField DataField="INTERCHANGE_NUMBER" HeaderText="Interchange Num" />
                <asp:BoundField DataField="DUTY_COVERED_BY" HeaderText="Covered By ID" />
                <asp:BoundField DataField="FIRST_NAME" HeaderText="Covered By Name" />
                <asp:BoundField DataField="REASON" HeaderText="Reason" />
                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status"  />

                <asp:ButtonField HeaderText="Obsolete" Text="Obsolete" CommandName="Obsolete" />

            </Columns>
        </asp:GridView>
    
    </div>

</asp:Content>
