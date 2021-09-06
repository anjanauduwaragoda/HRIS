<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="webFrmCompanyLocation.aspx.cs" Inherits="GroupHRIS.MetaData.Company.webFrmCompanyLocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <span style="font-weight: 700;">Company Location</span>
    <hr />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="styleMainTb">
                <tr>
                    <td class="styleMainTbLeftTD" style="width:250px">
                        Location ID :
                    </td>
                    <td class="styleMainTbRightTD">
                        <asp:Label ID="LocationIDLabel" runat="server"></asp:Label>
                    </td>
                    <td class="styleMainTbRightTD" style="width:200px">
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Company :
                    </td>
                    <td>
                        <asp:DropDownList ID="CompanyDropDownList" runat="server" AutoPostBack="True" 
                            OnSelectedIndexChanged="CompanyDropDownList_SelectedIndexChanged" 
                            Width="350px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="CompanyRequiredFieldValidator" runat="server" ErrorMessage="Company is Required"
                            Text="*" ValidationGroup="Grp01" ControlToValidate="CompanyDropDownList" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Location :
                    </td>
                    <td>
                        <asp:TextBox ID="LocationTextBox" runat="server" Width="350px" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="LocationRequiredFieldValidator" runat="server" ErrorMessage="Location is Required"
                            Text="*" ControlToValidate="LocationTextBox" ValidationGroup="Grp01" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Contact Number :
                    </td>
                    <td>
                        <asp:TextBox ID="ContactNumberTextBox" MaxLength="10" runat="server" onkeydown = "return (event.keyCode!=13);"> </asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="ContactNumberTextBoxFilteredTextBoxExtender" TargetControlID="ContactNumberTextBox"
                            FilterType="Numbers" runat="server">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                    <asp:RegularExpressionValidator ID="reLandPhone" runat="server" 
                    ControlToValidate="ContactNumberTextBox" 
                    ErrorMessage="Ten Characters required for Land Contact Number." ForeColor="Red" 
                    ValidationExpression="^([\S\s]{10,10})$" ValidationGroup="Grp01">*</asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" valign="top">
                        Address :
                    </td>
                    <td>
                        <asp:TextBox ID="AddressTextBox" TextMode="MultiLine" Width="350px" Height="40" onkeydown = "return (event.keyCode!=13);"
                            runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Address is Required"
                            Text="*" ControlToValidate="AddressTextBox" ValidationGroup="Grp01" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD" valign="top">
                        Remarks :
                    </td>
                    <td>
                        <asp:TextBox ID="RemarksTextBox" Width="350px" Height="100" onkeydown = "return (event.keyCode!=13);"
                            TextMode="MultiLine" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                        Status :
                    </td>
                    <td>
                        <asp:DropDownList ID="StatusDropDownList" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td><asp:RequiredFieldValidator ID="StatusRequiredFieldValidator" runat="server" ErrorMessage="Status is Required"
                            Text="*" ControlToValidate="StatusDropDownList" ValidationGroup="Grp01" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td>
                        <asp:Button ID="ToggleButton" runat="server" ValidationGroup="Grp01" Text="Save"
                            OnClick="ToggleButton_Click" Width="100px" />
                        <asp:Button ID="CancelButton" runat="server" Text="Clear" 
                            OnClick="ClearButton_Click" Width="100px" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="styleMainTbLeftTD">
                    </td>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary1" ForeColor="Red" ValidationGroup="Grp01"
                            runat="server" />
                        <asp:Label ID="StatusLabel" runat="server"></asp:Label>
                     </td>
                    <td>
                    </td>
                </tr>
            </table>
            <table style="margin: auto; margin-top: 50px;">
                <tr>
                    <td>
                        <asp:GridView ID="SearchResultsGridView" Style="margin: auto;" runat="server" AutoGenerateColumns="False"
                            AllowPaging="true" OnPageIndexChanging="SearchResultsGridView_PageIndexChanging"
                            OnSelectedIndexChanged="SearchResultsGridView_SelectedIndexChanged" OnRowDataBound="SearchResultsGridView_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="LOCATION_ID" HeaderText=" LOCATION ID " SortExpression="LOCATION_ID" />
                                <asp:BoundField DataField="LOCATION_NAME" HeaderText=" LOCATION " SortExpression="LOCATION_NAME" />
                                <asp:BoundField DataField="LOCATION_ADDRESS" HeaderText=" ADDRESS " SortExpression="LOCATION_ADDRESS" />
                                <asp:BoundField DataField="PHONE_NUMBER" HeaderText=" CONTACT NUMBER " SortExpression="PHONE_NUMBER" />
                                <asp:BoundField DataField="REMARKS" HeaderText=" REMARKS " SortExpression="REMARKS" />
                                <asp:BoundField DataField="COMP_NAME" HeaderText=" COMPANY NAME " SortExpression="COMP_NAME" />
                                <asp:BoundField DataField="STATUS" HeaderText=" Status " SortExpression="STATUS" />                                
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>