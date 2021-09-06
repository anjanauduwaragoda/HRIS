<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="WebFrmBank.aspx.cs" Inherits="GroupHRIS.MetaData.Bank.WebFrmBank" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span style="font-weight: 700">Banks</span><br />
            <hr />
            <br />
            <table class="styleMainTb">
                <tr>
                    <td  class="styleMainTbLeftTD" style="width:250px">
                        Bank ID :
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:TextBox ID="BankIDTextBox" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="BankIDRequiredFieldValidator" ControlToValidate="BankIDTextBox" ValidationGroup="Banks" runat="server" Text="*" ForeColor="Red" ErrorMessage="Bank ID is Required"></asp:RequiredFieldValidator>
                        <asp:FilteredTextBoxExtender ID="BankIDFilteredTextBoxExtender" TargetControlID="BankIDTextBox" FilterType="Numbers" runat="server">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr>
                    <td  class="styleMainTbLeftTD">
                        Bank Name :
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:TextBox ID="BankNameTextBox" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="BankNameRequiredFieldValidator" ControlToValidate="BankNameTextBox" runat="server" Text="*" ForeColor="Red" ValidationGroup="Banks" ErrorMessage="Bank Name is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td  class="styleMainTbLeftTD">
                    </td>
                    <td  class="styleMainTbRightTD">
                        <asp:Button ID="ToggleButton" runat="server" Text="Save"  
                            ValidationGroup="Banks" OnClick="ToggleButton_Click" Width="100px" />
                        <asp:Button ID="ClearButton" runat="server" Text=" Clear " 
                            OnClick="ClearButton_Click" Width="100px" />
                    </td>
                </tr>
                <tr >
                    <td  class="styleMainTbLeftTD"></td>
                    <td class="styleMainTbRightTD"><asp:Label ID="StatusLabel" runat="server"> </asp:Label>
                          <asp:ValidationSummary ID="BankValidationSummary" ForeColor="Red" 
                            ValidationGroup="Banks" runat="server" /> 
                    </td>
                </tr>
            </table>
            <br /><br />
            <asp:GridView ID="SearchResultsGridView" style="margin:auto;" runat="server" AutoGenerateColumns="False"
                AllowPaging="true" OnPageIndexChanging="SearchResultsGridView_PageIndexChanging" 
                onselectedindexchanged="SearchResultsGridView_SelectedIndexChanged" 
                onrowdatabound="SearchResultsGridView_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="BANK_ID" HeaderText=" Bank ID " SortExpression="BANK_ID" />
                    <asp:BoundField DataField="BANK_NAME" HeaderText=" Bank Name " SortExpression="BANK_NAME" />
                </Columns>
                <EmptyDataTemplate>
                    No Data Returned
                </EmptyDataTemplate>
            </asp:GridView>
            <br />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
