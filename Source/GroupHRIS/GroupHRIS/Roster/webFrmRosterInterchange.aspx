<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" CodeBehind="webFrmRosterInterchange.aspx.cs" Inherits="GroupHRIS.Roster.webFrmRosterInterchange" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function openLOVWindow(file, window, ctlName1, ctlName2) {
            document.getElementById("hfCaller").value = ctlName1;
            document.getElementById("hfCaller2").value = ctlName2;
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
        }


        function getValueFromChild(sRetVal) {
            var ctl = document.getElementById("hfCaller").value;
            document.getElementById(ctl).value = sRetVal;
        }

        function getValueFromChild(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName) {
            //document.getElementById("txtEmployeeID").value = sEmpId;
            //document.getElementById("txtName").value = sName;

            var ctl1 = document.getElementById("hfCaller").value;
            var ctl2 = document.getElementById("hfCaller2").value;

            document.getElementById(ctl1).value = sEmpId;
            document.getElementById(ctl2).value = sName;

            if(ctl1== "txtEmployeeID")
                writeToHiddenFieldsExisting(sEmpId, sName);
            else
                writeToHiddenFieldsInterchanger(sEmpId, sName);

            //DoPostBack();
        }

        function writeToHiddenFieldsExisting(sEmpId, sName) {
            document.getElementById("hfEmpID").value = sEmpId;
            document.getElementById("hfName").value = sName;
        }

        function writeToHiddenFieldsInterchanger(sEmpId, sName) {
            document.getElementById("hfInterchanger").value = sEmpId;
            document.getElementById("hfInterchangerName").value = sName;
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
    <span>Roster Interchange</span>
    <hr />


        <table class="styleMainTb">
        <tr>
            <td width="30%" align="right">
                &nbsp;<asp:Label ID="Label1" runat="server" 
                    Text="Employee (with Existing Roster)" AssociatedControlID="txtEmployeeID"></asp:Label>
            </td>
            <td width="40%" align="left">
                <asp:TextBox ID="txtEmployeeID" runat="server" Width="250px" ClientIDMode="Static"
                    ViewStateMode="Enabled" ReadOnly="True" AutoPostBack="True"></asp:TextBox>
                &nbsp;

                <img alt="Search for an Employee" src="../Images/Common/Search.jpg" height="20px"
                    width="20px" 
                    onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmployeeID','txtName')" 
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
                <asp:Label ID="Label4" runat="server" Text="Date" AssociatedControlID="txtName"></asp:Label>
            </td>
            <td>
                             
                <asp:TextBox ID="txtDate" runat="server" MaxLength="10" Width="250px" ></asp:TextBox>

                <asp:CalendarExtender ID="ceDate" runat="server" TargetControlID="txtDate" 
                    Format="yyyy/MM/dd">
                </asp:CalendarExtender>

                <asp:FilteredTextBoxExtender ID="ftDate" runat="server" TargetControlID="txtDate" FilterType="Custom, Numbers" ValidChars="/" >
                </asp:FilteredTextBoxExtender> 
            
            &nbsp;<asp:ImageButton ID="ibtnAdd" runat="server" 
                    ImageUrl="~/Images/Roster/load.jpg" Width="16px" onclick="ibtnAdd_Click" />
                             
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDate"
                    ErrorMessage="Date is required." ForeColor="Red" 
                    ValidationGroup="vgSubmit">*</asp:RequiredFieldValidator>
            </td>
        </tr>

        <tr>
            <td align="right">
                <asp:Label ID="Label6" runat="server" Text="Roster" 
                    AssociatedControlID="txtName"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtRosterID" runat="server" ReadOnly="True" Width="250px"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtRosterID"
                    ErrorMessage="Roster is required." ForeColor="Red" 
                    ValidationGroup="vgSubmit">*</asp:RequiredFieldValidator>
            </td>
        </tr>

        <tr>
            <td align="right">
                <asp:Label ID="Label7" runat="server" Text="From Time / To Time" 
                    AssociatedControlID="txtName"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtFromTime" runat="server" Width="121px" 
                    ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
                <asp:TextBox ID="txtToTime" runat="server" Width="121px" ClientIDMode="Static" 
                    ReadOnly="True"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
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
            <td width="30%" align="right">
                &nbsp;<asp:Label ID="Label3" runat="server" 
                    Text="Employee (Interchanger)" AssociatedControlID="txtEmployeeID"></asp:Label>
            </td>
            <td width="40%" align="left">
                <asp:TextBox ID="txtInterchangerID" runat="server" Width="250px" ClientIDMode="Static"
                    ViewStateMode="Enabled" ReadOnly="True" AutoPostBack="True"></asp:TextBox>
                &nbsp;

                <img alt="Search for an Employee" src="../Images/Common/Search.jpg" height="20px"
                    width="20px" 
                    onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtInterchangerID','txtInterchangerName')" 
                    id="img1" />

            </td>
            <td width="30%" align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtInterchangerID"
                    ErrorMessage="Employee ID (Interchanger) is required." ForeColor="Red" 
                    ValidationGroup="vgSubmit">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label5" runat="server" Text="Name " AssociatedControlID="txtName"></asp:Label>
            </td>
            <td align="left">
                <asp:TextBox ID="txtInterchangerName" runat="server" Width="250px" 
                    ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>


        <tr>
            <td align="right">
                <asp:Label ID="Label8" runat="server" Text="Date" AssociatedControlID="txtName"></asp:Label>
            </td>
            <td>
                             
                <asp:TextBox ID="txtDateInterchanger" runat="server" MaxLength="10" 
                    Width="250px" ></asp:TextBox>

                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateInterchanger" 
                    Format="yyyy/MM/dd">
                </asp:CalendarExtender>

                <asp:FilteredTextBoxExtender ID="ftDateInt" runat="server" TargetControlID="txtDateInterchanger" FilterType="Custom, Numbers" ValidChars="/" >
                </asp:FilteredTextBoxExtender> 
            
            &nbsp;<asp:ImageButton ID="ibtnAdd2" runat="server" 
                    ImageUrl="~/Images/Roster/load.jpg" Width="16px" 
                    onclick="ibtnAdd2_Click" />
                             
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDateInterchanger"
                    ErrorMessage="Date is required." ForeColor="Red" 
                    ValidationGroup="vgSubmit">*</asp:RequiredFieldValidator>
            </td>
        </tr>

        <tr>
            <td align="right">
                <asp:Label ID="Label9" runat="server" Text="Roster" 
                    AssociatedControlID="txtName"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtRosterInterganger" runat="server" ReadOnly="True" 
                    Width="250px"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtRosterInterganger"
                    ErrorMessage="Roster is required." ForeColor="Red" 
                    ValidationGroup="vgSubmit">*</asp:RequiredFieldValidator>
            </td>
        </tr>

        <tr>
            <td align="right">
                <asp:Label ID="Label10" runat="server" Text="From Time / To Time" 
                    AssociatedControlID="txtName"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtFromTimeIntger" runat="server" Width="121px" 
                    ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
                <asp:TextBox ID="txtToTimeInter" runat="server" Width="121px" ClientIDMode="Static" 
                    ReadOnly="True"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>



        <tr>
            <td align="right">
                <asp:Label ID="Label11" runat="server" Text="Reason" 
                    AssociatedControlID="txtName"></asp:Label>
            </td>
            <td>
                             
                <asp:TextBox ID="txtReason" runat="server" MaxLength="45" Rows="2" 
                    TextMode="MultiLine" Width="250px"></asp:TextBox>
                             
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtReason"
                    ErrorMessage="Reason is required." ForeColor="Red" 
                    ValidationGroup="vgSubmit">*</asp:RequiredFieldValidator>
            </td>
        </tr>



        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td>
                <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfRosterType" runat="server" />
                <asp:HiddenField ID="hfInerchangerRosterType" runat="server" />
                <asp:HiddenField ID="hfCaller2" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfEmpID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfName" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfInterchanger" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfInterchangerName" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfLineNo" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfInterchangerMode" runat="server" ClientIDMode="Static" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                &nbsp;
            </td>
            <td align="left">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" 
                    OnClick="btnSave_Click" ValidationGroup="vgSubmit" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="125px" 
                    OnClick="btnCancel_Click" />
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>
        </table>



    <table class="styleMainTb">
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
    <span>List Roster Interchanges</span>
    <hr />
    <br />

    <div>
        <asp:GridView ID="gvRoster" runat="server" AutoGenerateColumns="False" Width="100%"
            AllowPaging="True" onpageindexchanging="gvRoster_PageIndexChanging" 
            onrowdatabound="gvRoster_RowDataBound" 
            onselectedindexchanged="gvRoster_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="ROSTR_ID" HeaderText="Roster ID" />
                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Emp ID" />
                <asp:BoundField DataField="DUTY_DATE" HeaderText="Duty Date" DataFormatString="{0:yyyy/MM/dd}" />

                <asp:BoundField DataField="FROM_TIME" HeaderText="From Time" />
                <asp:BoundField DataField="TO_TIME" HeaderText="To Time" />
                <asp:BoundField DataField="ROSTER_TYPE" HeaderText="Roster Type" />

                <asp:BoundField DataField="IS_OFF_DAY" HeaderText="Off Day?" />
                <asp:BoundField DataField="IS_SUMMARIZED" HeaderText="Summarized" Visible="False" />
                <asp:BoundField DataField="INTERCHANGE_NUMBER" HeaderText="Interchange Num" />
                <asp:BoundField DataField="DUTY_COVERED_BY" HeaderText="Duty Covered By" />
                <asp:BoundField DataField="REASON" HeaderText="Reason" />
                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" Visible="False" />
            </Columns>
        </asp:GridView>
    </div>


</asp:Content>
