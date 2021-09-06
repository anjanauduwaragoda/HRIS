<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="webFrmModifyRosterAssignments.aspx.cs" Inherits="GroupHRIS.Roster.webFrmModifyRosterAssignments"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        // -------

        var TotalChkBx;
        var Counter;

        window.onload = function () {
            //Get total no. of CheckBoxes in side the GridView.
            TotalChkBx = parseInt('<%= this.gvRosterAssignments1.Rows.Count %>');

            //Get total no. of checked CheckBoxes in side the GridView.
            Counter = 0;
        }

        function HeaderClick(CheckBox) {
            //Get target base & child control.
            var TargetBaseControl =
       document.getElementById('<%= this.gvRosterAssignments1.ClientID %>');
            var TargetChildControl = "chkBxSelect";

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = CheckBox.checked;

        //Reset Counter
        Counter = CheckBox.checked ? TotalChkBx : 0;
    }

    function ChildClick(CheckBox, HCheckBox) {
        //get target control.
        var HeaderCheckBox = document.getElementById(HCheckBox);

        //Modifiy Counter; 
        if (CheckBox.checked && Counter < TotalChkBx)
            Counter++;
        else if (Counter > 0)
            Counter--;

        //Change state of the header CheckBox.
        if (Counter < TotalChkBx)
            HeaderCheckBox.checked = false;
        else if (Counter == TotalChkBx)
            HeaderCheckBox.checked = true;
    }
    //------


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

        //DoPostBack();
    }

    function writeToHiddenFields(sEmpId, sName, sCompanyCode, sCompanyName, sDepartmentId, sDepartmentName, sDivisionId, sDivisionName) {
        document.getElementById("hfEmpID").value = sEmpId;
        document.getElementById("hfName").value = sName;
    }

    function DoPostBack() {
        __doPostBack("txtEmployeeID", "TextChanged");
    }


    </script>
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <span>View / Obsolete Roster Assignments</span>
    <hr />
    <br />
    <table class="styleMainTb">
        <tr>
            <td width="30%" align="right">
                &nbsp;<asp:Label ID="Label1" runat="server" Text="Employee" AssociatedControlID="txtEmployeeID"></asp:Label>
            </td>
            <td width="40%" align="left">
                <asp:TextBox ID="txtEmployeeID" runat="server" Width="250px" ClientIDMode="Static"
                    ViewStateMode="Enabled" ReadOnly="True" AutoPostBack="True"></asp:TextBox>
                &nbsp;
                <img alt="Search for an Employee" src="../Images/Common/Search.jpg" height="20px"
                    width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmployeeID')"
                    id="imgSearch" />
            </td>
            <td width="30%" align="left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmployeeID"
                    ErrorMessage="Employee ID is required." ForeColor="Red" ValidationGroup="vgSubmit">*</asp:RequiredFieldValidator>
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
                <asp:Label ID="Label4" runat="server" Text="From Date" AssociatedControlID="txtFromDate"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                <asp:CalendarExtender ID="ceDate" runat="server" TargetControlID="txtFromDate" Format="yyyy/MM/dd">
                </asp:CalendarExtender>
                <asp:FilteredTextBoxExtender ID="ftDate" runat="server" TargetControlID="txtFromDate"
                    FilterType="Custom, Numbers" ValidChars="/">
                </asp:FilteredTextBoxExtender>
                &nbsp;&nbsp;
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFromDate"
                    ErrorMessage="From Date is required." ForeColor="Red" ValidationGroup="vgSubmit">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label3" runat="server" Text="To Date" AssociatedControlID="txtToDate"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                <asp:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate" Format="yyyy/MM/dd">
                </asp:CalendarExtender>
                <asp:FilteredTextBoxExtender ID="ftToDate" runat="server" TargetControlID="txtToDate"
                    FilterType="Custom, Numbers" ValidChars="/">
                </asp:FilteredTextBoxExtender>
                &nbsp;&nbsp;
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtToDate"
                    ErrorMessage="To Date is required." ForeColor="Red" ValidationGroup="vgSubmit">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:HiddenField ID="hfEmpID" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfName" runat="server" ClientIDMode="Static" />
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
                <asp:Button ID="btnSearch" runat="server" Text="Search" Width="125px" OnClick="btnSearch_Click"
                    ValidationGroup="vgSubmit" />
                <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="125px" OnClick="btnCancel_Click" />
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
                <asp:ValidationSummary ID="vsSubmit" runat="server" ValidationGroup="vgSubmit" ForeColor="Red" />
            </td>
        </tr>
    </table>
    <br />
    <span>List of Roster Assignments</span>
  
    <hr />
    <br />
    <div>
    <table style="margin:auto;">
        <tr>
            <td>
        <asp:GridView ID="gvRosterAssignments1" runat="server" AutoGenerateColumns="False"
            Width="100%" PageSize="20" OnPageIndexChanging="gvRosterAssignments_PageIndexChanging"
            OnRowDataBound="gvRosterAssignments_RowDataBound" OnSelectedIndexChanged="gvRosterAssignments_SelectedIndexChanged"
            AllowPaging="True">
            <Columns>
                <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Emp ID" />
                <asp:BoundField DataField="ROSTR_ID" HeaderText="Roster ID" />
                <asp:BoundField DataField="DUTY_DATE" HeaderText="Duty Date" DataFormatString="{0:yyyy/MM/dd}" />
                <asp:BoundField DataField="FROM_TIME" HeaderText="From Time" />
                <asp:BoundField DataField="TO_TIME" HeaderText="To Time" />
                <asp:BoundField DataField="description" HeaderText="Roster Type" />
                <asp:BoundField DataField="IS_OFF_DAY" HeaderText="Off Day?" HeaderStyle-CssClass="hideGridColumn"
                    ItemStyle-CssClass="hideGridColumn">
                    <HeaderStyle CssClass="hideGridColumn"></HeaderStyle>
                    <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="IS_SUMMARIZED" HeaderText="Summarized" Visible="False" />
                <asp:BoundField DataField="INTERCHANGE_NUMBER" HeaderText="Interchange Num" />
                <asp:BoundField DataField="DUTY_COVERED_BY" HeaderText="Covered By ID" />
                <asp:BoundField DataField="FIRST_NAME" HeaderText="Covered By Name" />
                <asp:BoundField DataField="REASON" HeaderText="Reason" />
                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label ID="lblObsoleate" runat="server" Text="Obsolete"></asp:Label><br />
                        <%--<asp:Label ID="lblchk" runat="server" Text="Check All" ></asp:Label>--%>
                        <asp:CheckBox ID="chkBxHeader" onclick="javascript:HeaderClick(this);" runat="server"
                            CommandName="Obsolete" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkBxSelect" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </td>
        </tr>
        <tr>
        <td></td>
        </tr>
        <tr>
            <td style="text-align:right;">
            
            <asp:Button ID="btnObsolete" runat="server" Text="Obsolete" 
                    OnClick="btnObsolete_Click" style="height: 26px" />
            </td>
        </tr>
    </table>
            
    </div>
</asp:Content>
