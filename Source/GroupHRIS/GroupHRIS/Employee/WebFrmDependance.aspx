<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="WebFrmDependance.aspx.cs" Inherits="GroupHRIS.Employee.WebFrmDependance" %>

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
    <style type="text/css">
        input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
        
        .hideGridColumn
        {
            display: none;
        }
        .style1
        {
            height: 21px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <span style="font-weight: 700">Dependents Information</span>
            </td>
        </tr>
    </table>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <hr />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="margin: auto;">
                <tr>
                    <td>
                        Employee
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtemployee" runat="server" ClientIDMode="Static" ReadOnly="true"
                            Width="150px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                        <img alt="" src="../../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('/Employee/webFrmEmployeeSearch.aspx','Search','txtemployee')" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Employee is Required"
                            ForeColor="Red" Text="*" ControlToValidate="txtemployee" ValidationGroup="Main"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblInitialsName" runat="server" Style="font-weight: 700; color: #0000FF"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <b>Dependents</b>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        Full Name
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFullName" Width="185px" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Full Name is Required"
                            ForeColor="Red" Text="*" ControlToValidate="txtFullName" ValidationGroup="Main"></asp:RequiredFieldValidator>
                        <asp:ImageButton ID="ibtnApply" runat="server" Height="25px" ImageUrl="~/Images/Common/apply.jpg"
                            ToolTip="Generate Name With Initials" Width="25px" OnClick="ibtnApply_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Name with Initials
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtInitialsName" Width="185px" ReadOnly="true" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Name with Initials is Required"
                            ForeColor="Red" Text="*" ControlToValidate="txtInitialsName" ValidationGroup="Main"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Relationship to Employee
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRelationshipToEmployee" Width="185px" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlRelationshipToEmployee_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Relationship to Employee is Required"
                            ForeColor="Red" Text="*" ControlToValidate="ddlRelationshipToEmployee" ValidationGroup="Main"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Gender
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlGender" Width="185px" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Gender is Required"
                            ForeColor="Red" Text="*" ControlToValidate="ddlGender" ValidationGroup="Main"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Date of Birth
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDOB" placeholder='DD/MM/YYYY' ToolTip="Date Format [DD/MM/YYYY]"
                            Width="185px" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Date of Birth is Required"
                            ForeColor="Red" Text="*" ControlToValidate="txtDOB" ValidationGroup="Main"></asp:RequiredFieldValidator>
                        <asp:CalendarExtender ID="cefromdate" runat="server" TargetControlID="txtDOB" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="ftefromdate" runat="server" TargetControlID="txtDOB"
                            FilterType="Custom, Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                        <asp:RegularExpressionValidator ID="revfrmDate" runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                            ErrorMessage="(DD/MM/YYYY)" ControlToValidate="txtDOB" ForeColor="Red" ValidationGroup="Main"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        NIC
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtNIC" Width="185px" MaxLength="10" runat="server"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="fteNic" runat="server" TargetControlID="txtNIC"
                            FilterType="Custom, Numbers" ValidChars="0,1,2,3,4,5,6,7,8,9,V,X,x,v">
                        </asp:FilteredTextBoxExtender>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="\d{9}[xXvV]"
                            ControlToValidate="txtNIC" ValidationGroup="Main" runat="server" Text="*" ForeColor="Red"
                            ErrorMessage="Invalid NIC Format"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Occupation
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtOccupation" Width="185px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Place of Work
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPlaceOfWork" Width="185px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Mobile Number
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtMobileNumber" Width="185px" MaxLength="10" runat="server"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtMobileNumber"
                            FilterType="Numbers" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Land Phone Number
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLandPhoneNumber" Width="185px" MaxLength="10" runat="server"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtLandPhoneNumber"
                            FilterType="Numbers" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkEmergencyContact" Text="is Emegency Contact Person" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Width="75px" Text="Save" ValidationGroup="Main"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Width="75px" Text="Clear" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                    </td>
                    <td class="style1">
                    </td>
                    <td class="style1">
                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" ForeColor="Red" ValidationGroup="Main"
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
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:GridView ID="gvDependents" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                            OnRowDataBound="gvDependents_RowDataBound" OnSelectedIndexChanged="gvDependents_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="DEPENDANT_ID" HeaderText="DEPENDANT ID" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="NAME_WITH_INITIALS" HeaderText="DEPENDANT'S NAME" ControlStyle-Width="150px" />
                                <asp:BoundField DataField="RELATIONSHIP_TO_EMPLOYEE" HeaderText="RELATIONSHIP" />
                                <asp:BoundField DataField="GENDER" HeaderText="GENDER" />
                                <asp:BoundField DataField="DOB" HeaderText="DOB" />
                                <asp:BoundField DataField="NIC" HeaderText="NIC" />
                                <asp:BoundField DataField="CONTACT_NUMBER_MOBILE" HeaderText="MOBILE No." />
                                <asp:BoundField DataField="CONTACT_NUMBER_LAND" HeaderText="LAND PHONE No." />
                                <asp:BoundField DataField="IS_EMRGENCY_CONTACT" HeaderText="IS EMERGENCY CONTACT" />
                            </Columns>
                            <RowStyle Width="150px" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
                        <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
