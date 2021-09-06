<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="webFrmEmployeeTransfer.aspx.cs" Inherits="GroupHRIS.Employee.webFrmEmployeeTransfer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">
        var txb;
        function openLOVWindowrpt(file, window, ctlName) {
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
        .style2
        {
            width: 176px;
        }
    </style>
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
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <span>Employee Transfer</span>
    <hr />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="margin: auto;">
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label1" runat="server" Text="Employee Id" AssociatedControlID="txtEmployeeID"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmployeeID" runat="server" Width="250px" ClientIDMode="Static"
                            ViewStateMode="Enabled" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                        <img alt="Search for an Employee" src="../Images/Common/Search.jpg" height="20px"
                            width="20px" onclick="openLOVWindowrpt('webFrmEmployeeSearch.aspx','Search','txtEmployeeID')" />
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmployeeID"
                            ErrorMessage="Employee Id  is required." ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label2" runat="server" Text="Name " AssociatedControlID="txtName"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" Width="250px" ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        From First Level Supervisor
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromFirstLevelSupervisor" runat="server" Width="65px" ClientIDMode="Static"
                            ViewStateMode="Enabled" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtFromFirstLevelSupervisorName" runat="server" Width="181px" ClientIDMode="Static"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        From Second Level Supervisor
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromSecondLevelSupervisor" runat="server" Width="65px" ClientIDMode="Static"
                            ViewStateMode="Enabled" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtFromSecondLevelSupervisorName" runat="server" Width="181px" ClientIDMode="Static"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        From Third Level Supervisor
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromThirdLevelSupervisor" runat="server" Width="65px" ClientIDMode="Static"
                            ViewStateMode="Enabled" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtFromThirdLevelSupervisorName" runat="server" Width="181px" ClientIDMode="Static"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label3" runat="server" Text="From Company" AssociatedControlID="txtFromCompanyName"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromCompanyCode" runat="server" Width="65px" ClientIDMode="Static"
                            ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtFromCompanyName" runat="server" Width="181px" ClientIDMode="Static"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label4" runat="server" Text="From Department" AssociatedControlID="txtFromDepartmentName"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDepartmentID" runat="server" Width="65px" ClientIDMode="Static"
                            ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtFromDepartmentName" runat="server" Width="181px" ClientIDMode="Static"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label5" runat="server" Text="From Division" AssociatedControlID="txtFromDivisionName"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDivisionID" runat="server" Width="65px" ClientIDMode="Static"
                            ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtFromDivisionName" runat="server" Width="181px" ClientIDMode="Static"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label11" runat="server" Text="From Branch" AssociatedControlID="txtFromDivisionName"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromBranchID" runat="server" Width="65px" ClientIDMode="Static"
                            ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtFromBranchName" runat="server" Width="181px" ClientIDMode="Static"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label17" runat="server" Text="From EPF " AssociatedControlID="txtFromCC"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtfrmEPF" runat="server" Width="250px" ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label21" runat="server" Text="From ETF " AssociatedControlID="txtfrmETF"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtfrmETF" runat="server" Width="250px" ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label18" runat="server" Text="From Designation " AssociatedControlID="txtFromCC"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtfrmDesignation" runat="server" Width="65px" ClientIDMode="Static"
                            ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtDesignationName" runat="server" Width="181px" ClientIDMode="Static"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label13" runat="server" Text="From Cost Center" AssociatedControlID="txtFromCC"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromCC" runat="server" Width="65px" ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtfccName" runat="server" Width="181px" ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label14" runat="server" Text="From Profit Center" AssociatedControlID="txtFromPC"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromPC" runat="server" Width="65px" ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtfpcName" runat="server" Width="181px" ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label6" runat="server" Text="To Company" AssociatedControlID="ddlToCompany"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlToCompany" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlToCompany_SelectedIndexChanged"
                            Width="250px">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlToCompany"
                            ErrorMessage="To Company  is required." ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label7" runat="server" Text="To Department" AssociatedControlID="ddlToDepartment"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlToDepartment" runat="server" Width="250px" TabIndex="1"
                            OnSelectedIndexChanged="ddlToDepartment_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlToDepartment"
                            ErrorMessage="To Department  is required." ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label8" runat="server" Text="To Division" AssociatedControlID="ddlToDivision"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlToDivision" runat="server" Width="250px" TabIndex="2" OnSelectedIndexChanged="ddlToDivision_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlToDivision"
                            ErrorMessage="To Division  is required." ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label12" runat="server" Text="To Branch" AssociatedControlID="ddlToBranch"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlToBranch" runat="server" Width="250px" TabIndex="2">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlToBranch"
                            ErrorMessage="To Branch  is required." ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label51" runat="server" Text="Next EPF No"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:Label ID="lblNextEpfNo" Style="text-align: right;" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="ibtnGet" runat="server" Height="20px" ImageUrl="~/Images/Common/apply.jpg"
                            OnClick="ibtnGet_Click" Width="20px" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label19" runat="server" Text="To EPF " AssociatedControlID="txtFromCC"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtToEPF" runat="server" Width="250px" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtToEPF"
                            ErrorMessage="To EPF is required." ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label20" runat="server" Text="To Designation " AssociatedControlID="txtFromCC"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlToDesignation" runat="server" Width="250px">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlToDesignation"
                            ErrorMessage="To Designation is required." ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label16" runat="server" Text="To Cost Center" AssociatedControlID="ddlToCC"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlToCC" runat="server" Width="250px">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlToCC"
                            ErrorMessage="To Cost Center  is required." ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label15" runat="server" Text="To Profit Center" AssociatedControlID="ddlToPC"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlToPC" runat="server" Width="250px">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlToPC"
                            ErrorMessage="To Profit Center  is required." ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        To First Level Supervisor
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtToFirstLevelSupervisor" runat="server" Width="65px" ClientIDMode="Static"
                            ViewStateMode="Enabled" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="lbltRpt1" runat="server" ClientIDMode="Static" ReadOnly="True" Width="181px"></asp:TextBox>
                        <%--<asp:Label ID="lbltRpt1" runat="server"></asp:Label>--%>
                    </td>
                    <td>
                        <img alt="Search for an Employee" src="../Images/Common/Search.jpg" height="20px"
                            width="20px" onclick="openLOVWindowrpt('webFrmEmployeeSearch.aspx','Search','txtToFirstLevelSupervisor')" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        To Second Level Supervisor
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtToSecondLevelSupervisor" runat="server" Width="65px" ClientIDMode="Static"
                            ViewStateMode="Enabled" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="lbltRpt2" runat="server" ClientIDMode="Static" ReadOnly="True" Width="181px"></asp:TextBox>
                        <%--<asp:Label ID="lbltRpt2" runat="server"></asp:Label>--%>
                    </td>
                    <td>
                        <img alt="Search for an Employee" src="../Images/Common/Search.jpg" height="20px"
                            width="20px" onclick="openLOVWindowrpt('webFrmEmployeeSearch.aspx','Search','txtToSecondLevelSupervisor')" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        To Third Level Supervisor
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtToThirdLevelSupervisor" runat="server" Width="65px" ClientIDMode="Static"
                            ViewStateMode="Enabled" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="lbltRpt3" runat="server" ClientIDMode="Static" ReadOnly="True" Width="181px"></asp:TextBox>
                        <%--<asp:Label ID="lbltRpt3" runat="server"></asp:Label>--%>
                    </td>
                    <td>
                        <img alt="Search for an Employee" src="../Images/Common/Search.jpg" height="20px"
                            width="20px" onclick="openLOVWindowrpt('webFrmEmployeeSearch.aspx','Search','txtToThirdLevelSupervisor')" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label9" runat="server" Text="Start Date" AssociatedControlID="txtStartDate"></asp:Label>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtStartDate" runat="server" MaxLength="10" placeholder="DD/MM/YYYY"
                            Width="250px"></asp:TextBox>
                        <asp:CalendarExtender ID="ceStartDate" runat="server" TargetControlID="txtStartDate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="fteDob" runat="server" TargetControlID="txtStartDate"
                            FilterType="Custom, Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="txtStartDate" Text="*" ValidationGroup="vgSubmit" ForeColor="Red" runat="server" ErrorMessage="Start Date is Required"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtStartDate"
                            runat="server" ValidationExpression="^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"
                            ErrorMessage="Incorrect Date Format (Start Date)" Text="*" ForeColor="Red"
                            ValidationGroup="vgSubmit"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; vertical-align: text-top;">
                        <asp:Label ID="Label10" runat="server" Text="Remarks"></asp:Label>
                    </td>
                    <td style="vertical-align: text-top;">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="45" Rows="2" TextMode="MultiLine"
                            Width="250px"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
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
                    <td style="text-align:right;">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" OnClick="btnSave_Click"
                            ValidationGroup="vgSubmit" />
                        <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="125px" OnClick="btnCancel_Click" />
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <table class="styleMainTb">
                <tr>
                    <td align="center">
                        <%-- <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>--%>
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfEmpID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfName" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfCompanyCode" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfCompanyName" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfDepartmentID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfDepartmentName" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfDivisionID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfDivisionName" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfBranchID" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfBranchName" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfCC" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfPC" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfEPF" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfDesignation" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfDesigName" runat="server" ClientIDMode="Static" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:ValidationSummary ID="vsSubmit" runat="server" ValidationGroup="vgSubmit" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblMsg" runat="server" Font-Size="10pt" ForeColor="Blue"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <span>List of Previous Employment</span>
            <hr />
            <br />
            <div>
                <asp:GridView ID="gvEmpTrans" runat="server" AutoGenerateColumns="False" Width="100%"
                    Style="width: 850px;" PageSize="15" AllowPaging="True" OnRowDataBound="gvEmpTrans_RowDataBound"
                    OnSelectedIndexChanged="gvEmpTrans_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="TRANS_ID" HeaderText="TRANS_ID" HeaderStyle-CssClass="hideGridColumn"
                            ItemStyle-CssClass="hideGridColumn" />
                        <asp:BoundField DataField="FROM_COMPANY" HeaderText="From Comp." />
                        <asp:BoundField DataField="FROM_DEPARTMENT" HeaderText="From Dept." />
                        <asp:BoundField DataField="FROM_DIVISION" HeaderText="From Div." />
                        <asp:BoundField DataField="FROM_EPF" HeaderText="From EPF" />
                        <asp:BoundField DataField="FROM_ETF" HeaderText="From ETF" />
                        <asp:BoundField DataField="TO_COMPANY" HeaderText="To Comp." />
                        <asp:BoundField DataField="TO_DEPARTMENT" HeaderText="To Dept." />
                        <asp:BoundField DataField="TO_DIVISION" HeaderText="To Div." />
                        <asp:BoundField DataField="TO_EPF" HeaderText="To EPF" />
                        <asp:BoundField DataField="TO_ETF" HeaderText="To ETF" />
                        <asp:BoundField DataField="TRANSFER_DATE" HeaderText="Transfer Date" />
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
