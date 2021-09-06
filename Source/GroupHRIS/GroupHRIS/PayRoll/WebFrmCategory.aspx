<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmCategory.aspx.cs" Inherits="GroupHRIS.PayRoll.WebFrmCategory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
            <span style="font-weight: 700">Category</span><br />
            <hr />
            <br />
            <table class="styleMainTb">
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Category Name :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCategory" runat="server" Width="200px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="fteCategory" FilterType="Numbers,Custom,LowercaseLetters,UppercaseLetters" ValidChars="-_" TargetControlID="txtCategory"  runat="server">
                        </asp:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="rfvCategory" ValidationGroup="Category" runat="server"
                            Text="*" ForeColor="Red" ErrorMessage="Category Name is required" ControlToValidate="txtCategory"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Remarks :
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemarks" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" style="width: 250px">
                        Status :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStstus" runat="server" Width="100px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" Text = "*" ValidationGroup="Category"
                            ErrorMessage="Status is required" ForeColor="Red" 
                            ControlToValidate="ddlStstus"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td>
                        <asp:CheckBox ID="chkOtherTransactions" runat="server" Text="Display In Other Transactions" />
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" ValidationGroup="Category"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Category"
                            ForeColor="RED" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <asp:GridView ID="gvCategory" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
                AllowPaging="true" OnRowDataBound="gvCategory_RowDataBound" OnPageIndexChanging="gvCategory_PageIndexChanging"
                OnSelectedIndexChanged="gvCategory_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="CATEGORY" HeaderText=" Category Name " />
                    <asp:BoundField DataField="REMARKS" HeaderText=" Remarks " />
                    <asp:BoundField DataField="STATUS_CODE" HeaderText=" Status " />
                    <asp:BoundField DataField="IS_DISPLAY_IN_OTHER_PAYMENTS" HeaderText=" Is Display" />
                </Columns>
            </asp:GridView>
</asp:Content>
