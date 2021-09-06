<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmExpectedExpenceAndDetails.aspx.cs"
    Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmExpectedExpenceAndDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
    </style>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span>Expected Expence Details</span><hr />
            <table style="background-color: #aed6f1; width: 100%">
                <tr>
                    <td class="styleTableCell1">
                        Training :
                    </td>
                    <td style="width: 250px">
                        <asp:TextBox ID="txtTraining" runat="server" Width="200px" ValidationGroup="expence"
                            ReadOnly="True"></asp:TextBox>
                    </td>
                    <td style="width: 30px">
                        <asp:Image ID="searchEmp" runat="server" alt="" src="../Images/Common/search-icon-01.png"
                            Height="22px" Width="25px" onclick="openLOVWindow('../TrainingAndDevelopment/WebFrmTrainingSearch.aspx','search','txtTraining')"
                            ImageUrl="~/Images/Common/search-icon-01.png" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Training is required"
                            Text="*" ControlToValidate="txtTraining" ForeColor="Red" ValidationGroup="expence"></asp:RequiredFieldValidator>
                    </td>
                    <td align="right">
                        Training Name :
                    </td>
                    <td>
                        <asp:Label ID="lblTrainingName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td align="right">
                        Program Name :
                    </td>
                    <td>
                        <asp:Label ID="lblProgramName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Description :
                    </td>
                    <td>
                        <asp:TextBox ID="txtTrDescription" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Description is required"
                            Text="*" ForeColor="Red" ControlToValidate="txtTrDescription" ValidationGroup="expence"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td>
                    </td>
                    <td align="right">
                        Remarks :
                    </td>
                    <td>
                        <asp:TextBox ID="txtTrRemark" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                        Status :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTrStatus" runat="server" Width="204px">
                        </asp:DropDownList>
                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Expected expence status is required"
                            Text="*" ForeColor="Red" ControlToValidate="ddlTrStatus" ValidationGroup="expence"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td>
                    </td>
                    <td align="right">
                        Per-Person Cost :
                    </td>
                    <td>
                        <asp:TextBox ID="txtTrPerPersonCost" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="styleTableCell1">
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td align="right">
                        Total Cost :
                    </td>
                    <td>
                        <asp:TextBox ID="txtTrTotal" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <%-- <asp:Button ID="btnTrSave" runat="server" Text="Save" Width="100px" 
                            onclick="btnTrSave_Click" />
                        <asp:Button ID="btnTrClear" runat="server" Text="Clear" Width="100px" 
                            onclick="btnTrClear_Click" />--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <%--<asp:Label ID="lblMessage" runat="server"></asp:Label>--%>
                    </td>
                </tr>
            </table>
            <br />
            <div style="text-align: center;">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="../Images/ProBar/720.GIF" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div style="text-align: center;">
                <asp:Label ID="lblMessage" runat="server"></asp:Label></div>
            <br />
            <span>Detail Expences</span><hr />
            <table>
                <tr>
                    <td style="background-color: #aed6f1; width: 45%;">
                        <table>
                            <tr>
                                <td class="styleTableCell1" style="vertical-align: top;">
                                    Expence Category :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcategory" runat="server" Width="205px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Expense category is required"
                                        Text="*" ForeColor="Red" ControlToValidate="ddlcategory" ValidationGroup="expence"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleTableCell1" style="vertical-align: top;">
                                    Expence Description :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDescription" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Expence description is required"
                                        Text="*" ForeColor="Red" ControlToValidate="txtDescription" ValidationGroup="expence"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleTableCell1" style="vertical-align: top;">
                                    Remarks :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemark" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleTableCell1" style="vertical-align: top;">
                                    Estimated Cost :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCost" runat="server" Width="200px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Estimated cost is required"
                                        Text="*" ForeColor="Red" ControlToValidate="txtCost" ValidationGroup="expence"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleTableCell1" style="vertical-align: top;">
                                    Status :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="205px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Detail expence status is required"
                                        Text="*" ControlToValidate="ddlStatus" ForeColor="Red" ValidationGroup="expence"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnDetailSave" runat="server" Text="Save" Width="103px" OnClick="btnDetailSave_Click"
                                        ValidationGroup="expence" /><asp:Button ID="btnDetailClear" runat="server" Text="Clear"
                                            Width="103px" OnClick="btnDetailClear_Click" />
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:Button ID="btnProcess" runat="server" Text="Process>>" OnClick="btnProcess_Click" />
                    </td>
                    <td>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:GridView ID="grdCompanywiseExpence" runat="server" AutoGenerateColumns="false"
                                        AllowPaging="false" OnRowDataBound="grdCompanywiseExpence_RowDataBound" PageSize="5" Style="width: 100%;">
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
                                                        TextMode="MultiLine"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="" HeaderText="Amount " ><ItemStyle HorizontalAlign="Center" /></asp:BoundField>--%>
                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="lblcmpAmount" runat="server"></asp:Label>--%>
                                                    <asp:TextBox ID="lblcmpAmount" runat="server" Width="95%" Style="border: 1px;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom, Numbers"
                                                        ValidChars="." runat="server" TargetControlID="lblcmpAmount">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlcompStatus" Width="100%" runat="server" Style="border: 0px;
                                                        text-align: center;">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataRowStyle BackColor="#aed6f1" />
                                        <EmptyDataTemplate>
                                            THERE ARE NO COMPANY EXPENCE DETAILS ASSICIATED WITH THE SELECTED TRAINING.
                                        </EmptyDataTemplate>
                                    </asp:GridView>
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
            <br />
            <div style="text-align: center;">
                <asp:Label ID="lblDetailMessage" runat="server"></asp:Label>
            </div>
            <br />
            <table style="width: 100%">
                <tr>
                    <td align="center">
                        <asp:GridView ID="grdexpCategoryDetails" runat="server" AutoGenerateColumns="false"
                            OnPageIndexChanging="grdexpCategoryDetails_PageIndexChanging" OnRowDataBound="grdexpCategoryDetails_RowDataBound"
                            OnSelectedIndexChanged="grdexpCategoryDetails_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="EXPENSE_CATEGORY_ID" HeaderText="Expence Category Id"
                                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="CATEGORY_NAME" HeaderText="Category Name " />
                                <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                                <asp:BoundField DataField="AMOUNT" HeaderText="Amount" />
                                <asp:BoundField DataField="REMARKS" HeaderText="Remark" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                                <asp:BoundField DataField="RECORD_ID" HeaderText="Record id" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                            </Columns>
                            <EmptyDataTemplate>
                                THERE ARE NO EXPENCE DETAILS ASSICIATED WITH THE SELECTED TRAINING.
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
