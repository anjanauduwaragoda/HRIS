<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="webFrmEmployee.aspx.cs" Inherits="GroupHRIS.Employee.webFrmEmployee"
    Trace="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--<%@ OutputCache Duration="30" VaryByParam="ddlCompany;ddlDepartment;ddlDivision;ddlEmployeeType;ddlEmployeeRole" Location ="Client"  %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <script type="text/JavaScript">
        function nicCheck() {
            //                var o = <%= txtNic.ClientID %>
            //		        alert(o.toString());
            var f9 = document.getElementById('<%= txtNic.ClientID %>').value;
            if (f9.length <= 9) {
                f9 = parseInt(f9);
            }
            document.getElementById('<%= txtNic.ClientID %>').value = f9;
            if (document.getElementById('<%= txtNic.ClientID %>').value == 'NaN') {
                document.getElementById('<%= txtNic.ClientID %>').value = "";
            }
            if (f9.length >= 10) {
                f9 = parseInt(f9);
                f9 = f9.toString();
                var temp = '';
                for (var i = 0; i < 9; i++) {
                    temp = temp + f9[i];
                }
                document.getElementById('<%= txtNic.ClientID %>').value = temp;
                f9 = temp;
            }
            if (f9.length == 9) {
                document.getElementById('<%= txtNic.ClientID %>').value = f9 + 'V';
            }        
        }

        function ValidateNIC() {
            var nicNumber = document.getElementById('<%= txtNic.ClientID %>').value;
            var nicLength = nicNumber.length;
            if (nicLength != '0') {
                //document.getElementById('lblMessage').innerHTML = nicNumber + '	(' + nicLength + ')';
            }
            else {
                //document.getElementById('lblMessage').innerHTML = '';
            }

            if (nicLength < 10) {
                var LastChar = nicNumber.slice(-1);
                if ((LastChar != '0') && (LastChar != '1') && (LastChar != '2') && (LastChar != '3') && (LastChar != '4') && (LastChar != '5') && (LastChar != '6') && (LastChar != '7') && (LastChar != '8') && (LastChar != '9')) {
                    nicNumber = nicNumber.slice(0, -1);
                    nicLength = nicNumber.length;

                    document.getElementById('<%= txtNic.ClientID %>').value = nicNumber;
                    if (nicLength != '0') {
                        //document.getElementById('lblMessage').innerHTML = nicNumber + '	(' + nicLength + ')';
                    }
                    else {
                        //document.getElementById('lblMessage').innerHTML = '';
                    }
                }
            }

            if (nicLength == '10') {
                document.getElementById('<%= txtNic.ClientID %>').setAttribute('maxlength', 12);
                var LastChar = nicNumber.slice(-1);
                if ((LastChar != 'x') && (LastChar != 'X') && (LastChar != 'v') && (LastChar != 'V')) {
                    if ((LastChar != '0') && (LastChar != '1') && (LastChar != '2') && (LastChar != '3') && (LastChar != '4') && (LastChar != '5') && (LastChar != '6') && (LastChar != '7') && (LastChar != '8') && (LastChar != '9')) {
                        nicNumber = nicNumber.slice(0, -1);
                        nicLength = nicNumber.length;
                        document.getElementById('<%= txtNic.ClientID %>').value = nicNumber;
                        //document.getElementById('lblMessage').innerHTML = nicNumber + '	(' + nicLength + ')';
                        document.getElementById('<%= txtNic.ClientID %>').setAttribute('maxlength', 12);
                    }
                }
                else {
                    document.getElementById('<%= txtNic.ClientID %>').setAttribute('maxlength', 12);
                    LastChar = nicNumber.slice(-1);
                    if ((LastChar != '0') && (LastChar != '1') && (LastChar != '2') && (LastChar != '3') && (LastChar != '4') && (LastChar != '5') && (LastChar != '6') && (LastChar != '7') && (LastChar != '8') && (LastChar != '9')) {
                        document.getElementById('<%= txtNic.ClientID %>').setAttribute('maxlength', 10);
                    }
                    else {
                        document.getElementById('<%= txtNic.ClientID %>').setAttribute('maxlength', 12);
                    }
                }
            }

            if (nicLength > 10) {
                var LastChar = nicNumber.slice(-1);
                if ((LastChar != '0') && (LastChar != '1') && (LastChar != '2') && (LastChar != '3') && (LastChar != '4') && (LastChar != '5') && (LastChar != '6') && (LastChar != '7') && (LastChar != '8') && (LastChar != '9')) {
                    nicNumber = nicNumber.slice(0, -1);
                    nicLength = nicNumber.length;

                    document.getElementById('<%= txtNic.ClientID %>').value = nicNumber;
                    if (nicLength != '0') {
                        //document.getElementById('lblMessage').innerHTML = nicNumber + '	(' + nicLength + ')';
                    }
                    else {
                        //document.getElementById('lblMessage').innerHTML = '';
                    }

                }
            }
        }

    </script>
    <script language="javascript" type="text/javascript">

