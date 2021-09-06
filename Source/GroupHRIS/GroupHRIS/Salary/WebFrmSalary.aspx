<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmSalary.aspx.cs" Inherits="GroupHRIS.Salary.WebFrmSalary" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

//        function openLOVWindow(file, window, ctlName) {
//            childWindow = open(file, window, 'resizable=no,width=920,height=600,scrollbars=yes,top=50,left=200,status=yes');

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            <br />
            <span>Salary</span><span style="font-weight: 700;"> </span>
            <hr />
            <table style="margin: auto; margin-top: 50px;">
                <tr>
                    <td style="text-align: right;">
                        Salary ID :
                    </td>
                    <td>
                        <asp:Label ID="SalaryIDLabel" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Employee ID :
                    </td>
                    <td>
                        <asp:TextBox ID="EmployeeIDTextBox" ClientIDMode="Static" ReadOnly="true" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Employee ID is Required"
                            ValidationGroup="Main" Text="*" ForeColor="Red" ControlToValidate="EmployeeIDTextBox"></asp:RequiredFieldValidator>
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" title="Search"
                            onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','EmployeeIDTextBox')" />
                        <asp:ImageButton ID="HistoryImageButton" runat="server" Height="20px" Visible="false"
                            Width="20px" ImageUrl="~/Images/Common/history.png" ToolTip="View History" OnClick="HistoryImageButton_Click" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Employee Name :
                    </td>
                    <td>
                        <asp:Label ID="EmployeeNameLabel" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Basic Amount :
                    </td>
                    <td>
                        <asp:TextBox ID="BasicAmountTextBox" runat="server"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="BasicAmountFilteredTextBoxExtender" TargetControlID="BasicAmountTextBox"
                            FilterType="Custom, Numbers" ValidChars="." runat="server">
                        </asp:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Basic Amount is Required"
                            ValidationGroup="Main" Text="*" ForeColor="Red" ControlToValidate="BasicAmountTextBox"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Budgetary Relief Allowance :
                    </td>
                    <td>
                        <asp:TextBox ID="BudgetaryReliefAllowanceTextBox" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Salary Components :
                    </td>
                    <td>
                        <hr />
                        <table>
                            <tr>
                                <td style="text-align: right;">
                                    Component Name :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ComponentNameDropDownList" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Component Name is Required"
                                        ValidationGroup="SalaryComp" Text="*" ForeColor="Red" ControlToValidate="ComponentNameDropDownList"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Amount :
                                </td>
                                <td>
                                    <asp:TextBox ID="AmountTextBox" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="AmountTextBoxFilteredTextBoxExtender" TargetControlID="AmountTextBox"
                                        FilterType="Custom, Numbers" ValidChars="." runat="server">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Text="*"
                                        ValidationGroup="SalaryComp" ForeColor="Red" ControlToValidate="AmountTextBox"
                                        ErrorMessage="Amount is Required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Status :
                                </td>
                                <td>
                                    <asp:DropDownList ID="SalaryComponentStatusDropDownList" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="SalaryComp"
                                        ControlToValidate="SalaryComponentStatusDropDownList" Text="*" ForeColor="Red"
                                        runat="server" ErrorMessage="Status is Required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="AddSalaryComponentButton" ValidationGroup="SalaryComp" runat="server"
                                        Text="Add" OnClick="AddSalaryComponentButton_Click" />
                                    <asp:Button ID="SalaryComponentButton" runat="server" Text="Clear" OnClick="SalaryComponentButton_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:GridView ID="SalaryComponentsGridView" Style="width: 350px;" AutoGenerateColumns="false"
                                        runat="server" OnRowDataBound="SalaryComponentsGridView_RowDataBound" OnSelectedIndexChanging="SalaryComponentsGridView_SelectedIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="COMPONENT_NAME" HeaderText="Component Name " />
                                            <asp:BoundField DataField="AMOUNT" HeaderText="Amount " />
                                            <asp:BoundField DataField="STATUS_CODE" HeaderText="Status " />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Other Amount :
                    </td>
                    <td>
                        <asp:TextBox ID="OtherAmountTextBox" ReadOnly="true" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Total Amount :
                    </td>
                    <td>
                        <asp:TextBox ID="TotalAmountTextBox" ReadOnly="true" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        is OT Applicable :
                    </td>
                    <td>
                        <asp:DropDownList ID="OTDropDownList" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="OTRequiredFieldValidator" Text="*" ForeColor="Red"
                            runat="server" ErrorMessage="OT Status is Required" ValidationGroup="Main" ControlToValidate="OTDropDownList"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        With Effect from :
                    </td>
                    <td>
                        <asp:TextBox ID="EffectFromTextBox" placeholder="YYYY/MM/DD" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="EffectFromCalendarExtender" TargetControlID="EffectFromTextBox"
                            Format="yyyy/MM/dd" runat="server">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="EffectFromFilteredTextBoxExtender" runat="server"
                            TargetControlID="EffectFromTextBox" FilterType="Custom, Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="EffectDateRequiredFieldValidator" runat="server"
                            ControlToValidate="EffectFromTextBox" ErrorMessage="Effect From Date is Required"
                            ForeColor="Red" Text="*" ValidationGroup="Main"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Remarks :
                    </td>
                    <td>
                        <asp:TextBox ID="RemarksTextBox" MaxLength="200" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Status :
                    </td>
                    <td>
                        <asp:DropDownList ID="StatusDropDownList" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="SalarayStatusRequiredFieldValidator" Text="*" ForeColor="Red"
                            runat="server" ErrorMessage="Status is Required" ValidationGroup="Main" ControlToValidate="StatusDropDownList"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                        <br />
                        <asp:ValidationSummary ID="ValidationSummary1" ForeColor="Red" ValidationGroup="SalaryComp"
                            runat="server" />
                        <asp:ValidationSummary ID="ValidationSummary2" ForeColor="Red" ValidationGroup="Main"
                            runat="server" />
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="ToggleButton" runat="server" ValidationGroup="Main" Text="Save" OnClick="ToggleButton_Click" />
                        <asp:Button ID="CancelButton" runat="server" Text="Clear" OnClick="CancelButton_Click" />
                    </td>
                </tr>
            </table>
            <hr />
            <asp:GridView ID="SearchResultGridView" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
                OnRowDataBound="SearchResultGridView_RowDataBound" AllowPaging="true" OnSelectedIndexChanging="SearchResultGridView_SelectedIndexChanging"
                OnPageIndexChanging="SearchResultGridView_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="SALARY_ID" HeaderText="Salary ID " />
                    <asp:BoundField DataField="EMPLOYEE_ID" HeaderText="Employee ID " />
                    <asp:BoundField DataField="BASIC_AMOUNT" HeaderText="Basic Amount " />
                    <asp:BoundField DataField="BUDGETARY_ALLOWANCE_AMOUNT" HeaderText="Budgetary Allowance " />
                    <asp:BoundField DataField="OTHER_AMOUNT" HeaderText="Other Amount " />
                    <asp:BoundField DataField="IS_OT_APPLICABLE" HeaderText="is OT Applicable " />
                    <asp:BoundField DataField="EFFECT_FROM" HeaderText="With Effect from " />
                    <asp:BoundField DataField="REMARKS" HeaderText="Remarks " />
                    <asp:BoundField DataField="STATUS_CODE" HeaderText="Status " />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>