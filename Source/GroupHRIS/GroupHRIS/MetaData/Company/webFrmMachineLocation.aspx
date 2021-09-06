<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Mastermain.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="webFrmMachineLocation.aspx.cs" Inherits="GroupHRIS.MetaData.Company.webFrmMachineLocation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/StyleCommon.css" rel="stylesheet" type="text/css" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table class="styleMainTb">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">    </asp:ToolkitScriptManager>
        <tr>
            <td   colspan="2">
                <strong>Machine Location Details</strong></td>
        </tr>
        <tr>
            <td   colspan="2">
                <hr style="height: -12px"></td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                &nbsp;</td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                Company :</td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlCompCode" runat="server" Width="400px" 
                    AutoPostBack="True" onselectedindexchanged="ddlCompCode_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                Machine ID :</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtMachineID" runat="server" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfMachineID" runat="server" 
                    ControlToValidate="txtMachineID" ErrorMessage="Machine ID is required." 
                    ForeColor="Red" ValidationGroup="vgMachineLoc">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                Location&nbsp; :</td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlLocation" runat="server" Width="200px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfComCode" runat="server" 
                    ControlToValidate="ddlLocation" ErrorMessage="Location is required." 
                    ForeColor="Red" ValidationGroup="vgMachineLoc">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                Brand Name :</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtBrandName" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                Vendor :</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtVendor" runat="server" Width="400px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                Contact No :</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtContactNo" runat="server" Width="200px" MaxLength="10"></asp:TextBox>
                <asp:RegularExpressionValidator ID="reContactNo" runat="server" 
                    ControlToValidate="txtContactNo" 
                    ErrorMessage="Ten Characters required for Contact No." ForeColor="Red" 
                    ValidationExpression="^([\S\s]{10,10})$" ValidationGroup="vgMachineLoc">*</asp:RegularExpressionValidator>
                 <asp:FilteredTextBoxExtender ID = "fteContactNo" runat="server" TargetControlID="txtContactNo" FilterType="Numbers"></asp:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                IP Address :</td>
            <td class="styleMainTbRightTD">
                <asp:TextBox ID="txtIPAddress" runat="server" Width="200px"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="txtIPAddress" ErrorMessage="IP Address is Invalid." 
                    ForeColor="Red" 
                    ValidationExpression="^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$" 
                    ValidationGroup="vgMachineLoc">*</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                Status :</td>
            <td class="styleMainTbRightTD">
                <asp:DropDownList ID="ddlStatus" runat="server" Width="150px"   >
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class= "style5">
                </td>
            <td class="style6">
                &nbsp;</td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                <asp:Button ID="btnSave" runat="server" Text="Save" 
                    Width="100px" onclick="btnSave_Click" ValidationGroup="vgMachineLoc" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="100px" 
                    onclick="btnClear_Click" />
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                <asp:Label ID="lblerror" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                <asp:ValidationSummary ID="vsMachineLocation" runat="server" ForeColor="Red" 
                    ValidationGroup="vgMachineLoc" />
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD" colspan="2">
                <hr /></td>
        </tr>
        <tr>
            <td align="center" colspan="2" class="style7">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    onrowdatabound="GridView1_RowDataBound" 
                    onselectedindexchanged="GridView1_SelectedIndexChanged" AllowPaging="True" 
                    onpageindexchanging="GridView1_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="COMPANY_ID" HeaderText="Company ID" />
                        <asp:BoundField DataField="MACHINE_ID" HeaderText="Machine ID" />
                        <asp:BoundField DataField="LOCATION" HeaderText="Location" />
                        <asp:BoundField DataField="IP_ADDRESS" HeaderText="IP Address" />
                        <asp:BoundField DataField="STATUS_CODE" HeaderText="Status" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                &nbsp;</td>
        </tr>
        <tr>
            <td class= "styleMainTbLeftTD">
                &nbsp;</td>
            <td class="styleMainTbRightTD">
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