//        function openLOVWindow(file, window, ctlName) {
//            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
//            //if (childWindow.opener == null) childWindow.opener = self;

//            document.getElementById("hfCaller").value = ctlName;
//        }

//        function getValueFromChild(sRetVal) {
//            var ctl = document.getElementById("hfCaller").value;
//            document.getElementById(ctl).value = sRetVal;
//            document.getElementById(hfEmpID).value = sRetVal;
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
    <style type="text/css">
        #txtReportTo1
        {
            width: 129px;
        }
        #txtReportTo4
        {
            width: 129px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="tsmCalander" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <span>Employee Personal Information</span>
    <hr />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="Company :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:DropDownList ID="ddlCompany" runat="server" Width="256px" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ErrorMessage="Company is required"
                            ControlToValidate="ddlCompany" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label2" runat="server" Text="Department :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:DropDownList ID="ddlDepartment" runat="server" Width="256px" TabIndex="1" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ErrorMessage="Department is required"
                            ControlToValidate="ddlDepartment" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label3" runat="server" Text="Division :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:DropDownList ID="ddlDivision" runat="server" Width="256px" TabIndex="2">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvDivision" runat="server" ErrorMessage="Division is required"
                            ControlToValidate="ddlDivision" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label4" runat="server" Text="Status :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="256px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ErrorMessage="Status is required"
                            ControlToValidate="ddlStatus" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label5" runat="server" Text="Employee Type :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:DropDownList ID="ddlEmployeeType" runat="server" Width="256px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvEmployeeType" runat="server" ErrorMessage="Employee type is required"
                            ControlToValidate="ddlEmployeeType" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label6" runat="server" Text="Employee Role :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:DropDownList ID="ddlEmployeeRole" runat="server" Width="256px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvEmployeeRole" runat="server" ErrorMessage="Employee role is required"
                            ControlToValidate="ddlEmployeeRole" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label44" runat="server" Text="Designation :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:DropDownList ID="ddlDesignation" runat="server" Width="256px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvDesignation" runat="server" ErrorMessage="Designation is required"
                            ControlToValidate="ddlDesignation" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label7" runat="server" Text="Title :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:DropDownList ID="ddlTitle" runat="server" Width="256px">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="Mr.">Mr</asp:ListItem>
                            <asp:ListItem Value="Mrs.">Mrs</asp:ListItem>
                            <asp:ListItem Value="Ms.">Ms</asp:ListItem>
                            <asp:ListItem Value="Miss.">Miss</asp:ListItem>
                            <asp:ListItem Value="Mx.">Mx</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ErrorMessage="Title is required"
                            ControlToValidate="ddlEmployeeRole" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label12" runat="server" Text="Full Name :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtFullName" runat="server" MaxLength="200" Width="250px"  onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                        <asp:ImageButton ID="ibtnApply" runat="server" Height="25px" ImageUrl="~/Images/Common/apply.jpg"
                            OnClick="ibtnApply_Click" Width="25px" />
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ErrorMessage="Full name is required"
                            ControlToValidate="txtFullName" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label45" runat="server" Text="Name with Initials :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtNameInitials" runat="server" MaxLength="45" Width="250px"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvNameInitials" runat="server" ErrorMessage="Name with Initials is required"
                            ControlToValidate="txtNameInitials" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label46" runat="server" Text="Known Name :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtKnownName" runat="server" MaxLength="45" Width="250px"  onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvKnownName" runat="server" ErrorMessage="Known Name is required"
                            ControlToValidate="txtKnownName" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label8" runat="server" Text="Initials :" ForeColor="Red" 
                            Visible="False"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtInitials" runat="server" MaxLength="45" Width="250px" 
                            Visible="False"  onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label9" runat="server" Text="First Name :" ForeColor="Red" Visible="False"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="45" Width="250px" Visible="False"  onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label10" runat="server" Text="Middle Name :" ForeColor="Red" Visible="False"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtMiddleName" runat="server" MaxLength="50" Width="250px" Visible="False"  onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label11" runat="server" Text="Last Name :" ForeColor="Red" Visible="False"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtLastName" runat="server" MaxLength="45" Width="250px" Visible="False"   onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label13" runat="server" Text="Gender :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:DropDownList ID="ddlGender" runat="server" Width="256px">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="M">Male</asp:ListItem>
                            <asp:ListItem Value="F">Female</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvGender" runat="server" ErrorMessage="Gender is required"
                            ControlToValidate="ddlGender" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label14" runat="server" Text="NIC :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <%--<asp:TextBox ID="TextBox1" runat="server" MaxLength="10" Width="250px" onkeyup="nicCheck()"/></asp:TextBox>--%>
                        <asp:TextBox ID="txtNic" runat="server" MaxLength="10" onkeyup="ValidateNIC()" Width="250px" />
                        <asp:FilteredTextBoxExtender ID="fteNic" runat="server" TargetControlID="txtNic"
                            FilterType="Custom, Numbers" ValidChars="0,1,2,3,4,5,6,7,8,9,V,X,x,v">
                        </asp:FilteredTextBoxExtender>
                        <%----%>
                    </td>
                    <td style="width: 20px">
                        
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvNic" runat="server" ErrorMessage="NIC is required"
                            ControlToValidate="txtNic" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="\d{9}[xXvV]" ControlToValidate="txtNic" ValidationGroup="vgEmployeeInformation" runat="server" Text="*" ForeColor="Red" ErrorMessage="Invalid NIC Format"></asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="revNicLength" runat="server" ValidationGroup="vgEmployeeInformation"
                            ControlToValidate="txtNic" ValidationExpression="^([\S\s]{10,10})$" ErrorMessage="Ten charactors required for the NIC"
                            ForeColor="Red">*</asp:RegularExpressionValidator>--%>
                    </td>
                    <td>
                        
                        <asp:CheckBox ID="chkNICDuplication" Text = "Allow NIC Duplication" runat="server" />
                        
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label15" runat="server" Text="Passport No :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtPassportNo" runat="server" Width="250px"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label16" runat="server" Text="Marital Status :"></asp:Label>
                    </td> 
                    <td style="width: 250px; text-align: right">
                        <asp:DropDownList ID="ddlMaritalStatus" runat="server" Width="256px" TabIndex="1">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Married</asp:ListItem>
                            <asp:ListItem>Unmarried</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvDOB" runat="server" ErrorMessage="Marital status is required"
                            ControlToValidate="ddlMaritalStatus" ValidationGroup="vgEmployeeInformation"
                            ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label17" runat="server" Text="Nationality :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtNationality" runat="server" MaxLength="45" Width="250px"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label18" runat="server" Text="Religion :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtReligon" runat="server" MaxLength="45" Width="250px"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label19" runat="server" Text="E-Mail :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Invalid e-mail address"
                            ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label51" runat="server" Text="Next EPF No :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:Label ID="lblNextEpfNo" runat="server" Width="250px"></asp:Label>                        
                    </td>
                    <td style="width: 20px">
                        <asp:ImageButton ID="ibtnGet" runat="server" Height="25px" ImageUrl="~/Images/Common/apply.jpg"
                             Width="25px" onclick="ibtnGet_Click" />
                    </td>
                    <td style="width: 20px">                        
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label20" runat="server" Text="EPF No :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtEpfNo" runat="server" MaxLength="5" Width="250px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="fteEpfNo" TargetControlID="txtEpfNo" runat="server"
                            FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvEpfNo" runat="server" ErrorMessage="EPF No is required"
                            ControlToValidate="txtEpfNo" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label21" runat="server" Text="ETF No :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtEtfNo" runat="server" MaxLength="5" Width="250px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="fteEtfNo" TargetControlID="txtEtfNo" runat="server"
                            FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvEtfNo" runat="server" ErrorMessage="ETF No is required"
                            ControlToValidate="txtEtfNo" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: left">
                    </td>
                    <td style="width: 250px; text-align: left">
                        <span>(YYYY/MM/DD)</span>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label34" runat="server" Text="Date of Birth :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtDob" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                        <asp:CalendarExtender ID="ceDob" runat="server" TargetControlID="txtDob" Format="yyyy/MM/dd">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="fteDob" runat="server" TargetControlID="txtDob"
                            FilterType="Custom, Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Date  of birth is required"
                            ControlToValidate="txtDob" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: left">
                    </td>
                    <td style="width: 250px; text-align: left">
                        <span>(YYYY/MM/DD)</span>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label35" runat="server" Text="Date of join :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtDoj" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                        <asp:CalendarExtender ID="ceDoj" runat="server" TargetControlID="txtDoj" Format="yyyy/MM/dd">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="fteDoj" runat="server" TargetControlID="txtDoj"
                            FilterType="Custom, Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvDoj" runat="server" ErrorMessage="Date  of join is required"
                            ControlToValidate="txtDoj" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: left">
                    </td>
                    <td style="width: 250px; text-align: left">
                        <span>(YYYY/MM/DD)</span>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label48" runat="server" Text="Probation/Contract End Date :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtProbConEndDate" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                        <asp:CalendarExtender ID="ceProbConEndDate" runat="server" TargetControlID="txtProbConEndDate"
                            Format="yyyy/MM/dd">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="ftbeProbConEndDate" runat="server" TargetControlID="txtProbConEndDate"
                            FilterType="Custom, Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right;">
                        <asp:Label ID="Label22" runat="server" Text="Permanent Address :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtPermanentAddress" runat="server" MaxLength="100" Width="250px"
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right;">
                        <asp:Label ID="Label23" runat="server" Text="Current Address :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtCurrentAddress" runat="server" MaxLength="100" Width="250px"
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right;">
                        <asp:Label ID="Label24" runat="server" Text="Land Phone :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtLandPhone" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="fteLandPhone" runat="server" TargetControlID="txtLandPhone"
                            FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RegularExpressionValidator ID="revLandPhone" runat="server" ValidationGroup="vgEmployeeInformation"
                            ControlToValidate="txtLandPhone" ValidationExpression="^([\S\s]{10,10})$" ErrorMessage="Ten numbers are required for the Land Phone"
                            ForeColor="Red">*</asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right;">
                        <asp:Label ID="Label25" runat="server" Text="Mobile Phone (Personal) :"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtMobilePersonal" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="fteMobilePersonal" runat="server" TargetControlID="txtMobilePersonal"
                            FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RegularExpressionValidator ID="revMobPersonal" runat="server" ValidationGroup="vgEmployeeInformation"
                            ControlToValidate="txtMobilePersonal" ValidationExpression="^([\S\s]{10,10})$"
                            ErrorMessage="Ten numbers are required for the Mobile Phone (Personal)" ForeColor="Red">*</asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right;">
                        <asp:Label ID="Label26" runat="server" Text="Mobile Phone (Office)"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtMobileOffice" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="fteMobileOffice" runat="server" TargetControlID="txtMobileOffice"
                            FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RegularExpressionValidator ID="revMobOffice" runat="server" ValidationGroup="vgEmployeeInformation"
                            ControlToValidate="txtMobileOffice" ValidationExpression="^([\S\s]{10,10})$"
                            ErrorMessage="Ten numbers are required for the Mobile Phone (Office)" ForeColor="Red">*</asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right;">
                        <asp:Label ID="Label27" runat="server" Text="Fuel Card No"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtFuelCardNo" runat="server" MaxLength="12" Width="250px"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right;">
                        <asp:Label ID="Label28" runat="server" Text="First Level Supervisor"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtReportTo1" runat="server" ReadOnly="true" ClientIDMode="Static" 
                            Style="width: 250px" MaxLength="8"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="ftbeReportTo1" runat="server" TargetControlID="txtReportTo1"
                            FilterType="Custom, Numbers" ValidChars="E,e,P,p">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td style="width: 20px">
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('webFrmEmployeeSearch.aspx','Search','txtReportTo1')" />
                    </td>
                    <td style="width: 20px">
                       <%-- <asp:RequiredFieldValidator ID="rfvReportTo1" runat="server" ErrorMessage="First Level Supervisor is required"
                            ControlToValidate="txtReportTo1" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right;">
                        <asp:Label ID="Label29" runat="server" Text="Second Level Supervisor"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtReportTo2" ReadOnly="true" runat="server" ClientIDMode="Static" 
                            Style="width: 250px" MaxLength="8"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('webFrmEmployeeSearch.aspx','Search','txtReportTo2')" />
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right;">
                        <asp:Label ID="Label30" runat="server" Text="Third Level Supervisor"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtReportTo3" ReadOnly="true" runat="server" ClientIDMode="Static" 
                            Style="width: 250px" MaxLength="8"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('webFrmEmployeeSearch.aspx','Search','txtReportTo3')" />
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right;">
                        <asp:Label ID="Label31" runat="server" Text="City"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtCity" runat="server" MaxLength="45" Width="250px"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right;">
                        <asp:Label ID="Label32" runat="server" Text="Distance to Office (Km)"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtDistance" runat="server" MaxLength="6" Width="250px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="fteDistance" TargetControlID="txtDistance" FilterType="Custom, Numbers"
                            ValidChars="." runat="server">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right;">
                        <asp:Label ID="Label36" runat="server" Text="Cost Center"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtCostCenter" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                        <%--<asp:DropDownList ID="ddlToCC" runat="server" Width="256px">
                </asp:DropDownList>--%>

                        <%--<asp:FilteredTextBoxExtender ID="fteCostCenter" TargetControlID="txtCostCenter" FilterType="Numbers"
                            runat="server">--%>
                       <%-- </asp:FilteredTextBoxExtender>--%>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right;">
                        <asp:Label ID="Label38" runat="server" Text="Profit Center"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtProfitCenter" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="fteProfitCenter" TargetControlID="txtProfitCenter"
                            FilterType="Numbers" runat="server"></asp:FilteredTextBoxExtender>
                            <%--<asp:DropDownList ID="ddlToPC"  runat="server" Width="256px">
                </asp:DropDownList>--%>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right;">
                        <asp:Label ID="Label33" runat="server" Text="Remarks"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="500" Width="250px" TextMode="MultiLine"
                            Height="150px"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label39" runat="server" Text="Date of Resign"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:TextBox ID="txtResignedDate" runat="server" MaxLength="10" Width="250px"></asp:TextBox>
                        <asp:CalendarExtender ID="ceResignedDate" runat="server" TargetControlID="txtResignedDate"
                            Format="yyyy/MM/dd">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="fteResignedDate" runat="server" TargetControlID="txtResignedDate"
                            FilterType="Custom, Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label40" runat="server" Text="Include for Welfare"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: left">
                        <asp:CheckBox ID="chkWelfare" runat="server" />
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label41" runat="server" Text="Reg No in Attendance Machine" Visible="False"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: left">
                        <asp:TextBox ID="txtAttRegNo" runat="server" MaxLength="6" Visible="False"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="fteAttRegNo" TargetControlID="txtAttRegNo" FilterType="Numbers"
                            runat="server">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label42" runat="server" Text="Branch"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: left">
                        <asp:DropDownList ID="ddlBranch" runat="server" Width="250px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Branch is required"
                            ControlToValidate="ddlBranch" ValidationGroup="vgEmployeeInformation" ForeColor="Red">*</asp:RequiredFieldValidator></td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label43" runat="server" Text="Location"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: left">
                        <asp:DropDownList ID="ddlLocation" runat="server" Width="250px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>                
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label49" runat="server" Text="Is OT Eligible"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: left">                        
                        <asp:CheckBox ID="chkOT" runat="server" AutoPostBack="false"
                            oncheckedchanged="chkOT_CheckedChanged" />
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="lblOt" runat="server" Text="OT Session"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: left">                        
                        <asp:DropDownList ID="ddlOtSession" runat="server" Width="250px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                 <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label50" runat="server" Text="Is Working for Roster"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: left">                        
                        <asp:CheckBox ID="chkIsRoster" runat="server" />
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label52" runat="server" Text="Is E-Mails Exclude"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: left">                        
                        <asp:CheckBox ID="chkMailExclude" runat="server" />
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>                
                <tr style="width: 490px">
                    <td style="width: 200px; text-align: right">
                        <asp:Label ID="Label47" runat="server" Text="Modification Category"></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: left">
                        <asp:DropDownList ID="ddlModCategory" runat="server" Width="250px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px; text-align: right;">
                    </td>
                    <td style="width: 250px; text-align: right">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" ValidationGroup="vgEmployeeInformation"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Clear" Width="125px" OnClick="btnCancel_Click" />
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px; text-align: right;">
                    </td>
                    <td style="width: 250px; text-align: left">
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px; text-align: right;">
                    </td>
                    <td style="width: 250px; text-align: left">
                        <asp:HiddenField ID="hfEmployeeId" runat="server" />
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px; text-align: right;">
                    </td>
                    <td style="width: 250px; text-align: left">
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
    <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px; text-align: right;">
                    </td>
                    <td style="width: 250px; text-align: left; height:20px">
                        <%--<asp:UpdateProgress ID="upProgressBar" runat="server" style="margin:auto;width:200px" >
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>--%>
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align:center;">
                        <table style="margin:auto;">
                            <tr>
                                <td style="text-align:left;">
                                    <asp:Label ID="lblMessage" runat="server" Text="" TextMode="MultiLine"> </asp:Label>            
                                    <asp:ValidationSummary ID="vsEmployee" runat="server" ValidationGroup="vgEmployeeInformation" ForeColor="Red" />                                
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table><br />
            <asp:HiddenField ID="hfEmpID" runat="server" />
            <table>
                <tr style="width: 615px">
                    <td style="width: 200px; text-align: right;">
                        <asp:Label ID="Label37" runat="server" Text="Select employee to modify "></asp:Label>
                    </td>
                    <td style="width: 250px; text-align: left">
                        <asp:TextBox ID="txtEmployeeID" runat="server" ReadOnly="true" Width="250px" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td style="width: 20px">
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('webFrmEmployeeSearch.aspx','Search','txtEmployeeID')" />
                    </td>
                    <td style="width: 20px">
                        <asp:RequiredFieldValidator ID="rfvEmployeeID" runat="server" ErrorMessage="Please select the employee"
                            ControlToValidate="txtEmployeeID" ValidationGroup="vgSearch" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="width: 615px">
                    <td style="width: 200px; text-align: right;">
                    </td>
                    <td style="width: 250px; text-align: left">
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="125px" ValidationGroup="vgSearch" OnClick="btnEdit_Click" />
                        <asp:Button ID="btnSCancel" runat="server" Text="Clear" Width="125px" OnClick="btnSCancel_Click" />
                    </td>
                    <td style="width: 20px">
                        &nbsp;
                    </td>
                    <td style="width: 20px">
                    </td>
                </tr>
            </table>
            <br />
            <asp:ValidationSummary ID="vsSearch" runat="server" ValidationGroup="vgSearch" ForeColor="Red" />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
