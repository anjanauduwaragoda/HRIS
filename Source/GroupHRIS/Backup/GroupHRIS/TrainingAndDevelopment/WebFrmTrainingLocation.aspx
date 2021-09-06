<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="WebFrmTrainingLocation.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainingLocation" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <span>Training Location Details</span><hr />
            <table>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        Location Name :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLocation" runat="server" Width="200px" MaxLength="150"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Location Name is required"
                            ControlToValidate="txtLocation" ForeColor="Red" Text="*" ValidationGroup="location"></asp:RequiredFieldValidator>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" FilterType="Custom, Numbers,LowercaseLetters, UppercaseLetters"
                            ValidChars="/,-,' '" runat="server" TargetControlID="txtLocation">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        Address :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Address is required"
                            ControlToValidate="txtAddress" ForeColor="Red" Text="*" ValidationGroup="location"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        Province :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProvince" runat="server" Width="205px" 
                            AutoPostBack="True" onselectedindexchanged="ddlProvince_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Province is required"
                            ControlToValidate="ddlProvince" ForeColor="Red" Text="*" ValidationGroup="location"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                 <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        District :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDistrict" runat="server" Width="205px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="District is required"
                            ControlToValidate="ddlDistrict" ForeColor="Red" Text="*" ValidationGroup="location"></asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        Primary Contact :
                    </td>
                    <td>
                        <asp:TextBox ID="txtContact1" runat="server" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Primary Contact is required"
                            ControlToValidate="txtContact1" ForeColor="Red" Text="*" ValidationGroup="location"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revPhone" runat="server" ValidationGroup="location"
                            ControlToValidate="txtContact1" ValidationExpression="^([\S\s]{10,10})$" ErrorMessage="Ten numbers are required for the Phone Number"
                            ForeColor="Red">*</asp:RegularExpressionValidator>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" FilterType="Numbers"
                             runat="server" TargetControlID="txtContact1">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        Secondary Contact :
                    </td>
                    <td>
                        <asp:TextBox ID="txtContact2" runat="server" Width="200px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="location"
                            ControlToValidate="txtContact2" ValidationExpression="^([\S\s]{10,10})$" ErrorMessage="Ten numbers are required for the Phone Number"
                            ForeColor="Red">*</asp:RegularExpressionValidator>
                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" FilterType="Numbers"
                             runat="server" TargetControlID="txtContact2">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        E-mail :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="E-mail is required"
                            ControlToValidate="txtEmail" ForeColor="Red" Text="*" ValidationGroup="location"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Invalid e-mail address"
                            ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            ValidationGroup="location" ForeColor="Red">*</asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        Location Capacity :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCapacity" runat="server" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Location capacity is required"
                            ControlToValidate="txtCapacity" ForeColor="Red" ValidationGroup="location" Text="*"></asp:RequiredFieldValidator>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType=" Numbers"
                            runat="server" TargetControlID="txtCapacity">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        Description :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescription" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        Bank :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBank" runat="server" Width="205px" OnSelectedIndexChanged="ddlBank_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Text="*"
                            ErrorMessage="Bank is required" ControlToValidate="ddlBank" ForeColor="Red" ValidationGroup="location"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        Bank Branch :
                        <td>
                            <asp:DropDownList ID="ddlBankBranch" runat="server" Width="205px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Bank branch is required"
                                ControlToValidate="ddlBankBranch" ForeColor="Red" Text="*" ValidationGroup="location"></asp:RequiredFieldValidator>
                        </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        Bank Account Number :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAccNo" runat="server" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Bank account number is required"
                            ControlToValidate="txtCapacity" ForeColor="Red" ValidationGroup="location" Text="*"></asp:RequiredFieldValidator>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" FilterType="Numbers" runat="server"
                            TargetControlID="txtAccNo">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        Payment Instruction :
                    </td>
                    <td>
                        <asp:TextBox ID="txtInstruction" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Payment instruction is required"
                                ControlToValidate="txtInstruction" ForeColor="Red" Text="*" ValidationGroup="location"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                        Status :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="205px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Status is required"
                            ControlToValidate="ddlStatus" ForeColor="Red" Text="*" ValidationGroup="location"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" OnClick="btnSave_Click"
                            ValidationGroup="location" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="styleTableRow">
                    <td class="styleTableCell1">
                    </td>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="location" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
            <br />
             <span>Training Locations </span>
            <hr />
            <table style="width: 100%;">
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td class="styleTableCell1">
                                    Province :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProvincesearch" runat="server" Width="180px" 
                                        AutoPostBack="True" 
                                        onselectedindexchanged="ddlProvincesearch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td><td><asp:Label ID="lblSpace" runat="server" Width="50px"></asp:Label></td>
                                <td style="text-align: right;">
                                    <asp:Label ID="Label1" runat="server" Text=" District :"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDistricSearch" runat="server" Width="180px">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell4" style="width:50px;">
                                    <asp:Label ID="Label2" runat="server"  Width="5px"></asp:Label>
                                    <asp:ImageButton ID="imgbtnSearch" runat="server" Height="30px" 
                        ImageUrl="~/Images/Search.png" Width="30px" 
                                        OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
           
                <tr>
                    <td align="center">
                        <asp:GridView ID="grdLocation" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="grdLocation_PageIndexChanging"
                            OnRowDataBound="grdLocation_RowDataBound" OnSelectedIndexChanged="grdLocation_SelectedIndexChanged"
                            PageSize="8" Width="800px">
                            <Columns>
                                <asp:BoundField DataField="LOCATION_ID" HeaderText="Training Location ID" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField DataField="LOCATION_NAME" HeaderText="Training Location" />
                                <asp:BoundField DataField="ADDRESS" HeaderText="Address" />
                                <asp:BoundField DataField="CONTACT_NO_1" HeaderText="Primary Contact" />
                                <asp:BoundField DataField="EMAIL" HeaderText="E mail" />
                                <asp:BoundField DataField="CAPACITY" HeaderText="Location Capacity" />
                                <asp:BoundField DataField="BANK_NAME" HeaderText="Bank" />
                                <asp:BoundField DataField="BRANCH_NAME" HeaderText="Bank Branch" />
                                <asp:BoundField DataField="PROVINCE_NAME" HeaderText="Province" />
                                <asp:BoundField DataField="DISTRICT_NAME" HeaderText="District" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                            </Columns>
                            <EmptyDataTemplate>
                                NO LOCATION FOUND.
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <asp:HiddenField ID="hfglId" runat="server" ClientIDMode="Static" Value="" ViewStateMode="Enabled" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
