<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation = "false" AutoEventWireup="true" CodeBehind="webFrmSalaryComponents.aspx.cs" Inherits="GroupHRIS.MetaData.webFrmSalaryComponents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" /> 
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="styleMainTb">
        <tr>
            <td colspan="2">
            <span style="font-weight: 700">Salary <strong>Component</strong> Details</span>
                &nbsp;</td>
        </tr>
        <tr>
            <td   colspan="2">
               <hr /></td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                &nbsp;</td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                Component Name : </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtComponentName" runat="server" Height="20px" Width="400px" 
                    MaxLength="45"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfCompName" runat="server" 
                    ControlToValidate="txtComponentName" ErrorMessage="Component Name is required." 
                    ForeColor="Red" ValidationGroup="vgSalaryComponent">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                Payroll Code : </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtpayrollcode" runat="server" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                Remarks : </td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtRemarks" runat="server" Height="20px" Width="400px" 
                    MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                Status : </td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlStatus" runat="server" Height="22px" Width="120px"   >
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfStatus" runat="server" 
                    ControlToValidate="ddlStatus" ErrorMessage="Status Code is required." 
                    ForeColor="Red" ValidationGroup="vgSalaryComponent">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                <asp:Button ID="btnSave" runat="server" Height="26px" Text="Save" 
                    Width="100px" ValidationGroup="vgSalaryComponent" 
                    onclick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" Height="26px" Text="Clear" 
                    Width="100px" onclick="btnClear_Click" />
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
            <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                <asp:ValidationSummary ID="vsSalaryComponent" runat="server" ForeColor="Red" 
                    ValidationGroup="vgSalaryComponent" />
            </td>
        </tr>
        <tr>
            <td  colspan="2">
                <hr /></td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                &nbsp;</td>
        </tr>
        <tr>
            <td align= "center"    colspan="2">
                <asp:GridView ID="GridView1" runat="server" 
                    onrowdatabound="GridView1_RowDataBound" 
                    onselectedindexchanged="GridView1_SelectedIndexChanged" AllowPaging="True" 
                    AutoGenerateColumns="False" 
                    onpageindexchanging="GridView1_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="COMPONENT_ID" HeaderText="Component ID" />
                        <asp:BoundField DataField="COMPONENT_NAME" HeaderText="Component Name" />
                        <asp:BoundField DataField="PAYROLL_CODE" HeaderText="Payroll Code" />
                        <asp:BoundField DataField="REMARKS" HeaderText="Remarks" />
                        <asp:BoundField DataField="STATUS_CODE" HeaderText="Status Code" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD" colspan="2">
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
