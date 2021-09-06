<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="WebFrmTrainingInstitutes.aspx.cs" Inherits="GroupHRIS.TrainingAndDevelopment.WebFrmTrainingInstitutes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/styleEmployeeLeaveSchedule.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideGridColumn
        {
            display: none;
        }
        .style1
        {
            width: 292px;
        }
        
        input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <span>Training Institute Details</span>
    <hr />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                        <table style="max-width: 550px;">
                            <%--Name--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label1" runat="server" Text="Name :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtName" runat="server" Width="250px" MaxLength="150"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftName" runat="server" FilterType="LowercaseLetters, UppercaseLetters, Custom"
                                        ValidChars=" " TargetControlID="txtName">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                        ErrorMessage="Name is required " ForeColor="Red" ValidationGroup="vgInstitute">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Address--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label2" runat="server" Text="Address :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtAddress" runat="server" Width="250px" MaxLength="300" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress"
                                        ErrorMessage="Address is required " ForeColor="Red" ValidationGroup="vgInstitute">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Contact No 1--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label3" runat="server" Text="Primary Contact No :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtContact_1" runat="server" Width="250px" MaxLength="10"></asp:TextBox>
                                    <%--pattern=".{0}|.{10,}" oninvalid="setCustomValidity('should contain 10 numbers')"--%>
                                </td>
                                <td class="styleTableCell3">
                                    <asp:FilteredTextBoxExtender ID="fteContact1" runat="server" FilterType="Numbers"
                                        ValidChars="+" TargetControlID="txtContact_1">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvContact1" runat="server" ControlToValidate="txtContact_1"
                                        ErrorMessage="Primary Contact No is required " ForeColor="Red" ValidationGroup="vgInstitute">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                    <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="txtContact_1"
                                        ValidationExpression="^[0-9]{10}$" ValidationGroup="vgInstitute" ForeColor="Red"
                                        ErrorMessage="Invalid Primary Contact No">*</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <%--Contact No 2 --%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label4" runat="server" Text="Secondary Contact No :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtContact_2" runat="server" Width="250px" MaxLength="10"></asp:TextBox>
                                    <%--pattern=".{0}|.{10,}" oninvalid="setCustomValidity('should contain 10 numbers')"--%>
                                </td>
                                <td class="styleTableCell3">
                                    <asp:FilteredTextBoxExtender ID="fteContact2" runat="server" FilterType="Numbers"
                                        ValidChars="" TargetControlID="txtContact_2">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RegularExpressionValidator runat="server" ID="rexNumber" ControlToValidate="txtContact_2"
                                        ValidationExpression="^[0-9]{10}$" ValidationGroup="vgInstitute" ForeColor="Red"
                                        ErrorMessage="Invalid Secondary Contact No">*</asp:RegularExpressionValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Email--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label5" runat="server" Text="Email :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtEmail" runat="server" Width="250px" MaxLength="150"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                        ErrorMessage="Email is required " ForeColor="Red" ValidationGroup="vgInstitute">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Invalid e-mail address"
                                        ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="vgInstitute" ForeColor="Red">*</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <%--Bank--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label6" runat="server" Text="Bank :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlBank" runat="server" Width="250px" OnSelectedIndexChanged="ddlBank_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvBank" runat="server" ControlToValidate="ddlBank"
                                        ErrorMessage="Bank is required " ForeColor="Red" ValidationGroup="vgInstitute">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Branch--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label7" runat="server" Text="Branch :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlBranch" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlBranch"
                                        ErrorMessage="Branch is required " ForeColor="Red" ValidationGroup="vgInstitute">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Account No --%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label8" runat="server" Text="Account No :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtAccount" runat="server" MaxLength="100" Width="250px"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers,Custom"
                                        ValidChars="-/" TargetControlID="txtAccount"></asp:FilteredTextBoxExtender>
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvAccount" runat="server" ControlToValidate="txtAccount"
                                        ErrorMessage="Account no. is required " ForeColor="Red" ValidationGroup="vgInstitute">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Payment Instructions--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label9" runat="server" Text="Payment Instructions :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:TextBox ID="txtPaymentInstruction" runat="server" Width="250px" MaxLength="500"
                                        TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPaymentInstruction"
                                        ErrorMessage="Payment Instructions are required " ForeColor="Red" ValidationGroup="vgInstitute">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Status--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                    <asp:Label ID="Label10" runat="server" Text="Status :"></asp:Label>
                                </td>
                                <td class="styleTableCell2">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                    <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus"
                                        ErrorMessage="Status is required " ForeColor="Red" ValidationGroup="vgInstitute">*</asp:RequiredFieldValidator>
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Buttons--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                </td>
                                <td class="styleTableCell2" align="left">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="125px" ValidationGroup="vgInstitute"
                                        Style="height: 26px; float: left;" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" Width="125px" Style="height: 26px;
                                        float: left; margin-left: 0;" OnClick="btnClear_Click" />
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <%--Validation--%>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1">
                                </td>
                                <td class="styleTableCell2" align="left">
                                    <asp:Label ID="lblErrorMsg" runat="server" Style="float: left"></asp:Label>
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgInstitute"
                                        ForeColor="Red" Style="text-align: left;" />
                                </td>
                                <td class="styleTableCell3">
                                </td>
                                <td class="styleTableCell4">
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                            <tr class="styleTableRow">
                                <td class="styleTableCell1" align="center">
                                    
                                </td>
                                <td class="styleTableCell2" align="center">
                                <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                        <ProgressTemplate>
                                            <img src="../Images/ProBar/720.GIF" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>--%>
                                    <asp:HiddenField ID="hfInstituteId" runat="server" ClientIDMode="Static" />
                                </td>
                                <td class="styleTableCell3">
                                
                                </td>
                                <td class="styleTableCell4">
                                </td>
                                <td class="styleTableCell5">
                                </td>
                            </tr>
                        </table>
                    <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </td>
            <%--background-color:#aed6f1;--%>
            <td class="style1" style="background-color: #aed6f1; min-width: 300px;" align="center">
                <table style="max-width: 300px; background-color: #f2f4f4; margin: 5px 5px 5px 5px;
                    padding: 10px 10px 10px 10px;" id="tblSelectedInstitute" runat="server">
                    <tr>
                        <td colspan="2">
                            Add programs to the institute
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label11" runat="server" Text="Institute Id : "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInstituteId" runat="server" ReadOnly="true" Width="143px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label12" runat="server" Text="Institute Name : "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInstituteName" runat="server" TextMode="MultiLine" Width="143px"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <br />
                            <asp:Button ID="btnAddPrograms" runat="server" Text="Add" Width="73px" OnClick="btnAddPrograms_Click" />
                            <asp:Button ID="btnInstituteClear" runat="server" Text="Clear" Width="73px" OnClick="btnInstituteClear_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <%--Institute Grid --%>
    <table width="100%">
        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
                <tr>
                    <td align="center">
                        <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span>Training Institutes</span>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:GridView ID="gvInstitutes" runat="server" AutoGenerateColumns="false" Style="width: 810px;"
                            OnRowDataBound="gvInstitutes_RowDataBound" OnSelectedIndexChanged="gvInstitutes_SelectedIndexChanged"
                            OnPageIndexChanging="gvInstitutes_PageIndexChanging" AllowPaging="true" PageSize="10">
                            <Columns>
                                <asp:BoundField HeaderText="Institute Id" DataField="INSTITUTE_ID" HeaderStyle-CssClass="hideGridColumn"
                                    ItemStyle-CssClass="hideGridColumn" />
                                <asp:BoundField HeaderText="Institute Name" DataField="INSTITUTE_NAME" />
                                <asp:BoundField DataField="CONTACT_NO_1" HeaderText="Contact No." />
                                <asp:BoundField DataField="EMAIL_ADDRESS" HeaderText="Email" />
                                <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                            <ProgressTemplate>
                                <img src="../Images/ProBar/720.GIF" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>--%>
                    </td>
                </tr>
           <%-- </ContentTemplate>
             <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
             </Triggers>
        </asp:UpdatePanel>--%>
    </table>
    
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
