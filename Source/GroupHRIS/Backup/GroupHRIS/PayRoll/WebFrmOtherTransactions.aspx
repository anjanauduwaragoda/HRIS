<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/Mastermain.Master"
    EnableEventValidation="false" CodeBehind="WebFrmOtherTransactions.aspx.cs" Inherits="GroupHRIS.PayRoll.WebFrmOtherTransactions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <script language="javascript" type="text/javascript">

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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span style="font-weight: 700">Other Transactions</span><br />
            <hr />
            <br />
            <table class="styleMainTb">
                <tr>
                    <td class="styleMainTbLeftTD">
                        Employee ID :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmploeeId" runat="server" ReadOnly="true" ClientIDMode="Static" OnTextChanged="txtEmploeeId_TextChanged"
                            Width="200px" AutoPostBack="True"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvEmployeeId" runat="server" ErrorMessage="Employee ID is Required"
                            ControlToValidate="txtEmploeeId" ForeColor="Red" ValidationGroup="Transaction">*</asp:RequiredFieldValidator>
                        <img alt="" src="../Images/Common/Search.jpg" height="20px" width="20px" onclick="openLOVWindow('../Employee/webFrmEmployeeSearch.aspx','Search','txtEmploeeId')" />
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Employee EPF No :
                    </td>
                    <td>
                        <asp:Label ID="lblEPFNo" runat="server"></asp:Label>
                    </td>
                    <td class="styleTableCell4">
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Employee Name :
                    </td>
                    <td>
                        <asp:Label ID="lblEmployeeName" runat="server"></asp:Label>
                    </td>
                    <td class="styleTableCell4">
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Company :
                    </td>
                    <td>
                        <%--<asp:DropDownList ID="ddlCompany" runat="server" Width="200px">
                        </asp:DropDownList>--%>
                        <asp:Label ID="lblCompany" runat="server" Width="200px"></asp:Label>
                    </td>
                    <td>
                        <%--<asp:RequiredFieldValidator ID="rfvCompany" ValidationGroup="Transaction"
                            runat="server" Text="*" ForeColor="Red" ErrorMessage="Company is Required"
                            ControlToValidate="ddlCompany"></asp:RequiredFieldValidator>--%>
                        <asp:HiddenField ID="hfcompanyId" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Category :
                    </td>
                    <td width="200">
                        <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                            Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvCategory" ValidationGroup="Transaction" runat="server"
                            Text="*" ForeColor="Red" ErrorMessage="Category is required" ControlToValidate="ddlCategory"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Subcategory :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSubcategory" runat="server" OnSelectedIndexChanged="ddlSubcategory_SelectedIndexChanged"
                            AutoPostBack="True" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td class="styleTableCell4">
                        <asp:RequiredFieldValidator ID="rfvSubcategory" ValidationGroup="Transaction" runat="server"
                            Text="*" ForeColor="Red" ErrorMessage="Subcategory is required" ControlToValidate="ddlSubcategory"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Type ID :
                    </td>
                    <td>
                        <asp:Label ID="lblTypeId" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Amount :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAmount" runat="server" Width="198px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Amount is required"
                            ControlToValidate="txtAmount" ForeColor="Red" ValidationGroup="Transaction">*</asp:RequiredFieldValidator>
                        <asp:FilteredTextBoxExtender ID="fteAmount" FilterType="Numbers,Custom" ValidChars="01234567890."
                            runat="server" TargetControlID="txtAmount">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                    </td>
                    <td colspan="2">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" ValidationGroup="Transaction"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="StatusLabel" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Transaction"
                ForeColor="RED" />
            <br />
            <br />
            Transaction Month :
            <asp:Label ID="lblTransactionMonth" runat="server"></asp:Label>
            <hr />
            <br />
            <asp:GridView ID="gvTransactions" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
                AllowPaging="true" OnPageIndexChanging="gvTransactions_PageIndexChanging" OnRowDataBound="gvTransactions_RowDataBound"
                OnSelectedIndexChanged="gvTransactions_SelectedIndexChanged">
                <Columns>
                    <%--<asp:BoundField DataField="EMPLOYEE_ID" HeaderText=" Employee Id "/>--%>
                    <asp:BoundField DataField="CATEGORY" HeaderText=" Category " />
                    <asp:BoundField DataField="SUB_CATEGORY" HeaderText=" Subcategory " />
                    <asp:BoundField DataField="TYPE_ID" HeaderText=" Type ID " />
                    <asp:BoundField DataField="AMOUNT" HeaderText=" Amount " />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
