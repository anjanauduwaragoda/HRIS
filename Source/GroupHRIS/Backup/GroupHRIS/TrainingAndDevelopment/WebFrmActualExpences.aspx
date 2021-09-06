<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    CodeBehind="WebFrmActualExpences.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmActualExpences"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function openLOVWindow(file, window, ctlName) {
            txb = ctlName;
            childWindow = open(file, window, 'resizable=no,width=950,height=780,scrollbars=yes,top=50,left=200,status=yes');

            document.getElementById("hfCaller").value = ctlName;
        }

        function getValueFromChild(sTrId, sTrName, sProName) {
            var ctl = document.getElementById("hfCaller").value;

            //alert("ctl : " + ctl + " : sTrId : " + sTrId);
            //document.getElementById(ctl).value = sTrId;

            document.getElementById("hfVal").value = sTrId;
            document.getElementById("hfTrName").value = sTrName;
            document.getElementById("hfProName").value = sProName;
            //alert("sTrId : " + sTrId);
            DoPostBack();
        }

        function DoPostBack() {
            __doPostBack();
        }
       
    </script>
    <style type="text/css">
        .style1
        {
            min-width: 133px;
            text-align: right;
            vertical-align: top;
        }
        .hideGridColumn
        {
            display: none;
        }
        .style3
        {
            width: 116px;
        }
        .style4
        {
            width: 403px;
        }
        .style5
        {
            width: 192px;
        }
        .AlgRgh
        {
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span>Actual Expenses</span><hr />
            <br />
            <table style="background-color: #aed6f1;" width="100%">
                <tr>
                    <td id="rightTbl" style="background-color: #aed6f1; padding: 10px 10px 10px 20px;"
                        align="center">
                        <table>
                            <tr>
                                <td class="style3" align="right">
                                    Training ID:
                                </td>
                                <td class="style5">
                                    <asp:TextBox ID="txtTraining" runat="server" Width="200px" ValidationGroup="expence"
                                        ReadOnly="True"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Image ID="searchEmp" runat="server" alt="" src="../Images/Common/search-icon-01.png"
                                        Height="25px" Width="25px" onclick="openLOVWindow('../TrainingAndDevelopment/WebFrmTrainingSearch.aspx','search','txtTraining')"
                                        ImageUrl="~/Images/Common/search-icon-01.png" />
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Training is required"
                                        Text="*" ControlToValidate="txtTraining" ForeColor="Red" ValidationGroup="vgActualExpense"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style3">
                                    Training Name :
                                </td>
                                <td class="style5">
                                    <asp:Label ID="lblTrainingName" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style3">
                                    Program Name :
                                </td>
                                <td class="style5">
                                    <asp:Label ID="lblProgramName" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Per-Person Cost :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTrPerPersonCost" runat="server" Width="200px" ReadOnly="true"
                                        Style="text-align: right;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Total Cost :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTrTotal" runat="server" Width="200px" ReadOnly="true" Style="text-align: right;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label1" runat="server" Text="Total Discount : "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalDiscount" runat="server" Width="200px" ReadOnly="true" Style="text-align: right;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label2" runat="server" Text="Grand Total : "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox Style="text-align: right;" ID="txtGrandTotal" runat="server" Width="200px"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td id="leftTbl" style="background-color: #aed6f1; padding: 10px 10px 10px 30px;"
                        class="style4" valign="top">
                        <table style="background-color: #aed6f1;">
                            <%--<tr>
                        <td class="style3" align="right">
                            Training :
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="txtTraining" runat="server" Width="200px" ValidationGroup="expence"
                                ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Image ID="searchEmp" runat="server" alt="" src="../Images/Common/search-icon-01.png"
                                Height="22px" Width="25px" onclick="openLOVWindow('../TrainingAndDevelopment/WebFrmTrainingSearch.aspx','search','txtTraining')"
                                ImageUrl="~/Images/Common/search-icon-01.png" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Training is required"
                                Text="*" ControlToValidate="txtTraining" ForeColor="Red" ValidationGroup="vgActualExpense"></asp:RequiredFieldValidator>
                        </td>
                    </tr>--%>
                            <%--<tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>--%>
                            <tr>
                                <td class="" align="right" valign="top">
                                    Description :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTrDescription" runat="server" Width="200px" MaxLength="300" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="" align="right">
                                    <%--Status :--%>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTrStatus" runat="server" Width="206px" Visible="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="" align="right">

                                </td>
                                <td align="right">
                                    <asp:Button ID="btnClearAll" runat="server" Text="Clear" Width="125px" 
                                        onclick="btnClearAll_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <%--<td class="styleDdlSmall" style="background-color: #aed6f1;">
            </td>--%>
                </tr>
            </table>
            <br />
            <div style="text-align: center;">
                <asp:Label ID="lblErrorMsg" runat="server"></asp:Label></div>
            <br />
            <span>Detail Expenses</span><hr />
            <table>
                <tr>
                    <td valign="top" align="right" style="min-width: 358px;">
                        <table style="background-color: #aed6f1; padding: 10px 10px 10px 10px;">
                            <tr>
                                <td class="style1">
                                    Expense Category :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcategory" runat="server" Width="205px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ErrorMessage="Expense Category is required"
                                        ValidationGroup="vgDetailExpense" ControlToValidate="ddlcategory" ForeColor="Red">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Expense Description :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDescription" runat="server" Width="200px" TextMode="MultiLine"
                                        MaxLength="300"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Remarks :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemark" runat="server" Width="200px" TextMode="MultiLine" MaxLength="300"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Actual Cost :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCost" runat="server" Width="200px" Style="text-align: right;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:FilteredTextBoxExtender ID="fteCost" runat="server" ValidChars="." FilterType="Numbers, Custom"
                                        TargetControlID="txtCost">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Actual Cost is required"
                                        ValidationGroup="vgDetailExpense" ControlToValidate="txtCost" ForeColor="Red">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Discount :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDiscount" runat="server" Width="200px" Style="text-align: right;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" ValidChars="."
                                        FilterType="Numbers, Custom" TargetControlID="txtDiscount">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Net Amount :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNetAmount" runat="server" Width="200px" Style="text-align: right;"
                                        ReadOnly="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" ValidChars='.'
                                        FilterType="Numbers, Custom" TargetControlID="txtNetAmount">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Net Amount is required"
                                        ValidationGroup="vgDetailExpense" ControlToValidate="txtNetAmount" ForeColor="Red">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Is Paid :
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkPaid" runat="server" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Payment Description :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExpencePaymentDescription" runat="server" Width="200px" TextMode="MultiLine"
                                        MaxLength="300"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    Status :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="205px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Status is required"
                                        ValidationGroup="vgDetailExpense" ControlToValidate="ddlStatus" ForeColor="Red">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                        <%--<table>
                    <tr>
                        
                        <td align="right" style="text-align:right; width:366px; padding-right:0px" colspan="2">--%>
                        <br />
                        <asp:Button ID="btnDetailAdd" runat="server" Text="Save" Width="101px" OnClick="btnDetailAdd_Click"
                            ValidationGroup="vgActualExpense" />
                        <asp:Button ID="btnDetailClear" runat="server" Text="Clear" Width="101px" OnClick="btnDetailClear_Click" />
                        <table border="0" cellpadding="0" cellspacing="0" style="margin-left: 0px" width="300px">
                            <tr>
                                <td align="left">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgDetailExpense"
                                        ForeColor="Red" Style="margin-left: 78px" />
                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="vgActualExpense"
                                        ForeColor="Red" Style="margin-left: 78px" />
                                </td>
                            </tr>
                        </table>
                        <%--</td>
                    </tr>
                </table>--%>
                    </td>
                    <td>
                        <asp:Button ID="btnProcess" runat="server" Text="Process >>" OnClick="btnProcess_Click"
                            ValidationGroup="vgDetailExpense" />
                        <br />
                    </td>
                    <td>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:GridView ID="gvCompanyWiseActualExpenses" runat="server" AutoGenerateColumns="false"
                                        Style="width: 100%;" AllowPaging="false" PageSize="5" OnRowDataBound="gvCompanyWiseActualExpenses_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="COMPANY_ID" HeaderText="Company Id" HeaderStyle-CssClass="hideGridColumn"
                                                ItemStyle-CssClass="hideGridColumn" />
                                            <asp:BoundField DataField="COMP_NAME" HeaderText="Company " />
                                            <asp:BoundField DataField="PLANNED_PARTICIPANTS" HeaderText="No Of Participant ">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Remarks">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtcompanyRemark" runat="server" Width="95%" Style="border: 1px;"
                                                        TextMode="MultiLine" placeholder="Enter Text Here"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount (Rs.)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblcmpAmount" runat="server" Width="95%" Style="border: 1px;" CssClass="AlgRgh"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="lblcmpAmount"
                                                        FilterType="Numbers, Custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlcompStatus" Width="100%" runat="server" Style="border: 0px;
                                                        text-align: center;">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>--%>
                                        </Columns>
                                        <EmptyDataRowStyle BackColor="#aed6f1" Width="650px" />
                                        <EmptyDataTemplate>
                                            NO COMPANY WISE EXPENSE DETAILS ASSOCIATED WITH THE SELECTED EXPENSE.
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblErrorMsgCompany" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <%-- <td align="right">
                        <asp:Button ID="btnDetailSave" runat="server" Text="Save" Width="100px" 
                            onclick="btnDetailSave_Click" /><asp:Button
                            ID="btnDetailClear" runat="server" Text="Clear" Width="100px" />
                    </td>--%>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
            <div style="text-align: center;">
                <asp:Label ID="lblDetailMessage" runat="server"></asp:Label>
            </div>
            <br />
            <table style="width: 100%">
                <tr>
                    <td align="center">
                        <asp:GridView ID="gvCategoryDetails" runat="server" AutoGenerateColumns="false" OnSelectedIndexChanged="gvCategoryDetails_SelectedIndexChanged"
                            OnRowDataBound="gvCategoryDetails_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="RECORD_ID" HeaderText="Record id" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="EXPENSE_CATEGORY_ID" HeaderText="Expence Category Id"
                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Category Name " />
                                <%--<asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />--%>
                                <asp:BoundField DataField="AMOUNT" HeaderText="Amount" />
                                <asp:BoundField DataField="DISCOUNT" HeaderText="Discount" />
                                <asp:BoundField DataField="NET_AMOUNT" HeaderText="Net Amount" />
                                <%--<asp:BoundField DataField="REMARKS" HeaderText="Remark" />--%>
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                            </Columns>
                            <EmptyDataTemplate>
                                THERE ARE NO EXPENCE DETAILS ASSOCIATED WITH THE SELECTED TRAINING.
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfCaller" runat="server" ClientIDMode="Static" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfglId" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfTrName" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfProName" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfrecordVal" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
            <asp:HiddenField ID="hfExpenseId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfRecordId" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
