<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="webFrmCompany.aspx.cs" Inherits="GroupHRIS.MetaData.WebfrmCompany" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>        
            <table class="styleMainTb">
        <tr>
            <td colspan="2">
                <span><strong>Company Details</strong></span><strong> &nbsp;</strong>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
            </td>
            <td class="styleMainTbRightTD">
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Company :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtComName" runat="server" Width="400px" MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvComName" runat="server" ControlToValidate="txtComName"
                    ErrorMessage="Company name is required." ForeColor="Red" ValidationGroup="vgCompanyInformation">*</asp:RequiredFieldValidator>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Address :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtAddress1" runat="server" Width="400px" MaxLength="45"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress1"
                    ErrorMessage="Address is required." ForeColor="Red" ValidationGroup="vgCompanyInformation">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtAddress2" runat="server" Width="400px" MaxLength="45" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtAddress3" runat="server" Width="400px" MaxLength="45" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtAddress4" runat="server" Width="400px" MaxLength="45" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Land Phone :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtLandNo1" runat="server" Width="120px" MaxLength="10"></asp:TextBox>
                &nbsp;
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="^([\S\s]{10,10})$"
                    runat="server" ErrorMessage="Ten charactors required for Land Phone Number."
                    ForeColor="Red" ValidationGroup="vgCompanyInformation" ControlToValidate="txtLandNo1">*</asp:RegularExpressionValidator>
                <asp:FilteredTextBoxExtender ID="fteLandPhone" runat="server" TargetControlID="txtLandNo1"
                    FilterType="Numbers">
                </asp:FilteredTextBoxExtender>
                <asp:RequiredFieldValidator ID="rfLandPhone" runat="server" ControlToValidate="txtLandNo1"
                    ErrorMessage="Land Phone Number is required." ForeColor="Red" ValidationGroup="vgCompanyInformation">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Land Phone 2 :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtLandNo2" runat="server" MaxLength="10" onkeydown="return (event.keyCode!=13);"
                    Width="120px"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="fteLandPhone2" runat="server" TargetControlID="txtLandNo2"
                    FilterType="Numbers">
                </asp:FilteredTextBoxExtender>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ValidationExpression="^([\S\s]{10,10})$"
                    runat="server" ErrorMessage="Ten charactors required for Land Phone Number 2."
                    ForeColor="Red" ValidationGroup="vgCompanyInformation" ControlToValidate="txtLandNo2">*</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Email Address :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtEmail" runat="server" Width="400px" MaxLength="50"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Email Address is invalid."
                    ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    ForeColor="Red" ValidationGroup="vgCompanyInformation">*</asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="Email Address is required." ForeColor="Red" ValidationGroup="vgCompanyInformation">*</asp:RequiredFieldValidator>
                
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Status :
            </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlStsCode" runat="server" Height="20px" Width="120px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfStatus" runat="server" ControlToValidate="ddlStsCode"
                    ErrorMessage="Status Code is required." ForeColor="Red" ValidationGroup="vgCompanyInformation">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Fax No :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtFaxNo" runat="server" MaxLength="10" Width="120px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                <asp:RegularExpressionValidator ID="reFaxNo" runat="server" ControlToValidate="txtFaxNo"
                    ErrorMessage="Ten Characters required for Fax Number." ForeColor="Red" ValidationExpression="^([\S\s]{10,10})$"
                    ValidationGroup="vgCompanyInformation">*</asp:RegularExpressionValidator>
                <asp:FilteredTextBoxExtender ID="fteFaxNo" runat="server" TargetControlID="txtFaxNo"
                    FilterType="Numbers">
                </asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Work Hour Start :
            </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlStartHour" runat="server" Height="20px" Width="45px">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlStartMinute" runat="server" Height="20px" Width="45px">
                </asp:DropDownList>
                (HH:MM)
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Work Hour End :
            </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlEndHour" runat="server" Height="20px" Width="45px">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlEndMinute" runat="server" Height="20px" Width="45px">
                </asp:DropDownList>
                <asp:CompareValidator ID="cvHourEnd" runat="server" ControlToCompare="ddlStartHour"
                    ControlToValidate="ddlEndHour" ErrorMessage="End Hour is Less than Start Hour"
                    ForeColor="Red" ValidationGroup="vgCompanyInformation" Operator="GreaterThan">*</asp:CompareValidator>
                (HH:MM)
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                <asp:Label ID="Label1" runat="server" Text="Company SAP ID :"></asp:Label>
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtSAPId" runat="server" MaxLength="5" Width="120px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtSAPId"
                    ErrorMessage="Company SAP ID is required." ForeColor="Red" ValidationGroup="vgCompanyInformation">*</asp:RequiredFieldValidator>
                <asp:FilteredTextBoxExtender ID="fteSAPId" runat="server" TargetControlID="txtSAPId"
                    FilterType="Numbers">
                </asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Employer EPF :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtEPFNo" runat="server" MaxLength="10" Width="120px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtEPFNo"
                    ErrorMessage="Employer EPF is required." ForeColor="Red" ValidationGroup="vgCompanyInformation">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Business Type :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtBusinessType" runat="server" MaxLength="100" Width="400px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                HR Email :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txthremail" runat="server" Width="400px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txthremail"
                    ErrorMessage="HR Email is required." ForeColor="Red" ValidationGroup="vgCompanyInformation">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="HR Email Address is invalid."
                    ControlToValidate="txthremail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    ForeColor="Red" ValidationGroup="vgCompanyInformation">*</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Vission :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtVission" runat="server" Width="525px" MaxLength="200" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Mission :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtMission" runat="server" Width="525px" MaxLength="200" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                Motto :
            </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtMotto" runat="server" Width="525px" MaxLength="250" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <%--<asp:CheckBox ID="chkSaturday" AutoPostBack="true" Text="is Saturday Working Company"
                    runat="server" OnCheckedChanged="chkSaturday_CheckedChanged" />--%>
            </td>
        </tr>
        <tr>
            <td style="text-align: right;">
                <asp:Label ID="lblSaturdayWorkStart" runat="server" Text="Saturday Work Hour Start : "></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlSStartHour" runat="server">
                </asp:DropDownList>
                &nbsp;<asp:Label ID="ddlSStartSC" runat="server" Text=":"></asp:Label>
                &nbsp;<asp:DropDownList ID="ddlSStartMinute" runat="server">
                </asp:DropDownList>
                &nbsp;<asp:Label ID="lblSStart" runat="server" Text="(HH : MM)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: right;">
                <asp:Label ID="lblSaturdayWorkEnd" runat="server" Text="Saturday Work Hour End : "></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlSEndHour" runat="server">
                </asp:DropDownList>
                &nbsp;<asp:Label ID="ddlSEndSC" runat="server" Text=":"></asp:Label>
                &nbsp;<asp:DropDownList ID="ddlSEndMinute" runat="server">
                </asp:DropDownList>
                &nbsp;<asp:Label ID="lblSEnd" runat="server" Text="(HH : MM)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;
            </td>
            <td class="styleMainTbRightTD">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" Width="100px"
                    ValidationGroup="vgCompanyInformation" />
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Clear"
                    Width="100px" />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;
            </td>
            <td class="styleMainTbRightTD">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="../../Images/ProBar/720.GIF" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;
            </td>
            <td class="styleMainTbRightTD">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgCompanyInformation" ForeColor="Red" />
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;
            </td>
            <td class="styleMainTbRightTD">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center" class="style4">
                <div class="stylediv">
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                        OnRowDataBound="GridView1_RowDataBound" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundField DataField="COMPANY_ID" HeaderText="Company ID" />
                            <asp:BoundField DataField="COMP_NAME" HeaderText="Company Name" />
                            <asp:BoundField DataField="LAND_PHONE1" HeaderText="Land Phone No" />
                            <asp:BoundField DataField="EMPLOYER_EPF" HeaderText="Employer EPF" />
                            <asp:BoundField DataField="COMP_SAP_ID" HeaderText="SAP ID" />
                            <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
        <tr>
            <td class="styleMainTbLeftTD">
                &nbsp;
            </td>
            <td class="styleMainTbRightTD">
                &nbsp;
            </td>
        </tr>
    </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>