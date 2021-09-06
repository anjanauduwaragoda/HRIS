<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="webFrmEmployeeLeaveScheme.aspx.cs"
    Inherits="GroupHRIS.EmployeeLeave.webFrmEmployeeLeaveScheme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleEmpLeaveSchme.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

//        function openLOVWindow(file, window, ctlName) {
//            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');
//            //if (childWindow.opener == null) childWindow.opener = self;

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
    <style type="text/css">
            .hideGridColumn
            {
                display: none;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <span>Employee Leave Scheme Information</span>
    <hr />
    <br />
    <table class="styleTable">
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                <asp:Label ID="Label1" runat="server" Text="Employee Id"></asp:Label>
            </td>
            <td class="styleTableCell2">
                <asp:TextBox ID="txtEmploeeId" runat="server" ReadOnly="true" CssClass="styleTableCell2TextBox" MaxLength="8"
                    ClientIDMode="Static"></asp:TextBox>
            </td>
            <td class="styleTableCell3">
                <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmploeeId')" />
            </td>
            <td class="styleTableCell4">
                <asp:RequiredFieldValidator ID="rfvEmployeeId" runat="server" ErrorMessage="Employee Id is required"
                    ControlToValidate="txtEmploeeId" ForeColor="Red" ValidationGroup="elScheme">*</asp:RequiredFieldValidator>
            </td>
            <td class="styleTableCell5">
                &nbsp;
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                <asp:Label ID="Label2" runat="server" Text="Leave Scheme Id"></asp:Label>
            </td>
            <td class="styleTableCell2">
                <asp:DropDownList ID="ddlLeaveScheme" runat="server" CssClass="styleDdl" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlLeaveScheme_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
                <asp:RequiredFieldValidator ID="rfvLeaveScheme" runat="server" ErrorMessage="Leave Scheme Id is required"
                    ControlToValidate="ddlLeaveScheme" ForeColor="Red" ValidationGroup="elScheme">*</asp:RequiredFieldValidator>
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
            </td>
            <td class="styleTableCell2">
                <asp:GridView ID="gvScheme" runat="server" AutoGenerateColumns="False" Style="width: 250px;">
                    <Columns>
                        <asp:BoundField DataField="LEAVE_TYPE_NAME" HeaderText="NAME"></asp:BoundField>
                        <asp:BoundField DataField="NUMBER_OF_DAYS" HeaderText="Amount"></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                <asp:Label ID="Label5" runat="server" Text="Status"></asp:Label>
            </td>
            <td class="styleTableCell2">
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="styleDdl" ValidationGroup="vgBankAccount">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
                <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ErrorMessage="Status is required"
                    ControlToValidate="ddlStatus" ForeColor="#FF3300" ValidationGroup="elScheme">*</asp:RequiredFieldValidator>
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
                <asp:Label ID="Label3" runat="server" Text="Remarks"></asp:Label>
            </td>
            <td class="styleTableCell2">
                <asp:TextBox ID="txtRemarks" runat="server" CssClass="styleTableCell2TextBox" MaxLength="100"
                    TextMode="MultiLine"></asp:TextBox>
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
            </td>
            <td class="styleTableCell2">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" OnClick="btnSave_Click"
                    ValidationGroup="elScheme" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="125px" OnClick="btnClear_Click" />
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
            </td>
            <td class="styleTableCell2">
                <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
            </td>
            <td class="styleTableCell2">
                <asp:HiddenField ID="hfLineNo" runat="server" ClientIDMode="Static" />
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>
        <tr class="styleTableRow">
            <td class="styleTableCell1">
            </td>
            <td class="styleTableCell2">
                <asp:HiddenField ID="hfEmpId" runat="server" ClientIDMode="Static" />
            </td>
            <td class="styleTableCell3">
            </td>
            <td class="styleTableCell4">
            </td>
            <td class="styleTableCell5">
            </td>
        </tr>       
        <tr class="styleTableRow">
            <td class="styleTableCell1">
            </td>
            <td colspan="5">
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <asp:ValidationSummary ID="vsEmployeeLeaveScheme" runat="server" ForeColor="Red"
        ValidationGroup="elScheme" />
    <br />
    <span>Leave Scheme History</span>
    <hr />
    <br />
    <table>
        <tr>
            <td style="text-align: center">
                <asp:GridView ID="gvLeaveSchemeHistory" runat="server" AutoGenerateColumns="False"
                    OnRowDataBound="gvLeaveSchemeHistory_RowDataBound" OnSelectedIndexChanged="gvLeaveSchemeHistory_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="LEAVE_SCHEME_ID" HeaderText="SCHEME_ID" />
                        <asp:BoundField DataField="LEAVE_SCHEM_NAME" HeaderText="NAME" />
                        <asp:BoundField DataField="STATUS" HeaderText="STATUS" />
                        <asp:BoundField DataField="REMARKS" HeaderText="REMARKS" />
                        <asp:BoundField DataField="LINE_NO" HeaderText="LINE_NO" HeaderStyle-CssClass = "hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td style="text-align: center">
                <asp:Label ID="lblSchemeName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: center">
                <asp:GridView ID="gvSchemeDetail" runat="server" AutoGenerateColumns="False" 
                    onrowdatabound="gvSchemeDetail_RowDataBound" 
                    onselectedindexchanged="gvSchemeDetail_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="LEAVE_TYPE_NAME" HeaderText="LEAVE_TYPE" />
                        <asp:BoundField DataField="NUMBER_OF_DAYS" HeaderText="DAYS" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <asp:LinkButton ID="lbtnClear" runat="server" onclick="lbtnClear_Click">Clear Leave Scheme Details</asp:LinkButton>
</asp:Content>
