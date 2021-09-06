<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmBankBranch.aspx.cs" Inherits="GroupHRIS.MetaData.Bank.WebFrmBankBranch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            font-size: large;
            margin: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <span style="font-weight: 700">Bank Branch</span><br />
            <hr />
            <br />
            <table style="margin: auto;" class="styleMainTb">
                <tr>
                    <td style="text-align: right;" class="styleMainTbLeftTD" valign="top">
                        Bank :
                    </td>
                    <td>
                        <asp:DropDownList ID="BankDropDownList" runat="server" AutoPostBack="True" 
                            OnSelectedIndexChanged="BankDropDownList_SelectedIndexChanged" 
                            Width="300px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="BankRequiredFieldValidator" ControlToValidate="BankDropDownList"
                            runat="server" Text="*" ForeColor="Red" ValidationGroup="Branches" ErrorMessage="Bank is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;" class="styleMainTbLeftTD" valign="top">
                        Bank Branch Code :
                    </td>
                    <td>
                        <asp:TextBox ID="BranchIDTextBox" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="BranchIDRequiredFieldValidator" ControlToValidate="BranchIDTextBox"
                            runat="server" Text="*" ForeColor="Red" ValidationGroup="Branches" ErrorMessage="Branch ID is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;" class="styleMainTbLeftTD" valign="top">
                        Branch Name :
                    </td>
                    <td>
                        <asp:TextBox ID="BranchTextBox" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="BranchRequiredFieldValidator" ControlToValidate="BranchTextBox"
                            runat="server" Text="*" ForeColor="Red" ValidationGroup="Branches" ErrorMessage="Branch Name is Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;" class="styleMainTbLeftTD" valign="top">
                        Branch Address :
                    </td>
                    <td>

                        <asp:TextBox ID="AddressTextBox" runat="server"  Width="300px" hight="100px" 
                            TextMode="MultiLine"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;" class="styleMainTbLeftTD" valign="top">
                        Land Phone 1 :
                    </td>
                    <td>

                        <asp:TextBox ID="LandPhone1TextBox" MaxLength="10" runat="server" Height="26px" 
                             Width="177px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="LandPhone1FilteredTextBoxExtender" runat="server" TargetControlID="LandPhone1TextBox" FilterType="Numbers">

                        </asp:FilteredTextBoxExtender>
                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="LandPhone1TextBox"
                            ID="RegularExpressionValidator2" ValidationExpression="^[\s\S]{10,}$" runat="server"
                            ForeColor="Red" ErrorMessage="Minimum 10 Numbers required." ValidationGroup="Branches"
                            Text="*"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;" class="styleMainTbLeftTD" valign="top">
                        Land Phone 2 :
                    </td>
                    <td>

                        <asp:TextBox ID="LandPhone2TextBox" MaxLength="10" runat="server"  Width="177px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="LandPhone2FilteredTextBoxExtender" runat="server" TargetControlID="LandPhone2TextBox" FilterType="Numbers">

                        </asp:FilteredTextBoxExtender>
                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="LandPhone2TextBox"
                            ID="LandPhone2RegularExpressionValidator" ValidationExpression="^[\s\S]{10,}$"
                            runat="server" ForeColor="red" ErrorMessage="Minimum 10 Numbers required." ValidationGroup="Branches"
                            Text="*"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" valign="top">
                    </td>
                    <td>
                        <asp:HiddenField ID="BranchIDHiddenField" runat="server" />
                        <asp:Button ID="ToggleButton" runat="server" ValidationGroup="Branches" Text="Save"
                            OnClick="ToggleButton_Click" Width="120px" />
                        <asp:Button ID="ClearButton" runat="server" Text="Clear" OnClick="ClearButton_Click"
                            Width="120px" />
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        </td>
                        <td >
                        <asp:Label ID="StatusLabel" runat="server"> </asp:Label>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:ValidationSummary ID="BankBranchValidationSummary" runat="server" Text="*" ForeColor="Red"
                            ValidationGroup="Branches" />
                        </td>

                </tr>
            </table>
            <br />
            <asp:GridView ID="SearchResultsGridView" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
                AllowPaging="true" OnPageIndexChanging="SearchResultsGridView_PageIndexChanging"
                OnSelectedIndexChanged="SearchResultsGridView_SelectedIndexChanged" OnRowDataBound="SearchResultsGridView_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="BRANCH_ID" HeaderText=" Branch ID " SortExpression="BRANCH_ID" />
                    <asp:BoundField DataField="BRANCH_NAME" HeaderText=" Branch Name " SortExpression="BRANCH_NAME" />
                    <asp:BoundField DataField="BANK_NAME" HeaderText=" Bank Name " SortExpression="BANK_NAME" />
                    <asp:BoundField DataField="ADDRESS" HeaderText=" Address " SortExpression="ADDRESS" />
                    <asp:BoundField DataField="LAND_PHONE1" HeaderText=" Land Phone 1 " SortExpression="LAND_PHONE1" />
                    <asp:BoundField DataField="LAND_PHONE2" HeaderText=" Land Phone 2 " SortExpression="LAND_PHONE2" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
